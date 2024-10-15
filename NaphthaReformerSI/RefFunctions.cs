using System;
using System.Windows.Forms;
using Units.UOM;

namespace NaphthaReformerSI
{
    public partial class NapReformerSI
    {
        public static double[] MassFraction(MassFlow[] MF)
        {
            double tot = 0;
            double[] res = new double[MF.Length];

            for (int i = 0; i < MF.Length; i++)
                tot += MF[i].BaseValue;

            for (int i = 0; i < MF.Length; i++)
                res[i] = MF[i].BaseValue / tot;

            return res;
        }

        public static double[] VolFraction(VolumeFlow[] VF)
        {
            double tot = 0;
            double[] res = new double[VF.Length];

            for (int i = 0; i < VF.Length; i++)
                tot += VF[i].BaseValue;

            for (int i = 0; i < VF.Length; i++)
                res[i] = VF[i].BaseValue / tot;

            return res;
        }

        public static double[] MoleFraction(double[] MoleF)
        {
            double tot = 0;
            double[] res = new double[MoleF.Length];

            for (int i = 0; i < MoleF.Length; i++)
                tot += MoleF[i];

            for (int i = 0; i < MoleF.Length; i++)
                res[i] = MoleF[i] / tot;

            return res;
        }

        private bool Flash(double[] F, Temperature TFlash, Pressure PFlash, ref double FracLiq, ref double[] F_Liq, ref double[] F_Vap)
        {
            double[] EqK = new double[32];
            double[] NewX = new double[32];
            double[] X = new double[32];
            double[] Y = new double[32];
            double[] FeedFrac = new double[32];
            int CountFracLiq0 = 0;
            int CountFracLiq1 = 0;
            FracLiq = 0.5;
            double TotFeed = 0.0;
            int MaxTrials = 100;
            bool Solved = false;

            for (int i = 0; i < NumComp; i++)
                TotFeed += F[i];

            for (int i = 0; i < NumComp; i++)
                FeedFrac[i] = F[i] / TotFeed;

            KIdeal(TFlash, PFlash, EqK);

            for (int J = 0; J < MaxTrials; J++)
            {
                double Deriv = 0.0;
                double Func = 0.0;
                bool RecalcK = false;

                for (int i = 0; i < NumComp; i++)
                {
                    double Term1 = 1.0 - EqK[i];
                    double Denom = FracLiq * Term1 + EqK[i];
                    NewX[i] = FeedFrac[i] / Denom;
                    if (Math.Abs(NewX[i] - X[i]) > 1E-05)
                        RecalcK = true;

                    double Eras = FeedFrac[i] * Term1 / Denom;
                    Func += Eras;
                    Deriv += Eras * Term1 / Denom;
                }

                if (Math.Abs(Func) - 1E-06 > 0.0)
                {
                    double FracLiqNew = FracLiq + Func / Deriv;
                    if (FracLiqNew <= 0.0)
                    {
                        FracLiq = 0.5 * FracLiq;
                        CountFracLiq0++;
                        if (CountFracLiq0 > 25)
                        {
                            Solved = true;
                            break;
                        }
                    }
                    else if (FracLiqNew > 1.0)
                    {
                        FracLiq = 0.5 * (FracLiq + 1.0);
                        CountFracLiq1++;
                        if (CountFracLiq1 > 25)
                        {
                            Solved = true;
                            break;
                        }
                    }
                    else
                        FracLiq = FracLiqNew;

                    continue;
                }
                double SumX = 0.0;
                double SumY = 0.0;

                for (int i = 0; i < NumComp; i++)
                {
                    double Term1 = 1.0 - EqK[i];
                    double Denom = FracLiq * Term1 + EqK[i];
                    X[i] = FeedFrac[i] / Denom;
                    Y[i] = EqK[i] * X[i];
                    SumX += X[i];
                    SumY += Y[i];
                }

                for (int i = 0; i < NumComp; i++)
                {
                    X[i] /= SumX;
                    Y[i] /= SumY;
                }

                if (RecalcK)
                {
                    KReal(TFlash, PFlash, EqK, X, Y);
                    continue;
                }
                Solved = true;
                break;
            }

            if (!Solved)
                EndWell("Flash calculation failed. Simulation abandoned.");

            double TotLiq = TotFeed * FracLiq;
            double TotVap = TotFeed - TotLiq;

            for (int i = 0; i < 31; i++)
            {
                F_Liq[i] = TotLiq * X[i];
                F_Vap[i] = TotVap * Y[i];
            }

            return Solved;
        }

        private bool AssignRecycle(double[] F_Vap, double[] F_NetGas, MoleFlow[] F_Recy)
        {
            double FracRecy;
            if (F_Vap[0] < MolH2Recy)
            {
                EndWell("Insufficient hydrogen produced for specified recycle. Simulation abandoned.");
                return false;
            }
            else
                FracRecy = MolH2Recy / F_Vap[0];

            for (int i = 0; i < NumComp; i++)
            {
                F_Recy[i] = F_Vap[i] * FracRecy;
                F_NetGas[i] = F_Vap[i] - F_Recy[i];
            }
            return true;
        }

        private void OrganizeResults(double[] F_Ref)
        {
            ReformateMassFlow = new MassFlow();
            RxEFF_MassFlow = new MassFlow();
            SepVap_MassFlow = new MassFlow();
            RecycleGas_MassFlow = new MassFlow();

            ReformateVolFlow = new VolumeFlow();
            RxEFF_VolFlow = new VolumeFlow();
            SepVap_VolFlow = new VolumeFlow();
            RecycleGas_VOlFlow = new VolumeFlow();

            for (int i = 0; i < NumComp; i++)
            {
                RefMassFlows[i].kg_hr = F_Ref[i] * MW[i];
                RefVolFlow[i].m3_hr = RefMassFlows[i] / SG[i];

                SumMol += F_Ref[i];

                ReformateMassFlow += RefMassFlows[i];
                ReformateVolFlow += RefVolFlow[i];
                SepVap_MassFlow += GasMassFlow[i];
            }

            RefMassFraction = MassFraction(RefMassFlows);
            RefVolFraction = VolFraction(RefVolFlow);
            RefMoleFraction = MoleFraction(F_Ref);
        }

        private void EstRecycle(MoleFlow[] F_Recy)
        {
            double[] CompToH2 = new double[NumComp]
            {
                1.0, 0.8, 0.065, 0.058, 0.013, 0.02, 0.012, 0.007, 0.007,
                0.003, 0.0, 0.0, 0.003, 0.005, 0.002, 0.0, 0.0, 0.005, 0.002,
                0.0, 0.0, 0.0, 0.0, 0.001, 0.0, 0.001, 0.0, 0.001, 0.0,
                0.0, 0.001
            };

            for (int i = 0; i < NumComp; i++)
                F_Recy[i] = MolH2Recy * CompToH2[i];
        }

