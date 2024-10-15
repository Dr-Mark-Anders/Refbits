using System;
using System.Windows.Forms;
using Units.UOM;

public enum enumPRMixMethod
{ none, standard }

namespace ModelEngine
{
    public partial class Equilibrium : Form
    {
        //VLE_T_PREoS VLE;
        private Components data;
        private Port_Material port;

        public Equilibrium()
        {
            InitializeComponent();
        }

        public Equilibrium(Port_Material port)
        {
            data = port.cc;
            InitializeComponent();

            foreach (BaseComp bc in data)
            {
                //dataGridView1.Rows.Add(new  object [] { bc.Name, bc.MW.Round(4), bc.CritT.Round(4)+273.15, bc.CritP.Round(4)/10, bc.Omega.Round(4) });
            }

            // VLE = new  VLE_T_PREoS();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int No = data.Count;
            if (No != 2)
                return;

            string[] Names = new string[No];
            double[] Mws = new double[No];
            double[] Tcs = new double[No];
            double[] Pcs = new double[No];
            double[] Omega = new double[No];
            double[] x = new double[No];
            double[] y = new double[No];

            for (int n = 0; n < data.Count; n++)
            {
                Names[n] = data[n].Name;
                Mws[n] = data[n].MW;
                Tcs[n] = data[n].CritT + 273.15;
                Pcs[n] = data[n].CritP * 1e5; //  bar to pa
                Omega[n] = data[n].Omega;
                x[n] = data[n].MoleFraction;
                y[n] = data[n].MoleFraction;
            }

            double[] res;

            double TinC;
            dataGridView1.Rows.Clear();

            TinC = port.T;
            double tres;
            double startT, EndT;

            if (double.TryParse(txtStartT.Text, out tres))
                startT = tres;
            else
                startT = 0;

            if (double.TryParse(txtEndT.Text, out tres))
                EndT = tres;
            else
                EndT = 0;

            double k1, k2;

            for (int N = 0; N < (int)EndT - (int)startT; N++)
            {
                TinC = startT + N;

                PengRobinson_V3.K(No, Tcs, Pcs, Omega, TinC + 273.15, x, y, out res, port.P);

                dataGridView1.Rows.Add();
                k1 = res[0];
                k2 = res[1];

                dataGridView1.Rows[N].Cells[0].Value = TinC;
                dataGridView1.Rows[N].Cells[1].Value = k1;
                dataGridView1.Rows[N].Cells[2].Value = k2;
                dataGridView1.Rows[N].Cells[3].Value = k1 / k2;

                //k1 = thermodynamics.VapPress(TinC, data.P, data[0], enumVapPressure  .PR) / data.P;
                //k2 = thermodynamics.VapPress(TinC, data.P, data[1], enumVapPressure  .PR) / data.P;

                dataGridView1.Rows[N].Cells[4].Value = k1;
                dataGridView1.Rows[N].Cells[5].Value = k2;
                dataGridView1.Rows[N].Cells[6].Value = k1 / k2;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int No = data.Count;

            string[] Names = new string[No];
            double[] Mws = new double[No];
            double[] Tcs = new double[No];
            double[] Pcs = new double[No];
            double[] Omega = new double[No];
            double[] F = new double[No];
            double[] x = new double[No];
            double[] y = new double[No];

            for (int n = 0; n < data.Count; n++)
            {
                Names[n] = data[n].Name;
                Mws[n] = data[n].MW;
                Tcs[n] = data[n].CritT + 273.15;
                Pcs[n] = data[n].CritP * 1e5; //  bar to pa
                Omega[n] = data[n].Omega;
                x[n] = data[n].MoleFraction;
                y[n] = data[n].MoleFraction;
            }

            double[] res;

            double TinC = port.T;
            dataGridView1.Rows.Clear();

            double tres;
            double startT, EndT;

            if (double.TryParse(txtStartT.Text, out tres))
                startT = tres;
            else
                startT = 0;

            if (double.TryParse(txtEndT.Text, out tres))
                EndT = tres;
            else
                EndT = 0;

            for (int N = 0; N < (int)EndT - (int)startT; N++)
            {
                TinC = startT + N;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[N].Cells[0].Value = TinC;

                //PengRobinson_V3.K(No, Tcs, Pcs, Omega, TinC+273.15, x, y, out res, data.P);
                PengRobinson_V3.Kmix(No, Tcs, Pcs, Omega, TinC + 273.15, x, y, out res, port.P);

                for (int i = 0; i < res.Length; i++)
                {
                    dataGridView1.Rows[N].Cells[i + 1].Value = res[i];
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int No = data.Count;

            string[] Names = new string[No];
            double[] Mws = new double[No];
            double[] Tcs = new double[No];
            double[] Pcs = new double[No];
            double[] Omega = new double[No];
            double[] F = new double[No];
            double[] x = new double[No];
            double[] y = new double[No];

            for (int n = 0; n < data.Count; n++)
            {
                Names[n] = data[n].Name;
                Mws[n] = data[n].MW;
                Tcs[n] = data[n].CritT + 273.15;
                Pcs[n] = data[n].CritP * 1e5; //  bar to pa
                Omega[n] = data[n].Omega;
                x[n] = data[n].MoleFraction;
                y[n] = data[n].MoleFraction;
            }

            double[] res;

            double TinC = port.T;
            dataGridView1.Rows.Clear();

            double tres;
            double startT, EndT;

            if (double.TryParse(txtStartT.Text, out tres))
                startT = tres;
            else
                startT = 0;

            if (double.TryParse(txtEndT.Text, out tres))
                EndT = tres;
            else
                EndT = 0;

            for (int N = 0; N < (int)EndT - (int)startT; N++)
            {
                TinC = startT + N;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[N].Cells[0].Value = TinC;

                PengRobinson_V3.K(No, Tcs, Pcs, Omega, TinC + 273.15, x, y, out res, port.P);
                //PengRobinson_V3.Kmix(No, Tcs, Pcs, Omega, TinC + 273.15, x, y, out res, data.P);

                for (int i = 0; i < res.Length; i++)
                {
                    dataGridView1.Rows[N].Cells[i + 1].Value = res[i];
                }
            }
        }

        public static double[] GetKValues(Components o, Pressure P, Temperature T)
        {
            int No = o.Count;

            string[] Names = new string[No];
            double[] Mws = new double[No];
            double[] Tcs = new double[No];
            double[] Pcs = new double[No];
            double[] Omega = new double[No];
            double[] F = new double[No];
            double[] x = new double[No];
            double[] y = new double[No];

            for (int n = 0; n < No; n++)
            {
                Names[n] = o[n].Name;
                Mws[n] = o[n].MW;
                Tcs[n] = o[n].CritT + 273.15;
                Pcs[n] = o[n].CritP * 1e5; //  bar to pa
                Omega[n] = o[n].Omega;
                x[n] = o[n].MoleFraction;
                y[n] = o[n].MoleFraction;
            }

            double[] res = null;

            PengRobinson_V3.K(No, Tcs, Pcs, Omega, T + 273.15, x, y, out res, P);

            return res;
        }
    }
}