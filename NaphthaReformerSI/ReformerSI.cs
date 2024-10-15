using ModelEngine;
using Extensions;
using RefReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using Units;
using Units.UOM;

namespace NaphthaReformerSI
{
    public partial class NapReformerSI
    {
        public List<double[]> Profile = new();
        public double[][] Profile1;
        public double[][] Profile2;
        public double[][] Profile3;
        public double[][] Profile4;
        public List<double> dWarray1 = new();
        public List<double> dWarray2 = new();
        public List<double> dWarray3 = new();
        public List<double> dWarray4 = new();

        public NapReformerSI()
        {
            D86 = new double[8];

            A1 = new double[NumRxn];
            A2 = new double[NumRxn];
            B1 = new double[NumRxn];
            B2 = new double[NumRxn];
            C9plusFact = new double[7];
            HCDistrib = new double[21, 13];
            StdHeatForm = new double[NumComp];
            MW = new double[NumComp];
            SG = new double[NumComp];
            NBP = new double[NumComp];
            TCritR = new double[NumComp];
            PCritPSIA = new double[NumComp];
            W = new double[NumComp];
            MolVol = new double[NumComp];
            Sol = new double[NumComp];
            RON = new double[NumComp];
            MON = new double[NumComp];
            RVP = new double[NumComp];

            MolFeed = new double[NumComp + 1];
            MassFeed = new MassFlow[NumComp + 1];
            VolFeed = new VolumeFlow[NumComp + 1];

            Profile1 = new double[0][];
            Profile2 = new double[0][];
            Profile3 = new double[0][];
            Profile4 = new double[0][];

            InitConstants();
            initRxData();
        }

