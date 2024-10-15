using ModelEngine;
using System.Windows.Forms;
using UOMGrid;

namespace Units.CaseStudy
{
    public partial class CaseStudyForm2 : Form
    {
        private int nocases = 0;
        private ModelEngine.CaseStudy caseStudy = new();
        private int ResultRows;
        private int Cols;
        private int InputRows;
        private int OutputRows;

        public CaseStudyForm2(ModelEngine.CaseStudy caseStudy = null)
        {
            if (caseStudy != null)
                this.caseStudy = caseStudy;

            InitializeComponent();

            if (caseStudy != null && !caseStudy.IsEmpty())
            {
                OutputRows = caseStudy.CaseOutput.Count;
                InputRows = caseStudy.CaseSets[0].Count;
                ResultRows = caseStudy.CaseResults[0].Count;
                Cols = caseStudy.CaseResults.Count;
            }

            gridInputs.RowCount = InputRows;
            gridInputs.SetupGrid();
            gridInputs.mode = Mode.CaseStudy;
            gridInputs.ColumnMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridInputs.ColOneMinimumWidth = 100;

            gridOutputs.RowCount = OutputRows;
            gridOutputs.SetupGrid();
            gridOutputs.mode = Mode.CaseStudy;
            gridOutputs.ColumnMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridOutputs.ColOneMinimumWidth = 100;

            gridOutputs.RowCount = ResultRows;
            gridOutputs.ColCount = Cols;
            gridResults.SetupGrid();
            gridResults.mode = Mode.CaseStudyResults;
            gridResults.ColumnMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridResults.ColOneMinimumWidth = 100;

            gridInputs.AllowUserToAddRows = true;
            gridOutputs.AllowUserToAddRows = true;

            LoadData(caseStudy);
        }

        private void LoadData(ModelEngine.CaseStudy caseStudy)
        {
            for (int sets = 0; sets < caseStudy.CaseSets.Count; sets++)
            {
                int Rows = caseStudy.CaseSets[sets].Count;
                gridInputs.RowCount = Rows;
                gridInputs.SetupGrid();

                for (int set = 0; set < caseStudy.CaseSets[sets].Count; set++)
                {
                }
            }

            for (int i = 0; i < caseStudy.CaseResults.Count; i++)
            {
            }
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
                set = new();
                for (int i = 0; i < gridInputs.RowCount; i++)
                {
                    if (gridInputs[3, i].Value is double d)
                        set.AddValue(gridInputs[2, i].uom, d);
                }

                caseStudy.AddSet(set);
            }

            for (int i = 0; i < gridOutputs.RowCount; i++)
            {
                StreamProperty prop;
                if (gridOutputs[2, i] is UOMGridCell cell)
                {
                    prop = cell.uom;
                    caseStudy.CaseOutput.AddOutput(prop);
                }
            }
            //CaseResultsres=caseStudy.RunCases();
        }

        //public eventRunFlowsheet();

        private void CaseStudyForm2_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}