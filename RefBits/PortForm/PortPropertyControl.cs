using ModelEngine;
using FormControls;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Units.UOM;

namespace Units.PortForm
{
    public partial class PortPropertyWorksheet : UserControl
    {
        private Port_Material port = new();
        private UOMDisplayList uomDisplayList = new();
        private DrawMaterialStream drawMaterialStream;
        private DataGridViewCell OldCell;

        public PortPropertyWorksheet()
        {
            InitializeComponent();
        }

        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        protected virtual void RaiseChangeEvent()
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            port.IsDirty = true;
            ValueChanged?.Invoke(this, new EventArgs());
        }

        public void PortsPropertyWorksheetInitialise(Port_Material Port, UOMDisplayList units)
        {
            if (Port is null)
                return;

            port = Port;
            if (Port.Owner != null)
                this.Text = Port.Owner.Name + " Stream Properties";

            //port.Components.UpdateLiqVapFractionCompositions(port.Components.Q);
            //Thermodynamicsclass .UpdateThermoProperties(port.Components, port.P.Value, port.T.Value, port.Components.Thermo);

            this.uomDisplayList = units;
            DGV.CellClick += new DataGridViewCellEventHandler(DGV_CellClick);

            port.H_.DisplayUnit = units.H.ToString();
            port.S_.DisplayUnit = units.S.ToString();
            port.MF_.DisplayUnit = units.MF.ToString();
            port.VF_.DisplayUnit = units.VF.ToString();
            port.MolarFlow_.DisplayUnit = units.MoleF.ToString();
            port.T_.DisplayUnit = units.T.ToString();
            port.P_.DisplayUnit = units.P.ToString();

            PortPropertyWorksheet_Load();

            UpdateValues();
        }

