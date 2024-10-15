using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units.Dialogs
{
    public partial class PumpAroundsFrm2 : Form
    {
        private DrawPACollection Pas = new DrawPACollection();
        private DrawPA Pa;
        private DrawColumn drawcolumn;

        private void PumpAroundsFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ReadGrid();

            foreach (DrawPA dpa in Pas)
            {
                //Specification s1 = cd.Specs.GetSpecByGUID(dpa.Specs[0].Guid);
                //Specification s2 = cd.Specs.GetSpecByGUID(dpa.Specs[0].Guid);
            }
        }

        public PumpAroundsFrm2()
        {
            InitializeComponent();
        }

        internal PumpAroundsFrm2(DrawColumn cd)
        {
            this.drawcolumn = cd;
            InitializeComponent();
            Pas.Add(cd.Column.PumpArounds);
            UpdateGrid();
        }

        public void UpdateGrid()
        {
            DataGridViewRow dgr;
            dataGridView1.Rows.Clear();
            int row = 0;

            Pas = drawcolumn.graphicslist.ReturnAllPumpArounds();

            DataGridViewComboBoxColumn col4 = (DataGridViewComboBoxColumn)dataGridView1.Columns[4];
            col4.DataSource = Enum.GetValues(typeof(ePASpecTypes));
            col4.ValueType = typeof(ePASpecTypes);

            DataGridViewComboBoxColumn col6 = (DataGridViewComboBoxColumn)dataGridView1.Columns[6];
            col6.DataSource = Enum.GetValues(typeof(ePASpecTypes));
            col6.ValueType = typeof(ePASpecTypes);

            for (int n = 0; n < Pas.Count; n++)
            {
                row = dataGridView1.Rows.Add();
                dgr = dataGridView1.Rows[row];
                dgr.Cells[0].Value = Pas[n].Name;
                dgr.Cells[1].Value = Pas[n].Active;
                dgr.Cells[2].Value = Pas[n].DrawTrayDrawGuid;
                dgr.Cells[3].Value = Pas[n].ReturnTrayDrawGuid;

                dgr.Cells[4].Value = Pas[n].Espec1;
                dgr.Cells[6].Value = Pas[n].Espec2;
            }
        }

        private void AddPA_Click(object sender, EventArgs e)
        {
            if (drawcolumn.Column.TraySections == null || drawcolumn.Column.TraySections.Count == 0)
                return;

            DataGridViewComboBoxColumn col4 = (DataGridViewComboBoxColumn)dataGridView1.Columns[4];
            col4.DataSource = Enum.GetValues(typeof(ePASpecTypes));
            col4.ValueType = typeof(ePASpecTypes);

            DataGridViewComboBoxColumn col6 = (DataGridViewComboBoxColumn)dataGridView1.Columns[6];
            col6.DataSource = Enum.GetValues(typeof(ePASpecTypes));
            col6.ValueType = typeof(ePASpecTypes);

            int row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].Cells[0].Value = "PA" + row.ToString();
            dataGridView1.Rows[row].Cells[1].Value = true;
            dataGridView1.Rows[row].Cells[2].Value = "2";
            dataGridView1.Rows[row].Cells[3].Value = "1";
            dataGridView1.Rows[row].Cells[4].Value = ePASpecTypes.Flow;
            dataGridView1.Rows[row].Cells[6].Value = ePASpecTypes.ReturnT;

            CreatePA();

            drawcolumn.Column.IsDirty = true;
        }

        public void CreatePA()
        {
            Pa = new DrawPA(300, 200, 50, 50);

            if (drawcolumn.Column.TraySections == null)
                return;
            DrawColumnTraySection dcts = new DrawColumnTraySection();

            Pa.Originguid = dcts.Guid;
            Pa.Destinationguid = dcts.Guid;

            Pa.ReturnTrayDrawGuid = Guid.Empty;
            Pa.DrawTrayDrawGuid = Guid.Empty;
            Pa.Active = true;

            Specification spec;

            spec = new Specification("PaSpec1", 0, ePropID.MOLEF, eSpecType.PAFlow, drawcolumn.Column[0], drawcolumn.Column[0].Trays[0], Pa.PumpAround);
            //spec.IsPumparoundSpec = true;
            spec.engineObjectguid = Pa.Guid;
            drawcolumn.Column.Specs.Add(spec);

            spec = new Specification("PaSpec2", 0, ePropID.T, eSpecType.PARetT, drawcolumn.Column[0], drawcolumn.Column[0].Trays[0], Pa.PumpAround);
            //spec.IsPumparoundSpec = true;
            spec.engineObjectguid = Pa.Guid;
            drawcolumn.Column.Specs.Add(spec);

            //CreateStreams(Pa);

            Pa.Name = "PA" + Pas.Count;
            Pas.Add(Pa);
            drawcolumn.graphicslist.Add(Pa);
        }

        private void CreateStreams(DrawPA pa)
        {
            DrawColumnTraySection tc = drawcolumn.MainSection;
            DrawTray t1 = tc.GetTray(pa.ReturnTrayDrawGuid);
            DrawTray t2 = tc.GetTray(pa.DrawTrayDrawGuid);

            if (t1 != null && t2 != null)
            {
                Node TrayPoint1 = t2.PaDraw;
                Node PAPoint1 = pa.Hotspots["FEED"];
                Node TrayPoint2 = t1.Pareturn;
                Node PAPoint2 = pa.Hotspots["PRODUCT"];

                DrawMaterialStream ds = new DrawMaterialStream(TrayPoint1, PAPoint1);
                ds.Name = pa.Name + "Draw";
                //ds.IsSolved = false;
                ds.EngineDrawTrayGuid = pa.DrawTrayDrawGuid;
                ds.EnginereturnTrayGuid = Guid.Empty;

                DrawMaterialStream ds2 = new DrawMaterialStream(PAPoint2, TrayPoint2);
                ds2.Name = pa.Name + "return  ";
                //ds2.IsSolved = false;
                ds.EngineDrawTrayGuid = Guid.Empty;
                ds2.EnginereturnTrayGuid = pa.ReturnTrayDrawGuid;

                drawcolumn.graphicslist.Add(ds);
                drawcolumn.graphicslist.Add(ds2);
            }
        }

        private void DeletePA_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            DrawPA pa;

            if (row != null && row.Index >= 0)
            {
                pa = Pas[row.Index];
                dataGridView1.Rows.Remove(row);
                deletePA(pa);
                drawcolumn.Column.Specs.RemoveByObject(pa.PumpAround.Guid);
            }
        }

        private void deletePA(DrawPA pa)
        {
            DrawMaterialStream ds1;
            DrawMaterialStream ds2;

            ds1 = drawcolumn.graphicslist.ReturnAttachedStream(pa.Hotspots["FEED"]);
            ds2 = drawcolumn.graphicslist.ReturnAttachedStream(pa.Hotspots["PRODUCT"]);
            drawcolumn.graphicslist.Remove(pa);
            drawcolumn.graphicslist.Remove(ds1);
            drawcolumn.graphicslist.Remove(ds2);
            UpdateGrid();
        }

        private void deletePAStreams(DrawPA pa)
        {
            DrawMaterialStream ds1;
            DrawMaterialStream ds2;

            ds1 = drawcolumn.graphicslist.ReturnAttachedStream(pa.Hotspots["FEED"]);
            ds2 = drawcolumn.graphicslist.ReturnAttachedStream(pa.Hotspots["PRODUCT"]);

            drawcolumn.graphicslist.Remove(ds1);
            drawcolumn.graphicslist.Remove(ds2);
        }

        public void ReadGrid()
        {
            string Name;
            Guid DrawTray = Guid.Empty, returnTray = Guid.Empty;
            bool isactive = true;
            DataGridViewRow dgvr;
            Tray tray = null;

            for (int n = 0; n < dataGridView1.RowCount; n++)
            {
                dgvr = dataGridView1.Rows[n];
                Name = dgvr.Cells[0].Value.ToString();
                if (dgvr.Cells[1].Value != null)
                {
                    isactive = Convert.ToBoolean(dgvr.Cells[1].Value);
                }
                if (dgvr.Cells[2].Value != null)
                {
                    if (int.TryParse(dgvr.Cells[2].Value.ToString(), out int TrayNo))
                        tray = drawcolumn.Column.TraySections[0].Trays[TrayNo];
                    if (tray != null)
                        DrawTray = tray.Guid;
                }
                if (dgvr.Cells[3].Value != null)
                {
                    if (int.TryParse(dgvr.Cells[3].Value.ToString(), out int TrayNo))
                        tray = drawcolumn.Column.TraySections[0].Trays[TrayNo];
                    if (tray != null)
                        returnTray = tray.Guid;
                }
                if (dgvr.Cells[4].Value != null)
                {
                    if (Pas.Count > n)
                        Pas[n].Espec1 = (ePASpecTypes)dgvr.Cells[4].Value;
                    //Pas[n].Espec1 = (ePASpecTypes)dgvr.Cells[4].Value;
                }
                if (dgvr.Cells[6].Value != null)
                {
                    if (Pas.Count > n)
                        Pas[n].Espec2 = (ePASpecTypes)dgvr.Cells[6].Value;
                    //Pas[n].Espec2 = (ePASpecTypes)dgvr.Cells[6].Value;
                }

                dataGridView1.Refresh();

                if (Pas.Count > n)
                {
                    Pas[n].Name = Name;
                    Pas[n].Active = isactive;
                    Pas[n].DrawTrayDrawGuid = DrawTray;
                    Pas[n].ReturnTrayDrawGuid = returnTray;
                    drawcolumn.Column.Specs.SetTrays(DrawTray, returnTray);

                    // Pas[n].UpdatePAStreams(cd.ColumnGraphicsList);

                    deletePAStreams(Pas[n]);
                    CreateStreams(Pas[n]);

                    checkifstreamactive(Pas[n], n);
                }
            }
        }

        internal void checkifstreamactive(DrawPA pa, int row)
        {
            DrawMaterialStream ds1, ds2;

            ds1 = drawcolumn.graphicslist.ReturnAttachedStream(pa.Hotspots["FEED"]);
            ds2 = drawcolumn.graphicslist.ReturnAttachedStream(pa.Hotspots["PRODUCT"]);
            bool isactive = Convert.ToBoolean(dataGridView1.Rows[row].Cells[1].Value);

            if (isactive)
            {
                if (ds1 != null)
                    ds1.Active = true;
                if (ds2 != null)
                    ds2.Active = true;
            }
            else
            {
                if (ds1 != null)
                    ds1.Active = false;
                if (ds2 != null)
                    ds2.Active = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}