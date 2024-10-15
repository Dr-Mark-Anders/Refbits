using ModelEngine;
using ModelEngine.ThermodynamicMethods.Activity_Models;
using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using Units.UOM;

namespace TestActivityModels
{
    /// <summary>
    /// Summary description for UnitTest4
    /// </summary>
    [TestClass]
    public class TestUNIQUAC
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
        public void TestUNIQUACfunction()
        {
            BaseComp Acetone = Thermodata.GetComponent("Acetone");
            BaseComp TwoPropanol = Thermodata.GetComponent("2-Propanol");
            BaseComp H2O = Thermodata.GetComponent("H2O");
            Components cc = new();

            Temperature T = 273.15 + 68.2;

            double[][] Params = new double[3][];

            Params[0] = new double[] { 1, 363.87, 275.96 };
            Params[1] = new double[] { -175.06, 1, 438.84 };
            Params[2] = new double[] { 177.12, 17.934, 1 };

            double[] R = new double[] { 2.5735, 2.7791, 0.9200 };
            double[] Q = new double[] { 2.336, 2.508, 1.400 };

            cc.Add(Acetone);
            cc.Add(TwoPropanol);
            cc.Add(H2O);

            Acetone.MoleFraction = 0.294;
            TwoPropanol.MoleFraction = 0.484;
            H2O.MoleFraction = 0.222;

            UNIQUAC uniquac = new UNIQUAC();
            uniquac.Init(cc, T, Params, R, Q);
            var activities = uniquac.SolveGamma();

            PrintInfo.PrintPortInfo(activities);
        }

        [TestMethod]
        public void TestUNIQUACFitting()
        {
            BaseComp Acetone = Thermodata.GetComponent("Acetone");
            BaseComp TwoPropanol = Thermodata.GetComponent("2-Propanol");
            Components cc = new();

            Temperature T = 273.15 + 68.2;

            double[][] Params = new double[2][];

            double[] R = new double[] { 2.5735, 2.7791 };
            double[] Q = new double[] { 2.336, 2.508 };

            cc.Add(Acetone);
            cc.Add(TwoPropanol);

            Acetone.MoleFraction = 0.5;
            TwoPropanol.MoleFraction = 0.5;

            UNIQUAC uniquac = new UNIQUAC();

            uniquac.Init(cc, T, Params, R, Q);
            var ps = uniquac.FitBinaryParams(Acetone, TwoPropanol, 1.1538, 1.0896, T);

            var activities = uniquac.SolveGamma();

            PrintInfo.PrintPortInfo(activities);
            PrintInfo.PrintPortInfo(ps); // 720 & -278
        }