        public void ReadInput(IUOM feedrate, List<UOMProperty> RxT, double[] CompFractions, AssayBasis assayBasis,
            DistPoints D86, Density density, Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> PNAO, Pressure Psep, Temperature Tsep,
            List<UOMProperty> CatPct, List<Pressure> RxPi, List<DeltaPressure> RxDP, double LHSV_Input, int NoRx, double H2HC_in,
            InputOption inputOption, double OctSpec = double.NaN)
        {
            double[] FeedInput = new double[NumComp];

            double SumFeedInput;
            this.PSep = Psep;
            this.TSep = Tsep;
            this.assayBasis = assayBasis;
            this.LHSV = LHSV_Input;
            this.H2HC = H2HC_in;
            this.OctSpec = OctSpec;
            this.NumReactor = NoRx;

            ReactorT_In = new Temperature[NumReactor];
            InletP = new Pressure[NumReactor];
            ReactorP = new Pressure[NumReactor];
            CatPercent = new double[NumReactor];
            AmtCat = new Mass[NumReactor];
            MetalAct = new double[NumReactor];
            AcidAct = new double[NumReactor];
            FurnEffic = new double[NumReactor];
            WHSV = new double[NumReactor];

            InitialiseComponents();

            for (int i = 0; i < 4; i++)
                Eff[i] = new double[32];

            NumReactor = NoRx;

            switch (inputOption)
            {

                case InputOption.Full:
                    for (int i = 0; i < CompFractions.Length; i++)
                        FeedInput[i] = CompFractions[i];
                    break;
                case InputOption.Medium:
                    FeedInput[0] = 0;
                    FeedInput[1] = 0;
                    FeedInput[2] = 0;
                    FeedInput[3] = 0;
                    FeedInput[4] = 0;
                    FeedInput[5] = 0;
                    FeedInput[6] = CompFractions[NameDict["C5"]] * 0.442;
                    FeedInput[7] = CompFractions[NameDict["C5"]] - FeedInput[7];
                    FeedInput[8] = CompFractions[NameDict["P6"]] * 0.442;
                    FeedInput[9] = CompFractions[NameDict["P6"]] - FeedInput[9];
                    FeedInput[10] = CompFractions[NameDict["CH7"]];
                    FeedInput[11] = CompFractions[NameDict["MCP"]];
                    FeedInput[12] = CompFractions[NameDict["A6"]];
                    FeedInput[13] = CompFractions[NameDict["P7"]] * 0.5184;
                    FeedInput[14] = CompFractions[NameDict["P7"]] - FeedInput[14];
                    FeedInput[15] = CompFractions[NameDict["N7"]] * 0.6051;
                    FeedInput[16] = CompFractions[NameDict["N7"]] - FeedInput[16];
                    FeedInput[17] = CompFractions[NameDict["A7"]];
                    FeedInput[18] = CompFractions[NameDict["P8"]];
                    FeedInput[19] = CompFractions[NameDict["N8"]] * 0.151;
                    FeedInput[20] = CompFractions[NameDict["N8"]] * 0.45396;
                    FeedInput[21] = CompFractions[NameDict["N8"]] * 0.09868;
                    FeedInput[22] = CompFractions[NameDict["N8"]] - FeedInput[20] - FeedInput[21] - FeedInput[22];
                    FeedInput[23] = CompFractions[NameDict["A8"]] * 0.17015;
                    FeedInput[24] = CompFractions[NameDict["A8"]] * 0.16714;
                    FeedInput[25] = CompFractions[NameDict["A8"]] * 0.43686;
                    FeedInput[26] = CompFractions[NameDict["A8"]] - FeedInput[24] - FeedInput[25] - FeedInput[26];
                    FeedInput[27] = CompFractions[NameDict["P9"]];
                    FeedInput[28] = CompFractions[NameDict["N9"]] * 0.498;
                    FeedInput[29] = CompFractions[NameDict["N9"]] - FeedInput[29];
                    FeedInput[30] = CompFractions[NameDict["A9"]];
                    break;
                case InputOption.Short:
                    P_LV = PNAO.Item1;
                    N_LV = PNAO.Item2;
                    A_LV = PNAO.Item3;
                    if ((P_LV < 0.0) | (P_LV > 100.0) | (N_LV < 0.0) | (N_LV > 100.0) | (A_LV < 0.0) | (A_LV > 100.0))
                        EndWell("For the Short Assay, the percentage of paraffins, naphthenes, and aromatics in the feed must be between  0 and 100.  Check input, then try again.");
                    else
                    {
                        SumFeedInput = P_LV + N_LV + A_LV;
                        if (SumFeedInput < 1E-06)
                            EndWell("For the Short Assay, enter the percentage of paraffins, naphthenes, and aromatics in the feed, then try again.");
                        else if (Math.Abs(SumFeedInput - 100.0) > 0.01)
                        {
                            CheckContinue();
                            P_LV = P_LV / SumFeedInput * 100.0;
                            N_LV = N_LV / SumFeedInput * 100.0;
                            A_LV = A_LV / SumFeedInput * 100.0;
                        }
                    }
                    for (int i = 0; i < 6; i += 3)
                        if (double.IsNaN(D86[i].BP))
                            EndWell("For the Short Assay, the IBP, 50%, and EP Temperatures are required.  Check input, then try again.");

                    SGFeed = density.SG;
                    APIFeed = Convert.ToDouble(SGToAPI(SGFeed));

                    if (SGFeed < 0.1)
                        EndWell("For the Short Assay, feed density must be greater than zero.  Check input, then try again.");

                    ShortCharFeed(P_LV, N_LV, A_LV, D86.GetTemperatures(), FeedInput);

                    break;

                default:
                    {
                        EndWell("Please select short, medium, or full assay from the option buttons, then try again.");
                        break;
                    }
            }

            switch (assayBasis)
            {
                case AssayBasis.Mass:
                    this.Feed.SetMassFractions(FeedInput);
                    this.Feed.NormaliseFractions(FlowFlag.Mass);
                    break;

                case AssayBasis.Molar:
                    this.Feed.SetMolFractions(FeedInput);
                    this.Feed.NormaliseFractions(FlowFlag.Molar);
                    break;

                case AssayBasis.Volume:
                    this.Feed.SetVolFractions(FeedInput);
                    Feed.NormaliseFractions(FlowFlag.LiqVol);
                    break;

                default:
                    break;
            }

            Feed_MW = Feed.MW();
            Feed_SG = Feed.SG_Calc();

            switch (feedrate)
            {
                case MassFlow mf:
                    this.NaphMassFeed = mf;
                    this.NaphVolFeed.BaseValue = this.NaphMassFeed / density;
                    this.NaphMolFeed.BaseValue = mf / Feed_MW;
                    break;

                case VolumeFlow vf:
                    this.NaphVolFeed = vf;
                    this.NaphMassFeed.BaseValue = this.NaphVolFeed * density;
                    this.NaphMolFeed.BaseValue = vf / Feed_MW;
                    break;

                case MoleFlow molflow:
                    this.NaphMolFeed = molflow;
                    this.NaphMassFeed.BaseValue = this.NaphMolFeed / Feed_MW;
                    this.NaphVolFeed.BaseValue = this.NaphMassFeed / Feed_SG;
                    break;
            }

            for (int i = 0; i < NumComp; i++)
            {
                MolFeed[i] = NaphMolFeed * Feed[i].MoleFraction;
                MassFeed[i] = NaphMassFeed * Feed[i].MassFraction;
                VolFeed[i] = NaphVolFeed * Feed[i].STDLiqVolFraction;
            }

            /*
                        //  Temporary to match Excel Verison
                        double [] VBAMolFeed = new  double []{double.NaN, 0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0100,0.0200,4.5300,5.3300,5.1300,17.2100,
                        19.3400,10.1000,8.4400,17.8400,62.6200,3.8200,16.2600,0.6300,10.5300,2.6300,1.5100,6.3400,3.3700,231.9300,57.1400,56.5900,43.4800 };

                        for (int  i = 1; i < NumComp; i++)
                        {
                            MolFeed[i].kgMole_hr = VBAMolFeed[i];
                        }*/

            if (!double.IsNaN(OctSpec))
            {
                if ((OctSpec < 50.0) | (OctSpec > 140.0))
                    EndWell("Octane specification is outside range.  Check input, then try again.");
            }

            if (FeedInput.Min() < 0.0)
                EndWell("Feed assay percent must be non-negative.  Check input and try again.");

            SumFeedInput = FeedInput.Sum();

            if ((Math.Abs(SumFeedInput - 100.0) > 0.01) && inputOption == InputOption.Short)
                CheckContinue();

            if (NaphMassFeed <= 0.0)
                EndWell("Feed rate must be greater than zero. Check input and try again.");

            CharFeed();

            if (H2HC <= 0.0)
                EndWell("Hydrogen to hydrocarbon ratio must be positive.   Check H2HC value and try again.");
            else
                MolH2Recy = H2HC * NaphMolFeed;

            if (DHCH > 0.0)
                DHCHFact = DHCH;
            else
                DHCHFact = 2.75;

            if (DHCP > 0.0)
                DHCPFact = DHCP;
            else
                DHCPFact = 2.75;

            if (ISOM > 0.0)
                ISOMFact = ISOM;
            else
                ISOMFact = 2.75;

            if (OPEN > 0.0)
                OPENFact = OPEN;
            else
                OPENFact = 2.75;

            if (PHC > 0.0)
                PHCFact = PHC;
            else
                PHCFact = 2.75;

            if (NHC > 0.0)
                NHCFact = NHC;
            else
                NHCFact = 2.75;

            if (HDA > 0.0)
                HDAFact = HDA;
            else
                HDAFact = 2.75;

            for (int i = 0; i < NumReactor; i++)
            {
                ReactorT_In[i] = RxT[i].BaseValue; // kelvin
                InletP[i] = RxPi[i];
                DP[i] = RxDP[i];
                ReactorP[i] = InletP[i] - DP[i] / 2.0;
            }

            MetalActiv = 0.0;
            AcidActiv = 0.0;
            AmtCat1 = 20.0;

            MetalAct[0] = MetalActiv;
            AcidAct[0] = AcidActiv;
            CatPercent[0] = 20;

            MetalAct[1] = MetalActiv;
            AcidAct[1] = AcidActiv;
            CatPercent[1] = 30;

            MetalAct[2] = MetalActiv;
            AcidAct[2] = AcidActiv;
            CatPercent[2] = 50;

            double SumCatPercent = 0.0;

            for (int i = 0; i < NumReactor; i++)
            {
                if (CatPercent[i] <= 0.0)
                    EndWell("Percentage of catalyst in each reactor (for selected number of reactors) must be greater than zero. Check input and try again.");
                else
                    SumCatPercent += CatPercent[i];
            }

            if (unchecked(SumCatPercent < 99.5 || SumCatPercent > 100.5))
                EndWell("For the selected number of reactors, the sum of percent catalyst loading should equal 100. Check reactor catalyst loading and try again.");
            else if (Math.Abs(SumCatPercent - 100.0) >= 0.01)
            {
                for (int i = 0; i <= NumReactor; i++)
                    CatPercent[i] = CatPercent[i] / SumCatPercent * 100.0;

                SumCatPercent = 100.0;
            }

            for (int i = 0; i < NumReactor; i++)
            {
                if ((InletP[i] <= 0.0) | (ReactorT_In[i] <= 0.0))
                    EndWell("For each reactor modeled, enter the catalyst loading, inlet temperature, inlet pressure, and pressure drop across the catalyst bed.  Check input for reactor " + Convert.ToString(i) + ", then try again.");
            }

            for (int i = 0; i < NumReactor; i++)
            {
                if (MetalAct[i] <= 0.0)
                    MetalAct[i] = 1.0;
                if (AcidAct[i] <= 0.0)
                    AcidAct[i] = 1.0;
            }

            double CatDensity = 37.7;
            double TotCatAmt = default;

            switch (catunit)
            {
                case CatalystUnits.LBS:
                    TotCatAmt = 0.0;
                    if (TotCatAmt <= 0.0)
                        EndWell("Total catalyst amount must be greater than zero. Check input and try again.");

                    LHSV = NaphVolFeed * 0.233940973 / TotCatAmt * CatDensity;
                    break;

                case CatalystUnits.kg:
                    TotCatAmt = Convert.ToDouble(KgToLbs(0.0));
                    if (TotCatAmt <= 0.0)
                        EndWell("Total catalyst amount must be greater than zero. Check input and try again.");

                    LHSV = NaphVolFeed * 0.233940973 / TotCatAmt * CatDensity;
                    break;

                case CatalystUnits.LHSV:
                    if (LHSV <= 0.0)
                        EndWell("LHSV (liquid hourly space volume) must be greater than zero. Check input and try again.");

                    TotCatAmt = NaphVolFeed.BPD * 0.233940973 / LHSV * CatDensity;
                    break;

                default:
                    EndWell("Choose valid units for catalyst amount (Lbs, kg, or LHSV) from drop-down menu, then try again.");
                    break;
            }

            for (int I = 0; I < NumReactor; I++)
                AmtCat[I] = CatPercent[I] / SumCatPercent * TotCatAmt;

            TotWHSV = 0.0;

            for (int I = 0; I < NumReactor; I++)
            {
                WHSV[I] = NaphVolFeed / TotCatAmt;
                TotWHSV += 1.0 / WHSV[I];
            }
            TotWHSV = 1.0 / TotWHSV;

            if (PSep <= 0.0)
                EndWell("Separator pressure must be greater than zero. Check input and try again.");

            FurnEffic[0] = 82.0;
            FurnEffic[1] = 82.0;
            FurnEffic[2] = 82.0;

            for (int i = 0; i < NumReactor; i++)
                if ((FurnEffic[i] > 100.0) | (FurnEffic[i] <= 1.0))
                    EndWell("Please enter furnace efficiency as a percentage (between 1 and 100).  Check input and try again. If the cell is left blank, the default value of 82% will be used.");
        }

