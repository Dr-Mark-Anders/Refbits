using System;
using MathNet.Numerics.LinearAlgebra;
using Extensions;
using System.Runtime.Serialization;
using System.Runtime;
using System.ComponentModel;
using System.Diagnostics;
using EngineThermo;

/*Cases
2 Fixed Temps, Fix UA
3 Fixed Temps, Calc UA
2 Fixed Inlet Temps, Fix Design
3 Fixed Temps, Calc Design
4 Fixed Temps, Calc Fouling Factor*/

public enum HeExCalcOptions { TwoFixInletTempsFixUA, ThreeFixedTempsCalcUA, TwoFixInletsFixedDesign, ThreeFixedTempsCalcDesign, ThreeFixTempsCalcFoulingFactor, ByPass, Off, DeltaTs };
public enum ExchangerHeatBalanceTypeShellTube { ShellSide, ShellSideTSInlet, ShellSideTSOutlet, TubeSide, TubeSideSSInlet, TubeSideSSOutlet, None, TwoInlets, TwoOutlets, SSInlet, TSInlet };// These two must correspond
public enum ExchangerHeatBalanceTypeHotCold { HotSide, HotSideCSInlet, HSSideCSOutlet, ColdSide, ColdSideHSInlet, ColdSideHSOutlet, None, TwoInlets, TwoOutlets, SSInlet, TSInlet };  // These Two must correpsond
public enum ExchangerMassBalanceType { TSIn, TSOut, SSIn, SSOut, SSint SIn, SSint SOut, SSOutTSin, SSOutTSOut, None, Overdefined };
public enum ExchangerComponentBalanceType { TSIn, TSOut, SSIn, SSOut, SSint SIn, SSint SOut, SSOutTSin, SSOutTSOut, None, Overdefined };
public enum TubePattern { Triangular, Square, RotatedSquare }

/*
1. Nodes must be int iialised in OnDeserialise method and object creation method.
2.
*/

namespace Units
{
    [Serializable]
    public class Exchanger : ISerializable
    {
        public ExchangerProps eProps = new ExchangerProps();

        public HeExCalcOptions option;
        ExchangerHeatBalanceTypeShellTube HeatBalanceType = ExchangerHeatBalanceTypeShellTube.None;
        ExchangerMassBalanceType MassBalanceType = ExchangerMassBalanceType.None;
        public Guid EXguid;

        Oil HSIn, HSOut, CSIn, CSOut;
        Oil SSIn, SSOut, TSIn, TSOut;

        public ExchangerProps Mech { get => eProps; set => eProps = value; }

        public ExchangerHeatBalanceTypeHotCold ExType
        {
            get
            {
                if ((int)HeatBalanceType > 5)
                    return (ExchangerHeatBalanceTypeHotCold)HeatBalanceType;

                if (eProps.ShellIsHotSide == true)
                {
                    return (ExchangerHeatBalanceTypeHotCold)HeatBalanceType;
                }
                else if (eProps.ShellIsHotSide == false)
                {
                    if ((int)HeatBalanceType < 3) // shell side = hot side
                    {
                        return (ExchangerHeatBalanceTypeHotCold)((int)HeatBalanceType);
                    }
                    else
                    {
                        return (ExchangerHeatBalanceTypeHotCold)((int)HeatBalanceType - 3);
                    }
                }
                else
                {
                    return ExchangerHeatBalanceTypeHotCold.None;
                }
            }
        }

        public Exchanger(Oil SSstreamIn, Oil SSstreamOut, Oil TSstreamIn, Oil TSstreamOut, Guid ExchangerID)
        {
            this.EXguid = ExchangerID;

            SSIn = SSstreamIn;
            SSOut = SSstreamOut;
            TSIn = TSstreamIn;
            TSOut = TSstreamOut;

            DoHeatBalanceType();
            DoMassBalanceType();
        }

        public void SolveTraditionalMethod()
        {
            DoMassBalanceType();
            DoMassBalance();
            DoHeatBalanceType();
            DoHeatBalance();

            switch (option)
            {
                case HeExCalcOptions.TwoFixInletTempsFixUA:
                    {
                        switch (HeatBalanceType)
                        {
                            case ExchangerHeatBalanceTypeShellTube.None:
                            case ExchangerHeatBalanceTypeShellTube.ShellSide:
                            case ExchangerHeatBalanceTypeShellTube.TubeSide:
                            case ExchangerHeatBalanceTypeShellTube.TwoOutlets:
                            case ExchangerHeatBalanceTypeShellTube.SSInlet:
                            case ExchangerHeatBalanceTypeShellTube.TSInlet:
                                {
                                    return;
                                }
                        }
                        HeatBalanceFromUAExplicit();
                        MinimumApproach();
                        break;
                    }
                case HeExCalcOptions.ThreeFixedTempsCalcUA:
                    {
                        if (HeatBalanceType == ExchangerHeatBalanceTypeShellTube.None ||
                             HeatBalanceType == ExchangerHeatBalanceTypeShellTube.TwoInlets ||
                             HeatBalanceType == ExchangerHeatBalanceTypeShellTube.TwoOutlets)
                        {
                            return;
                        }
                        CalcUA();
                        MinimumApproach();
                        break;
                    }
                case HeExCalcOptions.ThreeFixedTempsCalcDesign:
                    {
                        if (TSIn.T == double.NaN || SSIn.T == double.NaN)
                        {
                            return;
                        }
                        if (HeatBalanceType == ExchangerHeatBalanceTypeShellTube.None ||
                            HeatBalanceType == ExchangerHeatBalanceTypeShellTube.TwoInlets ||
                            HeatBalanceType == ExchangerHeatBalanceTypeShellTube.TwoOutlets)
                        {
                            return;
                        }
                        IterateU();
                        MinimumApproach();
                        CalcUA();
                        break;
                    }
                case HeExCalcOptions.ThreeFixTempsCalcFoulingFactor:
                    {
                        break;
                    }
                case HeExCalcOptions.TwoFixInletsFixedDesign:
                    {
                        if (TSIn.T == double.NaN || SSIn.T == double.NaN || eProps.UA == double.NaN)
                        {
                            return;
                        }
                        if (HeatBalanceType == ExchangerHeatBalanceTypeShellTube.None ||
                            HeatBalanceType == ExchangerHeatBalanceTypeShellTube.ShellSide ||
                            HeatBalanceType == ExchangerHeatBalanceTypeShellTube.TubeSide ||
                            HeatBalanceType == ExchangerHeatBalanceTypeShellTube.TwoOutlets)
                        {
                            return;
                        }
                        Mech.UA.SetValueIfChanged(CalcUAFromDesign(), SourceEnum.CalcResult, EXguid);
                        HeatBalanceFromUAExplicit();
                        break;
                    }
                case HeExCalcOptions.ByPass:
                    {
                        DoByPass();

                        break;
                    }
                case HeExCalcOptions.DeltaTs:
                    {
                        DoMassBalanceType();
                        DoMassBalance();
                        DoHeatBalanceType();
                        DoHeatBalance();
                        MinimumApproach();
                        CalcUA();

                        break;
                    }
            }
            return;
        }

