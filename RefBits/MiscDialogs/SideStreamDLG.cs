using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class SideStreamFrm : Form
    {
        public SideStreamFrm()
        {
            InitializeComponent();
        }

        public SideStreamFrm(Column rc)
        {
        }

        private void PumpAroundsFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection src = dataGridView1.SelectedRows;
            foreach (DataGridViewRow r in src)
                dataGridView1.Rows.Remove(r);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}