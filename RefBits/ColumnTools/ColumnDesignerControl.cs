using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Units
{
    public partial class ColumnDesignerControl : UserControl
    {
        private readonly Column column;
        private DrawArea drawArea1;
        private DrawTray t = null;
        private DrawColumnTraySection dc = null;

        internal ColumnDesignerControl(ColumnDLG columndlg)
        {
            InitializeComponent();
            column = columndlg.drawColumn.Column;
            dockPanel1.Theme = new VS2015BlueTheme();
            drawArea1 = columndlg.drawColumn.DrawArea;
            drawArea1.Show(dockPanel1, DockState.Document);
            drawArea1.ContextMenuStrip = ColumnContextMenu;
        }

        public ColumnDesignerControl()
        {
            InitializeComponent();
            dockPanel1.Theme = new VS2015BlueTheme();
            drawArea1 = new DrawArea("ColumnDesignerControlDrawArea");
            drawArea1.Show(dockPanel1, DockState.Document);
            drawArea1.ContextMenuStrip = ColumnContextMenu;
#if DEBUG
            propertyGrid1.Visible = true;
            propertyGrid1.SelectedObject = drawArea1;
#else
            propertyGrid1.Visible = false;
#endif
        }

        internal PropertyGrid PropertyGrid1 { get => propertyGrid1; set => propertyGrid1 = value; }

        public GraphicsList GraphicsList { get => drawArea1.GraphicsList; set => drawArea1.GraphicsList = value; }

        public DrawArea DrawArea1
        { get => drawArea1; set => drawArea1 = value; }

        internal void Initialize(ColumnDLG column)
        {
            drawArea1.Initialize(column);
        }

        internal void Initialize(LLEDLG column)
        {
            drawArea1.Initialize(column);
        }

        internal void DrawArea1_MouseDown(object sender, DrawMouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);

            DrawObject drawObject = drawArea1.returnObject(e.ModXY);

            switch (drawObject)
            {
                case DrawColumnTraySection dc:
                    foreach (DrawTray tt in dc.AllDrawColumnStages)
                        tt.Colour = Color.Black;
                    break;
            }

            drawArea1.Refresh();

            if (e.Button == MouseButtons.Right)
                switch (drawObject)
                {
                    case DrawColumnTraySection dc:
                        this.dc = dc;
                        t = dc.GetTray(p);
                        if (t != null)
                            t.Colour = Color.Green;
                        break;
                }

            drawArea1.Refresh();
            if (e.Button == MouseButtons.Right)
                ColumnContextMenu.Show(e.X, e.Y);
        }

        private void AddTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int loc = dc.DrawTrays.IndexOf(t);

            dc.Insert(loc, new(dc));

            dc.Hotspots.Add(new DrawTray().FeedLeft);
            dc.Hotspots.Add(new DrawTray().FeedRight);
            dc.Hotspots.Add(new DrawTray().LiquidDrawLeft);
            dc.Hotspots.Add(new DrawTray().LiquidDrawRight);
            dc.Hotspots.Add(new DrawTray().VapourDraw);
            if (column != null)
                column.SolutionConverged = false;//needtoresetfornextrun

            UpdateStripperConnections(dc);
        }

        private void RemoveTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (t != null && dc != null)
            {
                dc.Remove(t);
                if (column != null)
                    column.SolutionConverged = false;
                UpdateStripperConnections(dc);
            }
        }

        private void AddTrayBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (t != null)
            {
                dc.Insert(dc.IndexOf(t) + 1, new(dc));
                if (column != null)
                    column.SolutionConverged = false;
                UpdateStripperConnections(dc);
            }
        }

        private void AddMultipleTraysAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int NoTrays = 0;
            new InputBox().Show("EnternoofTrays", "EnternoofTrays", ref NoTrays);

            if (t != null)
            {
                for (int i = 0; i < NoTrays; i++)
                {
                    dc.Insert(dc.IndexOf(t), new DrawTray(dc));
                }
            }
            UpdateStripperConnections(dc);
            if (column != null)
                column.SolutionConverged = false;
        }

        private void AddMultpleTraysBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox box = new();
            int NoTrays = 0;
            int loc = dc.IndexOf(t);
            box.Show("EnternoofTrays", "EnternoofTrays", ref NoTrays);

            if (t != null)
            {
                for (int i = 0; i < NoTrays; i++)
                {
                    DrawTray TT = new(dc);
                    dc.Insert(loc + 1, TT);
                }
            }
            UpdateStripperConnections(dc);
            if (column != null)
                column.SolutionConverged = false;
        }

        private void DeleteMultipleTraysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox box = new();
            int NoTrays = 0;
            if (dc is null)
                return;
            int loc = dc.IndexOf(t);
            box.Show("EnternoofTrays", "EnternoofTrays", ref NoTrays);

            if (t != null)
            {
                for (int i = 0; i < NoTrays; i++)
                    if (loc > 0 && i > 0 && i < dc.TraySection.Trays.Count)
                        dc.DrawTrays.RemoveAt(loc);
            }

            UpdateStripperConnections(dc);
            if (column != null)
                column.SolutionConverged = false;
        }

        public void UpdateStripperConnections(DrawColumnTraySection dcts)
        {
            List<DrawBaseStream> streams = drawArea1.GraphicsList.ReturnStreams();
            foreach (DrawMaterialStream ds in streams)
            {
                DrawTray tray;
                if (ds.EndDrawObjectGuid == dcts.Guid)
                {
                    tray = dcts.GetTray(ds.EndNodeGuid);
                    if (tray != null)
                        ds.EnginereturnTrayGuid = tray.Guid;
                }

                if (ds.StartDrawObjectGuid == dcts.Guid)
                {
                    tray = dcts.GetTray(ds.StartNodeGuid);
                    if (tray != null)
                        ds.EngineDrawTrayGuid = tray.Guid;
                }
            }
        }

        private void AddStripper_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Stripper;
        }

        private void AddTrays_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Trays;
        }

        private void AddPA_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.PumpAround;
        }

        private void AddCondenser_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Condenser;
        }

        private void AddReboiler_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Reboiler;
        }

        private void DrawArea1_Paint(object sender, PaintEventArgs e)
        {
            //drawArea1.p
        }

        private void ToolStripButton10_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.PumpAround;
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea1.DeleteSelection();
            drawArea1.Refresh();
        }

        private void ToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Condenser;
        }

        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.PumpAround;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Trays;
        }

        private void StreamToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Stream;
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Reboiler;
        }

        private void AddToolboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea1.ShowColumnToolBox();
        }

        private void RemoveToolboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea1.HideColumnToolBox();
        }

        private void ColumnDesignerControl_Load(object sender, EventArgs e)
        {
            //ResizeDrawArea();
            drawArea1.ResetDrawArea();
            drawArea1.Initialize(this);
        }

        public ContextMenuStrip MenuStrip
        {
            get
            {
                return ColumnContextMenu;
            }
        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
        }
    }
}