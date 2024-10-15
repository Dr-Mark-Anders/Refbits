
namespace   FormControls
{
    partial class  PropertyGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new  System.Windows.Forms.DataGridViewCellStyle();
            this.DGV = new  System.Windows.Forms.DataGridView();
            this.GuidColumn = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueColumn = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CBOX1 = new  System.Windows.Forms.ComboBox();
            this.dataGridViewTextBoxColumn1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToOrderColumns = true;
            this.DGV.AllowUserToResizeRows = false;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.GuidColumn,
            this.NameColumn,
            this.ValueColumn});
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new  System.Drawing.Point (0, 0);
            this.DGV.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DGV.MultiSelect = false;
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersVisible = false;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.DGV.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV.Size = new  System.Drawing.Size(307, 197);
            this.DGV.TabIndex = 0;
            this.DGV.CellClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.DGV.CellDoubleClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CelldoubleClick);
            this.DGV.CellEndEdit += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEndEdit);
            // 
            // GuidColumn
            // 
            this.GuidColumn.HeaderText = "GUID";
            this.GuidColumn.Name = "GuidColumn";
            this.GuidColumn.Visible = false;
            // 
            // NameColumn
            // 
            this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.Width = 64;
            // 
            // ValueColumn
            // 
            this.ValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ValueColumn.HeaderText = "Value";
            this.ValueColumn.Name = "ValueColumn";
            // 
            // CBOX1
            // 
            this.CBOX1.FormattingEnabled = true;
            this.CBOX1.Location = new  System.Drawing.Point (100, 91);
            this.CBOX1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CBOX1.Name = "CBOX1";
            this.CBOX1.Size = new  System.Drawing.Size(98, 23);
            this.CBOX1.TabIndex = 2;
            this.CBOX1.Visible = false;
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
            // TextGrid
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CBOX1);
            this.Controls.Add(this.DGV);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "TextGrid";
            this.Size = new  System.Drawing.Size(307, 197);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.ComboBox CBOX1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        public  System.Windows.Forms.DataGridView DGV;
        private  System.Windows.Forms.DataGridViewTextBoxColumn GuidColumn;
        private  System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private  System.Windows.Forms.DataGridViewTextBoxColumn ValueColumn;
    }
}
