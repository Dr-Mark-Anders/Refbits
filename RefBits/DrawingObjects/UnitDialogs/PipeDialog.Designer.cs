using   ModelEngine;
using   System.Windows.Forms;
using   Units.PortForm;

namespace   Units
{
    partial class  PipeDialog
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
            this.uomPumpEfficiency = new  Units.UOMTextBox();
            this.uomPout = new  Units.UOMTextBox();
            this.uomPin = new  Units.UOMTextBox();
            this.label4 = new  System.Windows.Forms.Label();
            this.label3 = new  System.Windows.Forms.Label();
            this.label2 = new  System.Windows.Forms.Label();
            this.label1 = new  System.Windows.Forms.Label();
            this.ScheduleOut = new  System.Windows.Forms.ComboBox();
            this.ScheduleIn = new  System.Windows.Forms.ComboBox();
            this.NomPipeSizeOut = new  System.Windows.Forms.ComboBox();
            this.NomPipeSizeIn = new  System.Windows.Forms.ComboBox();
            this.PumpCurve = new  Units.MiscDialogs.Simple3ColumnDatGridView();
            this.Column7 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DischargeFittings = new  Units.MiscDialogs.Simple2ColumnDatGridView();
            this.Column5 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeedFittings = new  Units.MiscDialogs.Simple2ColumnDatGridView();
            this.Column3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uomPipeDiameterCharge = new  Units.UOMTextBox();
            this.uomLengthCharge = new  Units.UOMTextBox();
            this.uomHeightCharge = new  Units.UOMTextBox();
            this.uomPipeDiamDischarge = new  Units.UOMTextBox();
            this.uomLengthDischarge = new  Units.UOMTextBox();
            this.uomHeightDisch = new  Units.UOMTextBox();
            this.panel1 = new  System.Windows.Forms.Panel();
            this.Streams = new  System.Windows.Forms.TabPage();
            this.Worksheet = new  Units.PortForm.PortsPropertyWorksheet();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PumpCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DischargeFittings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedFittings)).BeginInit();
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
            this.tabControl1.Size = new  System.Drawing.Size(792, 477);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.LightGray;
            this.tabPage1.Controls.Add(this.uomPumpEfficiency);
            this.tabPage1.Controls.Add(this.uomPout);
            this.tabPage1.Controls.Add(this.uomPin);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.ScheduleOut);
            this.tabPage1.Controls.Add(this.ScheduleIn);
            this.tabPage1.Controls.Add(this.NomPipeSizeOut);
            this.tabPage1.Controls.Add(this.NomPipeSizeIn);
            this.tabPage1.Controls.Add(this.PumpCurve);
            this.tabPage1.Controls.Add(this.DischargeFittings);
            this.tabPage1.Controls.Add(this.FeedFittings);
            this.tabPage1.Controls.Add(this.uomPipeDiameterCharge);
            this.tabPage1.Controls.Add(this.uomLengthCharge);
            this.tabPage1.Controls.Add(this.uomHeightCharge);
            this.tabPage1.Controls.Add(this.uomPipeDiamDischarge);
            this.tabPage1.Controls.Add(this.uomLengthDischarge);
            this.tabPage1.Controls.Add(this.uomHeightDisch);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new  System.Drawing.Point (4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new  System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new  System.Drawing.Size(784, 451);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Layout";
            // 
            // uomPumpEfficiency
            // 
            this.uomPumpEfficiency.BackColor = System.Drawing.Color.LightGray;
            this.uomPumpEfficiency.DefaultUnits = "Quality ";
            this.uomPumpEfficiency.DefaultValue = "70";
            this.uomPumpEfficiency.DescriptionWidth = 88;
            this.uomPumpEfficiency.DisplayUnits = "";
            this.uomPumpEfficiency.Label = "Efficiency %";
            this.uomPumpEfficiency.LabelSize = 88;
            this.uomPumpEfficiency.Location = new  System.Drawing.Point (282, 235);
            this.uomPumpEfficiency.Name = "uomPumpEfficiency";
            this.uomPumpEfficiency.Precision = 4;
            this.uomPumpEfficiency.ReadOnly  = false;
            this.uomPumpEfficiency.Size = new  System.Drawing.Size(177, 20);
            this.uomPumpEfficiency.Source = SourceEnum.Input;
            this.uomPumpEfficiency.TabIndex = 47;
            this.uomPumpEfficiency.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomPumpEfficiency.TextBoxHeight = 20;
            this.uomPumpEfficiency.TextBoxSize = 89;
            this.uomPumpEfficiency.UOMprop = ((UOMProperty)(resources.GetObject("uomPumpEfficiency.UOMprop")));
            this.uomPumpEfficiency.UOMType = Units.ePropID.NullUnits;
            this.uomPumpEfficiency.Value = 70D;
            // 
            // uomPout
            // 
            this.uomPout.BackColor = System.Drawing.Color.LightGray;
            this.uomPout.DefaultUnits = "BarA";
            this.uomPout.DefaultValue = "NaN";
            this.uomPout.DescriptionWidth = 88;
            this.uomPout.DisplayUnits = "";
            this.uomPout.Label = "Pressure  Out";
            this.uomPout.LabelSize = 88;
            this.uomPout.Location = new  System.Drawing.Point (397, 7);
            this.uomPout.Name = "uomPout";
            this.uomPout.Precision = 4;
            this.uomPout.ReadOnly  = false;
            this.uomPout.Size = new  System.Drawing.Size(145, 20);
            this.uomPout.Source = SourceEnum.Input;
            this.uomPout.TabIndex = 46;
            this.uomPout.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomPout.TextBoxHeight = 20;
            this.uomPout.TextBoxSize = 50;
            this.uomPout.UOMprop = ((UOMProperty)(resources.GetObject("uomPout.UOMprop")));
            this.uomPout.UOMType = Units.ePropID.P;
            this.uomPout.Value = double.NaN;
            // 
            // uomPin
            // 
            this.uomPin.BackColor = System.Drawing.Color.LightGray;
            this.uomPin.DefaultUnits = "BarA";
            this.uomPin.DefaultValue = "NaN";
            this.uomPin.DescriptionWidth = 88;
            this.uomPin.DisplayUnits = "";
            this.uomPin.Label = "Pressure  In";
            this.uomPin.LabelSize = 88;
            this.uomPin.Location = new  System.Drawing.Point (213, 7);
            this.uomPin.Name = "uomPin";
            this.uomPin.Precision = 4;
            this.uomPin.ReadOnly  = false;
            this.uomPin.Size = new  System.Drawing.Size(145, 20);
            this.uomPin.Source = SourceEnum.Input;
            this.uomPin.TabIndex = 45;
            this.uomPin.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomPin.TextBoxHeight = 20;
            this.uomPin.TextBoxSize = 50;
            this.uomPin.UOMprop = ((UOMProperty)(resources.GetObject("uomPin.UOMprop")));
            this.uomPin.UOMType = Units.ePropID.P;
            this.uomPin.Value = double.NaN;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new  System.Drawing.Point (566, 102);
            this.label4.Name = "label4";
            this.label4.Size = new  System.Drawing.Size(52, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Schedule";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new  System.Drawing.Point (8, 98);
            this.label3.Name = "label3";
            this.label3.Size = new  System.Drawing.Size(52, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Schedule";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new  System.Drawing.Point (566, 75);
            this.label2.Name = "label2";
            this.label2.Size = new  System.Drawing.Size(79, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Nom. Pipe Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new  System.Drawing.Point (8, 74);
            this.label1.Name = "label1";
            this.label1.Size = new  System.Drawing.Size(79, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Nom. Pipe Size";
            // 
            // ScheduleOut
            // 
            this.ScheduleOut.FormattingEnabled = true;
            this.ScheduleOut.Location = new  System.Drawing.Point (688, 100);
            this.ScheduleOut.Name = "ScheduleOut";
            this.ScheduleOut.Size = new  System.Drawing.Size(60, 21);
            this.ScheduleOut.TabIndex = 40;
            this.ScheduleOut.SelectedIndexChanged += new  System.EventHandler(this.ScheduleOut_SelectedIndexChanged);
            // 
            // ScheduleIn
            // 
            this.ScheduleIn.FormattingEnabled = true;
            this.ScheduleIn.Location = new  System.Drawing.Point (129, 100);
            this.ScheduleIn.Name = "ScheduleIn";
            this.ScheduleIn.Size = new  System.Drawing.Size(60, 21);
            this.ScheduleIn.TabIndex = 39;
            this.ScheduleIn.SelectedIndexChanged += new  System.EventHandler(this.ScheduleIn_SelectedIndexChanged);
            // 
            // NomPipeSizeOut
            // 
            this.NomPipeSizeOut.FormattingEnabled = true;
            this.NomPipeSizeOut.Location = new  System.Drawing.Point (688, 74);
            this.NomPipeSizeOut.Name = "NomPipeSizeOut";
            this.NomPipeSizeOut.Size = new  System.Drawing.Size(60, 21);
            this.NomPipeSizeOut.TabIndex = 38;
            this.NomPipeSizeOut.SelectedIndexChanged += new  System.EventHandler(this.NomPipeOut_SelectedIndexChanged);
            // 
            // NomPipeSizeIn
            // 
            this.NomPipeSizeIn.FormattingEnabled = true;
            this.NomPipeSizeIn.Location = new  System.Drawing.Point (129, 73);
            this.NomPipeSizeIn.Name = "NomPipeSizeIn";
            this.NomPipeSizeIn.Size = new  System.Drawing.Size(60, 21);
            this.NomPipeSizeIn.TabIndex = 37;
            this.NomPipeSizeIn.SelectedIndexChanged += new  System.EventHandler(this.NomPipeIn_SelectedIndexChanged);
            // 
            // PumpCurve
            // 
            this.PumpCurve.AllowUserToDeleteRows = false;
            this.PumpCurve.AllowUserToResizeColumns = false;
            this.PumpCurve.AllowUserToResizeRows = false;
            this.PumpCurve.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PumpCurve.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PumpCurve.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column8,
            this.Column9});
            this.PumpCurve.Location = new  System.Drawing.Point (263, 271);
            this.PumpCurve.Name = "PumpCurve";
            this.PumpCurve.RowHeadersVisible = false;
            this.PumpCurve.Size = new  System.Drawing.Size(240, 150);
            this.PumpCurve.TabIndex = 36;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Flow";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Head";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Efficiency";
            this.Column9.Name = "Column9";
            // 
            // DischargeFittings
            // 
            this.DischargeFittings.AllowUserToAddRows = false;
            this.DischargeFittings.AllowUserToDeleteRows = false;
            this.DischargeFittings.AllowUserToResizeColumns = false;
            this.DischargeFittings.AllowUserToResizeRows = false;
            this.DischargeFittings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DischargeFittings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DischargeFittings.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column6});
            this.DischargeFittings.Location = new  System.Drawing.Point (566, 158);
            this.DischargeFittings.Name = "DischargeFittings";
            this.DischargeFittings.RowHeadersVisible = false;
            this.DischargeFittings.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.DischargeFittings.Size = new  System.Drawing.Size(181, 273);
            this.DischargeFittings.TabIndex = 35;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Fittings";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "No.";
            this.Column6.Name = "Column6";
            // 
            // FeedFittings
            // 
            this.FeedFittings.AllowUserToAddRows = false;
            this.FeedFittings.AllowUserToDeleteRows = false;
            this.FeedFittings.AllowUserToResizeColumns = false;
            this.FeedFittings.AllowUserToResizeRows = false;
            this.FeedFittings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.FeedFittings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FeedFittings.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4});
            this.FeedFittings.Location = new  System.Drawing.Point (8, 158);
            this.FeedFittings.Name = "FeedFittings";
            this.FeedFittings.RowHeadersVisible = false;
            this.FeedFittings.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.FeedFittings.Size = new  System.Drawing.Size(181, 273);
            this.FeedFittings.TabIndex = 34;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Fittings";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "No.";
            this.Column4.Name = "Column4";
            // 
            // uomPipeDiameterCharge
            // 
            this.uomPipeDiameterCharge.BackColor = System.Drawing.Color.LightGray;
            this.uomPipeDiameterCharge.DefaultUnits = "m";
            this.uomPipeDiameterCharge.DefaultValue = "NaN";
            this.uomPipeDiameterCharge.DescriptionWidth = 88;
            this.uomPipeDiameterCharge.DisplayUnits = "";
            this.uomPipeDiameterCharge.Label = "Pipe Diameter";
            this.uomPipeDiameterCharge.LabelSize = 88;
            this.uomPipeDiameterCharge.Location = new  System.Drawing.Point (8, 125);
            this.uomPipeDiameterCharge.Name = "uomPipeDiameterCharge";
            this.uomPipeDiameterCharge.Precision = 4;
            this.uomPipeDiameterCharge.ReadOnly  = false;
            this.uomPipeDiameterCharge.Size = new  System.Drawing.Size(181, 20);
            this.uomPipeDiameterCharge.Source = SourceEnum.UnitOpCalcResult;
            this.uomPipeDiameterCharge.TabIndex = 33;
            this.uomPipeDiameterCharge.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomPipeDiameterCharge.TextBoxHeight = 20;
            this.uomPipeDiameterCharge.TextBoxSize = 89;
            this.uomPipeDiameterCharge.UOMprop = ((UOMProperty)(resources.GetObject("uomPipeDiameterCharge.UOMprop")));
            this.uomPipeDiameterCharge.UOMType = Units.ePropID.Length;
            this.uomPipeDiameterCharge.Value = double.NaN;
            // 
            // uomLengthCharge
            // 
            this.uomLengthCharge.BackColor = System.Drawing.Color.LightGray;
            this.uomLengthCharge.DefaultUnits = "m";
            this.uomLengthCharge.DefaultValue = "NaN";
            this.uomLengthCharge.DescriptionWidth = 88;
            this.uomLengthCharge.DisplayUnits = "";
            this.uomLengthCharge.Label = "Pipe Length";
            this.uomLengthCharge.LabelSize = 88;
            this.uomLengthCharge.Location = new  System.Drawing.Point (8, 49);
            this.uomLengthCharge.Name = "uomLengthCharge";
            this.uomLengthCharge.Precision = 4;
            this.uomLengthCharge.ReadOnly  = false;
            this.uomLengthCharge.Size = new  System.Drawing.Size(181, 20);
            this.uomLengthCharge.Source = SourceEnum.Input;
            this.uomLengthCharge.TabIndex = 32;
            this.uomLengthCharge.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomLengthCharge.TextBoxHeight = 20;
            this.uomLengthCharge.TextBoxSize = 89;
            this.uomLengthCharge.UOMprop = ((UOMProperty)(resources.GetObject("uomLengthCharge.UOMprop")));
            this.uomLengthCharge.UOMType = Units.ePropID.Length;
            this.uomLengthCharge.Value = double.NaN;
            // 
            // uomHeightCharge
            // 
            this.uomHeightCharge.BackColor = System.Drawing.Color.LightGray;
            this.uomHeightCharge.DefaultUnits = "m";
            this.uomHeightCharge.DefaultValue = "NaN";
            this.uomHeightCharge.DescriptionWidth = 88;
            this.uomHeightCharge.DisplayUnits = "";
            this.uomHeightCharge.Label = "Static Height";
            this.uomHeightCharge.LabelSize = 88;
            this.uomHeightCharge.Location = new  System.Drawing.Point (8, 23);
            this.uomHeightCharge.Name = "uomHeightCharge";
            this.uomHeightCharge.Precision = 4;
            this.uomHeightCharge.ReadOnly  = false;
            this.uomHeightCharge.Size = new  System.Drawing.Size(181, 20);
            this.uomHeightCharge.Source = SourceEnum.Input;
            this.uomHeightCharge.TabIndex = 31;
            this.uomHeightCharge.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomHeightCharge.TextBoxHeight = 20;
            this.uomHeightCharge.TextBoxSize = 89;
            this.uomHeightCharge.UOMprop = ((UOMProperty)(resources.GetObject("uomHeightCharge.UOMprop")));
            this.uomHeightCharge.UOMType = Units.ePropID.Length;
            this.uomHeightCharge.Value = double.NaN;
            // 
            // uomPipeDiamDischarge
            // 
            this.uomPipeDiamDischarge.BackColor = System.Drawing.Color.LightGray;
            this.uomPipeDiamDischarge.DefaultUnits = "m";
            this.uomPipeDiamDischarge.DefaultValue = "NaN";
            this.uomPipeDiamDischarge.DescriptionWidth = 88;
            this.uomPipeDiamDischarge.DisplayUnits = "";
            this.uomPipeDiamDischarge.Label = "Pipe Diameter";
            this.uomPipeDiamDischarge.LabelSize = 88;
            this.uomPipeDiamDischarge.Location = new  System.Drawing.Point (566, 126);
            this.uomPipeDiamDischarge.Name = "uomPipeDiamDischarge";
            this.uomPipeDiamDischarge.Precision = 4;
            this.uomPipeDiamDischarge.ReadOnly  = false;
            this.uomPipeDiamDischarge.Size = new  System.Drawing.Size(181, 20);
            this.uomPipeDiamDischarge.Source = SourceEnum.UnitOpCalcResult;
            this.uomPipeDiamDischarge.TabIndex = 30;
            this.uomPipeDiamDischarge.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomPipeDiamDischarge.TextBoxHeight = 20;
            this.uomPipeDiamDischarge.TextBoxSize = 89;
            this.uomPipeDiamDischarge.UOMprop = ((UOMProperty)(resources.GetObject("uomPipeDiamDischarge.UOMprop")));
            this.uomPipeDiamDischarge.UOMType = Units.ePropID.Length;
            this.uomPipeDiamDischarge.Value = double.NaN;
            this.uomPipeDiamDischarge.Load += new  System.EventHandler(this.uomPipeDiamDischarge_Load);
            // 
            // uomLengthDischarge
            // 
            this.uomLengthDischarge.BackColor = System.Drawing.Color.LightGray;
            this.uomLengthDischarge.DefaultUnits = "m";
            this.uomLengthDischarge.DefaultValue = "NaN";
            this.uomLengthDischarge.DescriptionWidth = 88;
            this.uomLengthDischarge.DisplayUnits = "";
            this.uomLengthDischarge.Label = "Pipe Length";
            this.uomLengthDischarge.LabelSize = 88;
            this.uomLengthDischarge.Location = new  System.Drawing.Point (566, 48);
            this.uomLengthDischarge.Name = "uomLengthDischarge";
            this.uomLengthDischarge.Precision = 4;
            this.uomLengthDischarge.ReadOnly  = false;
            this.uomLengthDischarge.Size = new  System.Drawing.Size(181, 20);
            this.uomLengthDischarge.Source = SourceEnum.Input;
            this.uomLengthDischarge.TabIndex = 29;
            this.uomLengthDischarge.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomLengthDischarge.TextBoxHeight = 20;
            this.uomLengthDischarge.TextBoxSize = 89;
            this.uomLengthDischarge.UOMprop = ((UOMProperty)(resources.GetObject("uomLengthDischarge.UOMprop")));
            this.uomLengthDischarge.UOMType = Units.ePropID.Length;
            this.uomLengthDischarge.Value = double.NaN;
            // 
            // uomHeightDisch
            // 
            this.uomHeightDisch.BackColor = System.Drawing.Color.LightGray;
            this.uomHeightDisch.DefaultUnits = "m";
            this.uomHeightDisch.DefaultValue = "NaN";
            this.uomHeightDisch.DescriptionWidth = 88;
            this.uomHeightDisch.DisplayUnits = "";
            this.uomHeightDisch.Label = "Static Height";
            this.uomHeightDisch.LabelSize = 88;
            this.uomHeightDisch.Location = new  System.Drawing.Point (566, 22);
            this.uomHeightDisch.Name = "uomHeightDisch";
            this.uomHeightDisch.Precision = 4;
            this.uomHeightDisch.ReadOnly  = false;
            this.uomHeightDisch.Size = new  System.Drawing.Size(181, 20);
            this.uomHeightDisch.Source = SourceEnum.Input;
            this.uomHeightDisch.TabIndex = 28;
            this.uomHeightDisch.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomHeightDisch.TextBoxHeight = 20;
            this.uomHeightDisch.TextBoxSize = 89;
            this.uomHeightDisch.UOMprop = ((UOMProperty)(resources.GetObject("uomHeightDisch.UOMprop")));
            this.uomHeightDisch.UOMType = Units.ePropID.Length;
            this.uomHeightDisch.Value = double.NaN;
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Location = new  System.Drawing.Point (213, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new  System.Drawing.Size(329, 184);
            this.panel1.TabIndex = 27;
            // 
            // Streams
            // 
            this.Streams.Controls.Add(this.Worksheet);
            this.Streams.Location = new  System.Drawing.Point (4, 22);
            this.Streams.Margin = new  System.Windows.Forms.Padding(2);
            this.Streams.Name = "Streams";
            this.Streams.Padding = new  System.Windows.Forms.Padding(2);
            this.Streams.Size = new  System.Drawing.Size(787, 439);
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
            this.Worksheet.Size = new  System.Drawing.Size(785, 419);
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
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(792, 477);
            this.Controls.Add(this.tabControl1);
            this.Margin = new  System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PumpDialog";
            this.Text = "PumpDialog";
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.PumpDialog_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PumpCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DischargeFittings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedFittings)).EndInit();
            this.Streams.ResumeLayout(false);
            this.Streams.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private  TabControl tabControl1;
        private  TabPage Streams;
        private  PortsPropertyWorksheet Worksheet;
        private  TabPage tabPage1;
        private  Panel panel1;
        private  UOMTextBox uomHeightDisch;
        private  UOMTextBox uomLengthCharge;
        private  UOMTextBox uomHeightCharge;
        private  UOMTextBox uomPipeDiamDischarge;
        private  UOMTextBox uomLengthDischarge;
        private  DataGridViewTextBoxColumn Column1;
        private  DataGridViewTextBoxColumn Column2;
        private  MiscDialogs.Simple2ColumnDatGridView FeedFittings;
        private  MiscDialogs.Simple2ColumnDatGridView DischargeFittings;
        private  DataGridViewTextBoxColumn Column5;
        private  DataGridViewTextBoxColumn Column6;
        private  DataGridViewTextBoxColumn Column3;
        private  DataGridViewTextBoxColumn Column4;
        private  MiscDialogs.Simple3ColumnDatGridView PumpCurve;
        private  DataGridViewTextBoxColumn Column7;
        private  DataGridViewTextBoxColumn Column8;
        private  DataGridViewTextBoxColumn Column9;
        private  ComboBox NomPipeSizeOut;
        private  ComboBox NomPipeSizeIn;
        private  UOMTextBox uomPipeDiameterCharge;
        private  ComboBox ScheduleOut;
        private  ComboBox ScheduleIn;
        private  Label label4;
        private  Label label3;
        private  Label label2;
        private  Label label1;
        private  UOMTextBox uomPout;
        private  UOMTextBox uomPin;
        private  UOMTextBox uomPumpEfficiency;
    }
}