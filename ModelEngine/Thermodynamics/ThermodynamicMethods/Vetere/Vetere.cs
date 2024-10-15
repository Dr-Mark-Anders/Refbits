using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    public static class Vetere
    {
        private static double[] VetTC = new double[] { 1.60193, 0.00558, -0.00112, -0.52398, 0.00104, -0.06403, 0.93857, -0.00085, 0.28290 };
        private static double[] VetPC = new double[] { 10.74145, 0.07434, -0.00047, -2.10482, 0.00508, -1.18869, -0.66773, -0.01154, 1.53161 };

        public static Temperature CritT(double SG, Temperature BP, double MW)
        {  //=EXP(A+B*MW+C*BP+D*SG+E*BP*SG)*MW^F*BP^(G+H*MW)*SG^I-273.15
            Temperature res = new Temperature(Math.Exp(VetTC[0] + VetTC[1] * MW + VetTC[2] * BP + VetTC[3] * SG + VetTC[4] * BP * SG)
                * MW.Pow(VetTC[5]) * BP.Pow(VetTC[6] + VetTC[7] * MW) * SG.Pow(VetTC[8]));
            return res;
        }

        public static double CritP(double SG, Temperature BP, double MW)
        {
            double res = Math.Exp(VetPC[0] + VetPC[1] * MW + VetPC[2] * BP + VetPC[3] * SG + VetPC[4] * BP * SG)
                 * MW.Pow(VetPC[5]) * BP.Pow(VetPC[6] + VetPC[7] * MW) * SG.Pow(VetPC[8]);
            return res;
        }
    }
}