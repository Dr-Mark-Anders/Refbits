using Extensions;
using Math2;
using System;
using System.Collections.Generic;
using System.Drawing;
using Units.UOM;

namespace ModelEngine // not finished xxx
{
    public static class DistillationConversions
    {
        public static DistPoints Convert(enumDistType from, enumDistType to, DistPoints data)
        {
            DistPoints res = null;
            DistPoints filleddata;


            if (data.Count != 11)
                filleddata = FillMissingDataByProbability(data); // generate 11 points
            else
                filleddata = data;

            switch (from)
            {
                case enumDistType.D86:
                    switch (to)
                    {
                        case enumDistType.D86: // do nothing
                            res = filleddata;
                            break;

                        case enumDistType.D1160:
                            res = D86ToTBP(filleddata);
                            res = TBPToD1160(res);
                            break;

                        case enumDistType.D2887:
                            res = D86ToTBP(filleddata);
                            res = TBPToD2887(res);
                            break;

                        case enumDistType.TBP_VOL:
                            res = D86ToTBP(filleddata);
                            break;

                        case enumDistType.TBP_WT:
                            res = D86ToTBP(filleddata);
                            res = TBPToD2887(res);
                            break;
                    }
                    break;

                case enumDistType.D1160:
                    switch (to)
                    {
                        case enumDistType.D86:
                            res = D1160ToTBP(filleddata);
                            res = TBPToD86(res);
                            break;

                        case enumDistType.D1160:// do nothing
                            res = filleddata;
                            break;

                        case enumDistType.D2887:
                            res = D1160ToTBP(filleddata);
                            res = TBPToD1160(res);
                            break;

                        case enumDistType.TBP_VOL:
                            res = D1160ToTBP(filleddata);
                            break;

                        case enumDistType.TBP_WT:
                            res = D1160ToTBP(filleddata);
                            res = TBPToD2887(res);
                            break;
                    }
                    break;

                case enumDistType.TBP_VOL:
                    switch (to)
                    {
                        case enumDistType.D86:
                            res = TBPToD86(filleddata); ;
                            break;

                        case enumDistType.D1160:
                            res = TBPToD1160(filleddata);
                            break;

                        case enumDistType.D2887:
                            res = TBPToD2887(filleddata);
                            break;

                        case enumDistType.TBP_VOL:// do nothing
                            res = filleddata;
                            break;

                        case enumDistType.TBP_WT:
                            res = filleddata;
                            res = TBPToD2887(res);
                            break;
                    }
                    break;

                case enumDistType.TBP_WT:
                    switch (to)
                    {
                        case enumDistType.D86:
                            res = TBPToD86(filleddata); ;
                            break;

                        case enumDistType.D1160:
                            res = TBPToD1160(filleddata);
                            break;

                        case enumDistType.D2887:
                            res = TBPToD2887(filleddata);
                            break;

                        case enumDistType.TBP_VOL:// do nothing
                            res = filleddata;
                            break;

                        case enumDistType.TBP_WT:
                            res = filleddata;
                            //res = TBPToD2887(res);
                            break;
                    }
                    break;

                case enumDistType.D2887:
                    switch (to)
                    {
                        case enumDistType.D86:
                            res = D2887ToTBP(filleddata);
                            res = TBPToD86(res);
                            break;

                        case enumDistType.D1160:
                            res = D2887ToTBP(filleddata);
                            res = TBPToD1160(res);
                            break;

                        case enumDistType.D2887:// do nothing
                            res = filleddata;
                            break;

                        case enumDistType.TBP_VOL:
                            res = D2887ToTBP(filleddata);
                            break;

                        case enumDistType.TBP_WT:
                            res = D2887ToTBP(filleddata);
                            res = TBPToD2887(res);
                            break;
                    }
                    break;
            }

            res.DENSITY = data.DENSITY;

            return res;
        }

        public static DistPoints FillMissingDataByCubicConstrained(DistPoints data)
        {
            DistPoints res = new();
            DistPoint p;

            for (int n = 0; n < 11; n++)
            {
                if (data[n].BP_UOM.origin != SourceEnum.Input)
                {
                    p = new(Global.Lvpct_standard[n], data[n].BP, SourceEnum.Input);
                }
                else
                {
                    Temperature T = new Temperature(CubicSpline.CubSpline(eSplineMethod.Constrained, Global.Lv1[n], data.getPCTs(), data.GetKDoubles()));
                    p = new(Global.Lvpct_standard[n], T, SourceEnum.CalcEstimate);
                }
                res.Add(p);
            }

            return res;
        }

