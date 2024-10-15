using EngineThermo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamIAPWS97;
using System.Windows.Forms;

namespace UnitTests
{
    [TestClass]
    public class TestSteam
    {

        [TestMethod]
        public void TestIAPW97()
        {
            StmPropIAPWS97 steam = new StmPropIAPWS97();
            MessageBox.Show(steam.Psat(273.15 + 100, out int u).ToString());
        }

        [TestMethod]
        public void TestIAPW97Enthalpy()
        {
            StmPropIAPWS97 steam = new StmPropIAPWS97();
            MessageBox.Show((steam.hpt(600, 273.15 + 160, out int u, 0) * 18).ToString());
        }

        [TestMethod]
        public void TestMixtureFugacity()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.PR78;

            double[] res1, res2;
            Components cc = new Components();
            BaseComp sc;

            double pcrit = LeeKesler.PCrit(1.1209, 1148);

            sc = Thermodata.GetRealComponent("Propane");
            cc.Add(sc);
            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);
            cc[0].molefraction = 0.5;
            cc[1].molefraction = 0.5;

            cc.T = new Temperature(273.15);
            cc.P = new Pressure(1);
            res1 = Thermodynamics.KMix(cc, cc.MolFractions, cc.MolFractions);


            sc = Thermodata.GetRealComponent("H2O");
            cc.Add(sc);

            cc[0].molefraction = 0.5;
            cc[1].molefraction = 0.5;
            cc[2].molefraction = 0;

            cc.T = new Temperature(273.15);
            cc.P = new Pressure(1);
            res1 = Thermodynamics.KMix(cc, cc.MolFractions, cc.MolFractions);

            cc[0].molefraction = 0.15;
            cc[1].molefraction = 0.15;
            cc[2].molefraction = 0.7;

            cc.T = new Temperature(273.15);
            cc.P = new Pressure(1);
            res2 = Thermodynamics.KMix(cc, cc.MolFractions, cc.MolFractions);

        }
    }
}
