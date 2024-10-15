namespace   UOMGrid
{
    partial class  TestForm
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
            this.grid1 = new  UOMGrid.Grid();
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.Location = new  System.Drawing.Point (76, 42);
            this.grid1.Name = "grid1";
            this.grid1.Size = new  System.Drawing.Size(729, 424);
            this.grid1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(904, 518);
            this.Controls.Add(this.grid1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private  Grid grid1;
    }
}