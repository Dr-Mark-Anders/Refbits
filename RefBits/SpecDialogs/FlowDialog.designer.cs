using   System.Windows.Forms;

namespace   Units
{
    partial class  FlowDialog
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
            this.cbDraw = new  System.Windows.Forms.ComboBox();
            this.cbSpec = new  System.Windows.Forms.ComboBox();
            this.label3 = new  System.Windows.Forms.Label();
            this.cbFlowType = new  System.Windows.Forms.ComboBox();
            this.label1 = new  System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new  System.Drawing.Point (40, 25);
            this.label2.Name = "label2";
            this.label2.Size = new  System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Draw";
            // 
            // cbDraw
            // 
            this.cbDraw.FormattingEnabled = true;
            this.cbDraw.Location = new  System.Drawing.Point (108, 17);
            this.cbDraw.Name = "cbDraw";
            this.cbDraw.Size = new  System.Drawing.Size(102, 21);
            this.cbDraw.TabIndex = 3;
            this.cbDraw.Text = "0";
            this.cbDraw.SelectedIndexChanged += new  System.EventHandler(this.cbDraw_SelectedIndexChanged);
            // 
            // cbSpec
            // 
            this.cbSpec.FormattingEnabled = true;
            this.cbSpec.Location = new  System.Drawing.Point (108, 44);
            this.cbSpec.Name = "cbSpec";
            this.cbSpec.Size = new  System.Drawing.Size(102, 21);
            this.cbSpec.TabIndex = 5;
            this.cbSpec.Text = "0";
            this.cbSpec.SelectedIndexChanged += new  System.EventHandler(this.cbSpec_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new  System.Drawing.Point (40, 48);
            this.label3.Name = "label3";
            this.label3.Size = new  System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Value";
            // 
            // cbFlowType
            // 
            this.cbFlowType.FormattingEnabled = true;
            this.cbFlowType.Location = new  System.Drawing.Point (108, 71);
            this.cbFlowType.Name = "cbFlowType";
            this.cbFlowType.Size = new  System.Drawing.Size(102, 21);
            this.cbFlowType.TabIndex = 7;
            this.cbFlowType.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new  System.Drawing.Point (40, 73);
            this.label1.Name = "label1";
            this.label1.Size = new  System.Drawing.Size(56, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Flow Type";
            // 
            // FlowDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(243, 118);
            this.Controls.Add(this.cbFlowType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbSpec);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbDraw);
            this.Controls.Add(this.label2);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "FlowDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spec Location";
            this.TopMost = true;
            this.FormClosing += new  System.Windows.Forms.FormClosingEventHandler(this.FlowDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private  Label label2;
        private  ComboBox cbDraw;
        private  ComboBox cbSpec;
        private  Label label3;
        private  ComboBox cbFlowType;
        private  Label label1;
    }
}