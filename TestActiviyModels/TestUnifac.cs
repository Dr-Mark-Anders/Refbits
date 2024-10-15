using ModelEngine;
using ModelEngine.ThermodynamicMethods.UNIFAC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using Units.UOM;

namespace TestActivityModels
{
    /// <summary>
    /// Summary description for UnitTest4
    /// </summary>
    [TestClass]
    public class TestUNIFAC
    {
        public Pressure VP(Temperature T, int i)
        {
            double[][] VP = new double[3][];
            VP[0] = new double[3] { 7.1171, 1210.6, 229.66 };
            VP[1] = new double[3] { 8.8783, 2010.3, 252.64 };
            VP[2] = new double[3] { 8.0713, 1730.6, 233.43 };
            Pressure res = new Pressure();

            res.ATMA = Math.Pow(10, (VP[i][0] - VP[i][1] / (T.Celsius + VP[i][2]))) / 760;

            return res;
        }

        [TestMethod]
        public void TestUNIFAC_VLE()
        {
            UNIFAC uf = new UNIFAC();

            /* BaseComp H2O = Thermodata.GetRealComponent("H2O");
             BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
             BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");
             BaseComp Ethanol = Thermodata.GetRealComponent("ETHANOL");
             BaseComp NPentane = Thermodata.GetRealComponent("n-Pentane");*/

            BaseComp IPA = Thermodata.GetRealComponentCAS("67-63-0");
            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");
            BaseComp Ethanol = Thermodata.GetComponent("ETHANOL");
            BaseComp NPentane = Thermodata.GetComponent("n-Pentane");

            IPA.MoleFraction = 0.5;
            H2O.MoleFraction = 0.5;
            PropionicAcid.MoleFraction = 1e-20;
            Ethanol.MoleFraction = 1e-20;
            NPentane.MoleFraction = 1e-20;

            Components cc = new();

            cc.Add(IPA);
            cc.Add(H2O);
            cc.Add(PropionicAcid);
            cc.Add(Ethanol);
            cc.Add(NPentane);

            var activity = uf.SolveActivity(cc, cc.MoleFractions, 273.15 + 80.37, new VLEUnifacData());
            PrintInfo.PrintPortInfo(activity);
        }

        [TestMethod]
        public void TestUNIFAC_LLE()
        {
            UNIFAC uf = new UNIFAC();

            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");
            BaseComp Ethanol = Thermodata.GetComponent("ETHANOL");
            BaseComp NPentane = Thermodata.GetComponent("n-Pentane");
            Components cc = new();

            H2O.MoleFraction = 0.830;
            MEK.MoleFraction = 0.100;
            PropionicAcid.MoleFraction = 0.070;
            Ethanol.MoleFraction = 1e-20;
            NPentane.MoleFraction = 1e-20;

            cc.Add(H2O);
            cc.Add(MEK);
            cc.Add(PropionicAcid);
            cc.Add(Ethanol);
            cc.Add(NPentane);

            var activity = uf.SolveActivity(cc, cc.MoleFractions, 273.15 + 25, new LLEUnifacData());
            PrintInfo.PrintPortInfo(activity);
        }

        [TestMethod]
        public void TestUNIFAC_LLE3()
        {
            UNIFAC uf = new UNIFAC();

            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");
            Components cc = new();

            H2O.MoleFraction = 0.0999;
            MEK.MoleFraction = 0.899101;
            PropionicAcid.MoleFraction = 0.000999;

            cc.Add(H2O);
            cc.Add(MEK);
            cc.Add(PropionicAcid);

            cc.NormaliseFractions();

            LLEUnifacData LLE = new();

            cc.SetMolFractions(new double[] { 1, 0, 0 });
            var activity = uf.SolveActivity(cc, cc.MoleFractions, 273.15 + 25, LLE);

            cc.SetMolFractions(new double[] { 0, 1, 0 });
            var activity2 = uf.SolveActivity(cc, cc.MoleFractions, 273.15 + 25, LLE);

            double[] K = new double[3];

            for (int i = 0; i < 3; i++)
            {
                K[i] = activity[i] / activity2[i];
            }

            PrintInfo.PrintPortInfo(activity);
            PrintInfo.PrintPortInfo(activity2);
            PrintInfo.PrintPortInfo(K);
        }
    }
}