        private void ShortCharFeed(double P_LV, double N_LV, double A_LV, Temperature[] D86, double[] PctVol)
        {
            double[] Cut = new double[9];
            double[] Sum = new double[22];
            double[] Factors = new double[11];
            double[] Term = new double[12];
            double[] SG1 = new double[42];
            double[] MW1 = new double[42];

            for (int i = 0; i < 42; i++)
            {
                SG1[i] = SG[i];
                MW1[i] = MW[i];
            }

            if (double.IsNaN(D86[1]))
            {
                if (double.IsNaN(D86[2]))
                    D86[1] = D86[1] + 0.4474 * (D86[4] - D86[1]);
                else
                    D86[1] = D86[1] + 0.6081 * (D86[3] - D86[1]);
            }

            if (double.IsNaN(D86[2]))
            {
                if (double.IsNaN(D86[1]))
                    D86[2] = D86[0] + 0.7335 * (D86[3] - D86[0]);
                else
                    D86[2] = D86[1] + 0.5236 * (D86[3] - D86[1]);
            }

            if (double.IsNaN(D86[4]))
            {
                if (double.IsNaN(D86[6]))
                    D86[4] = D86[3] + 0.2003 * (D86[6] - D86[3]);
                else
                    D86[4] = D86[3] + 0.3802 * (D86[5] - D86[3]);
            }

            if (double.IsNaN(D86[5]))
            {
                if (double.IsNaN(D86[4]))
                    D86[5] = D86[3] + 0.5274 * (D86[6] - D86[3]);
                else
                    D86[5] = D86[4] + 0.4095 * (D86[6] - D86[4]);
            }

            InterDist(D86, Cut);

            double Total = 0.0;

            for (int i = 6; i <= 8; i++)
                Total += Cut[i];

            double Factor = 100.0 / Total;

            for (int i = 6; i <= 8; i++)
                Cut[i] = Cut[i] * Factor;

            Factors[0] = P_LV * 0.01;
            Factors[1] = N_LV * 0.01;
            Factors[2] = A_LV * 0.01;
            Sum[0] = N_LV + A_LV;
            Sum[1] = P_LV + N_LV;
            Factors[3] = N_LV / Sum[0];
            Factors[4] = A_LV / Sum[0];
            Factors[5] = P_LV / Sum[1];
            Factors[6] = N_LV / Sum[1];
            double C5P = Cut[0];
            double C6P = Cut[1];
            double C6N = Cut[2] * Factors[3];
            double C6A = Cut[2] * Factors[4];
            double C7P = Cut[3] * Factors[5];
            double C7N = Cut[3] * Factors[6];
            double C7A = Cut[4] * Factors[2];
            double C8P = Cut[4] * Factors[0];
            double C8N = Cut[4] * Factors[1];
            double C8A = Cut[5] * Factors[2];
            double C9P = Cut[5] * Factors[0];
            double C9N = Cut[5] * Factors[1];
            double C9A = Cut[6] * Factors[2];
            double C10P = Cut[6] * Factors[0];
            double C10N = Cut[6] * Factors[1];
            double C10A = Cut[7] * Factors[2];
            double C11P = Cut[7] * Factors[0];
            double C11N = Cut[7] * Factors[1];
            double C11A = Cut[8] * Factors[2];
            double C12P = Cut[8] * Factors[0];
            double C12N = Cut[8] * Factors[1];
            double P_LV2 = C5P + C6P + C7P + C8P + C9P + C10P + C11P + C12P;
            double N_LV2 = C6N + C7N + C8N + C9N + C10N + C11N + C12N;
            double A_LV2 = C6A + C7A + C8A + C9A + C10A + C11A;
            Sum[2] = P_LV2 + N_LV2 + A_LV2;
            Factor = 100.0 / Sum[2];
            P_LV2 *= Factor;
            N_LV2 *= Factor;
            A_LV2 *= Factor;
            double FactorP = P_LV / P_LV2;
            double FactorN = N_LV / N_LV2;
            double FactorA = A_LV / A_LV2;
            double C5P2 = C5P * FactorP;
            double C6P2 = C6P * FactorP;
            double C7P2 = C7P * FactorP;
            double C8P2 = C8P * FactorP;
            double C9P2 = C9P * FactorP;
            double C10P2 = C10P * FactorP;
            double C11P2 = C11P * FactorP;
            double C12P2 = C12P * FactorP;
            double C6N2 = C6N * FactorN;
            double C7N2 = C7N * FactorN;
            double C8N2 = C8N * FactorN;
            double C9N2 = C9N * FactorN;
            double C10N2 = C10N * FactorN;
            double C11N2 = C11N * FactorN;
            double C12N2 = C12N * FactorN;
            double C6A2 = C6A * FactorA;
            double C7A2 = C7A * FactorA;
            double C8A2 = C8A * FactorA;
            double C9A2 = C9A * FactorA;
            double C10A2 = C10A * FactorA;
            double C11A2 = C11A * FactorA;
            double P_LV3 = C5P2 + C6P2 + C7P2 + C8P2 + C9P2 + C10P2 + C11P2 + C12P2;
            double N_LV3 = C6N2 + C7N2 + C8N2 + C9N2 + C10N2 + C11N2 + C12N2;
            double A_LV3 = C6A2 + C7A2 + C8A2 + C9A2 + C10A2 + C11A2;
            Sum[3] = P_LV3 + N_LV3 + A_LV3;
            double ZIC6 = 0.4 * C6P2;
            double ZNC6 = 0.6 * C6P2;
            double ZCH = 0.45 * C6N2;
            double ZMCP = 0.55 * C6N2;
            double ZIC7 = 0.47 * C7P2;
            double ZNC7 = 0.53 * C7P2;
            double ZMCH = 0.54 * C7N2;
            double ZC7CP = 0.46 * C7N2;
            double ZECH = 0.12 * C8N2;
            double ZDMCH = 0.52 * C8N2;
            double ZPCP = 0.02 * C8N2;
            double ZC8CP = 0.34 * C8N2;
            double ZEB = 0.19 * C8A2;
            double ZPX = 0.11 * C8A2;
            double ZMX = 0.46 * C8A2;
            double ZOX = 0.24 * C8A2;
            Sum[4] = 0.0;
            Sum[4] = C5P2 * SG1[7] + ZIC6 * SG1[8] + ZNC6 * SG1[9] + ZCH * SG1[10] + ZMCP * SG1[11] + C6A2 * SG1[12] + ZIC7 * SG1[13] + ZNC7 * SG1[14]
                        + ZMCH * SG1[15] + ZC7CP * SG1[16] + C7A2 * SG1[17] + C8P2 * SG1[18] + ZECH * SG1[19] + ZDMCH * SG1[20] + ZPCP * SG1[21] + ZC8CP * SG1[22]
                        + ZEB * SG1[23] + ZPX * SG1[24] + ZMX * SG1[25] + ZOX * SG1[26] + C9P2 * SG1[31] + C9N2 * SG1[32] + C9A2 * SG1[33] + C10P2 * SG1[34]
                        + C10N2 * SG1[35] + C10A2 * SG1[36] + C11P2 * SG1[37] + C11N2 * SG1[38] + C11A2 * SG1[39] + C12P2 * SG1[40]
                        + C12N2 * SG1[41];
            Sum[4] *= 0.01;
            Term[0] = 0.0;
            Term[0] = ZIC6 + ZIC7 + ZC7CP + C8P2 + ZDMCH + ZC8CP + C9P2 + C9N2 + C10P2 + C10N2 + C11P2 + C11N2 + C12P2 + C12N2;
            Term[1] = 100.0 / Term[0];
            Sum[5] = ZIC6 * Term[1] * SG1[8] + ZIC7 * Term[1] * SG1[13] + ZC7CP * Term[1] * SG1[16] + C8P2 * Term[1] * SG1[18]
                + ZDMCH * Term[1] * SG1[20] + ZC8CP * Term[1] * SG1[22] + C9P2 * Term[1] * SG1[31] + C9N2 * Term[1] * SG1[32]
                + C10P2 * Term[1] * SG1[34] + C10N2 * Term[1] * SG1[35] + C11P2 * Term[1] * SG1[37] + C11N2 * Term[1] * SG1[38]
                + C12P2 * Term[1] * SG1[40] + C12N2 * Term[1] * SG1[41];
            Sum[5] *= 0.01;
            Factors[7] = Term[0] * 0.01 * Sum[5] + SGFeed - Sum[4];
            Factors[8] = Factors[7] / (Term[0] * 0.01);
            Factors[9] = Factors[8] / Sum[5];
            SG1[8] = Factors[9] * SG1[8];
            SG1[13] = Factors[9] * SG1[13];
            SG1[16] = Factors[9] * SG1[16];
            SG1[18] = Factors[9] * SG1[18];
            SG1[20] = Factors[9] * SG1[20];
            SG1[22] = Factors[9] * SG1[22];
            SG1[31] = Factors[9] * SG1[31];
            SG1[32] = Factors[9] * SG1[32];
            SG1[34] = Factors[9] * SG1[34];
            SG1[35] = Factors[9] * SG1[35];
            SG1[37] = Factors[9] * SG1[37];
            SG1[38] = Factors[9] * SG1[38];
            SG1[40] = Factors[9] * SG1[40];
            SG1[41] = Factors[9] * SG1[41];

            Sum[6] = 0.0;
            Sum[6] = C9P2 + C9N2 + C9A2 + C10P2 + C10N2 + C10A2 + C11P2 + C11N2 + C11A2 + C12P2 + C12N2;
            double SumP = C9P2 + C10P2 + C11P2 + C12P2;
            double SumN = C9N2 + C10N2 + C11N2 + C12N2;
            double SumA = C9A2 + C10A2 + C11A2;
            Term[2] = 100.0 / Sum[6];
            double C9P3 = Term[2] * C9P2;
            double C9N3 = Term[2] * C9N2;
            double C9A3 = Term[2] * C9A2;
            double C10P3 = Term[2] * C10P2;
            double C10N3 = Term[2] * C10N2;
            double C10A3 = Term[2] * C10A2;
            double C11P3 = Term[2] * C11P2;
            double C11N3 = Term[2] * C11N2;
            double C11A3 = Term[2] * C11A2;
            double C12P3 = Term[2] * C12P2;
            double C12N3 = Term[2] * C12N2;
            Sum[7] = 0.0;
            Sum[7] = C9P3 * SG1[31] + C9N3 * SG1[32] + C9A3 * SG1[33] + C10P3 * SG1[34] + C10N3 * SG1[35] + C10A3 * SG1[36] + C11P3 * SG1[37]
                + C11N3 * SG1[38] + C11A3 * SG1[39] + C12P3 * SG1[40] + C12N3 * SG1[41];
            Sum[7] *= 0.01;

            double C9P4 = C9P3 * SG1[31] / Sum[7];
            double C9N4 = C9N3 * SG1[32] / Sum[7];
            double C9A4 = C9A3 * SG1[33] / Sum[7];
            double C10P4 = C10P3 * SG1[34] / Sum[7];
            double C10N4 = C10N3 * SG1[35] / Sum[7];
            double C10A4 = C10A3 * SG1[36] / Sum[7];
            double C11P4 = C11P3 * SG1[37] / Sum[7];
            double C11N4 = C11N3 * SG1[38] / Sum[7];
            double C11A4 = C11A3 * SG1[39] / Sum[7];
            double C12P4 = C12P3 * SG1[40] / Sum[7];
            double C12N4 = C12N3 * SG1[41] / Sum[7];
            double C9P5 = C9P4 / MW1[31];
            double C9N5 = C9N4 / MW1[32];
            double C9A5 = C9A4 / MW1[33];
            double C10P5 = C10P4 / MW1[34];
            double C10N5 = C10N4 / MW1[35];
            double C10A5 = C10A4 / MW1[36];
            double C11P5 = C11P4 / MW1[37];
            double C11N5 = C11N4 / MW1[38];
            double C11A5 = C11A4 / MW1[39];
            double C12P5 = C12P4 / MW1[40];
            double C12N5 = C12N4 / MW1[41];
            Sum[8] = 0.0;
            Sum[8] = C9P5 + C9N5 + C9A5 + C10P5 + C10N5 + C10A5 + C11P5 + C11N5 + C11A5 + C12P5 + C12N5;
            Term[3] = 100.0 / Sum[8];
            Term[4] = Sum[8] * 0.01;
            double C9P6 = C9P5 / Term[4];
            double C9N6 = C9N5 / Term[4];
            double C9A6 = C9A5 / Term[4];
            double C10P6 = C10P5 / Term[4];
            double C10N6 = C10N5 / Term[4];
            double C10A6 = C10A5 / Term[4];
            double C11P6 = C11P5 / Term[4];
            double C11N6 = C11N5 / Term[4];
            double C11A6 = C11A5 / Term[4];
            double C12P6 = C12P5 / Term[4];
            double C12N6 = C12N5 / Term[4];
            Sum[9] = 0.0;
            Sum[9] = C9P3 + C10P3 + C11P3 + C12P3;
            if (Sum[9] != 0.0)
            {
                Term[5] = 100.0 / Sum[9];
                double C9P7 = Term[5] * C9P3;
                double C10P7 = Term[5] * C10P3;
                double C11P7 = Term[5] * C11P3;
                double C12P7 = Term[5] * C12P3;
                Sum[10] = 0.0;
                Sum[10] = C9P7 * SG1[31] + C10P7 * SG1[34] + C11P7 * SG1[37] + C12P7 * SG1[40];
                Sum[10] *= 0.01;
            }
            else
                Sum[10] = SG1[31];

            Sum[11] = 0.0;
            Sum[11] = C9P6 + C10P6 + C11P6 + C12P6;
            if (Sum[11] != 0.0)
            {
                Term[6] = 100.0 / Sum[11];
                double C9P8 = Term[6] * C9P6;
                double C10P8 = Term[6] * C10P6;
                double C11P8 = Term[6] * C11P6;
                double C12P8 = Term[6] * C12P6;
                Sum[12] = 0.0;
                Sum[12] = C9P8 * MW1[31] + C10P8 * MW1[34] + C11P8 * MW1[37] + C12P8 * MW1[40];
                Sum[12] *= 0.01;
            }
            else
                Sum[12] = MW1[31];

            Sum[13] = 0.0;
            Sum[13] = C9N3 + C10N3 + C11N3 + C12N3;

            if (Sum[13] != 0.0)
            {
                Term[7] = 100.0 / Sum[13];
                double C9N7 = Term[7] * C9N3;
                double C10N7 = Term[7] * C10N3;
                double C11N7 = Term[7] * C11N3;
                double C12N7 = Term[7] * C12N3;
                Sum[14] = 0.0;
                Sum[14] = C9N7 * SG1[32] + C10N7 * SG1[35] + C11N7 * SG1[38] + C12N7 * SG1[41];
                Sum[14] *= 0.01;
            }
            else
                Sum[14] = SG1[32];

            Sum[15] = 0.0;
            Sum[15] = C9N6 + C10N6 + C11N6 + C12N6;
            if (Sum[15] != 0.0)
            {
                Term[8] = 100.0 / Sum[15];
                double C9N8 = Term[8] * C9N6;
                double C10N8 = Term[8] * C10N6;
                double C11N8 = Term[8] * C11N6;
                double C12N8 = Term[8] * C12N6;
                Sum[16] = 0.0;
                Sum[16] = C9N8 * MW1[32] + C10N8 * MW1[35] + C11N8 * MW1[38] + C12N8 * MW1[41];
                Sum[16] *= 0.01;
            }
            else
                Sum[16] = MW1[32];

            Sum[17] = 0.0;
            Sum[17] = C9A3 + C10A3 + C11A3;
            if (Sum[17] != 0.0)
            {
                Term[9] = 100.0 / Sum[17];
                double C9A7 = Term[9] * C9A3;
                double C10A7 = Term[9] * C10A3;
                double C11A7 = Term[9] * C11A3;
                Sum[18] = 0.0;
                Sum[18] = C9A7 * SG1[33] + C10A7 * SG1[36] + C11A7 * SG1[39];
                Sum[18] *= 0.01;
            }
            else
                Sum[18] = SG1[33];

            Sum[19] = 0.0;
            Sum[19] = C9A6 + C10A6 + C11A6;
            if (Sum[19] != 0.0)
            {
                Term[10] = 100.0 / Sum[19];
                double C9A8 = Term[10] * C9A6;
                double C10A8 = Term[10] * C10A6;
                double C11A8 = Term[10] * C11A6;
                Sum[20] = 0.0;
                Sum[20] = C9A8 * MW1[33] + C10A8 * MW1[36] + C11A8 * MW1[39];
                Sum[20] *= 0.01;
            }
            else
                Sum[20] = MW1[33];

            //double  CA = (Term[3] + Sum[19] * 0.01 * 6.028 - Sum[11] * 0.01 * 2.016) / 14.027;
            double ZC9CH = Sum[14] + 0.0035;
            double ZC9CP = Sum[14] - 0.0035;
            SG1[27] = Sum[10];
            SG1[28] = ZC9CH;
            SG1[29] = ZC9CP;
            SG1[30] = Sum[18];
            MW1[27] = Sum[12];
            MW1[28] = Sum[16];
            MW1[29] = Sum[16];
            MW1[30] = Sum[20];

            for (int i = 0; i <= 9; i++)
                PctVol[i] = 0.0;

            PctVol[7] = C5P2;
            PctVol[8] = ZIC6;
            PctVol[9] = ZNC6;
            PctVol[10] = ZCH;
            PctVol[11] = ZMCP;
            PctVol[12] = C6A2;
            PctVol[13] = ZIC7;
            PctVol[14] = ZNC7;
            PctVol[15] = ZMCH;
            PctVol[16] = ZC7CP;
            PctVol[17] = C7A2;
            PctVol[18] = C8P2;
            PctVol[19] = ZECH;
            PctVol[20] = ZDMCH;
            PctVol[21] = ZPCP;
            PctVol[22] = ZC8CP;
            PctVol[23] = ZEB;
            PctVol[24] = ZPX;
            PctVol[25] = ZMX;
            PctVol[26] = ZOX;
            PctVol[27] = SumP;
            PctVol[28] = SumN * 0.5;
            PctVol[29] = SumN * 0.5;
            PctVol[30] = SumA;
        }

