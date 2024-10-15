using System.Windows.Forms;

namespace Units
{
    partial class ColumnDrawCondenserDLG
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
            CondenserType = new ComboBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // CondenserType
            // 
            CondenserType.FormattingEnabled = true;
            CondenserType.Items.AddRange(new object[] { "None", "Partial", "Total", "Total Reflux" });
            CondenserType.Location = new System.Drawing.Point(145, 36);
            CondenserType.Margin = new Padding(2);
            CondenserType.Name = "CondenserType";
            CondenserType.Size = new System.Drawing.Size(135, 23);
            CondenserType.TabIndex = 0;
            CondenserType.SelectionChangeCommitted += CondenserType_SelectionChangeCommitted;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(40, 38);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(91, 15);
            label1.TabIndex = 1;
            label1.Text = "Condenser Type";
            // 
            // ColumnDrawCondenserDLG
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(325, 97);
            Controls.Add(label1);
            Controls.Add(CondenserType);
            Margin = new Padding(2);
            Name = "ColumnDrawCondenserDLG";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Condenser Options";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox CondenserType;
        private Label label1;
    }
}