namespace NaphthaReformerSI
{
    partial class PONAData
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
            this.ponAdata1 = new DialogControls.PONAdata();
            this.SuspendLayout();
            // 
            // ponAdata1
            // 
            this.ponAdata1.Basis = enumMassMolarOrVol.Molar;
            this.ponAdata1.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ponAdata1.Location = new System.Drawing.Point(-1, 1);
            this.ponAdata1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ponAdata1.Name = "ponAdata1";
            this.ponAdata1.Size = new System.Drawing.Size(558, 315);
            this.ponAdata1.TabIndex = 0;
            // 
            // PONAData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 324);
            this.Controls.Add(this.ponAdata1);
            this.Name = "PONAData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PONAData";
            this.ResumeLayout(false);

        }

        #endregion

        private DialogControls.PONAdata ponAdata1;
    }
}