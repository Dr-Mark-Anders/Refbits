using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class FlowDialog : Form
    {
        private DrawColumn drawcolumn;
        private Specification spec;
        private TraySection section;

        internal FlowDialog(DrawColumn column, Specification spec)
        {
            this.drawcolumn = column;
            this.spec = spec;
            InitializeComponent();

            cbSpec.Items.AddRange(Enum.GetNames(typeof(eSpecType)));
            cbSpec.SelectedItem = spec.graphicSpecType.ToString();

            cbFlowType.Items.AddRange(Enum.GetNames(typeof(enumMassMolarOrVol)));
            cbSpec.SelectedItem = spec.FlowBasis.ToString();

            foreach (SideStream ss in drawcolumn.Column.LiquidSideStreams)
                cbDraw.Items.Add(ss.Name);

            DrawMaterialStream ds = drawcolumn.IntProductStreams[spec.drawObjectGuid];
            cbDraw.SelectedItem = ds.Name;
        }

        private void cbDraw_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string StreamName = (string)cbDraw.SelectedItem;
            SideStream ss = drawcolumn.Column.LiquidSideStreams[StreamName];
            spec.engineObjectguid = ss.Guid;
            section = ss.EngineDrawSection;
        }

        private void cbSpec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Enum.TryParse(cbSpec.SelectedItem.ToString(), out eSpecType espec))
                spec.graphicSpecType = espec;
        }

        private void FlowDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            string StreamName = (string)cbDraw.SelectedItem;
            SideStream ss = drawcolumn.Column.LiquidSideStreams[StreamName];
            if (ss != null)
            {
                spec.engineObjectguid = ss.Guid;
                spec.engineStageGuid = ss.EngineDrawTray.Guid;
            }
            if (section != null)
                spec.engineSectionGuid = section.Guid;

            spec.FlowBasis = (enumMassMolarOrVol)cbSpec.SelectedItem;
        }
    }
}