        private void CharFeed()
        {
            P_LV = (VolFeed[1] + VolFeed[2] + VolFeed[3] + VolFeed[4] + VolFeed[5] + VolFeed[6]
                + VolFeed[7] + VolFeed[8] + VolFeed[9] + VolFeed[10] + VolFeed[14] + VolFeed[15] + VolFeed[19] + VolFeed[28])
                / NaphVolFeed * 100.0;
            P_WT = (MassFeed[1] + MassFeed[2] + MassFeed[3] + MassFeed[4] + MassFeed[5] + MassFeed[6]
                + MassFeed[7] + MassFeed[8] + MassFeed[9] + MassFeed[10] + MassFeed[14] + MassFeed[15] + MassFeed[19] + MassFeed[28]) / NaphMassFeed * 100.0;
            N_LV = (VolFeed[11] + VolFeed[12] + VolFeed[16] + VolFeed[17] + VolFeed[20] + VolFeed[21] + VolFeed[22] + VolFeed[23] + VolFeed[29] + VolFeed[30])
                / NaphVolFeed * 100.0;
            N_WT = (MassFeed[11] + MassFeed[12] + MassFeed[16] + MassFeed[17] + MassFeed[20] + MassFeed[21] + MassFeed[22] + MassFeed[23] + MassFeed[29] + MassFeed[30])
                / NaphMassFeed * 100.0;
            A_LV = (VolFeed[13] + VolFeed[18] + VolFeed[24] + VolFeed[25] + VolFeed[26] + VolFeed[27] + VolFeed[31])
                / NaphVolFeed * 100.0;
            A_WT = (MassFeed[13] + MassFeed[18] + MassFeed[24] + MassFeed[25] + MassFeed[26] + MassFeed[27] + MassFeed[31])
                / NaphMassFeed * 100.0;
            SGFeed.BaseValue = NaphMassFeed / NaphVolFeed;
            APIFeed = 141.5 / SGFeed.SG - 131.5;
        }

