using System;
using System.Windows.Forms;

namespace Units
{
    class IsomMaths
    {
        int NumRxn = 28;
        int NumComp = 30;
        int NumReactors;

        const double Rgas = 8.314;
        double[] ReactorP = new double[4];
        double[] MetalAct = new double[4];
        double[] AcidAct = new double[4];
        double[] ReactorT = new double[4];
        double[] AmtCat = new double[4];
        double CHDehydrogTerm, CPDehydrogTerm, IsomTerm, CyclTerm, PCrackTerm, DealkTerm, NCrackTerm;
        double[] K = new double[80];
        double[] Ea = new double[80], keq = new double[80], Eaeq = new double[80], Factor = new double[80];

        short[] a = new short[80], b = new short[80], C = new short[80], D = new short[80];
        double[] H2Afact = new double[80], H2Bfact = new double[80];

        double[] Concs = new double[32];
        const double DHCHFact = 1;
        const double DHCPFact = 1;
        const double ISOMFact = 1, OPENFact = 1, PHCFact = 1, HDAFact = 1, NHCFact = 1;
        double NaphMolFeed;
        const int NumDepVar = 31;

        double[][] CpCoeft = new double[5][];    // 4 coefficients used in equation
        double[] StdHeatform = new double[31];
        int resultno;
        double CatDensity, MolH2Recy;
        double[] StdHeatForm = new double[31];
        const double FeedRate = 1000; //    // kg/hr
        double H2_HC_Ratio = 0;
        double LHSV, MassFeedRate;
        int activecol;
        double[] Pcrit = new double[31], tcrit = new double[31], NBP = new double[31], w = new double[31], MolVol = new double[31], Sol = new double[31];
        double Psep, Tsep;
        double[] MolFlowRates = new double[31], MolFeedRates = new double[31];
        bool SpecOct;
        double[] MW = new double[31], SG = new double[31], RON = new double[31], MON = new double[31], RVP = new double[31];
        double Dwtot, HeatLossFactor, tbase;
        bool Rigourous, IsIsothermal = false, FailedFlag;
        double[,] Stoich;

        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        Excel.Workbook xlIsomResultsBook;
        Excel.Worksheet xlIsomResultsSheet;
        Excel.Range names;

        public IsomMaths()
        {
            Stoich = new double[NumComp + 1, NumRxn + 1];
        }

        void Constants()
        {
            resultno = 0;
            MolVol = new double[] { 31.00, 52.00, 68.00, 84.00, 105.50, 101.40, 117.40, 116.10, 131.60, 108.70, 113.10, 89.40, 94.70, 131.45, 131.45, 131.45, 131.45, 147.50, 147.10, 147.10, 147.10, 147.10, 147.10, 147.10, 147.10, 147.10, 147.10, 128.30, 128.30, 106.80, 140.22 };

            // Solubility
            Sol = new double[] { 3.25, 5.68, 6.05, 6.4, 6.73, 6.73, 7.021, 7.021, 7.1561, 8.196, 7.849, 9.158, 7.1561, 7.1561, 7.266, 8.196, 7.849, 7.43, 7.2872, 7.2872, 7.2872, 7.2872, 7.2872, 7.2872, 7.2872, 7.2872, 7.2872, 7.826, 7.826, 8.915, 8.5676 };

            // Normal boiling point s in deg C was R
            //NBP(n) = ThisWorkbook.Worksheets("ComponentData").Range("BP").Offset(n, 0);

            // Critical Temperature s in deg K was R
            tcrit = new double[] { -239.71, -82.45, 32.28, 96.75, 134.95, 152.05, 187.25, 196.45, 234.75, 280.05, 259.55, 288.95, 238.45, 224.35, 231.30, 231.30, 226.83, 267.01, 247.35, 246.64, 258.02, 263.25, 264.20, 257.22, 262.10, 267.49, 313.89, 298.95, 296.37, 318.65, 357.22 };

            for (int n = 0; n < NumComp; n++)
            {
                tcrit[n] += 273.15;
            }

            // Critical Pressure s in Bar was psia  // kPa
            Pcrit = new double[] { 13.1550, 46.4068, 48.8385, 42.5666, 36.4762, 37.9662, 33.3359, 33.7512, 30.3162, 40.5300, 37.8955, 49.2439, 45.0895, 30.1036, 31.2384, 38.8062, 31.2687, 27.3678, 27.7326, 27.3678, 29.5362, 29.4551, 29.0802, 27.3362, 28.1379, 28.9080, 29.3853, 34.7537, 33.9762, 41.0004, 37.3281 };

            // Acentric factor
            w = new double[] { -0.1201, 0.0115, 0.0986, 0.1524, 0.1848, 0.2010, 0.2222, 0.2539, 0.3007, 0.2133, 0.2389, 0.2150, 0.1920, 0.2791, 0.2750, 0.2319, 0.2470, 0.3498, 0.3000, 0.3070, 0.2600, 0.2870, 0.3050, 0.3400, 0.3270, 0.3140, 0.2695, 0.2330, 0.2826, 0.2596, 0.3023 };

            CpCoeft[0] = new double[] { -4.97E+01, -1.30E+01, -1.77E+00, 3.95E+01, 3.09E+01, 6.77E+01, 6.32E+01, 6.43E+01, 7.45E+01, 4.56E-09, 1.27E+02, 8.45E+01, 1.56E-08, 1.11E+02, 8.38E+01, 0.00E+00, 0.00E+00, 7.14E+01, 7.77E+01, 7.59E+01, 8.60E+01, 8.27E+01, 8.00E+01, 4.77E+01, 1.96E-08, 7.86E+01, 1.41E+02, 1.08E+02, 6.49E+01, 7.42E+01, 7.42E+01, };
            CpCoeft[1] = new double[] { 1.38E+01, 2.36E+00, 1.14E+00, 3.95E-01, 1.53E-01, 8.54E-03, -1.17E-02, -1.32E-01, -9.67E-02, -6.48E-01, -6.84E-01, -5.13E-01, -7.65E-01, -6.06E-01, -1.70E-01, -1.93E-01, -1.70E-01, -9.69E-02, 2.15E-01, 2.23E-01, 1.55E-01, 1.56E-01, 1.58E-01, -1.25E-01, -5.61E-02, 1.55E-01, -3.36E-01, -7.05E-01, -6.77E-01, -4.23E-01, -4.23E-01 };
            CpCoeft[2] = new double[] { 3.00E-04, -2.13E-03, -3.24E-04, 2.11E-03, 2.63E-03, 3.28E-03, 3.32E-03, 3.54E-03, 3.48E-03, 3.63E-03, 4.01E-03, 3.25E-03, 3.87E-03, 4.92E-03, 3.68E-03, 3.65E-03, 3.57E-03, 3.47E-03, 2.82E-03, 2.82E-03, 2.83E-03, 2.83E-03, 2.83E-03, 3.60E-03, 3.38E-03, 2.83E-03, 3.32E-03, 4.10E-03, 4.22E-03, 3.18E-03, 3.18E-03 };
            CpCoeft[3] = new double[] { 3.46E-07, 5.66E-06, 4.24E-06, 3.96E-07, 7.27E-08, -1.11E-06, -1.17E-06, -1.33E-06, -1.32E-06, -9.99E-07, -1.68E-06, -1.54E-06, -1.44E-06, -3.02E-06, -1.56E-06, -2.42E-06, -2.35E-06, -1.33E-06, -6.68E-07, -6.68E-07, -6.74E-07, -6.75E-07, -6.76E-07, -1.28E-06, -1.21E-06, -6.77E-07, -8.87E-07, -1.53E-06, -2.12E-06, -1.44E-06, -1.44E-06 };
            CpCoeft[4] = new double[] { -9.71E-11, -3.72E-09, -3.39E-09, -6.67E-10, -7.28E-10, 1.77E-10, 2.00E-10, 2.51E-10, 2.52E-10, 3.92E-11, 3.58E-10, 3.65E-10, 2.31E-10, 1.07E-09, 3.54E-10, 6.44E-10, 6.41E-10, 2.56E-10, 0.00E+00, 0.00E+00, 0.00E+00, 0.00E+00, 0.00E+00, 9.86E-11, 1.85E-10, 0.00E+00, -8.93E-11, 1.83E-10, 7.00E-10, 3.27E-10, 3.27E-10 };

            // Standard state (25 Temperature ) heat of formation
            // for the 30 Components,kJ/Kgmole

            StdHeatForm = new double[] { 0.00E+00, -7.49E+04, -8.47E+04, -1.04E+05, -1.35E+05, -1.26E+05, -1.55E+05, -1.46E+05, -1.67E+05, -1.23E+05, -1.07E+05, 8.30E+04, -7.73E+04, -1.74E+05, -1.72E+05, -1.86E+05, -1.78E+05, -1.88E+05, -2.06E+05, -2.02E+05, -2.05E+05, -2.02E+05, -1.99E+05, -1.95E+05, -1.92E+05, -1.90E+05, -1.38E+05, -1.55E+05, -1.27E+05, 5.00E+04, 5.00E+04 };
            SG = new double[] { 0.0699, 0.2994, 0.3557, 0.5067, 0.5631, 0.5844, 0.6247, 0.6297, 0.6627, 0.664, 0.7834, 0.7536, 0.8844, 0.6565, 0.6667, 0.6526, 0.6652, 0.6868, 0.677, 0.676, 0.6933, 0.6963, 0.698, 0.6815, 0.6902, 0.7012, 0.8848, 0.7724, 0.7696, 0.8704, 0.867 };
            MW = new double[] { 2.016000032, 16.04290009, 30.06990051, 44.09700012, 58.12400055, 58.12400055, 72.15100098, 72.15100098, 86.17790222, 84.16000366, 84.16190338, 78.11000061, 70.13500214, 86.17790222, 86.17790222, 86.17790222, 86.17790222, 100.2050018, 100.2050018, 100.2050018, 100.2050018, 100.2050018, 100.2050018, 100.2050018, 100.2050018, 100.2050018, 98.18900299, 98.18900299, 98.18900299, 92.14080048, 112.21 };
            RON = new double[] { 0, 0, 0, 0, 100.2, 95.0, 92.7, 61.7, 31.0, 84.0, 89.3, 120.0, 101.6, 73.4, 74.5, 91.8, 104.3, 0, 92.8, 83.1, 112.1, 80.8, 91.7, 42.4, 52.0, 65.0, 92.3, 73.8, 67.2, 110.0, 110.0 };
            MON = new double[] { 97.0, 89.1, 104.9, 63.2, 26.0, 77.6, 81.0, 110.0, 84.9, 73.5, 73.3, 93.4, 94.2, 0, 95.6, 83.8, 101.3, 86.6, 88.5, 46.4, 55.0, 69.3, 89.3, 73.8, 61.2, 109.1, 109.1 };
            RVP = new double[] { 0, 0, 0, 90, 72.2, 51.6, 20.4, 15.6, 7, 5, 2.3, 4.5, 3.2, 2.3, 1.6, 1.6, 2, 1, 0.7, 0.5, 0.7, 0.7, 0.7, 0.4, 0.3, 0.3, 0.3, 0.3, 0.2, 0.3, 0.1 };
        }

