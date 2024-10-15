using ModelEngine;
using ModelEngine;
using System.Windows.Forms;

namespace Units
{
    public partial class PlugFlowDialog : BaseDialog, IUpdateableDialog
    {
        private readonly Components cc;

        public PlugFlowDialog()
        {
            InitializeComponent();
        }

        //public  event PumpDialogValueChangedEventHandler PumpDLGValueChangedEvent;
        //public  delegate void  PumpDialogValueChangedEventHandler(object sender, EventArgs e);

        internal PlugFlowDialog(PlugFlowRx plugflow)
        {
            cc = plugflow.PortIn.cc;
            InitializeComponent();
        }

        public static void Updatepipeinnerdiamter(PipeFittings pf, string NomPipeSize, string Schedule)
        {
            var temp = PipeFittings.GetPipeInsideDiamter(NomPipeSize, Schedule);
            if (temp != null)
                pf.InnerDiameter.BaseValue = PipeFittings.GetPipeInsideDiamter(NomPipeSize, Schedule).BaseValue;
        }

        private void PlusFlowDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Rxns_Click(object sender, System.EventArgs e)
        {
            DefineRxns defineRxns = new(cc);
            defineRxns.ShowDialog();
        }
    }
}