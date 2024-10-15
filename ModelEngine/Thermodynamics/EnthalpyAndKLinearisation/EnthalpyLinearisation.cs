using Steam97;
using System;
using System.Runtime.CompilerServices;
using Units;
using Units.UOM;

//=C_*(TC_PC)*(Tr^(D_/(Tr^0.5))/TrStar_^(D_/(TrStar_^0.5)))
namespace ModelEngine       //=(LN(C_*(TC_PC))-LN(Vdep2))/(1/TrStar_^0.5*LN(TrStar_)-1/Tr^0.5*LN(Tr))
{
    public class EnthalpyDepartureLinearisation
    {
        public double EnthForm25Liquid, EnthForm25Vapour;
        public LinearVapData dataVapour;
        public LinearLiqData dataLiquid;

        public void VapUpdate(Components cc, double[] X, Pressure P, Temperature Tbase, Temperature Testimate, ThermoDynamicOptions thermo)
        {
            double HDep1 = 0, HDep2 = 0;
            double Trsqrt, TCritMix, PCritMix, TC_PC;

            double[] XDry = cc.RemoveWater(X, out double watermolefrac, in cc.WaterLocation);

            TCritMix = cc.TCritMix(XDry);
            PCritMix = cc.PCritMix(XDry);
            TC_PC = TCritMix / PCritMix;

            double Tr = Tbase / TCritMix;
            double TrStar = Testimate / TCritMix;
            Trsqrt = Math.Sqrt(Tr);

            //ThermoProps thermo1 = ThermodynamicsClass.BulkStreamThermo(cc, XDry, P, Tbase, enumFluidRegion.Vapour, thermo);
            HDep1 = -EnthalpDepClass.HDeparture(cc,XDry, P,Tbase,enumFluidRegion.Vapour, thermo);
            HDep2 = -EnthalpDepClass.HDeparture(cc, XDry, P, Testimate, enumFluidRegion.Vapour, thermo);

            /* if (thermo1 is not null)
                 HDep1 = -thermo1.H_higm;
             else
                 HDep1 = double.NaN;*/

            //ThermoProps thermo2 = ThermodynamicsClass.BulkStreamThermo(cc, XDry, P, Testimate, enumFluidRegion.Vapour, thermo);

            /*if (thermo2 is not null)
                HDep2 = -thermo2.H_higm;
            else
                HDep2 = double.NaN;*/

            dataVapour.BaseHDep = HDep1;
            dataVapour.D = (Math.Log(HDep1) - Math.Log(HDep2))
                / (1 / Math.Sqrt(TrStar) * Math.Log(TrStar) - 1 / Trsqrt * Math.Log(Tr));

            dataVapour.Tr = Tr;

            EnthForm25Vapour = ThermodynamicsClass.EnthalpyFormation25(cc, X);
        }

        public void LiqUpdate(Components cc, double[] X, Pressure P, Temperature Tbase, Temperature Testimate, ThermoDynamicOptions thermo)
        {
            double HDep1, HDep2;
            double Trsqrt, TCritMix, PCritMix, TC_PC;

            double[] XDry = cc.RemoveWater(X, out double watermolefrac, in cc.WaterLocation);

            TCritMix = cc.TCritMix(XDry);
            PCritMix = cc.PCritMix(XDry);
            TC_PC = TCritMix / PCritMix;

            double Tr = Tbase / TCritMix;
            double TrStar = Testimate / TCritMix;
            Trsqrt = Math.Sqrt(Tr);

            //HDep1 = -ThermodynamicsClass.BulkStreamThermo(cc, XDry, P, Tbase, enumFluidRegion.Liquid, thermo).H_higm;
            //HDep2 = -ThermodynamicsClass.BulkStreamThermo(cc, XDry, P, Testimate, enumFluidRegion.Liquid, thermo).H_higm;
            HDep1 = -EnthalpDepClass.HDeparture(cc, XDry, P, Tbase, enumFluidRegion.Liquid, thermo);
            HDep2 = -EnthalpDepClass.HDeparture(cc, XDry, P, Testimate, enumFluidRegion.Liquid, thermo);

            dataLiquid.BaseHDep = HDep1;
            dataLiquid.D = (Math.Log(HDep1) - Math.Log(HDep2))
                / (1 / Math.Sqrt(TrStar) * Math.Log(TrStar) - 1 / Math.Sqrt(Tr) * Math.Log(Tr));

            dataLiquid.Tr = Tr;

            EnthForm25Liquid = ThermodynamicsClass.EnthalpyFormation25(cc, X);
        }

        public double VapDepEstimate(Components cc, double[] Y, Temperature T)
        {
            double[] XDry = cc.RemoveWater(Y, out double watermolefrac, in cc.WaterLocation);
            double TrStar = T / cc.TCritMix(XDry);
            return dataVapour.BaseHDep *
                Math.Pow(dataVapour.Tr, (dataVapour.D / Math.Sqrt(dataVapour.Tr)))
                / Math.Pow(TrStar, dataVapour.D / (Math.Sqrt(TrStar)));
        }

       /* public Enthalpy VapEnthalpyOld(Components cc, double[] Y, Pressure P, Temperature T)
        {
            double[] XDry = cc.RemoveWater(Y, out double watermolefrac, in cc.WaterLocation);
            double IDealGas = ThermodynamicsClass.StreamIdealGasMolarEnthalpy(cc, T, XDry);
            double EDep = VapDepEstimate(cc, XDry, T);
            double EFormation = ThermodynamicsClass.EnthalpyFormation25(cc, XDry);
            double result = IDealGas - EDep + EFormation;

            if (watermolefrac == 0)
                return result;

            ThermoProps water = StmPropIAPWS97.WaterPropsMolar(P, T, enumSatType.Normal);
            result = result * (1 - watermolefrac) + water.H * watermolefrac;

            return result;
        }*/

