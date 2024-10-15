using Extensions;
using Math2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public partial class GenericAssayForm : Form
    {
        private readonly CreateAssayClass CreateAssay;
        private readonly List<double> CutPoints = new();
        private readonly string filelocation = "C:\\Users\\andersm\\Documents\\Visual Studio 2017\\Projects\\Assays";
        private readonly Port_Material port;
        private readonly AssayPropertyCollection apc;
        private string assayname;
        private enumAssayType assaytype = enumAssayType.Assay;
        private Components crude;
        private bool optimiseassayFit = false;
        private readonly DataArrays data;
        private enumAssayPCProperty prop;

        // make default list
        private bool regenerated = false;

        private GenericSynthesis syn;

        public GenericAssayForm(Port_Material port, DataArrays data, AssayPropertyCollection apc, Components crude, string assayname)
        {
            this.port = port;
            this.data = data;
            this.apc = apc;
            this.crude = crude;
            regenerated = false;
            InitializeComponent();
            SetGridViewSortState(dgvGenAssay, DataGridViewColumnSortMode.NotSortable);
            this.assayname = assayname;
            splitContainer2.SplitterDistance = 1000;
            CreateAssay = new CreateAssayClass(dgvGenAssay, data, assaytype);
            CBXoptimiseAssayFit.Checked = optimiseassayFit;
        }

        public string AssayName
        {
            get { return assayname; }
            set { assayname = value; }
        }

        public Components Crude
        {
            get { return crude; }
            set { crude = value; }
        }

        public bool Regenerated
        {
            get { return regenerated; }
            set { regenerated = value; }
        }

        /// <summary>
        /// Sets the sort mode for the data grid view by setting the sort mode of individual columns
        /// </summary>
        /// <param name="dgv">Data Grid View</param>
        /// <param name="sortMode">Sort node of type DataGridViewColumnSortMode</param>
        public static void SetGridViewSortState(DataGridView dgv, DataGridViewColumnSortMode sortMode)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
                col.SortMode = sortMode;
        }

        public void AddChartRawLVPCTS()
        {
            if (data is null)
                return;

            if (crude == null)
                return;

            Series series = null;

            /*    for (int  i = 0; i < ChartLVPCT...d..Series.Count; i++)
                {
                    if (ChartLVPCT.Series[i].Name == "Lab LV%")
                    {
                        series = ChartLVPCT.Series["Lab LV%"];
                        break;
                    }
                }*/

            if (series is null)
            {
                //    series = chartControl1.Series.Add(("Lab LV%"));
            }

            double[] cumlppcts = data.StreamFlowsVol.CumulateArray().ToArray();

            for (int stream = 0; stream < data.StreamNames.Count; stream++)
            {
                double y = data.TBPCutPointsC[stream + 1].Celsius;
                double x = cumlppcts[stream] * 100;
                if (series is not null)
                    series.Points.Add(new DataPoint(x, y));
            }
            if (series is not null)
            {
                series.Color = Color.Red;
                series.MarkerStyle = MarkerStyle.Triangle;
                series.ChartType = SeriesChartType.Point;
                series.MarkerSize = 12;
            }
        }

        public void AddComponent(enumCommonPureComps prop)
        {
            DataGridViewRow dr;
            int r;
            DataGridViewComboBoxCell c;

            dr = new DataGridViewRow();
            dr.CreateCells(dgvGenAssay);
            r = dgvGenAssay.Rows.Add(dr);
            c = new();
            c.DataSource = Enum.GetValues(typeof(enumCommonPureComps));
            c.ValueType = typeof(enumCommonPureComps);
            c.Value = prop;
            dgvGenAssay[0, r] = c;
        }

        public void AddComponent(enumCommonPureComps prop, int row, int column)
        {
            DataGridViewComboBoxCell c = new();
            c.DataSource = Enum.GetValues(typeof(enumCommonPureComps));
            c.ValueType = typeof(enumCommonPureComps);
            c.Value = prop;
            dgvGenAssay[column, row] = c;
        }

        public void AddFlowBasis(enumMassVolMol prop, int r)
        {
            DataGridViewComboBoxCell c = new();
            c.DataSource = Enum.GetValues(typeof(enumMassVolMol));
            c.ValueType = typeof(enumMassVolMol);
            c.Value = prop;
            dgvGenAssay[1, r] = c;
        }

        public int AddProperty(enumAssayPCProperty prop)
        {
            DataGridViewRow dr;
            int r;
            DataGridViewComboBoxCell c;

            dr = new DataGridViewRow();
            dr.CreateCells(dgvGenAssay);
            r = dgvGenAssay.Rows.Add(dr);
            c = new DataGridViewComboBoxCell();

            string text = apc[prop].Name;

            //c.DataSource = Enum.GetValues(typeof(enumAssayPCProperty));
            c.DataSource = apc.Names();
            c.ValueType = typeof(enumAssayPCProperty);
            //c.Value = prop;
            c.Value = text;
            dgvGenAssay[0, r] = c;
            return r;
        }

        public void AddProperty(enumAssayPCProperty prop, int row, int column)
        {
            DataGridViewComboBoxCell c;
            c = new DataGridViewComboBoxCell();
            if (apc[prop] != null)
            {
                string text = apc[prop].Name;
                //c.DataSource = Enum.GetValues(typeof(enumAssayPCProperty));
                c.DataSource = apc.Names();
                c.ValueType = typeof(string);
                //c.Value = prop;
                c.Value = text;
                dgvGenAssay[column, row] = c;
            }
        }

        public int AddProperty(enumDistType prop)
        {
            DataGridViewRow dr = new();
            dr.CreateCells(dgvGenAssay);
            int r = dgvGenAssay.Rows.Add(dr);
            DataGridViewComboBoxCell c = new();
            c.DataSource = Enum.GetValues(typeof(enumDistType));
            c.ValueType = typeof(enumDistType);
            c.Value = prop;
            dgvGenAssay[0, r] = c;
            return r;
        }

        public void AddProperty(enumPCTType prop, int row, int column)
        {
            DataGridViewComboBoxCell c = new();
            c.DataSource = Enum.GetValues(typeof(enumPCTType));
            c.ValueType = typeof(enumPCTType);
            c.Value = prop;
            dgvGenAssay[column, row] = c;
        }

        public void AddProperty(enumTemp prop, int row, int column)
        {
            DataGridViewComboBoxCell c = new();
            c.DataSource = Enum.GetValues(typeof(enumTemp));
            c.ValueType = typeof(enumTemp);
            c.Value = prop;
            dgvGenAssay[column, row] = c;
        }

        public void AddProperty(enumMassVolMol prop, int row, int column)
        {
            DataGridViewComboBoxCell c = new();
            c.DataSource = Enum.GetValues(typeof(enumMassVolMol));
            c.ValueType = typeof(enumMassVolMol);
            c.Value = prop;
            dgvGenAssay[column, row] = c;
        }

        public void AddProperty(enumDistType prop, int row, int column)
        {
            DataGridViewComboBoxCell c = new();
            c.DataSource = Enum.GetValues(typeof(enumDistType));
            c.ValueType = typeof(enumDistType);
            c.Value = prop;
            dgvGenAssay[column, row] = c;
        }

        public void AddText(string prop)
        {
            DataGridViewRow dr = new();
            dr.CreateCells(dgvGenAssay);
            int r = dgvGenAssay.Rows.Add(dr);
            DataGridViewComboBoxCell c = new();
            c.DataSource = Enum.GetValues(typeof(enumPCTType));
            c.ValueType = typeof(enumPCTType);
            c.Value = prop;
            dgvGenAssay[0, r] = c;
        }

        public void AddValue(int r, string val)
        {
            if (dgvGenAssay.Rows.Count > r)
                dgvGenAssay[1, r].Value = val;
            else
            {
                DataGridViewRow dr;
                dr = new DataGridViewRow();
                dr.CreateCells(dgvGenAssay);
                r = dgvGenAssay.Rows.Add(dr);
                dgvGenAssay[0, r].Value = val;
            }
        }

        public void ChartAddLine(double X1, double X2, double Y1, double Y2)
        {
            if (crude == null)
                return;

            Series series1 = chart1.Series.Add("");
            series1.ChartType = SeriesChartType.Line;

            DataPointCollection list = series1.Points;
            series1.MarkerStyle = MarkerStyle.None;
            series1.MarkerSize = 2;
            series1.Color = Color.Black;
            series1.IsVisibleInLegend = false;

            list.AddXY(X1, Y1);
            list.AddXY(X2, Y2);
        }

        public void ChartProductCurvesTBP()
        {
            if (crude is null || data is null)
                return;

            List<Color> colors = new();
            colors.Add(Color.Red);
            colors.Add(Color.Green);
            colors.Add(Color.LimeGreen);
            colors.Add(Color.Blue);
            colors.Add(Color.Orange);
            colors.Add(Color.Magenta);
            colors.Add(Color.Purple);
            colors.Add(Color.Cyan);
            colors.Add(Color.DarkCyan);
            colors.Add(Color.DarkBlue);
            colors.Add(Color.DarkGreen);
            colors.Add(Color.DarkRed);
            colors.Add(Color.DarkSalmon);
            colors.Add(Color.DarkOrange);
            double StreamFlow;

            // Get a reference to the GraphPane instance in the ZedGraphControl
            //GraphPane myPane = zg1.GraphPane;
            double CumLV = data.StreamFlowsFinalVol[0];

            for (int stream = 0; stream < data.StreamNames.Count; stream++)
            {
                Series series1 = null;

                for (int i = 0; i < chart1.Series.Count; i++)
                {
                    if (chart1.Series[i].Name == data.StreamNames[stream])
                    {
                        series1 = chart1.Series[data.StreamNames[stream]];
                        break;
                    }
                }

                if (series1 is null)
                    series1 = chart1.Series.Add(data.StreamNames[stream]);

                series1.ChartType = SeriesChartType.Line;
                series1.IsValueShownAsLabel = false;
                series1["LabelStyle"] = "Center";

                DataPointCollection list = series1.Points;
                list.Clear();
                StreamFlow = data.StreamFlowsFinalVol[stream] * 100; // fraction to percent

                switch (data.streamCompType[stream])
                {
                    case enumStreamComponentType.Mixed: // only do streams with distillation curves input
                    case enumStreamComponentType.Pseudo:
                        DistPoints strC = data.TBPdistPoints[stream];

                        for (int c = 0; c < strC.Count; c++)
                        {
                            double y = strC[c].BP.Celsius;
                            double x = strC[c].Pct / 100D * StreamFlow + CumLV;
                            list.AddXY(x, y);
                        }
                        series1.MarkerColor = Color.Blue;
                        series1.MarkerSize = 2;
                        break;
                }
                CumLV += StreamFlow;
            }
        }

        public void ChartPropertyvsBP()
        {
            chartControl2.Titles.Clear();
            chartControl2.Titles.Add("Property vs. BP (C)");
            chartControl2.Series.Clear();

            chartControl2.ChartAreas.Clear();
            chartControl2.ChartAreas.Add(new ChartArea());
            chartControl2.ChartAreas[0].AxisX.Title = "NBP C";
            chartControl2.ChartAreas[0].AxisY.Title = "Property";

            Series series1 = chartControl2.Series.Add("Calculated");
            series1.ChartType = SeriesChartType.Line;
            series1.MarkerStyle = MarkerStyle.Triangle;
            series1.Color = Color.Red;

            Series series2 = chartControl2.Series.Add("Lab Data");// CurveList.Clear();
            series2.Color = Color.Blue;
            series2.ChartType = SeriesChartType.Point;
            series2.MarkerStyle = MarkerStyle.Cross;
            series2.MarkerSize = 14;

            DataPointCollection list1 = series1.Points;
            DataPointCollection list2 = series2.Points;
            if (data is null || data.MidBP.Count < 1)
                return;
            List<Temperature> MidBP = data.MidBP.GetRange(1, data.MidBP.Count - 1);

            double x, y;

            for (int i = 0; i < crude.ComponentList.Count; i++)
            {
                if (i >= crude.ComponentList.Count)
                {
                }
                else
                {
                    if (prop == enumAssayPCProperty.DENSITY15)
                    {
                        y = crude.ComponentList[i].Density;
                        x = crude.ComponentList[i].MidBP.Celsius;
                        list1.AddXY(x, y);
                    }
                    else if (crude.ComponentList[i].Properties.ContainsKey(prop))
                    {
                        y = crude.ComponentList[i].Properties[prop];
                        x = crude.ComponentList[i].MidBP.Celsius;
                        list1.AddXY(x, y);
                    }
                }
            }

            int row;
            if (data != null && data.StreamNames != null)
            {
                if (data.Properties.Contains(apc[prop].Name))
                {
                    for (int i = 0; i < data.Properties[apc[prop].Name].Length; i++)
                    {
                        if (data.Properties.Contains(apc[prop].Name))
                        {
                            row = data.Properties.GetRowIndex(apc[prop].Name);
                            y = data.Properties[apc[prop].Name][i];
                            x = MidBP[data.Properties.DataIndex[row][i]].Celsius;
                            list2.AddXY(x, y);
                        }
                    }
                }
            }

            if (list1.Count > 0)
            {
                chartControl2.ChartAreas[0].AxisX.Maximum = 850;
                chartControl2.ChartAreas[0].AxisX.Minimum = 36;

                double YMax;
                if (prop == enumAssayPCProperty.DENSITY15)
                    YMax = 1300;
                else
                    YMax = list1.FindMaxByValue().YValues.Max() * 1.1;

                chartControl2.ChartAreas[0].AxisY.Maximum = Math.Round(YMax, 1);
                chartControl2.ChartAreas[0].AxisY.Minimum = Math.Round(list1.FindMinByValue().YValues.Min() / 1.1, 2);
                //PropertyChart.ChartAreas[0].AxisY.LabelStyle.Format.;
                if (chartControl2.ChartAreas[0].AxisY.Maximum == chartControl2.ChartAreas[0].AxisY.Minimum)
                    chartControl2.ChartAreas[0].AxisY.Maximum = chartControl2.ChartAreas[0].AxisY.Minimum + 10;
            }
        }

        public void ChartPseudoComponentsSG()
        {
            if (crude == null)
                return;

            Series series1 = chart1.Series.Add("Pseudo Component LV%");
            DataPointCollection list2 = series1.Points;
            series1.MarkerStyle = MarkerStyle.Triangle;
            series1.Color = Color.Red;
            //GraphPane myPane2 = zg2.GraphPane;
            double CumLV = 0;
            double x;

            for (int i = 0; i < crude.ComponentList.Count; i++)
            {
                BaseComp bc = crude.ComponentList[i];
                double y = bc.SG_60F;
                if (bc.IsPure)
                    x = CumLV + bc.LiqVolumePercent;   // midpoint
                else
                    x = CumLV + bc.LiqVolumePercent / 2;   // midpoint

                CumLV += bc.LiqVolumePercent;
                list2.Add(x, y);
            }
        }

        public void ChartSGvsBP()
        {
            chartControl1.Series.Clear();
            chartControl1.Titles.Clear();
            chartControl1.Titles.Add("Density vs BP (C)");

            chartControl1.ChartAreas.Clear();
            chartControl1.ChartAreas.Add(new ChartArea());
            chartControl1.ChartAreas[0].AxisX.Name = "Density kg/m3";
            chartControl1.ChartAreas[0].AxisY.Name = "TBP Deg C";

            chartControl1.ChartAreas[0].AxisX.Minimum = 36;
            chartControl1.ChartAreas[0].AxisX.Maximum = 850;
            chartControl1.ChartAreas[0].AxisY.Minimum = 400;

            Series series1 = chartControl1.Series.Add("Calculated");
            series1.ChartType = SeriesChartType.Line;
            series1.MarkerStyle = MarkerStyle.Triangle;
            series1.Color = Color.Red;

            Series series2 = chartControl1.Series.Add("Lab Data");// CurveList.Clear();
            series2.Color = Color.Blue;
            series2.ChartType = SeriesChartType.Point;
            series2.MarkerStyle = MarkerStyle.Cross;
            series2.MarkerSize = 14;

            DataPointCollection list1 = series1.Points;
            DataPointCollection list2 = series2.Points;

            double x, y;
            if (data is null || data.NoAllComps == 0 || crude.ComponentList.Count == 0)
                return;

            for (int i = 0; i < data.NoAllComps; i++)
            {
                if (i >= crude.ComponentList.Count)
                {
                }
                else
                {
                    y = crude.ComponentList[i].SG_60F * 1000;
                    x = crude.ComponentList[i].MidBP.Celsius;
                    list1.AddXY(x, y);
                }
            }

            if (data != null && data.StreamNames != null)
            {
                for (int i = 0; i < data.StreamNames.Count; i++)
                {
                    y = data.StreamTotalDensity[i] / 1000;
                    x = data.MidBP[i].Celsius;
                    list2.AddXY(x, y);
                }
            }
        }

        /// <summary>
        /// Sequence:
        /// 1/ Calculate LV%
        /// 2/ Calculate LV% Adjustments
        /// 3/ Calculate SG Curve
        /// 4/ Calculate WABT and VABT for each cut
        /// 5/ Calculate Property Curves
        /// <param name="thermo"></param>
        /// </summary>
        public void CreateAssayMethod(Port_Material port, DataArrays data, ThermoDynamicOptions thermo)
        {
            if (data.Components.Count == 0)
                return;
            syn = new GenericSynthesis(port, assaytype, data, thermo);
            syn.DoAssay(crude, optimiseassayFit);   // CREATE Cumulative TBP vs. lvpct CURVE.
            crude.NormaliseFractions(FlowFlag.LiqVol);
            regenerated = true;
        }

        public void CreateLVChart()
        {
            // if (syn is null || syn.CumLVPCts is null)
            //     return  ;

            chart1.Series.Clear();
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
            series1.ChartType = SeriesChartType.Line;

            DataPointCollection list = series1.Points;

            for (int i = 0; i < crude.Count; i++)
            {
                double y = crude[i].MidBP.Celsius;
                double x = crude.VolFractionsCumulative[i] * 100;
                if (!double.IsNaN(x))
                    list.AddXY(x, y);
            }

            series1.LegendText = "Lab LV%";
            series1.Color = Color.Blue;
            series1.MarkerStyle = MarkerStyle.Diamond;
            series1.MarkerSize = 3;
        }

        public void CutPointsCalc()
        {
            if (data is null)
                return;

            double temp;

            List<double> StreamVols = data.StreamFlowsVol.CumulateArray();

            CutPoints.Clear();

            List<double> clv = syn.CumLVPCts;

            for (int i = 0; i < StreamVols.Count; i++)
            {
                temp = CubicSpline.CubSpline(eSplineMethod.Constrained, StreamVols[i], clv, ModelEngine.Components.GetMidBPs_C(data.Components));
                CutPoints.Add(temp);
            }
        }

        public string GetStringFromAssayClass(string prop)
        {
            string res;
            if (apc.Contains(prop))
            {
                res = apc[prop].Name;
            }
            else
                res = prop;

            return res;
        }

        private static string StripAssayName(string longname)
        {
            Char delimiter = '.';
            String[] substrings = longname.Split(delimiter);
            return substrings[0];
        }

        private void BtnLoadBBAssay_Click(object sender, EventArgs e)
        {
            LoadIntelligentAssayFromFile(true);

            switch (assaytype)
            {
                case enumAssayType.Assay:
                    rbtCrudeAssay.Select();
                    break;

                case enumAssayType.MultipleStream:
                    rbtMultStreamAssay.Select();
                    break;

                case enumAssayType.SingleStream:
                    rbtstreamAssay.Select();
                    break;
            }
        }

        private void BtnResetAssay_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void BtnResetAssayClick(object sender, EventArgs e)
        {
            BtnResetAssay_Click(sender, e);
        }

        private void CBProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (apc.Contains(CBProperty.Text))
                prop = apc[CBProperty.Text].Prop;
            else
                prop = enumAssayPCProperty.SULFUR;

            ChartPropertyvsBP();
        }

        private void CBXoptimiseAssayFit_CheckedChanged(object sender, EventArgs e)
        {
            optimiseassayFit = CBXoptimiseAssayFit.Checked;
        }

        private void ChartPseudoComponents_Click(object sender, EventArgs e)
        {
            AddChartRawLVPCTS();
        }

        private void ChartSGComponents_Click(object sender, EventArgs e)
        {
            ChartPseudoComponentsSG();
        }

        private void CreateSynStreams_Click(object sender, EventArgs e)
        {
            ThermoDynamicOptions thermo = new();
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            CreateAssay.LoadDataIntoArrays(port);
            CreateAssay.ProcessDataFormat();
            CreateAssayMethod(port, data, thermo);
        }

        private void CutPoints_Click(object sender, EventArgs e)
        {
            CutPointsCalc();
        }

        private void DgvBackBlend_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void DgvBackBlend_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void DgvBackBlend_KeyUp(object sender, KeyEventArgs e)
        {
            //if user clicked Shift+Ins or Ctrl+V (paste from clipboard)

            if ((e.Shift && e.KeyCode == Keys.Insert) || (e.Control && e.KeyCode == Keys.V))
            {
                char[] rowSplitter = { '\r', '\n' };
                char[] columnSplitter = { '\t' };

                //get the text from clipboard

                IDataObject dataInClipboard = Clipboard.GetDataObject();
                string stringInClipboard = (string)dataInClipboard.GetData(DataFormats.Text);

                //split it int o lines
                string[] rowsInClipboard = stringInClipboard.Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);

                //get the row and column of selected cell in grid
                int r = dgvGenAssay.SelectedCells[0].RowIndex;
                int c = dgvGenAssay.SelectedCells[0].ColumnIndex;

                //add rows int o grid to fit clipboard lines

                if (dgvGenAssay.Rows.Count < (r + rowsInClipboard.Length))
                {
                    dgvGenAssay.Rows.Add(r + rowsInClipboard.Length - dgvGenAssay.Rows.Count);
                }

                // loop through the lines, split them int o cells and place the values in the corresponding cell.

                for (int iRow = 0; iRow < rowsInClipboard.Length; iRow++)
                {
                    //split row int o cell values
                    string[] valuesInRow = rowsInClipboard[iRow].Split(columnSplitter);

                    //cycle through cell values
                    for (int iCol = 0; iCol < valuesInRow.Length; iCol++)
                    {
                        //assign cell value, only if it within columns of the grid
                        if (dgvGenAssay.ColumnCount - 1 >= c + iCol)
                        {
                            dgvGenAssay.Rows[r + iRow].Cells[c + iCol].Value = valuesInRow[iCol];
                        }
                    }
                }
            }
        }

        private void DgvGenAssasy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
                PasteClipboard();
        }

        private int FindRow(string val)
        {
            for (int i = 0; i < dgvGenAssay.RowCount; i++)
            {
                if (val == dgvGenAssay[0, i].Value.ToString())
                {
                    return i;
                }
            }
            return 9999;
        }

        private void GenericAssayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveBBAssay_Click(sender, e);
        }

        private void HideAllbutOneColumn()
        {
            for (int i = 3; i < dgvGenAssay.Columns.Count; i++)
            {
                dgvGenAssay.Columns[i].Visible = false;
            }

            dgvGenAssay.Columns[2].Width = 70;
        }

        private void LoadIntelligentAssayFromFile(bool ShowDialog)
        {
            int currentrow = 0;

            if (ShowDialog)
            {
                OpenFileDialog dlg = new();
                dlg.InitialDirectory = textBoxAssayName.Text;

                if (assayname == "")
                    dlg.FileName = filelocation;
                else
                    dlg.FileName = assayname + ".ass";

                dlg.ShowDialog();
                assayname = StripAssayName(dlg.FileName);
            }

            textBoxAssayName.Text = assayname;
            string file = assayname + ".ass";

            if (File.Exists(file))
            {
                try
                {
                    using BinaryReader bw = new(File.Open(file, FileMode.Open));
                    int cols = bw.ReadInt32();
                    int rows = bw.ReadInt32();

                    bool resb;
                    string res;

                    for (int row = 0; row < rows; ++row)
                    {
                        resb = bw.ReadBoolean();
                        res = bw.ReadString();  // Row Name

                        if (resb)
                            currentrow = FindRow(res);

                        for (int j = 1; j < cols; ++j)
                        {
                            resb = bw.ReadBoolean();
                            res = bw.ReadString(); // Value
                            if (currentrow != 9999)
                                dgvGenAssay[j, row].Value = res;
                        }
                    }

                    try
                    {
                        string assaytypestring = bw.ReadString();
                        if (Enum.TryParse(assaytypestring, out enumAssayType temp))
                            assaytype = temp;
                        else
                            assaytype = enumAssayType.Assay;
                    }
                    catch { }
                }
                catch { }
            }
        }

        private bool LoadNewAssayFromFile(bool ShowDialog)
        {
            if (ShowDialog)
            {
                OpenFileDialog dlg = new();
                dlg.InitialDirectory = textBoxAssayName.Text;

                if (assayname == "")
                    dlg.FileName = filelocation;
                else
                    dlg.FileName = assayname + ".ass";

                dlg.ShowDialog();
                assayname = StripAssayName(dlg.FileName);
            }

            textBoxAssayName.Text = assayname;

            string file = assayname + ".ass";

            dgvGenAssay.Rows.Clear();

            if (File.Exists(file))
            {
                try
                {
                    using BinaryReader bw = new(File.Open(file, FileMode.Open));
                    int cols = bw.ReadInt32();
                    int rows = bw.ReadInt32();
                    for (int row = 0; row < rows; ++row)
                    {
                        dgvGenAssay.Rows.Add();
                        for (int j = 0; j < cols; ++j)
                        {
                            bool resb = bw.ReadBoolean();
                            string res = bw.ReadString();

                            if (resb)
                            {
                                if (Enum.IsDefined(typeof(enumCommonPureComps), res))
                                    AddComponent(res.ToEnum<enumCommonPureComps>(), row, j);
                                else if (Enum.IsDefined(typeof(enumAssayPCProperty), res))
                                    AddProperty(res.ToEnum<enumAssayPCProperty>(), row, j);
                                else if (Enum.IsDefined(typeof(enumPCTType), res))
                                    AddProperty(res.ToEnum<enumPCTType>(), row, j);
                                else if (Enum.IsDefined(typeof(enumTemp), res))
                                    AddProperty(res.ToEnum<enumTemp>(), row, j);
                                else if (Enum.IsDefined(typeof(enumMassVolMol), res))
                                    AddProperty(res.ToEnum<enumMassVolMol>(), row, j);
                                else if (Enum.IsDefined(typeof(enumDistType), res))
                                    AddProperty(res.ToEnum<enumDistType>(), row, j);
                                else
                                    dgvGenAssay[j, row].Value = res;
                            }
                        }
                    }
                }
                catch
                {
                }
                return true;
            }
            return false;
        }

        private void MakeDataTable()
        {
            if (data is null)
                return;
            dgvResults.Rows.Clear();
            double[] CumLVPCT = crude.VolFractionsCumulative;

            for (int i = 0; i < data.NoAllComps; i++)
            {
                int row = dgvResults.Rows.Add();
                dgvResults.Rows[row].Cells[0].Value = data.Components[i].Name;
                dgvResults.Rows[row].Cells[1].Value = CumLVPCT[i].Round(5);
                dgvResults.Rows[row].Cells[2].Value = crude.ComponentList[i].STDLiqVolFraction.Round(5);
                dgvResults.Rows[row].Cells[3].Value = crude.ComponentList[i].MassFraction.Round(5);
                dgvResults.Rows[row].Cells[4].Value = crude.ComponentList[i].MoleFraction.Round(5);
                dgvResults.Rows[row].Cells[5].Value = crude.ComponentList[i].MW.Round(5);
                dgvResults.Rows[row].Cells[6].Value = crude.ComponentList[i].SG_60F.Round(5);
            }
        }

        private void PasteClipboard()
        {
            string s = Clipboard.GetText();
            string[] lines = s.Split('\n');
            int iRow = dgvGenAssay.CurrentCell.RowIndex;
            int iCol = dgvGenAssay.CurrentCell.ColumnIndex;
            DataGridViewCell oCell;
            foreach (string line in lines)
            {
                if (iRow < dgvGenAssay.RowCount && line.Length > 0)
                {
                    string[] sCells = line.Split('\t');
                    for (int i = 0; i < sCells.GetLength(0); ++i)
                    {
                        if (iCol + i < this.dgvGenAssay.ColumnCount)
                        {
                            if (dgvGenAssay[iCol + i, iRow].Value is null)
                            {
                                dgvGenAssay[iCol + i, iRow].Value = "";
                            }
                            oCell = dgvGenAssay[iCol + i, iRow];
                            if (!oCell.ReadOnly)
                            {
                                if (oCell.Value.ToString() != sCells[i] && oCell.ValueType == typeof(object))
                                {
                                    oCell.Value = Convert.ChangeType(sCells[i], oCell.ValueType);
                                    oCell.Style.BackColor = Color.Tomato;
                                }
                            }
                        }
                    }
                    iRow++;
                }
            }
        }

        private void PlotPseudoComponentsbutton2_Click(object sender, EventArgs e)
        {
            AddChartRawLVPCTS();
        }

        /// <summary>
        /// </summary>
        /// <param name="thermo"></param>
        private void ProductCurves_Click(object sender, EventArgs e)
        {
            ChartProductCurvesTBP();
            CutPointsCalc();
            if (data != null)
            {
                List<double> CumLV = data.StreamFlowsVol.CumulateArray();

                if (CutPoints.Count >= data.StreamNames.Count)
                {
                    for (int i = 0; i < data.StreamNames.Count; i++)
                    {
                        ChartAddLine(CumLV[i] * 100D, CumLV[i] * 100D, CutPoints[i] - 150, CutPoints[i] + 150);
                    }
                }
            }
        }

        private void PropertyTable()
        {
            if (data is null)
                return;

            DGPropertyComparison.Rows.Clear();
            DGPropertyComparison.Columns.Clear();
            int columnnumber = DGPropertyComparison.Columns.Add("Name", "Name");
            if (data.StreamNames.Count <= 0)
                return;

            DGPropertyComparison.Rows.Add(data.StreamNames.Count);

            for (int stream = 0; stream < data.StreamNames.Count; stream++)
            {
                DGPropertyComparison[columnnumber, stream].Value = data.StreamNames[stream];
            }

            for (int property = 0; property < data.Properties.Count; property++)
            {
                string prop = data.Properties.Names()[property];
                columnnumber = DGPropertyComparison.Columns.Add(prop, prop);
                double Value;

                for (int stream = 0; stream < data.Properties[prop].Length; stream++)
                {
                    int streamindex = data.Properties.DataIndex[property][stream];
                    Value = data.Properties[prop][stream];
                    string CalcValue = Value.ToString();

                    if (data.PropertiesCalculated.Contains(prop))
                    {
                        if (data.PropertiesCalculated[prop].Length > stream)
                        {
                            int digits = SignificantDigits.CountSignificantDigits(Value);
                            double secondvalue = data.PropertiesCalculated[prop][stream];
                            secondvalue = SignificantDigits.Round(secondvalue, digits);
                            CalcValue = CalcValue + " (" + secondvalue.ToString() + ")";
                        }
                    }
                    DGPropertyComparison[columnnumber, streamindex + 1].Value = CalcValue;
                }
            }
        }

        private void RbtCrudeAssay_CheckedChanged(object sender, EventArgs e)
        {
            assaytype = enumAssayType.Assay;
            ShowAllColumns();
        }

        private void RbtMultStreamAssay_CheckedChanged(object sender, EventArgs e)
        {
            assaytype = enumAssayType.MultipleStream;
            ShowAllColumns();
        }

        private void RbtStreamAssay_CheckedChanged(object sender, EventArgs e)
        {
            assaytype = enumAssayType.SingleStream;
            HideAllbutOneColumn();
        }

        private void ResetForm()
        {
            int r;
            int startcol = 2;

            assaytype = enumAssayType.Assay;

            dgvGenAssay.Rows.Clear();

            DataGridViewRow dr = new();
            dr.CreateCells(dgvGenAssay);
            dr.Cells[0].Value = "Name";
            dgvGenAssay.Rows.Add(dr);

            dr = new();
            dr.CreateCells(dgvGenAssay);
            dr.Cells[0].Value = "Composition Basis";
            r = dgvGenAssay.Rows.Add(dr);

            DataGridViewComboBoxCell c;

            for (int n = startcol; n < dgvGenAssay.ColumnCount; n++)
            {
                c = new();
                c.DataSource = Enum.GetValues(typeof(enumPCTType));
                c.ValueType = typeof(enumPCTType);
                c.Value = enumPCTType.LV_Crude;
                dgvGenAssay.Rows[r].Cells[n] = c;
            }

            AddComponent(enumCommonPureComps.H2);
            AddComponent(enumCommonPureComps.H2S);
            AddComponent(enumCommonPureComps.C1);
            AddComponent(enumCommonPureComps.C2);
            AddComponent(enumCommonPureComps.C3);
            AddComponent(enumCommonPureComps.iC4);
            AddComponent(enumCommonPureComps.nC4);
            AddComponent(enumCommonPureComps.iC5);
            AddComponent(enumCommonPureComps.nC5);

            r = AddProperty(enumAssayPCProperty.TBPCUTPOINT);
            c = new();
            c.DataSource = Enum.GetValues(typeof(enumTemp));
            c.ValueType = typeof(enumTemp);
            c.Value = enumTemp.C;
            dgvGenAssay.Rows[r].Cells[1] = c;

            r++;
            AddValue(r, "Flow/Percent");
            AddFlowBasis(enumMassVolMol.Wt_PCT, r);
            AddProperty(enumAssayPCProperty.DENSITY15);

            r = dgvGenAssay.Rows.Add();
            dgvGenAssay[0, r].Value = "Dist Type";

            for (int n = startcol; n < dgvGenAssay.ColumnCount; n++)
            {
                c = new();
                c.DataSource = Enum.GetValues(typeof(enumDistType));
                c.ValueType = typeof(enumDistType);
                c.Value = enumDistType.TBP_WT;
                dgvGenAssay.Rows[r].Cells[n] = c;
            }

            //AddProperty(enumAssayPCProperty.IBP);
            AddProperty(enumAssayPCProperty.BP1);
            AddProperty(enumAssayPCProperty.BP5);
            AddProperty(enumAssayPCProperty.BP10);
            AddProperty(enumAssayPCProperty.BP20);
            AddProperty(enumAssayPCProperty.BP30);
            //AddProperty(enumAssayPCProperty.BP40);
            AddProperty(enumAssayPCProperty.BP50);
            //AddProperty(enumAssayPCProperty.BP60);
            AddProperty(enumAssayPCProperty.BP70);
            AddProperty(enumAssayPCProperty.BP80);
            AddProperty(enumAssayPCProperty.BP90);
            AddProperty(enumAssayPCProperty.BP95);
            AddProperty(enumAssayPCProperty.BP99);

            /*
           Yield (% wt)
           Yield (% vol)
           Cumulative Yield (% wt)
           Volume Average B.P. (°C)
           Density @ 15°C (g/cc)
           API Gravity
           UOPK
           Molecular Weight (g/mol)*/

            /*
            Total Sulphur (% wt)
            Mercaptan Sulphur (ppm)
            Total Nitrogen (ppm)
            Basic Nitrogen (ppm)
            Total Acid Number (mgKOH/g)
            */

            AddProperty(enumAssayPCProperty.SULFUR);
            AddProperty(enumAssayPCProperty.MERCAPTANSULFUR);
            AddProperty(enumAssayPCProperty.TOTALNITROGEN);
            AddProperty(enumAssayPCProperty.BASICNITROGEN);
            AddProperty(enumAssayPCProperty.TOTALACIDNUMER);

            /*
            Viscosity @ 20°C (cSt)
            Viscosity @ 40°C (cSt)
            Viscosity @ 50°C (cSt)
            Viscosity @ 60°C (cSt)
            Viscosity @ 100°C (cSt)
            Viscosity @ 130°C (cSt)
            */

            AddValue(AddProperty(enumAssayPCProperty.VIS20), 20.ToString());
            AddValue(AddProperty(enumAssayPCProperty.VIS40), 40.ToString());
            AddValue(AddProperty(enumAssayPCProperty.VIS50), 50.ToString());
            AddValue(AddProperty(enumAssayPCProperty.VIS60), 60.ToString());
            AddValue(AddProperty(enumAssayPCProperty.VIS100), 100.ToString());
            AddValue(AddProperty(enumAssayPCProperty.VIS130), 130.ToString());

            /*
            RON (Clear)
            MON (Clear)
            Paraffins (% wt)
            Naphthenes (%wt)
            Aromatics (% wt)*/

            AddProperty(enumAssayPCProperty.RONC);
            AddProperty(enumAssayPCProperty.MONC);
            AddProperty(enumAssayPCProperty.PARAFFINS);
            AddProperty(enumAssayPCProperty.NAPHTHENES);
            AddProperty(enumAssayPCProperty.AROMATICS);

            /*
            Pour Point  (°C)
            Cloud Point  (°C)
            Freeze Point  (°C)
            Smoke Point  (mm)
            Cetane Index
            Naphthalenes (% vol)
            Aniline Point  (°C)
            Hydrogen (% wt)
            Wax (% wt)
            */

            AddProperty(enumAssayPCProperty.CLOUDPOINT);
            AddProperty(enumAssayPCProperty.FREEZEPOINT);
            AddProperty(enumAssayPCProperty.CETANEINDEX);
            AddProperty(enumAssayPCProperty.NAPHTHALENES);
            AddProperty(enumAssayPCProperty.ANILINEPOINT);
            AddProperty(enumAssayPCProperty.HYDROGEN);
            AddProperty(enumAssayPCProperty.WAX);

            /*
            C7 Asphaltenes (% wt)
            Micro Carbon Residue (% wt)
            Rams. Carbon Residue (% wt)
            Vanadium (ppm)
            Nickel (ppm)
            Iron (ppm)
            */

            AddProperty(enumAssayPCProperty.C7ASPHALTENES);
            AddProperty(enumAssayPCProperty.MICROCARBONRESIDUE);
            AddProperty(enumAssayPCProperty.RAMSBOTTOMCARBON);
            AddProperty(enumAssayPCProperty.VANADIUM);
            AddProperty(enumAssayPCProperty.NICKEL);
            AddProperty(enumAssayPCProperty.IRON);
        }

        private void SavAsCSV_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new();
            dlg.InitialDirectory = filelocation;
            dlg.FileName = assayname + ".bba";
            dlg.ShowDialog();

            assayname = StripAssayName(dlg.FileName);

            string file = assayname + ".CSV";

            //before your loop
            var csv = new StringBuilder();

            if (crude != null)
            {
                //in your loop

                csv.AppendLine("Name," + this.Name);
                csv.AppendLine("Created," + DateTime.Now.ToString());
                csv.AppendLine("Modified" + DateTime.Now.ToString());
                var newLine = string.Format("Cpt,Composition,Boiling Temperature,Molecular Weight,Standard Liquid Density");
                csv.AppendLine(newLine);

                string name;

                for (int i = 0; i < crude.ComponentList.Count; i++)
                {
                    if (crude.ComponentList[i].Name.StartsWith("Quasi"))
                        name = crude.ComponentList[i].Name + "*";
                    else
                        name = crude.ComponentList[i].Name;

                    var molffrac = crude.ComponentList[i].MoleFraction;
                    var bp = crude.ComponentList[i].BP + 273.15;
                    var mw = crude.ComponentList[i].MW;
                    var density = crude.ComponentList[i].SG_60F * 1000;
                    var CritT = crude.ComponentList[i].CritT;
                    var CritP = crude.ComponentList[i].CritP;
                    var Omega = crude.ComponentList[i].Omega;
                    var HForm25 = crude.ComponentList[i].HForm25;
                    //Suggestion made by KyleMit
                    newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", name,
                        molffrac, bp, mw, density, CritT, CritP, Omega, HForm25);
                    csv.AppendLine(newLine);
                }

                //after your loop
                File.WriteAllText(file, csv.ToString());  
            }
        }

        private void SaveBBAssay_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new();
            dlg.InitialDirectory = filelocation;
            dlg.FileName = assayname + ".ass";
            dlg.ShowDialog();

            assayname = StripAssayName(dlg.FileName);

            string file = assayname + ".ass";

            using BinaryWriter bw = new(File.Open(file, FileMode.Create));
            bw.Write(dgvGenAssay.Columns.Count);
            bw.Write(dgvGenAssay.Rows.Count);

            foreach (DataGridViewRow dgvR in dgvGenAssay.Rows)
            {
                for (int j = 0; j < dgvGenAssay.Columns.Count; ++j)
                {
                    object val = dgvR.Cells[j].Value;
                    if (val == null)
                    {
                        bw.Write(false);
                        bw.Write(false);
                    }
                    else
                    {
                        bw.Write(true);
                        bw.Write(GetStringFromAssayClass(val.ToString()));
                    }
                }
            }
            bw.Write(assaytype.ToString());  // Store assay type at end of file
        }

        private void ShowAllColumns()
        {
            for (int i = 3; i < dgvGenAssay.Columns.Count; i++)
            {
                dgvGenAssay.Columns[i].Visible = true;
            }
        }

        private void TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            splitContainer2.SplitterDistance = 1000;
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedIndex)
            {
                case 1:
                    CreateLVChart();
                    break;

                case 2:
                    ChartSGvsBP();
                    break;

                case 3:
                    MakeDataTable();
                    break;

                case 4:
                    if (data is not null)
                        CBProperty.DataSource = apc.Names(); //..Enum.GetValues(typeof(enumAssayPCProperty));
                    CBProperty.Text = "SULFUR";
                    break;

                case 5:
                    PropertyTable();
                    break;

                default:
                    break;
            }
            //splitContainer2.SplitterDistance = 1000;
        }

        private void GenericAssayForm_Load(object sender, EventArgs e)
        {
            if (!LoadNewAssayFromFile(false))
            {
                ResetForm();
                dgvGenAssay.Visible = true;
            }
        }
    }
}