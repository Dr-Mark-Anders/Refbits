using DialogControls;
using ModelEngine;
using NaphthaReformer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Units;
using Units.UOM;

namespace NaphthaReformerSI
{
    public partial class ReformerForm : Form
    {
        private readonly NapReformerSI NapRef = new();
        private ReformerDataCollection datasetcollection = new();
        private readonly int CaseNo = 0;
        private ReformerDataSet dataset = new();

        private readonly UOMPropertyList FeedDistPoints = new();
        private readonly UOMPropertyList FeedPNA = new();

        private readonly List<string> Names = new() {"H2","C1","C2","C3","IC4","NC4","IC5","NC5","IC6","NC6","CH","MCP","BEN",
            "IC7","NC7","MCH","C7CP","TOL","C8P","ECH","DMCH","PCP","C8CP","EB","PX","MX","OX","C9+P","C9+CH","C9+CP","C9+A"};

        private AssayBasis assayBasis = AssayBasis.Volume;

        public ReformerForm()
        {
            InitializeComponent();
            RxData.DGV.Columns[2].HeaderText = "Reactor1";
            RxData.AddColumn("Reactor2");
            RxData.AddColumn("Reactor3");
            RxData.AddColumn("Reactor4");

            FurnaceData.DGV.Columns[2].HeaderText = "Furnace 1";
            FurnaceData.AddColumn("Furnace 2");
            FurnaceData.AddColumn("Furnace 3");

            RxDataResults.DGV.Columns[2].HeaderText = "Rx1";
            RxDataResults.AddColumn("Rx2");
            RxDataResults.AddColumn("Rx3");
            RxDataResults.AddColumn("Rx4");

            datasetcollection.Add(dataset);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataset();
            dataset = datasetcollection[0];
            cBCaseSelection.Items.AddRange(datasetcollection.CaseNames());
            this.cBCaseSelection.SelectedIndexChanged -= new EventHandler(this.CBCaseSelection_SelectedIndexChanged);
            cBCaseSelection.SelectedItem = dataset.Name;
            this.cBCaseSelection.SelectedIndexChanged += new EventHandler(this.CBCaseSelection_SelectedIndexChanged);
        }

