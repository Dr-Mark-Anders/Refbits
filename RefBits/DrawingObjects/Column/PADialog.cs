using ModelEngine;
using System.Collections.Generic;

namespace Units
{
    public partial class PADialog : BaseDialog, IUpdateableDialog
    {
        private readonly PumpAround pa;

        public PADialog()
        {
            InitializeComponent();
        }

        internal PADialog(DrawPA drawpa)
        {
            InitializeComponent();

            this.pa = drawpa.PumpAround;
            Worksheet.ValueChanged += Worksheet_ValueChanged;

            List<StreamMaterial> streams = drawpa.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, drawpa.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();
        }

        private void Worksheet_ValueChanged(object sender, System.EventArgs e)
        {
            base.RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            Worksheet.UpdateValues();
        }
    }
}