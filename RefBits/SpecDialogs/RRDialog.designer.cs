using System.Windows.Forms;

namespace Units
{
    partial class RRDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
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
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSection = new System.Windows.Forms.ComboBox();
            this.cbStage = new System.Windows.Forms.ComboBox();
            this.cbSpec = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Section";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Stage";
            // 
            // cbSection
            // 
            this.cbSection.FormattingEnabled = true;
            this.cbSection.Location = new System.Drawing.Point(117, 44);
            this.cbSection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbSection.Name = "cbSection";
            this.cbSection.Size = new System.Drawing.Size(118, 23);
            this.cbSection.TabIndex = 2;
            this.cbSection.Text = "0";
            this.cbSection.SelectedIndexChanged += new System.EventHandler(this.cbSection_SelectedIndexChanged);
            // 
            // cbStage
            // 
            this.cbStage.FormattingEnabled = true;
            this.cbStage.Location = new System.Drawing.Point(117, 75);
            this.cbStage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbStage.Name = "cbStage";
            this.cbStage.Size = new System.Drawing.Size(118, 23);
            this.cbStage.TabIndex = 3;
            this.cbStage.Text = "0";
            this.cbStage.SelectedIndexChanged += new System.EventHandler(this.cbStage_SelectedIndexChanged);
            // 
            // cbSpec
            // 
            this.cbSpec.FormattingEnabled = true;
            this.cbSpec.Location = new System.Drawing.Point(117, 14);
            this.cbSpec.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbSpec.Name = "cbSpec";
            this.cbSpec.Size = new System.Drawing.Size(118, 23);
            this.cbSpec.TabIndex = 5;
            this.cbSpec.Text = "0";
            this.cbSpec.SelectedIndexChanged += new System.EventHandler(this.cbSpec_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Spec";
            // 
            // RRDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 118);
            this.Controls.Add(this.cbSpec);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbStage);
            this.Controls.Add(this.cbSection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RRDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reflux Ratio";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrayDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private ComboBox cbSection;
        private ComboBox cbStage;
        private ComboBox cbSpec;
        private Label label3;
    }
}