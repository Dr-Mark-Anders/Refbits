using ModelEngine;
using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Units.UOM;

namespace ModelEngine.UnitOperations.RK4New
{
    internal partial class RK4New
    {
        private double Weight = 250, Step = 1, time = 0, moles = 0, DeltaHForm = 0, Cp = 0, deltaT;
        private double[] K1, K2, K3, K4, MoleDeltas = new double[0], StepMoleFlows = new double[0], TempMoleFlows = new double[0];
        private Components temp_cc, comps;
        private Reactions Rs;
        private Temperature InletT = 273.15, CurrentT = 273.15;
        private DeltaTemperature DT1, DT2, DT3, DT4;
        private List<double[]> ComProfile;
        private double MindW = 1, BigResid = 0, MaxResid = 0;
        public double[] FinalMoleFracs;
        private int Segments = 0;
        private double[] RateOfChange;
        private double[] RateOfChange2;

        public RK4New(List<double[]> Profile)
        {
            ComProfile = Profile;
        }

        public bool SolveRK4(Reactions rsx, Pressure P, Temperature T, Port_Material port, MoleFlow moleflow, double StepSize, bool isothermal = false)
        {
            Dictionary<string, int> Index = port.cc.Index;
            Rs = rsx;
            this.InletT = T;
            CurrentT = InletT;
            double CurrentTime = 0;
            double[] MoleFracs = port.cc.MoleFractions;

            int count = port.cc.CountIncSolids;
            comps = port.cc;
            Enthalpy h = port.H;
            port.Flash();

            MoleDeltas = new double[count];
            StepMoleFlows = new double[count];
            TempMoleFlows = new double[count];

            K1 = new double[Rs.Count];
            K2 = new double[Rs.Count];
            K3 = new double[Rs.Count];
            K4 = new double[Rs.Count];

            double[] KFinal = new double[Rs.Count];

            DeltaHForm = comps.Hform25();

            temp_cc = comps.Clone();
            for (int i = 0; i < comps.Count; i++)
                StepMoleFlows[i] = comps[i].MoleFraction * moleflow;

            TempMoleFlows = (double[])StepMoleFlows.Clone();

            Step = Weight / Segments;

            do
            {
                CurrentTime += StepSize;

                Array.Clear(MoleDeltas);
                MoleFracs = TempMoleFlows.Normalise();

                for (int r = 0; r < Rs.Count; r++)
                {
                    Reaction rx = (Reaction)Rs[r];
                    K1[r] = rx.solve(Index, P, T, KFinal);
                    UpdateMoleDeltas(1, StepSize);
                    UpdateTempMoleFlows(StepMoleFlows, MoleDeltas);
                }

                if (!isothermal)
                    DT1.BaseValue = T - UpdateTempTempH(port, port.H_.BaseValue, port.P_.BaseValue, CurrentT);

                Array.Clear(MoleDeltas);

                MoleFracs = TempMoleFlows.Normalise();

                for (int r = 0; r < Rs.Count; r++)
                {
                    Reaction rx = (Reaction)Rs[r];
                    K2[r] = rx.solve(Index, P, T + DT1 / 2, KFinal);
                    UpdateMoleDeltas(2, StepSize);
                    UpdateTempMoleFlows(StepMoleFlows, MoleDeltas);
                }

                if (!isothermal)
                    DT1.BaseValue = T - UpdateTempTempH(port, port.H_.BaseValue, port.P_.BaseValue, Step);

                Array.Clear(MoleDeltas);

                MoleFracs = TempMoleFlows.Normalise();

                for (int r = 0; r < Rs.Count; r++)
                {
                    Reaction rx = (Reaction)Rs[r];
                    K3[r] = rx.solve(Index, P, T + DT2 / 2, KFinal);
                    UpdateMoleDeltas(2, StepSize);
                    UpdateTempMoleFlows(StepMoleFlows, MoleDeltas);
                }

                if (!isothermal)
                    DT1.BaseValue = T - UpdateTempTempH(port, port.H_.BaseValue, port.P_.BaseValue, CurrentT);

                Array.Clear(MoleDeltas);

                MoleFracs = TempMoleFlows.Normalise();

                for (int r = 0; r < Rs.Count; r++)
                {
                    Reaction rx = (Reaction)Rs[r];
                    K4[r] = rx.solve(Index, P, T + DT3, KFinal);
                    UpdateMoleDeltas(1, StepSize);
                    UpdateTempMoleFlows(StepMoleFlows, MoleDeltas);
                }

                if (!isothermal)
                    DT1.BaseValue = T - UpdateTempTempH(port, port.H_.BaseValue, port.P_.BaseValue, CurrentT);

                Temperature TRnew = this.InletT + (DT1 + DT2 * 2 + DT3 * 2 + DT4) / 6.0;

                Array.Clear(MoleDeltas);
                for (int r = 0; r < Rs.Count; r++)
                {
                    Reaction rx = (Reaction)Rs[r];
                    KFinal[r] = Step / 6 * (K1[r] + K2[r] * 2 + K3[r] * 2 + K4[r]);
                    UpdateMoleDeltas(1, StepSize);
                    UpdateTempMoleFlows(StepMoleFlows, MoleDeltas);
                }

                StepMoleFlows = (double[])TempMoleFlows.Clone();

                ComProfile.Add((double[])StepMoleFlows.Clone());

                T = TRnew;
            }
            while (CurrentTime < Segments);

            return true;
        }

