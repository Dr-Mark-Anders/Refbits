using System.Windows.Forms;

namespace Units
{
    partial class PALocationDialog
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
            this.label1 = new  System.Windows.Forms.Label();
            this.label2 = new  System.Windows.Forms.Label();
            this.cbSection = new  System.Windows.Forms.ComboBox();
            this.cbStage = new  System.Windows.Forms.ComboBox();
            this.cbreturnTray = new  System.Windows.Forms.ComboBox();
            this.CBreturnSection = new  System.Windows.Forms.ComboBox();
            this.label3 = new  System.Windows.Forms.Label();
            this.label4 = new  System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new  System.Drawing.Point (38, 56);
            this.label1.Name = "label1";
            this.label1.Size = new  System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Section";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new  System.Drawing.Point (38, 83);
            this.label2.Name = "label2";
            this.label2.Size = new  System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Stage";
            // 
            // cbSection
            // 
            this.cbSection.FormattingEnabled = true;
            this.cbSection.Location = new  System.Drawing.Point (106, 53);
            this.cbSection.Name = "cbSection";
            this.cbSection.Size = new  System.Drawing.Size(102, 21);
            this.cbSection.TabIndex = 2;
            this.cbSection.Text = "0";
            this.cbSection.SelectedIndexChanged += new  System.EventHandler(this.cbSection_SelectedIndexChanged);
            // 
            // cbStage
            // 
            this.cbStage.FormattingEnabled = true;
            this.cbStage.Location = new  System.Drawing.Point (106, 83);
            this.cbStage.Name = "cbStage";
            this.cbStage.Size = new  System.Drawing.Size(102, 21);
            this.cbStage.TabIndex = 3;
            this.cbStage.Text = "0";
            // 
            // cbreturn  Tray
            // 
            this.cbreturnTray.FormattingEnabled = true;
            this.cbreturnTray.Location = new  System.Drawing.Point (230, 83);
            this.cbreturnTray.Name = "cbreturnTray";
            this.cbreturnTray.Size = new  System.Drawing.Size(102, 21);
            this.cbreturnTray.TabIndex = 5;
            this.cbreturnTray.Text = "0";
            // 
            // CBreturn  Section
            // 
            this.CBreturnSection.FormattingEnabled = true;
            this.CBreturnSection.Location = new  System.Drawing.Point (230, 53);
            this.CBreturnSection.Name = "CBreturn  Section";
            this.CBreturnSection.Size = new  System.Drawing.Size(102, 21);
            this.CBreturnSection.TabIndex = 4;
            this.CBreturnSection.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new  System.Drawing.Point (103, 22);
            this.label3.Name = "label3";
            this.label3.Size = new  System.Drawing.Size(32, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Draw";
            this.label3.Click += new  System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new  System.Drawing.Point (227, 22);
            this.label4.Name = "label4";
            this.label4.Size = new  System.Drawing.Size(39, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "return  ";
            this.label4.Click += new  System.EventHandler(this.label4_Click);
            // 
            // PADialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(422, 153);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbreturnTray);
            this.Controls.Add(this.CBreturnSection);
            this.Controls.Add(this.cbStage);
            this.Controls.Add(this.cbSection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new  System.Windows.Forms.Padding(2);
            this.Name = "PADialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pumparound Location";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private  Label label1;
        private  Label label2;
        private  ComboBox cbSection;
        private  ComboBox cbStage;
        private  ComboBox cbreturnTray;
        private  ComboBox CBreturnSection;
        private  Label label3;
        private  Label label4;
    }
}