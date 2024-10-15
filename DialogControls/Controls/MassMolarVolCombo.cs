using Extensions;
using System;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class MassMolarVolCombo : UserControl
    {
        public enumMassMolarOrVol flowtype = enumMassMolarOrVol.Molar;

        public MassMolarVolCombo()
        {
            InitializeComponent();
            comboBox1.Items.Add("Mass");
            comboBox1.Items.Add("Molar");
            comboBox1.Items.Add("Volume");

            switch (flowtype)
            {
                case enumMassMolarOrVol.Mass:
                    comboBox1.SelectedIndex = 0;
                    break;

                case enumMassMolarOrVol.Molar:
                    comboBox1.SelectedIndex = 1;
                    break;

                case enumMassMolarOrVol.Vol:
                    comboBox1.SelectedIndex = 2;
                    break;
            }
        }

        public double[] Composition(double[] Mol, double[] SG, double[] MW)
        {
            switch (flowtype)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    flowtype = enumMassMolarOrVol.Mass;
                    break;

                case 1:
                    flowtype = enumMassMolarOrVol.Molar;
                    break;

                case 2:
                    flowtype = enumMassMolarOrVol.Vol;
                    break;
            }
        }
    }
}