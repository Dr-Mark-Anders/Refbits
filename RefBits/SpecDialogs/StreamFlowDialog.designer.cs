using   System.Windows.Forms;

namespace   Units
{
    partial class  StreamFlowDialog
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
            this.SuspendLayout();
            // 
            // cbDraw
            // 
            this.cbDraw.FormattingEnabled = true;
            this.cbDraw.Location = new  System.Drawing.Point (130, 34);
            this.cbDraw.Name = "cbDraw";
            this.cbDraw.Size = new  System.Drawing.Size(102, 21);
            this.cbDraw.TabIndex = 5;
            this.cbDraw.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new  System.Drawing.Point (59, 37);
            this.label3.Name = "label3";
            this.label3.Size = new  System.Drawing.Size(40, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Stream";
            // 
            // StreamFlowDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(294, 97);
            this.Controls.Add(this.cbDraw);
            this.Controls.Add(this.label3);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "StreamFlowDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stream Flow";
            this.TopMost = true;
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.FlowDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private  ComboBox cbDraw;
        private  Label label3;
    }
}