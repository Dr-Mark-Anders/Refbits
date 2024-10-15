using Math2;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Units.UOM;
using Extensions;

namespace DialogControls
{
    public partial class Distillations : Form
    {
        private TemperatureUnit tempUnits = TemperatureUnit.Celsius;

        private DistPoints distdata = new(true);
        private StreamMaterial stream;

        private Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> PNAO;
        private UOMProperty prop1 = new();
        private UOMProperty prop2 = new();
        private UOMProperty prop3 = new();
        private UOMProperty prop4 = new();
        private UOMProperty prop5 = new();
        private UOMProperty prop6 = new();
        private UOMProperty prop7 = new();
        private UOMProperty prop8 = new();
        private UOMProperty prop9 = new();
        private UOMProperty prop10 = new();
        private UOMProperty prop11 = new();

        private UOMProperty Density;

        public Distillations(DistPoints distdata)
        {
            InitializeComponent();
            this.CBXTempUnits.SelectedIndexChanged += new System.EventHandler(this.CBXTempUnits_SelectedIndexChanged);
            this.distdata = distdata;
            this.PNAO = distdata.PNAO;
            this.Density = distdata.DENSITY;

            CBXTempUnits.DataSource = Enum.GetNames(typeof(TemperatureUnit));
            CBXTempUnits.SelectedItem = tempUnits.ToString();
        }

        public Distillations(StreamMaterial stream)
        {
            InitializeComponent();
            this.stream = stream;
            this.distdata = stream.plantdata.Distpoints;
            this.PNAO = distdata.PNAO;
            this.Density = distdata.DENSITY;
            CBXTempUnits.DataSource = Enum.GetNames(typeof(TemperatureUnit));
            CBXTempUnits.SelectedItem = tempUnits.ToString();
            this.CBXTempUnits.SelectedIndexChanged += new System.EventHandler(this.CBXTempUnits_SelectedIndexChanged);
        }

        public Distillations(PseudoPlantData pdata)
        {
            InitializeComponent();
            this.distdata = pdata.Distpoints;
            this.PNAO = distdata.PNAO;
            this.Density = distdata.DENSITY;
            CBXTempUnits.DataSource = Enum.GetNames(typeof(TemperatureUnit));
            CBXTempUnits.SelectedItem = tempUnits.ToString();
            this.CBXTempUnits.SelectedIndexChanged += new System.EventHandler(this.CBXTempUnits_SelectedIndexChanged);
        }

        public Distillations(StreamMaterial stream, Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> PNAO)
        {
            InitializeComponent();
            this.CBXTempUnits.SelectedIndexChanged += new System.EventHandler(this.CBXTempUnits_SelectedIndexChanged);
            //this.distdata = stream.distpts;
            this.PNAO = PNAO;
            PropGridDensity.Visible = false;
        }

        public Distillations()
        {
        }

        public Distillations(DistPoints distdata, Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> PNAO)
        {
            InitializeComponent();
            this.CBXTempUnits.SelectedIndexChanged += new System.EventHandler(this.CBXTempUnits_SelectedIndexChanged);
            this.distdata = distdata;
            this.PNAO = PNAO;
            this.Density = distdata.DENSITY;
            PropGridDensity.Visible = false;
        }

        private void pdg_Load(object sender, System.EventArgs e)
        {
            if (distdata is null)
                return;

            CBXTempUnits.DataSource = Enum.GetNames(typeof(TemperatureUnit));
            tempUnits = distdata.TemperatureUnits;
            CBXTempUnits.SelectedIndex = (int)(TemperatureUnit)tempUnits;

            prop1 = distdata[0].BP_UOM;
            prop2 = distdata[1].BP_UOM;
            prop3 = distdata[2].BP_UOM;
            prop4 = distdata[3].BP_UOM;
            prop5 = distdata[4].BP_UOM;
            prop6 = distdata[5].BP_UOM;
            prop7 = distdata[6].BP_UOM;
            prop8 = distdata[7].BP_UOM;
            prop9 = distdata[8].BP_UOM;
            prop10 = distdata[9].BP_UOM;
            prop11 = distdata[10].BP_UOM;

            prop1.DisplayName = "1%";
            prop2.DisplayName = "5%";
            prop3.DisplayName = "10%";
            prop4.DisplayName = "20%";
            prop5.DisplayName = "30%";
            prop6.DisplayName = "50%";
            prop7.DisplayName = "70%";
            prop8.DisplayName = "80%";
            prop9.DisplayName = "90%";
            prop10.DisplayName = "95%";
            prop11.DisplayName = "99%";

            pdg.Add(prop1, "1%");
            pdg.Add(prop2, "5%");
            pdg.Add(prop3, "10%");
            pdg.Add(prop4, "20%");
            pdg.Add(prop5, "30%");
            pdg.Add(prop6, "50%");
            pdg.Add(prop7, "70%");
            pdg.Add(prop8, "80%");
            pdg.Add(prop9, "90%");
            pdg.Add(prop10, "95%");
            pdg.Add(prop11, "99%");

            distillationBasis1.Value = distdata.disttype;

            pdg.UpdateValues();

            pdgPNA.Add(PNAO.Item1, "P");
            pdgPNA.Add(PNAO.Item2, "N");
            pdgPNA.Add(PNAO.Item3, "A");
            pdgPNA.Add(PNAO.Item4, "O");
            pdgPNA.UpdateValues();

            PropGridDensity.Add(Density, "Density");
            PropGridDensity.UpdateValues();

            PropGridDensity.Refresh();
        }