        private void PortPropertyWorksheet_Load()
        {
            DGV.Rows.Add(8);
            int column = 1;
           // port.Components.UpdateLiqVapFractionCompositions(port.Q.BaseValue);
            port.UpdateThermoProperties();

            ThermoProps propsVap = port.cc.ThermoVap;
            ThermoProps propsLiq = port.cc.ThermoLiq;

            DGV[0, 0].Value = "Quality ";
            DGV[0, 1].Value = "Mass Flow, " + uomDisplayList.MF.ToString();
            DGV[0, 2].Value = "Vol Flow, " + uomDisplayList.VF.ToString();
            DGV[0, 3].Value = "Molar Flow, " + uomDisplayList.MoleF.ToString();
            DGV[0, 4].Value = "Pressure , " + uomDisplayList.P.ToString();
            DGV[0, 5].Value = "Temperature , " + uomDisplayList.T.ToString();
            DGV[0, 6].Value = "Entropy, " + uomDisplayList.S.ToString();
            DGV[0, 7].Value = "Enthalpy, " + uomDisplayList.H.ToString();

            port.UpdateDisplysettings(uomDisplayList);

            DGV[column, 0] = new DataGridViewUOMCell(port.Q_);
            DGV[column, 1] = new DataGridViewUOMCell(port.MF_);
            DGV[column, 2] = new DataGridViewUOMCell(port.VF_);
            DGV[column, 3] = new DataGridViewUOMCell(port.MolarFlow_);
            DGV[column, 4] = new DataGridViewUOMCell(port.P_);
            DGV[column, 5] = new DataGridViewUOMCell(port.T_);
            DGV[column, 6] = new DataGridViewUOMCell(port.S_);
            DGV[column, 7] = new DataGridViewUOMCell(port.H_);
            column++;

            if (port.Q_ < 1) //; liquid props
            {
                DGV[column, 0].Value = 0;
                DGV[column, 1] = new DataGridViewUOMCell(new StreamProperty(ePropID.MF, port.MF_ * (1 - port.MassFractionLiquid()), SourceEnum.UnitOpCalcResult));
                DGV[column, 2] = new DataGridViewUOMCell(new StreamProperty(ePropID.VF, port.VF_ * (1 - port.VolFractionLiquid()), SourceEnum.UnitOpCalcResult));
                DGV[column, 3] = new DataGridViewUOMCell(new StreamProperty(ePropID.MOLEF, port.MolarFlow_ * (1 - port.cc.MoleFractionLiquid()), SourceEnum.UnitOpCalcResult));
                DGV[column, 4] = new DataGridViewUOMCell(new StreamProperty(ePropID.P, port.P_, SourceEnum.UnitOpCalcResult));
                DGV[column, 5] = new DataGridViewUOMCell(new StreamProperty(ePropID.T, port.T_, SourceEnum.UnitOpCalcResult));

                if (propsLiq != null)
                {
                    DGV[column, 6] = new DataGridViewUOMCell(new StreamProperty(ePropID.S, propsLiq.S, SourceEnum.UnitOpCalcResult));
                    DGV[column, 7] = new DataGridViewUOMCell(new StreamProperty(ePropID.H, propsLiq.H, SourceEnum.UnitOpCalcResult));
                }
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
            if (port.Q_ > 0)
            {
                DGV[column, 0].Value = 1;
                DGV[column, 1] = new DataGridViewUOMCell(new StreamProperty(ePropID.MF, port.MF_ * port.MassFractionLiquid(), SourceEnum.UnitOpCalcResult));
                DGV[column, 2] = new DataGridViewUOMCell(new StreamProperty(ePropID.VF, port.VF_ * port.VolFractionLiquid(), SourceEnum.UnitOpCalcResult));
                DGV[column, 3] = new DataGridViewUOMCell(new StreamProperty(ePropID.MOLEF, port.MolarFlow_ * port.cc.MoleFractionLiquid(), SourceEnum.UnitOpCalcResult));
                DGV[column, 4] = new DataGridViewUOMCell(new StreamProperty(ePropID.P, port.P_, SourceEnum.UnitOpCalcResult));
                DGV[column, 5] = new DataGridViewUOMCell(new StreamProperty(ePropID.T, port.T_, SourceEnum.UnitOpCalcResult));

                if (propsVap != null)
                {
                    DGV[column, 6] = new DataGridViewUOMCell(new StreamProperty(ePropID.S, propsVap.S, SourceEnum.UnitOpCalcResult));
                    DGV[column, 7] = new DataGridViewUOMCell(new StreamProperty(ePropID.H, propsVap.H, SourceEnum.UnitOpCalcResult));
                }
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

            port.UpdateCalcProperties();
            int Count;

            Count = 0;
            foreach (KeyValuePair<string, CalcProperty> item in port.PropsCalculated)
            {
                DGV2.Rows.Add();
                DGV2[0, Count].Value = item.Value.DisplayName;
                DGV2[1, Count].Value = item.Value.BaseValue;
                Count++;
            }

            //UpdateValues();
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.CurrentCell != OldCell)
                CBOX1.Visible = false;
        }

        private bool isSimple = false;

        public bool Simplify
        {
            get
            {
                return isSimple;
            }
            set
            {
                isSimple = value;
                if (isSimple)
                {
                    btnComposition.Visible = false;
                    btnAqueous.Visible = false;
                    Options.Visible = false;
                    Options2.Visible = false;
                    rbColdProps.Visible = false;
                    rbComposition.Visible = false;
                    rbDefault.Visible = false;
                    rbGasProps.Visible = false;
                    rbRefinery.Visible = false;

                    btnPropPlot.Visible = false;
                    btnTBPPlot.Visible = false;
                    btnDistData.Visible = false;
                    btnCopyFrom.Visible = false;
                }
                else
                {
                    btnComposition.Visible = true;
                    btnAqueous.Visible = true;
                    Options.Visible = true;
                    Options2.Visible = true;
                    rbColdProps.Visible = true;
                    rbComposition.Visible = true;
                    rbDefault.Visible = true;
                    rbGasProps.Visible = true;
                    rbRefinery.Visible = true;

                    btnPropPlot.Visible = true;
                    btnTBPPlot.Visible = true;
                    btnDistData.Visible = true;
                    btnCopyFrom.Visible = true;
                }
            }
        }

        public DrawMaterialStream DrawMaterialStream { get => drawMaterialStream; set => drawMaterialStream = value; }

        public void UpdateValues(bool ValueOnly = false)
        {
            port.UpdateDisplysettings(uomDisplayList);
            UpdateDisplayedUnits();

            if (DGV.RowCount > 7)
            {
                ((DataGridViewUOMCell)DGV[1, 0]).Update(port.Q_, valueonly: ValueOnly);
                ((DataGridViewUOMCell)DGV[1, 1]).Update(port.MF_, valueonly: ValueOnly);
                ((DataGridViewUOMCell)DGV[1, 2]).Update(port.VF_, valueonly: ValueOnly);
                ((DataGridViewUOMCell)DGV[1, 3]).Update(port.MolarFlow_, valueonly: ValueOnly);
                ((DataGridViewUOMCell)DGV[1, 4]).Update(port.P_, valueonly: ValueOnly);
                ((DataGridViewUOMCell)DGV[1, 5]).Update(port.T_, valueonly: ValueOnly);
                ((DataGridViewUOMCell)DGV[1, 6]).Update(port.S_, valueonly: ValueOnly);
                ((DataGridViewUOMCell)DGV[1, 7]).Update(port.H_, valueonly: ValueOnly);
            }
            else return;

            int column = 2;
           // Components liq = port.Components.LiquidComponents;
          //  Components vap = port.Components.VapourComponents;

            ((DataGridViewUOMCell)DGV[2, 0]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[2, 1]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[2, 2]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[2, 3]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[2, 4]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[2, 5]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[2, 6]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[2, 7]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[3, 0]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[3, 1]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[3, 2]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[3, 3]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[3, 4]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[3, 5]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[3, 6]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);
            ((DataGridViewUOMCell)DGV[3, 7]).Update(new StreamProperty(ePropID.NullUnits, double.NaN), true);

            if (port.Q_ < 1 && port.cc.ThermoLiq != null)
            {
                DGV[column, 0].Value = 0;
                ((DataGridViewUOMCell)DGV[column, 0]).Update(new StreamProperty(ePropID.Q, 1, SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 1]).Update(new StreamProperty(ePropID.MF, port.MF_ * (1 - port.MassFractionLiquid()), SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 2]).Update(new StreamProperty(ePropID.VF, port.VF_ * (1 - port.VolFractionLiquid()), SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 3]).Update(new StreamProperty(ePropID.MOLEF, port.MolarFlow_ * (1 - port.cc.MoleFractionLiquid()), SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 4]).Update(new StreamProperty(ePropID.P, port.P_, SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 5]).Update(new StreamProperty(ePropID.T, port.T_, SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 6]).Update(new StreamProperty(ePropID.S, port.S_, SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 7]).Update(new StreamProperty(ePropID.H, port.H, SourceEnum.UnitOpCalcResult));
            }

            column = 3;
            if (port.Q_ > 0 && port.cc.ThermoVap != null)
            {
                ((DataGridViewUOMCell)DGV[column, 0]).Update(new StreamProperty(ePropID.Q, 1, SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 1]).Update(new StreamProperty(ePropID.MF, port.MF_ * port.MassFractionLiquid(), SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 2]).Update(new StreamProperty(ePropID.VF, port.VF_ * port.VolFractionLiquid(), SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 3]).Update(new StreamProperty(ePropID.MOLEF, port.MolarFlow_ * port.cc.MoleFractionLiquid(), SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 4]).Update(new StreamProperty(ePropID.P, port.P_, SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 5]).Update(new StreamProperty(ePropID.T, port.T_, SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 6]).Update(new StreamProperty(ePropID.S, port.cc.ThermoVap.S, SourceEnum.UnitOpCalcResult));
                ((DataGridViewUOMCell)DGV[column, 7]).Update(new StreamProperty(ePropID.H, port.cc.ThermoVap.H, SourceEnum.UnitOpCalcResult));
            }

            if (port.Q_.IsEditable)
                DGV[1, 0].ReadOnly = false;
            else
                DGV[1, 0].ReadOnly = true;

            if (port.MF_.IsEditable)
                DGV[1, 1].ReadOnly = false;
            else
                DGV[1, 1].ReadOnly = true;

            if (port.VF_.IsEditable)
                DGV[1, 2].ReadOnly = false;
            else
                DGV[1, 2].ReadOnly = true;

            if (port.MolarFlow_.IsEditable)
                DGV[1, 3].ReadOnly = false;
            else
                DGV[1, 3].ReadOnly = true;

            if (port.P_.IsEditable)
                DGV[1, 4].ReadOnly = false;
            else
                DGV[1, 4].ReadOnly = true;

            if (port.T_.IsEditable)
                DGV[1, 5].ReadOnly = false;
            else
                DGV[1, 5].ReadOnly = true;

            if (port.S_.IsEditable)
                DGV[1, 6].ReadOnly = false;
            else
                DGV[1, 6].ReadOnly = true;

            if (port.H_.IsEditable)
                DGV[1, 7].ReadOnly = false;
            else
                DGV[1, 7].ReadOnly = true;

            for (int row = 1; row < DGV.RowCount; row++)
                for (int col = 2; col <= 3; col++)
                    if (DGV[col, row] is DataGridViewUOMCell)
                        DGV[col, row].ReadOnly = true;

            port.UpdateCalcProperties();

            int Count = 0;

            column = 1;

            if (port.Q_ == 0)
                column = 2;
            else if (port.Q_ == 1)
                column = 3;

            foreach (KeyValuePair<string, CalcProperty> item in port.PropsCalculated)
            {
                if (DGV2.InvokeRequired)
                {
                    DGV2.Invoke(new Action(() => DGV2[1, Count].Value = ""));
                    DGV2.Invoke(new Action(() => DGV2[2, Count].Value = ""));

                    DGV2.Invoke(new Action(() => DGV2[3, Count].Value = ""));
                    DGV2.Invoke(new Action(() => DGV2[0, Count].Value = item.Value.DisplayName + " " + item.Value.DisplayUnit));
                    DGV2.Invoke(new Action(() => DGV2[1, Count].Value = item.Value.BaseValue));
                    DGV2.Invoke(new Action(() => DGV2[column, Count].Value = item.Value.BaseValue));
                }
                else
                {
                    DGV2[1, Count].Value = "";
                    DGV2[2, Count].Value = "";

                    DGV2[3, Count].Value = "";
                    DGV2[0, Count].Value = item.Value.DisplayName + " " + item.Value.DisplayUnit;
                    DGV2[1, Count].Value = item.Value.BaseValue;
                    DGV2[column, Count].Value = item.Value.BaseValue;
                }
                Count++;
            }

            DGV.Refresh();
        }

        private void btnComposition_Click(object sender, EventArgs e)
        {
            CompositionForm cf = new(drawMaterialStream);
            cf.Show();
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SourceEnum source;
            if (DGV.RowCount > e.RowIndex && DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
            {
                if (!double.IsNaN(cell.Value))
                    source = cell.uom.Source;
                else
                    source = SourceEnum.Empty;

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
            if (e.RowIndex < 0 || e.ColumnIndex < 1 || e.ColumnIndex > 1)
                return;

            if (DGV.CurrentCell.ReadOnly)
                return;

            DataGridViewUOMCell cell = (DataGridViewUOMCell)DGV.CurrentCell;

            Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

            CBOX1.DataSource = cell.uom.AllUnits;
            CBOX1.Location = new Point(r.Right + DGV.Left, r.Top + DGV.Top + 1);
            CBOX1.Text = cell.uom.DisplayUnit;
            if (cell.uom.IsInput)
            {
                CBOX1.BringToFront();
                CBOX1.Visible = true;
            }
            OldCell = DGV.CurrentCell;
        }

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            if (DGV.CurrentCell is DataGridViewUOMCell cell)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        if (cell.uom.origin == SourceEnum.Input)
                        {
                            port.Properties[cell.Propid].Clear();
                            cell.Value = double.NaN; // force an update
                            UpdateValues();
                        }
                        //ResetNonFixedValues();
                        port.ClearPortCalcValues();
                        port.Flash(calcderivatives: true);
                        UpdateValues(ValueOnly: true);
                        RaiseChangeEvent();
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
            if (DGV.RowCount > 7)
            {
                DGV[0, 0].Value = "Quality ";
                DGV[0, 1].Value = "Mass Flow, " + uomDisplayList.MF.ToString();
                DGV[0, 2].Value = "Vol Flow, " + uomDisplayList.VF.ToString();
                DGV[0, 3].Value = "Molar Flow, " + uomDisplayList.MoleF.ToString();
                DGV[0, 4].Value = "Pressure , " + uomDisplayList.P.ToString();
                DGV[0, 5].Value = "Temperature , " + uomDisplayList.T.ToString();
                DGV[0, 6].Value = "Entropy, " + uomDisplayList.S.ToString();
                DGV[0, 7].Value = "Enthalpy, " + uomDisplayList.H.ToString();
            }
        }

        private void CBOX1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string newUnit = cb.SelectedItem.ToString();

            switch (DGV.CurrentCell)
            {
                case DataGridViewUOMCell cell:  // Porperty value column
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

                case DataGridViewCell cell2: //  description column
                    DataGridViewUOMCell cell3 = (DataGridViewUOMCell)DGV[cell2.ColumnIndex + 1, cell2.RowIndex];
                    if (newUnit != "" || newUnit != null)
                    {
                        cell3.uom.DisplayUnit = newUnit;
                        cell3.Update();
                        switch (cell3.uom.Propid)
                        {
                            case ePropID.MF:
                                uomDisplayList.MF = (MassFlowUnit)Enum.Parse(typeof(MassFlowUnit), newUnit);
                                break;

                            case ePropID.VF:
                                uomDisplayList.VF = (VolFlowUnit)Enum.Parse(typeof(VolFlowUnit), newUnit);
                                break;

                            case ePropID.MOLEF:
                                uomDisplayList.MoleF = (MoleFlowUnit)Enum.Parse(typeof(MoleFlowUnit), newUnit);
                                break;

                            case ePropID.T:
                                uomDisplayList.T = (TemperatureUnit)Enum.Parse(typeof(TemperatureUnit), newUnit);
                                break;

                            case ePropID.P:
                                uomDisplayList.P = (PressureUnit)Enum.Parse(typeof(PressureUnit), newUnit);
                                break;

                            case ePropID.MassEnthalpy:
                                uomDisplayList.H = (EnthalpyUnit)Enum.Parse(typeof(EnthalpyUnit), newUnit);
                                break;

                            case ePropID.MassEntropy:
                                uomDisplayList.S = (EntropyUnit)Enum.Parse(typeof(EntropyUnit), newUnit);
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
            if (e.Button == MouseButtons.Right && e.ColumnIndex == 0
                && DGV[e.ColumnIndex + 1, e.RowIndex] is DataGridViewUOMCell uomCell)
            {
                Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                CBOX1.DataSource = uomCell.uom.AllUnits;
                CBOX1.Location = new Point(r.Right + DGV.Left, r.Top + DGV.Top + 1);
                CBOX1.Text = uomCell.uom.DisplayUnit;
                CBOX1.BringToFront();
                CBOX1.Visible = true;
                OldCell = DGV.CurrentCell;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChartForm cf = new(port);
            cf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Chart2 cf = new(port);
            cf.Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            DistilationDataForm ddf = new(port);
            ddf.Show();
        }

        private void CopyStream_Click(object sender, EventArgs e)
        {
            DrawArea da = DrawMaterialStream.Owner;
            if (da is null)
                return;
            List<DrawMaterialStream> streams = da.GraphicsList.ReturnMaterialStreams();
            CopyStreamDLG csdlg = new(streams);
            DialogResult dr = csdlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.port.cc = csdlg.stream.Components.Clone();
                this.port.Properties = csdlg.stream.Port.Properties.Clone();
                if (this.port.MolarFlow_.IsKnown)
                    this.port.MolarFlow_.origin = SourceEnum.Input;

                port.IsDirty = true;
                this.port.cc.Origin = SourceEnum.Input;
            }
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.CurrentCell is DataGridViewUOMCell cell
                && cell != null && double.TryParse(DGV.CurrentCell.Value.ToString(), out double res))
            {
                UOMProperty v = new UOMProperty(cell.Propid, res, cell.uom.DisplayUnit);
                port.SetPortValue(cell.uom.Propid, v.BaseValue, SourceEnum.Input, false);
                //cell.uom.ValueIn(cell.uom.DisplayUnit, res);   // set port value from call value
                if (!double.IsNaN(res))
                    cell.uom.Source = SourceEnum.Input;
            }

            port.ClearPortCalcValues();
            port.Flash(calcderivatives: true);
            UpdateValues(ValueOnly: true);
            RaiseChangeEvent();
        }

        private void DGV_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                DGV[e.ColumnIndex, e.RowIndex] is DataGridViewUOMCell cell)
            {
                UOMProperty prop = cell.uom;
                this.DoDragDrop(prop, DragDropEffects.Move);
            }
        }

        public void ResetNonFixedValues()
        {
            port.ClearProps();  // keep composition
            port.IsDirty = true;
            Port p = port.ConnectedPortNext;
            if (p is not null)
            {
                p.ClearProps();
                p.IsDirty = true;
            }
        }
    }
}