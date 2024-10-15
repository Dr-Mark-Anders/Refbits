using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class PASpecDialog : Form
    {
        private DrawColumn drawcolumn;
        public int SectionNo = 0;
        public int StageNo = 0;
        private Specification spec;

        internal PASpecDialog(DrawColumn column, Specification spec)
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
    }
}