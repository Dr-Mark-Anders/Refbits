using ModelEngine;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Units.UOM;

namespace Units
{
    public partial class ChartForm : Form
    {
        private Port_Material port;
        private enumAssayPCProperty prop;

        public ChartForm()
        {
            InitializeComponent();
            CBProperty.DataSource = Enum.GetNames(typeof(enumAssayPCProperty));
        }

        public ChartForm(Port_Material port)
        {
            this.port = port;
            InitializeComponent();
            CBProperty.DataSource = Enum.GetNames(typeof(enumAssayPCProperty));
        }

        public void ChartPropertyvsBP()
        {
            PropertyChart.Titles.Clear();
            PropertyChart.Titles.Add("Propertyvs.BP(C)");
            PropertyChart.Series.Clear();
            PropertyChart.ChartAreas[0].AxisX.Title = "NBPC";
            PropertyChart.ChartAreas[0].AxisY.Title = "Property";

            Series series1 = PropertyChart.Series.Add("Calculated");
            series1.ChartType = SeriesChartType.Line;
            series1.MarkerStyle = MarkerStyle.Triangle;
            series1.Color = Color.Red;

            Series series2 = PropertyChart.Series.Add("LabData");//CurveList.Clear();
            series2.Color = Color.Blue;
            series2.ChartType = SeriesChartType.Point;
            series2.MarkerStyle = MarkerStyle.Cross;
            series2.MarkerSize = 14;

            DataPointCollection list1 = series1.Points;
            DataPointCollection list2 = series2.Points;
            Temperature[] MidBP = port.cc.BPArray;

            double x, y;

            for (int i = 0; i < port.cc.Count; i++)
            {
                if (prop == enumAssayPCProperty.DENSITY15)
                {
                    y = port.cc[i].Density;
                    x = port.cc[i].MidBP.Celsius;
                    list1.AddXY(x, y);
                }
                else if (port.cc[i].Properties.ContainsKey(prop))
                {
                    y = port.cc[i].Properties[prop];
                    x = port.cc[i].MidBP.Celsius;
                    list1.AddXY(x, y);
                }
            }

            if (list1.Count > 0)
            {
                PropertyChart.ChartAreas[0].AxisX.Maximum = 850;
                PropertyChart.ChartAreas[0].AxisX.Minimum = 36;

                double YMax;
                if (prop == enumAssayPCProperty.DENSITY15)
                    YMax = 1300;
                else
                    YMax = list1.FindMaxByValue().YValues.Max() * 1.1;

                PropertyChart.ChartAreas[0].AxisY.Maximum = Math.Round(YMax, 1);
                PropertyChart.ChartAreas[0].AxisY.Minimum = Math.Round(list1.FindMinByValue().YValues.Min() / 1.1, 2);
                //PropertyChart.ChartAreas[0].AxisY.LabelStyle.Format.;
                if (PropertyChart.ChartAreas[0].AxisY.Maximum == PropertyChart.ChartAreas[0].AxisY.Minimum)
                    PropertyChart.ChartAreas[0].AxisY.Maximum = PropertyChart.ChartAreas[0].AxisY.Minimum + 10;
            }
        }

        private void CBProperty_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (Enum.TryParse(CBProperty.Text, out enumAssayPCProperty prop))
                this.prop = prop;
            else
                this.prop = enumAssayPCProperty.SULFUR;

            ChartPropertyvsBP();
        }
    }
}