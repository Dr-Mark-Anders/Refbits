using System.Collections.Generic;
using Units.UOM;

namespace GenericRxBed
{
    public partial class BaseRxBed
    {
        public BaseReaction[] reactions;
        public BaseReactionSet rSet = new BaseReactionSet();
        public double[] RxnRate = new double[80];
        public double[] RateOfChange;
        public BaseReaction[] NormalReactionsArray;
        public int NoRxs;
        public int NoComps;
        public double[] Composition;
        public double FeedRate = 0;
        public double forward;
        public double[] HFORM25;
        public double[][] Cp;
        public Dictionary<string, int> NameDict = new Dictionary<string, int>();

        public virtual double[] Solve(Temperature Tk, Pressure P, MoleFlow[] MoleFlows, double MetalAct, double AcidAct, MoleFlow NaphMolFeed)
        {
            //Parallel.For(0,NoRxs,i=>

            for (int i = 0; i < NoRxs; i++)
            {
                BaseReaction r = reactions[i];
                RxnRate[i] = r.Solve(Tk._Kelvin, Composition);
            }

            for (int i = 0; i < NormalReactionsArray.Length; i++)
            {
                BaseReaction r = NormalReactionsArray[i];
                for (int J = 0; J < r.Feeds.Length; J++)
                {
                    int index = r.FeedPositionIndex[J];
                    RateOfChange[index] += r.Rate * r.stoich[J];
                }

                for (int J = 0; J < r.Products.Length; J++)
                {
                    int index = r.ProdsPositionIndex[J];
                    RateOfChange[index] += r.Rate * r.stoich[J + r.Feeds.Length];
                }
            }

            for (int i = 0; i <= NoComps; i++)
                RateOfChange[i] *= NaphMolFeed;

            double T1 = Tk.Kelvin;
            double T2 = T1 * T1;
            double T3 = T1 * T2;
            double T4 = T1 * T3;
            double TD1 = Tk.Celsius - 25;
            double TD2 = T2 / 2.0;
            double TD3 = T3 / 3.0;
            double TD4 = T4 / 4.0;
            double SumHF = 0.0;
            double SumCP = 0.0;

            for (int i = 0; i <= NoComps; i++)
            {
                SumHF += RateOfChange[i] * (HFORM25[i] + (Cp[i][0] * TD1 + Cp[i][1] * TD2 + Cp[i][2] * TD3 + Cp[i][3] * TD4));//enthalpychangesDKJ
                SumCP += MoleFlows[i] * (Cp[i][0] + Cp[i][1] * T1 + Cp[i][2] * T2 + Cp[i][3] * T3);//HeatCapcityinkJ/C
            }

            double DelTHB = SumHF / SumCP;
            RateOfChange[NoComps + 1] = -DelTHB;

            return RateOfChange;
        }
    }
}