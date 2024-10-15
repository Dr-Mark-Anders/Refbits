using System;

namespace ModelEngine
{
    // Based on code from RL Smith VLE spreadheet

    public class PengRobinson_V3
    {
        private double[] yi = new double[5];
        private static double[,] kijm, lijm;
        private double[] Mwi = new double[5], Tci = new double[5], Pci = new double[5], wi = new double[5];
        private double[] PhiLi = new double[5], PhiVi = new double[5];

        public PengRobinson_V3()
        {
        }

        public static double K1inf(double[] Tci, double[] Pci, double[] wi, double[,] kijm, double[,] lijm, double TinC, double P)
        {
            double T = TinC + 273.15;
            double[] xi = new double[2], yi = new double[2];
            double lnPhiL1inf = 0, lnPhiV1inf = 0;

            xi[1] = 0D;
            xi[2] = 1D;

            lnPhiL1inf = LnPhiL_i(1, Tci, Pci, wi, kijm, lijm, T, P, xi);

            yi[1] = 0D;
            yi[2] = 1D;

            lnPhiV1inf = LnPhiV_i(1, Tci, Pci, wi, kijm, lijm, T, P, yi);

            double res;

            if (lnPhiL1inf == lnPhiV1inf)
                res = Math.Exp(lnPhiV1inf);
            else
                res = Math.Exp(lnPhiL1inf) / Math.Exp(lnPhiV1inf);

            return res;
        }

        public static void K(int NoComps, double[] Tc, double[] Pc, double[] w, double TinK, double[] xi, double[] yi, out double[] results, double Pstream)
        {
            results = new double[NoComps];
            double lnPhiL1inf = 0, lnPhiV1inf = 0;
            kijm = new double[NoComps, NoComps];
            lijm = new double[NoComps, NoComps];

            for (int i = 0; i < NoComps; i++)
            {
                for (int j = 0; j < NoComps; j++)
                {
                    kijm[i, j] = 0;
                    lijm[i, j] = 0;
                }
            }

            // convert Pressure   from MPa to Pa

            double P = Pstream * 100000D;

            double K1 = 0;

            for (int i = 0; i < NoComps; i++)
            {
                lnPhiL1inf = LnPhiL_i(i, Tc, Pc, w, kijm, lijm, TinK, P, xi);
                lnPhiV1inf = LnPhiV_i(i, Tc, Pc, w, kijm, lijm, TinK, P, yi);

                if (lnPhiL1inf == lnPhiV1inf)
                    K1 = Math.Exp(lnPhiV1inf);
                else
                    K1 = Math.Exp(lnPhiL1inf) / Math.Exp(lnPhiV1inf);

                results[i] = K1;
            }

            return;
        }

        public static void Kmix(int NoComps, double[] Tc, double[] Pc, double[] w, double TinK, double[] xi, double[] yi, out double[] results, double Pstream)
        {
            results = new double[NoComps];
            double[] lnPhiL1inf = new double[NoComps];
            double[] lnPhiV1inf = new double[NoComps];
            double VL = 0, VV = 0;

            for (int i = 0; i < NoComps; i++)
            {
                for (int j = 0; j < NoComps; j++)
                {
                    kijm[i, j] = 0;
                    lijm[i, j] = 0;
                }
            }

            // convert Pressure   from MPa to Pa
            double P = Pstream * 100000D;

            lnPhiV(NoComps, Tc, Pc, w, kijm, lijm, TinK, P, xi, ref VV, lnPhiV1inf);
            lnPhiL(NoComps, Tc, Pc, w, kijm, lijm, TinK, P, xi, ref VL, lnPhiL1inf);

            for (int i = 0; i < NoComps; i++)
            {
                if (lnPhiL1inf[i] == lnPhiV1inf[i])
                    results[i] = Math.Exp(lnPhiV1inf[i]);
                else
                    results[i] = Math.Exp(lnPhiL1inf[i]) / Math.Exp(lnPhiV1inf[i]);
            }
            return;
        }

