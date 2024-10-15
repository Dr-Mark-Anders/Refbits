using   System.Windows.Forms;

namespace   Units
{
    partial class  ProductDrawDialog
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
            this.cbDraw = new  System.Windows.Forms.ComboBox();
            this.label3 = new  System.Windows.Forms.Label();
            this.groupBox1 = new  System.Windows.Forms.GroupBox();
            this.rbVolume = new  System.Windows.Forms.RadioButton();
            this.rbMass = new  System.Windows.Forms.RadioButton();
            this.rbMole = new  System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbDraw
            // 
            this.cbDraw.FormattingEnabled = true;
            this.cbDraw.Location = new  System.Drawing.Point (144, 34);
            this.cbDraw.Name = "cbDraw";
            this.cbDraw.Size = new  System.Drawing.Size(102, 21);
            this.cbDraw.TabIndex = 5;
            this.cbDraw.Text = "0";
            this.cbDraw.SelectedIndexChanged += new  System.EventHandler(this.cbDraw_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new  System.Drawing.Point (25, 38);
            this.label3.Name = "label3";
            this.label3.Size = new  System.Drawing.Size(103, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Liquid Product Draw";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbVolume);
            this.groupBox1.Controls.Add(this.rbMass);
            this.groupBox1.Controls.Add(this.rbMole);
            this.groupBox1.Location = new  System.Drawing.Point (28, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new  System.Drawing.Size(200, 92);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flow Type";
            // 
            // rbVolume
            // 
            this.rbVolume.AutoSize = true;
            this.rbVolume.Location = new  System.Drawing.Point (41, 66);
            this.rbVolume.Name = "rbVolume";
            this.rbVolume.Size = new  System.Drawing.Size(106, 17);
            this.rbVolume.TabIndex = 2;
            this.rbVolume.Text = "Standard Volume";
            this.rbVolume.UseVisualStyleBackColor = true;
            this.rbVolume.CheckedChanged += new  System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // rbMass
            // 
            this.rbMass.AutoSize = true;
            this.rbMass.Location = new  System.Drawing.Point (41, 43);
            this.rbMass.Name = "rbMass";
            this.rbMass.Size = new  System.Drawing.Size(50, 17);
            this.rbMass.TabIndex = 1;
            this.rbMass.Text = "Mass";
            this.rbMass.UseVisualStyleBackColor = true;
            this.rbMass.CheckedChanged += new  System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbMole
            // 
            this.rbMole.AutoSize = true;
            this.rbMole.Checked = true;
            this.rbMole.Location = new  System.Drawing.Point (41, 20);
            this.rbMole.Name = "rbMole";
            this.rbMole.Size = new  System.Drawing.Size(48, 17);
            this.rbMole.TabIndex = 0;
            this.rbMole.TabStop = true;
            this.rbMole.Text = "Mole";
            this.rbMole.UseVisualStyleBackColor = true;
            this.rbMole.CheckedChanged += new  System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // ProductDrawDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(387, 177);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbDraw);
            this.Controls.Add(this.label3);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "ProductDrawDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Draw";
            this.TopMost = true;
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.FlowDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private  ComboBox cbDraw;
        private  Label label3;
        private  GroupBox groupBox1;
        private  RadioButton rbVolume;
        private  RadioButton rbMass;
        private  RadioButton rbMole;
    }
}