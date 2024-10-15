using Extensions;
using System;
using System.Collections.Generic;
using Units.UOM;

public enum eSplineMethod
{ Normal, Constrained }

namespace Math2
{
    public static class CubicSpline
    {
        /// <summary>
        /// Simple Cubic Spline Fitting Routine, original method
        /// </summary>
        ///
        public static double CubSpline(eSplineMethod Method, Temperature xi, List<Temperature> xx, List<double> yy)
        {
            if (!xi.IsKnown)
                return double.NaN;

            double[] temps = new double[xx.Count];

            for (int i = 0; i < xx.Count; i++)
            {
                temps[i] = xx[i].Celsius;
            }

            return CubSpline(Method, xi.Celsius, temps, yy.ToArray());
        }

        public static double CubSpline(eSplineMethod Method, double xi, List<double> xx, List<double> yy)
        {
            if (xi is double.NaN)
                return double.NaN;
            return CubSpline(Method, xi, xx.ToArray(), yy.ToArray());
        }

        public static double CubSpline(eSplineMethod Method, double xi, double[] xx, double[] yy)
        {
            if (xx is null || yy is null || xx.Length != yy.Length)
                return double.NaN;

            int i = 0;
            double yi = 0;
            double[] x = new double[xx.Length];
            double[] y = new double[xx.Length];
            double[] y2 = new double[xx.Length];

            for (i = 0; i < xx.Length; i++)
            {
                if (!double.IsNaN(xx[i]))
                    x[i] = xx[i];
                if (!double.IsNaN(yy[i]))
                    y[i] = yy[i];
            }

            if ((Method == eSplineMethod.Normal))
            {
                spline(x, y, x.Length, Math.Pow(10, 30), Math.Pow(10, 30), y2);// NR cubic spline
                splint(x, y, y2, x.Length, xi, out yi);// Get y
            }
            else if ((Method == eSplineMethod.Constrained))
            {
                if (xi is double.NaN)
                    return double.NaN;
                yi = SplineX3(xi, x, y); // Constrained cubic spline
            }

            return yi;
        }

        private static void spline(double[] x, double[] y, int N, double yp1, double ypn, double[] y2)
        {
            //Given arrays x(1:n) and y(1:n) containing a tabulated function, i.e., y i = f(xi), with
            //x1<x2< :::<xN , and given values yp1 and ypn for the first derivative of the int er-
            //polating function at point s 1 and n, respectively, this rout ine return  s an array y2(1:n) of
            //length n which contains the second derivatives of the int erpolating function at the tabulated
            //point s xi. if yp1 and/or ypn are equal to 1 * 10^30 or larger, the rout ine is signaled to set
            //the corresponding boundary condition for a natural spline, with zero second derivative on
            //that boundary.
            //Parameter: NMAX is the largest anticipated value of n.

            int Nmax = 500;
            double p, qn, sig, un;

            double[] u = new double[Nmax];

            //The lower boundary condition is set either to be natural
            if (yp1 > 9.9E+29)
            {
                y2[1] = 0D;
                u[0] = 0D;
            }
            else
            {
                //or else to have a specicied first derivative.
                y2[1] = -0.5;
                u[1] = (3D / (x[2] - x[1])) * ((y[2] - y[1]) / (x[2] - x[1]) - yp1);
            }

            //This is the decomposition loop of the tridiagonal
            //algorithm. y2 and u are used for temporary
            //storage of the decomposed factors.

            for (int i = 1; i < N - 1; i++)
            {
                sig = (x[i] - x[i - 1]) / (x[i + 1] - x[i - 1]);
                p = sig * y2[i - 1] + 2D;
                y2[i] = (sig - 1D) / p;
                u[i] = (6D * ((y[i + 1] - y[i]) / (x[i + 1] - x[i]) - (y[i] - y[i - 1]) / (x[i] - x[i - 1])) / (x[i + 1] - x[i - 1]) - sig * u[i - 1]) / p;
            }

            //The upper boundary condition is set either to be natural
            if (ypn > 9.9E+29)
            {
                qn = 0D;
                un = 0D;
            }
            else
            {
                //or else to have a specified first derivative.
                qn = 0.5;
                un = (3D / (x[N] - x[N - 1])) * (ypn - (y[N] - y[N - 1]) / (x[N] - x[N - 1]));
            }
            y2[N - 1] = (un - qn * u[N - 1]) / (qn * y2[N - 1] + 1D);

            //This is the backsubstitution loop of the tridiagonal algorithm.
            for (int k = N - 2; k >= 0; k--)
                y2[k] = y2[k] * y2[k + 1] + u[k];

            return;
        }

