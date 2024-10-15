namespace   RefbitsProperties
{
    partial class  SteamPropsForm:Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private  void  InitializeComponent()
        {
            this.groupBox1 = new  System.Windows.Forms.GroupBox();
            this.rb1997 = new  System.Windows.Forms.RadioButton();
            this.rb1967 = new  System.Windows.Forms.RadioButton();
            this.portProperty = new  Units.PortForm.PortPropertyWorksheet();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb1997);
            this.groupBox1.Controls.Add(this.rb1967);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new  System.Drawing.Point (25, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new  System.Drawing.Size(87, 83);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Method";
            // 
            // rb1997
            // 
            this.rb1997.AutoSize = true;
            this.rb1997.Checked = true;
            this.rb1997.Location = new  System.Drawing.Point (16, 47);
            this.rb1997.Name = "rb1997";
            this.rb1997.Size = new  System.Drawing.Size(49, 19);
            this.rb1997.TabIndex = 3;
            this.rb1997.TabStop = true;
            this.rb1997.Text = "1997";
            this.rb1997.UseVisualStyleBackColor = true;
            // 
            // rb1967
            // 
            this.rb1967.AutoSize = true;
            this.rb1967.Location = new  System.Drawing.Point (16, 22);
            this.rb1967.Name = "rb1967";
            this.rb1967.Size = new  System.Drawing.Size(49, 19);
            this.rb1967.TabIndex = 2;
            this.rb1967.Text = "1967";
            this.rb1967.UseVisualStyleBackColor = true;
            // 
            // portProperty
            // 
            this.portProperty.DrawMaterialStream = null;
            this.portProperty.Location = new  System.Drawing.Point (11, 11);
            this.portProperty.Margin = new  System.Windows.Forms.Padding(2);
            this.portProperty.Name = "portProperty";
            this.portProperty.Simplify = true;
            this.portProperty.Size = new  System.Drawing.Size(901, 477);
            this.portProperty.TabIndex = 2;
            this.portProperty.Load += new  System.EventHandler(this.portPropertyWorksheet1_Load);
            // 
            // SteamPropsForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(918, 492);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.portProperty);
            this.Name = "SteamPropsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SteamProperties";
            this.TopMost = true;
            this.Load += new  System.EventHandler(this.SteamPropsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private  GroupBox groupBox1;
        private  RadioButton rb1997;
        private  RadioButton rb1967;
        private  Units.PortForm.PortPropertyWorksheet portProperty;
    }
}