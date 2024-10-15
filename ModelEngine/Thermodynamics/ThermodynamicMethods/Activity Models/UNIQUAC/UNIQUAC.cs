using Extensions;
using System;
using System.Linq;
using Units.UOM;

namespace ModelEngine.ThermodynamicMethods.Activity_Models
{
    public class UNIQUAC
    {
        private double[][] Params;
        private double[] R;
        private double[] Q;
        private Components cc;
        private Temperature t;

        public void Init(Components cc, Temperature t, double[][] Params, double[] R, double[] Q)
        {
            this.cc = cc;
            this.t = t;
            this.Params = Params;
            this.R = R;
            this.Q = Q;
        }

        public void SetParams(double[][] Params)
        {
            this.Params = Params;
        }

        public double[] SolveGamma()
        {
            double[] gamma = SolveLnGamma();

            for (int i = 0; i < gamma.Length; i++)
            {
                Math.Exp(gamma[i]);
            }

            return gamma;
        }

        public double[] SolveLnGamma()
        {
            Temperature T = t;

            int NoComps = cc.Count;
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
            double Rgas = 1.98721;
            double z = 10;

            double MoleFrac = 0.0000000001;
            double[] TempMoleFrac = new double[NoComps];

            for (int i = 0; i < NoComps; i++)
            {
                if (cc[i].MoleFraction == 0)
                    TempMoleFrac[i] = MoleFrac;
                else
                    TempMoleFrac[i] = cc[i].MoleFraction;
            }

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
                xl[i] = l[i] * TempMoleFrac[i];
                qx[i] = Q[i] * TempMoleFrac[i];
            }

            for (int i = 0; i < NoComps; i++)
            {
                theta[i] = qx[i] / qx.Sum();
                rx[i] = R[i] * TempMoleFrac[i];
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
                gammac[i] = Math.Log(phi[i] / TempMoleFrac[i]) + z / 2 * Q[i] * Math.Log(theta[i] / phi[i])
                    + l[i] - phi[i] / TempMoleFrac[i] * xl.Sum();

                gammar[i] = Q[i] * (1 - Math.Log(sumThetaTau[i]) - Sum_theta_Tau_SumKThetaTau[i]);

                gamma[i] = gammac[i] + gammar[i];
            }

            return gamma;
        }

        public double[] FitBinaryParams(BaseComp A, BaseComp B, double activity1, double activity2, Temperature T)
        {
            this.t = T;
            cc = new Components();
            cc.Add(A);
            cc.Add(B);

            double[] res1, res2, baseres;
            double[] error = new double[2];
            double[][] Gradient = new double[2][];
            Gradient[0] = new double[2];
            Gradient[1] = new double[2];

            Params = new double[2][];
            Params[0] = new double[2];
            Params[1] = new double[2];

            R = new double[2] { A.UnifacR, B.UnifacR };
            Q = new double[2] { A.UnifacQ, B.UnifacQ };
            double delta = 1000;
            double[] res;

            int count = 0;

            do
            {
                count++;
                baseres = SolveGamma();

                error[0] = activity1 - baseres[0];
                error[1] = activity2 - baseres[1];

                Params[0][1] += delta;
                res1 = SolveGamma();
                Params[0][1] -= delta;

                Params[1][0] += delta;
                res2 = SolveGamma();
                Params[1][0] -= delta;

                Gradient[0][0] = baseres[0] - res1[0];
                Gradient[1][0] = baseres[1] - res1[1];
                Gradient[0][1] = baseres[0] - res2[0];
                Gradient[1][1] = baseres[1] - res2[1];

                var MInv = MatrixInverse.MatrixInverseProgram.MatrixInverse(Gradient);

                res = MInv.Mult(error);

                Params[0][1] -= res[0] * delta;
                Params[1][0] -= res[1] * delta;
            } while (count < 100 && error.SumSQR() > 0.00001);

            if (double.IsNaN(error.SumSQR()) || error.SumSQR() > 0.00001)
                return new double[] { 0, 0 };

            return new double[] { Params[0][1], Params[1][0] };
        }

        public void TestUNIQUAC()
        {
            UNIQUAC uniquac = new UNIQUAC();
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
                theta[i] = qx[i] / qx.Sum();
                rx[i] = R[i] * cc[i].MoleFraction;
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