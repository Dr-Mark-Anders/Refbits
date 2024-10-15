using   System.Windows.Forms;

namespace   Units.DrawingObjectDialogs
{
    partial class  HeatExIODialog
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
            this.groupBox1 = new  GroupBox();
            this.dbShellSideDP = new  Units.DataBoundDimTextBox();
            this.dbTubeSideDP = new  Units.DataBoundDimTextBox();
            this.groupBox2 = new  GroupBox();
            this.dbShellSideDT = new  Units.DataBoundDimTextBox();
            this.dbTubeSideDT = new  Units.DataBoundDimTextBox();
            this.dbSSOutletT = new  Units.DataBoundDimTextBox();
            this.dbTSOutletT = new  Units.DataBoundDimTextBox();
            this.dbSSInletT = new  Units.DataBoundDimTextBox();
            this.dbTSInletT = new  Units.DataBoundDimTextBox();
            this.dGV1 = new  DataGridView();
            this.Constraint  = new  DataGridViewTextBoxColumn();
            this.Active = new  DataGridViewCheckBoxColumn();
            this.label1 = new  Label();
            this.tbDegreesOfFreedom = new  TextBox();
            this.dataGridViewTextBoxColumn1 = new  DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new  DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dbShellSideDP);
            this.groupBox1.Controls.Add(this.dbTubeSideDP);
            this.groupBox1.Location = new  System.Drawing.Point (472, 318);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new  System.Drawing.Size(332, 127);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pressure s";
            // 
            // dbShellSideDP
            // 
            this.dbShellSideDP.BackColor = System.Drawing.SystemColors.Control;
            this.dbShellSideDP.DescriptionText = "Shell Side DP";
            this.dbShellSideDP.DescriptionWidth = 94;
            this.dbShellSideDP.DropDownVisible = true;
            this.dbShellSideDP.EditBoxLeft = 150;
            this.dbShellSideDP.EditBoxReadOnly  = true;
            this.dbShellSideDP.EditBoxWidth = 60;
            this.dbShellSideDP.Location = new  System.Drawing.Point (16, 62);
            this.dbShellSideDP.Name = "dbShellSideDP";
            this.dbShellSideDP.Origin = SourceEnum.Empty;
            this.dbShellSideDP.ReadOnly  = true;
            this.dbShellSideDP.Size = new  System.Drawing.Size(258, 30);
            this.dbShellSideDP.TabIndex = 13;
            this.dbShellSideDP.Textbxwidth = 60;
            this.dbShellSideDP.TextLeft = 150;
            this.dbShellSideDP.TextWidth = 60;
            this.dbShellSideDP.UnitList = "";
            this.dbShellSideDP.Value = 0D;
            // 
            // dbTubeSideDP
            // 
            this.dbTubeSideDP.BackColor = System.Drawing.SystemColors.Control;
            this.dbTubeSideDP.DescriptionText = "Tube Side DP";
            this.dbTubeSideDP.DescriptionWidth = 96;
            this.dbTubeSideDP.DropDownVisible = true;
            this.dbTubeSideDP.EditBoxLeft = 150;
            this.dbTubeSideDP.EditBoxReadOnly  = true;
            this.dbTubeSideDP.EditBoxWidth = 60;
            this.dbTubeSideDP.Location = new  System.Drawing.Point (16, 30);
            this.dbTubeSideDP.Name = "dbTubeSideDP";
            this.dbTubeSideDP.Origin = SourceEnum.Empty;
            this.dbTubeSideDP.ReadOnly  = true;
            this.dbTubeSideDP.Size = new  System.Drawing.Size(258, 32);
            this.dbTubeSideDP.TabIndex = 12;
            this.dbTubeSideDP.Textbxwidth = 60;
            this.dbTubeSideDP.TextLeft = 150;
            this.dbTubeSideDP.TextWidth = 60;
            this.dbTubeSideDP.UnitList = "";
            this.dbTubeSideDP.Value = 0D;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dbShellSideDT);
            this.groupBox2.Controls.Add(this.dbTubeSideDT);
            this.groupBox2.Controls.Add(this.dbSSOutletT);
            this.groupBox2.Controls.Add(this.dbTSOutletT);
            this.groupBox2.Controls.Add(this.dbSSInletT);
            this.groupBox2.Controls.Add(this.dbTSInletT);
            this.groupBox2.Location = new  System.Drawing.Point (472, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new  System.Drawing.Size(332, 272);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Temperature s";
            // 
            // dbShellSideDT
            // 
            this.dbShellSideDT.BackColor = System.Drawing.SystemColors.Control;
            this.dbShellSideDT.DescriptionText = "Shell Side DT";
            this.dbShellSideDT.DescriptionWidth = 94;
            this.dbShellSideDT.DropDownVisible = true;
            this.dbShellSideDT.EditBoxLeft = 150;
            this.dbShellSideDT.EditBoxReadOnly  = true;
            this.dbShellSideDT.EditBoxWidth = 60;
            this.dbShellSideDT.Location = new  System.Drawing.Point (27, 222);
            this.dbShellSideDT.Name = "dbShellSideDT";
            this.dbShellSideDT.Origin = SourceEnum.Empty;
            this.dbShellSideDT.ReadOnly  = true;
            this.dbShellSideDT.Size = new  System.Drawing.Size(276, 30);
            this.dbShellSideDT.TabIndex = 15;
            this.dbShellSideDT.Textbxwidth = 60;
            this.dbShellSideDT.TextLeft = 150;
            this.dbShellSideDT.TextWidth = 60;
            this.dbShellSideDT.UnitList = "";
            this.dbShellSideDT.Value = 0D;
            // 
            // dbTubeSideDT
            // 
            this.dbTubeSideDT.BackColor = System.Drawing.SystemColors.Control;
            this.dbTubeSideDT.DescriptionText = "Tube Side DT";
            this.dbTubeSideDT.DescriptionWidth = 96;
            this.dbTubeSideDT.DropDownVisible = true;
            this.dbTubeSideDT.EditBoxLeft = 150;
            this.dbTubeSideDT.EditBoxReadOnly  = true;
            this.dbTubeSideDT.EditBoxWidth = 60;
            this.dbTubeSideDT.Location = new  System.Drawing.Point (27, 189);
            this.dbTubeSideDT.Name = "dbTubeSideDT";
            this.dbTubeSideDT.Origin = SourceEnum.Empty;
            this.dbTubeSideDT.ReadOnly  = true;
            this.dbTubeSideDT.Size = new  System.Drawing.Size(276, 32);
            this.dbTubeSideDT.TabIndex = 14;
            this.dbTubeSideDT.Textbxwidth = 60;
            this.dbTubeSideDT.TextLeft = 150;
            this.dbTubeSideDT.TextWidth = 60;
            this.dbTubeSideDT.UnitList = "";
            this.dbTubeSideDT.Value = 0D;
            // 
            // dbSSOutletT
            // 
            this.dbSSOutletT.BackColor = System.Drawing.SystemColors.Control;
            this.dbSSOutletT.DescriptionText = "Shell Side Outlet T";
            this.dbSSOutletT.DescriptionWidth = 126;
            this.dbSSOutletT.DropDownVisible = true;
            this.dbSSOutletT.EditBoxLeft = 150;
            this.dbSSOutletT.EditBoxReadOnly  = true;
            this.dbSSOutletT.EditBoxWidth = 60;
            this.dbSSOutletT.Location = new  System.Drawing.Point (27, 142);
            this.dbSSOutletT.Name = "dbSSOutletT";
            this.dbSSOutletT.Origin = SourceEnum.Empty;
            this.dbSSOutletT.ReadOnly  = true;
            this.dbSSOutletT.Size = new  System.Drawing.Size(276, 32);
            this.dbSSOutletT.TabIndex = 13;
            this.dbSSOutletT.Textbxwidth = 60;
            this.dbSSOutletT.TextLeft = 150;
            this.dbSSOutletT.TextWidth = 60;
            this.dbSSOutletT.UnitList = "";
            this.dbSSOutletT.Value = 0D;
            // 
            // dbTSOutletT
            // 
            this.dbTSOutletT.BackColor = System.Drawing.SystemColors.Control;
            this.dbTSOutletT.DescriptionText = "Tube Side Outlet T";
            this.dbTSOutletT.DescriptionWidth = 128;
            this.dbTSOutletT.DropDownVisible = true;
            this.dbTSOutletT.EditBoxLeft = 150;
            this.dbTSOutletT.EditBoxReadOnly  = true;
            this.dbTSOutletT.EditBoxWidth = 60;
            this.dbTSOutletT.Location = new  System.Drawing.Point (27, 104);
            this.dbTSOutletT.Name = "dbTSOutletT";
            this.dbTSOutletT.Origin = SourceEnum.Empty;
            this.dbTSOutletT.ReadOnly  = true;
            this.dbTSOutletT.Size = new  System.Drawing.Size(276, 32);
            this.dbTSOutletT.TabIndex = 12;
            this.dbTSOutletT.Textbxwidth = 60;
            this.dbTSOutletT.TextLeft = 150;
            this.dbTSOutletT.TextWidth = 60;
            this.dbTSOutletT.UnitList = "";
            this.dbTSOutletT.Value = 0D;
            // 
            // dbSSInletT
            // 
            this.dbSSInletT.BackColor = System.Drawing.SystemColors.Control;
            this.dbSSInletT.DescriptionText = "Shell Side Inlet T";
            this.dbSSInletT.DescriptionWidth = 114;
            this.dbSSInletT.DropDownVisible = true;
            this.dbSSInletT.EditBoxLeft = 150;
            this.dbSSInletT.EditBoxReadOnly  = true;
            this.dbSSInletT.EditBoxWidth = 60;
            this.dbSSInletT.Location = new  System.Drawing.Point (27, 69);
            this.dbSSInletT.Name = "dbSSInletT";
            this.dbSSInletT.Origin = SourceEnum.Empty;
            this.dbSSInletT.ReadOnly  = true;
            this.dbSSInletT.Size = new  System.Drawing.Size(276, 30);
            this.dbSSInletT.TabIndex = 11;
            this.dbSSInletT.Textbxwidth = 60;
            this.dbSSInletT.TextLeft = 150;
            this.dbSSInletT.TextWidth = 60;
            this.dbSSInletT.UnitList = "";
            this.dbSSInletT.Value = 0D;
            // 
            // dbTSInletT
            // 
            this.dbTSInletT.BackColor = System.Drawing.SystemColors.Control;
            this.dbTSInletT.DescriptionText = "Tube Side Inlet T";
            this.dbTSInletT.DescriptionWidth = 116;
            this.dbTSInletT.DropDownVisible = true;
            this.dbTSInletT.EditBoxLeft = 150;
            this.dbTSInletT.EditBoxReadOnly  = true;
            this.dbTSInletT.EditBoxWidth = 60;
            this.dbTSInletT.Location = new  System.Drawing.Point (27, 37);
            this.dbTSInletT.Name = "dbTSInletT";
            this.dbTSInletT.Origin = SourceEnum.Empty;
            this.dbTSInletT.ReadOnly  = true;
            this.dbTSInletT.Size = new  System.Drawing.Size(276, 32);
            this.dbTSInletT.TabIndex = 10;
            this.dbTSInletT.Textbxwidth = 60;
            this.dbTSInletT.TextLeft = 150;
            this.dbTSInletT.TextWidth = 60;
            this.dbTSInletT.UnitList = "";
            this.dbTSInletT.Value = 0D;
            // 
            // dGV1
            // 
            this.dGV1.AllowUserToAddRows = false;
            this.dGV1.AllowUserToDeleteRows = false;
            this.dGV1.AllowUserToResizeRows = false;
            this.dGV1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV1.Location = new  System.Drawing.Point (65, 35);
            this.dGV1.Name = "dGV1";
            this.dGV1.RowHeadersVisible = false;
            this.dGV1.RowTemplate.Height = 24;
            this.dGV1.Size = new  System.Drawing.Size(382, 221);
            this.dGV1.TabIndex = 18;
            this.dGV1.CellFormatting += new  DataGridViewCellFormattingEventHandler(this.dGV1_CellFormatting);
            this.dGV1.CurrentCellDirtyStateChanged += new  System.EventHandler(this.dGV1_CurrentCellDirtyStateChanged);
            // 
            // Constraint 
            // 
            this.Constraint .HeaderText = "Constraint ";
            this.Constraint .Name = "Constraint ";
            // 
            // Active
            // 
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            this.Active.Resizable = DataGridViewTriState.True;
            this.Active.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new  System.Drawing.Point (74, 289);
            this.label1.Name = "label1";
            this.label1.Size = new  System.Drawing.Size(138, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "Degrees of Freedom";
            // 
            // tbDegreesOfFreedom
            // 
            this.tbDegreesOfFreedom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbDegreesOfFreedom.Location = new  System.Drawing.Point (218, 289);
            this.tbDegreesOfFreedom.Name = "tbDegreesOfFreedom";
            this.tbDegreesOfFreedom.ReadOnly  = true;
            this.tbDegreesOfFreedom.Size = new  System.Drawing.Size(68, 22);
            this.tbDegreesOfFreedom.TabIndex = 20;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Constraint ";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly  = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly  = true;
            // 
            // HeatExIODialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(889, 476);
            this.Controls.Add(this.tbDegreesOfFreedom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dGV1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "HeatExIODialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "HeatExDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private  DataBoundDimTextBox dbTSInletT;
        private  DataBoundDimTextBox dbSSInletT;
        private  DataBoundDimTextBox dbShellSideDP;
        private  DataBoundDimTextBox dbTubeSideDP;
        private  GroupBox groupBox1;
        private  GroupBox groupBox2;
        private  DataBoundDimTextBox dbShellSideDT;
        private  DataBoundDimTextBox dbTubeSideDT;
        private  DataBoundDimTextBox dbSSOutletT;
        private  DataBoundDimTextBox dbTSOutletT;
        private  DataGridView dGV1;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  Label label1;
        private  TextBox tbDegreesOfFreedom;
        private  DataGridViewTextBoxColumn Constraint ;
        private  DataGridViewCheckBoxColumn Active;

    }
}