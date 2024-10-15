
namespace   DialogControls
{
    partial class  DistillationBasis
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private  void   InitializeComponent()
        {
            this.groupBox10 = new  System.Windows.Forms.GroupBox();
            this.TBPWT = new  System.Windows.Forms.RadioButton();
            this.D2887 = new  System.Windows.Forms.RadioButton();
            this.D1160 = new  System.Windows.Forms.RadioButton();
            this.D86 = new  System.Windows.Forms.RadioButton();
            this.TBPVOL = new  System.Windows.Forms.RadioButton();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.TBPWT);
            this.groupBox10.Controls.Add(this.D2887);
            this.groupBox10.Controls.Add(this.D1160);
            this.groupBox10.Controls.Add(this.D86);
            this.groupBox10.Controls.Add(this.TBPVOL);
            this.groupBox10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox10.Location = new  System.Drawing.Point (0, 0);
            this.groupBox10.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox10.Size = new  System.Drawing.Size(172, 156);
            this.groupBox10.TabIndex = 4;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Assay Basis";
            // 
            // TBPWT
            // 
            this.TBPWT.AutoSize = true;
            this.TBPWT.Location = new  System.Drawing.Point (17, 122);
            this.TBPWT.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TBPWT.Name = "TBPWT";
            this.TBPWT.Size = new  System.Drawing.Size(65, 19);
            this.TBPWT.TabIndex = 25;
            this.TBPWT.Text = "TBP WT";
            this.TBPWT.UseVisualStyleBackColor = true;
            this.TBPWT.CheckedChanged += new  System.EventHandler(this.TBPWT_CheckedChanged);
            // 
            // D2887
            // 
            this.D2887.AutoSize = true;
            this.D2887.Location = new  System.Drawing.Point (17, 72);
            this.D2887.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.D2887.Name = "D2887";
            this.D2887.Size = new  System.Drawing.Size(57, 19);
            this.D2887.TabIndex = 24;
            this.D2887.Text = "D2887";
            this.D2887.UseVisualStyleBackColor = true;
            // 
            // D1160
            // 
            this.D1160.AutoSize = true;
            this.D1160.Location = new  System.Drawing.Point (17, 48);
            this.D1160.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.D1160.Name = "D1160";
            this.D1160.Size = new  System.Drawing.Size(57, 19);
            this.D1160.TabIndex = 23;
            this.D1160.Text = "D1160";
            this.D1160.UseVisualStyleBackColor = true;
            // 
            // D86
            // 
            this.D86.AutoSize = true;
            this.D86.Cursor = System.Windows.Forms.Cursors.Default;
            this.D86.Location = new  System.Drawing.Point (17, 23);
            this.D86.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.D86.Name = "D86";
            this.D86.Size = new  System.Drawing.Size(45, 19);
            this.D86.TabIndex = 22;
            this.D86.Text = "D86";
            this.D86.UseVisualStyleBackColor = true;
            // 
            // TBPVOL
            // 
            this.TBPVOL.AutoSize = true;
            this.TBPVOL.Location = new  System.Drawing.Point (17, 97);
            this.TBPVOL.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TBPVOL.Name = "TBPVOL";
            this.TBPVOL.Size = new  System.Drawing.Size(70, 19);
            this.TBPVOL.TabIndex = 21;
            this.TBPVOL.Text = "TBP VOL";
            this.TBPVOL.UseVisualStyleBackColor = true;
            // 
            // DistillationBasis
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox10);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DistillationBasis";
            this.Size = new  System.Drawing.Size(172, 156);
            this.MouseClick += new  System.Windows.Forms.MouseEventHandler(this.MassMolarVol_MouseClick);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.GroupBox groupBox10;
        private  System.Windows.Forms.RadioButton D2887;
        private  System.Windows.Forms.RadioButton D1160;
        private  System.Windows.Forms.RadioButton D86;
        private  System.Windows.Forms.RadioButton TBPVOL;
        private  System.Windows.Forms.RadioButton TBPWT;
    }
}
