namespace   Units
{
    partial class  TableForm
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
            this.tableControl1 = new  Units.TableControl();
            this.SuspendLayout();
            // 
            // tableControl1
            // 
            this.tableControl1.BackColor = System.Drawing.SystemColors.Info;
            this.tableControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableControl1.Location = new  System.Drawing.Point (0, 0);
            this.tableControl1.Name = "tableControl1";
            this.tableControl1.Size = new  System.Drawing.Size(800, 105);
            this.tableControl1.TabIndex = 0;
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new  System.Drawing.Size(800, 256);
            this.Controls.Add(this.tableControl1);
            this.Name = "TableForm";
            this.Text = "TableForm";
            this.ResumeLayout(false);

        }

        #endregion

        private  TableControl tableControl1;
    }
}