        public static DistPoints FillMissingDataByProbability(DistPoints data)
        {
            List<PointF> points = new();
            DistPoints newpoints = new();
            float a, bb;

            for (int n = 0; n < data.Count; n++)
            {
                a = (float)Probability.NormSInv((int)data[n].Pct / 100f);
                bb = (float)data[n].BP;

                if (!double.IsNaN(bb) && data[n].BP_UOM.origin == SourceEnum.Input)
                    points.Add(new PointF(a, bb));
            }

            Probability.FindLinearLeastSquaresFit(points, out double m, out double b);

            for (int n = 0; n < 11; n++)
            {
                double res = m * Probability.NormSInv(Global.Lvpct_standard[n] / 100d) + b;

                DistPoint p = new(Global.Lvpct_standard[n], new Temperature(res, TemperatureUnit.Kelvin), SourceEnum.CalcEstimate);
                newpoints.Add(p);
            }

            for (int i = 0; i < data.Count; i++)
            {
                DistPoint old = data[i];
                for (int n = 0; n < newpoints.Count; n++) // keep any allready existing points
                {
                    DistPoint newP = newpoints[n];

                    if (old.Stdpct == newP.Stdpct)
                    {
                        newpoints[n] = old.Clone();
                    }
                }
            }

            return newpoints;
        }

        private static DistPoints D86ToTBP(DistPoints DistA) // Daubert et al.
        {
            //   Y = AX^B
            //                   A       B       Y
            //1  0  10 - 0    5.8589  0.6204
            //5  1
            //10 2  30 - 10   4.1481  0.7164
            //20 3
            //30 4  50 - 30   2.6956  0.8008
            //50 5
            //70 6  70 - 50   2.2744  0.8200
            //80 7
            //90 8  90 - 70   2.6339  0.7550
            //95 9
            //99 10 100 - 90  0.1403  1.6606

            const double A_10_0 = 5.8589, B_10_0 = 0.6204;
            const double A_30_10 = 4.1481, B_30_10 = 0.7164;
            const double A_50_30 = 2.6956, B_50_30 = 0.8008;
            const double A_70_50 = 2.2744, B_70_50 = 0.8200;
            const double A_90_70 = 2.6339, B_90_70 = 0.7550;
            const double A_100_90 = 0.1403, B_100_90 = 1.6606;

            double Delta_10_0 = DistA[2].BP - DistA[0].BP;
            double Delta_30_10 = DistA[4].BP - DistA[2].BP;
            double Delta_50_30 = DistA[5].BP - DistA[4].BP;
            double Delta_70_50 = DistA[6].BP - DistA[5].BP;
            double Delta_90_70 = DistA[8].BP - DistA[6].BP;
            double Delta_100_90 = DistA[10].BP - DistA[8].BP;

            double Y_10_0 = A_10_0 * Math.Pow(Delta_10_0, B_10_0);
            double Y_30_10 = A_30_10 * Math.Pow(Delta_30_10, B_30_10);
            double Y_50_30 = A_50_30 * Math.Pow(Delta_50_30, B_50_30);
            double Y_70_50 = A_70_50 * Math.Pow(Delta_70_50, B_70_50);
            double Y_90_70 = A_90_70 * Math.Pow(Delta_90_70, B_90_70);
            double Y_100_90 = A_100_90 * Math.Pow(Delta_100_90, B_100_90);

            DistPoints DistB = new();

            Temperature DistB_50 = new(255.4 + 0.8851 * Math.Pow(DistA[5].BP.Kelvin - 255.4, 1.0258));

            Temperature DistB_30 = DistB_50 - Y_50_30;
            Temperature DistB_10 = DistB_30 - Y_30_10;
            Temperature DistB_0 = DistB_10 - Y_10_0;

            Temperature DistB_70 = DistB_50 + Y_70_50;
            Temperature DistB_90 = DistB_70 + Y_90_70;
            Temperature DistB_100 = DistB_90 + Y_100_90;

            DistB.Add(1, DistB_0);
            DistB.Add(10, DistB_10);
            DistB.Add(30, DistB_30);
            DistB.Add(50, DistB_50);
            DistB.Add(70, DistB_70);
            DistB.Add(90, DistB_90);
            DistB.Add(99, DistB_100);

            DistPoints res = FillMissingDataByCubicConstrained(DistB);

            return res;
        }

