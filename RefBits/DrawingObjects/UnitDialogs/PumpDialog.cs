using ModelEngine;
using System.Collections.Generic;

namespace Units
{
    public partial class PumpDialog : BaseDialog, IUpdateableDialog
    {
        private readonly Pump pump;

        public PumpDialog()
        {
            InitializeComponent();
        }

        internal PumpDialog(DrawPump dpump)
        {
            InitializeComponent();

            pump = dpump.Pump;

            uomPumpEfficiency.Bind(dpump.Pump.Eff.Value);
            List<StreamMaterial> streams = dpump.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, dpump.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();

            uomPin.UOMprop = dpump.Pump.PortIn.P_;
            uomPout.UOMprop = dpump.Pump.PortOut.P_;

            Parameters.Add(dpump.Pump.PortIn.P_, "Inlet P");
            Parameters.Add(dpump.Pump.PortOut.P_, "Outlet P");
            Parameters.Add(dpump.Pump.DP.Value, "Delta P");
            Parameters.Add(dpump.Pump.Eff.Value, "Efficiency");
            Parameters.Add(dpump.Pump.DT.Value, "Delta T");
            Parameters.Add(dpump.Pump.DifferentialHead.Value, "Diff. Head");
            Parameters.Add(dpump.Pump.TotalHydraulicpower.Value, "Hydraulic Power");

            Parameters.UpdateValues();

            Parameters.ValueChanged += Worksheet_ValueChanged;
            Worksheet.ValueChanged += Worksheet_ValueChanged;
        }

        private void Worksheet_ValueChanged(object sender, System.EventArgs e)
        {
            pump.IsDirty = true;
            base.RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            Parameters.UpdateValues();
            Worksheet.UpdateValues();
        }
    }
}