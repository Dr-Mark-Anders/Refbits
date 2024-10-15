using EngineThermo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]

    public class TestEnthalpy
    {
        private enumCalcResult cres;

        [TestMethod]
        public void TestEnthalpy4Comps()
        {
            Thermodata data = new Thermodata();

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = true;
            Components cc = new Components();
            BaseComp sc;


            sc = Thermodata.GetRealComponent("propane");
            sc.molefraction = 0.5;
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("n-Butane");
            sc.molefraction = 0.4;
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("n-Pentane");
            sc.molefraction = 0.08;
            cc.Add(sc);

            //sc = new PseudoComponent(875 + 273.15, 1.1209, 1158.9180, 1.77649998664856, 992.2591+273.15, 8.15325, 2.1441, thermo);
            sc = new PseudoComponent(1.1209, new Temperature(273.15 + 875), "Test", thermo);
            sc.molefraction = 0.02;
            cc.Add(sc);


            cc.T = 25 + 273.15;
            cc.P = 6;

            FlashClass.Flash(cc);
            var Res2 = Thermodynamics.BulkStreamThermo(cc, cc.T, enumMassOrMolar.Molar, enumFluidRegion.Liquid, ref cres).H;
            var res3 = cc.BulkEnthalpy();
        }


        [TestMethod]
        public void TestLeeKeslerCp()
        {
            Temperature Tk = new Temperature(273.15 + 250);
            double[] res = LeeKesler.GetIdealVapCpCoefficients(0.3332, 12.01, 0.7454);
            double Cp = res[1] + res[2] * Tk + res[3] * Tk.Pow(2); // 2.48044
        }

        [TestMethod]
        public void TestFlash()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[2];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            Components cc = new Components();
            BaseComp sc;

            thermo.KMethod = enumEquiKMethod.PR78;

            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("propane");
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("n-C30");
            cc.Add(sc);

            cc[0].molefraction = 0.5;
            cc[1].molefraction = 0.4;
            cc[2].molefraction = 0.1;

            cc.T = 273.15 + 10;
            cc.P = 3;

            FlashClass.Flash(cc);

            double[,] res = new double[2, cc.components.Count];

            for (int i = 0; i < cc.components.Count; i++)
            {
                res[0, i] = cc.LiquidComponents.MolFractions[i];
            }

            for (int i = 0; i < cc.components.Count; i++)
            {
                res[1, i] = cc.VapourComponents.MolFractions[i];
            }
        }

        [TestMethod]
        public void TestAssay()
        {
            Thermodata data = new Thermodata();

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

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.UseBIPs = false;

            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;


            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetRealComponent(RealNames[i]);
                sc.molefraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new PseudoComponent(BPk[i], SG[i], thermo);
                pc.molefraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc.T.Celsius = 370;
            cc.P.BarA = 1;

            cc.NormaliseMolFrac();

            FlashClass.Flash(cc);

            var res = cc.BulkEnthalpy();
            Debug.Print(cc.CP_MASS().ToString());
        }

        [TestMethod]
        public void TestComponent()
        {
            Thermodata data = new Thermodata();

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new Components();

            string[] RealNames = new string[] { };

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

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR78;

            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;


            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetRealComponent(RealNames[i]);
                sc.molefraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new PseudoComponent(BPk[i], SG[i], thermo);
                pc.molefraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc.T.Celsius = 370;
            cc.P.BarA = 1;
            cc[0].molefraction = 1;

            FlashClass.Flash(cc);

            var res = cc.BulkEnthalpy();
            Debug.Print(cc.CP_MASS().ToString());
        }


        [TestMethod]
        public void TestQuasiEnthalpy()
        {
            Thermodata data = new Thermodata();
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR76;
            thermo.KMethod = enumEquiKMethod.PR76;

            enumFluidRegion stateVap = enumFluidRegion.Vapour;
            enumFluidRegion stateLiq = enumFluidRegion.Liquid;
            Components cc = new Components();

            PseudoComponent pc = new PseudoComponent(0.6150, new Temperature(273.15 + 38), "Test", thermo);
            pc.Name = "PC" + cc.components.Count.ToString();
            pc.molefraction = 1;
            cc.Add(pc);

            X[0] = 1;

            Thermodynamics.CheckState(cc, X, new Temperature(273), new Pressure(1), thermo);

            double res = Thermodynamics.BulkEnthalpyMix(cc, X, new Temperature(273.15 + 250), new Pressure(6), stateVap);
            double res2 = Thermodynamics.BulkEnthalpyMix(cc, X, new Temperature(273.15 + 251), new Pressure(6), stateVap);
            double Cp = res2 - res;
        }

        [TestMethod]
        public void TestRealEnthalpy()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[2];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            enumFluidRegion state = enumFluidRegion.Vapour;

            double res1, res2, res3 = 0;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("propane");
            cc.Add(sc);

            X[0] = 0.5;
            X[1] = 0.5;

            res1 = Thermodynamics.BulkEnthalpyMix(cc, X, new Temperature(275.15 + 25), new Pressure(3), state);

            X[0] = 1;
            X[1] = 0;

            res2 = Thermodynamics.BulkEnthalpyMix(cc, X, new Temperature(275.15 + 25), new Pressure(3), state);

            X[0] = 0;
            X[1] = 1;

            res3 = Thermodynamics.BulkEnthalpyMix(cc, X, new Temperature(275.15 + 25), new Pressure(3), state);
        }

        [TestMethod]
        public void TestCOMEnthalpyPure()
        {
            Excel_Thermo ethermo = new Excel_Thermo();

            double[] X = new double[1];
            string[] names = new string[2];

            names[0] = "n-hexane";

            X[0] = 1;

            double res = ethermo.LiqEnthalpyReal(names, X, new Temperature(273.15 + 25), new Pressure(4), (int)enumEnthalpy.PR78);
            double res2 = ethermo.VapEnthalpyReal(names, X, new Temperature(273.15 + 25), new Pressure(4), (int)enumEnthalpy.PR78);
            Debug.Print(res.ToString() + " " + res2.ToString());
        }




        [TestMethod]
        public void TestPCEnthalpy2()
        {
            Thermodata data = new Thermodata();
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.Ideal;
            thermo.KMethod = enumEquiKMethod.PR76;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            enumFluidRegion stateVap = enumFluidRegion.Vapour;
            Components cc = new Components();

            PseudoComponent pc = new PseudoComponent(new Temperature(600), 0.9706, thermo);
            pc.Name = "PC" + cc.components.Count.ToString();
            pc.molefraction = 1;
            cc.Add(pc);

            X[0] = 1;

            Thermodynamics.CheckState(cc, X, new Temperature(1000), new Pressure(6), thermo);

            double res = Thermodynamics.BulkEnthalpyMix(cc, X, new Temperature(100), new Pressure(6), stateVap);
            cc.T = new Temperature(100);
            double res3 = Thermodynamics.StreamIdealMolarEnthalpyFast(cc, X);
        }

        [TestMethod]
        public void TestVapEnthalpyIdeal()
        {
            Components cc = new Components();
            BaseComp sc;

            Thermodata data = new Thermodata();

            string[] c = new string[]{"H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
            "n-Butane","i-Pentane","n-Pentane","Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
            "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
            "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
            "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
            "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
            "Quasi825*","Quasi875*"};

            //double[] x = new double[]  {0.11962, 0.00190, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01813, 0.00000, 0.00099, 0.00000, 0.01070, 0.02054, 0.01243, 0.02686, 0.04282, 0.00051, 0.00818, 0.01996, 0.01489, 0.01575, 0.02810, 0.02886, 0.04733, 0.01624, 0.03913, 0.02958, 0.03639, 0.03162, 0.03278, 0.02920, 0.03165, 0.02464, 0.01935, 0.02301, 0.02842, 0.02424, 0.02049, 0.02778, 0.01301, 0.01428, 0.01195, 0.00419, 0.00193, 0.00027, 0.00004, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000 };
            double[] x = new double[] { 0.135732774414621, 0.00215468506415423, 0, 0, 0, 0.0040308022232848, 0, 0.0205695504978483, 0, 0.00112867577145799, 0, 0.012145088837949, 0.0233015561421689, 0.0141024421399901, 0.0304793692227981, 0.0485837079279351, 0.000575215436192718, 0.0092822005770224, 0.0226435863400046, 0.0168918871952106, 0.0178767122687875, 0.0318902910790664, 0.0327534194713751, 0.0537023657943689, 0.0184311210311017, 0.0444009322636192, 0.0335596469258279, 0.0412962160913424, 0.0358771869940697, 0.0371930155690997, 0.0331300014439141, 0.035914641309358, 0.0279557907376905, 0.021952113307905, 0.0261134905810872, 0.0322528765686226, 0.0275004545362876, 0.0232519132782944, 0.0315204595917805, 0.0147578354952504, 0.0162052371926885, 0.0135548574764113, 0.00474909354016855, 0.00219263685340534, 0.000302596460158383, 4.18107775868331E-05, 1.53775892680625E-06, 1.91804783680153E-07, 1.07983510399437E-08, 1.11511408933161E-09, 8.63899097261722E-11, 6.12581908949474E-12, 3.54050464074921E-13, 4.58746710586419E-14, 3.03699384603848E-15, 2.51208135433161E-16, 1.86238776871088E-17, 1.02074204103091E-18, 2.81077119585737E-20, 1.81203359796247E-22, 3.63679596787214E-25, 1.09412087254464E-27, 2.22854161297157E-30 };
            double[] sg = new double[] { 1, 0.06992, 0.80712, 0.80013, 1.13874, 0.29967, 0.38358, 0.35601, 0.82610, 0.78914, 0.52144, 0.50715, 0.56249, 0.58377, 0.62402, 0.63031, 0.61505, 0.63181, 0.65515, 0.67719, 0.69724, 0.71462, 0.72864, 0.73863, 0.74390, 0.74539, 0.75097, 0.76004, 0.77054, 0.78041, 0.78757, 0.79003, 0.79329, 0.80040, 0.80916, 0.81738, 0.82290, 0.82423, 0.82671, 0.83135, 0.83742, 0.84419, 0.85095, 0.85696, 0.86150, 0.86383, 0.86476, 0.86833, 0.87408, 0.88118, 0.88880, 0.89612, 0.90231, 0.90654, 0.91222, 0.91518, 0.91866, 0.92236, 0.92771, 0.93286, 0.93419, 0.93675, 0.94200, 0.94962, 0.95925, 0.97056, 0.98320, 0.99685, 1.01116, 1.02553, 1.03532, 1.04510, 1.05489, 1.07201, 1.09648, 1.12094 };
            double[] bp = new double[] { 373.15, 20.5548, 77.3498, 81.6996, 90.1996, 111.625, 169.399, 184.55, 194.59801, 213.498, 225.399, 231.048, 261.42001, 272.6480103, 301.02802, 309.20901, 311.15, 318.15, 328.15, 338.15, 348.15, 358.15, 368.15, 378.15, 388.15, 398.15, 408.15, 418.15, 428.15, 438.15, 448.15, 458.15, 468.15, 478.15, 488.15, 498.15, 508.15, 518.15, 528.15, 538.15, 548.15, 558.15, 568.15, 578.15, 588.15, 598.15, 608.15, 618.15, 628.15, 638.15, 648.15, 658.15, 668.15, 678.15, 688.15, 698.15, 708.15, 718.15, 733.15, 753.15, 773.15, 793.15, 813.15, 833.15, 853.15, 873.15, 893.15, 913.15, 933.15, 953.15, 973.15, 993.15, 1013.15, 1048.15, 1098.15, 1148.15 };

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.Ideal;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetRealComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new PseudoComponent(new Temperature(bp[i]), sg[i], thermo);
                    pc.molefraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.molefraction = x[i];
                    cc.Add(sc);
                }
            }

            enumCalcResult cres = default(enumCalcResult);
            cc.T = new Temperature(179.8568);
            cc.P = new Pressure(6);
            double res = Thermodynamics.BulkStreamThermo(cc, cc.T, enumMassOrMolar.Molar, enumFluidRegion.Vapour, ref cres).H_ig;
            var res2 = Thermodynamics.StreamIdealMolarEnthalpyFast(cc, x);
            return;
        }

    }
}
