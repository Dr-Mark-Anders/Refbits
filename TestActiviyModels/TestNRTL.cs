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
    public class TestNRTL
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
        public void TestNRTLFitting()
        {
            BaseComp Acetone = Thermodata.GetComponent("Acetone");
            BaseComp TwoPropanol = Thermodata.GetComponent("2-Propanol");
            Components cc = new();

            Temperature T = 273.15 + 68.2;

            double[][] Params = new double[3][];

            cc.Add(Acetone);
            cc.Add(TwoPropanol);

            Acetone.MoleFraction = 0.5;
            TwoPropanol.MoleFraction = 0.5;

            NRTLclass nrtl = new NRTLclass();

            double[][] Alpha = new double[2][];
            Alpha[0] = new double[] { 0.3, 0.3 };
            Alpha[1] = new double[] { 0.3, 0.3 };

            nrtl.Init(cc, T, Params, Alpha);
            var ps = nrtl.FitBinaryParams(Acetone, TwoPropanol, 1.1538, 1.0896, T);

            var activities = nrtl.Solve();

            PrintInfo.PrintPortInfo(activities);
            PrintInfo.PrintPortInfo(ps); // 720 & -278
        }

        [TestMethod]
        public void TestNRTLFitting2()
        {
            BaseComp Acetone = Thermodata.GetComponent("Acetone");
            BaseComp TwoPropanol = Thermodata.GetComponent("H2O");
            Components cc = new();

            Temperature T = 273.15 + 68.2;

            double[][] Params = new double[3][];

            cc.Add(Acetone);
            cc.Add(TwoPropanol);

            Acetone.MoleFraction = 0.5;
            TwoPropanol.MoleFraction = 0.5;

            NRTLclass nrtl = new NRTLclass();

            double[][] Alpha = new double[2][];
            Alpha[0] = new double[] { 0.3, 0.3 };
            Alpha[1] = new double[] { 0.3, 0.3 };

            nrtl.Init(cc, T, Params, Alpha);
            var ps = nrtl.FitBinaryParams(Acetone, TwoPropanol, 1.4819587345065088, 1.645690077825837, T);

            var activities = nrtl.Solve();

            PrintInfo.PrintPortInfo(activities);
            PrintInfo.PrintPortInfo(ps); // 720 & -278
        }

        [TestMethod]
        public void TestNRTLfunction()
        {
            NRTLclass nrtl = new NRTLclass();
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

            double[][] NRTLParams = new double[NoComps][];

            NRTLParams[0] = new double[] { 1, 1114.9, 93.641 };
            NRTLParams[1] = new double[] { -530.98, 1, -16.872 };
            NRTLParams[2] = new double[] { 1622.9, 1640.2, 1 };

            double[][] Alpha = new double[NoComps][];
            Alpha[0] = new double[] { 1, 0.284, 0.324 };
            Alpha[1] = new double[] { 0.284, 1, 0.277 };
            Alpha[2] = new double[] { 0.324, 0.277, 1 };

            nrtl.Init(cc, T, NRTLParams, Alpha);
            var y = nrtl.Solve();

            PrintInfo.PrintPortInfo(y);
        }

        [TestMethod]
        public void TestNRTLSolve()
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

            double[][] NRTLParams = new double[NoComps][];
            double[][] Alpha = new double[NoComps][];
            Alpha[0] = new double[] { 1, 0.284, 0.324 };
            Alpha[1] = new double[] { 0.284, 1, 0.277 };
            Alpha[2] = new double[] { 0.324, 0.277, 1 };

            NRTLParams[0] = new double[] { 1, 1114.9, 93.641 };
            NRTLParams[1] = new double[] { -530.98, 1, -16.872 };
            NRTLParams[2] = new double[] { 1622.9, 1640.2, 1 };

            double[,] tauIJ = new double[NoComps, NoComps];
            double[,] GIJ = new double[NoComps, NoComps];
            double[,] TauGIJ = new double[NoComps, NoComps];
            double[] SumTauGixi = new double[NoComps];
            double[,] Gixi = new double[NoComps, NoComps];
            double[] SumGixi = new double[NoComps];
            double[] Term1 = new double[NoComps];
            double[,] XjGj_SumGijxXi = new double[NoComps, NoComps];
            double[,] XnTauGn = new double[NoComps, NoComps];
            double[,] Term2 = new double[NoComps, NoComps];
            double[] SumOnJ = new double[NoComps];
            double[] y = new double[NoComps];
            double[] gamma = new double[NoComps];

            double Rgas = 1.98721;

            //Cal TauIJ
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                {
                    if (i == j)
                        tauIJ[i, j] = 0;
                    else
                        tauIJ[i, j] = NRTLParams[i][j] / Rgas / T;
                }

            //  Calc Gij
            for (int i = 0; i < NoComps; i++)
            {
                for (int j = 0; j < NoComps; j++)
                {
                    if (i == j)
                        GIJ[i, j] = 1;
                    else
                        GIJ[i, j] = Math.Exp(-tauIJ[i, j] * Alpha[i][j]);
                }
            }

            //calc TauGijXj

            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    TauGIJ[i, j] = tauIJ[i, j] * GIJ[i, j] * cc[i].MoleFraction;

            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    SumTauGixi[i] += TauGIJ[j, i];

            //calc Gixi
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    Gixi[i, j] = GIJ[i, j] * cc[i].MoleFraction;

            SumGixi = new double[NoComps];
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    SumGixi[i] += Gixi[j, i];

            for (int i = 0; i < NoComps; i++)
                Term1[i] = SumTauGixi[i] / SumGixi[i]; // not correct yet

            //XjGj_SumGijxXi
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    XjGj_SumGijxXi[i, j] = cc[i].MoleFraction * GIJ[j, i] / SumGixi[i];

            //XnTauGn
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    XnTauGn[i, j] = cc[i].MoleFraction * tauIJ[i, j] * GIJ[i, j];

            double[] Sumon_n = new double[NoComps];
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    Sumon_n[i] += XnTauGn[j, i];

            //Term2
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    Term2[i, j] = XjGj_SumGijxXi[i, j] * (tauIJ[j, i] - Sumon_n[i] / SumGixi[i]);

            SumOnJ = new double[NoComps];
            for (int i = 0; i < NoComps; i++)
                for (int j = 0; j < NoComps; j++)
                    SumOnJ[i] += Term2[j, i];

            // calc gamma nd y
            for (int i = 0; i < NoComps; i++)
            {
                gamma[i] = Math.Exp(SumOnJ[i] + Term1[i]);
                y[i] = cc[i].MoleFraction * VP(T, i).ATMA * gamma[i];
            }

            PrintInfo.PrintPortInfo(y);
        }
    }
}