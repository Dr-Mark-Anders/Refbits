namespace   Units.PortForm
{
    partial class  PortPropertyWorksheet
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new  System.Windows.Forms.DataGridViewCellStyle();
            this.DGV = new  System.Windows.Forms.DataGridView();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  FormControls.DataGridViewUOMColumn();
            this.Column3 = new  FormControls.DataGridViewUOMColumn();
            this.Water = new  FormControls.DataGridViewUOMColumn();
            this.Options = new  System.Windows.Forms.GroupBox();
            this.btnCopyFrom = new  System.Windows.Forms.Button();
            this.btnDistData = new  System.Windows.Forms.Button();
            this.btnTBPPlot = new  System.Windows.Forms.Button();
            this.btnPropPlot = new  System.Windows.Forms.Button();
            this.btnAqueous = new  System.Windows.Forms.Button();
            this.btnComposition = new  System.Windows.Forms.Button();
            this.groupBoxStream = new  System.Windows.Forms.GroupBox();
            this.CBOX1 = new  System.Windows.Forms.ComboBox();
            this.groupBoxProps = new  System.Windows.Forms.GroupBox();
            this.DGV2 = new  System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Options2 = new  System.Windows.Forms.GroupBox();
            this.rbGasProps = new  System.Windows.Forms.RadioButton();
            this.rbComposition = new  System.Windows.Forms.RadioButton();
            this.rbPONA = new  System.Windows.Forms.RadioButton();
            this.rbColdProps = new  System.Windows.Forms.RadioButton();
            this.rbRefinery = new  System.Windows.Forms.RadioButton();
            this.rbDefault = new  System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.Options.SuspendLayout();
            this.groupBoxStream.SuspendLayout();
            this.groupBoxProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV2)).BeginInit();
            this.Options2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToResizeColumns = false;
            this.DGV.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.DGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DGV.CausesValidation = false;
            this.DGV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Water});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle2.Font = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.DGV.Location = new  System.Drawing.Point (18, 22);
            this.DGV.Margin = new  System.Windows.Forms.Padding(2);
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersVisible = false;
            this.DGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV.RowTemplate.Height = 28;
            this.DGV.Size = new  System.Drawing.Size(370, 309);
            this.DGV.TabIndex = 4;
            this.DGV.CellDoubleClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CelldoubleClick);
            this.DGV.CellEndEdit += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEndEdit);
            this.DGV.CellFormatting += new  System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            this.DGV.CellMouseClick += new  System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseClick);
            this.DGV.CellMouseDown += new  System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseDown);
            this.DGV.KeyDown += new  System.Windows.Forms.KeyEventHandler(this.DGV_KeyDown);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column1.HeaderText = "Property";
            this.Column1.MinimumWidth = 80;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly  = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 59.77011F;
            this.Column2.HeaderText = "Stream";
            this.Column2.MinimumWidth = 60;
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 59.77011F;
            this.Column3.HeaderText = "Liquid";
            this.Column3.MinimumWidth = 60;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly  = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Water
            // 
            this.Water.FillWeight = 59.77011F;
            this.Water.HeaderText = "Vapour";
            this.Water.MinimumWidth = 60;
            this.Water.Name = "Water";
            this.Water.ReadOnly  = true;
            this.Water.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Options
            // 
            this.Options.Controls.Add(this.btnCopyFrom);
            this.Options.Controls.Add(this.btnDistData);
            this.Options.Controls.Add(this.btnTBPPlot);
            this.Options.Controls.Add(this.btnPropPlot);
            this.Options.Controls.Add(this.btnAqueous);
            this.Options.Controls.Add(this.btnComposition);
            this.Options.Location = new  System.Drawing.Point (14, 14);
            this.Options.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Options.Name = "Options";
            this.Options.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Options.Size = new  System.Drawing.Size(414, 97);
            this.Options.TabIndex = 5;
            this.Options.TabStop = false;
            this.Options.Text = "Options";
            // 
            // btnCopyFrom
            // 
            this.btnCopyFrom.Location = new  System.Drawing.Point (222, 60);
            this.btnCopyFrom.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCopyFrom.Name = "btnCopyFrom";
            this.btnCopyFrom.Size = new  System.Drawing.Size(103, 30);
            this.btnCopyFrom.TabIndex = 16;
            this.btnCopyFrom.Text = "Copy From...";
            this.btnCopyFrom.UseVisualStyleBackColor = true;
            this.btnCopyFrom.Click += new  System.EventHandler(this.CopyStream_Click);
            // 
            // btnDistData
            // 
            this.btnDistData.Location = new  System.Drawing.Point (222, 27);
            this.btnDistData.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDistData.Name = "btnDistData";
            this.btnDistData.Size = new  System.Drawing.Size(103, 30);
            this.btnDistData.TabIndex = 18;
            this.btnDistData.Text = "Distillation Data";
            this.btnDistData.UseVisualStyleBackColor = true;
            this.btnDistData.Click += new  System.EventHandler(this.Button4_Click);
            // 
            // btnTBPPlot
            // 
            this.btnTBPPlot.Location = new  System.Drawing.Point (112, 61);
            this.btnTBPPlot.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnTBPPlot.Name = "btnTBPPlot";
            this.btnTBPPlot.Size = new  System.Drawing.Size(103, 30);
            this.btnTBPPlot.TabIndex = 17;
            this.btnTBPPlot.Text = "TBP Plots";
            this.btnTBPPlot.UseVisualStyleBackColor = true;
            this.btnTBPPlot.Click += new  System.EventHandler(this.button3_Click);
            // 
            // btnPropPlot
            // 
            this.btnPropPlot.Location = new  System.Drawing.Point (112, 27);
            this.btnPropPlot.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPropPlot.Name = "btnPropPlot";
            this.btnPropPlot.Size = new  System.Drawing.Size(103, 30);
            this.btnPropPlot.TabIndex = 15;
            this.btnPropPlot.Text = "Property Plots";
            this.btnPropPlot.UseVisualStyleBackColor = true;
            this.btnPropPlot.Click += new  System.EventHandler(this.button5_Click);
            // 
            // btnAqueous
            // 
            this.btnAqueous.Location = new  System.Drawing.Point (18, 63);
            this.btnAqueous.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAqueous.Name = "btnAqueous";
            this.btnAqueous.Size = new  System.Drawing.Size(88, 27);
            this.btnAqueous.TabIndex = 1;
            this.btnAqueous.Text = "Aqueous";
            this.btnAqueous.UseVisualStyleBackColor = true;
            // 
            // btnComposition
            // 
            this.btnComposition.Location = new  System.Drawing.Point (18, 27);
            this.btnComposition.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnComposition.Name = "btnComposition";
            this.btnComposition.Size = new  System.Drawing.Size(88, 30);
            this.btnComposition.TabIndex = 0;
            this.btnComposition.Text = "Composition";
            this.btnComposition.UseVisualStyleBackColor = true;
            this.btnComposition.Click += new  System.EventHandler(this.btnComposition_Click);
            // 
            // groupBoxStream
            // 
            this.groupBoxStream.Controls.Add(this.CBOX1);
            this.groupBoxStream.Controls.Add(this.DGV);
            this.groupBoxStream.Location = new  System.Drawing.Point (14, 118);
            this.groupBoxStream.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxStream.Name = "groupBoxStream";
            this.groupBoxStream.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxStream.Size = new  System.Drawing.Size(414, 345);
            this.groupBoxStream.TabIndex = 6;
            this.groupBoxStream.TabStop = false;
            this.groupBoxStream.Text = "Conditions and Flows";
            // 
            // CBOX1
            // 
            this.CBOX1.FormattingEnabled = true;
            this.CBOX1.Location = new  System.Drawing.Point (64, 75);
            this.CBOX1.Margin = new  System.Windows.Forms.Padding(2);
            this.CBOX1.Name = "CBOX1";
            this.CBOX1.Size = new  System.Drawing.Size(126, 23);
            this.CBOX1.TabIndex = 5;
            this.CBOX1.Visible = false;
            this.CBOX1.SelectionChangeCommitted += new  System.EventHandler(this.CBOX1_SelectionChangeCommitted);
            // 
            // groupBoxProps
            // 
            this.groupBoxProps.Controls.Add(this.DGV2);
            this.groupBoxProps.Location = new  System.Drawing.Point (472, 119);
            this.groupBoxProps.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxProps.Name = "groupBoxProps";
            this.groupBoxProps.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxProps.Size = new  System.Drawing.Size(414, 345);
            this.groupBoxProps.TabIndex = 7;
            this.groupBoxProps.TabStop = false;
            this.groupBoxProps.Text = "Properties";
            // 
            // DGV2
            // 
            this.DGV2.AllowUserToAddRows = false;
            this.DGV2.AllowUserToDeleteRows = false;
            this.DGV2.AllowUserToResizeColumns = false;
            this.DGV2.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.DGV2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.DGV2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DGV2.CausesValidation = false;
            this.DGV2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.DGV2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV2.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle4.Font = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV2.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV2.Location = new  System.Drawing.Point (18, 22);
            this.DGV2.Margin = new  System.Windows.Forms.Padding(2);
            this.DGV2.Name = "DGV2";
            this.DGV2.RowHeadersVisible = false;
            this.DGV2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV2.RowTemplate.Height = 28;
            this.DGV2.Size = new  System.Drawing.Size(370, 308);
            this.DGV2.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn5.HeaderText = "Property";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 80;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly  = true;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Width = 80;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Stream";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Liquid";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly  = true;
            this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Vapour";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly  = true;
            // 
            // Options2
            // 
            this.Options2.Controls.Add(this.rbGasProps);
            this.Options2.Controls.Add(this.rbComposition);
            this.Options2.Controls.Add(this.rbPONA);
            this.Options2.Controls.Add(this.rbColdProps);
            this.Options2.Controls.Add(this.rbRefinery);
            this.Options2.Controls.Add(this.rbDefault);
            this.Options2.Location = new  System.Drawing.Point (472, 15);
            this.Options2.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Options2.Name = "Options2";
            this.Options2.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Options2.Size = new  System.Drawing.Size(414, 97);
            this.Options2.TabIndex = 8;
            this.Options2.TabStop = false;
            this.Options2.Text = "Options";
            // 
            // rbGasProps
            // 
            this.rbGasProps.AutoSize = true;
            this.rbGasProps.Location = new  System.Drawing.Point (270, 52);
            this.rbGasProps.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbGasProps.Name = "rbGasProps";
            this.rbGasProps.Size = new  System.Drawing.Size(77, 19);
            this.rbGasProps.TabIndex = 19;
            this.rbGasProps.TabStop = true;
            this.rbGasProps.Text = "Gas Props";
            this.rbGasProps.UseVisualStyleBackColor = true;
            // 
            // rbComposition
            // 
            this.rbComposition.AutoSize = true;
            this.rbComposition.Location = new  System.Drawing.Point (270, 25);
            this.rbComposition.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbComposition.Name = "rbComposition";
            this.rbComposition.Size = new  System.Drawing.Size(94, 19);
            this.rbComposition.TabIndex = 18;
            this.rbComposition.TabStop = true;
            this.rbComposition.Text = "Composition";
            this.rbComposition.UseVisualStyleBackColor = true;
            // 
            // rbPONA
            // 
            this.rbPONA.AutoSize = true;
            this.rbPONA.Location = new  System.Drawing.Point (163, 52);
            this.rbPONA.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbPONA.Name = "rbPONA";
            this.rbPONA.Size = new  System.Drawing.Size(58, 19);
            this.rbPONA.TabIndex = 17;
            this.rbPONA.TabStop = true;
            this.rbPONA.Text = "PONA";
            this.rbPONA.UseVisualStyleBackColor = true;
            // 
            // rbColdProps
            // 
            this.rbColdProps.AutoSize = true;
            this.rbColdProps.Location = new  System.Drawing.Point (163, 25);
            this.rbColdProps.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbColdProps.Name = "rbColdProps";
            this.rbColdProps.Size = new  System.Drawing.Size(83, 19);
            this.rbColdProps.TabIndex = 16;
            this.rbColdProps.TabStop = true;
            this.rbColdProps.Text = "Cold Props";
            this.rbColdProps.UseVisualStyleBackColor = true;
            // 
            // rbRefinery
            // 
            this.rbRefinery.AutoSize = true;
            this.rbRefinery.Location = new  System.Drawing.Point (46, 52);
            this.rbRefinery.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbRefinery.Name = "rbRefinery";
            this.rbRefinery.Size = new  System.Drawing.Size(68, 19);
            this.rbRefinery.TabIndex = 15;
            this.rbRefinery.TabStop = true;
            this.rbRefinery.Text = "Refinery";
            this.rbRefinery.UseVisualStyleBackColor = true;
            // 
            // rbDefault
            // 
            this.rbDefault.AutoSize = true;
            this.rbDefault.Checked = true;
            this.rbDefault.Location = new  System.Drawing.Point (46, 25);
            this.rbDefault.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbDefault.Name = "rbDefault";
            this.rbDefault.Size = new  System.Drawing.Size(63, 19);
            this.rbDefault.TabIndex = 14;
            this.rbDefault.TabStop = true;
            this.rbDefault.Text = "Default";
            this.rbDefault.UseVisualStyleBackColor = true;
            // 
            // PortPropertyWorksheet
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Options2);
            this.Controls.Add(this.groupBoxProps);
            this.Controls.Add(this.Options);
            this.Controls.Add(this.groupBoxStream);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "PortPropertyWorksheet";
            this.Size = new  System.Drawing.Size(901, 477);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.Options.ResumeLayout(false);
            this.groupBoxStream.ResumeLayout(false);
            this.groupBoxProps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV2)).EndInit();
            this.Options2.ResumeLayout(false);
            this.Options2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.GroupBox Options;
        private  System.Windows.Forms.GroupBox groupBoxStream;
        private  System.Windows.Forms.GroupBox groupBoxProps;
        private  System.Windows.Forms.DataGridView DGV2;
        private  System.Windows.Forms.GroupBox Options2;
        private  System.Windows.Forms.RadioButton rbGasProps;
        private  System.Windows.Forms.RadioButton rbComposition;
        private  System.Windows.Forms.RadioButton rbPONA;
        private  System.Windows.Forms.RadioButton rbColdProps;
        private  System.Windows.Forms.RadioButton rbRefinery;
        private  System.Windows.Forms.RadioButton rbDefault;
        private  System.Windows.Forms.Button btnComposition;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private  System.Windows.Forms.Button btnAqueous;
        public  System.Windows.Forms.DataGridView DGV;
        private  System.Windows.Forms.ComboBox CBOX1;
        private  System.Windows.Forms.Button btnCopyFrom;
        private  System.Windows.Forms.Button btnDistData;
        private  System.Windows.Forms.Button btnTBPPlot;
        private  System.Windows.Forms.Button btnPropPlot;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private  FormControls.DataGridViewUOMColumn Column2;
        private  FormControls.DataGridViewUOMColumn Column3;
        private  FormControls.DataGridViewUOMColumn Water;
    }
}

