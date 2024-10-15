
using   ModelEngine;

namespace   DialogControls
{
    partial class  SingleDataEntry
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected  override  void   Dispose(bool  disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private  void   InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleDataEntry));
            this.gb = new System.Windows.Forms.GroupBox();
            this.uom1 = new Units.UOMTextBox();
            this.gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb
            // 
            this.gb.Controls.Add(this.uom1);
            this.gb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb.Location = new System.Drawing.Point(0, 0);
            this.gb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gb.Name = "gb";
            this.gb.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gb.Size = new System.Drawing.Size(218, 61);
            this.gb.TabIndex = 4;
            this.gb.TabStop = false;
            this.gb.Text = "Flow Type";
            this.gb.Enter += new System.EventHandler(this.gb_Enter);
            // 
            // uom1
            // 
            this.uom1.BackColor = System.Drawing.SystemColors.Control;
            this.uom1.ComboBoxHeight = 23;
            this.uom1.ComboBoxVisible = false;
            this.uom1.ComboBoxWidth = 60;
            this.uom1.DefaultUnits = "Quality";
            this.uom1.DefaultValue = "0E000";
            this.uom1.DescriptionWidth = 64;
            this.uom1.DisplayUnitArray = new string[] {
        "Quality"};
            this.uom1.DisplayUnits = "Quality";
            this.uom1.Label = "";
            this.uom1.LabelSize = 64;
            this.uom1.Location = new System.Drawing.Point(16, 22);
            this.uom1.Name = "uom1";
            this.uom1.Precision = 4;
            this.uom1.ReadOnly = false;
            this.uom1.Size = new System.Drawing.Size(185, 22);
            this.uom1.Source = SourceEnum.Input;
            this.uom1.TabIndex = 0;
            this.uom1.TextBoxFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uom1.TextBoxHeight = 23;
            this.uom1.TextBoxLeft = 64;
            this.uom1.TextBoxSize = 121;
            this.uom1.UOMprop = ((ModelEngine.UOMProperty)(resources.GetObject("uom1.UOMprop")));
            this.uom1.UOMType = Units.ePropID.NullUnits;
            this.uom1.Value = 0D;
            // 
            // SingleDataEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SingleDataEntry";
            this.Size = new System.Drawing.Size(218, 61);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MassMolarVol_MouseClick);
            this.Resize += new System.EventHandler(this.SingleProperty_Resize);
            this.gb.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.GroupBox gb;
        private  Units.UOMTextBox uom1;
    }
}
