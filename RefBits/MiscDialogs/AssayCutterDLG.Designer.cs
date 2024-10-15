using System.Windows.Forms;

namespace Units
{
    partial class AssayCutterDLG
    {
        ///<summary>
        ///Requireddesignervariable.
        ///</summary>
        private System.ComponentModel.IContainer Components = null;

        ///<summary>
        ///Cleanupanyresourcesbeingused.
        ///</summary>
        ///<paramname="disposing">trueifmanagedresourcesshouldbedisposed;otherwise,false.</param>
        protected override void  Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region WindowsFormDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>
        private void  InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FCP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Worksheet = new Units.PortForm.PortsPropertyWorksheet();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            //
            //dataGridView1
            //
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]{
this.Column1,
this.FCP,
this.Column5,
this.Column6,
this.Column7,
this.Column2,
this.Column3,
this.Column4});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1036, 430);
            this.dataGridView1.TabIndex = 0;
            //
            //Column1
            //
            this.Column1.HeaderText = "InitialCutPoint (C)";
            this.Column1.Name = "Column1";
            //
            //FCP
            //
            this.FCP.HeaderText = "FinalCutPoint (C)";
            this.FCP.Name = "FCP";
            //
            //Column5
            //
            this.Column5.HeaderText = "MassFlow";
            this.Column5.Name = "Column5";
            //
            //Column6
            //
            this.Column6.HeaderText = "VolFlow";
            this.Column6.Name = "Column6";
            //
            //Column7
            //
            this.Column7.HeaderText = "MolFlow";
            this.Column7.Name = "Column7";
            //
            //Column2
            //
            this.Column2.HeaderText = "MassFrac";
            this.Column2.Name = "Column2";
            //
            //Column3
            //
            this.Column3.HeaderText = "VolFrac";
            this.Column3.Name = "Column3";
            //
            //Column4
            //
            this.Column4.HeaderText = "MolFrac";
            this.Column4.Name = "Column4";
            //
            //button1
            //
            this.button1.Location = new System.Drawing.Point(937, 14);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 36);
            this.button1.TabIndex = 1;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            //dataGridViewTextBoxColumn1
            //
            this.dataGridViewTextBoxColumn1.HeaderText = "InitialCutPoint (C)";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            //
            //dataGridViewTextBoxColumn2
            //
            this.dataGridViewTextBoxColumn2.HeaderText = "MassFrac";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            //
            //dataGridViewTextBoxColumn3
            //
            this.dataGridViewTextBoxColumn3.HeaderText = "VolFrac";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            //
            //dataGridViewTextBoxColumn4
            //
            this.dataGridViewTextBoxColumn4.HeaderText = "MolFrac";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            //
            //tabControl1
            //
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1050, 464);
            this.tabControl1.TabIndex = 2;
            //
            //tabPage1
            //
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1042, 436);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "CutDefinitions";
            this.tabPage1.UseVisualStyleBackColor = true;
            //
            //tabPage2
            //
            this.tabPage2.Controls.Add(this.Worksheet);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1042, 436);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "StreamData";
            this.tabPage2.UseVisualStyleBackColor = true;
            //
            //Worksheet
            //
            this.Worksheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Worksheet.Location = new System.Drawing.Point(3, 3);
            this.Worksheet.Margin = new System.Windows.Forms.Padding(2);
            this.Worksheet.Name = "Worksheet";
            this.Worksheet.Size = new System.Drawing.Size(1036, 430);
            this.Worksheet.TabIndex = 0;
            //
            //AssayCutterDLG
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 464);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AssayCutterDLG";
            this.Text = "AssayCutterDLG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AssayCutterDLG_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn FCP;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private PortForm.PortsPropertyWorksheet Worksheet;
    }
}