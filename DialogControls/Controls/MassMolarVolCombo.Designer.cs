
namespace   DialogControls
{
    partial class  MassMolarVolCombo
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
            this.comboBox1 = new  System.Windows.Forms.ComboBox();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.comboBox1);
            this.groupBox10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox10.Location = new  System.Drawing.Point (0, 0);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new  System.Drawing.Size(154, 71);
            this.groupBox10.TabIndex = 4;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Flow Type";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new  System.Drawing.Point (16, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new  System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new  System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // MassMolarVolCombo
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox10);
            this.Name = "MassMolarVolCombo";
            this.Size = new  System.Drawing.Size(154, 71);
            this.MouseClick += new  System.Windows.Forms.MouseEventHandler(this.MassMolarVol_MouseClick);
            this.groupBox10.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.GroupBox groupBox10;
        private  System.Windows.Forms.ComboBox comboBox1;
    }
}
