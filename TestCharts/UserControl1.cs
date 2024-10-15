using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestCharts
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public void AddPlot(string Name, double[] XValues, double[] YValues, Color color)
        {
            Series series1 = null;

            if (series1 is null)
                series1 = chart1.Series.Add(Name);

            series1.ChartType = SeriesChartType.Line;
            series1.IsValueShownAsLabel = false;
            series1["LabelStyle"] = "Center";

            DataPointCollection list = series1.Points;
            list.Clear();

            for (int c = 0; c < XValues.Length; c++)
            {
                double y = YValues[c];
                double x = XValues[c];
                list.AddXY(x, y);
            }

            series1.MarkerColor = color;
            series1.MarkerSize = 2;
        }
    }
}