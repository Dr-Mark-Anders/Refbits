
namespace TestUOMForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.uomTextBox2 = new Units.UOMTextBox();
            this.SuspendLayout();
            // 
            // uomTextBox2
            // 
            this.uomTextBox2.BackColor = System.Drawing.SystemColors.Control;
            this.uomTextBox2.ComboBoxHeight = 21;
            this.uomTextBox2.ComboBoxVisible = false;
            this.uomTextBox2.ComboBoxWidth = 60;
            this.uomTextBox2.DefaultUnits = "kg_hr";
            this.uomTextBox2.DefaultValue = "0E000";
            this.uomTextBox2.DescriptionWidth = 64;
            this.uomTextBox2.DisplayUnitArray = new string[] {
        "kg_hr",
        "kg_s",
        "te_d",
        "te_hr",
        "lbs_hr"};
            this.uomTextBox2.DisplayUnits = "kg_hr";
            this.uomTextBox2.Label = "Flow";
            this.uomTextBox2.LabelSize = 64;
            this.uomTextBox2.Location = new System.Drawing.Point(186, 70);
            this.uomTextBox2.Name = "uomTextBox2";
            this.uomTextBox2.Precision = 4;
            this.uomTextBox2.ReadOnly = false;
            this.uomTextBox2.Size = new System.Drawing.Size(227, 22);
            this.uomTextBox2.Source = SourceEnum.Input;
            this.uomTextBox2.TabIndex = 0;
            this.uomTextBox2.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uomTextBox2.TextBoxHeight = 20;
            this.uomTextBox2.TextBoxLeft = 90;
            this.uomTextBox2.TextBoxSize = 50;
            this.uomTextBox2.UOMprop = ((EngineThermo.UOMProperty)(resources.GetObject("uomTextBox2.UOMprop")));
            this.uomTextBox2.UOMType = Units.ePropID.MF;
            this.uomTextBox2.Value = 0D;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 243);
            this.Controls.Add(this.uomTextBox2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private Units.UOMTextBox uomTextBox1;
        private Units.UOMTextBox uomTextBox2;
    }
}

