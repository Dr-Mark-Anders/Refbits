using ModelEngine;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Units.UOM;

namespace Units
{
    public partial class SpecificationSheet : UserControl
    {
        internal DrawColumn drawColumn;
        private Specification currentspec;

        internal SpecificationSheet(DrawColumn column)
        {
            InitializeComponent();
            this.drawColumn = column;
            DGV.AutoGenerateColumns = false;
            condenserTypeControl1.InitWithColumnData(drawColumn, this);
        }

        public SpecificationSheet()
        {
            InitializeComponent();
        }

        private bool isdatasetting = false;

        public void SetData()
        {
            CheckDegOfFreedom();
            isdatasetting = true;
            DGV.Rows.Clear();
            if (drawColumn != null && drawColumn.Column.Specs != null)

                for (int i = 0; i < drawColumn.Column.Specs.Count(); i++)
                {
                    Specification spec = drawColumn.Column.Specs[i];

                    if (spec.graphicSpecType != eSpecType.TrayDuty)
                    {
                        int row = DGV.Rows.Add();
                        DGV[0, row].Value = spec.SpecName + ", " + spec.unitConvert.UnitDescriptor;
                        DGV[1, row].Value = spec.SpecDisplayValue;
                        DGV[2, row].Value = spec.DisplayValue.ToString("G4");
                        DGV[3, row].Value = spec.IsActive;
                        DGV[5, row].Value = spec.Guid;
                        if (spec.IsConfigured)
                            DGV.Rows[row].DefaultCellStyle.BackColor = Color.White;
                        else
                            DGV.Rows[row].DefaultCellStyle.BackColor = Color.Red;
                    }
                }

            DGV.Refresh();
            DGV.Invalidate();

            isdatasetting = false;
        }

        internal void SetData(DrawColumn column)
        {
            this.drawColumn = column;
            SetData();
            CheckDegOfFreedom();
            //column.Isdirty = true;
            //column.IsSolved = false;

            upPressures.Clear();
            upPressures.Add(column.Column.MainSectionStages.TopTray.P, "Top Stage P");
            upPressures.Add(column.Column.MainSectionStages.ovhdDP, "OVHD P. Drop");
            upPressures.Add(column.Column.MainSectionStages.BottomTray.P, "Bottom P");
        }

        public void StoreOneValue(DataGridViewCellEventArgs e)
        {
            if (drawColumn != null && drawColumn.Column != null && !isdatasetting)
            {
                int row = e.RowIndex;
                DataGridViewRow dgvrow = DGV.Rows[row];
                Specification spec = drawColumn.Column.Specs[(Guid)dgvrow.Cells[5].Value];
                if (dgvrow.Cells[0].Value is not null && dgvrow.Cells[1].Value is not null)
                {
                    spec.SpecName = dgvrow.Cells[0].Value.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
                    spec.IsActive = (bool)dgvrow.Cells[3].Value;

                    if (double.TryParse(dgvrow.Cells[1].Value.ToString(), out double res))
                        spec.SpecDisplayValue = res;
                }
            }
            CheckDegOfFreedom();
        }

        public void StoreData()
        {
            if (drawColumn != null && drawColumn.Column != null && !isdatasetting)
            {
                for (int i = 0; i < DGV.RowCount; i++)
                {
                    Specification spec = drawColumn.Column.Specs[(Guid)DGV[5, i].Value];
                    spec.SpecName = DGV[0, i].Value.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
                    spec.IsActive = (bool)DGV[3, i].Value;
                    if (DGV[1, i].Value is not null)
                    {
                        if (double.TryParse(DGV[1, i].Value.ToString(), out double res))
                            spec.SpecDisplayValue = res;
                    }
                }
            }
            CheckDegOfFreedom();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CBOX1.Visible = false;
            int sum = 0;
            foreach (DataGridViewRow dgvr in DGV.Rows)
                if (DGV.Rows.Count > 3 && Convert.ToBoolean(dgvr.Cells[3].Value))
                    sum++;

            StoreData();
        }

