using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace Dialogs
{
    /// <summary>
    /// Summary description for FeedDLG.
    /// </summary>
    public class SynDLG : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container Components = null;

        public double Flow, Temp, Press;
        string[] DistType = new string[15];
        private Button button1;

        private ArrayList plantdata = new ArrayList();
        private Button button2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button3;
        private ListViewEdit grid2;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private ListViewEdit grid1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader13;
        private ColumnHeader columnHeader14;
        private ColumnHeader columnHeader15;
        private ColumnHeader columnHeader16;
        private ColumnHeader columnHeader17;
        Models.WOSISynthesis syn;

        public SynDLG(Models.WOSISynthesis s)
        {
            InitializeComponent();
            this.syn = s;
            plantdata.Add(new PlantData(new double[] { -56.7, -19.0, 8.1, 41.5, 63.2, 97.2, 133.1, 151.9, 171.5, 184.7, 194 }, 269.7, 0.7139, 0.01, 5, 10));
            plantdata.Add(new PlantData(new double[] { 178.3, 211.2, 223.2, 236.5, 246.3, 264.9, 285, 296.2, 311.3, 321.3, 336.4 }, 125.7, 0.8261, 0.1, 5, 10));
            plantdata.Add(new PlantData(new double[] { 244.1, 288.3, 305.4, 322.1, 333.4, 353.1, 374.1, 386.3, 401.2, 412.6, 425.1 }, 101.9, 0.8624, 0.5, 5, 10));
            plantdata.Add(new PlantData(new double[] { 282.1, 359.1, 394.3, 428.4, 452.9, 512.9, 578.3, 667.2, 739.3, 785, 823.1 }, 249.6, 0.9246, 0.7, 5, 10));
        }

        public SynDLG()
        {
            InitializeComponent();
            plantdata.Add(new PlantData(new double[] { -56.7, -19.0, 8.1, 41.5, 63.2, 97.2, 133.1, 151.9, 171.5, 184.7, 194 }, 269.7, 0.7139, 0.01, 5, 10));
            plantdata.Add(new PlantData(new double[] { 178.3, 211.2, 223.2, 236.5, 246.3, 264.9, 285, 296.2, 311.3, 321.3, 336.4 }, 125.7, 0.8261, 0.1, 5, 10));
            plantdata.Add(new PlantData(new double[] { 244.1, 288.3, 305.4, 322.1, 333.4, 353.1, 374.1, 386.3, 401.2, 412.6, 425.1 }, 101.9, 0.8624, 0.5, 5, 10));
            plantdata.Add(new PlantData(new double[] { 282.1, 359.1, 394.3, 428.4, 452.9, 512.9, 578.3, 667.2, 739.3, 785, 823.1 }, 249.6, 0.9246, 0.7, 5, 10));
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Components != null)
                {
                    Components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ListViewItem listViewItem1 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem2 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem3 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem4 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem5 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem6 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem7 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem8 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem9 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            ListViewItem listViewItem10 = new ListViewItem(new string[] {
            "",
            "",
            "",
            "",
            ""}, -1);
            this.button1 = new Button();
            this.button2 = new Button();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.grid2 = new Units.ListViewEdit();
            this.columnHeader7 = new ColumnHeader();
            this.columnHeader8 = new ColumnHeader();
            this.columnHeader9 = new ColumnHeader();
            this.columnHeader10 = new ColumnHeader();
            this.columnHeader11 = new ColumnHeader();
            this.columnHeader12 = new ColumnHeader();
            this.tabPage2 = new TabPage();
            this.button3 = new Button();
            this.grid1 = new Units.ListViewEdit();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.columnHeader4 = new ColumnHeader();
            this.columnHeader5 = new ColumnHeader();
            this.columnHeader6 = new ColumnHeader();
            this.columnHeader13 = new ColumnHeader();
            this.columnHeader14 = new ColumnHeader();
            this.columnHeader15 = new ColumnHeader();
            this.columnHeader16 = new ColumnHeader();
            this.columnHeader17 = new ColumnHeader();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            //
            // button1
            //
            this.button1.Location = new System.Drawing.Point(24, 443);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Synthesise";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // button2
            //
            this.button2.Location = new System.Drawing.Point(141, 443);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Chart";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            //
            // tabControl1
            //
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(897, 437);
            this.tabControl1.TabIndex = 3;
            //
            // tabPage1
            //
            this.tabPage1.Controls.Add(this.grid2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(889, 411);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "PureComps";
            this.tabPage1.UseVisualStyleBackColor = true;
            //
            // grid2
            //
            this.grid2.Columns.AddRange(new ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
            this.grid2.ComboBoxList = new string[0];
            this.grid2.Dock = DockStyle.Fill;
            this.grid2.FullRowSelect = true;
            this.grid2.Location = new System.Drawing.Point(3, 3);
            this.grid2.Name = "grid2";
            this.grid2.Names = new string[] {
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null};
            this.grid2.Size = new System.Drawing.Size(883, 405);
            this.grid2.TabIndex = 2;
            this.grid2.UseCompatibleStateImageBehavior = false;
            this.grid2.View = View.Details;
            //
            // tabPage2
            //
            this.tabPage2.Controls.Add(this.grid1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(889, 411);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Distillates";
            this.tabPage2.UseVisualStyleBackColor = true;
            //
            // button3
            //
            this.button3.Location = new System.Drawing.Point(255, 443);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Chart";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            //
            // grid1
            //
            this.grid1.Columns.AddRange(new ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17});
            this.grid1.ComboBoxList = new string[] {
        "TBP V%",
        "TBP W%",
        "D86",
        "D1160",
        "D2887"};
            this.grid1.Dock = DockStyle.Fill;
            this.grid1.FullRowSelect = true;
            this.grid1.Items.AddRange(new ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10});
            this.grid1.Location = new System.Drawing.Point(3, 3);
            this.grid1.Name = "grid1";
            this.grid1.Names = new string[] {
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null};
            this.grid1.Size = new System.Drawing.Size(883, 405);
            this.grid1.TabIndex = 2;
            this.grid1.UseCompatibleStateImageBehavior = false;
            this.grid1.View = View.Details;
            //
            // SynDLG
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(897, 490);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SynDLG";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Synthesis";
            this.Load += new System.EventHandler(this.FeedDLG_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        public void Calculate()
        {
            Oil o;
            OilSynthesis os = new OilSynthesis();
            o = os.Synthesise(ReadData());
            syn.Product.oil = o;
        }

        private void FeedDLG_Load(object sender, System.EventArgs e)
        {
            //grid1.SetGrid(17, 10);
            grid1[0, 0] = "Property";
            grid1[0, 1] = "Cut1";
            grid1[0, 2] = "Cut2";
            grid1[0, 3] = "Cut3";
            grid1[0, 4] = "Cut4";
            grid1[0, 5] = "Cut5";
            grid1[0, 6] = "Cut6";
            grid1[0, 7] = "Cut7";
            grid1[0, 8] = "Cut8";
            grid1[0, 9] = "Cut9";
            grid1[0, 10] = "Cut10";

            grid1[1, 0] = "Distillation";
            grid1[2, 0] = "1%";
            grid1[3, 0] = "5%";
            grid1[4, 0] = "10%";
            grid1[5, 0] = "20%";
            grid1[6, 0] = "30%";
            grid1[7, 0] = "50%";
            grid1[8, 0] = "70%";
            grid1[9, 0] = "80%";
            grid1[10, 0] = "90%";
            grid1[11, 0] = "95%";
            grid1[12, 0] = "99%";
            grid1[13, 0] = "VF";
            grid1[14, 0] = "SG";
            grid1[15, 0] = "S";
            grid1[16, 0] = "ARO";
            grid1[17, 0] = "NAP";

            for (int c = 1; c <= plantdata.Count; c++)
            {
                for (int r = 2; r <= 12; r++)
                {
                    grid1[r, c] = ((PlantData)plantdata[c - 1]).TBP[r - 2].ToString();
                }
                grid1[13, c] = ((PlantData)plantdata[c - 1]).VF.ToString();
                grid1[14, c] = ((PlantData)plantdata[c - 1]).SG.ToString();
                grid1[15, c] = ((PlantData)plantdata[c - 1]).S.ToString();
                grid1[16, c] = ((PlantData)plantdata[c - 1]).ARO.ToString();
                grid1[17, c] = ((PlantData)plantdata[c - 1]).NAP.ToString();
            }
            for (int c = 1; c <= 10; c++)
            {
                grid1[1, c] = "TBP LV%";
            }

            grid2.ComboBoxList = new string[] { "Wt%%", "Vol%", "Mol%" };

            grid2[0, 0] = "Property";
            grid2[0, 1] = "Component 1";

            for (int r = 1; r < Pure.CompNames.Length; r++)
            {
                //grid2.Rows.Insert(r);
                grid2[r, 0] = Pure.CompNames[r].ToString();
            }

            for (int c = 1; c < 2; c++)
            {
                for (int r = 2; r < Pure.CompNames.Length; r++)
                {
                    grid2[r, c] = "0";
                }
                grid2[3, c] = "100";
            }
            for (int c = plantdata.Count + 1; c <= 10; c++)
            {
            }
        }

        public PlantDatas ReadData()
        {
            PlantDatas pd = new PlantDatas();
            for (int c = 1; c < 11; c++)
            {
                if (grid1[14, c] != "0")    // if sg not set not valid stream
                {
                    PlantData p = new PlantData(new double[11], 0, 0);
                    DistType[c] = grid1[1, c].ToString();
                    for (int r = 2; r <= 12; r++)
                    {
                        p.TBP[r - 2] = Convert.Todouble(grid1[r, c]);
                    }
                    p.VF = Convert.Todouble(grid1[13, c]);
                    p.SG = Convert.Todouble(grid1[14, c]);
                    p.S = Convert.Todouble(grid1[15, c]);
                    p.ARO = Convert.Todouble(grid1[16, c]);
                    p.NAP = Convert.Todouble(grid1[17, c]);
                    pd.add(p);
                }
            }
            return pd;
        }

        public PureComponentList ReadPureData()
        {
            PureComponentList pd = new PureComponentList();
            for (int c = 1; c < 22; c++)
            {
                string name = grid2[c, 0].ToString();
                pd.add(Pure.GetPure(name));
            }
            return pd;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QuickAssay f = new QuickAssay();
            double[] OldLV = new double[11];
            f.MinX = 0;
            f.MinY = -100;
            try
            {
                if (syn.Product.oil != null)
                {
                    syn.Product.oil.CalcCumLVPCT();
                    syn.Product.oil.CreateShortTBPCurveFromLVpct();
                    f.AddCurve("Synthesised Stream", syn.Product.oil.lv, syn.Product.oil.tbp, Color.Black);
                    double pct = 0, totflow = 0, flow = 0;
                    foreach (PlantData pd in plantdata)
                    {
                        totflow += pd.VF;
                    }
                    foreach (PlantData pd in plantdata)
                    {
                        f.AddCurve("Plantdata Stream", syn.Product.oil.lv, ((PlantData)pd).TBP, Color.Red, flow / totflow * 100, pd.VF / totflow);
                        flow += pd.VF;
                    }
                    f.Show();
                }
            }
            catch
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReadPureData();
        }

        /// <summary>
        /// The main entry point  for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new SynDLG());
        }
    }
}