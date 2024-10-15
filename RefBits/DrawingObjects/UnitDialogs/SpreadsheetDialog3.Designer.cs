using   UOMGrid;

namespace   Units
{
    partial class  SpreadsheetDialog3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer components = null;

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
            this.Components = new  System.ComponentModel.Container();
            this.grid1 = new  UOMGrid.Grid();
            this.toolTip1 = new  System.Windows.Forms.ToolTip(this.Components);
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.AllowDrop = true;
            this.grid1.Cols = 10;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Location = new  System.Drawing.Point (0, 0);
            this.grid1.Name = "grid1";
            this.grid1.Rows = 20;
            this.grid1.Size = new  System.Drawing.Size(939, 542);
            this.grid1.TabIndex = 0;
            this.grid1.DragOver += new  System.Windows.Forms.DragEventHandler(this.grid1_DragOver);
            // 
            // SpreadsheetDialog3
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(939, 542);
            this.Controls.Add(this.grid1);
            this.Margin = new  System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "SpreadsheetDialog3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetDialog3_FormClosing);
            this.Load += new  System.EventHandler(this.SpreadsheetDialog_Load);
            this.DragDrop += new  System.Windows.Forms.DragEventHandler(this.GridControl_DragOver);
            this.DragOver += new  System.Windows.Forms.DragEventHandler(this.GridControl_DragOver);
            this.ResumeLayout(false);

        }

        #endregion

        private  UOMGrid.Grid grid1;
        private  System.Windows.Forms.ToolTip toolTip1;
        private  System.ComponentModel.IContainer Components;
    }
}