using ModelEngine;
using ModelEngine.BinaryInteraction;
using ModelEngine;
using System;
using System.Drawing;
using System.Windows.Forms;
using Units.PortForm;

namespace Units
{
    public enum eDisplayState
    { Mole, Mass, Vol };

    public partial class CompositionForm : Form
    {
        public Port_Material port;

        //readonly DrawMaterialStreamstream;
        private eDisplayState CurrentState = eDisplayState.Mole;

        internal CompositionForm(DrawMaterialStream stream)
        {
            //this.stream=stream;
            port = stream.Stream.Port;
            InitializeComponent();
        }

        private void CompositionForm_Load(object sender, System.EventArgs e)
        {
            FillFractionData();
            switch (port.cc.Origin)
            {
                case SourceEnum.Transferred:
                case SourceEnum.UnitOpCalcResult:
                    DGV.ReadOnly = true;
                    break;

                default:
                    DGV.ReadOnly = false;
                    break;
            }
        }

        private void FillFractionData()
        {
            BaseComp bc;
            int column = 1;
            for (int i = 0; i < port.ComponentList.Count; i++)
            {
                DGV.Rows.Add(1);
                bc = port.ComponentList[i];
                DGV[0, i].Value = bc.Name;
                DataGridViewCompCell cell = new(bc);
                cell.origin = port.cc.Origin;
                DGV[1, i] = cell;
            }

            if (port.cc.LiqPhaseMolFractions != null)
            {
                column++;
                double[] MolFracs = port.cc.LiqPhaseMolFractions;
                double[] MassFracs = port.cc.LiqPhaseMassFractions;
                double[] VolFracs = port.cc.LiqPhaseVolFractions;

                for (int row = 0; row < MolFracs.Length; row++)
                {
                    DataGridViewCompCell cell = new(port.cc[row].Clone());
                    cell.origin = port.cc.Origin;
                    if (RBMoleFraction.Checked)
                        DGV[column, row].Value = MolFracs[row].ToString("F4");
                    else if (RBMassFraction.Checked)
                        DGV[column, row].Value = MassFracs[row].ToString("F4");
                    else if (RBStdLiqVolume.Checked)
                        DGV[column, row].Value = VolFracs[row].ToString("F4");
                }
            }

            if (port.cc.VapPhaseMolFractions != null)
            {
                column++;
                double[] MolFracs = port.cc.VapPhaseMolFractions;
                double[] MassFracs = port.cc.VapPhaseMassFractions;
                double[] VolFracs = port.cc.VapPhaseVolFractions;

                for (int row = 0; row < MolFracs.Length; row++)
                {
                    DataGridViewCompCell cell = new(port.cc[row].Clone());
                    cell.origin = port.cc.Origin;
                    if (RBMoleFraction.Checked)
                        DGV[column, row].Value = MolFracs[row].ToString("F4");
                    else if (RBMassFraction.Checked)
                        DGV[column, row].Value = MassFracs[row].ToString("F4");
                    else if (RBStdLiqVolume.Checked)
                        DGV[column, row].Value = VolFracs[row].ToString("F4");
                }
            }
        }

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = DGV.GetClipboardContent();
                Clipboard.SetDataObject(d);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int row = DGV.CurrentCell.RowIndex;
                int col = DGV.CurrentCell.ColumnIndex;
                foreach (string line in lines)
                {
                    if (row < DGV.RowCount && line.Length > 0)
                    {
                        string[] cells = line.Split('\t');
                        for (int i = 0; i < cells.GetLength(0); ++i)
                            if (col + i < this.DGV.ColumnCount)
                                DGV[col + i, row].Value = Convert.ChangeType(cells[i], DGV[col + i, row].ValueType);
                            else
                                break;
                        row++;
                    }
                    else
                        break;
                }
            }
        }

        private void UpdateFractionData()
        {
            BaseComp bc;
            int column = 1;
            for (int i = 0; i < port.ComponentList.Count; i++)
            {
                bc = port.ComponentList[i];
                DGV[0, i].Value = bc.Name;
                DataGridViewCompCell cell = new(bc);
                cell.origin = port.cc.Origin;
                DGV[1, i] = cell;
            }

            if (port.cc.LiqPhaseMolFractions != null)
            {
                column++;
                double[] MolFracs = port.cc.LiqPhaseMolFractions;
                double[] MassFracs = port.cc.LiqPhaseMassFractions;
                double[] VolFracs = port.cc.LiqPhaseVolFractions;

                for (int row = 0; row < MolFracs.Length; row++)
                {
                    DataGridViewCompCell cell = new(port.cc[row].Clone());
                    cell.origin = port.cc.Origin;
                    if (RBMoleFraction.Checked)
                        DGV[column, row].Value = MolFracs[row].ToString("F4");
                    else if (RBMassFraction.Checked)
                        DGV[column, row].Value = MassFracs[row].ToString("F4");
                    else if (RBStdLiqVolume.Checked)
                        DGV[column, row].Value = VolFracs[row].ToString("F4");
                }
            }

            if (port.cc.VapPhaseMolFractions != null)
            {
                column++;
                double[] MolFracs = port.cc.VapPhaseMolFractions;
                double[] MassFracs = port.cc.VapPhaseMassFractions;
                double[] VolFracs = port.cc.VapPhaseVolFractions;

                for (int row = 0; row < MolFracs.Length; row++)
                {
                    DataGridViewCompCell cell = new(port.cc[row].Clone());
                    cell.origin = port.cc.Origin;
                    if (RBMoleFraction.Checked)
                        DGV[column, row].Value = MolFracs[row].ToString("F4");
                    else if (RBMassFraction.Checked)
                        DGV[column, row].Value = MassFracs[row].ToString("F4");
                    else if (RBStdLiqVolume.Checked)
                        DGV[column, row].Value = VolFracs[row].ToString("F4");
                }
            }
        }

        private void Normalise_Click(object sender, EventArgs e)
        {
            port.cc.Origin = SourceEnum.Input;
            Normalise();
            UpdateFractionData();
            DGV_UpdateValue();
            Refresh();
        }

        public void Normalise()
        {
            if (port.ComponentList.Count > 0)
            {
                switch (CurrentState)
                {
                    case eDisplayState.Mole:
                        port.cc.NormaliseFractions(FlowFlag.Molar);
                        break;

                    case eDisplayState.Mass:
                        port.cc.NormaliseFractions(FlowFlag.Mass);
                        break;

                    case eDisplayState.Vol:
                        port.cc.NormaliseFractions(FlowFlag.LiqVol);
                        break;
                }
            }

            DGV_UpdateValue();

            port.Flash();

            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm is PortPropertyForm2 form)
                    form.UpdateValues();
            }
        }

        private void DGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell dgcell = null;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                dgcell = DGV[e.ColumnIndex, e.RowIndex];

            if (dgcell is DataGridViewCompCell compcell)
            {
                compcell.CompValueUpdate();
            }
        }

        private void DGV_UpdateValue()
        {
            int column = 1;
            BaseComp bc;

            for (int i = 0; i < port.ComponentList.Count; i++)
            {
                if (DGV[1, i] is DataGridViewCompCell compcell)
                {
                    compcell.ValueUpdate(CurrentState);
                }
            }

            if (port.cc.LiqPhaseMolFractions != null)
            {
                column++;
                double[] MolFracs = port.cc.LiqPhaseMolFractions;
                double[] MassFracs = port.cc.LiqPhaseMassFractions;
                double[] VolFracs = port.cc.LiqPhaseVolFractions;

                for (int row = 0; row < MolFracs.Length; row++)
                {
                    DataGridViewCompCell cell = new(port.cc[row].Clone());
                    cell.origin = port.cc.Origin;
                    if (RBMoleFraction.Checked)
                        DGV[column, row].Value = MolFracs[row].ToString("F4");
                    else if (RBMassFraction.Checked)
                        DGV[column, row].Value = MassFracs[row].ToString("F4");
                    else if (RBStdLiqVolume.Checked)
                        DGV[column, row].Value = VolFracs[row].ToString("F4");
                }
            }

            if (port.cc.VapPhaseMolFractions != null)
            {
                column++;
                double[] MolFracs = port.cc.VapPhaseMolFractions;
                double[] MassFracs = port.cc.VapPhaseMassFractions;
                double[] VolFracs = port.cc.VapPhaseVolFractions;

                for (int row = 0; row < MolFracs.Length; row++)
                {
                    DataGridViewCompCell cell = new(port.cc[row].Clone());
                    cell.origin = port.cc.Origin;
                    if (RBMoleFraction.Checked)
                        DGV[column, row].Value = MolFracs[row].ToString("F4");
                    else if (RBMassFraction.Checked)
                        DGV[column, row].Value = MassFracs[row].ToString("F4");
                    else if (RBStdLiqVolume.Checked)
                        DGV[column, row].Value = VolFracs[row].ToString("F4");
                }
            }
        }

        private void RB_CheckedChanged(object sender, EventArgs e)
        {
            if (RBMoleFraction.Checked)
                CurrentState = eDisplayState.Mole;
            if (RBMassFraction.Checked)
                CurrentState = eDisplayState.Mass;
            if (RBStdLiqVolume.Checked)
                CurrentState = eDisplayState.Vol;

            DGV_UpdateValue();
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SourceEnum source;

            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewCompCell cell)
            {
                if (cell.Value != null)
                    source = cell.origin;
                else
                    source = SourceEnum.Empty;

                if (cell != null && cell.comp != null)
                {
                    if (source != SourceEnum.UnitOpCalcResult)
                    {
                        if (source == SourceEnum.Empty)
                            e.CellStyle.ForeColor = Color.Red;
                        if (source == SourceEnum.Input)
                            e.CellStyle.ForeColor = Color.Blue;
                        if (source == SourceEnum.Transferred)
                        {
                            e.CellStyle.ForeColor = Color.Black;
                        }
                    }
                    else { e.CellStyle.ForeColor = Color.Black; }
                }
            }
            //e.FormattingApplied=true
        }

        private void EraseCompositions_Click(object sender, EventArgs e)
        {
            if (port != null && port.cc != null)
            {
                for (int i = 0; i < port.cc.Count; i++)
                {
                    port.cc[i].MoleFraction = double.NaN;
                    if (port != null
                    && port.cc != null
                    && port.cc.Count > 0)
                        port.cc[i].MoleFraction = double.NaN;
                }

                if (port != null && port.cc != null)
                    port.cc.Origin = SourceEnum.Empty;

                port.cc.Origin = SourceEnum.Empty;
            }
        }

        private void CompositionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Normalise();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamProperties props = new(port.cc);
            props.Show();
        }

        private void DGV_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            port.cc.Origin = SourceEnum.Input;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (port.cc.Origin == SourceEnum.Input)
            {
                Normalise();
                UpdateFractionData();
            }
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BIPForm bf = new(port);
            bf.Show();
        }
    }
}