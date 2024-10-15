using ModelEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Units.UOM;

namespace TestDistillations
{
    [TestClass]
    public class TestDistillations
    {
        [TestMethod]
        public void TestD86DistConversion()
        {
            DistPoints res;
            DistPoint a = new(1, new Temperature(-2.00, TemperatureUnit.Celsius));
            DistPoint b = new(5, new Temperature(26.00, TemperatureUnit.Celsius));
            DistPoint c = new(10, new Temperature(36.00, TemperatureUnit.Celsius));
            DistPoint d = new(20, new Temperature(68.00, TemperatureUnit.Celsius));
            DistPoint e = new(30, new Temperature(86.00, TemperatureUnit.Celsius));
            DistPoint f = new(50, new Temperature(109.00, TemperatureUnit.Celsius));
            DistPoint g = new(70, new Temperature(136.00, TemperatureUnit.Celsius));
            DistPoint h = new(80, new Temperature(148.00, TemperatureUnit.Celsius));
            DistPoint i = new(90, new Temperature(161.00, TemperatureUnit.Celsius));
            DistPoint j = new(95, new Temperature(168.00, TemperatureUnit.Celsius));
            DistPoint k = new(99, new Temperature(177.00, TemperatureUnit.Celsius));
            DistPoints points = new();
            points.Add(a);
            points.Add(b);
            points.Add(c);
            points.Add(d);
            points.Add(e);
            points.Add(f);
            points.Add(g);
            points.Add(h);
            points.Add(i);
            points.Add(j);
            points.Add(k);

            enumDistType from = enumDistType.D86, to = enumDistType.D86;
            res = DistillationConversions.Convert(from, to, points);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(from.ToString() + " " + to.ToString() + " " + res.getPCTs()[n] + " " + res[n].BP.ToString());

            from = enumDistType.D86;
            to = enumDistType.TBP_VOL;
            res = DistillationConversions.Convert(from, to, points);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(from.ToString() + " " + to.ToString() + " " + res.getPCTs()[n] + " " + res[n].BP.ToString());

            from = enumDistType.TBP_VOL;
            to = enumDistType.D86;
            res = DistillationConversions.Convert(from, to, points);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(from.ToString() + " " + to.ToString() + " " + res.getPCTs()[n] + " " + res[n].BP.ToString());

            from = enumDistType.D86;
            to = enumDistType.TBP_WT;
            res = DistillationConversions.Convert(from, to, points);

            res = DistillationConversions.Convert(to, from, res);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(from.ToString() + " " + to.ToString() + " " + res.getPCTs()[n] + " " + res[n].BP.ToString());

            from = enumDistType.D1160;
            to = enumDistType.TBP_VOL;
            res = DistillationConversions.Convert(from, to, points);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(from.ToString() + " " + to.ToString() + " " + res.getPCTs()[n] + " " + res[n].BP.ToString());

            res = DistillationConversions.Convert(to, from, res);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(from.ToString() + " " + to.ToString() + " " + res.getPCTs()[n] + " " + res[n].BP.ToString());

            from = enumDistType.D2887;
            to = enumDistType.TBP_VOL;
            res = DistillationConversions.Convert(from, to, points);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(from.ToString() + " " + to.ToString() + " " + res.getPCTs()[n] + " " + res[n].BP.ToString());

            from = enumDistType.D2887;
            to = enumDistType.TBP_WT;
            res = DistillationConversions.Convert(to, from, res);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(from.ToString() + " " + to.ToString() + " " + res.getPCTs()[n] + " " + res[n].BP.ToString());
        }

        [TestMethod]
        public void TestDistConversion2()
        {
            DistPoint a = new(1, new Temperature(-48.19));
            DistPoint c = new(10, new Temperature(7.78));
            DistPoint e = new(30, new Temperature(76.17));
            DistPoint f = new(50, new Temperature(109.36));
            DistPoint g = new(70, new Temperature(143.29));
            DistPoint i = new(90, new Temperature(173.22));
            DistPoint k = new(99, new Temperature(187.24));
            DistPoints points = new();
            points.Add(a);
            //points.Add(b);
            points.Add(c);
            points.Add(e);
            points.Add(f);
            points.Add(g);
            //points.Add(h);
            points.Add(i);
            //points.Add(j);
            points.Add(k);
            Debug.Print(Math2.CubicSpline.CubSpline(eSplineMethod.Constrained, 95D, points.getPCTs(), points.GetKDoubles()).ToString());
        }
    }
}