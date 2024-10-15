using ModelEngine;
using System.Collections.Generic;

namespace Units
{
    public partial class SeparatorDialog : BaseDialog, IUpdateableDialog
    {
        private DrawSeparator obj;

        public SeparatorDialog()
        {
            InitializeComponent();
        }

        internal SeparatorDialog(DrawSeparator obj)
        {
            InitializeComponent();

            this.obj = obj;

            List<StreamMaterial> streams = obj.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, obj.DrawArea.UOMDisplayList);

            Worksheet.ValueChanged += Worksheet_ValueChanged;
        }

        public override void UpdateValues()
        {
            Worksheet.UpdateValues();
            this.Invalidate();
            //this.Refresh();
        }

        private void Worksheet_ValueChanged(object sender, System.EventArgs e)
        {
            base.RaiseValueChangedEvent(e);
        }
    }
}