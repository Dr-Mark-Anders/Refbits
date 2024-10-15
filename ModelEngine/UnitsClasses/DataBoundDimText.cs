using ModelEngine;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Units.UOM;

namespace Units
{
    /// <summary>
    ///     Summary description for UserControl1.
    /// </summary>
    public class DataBoundDimTextBox : UserControl
    {
        public delegate void MouseLeftEventHandler(object sender, EventArgs e);

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        private IContainer Components = null;
        private UnitDelegate ConvFunc;
        private Label Description;
        public FlexTextBox EditBox;
        private BindingSource exBindingSource = new BindingSource();
        private Binding testbind;
        private int textbxwidth = 60;
        private DimTextBox dTextBox;
        private UOMProperty DD = new UOMProperty(new Null());

        public DataBoundDimTextBox(UOMProperty d)
        {
            dTextBox.Visible = false;
            InitializeComponent();
            UnitList = d.DefaultUnit;
            EditBox.Width = textbxwidth;
        }

        public DataBoundDimTextBox()
        {
            InitializeComponent();
            dTextBox.Visible = false;
            UnitList = "";
            ConvFunc = UnitConv.NullUnitDelegate;
        }

        public ArrayList DimList
        {
            get
            {
                var o = new object();
                string[] a = { };
                if (ConvFunc != null)
                    ConvFunc(1, "", false, ref a, o);
                return new ArrayList(a);
            }
        }

        public string GetDefaultUnit
        {
            get { return (string)DimList[0]; }
        }

        public SourceEnum Origin
        {
            get { return DD.Source; }
            set { DD.Source = value; }
        }

        public new ControlBindingsCollection DataBindings
        {
            get { return EditBox.DataBindings; }
        }

        public int Textbxwidth
        {
            get { return textbxwidth; }
            set { textbxwidth = value; }
        }

        public int EditBoxLeft
        {
            get { return EditBox.Left; }
            set { EditBox.Left = value; }
        }

        public bool EditBoxReadOnly
        {
            get { return EditBox.ReadOnly; }
            set { EditBox.ReadOnly = value; }
        }

        public int EditBoxWidth
        {
            get { return EditBox.Width; }
            set { EditBox.Width = value; }
        }

        public string UnitList { get; set; }

        public ArrayList GetDimList
        {
            get
            {
                var o = new object();
                string[] a = { };
                if (ConvFunc != null)
                    ConvFunc(1, "", false, ref a, o);
                return new ArrayList(a);
            }
        }

        public int TextWidth
        {
            get { return EditBox.Width; }
            set { EditBox.Width = value; }
        }

        public int TextLeft
        {
            get { return EditBox.Left; }
            set { EditBox.Left = value; }
        }

        public bool ReadOnly
        {
            get { return EditBox.ReadOnly; }
            set
            {
                EditBox.ReadOnly = value;
                DropDownVisible = !false;
            }
        }

        public bool DropDownVisible
        {
            get { return dTextBox.Visible; }
            set { dTextBox.Visible = value; }
        }

        public string DescriptionText
        {
            get { return Description.Text; }
            set { Description.Text = value; }
        }

        public int DescriptionWidth
        {
            get { return Description.Width; }
            set { Description.Width = value; }
        }

        public event KeyEventHandler DTextKeyDownEvent;

        public event ValueChangedEventHandler ValueChangedEvent;

        public event MouseLeftEventHandler MouseLeftEvent;

        public void BindValues(UOMProperty dd)
        {
            if (dd != null)
            {
                if (dd.Source == SourceEnum.Input ||
                    dd.Source == SourceEnum.Empty)
                    EditBox.ReadOnly = false;
                else
                {
                    EditBox.ReadOnly = true;
                }

                DD = dd;
                if (DD.Property != ePropID.NullUnits)
                {
                    DescriptionText = DescriptionText + ", " + dd.DefaultUnit;
                }
                EditBox.Text = dd.BaseValue.ToString();

                this.dTextBox.Visible = false;
                this.dTextBox.SetValues(dd);

                testbind = new Binding("Text", dd, "Value");
                testbind.Format += new ConvertEventHandler(DoubleToString);
                testbind.Parse += new ConvertEventHandler(StringTodouble);

                if (EditBox.DataBindings.Count == 0)
                {
                    EditBox.DataBindings.Add(testbind);
                }
            }
        }

        public void LinkValues(UOMProperty dd)
        {
            DD = dd;
        }

        private void DoubleToString(object sender, ConvertEventArgs cevent)
        {
            // The method converts only to string  type. Test this using   the DesiredType.

            if (cevent.DesiredType != typeof(string)) return;
            cevent.Value = ((double)cevent.Value).ToString("#####.####");
        }

        private void StringTodouble(object sender, ConvertEventArgs cevent)
        {
            double res;
            bool err;

            // The method converts back to decimal type only.
            if (cevent.DesiredType != typeof(double)) return;
            // Converts the string  back to decimal using   the static Parse method.

            err = double.TryParse(cevent.Value.ToString(), out res);

            if (err)
            {
                cevent.Value = res;
                DD.IsDirty = true;
                DD.Source = SourceEnum.Input;
            }
            else
            {
                cevent.Value = double.NaN;
                DD.IsDirty = true;
                DD.Source = SourceEnum.Empty;
            }
        }

