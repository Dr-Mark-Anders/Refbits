using   ModelEngine;
using   System.Windows.Forms;
using   Units.PortForm;

namespace   Units
{
    partial class  MixerDialog
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
            this.Worksheet = new  PortsPropertyWorksheet();
            this.Streams = new  System.Windows.Forms.TabPage();
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
            this.tabControl1.Size = new  System.Drawing.Size(796, 451);
            this.tabControl1.TabIndex = 20;
            // 
            // Summary
            // 
            this.Summary.BackColor = System.Drawing.SystemColors.Control;
            this.Summary.Controls.Add(this.comboBox1);
            this.Summary.Location = new  System.Drawing.Point (4, 22);
            this.Summary.Margin = new  System.Windows.Forms.Padding(2);
            this.Summary.Name = "Summary";
            this.Summary.Padding = new  System.Windows.Forms.Padding(2);
            this.Summary.Size = new  System.Drawing.Size(788, 425);
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
            // Worksheet
            // 
            this.Worksheet.AutoSize = true;
            this.Worksheet.Location = new  System.Drawing.Point (2, 4);
            this.Worksheet.Margin = new  System.Windows.Forms.Padding(2);
            this.Worksheet.Name = "Worksheet";
            this.Worksheet.Size = new  System.Drawing.Size(763, 405);
            this.Worksheet.TabIndex = 0;
            // 
            // Streams
            // 
            this.Streams.Controls.Add(this.Worksheet);
            this.Streams.Location = new  System.Drawing.Point (4, 22);
            this.Streams.Margin = new  System.Windows.Forms.Padding(2);
            this.Streams.Name = "Streams";
            this.Streams.Padding = new  System.Windows.Forms.Padding(2);
            this.Streams.Size = new  System.Drawing.Size(788, 425);
            this.Streams.TabIndex = 1;
            this.Streams.Text = "Streams";
            this.Streams.UseVisualStyleBackColor = true;
            // 
            // MixerDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(796, 451);
            this.Controls.Add(this.tabControl1);
            this.Margin = new  System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MixerDialog";
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
        private  PortsPropertyWorksheet Worksheet;
    }
}