        [TestMethod]
        public void TestUNIQUACSolve()
        {
            BaseComp Acetone = Thermodata.GetComponent("Acetone");
            BaseComp TwoPropanol = Thermodata.GetComponent("2-Propanol");
            BaseComp H2O = Thermodata.GetComponent("H2O");
            Components cc = new();

            Temperature T = 273.15 + 68.2;

            cc.Add(Acetone);
            cc.Add(TwoPropanol);
            cc.Add(H2O);

            Acetone.MoleFraction = 0.294;
            TwoPropanol.MoleFraction = 0.484;
            H2O.MoleFraction = 0.222;

            int NoComps = cc.Count;

            double[][] Params = new double[3][];

            Params[0] = new double[] { 1, 363.87, 275.96 };
            Params[1] = new double[] { -175.06, 1, 438.84 };
            Params[2] = new double[] { 177.12, 17.934, 1 };

            double[,] tauIJ = new double[NoComps, NoComps];
            double[] l = new double[NoComps];
            double[] xl = new double[NoComps];
            double[] qx = new double[NoComps];
            double[] theta = new double[NoComps];
            double[] rx = new double[NoComps];
            double[] phi = new double[NoComps];
            double[,] theta_tau = new double[NoComps, NoComps];
            double[] sumThetaTau = new double[NoComps];
            double[,] theta_Tau_SumKThetaTau = new double[NoComps, NoComps];
            double[] gammac = new double[NoComps];
            double[] gammar = new double[NoComps];
            double[] gamma = new double[NoComps];
            double[] y = new double[NoComps];
            double[] R = new double[] { 2.5735, 2.7791, 0.9200 };
            double[] Q = new double[] { 2.336, 2.508, 1.400 };
            double Rgas = 1.98721;
            double z = 10;

            for (int i = 0; i < NoComps; i++)
            {
                for (int j = 0; j < NoComps; j++)
                {
                    if (i == j)
                        tauIJ[i, j] = 1;
                    else
                        tauIJ[i, j] = Math.Exp(-Params[i][j] / Rgas / T);
                }
                l[i] = z / 2 * (R[i] - Q[i]) - (R[i] - 1);
            }

            for (int i = 0; i < NoComps; i++)
            {
                xl[i] = l[i] * cc[i].MoleFraction;
                qx[i] = Q[i] * cc[i].MoleFraction;
            }

            for (int i = 0; i < NoComps; i++)
            {
                theta[i] = qx[i] / qx.Sum();
                rx[i] = R[i] * cc[i].MoleFraction;
            }

            for (int i = 0; i < NoComps; i++)
            {
                phi[i] = rx[i] / rx.Sum();
            }

            //thetatau
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    theta_tau[i, j] = theta[i] * tauIJ[i, j];

            sumThetaTau = new double[NoComps];
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    sumThetaTau[i] += theta_tau[j, i];

            //thetatau
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    theta_Tau_SumKThetaTau[i, j] = tauIJ[j, i] * theta[i] / sumThetaTau[i];

            double[] Sum_theta_Tau_SumKThetaTau = new double[3];
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    Sum_theta_Tau_SumKThetaTau[j] += theta_Tau_SumKThetaTau[i, j];

            for (int i = 0; i < NoComps; i++)
            {
                gammac[i] = Math.Log(phi[i] / cc[i].MoleFraction) + z / 2 * Q[i] * Math.Log(theta[i] / phi[i])
                    + l[i] - phi[i] / cc[i].MoleFraction * xl.Sum();
                gammar[i] = Q[i] * (1 - Math.Log(sumThetaTau[i]) - Sum_theta_Tau_SumKThetaTau[i]);
                gamma[i] = Math.Exp(gammac[i] + gammar[i]);
                y[i] = cc[i].MoleFraction * VP(T, i).ATMA * gamma[i];
            }

            PrintInfo.PrintPortInfo(y);
        }

        [TestMethod]
        public void TestUNIQUAC_LLE3()
        {
            Components cc = new();
            UNIQUAC uq = new UNIQUAC();

            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");

            H2O.MoleFraction = 0.0999;
            MEK.MoleFraction = 0.899101;
            PropionicAcid.MoleFraction = 0.000999;

            cc.Add(H2O);
            cc.Add(MEK);
            cc.Add(PropionicAcid);

            cc.NormaliseFractions();

            double[][] Params = new double[3][];

            Params[0] = new double[] { 1, 10.75, -142.30 };
            Params[1] = new double[] { 1187.00, 1, 579.08 };
            Params[2] = new double[] { 570.61, -327.84, 1 };

            double[] R = new double[] { 0.9200, 3.2479, 2.8768 };
            double[] Q = new double[] { 1.3997, 2.8759, 2.612 };

            uq.Init(cc, 273.15 + 25, Params, R, Q);

            cc.SetMolFractions(new double[] { 1, 0, 0 });
            var activity = uq.SolveGamma();

            cc.SetMolFractions(new double[] { 0, 0.5, 0.5 });
            var activity2 = uq.SolveGamma();

            double[] K = new double[3];

            for (int i = 0; i < 3; i++)
            {
                K[i] = activity[i] / activity2[i];
            }

            //PrintInfo.PrintPortInfo(activity);
            //PrintInfo.PrintPortInfo(activity2);
            PrintInfo.PrintPortInfo(K);
        }
    }
}