        public bool SolveEuler(Port_Material port, Components cc, Pressure P, Temperature T, Enthalpy H, MoleFlow moleflow, Reactions reactions, int Segments, double InitialStepSize, bool isothermal)
        {
            RateOfChange2 = new double[cc.CountIncSolids + 1];
            CurrentT = T;

            comps = cc;
            temp_cc = cc.Clone();
            double StepSize = InitialStepSize;

            Rs = reactions;
            this.InletT = T;

            double[] MoleFracs = cc.MoleFractions;

            int CompCount = MoleFracs.Length;
            MoleDeltas = new double[CompCount];
            StepMoleFlows = new double[CompCount];
            TempMoleFlows = new double[CompCount];

            K1 = new double[Rs.Count];
            double[] KFinal = new double[Rs.Count];

            DeltaHForm = comps.Hform25();

            temp_cc = comps.Clone();

            for (int i = 0; i < comps.Count; i++)
                StepMoleFlows[i] = comps[i].MoleFraction;

            TempMoleFlows = (double[])StepMoleFlows.Clone();

            double CurrentStep = 0;

            comps.UpdateIndex();
            Dictionary<string, int> Index = comps.Index;

            do
            {
                CurrentStep += StepSize;
                Array.Clear(MoleDeltas);

                for (int r = 0; r < Rs.Count; r++)
                {
                    Reaction rx = Rs[r];
                    switch (rx)
                    {
                        case HydroCrackReaction hcr:
                            K1[r] = hcr.solve(Index, P, CurrentT, MoleFracs, reactions.factors);
                            break;

                        case SumProductReaction spr:
                            K1[r] = spr.solve(Index, P, CurrentT, MoleFracs, reactions.factors);
                            break;

                        case Reaction rr:
                            K1[r] = rr.solve(Index, P, CurrentT, MoleFracs, reactions.factors);
                            break;
                    }
                }

                Array.Clear(RateOfChange2);
                for (int r = 0; r < Rs.Count; r++)
                {
                    Reaction rx = Rs[r];
                    UpdateCompRate(rx, Index);
                }

                Temperature TRnew;

                double[] TempMoleFlows = StepMoleFlows;
                do
                {
                    MoleDeltas = UpdateMoleDeltas(1, StepSize);
                    TempMoleFlows = UpdateTempMoleFlows(StepMoleFlows, MoleDeltas);
                    temp_cc.SetMolFractionsIncSolids(TempMoleFlows);

                    if (!isothermal)
                        TRnew = UpdateTempTempH(port, H, P, CurrentT); // Enthalpy does not change, T does
                    else
                        TRnew = 0;
                } while (!CheckResidualsEuler(ref StepSize, StepMoleFlows, RateOfChange2, CurrentT, TRnew));

                StepMoleFlows = TempMoleFlows; // update the step composition

                Debug.Print(TRnew.ToString());

                CurrentT = TRnew;

                StepMoleFlows = (double[])TempMoleFlows.Clone();
                MoleFracs = StepMoleFlows.Normalise();
                ComProfile.Add((double[])StepMoleFlows.Clone());
            } while (CurrentStep < Segments);

            FinalMoleFracs = StepMoleFlows;

            return true;
        }

