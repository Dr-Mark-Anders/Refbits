namespace ModelEngine
{
    partial class InconsistencyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
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
        private void InitializeComponent()
        {
            this.DGV = new System.Windows.Forms.DataGridView();
            this.Inconsistency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourcePort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DestObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DestPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Inconsistency,
            this.SourceObject,
            this.SourcePort,
            this.Value1,
            this.DestObject,
            this.DestPort,
            this.Value2});
            this.DGV.Location = new System.Drawing.Point(14, 14);
            this.DGV.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersVisible = false;
            this.DGV.Size = new System.Drawing.Size(905, 492);
            this.DGV.TabIndex = 0;
            // 
            // Inconsistency
            // 
            this.Inconsistency.HeaderText = "Inconsistency";
            this.Inconsistency.Name = "Inconsistency";
            // 
            // Sourceobject 
            // 
            this.SourceObject.HeaderText = "Source Object";
            this.SourceObject.Name = "SourceObject";
            // 
            // SourcePort
            // 
            this.SourcePort.HeaderText = "SourcePort";
            this.SourcePort.Name = "SourcePort";
            // 
            // Value1
            // 
            this.Value1.HeaderText = "Value";
            this.Value1.Name = "Value1";
            // 
            // Destobject 
            // 
            this.DestObject.HeaderText = "DestObject";
            this.DestObject.Name = "DestObject";
            // 
            // DestPort
            // 
            this.DestPort.HeaderText = "DestPort";
            this.DestPort.Name = "DestPort";
            // 
            // Value2
            // 
            this.Value2.HeaderText = "Value";
            this.Value2.Name = "Value2";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Inconsistency";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Property";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Value";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Location";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // InconsistencyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.DGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "InconsistencyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InconsistencyForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.InconsistencyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inconsistency;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourceObject;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourcePort;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DestObject;
        private System.Windows.Forms.DataGridViewTextBoxColumn DestPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value2;
    }
}