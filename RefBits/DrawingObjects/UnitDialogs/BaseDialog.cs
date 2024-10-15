using System;
using System.Windows.Forms;

namespace Units
{
    public partial class BaseDialog : Form//,IUpdateableDialog
    {
        public BaseDialog()
        {
            InitializeComponent();
        }

        public event DialogValueChangedEventHandler DLGValueChangedEvent;

        public delegate void DialogValueChangedEventHandler(object sender, EventArgs e);

        internal virtual void RaiseValueChangedEvent(EventArgs e)
        {
            DLGValueChangedEvent?.Invoke(this, e);
        }

        public virtual void UpdateValues()
        {
            this.Invalidate();
            this.Refresh();
        }
    }
}