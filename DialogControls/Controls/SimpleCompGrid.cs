using ModelEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormControls
{
    public partial class RXStoichiometryGrid : UserControl
    {
        private Components comps = new();
        private List<string> rowNames = new();
        private bool readOnly = false;

        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        protected virtual void RaiseChangeEvent()
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            ValueChanged?.Invoke(this, new EventArgs());
        }

        public RXStoichiometryGrid()
        {
            InitializeComponent();
        }

        [Description("")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
                "System.Design, Version=2.0.0.0, Culture=neutral, public KeyToken=b03f5f7f11d50a3a",
        typeof(System.Drawing.Design.UITypeEditor))]
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

        internal void AddRange(Components cc)
        {
            comps.Clear();
            for (int i = 0; i < cc.Count; i++)
            {
                comps.Add(cc[i]);
            };
        }

        [Browsable(true)]
        public string TopText
        {
            get { return gpbox.Text; }
            set { gpbox.Text = value; }
        }

        public void Add(BaseComp comp)
        {
            comps.Add(comp);
            Update();
        }

        public void UpdateGrid(Components cc)
        {
            comps = cc;
            DGV.Rows.Clear();

            for (int i = 0; i < comps.Count; i++)
            {
                int rowno = DGV.Rows.Add();
                DGV[1, rowno].Value = comps[i].Name;
            }
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*   if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
               {
                   SourceEnum source = cell.uom.Source;

                   if (double .IsNaN(cell.uom.Value))
                       source = SourceEnum.Empty;

                   if (cell.uom != null)
                   {
                       switch (source)
                       {
                           case  SourceEnum.Input:
                               e.CellStyle.ForeColor = Color.Blue;
                               cell.readonly  = false;
                               break;

                           case  SourceEnum.CalcResult:
                               e.CellStyle.ForeColor = Color.Black;
                               cell.readonly  = true;
                               break;

                           case  SourceEnum.Empty:
                               e.CellStyle.ForeColor = Color.Red;
                               cell.readonly  = false;
                               break;

                           case  SourceEnum.Default:
                               break;

                           case  SourceEnum.Transferred:
                               e.CellStyle.ForeColor = Color.Black;
                               break;

                           case  SourceEnum.CalcEstimate:
                               break;

                           case  SourceEnum.FixedEstimate:
                               break;

                           default:
                               break;
                       }
                   }

                   if (readonly )
                   {
                       cell.readonly  = true;
                       e.CellStyle.ForeColor = Color.Black;
                   }
               }*/
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

        public List<string> RowNames { get => rowNames; set => rowNames = value; }
        public bool ReadOnly { get => readOnly; set => readOnly = value; }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
            {
                cell.uom.Source = SourceEnum.Input;
            }

            if (e.ColumnIndex == 2)
            {
                if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewTextBoxCell celltext)
                {
                    if (int.TryParse(celltext.Value.ToString(), out int res))
                    {
                        comps[e.RowIndex].RxStoichiometry = res;
                    }
                    else
                    {
                        MessageBox.Show("Please Meter an int eger");
                        celltext.Value = 1;
                    }
                }
            }

            RaiseChangeEvent();
        }

        public void Clear()
        {
            DGV.Rows.Clear();
            comps.Clear();
            //props.Clear();
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

        // Declare the delegate (if using   non-generic pattern).
        public delegate void RowsChangedEventHandler(object sender, RowsChangedEventArgs e);

        // Declare the event.
        public event RowsChangedEventHandler RowsChanged;

        // Wrap the event in a protected  virtual method
        // to enable derived class es to raise the event.
        protected virtual void RaiseRowsChangedEvent()
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            //RowsChanged?.Invoke(this, new  RowsChangedEventArgs("Hello"));
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

        private void DGV_CelldoubleClick(object sender, DataGridViewCellEventArgs e)
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
                    if (DGV.CurrentCell.ColumnIndex == 2 && DGV.CurrentCell is DataGridViewUOMCell)
                    {
                        DataGridViewUOMCell cell = (DataGridViewUOMCell)DGV.CurrentCell;
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
                DGV.CurrentCell = DGV[e.ColumnIndex, e.RowIndex];
                DataGridViewUOMCell uomCell;
                DataGridViewCell cell = (DataGridViewCell)DGV.CurrentCell;
                uomCell = (DataGridViewUOMCell)DGV[e.ColumnIndex + 1, e.RowIndex];

                Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                //CBOX1.Location = new  Point (r.Left - groupBoxStream.Left - DGV.Left, r.Top + 1 + groupBoxStream.Top - DGV.Top);
                CBOX1.DataSource = uomCell.uom.AllUnits;
                CBOX1.Location = new Point(r.Right + DGV.Left, r.Top + DGV.Top + 1);
                CBOX1.Text = uomCell.uom.DisplayUnit;
                CBOX1.BringToFront();
                CBOX1.Visible = true;
                OldCell = DGV.CurrentCell;
            }
        }

        private DataGridViewCell OldCell;

        private void CBOX1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string newUnit = cb.SelectedItem.ToString();

            switch (DGV.CurrentCell)
            {
                case DataGridViewUOMCell cell:
                    if (newUnit != "" || newUnit != null)
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
    }
}