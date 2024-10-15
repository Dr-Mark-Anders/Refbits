using ModelEngine;
using System.Windows.Forms;

namespace Units.PortForm
{
    public partial class StreamProperties : Form
    {
        public StreamProperties(Components cc)
        {
            InitializeComponent();
            int row = 0;

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                row = dataGridProps.Rows.Add();
                dataGridProps[0, row].Value = cc[i].Name;
                dataGridProps[1, row].Value = cc[i].MW;
                dataGridProps[2, row].Value = cc[i].SG_60F;
                dataGridProps[3, row].Value = cc[i].BP.Celsius;
                dataGridProps[4, row].Value = cc[i].CritT;
                dataGridProps[5, row].Value = cc[i].CritP;
                dataGridProps[6, row].Value = cc[i].CritZ;
                dataGridProps[7, row].Value = cc[i].CritV;
                dataGridProps[8, row].Value = cc[i].Omega;
            }
        }
    }
}