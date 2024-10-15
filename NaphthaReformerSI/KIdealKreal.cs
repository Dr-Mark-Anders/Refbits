using System;
using Units.UOM;

namespace NaphthaReformerSI
{
    public partial class NapReformerSI
    {
        private void KIdeal(Temperature TSep, Pressure PSep, double[] EqK)
        {
            double[] GamLog = new double[32];
            double[] FLLog = new double[32];
            double[] FVLog = new double[32];
            double Term;

            for (int I = 0; I < NumComp; I++)
            {
                GamLog[I] = 0.0;
                double TR = TSep.Rankine / TCritR[I];
                double PR = PSep.PSIA / PCritPSIA[I];
                double TR2 = TR * TR;
                double TR3 = TR2 * TR;
                double PR2 = PR * PR;
                if (I == 0)
                {
                    Term = 1.96718 + 1.02972 / TR - 0.054009 * TR + 0.0005288 * TR2
                        + 0.008585 * PR;
                }
                else if (I == 1)
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
            double AMix = 0.0;
            double BMix = 0.0;
            double VolMix = 0.0;
            double SolAvg = 0.0;
            double Term;

            for (int i = 0; i < NumComp; i++)
            {
                double TR = TSep.Rankine / TCritR[i];
                A_EOS[i] = Math.Sqrt(0.4278 / (PCritPSIA[i] * Math.Pow(TR, 2.5)));
                B_EOS[i] = 0.0867 / (PCritPSIA[i] * TR);
                AMix += A_EOS[i] * Y[i];
                BMix += B_EOS[i] * Y[i];
            }

            double AOB = Math.Pow(AMix, 2.0) / BMix;
            double BP = BMix * PSep.PSIA;
            double Z = 1.0;
            bool Solved = false;

            for (int i = 0; i < NumComp; i++)
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
                CheckContinue();
            }

            for (int i = 0; i < NumComp; i++)
            {
                Term = X[i] * MolVol[i];
                VolMix += Term;
                SolAvg += Term * Sol[i];
            }

            SolAvg /= VolMix;
            double RT = 1.1038889 * TSep;
            double Term2 = Z - 1.0;
            double Term3 = Math.Log(Z - BP);
            double Term4 = Math.Log(1.0 + BP / Z);

            for (int i = 0; i < NumComp; i++)
            {
                GamLog[i] = MolVol[i] / RT * Math.Pow(Sol[i] - SolAvg, 2.0);
                double TR = TSep.Rankine / TCritR[i];
                double PR = PSep.PSIA / PCritPSIA[i];
                double TR2 = TR * TR;
                double TR3 = TR2 * TR;
                double PR2 = PR * PR;
                if (i == 0)
                {
                    Term = 1.96718 + 1.02972 / TR - 0.054009 * TR + 0.0005288 * TR2
                           + 0.008585 * PR;
                }
                else if (i == 1)
                {
                    Term = 2.4384 - 2.2455 / TR - 0.34084 * TR + 0.00212 * TR2
                           - 0.00223 * TR3 + PR * (0.10486 - 0.03691 * TR);
                }
                else
                {
                    Term = 5.75748 - 3.01761 / TR - 4.985 * TR + 2.02299 * TR2
                           + PR * (0.08427 + 0.26667 * TR - 0.31138 * TR2)
                           + PR2 * (-0.02655 + 0.02883 * TR)
                           + W[i] * (-4.23893 + 8.65808 * TR - 1.2206 / TR - 3.15224 * TR3
                           - 0.025 * (PR - 0.6));
                }
                FLLog[i] = Term * 2.3025851;
                double BOBMix = B_EOS[i] / BMix;
                double AOAMix = 2.0 * A_EOS[i] / AMix;
                FVLog[i] = Term2 * BOBMix - Term3 - AOB * (AOAMix - BOBMix) * Term4;
                EqK[i] = Math.Exp(FLLog[i] + GamLog[i] - FVLog[i]) / PR;
            }
        }
    }
}