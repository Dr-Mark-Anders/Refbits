using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using Steam97;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace TestCrudeFlashes
{
    [TestClass]
    public class TestCrudeFlashes
    {
        [TestMethod]
        public void TestUralsEnthlpySpeed()
        {
            Components cc = DefaultStreams.CrudeUrals();

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.UseBIPs = true;

            Temperature T = 150;
            Pressure P = 1;

            cc.NormaliseFractions();
            cc.Origin = SourceEnum.Input;

            Port_Material p = new(cc);

            p.T_ = new(ePropID.T, T, SourceEnum.Input);
            p.P_ = new(ePropID.P, 1, SourceEnum.Input);
            p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            p.Flash(enumFlashType.PT, thermo: thermo);

            double Enthalpy = p.H_;

            p.T_.Clear();
            p.H_ = new(Units.ePropID.H, Enthalpy, SourceEnum.Input);
            p.P_ = new(Units.ePropID.P, P, SourceEnum.Input);

            thermo.FlashMethod = enumFlashAlgorithm.RR;

            var time = new Stopwatch();

            HDeparture(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);

            var res = 0.0;
            time.Start();

            int compcount = cc.Count;

            for (int i = 0; i < 100000000; i++)
            {
                for (int y = 0; y < compcount; y++)
                {
                    res += cc[y].Molwt;// same
                }
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show(res.ToString() + "Time ms " + (time.ElapsedMilliseconds / 1000).ToString());

            time.Reset();

            res = 0;
            time.Start();

            for (int i = 0; i < 100000000; i++)
            {
                for (int y = 0; y < compcount; y++)
                {
                    res += cc[y].MW;// same
                }
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show(res.ToString() + "Time ms " + (time.ElapsedMilliseconds / 1000).ToString());

            time.Reset();

            res = 0;
            time.Start();

            for (int i = 0; i < 100000000; i++)  // definately slower
            {
                foreach (BaseComp y in cc)
                {
                    res += y.MW;
                }
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show(res.ToString() + "Time ms " + (time.ElapsedMilliseconds / 1000).ToString());
        }

        [TestMethod]
        public void TestUralsPropertySpeed()
        {
            Components cc = DefaultStreams.CrudeUrals();

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.UseBIPs = true;

            TemperatureC T = 150;
            Pressure P = 1;

            cc.NormaliseFractions();
            cc.Origin = SourceEnum.Input;

            Port_Material p = new(cc);

            p.T_ = new(ePropID.T, T, SourceEnum.Input);
            p.P_ = new(ePropID.P, 1, SourceEnum.Input);
            p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
            p.Flash(enumFlashType.PT, thermo: thermo);

            double Enthalpy = p.H_;

            p.T_.Clear();
            p.H_ = new(ePropID.H, Enthalpy, SourceEnum.Input);
            p.P_ = new(ePropID.P, P, SourceEnum.Input);

            thermo.FlashMethod = enumFlashAlgorithm.RR;

            var time = new Stopwatch();

            HDeparture(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);

            var res = 0.0;
            time.Start();

            int compcount = cc.Count;

            for (int i = 0; i < 100000000; i++)
            {
                for (int y = 0; y < compcount; y++)
                {
                    res += cc[y].Molwt;// same
                }
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show(res.ToString() + "Time ms " + (time.ElapsedMilliseconds / 1000).ToString());

            time.Reset();

            res = 0;
            time.Start();

            for (int i = 0; i < 100000000; i++)
            {
                for (int y = 0; y < compcount; y++)
                {
                    res += cc[y].MW;// same
                }
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show(res.ToString() + "Time ms " + (time.ElapsedMilliseconds / 1000).ToString());

            time.Reset();

            res = 0;
            time.Start();

            for (int i = 0; i < 100000000; i++)  // definately slower
            {
                foreach (BaseComp y in cc)
                {
                    res += y.MW;
                }
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show(res.ToString() + "Time ms " + (time.ElapsedMilliseconds / 1000).ToString());
        }

        public static Enthalpy HDeparture(Components cc, double[] XWet, Pressure P, Temperature T, enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            if (cc is null || cc.Count == 0 || !P.IsKnown || !T.IsKnown)
                return double.NaN;

            double[] XCopy = (double[])XWet.Clone();

            if (cc is null || cc.ComponentList is null)
                return double.NaN;

            bool HandleWaterSeperately = false;

            Enthalpy EnthDeparture = double.NaN;
            Enthalpy WaterH = 0;

            int loc = cc.WaterLocation;
            double watermolefrac = XWet[loc];

            StmPropIAPWS97 steam = new();

            if (loc >= 0) // water
            {
                HandleWaterSeperately = true;
            }

            if (HandleWaterSeperately)
            {
                Temperature BoilPoint = steam.Tsat(P);

                if (Math.Abs(BoilPoint.BaseValue - T.BaseValue) < 0.000001)
                {
                    WaterH = StmPropIAPWS97.WaterPropsMolar(P, BoilPoint, enumSatType.SatLiq).H_higm;

                    switch (state)
                    {
                        case enumFluidRegion.Liquid:
                            WaterH = StmPropIAPWS97.WaterPropsMolar(P, BoilPoint, enumSatType.SatLiq).H_higm;
                            break;

                        case enumFluidRegion.Vapour:
                            WaterH = StmPropIAPWS97.WaterPropsMolar(P, BoilPoint, enumSatType.SatVap).H_higm;
                            break;

                        default:
                            WaterH = StmPropIAPWS97.WaterPropsMolar(P, BoilPoint, enumSatType.Normal).H_higm;
                            break;
                    }
                }
                else
                {
                    WaterH = StmPropIAPWS97.WaterPropsMolar(P, T, enumSatType.Normal).H_higm;
                }

                EnthDeparture = WaterH;
                XCopy[loc] = 0; // Set Water to Zero
                XCopy = XCopy.Normalise();
            }

            if (cc.Count > 1 || (cc.Count > 0 && !HandleWaterSeperately))
            {
                switch (thermo.Enthalpy)
                {
                    case enumEnthalpy.Ideal:
                        EnthDeparture = 0;
                        break;

                    case enumEnthalpy.LeeKesler:
                    case enumEnthalpy.ChaoSeader:
                    case enumEnthalpy.GraysonStreed:
                        EnthDeparture = LeeKesler.H_Hig(cc, XCopy, P, T, state);
                        break;

                    case enumEnthalpy.PR76:
                        EnthDeparture = PengRobinson.H_Hig(cc, XCopy, P, T, state, thermo);
                        break;

                    case enumEnthalpy.PR78:
                        EnthDeparture = PengRobinson.H_Hig(cc, XCopy, P, T, state, thermo);
                        break;

                    case enumEnthalpy.PRSV:
                        EnthDeparture = PengRobinson.H_Hig(cc, XCopy, P, T, state, thermo);
                        break;

                    case enumEnthalpy.SRK:
                        EnthDeparture = SRK.H_Hig(cc, P, T, XCopy, state);
                        break;

                    case enumEnthalpy.RK:
                        EnthDeparture = RK.H_Hig(cc, P, T, XCopy, state);
                        break;

                    case enumEnthalpy.BWRS:
                        EnthDeparture = BWRS.H_Hig(cc, XCopy, T);
                        break;

                    default:
                        EnthDeparture = double.NaN;
                        break;
                }
            }

            if ((HandleWaterSeperately && cc.Count == 1) || (loc >= 0 && watermolefrac == 1)) // only water
            {
                EnthDeparture = WaterH;
            }
            else if (!double.IsNaN(EnthDeparture) && !double.IsNaN(watermolefrac))
            {
                EnthDeparture = EnthDeparture * (1 - watermolefrac) + WaterH * watermolefrac;
            }

            return EnthDeparture;
        }

        [TestMethod]
        public void TestUralsFlash()
        {
            Components cc = DefaultStreams.CrudeUrals();

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.UseBIPs = true;

            TemperatureC T = 150;
            Pressure P = 1;

            cc.NormaliseFractions();
            cc.Origin = SourceEnum.Input;

            Port_Material p = new(cc);

            p.T_ = new(ePropID.T, T, SourceEnum.Input);
            p.P_ = new(ePropID.P, 1, SourceEnum.Input);
            p.MolarFlow_ = new(ePropID.MOLEF, 1, SourceEnum.Input);
            p.Flash(enumFlashType.PT, thermo: thermo);

            double Enthalpy = p.H_;

            p.T_.Clear();
            p.H_ = new(ePropID.H, Enthalpy, SourceEnum.Input);
            p.P_ = new(ePropID.P, P, SourceEnum.Input);

            thermo.FlashMethod = enumFlashAlgorithm.RR;

            var time = new Stopwatch();

            time.Start();

            for (int i = 0; i < 1000; i++)
            {
                p.Props.Clear();
                p.H_ = new(ePropID.H, Enthalpy, SourceEnum.Input);
                p.P_ = new(ePropID.P, 1, SourceEnum.Input);
                p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(p.Flash(enumFlashType.PH, thermo: thermo), "Flash failed");
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show("Time ms " + (time.ElapsedMilliseconds / 1000).ToString());

            time.Reset();
            time.Start();

            for (int i = 0; i < 1000; i++)
            {
                p.Props.Clear();
                p.H_ = new(Units.ePropID.H, Enthalpy, SourceEnum.Input);
                p.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
                p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(p.Flash(enumFlashType.PHIO, thermo: thermo), "Flash failed");
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show("Time ms " + (time.ElapsedMilliseconds / 1000).ToString());

            time.Reset();
            time.Start();

            for (int i = 0; i < 1000; i++)
            {
                p.Props.Clear();
                p.H_ = new(Units.ePropID.H, Enthalpy, SourceEnum.Input);
                p.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
                p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(p.Flash(enumFlashType.PHOld, thermo: thermo), "Flash failed");
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show("Time ms " + (time.ElapsedMilliseconds / 1000).ToString());
        }

        [TestMethod]
        public void TestUralsFlash370()
        {
            Components cc = DefaultStreams.CrudeUrals();

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.UseBIPs = true;

            TemperatureC T = 370;
            Pressure P = 3;

            cc.NormaliseFractions();
            cc.Origin = SourceEnum.Input;

            Port_Material p = new(cc);

            p.T_ = new(ePropID.T, T, SourceEnum.Input);
            p.P_ = new(ePropID.P, P, SourceEnum.Input);
            p.MolarFlow_ = new(ePropID.MOLEF, 1, SourceEnum.Input);
            p.Flash(enumFlashType.PT, thermo: thermo);

            double Enthalpy = p.H_;

            p.T_.Clear();
            p.H_ = new(ePropID.H, Enthalpy, SourceEnum.Input);
            p.P_ = new(ePropID.P, P, SourceEnum.Input);

            thermo.FlashMethod = enumFlashAlgorithm.RR;

            var time = new Stopwatch();

            time.Start();

            for (int i = 0; i < 1000; i++)
            {
                p.Props.Clear();
                p.H_ = new(ePropID.H, Enthalpy, SourceEnum.Input);
                p.P_ = new(ePropID.P, 1, SourceEnum.Input);
                p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(p.Flash(enumFlashType.PH, thermo: thermo), "Flash failed");
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show("Time ms " + (time.ElapsedMilliseconds / 1000).ToString());

            time.Reset();
            time.Start();

            for (int i = 0; i < 1000; i++)
            {
                p.Props.Clear();
                p.H_ = new(Units.ePropID.H, Enthalpy, SourceEnum.Input);
                p.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
                p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(p.Flash(enumFlashType.PHIO, thermo: thermo), "Flash failed");
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show("Time ms " + (time.ElapsedMilliseconds / 1000).ToString());

            time.Reset();
            time.Start();

            for (int i = 0; i < 1000; i++)
            {
                p.Props.Clear();
                p.H_ = new(Units.ePropID.H, Enthalpy, SourceEnum.Input);
                p.P_ = new(Units.ePropID.P, 1, SourceEnum.Input);
                p.MolarFlow_ = new(Units.ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(p.Flash(enumFlashType.PHOld, thermo: thermo), "Flash failed");
            }

            time.Stop();
            System.Windows.Forms.MessageBox.Show("Time ms " + (time.ElapsedMilliseconds / 1000).ToString());
        }


        [TestMethod]
        public void TestDefaultCrudePHFlash()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.Crude();

            cc.Thermo.Enthalpy = enumEnthalpy.PR78;
            cc.Thermo.KMethod = enumEquiKMethod.PR78;
            cc.Thermo.CritTMethod = enumCritTMethod.TWU;
            cc.Thermo.CritPMethod = enumCritPMethod.TWU;
            cc.Thermo.MW_Method = enumMW_Method.TWU;
            cc.Thermo.UseBIPs = false;

            TemperatureC T= 370;
            Pressure P = 3;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            feed.Flash();
            PrintInfo.PrintPortInfo(feed);

            double Enthalpy = feed.H_;

            var watch = Stopwatch.StartNew();
            watch.Restart();

            for (int i = 0; i < 1; i++)
            {
                feed.Props.Clear();
                feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
                feed.H_ = new StreamProperty(ePropID.H, Enthalpy, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
                feed.Flash();
            }

            PrintInfo.PrintPortInfo(feed);
            System.Windows.Forms.MessageBox.Show(watch.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void TestDefaultReducedCrudePHFlash()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.ShortResidue();

            cc.Thermo.Enthalpy = enumEnthalpy.PR78;
            cc.Thermo.KMethod = enumEquiKMethod.PR78;
            cc.Thermo.CritTMethod = enumCritTMethod.TWU;
            cc.Thermo.CritPMethod = enumCritPMethod.TWU;
            cc.Thermo.MW_Method = enumMW_Method.TWU;
            cc.Thermo.UseBIPs = false;


            TemperatureC T = 370;
            Pressure P = 3;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            var watch = Stopwatch.StartNew();
            watch.Restart();

            feed.Flash();
            PrintInfo.PrintPortInfo(feed);

            double Enthalpy = feed.H_;

            for (int i = 0; i < 1; i++)
            {
                feed.Props.Clear();
                feed.H_ = new StreamProperty(ePropID.H, Enthalpy, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
                feed.Flash();
            }

            PrintInfo.PrintPortInfo(feed);
            System.Windows.Forms.MessageBox.Show(watch.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void TestDefaultResiduePHFlash()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.Residue();

            cc.Thermo.Enthalpy = enumEnthalpy.PR78;
            cc.Thermo.KMethod = enumEquiKMethod.PR78;
            cc.Thermo.CritTMethod = enumCritTMethod.TWU;
            cc.Thermo.CritPMethod = enumCritPMethod.TWU;
            cc.Thermo.MW_Method = enumMW_Method.TWU;
            cc.Thermo.UseBIPs = false;

            TemperatureC T = 370;
            Pressure P = 3;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            var watch = Stopwatch.StartNew();
            watch.Restart();

            feed.Flash();
            PrintInfo.PrintPortInfo(feed);

            double Enthalpy = feed.H_;

            for (int i = 0; i < 1; i++)
            {
                feed.Props.Clear();
                feed.H_ = new StreamProperty(ePropID.H, Enthalpy, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
                feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
                feed.Flash();
            }

            PrintInfo.PrintPortInfo(feed);
            System.Windows.Forms.MessageBox.Show(watch.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void TestCrudeTPFlash()
        {
            FlowSheet fs = new FlowSheet();
            double Pressure = 3, Temperature = 370;

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new Components();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

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

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new PseudoComponent(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc.Thermo = thermo;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, Pressure, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + Temperature, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            var watch = Stopwatch.StartNew();
            watch.Restart();
            int No = 1;

            for (int i = 0; i < No; i++)
            {
                feed.Properties.Clear();
                feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
                feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
                feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(feed.Flash());
            }

            PrintInfo.PrintPortInfo(feed, "Test P & T with Crude");
            //Debug.Print ((watch.ElapsedMilliseconds/10000).ToString());
            //MessageBox.Show((watch.ElapsedMilliseconds/No).ToString());
        }

        [TestMethod]
        public void TestCrudePH_IO_Flash()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new Components();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

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

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new PseudoComponent(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            TemperatureC T = 370;
            Pressure P = 3;

            cc.Thermo = thermo;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            var watch = Stopwatch.StartNew();
            watch.Restart();

            feed.Flash();

            PrintInfo.PrintPortInfo(feed);

            double Enthalpy = feed.H_;

            feed.Props.Clear();
            feed.H_ = new StreamProperty(ePropID.H, Enthalpy);
            feed.P_ = new StreamProperty(ePropID.P, 3);

            thermo.FlashMethod = enumFlashAlgorithm.RR;
            int No = 100;
            for (int i = 0; i < No; i++)
            {
                feed.Props.Clear();
                feed.H_ = new StreamProperty(ePropID.H, Enthalpy);
                feed.P_ = new StreamProperty(ePropID.P, 3);
                feed.Flash();
            }

            //System.Windows.Forms.MessageBox.Show(watch.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void TestCrudePHFlash()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new Components();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

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

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new PseudoComponent(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }


            TemperatureC T = 370;
            Pressure P = 3;

            cc.Thermo = thermo;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            var watch = Stopwatch.StartNew();
            watch.Restart();

            feed.Flash();
            double Enthalpy = feed.H_;

            for (int i = 0; i < 100; i++)
            {
                feed.Props.Clear();
                feed.H_ = new StreamProperty(ePropID.H, Enthalpy, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
                feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
                feed.Flash();
            }

            PrintInfo.PrintPortInfo(feed);
            //System.Windows.Forms.MessageBox.Show(watch.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void TestCrudeAtT()
        {
            TestCrudeAllFlashes(273.15 + 370, true, 30);
        }

        [TestMethod]
        public void FlashCrudeAllTemps()
        {
            Temperature T = 273.15;
            Temperature T2 = 273.15 + 500;
            for (int i = 0; i < 100; i++)
            {
                TestCrudeAllFlashes(T + (T2 - T) / 100 * i, false);
            }
        }

        public void TestCrudeAllFlashes(double Td = double.NaN, bool showmessage = false, double Pressure = 4)
        {
            int No = 1;
            Temperature T;
            if (double.IsNaN(Td))
                T = 273.15 + 370;
            else
                T = Td;

            Pressure P = Pressure;

            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new Components();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

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

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new PseudoComponent(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc.Thermo = thermo;

            fs.Add(cc);

            Port_Material feed = new Port_Material();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, T, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            var watch = Stopwatch.StartNew();
            watch.Restart();

            Assert.IsTrue(feed.Flash());
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
                feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(feed.Flash());
            }
            var res4 = watch.ElapsedMilliseconds;

            PrintInfo.PrintPortInfo(feed, "new  RR T and P");

            watch.Restart();
            for (int i = 0; i < No; i++)
            {
                feed.Props.Clear();
                feed.H_ = new StreamProperty(ePropID.H, H, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
                feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(feed.Flash());
            }
            var res5 = watch.ElapsedMilliseconds;

            PrintInfo.PrintPortInfo(feed, "new  RR H and P");

            watch.Restart();
            for (int i = 0; i < No; i++)
            {
                feed.Props.Clear();
                feed.S_ = new StreamProperty(ePropID.S, S, SourceEnum.Input);
                feed.P_ = new StreamProperty(ePropID.P, P, SourceEnum.Input);
                feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
                Assert.IsTrue(feed.Flash());
            }
            var res6 = watch.ElapsedMilliseconds;

            PrintInfo.PrintPortInfo(feed, "new  RR S and P");

            if (showmessage)
            {
                /*    MessageBox.Show("Old T and P " + res1 / No + "\n" +
                                     "Old H and P " + res2 / No + "\n" +
                                     "Old S and P " + res3 / No + "\n" +
                                     "new  T and P " + res4 / No + "\n" +
                                     "new  H and P " + res5 / No + "\n" +
                                     "new  S and P " + res6 / No);*/
            }

            #endregion new  RR Methods
        }

        [TestMethod]
        public void TestHVGOFlash()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.0001, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000 };

            double[] QuasiFractions = new double[] {0.0000,0.0000,0.0000,0.0001,0.0004,0.0011,0.0026,0.0061,0.0145,0.0333,0.0724,0.1405,0.2188,0.2344,0.1443,0.0659,0.0299,0.0143,0.0076,0.0044,0.0028,
                0.0019,0.0013,0.0009,0.0006,0.0005,0.0003,0.0002,0.0001,0.0001,0.0001,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,
                0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

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

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }


            TemperatureC T = 370;
            Pressure P = 3;

            Port_Material port = new Port_Material();
            port.cc.Add(cc);

            var res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, new Temperature(117 + 273.15), enumFluidRegion.Liquid, thermo).H;

            port.SetPortValue(Units.ePropID.T, new Temperature(117 + 273.15), SourceEnum.Input);
            port.SetPortValue(Units.ePropID.P, new Pressure(0.4), SourceEnum.Input);
            port.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            port.cc.Origin = SourceEnum.Transferred;

            port.Flash();
            var t = ThermodynamicsClass.BubblePoint(port.cc, port.P, port.Thermo, out _);
            PrintInfo.PrintPortInfo(port);

            port.SetPortValue(Units.ePropID.T, new Temperature(118 + 273.15), SourceEnum.Input);
            port.Flash(true);
            PrintInfo.PrintPortInfo(port);

            port.SetPortValue(Units.ePropID.T, new Temperature(119 + 273.15), SourceEnum.Input);
            port.Flash(true);
            PrintInfo.PrintPortInfo(port);

            port.SetPortValue(Units.ePropID.T, new Temperature(129 + 273.15), SourceEnum.Input);
            port.Flash(true);
            PrintInfo.PrintPortInfo(port);

            port.T_.Clear();
            port.Q_.Clear();
            port.SetPortValue(Units.ePropID.Q, new Quality(0), SourceEnum.Input);
            port.Flash(true);
            PrintInfo.PrintPortInfo(port);
        }
    }
}