        /// <summary>
        ///     Clean up any resources being used.
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
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Description = new Label();
            this.dTextBox = new Units.DimTextBox();
            this.EditBox = new Units.FlexTextBox();
            this.SuspendLayout();
            //
            // Description
            //
            this.Description.AutoSize = true;
            this.Description.Location = new System.Drawing.Point(3, 7);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(100, 17);
            this.Description.TabIndex = 5;
            this.Description.Text = "DESCRIPTION";
            //
            // dTextBox
            //
            this.dTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.dTextBox.Ddbxleft = 60;
            this.dTextBox.Ddbxwidth = 60;
            this.dTextBox.DropDownVisible = true;
            this.dTextBox.Location = new System.Drawing.Point(185, 7);
            this.dTextBox.Name = "dTextBox";
            this.dTextBox.Precision = 4;
            this.dTextBox.ReadOnly = false;
            this.dTextBox.Size = new System.Drawing.Size(128, 22);
            this.dTextBox.TabIndex = 6;
            this.dTextBox.Textbxwidth = 60;
            this.dTextBox.TextWidth = 60;
            this.dTextBox.Visible = false;
            this.dTextBox.ValueChangedEvent += new Units.DimTextBox.ValueChangedEventHandler(this.DimTextBox1_ValueChangedEvent);
            this.dTextBox.MouseLeftEvent += new Units.DimTextBox.MouseLeftEventHandler(this.dTextBox_MouseLeftEvent);
            this.dTextBox.Leave += new System.EventHandler(this.dTextBox_Leave);
            this.dTextBox.MouseLeave += new System.EventHandler(this.dTextBox_MouseLeave);
            //
            // EditBox
            //
            this.EditBox.Location = new System.Drawing.Point(108, 6);
            this.EditBox.Name = "EditBox";
            this.EditBox.ReadOnly = true;
            this.EditBox.Size = new System.Drawing.Size(71, 22);
            this.EditBox.TabIndex = 3;
            this.EditBox.Text = "VALUE";
            this.EditBox.TextIfBlank = "VALUE";
            this.EditBox.Click += new System.EventHandler(this.EditBox_Click);
            this.EditBox.TextChanged += new System.EventHandler(this.EditBox_TextChanged);
            this.EditBox.DoubleClick += new System.EventHandler(this.EditBox_doubleClick);
            this.EditBox.KeyDown += new KeyEventHandler(this.textBox_KeyDown);
            this.EditBox.MouseLeave += new System.EventHandler(this.EditBox_MouseLeave);
            //
            // DataBoundDimTextBox
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.dTextBox);
            this.Controls.Add(this.EditBox);
            this.Controls.Add(this.Description);
            this.Name = "DataBoundDimTextBox";
            this.Size = new System.Drawing.Size(314, 34);
            this.Load += new System.EventHandler(this.DataBoundDimTextBox_Load);
            this.MouseLeave += new System.EventHandler(this.DataBoundDimTextBox_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Component Designer generated code

        public double Value
        {
            get
            {
                return DD.BaseValue;
            }
            set
            {
                this.EditBox.Text = Math.Round(value, 1).ToString();
                DD.BaseValue = value;
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (Origin)
            {
                case SourceEnum.Input:
                case SourceEnum.Empty:
                    {
                        if (DD.Property != ePropID.NullUnits)
                            EditBox.ReadOnly = false;
                        break;
                    }
            }
        }

        protected virtual void RaiseKeyEnterEvent(KeyEventArgs e)
        {
            dTextBox.Visible = false;

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
            DD = dd;
            Value = DD.BaseValue;
            //ConvFunc = dd.ConvFunc;
            this.dTextBox.Update(dd);
            dTextBox.Text = UnitList;
        }

        public void SelectText()
        {
            EditBox.SelectAll();
        }

        private void EditBox_doubleClick(object sender, EventArgs e)
        {
            switch (DD.Source)
            {
                case SourceEnum.Input:
                case SourceEnum.Empty:
                    {
                        if (DD.Property != ePropID.NullUnits)
                        {
                            dTextBox.Left = EditBox.Left;
                            dTextBox.Top = EditBox.Top;
                            dTextBox.Visible = true;
                            dTextBox.SelectText();
                        }
                        break;
                    }
                case SourceEnum.Transferred:
                    {
                        break;
                    }
            }
        }

        private void DimTextBox1_ValueChangedEvent(object sender, EventArgs e)
        {
            Value = DD.BaseValue;
            dTextBox.Visible = false;
            Origin = SourceEnum.Input;
            RaiseValueChangedEvent(new EventArgs());
            EditBox.Focus(); // give focus back to data grid form text box
        }

        private void dTextBox_MouseLeftEvent(object sender, EventArgs e)
        {
            dTextBox.Visible = false;
        }

        public new string Text
        {
            get { return Description.Text; }
            set { Description.Text = value; }
        }

        private void dTextBox_Leave(object sender, EventArgs e)
        {
            Value = dTextBox.Value;
            dTextBox.Visible = false;
        }

        private void DataBoundDimTextBox_Load(object sender, EventArgs e)
        {
        }

        private void EditBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void EditBox_Click(object sender, EventArgs e)
        {
            if (dTextBox.Visible)
            {
                dTextBox.Visible = false;
            }
            else
            {
                switch (DD.Source)
                {
                    case SourceEnum.Input:
                    case SourceEnum.Empty:
                        {
                            if (DD.Property != ePropID.NullUnits)
                            {
                                dTextBox.Left = EditBox.Left;
                                dTextBox.Top = EditBox.Top;
                                dTextBox.Visible = true;
                                dTextBox.SelectText();
                            }
                            break;
                        }
                    case SourceEnum.Transferred:
                        {
                            break;
                        }
                }
            }
        }

        private void EditBox_MouseLeave(object sender, EventArgs e)
        {
        }

        private void DataBoundDimTextBox_MouseLeave(object sender, EventArgs e)
        {
            //dTextBox.Visible = false;
            //dTextBox.Refresh();
        }

        private void dTextBox_MouseLeave(object sender, EventArgs e)
        {
            dTextBox.Visible = false;
            dTextBox.Refresh();
        }
    }
}