        private static DistPoints TBPToD86(DistPoints DistA) // Daubert et al.
        {
            //   Delta = (Y/A)^(1/B)
            //              A       B       Y
            //1  0  10 - 0    5.8589  0.6204  55.96
            //5  1
            //10 2  30 - 10   4.1481  0.7164  68.39
            //20 3
            //30 4  50 - 30   2.6956  0.8008  33.20
            //50 5
            //70 6  70 - 50   2.2744  0.8200  33.93
            //80 7
            //90 8  90 - 70   2.6339  0.7550  29.93
            //95 9
            //99 10 100 - 90  0.1403  1.6606  14.02

            const double A_10_0 = 5.8589, B_10_0 = 0.6204;
            const double A_30_10 = 4.1481, B_30_10 = 0.7164;
            const double A_50_30 = 2.6956, B_50_30 = 0.8008;
            const double A_70_50 = 2.2744, B_70_50 = 0.8200;
            const double A_90_70 = 2.6339, B_90_70 = 0.7550;
            const double A_100_90 = 0.1403, B_100_90 = 1.6606;

            double Y_10_0 = DistA[2].BP - DistA[0].BP;
            double Y_30_10 = DistA[4].BP - DistA[2].BP;
            double Y_50_30 = DistA[5].BP - DistA[4].BP;
            double Y_70_50 = DistA[6].BP - DistA[5].BP;
            double Y_90_70 = DistA[8].BP - DistA[6].BP;
            double Y_100_90 = DistA[10].BP - DistA[8].BP;

            double Delta_10_0 = Math.Pow((Y_10_0 / A_10_0), (1 / B_10_0));
            double Delta_30_10 = Math.Pow((Y_30_10 / A_30_10), (1 / B_30_10));
            double Delta_50_30 = Math.Pow((Y_50_30 / A_50_30), (1 / B_50_30));
            double Delta_70_50 = Math.Pow((Y_70_50 / A_70_50), (1 / B_70_50));
            double Delta_90_70 = Math.Pow((Y_90_70 / A_90_70), (1 / B_90_70));
            double Delta_100_90 = Math.Pow((Y_100_90 / A_100_90), (1 / B_100_90));

            DistPoints DistB = new();

            Temperature DistB50 = new(Math.Pow(((DistA[5].BP - 255.4) / 0.8851), (1 / 1.0258)) + 255.4);

            Temperature DistB30 = DistB50 - Delta_50_30;
            Temperature DistB10 = DistB30 - Delta_30_10;
            Temperature DistB0 = DistB10 - Delta_10_0;

            Temperature DistB70 = DistB50 + Delta_70_50;
            Temperature DistB90 = DistB70 + Delta_90_70;
            Temperature DistB100 = DistB90 + Delta_100_90;

            DistB.Add(1, DistB0);
            DistB.Add(10, DistB10);
            DistB.Add(30, DistB30);

            DistB.Add(50, DistB50);

            DistB.Add(70, DistB70);
            DistB.Add(90, DistB90);
            DistB.Add(99, DistB100);

            DistB = FillMissingDataByCubicConstrained(DistB);

            return DistB;
        }