        public bool SolveCase()
        {
            double OctTolerance = 0.025;
            double OctHigh = OctSpec + OctTolerance;
            double OctLow = OctSpec - OctTolerance;
            double ChangeBy = 8.0;
            bool BeginBinary = false;
            bool NotFirstGuess = false;
            int MaxIterations = 200;
            double Tolerance = 0.02;
            bool Solved = false;
            int CountIter = 1;

            Profile.Clear();
            Profile1 = new double[NumComp][];
            Profile2 = new double[NumComp][];
            Profile3 = new double[NumComp][];
            Profile4 = new double[NumComp][];

            EstRecycle(F_RecyStar);

            if (!Reactors(F_RecyStar, F_Inlet, out F_Eff, ref ReactorT_Out))
                return false;

            double FracLiq = default;
            Flash(F_Eff, TSep, PSep, ref FracLiq, ref F_Ref, ref F_Vap);
            AssignRecycle(F_Vap, F_NetGas, F_RecyNew);

            F_RecyStarOld = F_RecyStar;
            F_RecyStar = F_RecyNew;
            F_Recy = F_RecyNew;

            double TempRON = default;
            double TempMON = default;
            bool LastGuessHigh = default;
            double OldTemp1 = default;

            for (int J = 0; J < MaxIterations; J++)
            {
                CountIter++;
                if (!Reactors(F_RecyStar, F_Inlet, out F_Eff, ref ReactorT_Out))
                    return false;

                if (!Flash(F_Eff, TSep, PSep, ref FracLiq, ref F_Ref, ref F_Vap))
                    return false;

                if (!AssignRecycle(F_Vap, F_NetGas, F_RecyNew))
                    return false;

                for (int i = 0; i < NumComp; i++)
                {
                    double Denom = F_RecyNew[i] + F_RecyStarOld[i] - F_Recy[i] - F_RecyStar[i];
                    if (Denom > 1E-07)
                        F_RecyStarNew[i] = (F_RecyNew[i] * F_RecyStarOld[i] - F_Recy[i] * F_RecyStar[i]) / Denom;
                    else
                        F_RecyStarNew[i] = F_RecyStar[i];
                }

                double SumError = 0.0;

                for (int i = 0; i < NumComp; i++)
                    SumError += Math.Abs(F_RecyStar[i] - F_RecyStarOld[i]);

                F_RecyStarOld = F_RecyStar;
                F_RecyStar = F_RecyStarNew;
                F_Recy = F_RecyNew;

                if (SumError < Tolerance)
                    break;

                if (!double.IsNaN(OctSpec))
                {
                    CalcOctane(F_Ref, ref TempRON, ref TempMON);

                    if (Math.Abs(TempRON - OctSpec) <= Tolerance)
                    {
                        NotFirstGuess = false;
                        break;
                    }
                    if (NotFirstGuess)
                    {
                        if (unchecked((TempRON >= OctHigh && !LastGuessHigh) || (TempRON <= OctLow && LastGuessHigh)))
                            BeginBinary = true;
                        if (BeginBinary && ChangeBy > 0.5)
                            ChangeBy /= 2.0;
                    }
                    else
                        NotFirstGuess = true;

                    double OldTemp2 = OldTemp1;
                    OldTemp1 = ReactorT_In[1];
                    if (TempRON >= OctHigh)
                    {
                        LastGuessHigh = true;

                        for (int K = 0; K < NumReactor; K++)
                            ReactorT_In[K] -= ChangeBy;

                        if (ReactorT_In[1] == OldTemp2)
                        {
                            ChangeBy /= 2.0;

                            for (int K = 0; K < NumReactor; K++)
                                ReactorT_In[K] += ChangeBy;
                        }
                    }
                    else
                    {
                        if (!(TempRON <= OctLow))
                            continue;

                        LastGuessHigh = false;

                        for (int K = 0; K < NumReactor; K++)
                            ReactorT_In[K] += ChangeBy;

                        if (ReactorT_In[1] == OldTemp2)
                        {
                            ChangeBy /= 2.0;
                            for (int K = 1; K <= NumReactor; K++)
                                ReactorT_In[K] -= ChangeBy;
                        }
                    }
                }
            }

            for (int J = 0; J <= 400; J++)
            {
                CountIter++;
                double SumError = 0.0;

                for (int i = 0; i < NumComp; i++)
                    SumError += Math.Abs(F_RecyStar[i] - F_Recy[i]);

                if (SumError < Tolerance)
                {
                    if (SpecOct)
                    {
                        CalcOctane(F_Ref, ref TempRON, ref TempMON);
                        if (Math.Abs(TempRON - OctSpec) <= Tolerance)
                        {
                            Solved = true;
                            break;
                        }
                        else if (NotFirstGuess)
                        {
                            if ((TempRON >= OctHigh && !LastGuessHigh) || (TempRON <= OctLow && LastGuessHigh))
                            {
                                if (ChangeBy > 0.5)
                                    ChangeBy /= 2.0;
                            }
                            else
                                NotFirstGuess = true;
                        }

                        if (TempRON >= OctHigh)
                        {
                            LastGuessHigh = true;
                            int num6 = Convert.ToInt32(NumReactor);
                            for (int K = 1; K <= num6; K++)
                                ReactorT_In[K] -= ChangeBy;
                        }
                        else if (TempRON <= OctLow)
                        {
                            LastGuessHigh = false;
                            int num7 = Convert.ToInt32(NumReactor);
                            for (int K = 1; K <= num7; K++)
                                ReactorT_In[K] += ChangeBy;
                        }
                    }

                    else
                    {
                        Solved = true;
                        break;
                    }
                }
                else
                {
                    for (int i = 0; i < NumComp; i++)
                        F_RecyStar[i] = F_Recy[i];
                }

                Reactors(F_RecyStar, F_Inlet, out F_Eff, ref ReactorT_Out);
                Flash(F_Eff, TSep, PSep, ref FracLiq, ref F_Ref, ref F_Vap);
                AssignRecycle(F_Vap, F_NetGas, F_Recy);
            }

            if (Solved)
            {
                Reactors(F_Recy, F_Inlet, out F_Eff, ref ReactorT_Out, true);
                Flash(F_Eff, TSep, PSep, ref FracLiq, ref F_Ref, ref F_Vap);
                AssignRecycle(F_Vap, F_NetGas, F_RecyNew);
            }
            else
            {
                string Message = ((!SpecOct) ? "Recycle calculation failed to converge. Simulation abandoned." :
                    Convert.ToString("Octane specification failed to converge.  The last iteration used a first reactor inlet temperature of " + RToF(ReactorT_In[1])) + " °F.  Try starting with an inlet temperature closer to that value.");
                EndWell(Message);
            }

            for (int i = 0; i < NumReactor; i++)
                Furnace(ReactorT_Out[i], ReactorT_In[i], Eff[i], ref Duty[i]);

            OrganizeResults(F_Ref);
            CalcOctane(F_Ref, ref SumRON, ref SumMON);
            Reformate = F_Ref;
            offgas = F_NetGas;

            return true;
        }


        private bool Reactors(MoleFlow[] F_RecyStar, double[,] F_Inlet, out double[] F_Eff, ref Temperature[] ReactorT_Out, bool LastCalc = false)
        {
            double[] RxFeedMoleFlow = new double[NumComp];
            Temperature RxT;

            for (int J = 0; J < NumReactor; J++)
            {
                if (J == 0)
                {
                    for (int i = 0; i < NumComp; i++)
                    {
                        F_Inlet[i, J] = MolFeed[i] + F_RecyStar[i];
                        RxFeedMoleFlow[i] = F_Inlet[i, J];
                    }
                }
                else
                    for (int i = 0; i < NumComp; i++)
                        F_Inlet[i, J] = RxFeedMoleFlow[i];

                InitRateFactors(ReactorP[J], MetalAct[J], AcidAct[J]);

                RxT = ReactorT_In[J];

                if (RungeKutta(ref RxT, ReactorP[J], AmtCat[J], RxFeedMoleFlow, LastCalc, J))
                {
                    ReactorT_Out[J] = RxT;
                    Eff[J] = RxFeedMoleFlow;
                }
                else
                {
                    F_Eff=new double[0];
                    return false;
                }
            }
            F_Eff = RxFeedMoleFlow;
            return true;
        }

