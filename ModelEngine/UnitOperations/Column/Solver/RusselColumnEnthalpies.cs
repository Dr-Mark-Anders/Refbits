using ModelEngine;
using ModelEngine;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Units.UOM;

namespace RusselColumn
{
    /* Parallel.For(0, Stages[section], tray =>  // slower
    {
        liqenth[section][tray] = EstimateLiqEnth(TrayTK[section][tray], TrayLiqComposition[section][tray], section, tray, ref cres);
        vapenth[section][tray] = EstimateVapEnth(TrayTK[section][tray], TrayVapComposition[section][tray], section, tray, ref cres);
    });*/

    public partial class RusselSolver
    {
        public bool EnthalpiesUpdate(ref enumCalcResult cres)
        {
            TraySection section;
            Tray tray;

            
            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                section = column.TraySections[sectionNo];
                if (section is not null)
                {
                    int count = section.Trays.Count;
                    //Parallel.For(0, count, trayNo =>
                   /// {
                        for (int trayNo = 0; trayNo < count; trayNo++)
                        {
                            if(trayNo>section.Trays.Count)
                                Debugger.Break();

                            tray = section.Trays[trayNo];

                            tray.vapenth = tray.VapEnthalpy(column, tray.TPredicted);
                            tray.liqenth = tray.LiqEnthalpy(column, tray.TPredicted);
                        }
                    //});
                }
            }
            
            foreach (ConnectingStream stream in column.LiquidStreams)  // do liquid additions // TrDMatrix[Comp][Down][Across]
            {
                TraySection dcts_Draw = stream.EngineDrawSection;
                TraySection dcts_Return = stream.EngineReturnSection;

                if (stream.engineDrawTray is null || stream.engineReturnTray is null)
                    return false;

                int ReturnSection = column.TraySections.IndexOf(dcts_Return);
                int ReturnTrayIndex = dcts_Return.Trays.IndexOf(stream.engineReturnTray);
                int DrawTrayIndex = dcts_Draw.Trays.IndexOf(stream.engineDrawTray);
                if (DrawTrayIndex < 0)
                    return false;
                if (ReturnSection >= 0 && ReturnTrayIndex >= 0)
                    StripperLiqFeedEnthalpies[ReturnSection][ReturnTrayIndex]
                        = stream.FlowEstimate * dcts_Draw.Trays[DrawTrayIndex].liqenth;
            }

            foreach (ConnectingStream st in column.VapourStreams)  // do vapour additions
            {
                TraySection dcts_Draw = st.EngineDrawSection;
                TraySection dcts_Return = st.EngineReturnSection;

                int ReturnSection = column.TraySections.IndexOf(dcts_Return);
                int ReturnTrayIndex = dcts_Return.Trays.IndexOf(st.engineReturnTray);
                int DrawTrayIndex = dcts_Draw.Trays.IndexOf(st.engineDrawTray);

                StripperVapReturnTotEnthalpies[ReturnSection][ReturnTrayIndex]
                    = st.FlowEstimate * dcts_Draw.Trays[DrawTrayIndex].liqenth;
            }
            return true;
        }

