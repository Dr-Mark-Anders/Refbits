using System;
using System.Collections.Generic;
using Units.UOM;

namespace ModelEngine
{
    public partial class RK4
    {
        public List<double[]> Profile = new();
        private readonly List<double> dWarray1 = new();
        private readonly List<double> dWarray2 = new();
        private readonly List<double> dWarray3 = new();
        private readonly List<double> dWarray4 = new();

        private bool RungeKutta(ref Temperature TR, Pressure ReactorP, Mass AmtCat, double[] FeedMoleFractions, bool FinalLoop, int RxNo)
        {
            double[] Finter = new double[32];
            double[] Fnew = new double[32];
            double[] k1 = new double[32];
            double[] k2 = new double[32];
            double[] k3 = new double[32];
            double[] k4 = new double[32];
            double[] resid = new double[32];
            double[] dFdW = new double[32];
            double MaxN = 10000.0;
            double MaxdW = AmtCat / 25;
            double MindW = AmtCat / MaxN;
            double dW = MaxdW;
            double MaxResid = 0.05;
            double BigResid = 0.0;
            double Weight = 0.0;
            int LoopCount = 0;

            if (FinalLoop)
            {
                Profile.Clear();
                Profile.Add((double[])FeedMoleFractions.Clone());
                switch (RxNo)
                {
                    case 0:
                        dWarray1.Add(0);
                        break;

                    case 1:
                        dWarray2.Add(0);
                        break;

                    case 2:
                        dWarray3.Add(0);
                        break;

                    case 3:
                        dWarray4.Add(0);
                        break;
                }
            }

            while ((double)0.0 < AmtCat)
            {
                LoopCount++;
            whileloop:

                CalcRates(TR, FeedMoleFractions, dFdW);  // rate in lbmoles

                for (int J = 0; J < NumDepVar; J++)
                    k1[J] = dW * dFdW[J];

                for (int J = 0; J < NumComp; J++)
                    Finter[J] = FeedMoleFractions[J] + k1[J] / 2.0;

                Temperature TRinter = TR + k1[NumComp] / 2.0;

                CalcRates(TRinter, FeedMoleFractions, dFdW);

                for (int J = 0; J < NumDepVar; J++)
                    k2[J] = dW * dFdW[J];

                for (int J = 0; J < NumComp; J++)
                    Finter[J] = FeedMoleFractions[J] + k2[J] / 2.0;

                TRinter = TR + k2[NumComp] / 2.0;

                CalcRates(TRinter, FeedMoleFractions, dFdW);

                for (int J = 0; J < NumDepVar; J++)
                    k3[J] = dW * dFdW[J];

                for (int J = 0; J < NumComp; J++)
                    Finter[J] = FeedMoleFractions[J] + k3[J];

                TRinter = TR + k3[NumComp];
                CalcRates(TRinter, Finter, dFdW);

                for (int J = 0; J < NumDepVar; J++)
                    k4[J] = dW * dFdW[J];

                for (int J = 0; J < NumComp; J++)
                    Fnew[J] = FeedMoleFractions[J] + (k1[J] + 2.0 * k2[J] + 2.0 * k3[J] + k4[J]) / 6.0;

                Temperature TRnew = TR + (k1[NumComp] + 2.0 * k2[NumComp] + 2.0 * k3[NumComp] + k4[NumComp]) / 6.0;

                CalcRates(TRnew, Fnew, dFdW);

                for (int J = 0; J < NumComp; J++)
                    resid[J] = (Fnew[J] - FeedMoleFractions[J]) / dW - dFdW[J];

                resid[NumComp] = (TRnew - TR) / dW - dFdW[NumComp];

                for (int J = 0; J < NumDepVar; J++)
                {
                    if (Math.Abs(resid[J]) > BigResid)
                    {
                        if (Math.Abs(resid[J]) > MaxResid)
                        {
                            if (dW > MindW)
                            {
                                dW /= 2.0;
                                goto whileloop;
                            }
                            else
                            {
                                //EndWell("Simulation abandoned due to failure to converge");
                                return false;
                            }
                        }
                        BigResid = resid[J];
                    }
                }

                for (int I = 0; I < NumComp; I++)
                    FeedMoleFractions[I] = Fnew[I];

                TR = TRnew;
                Weight += dW;

                if (FinalLoop)
                {
                    switch (RxNo)
                    {
                        case 0:
                            Profile.Add((double[])Fnew.Clone());
                            dWarray1.Add((double)0.0 / AmtCat);
                            break;

                        case 1:
                            Profile.Add((double[])Fnew.Clone());
                            dWarray2.Add((double)0.0 / AmtCat);
                            break;

                        case 2:
                            Profile.Add((double[])Fnew.Clone());
                            dWarray3.Add((double)0.0 / AmtCat);
                            break;

                        case 3:
                            Profile.Add((double[])Fnew.Clone());
                            dWarray4.Add((double)0.0 / AmtCat);
                            break;
                    }
                }
            }
            return true;
        }
    }
}