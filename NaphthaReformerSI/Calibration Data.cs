using DialogControls;
using ModelEngine;
using NaphthaReformer;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Units;
using Units.UOM;

namespace NaphthaReformerSI
{
    public partial class Calibration_Data : Form
    {
        private ReformerDataCollection CalibratioDataCollection = new ReformerDataCollection();
        private ReformerDataSet dataset;
        private UOMPropertyList DistPoints = new UOMPropertyList();
        private UOMPropertyList PNA = new UOMPropertyList();

        public Calibration_Data()
        {
            InitializeComponent();
            dataset = CalibratioDataCollection[0];
        }

        private void singleDataEntry1_Load(object sender, EventArgs e)
        {
        }

        private void btnAddCase_Click(object sender, EventArgs e)
        {
            dataset = CalibratioDataCollection.Add();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            CalibratioDataCollection.Remove(dataset);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CalibratioDataCollection.Add(dataset.Clone());
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnFeed_Click(object sender, EventArgs e)
        {
            switch (shortMediumFull2.Value)
            {
                case enumShortMediumFull.Short:
                    Distillations dist = new Distillations(dataset.calibdata.FeedDistillation, dataset.calibdata.FeedPNAO);
                    dist.Show();
                    break;

                case enumShortMediumFull.Medium:
                    RefShortFeed pd = new RefShortFeed(dataset.calibdata.Shortgc);
                    pd.Show();
                    break;

                case enumShortMediumFull.Full:
                    RefFullFeed gc = new RefFullFeed(dataset.calibdata.Fullgc);
                    gc.ShowDialog();
                    break;

                default:
                    break;
            }
        }

        private void streamCompositionEntry1_Load(object sender, EventArgs e)
        {
            Products.ComponentNames.AddRange(dataset.calibdata.Products[0].Names);
            Products.setColumns(dataset.calibdata.streamnames);
            Products.SetData(dataset.calibdata.Products);
        }

        public void LoadData(ReformerDataSet dataset)
        {
            this.dataset = dataset;
            LoadCalibDataset();
        }

        public void LoadCalibDataset()
        {
            SepConditions.Clear();
            SepConditions.Add(dataset.calibdata.PSep);
            SepConditions.Add(dataset.calibdata.TSep);
            SepConditions.UpdateValues();

            UOMProperty prop1 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[0].BP, "IBP");
            UOMProperty prop2 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[1].BP, "5");
            UOMProperty prop3 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[2].BP, "10");
            UOMProperty prop4 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[3].BP, "30");
            UOMProperty prop5 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[4].BP, "50");
            UOMProperty prop6 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[5].BP, "70");
            UOMProperty prop7 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[6].BP, "90");
            UOMProperty prop8 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[7].BP, "95");
            UOMProperty prop9 = new(ePropID.T, SourceEnum.Input, dataset.calibdata.FeedDistillation[8].BP, "99");

            switch (dataset.calibdata.feedflow.UOM)
            {
                case MassFlow mf:
                    //txtFeedRate.UOMprop=new (mf);
                    break;

                case MoleFlow molf:
                    //txtFeedRate.UOMprop=new UOMProperty(molf);
                    break;

                case VolumeFlow vf:
                    //txtFeedRate.UOMprop=new UOMProperty(vf);
                    break;
            }

            //txtFeedRate.Value=dataset.flow.BaseValue;
            //pdg.Add(dataset.flow,"Rate");
            DistPoints.Clear();
            DistPoints.Add(prop1, "");
            DistPoints.Add(prop2);
            DistPoints.Add(prop3);
            DistPoints.Add(prop4);
            DistPoints.Add(prop5);
            DistPoints.Add(prop6);
            DistPoints.Add(prop7);
            DistPoints.Add(prop8);
            DistPoints.Add(prop9);
            DistPoints.Add(dataset.calibdata.feeddensity, "Density");

            LoadPNA();
            LoadShortGC();
            LoadFullGC();
            LoadReactor();
            LoadRecyle();
            LoadCatalyst();
            LoadFurnace();
            LoadNoRx();
            LoadCalibrationData();
        }

        private void LoadCalibrationData()
        {
            CalibFactors.Clear();

            CalibFactors.Add(dataset.calibdata.calibfactors["CrackP6"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["CrackP7"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["CrackP8"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["DealkA6"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["DealkA7"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["DealkA8"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["CycleP6"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["CycleP7"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["CycleP8"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["CycleP9"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["DehydN6"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["DehydN7"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["DehydN8"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["DehydN9"]);
            CalibFactors.Add(dataset.calibdata.calibfactors["EquilCyc9"]);
        }

        public void LoadPNA()
        {
            PNA.Clear();
            PNA.Add(dataset.calibdata.FeedPNAO.Item1);
            PNA.Add(dataset.calibdata.FeedPNAO.Item2);
            PNA.Add(dataset.calibdata.FeedPNAO.Item3);
            PNA.Add(dataset.calibdata.FeedPNAO.Item4);
        }

        public void LoadShortGC()
        {
        }

        public void LoadFullGC()
        {
        }

        public void LoadReactor()
        {
            RxData.Clear();
            RxOPData.Clear();

            RxData.Add(dataset.calibdata.R1CatLoad);
            RxOPData.Add(dataset.calibdata.R1Tin);
            RxOPData.Add(dataset.calibdata.R1Pin);
            RxOPData.Add(dataset.calibdata.R1PDrop);
            RxData.Add(dataset.calibdata.R1MetActivity);
            RxData.Add(dataset.calibdata.R1AcidActivity);

            RxData.Add(dataset.calibdata.R2CatLoad, "", 1);
            RxOPData.Add(dataset.calibdata.R2Tin, "", 1);
            RxOPData.Add(dataset.calibdata.R2Pin, "", 1);
            RxOPData.Add(dataset.calibdata.R2PDrop, "", 1);
            RxData.Add(dataset.calibdata.R2MetActivity, "", 1);
            RxData.Add(dataset.calibdata.R2AcidActivity, "", 1);

            RxData.Add(dataset.calibdata.R3CatLoad, "", 2);
            RxOPData.Add(dataset.calibdata.R3Tin, "", 2);
            RxOPData.Add(dataset.calibdata.R3Pin, "", 2);
            RxOPData.Add(dataset.calibdata.R3PDrop, "", 2);
            RxData.Add(dataset.calibdata.R3MetActivity, "", 2);
            RxData.Add(dataset.calibdata.R3AcidActivity, "", 2);

            RxData.UpdateValues();
            RxOPData.UpdateValues();
        }

        public void LoadRecyle()
        {
            RecData.Clear();
            RecData.Add(dataset.calibdata.H2HC);
            RecData.Add(dataset.calibdata.TSep);
            RecData.Add(dataset.calibdata.PSep);
            RecData.UpdateValues();
        }

        public void LoadCatalyst()
        {
            CatData.Clear();
            CatData.Add(dataset.calibdata.CatAmount);
            CatData.Add(dataset.calibdata.CatDensity);
            CatData.UpdateValues();
        }

        public void LoadFurnace()
        {
            FurnEff.Clear();
            FurnEff.Add(dataset.calibdata.furneff);
            FurnEff.UpdateValues();
        }

        public void LoadNoRx()
        {
            NoRx.Clear();
            NoRx.Add(dataset.calibdata.NoReactors);
            NoRx.UpdateValues();
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = (TabControl)sender;
            switch (tab.SelectedTab.Name)
            {
                case "Products":
                    Products.SetData(dataset.calibdata.Products);
                    break;

                default:
                    Products.GetData(dataset.calibdata.Products);
                    break;
            }
        }

        private void productNames_Load(object sender, EventArgs e)
        {
            //productNames.RowsChanged-=new FormControls.GCCompGrid.RowsChangedEventHandler(this.productNames_RowsChanged);
            productNames.DGV.Columns.Clear();
            productNames.DGV.Columns.Add(new DataGridViewTextBoxColumn());
            int rono;
            for (int i = 0; i < dataset.calibdata.streamnames.Count; i++)
            {
                rono = productNames.DGV.Rows.Add();
                productNames.DGV.Rows[rono].Cells[0].Value = dataset.calibdata.streamnames[i];
            }
            //productNames.RowsChanged+=new FormControls.GCCompGrid.RowsChangedEventHandler(this.productNames_RowsChanged);
        }

        private void productNames_RowsChanged(object sender, FormControls.RowsChangedEventArgs e)
        {
            dataset.calibdata.streamnames.Clear();
            for (int i = 0; i < productNames.DGV.RowCount; i++)
            {
                if (productNames.DGV.Rows[i].Cells[0].Value != null)
                {
                    string name = productNames.DGV.Rows[i].Cells[0].Value.ToString();
                    dataset.calibdata.streamnames.Add(name);
                }
            }
            Products.setColumns(dataset.calibdata.streamnames);

            MassBalance.ColumnNames = dataset.calibdata.streamnames;

            dataset.calibdata.Products.Clear();

            for (int i = 0; i < dataset.calibdata.streamnames.Count; i++)
            {
                dataset.calibdata.Products.Add(new RefomerFullCompList());
            }
        }

        private void Calibration_Data_Load(object sender, EventArgs e)
        {
            MassBalance.ColumnNames = dataset.calibdata.streamnames;
            MassBalance.DGV.Rows.Add();

            for (int i = 0; i < dataset.calibdata.Products.Count; i++)
            {
                MassBalance.Add(new UOMProperty(ePropID.NullUnits, dataset.calibdata.Products[i].MassFlow), "", i);
            }

            MassBalance.UpdateValues();
        }

        private void Calibration_Data_FormClosing(object sender, FormClosingEventArgs e)
        {
            Products.GetData(dataset.calibdata.Products);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SerializeNow();
        }

        public void SerializeNow()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            BinaryFormatter b = new BinaryFormatter();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    b.Serialize(myStream, dataset.calibdata);
                    myStream.Close();
                }
            }
        }

        public void LoadData()
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            BinaryFormatter b = new BinaryFormatter();

            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    dataset.calibdata = (ReformerCalibData)b.Deserialize(myStream);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }

            LoadCalibDataset();

            //cBcase Selection.Items.Clear();

            /*foreach(varitemindatasetcollection)
            {
            if(item.Name!=null)
            cBcase Selection.Items.Add(item.Name);
            }*/

            //cBcase Selection.Text=(string)cBcase Selection.Items[0];
            //cBcase Selection.SelectedItem=dataset.Name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            switch (shortMediumFull2.Value)
            {
                case enumShortMediumFull.Short:
                    Distillations dist = new Distillations(dataset.calibdata.FeedDistillation, dataset.simulationdata.feedPNAO);
                    dist.ShowDialog();
                    break;

                case enumShortMediumFull.Medium:
                    RefShortFeed shortgc = new RefShortFeed(dataset.calibdata.Shortgc);
                    shortgc.ShowDialog();
                    break;

                case enumShortMediumFull.Full:
                    RefFullFeed fullfd = new RefFullFeed(dataset.calibdata.Fullgc);
                    fullfd.ShowDialog();
                    break;

                default:
                    break;
            }
        }
    }
}