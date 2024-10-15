using ModelEngine;
using System;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class RefShortFeed : Form
    {
        private RefomerShortCompList comps;

        public RefShortFeed(RefomerShortCompList data)
        {
            comps = data;
            InitializeComponent();
        }

        private void FullGC_FormClosing(object sender, FormClosingEventArgs e)
        {
            comps.basis = ponAdata1.basis;
            comps.SetFractions(ponAdata1.GetFractions());
        }

        private void FullGC_Load(object sender, EventArgs e)
        {
            ponAdata1.SetFractions(comps);
            ponAdata1.Basis = comps.basis;
        }

        private void btnViewComponents_Click(object sender, EventArgs e)
        {
            ViewCompProps vcp = new ViewCompProps(comps);
            vcp.ShowDialog();
        }
    }
}