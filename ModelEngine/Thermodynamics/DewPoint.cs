using Extensions;
using System.Linq;
using Units.UOM;

namespace ModelEngine
{
    public class DewPointClass
    {
        private Components cc;
        private Pressure P;
        private double[] LiqMolFracs;

        public DewPointClass(Components cc, Pressure P)
        {
            this.cc = cc;
            this.P = P;
            LiqMolFracs = new double[cc.Count];
            int MinLoc = cc.MaxNonZeroBoiling;
            LiqMolFracs[MinLoc] = 1;
        }

        public double DewFunc(double T, out double[] Kn)
        {
            Kn = ThermodynamicsClass.KMixArray(cc, P, T, LiqMolFracs, cc.MoleFractions, out _, cc.Thermo, false);

            if (Kn is null)
                return double.NaN;

            var KnDimSum = cc.MoleFractions.Divide(Kn).Sum();

            if (double.IsInfinity(KnDimSum))
                return 1e9;

            return KnDimSum;
        }
    }

    public class BoilingPointClass
    {
        private Components cc;
        private Pressure P;
        private double[] VapMolFracs;

        public BoilingPointClass(Components cc, Pressure P)
        {
            this.cc = cc;
            this.P = P;
            VapMolFracs = new double[cc.Count];
            int MaxLoc = cc.MinNonZeroBoiling;
            VapMolFracs[MaxLoc] = 1;
        }

        public double BoilingPointFunc(double T, out double[] Kn, out enumFluidRegion state)
        {
            Kn = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, VapMolFracs, out state, cc.Thermo, false);

            if (Kn is null)
                return double.NaN;

            var KnDimSum = cc.MoleFractions.Mult(Kn).Sum();

            if (double.IsInfinity(KnDimSum))
                return 1e9;

            return KnDimSum;
        }
    }
}