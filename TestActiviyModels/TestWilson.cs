using ModelEngine;
using ModelEngine.ThermodynamicMethods.Activity_Models.NRTL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using Units.UOM;

namespace TestActivityModels
{
    /// <summary>
    /// Summary description for UnitTest4
    /// </summary>
    [TestClass]
    public class TestWilson
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
        public void TestWilsonfunction()
        {
            Wilsonclass wilson = new Wilsonclass();
            BaseComp Acetone = Thermodata.GetComponent("Acetone");
            BaseComp TwoPropanol = Thermodata.GetComponent("2-Propanol");
            BaseComp H2O = Thermodata.GetComponent("H2O");
            Components cc = new();

            Temperature T = 273.15 + 68.2;

            Acetone.MoleFraction = 0.294;
            TwoPropanol.MoleFraction = 0.484;
            H2O.MoleFraction = 0.222;

            cc.Add(Acetone);
            cc.Add(TwoPropanol);
            cc.Add(H2O);

            int NoComps = cc.Count;

            double[][] Params = new double[NoComps][];
            Params[0] = new double[] { 1, -203.11, 679.49 };
            Params[1] = new double[] { 593.18, 1, 556.39 };
            Params[2] = new double[] { 1251.9, 1294.7, 1 };

            double[] V = new double[] { 74.05, 76.92, 18.07 };

            double[,] VRatios = new double[NoComps, NoComps];

            for (int i = 0; i < NoComps; i++)
                for (int yy = 0; yy < 3; yy++)
                    VRatios[i, yy] = V[i] / V[yy];

            wilson.Init(cc, T, Params, V);

            var y = wilson.Solve();
            PrintInfo.PrintPortInfo(y);
        }

        [TestMethod]
        public void TestWilsonSolve()
        {
            BaseComp Acetone = Thermodata.GetComponent("Acetone");
            BaseComp TwoPropanol = Thermodata.GetComponent("2-Propanol");
            BaseComp H2O = Thermodata.GetComponent("H2O");
            Components cc = new();

            Temperature T = 273.15 + 68.2;

            Acetone.MoleFraction = 0.294;
            TwoPropanol.MoleFraction = 0.484;
            H2O.MoleFraction = 0.222;

            cc.Add(Acetone);
            cc.Add(TwoPropanol);
            cc.Add(H2O);

            int NoComps = cc.Count;

            double[][] Params = new double[NoComps][];
            Params[0] = new double[] { 1, -203.11, 679.49 };
            Params[1] = new double[] { 593.18, 1, 556.39 };
            Params[2] = new double[] { 1251.9, 1294.7, 1 };

            double[] V = new double[] { 74.05, 76.92, 18.07 };

            double[,] VRatios = new double[NoComps, NoComps];

            for (int i = 0; i < NoComps; i++)
                for (int yy = 0; yy < 3; yy++)
                    VRatios[i, yy] = V[i] / V[yy];

            double Rgas = 1.98721;
            double[,] Lambda = new double[NoComps, NoComps];
            double[,] IJ = new double[NoComps, NoComps];
            double[,] k_i = new double[NoComps, NoComps];
            double[] SumIJ = new double[NoComps];
            double[] SumOnK = new double[NoComps];
            double[] SumOnJ = new double[NoComps];
            double[] y = new double[NoComps];
            double[] lngamma = new double[NoComps];
            double[] gamma = new double[NoComps];

            //Cal TauIJ
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    Lambda[i, j] = VRatios[i, j] * Math.Exp(-Params[j][i] / Rgas / T);

            //  Calc ij
            for (int i = 0; i < NoComps; i++)
            {
                for (int j = 0; j < NoComps; j++)
                {
                    if (i == j)
                        IJ[i, j] = cc[i].MoleFraction;
                    else
                        IJ[i, j] = Lambda[j, i] * cc[j].MoleFraction; // not correct yet

                    SumIJ[i] += IJ[i, j];
                }
            }

            SumOnK = new double[NoComps];

            //calc k/i
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    k_i[i, j] = cc[i].MoleFraction * Lambda[j, i] / SumIJ[i]; // not correct yet

            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    SumOnK[i] += k_i[j, i];

            // calc gamma nd y
            for (int i = 0; i < NoComps; i++)
            {
                lngamma[i] = -Math.Log(SumIJ[i]) + 1 - SumOnK[i];
                gamma[i] = Math.Exp(lngamma[i]);
                y[i] = cc[i].MoleFraction * VP(T, i).ATMA * gamma[i];
            }

            PrintInfo.PrintPortInfo(y);
        }

        [TestMethod]
        public void TestWilsonFitting()
        {
            BaseComp Acetone = Thermodata.GetComponent("Acetone");
            BaseComp TwoPropanol = Thermodata.GetComponent("2-Propanol");
            BaseComp H2O = Thermodata.GetComponent("H2O");
            Components cc = new();

            Wilsonclass wilson = new Wilsonclass();

            Temperature T = 273.15 + 68.2;

            Acetone.MoleFraction = 0.5;
            TwoPropanol.MoleFraction = 0.5;
            //H2O.MoleFraction = 0.222;

            cc.Add(Acetone);
            cc.Add(TwoPropanol);
            //cc.Add(H2O);

            int NoComps = cc.Count;

            double[][] Params = new double[NoComps][];
            Params[0] = new double[2];
            Params[1] = new double[2];

            double[] V = new double[] { 74.05, 76.92 };

            wilson.Init(cc, T, Params, V);

            var y = wilson.FitBinaryParams(Acetone, TwoPropanol, 1.1538, 1.0896, T);

            var activities = wilson.Solve();
            PrintInfo.PrintPortInfo(activities);
            PrintInfo.PrintPortInfo(y);
        }
    }
}