
namespace Units
{
    partial class Chart2
    {
        ///<summary>
        ///Requireddesignervariable.
        ///</summary>
        private System.ComponentModel.IContainer Components = null;

        ///<summary>
        ///Cleanupanyresourcesbeingused.
        ///</summary>
        ///<paramname="disposing">trueifmanagedresourcesshouldbedisposed;otherwise,false.</param>
        protected override void  Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region WindowsFormDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>
        private void  InitializeComponent()
        {
            this.PropertyChart = new Charts.ChartControl();
            this.SuspendLayout();
            //
            //chartControl1
            //
            this.PropertyChart.Location = new System.Drawing.Point(29, 43);
            this.PropertyChart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PropertyChart.Name = "chartControl1";
            this.PropertyChart.Size = new System.Drawing.Size(866, 429);
            this.PropertyChart.TabIndex = 0;
            //
            //Chart2
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.PropertyChart);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Chart2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chart2";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private Charts.ChartControl PropertyChart;
    }
}