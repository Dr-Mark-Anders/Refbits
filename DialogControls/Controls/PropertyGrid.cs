using ModelEngine;
using ModelEngine;
using System;
using System.Windows.Forms;
using Units;

namespace FormControls
{
    public partial class PropertyGrid : UserControl
    {
        private PropertyList props = new();

        public PropertyGrid()
        {
            InitializeComponent();
        }

        public DataGridView DataGrid
        {
            get
            {
                return DGV;
            }
            set
            {
                DGV = value;
            }
        }

        public void Add(UOMProperty propa)
        {
            props.Add(propa);
        }

        internal void AddRange(UOMPropertyList gc)
        {
            props.Clear();
            for (int i = 0; i < gc.Count; i++)
            {
                props.Add(gc[i].Item1);
            };
        }

        public void Add(double value, string Name, int col = 0)
        {
            UOMProperty temp = new UOMProperty(ePropID.NullUnits, SourceEnum.UnitOpCalcResult, Name);
            temp.BaseValue = value;
            props.Add(temp);
        }

        public void UpdateProps()
        {
            DGV.Rows.Clear();

            DataGridViewRow dgvr;

            for (int i = 0; i < props.Count; i++)
            {
                UOMProperty item = props[i];
                if (i > DGV.Rows.Count - 1 && DGV.RowCount > 0)
                {
                    dgvr = DGV.Rows[(int)(i - DGV.Rows.Count * (i / DGV.Rows.Count))];
                    dgvr.Cells[0].Value = item.guid;
                    dgvr.Cells[1].Value = item.Name;
                    dgvr.Cells[2].Value = item.DisplayValueOut();
                }
            }
        }

        public int FirstColumnWidth
        {
            get { return DGV.Columns[1].Width; }
            set { DGV.Columns[1].Width = value; }
        }

        public void AddColumn(string name = "")
        {
            int col = DGV.Columns.Add(name, name);
            DGV.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        internal void SetData()
        {
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

        public void StoreOneValue(DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            UOMProperty prop;

            DataGridViewRow dgvrow = DGV.Rows[row];

            prop = props[(Guid)dgvrow.Cells[3].Value];

            if (double.TryParse(dgvrow.Cells[1].Value.ToString(), out double res))
                prop.DisplayValueIn(res);
        }

        public void StoreData()
        {
            //bindingSource.Clear();
            UOMProperty prop;
            for (int i = 0; i < DGV.RowCount; i++)
            {
                prop = props[(Guid)DGV[0, i].Value];

                if (double.TryParse(DGV[0, i].Value.ToString(), out double res))
                    prop.DisplayValueIn(res);
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
            StoreData();
        }

        public void Clear()
        {
            DGV.Rows.Clear();
            props.Clear();
        }
    }
}