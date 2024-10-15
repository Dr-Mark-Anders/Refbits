using System;
using System.Windows.Forms;

namespace Units
{
    public partial class SelectionDLG : Form
    {
        public string DLGResult;

        public SelectionDLG()
        {
            InitializeComponent();
        }

        public SelectionDLG(string[] list)
        {
            InitializeComponent();
            comboBox1.Items.AddRange(list);
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DLGResult = comboBox1.SelectedItem.ToString();
        }
    }
}