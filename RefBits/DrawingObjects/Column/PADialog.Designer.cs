using ModelEngine;
using ModelEngine;
using System.Windows.Forms;
using Units.PortForm;

namespace Units
{
    partial class PADialog
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            Streams = new TabPage();
            Worksheet = new PortsPropertyWorksheet();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            tabControl1.SuspendLayout();
            Streams.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(Streams);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Margin = new Padding(2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(924, 550);
            tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = System.Drawing.SystemColors.Control;
            tabPage1.ImeMode = ImeMode.Off;
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new System.Drawing.Size(916, 522);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "Parameters";
            // 
            // Streams
            // 
            Streams.Controls.Add(Worksheet);
            Streams.Location = new System.Drawing.Point(4, 24);
            Streams.Margin = new Padding(2);
            Streams.Name = "Streams";
            Streams.Padding = new Padding(2);
            Streams.Size = new System.Drawing.Size(916, 522);
            Streams.TabIndex = 1;
            Streams.Text = "Streams";
            Streams.UseVisualStyleBackColor = true;
            // 
            // Worksheet
            // 
            Worksheet.AutoSize = true;
            Worksheet.Dock = DockStyle.Fill;
            Worksheet.Location = new System.Drawing.Point(2, 2);
            Worksheet.Margin = new Padding(2);
            Worksheet.Name = "Worksheet";
            Worksheet.Size = new System.Drawing.Size(912, 518);
            Worksheet.TabIndex = 0;
            // 
            // Column1
            // 
            Column1.HeaderText = "Fitting";
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.HeaderText = "No.";
            Column2.Name = "Column2";
            // 
            // PADialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(924, 550);
            Controls.Add(tabControl1);
            Margin = new Padding(4, 2, 4, 2);
            Name = "PADialog";
            Text = "PumpDialog";
            tabControl1.ResumeLayout(false);
            Streams.ResumeLayout(false);
            Streams.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TabControl tabControl1;
        private TabPage Streams;
        private PortsPropertyWorksheet Worksheet;
        private TabPage tabPage1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
    }
}