        public void DoHeatBalanceType()
        {
            if (SSIn is null || SSOut is null || TSIn is null || TSOut is null)
            {
            }
            else
            {
                int Sum = Convert.ToInt32(SSIn.T.IsExtOrSpec(EXguid)) * 1 + Convert.ToInt32(SSOut.T.IsExtOrSpec(EXguid)) * 2
                + Convert.ToInt32(TSIn.T.IsExtOrSpec(EXguid)) * 4 + Convert.ToInt32(TSOut.T.IsExtOrSpec(EXguid)) * 8;

                //TSOut.T.IsExtOrSpec(EXguid);
                //TSIn.T.IsExtOrSpec(EXguid);

                switch (Sum)
                {
                    case 1: // SS inlet only
                        {
                            eProps.ShellIsHotSide = null;
                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.SSInlet;
                            break;
                        }
                    case 3: // Shell side only known
                        {
                            eProps.ShellIsHotSide = null;
                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.ShellSide;
                            break;
                        }
                    case 4: // SS inlet only
                        {
                            eProps.ShellIsHotSide = null;
                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.TSInlet;
                            break;
                        }
                    case 5: // both inlets known
                        {
                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.TwoInlets;
                            if (SSIn.T > TSIn.T)
                                eProps.ShellIsHotSide = true;
                            else
                                eProps.ShellIsHotSide = false;

                            break;
                        }
                    case 6: // both outlets known
                        {
                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.TwoOutlets;

                            if (SSOut.T > TSOut.T)
                                eProps.ShellIsHotSide = true;
                            else
                                eProps.ShellIsHotSide = false;

                            break;
                        }
                    case 7: // SS and TS inlet known
                        {
                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.ShellSideTSInlet;

                            if (SSIn.T > TSIn.T)
                                eProps.ShellIsHotSide = true;
                            else
                                eProps.ShellIsHotSide = false;

                            break;
                        }
                    case 11:  // SS and TS outlet known
                        {
                            if (SSIn.T > TSIn.T)
                                eProps.ShellIsHotSide = true;
                            else
                                eProps.ShellIsHotSide = false;

                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.ShellSideTSOutlet;
                            break;
                        }
                    case 12: // Tube side only known
                        {
                            eProps.ShellIsHotSide = null;
                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.TubeSide;
                            break;
                        }
                    case 13: // Tube Side + SS Inlet Known
                        {
                            if (SSIn.T > TSIn.T)
                                eProps.ShellIsHotSide = true;
                            else
                                eProps.ShellIsHotSide = false;

                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.TubeSideSSInlet;
                            break;
                        }
                    case 14: // Tube Side + SS Outlet Known
                        {
                            if (SSOut.T > TSOut.T)
                                eProps.ShellIsHotSide = true;
                            else
                                eProps.ShellIsHotSide = false;

                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.TubeSideSSOutlet;
                            break;
                        }
                    case 15: // All Known
                        {
                            if (SSOut.T > TSOut.T)
                                eProps.ShellIsHotSide = true;
                            else
                                eProps.ShellIsHotSide = false;

                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.None;
                            break;
                        }

                    default:
                        {
                            eProps.ShellIsHotSide = null;
                            HeatBalanceType = ExchangerHeatBalanceTypeShellTube.None;
                            break;
                        }
                }

                if (eProps.ShellIsHotSide == true)
                {
                    HSIn = SSIn;
                    HSOut = SSOut;
                    CSIn = TSIn;
                    CSOut = TSOut;
                }
                else if (eProps.ShellIsHotSide == false)
                {
                    CSIn = SSIn;
                    CSOut = SSOut;
                    HSIn = TSIn;
                    HSOut = TSOut;
                }
                else
                {
                    CSIn = null;
                    CSOut = null;
                    HSIn = null;
                    HSOut = null;
                }
            }
        }

