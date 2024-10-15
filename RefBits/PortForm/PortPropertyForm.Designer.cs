namespace   Units.PortForm
{
    partial class  PortPropertyForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new  System.Windows.Forms.DataGridViewCellStyle();
            this.CBOX1 = new  System.Windows.Forms.ComboBox();
            this.DGV = new  System.Windows.Forms.DataGridView();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Water = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Options = new  System.Windows.Forms.GroupBox();
            this.button6 = new  System.Windows.Forms.Button();
            this.button4 = new  System.Windows.Forms.Button();
            this.button3 = new  System.Windows.Forms.Button();
            this.button2 = new  System.Windows.Forms.Button();
            this.button1 = new  System.Windows.Forms.Button();
            this.btnComposition = new  System.Windows.Forms.Button();
            this.FlashButton = new  System.Windows.Forms.Button();
            this.groupBoxStream = new  System.Windows.Forms.GroupBox();
            this.button5 = new  System.Windows.Forms.Button();
            this.groupBoxProps = new  System.Windows.Forms.GroupBox();
            this.dataGridProperties = new  System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.groupBoxProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProperties)).BeginInit();
            this.Options2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBOX1
            // 
            this.CBOX1.FormattingEnabled = true;
            this.CBOX1.Location = new  System.Drawing.Point (50, 160);
            this.CBOX1.Margin = new  System.Windows.Forms.Padding(2);
            this.CBOX1.Name = "CBOX1";
            this.CBOX1.Size = new  System.Drawing.Size(109, 21);
            this.CBOX1.TabIndex = 2;
            this.CBOX1.Visible = false;
            this.CBOX1.SelectionChangeCommitted += new  System.EventHandler(this.CBOX1_SelectionChangeCommitted);
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToResizeColumns = false;
            this.DGV.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.DGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
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
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle6.Font = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.DefaultCellStyle = dataGridViewCellStyle6;
            this.DGV.Location = new  System.Drawing.Point (15, 19);
            this.DGV.Margin = new  System.Windows.Forms.Padding(2);
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersVisible = false;
            this.DGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV.RowTemplate.Height = 28;
            this.DGV.Size = new  System.Drawing.Size(317, 268);
            this.DGV.TabIndex = 4;
            this.DGV.CellClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellClick);
            this.DGV.Celldouble Click += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Celldouble Click);
            this.DGV.CellFormatting += new  System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            this.DGV.CellLeave += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellLeave);
            this.DGV.CellValueChanged += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellValueChanged);
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
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Water
            // 
            this.Water.FillWeight = 59.77011F;
            this.Water.HeaderText = "Vapour";
            this.Water.MinimumWidth = 60;
            this.Water.Name = "Water";
            this.Water.ReadOnly  = true;
            // 
            // Options
            // 
            this.Options.Controls.Add(this.button6);
            this.Options.Controls.Add(this.button4);
            this.Options.Controls.Add(this.button3);
            this.Options.Controls.Add(this.button2);
            this.Options.Controls.Add(this.button1);
            this.Options.Controls.Add(this.btnComposition);
            this.Options.Location = new  System.Drawing.Point (12, 12);
            this.Options.Name = "Options";
            this.Options.Size = new  System.Drawing.Size(355, 84);
            this.Options.TabIndex = 5;
            this.Options.TabStop = false;
            this.Options.Text = "Options";
            // 
            // button6
            // 
            this.button6.Location = new  System.Drawing.Point (190, 52);
            this.button6.Name = "button6";
            this.button6.Size = new  System.Drawing.Size(88, 26);
            this.button6.TabIndex = 9;
            this.button6.Text = "Copy From...";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new  System.EventHandler(this.CopyStream_Click);
            // 
            // button4
            // 
            this.button4.Location = new  System.Drawing.Point (190, 23);
            this.button4.Name = "button4";
            this.button4.Size = new  System.Drawing.Size(88, 26);
            this.button4.TabIndex = 10;
            this.button4.Text = "Distillation Data";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new  System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new  System.Drawing.Point (96, 53);
            this.button3.Name = "button3";
            this.button3.Size = new  System.Drawing.Size(88, 26);
            this.button3.TabIndex = 9;
            this.button3.Text = "TBP Plots";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new  System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new  System.Drawing.Point (96, 23);
            this.button2.Name = "button2";
            this.button2.Size = new  System.Drawing.Size(88, 26);
            this.button2.TabIndex = 3;
            this.button2.Text = "Property Plots";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new  System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (15, 55);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Aqueous";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnComposition
            // 
            this.btnComposition.Location = new  System.Drawing.Point (15, 23);
            this.btnComposition.Name = "btnComposition";
            this.btnComposition.Size = new  System.Drawing.Size(75, 26);
            this.btnComposition.TabIndex = 0;
            this.btnComposition.Text = "Composition";
            this.btnComposition.UseVisualStyleBackColor = true;
            this.btnComposition.Click += new  System.EventHandler(this.btnComposition_Click);
            // 
            // FlashButton
            // 
            this.FlashButton.Location = new  System.Drawing.Point (340, 338);
            this.FlashButton.Name = "FlashButton";
            this.FlashButton.Size = new  System.Drawing.Size(75, 51);
            this.FlashButton.TabIndex = 2;
            this.FlashButton.Text = "Re-Flash";
            this.FlashButton.UseVisualStyleBackColor = true;
            this.FlashButton.Click += new  System.EventHandler(this.button2_Click);
            // 
            // groupBoxStream
            // 
            this.groupBoxStream.Controls.Add(this.button5);
            this.groupBoxStream.Controls.Add(this.DGV);
            this.groupBoxStream.Location = new  System.Drawing.Point (12, 102);
            this.groupBoxStream.Name = "groupBoxStream";
            this.groupBoxStream.Size = new  System.Drawing.Size(355, 299);
            this.groupBoxStream.TabIndex = 6;
            this.groupBoxStream.TabStop = false;
            this.groupBoxStream.Text = "Conditons and Flows";
            // 
            // button5
            // 
            this.button5.Location = new  System.Drawing.Point (190, -35);
            this.button5.Name = "button5";
            this.button5.Size = new  System.Drawing.Size(88, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "Re-Flash";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // groupBoxProps
            // 
            this.groupBoxProps.Controls.Add(this.dataGridProperties);
            this.groupBoxProps.Location = new  System.Drawing.Point (405, 103);
            this.groupBoxProps.Name = "groupBoxProps";
            this.groupBoxProps.Size = new  System.Drawing.Size(355, 299);
            this.groupBoxProps.TabIndex = 7;
            this.groupBoxProps.TabStop = false;
            this.groupBoxProps.Text = "Stream Properties";
            // 
            // dataGridProperties
            // 
            this.dataGridProperties.AllowUserToAddRows = false;
            this.dataGridProperties.AllowUserToDeleteRows = false;
            this.dataGridProperties.AllowUserToResizeColumns = false;
            this.dataGridProperties.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridProperties.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridProperties.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridProperties.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridProperties.CausesValidation = false;
            this.dataGridProperties.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dataGridProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridProperties.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle8.Font = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridProperties.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridProperties.Location = new  System.Drawing.Point (15, 19);
            this.dataGridProperties.Margin = new  System.Windows.Forms.Padding(2);
            this.dataGridProperties.Name = "dataGridProperties";
            this.dataGridProperties.RowHeadersVisible = false;
            this.dataGridProperties.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridProperties.RowTemplate.Height = 28;
            this.dataGridProperties.Size = new  System.Drawing.Size(317, 267);
            this.dataGridProperties.TabIndex = 4;
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
            this.Options2.Controls.Add(this.radioButton6);
            this.Options2.Controls.Add(this.radioButton5);
            this.Options2.Controls.Add(this.radioButton4);
            this.Options2.Controls.Add(this.radioButton3);
            this.Options2.Controls.Add(this.radioButton2);
            this.Options2.Controls.Add(this.radioButton1);
            this.Options2.Location = new  System.Drawing.Point (405, 13);
            this.Options2.Name = "Options2";
            this.Options2.Size = new  System.Drawing.Size(355, 84);
            this.Options2.TabIndex = 8;
            this.Options2.TabStop = false;
            this.Options2.Text = "Options";
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new  System.Drawing.Point (231, 45);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new  System.Drawing.Size(74, 17);
            this.radioButton6.TabIndex = 19;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "Gas Props";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new  System.Drawing.Point (231, 22);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new  System.Drawing.Size(82, 17);
            this.radioButton5.TabIndex = 18;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Composition";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new  System.Drawing.Point (140, 45);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new  System.Drawing.Size(55, 17);
            this.radioButton4.TabIndex = 17;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "PONA";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new  System.Drawing.Point (140, 22);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new  System.Drawing.Size(76, 17);
            this.radioButton3.TabIndex = 16;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Cold Props";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new  System.Drawing.Point (39, 45);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new  System.Drawing.Size(64, 17);
            this.radioButton2.TabIndex = 15;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Refinery";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new  System.Drawing.Point (39, 22);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new  System.Drawing.Size(59, 17);
            this.radioButton1.TabIndex = 14;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Default";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "Property";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 80;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly  = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 59.77011F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Stream";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 78;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 59.77011F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Liquid";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly  = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 79;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 59.77011F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Vapour";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly  = true;
            this.dataGridViewTextBoxColumn4.Width = 78;
            // 
            // PortPropertyForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(772, 413);
            this.Controls.Add(this.FlashButton);
            this.Controls.Add(this.Options2);
            this.Controls.Add(this.groupBoxProps);
            this.Controls.Add(this.CBOX1);
            this.Controls.Add(this.groupBoxStream);
            this.Controls.Add(this.Options);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "PortPropertyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stream Properties";
            this.TopMost = true;
            this.Load += new  System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.Options.ResumeLayout(false);
            this.groupBoxStream.ResumeLayout(false);
            this.groupBoxProps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProperties)).EndInit();
            this.Options2.ResumeLayout(false);
            this.Options2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.ComboBox CBOX1;
        private  System.Windows.Forms.GroupBox Options;
        private  System.Windows.Forms.GroupBox groupBoxStream;
        private  System.Windows.Forms.GroupBox groupBoxProps;
        private  System.Windows.Forms.DataGridView dataGridProperties;
        private  System.Windows.Forms.GroupBox Options2;
        private  System.Windows.Forms.RadioButton radioButton6;
        private  System.Windows.Forms.RadioButton radioButton5;
        private  System.Windows.Forms.RadioButton radioButton4;
        private  System.Windows.Forms.RadioButton radioButton3;
        private  System.Windows.Forms.RadioButton radioButton2;
        private  System.Windows.Forms.RadioButton radioButton1;
        private  System.Windows.Forms.Button btnComposition;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Water;
        private  System.Windows.Forms.Button button1;
        public  System.Windows.Forms.DataGridView DGV;
        private  System.Windows.Forms.Button FlashButton;
        private  System.Windows.Forms.Button button2;
        private  System.Windows.Forms.Button button3;
        private  System.Windows.Forms.Button button4;
        private  System.Windows.Forms.Button button6;
        private  System.Windows.Forms.Button button5;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}

