using System;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class DistillationBasis : UserControl
    {
        private enumDistType value = enumDistType.D86;

        public enumDistType Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            switch (this.value)
            {
                case enumDistType.D86:
                    D86.Checked = true;
                    break;

                case enumDistType.D1160:
                    D1160.Checked = true;
                    break;

                case enumDistType.D2887:
                    D2887.Checked = true;
                    break;

                case enumDistType.TBP_WT:
                    TBPWT.Checked = true;
                    break;

                case enumDistType.TBP_VOL:
                    TBPVOL.Checked = true;
                    break;

                case enumDistType.NON:
                    break;

                default:
                    break;
            }
        }

        public DistillationBasis()
        {
            InitializeComponent();
        }

        public new event MouseEventHandler MouseClick;

        private void MassMolarVol_MouseClick(object sender, MouseEventArgs e)
        {
            RaiseMouseEvent(MouseClick, e);
        }

        private void TBP_CheckedChanged(object sender, EventArgs e)
        {
            value = enumDistType.TBP_VOL;
        }

        private void D86_CheckedChanged(object sender, EventArgs e)
        {
            value = enumDistType.D86;
        }

        private void D1160_CheckedChanged(object sender, EventArgs e)
        {
            value = enumDistType.D1160;
        }

        private void D2887_CheckedChanged(object sender, EventArgs e)
        {
            value = enumDistType.D2887;
        }

        private void TBPWT_CheckedChanged(object sender, EventArgs e)
        {
            value = enumDistType.TBP_WT;
        }
    }
}