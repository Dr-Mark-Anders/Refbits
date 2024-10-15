using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class CondenserTypeControl : UserControl
    {
        private DrawColumn drawColumn;
        private SpecificationSheet spsh;

        public CondenserTypeControl()
        {
            InitializeComponent();
        }

        internal void InitWithColumnData(DrawColumn drawColumn, SpecificationSheet spsh)
        {
            this.drawColumn = drawColumn;
            this.spsh = spsh;

            cbTotalReflux.Checked = drawColumn.Column.Totalreflux;

            switch (drawColumn.Column.MainTraySection.CondenserType)
            {
                case CondType.Partial:
                    rbPartial.Checked = true;
                    break;

                case CondType.Subcooled:
                    rbSubCooled.Checked = true;
                    txtSubCooling.Text = drawColumn.Column.SubcoolDT.ToString();
                    break;

                case CondType.TotalReflux:
                    rbTotal.Checked = true;
                    break;
            }
        }

        private void rbPartial_CheckedChanged(object sender, EventArgs e)
        {
            if (drawColumn is null)
                return;
            drawColumn.Column.MainTraySection.CondenserType = CondType.Partial;
            drawColumn.Column.ValidateDesign();
            spsh.CheckDegOfFreedom();
        }

        private void rbTotal_CheckedChanged(object sender, EventArgs e)
        {
            if (drawColumn is null)
                return;
            drawColumn.Column.MainTraySection.CondenserType = CondType.TotalReflux;
            drawColumn.Column.ValidateDesign();
            spsh.CheckDegOfFreedom();
        }

        private void rbSubCooled_CheckedChanged(object sender, EventArgs e)
        {
            if (drawColumn is null)
                return;
            drawColumn.Column.MainTraySection.CondenserType = CondType.Subcooled;
            drawColumn.Column.ValidateDesign();
            spsh.CheckDegOfFreedom();
        }

        private void cbTotalReflux_CheckedChanged(object sender, EventArgs e)
        {
            if (drawColumn is null)
                return;
            drawColumn.Column.Totalreflux = cbTotalReflux.Checked;
            drawColumn.Column.ValidateDesign();
            spsh.CheckDegOfFreedom();
        }

        private void txtSubCooling_Validated(object sender, EventArgs e)
        {
            if (drawColumn is null)
                return;
            double res;
            if (double.TryParse(txtSubCooling.Text, out res))
                drawColumn.Column.SubcoolDT = res;
            else
                txtSubCooling.Text = "0";
        }
    }
}