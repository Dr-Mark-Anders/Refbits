using ModelEngine;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>
    ///

    public class DimTextBox : UserControl
    {
        private UOMProperty value;
        private ePropID property;
        private UnitDelegate ConvFunc;
        private IContainer Components;
        private Splitter splitter1;
        private Label UnitsDisplay;
        private ErrorProvider errorProvider1;
        public FlexTextBox textBox;
        private ComboBox dropdownbox;
        private int sigfigures = 4;
        private int textbxwidth = 60;
        private int ddbxleft = 60;
        private int ddbxwidth = 80;

        public event KeyEventHandler DTextKeyDownEvent;

        public event ValueChangedEventHandler ValueChangedEvent;

        public event MouseLeftEventHandler MouseLeftEvent;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        public delegate void MouseLeftEventHandler(object sender, EventArgs e);

        public UOMProperty Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.textBox.Text = Math.Round(value.DisplayValueOut(), sigfigures).ToString();
                this.value = value;
            }
        }

        public ArrayList DimList
        {
            get
            {
                object o = new object();
                string[] a = new string[] { };
                if (this.ConvFunc != null)
                    this.ConvFunc(1, "", false, ref a, o);
                return new ArrayList(a);
            }
        }

        public string GetDefaultUnit
        {
            get
            {
                return (string)DimList[0];
            }
        }

        public bool IsEmpty
        {
            get { return value.Source == SourceEnum.Empty; }
        }

        public bool IsSpecified
        {
            get { return value.Source == SourceEnum.Input; }
        }

        public SourceEnum origin
        {
            get { return value.Source; }
        }

        public bool IsCalcResult
        {
            get { return value.Source == SourceEnum.UnitOpCalcResult; }
        }

        public int Textbxwidth
        {
            get { return textbxwidth; }
            set { textbxwidth = value; }
        }

        public int Ddbxleft
        {
            get { return ddbxleft; }
            set { ddbxleft = value; }
        }

        public int Ddbxwidth
        {
            get { return ddbxwidth; }
            set { ddbxwidth = value; }
        }

        public ePropID Property
        {
            get
            {
                return value.Property;
            }
            set
            {
                this.value = new UOMProperty(UOMUtility.GetUOM(value));
            }
        }

        public string UnitList
        {
            get
            {
                return value.DisplayUnit;
            }
            set
            {
                this.value.DisplayUnit = value;
            }
        }

        public DimTextBox()
        {
            InitializeComponent();
            ConvFunc = UnitConv.NullUnitDelegate;
            dropdownbox.Left = textbxwidth;
            dropdownbox.Width = this.Width - textbxwidth;
        }

        public void SetValues(UOMProperty dd)
        {
            if (dd != null)
            {
                value = dd;
            }
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

        public int TextWidth
        {
            get
            {
                return this.textBox.Width;
            }
            set
            {
                this.textBox.Width = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.textBox.ReadOnly;
            }
            set
            {
                this.textBox.ReadOnly = value;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Components != null)
                {
                    Components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Components = new System.ComponentModel.Container();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.UnitsDisplay = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.Components);
            this.dropdownbox = new System.Windows.Forms.ComboBox();
            this.textBox = new Units.FlexTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // splitter1
            //
            this.splitter1.Location = new System.Drawing.Point(41, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 25);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            //
            // UnitsDisplay
            //
            this.UnitsDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UnitsDisplay.Location = new System.Drawing.Point(45, 0);
            this.UnitsDisplay.Name = "UnitsDisplay";
            this.UnitsDisplay.Size = new System.Drawing.Size(70, 25);
            this.UnitsDisplay.TabIndex = 2;
            this.UnitsDisplay.Text = "NA";
            this.UnitsDisplay.Click += new System.EventHandler(this.label1_Click);
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.DataMember = "";
            //
            // dropdownbox
            //
            this.dropdownbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropdownbox.FormattingEnabled = true;
            this.dropdownbox.Location = new System.Drawing.Point(121, 1);
            this.dropdownbox.Name = "dropdownbox";
            this.dropdownbox.Size = new System.Drawing.Size(98, 21);
            this.dropdownbox.TabIndex = 4;
            this.dropdownbox.Visible = false;
            this.dropdownbox.SelectedIndexChanged += new System.EventHandler(this.dd_SelectedIndexChanged);
            this.dropdownbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dropdownbox_KeyDown);
            //
            // textBox
            //
            this.textBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(41, 20);
            this.textBox.TabIndex = 3;
            this.textBox.TextIfBlank = "";
            this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.textBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            //
            // DimTextBox
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.dropdownbox);
            this.Controls.Add(this.UnitsDisplay);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.textBox);
            this.Name = "DimTextBox";
            this.Size = new System.Drawing.Size(313, 25);
            this.SizeChanged += new System.EventHandler(this.DText_SizeChanged);
            this.MouseLeave += new System.EventHandler(this.DTextBox_MouseLeave);
            this.Resize += new System.EventHandler(this.DText_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Component Designer generated code

        private void label1_Click(object sender, System.EventArgs e)
        {
            if (value.Source == SourceEnum.Empty || value.Source == SourceEnum.Input)
            {
                dropdownbox.Location = new System.Drawing.Point(this.Left + UnitsDisplay.Left, this.Top);
                dropdownbox.Name = "comboBox1";
                dropdownbox.TabIndex = 4;
                dropdownbox.Text = "comboBox1";
                dropdownbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                dropdownbox.DataSource = this.DimList;
                dropdownbox.Visible = true;
                dropdownbox.SelectionChangeCommitted += new EventHandler(dd_select);
                dropdownbox.MouseLeave += new EventHandler(dd_mouseleave);
                this.Parent.Controls.Add(dropdownbox);
                this.Parent.Controls.SetChildIndex(dropdownbox, 0);
            }
        }

        public bool DropDownVisible
        {
            get
            {
                return dropdownbox.Visible;
            }
            set
            {
                dropdownbox.Visible = true;
            }
        }

        public ePropID Property1 { get => property; set => property = value; }

        private void DText_Resize(object sender, System.EventArgs e)
        {
            textBox.Width = textbxwidth;
            UnitsDisplay.Left = textbxwidth;
            UnitsDisplay.Width = this.Width - textbxwidth;
            dropdownbox.Left = textbxwidth;
            dropdownbox.Width = this.Width - textbxwidth;
        }

        private void DText_SizeChanged(object sender, EventArgs e)
        {
            textBox.Width = textbxwidth;
            UnitsDisplay.Left = textbxwidth;
            UnitsDisplay.Width = this.Width - textbxwidth;
            dropdownbox.Left = textbxwidth;
            dropdownbox.Width = this.Width - textbxwidth;
        }

        private void dd_select(object sender, EventArgs e)
        {
            dropdownbox.Visible = false;
            this.UnitsDisplay.Text = (string)DimList[dropdownbox.SelectedIndex];
            this.textBox.Select();
        }

        private void dd_mouseleave(object sender, EventArgs e)
        {
            dropdownbox.Visible = false;
        }

        private void MyValidatingCode(TextBox sender)
        {
            // Confirm there is text in the control.
            try
            {
                Convert.ToDouble(sender.Text);
                this.Value.ValueIn(dropdownbox.Text, Convert.ToDouble(sender.Text));
            }
            catch
            {
                throw new Exception("Value is not a number");
            }
        }

        private void textBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                MyValidatingCode(tb);
            }
            catch (Exception ex)
            {
                UnitsDisplay.Visible = false;
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                tb.Select(0, tb.Text.Length);

                // Set the ErrorProvider error with the text to display.
                this.errorProvider1.SetError(tb, ex.Message);
            }
        }

        private void dd_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tempunits;
            tempunits = (string)dropdownbox.SelectedItem;   // Convert value in text box to default units
            if (double.TryParse(textBox.Text, out double Res))
                Value.ValueIn(tempunits, Res);
            //RaiseKeyEnterEvent(new  KeyEventArgs(Keys.Enter));
        }

        private void dropdownbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                RaiseKeyEnterEvent(e);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double res = 0;
                //textBox.Visible = false;
                if (double.TryParse(textBox.Text, out res))
                {
                    Value.ValueIn(value.DisplayUnit, res);
                }
                else
                {
                    value.BaseValue = double.NaN;
                }

                this.Visible = false;
                RaiseValueChangedEvent(e);
            }
        }

        protected virtual void RaiseKeyEnterEvent(KeyEventArgs e)
        {
            dropdownbox.Visible = false;
            // Raise the event by using   the () operator.
            if (DTextKeyDownEvent != null)
                DTextKeyDownEvent(this, e);
        }

        protected virtual void RaiseValueChangedEvent(EventArgs e)
        {
            if (ValueChangedEvent != null)
                ValueChangedEvent(this, e);
        }

        protected virtual void RaiseMouseLeftEvent(EventArgs e)
        {
            if (MouseLeftEvent != null)
                MouseLeftEvent(this, e);
        }

        public void Update(UOMProperty dd)
        {
            this.Value.BaseValue = dd.BaseValue;
            //this.ConvFunc = dd.ConvFunc;
            this.dropdownbox.DataSource = this.DimList;
            dropdownbox.Text = dd.DisplayUnit;
        }

        public void SelectText()
        {
            textBox.SelectAll();
        }

        private void DTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (MouseLeftEvent != null)
                MouseLeftEvent(this, e);
        }

        public static implicit operator UOMProperty(DimTextBox dt)
        {
            UOMProperty res = (UOMProperty)dt.value.Clone();
            res.Source = dt.origin;
            return res;
        }
    }
}