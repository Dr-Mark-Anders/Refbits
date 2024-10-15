namespace   Units.PortForm
{
    partial class  PortsPropertyWorksheet
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new  System.Windows.Forms.DataGridViewCellStyle();
            this.DGV = new  System.Windows.Forms.DataGridView();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Options = new  System.Windows.Forms.GroupBox();
            this.button1 = new  System.Windows.Forms.Button();
            this.btnComposition = new  System.Windows.Forms.Button();
            this.groupBoxStream = new  System.Windows.Forms.GroupBox();
            this.CBOX1 = new  System.Windows.Forms.ComboBox();
            this.Options2 = new  System.Windows.Forms.GroupBox();
            this.radioButton6 = new  System.Windows.Forms.RadioButton();
            this.radioButton5 = new  System.Windows.Forms.RadioButton();
            this.radioButton4 = new  System.Windows.Forms.RadioButton();
            this.radioButton3 = new  System.Windows.Forms.RadioButton();
            this.radioButton2 = new  System.Windows.Forms.RadioButton();
            this.radioButton1 = new  System.Windows.Forms.RadioButton();
            this.dataGridViewTextBoxColumn1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.Options.SuspendLayout();
            this.groupBoxStream.SuspendLayout();
            this.Options2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToResizeColumns = false;
            this.DGV.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.DGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.DGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DGV.CausesValidation = false;
            this.DGV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle4.Font = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV.Location = new  System.Drawing.Point (18, 22);
            this.DGV.Margin = new  System.Windows.Forms.Padding(2);
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersVisible = false;
            this.DGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV.RowTemplate.Height = 28;
            this.DGV.Size = new  System.Drawing.Size(835, 309);
            this.DGV.TabIndex = 4;
            this.DGV.CellDoubleClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CelldoubleClick);
            this.DGV.CellEndEdit += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEndEdit);
            this.DGV.CellFormatting += new  System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            this.DGV.CellMouseClick += new  System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseClick);
            this.DGV.KeyDown += new  System.Windows.Forms.KeyEventHandler(this.DGV_KeyDown);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "Property";
            this.Column1.MinimumWidth = 80;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly  = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.FillWeight = 59.77011F;
            this.Column2.HeaderText = "Stream";
            this.Column2.MinimumWidth = 60;
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Options
            // 
            this.Options.Controls.Add(this.button1);
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
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (18, 63);
            this.button1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(88, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "Aqueous";
            this.button1.UseVisualStyleBackColor = true;
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
            this.groupBoxStream.Size = new  System.Drawing.Size(873, 345);
            this.groupBoxStream.TabIndex = 6;
            this.groupBoxStream.TabStop = false;
            this.groupBoxStream.Text = "Conditons and Flows";
            // 
            // CBOX1
            // 
            this.CBOX1.FormattingEnabled = true;
            this.CBOX1.Location = new  System.Drawing.Point (73, 71);
            this.CBOX1.Name = "CBOX1";
            this.CBOX1.Size = new  System.Drawing.Size(121, 23);
            this.CBOX1.TabIndex = 9;
            this.CBOX1.Visible = false;
            this.CBOX1.SelectionChangeCommitted += new  System.EventHandler(this.CBOX1_SelectionChangeCommitted);
            // 
            // Options2
            // 
            this.Options2.Controls.Add(this.radioButton6);
            this.Options2.Controls.Add(this.radioButton5);
            this.Options2.Controls.Add(this.radioButton4);
            this.Options2.Controls.Add(this.radioButton3);
            this.Options2.Controls.Add(this.radioButton2);
            this.Options2.Controls.Add(this.radioButton1);
            this.Options2.Location = new  System.Drawing.Point (472, 15);
            this.Options2.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Options2.Name = "Options2";
            this.Options2.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Options2.Size = new  System.Drawing.Size(414, 97);
            this.Options2.TabIndex = 8;
            this.Options2.TabStop = false;
            this.Options2.Text = "Options";
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new  System.Drawing.Point (270, 52);
            this.radioButton6.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new  System.Drawing.Size(77, 19);
            this.radioButton6.TabIndex = 19;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "Gas Props";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new  System.Drawing.Point (270, 25);
            this.radioButton5.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new  System.Drawing.Size(94, 19);
            this.radioButton5.TabIndex = 18;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Composition";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new  System.Drawing.Point (163, 52);
            this.radioButton4.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new  System.Drawing.Size(58, 19);
            this.radioButton4.TabIndex = 17;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "PONA";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new  System.Drawing.Point (163, 25);
            this.radioButton3.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new  System.Drawing.Size(83, 19);
            this.radioButton3.TabIndex = 16;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Cold Props";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new  System.Drawing.Point (46, 52);
            this.radioButton2.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new  System.Drawing.Size(68, 19);
            this.radioButton2.TabIndex = 15;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Refinery";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new  System.Drawing.Point (46, 25);
            this.radioButton1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new  System.Drawing.Size(63, 19);
            this.radioButton1.TabIndex = 14;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Default";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "Property";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 80;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly  = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.FillWeight = 59.77011F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Stream";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.FillWeight = 59.77011F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Liquid";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly  = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.FillWeight = 59.77011F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Vapour";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly  = true;
            // 
            // PortsPropertyWorksheet
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Options2);
            this.Controls.Add(this.Options);
            this.Controls.Add(this.groupBoxStream);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "PortsPropertyWorksheet";
            this.Size = new  System.Drawing.Size(901, 477);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.Options.ResumeLayout(false);
            this.groupBoxStream.ResumeLayout(false);
            this.Options2.ResumeLayout(false);
            this.Options2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.GroupBox Options;
        private  System.Windows.Forms.GroupBox groupBoxStream;
        private  System.Windows.Forms.GroupBox Options2;
        private  System.Windows.Forms.RadioButton radioButton6;
        private  System.Windows.Forms.RadioButton radioButton5;
        private  System.Windows.Forms.RadioButton radioButton4;
        private  System.Windows.Forms.RadioButton radioButton3;
        private  System.Windows.Forms.RadioButton radioButton2;
        private  System.Windows.Forms.RadioButton radioButton1;
        private  System.Windows.Forms.Button btnComposition;
        private  System.Windows.Forms.Button button1;
        public  System.Windows.Forms.DataGridView DGV;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private  System.Windows.Forms.ComboBox CBOX1;
    }
}

