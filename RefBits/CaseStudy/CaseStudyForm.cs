using System.Windows.Forms;
using UOMGrid;

namespace Units.CaseStudy
{
    public partial class CaseStudyForm : Form
    {
        private int nocases = 0;
        private ModelEngine.CaseStudy caseStudy = new();

        public CaseStudyForm(ModelEngine.CaseStudy caseStudy = null)
        {
            if (caseStudy != null)
                this.caseStudy = caseStudy;

            InitializeComponent();
            gridInputs.SetupGrid();
            gridInputs.mode = Mode.CaseStudy;
            gridInputs.ColumnMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridInputs.ColOneMinimumWidth = 100;

            gridOutputs.SetupGrid();
            gridOutputs.mode = Mode.CaseStudy;
            gridOutputs.ColumnMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridOutputs.ColOneMinimumWidth = 100;

            gridResults.SetupGrid();
            gridResults.mode = Mode.CaseStudyResults;
            gridResults.ColumnMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridResults.ColOneMinimumWidth = 100;

            gridInputs.AllowUserToAddRows = true;
            gridOutputs.AllowUserToAddRows = true;
        }

        private void MyTextBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (int.TryParse(myTextBox1.Text, out int no))
            {
                nocases = no;
            }
            else
            {
                myTextBox1.Text = 0.ToString();
                e.Cancel = true;
                MessageBox.Show("PleaseEnteranint eger");
            }
        }

        private void myTextBox1_Validated(object sender, System.EventArgs e)
        {
            gridResults.Cols = nocases + 2;
            gridInputs.Cols = nocases + 2;
            gridResults.SetupCaseStudyGrid(nocases);
            gridInputs.SetupCaseStudyGrid(nocases);
        }

        private void Setup()
        {
            for (int i = 0; i < gridInputs.RowCount; i++)
            {
                if (gridInputs[2, i].Value is UOMGridCell cell)
                    caseStudy.AddOutput(cell.uom);
            }
        }

        private void Run_Click(object sender, System.EventArgs e)
        {
            Setup();
            caseStudy.Clear();
            ModelEngine.CaseSet set = new();

            for (int caseno = 0; caseno < nocases; caseno++)
            {
                for (int i = 0; i < gridInputs.RowCount; i++)
                {
                    if (gridInputs[3, i].Value is double d)
                        set.AddValue(gridInputs[2, i].uom, d);
                }
            }

            //caseStudy.RunCases();
        }
    }
}