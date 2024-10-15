using ModelEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Units.UOM;

namespace UnitTests
{
    [TestClass]
    public class TestGibbs
    {
        [TestMethod]
        public void TestButaneGIBBSFormation()
        {
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.PR78;

            Gibbs res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            TemperatureC T = 25;
            Pressure P = 1;
            res = IdealGas.StreamGibbsFormation(cc, T, cc.MoleFractions);
        }

        [TestMethod]
        public void TestGIBBS()
        {
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.PR78;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetComponent("CO");
            cc.Add(sc);
            sc = Thermodata.GetComponent("CO2");
            cc.Add(sc);
            sc = Thermodata.GetComponent("H2O");
            cc.Add(sc);
            sc = Thermodata.GetComponent("Hydrogen");
            cc.Add(sc);

            cc[0].MoleFraction = 1 / 3d;
            cc[1].MoleFraction = 1 / 3d;
            cc[2].MoleFraction = 1 / 3d;
            cc[3].MoleFraction = 0;

            TemperatureC T = 100;
            Pressure P = 1;
            res = ThermodynamicsClass.GibbsMix(cc, P, T, state, thermo);
        }

        [TestMethod]
        public void TestButaneGIBBS()
        {
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.PR78;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            TemperatureC T = 25;
            Pressure P = 1;
            res = ThermodynamicsClass.GibbsMix(cc, P, T, state, thermo);
        }

        [TestMethod]
        public void TestButanePropaneGIBBS()
        {
            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new();
            BaseComp sc;

            TemperatureC T = 25;
            Pressure P = 1;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("propane");
            cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.5;

            res = ThermodynamicsClass.GibbsMix(cc, P, T, state, cc.Thermo);
        }
    }
}