using System;
using System.Threading;
using System.Windows.Forms;

namespace ModelEngine
{
    public partial class ViewArray : Form
    {
        public ViewArray()
        {
            InitializeComponent();
        }

        public void View(double[,] Arr)
        {
            dataGridView1.Columns.Clear();

            for (int col = 0; col <= Arr.GetUpperBound(0); col++)
            {
                dataGridView1.Columns.Add("Column" + col.ToString(), col.ToString());
            }

            for (int row = 0; row <= Arr.GetUpperBound(0); row++)
            {
                dataGridView1.Rows.Add();
            }

            for (int rows = 0; rows <= Arr.GetUpperBound(0); rows++)
            {
                for (int col = 0; col <= Arr.GetUpperBound(0); col++)
                {
                    dataGridView1[col, rows].Value = Arr[rows, col];
                }
            }

            this.ShowDialog();
        }

        [STAThread]
        public void View(double[][] Arr)
        {
            dataGridView1.Columns.Clear();

            for (int i = 0; i <= Arr[0].GetUpperBound(0); i++)
            {
                dataGridView1.Columns.Add("Column" + i.ToString(), i.ToString());
            }

            for (int i = 0; i <= Arr.GetUpperBound(0); i++)
            {
                dataGridView1.Rows.Add();
            }

            for (int i = 0; i <= Arr.GetUpperBound(0); i++)
            {
                for (int y = 0; y <= Arr[i].GetUpperBound(0); y++)
                {
                    dataGridView1[y, i].Value = Arr[i][y];
                }
            }

            this.ShowDialog();
        }

        [STAThread]
        private void Copy()
        {
            DataObject d = dataGridView1.GetClipboardContent();
            Clipboard.SetDataObject(d, true);
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show();
        }

        [STAThread]
        private void copyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(Copy);          // Kick off a new  thread
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        [STAThread]
        private void btnTextReadReports_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(Copy);          // Kick off a new  thread
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();                               // running WriteY()
        }
    }
}