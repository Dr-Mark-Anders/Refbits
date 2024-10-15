using ModelEngine;
using System.Collections.Generic;

namespace Units
{
    public partial class ValveDialog : BaseDialog, IUpdateableDialog
    {
        private readonly Valve valve;

        public ValveDialog()
        {
            InitializeComponent();
        }

        internal ValveDialog(DrawValve Dvalve)
        {
            InitializeComponent();

            Parameters.ValueChanged += Parameters_ValueChanged;

            this.valve = Dvalve.valve;

            Parameters.Add(Dvalve.valve.DP.Value, "Pressure Drop");

            Results.Add(Dvalve.valve.DP.Value, "Pressure Drop");
            Results.Add(Dvalve.valve.DT.Value, "Temperature Change");

            List<StreamMaterial> streams = Dvalve.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, Dvalve.displayunits);

            Parameters.ValueChanged += Parameters_ValueChanged;
            Worksheet.ValueChanged += Parameters_ValueChanged;
        }

        private void Parameters_ValueChanged(object sender, System.EventArgs e)
        {
            valve.IsDirty = true;
            base.RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            Parameters.UpdateValues();
            Worksheet.UpdateValues();
            Results.UpdateValues();
        }

        private void Parameters_Load(object sender, System.EventArgs e)
        {
        }
    }
}