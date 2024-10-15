using System;
using Units.UOM;

namespace ModelEngine
{
    public class ChaoSeader
    {
        private void Flash(Components F, Temperature TFlash, Pressure PFlash, double FracLiq, double[] F_Liq, double[] F_Vap)
        {
            //Chao-Seaderflashcalculation.Givenamolarstreamcompositionand
            //separationT(indegR)&P(inpsia),return  stheseparatorL/F(liquidfraction),
            //andliquidandvaporcomposition(inmol/hr).
            //
            //CallsKIdealandKRealtocomputeequilibriumKvalues.Attemptscompositionconvergenceusing
            //thefollowingmodifiedflashequations:
            //Sum(n=1toNumComp)(FeedFrac_i)(1.0-K_i)/((L/F)*(1.0-K_i)+K_i))=0.0
            //whereX_i=(FeedFrac_i)/((L/F)*(1.0-K_i)+K_i))andY_i=K_i*X_i

            int NumComp = F.Count;

            double[] EqK = new double[NumComp + 1];//EquilibriumKvalues(vaporizationequilibirumratios)forflashcalc
            double[] newX = new double[NumComp + 1];//new valueofliquidmolefraction
            double TotFeed = 0;//Totalseparatorfeed(mol/hr)
            double[] X = new double[NumComp + 1];//Liquidmolefraction
            double[] Y = new double[NumComp + 1];//Vapormolefraction

            double[] FeedFrac = new double[NumComp + 1];//Separatorfeedmolefraction
            bool RecalcK = false;//FlagtorecalculateKvalue
            double SumX = 0, SumY = 0;//Sumofliquidmolefractions,sumvapormolefractions

            double Deriv = 0, Func = 0, Eras = 0;//Usedinflashconvergencetechnique

            double Term1 = 0;//Term(1.0-K_i)offlashequation
            double Denom = 0;//Denominatorofflashequation
            double FracLiqnew = 0;//new valueforliquidfraction(L/F)
            int MaxTrials = 0;//Maximumnumberoftrialanderrorattemptstosolveflashcalc
            bool Solved = false;//Flagifvariablesolvedwithint hetrialanderrorroutine

            double TotLiq = 0, TotVap = 0;

            //Totalmoles/hrofliquidandvapor(gas)leavingseparator
            int I = 0, J = 0;
            //Counters

            int CountFracLiq0 = 0;
            int CountFracLiq1 = 0;

            FracLiq = 0.5;//initialguess

            for (I = 1; I <= NumComp; I++)
            {
                FeedFrac[I] = F[I].MoleFraction;
            }

            KIdeal(F, TFlash, PFlash, EqK);

            for (I = 1; I <= NumComp; I++)
            {
                X[I] = 0.0;
                Y[I] = 0.0;
            }

            MaxTrials = 100;
            Solved = false;

            for (J = 1; J <= MaxTrials; J++)
            {
                Deriv = 0.0;
                Func = 0.0;
                RecalcK = false;
                for (I = 1; I <= NumComp; I++)
                {
                    Term1 = 1.0 - EqK[I];
                    Denom = FracLiq * Term1 + EqK[I];
                    newX[I] = FeedFrac[I] / Denom;
                    if ((Math.Abs(newX[I] - X[I]) > 1E-05))
                    {
                        RecalcK = true;
                    }
                    Eras = FeedFrac[I] * Term1 / Denom;
                    Func = Func + Eras;
                    Deriv = Deriv + Eras * Term1 / Denom;
                }

                if ((Math.Abs(Func) - 1E-06) > 0.0)
                {
                    FracLiqnew = FracLiq + Func / Deriv;
                    //Firsttestisnotpartoftheoriginalcode.Originalcodedidnot
                    //lookfordeworbubblepoint s,wheremanyiterationswillhappenwith
                    //sameFracLiqas0or1.
                    if (FracLiqnew <= 0.0)
                    {
                        FracLiq = 0.5 * FracLiq;
                        CountFracLiq0 = CountFracLiq0 + 1;
                        if (CountFracLiq0 > 25)
                        {
                            //Tisabovedewpoint
                            Solved = true;
                            break;//TODO:mightnotbecorrect.Was:ExitFor
                        }
                    }
                    else if (FracLiqnew > 1.0)
                    {
                        FracLiq = 0.5 * (FracLiq + 1.0);
                        CountFracLiq1 = CountFracLiq1 + 1;
                        if (CountFracLiq1 > 25)
                        {
                            //Tisbelowbubblepoint
                            Solved = true;
                            break;//TODO:mightnotbecorrect.Was:ExitFor
                        }
                    }
                    else
                    {
                        FracLiq = FracLiqnew;
                    }
                }
                else
                {
                    SumX = 0.0;
                    SumY = 0.0;
                    for (I = 1; I <= NumComp; I++)
                    {
                        Term1 = 1.0 - EqK[I];
                        Denom = FracLiq * Term1 + EqK[I];
                        X[I] = FeedFrac[I] / Denom;
                        Y[I] = EqK[I] * X[I];
                        SumX = SumX + X[I];
                        SumY = SumY + Y[I];
                    }
                    for (I = 1; I <= NumComp; I++)
                    {
                        X[I] = X[I] / SumX;
                        Y[I] = Y[I] / SumY;
                    }
                    if ((RecalcK == true))
                    {
                        KReal(F, TFlash, PFlash, EqK, X, Y);
                    }
                    else
                    {
                        Solved = true;
                        break;//TODO:mightnotbecorrect.Was:ExitFor
                    }
                }
            }

            if ((Solved == false))
            {
                //Message="Flashcalculationfailed.Simulationabandoned.";
                //EndWell(Message);
            }

            TotLiq = TotFeed * FracLiq;
            TotVap = TotFeed - TotLiq;
            for (I = 1; I <= NumComp; I++)
            {
                F_Liq[I] = TotLiq * X[I];
                F_Vap[I] = TotVap * Y[I];
            }
        }

