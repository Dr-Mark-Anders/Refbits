using FormControls;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Units.UOM;

namespace Units.PortForm
{
    public partial class PortsPropertyWorksheet : UserControl
    {
        private List<StreamMaterial> streams;
        private UOMDisplayList DisplayList;

        public PortsPropertyWorksheet()
        {
            InitializeComponent();
        }

        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        protected virtual void RaiseChangeEvent()
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            ValueChanged?.Invoke(this, new EventArgs());
        }

        public void PortsPropertyWorksheetInitialise(List<StreamMaterial> streams, UOMDisplayList units)
        {
            this.streams = streams;
            int count = 1;
            for (int i = 0; i < streams.Count; i++)
            {
                StreamMaterial stream = streams[i];
                if (count > 1)
                    DGV.Columns.Add("Col" + count.ToString(), stream.Name);
                DGV.Columns[count].HeaderText = stream.Name;
                count++;
            }

            this.DisplayList = units;

            AddRows();

            Port_Material port;

            if (this.streams != null)
            {
                for (int i = 1; i <= streams.Count; i++)
                {
                    port = streams[i - 1].Port;

                    DGV[i, 0] = new DataGridViewUOMCell(port.Q_);
                    DGV[i, 1] = new DataGridViewUOMCell(port.MF_);
                    DGV[i, 2] = new DataGridViewUOMCell(port.VF_);
                    DGV[i, 3] = new DataGridViewUOMCell(port.MolarFlow_);
                    DGV[i, 4] = new DataGridViewUOMCell(port.P_);
                    DGV[i, 5] = new DataGridViewUOMCell(port.T_);
                    DGV[i, 6] = new DataGridViewUOMCell(port.S_);
                    DGV[i, 7] = new DataGridViewUOMCell(port.H_);
                }

                UpdateValues();
            }
        }

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            if (((DataGridView)sender).CurrentCell is DataGridViewUOMCell UOMcell)
            {
                Port_Material port = streams[UOMcell.ColumnIndex - 1].Port;
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        if (UOMcell.uom.Source == SourceEnum.Input)
                        {
                            UOMcell.uom.Clear();
                        }
                        else
                        {
                            port.Properties[UOMcell.Propid].Clear();
                            UOMcell.Value = double.NaN; // force an update
                        }
                        ResetNonFixedValues(port);
                        RaiseChangeEvent();

                        break;

                    case Keys.Enter:  // not fired when cell inedit mode
                        break;

                    default:
                        break;
                }
            }
        }

        private void CBOX1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string newUnit = cb.SelectedItem.ToString();

            switch (DGV.CurrentCell)
            {
                case DataGridViewUOMCell cell:  // Porperty value column
                    if (newUnit != "" && newUnit != null)
                    {
                        if (DGV.CurrentCell != null && double.TryParse(DGV.CurrentCell.EditedFormattedValue.ToString(), out double res))
                        {
                            cell.uom.ValueIn(newUnit, res);
                            cell.Update();
                            cell.uom.Source = SourceEnum.Input;
                        }

                        cb.Visible = false;

                        if (DGV.CurrentCell != null)
                            DGV.RefreshEdit();
                    }
                    break;

                case DataGridViewCell cell2: //  description column
                    DataGridViewUOMCell cell3 = (DataGridViewUOMCell)DGV[cell2.ColumnIndex + 1, cell2.RowIndex];
                    if (newUnit != "" && newUnit != null)
                    {
                        cell3.uom.DisplayUnit = newUnit;
                        cell3.Update();
                        switch (cell3.uom.Propid)
                        {
                            case ePropID.MF:
                                DisplayList.MF = (MassFlowUnit)Enum.Parse(typeof(MassFlowUnit), newUnit);
                                break;

                            case ePropID.VF:
                                DisplayList.VF = (VolFlowUnit)Enum.Parse(typeof(VolFlowUnit), newUnit);
                                break;

                            case ePropID.MOLEF:
                                DisplayList.MoleF = (MoleFlowUnit)Enum.Parse(typeof(MoleFlowUnit), newUnit);
                                break;

                            case ePropID.T:
                                DisplayList.T = (TemperatureUnit)Enum.Parse(typeof(TemperatureUnit), newUnit);
                                break;

                            case ePropID.P:
                                DisplayList.P = (PressureUnit)Enum.Parse(typeof(PressureUnit), newUnit);
                                break;

                            case ePropID.MassEnthalpy:
                                DisplayList.H = (EnthalpyUnit)Enum.Parse(typeof(EnthalpyUnit), newUnit);
                                break;

                            case ePropID.MassEntropy:
                                DisplayList.S = (EntropyUnit)Enum.Parse(typeof(EntropyUnit), newUnit);
                                break;
                        }

                        UpdateValues();

                        cb.Visible = false;

                        if (DGV.CurrentCell != null)
                            DGV.RefreshEdit();
                    }
                    break;
            }
        }

        private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.ColumnIndex == 0)
            {
                DGV.CurrentCell = DGV[e.ColumnIndex, e.RowIndex];
                DataGridViewUOMCell uomCell;
                uomCell = (DataGridViewUOMCell)DGV[e.ColumnIndex + 1, e.RowIndex];
                Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                CBOX1.DataSource = uomCell.uom.AllUnits;
                CBOX1.Location = new Point(r.Right + DGV.Left, r.Top + DGV.Top + 1);
                CBOX1.Text = uomCell.uom.DisplayUnit;
                CBOX1.BringToFront();
                CBOX1.Visible = true;
            }
        }

        private void DGV_CelldoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1 || e.ColumnIndex > DGV.ColumnCount)
                return;

            if (DGV.CurrentCell is DataGridViewUOMCell cell && !cell.ReadOnly)
            {
                Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                CBOX1.DataSource = cell.uom.AllUnits;
                CBOX1.Location = new Point(r.Right + DGV.Left, r.Top + DGV.Top + 1);
                CBOX1.Text = cell.uom.DisplayUnit;
                CBOX1.BringToFront();
                CBOX1.Visible = true;
            }
        }

        private void AddRows()
        {
            DGV.Rows.Add(8);

            if (DisplayList != null)
            {
                DGV[0, 0].Value = "Quality ";
                DGV[0, 1].Value = "Mass Flow, " + DisplayList.MF.ToString();
                DGV[0, 2].Value = "Vol Flow, " + DisplayList.VF.ToString();
                DGV[0, 3].Value = "Molar Flow, " + DisplayList.MoleF.ToString();
                DGV[0, 4].Value = "Pressure , " + DisplayList.P.ToString();
                DGV[0, 5].Value = "Temperature , " + DisplayList.T.ToString();
                DGV[0, 6].Value = "Entropy, " + DisplayList.S.ToString();
                DGV[0, 7].Value = "Enthalpy, " + DisplayList.H.ToString();

                if (streams != null)
                {
                    foreach (var stream in streams)
                    {
                        stream.Port.UpdateDisplysettings(DisplayList);
                    }
                }
            }
            DGV.Update();
        }

        private void UpdateDisplayedUnits()
        {
            if (DisplayList != null)
            {
                DGV[0, 0].Value = "Quality ";
                DGV[0, 1].Value = "Mass Flow, " + DisplayList.MF.ToString();
                DGV[0, 2].Value = "Vol Flow, " + DisplayList.VF.ToString();
                DGV[0, 3].Value = "Molar Flow, " + DisplayList.MoleF.ToString();
                DGV[0, 4].Value = "Pressure , " + DisplayList.P.ToString();
                DGV[0, 5].Value = "Temperature , " + DisplayList.T.ToString();
                DGV[0, 6].Value = "Entropy, " + DisplayList.S.ToString();
                DGV[0, 7].Value = "Enthalpy, " + DisplayList.H.ToString();
            }
        }

        public void UpdateValues(bool ValueOnly = false)
        {
            UpdateDisplayedUnits();

            int count = 0;
            if (streams != null)
            {
                DGV.SuspendLayout();
                foreach (StreamMaterial stream in streams)
                {
                    count++;
                    Port_Material port = stream.Port;
                    port.UpdateDisplysettings(DisplayList);

                    if (DGV.InvokeRequired)
                    {
                        DGV.Invoke(() => ((DataGridViewUOMCell)DGV[count, 0]).Update(port.Q_, valueonly: ValueOnly)); 
                        DGV.Invoke(() => ((DataGridViewUOMCell)DGV[count, 1]).Update(port.MF_, valueonly: ValueOnly));
                        DGV.Invoke(() => ((DataGridViewUOMCell)DGV[count, 2]).Update(port.VF_, valueonly: ValueOnly));
                        DGV.Invoke(() => ((DataGridViewUOMCell)DGV[count, 3]).Update(port.MolarFlow_, valueonly: ValueOnly));
                        DGV.Invoke(() => ((DataGridViewUOMCell)DGV[count, 4]).Update(port.P_, valueonly: ValueOnly));
                        DGV.Invoke(() => ((DataGridViewUOMCell)DGV[count, 5]).Update(port.T_, valueonly: ValueOnly));
                        DGV.Invoke(() => ((DataGridViewUOMCell)DGV[count, 6]).Update(port.S_, valueonly: ValueOnly));
                        DGV.Invoke(() => ((DataGridViewUOMCell)DGV[count, 7]).Update(port.H_, valueonly: ValueOnly));
                    }
                    else
                    {
                        ((DataGridViewUOMCell)DGV[count, 0]).Update(port.Q_, valueonly: ValueOnly);
                        ((DataGridViewUOMCell)DGV[count, 1]).Update(port.MF_, valueonly: ValueOnly);
                        ((DataGridViewUOMCell)DGV[count, 2]).Update(port.VF_, valueonly: ValueOnly);
                        ((DataGridViewUOMCell)DGV[count, 3]).Update(port.MolarFlow_, valueonly: ValueOnly);
                        ((DataGridViewUOMCell)DGV[count, 4]).Update(port.P_, valueonly: ValueOnly);
                        ((DataGridViewUOMCell)DGV[count, 5]).Update(port.T_, valueonly: ValueOnly);
                        ((DataGridViewUOMCell)DGV[count, 6]).Update(port.S_, valueonly: ValueOnly);
                        ((DataGridViewUOMCell)DGV[count, 7]).Update(port.H_, valueonly: ValueOnly);
                    }
                }
            }
            UpdateFormats();
            DGV.ResumeLayout();
            if(this.InvokeRequired)
                this.Invoke(() => this.Refresh());
            else
                Refresh();
        }

        private void btnComposition_Click(object sender, EventArgs e)
        {
            CompositionsControl cf = new(streams);
            cf.FillFractionData();
            cf.Show();
        }

        public void UpdateFormats()
        {
            SourceEnum source;

            for (int col = 1; col < DGV.ColumnCount; col++)
            {
                for (int row = 0; row < DGV.RowCount; row++)
                {
                    if (DGV[col, row] is DataGridViewUOMCell Cell)
                    {
                        //if (Cell.Value != null)
                            source = Cell.uom.Source;
                        //else
                        //    source = SourceEnum.Empty;

                        if (double.IsNaN(Cell.uom.Value))
                            source = SourceEnum.Empty;

                        if (Cell.uom != null)
                        {
                            switch (source)
                            {
                                case SourceEnum.Input:
                                    Cell.Style.ForeColor = Color.Blue;
                                    break;

                                case SourceEnum.PortCalcResult:
                                case SourceEnum.UnitOpCalcResult:
                                    Cell.Style.ForeColor = Color.Black;
                                    break;

                                case SourceEnum.Empty:
                                    Cell.Style.ForeColor = Color.Red;
                                    break;

                                case SourceEnum.Default:
                                    break;

                                case SourceEnum.Transferred:
                                    Cell.Style.ForeColor = Color.Black;
                                    break;

                                case SourceEnum.CalcEstimate:
                                    Cell.Style.ForeColor = Color.Black;
                                    break;

                                case SourceEnum.FixedEstimate:
                                    Cell.Style.ForeColor = Color.Black;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.CurrentCell is DataGridViewUOMCell cell
                 && double.TryParse(((DataGridViewCell)cell).Value.ToString(), out double res))
            {
                cell.uom.ValueIn(cell.uom.DisplayUnit, res);   // set port value from call value
                if (!double.IsNaN(res))
                    cell.uom.Source = SourceEnum.Input;
            }
            Port_Material port = streams[DGV.CurrentCell.ColumnIndex - 1].Port;
            ResetNonFixedValues(port);  // keep composition
            port.Flash(calcderivatives: true);
            RaiseChangeEvent();
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SourceEnum source;
            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
            {
                source = cell.uom.Source;

                if (double.IsNaN(cell.uom.Value))
                    source = SourceEnum.Empty;

                // cell.ReadOnly  = true;

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
                            cell.ReadOnly = true;
                            break;

                        case SourceEnum.Transferred:
                            cell.ReadOnly = true;
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

        public static void ResetNonFixedValues(Port port)
        {
            port.ClearProps();  // keep composition
            port.IsDirty = true;
            Port p = port.StreamPort;
            if (p != null)
            {
                p.ClearProps();
                p.IsDirty = true;
            }
        }
    }
}