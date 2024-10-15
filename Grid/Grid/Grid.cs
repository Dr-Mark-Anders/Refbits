using ModelEngine;
using ModelEngine;
using SimpleExpressionEngine;
using System.Text.RegularExpressions;
using Units;
using static System.Windows.Forms.DataGridView;

namespace UOMGrid
{
    public enum Mode
    {
        Spreadhseet, CaseStudy,
        CaseStudyResults
    }

    public partial class Grid : UserControl
    {
        private UOMDisplayList uomDisplayList = new UOMDisplayList();
        private Dictionary<Guid, Point> CellLocations = new Dictionary<Guid, Point>();
        private Dictionary<Guid, List<Guid>> LinkedToCellList = new();
        private Dictionary<Guid, List<Guid>> LinkedFromCellList = new();
        public Dictionary<Guid, UOMGridCell> CellList = new();
        public Dictionary<Guid, CellData> CellDataList = new();

        public Mode mode = Mode.Spreadhseet;

        public bool SetDescription = false;

        public void TransferCellListToGrid()
        {
            if (CellList != null)
            {
                foreach (CellData cell in CellDataList.Values)
                {
                    UOMGridCell newcell = new(cell);
                    DGV[cell.ColumnIndex, cell.RowIndex] = newcell;
                }
            }
        }

        public UOMGridCell this[int Col, int Row]
        {
            get { return (UOMGridCell)DGV[Col, Row]; }
        }

        public Grid()
        {
            InitializeComponent();
        }

