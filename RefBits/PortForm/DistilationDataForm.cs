using ModelEngine;
using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class DistilationDataForm : Form
    {
        private readonly Port_Material port;

        public DistilationDataForm()
        {
            InitializeComponent();
            Update();
        }

        public DistilationDataForm(Port_Material port)
        {
            this.port = port;
            InitializeComponent();
            Update();
        }

        public new void Update()
        {
            DGV.Rows.Clear();
            DistPoints TBP = port.cc.CreateShortTBPCurveFromLVpct();
            DistPoints TBPWT = DistillationConversions.Convert(enumDistType.TBP_VOL, enumDistType.TBP_WT, TBP);
            DistPoints D86 = DistillationConversions.Convert(enumDistType.TBP_VOL, enumDistType.D86, TBP);
            DistPoints D1160 = DistillationConversions.Convert(enumDistType.TBP_VOL, enumDistType.D1160, TBP);
            DistPoints D2887 = DistillationConversions.Convert(enumDistType.TBP_VOL, enumDistType.D2887, TBP);

            for (int i = 0; i < TBP.Count; i++)
            {
                int row = DGV.Rows.Add();
                DGV[0, row].Value = Global.Lvpct_standard[row].ToString();
                DGV[1, row].Value = D86[row].BP.Celsius.ToString("F2");
                DGV[2, row].Value = D1160[row].BP.Celsius.ToString("F2");
                DGV[3, row].Value = TBPWT[row].BP.Celsius.ToString("F2");
                DGV[4, row].Value = TBP[row].BP.Celsius.ToString("F2");
                DGV[5, row].Value = D2887[row].BP.Celsius.ToString("F2");
            }
        }

        private void rbVolume_CheckedChanged(object sender, EventArgs e)
        {
            Update();
        }

        private void rbMass_CheckedChanged(object sender, EventArgs e)
        {
            Update();
        }

        private void rbMolar_CheckedChanged(object sender, EventArgs e)
        {
            Update();
        }
    }
}