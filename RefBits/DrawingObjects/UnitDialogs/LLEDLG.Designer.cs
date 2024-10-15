using   ModelEngine;
using   System.Windows.Forms;
using   Units.PortForm;

namespace   Units
{
    internal  partial class  LLEDLG
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void  Dispose(bool disposing)
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
        private  void  InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(LLEDLG));
            this.Tabs = new  System.Windows.Forms.TabControl();
            this.TabePageSolver = new  System.Windows.Forms.TabPage();
            this.checkBoxActive = new  System.Windows.Forms.CheckBox();
            this.label15 = new  System.Windows.Forms.Label();
            this.button11 = new  System.Windows.Forms.Button();
            this.button12 = new  System.Windows.Forms.Button();
            this.textBox1 = new  System.Windows.Forms.TextBox();
            this.button13 = new  System.Windows.Forms.Button();
            this.ComponentProfileDG = new  System.Windows.Forms.DataGridView();
            this.cbResetInitialTemps = new  System.Windows.Forms.CheckBox();
            this.CB_SplitMainFeed = new  System.Windows.Forms.CheckBox();
            this.label7 = new  System.Windows.Forms.Label();
            this.txtOuterTolerance = new  System.Windows.Forms.TextBox();
            this.label8 = new  System.Windows.Forms.Label();
            this.txtInnerTolerance = new  System.Windows.Forms.TextBox();
            this.label6 = new  System.Windows.Forms.Label();
            this.txtMaxInnerIterations = new  System.Windows.Forms.TextBox();
            this.label4 = new  System.Windows.Forms.Label();
            this.txtMaxOuterIterations = new  System.Windows.Forms.TextBox();
            this.label3 = new  System.Windows.Forms.Label();
            this.DampFactortxt = new  System.Windows.Forms.TextBox();
            this.label2 = new  System.Windows.Forms.Label();
            this.label1 = new  System.Windows.Forms.Label();
            this.Error2 = new  System.Windows.Forms.RichTextBox();
            this.Error1 = new  System.Windows.Forms.RichTextBox();
            this.TabPageEstimates = new  System.Windows.Forms.TabPage();
            this.DGVEfficiencies = new  FormControls.UserPropGrid();
            this.DGVPressures = new  FormControls.UserPropGrid();
            this.label16 = new  System.Windows.Forms.Label();
            this.DiagnosticDataGridView = new  System.Windows.Forms.DataGridView();
            this.button8 = new  System.Windows.Forms.Button();
            this.button7 = new  System.Windows.Forms.Button();
            this.btnEstimateTs = new  System.Windows.Forms.Button();
            this.TabPageSpecifications = new  System.Windows.Forms.TabPage();
            this.specificationSheet1 = new  Units.SpecificationSheet();
            this.TabPageColumnDesigner = new  System.Windows.Forms.TabPage();
            this.dGVProductsConnections = new  System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn1 = new  System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dGVFeedsConnections = new  System.Windows.Forms.DataGridView();
            this.ExternalName = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publicName = new  System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TabPageDesigner = new  System.Windows.Forms.TabPage();
            this.columnDesignerControl1 = new  Units.ColumnDesignerControl();
            this.TabPageStreams = new  System.Windows.Forms.TabPage();
            this.worksheet = new  Units.PortForm.PortsPropertyWorksheet();
            this.tabPageStageProps = new  System.Windows.Forms.TabPage();
            this.groupBox2 = new  System.Windows.Forms.GroupBox();
            this.radioButton10 = new  System.Windows.Forms.RadioButton();
            this.radioButton9 = new  System.Windows.Forms.RadioButton();
            this.radioButton8 = new  System.Windows.Forms.RadioButton();
            this.radioButton7 = new  System.Windows.Forms.RadioButton();
            this.radioButton6 = new  System.Windows.Forms.RadioButton();
            this.radioButton5 = new  System.Windows.Forms.RadioButton();
            this.radioButton4 = new  System.Windows.Forms.RadioButton();
            this.radioButton3 = new  System.Windows.Forms.RadioButton();
            this.radioButton2 = new  System.Windows.Forms.RadioButton();
            this.radioButton1 = new  System.Windows.Forms.RadioButton();
            this.dataGridView1 = new  System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new  System.ComponentModel.BackgroundWorker();
            this.dataGridViewTextBoxColumn1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn2 = new  System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Tabs.SuspendLayout();
            this.TabePageSolver.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComponentProfileDG)).BeginInit();
            this.TabPageEstimates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DiagnosticDataGridView)).BeginInit();
            this.TabPageSpecifications.SuspendLayout();
            this.TabPageColumnDesigner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVProductsConnections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGVFeedsConnections)).BeginInit();
            this.TabPageDesigner.SuspendLayout();
            this.TabPageStreams.SuspendLayout();
            this.tabPageStageProps.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.TabePageSolver);
            this.Tabs.Controls.Add(this.TabPageEstimates);
            this.Tabs.Controls.Add(this.TabPageSpecifications);
            this.Tabs.Controls.Add(this.TabPageColumnDesigner);
            this.Tabs.Controls.Add(this.TabPageDesigner);
            this.Tabs.Controls.Add(this.TabPageStreams);
            this.Tabs.Controls.Add(this.tabPageStageProps);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new  System.Drawing.Point (0, 0);
            this.Tabs.Margin = new  System.Windows.Forms.Padding(2);
            this.Tabs.Multiline = true;
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new  System.Drawing.Size(912, 527);
            this.Tabs.TabIndex = 0;
            this.Tabs.Selected += new  System.Windows.Forms.TabControlEventHandler(this.Tabs_Selected);
            // 
            // TabePageSolver
            // 
            this.TabePageSolver.BackColor = System.Drawing.SystemColors.Control;
            this.TabePageSolver.Controls.Add(this.checkBoxActive);
            this.TabePageSolver.Controls.Add(this.label15);
            this.TabePageSolver.Controls.Add(this.button11);
            this.TabePageSolver.Controls.Add(this.button12);
            this.TabePageSolver.Controls.Add(this.textBox1);
            this.TabePageSolver.Controls.Add(this.button13);
            this.TabePageSolver.Controls.Add(this.ComponentProfileDG);
            this.TabePageSolver.Controls.Add(this.cbResetInitialTemps);
            this.TabePageSolver.Controls.Add(this.CB_SplitMainFeed);
            this.TabePageSolver.Controls.Add(this.label7);
            this.TabePageSolver.Controls.Add(this.txtOuterTolerance);
            this.TabePageSolver.Controls.Add(this.label8);
            this.TabePageSolver.Controls.Add(this.txtInnerTolerance);
            this.TabePageSolver.Controls.Add(this.label6);
            this.TabePageSolver.Controls.Add(this.txtMaxInnerIterations);
            this.TabePageSolver.Controls.Add(this.label4);
            this.TabePageSolver.Controls.Add(this.txtMaxOuterIterations);
            this.TabePageSolver.Controls.Add(this.label3);
            this.TabePageSolver.Controls.Add(this.DampFactortxt);
            this.TabePageSolver.Controls.Add(this.label2);
            this.TabePageSolver.Controls.Add(this.label1);
            this.TabePageSolver.Controls.Add(this.Error2);
            this.TabePageSolver.Controls.Add(this.Error1);
            this.TabePageSolver.Location = new  System.Drawing.Point (4, 24);
            this.TabePageSolver.Margin = new  System.Windows.Forms.Padding(2);
            this.TabePageSolver.Name = "TabePageSolver";
            this.TabePageSolver.Padding = new  System.Windows.Forms.Padding(2);
            this.TabePageSolver.Size = new  System.Drawing.Size(904, 499);
            this.TabePageSolver.TabIndex = 0;
            this.TabePageSolver.Text = "Solver";
            // 
            // checkBoxActive
            // 
            this.checkBoxActive.AutoSize = true;
            this.checkBoxActive.Location = new  System.Drawing.Point (769, 472);
            this.checkBoxActive.Name = "checkBoxActive";
            this.checkBoxActive.Size = new  System.Drawing.Size(59, 19);
            this.checkBoxActive.TabIndex = 28;
            this.checkBoxActive.Text = "Active";
            this.checkBoxActive.UseVisualStyleBackColor = true;
            this.checkBoxActive.CheckedChanged += new  System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new  System.Drawing.Point (442, 13);
            this.label15.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new  System.Drawing.Size(46, 15);
            this.label15.TabIndex = 27;
            this.label15.Text = "Profiles";
            // 
            // button11
            // 
            this.button11.Location = new  System.Drawing.Point (118, 446);
            this.button11.Margin = new  System.Windows.Forms.Padding(2);
            this.button11.Name = "button11";
            this.button11.Size = new  System.Drawing.Size(82, 35);
            this.button11.TabIndex = 26;
            this.button11.Text = "Step";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Location = new  System.Drawing.Point (204, 446);
            this.button12.Margin = new  System.Windows.Forms.Padding(2);
            this.button12.Name = "button12";
            this.button12.Size = new  System.Drawing.Size(82, 35);
            this.button12.TabIndex = 25;
            this.button12.Text = "Reset";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new  System.Drawing.Point (290, 458);
            this.textBox1.Margin = new  System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new  System.Drawing.Size(109, 23);
            this.textBox1.TabIndex = 24;
            // 
            // button13
            // 
            this.button13.Location = new  System.Drawing.Point (31, 446);
            this.button13.Margin = new  System.Windows.Forms.Padding(2);
            this.button13.Name = "button13";
            this.button13.Size = new  System.Drawing.Size(82, 35);
            this.button13.TabIndex = 23;
            this.button13.Text = "Calculate";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new  System.EventHandler(this.ButtonCalculate_Click);
            // 
            // ComponentProfileDG
            // 
            this.ComponentProfileDG.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.ComponentProfileDG.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ComponentProfileDG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ComponentProfileDG.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.ComponentProfileDG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ComponentProfileDG.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.ComponentProfileDG.Location = new  System.Drawing.Point (437, 31);
            this.ComponentProfileDG.Margin = new  System.Windows.Forms.Padding(2);
            this.ComponentProfileDG.Name = "ComponentProfileDG";
            this.ComponentProfileDG.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.ComponentProfileDG.RowHeadersVisible = false;
            this.ComponentProfileDG.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ComponentProfileDG.RowTemplate.Height = 24;
            this.ComponentProfileDG.Size = new  System.Drawing.Size(282, 461);
            this.ComponentProfileDG.TabIndex = 22;
            // 
            // cbResetInitialTemps
            // 
            this.cbResetInitialTemps.AutoSize = true;
            this.cbResetInitialTemps.Location = new  System.Drawing.Point (239, 310);
            this.cbResetInitialTemps.Margin = new  System.Windows.Forms.Padding(2);
            this.cbResetInitialTemps.Name = "cbResetInitialTemps";
            this.cbResetInitialTemps.Size = new  System.Drawing.Size(123, 19);
            this.cbResetInitialTemps.TabIndex = 19;
            this.cbResetInitialTemps.Text = "Reset Initial Temps";
            this.cbResetInitialTemps.UseVisualStyleBackColor = true;
            this.cbResetInitialTemps.CheckedChanged += new  System.EventHandler(this.CB_ResetInitialTemps_CheckedChanged);
            // 
            // CB_SplitMainFeed
            // 
            this.CB_SplitMainFeed.AutoSize = true;
            this.CB_SplitMainFeed.Checked = true;
            this.CB_SplitMainFeed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_SplitMainFeed.Location = new  System.Drawing.Point (239, 334);
            this.CB_SplitMainFeed.Margin = new  System.Windows.Forms.Padding(2);
            this.CB_SplitMainFeed.Name = "CB_SplitMainFeed";
            this.CB_SplitMainFeed.Size = new  System.Drawing.Size(107, 19);
            this.CB_SplitMainFeed.TabIndex = 18;
            this.CB_SplitMainFeed.Text = "Split Main Feed";
            this.CB_SplitMainFeed.UseVisualStyleBackColor = true;
            this.CB_SplitMainFeed.CheckedChanged += new  System.EventHandler(this.CB_SplitMainFeed_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new  System.Drawing.Point (27, 419);
            this.label7.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new  System.Drawing.Size(120, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "Outer Loop Tolerance";
            // 
            // txtOuterTolerance
            // 
            this.txtOuterTolerance.Location = new  System.Drawing.Point (174, 416);
            this.txtOuterTolerance.Margin = new  System.Windows.Forms.Padding(2);
            this.txtOuterTolerance.Name = "txtOuterTolerance";
            this.txtOuterTolerance.Size = new  System.Drawing.Size(44, 23);
            this.txtOuterTolerance.TabIndex = 16;
            this.txtOuterTolerance.Text = "0.001";
            this.txtOuterTolerance.TextChanged += new  System.EventHandler(this.TxtOuterTolerance_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new  System.Drawing.Point (27, 393);
            this.label8.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new  System.Drawing.Size(117, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "Inner Loop Tolerance";
            // 
            // txtInnerTolerance
            // 
            this.txtInnerTolerance.Location = new  System.Drawing.Point (174, 390);
            this.txtInnerTolerance.Margin = new  System.Windows.Forms.Padding(2);
            this.txtInnerTolerance.Name = "txtInnerTolerance";
            this.txtInnerTolerance.Size = new  System.Drawing.Size(44, 23);
            this.txtInnerTolerance.TabIndex = 14;
            this.txtInnerTolerance.Text = "0.001";
            this.txtInnerTolerance.TextChanged += new  System.EventHandler(this.TxtInnerTolerance_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new  System.Drawing.Point (27, 366);
            this.label6.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new  System.Drawing.Size(112, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Max Inner Iterations";
            // 
            // txtMaxInnerIterations
            // 
            this.txtMaxInnerIterations.Location = new  System.Drawing.Point (174, 362);
            this.txtMaxInnerIterations.Margin = new  System.Windows.Forms.Padding(2);
            this.txtMaxInnerIterations.Name = "txtMaxInnerIterations";
            this.txtMaxInnerIterations.Size = new  System.Drawing.Size(44, 23);
            this.txtMaxInnerIterations.TabIndex = 11;
            this.txtMaxInnerIterations.Text = "50";
            this.txtMaxInnerIterations.TextChanged += new  System.EventHandler(this.TxtMaxInnerIterations_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new  System.Drawing.Point (27, 338);
            this.label4.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new  System.Drawing.Size(115, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Max Outer Iterations";
            // 
            // txtMaxOuterIterations
            // 
            this.txtMaxOuterIterations.Location = new  System.Drawing.Point (174, 336);
            this.txtMaxOuterIterations.Margin = new  System.Windows.Forms.Padding(2);
            this.txtMaxOuterIterations.Name = "txtMaxOuterIterations";
            this.txtMaxOuterIterations.Size = new  System.Drawing.Size(44, 23);
            this.txtMaxOuterIterations.TabIndex = 9;
            this.txtMaxOuterIterations.Text = "50";
            this.txtMaxOuterIterations.TextChanged += new  System.EventHandler(this.TxtMaxOuterIterations_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new  System.Drawing.Point (27, 312);
            this.label3.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new  System.Drawing.Size(92, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Damping Factor";
            // 
            // DampFactortxt
            // 
            this.DampFactortxt.Location = new  System.Drawing.Point (174, 307);
            this.DampFactortxt.Margin = new  System.Windows.Forms.Padding(2);
            this.DampFactortxt.Name = "DampFactortxt";
            this.DampFactortxt.Size = new  System.Drawing.Size(44, 23);
            this.DampFactortxt.TabIndex = 7;
            this.DampFactortxt.Text = "0.7";
            this.DampFactortxt.TextChanged += new  System.EventHandler(this.DampFactortxt_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new  System.Drawing.Point (229, 13);
            this.label2.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new  System.Drawing.Size(95, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Outer Loop Error";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new  System.Drawing.Point (24, 13);
            this.label1.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new  System.Drawing.Size(92, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Inner Loop Error";
            // 
            // Error2
            // 
            this.Error2.Location = new  System.Drawing.Point (232, 30);
            this.Error2.Margin = new  System.Windows.Forms.Padding(2);
            this.Error2.Name = "Error2";
            this.Error2.Size = new  System.Drawing.Size(201, 273);
            this.Error2.TabIndex = 2;
            this.Error2.Text = "";
            // 
            // Error1
            // 
            this.Error1.Location = new  System.Drawing.Point (28, 30);
            this.Error1.Margin = new  System.Windows.Forms.Padding(2);
            this.Error1.Name = "Error1";
            this.Error1.Size = new  System.Drawing.Size(199, 273);
            this.Error1.TabIndex = 0;
            this.Error1.Text = "";
            // 
            // TabPageEstimates
            // 
            this.TabPageEstimates.BackColor = System.Drawing.SystemColors.Control;
            this.TabPageEstimates.Controls.Add(this.DGVEfficiencies);
            this.TabPageEstimates.Controls.Add(this.DGVPressures);
            this.TabPageEstimates.Controls.Add(this.label16);
            this.TabPageEstimates.Controls.Add(this.DiagnosticDataGridView);
            this.TabPageEstimates.Controls.Add(this.button8);
            this.TabPageEstimates.Controls.Add(this.button7);
            this.TabPageEstimates.Controls.Add(this.btnEstimateTs);
            this.TabPageEstimates.Location = new  System.Drawing.Point (4, 24);
            this.TabPageEstimates.Margin = new  System.Windows.Forms.Padding(2);
            this.TabPageEstimates.Name = "TabPageEstimates";
            this.TabPageEstimates.Padding = new  System.Windows.Forms.Padding(2);
            this.TabPageEstimates.Size = new  System.Drawing.Size(904, 499);
            this.TabPageEstimates.TabIndex = 3;
            this.TabPageEstimates.Text = "Estimates";
            // 
            // DGVEfficiencies
            // 
            this.DGVEfficiencies.AllowUserToAddRows = false;
            this.DGVEfficiencies.AllowUserToDeleteRows = false;
            this.DGVEfficiencies.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("DGVEfficiencies.ColumnNames")));
            this.DGVEfficiencies.DisplayTitles = true;
            this.DGVEfficiencies.FirstColumnWidth = 64;
            this.DGVEfficiencies.Location = new  System.Drawing.Point (163, 11);
            this.DGVEfficiencies.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DGVEfficiencies.Name = "DGVEfficiencies";
            this.DGVEfficiencies.ReadOnly  = false;
            this.DGVEfficiencies.RowHeadersVisible = false;
            this.DGVEfficiencies.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("DGVEfficiencies.RowNames")));
            this.DGVEfficiencies.Size = new  System.Drawing.Size(156, 467);
            this.DGVEfficiencies.TabIndex = 16;
            this.DGVEfficiencies.TopText = "Tray Efficiencies";
            // 
            // DGVPressure s
            // 
            this.DGVPressures.AllowUserToAddRows = false;
            this.DGVPressures.AllowUserToDeleteRows = false;
            this.DGVPressures.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("DGVPressure s.ColumnNames")));
            this.DGVPressures.DisplayTitles = true;
            this.DGVPressures.FirstColumnWidth = 64;
            this.DGVPressures.Location = new  System.Drawing.Point (6, 11);
            this.DGVPressures.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DGVPressures.Name = "DGVPressure s";
            this.DGVPressures.ReadOnly  = false;
            this.DGVPressures.RowHeadersVisible = false;
            this.DGVPressures.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("DGVPressure s.RowNames")));
            this.DGVPressures.Size = new  System.Drawing.Size(140, 467);
            this.DGVPressures.TabIndex = 15;
            this.DGVPressures.TopText = "Column Pressure s";
            this.DGVPressures.ValueChanged += new  FormControls.UserPropGrid.ValueChangedEventHandler(this.DgvEstimates_ValueChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new  System.Drawing.Point (354, 11);
            this.label16.Margin = new  System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new  System.Drawing.Size(116, 15);
            this.label16.TabIndex = 14;
            this.label16.Text = "Column Information";
            // 
            // DiagnosticDataGridView
            // 
            this.DiagnosticDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.DiagnosticDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DiagnosticDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DiagnosticDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.DiagnosticDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DiagnosticDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.DiagnosticDataGridView.Location = new  System.Drawing.Point (354, 28);
            this.DiagnosticDataGridView.Margin = new  System.Windows.Forms.Padding(2);
            this.DiagnosticDataGridView.Name = "DiagnosticDataGridView";
            this.DiagnosticDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.DiagnosticDataGridView.RowHeadersVisible = false;
            this.DiagnosticDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DiagnosticDataGridView.RowTemplate.Height = 24;
            this.DiagnosticDataGridView.Size = new  System.Drawing.Size(543, 464);
            this.DiagnosticDataGridView.TabIndex = 13;
            // 
            // button8
            // 
            this.button8.Location = new  System.Drawing.Point (136, 541);
            this.button8.Margin = new  System.Windows.Forms.Padding(2);
            this.button8.Name = "button8";
            this.button8.Size = new  System.Drawing.Size(103, 32);
            this.button8.TabIndex = 12;
            this.button8.Text = "Clear Estimates";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new  System.EventHandler(this.BtnResetEstimates_Click);
            // 
            // button7
            // 
            this.button7.Location = new  System.Drawing.Point (243, 541);
            this.button7.Margin = new  System.Windows.Forms.Padding(2);
            this.button7.Name = "button7";
            this.button7.Size = new  System.Drawing.Size(164, 32);
            this.button7.TabIndex = 11;
            this.button7.Text = "Load T From Solution";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new  System.EventHandler(this.Button7_Click);
            // 
            // btnEstimateTs
            // 
            this.btnEstimateTs.Location = new  System.Drawing.Point (29, 541);
            this.btnEstimateTs.Margin = new  System.Windows.Forms.Padding(2);
            this.btnEstimateTs.Name = "btnEstimateTs";
            this.btnEstimateTs.Size = new  System.Drawing.Size(103, 32);
            this.btnEstimateTs.TabIndex = 10;
            this.btnEstimateTs.Text = "Estimate T";
            this.btnEstimateTs.UseVisualStyleBackColor = true;
            this.btnEstimateTs.Click += new  System.EventHandler(this.BtnEstT_Click);
            // 
            // TabPageSpecifications
            // 
            this.TabPageSpecifications.BackColor = System.Drawing.SystemColors.Control;
            this.TabPageSpecifications.Controls.Add(this.specificationSheet1);
            this.TabPageSpecifications.Location = new  System.Drawing.Point (4, 24);
            this.TabPageSpecifications.Margin = new  System.Windows.Forms.Padding(2);
            this.TabPageSpecifications.Name = "TabPageSpecifications";
            this.TabPageSpecifications.Padding = new  System.Windows.Forms.Padding(2);
            this.TabPageSpecifications.Size = new  System.Drawing.Size(904, 499);
            this.TabPageSpecifications.TabIndex = 6;
            this.TabPageSpecifications.Text = "Specifications";
            // 
            // specificationSheet1
            // 
            this.specificationSheet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.specificationSheet1.Location = new  System.Drawing.Point (2, 2);
            this.specificationSheet1.Margin = new  System.Windows.Forms.Padding(2);
            this.specificationSheet1.Name = "specificationSheet1";
            this.specificationSheet1.Size = new  System.Drawing.Size(900, 495);
            this.specificationSheet1.TabIndex = 0;
            // 
            // TabPageColumnDesigner
            // 
            this.TabPageColumnDesigner.BackColor = System.Drawing.SystemColors.Control;
            this.TabPageColumnDesigner.Controls.Add(this.dGVProductsConnections);
            this.TabPageColumnDesigner.Controls.Add(this.dGVFeedsConnections);
            this.TabPageColumnDesigner.Location = new  System.Drawing.Point (4, 24);
            this.TabPageColumnDesigner.Margin = new  System.Windows.Forms.Padding(2);
            this.TabPageColumnDesigner.Name = "TabPageColumnDesigner";
            this.TabPageColumnDesigner.Padding = new  System.Windows.Forms.Padding(2);
            this.TabPageColumnDesigner.Size = new  System.Drawing.Size(904, 499);
            this.TabPageColumnDesigner.TabIndex = 1;
            this.TabPageColumnDesigner.Text = "Column Configuration";
            // 
            // dGVProductsConnections
            // 
            this.dGVProductsConnections.AllowUserToAddRows = false;
            this.dGVProductsConnections.AllowUserToDeleteRows = false;
            this.dGVProductsConnections.AllowUserToResizeRows = false;
            this.dGVProductsConnections.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dGVProductsConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVProductsConnections.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewComboBoxColumn1});
            this.dGVProductsConnections.Location = new  System.Drawing.Point (320, 32);
            this.dGVProductsConnections.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dGVProductsConnections.Name = "dGVProductsConnections";
            this.dGVProductsConnections.RowHeadersVisible = false;
            this.dGVProductsConnections.Size = new  System.Drawing.Size(250, 228);
            this.dGVProductsConnections.TabIndex = 28;
            this.dGVProductsConnections.EditingControlShowing += new  System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DataGridViewProductConnections_EditingControlShowing);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "ExternalStream";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.HeaderText = "public Stream";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            // 
            // dGVFeedsConnections
            // 
            this.dGVFeedsConnections.AllowUserToAddRows = false;
            this.dGVFeedsConnections.AllowUserToDeleteRows = false;
            this.dGVFeedsConnections.AllowUserToResizeRows = false;
            this.dGVFeedsConnections.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dGVFeedsConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVFeedsConnections.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.ExternalName,
            this.publicName});
            this.dGVFeedsConnections.Location = new  System.Drawing.Point (38, 32);
            this.dGVFeedsConnections.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dGVFeedsConnections.Name = "dGVFeedsConnections";
            this.dGVFeedsConnections.RowHeadersVisible = false;
            this.dGVFeedsConnections.Size = new  System.Drawing.Size(250, 228);
            this.dGVFeedsConnections.TabIndex = 27;
            this.dGVFeedsConnections.EditingControlShowing += new  System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DataGridViewFeedConnections_EditingControlShowing);
            // 
            // ExternalName
            // 
            this.ExternalName.HeaderText = "ExternalStream";
            this.ExternalName.Name = "ExternalName";
            this.ExternalName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ExternalName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // public Name
            // 
            this.publicName.HeaderText = "public Stream";
            this.publicName.Name = "public Name";
            // 
            // TabPageDesigner
            // 
            this.TabPageDesigner.Controls.Add(this.columnDesignerControl1);
            this.TabPageDesigner.Location = new  System.Drawing.Point (4, 24);
            this.TabPageDesigner.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPageDesigner.Name = "TabPageDesigner";
            this.TabPageDesigner.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPageDesigner.Size = new  System.Drawing.Size(904, 499);
            this.TabPageDesigner.TabIndex = 9;
            this.TabPageDesigner.Text = "Visual Designer";
            this.TabPageDesigner.UseVisualStyleBackColor = true;
            // 
            // columnDesignerControl1
            // 
            this.columnDesignerControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.columnDesignerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.columnDesignerControl1.GraphicsList = ((Units.GraphicsList)(resources.GetObject("columnDesignerControl1.GraphicsList")));
            this.columnDesignerControl1.Location = new  System.Drawing.Point (4, 3);
            this.columnDesignerControl1.Name = "columnDesignerControl1";
            this.columnDesignerControl1.Size = new  System.Drawing.Size(896, 493);
            this.columnDesignerControl1.TabIndex = 0;
            // 
            // TabPageStreams
            // 
            this.TabPageStreams.Controls.Add(this.worksheet);
            this.TabPageStreams.Location = new  System.Drawing.Point (4, 24);
            this.TabPageStreams.Margin = new  System.Windows.Forms.Padding(5, 3, 5, 3);
            this.TabPageStreams.Name = "TabPageStreams";
            this.TabPageStreams.Padding = new  System.Windows.Forms.Padding(5, 3, 5, 3);
            this.TabPageStreams.Size = new  System.Drawing.Size(904, 499);
            this.TabPageStreams.TabIndex = 10;
            this.TabPageStreams.Text = "Streams";
            this.TabPageStreams.UseVisualStyleBackColor = true;
            // 
            // worksheet
            // 
            this.worksheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.worksheet.Location = new  System.Drawing.Point (5, 3);
            this.worksheet.Margin = new  System.Windows.Forms.Padding(2);
            this.worksheet.Name = "worksheet";
            this.worksheet.Size = new  System.Drawing.Size(894, 493);
            this.worksheet.TabIndex = 0;
            // 
            // tabPageStageProps
            // 
            this.tabPageStageProps.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageStageProps.Controls.Add(this.groupBox2);
            this.tabPageStageProps.Controls.Add(this.dataGridView1);
            this.tabPageStageProps.Location = new  System.Drawing.Point (4, 24);
            this.tabPageStageProps.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageStageProps.Name = "tabPageStageProps";
            this.tabPageStageProps.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageStageProps.Size = new  System.Drawing.Size(904, 499);
            this.tabPageStageProps.TabIndex = 11;
            this.tabPageStageProps.Text = "Stage Properties";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton10);
            this.groupBox2.Controls.Add(this.radioButton9);
            this.groupBox2.Controls.Add(this.radioButton8);
            this.groupBox2.Controls.Add(this.radioButton7);
            this.groupBox2.Controls.Add(this.radioButton6);
            this.groupBox2.Controls.Add(this.radioButton5);
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Location = new  System.Drawing.Point (9, 7);
            this.groupBox2.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new  System.Drawing.Size(209, 571);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Property";
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new  System.Drawing.Point (20, 143);
            this.radioButton10.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new  System.Drawing.Size(134, 19);
            this.radioButton10.TabIndex = 9;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "Composition Vapour";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new  System.Drawing.Point (20, 118);
            this.radioButton9.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new  System.Drawing.Size(130, 19);
            this.radioButton9.TabIndex = 8;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "Composition Liquid";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new  System.Drawing.Point (20, 65);
            this.radioButton8.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new  System.Drawing.Size(69, 19);
            this.radioButton8.TabIndex = 7;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Pressure";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new  System.Drawing.Point (20, 38);
            this.radioButton7.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new  System.Drawing.Size(91, 19);
            this.radioButton7.TabIndex = 6;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "Temperature";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new  System.Drawing.Point (20, 276);
            this.radioButton6.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new  System.Drawing.Size(107, 19);
            this.radioButton6.TabIndex = 5;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "Surface Tension";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new  System.Drawing.Point (20, 249);
            this.radioButton5.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new  System.Drawing.Size(139, 19);
            this.radioButton5.TabIndex = 4;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Thermal Conductivity";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new  System.Drawing.Point (20, 223);
            this.radioButton4.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new  System.Drawing.Size(71, 19);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Viscosity";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new  System.Drawing.Point (20, 196);
            this.radioButton3.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new  System.Drawing.Size(68, 19);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "K Values";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new  System.Drawing.Point (20, 170);
            this.radioButton2.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new  System.Drawing.Size(71, 19);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Enthalpy";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new  System.Drawing.Point (20, 91);
            this.radioButton1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new  System.Drawing.Size(55, 19);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Flows";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridView1.Location = new  System.Drawing.Point (224, 16);
            this.dataGridView1.Margin = new  System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new  System.Drawing.Size(673, 481);
            this.dataGridView1.TabIndex = 14;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ExternalStream";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 69;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Pressure";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 87;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "ExternalStream";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 111;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "ExternalStream";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Width = 106;
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.HeaderText = "public Stream";
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            this.dataGridViewComboBoxColumn2.Width = 105;
            // 
            // LLEDLG
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(912, 527);
            this.Controls.Add(this.Tabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "LLEDLG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ColumnDLG";
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.LLEDLG_FormClosing);
            this.Load += new  System.EventHandler(this.ColumnDLG_Load);
            this.KeyDown += new  System.Windows.Forms.KeyEventHandler(this.ColumnDLG_KeyDown);
            this.Tabs.ResumeLayout(false);
            this.TabePageSolver.ResumeLayout(false);
            this.TabePageSolver.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComponentProfileDG)).EndInit();
            this.TabPageEstimates.ResumeLayout(false);
            this.TabPageEstimates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DiagnosticDataGridView)).EndInit();
            this.TabPageSpecifications.ResumeLayout(false);
            this.TabPageColumnDesigner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVProductsConnections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGVFeedsConnections)).EndInit();
            this.TabPageDesigner.ResumeLayout(false);
            this.TabPageStreams.ResumeLayout(false);
            this.tabPageStageProps.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private  TabControl Tabs;
        private  TabPage TabPageColumnDesigner;
        private  TabPage TabePageSolver;
        private  Label label2;
        private  Label label1;
        public  RichTextBox Error2;
        public  RichTextBox Error1;
        private  Label label3;
        private  TextBox DampFactortxt;
        private  TabPage TabPageEstimates;
        private  TabPage TabPageSpecifications;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  Button button7;
        private  Button btnEstimateTs;
        private  Button button8;
        private  Label label6;
        private  TextBox txtMaxInnerIterations;
        private  Label label4;
        private  TextBox txtMaxOuterIterations;
        private  Label label7;
        private  TextBox txtOuterTolerance;
        private  Label label8;
        private  TextBox txtInnerTolerance;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private  DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private  CheckBox CB_SplitMainFeed;
        private  CheckBox cbResetInitialTemps;
        private  System.ComponentModel.BackgroundWorker backgroundWorker1;
        private  TabPage TabPageDesigner;
        private  DataGridView ComponentProfileDG;
        private  Button button11;
        private  Button button12;
        public  TextBox textBox1;
        private  Button button13;
        private  Label label15;
        private  Label label16;
        private  DataGridView DiagnosticDataGridView;
        private  DataGridView dGVProductsConnections;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private  DataGridView dGVFeedsConnections;
        private  DataGridViewTextBoxColumn ExternalName;
        private  DataGridViewComboBoxColumn publicName;
        private  TabPage TabPageStreams;
        private  TabPage tabPageStageProps;
        private  GroupBox groupBox2;
        private  RadioButton radioButton8;
        private  RadioButton radioButton7;
        private  RadioButton radioButton6;
        private  RadioButton radioButton5;
        private  RadioButton radioButton4;
        private  RadioButton radioButton3;
        private  RadioButton radioButton2;
        private  RadioButton radioButton1;
        private  DataGridView dataGridView1;
        private  RadioButton radioButton10;
        private  RadioButton radioButton9;
        private  FormControls.UserPropGrid DGVPressures;
        private  SpecificationSheet specificationSheet1;
        private  PortsPropertyWorksheet worksheet;
        internal  ColumnDesignerControl columnDesignerControl1;
        private  CheckBox checkBoxActive;
        private  FormControls.UserPropGrid DGVEfficiencies;
    }
}