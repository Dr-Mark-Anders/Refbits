using ModelEngine;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units
{
    public partial class PipeDialog : BaseDialog, IUpdateableDialog
    {
        private DrawPipe dpipe;
        private bool isLoaded = false;

        public PipeDialog()
        {
            InitializeComponent();
        }

        //public  event PumpDialogValueChangedEventHandler PumpDLGValueChangedEvent;
        //public  delegate void  PumpDialogValueChangedEventHandler(object sender, EventArgs e);

        internal PipeDialog(DrawPipe dpipe)
        {
            this.dpipe = dpipe;
            InitializeComponent();

            uomPumpEfficiency.Bind(dpipe.pipe.Eff.Value);
            List<StreamMaterial> streams = dpipe.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, dpipe.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();
            FeedFittings.LoadData(dpipe.pipe.pipefittingsin.FittingTypes);
            DischargeFittings.LoadData(dpipe.pipe.pipefittingsout.FittingTypes);

            NomPipeSizeIn.DataSource = new BindingSource(dpipe.pipe.pipefittingsin.NominalPipeSize, null);
            NomPipeSizeIn.Text = dpipe.pipe.pipefittingsin.NomPipeSize;

            ScheduleIn.DataSource = new BindingSource(dpipe.pipe.pipefittingsin.ScheduleList, null);
            ScheduleIn.Text = dpipe.pipe.pipefittingsin.schedule;

            NomPipeSizeOut.DataSource = new BindingSource(dpipe.pipe.pipefittingsout.NominalPipeSize, null);
            NomPipeSizeOut.Text = dpipe.pipe.pipefittingsout.NomPipeSize;

            ScheduleOut.DataSource = new BindingSource(dpipe.pipe.pipefittingsout.ScheduleList, null);
            ScheduleOut.Text = dpipe.pipe.pipefittingsout.schedule;

            uomLengthDischarge.UOMprop = dpipe.pipe.pipefittingsout.PipeLength;
            uomLengthCharge.UOMprop = dpipe.pipe.pipefittingsin.PipeLength;

            uomHeightDisch.UOMprop = dpipe.pipe.pipefittingsout.StaticHead;
            uomHeightCharge.UOMprop = dpipe.pipe.pipefittingsin.StaticHead;

            uomPin.UOMprop = dpipe.pipe.PortIn.P_;
            uomPout.UOMprop = dpipe.pipe.PortOut.P_;

            uomPipeDiameterCharge.Bind(dpipe.pipe.pipefittingsin.InnerDiameter);
            uomPipeDiamDischarge.Bind(dpipe.pipe.pipefittingsout.InnerDiameter);

            isLoaded = true;
        }

        private void NomPipeIn_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (isLoaded)
            {
                dpipe.pipe.pipefittingsin.NomPipeSize = NomPipeSizeIn.Text;
                updatepipeinnerdiamter(dpipe.pipe.pipefittingsin, NomPipeSizeIn.Text, ScheduleIn.Text);
            }
        }

        private void NomPipeOut_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (isLoaded)
            {
                dpipe.pipe.pipefittingsout.NomPipeSize = NomPipeSizeOut.Text;
                updatepipeinnerdiamter(dpipe.pipe.pipefittingsout, NomPipeSizeOut.Text, ScheduleOut.Text);
            }
        }

        private void ScheduleOut_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (isLoaded)
            {
                dpipe.pipe.pipefittingsout.schedule = ScheduleOut.Text;
                updatepipeinnerdiamter(dpipe.pipe.pipefittingsout, NomPipeSizeOut.Text, ScheduleOut.Text);
            }
        }

        private void ScheduleIn_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (isLoaded)
            {
                dpipe.pipe.pipefittingsin.schedule = ScheduleIn.Text;
                updatepipeinnerdiamter(dpipe.pipe.pipefittingsin, NomPipeSizeIn.Text, ScheduleIn.Text);
            }
        }

        public void updatepipeinnerdiamter(PipeFittings pf, string NomPipeSize, string Schedule)
        {
            var temp = PipeFittings.GetPipeInsideDiamter(NomPipeSize, Schedule);
            if (temp != null)
                pf.InnerDiameter.BaseValue = PipeFittings.GetPipeInsideDiamter(NomPipeSize, Schedule).BaseValue;
        }

        private void PumpDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void uomPipeDiamDischarge_Load(object sender, System.EventArgs e)
        {
        }
    }
}