        public static void lnPhiV(double Nsub, double[] Tci, double[] Pci, double[] wi, double[,] kijm, double[,] lijm, double TinK, double P, double[] yi, ref double VG, double[] lnPhiVi)
        {
            const double R = 8.314;
            double RT = R * TinK;
            double SQRT2 = Math.Sqrt(2D);
            double AAi, BBi;

            double aalpha = amix(Tci, Pci, wi, yi, kijm, TinK);
            double bmix = PengRobinson_V3.bmix(Tci, Pci, yi, lijm);

            VG = VGPR(TinK, P, aalpha, bmix);

            double ZG = P * VG / RT;
            double AA = aalpha * P / Math.Pow(RT, 2);
            double BB = bmix * P / RT;
            for (int i_index = 0; i_index < Nsub; i_index++)
            {
                AAi = damixdni_i(i_index, Tci, Pci, wi, yi, kijm, TinK) * P / Math.Pow(RT, 2);
                BBi = dbmixdni(i_index, Tci, Pci, yi, lijm) * P / RT;

                lnPhiVi[i_index] = BBi / BB * (ZG - 1D) - Math.Log(ZG - BB) - AA / (2D * SQRT2 * BB) * (AAi / AA - BBi / BB) * Math.Log((ZG + (1D + SQRT2) * BB) / (ZG + (1D - SQRT2) * BB));
            }
        }

        public static void lnPhiL(double Nsub, double[] Tci, double[] Pci, double[] wi, double[,] kijm, double[,] lijm, double TinK, double P, double[] xi, ref double VL, double[] lnPhiLi)
        {
            double R = 8.314;
            double RT = R * TinK;
            double SQRT2 = Math.Sqrt(2D);
            double aalpha = amix(Tci, Pci, wi, xi, kijm, TinK);
            double bmix = PengRobinson_V3.bmix(Tci, Pci, xi, lijm);
            VL = VLPR(TinK, P, aalpha, bmix);

            double Zl = P * VL / RT;
            double AA = aalpha * P / Math.Pow(RT, 2);
            double BB = bmix * P / RT;

            double AAi, BBi;

            for (int i_index = 0; i_index < Nsub; i_index++)
            {
                AAi = damixdni_i(i_index, Tci, Pci, wi, xi, kijm, TinK) * P / Math.Pow(RT, 2);
                BBi = dbmixdni(i_index, Tci, Pci, xi, lijm) * P / RT;

                lnPhiLi[i_index] = BBi / BB * (Zl - 1D) - Math.Log(Zl - BB) - AA / (2D * SQRT2 * BB) * (AAi / AA - BBi / BB) * Math.Log((Zl + (1D + SQRT2) * BB) / (Zl + (1D - SQRT2) * BB));
            }
        }

        public static double LnPhiV_i(int i_index, double[] Tci, double[] Pci, double[] wi, double[,] kijm, double[,] lijm, double TinK, double PinPa, double[] xyi)
        {
            double R = 8.314;
            double P = PinPa;
            double RT = R * TinK;
            double SQRT2 = Math.Sqrt(2d);
            double aalpha = amix(Tci, Pci, wi, xyi, kijm, TinK);
            double bmix = PengRobinson_V3.bmix(Tci, Pci, xyi, lijm);
            double VG = VGPR(TinK, PinPa, aalpha, bmix);

            double ZG = P * VG / RT;
            double AA = aalpha * P / Math.Pow(RT, 2);
            double BB = bmix * P / RT;
            double AAi = damixdni_i(i_index, Tci, Pci, wi, xyi, kijm, TinK) * P / Math.Pow(RT, 2);
            double BBi = dbmixdni(i_index, Tci, Pci, xyi, lijm) * P / RT;

            double res = BBi / BB * (ZG - 1D) - Math.Log(ZG - BB) - AA / (2D * SQRT2 * BB) * (AAi / AA - BBi / BB) * Math.Log((ZG + (1D + SQRT2) * BB) / (ZG + (1D - SQRT2) * BB));

            return res;
        }

        public static double LnPhiL_i(int i_index, double[] Tci, double[] Pci, double[] wi, double[,] kijm, double[,] lijm, double TinK, double PinPa, double[] xyi)
        {
            double R = 8.314;
            double P = PinPa;
            double RT = R * TinK;
            double SQRT2 = Math.Sqrt(2d);
            double aalpha = amix(Tci, Pci, wi, xyi, kijm, TinK);
            double bmix = PengRobinson_V3.bmix(Tci, Pci, xyi, lijm);
            double VL = VLPR(TinK, PinPa, aalpha, bmix);

            double Zl = P * VL / RT;
            double AA = aalpha * P / Math.Pow(RT, 2);
            double BB = bmix * P / RT;
            double AAi = damixdni_i(i_index, Tci, Pci, wi, xyi, kijm, TinK) * P / Math.Pow(RT, 2);
            double BBi = dbmixdni(i_index, Tci, Pci, xyi, lijm) * P / RT;

            double res = BBi / BB * (Zl - 1D) - Math.Log(Zl - BB) - AA / (2D * SQRT2 * BB) * (AAi / AA - BBi / BB) * Math.Log((Zl + (1D + SQRT2) * BB) / (Zl + (1D - SQRT2) * BB));

            return res;
        }

