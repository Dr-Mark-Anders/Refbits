using ModelEngine;
using FormControls;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class DefineRxns : Form
    {
        private Components cc;

        public DefineRxns(Components cc)
        {
            InitializeComponent();
            this.cc = cc;
        }

        private void Feeds_Click(object sender, EventArgs e)
        {
            ComponenetSelection cs = new(cc);
            cs.ShowDialog();
            Feeds.UpdateGrid(cc);
        }

        public void AddTolist(RXStoichiometryGrid comps)
        {
            for (int i = 0; i < cc.Count; i++)
            {
                comps.Add(cc[i]);
            }
        }

        private void Products_Click(object sender, EventArgs e)
        {
            ComponenetSelection cs = new(cc);
            cs.ShowDialog();
            Products.UpdateGrid(cc);
        }

        private void Feeds_Load(object sender, EventArgs e)
        {
        }
    }
}