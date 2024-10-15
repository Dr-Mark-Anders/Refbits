using   ModelEngine;
using   System.Windows.Forms;
using   Units;
using   Units.PortForm;

namespace   Units
{
    partial class  HeatExDialog 
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
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(HeatExDialog));
            this.tabControl1 = new  System.Windows.Forms.TabControl();
            this.Summary = new  System.Windows.Forms.TabPage();
            this.Specifications = new  FormControls.UserPropGrid();
            this.Paremeters = new  FormControls.UserPropGrid();
            this.Results = new  FormControls.UserPropGrid();
            this.cbxActive = new  System.Windows.Forms.CheckBox();
            this.comboBox1 = new  System.Windows.Forms.ComboBox();
            this.Streams = new  System.Windows.Forms.TabPage();
            this.Worksheet = new  Units.PortForm.PortsPropertyWorksheet();
            this.Mechanical = new  System.Windows.Forms.TabPage();
            this.groupBox8 = new  System.Windows.Forms.GroupBox();
            this.groupBox3 = new  System.Windows.Forms.GroupBox();
            this.groupBox9 = new  System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.Summary.SuspendLayout();
            this.Streams.SuspendLayout();
            this.Mechanical.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Summary);
            this.tabControl1.Controls.Add(this.Streams);
            this.tabControl1.Controls.Add(this.Mechanical);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new  System.Drawing.Point (0, 0);
            this.tabControl1.Margin = new  System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new  System.Drawing.Size(919, 512);
            this.tabControl1.TabIndex = 20;
            // 
            // Summary
            // 
            this.Summary.BackColor = System.Drawing.SystemColors.Control;
            this.Summary.Controls.Add(this.Specifications);
            this.Summary.Controls.Add(this.Paremeters);
            this.Summary.Controls.Add(this.Results);
            this.Summary.Controls.Add(this.cbxActive);
            this.Summary.Controls.Add(this.comboBox1);
            this.Summary.Location = new  System.Drawing.Point (4, 24);
            this.Summary.Margin = new  System.Windows.Forms.Padding(2);
            this.Summary.Name = "Summary";
            this.Summary.Padding = new  System.Windows.Forms.Padding(2);
            this.Summary.Size = new  System.Drawing.Size(911, 484);
            this.Summary.TabIndex = 0;
            this.Summary.Text = "Summary";
            // 
            // Specifications
            // 
            this.Specifications.AllowUserToAddRows = false;
            this.Specifications.AllowUserToDeleteRows = false;
            this.Specifications.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Specifications.ColumnNames")));
            this.Specifications.DisplayTitles = true;
            this.Specifications.FirstColumnWidth = 64;
            this.Specifications.Location = new  System.Drawing.Point (58, 32);
            this.Specifications.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Specifications.Name = "Specifications";
            this.Specifications.ReadOnly  = false;
            this.Specifications.RowHeadersVisible = false;
            this.Specifications.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Specifications.RowNames")));
            this.Specifications.Size = new  System.Drawing.Size(363, 235);
            this.Specifications.TabIndex = 29;
            this.Specifications.TopText = "Specifications";
            // 
            // Paremeters
            // 
            this.Paremeters.AllowUserToAddRows = false;
            this.Paremeters.AllowUserToDeleteRows = false;
            this.Paremeters.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Paremeters.ColumnNames")));
            this.Paremeters.DisplayTitles = true;
            this.Paremeters.FirstColumnWidth = 64;
            this.Paremeters.Location = new  System.Drawing.Point (58, 287);
            this.Paremeters.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Paremeters.Name = "Paremeters";
            this.Paremeters.ReadOnly  = false;
            this.Paremeters.RowHeadersVisible = false;
            this.Paremeters.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Paremeters.RowNames")));
            this.Paremeters.Size = new  System.Drawing.Size(363, 119);
            this.Paremeters.TabIndex = 28;
            this.Paremeters.TopText = "Parameters";
            // 
            // Results
            // 
            this.Results.AllowUserToAddRows = false;
            this.Results.AllowUserToDeleteRows = false;
            this.Results.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Results.ColumnNames")));
            this.Results.DisplayTitles = true;
            this.Results.FirstColumnWidth = 64;
            this.Results.Location = new  System.Drawing.Point (457, 32);
            this.Results.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Results.Name = "Results";
            this.Results.ReadOnly  = true;
            this.Results.RowHeadersVisible = false;
            this.Results.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Results.RowNames")));
            this.Results.Size = new  System.Drawing.Size(322, 235);
            this.Results.TabIndex = 27;
            this.Results.TopText = "Results";
            // 
            // cbxActive
            // 
            this.cbxActive.AutoSize = true;
            this.cbxActive.Location = new  System.Drawing.Point (835, 453);
            this.cbxActive.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxActive.Name = "cbxActive";
            this.cbxActive.Size = new  System.Drawing.Size(59, 19);
            this.cbxActive.TabIndex = 25;
            this.cbxActive.Text = "Active";
            this.cbxActive.UseVisualStyleBackColor = true;
            this.cbxActive.CheckedChanged += new  System.EventHandler(this.cbxActive_CheckedChanged);
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
            this.Streams.BackColor = System.Drawing.SystemColors.Control;
            this.Streams.Controls.Add(this.Worksheet);
            this.Streams.Location = new  System.Drawing.Point (4, 24);
            this.Streams.Margin = new  System.Windows.Forms.Padding(2);
            this.Streams.Name = "Streams";
            this.Streams.Padding = new  System.Windows.Forms.Padding(2);
            this.Streams.Size = new  System.Drawing.Size(911, 484);
            this.Streams.TabIndex = 1;
            this.Streams.Text = "Streams";
            // 
            // Worksheet
            // 
            this.Worksheet.AutoSize = true;
            this.Worksheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Worksheet.Location = new  System.Drawing.Point (2, 2);
            this.Worksheet.Margin = new  System.Windows.Forms.Padding(2);
            this.Worksheet.Name = "Worksheet";
            this.Worksheet.Size = new  System.Drawing.Size(907, 480);
            this.Worksheet.TabIndex = 0;
            // 
            // Mechanical
            // 
            this.Mechanical.BackColor = System.Drawing.SystemColors.Control;
            this.Mechanical.Controls.Add(this.groupBox8);
            this.Mechanical.Controls.Add(this.groupBox3);
            this.Mechanical.Controls.Add(this.groupBox9);
            this.Mechanical.Location = new  System.Drawing.Point (4, 24);
            this.Mechanical.Margin = new  System.Windows.Forms.Padding(2);
            this.Mechanical.Name = "Mechanical";
            this.Mechanical.Padding = new  System.Windows.Forms.Padding(2);
            this.Mechanical.Size = new  System.Drawing.Size(911, 484);
            this.Mechanical.TabIndex = 2;
            this.Mechanical.Text = "Design";
            // 
            // groupBox8
            // 
            this.groupBox8.Location = new  System.Drawing.Point (356, 23);
            this.groupBox8.Margin = new  System.Windows.Forms.Padding(2);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new  System.Windows.Forms.Padding(2);
            this.groupBox8.Size = new  System.Drawing.Size(316, 378);
            this.groupBox8.TabIndex = 8;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Shell And Tube Design Results";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new  System.Drawing.Point (33, 23);
            this.groupBox3.Margin = new  System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new  System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new  System.Drawing.Size(264, 224);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Shell And Tube Design Input";
            // 
            // groupBox9
            // 
            this.groupBox9.Location = new  System.Drawing.Point (33, 278);
            this.groupBox9.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox9.Size = new  System.Drawing.Size(264, 123);
            this.groupBox9.TabIndex = 12;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Fouling Resistance";
            // 
            // HeatExDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(919, 512);
            this.Controls.Add(this.tabControl1);
            this.Margin = new  System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "HeatExDialog";
            this.Text = "HeatExDialog";
            this.TopMost = true;
            this.Load += new  System.EventHandler(this.HeatExDialog_Load);
            this.tabControl1.ResumeLayout(false);
            this.Summary.ResumeLayout(false);
            this.Summary.PerformLayout();
            this.Streams.ResumeLayout(false);
            this.Streams.PerformLayout();
            this.Mechanical.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private  TabControl tabControl1;
        private  TabPage Summary;
        private  ComboBox comboBox1;
        private  TabPage Streams;
        private  PortsPropertyWorksheet Worksheet;
        private  TabPage Mechanical;
        private  GroupBox groupBox3;
        private  GroupBox groupBox8;
        private  GroupBox groupBox9;
        private  CheckBox cbxActive;
        private  FormControls.UserPropGrid Results;
        private  FormControls.UserPropGrid Paremeters;
        private  FormControls.UserPropGrid Specifications;
    }
}