using ModelEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Units;
using Units.UOM;

namespace FormControls
{
    public partial class UserPropGrid : UserControl
    {
        private readonly List<PropertyList> props = new();
        private List<string> rowNames = new();
        public List<string> RowNames { get => rowNames; set => rowNames = value; }

        private bool readOnly = false;
        private bool allowChangeEvent = true;

        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        protected virtual void RaiseChangeEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            if (allowChangeEvent)
                ValueChanged?.Invoke(this, new EventArgs());
        }

        public UserPropGrid()
        {
            InitializeComponent();
            props.Add(new PropertyList());
        }

        /*[Description("")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
                "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        typeof(System.Drawing.Design.UITypeEditor))]*/
        public List<string> ColumnNames { get; set; } = new() { };

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

        internal void AddRange(UOMPropertyList gc, int col)
        {
            props[col].Clear();
            for (int i = 0; i < gc.Count; i++)
            {
                props[col].Add(gc[i].Item1);
            };
        }

        [Browsable(true)]
        public string TopText
        {
            get { return gpbox.Text; }
            set { gpbox.Text = value; }
        }

        public void Add(double value, string Name, int col = 0)
        {
            UOMProperty temp = new(ePropID.NullUnits, SourceEnum.UnitOpCalcResult, Name);
            temp.BaseValue = value;
            if (props.Count <= col)
                props.Add(new PropertyList());
            else
                props[col].Add(temp);
            UpdateValues();
        }

        public void Add(Port_Signal propa, string description, int col = 0)
        {
            propa.Value.DisplayName = description;
            UOMProperty uomp = propa.Value;
            for (int i = 0; i <= col; i++)
            {
                if (props.Count <= i)
                    props.Add(new PropertyList());
            }

            props[col].Add(uomp);
            UpdateValues();
        }

        public void Add(StreamProperty propa, string description, int col = 0)
        {
            propa.DisplayName = description;
            UOMProperty uomp = propa;
            for (int i = 0; i <= col; i++)
            {
                if (props.Count <= i)
                    props.Add(new PropertyList());
            }

            props[col].Add(uomp);
            UpdateValues();
        }

        public void Add(UOMProperty propa, string DisplayName, int col = 0, string ColumnName = "")
        {
            propa.DisplayName = DisplayName;

            for (int i = 0; i <= col; i++)
            {
                if (props.Count <= i)
                {
                    PropertyList pl = new();
                    pl.ColumnName = ColumnName;
                    props.Add(pl);
                }
            }

            props[col].Add(propa);
            UpdateValues();
        }

        public void Add(UOMProperty propa, int col = 0)
        {
            for (int i = 0; i <= col; i++)
            {
                if (props.Count <= i)
                {
                    PropertyList pl = new();
                    props.Add(pl);
                }
            }

            props[col].Add(propa);
            UpdateValues();
        }

