using System.Windows.Forms;
using Units.PortForm;

namespace Units
{
    partial class PlugFlowDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlugFlowDialog));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            userPropGrid1 = new FormControls.UserPropGrid();
            button1 = new Button();
            uomPin = new UOMTextBox();
            label1 = new Label();
            NomPipeSizeIn = new ComboBox();
            uomLengthCharge = new UOMTextBox();
            uomHeightCharge = new UOMTextBox();
            Streams = new TabPage();
            Worksheet = new PortsPropertyWorksheet();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
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
            tabPage1.BackColor = System.Drawing.SystemColors.ControlLight;
            tabPage1.Controls.Add(userPropGrid1);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(uomPin);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(NomPipeSizeIn);
            tabPage1.Controls.Add(uomLengthCharge);
            tabPage1.Controls.Add(uomHeightCharge);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new System.Drawing.Size(916, 522);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "Layout";
            // 
            // userPropGrid1
            // 
            userPropGrid1.AllowChangeEvent = true;
            userPropGrid1.AllowUserToAddRows = false;
            userPropGrid1.AllowUserToDeleteRows = false;
            userPropGrid1.BackColor = System.Drawing.SystemColors.ControlLight;
            userPropGrid1.ColumnNames = (System.Collections.Generic.List<string>)resources.GetObject("userPropGrid1.ColumnNames");
            userPropGrid1.DisplayTitles = true;
            userPropGrid1.FirstColumnWidth = 64;
            userPropGrid1.Location = new System.Drawing.Point(547, 57);
            userPropGrid1.Margin = new Padding(4, 3, 4, 3);
            userPropGrid1.Name = "userPropGrid1";
            userPropGrid1.ReadOnly = false;
            userPropGrid1.RowHeadersVisible = false;
            userPropGrid1.RowNames = (System.Collections.Generic.List<string>)resources.GetObject("userPropGrid1.RowNames");
            userPropGrid1.Size = new System.Drawing.Size(253, 353);
            userPropGrid1.TabIndex = 48;
            userPropGrid1.TopText = "gpBox";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(547, 17);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(107, 33);
            button1.TabIndex = 46;
            button1.Text = "Define Reactions";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Rxns_Click;
            // 
            // uomPin
            // 
            uomPin.BackColor = System.Drawing.SystemColors.ControlLight;
            uomPin.ComboBoxHeight = 23;
            uomPin.ComboBoxVisible = false;
            uomPin.ComboBoxWidth = 69;
            uomPin.DefaultUnits = "BarA";
            uomPin.DefaultValue = "NaN";
            uomPin.DescriptionWidth = 103;
            uomPin.DisplayUnitArray = new string[] { "BarA", "MPa", "KPa", "MMHga", "PSIA", "BarG", "MPaG", "KPaG", "MMHgG", "PSIG", "Kg_cm2_g", "Kg_cm2", "atmg", "atma" };
            uomPin.DisplayUnits = "BarA";
            uomPin.Label = "Pressure  In";
            uomPin.LabelSize = 103;
            uomPin.Location = new System.Drawing.Point(248, 8);
            uomPin.Margin = new Padding(4, 3, 4, 3);
            uomPin.Name = "uomPin";
            uomPin.Precision = 4;
            uomPin.ReadOnly = false;
            uomPin.Size = new System.Drawing.Size(169, 23);
            uomPin.Source = SourceEnum.Input;
            uomPin.TabIndex = 45;
            uomPin.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            uomPin.TextBoxHeight = 20;
            uomPin.TextBoxLeft = 103;
            uomPin.TextBoxSize = 66;
            uomPin.UOMprop = (ModelEngine.UOMProperty)resources.GetObject("uomPin.UOMprop");
            uomPin.UOMType = ePropID.P;
            uomPin.Value = double.NaN;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 85);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(98, 15);
            label1.TabIndex = 41;
            label1.Text = "Reactor Diameter";
            // 
            // NomPipeSizeIn
            // 
            NomPipeSizeIn.FormattingEnabled = true;
            NomPipeSizeIn.Location = new System.Drawing.Point(150, 84);
            NomPipeSizeIn.Margin = new Padding(4, 3, 4, 3);
            NomPipeSizeIn.Name = "NomPipeSizeIn";
            NomPipeSizeIn.Size = new System.Drawing.Size(69, 23);
            NomPipeSizeIn.TabIndex = 37;
            // 
            // uomLengthCharge
            // 
            uomLengthCharge.BackColor = System.Drawing.SystemColors.ControlLight;
            uomLengthCharge.ComboBoxHeight = 23;
            uomLengthCharge.ComboBoxVisible = false;
            uomLengthCharge.ComboBoxWidth = 69;
            uomLengthCharge.DefaultUnits = "m";
            uomLengthCharge.DefaultValue = "NaN";
            uomLengthCharge.DescriptionWidth = 103;
            uomLengthCharge.DisplayUnitArray = new string[] { "m", "cm", "mm" };
            uomLengthCharge.DisplayUnits = "m";
            uomLengthCharge.Label = "Reactor Length";
            uomLengthCharge.LabelSize = 103;
            uomLengthCharge.Location = new System.Drawing.Point(9, 57);
            uomLengthCharge.Margin = new Padding(4, 3, 4, 3);
            uomLengthCharge.Name = "uomLengthCharge";
            uomLengthCharge.Precision = 4;
            uomLengthCharge.ReadOnly = false;
            uomLengthCharge.Size = new System.Drawing.Size(211, 23);
            uomLengthCharge.Source = SourceEnum.Input;
            uomLengthCharge.TabIndex = 32;
            uomLengthCharge.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            uomLengthCharge.TextBoxHeight = 20;
            uomLengthCharge.TextBoxLeft = 103;
            uomLengthCharge.TextBoxSize = 108;
            uomLengthCharge.UOMprop = (ModelEngine.UOMProperty)resources.GetObject("uomLengthCharge.UOMprop");
            uomLengthCharge.UOMType = ePropID.Length;
            uomLengthCharge.Value = double.NaN;
            // 
            // uomHeightCharge
            // 
            uomHeightCharge.BackColor = System.Drawing.SystemColors.ControlLight;
            uomHeightCharge.ComboBoxHeight = 23;
            uomHeightCharge.ComboBoxVisible = false;
            uomHeightCharge.ComboBoxWidth = 69;
            uomHeightCharge.DefaultUnits = "m";
            uomHeightCharge.DefaultValue = "NaN";
            uomHeightCharge.DescriptionWidth = 103;
            uomHeightCharge.DisplayUnitArray = new string[] { "m", "cm", "mm" };
            uomHeightCharge.DisplayUnits = "m";
            uomHeightCharge.Label = "Static Height";
            uomHeightCharge.LabelSize = 103;
            uomHeightCharge.Location = new System.Drawing.Point(9, 27);
            uomHeightCharge.Margin = new Padding(4, 3, 4, 3);
            uomHeightCharge.Name = "uomHeightCharge";
            uomHeightCharge.Precision = 4;
            uomHeightCharge.ReadOnly = false;
            uomHeightCharge.Size = new System.Drawing.Size(211, 23);
            uomHeightCharge.Source = SourceEnum.Input;
            uomHeightCharge.TabIndex = 31;
            uomHeightCharge.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            uomHeightCharge.TextBoxHeight = 20;
            uomHeightCharge.TextBoxLeft = 103;
            uomHeightCharge.TextBoxSize = 108;
            uomHeightCharge.UOMprop = (ModelEngine.UOMProperty)resources.GetObject("uomHeightCharge.UOMprop");
            uomHeightCharge.UOMType = ePropID.Length;
            uomHeightCharge.Value = double.NaN;
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
            Worksheet.Location = new System.Drawing.Point(2, 0);
            Worksheet.Margin = new Padding(2);
            Worksheet.Name = "Worksheet";
            Worksheet.Size = new System.Drawing.Size(912, 522);
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
            // PlugFlowDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(924, 550);
            Controls.Add(tabControl1);
            Margin = new Padding(4, 2, 4, 2);
            Name = "PlugFlowDialog";
            Text = "PumpDialog";
            FormClosing += PlusFlowDialog_FormClosing;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            Streams.ResumeLayout(false);
            Streams.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TabControl tabControl1;
        private TabPage Streams;
        private PortsPropertyWorksheet Worksheet;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private TabPage tabPage1;
        private UOMTextBox uomPin;
        private Label label1;
        private ComboBox NomPipeSizeIn;
        private UOMTextBox uomLengthCharge;
        private UOMTextBox uomHeightCharge;
        private FormControls.UserPropGrid userPropGrid1;
        private Button button1;
    }
}