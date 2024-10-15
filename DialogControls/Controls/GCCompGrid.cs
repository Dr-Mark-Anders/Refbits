using ModelEngine;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Units;

namespace FormControls
{
    public partial class GCCompGrid : UserControl
    {
        private readonly List<PropertyList> props = new();
        private List<string> rowNames = new();
        private int firstColumnWidth = 100;

        public GCCompGrid()
        {
            InitializeComponent();
        }

        internal List<double> GetFractions()
        {
            List<double> res = new();

            for (int i = 0; i < DGV.RowCount; i++)
            {
                res.Add((double)DGV.Rows[i].Cells[1].Value);
            }

            return res;
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

        internal void AddRange(SimpleCompList comps)
        {
            int rowno;
            for (int i = 0; i < comps.Components.Count; i++)
            {
                rowno = DGV.Rows.Add();
                DGV.Rows[rowno].Cells[0].Value = comps.Components[i].Name;
                DGV.Rows[rowno].Cells[1].Value = comps.Components[i].MoleFraction;
            };
        }

        public void Add(UOMProperty propa, int col = 0)
        {
            for (int i = 0; i <= col; i++)
            {
                if (props.Count <= i)
                    props.Add(new PropertyList());
            }

            props[col].Add(propa);
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
            props[col].Add(temp);
        }

        public void SetValue(string value, int row, int col)
        {
            DGV[col, row].Value = value;
        }

        public static void UpdateProps()
        {
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*  SourceEnum source;

               if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMPropertyCell)
               {
                   DataGridViewUOMPropertyCell cell = (DataGridViewUOMPropertyCell)DGV[e.ColumnIndex, e.RowIndex];

                   if (cell.Value != null)
                       source = cell.uom.Source;
                   else
                       source = SourceEnum.Empty;

                   if (double .IsNaN(cell.uom.Value))
                       source = SourceEnum.Empty;

                   if (cell != null && cell.uom != null)
                   {
                       switch (source)
                       {
                           case  SourceEnum.Input:
                               e.CellStyle.ForeColor = Color.Blue;
                               break;

                           case  SourceEnum.CalcResult:
                               e.CellStyle.ForeColor = Color.Black;
                               break;

                           case  SourceEnum.Empty:
                               e.CellStyle.ForeColor = Color.Red;
                               break;

                           case  SourceEnum.Default:
                               break;

                           case  SourceEnum.Transferred:
                               break;

                           case  SourceEnum.CalcEstimate:
                               break;

                           case  SourceEnum.FixedEstimate:
                               break;

                           default:
                               break;
                       }
                   }
               }
               //e.FormattingApplied = true
            */
        }

        public int FirstColumnWidth
        {
            get { return firstColumnWidth; }
            set
            {
                DGV.Columns[0].Width = value;
                firstColumnWidth = value;
            }
        }

        public void AddColumn(string name = "")
        {
            int col = DGV.Columns.Add(name, name);
            DGV.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        public void StoreOneValue(DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            UOMProperty prop;

            DataGridViewRow dgvrow = DGV.Rows[row];

            prop = props[col][(Guid)dgvrow.Cells[col].Value];

            if (double.TryParse(dgvrow.Cells[1].Value.ToString(), out double res))
                prop.DisplayValueIn(res);
        }

        public void StoreData()
        {
            //bindingSource.Clear();
            UOMProperty prop;
            for (int col = 0; col < DGV.ColumnCount; col++)
            {
                for (int i = 0; i < DGV.RowCount; i++)
                {
                    prop = props[col][(Guid)DGV[0, i].Value];

                    if (double.TryParse(DGV[0, i].Value.ToString(), out double res))
                        prop.DisplayValueIn(res);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CBOX1.Visible = false;
            int sum = 0;
            foreach (DataGridViewRow dgvr in DGV.Rows)
                sum++;
        }

        private void dataGridView1_CelldoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
            {
                UOMProperty prop = props[e.ColumnIndex][cell.uom.guid];
                prop.Value = (double)cell.Value;
            }
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
    }
}