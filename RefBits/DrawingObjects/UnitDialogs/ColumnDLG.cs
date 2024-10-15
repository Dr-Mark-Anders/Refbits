using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Units.Dialogs;

namespace Units
{
    internal partial class ColumnDLG : Form
    {
        private Column column;
        private DrawColumn drawcolumn;

        internal DrawColumn drawColumn
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

        public Column Column { get => column; set => column = value; }

        //private readonly BindingSource BDColumnData = new();
        //private readonly BindingSource BDTrays = new();

        //Thismethoddemonstratesapatternformakingthread-safe
        //callsonaWindowsFormscontrol.
        //
        //Ifthecallingthreadisdifferentfromthethreadthat
        //createdtheTextBoxcontrol,thismethodcreatesa
        //SetTextCallbackandcallsitselfasynchronouslyusing the
        //Invokemethod.
        //
        //Ifthecallingthreadisthesameasthethreadthatcreated
        //theTextBoxcontrol,theTextpropertyissetdirectly.
        public delegate void SetTextCallback(string text);

        public void SetTextErr1(string text)
        {
            //InvokeRequiredrequiredcomparesthethreadIDofthe
            //callingthreadtothethreadIDofthecreatingthread.
            //Ifthesethreadsaredifferent,itreturn  strue.

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
            //InvokeRequiredrequiredcomparesthethreadIDofthe
            //callingthreadtothethreadIDofthecreatingthread.
            //Ifthesethreadsaredifferent,itreturn  strue.

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
            Error1.Text += column.Err1;
            Error2.Text += column.Err2;
        }

        public void UpdateSpecs()
        {
            specificationSheet1.SetData();
        }

        internal ColumnDLG(DrawColumn drawColumn)
        {
            this.Column = drawColumn.Column;
            this.drawcolumn = drawColumn;
            InitializeComponent();
            if (specificationSheet1 != null)
                specificationSheet1.drawColumn = drawcolumn;

            //BDColumnData.Add(Column);

            FillProfileData(Column);
            FillDiagnosticData(Column);
            //FillCompositionData(Column, false);

            if (columnDesignerControl1 != null)
                columnDesignerControl1.Initialize(this);

            drawColumn.DesignChanged();
        }

