using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace UnitTests
{
    [TestClass]
    public class TestButane
    {
        [TestMethod]
        public void TestKMix()
        {
            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new();
            BaseComp sc;

            double pcrit = LeeKesler.PCrit(1.1209, 1148);

            sc = Thermodata.GetComponent("Propane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            thermo.CritTMethod = enumCritTMethod.LeeKesler;
            thermo.CritPMethod = enumCritPMethod.LeeKesler;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            //sc = new  PseudoComponent(1148, 1.1209, thermo);

            //cc.Add(sc);

            cc = DefaultStreams.Crude();

            double[] Y = new double[cc.Count];
            Y[0] = 1;

            //cc[0].MoleFraction = 0.5;
            //cc[1].MoleFraction = 0.5;
            //cc[2].MoleFraction = 0;

            TemperatureC T = 180;
            Pressure P = 3;

            res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, Y, out _, cc.Thermo);

            Debug.Print("T = " + T.ToString() + " " + Math.Exp(res[18]).ToString());

            T = 185;
            P = 3;

            res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, Y, out _, cc.Thermo);

            Debug.Print("T = " + T.ToString() + " " + Math.Exp(res[18]).ToString());

            /* for (int  i = 0; i < 300; i++)
             {
                 cc.T = new  Temperature (273.15 + i);
                 res = Thermodynamicsclass .KMix(cc, cc.P, cc.T, cc.MolFractions, cc.MolFractions, cc.Thermo, out _);
                 Debug.Print ("T = " + cc.T.ToString() + " " + Math.Exp(res[18]).ToString());
             }*/
        }

        [TestMethod]
        public void TestfugMix()
        {
            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new();
            BaseComp sc;

            double pcrit = LeeKesler.PCrit(1.1209, 1148);

            sc = Thermodata.GetComponent("Propane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            thermo.CritTMethod = enumCritTMethod.LeeKesler;
            thermo.CritPMethod = enumCritPMethod.LeeKesler;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            //sc = new  PseudoComponent(1148, 1.1209, thermo);
            //cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.5;
            //cc[2].MoleFraction = 0;

            TemperatureC T = 370;
            Pressure P = 3;

            res = ThermodynamicsClass.LnFugMix(cc, P, T, state, cc.Thermo);

            for (int i = 0; i < 100; i++)
            {
                T = new Temperature(273.15 + i);
                res = ThermodynamicsClass.LnFugMix(cc, P, T, state, cc.Thermo);
                Debug.Print("T = " + T.ToString() + " " + Math.Exp(res[0]).ToString());
            }
        }

        [TestMethod]
        public void TestMixtureFugacity()
        {
            Thermodata data = new();

            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.RK;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new Components();
            BaseComp sc;

            double pcrit = LeeKesler.PCrit(1.1209, 1148);

            sc = Thermodata.GetComponent("Propane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            thermo.CritTMethod = enumCritTMethod.LeeKesler;
            thermo.CritPMethod = enumCritPMethod.LeeKesler;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            sc = new PseudoComponent(1148, 1.1209, thermo);
            cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.5;
            cc[2].MoleFraction = 0;

            TemperatureC T = 370;
            Pressure P = 3;

            res = ThermodynamicsClass.LnFugMix(cc, P, T, state, cc.Thermo);
        }

        [TestMethod]
        public void TestWaterLKEnthalpy()
        {
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("H2O");
            cc.Add(sc);

            X[0] = 1;

            double Tr = 0.3;
            double Pr = 0.3;

            enumFluidRegion state = enumFluidRegion.Vapour;
            var watch = Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 1; i++)
            {
                var zz0 = LeeKesler.Z0_BisectTable(Tr, Pr, state);
                var zz1 = LeeKesler.Z1_BisectTable(Tr, Pr, state);
            }
            watch.Stop();
            Debug.Print(watch.Elapsed.ToString());

            watch = Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 1; i++)
            {
                var zz0 = LeeKesler.Z0_Rig(Tr, Pr, state);
                var zz1 = LeeKesler.Z1_Rig(Tr, Pr, state);
            }
            watch.Stop();
            Debug.Print(watch.Elapsed.ToString());

            double res = LeeKesler.EnthDeparture(Tr, Pr, 0.201, state) * 8.3145 * 425.15;

            res = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(10), new Temperature(300), state, thermo);
        }

        [TestMethod]
        public void TestButaneEnthalpy()
        {
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;

            double res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            X[0] = 1;

            enumFluidRegion state = enumFluidRegion.Vapour;

            TemperatureC T = 370;
            Pressure P = 3;

            res = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, P, T, state, thermo);
            cc.ThermoLiqDerivatives = ThermodynamicsClass.UpdateThermoDerivativeProperties(cc, P, T, cc.Thermo, enumFluidRegion.Liquid);
        }

        [TestMethod]
        public void TestButaneEnthalpyLoop()
        {
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            Temperature t = 1;
            Pressure P = 1;

            Port_Material pm = new(cc);
            pm.P = P;

            RachfordRice rr = new(pm, thermo);
            double Q = 0;

            for (int i = 0;i<1000;i++)
            {
                t += 1;
                rr.SolveTP(t, out Q);
                ThermodynamicsClass.UpdateThermoProperties(cc, P, t, cc.Thermo);
                Debug.Print("T: " + t.ToString() + " Q: " + Q.ToString() + " " + cc.H(P, t, Q).ToString());
            }
        }

        [TestMethod]
        public void TestButaneCp()
        {
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;

            double res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            Pressure P = 1;
            Temperature T = new(100, TemperatureUnit.Celsius);

            cc.ThermoLiqDerivatives = ThermodynamicsClass.UpdateThermoDerivativeProperties(cc, P, T, cc.Thermo, enumFluidRegion.Vapour);
        }

        [TestMethod]
        public void TestButaneLKEnthalpy()
        {
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;

            double res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            X[0] = 1;

            double Tr = 1.05;
            double Pr = 0.01;

            //double  Tr = 0.7057;
            //double  Pr = 0.26316;

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
            var watch = Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 10000; i++)
            {
                var zz0 = LeeKesler.Z0_BisectTable(Tr, Pr, state);
                var zz1 = LeeKesler.Z1_BisectTable(Tr, Pr, state);
            }
            watch.Stop();
            Debug.Print(watch.Elapsed.ToString());

            watch = Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 10000; i++)
            {
                var zz0 = LeeKesler.Z0_Rig(Tr, Pr, state);
                var zz1 = LeeKesler.Z1_Rig(Tr, Pr, state);
            }
            watch.Stop();
            Debug.Print(watch.Elapsed.ToString());

            res = LeeKesler.EnthDeparture(Tr, Pr, 0.201, state) * 8.3145 * 425.15;

            res = ThermodynamicsClass.BulkStreamEnthalpy(cc, X, new Pressure(10), new Temperature(300), state, thermo);
        }

        [TestMethod]
        public void TestButaneCOMEnthalpy()
        {
            COMColumnNS.COMThermo et = new();
            var res = et.VapEnthalpyReal("n-Butane", 1, 273.15 + 25, 1, 5);
        }

        [TestMethod]
        public void TestButanePortEnthalpy()
        {
            Port_Material port = new();

            port.cc.Add("n-Butane");
            port.cc.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.P, 7, SourceEnum.Input);
            port.SetPortValue(ePropID.T, 64 + 273.15, SourceEnum.Input);
            port.Flash();
            Debug.Print(port.H_.ToString());

            Debug.Print(port.CP_MASS().ToString());

            double H = port.H_.BaseValue;

            port.T_.Clear();
            port.SetPortValue(ePropID.H, H, SourceEnum.Input);
            port.Flash();
            port.Flash();
            Debug.Print(port.T_.ToString());
        }

        [TestMethod]
        public void TestButanePHFlash()
        {
            Port_Material port = new();

            port.cc.Add("n-Butane");
            port.cc.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.P, 7, SourceEnum.Input);
            port.SetPortValue(ePropID.H, -123374, SourceEnum.Input);
            port.Flash();
            Debug.Print(port.T_.ToString());
        }

        [TestMethod]
        public void TestButanePSFlash()
        {
            Thermodata data = new();
            Port_Material port = new();

            port.cc.Add("n-Butane");
            port.cc.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.P, 7, SourceEnum.Input);
            port.SetPortValue(ePropID.S, -368, SourceEnum.Input);
            port.Flash();
            Debug.Print(port.T_.ToString());

            double H = port.H_;

            port.S_.Clear();
            port.SetPortValue(ePropID.H, H, SourceEnum.Input);
            port.Flash();
            Debug.Print(port.S_.ToString());
        }

        [TestMethod]
        public void TestButanePSat()
        {
            Port_Material port = new();

            port.cc.Add("n-Butane");
            port.cc.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.T, 25 + 273.15, SourceEnum.Input);
            port.PSat();
            Debug.Print(port.P_.ToString());
        }

        [TestMethod]
        public void TestButaneTSat()
        {
            Port_Material port = new();

            port.cc.Add("n-Butane");
            port.cc.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.P, 12, SourceEnum.Input);
            port.TSat();
            Debug.Print(port.T_.ToString());
        }

        [TestMethod]
        public void TestButaneIdealVapourPressure()
        {
            Port_Material port = new();

            port.cc.Add("n-Butane");
            port.cc.SetMolFractions(new double[] { 1 }, SourceEnum.Input);
            port.SetPortValue(ePropID.T, 25 + 273.15, SourceEnum.Input);
            port.IdealVapourPressure();
            Debug.Print(port.T_.ToString());
        }

        [TestMethod]
        public void TestButaneVapFugacity()
        {
            Thermodata data = new();

            double[] X = new double[1];
            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.RK;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double[] res;
            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            TemperatureC T = 370;
            Pressure P = 3;

            res = ThermodynamicsClass.LnFugMix(cc, P, T, state, thermo);
        }

        [TestMethod]
        public void TestButanePentaneDifferentials()
        {
            double[] X = new double[2];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            enumFluidRegion state = enumFluidRegion.Vapour;

            ThermoDifferentialPropsCollection res;
            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("n-Pentane");
            cc.Add(sc);

            X[0] = 0.3563;
            X[1] = 1 - 0.3563;

            Temperature T = new Temperature(390);
            Pressure P = new Pressure(11);
            res = ThermodynamicsClass.ThermoDifferentials(cc, P, T, X, state, thermo);
        }

        [TestMethod]
        public void TestButanePentaneBulkThermos()
        {
            double[] X = new double[2];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR76;
            enumFluidRegion state = enumFluidRegion.Vapour;

            ThermoProps res;
            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("n-Pentane");
            cc.Add(sc);

            X[0] = 0.3563;
            X[1] = 1 - 0.3563;
            Temperature T = new Temperature(390);
            Pressure P = new Pressure(11);
            res = ThermodynamicsClass.BulkStreamThermo(cc, X, P, T, state, cc.Thermo);
        }

        private enum Atoms
        { C, H, S, O, N }

        [TestMethod]
        public void TestButaneFormula()
        {
            BaseComp sc;
            sc = Thermodata.GetComponent("n-Butane");
            Dictionary<string, double> composition = new Dictionary<string, double>();

            string form = sc.Formula.Trim();

            int loc;
            string value;
            bool cont;
            int x;
            int NoofAtoms;

            string[] comps = Enum.GetNames(typeof(Atoms));

            for (int i = 0; i < comps.Length; i++)
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