using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steam;
using Steam97;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace TestSteam
{
    [TestClass]
    public class UnitTest1
    {
        public const double Gravity = 9.80665;
        private double RGas = 8.31446261815324;
        public const double MW = 18.01528;

        [TestMethod]
        public void TestSteamEnthalpy()
        {
            Temperature t = new(360, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            var res = steam.hpt(30, t);
            var Tres = steam.Tph(1, res);
        }

        [TestMethod]
        public void TestSteamCp()
        {
            Temperature t = new(360, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            var res = steam.cppt(30, t);
        }

        [TestMethod]
        public void TestSteamCv()
        {
            Temperature t = new(360, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            var res = steam.cvpt(30, t);
        }

        [TestMethod]
        public void TestSteamEntropy()
        {
            Temperature t = new(100, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            var S = steam.spt(1, 273.15 + 900);
            var Ts = steam.Tps(1, S) - 273.15;
        }

        [TestMethod]
        public void TestPump()
        {
            Temperature t = new(100, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            steam97.Pump(1000, 1, 25, 60, 75);
        }

        [TestMethod]
        public void TestExpander()
        {
            Temperature t = new(360, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            var res = steam97.Expander(1000, 60, 360, 50, 75);
        }

        [TestMethod]
        public void TestCompressor()
        {
            Temperature t = new(360, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            var res = steam97.Compressor(1000, 50, 360, 54.77226, 100);
        }

        [TestMethod]
        public void TestDSP()
        {
            Temperature t = new(360, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            steam97.DSP(1000, 50, 360, 50, 340, 50, 100);
        }

        [TestMethod]
        public void TestDSPF()
        {
            Temperature t = new(360, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            var res = steam97.DSPF(1000, 50, 360, 50, 1000, 50, 100);
        }

        [TestMethod]
        public void TestVALVE()
        {
            Temperature t = new(360, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            var res = steam97.Valve(1000, 50, 360, 40);
        }

        [TestMethod]
        public void TestTsat()
        {
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            var res = steam.Tsat(10);
        }

        [TestMethod]
        public void TestFlash()
        {
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            Temperature Tsat = steam.Tsat(50);
            steam97.Flash(50, Tsat.Celsius, 40);
        }

        [TestMethod]
        public void TestBoiler()
        {
            StmPropIAPWS97 steam = new();
            COMSteam steam97 = new();
            double duty = steam97.Boiler(100, 50, 100, 60, 90);
        }

        [TestMethod]
        public void PolyHeadFactoredExpanderPoly()
        {
            Temperature T = new(360, TemperatureUnit.Celsius);
            ThermoPropsMass PropsIn = Steam97.StmPropIAPWS97.WaterPropsMass(60, T);
            StmPropIAPWS97 steam = new();
            ExpansionCompression exp = new(steam.Tph, StmPropIAPWS97.WaterPropsMass, PropsIn.P, 50, PropsIn.T, 1000, 75, EffType.poly, Factormethod.Huntington, true);
            var res = exp.PolytropicFluidHead;
            Debug.Print("Duty:" + exp.Power.ToString() + "VS 9.732188");
            Debug.Print("IsenEff:" + exp.IsenEff.ToString());
            Debug.Print("PolyEff:" + exp.PolyEff.ToString());
        }

        [TestMethod]
        public void PolyHeadFactoredExpanderIsen()
        {
            Temperature T = new(360, TemperatureUnit.Celsius);
            ThermoPropsMass PropsIn = Steam97.StmPropIAPWS97.WaterPropsMass(60, T);
            StmPropIAPWS97 steam = new();
            ExpansionCompression exp = new(steam.Tph, StmPropIAPWS97.WaterPropsMass, PropsIn.P, 50, PropsIn.T, 1000, 75, EffType.Isen, Factormethod.Huntington, true);
            var res = exp.PolytropicFluidHead;
            Debug.Print("Duty:" + exp.Power.ToString() + "VS 9.6814");
            Debug.Print("IsenEff:" + exp.IsenEff.ToString());
            Debug.Print("PolyEff:" + exp.PolyEff.ToString());
        }

        [TestMethod]
        public void PolyHeadFactoredCompressionPoly()
        {
            Temperature T = new(360, TemperatureUnit.Celsius);
            ThermoPropsMass PropsIn = Steam97.StmPropIAPWS97.WaterPropsMass(50, T);
            StmPropIAPWS97 steam = new();
            ExpansionCompression exp = new(steam.Tph, StmPropIAPWS97.WaterPropsMass, PropsIn.P, 60, T, 1000, 75, EffType.poly, Factormethod.Huntington, false);
            var res = exp.PolytropicFluidHead;
            Debug.Print("Duty:" + exp.Power.ToString() + "VS -18.45794");
            Debug.Print("IsenEff:" + exp.IsenEff.ToString());

            Debug.Print("PolyEff:" + exp.PolyEff.ToString());
        }

        [TestMethod]
        public void PolyHeadFactoredCompressionIsen()
        {
            Temperature T = new(360, TemperatureUnit.Celsius);
            ThermoPropsMass PropsIn = Steam97.StmPropIAPWS97.WaterPropsMass(50, T);
            StmPropIAPWS97 steam = new();
            ExpansionCompression exp = new(steam.Tph, StmPropIAPWS97.WaterPropsMass, PropsIn.P, 60, T, 1000, 75, EffType.Isen, Factormethod.Huntington, false);
            var res = exp.PolytropicFluidHead;
            Debug.Print("Duty:" + exp.Power.ToString() + "VS -18.32838454");
            Debug.Print("IsenEff:" + exp.IsenEff.ToString());
            Debug.Print("PolyEff:" + exp.PolyEff.ToString());
        }

        [TestMethod]
        public void PolyIsenFactored()
        {
            Temperature T = new(360, TemperatureUnit.Celsius);
            ThermoPropsMass PropsIn = Steam97.StmPropIAPWS97.WaterPropsMass(60, T);
            StmPropIAPWS97 steam = new();
            ExpansionCompression exp = new(steam.Tph, StmPropIAPWS97.WaterPropsMass, PropsIn.P, 50, PropsIn.T, 75, 1000, EffType.Isen, Factormethod.Huntington, true);
            var res = exp.IsentropicFluidHead;
        }

        [TestMethod]
        public void TestZ()
        {
            COMSteam steam97 = new();
            var res = steam97.Z(60, 360);
        }
    }
}