        public void SetupGrid()
        {
            for (int i = 0; i < Cols; i++)
            {
                UOMGridColumn col = new();
                DGV.Columns.Add(col);
                DGV.Columns[i].Name = GetAlphabets(i).ToString();
                DGV.Columns[i].HeaderText = GetAlphabets(i).ToString();
                DGV.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (int i = 0; i < Rows; i++)
            {
                int row = DGV.Rows.Add();
                DGV.Rows[row].HeaderCell.Value = (i + 1).ToString();
            }

            switch (mode)
            {
                case Mode.Spreadhseet:
                case Mode.CaseStudy:
                    break;
            }
        }

        public void SetupCaseStudyGrid(int NoCases)
        {
            for (int i = DGV.ColumnCount; i > 2; i--)
            {
                DGV.Columns.RemoveAt(i - 1);
            }

            Cols = 2 + NoCases;
            for (int i = 2; i < NoCases + 2; i++)
            {
                UOMGridColumn col = new();
                DGV.Columns.Add(col);
                //DGV.Columns[i].Name = GetAlphabets(i).ToString();
                DGV.Columns[i].HeaderText = "Case: " + (i - 2).ToString();
                DGV.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public int Rows { get; set; }
        public int Cols { get; set; }
        public new string Name { get; set; }

        public static string GetAlphabets(int n)
        {
            //Declare string for alphabet

            string strAlpha = "";
            //Loop through the ASCII characters 65 to 90
            strAlpha += ((char)(65 + n)).ToString() + " ";
            return strAlpha;
        }

        public static string ExtraAlphas(string text)
        {
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(text);

            string alphaPart = result.Groups[1].Value;
            //string numberPart = result.Groups[2].Value;

            return alphaPart;
        }

        private class MyGridContext : IContext
        {
            private DataGridView dgv;

            public MyGridContext(DataGridView DGV)
            {
                dgv = DGV;
            }

            public double ResolveVariable(string name)
            {
                int column = 0, Row;

                Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
                Match result = re.Match(name);

                string alphaPart = result.Groups[1].Value;
                string numberPart = result.Groups[2].Value;

                for (int i = 0; i < alphaPart.Length; i++)
                {
                    column = column * (i * 24) + alphaPart[i] - 65;
                }

                if (int.TryParse(numberPart, out Row)
                    && dgv[column, Row - 1] is UOMGridCell cell
                    && cell.Value != null
                    && double.TryParse(cell.Value.ToString(), out double res))
                    return res;

                return double.NaN;
            }

            public double CallFunction(string name, double[] arguments)
            {
                return double.NaN;
            }
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV[e.ColumnIndex, e.RowIndex] is UOMGridCell c)
            {
                if (c.Value is string CellValue)
                {
                    //string CellValue = c.Value.ToString();
                    if (CellValue.Contains('='))
                    {
                        c.Formula = CellValue;
                        var ctx = new MyGridContext(DGV);
                        c.Value = Parser.Parse(CellValue.Substring(1)).Eval(ctx).ToString();
                    }
                }
            }
            LinkCells();
        }

        public UOMGridCell? CellPos(int x, int y)
        {
            Point P = new Point(x, y);
            Point PP = DGV.PointToClient(P);
            HitTestInfo htinfo = DGV.HitTest(PP.X, PP.Y);

            if (htinfo.Type == DataGridViewHitTestType.Cell
                && DGV[htinfo.ColumnIndex, htinfo.RowIndex] is UOMGridCell cell)
                return cell;

            return null;
        }

        private void DGV_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void DGV_DragDrop(object sender, DragEventArgs e)
        {
            int Row;
            int Col;
            StreamProperty p;
            if (e.Data != null && e.Data.GetDataPresent(typeof(StreamProperty)))
                p = (StreamProperty)e.Data.GetData(typeof(StreamProperty));
            else
                return;

            UOMGridCell? cell = CellPos(e.X, e.Y);
            if (cell != null)
            {
                Row = cell.RowIndex;
                Col = cell.ColumnIndex;
            }
            else
                return;

            switch (mode)
            {
                case Mode.Spreadhseet:
                    cell.uom = p;
                    cell.Formula = null;
                    cell.Update();
                    break;

                case Mode.CaseStudy:
                    if (Col == 1 && DGV[Col - 1, Row] is UOMGridCell cell2)
                    {
                        cell.uom = p;
                        cell.Formula = null;
                        cell.Update();
                        if (cell2 != null)
                            cell2.Value = cell.uom.Port.Owner.Name + ": " + cell.uom.Name;
                    }
                    break;
            }

            e.Effect = DragDropEffects.None;

            LinkCells();
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SourceEnum source;
            if (DGV[e.ColumnIndex, e.RowIndex] is UOMGridCell cell && cell.uom != null)
            {
                if (cell.Value != null)
                    source = cell.uom.Source;
                else
                    source = SourceEnum.Empty;

                if (double.IsNaN(cell.uom.Value))
                    source = SourceEnum.Empty;

                if (cell.uom != null)
                {
                    //cell.Value = cell.uom.Value;
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
            //e.FormattingApplied = true
        }

        private void DGV_CelldoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (DGV.CurrentCell.ReadOnly)
                return;

            UOMGridCell cell = (UOMGridCell)DGV.CurrentCell;

            if (cell.Formula != null)
                cell.Value = cell.Formula;

            //  Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

            /*  CBOX1.DataSource = cell.uom.AllUnits;
              CBOX1.Location = new  Point (r.Right + DGV.Left, r.Top + DGV.Top + 1);
              CBOX1.Text = cell.uom.DisplayUnit;
              if (cell.uom.IsInput)
              {
                  CBOX1.BringToFront();
                  CBOX1.Visible = true;
              }
              OldCell = DGV.CurrentCell;*/
        }

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            if (DGV.CurrentCell is UOMGridCell cell)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        if (cell.Propid == ePropID.VF || cell.Propid == ePropID.MOLEF || cell.Propid == ePropID.MF)
                        {
                            UpdateValues();
                        }
                        else
                        {
                            cell.Value = double.NaN; // force an update
                            UpdateValues();
                        }
                        break;

                    case Keys.Enter:  // not fired when cell inedit mode
                        break;

                    default:
                        break;
                }
            }
        }

        private void UpdateDisplayedUnits()
        {
            /*  DGV[0, 0].Value = "Quality ";
              DGV[0, 1].Value = "Mass Flow, " + uomDisplayList.MF.ToString();
              DGV[0, 2].Value = "Vol Flow, " + uomDisplayList.VF.ToString();
              DGV[0, 3].Value = "Molar Flow, " + uomDisplayList.MoleF.ToString();
              DGV[0, 4].Value = "Pressure , " + uomDisplayList.P.ToString();
              DGV[0, 5].Value = "Temperature , " + uomDisplayList.T.ToString();
              DGV[0, 6].Value = "Entropy, " + uomDisplayList.S.ToString();
              DGV[0, 7].Value = "Enthalpy, " + uomDisplayList.H.ToString();*/
        }

