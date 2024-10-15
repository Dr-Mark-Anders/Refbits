using System;
using Units.UOM;

namespace ModelEngine
{
    public class KLinearisation
    {
        public Data data;

        public double[] GetLinearisedKValues(Temperature T)
        {
            double[] res = new double[data.lnkGrad.Length];

            for (int i = 0; i < data.lnkGrad.Length; i++)
                res[i] = Math.Exp(data.LnK[i] + data.lnkGrad[i] * (T._Kelvin - data.T));

            return res;
        }

        public void Linearise(Components cc, Temperature Test, Pressure P, double[] XFractions, double[] YFractions, ThermoDynamicOptions thermo)
        {
            int NoComps = cc.Count;

            data.T = Test;

            data.LnK = ThermodynamicsClass.LnKMixArray(cc, P, Test, XFractions, YFractions, out enumFluidRegion state, thermo);
            data.Lnk2 = ThermodynamicsClass.LnKMixArray(cc, P, Test + 1, XFractions, YFractions, out state, thermo);

            data.lnKdelta = 1;

            data.lnkGrad = new double[NoComps];

            for (int CompNo = 0; CompNo < NoComps; CompNo++)
                data.lnkGrad[CompNo] = data.Lnk2[CompNo] - data.LnK[CompNo];
        }
    }

    public struct Data
    {
        public double[] LnK, Lnk2, lnkGrad;
        public double lnKdelta;
        public double T;
    }
}