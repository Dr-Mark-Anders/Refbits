using System;
using Units.UOM;

namespace ModelEngine.ThermodynamicMethods.Activity_Models.NRTL
{
    internal class NRTL_FUNCTIONS
    {
        public double[] NRTL(double[] x, double[] Aij, double[] Aji, double[] alpha, double P, Temperature Tguess, double[] Ap, double[] Bp, double[] Cp)
        {
            Temperature T1, T2, T3;
            double[] y;

            T1 = Tguess;
            do
            {
                yError(x, Aij, Aji, alpha, P, T1, Ap, Bp, Cp, out y, out double yE1);
                T2 = T1 + 0.1;
                yError(x, Aij, Aji, alpha, P, T2, Ap, Bp, Cp, out y, out double yE2);
                T3 = T1 - yE1 * 0.1 / (yE2 - yE1);
                if (Math.Abs(T3 - T1) < 0.001)
                    break;
                else
                    T1 = T3;
            } while (true);
            return y;
        }

        public void yError(double[] x, double[] Aij, double[] Aji, double[] alpha, double P, Temperature T,
            double[] Ap, double[] Bp, double[] Cp, out double[] y, out double yE)
        {
            double[] Vp = new double[3];
            y = new double[3];
            double[,] A = new double[3, 3], tau = new double[3, 3], G = new double[3, 3], Alph = new double[3, 3];
            double Sum1, Sum2;
            double[] Term1 = new double[3], Sum3 = new double[3], gam = new double[3];
            double[,] Term2 = new double[3, 3];
            double[] lngam = new double[3];
            double R, TK;
            int i, j;
            R = 1.98721;
            TK = T + 273.15;

            for (i = 1; i <= 3; i++)
            {
                A[i, i] = 0;
            }
            A[2, 1] = Aji[1];
            A[3, 1] = Aji[2];
            A[3, 2] = Aji[3];
            A[1, 2] = Aij[1];
            A[1, 3] = Aij[2];
            A[2, 3] = Aij[3];

            for (i = 1; i <= 3; i++)
            {
                Alph[i, i] = 0;
            }

            Alph[2, 1] = alpha[1];
            Alph[3, 1] = alpha[2];
            Alph[3, 2] = alpha[3];
            Alph[1, 2] = alpha[1];
            Alph[1, 3] = alpha[2];
            Alph[2, 3] = alpha[3];

            for (i = 1; i <= 3; i++)
                for (j = 1; j <= 3; j++)
                    tau[i, j] = A[i, j] / R / TK;

            for (i = 1; i <= 3; i++)
                for (j = 1; j <= 3; j++)
                {
                    if (i == j)
                        G[i, i] = 1;
                    else
                        G[i, j] = Math.Exp(-Alph[i, j] * tau[i, j]);
                }

            for (i = 1; i <= 3; i++)
            {
                Sum1 = 0;

                for (j = 1; j <= 3; j++)
                    Sum1 = Sum1 + tau[j, i] * G[j, i] * x[j];

                Sum2 = 0;

                for (int l = 1; l <= 3; l++)
                    Sum2 = Sum2 + G[l, i] * x[l];

                Term1[i] = Sum1 / Sum2;

                for (int l = 1; l <= 3; l++)
                    Sum3[l] = 0;

                for (j = 1; j <= 3; j++)
                {
                    Sum1 = 0;
                    for (int l = 1; l <= 3; l++)
                        Sum1 = Sum1 + G[l, j] * x[l];

                    Sum2 = 0;
                    for (int n = 1; n <= 3; n++)
                        Sum2 = Sum2 + x[n] * tau[n, j] * G[n, j];

                    Term2[i, j] = x[j] * G[i, j] / Sum1 * (tau[i, j] - Sum2 / Sum1);
                    Sum3[i] = Sum3[i] + Term2[i, j];
                }

                lngam[i] = Term1[i] + Sum3[i];
                gam[i] = Math.Exp(lngam[i]);
            }

            for (i = 1; i <= 3; i++)
                Vp[i] = Math.Pow(10, (Ap[i] - Bp[i] / (T + Cp[i])) * 101325 / 760);

            Sum1 = 0;
            for (i = 1; i <= 3; i++)
                y[i] = x[i] * Vp[i] / P * gam[i];

            Sum1 = Sum1 + y[i];

            yE = 1 - Sum1;
        }
    }
}