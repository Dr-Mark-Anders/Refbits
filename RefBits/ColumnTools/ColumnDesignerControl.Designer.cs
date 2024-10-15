using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    partial class ColumnDesignerControl
    {
        ///<summary>
        ///Cleanupanyresourcesbeingused.
        ///</summary>
        ///<paramname="disposing">trueifmanagedresourcesshouldbedisposed;otherwise,false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region WindowsFormDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>
        private void InitializeComponent()
        {
            this.Components = new System.ComponentModel.Container();
            this.ColumnContextMenu = new System.Windows.Forms.ContextMenuStrip(this.Components);
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.addTraysAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTraysBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.add1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMultipleTraysBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addCondenserToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addReboilerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addPAToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mwnu1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.streamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.ColumnContextMenu.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            //ColumnContextMenu
            //
            this.ColumnContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[]{
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.addTraysAboveToolStripMenuItem,
            this.addTraysBelowToolStripMenuItem,
            this.add1ToolStripMenuItem,
            this.addMultipleTraysBelowToolStripMenuItem,
            this.toolStripSeparator2,
            this.addCondenserToolStripMenuItem1,
            this.addReboilerToolStripMenuItem1,
            this.addPAToolStripMenuItem1,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem});
            this.ColumnContextMenu.Name = "contextMenuStrip3";
            this.ColumnContextMenu.Size = new System.Drawing.Size(210, 236);
            //
            //toolStripMenuItem6
            //
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(209, 22);
            this.toolStripMenuItem6.Text = "DeleteTray";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.RemoveTrayToolStripMenuItem_Click);
            //
            //toolStripMenuItem7
            //
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(209, 22);
            this.toolStripMenuItem7.Text = "DeleteMultipleTrays";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.DeleteMultipleTraysToolStripMenuItem_Click);
            //
            //addTraysAboveToolStripMenuItem
            //
            this.addTraysAboveToolStripMenuItem.Name = "addTraysAboveToolStripMenuItem";
            this.addTraysAboveToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.addTraysAboveToolStripMenuItem.Text = "AddTrayAbove";
            this.addTraysAboveToolStripMenuItem.Click += new System.EventHandler(this.AddTrayToolStripMenuItem_Click);
            //
            //addTraysBelowToolStripMenuItem
            //
            this.addTraysBelowToolStripMenuItem.Name = "addTraysBelowToolStripMenuItem";
            this.addTraysBelowToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.addTraysBelowToolStripMenuItem.Text = "AddTrayBelow";
            this.addTraysBelowToolStripMenuItem.Click += new System.EventHandler(this.AddTrayBelowToolStripMenuItem_Click);
            //
            //add1ToolStripMenuItem
            //
            this.add1ToolStripMenuItem.Name = "add1ToolStripMenuItem";
            this.add1ToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.add1ToolStripMenuItem.Text = "AddMultipleTraysAbove";
            this.add1ToolStripMenuItem.Click += new System.EventHandler(this.AddMultipleTraysAboveToolStripMenuItem_Click);
            //
            //addMultipleTraysBelowToolStripMenuItem
            //
            this.addMultipleTraysBelowToolStripMenuItem.Name = "addMultipleTraysBelowToolStripMenuItem";
            this.addMultipleTraysBelowToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.addMultipleTraysBelowToolStripMenuItem.Text = "AddMultipleTraysBelow";
            this.addMultipleTraysBelowToolStripMenuItem.Click += new System.EventHandler(this.AddMultpleTraysBelowToolStripMenuItem_Click);
            //
            //toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(206, 6);
            //
            //addCondenserToolStripMenuItem1
            //
            this.addCondenserToolStripMenuItem1.Name = "addCondenserToolStripMenuItem1";
            this.addCondenserToolStripMenuItem1.Size = new System.Drawing.Size(209, 22);
            this.addCondenserToolStripMenuItem1.Text = "AddCondenser";
            this.addCondenserToolStripMenuItem1.Click += new System.EventHandler(this.AddCondenser_Click);
            //
            //addReboilerToolStripMenuItem1
            //
            this.addReboilerToolStripMenuItem1.Name = "addReboilerToolStripMenuItem1";
            this.addReboilerToolStripMenuItem1.Size = new System.Drawing.Size(209, 22);
            this.addReboilerToolStripMenuItem1.Text = "AddReboiler";
            this.addReboilerToolStripMenuItem1.Click += new System.EventHandler(this.AddReboiler_Click);
            //
            //addPAToolStripMenuItem1
            //
            this.addPAToolStripMenuItem1.Name = "addPAToolStripMenuItem1";
            this.addPAToolStripMenuItem1.Size = new System.Drawing.Size(209, 22);
            this.addPAToolStripMenuItem1.Text = "AddPA";
            this.addPAToolStripMenuItem1.Click += new System.EventHandler(this.AddPA_Click);
            //
            //toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(206, 6);
            //
            //deleteToolStripMenuItem
            //
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            //
            //menuStrip1
            //
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]{
this.mwnu1ToolStripMenuItem,
this.menu1ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(856, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            //
            //mwnu1ToolStripMenuItem
            //
            this.mwnu1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]{
this.toolStripMenuItem2,
this.toolStripMenuItem3,
this.toolStripMenuItem4,
this.toolStripMenuItem5,
this.streamToolStripMenuItem});
            this.mwnu1ToolStripMenuItem.Name = "mwnu1ToolStripMenuItem";
            this.mwnu1ToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
            this.mwnu1ToolStripMenuItem.Text = "ColumnStructure";
            //
            //toolStripMenuItem2
            //
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem2.Text = "Condenser";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2_Click_1);
            //
            //toolStripMenuItem3
            //
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem3.Text = "Reboiler";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.ToolStripMenuItem3_Click);
            //
            //toolStripMenuItem4
            //
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem4.Text = "PumpAournd";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.ToolStripMenuItem4_Click);
            //
            //toolStripMenuItem5
            //
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem5.Text = "TraySection";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            //
            //streamToolStripMenuItem
            //
            this.streamToolStripMenuItem.Name = "streamToolStripMenuItem";
            this.streamToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.streamToolStripMenuItem.Text = "Stream";
            this.streamToolStripMenuItem.Click += new System.EventHandler(this.StreamToolStripMenuItem_Click_1);
            //
            //menu1ToolStripMenuItem
            //
            this.menu1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]{
this.addToolboxToolStripMenuItem,
this.removeToolboxToolStripMenuItem});
            this.menu1ToolStripMenuItem.Name = "menu1ToolStripMenuItem";
            this.menu1ToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.menu1ToolStripMenuItem.Text = "Toolbox";
            //
            //addToolboxToolStripMenuItem
            //
            this.addToolboxToolStripMenuItem.Name = "addToolboxToolStripMenuItem";
            this.addToolboxToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.addToolboxToolStripMenuItem.Text = "AddToolbox";
            this.addToolboxToolStripMenuItem.Click += new System.EventHandler(this.AddToolboxToolStripMenuItem_Click);
            //
            //removeToolboxToolStripMenuItem
            //
            this.removeToolboxToolStripMenuItem.Name = "removeToolboxToolStripMenuItem";
            this.removeToolboxToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.removeToolboxToolStripMenuItem.Text = "RemoveToolbox";
            this.removeToolboxToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolboxToolStripMenuItem_Click);
            //
            //dockPanel1
            //
            this.dockPanel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 24);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(856, 441);
            this.dockPanel1.TabIndex = 2;
            this.dockPanel1.ActiveContentChanged += new System.EventHandler(this.dockPanel1_ActiveContentChanged);
            //
            //propertyGrid1
            //
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.Location = new System.Drawing.Point(591, 24);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(265, 441);
            this.propertyGrid1.TabIndex = 3;
            //
            //ColumnDesignerControl
            //
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ColumnDesignerControl";
            this.Size = new System.Drawing.Size(856, 465);
            this.Load += new System.EventHandler(this.ColumnDesignerControl_Load);
            this.ColumnContextMenu.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.IContainer Components;
        private ContextMenuStrip ColumnContextMenu;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem addTraysAboveToolStripMenuItem;
        private ToolStripMenuItem addTraysBelowToolStripMenuItem;
        private ToolStripMenuItem add1ToolStripMenuItem;
        private ToolStripMenuItem addMultipleTraysBelowToolStripMenuItem;
        private ToolStripMenuItem addCondenserToolStripMenuItem1;
        private ToolStripMenuItem addReboilerToolStripMenuItem1;
        private ToolStripMenuItem addPAToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mwnu1ToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem menu1ToolStripMenuItem;
        private ToolStripMenuItem streamToolStripMenuItem;
        private ToolStripMenuItem addToolboxToolStripMenuItem;
        private ToolStripMenuItem removeToolboxToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private PropertyGrid propertyGrid1;


    }
}