using   System.Windows.Forms;

namespace   Units
{
    partial class  StreamDistDialog
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
            this.label3 = new  System.Windows.Forms.Label();
            this.label1 = new  System.Windows.Forms.Label();
            this.label2 = new  System.Windows.Forms.Label();
            this.cbDraw = new  System.Windows.Forms.ComboBox();
            this.cbDistType = new  System.Windows.Forms.ComboBox();
            this.cbDistPoint  = new  System.Windows.Forms.ComboBox();
            this.SuspendLayout();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new  System.Drawing.Point (60, 64);
            this.label1.Name = "label1";
            this.label1.Size = new  System.Drawing.Size(79, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Distilation Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new  System.Drawing.Point (60, 91);
            this.label2.Name = "label2";
            this.label2.Size = new  System.Drawing.Size(79, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Distilation Point ";
            // 
            // cbDraw
            // 
            this.cbDraw.FormattingEnabled = true;
            this.cbDraw.Location = new  System.Drawing.Point (154, 29);
            this.cbDraw.Name = "cbDraw";
            this.cbDraw.Size = new  System.Drawing.Size(102, 21);
            this.cbDraw.TabIndex = 5;
            this.cbDraw.Text = "0";
            // 
            // cbDistType
            // 
            this.cbDistType.FormattingEnabled = true;
            this.cbDistType.Location = new  System.Drawing.Point (154, 56);
            this.cbDistType.Name = "cbDistType";
            this.cbDistType.Size = new  System.Drawing.Size(102, 21);
            this.cbDistType.TabIndex = 7;
            this.cbDistType.Text = "0";
            // 
            // cbDistPoint 
            // 
            this.cbDistPoint .FormattingEnabled = true;
            this.cbDistPoint .Location = new  System.Drawing.Point (154, 83);
            this.cbDistPoint .Name = "cbDistPoint ";
            this.cbDistPoint .Size = new  System.Drawing.Size(102, 21);
            this.cbDistPoint .TabIndex = 8;
            this.cbDistPoint .Text = "0";
            // 
            // StreamDistDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(342, 130);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbDistPoint );
            this.Controls.Add(this.cbDistType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDraw);
            this.Controls.Add(this.label3);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "StreamDistDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stream Flow";
            this.TopMost = true;
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.FlowDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private  Label label3;
        private  Label label1;
        private  Label label2;
        private  ComboBox cbDraw;
        private  ComboBox cbDistType;
        private  ComboBox cbDistPoint ;
    }
}