        public bool CheckDegOfFreedom()
        {
            if (drawColumn != null)
            {
                Column column = drawColumn.Column;
                int activespecs = column.Specs.GetActiveSpecsCount(false);
                int requiredspecs = column.RequiredSpecsCount;
                txtActSpecs.Text = activespecs.ToString();
                txtReqiredSpecs.Text = requiredspecs.ToString();
                txtDegOfFreedom.Text = (requiredspecs - activespecs).ToString();
                column.Specs.DegreesOfFreedom = (requiredspecs - activespecs);
            }

            return true;
        }

        private void DataGridView1_CelldoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.ColumnIndex > 1)
                return;

            if (DGV.CurrentCell.ReadOnly)
                return;

            if (e.ColumnIndex == 1)
            {
                Guid specguid = (Guid)DGV[5, e.RowIndex].Value;

                Rectangle r = DGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                currentspec = this.drawColumn.Column.Specs[specguid];

                CBOX1.Items.Clear();
                if (currentspec.UOM != null)
                    CBOX1.Items.AddRange(currentspec.UOM.AllUnits);
                CBOX1.Location = new Point(r.Right + DGV.Left, r.Top + 1 + DGV.Top);
                CBOX1.Text = currentspec.currentUnits;
                CBOX1.BringToFront();
                CBOX1.Visible = true;
            }
            else if (e.ColumnIndex == 0)
            {
                DataGridViewCell cell = DGV.CurrentCell;
                Guid specguid = (Guid)DGV[5, e.RowIndex].Value;
                currentspec = this.drawColumn.Column.Specs[specguid];
                cell.Value = currentspec.SpecName;
            }
            else
                EditSpecs(e.RowIndex);
            drawColumn.Column.IsDirty = true;
        }

        private static RadioButton GetCheckedRadio(Control container)
        {
            foreach (var control in container.Controls)
            {
                if (control is RadioButton radio && radio.Checked)
                    return radio;
            }
            return null;
        }

        public void EditSpecs(int rowindex)
        {
            if (!GlobalModel.IsRunning)
                drawColumn.UpdateAttachedModel();

            if (drawColumn.Column.Specs != null)
            {
                Guid specguid = (Guid)DGV["SpecGuid", rowindex].Value;
                Specification spec = drawColumn.Column.Specs[specguid];

                switch (spec.graphicSpecType)
                {
                    case eSpecType.VapProductDraw:
                    case eSpecType.LiquidProductDraw:
                        ProductDrawDialog flowdlg = new(drawColumn, spec);
                        flowdlg.ShowDialog();
                        break;

                    case eSpecType.RefluxRatio:
                        TrayDialog rrtray = new(drawColumn, spec);
                        rrtray.ShowDialog();
                        break;

                    case eSpecType.TrayNetLiqFlow:
                    case eSpecType.TrayNetVapFlow:
                    case eSpecType.Temperature:
                        TrayDialog fdtray = new(drawColumn, spec);
                        fdtray.ShowDialog();
                        break;

                    case eSpecType.PADeltaT:
                    case eSpecType.PARetT:
                        PASpecDialog sd2 = new(drawColumn, spec);
                        sd2.ShowDialog();
                        break;

                    case eSpecType.PADuty:
                        PAEnergySpecDialog PAEnergy = new(drawColumn, spec);
                        PAEnergy.ShowDialog();
                        break;

                    case eSpecType.Energy:
                        EnergySpecDialog Energy = new(drawColumn, spec);
                        Energy.ShowDialog();
                        break;

                    case eSpecType.PAFlow:
                        PAFlowSpecDialog sd3 = new(drawColumn, spec);
                        sd3.ShowDialog();
                        break;

                    case eSpecType.LiquidStream:
                    case eSpecType.VapStream:
                        StreamFlowDialog sfd = new(drawColumn, spec);
                        sfd.ShowDialog();
                        break;

                    case eSpecType.DistSpec:
                        StreamDistDialog sdd = new(drawColumn, spec);
                        sdd.ShowDialog();
                        break;
                }
                spec.IsConfigured = true;
                //ChangeUnits(spec);
            }
            drawColumn.DesignChanged();
        }

        public static void ChangeUnits(Specification spec)
        {
            switch (spec.engineSpecType)
            {
                case eSpecType.TrayNetLiqFlow:
                    spec.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    spec.currentUnits = MoleFlowUnit.kgmol_hr.ToString();
                    break;

                case eSpecType.TrayNetVapFlow:
                    spec.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    spec.currentUnits = MoleFlowUnit.kgmol_hr.ToString();
                    break;

                case eSpecType.Temperature:
                    spec.UOM = UOMUtility.GetUOM(ePropID.T);
                    spec.currentUnits = TemperatureUnit.Celsius.ToString();
                    break;

                case eSpecType.RefluxRatio:
                    spec.UOM = UOMUtility.GetUOM(ePropID.NullUnits);
                    break;

                case eSpecType.Energy:
                    spec.UOM = UOMUtility.GetUOM(ePropID.EnergyFlow);
                    spec.currentUnits = EnergyFlowUnit.MW.ToString();
                    break;

                case eSpecType.PAFlow:
                    spec.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    spec.currentUnits = MoleFlowUnit.kgmol_hr.ToString();
                    break;

                case eSpecType.PARetT:
                    spec.UOM = UOMUtility.GetUOM(ePropID.T);
                    spec.currentUnits = TemperatureUnit.Celsius.ToString();
                    break;

                case eSpecType.PADeltaT:
                    spec.UOM = UOMUtility.GetUOM(ePropID.DeltaT);
                    spec.currentUnits = DeltaTemperatureUnit.Celsius.ToString();
                    break;

                case eSpecType.PADuty:
                    spec.UOM = UOMUtility.GetUOM(ePropID.EnergyFlow);
                    spec.currentUnits = EnergyFlowUnit.MW.ToString();
                    break;

                case eSpecType.VapProductDraw:
                    spec.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    spec.currentUnits = MoleFlowUnit.kgmol_hr.ToString();
                    break;

                case eSpecType.LiquidProductDraw:
                    spec.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    spec.currentUnits = MoleFlowUnit.kgmol_hr.ToString();
                    break;

                default:
                    break;
            }
        }

        private void AddSpec_Click(object sender, EventArgs e)
        {
            //SpecGroupBox.

            RadioButton r = GetCheckedRadio(SpecGroupBox);
            Specification s = new();

            switch (r.Name)
            {
                case "rbLiquidFlow":
                    s.propID = ePropID.MOLEF;
                    s.graphicSpecType = eSpecType.TrayNetLiqFlow;
                    s.SpecName = "Liquid Flow";
                    s.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    break;

                case "rbVapourFlow":
                    s.propID = ePropID.MOLEF;
                    s.graphicSpecType = eSpecType.TrayNetVapFlow;
                    s.SpecName = "Vapour Flow";
                    s.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    break;

                case "rbStageTemperature ":
                    s.propID = ePropID.T;
                    s.graphicSpecType = eSpecType.Temperature;
                    s.SpecName = "StageTemperature ";
                    s.UOM = UOMUtility.GetUOM(ePropID.T);
                    break;

                case "rbRefluxRate":
                    s.propID = ePropID.MOLEF;
                    s.graphicSpecType = eSpecType.TrayNetLiqFlow;
                    s.engineStageGuid = Guid.Empty;
                    s.SpecName = "RefluxRate";
                    s.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    break;

                case "rbRefluxRatio":
                    s.propID = ePropID.NullUnits;
                    s.graphicSpecType = eSpecType.RefluxRatio;
                    s.engineStageGuid = Guid.Empty;
                    s.SpecName = "RefluxRatio";
                    s.UOM = UOMUtility.GetUOM(ePropID.NullUnits);
                    break;

                case "rbLiquidDrawRate":
                    s.propID = ePropID.MOLEF;
                    s.graphicSpecType = eSpecType.LiquidProductDraw;
                    s.engineStageGuid = Guid.Empty;
                    s.SpecName = "LiquidDrawRate";
                    s.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    break;

                case "rbVapourDrawRate":
                    s.propID = ePropID.MOLEF;
                    s.graphicSpecType = eSpecType.VapProductDraw;
                    s.engineStageGuid = Guid.Empty;
                    s.SpecName = "VapourDrawRate";
                    s.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    break;

                case "rbPAFlow":
                    s.propID = ePropID.MOLEF;
                    s.graphicSpecType = eSpecType.PAFlow;
                    s.SpecName = "PA Flow";
                    s.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    break;

                case "rbPARetT":
                    s.propID = ePropID.T;
                    s.graphicSpecType = eSpecType.PARetT;
                    s.SpecName = "PA Ret T";
                    s.UOM = UOMUtility.GetUOM(ePropID.T);
                    break;

                case "rbPADeltaT":
                    s.propID = ePropID.DeltaT;
                    s.graphicSpecType = eSpecType.PADeltaT;
                    s.SpecName = "PA Delta T";
                    s.UOM = UOMUtility.GetUOM(ePropID.DeltaT);
                    break;

                case "rbPADuty":
                    s.propID = ePropID.EnergyFlow;
                    s.graphicSpecType = eSpecType.PADuty;
                    s.SpecName = "PA Duty";
                    s.UOM = UOMUtility.GetUOM(ePropID.EnergyFlow);
                    break;

                case "rbLiquidStreamRate":
                    s.propID = ePropID.MOLEF;
                    s.graphicSpecType = eSpecType.LiquidStream;
                    s.SpecName = "Liquid Stream";
                    s.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    break;

                case "rbVapStreamRate":
                    s.propID = ePropID.MOLEF;
                    s.graphicSpecType = eSpecType.VapStream;
                    s.SpecName = "Vap Stream";
                    s.UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                    break;

                case "rdDistSpec":
                    s.propID = ePropID.T;
                    s.graphicSpecType = eSpecType.DistSpec;
                    s.engineSpecType = eSpecType.DistSpec;
                    s.SpecName = "Distillation Point ";
                    s.UOM = UOMUtility.GetUOM(ePropID.T);
                    break;
            }
            drawColumn.Column.Specs.Add(s);
            SetData(drawColumn);
            drawColumn.Column.IsDirty = true;
        }

        private void DelSpec_Click(object sender, EventArgs e)
        {
            for (int i = DGV.Rows.Count - 1; i >= 0; i--)
            {
                if (DGV.Rows[i].Selected)
                {
                    Guid g = (Guid)DGV.Rows[i].Cells[5].Value;
                    drawColumn.Column.Specs.RemoveBySpecGuid(g);
                    DGV.Rows.RemoveAt(i);
                }
            }
            SetData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex == DGV.Columns["Edit"].Index)
                EditSpecs(e.RowIndex);

            DGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
            drawColumn.Column.IsDirty = true;
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            StoreOneValue(e);
        }

        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).CurrentCell.EditedFormattedValue != null)
                Debug.Print(((DataGridView)sender).CurrentCell.EditedFormattedValue.ToString());
        }

        private void DataGridView1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < DGV.RowCount; i++)
            {
                Guid specguid = (Guid)DGV[5, i].Value;
                Specification spec = drawColumn.Column.Specs[specguid];

                if (spec != null && spec.IsConfigured)
                    DGV.Rows[i].DefaultCellStyle.BackColor = Color.White;
                else
                    DGV.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        private void CBOX1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CBOX1.Visible = false;
            currentspec.currentUnits = CBOX1.SelectedItem.ToString();
            currentspec.UnitDescriptor = currentspec.unitConvert.UnitDescriptor;
            SetData();
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            StoreData();
        }
    }
}