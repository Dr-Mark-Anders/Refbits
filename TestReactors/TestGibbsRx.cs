using ModelEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using Units.UOM;

namespace TestReactors
{
    [TestClass]
    public class GibbsRx
    {
        [TestMethod]
        public void TestGibbssRx()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            Components cc = new();
            BaseComp sc;
            thermo.UseBIPs = false;

            sc = Thermodata.GetComponent("CO");
            cc.Add(sc);
            sc = Thermodata.GetComponent("Hydrogen");
            cc.Add(sc);
            sc = Thermodata.GetComponent("H2O");
            cc.Add(sc);
            sc = Thermodata.GetComponent("CO2");
            cc.Add(sc);

            cc[0].MoleFraction = 0.23830422367826207;
            cc[1].MoleFraction = 0.27259346120101074;
            cc[2].MoleFraction = 0.238304223678262;
            cc[3].MoleFraction = 0.25079809144246507;


            TemperatureC T = 1000;
            Pressure P = 3;

            GibbsReactor RX = new();

            Port_Material p = new Port_Material();
            p.cc = cc;
            p.T_ = new(Units.ePropID.T, 1000, SourceEnum.Input);
            p.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
            p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            p.cc.Origin = SourceEnum.Input;

            p.Flash();

            RX.PortIn = p;
            RX.Solve();
        }
    }
}