using EngineThermo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System;
using System.Diagnostics;
using Units;
using static ModelEngine.PrintInfo;

namespace UnitTests
{
    [TestClass]
    public class TestVacFeedFlashes
    {
        [TestMethod]
        public void TestVacColumnWith2PA_2()
        {
            FlowSheet fs = new FlowSheet();
            BaseComp sc;
            PseudoComponent pc;
            Components cc = new Components();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            cc = TestStreams.ShortResidue3();

            cc.T.Celsius = 400;
            cc.P.BarA = 0.4;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.MolarFlow = new StreamProperty(ePropID.MOLEF, SourceEnum.Input, 1);
            feed.P = new StreamProperty(ePropID.P, SourceEnum.Input, 0.4);
            feed.T = new StreamProperty(ePropID.T, SourceEnum.Input, 273.15 + 400);
            feed.components.AddRange(fs.ComponentList);
            feed.components.Origin = SourceEnum.Input;

            var res = feed.Flash();
            PrintPortInfo(feed);

            var H = feed.H;

            feed.Props.Clear();
            feed.MolarFlow = new StreamProperty(ePropID.MOLEF, SourceEnum.Input, 1);
            feed.P = new StreamProperty(ePropID.P, SourceEnum.Input, 0.4);
            feed.H = H;

            feed.Flash();
            PrintPortInfo(feed);


            enumCalcResult cres = enumCalcResult.Converged;
            var props = Thermodynamics.CalcSinglePhaseEnthalpyAndEntropy(feed.components.LiquidComponents, 273.15 + 400, enumFluidRegion.Liquid, enumMassOrMolar.Molar, feed.Thermo, ref cres);
            var propsvap = Thermodynamics.CalcSinglePhaseEnthalpyAndEntropy(feed.components.VapourComponents, 273.15 + 400, enumFluidRegion.Vapour, enumMassOrMolar.Molar, feed.Thermo, ref cres);


            var watch = Stopwatch.StartNew();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Assert.IsTrue(res);
        }
    }
}
