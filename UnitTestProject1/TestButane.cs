using EngineThermo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Units;

namespace UnitTests
{
    [TestClass]
    public class TestButane
    {

        [TestMethod]
        public void TestKMix()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.RK;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new Components();
            BaseComp sc;

            double pcrit = LeeKesler.PCrit(1.1209, 1148);

            sc = Thermodata.GetRealComponent("Propane");
            cc.Add(sc);
            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);

            thermo.CritTMethod = enumCritTMethod.LeeKesler;
            thermo.CritPMethod = enumCritPMethod.LeeKesler;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            sc = new PseudoComponent(1148, 1.1209, thermo);
            cc.Add(sc);

            cc[0].molefraction = 0.5;
            cc[1].molefraction = 0.5;
            cc[2].molefraction = 0;

            cc.T = new Temperature(273.15);
            cc.P = new Pressure(1);
            res = Thermodynamics.LnFugMix(cc, state);
        }

        [TestMethod]
        public void TestMixtureFugacity()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.RK;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new Components();
            BaseComp sc;

            double pcrit = LeeKesler.PCrit(1.1209, 1148);

            sc = Thermodata.GetRealComponent("Propane");
            cc.Add(sc);
            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);

            thermo.CritTMethod = enumCritTMethod.LeeKesler;
            thermo.CritPMethod = enumCritPMethod.LeeKesler;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            sc = new PseudoComponent(1148, 1.1209, thermo);
            cc.Add(sc);

            cc[0].molefraction = 0.5;
            cc[1].molefraction = 0.5;
            cc[2].molefraction = 0;

            cc.T = new Temperature(273.15);
            cc.P = new Pressure(1);
            res = Thermodynamics.LnFugMix(cc, state);
        }



        [TestMethod]
        public void TestWaterLKEnthalpy()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("H2O");
            cc.Add(sc);

            X[0] = 1;

            double Tr = 0.3;
            double Pr = 0.3;

            enumFluidRegion state = enumFluidRegion.Vapour;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 1; i++)
            {
                var zz0 = LeeKesler.Z0_BisectTable(Tr, Pr, state);
                var zz1 = LeeKesler.Z1_BisectTable(Tr, Pr, state);
            }
            watch.Stop();
            Debug.Print(watch.Elapsed.ToString());


            watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 1; i++)
            {
                var zz0 = LeeKesler.Z0_Rig(Tr, Pr, state);
                var zz1 = LeeKesler.Z1_Rig(Tr, Pr, state);
            }
            watch.Stop();
            Debug.Print(watch.Elapsed.ToString());


            double res = LeeKesler.EnthDeparture(Tr, Pr, 0.201, state) * 8.3145 * 425.15;

            res = Thermodynamics.BulkEnthalpyMix(cc, X, new Temperature(300), new Pressure(10), state);
        }

        [TestMethod]
        public void TestButaneLKEnthalpy()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;


            double res = 0;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);

            X[0] = 1;

            double Tr = 1.05;
            double Pr = 0.01;

            //double Tr = 0.7057;
            //double Pr = 0.26316; 

            //Tr = 0.99210;
            //Pr = 0.94810;

            //Tr = 0.3;
            //Pr = 11;

            //Tr = 0.806;
            //Pr = 0.211;

            //Tr = 0.9921;
            //Pr = 0.9481;
            //Tr = 0.998;
            //Pr = 0.986;

            Tr = 0.818;
            Pr = 0.237;

            Tr = 0.929;
            Pr = 0.606;

            Tr = 0.687; Pr = 0.053;

            Tr = 1.000; Pr = 0.998;

            Tr = 0.929; Pr = 0.606;

            Tr = 0.984; Pr = 0.895;

            Tr = 0.6408; Pr = 0.0263;

            Tr = 0.99704; Pr = 0.97972;
            Tr = 0.983664532; Pr = 0.895443771;
            Tr = 0.3; Pr = 0.01;
            Tr = 0.93; Pr = 0.01;


            enumFluidRegion state = enumFluidRegion.Vapour;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 10000; i++)
            {
                var zz0 = LeeKesler.Z0_BisectTable(Tr, Pr, state);
                var zz1 = LeeKesler.Z1_BisectTable(Tr, Pr, state);
            }
            watch.Stop();
            Debug.Print(watch.Elapsed.ToString());


            watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 10000; i++)
            {
                var zz0 = LeeKesler.Z0_Rig(Tr, Pr, state);
                var zz1 = LeeKesler.Z1_Rig(Tr, Pr, state);
            }
            watch.Stop();
            Debug.Print(watch.Elapsed.ToString());


            res = LeeKesler.EnthDeparture(Tr, Pr, 0.201, state) * 8.3145 * 425.15;

            res = Thermodynamics.BulkEnthalpyMix(cc, X, new Temperature(300), new Pressure(10), state);
        }

        [TestMethod]
        public void TestButaneCOMEnthalpy()
        {
            Excel_Thermo et = new Excel_Thermo();

            var res = et.VapEnthalpyReal("Propane", 1, 273.15 + 150, 6, 4);
        }

        [TestMethod]
        public void TestButanePortEnthalpy()
        {
            Thermodata data = new Thermodata();
            Port_Material port = new Port_Material();

            port.components.Add("n-Butane");
            port.components.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.P, SourceEnum.Input, 7);
            port.SetPortValue(ePropID.T, SourceEnum.Input, 64 + 273.15);
            port.Flash();
            Debug.Print(port.H.ToString());

            Debug.Print(port.components.CP_MASS().ToString());

            double H = port.H.BaseValue;

            port.T.Clear();
            port.SetPortValue(ePropID.H, SourceEnum.Input, H);
            port.Flash();
            port.Flash();
            Debug.Print(port.T.ToString());
        }

        [TestMethod]
        public void TestButanePHFlash()
        {
            Thermodata data = new Thermodata();
            Port_Material port = new Port_Material();

            port.components.Add("n-Butane");
            port.components.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.P, SourceEnum.Input, 7);
            port.SetPortValue(ePropID.H, SourceEnum.Input, -123374);
            port.Flash();
            Debug.Print(port.T.ToString());
        }


        [TestMethod]
        public void TestButanePSFlash()
        {
            Thermodata data = new Thermodata();
            Port_Material port = new Port_Material();

            port.components.Add("n-Butane");
            port.components.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.P, SourceEnum.Input, 7);
            port.SetPortValue(ePropID.S, SourceEnum.Input, -368);
            port.Flash();
            Debug.Print(port.T.ToString());

            double H = port.H;

            port.S.Clear();
            port.SetPortValue(ePropID.H, SourceEnum.Input, H);
            port.Flash();
            Debug.Print(port.S.ToString());

        }


        [TestMethod]
        public void TestButanePSat()
        {
            Thermodata data = new Thermodata();
            Port_Material port = new Port_Material();

            port.components.Add("n-Butane");
            port.components.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.T, SourceEnum.Input, 25 + 273.15);
            port.PSat();
            Debug.Print(port.P.ToString());
        }

        [TestMethod]
        public void TestButaneTSat()
        {
            Thermodata data = new Thermodata();
            Port_Material port = new Port_Material();

            port.components.Add("n-Butane");
            port.components.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.P, SourceEnum.Input, 12);
            port.TSat();
            Debug.Print(port.T.ToString());
        }

        [TestMethod]
        public void TestButaneIdealVapourPressure()
        {
            Thermodata data = new Thermodata();
            Port_Material port = new Port_Material();

            port.components.Add("n-Butane");
            port.components.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.T, SourceEnum.Input, 25 + 273.15);
            port.IdealVapourPressure();
            Debug.Print(port.T.ToString());
        }

        [TestMethod]
        public void TestButaneVapFugacity()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.RK;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);

            cc[0].molefraction = 1;

            cc.T = new Temperature(273.15);
            cc.P = new Pressure(1);
            res = Thermodynamics.LnFugMix(cc, state);
        }

        [TestMethod]
        public void TestButanePentaneDifferentials()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[2];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            enumFluidRegion state = enumFluidRegion.Vapour;

            ThermoDifferentialPropsCollection res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);
            sc = Thermodata.GetRealComponent("n-Pentane");
            cc.Add(sc);

            X[0] = 0.3563;
            X[1] = 1 - 0.3563;

            cc.T = new Temperature(390);
            cc.P = new Pressure(11);
            res = Thermodynamics.ThermoDifferentials(cc, X, thermo, enumMassOrMolar.Molar, state);
        }

        [TestMethod]
        public void TestButanePentaneBulkThermos()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[2];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR76;
            enumFluidRegion state = enumFluidRegion.Vapour;

            ThermoPropsCollection res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);
            sc = Thermodata.GetRealComponent("n-Pentane");
            cc.Add(sc);

            X[0] = 0.3563;
            X[1] = 1 - 0.3563;
            cc.T = new Temperature(390);
            cc.P = new Pressure(11);
            res = Thermodynamics.BulkStreamThermo(cc, cc.T, X, enumMassOrMolar.Molar, state);
        }

        enum Atoms { C, H, S, O, N }
        [TestMethod]
        public void TestButaneFormula()
        {
            Thermodata data = new Thermodata();

            BaseComp sc;
            sc = Thermodata.GetRealComponent("n-Butane");
            Dictionary<string, double> composition = new Dictionary<string, double>();

            string form = sc.Formula.Trim();

            int loc;
            string value;
            bool cont;
            int x;
            int NoofAtoms;

            string[] comps = Enum.GetNames(typeof(Atoms));

            for (int i = 0; i < comps.Count(); i++)
            {
                value = "";
                NoofAtoms = 0;
                x = 1;
                loc = form.IndexOf(comps[i]);
                do
                {
                    cont = false;
                    if (form.Length > loc + x)
                    {
                        value += form[loc + x];
                        if (int.TryParse(value, out int res1))
                        {
                            cont = true;
                            NoofAtoms = res1;
                            x++;
                        }
                        else
                            cont = false;
                    }
                } while (cont);
                composition[comps[i].ToString()] = NoofAtoms;
            }
        }


    }
}
