using ModelEngine;
using ModelEngine.ThermodynamicMethods.Activity_Models;
using ModelEngine.ThermodynamicMethods.Activity_Models.NRTL;
using ModelEngine.ThermodynamicMethods.UNIFAC;
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
    public class TestActivityBIPS
    {
        [TestMethod]
        public void SolubilityFromUniquac3Comps()
        {
            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            BaseComp P3Acid = Thermodata.GetRealComponentCAS("79-09-4");
            Components cc = new();

            Temperature T = 273.15 + 25;

            double[][] Params = new double[3][];
            Params[0] = new double[] { 1, 10.74651718, -142.30 };
            Params[1] = new double[] { 1187.46, 1, 579.08 };
            Params[2] = new double[] { 570.61, -327.84, 1 };

            double[] R = new double[] { 0.9200, 3.2479, 2.8768 };
            double[] Q = new double[] { 1.3997, 2.8759, 2.612 };

            cc.Add(H2O);
            cc.Add(MEK);
            cc.Add(P3Acid);

            H2O.MoleFraction = 0.1;
            MEK.MoleFraction = 0.899;
            P3Acid.MoleFraction = 0.001;

            double[] Z = cc.MoleFractions;

            UNIQUAC uniquac = new UNIQUAC();
            uniquac.Init(cc, T, Params, R, Q);
            var activities = uniquac.SolveGamma();
            //  PrintInfo.PrintPortInfo(activities);

            H2O.MoleFraction = 1;
            MEK.MoleFraction = 1e-10;
            P3Acid.MoleFraction = 1e-10;
            var activities1 = uniquac.SolveGamma();
            //  PrintInfo.PrintPortInfo(activities);

            H2O.MoleFraction = 1e-10;
            MEK.MoleFraction = 1;
            P3Acid.MoleFraction = 1e-10;
            var activities2 = uniquac.SolveGamma();

            H2O.MoleFraction = 1e-10;
            MEK.MoleFraction = 1e-10;
            P3Acid.MoleFraction = 1;
            var activities3 = uniquac.SolveGamma();

            H2O.MoleFraction = 0.5;
            MEK.MoleFraction = 0.5;
            P3Acid.MoleFraction = 1e-10;
            var activities4 = uniquac.SolveGamma();
            //  PrintInfo.PrintPortInfo(activities);

            double[] K = new double[3];
            K[0] = activities1[0] / activities2[0];
            K[1] = activities1[1] / activities2[1];
            K[2] = activities3[2] / activities4[2];
            // PrintInfo.PrintPortInfo(K);

            double[] Xa = new double[3];
            double[] Xb = new double[3];
            double Sum = 0, oldsum = 0;

            double fv = 0.5;
            double[] res = new double[3];

            var Lower = C2(Z, 0, K);
            var Upper = C2(Z, 1, K);

            fv = Math.Abs(Lower) / (Lower - Upper);

            for (int n = 0; n < 50; n++)
            {
                for (int i = 0; i < 50; i++)
                {
                    var current = C2(Z, fv, K);
                    var current2 = C2(Z, fv + 0.01, K);

                    var Grad = (current2 - current) / 0.01;
                    var error = current;

                    fv -= error / Grad;

                    if (Math.Abs(current) < 0.0000001)
                        break;
                }

                for (int i = 0; i < K.Length; i++)
                {
                    Xa[i] = Z[i] / (fv * (K[i] - 1) + 1);
                    Xb[i] = Xa[i] * K[i];
                }

                cc.SetMolFractions(Xa);
                activities1 = uniquac.SolveGamma();

                cc.SetMolFractions(Xb);
                activities2 = uniquac.SolveGamma();

                for (int x = 0; x < 3; x++)
                    K[x] = activities1[x] / activities2[x];

                oldsum = Sum;
                Sum = Xa.SumSQR();

                if (Math.Abs(Sum - oldsum) < 0.0000001)
                {
                    PrintInfo.PrintPortInfo(K);
                    break;
                }
            }

            PrintInfo.PrintPortInfo(Xa);
            PrintInfo.PrintPortInfo(Xb);
        }

        public double C2(double[] Z, double fv, double[] Kn)
        {
            double res = 0;

            for (int y = 0; y < 3; y++)
            {
                res += Z[y] * (Kn[y] - 1) / (1 + fv * (Kn[y] - 1));
            }

            return res;
        }

        public double C(double[] Z, double fv, double[] Kn)
        {
            double C = 0;
            int count = Kn.Length;

            if (Kn is null)
                return double.NaN;

            var X = new double[count];
            var Y = new double[count];

            for (int n = 0; n < count; n++) // reset X and Y, only needed if fv reset
            {
                if (fv == 1 && (Kn[n] - 1.0) == -1) // otherwise fails when Kn[n] ~ 0
                    X[n] = 0;
                else
                    X[n] = Z[n] / (1 + fv * (Kn[n] - 1.0));        // Liquid

                Y[n] = Kn[n] * X[n];

                C += X[n] - Y[n];
            }

            return C;
        }

        public double SolveKActivity(Components cc, double[] Kn, ThermoDynamicOptions thermo)
        {
            double c = double.NaN;
            var Z = cc.MoleFractions;

            var X = Z;
            var Y = Z;

            double fv = 0.5;
            for (int i = 0; i < 500; i++)
            {
                double c2 = C(Z, fv + 0.01, Kn);
                c = C(Z, fv, Kn);

                var gradient = (c2 - c) / 0.01;
                var delta = c / gradient;

                fv -= delta;

                if (Math.Abs(c) < FlashClass.C_Tolerance || Math.Abs(delta) < FlashClass.C_Tolerance || fv > 1000)
                    break;
            }

            return fv;
        }

        [TestMethod]
        public void SolubilityFromUniquac()
        {
            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            Components cc = new();

            Temperature T = 273.15 + 25;

            double[][] Params = new double[2][];
            Params[0] = new double[] { 1, 10.74651718 };
            Params[1] = new double[] { 1187.46, 1 };

            double[] R = new double[] { 0.9200, 3.2479 };
            double[] Q = new double[] { 1.3997, 2.8759 };

            cc.Add(H2O);
            cc.Add(MEK);

            H2O.MoleFraction = 0.5;
            MEK.MoleFraction = 0.5;

            UNIQUAC uniquac = new UNIQUAC();
            uniquac.Init(cc, T, Params, R, Q);
            var activities = uniquac.SolveGamma();
            //  PrintInfo.PrintPortInfo(activities);

            H2O.MoleFraction = 1;
            MEK.MoleFraction = 1e-10;
            var activities1 = uniquac.SolveGamma();
            //  PrintInfo.PrintPortInfo(activities);

            H2O.MoleFraction = 1e-10;
            MEK.MoleFraction = 1;
            var activities2 = uniquac.SolveGamma();
            //  PrintInfo.PrintPortInfo(activities);

            double[] K = new double[2];
            K[0] = activities1[0] / activities2[0];
            K[1] = activities1[1] / activities2[1];
            // PrintInfo.PrintPortInfo(K);

            double[] Xa = new double[2];
            double[] Xb = new double[2];
            double Sum = 0, oldsum = 0;

            for (int i = 0; i < 50; i++)
            {
                Xa[0] = (1 - K[1]) / (K[0] - K[1]);
                Xa[1] = 1 - Xa[0];
                Xb[0] = Xa[0] * K[0];
                Xb[1] = 1 - Xb[0];

                cc.SetMolFractions(Xa);
                activities1 = uniquac.SolveGamma();

                cc.SetMolFractions(Xb);
                activities2 = uniquac.SolveGamma();

                K[0] = activities1[0] / activities2[0];
                K[1] = activities1[1] / activities2[1];

                oldsum = Sum;
                Sum = Xa.SumSQR();

                if (Math.Abs(oldsum - Sum) < 0.0000001)
                    break;
            }

            PrintInfo.PrintPortInfo(Xa);
            PrintInfo.PrintPortInfo(Xb);
        }

        [TestMethod]
        public void CreateBIPs()
        {
            Components cc = new();

            Temperature T = 273.15 + 80.37;

            double[][] Params = new double[3][];

            double[] R = new double[] { 2.5735, 2.7791 };
            double[] Q = new double[] { 2.336, 2.508 };

            NRTLclass nrtl = new NRTLclass();

            double[][] Alpha = new double[2][];
            Alpha[0] = new double[] { 0.3, 0.3 };
            Alpha[1] = new double[] { 0.3, 0.3 };

            BaseComp IPA = Thermodata.GetRealComponentCAS("67-63-0");
            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");
            BaseComp Ethanol = Thermodata.GetComponent("ETHANOL");
            BaseComp NPentane = Thermodata.GetComponent("n-Pentane");

            IPA.MoleFraction = 0.5;
            H2O.MoleFraction = 0.5;
            PropionicAcid.MoleFraction = 0.5;
            Ethanol.MoleFraction = 0.5;
            NPentane.MoleFraction = 0.5;

            cc.Add(IPA);
            cc.Add(H2O);
            cc.Add(PropionicAcid);
            cc.Add(Ethanol);
            cc.Add(NPentane);

            UNIFAC uf = new UNIFAC();
            VLEUnifacData data = new VLEUnifacData();
            UNIQUAC uniq = new UNIQUAC();
            Wilsonclass wilson = new Wilsonclass();

            double[,] BIPS = new double[cc.Count, cc.Count];
            double[,] UNIQUACPARAMS = new double[cc.Count, cc.Count];
            double[,] NRTLPARAMS = new double[cc.Count, cc.Count];
            double[,] WILSONPARAMS = new double[cc.Count, cc.Count];

            for (int i = 0; i < cc.Count; i++)
            {
                for (int y = i; y < cc.Count; y++)
                {
                    if (i != y)
                    {
                        double[] bips = uf.Solve(cc[i], cc[y], T, data);
                        BIPS[i, y] = bips[0];
                        BIPS[y, i] = bips[1];
                    }
                }
            }

            for (int i = 0; i < cc.Count; i++)
            {
                for (int y = i; y < cc.Count; y++)
                {
                    if (i != y)
                    {
                        BaseComp A = cc[i];
                        BaseComp B = cc[y];
                        var bips = nrtl.FitBinaryParams(A, B, BIPS[i, y], BIPS[y, i], T);
                        NRTLPARAMS[i, y] = bips[0];
                        NRTLPARAMS[y, i] = bips[1];
                    }
                }
            }

            for (int i = 0; i < cc.Count; i++)
            {
                for (int y = i; y < cc.Count; y++)
                {
                    if (i != y)
                    {
                        BaseComp A = cc[i];
                        BaseComp B = cc[y];
                        double[] bips = uniq.FitBinaryParams(A, B, BIPS[i, y], BIPS[y, i], T);
                        UNIQUACPARAMS[i, y] = bips[0];
                        UNIQUACPARAMS[y, i] = bips[1];
                    }
                }
            }

            for (int i = 0; i < cc.Count; i++)
            {
                for (int y = i; y < cc.Count; y++)
                {
                    if (i != y)
                    {
                        BaseComp A = cc[i];
                        BaseComp B = cc[y];
                        // double [] V = new  double [] { 74.05, 76.92 };
                        // wilson.Init(cc, T, Params, V);
                        double[] bips = wilson.FitBinaryParams(A, B, BIPS[i, y], BIPS[y, i], T);
                        WILSONPARAMS[i, y] = bips[0];
                        WILSONPARAMS[y, i] = bips[1];
                    }
                }
            }

            PrintInfo.PrintPortInfo(BIPS);
            PrintInfo.PrintPortInfo(NRTLPARAMS);
            PrintInfo.PrintPortInfo(UNIQUACPARAMS);
            PrintInfo.PrintPortInfo(WILSONPARAMS);
        }

        [TestMethod]
        public void Test2Comps()
        {
            UNIFAC uf = new UNIFAC();

            BaseComp NPentane = Thermodata.GetComponent("n-Pentane");

            BaseComp IPA = Thermodata.GetRealComponentCAS("67-63-0");
            BaseComp H2O = Thermodata.GetComponent("H2O");

            IPA.MoleFraction = 0.5;
            H2O.MoleFraction = 0.5;

            Components cc = new();

            cc.Add(IPA);
            cc.Add(H2O);

            Temperature T = 273.15 + 80.37;
            VLEUnifacData data = new();

            var activity = uf.SolveActivity(cc, cc.MoleFractions, T, data);
            PrintInfo.PrintPortInfo(activity);

            UNIQUAC uniq = new();
            double[] bips = uniq.FitBinaryParams(IPA, H2O, activity[0], activity[1], T);

            PrintInfo.PrintPortInfo(bips);

            NRTLclass nrtl = new();
            double[] bipsnrtl = nrtl.FitBinaryParams(IPA, H2O, activity[0], activity[1], T);

            PrintInfo.PrintPortInfo(bipsnrtl);
        }

        [TestMethod]
        public void Test2CompsWater_Pentane()
        {
            UNIFAC uf = new UNIFAC();

            BaseComp NPentane = Thermodata.GetComponent("n-Pentane");
            BaseComp H2O = Thermodata.GetComponent("H2O");

            NPentane.MoleFraction = 0.5;
            H2O.MoleFraction = 0.5;

            Components cc = new();

            cc.Add(NPentane);
            cc.Add(H2O);

            Temperature T = 273.15 + 80.37;
            VLEUnifacData data = new();

            var activity = uf.SolveActivity(cc, cc.MoleFractions, T, data);
            PrintInfo.PrintPortInfo(activity);

            UNIQUAC uniq = new();
            double[] bips = uniq.FitBinaryParams(NPentane, H2O, activity[0], activity[1], T);

            PrintInfo.PrintPortInfo(bips);

            NRTLclass nrtl = new();
            double[] bipsnrtl = nrtl.FitBinaryParams(NPentane, H2O, activity[0], activity[1], T);

            PrintInfo.PrintPortInfo(bipsnrtl);
        }

        [TestMethod]
        public void MatrixMultiply()
        {
            int m = 2, n = 3, p = 3, q = 3, i, j;
            int[,] a = { { 1, 4, 2 }, { 2, 5, 1 } };
            int[,] b = { { 3, 4, 2 }, { 3, 5, 7 }, { 1, 2, 1 } };
            Console.WriteLine("Matrix a:");
            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n; j++)
                {
                    Console.Write(a[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Matrix b:");
            for (i = 0; i < p; i++)
            {
                for (j = 0; j < q; j++)
                {
                    Console.Write(b[i, j] + " ");
                }
                Console.WriteLine();
            }
            if (n != p)
            {
                Console.WriteLine("Matrix multiplication not possible");
            }
            else
            {
                int[,] c = new int[m, q];
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < q; j++)
                    {
                        c[i, j] = 0;
                        for (int k = 0; k < n; k++)
                        {
                            c[i, j] += a[i, k] * b[k, j];
                        }
                    }
                }
                Console.WriteLine("The product of the two matrices is :");
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        Console.Write(c[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}