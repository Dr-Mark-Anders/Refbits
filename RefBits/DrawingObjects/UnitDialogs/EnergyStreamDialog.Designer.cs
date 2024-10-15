namespace   Units.DrawingObjects.UnitDialogs
{
    partial class  EnergyStreamDialog
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
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(EnergyStreamDialog));
            this.uomTextBox1 = new  Units.UOMTextBox();
            this.SuspendLayout();
            // 
            // uomTextBox1
            // 
            this.uomTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.uomTextBox1.DefaultUnits = "kW";
            this.uomTextBox1.DefaultValue = "NaN";
            this.uomTextBox1.DescriptionWidth = 100;
            this.uomTextBox1.DisplayUnits = "";
            this.uomTextBox1.Label = "Energy Flow";
            this.uomTextBox1.LabelSize = 100;
            this.uomTextBox1.Location = new  System.Drawing.Point (24, 26);
            this.uomTextBox1.Name = "uomTextBox1";
            this.uomTextBox1.Precision = 4;
            this.uomTextBox1.ReadOnly  = false;
            this.uomTextBox1.Size = new  System.Drawing.Size(188, 20);
            this.uomTextBox1.Source = SourceEnum.Empty;
            this.uomTextBox1.TabIndex = 0;
            this.uomTextBox1.TextBoxFont = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            this.uomTextBox1.TextBoxHeight = 20;
            this.uomTextBox1.TextBoxSize = 89;
            this.uomTextBox1.UOMType = Units.ePropID.EnergyFlow;
            this.uomTextBox1.Value = double.NaN;
            // 
            // EnergyStreamDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(260, 77);
            this.Controls.Add(this.uomTextBox1);
            this.Name = "EnergyStreamDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EnergyStreamDialog";
            this.ResumeLayout(false);
        }

        #endregion

        private  UOMTextBox uomTextBox1;
    }
}