using Extensions;
using System;
using Units.UOM;

namespace ModelEngine.ThermodynamicMethods.Activity_Models.NRTL
{
    public partial class NRTLclass
    {
        private double[][] Params, Alpha2;
        private Components cc;
        private Temperature T;

        public void Init(Components cc, Temperature T, double[][] NRTLParams, double[][] Alpha2)
        {
            this.Params = NRTLParams;
            this.Alpha2 = Alpha2;
            this.T = T;
            this.cc = cc;
        }

        public double[] Solve()
        {
            int NoComps = cc.Count;
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
                        tauIJ[i, j] = Params[i][j] / Rgas / T;
                }

            //  Calc Gij
            for (int i = 0; i < NoComps; i++)
            {
                for (int j = 0; j < NoComps; j++)
                {
                    if (i == j)
                        GIJ[i, j] = 1;
                    else
                        GIJ[i, j] = Math.Exp(-tauIJ[i, j] * Alpha2[i][j]);
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

            return gamma;
        }

        public double[] FitBinaryParams(BaseComp a, BaseComp b, double activity1, double activity2, Temperature T, double alpha = 0.3)
        {
            this.T = T;
            cc = new Components();
            cc.Add(a);
            cc.Add(b);

            Alpha2 = new double[2][];
            Alpha2[0] = new double[2] { alpha, alpha };
            Alpha2[1] = new double[2] { alpha, alpha };

            double[] res1, res2, baseres;
            double[] error = new double[2];
            double[][] Gradient = new double[2][];
            Gradient[0] = new double[2];
            Gradient[1] = new double[2];

            Params = new double[2][];
            Params[0] = new double[2];
            Params[1] = new double[2];

            double[] res;

            int count = 0;
            double delta = 1000;

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

                Params[0][1] -= res[0] * delta / 10;
                Params[1][0] -= res[1] * delta / 10;
            } while (count < 100 && error.SumSQR() > 0.00001);

            if (double.IsNaN(error.SumSQR()) || error.SumSQR() > 0.00001)
                return new double[] { 0, 0 };

            return new double[] { Params[0][1], Params[1][0] };
        }
    }
}