using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class ReboilerTypeControl : UserControl
    {
        private DrawColumn drawColumn;
        private SpecificationSheet spsh;

        public ReboilerTypeControl()
        {
            InitializeComponent();
        }

        internal void InitWithColumnData(DrawColumn drawColumn, SpecificationSheet spsh)
        {
            this.drawColumn = drawColumn;
            this.spsh = spsh;

            switch (drawColumn.Column.MainTraySection.ReboilerType)
            {
                case ReboilerType.None:
                    rbKettle.Checked = true;
                    break;
                case ReboilerType.HeatEx:
                    rbKettle.Checked = true;
                    break;
                case ReboilerType.Kettle:
                    rbKettle.Checked = true;
                    break;
                case ReboilerType.Thermosiphon:
                    rbKettle.Checked = true;
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
            drawColumn.Column.ValidateDesign();
            spsh.CheckDegOfFreedom();
        }

        private void txtSubCooling_Validated(object sender, EventArgs e)
        {
            if (drawColumn is null)
                return;
        }
    }
}