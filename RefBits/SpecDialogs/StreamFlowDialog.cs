using ModelEngine;
using System.Windows.Forms;

namespace Units
{
    public partial class StreamFlowDialog : Form
    {
        private DrawColumn drawcolumn;
        private Specification spec;

        internal StreamFlowDialog(DrawColumn column, Specification spec)
        {
            this.drawcolumn = column;
            this.spec = spec;
            InitializeComponent();

            cbDraw.Items.AddRange(column.InternalConnectingStreams.Names);
            DrawMaterialStream dstream = column.InternalConnectingStreams[spec.drawObjectGuid];
            if (dstream is null)
                cbDraw.SelectedItem = null;
            else
                cbDraw.SelectedItem = dstream.Name;

            DrawMaterialStream ds = drawcolumn.InternalConnectingStreams[spec.drawObjectGuid];
            if (ds is null)
                cbDraw.SelectedItem = null;
            else
                cbDraw.SelectedItem = ds.Name;
        }

        private void FlowDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            string StreamName = (string)cbDraw.SelectedItem;
            DrawMaterialStream stream = drawcolumn.InternalConnectingStreams[StreamName];
            if (stream != null)
            {
                spec.drawStreamGuid = stream.Guid;
                spec.drawSectionGuid = stream.StartDrawObject.Guid;
            }
        }
    }
}