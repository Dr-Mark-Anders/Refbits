using System.Windows.Forms;

namespace Units
{
    partial class ReboilerTypeControl
    {
        ///<summary>
        ///Requireddesignervariable.
        ///</summary>
        private System.ComponentModel.IContainer Components = null;

        ///<summary>
        ///Cleanupanyresourcesbeingused.
        ///</summary>
        ///<paramname="disposing">trueifmanagedresourcesshouldbedisposed;otherwise,false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

# region ComponentDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>
        private void InitializeComponent()
        {
            this.gbReboilerType = new System.Windows.Forms.GroupBox();
            this.rbKettle = new System.Windows.Forms.RadioButton();
            this.gbReboilerType.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbReboilerType
            // 
            this.gbReboilerType.Controls.Add(this.rbKettle);
            this.gbReboilerType.Location = new System.Drawing.Point(4, 3);
            this.gbReboilerType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbReboilerType.Name = "gbReboilerType";
            this.gbReboilerType.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbReboilerType.Size = new System.Drawing.Size(192, 132);
            this.gbReboilerType.TabIndex = 16;
            this.gbReboilerType.TabStop = false;
            this.gbReboilerType.Text = "RebilerOptions";
            // 
            // rbKettle
            // 
            this.rbKettle.AutoSize = true;
            this.rbKettle.Checked = true;
            this.rbKettle.Location = new System.Drawing.Point(22, 30);
            this.rbKettle.Margin = new System.Windows.Forms.Padding(2);
            this.rbKettle.Name = "rbKettle";
            this.rbKettle.Size = new System.Drawing.Size(55, 19);
            this.rbKettle.TabIndex = 12;
            this.rbKettle.TabStop = true;
            this.rbKettle.Text = "Kettle";
            this.rbKettle.UseVisualStyleBackColor = true;
            this.rbKettle.CheckedChanged += new System.EventHandler(this.rbPartial_CheckedChanged);
            // 
            // ReboilerTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbReboilerType);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ReboilerTypeControl";
            this.Size = new System.Drawing.Size(205, 143);
            this.gbReboilerType.ResumeLayout(false);
            this.gbReboilerType.PerformLayout();
            this.ResumeLayout(false);

        }

#endregion
        private GroupBox gbReboilerType;
        private RadioButton rbKettle;
    }
}
