using ModelEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>
    ///

    public class UOMTextBox2 : UserControl
    {
        private TextBox TextBox;
        private Label labelbox;
        private string description = "";
        private SourceEnum origin;
        private UOMProperty uomprop = new(ePropID.NullUnits);
        private int sigfigures = 4;
        private ComboBox comboBox1;
        private ePropID property;
        private bool displyunitmode = false;

        public event ValueChangedEventHandler ValueChangedEvent;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        public delegate void MouseLeftEventHandler(object sender, EventArgs e);

        [Browsable(false)]
        [Category("Properties"), Description("UOM")]
        [DisplayName("Value")]
        public string DefaultValue
        {
            get
            {
                return TextBox.Text;
            }
            set
            {
                TextBox.Text = value;
            }
        }

        public ePropID UOMType
        {
            get
            {//
                return property;
            }
            set
            {
                string dispunit = this.DisplayUnits;
                property = value;
                uomprop = new(UOMUtility.GetUOM(value)); // display units overwrritten
                uomprop.DisplayUnit = dispunit;
            }
        }

        [Browsable(false)]
        [Category("Properties"), Description("UOM")]
        [DisplayName("Default Units")]
        public string DefaultUnits
        {
            get
            {
                return uomprop.DefaultUnit;
            }
            set
            {
                uomprop.DefaultUnit = value;
            }
        }

        [Browsable(true)]
        [Category("Properties"), Description("UOM")]
        [DisplayName("Text Width")]
        public int TextBoxSize
        {
            get
            {
                return TextBox.Width;
            }
            set
            {
                TextBox.Width = value;
            }
        }

        [Browsable(true)]
        [Category("Properties"), Description("UOM")]
        [DisplayName("Label Width")]
        public int LabelSize
        {
            get
            {
                return labelbox.Width;
            }
            set
            {
                labelbox.Width = value;
            }
        }

        [Browsable(true)]
        [Category("Properties"), Description("UOM")]
        [DisplayName("Display Units")]
        public string DisplayUnits
        {
            get
            {
                if (uomprop is null)
                    return "";
                return uomprop.DisplayUnit;
            }
            set
            {
                if (uomprop != null)
                    uomprop.DisplayUnit = value;
            }
        }

        private void TextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.TextBox_Validated(sender, new CancelEventArgs());
                    break;

                case Keys.Delete:
                    uomprop.Source = SourceEnum.Empty;
                    uomprop.OriginPortGuid = Guid.Empty;
                    uomprop.BaseValue = double.NaN;
                    TextBox.Text = double.NaN.ToString();
                    TextBox.ReadOnly = false;
                    TextBox.SelectAll();
                    this.Refresh();
                    break;
            }
        }

        public void Bind(UOMProperty newUOM)
        {
            if (origin == SourceEnum.Input)
            {
                newUOM.Value = this.Value;
                newUOM.origin = SourceEnum.Input;
            }

            newUOM.DisplayUnit = this.DisplayUnits; //  dont lose the display units
            this.uomprop = newUOM;
        }

        public double Value
        {
            get
            {
                if (this.uomprop is null)
                    return double.NaN;
                return this.uomprop.DisplayValueOut();
            }
            set
            {
                this.uomprop.DisplayValueIn(value);
            }
        }

        public SourceEnum Source
        {
            get { return origin; }
            set { origin = value; }
        }

        public UOMTextBox2()
        {
            InitializeComponent();
            comboBox1.Visible = false;
        }

        public UOMTextBox2(UOMProperty d)
        {
            InitializeComponent();
            uomprop = d;
            UOMType = d.Propid;
        }

        public UOMTextBox2(ePropID property)
        {
            InitializeComponent();
            UOMType = property;
        }

        public int Precision
        {
            get
            {
                return sigfigures;
            }
            set
            {
                sigfigures = value;
            }
        }

        public string Label
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public int DescriptionWidth
        {
            get
            {
                return labelbox.Width;
            }
            set
            {
                this.labelbox.Width = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.TextBox.ReadOnly;
            }
            set
            {
                this.TextBox.ReadOnly = value;
                this.comboBox1.Visible = false;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing) { }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelbox = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.TextBox = new Units.FlexTextBox();
            this.SuspendLayout();
            //
            // labelbox
            //
            this.labelbox.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelbox.Location = new System.Drawing.Point(0, 0);
            this.labelbox.Name = "labelbox";
            this.labelbox.Size = new System.Drawing.Size(64, 20);
            this.labelbox.TabIndex = 5;
            this.labelbox.Text = "Label";
            this.labelbox.DoubleClick += new System.EventHandler(this.description_doubleClick);
            //
            // comboBox1
            //
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(100, -1);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(45, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.Visible = false;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            this.comboBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox1_KeyPress);
            this.comboBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ComboBoxBox_MouseDown);
            this.comboBox1.MouseLeave += new System.EventHandler(this.comboBox1_MouseLeave);
            //
            // TextBox
            //
            this.TextBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.TextBox.Location = new System.Drawing.Point(56, 0);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(89, 20);
            this.TextBox.TabIndex = 3;
            this.TextBox.Text = "Undefined";
            this.TextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            this.TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            this.TextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TextBox_MousedoubleClick);
            this.TextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBox_MouseDown);
            this.TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
            this.TextBox.Validated += new System.EventHandler(this.TextBox_Validated);
            //
            // UOMTextBox
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.TextBox);
            this.Controls.Add(this.labelbox);
            this.Name = "UOMTextBox";
            this.Size = new System.Drawing.Size(145, 20);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.LabelledDimTextBox_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected virtual void RaiseValueChangedEvent(EventArgs e)
        {
            ValueChangedEvent?.Invoke(this, e);
        }

        public void SelectText()
        {
            TextBox.SelectAll();
        }

        private void LabelledDimTextBox_Paint(object sender, PaintEventArgs e)
        {
            if (uomprop is UOMProperty prop)
            {
                if (double.IsNaN(Value))
                {
                    TextBox.Text = double.NaN.ToString();
                }
                else
                {
                    if (prop.DisplayValueOut() > 10000)
                        TextBox.Text = prop.DisplayValueOut().ToString("0.###E-000");
                    else if (prop.DisplayValueOut() < 0.01)
                        TextBox.Text = prop.DisplayValueOut().ToString("0.###E-000");
                    else
                        TextBox.Text = prop.DisplayValueOut().ToString("0.##");
                }

                if (prop.Source == SourceEnum.Input ||
                     prop.Source == SourceEnum.Empty ||
                     prop.Source == SourceEnum.Default)
                {
                    ReadOnly = false;
                }
                else
                {
                    ReadOnly = true;
                }
            }

            labelbox.Text = description + ", " + DisplayUnits;
        }

        public new Font Font
        {
            get
            {
                return TextBox.Font;
            }
            set
            {
                comboBox1.Font = value;
                labelbox.Font = value;
            }
        }

        public Font TextBoxFont
        {
            get
            {
                return TextBox.Font;
            }
            set
            {
                TextBox.Font = value;
            }
        }

        public int TextBoxHeight
        {
            get
            {
                return TextBox.Height;
            }
            set
            {
                TextBox.Height = value;
            }
        }

        [Browsable(false)]
        public UOMProperty UOMprop { get => uomprop; set => uomprop = value; }

        //public  ePropID Property { get => property; set => property = value; }

        private void TextBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            TextBox.ReadOnly = false;

            if (uomprop is null)
                return;

            switch (uomprop.Source)
            {
                case SourceEnum.Input:
                case SourceEnum.Empty:
                case SourceEnum.CalcEstimate:
                case SourceEnum.FixedEstimate:
                    TextBox.SelectAll();
                    comboBox1.Top = TextBox.Top;
                    comboBox1.Left = TextBox.Right - comboBox1.Width;
                    comboBox1.Visible = true;
                    break;

                case SourceEnum.UnitOpCalcResult:
                case SourceEnum.Transferred:
                    TextBox.ReadOnly = true;
                    break;

                case SourceEnum.Default:
                    TextBox.ReadOnly = true;
                    break;

                default:
                    break;
            }
        }

        private void TextBox_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            TextBox.SelectAll();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string ChosenUnit = (string)comboBox1.SelectedItem;

            if (displyunitmode)
            {
                DisplayUnits = ChosenUnit;
                comboBox1.Visible = false;
            }
            else
            {
                if (double.TryParse(TextBox.Text, out double res))
                {
                    uomprop.ValueIn(ChosenUnit, res);
                    TextBox.Text = uomprop.DisplayValueOut().ToString();
                }
                else
                    TextBox.Text = double.NaN.ToString();

                comboBox1.Visible = false;
                TextBox.SelectAll();
            }
        }

        private void ComboBoxBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(uomprop.AllUnits);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TextBox.Text, out double res))
            {
                uomprop.DisplayValueIn(res);
            }
        }

        private void TextBox_MousedoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Left = TextBox.Right;
            comboBox1.Items.AddRange(uomprop.AllUnits);
            comboBox1.Visible = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
        }

        private void description_doubleClick(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Left = labelbox.Right;
            comboBox1.Items.AddRange(uomprop.AllUnits);
            comboBox1.Visible = true;
            displyunitmode = true;
        }

        private void comboBox1_MouseLeave(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    comboBox1.Visible = false;
                    break;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    comboBox1.Visible = false;
                    break;
            }
        }

        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (double.IsNaN(uomprop.Value))
                uomprop.origin = SourceEnum.Empty;
            else
                uomprop.origin = SourceEnum.Input;

            switch (uomprop.origin)
            {
                case SourceEnum.UnitOpCalcResult:
                    TextBox.ReadOnly = true;
                    break;

                default:
                    TextBox.ReadOnly = false;
                    break;
            }
        }

        private void TextBox_Validated(object sender, EventArgs e)
        {
            BaseDialog parent = (BaseDialog)this.ParentForm;
            parent.RaiseValueChangedEvent(e);
        }
    }
}