        private static void InterDist(Temperature[] D86Dist, double[] Cut)
        {
            double[] XDB = new double[7];
            double[] XDA = new double[7];
            double[] XDC = new double[10];
            double[] CutB = new double[10];
            double[] Regs = new double[3];
            XDA[0] = 0.0;
            XDA[1] = 10.0;
            XDA[2] = 30.0;
            XDA[3] = 50.0;
            XDA[4] = 70.0;
            XDA[5] = 90.0;
            XDA[6] = 100.0;
            XDC[0] = 0.0;
            XDC[1] = 120.0;
            XDC[2] = 160.0;
            XDC[3] = 180.0;
            XDC[4] = 220.0;
            XDC[5] = 270.0;
            XDC[6] = 310.0;
            XDC[7] = 350.0;
            XDC[8] = 390.0;
            XDC[9] = 600.0;

            ConvD86ToTBP(D86Dist, XDB);
            XDC[0] = XDB[0];
            XDC[9] = XDB[6];

            for (int i = 1; i <= 9; i++)
            {
                if (XDC[i] < XDC[0])
                    XDC[i] = XDC[0];
            }

            for (int i = 0; i <= 8; i++)
            {
                if (XDC[i] > XDC[9])
                    XDC[i] = XDC[9];
            }

            for (int i = 0; i <= 9; i++)
            {
                int IFlag = -1;
                double YVal = 0.0;
                QuadInterp(XDB, XDA, 6.0, XDC[i], ref YVal, Regs, IFlag);
                CutB[i] = YVal;
            }

            for (int i = 0; i <= 9; i++)
            {
                if (CutB[i] < 0.0)
                    CutB[i] = 0.0;
                else if (CutB[i] > 100.0)
                    CutB[i] = 100.0;
            }

            for (int i = 9; i <= 8; i++)
            {
                if (CutB[i + 1] < CutB[i])
                    EndWell("Error in short assay characterization. Please check distillation input data, then try again.");
            }

            Cut[0] = CutB[1] - CutB[0];
            Cut[1] = CutB[2] - CutB[1];
            Cut[2] = CutB[3] - CutB[2];
            Cut[3] = CutB[4] - CutB[3];
            Cut[4] = CutB[5] - CutB[4];
            Cut[5] = CutB[6] - CutB[5];
            Cut[6] = CutB[7] - CutB[6];
            Cut[7] = CutB[8] - CutB[7];
            Cut[8] = CutB[9] - CutB[8];
        }

