using ModelEngine;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using unvell.ReoGrid;

namespace Units
{
    public partial class SpreadsheetDialog : Form, IUpdateableDialog
    {
        private DrawSpreadhseet obj;

        public SpreadsheetDialog()
        {
            InitializeComponent();
        }

        internal SpreadsheetDialog(DrawSpreadhseet obj)
        {
            InitializeComponent();
        }

        public void UpdateValues()
        {
            double val = 0;
            reoGridControl1.IterateCells(0, 0, 10, 10, (row, col, cell) =>
            {
                //cell.Pos = pos;
                reoGridControl1.CurrentEditingCell = cell;
                reoGridControl1.EndEdit(cell.Data, ReoGridEndEditReason.NormalFinish);
                return true;
            });
            Invalidate();
        }

        private void Worksheet_ValueChanged(object sender, System.EventArgs e)
        {
            // base.RaiseValueChangedEvent(e);
        }

        private void ReoGridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void reoGridControl1_DragDrop(object sender, DragEventArgs e)
        {
            StreamProperty p;

            if (e.Data.GetDataPresent(typeof(StreamProperty)))
            {
                p = (StreamProperty)e.Data.GetData(typeof(StreamProperty));

                Point clientPoint = reoGridControl1.Point ToClient(new Point(e.X, e.Y));
                ReoGridPos pos = reoGridControl1.GetCellPosByPoint(clientPoint);
                ReoGridCell cell = new();
                //cell.Data = p.Value;
                cell.Data = p;
                cell.Pos = pos;
                reoGridControl1.CurrentEditingCell = cell;
                reoGridControl1.EndEdit(cell.Data, ReoGridEndEditReason.NormalFinish);
                //RGAction action;
                //reoGridControl1.DoAction(action);
                reoGridControl1.Invalidate();
            }
        }

        private void SpreadsheetDialog_Load(object sender, System.EventArgs e)
        {
        }
    }
}