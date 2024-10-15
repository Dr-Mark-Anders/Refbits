using   ModelEngine;
using   System.Windows.Forms;
using   Units.PortForm;

namespace   Units
{
    partial class  SpreadsheetDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void  Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private  void  InitializeComponent()
        {
            this.reoGridControl1 = new  unvell.ReoGrid.ReoGridControl();
            this.SuspendLayout();
            // 
            // reoGridControl1
            // 
            this.reoGridControl1.AllowDrop = true;
            this.reoGridControl1.BackColor = System.Drawing.SystemColors.Control;
            this.reoGridControl1.CellContextMenuStrip = null;
            this.reoGridControl1.ColCount = 13;
            this.reoGridControl1.ColHeadContextMenuStrip = null;
            this.reoGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControl1.Location = new  System.Drawing.Point (0, 0);
            this.reoGridControl1.Name = "reoGridControl1";
            this.reoGridControl1.RowCount = 24;
            this.reoGridControl1.RowHeadContextMenuStrip = null;
            this.reoGridControl1.Script = null;
            this.reoGridControl1.Size = new  System.Drawing.Size(939, 542);
            this.reoGridControl1.TabIndex = 0;
            this.reoGridControl1.Text = "reoGridControl1";
            this.reoGridControl1.DragDrop += new  System.Windows.Forms.DragEventHandler(this.reoGridControl1_DragDrop);
            this.reoGridControl1.DragOver += new  System.Windows.Forms.DragEventHandler(this.ReoGridControl1_DragOver);
            // 
            // SpreadsheetDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(939, 542);
            this.Controls.Add(this.reoGridControl1);
            this.Margin = new  System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "SpreadsheetDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new  System.EventHandler(this.SpreadsheetDialog_Load);
            this.DragOver += new  System.Windows.Forms.DragEventHandler(this.ReoGridControl1_DragOver);
            this.ResumeLayout(false);

        }

        #endregion

        private  unvell.ReoGrid.ReoGridControl reoGridControl1;
    }
}