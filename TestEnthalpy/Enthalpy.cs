using COMColumnNS;
using ModelEngine;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace TestEnthalpy
{
    [TestClass]
    public class TestEnthalpy
    {
        [TestMethod]
        public void TestSolidButaneMix()
        {
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            Components cc = new();
            Temperature T = new Temperature(273.15 + 50);
            Pressure P = new Pressure(1);
            cc.Add(Thermodata.GetComponent("n-butane"));
            SolidComponent solid = new SolidComponent();
            cc.Add(solid);

            Port_Material pm = new(cc);
            pm.T_ = new(ePropID.T, T);
            pm.P_ = new(ePropID.P, P);
            pm.cc.Origin = SourceEnum.Input;

            pm.SetMoleFractions(new double[] { 1, 0 });
            pm.NormaliseMoleFractions();
            pm.MolarFlow_ = new StreamProperty(ePropID.MOLEF,1);
            pm.UpdateFlows();
            pm.SetSolidMassRatio(7);

            pm.Flash();

            var Hl = pm.LiquidEnthalpy;
            var Hv = pm.VapourEnthalpy;

        }

        [TestMethod]
        public void TestEnthalpyMethods()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.SRK;
            thermo.UseBIPs = true;
            Components cc = new();
            BaseComp sc;
            GibbsReactor RX = new();
            Port_Material port = new Port_Material();
            Pressure P = new Pressure(9.7);
            Temperature T = new Temperature(20, TemperatureUnit.Celsius);
            Quality Q = new Quality(0.3498);

            sc = Thermodata.GetComponent("Ethane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("ethylene");
            cc.Add(sc);
            sc = Thermodata.GetComponent("Propane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("Propene");
            cc.Add(sc);
            sc = Thermodata.GetComponent("i-Butene");
            cc.Add(sc);
            sc = Thermodata.GetComponent("n-butane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("1-butene");
            cc.Add(sc);

            cc[0].MoleFraction = 0.0002;
            cc[1].MoleFraction = 0.0002;
            cc[2].MoleFraction = 0.1998;
            cc[3].MoleFraction = 0.7974;
            cc[4].MoleFraction = 0.0020;
            cc[5].MoleFraction = 0.0002;
            cc[6].MoleFraction = 0.0002;

            cc.NormaliseFractions();

            port.cc = cc;
            port.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
            port.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Input;
            port.cc.Thermo.UseBIPs = false;

            port.ClearNonInputs();
            List<Enthalpy> Hs = new();

            //P = 0.1;
            port.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            //port.Q = new(Units.ePropID.Q, Q, SourceEnum.Input);
            port.T_ = new(Units.ePropID.T, T, SourceEnum.Input);
            port.Flash();
            Enthalpy H = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo);
            Enthalpy IdealH = IdealGas.StreamIdealGasMolarEnthalpy(cc, T, cc.MoleFractions);
            Enthalpy HForm = ThermodynamicsClass.EnthalpyFormation25(cc, cc.MoleFractions);
            Enthalpy HDep = EnthalpDepClass.HDeparture(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo);
            ThermoProps res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo);

            EnthalpyDepartureLinearisation edepLine = new();
            edepLine.VapUpdate(cc, cc.MoleFractions, P, T, T + 1, thermo);
            Enthalpy LinEst = edepLine.VapEnthalpy(cc, cc.MoleFractions, P, T);

            cc[0].MoleFraction = 0.37478;
            cc[1].MoleFraction = 0.53001;
            cc[2].MoleFraction = 0.09521;

            Enthalpy HL = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);
            Enthalpy IdealHL = IdealGas.StreamIdealGasMolarEnthalpy(cc, T, cc.MoleFractions);
            Enthalpy HFormL = ThermodynamicsClass.EnthalpyFormation25(cc, cc.MoleFractions);
            Enthalpy HDepL = EnthalpDepClass.HDeparture(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);
            ThermoProps resL = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);

            cc[0].MoleFraction = 0.005;
            cc[1].MoleFraction = 0.995;
            cc[2].MoleFraction = 0.0;

            T.Celsius = 14.6;
            P = 9.7;

            HL = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);
            IdealHL = IdealGas.StreamIdealGasMolarEnthalpy(cc, T, cc.MoleFractions);
            HFormL = ThermodynamicsClass.EnthalpyFormation25(cc, cc.MoleFractions);
            HDepL = EnthalpDepClass.HDeparture(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);
            resL = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);

            COMColumnNS.COMThermo com = new();

            Enthalpy res1 = com.LiqEnthalpyRealAndQuasiFixedProps(cc.NameArray, cc.MoleFractions, T, P,
                cc.MoleFractions, cc.MoleFractions, cc.MoleFractions, cc.MoleFractions, cc.MoleFractions, cc.MoleFractions, cc.MoleFractions, 5, false);

            // 2954.57

            cc[0].MoleFraction = 0.168134519;
            cc[1].MoleFraction = 0.831845987;
            cc[2].MoleFraction = 1.94933E-05;

            T.Celsius = 15.74;
            P = 8.7;

            HL = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);
            IdealHL = IdealGas.StreamIdealGasMolarEnthalpy(cc, T, cc.MoleFractions);
            HFormL = ThermodynamicsClass.EnthalpyFormation25(cc, cc.MoleFractions);
            HDepL = EnthalpDepClass.HDeparture(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);
            resL = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);

            Debug.Print("H " + H.ToString());
        }

        [TestMethod]
        public void TestEnthalpyC3()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;
            Components cc = new();
            BaseComp sc;
            GibbsReactor RX = new();
            Port_Material port = new Port_Material();
            Pressure P = new Pressure(8, PressureUnit.BarG);
            //Temperature T = new Temperature(15.23, TemperatureUnit.Celsius);
            Quality Q = new Quality(0.1234);

            sc = Thermodata.GetComponent("Propane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            cc.NormaliseFractions();

            port.cc = cc;
            port.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            //port.T = new(Units.ePropID.P, T, SourceEnum.Input);
            port.Q_ = new(Units.ePropID.Q, Q, SourceEnum.Input);
            port.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Input;
            port.cc.Thermo.UseBIPs = false;
            port.ClearNonInputs();
            port.Flash();

            double res = port.H_;
        }

        [TestMethod]
        public void TestEnthalpyC3E()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;
            Components cc = new();
            BaseComp sc;
            GibbsReactor RX = new();
            Port_Material port = new Port_Material();
            Pressure P = new Pressure(8, PressureUnit.BarG);
            //Temperature T = new Temperature(15.23, TemperatureUnit.Celsius);
            Quality Q = new Quality(0.1234);

            sc = Thermodata.GetComponent("Propene");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            cc.NormaliseFractions();

            port.cc = cc;
            port.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            //port.T = new(Units.ePropID.P, T, SourceEnum.Input);
            port.Q_ = new(Units.ePropID.Q, Q, SourceEnum.Input);
            port.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Input;
            port.cc.Thermo.UseBIPs = false;
            port.ClearNonInputs();
            port.Flash();

            double res = port.H_;
        }

        [TestMethod]
        public void iC4()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;
            Components cc = new();
            BaseComp sc;
            GibbsReactor RX = new();
            Port_Material port = new Port_Material();
            Pressure P = new Pressure(8, PressureUnit.BarG);
            //Temperature T = new Temperature(15.23, TemperatureUnit.Celsius);
            Quality Q = new Quality(0.1234);

            sc = Thermodata.GetComponent("i-Butane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            cc.NormaliseFractions();

            port.cc = cc;
            port.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            //port.T = new(Units.ePropID.P, T, SourceEnum.Input);
            port.Q_ = new(Units.ePropID.Q, Q, SourceEnum.Input);
            port.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Input;
            port.cc.Thermo.UseBIPs = false;
            port.ClearNonInputs();
            port.Flash();

            double res = port.H_;
        }

        [TestMethod]
        public void TestEnthalpyC2_C3()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;
            Components cc = new();
            BaseComp sc;
            GibbsReactor RX = new();
            Port_Material port = new Port_Material();
            Pressure P = new Pressure(1);
            Temperature T = new Temperature(1);

            sc = Thermodata.GetComponent("Ethane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("Propane");
            cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.5;

            cc.NormaliseFractions();

            port.cc = cc;
            port.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
            port.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Input;
            port.cc.Thermo.UseBIPs = false;

            port.ClearNonInputs();
            List<Enthalpy> Hs = new();

            T = 1;
            port.T_ = new(Units.ePropID.T, T, SourceEnum.Input);
            for (int i = 0; i < 400; i++)
            {
                port.T = T;
                port.Flash();
                Debug.Print(" T: " + T.Celsius.ToString("0.00") + " " + port.H_.ToString() + " Q: " + port.Q_.ToString());
                Hs.Add(port.H_.BaseValue);
                port.ClearNonInputs();
                T += 1;
            }

            port.T = 273.15 + 57.85;
            port.Flash();
            Debug.Print(" T: " + T.Celsius.ToString("0.0") + " " + port.H_.ToString() + " Q: " + port.Q_.ToString());

            port.T_.Clear();
            port.H_ = new StreamProperty(ePropID.H, 0, SourceEnum.Input);
            for (int i = 0; i < 400; i++)
            {
                port.H = Hs[i];
                port.Flash();
                Debug.Print(" H: " + Hs[i].ToString() + " T: " + port.T_.ToString());
                Hs.Add(port.H_.BaseValue);
                port.ClearNonInputs();
                T += 1;
            }
        }

        [TestMethod]
        public void TestEnthalpyC4()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;
            Components cc = new();
            BaseComp sc;
            GibbsReactor RX = new();
            Port_Material port = new Port_Material();
            Pressure P = new Pressure(1);
            Temperature T = new Temperature(1);

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            port.cc = cc;
            port.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
            port.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Input;
            port.cc.Thermo.UseBIPs = false;

            port.ClearNonInputs();
            List<Enthalpy> Hs = new();

            T = 1;
            port.T_ = new(Units.ePropID.T, T, SourceEnum.Input);
            for (int i = 0; i < 400; i++)
            {
                port.T = T;
                port.Flash();
                Debug.Print(" T: " + T.Celsius.ToString("0.00") + " " + port.H_.ToString() + " Q: " + port.Q_.ToString());
                Hs.Add(port.H_.BaseValue);
                port.ClearNonInputs();
                T += 1;
            }

            port.T = 351;
            port.Flash();
            double Enthalpy = port.H_;
            port.T_.Clear();
            port.H_ = new StreamProperty(ePropID.H, Enthalpy, SourceEnum.Input);
            port.Flash();

            Debug.Print(" T: " + T.Celsius.ToString("0.0") + " " + port.H_.ToString() + " Q: " + port.Q_.ToString());

            port.T_.Clear();
            port.H_ = new StreamProperty(ePropID.H, 0, SourceEnum.Input);
            for (int i = 0; i < 400; i++)
            {
                port.H = Hs[i];
                port.Flash();
                Debug.Print(" H: " + Hs[i].ToString() + " T: " + port.T_.ToString());
                Hs.Add(port.H_.BaseValue);
                port.ClearNonInputs();
                T += 1;
            }
        }

        public EnthalpyDepartureLinearisation LinearDepEnthalpies = new();
        public EnthalpySimpleLinearisation SimpleLinear = new();

        [TestMethod]
        public void TestLinearEnthalpy()
        {
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.PR78;
            Components cc = DefaultStreams.ShortResidue3();
            Temperature T = new Temperature(273.15 + 25);
            Pressure P = new Pressure(1);
            double dt = 50;

            SimpleLinear.LiqUpdate(cc, cc.MoleFractions, P, T, T + 1, thermo);

            var res1 = SimpleLinear.LiqEstimate(T, cc.MW(cc.MoleFractions));
            var res2 = SimpleLinear.LiqEstimate(T + dt, cc.MW(cc.MoleFractions));

            var resultRig1 = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P.BaseValue, T, enumFluidRegion.Liquid, thermo);
            var resultRig2 = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P.BaseValue, T + dt, enumFluidRegion.Liquid, thermo);

            var CP1 = (res2 - res1) / dt;
            var CP2 = (resultRig2 - resultRig1) / dt;
        }

        [TestMethod]
        public void TestLinearEnthalpyDeparture()
        {
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.PR78;
            Components cc = DefaultStreams.ShortResidue3();
            cc.NormaliseFractions();
            Temperature T = new Temperature(273.15 + 25);
            Pressure P = new Pressure(1);
            double dt = 50;

            LinearDepEnthalpies.LiqUpdate(cc, cc.MoleFractions, P, T, T + dt, thermo);
            var res1 = LinearDepEnthalpies.LiqDepEstimate(cc, cc.MoleFractions, T);
            var res2 = LinearDepEnthalpies.LiqDepEstimate(cc, cc.MoleFractions, T + dt);

            var res3 = LinearDepEnthalpies.LiqEnthalpy(cc, cc.MoleFractions, P, T);
            var res4 = LinearDepEnthalpies.LiqEnthalpy(cc, cc.MoleFractions, P, T + dt);

            var res = ThermodynamicsClass.EnthalpyFormation25(cc, cc.MoleFractions);

            var resultRig1 = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P.BaseValue, T, enumFluidRegion.Liquid, thermo);
            var resultRig2 = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P.BaseValue, T + dt, enumFluidRegion.Liquid, thermo);

            var CP1 = (res4 - res3) / dt;
            var CP2 = (resultRig2 - resultRig1) / dt;
        }

        [TestMethod]
        public void TestVapourLinearEnthalpyDeparture()
        {
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.PR78;
            Components cc = DefaultStreams.ShortResidue3();
            Temperature T = new Temperature(273.15 + 25);
            Pressure P = new Pressure(1);
            double dt = 50;

            LinearDepEnthalpies.VapUpdate(cc, cc.MoleFractions, P, T, T + dt, thermo);
            var res1 = LinearDepEnthalpies.VapDepEstimate(cc, cc.MoleFractions, T);
            var res2 = LinearDepEnthalpies.VapDepEstimate(cc, cc.MoleFractions, T + dt);

            var res3 = LinearDepEnthalpies.VapEnthalpy(cc, cc.MoleFractions, P, T);
            var res4 = LinearDepEnthalpies.VapEnthalpy(cc, cc.MoleFractions, P, T + dt);

            var resultRig1 = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P.BaseValue, T, enumFluidRegion.Vapour, thermo);
            var resultRig2 = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P.BaseValue, T + dt, enumFluidRegion.Vapour, thermo);

            var CP1 = (res4 - res3) / dt;
            var CP2 = (resultRig2 - resultRig1) / dt;
        }

        [TestMethod]
        public void TestResidueEnthalpy()
        {
            Components cc = DefaultStreams.Residue();

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.UseBIPs = true;

            cc.SetMolFractions(new double[] { 0.000224977043788792, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2.79903954669966E-06, 8.95545394387812E-06, 0, 3.16998559964643E-06, 7.29331157896565E-06, 8.47959714748754E-06, 0.000014730817975791, 3.97813241133139E-05, 5.24634895359301E-05, 0.000128892530756838, 6.13358407944432E-05, 0.000193141943211708, 0.000201310114814503, 0.000348375447859634, 0.000427515943964246, 0.000624182044609985, 0.000767248126562134, 0.00114131121478601, 0.0012377579511817, 0.00134277175968445, 0.00229034960442445, 0.00407049147009185, 0.00515351404421528, 0.00660186037144042, 0.0142760549093449, 0.0113587945470787, 0.0233917533669569, 0.0409460662309939, 0.0379760801263419, 0.0741172690221668, 0.0879658686800503, 0.170113525586903, 0.0971025535617221, 0.167279033205303, 0.102273246797598, 0.0842054840262652, 0.0414328894183491, 0.0152680412797172, 0.00412906735543702, 0.00232422996417874, 0.000613993242497766, 0.000199584099143722, 5.77756404279783E-05, 1.20280116492651E-05, 3.48038098868031E-06, 4.57533126951494E-07, 1.3785017145733E-08, 6.94908069325511E-10, 1.67191984730162E-11, 5.59284144016529E-13, 6.30362707868515E-14, 8.50961821733026E-12, 3.34399135382889E-12, 6.62564485401165E-12, 1.67382914110085E-12, 1.67375400367903E-13, 2.64162871884797E-14, 4.15569937193642E-15, 2.0437086034659E-11, 4.04925362565638E-12, 2.49795979668467E-14, 1.32596537245029E-16 });

            TemperatureC T = new(25);
            Pressure P = 0.4;
            cc.NormaliseFractions();
            cc.Origin = SourceEnum.Input;

            Port_Material p = new(cc);
            p.T_ = new(ePropID.T, T, SourceEnum.Input);
            p.P_ = new(ePropID.P, P, SourceEnum.Input);

            p.Flash(enumFlashType.PT, thermo: thermo);

            var res = p.StreamEnthalpy();
            Debug.Print(res.ToString());

            var result = ThermodynamicsClass.BulkStreamEnthalpy(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);
        }

        [TestMethod]
        public void TestUralsAssayEnthalpy()
        {
            Components cc = DefaultStreams.CrudeUrals();

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.UseBIPs = true;

            TemperatureC T = 25.609265;
            Pressure P = 20;

            cc.NormaliseFractions();
            cc.Origin = SourceEnum.Input;

            Port_Material p = new(cc);
            p.T_ = new(Units.ePropID.T, T, SourceEnum.Input);
            p.P_ = new(Units.ePropID.P, P, SourceEnum.Input);

            p.Flash(enumFlashType.PT, thermo: thermo);

            double Enthlpy = -412227.5017; // p.H;

            p.T_.Clear();
            p.H_ = new(Units.ePropID.H, Enthlpy, SourceEnum.Input);
            p.P_ = new(Units.ePropID.P, P, SourceEnum.Input);

            p.Flash(enumFlashType.PH, thermo: thermo);

            var res = cc.StreamEnthalpy(p.Q);
            Debug.Print(res.ToString());
            Debug.Print(cc.CP_MASS(P, T, p.Q).ToString());
        }

        [TestMethod]
        public void TestLeeKeslerEnthalpy2()
        {
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = true;
            Components cc = new();

            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");

            cc.Add(H2O);
            cc.Add(MEK);
            cc.Add(PropionicAcid);

            cc.SetMolFractions(new double[] { 0.978474108, 0.021501388, 0.000024504 });
            cc.Origin = SourceEnum.Input;

            TemperatureC T = 273.15; // + 22.586;
            Pressure P = 1;

            Port_Material p = new(cc);
            p.Flash(enumFlashType.PT);

            var Res2 = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo).H;
            var Res3 = p.StreamEnthalpy();
            Debug.Print(Res2.ToString());
            Debug.Print(Res3.ToString());
        }

        [TestMethod]
        public void TestLeeKeslerEnthalpy()
        {
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = true;
            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            sc.MoleFraction = 1;
            cc.Add(sc);
            cc.Origin = SourceEnum.Input;

            TemperatureC T = 273.15;
            Pressure P = 5;

            Port_Material p = new(cc);
            p.Flash(enumFlashType.PT);

            var Res2 = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo).H;
            var res3 = p.StreamEnthalpy();
            Debug.Print(Res2.ToString());
            Debug.Print(res3.ToString());
        }

        [TestMethod]
        public void TestLeeKeslerintprivateerpolation()

        {
            double[,] table = LeeKesler.Z0Liquid;
            double trval = 0.375, prval = 0.04;
            var res = LeeKesler.Bilinearinterpolation(table, trval, prval, out List<double> Ts, out List<double> Ps, out List<double> Zs);

            trval = 0.001; prval = 0.6;
            res = LeeKesler.Bilinearinterpolation(table, trval, prval, out Ts, out Ps, out Zs);
        }

        [TestMethod]
        public void TestEnthalpy4Comps()
        {
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = true;
            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("propane");
            sc.MoleFraction = 0.5;
            cc.Add(sc);

            sc = Thermodata.GetComponent("n-Butane");
            sc.MoleFraction = 0.4;
            cc.Add(sc);

            sc = Thermodata.GetComponent("n-Pentane");
            sc.MoleFraction = 0.08;
            cc.Add(sc);

            //sc = new  PseudoComponent(875 + 273.15, 1.1209, 1158.9180, 1.77649998664856, 992.2591+273.15, 8.15325, 2.1441, thermo);
            sc = new PseudoComponent(1.1209, new Temperature(273.15 + 875), "Test", thermo)
            {
                MoleFraction = 0.02
            };
            cc.Add(sc);

            TemperatureC T = 25 + 273.15;
            Pressure P = 6;

            Port_Material p = new(cc);
            p.Flash(enumFlashType.PT, thermo: thermo);

            var Res2 = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo).H;
            var res3 = p.StreamEnthalpy();
            Debug.Print(Res2.ToString());
            Debug.Print(res3.ToString());
        }

        [TestMethod]
        public void TestLeeKeslerCp()
        {
            Temperature Tk = new(273.15 + 250);
            double[] res = LeeKesler.GetIdealVapCpCoefficients(0.3332, 12.01, 0.7454);
            double Cp = res[1] + res[2] * Tk + res[3] * Tk.Pow(2); // 2.48044
            Debug.Print(Cp.ToString());
        }

        [TestMethod]
        public void TestFlash()
        {
            ThermoDynamicOptions thermo = new();
            Components cc = new();
            BaseComp sc;

            thermo.KMethod = enumEquiKMethod.PR78;

            sc = Thermodata.GetComponent("N-Butane");
            cc.Add(sc);

            sc = Thermodata.GetComponent("propane");
            cc.Add(sc);

            sc = Thermodata.GetComponent("n-C30");
            cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.4;
            cc[2].MoleFraction = 0.1;

            TemperatureC T = 273.15 + 10;
            Pressure P = 3;

            Port_Material p = new(cc);
            p.Thermo = thermo;
            p.Flash(enumFlashType.PT);

            double[,] res = new double[2, cc.ComponentList.Count];

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                res[0, i] = cc.LiqPhaseMolFractions[i];
            }

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                res[1, i] = cc.VapPhaseMolFractions[i];
            }
        }

        [TestMethod]
        public void TestAssay()
        {
            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();
            Pressure P = new(); Temperature T = new(); Quality Q = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0, 0, 0, 0, 0, 0.004547067, 0, 0.018074291, 0, 0.00099159, 0.0009, 0.010643269, 0.020345064, 0.012299614,
                0.026398419, 0.042048069 };

            double[] QuasiFractions = new double[] {0.00049863,0.008020049,0.019461243,0.014430737,0.015168741,0.026853189,0.027342387,0.044394572,0.015071084,
                0.035868414,0.026714637,0.032299178,0.027497042,0.027870072,0.024231411,0.025618686,0.019434524,0.014882236,0.017338558,0.021172108,0.01813969,
                0.015803173,0.022931753,0.012160108,0.016425096,0.019225045,0.011943069,0.015985843,0.013523616,0.019728995,0.009311229,0.015344892,0.011103278,
                0.01418812,0.013216524,0.011124846, 0.00765835,0.012029387,0.0098145,0.010369175,0.010123575,0.007566189,0.011291951,0.017426168,0.010196898,
                0.010759995,0.009204489,0.005670386,0.007867202,0.003149339,0.005107855,0.002719051,0.0059036,0.004733464,0.005576194,0.006102976,0.004217355,
                0.008018429,0.00218887,0.001740744};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.UseBIPs = false;

            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new(BPk[i], SG[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            T.Celsius = 370;
            P.BarA = 1;
            cc.NormaliseFractions();

            Port_Material p = new(cc);
            p.Flash(enumFlashType.PT, thermo: thermo);

            var res = cc.StreamEnthalpy(Q);
            Debug.Print(res.ToString());
            Debug.Print(cc.CP_MASS(P, T, Q).ToString());
        }

        [TestMethod]
        public void TestQuasiComponent()
        {
            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            Pressure P = new(); Temperature T = new(); Quality Q = new();

            string[] RealNames = System.Array.Empty<string>();

            string[] QuasiNames = new string[] { "Quasi875*" };

            double[] RealFractions = new double[] { 0, 0, 0, 0, 0, 0.004547067, 0, 0.018074291, 0, 0.00099159, 0.0009, 0.010643269, 0.020345064, 0.012299614,
                0.026398419, 0.042048069 };

            double[] QuasiFractions = new double[] {0.00049863,0.008020049,0.019461243,0.014430737,0.015168741,0.026853189,0.027342387,0.044394572,0.015071084,
                0.035868414,0.026714637,0.032299178,0.027497042,0.027870072,0.024231411,0.025618686,0.019434524,0.014882236,0.017338558,0.021172108,0.01813969,
                0.015803173,0.022931753,0.012160108,0.016425096,0.019225045,0.011943069,0.015985843,0.013523616,0.019728995,0.009311229,0.015344892,0.011103278,
                0.01418812,0.013216524,0.011124846, 0.00765835,0.012029387,0.0098145,0.010369175,0.010123575,0.007566189,0.011291951,0.017426168,0.010196898,
                0.010759995,0.009204489,0.005670386,0.007867202,0.003149339,0.005107855,0.002719051,0.0059036,0.004733464,0.005576194,0.006102976,0.004217355,
                0.008018429,0.00218887,0.001740744};

            double[] SG = new double[] { 1.1209 };

            double[] BPk = new double[] { 1148.15 };

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;

            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new(BPk[i], SG[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc[0].MoleFraction = 1;

            Port_Material p = new(cc);
            p.T_ = new StreamProperty(ePropID.T, 370, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 1, SourceEnum.Input);
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.Flash(calcderivatives: true);

            var res = cc.StreamEnthalpy(Q);
            Debug.Print(cc.CP_MASS(P, T, Q).ToString());
            Debug.Print(res.ToString());
        }

        [TestMethod]
        public void TestQuasiComponen2()
        {
            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            Pressure P = new(); Quality Q = new();

            Density SG = new(1062, DensityUnit.kg_m3);
            Temperature T = new Temperature(900, TemperatureUnit.Celsius);

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;

            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;

            pc = new(T, SG, thermo);
            pc.MoleFraction = 1;
            cc.Add(pc);

            Port_Material p = new(cc);
            p.T_ = new StreamProperty(ePropID.T, 370, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 1, SourceEnum.Input);
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.Flash(calcderivatives: true);

            var res = cc.StreamEnthalpy(Q);
            Debug.Print(cc.CP_MASS(P, T, Q).ToString());
            Debug.Print(res.ToString());
        }

        [TestMethod]
        public void TestQuasiEnthalpy()
        {
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR76;
            thermo.KMethod = enumEquiKMethod.PR76;

            enumFluidRegion stateVap = enumFluidRegion.Vapour;
            Components cc = new();

            PseudoComponent pc = new(0.6150, new Temperature(273.15 + 38), "Test", thermo);
            pc.Name = "PC" + cc.ComponentList.Count.ToString();
            pc.MoleFraction = 1;
            cc.Add(pc);

            X[0] = 1;

            ThermodynamicsClass.CheckState(cc, X, new Temperature(273), new Pressure(1), thermo);

            double res = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(6), new Temperature(273.15 + 250), stateVap, thermo);
            double res2 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(6), new Temperature(273.15 + 251), stateVap, thermo);
            double Cp = res2 - res;
            Debug.Print(Cp.ToString());
        }

        [TestMethod]
        public void TestRealEnthalpyCO()
        {
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            enumFluidRegion state = enumFluidRegion.Vapour;
            double res1;
            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("CO");
            cc.Add(sc);

            X[0] = 1;
            res1 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(1), new Temperature(273.15 + 25), state, thermo);
            Debug.Print(res1.ToString());

            X[0] = 1;
            res1 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(1), new Temperature(1000), state, thermo);
            Debug.Print(res1.ToString());
        }

        [TestMethod]
        public void TestRealEnthalpy()
        {
            double[] X = new double[2];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            enumFluidRegion state = enumFluidRegion.Vapour;
            double res1, res2;
            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            sc = Thermodata.GetComponent("propane");
            cc.Add(sc);

            X[0] = 0.5;
            X[1] = 0.5;

            res1 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(3), new Temperature(275.15 + 25), state, thermo);

            X[0] = 1;
            X[1] = 0;

            res2 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(3), new Temperature(275.15 + 25), state, thermo);

            X[0] = 0;
            X[1] = 1;

            double res3 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(3), new Temperature(275.15 + 25), state, thermo);

            Debug.Print(res1.ToString());
            Debug.Print(res2.ToString());
            Debug.Print(res3.ToString());
        }

        [TestMethod]
        public void TestRealLiquidEnthalpyWithWater()
        {
            double[] X = new double[2];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            enumFluidRegion state = enumFluidRegion.Liquid;
            double res1, res2, res3;
            Components cc = new();
            BaseComp sc;
            Pressure P = 1;

            sc = Thermodata.GetComponent("H2O");
            cc.Add(sc);

            sc = Thermodata.GetComponent("M-E-Ketone");
            cc.Add(sc);

            X[0] = 0.999;
            X[1] = 0.001;

            res1 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, P, new Temperature(273.15 + 25), state, thermo);

            X[0] = 0.991;
            X[1] = 0.009;

            res2 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, P, new Temperature(273.15 + 25), state, thermo);

            X[0] = 0.99;
            X[1] = 0.01;

            res3 = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, P, new Temperature(273.15 + 25), state, thermo);

            Debug.Print(res1.ToString());
            Debug.Print(res2.ToString());
            Debug.Print(res3.ToString());
        }

        [TestMethod]
        public void TestCOMEnthalpyPureHexane()
        {
            COMThermo ethermo = new();

            double[] X = new double[1];
            string[] names = new string[2];

            names[0] = "n-hexane";

            X[0] = 1;

            double res = ethermo.LiqEnthalpyReal(names, X, new Temperature(273.15 + 25), new Pressure(4), (int)enumEnthalpy.PR78);
            double res2 = ethermo.VapEnthalpyReal(names, X, new Temperature(273.15 + 25), new Pressure(4), (int)enumEnthalpy.PR78);
            Debug.Print(res.ToString() + " " + res2.ToString());
        }

        [TestMethod]
        public void TestCOMEnthalpyPureWater()
        {
            COMThermo ethermo = new();

            double[] X = new double[1];
            string[] names = new string[2];

            names[0] = "H2O";

            X[0] = 1;

            double res = ethermo.LiqEnthalpyReal(names, X, new Temperature(273.15 + 25), new Pressure(4), (int)enumEnthalpy.PR78);
            double res2 = ethermo.VapEnthalpyReal(names, X, new Temperature(273.15 + 25), new Pressure(4), (int)enumEnthalpy.PR78);
            Debug.Print(res.ToString() + " " + res2.ToString());
        }

        [TestMethod]
        public void TestPCEnthalpy2()
        {
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.Ideal;
            thermo.KMethod = enumEquiKMethod.PR76;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            enumFluidRegion stateVap = enumFluidRegion.Vapour;
            Components cc = new();

            PseudoComponent pc = new(new Temperature(600), 0.9706, thermo)
            {
                Name = "PC" + cc.ComponentList.Count.ToString(),
                MoleFraction = 1
            };
            cc.Add(pc);

            X[0] = 1;

            ThermodynamicsClass.CheckState(cc, X, new Temperature(1000), new Pressure(6), thermo);

            double res = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(6), new Temperature(100), stateVap, thermo);
            TemperatureC T = new Temperature(100);
            double res3 = IdealGas.StreamIdealGasMolarEnthalpy(cc, T, cc.MoleFractions);

            Debug.Print(res.ToString());
            Debug.Print(res3.ToString());
        }

        [TestMethod]
        public void TestVapEnthalpyIdeal()
        {
            Components cc = new();
            BaseComp sc;

            string[] c = new string[]{"H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
            "n-Butane","i-Pentane","n-Pentane","Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
            "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
            "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
            "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
            "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
            "Quasi825*","Quasi875*"};

            //double [] x = new  double []  {0.11962, 0.00190, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01813, 0.00000, 0.00099, 0.00000, 0.01070, 0.02054, 0.01243, 0.02686, 0.04282, 0.00051, 0.00818, 0.01996, 0.01489, 0.01575, 0.02810, 0.02886, 0.04733, 0.01624, 0.03913, 0.02958, 0.03639, 0.03162, 0.03278, 0.02920, 0.03165, 0.02464, 0.01935, 0.02301, 0.02842, 0.02424, 0.02049, 0.02778, 0.01301, 0.01428, 0.01195, 0.00419, 0.00193, 0.00027, 0.00004, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000 };
            double[] x = new double[] { 0.135732774414621, 0.00215468506415423, 0, 0, 0, 0.0040308022232848, 0, 0.0205695504978483, 0, 0.00112867577145799, 0, 0.012145088837949, 0.0233015561421689, 0.0141024421399901, 0.0304793692227981, 0.0485837079279351, 0.000575215436192718, 0.0092822005770224, 0.0226435863400046, 0.0168918871952106, 0.0178767122687875, 0.0318902910790664, 0.0327534194713751, 0.0537023657943689, 0.0184311210311017, 0.0444009322636192, 0.0335596469258279, 0.0412962160913424, 0.0358771869940697, 0.0371930155690997, 0.0331300014439141, 0.035914641309358, 0.0279557907376905, 0.021952113307905, 0.0261134905810872, 0.0322528765686226, 0.0275004545362876, 0.0232519132782944, 0.0315204595917805, 0.0147578354952504, 0.0162052371926885, 0.0135548574764113, 0.00474909354016855, 0.00219263685340534, 0.000302596460158383, 4.18107775868331E-05, 1.53775892680625E-06, 1.91804783680153E-07, 1.07983510399437E-08, 1.11511408933161E-09, 8.63899097261722E-11, 6.12581908949474E-12, 3.54050464074921E-13, 4.58746710586419E-14, 3.03699384603848E-15, 2.51208135433161E-16, 1.86238776871088E-17, 1.02074204103091E-18, 2.81077119585737E-20, 1.81203359796247E-22, 3.63679596787214E-25, 1.09412087254464E-27, 2.22854161297157E-30 };
            double[] sg = new double[] { 1, 0.06992, 0.80712, 0.80013, 1.13874, 0.29967, 0.38358, 0.35601, 0.82610, 0.78914, 0.52144, 0.50715, 0.56249, 0.58377, 0.62402, 0.63031, 0.61505, 0.63181, 0.65515, 0.67719, 0.69724, 0.71462, 0.72864, 0.73863, 0.74390, 0.74539, 0.75097, 0.76004, 0.77054, 0.78041, 0.78757, 0.79003, 0.79329, 0.80040, 0.80916, 0.81738, 0.82290, 0.82423, 0.82671, 0.83135, 0.83742, 0.84419, 0.85095, 0.85696, 0.86150, 0.86383, 0.86476, 0.86833, 0.87408, 0.88118, 0.88880, 0.89612, 0.90231, 0.90654, 0.91222, 0.91518, 0.91866, 0.92236, 0.92771, 0.93286, 0.93419, 0.93675, 0.94200, 0.94962, 0.95925, 0.97056, 0.98320, 0.99685, 1.01116, 1.02553, 1.03532, 1.04510, 1.05489, 1.07201, 1.09648, 1.12094 };
            double[] bp = new double[] { 373.15, 20.5548, 77.3498, 81.6996, 90.1996, 111.625, 169.399, 184.55, 194.59801, 213.498, 225.399, 231.048, 261.42001, 272.6480103, 301.02802, 309.20901, 311.15, 318.15, 328.15, 338.15, 348.15, 358.15, 368.15, 378.15, 388.15, 398.15, 408.15, 418.15, 428.15, 438.15, 448.15, 458.15, 468.15, 478.15, 488.15, 498.15, 508.15, 518.15, 528.15, 538.15, 548.15, 558.15, 568.15, 578.15, 588.15, 598.15, 608.15, 618.15, 628.15, 638.15, 648.15, 658.15, 668.15, 678.15, 688.15, 698.15, 708.15, 718.15, 733.15, 753.15, 773.15, 793.15, 813.15, 833.15, 853.15, 873.15, 893.15, 913.15, 933.15, 953.15, 973.15, 993.15, 1013.15, 1048.15, 1098.15, 1148.15 };

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.Ideal;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(new Temperature(bp[i]), sg[i], thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            TemperatureC T = new Temperature(179.8568);
            Pressure P = new Pressure(6);
            double res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo).H_ig;
            var res2 = IdealGas.StreamIdealGasMolarEnthalpy(cc, T, cc.MoleFractions);
            Debug.Print(res.ToString());
            Debug.Print(res2.ToString());
            return;
        }
    }
}