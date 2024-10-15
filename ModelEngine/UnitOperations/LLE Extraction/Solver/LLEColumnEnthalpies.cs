using ModelEngine;
using ModelEngine;
using System;

namespace LLE_Solver
{
    /* Parallel.For(0, Stages[section], tray =>  // slower
    {
        liqenth[section][tray] = EstimateLiqEnth(TrayTK[section][tray], TrayLiqComposition[section][tray], section, tray, ref cres);
        vapenth[section][tray] = EstimateVapEnth(TrayTK[section][tray], TrayVapComposition[section][tray], section, tray, ref cres);
    });*/

    public partial class LLESolver
    {
        public bool EnthalpiesUpdate(ref enumCalcResult cres)
        {
            TraySection section;
            Tray tray;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                section = column.TraySections[sectionNo];
                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++)
                {
                    tray = section.Trays[trayNo];
                    tray.vapenth = tray.LightPhaseEnthalpy(tray.T);
                    tray.liqenth = tray.HeavyPhaseEnthalpy(tray.T);
                }
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

            /* foreach (TraySection traySection in column.TraySections) //Do Tray Enthalpy balances
             {
                 foreach (Tray tray in traySection)
                 {
                     if (IsBaseErrors)
                         tray.baseTrayError = tray.enthalpyBalance;
                     else
                     {
                         tray.trayError = tray.enthalpyBalance - tray.baseTrayError;
                         //Debug.Print (tray.trayError.ToString());
                     }
                 }
             }*/

            foreach (TraySection traySection in column.TraySections) //Do Tray Enthalpy balances
            {
                foreach (Tray tray in traySection)
                {
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
                TraySection traysection = column.TraySections[SecGuid];
                if (traysection is null)
                    continue;
                Tray tray = traysection[TrayGuid];
                PumpAround pa = column.PumpArounds[paguid];
                ConnectingStream stream;

                double MW = double.NaN;
                double SG = double.NaN;

                switch (spec.engineSpecType)
                {
                    case eSpecType.LLE_KSpec:
                        if (tray.LLE_K is null)
                            return false;

                        if (IsBaseErrors)
                        {
                            spec.baseerror = 0;
                            spec.value = 0;
                            for (int i = 0; i < NoComps; i++)
                            {
                                spec.value += -tray.VapComposition[i] / tray.LiqComposition[i];
                                spec.baseerror += -(tray.VapComposition[i] / tray.LiqComposition[i] - tray.LLE_K[i]);
                            }
                        }
                        else
                        {
                            spec.error = 0;
                            spec.value = 0;
                            for (int i = 0; i < NoComps; i++)
                            {
                                spec.value += -(tray.VapComposition[i] / tray.LiqComposition[i]);
                                spec.error += -(tray.VapComposition[i] / tray.LiqComposition[i] - tray.LLE_K[i]);
                            }
                            spec.error += -spec.baseerror;
                        }
                        break;

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
                        MW = column.Components.MW(tray.LiqComposition);
                        SG = column.Components.SG(tray.LiqComposition);

                        if (IsBaseErrors)
                        {
                            spec.baseerror = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.L;
                            spec.Value = tray.LiqFlow(spec.FlowType, MW, SG) / column.FeedRatio;
                        }
                        else
                            spec.error = spec.SpecValueMoles(MW, SG) * column.FeedRatio - tray.L - spec.baseerror;
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