        private static DistPoints D2887ToTBP(DistPoints DistA) // Daubert et al.
        {
            //   Y = AX^B
            //                   A       B       Y
            //1  0  10 - 0     0.20312 1.42960
            //5  1
            //10 2  30 - 10    0.02175 2.02530
            //20 3
            //30 4  50 - 30    0.08055 1.69880
            //50 5
            //70 6  70 - 50    0.25088 1.39750
            //80 7
            //90 8  90 - 70    0.37475 1.29380
            //95 9  95 - 90    0.90427 0.87230
            //99 10 100 - 95   0.20312 1.97330

            const double A_10_0 = 0.20312, B_10_0 = 1.42960;
            const double A_30_10 = 0.02175, B_30_10 = 2.02530;
            const double A_50_30 = 0.08055, B_50_30 = 1.69880;
            const double A_70_50 = 0.25088, B_70_50 = 1.39750;
            const double A_90_70 = 0.37475, B_90_70 = 1.29380;
            const double A_95_90 = 0.90427, B_95_90 = 0.87230;
            const double A_100_95 = 0.20312, B_100_95 = 1.97330;

            double Delta_10_0 = DistA[2].BP - DistA[0].BP;
            double Delta_30_10 = DistA[4].BP - DistA[2].BP;
            double Delta_50_30 = DistA[5].BP - DistA[4].BP;
            double Delta_70_50 = DistA[6].BP - DistA[5].BP;
            double Delta_90_70 = DistA[8].BP - DistA[6].BP;
            double Delta_95_90 = DistA[9].BP - DistA[8].BP;
            double Delta_100_95 = DistA[10].BP - DistA[9].BP;

            double Y_10_0 = A_10_0 * System.Math.Pow(Delta_10_0, B_10_0);
            double Y_30_10 = A_30_10 * System.Math.Pow(Delta_30_10, B_30_10);
            double Y_50_30 = A_50_30 * System.Math.Pow(Delta_50_30, B_50_30);
            double Y_70_50 = A_70_50 * System.Math.Pow(Delta_70_50, B_70_50);
            double Y_90_70 = A_90_70 * System.Math.Pow(Delta_90_70, B_90_70);
            double Y_95_90 = A_95_90 * System.Math.Pow(Delta_95_90, B_95_90);
            double Y_100_95 = A_100_95 * System.Math.Pow(Delta_100_95, B_100_95);

            DistPoints DistB = new();

            Temperature DistB50 = new(255.4 + 0.8851 * System.Math.Pow(DistA[5].BP - 255.4, 1.0258));

            Temperature DistB30 = DistB50 - Y_50_30;
            Temperature DistB10 = DistB30 - Y_30_10;
            Temperature DistB0 = DistB10 - Y_10_0;

            Temperature DistB70 = DistB50 + Y_70_50;
            Temperature DistB90 = DistB70 + Y_90_70;
            Temperature DistB95 = DistB90 + Y_95_90;
            Temperature DistB100 = DistB95 + Y_100_95;

            DistB.Add(1, DistB0);
            DistB.Add(10, DistB10);
            DistB.Add(30, DistB30);
            DistB.Add(50, DistB50);
            DistB.Add(70, DistB70);
            DistB.Add(90, DistB90);
            DistB.Add(95, DistB95);
            DistB.Add(99, DistB100);

            DistB = FillMissingDataByCubicConstrained(DistB);

            return DistB;
        }

        private static DistPoints TBPToD2887(DistPoints DistA) // Daubert et al.
        {
            //   Delta = (Y/A)^(1/B)
            //                   A       B       Y
            //1  0  10 - 0     0.20312 1.42960
            //5  1
            //10 2  30 - 10    0.02175 2.02530
            //20 3
            //30 4  50 - 30    0.08055 1.69880
            //50 5
            //70 6  70 - 50    0.25088 1.39750
            //80 7
            //90 8  90 - 70    0.37475 1.29380
            //95 9  95 - 90    0.90427 0.87230
            //99 10 100 - 95   0.20312 1.97330

            const double A_10_0 = 0.20312, B_10_0 = 1.42960;
            const double A_30_10 = 0.02175, B_30_10 = 2.02530;
            const double A_50_30 = 0.08055, B_50_30 = 1.69880;
            const double A_70_50 = 0.25088, B_70_50 = 1.39750;
            const double A_90_70 = 0.37475, B_90_70 = 1.29380;
            const double A_95_90 = 0.90427, B_95_90 = 0.87230;
            const double A_100_95 = 0.20312, B_100_95 = 1.97330;

            double Y_10_0 = DistA[2].BP - DistA[0].BP;
            double Y_30_10 = DistA[4].BP - DistA[2].BP;
            double Y_50_30 = DistA[5].BP - DistA[4].BP;
            double Y_70_50 = DistA[6].BP - DistA[5].BP;
            double Y_90_70 = DistA[8].BP - DistA[6].BP;
            double Y_95_90 = DistA[9].BP - DistA[8].BP;
            double Y_100_95 = DistA[10].BP - DistA[9].BP;

            double Delta_10_0 = System.Math.Pow((Y_10_0 / A_10_0), (1 / B_10_0));
            double Delta_30_10 = System.Math.Pow((Y_30_10 / A_30_10), (1 / B_30_10));
            double Delta_50_30 = System.Math.Pow((Y_50_30 / A_50_30), (1 / B_50_30));
            double Delta_70_50 = System.Math.Pow((Y_70_50 / A_70_50), (1 / B_70_50));
            double Delta_90_70 = System.Math.Pow((Y_90_70 / A_90_70), (1 / B_90_70));
            double Delta_95_90 = System.Math.Pow((Y_95_90 / A_95_90), (1 / B_95_90));
            double Delta_100_95 = System.Math.Pow((Y_100_95 / A_100_95), (1 / B_100_95));

            DistPoints DistB = new();

            Temperature DistB50 = new(Math.Pow(((DistA[5].BP + 273.15 - 255.4) / 0.8851), (1 / 1.0258)) + 255.4 - 273.15);

            Temperature DistB30 = DistB50 - Delta_50_30;
            Temperature DistB10 = DistB30 - Delta_30_10;
            Temperature DistB0 = DistB10 - Delta_10_0;

            Temperature DistB70 = DistB50 + Delta_70_50;
            Temperature DistB90 = DistB70 + Delta_90_70;
            Temperature DistB95 = DistB90 + Delta_95_90;
            Temperature DistB100 = DistB95 + Delta_100_95;

            DistB.Add(1, DistB0);
            DistB.Add(10, DistB10);
            DistB.Add(30, DistB30);
            DistB.Add(50, DistB50);
            DistB.Add(70, DistB70);
            DistB.Add(90, DistB90);
            DistB.Add(95, DistB95);
            DistB.Add(99, DistB100);

            DistB = FillMissingDataByCubicConstrained(DistB);

            return DistB;
        }

