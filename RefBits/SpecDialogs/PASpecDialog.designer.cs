using System.Windows.Forms;

namespace Units
{
    partial class  PASpecDialog
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
            this.label2 = new  System.Windows.Forms.Label();
            this.cbSpec = new  System.Windows.Forms.ComboBox();
            this.cbName = new  System.Windows.Forms.ComboBox();
            this.label3 = new  System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new  System.Drawing.Point (37, 45);
            this.label2.Margin = new  System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new  System.Drawing.Size(32, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Spec";
            // 
            // cbSpec
            // 
            this.cbSpec.FormattingEnabled = true;
            this.cbSpec.Location = new  System.Drawing.Point (135, 42);
            this.cbSpec.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbSpec.Name = "cbSpec";
            this.cbSpec.Size = new  System.Drawing.Size(118, 23);
            this.cbSpec.TabIndex = 3;
            this.cbSpec.Text = "0";
            this.cbSpec.SelectedIndexChanged += new  System.EventHandler(this.cbSpec_SelectedIndexChanged);
            // 
            // cbName
            // 
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new  System.Drawing.Point (135, 10);
            this.cbName.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbName.Name = "cbName";
            this.cbName.Size = new  System.Drawing.Size(118, 23);
            this.cbName.TabIndex = 5;
            this.cbName.Text = "0";
            this.cbName.SelectedIndexChanged += new  System.EventHandler(this.cbName_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new  System.Drawing.Point (37, 14);
            this.label3.Margin = new  System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new  System.Drawing.Size(77, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pumparound";
            // 
            // PASpecDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(306, 91);
            this.Controls.Add(this.cbName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSpec);
            this.Controls.Add(this.label2);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "PASpecDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spec Location";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label label2;
        private ComboBox cbSpec;
        private ComboBox cbName;
        private Label label3;
    }
}