
namespace   FormControls
{
    partial class  EnumerationGrid
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected  override  void   Dispose(bool  disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }



        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private  void   InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new  System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewTextBoxColumn1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gpbox = new  System.Windows.Forms.GroupBox();
            this.DGV = new  System.Windows.Forms.DataGridView();
            this.GuidColumn = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueColumn = new  System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gpbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "GUID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // gpbox
            // 
            this.gpbox.BackColor = System.Drawing.SystemColors.Control;
            this.gpbox.Controls.Add(this.DGV);
            this.gpbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpbox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gpbox.Location = new  System.Drawing.Point (0, 0);
            this.gpbox.Name = "gpbox";
            this.gpbox.Size = new  System.Drawing.Size(216, 307);
            this.gpbox.TabIndex = 3;
            this.gpbox.TabStop = false;
            this.gpbox.Text = "gpBox";
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToResizeColumns = false;
            this.DGV.AllowUserToResizeRows = false;
            this.DGV.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.GuidColumn,
            this.NameColumn,
            this.ValueColumn});
            this.DGV.Location = new  System.Drawing.Point (18, 22);
            this.DGV.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.DGV.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.DGV.Size = new  System.Drawing.Size(178, 269);
            this.DGV.TabIndex = 1;
            this.DGV.CellEndEdit += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEndEdit);
            this.DGV.RowValidated += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_RowValidated);
            // 
            // GuidColumn
            // 
            this.GuidColumn.HeaderText = "GUID";
            this.GuidColumn.Name = "GuidColumn";
            this.GuidColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GuidColumn.Visible = false;
            // 
            // NameColumn
            // 
            this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NameColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.Width = 64;
            // 
            // ValueColumn
            // 
            this.ValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ValueColumn.HeaderText = "Value";
            this.ValueColumn.Name = "ValueColumn";
            this.ValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // EnumerationGrid
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpbox);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "EnumerationGrid";
            this.Size = new  System.Drawing.Size(216, 307);
            this.Resize += new  System.EventHandler(this.PropertyDisplayGrid2_Resize);
            this.gpbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private  System.Windows.Forms.GroupBox gpbox;
        public  System.Windows.Forms.DataGridView DGV;
        private  System.Windows.Forms.DataGridViewTextBoxColumn GuidColumn;
        private  System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private  System.Windows.Forms.DataGridViewComboBoxColumn ValueColumn;
    }
}
