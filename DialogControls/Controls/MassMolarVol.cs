using Extensions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class MassMolarVol : UserControl
    {
        private enumMassMolarOrVol value = enumMassMolarOrVol.Molar;

        public MassMolarVol()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        public override string Text
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

        public enumMassMolarOrVol Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;

                switch (value)
                {
                    case enumMassMolarOrVol.Mass:
                        rbMass.Checked = true;
                        break;

                    case enumMassMolarOrVol.Molar:
                        rbMolar.Checked = true;
                        break;

                    case enumMassMolarOrVol.Vol:
                        rbVol.Checked = true;
                        break;

                    default:
                        break;
                }
            }
        }

        private void rbMass_CheckedChanged(object sender, System.EventArgs e)
        {
            value = enumMassMolarOrVol.Mass;
        }

        private void rbMolar_CheckedChanged(object sender, System.EventArgs e)
        {
            value = enumMassMolarOrVol.Molar;
        }

        private void rbVol_CheckedChanged(object sender, System.EventArgs e)
        {
            value = enumMassMolarOrVol.Vol;
        }

        public double[] Composition(double[] Mol, double[] SG, double[] MW)
        {
            switch (value)
            {
                case enumMassMolarOrVol.Mass:
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                    return Mol.Mult(MW).Normalise();

                case enumMassMolarOrVol.Molar:
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                    return Mol;

                case enumMassMolarOrVol.Vol:
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                    return Mol.Divide(SG).Normalise();
            }
            return null;
        }

        public new event MouseEventHandler MouseClick;

        public event EventHandler SelectionChanged;

        private void MassMolarVol_MouseClick(object sender, MouseEventArgs e)
        {
            RaiseMouseEvent(MouseClick, e);
        }
    }
}