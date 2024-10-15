using   ModelEngine;
using   ModelEngine;
using   System.Windows.Forms;
using   Units.PortForm;

namespace   Units
{
    partial class  PumpDialog
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
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(PumpDialog));
            this.tabControl1 = new  System.Windows.Forms.TabControl();
            this.tabPage1 = new  System.Windows.Forms.TabPage();
            this.Parameters = new  FormControls.UserPropGrid();
            this.panel1 = new  System.Windows.Forms.Panel();
            this.uomPumpEfficiency = new  Units.UOMTextBox();
            this.uomPout = new  Units.UOMTextBox();
            this.uomPin = new  Units.UOMTextBox();
            this.pictureBox1 = new  System.Windows.Forms.PictureBox();
            this.Streams = new  System.Windows.Forms.TabPage();
            this.Worksheet = new  Units.PortForm.PortsPropertyWorksheet();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Streams.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.Streams);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new  System.Drawing.Point (0, 0);
            this.tabControl1.Margin = new  System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new  System.Drawing.Size(924, 550);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.Parameters);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new  System.Drawing.Point (4, 24);
            this.tabPage1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Size = new  System.Drawing.Size(916, 522);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Parameters";
            // 
            // Parameters
            // 
            this.Parameters.AllowUserToAddRows = false;
            this.Parameters.AllowUserToDeleteRows = false;
            this.Parameters.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Parameters.ColumnNames")));
            this.Parameters.DisplayTitles = true;
            this.Parameters.FirstColumnWidth = 64;
            this.Parameters.Location = new  System.Drawing.Point (9, 6);
            this.Parameters.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Parameters.Name = "Parameters";
            this.Parameters.ReadOnly  = false;
            this.Parameters.RowHeadersVisible = false;
            this.Parameters.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Parameters.RowNames")));
            this.Parameters.Size = new  System.Drawing.Size(216, 307);
            this.Parameters.TabIndex = 28;
            this.Parameters.TopText = "Pump Data";
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Controls.Add(this.uomPumpEfficiency);
            this.panel1.Controls.Add(this.uomPout);
            this.panel1.Controls.Add(this.uomPin);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new  System.Drawing.Point (282, 26);
            this.panel1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new  System.Drawing.Size(571, 451);
            this.panel1.TabIndex = 27;
            // 
            // uomPumpEfficiency
            // 
            this.uomPumpEfficiency.BackColor = System.Drawing.SystemColors.Control;
            this.uomPumpEfficiency.ComboBoxHeight = 23;
            this.uomPumpEfficiency.ComboBoxVisible = false;
            this.uomPumpEfficiency.ComboBoxWidth = 69;
            this.uomPumpEfficiency.DefaultUnits = "Quality ";
            this.uomPumpEfficiency.DefaultValue = "70";
            this.uomPumpEfficiency.DescriptionWidth = 120;
            this.uomPumpEfficiency.DisplayUnitArray = new  string[] {
        "Quality "};
            this.uomPumpEfficiency.DisplayUnits = "Quality ";
            this.uomPumpEfficiency.Label = "Efficiency %";
            this.uomPumpEfficiency.LabelSize = 120;
            this.uomPumpEfficiency.Location = new  System.Drawing.Point (149, 317);
            this.uomPumpEfficiency.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.uomPumpEfficiency.Name = "uomPumpEfficiency";
            this.uomPumpEfficiency.Precision = 4;
            this.uomPumpEfficiency.ReadOnly  = false;
            this.uomPumpEfficiency.Size = new  System.Drawing.Size(206, 23);
            this.uomPumpEfficiency.Source = SourceEnum.Input;
            this.uomPumpEfficiency.TabIndex = 47;
            this.uomPumpEfficiency.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.uomPumpEfficiency.TextBoxHeight = 20;
            this.uomPumpEfficiency.TextBoxLeft = 120;
            this.uomPumpEfficiency.TextBoxSize = 86;
            this.uomPumpEfficiency.UOMprop = ((ModelEngine.UOMProperty)(resources.GetObject("uomPumpEfficiency.UOMprop")));
            this.uomPumpEfficiency.UOMType = Units.ePropID.NullUnits;
            this.uomPumpEfficiency.Value = 70D;
            // 
            // uomPout
            // 
            this.uomPout.BackColor = System.Drawing.SystemColors.Control;
            this.uomPout.ComboBoxHeight = 23;
            this.uomPout.ComboBoxVisible = false;
            this.uomPout.ComboBoxWidth = 69;
            this.uomPout.DefaultUnits = "BarA";
            this.uomPout.DefaultValue = "NaN";
            this.uomPout.DescriptionWidth = 120;
            this.uomPout.DisplayUnitArray = new  string[] {
        "BarA",
        "MPa",
        "KPa",
        "MMHga",
        "PSIA",
        "BarG",
        "MPaG",
        "KPaG",
        "MMHgG",
        "PSIG",
        "Kg_cm2_g",
        "Kg_cm2"};
            this.uomPout.DisplayUnits = "BarA";
            this.uomPout.Label = "Pressure  Out";
            this.uomPout.LabelSize = 120;
            this.uomPout.Location = new  System.Drawing.Point (335, 103);
            this.uomPout.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.uomPout.Name = "uomPout";
            this.uomPout.Precision = 4;
            this.uomPout.ReadOnly  = false;
            this.uomPout.Size = new  System.Drawing.Size(206, 23);
            this.uomPout.Source = SourceEnum.Input;
            this.uomPout.TabIndex = 46;
            this.uomPout.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.uomPout.TextBoxHeight = 20;
            this.uomPout.TextBoxLeft = 120;
            this.uomPout.TextBoxSize = 86;
            this.uomPout.UOMprop = ((ModelEngine.UOMProperty)(resources.GetObject("uomPout.UOMprop")));
            this.uomPout.UOMType = Units.ePropID.P;
            this.uomPout.Value = double.NaN;
            // 
            // uomPin
            // 
            this.uomPin.BackColor = System.Drawing.SystemColors.Control;
            this.uomPin.ComboBoxHeight = 23;
            this.uomPin.ComboBoxVisible = false;
            this.uomPin.ComboBoxWidth = 69;
            this.uomPin.DefaultUnits = "BarA";
            this.uomPin.DefaultValue = "NaN";
            this.uomPin.DescriptionWidth = 120;
            this.uomPin.DisplayUnitArray = new  string[] {
        "BarA",
        "MPa",
        "KPa",
        "MMHga",
        "PSIA",
        "BarG",
        "MPaG",
        "KPaG",
        "MMHgG",
        "PSIG",
        "Kg_cm2_g",
        "Kg_cm2"};
            this.uomPin.DisplayUnits = "BarA";
            this.uomPin.Label = "Pressure  In";
            this.uomPin.LabelSize = 120;
            this.uomPin.Location = new  System.Drawing.Point (4, 136);
            this.uomPin.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.uomPin.Name = "uomPin";
            this.uomPin.Precision = 4;
            this.uomPin.ReadOnly  = false;
            this.uomPin.Size = new  System.Drawing.Size(206, 23);
            this.uomPin.Source = SourceEnum.Input;
            this.uomPin.TabIndex = 45;
            this.uomPin.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.uomPin.TextBoxHeight = 20;
            this.uomPin.TextBoxLeft = 120;
            this.uomPin.TextBoxSize = 86;
            this.uomPin.UOMprop = ((ModelEngine.UOMProperty)(resources.GetObject("uomPin.UOMprop")));
            this.uomPin.UOMType = Units.ePropID.P;
            this.uomPin.Value = double.NaN;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new  System.Drawing.Point (0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new  System.Drawing.Size(571, 451);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Streams
            // 
            this.Streams.Controls.Add(this.Worksheet);
            this.Streams.Location = new  System.Drawing.Point (4, 24);
            this.Streams.Margin = new  System.Windows.Forms.Padding(2);
            this.Streams.Name = "Streams";
            this.Streams.Padding = new  System.Windows.Forms.Padding(2);
            this.Streams.Size = new  System.Drawing.Size(916, 522);
            this.Streams.TabIndex = 1;
            this.Streams.Text = "Streams";
            this.Streams.UseVisualStyleBackColor = true;
            // 
            // Worksheet
            // 
            this.Worksheet.AutoSize = true;
            this.Worksheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Worksheet.Location = new  System.Drawing.Point (2, 2);
            this.Worksheet.Margin = new  System.Windows.Forms.Padding(2);
            this.Worksheet.Name = "Worksheet";
            this.Worksheet.Size = new  System.Drawing.Size(912, 518);
            this.Worksheet.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Fitting";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "No.";
            this.Column2.Name = "Column2";
            // 
            // PumpDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(924, 550);
            this.Controls.Add(this.tabControl1);
            this.Margin = new  System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "PumpDialog";
            this.Text = "PumpDialog";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Streams.ResumeLayout(false);
            this.Streams.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private  TabControl tabControl1;
        private  TabPage Streams;
        private  PortsPropertyWorksheet Worksheet;
        private  TabPage tabPage1;
        private  DataGridViewTextBoxColumn Column1;
        private  DataGridViewTextBoxColumn Column2;
        private  UOMTextBox uomPumpEfficiency;
        private  UOMTextBox uomPout;
        private  UOMTextBox uomPin;
        private  Panel panel1;
        private  PictureBox pictureBox1;
        private  FormControls.UserPropGrid Parameters;
    }
}