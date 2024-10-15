
namespace Units
{
    partial class ChartForm
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

        #region WindowsFormDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.CBProperty = new System.Windows.Forms.ComboBox();
            this.PropertyChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyChart)).BeginInit();
            this.SuspendLayout();
            //
            //CBProperty
            //
            this.CBProperty.FormattingEnabled = true;
            this.CBProperty.Location = new System.Drawing.Point(706, 12);
            this.CBProperty.Name = "CBProperty";
            this.CBProperty.Size = new System.Drawing.Size(150, 21);
            this.CBProperty.TabIndex = 3;
            this.CBProperty.SelectedIndexChanged += new System.EventHandler(this.CBProperty_SelectedIndexChanged);
            //
            //PropertyChart
            //
            this.PropertyChart.BackColor = System.Drawing.Color.LightGray;
            chartArea1.AxisX.Maximum = 900D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Maximum = 5000D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.PropertyChart.ChartAreas.Add(chartArea1);
            this.PropertyChart.Dock = System.Windows.Forms.DockStyle.Left;
            legend1.Name = "Legend1";
            this.PropertyChart.Legends.Add(legend1);
            this.PropertyChart.Location = new System.Drawing.Point(0, 0);
            this.PropertyChart.Margin = new System.Windows.Forms.Padding(2);
            this.PropertyChart.Name = "PropertyChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.PropertyChart.Series.Add(series1);
            this.PropertyChart.Size = new System.Drawing.Size(692, 453);
            this.PropertyChart.TabIndex = 4;
            this.PropertyChart.Text = "chart3";
            //
            //ChartForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 453);
            this.Controls.Add(this.PropertyChart);
            this.Controls.Add(this.CBProperty);
            this.Name = "ChartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chart";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.PropertyChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox CBProperty;
        private System.Windows.Forms.DataVisualization.Charting.Chart PropertyChart;
    }
}