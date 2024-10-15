using ModelEngine;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Units
{
    public partial class Chart2 : Form
    {
        private readonly Port_Material port;

        public Chart2()
        {
            InitializeComponent();
        }

        public Chart2(Port_Material port)
        {
            InitializeComponent();
            this.port = port;
            ChartPropertyvsBP();

            this.Text = port.Owner.Name;
        }

        public void ChartPropertyvsBP()
        {
            PropertyChart.Titles.Clear();
            PropertyChart.Titles.Add("Propertyvs.BP(C)");
            PropertyChart.Series.Clear();
            PropertyChart.ChartAreas[0].AxisX.Title = "LV%";
            PropertyChart.ChartAreas[0].AxisY.Title = "NBPC";

            Series series1 = PropertyChart.Series.Add("Calculated");
            series1.ChartType = SeriesChartType.Line;
            series1.MarkerStyle = MarkerStyle.Triangle;
            series1.Color = Color.Red;

            DataPointCollection list1 = series1.Points;

            double x, y;
            double[] CumLVct = port.cc.VolFractionsCumulative;

            for (int i = 0; i < port.cc.Count; i++)
            {
                x = Math.Round(CumLVct[i], 2) * 100;
                y = port.cc[i].MidBP.Celsius;
                if (x < 100)
                    list1.AddXY(x, y);
            }

            if (list1.Count > 0)
            {
                double YMax = list1.FindMaxByValue().YValues.Max() * 1.1;
                PropertyChart.ChartAreas[0].AxisY.Maximum = YMax;
                PropertyChart.ChartAreas[0].AxisY.Minimum = 0;

                //PropertyChart.ChartAreas[0].AxisY.Maximum=Math.Round(YMax,1);
                //PropertyChart.ChartAreas[0].AxisY.Minimum=Math.Round(list1.FindMinByValue().YValues.Min()/1.1,2);
                //PropertyChart.ChartAreas[0].AxisY.LabelStyle.Format.;
                if (PropertyChart.ChartAreas[0].AxisY.Maximum == PropertyChart.ChartAreas[0].AxisY.Minimum)
                    PropertyChart.ChartAreas[0].AxisY.Maximum = PropertyChart.ChartAreas[0].AxisY.Minimum + 10;
            }
        }
    }
}