        public Enthalpy VapEnthalpy(Components cc, double[] Y, Pressure P, Temperature T)
        {
            double[] XDry = cc.RemoveWater(Y, out double watermolefrac, in cc.WaterLocation);
            double IDealGas = IdealGas.StreamIdealGasMolarEnthalpyAndFormation(cc, T, XDry);
            double EDep = VapDepEstimate(cc, XDry, T);
            //double EFormation = ThermodynamicsClass.EnthalpyFormation25(cc, XDry);
            double result = IDealGas - EDep;

            if (watermolefrac == 0)
                return result;

            ThermoProps water = StmPropIAPWS97.WaterPropsMolar(P, T, enumSatType.Normal);
            result = result * (1 - watermolefrac) + water.H * watermolefrac;

            return result;
        }

        public double LiqDepEstimate(Components cc, double[] X, Temperature T)
        {
            double[] XDry = cc.RemoveWater(X, out double watermolefrac, in cc.WaterLocation);
            double TrStar = T / cc.TCritMix(XDry);
            return dataLiquid.BaseHDep *
                Math.Pow(dataLiquid.Tr, (dataLiquid.D / Math.Sqrt(dataLiquid.Tr)))
                / Math.Pow(TrStar, dataLiquid.D / (Math.Sqrt(TrStar)));
        }

        public Enthalpy LiqEnthalpyOld(Components cc, double[] X, Pressure P, Temperature T)
        {
            double[] XDry = cc.RemoveWater(X, out double watermolefrac,in cc.WaterLocation);
            double IDealGas = IdealGas.StreamIdealGasMolarEnthalpy(cc, T, XDry);
            double EDep = LiqDepEstimate(cc, XDry, T);
            double EFormation = ThermodynamicsClass.EnthalpyFormation25(cc, XDry);
            double result = IDealGas - EDep + EFormation;

            if (watermolefrac == 0)
                return result;

            ThermoProps water = StmPropIAPWS97.WaterPropsMolar(P, T, enumSatType.Normal);

            result = result * (1 - watermolefrac) + water.H * watermolefrac;

            return result;
        }

        public Enthalpy LiqEnthalpy(Components cc, double[] X, Pressure P, Temperature T)
        {
            double[] XDry = cc.RemoveWater(X, out double watermolefrac, in cc.WaterLocation);
            double IDealGas = IdealGas.StreamIdealGasMolarEnthalpyAndFormation(cc, T, XDry);
            double EDep = LiqDepEstimate(cc, XDry, T);
            //double EFormation = ThermodynamicsClass.EnthalpyFormation25(cc, XDry);
            double result = IDealGas - EDep;

            if (watermolefrac == 0)
                return result;

            ThermoProps water = StmPropIAPWS97.WaterPropsMolar(P, T, enumSatType.Normal);

            result = result * (1 - watermolefrac) + water.H * watermolefrac;

            return result;
        }
    }

    public class EnthalpySimpleLinearisation
    {
        public double EnthForm25;
        public LinearVapData dataVapour;
        public LinearLiqData dataLiquid;

        public double VapEstimate(Temperature T, double MW)
        {
            return dataVapour.BaseValue + dataVapour.Grad * (T.Kelvin - dataVapour.BaseT) * MW / dataVapour.BaseMW;
        }

        public double LiqEstimate(Temperature T, double MW)
        {
            return dataLiquid.BaseValue + dataLiquid.Grad * (T.Kelvin - dataLiquid.BaseT) * MW / dataLiquid.BaseMW;
        }

        public void VapUpdate(Components cc, double[] Y, Pressure P, Temperature T, Temperature Testimate, ThermoDynamicOptions thermo)
        {
            ThermoProps propsbase = ThermodynamicsClass.BulkStreamThermo(cc, Y, P, T, enumFluidRegion.Vapour, thermo);
            if (propsbase != null)
                dataVapour.BaseValue = propsbase.H;

            ThermoProps propsestimate = ThermodynamicsClass.BulkStreamThermo(cc, Y, P, Testimate, enumFluidRegion.Vapour, thermo);
            if (propsestimate != null)
                dataVapour.Grad = (propsestimate.H - propsbase.H) / (Testimate - T);
            dataVapour.BaseT = T;
            dataVapour.BaseMW = cc.MW(Y);
        }

        public void LiqUpdate(Components cc, double[] X, Pressure P, Temperature T, Temperature Testimate, ThermoDynamicOptions thermo)
        {
            ThermoProps propsbase = ThermodynamicsClass.BulkStreamThermo(cc, X, P, T, enumFluidRegion.Liquid, thermo);
            if (propsbase != null)
                dataLiquid.BaseValue = propsbase.H;

            ThermoProps propsestimate = ThermodynamicsClass.BulkStreamThermo(cc, X, P, Testimate, enumFluidRegion.Liquid, thermo);
            if (propsestimate != null)
                dataLiquid.Grad = (propsestimate.H - propsbase.H) / (Testimate - T);

            dataLiquid.BaseT = T;
            dataLiquid.BaseMW = cc.MW(X);
        }
    }

    public struct LinearVapData
    {
        public double BaseHDep, D, Tr, BaseValue, BaseT, Grad, BaseMW;
    }

    public struct LinearLiqData
    {
        public double BaseHDep, D, Tr, BaseValue, BaseT, Grad, BaseMW;
    }
}