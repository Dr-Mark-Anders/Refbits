using System;
using Units.UOM;

namespace NaphthaReformerUS
{
    public partial class NapReformerUS
    {
        private void KIdeal(Temperature TSep, Pressure PSep, double[] EqK)
        {
            double[] GamLog = new double[32];
            double[] FLLog = new double[32];
            double[] FVLog = new double[32];
            double Term;

            for (int I = 1; I <= 31; I++)
            {
                GamLog[I] = 0.0;
                double TR = TSep.Rankine / TCritR[I];
                double PR = PSep.PSIA / PCritPSIA[I];
                double TR2 = TR * TR;
                double TR3 = TR2 * TR;
                double PR2 = PR * PR;
                if (I == 1)
                {
                    Term = 1.96718 + 1.02972 / TR - 0.054009 * TR + 0.0005288 * TR2
                        + 0.008585 * PR;
                }
                else if (I == 2)
                {
                    Term = 2.4384 - 2.2455 / TR - 0.34084 * TR + 0.00212 * TR2 - 0.00223 * TR3
                           + PR * (0.10486 - 0.03691 * TR);
                }
                else
                {
                    Term = 5.75748 - 3.01761 / TR - 4.985 * TR + 2.02299 * TR2
                           + PR * (0.08427 + 0.26667 * TR - 0.31138 * TR2)
                           + PR2 * (-0.02655 + 0.02883 * TR)
                           + W[I] * (-4.23893 + 8.65808 * TR - 1.2206 / TR - 3.15224 * TR3 - 0.025 * (PR - 0.6));
                }
                FLLog[I] = Term * 2.3025851;
                FVLog[I] = 0.0;
                EqK[I] = Math.Exp(FLLog[I] + GamLog[I] - FVLog[I]) / PR;
            }
        }

        private void KReal(Temperature TSep, Pressure PSep, double[] EqK, double[] X, double[] Y)
        {
            double[] A_EOS = new double[32];
            double[] B_EOS = new double[32];
            double[] FLLog = new double[32];
            double[] FVLog = new double[32];
            double[] GamLog = new double[32];
            int MaxTrials = 40;
            double AMix = 0.0;
            double BMix = 0.0;

            for (int I = 1; I <= 31; I++)
            {
                double TR = TSep.Rankine / TCritR[I];
                A_EOS[I] = Math.Sqrt(0.4278 / (PCritPSIA[I] * Math.Pow(TR, 2.5)));
                B_EOS[I] = 0.0867 / (PCritPSIA[I] * TR);
                AMix += A_EOS[I] * Y[I];
                BMix += B_EOS[I] * Y[I];
            }

            double AOB = Math.Pow(AMix, 2.0) / BMix;
            double BP = BMix * PSep.PSIA;
            double Z = 1.0;
            bool Solved = false;
            int num = MaxTrials;
            for (int J = 1; J <= num; J++)
            {
                double Func = Z / (Z - BP) - AOB * BP / (Z + BP) - Z;
                if (Math.Abs(Func) - 1E-06 <= 0.0)
                {
                    Solved = true;
                    break;
                }
                double Deriv = (0.0 - BP) / Math.Pow(Z - BP, 2.0) + AOB * BP / Math.Pow(Z + BP, 2.0) - 1.0;
                double ZNew = Z - Func / Deriv;
                Z = (!(ZNew - BP < 0.0) ? (!(ZNew > 2.0) ? ZNew : (Z + 2.0) * 0.5) : (Z + BP) * 0.5);
            }
            if (!Solved)
            {
                Z = 1.02;
                string Message = "Compressibility factor (Z) calculation failed in KReal routine. Used assigned value of 1.02.";
                CheckContinue(Message);
            }
            double VolMix = 0.0;
            double SolAvg = 0.0;
            double Term;

            for (int I = 1; I <= 31; I++)
            {
                Term = X[I] * MolVol[I];
                VolMix += Term;
                SolAvg += Term * Sol[I];
            }

            SolAvg /= VolMix;
            double RT = 1.1038889 * TSep;
            double Term2 = Z - 1.0;
            double Term3 = Math.Log(Z - BP);
            double Term4 = Math.Log(1.0 + BP / Z);

            for (int I = 1; I <= 31; I++)
            {
                GamLog[I] = MolVol[I] / RT * Math.Pow(Sol[I] - SolAvg, 2.0);
                double TR = TSep.Rankine / TCritR[I];
                double PR = PSep.PSIA / PCritPSIA[I];
                double TR2 = TR * TR;
                double TR3 = TR2 * TR;
                double PR2 = PR * PR;
                if (I == 1)
                {
                    Term = 1.96718 + 1.02972 / TR - 0.054009 * TR + 0.0005288 * TR2
                           + 0.008585 * PR;
                }
                else if (I == 2)
                {
                    Term = 2.4384 - 2.2455 / TR - 0.34084 * TR + 0.00212 * TR2
                           - 0.00223 * TR3 + PR * (0.10486 - 0.03691 * TR);
                }
                else
                {
                    Term = 5.75748 - 3.01761 / TR - 4.985 * TR + 2.02299 * TR2
                           + PR * (0.08427 + 0.26667 * TR - 0.31138 * TR2)
                           + PR2 * (-0.02655 + 0.02883 * TR)
                           + W[I] * (-4.23893 + 8.65808 * TR - 1.2206 / TR - 3.15224 * TR3
                           - 0.025 * (PR - 0.6));
                }
                FLLog[I] = Term * 2.3025851;
                double BOBMix = B_EOS[I] / BMix;
                double AOAMix = 2.0 * A_EOS[I] / AMix;
                FVLog[I] = Term2 * BOBMix - Term3 - AOB * (AOAMix - BOBMix) * Term4;
                EqK[I] = Math.Exp(FLLog[I] + GamLog[I] - FVLog[I]) / PR;
            }
        }
    }
}
