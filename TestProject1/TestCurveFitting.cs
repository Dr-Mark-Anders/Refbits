using ModelEngine;
using Extensions;
using Math2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System;
using System.Collections.Generic;
using Units;
using Units.UOM;

namespace UnitTests
{
    [TestClass]
    public class TestCurveFittingAndCrudeCutting
    {
        [TestMethod]
        public void TestCrudeCutter()
        {
            Components cc = DefaultStreams.CrudeUrals();
            CrudeCutter cutter = new();
            Port_Material p = new(cc);
            p.SetPortValue(ePropID.MOLEF, 1, SourceEnum.Input);
            p.NormaliseFractions(FlowFlag.Molar);
            p.UpdateFlows();

            Temperature[] CutPoints = new Temperature[3] { 100 + 273.15, 150 + 273.15, 300 + 273.15 };

            List<Port_Material> res = cutter.CutStreams(p, CutPoints, Guid.Empty);
        }

        [TestMethod]
        public void TestMW()
        {
            MW(687.9, 338.7);
        }

        [TestMethod]
        public void TestSpline()
        {
            var cumLVPCts = new List<double>();
            List<double> ebp, lvs;
            lvs = new List<double>(new double[11] { 5.2518, 14.06, 25.6, 31.17, 40.67, 61.37, 65.64, 86.03, 89.98, 92.51, 100.00 });
            ebp = new List<double>(new double[11] { 36.10, 95.00, 149.00, 175.00, 232.00, 342.00, 369.00, 509.00, 550.00, 585.00, 850.00 });

            var res = CubicSpline.CubSpline(eSplineMethod.Constrained, 40, ebp, lvs).ToString();
        }

        public double MW(double Density, double MeABP)
        {
            double mw;
            //=-12272.6+9486.4*I40/1000+(8.3741-5.9917*I40/1000)*I69
            //+(1-0.77084*I40/1000-0.02058*(I40/1000)^2)*(0.7465-222.4666/I69)*10^7/I69
            //+(1-0.80882*I40/1000+0.02226*(I40/1000)^2)*(0.3228-17.335/I69)*10^12/I69^3
            mw = -12272.6 + 9486.4 * Density / 1000 + (8.3741 - 5.9917 * Density / 1000) * MeABP
                + (1 - 0.77084 * Density / 1000 - 0.02058 * (Density / 1000).Pow(2)) * (0.7465 - 222.4666 / MeABP) * 1e7 / MeABP
                + (1 - 0.80882 * Density / 1000 + 0.02226 * (Density / 1000).Pow(2)) * (0.3228 - 17.335 / MeABP) * 1e12 / MeABP.Pow(3);
            return mw;
        }

        [TestMethod]
        public void teststats()
        {
            Math2.Statistical.NormSInv(0.7);  // result 0.52440051327929416

            Math2.Statistical.NormDist(0.5244);  // 0.7
        }
    }
}