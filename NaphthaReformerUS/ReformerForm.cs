using EngineThermo;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Units;
using Units.UOM;

namespace NaphthaReformerUS
{
    public partial class ReformerForm : Form
    {
        NapReformerUS NapRef = new NapReformerUS();
        List<string> Names = new List<string>() {"H2","C1","C2","C3","IC4","NC4","IC5","NC5","IC6","NC6","CH","MCP","BEN",
            "IC7","NC7","MCH","C7CP","TOL","C8P","ECH","DMCH","PCP","C8CP","EB","PX","MX","OX","C9+P","C9+CH","C9+CP","C9+A"};

        AssayBasis assayBasis = AssayBasis.Volume;
        UOMProperty Psep = new();
        UOMProperty Tsep = new();

        public ReformerForm()
        {
            InitializeComponent();
            ReactorData.DGV.Columns[2].HeaderText = "Reactor 1";
            ReactorData.AddColumn("Reactor2");
            ReactorData.AddColumn("Reactor3");
            ReactorData.AddColumn("Reactor4");

            DGVFurnace.DGV.Columns[2].HeaderText = "Furnace 1";
            DGVFurnace.AddColumn("Furnace 2");
            DGVFurnace.AddColumn("Furnace 3");

            DGVReactor.DGV.Columns[2].HeaderText = "Rx 1";
            DGVReactor.AddColumn("Rx2");
            DGVReactor.AddColumn("Rx3");
            DGVReactor.AddColumn("Rx4");

            // DGVProductData.AddColumn()
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            Psep = new UOMProperty(new Pressure(2.5, PressureUnit.Kg_cm2_g), SourceEnum.Input, "Separtor Pressure");
            Tsep = new UOMProperty(new Temperature(38, TemperatureUnit.Celsius), SourceEnum.Input, "Separator Temperature");

            DGSeparator.Add(Psep);
            DGSeparator.Add(Tsep);
            DGSeparator.UpdateProps();

            UOMProperty prop0 = new UOMProperty(ePropID.MF, SourceEnum.Input, 1676, "Rate");

            UOMProperty prop1 = new UOMProperty(ePropID.T, SourceEnum.Input, 273.15 + 102, "D86 IBP");
            UOMProperty prop2 = new UOMProperty(ePropID.T, SourceEnum.Input, 273.15 + 114, "D86 5");
            UOMProperty prop3 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "D86 10");
            UOMProperty prop4 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "D86 30");
            UOMProperty prop5 = new UOMProperty(ePropID.T, SourceEnum.Input, 273.15 + 132, "D86 50");
            UOMProperty prop6 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "D86 70");
            UOMProperty prop7 = new UOMProperty(ePropID.T, SourceEnum.Input, 273.15 + 160, "D86 90");
            UOMProperty prop8 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "D86 95");
            UOMProperty prop9 = new UOMProperty(ePropID.T, SourceEnum.Input, 273.15 + 178, "D86 99");
            UOMProperty prop10 = new UOMProperty(ePropID.Density, SourceEnum.Input, 0.747, "Density");

            pdg.Add(prop0);
            pdg.Add(prop1);
            pdg.Add(prop2);
            pdg.Add(prop3);
            pdg.Add(prop4);
            pdg.Add(prop5);
            pdg.Add(prop6);
            pdg.Add(prop7);
            pdg.Add(prop8);
            pdg.Add(prop9);
            pdg.Add(prop10);
            pdg.UpdateProps();

            LoadShortGrid();
            LoadMediumGrid();
            LoadFullGrid();
            LoadReactor();
            LoadRecyle();
            LoadCatalyst();
            LoadFurnace();
            LoadNoRx();
            LoadCalibrationData();
        }

        private void LoadCalibrationData()
        {
            UOMProperty R1 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.0, "CrackP6");
            UOMProperty R2 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.05, "CrackP7");
            UOMProperty R3 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.09, "CrackP8");
            UOMProperty R4 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.0, "DealkA6");
            UOMProperty R5 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.3, "DealkA7");
            UOMProperty R6 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.05, "DealkA8");
            UOMProperty R7 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.0, "CycleP6");
            UOMProperty R8 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.0, "CycleP7");
            UOMProperty R9 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.25, "CycleP8");
            UOMProperty R10 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.56, "CycleP9");
            UOMProperty R11 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.9, "DehydN6");
            UOMProperty R12 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.8, "DehydN7");
            UOMProperty R13 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.5, "DehydN8");
            UOMProperty R14 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.4, "DehydN9");
            UOMProperty R15 = new UOMProperty(ePropID.NullUnits, SourceEnum.Input, 1.0, "EquilCyc9");

            DGVCalibrationData1.Add(R1);
            DGVCalibrationData1.Add(R2);
            DGVCalibrationData1.Add(R3);
            DGVCalibrationData1.Add(R4);
            DGVCalibrationData1.Add(R5);
            DGVCalibrationData1.Add(R6);
            DGVCalibrationData1.Add(R7);
            DGVCalibrationData1.Add(R8);
            DGVCalibrationData1.Add(R9);
            DGVCalibrationData1.Add(R10);
            DGVCalibrationData1.Add(R11);
            DGVCalibrationData1.Add(R12);
            DGVCalibrationData1.Add(R13);
            DGVCalibrationData1.Add(R14);
            DGVCalibrationData1.Add(R15);

            DGVCalibrationData1.UpdateProps();
        }

        public void LoadShortGrid()
        {
            UOMProperty P = new UOMProperty(ePropID.Percentage, SourceEnum.Input, 68.7, "P, LV %");
            UOMProperty N = new UOMProperty(ePropID.Percentage, SourceEnum.Input, 28.34, "N, LV %");
            UOMProperty A = new UOMProperty(ePropID.Percentage, SourceEnum.Input, 10.79, "A, LV %");

            ShortGrid.Add(P);
            ShortGrid.Add(N);
            ShortGrid.Add(A);
            ShortGrid.UpdateProps();
        }
        public void LoadMediumGrid()
        {
            UOMProperty M1 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "Units");
            UOMProperty M2 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "C5");
            UOMProperty M3 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "P6");
            UOMProperty M4 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "P7");
            UOMProperty M5 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "P8");
            UOMProperty M6 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "P9+");
            UOMProperty M7 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "MCP");
            UOMProperty M8 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "CH");
            UOMProperty M9 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "N7");
            UOMProperty M10 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "N8");
            UOMProperty M11 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "N9+");
            UOMProperty M12 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "A6");
            UOMProperty M13 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "A7");
            UOMProperty M14 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "A8");
            UOMProperty M15 = new UOMProperty(ePropID.Percentage, SourceEnum.Input, double.NaN, "A9+");


            MediumGrid.Add(M1);
            MediumGrid.Add(M2);
            MediumGrid.Add(M3);
            MediumGrid.Add(M4);
            MediumGrid.Add(M5);
            MediumGrid.Add(M6);
            MediumGrid.Add(M7);
            MediumGrid.Add(M8);
            MediumGrid.Add(M9);
            MediumGrid.Add(M10);
            MediumGrid.Add(M11);
            MediumGrid.Add(M12);
            MediumGrid.Add(M13);
            MediumGrid.Add(M14);
            MediumGrid.Add(M15);
            MediumGrid.UpdateProps();
        }
        public void LoadFullGrid()
        {
            UOMProperty F1 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "H2");
            UOMProperty F2 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C1");
            UOMProperty F3 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C2");
            UOMProperty F4 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C3");
            UOMProperty F5 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "IC4");
            UOMProperty F6 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "NC4");
            UOMProperty F7 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "IC5");
            UOMProperty F8 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "NC5");
            UOMProperty F9 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "IC6");
            UOMProperty F10 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "H2");
            UOMProperty F11 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "NC6");
            UOMProperty F12 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "CH");
            UOMProperty F13 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "MCP");
            UOMProperty F14 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "BEN");
            UOMProperty F15 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "IC7");
            UOMProperty F16 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "NC7");
            UOMProperty F17 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "MCH");
            UOMProperty F18 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C7CP");
            UOMProperty F19 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "TOL");
            UOMProperty F20 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C8P");
            UOMProperty F21 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "ECH");
            UOMProperty F22 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "DMCH");
            UOMProperty F23 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "PCP");
            UOMProperty F24 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C8CP");
            UOMProperty F25 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "EB");
            UOMProperty F26 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "PX");
            UOMProperty F27 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "MX");
            UOMProperty F28 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "OX");
            UOMProperty F29 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C9 + PEB");
            UOMProperty F30 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C9 + CEB");
            UOMProperty F31 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C9 + CEB");
            UOMProperty F32 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "C9 + AEB");

            FullGrid.Add(F1);
            FullGrid.Add(F2);
            FullGrid.Add(F3);
            FullGrid.Add(F4);
            FullGrid.Add(F5);
            FullGrid.Add(F6);
            FullGrid.Add(F7);
            FullGrid.Add(F8);
            FullGrid.Add(F9);
            FullGrid.Add(F10);
            FullGrid.Add(F11);
            FullGrid.Add(F12);
            FullGrid.Add(F13);
            FullGrid.Add(F14);
            FullGrid.Add(F15);
            FullGrid.Add(F16);
            FullGrid.Add(F17);
            FullGrid.Add(F18);
            FullGrid.Add(F19);
            FullGrid.Add(F20);
            FullGrid.Add(F21);
            FullGrid.Add(F22);
            FullGrid.Add(F23);
            FullGrid.Add(F24);
            FullGrid.Add(F25);
            FullGrid.Add(F26);
            FullGrid.Add(F27);
            FullGrid.Add(F28);
            FullGrid.Add(F29);
            FullGrid.Add(F30);
            FullGrid.Add(F31);
            FullGrid.Add(F32);
            FullGrid.UpdateProps();
        }
        public void LoadReactor()
        {
            UOMProperty R1 = new UOMProperty(ePropID.Mass, SourceEnum.Input, 20, "Catalyst loading (% of total catalyst)");
            UOMProperty R2 = new UOMProperty(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
            UOMProperty R3 = new UOMProperty(ePropID.P, SourceEnum.Input, 4.71, "Inlet Presure");
            UOMProperty R4 = new UOMProperty(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
            UOMProperty R5 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "Metal activity for fresh catalyst (0.1 to 10.0)");
            UOMProperty R6 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "Acid activity for fresh catalyst (0.1 to 10.0)");

            UOMProperty R21 = new UOMProperty(ePropID.Mass, SourceEnum.Input, 30, "Catalyst loading (% of total catalyst)");
            UOMProperty R22 = new UOMProperty(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
            UOMProperty R23 = new UOMProperty(ePropID.P, SourceEnum.Input, 4.21, "Inlet Presure");
            UOMProperty R24 = new UOMProperty(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
            UOMProperty R25 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "Metal activity for fresh catalyst (0.1 to 10.0)");
            UOMProperty R26 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "Acid activity for fresh catalyst (0.1 to 10.0)");

            UOMProperty R31 = new UOMProperty(ePropID.Mass, SourceEnum.Input, 50, "Catalyst loading (% of total catalyst)");
            UOMProperty R32 = new UOMProperty(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
            UOMProperty R33 = new UOMProperty(ePropID.P, SourceEnum.Input, 3.71, "Inlet Presure");
            UOMProperty R34 = new UOMProperty(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
            UOMProperty R35 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "Metal activity for fresh catalyst (0.1 to 10.0)");
            UOMProperty R36 = new UOMProperty(ePropID.T, SourceEnum.Input, double.NaN, "Acid activity for fresh catalyst (0.1 to 10.0)");

            ReactorData.Add(R1);
            ReactorData.Add(R2);
            ReactorData.Add(R3);
            ReactorData.Add(R4);
            ReactorData.Add(R5);
            ReactorData.Add(R6);

            ReactorData.Add(R21);
            ReactorData.Add(R22);
            ReactorData.Add(R23);
            ReactorData.Add(R24);
            ReactorData.Add(R25);
            ReactorData.Add(R26);

            ReactorData.Add(R31);
            ReactorData.Add(R32);
            ReactorData.Add(R33);
            ReactorData.Add(R34);
            ReactorData.Add(R35);
            ReactorData.Add(R36);

            ReactorData.UpdateProps();
        }
        public void LoadRecyle()
        {
            UOMProperty R1 = new UOMProperty(ePropID.Mass, SourceEnum.Input, 2.2, "H2:HC (moles H2 to moles hydrocarbon)");
            UOMProperty R2 = new UOMProperty(ePropID.T, SourceEnum.Input, 38.0, "Separator Temperature");
            UOMProperty R3 = new UOMProperty(ePropID.P, SourceEnum.Input, 2.5, "Separator Pressure");

            RecycleData.Add(R1);
            RecycleData.Add(R2);
            RecycleData.Add(R3);
            RecycleData.UpdateProps();
        }
        public void LoadCatalyst()
        {
            UOMProperty R1 = new UOMProperty(ePropID.Mass, SourceEnum.Input, 2.2, "Units for catalyst amount");
            UOMProperty R2 = new UOMProperty(ePropID.T, SourceEnum.Input, 38.0, "Value in chosen units");
            UOMProperty R3 = new UOMProperty(ePropID.P, SourceEnum.Input, 2.5, "Units for catalyst density");
            UOMProperty R4 = new UOMProperty(ePropID.P, SourceEnum.Input, 2.5, "Catalyst density in chosen units");

            CatalystData.Add(R1);
            CatalystData.Add(R2);
            CatalystData.Add(R3);
            CatalystData.Add(R4);
            CatalystData.UpdateProps();
        }
        public void LoadFurnace()
        {
            UOMProperty R1 = new UOMProperty(ePropID.Mass, SourceEnum.Input, 2.2, "Furnace eff(%)");

            FurnaceEfficiency.Add(R1);
            FurnaceEfficiency.UpdateProps();
        }
        public void LoadNoRx()
        {
            UOMProperty R1 = new UOMProperty(ePropID.Mass, SourceEnum.Input, 3, "Number of Reactors");

            NoReactors.Add(R1);
            NoReactors.UpdateProps();
        }

        private void Solve_Click(object sender, System.EventArgs e)
        {
            DistPoints Feed = new DistPoints();
            Tuple<double, double, double, double> PNAO;
            double[] FeedComponents = new double[31];
            Pressure[] RxP = new Pressure[4];
            RxP[0].kg_cm2_g = 4.71;
            RxP[1].kg_cm2_g = 4.21;
            RxP[2].kg_cm2_g = 3.71;
            RxP[3].kg_cm2_g = double.NaN;

            DeltaPressure[] RxDp = new DeltaPressure[4];
            RxDp[0].kg_cm2 = 0.5;
            RxDp[1].kg_cm2 = 0.5;
            RxDp[2].kg_cm2 = 0.5;
            RxDp[3].kg_cm2 = double.NaN;

            Temperature[] RxT = new Temperature[4];
            RxT[0].Celsius = 510;
            RxT[1].Celsius = 510;
            RxT[2].Celsius = 510;
            RxT[3].Celsius = double.NaN;

            Feed.Add(new DistPoint(0, new Temperature(0, TemperatureUnit.Celsius))); // dummy as reformer arrays have base 1
            Feed.Add(new DistPoint(1, new Temperature(102, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(10, new Temperature(114, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(30, new Temperature(double.NaN, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(50, new Temperature(132, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(70, new Temperature(double.NaN, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(90, new Temperature(160, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(99, new Temperature(178, TemperatureUnit.Celsius)));

            PNAO = Tuple.Create<double, double, double, double>(60.87, 28.34, 10.79, 0.0);

            Density density = 0.747 * 1000;
            MassFlow flow = new MassFlow();
            flow.te_d = 1675.6242;

            NapRef.ReadInput(flow, RxT, FeedComponents, assayBasis, Feed, density, PNAO, (Pressure)Psep.UOM,
                (Temperature)Tsep.UOM, new double[] { 20, 30, 50 }, RxP, RxDp, 1.95, 3, 2.2, InputOption.Short);

            NapRef.SolveCase();

            LoadResults();
            LoadSummaryResults();
            LoadReactorResults();
            LoadRecycleResults();
            LoadFurnaceResults();
            LoadSeparatorResults();
            LoadFeedSummaryResults();
        }

        private void LoadSummaryResults()
        {
            DGVProductData.Clear();
            DGVProductData.Add(NapRef.SumRON, "Separator bottoms RON");
            DGVProductData.Add(NapRef.SumMON, "Separator bottoms MON");
            DGVProductData.Add(NapRef.SumRVP.kg_cm2_g, "Separator bottoms RVP");
            DGVProductData.Add(NapRef.RefC5PlusLV / NapRef.NaphVolFeed * 100, "Separator bottoms LV%");
            DGVProductData.Add(NapRef.RefVolFraction[13], "Benzene in Separator bottoms LV%");

            DGVProductData.UpdateProps();
        }

        private void LoadRecycleResults()
        {
            DGVRecyleResults.Clear();
            DGVRecyleResults.Add(NapRef.SumMol, "Recycle Total Kg-moles/hr");
            DGVRecyleResults.Add(NapRef.SumNM3, "Recycle Total Nm³/m³feed");

            DGVRecyleResults.UpdateProps();
        }

        private void LoadSeparatorResults()
        {
            DGVSeparatorResults.Clear();
            DGVSeparatorResults.Add(NapRef.TSep.Celsius, "Separation Temp (°C)");
            DGVSeparatorResults.Add(NapRef.PSep.kg_cm2_g, "Separation P (kg/cm², gauge)");

            DGVSeparatorResults.UpdateProps();
        }
        private void LoadFeedSummaryResults()
        {
            DGVFeedSummary.Clear();
            DGVFeedSummary.Add(NapRef.NaphMassFeed.te_d / 1000, "Feed Rate (MTPD)");
            DGVFeedSummary.Add(NapRef.LHSV, "LHSV (1/hour)");
            DGVFeedSummary.Add(NapRef.H2HC, "H2:HC");
            DGVFeedSummary.Add(NapRef.P_WT, "Weight % Paraffin");
            DGVFeedSummary.Add(NapRef.N_WT, "Weight % Naphthene");
            DGVFeedSummary.Add(NapRef.A_WT, "Weight % Aromatic");
            DGVFeedSummary.Add(NapRef.SGFeed.SG, "SG");

            DGVFeedSummary.UpdateProps();
        }

        private void LoadMassBalanceResults()
        {
            DGVMassBalance.Clear();
            DGVMassBalance.Add(NapRef.NaphMassFeed.te_hr, "Feed Rate (te/hr)");
            DGVMassBalance.Add(NapRef.LHSV, "LHSV (1/hour)");
            DGVMassBalance.Add(NapRef.H2HC, "H2:HC");
            DGVMassBalance.Add(NapRef.P_WT, "Weight % Paraffin");
            DGVMassBalance.Add(NapRef.N_WT, "Weight % Naphthene");
            DGVMassBalance.Add(NapRef.A_WT, "Weight % Aromatic");
            DGVMassBalance.Add(NapRef.SGFeed.SG, "SG");

            DGVMassBalance.UpdateProps();
        }

        private void LoadFurnaceResults()
        {
            DGVFurnace.Clear();

            for (int i = 1; i <= NapRef.NumReactor; i++)
            {
                DGVFurnace.Add(NapRef.MMBTUPerHrToMW(NapRef.Duty[i-1]), "Reheat Duty (MW)", i - 1);
                DGVFurnace.Add(NapRef.FurnEffic[i], "Efficiency (%)", i - 1);
                DGVFurnace.Add(NapRef.MMBTUPerHrToMW(NapRef.Duty[i-1])/ (NapRef.FurnEffic[i]/100), "Furnace Duty (MW)", i - 1);
            }

            DGVFurnace.UpdateProps();
        }

        private void LoadReactorResults()
        {
            DGVReactor.Clear();

            /* DGVReactor.Add(NapRef.ReactorT_In[1].Celsius, "Inlet Temp (C)");
             DGVReactor.Add(NapRef.ReactorT_Out[1].Celsius, "Outlet Temp (C)");
             DGVReactor.Add(NapRef.ReactorT_Out[1].Celsius - NapRef.ReactorT_In[0].Celsius, "Delta T");
             DGVReactor.Add(NapRef.InletP[1], "Inlet Pressure (kg/cm2g)");
             DGVReactor.Add(NapRef.AmtCat[1], "Catalyst Amount(kg)");
             DGVReactor.Add(NapRef.CatPercent[1], "Catalyst Distribution(%)");*/

            for (int i = 1; i <= NapRef.NumReactor; i++)
            {
                DGVReactor.Add(NapRef.ReactorT_In[i].Celsius, "Inlet Temp (C)", i - 1);
                DGVReactor.Add(NapRef.ReactorT_Out[i].Celsius, "Outlet Temp (C)", i - 1);
                DGVReactor.Add(NapRef.ReactorT_Out[i].Celsius - NapRef.ReactorT_In[i].Celsius, "Delta T", i - 1);
                DGVReactor.Add(NapRef.InletP[i].kg_cm2_g, "Inlet Pressure (kg/cm2g)", i - 1);
                DGVReactor.Add(NapRef.AmtCat[i], "Catalyst Amount(kg)", i - 1);
                DGVReactor.Add(NapRef.CatPercent[i], "Catalyst Distribution(%)", i - 1);
            }

            DGVReactor.UpdateProps();
        }

        public void LoadResults()
        {
            if (NapRef.NumReactor < 4)
                DGVResults1.Columns["Rx4"].Visible = false;

            switch (floworFraction)
            {
                case enumFlowOrFraction.Fraction:
                    break;
            }

            DGVResults1.Rows.Clear();
            int rono;
            DataGridViewRow row;
            for (int i = 1; i <= Names.Count; i++)
            {
                rono = DGVResults1.Rows.Add();
                row = DGVResults1.Rows[rono];

                try
                {
                    row.HeaderCell.Value = Names[i - 1];
                    row.Cells[0].Value = NapRef.MolFeed[i].kgMole_hr.ToString("F2");
                    row.Cells[1].Value = NapRef.F_Recy[i].kgMole_hr.ToString("F2"); 
                    row.Cells[2].Value = NapRef.F_Inlet[i, 1].kgMole_hr.ToString("F2");
                    if (NapRef.Eff[0] != null)
                        row.Cells[3].Value = NapRef.F_Inlet[i, 2].kgMole_hr.ToString("F2");
                    if (NapRef.Eff[1] != null)
                        row.Cells[4].Value = NapRef.F_Inlet[i, 3].kgMole_hr.ToString("F2");
                    if (NapRef.Eff[2] != null)
                        row.Cells[5].Value = NapRef.F_Inlet[i, 4].kgMole_hr.ToString("F2");
                    if (NapRef.Eff[3] != null)
                        row.Cells[6].Value = NapRef.Eff[3][i].kgMole_hr.ToString("F2");

                    row.Cells[7].Value = NapRef.F_Ref[i].kgMole_hr.ToString("F2");
                    row.Cells[8].Value = NapRef.F_NetGas[i].kgMole_hr.ToString("F2");
                }
                catch  // nothing to do
                {

                }
            }
        }

        private void rbVolume_CheckedChanged(object sender, EventArgs e)
        {
            assayBasis = AssayBasis.Volume;
        }

        private void rbMolar_CheckedChanged(object sender, EventArgs e)
        {
            assayBasis = AssayBasis.Molar;
        }

        private void rbMass_CheckedChanged(object sender, EventArgs e)
        {
            assayBasis = AssayBasis.Mass;
        }


        public enum enumFlowOrFraction { Flow, Fraction }
        enumFlowOrFraction floworFraction = enumFlowOrFraction.Fraction;
        flowBasis flowbasis = flowBasis.mass;
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            floworFraction = enumFlowOrFraction.Fraction;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            flowbasis = flowBasis.molar;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            flowbasis = flowBasis.volume;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            flowbasis = flowBasis.mass;
        }
    }
}
