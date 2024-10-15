using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    public class RiaziDaubert
    {
        public static double MW(Temperature meabp, double sg)  // api method
        {
            double MW = 20.486 * (Math.Exp(1.165 / 10000 * meabp.Rankine - 7.78712 * sg + 1.1582
                / 1000 * meabp.Rankine * sg)) * meabp.Rankine.Pow(1.26007) * sg.Pow(4.98308);
            return MW;
        }

        public static Temperature RD0_TCrit(double sg, Temperature Tb)
        {
            return new Temperature(19.06232 * Tb.Pow(0.58848) * sg.Pow(0.3596));
        }

        public static Temperature RD1_TCrit(double sg, Temperature Tb)
        {
            return new Temperature(9.5233 * (Math.Exp(-0.0009314 * Tb - 0.544442 * sg + 0.00064791 * Tb * sg))
                * Tb.Pow(0.81067) * sg.Pow(0.53691));
        }

        public static Temperature RD2_TCrit(double sg, Temperature Tb)
        {
            return new Temperature(35.9413 * Math.Exp(-0.00069 * Tb - 1.4442 * sg
                + 0.000491 * Tb * sg) * Tb.Pow(0.7293) * sg.Pow(1.2771));
        }

        public static double RD0_PCrit(double sg, Temperature Tb)
        {
            return 5.53027e7 * Tb.Pow(-2.3125) * sg.Pow(2.3201);  // bar
        }

        public static double RD1_PCrit(double sg, Temperature Tb)
        {
            return 319580 * (Math.Exp(-0.008505 * Tb - 4.8014 * sg +
                0.005749 * Tb * sg)) * Tb.Pow(-0.4844) * sg.Pow(4.0846);  // bar
        }

        public static double RD2_PCrit(double sg, Temperature Tb)
        {
            return 6.9575 * (Math.Exp(-0.0135 * Tb - 0.3129 * sg + 0.009174 * Tb * sg)) * Tb.Pow(0.6791) * sg.Pow(-0.6807);  // bar
        }
    }
}