using WeifenLuo.WinFormsUI.Docking;

namespace Units
{
    public partial class SetupSimulationForm : DockContent
    {
        private readonly MainForm m_form;
        /*  public  SetupSimulationForm()
          {
              InitializeComponent();
          }*/

        public SetupSimulationForm(MainForm mainform)
        {
            InitializeComponent();
            m_form = mainform;
        }

        private void Compositions_Click(object sender, System.EventArgs e)
        {
            m_form.Compositions_Click(sender, e);
        }

        private void Thermo_Click(object sender, System.EventArgs e)
        {
            m_form.Thermo_Click(sender, e);
        }

        private void btn_Worksheet_Click(object sender, System.EventArgs e)
        {
            m_form.WorkSheet_Click(sender, e);
        }

        private void btn_CaseStudies_Click(object sender, System.EventArgs e)
        {
            m_form.CaseStudy_Click(sender, e);
        }
    }
}