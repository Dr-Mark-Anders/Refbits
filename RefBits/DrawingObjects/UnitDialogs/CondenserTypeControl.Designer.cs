using System.Windows.Forms;

namespace Units
{
    partial class CondenserTypeControl
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
            this.cbTotalReflux = new System.Windows.Forms.CheckBox();
            this.gbCondenserType = new System.Windows.Forms.GroupBox();
            this.txtSubCooling = new System.Windows.Forms.TextBox();
            this.rbSubCooled = new System.Windows.Forms.RadioButton();
            this.rbTotal = new System.Windows.Forms.RadioButton();
            this.rbPartial = new System.Windows.Forms.RadioButton();
            this.gbCondenserType.SuspendLayout();
            this.SuspendLayout();
            //
            //cbTotalReflux
            //
            this.cbTotalReflux.AutoSize = true;
            this.cbTotalReflux.Location = new System.Drawing.Point(21, 100);
            this.cbTotalReflux.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbTotalReflux.Name = "cbTotalReflux";
            this.cbTotalReflux.Size = new System.Drawing.Size(87, 19);
            this.cbTotalReflux.TabIndex = 17;
            this.cbTotalReflux.Text = "TotalReflux";
            this.cbTotalReflux.UseVisualStyleBackColor = true;
            this.cbTotalReflux.CheckedChanged += new System.EventHandler(this.cbTotalReflux_CheckedChanged);
            //
            //gbCondenserType
            //
            this.gbCondenserType.Controls.Add(this.cbTotalReflux);
            this.gbCondenserType.Controls.Add(this.txtSubCooling);
            this.gbCondenserType.Controls.Add(this.rbSubCooled);
            this.gbCondenserType.Controls.Add(this.rbTotal);
            this.gbCondenserType.Controls.Add(this.rbPartial);
            this.gbCondenserType.Location = new System.Drawing.Point(4, 3);
            this.gbCondenserType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbCondenserType.Name = "gbCondenserType";
            this.gbCondenserType.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbCondenserType.Size = new System.Drawing.Size(192, 132);
            this.gbCondenserType.TabIndex = 16;
            this.gbCondenserType.TabStop = false;
            this.gbCondenserType.Text = "CondenserOptions";
            //
            //txtSubCooling
            //
            this.txtSubCooling.Location = new System.Drawing.Point(115, 74);
            this.txtSubCooling.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSubCooling.Name = "txtSubCooling";
            this.txtSubCooling.Size = new System.Drawing.Size(63, 23);
            this.txtSubCooling.TabIndex = 8;
            this.txtSubCooling.Text = "0";
            this.txtSubCooling.Validated += new System.EventHandler(this.txtSubCooling_Validated);
            //
            //rbSubCooled
            //
            this.rbSubCooled.AutoSize = true;
            this.rbSubCooled.Location = new System.Drawing.Point(21, 76);
            this.rbSubCooled.Margin = new System.Windows.Forms.Padding(2);
            this.rbSubCooled.Name = "rbSubCooled";
            this.rbSubCooled.Size = new System.Drawing.Size(88, 19);
            this.rbSubCooled.TabIndex = 14;
            this.rbSubCooled.Text = "Sub-Cooled";
            this.rbSubCooled.UseVisualStyleBackColor = true;
            this.rbSubCooled.CheckedChanged += new System.EventHandler(this.rbSubCooled_CheckedChanged);
            //
            //rbTotal
            //
            this.rbTotal.AutoSize = true;
            this.rbTotal.Location = new System.Drawing.Point(21, 50);
            this.rbTotal.Margin = new System.Windows.Forms.Padding(2);
            this.rbTotal.Name = "rbTotal";
            this.rbTotal.Size = new System.Drawing.Size(122, 19);
            this.rbTotal.TabIndex = 13;
            this.rbTotal.Text = "Total(LiquidOnly)";
            this.rbTotal.UseVisualStyleBackColor = true;
            this.rbTotal.CheckedChanged += new System.EventHandler(this.rbTotal_CheckedChanged);
            //
            //rbPartial
            //
            this.rbPartial.AutoSize = true;
            this.rbPartial.Checked = true;
            this.rbPartial.Location = new System.Drawing.Point(21, 23);
            this.rbPartial.Margin = new System.Windows.Forms.Padding(2);
            this.rbPartial.Name = "rbPartial";
            this.rbPartial.Size = new System.Drawing.Size(144, 19);
            this.rbPartial.TabIndex = 12;
            this.rbPartial.TabStop = true;
            this.rbPartial.Text = "Partial(Liquid/Vapour)";
            this.rbPartial.UseVisualStyleBackColor = true;
            this.rbPartial.CheckedChanged += new System.EventHandler(this.rbPartial_CheckedChanged);
            //
            //CondenserTypeControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCondenserType);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CondenserTypeControl";
            this.Size = new System.Drawing.Size(205, 143);
            this.gbCondenserType.ResumeLayout(false);
            this.gbCondenserType.PerformLayout();
            this.ResumeLayout(false);

        }

#endregion

        private CheckBox cbTotalReflux;
        private GroupBox gbCondenserType;
        private TextBox txtSubCooling;
        private RadioButton rbSubCooled;
        private RadioButton rbTotal;
        private RadioButton rbPartial;
    }
}
