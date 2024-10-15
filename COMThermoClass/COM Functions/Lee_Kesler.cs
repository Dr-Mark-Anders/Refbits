using ModelEngine;

namespace COMColumnNS
{
    public partial class COMThermo
    {
        public double LK_Z_Liquid(double Tr, double Pr, double Omega)
        {
            //Debugger.Launch();
            return LeeKesler.Z(Tr, Pr, Omega, enumFluidRegion.Liquid);
        }

        public double LK_Z_Vapour(double Tr, double Pr, double Omega)
        {
            //Debugger.Launch();
            return LeeKesler.Z(Tr, Pr, Omega, enumFluidRegion.Vapour);
        }

        public double LK_Z0_Vapour_Rig(double Tr, double Pr)
        {
            //Debugger.Launch();
            return LeeKesler.Z0_Rig(Tr, Pr, enumFluidRegion.Vapour);
        }

        public double LK_Z1_Vapour_Rig(double Tr, double Pr)
        {
            // Debugger.Launch();
            return LeeKesler.Z1_Rig(Tr, Pr, enumFluidRegion.Vapour);
        }

        public double LK_Z0_Liquid_Rig(double Tr, double Pr)
        {
            //Debugger.Launch();
            return LeeKesler.Z0_Rig(Tr, Pr, enumFluidRegion.Liquid);
        }

        public double LK_Z1_Liquid_Rig(double Tr, double Pr)
        {
            // Debugger.Launch();
            return LeeKesler.Z1_Rig(Tr, Pr, enumFluidRegion.Liquid);
        }

        public double LK_Z0_VapourTableInterpolate(double Tr, double Pr)
        {
            //Debugger.Launch();
            return LeeKesler.Z0_FromTable(Tr, Pr, enumFluidRegion.Vapour, out List<double> Ts, out List<double> Ps, out List<double> Zs);
        }

        public double LK_Z1_VapourTableInterpolate(double Tr, double Pr)
        {
            // Debugger.Launch();
            return LeeKesler.Z1_FromTable(Tr, Pr, enumFluidRegion.Vapour, out List<double> Ts, out List<double> Ps, out List<double> Zs);
        }

        public double LK_Z0_LiquidTableInterpolate(double Tr, double Pr)
        {
            //Debugger.Launch();
            return LeeKesler.Z0_FromTable(Tr, Pr, enumFluidRegion.Liquid, out List<double> Ts, out List<double> Ps, out List<double> Zs);
        }

        public double LK_Z1_LiquidTableInterpolate(double Tr, double Pr)
        {
            // Debugger.Launch();
            return LeeKesler.Z1_FromTable(Tr, Pr, enumFluidRegion.Liquid, out List<double> Ts, out List<double> Ps, out List<double> Zs);
        }

        public double LK_Z0_VapourBisectTable(double Tr, double Pr)
        {
            //Debugger.Launch();
            return LeeKesler.Z0_BisectTable(Tr, Pr, enumFluidRegion.Vapour);
        }

        public double LK_Z1_VapourBisectTable(double Tr, double Pr)
        {
            // Debugger.Launch();
            return LeeKesler.Z1_BisectTable(Tr, Pr, enumFluidRegion.Vapour);
        }

        public double LK_Z0_LiquidBisectTable(double Tr, double Pr)
        {
            //Debugger.Launch();
            return LeeKesler.Z0_BisectTable(Tr, Pr, enumFluidRegion.Liquid);
        }

        public double LK_Z1_LiquidBisectTable(double Tr, double Pr)
        {
            // Debugger.Launch();
            return LeeKesler.Z1_BisectTable(Tr, Pr, enumFluidRegion.Liquid);
        }

        public double LK_EnDep0(double Z, double Tr, double Pr)
        {
            //Debugger.Launch();
            return LeeKesler.EnthDep0(Z, Tr, Pr);
        }

        public double LK_EnDep1(double Z, double Tr, double Pr)
        {
            //Debugger.Launch();
            return LeeKesler.EnthDep1(Z, Tr, Pr);
        }

        public double LK_EnthDepLiquid(double Tr, double Pr, double Omega)
        {
            //Debugger.Launch();
            return LeeKesler.EnthDeparture(Tr, Pr, Omega, enumFluidRegion.Liquid);
        }

        public double LK_EnthDepVapour(double Tr, double Pr, double Omega)
        {
            //Debugger.Launch();
            return LeeKesler.EnthDeparture(Tr, Pr, Omega, enumFluidRegion.Vapour);
        }

        public double LK_Ideal_Enthalpy(double T, double P, double MeABP, double sg, double MW, int MM)
        {
            //Debugger.Launch();
            return LeeKesler.IdealVapEnthalpyAbsolute(T, P, MeABP, sg, MW, MM); ;
        }

        public double LK_VP(double T, double SG, double bp, double CritT, double CritP, double Omega)
        {
            //Debugger.Launch();
            return LeeKesler.Psat(T, SG, bp, CritT, CritP, Omega);
        }
    }
}