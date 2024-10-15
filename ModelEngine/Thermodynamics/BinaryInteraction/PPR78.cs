using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    public class PPR78e
    {
        private static double[][] data = new double[27][];
        private static double[,] PPR78EArray = new double[2, 20];
        //private int fin;

        //CH3 (group 1)"
        //CH2 (group 2)"
        //CH   (group 3)
        //C    (group 4)
        //CH4 (group 5)"
        //C2H6(group 6)"
        //CHaro (group 7)"
        //Caro  (group 8)"
        //Cfused aromatic rings (group 9)"
        //CH2,cyclic (group 10)"
        //CHcyclic/Ccyclic (group 11)"
        //CO2 (group 12)"
        //N2  (group 13)"
        //H2S  (group 14)
        //SH   (group 15)
        //H2O  (group 16)
        //C2H4 (group 17)"
        //CH2,alkenic/CHalkenic (group 18)"
        //Calkenic (group 19)"
        //CHcycloalkeni /Ccycloalkenic (group 20)
        //H2   (group 21)"
        //CO   (group 22)
        //He  (group 23)"
        //Ar  (group 24)"
        //SO2 (group 25)"
        //O2  (group 26)"
        //NO   (group 27)

        public PPR78e()
        {    // A matrix, down & Bmatrix across
            data[0] = new double[27] { 0, 105.7, 294.9, 575.0, 20.25, 8.922, 136.2, 103.60, 774.10, 60.05, 170.9, 401.5, 88.19, 227.8, 1829, 11195, 35.00, 44.27, 260.1, 169.5, 239.5, 94.24, 513.4, 55.48, 201.4, 87.5, 0 };
            data[1] = new double[27] { 65.54, 0, 41.59, 183.9, 74.81, 65.88, 64.51, -7.549, -4.118, 27.79, -74.46, 237.1, 188.7, 124.6, 504.8, 12126, 82.35, 50.79, 51.82, 51.13, 240.9, 45.55, 673.22, 231.6, -28.5, 200.8, 0 };
            data[2] = new double[27] { 214.9, 39.05, 0, 85.10, 157.5, 96.77, 129.7, -89.22, 0, 71.37, 18.53, 380.9, 375.4, 562.8, 520.9, 567.6, -55.59, 193.2, 54.90, 0, 287.9, 0, 750.9, 634.2, 233.7, 0, 0 };
            data[3] = new double[27] { 431.6, 134.5, -86.13, 0, 35.69, -224.8, 284.1, 189.1, 0, 294.4, 81.33, 162.7, 635.2, -297.2, 1547, 0, -219.3, 419.0, 0, 0, 2343, 0, 0, 4655, 0, 0, 0 };
            data[4] = new double[27] { 28.48, 37.75, 131.4, 309.5, 0, 13.73, 167.5, 190.8, 408.3, 5.490, 473.9, 219.3, 37.06, 304.0, 1318, 4722, 33.29, 68.29, 0, 0, 92.99, 20.92, 378.1, 24.48, 347.3, 0, 0 };
            data[5] = new double[27] { 3.775, 29.85, 156.1, 388.1, 9.951, 0, 50.79, 210.7, 0, 73.43, -212.8, 235.7, 84.92, 217.1, 0, 5147, 20.93, -5.147, 0, 0, 150.0, 33.30, 517.1, 53.10, 0, 0, 0 };
            data[6] = new double[27] { 98.83, 25.05, 56.62, 170.5, 67.26, 41.18, 0, 16.47, 251.2, 65.54, 36.72, 253.6, 490.7, 6.177, 449.5, 6218, 78.92, 19.90, 61.42, 1.716, 189.1, 153.4, 590.5, 361.3, -23.7, 404.9, 0 };
            data[7] = new double[27] { 103.60, 5.147, 48.73, 128.3, 106.7, 67.94, -16.47, 0, -569.3, 53.53, -193.5, 374.4, 1712, -36.72, -736.4, 411.8, 67.94, 27.79, 880.2, -7.206, 1201, -231.1, 590.5, 0, -397.4, 2559.4, 0 };
            data[8] = new double[27] { 624.9, -17.84, 0, 0, 249.1, 0, 52.50, -328.0, 0, 277.6, -193.5, 276.6, 1889, -36.72, -736.4, -65.88, 3819, 589.5, 0, 0, 1463, -238.8, 590.5, 0, 0, 0, 0 };
            data[9] = new double[27] { 43.58, 8.579, 73.09, 208.6, 33.97, 12.70, 28.82, 37.40, 140.7, 0, 35.69, 354.1, 546.6, 166.4, 832.1, 13031, 52.50, 24.36, 140.7, 69.32, 192.5, 143.6, 0, 18666, 0, 281.4, 50.1 };
            data[10] = new double[27] { 293.4, 63.48, -120.8, 25.05, 188.0, 118.0, 129.0, -99.17, -99.17, 139.0, 0, -132.8, 389.8, -127.7, -337.7, -60.39, -647.2, 134.9, 0, 2.745, 34.31, 0, 0, 0, 0, 988.0, 0 };
            data[11] = new double[27] { 144.8, 141.4, 191.8, 377.5, 134.9, 136.2, 98.48, 154.4, 331.1, 144.1, 216.2, 0, 255.6, 201.4, 0, 277.9, 106.7, 183.9, -266.6, 66.91, 268.3, 158.9, 559.3, -1.612, 59.7, 142.9, 48.4 };
            data[12] = new double[27] { 38.09, 83.73, 383.6, 341.8, 30.88, 61.59, 185.3, 343.8, 702.4, 179.5, 331.5, 95.05, 0, 550.1, 0, 5490, 92.65, 227.2, 94.71, 0, 70.10, -25.40, 222.8, 8.774, 362.7, 7.5, 98.8 };
            data[13] = new double[27] { 159.6, 136.6, 192.5, 330.8, 181.9, 157.2, 21.28, 9.608, 9.608, 117.4, 71.37, 134.9, 319.5, 0, 153.7, 599.1, 0, 0, 0, 0, 823.5, 0, 0, 0, 0, 0, 0 };
            data[14] = new double[27] { 789.6, 439.9, 374.0, 685.9, 701.7, 0, 277.6, 1002, 1002, 493.1, 463.2, 0, 0, -157.0, 0, -113.0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            data[15] = new double[27] { 3557, 4324, 971.4, 0, 2265, 2333, 2268, 543.5, 1340, 4211, 244.0, 559.3, 2574, 603.9, 30.88, 0, 1661, 5199, 0, 0, -137.0, 0, 0, 0, 148.6, 1609.0, 0 };
            data[16] = new double[27] { 7.892, 59.71, 147.9, 366.8, 19.22, 7.549, 25.74, 97.80, 209.7, 35.34, 297.2, 73.09, 45.30, 0, 0, 1650, 0, 11.32, 6815, 1809, 165.1, -7.51, 536.7, 0, 0, 0, 0 };
            data[17] = new double[27] { 48.73, 9.608, 84.76, 181.2, 48.73, 26.77, 9.951, -48.38, 669.8, -15.44, 260.1, 60.74, 59.71, 0, 0, 2243, 14.76, 0, 121.8, -12.3, 373.0, 0, 687.7, -11.7, 26.8, 0, 0 };
            data[18] = new double[27] { 102.6, 64.85, 91.62, 0, 0, 0, -16.47, 343.1, 0, 159.6, 0, 74.81, 541.5, 0, 0, 0, -518.0, 24.71, 0, 87.50, 873.6, 0, 0, 0, -151.0, 0, 0 };
            data[19] = new double[27] { 47.01, 34.31, 0, 0, 0, 0, 3.775, 242.9, 0, 31.91, 151.3, 87.85, 0, 0, 0, 0, -98.8, 14.07, 23.68, 0, 2167, 0, 0, 0, 0, 0, 0 };
            data[20] = new double[27] { 174.0, 155.4, 326.0, 548.3, 156.1, 137.6, 288.9, 400.1, 602.9, 236.1, -51.82, 265.9, 65.20, 145.8, 0, 830.8, 151.3, 175.7, 621.4, 460.8, 0, 73.34, 95.49, 102.9, 0, 0, 0 };
            data[21] = new double[27] { 91.24, 44.00, 0, 0, 14.43, 15.42, 153.4, 125.77, 197.0, 113.1, 0, 106.4, 23.33, 0, 0, 0, 84.55, 0, 0, 0, 73.20, 0, 259.9, 8.180, 0, 0, 0 };
            data[22] = new double[27] { 416.3, 520.52, 728.1, 0, 394.5, 581.3, 753.6, 753.6, 753.6, 0, 0, 685.9, 204.7, 0, 0, 0, 569.6, 644.3, 0, 0, 138.7, 260.1, 0, 305.6, 0, 0, 0 };
            data[23] = new double[27] { 11.27, 113.6, 185.8, 899.0, 15.97, 43.81, 195.6, 0, 0, 1269, 0, 179.5, 6.488, 0, 0, 0, 0, 203.0, 0, 0, 128.2, 4.042, 243.1, 0, 766.0, 7.9, 0 };
            data[24] = new double[27] { 322.2, 55.9, -70.0, 0, 240.6, 0, 37.1, -196.6, 0, 0, 0, 46.0, 282.4, 0, 0, 374.4, 0, 26.8, -141.0, 0, 0, 0, 0, 340.1, 0, 665.7, 1343.0 };
            data[25] = new double[27] { 86.1, 107.4, 0, 0, 0, 0, 233.4, 177.1, 0, 181.2, 102.3, 163.4, 7.5, 0, 0, 1376.0, 0, 0, 0, 0, 0, 0, 0, 4.8, 340.0, 0, 0 };
            data[26] = new double[27] { 0, 0, 0, 0, 0, 0, 0, 0, 0, -27.5, 0, 5.1, 257.0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 172.3, 0, 0 };
        }

        public static double[,] CreateKij(Components bc, Temperature T)
        {
            double kappa1, alpha1, ac1, ai1, bi1;
            double kappa2, alpha2, ac2, ai2, bi2;
            BaseComp A, B;
            double[,] bips = new double[bc.Count, bc.Count];

            for (int x = 0; x < bc.Count; x++)
            {
                A = bc[x];
                for (int y = 0; y < bc.Count; y++)
                {
                    B = bc[y];

                    CreatePPRArray(A, B);

                    kappa1 = 0.37464 + 1.54226 * A.Omega - 0.26992 * A.Omega.Pow(2);
                    kappa2 = 0.37464 + 1.54226 * B.Omega - 0.26992 * B.Omega.Pow(2);

                    alpha1 = (1 + kappa1 * (1 - Math.Sqrt(T / A.CritT._Kelvin))).Pow(2);
                    alpha2 = (1 + kappa2 * (1 - Math.Sqrt(T / B.CritT._Kelvin))).Pow(2);

                    ac1 = 0.457235529 * ThermodynamicsClass.Rgas.Pow(2) * A.CritT._Kelvin.Pow(2) / A.CritP;
                    ac2 = 0.457235529 * ThermodynamicsClass.Rgas.Pow(2) * B.CritT._Kelvin.Pow(2) / B.CritP;

                    ai1 = alpha1 * ac1;
                    ai2 = alpha2 * ac2;

                    bi1 = 0.077796074 * ThermodynamicsClass.Rgas * A.CritT._Kelvin / A.CritP;
                    bi2 = 0.077796074 * ThermodynamicsClass.Rgas * B.CritT._Kelvin / B.CritP;

                    double res = Kij(T, ai1, ai2, bi1, bi2);
                    bips[x, y] = res;
                }
            }
            return bips;
        }

        private static void CreatePPRArray(BaseComp a, BaseComp b)
        {
            throw new NotImplementedException();
        }

        private static double Kij(Temperature T, double ai1, double ai2, double bi1, double bi2)
        {
            double adiff, bdiff, sum = 0, A = 0, B = 0;
            double sum2, sum3, res;
            int K, L;
            for (K = 1; K < 27; K++)
            {
                for (L = 1; L < 27; L++)
                {
                    adiff = PPR78EArray[1, K] - PPR78EArray[2, K];
                    bdiff = PPR78EArray[1, L] - PPR78EArray[2, L];
                    if (L != K)
                    {
                        A = GetA(K, L);
                        B = GetB(K, L);
                    }
                    if (A != 0)
                    {
                        sum = sum - 0.5 * adiff * bdiff * A * (298.15 / T).Pow(B / A - 1);
                    }
                }
            }

            sum2 = (ai1.Sqrt() / bi1 - ai2.Sqrt() / bi2).Sqr() / 10;
            sum3 = (2 * (ai1 * ai2).Sqrt()) / (bi1 * bi2) / 10;

            sum = sum - sum2;
            res = sum / sum3;
            return res;
        }

        private static double GetA(int K, int L)
        {
            int A, B;
            double Res;

            if (L > K)
            {
                A = L;
                B = K;
            }
            else
            {
                A = K;
                B = L;
            }

            Res = data[A][B];
            return Res;
        }

        private static double GetB(int K, int L)
        {
            int A, B;
            double Res;

            if (L > K)
            {
                A = L;
                B = K;
            }
            else
            {
                A = K;
                B = L;
            }

            Res = data[B][A];  // Bdata in upper triangle;
            return Res;
        }

        public static void GetUnifac(string text)
        {
            int Count, pos;
            double TtMolwt;
            const int ArrayLength = 55;
            const int PPR78ArrayLength = 29;
            string[] UGroups;
            double[] Molwt;

            UGroups = new string[]{"CH3", "CH2", "CH", "C", "CH4", "C2H6", "ACH", "ACCH", "ACCH2", "ACCH3", "AC", "Cf", "CH2cyclic", "CHcyc", "CO2", "N2", "H2S", "-SH", "H2O", "C2H4", "CH2=CH", "CH=CH",
                                    "CH2=C", "CH=C", "C=C", "CH2=", "C-=C", "CH-=C", "CH=", "C=", "CHcyc=", "Ccyc=", "CH2O", "COO", "CH2COO", "CH3COO", "OH", "HCOO", "CH2CO", "CH3CO", "CHO", "ACOH", "CH2NH", "CH2NH2",
                                    "CH3NH", "CHNH2", "CHNH", "CH3N", "NH2", "ACNH2", "C5H4N", "CH2N", "C5H3N", "ACNO2", "COOH", "H2"};

            Molwt = new double[]{15.03452, 14.02658, 13.01864, 12.0107, 16.04246, 30.06904, 13.01864, 25.02934, 26.03728, 27.04522, 12.0107, 12.0107, 14.02658, 13.01864, 44.0095, 28.0134, 34.03588,
            33.02794, 18.01528, 28.05316, 27.04522, 26.03728, 26.03728, 25.02934, 24.0214, 13.01864, 24.0214, 25.02934, 13.01864, 12.0107, 13.01864, 12.0107, 30.02598, 44.0095, 58.03608, 59.04402,
            17.00734, 45.017, 42.037, 43.044, 29.018, 29.018, 29.04122, 30.04916, 30.04916, 29.04, 28.033, 29.04, 16.022, 28.03328, 78.09196, 28.03328, 77.08402, 58.0162, 45.01744, 2.01588 };

            string[] CompGroupList = new string[40];
            double[] Frac = new double[20];
            double[] Result = new double[20];
            string[] PPR78 = new string[20];
            double[] PPR78amounts = new double[20];
            PPR78 = new string[PPR78ArrayLength] { "CH3", "CH2", "CH", "C", "CH4", "C2H6", "CHaro", "Caro", "Cfused", "CH2c", "CHc", "CO2", "N2", "H2S", "SH", "H2O", "C2H4", "CH2alk", "CHalk", "Calk", "CHcalk", "Ccalk", "H2", "CO2", "He", "Ar", "SO2", "O2", "NO" };

            double[] PPR78Molwt = new double[PPR78ArrayLength] { 15.035, 14.027, 13.019, 12.011, 16.042, 30.069, 13.019, 12.011, 12.011, 14.027, 13.019, 44.01, 28.013, 34.03588, 33.02794, 18.01528, 28.05316, 14.02658, 13.01864, 12.0107, 13.01864, 12.0107, 2.01588, 44.0095, 4.002602, 39.95, 64.0188, 31.9988, 30.0061 };
            double PPMW;

            TtMolwt = 0;
            PPMW = 0;
            Count = 0;

            int res;

            for (int n = 0; n < ArrayLength; n++)
            {
                CompGroupList[n] = "";
                Frac[n] = 0;
                Result[n] = 0;
                PPR78amounts[n] = 0;
            }
            if (text == "")
                return;
            do
            {
                res = text.IndexOf(" "); // (Start, text, " ");
                if (res > 0)
                {
                    int fin = text.Length - res;
                    Frac[Count] = Frac[Count] + 1;
                    CompGroupList[Count] = text.Substring(0, res - 1);
                    text = text.Substring(text.Length - res);
                    Count = Count + 1;
                }
            } while (res != 0);

            for (int n = 0; n < ArrayLength; n++)
            {
                pos = CompGroupList[n].IndexOf(")");
                if (pos != 0)
                {
                    Frac[n] = Convert.ToDouble(CompGroupList[n].Substring(pos + 1));
                    text = CompGroupList[n].Substring(2, pos - 2);
                    CompGroupList[n] = text;
                }
            }

            for (int n = 0; n < ArrayLength; n++)
                for (int i = 0; i < ArrayLength; i++)
                    if (UGroups[n] == CompGroupList[i])
                    {
                        Result[n] = Frac[i];
                        TtMolwt = TtMolwt + Molwt[n] * Result[n];
                    }

            for (int n = 0; n < ArrayLength; n++)
            {
                switch (n)
                {
                    case 0:
                        PPR78amounts[0] = PPR78amounts[0] + 1 * Result[n];
                        break;

                    case 1:
                        PPR78amounts[1] = PPR78amounts[1] + 1 * Result[n];
                        break;

                    case 2:
                        PPR78amounts[2] = PPR78amounts[2] + 1 * Result[n];
                        break;

                    case 3:
                        PPR78amounts[3] = PPR78amounts[3] + 1 * Result[n];
                        break;

                    case 4:
                        PPR78amounts[4] = PPR78amounts[4] + 1 * Result[n];
                        break;

                    case 5:
                        PPR78amounts[5] = PPR78amounts[5] + 1 * Result[n];
                        break;

                    case 6:
                        PPR78amounts[6] = PPR78amounts[6] + 1 * Result[n];
                        break;

                    case 7:
                        PPR78amounts[7] = PPR78amounts[7] + 1 * Result[n];
                        PPR78amounts[2] = PPR78amounts[2] + 1 * Result[n];
                        break;

                    case 8:
                        PPR78amounts[7] = PPR78amounts[7] + 1 * Result[n];
                        PPR78amounts[1] = PPR78amounts[1] + 1 * Result[n];
                        break;

                    case 9:
                        PPR78amounts[7] = PPR78amounts[7] + 1 * Result[n];
                        PPR78amounts[0] = PPR78amounts[0] + 1 * Result[n];
                        break;

                    case 10:
                        PPR78amounts[7] = PPR78amounts[7] + 1 * Result[n];
                        break;

                    case 11:
                    case 12:
                    case 13:
                    case 14:
                        PPR78amounts[11] = PPR78amounts[11] + 1 * Result[n];
                        break;

                    case 15:
                        PPR78amounts[12] = PPR78amounts[12] + 1 * Result[n];
                        break;

                    case 16:
                        PPR78amounts[12] = PPR78amounts[12] + 1 * Result[n];
                        break;

                    case 17:
                    case 18:
                        PPR78amounts[15] = PPR78amounts[15] + 1 * Result[n];
                        break;

                    case 19:
                        PPR78amounts[16] = PPR78amounts[16] + 1 * Result[n];
                        break;

                    case 20:
                        PPR78amounts[17] = PPR78amounts[17] + 1 * Result[n];
                        PPR78amounts[18] = PPR78amounts[18] + 1 * Result[n];
                        break;

                    case 21:
                        PPR78amounts[18] = PPR78amounts[18] + 1 * Result[n];
                        PPR78amounts[18] = PPR78amounts[18] + 1 * Result[n];
                        break;

                    case 22:
                        PPR78amounts[17] = PPR78amounts[17] + 1 * Result[n];
                        PPR78amounts[19] = PPR78amounts[19] + 1 * Result[n];
                        break;

                    case 23:
                        PPR78amounts[18] = PPR78amounts[18] + 1 * Result[n];
                        PPR78amounts[19] = PPR78amounts[19] + 1 * Result[n];
                        break;

                    case 24:
                        PPR78amounts[19] = PPR78amounts[19] + 1 * Result[n];
                        PPR78amounts[19] = PPR78amounts[19] + 1 * Result[n];
                        break;

                    case 25:
                        PPR78amounts[17] = PPR78amounts[17] + 1 * Result[n];
                        break;

                    case 26:
                        PPR78amounts[3] = PPR78amounts[3] + 1 * Result[n];
                        PPR78amounts[3] = PPR78amounts[3] + 1 * Result[n];
                        break;

                    case 27:
                        PPR78amounts[18] = PPR78amounts[18] + 1 * Result[n];
                        PPR78amounts[19] = PPR78amounts[19] + 1 * Result[n];
                        break;

                    case 28:
                        PPR78amounts[17] = PPR78amounts[17] + 1 * Result[n];
                        break;

                    case 29:
                        PPR78amounts[18] = PPR78amounts[18] + 1 * Result[n];
                        break;

                    case 30:
                        PPR78amounts[2] = PPR78amounts[2] + 1 * Result[n];
                        break;

                    case 31:
                        PPR78amounts[3] = PPR78amounts[3] + 1 * Result[n];
                        break;

                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                        PPR78amounts[20] = PPR78amounts[20] + 1 * Result[n];
                        break;
                }
            }

            for (int n = 0; n < PPR78ArrayLength - 1; n++)
                PPMW = PPMW + PPR78Molwt[n] * PPR78amounts[n];

            /*  for (int n = 0; n < PPR78ArrayLength; n++)
                  Worksheets["All HC's PPRGroups"].Range["f3"].Offset[Row, n] = PPR78amounts[n];

             /* for (int n = 0; n < ArrayLength; n++)
                  result[n] = Result[n];

              /*   If Result[n] <> 0 Then
                     If Result[n] = 1 Then
                         Formula = Formula & " " & UGroups[n]
                     Else
                         Formula = Formula & " [" & UGroups[n] & ")" & Result[n]
                     End If
                 End If
             Next

             For n = 0 To PPR78ArrayLength
                 If PPR78amounts[n] <> 0 Then
                     If PPR78amounts[n] = 1 Then
                         PPR78Formula = PPR78Formula & " " & PPR78[n]
                     Else
                         PPR78Formula = PPR78Formula & " [" & PPR78[n] & "]" & PPR78amounts[n]
                     End If
                 End If
             Next

             Worksheets["All HC's PPRGroups").Range["A3").Offset[Row, 0).Value = Formula
             Worksheets["All HC's PPRGroups").Range["A3").Offset[Row, 1).Value = PPR78Formula
             Worksheets["All HC's PPRGroups").Range["A3").Offset[Row, 2).Value = PPMW
             .Range["W2").Offset[Row, 1).Value = PPR78Formula
             .Range["W2").Offset[Row, 3).Value = Formula
             .Range["W2").Offset[Row, 4).Value = TtMolwt
             .Range["W2").Offset[Row, 5).Value = PPMW
             Row = Row + 1
             Loop Until[Row = 650)

             e:*/
        }
    }
}