        public void DoHeatBalance()
        {
            switch (ExType)
            {
                case ExchangerHeatBalanceTypeHotCold.HotSide:
                    {   // kJ/hr
                        eProps.Duty.SetValueIfChanged((HSOut.MassEnthalpy - HSIn.MassEnthalpy) * HSIn.MF.ToUnits("kg/hr"), SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.ColdSide:
                    {
                        eProps.Duty.SetValueIfChanged((CSOut.MassEnthalpy - CSIn.MassEnthalpy) * CSIn.MF.ToUnits("kg/hr"), SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.HotSideCSInlet:
                    {
                        eProps.Duty.SetValueIfChanged((HSOut.MassEnthalpy - HSIn.MassEnthalpy) * HSIn.MF.ToUnits("kg/hr"), SourceEnum.UnitOp, EXguid);
                        CSOut.MolarEnthalpy.SetValueIfChanged(CSIn.MolarEnthalpy.Value - eProps.Duty.Value / CSIn.MolarFlow, SourceEnum.UnitOp, EXguid);
                        CSOut.Flash();
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.HSSideCSOutlet:
                    {
                        eProps.Duty.SetValueIfChanged((HSOut.MassEnthalpy - HSIn.MassEnthalpy) * HSIn.MF.ToUnits("kg/hr"), SourceEnum.UnitOp, EXguid);
                        CSIn.MolarEnthalpy.SetValueIfChanged(CSOut.MassEnthalpy.Value + eProps.Duty.Value / CSIn.MassEnthalpy, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.ColdSideHSInlet:
                    {
                        eProps.Duty.SetValueIfChanged((CSOut.MassEnthalpy - CSIn.MassEnthalpy) * CSIn.MF.ToUnits("kg/hr"), SourceEnum.UnitOp, EXguid); //kJ/hr
                        HSOut.MolarEnthalpy.SetValueIfChanged(HSIn.MolarEnthalpy.Value - eProps.Duty.Value / HSIn.MolarFlow, SourceEnum.UnitOp, EXguid);
                        HSOut.Flash();
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.ColdSideHSOutlet:
                    {
                        eProps.Duty.SetValueIfChanged((CSIn.MassEnthalpy - CSOut.MassEnthalpy) * CSIn.MF.ToUnits("kg/hr"), SourceEnum.UnitOp, EXguid);
                        HSIn.MolarEnthalpy.SetValueIfChanged(HSOut.MolarEnthalpy.Value + eProps.Duty.Value / HSIn.MolarFlow, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.TwoInlets:
                    {
                        switch (option)
                        {
                            case HeExCalcOptions.ByPass:
                            case HeExCalcOptions.DeltaTs:
                            case HeExCalcOptions.Off:
                                {
                                    if (eProps.TubeProps.DT.IsExtOrSpec(EXguid) && eProps.ShellProps.DT.IsExtOrSpec(EXguid))
                                    {
                                        TSOut.T.SetValueIfChanged(TSIn.T.Value + eProps.TubeProps.DT.Value, SourceEnum.UnitOp, EXguid);
                                        SSOut.T.SetValueIfChanged(SSIn.T.Value + eProps.TubeProps.DT.Value, SourceEnum.UnitOp, EXguid);
                                    }
                                    else if (eProps.TubeProps.DT.IsExtOrSpec(EXguid))
                                    {
                                        TSOut.T.SetValueIfChanged(TSIn.T.Value + eProps.TubeProps.DT.Value, SourceEnum.UnitOp, EXguid);
                                        TSOut.Flash();
                                        Duty = (TSOut.MolarEnthalpy.Value - TSIn.MolarEnthalpy.Value) * TSIn.MolarFlow;
                                        SSOut.MolarEnthalpy.SetValueIfChanged(SSIn.MolarEnthalpy - Duty / SSIn.MolarFlow, SourceEnum.UnitOp, EXguid);
                                    }
                                    else if (eProps.ShellProps.DT.IsExtOrSpec(EXguid))
                                    {
                                        SSOut.T.SetValueIfChanged(SSIn.T.Value + eProps.ShellProps.DT.Value, SourceEnum.UnitOp, EXguid);
                                        SSOut.Flash();
                                        Duty = (SSOut.MolarEnthalpy.Value - SSIn.MolarEnthalpy.Value) * SSIn.MolarFlow;
                                        TSOut.MolarEnthalpy.SetValueIfChanged(TSIn.MolarEnthalpy - Duty / TSIn.MolarFlow, SourceEnum.UnitOp, EXguid);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.TSInlet:
                    {
                        if (eProps.TubeProps.DT.IsExtOrSpec(EXguid))
                        {
                            //TSOut.MolarEnthalpy.EraseValue();
                            TSOut.T.SetValueIfChanged(TSIn.T.Value + eProps.TubeProps.DT.Value, SourceEnum.UnitOp, EXguid);
                        }
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.SSInlet:
                    {
                        if (eProps.ShellProps.DT.IsExtOrSpec(EXguid))
                        {
                            // SSOut.MolarEnthalpy.EraseValue();
                            SSOut.T.SetValueIfChanged(SSIn.T.Value + eProps.TubeProps.DT.Value, SourceEnum.UnitOp, EXguid);
                        }
                        break;
                    }
            }

            switch (HeatBalanceType) // set DT's if info available.
            {
                case ExchangerHeatBalanceTypeShellTube.TubeSide:
                case ExchangerHeatBalanceTypeShellTube.TubeSideSSInlet:
                case ExchangerHeatBalanceTypeShellTube.TubeSideSSOutlet:
                    {
                        eProps.TubeProps.DT.SetValueIfChanged(Math.Abs(TSOut.T - TSIn.T), SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeShellTube.ShellSide:
                case ExchangerHeatBalanceTypeShellTube.ShellSideTSInlet:
                case ExchangerHeatBalanceTypeShellTube.ShellSideTSOutlet:
                    {
                        eProps.ShellProps.DT.SetValueIfChanged(Math.Abs(SSOut.T - SSIn.T), SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeShellTube.TwoInlets:
                case ExchangerHeatBalanceTypeShellTube.TwoOutlets:
                    {
                        break;
                    }
                default:
                    {
                        eProps.TubeProps.DT.EraseValue();
                        eProps.ShellProps.DT.EraseValue();
                        break;
                    }
            }
        }

        public void DoMassBalanceType()
        {
            if (SSIn is null || SSOut is null || TSIn is null || TSOut is null)
            {
            }
            else
            {
                int Sum = Convert.ToInt32(SSIn.Flow.IsExtOrSpec(EXguid)) * 1 + Convert.ToInt32(SSOut.Flow.IsExtOrSpec(EXguid)) * 2
                    + Convert.ToInt32(TSIn.Flow.IsExtOrSpec(EXguid)) * 4 + Convert.ToInt32(TSOut.Flow.IsExtOrSpec(EXguid)) * 8;

                SSOut.Flow.IsExtOrSpec(EXguid);

                switch (Sum)
                {
                    case 1: // SS inlet only
                        {
                            MassBalanceType = ExchangerMassBalanceType.SSIn;
                            break;
                        }
                    case 2:
                        {
                            MassBalanceType = ExchangerMassBalanceType.SSOut;
                            break;
                        }
                    case 4:
                        {
                            MassBalanceType = ExchangerMassBalanceType.TSIn;
                            break;
                        }
                    case 8:
                        {
                            MassBalanceType = ExchangerMassBalanceType.TSOut;
                            break;
                        }
                    case 5: // SS and TS inlet known
                        {
                            MassBalanceType = ExchangerMassBalanceType.SSint SIn;
                            break;
                        }
                    case 9:  // SS and TS outlet known
                        {
                            MassBalanceType = ExchangerMassBalanceType.SSint SOut;
                            break;
                        }
                    case 6: // Tube side only known
                        {
                            MassBalanceType = ExchangerMassBalanceType.SSOutTSin;
                            break;
                        }
                    case 7: // Tube side only known
                        {
                            MassBalanceType = ExchangerMassBalanceType.SSOutTSin;
                            break;
                        }
                    case 10: // Tube Side + SS Inlet Known
                        {
                            MassBalanceType = ExchangerMassBalanceType.SSOutTSOut;
                            break;
                        }
                    default:
                        {
                            MassBalanceType = ExchangerMassBalanceType.None;
                            break;
                        }
                }
            }
        }

        public void DoMassBalance()
        {
            switch (MassBalanceType)
            {
                case ExchangerMassBalanceType.SSIn:
                    {
                        SSOut.Flow.SetValueIfChanged(SSIn.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerMassBalanceType.SSOut:
                    {
                        SSIn.Flow.SetValueIfChanged(SSOut.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerMassBalanceType.TSIn:
                    {
                        TSOut.Flow.SetValueIfChanged(TSIn.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerMassBalanceType.TSOut:
                    {
                        TSIn.Flow.SetValueIfChanged(TSOut.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerMassBalanceType.SSint SIn:
                    {
                        SSOut.Flow.SetValueIfChanged(SSIn.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        TSOut.Flow.SetValueIfChanged(TSIn.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerMassBalanceType.SSint SOut:
                    {
                        SSOut.Flow.SetValueIfChanged(SSIn.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        TSIn.Flow.SetValueIfChanged(TSOut.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerMassBalanceType.SSOutTSin:
                    {
                        SSIn.Flow.SetValueIfChanged(SSOut.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        TSOut.Flow.SetValueIfChanged(TSIn.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerMassBalanceType.SSOutTSOut:
                    {
                        SSIn.Flow.SetValueIfChanged(SSOut.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        TSIn.Flow.SetValueIfChanged(TSOut.MolarFlow, FlowPropagateFlag.MOL, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerMassBalanceType.Overdefined:
                    {
                        //overdeifned
                        break;
                    }
            }
        }

        public void UpdateProperties(Node tSIN, Node sSIN, Node tSOUT, Node sSOUT)
        {
            this.SSIn = sSIN.Oil;
            this.TSIn = tSIN.Oil;
            this.SSOut = sSOUT.Oil;
            this.TSOut = tSOUT.Oil;

            if (TSIn != null && sSIN != null && TSOut != null && SSOut != null)
            {
                /*exchangerProps.TubeProps.HeatCapacity = TSIn.Cp_mass_Liq;
                exchangerProps.ShellProps.HeatCapacity = SSIn.Cp_mass_Liq;

                exchangerProps.TubeProps.ThermaCond.Value = (double )TSIn.ThermalConductivity;
                exchangerProps.ShellProps.ThermaCond.Value = (double )SSIn.ThermalConductivity;

                exchangerProps.TubeProps.Visc.Value = TSIn.Viscosity;
                exchangerProps.ShellProps.Visc.Value = SSIn.Viscosity;

                exchangerProps.TubeProps.Density.Value = TSIn.ActLiqDensity();
                exchangerProps.ShellProps.Density.Value = SSIn.ActLiqDensity();

                exchangerProps.TubeProps.MassFlow.Value = TSIn.MF;
                exchangerProps.ShellProps.MassFlow.Value = SSIn.MF;*/
            }
        }

        public void UpdateProperties()
        {
            //exchangerProps.TubeProps.Visc.Value = TSIn.Viscosity;
            //exchangerProps.ShellProps.Visc.Value = SSIn.Viscosity;
        }

        public double TubeSideH(kg_hr MassFlow, kg_m3 Density, W_MK ThermCond, Cp viscosity, kJ_kg_C massheatcapacity, Metres TubeInnerDiam, m_s velocity)
        {
            double ReynoldsNo, PrandtlNo, NusseltNo, jh, h;
            jh = 0.0032;

            bool isheating;

            if (eProps.ShellIsHotSide == true)
                isheating = true;
            else
                isheating = false;

            ReynoldsNo = HeatTransferCalcs.ReynoldsNo(Density, velocity, TubeInnerDiam, viscosity);
            PrandtlNo = HeatTransferCalcs.PrandtlNo(massheatcapacity, viscosity, (double)ThermCond);
            NusseltNo = HeatTransferCalcs.NusseltNo(jh, ReynoldsNo, PrandtlNo, isheating);

            h = NusseltNo * (double)ThermCond / TubeInnerDiam;

            return h;
        }

        public double ShellSideH(kg_m3 density, Cp viscosity, W_MK thermcond, kJ_kg_C massheatcapacity, Metres EquivDiam)
        {
            double ReynoldsNo, PrandtlNo, NusseltNo, jh, h;
            jh = 0.007;
            bool isheating;

            if (eProps.ShellIsHotSide == true)
                isheating = false;
            else
                isheating = true;

            ReynoldsNo = HeatTransferCalcs.ReynoldsNo(density, Mech.ShellVelocity, EquivDiam, viscosity);
            PrandtlNo = HeatTransferCalcs.PrandtlNo(massheatcapacity, viscosity, (double)thermcond);
            NusseltNo = HeatTransferCalcs.NusseltNo(jh, ReynoldsNo, PrandtlNo, isheating);

            h = NusseltNo * (double)thermcond / EquivDiam;

            return h;
        }

        public void UpdateStoredValues()
        {
            eProps.TransferArea.SetValueIfChanged(TransferArea, SourceEnum.UnitOp, EXguid);
            Mech.TubeVelocity.SetValueIfChanged(TubeVelocity, SourceEnum.UnitOp, EXguid);
            eProps.ShellDiam.SetValueIfChanged(ShellDiam, SourceEnum.UnitOp, EXguid);
            eProps.bafflespacing.SetValueIfChanged(Bafflespacing, SourceEnum.UnitOp, EXguid);
            Mech.ShellVelocity.SetValueIfChanged(ShellVelocity, SourceEnum.UnitOp, EXguid);
            Mech.Shellh.SetValueIfChanged(Shellh, SourceEnum.UnitOp, EXguid);
            Mech.Tubeh.SetValueIfChanged(Tubeh, SourceEnum.UnitOp, EXguid);
            Mech.overallh.SetValueIfChanged(Overallh, SourceEnum.UnitOp, EXguid);
            eProps.TubeInnerDiam.SetValueIfChanged(TubeInnerDiam, SourceEnum.UnitOp, EXguid);
            eProps.Duty.SetValueIfChanged(Duty, SourceEnum.UnitOp, EXguid);
            eProps.NoTubes.SetValueIfChanged(NoTubes, SourceEnum.UnitOp, EXguid);
            eProps.tubepitch.SetValueIfChanged(Tubepitch, SourceEnum.UnitOp, EXguid);

            /*eProps.TubeOuterDiam.SetValueIfChanged(TubeOuterDiam, SourceEnum.UnitOp, EXguid);
            eProps.TubeThickness.SetValueIfChanged(TubeThickness, SourceEnum.UnitOp, EXguid);
            eProps.TubeLength.SetValueIfChanged(TubeLength, SourceEnum.UnitOp, EXguid);
            eProps.NoTubePasses.SetValueIfChanged(NoTubePasses, SourceEnum.UnitOp, EXguid);
        */
        }

        public void GetStoredValues()
        {
            TransferArea = eProps.TransferArea;
            TubeVelocity = Mech.TubeVelocity;
            ShellDiam = eProps.ShellDiam.ToUnits("m");
            Bafflespacing = eProps.bafflespacing.ToUnits("m");
            ShellVelocity = Mech.ShellVelocity;
            Shellh = Mech.Shellh;
            Tubeh = Mech.Tubeh;
            Overallh = Mech.overallh;
            TubeInnerDiam = eProps.TubeInnerDiam.ToUnits("m");
            TubeOuterDiam = eProps.TubeOuterDiam.ToUnits("m");
            TubeThickness = eProps.TubeThickness;
            Duty = eProps.Duty.ToUnits("kW");
            TubeLength = eProps.TubeLength.ToUnits("m");
            NoTubes = eProps.NoTubes;
            Tubepitch = eProps.tubepitch.ToUnits("m");
            NoTubePasses = eProps.NoTubePasses;
            TubeFoulingRes = eProps.TubeProps.FoulingRes;
            ShellFoulingRes = eProps.ShellProps.FoulingRes;
        }

        double TransferArea, TubeVelocity, ShellDiam, Bafflespacing, ShellVelocity, Shellh, Tubeh,
            Overallh, TubeInnerDiam, TubeOuterDiam, TubeThickness, LMTD, LMTDCorr, TubeSurfaceArea,
            TubeCrossSectionArea, TubesPerPass, Areaoftubesperpass, Duty, TubeLength, NoTubes, Tubepitch, BundleDiameter, NoTubePasses, TubeFoulingRes, ShellFoulingRes;

        public double CalcU(double UGuess = 1000)
        {
            double R1, R2, R3, R4;

            TubeInnerDiam = TubeOuterDiam - TubeThickness;

            LMTD = HeatTransferCalcs.LMTD(HSIn.T, HSOut.T, CSIn.T, CSOut.T);
            LMTDCorr = LMTD * HeatTransferCalcs.Ft(HSIn.T, HSOut.T, CSIn.T, CSOut.T, (int)eProps.NoShells);

            TransferArea = Duty * 1000 / (LMTDCorr * UGuess);
            TubeSurfaceArea = TubeOuterDiam * TubeLength * Math.PI;
            TubeCrossSectionArea = Math.Pow(TubeInnerDiam, 2) * Math.PI / 4;
            NoTubes = Math.Round(TransferArea / TubeSurfaceArea);
            Tubepitch = TubeOuterDiam * 1.25;

            TubesPerPass = Math.Max((int)(NoTubes / NoTubePasses), 1);
            Areaoftubesperpass = TubesPerPass * TubeCrossSectionArea;

            //m/s
            TubeVelocity = (TSIn.MF / 3600 / TSIn.ActLiqDensity()) / Areaoftubesperpass;

            KandN((int)NoTubes, out double K, out double N);

            BundleDiameter = TubeOuterDiam * (((int)NoTubes / K).Pow(1 / N));
            ShellDiam = Math.Max(BundleDiameter + 0.0125, 0.205);
            Bafflespacing = 0.55 * ShellDiam;

            var ShellArea = (Tubepitch - TubeOuterDiam) * ShellDiam * Bafflespacing / Tubepitch;
            var EquivDiam = (1.1 / TubeOuterDiam) * (((Tubepitch.Pow(2)) - (0.917 * (TubeOuterDiam.Pow(2)))));
            var VolFlowRateShellSide = SSIn.MF / SSIn.ActLiqDensity();

            ShellVelocity = VolFlowRateShellSide / 60 / 60 / ShellArea;

            Shellh = ShellSideH(SSIn.ActLiqDensity(), SSIn.Viscosity, SSIn.ThermalConductivity, SSIn.Cp_mass_Liq, EquivDiam);
            Tubeh = TubeSideH(TSIn.MF.Value, TSIn.ActLiqDensity(), TSIn.ThermalConductivity, TSIn.Viscosity, TSIn.Cp_mass_Liq, TubeInnerDiam, new m_s(((double)Mech.TubeVelocity)));

            R1 = 1 / Tubeh;
            R2 = TubeFoulingRes;
            R3 = (TubeOuterDiam * Math.Log(TubeOuterDiam / TubeInnerDiam)) / (2 * 45);
            R4 = (TubeOuterDiam / TubeInnerDiam) * ((1 / Shellh) + TubeFoulingRes);

            Overallh = 1 / (R1 + R2 + R3 + R4);

            return Overallh;
        }

        public double CalcUAFromDesign()
        {
            double TubeSurfaceArea, TubeCrossSectionArea, TubesPerPass, Areaoftubesperpass;
            double R1, R2, R3, R4;

            eProps.TubeInnerDiam = eProps.TubeOuterDiam - eProps.TubeThickness;

            //eProps.TransferArea.Value = (eProps.Duty.ToUnits("kW") * 1000) / (LMTDCorr * UGuess);
            TubeSurfaceArea = eProps.TubeOuterDiam * eProps.TubeLength * Math.PI;
            TubeCrossSectionArea = Math.Pow(eProps.TubeInnerDiam, 2) * Math.PI / 4;
            // eProps.NoTubes.Value = Math.Round(eProps.TransferArea / TubeSurfaceArea);
            eProps.tubepitch.Value = eProps.TubeOuterDiam * 1.25;

            TubesPerPass = Math.Max((int)(eProps.NoTubes / eProps.NoTubePasses), 1);
            Areaoftubesperpass = TubesPerPass * TubeCrossSectionArea;
            //m/s
            Mech.TubeVelocity.Value = (TSIn.MF / 3600 / TSIn.ActLiqDensity()) / Areaoftubesperpass;

            KandN((int)eProps.NoTubes, out double K, out double N);

            double BundleDiameter = eProps.TubeOuterDiam.ToUnits("m") * (((int)eProps.NoTubes / K).Pow(1 / N));
            eProps.ShellDiam.Value = Math.Max(BundleDiameter + 0.0125, 0.205);

            var ShellArea = (eProps.tubepitch - eProps.TubeOuterDiam) * eProps.ShellDiam * eProps.bafflespacing / eProps.tubepitch;
            var EquivDiam = (1.1 / eProps.TubeOuterDiam) * (((eProps.tubepitch.Value.Pow(2)) - (0.917 * (eProps.TubeOuterDiam.Value.Pow(2)))));
            var VolFlowRate = SSIn.MF / SSIn.ActLiqDensity();
            Mech.ShellVelocity.Value = VolFlowRate / 60 / 60 / ShellArea;

            Mech.Shellh.Value = ShellSideH(SSIn.ActLiqDensity(), SSIn.Viscosity, SSIn.ThermalConductivity, SSIn.Cp_mass_Liq, EquivDiam);
            Mech.Tubeh.Value = TubeSideH(TSIn.MF.Value, TSIn.ActLiqDensity(), TSIn.ThermalConductivity, TSIn.Viscosity, TSIn.Cp_mass_Liq, eProps.TubeInnerDiam.Value, new m_s(((double)Mech.ShellVelocity.Value)));

            R1 = 1 / Mech.Shellh.Value;
            R2 = eProps.TubeProps.FoulingRes;
            R3 = (eProps.TubeOuterDiam * Math.Log(eProps.TubeOuterDiam / eProps.TubeInnerDiam)) / (2 * 45);
            R4 = (eProps.TubeOuterDiam / eProps.TubeInnerDiam) * ((1 / Mech.Tubeh.Value) + eProps.ShellProps.FoulingRes);

            Mech.overallh.Value = 1 / (R1 + R2 + R3 + R4);

            return Mech.overallh.Value * eProps.TransferArea;
        }

        public void KandN(int tubepasses, out double K, out double N)
        {
            //For 2 t.p For 4 t.p For 6 t.p
            //k1  0.249   0.175   0.0743
            //n1  2.207   2.285   2.499
            //Clearance   0.0125

            switch (tubepasses)
            {
                case 2:
                    {
                        K = 0.249;
                        N = 2.207;
                        break;
                    }
                case 4:
                    {
                        K = 0.175;
                        N = 2.285;
                        break;
                    }
                case 6:
                    {
                        K = 0.0743;
                        N = 2.249;
                        break;
                    }
                default:
                    {
                        K = 0.249;
                        N = 2.207;
                        break;
                    }
            }
        }

        public void SolveBFGS()
        {
            double[,] test = new double[2, 2] { { 0, 0 }, { 0, 0 } };
            var a = Matrix<double>.Build.DenseOfArray(test);

            var guess = Vector<double>.Build.DenseOfArray(new double[2] { 1, 1 });
            var guess2 = Vector<double>.Build.DenseOfArray(new double[2] { 1, 1 });

            //var res = BfgsSolver.Solve(guess, Solve, guess2);
        }

        /*      public  void  HeatBalanceFromUAIterative()
              {
                  if (HSIn == null || CSIn == null || double .IsNaN(eProps.UA))
                      return  ;
                  double  lmtd = 1;
                  double  lmtdnew  = 1;
                  double  duty = 1000;
                  double  UA = eProps.UA;
                  int  shells = 1;
                  int  count = 0;
                  double  HSEnth = HSIn.MolarEnthalpy, CSEnth = CSIn.MolarEnthalpy;
                  double  calcduty = 0;
                  double  minlmtd = 0, oldLMTD = 0;

                  Oil HSClone = (Oil)HSIn.Clone();
                  Oil CSClone = (Oil)CSIn.Clone();

                  double  maxlmtd = HSIn.T - CSIn.T;

                  for (int  i = 1; i <= 10; i++)
                  {
                      count++;

                      lmtd = ((maxlmtd - minlmtd) * i / 10D + minlmtd);

                      duty = lmtd * UA * 60 * 60; //kJ/hr

                      switch (HeatBalanceType)
                      {
                          case ExchangerHeatBalanceTypeShellTube.TwoInlets:
                              {
                                  HSOut.MolarEnthalpy.SetValueIfChanged(HSEnth - duty / HSIn.MolarFlow, SourceEnum.UnitOp, EXguid);
                                  HSOut.Flash(HSOut.ThermoBasis);

                                  CSOut.MolarEnthalpy.SetValue(CSEnth + duty / CSIn.MolarFlow, SourceEnum.UnitOp, EXguid);
                                  CSOut.Flash(CSOut.ThermoBasis);

                                  lmtdnew  = HeatTransferCalcs.LMTDft(HSIn.T, HSOut.T, CSIn.T, CSOut.T, shells);
                                  calcduty = lmtdnew  * UA * 60 * 60;

                                  if (calcduty < duty)
                                  {
                                      maxlmtd = oldLMTD;
                                      minlmtd = lmtdnew ;
                                  }
                                  else
                                      oldLMTD = lmtdnew ;

                                  if (double .IsNaN(lmtdnew ))
                                  {
                                      maxlmtd *= 0.99;
                                      i = 0;
                                  }
                                  break;
                              }
                      }
                      if (Math.Abs(maxlmtd - minlmtd) < 0.01 || count > 1000)
                          break;
                  }

                  eProps.Duty.SetValueIfChanged(calcduty, SourceEnum.UnitOp, EXguid);
                  eProps.LMTD.SetValueIfChanged(HeatTransferCalcs.LMTD(HSIn.T, HSOut.T, CSIn.T, CSOut.T), SourceEnum.UnitOp, EXguid);
                  eProps.Ft.SetValueIfChanged(HeatTransferCalcs.Ft(HSIn.T, HSOut.T, CSIn.T, CSOut.T, shells), SourceEnum.UnitOp, EXguid);
                  eProps.LMTDCorr.SetValueIfChanged(HeatTransferCalcs.LMTDft(HSIn.T, HSOut.T, CSIn.T, CSOut.T, shells), SourceEnum.UnitOp, EXguid);
              }
      */
        public void HeatBalanceFromUAExplicit()
        {
            if (HSIn == null || CSIn == null || double.IsNaN(eProps.UA))
                return;

            int shells = 1, count = 0;
            double UA = eProps.UA;
            double Qold = 0;

            double MCP1, MCP2, CSint, HSint, CSOutT = 0, HSOutT = 0, HotApproachTemp, ColdApproachTemp, Q = 0, LMTD = 0, ft = 1;

            CSint = CSIn.T;
            HSint = HSIn.T;

            MCP1 = CSIn.Cp_mass_Liq * CSIn.MF;
            MCP2 = HSIn.Cp_mass_Liq * HSIn.MF;

            //MCP1 = CSIn.CalcMassBulkStreamHeatCapacity(CSint , HSint ) * CSIn.MF;
            //MCP2 = SSIn.CalcMassBulkStreamHeatCapacity(HSint , CSint ) * HSIn.MF;

            switch (HeatBalanceType)
            {
                case ExchangerHeatBalanceTypeShellTube.TwoInlets:
                    {
                        if (HSint == CSint)
                        {
                            HSOutT = HSint;
                            CSOut = CSIn;
                            LMTD = 0;
                            ft = 1;
                            Q = 0;
                        }
                        else
                        {
                            do  //converged after 3 ??
                            {
                                count++;
                                Qold = Q;
                                Q = ft * HeatTransferCalcs.Q_UAExplicitTHint CIn(UA, MCP1, MCP2, CSint, HSint); // kJ/hr // UA kJ/hr/C

                                CSOutT = CSint + Q / MCP1;
                                HSOutT = HSint - Q / MCP2;

                                LMTD = HeatTransferCalcs.LMTD(HSint, HSOutT, CSint, CSOutT);

                                CSOut.T.SetValueIfChanged(CSOutT, SourceEnum.UnitOp, EXguid);
                                HSOut.T.SetValueIfChanged(HSOutT, SourceEnum.UnitOp, EXguid);

                                CSOut.Flash(); // update enthalpies etc
                                HSOut.Flash();

                                //MCP1 = (CSOut.MassEnthalpy - CSIn.MassEnthalpy) / (CSOutT - CSint ) * CSIn.MF;
                                //MCP2 = (HSIn.MassEnthalpy - HSOut.MassEnthalpy) / (HSint  - HSOutT) * HSIn.MF;

                                //MCP1 = CSIn.CalcMassBulkStreamHeatCapacity(CSint , CSOutT) * CSIn.MF;
                                //MCP2 = SSIn.CalcMassBulkStreamHeatCapacity(HSint , HSOutT) * HSIn.MF;

                                MCP1 = CSIn.CalcBulkStreamAverageHeatCapacity(CSint, CSOutT) * CSIn.MF;
                                MCP2 = HSIn.CalcBulkStreamAverageHeatCapacity(HSint, HSOutT) * CSIn.MF;

                                Debug.Print(MCP1.ToString() + " " + MCP2.ToString());

                                ft = HeatTransferCalcs.Ft(HSint, HSOutT, CSint, CSOutT, shells);

                                if (ft == double.NaN || double.IsNaN(ft))
                                    ft = 1;
                            } while ((Math.Abs(Q - Qold) > 0.000001) && count < 500);
                        }

                        HotApproachTemp = HSint - CSOutT;
                        ColdApproachTemp = HSOutT - CSint;
                        break;
                    }
            }

            eProps.Duty.SetValueIfChanged(Q * 60 * 60, SourceEnum.UnitOp, EXguid);
            eProps.LMTD.SetValueIfChanged(LMTD, SourceEnum.UnitOp, EXguid);
            eProps.Ft.SetValueIfChanged(ft, SourceEnum.UnitOp, EXguid);
            eProps.LMTDCorr.SetValueIfChanged(LMTD * ft, SourceEnum.UnitOp, EXguid);
        }

        public void CalcUA()
        {
            switch (HeatBalanceType)
            {
                case ExchangerHeatBalanceTypeShellTube.None:
                case ExchangerHeatBalanceTypeShellTube.ShellSide:
                case ExchangerHeatBalanceTypeShellTube.TubeSide:
                case ExchangerHeatBalanceTypeShellTube.TwoInlets:
                case ExchangerHeatBalanceTypeShellTube.TwoOutlets:
                    {
                        eProps.LMTD.EraseValue();
                        eProps.Ft.EraseValue();
                        eProps.LMTDCorr.EraseValue();
                        eProps.UA.EraseValue();
                        return;
                    }
                default:
                    {
                        if (HSIn != null && HSOut != null && CSIn != null && CSOut != null)
                        {
                            eProps.LMTD.SetValueIfChanged(HeatTransferCalcs.LMTD(HSIn.T, HSOut.T, CSIn.T, CSOut.T), SourceEnum.CalcResult, EXguid);
                            eProps.Ft.SetValueIfChanged(HeatTransferCalcs.Ft(HSIn.T, HSOut.T, CSIn.T, CSOut.T, 1), SourceEnum.CalcResult, EXguid);
                            eProps.LMTDCorr.SetValueIfChanged(eProps.LMTD * eProps.Ft, SourceEnum.CalcResult, EXguid);
                            eProps.UA.SetValueIfChanged(eProps.Duty.ToUnits("kW") / eProps.LMTD, SourceEnum.CalcResult, EXguid); // kW/C
                        }
                        break;
                    }
            }
        }

        public void IterateU()
        {
            double Uguess = 0, Unew = 1000;

            GetStoredValues();

            do
            {
                Uguess = Unew;
                Unew = CalcU(Uguess);
            } while (Uguess != Unew && !double.IsNaN(Unew));

            UpdateStoredValues();
        }

        public void DoByPass()
        {
            switch (ExType)
            {
                case ExchangerHeatBalanceTypeHotCold.TwoInlets:
                    {
                        CSOut.MolarEnthalpy.SetValueIfChanged(CSIn.MolarEnthalpy, SourceEnum.UnitOp, EXguid);
                        HSOut.MolarEnthalpy.SetValueIfChanged(HSIn.MolarEnthalpy, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.TwoOutlets:
                    {
                        CSIn.MolarEnthalpy.SetValueIfChanged(CSOut.MolarEnthalpy, SourceEnum.UnitOp, EXguid);
                        HSIn.MolarEnthalpy.SetValueIfChanged(HSOut.MolarEnthalpy, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.SSInlet:
                    {
                        SSOut.MolarEnthalpy.SetValueIfChanged(SSIn.MolarEnthalpy, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeHotCold.TSInlet:
                    {
                        TSOut.MolarEnthalpy.SetValueIfChanged(TSIn.MolarEnthalpy, SourceEnum.UnitOp, EXguid);
                        break;
                    }
            }
        }

        public void MinimumApproach()
        {
            if (HSIn != null && CSOut != null && HSOut != null && CSIn != null)
            {
                if ((HSIn.T - CSOut.T) > (HSOut.T - CSIn.T))
                    eProps.MinApproach.SetValueIfChanged(HSOut.T - CSIn.T, SourceEnum.UnitOp, EXguid);
                else
                    eProps.MinApproach.SetValueIfChanged(HSIn.T - CSOut.T, SourceEnum.UnitOp, EXguid);
            }

            switch (HeatBalanceType)
            {
                case ExchangerHeatBalanceTypeShellTube.ShellSide:
                    {
                        if (eProps.ShellIsHotSide == true)
                            eProps.ShellProps.DT.SetValueIfChanged(HSIn.T - HSOut.T, SourceEnum.UnitOp, EXguid);
                        else
                            eProps.ShellProps.DT.SetValueIfChanged(CSOut.T - CSIn.T, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeShellTube.TubeSide:
                    {
                        if (eProps.ShellIsHotSide == true)
                            eProps.TubeProps.DT.SetValueIfChanged(CSOut.T - CSIn.T, SourceEnum.UnitOp, EXguid);
                        else
                            eProps.TubeProps.DT.SetValueIfChanged(HSIn.T - HSOut.T, SourceEnum.UnitOp, EXguid);
                        break;
                    }
                case ExchangerHeatBalanceTypeShellTube.ShellSideTSInlet:
                case ExchangerHeatBalanceTypeShellTube.TubeSideSSOutlet:
                case ExchangerHeatBalanceTypeShellTube.ShellSideTSOutlet:
                case ExchangerHeatBalanceTypeShellTube.TubeSideSSInlet:
                    {
                        if (eProps.ShellIsHotSide == true)
                        {
                            eProps.ShellProps.DT.SetValueIfChanged(HSIn.T - HSOut.T, SourceEnum.UnitOp, EXguid);
                            eProps.TubeProps.DT.SetValueIfChanged(CSOut.T - CSIn.T, SourceEnum.UnitOp, EXguid);
                        }
                        else
                        {
                            eProps.ShellProps.DT.SetValueIfChanged(CSOut.T - CSIn.T, SourceEnum.UnitOp, EXguid);
                            eProps.TubeProps.DT.SetValueIfChanged(HSIn.T - HSOut.T, SourceEnum.UnitOp, EXguid);
                        }
                        break;
                    }
            }
        }

        public Exchanger(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                eProps = (ExchangerProps)info.GetValue("ExchangerProps", typeof(ExchangerProps));
                option = (HeExCalcOptions)info.GetValue("option", typeof(HeExCalcOptions));
                EXguid = (Guid)info.GetValue("EXguid", typeof(Guid));
            }
            catch
            {
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ExchangerProps", eProps, typeof(ExchangerProps));
            info.AddValue("option", option, typeof(HeExCalcOptions));
            info.AddValue("EXguid", EXguid, typeof(Guid));
        }
    }

    public static class HeatTransferCalcs
    {
        public static double PrandtlNo(double HeatCapc, double Viscosity, double ThermalConductivity)
        {
            return HeatCapc * Viscosity / ThermalConductivity;
        }

        public static double ReynoldsNo(double Density, double Velocity, double ID, double visc)
        {
            return Density * Velocity * ID * 1000 / visc;
        }

        enum HeatTransferCorrelation { DittusBoelter, SiederTate }

        public static double NusseltNo(double jh, double ReynoldsNo, double PrandtlNo, bool heating)
        {
            HeatTransferCorrelation corr = HeatTransferCorrelation.DittusBoelter;
            switch (corr)
            {
                case HeatTransferCorrelation.DittusBoelter:
                    {
                        if (!heating)
                            return 0.023 * ReynoldsNo.Pow(0.8) * PrandtlNo.Pow(0.3); // cooling
                        else
                            return 0.023 * ReynoldsNo.Pow(0.8) * PrandtlNo.Pow(0.4); // heating
                    }
                case HeatTransferCorrelation.SiederTate:
                    {
                        return 0;
                    }
                default:
                    {
                        return jh * ReynoldsNo * PrandtlNo.Pow(0.33);
                    }
            }
        }

        // shells and even tube passes
        public static double Ft(double Th1, double Th2, double TC1, double TC2, int shells)
        {
            if (Th1 != double.NaN &&
                Th2 != double.NaN &&
                TC1 != double.NaN &&
                TC2 != double.NaN)
            {
                double P, R, X, Ft;
                try
                {
                    P = (TC2 - TC1) / (Th1 - TC1);
                    R = (Th1 - Th2) / (TC2 - TC1);
                    X = (1 - ((R * P - 1) / (P - 1)).Pow(1 / shells)) / (R - ((R * P - 1) / (P - 1)).Pow(1 / shells));
                    Ft = ((((R.Sqr() + 1).Sqrt()) / (R - 1)) * Math.Log((1 - X) / (1 - R * X))) / (Math.Log((2 / X - 1 - R + (R.Sqr() + 1).Sqrt()) / (2 / X - 1 - R - (R.Sqr() + 1).Sqrt())));
                }
                catch
                {
                    Ft = 1;
                }

                return Ft;
            }
            else
            {
                return double.NaN;
            }
        }

        public static double LMTD(double hsin, double hsout, double csin, double csout)
        {
            if (double.IsNaN(Math.Log((hsin - csout) / (hsout - csin))))
            {
                return (hsin - hsout + csout - csout) / 2;
            }
            else
            {
                return ((hsin - csout) - (hsout - csin)) / Math.Log((hsin - csout) / (hsout - csin));
            }
        }

        public static double LMTDft(double Th1, double Th2, double TC1, double TC2, int shells)
        {
            return LMTD(Th1, Th2, TC1, TC2) * Ft(Th1, Th2, TC1, TC2, shells);
        }

        public static double Q_UAExplicitTHint CIn(double UA, double MCP1, double MCP2, double TCin, double THin)
        {
            return (Math.Exp(UA / MCP2 - UA / MCP1) * (THin - TCin) + TCin - THin) / ((Math.Exp(UA / MCP2 - UA / MCP1) / MCP2 - 1 / MCP1));
        }
    }

    [Serializable]
    public class ExTubeShellProps : ISerializable
    {
        /*public  BasicProperty InC = new  BasicProperty(PropertyEnum.Temperature ),
            OutC = new  BasicProperty(PropertyEnum.Temperature );
        public  BasicProperty InP = new  BasicProperty(PropertyEnum.Pressure ),
            OutP = new  BasicProperty(PropertyEnum.Pressure );
        public  BasicProperty MassFlow = new  BasicProperty(PropertyEnum.MassFlow),
            Density = new  BasicProperty(PropertyEnum.Density),
            Visc = new  BasicProperty(PropertyEnum.KinViscosity),
            ThermaCond = new  BasicProperty(PropertyEnum.ThermalConductivity),

            Velocity = new  BasicProperty(PropertyEnum.Velocity);
        public  BasicProperty HeatCapacity = new  BasicProperty(PropertyEnum.HeatFlux);*/
        public BasicProperty DP = new BasicProperty(0.5, PropertyEnum.DeltaP, "bar", SourceEnum.Default);
        public BasicProperty DT = new BasicProperty(10, PropertyEnum.DeltaT, "C", SourceEnum.Default);
        public BasicProperty FoulingRes = new BasicProperty(0, PropertyEnum.HeatTransferResistace, SourceEnum.Default);

        public ExTubeShellProps()
        {
        }

        public ExTubeShellProps(SerializationInfo info, StreamingContext context)
        {
            try
            {
                DP = (BasicProperty)info.GetValue("DP", typeof(BasicProperty));
                DT = (BasicProperty)info.GetValue("DT", typeof(BasicProperty));
                FoulingRes = (BasicProperty)info.GetValue("FoulingRes", typeof(BasicProperty));
            }
            catch
            {
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("DP", DP, typeof(BasicProperty));
            info.AddValue("DT", DT, typeof(BasicProperty));
            info.AddValue("FoulingRes", FoulingRes, typeof(BasicProperty));
        }
    }

    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class ExchangerProps : ISerializable
    {
        public bool? ShellIsHotSide;
        public TubePattern tubepattern;
        public BasicProperty NoTubes = new BasicProperty(1, PropertyEnum.NullUnits, SourceEnum.Default);
        public BasicProperty TubeOuterDiam = new BasicProperty(2.54, PropertyEnum.Length, "cm", SourceEnum.Default);
        public BasicProperty NoShells = new BasicProperty(1, PropertyEnum.NullUnits, SourceEnum.Default);
        public BasicProperty NoTubePasses = new BasicProperty(2, PropertyEnum.NullUnits, SourceEnum.Default);
        public BasicProperty TubeThickness = new BasicProperty(2.11, PropertyEnum.Length, "mm", SourceEnum.Default);
        public BasicProperty TubeInnerDiam = new BasicProperty(2.33, PropertyEnum.Length, "cm", SourceEnum.Default);
        public BasicProperty TubeLength = new BasicProperty(6, PropertyEnum.Length, "m", SourceEnum.Default);
        public BasicProperty ShellDiam = new BasicProperty(0.5, PropertyEnum.Length, "m", SourceEnum.Default);
        public BasicProperty tubepitch = new BasicProperty(25, PropertyEnum.Length, "cm", SourceEnum.Default);
        public BasicProperty bafflespacing = new BasicProperty(20, PropertyEnum.Length, "cm", SourceEnum.Default);
        public BasicProperty Duty = new BasicProperty(0, PropertyEnum.EnergyFlow, "kJ/hr", SourceEnum.Default);
        public BasicProperty LMTD = new BasicProperty(10, PropertyEnum.Temperature, "C", SourceEnum.Default);
        public BasicProperty Ft = new BasicProperty(1, PropertyEnum.NullUnits, SourceEnum.Default);
        public BasicProperty LMTDCorr = new BasicProperty(10, PropertyEnum.Temperature, "C", SourceEnum.Default);
        public BasicProperty UA = new BasicProperty(10, PropertyEnum.OverallUa, SourceEnum.Default);
        public BasicProperty MinApproach = new BasicProperty(10, PropertyEnum.Temperature, "C", SourceEnum.Default);
        public BasicProperty Shellh = new BasicProperty(0, PropertyEnum.HeatFlux, SourceEnum.Default);
        public BasicProperty Tubeh = new BasicProperty(0, PropertyEnum.HeatFlux, SourceEnum.Default);
        public BasicProperty overallh = new BasicProperty(0, PropertyEnum.HeatFlux, SourceEnum.Default);
        public BasicProperty Bafflecut = new BasicProperty(30, PropertyEnum.Percentage, "%", SourceEnum.Default);
        public BasicProperty TransferArea = new BasicProperty(500, PropertyEnum.Area, "m2", SourceEnum.Default);
        public BasicProperty TubeVelocity = new BasicProperty(1, PropertyEnum.Velocity, "m/s", SourceEnum.Default);
        public BasicProperty ShellVelocity = new BasicProperty(1, PropertyEnum.Velocity, "m/s", SourceEnum.Default);
        public BasicProperty TubeSpacing = new BasicProperty(0.1, PropertyEnum.Length, "m", SourceEnum.Default);

        public ExTubeShellProps TubeProps = new ExTubeShellProps();
        public ExTubeShellProps ShellProps = new ExTubeShellProps();

        public ExchangerProps()
        {
        }

        public ExchangerProps(SerializationInfo info, StreamingContext context)
        {
            try
            {
                NoTubes = (BasicProperty)info.GetValue("NoTubes", typeof(BasicProperty));
                TubeOuterDiam = (BasicProperty)info.GetValue("TubeOuterDiam", typeof(BasicProperty));
                NoShells = (BasicProperty)info.GetValue("NoShells", typeof(BasicProperty));
                NoTubePasses = (BasicProperty)info.GetValue("NoTubePasses", typeof(BasicProperty));
                TubeThickness = (BasicProperty)info.GetValue("TubeThickness", typeof(BasicProperty));
                TubeInnerDiam = (BasicProperty)info.GetValue("TubeInnerDiam", typeof(BasicProperty));
                TubeLength = (BasicProperty)info.GetValue("TubeLength", typeof(BasicProperty));
                ShellDiam = (BasicProperty)info.GetValue("ShellDiam", typeof(BasicProperty));
                tubepitch = (BasicProperty)info.GetValue("tubepitch", typeof(BasicProperty));
                bafflespacing = (BasicProperty)info.GetValue("bafflespacing", typeof(BasicProperty));
                Duty = (BasicProperty)info.GetValue("Duty", typeof(BasicProperty));
                LMTD = (BasicProperty)info.GetValue("LMTD", typeof(BasicProperty));
                Ft = (BasicProperty)info.GetValue("Ft", typeof(BasicProperty));
                LMTDCorr = (BasicProperty)info.GetValue("LMTDCorr", typeof(BasicProperty));
                UA = (BasicProperty)info.GetValue("UA", typeof(BasicProperty));
                MinApproach = (BasicProperty)info.GetValue("MinApproach", typeof(BasicProperty));
                Shellh = (BasicProperty)info.GetValue("Shellh", typeof(BasicProperty));
                Tubeh = (BasicProperty)info.GetValue("tubeh", typeof(BasicProperty));
                overallh = (BasicProperty)info.GetValue("overallh", typeof(BasicProperty));
                Bafflecut = (BasicProperty)info.GetValue("Bafflecut", typeof(BasicProperty));
                TransferArea = (BasicProperty)info.GetValue("TransferArea", typeof(BasicProperty));
                TubeVelocity = (BasicProperty)info.GetValue("TubeVelocity", typeof(BasicProperty));
                ShellVelocity = (BasicProperty)info.GetValue("ShellVelocity", typeof(BasicProperty));
                TubeSpacing = (BasicProperty)info.GetValue("TubeSpacing", typeof(BasicProperty));

                TubeProps = (ExTubeShellProps)info.GetValue("TubeProps", typeof(ExTubeShellProps));
                ShellProps = (ExTubeShellProps)info.GetValue("ShellProps", typeof(ExTubeShellProps));
            }
            catch
            {
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("NoTubes", NoTubes, typeof(BasicProperty));
            info.AddValue("TubeOuterDiam", TubeOuterDiam, typeof(BasicProperty));
            info.AddValue("NoShells", NoShells, typeof(BasicProperty));
            info.AddValue("NoTubePasses", NoTubePasses, typeof(BasicProperty));
            info.AddValue("TubeThickness", TubeThickness, typeof(BasicProperty));
            info.AddValue("TubeInnerDiam", TubeInnerDiam, typeof(BasicProperty));
            info.AddValue("TubeLength", TubeLength, typeof(BasicProperty));
            info.AddValue("ShellDiam", ShellDiam, typeof(BasicProperty));
            info.AddValue("tubepitch", tubepitch, typeof(BasicProperty));
            info.AddValue("bafflespacing", bafflespacing, typeof(BasicProperty));
            info.AddValue("Duty", Duty, typeof(BasicProperty));
            info.AddValue("LMTD", LMTD, typeof(BasicProperty));
            info.AddValue("Ft", Ft, typeof(BasicProperty));
            info.AddValue("LMTDCorr", LMTDCorr, typeof(BasicProperty));
            info.AddValue("UA", UA, typeof(BasicProperty));
            info.AddValue("MinApproach", MinApproach, typeof(BasicProperty));
            info.AddValue("Shellh", Shellh, typeof(BasicProperty));
            info.AddValue("tubeh", Tubeh, typeof(BasicProperty));
            info.AddValue("overallh", overallh, typeof(BasicProperty));
            info.AddValue("Bafflecut", Bafflecut, typeof(BasicProperty));
            info.AddValue("TransferArea", TransferArea, typeof(BasicProperty));
            info.AddValue("TubeVelocity", TubeVelocity, typeof(BasicProperty));
            info.AddValue("ShellVelocity", ShellVelocity, typeof(BasicProperty));
            info.AddValue("TubeSpacing", TubeSpacing, typeof(BasicProperty));

            info.AddValue("TubeProps", TubeProps, typeof(ExTubeShellProps));
            info.AddValue("ShellProps", ShellProps, typeof(ExTubeShellProps));
        }
    }
}