        public static double amix(double[] Tci, double[] Pci, double[] wi, double[] xyi, double[,] kijm, double TinK)
        {
            //
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa
            //
            int n = Tci.Length;
            double[,] a = new double[n, n];
            double R = 8.314;
            double amix = 0D;

            for (int i = 0; i < n; i++)
            {
                a[i, i] = 0.45724 * Math.Pow(R, 2) * Math.Pow(Tci[i], 2) / Pci[i] * alpha(wi[i], TinK, Tci[i]);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (i != j)
                    {
                        a[i, j] = (1D - kijm[i, j]) * Math.Sqrt(a[i, i] * a[j, j]);
                        a[j, i] = a[i, j];
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    amix = amix + xyi[i] * xyi[j] * a[i, j];
                }
            }

            //amix_Tci_Pci_wi_xyi_kijm_T = amix;
            return amix;
        }

        public static double bmix(double[] Tci, double[] Pci, double[] xyi, double[,] lijm)
        {
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa

            int n = Tci.Length;
            double[,] b = new double[n, n];
            double R = 8.314;

            for (int i = 0; i < n; i++)
            {
                b[i, i] = 0.0778 * R * Tci[i] / Pci[i];
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (i != j)
                    {
                        b[i, j] = (1D - lijm[i, j]) * (b[i, i] + b[j, j]) * 0.5;
                        b[j, i] = b[i, j];
                    }
                }
            }

