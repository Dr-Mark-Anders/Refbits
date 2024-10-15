namespace   Units.DrawingObjects.UnitDialogs
{
    partial class  TableDialog
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
            this.cbMassFlow = new  System.Windows.Forms.CheckBox();
            this.cbVolFlow = new  System.Windows.Forms.CheckBox();
            this.CBMolarFlow = new  System.Windows.Forms.CheckBox();
            this.cbTemperature  = new  System.Windows.Forms.CheckBox();
            this.CBPressure  = new  System.Windows.Forms.CheckBox();
            this.cbHeatFlow = new  System.Windows.Forms.CheckBox();
            this.dgv = new  System.Windows.Forms.DataGridView();
            this.StreamName = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Included = new  System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new  System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // cbMassFlow
            // 
            this.cbMassFlow.AutoSize = true;
            this.cbMassFlow.Location = new  System.Drawing.Point (26, 78);
            this.cbMassFlow.Name = "cbMassFlow";
            this.cbMassFlow.Size = new  System.Drawing.Size(76, 17);
            this.cbMassFlow.TabIndex = 0;
            this.cbMassFlow.Text = "Mass Flow";
            this.cbMassFlow.UseVisualStyleBackColor = true;
            // 
            // cbVolFlow
            // 
            this.cbVolFlow.AutoSize = true;
            this.cbVolFlow.Location = new  System.Drawing.Point (26, 101);
            this.cbVolFlow.Name = "cbVolFlow";
            this.cbVolFlow.Size = new  System.Drawing.Size(102, 17);
            this.cbVolFlow.TabIndex = 1;
            this.cbVolFlow.Text = "Act VolumnFlow";
            this.cbVolFlow.UseVisualStyleBackColor = true;
            // 
            // CBMolarFlow
            // 
            this.CBMolarFlow.AutoSize = true;
            this.CBMolarFlow.Location = new  System.Drawing.Point (26, 124);
            this.CBMolarFlow.Name = "CBMolarFlow";
            this.CBMolarFlow.Size = new  System.Drawing.Size(77, 17);
            this.CBMolarFlow.TabIndex = 2;
            this.CBMolarFlow.Text = "Molar Flow";
            this.CBMolarFlow.UseVisualStyleBackColor = true;
            // 
            // cbTemperature 
            // 
            this.cbTemperature .AutoSize = true;
            this.cbTemperature .Location = new  System.Drawing.Point (26, 147);
            this.cbTemperature .Name = "cbTemperature ";
            this.cbTemperature .Size = new  System.Drawing.Size(86, 17);
            this.cbTemperature .TabIndex = 3;
            this.cbTemperature .Text = "Temperature";
            this.cbTemperature .UseVisualStyleBackColor = true;
            // 
            // CBPressure 
            // 
            this.CBPressure .AutoSize = true;
            this.CBPressure .Location = new  System.Drawing.Point (26, 170);
            this.CBPressure .Name = "CBPressure ";
            this.CBPressure .Size = new  System.Drawing.Size(67, 17);
            this.CBPressure .TabIndex = 5;
            this.CBPressure .Text = "Pressure";
            this.CBPressure .UseVisualStyleBackColor = true;
            // 
            // cbHeatFlow
            // 
            this.cbHeatFlow.AutoSize = true;
            this.cbHeatFlow.Location = new  System.Drawing.Point (26, 193);
            this.cbHeatFlow.Name = "cbHeatFlow";
            this.cbHeatFlow.Size = new  System.Drawing.Size(74, 17);
            this.cbHeatFlow.TabIndex = 6;
            this.cbHeatFlow.Text = "Heat Flow";
            this.cbHeatFlow.UseVisualStyleBackColor = true;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.StreamName,
            this.Included});
            this.dgv.Location = new  System.Drawing.Point (182, 12);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new  System.Drawing.Size(207, 252);
            this.dgv.TabIndex = 7;
            this.dgv.CurrentCellDirtyStateChanged += new  System.EventHandler(this.dgv_CurrentCellDirtyStateChanged);
            // 
            // StreamName
            // 
            this.StreamName.HeaderText = "Stream Name";
            this.StreamName.Name = "StreamName";
            // 
            // Included
            // 
            this.Included.HeaderText = "Display";
            this.Included.Name = "Included";
            this.Included.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Included.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Stream Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 102;
            // 
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (418, 41);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Add All";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new  System.EventHandler(this.AddAll_Click);
            // 
            // TableDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(528, 289);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.cbHeatFlow);
            this.Controls.Add(this.CBPressure );
            this.Controls.Add(this.cbTemperature );
            this.Controls.Add(this.CBMolarFlow);
            this.Controls.Add(this.cbVolFlow);
            this.Controls.Add(this.cbMassFlow);
            this.Name = "TableDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SummaryDialog";
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.TableDialog_FormClosing);
            this.Load += new  System.EventHandler(this.SummaryDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private  System.Windows.Forms.CheckBox cbMassFlow;
        private  System.Windows.Forms.CheckBox cbVolFlow;
        private  System.Windows.Forms.CheckBox CBMolarFlow;
        private  System.Windows.Forms.CheckBox cbTemperature ;
        private  System.Windows.Forms.CheckBox CBPressure ;
        private  System.Windows.Forms.CheckBox cbHeatFlow;
        private  System.Windows.Forms.DataGridView dgv;
        private  System.Windows.Forms.DataGridViewTextBoxColumn StreamName;
        private  System.Windows.Forms.DataGridViewCheckBoxColumn Included;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  System.Windows.Forms.Button button1;
    }
}