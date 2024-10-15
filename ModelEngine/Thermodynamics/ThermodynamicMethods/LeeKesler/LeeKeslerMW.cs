using System;
using Units.UOM;

namespace ModelEngine
{
    public partial class LeeKesler
    {
        public static double MW(Temperature meabp, double sg)  //=-12272.6+9486.4*D2+(8.3741-5.9917*D2)*K2+(1-0.77084*D2-0.02058*D2^2)*(0.7465-222.4666/K2)*10^7/K2+(1-0.80882*D2+0.02226*D2^2)*(0.3228-17.335/K2)*10^12/K2^3
        {
            double t = meabp.Kelvin;

            double MW = -12272.6 + 9486.4 * sg + (8.3741 - 5.9917 * sg) * t + (1 - 0.77084 * sg - 0.02058 * sg * sg)
                * (0.7465 - 222.4666 / t) * 1E7 / t + (1 - 0.80882 * sg + 0.02226 * sg * sg)
                * (0.3228 - 17.335 / t) * 1E12 / Math.Pow(t, 3);

            return MW;
        }
    }
}