        private static void ConvD86ToTBP(Temperature[] DistA, double[] DistB)
        {
            DistB[1] = 0.5277 * Math.Pow(DistA[1].Fahrenheit + 459.67, 1.090011) - 459.67;
            DistB[2] = 0.7429 * Math.Pow(DistA[2].Fahrenheit + 459.67, 1.042533) - 459.67;
            DistB[3] = 0.89203 * Math.Pow(DistA[3].Fahrenheit + 459.67, 1.017565) - 459.67;
            DistB[4] = 0.87051 * Math.Pow(DistA[4].Fahrenheit + 459.67, 1.02259) - 459.67;
            DistB[5] = 0.948976 * Math.Pow(DistA[5].Fahrenheit + 459.67, 1.010955) - 459.67;
            double D86_10 = DistA[1].Fahrenheit - DistA[0].Fahrenheit;
            double TBP_10 = 0.0 + 2.84582 * D86_10 - 0.0571924 * Math.Pow(D86_10, 2.0) + 0.00104465 * Math.Pow(D86_10, 3.0) - 1.01842E-05 * Math.Pow(D86_10, 4.0) + 4.9828E-08 * Math.Pow(D86_10, 5.0) - 9.53795E-11 * Math.Pow(D86_10, 6.0);
            DistB[0] = DistB[1] - TBP_10;
            double D86_11 = DistA[6].Fahrenheit - DistA[5].Fahrenheit;
            double TBP_11 = 0.0 + 1.20092 * D86_11 + 0.00661377 * Math.Pow(D86_11, 2.0) - 0.000650227 * Math.Pow(D86_11, 3.0) + 1.63615E-05 * Math.Pow(D86_11, 4.0) - 1.71884E-07 * Math.Pow(D86_11, 5.0) + 6.8672E-10 * Math.Pow(D86_11, 6.0);
            DistB[6] = DistB[5] + TBP_11;
        }

        private static void QuadInterp(double[] XArray, double[] YArray, double NVal, double X, ref double Y, double[] Regs, int IFlag)
        {
            double[] VVal = new double[4];
            double[] GVal = new double[4];
            int IErr = 0;
            if (Y > 0.0)
            {
                IErr = 1;
            }
            int II = 0;
            int MFlag = default;
            int J;
            checked
            {
                if (NVal > 2.0)
                {
                    int num = (int)Math.Round(NVal);
                    J = 1;
                    while (true)
                    {
                        if (J <= num)
                        {
                            II = J;
                            if (X <= XArray[J])
                            {
                                break;
                            }
                            J++;
                            continue;
                        }
                        II = (int)Math.Round(NVal - 1.0);
                        break;
                    }
                    if (YArray[II] != YArray[II - 1])
                    {
                        if (II >= NVal)
                        {
                            II = (int)Math.Round(NVal - 1.0);
                        }
                        II--;
                        if ((II == IFlag) & (X < XArray[II + 1]))
                        {
                            goto IL_01a0;
                        }
                        J = 0;
                        do
                        {
                            int TempInt = II + J;
                            VVal[J] = XArray[TempInt];
                            GVal[J] = YArray[TempInt];
                            J++;
                        }
                        while (J <= 2);
                        MFlag = 2;
                        goto IL_018d;
                    }
                    II--;
                }
                Regs[1] = 0.0;
                if (XArray[II + 1] == XArray[II])
                {
                    Regs[2] = YArray[II] / XArray[II];
                    Regs[3] = 0.0;
                }
                else
                {
                    Regs[2] = (YArray[II + 1] - YArray[II]) / (XArray[II + 1] - XArray[II]);
                    Regs[3] = YArray[II] - Regs[2] * XArray[II];
                }
                MFlag = 2;
                goto IL_0316;
            }
        IL_018d:
            double RVal = default;
            Quad(GVal, VVal, MFlag, Regs, ref RVal);
            //IFlag = II;
            goto IL_01a0;
        IL_01a0:
            if (!((IErr == 0) | (Regs[0] == 0.0)))
            {
                double XN = (0.0 - Regs[2]) / (Regs[1] * 2.0);
                if (MFlag != 2)
                {
                    if (XN < 0.0)
                        goto IL_0316;

                    XN = 2.0 * XN / 3.0;
                    XN = Math.Sqrt(XN);
                    J = 1;
                    while ((XN < VVal[1] && XN < X) | (XN > VVal[3] && XN > X))
                    {
                        XN = 0.0 - XN;
                        J = checked(J + 1);
                        if (J > 2)
                            break;
                    }
                }
                if (!(((XN <= VVal[1]) & (X >= VVal[1])) | ((XN <= VVal[2]) & (X >= VVal[2]))) && !(((XN >= VVal[2]) & (X <= VVal[2])) | ((XN >= VVal[3]) & (X <= VVal[3]))))
                {
                    if (MFlag != 3)
                    {
                        MFlag = 3;
                        goto IL_018d;
                    }
                    II = 1;

                    if (X > VVal[2])
                        II = 3;

                    Regs[1] = 0.0;
                    Regs[2] = (GVal[2] - GVal[II]) / (VVal[2] - VVal[II]);
                    Regs[3] = GVal[2] - Regs[2] * VVal[2];
                }
            }
            goto IL_0316;
        IL_0316:
            J = checked(MFlag - 1);
            Y = (Regs[0] * Math.Pow(X, J) + Regs[1]) * X + Regs[2];
        }