        private void InitRateFactors(Pressure ReactorP, double MetalAct, double AcidAct)
        {
            double IsomPExp = 0.37;
            double CyclPExp = -0.7;
            double CrackPExp = 0.53;
            double DealkPExp = 0.5;
            CHDehydrogTerm = MetalAct * DHCHFact / NaphMolFeed.lbMole_hr;
            CPDehydrogTerm = MetalAct * DHCPFact / NaphMolFeed.lbMole_hr;
            IsomTerm = AcidAct * Math.Pow(ReactorP.ATMA, IsomPExp) * ISOMFact / NaphMolFeed.lbMole_hr;
            CyclTerm = AcidAct * Math.Pow(ReactorP.ATMA, CyclPExp) * OPENFact / NaphMolFeed.lbMole_hr;
            PCrackTerm = AcidAct * Math.Pow(ReactorP.ATMA, CrackPExp) * PHCFact / NaphMolFeed.lbMole_hr;
            DealkTerm = AcidAct * Math.Pow(ReactorP.ATMA, DealkPExp) * HDAFact / NaphMolFeed.lbMole_hr;
            NCrackTerm = AcidAct * Math.Pow(ReactorP.ATMA, CrackPExp) * NHCFact / NaphMolFeed.lbMole_hr;
        }

        private bool RungeKutta(ref Temperature TR, Pressure ReactorP, double AmtCat, double[] FeedMoleFlow, bool FinalLoop, int RxNo)
        {
            double[] FInter = new double[32];
            double[] FNew = new double[32];
            double[] k1 = new double[32];
            double[] k2 = new double[32];
            double[] k3 = new double[32];
            double[] k4 = new double[32];
            double[] resid = new double[32];
            double[] dFdW = new double[32];
            int i = 25;
            double MaxN = 10000.0;
            double MaxdW = AmtCat / (double)i;
            double MindW = AmtCat / MaxN;
            double dW = MaxdW;
            double MaxResid = 0.05;
            double BigResid = 0.0;
            double Weight = 0.0;
            Temperature TRNew;
            Temperature TRInter;
            NapRefBed testbed = new();
            bool OldMethod = true;
            //bool  OldMethod = true;
            int LoopCount = 0;

            if (FinalLoop)
            {
                Profile.Clear();
                Profile.Add((double[])FeedMoleFlow.Clone());
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

            while (Weight < AmtCat)
            {
                LoopCount++;
            whileloop:

                if (OldMethod)
                    CalcRates(TR, ReactorP, FeedMoleFlow, dFdW);  // rate in lbmoles
                else
                    //var res = testbed.Solve(TR, ReactorP, FeedMoleFlow, MetalAct[0], AcidAct[0], NaphMolFeed);
                    dFdW = testbed.Solve(TR, ReactorP, FeedMoleFlow, MetalAct[0], AcidAct[0], NaphMolFeed);

                for (int J = 0; J < NumDepVar; J++)
                    k1[J] = dW * dFdW[J];

                for (int J = 0; J < NumComp; J++)
                    FInter[J] = FeedMoleFlow[J] + k1[J] / 2.0;

                TRInter = TR + k1[NumComp] / 2.0;

                if (OldMethod)
                    CalcRates(TRInter, ReactorP, FInter, dFdW);
                else
                    dFdW = testbed.Solve(TRInter, ReactorP, FInter, MetalAct[0], AcidAct[0], NaphMolFeed);

                for (int J = 0; J < NumDepVar; J++)
                    k2[J] = dW * dFdW[J];

                for (int J = 0; J < NumComp; J++)
                    FInter[J] = FeedMoleFlow[J] + k2[J] / 2.0;

                TRInter = TR + k2[NumComp] / 2.0;

                if (OldMethod)
                    CalcRates(TRInter, ReactorP, FInter, dFdW);
                else
                    dFdW = testbed.Solve(TRInter, ReactorP, FInter, MetalAct[0], AcidAct[0], NaphMolFeed);

                for (int J = 0; J < NumDepVar; J++)
                    k3[J] = dW * dFdW[J];

                for (int J = 0; J < NumComp; J++)
                    FInter[J] = FeedMoleFlow[J] + k3[J];

                TRInter = TR + k3[NumComp];

                if (OldMethod)
                    CalcRates(TRInter, ReactorP, FInter, dFdW);
                else
                    dFdW = testbed.Solve(TRInter, ReactorP, FInter, MetalAct[0], AcidAct[0], NaphMolFeed);

                for (int J = 0; J < NumDepVar; J++)
                    k4[J] = dW * dFdW[J];

                for (int J = 0; J < NumComp; J++)
                    FNew[J] = FeedMoleFlow[J] + (k1[J] + 2.0 * k2[J] + 2.0 * k3[J] + k4[J]) / 6.0;

                TRNew = TR + (k1[NumComp] + 2.0 * k2[NumComp] + 2.0 * k3[NumComp] + k4[NumComp]) / 6.0;

                if (OldMethod)
                    CalcRates(TRNew, ReactorP, FNew, dFdW);
                else
                    dFdW = testbed.Solve(TRNew, ReactorP, FNew, MetalAct[0], AcidAct[0], NaphMolFeed);

                for (int J = 0; J < NumComp; J++)
                    resid[J] = (FNew[J] - FeedMoleFlow[J]) / dW - dFdW[J];

                resid[NumComp] = (TRNew - TR) / dW - dFdW[NumComp];

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
                                EndWell("Simulation abandoned due to failure to converge");
                                return false;
                            }
                        }
                        BigResid = resid[J];
                    }
                }

                for (int I = 0; I < NumComp; I++)
                    FeedMoleFlow[I] = FNew[I];

                TR = TRNew;
                Weight += dW;

