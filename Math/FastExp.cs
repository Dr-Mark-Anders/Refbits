using System;

namespace Math2
{
    public class FastExp
    {
        public static double Exp(double x)
        {
            var tmp = (long)(1512775 * x + 1072632447);
            return BitConverter.Int64BitsToDouble(tmp << 32);
        }
    }
}