using System;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class StreamAnalysysType : UserControl
    {
        private enumShortMediumFull value = enumShortMediumFull.Short;

        public enumShortMediumFull Value { get => value; set => this.value = value; }

        public StreamAnalysysType()
        {
            InitializeComponent();
        }

        private void rbBasic_CheckedChanged(object sender, EventArgs e)
        {
            value = enumShortMediumFull.Short;
        }

        private void rbMedium_CheckedChanged(object sender, EventArgs e)
        {
            value = enumShortMediumFull.Medium;
        }

        private void rbFull_CheckedChanged(object sender, EventArgs e)
        {
            value = enumShortMediumFull.Full;
        }
    }
}