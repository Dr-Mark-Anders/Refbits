using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class DropDownDataEntry : UserControl
    {
        public DropDownDataEntry()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        public string TitleText
        {
            get
            {
                return gb.Text;
            }
            set
            {
                gb.Text = value;
            }
        }

        [Browsable(true)]
        public object SelectedItem
        {
            get
            {
                return cb1.SelectedItem;
            }
            set
            {
                cb1.SelectedItem = value;
            }
        }

        public new event MouseEventHandler MouseClick;

        private void MassMolarVol_MouseClick(object sender, MouseEventArgs e)
        {
            RaiseMouseEvent(MouseClick, e);
        }

        private void SingleProperty_Resize(object sender, EventArgs e)
        {
            cb1.Left = gb.Left + 10;
            cb1.Top = gb.Top + 20;
            cb1.Height = gb.Height - 30;
            cb1.Width = gb.Width - 20;
        }
    }
}