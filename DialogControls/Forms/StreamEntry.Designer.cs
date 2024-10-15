namespace   DialogControls
{
    partial class  StreamEntry
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
            this.streamCompositionEntry1 = new  DialogControls.StreamCompositionEntry();
            this.SuspendLayout();
            // 
            // streamCompositionEntry1
            // 
            this.streamCompositionEntry1.Location = new  System.Drawing.Point (34, 6);
            this.streamCompositionEntry1.Name = "streamCompositionEntry1";
            this.streamCompositionEntry1.Size = new  System.Drawing.Size(622, 432);
            this.streamCompositionEntry1.TabIndex = 0;
            // 
            // StreamEntry
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(688, 450);
            this.Controls.Add(this.streamCompositionEntry1);
            this.Name = "StreamEntry";
            this.Text = "StreamEntry";
            this.ResumeLayout(false);

        }

        #endregion

        private  StreamCompositionEntry streamCompositionEntry1;
    }
}