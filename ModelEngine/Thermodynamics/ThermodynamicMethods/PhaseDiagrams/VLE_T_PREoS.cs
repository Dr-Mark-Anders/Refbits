using System;
using System.Collections.Generic;

namespace ModelEngine
{
    public class VLE_T_PREoS
    {
        public const double PI = 3.14159265358979;

        private double[] yi = new double[5], xi = new double[5], lnPhiLi = new double[2], lnPhiVi = new double[2];
        private double[,] kijm = new double[5, 5], lijm = new double[5, 5];
        private double[] Tci = new double[5], Pci = new double[5], Mwi = new double[5], wi = new double[5], PhiLi = new double[5], PhiVi = new double[5], MolarRatio = new double[5];
        private double T;
        private double PhiV1, PhiV2, PhiL1, PhiL2;
        private double lnPhiL1inf, lnPhiV1inf;

        private double K1, K2;
        private double VL, VG;
        private string[] Substancei = new string[5];
        private double[] vvector = new double[14];

        private List<double[]> results = new List<double[]>();
        private double lnPhiL2pure;

        public VLE_T_PREoS()
        {
        }

        public void VLE_T(string[] Name, double[] MW, double[] Tc, double[] Pc, double[] w, double TinC, double Pstart, double Pend, double Pinc, out List<string[]> results)
        {
            results = new List<string[]>();

            if (MW.Length != 2)
            {
                results[0][15] = "Numerical error, terminated.";
                return;
            }

            int irow = 18;

            //--------- Error Handling -----------

            //     On Error GoTo calcerror
            //
            //--------- Error Handling -----------

            // Count the number of substances according to the existence of the critical Temperature  and substance name

            double Nsub = 2;

            Tci = Tc;
            Pci = Pc;
            Substancei = Name;
            Mwi = MW;
            wi = w;

            // Read in binary int eraction parameters, kij and lij

            for (int i = 0; i < Nsub; i++)
            {
                for (int j = 0; j < Nsub; j++)
                {
                    kijm[i, j] = 0;
                    lijm[i, j] = 0;
                }
            }

            // convert Temperature  from C to K

            T = TinC + 273.15;

            // convert Pressure   from MPa to Pa

            double P = Pstart * 1000000D;

            // check the validity of the minimum Pressure   set by the user.  Increase it to at least the vapor Pressure   + 100 Pa

            double Pmin = PengRobinson_V3.PSatPREOS(Tci[1], Pci[1], wi[1], TinC);

            if (P < Pmin)
                P = Pmin + 100D;

            double dp = Pinc * 1000000D;
            Pend = Pend * 1000000D;

            K1 = 0;
            K2 = 0;

            if (K1 == 0)
            {
                // supercritical component:

                xi[0] = 0D;
                xi[1] = 1D;

                lnPhiL1inf = PengRobinson_V3.LnPhiL_i(0, Tci, Pci, wi, kijm, lijm, T, P, xi);

                yi[0] = 0D;
                yi[1] = 1D;

                lnPhiV1inf = PengRobinson_V3.LnPhiV_i(0, Tci, Pci, wi, kijm, lijm, T, P, yi);

                if (lnPhiL1inf == lnPhiV1inf)
                    K1 = Math.Exp(lnPhiV1inf);
                else
                    K1 = Math.Exp(lnPhiL1inf) / Math.Exp(lnPhiV1inf);
            }

            if (K2 == 0)
            {
                // subcritical component:

                xi[0] = 0D;
                xi[1] = 1D;

                lnPhiL2pure = PengRobinson_V3.LnPhiL_i(1, Tci, Pci, wi, kijm, lijm, T, P, xi);

                yi[0] = 0D;
                yi[1] = 1D;

                lnPhiV1inf = PengRobinson_V3.LnPhiV_i(1, Tci, Pci, wi, kijm, lijm, T, P, yi);

                K2 = Math.Exp(lnPhiL2pure);
            }

            double Err_F1 = 0.000001;
            double Err_F2 = 0.000001;
            double F1, F2;

            double IFirst = 0;

            int KLoopCount, PLoopCount = 0;
            // Pressure   loop

            for (int loop_P = 1; loop_P <= 5000; loop_P++) //  used Rachford-Rice formulation
            {
                KLoopCount = 0;
                PLoopCount++;

                string[] result = new string[16];
                results.Add(result);

                for (int loop_K = 1; loop_K <= 5000; loop_K++)
                {
                    KLoopCount++;
                    xi[0] = Math.Abs((1D - K2) / (K1 - K2));
                    xi[1] = Math.Abs(1D - xi[0]);
                    yi[0] = Math.Abs(K1 * xi[0]);
                    yi[1] = Math.Abs(1D - yi[0]);

                    PengRobinson_V3.lnPhiV(Nsub, Tci, Pci, wi, kijm, lijm, T, P, yi, ref VG, lnPhiVi); // Calcualte Volume and fugacity
                    PengRobinson_V3.lnPhiL(Nsub, Tci, Pci, wi, kijm, lijm, T, P, xi, ref VL, lnPhiLi);

                    F1 = Math.Log(xi[0]) + lnPhiLi[0] - Math.Log(yi[0]) - lnPhiVi[0];
                    F2 = Math.Log(xi[1]) + lnPhiLi[1] - Math.Log(yi[1]) - lnPhiVi[1];

                    if (double.IsNaN(F1) || double.IsNaN(F2))
                        goto calcerror;

                    if (Math.Abs(F1) < Err_F1 && Math.Abs(F2) < Err_F2)
                    {
                        PhiV1 = Math.Exp(lnPhiVi[0]);
                        PhiV2 = Math.Exp(lnPhiVi[1]);
                        PhiL1 = Math.Exp(lnPhiLi[0]);
                        PhiL2 = Math.Exp(lnPhiLi[1]);

                        if (double.IsNaN(PhiV1) || double.IsNaN(PhiV2) || double.IsNaN(PhiL1) || double.IsNaN(PhiL2))
                            goto calcerror;

                        break;
                    }
                    else
                    {
                        K1 = Math.Exp(lnPhiLi[0] - lnPhiVi[0]);    // check for convergence
                        K2 = Math.Exp(lnPhiLi[1] - lnPhiVi[1]);

                        if (loop_P == 1 && K1 <= 1.05 && IFirst == 0)
                        {
                            K1 = 10D;
                            IFirst = 1;
                        }
                    }
                }

                irow = irow + 1;

                // Calculate the properties from the converged values
                // Vapor phase

                double fv1 = yi[0] * P * PhiV1;
                double fV2 = yi[1] * P * PhiV2;
                double MW_vapor = 0D;

                for (int i = 0; i < Nsub; i++)
                {
                    MW_vapor = MW_vapor + yi[i] * Mwi[i];
                }

                double rhoV = (1D / VG) * MW_vapor / 1000D;

                result[0] = (P / 1000000D).ToString();
                result[1] = yi[0].ToString();
                result[2] = yi[1].ToString();
                result[3] = fv1.ToString();
                result[4] = fV2.ToString();
                result[5] = rhoV.ToString();

                // Liquid phase

                double fL1 = xi[0] * P * PhiL1;
                double fL2 = xi[1] * P * PhiL2;
                double MW_liquid = 0D;

                for (int i = 0; i < Nsub; i++)
                {
                    MW_liquid = MW_liquid + xi[i] * Mwi[i];
                }

                double rhoL = (1D / VL) * MW_liquid / 1000D;

                result[6] = xi[0].ToString();
                result[7] = xi[1].ToString();
                result[8] = fL1.ToString();
                result[9] = fL2.ToString();
                result[10] = rhoL.ToString();

                // Calculations for analyses

                result[11] = (yi[0] / xi[0]).ToString();
                result[12] = (yi[1] / xi[1]).ToString();
                result[13] = (K2 / K1).ToString();
                result[14] = KLoopCount.ToString();

                double drhoLV = rhoL - rhoV;

                if (loop_P == 1)
                {
                    result[15] = "Number of substances:" + Nsub.ToString();
                }

                if (drhoLV < 0 && loop_P > 1)
                {
                    result[15] = "NOTE: L-V density inversion!";
                }

                if (KLoopCount > 30)
                    P = P + 0.1 * dp;
                else
                    P = P + dp;

                if (dp > 0 && P >= Pend)
                {
                    result[15] = "Upper Pressure   bound reached.";
                    return;
                }
                else if (dp < 0 && P <= Pend)
                {
                    result[15] = "Lower Pressure   bound reached.";
                    return;
                }

                if (Math.Abs(K1 / K2 - 1D) <= 0.06)
                {
                    result[15] = "K-values close to one.";
                    break;
                }
            }
            return;

        //--------- Error Handling -----------
        //     Terminates calculation if a numerical error occurs

        calcerror:
            if (irow <= 19)
                irow = 19;

            results[PLoopCount - 1][15] = "Numerical error, terminated.";
            return;

            //--------- Error Handling -----------
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Nsub"></param>
        /// <param name="Tci"></param>
        /// <param name="Pci"></param>
        /// <param name="wi"></param>
        /// <param name="kijm"></param>
        /// <param name="lijm"></param>
        /// <param name="T"></param>
        /// <param name="P"></param>
        /// <param name="yi"></param>
        /// <param name="VG"></param>
        /// <param name="lnPhiVi"></param>

        public double sign(double a, double b)
        {
            double sign;
            if (b <= 0)
            {
                sign = Math.Abs(a);
            }
            else
            {
                sign = -Math.Abs(a);
            }

            return sign;
        }
    }
}