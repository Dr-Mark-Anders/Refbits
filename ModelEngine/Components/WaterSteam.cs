using Steam97;
using Units.UOM;

namespace ModelEngine
{
    public class WaterSteam : BaseComp
    {
        private StmPropIAPWS97 steam97 = new StmPropIAPWS97();

        public WaterSteam(BaseComp basecomp)
        {
            this.IsPure = basecomp.IsPure;
            this.name = basecomp.name;
            this.CritT = basecomp.CritT;
            this.CritP = basecomp.CritP;
            this.CritZ = basecomp.CritZ;
            this.CritV = basecomp.CritV;
            this.Omega = basecomp.Omega;
            this.AntK = basecomp.AntK;
            this.IdealVapCP = basecomp.IdealVapCP;
            this.Molwt = basecomp.Molwt;
            this.SG_60F = basecomp.SG_60F;
            this.BP = basecomp.BP;
            this.MoleFracVap = basecomp.MoleFracVap;
            this.RefEnthalpyDep1 = basecomp.RefEnthalpyDep1;
            this.RefEntropyDep1 = basecomp.RefEntropyDep1;
            this.RefEnthalpyDep3 = basecomp.RefEnthalpyDep3;
            this.HForm25 = basecomp.HForm25;
            this.GForm25 = basecomp.GForm25;
            this.MoleFraction = basecomp.MoleFraction;
            this.MassFraction = basecomp.MassFraction;
            this.STDLiqVolFraction = basecomp.STDLiqVolFraction;
            this.Formula = basecomp.Formula;
            this.CAS = basecomp.CAS;
            this.Properties = basecomp.Properties;
        }

        public double Phase(Pressure P, Temperature T)
        {
            Pressure Psat = steam97.Psat(T);
            if (P < Psat)
                return 1;
            else
                return 0;
        }

        public override double IdealGasCp(Temperature T, enumMassOrMolar mm)
        {
            return steam97.cppt(new Pressure(1), T);
        }

        public double ActLiqDensity(Pressure P, Temperature T)
        {
            return 1 / steam97.vpt(P, T);
        }

        internal HeatCapacity CP(Temperature T)
        {
            double res = steam97.cppt(new Pressure(1), T);
            return res * this.MW;
        }

        internal MassHeatCapacity CP_MASS(Pressure P, Temperature T)
        {
            double res = steam97.cppt(P, T);
            return res;
        }

        internal HeatCapacity CV(Temperature T)
        {
            double res = steam97.cvpt(new Pressure(1), T);
            return res * this.MW;
        }

        internal MassHeatCapacity CV_MASS(Temperature T)
        {
            double res = steam97.cvpt(new Pressure(1), T);
            return res;
        }

        internal InternalEnergy U(Pressure P, Temperature T)
        {
            double res = steam97.upt(P, T);
            return res;
        }

        internal InternalEnergy U_MASS(Pressure P, Temperature T)
        {
            double res = steam97.upt(P, T);
            return res;
        }

        internal Helmotz A_MASS(Pressure p, Temperature t)
        {
            return new Helmotz(U(p, t) - t.Kelvin * S_MASS(p, t));
        }

        internal Helmotz A(Pressure p, Temperature t)
        {
            return new Helmotz(U(p, t) - t.Kelvin * S_MASS(p, t)) * this.MW;
        }

        public MassEntropy S_MASS(Pressure p, Temperature t)
        {
            return steam97.spt(p, t);
        }

        public Entropy S(Pressure p, Temperature t)
        {
            return steam97.spt(p, t) * this.MW;
        }

        public MassEnthalpy H_MASS(Pressure p, Temperature t)
        {
            return steam97.hpt(p, t);
        }

        public Enthalpy H(Pressure p, Temperature t)
        {
            return steam97.hpt(p, t) * this.MW;
        }
    }
}