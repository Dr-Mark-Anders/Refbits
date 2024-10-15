using ModelEngine;
using ModelEngine;
using ModelEngineTest;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Units.UOM;

namespace RusselColumnTest
{
    /* Parallel.For(0, Stages[section], tray =>  // slower
    {
        liqenth[section][tray] = EstimateLiqEnth(TrayTK[section][tray], TrayLiqComposition[section][tray], section, tray, ref cres);
        vapenth[section][tray] = EstimateVapEnth(TrayTK[section][tray], TrayVapComposition[section][tray], section, tray, ref cres);
    });*/

    public partial class RusselSolverTest
    {
        public bool EnthalpiesUpdate(ref enumCalcResult cres)
        {
            TraySectionTest section;
            TrayTest     tray;

            
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
                            tray = section.Trays[trayNo];
                            tray.vapenth = tray.VapEnthalpy(column, tray.TPredicted);
                            tray.liqenth = tray.LiqEnthalpy(column, tray.TPredicted);
                        }
                    //});
                }
            }
            
            foreach (ConnectingStreamTest stream in column.LiquidStreams)  // do liquid additions // TrDMatrix[Comp][Down][Across]
            {
                TraySectionTest  dcts_Draw = stream.EngineDrawSection;
                TraySectionTest dcts_Return = stream.EngineReturnSection;

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

            foreach (ConnectingStreamTest st in column.VapourStreams)  // do vapour additions
            {
                TraySectionTest dcts_Draw = st.EngineDrawSection;
                TraySectionTest dcts_Return = st.EngineReturnSection;

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
                TraySectionTest traySection = column.TraySections.traysections[i];

                int TrayNo = traySection.Trays.Count;
                for (int i1 = 0; i1 < TrayNo; i1++)
                {
                    TrayTest tray = traySection.Trays[i1];
                    if (IsBaseErrors)
                        tray.baseTrayError = tray.enthalpyBalance;
                    else
                    {
                        tray.trayError = tray.enthalpyBalance - tray.baseTrayError;
                        //Debug.Print (tray.trayError.ToString());
                    }
                }
            }

            foreach (SpecificationTest spec in column.Specs)
            {
                Guid SecGuid = spec.engineSectionGuid;
                Guid TrayGuid = spec.engineStageGuid;
                Guid paguid = spec.PAguid;

                TraySectionTest traysection = spec.traysection;
                //TraySection traysection = column.TraySections[SecGuid];
                if (traysection is null)
                    continue;

                TrayTest tray = spec.tray;// traysection[TrayGuid];
                //Tray tray = traysection[TrayGuid];

                PumpAroundTest pa = column.PumpArounds[paguid];
                ConnectingStream stream;

                double MW = double.NaN;
                double SG = double.NaN;

                switch (spec.engineSpecType)
                {
                    case COMeSpecType.TrayDuty:
                        if (IsBaseErrors)
                        {
                            spec.baseerror = tray.enthalpyBalance;
                            spec.Value = tray.enthalpyBalance;
                        }
                        else
                            spec.error = tray.enthalpyBalance - spec.baseerror;
                        break;

                    case COMeSpecType.TrayNetLiqFlow:
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

                    case COMeSpecType.TrayNetVapFlow:
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

                    case COMeSpecType.Temperature:
                        if (IsBaseErrors)
                        {
                            spec.baseerror = spec.SpecValue - tray.TPredicted;
                            spec.Value = tray.TPredicted;
                        }
                        else
                            spec.error = spec.SpecValue - tray.TPredicted - spec.baseerror;
                        break;

                    case COMeSpecType.RefluxRatio:
                        if (IsBaseErrors)
                        {
                            spec.baseerror = spec.SpecValue - traysection.RefluxRatio;
                            spec.Value = traysection.RefluxRatio;
                        }
                        else
                            spec.error = spec.SpecValue - traysection.RefluxRatio - spec.baseerror;  // overwrite top stage heat spec
                        break;

                    case COMeSpecType.Energy:
                        break;

                    case COMeSpecType.PAFlow:
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

                    case COMeSpecType.PARetT:
                        spec.Value = spec.SpecValue;
                        break;

                    case COMeSpecType.PADeltaT:
                        spec.Value = spec.SpecValue;
                        break;

                    case COMeSpecType.PADuty:
                        break;

                    case COMeSpecType.VapProductDraw:
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

                    case COMeSpecType.LiquidProductDraw:
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

                    case COMeSpecType.VapStream:
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

                    case COMeSpecType.LiquidStream:
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

                    case COMeSpecType.DistSpec:
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