namespace Charts
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            chartControl1 = new ChartControl();
            SuspendLayout();
            // 
            // chartControl1
            // 
            chartControl1.BackColor = SystemColors.ActiveCaption;
            chartControl1.Dock = DockStyle.Fill;
            chartControl1.Location = new Point(0, 0);
            chartControl1.Margin = new Padding(4, 3, 4, 3);
            chartControl1.Name = "chartControl1";
            chartControl1.Size = new Size(895, 534);
            chartControl1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 534);
            Controls.Add(chartControl1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private ChartControl chartControl1;
    }
}