using Extensions;
using System;
using Units.UOM;

namespace ModelEngine.ThermodynamicMethods.Activity_Models.NRTL
{
    public class Wilsonclass
    {
        private double[][] Params;
        private double[] V;
        private Components cc;
        private Temperature t;

        public void Init(Components cc, Temperature T, double[][] Params, double[] V)
        {
            this.Params = Params;
            this.V = V;
            this.cc = cc;
            this.t = T;
        }

        public double[] Solve()
        {
            int NoComps = cc.Count;
            double[,] VRatios = new double[NoComps, NoComps];

            for (int i = 0; i < NoComps; i++)
                for (int yy = 0; yy < NoComps; yy++)
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
                    Lambda[i, j] = VRatios[i, j] * Math.Exp(-Params[j][i] / Rgas / t);

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
                y[i] = cc[i].MoleFraction * VP(t, i).ATMA * gamma[i];
            }

            return gamma;
        }

        public double[] FitBinaryParams(BaseComp a, BaseComp b, double activity1, double activity2, Temperature T)
        {
            this.t = T;
            cc = new Components();
            cc.Add(a);
            cc.Add(b);

            double[] res1, res2, baseres;
            double[] error = new double[2];
            double[][] Gradient = new double[2][];
            Gradient[0] = new double[2];
            Gradient[1] = new double[2];

            Params = new double[2][];
            Params[0] = new double[2];
            Params[1] = new double[2];

            V = new double[] { a.StandardMolarVolume, b.StandardMolarVolume };

            double[] res;
            double delta = 1000;
            int count = 0;

            do
            {
                count++;
                baseres = Solve();

                error[0] = activity1 - baseres[0];
                error[1] = activity2 - baseres[1];

                Params[0][1] += delta;
                res1 = Solve();
                Params[0][1] -= delta;

                Params[1][0] += delta;
                res2 = Solve();
                Params[1][0] -= delta;

                Gradient[0][0] = baseres[0] - res1[0];
                Gradient[1][0] = baseres[1] - res1[1];
                Gradient[0][1] = baseres[0] - res2[0];
                Gradient[1][1] = baseres[1] - res2[1];

                var MInv = MatrixInverse.MatrixInverseProgram.MatrixInverse(Gradient);

                res = MInv.Mult(error);

                Params[0][1] -= res[0] * delta;
                Params[1][0] -= res[1] * delta;
            } while (count < 100 && error.SumSQR() > 0.000000001);

            if (double.IsNaN(error.SumSQR()) || error.SumSQR() > 0.000000001)
                return new double[] { 0, 0 };

            return new double[] { Params[0][1], Params[1][0] };
        }

        public void TestWilson()
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
        }

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
    }
}