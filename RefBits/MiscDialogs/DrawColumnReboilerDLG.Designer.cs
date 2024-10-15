using System.Windows.Forms;

namespace Units
{
    partial class ColumnDrawreboilerDLG
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
            ReboilerTypeCombo = new ComboBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // ReboilerTypeCombo
            // 
            ReboilerTypeCombo.FormattingEnabled = true;
            ReboilerTypeCombo.Items.AddRange(new object[] { "None", "HeatEx", "Kettle", "Thermosiphon" });
            ReboilerTypeCombo.Location = new System.Drawing.Point(145, 36);
            ReboilerTypeCombo.Margin = new Padding(2);
            ReboilerTypeCombo.Name = "ReboilerTypeCombo";
            ReboilerTypeCombo.Size = new System.Drawing.Size(135, 23);
            ReboilerTypeCombo.TabIndex = 0;
            ReboilerTypeCombo.SelectionChangeCommitted += ReboilerTypeCombo_SelectionChangeCommitted;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(40, 38);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 15);
            label1.TabIndex = 1;
            label1.Text = "Reboiler Type";
            // 
            // ColumnDrawreboilerDLG
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(325, 97);
            Controls.Add(label1);
            Controls.Add(ReboilerTypeCombo);
            Margin = new Padding(2);
            Name = "ColumnDrawreboilerDLG";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reboiler Options";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox ReboilerTypeCombo;
        private Label label1;
    }
}