        public void UpdateValues()
        {
            //if(DGV is not null)
            //DGV.Rows.Clear();

            DGV.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DGV.Columns[1].MinimumWidth = 30;
            DGV.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (props.Count + 1 >= DGV.ColumnCount)
            {
                do
                {
                    DataGridViewUOMColumn dgvc = new();
                    DGV.Columns.Add(dgvc);
                    dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                } while (DGV.ColumnCount < props.Count + 2);
            }

            for (int i = 2; i < DGV.ColumnCount; i++)
            {
                DGV.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (ColumnNames is not null)
                for (int i = 0; i < props.Count; i++)
                {
                    if (DGV.ColumnCount > i + 2 && ColumnNames.Count > i)
                        DGV.Columns[i + 2].HeaderText = ColumnNames[i];
                }

            int MaxRows = 0;
            for (int col = 0; col < props.Count; col++)
            {
                if (props[col].Count > MaxRows)
                    MaxRows = props[col].Count;
            }

            if (DGV.RowCount < MaxRows)
                DGV.Rows.Add(MaxRows - DGV.RowCount);

            for (int col = 1; col <= props.Count; col++)
            {
                if (props[col - 1].ColumnName != "")
                    DGV.Columns[col + 1].HeaderText = props[col - 1].ColumnName;

                if (DGV.ColumnCount >= col)
                {
                    for (int row = 0; row < props[col - 1].Count; row++)
                    {
                        UOMProperty uomProp = props[col - 1][row];
                        DataGridViewRow dgvr = DGV.Rows[row];

                        if (col == 1)
                        {
                            mydelegate = UpdateValueMethod;

                            if (uomProp.UOM is not Quality)
                                if (DGV.InvokeRequired)
                                {
                                    string s = uomProp.DisplayName + ", " + uomProp.DisplayUnit;
                                    DGV.Invoke(new Action(() => DGV[col, row].Value = s));
                                }
                                else
                                    dgvr.Cells[1].Value = uomProp.DisplayName;
                        }

                        dgvr.Cells[col + 1].Value = uomProp.DisplayValueOut();
                        if (dgvr.Cells[col + 1] is DataGridViewUOMCell cell)
                            cell.uom = uomProp;
                    }
                }
            }
        }

        public delegate void UpdateValue(int r, int c, string val);

        public UpdateValue mydelegate;

        public void UpdateValueMethod(int r, int c, string val)
        {
            DGV[c, r].Value = val;
            DGV.Update();
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
            {
                SourceEnum source = cell.uom.Source;

                if (double.IsNaN(cell.uom.Value))
                    source = SourceEnum.Empty;

                if (cell.uom != null)
                {
                    switch (source)
                    {
                        case SourceEnum.Input:
                            e.CellStyle.ForeColor = Color.Blue;
                            cell.ReadOnly = false;
                            break;

                        case SourceEnum.UnitOpCalcResult:
                            e.CellStyle.ForeColor = Color.Black;
                            cell.ReadOnly = true;
                            break;

                        case SourceEnum.Empty:
                            e.CellStyle.ForeColor = Color.Red;
                            cell.ReadOnly = false;
                            break;

                        case SourceEnum.Default:
                            break;

                        case SourceEnum.Transferred:
                            e.CellStyle.ForeColor = Color.Black;
                            break;

                        case SourceEnum.CalcEstimate:
                            break;

                        case SourceEnum.FixedEstimate:
                            break;

                        default:
                            break;
                    }
                }

                if (readOnly)
                {
                    cell.ReadOnly = true;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
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

        public void AddColumn(string name = "")
        {
            int col = DGV.Columns.Add(name, name);
            DGV.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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

        public bool ReadOnly { get => readOnly; set => readOnly = value; }
        public bool AllowChangeEvent { get => allowChangeEvent; set => allowChangeEvent = value; }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
            {
                cell.uom.Source = SourceEnum.Input;
            }
            RaiseChangeEvent();
        }

        public void Clear()
        {
            DGV.Rows.Clear();
            props.Clear();
        }

        private void PropertyDisplayGrid2_Resize(object sender, EventArgs e)
        {
            DGV.Left = gpbox.Left + 10;
            DGV.Top = gpbox.Top + 20;
            DGV.Height = gpbox.Height - 30;
            DGV.Width = gpbox.Width - 20;
        }

        private void DGV_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            RaiseRowsChangedEvent();
        }

        private void DGV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            RaiseRowsChangedEvent();
        }

        // Declare the delegate (if using non-generic pattern).
        public delegate void RowsChangedEventHandler(object sender, RowsChangedEventArgs e);

        // Declare the event.
        public event RowsChangedEventHandler RowsChanged;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseRowsChangedEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            //RowsChanged?.Invoke(this, new RowsChangedEventArgs("Hello"));
        }

        private void DGV_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            RowsChanged?.Invoke(this, new RowsChangedEventArgs("Hello"));
        }

        private void DGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                switch (DGV[e.ColumnIndex, e.RowIndex])
                {
                    case DataGridViewUOMCell cell:
                        if (double.TryParse(((DataGridViewCell)cell).Value.ToString(), out double res))
                            cell.Value = res;
                        break;

                    default:
                        break;
                }
            }
            //RaiseChangeEvent();
        }

        private void DGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 0 && DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
            {
                Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                if (cell.uom.IsInput && !cell.ReadOnly)
                {
                    CBOX1.DataSource = cell.uom.AllUnits;
                    CBOX1.Location = new Point(r.Left + gpbox.Left + DGV.Left, r.Top + 1 + gpbox.Top + DGV.Top);
                    CBOX1.Text = cell.uom.DisplayUnit;
                    CBOX1.BringToFront();
                    CBOX1.Visible = true;
                }
            }
            OldCell = DGV.CurrentCell;
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.CurrentCell != OldCell)
                CBOX1.Visible = false;
        }

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (DGV.CurrentCell.ColumnIndex == 2 && DGV.CurrentCell is DataGridViewUOMCell cell)
                    {
                        cell.uom.Clear();
                        cell.Value = double.NaN;
                        RaiseChangeEvent();
                    }
                    break;
            }
        }

        private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.ColumnIndex == 1)
            {
                if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
                {
                    Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                    //CBOX1.Location = new Point(r.Left - groupBoxStream.Left - DGV.Left, r.Top + 1 + groupBoxStream.Top - DGV.Top);
                    CBOX1.DataSource = cell.uom.AllUnits;
                    CBOX1.Location = new Point(r.Right + DGV.Left, r.Top + DGV.Top + 1);
                    CBOX1.Text = cell.uom.DisplayUnit;
                    CBOX1.BringToFront();
                    CBOX1.Visible = true;
                    OldCell = DGV.CurrentCell;
                }
            }
        }

        private DataGridViewCell OldCell;

        private void CBOX1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cb = (System.Windows.Forms.ComboBox)sender;
            string NewUnit = cb.SelectedItem.ToString();

            switch (DGV.CurrentCell)
            {
                case DataGridViewUOMCell cell:
                    if (NewUnit != "" || NewUnit != null)
                    {
                        if (DGV.CurrentCell != null && double.TryParse(DGV.CurrentCell.EditedFormattedValue.ToString(), out double res))
                        {
                            cell.uom.ValueIn(NewUnit, res);
                            cell.Update();
                        }

                        cb.Visible = false;

                        if (DGV.CurrentCell != null)
                            DGV.RefreshEdit();
                    }
                    break;
            }
        }
    }

    public class RowsChangedEventArgs
    {
        public RowsChangedEventArgs(string text)
        { Text = text; }

        public string Text { get; } // readonly
    }
}