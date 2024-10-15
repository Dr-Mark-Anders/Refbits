using ModelEngine;
using System.Collections.Generic;

namespace Units
{
    public partial class CoolerDialog : BaseDialog, IUpdateableDialog
    {
        private readonly Cooler cooler;

        public CoolerDialog()
        {
            InitializeComponent();
        }

        internal CoolerDialog(DrawCooler DCooler)
        {
            InitializeComponent();

            cooler = DCooler.Cooler;

            Parameters.Add(DCooler.Cooler.DP.Value, "Pressure Drop");
            Parameters.Add(DCooler.Cooler.DT.Value, "Temperature Change");

            Results.Add(DCooler.Cooler.DP.Value, "Pressure Drop");
            Results.Add(DCooler.Cooler.Q.Value, "Duty");

            List<StreamMaterial> streams = DCooler.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, DCooler.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();

            Worksheet.ValueChanged += Worksheet_ValueChangedEvent;
            Parameters.ValueChanged += Worksheet_ValueChangedEvent;
        }

        private void Worksheet_ValueChangedEvent(object sender, System.EventArgs e)
        {
            cooler.IsDirty = true;
            base.RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            Parameters.UpdateValues();
            Worksheet.UpdateValues();
            Results.UpdateValues();
        }
    }
}