        private static void Quad(double[] GVal, double[] YVal, int MFlag, double[] Regs, ref double RVal)
        {
            int KK = 1;
            double TT1 = YVal[2] - YVal[1];
            double TT2 = YVal[1] - YVal[0];
            if (Math.Abs(TT2) <= Math.Abs(TT1) * 100.0)
            {
                KK = 2;
                if (Math.Abs(TT1) <= Math.Abs(TT2) * 100.0)
                {
                    //if (GVal[1] < 0.0)
                    //  KK = 1;

                    KK = 1;
                    double XX2 = YVal[2] + YVal[0];
                    if (MFlag == 3)
                    {
                        TT1 *= YVal[0] + YVal[1] + YVal[2];
                        XX2 = XX2 * YVal[2] + Math.Pow(YVal[0], 2.0);
                        KK = 1;
                    }
                    if (TT1 <= 0.001)
                        TT1 = 0.001;

                    double XX1 = 1.0 / TT1;
                    double Temp1 = YVal[2] - YVal[0];
                    if (Temp1 <= 0.001)
                        Temp1 = 0.001;

                    double YY1 = (GVal[2] - GVal[0]) / Temp1;
                    if (TT2 <= 0.001)
                        TT2 = 0.001;

                    double YY2 = (GVal[1] - GVal[0]) / TT2;
                    Regs[0] = XX1 * (YY1 - YY2);
                    Regs[1] = YY1 - Regs[0] * XX2;
                    Regs[2] = GVal[1] - (Regs[0] * Math.Pow(YVal[1], KK) + Regs[1]) * YVal[1];
                    if (MFlag >= 2)
                        return;

                    if ((GVal[0] >= 0.0) & (GVal[2] <= 0.0))
                    {
                        double TT3 = Math.Abs((GVal[2] - GVal[1]) / TT1);
                        double TT4 = Math.Abs(YY2);
                        if (TT3 >= 100.0 * TT4 || TT3 <= 0.01 * TT4)
                        {
                            KK = 1;
                            if (GVal[1] > 0.0)
                                KK = 2;

                            RVal = (YVal[KK] + YVal[checked(KK + 1)]) / 2.0;
                            return;
                        }
                    }
                    if (!(Math.Abs(Regs[1]) < Math.Abs(Regs[2]) * 1E-06))
                    {
                        TT1 = Math.Pow(Regs[1], 2.0) - 4.0 * Regs[0] * Regs[2];
                        if (!(TT1 < 0.0))
                        {
                            XX1 = Math.Sqrt(TT1);
                            XX2 = -1.0 / (2.0 * Regs[0]);
                            TT1 = XX2 * (Regs[2] + XX1);
                            TT2 = XX2 * (Regs[2] - XX1);
                            RVal = TT1;
                            double TT3;
                            if ((GVal[0] >= 0.0) & (GVal[2] <= 0.0))
                            {
                                KK = 1;
                                if (GVal[1] > 0.0)
                                    KK = 2;

                                TT3 = (YVal[checked(KK + 1)] + YVal[KK]) / 2.0;
                            }
                            else
                            {
                                KK = 1;
                                if (GVal[3] > 0.0)
                                    KK = 3;

                                TT3 = YVal[KK];
                            }
                            if (Math.Abs(TT3 - TT1) > Math.Abs(TT3 - TT2))
                                RVal = TT2;

                            return;
                        }
                    }
                }
            }
            Regs[0] = 0.0;
            Regs[1] = (GVal[1] - GVal[KK]) / (YVal[1] - YVal[KK]);
            Regs[2] = GVal[1] - YVal[1] * Regs[1];
            RVal = (0.0 - Regs[2]) / Regs[1];
        }

        private double CrackProd(int I, double[] RxnRate)
        {
            return RxnRate[44] * HCDistrib[0, I] + RxnRate[70] * HCDistrib[1, I] + RxnRate[71] * HCDistrib[2, I]
                + RxnRate[45] * HCDistrib[3, I] + RxnRate[72] * HCDistrib[4, I] + RxnRate[73] * HCDistrib[5, I]
                + RxnRate[74] * HCDistrib[6, I] + RxnRate[75] * HCDistrib[7, I] + RxnRate[46] * HCDistrib[8, I]
                + RxnRate[47] * HCDistrib[9, I] + RxnRate[76] * HCDistrib[10, I] + RxnRate[77] * HCDistrib[11, I]
                + RxnRate[48] * HCDistrib[12, I] + RxnRate[49] * HCDistrib[13, I] + RxnRate[78] * HCDistrib[14, I]
                + RxnRate[79] * HCDistrib[15, I] + RxnRate[50] * HCDistrib[16, I] + RxnRate[51] * HCDistrib[17, I]
                + RxnRate[52] * HCDistrib[18, I] + RxnRate[53] * HCDistrib[19, I];
        }

        private void Furnace(Temperature T_In, Temperature T_Out, double[] F_Moles, ref EnergyFlow Duty)
        {
            Duty = 0.0;
            double T1 = T_In.Rankine * 0.01;
            double T2 = T_Out.Rankine * 0.01;
            double TD1 = T2 - T1;
            double TD2 = (Math.Pow(T2, 2.0) - Math.Pow(T1, 2.0)) / 2.0;
            double TD3 = (Math.Pow(T2, 3.0) - Math.Pow(T1, 3.0)) / 3.0;
            double TD4 = (Math.Pow(T2, 4.0) - Math.Pow(T1, 4.0)) / 4.0;

            for (int i = 0; i < NumComp; i++)
            {
                double DeltaH = CpCoeft[i][0] * TD1 + CpCoeft[i][1] * TD2 + CpCoeft[i][2] * TD3 + CpCoeft[i][3] * TD4;
                Duty += DeltaH * F_Moles[i] * 100.0;
            }

            Duty /= 1000000.0;
        }

        private static void EndWell(string Message)
        {
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK);
        }