        public void Solve()
        {
            Rigourous = true;
            tbase = 273.15;
            HeatLossFactor = 0.6;
            SpecOct = false;
            FailedFlag = false;
            InitReactions(3);
        }

        void InitReactions(int ac)
        {
            String Filename = "C:\\Documents and Settings\\andersm\\My Documents\\Visual Studio 2008\\Projects\\AWOSI Versions\\FreeSim\\ISOM_MODEL.XLS";
            object misValue = System.Reflection.Missing.Value;
            object missing = Type.Missing;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(Filename, 0, false, 5, "", "", true, Microsoft.Office.int erop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlIsomResultsBook = xlApp.Workbooks.Add(missing);
            xlIsomResultsSheet = (Excel.Worksheet)xlIsomResultsBook.Worksheets[1];

            NumRxn = GetExcelint Value("NoReactions", "ReactionStoich", 0, 0);
            NumComp = GetExcelint Value("NoComps", "ReactionStoich", 0, 0);
            double RX1_PCT, RX2_PCT, RX3_PCT, RX4_PCT, Flowrate = 0, LiqVol, TotCat;

            double H2Moles, fracLiq = 0;
            double[] RecycleGas = new double[NumComp];
            double[] eff = new double[31], F_Ref = new double[31], F_Vap = new double[31], rxtout = new double[4];

            // int  n, m;

            xlWorkBook.Activate();
            xlApp.Run("InitReactions.InitReactions", 7, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

            activecol = ac;
            Constants();

            xlApp.Visible = true;

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(6);
            names = xlWorkSheet.get_Range("NoReactions", "NoReactions");

            //NumRxn = Convert.Toint 16(xlWorkSheet.get_Range("NoReactions", "NoReactions").BaseValue2);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item("Input");

            RX1_PCT = GetExcelValue("PCTR1", "Input", 0, activecol - 2);
            RX2_PCT = GetExcelValue("PCTR2", "Input", 0, activecol - 2);
            RX3_PCT = 0;
            RX4_PCT = 0;

            Psep = 10;
            Tsep = 290;

            ReactorP[0] = GetExcelValue("Pr", "Input", 0, activecol - 2);
            ReactorP[1] = GetExcelValue("Pr", "Input", 0, activecol - 2);

            IsIsothermal = false; //ThisWorkbook.Worksheets["Input"].Range["Isothermal"].Offset[0, activecol - 2].BaseValue = "YES";

            MetalAct[0] = 1;
            MetalAct[1] = 1;
            MetalAct[2] = 1;

            AcidAct[0] = 1;
            AcidAct[1] = 1;
            AcidAct[2] = 1;

            ReactorT[0] = GetExcelValue("Rx1T", "Input", 0, activecol - 2); //385.1513; //[ThisWorkbook.Worksheets["Input"].Range["Rx1T"].Offset[0, activecol - 2].BaseValue + 273.15];// // C -> K
            ReactorT[1] = GetExcelValue("Rx2T", "Input", 0, activecol - 2); //400.6147; //(ThisWorkbook.Worksheets("Input").Range("Rx2T").Offset(0, activecol - 2).BaseValue + 273.15);

            LHSV = GetExcelValue("LHSV", "Input", 0, activecol - 2); //2.7011;// ThisWorkbook.Worksheets("Input").Range("LHSV").Offset(0, activecol - 2).BaseValue;
            H2_HC_Ratio = GetExcelValue("H2_HC", "Input", 0, activecol - 2); //0; //ThisWorkbook.Worksheets("Input").Range("H2_HC").Offset(0, activecol - 2).BaseValue;
            MassFeedRate = GetExcelValue("FeedRate", "Input", 0, activecol - 2); //1000; // ThisWorkbook.Worksheets("Input").Range("FeedRate").Offset(0, activecol - 2).BaseValue;
            CatDensity = GetExcelValue("CatDensity", "Input", 0, activecol - 2); //881;//ThisWorkbook.Worksheets("Input").Range("CatDensity").Offset(0, activecol - 2).BaseValue;
            NumReactors = GetExcelint Value("NoReact", "Input", 0, activecol - 2); //2; //ThisWorkbook.Worksheets("Input").Range("NoReact").Offset(0, activecol - 2).BaseValue;

            LiqVol = GetExcelValue("LiqVolFlow", "Input", 0, activecol - 2); //1.563; // ThisWorkbook.Worksheets("Input").Range("LiqVolFlow").Offset(0, activecol - 2).BaseValue;
            // //TotCat = LiqVol / LHSV * CatDensity * 2.2046226218; // lb
            TotCat = LiqVol / LHSV * CatDensity;  // kg

            AmtCat[0] = TotCat * RX1_PCT;
            AmtCat[1] = TotCat * RX2_PCT;
            AmtCat[2] = TotCat * RX3_PCT;
            AmtCat[3] = TotCat * RX4_PCT;

            // With ThisWorkbook.Worksheets("ReactionStoich")

            var return edK = xlApp.Run("InitReactions.GetK", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edEa = xlApp.Run("InitReactions.GetEa", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edKeq = xlApp.Run("InitReactions.GetKeq", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edEaeq = xlApp.Run("InitReactions.GetEaeq", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edA = xlApp.Run("InitReactions.GetA", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edB = xlApp.Run("InitReactions.GetB", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edC = xlApp.Run("InitReactions.GetC", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edD = xlApp.Run("InitReactions.GetD", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edH2Afact = xlApp.Run("InitReactions.GetH2Afact", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edH2Bfact = xlApp.Run("InitReactions.GetH2Bfact", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edStoich = xlApp.Run("InitReactions.GetStoich", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edCatAmount = xlApp.Run("InitReactions.GetAmtCat", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            var return edFactor = xlApp.Run("InitReactions.GetFactor", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

            for (int n = 0; n < NumRxn; n++)
            {
                K[n] = ((double[])return edK)[n +1];
            Ea[n] = ((double[])return edEa)[n +1];
            keq[n] = ((double[])return edKeq)[n +1];
            Eaeq[n] = ((double[])return edEaeq)[n +1];
            Factor[n] = ((double[])return edFactor)[n +1];
            a[n] = ((short[])(return edA))[n +1];
            b[n] = ((short[])return edB)[n +1];
            C[n] = ((short[])return edC)[n +1];
            D[n] = ((short[])return edD)[n +1];
            H2Afact[n] = ((short[])return edH2Afact)[n +1];
            H2Bfact[n] = ((short[])return edH2Bfact)[n +1];
            for (int m = 0; m < NumComp; m++)
            {
                Stoich[m, n] = ((double[,])return edStoich)[m, n] ;
        }
    }

    var return  edMolFeedRates = xlApp.Run("InitReactions.GetMolFeedRates", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

            NaphMolFeed = 0;

            for (int n = 0; n<NumComp; n++)
            {
                MolFeedRates[n] = ((double[])return  edMolFeedRates)[n + 1];
                NaphMolFeed = NaphMolFeed + MolFeedRates[n];
            }

H2Moles = H2_HC_Ratio * NaphMolFeed;
MolH2Recy = H2Moles;

EstRecycle(RecycleGas);

for (int n = 0; n < NumComp; n++)
{
    MolFlowRates[n] = MolFeedRates[n] + RecycleGas[n];
    Flowrate += MolFlowRates[n];
}

Dwtot = 0;
Reactors(MolFlowRates, eff, rxtout);
Flash(eff, Tsep, Psep, fracLiq, F_Ref, F_Vap);

//StreamEnthalpy = molarenthalpy(eff, Tsep, Psep)

// xlIsomResultsSheet.ActivateResults();
xlIsomResultsSheet.Cells.get_Range("C1", missing).Value2 = (rxtout[0] - 273.15).ToString();
xlIsomResultsSheet.Cells.get_Range("C2", missing).Value2 = (rxtout[1] - 273.15).ToString();

// Call Recycle
for (int n = 1; n < NumComp; n++)
{
    xlIsomResultsSheet.Cells.get_Range("C" + (n + 5).ToString(), missing).Value2 = F_Ref[n];
}

xlIsomResultsBook.Close(true, "IsomResults.xls", missing);
xlWorkBook.Close(false, misValue, misValue);
xlApp.Quit();

releaseObject(xlWorkSheet);
releaseObject(xlWorkBook);
releaseObject(xlApp);
        }

        void Recycle()
{
    // Solve the system of reactors including recycle convergence.  Calls Reactors subroutine to
    // calculate the effluent from each reactor and { the Flash subroutine to calculate the
    // recycle.  The process is repeated until the recycle converges using   the Wegstein (1958)
    // method.  { calls the DisplayResults subroutine to print  the final results.

    double[] ReactorT_Out = new double[NumReactors];    // outlet reactor Temperature s in degree R (from degF)
    int MaxIterations;                            // max number of iterations to get recycle to converge
    bool Solved;                                  // Flag if variable solved
    string Message = "";                               // Error message
    double SumError;                              // Sum of difference in recycle Components between two iterations
    double Tolerance;                             // Tolerance (moles) for differences in recycle between iterations
    double[] F_Recy = new double[NumComp];                   // component flow rate (mol/hr) of recycle
    double[] F_Inlet = new double[NumComp];// component flow rate (mol/hr) int o reactors, (feed plus recycle, or effluent from previous reactor)
    double[] F_Eff = new double[NumComp];           // component flow rate (mol/hr) from last reactor
    double[] F_Ref = new double[NumComp];           // component flow rate (mol/hr) in reformate
    double fracLiq = 0;                               // Fraction of liquid in separator
    double[] F_Vap = new double[NumComp];           // Vapor (gas) composition leaving separator
    double[] F_NetGas = new double[NumComp];        // component flow rate (mol/hr) of net gas production
    double[] F_Recynew = new double[NumComp];       // new  calculation of recycle flow rates (mol/hr)
    double[] F_RecyOld = new double[NumComp];       // old calculation of recycle flow rates (mol/hr)
    double[] F_RecyStar = new double[NumComp];      // used in Wegstein convergence method
    double[] F_RecyStarnew = new double[NumComp];   // used in Wegstein convergence method
    double[] F_RecyStarOld = new double[NumComp];   // used in Wegstein convergence method
                                                    // denominator in a Wegstein equation
                                                    // Used for print ing int ermediate recycle results
    int CountIter;                                // Count iterations, defined as number of times "Reactors" is called in solution

    // for octane specification
    double ChangeBy, OctHigh, OctLow, OldTemp2, OldTemp1 = 0, OctTolerance, TempRON = 0, OctSpec = 0, Denom;                      // Degrees to change reactor inlet Temperature s
    bool BeginBinary, LastGuessHigh = false, NotFirstGuess;

    OctTolerance = 0.025;
    OctHigh = OctSpec + OctTolerance;
    OctLow = OctSpec - OctTolerance;
    ChangeBy = 8;
    BeginBinary = false;
    NotFirstGuess = false;

    MaxIterations = 200;
    Tolerance = 0.02;
    Solved = false;
    CountIter = 1;

    // First guess at recycle.  Use Wegstein method for convergence
    // Wegstein Step 1: XStar = X_0
    EstRecycle(F_RecyStar);

    // Wegstein Step 2: Xnew  = f(Xstar)
    Reactors(F_Inlet, F_Eff, ReactorT_Out);
    Flash(F_Eff, Tsep, Psep, fracLiq, F_Ref, F_Vap);
    AssignRecycle(F_Vap, F_NetGas, F_Recynew);

    // Wegstein Step 3: Xstar_old = X_0 = Xstar, Xstar_new  = Xnew ; Step 7: X = Xnew
    for (int I = 1; I < NumComp; I++)
    {
        F_RecyStarOld[I] = F_RecyStar[I];
        F_RecyStar[I] = F_Recynew[I];
        F_Recy[I] = F_Recynew[I];
    }

    // Iterate with Wegstein method to find recycle convergence (steps 2-4-5-6-7, repeating)
    for (int J = 1; J < MaxIterations; J++)
    {
        CountIter = CountIter + 1;
        //Application.StatusBar = "Program running, performing iteration " & CountIter;

        // Wegstein Step 2: Xnew  = f(Xstar)
        Reactors(F_Inlet, F_Eff, ReactorT_Out);
        Flash(F_Eff, Tsep, Psep, fracLiq, F_Ref, F_Vap);
        AssignRecycle(F_Vap, F_NetGas, F_Recynew);
        // The rate of H2 recyle is set (no convergence needed). Start with component 2 (methane).
        for (int I = 2; I < NumComp; I++)
        {
            // Wegstein Step 4
            Denom = F_Recynew[I] + F_RecyStarOld[I] - F_Recy[I] - F_RecyStar[I];
            // Wegstein Step 5. Make sure don//t divide by zero. if dividing by zero, probably already converged.
            if (Denom > 0.0000001)
            {
                F_RecyStarnew[I] = (F_Recynew[I] * F_RecyStarOld[I] - F_Recy[I] * F_RecyStar[I]) / Denom;
            }
            else
            {
                F_RecyStarnew[I] = F_RecyStar[I];
            }
            // Wegstein Step 6
            F_RecyStarOld[I] = F_RecyStar[I];
            F_RecyStar[I] = F_RecyStarnew[I];
            // Wegstein Step 7
            F_Recy[I] = F_Recynew[I];
        }

        // Check that each component is converged within tolerance before exiting.
        // if octane specified, adjust inlet Temperature s as necessary and continue
        SumError = 0;
        for (int I = 1; I < NumComp; I++)
        {
            SumError = SumError + Math.Abs(F_RecyStar[I] - F_RecyStarOld[I]);
        }
        if (SumError < Tolerance)
        {
            if (SpecOct)
            {
                //Call CalcOctane(F_Ref, TempRON, TempMON)
                if (Math.Abs(TempRON - OctSpec) < Tolerance)
                {
                    // Reset for }
                    NotFirstGuess = false;
                    break;
                }
                else
                {
                    if (NotFirstGuess)
                    {
                        if ((TempRON >= OctHigh & LastGuessHigh == false) | (TempRON < OctLow & LastGuessHigh == true))
                        {
                            BeginBinary = true;
                        }
                        if (BeginBinary)
                        {
                            if (ChangeBy > 0.5) ChangeBy = ChangeBy / 2;
                        }
                        else
                        {
                            NotFirstGuess = true;
                        }
                        // Avoid  flipping back and forth between two temps
                        OldTemp2 = OldTemp1;
                        OldTemp1 = ReactorT[1];
                        if (TempRON >= OctHigh)
                        {
                            LastGuessHigh = true;
                            for (int K = 1; K < NumReactors; K++)
                            {
                                ReactorT[K] = ReactorT[K] - ChangeBy;
                            }
                            if (ReactorT[1] == OldTemp2)
                            {
                                ChangeBy = ChangeBy / 2;
                                for (int K = 1; K < NumReactors; K++)
                                {
                                    ReactorT[K] = ReactorT[K] + ChangeBy;
                                }
                            }
                            else if (TempRON < OctLow)
                            {
                                LastGuessHigh = false;
                                for (int K = 1; K < NumReactors; K++)
                                {
                                    ReactorT[K] = ReactorT[K] + ChangeBy;
                                }
                                if (ReactorT[1] == OldTemp2)
                                {
                                    ChangeBy = ChangeBy / 2;
                                    for (int K = 1; K < NumReactors; K++)
                                    {
                                        ReactorT[K] = ReactorT[K] - ChangeBy;
                                    }
                                }
                            }
                        }
                        else
                            break;
                    }
                }
            }
        }
    }

    // In Wegstein, F_RecyStar is entered in reactors, and F_Recynew  (passed later to F_Recy)
    // comes out. Check if they are the same (truly converged). if not, try converging
    // with direct substitution.

    OctTolerance = OctTolerance * 2;

    for (int J = 0; J < 400; J++)
    {
        CountIter = CountIter + 1;
        //Application.StatusBar = "Program running, performing iteration " & CountIter;

        SumError = 0;
        for (int I = 0; I < NumComp; I++)
        {
            SumError = SumError + Math.Abs(F_RecyStar[I] - F_Recy[I]);
        }
        if (SumError < Tolerance)
        {
            if (SpecOct)
            {
                //Call CalcOctane(F_Ref, TempRON, TempMON)
                if (Math.Abs(TempRON - OctSpec) < Tolerance)
                {
                    Solved = true;
                    break;
                }
                else
                {
                    if (NotFirstGuess)
                    {
                        if ((TempRON >= OctHigh & LastGuessHigh == false) | (TempRON < OctLow & LastGuessHigh == true))
                        {
                            if (ChangeBy > 0.5)
                            {
                                ChangeBy = ChangeBy / 2;
                            }
                        }
                    }
                    else
                    {
                        NotFirstGuess = true;
                    }

                    if (TempRON >= OctHigh)
                    {
                        LastGuessHigh = true;
                        for (int K = 0; K < NumReactors; K++)
                        {
                            ReactorT[K] = ReactorT[K] - ChangeBy;
                        }
                    }
                    else if (TempRON < OctLow)
                    {
                        LastGuessHigh = false;
                        for (int K = 0; K < NumReactors; K++)
                        {
                            ReactorT[K] = ReactorT[K] + ChangeBy;
                        }
                    }
                    else
                    {
                        Solved = true;
                        break;
                    }
                }
            }
        }
        else
        {
            for (int I = 0; I < NumComp; I++)
            {
                F_RecyStar[I] = F_Recy[I];
            }
        }
        Reactors(F_Inlet, F_Eff, ReactorT_Out);
        Flash(F_Eff, Tsep, Psep, fracLiq, F_Ref, F_Vap);
        AssignRecycle(F_Vap, F_NetGas, F_Recy);
    }

    if (Solved)
    {
        Reactors(F_Inlet, F_Eff, ReactorT_Out);
        Flash(F_Eff, Tsep, Psep, fracLiq, F_Ref, F_Vap);
        AssignRecycle(F_Vap, F_NetGas, F_Recynew);
    }
    else
    {
        /*if (true) //Sheets("Input").SpecOct.BaseValue)
        {
            Message = "Octane specification failed to converge.  The last iteration used a first reactor inlet Temperature  of " + ReactorT[1] + " °F.  Try starting with an inlet Temperature  closer to that value.";
        }
        else
        {
            Message = "Recycle calculation failed to converge. Simulation abandoned.";
        }*/
        MessageBox.Show(Message);
    }

    //Application.StatusBar = "Simulation complete, displaying results";
    //DisplayResults(F_Recy, F_Inlet, F_Eff, F_Ref, F_NetGas,        ReactorT_Out)
}

void RungeKutta(double Tr, double ReactorP, double[] F, int RxNo, double WHSV)
{
    // Perform numerical int egration with fourth-order Runge-Kutta method.
    // The independent variable is catalyst weight (weight) and the dependent
    // variables are the flow rate of the Components (F(1 To NumComp)) and the
    // reactor Temperature  (TR).
    //
    // Each iteration adds dW to weight.  if the step made by dW is too large, as
    // determined by the residual, that iteration is repeated with a dW half the
    // initial size.

    // Declare Local variables
    double[] Fint er = new double[31];             // F is flow rate of the Components (passed in), Fint er is int ermediate
    double[] Fnew = new double[31];              // and Fnew  is new  values calcuated in Runge-Kutta loop
    double TRint er, TRnew;                     // int ermediate and new  Temperature  of the reactor in RK loop
    double[] k1 = new double[32], k2 = new double[32], k3 = new double[32], k4 = new double[32];          // Runge-Kutta terms 1 to 4 for the dependent variables
    double[] resid = new double[32];             // checks for any large in any dependent variable
    double BigResid, MaxResid;                  // BigResid stores largest residual used, MaxResid stores largest allowed (both evaluate the absolute value of resid)
                                                // MaxResid stores lowest resid before doubling stepsize
                                                //int  n;                                      // Number of steps taken
    double Dw, MindW, MaxdW, Weight;              // Change in catalyst weight per step, min cat weight per step

    // Catalyst weight (kg)
    double[] dFdW = new double[32];               // Molar flow rate change in,
                                                  // EXCEPT last element, which is dTemperature dW in degR/hr/lbcatalyst
                                                  //int  I, J;                                   // Counters
    string Message = "";                          // Error message if unable to solve
    double Flowrate;

    MaxdW = 0.01; //AmtCat / CDbl(n)
    MindW = 0.01; //AmtCat / CDbl(MaxN)
    Dw = MaxdW;
    MaxResid = 0.05;
    BigResid = 0;
    Weight = 0;

    WHSV = 1 / WHSV;

    do
    {
    restart: // label to allow to go back to the beginning of the loop mid-iteration
             // Evaluate k1 (Runge-Kutta Term 1)
        CalcRates(Tr, ReactorP, F, dFdW, Dw);
        for (int J = 0; J < 32; J++)
        {
            k1[J] = Dw * dFdW[J];
        }

        // Evaluate k2
        for (int J = 0; J < NumComp; J++)
        {
            Fint er[J] = F[J] + k1[J] / 2;
        }

        TRint er = Tr + k1[NumDepVar] / 2;  // Last element hold Temperature  value

        CalcRates(TRint er, ReactorP, Fint er, dFdW, Dw);

        for (int J = 0; J < NumDepVar; J++)
        {
            k2[J] = Dw * dFdW[J];
        }

        // Evaluate k3
        for (int J = 0; J < NumComp; J++)
        {
            Fint er[J] = F[J] + k2[J] / 2;
        }

        TRint er = Tr + k2[NumDepVar] / 2;

        CalcRates(TRint er, ReactorP, Fint er, dFdW, Dw);

        for (int J = 0; J < NumDepVar; J++)
        {
            k3[J] = Dw * dFdW[J];
        }

        // Evaluate k4
        for (int J = 0; J < NumComp; J++)
        {
            Fint er[J] = F[J] + k3[J];
        }

        TRint er = Tr + k3[NumDepVar];

        CalcRates(TRint er, ReactorP, Fint er, dFdW, Dw);

        for (int J = 0; J < NumDepVar; J++)
        {
            k4[J] = Dw * dFdW[J];
        }

        // Evaluate new  flow rates, reactor Temperature , and residuals.
        // if the residual is too large, repeat iteration with smaller dW.
        for (int J = 0; J < NumComp; J++)
        {
            Fnew[J] = F[J] + ((k1[J] + 2 * k2[J] + 2 * k3[J] + k4[J]) / 6);
        }

        TRnew = Tr + (k1[NumDepVar] + 2 * k2[NumDepVar] + 2 * k3[NumDepVar] + k4[NumDepVar]) / 6;

        CalcRates(TRnew, ReactorP, Fnew, dFdW, Dw);

        for (int J = 0; J < NumComp; J++)
        {
            resid[J] = (Fnew[J] - F[J]) / Dw - dFdW[J];
        }

        resid[NumDepVar] = (TRnew - Tr) / Dw - dFdW[NumComp];

        for (int J = 0; J < NumDepVar; J++)
        {
            if (Math.Abs(resid[J]) > BigResid)
            {
                if (Math.Abs(resid[J]) > MaxResid)
                {
                    if (Dw > MindW)
                    {
                        // repeat this iteration with dW half as big.
                        Dw = Dw / 2;
                        goto restart;
                        //GoTo 1;  // go to the beginning of the loop
                    }
                    else
                    {
                        Message = "Simulation abandoned due to failure to converge";
                        MessageBox.Show(Message);
                    }
                }
                BigResid = resid[J];
            }
        }
        for (int J = 0; J < NumComp; J++)
        {
            F[J] = Fnew[J];
        }

        Tr = TRnew;

        Weight = Weight + Dw;
        Dwtot = Dwtot + Dw;

        Flowrate = 0;
        for (int n = 0; n < NumComp; n++)
        {
            Flowrate = Flowrate + MolFlowRates[n];
        }

        resultno = resultno + 1;
        //ThisWorkbook.Worksheets("outputData").Range("a10").Offset(resultno, 0) = Dwtot;
        //ThisWorkbook.Worksheets("outputData").Range("b10").Offset(resultno, 0) = TRnew  - 273.15;
        for (int n = 0; n < NumComp; n++)
        {
            //ThisWorkbook.Worksheets("outputdata").Range("c10").Offset(resultno, n) = F(n) / Flowrate;
        }

        if (FailedFlag == true)
            break;
    }
    while ((double)(Weight + 0.00001) < WHSV);
}

void EstRecycle(double[] F_Recy)
{
    //  Initial estimate of recycle composition.  Based on previous model output.

    double[] CompToH2 = new double[NumComp]; // ratio of moles of component to moles of hydrocarbon in feed (use as first guess)
                                             //int  I;                                 // counter

    CompToH2[0] = 1;
    CompToH2[1] = 0;
    CompToH2[2] = 0; //0.8 //  0.063
    CompToH2[3] = 0; //0.065
    CompToH2[4] = 0; //0.058
    CompToH2[5] = 0; //0.013
    CompToH2[6] = 0; //0.02
    CompToH2[7] = 0; //0.012
    CompToH2[8] = 0; //0.007
    CompToH2[9] = 0; //0.007
    CompToH2[10] = 0; //0.003
    CompToH2[11] = 0;
    CompToH2[12] = 0;
    CompToH2[13] = 0; //0.003
    CompToH2[14] = 0; //0.005
    CompToH2[15] = 0; //0.002
    CompToH2[16] = 0;
    CompToH2[17] = 0;
    CompToH2[18] = 0; ;//0.005
    CompToH2[19] = 0; //0.002
    CompToH2[20] = 0;
    CompToH2[21] = 0;
    CompToH2[22] = 0;
    CompToH2[23] = 0;
    CompToH2[24] = 0; //0.001
    CompToH2[25] = 0;
    CompToH2[26] = 0; //0.001
    CompToH2[27] = 0;
    CompToH2[28] = 0; //0.001
    CompToH2[29] = 0;

    for (int I = 0; I < NumComp; I++)
    {
        F_Recy[I] = MolH2Recy * CompToH2[I];
    }
}

void AssignRecycle(double[] F_Vap, double[] F_NetGas, double[] F_Recy)
{
    // using   the results of a flash calculation, determine the recycle
    // (in mol/hr) to the first reactor and the net gas (in mol/hr).
    // Separated from Flash routine to make flash routine more general.

    string Message = "";                 // Error message
    double FracRecy = 0;                 // Fraction of vapor produced used in recycle
                                         //int  I;                      // Counter

    if (F_Vap[1] < MolH2Recy)
    {
        Message = "Insufficient hydrogen produced for specified recycle. Simulation abandoned.";
        MessageBox.Show(Message);
    }
    else
    {
        FracRecy = MolH2Recy / F_Vap[1];
    }
    for (int I = 0; I < NumComp; I++)
    {
        F_Recy[I] = F_Vap[I] * FracRecy;
        F_NetGas[I] = F_Vap[I] - F_Recy[I];
    }
}

void Reactors(double[] F, double[] F_Eff, double[] ReactorT_Out)
{
    // Calculates the effluent and final Temperature  of each reactor, by calling the
    // InitRateFactors and RungeKutta subroutines, which in turn call the
    // subroutines that model the reactions in the reactors.

    //I As int eger, J As int eger, n As int eger // counters
    double Flowrate;
    double MassFlowRate = 0, VolumeFlowRate = 0;

    Flowrate = 0;
    for (int n = 0; n < NumComp; n++)
        Flowrate = Flowrate + F[n];

    //ThisWorkbook.Worksheets("outputData").Range("a10").Offset(resultno, 0) = 0
    //ThisWorkbook.Worksheets("outputData").Range("b10").Offset(resultno, 0) = ReactorT(1) - 273.15
    ////ThisWorkbook.Worksheets("outputData").Range("c10").Offset(resultno, 0) = -DelTHB
    for (int n = 0; n < NumComp; n++)
    {
        // ThisWorkbook.Worksheets("outputdata").Range("c10").Offset(resultno, n) = F(n) / Flowrate
    }

    double[] liqfracIn = new double[2], liqfracOut = new double[2], F_Liq = new double[31], F_Vap = new double[31];

    for (int J = 0; J < NumReactors; J++)
    {
        Flash(F, (ReactorT[J]), ReactorP[J], liqfracIn[J], F_Liq, F_Vap);  //checked
        InitRateFactors(ReactorP[J], MetalAct[J], AcidAct[J]);  //checked
        RungeKutta(ReactorT[J], ReactorP[J], F, J, MassFeedRate / AmtCat[J]);  //not checked
        ReactorT_Out[J] = ReactorT[J];
        Flash(F, (ReactorT_Out[J]), ReactorP[J], liqfracOut[J], F_Liq, F_Vap);

        if (FailedFlag)
            break;

        if (J == 0)
        {
            for (int n = 0; n < NumComp; n++)
            {
                Flowrate = Flowrate + F[n];
                MassFlowRate = MassFlowRate + F[n] * MW[n];
                VolumeFlowRate = VolumeFlowRate + F[n] * MW[n] / SG[n];
            }

            for (int n = 0; n < NumComp; n++)
            {
                // ThisWorkbook.Worksheets("output").Range("WtPCTint er").Offset(n, activecol - 2) = (F(n) * MW(n)) / MassFlowRate;
                PutExcelValue((F[n] * MW[n]) / MassFlowRate, "WtPCTint er", "output", n, activecol - 2);
            }
        }
    }

    //ThisWorkbook.Worksheets("output").Range("VRx1In").Offset(0, activecol - 2) = 1 - liqfracIn(1)
    //ThisWorkbook.Worksheets("output").Range("VRx1Out").Offset(0, activecol - 2) = 1 - liqfracOut(1)
    //ThisWorkbook.Worksheets("output").Range("VRx2In").Offset(0, activecol - 2) = 1 - liqfracIn(2)
    //ThisWorkbook.Worksheets("output").Range("VRx2Out").Offset(0, activecol - 2) = 1 - liqfracOut(2)
    PutExcelValue(1 - liqfracIn[0], "VRx1In", "output", 0, activecol - 2);
    PutExcelValue(1 - liqfracOut[0], "VRx1Out", "output", 0, activecol - 2);
    PutExcelValue(1 - liqfracIn[1], "VRx2In", "output", 0, activecol - 2);
    PutExcelValue(1 - liqfracOut[1], "VRx2Out", "output", 0, activecol - 2);

    Flowrate = 0;

    MassFlowRate = 0;
    VolumeFlowRate = 0;

    for (int n = 0; n < NumComp; n++)
    {
        Flowrate = Flowrate + F[n];
        MassFlowRate = MassFlowRate + F[n] * MW[n];
        VolumeFlowRate = VolumeFlowRate + F[n] * MW[n] / SG[n];
    }

    //ThisWorkbook.Worksheets("output").Range("TMol").Offset(0, activecol - 2) = Flowrate
    //ThisWorkbook.Worksheets("output").Range("TVol").Offset(0, activecol - 2) = VolumeFlowRate
    //ThisWorkbook.Worksheets("output").Range("TMAss").Offset(0, activecol - 2) = MassFlowRate
    PutExcelValue(Flowrate, "TMol", "output", 0, activecol - 2);
    PutExcelValue(VolumeFlowRate, "TVol", "output", 0, activecol - 2);
    PutExcelValue(MassFlowRate, "TMAss", "output", 0, activecol - 2);

    for (int n = 0; n < NumComp; n++) // Molar
    {
        //ThisWorkbook.Worksheets("output").Range("MolYields").Offset(n, activecol - 2) = F(n) / Flowrate;
        PutExcelValue(F[n] / Flowrate, "MolYields", "output", 0, activecol - 2);
    }

    for (int n = 0; n < NumComp; n++)
    {
        //   ThisWorkbook.Worksheets("output").Range("MassYields").Offset(n, activecol - 2) = (F(n) * MW(n)) / MassFlowRate
        PutExcelValue((F[n] * MW[n]) / MassFlowRate, "MassYields", "output", 0, activecol - 2);
    }

    for (int n = 0; n < NumComp; n++)
    {
        //ThisWorkbook.Worksheets("output").Range("VolYields").Offset(n, activecol - 2) = (F(n) * MW(n) / SG(n)) / VolumeFlowRate
        PutExcelValue((F[n] * MW[n] / SG[n]) / VolumeFlowRate, "VolYields", "output", 0, activecol - 2);
    }

    // effluent of last reactor
    for (int n = 0; n < NumComp; n++)
    {
        F_Eff[n] = F[n];
    }
}

void InitRateFactors(double ReactorP, double MetalAct, double AcidAct)
{
    // Before calling CalcRates, initialize factors. They remain constant,
    // only need to be evaluated once, so done beforehand.

    double IsomPExp;
    double CyclPExp;
    double CrackPExp;
    double DealkPExp;

    // Total Pressure  exponent factors for isomerization, dehydrocyclization,
    // hydrocracking, and hydrodealkylation
    IsomPExp = 0.37;
    CyclPExp = -0.7;
    CrackPExp = 0.53; // .433
    DealkPExp = 0.5;

    // Reaction rates depend on metal or catalyst activity, a Pressure  factor
    // (except the dehydrogenation reactions), and a specific catalyst activity
    // factor that helps tune the results to match test data.
    // Divide by NaphMolFeed to scale reaction rates.  Scaling reversed
    // within the CalcRates subroutine.

    CHDehydrogTerm = MetalAct * DHCHFact / NaphMolFeed;
    CPDehydrogTerm = MetalAct * DHCPFact / NaphMolFeed;
    IsomTerm = AcidAct * (Math.Pow(ReactorP, IsomPExp)) * ISOMFact / NaphMolFeed;
    CyclTerm = AcidAct * (Math.Pow(ReactorP, CyclPExp)) * OPENFact / NaphMolFeed;
    PCrackTerm = AcidAct * (Math.Pow(ReactorP, CrackPExp)) * PHCFact / NaphMolFeed;
    DealkTerm = AcidAct * (Math.Pow(ReactorP, DealkPExp)) * HDAFact / NaphMolFeed;
    NCrackTerm = AcidAct * (Math.Pow(ReactorP, CrackPExp)) * NHCFact / NaphMolFeed;
}

void Flash(double[] F, double TFlash, double PFlash, double fracLiq, double[] F_Liq, double[] F_Vap)
{
    // Chao-Seader flash calculation. Given a molar stream composition and
    // separation T (in degR) & P (in psia), return  s the separator L/F (liquid fraction),
    // and liquid and vapor composition (in mol/hr).
    //
    // Calls KIdeal and KReal to compute equilibrium K values.  Attempts composition convergence using
    // the following modified flash equations:
    //     Sum(n=1 to NumComp) (FeedFrac_i)(1.0-K_i) / ( (L/F)*(1.0-K_i)+K_i) ) = 0.0
    //     where X_i = (FeedFrac_i) / ( (L/F)*(1.0-K_i)+K_i) )   and Y_i = K_i*X_i

    double[] EqK = new double[NumComp];          // Equilibrium K values (vaporization equilibirum ratios) for flash calc
    double[] new X = new double[NumComp];         // new  value of liquid mole fraction
    double TotFeed;                             // Total separator feed (mol/hr)
    double[] X = new double[NumComp];            // Liquid mole fraction
    double[] Y = new double[NumComp];            // Vapor mole fraction
    double[] FeedFrac = new double[NumComp];    // Separator feed mole fraction
    bool RecalcK;                               // Flag to recalculate K value
    double SumX, SumY;                          // Sum of liquid mole fractions, sum vapor mole fractions
    double Deriv, Func, Eras;                    // Used in flash convergence technique
    double Term1;                        // Term (1.0 - K_i) of flash equation
    double Denom;                         // Denominator of flash equation
    double FracLiqnew;                    // new  value for liquid fraction (L/F)
    int MaxTrials;                    // Maximum number of trial and error attempts to solve flash calc
    bool Solved;                      // Flag if variable solved within the trial and error routine
    string Message;                      // Error message
    double TotLiq, TotVap;      // Total moles/hr of liquid and vapor (gas) leaving separator
                                // int  I, J, K;         // Counters

    int CountFracLiq0, CountFracLiq1;

    CountFracLiq0 = 0;
    CountFracLiq1 = 0;

    fracLiq = 0.5;      // initial guess

    TotFeed = 0;

    for (int I = 0; I < NumComp; I++)
    {
        TotFeed = TotFeed + F[I];
    }

    for (int I = 0; I < NumComp; I++)
    {
        FeedFrac[I] = F[I] / TotFeed;
    }

    KIdeal(TFlash, PFlash, EqK);

    for (int I = 0; I < NumComp; I++)
    {
        X[I] = 0;
        Y[I] = 0;
    }

    if (FailedFlag)
        return;

    MaxTrials = 100;
    Solved = false;

    for (int J = 0; J < MaxTrials; J++)
    {
        Deriv = 0;
        Func = 0;
        RecalcK = false;
        for (int I = 0; I < NumComp; I++)
        {
            Term1 = 1 - EqK[I];
            Denom = fracLiq * Term1 + EqK[I];
            new X[I] = FeedFrac[I] / Denom;
            if (Math.Abs(new X[I] - X[I]) > 0.00001)
                RecalcK = true;

            Eras = FeedFrac[I] * Term1 / Denom;
            Func = Func + Eras;
            Deriv = Deriv + Eras * Term1 / Denom;
        }

        if ((Math.Abs(Func) - 0.000001) > 0)
        {
            FracLiqnew = fracLiq + Func / Deriv;
            // First test is not part of the original code.  Original code did not
            // look for dew or bubble point s, where many iterations will happen with
            // same FracLiq as 0 or 1.
            if (FracLiqnew < 0)
            {
                fracLiq = 0.5 * fracLiq;
                CountFracLiq0 = CountFracLiq0 + 1;
                if (CountFracLiq0 > 25)
                {
                    // T is above dew point
                    Solved = true;
                    break;
                }
            }
            else if (FracLiqnew > 1)
            {
                fracLiq = 0.5 * (fracLiq + 1);
                CountFracLiq1 = CountFracLiq1 + 1;
                if (CountFracLiq1 > 25)
                {
                    // T is below bubble point
                    Solved = true;
                    break;
                }
            }
            else
            {
                fracLiq = FracLiqnew;
            }
        }
        else
        {
            SumX = 0;
            SumY = 0;
            for (int I = 0; I < NumComp; I++)
            {
                Term1 = 1 - EqK[I];
                Denom = fracLiq * Term1 + EqK[I];
                X[I] = FeedFrac[I] / Denom;
                Y[I] = EqK[I] * X[I];
                SumX = SumX + X[I];
                SumY = SumY + Y[I];
            }
            for (int I = 0; I < NumComp; I++)
            {
                X[I] = X[I] / SumX;
                Y[I] = Y[I] / SumY;
            }
            if (RecalcK)
            {
                KReal(TFlash, PFlash, EqK, X, Y);
            }
            else
            {
                Solved = true;
                break;
            }
        }
    }

    if (!Solved)
    {
        Message = "Flash calculation failed. Simulation abandoned.";
        MessageBox.Show(Message);
    }

    if (fracLiq > 0.999999)
    {   // BELOW BUBBLE POint  // THIS IS MISSING forM THE ORIGINAL REforMER CODE
        for (int I = 0; I < NumComp; I++)
        {
            F_Liq[I] = F[I];
            F_Vap[I] = 0;
        }
        return;
    }
    else if (fracLiq < 0.00001)
    { // ABOVE DEW POint
        for (int I = 0; I < NumComp; I++)
        {
            F_Liq[I] = 0;
            F_Vap[I] = F[I];
        }
    }

    TotLiq = TotFeed * fracLiq;
    TotVap = TotFeed - TotLiq;
    for (int I = 0; I < NumComp; I++)
    {
        F_Liq[I] = TotLiq * X[I];
        F_Vap[I] = TotVap * Y[I];
    }
}

void CalcRates(double tK, double Pr, double[] Flowrate, double[] RateOfChange, double Dw)
{
    if (Math.Abs(tK) > 3000)
    {
        MessageBox.Show("Calc Failed");
        //Application.Calculation = xlCalculationAutomatic;
    }

    // Calculates rate constants (RateK) and equilibrium constants
    // (EquilK) for the 80 reactions at the given reactor Temperature
    // (TR). A1, A2, B1, and B2 are constants declared in InitConstants.
    //
    // Then calculates the reaction rates for each reaction, which
    // are used to determine the rates of change for each component and
    // for the reactor Temperature .

    double[] RateK = new double[NumRxn], equilk = new double[NumRxn], RxnRate = new double[NumRxn];
    double H2Term, T1, T2, T3, T4;                        // Modified Temps for CP calcs
    double TD1, TD2, TD3, TD4;    // Modified Temp differences for Heat of Formation calcs
    double DelTHB, SumHF, SumCP, TempTotal;
    // Declare variables to store component mole fractions
    double YH2, YC1, YC2, YC3, YIC4, YNC4, YIC5, YNC5, YIC6, YNC6, YCH, YMCP, YBEN, YIC7, YNC7, YMCH, YC7CP, YTOL, YC8P, YECH, YDMCH, YPCP, YC8CP, YEB, YPX, YMX, YOX, YC9P;            // C9+ paraffins
    double YC9CH;     // C9+ cyclohexanes
    double YC9CP;     // C9+ cyclopentanes
    double YC9A;      // C9+ aromatics

    // Calculate mole fractions
    TempTotal = 0;

    for (int I = 0; I < NumComp; I++)
    {
        TempTotal = TempTotal + Flowrate[I];
    }
    // Use the original naphtha feed rate (in mol/hr) to determine Y values,
    // except for YH2 which is the H2 partial Pressure
    YH2 = Flowrate[0] / TempTotal;
    YC1 = Flowrate[1] / NaphMolFeed;
    YC2 = Flowrate[2] / NaphMolFeed;
    YC3 = Flowrate[3] / NaphMolFeed;
    YIC4 = Flowrate[4] / NaphMolFeed;
    YNC4 = Flowrate[5] / NaphMolFeed;
    YIC5 = Flowrate[6] / NaphMolFeed;
    YNC5 = Flowrate[7] / NaphMolFeed;
    YIC6 = Flowrate[8] / NaphMolFeed;
    YNC6 = Flowrate[9] / NaphMolFeed;
    YCH = Flowrate[10] / NaphMolFeed;
    YMCP = Flowrate[11] / NaphMolFeed;
    YBEN = Flowrate[12] / NaphMolFeed;
    YIC7 = Flowrate[13] / NaphMolFeed;
    YNC7 = Flowrate[14] / NaphMolFeed;
    YMCH = Flowrate[15] / NaphMolFeed;
    YC7CP = Flowrate[16] / NaphMolFeed;
    YTOL = Flowrate[17] / NaphMolFeed;
    YC8P = Flowrate[18] / NaphMolFeed;
    YECH = Flowrate[19] / NaphMolFeed;
    YDMCH = Flowrate[20] / NaphMolFeed;
    YPCP = Flowrate[21] / NaphMolFeed;
    YC8CP = Flowrate[22] / NaphMolFeed;
    YEB = Flowrate[23] / NaphMolFeed;
    YPX = Flowrate[24] / NaphMolFeed;
    YMX = Flowrate[25] / NaphMolFeed;
    YOX = Flowrate[26] / NaphMolFeed;
    YC9P = Flowrate[27] / NaphMolFeed;
    YC9CH = Flowrate[28] / NaphMolFeed;
    YC9CP = Flowrate[29] / NaphMolFeed;
    YC9A = Flowrate[30] / NaphMolFeed;

    Concs[0] = 1;   //
    Concs[1] = YH2; // Not in reactionbn// do not rebase to 0
    for (int I = 2; I <= NumComp; I++)
    {
        Concs[I] = Flowrate[I - 1] / NaphMolFeed;
    }

    for (int I = 0; I < NumRxn; I++)
    {
        RateK[I] = K[I] * Math.Exp(-Ea[I] / (Rgas * tK));
        equilk[I] = keq[I] * Math.Exp(Eaeq[I] / (Rgas * tK));
    }

    double[] rrate = new double[80];

    H2Term = YH2 * Pr;

    for (int I = 0; I < NumRxn; I++)
    {
        if (equilk[I] == 0)
        {
            rrate[I] = Factor[I] * RateK[I] * ((Math.Pow(YH2, H2Afact[I])) * Concs[a[I]] * Concs[C[I]]);
        }
        else
        {
            rrate[I] = Factor[I] * RateK[I] * ((Math.Pow(H2Term, H2Afact[I]) * Concs[a[I]] * Concs[C[I]]
                - Math.Pow(H2Term, H2Bfact[I]) * Concs[b[I]] * Concs[D[I]] / equilk[I]));
        }
    }

    //For I = 0 To NumRxn
    //    ThisWorkbook.Worksheets("ReactionStoich").Range("d48").Offset(0, I) = rrate(I)
    //    ThisWorkbook.Worksheets("ReactionStoich").Range("d49").Offset(0, I) = Concs(A(I))
    //    ThisWorkbook.Worksheets("ReactionStoich").Range("d50").Offset(0, I) = Concs(B(I))
    //Next

    for (int II = 0; II < NumComp; II++)
    {
        RateOfChange[II] = 0;   // Moles
        for (int I = 0; I < NumRxn; I++)
        {
            RateOfChange[II] = RateOfChange[II] + Stoich[II, I] * rrate[I] * NaphMolFeed;
        }
    }

    // Adjust Temperature s for use in the heat calculation double  s
    T1 = tK;                   // Divide by 100 to keep numbers smaller during calculation
    T2 = T1 * T1;              // T1^2
    T3 = T1 * T2;              // T1^3
    T4 = T1 * T3;              // T1^4
    TD1 = T1;
    TD2 = T2;
    TD3 = T3;
    TD4 = T4;

    double Enth, Hc, NT, DHf = 0;

    SumHF = 0;
    SumCP = 0;

    for (int I = 0; I < NumComp; I++)
    {
        Enth = (CpCoeft[1][I] * (TD1 - tbase) + CpCoeft[2][I] * (Math.Pow(tK, 2) - Math.Pow(tbase, 2)) + CpCoeft[3][I] * (Math.Pow(tK, 3) - Math.Pow(tbase, 3))
        + CpCoeft[4][I] * (Math.Pow(tK, 4) - Math.Pow(tbase, 4))) * MW[I];

        SumHF = SumHF + RateOfChange[I] * (StdHeatForm[I] + Enth);

        DHf = DHf + RateOfChange[I] * StdHeatForm[I] * 0.01;

        Hc = (CpCoeft[1][I] + 2 * CpCoeft[2][I] * T1 + 3 * CpCoeft[3][I] * T2 + 4 * CpCoeft[4][I] * T3) * MW[I];
        SumCP = SumCP + Flowrate[I] * Hc;
    }

    NT = Enthalpy(Flowrate, tK, Pr);

    if (IsIsothermal)
    {
        DelTHB = 0;
    }
    else
    {
        if (!Rigourous)
        {
            DelTHB = SumHF / SumCP;
            RateOfChange[NumDepVar] = -DelTHB * HeatLossFactor;
        }
        else
        {
            //DHf = HFormation(Flowrate, tK, Pr, RateOfChange)
            NT = new T(Flowrate, tK, Pr, RateOfChange, DHf, Dw);
            DelTHB = (tK - NT) / Dw;
            RateOfChange[NumDepVar] = -DelTHB;
        }
    }

    if (FailedFlag)
        return;
}

double new T(double[] Flowrate, double  tK, double  Pr, double[] RateOfChange, double  DHf, double  Dw)
        {
            double  totenth, new enth, oldenth, delta, tnew ;
            double [] Fnew  = new  double [31];

            totenth = Enthalpy(Flowrate, tK, Pr) - DHf;

            for (int  J = 0; J < NumComp; J++)
            {
                Fnew [J] = Flowrate[J] + RateOfChange[J] * Dw;
            }

            delta = 5;
tnew = tK + delta;

oldenth = Enthalpy(Fnew, tnew, Pr);

do
{
    new enth = Enthalpy(Fnew, tnew, Pr);

    if (delta < 0.00000001)
    {
        return tnew;
    }

    if (new enth > totenth & oldenth < totenth)
    {
        delta = delta / 2;
    }
    else if (new enth<totenth & oldenth > totenth)
    {
        delta = delta / 2;
    }

    if (new enth > totenth)
    {
        tnew = tnew - delta;
    }
    else
    {
        tnew = tnew + delta;
    }

    oldenth = new enth;
}

while (Math.Abs(new enth - totenth) > 1);   //1%
return tnew;
        }

        void KIdeal(double Tsep, double Psep, double[] EqK)
{
    // Compute ideal K (vaporization equilibirum ratio, K=y/x) values using   the Chao-Seader
    // technique

    //Dim I As int eger                     ' Counter
    double[] GamLog = new double[NumComp];    //   ' Natural log of gamma (liquid activity coefficient)
    double Tr, TR2, TR3, Pr, PR2, Term;          // Reduced Temperature
                                                 // Reduced temp squared, cubed
                                                 // Reduced Pressure , reduced Pressure  squared
                                                 // int ermediate calculation
    double[] FLLog = new double[NumComp];    // Natural log of liquid fugacity coefficient
    double[] FVLog = new double[NumComp];    // Natural log of vapor fugacity coefficient

    try
    {
        for (int I = 0; I < NumComp; I++)
        {
            GamLog[I] = 0;
            Tr = Tsep / tcrit[I];
            Pr = Psep / Pcrit[I];
            TR2 = Tr * Tr;
            TR3 = TR2 * Tr;
            PR2 = Pr * Pr;
            // calculate natural log of liquid fugacity using   Chao-Seader liquid
            // fugacity coefficients.  Use separate coefficients for hydrogen, methane,
            // and generalized fluids
            if (I == 0) // H2
            {
                Term = 1.96718 + 1.02972 / Tr
                    - 0.054009 * Tr + 0.0005288 * TR2
                       + 0.008585 * Pr;
            }
            else if (I == 1) // CH4
            {
                Term = 2.4384 - 2.2455 / Tr - 0.34084 * Tr
                    + 0.00212 * TR2 - 0.00223 * TR3
                       + Pr * (0.10486 - 0.03691 * Tr);
            }
            else
            {
                Term = 5.75748 - 3.01761 / Tr - 4.985 * Tr + 2.02299 * TR2
                    + Pr * (0.08427 + 0.26667 * Tr - 0.31138 * TR2)
                       + PR2 * (-0.02655 + 0.02883 * Tr) + w[I] * (-4.23893 + 8.65808 * Tr - 1.2206 / Tr
                       - 3.15224 * TR3 - 0.025 * (Pr - 0.6));
            }

            FLLog[I] = Term * 2.3025851;
            FVLog[I] = 0;
            EqK[I] = Math.Exp(FLLog[I] + GamLog[I] - FVLog[I]) / Pr;
        }
    }
    catch
    {
        FailedFlag = true;
    }
}

void KReal(double Tsep, double Psep, double[] EqK, double[] X, double[] Y)
{
    // Compute real K (vaporization equilibirum ratio, K=y/x) values using   the Chao-Seader
    // technique

    double[] A_EOS = new double[NumComp];      // Constant a for the Redlich-Kwong equation of state
    double[] B_EOS = new double[NumComp];      // Constant b for the Redlich-Kwong equation of state
    double AMix, BMix;    // Redlich-Kwong constants for the mixture
    double AOB, BP;       // terms in Redlich-Kwong equation
    double AOAMix, BOBMix, RT;
    double Tr;                      // Reduced Temperature
    double TR2, TR3;      // Reduced temp squared, cubed
    double Pr, PR2;       // Reduced Pressure , reduced Pressure  squared
    double Term, Term1, Term2, Term3; // int ermediate calculations
    double[] FLLog = new double[NumComp];      // Natural log of liquid fugacity coefficient
    double[] FVLog = new double[NumComp];     // Natural log of vapor fugacity coefficient
    double Z, Znew;        // Compressibility factor
    int MaxTrials;               // Maximum number of trial and error attempts to solve for Z
    bool Solved;                  // Flag to check if variable solved within the trial and error routine
    double Func, Deriv;    // For solution by trial and error by int erval halving method
    double VolMix;                   // Volume of mixture (molar)
    double SolAvg;                   // Average solubility parameter for solution
    double[] GamLog = new double[NumComp];     // Natural log of gamma (liquid activity coefficient)
    string Message;                 // Error message

    // Calculate Z (vapor phase compressibility factor) using   Redlich and Kwong EOS
    // using   a trial and error method by int erval halving technique

    try
    {
        MaxTrials = 40;
        AMix = 0;
        BMix = 0;
        for (int I = 0; I < NumComp; I++)
        {
            Tr = Tsep / tcrit[I];
            A_EOS[I] = Math.Sqrt(0.4278 / (Pcrit[I] * (Math.Pow(Tr, 2.5))));
            B_EOS[I] = 0.0867 / (Pcrit[I] * Tr);
            AMix = AMix + A_EOS[I] * Y[I];
            BMix = BMix + B_EOS[I] * Y[I];
        }
        AOB = (Math.Pow(AMix, 2)) / BMix;
        BP = BMix * Psep;
        Z = 1;
        Solved = false;
        for (int J = 0; J < MaxTrials; J++)
        {
            Func = Z / (Z - BP) - AOB * BP / (Z + BP) - Z;
            if ((Math.Abs(Func) - 0.000001) < 0)
            {
                Solved = true;
                break;
            }
            Deriv = -BP / Math.Pow((Z - BP), 2) + AOB * BP / Math.Pow((Z + BP), 2) - 1;
            Znew = Z - Func / Deriv;
            if ((Znew - BP) < 0)
            {
                Z = (Z + BP) * 0.5;
            }
            else if (Znew > 2)
            {
                Z = (Z + 2) * 0.5;
            }
            else
            {
                Z = Znew;
            }
        }
        // If Z did not converge within MaxTrials, assign a value
        if (!Solved)
        {
            Z = 1.02;
            Message = "Compressibility factor (Z) calculation failed in KReal routine. Used assigned value of 1.02.";
            MessageBox.Show(Message);
        }

        VolMix = 0;
        SolAvg = 0;
        for (int I = 0; I < NumComp; I++)
        {
            Term = X[I] * MolVol[I];
            VolMix = VolMix + Term;
            SolAvg = SolAvg + Term * Sol[I];
        }
        SolAvg = SolAvg / VolMix;
        RT = 1.1038889 * Tsep;
        Term1 = Z - 1;
        if (Z < BP)
        {                           // temporary solution too MUCh hydrogen
            Term2 = 0;
        }
        else
        {
            Term2 = Math.Log(Z - BP);
        }

        Term3 = Math.Log(1 + BP / Z);

        for (int I = 0; I < NumComp; I++)
        {
            GamLog[I] = (MolVol[I] / RT) * Math.Pow((Sol[I] - SolAvg), 2);
            Tr = Tsep / tcrit[I];
            Pr = Psep / Pcrit[I];
            TR2 = Tr * Tr;
            TR3 = TR2 * Tr;
            PR2 = Pr * Pr;
            if (I == 0) // H2
            {
                Term = 1.96718 + 1.02972 / Tr - 0.054009 * Tr + 0.0005288 * TR2 + 0.008585 * Pr;
            }
            else if (I == 1) //CH4
            {
                Term = 2.4384 - 2.2455 / Tr - 0.34084 * Tr + 0.00212 * TR2 - 0.00223 * TR3 + Pr * (0.10486 - 0.03691 * Tr);
            }
            else
            {
                Term = 5.75748 - 3.01761 / Tr - 4.985 * Tr + 2.02299 * TR2 + Pr * (0.08427 + 0.26667 * Tr - 0.31138 * TR2)
                       + PR2 * (-0.02655 + 0.02883 * Tr) + w[I] * (-4.23893 + 8.65808 * Tr - 1.2206 / Tr - 3.15224 * TR3
                       - 0.025 * (Pr - 0.6));
            }
            FLLog[I] = Term * 2.3025851;
            BOBMix = B_EOS[I] / BMix;
            AOAMix = 2 * A_EOS[I] / AMix;
            FVLog[I] = Term1 * BOBMix - Term2 - AOB * (AOAMix - BOBMix) * Term3;
            EqK[I] = Math.Exp(FLLog[I] + GamLog[I] - FVLog[I]) / Pr;
        }

        return;
    }
    catch
    {
        FailedFlag = true;
    }
}

double Enthalpy(double[] Stream, double tK, double pBar)
{
    double[] Liq = new double[31], Vap = new double[31];
    double Enth, TLiqEnth, TGasEnth;
    int L = 0;

    Flash(Stream, tK, pBar, L, Liq, Vap);

    if (FailedFlag)
        return 0;

    Enth = 0;
    for (int n = 0; n < 31; n++)
    {
        //Enth = Enth + StdHeatForm(N) * (Liq(N) + Vap(N))
        TLiqEnth = LiqEnthalpy(n, tK, pBar);
        TGasEnth = GasEnthalpy(n, tK, pBar);
        Enth = Enth + TLiqEnth * Liq[n];
        Enth = Enth + TGasEnth * Vap[n];
        //ThisWorkbook.Worksheets("ComponentData").Range("FlashVap").Offset(N, 0) = Vap(N)
        //ThisWorkbook.Worksheets("ComponentData").Range("FlashLiq").Offset(N, 0) = Liq(N)
        //ThisWorkbook.Worksheets("ComponentData").Range("FlashVap").Offset(N, 3) = TGasEnth
        //ThisWorkbook.Worksheets("ComponentData").Range("FlashVap").Offset(N, 4) = TLiqEnth
    }
    return Enth;
}

double HFormation(double[] Stream, double tK, double pBar)
{
    double Enth = 0;

    for (int n = 0; n < 31; n++)
    {
        Enth = Enth + StdHeatForm[n] * (Stream[n]);
    }

    return Enth;
}

double GasEnthalpy(int I, double tK, double pBar)
{
    //PengRobinson Pr, PRb;

    double EnthDep1, EnthDep2;
    PengRobinson.Initialise(tcrit[I] - 273.15, Pcrit[I], w[I]);
    PengRobinson.Update(tK - 273.15, pBar);
    EnthDep1 = PengRobinson.enthdep1;

    PengRobinson.Initialise(tcrit[I] - 273.15, Pcrit[I], w[I]);
    PengRobinson.Update(273.15, 1);
    EnthDep2 = PengRobinson.enthdep1;

    double igEnth;

    tbase = 273.15;

    if (tcrit[1] == 0)
        Constants();

    //Pr = new  PengRobinson(tcrit[I] - 273.15, Pcrit[I], w[I], tK - 273.15, pBar);
    //PRb = new  PengRobinson(tcrit[I] - 273.15, Pcrit[I], w[I], 273.15, 1);

    igEnth = (CpCoeft[1][I] * (tK - tbase)
       + CpCoeft[2][I] * (Math.Pow(tK, 2) - Math.Pow(tbase, 2))
       + CpCoeft[3][I] * (Math.Pow(tK, 3) - Math.Pow(tbase, 3))
       + CpCoeft[4][I] * (Math.Pow(tK, 4) - Math.Pow(tbase, 4))) * MW[I];

    return (igEnth + EnthDep1 - EnthDep2);  // vap or supercritical
}

double LiqEnthalpy(int I, double tK, double pBar)
{
    double EnthDep1, EnthDep2, EnthDep3;
    PengRobinson.Initialise(tcrit[I] - 273.15, Pcrit[I], w[I]);
    PengRobinson.Update(tK - 273.15, pBar);
    EnthDep1 = PengRobinson.enthdep1;
    EnthDep3 = PengRobinson.enthdep3;

    PengRobinson.Initialise(tcrit[I] - 273.15, Pcrit[I], w[I]);
    PengRobinson.Update(0, 1);
    EnthDep2 = PengRobinson.enthdep1;
    double igEnth;

    tbase = 273.15;

    if (tcrit[1] == 0)
        Constants();

    //Pr = new  PengRobinson(tcrit[I] - 273.15, Pcrit[I], w[I], tK-273.15, pBar);  //C and Bar
    //PRb = new  PengRobinson(tcrit[I] - 273.15, Pcrit[I], w[I], 0, 1);            //C and Bar

    igEnth = (CpCoeft[1][I] * (tK - tbase)
       + CpCoeft[2][I] * (Math.Pow(tK, 2) - Math.Pow(tbase, 2))
       + CpCoeft[3][I] * (Math.Pow(tK, 3) - Math.Pow(tbase, 3))
       + CpCoeft[4][I] * (Math.Pow(tK, 4) - Math.Pow(tbase, 4))) * MW[I];

    if (double.IsNaN(EnthDep3)) // Vapour Only, shouldnt be any material in liquid phase, assume ideal if there is.
    {
        return igEnth;  // + Pr.EnthDep1 - PRb.EnthDep1;
    }
    else  // Sub Critical liq and vap
    {
        return igEnth + EnthDep3 - EnthDep2; ;
    }
}

double EnthalpyDep(double TcK, double PcBar, double Omega, double tK, double pBar)
{
    PengRobinson.InitialiseAndCalc(TcK, PcBar / 10, Omega, tK, pBar / 10);
    return PengRobinson.enthdep1;
}

double EnthalpyDep3(double TcK, double PcBar, double Omega, double tK, double pBar)
{
    PengRobinson.InitialiseAndCalc(TcK, PcBar / 10, Omega, tK, pBar / 10);
    return PengRobinson.enthdep3;
}

bool return OneRoot(double  TcK, double  PcBar, double  Omega, double  tK, double  pBar)
        {
    PengRobinson.InitialiseAndCalc(TcK, PcBar / 10, Omega, tK, pBar / 10);
    return PengRobinson.isoneroot;
}

double FugacityRatio(double TcK, double PcBar, double Omega, double tK, double pBar)
{
    PengRobinson.InitialiseAndCalc(TcK, PcBar / 10, Omega, tK, pBar / 10);
    return PengRobinson.fugratio;
}

double Tboil(double TcK, double PcBar, double Omega, double tK, double pBar)
{
    double OldFugRatio, Temp, delta;

    Temp = 273.15;
    PengRobinson.InitialiseAndCalc(tK, PcBar / 10, Omega, Temp, pBar / 10);

    if (PengRobinson.isoneroot)
    {
        return 0;
    }

    OldFugRatio = PengRobinson.fugratio;

    if (PengRobinson.fugratio < 1)

        delta = -10;
    else
        delta = 10;

    do
    {
        Temp = Temp + delta;
        PengRobinson.Update(Temp, pBar / 10);
        if (PengRobinson.fugratio > 1 & OldFugRatio < 1)
            delta = -delta / 2;
        else if (PengRobinson.fugratio < 1 & OldFugRatio > 1)
            delta = -delta / 2;

        OldFugRatio = PengRobinson.fugratio;

        if (OldFugRatio == 9999)
        {
            return 0;
        }
    }

    while (Math.Abs(PengRobinson.fugratio - 1) > 0.00001);

    return Temp - 273.15;
}

double VP(double TcK, double PcBar, double Omega, double tK, double pBar)
{
    if (tK >= TcK)
    {
        return 10;
    }

    double Vpr;

    Vpr = 0.1;

    PengRobinson.InitialiseAndCalc(TcK, PcBar / 10, Omega, tK, Vpr);

    if (PengRobinson.isoneroot)
    {
        return 9999;
    }

    double OldFugRatio, delta;

    OldFugRatio = PengRobinson.fugratio;

    if (PengRobinson.fugratio < 1)
    {
        delta = 0.01;
    }
    else
    {
        delta = -0.01;
    }

    do
    {
        Vpr = Vpr + delta;
        PengRobinson.Update(tK, Vpr);
        if (PengRobinson.fugratio > 1 & OldFugRatio < 1)
        {
            delta = -delta / 2;
        }
        else if (PengRobinson.fugratio < 1 & OldFugRatio > 1)
        {
            delta = -delta / 2;
        }

        OldFugRatio = PengRobinson.fugratio;
        if (OldFugRatio == 9999)
        {
            return 0;
        }
    }

    while (Math.Abs(PengRobinson.fugratio - 1) > 0.00001);

    return Vpr * 10;  // bar
}

double H2VP(double tK, double patm)
{
    patm = patm / 100;
    return Math.Pow(1.05, (patm / 50) * Math.Exp(1.4054 + 812 / (tK)));
}

private void releaseObject(object obj)
{
    try
    {
        System.Runtime.int eropServices.Marshal.ReleaseComObject(obj);
        obj = null;
    }
    catch (Exception ex)
    {
        obj = null;
        MessageBox.Show("Unable to release the Object " + ex.ToString());
    }
    finally
    {
        GC.Collect();
    }
}

private void RunMacro(object oApp, object[] oRunArgs)
{
    oApp.GetType().InvokeMember("Run",
        System.Reflection.BindingFlags.Default |
        System.Reflection.BindingFlags.InvokeMethod,
        null, oApp, oRunArgs);
}

double GetExcelValue(string A, String WS, int rowoffset, int columnoffset)
{
    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(WS);
    Excel.Range Range = xlWorkSheet.get_Range(A, A);
    int Row = Range.Row + rowoffset, Column = Range.Column + columnoffset;
    string Col = Number2String(Column, true);
    string cell = Col + Row.ToString();
    double result = 0;
    try
    {
        result = (double)xlWorkSheet.get_Range(cell, cell).Value2;
    }
    catch
    {
        result = 0;
    }
    return result;
}

int GetExcelint Value(string A, String WS, int  rowoffset, int  columnoffset)
        {
    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(WS);
    Excel.Range Range = xlWorkSheet.get_Range(A, A);
    int Row = Range.Row + rowoffset, Column = Range.Column + columnoffset;
    string Col = Number2String(Column, true);
    string cell = Col + Row.ToString();
    int result = 0;
    try
    {
        result = Convert.Toint 16(xlWorkSheet.get_Range(cell, cell).Value2);
    }
    catch
    {
        result = 0;
    }
    return result;
}

void PutExcelValue(double Val, string Location, String WS, int rowoffset, int columnoffset)
{
    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(WS);
    Excel.Range Range = xlWorkSheet.get_Range(Location, Location);
    int Row = Range.Row + rowoffset, Column = Range.Column + columnoffset;
    string Col = Number2String(Column, true);
    string cell = Col + Row.ToString();
    try
    {
        xlWorkSheet.get_Range(cell, cell).Value2 = Val;
    }
    catch
    {
    }
    return;
}

private String Number2String(int number, bool isCaps)
{
    Char c = (Char)((isCaps ? 65 : 97) + (number - 1));
    return c.ToString();
}
    }
}