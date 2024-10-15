
namespace   DialogControls
{
    partial class  MassMolarVol
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
            this.gb = new  System.Windows.Forms.GroupBox();
            this.rbVol = new  System.Windows.Forms.RadioButton();
            this.rbMolar = new  System.Windows.Forms.RadioButton();
            this.rbMass = new  System.Windows.Forms.RadioButton();
            this.gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox10
            // 
            this.gb.Controls.Add(this.rbVol);
            this.gb.Controls.Add(this.rbMolar);
            this.gb.Controls.Add(this.rbMass);
            this.gb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb.Location = new  System.Drawing.Point (0, 0);
            this.gb.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gb.Name = "groupBox10";
            this.gb.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gb.Size = new  System.Drawing.Size(201, 114);
            this.gb.TabIndex = 4;
            this.gb.TabStop = false;
            this.gb.Text = "Flow Type";
            // 
            // rbVol
            // 
            this.rbVol.AutoSize = true;
            this.rbVol.Location = new  System.Drawing.Point (26, 77);
            this.rbVol.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbVol.Name = "rbVol";
            this.rbVol.Size = new  System.Drawing.Size(65, 19);
            this.rbVol.TabIndex = 2;
            this.rbVol.TabStop = true;
            this.rbVol.Text = "Volume";
            this.rbVol.UseVisualStyleBackColor = true;
            this.rbVol.CheckedChanged += new  System.EventHandler(this.rbVol_CheckedChanged);
            // 
            // rbMolar
            // 
            this.rbMolar.AutoSize = true;
            this.rbMolar.Location = new  System.Drawing.Point (26, 51);
            this.rbMolar.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbMolar.Name = "rbMolar";
            this.rbMolar.Size = new  System.Drawing.Size(56, 19);
            this.rbMolar.TabIndex = 1;
            this.rbMolar.TabStop = true;
            this.rbMolar.Text = "Molar";
            this.rbMolar.UseVisualStyleBackColor = true;
            this.rbMolar.CheckedChanged += new  System.EventHandler(this.rbMolar_CheckedChanged);
            // 
            // rbMass
            // 
            this.rbMass.AutoSize = true;
            this.rbMass.Location = new  System.Drawing.Point (26, 24);
            this.rbMass.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbMass.Name = "rbMass";
            this.rbMass.Size = new  System.Drawing.Size(52, 19);
            this.rbMass.TabIndex = 0;
            this.rbMass.TabStop = true;
            this.rbMass.Text = "Mass";
            this.rbMass.UseVisualStyleBackColor = true;
            this.rbMass.CheckedChanged += new  System.EventHandler(this.rbMass_CheckedChanged);
            // 
            // MassMolarVol
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MassMolarVol";
            this.Size = new  System.Drawing.Size(201, 114);
            this.MouseClick += new  System.Windows.Forms.MouseEventHandler(this.MassMolarVol_MouseClick);
            this.gb.ResumeLayout(false);
            this.gb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.GroupBox gb;
        private  System.Windows.Forms.RadioButton rbVol;
        private  System.Windows.Forms.RadioButton rbMolar;
        private  System.Windows.Forms.RadioButton rbMass;
    }
}