        private void InitConstants()
        {
            Names = new string[] {"H2","C1","C2","C3","IC4","NC4","IC5","NC5","IC6","NC6","CH","MCP"
                ,"BEN","IC7","NC7","MCH","C7CP","TOL","C8P","ECH","DMCH","PCP","C8CP","EB","PX","MX","OX","C9+P","C9+CH","C9+CP","C9+A"};

            SG = new double[] {0.135,0.3,0.3564,0.5077,0.5631,0.5844,0.6247,0.631,0.6609,0.664,0.7834,0.7536,0.8844,0.6906,
            0.6882,0.774,0.7595,0.8718,0.7089,0.7922,0.779,0.7807,0.7713,0.8718,0.8657,0.8687,0.8848,0.7272,
            0.79,0.7825,0.8769,0.7272,0.7863,0.8769,0.7408,0.7946,0.8837,0.7511,0.8014,0.8919,0.76,0.8074};

            NBP = new double[] {36.77,200.97,332.17,415.97,470.57,490.77,541.77,556.57,597.47,615.37,636.97,620.97,635.87,652.17,668.87,673.37,661.27,
            690.77,702.07,728.87,711.57,727.37,703.37,736.77,740.67,742.07,751.57,749.67,759.67,744.67,790.67 };

            TCritR = new double[] {59.742,343.08,549.54,665.64,734.58,765.18,829.8,845.46,900.9,913.68,996.48,959.04,1011.96, 956.0,972.36,1029.96, 990.3,1065.24,
                1024.9,1054.1,1054.1,1044.9,1044.9,1115.8,1113.1,1114.9,1138.3,1055.8,1118.8,1108.8,1146.7};

            PCritPSIA = new double[] {190.8,673.1,709.8,617.4,529.1,550.7,483.0,489.5,449.5, 440.0, 561.0, 549.0, 714.0, 414.2, 396.8, 504.4,
            497.8, 590.0, 362.1, 468.1, 468.1, 448.4, 448.4, 540.0, 500.0, 510.0, 530.0, 347.0, 418.1, 412.1, 492.2};

            W = new double[] {0.0,0.0,0.1064,0.1538,0.1825,0.1953,0.2104,0.2387,0.2861,0.2972,0.2032,0.2346,0.213,0.3427,0.3403,0.2421,
            0.3074,0.2591,0.3992,0.3558,0.3558,0.364,0.364,0.2936,0.2969,0.3045,0.2904,0.4406,0.4124,0.4164,0.3799};

            MolVol = new double[]{31.0, 52.0, 68.0, 84.0, 105.5, 101.4, 117.4, 116.1, 131.449, 131.6, 108.7, 113.1, 89.4, 147.097, 147.5, 128.3, 129.71, 106.8, 163.5, 143.532, 143.532,
            146.916, 146.916, 123.1, 124.0, 123.5, 121.2, 180.094, 158.936, 160.936, 140.22 };

            Sol = new double[]{3.25,5.68,6.05,6.4,6.73,6.73,7.021,7.021,7.1561,7.266,8.196,7.849,9.158,7.2872,7.43,
            7.826,7.7851,8.915,7.551,7.9251,7.9251,7.826,7.826,8.787,8.769,8.818,8.987,7.433,8.3351,7.8351,8.5676};

            RON = new double[] {0.0,0.0,0.0,0.0,100.2,95.0,92.704,61.7,82.742,31.0,100.0,96.0,120.0,66.879,0.0,73.8,78.994,
            110.0,58.0,46.5,72.547,31.2,65.0,124.0,146.0,145.0,120.0,56.667,43.77,0.0,110.7};

            MON = new double[] {0.0,0.0,0.0,0.0,97.6,93.5,88.575,61.3,83.314,30.0,77.2,85.0,114.8,69.675,0.0,
            71.1,72.923,102.0,61.0,40.8,70.087,28.1,63.0,107.0,127.0,124.0,103.0,64.686,41.641,0.0,101.75};

            RVP = new double[] {0.0,0.0,0.0,90.0,72.2,51.6,20.4,15.6,7.0,5.0,2.3,4.5,3.2,2.3,1.6,1.6,2.0,
            1.0,0.7,0.5,0.7,0.7,0.7,0.4,0.3,0.3,0.3,0.3,0.2,0.3,0.1};

            StdHeatForm = new double[] {0.0,-32200.2,-36424.8,-44676.0,-57870.0,-54270.0,-66456.0,-63000.0,-75778.2,-71928.0,-52974.0,-45900.0,35676.0,
            -84576.6,-80802.0,-66582.0,-57498.3,21510.0,-92296.8,-73890.0,-77823.0,-63702.0,-68916.6,12816.0,7722.0,
            7416.0,8172.0,-101057.4,-88524.0,-79380.0,-3681.0};

            MW = new double[] {2.016,16.043,30.07,44.097,58.124,58.124,72.151,72.151,86.178,86.178,84.162,84.162,78.114,100.205,100.205,
            98.189,98.189,92.141,114.232,112.216,112.216,112.216,112.216,106.168,106.168,106.168,106.168,128.259,126.243,
            126.243,120.195,128.259,126.243,120.195,142.286,140.27,134.222,156.312,154.296,148.248,170.338,168.322};

            CpCoeft = new double[NumComp][];
            CpCoeft[0] = new double[] { 6.3539, 0.167945, -0.014925, 0.00046677 };
            CpCoeft[1] = new double[] { 6.367, 0.102427, 0.067468, -0.00224337 };
            CpCoeft[2] = new double[] { 2.3112, 2.01391, -0.015322, -0.00076208 };
            CpCoeft[3] = new double[] { -1.0719, 4.03765, -0.11203, 0.0010955 };
            CpCoeft[4] = new double[] { -2.4505, 5.6504, -0.177432, 0.00214335 };
            CpCoeft[5] = new double[] { -0.1302, 5.01947, -0.128858, 0.0009526 };
            CpCoeft[6] = new double[] { -2.28095, 6.692725, -0.194371, 0.00200046 };
            CpCoeft[7] = new double[] { -0.4145, 6.28296, -0.167989, 0.00138127 };
            CpCoeft[8] = new double[] { -5.9217, 9.1914463, -0.357933, 0.00612045 };
            CpCoeft[9] = new double[] { -0.7017, 7.56254387, -0.209619, 0.0019052 };
            CpCoeft[10] = new double[] { -11.9067, 7.71600485, -0.139477, -0.00114312 };
            CpCoeft[11] = new double[] { -11.2874, 8.19076347, -0.23556, 0.00209572 };
            CpCoeft[12] = new double[] { -10.52, 6.9534831, -0.277153, 0.00433432 };
            CpCoeft[13] = new double[] { -0.955, 8.828329, -0.249743, 0.0023815 };
            CpCoeft[14] = new double[] { -0.995, 8.828329, -0.249743, 0.0023815 };
            CpCoeft[15] = new double[] { -12.8626, 9.70844364, -0.251213, 0.00133364 };
            CpCoeft[16] = new double[] { -13.043, 9.98848057, -0.316836, 0.00359606 };
            CpCoeft[17] = new double[] { -9.1226, 7.66801119, -0.272413, 0.0036675 };
            CpCoeft[18] = new double[] { -1.2219, 10.096693, -0.289976, 0.0028578 };
            CpCoeft[19] = new double[] { -13.5338, 11.1712523, -0.30787, 0.00223861 };
            CpCoeft[20] = new double[] { -13.3602, 10.9259481, -0.27381, 0.00109549 };
            CpCoeft[21] = new double[] { -15.94786, 12.1267424, -0.457856, 0.00752553 };
            CpCoeft[22] = new double[] { -15.94786, 12.1267424, -0.457856, 0.00752553 };
            CpCoeft[23] = new double[] { -10.2083, 9.336, -0.348471, 0.0050964 };
            CpCoeft[24] = new double[] { -5.38286, 7.781393, -0.22259, 0.0019052 };
            CpCoeft[25] = new double[] { -6.5457, 8.188448, -0.257013, 0.00281017 };
            CpCoeft[26] = new double[] { -3.38786, 7.75355, -0.236148, 0.00247676 };
            CpCoeft[27] = new double[] { -1.40476, 11.34312, -0.3284465, 0.00328647 };
            CpCoeft[28] = new double[] { -13.8138, 12.6579, -0.370591, 0.00323884 };
            CpCoeft[29] = new double[] { -16.0852, 13.35798, -0.494782, 0.00790657 };
            CpCoeft[30] = new double[] { -9.1543, 10.2822752, -0.361993, 0.00485825 };
        }

