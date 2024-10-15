namespace   RefbitsProperties
{
    partial class  PropsForm:Form
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
            this.portProperty = new  Units.PortForm.PortPropertyWorksheet();
            this.btnComponents = new  System.Windows.Forms.Button();
            this.button1 = new  System.Windows.Forms.Button();
            this.button2 = new  System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // portProperty
            // 
            this.portProperty.Location = new  System.Drawing.Point (11, 53);
            this.portProperty.Margin = new  System.Windows.Forms.Padding(2);
            this.portProperty.Name = "portProperty";
            this.portProperty.Simplify = false;
            this.portProperty.Size = new  System.Drawing.Size(901, 477);
            this.portProperty.TabIndex = 2;
            this.portProperty.Load += new  System.EventHandler(this.portPropertyWorksheet1_Load);
            // 
            // btnComponents
            // 
            this.btnComponents.Location = new  System.Drawing.Point (39, 21);
            this.btnComponents.Name = "btnComponents";
            this.btnComponents.Size = new  System.Drawing.Size(145, 27);
            this.btnComponents.TabIndex = 3;
            this.btnComponents.Text = "Component Selection";
            this.btnComponents.UseVisualStyleBackColor = true;
            this.btnComponents.Click += new  System.EventHandler(this.btnComponents_Click);
            // 
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (190, 21);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(94, 27);
            this.button1.TabIndex = 4;
            this.button1.Text = "Save Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new  System.EventHandler(this.btnSave_Click);
            // 
            // button2
            // 
            this.button2.Location = new  System.Drawing.Point (290, 21);
            this.button2.Name = "button2";
            this.button2.Size = new  System.Drawing.Size(94, 27);
            this.button2.TabIndex = 5;
            this.button2.Text = "Load Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new  System.EventHandler(this.btnLoadData_Click);
            // 
            // PropsForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(918, 541);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnComponents);
            this.Controls.Add(this.portProperty);
            this.Name = "PropsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Refbits Properties";
            this.Load += new  System.EventHandler(this.SteamPropsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private  Units.PortForm.PortPropertyWorksheet portProperty;
        private  Button btnComponents;
        private  Button button1;
        private  Button button2;
    }
}