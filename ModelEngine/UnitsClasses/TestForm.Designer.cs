namespace   Units.Unitsclasses
{
    partial class  TestForm
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
            this.dTextBox1 = new  Units.DimTextBox();
            this.SuspendLayout();
            // 
            // dTextBox1
            // 
            this.dTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.dTextBox1.Ddbxleft = 60;
            this.dTextBox1.Ddbxwidth = 60;
            this.dTextBox1.DropDownVisible = true;
            this.dTextBox1.Location = new  System.Drawing.Point (39, 39);
            this.dTextBox1.Margin = new  System.Windows.Forms.Padding(2);
            this.dTextBox1.Name = "dTextBox1";
            this.dTextBox1.Precision = 4;
            this.dTextBox1.ReadOnly = false;
            this.dTextBox1.Size = new  System.Drawing.Size(229, 23);
            this.dTextBox1.TabIndex = 0;
            this.dTextBox1.Textbxwidth = 60;
            this.dTextBox1.TextWidth = 60;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(338, 190);
            this.Controls.Add(this.dTextBox1);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private  DimTextBox dTextBox1;
    }
}