        private static void splint(double[] xa, double[] ya, double[] y2a, int N, double x, out double y)
        {
            //Given the arrays xa(1:n) and ya(1:n) of length n, which tabulate a function (with the
            //xai 's in order), and given the array y2a(1:n), which is the out put from spline above,
            //and given a value of x, this rout ine return  s a cubic-spline int erpolated value y.

            int k, khi, klo;
            double A, B, h;

            //We will the right place in the table by means of bisection.
            klo = 1;
            khi = N;

            while (khi - klo > 1)
            {
                k = (khi + klo) / 2;
                if (xa[k - 1] > x)
                    khi = k;
                else
                    klo = k;
            }

            //klo and khi now bracket the input value of x.
            h = xa[khi - 1] - xa[klo - 1];
            if (h == 0)
            {
                //   MarshalByRefobject //M.sh("bad xa input in splint ")
                //Cubic spline polynomial is now evaluated.
            }
            A = (xa[khi - 1] - x) / h;
            B = (x - xa[klo - 1]) / h;
            y = A * ya[klo - 1] + B * ya[khi - 1] + ((System.Math.Pow(A, 3) - A) * y2a[klo - 1] + (System.Math.Pow(B, 3) - B) * y2a[khi - 1]) * (h * h) / 6D;
        }

