using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class ProductDrawDialog : Form
    {
        private readonly DrawColumn drawcolumn;
        private readonly Specification spec;

        internal ProductDrawDialog(DrawColumn column, Specification spec)
        {
            this.drawcolumn = column;
            this.spec = spec;
            InitializeComponent();

            DrawStreamCollection streams;

            if (spec.IsLiquid)
            {
                streams = column.IntProductStreams.GetLiquidStreams();
            }
            else
            {
                streams = column.IntProductStreams.GetVapourStreams();
            }

            cbDraw.Items.AddRange(streams.Names);

            DrawMaterialStream dstream = column.IntProductStreams[spec.drawStreamGuid];
            if (dstream is null)
                cbDraw.SelectedItem = null;
            else
                cbDraw.SelectedItem = dstream.Name;

            switch (spec.FlowType)
            {
                case enumflowtype.Molar:
                    rbMole.Checked = true;
                    break;

                case enumflowtype.Mass:
                    rbMass.Checked = true;
                    break;

                case enumflowtype.StdLiqVol:
                    rbVolume.Checked = true;
                    break;
            }
        }

        private void cbDraw_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string StreamName = (string)cbDraw.SelectedItem;
            SideStream ss = drawcolumn.Column.LiquidSideStreams[StreamName];
            if (ss != null)
            {
                spec.engineObjectguid = ss.Guid;
            }
        }

        private void FlowDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            string StreamName = (string)cbDraw.SelectedItem;
            DrawMaterialStream stream = drawcolumn.IntProductStreams[StreamName];
            if (stream != null)
            {
                spec.drawStreamGuid = stream.Guid;
                spec.drawSectionGuid = stream.StartDrawObjectGuid;
            }

            //spec.FlowBasis = (enumMassMolarOrVol)cbSpec.SelectedItem;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            spec.FlowType = enumflowtype.Molar;
            spec.ChangeUnits();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            spec.FlowType = enumflowtype.Mass;
            spec.ChangeUnits();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            spec.FlowType = enumflowtype.StdLiqVol;
            spec.ChangeUnits();
        }
    }
}