        private void KIdeal(Components comps, Temperature Tsep, Pressure Psep, double[] EqK)
        {
            //ComputeidealK(vaporizationequilibirumratio,K=y/x)valuesusing theChao-Seader
            //technique

            int NumComp = comps.Count;
            BaseComp bc;

            //DimIAsint eger'Counter
            double[] GamLog = new double[NumComp];//'Naturallogofgamma(liquidactivitycoefficient)
            double Tr, TR2, TR3, Pr, PR2, Term;//ReducedTemperature
                                               //Reducedtempsquared,cubed
                                               //ReducedPressure  ,reducedPressure  squared
                                               //int ermediatecalculation
            double[] FLLog = new double[NumComp];//Naturallogofliquidfugacitycoefficient
            double[] FVLog = new double[NumComp];//Naturallogofvaporfugacitycoefficient

            for (int I = 0; I < NumComp; I++)
            {
                bc = comps[I];
                GamLog[I] = 0;
                Tr = Tsep / bc.CritT;
                Pr = Psep / bc.CritP;
                TR2 = Tr * Tr;
                TR3 = TR2 * Tr;
                PR2 = Pr * Pr;
                //calculatenaturallogofliquidfugacityusing Chao-Seaderliquid
                //fugacitycoefficients.Useseparatecoefficientsforhydrogen,methane,
                //andgeneralizedfluids
                if (bc.MW < 3)//H2
                {
                    Term = 1.96718 + 1.02972 / Tr - 0.054009 * Tr + 0.0005288 * TR2 + 0.008585 * Pr;
                }
                else if (bc.MW < 16.1 && bc.MW >= 16.0)//CH4
                {
                    Term = 2.4384 - 2.2455 / Tr - 0.34084 * Tr + 0.00212 * TR2 - 0.00223 * TR3 + Pr * (0.10486 - 0.03691 * Tr);
                }
                else
                {
                    Term = 5.75748 - 3.01761 / Tr - 4.985 * Tr + 2.02299 * TR2
                    + Pr * (0.08427 + 0.26667 * Tr - 0.31138 * TR2) + PR2 * (-0.02655 + 0.02883 * Tr)
                    + bc.Omega * (-4.23893 + 8.65808 * Tr - 1.2206 / Tr - 3.15224 * TR3 - 0.025 * (Pr - 0.6));
                }

                FLLog[I] = Term * 2.3025851;
                FVLog[I] = 0;
                EqK[I] = Math.Exp(FLLog[I] + GamLog[I] - FVLog[I]) / Pr;
            }
        }

