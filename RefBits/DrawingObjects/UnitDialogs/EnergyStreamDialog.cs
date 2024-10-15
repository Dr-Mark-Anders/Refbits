using System.Windows.Forms;

namespace Units.DrawingObjects.UnitDialogs
{
    public partial class EnergyStreamDialog : Form
    {
        internal EnergyStreamDialog(DrawEnergyStream stream)
        {
            InitializeComponent();

            uomTextBox1.Bind(stream.Stream.Port.Value);
        }
    }
}