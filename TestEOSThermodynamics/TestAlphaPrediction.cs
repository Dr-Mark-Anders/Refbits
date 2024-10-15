using COMColumnNS;
using ModelEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace TestAlphas
{
    [TestClass]
    public class TestTrayAlpha
    {
        [TestMethod]
        public void TestBadMixK()
        {
            Components cc = DefaultStreams.CrudeUrals();

            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;

            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;

            double[] X = new double[] { 0.993789362019244, 0, 0, 0, 0, 0, 0, 8.66160273753792E-06, 0, 0, 0, 0.000189223199500422, 0.000105978489499716, 0.000399527872316555, 6.38769152280496E-05, 6.39539366104141E-05, 4.05555100548711E-05, 0.000142575128903367, 0.000195486693463871, 0.00019189747315718, 0.000192441951538724, 0.000196467138786764, 0.000202608891349124, 0.000217896022656215, 0.000231868364853132, 0.000236074264308874, 0.00023112869072342, 0.000218359252012682, 0.000204781957245967, 0.000201068043915564, 0.000179400751417187, 0.000153280847173246, 0.000138497261371854, 0.000128669859522253, 0.000123820805520862, 0.000123229936671773, 0.000126176190128595, 0.000127599859307455, 0.000126752304401268, 0.000124336565773822, 0.000121770477351014, 0.000117599923368077, 0.000113238257721285, 0.000107263356017383, 0.000101086900943911, 9.46972691979923E-05, 8.67541266634019E-05, 7.86731356454121E-05, 6.92248931144383E-05, 6.10329529012632E-05, 5.57559776331155E-05, 5.19279297761388E-05, 4.72298505939839E-05, 4.23210720961585E-05, 3.68993826105198E-05, 3.15799392060988E-05, 0.000026495443383398, 2.17800866509131E-05, 2.31544801187954E-05, 1.81247110713477E-05, 9.60965827250961E-06, 4.66915389812492E-06, 2.04424952576646E-06, 9.34808719527667E-07, 3.95125616015825E-07, 1.03359991983799E-07, 4.33071532281719E-08, 1.89201138681127E-08, 7.99346694318762E-09, 3.28943662897953E-09, 1.31655635405967E-09, 5.42968701353481E-10, 2.19780892716752E-10, 7.70047350836504E-11, 1.20337063014676E-11, 1.77509703237962E-15 };
            double[] Y = new double[] { 0.902973209681753, 0, 0, 0, 0, 1.9107488820722E-06, 0, 4.24492988511065E-05, 0, 3.86375270873885E-06, 0, 0.000068749984704943, 0.000261766060977038, 0.000197886964537583, 0.000806467595209335, 0.00139404661107624, 1.52168633501899E-05, 0.000288697844952854, 0.00087979510526425, 0.000812046860669605, 0.00103380041931005, 0.00216619107573198, 0.00254585446494286, 0.00467913418768606, 0.00175884563304148, 0.0045572629713295, 0.00366165707215947, 0.00474561935854063, 0.00428220839827463, 0.00455775587020884, 0.00413757451429215, 0.0045596254844586, 0.00363240595364047, 0.00297934716135247, 0.003783553076164, 0.00511252404755604, 0.0047921063606465, 0.00442776047459691, 0.00638955028411952, 0.00312218885965871, 0.00362802890982768, 0.00349497823772486, 0.00172396926892689, 0.00180071498666656, 0.00117564987093385, 0.00130819376609919, 0.000466564011384189, 0.000576844065290026, 0.000310215127249374, 0.000289617776574152, 0.000197140395403674, 0.000119742595497238, 5.90811831373611E-05, 6.60084943137008E-05, 3.77854788719338E-05, 2.78076903591724E-05, 1.87888055568518E-05, 9.60458778476845E-06, 8.82684456532625E-06, 6.66258006952073E-06, 1.65704369528714E-06, 7.17271434432287E-07, 2.39328578510836E-07, 5.48927839001201E-08, 2.72701662341532E-08, 3.74972341086043E-09, 2.03304991517832E-09, 3.51219164780295E-10, 2.47160954217693E-10, 6.34392883269989E-11, 2.2717840203351E-11, 7.17691336267721E-12, 1.45753019702614E-12, 5.04432371312275E-13, 7.98916442162957E-15, 1.52035811730459E-16 };

            TemperatureC T = 98;
            Pressure P = 1;

            cc.NormaliseFractions();
            cc.Thermo = thermo;

            Port_Material p = new(cc);

            var res = ThermodynamicsClass.KMixArray(cc, 3, T, X, Y, out _, thermo);

            COMThermo ct = new();
            double[] res2 = (double[])ct.KMixFixedProps(cc.NameArray, cc.MoleFractions, cc.MoleFractions, cc.SGArray, cc.BPArray.ToDouble(), cc.MWArray, cc.TCritKArray, cc.CritPArray,
                cc.VCritArray, cc.OmegaArray, T, P, (int)enumEquiKMethod.PR78, false);

            Debug.Print(res[0].ToString() + " " + res2[0].ToString());
        }

        [TestMethod]
        public void TestUralsAssayK()
        {
            Components cc = DefaultStreams.CrudeUrals();

            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;

            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;

            TemperatureC T = 100;
            Pressure P = 3;

            cc[0].MoleFraction = 0.999;
            cc.NormaliseFractions();
            cc.Thermo = thermo;

            Port_Material p = new(cc);

            var res = ThermodynamicsClass.KMixArray(cc, 3, T, cc.MoleFractions, cc.MoleFractions, out _, thermo);

            COMThermo ct = new();
            double[] res2 = (double[])ct.KMixFixedProps(cc.NameArray, cc.MoleFractions, cc.MoleFractions, cc.SGArray, cc.BPArray.ToDouble(), cc.MWArray, cc.TCritKArray, cc.CritPArray,
                cc.VCritArray, cc.OmegaArray, T, P, (int)enumEquiKMethod.PR78, false);

            Debug.Print(res[0].ToString() + " " + res2[0].ToString());
        }

        [TestMethod]
        public void TestUralsAssayKCoolWater()
        {
            Components cc = DefaultStreams.CrudeUrals();

            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;

            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;

            TemperatureC T = 100;
            Pressure P = 1;

            cc[0].MoleFraction = 0.999;
            cc.NormaliseFractions();
            cc.Thermo = thermo;

            for (int i = 0; i < cc.Count; i++)
            {
                cc[i].MoleFraction = 0;
            }

            cc[0].MoleFraction = 1;

            Port_Material p = new(cc);

            var res = ThermodynamicsClass.KMixArray(cc, 3, T, cc.MoleFractions, cc.MoleFractions, out _, thermo);

            COMThermo ct = new();
            double[] res2 = (double[])ct.KMixFixedProps(cc.NameArray, cc.MoleFractions, cc.MoleFractions, cc.SGArray, cc.BPArray.ToDouble(), cc.MWArray, cc.TCritKArray, cc.CritPArray,
                cc.VCritArray, cc.OmegaArray, T, P, (int)enumEquiKMethod.PR78, false);

            Debug.Print(res[0].ToString() + " " + res2[0].ToString());
        }

        [TestMethod]
        public void TestAlphas()
        {
            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.UseBIPs = false;
            double[] res;
            Components cc = DefaultStreams.Crude();
            double[] Y = new double[cc.Count];
            double[] X;
            Y[0] = 1;

            TemperatureC T = 100;
            Pressure P = 3;

            res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, Y, out _, cc.Thermo);

            Debug.Print("T = " + T.ToString() + " " + res[18].ToString());
            Tray tray = new(null);

            X = cc.MoleFractions;
            tray.LiqCompositionInitial = X;
            tray.VapCompositionInitial = Y;

            tray.LiqComposition = X;
            tray.VapComposition = Y;

            tray.LiqCompositionPred = X;
            tray.VapCompositionPred = Y;

            tray.T = new Temperature(370, TemperatureUnit.Celsius);
            tray.P = new StreamProperty(Units.ePropID.P, 3);

            tray.KB_MA.DeltaT = 5;

            tray.KTray = new double[X.Length];
            tray.UpdateAlphas(cc, X, Y, ColumnAlphaMethod.LogLinear, thermo);
            tray.EstimateT(cc, X, Y, ColumnTestimateMethod.LinearEstimate2Values);

            Debug.Print((tray.TPredicted - 273.15).ToString());

            tray.UpdateAlphas(cc, X, Y, ColumnAlphaMethod.MA, thermo);
            tray.EstimateT(cc, X, Y, ColumnTestimateMethod.MA);

            Debug.Print((tray.TPredicted - 273.15).ToString());

            double res1 = tray.K_TestFast(0, new Temperature(75, TemperatureUnit.Celsius), ColumnKMethod.MA);

            Debug.Print(res1.ToString());
        }
    }
}