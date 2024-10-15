using ModelEngine;
using System.Collections.Generic;

namespace Units
{
    public partial class MixerDialog : BaseDialog, IUpdateableDialog
    {
        private DrawMixer drawmixer;
        private Mixer mixer;

        internal MixerDialog()
        {
            InitializeComponent();
        }

        internal MixerDialog(DrawMixer drawmixer)
        {
            this.drawmixer = drawmixer;
            InitializeComponent();

            this.mixer = (Mixer)drawmixer.AttachedModel;
            List<StreamMaterial> streams = drawmixer.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, drawmixer.DrawArea.UOMDisplayList);
            Worksheet.ValueChanged += Worksheet_ValueChanged;
        }

        private void Worksheet_ValueChanged(object sender, System.EventArgs e)
        {
            base.RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            //Parameters.UpdateValues();
            Worksheet.UpdateValues();
        }
    }
}