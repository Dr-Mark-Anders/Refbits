using ModelEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Units
{
    public partial class CompSplitterDialog : BaseDialog, IUpdateableDialog
    {
        private readonly CompSplitter compsplitter;
        private readonly DataTable data = new();
        private readonly int NoComps;
        private readonly int NoStreams;
        private bool isupdateing = false;

        public CompSplitterDialog()
        {
            InitializeComponent();
        }

        internal CompSplitterDialog(DrawCompSplitter drawcompsplitter)
        {
            InitializeComponent();
            NoComps = DrawArea.ComponentList.Count;
            this.compsplitter = drawcompsplitter.compsplitter;

            List<StreamMaterial> streams = drawcompsplitter.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, drawcompsplitter.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();

            List<StreamMaterial> productstreams = drawcompsplitter.GetProductStreamListFromNodes();
            NoStreams = productstreams.Count;

            if (compsplitter.splits.Count == 0
            || compsplitter.splits.Count != productstreams.Count
            || compsplitter.splits[0].Count != NoComps)//resetsplitifdimensiondontmatch
            {
                compsplitter.splits = new List<List<Port_Signal>>();
                for (int i = 0; i < productstreams.Count; i++)
                {
                    compsplitter.splits.Add(new List<Port_Signal>());
                    for (int compno = 0; compno < NoComps; compno++)
                    {
                        compsplitter.splits[i].Add(new Port_Signal());
                    }
                }
            }

            for (int i = 0; i < productstreams.Count; i++)
            {
                data.Columns.Add(productstreams[i].Name);
                int col = DGV.Columns.Add("Stream" + i, productstreams[i].Name);
                DGV.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                DGV.Columns[col].Width = 100;
            }

            for (int i = 0; i < NoComps; i++)
            {
                DataRow row = data.NewRow();
                data.Rows.Add(row);
                DGV.Rows.Add();
                DGV.Rows[i].HeaderCell.Value = DrawArea.ComponentList[i].Name;
            }

            for (int streamNo = 0; streamNo < productstreams.Count; streamNo++)
            {
                for (int compNo = 0; compNo < NoComps; compNo++)
                {
                    Port_Signal p = compsplitter.splits[streamNo][compNo];
                    if (p != null)
                    {
                        DGV[streamNo, compNo] = new DataGridViewPortSignalCell(p);
                    }
                }
            }
        }

        private void Worksheet_ValueChangedEvent(object sender, System.EventArgs e)
        {
            compsplitter.IsDirty = true;
            base.RaiseValueChangedEvent(e);
        }

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            if (((DataGridView)sender).CurrentCell is DataGridViewPortSignalCell Portcell)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        if (Portcell.port.Value.origin == SourceEnum.Input)
                        {
                            Portcell.Erase();
                            //ResetCells();
                            e.SuppressKeyPress = true;
                        }
                        break;

                    default:
                        break;
                }
            }

            compsplitter.CalcMissingSplits(NoStreams, NoComps);

            UpdateAllValues();
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SourceEnum source;

            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewPortSignalCell cell)
            {
                if (cell.Value != null)
                    source = cell.port.Value.origin;
                else
                    source = SourceEnum.Empty;

                if (cell != null && cell.port != null)
                {
                    switch (source)
                    {
                        case SourceEnum.Input:
                            e.CellStyle.ForeColor = Color.Blue;
                            break;

                        case SourceEnum.UnitOpCalcResult:
                            e.CellStyle.ForeColor = Color.Black;
                            break;

                        case SourceEnum.Empty:
                            e.CellStyle.ForeColor = Color.Red;
                            break;

                        case SourceEnum.Default:
                            break;

                        case SourceEnum.Transferred:
                            break;

                        case SourceEnum.CalcEstimate:
                            break;

                        case SourceEnum.FixedEstimate:
                            break;

                        default:
                            break;
                    }
                }
            }
            //e.FormattingApplied=true
        }

        public void ResetCells()
        {
            for (int i = 0; i < DGV.Rows.Count; i++)
            {
                if (DGV[1, i] is DataGridViewPortSignalCell cell)
                {
                    if (cell.port.Value.origin == SourceEnum.UnitOpCalcResult)
                    {
                        cell.port.Value = new StreamProperty(ePropID.NullUnits, double.NaN, SourceEnum.Empty);
                        cell.Value = double.NaN;
                    }
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isupdateing)
                return;

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewPortSignalCell cell)
                {
                    if (double.TryParse(cell.Value.ToString(), out double res))
                        if (!double.IsNaN(res))
                            if (cell.port.Value.origin == SourceEnum.Input || cell.port.Value.origin == SourceEnum.Empty)
                            {
                                DataGridViewPortSignalCell currentcell = cell;
                                currentcell.PortValueUpdate(SourceEnum.Input);
                            }
                }

                double sum = 0;
                DataGridViewPortSignalCell emptycell = null;
                int count = 0;

                for (int i = 0; i < DGV.Rows.Count; i++)
                {
                    if (DGV[1, i] is DataGridViewPortSignalCell cells)
                    {
                        if (cells.port.Value.origin == SourceEnum.Empty
                        || cells.port.Value.origin == SourceEnum.UnitOpCalcResult)
                        {
                            count++;
                            emptycell = cells;
                        }
                        else
                        {
                            if (double.TryParse(cells.Value.ToString(), out double res))
                                sum += res;
                        }
                    }
                }
                if (count == 1)
                {
                    emptycell.Value = 1 - sum;
                    emptycell.PortValueUpdate(SourceEnum.UnitOpCalcResult);
                }
            }

            compsplitter.CalcMissingSplits(NoStreams, NoComps);
            UpdateAllValues();
            DGV.Refresh();
        }

        public void UpdateAllValues()
        {
            isupdateing = true;
            for (int col = 0; col < DGV.ColumnCount; col++)
            {
                for (int row = 0; row < DGV.RowCount; row++)
                {
                    if (DGV[col, row] is DataGridViewPortSignalCell cell)
                    {
                        cell.ValueUpdate();
                    }
                }
            }
            isupdateing = false;
        }

        private void SetTo1_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridViewCell cell in DGV.SelectedCells)
            {
                cell.Value = 1;
            }
        }

        private void SetToZero_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridViewCell cell in DGV.SelectedCells)
            {
                cell.Value = 0;
            }
        }

        [Category("Ports"), Description("InPort")]
        public Port PortIn
        {
            get
            {
                return compsplitter.PortIn;
            }
        }

        [Category("Ports"), Description("InPort")]
        public PortList PortsOut
        {
            get
            {
                return compsplitter.Ports.PortsOut();
            }
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            compsplitter.IsDirty = true;
            base.RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            Worksheet.UpdateValues();
        }
    }
}