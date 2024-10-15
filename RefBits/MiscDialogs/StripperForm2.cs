using ModelEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units.Dialogs
{
    using DrawList = List<DrawObject>;

    public partial class StripperFrm2 : Form
    {
        private readonly DrawColumn drawcolumn;
        private DrawColumnTraySectionCollection Strippers = new();
        private DrawColumnTraySection St;

        private void StripperFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ReadGrid();
        }

        public StripperFrm2()
        {
            InitializeComponent();
        }

        internal StripperFrm2(DrawColumn column)
        {
            this.drawcolumn = column;
            InitializeComponent();
            for (int i = 1; i < drawcolumn.Column.TraySections.Count; i++)
                Strippers.AddRange(drawcolumn.Column.TraySections[i]);

            UpdateGrid();
        }

        public void UpdateGrid()
        {
            dataGridView1.Rows.Clear();
            Strippers = drawcolumn.graphicslist.ReturnAllStrippers();

            DataGridViewComboBoxColumn col5 = (DataGridViewComboBoxColumn)dataGridView1.Columns[5];
            col5.DataSource = Enum.GetValues(typeof(eStripperType));
            col5.ValueType = typeof(eStripperType);

            for (int n = 0; n < Strippers.Count; n++)
            {
                int row = dataGridView1.Rows.Add();
                DataGridViewRow dgr = dataGridView1.Rows[row];
                dgr.Cells[0].Value = Strippers[n].Name;
                dgr.Cells[1].Value = Strippers[n].Active;
                dgr.Cells[2].Value = Strippers[n].DrawTrays.Count;
                dgr.Cells[3].Value = Strippers[n].StripperDrawTray;
                dgr.Cells[4].Value = Strippers[n].StripperReturnTray;
                dgr.Cells[5].Value = Strippers[n].Striptype;
            }
        }

        private void AddSt_Click(object sender, EventArgs e)
        {
            if (drawcolumn.Column.TraySections == null || drawcolumn.Column.TraySections.Count == 0)
                return;
            int row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].Cells[0].Value = "Stripper" + row.ToString();
            dataGridView1.Rows[row].Cells[1].Value = true;
            dataGridView1.Rows[row].Cells[2].Value = "3";
            dataGridView1.Rows[row].Cells[3].Value = "7";
            dataGridView1.Rows[row].Cells[4].Value = "6";

            dataGridView1.Rows[row].Cells[5].Value = eStripperType.Stripped;

            CreateStripper();

            drawcolumn.Isdirty = true;
        }

        public void CreateStripper()
        {
            St = new(1000, 300, 40, 50, 10);
            St.Striptype = eStripperType.None;
            St.Reboilertype = ReboilerType.None;
            drawcolumn.MainSection.CondenserType = CondType.None;

            CreateStreams(St, 6, 5); // must do first

            drawcolumn.graphicslist.SetStripperDrawGUIDs(); // attach streams to point ers on column section
            drawcolumn.graphicslist.SetStripperTopFeedGUIDs();

            Specification spec1 = new FlowSpec("Stripper Draw Flow", double.NaN, ePropID.MF, eSpecType.TrayNetLiqFlow)
            {
                Value = (MoleFlow)0.1
            };
            drawcolumn.Column.Specs.Add(spec1);

            St.Name = "Stripper" + Strippers.Count;
            Strippers.Add(St);
            drawcolumn.graphicslist.Add(St);
        }

        private void CreateStreams(DrawColumnTraySection St, int DrawTray, int returnTray)
        {
            if (drawcolumn.Column.TraySections != null)
            {
                DrawTray rettray = St[returnTray];
                DrawTray drawtray = St[DrawTray];

                if (rettray != null && drawtray != null)
                {
                    Node TrayPoint1 = drawtray.LiquidDrawRight;
                    Node PAPoint1 = St.DrawTrays[0].FeedLeft;
                    Node TrayPoint2 = rettray.FeedRight;
                    Node PAPoint2 = St.Hotspots["VAPPRODUCT"];

                    DrawMaterialStream feed = new(TrayPoint1, PAPoint1); // from column to stripper
                    //feed.IsSolved = false;
                    feed.EnginereturnTrayGuid = Guid.Empty;
                    St.StripFeedGuid = feed.Guid;
                    St.StripperTopFeed = feed;

                    DrawMaterialStream vap = new(PAPoint2, TrayPoint2); // from stripper to column
                   // vap.IsSolved = false;
                    vap.EngineDrawTrayGuid = Guid.Empty;
                    St.StripVapourGuid = vap.Guid;
                    St.StripperVapourProduct = vap;

                    drawcolumn.graphicslist.Add(feed);
                    drawcolumn.graphicslist.Add(vap);

                    int x = St.Rectangle.Right;
                    int y = St.Rectangle.Bottom;

                    Node BotProd = St.Hotspots["BOTPRODUCT"];
                    Node BotProd2 = new(drawcolumn.DrawArea, x + 20, y + 20, "Floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.ColDrawArea);

                    DrawMaterialStream ds3 = new(BotProd, BotProd2);
                    //ds3.IsSolved = false;
                    drawcolumn.graphicslist.Add(ds3);

                    x = St.Rectangle.Right;
                    y = St.Rectangle.Bottom;

                    BotProd = St.BottomTray.FeedRight;
                    BotProd2 = new(drawcolumn.DrawArea, x + 20, y, "Floating", NodeDirections.Left, HotSpotType.Floating, HotSpotOwnerType.ColDrawArea);

                    ds3 = new(BotProd, BotProd2);
                  //  ds3.IsSolved = false;
                    drawcolumn.graphicslist.Add(ds3);
                }
            }
        }

        private void DeleteSt_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            DrawColumnTraySection St;

            if (row != null && row.Index >= 0)
            {
                St = Strippers[row.Index];
                dataGridView1.Rows.Remove(row);
                deleteSt(St);
                drawcolumn.Column.Specs.RemoveByObject(St.Guid);
            }
        }

        private void deleteSt(DrawColumnTraySection st)
        {
            DrawList list;

            list = drawcolumn.graphicslist.ReturnStreams(st);

            foreach (DrawMaterialStream ds in list)
                drawcolumn.graphicslist.Remove(ds);

            drawcolumn.graphicslist.Remove(st);

            UpdateGrid();
        }

        private void deleteStStreams(DrawColumnTraySection st)
        {
            DrawMaterialStream ds1, ds2, ds3, ds4;

            ds1 = drawcolumn.graphicslist.ReturnAttachedStream(st.Hotspots["VAPPRODUCT"]);
            ds2 = drawcolumn.graphicslist.ReturnAttachedStream(st.Hotspots["BOTPRODUCT"]);
            ds3 = drawcolumn.graphicslist.ReturnAttachedStream(st.TopTray.FeedLeft);
            ds4 = drawcolumn.graphicslist.ReturnAttachedStream(st.BottomTray.FeedRight);
            drawcolumn.graphicslist.Remove(ds1);
            drawcolumn.graphicslist.Remove(ds2);
            drawcolumn.graphicslist.Remove(ds3);
            drawcolumn.graphicslist.Remove(ds4);
        }

        private void UpdateStStreams(DrawColumnTraySection st)
        {
            if (drawcolumn.Column.TraySections != null)
            {
                int rettray, drawtray;
                rettray = st.GetTrayNo(st.StripperReturnTray);
                drawtray = st.GetTrayNo(st.StripperDrawTray) - 1;
                DrawTray t1 = st[rettray]; // trays zero based
                DrawTray t2 = st[drawtray];
                if (t1 != null && t2 != null)
                {
                    Node TrayPoint1 = t2.LiquidDrawRight;
                    Node TrayPoint2 = t1.FeedRight;

                    DrawMaterialStream ds = st.StripperTopFeed; // from column to stripper
                    //ds.IsSolved = false;
                    ds.StartNodeGuid = TrayPoint1.Guid;

                    DrawMaterialStream ds2 = st.StripperVapourProduct; // from stripper to column
                   // ds2.IsSolved = false;
                    ds2.EndNodeGuid = TrayPoint2.Guid;
                }
            }
        }

        public void ReadGrid()
        {
            string Name;
            Guid DrawTray = Guid.Empty, returnTray = Guid.Empty;
            int NoTrays = 3;
            bool isactive = true;
            DataGridViewRow dgvr;

            for (int n = 0; n < dataGridView1.RowCount; n++)
            {
                dgvr = dataGridView1.Rows[n];
                Name = dgvr.Cells[0].Value.ToString();
                if (dgvr.Cells[1].Value != null)
                {
                    isactive = Convert.ToBoolean(dgvr.Cells[1].Value);
                }
                if (dgvr.Cells[2].Value != null)
                    NoTrays = Convert.ToInt16(dgvr.Cells[2].Value);

                if (dgvr.Cells[3].Value != null)
                    DrawTray = drawcolumn.Column.TraySections[0].Trays[Convert.ToInt16(dgvr.Cells[3].Value)].Guid;

                if (dgvr.Cells[4].Value != null)
                    returnTray = drawcolumn.Column.TraySections[0].Trays[Convert.ToInt16(dgvr.Cells[4].Value)].Guid;

                Strippers[n].Name = Name;
                Strippers[n].Active = isactive;
                Strippers[n].StripperDrawTray = DrawTray;
                Strippers[n].StripperReturnTray = returnTray;
                Strippers[n].SetNoTrays(NoTrays);

                Strippers[n].Striptype = (eStripperType)dgvr.Cells[5].Value;

                UpdateStStreams(Strippers[n]);

                checkifstreamactive(Strippers[n], n);
            }
        }

        internal void checkifstreamactive(DrawColumnTraySection st, int row)
        {
            DrawMaterialStream ds1, ds2, ds3, ds4;

            ds1 = drawcolumn.graphicslist.ReturnAttachedStream(st.Hotspots["VAPPRODUCT"]);
            ds2 = drawcolumn.graphicslist.ReturnAttachedStream(st.Hotspots["BOTPRODUCT"]);
            ds3 = drawcolumn.graphicslist.ReturnAttachedStream(st.TopTray.FeedLeft);
            ds4 = drawcolumn.graphicslist.ReturnAttachedStream(st.BottomTray.FeedRight);

            bool isactive = Convert.ToBoolean(dataGridView1.Rows[row].Cells[1].Value);

            if (isactive)
            {
                if (ds1 != null)
                    ds1.Active = true;
                if (ds2 != null)
                    ds2.Active = true;
                if (ds3 != null)
                    ds3.Active = true;
                if (ds4 != null)
                    ds4.Active = true;
            }
            else
            {
                if (ds1 != null)
                    ds1.Active = false;
                if (ds2 != null)
                    ds2.Active = false;
                if (ds3 != null)
                    ds3.Active = false;
                if (ds4 != null)
                    ds4.Active = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ReadGrid();
        }
    }
}