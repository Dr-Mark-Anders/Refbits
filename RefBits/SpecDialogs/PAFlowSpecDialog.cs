using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class PAFlowSpecDialog : Form
    {
        private DrawColumn drawcolumn;
        public int SectionNo = 0;
        public int StageNo = 0;
        private Specification spec;

        internal PAFlowSpecDialog(DrawColumn column, Specification spec)
        {
            this.drawcolumn = column;
            this.spec = spec;
            InitializeComponent();

            cbSpec.Items.AddRange(Enum.GetNames(typeof(eSpecType)));
            cbSpec.SelectedItem = spec.graphicSpecType.ToString();

            foreach (PumpAround ss in drawcolumn.Column.PumpArounds)
                cbName.Items.Add(ss.Name);

            DrawPA pa = drawcolumn.PAs[spec.drawObjectGuid];
            if (pa != null)
                cbName.SelectedItem = pa.Name;

            switch (spec.FlowType)
            {
                case enumflowtype.Molar:
                    rbMole.Checked = true;
                    break;

                case enumflowtype.Mass:
                    rbMass.Checked = true;
                    break;

                case enumflowtype.StdLiqVol:
                    rbVol.Checked = true;
                    break;
            }
        }

        private void cbName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string PAName = (string)cbName.SelectedItem;
            DrawPA pa = drawcolumn.PAs[PAName];
            if (pa != null)
            {
                spec.drawObjectGuid = pa.Guid;
                spec.PAguid = pa.PumpAround.Guid;
            }
        }

        private void cbSpec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSpec.SelectedItem != null)
                if (Enum.TryParse(cbSpec.SelectedItem.ToString(), out eSpecType espec))
                {
                    spec.graphicSpecType = espec;
                    spec.engineSpecType = espec;
                }
        }

        private void PASpecDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
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