        public bool CalcErrors(bool IsBaseErrors)
        {
            // Try to keep logical order, although order is not importnt for calculations
            // Stage errors (or alternative for trays not heatbalanced)
            // Conndenser Liquid Draw
            // Side Liquid Draws
            // Reflux Ratio
            // Sidestreams, All
            int No = column.TraySections.traysections.Count;

            for (int i = 0; i < No; i++) //Do Tray Enthalpy balances
            {
                TraySection traySection = column.TraySections.traysections[i];

                int TrayNo = traySection.Trays.Count;
                for (int i1 = 0; i1 < TrayNo; i1++)
                {
                    Tray tray = traySection.Trays[i1];
                    if (IsBaseErrors)
                        tray.baseTrayError = tray.enthalpyBalance;
                    else
                    {
                        tray.trayError = tray.enthalpyBalance - tray.baseTrayError;
                        //Debug.Print (tray.trayError.ToString());
                    }
                }
            }

            foreach (Specification spec in column.Specs)
            {
                Guid SecGuid = spec.engineSectionGuid;
                Guid TrayGuid = spec.engineStageGuid;
                Guid paguid = spec.PAguid;

                TraySection traysection = spec.traysection;
                //TraySection traysection = column.TraySections[SecGuid];
                if (traysection is null)
                    continue;

                Tray tray = spec.tray;// traysection[TrayGuid];
                //Tray tray = traysection[TrayGuid];

                PumpAround pa = column.PumpArounds[paguid];
                ConnectingStream stream;

                double MW = double.NaN;
                double SG = double.NaN;

                switch (spec.engineSpecType)
                {
                    case eSpecType.TrayDuty:
                        if (IsBaseErrors)
                        {
                            spec.baseerror = tray.enthalpyBalance;
                            spec.Value = tray.enthalpyBalance;
                        }
                        else
                            spec.error = tray.enthalpyBalance - spec.baseerror;
                        break;

                    case eSpecType.TrayNetLiqFlow:
                        if (tray is not null)
                        {
                            MW = column.Components.MW(tray.LiqComposition);
                            SG = column.Components.SG(tray.LiqComposition);

                            if (IsBaseErrors)
                            {
                                spec.baseerror = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.L;
                                spec.Value = tray.LiqFlow(spec.FlowType, MW, SG) / column.FeedRatio;
                            }
                            else
                                spec.error = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.L - spec.baseerror;
                        }
                        break;

                    case eSpecType.TrayNetVapFlow:
                        if (tray != null)
                        {
                            MW = column.Components.MW(tray.VapComposition);
                            SG = column.Components.SG(tray.VapComposition);

                            if (IsBaseErrors)
                            {
                                spec.baseerror = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.V;
                                spec.Value = tray.VapFlow(spec.FlowType, MW, SG) / column.FeedRatio;
                            }
                            else
                                spec.error = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.V - spec.baseerror; // fixed Vapour rate on any stage
                        }
                        break;

                    case eSpecType.Temperature:
                        if (IsBaseErrors)
                        {
                            spec.baseerror = spec.SpecValue - tray.TPredicted;
                            spec.Value = tray.TPredicted;
                        }
                        else
                            spec.error = spec.SpecValue - tray.TPredicted - spec.baseerror;
                        break;

                    case eSpecType.RefluxRatio:
                        if (IsBaseErrors)
                        {
                            spec.baseerror = spec.SpecValue - traysection.RefluxRatio;
                            spec.Value = traysection.RefluxRatio;
                        }
                        else
                            spec.error = spec.SpecValue - traysection.RefluxRatio - spec.baseerror;  // overwrite top stage heat spec
                        break;

                    case eSpecType.Energy:
                        break;

                    case eSpecType.PAFlow:
                        if (pa != null)
                        {
                            MW = column.Components.MW(pa.drawTray.LiqComposition);
                            SG = column.Components.SG(pa.drawTray.LiqComposition);

                            if (IsBaseErrors)
                            {
                                spec.baseerror = spec.SpecValueMoles(MW, SG) * column.FeedRatio - pa.MoleFlow;
                                spec.Value = pa.FlowEstimate(spec.FlowType, MW, SG) / column.FeedRatio;
                            }
                            else
                                spec.error = spec.SpecValueMoles(MW, SG) * column.FeedRatio - pa.MoleFlow - spec.baseerror;
                        }
                        break;

                    case eSpecType.PARetT:
                        spec.Value = spec.SpecValue;
                        break;

                    case eSpecType.PADeltaT:
                        spec.Value = spec.SpecValue;
                        break;

                    case eSpecType.PADuty:
                        break;

                    case eSpecType.VapProductDraw:
                        MW = column.Components.MW(tray.VapComposition);
                        SG = column.Components.SG(tray.VapComposition);

                        if (IsBaseErrors)
                        {
                            spec.baseerror = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.vss_estimate;
                            spec.Value = tray.VssFlow(spec.FlowType, MW, SG) * column.FeedRatio;
                        }
                        else
                            spec.error = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.vss_estimate - spec.baseerror;  // side draw or top product
                        break;

                    case eSpecType.LiquidProductDraw:
                        MW = column.Components.MW(tray.LiqComposition);
                        SG = column.Components.SG(tray.LiqComposition);

                        if (tray != null)
                        {
                            if (IsBaseErrors)
                            {
                                spec.baseerror = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.lss_estimate;
                                spec.Value = tray.lssFlow(spec.FlowType, MW, SG) / column.FeedRatio;
                            }
                            else
                                spec.error = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.lss_estimate - spec.baseerror;  // side draw or top product
                        }
                        break;

                    case eSpecType.VapStream:
                        stream = spec.connectedStream;

                        MW = column.Components.MW(stream.engineDrawTray.VapComposition);
                        SG = column.Components.SG(stream.engineDrawTray.VapComposition);

                        if (IsBaseErrors)
                        {
                            spec.baseerror = spec.SpecValueMoles(MW, SG) * column.FeedRatio - stream.FlowEstimate;
                            spec.Value = stream.FlowEstimate * column.FeedRatio;
                        }
                        else
                            spec.error = spec.SpecValueMoles(MW, SG) * column.FeedRatio - stream.FlowEstimate - spec.baseerror;  // side draw or top product
                        break;

                    case eSpecType.LiquidStream:
                        stream = spec.connectedStream;

                        MW = column.Components.MW(stream.engineDrawTray.LiqComposition);
                        SG = column.Components.SG(stream.engineDrawTray.LiqComposition);

                        if (stream != null)
                        {
                            if (IsBaseErrors)
                            {
                                spec.baseerror = spec.SpecValueMoles(MW, SG) * column.FeedRatio - stream.FlowEstimate;
                                spec.Value = stream.FlowEstimate * column.FeedRatio;
                            }
                            else
                                spec.error = spec.SpecValueMoles(MW, SG) * column.FeedRatio - stream.FlowEstimate - spec.baseerror;  // side draw or top product
                        }
                        break;

                    case eSpecType.DistSpec:
                        if (IsBaseErrors)
                        {
                            spec.baseerror = spec.SpecValue - Components.StreamDistillation(column.Components, traysection[spec.engineStageGuid].LiqCompositionPred, spec.distType, spec.distpoint);
                            spec.Value = Components.StreamDistillation(column.Components, traysection[spec.engineStageGuid].LiqCompositionPred, spec.distType, spec.distpoint);
                        }
                        else
                        {
                            spec.error = spec.SpecValue - Components.StreamDistillation(column.Components, traysection[spec.engineStageGuid].LiqCompositionPred, spec.distType, spec.distpoint);// - spec.baseerror;  // side draw or top product
                            spec.delta = spec.error - spec.baseerror;
                        }
                        break;

                    default:
                        break;
                }
                //Debug.Print (spec.SpecName + " " +  spec.engineSpecType + " "+ spec.error.ToString() + " " +spec.baseerror.ToString());
            }
            return true;
        }
    }
}