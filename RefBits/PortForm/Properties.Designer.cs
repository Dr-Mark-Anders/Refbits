namespace   Units.PortForm
{
    partial class  StreamProperties
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
            this.dataGridProps = new  System.Windows.Forms.DataGridView();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BP = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Omega = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProps)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridProps
            // 
            this.dataGridProps.AllowUserToAddRows = false;
            this.dataGridProps.AllowUserToDeleteRows = false;
            this.dataGridProps.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridProps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridProps.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.BP,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Omega});
            this.dataGridProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridProps.Location = new  System.Drawing.Point (0, 0);
            this.dataGridProps.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridProps.Name = "dataGridProps";
            this.dataGridProps.ReadOnly  = true;
            this.dataGridProps.RowHeadersVisible = false;
            this.dataGridProps.Size = new  System.Drawing.Size(826, 692);
            this.dataGridProps.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Component";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly  = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "MW";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly  = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "SG";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly  = true;
            // 
            // BP
            // 
            this.BP.HeaderText = "BP";
            this.BP.Name = "BP";
            this.BP.ReadOnly  = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "CritT";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly  = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "CritP";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly  = true;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "CritZ";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly  = true;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "CritV";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly  = true;
            // 
            // Omega
            // 
            this.Omega.HeaderText = "Acentric Fact.";
            this.Omega.Name = "Omega";
            this.Omega.ReadOnly  = true;
            // 
            // StreamProperties
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(826, 692);
            this.Controls.Add(this.dataGridProps);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "StreamProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Properties";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProps)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.DataGridView dataGridProps;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private  System.Windows.Forms.DataGridViewTextBoxColumn BP;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Omega;
    }
}