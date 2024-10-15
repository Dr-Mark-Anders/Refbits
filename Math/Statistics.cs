using System;
using System.Collections.Generic;
using System.Drawing;

namespace Math2
{
    public static class Probability
    {
        // returns the probability that the observed value of a standard normal random variable will be less than or equal to d
        public static double NormSDist(double d)
        {
            double erfHolder = Erf(d / Math.Sqrt(2.0));
            return (1 + erfHolder) / 2;
        }

        private static double Erf(double z)
        {
            double erfValue = z;
            double currentCoefficient = 1.0;
            int termCount = 50;
            for (int n = 1; n < termCount; n++)
            {
                currentCoefficient *= -1.0 * (2.0 * (double)n - 1.0) / ((double)n * (2.0 * (double)n + 1.0));
                erfValue += currentCoefficient * Math.Pow(z, (2 * n + 1));
            }
            return erfValue * (2.0 / Math.Sqrt(Math.PI));
        }

        // This function is a replacement for the Microsoft Excel Worksheet function NORMSINV.
        // It uses the algorithm of Peter J. Acklam to compute the inverse normal cumulative
        // distribution. Refer to http://home.online.no/~pjacklam/notes/invnorm/index.html for
        // a description of the algorithm.
        // Adapted to VB by Christian d'Heureuse, http://www.source-code.biz.
        public static double NormSInv(double p)
        {
            double NormSInv = 0;
            const double a1 = -39.6968302866538, a2 = 220.946098424521, a3 = -275.928510446969;
            const double a4 = 138.357751867269, a5 = -30.6647980661472, a6 = 2.50662827745924;
            const double b1 = -54.4760987982241, b2 = 161.585836858041, b3 = -155.698979859887;
            const double b4 = 66.8013118877197, b5 = -13.2806815528857, c1 = -7.78489400243029E-03;
            const double c2 = -0.322396458041136, c3 = -2.40075827716184, c4 = -2.54973253934373;
            const double c5 = 4.37466414146497, c6 = 2.93816398269878, d1 = 7.78469570904146E-03;
            const double d2 = 0.32246712907004, d3 = 2.445134137143, d4 = 3.75440866190742;
            const double p_low = 0.02425, p_high = 1 - p_low;
            double q, r;

            if (p < 0 || p > 1)
            {
                //Err.Raise vbobject Error, , "NormSInv: Argument out  of range.";
            }
            else if (p < p_low)
            {
                q = Math.Sqrt(-2 * Math.Log(p));
                NormSInv = (((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) /
                   ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
            }
            else if (p <= p_high)
            {
                q = p - 0.5;
                r = q * q;
                NormSInv = (((((a1 * r + a2) * r + a3) * r + a4) * r + a5) * r + a6) * q /
                    (((((b1 * r + b2) * r + b3) * r + b4) * r + b5) * r + 1);
            }
            else
            {
                q = Math.Sqrt(-2 * Math.Log(1 - p));
                NormSInv = -(((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) /
                    ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
            }
            return NormSInv;
        }

        // Find the least squares linear fit.
        // return   the total error.
        public static double FindLinearLeastSquaresFit(
            List<PointF> points, out double m, out double b)
        {
            // Perform the calculation.
            // Find the values S1, Sx, Sy, Sxx, and Sxy.
            double S1 = points.Count;
            double Sx = 0;
            double Sy = 0;
            double Sxx = 0;
            double Sxy = 0;
            foreach (PointF pt in points)
            {
                Sx += pt.X;
                Sy += pt.Y;
                Sxx += pt.X * pt.X;
                Sxy += pt.X * pt.Y;
            }

            // Solve for m and b.
            m = (Sxy * S1 - Sx * Sy) / (Sxx * S1 - Sx * Sx);
            b = (Sxy * Sx - Sy * Sxx) / (Sx * Sx - S1 * Sxx);

            return Math.Sqrt(ErrorSquared(points, m, b));
        }

        // return   the error squared.
        public static double ErrorSquared(List<PointF> points,
            double m, double b)
        {
            double total = 0;
            foreach (PointF pt in points)
            {
                double dy = pt.Y - (m * pt.X + b);
                total += dy * dy;
            }
            return total;
        }
    }
}