        private static double SplineX3(double x, double[] xx, double[] yy)
        {
            //|-------------------------------------------------------------------------------
            //| Function return  s y value for a corresponding x value, based on cubic spline.
            //| Will never oscillates or overshoot. No need to solve matrix.
            //| Also calculate constants for cubic in case  needed (for int egration).
            //|
            //| xx(0 to No_of_lines) is x values
            //|    * Must be unique (no two consequetive ones the same)
            //|    * Must be in ascending order
            //|    * No of lines = Number of point s - 1
            //| yy(0 to No_of_lines) is y values
            //|
            //| Uses function dxx to prevent div by zero.
            //|
            //| Developer: C Kruger, Guildford, UK
            //| Date: December 2001
            //|-------------------------------------------------------------------------------

            if (xx.Length < 2 || yy.Length < 2 || xx.Length != yy.Length)
                return double.NaN;

            int i, Nmax, Num;
            double[] gxx = new double[2];
            double[] ggxx = new double[2];
            //1st and 2nd derivative for left and right ends of line
            double A, B, C, D;
            //Number of lines = point s - 1
            Nmax = xx.Length - 1;

            //(1a) Find LineNumber or segment. Linear extrapolate if out side range.
            Num = 0;
            if (x < xx[0] || x > xx[Nmax])
            {
                //X out isde range. Linear int erpolate
                //Below min or max?
                if (x < xx[0])
                    Num = 1;
                else
                    Num = Nmax;
                {
                    B = (yy[Num] - yy[Num - 1]) / dxx(xx[Num], xx[Num - 1]);
                    A = yy[Num] - B * xx[Num];
                }
                return A + B * x;
            }
            else //(1b) Find LineNumber or segment. Linear extrapolate if out side range.
            {
                //X in range. Get line.
                for (i = 1; i <= Nmax; i++)
                {
                    if (x is double.NaN)
                        return double.NaN;
                    if (x <= xx[i])
                    {
                        Num = i;
                        break;
                    }
                }
            }

            //(2) Calc first derivative (slope) for int ermediate point s
            for (int j = 0; j <= 1; j++)          //Two point s around line
            {
                i = Num - 1 + j;
                if (i == 0 || i == Nmax)
                {
                    //Set very large slope at ends
                    gxx[j] = System.Math.Pow(10, 30);
                }
                else if (yy[i + 1] - yy[i] == 0 || yy[i] - yy[i - 1] == 0)
                    //Only check for 0 dy. dx assumed NEVER equals 0 !
                    gxx[j] = 0;
                else if ((xx[i + 1] - xx[i]) / (yy[i + 1] - yy[i]) + (xx[i] - xx[i - 1]) / (yy[i] - yy[i - 1]) == 0)    //Pos PLUS neg slope is 0. Prevent div by zero.
                    gxx[j] = 0;
                else if ((yy[i + 1] - yy[i]) * (yy[i] - yy[i - 1]) < 0)  //Pos AND neg slope, assume slope = 0 to prevent overshoot
                    gxx[j] = 0;
                else //Calculate an average slope for point  based on connecting lines
                    gxx[j] = 2 / (dxx(xx[i + 1], xx[i]) / (yy[i + 1] - yy[i]) + dxx(xx[i], xx[i - 1]) / (yy[i] - yy[i - 1]));
            }

            //(3) Reset first derivative (slope) at first and last point
            if (Num == 1) //First point  has 0 2nd derivative
                gxx[0] = 3 / 2D * (yy[Num] - yy[Num - 1]) / dxx(xx[Num], xx[Num - 1]) - gxx[1] / 2.0;
            if (Num == Nmax)//Last point  has 0 2nd derivative
            {
                gxx[1] = 3 / 2D * (yy[Num] - yy[Num - 1]) / dxx(xx[Num], xx[Num - 1]) - gxx[0] / 2.0;
            }

            //(4) Calc second derivative at point s
            ggxx[0] = -2D * (gxx[1] + 2 * gxx[0]) / dxx(xx[Num], xx[Num - 1]) + 6 * (yy[Num] - yy[Num - 1]) / dxx(xx[Num], xx[Num - 1]).Pow(2);
            ggxx[1] = 2D * (2 * gxx[1] + gxx[0]) / dxx(xx[Num], xx[Num - 1]) - 6 * (yy[Num] - yy[Num - 1]) / dxx(xx[Num], xx[Num - 1]).Pow(2);

            //(5) Calc constants for cubic
            D = 1 / 6D * (ggxx[1] - ggxx[0]) / dxx(xx[Num], xx[Num - 1]);
            C = 1 / 2D * (xx[Num] * ggxx[0] - xx[Num - 1] * ggxx[1]) / dxx(xx[Num], xx[Num - 1]);
            B = (yy[Num] - yy[Num - 1] - C * (xx[Num].Pow(2) - xx[Num - 1].Pow(2)) - D * (xx[Num].Pow(3) - xx[Num - 1].Pow(3))) / dxx(xx[Num], xx[Num - 1]);
            A = yy[Num - 1] - B * xx[Num - 1] - C * xx[Num - 1].Pow(2) - D * xx[Num - 1].Pow(3);

            //return   function
            double res = A + B * x + C * x.Pow(2) + D * x.Pow(3);
            return res;

            //'Alternative method based on Numerical Recipes.
            //'Shorter but does not calc cubic constants A, B, C, D
            //i = Num
            //A = (xx(i) - x) / (xx(i) - xx(i - 1))
            //B = 1 - A
            //Cy = 1 / 6 * (A ^ 3 - A) * (6 * (yy(i) - yy(i - 1)) - 2 * (gxx(i) + 2 * gxx(i - 1)) * (xx(i) - xx(i - 1)))
            //Dy = 1 / 6 * (B ^ 3 - B) * (2 * (2 * gxx(i) + gxx(i - 1)) * (xx(i) - xx(i - 1)) - 6 * (yy(i) - yy(i - 1)))
            //'return   function
            //SplineX3 = A * yy(i - 1) + B * yy(i) + Cy + Dy
        }

        private static double dxx(double x1, double x0)
        {
            //Calc Xi - Xi-1 to prevent div by zero

            double dxx = x1 - x0;
            if (dxx == 0)
                dxx = System.Math.Pow(10, 30);
            return dxx;
        }
    }
}