        private void Distillations_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (distdata is null)
                return;
        }

        private void pdg_ValueChanged(object sender, EventArgs e)
        {
            FillData();
        }

        public void FillData()
        {
           // DistPoints newdistdata = distdata.FillMissingData();

            prop1.DisplayUnit = tempUnits.ToString();
            prop2.DisplayUnit = tempUnits.ToString();
            prop3.DisplayUnit = tempUnits.ToString();
            prop4.DisplayUnit = tempUnits.ToString();
            prop5.DisplayUnit = tempUnits.ToString();
            prop6.DisplayUnit = tempUnits.ToString();
            prop7.DisplayUnit = tempUnits.ToString();
            prop8.DisplayUnit = tempUnits.ToString();
            prop9.DisplayUnit = tempUnits.ToString();
            prop10.DisplayUnit = tempUnits.ToString();
            prop11.DisplayUnit = tempUnits.ToString();

            /* if (!prop1.IsInput)
             {
                 prop1.origin = SourceEnum.Default;
             }

             if (!prop2.IsInput)
             {
                 prop2.origin = SourceEnum.Default;
             }

             if (!prop3.IsInput)
             {
                 prop3.origin = SourceEnum.Default;
             }

             if (!prop4.IsInput)
             {
                 prop4.origin = SourceEnum.Default;
             }

             if (!prop5.IsInput)
             {
                 prop5.origin = SourceEnum.Default;
             }

             if (!prop6.IsInput)
             {
                 prop6.origin = SourceEnum.Default;
             }

             if (!prop7.IsInput)
             {
                 prop7.origin = SourceEnum.Default;
             }

             if (!prop8.IsInput)
             {
                 prop8.origin = SourceEnum.Default;
             }

             if (!prop9.IsInput)
             {
                 prop9.origin = SourceEnum.Default;
             }

             if (!prop10.IsInput)
             {
                 prop10.origin = SourceEnum.Default;
             }

             if (!prop11.IsInput)
             {
                 prop11.origin = SourceEnum.Default;
             }*/

            pdg.Clear();
            pdg.Add(prop1);
            pdg.Add(prop2);
            pdg.Add(prop3);
            pdg.Add(prop4);
            pdg.Add(prop5);
            pdg.Add(prop6);
            pdg.Add(prop7);
            pdg.Add(prop8);
            pdg.Add(prop9);
            pdg.Add(prop10);
            pdg.Add(prop11);

            prop1.DisplayUnit = tempUnits.ToString();
            prop2.DisplayUnit = tempUnits.ToString();
            prop3.DisplayUnit = tempUnits.ToString();
            prop4.DisplayUnit = tempUnits.ToString();
            prop5.DisplayUnit = tempUnits.ToString();
            prop6.DisplayUnit = tempUnits.ToString();
            prop7.DisplayUnit = tempUnits.ToString();
            prop8.DisplayUnit = tempUnits.ToString();
            prop9.DisplayUnit = tempUnits.ToString();
            prop10.DisplayUnit = tempUnits.ToString();
            prop11.DisplayUnit = tempUnits.ToString();

            pdg.UpdateValues();
            pdg.Refresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            pdgPNA.Clear();
            PropGridDensity.Clear();
            distdata.Clear();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (distdata.Count == 11 && distdata.isValid())
            {
                DistPoints points = DistillationConversions.Convert(enumDistType.D86, enumDistType.TBP_VOL, distdata);
                Temperature MEABP = points.MeABP();
                double Wk = MEABP.Rankine.Pow(1 / 3D) / (distdata.DENSITY / 999.9);

                Components cc = stream.Port.cc;
                double LVPct;
                List<double> CumLVPC = new();

                for (int i = 0; i < cc.Count; i++)
                {
                    if (cc[i].IsPure)
                    {
                        cc[i].MoleFraction = 0;
                        continue;
                    }

                    LVPct = CubicSpline.CubSpline(eSplineMethod.Constrained, cc[i].BP, points.GetKDoubles(), points.getPCTs());

                    if (LVPct < 0)
                        LVPct = 0;
                    else if (LVPct > 100)
                        LVPct = 100;

                    CumLVPC.Add(LVPct);
                }

                for (int i = 0; i < cc.Count; i++)
                {
                    cc[i].MoleFraction = CumLVPC[i] - CumLVPC[i - 1];
                    cc[i].Density = cc[i].BP.Rankine.Pow(1 / 3D) / Wk * 999.9;
                }
            }
        }

        private void CBXTempUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void CBXTempUnits_SelectionChangeCommitted(object sender, EventArgs e)
        {
            tempUnits = (TemperatureUnit)CBXTempUnits.SelectedIndex;
            FillData();
        }
    }
}