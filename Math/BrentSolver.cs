using MathNet.Numerics.RootFinding;
using System;

namespace Math2
{
    public static class BrentSolver
    {
        public static double Solve(double a, double b, double value, Func<double, double> func, double sigma)
        {
            if (Brent.TryFindRoot(func, a, b, sigma, 100, out double res))
                return res;
            return double.NaN;
        }

        //formwikedia,buggy
        public static double SolveOld(double a, double b, double value, Func<double, double, double> func, double sigma)
        {
            //inputa,b,and(apoint erto)afunctionforf
            double c, s = double.NaN, d = double.NaN;
            double ares = func(a, value);
            double bres = func(b, value);
            double cres, sres = double.NaN;

            if (ares * bres >= 0)
                return double.NaN;

            if (ares < bres)
                Swap(ref a, ref b);

            c = a;
            cres = func(b, value);
            bool mflag = true;

            do
            {
                if (ares != cres && bres != cres)
                    s = a * bres * cres / ((ares - bres) * (ares - cres))  // (inversequadraticint erpolation)
                    + b * ares * cres / ((bres - ares) * (bres - cres))
                    + c * ares * bres / ((cres - ares) * (cres - bres));
                else
                    s = b - bres * (b - a) / (bres - ares);

                if (!Between(s, (3 * a + b) / 4, b) ||
                    mflag && Math.Abs(s - b) >= Math.Abs(b - c) / 2 ||
                        !mflag && Math.Abs(s - b) >= Math.Abs(c - d) / 2 ||
                            mflag && Math.Abs(b - c) < sigma ||
                               !mflag && Math.Abs(c - d) < sigma)
                {
                    s = (a + b) / 2;
                    mflag = true;
                }
                else
                    mflag = false;

                sres = func(s, value);
                d = c; // (disassignedforthefirsttimehere;itwon'tbeusedaboveonthefirstiterationbecausemflagisset)
                c = b;

                if (ares * sres < 0)
                    b = s;
                else
                    a = s;

                bres = func(b, value);
                if (ares < bres)
                    Swap(ref a, ref b);

                ares = func(a, value);
                bres = func(b, value);
                cres = func(c, value);
            } while (bres != 0 && sres != 0 && Math.Abs(b - a) > sigma);

            return b;
        }

        private static void Swap(ref double a, ref double b)
        {
            if (Math.Abs(a) < Math.Abs(b))
            {
                double c = a;
                a = b;
                b = c;
            }
        }

        private static bool Between(double X, double a, double b)
        {
            if (X > a && X < b)
                return true;
            return false;
        }
    }
}