        public void LoadDataset()
        {
            SepConditions.Clear();
            SepConditions.Add(dataset.simulationdata.PSep, "Sep. Pressure");
            SepConditions.Add(dataset.simulationdata.TSep, "Sep. Temperature");
            SepConditions.UpdateValues();

            UOMProperty Dist1 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[0].BP, "IBP");
            UOMProperty Dist5 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[1].BP, "5");
            UOMProperty Dist10 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[2].BP, "10");
            UOMProperty Dist30 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[3].BP, "30");
            UOMProperty Dist50 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[4].BP, "50");
            UOMProperty Dist70 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[5].BP, "70");
            UOMProperty Dist90 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[6].BP, "90");
            UOMProperty Dist95 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[7].BP, "95");
            UOMProperty Dist99 = new(ePropID.T, SourceEnum.Input, dataset.simulationdata.feed[8].BP, "99");

            switch (dataset.simulationdata.feedflow.UOM)
            {
                case MassFlow mf:
                    //    txtFeedRate.UOMprop = new (mf);
                    break;

                case MoleFlow molf:
                    //    txtFeedRate.UOMprop = new  UOMProperty(molf);
                    break;

                case VolumeFlow vf:
                    // txtFeedRate.UOMprop = new  UOMProperty(vf);
                    break;
            }

            //txtFeedRate.Value = dataset.flow.BaseValue;
            //pdg.Add(dataset.flow,"Rate");
            FeedDistPoints.Clear();
            FeedDistPoints.Add(Dist1, "");
            FeedDistPoints.Add(Dist5);
            FeedDistPoints.Add(Dist10);
            FeedDistPoints.Add(Dist30);
            FeedDistPoints.Add(Dist50);
            FeedDistPoints.Add(Dist70);
            FeedDistPoints.Add(Dist90);
            FeedDistPoints.Add(Dist95);
            FeedDistPoints.Add(Dist99);
            FeedDistPoints.Add(dataset.simulationdata.feeddensity, "Density");

            LoadPNA();
            LoadShortGC();
            LoadFullGC();
            LoadReactor();
            LoadRecyle();
            LoadCatalyst();
            LoadFurnace();
            LoadNoRx();
            LoadCalibrationData();
        }

        private void LoadCalibrationData()
        {
            CalibFactors.Clear();

            CalibFactors.Add(dataset.simulationdata.calibfactors["CrackP6"], "CrackP6");
            CalibFactors.Add(dataset.simulationdata.calibfactors["CrackP7"], "CrackP7");
            CalibFactors.Add(dataset.simulationdata.calibfactors["CrackP8"], "CrackP8");
            CalibFactors.Add(dataset.simulationdata.calibfactors["DealkA6"], "DealkA6");
            CalibFactors.Add(dataset.simulationdata.calibfactors["DealkA7"], "DealkA7");
            CalibFactors.Add(dataset.simulationdata.calibfactors["DealkA8"], "DealkA8");
            CalibFactors.Add(dataset.simulationdata.calibfactors["CycleP6"], "CycleP6");
            CalibFactors.Add(dataset.simulationdata.calibfactors["CycleP7"], "CycleP7");
            CalibFactors.Add(dataset.simulationdata.calibfactors["CycleP8"], "CycleP8");
            CalibFactors.Add(dataset.simulationdata.calibfactors["CycleP9"], "CycleP9");
            CalibFactors.Add(dataset.simulationdata.calibfactors["DehydN6"], "DehydN6");
            CalibFactors.Add(dataset.simulationdata.calibfactors["DehydN7"], "DehydN7");
            CalibFactors.Add(dataset.simulationdata.calibfactors["DehydN8"], "DehydN8");
            CalibFactors.Add(dataset.simulationdata.calibfactors["DehydN9"], "DehydN9");
            CalibFactors.Add(dataset.simulationdata.calibfactors["EquilCyc9"], "EquilCyc9");
            CalibFactors.UpdateValues();
        }

        public void LoadPNA()
        {
            FeedPNA.Clear();
            FeedPNA.Add(dataset.simulationdata.feedPNAO.Item1, "Paraffins");
            FeedPNA.Add(dataset.simulationdata.feedPNAO.Item2, "Naphthenes");
            FeedPNA.Add(dataset.simulationdata.feedPNAO.Item3, "Aromatics");
            FeedPNA.Add(dataset.simulationdata.feedPNAO.Item4, "Olefins");
        }

        public void LoadShortGC()
        {
        }

        public void LoadFullGC()
        {
        }

        public void LoadReactor()
        {
            RxData.Clear();

            RxData.Add(dataset.simulationdata.R1CatLoad, "Cat Load");
            RxData.Add(dataset.simulationdata.R1Tin, "T in");
            RxData.Add(dataset.simulationdata.R1Pin, "P in");
            RxData.Add(dataset.simulationdata.R1PDrop, "P Drop");
            RxData.Add(dataset.simulationdata.R1MetActivity, "Met Activity");
            RxData.Add(dataset.simulationdata.R1AcidActivity, "Acid Activity");

            RxData.Add(dataset.simulationdata.R2CatLoad, "", 1);
            RxData.Add(dataset.simulationdata.R2Tin, "", 1);
            RxData.Add(dataset.simulationdata.R2Pin, "", 1);
            RxData.Add(dataset.simulationdata.R2PDrop, "", 1);
            RxData.Add(dataset.simulationdata.R2MetActivity, "", 1);
            RxData.Add(dataset.simulationdata.R2AcidActivity, "", 1);

            RxData.Add(dataset.simulationdata.R3CatLoad, "", 2);
            RxData.Add(dataset.simulationdata.R3Tin, "", 2);
            RxData.Add(dataset.simulationdata.R3Pin, "", 2);
            RxData.Add(dataset.simulationdata.R3PDrop, "", 2);
            RxData.Add(dataset.simulationdata.R3MetActivity, "", 2);
            RxData.Add(dataset.simulationdata.R3AcidActivity, "", 2);

            //RxData.UpdateValues();
        }

        public void LoadRecyle()
        {
            RecData.Clear();
            RecData.Add(dataset.simulationdata.H2HC, "H2HC Ratio");
            RecData.Add(dataset.simulationdata.TSep, "Sep T");
            RecData.Add(dataset.simulationdata.PSep, "Sep P");
            RecData.UpdateValues();
        }

        public void LoadCatalyst()
        {
            CatData.Clear();
            CatData.Add(dataset.simulationdata.CatAmount, "Cat Amount");
            CatData.Add(dataset.simulationdata.CatDensity, "Cat Density");
            CatData.UpdateValues();
        }

        public void LoadFurnace()
        {
            FurnEff.Clear();

            UOMProperty R1 = new(ePropID.Mass, SourceEnum.Input, 2.2, "Furnace eff(%)");

            FurnEff.Add(R1, "Furnace eff(%)");
            FurnEff.UpdateValues();
        }

        public void LoadNoRx()
        {
            NoRx.Clear();
            UOMProperty R1 = new(ePropID.Mass, SourceEnum.Input, 3, "Number of Reactors");

            NoRx.Add(R1, "Number of Reactors");
            NoRx.UpdateValues();
        }

        private void SolveCase_Click(object sender, EventArgs e)
        {
            double[] FeedComponents = new double[31];
            ReformerDataSet dataset = datasetcollection[CaseNo];

            NapRef.ReadInput(dataset.simulationdata.feedflow.UOM, dataset.simulationdata.RxT, FeedComponents, assayBasis, dataset.simulationdata.feed,
                dataset.simulationdata.feeddensity.BaseValue, dataset.simulationdata.feedPNAO,
                dataset.simulationdata.PSep.BaseValue, dataset.simulationdata.TSep.BaseValue,
                dataset.simulationdata.CatAmt, dataset.simulationdata.RxP, dataset.simulationdata.RxDp, dataset.simulationdata.LHSV,
                dataset.simulationdata.NoRx, dataset.simulationdata.H2HC, InputOption.Short);

            NapRef.SolveCase();

            LoadResults();
            LoadSummaryResults();
            LoadReactorResults();
            LoadRecycleResults();
            LoadFurnaceResults();
            LoadSeparatorResults();
            LoadFeedSummaryResults();
        }

        private void SolveOld_Click(object sender, System.EventArgs e)
        {
            DistPoints Feed = new();
            Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> PNAO;
            double[] FeedComponents = new double[31];
            Pressure Psep = new(3.5);
            Temperature Tsep = new(25, TemperatureUnit.Celsius);

            List<Pressure> RxP = new(4);
            RxP.Add(new Pressure(4.71, PressureUnit.Kg_cm2));
            RxP.Add(new Pressure(4.21, PressureUnit.Kg_cm2));
            RxP.Add(new Pressure(3.71, PressureUnit.Kg_cm2));

            DeltaPressure[] RxDp = new DeltaPressure[4];
            RxDp[0].kg_cm2 = 0.5;
            RxDp[1].kg_cm2 = 0.5;
            RxDp[2].kg_cm2 = 0.5;
            RxDp[3].kg_cm2 = double.NaN;

            List<UOMProperty> RxT = new(4)
            {
                new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)),
                new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)),
                new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)),
                new UOMProperty(new Temperature(double.NaN))
            };

            //Feed.Add(new  DistPoint (0, new  Temperature (0, TemperatureUnit.Celsius))); // dummy as reformer arrays have base 1
            Feed.Add(new DistPoint(1, new Temperature(102, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(10, new Temperature(114, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(30, new Temperature(double.NaN, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(50, new Temperature(132, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(70, new Temperature(double.NaN, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(90, new Temperature(160, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(99, new Temperature(178, TemperatureUnit.Celsius)));

            PNAO = Tuple.Create<UOMProperty, UOMProperty, UOMProperty, UOMProperty>(new UOMProperty(ePropID.Value, 60.87),
                new UOMProperty(ePropID.Value, 28.34),
                new UOMProperty(ePropID.Value, 10.79),
                new UOMProperty(ePropID.Value, 0.0));

            Density density = 0.747 * 1000;
            MassFlow flow = new();
            flow.te_d = 1675.6242;

            NapRef.ReadInput(flow, RxT, FeedComponents, assayBasis, Feed, density, PNAO, Psep,
                Tsep, new List<UOMProperty> {
                    new UOMProperty(ePropID.Mass, 20),
                    new UOMProperty(ePropID.Mass, 30),
                    new UOMProperty(ePropID.Mass, 50)}, RxP, RxDp.ToList(), 1.95, 3, 2.2, InputOption.Short);

            NapRef.SolveCase();

            LoadResults();
            LoadSummaryResults();
            LoadReactorResults();
            LoadRecycleResults();
            LoadFurnaceResults();
            LoadSeparatorResults();
            LoadFeedSummaryResults();

            DoCharts();
        }

        private void LoadSummaryResults()
        {
            ProductData.Clear();
            ProductData.Add(NapRef.SumRON, "Separator bottoms RON");
            ProductData.Add(NapRef.SumMON, "Separator bottoms MON");
            ProductData.Add(NapRef.SumRVP.kg_cm2_g, "Separator bottoms RVP");
            ProductData.Add(NapRef.RefC5PlusLV / NapRef.NaphVolFeed * 100, "Separator bottoms LV%");
            ProductData.Add(NapRef.RefVolFraction[13], "Benzene in Separator bottoms LV%");

            ProductData.UpdateValues();
        }

        private void LoadRecycleResults()
        {
            RecData.Clear();
            RecData.Add(NapRef.SumMol, "Recycle Total Kg-moles/hr");
            RecData.Add(NapRef.SumNM3, "Recycle Total Nm³/m³feed");

            RecData.UpdateValues();
        }

        private void LoadSeparatorResults()
        {
            SeparatorResults.Clear();
            SeparatorResults.Add(NapRef.TSep.Celsius, "Separation Temp (°C)");
            SeparatorResults.Add(NapRef.PSep.kg_cm2_g, "Separation P (kg/cm², gauge)");

            SeparatorResults.UpdateValues();
        }

        private void LoadFeedSummaryResults()
        {
            FeedSummary.Clear();
            FeedSummary.Add(NapRef.NaphMassFeed.te_d / 1000, "Feed Rate (MTPD)");
            FeedSummary.Add(NapRef.LHSV, "LHSV (1/hour)");
            FeedSummary.Add(NapRef.H2HC, "H2:HC");
            FeedSummary.Add(NapRef.P_WT, "Weight % Paraffin");
            FeedSummary.Add(NapRef.N_WT, "Weight % Naphthene");
            FeedSummary.Add(NapRef.A_WT, "Weight % Aromatic");
            FeedSummary.Add(NapRef.SGFeed.SG, "SG");

            FeedSummary.UpdateValues();
        }

        private void LoadMassBalanceResults()
        {
            MassBalance.Clear();
            MassBalance.Add(NapRef.NaphMassFeed.te_hr, "Feed Rate (te/hr)");
            MassBalance.Add(NapRef.LHSV, "LHSV (1/hour)");
            MassBalance.Add(NapRef.H2HC, "H2:HC");
            MassBalance.Add(NapRef.P_WT, "Weight % Paraffin");
            MassBalance.Add(NapRef.N_WT, "Weight % Naphthene");
            MassBalance.Add(NapRef.A_WT, "Weight % Aromatic");
            MassBalance.Add(NapRef.SGFeed.SG, "SG");

            MassBalance.UpdateValues();
        }

        private void LoadFurnaceResults()
        {
            FurnaceData.Clear();

            for (int i = 0; i < NapRef.NumReactor; i++)
            {
                FurnaceData.Add(NapRef.MMBTUPerHrToMW(NapRef.Duty[i]), "Reheat Duty (MW)", i);
                FurnaceData.Add(NapRef.FurnEffic[i], "Efficiency (%)", i);
                FurnaceData.Add(NapRef.MMBTUPerHrToMW(NapRef.Duty[i]) / (NapRef.FurnEffic[i] / 100), "Furnace Duty (MW)", i);
            }

            FurnaceData.UpdateValues();
        }

        private void LoadReactorResults()
        {
            RxDataResults.Clear();

            /* DGVReactor.Add(NapRef.ReactorT_In[1].Celsius, "Inlet Temp (C)");
             DGVReactor.Add(NapRef.ReactorT_Out[1].Celsius, "Outlet Temp (C)");
             DGVReactor.Add(NapRef.ReactorT_Out[1].Celsius - NapRef.ReactorT_In[0].Celsius, "Delta T");
             DGVReactor.Add(NapRef.InletP[1], "Inlet Pressure  (kg/cm2g)");
             DGVReactor.Add(NapRef.AmtCat[1], "Catalyst Amount(kg)");
             DGVReactor.Add(NapRef.CatPercent[1], "Catalyst Distribution(%)");*/

            for (int i = 0; i < NapRef.NumReactor; i++)
            {
                RxDataResults.Add(NapRef.ReactorT_In[i].Celsius, "Inlet Temp (C)", i);
                RxDataResults.Add(NapRef.ReactorT_Out[i].Celsius, "Outlet Temp (C)", i);
                RxDataResults.Add(NapRef.ReactorT_Out[i].Celsius - NapRef.ReactorT_In[i].Celsius, "Delta T", i);
                RxDataResults.Add(NapRef.InletP[i].kg_cm2_g, "Inlet Pressure (kg/cm2g)", i);
                RxDataResults.Add(NapRef.AmtCat[i], "Catalyst Amount(kg)", i);
                RxDataResults.Add(NapRef.CatPercent[i], "Catalyst Distribution(%)", i);
            }

            RxDataResults.UpdateValues();
        }

        public void LoadResults()
        {
            if (NapRef.NumReactor < 4)
                DGVResults1.Columns["Rx4"].Visible = false;

            switch (floworFraction)
            {
                case EnumFlowOrFraction.Fraction:
                    break;
            }

            DGVResults1.Rows.Clear();
            int rono;
            DataGridViewRow row;
            for (int i = 0; i < Names.Count; i++)
            {
                rono = DGVResults1.Rows.Add();
                row = DGVResults1.Rows[rono];

                try
                {
                    row.HeaderCell.Value = Names[i];
                    row.Cells[0].Value = NapRef.MolFeed[i].ToString("F2");
                    row.Cells[1].Value = NapRef.F_Recy[i].ToString("F2"); ;
                    row.Cells[2].Value = NapRef.F_Inlet[i, 0].ToString("F2");
                    if (NapRef.Eff[0] != null)
                        row.Cells[3].Value = NapRef.F_Inlet[i, 1].ToString("F2");
                    if (NapRef.Eff[1] != null)
                        row.Cells[4].Value = NapRef.F_Inlet[i, 2].ToString("F2");
                    if (NapRef.Eff[2] != null)
                        row.Cells[5].Value = NapRef.F_Inlet[i, 3].ToString("F2");
                    if (NapRef.Eff[3] != null)
                        row.Cells[6].Value = NapRef.Eff[2][i].ToString("F2");

                    row.Cells[7].Value = NapRef.F_Ref[i].ToString("F2"); ;
                    row.Cells[8].Value = NapRef.F_NetGas[i].ToString("F2");
                }
                catch
                {
                }
            }
        }

        private void RbVolume_CheckedChanged(object sender, EventArgs e)
        {
            assayBasis = AssayBasis.Volume;
        }

        private void RbMolar_CheckedChanged(object sender, EventArgs e)
        {
            assayBasis = AssayBasis.Molar;
        }

        private void RbMass_CheckedChanged(object sender, EventArgs e)
        {
            assayBasis = AssayBasis.Mass;
        }

        public enum EnumFlowOrFraction
        { Flow, Fraction }

        private readonly EnumFlowOrFraction floworFraction = EnumFlowOrFraction.Fraction;
        private readonly List<double[]> SortedProfile1 = new();
        private readonly List<double[]> SortedProfile2 = new();
        private readonly List<double[]> SortedProfile3 = new();
        private readonly List<double[]> SortedProfile4 = new();

        /*     private  void   SortProfiles()
             {
                 double [] res = new  double [NapRef.Profile1.Count];
                 for (int  i = 0; i < NapRef.Names.Count(); i++)
                 {
                     SortedProfile1.Add((double [])res.Clone());
                     for (int  j = 0; j < NapRef.Profile1.Count; j++)
                     {
                         SortedProfile1[i][j] = NapRef.Profile1[j][i];
                     }
                     SortedProfile1[i].Normalise();
                 }

                 for (int  i = 0; i < NapRef.Names.Count(); i++)
                 {
                     SortedProfile2.Add((double [])res.Clone());
                     for (int  j = 0; j < NapRef.Profile2.Count; j++)
                     {
                         SortedProfile2[i][j] = NapRef.Profile2[j][i];
                     }
                     SortedProfile2[i].Normalise();
                 }

                 for (int  i = 0; i < NapRef.Names.Count(); i++)
                 {
                     SortedProfile3.Add((double [])res.Clone());
                     for (int  j = 0; j < NapRef.Profile3.Count; j++)
                     {
                         SortedProfile3[i][j] = NapRef.Profile3[j][i];
                     }
                     SortedProfile3[i].Normalise();
                 }
             }
        */

        private void DoCharts()
        {
            DGVChartComponents.Rows.Clear();

            foreach (var item in NapRef.Names)
            {
                int row = DGVChartComponents.Rows.Add();
                DGVChartComponents.Rows[row].Cells[1].Value = item.ToString();
            }

            PlotChart();
        }

        private void PlotChart()
        {
            //Set the titles and axis labels
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add("Main");
            chart1.Titles[0].Text = "Concentration Profile";
            //chart.ChartAreas[0].AxisX.;


           /* chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add("Crude Assay Chart");

            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add(new ChartArea());
            chart1.ChartAreas[0].AxisX.Name = "Cumulative LV%";
            chart1.ChartAreas[0].AxisY.Name = "TBP Deg C";

            //chart1.
            chart1.ChartAreas[0].AxisX.Maximum = 100;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 850;
            chart1.ChartAreas[0].AxisY.Minimum = 0;

            Series series1 = chart1.Series.Add("Calculated LV%");
            series1.ChartType = SeriesChartType.Line;*/

            



           /* series1.LegendText = "Lab LV%";
            series1.Color = Color.Blue;
            series1.MarkerStyle = MarkerStyle.Diamond;
            series1.MarkerSize = 3;*/


            Title T2 = chart1.Titles.Add("X");
            T2.Text = "% Bed";
            T2.Docking = Docking.Bottom;

            Title T3 = chart1.Titles.Add("Y");
            T3.Text = "Concentration";
            T3.Docking = Docking.Left;

            Series s1 = chart1.Series.Add("RX1");
            s1.ChartType = SeriesChartType.Point;

            Series s2 = chart1.Series.Add("RX2");
            s2.ChartType = SeriesChartType.Point;

            Series s3 = chart1.Series.Add("RX3");
            s3.ChartType = SeriesChartType.Point;

            foreach (var item in ClickedRow)
            {
                for (int j = 0; j < NapRef.dWarray1.Count; j++)
                {
                    DataPointCollection list = s1.Points;
                    double y = NapRef.Profile1[item][j];
                    double x = NapRef.dWarray1[j] * 100;
                    if (!double.IsNaN(x))
                        list.AddXY(x, y);
                }

                for (int j = 0; j < NapRef.dWarray2.Count; j++)
                {
                    DataPointCollection list = s2.Points;
                    double y = NapRef.Profile2[item][j];
                    double x = NapRef.dWarray2[j] * 100+100;
                    if (!double.IsNaN(x))
                        list.AddXY(x, y);
                }

                for (int j = 0; j < NapRef.dWarray3.Count; j++)
                {
                    DataPointCollection list = s3.Points;
                    double y = NapRef.Profile3[item][j];
                    double x = NapRef.dWarray3[j] * 100+200;
                    if (!double.IsNaN(x))
                        list.AddXY(x, y);
                }
            }
        }

        private void PlotChartOld()
        {
            /*      //chart1.Series.Clear();
                  Series s1, s2, s3;
                  chart1.Series.Clear();

                 // chart1.Titles.Add(new  Title("kg.moles/hr", Docking.Left, new  Font(FontFamily.GenericSansSerif,12f),Color.Black));
                 // chart1.Titles.Add(new  Title("Bed % (Bed1, Bed2, Bed3...)", Docking.Bottom, new  Font(FontFamily.GenericSansSerif, 12f), Color.Black));
                  TickMark t = new  TickMark();
                  //t.int erval =
                      //chart1.

                  foreach (var item in ClickedRow)
                  {
                      s1 = new  Series("Rx1, Comp: " + NapRef.Names[item].ToString());
                      if (!chart1.Series.Contains(s1))
                          chart1.Series.Add(s1);

                      s2 = new  Series("Rx2, Comp: " + NapRef.Names[item].ToString());
                      if (!chart1.Series.Contains(s2))
                          chart1.Series.Add(s2);

                      s3 = new  Series("Rx3, Comp: " + NapRef.Names[item].ToString());
                      if (!chart1.Series.Contains(s3))
                          chart1.Series.Add(s3);

                      s1.ChartType = SeriesChartType.Spline;
                      s2.ChartType = SeriesChartType.Spline;
                      s3.ChartType = SeriesChartType.Spline;

                      if (NapRef.Profile1.Length > 0)
                      {
                          //NapRef.Profile1[i].Normalise();
                          for (int  j = 0; j < NapRef.dWarray1.Count; j++)
                              s1.Point s.AddXY(NapRef.dWarray1[j], NapRef.Profile1[item][j]);
                      }

                      if (NapRef.Profile2.Length > 0)
                      {
                          //NapRef.Profile2[i].Normalise();
                          for (int  j = 0; j < NapRef.dWarray2.Count; j++)
                              s2.Point s.AddXY(1 + NapRef.dWarray2[j], NapRef.Profile2[item][j]);
                      }

                      if (NapRef.Profile3.Length > 0)
                      {
                          //NapRef.Profile3[i].Normalise();
                          for (int  j = 0; j < NapRef.dWarray3.Count; j++)
                              s3.Point s.AddXY(2 + NapRef.dWarray3[j], NapRef.Profile3[item][j]);
                      }
                  }*/
        }

        private List<int> ClickedRow = new();

        private void DGVChartComponents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            ClickedRow.Clear();

            DataGridViewCheckBoxCell cell;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn &&
                e.RowIndex >= 0)
            {
                foreach (DataGridViewRow item in DGVChartComponents.Rows)
                {
                    cell = (DataGridViewCheckBoxCell)item.Cells[e.ColumnIndex];
                    if ((bool)cell.EditingCellFormattedValue)
                    {
                        ClickedRow.Add(cell.RowIndex);
                    }
                }
            }

            PlotChart();
        }

        private void MassMolarVol1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void CBCaseSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataset = datasetcollection.dataset((string)cBCaseSelection.SelectedItem);
            LoadDataset();
        }

        private void BtDeleteCase_Click(object sender, EventArgs e)
        {
            if (dataset.Name != "Default")
            {
                datasetcollection.Delete(dataset.Name);

                cBCaseSelection.Items.Clear();

                foreach (var item in datasetcollection)
                {
                    cBCaseSelection.Items.Add(item.Name);
                }

                cBCaseSelection.Text = (string)cBCaseSelection.Items[0];
                cBCaseSelection.SelectedItem = dataset.Name;
            }
        }

        private void CloneDatset_Click(object sender, EventArgs e)
        {
            ReformerDataSet NewSet = dataset.Clone();
            NewSet.Name = dataset.Name + "_1";
            datasetcollection.Add(NewSet);

            cBCaseSelection.Items.Clear();

            foreach (var item in datasetcollection)
            {
                cBCaseSelection.Items.Add(item.Name);
            }
        }

        private void Feed_Click(object sender, EventArgs e)
        {
            switch (shortMediumFull1.Value)
            {
                case enumShortMediumFull.Short:
                    Distillations dist = new(dataset.simulationdata.feed, dataset.simulationdata.feedPNAO);
                    dist.ShowDialog();
                    break;

                case enumShortMediumFull.Medium:
                    RefShortFeed shortgc = new(dataset.simulationdata.shortgc);
                    shortgc.ShowDialog();
                    break;

                case enumShortMediumFull.Full:
                    RefFullFeed fullfd = new(dataset.simulationdata.fullgc);
                    fullfd.ShowDialog();
                    break;

                default:
                    break;
            }
        }

        private void RbMedium_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SerializeNow();
        }

        public void SerializeNow()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new();
            BinaryFormatter b = new();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    b.Serialize(myStream, datasetcollection);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new();
            BinaryFormatter b = new();

            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    datasetcollection = (ReformerDataCollection)b.Deserialize(myStream);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }

            dataset = datasetcollection[0];
            LoadDataset();
            cBCaseSelection.Items.Clear();

            foreach (var item in datasetcollection)
            {
                if (item.Name != null)
                    cBCaseSelection.Items.Add(item.Name);
            }

            //cBCase Selection.Text = (string)cBCase Selection.Items[0];
            cBCaseSelection.SelectedItem = dataset.Name;
        }

        private void CalibFactors_Load(object sender, EventArgs e)
        {
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            Calibration_Data data = new();
            data.LoadData(dataset);
            data.ShowDialog();
        }

        private void propertyDisplayGrid24_Load(object sender, EventArgs e)
        {
        }

        private void FurnaceData2_Load(object sender, EventArgs e)
        {
        }

        private void FurnEff1_Load(object sender, EventArgs e)
        {
        }

        private void NoRx1_Load(object sender, EventArgs e)
        {
        }

        private void RecData1_Load(object sender, EventArgs e)
        {
        }

        private void btnNewCase_Click(object sender, EventArgs e)
        {
        }

        private void Feed_Click_1(object sender, EventArgs e)
        {
        }
    }
}