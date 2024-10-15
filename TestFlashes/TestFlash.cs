using ModelEngine;
using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace TestFlashes
{
    [TestClass]
    public class TestFlashes
    {
 
        [TestMethod]
        public void Test4CompFlash()
        {
            Thermodata data = new Thermodata();

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            Components cc = new Components();
            BaseComp sc;

            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;

            sc = Thermodata.GetComponent("propane");
            cc.Add(sc);

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            //sc = Thermodata.GetRealComponent("n-C30");
            sc = Thermodata.GetComponent("H2O");
            cc.Add(sc);

            //850.0	0.951121643	901.8	2.147	2.257	1803.7

            sc = new PseudoComponent(0.951121643, 850 + 273.15, 1803.7, 2.257, 901.8 + 273.15, 2.147, 3667, thermo);
            cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.4;
            cc[2].MoleFraction = 0.0;
            cc[3].MoleFraction = 0.1;

            Port_Material p = new(cc);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);

            p.Flash(enumFlashType.PT, thermo: thermo);

            double[,] res = new double[2, cc.ComponentList.Count];

            for (int i = 0; i < cc.ComponentList.Count; i++)
                res[0, i] = cc.LiqPhaseMolFractions[i];

            for (int i = 0; i < cc.ComponentList.Count; i++)
                res[1, i] = cc.VapPhaseMolFractions[i];
        }

        [TestMethod]
        public void Test3CompFlash()
        {
            Thermodata data = new Thermodata();

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            Components cc = new Components();
            BaseComp sc;

            thermo.KMethod = enumEquiKMethod.PR78;

            sc = Thermodata.GetComponent("propane");
            cc.Add(sc);

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            sc = new PseudoComponent(850 + 273.15, 0.951121643, thermo);
            cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.4;
            cc[2].MoleFraction = 0.1;

            Port_Material p = new(cc);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 10, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);

            Debug.Assert(p.Flash(enumFlashType.PT, thermo: thermo), "Flash Failed");

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
        public void TestDewFunc()
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
            Temperature T = new(1);

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

            DewPointClass dew = new(cc, P);

            T = 1;
            for (int i = 0; i < 400; i++)
            {
                var res = dew.DewFunc(T, out _);
                Debug.Print(" DewFunc V% : " + T.Kelvin.ToString("0.0") + " " + res.ToString());
                T += 1;
            }
        }

        [TestMethod]
        public void TestRachfordRiceFvFunc()
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
            Temperature T = new(1);

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

            DewPointClass dew = new(cc, P);

            double[] X = cc.MoleFractions;
            double[] Y = cc.MoleFractions;

            int LightestComp = cc.MinNonZeroBoiling;
            int HeaviestComp = cc.MaxNonZeroBoiling;

            T = 1;
            for (int i = 0; i < 400; i++)
            {
                double[] Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out _, thermo);
                var fv = CommonRoutines.fv(cc.MoleFractions, Kn);
                CommonRoutines.UpdateXY(fv, Kn, cc.MoleFractions, ref X, ref Y);
                if (Y.Sum() < 1)
                    Y[LightestComp] = 1;
                if (X.Sum() < 1)
                    X[HeaviestComp] = 1;

                Debug.Print(" Fv V% : " + T.Celsius.ToString("0.0") + " " + fv.ToString());
                T += 1;
            }
        }

        [TestMethod]
        public void TestDewPointMethodsC2_C3()
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

            P = 0.1;
            for (int i = 0; i < 400; i++)
            {
                var res = ThermodynamicsClass.DewPoint(cc, P, thermo, out _);
                Debug.Print(" DewPoint new  P: " + P.BarA.ToString("0.0") + " " + res.ToString());
                P += 0.1;
            }

            P = 0.1;
            for (int i = 0; i < 400; i++)
            {
                var res = ThermodynamicsClass.DewPointOld(cc, P, thermo, out _);
                Debug.Print(" DewPoint Old P: " + P.BarA.ToString("0.0") + " " + res.ToString());
                P += 0.1;
            }
        }



        [TestMethod]
        public void TestBubbleC4()
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

            P = 0.1;
            port.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            for (int i = 0; i < 400; i++)
            {
                Temperature Tb = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
                Debug.Print(" P: " + P.BaseValue.ToString("0.0") + " T: " + Tb.ToString());
                P += 0.1;
            }
        }

        [TestMethod]
        public void TestBubbleC2C3()
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

            P = 0.1;
            port.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            for (int i = 0; i < 400; i++)
            {
                Temperature Tb = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
                Debug.Print(" P: " + P.BaseValue.ToString("0.0") + " T: " + Tb.ToString());
                P += 0.1;
            }

            Temperature Tb1 = ThermodynamicsClass.BubblePoint(cc, 8.6, thermo, out _);
        }

        [TestMethod]
        public void TestBubbleH2CO2()
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

            sc = Thermodata.GetComponent("Hydrogen");
            cc.Add(sc);
            sc = Thermodata.GetComponent("CO2");
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

            P = 0.1;
            port.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            for (int i = 0; i < 400; i++)
            {
                Temperature Tb = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
                Debug.Print(" P: " + P.BaseValue.ToString("0.0") + " T: " + Tb.ToString());
                P += 0.1;
            }

            Temperature Tb1 = ThermodynamicsClass.BubblePoint(cc, 40, thermo, out _);
        }

        [TestMethod]
        public void TestBubbleH2CO()
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

            sc = Thermodata.GetComponent("Hydrogen");
            cc.Add(sc);
            sc = Thermodata.GetComponent("CO");
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

            P = 0.1;
            port.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            for (int i = 0; i < 400; i++)
            {
                Temperature Tb = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
                Debug.Print(" P: " + P.BaseValue.ToString("0.0") + " T: " + Tb.ToString());
                P += 0.1;
            }

            Temperature Tb1 = ThermodynamicsClass.BubblePoint(cc, 40, thermo, out _);
        }

        [TestMethod]
        public void TestDewPointH2_CO_CO2()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;
            Components cc = new();
            BaseComp sc;
            GibbsReactor RX = new();
            Port_Material port = new Port_Material();
            Temperature T = new Temperature(273.15 + 6);
            Pressure P = new Pressure(1);

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
            cc[2].MoleFraction = 0; // 0.238304223678262;
            cc[3].MoleFraction = 0.25079809144246507;

            cc.NormaliseFractions();

            port.cc = cc;
            port.T_ = new(Units.ePropID.T, T, SourceEnum.Input);
            port.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
            port.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Input;
            port.cc.Thermo.UseBIPs = false;

            // port.T = new  StreamProperty(Units.ePropID.T, 278.15, SourceEnum.Input);
            // port.Flash();
            // port.ClearNonInputs();
            double[] Y = new double[4];
            Y[1] = 1;

            port.T_ = new StreamProperty(Units.ePropID.T, 10, SourceEnum.Input);
            port.Flash();
            port.ClearNonInputs();

            port.T_.BaseValue = 10;
            port.Flash();

            T = 26;
            var res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, Y, out _, thermo);

            T = 1;
            for (int i = 0; i < 400; i++)
            {
                res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, Y, out _, thermo);
                var res2 = cc.MoleFractions.Divide(res).Sum();
                T += 1;
                Debug.Print(T.Celsius.ToString() + " 1/KMix " + res2.ToString());
            }

            T = 1;
            for (int i = 0; i < 400; i++)
            {
                port.T_.BaseValue = T + i;
                port.Flash();
                port.H_.Clear();
                Debug.Print(port.T_.ToString() + " Q " + port.Q_.ToString());
            }

            var res3 = ThermodynamicsClass.DewPoint(cc, P, thermo, out _);

            Debug.Print("Dew Point : @ " + res3.ToString() + "  " + port.Q_.ToString());

            var res4 = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);

            Debug.Print("Bubble Point : @ " + res4.ToString() + "  " + port.Q_.ToString());
        }

        [TestMethod]
        public void TestDewPointC2_C3()
        {
            ThermoDynamicOptions thermo = new();
            FlowSheet fs = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;
            Components cc = new();
            BaseComp sc;
            GibbsReactor RX = new();
            Port_Material port = new Port_Material();
            Temperature T = new Temperature(273.15 + 6);
            Pressure P = new Pressure(1);

            sc = Thermodata.GetComponent("Ethane");
            cc.Add(sc);
            sc = Thermodata.GetComponent("Propane");
            cc.Add(sc);

            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.5;

            cc.NormaliseFractions();

            port.cc = cc;
            port.T_ = new(Units.ePropID.T, T, SourceEnum.Input);
            port.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
            port.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Input;
            port.cc.Thermo.UseBIPs = false;

            double[] Y = new double[4];
            Y[0] = 1;

            port.T_ = new StreamProperty(Units.ePropID.T, 10, SourceEnum.Input);
            port.Flash();
            port.ClearNonInputs();
            port.T_.BaseValue = 10;
            port.Flash();

            T = 26;
            var res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, Y, out _, thermo);

            T = 1;
            for (int i = 0; i < 400; i++)
            {
                res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, Y, out _, thermo);
                var res2 = cc.MoleFractions.Divide(res).Sum();
                T += 1;
                Debug.Print(T.Celsius.ToString() + " 1/KMix " + res2.ToString());
            }

            T = 1;
            for (int i = 0; i < 400; i++)
            {
                port.T_.BaseValue = T + i;
                port.Flash();
                port.H_.Clear();
                Debug.Print(port.T_.ToString() + " Q " + port.Q_.ToString());
            }

            var res3 = ThermodynamicsClass.DewPoint(cc, P, thermo, out _);

            Debug.Print("Dew Point : @ " + res3.ToString() + "  " + port.Q_.ToString());

            var res4 = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);

            Debug.Print("Bubble Point : @ " + res4.ToString() + "  " + port.Q_.ToString());
        }

        [TestMethod]
        public void FlashC3_C4AllTemps()
        {
            Temperature T = 273.15;
            Temperature T2 = 273.15 + 500;
            for (int i = 0; i < 100; i++)
            {
                TestButane_PropaneAllFlashes(T + (T2 - T) / 100 * i);
            }
        }

        public void TestButane_PropaneAllFlashes(double Td = double.NaN)
        {
            int No = 1;
            Temperature T;
            if (double.IsNaN(Td))
                T = 273.15 + 370;
            else
                T = Td;

            Pressure P = 4;
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            Components cc = new Components();

            string[] RealNames = new string[] { "Propane", "n-Butane" };

            double[] RealFractions = new double[] { 0.50000, 0.50000 };

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            T = 11;
            P = 3;

            cc.Thermo = thermo;
            cc.Thermo.UseBIPs = false;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, T, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;
            feed.cc.Thermo.UseBIPs = false;

            var watch = Stopwatch.StartNew();
            watch.Restart();

            feed.Flash();
            double H = feed.H_;
            double S = feed.S_;
            double Q = feed.Q_;
            PrintInfo.PrintPortInfo(feed, "Calc Base Conditions T and P");

            #region new  RR Methods

            watch.Restart();
            for (int i = 0; i < No; i++)
            {
                feed.Props.Clear();
                feed.T_ = new StreamProperty(ePropID.T, T, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
                feed.Flash();
            }
            var res4 = watch.ElapsedMilliseconds.ToString();

            PrintInfo.PrintPortInfo(feed, "new  RR T and P");

            watch.Restart();
            for (int i = 0; i < No; i++)
            {
                feed.Props.Clear();
                feed.H_ = new StreamProperty(ePropID.H, H, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
                feed.Flash();
            }
            var res5 = watch.ElapsedMilliseconds.ToString();

            PrintInfo.PrintPortInfo(feed, "new  RR H and P");

            watch.Restart();
            for (int i = 0; i < No; i++)
            {
                feed.Props.Clear();
                feed.S_ = new StreamProperty(ePropID.S, S, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
                feed.Flash();
            }
            var res6 = watch.ElapsedMilliseconds.ToString();

            PrintInfo.PrintPortInfo(feed, "new  RR S and P");

            Debug.Print("new  T and P " + res4 + "\n" +
                            "new  H and P " + res5 + "\n" +
                            "new  S and P " + res6);

            #endregion new  RR Methods
        }

        [TestMethod]
        public void TestC4_C4_n_C30_AllFlashes()
        {
            int No = 1;
            Temperature T;

            T = 273.15 + 25;

            Pressure P = 12;
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            Components cc = new Components();

            string[] RealNames = new string[] { "Propane", "N-butane", "n-C30" };

            double[] RealFractions = new double[] { 0.00650, 0.94004, 0.05345 };

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            T = 11;
            P = 2;

            cc.Thermo = thermo;
            cc.Thermo.UseBIPs = false;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, T, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;
            feed.cc.Thermo.UseBIPs = false;

            var watch = Stopwatch.StartNew();
            watch.Restart();

            feed.Flash();
            double H = feed.H_;
            double S = feed.S_;
            double Q = feed.Q_;

            watch.Restart();

            No = 1;
            for (int i = 0; i < No; i++)
            {
                feed.Props.Clear();
                feed.T_ = new StreamProperty(ePropID.T, 88.96 + 273.15, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
                Assert.IsTrue(feed.Flash());
                PrintInfo.PrintPortInfo(feed, "new  RR T and P");
            }
            var res4 = watch.ElapsedMilliseconds.ToString();
        }

        [TestMethod]
        public void TestH2O_MEK_C3ACID()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = new();

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;

            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");

            /*   H2O.MoleFraction = 0.99;
               MEK.MoleFraction = 0.01;
               PropionicAcid.MoleFraction = 0;*/

            H2O.MoleFraction = 1;
            MEK.MoleFraction = 0.00;
            PropionicAcid.MoleFraction = 0;

            cc.Add(H2O);
            cc.Add(MEK);
            cc.Add(PropionicAcid);
            cc.Thermo = thermo;

            Port_Material pm = new();
            pm.cc = cc;
            pm.cc.Origin = SourceEnum.Input;

            pm.T_ = new StreamProperty(Units.ePropID.T, 25 + 273.15);
            pm.P_ = new StreamProperty(Units.ePropID.P, 1);
            pm.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1);

            pm.Flash(true, enumFlashType.PT);

            var prop = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, pm.P, pm.T, enumFluidRegion.Liquid, thermo);
            Debug.Print(pm.H_.BaseValue.ToString());
            Debug.Print(prop.H.BaseValue.ToString());

            PrintInfo.PrintPortInfo(pm);

            Enthalpy H = pm.H_.BaseValue;
            pm.T_.Clear();

            pm.H_ = new StreamProperty(Units.ePropID.H, H, SourceEnum.Input);

            pm.Flash(true, enumFlashType.PH);

            PrintInfo.PrintPortInfo(pm);

            Temperature dewpoint = ThermodynamicsClass.DewPoint(cc, pm.P, thermo, out _);
        }
    }
}