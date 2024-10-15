using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Units;

namespace ModelEngine
{
    public partial class PhaseDiagram : Form
    {
        private VLE_T_PREoS VLE = new VLE_T_PREoS();
        private Components data;

        public PhaseDiagram()
        {
            InitializeComponent();

            //default diagram
            dataGridView1.Rows.Add(new object[] { "Propane", 44.094, 369.8, 4.25, 0.1530 });
            dataGridView1.Rows.Add(new object[] { "n-Hexane", 86.178, 507.5, 3.01, 0.2990 });
            VLE = new VLE_T_PREoS();
        }

        public PhaseDiagram(Port_Material port, Components o)
        {
            data = o;
            InitializeComponent();

            foreach (BaseComp bc in data)
            {
                dataGridView1.Rows.Add(new object[] { bc.Name, bc.MW.Round(4), bc.CritT, bc.CritP, bc.Omega.Round(4) });
            }

            textBoxTemperature.Text = port.T.ToString();

            //VLE = new  VLE_T_PREoS();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int No = dataGridView1.Rows.Count - 1;

            string[] Names = new string[No];
            double[] Mws = new double[No];
            double[] Tcs = new double[No];
            double[] Pcs = new double[No];
            double[] Omega = new double[No];

            List<string[]> res = new List<string[]>();
            double result;
            double Temperature;
            dataGridView2.Rows.Clear();

            if (double.TryParse(textBoxTemperature.Text, out result))
                Temperature = Convert.ToDouble(textBoxTemperature.Text);
            else
            {
                Temperature = 150;
                textBoxTemperature.Text = "150";
            }

            for (int n = 0; n < No; n++)
            {
                Names[n] = dataGridView1.Rows[n].Cells[0].Value.ToString();
                Mws[n] = (double)dataGridView1.Rows[n].Cells[1].Value;
                Tcs[n] = (double)dataGridView1.Rows[n].Cells[2].Value;
                Pcs[n] = (double)dataGridView1.Rows[n].Cells[3].Value * 1e6;
                Omega[n] = (double)dataGridView1.Rows[n].Cells[4].Value;
            }

            VLE.VLE_T(Names, Mws, Tcs, Pcs, Omega, Temperature, 0.1, 200, 0.01, out res);

            foreach (string[] dr in res)
            {
                int row = dataGridView2.Rows.Add();

                for (int i = 0; i < 16; i++)
                {
                    dataGridView2.Rows[row].Cells[i].Value = dr[i];
                }
            }

            /* chart1.Series[0].Point s.Clear();
             chart1.Series[1].Point s.Clear();

             double  X, Y;

             for (int  point Index = 0; point Index < res.Count - 1; point Index++)
             {
                 X = Convert.Todouble (res[point Index][1]);
                 Y = Convert.Todouble (res[point Index][0]);

                 chart1.Series["Series1"].Point s.AddXY(X, Y);

                 X = Convert.Todouble (res[point Index][6]);
                 Y = Convert.Todouble (res[point Index][0]);

                 chart1.Series["Series2"].Point s.AddXY(X, Y);
             }

             // Set point  chart type
             chart1.Series["Series1"].ChartType = SeriesChartType.Line;
             chart1.Series["Series2"].ChartType = SeriesChartType.Line;

             // Enable data point s labels
             chart1.Series["Series1"].IsValueShownAsLabel = false;
             chart1.Series["Series1"]["LabelStyle"] = "Center";

             // Set marker size
             chart1.Series["Series1"].MarkerSize = 15;

             // Set marker shape
             chart1.Series["Series1"].MarkerStyle = MarkerStyle.None;
             chart1.Series["Series2"].MarkerStyle = MarkerStyle.None;*/

            // Set to 3D
            //chart.ChartAreas[strChartArea].Area3DStyle.Enable3D = true;
        }
    }
}