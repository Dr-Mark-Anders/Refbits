using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Forms;
using UOMGrid;

namespace Units
{
    [Serializable]
    public partial class SpreadsheetDialog3 : Form, IUpdateableDialog, ISerializable
    {
        private DrawSpreadhseet drawSpread;

        public SpreadsheetDialog3()
        {
            InitializeComponent();
        }

        public SpreadsheetDialog3(SerializationInfo info, StreamingContext context)
        {
            grid1 = (Grid)info.GetValue("Grid", typeof(Grid));
        }

        public int Rows
        {
            get { return grid1.Rows; }
            set { grid1.Rows = value; }
        }

        public int Cols
        {
            set { grid1.Cols = value; }
            get { return grid1.Cols; }
        }

        internal SpreadsheetDialog3(DrawSpreadhseet drawSpread)
        {
            InitializeComponent();
            this.drawSpread = drawSpread;
            this.grid1.CellDataList = drawSpread.CellDataList;
        }

        public void SetUp()
        {
            grid1.SetupGrid();
            grid1.TransferCellListToGrid();
        }

        public void UpdateValues()
        {
            grid1.UpdateValues();
            Invalidate();
        }

        private void Worksheet_ValueChanged(object sender, System.EventArgs e)
        {
            // base.RaiseValueChangedEvent(e);
        }

        private void GridControl_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        private void SpreadsheetDialog_Load(object sender, System.EventArgs e)
        {
        }

        private void grid1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Grid", grid1);
        }

        private void SpreadsheetDialog3_FormClosing(object sender, FormClosingEventArgs e)
        {
            drawSpread.CellDataList.Clear();
            for (int col = 0; col < grid1.Cols; col++)
            {
                for (int row = 0; row < grid1.Rows; row++)
                {
                    if (grid1[col, row] is UOMGridCell cell)
                    {
                        Debug.Print(row.ToString() + "" + col.ToString());
                        if (cell != null && (cell.Formula != null || cell.Value != null))
                        {
                            drawSpread.CellDataList.Add(cell.Guid, new CellData(cell));
                        }
                    }
                }
            }
        }
    }
}