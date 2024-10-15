using   ModelEngine;
using   System.Windows.Forms;
using   System.Windows.Forms.DataVisualization;
using   Units.PortForm;

namespace   Units
{
    partial class  RecycleDialog
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
            this.checkBox1 = new  System.Windows.Forms.CheckBox();
            this.groupBox1 = new  System.Windows.Forms.GroupBox();
            this.rbDEM = new  System.Windows.Forms.RadioButton();
            this.rbWegstein = new  System.Windows.Forms.RadioButton();
            this.rbDirectSubstition = new  System.Windows.Forms.RadioButton();
            this.comboBox1 = new  System.Windows.Forms.ComboBox();
            this.Streams = new  System.Windows.Forms.TabPage();
            this.HistoryChart = new  System.Windows.Forms.TabPage();
            this.HistoryTable = new  System.Windows.Forms.TabPage();
            this.dataGridView1 = new  System.Windows.Forms.DataGridView();
            this.Pressure  = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temperature  = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MolarEnthalpy = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Flow = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VapourFraction = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Worksheet = new  Units.PortForm.PortsPropertyWorksheet();
            this.tabControl1.SuspendLayout();
            this.Summary.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.Streams.SuspendLayout();
            this.HistoryTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Summary);
            this.tabControl1.Controls.Add(this.Streams);
            this.tabControl1.Controls.Add(this.HistoryChart);
            this.tabControl1.Controls.Add(this.HistoryTable);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new  System.Drawing.Point (0, 0);
            this.tabControl1.Margin = new  System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new  System.Drawing.Size(926, 518);
            this.tabControl1.TabIndex = 20;
            // 
            // Summary
            // 
            this.Summary.BackColor = System.Drawing.SystemColors.Control;
            this.Summary.Controls.Add(this.checkBox1);
            this.Summary.Controls.Add(this.groupBox1);
            this.Summary.Controls.Add(this.comboBox1);
            this.Summary.Location = new  System.Drawing.Point (4, 24);
            this.Summary.Margin = new  System.Windows.Forms.Padding(2);
            this.Summary.Name = "Summary";
            this.Summary.Padding = new  System.Windows.Forms.Padding(2);
            this.Summary.Size = new  System.Drawing.Size(918, 490);
            this.Summary.TabIndex = 0;
            this.Summary.Text = "Summary";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new  System.Drawing.Point (852, 464);
            this.checkBox1.Margin = new  System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new  System.Drawing.Size(59, 19);
            this.checkBox1.TabIndex = 28;
            this.checkBox1.Text = "Active";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new  System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDEM);
            this.groupBox1.Controls.Add(this.rbWegstein);
            this.groupBox1.Controls.Add(this.rbDirectSubstition);
            this.groupBox1.Location = new  System.Drawing.Point (46, 48);
            this.groupBox1.Margin = new  System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new  System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new  System.Drawing.Size(180, 122);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Solution Method";
            // 
            // rbDEM
            // 
            this.rbDEM.AutoSize = true;
            this.rbDEM.Location = new  System.Drawing.Point (19, 83);
            this.rbDEM.Margin = new  System.Windows.Forms.Padding(2);
            this.rbDEM.Name = "rbDEM";
            this.rbDEM.Size = new  System.Drawing.Size(138, 19);
            this.rbDEM.TabIndex = 27;
            this.rbDEM.Text = "Dominant Eigenvalue";
            this.rbDEM.UseVisualStyleBackColor = true;
            // 
            // rbWegstein
            // 
            this.rbWegstein.AutoSize = true;
            this.rbWegstein.Location = new  System.Drawing.Point (19, 59);
            this.rbWegstein.Margin = new  System.Windows.Forms.Padding(2);
            this.rbWegstein.Name = "rbWegstein";
            this.rbWegstein.Size = new  System.Drawing.Size(74, 19);
            this.rbWegstein.TabIndex = 25;
            this.rbWegstein.Text = "Wegstein";
            this.rbWegstein.UseVisualStyleBackColor = true;
            this.rbWegstein.CheckedChanged += new  System.EventHandler(this.rbWegstein_CheckedChanged);
            // 
            // rbDirectSubstition
            // 
            this.rbDirectSubstition.AutoSize = true;
            this.rbDirectSubstition.Location = new  System.Drawing.Point (19, 33);
            this.rbDirectSubstition.Margin = new  System.Windows.Forms.Padding(2);
            this.rbDirectSubstition.Name = "rbDirectSubstition";
            this.rbDirectSubstition.Size = new  System.Drawing.Size(123, 19);
            this.rbDirectSubstition.TabIndex = 26;
            this.rbDirectSubstition.Text = "Direct Substitution";
            this.rbDirectSubstition.UseVisualStyleBackColor = true;
            this.rbDirectSubstition.CheckedChanged += new  System.EventHandler(this.rbDirectSubstition_CheckedChanged);
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
            this.Streams.Controls.Add(this.Worksheet);
            this.Streams.Location = new  System.Drawing.Point (4, 24);
            this.Streams.Margin = new  System.Windows.Forms.Padding(2);
            this.Streams.Name = "Streams";
            this.Streams.Padding = new  System.Windows.Forms.Padding(2);
            this.Streams.Size = new  System.Drawing.Size(918, 490);
            this.Streams.TabIndex = 1;
            this.Streams.Text = "Streams";
            this.Streams.UseVisualStyleBackColor = true;
            // 
            // HistoryChart
            // 
            this.HistoryChart.Location = new  System.Drawing.Point (4, 24);
            this.HistoryChart.Margin = new  System.Windows.Forms.Padding(2);
            this.HistoryChart.Name = "HistoryChart";
            this.HistoryChart.Padding = new  System.Windows.Forms.Padding(2);
            this.HistoryChart.Size = new  System.Drawing.Size(918, 490);
            this.HistoryChart.TabIndex = 2;
            this.HistoryChart.Text = "History Chart";
            this.HistoryChart.UseVisualStyleBackColor = true;
            // 
            // HistoryTable
            // 
            this.HistoryTable.Controls.Add(this.dataGridView1);
            this.HistoryTable.Location = new  System.Drawing.Point (4, 24);
            this.HistoryTable.Margin = new  System.Windows.Forms.Padding(2);
            this.HistoryTable.Name = "HistoryTable";
            this.HistoryTable.Padding = new  System.Windows.Forms.Padding(2);
            this.HistoryTable.Size = new  System.Drawing.Size(918, 490);
            this.HistoryTable.TabIndex = 3;
            this.HistoryTable.Text = "History Table";
            this.HistoryTable.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Pressure ,
            this.Temperature ,
            this.MolarEnthalpy,
            this.Flow,
            this.VapourFraction});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new  System.Drawing.Point (2, 2);
            this.dataGridView1.Margin = new  System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new  System.Drawing.Size(914, 486);
            this.dataGridView1.TabIndex = 0;
            // 
            // Pressure 
            // 
            this.Pressure .HeaderText = "Pressure";
            this.Pressure .Name = "Pressure";
            // 
            // Temperature 
            // 
            this.Temperature .HeaderText = "Temperature";
            this.Temperature .Name = "Temperature";
            // 
            // MolarEnthalpy
            // 
            this.MolarEnthalpy.HeaderText = "Molar Enthalpy";
            this.MolarEnthalpy.Name = "MolarEnthalpy";
            // 
            // Flow
            // 
            this.Flow.HeaderText = "Flow";
            this.Flow.Name = "Flow";
            // 
            // VapourFraction
            // 
            this.VapourFraction.HeaderText = "Vapour Fraction";
            this.VapourFraction.Name = "VapourFraction";
            // 
            // portsPropertyWorksheet1
            // 
            this.Worksheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Worksheet.Location = new  System.Drawing.Point (2, 2);
            this.Worksheet.Margin = new  System.Windows.Forms.Padding(2);
            this.Worksheet.Name = "portsPropertyWorksheet1";
            this.Worksheet.Size = new  System.Drawing.Size(914, 486);
            this.Worksheet.TabIndex = 0;
            // 
            // RecycleDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(926, 518);
            this.Controls.Add(this.tabControl1);
            this.Margin = new  System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "RecycleDialog";
            this.Load += new  System.EventHandler(this.RecycleDialog_Load);
            this.tabControl1.ResumeLayout(false);
            this.Summary.ResumeLayout(false);
            this.Summary.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Streams.ResumeLayout(false);
            this.HistoryTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private  TabControl tabControl1;
        private  TabPage Summary;
        private  ComboBox comboBox1;
        private  TabPage Streams;
        private  TabPage HistoryChart;
        private  TabPage HistoryTable;
        private  System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private  DataGridView dataGridView1;
        private  GroupBox groupBox1;
        public  RadioButton rbDirectSubstition;
        public  RadioButton rbWegstein;
        private  DataGridViewTextBoxColumn Pressure ;
        private  DataGridViewTextBoxColumn Temperature ;
        private  DataGridViewTextBoxColumn MolarEnthalpy;
        private  DataGridViewTextBoxColumn Flow;
        private  DataGridViewTextBoxColumn VapourFraction;
        private  CheckBox checkBox1;
        public  RadioButton rbDEM;
        private  PortsPropertyWorksheet Worksheet;
    }
}