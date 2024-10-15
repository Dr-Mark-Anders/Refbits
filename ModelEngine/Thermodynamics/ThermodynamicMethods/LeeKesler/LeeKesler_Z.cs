using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelEngine
{
    public static partial class LeeKesler
    {
        public static double Z0_FromTable(double Tr, double Pr, enumFluidRegion ss, out List<double> Ts, out List<double> Ps, out List<double> Zs)
        {
            double res;
            if (ss == enumFluidRegion.Liquid)
                res = Bilinearinterpolation(Z0Liquid, Tr, Pr, out Ts, out Ps, out Zs);
            else // all other states, not solid
                res = Bilinearinterpolation(Z0Vapour, Tr, Pr, out Ts, out Ps, out Zs);

            return res;
        }

        public static double Z1_FromTable(double Tr, double Pr, enumFluidRegion ss, out List<double> Ts, out List<double> Ps, out List<double> Zs)
        {
            double res;
            if (ss == enumFluidRegion.Liquid)
                res = Bilinearinterpolation(Z1Liquid, Tr, Pr, out Ts, out Ps, out Zs);
            else // all other states, not solid
                res = Bilinearinterpolation(Z1Vapour, Tr, Pr, out Ts, out Ps, out Zs);

            return res;
        }

        public static double Z0_Rig(double Tr, double Pr, enumFluidRegion state)
        {
            double B, C, D, LeftSide, Vr;
            double DVr = 0.001, ErrOld, Err2;
            double count = 0;
            double VrMin, VrMax;
            bool solutioncrossed = false;

            B = Simple.b1 - Simple.b2 / Tr - Simple.b3 / Math.Pow(Tr, 2) - Simple.b4 / Math.Pow(Tr, 3);
            C = Simple.c1 - Simple.c2 / Tr + Simple.c3 / Math.Pow(Tr, 3);
            D = Simple.d1 + Simple.d2 / Tr;

            VrMin = 0.00001 * Tr / Pr;
            VrMax = Tr / Pr;

            if (Tr > 1)
                VrMax = 1.05 * Tr / Pr;

            if (Pr <= 0.2 && Tr < 1)
                VrMax = Tr / Pr;
            if (Pr > 0.2 && Pr <= 1)
                VrMax = 1.1 * Tr / Pr;
            else if (Pr > 1 && Pr <= 4)
                VrMax = 1.2 * Tr / Pr;
            else if (Pr > 4 && Pr <= 11)
                VrMax = 4 * Tr / Pr;
            else if (Pr > 11)
                VrMax = 5 * Tr / Pr;

            if (state == enumFluidRegion.Liquid)
            {
                Vr = VrMin;
            }
            else
            {
                Vr = VrMax;
                DVr = -DVr;
            }

            double Err1 = ErrorSimple(Vr, Tr, Pr, B, C, D, out _);

            do
            {
                count++;
                ErrOld = Err1;
                Err1 = ErrorSimple(Vr, Tr, Pr, B, C, D, out double Z);
                Err2 = ErrorSimple(Vr + DVr, Tr, Pr, B, C, D, out double Z1);
                if ((Err1 < 0 && ErrOld > 0) || (Err1 > 0 && ErrOld < 0)) // Then  ' solution crossed
                {
                    solutioncrossed = true;
                    DVr = -DVr / 2;
                }
                else if (solutioncrossed && Math.Abs(Err2) > Math.Abs(Err1)) //  moving away from solution
                {
                    DVr = -DVr / 2;
                    // solutioncrossed = false;
                }

                Vr = Vr + DVr;
            } while (Math.Abs(Err1) > 0.000000001 && count < 1000000);

            LeftSide = Pr * Vr / Tr;

            return Pr * Vr / Tr;
        }

        public static double Z1_Rig(double Tr, double Pr, enumFluidRegion state)
        {
            double B, C, D, Vr, LeftSide;
            double DVr = 0.001, Err1, Err2, VrMin, VrMax, ErrOld;
            double count = 0;
            bool solutioncrossed = false;

            B = Corr.b1 - Corr.b2 / Tr - Corr.b3 / Math.Pow(Tr, 2) - Corr.b4 / Math.Pow(Tr, 3);
            C = Corr.c1 - Corr.c2 / Tr + Corr.c3 / Math.Pow(Tr, 3);
            D = Corr.d1 + Corr.d2 / Tr;

            VrMin = 0.00001 * Tr / Pr;
            VrMax = Tr / Pr;

            if (Tr > 1)
                VrMax = 1.05 * Tr / Pr;

            if (Pr <= 0.2 && Tr < 1)
                VrMax = Tr / Pr;
            if (Pr > 0.2 && Pr <= 1)
                VrMax = 1.1 * Tr / Pr;
            else if (Pr > 1 && Pr <= 4)
                VrMax = 1.2 * Tr / Pr;
            else if (Pr > 4 && Pr <= 11)
                VrMax = 4 * Tr / Pr;
            else if (Pr > 11)
                VrMax = 5 * Tr / Pr;

            if (state == enumFluidRegion.Liquid)
            {
                Vr = VrMin;
            }
            else
            {
                Vr = VrMax;
                DVr = -DVr;
            }

            Err1 = ErrorSimple(Vr, Tr, Pr, B, C, D, out _);

            do
            {
                count++;
                ErrOld = Err1;
                Err1 = ErrorCorr(Vr, Tr, Pr, B, C, D, out double Z);
                Err2 = ErrorCorr(Vr + DVr, Tr, Pr, B, C, D, out double Z1);
                if ((Err1 < 0 && ErrOld > 0) || (Err1 > 0 && ErrOld < 0)) // Then  ' solution crossed
                {
                    DVr = -DVr / 2;
                    solutioncrossed = true;
                }
                else if (solutioncrossed && Math.Abs(Err2) > Math.Abs(Err1))
                {
                    DVr = -DVr / 2;
                    //solutioncrossed = false;
                }

                Vr = Vr + DVr;
            } while (Math.Abs(Err1) > 0.000000001 && count < 1000000);

            LeftSide = Pr * Vr / Tr;

            return Pr * Vr / Tr;
        }

        public static double Z0_ConstrainedRigOld(double Tr, double Pr, enumFluidRegion state)
        {
            double B, C, D, Vr;
            double DVr = 0.01, ErrOld, Err2;
            double count = 0;
            double VrMin, VrMax;
            bool solutioncrossed = false;
            double ZHigh, ZLow;

            B = Simple.b1 - Simple.b2 / Tr - Simple.b3 / Math.Pow(Tr, 2) - Simple.b4 / Math.Pow(Tr, 3);
            C = Simple.c1 - Simple.c2 / Tr + Simple.c3 / Math.Pow(Tr, 3);
            D = Simple.d1 + Simple.d2 / Tr;

            var Zestimate = Z0_FromTable(Tr, Pr, state, out List<double> Ts, out List<double> Ps, out List<double> Zs);

            ZLow = Zs.Min();
            ZHigh = Zs.Max();

            if (state == enumFluidRegion.Liquid)
            {
                VrMin = ZLow * Tr / Pr;
                VrMax = ZHigh * Tr / Pr;
            }
            else
            {
                VrMin = ZLow * Tr / Pr;
                VrMax = ZHigh * Tr / Pr;
            }

            if (state == enumFluidRegion.Liquid)
            {
                Vr = VrMin;
            }
            else
            {
                Vr = VrMax;
                DVr = -DVr;
            }

            double Err1 = ErrorSimple(Vr, Tr, Pr, B, C, D, out _);

            do
            {
                count++;
                ErrOld = Err1;
                Err1 = ErrorSimple(Vr, Tr, Pr, B, C, D, out double Z);
                Err2 = ErrorSimple(Vr + DVr, Tr, Pr, B, C, D, out double _);
                if ((Err1 < 0 && ErrOld > 0) || (Err1 > 0 && ErrOld < 0)) // Then  ' solution crossed
                {
                    solutioncrossed = true;
                    DVr = -DVr / 2;
                }
                else if (solutioncrossed && Math.Abs(Err2) > Math.Abs(Err1)) //  moving away from solution
                {
                    DVr = -DVr / 2;
                    // solutioncrossed = false;
                }

                Vr = Vr + DVr;
            } while (Math.Abs(Err1) > 0.00001 && count < 10000);
            Vr = Vr - DVr;

            if (Math.Abs(Err1) > 0.00001)
                return double.NaN;

            return Pr * Vr / Tr;
        }

        public static double Z1_ConstrainedRigOld(double Tr, double Pr, enumFluidRegion state)
        {
            double B, C, D, Vr;
            double DVr = 0.01, Err1, Err2, VrMin, VrMax, ErrOld;
            double count = 0;
            bool solutioncrossed = false;
            double ZLow, ZHigh;

            B = Corr.b1 - Corr.b2 / Tr - Corr.b3 / Math.Pow(Tr, 2) - Corr.b4 / Math.Pow(Tr, 3);
            C = Corr.c1 - Corr.c2 / Tr + Corr.c3 / Math.Pow(Tr, 3);
            D = Corr.d1 + Corr.d2 / Tr;

            var Zestimate = Z1_FromTable(Tr, Pr, state, out List<double> Ts, out List<double> Ps, out List<double> Zs);

            ZLow = Zs.Min();
            ZHigh = Zs.Max();

            if (state == enumFluidRegion.Liquid)
            {
                VrMin = ZLow * Tr / Pr;
                VrMax = ZHigh * Tr / Pr;
            }
            else
            {
                VrMin = ZLow * Tr / Pr;
                VrMax = ZHigh * Tr / Pr;
            }

            if (state == enumFluidRegion.Liquid)
            {
                Vr = VrMin;
            }
            else
            {
                Vr = VrMax;
                DVr = -DVr;
            }

            Err1 = ErrorSimple(Vr, Tr, Pr, B, C, D, out _);

            do
            {
                count++;
                ErrOld = Err1;
                Err1 = ErrorCorr(Vr, Tr, Pr, B, C, D, out double Z);
                Err2 = ErrorCorr(Vr + DVr, Tr, Pr, B, C, D, out double _);
                if ((Err1 < 0 && ErrOld > 0) || (Err1 > 0 && ErrOld < 0)) // Then  ' solution crossed
                {
                    DVr = -DVr / 2;
                    solutioncrossed = true;
                }
                else if (solutioncrossed && Math.Abs(Err2) > Math.Abs(Err1))
                {
                    DVr = -DVr / 2;
                    //solutioncrossed = false;
                }

                Vr = Vr + DVr;
            } while (Math.Abs(Err1) > 0.00001 && count < 10000);

            Vr = Vr - DVr;

            if (Math.Abs(Err1) > 0.00001)
                return double.NaN;

            return Pr * Vr / Tr;
        }

        public static double Z0_BisectTable(double Tr, double Pr, enumFluidRegion state)
        {
            if (double.IsNaN(Tr) || double.IsNaN(Pr))
                return double.NaN;

            double B, C, D;
            double Err2, Err1, V1, V2;
            double count = 0;
            double VrMin, VrMax;
            double ZHigh, ZLow;
            int bisectsteps = 100;
            double stepsize;

            B = Simple.b1 - Simple.b2 / Tr - Simple.b3 / Math.Pow(Tr, 2) - Simple.b4 / Math.Pow(Tr, 3);
            C = Simple.c1 - Simple.c2 / Tr + Simple.c3 / Math.Pow(Tr, 3);
            D = Simple.d1 + Simple.d2 / Tr;

            var Zestimate = Z0_FromTable(Tr, Pr, state, out _, out _, out List<double> Zs);

            if (double.IsNaN(Zestimate))
                return double.NaN;

            ZLow = Zs.Min();
            ZHigh = Zs.Max();

            VrMin = (ZLow - ZLow * 0.1) * Tr / Pr;
            VrMax = (ZHigh + ZHigh * 0.1) * Tr / Pr;

            do
            {
                stepsize = Math.Abs(VrMax - VrMin) / bisectsteps;

                Err1 = ErrorSimple(VrMin, Tr, Pr, B, C, D, out _);
                for (int i = 0; i <= bisectsteps; i++)
                {
                    if (state == enumFluidRegion.Liquid)
                    {
                        V1 = stepsize * i + VrMin;
                        V2 = stepsize * (i + 1) + VrMin;
                    }
                    else
                    {
                        V1 = VrMax - stepsize * i;
                        V2 = VrMax - stepsize * (i + 1);
                    }
                    Err1 = ErrorSimple(V1, Tr, Pr, B, C, D, out double Z);
                    Err2 = ErrorSimple(V2, Tr, Pr, B, C, D, out _);

                    if ((Err1 < 0 && Err2 > 0) || (Err1 > 0 && Err2 < 0)) // Then  ' solution crossed
                    {
                        if (state == enumFluidRegion.Liquid)
                        {
                            VrMin = V1;
                            VrMax = V2;
                        }
                        else
                        {
                            VrMin = V2;
                            VrMax = V1;
                        }
                        break;
                    }
                }
                count++;
            } while (Math.Abs(Err1) > 0.00001 && count < 10000 && Math.Abs(VrMin - VrMax) > 1e-10);

            if (Math.Abs(Err1) > 0.00001)
                return double.NaN;

            return Pr * VrMin / Tr;
        }

        public static double Z1_BisectTable(double Tr, double Pr, enumFluidRegion state)
        {
            if (double.IsNaN(Tr) || double.IsNaN(Pr))
                return double.NaN;

            double B, C, D;
            double Err1, Err2, VrMin, VrMax, V1, V2;
            double count = 0;
            double ZLow, ZHigh;
            double stepsize;

            B = Corr.b1 - Corr.b2 / Tr - Corr.b3 / Math.Pow(Tr, 2) - Corr.b4 / Math.Pow(Tr, 3);
            C = Corr.c1 - Corr.c2 / Tr + Corr.c3 / Math.Pow(Tr, 3);
            D = Corr.d1 + Corr.d2 / Tr;

            var Zestimate = Z1_FromTable(Tr, Pr, state, out _, out _, out List<double> Zs);

            if (double.IsNaN(Zestimate))
                return double.NaN;

            ZLow = Zs.Min();
            ZHigh = Zs.Max();

            VrMin = (ZLow - ZLow * 0.1) * Tr / Pr;
            VrMax = (ZHigh + ZHigh * 0.1) * Tr / Pr;

            do
            {
                stepsize = (VrMax - VrMin) / 10;
                Err1 = ErrorCorr(VrMin, Tr, Pr, B, C, D, out _);
                for (int i = 0; i <= 10; i++)
                {
                    if (state == enumFluidRegion.Liquid)
                    {
                        V1 = stepsize * i + VrMin;
                        V2 = stepsize * (i + 1) + VrMin;
                    }
                    else
                    {
                        V1 = VrMax - stepsize * i;
                        V2 = VrMax - stepsize * (i + 1);
                    }

                    Err1 = ErrorCorr(V1, Tr, Pr, B, C, D, out double Z);
                    Err2 = ErrorCorr(V2, Tr, Pr, B, C, D, out _);

                    if ((Err1 < 0 && Err2 > 0) || (Err1 > 0 && Err2 < 0)) // Then  ' solution crossed
                    {
                        if (state == enumFluidRegion.Liquid)
                        {
                            VrMin = V1;
                            VrMax = V2;
                        }
                        else
                        {
                            VrMin = V2;
                            VrMax = V1;
                        }
                        break;
                    }
                }
                count++;
            } while (Math.Abs(Err1) > 0.00001 && count < 10000 && Math.Abs(VrMin - VrMax) > 1e-10);

            if (Math.Abs(Err1) > 0.00001)
                return double.NaN;

            return Pr * VrMin / Tr;
        }
    }
}