        public void FillProfileData(Column column)
        {
            if (column is null)
                return;

            ComponentProfileDG.Columns.Clear();
            ComponentProfileDG.Columns.Add("Stage", "Stage");
            ComponentProfileDG.Columns.Add("Temp", "DegC");
            ComponentProfileDG.Columns.Add("Liquid", "LiquidRate");
            ComponentProfileDG.Columns.Add("Vapour", "VapourRate");

            if (!column.IsDirty)//donotuseoutofdatedata
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
                        ComponentProfileDG.Rows[row].Cells[2].Value = SignificantDigits.Round(tray.L, 4) / column.FeedRatio;
                        ComponentProfileDG.Rows[row].Cells[3].Value = SignificantDigits.Round(tray.V, 4) / column.FeedRatio;

                        for (int i = 0; i < column.NoComps; i++)//cd.CompCollection[i].Nam
                            ComponentProfileDG.Rows[row].Cells[i + 4].Value = SignificantDigits.Round(tray.LiqComposition[i], 5);
                    }
                }
            }
        }

        public void ClearOldResults()
        {
            ComponentProfileDG.Rows.Clear();
        }

        public void FillDiagnosticData(Column cd)
        {
            DiagnosticDataGridView.Columns.Clear();
            if (cd is null)
                return;

            DiagnosticDataGridView.Columns.Add("Stage", "Stage");
            DiagnosticDataGridView.Columns.Add("Temp", "DegC");
            DiagnosticDataGridView.Columns.Add("Liquid", "LiquidRate");
            DiagnosticDataGridView.Columns.Add("Vapour", "VapourRate");

            DiagnosticDataGridView.Columns.Add("EnergyBalance", "EnergyBalance");
            DiagnosticDataGridView.Columns.Add("LqiuidEnthalpy", "LqiuidEnthalpy");
            DiagnosticDataGridView.Columns.Add("VapourEnthalpy", "VapourEnthalpy");
            DiagnosticDataGridView.Columns.Add("LiquidProducts", "LiquidProducts");
            DiagnosticDataGridView.Columns.Add("VapourProducts", "VapourProducts");

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

        public void FillCompositionData(Column cd, bool Vapour)
        {
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            if (cd is null)
                return;

            dataGridView2.Columns.Add("Stage", "Stage");

            for (int i = 0; i < cd.Components.Count - 1; i++)
            {
                dataGridView2.Columns.Add("Stage" + i.ToString(), "Stage" + i.ToString());
            }

            int count = -1;
            for (int sectionNo = 0; sectionNo < cd.NoSections; sectionNo++)
            {
                TraySection section = cd.TraySections[sectionNo];
                for (int trayNo = 0; trayNo < cd.TraySections[sectionNo].NoTrays; trayNo++)
                {
                    Tray tray = section.Trays[trayNo];
                    count++;
                    int row = dataGridView2.Rows.Add();
                    dataGridView2.Rows[row].Cells[0].Value = count.ToString();
                    if (tray.LiqComposition != null && !Vapour)
                        for (int i = 0; i < tray.LiqComposition.Count(); i++)
                            if (dataGridView2.RowCount > i)
                                dataGridView2.Rows[row].Cells[i].Value = SignificantDigits.Round(section.Trays[trayNo].LiqComposition[i], 4);
                    if (tray.VapComposition != null && Vapour)
                        for (int i = 0; i < tray.LiqComposition.Count(); i++)
                            if (dataGridView2.RowCount > i)
                                dataGridView2.Rows[row].Cells[i].Value = SignificantDigits.Round(section.Trays[trayNo].VapComposition[i], 4);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PumpAroundsFrm2 pafrm2 = new(drawcolumn);
            pafrm2.ShowDialog();
            Column.SolutionConverged = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StripperFrm2 pafrm = new(drawcolumn);
            pafrm.ShowDialog();
            Column.SolutionConverged = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SideStreamFrm pafrm = new(Column);
            pafrm.ShowDialog();
            Column.SolutionConverged = false;
        }

        private void ColumnDLG_Load(object sender, EventArgs e)
        {
            //TopPressure Txt.SetValues(cd.TopPressure );
            //BottomPressure Txt.SetValues(cd.BotPressure );
            loaddata();

            DampFactortxt.Text = Column.TempDampfactor.ToString();
            cbResetInitialTemps.Checked = Column.ResetInitialTemps;
            //column.ValidateDesign();

            //FillEstimateData(column);//needstohavesectionsetcinitisilaisedbyvalidatedesign

            //dgvEstimates.AddColumn("T");

            checkBoxActive.Checked = column.IsActive;
        }

        public void loaddata()
        {
            Error1.Text = Column.ErrHistory1;
            Error2.Text = Column.ErrHistory2;
            FillProfileData(Column);
            //this.Cursor=Cursors.WaitCursor;
        }

        public void UpdateInitConvergence(string newline)
        {
            ErrorConvergnce.Text += newline + "\r";
            ErrorConvergnce.SelectionStart = Error1.TextLength;
            ErrorConvergnce.ScrollToCaret();
            ErrorConvergnce.Refresh();
            ErrorConvergnce.Update();
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

        internal void UpdateSpecs(DrawColumn drawColumn)
        {
            specificationSheet1.SetData(drawColumn);
        }

        private void Tabs_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPage.Name)
            {
                case "TabPageSpecifications":
                    this.WindowState = FormWindowState.Normal;
                    //drawColumn.UpdateAttachedModel();
                    if (!GlobalModel.IsRunning)
                    {
                        specificationSheet1.SetData(drawColumn);
                    }
                    break;

                case "TabPageEstimates":
                    this.WindowState = FormWindowState.Normal;
                    //drawColumn.UpdateAttachedModel();
                    if (!GlobalModel.IsRunning)
                    {
                        FillPressureData();
                        FillEffciencyData();
                    }
                    break;

                case "TabPageTrays":
                    this.WindowState = FormWindowState.Normal;
                    // BDTrays.ResetBindings(false);
                    break;

                case "TabPageConnections":
                    this.WindowState = FormWindowState.Normal;
                    //PopulateConnectionsDataViewGrids();
                    break;

                case "tabStreamConnections":
                    this.WindowState = FormWindowState.Normal;
                    //PopulateTrayConnectionsDataViewGrids();
                    PopulateConnectionsDataViewGrids();
                    break;

                case "TabPageColumnDesigner":
                    this.WindowState = FormWindowState.Normal;
                    PopulateConnectionsDataViewGrids();
                    break;

                case "TabPageExternalStreams":
                    this.WindowState = FormWindowState.Normal;
                    //PopulateConnectionsDataViewGrids();
                    List<StreamMaterial> streamsExternal = drawColumn.GetStreamListFromNodes();
                    worksheet.PortsPropertyWorksheetInitialise(streamsExternal, drawColumn.DrawArea.UOMDisplayList);
                    worksheet.UpdateValues();
                    break;

                case "TabPageinternal Streams":
                    this.WindowState = FormWindowState.Normal;
                    //PopulateConnectionsDataViewGrids();
                    List<StreamMaterial> streamsinternal = new();
                    streamsinternal.AddRange(drawColumn.InternalConnectingStreams.Streams());
                    streamsinternal.AddRange(drawColumn.IntProductStreams.Streams());
                    streamsinternal.AddRange(drawcolumn.IntersectionNetStreams.Streams());
                    streamsinternal.AddRange(drawcolumn.IntersectionDrawStreams.Streams());

                    internalWorksheet.PortsPropertyWorksheetInitialise(streamsinternal, drawColumn.DrawArea.UOMDisplayList);
                    internalWorksheet.UpdateValues();
                    break;

                case "TabPageDesigner":
                    this.WindowState = FormWindowState.Normal;
                    columnDesignerControl1.GraphicsList = drawColumn.graphicslist;
                    columnDesignerControl1.Initialize(this);
                    columnDesignerControl1.DrawArea1.GraphicsList.UpdateAllStreams();
                    columnDesignerControl1.Invalidate();
                    columnDesignerControl1.Refresh();
                    drawColumn.UpdateAttachedModel();
                    break;

                case "TabPageTrayCompositions":
                    dataGridView2.Rows.Clear();
                    dataGridView2.Columns.Clear();
                    for (int i = 0; i < 100; i++)
                    {
                        dataGridView2.Columns.Add("Compon" + i.ToString(), "Compon" + i.ToString());
                    }
                    foreach (TraySection traySection in this.column.TraySections)
                    {
                        for (int i1 = 0; i1 < traySection.Trays.Count; i1++)
                        {
                            Tray tray = traySection.Trays[i1];
                            dataGridView2.Rows.Add();
                            if (tray != null && tray.LiqComposition is not null)
                                for (int i = 0; i < tray.LiqComposition.Count(); i++)
                                {
                                    dataGridView2[i, i1].Value = tray.LiqComposition[i].ToString();
                                }
                        }
                    }

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
                dGVPAConnections.Rows.Clear();

                foreach (DrawMaterialStream ds in drawcolumn.ExtFeedStreams)
                {
                    dGVFeedsConnections.Rows.Add(1);
                    dGVFeedsConnections.Rows[count].Cells[0].Value = ds.Name;

                    ArrayList txt = new();
                    txt.Add("NotConnected");

                    foreach (DrawMaterialStream internalStream in drawcolumn.IntFeedStreams)//seeifisconnected
                    {
                        txt.Add(internalStream.Name);
                        if (drawcolumn.StreamConnectionsGuidDic.TryGetValue(ds.Guid, out connectionGuid))//isitconnected
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
                    txt.Add("NotConnected");           
                    foreach (DrawMaterialStream dsi in drawcolumn.IntProductStreams)
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

                count = 0;
                foreach (DrawMaterialStream ds in drawcolumn.ExportedStreams)
                {
                    dGVPAConnections.Rows.Add(1);
                    dGVPAConnections.Rows[count].Cells[0].Value = ds.Name;

                    ArrayList txt = new();
                    txt.Add("NotConnected");
                    foreach (DrawMaterialStream dsi in drawcolumn.InternalConnectingStreams)
                    {
                        if (dsi is not null)
                        {
                            txt.Add(dsi.Name);
                            if (drawcolumn.StreamConnectionsGuidDic.TryGetValue(ds.Guid, out connectionGuid))
                                if (connectionGuid == dsi.Guid)
                                    dGVPAConnections.Rows[count].Cells[1].Value = dsi.Name;
                        }
                    }

                    DataGridViewComboBoxCell cbcell = (DataGridViewComboBoxCell)dGVPAConnections.Rows[count].Cells[1];

                    cbcell.DataSource = (string[])txt.ToArray(typeof(string));
                    count++;
                }
            }
        
        private void DataGridViewFeedConnections_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                //Removeanexistingevent-handler,ifpresent,toavoid
                //addingmultiplehandlerswhentheeditingcontrolisreused.
                combo.SelectedIndexChanged -= new EventHandler(ComboBox_FeedSelectedIndexChanged);
                //combo.SelectedIndexChanged -= new EventHandler(ComboBox_FeedTrayChanged);

                //Addtheeventhandler.
                combo.SelectedIndexChanged += new EventHandler(ComboBox_FeedSelectedIndexChanged);
            }
        }

        private void DataGridViewProductConnections_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                //Removeanexistingevent-handler,ifpresent,toavoid
                //addingmultiplehandlerswhentheeditingcontrolisreused.
                combo.SelectedIndexChanged -= new EventHandler(ComboBox_ProductSelectedIndexChanged);

                //Addtheeventhandler.
                combo.SelectedIndexChanged += new EventHandler(ComboBox_ProductSelectedIndexChanged);
            }
        }

        private void dGVPAConnections_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                //Removeanexistingevent-handler,ifpresent,toavoid
                //addingmultiplehandlerswhentheeditingcontrolisreused.
                combo.SelectedIndexChanged -= new EventHandler(ComboBox_PASelectedIndexChanged);

                //Addtheeventhandler.
                combo.SelectedIndexChanged += new EventHandler(ComboBox_PASelectedIndexChanged);
            }
        }

        private void ComboBox_TraySelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1)
                return;
            ComboBox combo = (ComboBox)sender;
            string intName = combo.Text;
            string ExtName = dGVFeedsConnections.Rows[((DataGridViewComboBoxEditingControl)sender).EditingControlRowIndex].Cells[0].Value.ToString();

            Guid ExtKey;
            DrawMaterialStream extstream = drawcolumn.ExtFeedStreams[ExtName];
            DrawMaterialStream intstream = drawcolumn.IntFeedStreams[intName];

            if (extstream != null)
            {
                ExtKey = extstream.Guid;
                if (intstream != null)
                    drawcolumn.StreamConnectionsGuidDic[ExtKey] = intstream.Guid;
                else
                    drawcolumn.StreamConnectionsGuidDic[ExtKey] = new Guid();
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
            DrawMaterialStream intstream = drawcolumn.IntFeedStreams[intName];

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
            DrawMaterialStream intstream = drawcolumn.IntProductStreams[intName];

            if (extstream != null)
            {
                ExtKey = extstream.Guid;
                if (intstream != null)
                    drawcolumn.StreamConnectionsGuidDic[ExtKey] = intstream.Guid;
                else
                    drawcolumn.StreamConnectionsGuidDic[ExtKey] = Guid.NewGuid();
            }
        }

        private void ComboBox_PASelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1)
                return;
            ComboBox combo = (ComboBox)sender;
            string intName = combo.Text;
            string ExtName = dGVPAConnections.Rows[((DataGridViewComboBoxEditingControl)sender).EditingControlRowIndex].Cells[0].Value.ToString();

            Guid ExtKey;
            DrawMaterialStream extstream = drawcolumn.ExportedStreams[ExtName];
            DrawMaterialStream intstream = drawcolumn.InternalConnectingStreams[intName];

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
            RusselColumn.RusselSolver r = new();
            r.CalcInitialTs(Column);

            FillPressureData();
        }

        private void BtnResetEstimates_Click(object sender, EventArgs e)
        {
            //dgTrayEstimates.Rows.Clear();
            for (int sectionNo = 0; sectionNo < Column.NoSections; sectionNo++)
            {
                for (int trayNo = 0; trayNo < Column.TraySections[sectionNo].NoTrays; trayNo++)
                {
                    Tray tray = column.TraySections[sectionNo].Trays[trayNo];
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
                Column.TempDampfactor = res;
            else
                Column.TempDampfactor = 1;
        }

        private void TxtMaxOuterIterations_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtMaxOuterIterations.Text, out double res))
                Column.MaxOuterIterations = res;
            else
                Column.MaxOuterIterations = 50;
        }

        private void TxtMaxInnerIterations_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtMaxInnerIterations.Text, out double res))
                Column.MaxTemperatureLoopCount = res;
            else
                Column.MaxTemperatureLoopCount = 50;
        }

        private void TxtInnerTolerance_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtInnerTolerance.Text, out double res))
                Column.SpecificationTolerance = res;
            else
                Column.SpecificationTolerance = 0.1;
        }

        private void TxtOuterTolerance_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtOuterTolerance.Text, out double res))
                Column.TrayTempTolerance = res;
            else
                Column.TrayTempTolerance = 0.1;
        }

        private void CB_SplitMainFeed_CheckedChanged(object sender, EventArgs e)
        {
            Column.SplitMainFeed = CB_SplitMainFeed.Checked;
        }

        private void CB_ResetInitialTemps_CheckedChanged(object sender, EventArgs e)
        {
            Column.ResetInitialTemps = cbResetInitialTemps.Checked;
        }

        private void BtnConverged_Click(object sender, EventArgs e)
        {
            Column.SolutionConverged = false;
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
            //Cursor=Cursors.WaitCursor;
            DisableInputs();
            drawcolumn.Calculate(this, new EventArgs());
            //this.Cursor=Cursors.Default;
        }

        private void DgvEstimates_ValueChanged(object sender, EventArgs e)
        {
            column.InterpolatePressureAllSection();
            DGVPressures.UpdateValues();
        }

        private void ColumnDLG_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DialogResult res = MessageBox.Show("DeleteObjectsY/N", "Delete", MessageBoxButtons.OKCancel);
                    if (res == DialogResult.OK)
                    {
                        drawcolumn.columndrawarea.DeleteSelection();
                        drawColumn.columndrawarea.Refresh();
                    }
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
            column.IsActive = checkBoxActive.Checked;
        }

        private void radioButtonLiquid_CheckedChanged(object sender, EventArgs e)
        {
            FillCompositionData(Column, false);
        }

        private void radioButtonVapour_CheckedChanged(object sender, EventArgs e)
        {
            FillCompositionData(Column, true);
        }

        public void DisableInputs()
        {
            DGVPressures.Enabled = false;
            DGVEfficiencies.Enabled = false;
        }

        public void EnableInputs()
        {
            DGVPressures.Enabled = true;
            DGVEfficiencies.Enabled = true;
        }

        private void dGVPAConnections_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void RB_Temps_CheckedChanged(object sender, EventArgs e)
        {

        }

      
        private void PopulateTrayConnectionsDataViewGrids()
        {
            drawcolumn.RefreshStreams();
            dGVFeedsConnections.Rows.Clear();
            dGVProductsConnections.Rows.Clear();
            dGVPAConnections.Rows.Clear();
            int count = 0;

            foreach (DrawMaterialStream ds in drawcolumn.IntFeedStreams)
            {
                dataGridViewFeedConnectionTray.Rows.Add(1);
                dataGridViewFeedConnectionTray.Rows[count].Cells[0].Value = ds.Name;

                ArrayList txt = new();
                txt.Add("NotConnected");

                DataGridViewComboBoxCell cbcell2 = (DataGridViewComboBoxCell)dataGridViewFeedConnectionTray.Rows[count].Cells[1];
                cbcell2.DataSource = drawcolumn.DrawTraySections[0].TrayNames();


                if (ds.endDrawObject is DrawTray dt)
                {
                    cbcell2.Value = dt.Name;
                }
                count++;
            }

            count = 0;
            foreach (DrawMaterialStream ds in drawcolumn.IntProductStreams)
            {
                dataGridViewProductConnectiontray.Rows.Add(1);
                dataGridViewProductConnectiontray.Rows[count].Cells[0].Value = ds.Name;

                ArrayList txt = new();
                txt.Add("NotConnected");

                DataGridViewComboBoxCell cbcell2 = (DataGridViewComboBoxCell)dataGridViewProductConnectiontray.Rows[count].Cells[1];
                cbcell2.DataSource = drawcolumn.DrawTraySections[0].TrayNames();

                if (ds.startDrawObject is DrawTray dt)
                {
                    cbcell2.Value = dt.Name;
                }
                count++;
            }
        }

        private void dataGridViewFeedConnectionTray_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                combo.SelectedIndexChanged -= new EventHandler(ComboBox_FeedTrayChanged);
                combo.SelectedIndexChanged += new EventHandler(ComboBox_FeedTrayChanged);
            }
        }

        private void ComboBox_FeedTrayChanged(object sender, EventArgs e)
        {
            int streamindex = 0;

            if (sender is DataGridViewComboBoxEditingControl dge1)
                if (dge1.Parent.Parent is DataGridView dgv)
                    streamindex = dgv.CurrentCell.RowIndex;

            if (((ComboBox)sender).SelectedIndex == -1)
                return;

            ComboBox combo = (ComboBox)sender;
            string TrayName = combo.Text;

            int TrayIndex = drawcolumn.MainSection.TrayNames().ToList().IndexOf(TrayName);
            if (TrayIndex < 0) return;

            DrawTray dt = drawcolumn.MainSection.DrawTrays[TrayIndex];

            DrawBaseStream ds = drawcolumn.IntFeedStreams[streamindex];

            ds.endDrawObject = dt;
            ds.endDrawObjectGuid = dt.Guid;

            if (ds.EndNode is null)
            {
                ds.EndNode = new Node();
                ds.EndNode.NodeType = HotSpotType.Feed;
            }

            switch (ds.EndNode.NodeType)
            {
                case HotSpotType.Feed:
                    ds.EndNode = dt.FeedLeft;
                    ds.EndNodeGuid = dt.FeedLeft.Guid;
                    break;

                case HotSpotType.LiquidDraw:
                    break;

                case HotSpotType.Stream:
                    break;

                case HotSpotType.Water:
                    break;

                case HotSpotType.Floating:
                    break;

                case HotSpotType.VapourDraw:
                    break;

                case HotSpotType.EnergyIn:
                    break;

                case HotSpotType.EnergyOut:
                    break;

                case HotSpotType.FeedRight:
                    break;

                case HotSpotType.TrayNetLiquid:
                    break;

                case HotSpotType.TrayNetVapour:
                    break;

                case HotSpotType.BottomTrayLiquid:
                    break;

                case HotSpotType.TopTrayVapour:
                    break;

                case HotSpotType.LiquidDrawLeft:
                    break;

                case HotSpotType.None:
                    ds.EndNode = dt.FeedLeft;
                    ds.EndNodeGuid = dt.FeedLeft.Guid;
                    break;

                case HotSpotType.SignalIn:
                    break;

                case HotSpotType.SignalOut:
                    break;

                case HotSpotType.MaterialStream:
                    break;

                case HotSpotType.Signal:
                    break;

                case HotSpotType.InternalExport:
                    break;

                default:
                    break;
            }

            this.Refresh();
            drawColumn.UpdateAttachedModel();
        }

        private void dataGridViewProductConnectiontray_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                combo.SelectedIndexChanged -= new EventHandler(ComboBox_ProductTrayChanged);
                combo.SelectedIndexChanged += new EventHandler(ComboBox_ProductTrayChanged);
            }
        }

        private void ComboBox_ProductTrayChanged(object sender, EventArgs e)
        {
            int streamindex = 0;

            if (sender is DataGridViewComboBoxEditingControl dge1)
                if (dge1.Parent.Parent is DataGridView dgv)
                    streamindex = dgv.CurrentCell.RowIndex;

            if (((ComboBox)sender).SelectedIndex == -1)
                return;

            ComboBox combo = (ComboBox)sender;
            string TrayName = combo.Text;
            int TrayIndex = drawcolumn.MainSection.TrayNames().ToList().IndexOf(TrayName);
            DrawTray dt = drawcolumn.MainSection.DrawTrays[TrayIndex];

            DrawBaseStream ds = drawcolumn.IntProductStreams[streamindex];
            ds.startDrawObject = dt;
            ds.StartDrawObjectGuid = dt.Guid;

            switch (ds.StartNode.NodeType)
            {
                case HotSpotType.Feed:
                    break;

                case HotSpotType.LiquidDraw:
                    ds.StartNode = dt.LiquidDrawRight;
                    ds.startNodeGuid = dt.LiquidDrawRight.Guid;
                    break;

                case HotSpotType.Stream:
                    break;

                case HotSpotType.Water:
                    break;

                case HotSpotType.Floating:
                    break;

                case HotSpotType.VapourDraw:
                    break;

                case HotSpotType.EnergyIn:
                    break;

                case HotSpotType.EnergyOut:
                    break;

                case HotSpotType.FeedRight:
                    break;

                case HotSpotType.TrayNetLiquid:
                    break;

                case HotSpotType.TrayNetVapour:
                    break;

                case HotSpotType.BottomTrayLiquid:
                    break;

                case HotSpotType.TopTrayVapour:
                    break;

                case HotSpotType.LiquidDrawLeft:
                    ds.StartNode = dt.LiquidDrawLeft;
                    ds.startNodeGuid = dt.LiquidDrawLeft.Guid;
                    break;

                case HotSpotType.None:
                    ds.StartNode = dt.LiquidDrawRight;
                    ds.startNodeGuid = dt.LiquidDrawRight.Guid;
                    break;

                case HotSpotType.SignalIn:
                    break;

                case HotSpotType.SignalOut:
                    break;

                case HotSpotType.MaterialStream:
                    break;

                case HotSpotType.Signal:
                    break;

                case HotSpotType.InternalExport:
                    break;

                default:
                    break;
            }
            this.Refresh();
            drawColumn.UpdateAttachedModel();
        }
    }
}