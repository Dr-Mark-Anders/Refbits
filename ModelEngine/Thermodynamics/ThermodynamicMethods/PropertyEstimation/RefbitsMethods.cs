using Extensions;
using Units.UOM;

namespace ModelEngine
{
    /// <summary>
    /// int ended for use with TWU above 650C, alkane series given up to BP = 850C
    /// </summary>
    public class RefbitsMethods
    {
        public static double SG(Temperature BP)
        {
            return 0.00000000078762 * BP.Celsius.Pow(3) - 0.0000014494 * BP.Celsius.Sqr() + 0.0009545 * BP.Celsius + 0.60415;
        }

        public static double MW(Temperature BP)
        {
            return 0.0000000000000669299 * BP.Celsius.Pow(6) - 0.000000000122397 * BP.Celsius.Pow(5) + 0.0000000902593 *
                BP.Celsius.Pow(4) - 0.0000313871 * BP.Celsius.Pow(3) + 0.00616949 * BP.Celsius.Pow(2) -
                0.0714894 * BP.Celsius + 69.0122;
        }

        public static double CriticalT(Temperature BP)
        {
            return BP.Celsius + 0.00000000034489 * BP.Celsius.Pow(4) - 0.00000012797 * BP.Celsius.Pow(3) - 0.00061944 * BP.Celsius.Sqr()
                + 0.20191 * BP.Celsius + 154.47;
        }

        public static double CriticalP(Temperature BP)
        {
            return -0.000000065917 * BP.Celsius.Pow(3) + 0.00014936 * BP.Celsius.Sqr() - 0.1204 * BP.Celsius + 37.918;
        }
    }
}