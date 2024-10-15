using ModelEngine;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units
{
    public partial class GibbsDialog : BaseDialog, IUpdateableDialog
    {
        private DrawGibbs obj;

        public GibbsDialog()
        {
            InitializeComponent();
        }

        //public  event PumpDialogValueChangedEventHandler PumpDLGValueChangedEvent;
        //public  delegate void  PumpDialogValueChangedEventHandler(object sender, EventArgs e);

        internal GibbsDialog(DrawGibbs obj)
        {
            InitializeComponent();

            this.obj = obj;

            List<StreamMaterial> streams = obj.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, obj.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();
            int rowNo;
            for (int i = 0; i < obj.gibbs.PortIn.cc.Count; i++)
            {
                rowNo = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowNo];
                row.HeaderCell.Value = obj.gibbs.PortIn.cc[i].Name;
                row.Cells[1].Value = obj.gibbs.PortIn.cc[i].MoleFraction;
            }
            Worksheet.ValueChanged += Worksheet_ValueChanged;
        }

        private void Worksheet_ValueChanged(object sender, System.EventArgs e)
        {
            RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            Worksheet.UpdateValues();
        }
    }
}