using System;

namespace Extensions
{
    public static partial class Ext
    {
        public static double Exp2(double x)
        {
            var tmp = (long)(1512775 * x + 1072632447);
            return BitConverter.Int64BitsToDouble(tmp << 32);
        }

        public static double exp0(this double x)
        {
            x = 1.0 + x / 256.0;
            x *= x; x *= x; x *= x; x *= x;
            x *= x; x *= x; x *= x; x *= x;

            return x;
        }

        public static double exp01(this double x)
        {
            x = 1.0 + x * 0.00390625;
            x *= x; x *= x; x *= x; x *= x;
            x *= x; x *= x; x *= x; x *= x;

            return x;
        }

        public static double exp1(double x)
        {
            return (6 + x * (6 + x * (3 + x))) * 0.16666666f;
        }

        public static double exp2(double x)
        {
            return (24 + x * (24 + x * (12 + x * (4 + x)))) * 0.041666666f;
        }

        public static double exp3(double x)
        {
            return (120 + x * (120 + x * (60 + x * (20 + x * (5 + x))))) * 0.0083333333f;
        }

        public static double exp4(double x)
        {
            return (720 + x * (720 + x * (360 + x * (120 + x * (30 + x * (6 + x)))))) * 0.0013888888f;
        }

        public static double exp5(double x)
        {
            return (5040 + x * (5040 + x * (2520 + x * (840 + x * (210 + x * (42 + x * (7 + x))))))) * 0.00019841269f;
        }

        public static double exp6(double x)
        {
            return (40320 + x * (40320 + x * (20160 + x * (6720 + x * (1680 + x * (336 + x * (56 + x * (8 + x)))))))) * 2.4801587301e-5;
        }

        public static double exp7(double x)
        {
            return (362880 + x * (362880 + x * (181440 + x * (60480 + x * (15120 + x * (3024 + x * (504 + x * (72 + x * (9 + x))))))))) * 2.75573192e-6;
        }
    }
}