        private void KReal(Components comps, Temperature Tsep, Pressure Psep, double[] EqK, double[] X, double[] Y)
        {
            //ComputerealK(vaporizationequilibirumratio,K=y/x)valuesusing theChao-Seader
            //technique
            int NumComp = comps.Count;
            BaseComp bc;

            double[] A_EOS = new double[NumComp];//ConstantafortheRedlich-Kwongequationofstate
            double[] B_EOS = new double[NumComp];//ConstantbfortheRedlich-Kwongequationofstate
            double AMix, BMix;//Redlich-Kwongconstantsforthemixture
            double AOB, BP;//termsinRedlich-Kwongequation
            double AOAMix, BOBMix, RT;
            double Tr;//ReducedTemperature
            double TR2, TR3;//Reducedtempsquared,cubed
            double Pr, PR2;//ReducedPressure  ,reducedPressure  squared
            double Term, Term1, Term2, Term3;//int ermediatecalculations
            double[] FLLog = new double[NumComp];//Naturallogofliquidfugacitycoefficient
            double[] FVLog = new double[NumComp];//Naturallogofvaporfugacitycoefficient
            double Z, Znew;//Compressibilityfactor
            int MaxTrials;//MaximumnumberoftrialanderrorattemptstosolveforZ
            bool Solved;//Flagtocheckifvariablesolvedwithint hetrialanderrorroutine
            double Func, Deriv;//Forsolutionbytrialanderrorbyint ervalhalvingmethod
            double VolMix;//Volumeofmixture(molar)
            double SolAvg;//Averagesolubilityparameterforsolution
            double[] GamLog = new double[NumComp];//Naturallogofgamma(liquidactivitycoefficient)

            //CalculateZ(vaporphasecompressibilityfactor)using RedlichandKwongEOS
            //using atrialanderrormethodbyint ervalhalvingtechnique

            MaxTrials = 40;
            AMix = 0;
            BMix = 0;
            for (int I = 0; I < NumComp; I++)
            {
                bc = comps[I];
                Tr = Tsep / bc.CritT;
                A_EOS[I] = Math.Sqrt(0.4278 / (bc.CritP * (Math.Pow(Tr, 2.5))));
                B_EOS[I] = 0.0867 / (bc.CritP * Tr);
                AMix = AMix + A_EOS[I] * Y[I];
                BMix = BMix + B_EOS[I] * Y[I];
            }

            AOB = (Math.Pow(AMix, 2)) / BMix;
            BP = BMix * Psep;
            Z = 1;
            Solved = false;
            for (int J = 0; J < MaxTrials; J++)
            {
                Func = Z / (Z - BP) - AOB * BP / (Z + BP) - Z;
                if ((Math.Abs(Func) - 0.000001) < 0)
                {
                    Solved = true;
                    break;
                }
                Deriv = -BP / Math.Pow((Z - BP), 2) + AOB * BP / Math.Pow((Z + BP), 2) - 1;
                Znew = Z - Func / Deriv;
                if ((Znew - BP) < 0)
                {
                    Z = (Z + BP) * 0.5;
                }
                else if (Znew > 2)
                {
                    Z = (Z + 2) * 0.5;
                }
                else
                {
                    Z = Znew;
                }
            }
            //IfZdidnotconvergewithinMaxTrials,assignavalue
            if (!Solved)
            {
                Z = 1.02;
            }

            VolMix = 0;
            SolAvg = 0;
            for (int I = 0; I < NumComp; I++)
            {
                bc = comps[I];
                Term = X[I] * bc.LiquidMolarVolumeCM3();
                VolMix = VolMix + Term;
                SolAvg = SolAvg + Term * bc.HildebrandSolPar();
            }
            SolAvg = SolAvg / VolMix;
            RT = 1.1038889 * Tsep;
            Term1 = Z - 1;
            if (Z < BP)
            {//temporarysolutiontooMUChhydrogen
                Term2 = 0;
            }
            else
            {
                Term2 = Math.Log(Z - BP);
            }

            Term3 = Math.Log(1 + BP / Z);

            for (int I = 0; I < NumComp; I++)
            {
                bc = comps[I];
                GamLog[I] = (bc.LiquidMolarVolumeCM3() / RT) * Math.Pow((bc.HildebrandSolPar() - SolAvg), 2);
                Tr = Tsep / bc.CritT;
                Pr = Psep / bc.CritP;
                TR2 = Tr * Tr;
                TR3 = TR2 * Tr;
                PR2 = Pr * Pr;
                if (bc.MW < 3)//H2
                {
                    Term = 1.96718 + 1.02972 / Tr - 0.054009 * Tr + 0.0005288 * TR2 + 0.008585 * Pr;
                }
                else if (bc.MW > 16 && bc.MW < 16.2)//CH4
                {
                    Term = 2.4384 - 2.2455 / Tr - 0.34084 * Tr + 0.00212 * TR2 - 0.00223 * TR3 + Pr * (0.10486 - 0.03691 * Tr);
                }
                else
                {
                    Term = 5.75748 - 3.01761 / Tr - 4.985 * Tr + 2.02299 * TR2 + Pr * (0.08427 + 0.26667 * Tr - 0.31138 * TR2)
                    + PR2 * (-0.02655 + 0.02883 * Tr)

                    + bc.Omega * (-4.23893 + 8.65808 * Tr - 1.2206 / Tr - 3.15224 * TR3
                    - 0.025 * (Pr - 0.6));
                }
                FLLog[I] = Term * 2.3025851;
                BOBMix = B_EOS[I] / BMix;
                AOAMix = 2 * A_EOS[I] / AMix;
                FVLog[I] = Term1 * BOBMix - Term2 - AOB * (AOAMix - BOBMix) * Term3;
                EqK[I] = Math.Exp(FLLog[I] + GamLog[I] - FVLog[I]) / Pr;
            }
            return;
        }

