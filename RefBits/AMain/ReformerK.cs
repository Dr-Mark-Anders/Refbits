using System.Windows.Forms;

namespace ReformerCS
{
    using System;

    public class ReformerCS
    {
        public double A_LV;
        private double A_WT;
        private double[] A1 = new double[51];
        private double[] A2 = new double[51];
        private double[] AcidAct = new double[5];
        private double AcidActiv;
        private double AcidActiv2;
        private double AcidActiv3;
        private double AcidActiv4;
        private double[] AmtCat = new double[5];
        private double AmtCat1;
        private double AmtCat2;
        private double AmtCat3;
        private double AmtCat4;
        private double APIFeed;
        private double[] B1 = new double[0x51];
        private double[] B2 = new double[0x51];
        private double[] BpdFeed = new double[0x20];
        private double[] C9plusFact = new double[7];
        private string CatalystUnits;
        private double[] CatPercent = new double[5];
        private double CHDehydrogTerm;
        private const int ColEff = 8;
        private const int ColFeed = 2;
        private const int ColGas = 10;
        private const int ColRecy = 3;
        private const int ColRef = 9;
        private double[,] CpCoeft = new double[0x20, 5];
        private double CPDehydrogTerm;
        private double CyclTerm;
        public double[] D86 = new double[8];
        private double DealkTerm;
        private double DHCH = 1;
        private double DHCHFact;
        private double DHCP = 1;
        private double DHCPFact;
        private double ISOM = 1;
        private double NHC = 1;
        private double DP = 0.5;
        private double DP2 = 0.5;
        private double DP3 = 0.5;
        private double DP4 = 0.5;
        private const double FactorBPDtoLbmHr = 14.574166;
        private double[] FurnEffic = new double[4];
        private double[,] HCDistrib = new double[0x15, 13];
        private double HDA = 1;
        private double HDAFact = 1;
        private double InitialP;
        private double InitialP2;
        private double InitialP3;
        private double InitialP4;
        private double InitialTemp;
        private double InitialTemp2;
        private double InitialTemp3;
        private double InitialTemp4;
        private double[] InletP = new double[5];
        private int InputOption;
        private double ISOMFact;
        private double IsomTerm;
        private double[] LbsFeed = new double[0x20];
        private const int MaxNumReactor = 4;
        private double[] MetalAct = new double[5];
        private double MetalActiv;
        private double MetalActiv2;
        private double MetalActiv3;
        private double MetalActiv4;
        private double[] MolFeed = new double[0x20];
        private double MolH2Recy;
        private double[] MolVol = new double[0x20];
        private double[] MON = new double[0x20];
        private double[] MW = new double[0x2b];
        public double N_LV;
        private double N_WT;
        private double NaphBpdFeed;
        public double NaphFeedRate;
        private double NaphLbsFeed;
        private double NaphMolFeed;
        private double[] NBP = new double[0x20];
        private double NCrackTerm;
        private double NHCFact;
        private const int NumComp = 0x1f;
        private const int NumDepVar = 0x20;
        private int NumReactor;
        private const int NumRxn = 80;
        private double OctSpec;
        public double[] offgas;
        private double OPEN = 1;
        private double OPENFact = 1;
        private bool OptionShort = true;
        public double P_LV;
        private double P_WT;
        private double PCrackTerm;
        private double[] PCrit = new double[0x20];
        private double PHC = 1;
        private double PHCFact = 1;
        private string PressUnits;
        private double PSep;
        private double[] ReactorP = new double[5];
        private double[] ReactorT_In = new double[5];
        public double RefC5PlusLV;
        public double[] Reformate;
        private double[] RON = new double[0x20];
        private const int RowBPD = 0x4e;
        private const int RowLbs = 0x29;
        private const int RowMol = 4;
        private double[] RVP = new double[0x20];
        private double[] SG = new double[0x2b];
        private double SGFeed;
        private double[] Sol = new double[0x20];
        private bool SpecOct = true;
        private double[] StdHeatForm = new double[0x20];
        public double SumMON;
        public double SumRON;
        public double SumRVP;
        private double[] TCrit = new double[0x20];
        private string TempUnits;
        private double TotWHSV;
        private double TSep;
        private double[] W = new double[0x20];

        private double APIToSG(double value)
        {
            return (141.5 / (value + 131.5));
        }

        private void AssignRecycle(double[] F_Vap, double[] F_NetGas, double[] F_Recy)
        {
            double FracRecy = 0;
            if (F_Vap[1] < MolH2Recy)
            {
                string message = "Not enough hydrogen for simulation to continue";
                EndWell(message);
            }
            else
            {
                FracRecy = MolH2Recy / F_Vap[1];
            }
            int index = 1;
            do
            {
                F_Recy[index] = F_Vap[index] * FracRecy;
                F_NetGas[index] = F_Vap[index] - F_Recy[index];
                index++;
            }
            while (index <= NumComp);
        }

        private double AtmToKgPerCm2(double value)
        {
            return (value * 1.033227555);
        }

        private double AtmToKgPerCm2G(double value)
        {
            return ((value - 1.0) * 1.033227555);
        }

        private double AtmToPsia(double value)
        {
            return (value * 14.6959488);
        }

        private double BblToM3(double value)
        {
            return (value * 0.1589873);
        }

        private void CalcOctane(double[] RefMol, double SumRON, double SumMON)
        {
            int num4;
            double[] numArray2 = new double[0x20];
            double[] numArray = new double[0x20];
            double num = 0.0;
            double num2 = 0.0;
            SumRON = 0.0;
            SumMON = 0.0;
            int index = 7;
            do
            {
                numArray2[index] = RefMol[index] * MW[index];
                numArray[index] = numArray2[index] / (SG[index] * 14.574166);
                num += numArray[index];
                num2 += numArray2[index];
                SumRON += RON[index] * numArray[index];
                SumMON += MON[index] * numArray[index];
                index++;
                num4 = 0x1f;
            }
            while (index <= num4);
            SumRON /= num;
            SumMON /= num;
        }

        private void CalcRates(double TR, double PR, double[] FlowRate, double[] RateOfChange)
        {
            int num48;
            double[] numArray2 = new double[0x51];
            double[] numArray = new double[0x51];
            double[] rxnRate = new double[0x51];
            double num14 = 0.0;
            int index = 1;
            do
            {
                num14 += FlowRate[index];
                index++;
                num48 = 0x1f;
            }
            while (index <= num48);
            double num31 = FlowRate[1] / num14;
            double num16 = FlowRate[2] / NaphMolFeed;
            double num17 = FlowRate[3] / NaphMolFeed;
            double num18 = FlowRate[4] / NaphMolFeed;
            double num32 = FlowRate[5] / NaphMolFeed;
            double num39 = FlowRate[6] / NaphMolFeed;
            double num33 = FlowRate[7] / NaphMolFeed;
            double num40 = FlowRate[8] / NaphMolFeed;
            double num34 = FlowRate[9] / NaphMolFeed;
            double num41 = FlowRate[10] / NaphMolFeed;
            double num27 = FlowRate[11] / NaphMolFeed;
            double num37 = FlowRate[12] / NaphMolFeed;
            double num15 = FlowRate[13] / NaphMolFeed;
            double num35 = FlowRate[14] / NaphMolFeed;
            double num42 = FlowRate[15] / NaphMolFeed;
            double num36 = FlowRate[0x10] / NaphMolFeed;
            double num19 = FlowRate[0x11] / NaphMolFeed;
            double num46 = FlowRate[0x12] / NaphMolFeed;
            double num22 = FlowRate[0x13] / NaphMolFeed;
            double num30 = FlowRate[20] / NaphMolFeed;
            double num28 = FlowRate[0x15] / NaphMolFeed;
            double num44 = FlowRate[0x16] / NaphMolFeed;
            double num21 = FlowRate[0x17] / NaphMolFeed;
            double num29 = FlowRate[0x18] / NaphMolFeed;
            double num45 = FlowRate[0x19] / NaphMolFeed;
            double num38 = FlowRate[0x1a] / NaphMolFeed;
            double num43 = FlowRate[0x1b] / NaphMolFeed;
            double num26 = FlowRate[0x1c] / NaphMolFeed;
            double num24 = FlowRate[0x1d] / NaphMolFeed;
            double num25 = FlowRate[30] / NaphMolFeed;
            double num23 = FlowRate[0x1f] / NaphMolFeed;
            index = 1;
            do
            {
                numArray2[index] = Math.Exp(A1[index] + A2[index] / TR);
                numArray[index] = Math.Exp(B1[index] + B2[index] / TR);
                index++;
                num48 = 80;
            }
            while (index <= num48);
            double num47 = (num45 + num38) + num43;
            double num20 = num29 + num47;
            double num2 = Math.Pow(num31 * PR, 3.0);
            rxnRate[1] = (numArray2[1] * CHDehydrogTerm) * (num27 - ((num2 * num15) / numArray[1]));
            rxnRate[2] = (numArray2[2] * CPDehydrogTerm) * num37;
            rxnRate[3] = (numArray2[3] * CHDehydrogTerm) * (num36 - ((num2 * num46) / numArray[3]));
            rxnRate[4] = (numArray2[4] * CPDehydrogTerm) * (num19 - ((num2 * num46) / numArray[4]));
            rxnRate[5] = (numArray2[5] * CHDehydrogTerm) * (num30 - ((num2 * num29) / numArray[5]));
            rxnRate[6] = (numArray2[6] * CHDehydrogTerm) * (num28 - ((num2 * num47) / numArray[6]));
            rxnRate[7] = (numArray2[7] * CPDehydrogTerm) * (num44 - ((num2 * num29) / numArray[7]));
            rxnRate[8] = (numArray2[8] * CPDehydrogTerm) * (num21 - ((num2 * num47) / numArray[8]));
            rxnRate[9] = (numArray2[9] * CHDehydrogTerm) * (num24 - ((num2 * num23) / numArray[9]));
            rxnRate[10] = (numArray2[10] * CPDehydrogTerm) * (num25 - ((num2 * num23) / numArray[10]));
            rxnRate[13] = (numArray2[13] * IsomTerm) * (num43 - (num45 / numArray[13]));
            rxnRate[14] = (numArray2[14] * IsomTerm) * (num45 - (num38 / numArray[14]));
            rxnRate[15] = (numArray2[15] * IsomTerm) * (num38 - (num43 / numArray[15]));
            rxnRate[0x11] = (numArray2[0x11] * IsomTerm) * (num27 - (num37 / numArray[0x11]));
            rxnRate[0x12] = (numArray2[0x12] * IsomTerm) * (num36 - (num19 / numArray[0x12]));
            rxnRate[0x13] = (numArray2[0x13] * IsomTerm) * (num30 - (num44 / numArray[0x13]));
            rxnRate[20] = (numArray2[20] * IsomTerm) * (num44 - (num21 / numArray[20]));
            rxnRate[0x15] = (numArray2[0x15] * IsomTerm) * (num21 - (num28 / numArray[0x15]));
            rxnRate[0x16] = (numArray2[0x16] * IsomTerm) * (num28 - (num30 / numArray[0x16]));
            rxnRate[0x17] = (numArray2[0x17] * IsomTerm) * (num24 - (num25 / numArray[0x17]));
            rxnRate[0x18] = (numArray2[0x18] * IsomTerm) * (num42 - (num35 / numArray[0x18]));
            rxnRate[0x19] = (numArray2[0x19] * IsomTerm) * (num41 - (num34 / numArray[0x19]));
            rxnRate[0x1a] = (numArray2[0x1a] * IsomTerm) * (num40 - (num33 / numArray[0x1a]));
            rxnRate[0x1b] = (numArray2[0x1b] * IsomTerm) * (num39 - (num32 / numArray[0x1b]));
            num2 = num31 * PR;
            rxnRate[30] = (numArray2[30] * CyclTerm) * ((num27 * num2) - (num41 / numArray[30]));
            rxnRate[0x1f] = (numArray2[0x1f] * CyclTerm) * ((num37 * num2) - (num41 / numArray[0x1f]));
            rxnRate[0x20] = (numArray2[0x20] * CyclTerm) * ((num37 * num2) - (num34 / numArray[0x20]));
            rxnRate[0x21] = (numArray2[0x21] * CyclTerm) * ((num36 * num2) - (num42 / numArray[0x21]));
            rxnRate[0x22] = (numArray2[0x22] * CyclTerm) * ((num36 * num2) - (num35 / numArray[0x22]));
            rxnRate[0x23] = (numArray2[0x23] * CyclTerm) * ((num19 * num2) - (num42 / numArray[0x23]));
            rxnRate[0x24] = (numArray2[0x24] * CyclTerm) * ((num19 * num2) - (num35 / numArray[0x24]));
            rxnRate[0x25] = (numArray2[0x25] * CyclTerm) * ((num30 * num2) - (num22 / numArray[0x25]));
            rxnRate[0x26] = (numArray2[0x26] * CyclTerm) * ((num28 * num2) - (num22 / numArray[0x26]));
            rxnRate[0x27] = (numArray2[0x27] * CyclTerm) * ((num44 * num2) - (num22 / numArray[0x27]));
            rxnRate[40] = (numArray2[40] * CyclTerm) * ((num21 * num2) - (num22 / numArray[40]));
            rxnRate[0x29] = (numArray2[0x29] * CyclTerm) * ((num24 * num2) - (num26 / numArray[0x29]));
            rxnRate[0x2a] = (numArray2[0x2a] * CyclTerm) * ((num25 * num2) - (num26 / numArray[0x2a]));
            rxnRate[0x2d] = (numArray2[0x2d] * PCrackTerm) * num26;
            rxnRate[0x2e] = (numArray2[0x2e] * PCrackTerm) * num22;
            rxnRate[0x2f] = (numArray2[0x2f] * PCrackTerm) * num35;
            rxnRate[0x30] = (numArray2[0x30] * PCrackTerm) * num42;
            rxnRate[0x31] = (numArray2[0x31] * PCrackTerm) * num34;
            rxnRate[50] = (numArray2[50] * PCrackTerm) * num41;
            rxnRate[0x33] = (numArray2[0x33] * PCrackTerm) * num33;
            rxnRate[0x34] = (numArray2[0x34] * PCrackTerm) * num40;
            rxnRate[0x35] = (numArray2[0x35] * PCrackTerm) * num32;
            rxnRate[0x36] = (numArray2[0x36] * PCrackTerm) * num39;
            rxnRate[0x39] = (numArray2[0x39] * DealkTerm) * ((num36 * num31) - ((num27 * num16) / numArray[0x39]));
            rxnRate[0x3a] = (numArray2[0x3a] * DealkTerm) * ((num19 * num31) - ((num37 * num16) / numArray[0x3a]));
            rxnRate[0x3b] = (numArray2[0x3b] * DealkTerm) * ((num30 * num31) - ((num36 * num16) / numArray[0x3b]));
            rxnRate[60] = (numArray2[60] * DealkTerm) * ((num28 * num31) - ((num36 * num16) / numArray[60]));
            rxnRate[0x3d] = (numArray2[0x3d] * DealkTerm) * ((num44 * num31) - ((num19 * num16) / numArray[0x3d]));
            rxnRate[0x3e] = (numArray2[0x3e] * DealkTerm) * ((num21 * num31) - ((num19 * num16) / numArray[0x3e]));
            rxnRate[0x3f] = (numArray2[0x3f] * DealkTerm) * ((num24 * num31) - ((num30 * num16) / numArray[0x3f]));
            rxnRate[0x40] = (numArray2[0x40] * DealkTerm) * ((num24 * num31) - ((num28 * num16) / numArray[0x40]));
            rxnRate[0x41] = (numArray2[0x41] * DealkTerm) * ((num25 * num31) - ((num44 * num16) / numArray[0x41]));
            rxnRate[0x42] = (numArray2[0x42] * DealkTerm) * ((num25 * num31) - ((num21 * num16) / numArray[0x42]));
            rxnRate[0x43] = (numArray2[0x43] * DealkTerm) * ((num46 * num31) - ((num15 * num16) / numArray[0x43]));
            rxnRate[0x44] = (numArray2[0x44] * DealkTerm) * ((num20 * num31) - ((num46 * num16) / numArray[0x44]));
            rxnRate[0x45] = (numArray2[0x45] * DealkTerm) * ((num23 * num31) - ((num20 * num16) / numArray[0x45]));
            rxnRate[0x47] = (numArray2[0x47] * NCrackTerm) * num24;
            rxnRate[0x48] = (numArray2[0x48] * NCrackTerm) * num25;
            rxnRate[0x49] = (numArray2[0x49] * NCrackTerm) * num30;
            rxnRate[0x4a] = (numArray2[0x4a] * NCrackTerm) * num28;
            rxnRate[0x4b] = (numArray2[0x4b] * NCrackTerm) * num44;
            rxnRate[0x4c] = (numArray2[0x4c] * NCrackTerm) * num21;
            rxnRate[0x4d] = (numArray2[0x4d] * NCrackTerm) * num36;
            rxnRate[0x4e] = (numArray2[0x4e] * NCrackTerm) * num19;
            rxnRate[0x4f] = (numArray2[0x4f] * NCrackTerm) * num27;
            rxnRate[80] = (numArray2[80] * NCrackTerm) * num37;
            RateOfChange[1] = 3.0 * (((((((((rxnRate[1] + rxnRate[2]) + rxnRate[3]) + rxnRate[4]) + rxnRate[5]) + rxnRate[6]) + rxnRate[7]) + rxnRate[8]) + rxnRate[9]) + rxnRate[10]);
            RateOfChange[1] -= ((((((((rxnRate[30] + rxnRate[0x1f]) + rxnRate[0x20]) + rxnRate[0x21]) + rxnRate[0x22]) + rxnRate[0x23]) + rxnRate[0x24]) + rxnRate[0x25]) + rxnRate[0x26]) + rxnRate[0x27];
            RateOfChange[1] -= (((((((((((((rxnRate[40] + rxnRate[0x29]) + rxnRate[0x2a]) + rxnRate[0x2d]) + rxnRate[0x2e]) + rxnRate[0x2f]) + rxnRate[0x30]) + rxnRate[0x31]) + rxnRate[50]) + rxnRate[0x33]) + rxnRate[0x34]) + rxnRate[0x35]) + rxnRate[0x36]) + rxnRate[0x39]) + rxnRate[0x3a];
            RateOfChange[1] -= (((((((((rxnRate[0x3b] + rxnRate[60]) + rxnRate[0x3d]) + rxnRate[0x3e]) + rxnRate[0x3f]) + rxnRate[0x40]) + rxnRate[0x41]) + rxnRate[0x42]) + rxnRate[0x43]) + rxnRate[0x44]) + rxnRate[0x45];
            RateOfChange[1] -= 2.0 * (((((((((rxnRate[0x47] + rxnRate[0x48]) + rxnRate[0x49]) + rxnRate[0x4a]) + rxnRate[0x4b]) + rxnRate[0x4c]) + rxnRate[0x4d]) + rxnRate[0x4e]) + rxnRate[0x4f]) + rxnRate[80]);
            RateOfChange[2] = ((((((((((((rxnRate[0x39] + rxnRate[0x3a]) + rxnRate[0x3b]) + rxnRate[60]) + rxnRate[0x3d]) + rxnRate[0x3e]) + rxnRate[0x3f]) + rxnRate[0x40]) + rxnRate[0x41]) + rxnRate[0x42]) + rxnRate[0x43]) + rxnRate[0x44]) + rxnRate[0x45]) + CrackProd(1, rxnRate);
            RateOfChange[3] = CrackProd(2, rxnRate);
            RateOfChange[4] = CrackProd(3, rxnRate);
            RateOfChange[5] = (CrackProd(4, rxnRate) + rxnRate[0x1b]) - rxnRate[0x35];
            RateOfChange[6] = (CrackProd(5, rxnRate) - rxnRate[0x1b]) - rxnRate[0x36];
            RateOfChange[7] = (CrackProd(6, rxnRate) + rxnRate[0x1a]) - rxnRate[0x33];
            RateOfChange[8] = (CrackProd(7, rxnRate) - rxnRate[0x1a]) - rxnRate[0x34];
            RateOfChange[9] = ((CrackProd(8, rxnRate) + rxnRate[0x19]) + rxnRate[0x20]) - rxnRate[0x31];
            RateOfChange[10] = (((CrackProd(9, rxnRate) - rxnRate[0x19]) + rxnRate[30]) + rxnRate[0x1f]) - rxnRate[50];
            RateOfChange[11] = (((rxnRate[0x39] - rxnRate[30]) - rxnRate[0x11]) - rxnRate[1]) - rxnRate[0x4f];
            RateOfChange[12] = ((((rxnRate[0x3a] - rxnRate[0x1f]) - rxnRate[0x20]) + rxnRate[0x11]) - rxnRate[2]) - rxnRate[80];
            RateOfChange[13] = (rxnRate[1] + rxnRate[2]) + rxnRate[0x43];
            RateOfChange[14] = (((CrackProd(10, rxnRate) + rxnRate[0x18]) + rxnRate[0x22]) + rxnRate[0x24]) - rxnRate[0x2f];
            RateOfChange[15] = (((CrackProd(11, rxnRate) - rxnRate[0x18]) + rxnRate[0x21]) + rxnRate[0x23]) - rxnRate[0x30];
            RateOfChange[0x10] = ((((((rxnRate[0x3b] + rxnRate[60]) - rxnRate[0x39]) - rxnRate[0x21]) - rxnRate[0x22]) - rxnRate[0x12]) - rxnRate[3]) - rxnRate[0x4d];
            RateOfChange[0x11] = ((((((rxnRate[0x3d] + rxnRate[0x3e]) - rxnRate[0x3a]) - rxnRate[0x23]) - rxnRate[0x24]) + rxnRate[0x12]) - rxnRate[4]) - rxnRate[0x4e];
            RateOfChange[0x12] = ((rxnRate[0x44] - rxnRate[0x43]) + rxnRate[3]) + rxnRate[4];
            RateOfChange[0x13] = ((((CrackProd(12, rxnRate) + rxnRate[0x25]) + rxnRate[0x26]) + rxnRate[0x27]) + rxnRate[40]) - rxnRate[0x2e];
            RateOfChange[20] = (((((rxnRate[0x3f] - rxnRate[0x3b]) - rxnRate[0x25]) + rxnRate[0x16]) - rxnRate[0x13]) - rxnRate[5]) - rxnRate[0x49];
            RateOfChange[0x15] = (((((rxnRate[0x40] - rxnRate[60]) - rxnRate[0x26]) + rxnRate[0x15]) - rxnRate[0x16]) - rxnRate[6]) - rxnRate[0x4a];
            RateOfChange[0x16] = (((((rxnRate[0x41] - rxnRate[0x3d]) - rxnRate[0x27]) + rxnRate[0x13]) - rxnRate[20]) - rxnRate[7]) - rxnRate[0x4b];
            RateOfChange[0x17] = (((((rxnRate[0x42] - rxnRate[0x3e]) - rxnRate[40]) + rxnRate[20]) - rxnRate[0x15]) - rxnRate[8]) - rxnRate[0x4c];
            RateOfChange[0x18] = (rxnRate[5] + rxnRate[7]) + (0.096 * (rxnRate[0x45] - rxnRate[0x44]));
            RateOfChange[0x19] = ((rxnRate[13] - rxnRate[14]) + (0.232 * (rxnRate[6] + rxnRate[8]))) + (0.209 * (rxnRate[0x45] - rxnRate[0x44]));
            RateOfChange[0x1a] = ((rxnRate[14] - rxnRate[15]) + (0.395 * (rxnRate[6] + rxnRate[8]))) + (0.35 * (rxnRate[0x45] - rxnRate[0x44]));
            RateOfChange[0x1b] = ((rxnRate[15] - rxnRate[13]) + (0.373 * (rxnRate[6] + rxnRate[8]))) + (0.345 * (rxnRate[0x45] - rxnRate[0x44]));
            RateOfChange[0x1c] = (rxnRate[0x29] + rxnRate[0x2a]) - rxnRate[0x2d];
            RateOfChange[0x1d] = ((((-rxnRate[9] - rxnRate[0x17]) - rxnRate[0x29]) - rxnRate[0x3f]) - rxnRate[0x40]) - rxnRate[0x47];
            RateOfChange[30] = ((((-rxnRate[10] + rxnRate[0x17]) - rxnRate[0x2a]) - rxnRate[0x41]) - rxnRate[0x42]) - rxnRate[0x48];
            RateOfChange[0x1f] = (rxnRate[9] + rxnRate[10]) - rxnRate[0x45];
            index = 1;
            do
            {
                RateOfChange[index] *= NaphMolFeed;
                index++;
                num48 = 0x1f;
            }
            while (index <= num48);
            double num6 = TR * 0.01;
            double num7 = num6 * num6;
            double num8 = num6 * num7;
            double num9 = num6 * num8;
            double num10 = num6 - 5.36688;
            double num11 = (num7 / 2.0) - 14.4017;
            double num12 = (num8 / 3.0) - 51.52813;
            double num13 = (num9 / 4.0) - 207.40898;
            double num5 = 0.0;
            double num4 = 0.0;
            index = 1;
            do
            {
                num5 += RateOfChange[index] * (StdHeatForm[index] + (((((CpCoeft[index, 1] * num10) + (CpCoeft[index, 2] * num11)) + (CpCoeft[index, 3] * num12)) + (CpCoeft[index, 4] * num13)) * 100.0));
                num4 += FlowRate[index] * (((CpCoeft[index, 1] + (CpCoeft[index, 2] * num6)) + (CpCoeft[index, 3] * num7)) + (CpCoeft[index, 4] * num8));
                index++;
                num48 = 0x1f;
            }
            while (index <= num48);
            double num = num5 / num4;
            RateOfChange[0x20] = -num;
        }