        private static DistPoints D1160ToTBP(DistPoints DistA) // ABC.
        {
            //   Y = AX^B
            //                   A       B
            /*
                    1	10-1	0.98313	8.83857
                    5	10-5	0.95910	4.90200
                    10	30-10	0.97430	16.47870
                    20	30-20	0.88110	8.50010
                    30	50-30	0.88330	13.00200
                    50
                    70	70-50	0.97000	10.00000
                    80	80-70	1.23130	1.30000
                    90	90-70	1.20180	3.30000
                    95	95-90	1.07190	1.50000
                    99	100-95	1.03130	5.00000
            */

            const double A_10_1 = 0.98313, B_10_1 = 8.83857;
            const double A_10_5 = 0.95910, B_10_5 = 4.90200;
            const double A_30_10 = 0.97430, B_30_10 = 16.47870;
            const double A_30_20 = 0.88110, B_30_20 = 8.50010;
            const double A_50_30 = 0.88330, B_50_30 = 13.00200;
            const double A_70_50 = 0.97000, B_70_50 = 10.00000;
            const double A_80_70 = 1.23130, B_80_70 = 1.30000;
            const double A_90_70 = 1.20180, B_90_70 = 3.30000;
            const double A_95_90 = 1.07190, B_95_90 = 1.50000;
            const double A_99_95 = 1.03130, B_99_95 = 5.00000;

            double Delta_10_1 = DistA[2].BP - DistA[0].BP;
            double Delta_10_5 = DistA[2].BP - DistA[1].BP;
            double Delta_30_10 = DistA[4].BP - DistA[2].BP;
            double Delta_30_20 = DistA[4].BP - DistA[3].BP;
            double Delta_50_30 = DistA[5].BP - DistA[4].BP;
            double Delta_70_50 = DistA[6].BP - DistA[5].BP;
            double Delta_80_70 = DistA[7].BP - DistA[6].BP;
            double Delta_90_70 = DistA[8].BP - DistA[6].BP;
            double Delta_95_90 = DistA[9].BP - DistA[8].BP;
            double Delta_99_95 = DistA[10].BP - DistA[9].BP;

            double Y_10_1 = A_10_1 * Delta_10_1 + B_10_1;
            double Y_10_5 = A_10_5 * Delta_10_5 + B_10_5;
            double Y_30_10 = A_30_10 * Delta_30_10 + B_30_10;
            double Y_30_20 = A_30_20 * Delta_30_20 + B_30_20;
            double Y_50_30 = A_50_30 * Delta_50_30 + B_50_30;
            double Y_70_50 = A_70_50 * Delta_70_50 + B_70_50;
            double Y_80_70 = A_80_70 * Delta_80_70 + B_80_70;
            double Y_90_70 = A_90_70 * Delta_90_70 + B_90_70;
            double Y_95_90 = A_95_90 * Delta_95_90 + B_95_90;
            double Y_99_95 = A_99_95 * Delta_99_95 + B_99_95;

            DistPoints DistB = new();

            double TBP90_10 = Y_90_70 + Y_70_50 + Y_50_30 + Y_30_10;
            double corr = -0.02857 * TBP90_10 + 2.857;

            Temperature DistB50 = DistA[5].BP + corr;

            Temperature DistB30 = DistB50 - Y_50_30;
            Temperature DistB20 = DistB30 - Y_30_20;
            Temperature DistB10 = DistB30 - Y_30_10;
            Temperature DistB5 = DistB10 - Y_10_5;
            Temperature DistB1 = DistB10 - Y_10_1;

            Temperature DistB70 = DistB50 + Y_70_50;
            Temperature DistB80 = DistB70 + Y_80_70;
            Temperature DistB90 = DistB70 + Y_90_70;
            Temperature DistB95 = DistB90 + Y_95_90;
            Temperature DistB99 = DistB95 + Y_99_95;

            DistB.Add(1, DistB1);
            DistB.Add(5, DistB5);
            DistB.Add(10, DistB10);
            DistB.Add(20, DistB20);
            DistB.Add(30, DistB30);
            DistB.Add(50, DistB50);
            DistB.Add(70, DistB70);
            DistB.Add(80, DistB80);
            DistB.Add(90, DistB90);
            DistB.Add(95, DistB95);
            DistB.Add(99, DistB99);

            //DistB = FillMissingDataByProbability(DistB);

            return DistB;
        }

