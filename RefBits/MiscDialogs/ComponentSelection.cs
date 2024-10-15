using ModelEngine;
using ModelEngine;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Units
{
    public enum eTypes
    {
        Common, NP, IP, Naphthenes, Olefins, Aromatics, Alcohols, Amines, Ketones, Aldehydes, Esters, Carboxyls, Halogens, Nitriles, Ethers, Others
    }

    public partial class ComponenetSelection : Form
    {
        private readonly GraphicsList gl;
        private readonly FlowSheet fs;
        private readonly Components cc;

        public ComponenetSelection(GraphicsList gs, FlowSheet fs)
        {
            this.gl = gs;
            this.fs = fs;
            InitializeComponent();
            //FillFractionData();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public ComponenetSelection(DrawMaterialStream stream)
        {
            InitializeComponent();
            gl = stream.DrawArea.GraphicsList;
            fs = stream.DrawArea.Flowsheet;
        }

        public ComponenetSelection(Components cc)
        {
            this.cc = cc;
            InitializeComponent();
            gl = null;
            fs = null;
            FillComponentNames();
        }

        private void StreamDataEntry_Load(object sender, EventArgs e)
        {
            FillComponentNames();
            FilterType.Items.AddRange(Enum.GetNames(typeof(eTypes)));
            //dv.Sort="TB";
        }

        private DataView dv;

        public void FillComponentNames()
        {
            dv = new DataView(Thermodata.compData);
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = dv;

            if (gl != null)
            {
                for (int i = 0; i < gl.Components.Count; i++)
                {
                    dgv2.Rows.Add();
                    dgv2[0, i].Value = gl.Components[i].CAS;
                    dgv2[1, i].Value = gl.Components[i].Name;
                    dgv2[2, i].Value = gl.Components[i].Formula;
                }
            }
            if (cc != null)
            {
                for (int i = 0; i < cc.Count; i++)
                {
                    dgv2.Rows.Add();
                    dgv2[0, i].Value = cc[i].CAS;
                    dgv2[1, i].Value = cc[i].Name;
                    dgv2[2, i].Value = cc[i].Formula;
                }
            }
        }

        private void FilterTxt_TextChanged(object sender, EventArgs e)
        {
            String Filter = FilterCAS.Text;
            dv.RowFilter = "CAS LIKE'" + Filter + "*'";
        }

        private void AddQuasi_Click(object sender, EventArgs e)
        {
            double SG = Convert.ToDouble(textBoxSG.Text);
            double LBP = Convert.ToDouble(textBoxLBP.Text);
            double UBP = Convert.ToDouble(textBoxUBP.Text);

            ThermoDynamicOptions thermo = new();

            PseudoComponent pc = new(SG, (LBP + UBP) / 2, new(LBP), new(UBP), "", thermo);
            pc.Name = "PC" + gl.Components.Count.ToString();
            gl.Components.Add(pc);

            FillComponentNames();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gl.Components.Clear();
            dgv2.Rows.Clear();
        }

        private void FilterName_TextChanged(object sender, EventArgs e)
        {
            string Filter = FilterName.Text;
            dv.RowFilter = "NAME LIKE'" + Filter + "*'";
        }

        private void FilterFormula_TextChanged(object sender, EventArgs e)
        {
            string Filter = FilterFormula.Text;
            dv.RowFilter = "FORMULA LIKE'" + Filter + "*'";
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private void DataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left
            && dragBoxFromMouseDown != Rectangle.Empty &&
            !dragBoxFromMouseDown.Contains(e.X, e.Y))
            {
                //Proceedwiththedraganddrop,passingint helistitem.
                dgv.DoDragDrop(dgv.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
            }
        }

        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            //Gettheindexoftheitemthemouseisbelow.
            rowIndexFromMouseDown = dgv.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                //Rememberthepoint wherethemousedownoccurred.
                //TheDragSizeindicatesthesizethatthemousecanmove
                //beforeadrageventshouldbestarted.
                Size dragSize = SystemInformation.DragSize;

                //Createarectangleusing theDragSize,withthemousepositionbeing
                //atthecenteroftherectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                e.Y - (dragSize.Height / 2)),
                dragSize);
            }
            else
                //Resettherectangleifthemouseisnotoveranitemint heListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            //Themouselocationsarerelativetothescreen,sotheymustbe
            //convertedtoclientcoordinates.
            Point clientPoint = dgv2.PointToClient(new Point(e.X, e.Y));

            //Gettherowindexoftheitemthemouseisbelow.
            rowIndexOfItemUnderMouseToDrop = dgv2.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            //Ifthedragoperationwasamovethenremoveandinserttherow.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                string name = rowToMove.Cells[1].Value.ToString();

                var items = this.dgv2.Rows.Cast<DataGridViewRow>().Where(row => row.Cells[1].Value.ToString() == name);

                if (!items.Any())
                {
                    if (rowIndexOfItemUnderMouseToDrop == -1)
                    {
                        int row = dgv2.Rows.Add();
                        dgv2[0, row].Value = rowToMove.Cells[0].Value;
                        dgv2[1, row].Value = rowToMove.Cells[1].Value;
                        dgv2[2, row].Value = rowToMove.Cells[2].Value;
                    }
                    else
                    {
                        dgv2.Rows.Insert(rowIndexOfItemUnderMouseToDrop, 1);
                        dgv2[0, rowIndexOfItemUnderMouseToDrop].Value = rowToMove.Cells[0].Value;
                        dgv2[1, rowIndexOfItemUnderMouseToDrop].Value = rowToMove.Cells[1].Value;
                        dgv2[2, rowIndexOfItemUnderMouseToDrop].Value = rowToMove.Cells[2].Value;
                    }
                }
                //dgv.Rows.RemoveAt(rowIndexFromMouseDown);
            }
            fs.UpdateAllPortComponents(gl.Components);
            fs.EraseEstimates();
            fs.EraseMoleFractions();
        }

        private void Componentselection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fs != null)
                fs.UpdateAllPortComponents(gl.Components);
            UpdateGSComponentlist();
        }

        public void UpdateGSComponentlist()
        {
            if (gl != null)
            {
                gl.Components.Clear();
                for (int i = 0; i < dgv2.RowCount; i++)
                {
                    if (dgv2[1, i].Value != null)
                    {
                        BaseComp bc = Thermodata.GetComponent(dgv2[1, i].Value.ToString());
                        if (bc != null)
                            gl.Components.Add(bc);
                    }
                }
            }
            if (cc != null)
            {
                cc.Clear();
                for (int i = 0; i < dgv2.RowCount; i++)
                {
                    if (dgv2[1, i].Value != null)
                    {
                        BaseComp bc = Thermodata.GetComponent(dgv2[1, i].Value.ToString());
                        if (bc != null)
                            cc.Add(bc);
                    }
                }
            }

            AddCrudeQuasis();
        }

        private void AddCrudeQuasis()
        {
            if (gl != null)
                gl.Components.Add(Quasis);
        }

        private void FilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Enum.TryParse(FilterType.SelectedItem.ToString(), out eTypes type))
            {
                switch (type)
                {
                    case eTypes.Common:
                        AddCommonLightCompounds();
                        break;

                    case eTypes.NP:
                        dv.RowFilter = "Type='PN'";
                        break;

                    case eTypes.IP:
                        dv.RowFilter = "Type='IP'";
                        break;

                    case eTypes.Naphthenes:
                        dv.RowFilter = "Type='N'";
                        break;

                    case eTypes.Olefins:
                        dv.RowFilter = "Type='OL'";
                        break;

                    case eTypes.Aromatics:
                        dv.RowFilter = "Type='A'";
                        break;

                    case eTypes.Alcohols:
                        dv.RowFilter = "Type='OL'";
                        break;

                    case eTypes.Amines:
                        dv.RowFilter = "Type='MISC'";
                        break;

                    case eTypes.Ketones:
                        dv.RowFilter = "Type='KET'";
                        break;

                    case eTypes.Aldehydes:
                        break;

                    case eTypes.Esters:
                        dv.RowFilter = "Type='ES'";
                        break;

                    case eTypes.Carboxyls:
                        break;

                    case eTypes.Halogens:
                        dv.RowFilter = "Type='HAL'";
                        break;

                    case eTypes.Nitriles:
                        break;

                    case eTypes.Ethers:
                        break;

                    case eTypes.Others:
                        dv.RowFilter = "Type='MISC'";
                        break;

                    default:
                        break;
                }
            }
        }

        private void FilterMW_TextChanged(object sender, EventArgs e)
        {
            string Filter = FilterMW.Text;
            dv.RowFilter = "MW LIKE'" + Filter + "*'";
        }

        private void AddCommonLightCompounds()
        {
            object[] commons = new object[] { "H2O", "H2", "H2S", "CO", "CO2", "COS", "Methane", "Ethane", "Propane", "n-Butane", "2-Methylpropane", "n-Pentane", "2-Methylbutane" };
            string Filter = "";

            for (int i = 0; i < commons.Length; i++)
            {
                Filter += "NAME = '" + commons[i] + "'";
                if (i < commons.Length - 1)
                    Filter += " OR ";
            }

            dv.RowFilter = Filter;
            dv.Sort = "TB";
        }

        private void AddComponent_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection cells = dgv.SelectedCells;
            int DGV2row;

            foreach (DataGridViewCell item in cells)
            {
                var rowToMove = dgv.Rows[item.RowIndex];
                string name = rowToMove.Cells[1].Value.ToString();

                var items = this.dgv2.Rows.Cast<DataGridViewRow>().Where(row => row.Cells[1].Value.ToString() == name);

                if (!items.Any())
                {
                    DGV2row = dgv2.Rows.Add();
                    dgv2[0, DGV2row].Value = dgv[0, item.RowIndex].Value;
                    dgv2[1, DGV2row].Value = dgv[1, item.RowIndex].Value;
                    dgv2[2, DGV2row].Value = dgv[2, item.RowIndex].Value;
                }
            }
            UpdateGSComponentlist();
            if (fs != null)
                fs.UpdateAllPortComponents(gl.Components);
        }

        private void RemoveComponent_Click(object sender, EventArgs e)
        {
            DataGridViewCell[] cells = new DataGridViewCell[dgv2.SelectedCells.Count];
            dgv2.SelectedCells.CopyTo(cells, 0);

            foreach (DataGridViewCell item in cells)
            {
                if (item.RowIndex > -1 && item.RowIndex < dgv2.RowCount)
                    dgv2.Rows.RemoveAt(item.RowIndex);
            }

            UpdateGSComponentlist();
            fs.UpdateAllPortComponents(gl.Components);
            fs.EraseEstimates();
            fs.EraseMoleFractions();
        }

        private void FilterBP_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(FilterBP.Text, out double BP))
            {
                dv.RowFilter = "TB > '" + BP.ToString() + "'";
                dv.Sort = "TB";
            }
        }

        private readonly Components Quasis = new();

        private void CreateAssay_Click(object sender, EventArgs e)
        {
            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[]{"H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
"n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
"Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
"Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
"Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
"Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
"Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[]{0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[]{0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
1.0720,1.0965,1.1209};

            double[] BPk = new double[]{311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[]{32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[]{0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[]{72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[]{0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108};

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                if (sc != null)
                {
                    sc.MoleFraction = RealFractions[i];
                    cc.Add(sc);
                }
            }

            Quasis.Clear();

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
                Quasis.Add(pc);
            }

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                gl.Components.Add(cc[i]);
            }

            FillComponentNames();

            for (int i = 0; i < gl.Components.Count; i++)
            {
                gl.Components[i].MoleFraction = double.NaN;
            }

            gl.Components.Origin = SourceEnum.Empty;

            fs.EraseMoleFractions();
            fs.UpdateAllPortComponents(gl.Components);
            fs.EraseEstimates();
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new();
            BinaryFormatter b = new();
            DrawMaterialStream matstream;

            openFileDialog1.Filter = "data files (*.prp)|*.prp";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    matstream = (DrawMaterialStream)b.Deserialize(myStream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                    //Codetowritethestreamgoeshere.
                    myStream.Close();

                    Components cc = matstream.Port.cc;
                    gl.Components.Clear();
                    for (int i = 0; i < cc.ComponentList.Count; i++)
                    {
                        gl.Components.Add(cc[i]);
                    }
                    FillComponentNames();
                    fs.UpdateAllPortComponents(gl.Components);
                }
            }
        }
    }
}