using   System.Windows.Forms;

namespace   Units
{
    partial class  PAFlowSpecDialog
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
            this.label2 = new  System.Windows.Forms.Label();
            this.cbSpec = new  System.Windows.Forms.ComboBox();
            this.cbName = new  System.Windows.Forms.ComboBox();
            this.label3 = new  System.Windows.Forms.Label();
            this.groupBox1 = new  System.Windows.Forms.GroupBox();
            this.rbVol = new  System.Windows.Forms.RadioButton();
            this.rbMass = new  System.Windows.Forms.RadioButton();
            this.rbMole = new  System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new  System.Drawing.Point (32, 39);
            this.label2.Name = "label2";
            this.label2.Size = new  System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Spec";
            // 
            // cbSpec
            // 
            this.cbSpec.FormattingEnabled = true;
            this.cbSpec.Location = new  System.Drawing.Point (116, 36);
            this.cbSpec.Name = "cbSpec";
            this.cbSpec.Size = new  System.Drawing.Size(102, 21);
            this.cbSpec.TabIndex = 3;
            this.cbSpec.Text = "0";
            this.cbSpec.SelectedIndexChanged += new  System.EventHandler(this.cbSpec_SelectedIndexChanged);
            // 
            // cbName
            // 
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new  System.Drawing.Point (116, 9);
            this.cbName.Name = "cbName";
            this.cbName.Size = new  System.Drawing.Size(102, 21);
            this.cbName.TabIndex = 5;
            this.cbName.Text = "0";
            this.cbName.SelectedIndexChanged += new  System.EventHandler(this.cbName_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new  System.Drawing.Point (32, 12);
            this.label3.Name = "label3";
            this.label3.Size = new  System.Drawing.Size(67, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pumparound";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbVol);
            this.groupBox1.Controls.Add(this.rbMass);
            this.groupBox1.Controls.Add(this.rbMole);
            this.groupBox1.Location = new  System.Drawing.Point (35, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new  System.Drawing.Size(200, 92);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flow Type";
            // 
            // rbVol
            // 
            this.rbVol.AutoSize = true;
            this.rbVol.Location = new  System.Drawing.Point (41, 66);
            this.rbVol.Name = "rbVol";
            this.rbVol.Size = new  System.Drawing.Size(106, 17);
            this.rbVol.TabIndex = 2;
            this.rbVol.Text = "Standard Volume";
            this.rbVol.UseVisualStyleBackColor = true;
            this.rbVol.CheckedChanged += new  System.EventHandler(this.radioButton3_CheckedChanged);
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
            // PASpecDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(263, 181);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSpec);
            this.Controls.Add(this.label2);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "PASpecDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spec Location";
            this.TopMost = true;
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.PASpecDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private  Label label2;
        private  ComboBox cbSpec;
        private  ComboBox cbName;
        private  Label label3;
        private  GroupBox groupBox1;
        private  RadioButton rbVol;
        private  RadioButton rbMass;
        private  RadioButton rbMole;
    }
}