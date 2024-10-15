using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units
{
    internal partial class LLEDLG : Form
    {
        private LLESEP llesep;
        private DrawLLEColumn drawcolumn;

        internal DrawLLEColumn drawColumn
        {
            get
            {
                return drawcolumn;
            }
            set
            {
                drawcolumn = value;
            }
        }

        public LLESEP LLESep { get => llesep; set => llesep = value; }

        private readonly BindingSource BDColumnData = new();
        private readonly BindingSource BDTrays = new();

        // This method demonstrates a pattern for making thread-safe
        // calls on a Windows Forms control.
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextCallback and calls itself asynchronously using   the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly.
        public delegate void SetTextCallback(string text);

        public void SetTextErr1(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it return  s true.

            if (this.Error1.InvokeRequired)
            {
                SetTextCallback d = new(SetTextErr1);
                this.Invoke(d, new object[] { text });
            }
            else
                this.Error1.Text = text;
        }

        public void SetTextErr2(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it return  s true.

            if (this.Error2.InvokeRequired)
            {
                SetTextCallback d = new(SetTextErr2);
                this.Invoke(d, new object[] { text });
            }
            else
                this.Error2.Text = text;
        }

        public void UpdateProgress()
        {
            Error1.Text += llesep.Err1;
            Error2.Text += llesep.Err2;
        }

        public void UpdateSpecs()
        {
            specificationSheet1.SetData();
        }

        internal LLEDLG(DrawLLEColumn drawColumn)
        {
            this.llesep = drawColumn.Column;
            this.drawcolumn = drawColumn;
            InitializeComponent();
            //  if (specificationSheet1 != null)
            //       specificationSheet1.drawColumn = drawcolumn;

            BDColumnData.Add(llesep);

            FillProfileData(llesep);
            FillDiagnosticData(llesep);

            if (columnDesignerControl1 != null)
                columnDesignerControl1.Initialize(this);
        }

        public void FillProfileData(LLESEP column)
        {
            if (column is null)
                return;

            ComponentProfileDG.Columns.Clear();
            ComponentProfileDG.Columns.Add("Stage", "Stage");
            ComponentProfileDG.Columns.Add("Temp", "Deg C");
            ComponentProfileDG.Columns.Add("Liquid", "Liquid Rate");
            ComponentProfileDG.Columns.Add("Vapour", "Vapour Rate");

            if (!column.IsDirty) // do not use out of date data
            {
                for (int n = 0; n < column.NoComps; n++)
                    ComponentProfileDG.Columns.Add("Component" + n.ToString(), column.Components[n].Name.ToString());

                for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
                {
                    TraySection section = column.TraySections[sectionNo];

                    for (int trayNo = 0; trayNo < section.NoTrays; trayNo++)
                    {
                        Tray tray = section.Trays[trayNo];
                        int row = ComponentProfileDG.Rows.Add();
                        ComponentProfileDG.Rows[row].Cells[0].Value = tray.ToString();
                        ComponentProfileDG.Rows[row].Cells[1].Value = SignificantDigits.Round(tray.T - 273.15, 4);
                        ComponentProfileDG.Rows[row].Cells[2].Value = SignificantDigits.Round(tray.L, 4) * column.FeedRatio;
                        ComponentProfileDG.Rows[row].Cells[3].Value = SignificantDigits.Round(tray.V, 4) * column.FeedRatio;

                        for (int i = 0; i < column.NoComps; i++) //cd.CompCollection[i].Nam
                            ComponentProfileDG.Rows[row].Cells[i + 4].Value = SignificantDigits.Round(tray.LiqComposition[i], 5);
                    }
                }
            }
        }

        public void ClearOldResults()
        {
            ComponentProfileDG.Rows.Clear();
        }

        public void FillDiagnosticData(LLESEP cd)
        {
            DiagnosticDataGridView.Columns.Clear();
            if (cd is null)
                return;

            DiagnosticDataGridView.Columns.Add("Stage", "Stage");
            DiagnosticDataGridView.Columns.Add("Temp", "Deg C");
            DiagnosticDataGridView.Columns.Add("Liquid", "Liquid Rate");
            DiagnosticDataGridView.Columns.Add("Vapour", "Vapour Rate");

            DiagnosticDataGridView.Columns.Add("EnergyBalance", "Energy Balance");
            DiagnosticDataGridView.Columns.Add("LqiuidEnthalpy", "Lqiuid Enthalpy");
            DiagnosticDataGridView.Columns.Add("VapourEnthalpy", "Vapour Enthalpy");
            DiagnosticDataGridView.Columns.Add("LiquidProducts", "Liquid Products");
            DiagnosticDataGridView.Columns.Add("VapourProducts", "Vapour Products");

            int count = -1;
            for (int sectionNo = 0; sectionNo < cd.NoSections; sectionNo++)
            {
                TraySection section = cd.TraySections[sectionNo];
                for (int trayNo = 0; trayNo < cd.TraySections[sectionNo].NoTrays; trayNo++)
                {
                    Tray tray = section.Trays[trayNo];
                    count++;
                    int row = DiagnosticDataGridView.Rows.Add();
                    DiagnosticDataGridView.Rows[row].Cells[0].Value = count.ToString();
                    DiagnosticDataGridView.Rows[row].Cells[1].Value = SignificantDigits.Round(section.Trays[trayNo].T - 273.15, 4);
                    DiagnosticDataGridView.Rows[row].Cells[2].Value = SignificantDigits.Round(tray.L, 4) / cd.FeedRatio;
                    DiagnosticDataGridView.Rows[row].Cells[3].Value = SignificantDigits.Round(tray.V, 4) / cd.FeedRatio;
                    DiagnosticDataGridView.Rows[row].Cells[4].Value = SignificantDigits.Round(tray.enthalpyBalance, 5);
                    DiagnosticDataGridView.Rows[row].Cells[5].Value = SignificantDigits.Round(tray.liqenth, 5);
                    DiagnosticDataGridView.Rows[row].Cells[6].Value = SignificantDigits.Round(tray.vapenth, 5);
                    DiagnosticDataGridView.Rows[row].Cells[7].Value = SignificantDigits.Round(tray.L, 5) / cd.FeedRatio;
                    DiagnosticDataGridView.Rows[row].Cells[8].Value = SignificantDigits.Round(tray.V, 5) / cd.FeedRatio;
                }
            }
        }

        private void ColumnDLG_Load(object sender, EventArgs e)
        {
            //TopPressure Txt.SetValues(cd.TopPressure );
            //BottomPressure Txt.SetValues(cd.BotPressure );
            loaddata();

            DampFactortxt.Text = llesep.TempDampfactor.ToString();
            cbResetInitialTemps.Checked = llesep.ResetInitialTemps;
            LLESep.ValidateDesign();

            //FillEstimateData(column); // needs to have sections etc initisilaised by validate design

            //dgvEstimates.AddColumn("T");

            checkBoxActive.Checked = llesep.IsActive;
        }

        public void loaddata()
        {
            Error1.Text = llesep.ErrHistory1;
            Error2.Text = llesep.ErrHistory2;
            FillProfileData(llesep);
        }

        public void UpdateIteration1(string newline)

        {
            Error1.Text += newline;
            Error1.SelectionStart = Error1.TextLength;
            Error1.ScrollToCaret();
            Error1.Refresh();
            Error1.Update();
        }

        public void UpdateIteration2(string newline)
        {
            Error2.Text += newline;
            Error2.SelectionStart = Error2.TextLength;
            Error1.ScrollToCaret();
            Error2.Update();
        }

        internal void UpdateSpecs(DrawLLEColumn drawColumn)
        {
            //  specificationSheet1.SetData(drawColumn);
        }

        private void Tabs_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPage.Name)
            {
                case "TabPageSpecifications":
                    this.WindowState = FormWindowState.Normal;
                    drawColumn.UpdateAttachedModel();
                    //  specificationSheet1.SetData(drawColumn);
                    break;

                case "TabPageEstimates":
                    this.WindowState = FormWindowState.Normal;
                    drawColumn.UpdateAttachedModel();
                    FillPressureData();
                    FillEffciencyData();
                    break;

                case "TabPageTrays":
                    this.WindowState = FormWindowState.Normal;
                    BDTrays.ResetBindings(false);
                    break;

                case "TabPageConnections":
                    this.WindowState = FormWindowState.Normal;
                    //PopulateConnectionsDataViewGrids();
                    break;

                case "TabPageColumnDesigner":
                    this.WindowState = FormWindowState.Normal;
                    PopulateConnectionsDataViewGrids();
                    break;

                case "TabPageStreams":
                    this.WindowState = FormWindowState.Normal;
                    //PopulateConnectionsDataViewGrids();
                    List<StreamMaterial> streams = drawColumn.GetStreamListFromNodes();
                    worksheet.PortsPropertyWorksheetInitialise(streams, drawColumn.DrawArea.UOMDisplayList);
                    worksheet.UpdateValues();
                    break;

                case "TabPageDesigner":
                    this.WindowState = FormWindowState.Maximized;
                    columnDesignerControl1.GraphicsList = drawColumn.graphicslist;
                    columnDesignerControl1.Initialize(this);
                    break;
            }
        }

        private void PopulateConnectionsDataViewGrids()
        {
            drawcolumn.RefreshStreams();
            int count = 0;
            Guid connectionGuid;

            dGVFeedsConnections.Rows.Clear();
            dGVProductsConnections.Rows.Clear();

            foreach (DrawMaterialStream ds in drawcolumn.ExtFeedStreams)
            {
                dGVFeedsConnections.Rows.Add(1);
                dGVFeedsConnections.Rows[count].Cells[0].Value = ds.Name;

                ArrayList txt = new();
                txt.Add("Not Connected");

                foreach (DrawMaterialStream internalStream in drawcolumn.intFeedStreams) // see if is connected
                {
                    txt.Add(internalStream.Name);
                    if (drawcolumn.StreamConnectionsGuidDic.TryGetValue(ds.Guid, out connectionGuid)) // is it connected
                        if (connectionGuid == internalStream.Guid)
                            dGVFeedsConnections.Rows[count].Cells[1].Value = internalStream.Name;
                }

                DataGridViewComboBoxCell cbcell = (DataGridViewComboBoxCell)dGVFeedsConnections.Rows[count].Cells[1];

                cbcell.DataSource = (string[])txt.ToArray(typeof(string));
                count++;
            }

            count = 0;
            foreach (DrawMaterialStream ds in drawcolumn.ExtProductStreams)
            {
                dGVProductsConnections.Rows.Add(1);
                dGVProductsConnections.Rows[count].Cells[0].Value = ds.Name;

                ArrayList txt = new();
                txt.Add("Not Connected");
                foreach (DrawMaterialStream dsi in drawcolumn.intProductStreams)
                {
                    txt.Add(dsi.Name);
                    if (drawcolumn.StreamConnectionsGuidDic.TryGetValue(ds.Guid, out connectionGuid))
                        if (connectionGuid == dsi.Guid)
                            dGVProductsConnections.Rows[count].Cells[1].Value = dsi.Name;
                }

                DataGridViewComboBoxCell cbcell = (DataGridViewComboBoxCell)dGVProductsConnections.Rows[count].Cells[1];

                cbcell.DataSource = (string[])txt.ToArray(typeof(string));
                count++;
            }
        }

        private void DataGridViewFeedConnections_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                // Remove an existing event-handler, if present, to avoid
                // adding multiple handlers when the editing control is reused.
                combo.SelectedIndexChanged -= new EventHandler(ComboBox_FeedSelectedIndexChanged);

                // Add the event handler.
                combo.SelectedIndexChanged += new EventHandler(ComboBox_FeedSelectedIndexChanged);
            }
        }

        private void DataGridViewProductConnections_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                // Remove an existing event-handler, if present, to avoid
                // adding multiple handlers when the editing control is reused.
                combo.SelectedIndexChanged -= new EventHandler(ComboBox_ProductSelectedIndexChanged);

                // Add the event handler.
                combo.SelectedIndexChanged += new EventHandler(ComboBox_ProductSelectedIndexChanged);
            }
        }

        private void ComboBox_FeedSelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1)
                return;
            ComboBox combo = (ComboBox)sender;
            string intName = combo.Text;
            string ExtName = dGVFeedsConnections.Rows[((DataGridViewComboBoxEditingControl)sender).EditingControlRowIndex].Cells[0].Value.ToString();

            Guid ExtKey;
            DrawMaterialStream extstream = drawcolumn.ExtFeedStreams[ExtName];
            DrawMaterialStream intstream = drawcolumn.intFeedStreams[intName];

            if (extstream != null)
            {
                ExtKey = extstream.Guid;
                if (intstream != null)
                    drawcolumn.StreamConnectionsGuidDic[ExtKey] = intstream.Guid;
                else
                    drawcolumn.StreamConnectionsGuidDic[ExtKey] = new Guid();
            }
        }

        private void ComboBox_ProductSelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1)
                return;
            ComboBox combo = (ComboBox)sender;
            string intName = combo.Text;
            string ExtName = dGVProductsConnections.Rows[((DataGridViewComboBoxEditingControl)sender).EditingControlRowIndex].Cells[0].Value.ToString();

            Guid ExtKey;
            DrawMaterialStream extstream = drawcolumn.ExtProductStreams[ExtName];
            DrawMaterialStream intstream = drawcolumn.intProductStreams[intName];

            if (extstream != null)
            {
                ExtKey = extstream.Guid;
                if (intstream != null)
                    drawcolumn.StreamConnectionsGuidDic[ExtKey] = intstream.Guid;
                else
                    drawcolumn.StreamConnectionsGuidDic[ExtKey] = Guid.NewGuid();
            }
        }

        private void BtnEstT_Click(object sender, EventArgs e)
        {
            LLE_Solver.LLESolver r = new();
            r.CalcInitialTs(llesep);

            FillPressureData();
        }

        private void BtnResetEstimates_Click(object sender, EventArgs e)
        {
            // dgTrayEstimates.Rows.Clear();
            for (int sectionNo = 0; sectionNo < llesep.NoSections; sectionNo++)
            {
                for (int trayNo = 0; trayNo < llesep.TraySections[sectionNo].NoTrays; trayNo++)
                {
                    Tray tray = llesep.TraySections[sectionNo].Trays[trayNo];
                    tray.T = 25 + 275.15;
                }
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            FillPressureData();
        }

        private void DampFactortxt_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(DampFactortxt.Text, out double res))
                llesep.TempDampfactor = res;
            else
                llesep.TempDampfactor = 1;
        }

        private void TxtMaxOuterIterations_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtMaxOuterIterations.Text, out double res))
                llesep.MaxOuterIterations = res;
            else
                llesep.MaxOuterIterations = 50;
        }

        private void TxtMaxInnerIterations_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtMaxInnerIterations.Text, out double res))
                llesep.MaxTemperatureLoopCount = res;
            else
                llesep.MaxTemperatureLoopCount = 50;
        }

        private void TxtInnerTolerance_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtInnerTolerance.Text, out double res))
                llesep.SpecificationTolerance = res;
            else
                llesep.SpecificationTolerance = 0.1;
        }

        private void TxtOuterTolerance_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtOuterTolerance.Text, out double res))
                llesep.TrayTempTolerance = res;
            else
                llesep.TrayTempTolerance = 0.1;
        }

        private void CB_SplitMainFeed_CheckedChanged(object sender, EventArgs e)
        {
            llesep.SplitMainFeed = CB_SplitMainFeed.Checked;
        }

        private void CB_ResetInitialTemps_CheckedChanged(object sender, EventArgs e)
        {
            llesep.ResetInitialTemps = cbResetInitialTemps.Checked;
        }

        private void BtnConverged_Click(object sender, EventArgs e)
        {
            llesep.SolutionConverged = false;
        }

        public void FillPressureData()
        {
            DGVPressures.Clear();
            TraySectionCollection ts = drawcolumn.Column.TraySections;
            //ts.Sort();

            for (int sectionNo = 0; sectionNo < ts.Count; sectionNo++)
            {
                TraySection ts2 = ts[sectionNo];
                for (int row = 0; row < ts2.Trays.Count; row++)
                {
                    Tray tray = ts2.Trays[row];
                    DGVPressures.Add(tray.P, "Tray" + row.ToString(), sectionNo, "Section" + sectionNo.ToString());
                }
            }
        }

        public void FillEffciencyData()
        {
            DGVEfficiencies.Clear();
            TraySectionCollection ts = drawcolumn.Column.TraySections;
            //ts.Sort();

            for (int sectionNo = 0; sectionNo < ts.Count; sectionNo++)
            {
                TraySection ts2 = ts[sectionNo];
                for (int row = 0; row < ts2.Trays.Count; row++)
                {
                    Tray tray = ts2.Trays[row];
                    DGVEfficiencies.Add(tray.TrayEff, "Tray" + row.ToString(), sectionNo, "Section" + sectionNo.ToString());
                }
            }
        }

        private void ButtonCalculate_Click(object sender, EventArgs e)
        {
            drawcolumn.Calculate(this, new EventArgs());
        }

        private void DgvEstimates_ValueChanged(object sender, EventArgs e)
        {
            llesep.InterpolatePressureAllSection();
            DGVPressures.UpdateValues();
        }

        private void ColumnDLG_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    drawcolumn.columndrawarea.DeleteSelection();
                    drawColumn.columndrawarea.Refresh();
                    break;

                case Keys.Control:
                case Keys.ControlKey:
                    if (drawcolumn.columndrawarea.ControlPressed)
                        drawcolumn.columndrawarea.ControlPressed = false;
                    else
                        drawcolumn.columndrawarea.ControlPressed = true;
                    break;

                default:
                    drawcolumn.columndrawarea.ControlPressed = false;
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            llesep.IsActive = checkBoxActive.Checked;
        }

        private void LLEDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}