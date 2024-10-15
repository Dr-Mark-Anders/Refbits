using ModelEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steam97;
using System.Windows.Forms;
using Units.UOM;

namespace UnitTests
{
    [TestClass]
    public class TestSteam
    {
        [TestMethod]
        public void TestIAPW97()
        {
            Steam1967 steam = new Steam1967();
            // MessageBox.Show(steam.(273.15 + 100, out int  u).ToString());
        }

        [TestMethod]
        public void TestIAPW97Enthalpy()
        {
            StmPropIAPWS97 steam = new StmPropIAPWS97();
            MessageBox.Show((steam.hpt(600, 273.15 + 160) * 18).ToString());
        }

        [TestMethod]
        public void TestIAPW97FlashData()
        {
            StmPropIAPWS97 steam = new StmPropIAPWS97();
            double Tsat = steam.Tsat(5);
            var H = steam.hpt(5, Tsat);
            double Psat = steam.Psat(Tsat);
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

            sc = Thermodata.GetComponent("Propane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("N-Butane");
            cc.Add(sc);
            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.5;

            TemperatureC T = 100;
            Pressure P = 1;

            res1 = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out _, thermo);

            sc = Thermodata.GetComponent("H2O");
            cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.5;
            cc[2].MoleFraction = 0;

            res1 = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out _, thermo);

            cc[0].MoleFraction = 0.15;
            cc[1].MoleFraction = 0.15;
            cc[2].MoleFraction = 0.7;

            T = 0;
            P = 1;
            res2 = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out _, thermo);
        }
    }
}