                if (FinalLoop)
                {
                    switch (RxNo)
                    {
                        case 0:
                            Profile.Add((double[])FNew.Clone());
                            dWarray1.Add(Weight / AmtCat);
                            break;

                        case 1:
                            Profile.Add((double[])FNew.Clone());
                            dWarray2.Add(Weight / AmtCat);
                            break;

                        case 2:
                            Profile.Add((double[])FNew.Clone());
                            dWarray3.Add(Weight / AmtCat);
                            break;

                        case 3:
                            Profile.Add((double[])FNew.Clone());
                            dWarray4.Add(Weight / AmtCat);
                            break;
                    }
                }
            }

            /* for (int  count = 0; count < Profile.Count; count++)
             {
                 Profile[count].Normalise();
             }*/

            switch (RxNo)
            {
                case 0:
                    for (int comp = 0; comp < NumComp; comp++)
                    {
                        Profile1[comp] = new double[Profile.Count];
                        for (int j = 0; j < Profile.Count; j++)
                            Profile1[comp][j] = Profile[j][comp];
                    }
                    break;

                case 1:
                    for (int comp = 0; comp < NumComp; comp++)
                    {
                        Profile2[comp] = new double[Profile.Count];
                        for (int j = 0; j < Profile.Count; j++)
                            Profile2[comp][j] = Profile[j][comp];
                    }
                    break;

                case 2:
                    for (int comp = 0; comp < NumComp; comp++)
                    {
                        Profile3[comp] = new double[Profile.Count];
                        for (int j = 0; j < Profile.Count; j++)
                            Profile3[comp][j] = Profile[j][comp];
                    }
                    break;

                case 3:
                    for (int comp = 0; comp < NumComp; comp++)
                    {
                        Profile4[comp] = new double[Profile.Count];
                        for (int j = 0; j < Profile.Count; j++)
                            Profile4[comp][j] = Profile[j][comp];
                    }
                    break;
            }

            //MessageBox.Show(LoopCount.ToString());
            return true;
        }

        private void CalcRates(Temperature TR, Pressure PR, double[] MoleFlowRate, double[] RateOfChange)
        {
            double[] RateK = new double[81];
            double[] EquilK = new double[81];
            double[] RxnRate = new double[81];
            double TempTotal = 0.0;
            double TRankine = TR.Rankine;

            for (int i = 0; i < NumComp; i++)
                TempTotal += MoleFlowRate[i];

            double YH2 = MoleFlowRate[0] / TempTotal;
            double YC1 = MoleFlowRate[1] / NaphMolFeed;
            // double  YC2 = MoleFlowRate[2] / NaphMolFeed;
            // double  YC3 = MoleFlowRate[3] / NaphMolFeed;
            double YIC4 = MoleFlowRate[4] / NaphMolFeed;
            double YNC4 = MoleFlowRate[5] / NaphMolFeed;
            double YIC5 = MoleFlowRate[6] / NaphMolFeed;
            double YNC5 = MoleFlowRate[7] / NaphMolFeed;
            double YIC6 = MoleFlowRate[8] / NaphMolFeed;
            double YNC6 = MoleFlowRate[9] / NaphMolFeed;
            double YCH = MoleFlowRate[10] / NaphMolFeed;
            double YMCP = MoleFlowRate[11] / NaphMolFeed;
            double YBEN = MoleFlowRate[12] / NaphMolFeed;
            double YIC7 = MoleFlowRate[13] / NaphMolFeed;
            double YNC7 = MoleFlowRate[14] / NaphMolFeed;
            double YMCH = MoleFlowRate[15] / NaphMolFeed;
            double YC7CP = MoleFlowRate[16] / NaphMolFeed;
            double YTOL = MoleFlowRate[17] / NaphMolFeed;
            double YC8P = MoleFlowRate[18] / NaphMolFeed;
            double YECH = MoleFlowRate[19] / NaphMolFeed;
            double YDMCH = MoleFlowRate[20] / NaphMolFeed;
            double YPCP = MoleFlowRate[21] / NaphMolFeed;
            double YC8CP = MoleFlowRate[22] / NaphMolFeed;
            double YEB = MoleFlowRate[23] / NaphMolFeed;
            double YPX = MoleFlowRate[24] / NaphMolFeed;
            double YMX = MoleFlowRate[25] / NaphMolFeed;
            double YOX = MoleFlowRate[26] / NaphMolFeed;
            double YC9P = MoleFlowRate[27] / NaphMolFeed;
            double YC9CH = MoleFlowRate[28] / NaphMolFeed;
            double YC9CP = MoleFlowRate[29] / NaphMolFeed;
            double YC9A = MoleFlowRate[30] / NaphMolFeed;

            for (int i = 0; i < NumRxn; i++)
            {
                RateK[i] = Math.Exp(A1[i] + A2[i] / TRankine);
                EquilK[i] = Math.Exp(B1[i] + B2[i] / TRankine);
            }

            double YXylTot = YPX + YMX + YOX;
            double YC8ATot = YEB + YXylTot;

            double H2Term = Math.Pow(YH2 * PR.ATMA, 3.0);

            RxnRate[0] = RateK[0] * CHDehydrogTerm * (YCH - H2Term * YBEN / EquilK[0]);
            RxnRate[1] = RateK[1] * CPDehydrogTerm * YMCP;
            RxnRate[2] = RateK[2] * CHDehydrogTerm * (YMCH - H2Term * YTOL / EquilK[2]);
            RxnRate[3] = RateK[3] * CPDehydrogTerm * (YC7CP - H2Term * YTOL / EquilK[3]);
            RxnRate[4] = RateK[4] * CHDehydrogTerm * (YECH - H2Term * YEB / EquilK[4]);
            RxnRate[5] = RateK[5] * CHDehydrogTerm * (YDMCH - H2Term * YXylTot / EquilK[5]);
            RxnRate[6] = RateK[6] * CPDehydrogTerm * (YPCP - H2Term * YEB / EquilK[6]);
            RxnRate[7] = RateK[7] * CPDehydrogTerm * (YC8CP - H2Term * YXylTot / EquilK[7]);
            RxnRate[8] = RateK[8] * CHDehydrogTerm * (YC9CH - H2Term * YC9A / EquilK[8]);
            RxnRate[9] = RateK[9] * CPDehydrogTerm * (YC9CP - H2Term * YC9A / EquilK[9]);

            RxnRate[12] = RateK[12] * IsomTerm * (YOX - YPX / EquilK[12]);
            RxnRate[13] = RateK[13] * IsomTerm * (YPX - YMX / EquilK[13]);
            RxnRate[14] = RateK[14] * IsomTerm * (YMX - YOX / EquilK[14]);
            RxnRate[16] = RateK[16] * IsomTerm * (YCH - YMCP / EquilK[16]);
            RxnRate[17] = RateK[17] * IsomTerm * (YMCH - YC7CP / EquilK[17]);
            RxnRate[18] = RateK[18] * IsomTerm * (YECH - YPCP / EquilK[18]);
            RxnRate[19] = RateK[19] * IsomTerm * (YPCP - YC8CP / EquilK[19]);
            RxnRate[20] = RateK[20] * IsomTerm * (YC8CP - YDMCH / EquilK[20]);
            RxnRate[21] = RateK[21] * IsomTerm * (YDMCH - YECH / EquilK[21]);
            RxnRate[22] = RateK[22] * IsomTerm * (YC9CH - YC9CP / EquilK[22]);
            RxnRate[23] = RateK[23] * IsomTerm * (YNC7 - YIC7 / EquilK[23]);
            RxnRate[24] = RateK[24] * IsomTerm * (YNC6 - YIC6 / EquilK[24]);
            RxnRate[25] = RateK[25] * IsomTerm * (YNC5 - YIC5 / EquilK[25]);
            RxnRate[26] = RateK[26] * IsomTerm * (YNC4 - YIC4 / EquilK[26]);

            H2Term = YH2 * PR.ATMA;
            RxnRate[29] = RateK[29] * CyclTerm * (YCH * H2Term - YNC6 / EquilK[29]);
            RxnRate[30] = RateK[30] * CyclTerm * (YMCP * H2Term - YNC6 / EquilK[30]);
            RxnRate[31] = RateK[31] * CyclTerm * (YMCP * H2Term - YIC6 / EquilK[31]);
            RxnRate[32] = RateK[32] * CyclTerm * (YMCH * H2Term - YNC7 / EquilK[32]);
            RxnRate[33] = RateK[33] * CyclTerm * (YMCH * H2Term - YIC7 / EquilK[33]);
            RxnRate[34] = RateK[34] * CyclTerm * (YC7CP * H2Term - YNC7 / EquilK[34]);
            RxnRate[35] = RateK[35] * CyclTerm * (YC7CP * H2Term - YIC7 / EquilK[35]);
            RxnRate[36] = RateK[36] * CyclTerm * (YECH * H2Term - YC8P / EquilK[36]);
            RxnRate[37] = RateK[37] * CyclTerm * (YDMCH * H2Term - YC8P / EquilK[37]);
            RxnRate[38] = RateK[38] * CyclTerm * (YPCP * H2Term - YC8P / EquilK[38]);
            RxnRate[39] = RateK[39] * CyclTerm * (YC8CP * H2Term - YC8P / EquilK[39]);
            RxnRate[40] = RateK[40] * CyclTerm * (YC9CH * H2Term - YC9P / EquilK[40]);
            RxnRate[41] = RateK[41] * CyclTerm * (YC9CP * H2Term - YC9P / EquilK[41]);
            RxnRate[44] = RateK[44] * PCrackTerm * YC9P;
            RxnRate[45] = RateK[45] * PCrackTerm * YC8P;
            RxnRate[46] = RateK[46] * PCrackTerm * YIC7;
            RxnRate[47] = RateK[47] * PCrackTerm * YNC7;
            RxnRate[48] = RateK[48] * PCrackTerm * YIC6;
            RxnRate[49] = RateK[49] * PCrackTerm * YNC6;
            RxnRate[50] = RateK[50] * PCrackTerm * YIC5;
            RxnRate[51] = RateK[51] * PCrackTerm * YNC5;
            RxnRate[52] = RateK[52] * PCrackTerm * YIC4;
            RxnRate[53] = RateK[53] * PCrackTerm * YNC4;
            RxnRate[56] = RateK[56] * DealkTerm * (YMCH * YH2 - YCH * YC1 / EquilK[56]);
            RxnRate[57] = RateK[57] * DealkTerm * (YC7CP * YH2 - YMCP * YC1 / EquilK[57]);
            RxnRate[58] = RateK[58] * DealkTerm * (YECH * YH2 - YMCH * YC1 / EquilK[58]);
            RxnRate[59] = RateK[59] * DealkTerm * (YDMCH * YH2 - YMCH * YC1 / EquilK[59]);
            RxnRate[60] = RateK[60] * DealkTerm * (YPCP * YH2 - YC7CP * YC1 / EquilK[60]);
            RxnRate[61] = RateK[61] * DealkTerm * (YC8CP * YH2 - YC7CP * YC1 / EquilK[61]);
            RxnRate[62] = RateK[62] * DealkTerm * (YC9CH * YH2 - YECH * YC1 / EquilK[62]);
            RxnRate[63] = RateK[63] * DealkTerm * (YC9CH * YH2 - YDMCH * YC1 / EquilK[63]);
            RxnRate[64] = RateK[64] * DealkTerm * (YC9CP * YH2 - YPCP * YC1 / EquilK[64]);
            RxnRate[65] = RateK[65] * DealkTerm * (YC9CP * YH2 - YC8CP * YC1 / EquilK[65]);
            RxnRate[66] = RateK[66] * DealkTerm * (YTOL * YH2 - YBEN * YC1 / EquilK[66]);
            RxnRate[67] = RateK[67] * DealkTerm * (YC8ATot * YH2 - YTOL * YC1 / EquilK[67]);
            RxnRate[68] = RateK[68] * DealkTerm * (YC9A * YH2 - YC8ATot * YC1 / EquilK[68]);
            RxnRate[70] = RateK[70] * NCrackTerm * YC9CH;
            RxnRate[71] = RateK[71] * NCrackTerm * YC9CP;
            RxnRate[72] = RateK[72] * NCrackTerm * YECH;
            RxnRate[73] = RateK[73] * NCrackTerm * YDMCH;
            RxnRate[74] = RateK[74] * NCrackTerm * YPCP;
            RxnRate[75] = RateK[75] * NCrackTerm * YC8CP;
            RxnRate[76] = RateK[76] * NCrackTerm * YMCH;
            RxnRate[77] = RateK[77] * NCrackTerm * YC7CP;
            RxnRate[78] = RateK[78] * NCrackTerm * YCH;
            RxnRate[79] = RateK[79] * NCrackTerm * YMCP;

            RateOfChange[0] = 3.0 * (RxnRate[0] + RxnRate[1] + RxnRate[2] + RxnRate[3] + RxnRate[4] + RxnRate[5] + RxnRate[6] + RxnRate[7] + RxnRate[8] + RxnRate[9]);
            RateOfChange[0] -= RxnRate[29] + RxnRate[30] + RxnRate[31] + RxnRate[32] + RxnRate[33] + RxnRate[34] + RxnRate[35] + RxnRate[36] + RxnRate[37] + RxnRate[38];
            RateOfChange[0] -= RxnRate[39] + RxnRate[40] + RxnRate[41] + RxnRate[44] + RxnRate[45] + RxnRate[46] + RxnRate[47] + RxnRate[48] + RxnRate[49] + RxnRate[50] + RxnRate[51] + RxnRate[52] + RxnRate[53] + RxnRate[56] + RxnRate[57];
            RateOfChange[0] -= RxnRate[58] + RxnRate[59] + RxnRate[60] + RxnRate[61] + RxnRate[62] + RxnRate[63] + RxnRate[64] + RxnRate[65] + RxnRate[66] + RxnRate[67] + RxnRate[68];
            RateOfChange[0] -= 2.0 * (RxnRate[70] + RxnRate[71] + RxnRate[72] + RxnRate[73] + RxnRate[74] + RxnRate[75] + RxnRate[76] + RxnRate[77] + RxnRate[78] + RxnRate[79]);
            RateOfChange[1] = RxnRate[56] + RxnRate[57] + RxnRate[58] + RxnRate[59] + RxnRate[60] + RxnRate[61] + RxnRate[62] + RxnRate[63]
                + RxnRate[64] + RxnRate[65] + RxnRate[66] + RxnRate[67] + RxnRate[68] + CrackProd(0, RxnRate);
            RateOfChange[2] = CrackProd(1, RxnRate);
            RateOfChange[3] = CrackProd(2, RxnRate);
            RateOfChange[4] = CrackProd(3, RxnRate) + RxnRate[26] - RxnRate[52];
            RateOfChange[5] = CrackProd(4, RxnRate) - RxnRate[26] - RxnRate[53];
            RateOfChange[6] = CrackProd(5, RxnRate) + RxnRate[25] - RxnRate[50];
            RateOfChange[7] = CrackProd(6, RxnRate) - RxnRate[25] - RxnRate[51];
            RateOfChange[8] = CrackProd(7, RxnRate) + RxnRate[24] + RxnRate[31] - RxnRate[48];
            RateOfChange[9] = CrackProd(8, RxnRate) - RxnRate[24] + RxnRate[29] + RxnRate[30] - RxnRate[49];
            RateOfChange[10] = RxnRate[56] - RxnRate[29] - RxnRate[16] - RxnRate[0] - RxnRate[78];
            RateOfChange[11] = RxnRate[57] - RxnRate[30] - RxnRate[31] + RxnRate[16] - RxnRate[1] - RxnRate[79];
            RateOfChange[12] = RxnRate[0] + RxnRate[1] + RxnRate[66];
            RateOfChange[13] = CrackProd(9, RxnRate) + RxnRate[23] + RxnRate[33] + RxnRate[35] - RxnRate[46];
            RateOfChange[14] = CrackProd(10, RxnRate) - RxnRate[23] + RxnRate[32] + RxnRate[34] - RxnRate[47];
            RateOfChange[15] = RxnRate[58] + RxnRate[59] - RxnRate[56] - RxnRate[32] - RxnRate[33] - RxnRate[17] - RxnRate[2] - RxnRate[76];
            RateOfChange[16] = RxnRate[60] + RxnRate[61] - RxnRate[57] - RxnRate[34] - RxnRate[35] + RxnRate[17] - RxnRate[3] - RxnRate[77];
            RateOfChange[17] = RxnRate[67] - RxnRate[66] + RxnRate[2] + RxnRate[3];
            RateOfChange[18] = CrackProd(11, RxnRate) + RxnRate[36] + RxnRate[37] + RxnRate[38] + RxnRate[39] - RxnRate[45];
            RateOfChange[19] = RxnRate[62] - RxnRate[58] - RxnRate[36] + RxnRate[21] - RxnRate[18] - RxnRate[4] - RxnRate[72];
            RateOfChange[20] = RxnRate[63] - RxnRate[59] - RxnRate[37] + RxnRate[20] - RxnRate[21] - RxnRate[5] - RxnRate[73];
            RateOfChange[21] = RxnRate[64] - RxnRate[60] - RxnRate[38] + RxnRate[18] - RxnRate[19] - RxnRate[6] - RxnRate[74];
            RateOfChange[22] = RxnRate[65] - RxnRate[61] - RxnRate[39] + RxnRate[19] - RxnRate[20] - RxnRate[7] - RxnRate[75];
            RateOfChange[23] = RxnRate[4] + RxnRate[6] + 0.096 * (RxnRate[68] - RxnRate[67]); // EB
            RateOfChange[24] = RxnRate[12] - RxnRate[13] + 0.232 * (RxnRate[5] + RxnRate[7]) + 0.209 * (RxnRate[68] - RxnRate[67]); //PX
            RateOfChange[25] = RxnRate[13] - RxnRate[14] + 0.395 * (RxnRate[5] + RxnRate[7]) + 0.35 * (RxnRate[68] - RxnRate[67]); //MX
            RateOfChange[26] = RxnRate[14] - RxnRate[12] + 0.373 * (RxnRate[5] + RxnRate[7]) + 0.345 * (RxnRate[68] - RxnRate[67]); //OX
            RateOfChange[27] = RxnRate[40] + RxnRate[41] - RxnRate[44];
            RateOfChange[28] = 0.0 - RxnRate[8] - RxnRate[22] - RxnRate[40] - RxnRate[62] - RxnRate[63] - RxnRate[70];
            RateOfChange[29] = 0.0 - RxnRate[9] + RxnRate[22] - RxnRate[41] - RxnRate[64] - RxnRate[65] - RxnRate[71];
            RateOfChange[30] = RxnRate[8] + RxnRate[9] - RxnRate[68];

            for (int i = 0; i < NumComp; i++)
                RateOfChange[i] *= NaphMolFeed.lbMole_hr;

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
                SumCP += KgToLbs(MoleFlowRate[i]) * (CpCoeft[i][0] + CpCoeft[i][1] * T1 + CpCoeft[i][2] * T2 + CpCoeft[i][3] * T3);
            }

            double DelTHB = SumHF / SumCP;
            RateOfChange[NumDepVar - 1] = -DelTHB / 1.8;
        }
    }
}