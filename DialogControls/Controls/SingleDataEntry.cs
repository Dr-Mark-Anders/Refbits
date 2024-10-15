using ModelEngine;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class SingleDataEntry : UserControl
    {
        public SingleDataEntry()
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
        public UOMProperty uom
        {
            get
            {
                return uom1.UOMprop;
            }
            set
            {
                uom1.UOMprop = value;
            }
        }

        public new event MouseEventHandler MouseClick;

        private void MassMolarVol_MouseClick(object sender, MouseEventArgs e)
        {
            RaiseMouseEvent(MouseClick, e);
        }

        private void SingleProperty_Resize(object sender, EventArgs e)
        {
            uom1.Left = gb.Left + 10;
            uom1.Top = gb.Top + 20;
            uom1.Height = gb.Height - 30;
            uom1.Width = gb.Width - 20;
        }

        private void gb_Enter(object sender, EventArgs e)
        {
        }
    }
}