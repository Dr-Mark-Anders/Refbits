using System;

namespace Math2
{
    public static class RegulaFalsi
    {
        public static double FalsiMethod(double s, double t, double e, int m, Func<double, double> f)
        {
            double r = double.NaN, fr;
            int n, side = 0;
            /* starting values at endPoints of interval */
            double fs = f(s);
            double ft = f(t);

            for (n = 0; n < m; n++)
            {
                r = (fs * t - ft * s) / (fs - ft);
                if (Math.Abs(t - s) < e * Math.Abs(t + s)) break;
                fr = f(r);

                if (fr * ft > 0)
                {
                    /* fr and ft have same sign, copy r to t */
                    t = r; ft = fr;
                    if (side == -1) fs /= 2;
                    side = -1;
                }
                else if (fs * fr > 0)
                {
                    /* fr and fs have same sign, copy r to s */
                    s = r; fs = fr;
                    if (side == +1) ft /= 2;
                    side = +1;
                }
                else
                {
                    /* fr * f_ very small (looks like zero) */
                    break;
                }
            }
            return r;
        }
    }
}