namespace   Units
{
    partial class  MessagesForm
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
            this.rtMessageBox = new  System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtMessageBox
            // 
            this.rtMessageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtMessageBox.Location = new  System.Drawing.Point (0, 0);
            this.rtMessageBox.Margin = new  System.Windows.Forms.Padding(2);
            this.rtMessageBox.Name = "rtMessageBox";
            this.rtMessageBox.ShortcutsEnabled = false;
            this.rtMessageBox.Size = new  System.Drawing.Size(800, 175);
            this.rtMessageBox.TabIndex = 17;
            this.rtMessageBox.Text = "";
            // 
            // MessagesForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(800, 175);
            this.Controls.Add(this.rtMessageBox);
            this.Name = "MessagesForm";
            this.Text = "Messages";
            this.ResumeLayout(false);

        }

        #endregion

        public  System.Windows.Forms.RichTextBox rtMessageBox;
    }
}