using Units.UOM;

namespace ModelEngine.UnitOperations.RK4New
{
    internal partial class RK4New
    {
        public void TestRateofChange(double[] Rxn)
        {
            double[] RxnRate = new double[Rxn.Length + 2];

            for (int i = 1; i < Rxn.Length + 1; i++)
            {
                RxnRate[i] = Rxn[i - 1];
            }

            SetHCDistrib();

            RateOfChange[1] = 3D * (RxnRate[1] + RxnRate[2] + RxnRate[3] +
                RxnRate[4] + RxnRate[5] + RxnRate[6] + RxnRate[7] +
                RxnRate[8] + RxnRate[9] + RxnRate[10]);

            RateOfChange[1] = RateOfChange[1] - (RxnRate[30] + RxnRate[31] +
                RxnRate[32] + RxnRate[33] + RxnRate[34] + RxnRate[35] +
                RxnRate[36] + RxnRate[37] + RxnRate[38] + RxnRate[39]);

            RateOfChange[1] = RateOfChange[1] - (RxnRate[40] + RxnRate[41] +
                RxnRate[42] + RxnRate[45] + RxnRate[46] + RxnRate[47] +
                RxnRate[48] + RxnRate[49] + RxnRate[50] + RxnRate[51] +
                RxnRate[52] + RxnRate[53] + RxnRate[54] + RxnRate[57] + RxnRate[58]);

            RateOfChange[1] = RateOfChange[1] - (RxnRate[59] + RxnRate[60] +
                RxnRate[61] + RxnRate[62] + RxnRate[63] + RxnRate[64] + RxnRate[65] +
                RxnRate[66] + RxnRate[67] + RxnRate[68] + RxnRate[69]);

            RateOfChange[1] = RateOfChange[1] - 2D * (RxnRate[71] + RxnRate[72] +
                RxnRate[73] + RxnRate[74] + RxnRate[75] + RxnRate[76] +
                RxnRate[77] + RxnRate[78] + RxnRate[79] + RxnRate[80]);

            RateOfChange[2] = RxnRate[57] + RxnRate[58] + RxnRate[59] +
                RxnRate[60] + RxnRate[61] + RxnRate[62] + RxnRate[63] +
                RxnRate[64] + RxnRate[65] + RxnRate[66] + RxnRate[67] +
                RxnRate[68] + RxnRate[69] + CrackProd(1, RxnRate);

            RateOfChange[3] = CrackProd(2, RxnRate);

            //    RateOfChange[3] = CrackProd[2, RxnRate] + C9plusFact[2] * [RxnRate[63] + _
            //'        RxnRate[64]] + C9plusFact[3] * [RxnRate[65] + RxnRate[66]] + _
            //'        C9plusFact[4] * [RxnRate[69]]

            RateOfChange[4] = CrackProd(3, RxnRate);
            RateOfChange[5] = CrackProd(4, RxnRate) + RxnRate[27] - RxnRate[53];
            RateOfChange[6] = CrackProd(5, RxnRate) - RxnRate[27] - RxnRate[54];
            RateOfChange[7] = CrackProd(6, RxnRate) + RxnRate[26] - RxnRate[51];
            RateOfChange[8] = CrackProd(7, RxnRate) - RxnRate[26] - RxnRate[52];
            RateOfChange[9] = CrackProd(8, RxnRate) + RxnRate[25] + RxnRate[32] - RxnRate[49];
            RateOfChange[10] = CrackProd(9, RxnRate) - RxnRate[25] + RxnRate[30] + RxnRate[31] - RxnRate[50];

            RateOfChange[11] = RxnRate[57] - RxnRate[30] - RxnRate[17] - RxnRate[1] - RxnRate[79];

            RateOfChange[12] = RxnRate[58] - RxnRate[31] - RxnRate[32] + RxnRate[17] - RxnRate[2] - RxnRate[80];

            RateOfChange[13] = RxnRate[1] + RxnRate[2] + RxnRate[67];

            RateOfChange[14] = CrackProd(10, RxnRate) + RxnRate[24] + RxnRate[34] + RxnRate[36] - RxnRate[47];
            RateOfChange[15] = CrackProd(11, RxnRate) - RxnRate[24] + RxnRate[33] + RxnRate[35] - RxnRate[48];

            RateOfChange[16] = RxnRate[59] + RxnRate[60] - RxnRate[57] - RxnRate[33] - RxnRate[34] - RxnRate[18] - RxnRate[3] - RxnRate[77];
            RateOfChange[17] = RxnRate[61] + RxnRate[62] - RxnRate[58] - RxnRate[35] - RxnRate[36] + RxnRate[18] - RxnRate[4] - RxnRate[78];
            RateOfChange[18] = RxnRate[68] - RxnRate[67] + RxnRate[3] + RxnRate[4];

            RateOfChange[19] = CrackProd(12, RxnRate) + RxnRate[37] + RxnRate[38] + RxnRate[39] + RxnRate[40] - RxnRate[46];

            RateOfChange[20] = RxnRate[63] - RxnRate[59] - RxnRate[37] + RxnRate[22] - RxnRate[19] - RxnRate[5] - RxnRate[73];
            RateOfChange[21] = RxnRate[64] - RxnRate[60] - RxnRate[38] + RxnRate[21] - RxnRate[22] - RxnRate[6] - RxnRate[74];
            RateOfChange[22] = RxnRate[65] - RxnRate[61] - RxnRate[39] + RxnRate[19] - RxnRate[20] - RxnRate[7] - RxnRate[75];
            RateOfChange[23] = RxnRate[66] - RxnRate[62] - RxnRate[40] + RxnRate[20] - RxnRate[21] - RxnRate[8] - RxnRate[76];

            // The next four components participated in reaction as a mixed
            // component.  Factors are used to find the fractional production.
            RateOfChange[24] = RxnRate[5] + RxnRate[7] + 0.096 * (RxnRate[69] - RxnRate[68]);
            RateOfChange[25] = RxnRate[13] - RxnRate[14] + 0.232 * (RxnRate[6] + RxnRate[8]) + 0.209 * (RxnRate[69] - RxnRate[68]);
            RateOfChange[26] = RxnRate[14] - RxnRate[15] + 0.395 * (RxnRate[6] + RxnRate[8]) + 0.35 * (RxnRate[69] - RxnRate[68]);
            RateOfChange[27] = RxnRate[15] - RxnRate[13] + 0.373 * (RxnRate[6] + RxnRate[8]) + 0.345 * (RxnRate[69] - RxnRate[68]);

            RateOfChange[28] = RxnRate[41] + RxnRate[42] - RxnRate[45];
            RateOfChange[29] = -RxnRate[9] - RxnRate[23] - RxnRate[41] - RxnRate[63] - RxnRate[64] - RxnRate[71];
            RateOfChange[30] = -RxnRate[10] + RxnRate[23] - RxnRate[42] - RxnRate[65] - RxnRate[66] - RxnRate[72];
            RateOfChange[31] = RxnRate[9] + RxnRate[10] - RxnRate[69];
        }