        private void CharFeed()
        {
            P_LV = ((((((((((((((BpdFeed[1] + BpdFeed[2]) + BpdFeed[3]) + BpdFeed[4]) + BpdFeed[5]) + BpdFeed[6]) + BpdFeed[7]) + BpdFeed[8]) + BpdFeed[9]) + BpdFeed[10]) + BpdFeed[14]) + BpdFeed[15]) + BpdFeed[0x13]) + BpdFeed[0x1c]) / NaphBpdFeed) * 100.0;
            P_WT = ((((((((((((((LbsFeed[1] + LbsFeed[2]) + LbsFeed[3]) + LbsFeed[4]) + LbsFeed[5]) + LbsFeed[6]) + LbsFeed[7]) + LbsFeed[8]) + LbsFeed[9]) + LbsFeed[10]) + LbsFeed[14]) + LbsFeed[15]) + LbsFeed[0x13]) + LbsFeed[0x1c]) / NaphLbsFeed) * 100.0;
            N_LV = ((((((((((BpdFeed[11] + BpdFeed[12]) + BpdFeed[0x10]) + BpdFeed[0x11]) + BpdFeed[20]) + BpdFeed[0x15]) + BpdFeed[0x16]) + BpdFeed[0x17]) + BpdFeed[0x1d]) + BpdFeed[30]) / NaphBpdFeed) * 100.0;
            N_WT = ((((((((((LbsFeed[11] + LbsFeed[12]) + LbsFeed[0x10]) + LbsFeed[0x11]) + LbsFeed[20]) + LbsFeed[0x15]) + LbsFeed[0x16]) + LbsFeed[0x17]) + LbsFeed[0x1d]) + LbsFeed[30]) / NaphLbsFeed) * 100.0;
            A_LV = (((((((BpdFeed[13] + BpdFeed[0x12]) + BpdFeed[0x18]) + BpdFeed[0x19]) + BpdFeed[0x1a]) + BpdFeed[0x1b]) + BpdFeed[0x1f]) / NaphBpdFeed) * 100.0;
            A_WT = (((((((LbsFeed[13] + LbsFeed[0x12]) + LbsFeed[0x18]) + LbsFeed[0x19]) + LbsFeed[0x1a]) + LbsFeed[0x1b]) + LbsFeed[0x1f]) / NaphLbsFeed) * 100.0;
            SGFeed = NaphLbsFeed / (NaphBpdFeed * 14.574166);
            APIFeed = (141.5 / SGFeed) - 131.5;
        }

        private void CheckContinue(string Message)
        {
        }

        private void ClearOutput()
        {
            int num3;
            int num = 1;
            do
            {
                int num2 = 2;
                do
                {
                    num2++;
                    num3 = 10;
                }
                while (num2 <= num3);
                num++;
                num3 = 0x20;
            }
            while (num <= num3);
        }

        private void ConvD86ToTBP(double[] DistA, double[] DistB)
        {
            DistB[2] = (0.5277 * Math.Pow(DistA[2] + 459.67, 1.090011)) - 459.67;
            DistB[3] = (0.7429 * Math.Pow(DistA[3] + 459.67, 1.042533)) - 459.67;
            DistB[4] = (0.89203 * Math.Pow(DistA[4] + 459.67, 1.017565)) - 459.67;
            DistB[5] = (0.87051 * Math.Pow(DistA[5] + 459.67, 1.02259)) - 459.67;
            DistB[6] = (0.948976 * Math.Pow(DistA[6] + 459.67, 1.010955)) - 459.67;
            double x = DistA[2] - DistA[1];
            double num3 = (((((0.0 + (2.84582 * x)) - (0.0571924 * Math.Pow(x, 2.0))) + (0.00104465 * Math.Pow(x, 3.0))) - (1.01842E-05 * Math.Pow(x, 4.0))) + (4.9828E-08 * Math.Pow(x, 5.0))) - (9.53795E-11 * Math.Pow(x, 6.0));
            DistB[1] = DistB[2] - num3;
            double num2 = DistA[7] - DistA[6];
            double num4 = (((((0.0 + (1.20092 * num2)) + (0.00661377 * Math.Pow(num2, 2.0))) - (0.000650227 * Math.Pow(num2, 3.0))) + (1.63615E-05 * Math.Pow(num2, 4.0))) - (1.71884E-07 * Math.Pow(num2, 5.0))) + (6.8672E-10 * Math.Pow(num2, 6.0));
            DistB[7] = DistB[6] + num4;
        }

        private double CrackProd(int I, double[] RxnRate)
        {
            return ((((((((((((((((((((RxnRate[0x2d] * HCDistrib[1, I]) + (RxnRate[0x47] * HCDistrib[2, I])) + (RxnRate[0x48] * HCDistrib[3, I])) + (RxnRate[0x2e] * HCDistrib[4, I])) + (RxnRate[0x49] * HCDistrib[5, I])) + (RxnRate[0x4a] * HCDistrib[6, I])) + (RxnRate[0x4b] * HCDistrib[7, I])) + (RxnRate[0x4c] * HCDistrib[8, I])) + (RxnRate[0x2f] * HCDistrib[9, I])) + (RxnRate[0x30] * HCDistrib[10, I])) + (RxnRate[0x4d] * HCDistrib[11, I])) + (RxnRate[0x4e] * HCDistrib[12, I])) + (RxnRate[0x31] * HCDistrib[13, I])) + (RxnRate[50] * HCDistrib[14, I])) + (RxnRate[0x4f] * HCDistrib[15, I])) + (RxnRate[80] * HCDistrib[0x10, I])) + (RxnRate[0x33] * HCDistrib[0x11, I])) + (RxnRate[0x34] * HCDistrib[0x12, I])) + (RxnRate[0x35] * HCDistrib[0x13, I])) + (RxnRate[0x36] * HCDistrib[20, I]));
        }

        private double CToF(double value)
        {
            return ((value * 1.8) + 32.0);
        }

        private double CToK(double value)
        {
            return (value + 273.15);
        }

        private double CToR(double value)
        {
            return ((value + 273.15) * 1.8);
        }

        private void DisplayResults(double[] F_RecyStar, double[,] F_Inlet, double[] F_Eff, double[] F_Ref, double[] F_NetGas, double[] ReactorT_Out)
        {
            double InSCF;
            double InLbs;
            double InNM3;
            double SumSCF = 0;
            double[] numArray = new double[4];
            double[] numArray3 = new double[4];
            double[] f = new double[0x20];
            double[] RefBPD = new double[0x20];
            double[] RefLVFF = new double[0x20];
            double[] RefLbs = new double[0x20];
            double[] RefWTFF = new double[0x20];
            double[] GasLbs = new double[0x20];
            double NaphM3FeedRate = BblToM3(NaphBpdFeed);
            double NaphMTPDFeedRate = Math.Round(LbsToMT(NaphLbsFeed) * 24.0, 2);
            double SumMol = 0.0;
            double SumLBS = 0.0;
            double SumNM3 = 0.0;

            for (int I = 1; I <= NumComp; I++)
            {
                InLbs = F_RecyStar[I] * MW[I];
                InSCF = LbMolToSCF(F_RecyStar[I]) * 24.0 / NaphBpdFeed;
                InNM3 = KgMolToNM3(LbsToKg(F_RecyStar[I])) * 24.0 / NaphM3FeedRate;
                SumMol += F_RecyStar[I];
                SumLBS += InLbs;
                SumSCF += InSCF;
                SumNM3 += InNM3;
            }

            SumMol = Math.Round(SumMol, 2);
            SumLBS = Math.Round(SumLBS, 2);
            SumSCF = Math.Round(SumSCF, 2);
            SumNM3 = Math.Round(SumNM3, 2);

            for (int j = 1; j <= NumComp; j++)
            {
                SumMol = 0.0;
                SumLBS = 0.0;
                SumSCF = 0.0;

                for (int I = 1; I <= NumComp; I++)
                {
                    InLbs = F_Inlet[I, j] * MW[I];
                    InSCF = InLbs / (SG[I] * 14.574166);
                    SumMol += F_Inlet[I, j];
                    SumLBS += InLbs;
                    SumSCF += InSCF;
                    //SumNM3 += InNM3;
                }

                if (j == 1)
                {
                }
                else
                {
                    for (int i = 1; i <= NumComp; i++)
                    {
                        f[i] = F_Inlet[i, j];
                    }

                    Furnace(ReactorT_Out[j - 1], ReactorT_In[j], f, numArray[j - 1]);
                    if (((j == 2) || (j == 3)) || (j == 4))
                    {
                    }
                }
            }

            double WAIT = 0.0;
            double WAIP = 0.0;

            for (int I = 1; I <= NumReactor; I++)
            {
                WAIT += CatPercent[I] * ReactorT_In[I];
                WAIP += CatPercent[I] * InletP[I];
            }

            WAIT /= 100.0;
            WAIP /= 100.0;
            SumMol = 0.0;
            SumLBS = 0.0;
            SumSCF = 0.0;

            for (int i = 1; i <= NumComp; i++)
            {
                InLbs = F_Eff[i] * MW[i];
                InSCF = InLbs / (SG[i] * 14.574166);
                SumMol += F_Eff[i];
                SumLBS += InLbs;
                SumSCF += InSCF;
            }

            SumMol = 0.0;
            double SumRefLbs = 0.0;
            SumSCF = 0.0;

            for (int i = 1; i < NumComp; i++)
            {
                RefLbs[i] = F_Ref[i] * MW[i];
                RefWTFF[i] = (RefLbs[i] / NaphLbsFeed) * 100.0;
                RefBPD[i] = RefLbs[i] / (SG[i] * 14.574166);
                RefLVFF[i] = (RefBPD[i] / NaphBpdFeed) * 100.0;
                SumMol += F_Ref[i];
                SumRefLbs += RefLbs[i];
                SumSCF += RefBPD[i];
            }

            double num = 0.0;
            double num2 = 0.0;
            SumRON = 0.0;
            SumMON = 0.0;
            SumRVP = 0.0;

            for (int i = 7; i <= NumComp; i++)
            {
                num += RefBPD[i];
                num2 += RefLbs[i];
                SumRON += RON[i] * RefBPD[i];
                SumMON += MON[i] * RefBPD[i];
                SumRVP += RVP[i] * RefBPD[i];
            }

            SumRON /= num;
            SumMON /= num;
            SumRVP /= num;
            RefC5PlusLV = (num / NaphBpdFeed) * 100.0;
            double num18 = (num2 / NaphLbsFeed) * 100.0;
            SumRON = Math.Round(SumRON, 2);
            SumMON = Math.Round(SumMON, 2);
            RefC5PlusLV = Math.Round(RefC5PlusLV, 2);
            num18 = Math.Round(num18, 2);

            for (int i = 7; i <= NumComp; i++)
            {
                RefLVFF[i] = Math.Round(RefLVFF[i], 2);
                RefWTFF[i] = Math.Round(RefWTFF[i], 2);
            }

            double RefAromLV = RefLVFF[13] + RefLVFF[18] + RefLVFF[24] + RefLVFF[25] + RefLVFF[26] + RefLVFF[27] + RefLVFF[31];
            double RefAromWT = RefWTFF[13] + RefWTFF[18] + RefWTFF[24] + RefWTFF[25] + RefWTFF[26] + RefWTFF[27] + RefWTFF[31];
            SumMol = 0.0;
            SumLBS = 0.0;
            SumSCF = 0.0;
            SumNM3 = 0.0;

            for (int I = 1; I <= NumComp; I++)
            {
                GasLbs[I] = F_NetGas[I] * MW[I];
                InSCF = LbMolToSCF(F_NetGas[I]) * 24.0 / NaphBpdFeed;
                InNM3 = KgMolToNM3(LbsToKg(F_NetGas[I])) * 24.0 / NaphM3FeedRate;
                SumMol += F_NetGas[I];
                SumLBS += GasLbs[I];
                SumSCF += InSCF;
                SumNM3 += InNM3;
            }

            double Diff = NaphLbsFeed - SumLBS - SumRefLbs;
            if (Math.Abs(Diff) > 0.0001)
            {
                int J = 0;
                if (Diff > 0.001)
                {
                    J = NumComp;
                }
                else
                {
                    J = NumComp - 3;
                }

                for (int K = J; K >= 1; K--)
                {
                    if (RefLbs[K] > Math.Abs(Diff))
                    { break; }
                    RefLbs[K] += Diff;
                }

                SumRefLbs += Diff;
            }

            double SumKgMol = Math.Round(LbsToKg(SumMol), 2);
            SumMol = Math.Round(SumMol, 2);
            double SumKg = Math.Round(LbsToKg(SumLBS), 2);
            SumLBS = Math.Round(SumLBS, 2);
            SumSCF = Math.Round(SumSCF, 2);
            SumNM3 = Math.Round(SumNM3, 2);
            double H2ScfPerBBl = Math.Round(LbMolToSCF(F_NetGas[1]) * 24.0 / NaphBpdFeed, 2);
            double H2WtPerc = Math.Round(GasLbs[1] / NaphLbsFeed * 100.0, 2);
            double H2NM3PerM3 = KgMolToNM3(LbsToKg(F_NetGas[1])) * 24.0 / NaphM3FeedRate;
        }

        private void EndWell(string Message)
        {
            MessageBox.Show(Message, "Error");
        }

        private void EstRecycle(double[] F_Recy)
        {
            int num2;
            double[] numArray = new double[0x20];
            numArray[1] = 1.0;
            numArray[2] = 0.8;
            numArray[3] = 0.065;
            numArray[4] = 0.058;
            numArray[5] = 0.013;
            numArray[6] = 0.02;
            numArray[7] = 0.012;
            numArray[8] = 0.007;
            numArray[9] = 0.007;
            numArray[10] = 0.003;
            numArray[11] = 0.0;
            numArray[12] = 0.0;
            numArray[13] = 0.003;
            numArray[14] = 0.005;
            numArray[15] = 0.002;
            numArray[0x10] = 0.0;
            numArray[0x11] = 0.0;
            numArray[0x12] = 0.005;
            numArray[0x13] = 0.002;
            numArray[20] = 0.0;
            numArray[0x15] = 0.0;
            numArray[0x16] = 0.0;
            numArray[0x17] = 0.0;
            numArray[0x18] = 0.001;
            numArray[0x19] = 0.0;
            numArray[0x1a] = 0.001;
            numArray[0x1b] = 0.0;
            numArray[0x1c] = 0.001;
            numArray[0x1d] = 0.0;
            numArray[30] = 0.0;
            numArray[0x1f] = 0.001;
            int index = 1;
            do
            {
                F_Recy[index] = MolH2Recy * numArray[index];
                index++;
                num2 = 0x1f;
            }
            while (index <= num2);
        }

