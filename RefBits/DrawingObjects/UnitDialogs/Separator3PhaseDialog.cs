namespace Units
{
    public partial class Separator3PhaseDialog : BaseDialog, IUpdateableDialog
    {
        private Draw3PhaseSeparator obj;

        public Separator3PhaseDialog()
        {
            InitializeComponent();
        }

        //public  event PumpDialogValueChangedEventHandler PumpDLGValueChangedEvent;
        //public  delegate void  PumpDialogValueChangedEventHandler(object sender, EventArgs e);

        internal Separator3PhaseDialog(Draw3PhaseSeparator obj)
        {
            InitializeComponent();

            this.obj = obj;

            Worksheet.UpdateValues();
        }

        private void Worksheet_ValueChangedEvent(object sender, System.EventArgs e)
        {
            base.RaiseValueChangedEvent(e);
        }
    }
}