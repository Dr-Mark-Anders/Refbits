using ModelEngine;
using System;
using System.Collections.Generic;

namespace Units
{
    public partial class HeaterDialog : BaseDialog, IUpdateableDialog
    {
        private readonly Heater h;

        public HeaterDialog()
        {
            InitializeComponent();
        }

        //public  event PumpDialogValueChangedEventHandler PumpDLGValueChangedEvent;
        //public  delegate void  PumpDialogValueChangedEventHandler(object sender, EventArgs e);

        internal HeaterDialog(DrawHeater dheater)
        {
            InitializeComponent();

            this.h = dheater.Heater;

            Parameters.Add(h.DP.Value, "Pressure  Drop");
            Parameters.Add(h.DT.Value, "Delta Temperature ");

            Results.Add(h.DP.Value, "Pressure  Drop");
            Results.Add(h.Q.Value, "Duty");

            List<StreamMaterial> streams = dheater.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, dheater.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();

            Worksheet.ValueChanged += Worksheet_ValueChangedEvent;
            Parameters.ValueChanged += Worksheet_ValueChangedEvent;
        }

        private void Worksheet_ValueChangedEvent(object sender, EventArgs e)
        {
            h.IsDirty = true;
            base.RaiseValueChangedEvent(e);
        }

        public class HeaterEventArgs : EventArgs
        {
        }

        public override void UpdateValues()
        {
            Worksheet.ValueChanged -= Worksheet_ValueChangedEvent;
            Parameters.ValueChanged -= Worksheet_ValueChangedEvent;

            Parameters.UpdateValues();
            Worksheet.UpdateValues();
            Results.UpdateValues();

            Worksheet.ValueChanged += Worksheet_ValueChangedEvent;
            Parameters.ValueChanged += Worksheet_ValueChangedEvent;
        }
    }
}