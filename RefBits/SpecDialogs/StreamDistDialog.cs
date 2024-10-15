using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class StreamDistDialog : Form
    {
        private DrawColumn drawcolumn;
        private Specification spec;

        internal StreamDistDialog(DrawColumn column, Specification spec)
        {
            this.drawcolumn = column;
            this.spec = spec;
            InitializeComponent();

            cbDraw.Items.AddRange(column.IntProductStreams.Names);
            DrawMaterialStream dstream = column.IntProductStreams[spec.drawStreamGuid];
            if (dstream is null)
                cbDraw.SelectedItem = null;
            else
                cbDraw.SelectedItem = dstream.Name;

            cbDistType.DataSource = Enum.GetValues(typeof(enumDistType));
            cbDistType.SelectedItem = spec.distType;

            cbDistPoint.DataSource = Enum.GetValues(typeof(enumDistPoints));
            cbDistPoint.SelectedItem = spec.distpoint;
        }

        private void FlowDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            string StreamName = (string)cbDraw.SelectedItem;
            DrawMaterialStream stream = drawcolumn.IntProductStreams[StreamName];
            if (stream != null)
            {
                spec.drawStreamGuid = stream.Guid;
                spec.drawSectionGuid = stream.StartDrawObject.Guid;
                spec.distType = (enumDistType)cbDistType.SelectedItem;
                spec.distpoint = (enumDistPoints)cbDistPoint.SelectedItem;
            }
            spec.UOM = UOMUtility.GetUOM(ePropID.T);
        }
    }
}