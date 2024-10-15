using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units.DrawingObjects.UnitDialogs
{
    public partial class TableDialog : Form
    {
        private TableControl ds;
        private List<DrawMaterialStream> streams;

        internal TableDialog(TableControl ds, List<DrawMaterialStream> streams)
        {
            InitializeComponent();
            this.streams = streams;
            this.ds = ds;
            if (streams.Count > 0)
            {
                dgv.Rows.Add(streams.Count);
                int count = 0;
                foreach (var item in streams)
                {
                    dgv[0, count].Value = item.Name;
                    if (ds.activestreams.Contains(item.Guid))
                        dgv[1, count].Value = true;
                    //dgv[1, count].Value = item;
                    count++;
                }
            }

            List<ePropID> tempprops = new List<ePropID>();
            tempprops.AddRange(ds.props);
            foreach (ePropID item in tempprops)
            {
                switch (item)
                {
                    case ePropID.NullUnits:
                        break;

                    case ePropID.T:
                        cbTemperature.Checked = true;
                        break;

                    case ePropID.P:
                        CBPressure.Checked = true;
                        break;

                    case ePropID.H:
                        break;

                    case ePropID.S:
                        break;

                    case ePropID.Z:
                        break;

                    case ePropID.F:
                        break;

                    case ePropID.MOLEF:
                        CBMolarFlow.Checked = true;
                        break;

                    case ePropID.MF:
                        cbMassFlow.Checked = true;
                        break;

                    case ePropID.VF:
                        cbVolFlow.Checked = true;
                        break;

                    case ePropID.Q:
                        break;

                    case ePropID.SG:
                        break;

                    case ePropID.Density:
                        break;

                    case ePropID.FUG:
                        break;

                    case ePropID.DeltaP:
                        break;

                    case ePropID.SG_ACT:
                        break;

                    case ePropID.VolFlow_ACT:
                        break;

                    case ePropID.Density_ACT:
                        break;

                    case ePropID.DeltaT:
                        break;

                    case ePropID.EnergyFlow:
                        break;

                    case ePropID.LiquidVolumeFlow:
                        break;

                    case ePropID.Mass:
                        break;

                    case ePropID.LHV:
                        break;

                    case ePropID.SpecificVolume:
                        break;

                    case ePropID.DynViscosity:
                        break;

                    case ePropID.ElectricalFlow:
                        break;

                    case ePropID.Fueloil:
                        break;

                    case ePropID.HeatFlowRate:
                        cbHeatFlow.Checked = true;
                        break;

                    case ePropID.HeatFlux:
                        break;

                    case ePropID.KinViscosity:
                        break;

                    case ePropID.Length:
                        break;

                    case ePropID.LiquidVolume:
                        break;

                    case ePropID.Luminance:
                        break;

                    case ePropID.MolarSpecificEnergy:
                        break;

                    case ePropID.SpecificEnergy:
                        break;

                    case ePropID.SurfaceTension:
                        break;

                    case ePropID.ThermalConductivity:
                        break;

                    case ePropID.Time:
                        break;

                    case ePropID.VapourVolume:
                        break;

                    case ePropID.VapourVolumeFlow:
                        break;

                    case ePropID.Velocity:
                        break;

                    case ePropID.Voltage:
                        break;

                    case ePropID.VolumeRatio:
                        break;

                    case ePropID.VolumeSpecificEnergy:
                        break;

                    case ePropID.MassEnthalpy:
                        break;

                    case ePropID.MassEntropy:
                        break;

                    case ePropID.EnergyPrice:
                        break;

                    case ePropID.MassPrice:
                        break;

                    case ePropID.Quality:
                        break;

                    case ePropID.Percentage:
                        break;

                    case ePropID.Area:
                        break;

                    case ePropID.HeatTransferResistace:
                        break;

                    case ePropID.MolarHeatCapacity:
                        break;

                    case ePropID.MassCp:
                        break;

                    case ePropID.UA:
                        break;

                    case ePropID.Value:
                        break;

                    case ePropID.Components:
                        break;

                    case ePropID.HForm25:
                        break;

                    case ePropID.Gibbs:
                        break;

                    case ePropID.Entropyf25:
                        break;

                    case ePropID.Gibbsf25:
                        break;

                    case ePropID.U:
                        break;

                    case ePropID.A:
                        break;

                    default:
                        break;
                }
            }
        }

        private void SummaryDialog_Load(object sender, EventArgs e)
        {
        }

        private void dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv.IsCurrentCellDirty)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void TableDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            string name;
            ds.activestreams.Clear();
            foreach (DataGridViewRow item in dgv.Rows)
            {
                if (item.Cells[0].Value != null)
                {
                    name = item.Cells[0].Value.ToString();
                    var checkControl = item.Cells[1] as DataGridViewCheckBoxCell;
                    bool displayed = Convert.ToBoolean(checkControl.Value);
                    if (displayed)
                    {
                        DrawMaterialStream stream = streams.Find(x => x.Name == name);
                        ds.activestreams.Add(stream.Guid);
                    }
                }
            }

            if (cbMassFlow.Checked)
            {
                if (!ds.props.Contains(ePropID.MF))
                    ds.props.Add(ePropID.MF);
            }
            else
                ds.props.Remove(ePropID.MF);

            if (cbVolFlow.Checked)
            {
                if (!ds.props.Contains(ePropID.VF))
                    ds.props.Add(ePropID.VF);
            }
            else
                ds.props.Remove(ePropID.VF);

            if (CBMolarFlow.Checked)
            {
                if (!ds.props.Contains(ePropID.MOLEF))
                    ds.props.Add(ePropID.MOLEF);
            }
            else
                ds.props.Remove(ePropID.MOLEF);

            if (cbTemperature.Checked)
            {
                if (!ds.props.Contains(ePropID.T))
                    ds.props.Add(ePropID.T);
            }
            else
                ds.props.Remove(ePropID.T);

            if (CBPressure.Checked)
            {
                if (!ds.props.Contains(ePropID.P))
                    ds.props.Add(ePropID.P);
            }
            else
                ds.props.Remove(ePropID.P);

            if (cbHeatFlow.Checked)
            {
                if (!ds.props.Contains(ePropID.HeatFlowRate))
                    ds.props.Add(ePropID.HeatFlowRate);
            }
            else
                ds.props.Remove(ePropID.HeatFlowRate);
        }

        private void AddAll_Click(object sender, EventArgs e)
        {
            string name;
            ds.activestreams.Clear();
            foreach (DataGridViewRow item in dgv.Rows)
            {
                if (item.Cells[0].Value != null)
                {
                    name = item.Cells[0].Value.ToString();
                    var checkControl = item.Cells[1] as DataGridViewCheckBoxCell;
                    checkControl.Value = true;
                    DrawMaterialStream stream = streams.Find(x => x.Name == name);
                    ds.activestreams.Add(stream.Guid);
                }
            }
        }
    }
}