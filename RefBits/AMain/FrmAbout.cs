using System;
using System.Windows.Forms;

namespace Units
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            this.Text = "About " + Application.ProductName;

            lblText.Text = "Program: " + Application.ProductName + "\n" +
                "Version: " + Application.ProductVersion;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}