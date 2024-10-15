using ModelEngine;
using System.Windows.Forms;

namespace Units
{
    public partial class PALocationDialog : Form
    {
        private Column column;
        public int SectioNo = 0;
        public int StageNo = 0;

        public PALocationDialog(Column column)
        {
            this.column = column;
            InitializeComponent();

            for (int i = 0; i < column.NoSections; i++)
                cbSection.Items.Add(i);
        }

        private void cbSection_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TraySection section;

            int sectionNo = cbSection.SelectedIndex;

            if (sectionNo < column.NoSections)
                section = column.TraySections[sectionNo];
            else
                return;

            for (int i = 0; i < section.Trays.Count; i++)
                cbStage.Items.Add(i);
        }

        private void label3_Click(object sender, System.EventArgs e)
        {
        }

        private void label4_Click(object sender, System.EventArgs e)
        {
        }
    }
}