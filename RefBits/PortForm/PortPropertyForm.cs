using EngineThermo;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Units.UOM;

namespace Units.PortForm
{
    public partial class PortPropertyForm : Form
    {
        string Unit;
        Port_Material port;
        public UOMDisplayList uomDisplayList;
        bool Valuechanged = false;
        bool IsLoading = false;
        readonly FlowSheet fs;
        public bool IsDirty = false;
        internal DrawMaterialStream DrawMaterialStream;

        public PortPropertyForm(Port_Material Port, UOMDisplayList units)
        {
            InitializeComponent();
            if (Port != null)
                this.Text = Port.Owner.Name + " Stream Properties";

            port = Port;
            this.fs = port.Owner.Flowsheet;
            this.uomDisplayList = units;
            CBOX1.DataSource = Enum.GetNames(typeof(TemperatureUnit));
            DGV.CellClick += new DataGridViewCellEventHandler(DGV_CellClick);
#if DEBUG
            FlashButton.Visible = true;
#else
            FlashButton.Visible = false;
#endif
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CBOX1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IsLoading = true;
            DGV.Rows.Add(8);
            int column = 1;

            port.Components.CalcLiquidSH(port.T.BaseValue, port.P.BaseValue, port.Q.BaseValue);
            port.Components.CalcVapourSH(port.T.BaseValue, port.P.BaseValue, port.Q.BaseValue);
            port.Components.Q = port.Q.BaseValue;

            Components liq = port.Components.LiquidComponents;
            Components vap = port.Components.VapourComponents;

            IgnoreCellValueChanged();

            DGV[0, 0].Value = "Quality ";
            DGV[0, 1].Value = "Mass Flow, " + uomDisplayList.MF.ToString();
            DGV[0, 2].Value = "Vol Flow, " + uomDisplayList.VF.ToString();
            DGV[0, 3].Value = "Molar Flow, " + uomDisplayList.MoleF.ToString();
            DGV[0, 4].Value = "Pressure , " + uomDisplayList.P.ToString();
            DGV[0, 5].Value = "Temperature , " + uomDisplayList.T.ToString();
            DGV[0, 6].Value = "Entropy, " + uomDisplayList.S.ToString();
            DGV[0, 7].Value = "Enthalpy, " + uomDisplayList.H.ToString();

            port.UpdateDisplysettings(uomDisplayList);

            DGV[column, 0] = new DataGridViewUOMCell(port.Q);
            DGV[column, 1] = new DataGridViewUOMCell(port.MF);
            DGV[column, 2] = new DataGridViewUOMCell(port.VF);
            DGV[column, 3] = new DataGridViewUOMCell(port.MolarFlow);
            DGV[column, 4] = new DataGridViewUOMCell(port.P);
            DGV[column, 5] = new DataGridViewUOMCell(port.T);
            DGV[column, 6] = new DataGridViewUOMCell(port.S);
            DGV[column, 7] = new DataGridViewUOMCell(port.H);
            column++;

            if (liq != null && port.Q < 1)
            {
                DGV[column, 0].Value = 0;
                DGV[column, 1] = new DataGridViewUOMCell(new StreamProperty(ePropID.MF, SourceEnum.CalcResult, port.MF * (1 - port.Components.MassFractionLiquid())));
                DGV[column, 2] = new DataGridViewUOMCell(new StreamProperty(ePropID.VF, SourceEnum.CalcResult, port.VF * (1 - port.Components.VolFractionLiquid())));
                DGV[column, 3] = new DataGridViewUOMCell(new StreamProperty(ePropID.MOLEF, SourceEnum.CalcResult, port.MolarFlow * (1 - port.Components.MoleFractionLiquid())));
                DGV[column, 4] = new DataGridViewUOMCell(new StreamProperty(ePropID.P, SourceEnum.CalcResult, port.P));
                DGV[column, 5] = new DataGridViewUOMCell(new StreamProperty(ePropID.T, SourceEnum.CalcResult, port.T));
                DGV[column, 6] = new DataGridViewUOMCell(new StreamProperty(ePropID.S, SourceEnum.CalcResult, port.Components.ThermoLiq.S));
                DGV[column, 7] = new DataGridViewUOMCell(new StreamProperty(ePropID.H, SourceEnum.CalcResult, port.Components.ThermoLiq.H));
            }
            else
            {
                DGV[column, 1] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 2] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 3] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 4] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 5] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 6] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 7] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            }

            column = 3;
            if (vap != null && port.Q > 0)
            {
                DGV[column, 0].Value = 1;
                DGV[column, 1] = new DataGridViewUOMCell(new StreamProperty(ePropID.MF, SourceEnum.CalcResult, port.MF * port.Components.MassFractionLiquid()));
                DGV[column, 2] = new DataGridViewUOMCell(new StreamProperty(ePropID.VF, SourceEnum.CalcResult, port.VF * port.Components.VolFractionLiquid()));
                DGV[column, 3] = new DataGridViewUOMCell(new StreamProperty(ePropID.MOLEF, SourceEnum.CalcResult, port.MolarFlow * port.Components.MoleFractionLiquid()));
                DGV[column, 4] = new DataGridViewUOMCell(new StreamProperty(ePropID.P, SourceEnum.CalcResult, port.P));
                DGV[column, 5] = new DataGridViewUOMCell(new StreamProperty(ePropID.T, SourceEnum.CalcResult, port.T));
                DGV[column, 6] = new DataGridViewUOMCell(new StreamProperty(ePropID.S, SourceEnum.CalcResult, port.Components.ThermoVap.S));
                DGV[column, 7] = new DataGridViewUOMCell(new StreamProperty(ePropID.H, SourceEnum.CalcResult, port.Components.ThermoVap.H));
            }
            else
            {
                DGV[column, 1] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 2] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 3] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 4] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 5] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 6] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 7] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            }

            port.Components.T = port.T.BaseValue;
            port.UpdateCalcProperties();
            int Count = 0;
            foreach (KeyValuePair<string, CalcProperty> item in port.PropsCalculated)
            {
                dataGridProperties.Rows.Add();
                dataGridProperties[0, Count].Value = item.Value.DisplayName;
                dataGridProperties[1, Count].Value = item.Value.BaseValue;
                Count++;
            }

            UpdateValues();
            IsLoading = false;

            ResumeCellValueChanged();
        }

        public void IgnoreCellValueChanged()
        {
            this.DGV.CellValueChanged -= this.DGV_CellValueChanged;
        }

        public void ResumeCellValueChanged()
        {
            this.DGV.CellValueChanged -= this.DGV_CellValueChanged;
            this.DGV.CellValueChanged += this.DGV_CellValueChanged;
        }

        public void UpdateValues()
        {
            port.UpdateDisplysettings(uomDisplayList);

            IgnoreCellValueChanged();

            ((DataGridViewUOMCell)DGV[1, 0]).Update(port.Q);
            ((DataGridViewUOMCell)DGV[1, 1]).Update(port.MF);
            ((DataGridViewUOMCell)DGV[1, 2]).Update(port.VF);
            ((DataGridViewUOMCell)DGV[1, 3]).Update(port.MolarFlow);
            ((DataGridViewUOMCell)DGV[1, 4]).Update(port.P);
            ((DataGridViewUOMCell)DGV[1, 5]).Update(port.T);
            ((DataGridViewUOMCell)DGV[1, 6]).Update(port.S);
            ((DataGridViewUOMCell)DGV[1, 7]).Update(port.H);

            int column = 2;
            Components liq = port.Components.LiquidComponents;
            Components vap = port.Components.VapourComponents;

            if (liq != null && port.Q < 1)
            {
                DGV[column, 0].Value = 0;
                ((DataGridViewUOMCell)DGV[column, 1]).Update(new StreamProperty(ePropID.MF, SourceEnum.CalcResult, port.MF * (1 - port.Components.MassFractionLiquid())));
                ((DataGridViewUOMCell)DGV[column, 2]).Update(new StreamProperty(ePropID.VF, SourceEnum.CalcResult, port.VF * (1 - port.Components.VolFractionLiquid())));
                ((DataGridViewUOMCell)DGV[column, 3]).Update(new StreamProperty(ePropID.MOLEF, SourceEnum.CalcResult, port.MolarFlow * (1 - port.Components.MoleFractionLiquid())));
                ((DataGridViewUOMCell)DGV[column, 4]).Update(new StreamProperty(ePropID.P, SourceEnum.CalcResult, port.P));
                ((DataGridViewUOMCell)DGV[column, 5]).Update(new StreamProperty(ePropID.T, SourceEnum.CalcResult, port.T));
                ((DataGridViewUOMCell)DGV[column, 6]).Update(new StreamProperty(ePropID.S, SourceEnum.CalcResult, port.Components.ThermoLiq.S));
                ((DataGridViewUOMCell)DGV[column, 7]).Update(new StreamProperty(ePropID.H, SourceEnum.CalcResult, port.Components.ThermoLiq.H));
                //column++;
            }
            else
            {
                ((DataGridViewUOMCell)DGV[column, 1]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                ((DataGridViewUOMCell)DGV[column, 2]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                ((DataGridViewUOMCell)DGV[column, 3]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                ((DataGridViewUOMCell)DGV[column, 4]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                ((DataGridViewUOMCell)DGV[column, 5]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                ((DataGridViewUOMCell)DGV[column, 6]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                ((DataGridViewUOMCell)DGV[column, 7]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            }

            column = 3;
            if (vap != null && port.Q > 0)
            {
                DGV[column, 0].Value = 1;
                DGV[column, 1] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 2] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 3] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 4] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 5] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 6] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 7] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                ((DataGridViewUOMCell)DGV[column, 1]).Update(new StreamProperty(ePropID.MF, SourceEnum.CalcResult, port.MF * port.Components.MassFractionLiquid()));
                ((DataGridViewUOMCell)DGV[column, 2]).Update(new StreamProperty(ePropID.VF, SourceEnum.CalcResult, port.VF * port.Components.VolFractionLiquid()));
                ((DataGridViewUOMCell)DGV[column, 3]).Update(new StreamProperty(ePropID.MOLEF, SourceEnum.CalcResult, port.MolarFlow * port.Components.MoleFractionLiquid()));
                ((DataGridViewUOMCell)DGV[column, 4]).Update(new StreamProperty(ePropID.P, SourceEnum.CalcResult, port.P));
                ((DataGridViewUOMCell)DGV[column, 5]).Update(new StreamProperty(ePropID.T, SourceEnum.CalcResult, port.T));
                ((DataGridViewUOMCell)DGV[column, 6]).Update(new StreamProperty(ePropID.S, SourceEnum.CalcResult, port.Components.ThermoVap.S));
                ((DataGridViewUOMCell)DGV[column, 7]).Update(new StreamProperty(ePropID.H, SourceEnum.CalcResult, port.Components.ThermoVap.H));
                column++;
            }
            else
            {
                DGV[column, 1] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 2] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 3] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 4] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 5] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 6] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
                DGV[column, 7] = new DataGridViewUOMCell(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            }

            if (port.Q.IsEditable)
                DGV[1, 0].ReadOnly = false;
            else
                DGV[1, 0].ReadOnly = true;

            if (port.MF.IsEditable)
                DGV[1, 1].ReadOnly = false;
            else
                DGV[1, 1].ReadOnly = true;

            if (port.VF.IsEditable)
                DGV[1, 2].ReadOnly = false;
            else
                DGV[1, 2].ReadOnly = true;

            if (port.MolarFlow.IsEditable)
                DGV[1, 3].ReadOnly = false;
            else
                DGV[1, 3].ReadOnly = true;

            if (port.P.IsEditable)
                DGV[1, 4].ReadOnly = false;
            else
                DGV[1, 4].ReadOnly = true;

            if (port.T.IsEditable)
                DGV[1, 5].ReadOnly = false;
            else
                DGV[1, 5].ReadOnly = true;

            if (port.S.IsEditable)
                DGV[1, 6].ReadOnly = false;
            else
                DGV[1, 6].ReadOnly = true;

            if (port.H.IsEditable)
                DGV[1, 7].ReadOnly = false;
            else
                DGV[1, 7].ReadOnly = true;

            for (int row = 1; row < DGV.RowCount; row++)
                for (int col = 2; col <= 3; col++)
                    if (DGV[col, row] is DataGridViewUOMCell)
                        DGV[col, row].ReadOnly = true;

            port.Components.T = port.T.BaseValue;
            port.UpdateCalcProperties();
            int Count = 0;
            foreach (KeyValuePair<string, CalcProperty> item in port.PropsCalculated)
            {
                dataGridProperties[0, Count].Value = item.Value.DisplayName + " " + item.Value.DisplayUnit;
                dataGridProperties[1, Count].Value = item.Value.BaseValue;
                Count++;
            }

            ResumeCellValueChanged();
            DGV.Refresh();
        }

        public Port_Material SetPort
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }

        public StreamPropList Proplist { get { return port.Properties; } }

        private void DGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (fs != null && !fs.Isrunning && !IsLoading)
            {
                if (DGV.CurrentCell is DataGridViewUOMCell)
                {
                    DataGridViewUOMCell cell = ((DataGridViewUOMCell)DGV.CurrentCell);

                    if (cell != null && cell.Value != null)
                        if (double.TryParse(cell.Value.ToString(), out double res))
                        {
                            cell.uom.ValueIn(cell.uom.DisplayUnit, res);   // set port value from call value
                            if (!double.IsNaN(res))
                                cell.uom.Source = SourceEnum.Input;
                        }
                }
                port.ForgetProps();  // keep composition
                port.Flash();
                //fs.Solve();
                Valuechanged = false;
                UpdateValues();
                Valuechanged = true;
            }

            IsDirty = true;
            DrawMaterialStream.IsDirty = true;
        }

        private void CBOX1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            Unit = cb.SelectedItem.ToString();

            if (Unit != "" || Unit != null)
            {
                DataGridViewUOMCell cell = (DataGridViewUOMCell)DGV.CurrentCell;

                if (DGV.CurrentCell != null && double.TryParse(DGV.CurrentCell.EditedFormattedValue.ToString(), out double res))
                    cell.uom.ValueIn(Unit, res);

                cb.Visible = false;

                if (DGV.CurrentCell != null)
                    DGV.RefreshEdit();
            }
        }

        private void DGV_Celldouble Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1 || e.ColumnIndex > 1)
                return;

            if (DGV.CurrentCell.ReadOnly)
                return;

            DataGridViewUOMCell cell = (DataGridViewUOMCell)DGV.CurrentCell;

            Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

            CBOX1.Location = new Point(r.Right + groupBoxStream.Left + DGV.Left, r.Top + 1 + groupBoxStream.Top + DGV.Top);
            CBOX1.Text = cell.uom.DisplayUnit;
            CBOX1.BringToFront();
            CBOX1.Visible = true;
        }

        private void btnComposition_Click(object sender, EventArgs e)
        {
            CompositionForm cf = new CompositionForm(DrawMaterialStream);
            cf.Show();
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SourceEnum source;

            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell)
            {
                DataGridViewUOMCell cell = (DataGridViewUOMCell)DGV[e.ColumnIndex, e.RowIndex];

                if (cell.Value != null)
                    source = cell.uom.Source;
                else
                    source = SourceEnum.Empty;

                if (double.IsNaN(cell.uom.Value))
                    source = SourceEnum.Empty;

                if (cell != null && cell.uom != null)
                {
                    switch (source)
                    {
                        case SourceEnum.Input:
                            e.CellStyle.ForeColor = Color.Blue;
                            break;

                        case SourceEnum.CalcResult:
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
            //e.FormattingApplied = true
        }

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewCell cell = ((DataGridView)sender).CurrentCell;

            if (cell is DataGridViewUOMCell)
            {
                DataGridViewUOMCell UOMcell = (DataGridViewUOMCell)cell;

                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        if (UOMcell.Propid == ePropID.VF || UOMcell.Propid == ePropID.MOLEF || UOMcell.Propid == ePropID.MF)
                        {
                            port.MF.EraseValue();
                            port.VF.EraseValue();
                            port.MolarFlow.EraseValue();
                            UpdateValues();
                        }
                        else
                        {
                            port.Properties[UOMcell.Propid].EraseValue();
                            UOMcell.Value = double.NaN; // force an update
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

        private void DGV_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!port.IsFlashed && Valuechanged)
            {
                // GlobalModel.Flowsheet.Solve();
                // UpdateValues();
                // Valuechanged = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DrawMaterialStream.IsSolved = false;
            port.ForgetCalcValues();
            port.Flash(true);
            Refresh();
            UpdateValues();
            DGV.Refresh();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ChartForm cf = new ChartForm(port);
            cf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Chart2 cf = new Chart2(port);
            cf.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DistilationDataForm ddf = new DistilationDataForm(port);
            ddf.Show();
        }

        private void CopyStream_Click(object sender, EventArgs e)
        {
            DrawArea da = DrawMaterialStream.Owner;
            if (da is null)
                return;
            List<DrawMaterialStream> streams = da.GraphicsList.return MaterialStreams();
            CopyStreamDLG csdlg = new CopyStreamDLG(streams);
            DialogResult dr = csdlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.port.Components = csdlg.stream.Components.Clone();
                this.port.Properties = csdlg.stream.PortIn.Properties.Clone();
                if (this.port.MolarFlow.IsKnown)
                    this.port.MolarFlow.origin = SourceEnum.Input;

                this.IsDirty = true;
                this.port.Components.Origin = SourceEnum.Input;
            }
        }
    }
}