        private void Flash(double[] F, double TFlash, double PFlash, double FracLiq, double[] F_Liq, double[] F_Vap)
        {
            int num19;
            double[] eqK = new double[0x20];
            double[] numArray3 = new double[0x20];
            double[] x = new double[0x20];
            double[] y = new double[0x20];
            double[] numArray2 = new double[0x20];
            int num = 0;
            int num2 = 0;
            FracLiq = 0.5;
            double num15 = 0.0;
            int index = 1;
            do
            {
                num15 += F[index];
                index++;
                num19 = 0x1f;
            }
            while (index <= num19);
            index = 1;
            do
            {
                numArray2[index] = F[index] / num15;
                index++;
                num19 = 0x1f;
            }
            while (index <= num19);
            KIdeal(TFlash, PFlash, eqK);
            index = 1;
            do
            {
                x[index] = 0.0;
                y[index] = 0.0;
                index++;
                num19 = 0x1f;
            }
            while (index <= num19);
            int num11 = 100;
            bool flag2 = false;
            int num18 = num11;
            for (int i = 1; i <= num18; i++)
            {
                double num3;
                double num14;
                double num4 = 0.0;
                double num7 = 0.0;
                bool flag = false;
                index = 1;
                do
                {
                    num14 = 1.0 - eqK[index];
                    num3 = (FracLiq * num14) + eqK[index];
                    numArray3[index] = numArray2[index] / num3;
                    if (Math.Abs((double)(numArray3[index] - x[index])) > 1E-05)
                    {
                        flag = true;
                    }
                    double num5 = (numArray2[index] * num14) / num3;
                    num7 += num5;
                    num4 += (num5 * num14) / num3;
                    index++;
                    num19 = 0x1f;
                }
                while (index <= num19);
                if ((Math.Abs(num7) - 1E-06) > 0.0)
                {
                    double num6 = FracLiq + (num7 / num4);
                    if (num6 <= 0.0)
                    {
                        FracLiq = 0.5 * FracLiq;
                        num++;
                        if (num > 0x19)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    else if (num6 > 1.0)
                    {
                        FracLiq = 0.5 * (FracLiq + 1.0);
                        num2++;
                        if (num2 > 0x19)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    else
                    {
                        FracLiq = num6;
                    }
                }
                else
                {
                    double num12 = 0.0;
                    double num13 = 0.0;
                    index = 1;
                    do
                    {
                        num14 = 1.0 - eqK[index];
                        num3 = (FracLiq * num14) + eqK[index];
                        x[index] = numArray2[index] / num3;
                        y[index] = eqK[index] * x[index];
                        num12 += x[index];
                        num13 += y[index];
                        index++;
                        num19 = 0x1f;
                    }
                    while (index <= num19);
                    index = 1;
                    do
                    {
                        x[index] /= num12;
                        y[index] /= num13;
                        index++;
                        num19 = 0x1f;
                    }
                    while (index <= num19);
                    if (flag)
                    {
                        KReal(TFlash, PFlash, eqK, x, y);
                    }
                    else
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            if (!flag2)
            {
                string message = "Flash calculation failed. Simulation abandoned.";
                EndWell(message);
            }
            double num16 = num15 * FracLiq;
            double num17 = num15 - num16;
            index = 1;
            do
            {
                F_Liq[index] = num16 * x[index];
                F_Vap[index] = num17 * y[index];
                index++;
                num19 = 0x1f;
            }
            while (index <= num19);
        }

        private double FToC(double value)
        {
            return ((value - 32.0) / 1.8);
        }

        private double FToK(double value)
        {
            return ((value + 459.67) / 1.8);
        }

        private double FToR(double value)
        {
            return (value + 459.67);
        }

        private void Furnace(double T_In, double T_Out, double[] F, double Duty)
        {
            int num9;
            Duty = 0.0;
            double x = T_In * 0.01;
            double num4 = T_Out * 0.01;
            double num5 = num4 - x;
            double num6 = (Math.Pow(num4, 2.0) - Math.Pow(x, 2.0)) / 2.0;
            double num7 = (Math.Pow(num4, 3.0) - Math.Pow(x, 3.0)) / 3.0;
            double num8 = (Math.Pow(num4, 4.0) - Math.Pow(x, 4.0)) / 4.0;
            int index = 1;
            do
            {
                double num = (((CpCoeft[index, 1] * num5) + (CpCoeft[index, 2] * num6)) + (CpCoeft[index, 3] * num7)) + (CpCoeft[index, 4] * num8);
                Duty += (num * F[index]) * 100.0;
                index++;
                num9 = 0x1f;
            }
            while (index <= num9);
            Duty /= 1000000.0;
        }

        private double GmPerCm3ToLbsPerFt3(double value)
        {
            return ((value * 2.20462262) / 0.0353146667);
        }

        private void InitConstants()
        {
            double num = 1.0;
            double num2 = 1.05;
            double num3 = 1.09;
            double num8 = 1.0;
            double num9 = 1.3;
            double num10 = 1.05;
            double num4 = 1.0;
            double num5 = 1.0;
            double num6 = 1.25;
            double num7 = 1.56;
            double num11 = 1.9;
            double num12 = 1.8;
            double num13 = 1.5;
            double num14 = 1.4;
            A1[1] = 16.49944 / num11;
            A1[2] = 15.9689 / num11;
            A1[3] = 16.49944 / num12;
            A1[4] = 15.9689 / num12;
            A1[5] = 16.77974 / num13;
            A1[6] = 16.71084 / num13;
            A1[7] = 16.3744 / num13;
            A1[8] = 16.3172 / num13;
            A1[9] = 16.89464 / num14;
            A1[10] = 16.5 / num14;
            A1[11] = 0.0;
            A1[12] = 0.0;
            A1[13] = 12.908;
            A1[14] = 12.908;
            A1[15] = 12.908;
            A1[0x10] = 0.0;
            A1[0x11] = 8.2122;
            A1[0x12] = 9.5985;
            A1[0x13] = 10.158;
            A1[20] = 11.208;
            A1[0x15] = 10.158;
            A1[0x16] = 11.208;
            A1[0x17] = 10.5148;
            A1[0x18] = 2.9775;
            A1[0x19] = 5.119;
            A1[0x1a] = 3.6946;
            A1[0x1b] = 23.947;
            A1[0x1c] = 0.0;
            A1[0x1d] = 0.0;
            A1[30] = 40.6743 / num4;
            A1[0x1f] = 40.6097 / num4;
            A1[0x20] = 40.5407 / num4;
            A1[0x21] = 41.0806 / num5;
            A1[0x22] = 41.0806 / num5;
            A1[0x23] = 40.8983 / num5;
            A1[0x24] = 40.8983 / num5;
            A1[0x25] = 39.4746 / num6;
            A1[0x26] = 40.0531 / num6;
            A1[0x27] = 39.4746 / num6;
            A1[40] = 40.0531 / num6;
            A1[0x29] = 37.3642 / num7;
            A1[0x2a] = 35.9782 / num7;
            A1[0x2b] = 0.0;
            A1[0x2c] = 0.0;
            A1[0x2d] = 24.9 / num3;
            A1[0x2e] = 26.70394;
            A1[0x2f] = 27.7194 / num2;
            A1[0x30] = 27.1739 / num2;
            A1[0x31] = 27.88949 / num;
            A1[50] = 27.26899 / num;
            A1[0x33] = 30.34819;
            A1[0x34] = 26.76859;
            A1[0x35] = 23.63919;
            A1[0x36] = 25.88269;
            A1[0x37] = 0.0;
            A1[0x38] = 0.0;
            A1[0x39] = 12.69022;
            A1[0x3a] = 12.69022;
            A1[0x3b] = 13.24982;
            A1[60] = 13.24982;
            A1[0x3d] = 13.24982;
            A1[0x3e] = 13.24982;
            A1[0x3f] = 13.60372;
            A1[0x40] = 13.60372;
            A1[0x41] = 13.60372;
            A1[0x42] = 13.30372;
            A1[0x43] = 9.18362;
            A1[0x44] = 10.49602 / num9;
            A1[0x45] = 15.21074 / num10;
            A1[70] = 0.0;
            A1[0x47] = 18.1665;
            A1[0x48] = 16.8755;
            A1[0x49] = 17.1856;
            A1[0x4a] = 17.1856;
            A1[0x4b] = 15.8967;
            A1[0x4c] = 15.9867;
            A1[0x4d] = 16.087;
            A1[0x4e] = 14.7828;
            A1[0x4f] = 15.8639 / num8;
            A1[80] = 14.5545 / num8;
            A2[1] = -25056.0 / num11;
            A2[2] = -25056.0 / num11;
            A2[3] = -25056.0 / num12;
            A2[4] = -25056.0 / num12;
            A2[5] = -25056.0 / num13;
            A2[6] = -25056.0 / num13;
            A2[7] = -25056.0 / num13;
            A2[8] = -25056.0 / num13;
            A2[9] = -25000.0 / num14;
            A2[10] = -25000.0 / num14;
            A2[11] = 0.0;
            A2[12] = 0.0;
            A2[13] = -21000.0;
            A2[14] = -21000.0;
            A2[15] = -21000.0;
            A2[0x10] = 0.0;
            A2[0x11] = -21000.0;
            A2[0x12] = -21000.0;
            A2[0x13] = -21000.0;
            A2[20] = -21000.0;
            A2[0x15] = -21000.0;
            A2[0x16] = -21000.0;
            A2[0x17] = -21000.0;
            A2[0x18] = -12838.0;
            A2[0x19] = -16946.0;
            A2[0x1a] = -15494.0;
            A2[0x1b] = -46052.0;
            A2[0x1c] = 0.0;
            A2[0x1d] = 0.0;
            A2[30] = -60000.0 / num4;
            A2[0x1f] = -60000.0 / num4;
            A2[0x20] = -60000.0 / num4;
            A2[0x21] = -60000.0 / num5;
            A2[0x22] = -60000.0 / num5;
            A2[0x23] = -60000.0 / num5;
            A2[0x24] = -60000.0 / num5;
            A2[0x25] = -60000.0 / num6;
            A2[0x26] = -60000.0 / num6;
            A2[0x27] = -60000.0 / num6;
            A2[40] = -60000.0 / num6;
            A2[0x29] = -58200.0 / num7;
            A2[0x2a] = -58200.0 / num7;
            A2[0x2b] = 0.0;
            A2[0x2c] = 0.0;
            A2[0x2d] = -41648.7 / num3;
            A2[0x2e] = -46052.0;
            A2[0x2f] = -47134.0 / num2;
            A2[0x30] = -46052.0 / num2;
            A2[0x31] = -48408.0 / num;
            A2[50] = -47400.0 / num;
            A2[0x33] = -51948.0;
            A2[0x34] = -46836.0;
            A2[0x35] = -46052.0;
            A2[0x36] = -47400.0;
            A2[0x37] = 0.0;
            A2[0x38] = 0.0;
            A2[0x39] = -32000.0;
            A2[0x3a] = -32000.0;
            A2[0x3b] = -32000.0;
            A2[60] = -32000.0;
            A2[0x3d] = -32000.0;
            A2[0x3e] = -32000.0;
            A2[0x3f] = -32000.0;
            A2[0x40] = -32000.0;
            A2[0x41] = -32000.0;
            A2[0x42] = -32000.0;
            A2[0x43] = -32000.0;
            A2[0x44] = -32000.0 / num9;
            A2[0x45] = -32000.0 / num10;
            A2[70] = 0.0;
            A2[0x47] = -31374.0;
            A2[0x48] = -31374.0;
            A2[0x49] = -31374.0;
            A2[0x4a] = -31374.0;
            A2[0x4b] = -31374.0;
            A2[0x4c] = -31374.0;
            A2[0x4d] = -31374.0;
            A2[0x4e] = -31374.0;
            A2[0x4f] = -31374.0 / num8;
            A2[80] = -31374.0 / num8;
            B1[1] = 47.71902;
            B1[2] = 0.0;
            B1[3] = 47.45232;
            B1[4] = 43.0002;
            B1[5] = 47.74953;
            B1[6] = 47.48208;
            B1[7] = 44.22683;
            B1[8] = 47.7315;
            B1[9] = 47.90527;
            B1[10] = 44.0558;
            B1[11] = 0.0;
            B1[12] = 0.0;
            B1[13] = -0.54851;
            B1[14] = 0.79004;
            B1[15] = -0.24153;
            B1[0x10] = 0.0;
            B1[0x11] = 4.88114;
            B1[0x12] = 4.45838;
            B1[0x13] = 3.52239;
            B1[20] = 0.29286;
            B1[0x15] = -3.43419;
            B1[0x16] = -0.38091;
            B1[0x17] = 3.84955;
            B1[0x18] = 0.39547;
            B1[0x19] = -0.16411;
            B1[0x1a] = -0.60385;
            B1[0x1b] = -1.81158;
            B1[0x1c] = 0.0;
            B1[0x1d] = 0.0;
            B1[30] = -4.98164;
            B1[0x1f] = -9.8627;
            B1[0x20] = -9.94232;
            B1[0x21] = -6.34038;
            B1[0x22] = -6.10107;
            B1[0x23] = -10.35908;
            B1[0x24] = -10.39403;
            B1[0x25] = -5.55248;
            B1[0x26] = -5.67535;
            B1[0x27] = -9.43751;
            B1[40] = -8.94501;
            B1[0x29] = -5.40945;
            B1[0x2a] = -9.25892;
            B1[0x2b] = 0.0;
            B1[0x2c] = 0.0;
            B1[0x2d] = 0.0453;
            B1[0x2e] = 0.0;
            B1[0x2f] = 0.0;
            B1[0x30] = 0.0;
            B1[0x31] = 0.0;
            B1[50] = 0.0;
            B1[0x33] = 0.0;
            B1[0x34] = 0.0;
            B1[0x35] = 0.0;
            B1[0x36] = 0.0;
            B1[0x37] = 0.0;
            B1[0x38] = 0.0;
            B1[0x39] = -1.25772;
            B1[0x3a] = -0.83495;
            B1[0x3b] = 0.05072;
            B1[60] = -0.33178;
            B1[0x3d] = 0.9867;
            B1[0x3e] = 0.6948;
            B1[0x3f] = -0.60358;
            B1[0x40] = -0.22108;
            B1[0x41] = -0.93067;
            B1[0x42] = -0.63876;
            B1[0x43] = -0.99102;
            B1[0x44] = -0.66898;
            B1[0x45] = -0.33683;
            B1[70] = 0.0;
            B1[0x47] = 0.0;
            B1[0x48] = 0.0;
            B1[0x49] = 0.0;
            B1[0x4a] = 0.0;
            B1[0x4b] = 0.0;
            B1[0x4c] = 0.0;
            B1[0x4d] = 0.0;
            B1[0x4e] = 0.0;
            B1[0x4f] = 0.0;
            B1[80] = 0.0;
            B2[1] = -47771.8;
            B2[2] = 0.0;
            B2[3] = -46661.3;
            B2[4] = -42310.4;
            B2[5] = -46052.7;
            B2[6] = -45109.7;
            B2[7] = -41677.8;
            B2[8] = -46229.4;
            B2[9] = -44667.7;
            B2[10] = -40791.1;
            B2[11] = 0.0;
            B2[12] = 0.0;
            B2[13] = 640.0;
            B2[14] = 11.8;
            B2[15] = -652.2;
            B2[0x10] = 0.0;
            B2[0x11] = -3378.5;
            B2[0x12] = -4359.9;
            B2[0x13] = -4374.8;
            B2[20] = 2787.3;
            B2[0x15] = 3521.6;
            B2[0x16] = -1934.3;
            B2[0x17] = -3876.7;
            B2[0x18] = 1800.2;
            B2[0x19] = 1676.1;
            B2[0x1a] = 1675.7;
            B2[0x1b] = 1784.4;
            B2[0x1c] = 0.0;
            B2[0x1d] = 0.0;
            B2[30] = 9782.3;
            B2[0x1f] = 13160.7;
            B2[0x20] = 14221.0;
            B2[0x21] = 7961.7;
            B2[0x22] = 9122.6;
            B2[0x23] = 12600.2;
            B2[0x24] = 14089.6;
            B2[0x25] = 9019.9;
            B2[0x26] = 7714.8;
            B2[0x27] = 13366.9;
            B2[40] = 11218.0;
            B2[0x29] = 6478.5;
            B2[0x2a] = 10355.1;
            B2[0x2b] = 0.0;
            B2[0x2c] = 0.0;
            B2[0x2d] = 11873.5;
            B2[0x2e] = 0.0;
            B2[0x2f] = 0.0;
            B2[0x30] = 0.0;
            B2[0x31] = 0.0;
            B2[50] = 0.0;
            B2[0x33] = 0.0;
            B2[0x34] = 0.0;
            B2[0x35] = 0.0;
            B2[0x36] = 0.0;
            B2[0x37] = 0.0;
            B2[0x38] = 0.0;
            B2[0x39] = 11548.5;
            B2[0x3a] = 12530.3;
            B2[0x3b] = 14202.4;
            B2[60] = 12270.4;
            B2[0x3d] = 14216.9;
            B2[0x3e] = 11428.0;
            B2[0x3f] = 10446.2;
            B2[0x40] = 12378.2;
            B2[0x41] = 9948.0;
            B2[0x42] = 12736.9;
            B2[0x43] = 10438.0;
            B2[0x44] = 10996.0;
            B2[0x45] = 11659.0;
            B2[70] = 0.0;
            B2[0x47] = 0.0;
            B2[0x48] = 0.0;
            B2[0x49] = 0.0;
            B2[0x4a] = 0.0;
            B2[0x4b] = 0.0;
            B2[0x4c] = 0.0;
            B2[0x4d] = 0.0;
            B2[0x4e] = 0.0;
            B2[0x4f] = 0.0;
            B2[80] = 0.0;
            CpCoeft[1, 1] = 6.3539;
            CpCoeft[2, 1] = 6.367;
            CpCoeft[3, 1] = 2.3112;
            CpCoeft[4, 1] = -1.0719;
            CpCoeft[5, 1] = -2.4505;
            CpCoeft[6, 1] = -0.1302;
            CpCoeft[7, 1] = -2.28095;
            CpCoeft[8, 1] = -0.4145;
            CpCoeft[9, 1] = -5.9217;
            CpCoeft[10, 1] = -0.7017;
            CpCoeft[11, 1] = -11.9067;
            CpCoeft[12, 1] = -11.2874;
            CpCoeft[13, 1] = -10.52;
            CpCoeft[14, 1] = -0.955;
            CpCoeft[15, 1] = -0.995;
            CpCoeft[0x10, 1] = -12.8626;
            CpCoeft[0x11, 1] = -13.043;
            CpCoeft[0x12, 1] = -9.1226;
            CpCoeft[0x13, 1] = -1.2219;
            CpCoeft[20, 1] = -13.5338;
            CpCoeft[0x15, 1] = -13.3602;
            CpCoeft[0x16, 1] = -15.94786;
            CpCoeft[0x17, 1] = -15.94786;
            CpCoeft[0x18, 1] = -10.2083;
            CpCoeft[0x19, 1] = -5.38286;
            CpCoeft[0x1a, 1] = -6.5457;
            CpCoeft[0x1b, 1] = -3.38786;
            CpCoeft[0x1c, 1] = -1.40476;
            CpCoeft[0x1d, 1] = -13.8138;
            CpCoeft[30, 1] = -16.0852;
            CpCoeft[0x1f, 1] = -9.1543;
            CpCoeft[1, 2] = 0.167945;
            CpCoeft[2, 2] = 0.102427;
            CpCoeft[3, 2] = 2.01391;
            CpCoeft[4, 2] = 4.03765;
            CpCoeft[5, 2] = 5.6504;
            CpCoeft[6, 2] = 5.01947;
            CpCoeft[7, 2] = 6.692725;
            CpCoeft[8, 2] = 6.28296;
            CpCoeft[9, 2] = 9.1914463;
            CpCoeft[10, 2] = 7.56254387;
            CpCoeft[11, 2] = 7.71600485;
            CpCoeft[12, 2] = 8.19076347;
            CpCoeft[13, 2] = 6.9534831;
            CpCoeft[14, 2] = 8.828329;
            CpCoeft[15, 2] = 8.828329;
            CpCoeft[0x10, 2] = 9.70844364;
            CpCoeft[0x11, 2] = 9.98848057;
            CpCoeft[0x12, 2] = 7.66801119;
            CpCoeft[0x13, 2] = 10.096693;
            CpCoeft[20, 2] = 11.1712523;
            CpCoeft[0x15, 2] = 10.9259481;
            CpCoeft[0x16, 2] = 12.1267424;
            CpCoeft[0x17, 2] = 12.1267424;
            CpCoeft[0x18, 2] = 9.336;
            CpCoeft[0x19, 2] = 7.781393;
            CpCoeft[0x1a, 2] = 8.188448;
            CpCoeft[0x1b, 2] = 7.75355;
            CpCoeft[0x1c, 2] = 11.34312;
            CpCoeft[0x1d, 2] = 12.6579;
            CpCoeft[30, 2] = 13.35798;
            CpCoeft[0x1f, 2] = 10.2822752;
            CpCoeft[1, 3] = -0.014925;
            CpCoeft[2, 3] = 0.067468;
            CpCoeft[3, 3] = -0.015322;
            CpCoeft[4, 3] = -0.11203;
            CpCoeft[5, 3] = -0.177432;
            CpCoeft[6, 3] = -0.128858;
            CpCoeft[7, 3] = -0.194371;
            CpCoeft[8, 3] = -0.167989;
            CpCoeft[9, 3] = -0.357933;
            CpCoeft[10, 3] = -0.209619;
            CpCoeft[11, 3] = -0.139477;
            CpCoeft[12, 3] = -0.23556;
            CpCoeft[13, 3] = -0.277153;
            CpCoeft[14, 3] = -0.249743;
            CpCoeft[15, 3] = -0.249743;
            CpCoeft[0x10, 3] = -0.251213;
            CpCoeft[0x11, 3] = -0.316836;
            CpCoeft[0x12, 3] = -0.272413;
            CpCoeft[0x13, 3] = -0.289976;
            CpCoeft[20, 3] = -0.30787;
            CpCoeft[0x15, 3] = -0.27381;
            CpCoeft[0x16, 3] = -0.457856;
            CpCoeft[0x17, 3] = -0.457856;
            CpCoeft[0x18, 3] = -0.348471;
            CpCoeft[0x19, 3] = -0.22259;
            CpCoeft[0x1a, 3] = -0.257013;
            CpCoeft[0x1b, 3] = -0.236148;
            CpCoeft[0x1c, 3] = -0.3284465;
            CpCoeft[0x1d, 3] = -0.370591;
            CpCoeft[30, 3] = -0.494782;
            CpCoeft[0x1f, 3] = -0.361993;
            CpCoeft[1, 4] = 0.00046677;
            CpCoeft[2, 4] = -0.00224337;
            CpCoeft[3, 4] = -0.00076208;
            CpCoeft[4, 4] = 0.0010955;
            CpCoeft[5, 4] = 0.00214335;
            CpCoeft[6, 4] = 0.0009526;
            CpCoeft[7, 4] = 0.00200046;
            CpCoeft[8, 4] = 0.00138127;
            CpCoeft[9, 4] = 0.00612045;
            CpCoeft[10, 4] = 0.0019052;
            CpCoeft[11, 4] = -0.00114312;
            CpCoeft[12, 4] = 0.00209572;
            CpCoeft[13, 4] = 0.00433432;
            CpCoeft[14, 4] = 0.0023815;
            CpCoeft[15, 4] = 0.0023815;
            CpCoeft[0x10, 4] = 0.00133364;
            CpCoeft[0x11, 4] = 0.00359606;
            CpCoeft[0x12, 4] = 0.0036675;
            CpCoeft[0x13, 4] = 0.0028578;
            CpCoeft[20, 4] = 0.00223861;
            CpCoeft[0x15, 4] = 0.00109549;
            CpCoeft[0x16, 4] = 0.00752553;
            CpCoeft[0x17, 4] = 0.00752553;
            CpCoeft[0x18, 4] = 0.0050964;
            CpCoeft[0x19, 4] = 0.0019052;
            CpCoeft[0x1a, 4] = 0.00281017;
            CpCoeft[0x1b, 4] = 0.00247676;
            CpCoeft[0x1c, 4] = 0.00328647;
            CpCoeft[0x1d, 4] = 0.00323884;
            CpCoeft[30, 4] = 0.00790657;
            CpCoeft[0x1f, 4] = 0.00485825;
            StdHeatForm[1] = 0.0;
            StdHeatForm[2] = -32200.2;
            StdHeatForm[3] = -36424.8;
            StdHeatForm[4] = -44676.0;
            StdHeatForm[5] = -57870.0;
            StdHeatForm[6] = -54270.0;
            StdHeatForm[7] = -66456.0;
            StdHeatForm[8] = -63000.0;
            StdHeatForm[9] = -75778.2;
            StdHeatForm[10] = -71928.0;
            StdHeatForm[11] = -52974.0;
            StdHeatForm[12] = -45900.0;
            StdHeatForm[13] = 35676.0;
            StdHeatForm[14] = -84576.6;
            StdHeatForm[15] = -80802.0;
            StdHeatForm[0x10] = -66582.0;
            StdHeatForm[0x11] = -57498.3;
            StdHeatForm[0x12] = 21510.0;
            StdHeatForm[0x13] = -92296.8;
            StdHeatForm[20] = -73890.0;
            StdHeatForm[0x15] = -77823.0;
            StdHeatForm[0x16] = -63702.0;
            StdHeatForm[0x17] = -68916.6;
            StdHeatForm[0x18] = 12816.0;
            StdHeatForm[0x19] = 7722.0;
            StdHeatForm[0x1a] = 7416.0;
            StdHeatForm[0x1b] = 8172.0;
            StdHeatForm[0x1c] = -101057.4;
            StdHeatForm[0x1d] = -88524.0;
            StdHeatForm[30] = -79380.0;
            StdHeatForm[0x1f] = -3681.0;
            MW[1] = 2.016;
            MW[2] = 16.043;
            MW[3] = 30.07;
            MW[4] = 44.097;
            MW[5] = 58.124;
            MW[6] = 58.124;
            MW[7] = 72.151;
            MW[8] = 72.151;
            MW[9] = 86.178;
            MW[10] = 86.178;
            MW[11] = 84.162;
            MW[12] = 84.162;
            MW[13] = 78.114;
            MW[14] = 100.205;
            MW[15] = 100.205;
            MW[0x10] = 98.189;
            MW[0x11] = 98.189;
            MW[0x12] = 92.141;
            MW[0x13] = 114.232;
            MW[20] = 112.216;
            MW[0x15] = 112.216;
            MW[0x16] = 112.216;
            MW[0x17] = 112.216;
            MW[0x18] = 106.168;
            MW[0x19] = 106.168;
            MW[0x1a] = 106.168;
            MW[0x1b] = 106.168;
            MW[0x1c] = 128.259;
            MW[0x1d] = 126.243;
            MW[30] = 126.243;
            MW[0x1f] = 120.195;
            MW[0x20] = 128.259;
            MW[0x21] = 126.243;
            MW[0x22] = 120.195;
            MW[0x23] = 142.286;
            MW[0x24] = 140.27;
            MW[0x25] = 134.222;
            MW[0x26] = 156.312;
            MW[0x27] = 154.296;
            MW[40] = 148.248;
            MW[0x29] = 170.338;
            MW[0x2a] = 168.322;
            SG[1] = 0.135;
            SG[2] = 0.3;
            SG[3] = 0.3564;
            SG[4] = 0.5077;
            SG[5] = 0.5631;
            SG[6] = 0.5844;
            SG[7] = 0.6247;
            SG[8] = 0.631;
            SG[9] = 0.6609;
            SG[10] = 0.664;
            SG[11] = 0.7834;
            SG[12] = 0.7536;
            SG[13] = 0.8844;
            SG[14] = 0.6906;
            SG[15] = 0.6882;
            SG[0x10] = 0.774;
            SG[0x11] = 0.7595;
            SG[0x12] = 0.8718;
            SG[0x13] = 0.7089;
            SG[20] = 0.7922;
            SG[0x15] = 0.779;
            SG[0x16] = 0.7807;
            SG[0x17] = 0.7713;
            SG[0x18] = 0.8718;
            SG[0x19] = 0.8657;
            SG[0x1a] = 0.8687;
            SG[0x1b] = 0.8848;
            SG[0x1c] = 0.7272;
            SG[0x1d] = 0.79;
            SG[30] = 0.7825;
            SG[0x1f] = 0.8769;
            SG[0x20] = 0.7272;
            SG[0x21] = 0.7863;
            SG[0x22] = 0.8769;
            SG[0x23] = 0.7408;
            SG[0x24] = 0.7946;
            SG[0x25] = 0.8837;
            SG[0x26] = 0.7511;
            SG[0x27] = 0.8014;
            SG[40] = 0.8919;
            SG[0x29] = 0.76;
            SG[0x2a] = 0.8074;
            NBP[1] = 36.77;
            NBP[2] = 200.97;
            NBP[3] = 332.17;
            NBP[4] = 415.97;
            NBP[5] = 470.57;
            NBP[6] = 490.77;
            NBP[7] = 541.77;
            NBP[8] = 556.57;
            NBP[9] = 597.47;
            NBP[10] = 615.37;
            NBP[11] = 636.97;
            NBP[12] = 620.97;
            NBP[13] = 635.87;
            NBP[14] = 652.17;
            NBP[15] = 668.87;
            NBP[0x10] = 673.37;
            NBP[0x11] = 661.27;
            NBP[0x12] = 690.77;
            NBP[0x13] = 702.07;
            NBP[20] = 728.87;
            NBP[0x15] = 711.57;
            NBP[0x16] = 727.37;
            NBP[0x17] = 703.37;
            NBP[0x18] = 736.77;
            NBP[0x19] = 740.67;
            NBP[0x1a] = 742.07;
            NBP[0x1b] = 751.57;
            NBP[0x1c] = 749.67;
            NBP[0x1d] = 759.67;
            NBP[30] = 744.67;
            NBP[0x1f] = 790.67;
            TCrit[1] = 59.742;
            TCrit[2] = 343.08;
            TCrit[3] = 549.54;
            TCrit[4] = 665.64;
            TCrit[5] = 734.58;
            TCrit[6] = 765.18;
            TCrit[7] = 829.8;
            TCrit[8] = 845.46;
            TCrit[9] = 900.9;
            TCrit[10] = 913.68;
            TCrit[11] = 996.48;
            TCrit[12] = 959.04;
            TCrit[13] = 1011.96;
            TCrit[14] = 956.0;
            TCrit[15] = 972.36;
            TCrit[0x10] = 1029.96;
            TCrit[0x11] = 990.3;
            TCrit[0x12] = 1065.24;
            TCrit[0x13] = 1024.9;
            TCrit[20] = 1054.1;
            TCrit[0x15] = 1054.1;
            TCrit[0x16] = 1044.9;
            TCrit[0x17] = 1044.9;
            TCrit[0x18] = 1115.8;
            TCrit[0x19] = 1113.1;
            TCrit[0x1a] = 1114.9;
            TCrit[0x1b] = 1138.3;
            TCrit[0x1c] = 1055.8;
            TCrit[0x1d] = 1118.8;
            TCrit[30] = 1108.8;
            TCrit[0x1f] = 1146.7;
            PCrit[1] = 190.8;
            PCrit[2] = 673.1;
            PCrit[3] = 709.8;
            PCrit[4] = 617.4;
            PCrit[5] = 529.1;
            PCrit[6] = 550.7;
            PCrit[7] = 483.0;
            PCrit[8] = 489.5;
            PCrit[9] = 449.5;
            PCrit[10] = 440.0;
            PCrit[11] = 561.0;
            PCrit[12] = 549.0;
            PCrit[13] = 714.0;
            PCrit[14] = 414.2;
            PCrit[15] = 396.8;
            PCrit[0x10] = 504.4;
            PCrit[0x11] = 497.8;
            PCrit[0x12] = 590.0;
            PCrit[0x13] = 362.1;
            PCrit[20] = 468.1;
            PCrit[0x15] = 468.1;
            PCrit[0x16] = 448.4;
            PCrit[0x17] = 448.4;
            PCrit[0x18] = 540.0;
            PCrit[0x19] = 500.0;
            PCrit[0x1a] = 510.0;
            PCrit[0x1b] = 530.0;
            PCrit[0x1c] = 347.0;
            PCrit[0x1d] = 418.1;
            PCrit[30] = 412.1;
            PCrit[0x1f] = 492.2;
            W[1] = 0.0;
            W[2] = 0.0;
            W[3] = 0.1064;
            W[4] = 0.1538;
            W[5] = 0.1825;
            W[6] = 0.1953;
            W[7] = 0.2104;
            W[8] = 0.2387;
            W[9] = 0.2861;
            W[10] = 0.2972;
            W[11] = 0.2032;
            W[12] = 0.2346;
            W[13] = 0.213;
            W[14] = 0.3427;
            W[15] = 0.3403;
            W[0x10] = 0.2421;
            W[0x11] = 0.3074;
            W[0x12] = 0.2591;
            W[0x13] = 0.3992;
            W[20] = 0.3558;
            W[0x15] = 0.3558;
            W[0x16] = 0.364;
            W[0x17] = 0.364;
            W[0x18] = 0.2936;
            W[0x19] = 0.2969;
            W[0x1a] = 0.3045;
            W[0x1b] = 0.2904;
            W[0x1c] = 0.4406;
            W[0x1d] = 0.4124;
            W[30] = 0.4164;
            W[0x1f] = 0.3799;
            MolVol[1] = 31.0;
            MolVol[2] = 52.0;
            MolVol[3] = 68.0;
            MolVol[4] = 84.0;
            MolVol[5] = 105.5;
            MolVol[6] = 101.4;
            MolVol[7] = 117.4;
            MolVol[8] = 116.1;
            MolVol[9] = 131.449;
            MolVol[10] = 131.6;
            MolVol[11] = 108.7;
            MolVol[12] = 113.1;
            MolVol[13] = 89.4;
            MolVol[14] = 147.097;
            MolVol[15] = 147.5;
            MolVol[0x10] = 128.3;
            MolVol[0x11] = 129.71;
            MolVol[0x12] = 106.8;
            MolVol[0x13] = 163.5;
            MolVol[20] = 143.532;
            MolVol[0x15] = 143.532;
            MolVol[0x16] = 146.916;
            MolVol[0x17] = 146.916;
            MolVol[0x18] = 123.1;
            MolVol[0x19] = 124.0;
            MolVol[0x1a] = 123.5;
            MolVol[0x1b] = 121.2;
            MolVol[0x1c] = 180.094;
            MolVol[0x1d] = 158.936;
            MolVol[30] = 160.936;
            MolVol[0x1f] = 140.22;
            Sol[1] = 3.25;
            Sol[2] = 5.68;
            Sol[3] = 6.05;
            Sol[4] = 6.4;
            Sol[5] = 6.73;
            Sol[6] = 6.73;
            Sol[7] = 7.021;
            Sol[8] = 7.021;
            Sol[9] = 7.1561;
            Sol[10] = 7.266;
            Sol[11] = 8.196;
            Sol[12] = 7.849;
            Sol[13] = 9.158;
            Sol[14] = 7.2872;
            Sol[15] = 7.43;
            Sol[0x10] = 7.826;
            Sol[0x11] = 7.7851;
            Sol[0x12] = 8.915;
            Sol[0x13] = 7.551;
            Sol[20] = 7.9251;
            Sol[0x15] = 7.9251;
            Sol[0x16] = 7.826;
            Sol[0x17] = 7.826;
            Sol[0x18] = 8.787;
            Sol[0x19] = 8.769;
            Sol[0x1a] = 8.818;
            Sol[0x1b] = 8.987;
            Sol[0x1c] = 7.433;
            Sol[0x1d] = 8.3351;
            Sol[30] = 7.8351;
            Sol[0x1f] = 8.5676;
            RON[1] = 0.0;
            RON[2] = 0.0;
            RON[3] = 0.0;
            RON[4] = 0.0;
            RON[5] = 100.2;
            RON[6] = 95.0;
            RON[7] = 92.704;
            RON[8] = 61.7;
            RON[9] = 82.742;
            RON[10] = 31.0;
            RON[11] = 100.0;
            RON[12] = 96.0;
            RON[13] = 120.0;
            RON[14] = 66.879;
            RON[15] = 0.0;
            RON[0x10] = 73.8;
            RON[0x11] = 78.994;
            RON[0x12] = 110.0;
            RON[0x13] = 58.0;
            RON[20] = 46.5;
            RON[0x15] = 72.547;
            RON[0x16] = 31.2;
            RON[0x17] = 65.0;
            RON[0x18] = 124.0;
            RON[0x19] = 146.0;
            RON[0x1a] = 145.0;
            RON[0x1b] = 120.0;
            RON[0x1c] = 56.667;
            RON[0x1d] = 43.77;
            RON[30] = 0.0;
            RON[0x1f] = 110.7;
            MON[1] = 0.0;
            MON[2] = 0.0;
            MON[3] = 0.0;
            MON[4] = 0.0;
            MON[5] = 97.6;
            MON[6] = 93.5;
            MON[7] = 88.575;
            MON[8] = 61.3;
            MON[9] = 83.314;
            MON[10] = 30.0;
            MON[11] = 77.2;
            MON[12] = 85.0;
            MON[13] = 114.8;
            MON[14] = 69.675;
            MON[15] = 0.0;
            MON[0x10] = 71.1;
            MON[0x11] = 72.923;
            MON[0x12] = 102.0;
            MON[0x13] = 61.0;
            MON[20] = 40.8;
            MON[0x15] = 70.087;
            MON[0x16] = 28.1;
            MON[0x17] = 63.0;
            MON[0x18] = 107.0;
            MON[0x19] = 127.0;
            MON[0x1a] = 124.0;
            MON[0x1b] = 103.0;
            MON[0x1c] = 64.686;
            MON[0x1d] = 41.641;
            MON[30] = 0.0;
            MON[0x1f] = 101.75;
            RVP[1] = 0.0;
            RVP[2] = 0.0;
            RVP[3] = 0.0;
            RVP[4] = 90.0;
            RVP[5] = 72.2;
            RVP[6] = 51.6;
            RVP[7] = 20.4;
            RVP[8] = 15.6;
            RVP[9] = 7.0;
            RVP[10] = 5.0;
            RVP[11] = 2.3;
            RVP[12] = 4.5;
            RVP[13] = 3.2;
            RVP[14] = 2.3;
            RVP[15] = 1.6;
            RVP[0x10] = 1.6;
            RVP[0x11] = 2.0;
            RVP[0x12] = 1.0;
            RVP[0x13] = 0.7;
            RVP[20] = 0.5;
            RVP[0x15] = 0.7;
            RVP[0x16] = 0.7;
            RVP[0x17] = 0.7;
            RVP[0x18] = 0.4;
            RVP[0x19] = 0.3;
            RVP[0x1a] = 0.3;
            RVP[0x1b] = 0.3;
            RVP[0x1c] = 0.3;
            RVP[0x1d] = 0.2;
            RVP[30] = 0.3;
            RVP[0x1f] = 0.1;
            C9plusFact[1] = (MW[0x1c] + MW[1]) / 130.266;
            C9plusFact[2] = (((MW[0x1d] + MW[1]) - MW[20]) - MW[2]) / MW[3];
            C9plusFact[3] = (((MW[30] + MW[1]) - MW[20]) - MW[2]) / MW[3];
            C9plusFact[4] = (((MW[0x1f] + MW[1]) - MW[0x18]) - MW[2]) / MW[3];
            C9plusFact[5] = (MW[0x1d] + (2.0 * MW[1])) / 130.266;
            C9plusFact[6] = (MW[30] + (2.0 * MW[1])) / 130.266;
            HCDistrib[1, 1] = 0.176;
            HCDistrib[2, 1] = 0.176;
            HCDistrib[3, 1] = 0.176;
            HCDistrib[4, 1] = 0.199;
            HCDistrib[5, 1] = 0.199;
            HCDistrib[6, 1] = 0.199;
            HCDistrib[7, 1] = 0.199;
            HCDistrib[8, 1] = 0.199;
            HCDistrib[9, 1] = 0.237;
            HCDistrib[10, 1] = 0.237;
            HCDistrib[11, 1] = 0.237;
            HCDistrib[12, 1] = 0.237;
            HCDistrib[13, 1] = 0.288;
            HCDistrib[14, 1] = 0.288;
            HCDistrib[15, 1] = 0.288;
            HCDistrib[0x10, 1] = 0.288;
            HCDistrib[0x11, 1] = 0.5;
            HCDistrib[0x12, 1] = 0.5;
            HCDistrib[0x13, 1] = 0.665;
            HCDistrib[20, 1] = 0.665;
            HCDistrib[1, 2] = 0.283;
            HCDistrib[2, 2] = 0.283;
            HCDistrib[3, 2] = 0.283;
            HCDistrib[4, 2] = 0.295;
            HCDistrib[5, 2] = 0.322;
            HCDistrib[6, 2] = 0.322;
            HCDistrib[7, 2] = 0.322;
            HCDistrib[8, 2] = 0.322;
            HCDistrib[9, 2] = 0.366;
            HCDistrib[10, 2] = 0.366;
            HCDistrib[11, 2] = 0.366;
            HCDistrib[12, 2] = 0.366;
            HCDistrib[13, 2] = 0.462;
            HCDistrib[14, 2] = 0.462;
            HCDistrib[15, 2] = 0.462;
            HCDistrib[0x10, 2] = 0.462;
            HCDistrib[0x11, 2] = 0.5;
            HCDistrib[0x12, 2] = 0.5;
            HCDistrib[0x13, 2] = 0.67;
            HCDistrib[20, 2] = 0.67;
            HCDistrib[1, 3] = 0.286;
            HCDistrib[2, 3] = 0.308;
            HCDistrib[3, 3] = 0.308;
            HCDistrib[4, 3] = 0.376;
            HCDistrib[5, 3] = 0.349;
            HCDistrib[6, 3] = 0.349;
            HCDistrib[7, 3] = 0.349;
            HCDistrib[8, 3] = 0.349;
            HCDistrib[9, 3] = 0.397;
            HCDistrib[10, 3] = 0.397;
            HCDistrib[11, 3] = 0.397;
            HCDistrib[12, 3] = 0.397;
            HCDistrib[13, 3] = 0.5;
            HCDistrib[14, 3] = 0.5;
            HCDistrib[15, 3] = 0.5;
            HCDistrib[0x10, 3] = 0.5;
            HCDistrib[0x11, 3] = 0.5;
            HCDistrib[0x12, 3] = 0.5;
            HCDistrib[0x13, 3] = 0.665;
            HCDistrib[20, 3] = 0.665;
            HCDistrib[1, 4] = 0.091;
            HCDistrib[2, 4] = 0.083;
            HCDistrib[3, 4] = 0.083;
            HCDistrib[4, 4] = 0.092;
            HCDistrib[5, 4] = 0.092;
            HCDistrib[6, 4] = 0.092;
            HCDistrib[7, 4] = 0.092;
            HCDistrib[8, 4] = 0.092;
            HCDistrib[9, 4] = 0.141;
            HCDistrib[10, 4] = 0.141;
            HCDistrib[11, 4] = 0.141;
            HCDistrib[12, 4] = 0.141;
            HCDistrib[13, 4] = 0.164;
            HCDistrib[14, 4] = 0.164;
            HCDistrib[15, 4] = 0.164;
            HCDistrib[0x10, 4] = 0.164;
            HCDistrib[0x11, 4] = 0.2;
            HCDistrib[0x12, 4] = 0.2;
            HCDistrib[0x13, 4] = 0.0;
            HCDistrib[20, 4] = 0.0;
            HCDistrib[1, 5] = 0.164;
            HCDistrib[2, 5] = 0.15;
            HCDistrib[3, 5] = 0.15;
            HCDistrib[4, 5] = 0.168;
            HCDistrib[5, 5] = 0.168;
            HCDistrib[6, 5] = 0.168;
            HCDistrib[7, 5] = 0.168;
            HCDistrib[8, 5] = 0.168;
            HCDistrib[9, 5] = 0.256;
            HCDistrib[10, 5] = 0.256;
            HCDistrib[11, 5] = 0.256;
            HCDistrib[12, 5] = 0.256;
            HCDistrib[13, 5] = 0.298;
            HCDistrib[14, 5] = 0.298;
            HCDistrib[15, 5] = 0.298;
            HCDistrib[0x10, 5] = 0.298;
            HCDistrib[0x11, 5] = 0.3;
            HCDistrib[0x12, 5] = 0.3;
            HCDistrib[0x13, 5] = 0.0;
            HCDistrib[20, 5] = 0.0;
            HCDistrib[1, 6] = 0.172;
            HCDistrib[2, 6] = 0.15;
            HCDistrib[3, 6] = 0.15;
            HCDistrib[4, 6] = 0.26;
            HCDistrib[5, 6] = 0.233;
            HCDistrib[6, 6] = 0.233;
            HCDistrib[7, 6] = 0.233;
            HCDistrib[8, 6] = 0.233;
            HCDistrib[9, 6] = 0.234;
            HCDistrib[10, 6] = 0.234;
            HCDistrib[11, 6] = 0.234;
            HCDistrib[12, 6] = 0.234;
            HCDistrib[13, 6] = 0.192;
            HCDistrib[14, 6] = 0.192;
            HCDistrib[15, 6] = 0.192;
            HCDistrib[0x10, 6] = 0.192;
            HCDistrib[0x11, 6] = 0.0;
            HCDistrib[0x12, 6] = 0.0;
            HCDistrib[0x13, 6] = 0.0;
            HCDistrib[20, 6] = 0.0;
            HCDistrib[1, 7] = 0.083;
            HCDistrib[2, 7] = 0.083;
            HCDistrib[3, 7] = 0.083;
            HCDistrib[4, 7] = 0.116;
            HCDistrib[5, 7] = 0.116;
            HCDistrib[6, 7] = 0.116;
            HCDistrib[7, 7] = 0.116;
            HCDistrib[8, 7] = 0.116;
            HCDistrib[9, 7] = 0.132;
            HCDistrib[10, 7] = 0.132;
            HCDistrib[11, 7] = 0.132;
            HCDistrib[12, 7] = 0.132;
            HCDistrib[13, 7] = 0.096;
            HCDistrib[14, 7] = 0.096;
            HCDistrib[15, 7] = 0.096;
            HCDistrib[0x10, 7] = 0.096;
            HCDistrib[0x11, 7] = 0.0;
            HCDistrib[0x12, 7] = 0.0;
            HCDistrib[0x13, 7] = 0.0;
            HCDistrib[20, 7] = 0.0;
            HCDistrib[1, 8] = 0.206;
            HCDistrib[2, 8] = 0.206;
            HCDistrib[3, 8] = 0.206;
            HCDistrib[4, 8] = 0.215;
            HCDistrib[5, 8] = 0.215;
            HCDistrib[6, 8] = 0.215;
            HCDistrib[7, 8] = 0.215;
            HCDistrib[8, 8] = 0.215;
            HCDistrib[9, 8] = 0.158;
            HCDistrib[10, 8] = 0.158;
            HCDistrib[11, 8] = 0.158;
            HCDistrib[12, 8] = 0.158;
            HCDistrib[13, 8] = 0.0;
            HCDistrib[14, 8] = 0.0;
            HCDistrib[15, 8] = 0.0;
            HCDistrib[0x10, 8] = 0.0;
            HCDistrib[0x11, 8] = 0.0;
            HCDistrib[0x12, 8] = 0.0;
            HCDistrib[0x13, 8] = 0.0;
            HCDistrib[20, 8] = 0.0;
            HCDistrib[1, 9] = 0.08;
            HCDistrib[2, 9] = 0.102;
            HCDistrib[3, 9] = 0.102;
            HCDistrib[4, 9] = 0.08;
            HCDistrib[5, 9] = 0.107;
            HCDistrib[6, 9] = 0.107;
            HCDistrib[7, 9] = 0.107;
            HCDistrib[8, 9] = 0.107;
            HCDistrib[9, 9] = 0.079;
            HCDistrib[10, 9] = 0.079;
            HCDistrib[11, 9] = 0.079;
            HCDistrib[12, 9] = 0.079;
            HCDistrib[13, 9] = 0.0;
            HCDistrib[14, 9] = 0.0;
            HCDistrib[15, 9] = 0.0;
            HCDistrib[0x10, 9] = 0.0;
            HCDistrib[0x11, 9] = 0.0;
            HCDistrib[0x12, 9] = 0.0;
            HCDistrib[0x13, 9] = 0.0;
            HCDistrib[20, 9] = 0.0;
            HCDistrib[1, 10] = 0.19;
            HCDistrib[2, 10] = 0.19;
            HCDistrib[3, 10] = 0.19;
            HCDistrib[4, 10] = 0.133;
            HCDistrib[5, 10] = 0.133;
            HCDistrib[6, 10] = 0.133;
            HCDistrib[7, 10] = 0.133;
            HCDistrib[8, 10] = 0.133;
            HCDistrib[9, 10] = 0.0;
            HCDistrib[10, 10] = 0.0;
            HCDistrib[11, 10] = 0.0;
            HCDistrib[12, 10] = 0.0;
            HCDistrib[13, 10] = 0.0;
            HCDistrib[14, 10] = 0.0;
            HCDistrib[15, 10] = 0.0;
            HCDistrib[0x10, 10] = 0.0;
            HCDistrib[0x11, 10] = 0.0;
            HCDistrib[0x12, 10] = 0.0;
            HCDistrib[0x13, 10] = 0.0;
            HCDistrib[20, 10] = 0.0;
            HCDistrib[1, 11] = 0.093;
            HCDistrib[2, 11] = 0.093;
            HCDistrib[3, 11] = 0.093;
            HCDistrib[4, 11] = 0.066;
            HCDistrib[5, 11] = 0.066;
            HCDistrib[6, 11] = 0.066;
            HCDistrib[7, 11] = 0.066;
            HCDistrib[8, 11] = 0.066;
            HCDistrib[9, 11] = 0.0;
            HCDistrib[10, 11] = 0.0;
            HCDistrib[11, 11] = 0.0;
            HCDistrib[12, 11] = 0.0;
            HCDistrib[13, 11] = 0.0;
            HCDistrib[14, 11] = 0.0;
            HCDistrib[15, 11] = 0.0;
            HCDistrib[0x10, 11] = 0.0;
            HCDistrib[0x11, 11] = 0.0;
            HCDistrib[0x12, 11] = 0.0;
            HCDistrib[0x13, 11] = 0.0;
            HCDistrib[20, 11] = 0.0;
            HCDistrib[1, 12] = 0.176;
            HCDistrib[2, 12] = 0.176;
            HCDistrib[3, 12] = 0.176;
            HCDistrib[4, 12] = 0.0;
            HCDistrib[5, 12] = 0.0;
            HCDistrib[6, 12] = 0.0;
            HCDistrib[7, 12] = 0.0;
            HCDistrib[8, 12] = 0.0;
            HCDistrib[9, 12] = 0.0;
            HCDistrib[10, 12] = 0.0;
            HCDistrib[11, 12] = 0.0;
            HCDistrib[12, 12] = 0.0;
            HCDistrib[13, 12] = 0.0;
            HCDistrib[14, 12] = 0.0;
            HCDistrib[15, 12] = 0.0;
            HCDistrib[0x10, 12] = 0.0;
            HCDistrib[0x11, 12] = 0.0;
            HCDistrib[0x12, 12] = 0.0;
            HCDistrib[0x13, 12] = 0.0;
            HCDistrib[20, 12] = 0.0;
        }

        private void InitRateFactors(double ReactorP, double MetalAct, double AcidAct)
        {
            double y = 0.37;
            double num2 = -0.7;
            double num = 0.53;
            double num3 = 0.5;
            CHDehydrogTerm = (MetalAct * DHCHFact) / NaphMolFeed;
            CPDehydrogTerm = (MetalAct * DHCPFact) / NaphMolFeed;
            IsomTerm = ((AcidAct * Math.Pow(ReactorP, y)) * ISOMFact) / NaphMolFeed;
            CyclTerm = ((AcidAct * Math.Pow(ReactorP, num2)) * OPENFact) / NaphMolFeed;
            PCrackTerm = ((AcidAct * Math.Pow(ReactorP, num)) * PHCFact) / NaphMolFeed;
            DealkTerm = ((AcidAct * Math.Pow(ReactorP, num3)) * HDAFact) / NaphMolFeed;
            NCrackTerm = ((AcidAct * Math.Pow(ReactorP, num)) * NHCFact) / NaphMolFeed;
        }

        private void interDist(double[] D86Dist, double[] Cut)
        {
            int num5;
            double[] distB = new double[8];
            double[] yArray = new double[8];
            double[] numArray5 = new double[11];
            double[] numArray = new double[11];
            double[] regs = new double[4];
            yArray[1] = 0.0;
            yArray[2] = 10.0;
            yArray[3] = 30.0;
            yArray[4] = 50.0;
            yArray[5] = 70.0;
            yArray[6] = 90.0;
            yArray[7] = 100.0;
            numArray5[1] = 0.0;
            numArray5[2] = 120.0;
            numArray5[3] = 160.0;
            numArray5[4] = 180.0;
            numArray5[5] = 220.0;
            numArray5[6] = 270.0;
            numArray5[7] = 310.0;
            numArray5[8] = 350.0;
            numArray5[9] = 390.0;
            numArray5[10] = 600.0;
            ConvD86ToTBP(D86Dist, distB);
            numArray5[1] = distB[1];
            numArray5[10] = distB[7];
            int index = 2;
            do
            {
                if (numArray5[index] < numArray5[1])
                {
                    numArray5[index] = numArray5[1];
                }
                index++;
                num5 = 10;
            }
            while (index <= num5);
            index = 1;
            do
            {
                if (numArray5[index] > numArray5[10])
                {
                    numArray5[index] = numArray5[10];
                }
                index++;
                num5 = 9;
            }
            while (index <= num5);
            index = 1;
            do
            {
                int iFlag = 0;
                double y = 0.0;
                Quadinterp(distB, yArray, 7, numArray5[index], ref y, regs, iFlag);
                numArray[index] = y;
                index++;
                num5 = 10;
            }
            while (index <= num5);
            index = 1;
            do
            {
                if (numArray[index] < 0.0)
                {
                    numArray[index] = 0.0;
                }
                else if (numArray[index] > 100.0)
                {
                    numArray[index] = 100.0;
                }
                index++;
                num5 = 10;
            }
            while (index <= num5);
            index = 1;
            do
            {
                if (numArray[index + 1] < numArray[index])
                {
                    string message = "Error in short assay characterization. Please check distillation input data, then try again.";
                    EndWell(message);
                }
                index++;
                num5 = 9;
            }
            while (index <= num5);
            Cut[1] = numArray[2] - numArray[1];
            Cut[2] = numArray[3] - numArray[2];
            Cut[3] = numArray[4] - numArray[3];
            Cut[4] = numArray[5] - numArray[4];
            Cut[5] = numArray[6] - numArray[5];
            Cut[6] = numArray[7] - numArray[6];
            Cut[7] = numArray[8] - numArray[7];
            Cut[8] = numArray[9] - numArray[8];
            Cut[9] = numArray[10] - numArray[9];
        }

        private double KgMolToNM3(double value)
        {
            return (value * 22.415);
        }

        private double KgPerCm2GToAtm(double value)
        {
            return ((value / 1.033227555) + 1.0);
        }

        private double KgPerCm2GToPsia(double value)
        {
            return ((value / 0.070306958) + 14.6959488);
        }

        private double KgPerCm2ToAtm(double value)
        {
            return (value / 1.033227555);
        }

        private double KgPerCm2ToPsi(double value)
        {
            return (value / 0.070306958);
        }

        private double KgPerCm2ToPsig(double value)
        {
            return ((value / 0.070306958) - 14.6959488);
        }

        private double KgPerM3ToLbsPerFt3(double value)
        {
            return (value / 16.01846);
        }

        private double KgToLbs(double value)
        {
            return (value * 2.20462262);
        }

        private void KIdeal(double TSep, double PSep, double[] EqK)
        {
            int num8;
            double[] numArray3 = new double[0x20];
            double[] numArray = new double[0x20];
            double[] numArray2 = new double[0x20];
            int index = 1;
            do
            {
                double num4;
                numArray3[index] = 0.0;
                double num5 = TSep / TCrit[index];
                double num2 = PSep / PCrit[index];
                double num6 = num5 * num5;
                double num7 = num6 * num5;
                double num3 = num2 * num2;
                if (index == 1)
                {
                    num4 = (((1.96718 + (1.02972 / num5)) - (0.054009 * num5)) + (0.0005288 * num6)) + (0.008585 * num2);
                }
                else if (index == 2)
                {
                    num4 = ((((2.4384 - (2.2455 / num5)) - (0.34084 * num5)) + (0.00212 * num6)) - (0.00223 * num7)) + (num2 * (0.10486 - (0.03691 * num5)));
                }
                else
                {
                    num4 = (((((5.75748 - (3.01761 / num5)) - (4.985 * num5)) + (2.02299 * num6)) + (num2 * ((0.08427 + (0.26667 * num5)) - (0.31138 * num6)))) + (num3 * (-0.02655 + (0.02883 * num5)))) + (W[index] * ((((-4.23893 + (8.65808 * num5)) - (1.2206 / num5)) - (3.15224 * num7)) - (0.025 * (num2 - 0.6))));
                }
                numArray[index] = num4 * 2.3025851;
                numArray2[index] = 0.0;
                EqK[index] = Math.Exp((numArray[index] + numArray3[index]) - numArray2[index]) / num2;
                index++;
                num8 = 0x1f;
            }
            while (index <= num8);
        }

        private void KReal(double TSep, double PSep, double[] EqK, double[] X, double[] Y)
        {
            double num16;
            double num20;
            int num27;
            double[] numArray = new double[0x20];
            double[] numArray2 = new double[0x20];
            double[] numArray3 = new double[0x20];
            double[] numArray4 = new double[0x20];
            double[] numArray5 = new double[0x20];
            int num11 = 40;
            double x = 0.0;
            double num4 = 0.0;
            int index = 1;
            do
            {
                num20 = TSep / TCrit[index];
                numArray[index] = Math.Sqrt(0.4278 / (PCrit[index] * Math.Pow(num20, 2.5)));
                numArray2[index] = 0.0867 / (PCrit[index] * num20);
                x += numArray[index] * Y[index];
                num4 += numArray2[index] * Y[index];
                index++;
                num27 = 0x1f;
            }
            while (index <= num27);
            double num3 = Math.Pow(x, 2.0) / num4;
            double num6 = num4 * PSep;
            double num24 = 1.0;
            bool flag = false;
            int num26 = num11;
            for (int i = 1; i <= num26; i++)
            {
                double num8 = ((num24 / (num24 - num6)) - ((num3 * num6) / (num24 + num6))) - num24;
                if ((Math.Abs(num8) - 1E-06) <= 0.0)
                {
                    flag = true;
                    break;
                }
                double num7 = ((-num6 / Math.Pow(num24 - num6, 2.0)) + ((num3 * num6) / Math.Pow(num24 + num6, 2.0))) - 1.0;
                double num25 = num24 - (num8 / num7);
                if ((num25 - num6) < 0.0)
                {
                    num24 = (num24 + num6) * 0.5;
                }
                else if (num25 > 2.0)
                {
                    num24 = (num24 + 2.0) * 0.5;
                }
                else
                {
                    num24 = num25;
                }
            }
            if (!flag)
            {
                num24 = 1.02;
                string message = "Compressibility factor (Z) calculation failed in KReal routine. Used assigned value of 1.02.";
                CheckContinue(message);
            }
            double num23 = 0.0;
            double num15 = 0.0;
            index = 1;
            do
            {
                num16 = X[index] * MolVol[index];
                num23 += num16;
                num15 += num16 * Sol[index];
                index++;
                num27 = 0x1f;
            }
            while (index <= num27);
            num15 /= num23;
            double num14 = 1.1038889 * TSep;
            double num17 = num24 - 1.0;
            double num18 = Math.Log(num24 - num6);
            double num19 = Math.Log(1.0 + (num6 / num24));
            index = 1;
            do
            {
                numArray5[index] = (MolVol[index] / num14) * Math.Pow(Sol[index] - num15, 2.0);
                num20 = TSep / TCrit[index];
                double num12 = PSep / PCrit[index];
                double num21 = num20 * num20;
                double num22 = num21 * num20;
                double num13 = num12 * num12;
                if (index == 1)
                {
                    num16 = (((1.96718 + (1.02972 / num20)) - (0.054009 * num20)) + (0.0005288 * num21)) + (0.008585 * num12);
                }
                else if (index == 2)
                {
                    num16 = ((((2.4384 - (2.2455 / num20)) - (0.34084 * num20)) + (0.00212 * num21)) - (0.00223 * num22)) + (num12 * (0.10486 - (0.03691 * num20)));
                }
                else
                {
                    num16 = (((((5.75748 - (3.01761 / num20)) - (4.985 * num20)) + (2.02299 * num21)) + (num12 * ((0.08427 + (0.26667 * num20)) - (0.31138 * num21)))) + (num13 * (-0.02655 + (0.02883 * num20)))) + (W[index] * ((((-4.23893 + (8.65808 * num20)) - (1.2206 / num20)) - (3.15224 * num22)) - (0.025 * (num12 - 0.6))));
                }
                numArray3[index] = num16 * 2.3025851;
                double num5 = numArray2[index] / num4;
                double num2 = (2.0 * numArray[index]) / x;
                numArray4[index] = ((num17 * num5) - num18) - ((num3 * (num2 - num5)) * num19);
                EqK[index] = Math.Exp((numArray3[index] + numArray5[index]) - numArray4[index]) / num12;
                index++;
                num27 = 0x1f;
            }
            while (index <= num27);
        }

        private double KToC(double value)
        {
            return (value - 459.67);
        }

        private double KToF(double value)
        {
            return ((value * 1.8) - 459.67);
        }

        private double KToR(double value)
        {
            return (value * 1.8);
        }

        private double LbMolToSCF(double value)
        {
            return (value * 379.482);
        }

        private double LbsPerFt3ToGmPerCm3(double value)
        {
            return ((value / 2.20462262) * 0.0353146667);
        }

        private double LbsPerFt3ToKgPerM3(double value)
        {
            return (value * 16.01846);
        }

        private double LbsPerFt3ToSG(double value)
        {
            return (value / 62.37);
        }

        private double LbsPerGalToSG(double value)
        {
            return (value / 8.338);
        }

        private double LbsToKg(double value)
        {
            return (value / 2.20462262);
        }

        private double LbsToMT(double value)
        {
            return (value / 2204.62262);
        }

        private double M3ToBbl(double value)
        {
            return (value / 0.1589873);
        }

        private double MMBTUPerHrToMW(double value)
        {
            return (value * 0.2930711);
        }

        private double MTToLbs(double value)
        {
            return (value * 2204.62262);
        }

        private double NM3ToKgMol(double value)
        {
            return (value / 22.415);
        }

        private double PsiaToAtm(double value)
        {
            return (value / 14.6959488);
        }

        private double PsiaToKgPerCm2G(double value)
        {
            return ((value - 14.6959488) * 0.070306958);
        }

        private double PsiaToPsig(double value)
        {
            return (value - 14.6959488);
        }

        private double PsigToAtm(double value)
        {
            return ((value / 14.6959488) + 1.0);
        }

        private double PsigToKgPerCm2(double value)
        {
            return ((value + 14.6959488) * 0.070306958);
        }

        private double PsigToPsia(double value)
        {
            return (value + 14.6959488);
        }

        private double PsiToKgPerCm2(double value)
        {
            return (value * 0.070306958);
        }

        private void Quad(double[] GVal, double[] YVal, int MFlag, double[] Regs, double RVal)
        {
            int index = 1;
            double num3 = YVal[3] - YVal[2];
            double num4 = YVal[2] - YVal[1];
            if (Math.Abs(num4) <= (Math.Abs(num3) * 100.0))
            {
                index = 3;
                if (Math.Abs(num3) <= (Math.Abs(num4) * 100.0))
                {
                    double num5;
                    if (GVal[2] < 0.0)
                    {
                        index = 1;
                    }
                    index = 1;
                    double num8 = YVal[3] + YVal[1];
                    if (MFlag == 3)
                    {
                        num3 *= (YVal[1] + YVal[2]) + YVal[3];
                        num8 = (num8 * YVal[3]) + Math.Pow(YVal[1], 2.0);
                        index = 2;
                    }
                    if (num3 <= 0.001)
                    {
                        num3 = 0.001;
                    }
                    double num7 = 1.0 / num3;
                    double num2 = YVal[3] - YVal[1];
                    if (num2 <= 0.001)
                    {
                        num2 = 0.001;
                    }
                    double num9 = (GVal[3] - GVal[1]) / num2;
                    if (num4 <= 0.001)
                    {
                        num4 = 0.001;
                    }
                    double num10 = (GVal[2] - GVal[1]) / num4;
                    Regs[1] = num7 * (num9 - num10);
                    Regs[2] = num9 - (Regs[1] * num8);
                    Regs[3] = GVal[2] - (((Regs[1] * Math.Pow(YVal[2], (double)index)) + Regs[2]) * YVal[2]);
                    if (MFlag >= 2)
                    {
                        return;
                    }
                    if ((GVal[1] >= 0.0) & (GVal[3] <= 0.0))
                    {
                        num5 = Math.Abs((double)((GVal[3] - GVal[2]) / num3));
                        double num6 = Math.Abs(num10);
                        if ((num5 >= (100.0 * num6)) | (num5 <= (0.01 * num6)))
                        {
                            index = 1;
                            if (GVal[2] > 0.0)
                            {
                                index = 2;
                            }
                            RVal = (YVal[index] + YVal[index + 1]) / 2.0;
                            return;
                        }
                    }
                    if (Math.Abs(Regs[1]) >= (Math.Abs(Regs[2]) * 1E-06))
                    {
                        num3 = Math.Pow(Regs[2], 2.0) - ((4.0 * Regs[1]) * Regs[3]);
                        if (num3 >= 0.0)
                        {
                            num7 = Math.Sqrt(num3);
                            num8 = -1.0 / (2.0 * Regs[1]);
                            num3 = num8 * (Regs[2] + num7);
                            num4 = num8 * (Regs[2] - num7);
                            RVal = num3;
                            if ((GVal[1] >= 0.0) & (GVal[3] <= 0.0))
                            {
                                index = 1;
                                if (GVal[2] > 0.0)
                                {
                                    index = 2;
                                }
                                num5 = (YVal[index + 1] + YVal[index]) / 2.0;
                            }
                            else
                            {
                                index = 1;
                                if (GVal[3] > 0.0)
                                {
                                    index = 3;
                                }
                                num5 = YVal[index];
                            }
                            if (Math.Abs((double)(num5 - num3)) > Math.Abs((double)(num5 - num4)))
                            {
                                RVal = num4;
                            }
                            return;
                        }
                    }
                }
            }
            Regs[1] = 0.0;
            Regs[2] = (GVal[2] - GVal[index]) / (YVal[2] - YVal[index]);
            Regs[3] = GVal[2] - (YVal[2] * Regs[2]);
            RVal = -Regs[3] / Regs[2];
        }

        private void Quadinterp(double[] XArray, double[] YArray, int NVal, double X, ref double Y, double[] Regs, int IFlag)
        {
            int MFlag = 0;
            double RVal = 0;
            double[] VVal = new double[4];
            double[] GVal = new double[4];
            int IErr = 0;

            if (Y > 0.0)
            {
                IErr = 1;
            }

            int II = 1;

            if (NVal > 2)
            {
                for (int J = 2; J <= NVal; J++)
                {
                    II = J;
                    if (X <= XArray[J])
                    {
                        goto Label_0089;
                    }
                }
                II = NVal - 1;

            Label_0089:

                if (YArray[II] != YArray[II - 1])
                {
                    if (II >= NVal)
                    {
                        II = (int)Math.Round((double)(NVal - 1.0));
                    }
                    II--;
                    if ((II == IFlag) & (X < XArray[II + 1]))
                    {
                        goto Label_0192;
                    }

                    for (int J = 1; J < 3; J++)
                    {
                        int Tempint = (II + J) - 1;
                        VVal[J] = XArray[Tempint];
                        GVal[J] = YArray[Tempint];
                    }

                    MFlag = 2;
                    goto Label_017D;
                }
                II--;
            }

            Regs[1] = 0.0;
            if (XArray[II + 1] == XArray[II])
            {
                Regs[2] = YArray[II] / XArray[II];
                Regs[3] = 0.0;
            }
            else
            {
                Regs[2] = (YArray[II + 1] - YArray[II]) / (XArray[II + 1] - XArray[II]);
                Regs[3] = YArray[II] - (Regs[2] * XArray[II]);
            }
            MFlag = 2;
            goto Label_0329;
        Label_017D:
            Quad(GVal, VVal, MFlag, Regs, RVal);
            IFlag = II;
        Label_0192:
            if (!((IErr == 0) | (Regs[1] == 0.0)))
            {
                double d = -Regs[2] / (Regs[1] * 2.0);
                if (MFlag != 2)
                {
                    if (d < 0.0)
                    {
                        goto Label_0329;
                    }
                    d = (2.0 * d) / 3.0;
                    d = Math.Sqrt(d);

                    for (int J = 1; J <= 2; J++)
                    {
                        if (((d < VVal[1]) & (d < X)) | ((d > VVal[3]) & (d > X)))
                        {
                            d = -d;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (!(((d <= VVal[1]) & (X >= VVal[1])) | ((d <= VVal[2]) & (X >= VVal[2]))) && !(((d >= VVal[2]) & (X <= VVal[2])) | ((d >= VVal[3]) & (X <= VVal[3]))))
                {
                    if (MFlag != 3)
                    {
                        MFlag = 3;
                        goto Label_017D;
                    }
                    II = 1;
                    if (X > VVal[2])
                    {
                        II = 3;
                    }
                    Regs[1] = 0.0;
                    Regs[2] = (GVal[2] - GVal[II]) / (VVal[2] - VVal[II]);
                    Regs[3] = GVal[2] - (Regs[2] * VVal[2]);
                }
            }
        Label_0329:
            int JJ = MFlag - 1;
            Y = (((Regs[1] * Math.Pow(X, JJ)) + Regs[2]) * X) + Regs[3];
        }

        private void Reactors(double[] F_RecyStar, double[,] F_Inlet, double[] F_Eff, double[] ReactorT_Out)
        {
            double[] f = new double[NumComp];
            double[] RactorT = new double[MaxNumReactor];

            for (int JJ = 1; JJ <= NumReactor; JJ++)
            {
                int ConvPrintCol = 0;
                int ConvPrintRow = 0;
                if (JJ == 1)
                {
                    for (int I = 1; I <= NumComp; I++)
                    {
                        F_Inlet[I, JJ] = MolFeed[I] + F_RecyStar[I];
                        f[I] = F_Inlet[I, JJ];
                    }
                }
                else
                {
                    for (int I = 1; I <= NumComp; I++)
                    {
                        F_Inlet[I, JJ] = f[I];
                    }
                }
                InitRateFactors(ReactorP[JJ], MetalAct[JJ], AcidAct[JJ]);
                RactorT[JJ] = ReactorT_In[JJ];
                RungeKutta(RactorT[JJ], ReactorP[JJ], AmtCat[JJ], f, ConvPrintRow, ConvPrintCol);
                ReactorT_Out[JJ] = RactorT[JJ];
            }

            for (int I = 1; I <= NumComp; I++)
            {
                F_Eff[I] = f[I];
            }
        }

        private void ReadInput()
        {
            string AssayUnit;
            double Factor;
            double num6;
            string Message;
            double SumFeedInput;
            double TotCatAmt = 0;
            double[] pctVol = new double[0x20];
            double[] numArray2 = new double[5];

            TempUnits = "C";
            PressUnits = "kg/cm2 g";
            InputOption = 3;
            if (TempUnits == "C")
            {
                for (int I = 1; I <= 7; I++)
                {
                    if (D86[I] > 0.0)
                    {
                        D86[I] = CToF(D86[I]);
                    }
                }
            }
            else if (TempUnits == "R")
            {
                for (int I = 1; I <= 7; I++)
                {
                    if (D86[I] != 0)
                    {
                        D86[I] = RToF(D86[I]);
                    }
                }
            }
            else if (TempUnits == "K")
            {
                for (int I = 1; I <= 7; I++)
                {
                    if (D86[I] != 0)
                    {
                        D86[I] = KToF(D86[I]);
                    }
                }
            }
            if (InputOption == 1)
            {
                AssayUnit = "Volume %";
                pctVol[1] = 0.0;
                pctVol[2] = 0.0;
                pctVol[3] = 0.0;
                pctVol[4] = 0.0;
                pctVol[5] = 0.0;
                pctVol[6] = 0.0;
                pctVol[7] = 0.0;
                pctVol[8] = 0.0;
                pctVol[9] = 0.0;
                pctVol[10] = 0.0;
                pctVol[11] = 0.0;
                pctVol[12] = 0.0;
                pctVol[13] = 0.0;
                pctVol[14] = 0.0;
                pctVol[15] = 0.0;
                pctVol[0x10] = 0.0;
                pctVol[0x11] = 0.0;
                pctVol[0x12] = 0.0;
                pctVol[0x13] = 0.0;
                pctVol[20] = 0.0;
                pctVol[0x15] = 0.0;
                pctVol[0x16] = 0.0;
                pctVol[0x17] = 0.0;
                pctVol[0x18] = 0.0;
                pctVol[0x19] = 0.0;
                pctVol[0x1a] = 0.0;
                pctVol[0x1b] = 0.0;
                pctVol[0x1c] = 0.0;
                pctVol[0x1d] = 0.0;
                pctVol[30] = 0.0;
                pctVol[0x1f] = 0.0;
            }
            else if (InputOption != 2)
            {
                if (InputOption == 3)
                {
                    P_LV = 60.87;
                    N_LV = 28.34;
                    A_LV = 10.79;
                    if ((((((P_LV < 0.0) | (P_LV > 100.0)) | (N_LV < 0.0)) | (N_LV > 100.0)) | (A_LV < 0.0)) | (A_LV > 100.0))
                    {
                        Message = "For the Short Assay, the percentage of paraffins, naphthenes, and aromatics in the feed must be between  0 and 100.  Check input, then try again.";
                        EndWell(Message);
                    }
                    else
                    {
                        SumFeedInput = 0.0;
                        SumFeedInput = (P_LV + N_LV) + A_LV;
                        if (SumFeedInput < 1E-06)
                        {
                            Message = "For the Short Assay, enter the percentage of paraffins, naphthenes, and aromatics in the feed, then try again.";
                            EndWell(Message);
                        }
                        else if (Math.Abs((double)(SumFeedInput - 100.0)) > 0.01)
                        {
                            Message = "Check feed input.  Sum of naphtha short assay percentages does not equal 100%.  Program will continue after normalizing to 100%.";
                            CheckContinue(Message);
                            P_LV = (P_LV / SumFeedInput) * 100.0;
                            N_LV = (N_LV / SumFeedInput) * 100.0;
                            A_LV = (A_LV / SumFeedInput) * 100.0;
                        }
                    }

                    for (int I = 1; I <= 7; I++)
                    {
                        if (D86[I] < 0.1)
                        {
                            Message = "For the Short Assay, the IBP, 50%, and EP Temperature s are required.  Check input, then try again.";
                            EndWell(Message);
                        }
                        I += 2;
                    }

                    string DensityUnits = "Specific Gravity";
                    double Density = 0.747;

                    if (DensityUnits == "API Gravity")
                    {
                        APIFeed = Density;
                        SGFeed = APIToSG(Density);
                    }
                    else if (DensityUnits == "Specific Gravity")
                    {
                        SGFeed = Density;
                        APIFeed = SGToAPI(Density);
                    }
                    else
                    {
                        Message = "For the Short Assay, please select API Gravity or Specific Gravity for feed density units, then try again.";
                        EndWell(Message);
                    }
                    if (SGFeed < 0.1)
                    {
                        Message = "For the Short Assay, feed density must be greater than zero.  Check input, then try again.";
                        EndWell(Message);
                    }
                    ShortCharFeed(P_LV, N_LV, A_LV, D86, pctVol);
                    AssayUnit = "Volume %";
                }
                else
                {
                    Message = "Please select short, medium, or full assay from the option buttons, then try again.";
                    EndWell(Message);
                }
            }
            if (SpecOct)
            {
                OctSpec = 100.0;
                if ((OctSpec < 50.0) | (OctSpec > 140.0))
                {
                    Message = "Octane specification is outside range.  Check input, then try again.";
                    EndWell(Message);
                }
            }
            SumFeedInput = 0.0;

            for (int I = 1; I < NumComp; I++)
            {
                if (pctVol[I] < 0.0)
                {
                    Message = "Feed assay percent must be non-negative.  Check input and try again.";
                    EndWell(Message);
                }
                else
                {
                    SumFeedInput += pctVol[I];
                }
            }

            if (SumFeedInput <= 0.0)
            {
                Message = "Sum of naphtha component assay must be greater than zero.  Check input and try again.";
                EndWell(Message);
            }
            else if ((Math.Abs((double)(SumFeedInput - 100.0)) > 0.01) & !OptionShort)
            {
                Message = "Check feed input.  Sum of naphtha assay does not equal 100%.  Program will continue after normalizing to 100%.";
                CheckContinue(Message);
            }
            NaphFeedRate *= 24.0;
            if (NaphFeedRate <= 0.0)
            {
                Message = "Feed rate must be greater than zero. Check input and try again.";
                EndWell(Message);
            }
            string NaphFeedUnit = "MTPD";
            if (NaphFeedUnit == "kg-mol/hr")
            {
                NaphFeedRate = KgToLbs(NaphFeedRate);
                NaphFeedUnit = "Mol/Hr";
            }
            else if (NaphFeedUnit == "kg/hr")
            {
                NaphFeedRate = KgToLbs(NaphFeedRate);
                NaphFeedUnit = "Lbs/Hr";
            }
            else if (NaphFeedUnit == "MTPD")
            {
                NaphFeedRate = MTToLbs(NaphFeedRate) / 24;
                NaphFeedUnit = "Lbs/Hr";
            }
            else if (NaphFeedUnit == "m\x00b3/day")
            {
                NaphFeedRate = M3ToBbl(NaphFeedRate);
                NaphFeedUnit = "BPD";
            }

            AssayUnit = "Volume %";
            switch (AssayUnit)
            {
                case "Mole %":
                    NaphMolFeed = NaphFeedRate;
                    NaphLbsFeed = 0.0;
                    NaphBpdFeed = 0.0;

                    for (int I = 1; I < NumComp; I++)
                    {
                        MolFeed[I] = (pctVol[I] / SumFeedInput) * NaphMolFeed;
                        LbsFeed[I] = MolFeed[I] * MW[I];
                        NaphLbsFeed += LbsFeed[I];
                        BpdFeed[I] = LbsFeed[I] / (SG[I] * 14.574166);
                        NaphBpdFeed += BpdFeed[I];
                    }

                    switch (NaphFeedUnit)
                    {
                        case "Lbs/Hr":
                            Factor = NaphFeedRate / NaphLbsFeed;
                            for (int I = 1; I < NumComp; I++)
                            {
                                MolFeed[I] *= Factor;
                                LbsFeed[I] *= Factor;
                                BpdFeed[I] *= Factor;
                            }

                            NaphMolFeed *= Factor;
                            NaphLbsFeed = NaphFeedRate;
                            NaphBpdFeed *= Factor;
                            break;

                        case "BPD":
                            Factor = NaphFeedRate / NaphBpdFeed;
                            for (int I = 1; I < NumComp; I++)
                            {
                                MolFeed[I] *= Factor;
                                LbsFeed[I] *= Factor;
                                BpdFeed[I] *= Factor;
                            }

                            NaphMolFeed *= Factor;
                            NaphLbsFeed *= Factor;
                            NaphBpdFeed = NaphFeedRate;
                            break;
                    }
                    if (NaphFeedUnit != "Mol/Hr")
                    {
                        Message = "Please select Mole/Hr, Lbs/Hr, BPD, kg-mol/hr, kg/hr, or m^3/day for naphtha feed rate units, then try again.";
                        EndWell(Message);
                    }
                    break;

                case "Weight %":
                    NaphMolFeed = 0.0;
                    NaphLbsFeed = NaphFeedRate;
                    NaphBpdFeed = 0.0;
                    for (int I = 1; I < NumComp; I++)
                    {
                        LbsFeed[I] = (pctVol[I] / SumFeedInput) * NaphLbsFeed;
                        MolFeed[I] = LbsFeed[I] / MW[I];
                        NaphMolFeed += MolFeed[I];
                        BpdFeed[I] = LbsFeed[I] / (SG[I] * 14.574166);
                        NaphBpdFeed += BpdFeed[I];
                    }

                    switch (NaphFeedUnit)
                    {
                        case "Mol/Hr":
                            Factor = NaphFeedRate / NaphMolFeed;
                            for (int I = 1; I < NumComp; I++)
                            {
                                MolFeed[I] *= Factor;
                                LbsFeed[I] *= Factor;
                                BpdFeed[I] *= Factor;
                            }

                            NaphMolFeed = NaphFeedRate;
                            NaphLbsFeed *= Factor;
                            NaphBpdFeed *= Factor;
                            break;

                        case "BPD":
                            Factor = NaphFeedRate / NaphBpdFeed;
                            for (int I = 1; I < NumComp; I++)
                            {
                                MolFeed[I] *= Factor;
                                LbsFeed[I] *= Factor;
                                BpdFeed[I] *= Factor;
                            }

                            NaphMolFeed *= Factor;
                            NaphLbsFeed *= Factor;
                            NaphBpdFeed = NaphFeedRate;
                            break;
                    }
                    if (NaphFeedUnit != "Lbs/Hr")
                    {
                        Message = "Please select Mole/Hr, Lbs/Hr, BPD, kg-mol/hr, kg/hr, or m^3/day for naphtha feed rate units, then try again.";
                        EndWell(Message);
                    }
                    break;

                case "Volume %":
                    NaphMolFeed = 0.0;
                    NaphLbsFeed = 0.0;
                    NaphBpdFeed = NaphFeedRate;

                    for (int I = 1; I < NumComp; I++)
                    {
                        BpdFeed[I] = (pctVol[I] / SumFeedInput) * NaphBpdFeed;
                        LbsFeed[I] = (BpdFeed[I] * SG[I]) * 14.574166;
                        NaphLbsFeed += LbsFeed[I];
                        MolFeed[I] = LbsFeed[I] / MW[I];
                        NaphMolFeed += MolFeed[I];
                    }

                    switch (NaphFeedUnit)
                    {
                        case "Mol/Hr":
                            Factor = NaphFeedRate / NaphMolFeed;

                            for (int I = 1; I < NumComp; I++)
                            {
                                MolFeed[I] *= Factor;
                                LbsFeed[I] *= Factor;
                                BpdFeed[I] *= Factor;
                            }

                            NaphMolFeed = NaphFeedRate;
                            NaphLbsFeed *= Factor;
                            NaphBpdFeed *= Factor;
                            break;

                        case "Lbs/Hr":
                            Factor = NaphFeedRate / NaphLbsFeed;
                            for (int I = 1; I < NumComp; I++)
                            {
                                MolFeed[I] *= Factor;
                                LbsFeed[I] *= Factor;
                                BpdFeed[I] *= Factor;
                            }

                            NaphMolFeed *= Factor;
                            NaphLbsFeed = NaphFeedRate;
                            NaphBpdFeed *= Factor;
                            break;
                    }
                    if (NaphFeedUnit != "BPD")
                    {
                        Message = "Please select Mole/Hr, Lbs/Hr, BPD, kg-mol/hr, kg/hr, or m^3/day for naphtha feed rate units, then try again.";
                        EndWell(Message);
                    }
                    break;

                default:
                    Message = "Please select Mole %, Weight %, or Volume % for naphtha assay input, then try again.";
                    EndWell(Message);
                    break;
            }
            CharFeed();
            double num4 = 3.2;
            if (num4 <= 0.0)
            {
                Message = "Hydrogen to hydrocarbon ratio must be positive.   Check H2HC value and try again.";
                EndWell(Message);
            }
            else
            {
                MolH2Recy = num4 * NaphMolFeed;
            }
            if (DHCH > 0.0)
            {
                DHCHFact = DHCH;
            }
            else
            {
                DHCHFact = 2.75;
            }
            if (DHCP > 0.0)
            {
                DHCPFact = DHCP;
            }
            else
            {
                DHCPFact = 2.75;
            }
            if (ISOM > 0.0)
            {
                ISOMFact = ISOM;
            }
            else
            {
                ISOMFact = 2.75;
            }
            if (OPEN > 0.0)
            {
                OPENFact = OPEN;
            }
            else
            {
                OPENFact = 2.75;
            }
            if (PHC > 0.0)
            {
                PHCFact = PHC;
            }
            else
            {
                PHCFact = 2.75;
            }
            if (NHC > 0.0)
            {
                NHCFact = NHC;
            }
            else
            {
                NHCFact = 2.75;
            }
            if (HDA > 0.0)
            {
                HDAFact = HDA;
            }
            else
            {
                HDAFact = 2.75;
            }
            InitialTemp = 510.0;
            InitialP = 4.7;
            MetalActiv = 0.0;
            AcidActiv = 0.0;
            AmtCat1 = 20.0;
            InitialTemp2 = 510.0;
            InitialP2 = 4.2;
            MetalActiv2 = 0.0;
            AcidActiv2 = 0.0;
            AmtCat2 = 30.0;
            InitialTemp3 = 510.0;
            InitialP3 = 3.7;
            MetalActiv3 = 0.0;
            AcidActiv3 = 0.0;
            AmtCat3 = 50.0;
            InitialTemp4 = 510.0;
            InitialP4 = 4.7;
            MetalActiv4 = 0.0;
            AcidActiv4 = 0.0;
            AmtCat4 = 0.0;
            ReactorT_In[1] = InitialTemp;
            InletP[1] = InitialP;
            ReactorP[1] = InletP[1] - (DP / 2.0);
            MetalAct[1] = MetalActiv;
            AcidAct[1] = AcidActiv;
            CatPercent[1] = AmtCat1;
            ReactorT_In[2] = InitialTemp2;
            InletP[2] = InitialP2;
            ReactorP[2] = InletP[2] - (DP2 / 2.0);
            MetalAct[2] = MetalActiv2;
            AcidAct[2] = AcidActiv2;
            CatPercent[2] = AmtCat2;
            ReactorT_In[3] = InitialTemp3;
            InletP[3] = InitialP3;
            ReactorP[3] = InletP[3] - (DP3 / 2.0);
            MetalAct[3] = MetalActiv3;
            AcidAct[3] = AcidActiv3;
            CatPercent[3] = AmtCat3;
            ReactorT_In[4] = InitialTemp4;
            InletP[4] = InitialP4;
            ReactorP[4] = InletP[4] - (DP4 / 2.0);
            MetalAct[4] = MetalActiv4;
            AcidAct[4] = AcidActiv4;
            CatPercent[4] = AmtCat4;
            NumReactor = 3;
            double SumCatPercent = 0.0;

            for (int I = 1; I <= NumReactor; I++)
            {
                if (CatPercent[I] <= 0.0)
                {
                    Message = "Percentage of catalyst in each reactor (for selected number of reactors) must be greater than zero. Check input and try again.";
                    EndWell(Message);
                }
                else
                {
                    SumCatPercent += CatPercent[I];
                }
            }
            if ((SumCatPercent < 99.5) | (SumCatPercent > 100.5))
            {
                Message = "For the selected number of reactors, the sum of percent catalyst loading should equal 100. Check reactor catalyst loading and try again.";
                EndWell(Message);
            }
            else if (Math.Abs((double)(SumCatPercent - 100.0)) >= 0.01)
            {
                for (int I = 1; I <= NumReactor; I++)
                {
                    CatPercent[I] = (CatPercent[I] / SumCatPercent) * 100.0;
                }
                SumCatPercent = 100.0;
            }

            for (int I = 1; I <= NumReactor; I++)
            {
                if ((InletP[I] <= 0.0) | (ReactorT_In[I] <= 0.0))
                {
                    Message = "For each reactor modeled, enter the catalyst loading, inlet Temperature , inlet Pressure , and Pressure  drop across the catalyst bed.  Check input for reactor , then try again.";
                    EndWell(Message);
                }
            }
            for (int I = 1; I < NumComp; I++)
            {
                if (MetalAct[I] <= 0.0)
                {
                    MetalAct[I] = 1.0;
                }
                if (AcidAct[I] <= 0.0)
                {
                    AcidAct[I] = 1.0;
                }
            }

            double num = 37.7;
            CatalystUnits = "LHSV";
            switch (CatalystUnits)
            {
                case "Lbs":
                    TotCatAmt = 0.0;
                    if (TotCatAmt <= 0.0)
                    {
                        Message = "Total catalyst amount must be greater than zero. Check input and try again.";
                        EndWell(Message);
                    }
                    num6 = ((NaphBpdFeed * 0.233940973) / TotCatAmt) * num;
                    break;

                case "kg":
                    TotCatAmt = KgToLbs(0.0);
                    if (TotCatAmt <= 0.0)
                    {
                        Message = "Total catalyst amount must be greater than zero. Check input and try again.";
                        EndWell(Message);
                    }
                    num6 = ((NaphBpdFeed * 0.233940973) / TotCatAmt) * num;
                    break;

                case "LHSV":
                    num6 = 1.95;
                    if (num6 <= 0.0)
                    {
                        Message = "LHSV (liquid hourly space volume) must be greater than zero. Check input and try again.";
                        EndWell(Message);
                    }
                    TotCatAmt = ((NaphBpdFeed * 0.233940973) / num6) * num;
                    break;

                default:
                    Message = "Choose valid units for catalyst amount (Lbs, kg, or LHSV) from drop-down menu, then try again.";
                    EndWell(Message);
                    break;
            }

            for (int I = 1; I <= NumReactor; I++)
            {
                AmtCat[I] = (CatPercent[I] / SumCatPercent) * TotCatAmt;
            }

            TotWHSV = 0.0;

            for (int I = 1; I <= NumReactor; I++)
            {
                numArray2[I] = NaphBpdFeed / TotCatAmt;
                TotWHSV += 1.0 / numArray2[I];
            }
            TotWHSV = 1.0 / TotWHSV;
            TSep = 38.0;
            PSep = 2.46;
            if (PSep <= 0.0)
            {
                Message = "Separator Pressure  must be greater than zero. Check input and try again.";
                EndWell(Message);
            }
            FurnEffic[1] = 82.0;
            FurnEffic[2] = 82.0;
            FurnEffic[3] = 82.0;

            for (int I = 1; I < NumComp; I++)
            {
                if ((FurnEffic[I] > 100.0) | (FurnEffic[I] <= 1.0))
                {
                    Message = "Please enter furnace efficiency as a percentage (between 1 and 100).  Check input and try again. If the cell is left blank, the default value of 82% will be used.";
                    EndWell(Message);
                }
            }
            if (TempUnits == "\x00b0F")
            {
                for (int I = 1; I <= NumReactor; I++)
                {
                    ReactorT_In[I] = FToR(ReactorT_In[I]);
                }
                TSep = FToR(TSep);
            }
            else if (TempUnits == "\x00b0C")
            {
                for (int I = 1; I <= NumReactor; I++)
                {
                    ReactorT_In[I] = CToR(ReactorT_In[I]);
                }
                TSep = CToR(TSep);
            }
            else if (TempUnits == "K")
            {
                for (int I = 1; I <= NumReactor; I++)
                {
                    ReactorT_In[I] = KToR(ReactorT_In[I]);
                }
                TSep = KToR(TSep);
            }
            else if (TempUnits != "\x00b0R")
            {
                Message = "No Temperature  unit chosen. Program will continue using   default unit, \x00b0F.";
                CheckContinue(Message);
                int num18 = NumReactor;
                for (int I = 1; I <= num18; I++)
                {
                    ReactorT_In[I] = FToR(ReactorT_In[I]);
                }
                TSep = FToR(TSep);
            }
            if (PressUnits == "kg/cm\x00b2 g")
            {
                for (int I = 1; I <= NumReactor; I++)
                {
                    InletP[I] = KgPerCm2ToPsi(InletP[I]);
                    ReactorP[I] = KgPerCm2GToAtm(ReactorP[I]);
                }
                PSep = KgPerCm2GToPsia(PSep);
            }
            else if (PressUnits == "kg/cm\x00b2 a")
            {
                int num20 = NumReactor;
                for (int I = 1; I <= num20; I++)
                {
                    InletP[I] = KgPerCm2ToPsig(InletP[I]);
                    ReactorP[I] = KgPerCm2ToAtm(ReactorP[I]);
                }
                PSep = KgPerCm2ToPsi(PSep);
            }
            else if (PressUnits == "psia")
            {
                int num21 = NumReactor;
                for (int I = 1; I <= num21; I++)
                {
                    InletP[I] = PsiaToPsig(InletP[I]);
                    ReactorP[I] = PsiaToAtm(ReactorP[I]);
                }
            }
            else
            {
                if (PressUnits != "psig")
                {
                    Message = "No Pressure  unit chosen. Program will continue using   default unit, psig.";
                    CheckContinue(Message);
                }
                int num22 = NumReactor;
                for (int I = 1; I <= num22; I++)
                {
                    ReactorP[I] = PsigToAtm(ReactorP[I]);
                }
                PSep = PsigToPsia(PSep);
            }
        }

        private double RToC(double value)
        {
            return ((value / 1.8) - 273.15);
        }

        private double RToF(double value)
        {
            return (value - 459.67);
        }

        private double RToK(double value)
        {
            return (value / 1.8);
        }

        private void RungeKutta(double TR, double ReactorP, double AmtCat, double[] F, int ConvPrintRow, int ConvPrintCol)
        {
            double[] flowRate = new double[0x20];
            double[] numArray3 = new double[0x20];
            double[] numArray4 = new double[0x21];
            double[] numArray5 = new double[0x21];
            double[] numArray6 = new double[0x21];
            double[] numArray7 = new double[0x21];
            double[] numArray8 = new double[0x21];
            double[] rateOfChange = new double[0x21];
            int num10 = 0x19;
            double num6 = 10000.0;
            double num5 = AmtCat / ((double)num10);
            double num8 = AmtCat / num6;
            double num2 = num5;
            double num7 = 0.05;
            double num = 0.0;
            double a = 0.0;
        Label_0357:
            while (((long)Math.Round(a)) < ((long)Math.Round(AmtCat)))
            {
                int num15;
                CalcRates(TR, ReactorP, F, rateOfChange);
                int index = 1;
                do
                {
                    numArray4[index] = num2 * rateOfChange[index];
                    index++;
                    num15 = 0x20;
                }
                while (index <= num15);
                index = 1;
                do
                {
                    flowRate[index] = F[index] + (numArray4[index] / 2.0);
                    index++;
                    num15 = 0x1f;
                }
                while (index <= num15);
                double tR = TR + (numArray4[0x20] / 2.0);
                CalcRates(tR, ReactorP, flowRate, rateOfChange);
                index = 1;
                do
                {
                    numArray5[index] = num2 * rateOfChange[index];
                    index++;
                    num15 = 0x20;
                }
                while (index <= num15);
                index = 1;
                do
                {
                    flowRate[index] = F[index] + (numArray5[index] / 2.0);
                    index++;
                    num15 = 0x1f;
                }
                while (index <= num15);
                tR = TR + (numArray5[0x20] / 2.0);
                CalcRates(tR, ReactorP, flowRate, rateOfChange);
                index = 1;
                do
                {
                    numArray6[index] = num2 * rateOfChange[index];
                    index++;
                    num15 = 0x20;
                }
                while (index <= num15);
                index = 1;
                do
                {
                    flowRate[index] = F[index] + numArray6[index];
                    index++;
                    num15 = 0x1f;
                }
                while (index <= num15);
                tR = TR + numArray6[0x20];
                CalcRates(tR, ReactorP, flowRate, rateOfChange);
                index = 1;
                do
                {
                    numArray7[index] = num2 * rateOfChange[index];
                    index++;
                    num15 = 0x20;
                }
                while (index <= num15);
                index = 1;
                do
                {
                    numArray3[index] = F[index] + ((((numArray4[index] + (2.0 * numArray5[index])) + (2.0 * numArray6[index])) + numArray7[index]) / 6.0);
                    index++;
                    num15 = 0x1f;
                }
                while (index <= num15);
                double num12 = TR + ((((numArray4[0x20] + (2.0 * numArray5[0x20])) + (2.0 * numArray6[0x20])) + numArray7[0x20]) / 6.0);
                CalcRates(num12, ReactorP, numArray3, rateOfChange);
                index = 1;
                do
                {
                    numArray8[index] = ((numArray3[index] - F[index]) / num2) - rateOfChange[index];
                    index++;
                    num15 = 0x1f;
                }
                while (index <= num15);
                numArray8[0x20] = ((num12 - TR) / num2) - rateOfChange[index];
                index = 1;
                do
                {
                    if (Math.Abs(numArray8[index]) > num)
                    {
                        if (Math.Abs(numArray8[index]) > num7)
                        {
                            if (num2 > num8)
                            {
                                num2 /= 2.0;
                                goto Label_0357;
                            }
                            string message = "Simulation abandoned due to failure to converge";
                            EndWell(message);
                        }
                        num = numArray8[index];
                    }
                    index++;
                    num15 = 0x20;
                }
                while (index <= num15);
                index = 1;
                do
                {
                    F[index] = numArray3[index];
                    index++;
                    num15 = 0x1f;
                }
                while (index <= num15);
                TR = num12;
                a += num2;
            }
        }

        public void RunSimulation()
        {
            InitConstants();
            ReadInput();
            SolveCase();
        }

        private double SGToAPI(double value)
        {
            return ((141.5 / value) - 131.5);
        }

        private double SGToLbsPerFt3(double value)
        {
            return (value * 62.37);
        }

        private double SGToLbsPerGal(double value)
        {
            return (value * 8.338);
        }

        private void ShortCharFeed(double P_LV, double N_LV, double A_LV, double[] D86, double[] PctVol)
        {
            double num151;
            double[] cut = new double[10];
            double[] numArray7 = new double[0x16];
            double[] numArray4 = new double[11];
            double[] numArray8 = new double[12];
            double[] numArray = new double[14];
            double[] numArray2 = new double[4];
            double[] numArray6 = new double[0x2b];
            double[] numArray5 = new double[0x2b];
            double a = 1.0;
            do
            {
                numArray6[(int)Math.Round(a)] = SG[(int)Math.Round(a)];
                numArray5[(int)Math.Round(a)] = MW[(int)Math.Round(a)];
                a++;
                num151 = 42.0;
            }
            while (a <= num151);
            if (D86[2] < 0.1)
            {
                if (D86[3] < 0.1)
                {
                    D86[2] = D86[1] + (0.4474 * (D86[4] - D86[1]));
                }
                else
                {
                    D86[2] = D86[1] + (0.6081 * (D86[3] - D86[1]));
                }
            }
            if (D86[3] < 0.1)
            {
                if (D86[2] < 0.1)
                {
                    D86[3] = D86[1] + (0.7335 * (D86[4] - D86[1]));
                }
                else
                {
                    D86[3] = D86[2] + (0.5236 * (D86[4] - D86[2]));
                }
            }
            if (D86[5] < 0.1)
            {
                if (D86[6] < 0.1)
                {
                    D86[5] = D86[4] + (0.2003 * (D86[7] - D86[4]));
                }
                else
                {
                    D86[5] = D86[4] + (0.3802 * (D86[6] - D86[4]));
                }
            }
            if (D86[6] < 0.1)
            {
                if (D86[5] < 0.1)
                {
                    D86[6] = D86[4] + (0.5274 * (D86[7] - D86[4]));
                }
                else
                {
                    D86[6] = D86[5] + (0.4095 * (D86[7] - D86[5]));
                }
            }
            interDist(D86, cut);
            double num132 = 0.0;
            a = 7.0;
            do
            {
                num132 += cut[(int)Math.Round(a)];
                a++;
                num151 = 9.0;
            }
            while (a <= num151);
            double num120 = 100.0 / num132;
            a = 7.0;
            do
            {
                cut[(int)Math.Round(a)] *= num120;
                a++;
                num151 = 9.0;
            }
            while (a <= num151);
            numArray4[1] = P_LV * 0.01;
            numArray4[2] = N_LV * 0.01;
            numArray4[3] = A_LV * 0.01;
            numArray7[1] = N_LV + A_LV;
            numArray7[2] = P_LV + N_LV;
            numArray4[4] = N_LV / numArray7[1];
            numArray4[5] = A_LV / numArray7[1];
            numArray4[6] = P_LV / numArray7[2];
            numArray4[7] = N_LV / numArray7[2];
            double num75 = cut[1];
            double num81 = cut[2];
            double num79 = cut[3] * numArray4[4];
            double num77 = cut[3] * numArray4[5];
            double num87 = cut[4] * numArray4[6];
            double num85 = cut[4] * numArray4[7];
            double num83 = cut[5] * numArray4[3];
            double num93 = cut[5] * numArray4[1];
            double num91 = cut[5] * numArray4[2];
            double num89 = cut[6] * numArray4[3];
            double num111 = cut[6] * numArray4[1];
            double num103 = cut[6] * numArray4[2];
            double num95 = cut[7] * numArray4[3];
            double num19 = cut[7] * numArray4[1];
            double num11 = cut[7] * numArray4[2];
            double num3 = cut[8] * numArray4[3];
            double num43 = cut[8] * numArray4[1];
            double num35 = cut[8] * numArray4[2];
            double num27 = cut[9] * numArray4[3];
            double num67 = cut[9] * numArray4[1];
            double num59 = cut[9] * numArray4[2];
            double num127 = ((((((num75 + num81) + num87) + num93) + num111) + num19) + num43) + num67;
            double num125 = (((((num79 + num85) + num91) + num103) + num11) + num35) + num59;
            double num = ((((num77 + num83) + num89) + num95) + num3) + num27;
            numArray7[3] = (num127 + num125) + num;
            num120 = 100.0 / numArray7[3];
            num127 *= num120;
            num125 *= num120;
            num *= num120;
            double num123 = P_LV / num127;
            double num122 = N_LV / num125;
            double num121 = A_LV / num;
            double num76 = num75 * num123;
            double num82 = num81 * num123;
            double num88 = num87 * num123;
            double num94 = num93 * num123;
            double num112 = num111 * num123;
            double num20 = num19 * num123;
            double num44 = num43 * num123;
            double num68 = num67 * num123;
            double num80 = num79 * num122;
            double num86 = num85 * num122;
            double num92 = num91 * num122;
            double num104 = num103 * num122;
            double num12 = num11 * num122;
            double num36 = num35 * num122;
            double num60 = num59 * num122;
            double num78 = num77 * num121;
            double num84 = num83 * num121;
            double num90 = num89 * num121;
            double num96 = num95 * num121;
            double num4 = num3 * num121;
            double num28 = num27 * num121;
            double num128 = ((((((num76 + num82) + num88) + num94) + num112) + num20) + num44) + num68;
            double num126 = (((((num80 + num86) + num92) + num104) + num12) + num36) + num60;
            double num2 = ((((num78 + num84) + num90) + num96) + num4) + num28;
            numArray7[4] = (num128 + num126) + num2;
            double num141 = 0.4 * num82;
            double num146 = 0.6 * num82;
            double num137 = 0.45 * num80;
            double num144 = 0.55 * num80;
            double num142 = 0.47 * num88;
            double num147 = 0.53 * num88;
            double num143 = 0.54 * num86;
            double num133 = 0.46 * num86;
            double num140 = 0.12 * num92;
            double num138 = 0.52 * num92;
            double num149 = 0.02 * num92;
            double num134 = 0.34 * num92;
            double num139 = 0.19 * num90;
            double num150 = 0.11 * num90;
            double num145 = 0.46 * num90;
            double num148 = 0.24 * num90;
            numArray7[5] = 0.0;
            numArray7[5] = ((((((((((((((((((((((((((((((num76 * numArray6[8]) + (num141 * numArray6[9])) + (num146 * numArray6[10])) + (num137 * numArray6[11])) + (num144 * numArray6[12])) + (num78 * numArray6[13])) + (num142 * numArray6[14])) + (num147 * numArray6[15])) + (num143 * numArray6[0x10])) + (num133 * numArray6[0x11])) + (num84 * numArray6[0x12])) + (num94 * numArray6[0x13])) + (num140 * numArray6[20])) + (num138 * numArray6[0x15])) + (num149 * numArray6[0x16])) + (num134 * numArray6[0x17])) + (num139 * numArray6[0x18])) + (num150 * numArray6[0x19])) + (num145 * numArray6[0x1a])) + (num148 * numArray6[0x1b])) + (num112 * numArray6[0x20])) + (num104 * numArray6[0x21])) + (num96 * numArray6[0x22])) + (num20 * numArray6[0x23])) + (num12 * numArray6[0x24])) + (num4 * numArray6[0x25])) + (num44 * numArray6[0x26])) + (num36 * numArray6[0x27])) + (num28 * numArray6[40])) + (num68 * numArray6[0x29])) + (num60 * numArray6[0x2a]);
            numArray7[5] *= 0.01;
            numArray8[1] = 0.0;
            numArray8[1] = ((((((((((((num141 + num142) + num133) + num94) + num138) + num134) + num112) + num104) + num20) + num12) + num44) + num36) + num68) + num60;
            numArray8[2] = 100.0 / numArray8[1];
            numArray7[6] = ((((((((((((((num141 * numArray8[2]) * numArray6[9]) + ((num142 * numArray8[2]) * numArray6[14])) + ((num133 * numArray8[2]) * numArray6[0x11])) + ((num94 * numArray8[2]) * numArray6[0x13])) + ((num138 * numArray8[2]) * numArray6[0x15])) + ((num134 * numArray8[2]) * numArray6[0x17])) + ((num112 * numArray8[2]) * numArray6[0x20])) + ((num104 * numArray8[2]) * numArray6[0x21])) + ((num20 * numArray8[2]) * numArray6[0x23])) + ((num12 * numArray8[2]) * numArray6[0x24])) + ((num44 * numArray8[2]) * numArray6[0x26])) + ((num36 * numArray8[2]) * numArray6[0x27])) + ((num68 * numArray8[2]) * numArray6[0x29])) + ((num60 * numArray8[2]) * numArray6[0x2a]);
            numArray7[6] *= 0.01;
            numArray4[8] = (((numArray8[1] * 0.01) * numArray7[6]) + SGFeed) - numArray7[5];
            numArray4[9] = numArray4[8] / (numArray8[1] * 0.01);
            numArray4[10] = numArray4[9] / numArray7[6];
            numArray6[9] = numArray4[10] * numArray6[9];
            numArray6[14] = numArray4[10] * numArray6[14];
            numArray6[0x11] = numArray4[10] * numArray6[0x11];
            numArray6[0x13] = numArray4[10] * numArray6[0x13];
            numArray6[0x15] = numArray4[10] * numArray6[0x15];
            numArray6[0x17] = numArray4[10] * numArray6[0x17];
            numArray6[0x20] = numArray4[10] * numArray6[0x20];
            numArray6[0x21] = numArray4[10] * numArray6[0x21];
            numArray6[0x23] = numArray4[10] * numArray6[0x23];
            numArray6[0x24] = numArray4[10] * numArray6[0x24];
            numArray6[0x26] = numArray4[10] * numArray6[0x26];
            numArray6[0x27] = numArray4[10] * numArray6[0x27];
            numArray6[0x29] = numArray4[10] * numArray6[0x29];
            numArray6[0x2a] = numArray4[10] * numArray6[0x2a];
            numArray7[7] = 0.0;
            numArray7[7] = (((((((((num112 + num104) + num96) + num20) + num12) + num4) + num44) + num36) + num28) + num68) + num60;
            double num131 = ((num112 + num20) + num44) + num68;
            double num130 = ((num104 + num12) + num36) + num60;
            double num129 = (num96 + num4) + num28;
            numArray8[3] = 100.0 / numArray7[7];
            double num113 = numArray8[3] * num112;
            double num105 = numArray8[3] * num104;
            double num97 = numArray8[3] * num96;
            double num21 = numArray8[3] * num20;
            double num13 = numArray8[3] * num12;
            double num5 = numArray8[3] * num4;
            double num45 = numArray8[3] * num44;
            double num37 = numArray8[3] * num36;
            double num29 = numArray8[3] * num28;
            double num69 = numArray8[3] * num68;
            double num61 = numArray8[3] * num60;
            numArray7[8] = 0.0;
            numArray7[8] = ((((((((((num113 * numArray6[0x20]) + (num105 * numArray6[0x21])) + (num97 * numArray6[0x22])) + (num21 * numArray6[0x23])) + (num13 * numArray6[0x24])) + (num5 * numArray6[0x25])) + (num45 * numArray6[0x26])) + (num37 * numArray6[0x27])) + (num29 * numArray6[40])) + (num69 * numArray6[0x29])) + (num61 * numArray6[0x2a]);
            numArray7[8] *= 0.01;
            double num114 = (num113 * numArray6[0x20]) / numArray7[8];
            double num106 = (num105 * numArray6[0x21]) / numArray7[8];
            double num98 = (num97 * numArray6[0x22]) / numArray7[8];
            double num22 = (num21 * numArray6[0x23]) / numArray7[8];
            double num14 = (num13 * numArray6[0x24]) / numArray7[8];
            double num6 = (num5 * numArray6[0x25]) / numArray7[8];
            double num46 = (num45 * numArray6[0x26]) / numArray7[8];
            double num38 = (num37 * numArray6[0x27]) / numArray7[8];
            double num30 = (num29 * numArray6[40]) / numArray7[8];
            double num70 = (num69 * numArray6[0x29]) / numArray7[8];
            double num62 = (num61 * numArray6[0x2a]) / numArray7[8];
            double num115 = num114 / numArray5[0x20];
            double num107 = num106 / numArray5[0x21];
            double num99 = num98 / numArray5[0x22];
            double num23 = num22 / numArray5[0x23];
            double num15 = num14 / numArray5[0x24];
            double num7 = num6 / numArray5[0x25];
            double num47 = num46 / numArray5[0x26];
            double num39 = num38 / numArray5[0x27];
            double num31 = num30 / numArray5[40];
            double num71 = num70 / numArray5[0x29];
            double num63 = num62 / numArray5[0x2a];
            numArray7[9] = 0.0;
            numArray7[9] = (((((((((num115 + num107) + num99) + num23) + num15) + num7) + num47) + num39) + num31) + num71) + num63;
            numArray8[4] = 100.0 / numArray7[9];
            numArray8[5] = numArray7[9] * 0.01;
            double num116 = num115 / numArray8[5];
            double num108 = num107 / numArray8[5];
            double num100 = num99 / numArray8[5];
            double num24 = num23 / numArray8[5];
            double num16 = num15 / numArray8[5];
            double num8 = num7 / numArray8[5];
            double num48 = num47 / numArray8[5];
            double num40 = num39 / numArray8[5];
            double num32 = num31 / numArray8[5];
            double num72 = num71 / numArray8[5];
            double num64 = num63 / numArray8[5];
            numArray7[10] = 0.0;
            numArray7[10] = ((num113 + num21) + num45) + num69;
            if (numArray7[10] != 0.0)
            {
                numArray8[6] = 100.0 / numArray7[10];
                double num117 = numArray8[6] * num113;
                double num25 = numArray8[6] * num21;
                double num49 = numArray8[6] * num45;
                double num73 = numArray8[6] * num69;
                numArray7[11] = 0.0;
                numArray7[11] = (((num117 * numArray6[0x20]) + (num25 * numArray6[0x23])) + (num49 * numArray6[0x26])) + (num73 * numArray6[0x29]);
                numArray7[11] *= 0.01;
            }
            else
            {
                numArray7[11] = numArray6[0x20];
            }
            numArray7[12] = 0.0;
            numArray7[12] = ((num116 + num24) + num48) + num72;
            if (numArray7[12] != 0.0)
            {
                numArray8[7] = 100.0 / numArray7[12];
                double num118 = numArray8[7] * num116;
                double num26 = numArray8[7] * num24;
                double num50 = numArray8[7] * num48;
                double num74 = numArray8[7] * num72;
                numArray7[13] = 0.0;
                numArray7[13] = (((num118 * numArray5[0x20]) + (num26 * numArray5[0x23])) + (num50 * numArray5[0x26])) + (num74 * numArray5[0x29]);
                numArray7[13] *= 0.01;
            }
            else
            {
                numArray7[13] = numArray5[0x20];
            }
            numArray7[14] = 0.0;
            numArray7[14] = ((num105 + num13) + num37) + num61;
            if (numArray7[14] != 0.0)
            {
                numArray8[8] = 100.0 / numArray7[14];
                double num109 = numArray8[8] * num105;
                double num17 = numArray8[8] * num13;
                double num41 = numArray8[8] * num37;
                double num65 = numArray8[8] * num61;
                numArray7[15] = 0.0;
                numArray7[15] = (((num109 * numArray6[0x21]) + (num17 * numArray6[0x24])) + (num41 * numArray6[0x27])) + (num65 * numArray6[0x2a]);
                numArray7[15] *= 0.01;
            }
            else
            {
                numArray7[15] = numArray6[0x21];
            }
            numArray7[0x10] = 0.0;
            numArray7[0x10] = ((num108 + num16) + num40) + num64;
            if (numArray7[0x10] != 0.0)
            {
                numArray8[9] = 100.0 / numArray7[0x10];
                double num110 = numArray8[9] * num108;
                double num18 = numArray8[9] * num16;
                double num42 = numArray8[9] * num40;
                double num66 = numArray8[9] * num64;
                numArray7[0x11] = 0.0;
                numArray7[0x11] = (((num110 * numArray5[0x21]) + (num18 * numArray5[0x24])) + (num42 * numArray5[0x27])) + (num66 * numArray5[0x2a]);
                numArray7[0x11] *= 0.01;
            }
            else
            {
                numArray7[0x11] = numArray5[0x21];
            }
            numArray7[0x12] = 0.0;
            numArray7[0x12] = (num97 + num5) + num29;
            if (numArray7[0x12] != 0.0)
            {
                numArray8[10] = 100.0 / numArray7[0x12];
                double num101 = numArray8[10] * num97;
                double num9 = numArray8[10] * num5;
                double num33 = numArray8[10] * num29;
                numArray7[0x13] = 0.0;
                numArray7[0x13] = ((num101 * numArray6[0x22]) + (num9 * numArray6[0x25])) + (num33 * numArray6[40]);
                numArray7[0x13] *= 0.01;
            }
            else
            {
                numArray7[0x13] = numArray6[0x22];
            }
            numArray7[20] = 0.0;
            numArray7[20] = (num100 + num8) + num32;
            if (numArray7[20] != 0.0)
            {
                numArray8[11] = 100.0 / numArray7[20];
                double num102 = numArray8[11] * num100;
                double num10 = numArray8[11] * num8;
                double num34 = numArray8[11] * num32;
                numArray7[0x15] = 0.0;
                numArray7[0x15] = ((num102 * numArray5[0x22]) + (num10 * numArray5[0x25])) + (num34 * numArray5[40]);
                numArray7[0x15] *= 0.01;
            }
            else
            {
                numArray7[0x15] = numArray5[0x22];
            }
            double num119 = ((numArray8[4] + ((numArray7[20] * 0.01) * 6.028)) - ((numArray7[12] * 0.01) * 2.016)) / 14.027;
            double num135 = numArray7[15] + 0.0035;
            double num136 = numArray7[15] - 0.0035;
            numArray6[0x1c] = numArray7[11];
            numArray6[0x1d] = num135;
            numArray6[30] = num136;
            numArray6[0x1f] = numArray7[0x13];
            numArray5[0x1c] = numArray7[13];
            numArray5[0x1d] = numArray7[0x11];
            numArray5[30] = numArray7[0x11];
            numArray5[0x1f] = numArray7[0x15];
            a = 1.0;
            do
            {
                PctVol[(int)Math.Round(a)] = 0.0;
                a++;
                num151 = 8.0;
            }
            while (a <= num151);
            PctVol[8] = num76;
            PctVol[9] = num141;
            PctVol[10] = num146;
            PctVol[11] = num137;
            PctVol[12] = num144;
            PctVol[13] = num78;
            PctVol[14] = num142;
            PctVol[15] = num147;
            PctVol[0x10] = num143;
            PctVol[0x11] = num133;
            PctVol[0x12] = num84;
            PctVol[0x13] = num94;
            PctVol[20] = num140;
            PctVol[0x15] = num138;
            PctVol[0x16] = num149;
            PctVol[0x17] = num134;
            PctVol[0x18] = num139;
            PctVol[0x19] = num150;
            PctVol[0x1a] = num145;
            PctVol[0x1b] = num148;
            PctVol[0x1c] = num131;
            PctVol[0x1d] = num130 * 0.5;
            PctVol[30] = num130 * 0.5;
            PctVol[0x1f] = num129;
        }

        private void SolveCase()
        {
            double FracLiq = 0;
            int num6;
            int num7;
            bool LastGuess = false;
            double SumError;
            double TempMON = 0;
            double TempRON = 0;
            int num27;
            double[] numArray12 = new double[5];
            double[] numArray4 = new double[0x20];
            double[,] numArray2 = new double[0x20, 5];
            double[] F_Eff = new double[0x20];
            double[] F_Ref = new double[0x20];
            double[] F_Vap = new double[0x20];
            double[] numArray3 = new double[0x20];
            double[] numArray5 = new double[0x20];
            double[] numArray6 = new double[0x20];
            double[] numArray7 = new double[0x20];
            double[] numArray8 = new double[0x20];
            double[] numArray9 = new double[0x20];
            double num11 = 0.025;
            double OctHigh = OctSpec + num11;
            double OctLow = OctSpec - num11;
            double ChangeBy = 8.0;
            bool flag = false;
            bool NotFirstGuess = false;
            int num8 = 200;
            double Tolerance = 0.02;
            int num2 = 1;
            EstRecycle(numArray7);
            Reactors(numArray7, numArray2, F_Eff, numArray12);
            Flash(F_Eff, TSep, PSep, FracLiq, F_Ref, F_Vap);
            AssignRecycle(F_Vap, numArray3, numArray5);
            int index = 1;
            do
            {
                numArray9[index] = numArray7[index];
                numArray7[index] = numArray5[index];
                numArray4[index] = numArray5[index];
                index++;
                num27 = 0x1f;
            }
            while (index <= num27);
            int num20 = num8;
            for (num6 = 1; num6 <= num20; num6++)
            {
                num2++;
                Reactors(numArray7, numArray2, F_Eff, numArray12);
                Flash(F_Eff, TSep, PSep, FracLiq, F_Ref, F_Vap);
                AssignRecycle(F_Vap, numArray3, numArray5);
                index = 2;
                do
                {
                    double num3 = ((numArray5[index] + numArray9[index]) - numArray4[index]) - numArray7[index];
                    if (num3 > 1E-07)
                    {
                        numArray8[index] = ((numArray5[index] * numArray9[index]) - (numArray4[index] * numArray7[index])) / num3;
                    }
                    else
                    {
                        numArray8[index] = numArray7[index];
                    }
                    numArray9[index] = numArray7[index];
                    numArray7[index] = numArray8[index];
                    numArray4[index] = numArray5[index];
                    index++;
                    num27 = 0x1f;
                }
                while (index <= num27);
                SumError = 0.0;
                index = 1;
                do
                {
                    SumError += Math.Abs((double)(numArray7[index] - numArray9[index]));
                    index++;
                    num27 = 0x1f;
                }
                while (index <= num27);
                if (SumError < Tolerance)
                {
                    double OldTemp1 = 0;
                    if (!SpecOct)
                    {
                        goto Label_0453;
                    }
                    CalcOctane(F_Ref, TempRON, TempMON);
                    if (Math.Abs((double)(TempRON - OctSpec)) <= Tolerance)
                    {
                        NotFirstGuess = false;
                        goto Label_0453;
                    }
                    if (NotFirstGuess)
                    {
                        if (((TempRON >= OctHigh) & !LastGuess) | ((TempRON <= OctLow) & LastGuess))
                        {
                            flag = true;
                        }
                        if (flag && (ChangeBy > 0.5))
                        {
                            ChangeBy /= 2.0;
                        }
                    }
                    else
                    {
                        NotFirstGuess = true;
                    }
                    double OldTemp2 = OldTemp1;
                    OldTemp1 = ReactorT_In[1];
                    if (TempRON >= OctHigh)
                    {
                        LastGuess = true;

                        num7 = 1;
                        while (num7 <= NumReactor)
                        {
                            ReactorT_In[num7] -= ChangeBy;
                            num7++;
                        }
                        if (ReactorT_In[1] == OldTemp2)
                        {
                            ChangeBy /= 2.0;

                            num7 = 1;
                            while (num7 <= NumReactor)
                            {
                                ReactorT_In[num7] += ChangeBy;
                                num7++;
                            }
                        }
                    }
                    else if (TempRON <= OctLow)
                    {
                        LastGuess = false;

                        num7 = 1;
                        while (num7 <= NumReactor)
                        {
                            ReactorT_In[num7] += ChangeBy;
                            num7++;
                        }
                        if (ReactorT_In[1] == OldTemp2)
                        {
                            ChangeBy /= 2.0;

                            num7 = 1;
                            while (num7 <= NumReactor)
                            {
                                ReactorT_In[num7] -= ChangeBy;
                                num7++;
                            }
                        }
                    }
                }
            }
        Label_0453:
            num11 *= 2.0;
            num6 = 1;
        Label_0464:
            num2++;
            SumError = 0.0;
            index = 1;
            do
            {
                SumError += Math.Abs((double)(numArray7[index] - numArray4[index]));
                index++;
                num27 = 0x1f;
            }
            while (index <= num27);
            if (SumError < Tolerance)
            {
                if (SpecOct)
                {
                    CalcOctane(F_Ref, TempRON, TempMON);
                    if (Math.Abs((double)(TempRON - OctSpec)) <= Tolerance)
                    {
                        NotFirstGuess = true;
                        goto Label_0651;
                    }
                    if (NotFirstGuess)
                    {
                        if ((((TempRON >= OctHigh) & !LastGuess) | ((TempRON <= OctLow) & LastGuess)) && (ChangeBy > 0.5))
                        {
                            ChangeBy /= 2.0;
                        }
                    }
                    else
                    {
                        NotFirstGuess = true;
                    }
                    if (TempRON >= OctHigh)
                    {
                        LastGuess = true;
                        for (num7 = 1; num7 <= NumReactor; num7++)
                        {
                            ReactorT_In[num7] -= ChangeBy;
                        }
                    }
                    else if (TempRON <= OctLow)
                    {
                        LastGuess = false;

                        for (num7 = 1; num7 <= NumReactor; num7++)
                        {
                            ReactorT_In[num7] += ChangeBy;
                        }
                    }
                    goto Label_0602;
                }
                NotFirstGuess = true;
                goto Label_0651;
            }
            index = 1;
            do
            {
                numArray7[index] = numArray4[index];
                index++;
                num27 = 0x1f;
            }
            while (index <= num27);
        Label_0602:
            Reactors(numArray7, numArray2, F_Eff, numArray12);
            Flash(F_Eff, TSep, PSep, FracLiq, F_Ref, F_Vap);
            AssignRecycle(F_Vap, numArray3, numArray4);
            num6++;
            num27 = 400;
            if (num6 <= num27)
            {
                goto Label_0464;
            }
        Label_0651:
            if (NotFirstGuess)
            {
                Reactors(numArray4, numArray2, F_Eff, numArray12);
                Flash(F_Eff, TSep, PSep, FracLiq, F_Ref, F_Vap);
                AssignRecycle(F_Vap, numArray3, numArray5);
            }
            else
            {
                string str;
                if (SpecOct)
                {
                    str = "Octane specification failed to converge.  The last iteration used a first reactor inlet Temperature  of " + RToF(ReactorT_In[1]).ToString() + "F.  Try starting with an inlet Temperature  closer to that value.";
                }
                else
                {
                    str = "Recycle calculation failed to converge. Simulation abandoned.";
                }
                EndWell(str);
            }
            DisplayResults(numArray4, numArray2, F_Eff, F_Ref, numArray3, numArray12);
            Reformate = F_Ref;
            offgas = numArray3;
        }
    }
}