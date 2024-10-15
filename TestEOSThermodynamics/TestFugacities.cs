using COMColumnNS;
using ModelEngine;
using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System.Diagnostics;
using Units.UOM;

namespace UnitTests
{
    [TestClass]
    public class TestFugacities
    {
        [TestMethod]
        public void TestKmixCrude()
        {
            FlowSheet fs = new();
            Components cc;

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            cc = DefaultStreams.ColumnGas(thermo);

            Pressure P = 0.001;
            Temperature T = new Temperature(25 + 275.15);

            P = 39.3;
            P = 40.5;

            Temperature bubblepoint = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
            Temperature dewpoint = ThermodynamicsClass.DewPoint(cc, P, thermo, out _);
            var res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out enumFluidRegion state, thermo);
            double KMXTot = res.SumProduct(cc.MoleFractions);

            Pressure PCrit = cc.PCritMix();
            Temperature TCrit = cc.TCritMix();

            for (int i = 0; i < 2000; i++)
            {
                P.BaseValue = i / 10d;

                if (P > PCrit)
                    break;

                bubblepoint = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
                dewpoint = ThermodynamicsClass.DewPoint(cc, P, thermo, out _);
                res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out state, thermo);
                KMXTot = res.SumProduct(cc.MoleFractions);

                Debug.Print("Bubble: " + P.BaseValue.ToString() + " " + KMXTot + " " + bubblepoint.ToString());
                Debug.Print("Dew:    " + P.BaseValue.ToString() + " " + KMXTot + " " + dewpoint.ToString());
            }
        }

        [TestMethod]
        public void TestFugacity()
        {
            COMThermo ethermo = new();
            Pressure Press = 12;

            double[] X = new double[3];
            double[] Y = new double[3];
            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            string[] names = new string[3];

            names[0] = "propane";
            names[1] = "n-butane";
            names[2] = "n-C30";

            X[0] = 0.00650;
            X[1] = 0.94004;
            X[2] = 0.05345;

            Y[0] = 0.02872;
            Y[1] = 0.97128;
            Y[2] = 0.0;

            //res = (double [])ethermo.LnkRealMix(names, X, Y, (273.15 + 25), p, (int )enumEnthalpy.PR78);
            double[] res = (double[])ethermo.KMixPureArray(names, X, Y, (273.15 + 25), Press, (int)enumEnthalpy.PR78);
            //res = (double [])ethermo.KMixPureandQuasiArray(names,  new  double [] { 0,0,0}, new  double [] { 0, 0,0 },X, Y,(273.15 + 25),  p, (int )enumEnthalpy.PR78, false);
            //var res1 = ethermo.KMixPureandQuasi(names, new  double [] { 0, 0, 0 }, new  double [] { 0, 0, 0 }, X, Y, (273.15 + 25), p, (int )enumEnthalpy.PR78,0);

            //            Debug.Print (res.ToString() + " " + res2.ToString());
        }
    }
}