using   ModelEngine;
using   System.Windows.Forms;
using   ModelEngine;
using   Units.PortForm;

namespace   Units
{
    partial class  ExpanderDialog
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
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(ExpanderDialog));
            this.tabControl1 = new  System.Windows.Forms.TabControl();
            this.Summary = new  System.Windows.Forms.TabPage();
            this.Results = new  FormControls.UserPropGrid();
            this.Parameters = new  FormControls.UserPropGrid();
            this.comboBox1 = new  System.Windows.Forms.ComboBox();
            this.Streams = new  System.Windows.Forms.TabPage();
            this.worksheet = new  Units.PortForm.PortsPropertyWorksheet();
            this.dataGridViewTextBoxColumn1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.Summary.SuspendLayout();
            this.Streams.SuspendLayout();
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
            this.tabControl1.Size = new  System.Drawing.Size(924, 522);
            this.tabControl1.TabIndex = 20;
            // 
            // Summary
            // 
            this.Summary.BackColor = System.Drawing.SystemColors.Control;
            this.Summary.Controls.Add(this.Results);
            this.Summary.Controls.Add(this.Parameters);
            this.Summary.Controls.Add(this.comboBox1);
            this.Summary.Location = new  System.Drawing.Point (4, 24);
            this.Summary.Margin = new  System.Windows.Forms.Padding(2);
            this.Summary.Name = "Summary";
            this.Summary.Padding = new  System.Windows.Forms.Padding(2);
            this.Summary.Size = new  System.Drawing.Size(916, 494);
            this.Summary.TabIndex = 0;
            this.Summary.Text = "Summary";
            // 
            // Results
            // 
            this.Results.AllowUserToAddRows = false;
            this.Results.AllowUserToDeleteRows = false;
            this.Results.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Results.ColumnNames")));
            this.Results.DisplayTitles = true;
            this.Results.FirstColumnWidth = 64;
            this.Results.Location = new  System.Drawing.Point (400, 35);
            this.Results.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Results.Name = "Results";
            this.Results.RowHeadersVisible = false;
            this.Results.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Results.RowNames")));
            this.Results.Size = new  System.Drawing.Size(324, 307);
            this.Results.TabIndex = 33;
            this.Results.TopText = "Results";
            // 
            // Parameters
            // 
            this.Parameters.AllowUserToAddRows = false;
            this.Parameters.AllowUserToDeleteRows = false;
            this.Parameters.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Parameters.ColumnNames")));
            this.Parameters.DisplayTitles = true;
            this.Parameters.FirstColumnWidth = 64;
            this.Parameters.Location = new  System.Drawing.Point (90, 35);
            this.Parameters.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Parameters.Name = "Parameters";
            this.Parameters.RowHeadersVisible = false;
            this.Parameters.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Parameters.RowNames")));
            this.Parameters.Size = new  System.Drawing.Size(202, 112);
            this.Parameters.TabIndex = 32;
            this.Parameters.TopText = "Parameters";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new  System.Drawing.Point (51, -209);
            this.comboBox1.Margin = new  System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new  System.Drawing.Size(163, 23);
            this.comboBox1.TabIndex = 24;
            // 
            // Streams
            // 
            this.Streams.Controls.Add(this.worksheet);
            this.Streams.Location = new  System.Drawing.Point (4, 24);
            this.Streams.Margin = new  System.Windows.Forms.Padding(2);
            this.Streams.Name = "Streams";
            this.Streams.Padding = new  System.Windows.Forms.Padding(2);
            this.Streams.Size = new  System.Drawing.Size(916, 494);
            this.Streams.TabIndex = 1;
            this.Streams.Text = "Streams";
            this.Streams.UseVisualStyleBackColor = true;
            // 
            // worksheet
            // 
            this.worksheet.AutoSize = true;
            this.worksheet.Location = new  System.Drawing.Point (5, 6);
            this.worksheet.Margin = new  System.Windows.Forms.Padding(2);
            this.worksheet.Name = "worksheet";
            this.worksheet.Size = new  System.Drawing.Size(1040, 538);
            this.worksheet.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Result";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // ExpanderDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(924, 522);
            this.Controls.Add(this.tabControl1);
            this.Name = "ExpanderDialog";
            this.Text = "ExpanderDialog";
            this.tabControl1.ResumeLayout(false);
            this.Summary.ResumeLayout(false);
            this.Streams.ResumeLayout(false);
            this.Streams.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private  TabControl tabControl1;
        private  TabPage Summary;
        private  ComboBox comboBox1;
        private  TabPage Streams;
        private  PortsPropertyWorksheet worksheet;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  FormControls.UserPropGrid Results;
        private  FormControls.UserPropGrid Parameters;
    }
}