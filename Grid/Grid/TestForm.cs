namespace UOMGrid
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            grid1.Rows = 10;
            grid1.Cols = 10;
            grid1.SetupGrid();
        }
    }
}