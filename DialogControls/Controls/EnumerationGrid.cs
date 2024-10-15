using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormControls
{
    public partial class EnumerationGrid : UserControl
    {
        public event ValueChangedEventHandler ValueChanged;

        public ThermoDynamicOptions options;

        public void SetUp()
        {
            Add(options);
        }

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        protected virtual void RaiseChangeEvent()
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            ValueChanged?.Invoke(this, new EventArgs());
        }

        public EnumerationGrid()
        {
            InitializeComponent();
        }

        private List<string> columnNames = new() { };

        [Description("")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
                "System.Design, Version=2.0.0.0, Culture=neutral, public KeyToken=b03f5f7f11d50a3a",
        typeof(System.Drawing.Design.UITypeEditor))]
        public List<string> ColumnNames
        {
            get
            {
                return columnNames;
            }
            set
            {
                columnNames = value;
            }
        }

        [Browsable(true)]
        public bool AllowUserToAddRows
        {
            get
            {
                return DGV.AllowUserToAddRows;
            }
            set
            {
                DGV.AllowUserToAddRows = value;
                DGV.RowHeadersVisible = value;
            }
        }

        [Browsable(true)]
        public bool AllowUserToDeleteRows
        {
            get
            {
                return DGV.AllowUserToDeleteRows;
            }
            set
            {
                DGV.AllowUserToDeleteRows = value;
                DGV.RowHeadersVisible = value;
            }
        }

        [Browsable(true)]
        public bool RowHeadersVisible
        {
            get
            {
                return DGV.RowHeadersVisible;
            }
            set
            {
                DGV.RowHeadersVisible = value;
            }
        }

        [Browsable(true)]
        public new Color BackColor
        {
            get
            {
                return gpbox.BackColor;
            }
            set
            {
                base.BackColor = value;
                gpbox.BackColor = value;
            }
        }

        [Browsable(true)]
        public string TopText
        {
            get { return gpbox.Text; }
            set { gpbox.Text = value; }
        }

        public void SetValue(string value, int row, int col)
        {
            DGV[col, row].Value = value;
        }

        public int FirstColumnWidth
        {
            get
            {
                return DGV.Columns[1].Width;
            }
            set
            {
                DGV.Columns[1].Width = value;
            }
        }

        public bool DisplayTitles
        {
            get
            {
                return DGV.ColumnHeadersVisible;
            }
            set
            {
                DGV.ColumnHeadersVisible = value;
            }
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)DGV[e.ColumnIndex, e.RowIndex];
                string value = cell.Value.ToString();

                switch (e.RowIndex)
                {
                    case 0:
                        options.KMethod = (enumEquiKMethod)Enum.Parse(typeof(enumEquiKMethod), value);
                        break;

                    case 1:
                        options.BIPMethod = (enumBIPPredMethod)Enum.Parse(typeof(enumBIPPredMethod), value);
                        break;

                    case 2:
                        options.Enthalpy = (enumEnthalpy)Enum.Parse(typeof(enumEnthalpy), value);
                        break;

                    case 3:
                        options.Density = (enumDensity)Enum.Parse(typeof(enumDensity), value);
                        break;

                    case 4:
                        options.ViscLiqMethod = (enumViscLiqMethod)Enum.Parse(typeof(enumViscLiqMethod), value);
                        break;

                    case 5:
                        options.ViscVapMethod = (enumViscVapMethod)Enum.Parse(typeof(enumViscVapMethod), value);
                        break;

                    case 6:
                        options.ThermcondMethod = (enumThermalConductivity)Enum.Parse(typeof(enumThermalConductivity), value);
                        break;

                    case 7:
                        options.SurfaceTensionMethod = (enumSurfaceTensionMethod)Enum.Parse(typeof(enumSurfaceTensionMethod), value);
                        break;

                    case 8:
                        options.MW_Method = (enumMW_Method)Enum.Parse(typeof(enumMW_Method), value);
                        break;

                    case 9:
                        options.CritTMethod = (enumCritTMethod)Enum.Parse(typeof(enumCritTMethod), value);
                        break;

                    case 10:
                        options.CritPMethod = (enumCritPMethod)Enum.Parse(typeof(enumCritPMethod), value);
                        break;

                    case 11:
                        options.OmegaMethod = (enumOmegaMethod)Enum.Parse(typeof(enumOmegaMethod), value);
                        break;
                }
            }
        }

        public void Clear()
        {
            DGV.Rows.Clear();
            //props.Clear();
        }

        private void PropertyDisplayGrid2_Resize(object sender, EventArgs e)
        {
            DGV.Left = gpbox.Left + 10;
            DGV.Top = gpbox.Top + 20;
            DGV.Height = gpbox.Height - 30;
            DGV.Width = gpbox.Width - 20;
        }

        // Declare the delegate (if using   non-generic pattern).
        public delegate void RowsChangedEventHandler(object sender, RowsChangedEventArgs e);

        // Declare the event.
        public event RowsChangedEventHandler RowsChanged;

        private void DGV_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            RowsChanged?.Invoke(this, new RowsChangedEventArgs("Hello"));
        }

        private void CBOX1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string newUnit = cb.SelectedItem.ToString();

            switch (DGV.CurrentCell)
            {
                case DataGridViewUOMCell cell:
                    if (newUnit != "" && newUnit != null)
                    {
                        if (DGV.CurrentCell != null && double.TryParse(DGV.CurrentCell.EditedFormattedValue.ToString(), out double res))
                        {
                            cell.uom.ValueIn(newUnit, res);
                            cell.Update();
                        }

                        cb.Visible = false;

                        if (DGV.CurrentCell != null)
                            DGV.RefreshEdit();
                    }
                    break;
            }
        }

        public void Add(object enums, string v)
        {
            Type t = enums.GetType();
            int RowNo = DGV.Rows.Add();
            DGV.Rows[RowNo].Cells[1].Value = v;
            DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)DGV.Rows[RowNo].Cells[2];
            var array = Enum.GetNames(t);
            cell.Items.AddRange(array);
            cell.Value = enums.ToString();
        }

        public void Add(ThermoDynamicOptions options)
        {
            this.options = options;
            Add(options.KMethod, "Equilibrium K Method");
            Add(options.BIPMethod, "Binary int erchange");
            Add(options.Enthalpy, "Enthalpy");
            Add(options.Density, "Actual Density");
            Add(options.ViscLiqMethod, "Liquid Viscosity");
            Add(options.ViscVapMethod, "Vapour Viscosity");
            Add(options.ThermcondMethod, "Thermal Conductivity");
            Add(options.SurfaceTensionMethod, "Surface Tension");
            Add(options.MW_Method, "MW Estimation");
            Add(options.CritTMethod, "Critical T Estimation");
            Add(options.CritPMethod, "Critial P Estimation");
            Add(options.OmegaMethod, "Acentric Factor Estimation");
        }
    }
}