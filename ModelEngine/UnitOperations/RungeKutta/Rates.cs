using System;
using Units.UOM;

namespace ModelEngine
{
    public partial class RK4
    {
        private readonly int NumComp = 1;
        private readonly int NumRxn = 1;
        private readonly double[] A1, A2, B1, B2;
        private readonly double MolFeed;

        public double[] StdHeatForm { get; private set; }
        public double[][] CpCoeft { get; private set; }
        public int NumDepVar { get; private set; }

        private void CalcRates(Temperature TR, double[] MoleFlowRate, double[] RateOfChange)
        {
            double[] RateK = new double[81];
            double[] EquilK = new double[81];
            double[] RxnRate = new double[81];
            double TempTotal = 0.0;
            double TRankine = TR.Rankine;

            for (int i = 0; i < NumComp; i++)
                TempTotal += MoleFlowRate[i];

            for (int i = 0; i < NumRxn; i++)
            {
                RateK[i] = Math.Exp(A1[i] + A2[i] / TRankine);
                EquilK[i] = Math.Exp(B1[i] + B2[i] / TRankine);
            }

            RateOfChange[0] = 3.0 * (RxnRate[0] + RxnRate[1] + RxnRate[2] + RxnRate[3] + RxnRate[4] + RxnRate[5] + RxnRate[6] + RxnRate[7] + RxnRate[8] + RxnRate[9]);

            RateOfChange[30] = RxnRate[8] + RxnRate[9] - RxnRate[68];

            for (int i = 0; i < NumComp; i++)
                RateOfChange[i] *= MolFeed;

            double T1 = TR.Rankine * 0.01;
            double T2 = T1 * T1;
            double T3 = T1 * T2;
            double T4 = T1 * T3;
            double TD1 = T1 - 5.36688;
            double TD2 = T2 / 2.0 - 14.4017;
            double TD3 = T3 / 3.0 - 51.52813;
            double TD4 = T4 / 4.0 - 207.40898;
            double SumHF = 0.0;
            double SumCP = 0.0;

            for (int i = 0; i < NumComp; i++)
            {
                SumHF += RateOfChange[i] * (StdHeatForm[i] + (CpCoeft[i][0] * TD1 + CpCoeft[i][1] * TD2 + CpCoeft[i][2] * TD3 + CpCoeft[i][3] * TD4) * 100.0);
                SumCP += MoleFlowRate[i] * (CpCoeft[i][0] + CpCoeft[i][1] * T1 + CpCoeft[i][2] * T2 + CpCoeft[i][3] * T3);
            }

            double DelTHB = SumHF / SumCP;
            RateOfChange[NumDepVar - 1] = -DelTHB / 1.8;
        }
    }
}