        private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0
                && e.Button == MouseButtons.Right && e.ColumnIndex == 0)
            {
                DGV.CurrentCell = DGV[e.ColumnIndex, e.RowIndex];
                UOMGridCell uomCell;
                DataGridViewCell cell = (DataGridViewCell)DGV.CurrentCell;
                uomCell = (UOMGridCell)DGV[e.ColumnIndex + 1, e.RowIndex];

                Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                //CBOX1.Location = new  Point (r.Left - groupBoxStream.Left - DGV.Left, r.Top + 1 + groupBoxStream.Top - DGV.Top);
                /* CBOX1.DataSource = uomCell.uom.AllUnits;
                 CBOX1.Location = new  Point (r.Right + DGV.Left, r.Top + DGV.Top + 1);
                 CBOX1.Text = uomCell.uom.DisplayUnit;
                 CBOX1.BringToFront();
                 CBOX1.Visible = true;
                 OldCell = DGV.CurrentCell;*/
            }
        }

        /*    private  void  DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
            {
                if (DGV.CurrentCell is UOMGridCell cell)
                {
                    if (cell != null && cell.Value != null)
                        if (double .TryParse(cell.Value.ToString(), out double  res))
                        {
                            cell.uom.ValueIn(cell.uom.DisplayUnit, res);   // set port value from call value
                            if (!double .IsNaN(res))
                                cell.uom.Source = SourceEnum.Input;
                        }
                }
                port.ForgetProps();  // keep composition
                port.Flash(calcderivatives: true);
                UpdateValues(ValueOnly: true);

                RaiseChangeEvent();
            }*/

        private void DGV_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 &&
                DGV[e.ColumnIndex, e.RowIndex] is UOMGridCell cell
                && cell.uom != null)
            {
                UOMProperty prop = cell.uom;
                this.DoDragDrop(prop, DragDropEffects.Move);
            }
        }

        public void UpdateValues(bool ValueOnly = false)
        {
            UpdateDisplayedUnits();

            for (int c = 0; c < DGV.ColumnCount; c++)
            {
                for (int r = 0; r < DGV.RowCount; r++)
                {
                    if (DGV[c, r] is UOMGridCell cell && cell.uom != null)
                        cell.Update();
                }
            }

            Calculate();
            DGV.Refresh();
        }

        public static int MatchToCol(string text)
        {
            string pattern = @"[A-Z]{1,}"; // get cell references
            MatchCollection matches;
            Regex defaultRegex = new Regex(pattern);
            matches = defaultRegex.Matches(text);
            int column = 0;

            for (int i = 0; i < matches[0].Value.Length; i++)
            {
                column = column * (i * 24) + matches[0].Value[i] - 65;
            }

            return column;
        }

        public static int? MatchToRow(string text)
        {
            string pattern = @"\d{1,}"; // get cell references
            MatchCollection matches;
            Regex defaultRegex = new Regex(pattern);
            matches = defaultRegex.Matches(text);

            if (int.TryParse(matches[0].Value, out int res))
                return res - 1;  // zero index
            else
                return null;
        }

        public void LinkCells()
        {
            CellLocations.Clear();
            LinkedToCellList.Clear();
            LinkedFromCellList.Clear(); // clear all previous links
            CellList.Clear();

            for (int row = 0; row < DGV.RowCount; row++)
            {
                for (int col = 0; col < DGV.ColumnCount; col++)
                {
                    if (DGV[col, row] is UOMGridCell CurrentCell)
                    {
                        CellLocations[CurrentCell.Guid] = new Point(CurrentCell.RowIndex, CurrentCell.ColumnIndex);
                        CellList.Add(CurrentCell.Guid, CurrentCell);

                        if (CurrentCell.Formula != null)
                        {
                            CurrentCell.CalcOrder = 0;

                            string text = CurrentCell.Formula;
                            string pattern = @"\w{1,}\d{1,}"; // get cell references

                            Regex defaultRegex = new(pattern);
                            // Get matches of pattern in text
                            MatchCollection matches = defaultRegex.Matches(text);
                            foreach (Match match in matches)
                            {
                                string matchtext = match.Value;
                                int? Col = MatchToCol(matchtext);
                                int? Row = MatchToRow(matchtext);

                                if (Row != null && Col != null
                                    && DGV[(int)Col, (int)Row] is UOMGridCell LinkedToCell)
                                {
                                    Point pos = new((int)Row, (int)Col);

                                    if (!LinkedToCellList.ContainsKey(CurrentCell.Guid))
                                        LinkedToCellList.Add(CurrentCell.Guid, new List<Guid>());

                                    LinkedToCellList[CurrentCell.Guid].Add(LinkedToCell.Guid);

                                    /*  if (!LinkedFromCellList.ContainsKey(CurrentCell.Guid))
                                          LinkedFromCellList.Add(CurrentCell.Guid, new  List<Guid>());

                                      LinkedFromCellList[CurrentCell.Guid].Add(CurrentCell.Guid);*/
                                }
                            }
                        }
                    }
                }
            }

            for (int row = 0; row < DGV.RowCount; row++)
            {
                for (int col = 0; col < DGV.ColumnCount; col++)
                {
                    if (DGV[col, row] is UOMGridCell CurrentCell)
                    {
                        if (CurrentCell.Formula != null)
                        {
                            if (LinkedToCellList.ContainsKey(CurrentCell.Guid)
                                    && LinkedToCellList[CurrentCell.Guid].Count > 0)
                            {
                                foreach (Guid Guid in LinkedToCellList[CurrentCell.Guid])
                                {
                                    Point Pos = CellLocations[Guid];
                                    if (DGV[Pos.Y, Pos.X] is UOMGridCell LinkedFromCell)
                                    {
                                        if (!LinkedFromCellList.ContainsKey(CurrentCell.Guid))
                                            LinkedFromCellList.Add(CurrentCell.Guid, new());

                                        LinkedFromCellList[CurrentCell.Guid].Add(LinkedFromCell.Guid);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int row = 0; row < DGV.RowCount; row++)
            {
                for (int col = 0; col < DGV.ColumnCount; col++)
                {
                    if (DGV[col, row] is UOMGridCell CurrentCell
                        && CurrentCell.Formula != null) // !nothing to do
                    {
                        if (LinkedToCellList.ContainsKey(CurrentCell.Guid)
                            && LinkedToCellList[CurrentCell.Guid].Count == 0) // level 0
                        {
                            CurrentCell.CalcOrder = 0;
                        }
                        else if (LinkedFromCellList.ContainsKey(CurrentCell.Guid)
                            && LinkedFromCellList[CurrentCell.Guid].Count > 0)
                        {
                            foreach (Guid LinkedGuid in LinkedFromCellList[CurrentCell.Guid])
                            {
                                Point P = CellLocations[LinkedGuid];
                                if (DGV[P.Y, P.X] is UOMGridCell LinkedeCell)
                                {
                                    if (LinkedeCell.CalcOrder + 1 > CurrentCell.CalcOrder)
                                        CurrentCell.CalcOrder = LinkedeCell.CalcOrder + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool Calculate()
        {
            for (int row = 0; row < DGV.RowCount; row++)
            {
                for (int col = 0; col < DGV.ColumnCount; col++)
                {
                    if (DGV[col, row] is UOMGridCell cell
                        && cell.Formula != null)
                    {
                        var res = Parser.Parse(cell.Formula.Substring(1));
                        if (res != null)
                            cell.Value = res.Eval(new MyGridContext(DGV));

                        cell.uom = null; // cant have formula and UOMProp in same cell
                    }
                }
            }
            return true;
        }

        private void DGV_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0
                && DGV[e.ColumnIndex, e.RowIndex] is UOMGridCell cell)
                cell.ToolTipText = cell.Formula + ": " + cell.CalcOrder.ToString();
        }

        public bool AllowUserToAddRows
        {
            get { return DGV.AllowUserToAddRows; }
            set { DGV.AllowUserToAddRows = value; }
        }

        public int RowCount { get => DGV.RowCount; set => DGV.RowCount = value; }

        public int ColOneMinimumWidth
        {
            get
            {
                return DGV.Columns[0].MinimumWidth;
            }
            set
            {
                DGV.Columns[0].MinimumWidth = value;
            }
        }

        public DataGridViewAutoSizeColumnMode ColumnMode
        {
            get
            {
                return DGV.Columns[0].AutoSizeMode;
            }
            set
            {
                DGV.Columns[0].AutoSizeMode = value;
            }
        }

        public int ColCount { get => DGV.ColumnCount; set => DGV.ColumnCount = value; }
    }
}