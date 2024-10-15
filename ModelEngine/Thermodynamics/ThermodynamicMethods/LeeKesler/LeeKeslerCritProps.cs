using Extensions;
using Units.UOM;

namespace ModelEngine
{
    public partial class LeeKesler
    {
        //  =(5.689 - 0.0566 / H80 - (0.43639 + 4.1216 / H80 + 0.21343 / H80^2) * 0.001 *E80
        // + (0.47579 + 1.182 / H80 + 0.15302 / H80^2)* 0.000001 * H80^2 - (2.4505 + 9.9099 / H80 ^2) * 10^-10 * H80^3)*10
        public static Pressure PCrit(double sg, Temperature MeABP)
        {
            Pressure res = new Pressure();
            res.MPa = 5.689 - 0.0566 / sg - (0.43639 + 4.1216 / sg + 0.21343 / sg.Pow(2)) * 0.001 * MeABP.BaseValue
                + (0.47579 + 1.182 / sg + 0.15302 / sg / sg) * 0.000001 * sg.Pow(2) - (2.4505 + 9.9099 / sg / sg) * 0.0000000001 * sg.Pow(3);
            return res;
        }

        public static Temperature TCrit(double sg, Temperature MeABP)
        {
            Temperature res;
            res = new Temperature(189.8 + 450.6 * sg + (0.4244 + 0.1174 * sg) * MeABP
                + (0.1441 - 1.0069 * sg) * 100000 / MeABP);
            return res;
        }
    }
}