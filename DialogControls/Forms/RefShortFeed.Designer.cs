namespace   DialogControls
{
    partial class  RefShortFeed
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
            this.ponAdata1 = new  DialogControls.PONAdata();
            this.btnViewComponents = new  System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ponAdata1
            // 
            this.ponAdata1.Basis = enumMassMolarOrVol.Molar;
            this.ponAdata1.Location = new  System.Drawing.Point (13, 12);
            this.ponAdata1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ponAdata1.Name = "ponAdata1";
            this.ponAdata1.Size = new  System.Drawing.Size(777, 393);
            this.ponAdata1.TabIndex = 0;
            // 
            // btnViewComponents
            // 
            this.btnViewComponents.Location = new  System.Drawing.Point (25, 161);
            this.btnViewComponents.Name = "btnViewComponents";
            this.btnViewComponents.Size = new  System.Drawing.Size(75, 23);
            this.btnViewComponents.TabIndex = 3;
            this.btnViewComponents.Text = "View Components";
            this.btnViewComponents.UseVisualStyleBackColor = true;
            this.btnViewComponents.Click += new  System.EventHandler(this.btnViewComponents_Click);
            // 
            // RefShortFeed
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(803, 417);
            this.Controls.Add(this.btnViewComponents);
            this.Controls.Add(this.ponAdata1);
            this.Name = "RefShortFeed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Component Data";
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.FullGC_FormClosing);
            this.Load += new  System.EventHandler(this.FullGC_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private  PONAdata ponAdata1;
        private  System.Windows.Forms.Button btnViewComponents;
    }
}