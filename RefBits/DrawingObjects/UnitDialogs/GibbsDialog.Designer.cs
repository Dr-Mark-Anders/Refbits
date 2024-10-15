using   ModelEngine;
using   System.Windows.Forms;
using   Units.PortForm;

namespace   Units
{
    partial class  GibbsDialog
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
            this.tabControl1 = new  System.Windows.Forms.TabControl();
            this.Summary = new  System.Windows.Forms.TabPage();
            this.comboBox1 = new  System.Windows.Forms.ComboBox();
            this.Streams = new  System.Windows.Forms.TabPage();
            this.Worksheet = new  PortsPropertyWorksheet();
            this.dataGridView1 = new  System.Windows.Forms.DataGridView();
            this.Active = new  System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Feed = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.Summary.SuspendLayout();
            this.Streams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Summary);
            this.tabControl1.Controls.Add(this.Streams);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new  System.Drawing.Point (0, 0);
            this.tabControl1.Margin = new  System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new  System.Drawing.Size(805, 470);
            this.tabControl1.TabIndex = 20;
            // 
            // Summary
            // 
            this.Summary.BackColor = System.Drawing.SystemColors.Control;
            this.Summary.Controls.Add(this.dataGridView1);
            this.Summary.Controls.Add(this.comboBox1);
            this.Summary.Location = new  System.Drawing.Point (4, 22);
            this.Summary.Margin = new  System.Windows.Forms.Padding(2);
            this.Summary.Name = "Summary";
            this.Summary.Padding = new  System.Windows.Forms.Padding(2);
            this.Summary.Size = new  System.Drawing.Size(797, 444);
            this.Summary.TabIndex = 0;
            this.Summary.Text = "Summary";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new  System.Drawing.Point (44, -181);
            this.comboBox1.Margin = new  System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new  System.Drawing.Size(140, 21);
            this.comboBox1.TabIndex = 24;
            // 
            // Streams
            // 
            this.Streams.Controls.Add(this.Worksheet);
            this.Streams.Location = new  System.Drawing.Point (4, 22);
            this.Streams.Margin = new  System.Windows.Forms.Padding(2);
            this.Streams.Name = "Streams";
            this.Streams.Padding = new  System.Windows.Forms.Padding(2);
            this.Streams.Size = new  System.Drawing.Size(797, 444);
            this.Streams.TabIndex = 1;
            this.Streams.Text = "Streams";
            this.Streams.UseVisualStyleBackColor = true;
            // 
            // Worksheet
            // 
            this.Worksheet.AutoSize = true;
            this.Worksheet.Location = new  System.Drawing.Point (2, 0);
            this.Worksheet.Margin = new  System.Windows.Forms.Padding(2);
            this.Worksheet.Name = "Worksheet";
            this.Worksheet.Size = new  System.Drawing.Size(763, 405);
            this.Worksheet.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Active,
            this.Feed,
            this.Column1});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView1.Location = new  System.Drawing.Point (2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dataGridView1.Size = new  System.Drawing.Size(352, 440);
            this.dataGridView1.TabIndex = 21;
            // 
            // Active
            // 
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            // 
            // Feed
            // 
            this.Feed.HeaderText = "Feed";
            this.Feed.Name = "Feed";
            this.Feed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Feed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Product";
            this.Column1.Name = "Column1";
            // 
            // GibbsDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(805, 470);
            this.Controls.Add(this.tabControl1);
            this.Margin = new  System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GibbsDialog";
            this.tabControl1.ResumeLayout(false);
            this.Summary.ResumeLayout(false);
            this.Streams.ResumeLayout(false);
            this.Streams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private  TabControl tabControl1;
        private  TabPage Summary;
        private  ComboBox comboBox1;
        private  TabPage Streams;
        private  PortsPropertyWorksheet Worksheet;
        private  DataGridView dataGridView1;
        private  DataGridViewCheckBoxColumn Active;
        private  DataGridViewTextBoxColumn Feed;
        private  DataGridViewTextBoxColumn Column1;
    }
}