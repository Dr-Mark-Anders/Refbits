using ModelEngine;
using System.Windows.Forms;
using Units.PortForm;

namespace Units
{
    partial class CompSplitterDialog
    {
        ///<summary>
        ///Requireddesignervariable.
        ///</summary>
        private System.ComponentModel.IContainer Components = null;

        ///<summary>
        ///Cleanupanyresourcesbeingused.
        ///</summary>
        ///<paramname="disposing">trueifmanagedresourcesshouldbedisposed;otherwise,false.</param>
        protected override void Dispose(bool disposing)
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
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Summary = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Streams = new System.Windows.Forms.TabPage();
            this.Worksheet = new Units.PortForm.PortsPropertyWorksheet();
            this.tabControl1.SuspendLayout();
            this.Summary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.Streams.SuspendLayout();
            this.SuspendLayout();
            //
            //tabControl1
            //
            this.tabControl1.Controls.Add(this.Summary);
            this.tabControl1.Controls.Add(this.Streams);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(937, 525);
            this.tabControl1.TabIndex = 20;
            //
            //Summary
            //
            this.Summary.BackColor = System.Drawing.SystemColors.Control;
            this.Summary.Controls.Add(this.button2);
            this.Summary.Controls.Add(this.button1);
            this.Summary.Controls.Add(this.DGV);
            this.Summary.Controls.Add(this.comboBox1);
            this.Summary.Location = new System.Drawing.Point(4, 24);
            this.Summary.Margin = new System.Windows.Forms.Padding(2);
            this.Summary.Name = "Summary";
            this.Summary.Padding = new System.Windows.Forms.Padding(2);
            this.Summary.Size = new System.Drawing.Size(929, 497);
            this.Summary.TabIndex = 0;
            this.Summary.Text = "Summary";
            //
            //button2
            //
            this.button2.Location = new System.Drawing.Point(29, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 27;
            this.button2.Text = "SettoZero";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SetToZero_Click);
            //
            //button1
            //
            this.button1.Location = new System.Drawing.Point(29, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "Setto1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SetTo1_Click);
            //
            //DGV
            //
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToResizeColumns = false;
            this.DGV.AllowUserToResizeRows = false;
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Location = new System.Drawing.Point(139, 5);
            this.DGV.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.DGV.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DGV.Size = new System.Drawing.Size(784, 484);
            this.DGV.TabIndex = 25;
            this.DGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEndEdit);
            this.DGV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            this.DGV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.DGV.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DGV_KeyDown);
            //
            //comboBox1
            //
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(51, -209);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(163, 23);
            this.comboBox1.TabIndex = 24;
            //
            //Streams
            //
            this.Streams.Controls.Add(this.Worksheet);
            this.Streams.Location = new System.Drawing.Point(4, 24);
            this.Streams.Margin = new System.Windows.Forms.Padding(2);
            this.Streams.Name = "Streams";
            this.Streams.Padding = new System.Windows.Forms.Padding(2);
            this.Streams.Size = new System.Drawing.Size(929, 497);
            this.Streams.TabIndex = 1;
            this.Streams.Text = "Streams";
            this.Streams.UseVisualStyleBackColor = true;
            //
            //Worksheet
            //
            this.Worksheet.AutoSize = true;
            this.Worksheet.Location = new System.Drawing.Point(2, 0);
            this.Worksheet.Margin = new System.Windows.Forms.Padding(2);
            this.Worksheet.Name = "Worksheet";
            this.Worksheet.Size = new System.Drawing.Size(1040, 538);
            this.Worksheet.TabIndex = 0;
            //
            //CompSplitterDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 525);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "CompSplitterDialog";
            this.Text = "DividerDialog";
            this.tabControl1.ResumeLayout(false);
            this.Summary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.Streams.ResumeLayout(false);
            this.Streams.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TabControl tabControl1;
        private TabPage Summary;
        private ComboBox comboBox1;
        private TabPage Streams;
        private PortsPropertyWorksheet Worksheet;
        private DataGridView DGV;
        private Button button2;
        private Button button1;
    }
}