        private double[,] HCDistrib = new double[20, 12];

        private DeltaTemperature UpdateTempTemp(Port_Material port)
        {
            DeltaHForm = temp_cc.Hform25(MoleDeltas);
            Cp = temp_cc.CP(port.T, port.Q);
            deltaT = DeltaHForm / Cp;
            return deltaT;
        }

        public double CrackProd(int I, double[] RxnRate)
        {
            // Cracked products are based on a distribution saved in
            // the array HCDistrib (hydrocracking products distribution array)
            I -= 1;

            double CrackProd = RxnRate[45] * HCDistrib[0, I]
                + RxnRate[71] * HCDistrib[1, I]
                + RxnRate[72] * HCDistrib[2, I]
                + RxnRate[46] * HCDistrib[3, I]
                + RxnRate[73] * HCDistrib[4, I]
                + RxnRate[74] * HCDistrib[5, I]
                + RxnRate[75] * HCDistrib[6, I]
                + RxnRate[76] * HCDistrib[7, I]
                + RxnRate[47] * HCDistrib[8, I]
                + RxnRate[48] * HCDistrib[9, I]
                + RxnRate[77] * HCDistrib[10, I]
                + RxnRate[78] * HCDistrib[11, I]
                + RxnRate[49] * HCDistrib[12, I]
                + RxnRate[50] * HCDistrib[13, I]
                + RxnRate[79] * HCDistrib[14, I]
                + RxnRate[80] * HCDistrib[15, I]
                + RxnRate[51] * HCDistrib[16, I]
                + RxnRate[52] * HCDistrib[17, I]
                + RxnRate[53] * HCDistrib[18, I]
                + RxnRate[54] * HCDistrib[19, I];
            return CrackProd;
        }