        public static Pressure PSat(Components cc, Temperature T)
        {
            int NoComps = cc.ComponentList.Count;
            double[] K = new double[NoComps];
            Pressure PCritMix;
            Temperature TCritMix;
            double SumK = 0, oldsum = 0;
            double delta = 0.1;
            bool crossedover = false;
            Pressure P = 5;

            KaysMixingRules.KaysMixing(cc, out TCritMix, out PCritMix);

            for (int i = 0; i <= 1000; i++)
            {
                K = GetKReal(cc, P, cc.MoleFractions, T);

                SumK = 0;

                for (int ii = 0; ii < NoComps; ii++)
                {
                    SumK += K[ii] * cc[ii].MoleFraction;
                }

                if (Math.Abs(SumK - 1) > 0.0001)//notfinished
                {
                    if (SumK > 1)
                        P += delta;
                    else if (SumK < 1)
                        P -= delta;

                    if (P <= 0)
                        return -999;
                }
                else
                    return P;//converged

                if (oldsum > 1 && SumK < 1 || oldsum < 1 && SumK > 1)
                {
                    crossedover = true;
                }

                oldsum = SumK;

                if (crossedover)
                {
                    delta = delta / 2D;
                    crossedover = false;
                }
            }
            return double.NaN;
        }

        public static double[] GetKReal(Components comps, Pressure Pb, double[] X, Temperature T)
        {
            int NumComp = comps.Count;
            BaseComp bc;
            double[] EqK = new double[NumComp];
            double t = T._Kelvin;
            double P = Pb.BarA;

            double RT;
            double Pr;//ReducedPressure  ,reducedPressure  squared
            double Term;//int ermediatecalculations
            double[] FLLog = new double[NumComp];//Naturallogofliquidfugacitycoefficient
            double[] FVLog;//Naturallogofvaporfugacitycoefficient
            double[] GamLog = new double[NumComp];//Naturallogofgamma(liquidactivitycoefficient)
            double VolMix;//Volumeofmixture(molar)
            double SolAvg;//Averagesolubilityparameterforsolution

            //CalculateZ(vaporphasecompressibilityfactor)using RedlichandKwongEOS
            FVLog = RK.LnFugMix(comps, P, T, enumFluidRegion.Vapour);

            VolMix = 0;
            SolAvg = 0;
            for (int I = 0; I < NumComp; I++)
            {
                bc = comps[I];
                Term = X[I] * bc.LiquidMolarVolumeCM3();
                VolMix += Term;
                SolAvg += Term * bc.HildebrandSolPar();
            }
            SolAvg /= VolMix;

            RT = 8.3145 * t;//Units?

            for (int I = 0; I < NumComp; I++)
            {
                bc = comps[I];
                Pr = P / bc.CritP.BarA;
                GamLog[I] = bc.LiquidMolarVolumeCM3() / RT * Math.Pow(bc.HildebrandSolPar() - SolAvg, 2D);//Calcsolublityparameters
                Term = GetLiqFug(bc, t, P);
                FLLog[I] = Math.Log(Term);
                EqK[I] = Math.Exp(FLLog[I] + GamLog[I] - FVLog[I]) / Pr;
            }
            return EqK;
        }

        public static double GetLiqFug(BaseComp bc, Temperature T, Pressure P)
        {
            double Tr = T / bc.CritT;
            double Pr = P / bc.CritP;
            double Tr2 = Tr * Tr;
            double Tr3 = Tr2 * Tr;
            double Pr2 = Pr * Pr;
            double Term, res;

            if (bc.MW < 3)//H2
            {
                Term = 1.96718 + 1.02972 / Tr - 0.054009 * Tr + 0.0005288 * Tr2 + 0.008585 * Pr;
            }
            else if (bc.MW > 16 && bc.MW < 16.2)//CH4
            {
                Term = 2.4384 - 2.2455 / Tr - 0.34084 * Tr + 0.00212 * Tr2 - 0.00223 * Tr3 + Pr * (0.10486 - 0.03691 * Tr);
            }
            else
            {
                Term = 5.75748 - 3.01761 / Tr - 4.985 * Tr + 2.02299 * Tr2 + Pr * (0.08427 + 0.26667 * Tr - 0.31138 * Tr2)
                + Pr2 * (-0.02655 + 0.02883 * Tr);//-Math.Log(Pr,10);

                Term += bc.Omega * (-4.23893 + 8.65808 * Tr - 1.2206 / Tr - 3.15224 * Tr3 - 0.025 * (Pr - 0.6));
            }

            res = Term * 2.3025851;//convertbase10tobasee

            res = Math.Exp(res);

            return res;
        }
    }
}