        public void initRxData()
        {
            double CrackP6 = 1.0;
            double CrackP7 = 1.05;
            double CrackP8 = 1.09;
            double DealkA6 = 1.0;
            double DealkA7 = 1.3;
            double DealkA8 = 1.05;
            double CycleP6 = 1.0;
            double CycleP7 = 1.0;
            double CycleP8 = 1.25;
            double CycleP9 = 1.56;
            double DehydN6 = 1.9;
            double DehydN7 = 1.8;
            double DehydN8 = 1.5;
            double DehydN9 = 1.4;
            double EquilCyc9;

            A1 = new double[]{16.49944 / DehydN6, 15.9689 / DehydN6, 16.49944 / DehydN7, 15.9689 / DehydN7, 16.77974 / DehydN8, 16.71084 / DehydN8,
                16.3744 / DehydN8,16.3172 / DehydN8, 16.89464 / DehydN9, 16.5 / DehydN9, 0.0, 0.0, 12.908, 12.908, 12.908,0.0, 8.2122,
                9.5985, 10.158,11.208, 10.158, 11.208, 10.5148, 2.9775, 5.119, 3.6946, 23.947, 0.0, 0.0, 40.6743 / CycleP6, 40.6097 / CycleP6,
                40.5407 / CycleP6, 41.0806 / CycleP7, 41.0806 / CycleP7, 40.8983 / CycleP7, 40.8983 / CycleP7,39.4746 / CycleP8, 40.0531 / CycleP8,
                39.4746 / CycleP8, 40.0531 / CycleP8, 37.3642 / CycleP9, 35.9782 / CycleP9, 0.0, 0.0, 24.9 / CrackP8, 26.70394, 27.7194 / CrackP7,
                27.1739 / CrackP7, 27.88949 / CrackP6, 27.26899 / CrackP6, 30.34819, 26.76859, 23.63919, 25.88269, 0.0, 0.0,12.69022, 12.69022,
                13.24982, 13.24982, 13.24982, 13.24982, 13.60372, 13.60372, 13.60372, 13.30372, 9.18362, 10.49602 / DealkA7, 15.21074 / DealkA8,
                0.0, 18.1665, 16.8755, 17.1856, 17.1856,15.8967, 15.9867, 16.087, 14.7828,15.8639 / DealkA6,14.5545 / DealkA6 };

            A2 = new double[] {-25056.0 / DehydN6,-25056.0 / DehydN6, -25056.0 / DehydN7, -25056.0 / DehydN7, -25056.0 / DehydN8, -25056.0 / DehydN8,
                -25056.0/DehydN8,-25056.0/DehydN8,-25000.0/DehydN9,-25000.0/DehydN9,0.0,0.0,-21000.0,-21000.0,-21000.0,0.0,-21000.0,-21000.0,-21000.0,-21000.0
                ,-21000.0,-21000.0,-21000.0,-12838.0,-16946.0,-15494.0,-46052.0,0.0,0.0,-60000.0/CycleP6,-60000.0/CycleP6,-60000.0/CycleP6,-60000.0/CycleP7
                ,-60000.0/CycleP7,-60000.0/CycleP7,-60000.0/CycleP7,-60000.0/CycleP8,-60000.0/CycleP8,-60000.0/CycleP8,-60000.0/CycleP8,-58200.0/CycleP9,-58200.0/CycleP9,0.0,0.0
                ,-41648.7/CrackP8,-46052.0,-47134.0/CrackP7,-46052.0/CrackP7,-48408.0/CrackP6,-47400.0/CrackP6,-51948.0,-46836.0,-46052.0,-47400.0,0.0,0.0,-32000.0,-32000.0
                ,-32000.0,-32000.0,-32000.0,-32000.0,-32000.0,-32000.0,-32000.0,-32000.0,-32000.0,-32000.0/DealkA7,-32000.0/DealkA8,0.0,-31374.0,-31374.0
                ,-31374.0,-31374.0,-31374.0,-31374.0,-31374.0,-31374.0,-31374.0/DealkA6,-31374.0/DealkA6};

            B1 = new double[]{47.71902,0.0,47.45232,43.0002,47.74953,47.48208,44.22683,47.7315,47.90527,44.0558,0.0,0.0,-0.54851,0.79004,
                -0.24153,0.0,4.88114,4.45838,3.52239,0.29286,-3.43419,-0.38091,3.84955,0.39547,-0.16411,-0.60385,-1.81158,0.0,0.0,-4.98164,-9.8627,-9.94232,
                -6.34038,-6.10107,-10.35908,-10.39403,-5.55248,-5.67535,-9.43751,-8.94501,-5.40945,-9.25892,0.0,0.0,0.0453,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,
                -1.25772,-0.83495,0.05072,-0.33178,0.9867,0.6948,-0.60358,-0.22108,-0.93067,-0.63876,-0.99102,-0.66898,-0.33683,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0};

            B2 = new double[]{-47771.8,0.0,-46661.3,-42310.4,-46052.7,-45109.7,-41677.8,-46229.4,-44667.7,-40791.1,0.0,0.0,640.0,11.8,-652.2,0.0,
                -3378.5,-4359.9,-4374.8,2787.3,3521.6,-1934.3,-3876.7,1800.2,1676.1,1675.7,1784.4,0.0,0.0,9782.3,13160.7,14221.0,7961.7,9122.6,12600.2,14089.6,9019.9,7714.8,13366.9,11218.0,
                6478.5,10355.1,0.0,0.0,11873.5,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,11548.5,12530.3,14202.4,12270.4,14216.9,11428.0,10446.2,12378.2,9948.0,12736.9,10438.0,10996.0,11659.0,0.0,0.0,0.0,
                0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0};

            C9plusFact[0] = (MW[27] + MW[0]) / 130.266;
            C9plusFact[1] = (MW[28] + MW[0] - MW[19] - MW[1]) / MW[2];
            C9plusFact[2] = (MW[29] + MW[0] - MW[19] - MW[1]) / MW[2];
            C9plusFact[3] = (MW[30] + MW[0] - MW[23] - MW[1]) / MW[2];
            C9plusFact[4] = (MW[28] + 2.0 * MW[0]) / 130.266;
            C9plusFact[5] = (MW[29] + 2.0 * MW[0]) / 130.266;

            HCDistrib[0, 0] = 0.176;
            HCDistrib[1, 0] = 0.176;
            HCDistrib[2, 0] = 0.176;
            HCDistrib[3, 0] = 0.199;
            HCDistrib[4, 0] = 0.199;
            HCDistrib[5, 0] = 0.199;
            HCDistrib[6, 0] = 0.199;
            HCDistrib[7, 0] = 0.199;
            HCDistrib[8, 0] = 0.237;
            HCDistrib[9, 0] = 0.237;
            HCDistrib[10, 0] = 0.237;
            HCDistrib[11, 0] = 0.237;
            HCDistrib[12, 0] = 0.288;
            HCDistrib[13, 0] = 0.288;
            HCDistrib[14, 0] = 0.288;
            HCDistrib[15, 0] = 0.288;
            HCDistrib[16, 0] = 0.5;
            HCDistrib[17, 0] = 0.5;
            HCDistrib[18, 0] = 0.665;
            HCDistrib[19, 0] = 0.665;

            HCDistrib[0, 1] = 0.283;
            HCDistrib[1, 1] = 0.283;
            HCDistrib[2, 1] = 0.283;
            HCDistrib[3, 1] = 0.295;
            HCDistrib[4, 1] = 0.322;
            HCDistrib[5, 1] = 0.322;
            HCDistrib[6, 1] = 0.322;
            HCDistrib[7, 1] = 0.322;
            HCDistrib[8, 1] = 0.366;
            HCDistrib[9, 1] = 0.366;
            HCDistrib[10, 1] = 0.366;
            HCDistrib[11, 1] = 0.366;
            HCDistrib[12, 1] = 0.462;
            HCDistrib[13, 1] = 0.462;
            HCDistrib[14, 1] = 0.462;
            HCDistrib[15, 1] = 0.462;
            HCDistrib[16, 1] = 0.5;
            HCDistrib[17, 1] = 0.5;
            HCDistrib[18, 1] = 0.67;
            HCDistrib[19, 1] = 0.67;
            HCDistrib[0, 2] = 0.286;
            HCDistrib[1, 2] = 0.308;
            HCDistrib[2, 2] = 0.308;
            HCDistrib[3, 2] = 0.376;
            HCDistrib[4, 2] = 0.349;
            HCDistrib[5, 2] = 0.349;
            HCDistrib[6, 2] = 0.349;
            HCDistrib[7, 2] = 0.349;
            HCDistrib[8, 2] = 0.397;
            HCDistrib[9, 2] = 0.397;
            HCDistrib[10, 2] = 0.397;
            HCDistrib[11, 2] = 0.397;
            HCDistrib[12, 2] = 0.5;
            HCDistrib[13, 2] = 0.5;
            HCDistrib[14, 2] = 0.5;
            HCDistrib[15, 2] = 0.5;
            HCDistrib[16, 2] = 0.5;
            HCDistrib[17, 2] = 0.5;
            HCDistrib[18, 2] = 0.665;
            HCDistrib[19, 2] = 0.665;
            HCDistrib[0, 3] = 0.091;
            HCDistrib[1, 3] = 0.083;
            HCDistrib[2, 3] = 0.083;
            HCDistrib[3, 3] = 0.092;
            HCDistrib[4, 3] = 0.092;
            HCDistrib[5, 3] = 0.092;
            HCDistrib[6, 3] = 0.092;
            HCDistrib[7, 3] = 0.092;
            HCDistrib[8, 3] = 0.141;
            HCDistrib[9, 3] = 0.141;
            HCDistrib[10, 3] = 0.141;
            HCDistrib[11, 3] = 0.141;
            HCDistrib[12, 3] = 0.164;
            HCDistrib[13, 3] = 0.164;
            HCDistrib[14, 3] = 0.164;
            HCDistrib[15, 3] = 0.164;
            HCDistrib[16, 3] = 0.2;
            HCDistrib[17, 3] = 0.2;
            HCDistrib[18, 3] = 0.0;
            HCDistrib[19, 3] = 0.0;
            HCDistrib[0, 4] = 0.164;
            HCDistrib[1, 4] = 0.15;
            HCDistrib[2, 4] = 0.15;
            HCDistrib[3, 4] = 0.168;
            HCDistrib[4, 4] = 0.168;
            HCDistrib[5, 4] = 0.168;
            HCDistrib[6, 4] = 0.168;
            HCDistrib[7, 4] = 0.168;
            HCDistrib[8, 4] = 0.256;
            HCDistrib[9, 4] = 0.256;
            HCDistrib[10, 4] = 0.256;
            HCDistrib[11, 4] = 0.256;
            HCDistrib[12, 4] = 0.298;
            HCDistrib[13, 4] = 0.298;
            HCDistrib[14, 4] = 0.298;
            HCDistrib[15, 4] = 0.298;
            HCDistrib[16, 4] = 0.3;
            HCDistrib[17, 4] = 0.3;
            HCDistrib[18, 4] = 0.0;
            HCDistrib[19, 4] = 0.0;
            HCDistrib[0, 5] = 0.172;
            HCDistrib[1, 5] = 0.15;
            HCDistrib[2, 5] = 0.15;
            HCDistrib[3, 5] = 0.26;
            HCDistrib[4, 5] = 0.233;
            HCDistrib[5, 5] = 0.233;
            HCDistrib[6, 5] = 0.233;
            HCDistrib[7, 5] = 0.233;
            HCDistrib[8, 5] = 0.234;
            HCDistrib[9, 5] = 0.234;
            HCDistrib[10, 5] = 0.234;
            HCDistrib[11, 5] = 0.234;
            HCDistrib[12, 5] = 0.192;
            HCDistrib[13, 5] = 0.192;
            HCDistrib[14, 5] = 0.192;
            HCDistrib[15, 5] = 0.192;
            HCDistrib[16, 5] = 0.0;
            HCDistrib[17, 5] = 0.0;
            HCDistrib[18, 5] = 0.0;
            HCDistrib[19, 5] = 0.0;
            HCDistrib[0, 6] = 0.083;
            HCDistrib[1, 6] = 0.083;
            HCDistrib[2, 6] = 0.083;
            HCDistrib[3, 6] = 0.116;
            HCDistrib[4, 6] = 0.116;
            HCDistrib[5, 6] = 0.116;
            HCDistrib[6, 6] = 0.116;
            HCDistrib[7, 6] = 0.116;
            HCDistrib[8, 6] = 0.132;
            HCDistrib[9, 6] = 0.132;
            HCDistrib[10, 6] = 0.132;
            HCDistrib[11, 6] = 0.132;
            HCDistrib[12, 6] = 0.096;
            HCDistrib[13, 6] = 0.096;
            HCDistrib[14, 6] = 0.096;
            HCDistrib[15, 6] = 0.096;
            HCDistrib[16, 6] = 0.0;
            HCDistrib[17, 6] = 0.0;
            HCDistrib[18, 6] = 0.0;
            HCDistrib[19, 6] = 0.0;
            HCDistrib[0, 7] = 0.206;
            HCDistrib[1, 7] = 0.206;
            HCDistrib[2, 7] = 0.206;
            HCDistrib[3, 7] = 0.215;
            HCDistrib[4, 7] = 0.215;
            HCDistrib[5, 7] = 0.215;
            HCDistrib[6, 7] = 0.215;
            HCDistrib[7, 7] = 0.215;
            HCDistrib[8, 7] = 0.158;
            HCDistrib[9, 7] = 0.158;
            HCDistrib[10, 7] = 0.158;
            HCDistrib[11, 7] = 0.158;
            HCDistrib[12, 7] = 0.0;
            HCDistrib[13, 7] = 0.0;
            HCDistrib[14, 7] = 0.0;
            HCDistrib[15, 7] = 0.0;
            HCDistrib[16, 7] = 0.0;
            HCDistrib[17, 7] = 0.0;
            HCDistrib[18, 7] = 0.0;
            HCDistrib[19, 7] = 0.0;
            HCDistrib[0, 8] = 0.08;
            HCDistrib[1, 8] = 0.102;
            HCDistrib[2, 8] = 0.102;
            HCDistrib[3, 8] = 0.08;
            HCDistrib[4, 8] = 0.107;
            HCDistrib[5, 8] = 0.107;
            HCDistrib[6, 8] = 0.107;
            HCDistrib[7, 8] = 0.107;
            HCDistrib[8, 8] = 0.079;
            HCDistrib[9, 8] = 0.079;
            HCDistrib[10, 8] = 0.079;
            HCDistrib[11, 8] = 0.079;
            HCDistrib[12, 8] = 0.0;
            HCDistrib[13, 8] = 0.0;
            HCDistrib[14, 8] = 0.0;
            HCDistrib[15, 8] = 0.0;
            HCDistrib[16, 8] = 0.0;
            HCDistrib[17, 8] = 0.0;
            HCDistrib[18, 8] = 0.0;
            HCDistrib[19, 8] = 0.0;
            HCDistrib[0, 9] = 0.19;
            HCDistrib[1, 9] = 0.19;
            HCDistrib[2, 9] = 0.19;
            HCDistrib[3, 9] = 0.133;
            HCDistrib[4, 9] = 0.133;
            HCDistrib[5, 9] = 0.133;
            HCDistrib[6, 9] = 0.133;
            HCDistrib[7, 9] = 0.133;
            HCDistrib[8, 9] = 0.0;
            HCDistrib[9, 9] = 0.0;
            HCDistrib[10, 9] = 0.0;
            HCDistrib[11, 9] = 0.0;
            HCDistrib[12, 9] = 0.0;
            HCDistrib[13, 9] = 0.0;
            HCDistrib[14, 9] = 0.0;
            HCDistrib[15, 9] = 0.0;
            HCDistrib[16, 9] = 0.0;
            HCDistrib[17, 9] = 0.0;
            HCDistrib[18, 9] = 0.0;
            HCDistrib[19, 9] = 0.0;
            HCDistrib[0, 10] = 0.093;
            HCDistrib[1, 10] = 0.093;
            HCDistrib[2, 10] = 0.093;
            HCDistrib[3, 10] = 0.066;
            HCDistrib[4, 10] = 0.066;
            HCDistrib[5, 10] = 0.066;
            HCDistrib[6, 10] = 0.066;
            HCDistrib[7, 10] = 0.066;
            HCDistrib[8, 10] = 0.0;
            HCDistrib[9, 10] = 0.0;
            HCDistrib[10, 10] = 0.0;
            HCDistrib[11, 10] = 0.0;
            HCDistrib[12, 10] = 0.0;
            HCDistrib[13, 10] = 0.0;
            HCDistrib[14, 10] = 0.0;
            HCDistrib[15, 10] = 0.0;
            HCDistrib[16, 10] = 0.0;
            HCDistrib[17, 10] = 0.0;
            HCDistrib[18, 10] = 0.0;
            HCDistrib[19, 10] = 0.0;
            HCDistrib[0, 11] = 0.176;
            HCDistrib[1, 11] = 0.176;
            HCDistrib[2, 11] = 0.176;
            HCDistrib[3, 11] = 0.0;
            HCDistrib[4, 11] = 0.0;
            HCDistrib[5, 11] = 0.0;
            HCDistrib[6, 11] = 0.0;
            HCDistrib[7, 11] = 0.0;
            HCDistrib[8, 11] = 0.0;
            HCDistrib[9, 11] = 0.0;
            HCDistrib[10, 11] = 0.0;
            HCDistrib[11, 11] = 0.0;
            HCDistrib[12, 11] = 0.0;
            HCDistrib[13, 11] = 0.0;
            HCDistrib[14, 11] = 0.0;
            HCDistrib[15, 11] = 0.0;
            HCDistrib[16, 11] = 0.0;
            HCDistrib[17, 11] = 0.0;
            HCDistrib[18, 11] = 0.0;
            HCDistrib[19, 11] = 0.0;
        }

        private static void CheckContinue()
        {
        }

        private void CalcOctane(double[] RefMol, ref double SumRON, ref double SumMON)
        {
            double[] RefLbs = new double[NumComp];
            double[] RefBPD = new double[NumComp];
            double C5PlusBPD = 0.0;
            double C5PlusLbs = 0.0;
            SumRON = 0.0;
            SumMON = 0.0;

            for (int i = 6; i < NumComp; i++)
            {
                RefLbs[i] = RefMol[i] * MW[i];
                RefBPD[i] = RefLbs[i] / (SG[i] * 14.574166);
                C5PlusBPD += RefBPD[i];
                C5PlusLbs += RefLbs[i];
                SumRON += RON[i] * RefBPD[i];
                SumMON += MON[i] * RefBPD[i];
            }

            SumRON /= C5PlusBPD;
            SumMON /= C5PlusBPD;
        }
    }
}