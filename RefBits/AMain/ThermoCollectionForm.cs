using System.Windows.Forms;

namespace Units
{
    public partial class ThermoCollectionForm : Form
    {
        public ThermoCollectionForm(ThermoDynamicOptions options)
        {
            InitializeComponent();
            enumerationGrid1.options = options;
            enumerationGrid1.SetUp();
            //enumerationGrid1.Add(options.KMethod, "K Method");
        }
    }
}