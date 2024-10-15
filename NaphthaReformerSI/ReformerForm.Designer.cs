
using Units;

namespace NaphthaReformerSI
{
    partial class ReformerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReformerForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            tcSimulation = new System.Windows.Forms.TabControl();
            Feed = new System.Windows.Forms.TabPage();
            CalibFactors = new FormControls.UserPropGrid();
            NoRx = new FormControls.UserPropGrid();
            FurnEff = new FormControls.UserPropGrid();
            CatData = new FormControls.UserPropGrid();
            RecData = new FormControls.UserPropGrid();
            SepConditions = new FormControls.UserPropGrid();
            RxData = new FormControls.UserPropGrid();
            shortMediumFull1 = new DialogControls.StreamAnalysysType();
            btnFeed = new System.Windows.Forms.Button();
            Products = new System.Windows.Forms.TabPage();
            MassBalance = new FormControls.UserPropGrid();
            FurnaceData = new FormControls.UserPropGrid();
            FeedSummary = new FormControls.UserPropGrid();
            SeparatorResults = new FormControls.UserPropGrid();
            ProductData = new FormControls.UserPropGrid();
            RxDataResults = new FormControls.UserPropGrid();
            Calibration = new System.Windows.Forms.TabPage();
            btnCalibrate = new System.Windows.Forms.Button();
            btnData = new System.Windows.Forms.Button();
            Worksheet = new System.Windows.Forms.TabPage();
            DetResults = new System.Windows.Forms.TabPage();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            groupBox6 = new System.Windows.Forms.GroupBox();
            radioButton8 = new System.Windows.Forms.RadioButton();
            radioButton9 = new System.Windows.Forms.RadioButton();
            DGVResults1 = new System.Windows.Forms.DataGridView();
            Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Rx2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Rx3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Rx4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Plots = new System.Windows.Forms.TabPage();
            chart1 = new Charts.ChartControl();
            massMolarVol1 = new DialogControls.MassMolarVol();
            groupBox9 = new System.Windows.Forms.GroupBox();
            DGVChartComponents = new System.Windows.Forms.DataGridView();
            Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            splitter1 = new System.Windows.Forms.Splitter();
            btnNewCase = new System.Windows.Forms.Button();
            btnLoadData = new System.Windows.Forms.Button();
            btnSave = new System.Windows.Forms.Button();
            btnSolveDef = new System.Windows.Forms.Button();
            btDeleteCase = new System.Windows.Forms.Button();
            btCloneCase = new System.Windows.Forms.Button();
            cbCase = new System.Windows.Forms.Label();
            cBCaseSelection = new System.Windows.Forms.ComboBox();
            btSolveCase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tcSimulation.SuspendLayout();
            Feed.SuspendLayout();
            Products.SuspendLayout();
            Calibration.SuspendLayout();
            DetResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DGVResults1).BeginInit();
            Plots.SuspendLayout();
            groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DGVChartComponents).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn1.HeaderText = "Feed";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn2.HeaderText = "Recycle";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn3.HeaderText = "Combined Feed";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn4.HeaderText = "R2 Feed";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn5.HeaderText = "R3 Feed";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn6.HeaderText = "R4 Feed";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn7.HeaderText = "Last Rx Effluent";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn8.HeaderText = "Separator Bottoms";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn9.HeaderText = "Net Gas";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(tcSimulation);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(btnNewCase);
            splitContainer2.Panel2.Controls.Add(btnLoadData);
            splitContainer2.Panel2.Controls.Add(btnSave);
            splitContainer2.Panel2.Controls.Add(btnSolveDef);
            splitContainer2.Panel2.Controls.Add(btDeleteCase);
            splitContainer2.Panel2.Controls.Add(btCloneCase);
            splitContainer2.Panel2.Controls.Add(cbCase);
            splitContainer2.Panel2.Controls.Add(cBCaseSelection);
            splitContainer2.Panel2.Controls.Add(btSolveCase);
            splitContainer2.Size = new System.Drawing.Size(1198, 692);
            splitContainer2.SplitterDistance = 609;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 3;
            // 
            // tcSimulation
            // 
            tcSimulation.Controls.Add(Feed);
            tcSimulation.Controls.Add(Products);
            tcSimulation.Controls.Add(Calibration);
            tcSimulation.Controls.Add(Worksheet);
            tcSimulation.Controls.Add(DetResults);
            tcSimulation.Controls.Add(Plots);
            tcSimulation.Dock = System.Windows.Forms.DockStyle.Fill;
            tcSimulation.Location = new System.Drawing.Point(0, 0);
            tcSimulation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tcSimulation.Name = "tcSimulation";
            tcSimulation.SelectedIndex = 0;
            tcSimulation.Size = new System.Drawing.Size(1198, 609);
            tcSimulation.TabIndex = 2;
            // 
            // Feed
            // 
            Feed.BackColor = System.Drawing.SystemColors.Control;
            Feed.Controls.Add(CalibFactors);
            Feed.Controls.Add(NoRx);
            Feed.Controls.Add(FurnEff);
            Feed.Controls.Add(CatData);
            Feed.Controls.Add(RecData);
            Feed.Controls.Add(SepConditions);
            Feed.Controls.Add(RxData);
            Feed.Controls.Add(shortMediumFull1);
            Feed.Controls.Add(btnFeed);
            Feed.Location = new System.Drawing.Point(4, 24);
            Feed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Feed.Name = "Feed";
            Feed.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Feed.Size = new System.Drawing.Size(1190, 581);
            Feed.TabIndex = 0;
            Feed.Text = "Simulation";
            Feed.Click += Feed_Click_1;
            // 
            // CalibFactors
            // 
            CalibFactors.AllowChangeEvent = true;
            CalibFactors.AllowUserToAddRows = false;
            CalibFactors.AllowUserToDeleteRows = false;
            CalibFactors.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("CalibFactors.ColumnNames");
            CalibFactors.DisplayTitles = true;
            CalibFactors.FirstColumnWidth = 80;
            CalibFactors.Location = new System.Drawing.Point(237, 12);
            CalibFactors.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CalibFactors.Name = "CalibFactors";
            CalibFactors.ReadOnly = false;
            CalibFactors.RowHeadersVisible = false;
            CalibFactors.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("CalibFactors.RowNames");
            CalibFactors.Size = new System.Drawing.Size(195, 510);
            CalibFactors.TabIndex = 25;
            CalibFactors.TopText = "Catalyst Factors";
            // 
            // NoRx
            // 
            NoRx.AllowChangeEvent = true;
            NoRx.AllowUserToAddRows = false;
            NoRx.AllowUserToDeleteRows = false;
            NoRx.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("NoRx.ColumnNames");
            NoRx.DisplayTitles = true;
            NoRx.FirstColumnWidth = 64;
            NoRx.Location = new System.Drawing.Point(664, 182);
            NoRx.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NoRx.Name = "NoRx";
            NoRx.ReadOnly = false;
            NoRx.RowHeadersVisible = false;
            NoRx.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("NoRx.RowNames");
            NoRx.Size = new System.Drawing.Size(216, 128);
            NoRx.TabIndex = 24;
            NoRx.TopText = "No Rx";
            NoRx.Load += NoRx1_Load;
            // 
            // FurnEff
            // 
            FurnEff.AllowChangeEvent = true;
            FurnEff.AllowUserToAddRows = false;
            FurnEff.AllowUserToDeleteRows = false;
            FurnEff.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("FurnEff.ColumnNames");
            FurnEff.DisplayTitles = true;
            FurnEff.FirstColumnWidth = 64;
            FurnEff.Location = new System.Drawing.Point(888, 182);
            FurnEff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FurnEff.Name = "FurnEff";
            FurnEff.ReadOnly = false;
            FurnEff.RowHeadersVisible = false;
            FurnEff.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("FurnEff.RowNames");
            FurnEff.Size = new System.Drawing.Size(216, 128);
            FurnEff.TabIndex = 23;
            FurnEff.TopText = "Furnace Efficiency";
            FurnEff.Load += FurnEff1_Load;
            // 
            // CatData
            // 
            CatData.AllowChangeEvent = true;
            CatData.AllowUserToAddRows = false;
            CatData.AllowUserToDeleteRows = false;
            CatData.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("CatData.ColumnNames");
            CatData.DisplayTitles = true;
            CatData.FirstColumnWidth = 64;
            CatData.Location = new System.Drawing.Point(440, 12);
            CatData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CatData.Name = "CatData";
            CatData.ReadOnly = false;
            CatData.RowHeadersVisible = false;
            CatData.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("CatData.RowNames");
            CatData.Size = new System.Drawing.Size(342, 164);
            CatData.TabIndex = 22;
            CatData.TopText = "Catalyst Data";
            // 
            // RecData
            // 
            RecData.AllowChangeEvent = true;
            RecData.AllowUserToAddRows = false;
            RecData.AllowUserToDeleteRows = false;
            RecData.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("RecData.ColumnNames");
            RecData.DisplayTitles = true;
            RecData.FirstColumnWidth = 64;
            RecData.Location = new System.Drawing.Point(798, 12);
            RecData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RecData.Name = "RecData";
            RecData.ReadOnly = false;
            RecData.RowHeadersVisible = false;
            RecData.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("RecData.RowNames");
            RecData.Size = new System.Drawing.Size(306, 164);
            RecData.TabIndex = 21;
            RecData.TopText = "Recyle Data";
            RecData.Load += RecData1_Load;
            // 
            // SepConditions
            // 
            SepConditions.AllowChangeEvent = true;
            SepConditions.AllowUserToAddRows = false;
            SepConditions.AllowUserToDeleteRows = false;
            SepConditions.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("SepConditions.ColumnNames");
            SepConditions.DisplayTitles = true;
            SepConditions.FirstColumnWidth = 64;
            SepConditions.Location = new System.Drawing.Point(440, 182);
            SepConditions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SepConditions.Name = "SepConditions";
            SepConditions.ReadOnly = false;
            SepConditions.RowHeadersVisible = false;
            SepConditions.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("SepConditions.RowNames");
            SepConditions.Size = new System.Drawing.Size(216, 128);
            SepConditions.TabIndex = 20;
            SepConditions.TopText = "Separator Conditions";
            // 
            // RxData
            // 
            RxData.AllowChangeEvent = true;
            RxData.AllowUserToAddRows = false;
            RxData.AllowUserToDeleteRows = false;
            RxData.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("RxData.ColumnNames");
            RxData.DisplayTitles = true;
            RxData.FirstColumnWidth = 150;
            RxData.Location = new System.Drawing.Point(440, 316);
            RxData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RxData.Name = "RxData";
            RxData.ReadOnly = false;
            RxData.RowHeadersVisible = false;
            RxData.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("RxData.RowNames");
            RxData.Size = new System.Drawing.Size(664, 206);
            RxData.TabIndex = 18;
            RxData.TopText = "RxData";
            // 
            // shortMediumFull1
            // 
            shortMediumFull1.Location = new System.Drawing.Point(28, 12);
            shortMediumFull1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            shortMediumFull1.Name = "shortMediumFull1";
            shortMediumFull1.Size = new System.Drawing.Size(201, 112);
            shortMediumFull1.TabIndex = 17;
            shortMediumFull1.Value = enumShortMediumFull.Short;
            // 
            // btnFeed
            // 
            btnFeed.Location = new System.Drawing.Point(28, 143);
            btnFeed.Name = "btnFeed";
            btnFeed.Size = new System.Drawing.Size(109, 33);
            btnFeed.TabIndex = 16;
            btnFeed.Text = "Feed";
            btnFeed.UseVisualStyleBackColor = true;
            btnFeed.Click += Feed_Click;
            // 
            // Products
            // 
            Products.BackColor = System.Drawing.SystemColors.Control;
            Products.Controls.Add(MassBalance);
            Products.Controls.Add(FurnaceData);
            Products.Controls.Add(FeedSummary);
            Products.Controls.Add(SeparatorResults);
            Products.Controls.Add(ProductData);
            Products.Controls.Add(RxDataResults);
            Products.Location = new System.Drawing.Point(4, 24);
            Products.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Products.Name = "Products";
            Products.Size = new System.Drawing.Size(1190, 581);
            Products.TabIndex = 3;
            Products.Text = "Results";
            // 
            // MassBalance
            // 
            MassBalance.AllowChangeEvent = true;
            MassBalance.AllowUserToAddRows = false;
            MassBalance.AllowUserToDeleteRows = false;
            MassBalance.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("MassBalance.ColumnNames");
            MassBalance.DisplayTitles = true;
            MassBalance.FirstColumnWidth = 64;
            MassBalance.Location = new System.Drawing.Point(752, 26);
            MassBalance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MassBalance.Name = "MassBalance";
            MassBalance.ReadOnly = false;
            MassBalance.RowHeadersVisible = false;
            MassBalance.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("MassBalance.RowNames");
            MassBalance.Size = new System.Drawing.Size(216, 463);
            MassBalance.TabIndex = 21;
            MassBalance.TopText = "Mass Balance";
            // 
            // FurnaceData
            // 
            FurnaceData.AllowChangeEvent = true;
            FurnaceData.AllowUserToAddRows = false;
            FurnaceData.AllowUserToDeleteRows = false;
            FurnaceData.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("FurnaceData.ColumnNames");
            FurnaceData.DisplayTitles = true;
            FurnaceData.FirstColumnWidth = 64;
            FurnaceData.Location = new System.Drawing.Point(63, 280);
            FurnaceData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FurnaceData.Name = "FurnaceData";
            FurnaceData.ReadOnly = false;
            FurnaceData.RowHeadersVisible = false;
            FurnaceData.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("FurnaceData.RowNames");
            FurnaceData.Size = new System.Drawing.Size(647, 79);
            FurnaceData.TabIndex = 20;
            FurnaceData.TopText = "Furnace Data";
            // 
            // FeedSummary
            // 
            FeedSummary.AllowChangeEvent = true;
            FeedSummary.AllowUserToAddRows = false;
            FeedSummary.AllowUserToDeleteRows = false;
            FeedSummary.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("FeedSummary.ColumnNames");
            FeedSummary.DisplayTitles = true;
            FeedSummary.FirstColumnWidth = 64;
            FeedSummary.Location = new System.Drawing.Point(63, 26);
            FeedSummary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FeedSummary.Name = "FeedSummary";
            FeedSummary.ReadOnly = false;
            FeedSummary.RowHeadersVisible = false;
            FeedSummary.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("FeedSummary.RowNames");
            FeedSummary.Size = new System.Drawing.Size(216, 245);
            FeedSummary.TabIndex = 3;
            FeedSummary.TopText = "Feed Summary";
            // 
            // SeparatorResults
            // 
            SeparatorResults.AllowChangeEvent = true;
            SeparatorResults.AllowUserToAddRows = false;
            SeparatorResults.AllowUserToDeleteRows = false;
            SeparatorResults.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("SeparatorResults.ColumnNames");
            SeparatorResults.DisplayTitles = true;
            SeparatorResults.FirstColumnWidth = 64;
            SeparatorResults.Location = new System.Drawing.Point(309, 377);
            SeparatorResults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SeparatorResults.Name = "SeparatorResults";
            SeparatorResults.ReadOnly = false;
            SeparatorResults.RowHeadersVisible = false;
            SeparatorResults.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("SeparatorResults.RowNames");
            SeparatorResults.Size = new System.Drawing.Size(401, 112);
            SeparatorResults.TabIndex = 2;
            SeparatorResults.TopText = "Separator Results";
            // 
            // ProductData
            // 
            ProductData.AllowChangeEvent = true;
            ProductData.AllowUserToAddRows = false;
            ProductData.AllowUserToDeleteRows = false;
            ProductData.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("ProductData.ColumnNames");
            ProductData.DisplayTitles = true;
            ProductData.FirstColumnWidth = 64;
            ProductData.Location = new System.Drawing.Point(63, 377);
            ProductData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ProductData.Name = "ProductData";
            ProductData.ReadOnly = false;
            ProductData.RowHeadersVisible = false;
            ProductData.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("ProductData.RowNames");
            ProductData.Size = new System.Drawing.Size(216, 188);
            ProductData.TabIndex = 1;
            ProductData.TopText = "Product Data";
            // 
            // RxDataResults
            // 
            RxDataResults.AllowChangeEvent = true;
            RxDataResults.AllowUserToAddRows = false;
            RxDataResults.AllowUserToDeleteRows = false;
            RxDataResults.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("RxDataResults.ColumnNames");
            RxDataResults.DisplayTitles = true;
            RxDataResults.FirstColumnWidth = 64;
            RxDataResults.Location = new System.Drawing.Point(287, 26);
            RxDataResults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RxDataResults.Name = "RxDataResults";
            RxDataResults.ReadOnly = false;
            RxDataResults.RowHeadersVisible = false;
            RxDataResults.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("RxDataResults.RowNames");
            RxDataResults.Size = new System.Drawing.Size(423, 248);
            RxDataResults.TabIndex = 0;
            RxDataResults.TopText = "Reactor Results";
            // 
            // Calibration
            // 
            Calibration.BackColor = System.Drawing.SystemColors.Control;
            Calibration.Controls.Add(btnCalibrate);
            Calibration.Controls.Add(btnData);
            Calibration.Location = new System.Drawing.Point(4, 24);
            Calibration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Calibration.Name = "Calibration";
            Calibration.Size = new System.Drawing.Size(1190, 581);
            Calibration.TabIndex = 4;
            Calibration.Text = "Calibration";
            // 
            // btnCalibrate
            // 
            btnCalibrate.Location = new System.Drawing.Point(209, 137);
            btnCalibrate.Name = "btnCalibrate";
            btnCalibrate.Size = new System.Drawing.Size(109, 33);
            btnCalibrate.TabIndex = 21;
            btnCalibrate.Text = "Calibrate";
            btnCalibrate.UseVisualStyleBackColor = true;
            btnCalibrate.Click += btnCalibrate_Click;
            // 
            // btnData
            // 
            btnData.Location = new System.Drawing.Point(209, 98);
            btnData.Name = "btnData";
            btnData.Size = new System.Drawing.Size(109, 33);
            btnData.TabIndex = 20;
            btnData.Text = "Data";
            btnData.UseVisualStyleBackColor = true;
            btnData.Click += btnData_Click;
            // 
            // Worksheet
            // 
            Worksheet.BackColor = System.Drawing.SystemColors.Control;
            Worksheet.Location = new System.Drawing.Point(4, 24);
            Worksheet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Worksheet.Name = "Worksheet";
            Worksheet.Size = new System.Drawing.Size(1190, 581);
            Worksheet.TabIndex = 2;
            Worksheet.Text = "Worksheet";
            // 
            // DetResults
            // 
            DetResults.BackColor = System.Drawing.Color.LightGray;
            DetResults.Controls.Add(splitContainer1);
            DetResults.Location = new System.Drawing.Point(4, 24);
            DetResults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DetResults.Name = "DetResults";
            DetResults.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DetResults.Size = new System.Drawing.Size(1190, 581);
            DetResults.TabIndex = 6;
            DetResults.Text = "Detailed Stream Results";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(4, 3);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            splitContainer1.Panel1.Controls.Add(groupBox6);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(DGVResults1);
            splitContainer1.Size = new System.Drawing.Size(1182, 575);
            splitContainer1.SplitterDistance = 109;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(radioButton8);
            groupBox6.Controls.Add(radioButton9);
            groupBox6.Location = new System.Drawing.Point(218, 11);
            groupBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox6.Size = new System.Drawing.Size(239, 95);
            groupBox6.TabIndex = 14;
            groupBox6.TabStop = false;
            groupBox6.Text = "Results Basis";
            // 
            // radioButton8
            // 
            radioButton8.AutoSize = true;
            radioButton8.Location = new System.Drawing.Point(28, 54);
            radioButton8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButton8.Name = "radioButton8";
            radioButton8.Size = new System.Drawing.Size(68, 19);
            radioButton8.TabIndex = 1;
            radioButton8.Text = "Fraction";
            radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            radioButton9.AutoSize = true;
            radioButton9.Checked = true;
            radioButton9.Location = new System.Drawing.Point(28, 28);
            radioButton9.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButton9.Name = "radioButton9";
            radioButton9.Size = new System.Drawing.Size(50, 19);
            radioButton9.TabIndex = 0;
            radioButton9.TabStop = true;
            radioButton9.Text = "Flow";
            radioButton9.UseVisualStyleBackColor = true;
            // 
            // DGVResults1
            // 
            DGVResults1.AllowUserToAddRows = false;
            DGVResults1.AllowUserToDeleteRows = false;
            DGVResults1.AllowUserToResizeColumns = false;
            DGVResults1.AllowUserToResizeRows = false;
            DGVResults1.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            DGVResults1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DGVResults1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGVResults1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Column1, Column2, Column9, Rx2, Rx3, Rx4, Column6, Column7, Column8 });
            DGVResults1.Dock = System.Windows.Forms.DockStyle.Fill;
            DGVResults1.Location = new System.Drawing.Point(0, 0);
            DGVResults1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DGVResults1.Name = "DGVResults1";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            DGVResults1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            DGVResults1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            DGVResults1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            DGVResults1.Size = new System.Drawing.Size(1182, 461);
            DGVResults1.TabIndex = 1;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Column1.HeaderText = "Feed";
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Column2.HeaderText = "Recycle";
            Column2.Name = "Column2";
            // 
            // Column9
            // 
            Column9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Column9.HeaderText = "Combined Feed";
            Column9.Name = "Column9";
            // 
            // Rx2
            // 
            Rx2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Rx2.HeaderText = "R2 Feed";
            Rx2.Name = "Rx2";
            // 
            // Rx3
            // 
            Rx3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Rx3.HeaderText = "R3 Feed";
            Rx3.Name = "Rx3";
            // 
            // Rx4
            // 
            Rx4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Rx4.HeaderText = "R4 Feed";
            Rx4.Name = "Rx4";
            // 
            // Column6
            // 
            Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Column6.HeaderText = "Last Rx Effluent";
            Column6.Name = "Column6";
            // 
            // Column7
            // 
            Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Column7.HeaderText = "Separator Bottoms";
            Column7.Name = "Column7";
            // 
            // Column8
            // 
            Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Column8.HeaderText = "Net Gas";
            Column8.Name = "Column8";
            // 
            // Plots
            // 
            Plots.BackColor = System.Drawing.SystemColors.Control;
            Plots.Controls.Add(chart1);
            Plots.Controls.Add(massMolarVol1);
            Plots.Controls.Add(groupBox9);
            Plots.Controls.Add(splitter1);
            Plots.Location = new System.Drawing.Point(4, 24);
            Plots.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Plots.Name = "Plots";
            Plots.Size = new System.Drawing.Size(1190, 581);
            Plots.TabIndex = 9;
            Plots.Text = "Component Plots";
            // 
            // chart
            // 
            chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            chart1.Location = new System.Drawing.Point(239, 0);
            chart1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chart1.Name = "chart";
            chart1.Size = new System.Drawing.Size(951, 581);
            chart1.TabIndex = 4;
            // 
            // massMolarVol1
            // 
            massMolarVol1.Location = new System.Drawing.Point(10, 3);
            massMolarVol1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            massMolarVol1.Name = "massMolarVol1";
            massMolarVol1.Size = new System.Drawing.Size(201, 114);
            massMolarVol1.TabIndex = 3;
            massMolarVol1.Value = enumMassMolarOrVol.Molar;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(DGVChartComponents);
            groupBox9.Location = new System.Drawing.Point(4, 153);
            groupBox9.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox9.Name = "groupBox9";
            groupBox9.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox9.Size = new System.Drawing.Size(214, 472);
            groupBox9.TabIndex = 1;
            groupBox9.TabStop = false;
            groupBox9.Text = "Component";
            // 
            // DGVChartComponents
            // 
            DGVChartComponents.AllowUserToAddRows = false;
            DGVChartComponents.AllowUserToDeleteRows = false;
            DGVChartComponents.AllowUserToResizeRows = false;
            DGVChartComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGVChartComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Column3, Column4 });
            DGVChartComponents.Location = new System.Drawing.Point(0, 22);
            DGVChartComponents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DGVChartComponents.Name = "DGVChartComponents";
            DGVChartComponents.RowHeadersVisible = false;
            DGVChartComponents.Size = new System.Drawing.Size(206, 629);
            DGVChartComponents.TabIndex = 0;
            DGVChartComponents.CellContentClick += DGVChartComponents_CellContentClick;
            // 
            // Column3
            // 
            Column3.FillWeight = 50F;
            Column3.HeaderText = "Plot";
            Column3.Name = "Column3";
            Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            Column3.Width = 50;
            // 
            // Column4
            // 
            Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Column4.HeaderText = "Component";
            Column4.Name = "Column4";
            // 
            // splitter1
            // 
            splitter1.Location = new System.Drawing.Point(0, 0);
            splitter1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(239, 581);
            splitter1.TabIndex = 0;
            splitter1.TabStop = false;
            // 
            // btnNewCase
            // 
            btnNewCase.Location = new System.Drawing.Point(753, 18);
            btnNewCase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnNewCase.Name = "btnNewCase";
            btnNewCase.Size = new System.Drawing.Size(126, 46);
            btnNewCase.TabIndex = 19;
            btnNewCase.Text = "New Case";
            btnNewCase.UseVisualStyleBackColor = true;
            btnNewCase.Click += btnNewCase_Click;
            // 
            // btnLoadData
            // 
            btnLoadData.Location = new System.Drawing.Point(631, 18);
            btnLoadData.Name = "btnLoadData";
            btnLoadData.Size = new System.Drawing.Size(115, 46);
            btnLoadData.TabIndex = 18;
            btnLoadData.Text = "Load Data";
            btnLoadData.UseVisualStyleBackColor = true;
            btnLoadData.Click += btnLoadData_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(510, 18);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(115, 46);
            btnSave.TabIndex = 17;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnSolveDef
            // 
            btnSolveDef.Location = new System.Drawing.Point(887, 18);
            btnSolveDef.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSolveDef.Name = "btnSolveDef";
            btnSolveDef.Size = new System.Drawing.Size(126, 46);
            btnSolveDef.TabIndex = 16;
            btnSolveDef.Text = "Solve Default";
            btnSolveDef.UseVisualStyleBackColor = true;
            btnSolveDef.Click += SolveOld_Click;
            // 
            // btDeleteCase
            // 
            btDeleteCase.Location = new System.Drawing.Point(377, 18);
            btDeleteCase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btDeleteCase.Name = "btDeleteCase";
            btDeleteCase.Size = new System.Drawing.Size(126, 46);
            btDeleteCase.TabIndex = 15;
            btDeleteCase.Text = "Delete Case";
            btDeleteCase.UseVisualStyleBackColor = true;
            btDeleteCase.Click += BtDeleteCase_Click;
            // 
            // btCloneCase
            // 
            btCloneCase.Location = new System.Drawing.Point(244, 18);
            btCloneCase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btCloneCase.Name = "btCloneCase";
            btCloneCase.Size = new System.Drawing.Size(126, 46);
            btCloneCase.TabIndex = 14;
            btCloneCase.Text = "Clone Case";
            btCloneCase.UseVisualStyleBackColor = true;
            btCloneCase.Click += CloneDatset_Click;
            // 
            // cbCase
            // 
            cbCase.AutoSize = true;
            cbCase.Location = new System.Drawing.Point(14, 13);
            cbCase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            cbCase.Name = "cbCase";
            cbCase.Size = new System.Drawing.Size(82, 15);
            cbCase.TabIndex = 13;
            cbCase.Text = "Selected Case:";
            // 
            // cBCaseSelection
            // 
            cBCaseSelection.FormattingEnabled = true;
            cBCaseSelection.Location = new System.Drawing.Point(14, 41);
            cBCaseSelection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cBCaseSelection.Name = "cBCaseSelection";
            cBCaseSelection.Size = new System.Drawing.Size(204, 23);
            cBCaseSelection.TabIndex = 12;
            cBCaseSelection.SelectedIndexChanged += CBCaseSelection_SelectedIndexChanged;
            // 
            // btSolveCase
            // 
            btSolveCase.Location = new System.Drawing.Point(1026, 18);
            btSolveCase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btSolveCase.Name = "btSolveCase";
            btSolveCase.Size = new System.Drawing.Size(126, 46);
            btSolveCase.TabIndex = 11;
            btSolveCase.Text = "Solve";
            btSolveCase.UseVisualStyleBackColor = true;
            btSolveCase.Click += SolveCase_Click;
            // 
            // ReformerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1198, 692);
            Controls.Add(splitContainer2);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ReformerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Reformer";
            Load += Form1_Load;
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tcSimulation.ResumeLayout(false);
            Feed.ResumeLayout(false);
            Products.ResumeLayout(false);
            Calibration.ResumeLayout(false);
            DetResults.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DGVResults1).EndInit();
            Plots.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DGVChartComponents).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tcSimulation;
        private System.Windows.Forms.TabPage Feed;
        private System.Windows.Forms.TabPage Products;
        private System.Windows.Forms.TabPage Worksheet;
        private System.Windows.Forms.TabPage Calibration;
        private System.Windows.Forms.TabPage DetResults;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.DataGridView DGVResults1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rx2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rx3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rx4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.TabPage Plots;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.DataGridView DGVChartComponents;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button btSolveCase;
        private System.Windows.Forms.Label cbCase;
        private System.Windows.Forms.ComboBox cBCaseSelection;
        private System.Windows.Forms.Button btDeleteCase;
        private System.Windows.Forms.Button btCloneCase;
        private System.Windows.Forms.Button btnSolveDef;
        private System.Windows.Forms.Button btnFeed;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.Button btnCalibrate;
        private DialogControls.StreamAnalysysType shortMediumFull1;
        private FormControls.UserPropGrid RecData;
        private FormControls.UserPropGrid CatData;
        private FormControls.UserPropGrid MassBalance;
        private FormControls.UserPropGrid CalibFactors;
        private FormControls.UserPropGrid NoRx;
        private FormControls.UserPropGrid FurnEff;
        private FormControls.UserPropGrid SepConditions;
        private FormControls.UserPropGrid RxData;
        private FormControls.UserPropGrid FurnaceData;
        private FormControls.UserPropGrid FeedSummary;
        private FormControls.UserPropGrid SeparatorResults;
        private FormControls.UserPropGrid ProductData;
        private FormControls.UserPropGrid RxDataResults;
        private DialogControls.MassMolarVol massMolarVol1;
        private System.Windows.Forms.Button btnNewCase;
        private Charts.ChartControl chart1;
    }
}