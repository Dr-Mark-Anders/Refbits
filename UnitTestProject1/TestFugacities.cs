using EngineThermo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestFugacities
    {
        [TestMethod]
        public void TestFugacity()
        {
            Thermodata data = new Thermodata();
            Excel_Thermo ethermo = new Excel_Thermo();
            Pressure Press = 12;

            double[] X = new double[3];
            double[] Y = new double[3];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR78;

            double[] res = new double[3];
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

            //res = (double[])ethermo.LnkRealMix(names, X, Y, (273.15 + 25), p, (int)enumEnthalpy.PR78);
            res = (double[])ethermo.KMixPureArray(names, X, Y, (273.15 + 25), Press, (int)enumEnthalpy.PR78);
            //res = (double[])ethermo.KMixPureandQuasiArray(names,  new double[] { 0,0,0}, new double[] { 0, 0,0 },X, Y,(273.15 + 25),  p, (int)enumEnthalpy.PR78, false);
            //var res1 = ethermo.KMixPureandQuasi(names, new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 }, X, Y, (273.15 + 25), p, (int)enumEnthalpy.PR78,0);

            //            Debug.Print(res.ToString() + " " + res2.ToString());
        }
    }
}
