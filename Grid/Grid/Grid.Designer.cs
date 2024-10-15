namespace   UOMGrid
{
    partial class  Grid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private  void  InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new  System.Windows.Forms.DataGridViewCellStyle();
            this.DGV = new  System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AllowDrop = true;
            this.DGV.AllowUserToAddRows = false;
            this.DGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.Font = new  System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.EnableHeadersVisualStyles = false;
            this.DGV.Location = new  System.Drawing.Point (0, 0);
            this.DGV.Name = "DGV";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.Font = new  System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DGV.RowHeadersWidth = 50;
            this.DGV.RowTemplate.Height = 25;
            this.DGV.Size = new  System.Drawing.Size(729, 424);
            this.DGV.TabIndex = 0;
            this.DGV.CellDoubleClick += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CelldoubleClick);
            this.DGV.CellEndEdit += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEndEdit);
            this.DGV.CellFormatting += new  System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            this.DGV.CellMouseClick += new  System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseClick);
            this.DGV.CellMouseDown += new  System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseDown);
            this.DGV.CellMouseEnter += new  System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellMouseEnter);
            this.DGV.DragDrop += new  System.Windows.Forms.DragEventHandler(this.DGV_DragDrop);
            this.DGV.DragOver += new  System.Windows.Forms.DragEventHandler(this.DGV_DragOver);
            this.DGV.KeyDown += new  System.Windows.Forms.KeyEventHandler(this.DGV_KeyDown);
            // 
            // Grid
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DGV);
            this.Name = "Grid";
            this.Size = new  System.Drawing.Size(729, 424);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private  DataGridView DGV;
    }
}