        public bool CheckResidualsEuler(ref double StepSize, double[] SteoMoleFlows, double[] RateOfChange, Temperature T, Temperature TNew)
        {
            double[] NewMoleFLows = new double[comps.CountIncSolids + 1];

            for (int J = 0; J < comps.CountIncSolids; J++)
                //FNew[J] = F[J] + (K1[J] + 2D * K2[J] + 2D * K3[J] + K4[J]) / 6d;
                NewMoleFLows[J] = SteoMoleFlows[J] + K1[J];

            double[] resid = new double[comps.CountIncSolids + 1];

            for (int J = 0; J < comps.CountIncSolids; J++)
                resid[J] = (NewMoleFLows[J] - SteoMoleFlows[J]) / StepSize - RateOfChange[J];

            resid[comps.CountIncSolids] = (TNew - T) / StepSize - RateOfChange[comps.CountIncSolids];

            // Calcuate Residuals
            for (int J = 0; J <= comps.Count; J++)
            {
                if (Math.Abs(resid[J]) > BigResid)
                {
                    if (Math.Abs(resid[J]) > MaxResid)
                    {
                        if (StepSize > MindW)
                        {
                            StepSize /= 2D;
                            return false;
                        }
                        else // dw < MindW
                        {
                            // Simulation Failed
                        }
                    }

                    BigResid = Math.Abs(resid[J]);
                }
            }
            return true;
        }

        private void UpdateCompRate(Reaction rx, Dictionary<string, int> Index)
        {
            switch (rx)
            {
                case HydroCrackReaction hcr:
                    foreach (ReactionComponent comp in rx.RComps.AllComponents())
                    {
                        int sgn = comp.Stoich;//Math.Sign(comp.Stoich);
                        RateOfChange2[Index[comp.Name]] += rx.Rate * sgn;
                    }

                    for (int i = 0; i < hcr.yieldPattern.Count; i++)
                        if (Thermodata.ShortNames.TryGetValue(hcr.yieldPattern.Names[i], out string name))
                            RateOfChange2[Index[name.ToUpper()]] += rx.Rate * hcr.yieldPattern.Yields[i];
                        else
                            RateOfChange2[Index[hcr.yieldPattern.Names[i]]] += rx.Rate;

                    break;

                case SumProductReaction spr:
                case Reaction r:
                    foreach (ReactionComponent comp in rx.RComps.AllComponents())
                    {
                        int sgn = comp.Stoich;//Math.Sign(comp.Stoich);
                        switch (comp.Type)
                        {
                            case CompType.Normal:
                                RateOfChange2[Index[comp.Name]] += rx.Rate * sgn;
                                break;

                            case CompType.YieldPattern:
                                for (int i = 0; i < comp.Yieldpattern.Count; i++)
                                    RateOfChange2[Index[comp.Yieldpattern.Names[i]]] += rx.Rate * sgn * comp.Yieldpattern.Yields[i];
                                break;

                            case CompType.Mixed:
                                RateOfChange2[Index[comp.Name]] += rx.Rate * sgn;
                                break;

                            case CompType.Short:
                            default:
                                RateOfChange2[Index[comp.Name]] += rx.Rate * sgn;
                                break;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private double[] UpdateTempMoleFlows(double[] MoleFlows, double[] MoleDeltas)
        {
            double[] res = new double[MoleFlows.Length];

            for (int i = 0; i < temp_cc.CountIncSolids; i++)
                res[i] = MoleFlows[i] + MoleDeltas[i];

            return res;
        }

        private Temperature UpdateTempTempH(Port_Material port, Enthalpy H, Pressure P, Temperature T)
        {
            FlashClass.Flash(port, port.Thermo, Guid.Empty);

            return port.T;
        }

        public double[] UpdateMoleDeltas(double RKstep, double StepSize)
        {
            double[] res = new double[RateOfChange2.Length];
            for (int i = 0; i < RateOfChange2.Length - 1; i++)
                res[i] = RateOfChange2[i] * StepSize / RKstep;
            return res;
        }
    }
}