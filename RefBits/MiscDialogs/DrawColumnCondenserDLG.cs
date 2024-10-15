using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    internal partial class ColumnDrawCondenserDLG : Form
    {
        private readonly DrawColumnTraySection section;
        internal CondType condtype = CondType.Partial;
        DrawColumnCondenser cond;

        public ColumnDrawCondenserDLG(DrawColumnCondenser dccond, DrawColumnTraySection section)
        {
            InitializeComponent();
            this.section = section;
            cond = dccond;
            this.condtype = section.CondenserType;

            CondenserType.DataSource = Enum.GetNames(typeof(CondType));

            CondenserType.SelectedIndex = (int)this.condtype;
        }


        private void CondenserType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            condtype = (CondType)CondenserType.SelectedIndex;

            if (cond is not null)
                cond.CondenserType = condtype;
            if (section is not null)
                section.CondenserType = condtype;
        }
    }
}