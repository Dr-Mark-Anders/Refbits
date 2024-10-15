
namespace   DialogControls
{
    partial class  StreamAnalysysType
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
            this.rbBasic = new  System.Windows.Forms.RadioButton();
            this.rbFull = new  System.Windows.Forms.RadioButton();
            this.rbMedium = new  System.Windows.Forms.RadioButton();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.rbBasic);
            this.groupBox10.Controls.Add(this.rbFull);
            this.groupBox10.Controls.Add(this.rbMedium);
            this.groupBox10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox10.Location = new  System.Drawing.Point (0, 0);
            this.groupBox10.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox10.Size = new  System.Drawing.Size(163, 104);
            this.groupBox10.TabIndex = 4;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Feed Analysis Type";
            // 
            // rbBasic
            // 
            this.rbBasic.AutoSize = true;
            this.rbBasic.Checked = true;
            this.rbBasic.Location = new  System.Drawing.Point (19, 22);
            this.rbBasic.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbBasic.Name = "rbBasic";
            this.rbBasic.Size = new  System.Drawing.Size(86, 19);
            this.rbBasic.TabIndex = 5;
            this.rbBasic.TabStop = true;
            this.rbBasic.Text = "Distillations";
            this.rbBasic.UseVisualStyleBackColor = true;
            this.rbBasic.CheckedChanged += new  System.EventHandler(this.rbBasic_CheckedChanged);
            // 
            // rbFull
            // 
            this.rbFull.AutoSize = true;
            this.rbFull.Location = new  System.Drawing.Point (19, 72);
            this.rbFull.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbFull.Name = "rbFull";
            this.rbFull.Size = new  System.Drawing.Size(63, 19);
            this.rbFull.TabIndex = 4;
            this.rbFull.Text = "Full GC";
            this.rbFull.UseVisualStyleBackColor = true;
            this.rbFull.CheckedChanged += new  System.EventHandler(this.rbFull_CheckedChanged);
            // 
            // rbMedium
            // 
            this.rbMedium.AutoSize = true;
            this.rbMedium.Location = new  System.Drawing.Point (19, 47);
            this.rbMedium.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbMedium.Name = "rbMedium";
            this.rbMedium.Size = new  System.Drawing.Size(72, 19);
            this.rbMedium.TabIndex = 3;
            this.rbMedium.Text = "Short GC";
            this.rbMedium.UseVisualStyleBackColor = true;
            this.rbMedium.CheckedChanged += new  System.EventHandler(this.rbMedium_CheckedChanged);
            // 
            // ShortMediumFull
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox10);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ShortMediumFull";
            this.Size = new  System.Drawing.Size(163, 104);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.GroupBox groupBox10;
        private  System.Windows.Forms.RadioButton rbFull;
        private  System.Windows.Forms.RadioButton rbMedium;
        private  System.Windows.Forms.RadioButton rbBasic;
    }
}
