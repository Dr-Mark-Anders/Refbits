using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    internal partial class ColumnDrawreboilerDLG : Form
    {
        private readonly DrawColumnTraySection section;
        ReboilerType reboiltype = ReboilerType.Kettle;
        DrawColumnReboiler dcr;

        public ColumnDrawreboilerDLG(DrawColumnReboiler reboiler, DrawColumnTraySection section)
        {
            InitializeComponent();
            this.section = section;
            dcr = reboiler;

            //this.reboiltype = section.Reboilertype;

            ReboilerTypeCombo.DataSource = Enum.GetNames(typeof(ReboilerType));

            ReboilerTypeCombo.SelectedIndex = (int)this.reboiltype;
        }


        private void ReboilerTypeCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            reboiltype = (ReboilerType)ReboilerTypeCombo.SelectedIndex;

            if (dcr is not null)
                dcr.reboilertype = reboiltype;
            if (section is not null)
                section.Reboilertype = reboiltype;
        }
    }
}