        private static DistPoints TBPToD1160(DistPoints DistA) // ABC.
        {
            //   Y = AX^B
            //                   A       B
            /*
                    1	10-1	0.98313	8.83857
                    5	10-5	0.95910	4.90200
                    10	30-10	0.97430	16.47870
                    20	30-20	0.88110	8.50010
                    30	50-30	0.88330	13.00200
                    50
                    70	70-50	0.97000	10.00000
                    80	80-70	1.23130	1.30000
                    90	90-70	1.20180	3.30000
                    95	95-90	1.07190	1.50000
                    99	100-95	1.03130	5.00000
            */

            const double A_10_1 = 0.98313, B_10_1 = 8.83857;
            const double A_10_5 = 0.95910, B_10_5 = 4.90200;
            const double A_30_10 = 0.97430, B_30_10 = 16.47870;
            const double A_30_20 = 0.88110, B_30_20 = 8.50010;
            const double A_50_30 = 0.88330, B_50_30 = 13.00200;
            const double A_70_50 = 0.97000, B_70_50 = 10.00000;
            const double A_80_70 = 1.23130, B_80_70 = 1.30000;
            const double A_90_70 = 1.20180, B_90_70 = 3.30000;
            const double A_95_90 = 1.07190, B_95_90 = 1.50000;
            const double A_99_95 = 1.03130, B_99_95 = 5.00000;

            double Delta_10_1 = DistA[2].BP - DistA[0].BP;
            double Delta_10_5 = DistA[2].BP - DistA[1].BP;
            double Delta_30_10 = DistA[4].BP - DistA[2].BP;
            double Delta_30_20 = DistA[4].BP - DistA[3].BP;
            double Delta_50_30 = DistA[5].BP - DistA[4].BP;
            double Delta_70_50 = DistA[6].BP - DistA[5].BP;
            double Delta_80_70 = DistA[7].BP - DistA[6].BP;
            double Delta_90_70 = DistA[8].BP - DistA[6].BP;
            double Delta_95_90 = DistA[9].BP - DistA[8].BP;
            double Delta_99_95 = DistA[10].BP - DistA[9].BP;

            double Y_10_1 = (Delta_10_1 - B_10_1) / A_10_1;
            double Y_10_5 = (Delta_10_5 - B_10_5) / A_10_5;
            double Y_30_10 = (Delta_30_10 - B_30_10) / A_30_10;
            double Y_30_20 = (Delta_30_20 - B_30_20) / A_30_20;
            double Y_50_30 = (Delta_50_30 - B_50_30) / A_50_30;
            double Y_70_50 = (Delta_70_50 - B_70_50) / A_70_50;
            double Y_80_70 = (Delta_80_70 - B_80_70) / A_80_70;
            double Y_90_70 = (Delta_90_70 - B_90_70) / A_90_70;
            double Y_95_90 = (Delta_95_90 - B_95_90) / A_95_90;
            double Y_99_95 = (Delta_99_95 - B_99_95) / A_99_95;

            DistPoints DistB = new();

            double TBP90_10 = DistA[8].BP - DistA[2].BP;
            double corr = -0.02857 * TBP90_10 + 2.857;

            Temperature DistB50 = DistA[5].BP - corr;

            Temperature DistB30 = DistB50 - Y_50_30;
            Temperature DistB20 = DistB30 - Y_30_20;
            Temperature DistB10 = DistB30 - Y_30_10;
            Temperature DistB5 = DistB10 - Y_10_5;
            Temperature DistB1 = DistB10 - Y_10_1;

            Temperature DistB70 = DistB50 + Y_70_50;
            Temperature DistB80 = DistB70 + Y_80_70;
            Temperature DistB90 = DistB70 + Y_90_70;
            Temperature DistB95 = DistB90 + Y_95_90;
            Temperature DistB99 = DistB95 + Y_99_95;

            DistB.Add(1, DistB1);
            DistB.Add(5, DistB5);
            DistB.Add(10, DistB10);
            DistB.Add(20, DistB20);
            DistB.Add(30, DistB30);
            DistB.Add(50, DistB50);
            DistB.Add(70, DistB70);
            DistB.Add(80, DistB80);
            DistB.Add(90, DistB90);
            DistB.Add(95, DistB95);
            DistB.Add(99, DistB99);

            return DistB;
        }

