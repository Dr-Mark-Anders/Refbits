using   ModelEngine;
using   System.Windows.Forms;

namespace   Units
{
    partial class  SpecificationSheet
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
                Components.Dispose();
            
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private  void  InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(SpecificationSheet));
            this.DGV = new  System.Windows.Forms.DataGridView();
            this.SpecName = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Active = new  System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Edit = new  System.Windows.Forms.DataGridViewButtonColumn();
            this.SpecGuid = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddSpec = new  System.Windows.Forms.Button();
            this.DelSpec = new  System.Windows.Forms.Button();
            this.txtDegOfFreedom = new  System.Windows.Forms.TextBox();
            this.DegOfFreedom = new  System.Windows.Forms.Label();
            this.rbLiquidFlow = new  System.Windows.Forms.RadioButton();
            this.SpecGroupBox = new  System.Windows.Forms.GroupBox();
            this.rdDistSpec = new  System.Windows.Forms.RadioButton();
            this.rbVapStreamRate = new  System.Windows.Forms.RadioButton();
            this.rbLiquidStreamRate = new  System.Windows.Forms.RadioButton();
            this.rbPADuty = new  System.Windows.Forms.RadioButton();
            this.rbPADeltaT = new  System.Windows.Forms.RadioButton();
            this.rbPAFlow = new  System.Windows.Forms.RadioButton();
            this.rbPARetT = new  System.Windows.Forms.RadioButton();
            this.rbVapourDrawRate = new  System.Windows.Forms.RadioButton();
            this.rbRefluxRatio = new  System.Windows.Forms.RadioButton();
            this.rbLiquidDrawRate = new  System.Windows.Forms.RadioButton();
            this.rbRefluxRate = new  System.Windows.Forms.RadioButton();
            this.rbStageTemperature  = new  System.Windows.Forms.RadioButton();
            this.rbVapourFlow = new  System.Windows.Forms.RadioButton();
            this.label1 = new  System.Windows.Forms.Label();
            this.txtActSpecs = new  System.Windows.Forms.TextBox();
            this.txtReqiredSpecs = new  System.Windows.Forms.TextBox();
            this.label2 = new  System.Windows.Forms.Label();
            this.CBOX1 = new  System.Windows.Forms.ComboBox();
            this.dataGridViewTextBoxColumn1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.condenserTypeControl1 = new  Units.CondenserTypeControl();
            this.groupBox1 = new  System.Windows.Forms.GroupBox();
            this.upPressures = new  FormControls.UserPropGrid();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SpecGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.SpecName,
            this.Column2,
            this.Column3,
            this.Active,
            this.Edit,
            this.SpecGuid});
            this.DGV.Location = new  System.Drawing.Point (0, 0);
            this.DGV.Margin = new  System.Windows.Forms.Padding(2);
            this.DGV.Name = "DGV";
            this.DGV.RowTemplate.Height = 24;
            this.DGV.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.Size = new  System.Drawing.Size(498, 433);
            this.DGV.TabIndex = 0;
            this.DGV.CellClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellClick);
            this.DGV.CellContentClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.DGV.CellDoubleClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CelldoubleClick);
            this.DGV.CellEndEdit += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEndEdit);
            this.DGV.CellValueChanged += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellValueChanged);
            this.DGV.CurrentCellDirtyStateChanged += new  System.EventHandler(this.DataGridView1_CurrentCellDirtyStateChanged);
            this.DGV.Paint  += new  System.Windows.Forms.PaintEventHandler(this.DataGridView1_Paint );
            // 
            // SpecName
            // 
            this.SpecName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SpecName.HeaderText = "Name";
            this.SpecName.Name = "SpecName";
            this.SpecName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SpecName.Width = 95;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.HeaderText = "SpecValue";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 95;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.HeaderText = "Value";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 95;
            // 
            // Active
            // 
            this.Active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            this.Active.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Active.Width = 75;
            // 
            // Edit
            // 
            this.Edit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Edit.HeaderText = "Configure";
            this.Edit.Name = "Edit";
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Edit.Text = "Edit";
            this.Edit.ToolTipText = "Configure Spec";
            this.Edit.UseColumnTextForButtonValue = true;
            // 
            // SpecGuid
            // 
            this.SpecGuid.HeaderText = "SpecGuid";
            this.SpecGuid.Name = "SpecGuid";
            this.SpecGuid.Visible = false;
            this.SpecGuid.Width = 82;
            // 
            // AddSpec
            // 
            this.AddSpec.Location = new  System.Drawing.Point (509, 406);
            this.AddSpec.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.AddSpec.Name = "AddSpec";
            this.AddSpec.Size = new  System.Drawing.Size(82, 27);
            this.AddSpec.TabIndex = 1;
            this.AddSpec.Text = "Add Spec";
            this.AddSpec.UseVisualStyleBackColor = true;
            this.AddSpec.Click += new  System.EventHandler(this.AddSpec_Click);
            // 
            // DelSpec
            // 
            this.DelSpec.Location = new  System.Drawing.Point (599, 406);
            this.DelSpec.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DelSpec.Name = "DelSpec";
            this.DelSpec.Size = new  System.Drawing.Size(82, 27);
            this.DelSpec.TabIndex = 2;
            this.DelSpec.Text = "Delete Spec";
            this.DelSpec.UseVisualStyleBackColor = true;
            this.DelSpec.Click += new  System.EventHandler(this.DelSpec_Click);
            // 
            // txtDegOfFreedom
            // 
            this.txtDegOfFreedom.Location = new  System.Drawing.Point (128, 85);
            this.txtDegOfFreedom.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDegOfFreedom.Name = "txtDegOfFreedom";
            this.txtDegOfFreedom.Size = new  System.Drawing.Size(51, 23);
            this.txtDegOfFreedom.TabIndex = 3;
            // 
            // DegOfFreedom
            // 
            this.DegOfFreedom.AutoSize = true;
            this.DegOfFreedom.Location = new  System.Drawing.Point (7, 93);
            this.DegOfFreedom.Margin = new  System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DegOfFreedom.Name = "DegOfFreedom";
            this.DegOfFreedom.Size = new  System.Drawing.Size(113, 15);
            this.DegOfFreedom.TabIndex = 4;
            this.DegOfFreedom.Text = "Degrees of Freedom";
            // 
            // rbLiquidFlow
            // 
            this.rbLiquidFlow.AutoSize = true;
            this.rbLiquidFlow.Location = new  System.Drawing.Point (8, 168);
            this.rbLiquidFlow.Margin = new  System.Windows.Forms.Padding(2);
            this.rbLiquidFlow.Name = "rbLiquidFlow";
            this.rbLiquidFlow.Size = new  System.Drawing.Size(132, 19);
            this.rbLiquidFlow.TabIndex = 5;
            this.rbLiquidFlow.Text = "Tray Net Liquid Flow";
            this.rbLiquidFlow.UseVisualStyleBackColor = true;
            // 
            // SpecGroupBox
            // 
            this.SpecGroupBox.Controls.Add(this.rdDistSpec);
            this.SpecGroupBox.Controls.Add(this.rbVapStreamRate);
            this.SpecGroupBox.Controls.Add(this.rbLiquidStreamRate);
            this.SpecGroupBox.Controls.Add(this.rbPADuty);
            this.SpecGroupBox.Controls.Add(this.rbPADeltaT);
            this.SpecGroupBox.Controls.Add(this.rbPAFlow);
            this.SpecGroupBox.Controls.Add(this.rbPARetT);
            this.SpecGroupBox.Controls.Add(this.rbVapourDrawRate);
            this.SpecGroupBox.Controls.Add(this.rbRefluxRatio);
            this.SpecGroupBox.Controls.Add(this.rbLiquidFlow);
            this.SpecGroupBox.Controls.Add(this.rbLiquidDrawRate);
            this.SpecGroupBox.Controls.Add(this.rbRefluxRate);
            this.SpecGroupBox.Controls.Add(this.rbStageTemperature );
            this.SpecGroupBox.Controls.Add(this.rbVapourFlow);
            this.SpecGroupBox.Location = new  System.Drawing.Point (502, 2);
            this.SpecGroupBox.Margin = new  System.Windows.Forms.Padding(2);
            this.SpecGroupBox.Name = "SpecGroupBox";
            this.SpecGroupBox.Padding = new  System.Windows.Forms.Padding(2);
            this.SpecGroupBox.Size = new  System.Drawing.Size(180, 399);
            this.SpecGroupBox.TabIndex = 6;
            this.SpecGroupBox.TabStop = false;
            this.SpecGroupBox.Text = "Specification Type";
            // 
            // rdDistSpec
            // 
            this.rdDistSpec.AutoSize = true;
            this.rdDistSpec.Location = new  System.Drawing.Point (8, 343);
            this.rdDistSpec.Margin = new  System.Windows.Forms.Padding(2);
            this.rdDistSpec.Name = "rdDistSpec";
            this.rdDistSpec.Size = new  System.Drawing.Size(112, 19);
            this.rdDistSpec.TabIndex = 17;
            this.rdDistSpec.Text = "Distillation Point ";
            this.rdDistSpec.UseVisualStyleBackColor = true;
            // 
            // rbVapStreamRate
            // 
            this.rbVapStreamRate.AutoSize = true;
            this.rbVapStreamRate.Location = new  System.Drawing.Point (8, 318);
            this.rbVapStreamRate.Margin = new  System.Windows.Forms.Padding(2);
            this.rbVapStreamRate.Name = "rbVapStreamRate";
            this.rbVapStreamRate.Size = new  System.Drawing.Size(128, 19);
            this.rbVapStreamRate.TabIndex = 16;
            this.rbVapStreamRate.Text = "Vapour Stream Rate";
            this.rbVapStreamRate.UseVisualStyleBackColor = true;
            // 
            // rbLiquidStreamRate
            // 
            this.rbLiquidStreamRate.AutoSize = true;
            this.rbLiquidStreamRate.Location = new  System.Drawing.Point (8, 293);
            this.rbLiquidStreamRate.Margin = new  System.Windows.Forms.Padding(2);
            this.rbLiquidStreamRate.Name = "rbLiquidStreamRate";
            this.rbLiquidStreamRate.Size = new  System.Drawing.Size(124, 19);
            this.rbLiquidStreamRate.TabIndex = 12;
            this.rbLiquidStreamRate.Text = "Liquid Stream Rate";
            this.rbLiquidStreamRate.UseVisualStyleBackColor = true;
            // 
            // rbPADuty
            // 
            this.rbPADuty.AutoSize = true;
            this.rbPADuty.Location = new  System.Drawing.Point (8, 268);
            this.rbPADuty.Margin = new  System.Windows.Forms.Padding(2);
            this.rbPADuty.Name = "rbPADuty";
            this.rbPADuty.Size = new  System.Drawing.Size(123, 19);
            this.rbPADuty.TabIndex = 15;
            this.rbPADuty.Text = "Pumparound Duty";
            this.rbPADuty.UseVisualStyleBackColor = true;
            // 
            // rbPADeltaT
            // 
            this.rbPADeltaT.AutoSize = true;
            this.rbPADeltaT.Location = new  System.Drawing.Point (8, 243);
            this.rbPADeltaT.Margin = new  System.Windows.Forms.Padding(2);
            this.rbPADeltaT.Name = "rbPADeltaT";
            this.rbPADeltaT.Size = new  System.Drawing.Size(134, 19);
            this.rbPADeltaT.TabIndex = 14;
            this.rbPADeltaT.Text = "Pumparound Delta T";
            this.rbPADeltaT.UseVisualStyleBackColor = true;
            // 
            // rbPAFlow
            // 
            this.rbPAFlow.AutoSize = true;
            this.rbPAFlow.Location = new  System.Drawing.Point (8, 193);
            this.rbPAFlow.Margin = new  System.Windows.Forms.Padding(2);
            this.rbPAFlow.Name = "rbPAFlow";
            this.rbPAFlow.Size = new  System.Drawing.Size(123, 19);
            this.rbPAFlow.TabIndex = 13;
            this.rbPAFlow.Text = "Pumparound Flow";
            this.rbPAFlow.UseVisualStyleBackColor = true;
            // 
            // rbPARetT
            // 
            this.rbPARetT.AutoSize = true;
            this.rbPARetT.Location = new  System.Drawing.Point (8, 218);
            this.rbPARetT.Margin = new  System.Windows.Forms.Padding(2);
            this.rbPARetT.Name = "rbPARetT";
            this.rbPARetT.Size = new  System.Drawing.Size(124, 19);
            this.rbPARetT.TabIndex = 12;
            this.rbPARetT.Text = "Pumparound Ret T";
            this.rbPARetT.UseVisualStyleBackColor = true;
            // 
            // rbVapourDrawRate
            // 
            this.rbVapourDrawRate.AutoSize = true;
            this.rbVapourDrawRate.Location = new  System.Drawing.Point (8, 43);
            this.rbVapourDrawRate.Margin = new  System.Windows.Forms.Padding(2);
            this.rbVapourDrawRate.Name = "rbVapourDrawRate";
            this.rbVapourDrawRate.Size = new  System.Drawing.Size(163, 19);
            this.rbVapourDrawRate.TabIndex = 11;
            this.rbVapourDrawRate.Text = "Vapour Product Draw Rate";
            this.rbVapourDrawRate.UseVisualStyleBackColor = true;
            // 
            // rbRefluxRatio
            // 
            this.rbRefluxRatio.AutoSize = true;
            this.rbRefluxRatio.Location = new  System.Drawing.Point (8, 118);
            this.rbRefluxRatio.Margin = new  System.Windows.Forms.Padding(2);
            this.rbRefluxRatio.Name = "rbRefluxRatio";
            this.rbRefluxRatio.Size = new  System.Drawing.Size(88, 19);
            this.rbRefluxRatio.TabIndex = 9;
            this.rbRefluxRatio.Text = "Reflux Ratio";
            this.rbRefluxRatio.UseVisualStyleBackColor = true;
            // 
            // rbLiquidDrawRate
            // 
            this.rbLiquidDrawRate.AutoSize = true;
            this.rbLiquidDrawRate.Checked = true;
            this.rbLiquidDrawRate.Location = new  System.Drawing.Point (8, 18);
            this.rbLiquidDrawRate.Margin = new  System.Windows.Forms.Padding(2);
            this.rbLiquidDrawRate.Name = "rbLiquidDrawRate";
            this.rbLiquidDrawRate.Size = new  System.Drawing.Size(159, 19);
            this.rbLiquidDrawRate.TabIndex = 10;
            this.rbLiquidDrawRate.TabStop = true;
            this.rbLiquidDrawRate.Text = "Liquid Product Draw Rate";
            this.rbLiquidDrawRate.UseVisualStyleBackColor = true;
            // 
            // rbRefluxRate
            // 
            this.rbRefluxRate.AutoSize = true;
            this.rbRefluxRate.Location = new  System.Drawing.Point (8, 93);
            this.rbRefluxRate.Margin = new  System.Windows.Forms.Padding(2);
            this.rbRefluxRate.Name = "rbRefluxRate";
            this.rbRefluxRate.Size = new  System.Drawing.Size(84, 19);
            this.rbRefluxRate.TabIndex = 8;
            this.rbRefluxRate.Text = "Reflux Rate";
            this.rbRefluxRate.UseVisualStyleBackColor = true;
            // 
            // rbStageTemperature 
            // 
            this.rbStageTemperature .AutoSize = true;
            this.rbStageTemperature .Location = new  System.Drawing.Point (8, 68);
            this.rbStageTemperature .Margin = new  System.Windows.Forms.Padding(2);
            this.rbStageTemperature .Name = "rbStageTemperature ";
            this.rbStageTemperature .Size = new  System.Drawing.Size(123, 19);
            this.rbStageTemperature .TabIndex = 7;
            this.rbStageTemperature .Text = "Stage Temperature ";
            this.rbStageTemperature .UseVisualStyleBackColor = true;
            // 
            // rbVapourFlow
            // 
            this.rbVapourFlow.AutoSize = true;
            this.rbVapourFlow.Location = new  System.Drawing.Point (8, 143);
            this.rbVapourFlow.Margin = new  System.Windows.Forms.Padding(2);
            this.rbVapourFlow.Name = "rbVapourFlow";
            this.rbVapourFlow.Size = new  System.Drawing.Size(136, 19);
            this.rbVapourFlow.TabIndex = 6;
            this.rbVapourFlow.Text = "Tray Net Vapour Flow";
            this.rbVapourFlow.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new  System.Drawing.Point (7, 32);
            this.label1.Margin = new  System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new  System.Drawing.Size(102, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "No. Specifications";
            // 
            // txtActSpecs
            // 
            this.txtActSpecs.Location = new  System.Drawing.Point (128, 26);
            this.txtActSpecs.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtActSpecs.Name = "txtActSpecs";
            this.txtActSpecs.Size = new  System.Drawing.Size(51, 23);
            this.txtActSpecs.TabIndex = 9;
            // 
            // txtReqiredSpecs
            // 
            this.txtReqiredSpecs.Location = new  System.Drawing.Point (128, 56);
            this.txtReqiredSpecs.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtReqiredSpecs.Name = "txtReqiredSpecs";
            this.txtReqiredSpecs.Size = new  System.Drawing.Size(51, 23);
            this.txtReqiredSpecs.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new  System.Drawing.Point (7, 62);
            this.label2.Margin = new  System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new  System.Drawing.Size(87, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Required Specs";
            // 
            // CBOX1
            // 
            this.CBOX1.FormattingEnabled = true;
            this.CBOX1.Location = new  System.Drawing.Point (183, 135);
            this.CBOX1.Margin = new  System.Windows.Forms.Padding(2);
            this.CBOX1.Name = "CBOX1";
            this.CBOX1.Size = new  System.Drawing.Size(126, 23);
            this.CBOX1.TabIndex = 12;
            this.CBOX1.Visible = false;
            this.CBOX1.SelectedIndexChanged += new  System.EventHandler(this.CBOX1_SelectedIndexChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 147;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 148;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "Units";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 147;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Active";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            this.dataGridViewTextBoxColumn4.Width = 147;
            // 
            // condenserTypeControl1
            // 
            this.condenserTypeControl1.Location = new  System.Drawing.Point (681, 3);
            this.condenserTypeControl1.Margin = new  System.Windows.Forms.Padding(5, 3, 5, 3);
            this.condenserTypeControl1.Name = "condenserTypeControl1";
            this.condenserTypeControl1.Size = new  System.Drawing.Size(203, 137);
            this.condenserTypeControl1.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDegOfFreedom);
            this.groupBox1.Controls.Add(this.DegOfFreedom);
            this.groupBox1.Controls.Add(this.txtReqiredSpecs);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtActSpecs);
            this.groupBox1.Location = new  System.Drawing.Point (682, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new  System.Drawing.Size(202, 127);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spec Count";
            // 
            // upPressure s
            // 
            this.upPressures.AllowUserToAddRows = false;
            this.upPressures.AllowUserToDeleteRows = false;
            this.upPressures.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("upPressure s.ColumnNames")));
            this.upPressures.DisplayTitles = true;
            this.upPressures.FirstColumnWidth = 64;
            this.upPressures.Location = new  System.Drawing.Point (689, 282);
            this.upPressures.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.upPressures.Name = "upPressure s";
            this.upPressures.ReadOnly  = false;
            this.upPressures.RowHeadersVisible = false;
            this.upPressures.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("upPressure s.RowNames")));
            this.upPressures.Size = new  System.Drawing.Size(195, 151);
            this.upPressures.TabIndex = 14;
            this.upPressures.TopText = "Pressure s";
            // 
            // SpecificationSheet
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.upPressures);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CBOX1);
            this.Controls.Add(this.condenserTypeControl1);
            this.Controls.Add(this.SpecGroupBox);
            this.Controls.Add(this.DelSpec);
            this.Controls.Add(this.AddSpec);
            this.Controls.Add(this.DGV);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "SpecificationSheet";
            this.Size = new  System.Drawing.Size(904, 499);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.SpecGroupBox.ResumeLayout(false);
            this.SpecGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private  DataGridView DGV;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private  Button AddSpec;
        private  Button DelSpec;
        public  TextBox txtDegOfFreedom;
        private  Label DegOfFreedom;
        private  RadioButton rbLiquidFlow;
        private  GroupBox SpecGroupBox;
        private  RadioButton rbVapourDrawRate;
        private  RadioButton rbLiquidDrawRate;
        private  RadioButton rbRefluxRatio;
        private  RadioButton rbRefluxRate;
        private  RadioButton rbStageTemperature ;
        private  RadioButton rbVapourFlow;
        private  CondenserTypeControl condenserTypeControl1;
        private  RadioButton rbPARetT;
        private  RadioButton rbPADuty;
        private  RadioButton rbPADeltaT;
        private  RadioButton rbPAFlow;
        private  Label label1;
        public  TextBox txtActSpecs;
        public  TextBox txtReqiredSpecs;
        private  Label label2;
        private  RadioButton rbVapStreamRate;
        private  RadioButton rbLiquidStreamRate;
        private  ComboBox CBOX1;
        private  RadioButton rdDistSpec;
        private  GroupBox groupBox1;
        private  DataGridViewTextBoxColumn SpecName;
        private  DataGridViewTextBoxColumn Column2;
        private  DataGridViewTextBoxColumn Column3;
        private  DataGridViewCheckBoxColumn Active;
        private  DataGridViewButtonColumn Edit;
        private  DataGridViewTextBoxColumn SpecGuid;
        private  FormControls.UserPropGrid upPressures;
    }
}