        public void SetHCDistrib()
        {
            HCDistrib[0, 0] = 0.176;
            HCDistrib[1, 0] = 0.176;
            HCDistrib[2, 0] = 0.176;
            HCDistrib[3, 0] = 0.199;
            HCDistrib[4, 0] = 0.199;
            HCDistrib[5, 0] = 0.199;
            HCDistrib[6, 0] = 0.199;
            HCDistrib[7, 0] = 0.199;
            HCDistrib[8, 0] = 0.237;
            HCDistrib[9, 0] = 0.237;
            HCDistrib[10, 0] = 0.237;
            HCDistrib[11, 0] = 0.237;
            HCDistrib[12, 0] = 0.288;
            HCDistrib[13, 0] = 0.288;
            HCDistrib[14, 0] = 0.288;
            HCDistrib[15, 0] = 0.288;
            HCDistrib[16, 0] = 0.5;
            HCDistrib[17, 0] = 0.5;
            HCDistrib[18, 0] = 0.665;
            HCDistrib[19, 0] = 0.665;

            HCDistrib[0, 1] = 0.283;
            HCDistrib[1, 1] = 0.283;
            HCDistrib[2, 1] = 0.283;
            HCDistrib[3, 1] = 0.295;
            HCDistrib[4, 1] = 0.322;
            HCDistrib[5, 1] = 0.322;
            HCDistrib[6, 1] = 0.322;
            HCDistrib[7, 1] = 0.322;
            HCDistrib[8, 1] = 0.366;
            HCDistrib[9, 1] = 0.366;
            HCDistrib[10, 1] = 0.366;
            HCDistrib[11, 1] = 0.366;
            HCDistrib[12, 1] = 0.462;
            HCDistrib[13, 1] = 0.462;
            HCDistrib[14, 1] = 0.462;
            HCDistrib[15, 1] = 0.462;
            HCDistrib[16, 1] = 0.5;
            HCDistrib[17, 1] = 0.5;
            HCDistrib[18, 1] = 0.67;
            HCDistrib[19, 1] = 0.67;
            HCDistrib[0, 2] = 0.286;
            HCDistrib[1, 2] = 0.308;
            HCDistrib[2, 2] = 0.308;
            HCDistrib[3, 2] = 0.376;
            HCDistrib[4, 2] = 0.349;
            HCDistrib[5, 2] = 0.349;
            HCDistrib[6, 2] = 0.349;
            HCDistrib[7, 2] = 0.349;
            HCDistrib[8, 2] = 0.397;
            HCDistrib[9, 2] = 0.397;
            HCDistrib[10, 2] = 0.397;
            HCDistrib[11, 2] = 0.397;
            HCDistrib[12, 2] = 0.5;
            HCDistrib[13, 2] = 0.5;
            HCDistrib[14, 2] = 0.5;
            HCDistrib[15, 2] = 0.5;
            HCDistrib[16, 2] = 0.5;
            HCDistrib[17, 2] = 0.5;
            HCDistrib[18, 2] = 0.665;
            HCDistrib[19, 2] = 0.665;
            HCDistrib[0, 3] = 0.091;
            HCDistrib[1, 3] = 0.083;
            HCDistrib[2, 3] = 0.083;
            HCDistrib[3, 3] = 0.092;
            HCDistrib[4, 3] = 0.092;
            HCDistrib[5, 3] = 0.092;
            HCDistrib[6, 3] = 0.092;
            HCDistrib[7, 3] = 0.092;
            HCDistrib[8, 3] = 0.141;
            HCDistrib[9, 3] = 0.141;
            HCDistrib[10, 3] = 0.141;
            HCDistrib[11, 3] = 0.141;
            HCDistrib[12, 3] = 0.164;
            HCDistrib[13, 3] = 0.164;
            HCDistrib[14, 3] = 0.164;
            HCDistrib[15, 3] = 0.164;
            HCDistrib[16, 3] = 0.2;
            HCDistrib[17, 3] = 0.2;
            HCDistrib[18, 3] = 0.0;
            HCDistrib[19, 3] = 0.0;
            HCDistrib[0, 4] = 0.164;
            HCDistrib[1, 4] = 0.15;
            HCDistrib[2, 4] = 0.15;
            HCDistrib[3, 4] = 0.168;
            HCDistrib[4, 4] = 0.168;
            HCDistrib[5, 4] = 0.168;
            HCDistrib[6, 4] = 0.168;
            HCDistrib[7, 4] = 0.168;
            HCDistrib[8, 4] = 0.256;
            HCDistrib[9, 4] = 0.256;
            HCDistrib[10, 4] = 0.256;
            HCDistrib[11, 4] = 0.256;
            HCDistrib[12, 4] = 0.298;
            HCDistrib[13, 4] = 0.298;
            HCDistrib[14, 4] = 0.298;
            HCDistrib[15, 4] = 0.298;
            HCDistrib[16, 4] = 0.3;
            HCDistrib[17, 4] = 0.3;
            HCDistrib[18, 4] = 0.0;
            HCDistrib[19, 4] = 0.0;
            HCDistrib[0, 5] = 0.172;
            HCDistrib[1, 5] = 0.15;
            HCDistrib[2, 5] = 0.15;
            HCDistrib[3, 5] = 0.26;
            HCDistrib[4, 5] = 0.233;
            HCDistrib[5, 5] = 0.233;
            HCDistrib[6, 5] = 0.233;
            HCDistrib[7, 5] = 0.233;
            HCDistrib[8, 5] = 0.234;
            HCDistrib[9, 5] = 0.234;
            HCDistrib[10, 5] = 0.234;
            HCDistrib[11, 5] = 0.234;
            HCDistrib[12, 5] = 0.192;
            HCDistrib[13, 5] = 0.192;
            HCDistrib[14, 5] = 0.192;
            HCDistrib[15, 5] = 0.192;
            HCDistrib[16, 5] = 0.0;
            HCDistrib[17, 5] = 0.0;
            HCDistrib[18, 5] = 0.0;
            HCDistrib[19, 5] = 0.0;
            HCDistrib[0, 6] = 0.083;
            HCDistrib[1, 6] = 0.083;
            HCDistrib[2, 6] = 0.083;
            HCDistrib[3, 6] = 0.116;
            HCDistrib[4, 6] = 0.116;
            HCDistrib[5, 6] = 0.116;
            HCDistrib[6, 6] = 0.116;
            HCDistrib[7, 6] = 0.116;
            HCDistrib[8, 6] = 0.132;
            HCDistrib[9, 6] = 0.132;
            HCDistrib[10, 6] = 0.132;
            HCDistrib[11, 6] = 0.132;
            HCDistrib[12, 6] = 0.096;
            HCDistrib[13, 6] = 0.096;
            HCDistrib[14, 6] = 0.096;
            HCDistrib[15, 6] = 0.096;
            HCDistrib[16, 6] = 0.0;
            HCDistrib[17, 6] = 0.0;
            HCDistrib[18, 6] = 0.0;
            HCDistrib[19, 6] = 0.0;
            HCDistrib[0, 7] = 0.206;
            HCDistrib[1, 7] = 0.206;
            HCDistrib[2, 7] = 0.206;
            HCDistrib[3, 7] = 0.215;
            HCDistrib[4, 7] = 0.215;
            HCDistrib[5, 7] = 0.215;
            HCDistrib[6, 7] = 0.215;
            HCDistrib[7, 7] = 0.215;
            HCDistrib[8, 7] = 0.158;
            HCDistrib[9, 7] = 0.158;
            HCDistrib[10, 7] = 0.158;
            HCDistrib[11, 7] = 0.158;
            HCDistrib[12, 7] = 0.0;
            HCDistrib[13, 7] = 0.0;
            HCDistrib[14, 7] = 0.0;
            HCDistrib[15, 7] = 0.0;
            HCDistrib[16, 7] = 0.0;
            HCDistrib[17, 7] = 0.0;
            HCDistrib[18, 7] = 0.0;
            HCDistrib[19, 7] = 0.0;
            HCDistrib[0, 8] = 0.08;
            HCDistrib[1, 8] = 0.102;
            HCDistrib[2, 8] = 0.102;
            HCDistrib[3, 8] = 0.08;
            HCDistrib[4, 8] = 0.107;
            HCDistrib[5, 8] = 0.107;
            HCDistrib[6, 8] = 0.107;
            HCDistrib[7, 8] = 0.107;
            HCDistrib[8, 8] = 0.079;
            HCDistrib[9, 8] = 0.079;
            HCDistrib[10, 8] = 0.079;
            HCDistrib[11, 8] = 0.079;
            HCDistrib[12, 8] = 0.0;
            HCDistrib[13, 8] = 0.0;
            HCDistrib[14, 8] = 0.0;
            HCDistrib[15, 8] = 0.0;
            HCDistrib[16, 8] = 0.0;
            HCDistrib[17, 8] = 0.0;
            HCDistrib[18, 8] = 0.0;
            HCDistrib[19, 8] = 0.0;
            HCDistrib[0, 9] = 0.19;
            HCDistrib[1, 9] = 0.19;
            HCDistrib[2, 9] = 0.19;
            HCDistrib[3, 9] = 0.133;
            HCDistrib[4, 9] = 0.133;
            HCDistrib[5, 9] = 0.133;
            HCDistrib[6, 9] = 0.133;
            HCDistrib[7, 9] = 0.133;
            HCDistrib[8, 9] = 0.0;
            HCDistrib[9, 9] = 0.0;
            HCDistrib[10, 9] = 0.0;
            HCDistrib[11, 9] = 0.0;
            HCDistrib[12, 9] = 0.0;
            HCDistrib[13, 9] = 0.0;
            HCDistrib[14, 9] = 0.0;
            HCDistrib[15, 9] = 0.0;
            HCDistrib[16, 9] = 0.0;
            HCDistrib[17, 9] = 0.0;
            HCDistrib[18, 9] = 0.0;
            HCDistrib[19, 9] = 0.0;
            HCDistrib[0, 10] = 0.093;
            HCDistrib[1, 10] = 0.093;
            HCDistrib[2, 10] = 0.093;
            HCDistrib[3, 10] = 0.066;
            HCDistrib[4, 10] = 0.066;
            HCDistrib[5, 10] = 0.066;
            HCDistrib[6, 10] = 0.066;
            HCDistrib[7, 10] = 0.066;
            HCDistrib[8, 10] = 0.0;
            HCDistrib[9, 10] = 0.0;
            HCDistrib[10, 10] = 0.0;
            HCDistrib[11, 10] = 0.0;
            HCDistrib[12, 10] = 0.0;
            HCDistrib[13, 10] = 0.0;
            HCDistrib[14, 10] = 0.0;
            HCDistrib[15, 10] = 0.0;
            HCDistrib[16, 10] = 0.0;
            HCDistrib[17, 10] = 0.0;
            HCDistrib[18, 10] = 0.0;
            HCDistrib[19, 10] = 0.0;
            HCDistrib[0, 11] = 0.176;
            HCDistrib[1, 11] = 0.176;
            HCDistrib[2, 11] = 0.176;
            HCDistrib[3, 11] = 0.0;
            HCDistrib[4, 11] = 0.0;
            HCDistrib[5, 11] = 0.0;
            HCDistrib[6, 11] = 0.0;
            HCDistrib[7, 11] = 0.0;
            HCDistrib[8, 11] = 0.0;
            HCDistrib[9, 11] = 0.0;
            HCDistrib[10, 11] = 0.0;
            HCDistrib[11, 11] = 0.0;
            HCDistrib[12, 11] = 0.0;
            HCDistrib[13, 11] = 0.0;
            HCDistrib[14, 11] = 0.0;
            HCDistrib[15, 11] = 0.0;
            HCDistrib[16, 11] = 0.0;
            HCDistrib[17, 11] = 0.0;
            HCDistrib[18, 11] = 0.0;
            HCDistrib[19, 11] = 0.0;
        }
    }
}