            double bmix = 0D;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    bmix = bmix + xyi[i] * xyi[j] * b[i, j];
                }
            }

            return bmix;
        }

        public static double alpha(double w, double TinK, double Tc)
        {
            double res = Math.Pow(1D + (0.37464 + 1.54226 * w - 0.26992 * w * w) * (1D - Math.Sqrt(TinK / Tc)), 2);
            return res;
        }

        public static double dbmixdni(int i_index, double[] Tci, double[] Pci, double[] xyi, double[,] lijm)
        {
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa

            double bmix = 0D;
            int n = Tci.Length;
            double[,] b = new double[n, n];
            double R = 8.314;

            for (int i = 0; i < n; i++)
            {
                b[i, i] = 0.0778 * R * Tci[i] / Pci[i];
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (i != j)
                    {
                        b[i, j] = (1D - lijm[i, j]) * (b[i, i] + b[j, j]) * 0.5;
                        b[j, i] = b[i, j];
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    bmix = bmix + xyi[i] * xyi[j] * b[i, j];
                }
            }

            double sumybij = 0;

            for (int j = 0; j < n; j++)
            {
                sumybij = sumybij + 2D * xyi[j] * b[i_index, j];
            }

            double res = -bmix + sumybij;
            return res;
        }

        public static double damixdni_i(int i_index, double[] Tci, double[] Pci, double[] wi, double[] xyi, double[,] kijm, double TinK)
        {
            double sumyaij = 0D;
            int n = Tci.Length;
            double[,] a = new double[n, n];
            double R = 8.314;

            for (int i = 0; i < n; i++)
            {
                a[i, i] = 0.45724 * Math.Pow(R, 2) * Math.Pow(Tci[i], 2) / Pci[i] * alpha(wi[i], TinK, Tci[i]);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (i != j)
                    {
                        a[i, j] = (1D - kijm[i, j]) * Math.Sqrt(a[i, i] * a[j, j]);
                        a[j, i] = a[i, j];
                    }
                }
            }

            for (int j = 0; j < n; j++)
            {
                sumyaij = sumyaij + 2D * xyi[j] * a[i_index, j];
            }

            return sumyaij;
        }

        public static double VGPR(double TinK, double PinPa, double aalpha, double b)
        {
            // T in C
            // P in Pa
            // a*alpha in Pa*(m3/mol)^2
            // b in m3/mol
            //            volume in units of m3/mol return  ed as "VGPRtp"
            //***********************************************************************************************************************************
            //          Cubic solver routine version 1.0,
            //          Tohoku University, coded by Richard L. Smith, Jr., inspired by Wataru Endo
            // Based on the algoritm of U. K. Dieters, "Calculation of Densities from Cubic Equations of State," AIChE Journal 48 (2002) 882-886.
            //
            //***********************************************************************************************************************************

            int iloop;
            double Rgas = 8.314;
            double X1 = 0, X2 = 0, X3 = 0;
            double CC1 = 0, CC2 = 0, CC0 = 0;

            // Error in the change in volume (dx) set to 1 x 10^(-6)

            double xerror = 0.000001;
            double P = PinPa;

            // Peng-Robinson (PR) equation of state

            double a = aalpha;
            double c1 = 2D * b;
            double c0 = -b * b;

            double RT = Rgas * TinK;

            double AA3 = P;
            double AA2 = c1 * P - b * P - RT;
            double aa1 = c0 * P - c1 * b * P - RT * c1 + a;
            double AA0 = -c0 * b * P - RT * c0 - a * b;
            double BB2 = AA2 / AA3;
            double bb1 = aa1 / AA3;
            double BB0 = AA0 / AA3;

            double BBmax = maxval(Math.Abs(BB0), Math.Abs(bb1), Math.Abs(BB2));
            double rr = 1 + BBmax;
            double xinfl = -1D / 3D * BB2;

            double xold = 0;

            if (gx(xinfl, BB0, bb1, BB2) > 0)
            {
                xold = -rr;
            }
            else if (gx(xinfl, BB0, bb1, BB2) <= 0)
            {
                xold = rr;
            }

            // Iterate by Kepler's method
            double VGPRtp = 9.9999999E+99;

            for (iloop = 0; iloop < 1000; iloop++)
            {
                double gx0 = gx(xold, BB0, bb1, BB2);
                double gx1 = dgx(xold, BB0, bb1, BB2);
                double gx2 = d2gx(xold, BB0, bb1, BB2);
                double dx = gx0 * gx1 / (gx1 * gx1 - 0.5 * gx0 * gx2);
                double xnew = xold - dx;
                if (Math.Abs(dx) < xerror || gx0 == 0)
                {
                    X1 = xnew;
                    CC2 = 1D;
                    CC1 = CC2 * X1 + BB2;
                    CC0 = CC1 * X1 + bb1;
                    VGPRtp = X1;
                    QuadEq(CC2, CC1, CC0, ref X1, ref X2, ref X3);
                    VGPRtp = maxval(X1, X2, X3);
                    break;
                }
                xold = xnew;
            }

            return VGPRtp;
        }

        public static double VLPR(double TinK, double PinPa, double aalpha, double b)
        {
            // T in C
            // P in Pa
            // a*alpha in Pa*(m3/mol)^2
            // b in m3/mol
            //           volume in units of m3/mol return  ed as "VLPRtp"
            //***********************************************************************************************************************************
            //          Cubic solver routine version 1.0,
            //          Tohoku University, coded by Richard L. Smith, Jr., inspired by Wataru Endo
            //  Based on the algoritm of U. K. Dieters, "Calculation of Densities from Cubic Equations of State," AIChE Journal 48 (2002) 882-886.
            //
            //***********************************************************************************************************************************

            int iloop;
            double Rgas = 8.314;

            // Error in the change in volume (dx) set to 1 x 10^(-6)

            double xerror = 0.000001;
            double P = PinPa;

            // Peng-Robinson (PR) equation of state

            double a = aalpha;
            double c1 = 2D * b;
            double c0 = -b * b;

            double RT = Rgas * TinK;

            double AA3 = P;
            double AA2 = c1 * P - b * P - RT;
            double aa1 = c0 * P - c1 * b * P - RT * c1 + a;
            double AA0 = -c0 * b * P - RT * c0 - a * b;
            double BB2 = AA2 / AA3;
            double bb1 = aa1 / AA3;
            double BB0 = AA0 / AA3;

            double BBmax = maxval(Math.Abs(BB0), Math.Abs(bb1), Math.Abs(BB2));
            double rr = 1 + BBmax;
            double xinfl = -1D / 3D * BB2;
            double xold = 0;

            if (gx(xinfl, BB0, bb1, BB2) > 0)
                xold = -rr;
            else if (gx(xinfl, BB0, bb1, BB2) <= 0)
                xold = rr;

            double gx0, gx1, gx2, dx, xnew = 0, CC2, CC1, CC0;
            // Iterate by Kepler's method
            double VLPRtp = 9.9999999E+99;
            double X1 = 0, X2 = 0, X3 = 0;

            for (iloop = 0; iloop < 1000; iloop++)
            {
                gx0 = gx(xold, BB0, bb1, BB2);
                gx1 = dgx(xold, BB0, bb1, BB2);
                gx2 = d2gx(xold, BB0, bb1, BB2);
                dx = gx0 * gx1 / (Math.Pow(gx1, 2) - 0.5 * gx0 * gx2);
                xnew = xold - dx;
                if (Math.Abs(dx) < xerror || gx0 == 0)
                {
                    X1 = xnew;
                    CC2 = 1D;
                    CC1 = CC2 * X1 + BB2;
                    CC0 = CC1 * X1 + bb1;
                    VLPRtp = X1;
                    QuadEq(CC2, CC1, CC0, ref X1, ref X2, ref X3);
                    VLPRtp = minval(X1, X2, X3, b);
                    break;
                }
                xold = xnew;
            }

            return VLPRtp;
        }

        public static double gx(double X, double BB0, double bb1, double BB2)
        {
            double gx = Math.Pow(X, 3) + BB2 * X * X + bb1 * X + BB0;
            return gx;
        }

        public static double dgx(double X, double BB0, double bb1, double BB2)
        {
            double dgx = 3D * X * X + 2D * BB2 * X + bb1;
            return dgx;
        }

        public static double d2gx(double X, double BB0, double bb1, double BB2)
        {
            double d2gx = 6D * X + 2D * BB2;
            return d2gx;
        }

        public static double maxval(double a, double b, double c)
        {
            double max0 = a;
            if (b > max0)
                max0 = b;
            if (c > max0)
                max0 = c;

            return max0;
        }

        public static double minval(double a, double b, double c, double Eos_b)
        {
            // Eliminate negative and zero values

            if (a <= 0 || a < Eos_b)
                a = 9.9999E+99;

            if (b <= 0 || b < Eos_b)
                b = 9.9999E+99;
            if (c <= 0 || c < Eos_b)
                c = 9.9999E+99;

            // Select the smallest positive value

            double min0 = a;
            if (b < min0)
                min0 = b;
            if (c < min0)
                min0 = c;
            return min0;
        }

        public static void QuadEq(double a, double b, double c, ref double X1, ref double X2, ref double X3)
        {
            double discrim = Math.Pow(b, 2) - 4D * a * c;
            if (discrim < 0)
            {
                X2 = X1;
                X3 = X1;
            }

            X2 = (-b + Math.Sqrt(discrim)) / (2D * a);
            X3 = (-b - Math.Sqrt(discrim)) / (2D * a);
        }

        // Pure component public  double s and calculations

        public static double PSatPREOS(double TcinK, double PcinPa, double w, double TinC)
        {
            // P in Pa,  V in m3/mol,  T in K

            double R = 8.314;
            double T = 273.15 + TinC;
            double RT = R * T;
            double Zc = 0.3074;
            double Tc = TcinK;
            double Pc = PcinPa;
            double res = 0;
            double VL = 0, Vv = 0;

            if (T > Tc)
            {
                return -999;
            }

            // Estimate Pressure   where 3 roots exist from the critical volume

            double Vc = Zc * R * Tc / Pc;
            double a = 0.45724 * R * R * Tc * Tc / Pc;
            double b = 0.0778 * R * Tc / Pc;
            double alpha = Math.Pow((1 + (0.37464 + 1.54226 * w - 0.26992 * w * w) * (1 - Math.Pow((T / Tc), 0.5))), 2);
            double aalpha = a * alpha;
            double P0 = PPR_aalpha(aalpha, b, TinC, Vc);

            if (P0 < 0)
            {
                P0 = 0.5;
            }

            for (int i = 1; i <= 50; i++)
            {
                VlVvPREOS(TinC, P0, aalpha, b, ref VL, ref Vv);

                if (VL == Vv)
                {
                    res = Pc;
                    break;
                }

                double Zl = P0 * VL / RT;
                double Zv = P0 * Vv / RT;
                double fugl = fPR_aalpha(aalpha, b, TinC, VL, P0);
                double fugV = fPR_aalpha(aalpha, b, TinC, Vv, P0);
                double Pnew = P0 * ((Zv - Zl) - Math.Log(fugV / fugl)) / (Zv - Zl);

                if (Math.Abs((Pnew - P0) / P0) < 0.00001)
                {
                    res = Pnew;
                    break;
                }
                else
                {
                    P0 = Pnew;
                }
            }
            return res;
        }

        public static void VlVvPREOS(double T, double P, double aalpha, double b, ref double VL, ref double Vv)
        {
            // T in C
            // P in Pa
            // a*alpha in Pa/(m3/mol)^2
            // b in m3/mol
            //            volume in units of m3/mol return  ed as "VPRtp"
            //***********************************************************************************************************************************
            //          Cubic solver routine version 1.0,
            //          Tohoku University, coded by Richard L. Smith, Jr., inspired by Wataru Endo
            //  Based on the algoritm of U. K. Dieters, "Calculation of Densities from Cubic Equations of State," AIChE Journal 48 (2002) 882-886.
            //
            //***********************************************************************************************************************************

            double Rgas = 8.314;

            // Error in the change in volume (dx) set to 1 x 10^(-6)

            double xerror = 0.000001;
            double TK = T + 273.15;

            // Peng-Robinson (PR) equation of state

            double a = aalpha;
            double c1 = 2D * b;
            double c0 = -b * b;
            double Eos_b = b;

            double RT = Rgas * TK;

            double AA3 = P;
            double AA2 = c1 * P - b * P - RT;
            double aa1 = c0 * P - c1 * b * P - RT * c1 + a;
            double AA0 = -c0 * b * P - RT * c0 - a * b;
            double BB2 = AA2 / AA3;
            double bb1 = aa1 / AA3;
            double BB0 = AA0 / AA3;

            double BBmax = maxval(Math.Abs(BB0), Math.Abs(bb1), Math.Abs(BB2));
            double rr = 1 + BBmax;
            double xinfl = -1D / 3D * BB2;

            double xold = 0;

            if (gx(xinfl, BB0, bb1, BB2) > 0)
            {
                xold = -rr;
            }
            else if (gx(xinfl, BB0, bb1, BB2) <= 0)
            {
                xold = rr;
            }

            // Iterate by Kepler's method
            double gx0, gx1, gx2, dx, xnew, X1, CC2, CC1, CC0, X2 = 0, X3 = 0;

            for (int iloop = 1; iloop < 200; iloop++)
            {
                gx0 = gx(xold, BB0, bb1, BB2);
                gx1 = dgx(xold, BB0, bb1, BB2);
                gx2 = d2gx(xold, BB0, bb1, BB2);
                dx = gx0 * gx1 / (Math.Pow(gx1, 2) - 0.5 * gx0 * gx2);
                xnew = xold - dx;
                if (Math.Abs(dx) < xerror || gx0 == 0)
                {
                    X1 = xnew;
                    CC2 = 1D;
                    CC1 = CC2 * X1 + BB2;
                    CC0 = CC1 * X1 + bb1;
                    QuadEq(CC2, CC1, CC0, ref X1, ref X2, ref X3);
                    Vv = maxval(X1, X2, X3);
                    VL = minval(X1, X2, X3, Eos_b);
                    break;
                }
                xold = xnew;
            }
            return;
        }

        public static double PPR_aalpha(double aalpha, double b, double TinC, double Vm3mol)
        {
            // P in Pa,  V in m3/mol,  T in K

            // a and b must be calculated in units of K and Pa

            double R = 8.314;
            double T = 273.15 + TinC;
            double V = Vm3mol;
            double res = R * T / (V - b) - aalpha / (V * (V + b) + b * (V - b));
            return res;
        }

        public static double fPR_aalpha(double aalpha, double b, double TinC, double Vm3mol, double PinPa)
        {
            // P in Pa,  V in m3/mol,  T in K
            // a and b must be calculated in units of K and Pa

            double R = 8.314;
            double T = 273.15 + TinC;
            double V = Vm3mol;
            double P = PinPa;
            double RT = R * T;
            double Z = P * V / RT;
            double BB = b * P / RT;
            double AA = aalpha * P / Math.Pow(RT, 2);
            double S2 = Math.Pow(2, 0.5);
            double lnPhi = (Z - 1) - Math.Log(Z - BB) - AA / (2 * S2 * BB) * Math.Log((Z + (1 + S2) * BB) / (Z + (1 - S2) * BB));
            double res = P * Math.Exp(lnPhi);
            return res;
        }

        // Rewrite to use ComponentCollections
    }
}