using ModelEngine;
using System.Windows.Forms;
using Units.PortForm;

namespace Units
{
    internal partial class ColumnDLG
    {
        ///<summary>
        ///Requireddesignervariable.
        ///</summary>
        private System.ComponentModel.IContainer Components = null;

        ///<summary>
        ///Cleanupanyresourcesbeingused.
        ///</summary>
        ///<paramname="disposing">trueifmanagedresourcesshouldbedisposed;otherwise,false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region WindowsFormDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColumnDLG));
            Tabs = new TabControl();
            TabePageSolver = new TabPage();
            label12 = new Label();
            ErrorConvergnce = new RichTextBox();
            checkBoxActive = new CheckBox();
            button11 = new Button();
            button12 = new Button();
            textBox1 = new TextBox();
            button13 = new Button();
            cbResetInitialTemps = new CheckBox();
            CB_SplitMainFeed = new CheckBox();
            label7 = new Label();
            txtOuterTolerance = new TextBox();
            label8 = new Label();
            txtInnerTolerance = new TextBox();
            label6 = new Label();
            txtMaxInnerIterations = new TextBox();
            label4 = new Label();
            txtMaxOuterIterations = new TextBox();
            label3 = new Label();
            DampFactortxt = new TextBox();
            label2 = new Label();
            label1 = new Label();
            Error2 = new RichTextBox();
            Error1 = new RichTextBox();
            TabPageEstimates = new TabPage();
            DGVEfficiencies = new FormControls.UserPropGrid();
            DGVPressures = new FormControls.UserPropGrid();
            button8 = new Button();
            button7 = new Button();
            btnEstimateTs = new Button();
            TabPageSpecifications = new TabPage();
            specificationSheet1 = new SpecificationSheet();
            TabPageColumnDesigner = new TabPage();
            label10 = new Label();
            label9 = new Label();
            Feeds = new Label();
            dGVPAConnections = new DataGridView();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewComboBoxColumn3 = new DataGridViewComboBoxColumn();
            dGVProductsConnections = new DataGridView();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewComboBoxColumn1 = new DataGridViewComboBoxColumn();
            dGVFeedsConnections = new DataGridView();
            ExternalName = new DataGridViewTextBoxColumn();
            publicName = new DataGridViewComboBoxColumn();
            TabPageDesigner = new TabPage();
            columnDesignerControl1 = new ColumnDesignerControl();
            tabPageStageProps = new TabPage();
            groupBox2 = new GroupBox();
            RBVapCompoisition = new RadioButton();
            RBLiqComposition = new RadioButton();
            RBPressures = new RadioButton();
            RBTemperatures = new RadioButton();
            RBSurfaceTension = new RadioButton();
            RBThermalCond = new RadioButton();
            RBViscosity = new RadioButton();
            RBKValues = new RadioButton();
            RBEnthalpy = new RadioButton();
            RBFlows = new RadioButton();
            dataGridView1 = new DataGridView();
            TabPageTrayCompositions = new TabPage();
            radioButtonVapour = new RadioButton();
            radioButtonLiquid = new RadioButton();
            label5 = new Label();
            dataGridView2 = new DataGridView();
            TabPageinternalStreams = new TabPage();
            internalWorksheet = new PortsPropertyWorksheet();
            TabPageExternalStreams = new TabPage();
            worksheet = new PortsPropertyWorksheet();
            tabProfiles = new TabPage();
            label11 = new Label();
            label16 = new Label();
            DiagnosticDataGridView = new DataGridView();
            ComponentProfileDG = new DataGridView();
            tabStreamConnections = new TabPage();
            dataGridViewProductConnectiontray = new DataGridView();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewComboBoxColumn4 = new DataGridViewComboBoxColumn();
            dataGridViewFeedConnectionTray = new DataGridView();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewComboBoxColumn5 = new DataGridViewComboBoxColumn();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewComboBoxColumn2 = new DataGridViewComboBoxColumn();
            Tabs.SuspendLayout();
            TabePageSolver.SuspendLayout();
            TabPageEstimates.SuspendLayout();
            TabPageSpecifications.SuspendLayout();
            TabPageColumnDesigner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dGVPAConnections).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dGVProductsConnections).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dGVFeedsConnections).BeginInit();
            TabPageDesigner.SuspendLayout();
            tabPageStageProps.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            TabPageTrayCompositions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            TabPageinternalStreams.SuspendLayout();
            TabPageExternalStreams.SuspendLayout();
            tabProfiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DiagnosticDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ComponentProfileDG).BeginInit();
            tabStreamConnections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProductConnectiontray).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFeedConnectionTray).BeginInit();
            SuspendLayout();
            // 
            // Tabs
            // 
            Tabs.Controls.Add(TabePageSolver);
            Tabs.Controls.Add(TabPageEstimates);
            Tabs.Controls.Add(TabPageSpecifications);
            Tabs.Controls.Add(TabPageColumnDesigner);
            Tabs.Controls.Add(TabPageDesigner);
            Tabs.Controls.Add(tabPageStageProps);
            Tabs.Controls.Add(TabPageTrayCompositions);
            Tabs.Controls.Add(TabPageinternalStreams);
            Tabs.Controls.Add(TabPageExternalStreams);
            Tabs.Controls.Add(tabProfiles);
            Tabs.Controls.Add(tabStreamConnections);
            Tabs.Dock = DockStyle.Fill;
            Tabs.Location = new System.Drawing.Point(0, 0);
            Tabs.Margin = new Padding(2);
            Tabs.Multiline = true;
            Tabs.Name = "Tabs";
            Tabs.SelectedIndex = 0;
            Tabs.Size = new System.Drawing.Size(915, 534);
            Tabs.TabIndex = 0;
            Tabs.Selected += Tabs_Selected;
            // 
            // TabePageSolver
            // 
            TabePageSolver.BackColor = System.Drawing.SystemColors.Control;
            TabePageSolver.Controls.Add(label12);
            TabePageSolver.Controls.Add(ErrorConvergnce);
            TabePageSolver.Controls.Add(checkBoxActive);
            TabePageSolver.Controls.Add(button11);
            TabePageSolver.Controls.Add(button12);
            TabePageSolver.Controls.Add(textBox1);
            TabePageSolver.Controls.Add(button13);
            TabePageSolver.Controls.Add(cbResetInitialTemps);
            TabePageSolver.Controls.Add(CB_SplitMainFeed);
            TabePageSolver.Controls.Add(label7);
            TabePageSolver.Controls.Add(txtOuterTolerance);
            TabePageSolver.Controls.Add(label8);
            TabePageSolver.Controls.Add(txtInnerTolerance);
            TabePageSolver.Controls.Add(label6);
            TabePageSolver.Controls.Add(txtMaxInnerIterations);
            TabePageSolver.Controls.Add(label4);
            TabePageSolver.Controls.Add(txtMaxOuterIterations);
            TabePageSolver.Controls.Add(label3);
            TabePageSolver.Controls.Add(DampFactortxt);
            TabePageSolver.Controls.Add(label2);
            TabePageSolver.Controls.Add(label1);
            TabePageSolver.Controls.Add(Error2);
            TabePageSolver.Controls.Add(Error1);
            TabePageSolver.Location = new System.Drawing.Point(4, 44);
            TabePageSolver.Margin = new Padding(2);
            TabePageSolver.Name = "TabePageSolver";
            TabePageSolver.Padding = new Padding(2);
            TabePageSolver.Size = new System.Drawing.Size(907, 486);
            TabePageSolver.TabIndex = 0;
            TabePageSolver.Text = "Solver";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(34, 11);
            label12.Margin = new Padding(2, 0, 2, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(138, 15);
            label12.TabIndex = 30;
            label12.Text = "InitialisatonConvergence";
            // 
            // ErrorConvergnce
            // 
            ErrorConvergnce.Location = new System.Drawing.Point(38, 28);
            ErrorConvergnce.Margin = new Padding(2);
            ErrorConvergnce.Name = "ErrorConvergnce";
            ErrorConvergnce.Size = new System.Drawing.Size(199, 401);
            ErrorConvergnce.TabIndex = 29;
            ErrorConvergnce.Text = "";
            // 
            // checkBoxActive
            // 
            checkBoxActive.AutoSize = true;
            checkBoxActive.Location = new System.Drawing.Point(817, 472);
            checkBoxActive.Name = "checkBoxActive";
            checkBoxActive.Size = new System.Drawing.Size(59, 19);
            checkBoxActive.TabIndex = 28;
            checkBoxActive.Text = "Active";
            checkBoxActive.UseVisualStyleBackColor = true;
            checkBoxActive.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button11
            // 
            button11.Location = new System.Drawing.Point(468, 456);
            button11.Margin = new Padding(2);
            button11.Name = "button11";
            button11.Size = new System.Drawing.Size(82, 35);
            button11.TabIndex = 26;
            button11.Text = "Step";
            button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            button12.Location = new System.Drawing.Point(554, 456);
            button12.Margin = new Padding(2);
            button12.Name = "button12";
            button12.Size = new System.Drawing.Size(82, 35);
            button12.TabIndex = 25;
            button12.Text = "Reset";
            button12.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(640, 468);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(109, 23);
            textBox1.TabIndex = 24;
            // 
            // button13
            // 
            button13.Location = new System.Drawing.Point(381, 456);
            button13.Margin = new Padding(2);
            button13.Name = "button13";
            button13.Size = new System.Drawing.Size(82, 35);
            button13.TabIndex = 23;
            button13.Text = "Calculate";
            button13.UseVisualStyleBackColor = true;
            button13.Click += ButtonCalculate_Click;
            // 
            // cbResetInitialTemps
            // 
            cbResetInitialTemps.AutoSize = true;
            cbResetInitialTemps.Location = new System.Drawing.Point(684, 30);
            cbResetInitialTemps.Margin = new Padding(2);
            cbResetInitialTemps.Name = "cbResetInitialTemps";
            cbResetInitialTemps.Size = new System.Drawing.Size(117, 19);
            cbResetInitialTemps.TabIndex = 19;
            cbResetInitialTemps.Text = "ResetInitialTemps";
            cbResetInitialTemps.UseVisualStyleBackColor = true;
            cbResetInitialTemps.CheckedChanged += CB_ResetInitialTemps_CheckedChanged;
            // 
            // CB_SplitMainFeed
            // 
            CB_SplitMainFeed.AutoSize = true;
            CB_SplitMainFeed.Checked = true;
            CB_SplitMainFeed.CheckState = CheckState.Checked;
            CB_SplitMainFeed.Location = new System.Drawing.Point(684, 54);
            CB_SplitMainFeed.Margin = new Padding(2);
            CB_SplitMainFeed.Name = "CB_SplitMainFeed";
            CB_SplitMainFeed.Size = new System.Drawing.Size(101, 19);
            CB_SplitMainFeed.TabIndex = 18;
            CB_SplitMainFeed.Text = "SplitMainFeed";
            CB_SplitMainFeed.UseVisualStyleBackColor = true;
            CB_SplitMainFeed.CheckedChanged += CB_SplitMainFeed_CheckedChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(685, 192);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(114, 15);
            label7.TabIndex = 17;
            label7.Text = "OuterLoopTolerance";
            // 
            // txtOuterTolerance
            // 
            txtOuterTolerance.Location = new System.Drawing.Point(832, 189);
            txtOuterTolerance.Margin = new Padding(2);
            txtOuterTolerance.Name = "txtOuterTolerance";
            txtOuterTolerance.Size = new System.Drawing.Size(44, 23);
            txtOuterTolerance.TabIndex = 16;
            txtOuterTolerance.Text = "0.001";
            txtOuterTolerance.TextChanged += TxtOuterTolerance_TextChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(685, 166);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(111, 15);
            label8.TabIndex = 15;
            label8.Text = "InnerLoopTolerance";
            // 
            // txtInnerTolerance
            // 
            txtInnerTolerance.Location = new System.Drawing.Point(832, 163);
            txtInnerTolerance.Margin = new Padding(2);
            txtInnerTolerance.Name = "txtInnerTolerance";
            txtInnerTolerance.Size = new System.Drawing.Size(44, 23);
            txtInnerTolerance.TabIndex = 14;
            txtInnerTolerance.Text = "0.001";
            txtInnerTolerance.TextChanged += TxtInnerTolerance_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(685, 139);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(106, 15);
            label6.TabIndex = 12;
            label6.Text = "MaxInnerIterations";
            // 
            // txtMaxInnerIterations
            // 
            txtMaxInnerIterations.Location = new System.Drawing.Point(832, 135);
            txtMaxInnerIterations.Margin = new Padding(2);
            txtMaxInnerIterations.Name = "txtMaxInnerIterations";
            txtMaxInnerIterations.Size = new System.Drawing.Size(44, 23);
            txtMaxInnerIterations.TabIndex = 11;
            txtMaxInnerIterations.Text = "50";
            txtMaxInnerIterations.TextChanged += TxtMaxInnerIterations_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(685, 111);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(109, 15);
            label4.TabIndex = 10;
            label4.Text = "MaxOuterIterations";
            // 
            // txtMaxOuterIterations
            // 
            txtMaxOuterIterations.Location = new System.Drawing.Point(832, 109);
            txtMaxOuterIterations.Margin = new Padding(2);
            txtMaxOuterIterations.Name = "txtMaxOuterIterations";
            txtMaxOuterIterations.Size = new System.Drawing.Size(44, 23);
            txtMaxOuterIterations.TabIndex = 9;
            txtMaxOuterIterations.Text = "50";
            txtMaxOuterIterations.TextChanged += TxtMaxOuterIterations_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(685, 85);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(89, 15);
            label3.TabIndex = 8;
            label3.Text = "DampingFactor";
            // 
            // DampFactortxt
            // 
            DampFactortxt.Location = new System.Drawing.Point(832, 80);
            DampFactortxt.Margin = new Padding(2);
            DampFactortxt.Name = "DampFactortxt";
            DampFactortxt.Size = new System.Drawing.Size(44, 23);
            DampFactortxt.TabIndex = 7;
            DampFactortxt.Text = "0.7";
            DampFactortxt.TextChanged += DampFactortxt_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(446, 11);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(89, 15);
            label2.TabIndex = 5;
            label2.Text = "OuterLoopError";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(241, 11);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(86, 15);
            label1.TabIndex = 4;
            label1.Text = "InnerLoopError";
            // 
            // Error2
            // 
            Error2.Location = new System.Drawing.Point(449, 28);
            Error2.Margin = new Padding(2);
            Error2.Name = "Error2";
            Error2.Size = new System.Drawing.Size(201, 401);
            Error2.TabIndex = 2;
            Error2.Text = "";
            // 
            // Error1
            // 
            Error1.Location = new System.Drawing.Point(245, 28);
            Error1.Margin = new Padding(2);
            Error1.Name = "Error1";
            Error1.Size = new System.Drawing.Size(199, 401);
            Error1.TabIndex = 0;
            Error1.Text = "";
            // 
            // TabPageEstimates
            // 
            TabPageEstimates.BackColor = System.Drawing.SystemColors.Control;
            TabPageEstimates.Controls.Add(DGVEfficiencies);
            TabPageEstimates.Controls.Add(DGVPressures);
            TabPageEstimates.Controls.Add(button8);
            TabPageEstimates.Controls.Add(button7);
            TabPageEstimates.Controls.Add(btnEstimateTs);
            TabPageEstimates.Location = new System.Drawing.Point(4, 44);
            TabPageEstimates.Margin = new Padding(2);
            TabPageEstimates.Name = "TabPageEstimates";
            TabPageEstimates.Padding = new Padding(2);
            TabPageEstimates.Size = new System.Drawing.Size(907, 486);
            TabPageEstimates.TabIndex = 3;
            TabPageEstimates.Text = "Estimates";
            // 
            // DGVEfficiencies
            // 
            DGVEfficiencies.AllowChangeEvent = true;
            DGVEfficiencies.AllowUserToAddRows = false;
            DGVEfficiencies.AllowUserToDeleteRows = false;
            DGVEfficiencies.ColumnNames = null;
            DGVEfficiencies.DisplayTitles = true;
            DGVEfficiencies.FirstColumnWidth = 64;
            DGVEfficiencies.Location = new System.Drawing.Point(444, 11);
            DGVEfficiencies.Margin = new Padding(4, 3, 4, 3);
            DGVEfficiencies.Name = "DGVEfficiencies";
            DGVEfficiencies.ReadOnly = false;
            DGVEfficiencies.RowHeadersVisible = false;
            DGVEfficiencies.RowNames = null;
            DGVEfficiencies.Size = new System.Drawing.Size(430, 467);
            DGVEfficiencies.TabIndex = 16;
            DGVEfficiencies.TopText = "TrayEfficiencies";
            // 
            // DGVPressures
            // 
            DGVPressures.AllowChangeEvent = true;
            DGVPressures.AllowUserToAddRows = false;
            DGVPressures.AllowUserToDeleteRows = false;
            DGVPressures.ColumnNames = null;
            DGVPressures.DisplayTitles = true;
            DGVPressures.FirstColumnWidth = 64;
            DGVPressures.Location = new System.Drawing.Point(6, 11);
            DGVPressures.Margin = new Padding(4, 3, 4, 3);
            DGVPressures.Name = "DGVPressures";
            DGVPressures.ReadOnly = false;
            DGVPressures.RowHeadersVisible = false;
            DGVPressures.RowNames = null;
            DGVPressures.Size = new System.Drawing.Size(430, 467);
            DGVPressures.TabIndex = 15;
            DGVPressures.TopText = "ColumnPressure s";
            DGVPressures.ValueChanged += DgvEstimates_ValueChanged;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(136, 541);
            button8.Margin = new Padding(2);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(103, 32);
            button8.TabIndex = 12;
            button8.Text = "ClearEstimates";
            button8.UseVisualStyleBackColor = true;
            button8.Click += BtnResetEstimates_Click;
            // 
            // button7
            // 
            button7.Location = new System.Drawing.Point(243, 541);
            button7.Margin = new Padding(2);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(164, 32);
            button7.TabIndex = 11;
            button7.Text = "LoadTFromSolution";
            button7.UseVisualStyleBackColor = true;
            button7.Click += Button7_Click;
            // 
            // btnEstimateTs
            // 
            btnEstimateTs.Location = new System.Drawing.Point(29, 541);
            btnEstimateTs.Margin = new Padding(2);
            btnEstimateTs.Name = "btnEstimateTs";
            btnEstimateTs.Size = new System.Drawing.Size(103, 32);
            btnEstimateTs.TabIndex = 10;
            btnEstimateTs.Text = "EstimateT";
            btnEstimateTs.UseVisualStyleBackColor = true;
            btnEstimateTs.Click += BtnEstT_Click;
            // 
            // TabPageSpecifications
            // 
            TabPageSpecifications.BackColor = System.Drawing.SystemColors.Control;
            TabPageSpecifications.Controls.Add(specificationSheet1);
            TabPageSpecifications.Location = new System.Drawing.Point(4, 44);
            TabPageSpecifications.Margin = new Padding(2);
            TabPageSpecifications.Name = "TabPageSpecifications";
            TabPageSpecifications.Padding = new Padding(2);
            TabPageSpecifications.Size = new System.Drawing.Size(907, 486);
            TabPageSpecifications.TabIndex = 6;
            TabPageSpecifications.Text = "Specifications";
            // 
            // specificationSheet1
            // 
            specificationSheet1.Dock = DockStyle.Fill;
            specificationSheet1.Location = new System.Drawing.Point(2, 2);
            specificationSheet1.Margin = new Padding(2);
            specificationSheet1.Name = "specificationSheet1";
            specificationSheet1.Size = new System.Drawing.Size(903, 482);
            specificationSheet1.TabIndex = 0;
            // 
            // TabPageColumnDesigner
            // 
            TabPageColumnDesigner.BackColor = System.Drawing.SystemColors.Control;
            TabPageColumnDesigner.Controls.Add(label10);
            TabPageColumnDesigner.Controls.Add(label9);
            TabPageColumnDesigner.Controls.Add(Feeds);
            TabPageColumnDesigner.Controls.Add(dGVPAConnections);
            TabPageColumnDesigner.Controls.Add(dGVProductsConnections);
            TabPageColumnDesigner.Controls.Add(dGVFeedsConnections);
            TabPageColumnDesigner.Location = new System.Drawing.Point(4, 44);
            TabPageColumnDesigner.Margin = new Padding(2);
            TabPageColumnDesigner.Name = "TabPageColumnDesigner";
            TabPageColumnDesigner.Padding = new Padding(2);
            TabPageColumnDesigner.Size = new System.Drawing.Size(907, 486);
            TabPageColumnDesigner.TabIndex = 1;
            TabPageColumnDesigner.Text = "ColumnConfiguration";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(597, 31);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(96, 15);
            label10.TabIndex = 32;
            label10.Text = "ExportedStreams";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(317, 31);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(54, 15);
            label9.TabIndex = 31;
            label9.Text = "Products";
            // 
            // Feeds
            // 
            Feeds.AutoSize = true;
            Feeds.Location = new System.Drawing.Point(31, 31);
            Feeds.Name = "Feeds";
            Feeds.Size = new System.Drawing.Size(37, 15);
            Feeds.TabIndex = 30;
            Feeds.Text = "Feeds";
            // 
            // dGVPAConnections
            // 
            dGVPAConnections.AllowUserToAddRows = false;
            dGVPAConnections.AllowUserToDeleteRows = false;
            dGVPAConnections.AllowUserToResizeRows = false;
            dGVPAConnections.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dGVPAConnections.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGVPAConnections.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn6, dataGridViewComboBoxColumn3 });
            dGVPAConnections.Location = new System.Drawing.Point(597, 49);
            dGVPAConnections.Margin = new Padding(4, 3, 4, 3);
            dGVPAConnections.Name = "dGVPAConnections";
            dGVPAConnections.RowHeadersVisible = false;
            dGVPAConnections.Size = new System.Drawing.Size(250, 439);
            dGVPAConnections.TabIndex = 29;
            dGVPAConnections.CellContentClick += dGVPAConnections_CellContentClick;
            dGVPAConnections.EditingControlShowing += dGVPAConnections_EditingControlShowing;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "ExternalStream";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewComboBoxColumn3
            // 
            dataGridViewComboBoxColumn3.HeaderText = "internal Stream";
            dataGridViewComboBoxColumn3.Name = "dataGridViewComboBoxColumn3";
            // 
            // dGVProductsConnections
            // 
            dGVProductsConnections.AllowUserToAddRows = false;
            dGVProductsConnections.AllowUserToDeleteRows = false;
            dGVProductsConnections.AllowUserToResizeRows = false;
            dGVProductsConnections.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dGVProductsConnections.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGVProductsConnections.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn2, dataGridViewComboBoxColumn1 });
            dGVProductsConnections.Location = new System.Drawing.Point(317, 49);
            dGVProductsConnections.Margin = new Padding(4, 3, 4, 3);
            dGVProductsConnections.Name = "dGVProductsConnections";
            dGVProductsConnections.RowHeadersVisible = false;
            dGVProductsConnections.Size = new System.Drawing.Size(250, 439);
            dGVProductsConnections.TabIndex = 28;
            dGVProductsConnections.EditingControlShowing += DataGridViewProductConnections_EditingControlShowing;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "ExternalStream";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewComboBoxColumn1
            // 
            dataGridViewComboBoxColumn1.HeaderText = "internal Stream";
            dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            // 
            // dGVFeedsConnections
            // 
            dGVFeedsConnections.AllowUserToAddRows = false;
            dGVFeedsConnections.AllowUserToDeleteRows = false;
            dGVFeedsConnections.AllowUserToResizeRows = false;
            dGVFeedsConnections.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dGVFeedsConnections.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGVFeedsConnections.Columns.AddRange(new DataGridViewColumn[] { ExternalName, publicName });
            dGVFeedsConnections.Location = new System.Drawing.Point(31, 49);
            dGVFeedsConnections.Margin = new Padding(4, 3, 4, 3);
            dGVFeedsConnections.Name = "dGVFeedsConnections";
            dGVFeedsConnections.RowHeadersVisible = false;
            dGVFeedsConnections.Size = new System.Drawing.Size(250, 439);
            dGVFeedsConnections.TabIndex = 27;
            dGVFeedsConnections.EditingControlShowing += DataGridViewFeedConnections_EditingControlShowing;
            // 
            // ExternalName
            // 
            ExternalName.HeaderText = "ExternalStream";
            ExternalName.Name = "ExternalName";
            ExternalName.Resizable = DataGridViewTriState.True;
            ExternalName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // publicName
            // 
            publicName.HeaderText = "internal Stream";
            publicName.Name = "publicName";
            // 
            // TabPageDesigner
            // 
            TabPageDesigner.Controls.Add(columnDesignerControl1);
            TabPageDesigner.Location = new System.Drawing.Point(4, 44);
            TabPageDesigner.Margin = new Padding(4, 3, 4, 3);
            TabPageDesigner.Name = "TabPageDesigner";
            TabPageDesigner.Padding = new Padding(4, 3, 4, 3);
            TabPageDesigner.Size = new System.Drawing.Size(907, 486);
            TabPageDesigner.TabIndex = 9;
            TabPageDesigner.Text = "VisualDesigner";
            TabPageDesigner.UseVisualStyleBackColor = true;
            // 
            // columnDesignerControl1
            // 
            columnDesignerControl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            columnDesignerControl1.BorderStyle = BorderStyle.FixedSingle;
            columnDesignerControl1.Dock = DockStyle.Fill;
            columnDesignerControl1.GraphicsList = null;
            columnDesignerControl1.Location = new System.Drawing.Point(4, 3);
            columnDesignerControl1.Name = "columnDesignerControl1";
            columnDesignerControl1.Size = new System.Drawing.Size(899, 480);
            columnDesignerControl1.TabIndex = 0;
            // 
            // tabPageStageProps
            // 
            tabPageStageProps.BackColor = System.Drawing.SystemColors.Control;
            tabPageStageProps.Controls.Add(groupBox2);
            tabPageStageProps.Controls.Add(dataGridView1);
            tabPageStageProps.Location = new System.Drawing.Point(4, 44);
            tabPageStageProps.Margin = new Padding(4, 3, 4, 3);
            tabPageStageProps.Name = "tabPageStageProps";
            tabPageStageProps.Padding = new Padding(4, 3, 4, 3);
            tabPageStageProps.Size = new System.Drawing.Size(907, 486);
            tabPageStageProps.TabIndex = 11;
            tabPageStageProps.Text = "StageProperties";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(RBVapCompoisition);
            groupBox2.Controls.Add(RBLiqComposition);
            groupBox2.Controls.Add(RBPressures);
            groupBox2.Controls.Add(RBTemperatures);
            groupBox2.Controls.Add(RBSurfaceTension);
            groupBox2.Controls.Add(RBThermalCond);
            groupBox2.Controls.Add(RBViscosity);
            groupBox2.Controls.Add(RBKValues);
            groupBox2.Controls.Add(RBEnthalpy);
            groupBox2.Controls.Add(RBFlows);
            groupBox2.Location = new System.Drawing.Point(9, 7);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(209, 489);
            groupBox2.TabIndex = 15;
            groupBox2.TabStop = false;
            groupBox2.Text = "Property";
            // 
            // RBVapCompoisition
            // 
            RBVapCompoisition.AutoSize = true;
            RBVapCompoisition.Location = new System.Drawing.Point(20, 143);
            RBVapCompoisition.Margin = new Padding(4, 3, 4, 3);
            RBVapCompoisition.Name = "RBVapCompoisition";
            RBVapCompoisition.Size = new System.Drawing.Size(131, 19);
            RBVapCompoisition.TabIndex = 9;
            RBVapCompoisition.TabStop = true;
            RBVapCompoisition.Text = "CompositionVapour";
            RBVapCompoisition.UseVisualStyleBackColor = true;
            // 
            // RBLiqComposition
            // 
            RBLiqComposition.AutoSize = true;
            RBLiqComposition.Location = new System.Drawing.Point(20, 118);
            RBLiqComposition.Margin = new Padding(4, 3, 4, 3);
            RBLiqComposition.Name = "RBLiqComposition";
            RBLiqComposition.Size = new System.Drawing.Size(127, 19);
            RBLiqComposition.TabIndex = 8;
            RBLiqComposition.TabStop = true;
            RBLiqComposition.Text = "CompositionLiquid";
            RBLiqComposition.UseVisualStyleBackColor = true;
            // 
            // RBPressures
            // 
            RBPressures.AutoSize = true;
            RBPressures.Location = new System.Drawing.Point(20, 65);
            RBPressures.Margin = new Padding(4, 3, 4, 3);
            RBPressures.Name = "RBPressures";
            RBPressures.Size = new System.Drawing.Size(69, 19);
            RBPressures.TabIndex = 7;
            RBPressures.TabStop = true;
            RBPressures.Text = "Pressure";
            RBPressures.UseVisualStyleBackColor = true;
            // 
            // RBTemperatures
            // 
            RBTemperatures.AutoSize = true;
            RBTemperatures.Location = new System.Drawing.Point(20, 38);
            RBTemperatures.Margin = new Padding(4, 3, 4, 3);
            RBTemperatures.Name = "RBTemperatures";
            RBTemperatures.Size = new System.Drawing.Size(91, 19);
            RBTemperatures.TabIndex = 6;
            RBTemperatures.TabStop = true;
            RBTemperatures.Text = "Temperature";
            RBTemperatures.UseVisualStyleBackColor = true;
            RBTemperatures.CheckedChanged += RB_Temps_CheckedChanged;
            // 
            // RBSurfaceTension
            // 
            RBSurfaceTension.AutoSize = true;
            RBSurfaceTension.Location = new System.Drawing.Point(20, 276);
            RBSurfaceTension.Margin = new Padding(4, 3, 4, 3);
            RBSurfaceTension.Name = "RBSurfaceTension";
            RBSurfaceTension.Size = new System.Drawing.Size(104, 19);
            RBSurfaceTension.TabIndex = 5;
            RBSurfaceTension.TabStop = true;
            RBSurfaceTension.Text = "SurfaceTension";
            RBSurfaceTension.UseVisualStyleBackColor = true;
            // 
            // RBThermalCond
            // 
            RBThermalCond.AutoSize = true;
            RBThermalCond.Location = new System.Drawing.Point(20, 249);
            RBThermalCond.Margin = new Padding(4, 3, 4, 3);
            RBThermalCond.Name = "RBThermalCond";
            RBThermalCond.Size = new System.Drawing.Size(136, 19);
            RBThermalCond.TabIndex = 4;
            RBThermalCond.TabStop = true;
            RBThermalCond.Text = "ThermalConductivity";
            RBThermalCond.UseVisualStyleBackColor = true;
            // 
            // RBViscosity
            // 
            RBViscosity.AutoSize = true;
            RBViscosity.Location = new System.Drawing.Point(20, 223);
            RBViscosity.Margin = new Padding(4, 3, 4, 3);
            RBViscosity.Name = "RBViscosity";
            RBViscosity.Size = new System.Drawing.Size(71, 19);
            RBViscosity.TabIndex = 3;
            RBViscosity.TabStop = true;
            RBViscosity.Text = "Viscosity";
            RBViscosity.UseVisualStyleBackColor = true;
            // 
            // RBKValues
            // 
            RBKValues.AutoSize = true;
            RBKValues.Location = new System.Drawing.Point(20, 196);
            RBKValues.Margin = new Padding(4, 3, 4, 3);
            RBKValues.Name = "RBKValues";
            RBKValues.Size = new System.Drawing.Size(65, 19);
            RBKValues.TabIndex = 2;
            RBKValues.TabStop = true;
            RBKValues.Text = "KValues";
            RBKValues.UseVisualStyleBackColor = true;
            // 
            // RBEnthalpy
            // 
            RBEnthalpy.AutoSize = true;
            RBEnthalpy.Location = new System.Drawing.Point(20, 170);
            RBEnthalpy.Margin = new Padding(4, 3, 4, 3);
            RBEnthalpy.Name = "RBEnthalpy";
            RBEnthalpy.Size = new System.Drawing.Size(71, 19);
            RBEnthalpy.TabIndex = 1;
            RBEnthalpy.TabStop = true;
            RBEnthalpy.Text = "Enthalpy";
            RBEnthalpy.UseVisualStyleBackColor = true;
            // 
            // RBFlows
            // 
            RBFlows.AutoSize = true;
            RBFlows.Location = new System.Drawing.Point(20, 91);
            RBFlows.Margin = new Padding(4, 3, 4, 3);
            RBFlows.Name = "RBFlows";
            RBFlows.Size = new System.Drawing.Size(55, 19);
            RBFlows.TabIndex = 0;
            RBFlows.TabStop = true;
            RBFlows.Text = "Flows";
            RBFlows.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnF2;
            dataGridView1.Location = new System.Drawing.Point(224, 16);
            dataGridView1.Margin = new Padding(2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.RowTemplate.Height = 24;
            dataGridView1.Size = new System.Drawing.Size(673, 481);
            dataGridView1.TabIndex = 14;
            // 
            // TabPageTrayCompositions
            // 
            TabPageTrayCompositions.Controls.Add(radioButtonVapour);
            TabPageTrayCompositions.Controls.Add(radioButtonLiquid);
            TabPageTrayCompositions.Controls.Add(label5);
            TabPageTrayCompositions.Controls.Add(dataGridView2);
            TabPageTrayCompositions.Location = new System.Drawing.Point(4, 44);
            TabPageTrayCompositions.Name = "TabPageTrayCompositions";
            TabPageTrayCompositions.Size = new System.Drawing.Size(907, 486);
            TabPageTrayCompositions.TabIndex = 12;
            TabPageTrayCompositions.Text = "TrayCompositions";
            TabPageTrayCompositions.UseVisualStyleBackColor = true;
            // 
            // radioButtonVapour
            // 
            radioButtonVapour.AutoSize = true;
            radioButtonVapour.Location = new System.Drawing.Point(9, 64);
            radioButtonVapour.Margin = new Padding(4, 3, 4, 3);
            radioButtonVapour.Name = "radioButtonVapour";
            radioButtonVapour.Size = new System.Drawing.Size(131, 19);
            radioButtonVapour.TabIndex = 18;
            radioButtonVapour.TabStop = true;
            radioButtonVapour.Text = "CompositionVapour";
            radioButtonVapour.UseVisualStyleBackColor = true;
            radioButtonVapour.CheckedChanged += radioButtonVapour_CheckedChanged;
            // 
            // radioButtonLiquid
            // 
            radioButtonLiquid.AutoSize = true;
            radioButtonLiquid.Location = new System.Drawing.Point(9, 39);
            radioButtonLiquid.Margin = new Padding(4, 3, 4, 3);
            radioButtonLiquid.Name = "radioButtonLiquid";
            radioButtonLiquid.Size = new System.Drawing.Size(127, 19);
            radioButtonLiquid.TabIndex = 17;
            radioButtonLiquid.TabStop = true;
            radioButtonLiquid.Text = "CompositionLiquid";
            radioButtonLiquid.UseVisualStyleBackColor = true;
            radioButtonLiquid.CheckedChanged += radioButtonLiquid_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(8, 9);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(113, 15);
            label5.TabIndex = 16;
            label5.Text = "ColumnInformation";
            // 
            // dataGridView2
            // 
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView2.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridView2.BorderStyle = BorderStyle.Fixed3D;
            dataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.EditMode = DataGridViewEditMode.EditOnF2;
            dataGridView2.Location = new System.Drawing.Point(8, 111);
            dataGridView2.Margin = new Padding(2);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView2.RowTemplate.Height = 24;
            dataGridView2.Size = new System.Drawing.Size(889, 379);
            dataGridView2.TabIndex = 15;
            // 
            // TabPageinternalStreams
            // 
            TabPageinternalStreams.Controls.Add(internalWorksheet);
            TabPageinternalStreams.Location = new System.Drawing.Point(4, 44);
            TabPageinternalStreams.Name = "TabPageinternalStreams";
            TabPageinternalStreams.Size = new System.Drawing.Size(907, 486);
            TabPageinternalStreams.TabIndex = 13;
            TabPageinternalStreams.Text = "internal Streams";
            TabPageinternalStreams.UseVisualStyleBackColor = true;
            // 
            // internalWorksheet
            // 
            internalWorksheet.Dock = DockStyle.Fill;
            internalWorksheet.Location = new System.Drawing.Point(0, 0);
            internalWorksheet.Margin = new Padding(2);
            internalWorksheet.Name = "internalWorksheet";
            internalWorksheet.Size = new System.Drawing.Size(907, 486);
            internalWorksheet.TabIndex = 1;
            // 
            // TabPageExternalStreams
            // 
            TabPageExternalStreams.Controls.Add(worksheet);
            TabPageExternalStreams.Location = new System.Drawing.Point(4, 44);
            TabPageExternalStreams.Margin = new Padding(5, 3, 5, 3);
            TabPageExternalStreams.Name = "TabPageExternalStreams";
            TabPageExternalStreams.Padding = new Padding(5, 3, 5, 3);
            TabPageExternalStreams.Size = new System.Drawing.Size(907, 486);
            TabPageExternalStreams.TabIndex = 10;
            TabPageExternalStreams.Text = "ExternalStreams";
            TabPageExternalStreams.UseVisualStyleBackColor = true;
            // 
            // worksheet
            // 
            worksheet.Dock = DockStyle.Fill;
            worksheet.Location = new System.Drawing.Point(5, 3);
            worksheet.Margin = new Padding(2);
            worksheet.Name = "worksheet";
            worksheet.Size = new System.Drawing.Size(897, 480);
            worksheet.TabIndex = 0;
            // 
            // tabProfiles
            // 
            tabProfiles.Controls.Add(label11);
            tabProfiles.Controls.Add(label16);
            tabProfiles.Controls.Add(DiagnosticDataGridView);
            tabProfiles.Controls.Add(ComponentProfileDG);
            tabProfiles.Location = new System.Drawing.Point(4, 44);
            tabProfiles.Name = "tabProfiles";
            tabProfiles.Padding = new Padding(3);
            tabProfiles.Size = new System.Drawing.Size(907, 486);
            tabProfiles.TabIndex = 14;
            tabProfiles.Text = "Profiles";
            tabProfiles.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(7, 15);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(46, 15);
            label11.TabIndex = 26;
            label11.Text = "Profiles";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(365, 15);
            label16.Margin = new Padding(2, 0, 2, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(113, 15);
            label16.TabIndex = 25;
            label16.Text = "ColumnInformation";
            // 
            // DiagnosticDataGridView
            // 
            DiagnosticDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            DiagnosticDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            DiagnosticDataGridView.BorderStyle = BorderStyle.Fixed3D;
            DiagnosticDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            DiagnosticDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DiagnosticDataGridView.EditMode = DataGridViewEditMode.EditOnF2;
            DiagnosticDataGridView.Location = new System.Drawing.Point(365, 32);
            DiagnosticDataGridView.Margin = new Padding(2);
            DiagnosticDataGridView.Name = "DiagnosticDataGridView";
            DiagnosticDataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            DiagnosticDataGridView.RowHeadersVisible = false;
            DiagnosticDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DiagnosticDataGridView.RowTemplate.Height = 24;
            DiagnosticDataGridView.Size = new System.Drawing.Size(543, 464);
            DiagnosticDataGridView.TabIndex = 24;
            // 
            // ComponentProfileDG
            // 
            ComponentProfileDG.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            ComponentProfileDG.BackgroundColor = System.Drawing.SystemColors.Window;
            ComponentProfileDG.BorderStyle = BorderStyle.Fixed3D;
            ComponentProfileDG.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            ComponentProfileDG.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ComponentProfileDG.EditMode = DataGridViewEditMode.EditOnF2;
            ComponentProfileDG.Location = new System.Drawing.Point(7, 32);
            ComponentProfileDG.Margin = new Padding(2);
            ComponentProfileDG.Name = "ComponentProfileDG";
            ComponentProfileDG.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            ComponentProfileDG.RowHeadersVisible = false;
            ComponentProfileDG.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            ComponentProfileDG.RowTemplate.Height = 24;
            ComponentProfileDG.Size = new System.Drawing.Size(357, 464);
            ComponentProfileDG.TabIndex = 23;
            // 
            // tabStreamConnections
            // 
            tabStreamConnections.Controls.Add(dataGridViewProductConnectiontray);
            tabStreamConnections.Controls.Add(dataGridViewFeedConnectionTray);
            tabStreamConnections.Location = new System.Drawing.Point(4, 44);
            tabStreamConnections.Name = "tabStreamConnections";
            tabStreamConnections.Padding = new Padding(3);
            tabStreamConnections.Size = new System.Drawing.Size(907, 486);
            tabStreamConnections.TabIndex = 15;
            tabStreamConnections.Text = "Stream Connections";
            tabStreamConnections.UseVisualStyleBackColor = true;
            // 
            // dataGridViewProductConnectiontray
            // 
            dataGridViewProductConnectiontray.AllowUserToAddRows = false;
            dataGridViewProductConnectiontray.AllowUserToDeleteRows = false;
            dataGridViewProductConnectiontray.AllowUserToResizeRows = false;
            dataGridViewProductConnectiontray.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProductConnectiontray.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewProductConnectiontray.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn7, dataGridViewComboBoxColumn4 });
            dataGridViewProductConnectiontray.Location = new System.Drawing.Point(341, 17);
            dataGridViewProductConnectiontray.Margin = new Padding(4, 3, 4, 3);
            dataGridViewProductConnectiontray.Name = "dataGridViewProductConnectiontray";
            dataGridViewProductConnectiontray.RowHeadersVisible = false;
            dataGridViewProductConnectiontray.Size = new System.Drawing.Size(250, 439);
            dataGridViewProductConnectiontray.TabIndex = 30;
            dataGridViewProductConnectiontray.EditingControlShowing += dataGridViewProductConnectiontray_EditingControlShowing;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "ExternalStream";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn7.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewComboBoxColumn4
            // 
            dataGridViewComboBoxColumn4.HeaderText = "internal Stream";
            dataGridViewComboBoxColumn4.Name = "dataGridViewComboBoxColumn4";
            // 
            // dataGridViewFeedConnectionTray
            // 
            dataGridViewFeedConnectionTray.AllowUserToAddRows = false;
            dataGridViewFeedConnectionTray.AllowUserToDeleteRows = false;
            dataGridViewFeedConnectionTray.AllowUserToResizeRows = false;
            dataGridViewFeedConnectionTray.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewFeedConnectionTray.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFeedConnectionTray.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn8, dataGridViewComboBoxColumn5 });
            dataGridViewFeedConnectionTray.Location = new System.Drawing.Point(55, 17);
            dataGridViewFeedConnectionTray.Margin = new Padding(4, 3, 4, 3);
            dataGridViewFeedConnectionTray.Name = "dataGridViewFeedConnectionTray";
            dataGridViewFeedConnectionTray.RowHeadersVisible = false;
            dataGridViewFeedConnectionTray.Size = new System.Drawing.Size(250, 439);
            dataGridViewFeedConnectionTray.TabIndex = 29;
            dataGridViewFeedConnectionTray.EditingControlShowing += DataGridViewProductConnections_EditingControlShowing;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "ExternalStream";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn8.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewComboBoxColumn5
            // 
            dataGridViewComboBoxColumn5.HeaderText = "internal Stream";
            dataGridViewComboBoxColumn5.Name = "dataGridViewComboBoxColumn5";
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "ExternalStream";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn1.Width = 69;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Pressure";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 87;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "ExternalStream";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn4.Width = 111;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "ExternalStream";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn5.Width = 106;
            // 
            // dataGridViewComboBoxColumn2
            // 
            dataGridViewComboBoxColumn2.HeaderText = "public Stream";
            dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            dataGridViewComboBoxColumn2.Width = 105;
            // 
            // ColumnDLG
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(915, 534);
            Controls.Add(Tabs);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            KeyPreview = true;
            Margin = new Padding(2);
            Name = "ColumnDLG";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ColumnDLG";
            Load += ColumnDLG_Load;
            KeyDown += ColumnDLG_KeyDown;
            Tabs.ResumeLayout(false);
            TabePageSolver.ResumeLayout(false);
            TabePageSolver.PerformLayout();
            TabPageEstimates.ResumeLayout(false);
            TabPageSpecifications.ResumeLayout(false);
            TabPageColumnDesigner.ResumeLayout(false);
            TabPageColumnDesigner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dGVPAConnections).EndInit();
            ((System.ComponentModel.ISupportInitialize)dGVProductsConnections).EndInit();
            ((System.ComponentModel.ISupportInitialize)dGVFeedsConnections).EndInit();
            TabPageDesigner.ResumeLayout(false);
            tabPageStageProps.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            TabPageTrayCompositions.ResumeLayout(false);
            TabPageTrayCompositions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            TabPageinternalStreams.ResumeLayout(false);
            TabPageExternalStreams.ResumeLayout(false);
            tabProfiles.ResumeLayout(false);
            tabProfiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DiagnosticDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)ComponentProfileDG).EndInit();
            tabStreamConnections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewProductConnectiontray).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFeedConnectionTray).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl Tabs;
        private TabPage TabPageColumnDesigner;
        private TabPage TabePageSolver;
        private Label label2;
        private Label label1;
        public RichTextBox Error2;
        public RichTextBox Error1;
        private Label label3;
        private TextBox DampFactortxt;
        private TabPage TabPageEstimates;
        private TabPage TabPageSpecifications;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private Button button7;
        private Button btnEstimateTs;
        private Button button8;
        private Label label6;
        private TextBox txtMaxInnerIterations;
        private Label label4;
        private TextBox txtMaxOuterIterations;
        private Label label7;
        private TextBox txtOuterTolerance;
        private Label label8;
        private TextBox txtInnerTolerance;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private CheckBox CB_SplitMainFeed;
        private CheckBox cbResetInitialTemps;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private TabPage TabPageDesigner;
        private Button button11;
        private Button button12;
        public TextBox textBox1;
        private Button button13;
        private DataGridView dGVProductsConnections;
        private DataGridView dGVFeedsConnections;
        private TabPage TabPageExternalStreams;
        private TabPage tabPageStageProps;
        private GroupBox groupBox2;
        private RadioButton RBPressures;
        private RadioButton RBTemperatures;
        private RadioButton RBSurfaceTension;
        private RadioButton RBThermalCond;
        private RadioButton RBViscosity;
        private RadioButton RBKValues;
        private RadioButton RBEnthalpy;
        private RadioButton RBFlows;
        private DataGridView dataGridView1;
        private RadioButton RBVapCompoisition;
        private RadioButton RBLiqComposition;
        private FormControls.UserPropGrid DGVPressures;
        private SpecificationSheet specificationSheet1;
        private PortsPropertyWorksheet worksheet;
        internal ColumnDesignerControl columnDesignerControl1;
        private CheckBox checkBoxActive;
        private FormControls.UserPropGrid DGVEfficiencies;
        private TabPage TabPageTrayCompositions;
        private Label label5;
        private DataGridView dataGridView2;
        private RadioButton radioButtonVapour;
        private RadioButton radioButtonLiquid;
        private TabPage TabPageinternalStreams;
        private PortsPropertyWorksheet internalWorksheet;
        private Label label10;
        private Label label9;
        private Label Feeds;
        private DataGridView dGVPAConnections;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private DataGridViewTextBoxColumn ExternalName;
        private DataGridViewComboBoxColumn publicName;
        private TabPage tabProfiles;
        private Label label11;
        private Label label16;
        private DataGridView DiagnosticDataGridView;
        private DataGridView ComponentProfileDG;
        private Label label12;
        public RichTextBox ErrorConvergnce;
        private TabPage tabStreamConnections;
        private DataGridView dataGridViewProductConnectiontray;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn4;
        private DataGridView dataGridViewFeedConnectionTray;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn5;
    }
}