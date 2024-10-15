using   ModelEngine;
using   System.ComponentModel;
using   System.Windows.Forms;

namespace Units
{
    partial class DrawArea
    {

        //private  BackgroundWorker backgroundworker;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuDrawArea = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doNothingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStream = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuObject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuDrawArea.SuspendLayout();
            this.contextMenuStream.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 534);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(1059, 17);
            this.hScrollBar1.TabIndex = 5;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(1042, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 534);
            this.vScrollBar1.TabIndex = 6;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // contextMenuDrawArea
            // 
            this.contextMenuDrawArea.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.doNothingToolStripMenuItem});
            this.contextMenuDrawArea.Name = "contextMenuDrawArea";
            this.contextMenuDrawArea.Size = new System.Drawing.Size(137, 70);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // doNothingToolStripMenuItem
            // 
            this.doNothingToolStripMenuItem.Name = "doNothingToolStripMenuItem";
            this.doNothingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.doNothingToolStripMenuItem.Text = "Do Nothing";
            // 
            // contextMenuStream
            // 
            this.contextMenuStream.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPointsToolStripMenuItem,
            this.removePointToolStripMenuItem});
            this.contextMenuStream.Name = "contextMenuStream";
            this.contextMenuStream.Size = new System.Drawing.Size(149, 48);
            // 
            // addPoint sToolStripMenuItem
            // 
            this.addPointsToolStripMenuItem.Name = "addPoint sToolStripMenuItem";
            this.addPointsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.addPointsToolStripMenuItem.Text = "Add Point ";
            // 
            // removePoint ToolStripMenuItem
            // 
            this.removePointToolStripMenuItem.Name = "removePoint ToolStripMenuItem";
            this.removePointToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.removePointToolStripMenuItem.Text = "Remove Point ";
            // 
            // contextMenuObject
            // 
            this.contextMenuObject.Name = "contextMenuObject";
            this.contextMenuObject.Size = new System.Drawing.Size(61, 4);
            // 
            // DrawArea
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1059, 551);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.ContextMenuStrip = this.contextMenuDrawArea;
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MdiChildrenMinimizedAnchorBottom = false;
            this.MinimizeBox = false;
            this.Name = "DrawArea";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DrawArea_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DrawArea_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DrawArea_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.DrawArea_DragOver);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawArea_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DrawArea_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MousedoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseUp);
            this.contextMenuDrawArea.ResumeLayout(false);
            this.contextMenuStream.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private HScrollBar hScrollBar1;
        private VScrollBar vScrollBar1;
        public ToolTip toolTip1;
        private System.ComponentModel.IContainer Components;
        private ContextMenuStrip contextMenuDrawArea;
        private ContextMenuStrip contextMenuStream;
        private ToolStripMenuItem addPointsToolStripMenuItem;
        private ToolStripMenuItem removePointToolStripMenuItem;
        private ContextMenuStrip contextMenuObject;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem doNothingToolStripMenuItem;
        private IContainer components;
    }
}
