namespace   DialogControls
{
    partial class  ViewCompProps
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected  override  void   Dispose(bool  disposing)
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
        private  void   InitializeComponent()
        {
            this.gbData = new  System.Windows.Forms.GroupBox();
            this.pg1 = new  FormControls.GCCompGrid();
            this.GCBasis = new  DialogControls.MassMolarVol();
            this.gbData.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbData
            // 
            this.gbData.Controls.Add(this.pg1);
            this.gbData.Location = new  System.Drawing.Point (198, 12);
            this.gbData.Name = "gbData";
            this.gbData.Size = new  System.Drawing.Size(281, 523);
            this.gbData.TabIndex = 22;
            this.gbData.TabStop = false;
            this.gbData.Text = "GC Data";
            // 
            // pg1
            // 
            this.pg1.DisplayTitles = true;
            this.pg1.FirstColumnWidth = 64;
            this.pg1.Location = new  System.Drawing.Point (20, 22);
            this.pg1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pg1.Name = "pg1";
            this.pg1.Size = new  System.Drawing.Size(239, 479);
            this.pg1.TabIndex = 0;
            // 
            // GCBasis
            // 
            this.GCBasis.Location = new  System.Drawing.Point (13, 12);
            this.GCBasis.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GCBasis.Name = "GCBasis";
            this.GCBasis.Size = new  System.Drawing.Size(157, 114);
            this.GCBasis.TabIndex = 23;
            this.GCBasis.Value = enumMassMolarOrVol.Molar;
            // 
            // FullGC
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(498, 553);
            this.Controls.Add(this.GCBasis);
            this.Controls.Add(this.gbData);
            this.Name = "FullGC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Component Data";
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.FullGC_FormClosing);
            this.Load += new  System.EventHandler(this.FullGC_Load);
            this.gbData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.GroupBox gbData;
        private  FormControls.GCCompGrid pg1;
        private  MassMolarVol GCBasis;
    }
}