        public static double[] GetPseudoCompLVfromDistPoints(Temperature[] StdBPs, DistPoints data, double PureCompaAdjustor, bool SetPureLVsToZero)
        {
            double[] LVPcts = new double[StdBPs.Length];
            double[] CumLVPcts = new double[StdBPs.Length];

            double res;

            for (int n = 0; n < StdBPs.Length; n++)
            {
                res = CubicSpline.CubSpline(eSplineMethod.Constrained, StdBPs[n], data.GetKDoubles(), data.getPCTs());
                if (res < 0)
                    CumLVPcts[n] = 0;
                else if (res > 100)
                    CumLVPcts[n] = 100;
                else
                    CumLVPcts[n] = res;

                if (n == 0)
                {
                    LVPcts[0] = CumLVPcts[0];
                }
                else if (SetPureLVsToZero && n < StaticCompDef.NoOfPureComps)
                {
                    LVPcts[n] = 0;
                }
                else
                {
                    LVPcts[n] = (CumLVPcts[n] - CumLVPcts[n - 1]) / PureCompaAdjustor;
                }
            }
            return LVPcts;
        }

 

        public static List<double> GetPseudoCompCumulativeLVfromDistPoints(Temperature[] StdBPs, DistPoints data)
        {
            List<double> CumLVPcts = new();

            double res;

            for (int n = 0; n < StdBPs.Length; n++)
            {
                res = CubicSpline.CubSpline(eSplineMethod.Constrained, StdBPs[n].Celsius, data.GetKDoubles(), data.getPCTs());

                if (res < 0)
                    CumLVPcts.Add(0);
                else if (res > 100)
                    CumLVPcts.Add(100);
                else
                    CumLVPcts.Add(res);
            }

            return CumLVPcts;
        }

        public static double[] GetCLVfromDistPoint(double[] BPs, DistPoints data)
        {
            double[] LVPcts = new double[BPs.Length];
            double[] CumLVPcts = new double[BPs.Length];

            double res;

            for (int n = 0; n < BPs.Length; n++)
            {
                res = CubicSpline.CubSpline(eSplineMethod.Constrained, BPs[n], data.GetKDoubles(), data.getPCTs());
                if (res < 0)
                    CumLVPcts[n] = 0;
                else if (res > 100)
                    CumLVPcts[n] = 100;
                else
                    CumLVPcts[n] = res;

                if (n == 0)
                {
                    LVPcts[0] = CumLVPcts[0];
                }
                else
                {
                    LVPcts[n] = CumLVPcts[n] - CumLVPcts[n - 1];
                }
            }
            return LVPcts;
        }
    }
}