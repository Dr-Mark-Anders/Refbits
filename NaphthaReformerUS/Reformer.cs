using EngineThermo;
using System;
using System.Linq;
using Units.UOM;

namespace NaphthaReformerUS
{
    public partial class NapReformerUS
    {
        public NapReformerUS()
        {
            D86 = new double[8];
            ReactorT_In = new Temperature[5];
            InletP = new Pressure[5];
            ReactorP = new Pressure[5];
            CatPercent = new double[5];
            AmtCat = new Mass[5];
            MetalAct = new double[5];
            AcidAct = new double[5];
            FurnEffic = new double[4];
            A1 = new double[81];
            A2 = new double[81];
            B1 = new double[81];
            B2 = new double[81];
            C9plusFact = new double[7];
            HCDistrib = new double[21, 13];
            StdHeatForm = new double[32];
            MW = new double[43];
            SG = new double[43];
            NBP = new double[32];
            TCritR = new double[32];
            PCritPSIA = new double[32];
            W = new double[32];
            MolVol = new double[32];
            Sol = new double[32];
            RON = new double[32];
            MON = new double[32];
            RVP = new double[32];

            MolFeed = new MoleFlow[NumComp + 1];
            MassFeed = new MassFlow[NumComp + 1];
            VolFeed = new VolFlow[NumComp + 1];

            InitConstants();
            initRxData();
        }

        public void ReadInput(object feedrate, Temperature[] RxT, double[] CompFractions, AssayBasis assayBasis,
            DistPoints D86, Density density, Tuple<double, double, double, double> PNAO, Pressure Psep, Temperature Tsep,
            double[] CatPct, Pressure[] RxPi, DeltaPressure[] RxDP, double LHSV_Input, int NoRx, double H2HC_in, 
            InputOption inputOption, double OctSpec = double.NaN)
        {
            double[] FeedInput = new double[32];
            double[] WHSV = new double[5];
            double SumFeedInput;
            this.PSep = Psep;
            this.TSep = Tsep;
            this.assayBasis = assayBasis;
            this.LHSV = LHSV_Input;
            this.H2HC = H2HC_in;
            this.OctSpec = OctSpec;

            InitialiseComponents();

            for (int i = 0; i < 4; i++)
                Eff[i] = new MoleFlow[32];

            NumReactor = NoRx;

            switch (inputOption)
            {
                case InputOption.Short:
                    P_LV = PNAO.Item1;
                    N_LV = PNAO.Item2;
                    A_LV = PNAO.Item3;
                    if ((P_LV < 0.0) || (P_LV > 100.0) || (N_LV < 0.0) || (N_LV > 100.0) || (A_LV < 0.0) || (A_LV > 100.0))
                        EndWell("For the Short Assay, the percentage of paraffins, naphthenes, and aromatics in the feed must be between  0 and 100.  Check input, then try again.");
                    else
                    {
                        SumFeedInput = P_LV + N_LV + A_LV;
                        if (SumFeedInput < 1E-06)
                            EndWell("For the Short Assay, enter the percentage of paraffins, naphthenes, and aromatics in the feed, then try again.");
                        else if (Math.Abs(SumFeedInput - 100.0) > 0.01)
                        {

                            P_LV = P_LV / SumFeedInput * 100.0;
                            N_LV = N_LV / SumFeedInput * 100.0;
                            A_LV = A_LV / SumFeedInput * 100.0;
                        }
                    }
                    for (int i = 1; i < 7; i += 3)
                        if (double.IsNaN(D86[i].BP))
                            EndWell("For the Short Assay, the IBP, 50%, and EP temperatures are required.  Check input, then try again.");

                    SGFeed = density;
                    APIFeed = Convert.ToDouble(SGToAPI(density));

                    if (SGFeed < 0.1)
                        EndWell("For the Short Assay, feed density must be greater than zero.  Check input, then try again.");

                    ShortCharFeed(P_LV, N_LV, A_LV, D86.getBPs(), FeedInput);

                    break;

                case InputOption.Medium:
                    FeedInput[1] = 0;
                    FeedInput[2] = 0;
                    FeedInput[3] = 0;
                    FeedInput[4] = 0;
                    FeedInput[5] = 0;
                    FeedInput[6] = 0;
                    FeedInput[7] = CompFractions[NameDict["C5"]] * 0.442;
                    FeedInput[8] = CompFractions[NameDict["C5"]] - FeedInput[7];
                    FeedInput[9] = CompFractions[NameDict["P6"]] * 0.442;
                    FeedInput[10] = CompFractions[NameDict["P6"]] - FeedInput[9];
                    FeedInput[11] = CompFractions[NameDict["CH7"]];
                    FeedInput[12] = CompFractions[NameDict["MCP"]];
                    FeedInput[13] = CompFractions[NameDict["A6"]];
                    FeedInput[14] = CompFractions[NameDict["P7"]] * 0.5184;
                    FeedInput[15] = CompFractions[NameDict["P7"]] - FeedInput[14];
                    FeedInput[16] = CompFractions[NameDict["N7"]] * 0.6051;
                    FeedInput[17] = CompFractions[NameDict["N7"]] - FeedInput[16];
                    FeedInput[18] = CompFractions[NameDict["A7"]];
                    FeedInput[19] = CompFractions[NameDict["P8"]];
                    FeedInput[20] = CompFractions[NameDict["N8"]] * 0.151;
                    FeedInput[21] = CompFractions[NameDict["N8"]] * 0.45396;
                    FeedInput[22] = CompFractions[NameDict["N8"]] * 0.09868;
                    FeedInput[23] = CompFractions[NameDict["N8"]] - FeedInput[20] - FeedInput[21] - FeedInput[22];
                    FeedInput[24] = CompFractions[NameDict["A8"]] * 0.17015;
                    FeedInput[25] = CompFractions[NameDict["A8"]] * 0.16714;
                    FeedInput[26] = CompFractions[NameDict["A8"]] * 0.43686;
                    FeedInput[27] = CompFractions[NameDict["A8"]] - FeedInput[24] - FeedInput[25] - FeedInput[26];
                    FeedInput[28] = CompFractions[NameDict["P9"]];
                    FeedInput[29] = CompFractions[NameDict["N9"]] * 0.498;
                    FeedInput[30] = CompFractions[NameDict["N9"]] - FeedInput[29];
                    FeedInput[31] = CompFractions[NameDict["A9"]];
                    break;

                case InputOption.Full:
                    for (int i = 0; i < CompFractions.Length; i++)
                        FeedInput[i] = CompFractions[i];
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
                    this.Feed.UpdateComponentFractions(FlowFlag.Mass);
                    break;
                case AssayBasis.Molar:
                    this.Feed.SetMolFractions(FeedInput);
                    this.Feed.UpdateComponentFractions(FlowFlag.Molar);
                    break;
                case AssayBasis.Volume:
                    this.Feed.SetVolFractions(FeedInput);
                    Feed.UpdateComponentFractions(FlowFlag.LiqVol);
                    break;
                default:
                    break;
            }

            Feed_MW = Feed.GetTotalMW();
            Feed_SG = Feed.SG_Calc();

            switch (feedrate)
            {
                case MassFlow mf:
                    this.NaphMassFeed = mf;
                    this.NaphVolFeed.BaseValue = this.NaphMassFeed / density;
                    this.NaphMolFeed.BaseValue = mf / Feed_MW;
                    break;
                case VolFlow vf:
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

            for (int i = 1; i <= NumComp; i++)
            {
                MolFeed[i] = NaphMolFeed * Feed[i].MoleFraction;
                MassFeed[i] = NaphMassFeed * Feed[i].MassFraction;
                VolFeed[i] = NaphVolFeed * Feed[i].LiqVolFraction;
            }

            if (!double.IsNaN(OctSpec) && ((OctSpec < 50.0) || (OctSpec > 140.0)))
                EndWell("Octane specification is outside range.  Check input, then try again.");

            if (FeedInput.Min() < 0.0)
                EndWell("Feed assay percent must be non-negative.  Check input and try again.");

            SumFeedInput = FeedInput.Sum();

            if ((Math.Abs(SumFeedInput - 100.0) > 0.01) && inputOption == InputOption.Short)
                CheckContinue("Check feed input.  Sum of naphtha assay does not equal 100%.  Program will continue after normalizing to 100%.");

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

            InitialTemp = RxT[0];
            InitialTemp2 = RxT[1];
            InitialTemp3 = RxT[2];
            InitialTemp4 = RxT[3];

            ReactorT_In[1] = InitialTemp;
            ReactorT_In[2] = InitialTemp2;
            ReactorT_In[3] = InitialTemp3;
            ReactorT_In[4] = InitialTemp4;

            InletP[1] = RxPi[0];
            InletP[2] = RxPi[1];
            InletP[3] = RxPi[2];
            InletP[4] = RxPi[3];

            DP = RxDP[0];
            DP2 = RxDP[1];
            DP3 = RxDP[2];
            DP4 = RxDP[0];

            ReactorP[1] = InletP[1] - DP / 2.0;
            ReactorP[2] = InletP[2] - DP2 / 2.0;
            ReactorP[3] = InletP[3] - DP3 / 2.0;
            ReactorP[4] = InletP[4] - DP4 / 2.0;

            MetalActiv = 0.0;
            AcidActiv = 0.0;
            AmtCat1 = 20.0;

            MetalActiv2 = 0.0;
            AcidActiv2 = 0.0;
            AmtCat2 = 30.0;

            MetalActiv3 = 0.0;
            AcidActiv3 = 0.0;
            AmtCat3 = 50.0;

            MetalActiv4 = 0.0;
            AcidActiv4 = 0.0;
            AmtCat4 = 0.0;

            MetalAct[1] = MetalActiv;
            AcidAct[1] = AcidActiv;
            CatPercent[1] = AmtCat1;

            MetalAct[2] = MetalActiv2;
            AcidAct[2] = AcidActiv2;
            CatPercent[2] = AmtCat2;

            MetalAct[3] = MetalActiv3;
            AcidAct[3] = AcidActiv3;
            CatPercent[3] = AmtCat3;

            MetalAct[4] = MetalActiv4;
            AcidAct[4] = AcidActiv4;
            CatPercent[4] = AmtCat4;

            double SumCatPercent = 0.0;

            for (int i = 1; i <= NumReactor; i++)
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
                for (int i = 1; i <= NumReactor; i++)
                    CatPercent[i] = CatPercent[i] / SumCatPercent * 100.0;

                SumCatPercent = 100.0;
            }

            for (int i = 1; i <= NumReactor; i++)
            {
                if ((InletP[i] <= 0.0) || (ReactorT_In[i] <= 0.0))
                    EndWell("For each reactor modeled, enter the catalyst loading, inlet temperature, inlet pressure, and pressure drop across the catalyst bed.  Check input for reactor " + Convert.ToString(i) + ", then try again.");
            }

            for (int i = 1; i <= 4; i++)
            {
                if (MetalAct[i] <= 0.0)
                    MetalAct[i] = 1.0;
                if (AcidAct[i] <= 0.0)
                    AcidAct[i] = 1.0;
            }

            double CatDensity = 37.7;
            double TotCatAmt = default(double);

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

            for (int I = 1; I <= NumReactor; I++)
                AmtCat[I] = CatPercent[I] / SumCatPercent * TotCatAmt;

            TotWHSV = 0.0;

            for (int I = 1; I <= NumReactor; I++)
            {
                WHSV[I] = NaphVolFeed / TotCatAmt;
                TotWHSV += 1.0 / WHSV[I];
            }
            TotWHSV = 1.0 / TotWHSV;

            if (PSep <= 0.0)
                EndWell("Separator pressure must be greater than zero. Check input and try again.");

            FurnEffic[1] = 82.0;
            FurnEffic[2] = 82.0;
            FurnEffic[3] = 82.0;

            for (int i = 1; i < 3; i++)
                if ((FurnEffic[i] > 100.0) || (FurnEffic[i] <= 1.0))
                    EndWell("Please enter furnace efficiency as a percentage (between 1 and 100).  Check input and try again. If the cell is left blank, the default value of 82% will be used.");
        }

        private void CheckContinue(string v)
        {
            System.Windows.Forms.MessageBox.Show(v);
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

            EstRecycle(F_RecyStar);

            if (!Reactors(F_RecyStar, F_Inlet, out F_Eff, ref ReactorT_Out))
                return false;

            double FracLiq = default(double);
            Flash(F_Eff, TSep, PSep, ref FracLiq, ref F_Ref, ref F_Vap);
            AssignRecycle(F_Vap, F_NetGas, F_RecyNew);

            F_RecyStarOld = F_RecyStar;
            F_RecyStar = F_RecyNew;
            F_Recy = F_RecyNew;

            double TempRON = default(double);
            double TempMON = default(double);
            bool LastGuessHigh = default(bool);
            double OldTemp1 = default(double);

            for (int J = 1; J <= MaxIterations; J++)
            {
                CountIter++;
                if (!Reactors(F_RecyStar, F_Inlet, out F_Eff, ref ReactorT_Out))
                    return false;

                if (!Flash(F_Eff, TSep, PSep, ref FracLiq, ref F_Ref, ref F_Vap))
                    return false;

                if (!AssignRecycle(F_Vap, F_NetGas, F_RecyNew))
                    return false;

                for (int i = 1; i <= 31; i++)
                {
                    double Denom = F_RecyNew[i] + F_RecyStarOld[i] - F_Recy[i] - F_RecyStar[i];
                    if (Denom > 1E-07)
                        F_RecyStarNew[i] = (F_RecyNew[i] * F_RecyStarOld[i] - F_Recy[i] * F_RecyStar[i]) / Denom;
                    else
                        F_RecyStarNew[i] = F_RecyStar[i];
                }

                F_RecyStarOld = F_RecyStar;
                F_RecyStar = F_RecyStarNew;
                F_Recy = F_RecyNew;

                double SumError = 0.0;

                for (int i = 1; i <= NumComp; i++)
                    SumError += Math.Abs(F_RecyStar[i] - F_RecyStarOld[i]);

                if (SumError >= Tolerance)
                    continue;

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

                        for (int K = 1; K <= NumReactor; K++)
                            ReactorT_In[K] -= ChangeBy;

                        if (ReactorT_In[1] == OldTemp2)
                        {
                            ChangeBy /= 2.0;

                            for (int K = 1; K <= NumReactor; K++)
                                ReactorT_In[K] += ChangeBy;
                        }
                    }
                    else
                    {
                        if (TempRON > OctLow)
                            continue;

                        LastGuessHigh = false;

                        for (int K = 1; K <= NumReactor; K++)
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

                for (int i = 0; i < 32; i++)
                    SumError += Math.Abs(F_RecyStar[i] - F_Recy[i]);

                if (SumError < Tolerance)
                {
                    if (!SpecOct)
                    {
                        Solved = true;
                        break;
                    }

                    CalcOctane(F_Ref, ref TempRON, ref TempMON);

                    if (Math.Abs(TempRON - OctSpec) <= Tolerance)
                    {
                        Solved = true;
                        break;
                    }

                    if (NotFirstGuess)
                        if (unchecked((TempRON >= OctHigh && !LastGuessHigh) || (TempRON <= OctLow && LastGuessHigh)) && ChangeBy > 0.5)
                            ChangeBy /= 2.0;
                        else
                            NotFirstGuess = true;

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
                    for (int i = 0; i < 32; i++)
                        F_RecyStar[i] = F_Recy[i];

                Reactors(F_RecyStar, F_Inlet, out F_Eff, ref ReactorT_Out);
                Flash(F_Eff, TSep, PSep, ref FracLiq, ref F_Ref, ref F_Vap);
                AssignRecycle(F_Vap, F_NetGas, F_Recy);
            }

            if (Solved)
            {
                Reactors(F_Recy, F_Inlet, out F_Eff, ref ReactorT_Out);
                Flash(F_Eff, TSep, PSep, ref FracLiq, ref F_Ref, ref F_Vap);
                AssignRecycle(F_Vap, F_NetGas, F_RecyNew);
            }
            else
            {
                string Message = ((!SpecOct) ? "Recycle calculation failed to converge. Simulation abandoned." :
                    Convert.ToString("Octane specification failed to converge.  The last iteration used a first reactor inlet temperature of " + RToF(ReactorT_In[1])) + " °F.  Try starting with an inlet temperature closer to that value.");
                EndWell(Message);
            }

            for (int i = 1; i <= NumReactor; i++)
                Furnace(ReactorT_Out[i - 1], ReactorT_In[i], Eff[i], ref Duty[i - 1]);
            

            OrganizeResults(F_Ref);
            CalcOctane(F_Ref, ref SumRON, ref SumMON);
            Reformate = F_Ref;
            offgas = F_NetGas;

            return true;
        }

        private bool Reactors(MoleFlow[] F_RecyStar, MoleFlow[,] F_Inlet, out MoleFlow[]? F_Eff, ref Temperature[] ReactorT_Out)
        {
            MoleFlow[] RxFeedMoleFlow = new MoleFlow[32];
            Temperature RxT;

            for (int J = 1; J <= NumReactor; J++)
            {
                if (J == 1)
                {
                    for (int i = 1; i <= 31; i++)
                    {
                        F_Inlet[i, J] = MolFeed[i] + F_RecyStar[i];
                        RxFeedMoleFlow[i] = F_Inlet[i, J];
                    }
                }
                else
                    for (int i = 1; i <= 31; i++)
                        F_Inlet[i, J] = RxFeedMoleFlow[i];

                InitRateFactors(ReactorP[J], MetalAct[J], AcidAct[J]);

                RxT = ReactorT_In[J];

                if (RungeKutta(ref RxT, ReactorP[J], AmtCat[J], RxFeedMoleFlow))
                {
                    ReactorT_Out[J] = RxT;
                    Eff[J] = RxFeedMoleFlow;
                }
                else
                {
                    F_Eff = null;
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

        private bool RungeKutta(ref Temperature TR, Pressure ReactorP, double AmtCat, MoleFlow[] FeedMoleFLow)
        {
            MoleFlow[] FInter = new MoleFlow[32];
            MoleFlow[] FNew = new MoleFlow[32];
            double[] k1 = new double[33];
            double[] k2 = new double[33];
            double[] k3 = new double[33];
            double[] k4 = new double[33];
            double[] resid = new double[33];
            double[] dFdW = new double[33];
            int i = 25;
            double MaxN = 10000.0;
            double MaxdW = AmtCat / (double)i;
            double MindW = AmtCat / MaxN;
            double dW = MaxdW;
            double MaxResid = 0.05;
            double BigResid = 0.0;
            double Weight = 0.0;
            Temperature TRNew = new Temperature();
            Temperature TRInter = new Temperature();

            while (Weight < AmtCat)
            {
            whileloop:

                CalcRates(TR, ReactorP, FeedMoleFLow, dFdW);  // rate in lbmoles

                for (int J = 1; J <= NumDepVar; J++)
                    k1[J] = dW * dFdW[J];

                for (int J = 1; J <= NumComp; J++)
                    FInter[J].lbMole_hr = FeedMoleFLow[J].lbMole_hr + k1[J] / 2.0;

                TRInter.Rankine = TR.Rankine + k1[NumDepVar] / 2.0;

                CalcRates(TRInter, ReactorP, FInter, dFdW);

                for (int J = 1; J <= NumDepVar; J++)
                    k2[J] = dW * dFdW[J];

                for (int J = 1; J <= NumComp; J++)
                    FInter[J].lbMole_hr = FeedMoleFLow[J].lbMole_hr + k2[J] / 2.0;

                TRInter.Rankine = TR.Rankine + k2[NumDepVar] / 2.0;

                CalcRates(TRInter, ReactorP, FInter, dFdW);

                for (int J = 1; J <= NumDepVar; J++)
                    k3[J] = dW * dFdW[J];

                for (int J = 1; J <= NumComp; J++)
                    FInter[J].lbMole_hr = FeedMoleFLow[J].lbMole_hr + k3[J];

                TRInter.Rankine = TR.Rankine + k3[NumDepVar];

                CalcRates(TRInter, ReactorP, FInter, dFdW);

                for (int J = 1; J <= NumDepVar; J++)
                    k4[J] = dW * dFdW[J];

                for (int J = 1; J <= NumComp; J++)
                    FNew[J].lbMole_hr = FeedMoleFLow[J].lbMole_hr + (k1[J] + 2.0 * k2[J] + 2.0 * k3[J] + k4[J]) / 6.0;

                TRNew.Rankine = TR.Rankine + (k1[32] + 2.0 * k2[32] + 2.0 * k3[32] + k4[32]) / 6.0;

                CalcRates(TRNew, ReactorP, FNew, dFdW);

                for (int J = 1; J <= NumComp; J++)
                    resid[J] = (FNew[J].lbMole_hr - FeedMoleFLow[J].lbMole_hr) / dW - dFdW[J];

                resid[NumDepVar] = (TRNew.Rankine - TR.Rankine) / dW - dFdW[NumDepVar];

                for (int J = 1; J <= NumDepVar; J++)
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

                for (int I = 1; I <= NumComp; I++)
                    FeedMoleFLow[I] = FNew[I];

                TR = TRNew;
                Weight += dW;
            }

            return true;
        }

        private void CalcRates(Temperature TR, Pressure PR, MoleFlow[] MoleFlowRate, double[] RateOfChange )
        {
            double[] RateK = new double[81];
            double[] EquilK = new double[81];
            double[] RxnRate = new double[81];
            double TempTotal = 0.0;
            double TRankine = TR.Rankine;

            for (int i = 1; i <= NumComp; i++)
                TempTotal += MoleFlowRate[i];

            double YH2 = MoleFlowRate[1] / TempTotal;
            double YC1 = MoleFlowRate[2] / NaphMolFeed;
            double YC2 = MoleFlowRate[3] / NaphMolFeed;
            double YC3 = MoleFlowRate[4] / NaphMolFeed;
            double YIC4 = MoleFlowRate[5] / NaphMolFeed;
            double YNC4 = MoleFlowRate[6] / NaphMolFeed;
            double YIC5 = MoleFlowRate[7] / NaphMolFeed;
            double YNC5 = MoleFlowRate[8] / NaphMolFeed;
            double YIC6 = MoleFlowRate[9] / NaphMolFeed;
            double YNC6 = MoleFlowRate[10] / NaphMolFeed;
            double YCH = MoleFlowRate[11] / NaphMolFeed;
            double YMCP = MoleFlowRate[12] / NaphMolFeed;
            double YBEN = MoleFlowRate[13] / NaphMolFeed;
            double YIC7 = MoleFlowRate[14] / NaphMolFeed;
            double YNC7 = MoleFlowRate[15] / NaphMolFeed;
            double YMCH = MoleFlowRate[16] / NaphMolFeed;
            double YC7CP = MoleFlowRate[17] / NaphMolFeed;
            double YTOL = MoleFlowRate[18] / NaphMolFeed;
            double YC8P = MoleFlowRate[19] / NaphMolFeed;
            double YECH = MoleFlowRate[20] / NaphMolFeed;
            double YDMCH = MoleFlowRate[21] / NaphMolFeed;
            double YPCP = MoleFlowRate[22] / NaphMolFeed;
            double YC8CP = MoleFlowRate[23] / NaphMolFeed;
            double YEB = MoleFlowRate[24] / NaphMolFeed;
            double YPX = MoleFlowRate[25] / NaphMolFeed;
            double YMX = MoleFlowRate[26] / NaphMolFeed;
            double YOX = MoleFlowRate[27] / NaphMolFeed;
            double YC9P = MoleFlowRate[28] / NaphMolFeed;
            double YC9CH = MoleFlowRate[29] / NaphMolFeed;
            double YC9CP = MoleFlowRate[30] / NaphMolFeed;
            double YC9A = MoleFlowRate[31] / NaphMolFeed;

            for (int i = 1; i <= NumRxn; i++)
            {
                RateK[i] = Math.Exp(A1[i] + A2[i] / TRankine);
                EquilK[i] = Math.Exp(B1[i] + B2[i] / TRankine);
            }

            double YXylTot = YPX + YMX + YOX;
            double YC8ATot = YEB + YXylTot;

            double H2Term = Math.Pow(YH2 * PR.ATMA, 3.0);

            RxnRate[1] = RateK[1] * CHDehydrogTerm * (YCH - H2Term * YBEN / EquilK[1]);
            RxnRate[2] = RateK[2] * CPDehydrogTerm * YMCP;
            RxnRate[3] = RateK[3] * CHDehydrogTerm * (YMCH - H2Term * YTOL / EquilK[3]);
            RxnRate[4] = RateK[4] * CPDehydrogTerm * (YC7CP - H2Term * YTOL / EquilK[4]);
            RxnRate[5] = RateK[5] * CHDehydrogTerm * (YECH - H2Term * YEB / EquilK[5]);
            RxnRate[6] = RateK[6] * CHDehydrogTerm * (YDMCH - H2Term * YXylTot / EquilK[6]);
            RxnRate[7] = RateK[7] * CPDehydrogTerm * (YPCP - H2Term * YEB / EquilK[7]);
            RxnRate[8] = RateK[8] * CPDehydrogTerm * (YC8CP - H2Term * YXylTot / EquilK[8]);
            RxnRate[9] = RateK[9] * CHDehydrogTerm * (YC9CH - H2Term * YC9A / EquilK[9]);
            RxnRate[10] = RateK[10] * CPDehydrogTerm * (YC9CP - H2Term * YC9A / EquilK[10]);
            RxnRate[13] = RateK[13] * IsomTerm * (YOX - YPX / EquilK[13]);
            RxnRate[14] = RateK[14] * IsomTerm * (YPX - YMX / EquilK[14]);
            RxnRate[15] = RateK[15] * IsomTerm * (YMX - YOX / EquilK[15]);
            RxnRate[17] = RateK[17] * IsomTerm * (YCH - YMCP / EquilK[17]);
            RxnRate[18] = RateK[18] * IsomTerm * (YMCH - YC7CP / EquilK[18]);
            RxnRate[19] = RateK[19] * IsomTerm * (YECH - YPCP / EquilK[19]);
            RxnRate[20] = RateK[20] * IsomTerm * (YPCP - YC8CP / EquilK[20]);
            RxnRate[21] = RateK[21] * IsomTerm * (YC8CP - YDMCH / EquilK[21]);
            RxnRate[22] = RateK[22] * IsomTerm * (YDMCH - YECH / EquilK[22]);
            RxnRate[23] = RateK[23] * IsomTerm * (YC9CH - YC9CP / EquilK[23]);
            RxnRate[24] = RateK[24] * IsomTerm * (YNC7 - YIC7 / EquilK[24]);
            RxnRate[25] = RateK[25] * IsomTerm * (YNC6 - YIC6 / EquilK[25]);
            RxnRate[26] = RateK[26] * IsomTerm * (YNC5 - YIC5 / EquilK[26]);
            RxnRate[27] = RateK[27] * IsomTerm * (YNC4 - YIC4 / EquilK[27]);
            H2Term = YH2 * PR.ATMA;
            RxnRate[30] = RateK[30] * CyclTerm * (YCH * H2Term - YNC6 / EquilK[30]);
            RxnRate[31] = RateK[31] * CyclTerm * (YMCP * H2Term - YNC6 / EquilK[31]);
            RxnRate[32] = RateK[32] * CyclTerm * (YMCP * H2Term - YIC6 / EquilK[32]);
            RxnRate[33] = RateK[33] * CyclTerm * (YMCH * H2Term - YNC7 / EquilK[33]);
            RxnRate[34] = RateK[34] * CyclTerm * (YMCH * H2Term - YIC7 / EquilK[34]);
            RxnRate[35] = RateK[35] * CyclTerm * (YC7CP * H2Term - YNC7 / EquilK[35]);
            RxnRate[36] = RateK[36] * CyclTerm * (YC7CP * H2Term - YIC7 / EquilK[36]);
            RxnRate[37] = RateK[37] * CyclTerm * (YECH * H2Term - YC8P / EquilK[37]);
            RxnRate[38] = RateK[38] * CyclTerm * (YDMCH * H2Term - YC8P / EquilK[38]);
            RxnRate[39] = RateK[39] * CyclTerm * (YPCP * H2Term - YC8P / EquilK[39]);
            RxnRate[40] = RateK[40] * CyclTerm * (YC8CP * H2Term - YC8P / EquilK[40]);
            RxnRate[41] = RateK[41] * CyclTerm * (YC9CH * H2Term - YC9P / EquilK[41]);
            RxnRate[42] = RateK[42] * CyclTerm * (YC9CP * H2Term - YC9P / EquilK[42]);
            RxnRate[45] = RateK[45] * PCrackTerm * YC9P;
            RxnRate[46] = RateK[46] * PCrackTerm * YC8P;
            RxnRate[47] = RateK[47] * PCrackTerm * YIC7;
            RxnRate[48] = RateK[48] * PCrackTerm * YNC7;
            RxnRate[49] = RateK[49] * PCrackTerm * YIC6;
            RxnRate[50] = RateK[50] * PCrackTerm * YNC6;
            RxnRate[51] = RateK[51] * PCrackTerm * YIC5;
            RxnRate[52] = RateK[52] * PCrackTerm * YNC5;
            RxnRate[53] = RateK[53] * PCrackTerm * YIC4;
            RxnRate[54] = RateK[54] * PCrackTerm * YNC4;
            RxnRate[57] = RateK[57] * DealkTerm * (YMCH * YH2 - YCH * YC1 / EquilK[57]);
            RxnRate[58] = RateK[58] * DealkTerm * (YC7CP * YH2 - YMCP * YC1 / EquilK[58]);
            RxnRate[59] = RateK[59] * DealkTerm * (YECH * YH2 - YMCH * YC1 / EquilK[59]);
            RxnRate[60] = RateK[60] * DealkTerm * (YDMCH * YH2 - YMCH * YC1 / EquilK[60]);
            RxnRate[61] = RateK[61] * DealkTerm * (YPCP * YH2 - YC7CP * YC1 / EquilK[61]);
            RxnRate[62] = RateK[62] * DealkTerm * (YC8CP * YH2 - YC7CP * YC1 / EquilK[62]);
            RxnRate[63] = RateK[63] * DealkTerm * (YC9CH * YH2 - YECH * YC1 / EquilK[63]);
            RxnRate[64] = RateK[64] * DealkTerm * (YC9CH * YH2 - YDMCH * YC1 / EquilK[64]);
            RxnRate[65] = RateK[65] * DealkTerm * (YC9CP * YH2 - YPCP * YC1 / EquilK[65]);
            RxnRate[66] = RateK[66] * DealkTerm * (YC9CP * YH2 - YC8CP * YC1 / EquilK[66]);
            RxnRate[67] = RateK[67] * DealkTerm * (YTOL * YH2 - YBEN * YC1 / EquilK[67]);
            RxnRate[68] = RateK[68] * DealkTerm * (YC8ATot * YH2 - YTOL * YC1 / EquilK[68]);
            RxnRate[69] = RateK[69] * DealkTerm * (YC9A * YH2 - YC8ATot * YC1 / EquilK[69]);
            RxnRate[71] = RateK[71] * NCrackTerm * YC9CH;
            RxnRate[72] = RateK[72] * NCrackTerm * YC9CP;
            RxnRate[73] = RateK[73] * NCrackTerm * YECH;
            RxnRate[74] = RateK[74] * NCrackTerm * YDMCH;
            RxnRate[75] = RateK[75] * NCrackTerm * YPCP;
            RxnRate[76] = RateK[76] * NCrackTerm * YC8CP;
            RxnRate[77] = RateK[77] * NCrackTerm * YMCH;
            RxnRate[78] = RateK[78] * NCrackTerm * YC7CP;
            RxnRate[79] = RateK[79] * NCrackTerm * YCH;
            RxnRate[80] = RateK[80] * NCrackTerm * YMCP;
            RateOfChange[1] = 3.0 * (RxnRate[1] + RxnRate[2] + RxnRate[3] + RxnRate[4] + RxnRate[5] + RxnRate[6] + RxnRate[7] + RxnRate[8] + RxnRate[9] + RxnRate[10]);
            RateOfChange[1] -= RxnRate[30] + RxnRate[31] + RxnRate[32] + RxnRate[33] + RxnRate[34] + RxnRate[35] + RxnRate[36] + RxnRate[37] + RxnRate[38] + RxnRate[39];
            RateOfChange[1] -= RxnRate[40] + RxnRate[41] + RxnRate[42] + RxnRate[45] + RxnRate[46] + RxnRate[47] + RxnRate[48] + RxnRate[49] + RxnRate[50] + RxnRate[51] + RxnRate[52] + RxnRate[53] + RxnRate[54] + RxnRate[57] + RxnRate[58];
            RateOfChange[1] -= RxnRate[59] + RxnRate[60] + RxnRate[61] + RxnRate[62] + RxnRate[63] + RxnRate[64] + RxnRate[65] + RxnRate[66] + RxnRate[67] + RxnRate[68] + RxnRate[69];
            RateOfChange[1] -= 2.0 * (RxnRate[71] + RxnRate[72] + RxnRate[73] + RxnRate[74] + RxnRate[75] + RxnRate[76] + RxnRate[77] + RxnRate[78] + RxnRate[79] + RxnRate[80]);
            RateOfChange[2] = RxnRate[57] + RxnRate[58] + RxnRate[59] + RxnRate[60] + RxnRate[61] + RxnRate[62] + RxnRate[63] + RxnRate[64] + RxnRate[65] + RxnRate[66] + RxnRate[67] + RxnRate[68] + RxnRate[69] + CrackProd(1, RxnRate);
            RateOfChange[3] = CrackProd(2, RxnRate);
            RateOfChange[4] = CrackProd(3, RxnRate);
            RateOfChange[5] = CrackProd(4, RxnRate) + RxnRate[27] - RxnRate[53];
            RateOfChange[6] = CrackProd(5, RxnRate) - RxnRate[27] - RxnRate[54];
            RateOfChange[7] = CrackProd(6, RxnRate) + RxnRate[26] - RxnRate[51];
            RateOfChange[8] = CrackProd(7, RxnRate) - RxnRate[26] - RxnRate[52];
            RateOfChange[9] = CrackProd(8, RxnRate) + RxnRate[25] + RxnRate[32] - RxnRate[49];
            RateOfChange[10] = CrackProd(9, RxnRate) - RxnRate[25] + RxnRate[30] + RxnRate[31] - RxnRate[50];
            RateOfChange[11] = RxnRate[57] - RxnRate[30] - RxnRate[17] - RxnRate[1] - RxnRate[79];
            RateOfChange[12] = RxnRate[58] - RxnRate[31] - RxnRate[32] + RxnRate[17] - RxnRate[2] - RxnRate[80];
            RateOfChange[13] = RxnRate[1] + RxnRate[2] + RxnRate[67];
            RateOfChange[14] = CrackProd(10, RxnRate) + RxnRate[24] + RxnRate[34] + RxnRate[36] - RxnRate[47];
            RateOfChange[15] = CrackProd(11, RxnRate) - RxnRate[24] + RxnRate[33] + RxnRate[35] - RxnRate[48];
            RateOfChange[16] = RxnRate[59] + RxnRate[60] - RxnRate[57] - RxnRate[33] - RxnRate[34] - RxnRate[18] - RxnRate[3] - RxnRate[77];
            RateOfChange[17] = RxnRate[61] + RxnRate[62] - RxnRate[58] - RxnRate[35] - RxnRate[36] + RxnRate[18] - RxnRate[4] - RxnRate[78];
            RateOfChange[18] = RxnRate[68] - RxnRate[67] + RxnRate[3] + RxnRate[4];
            RateOfChange[19] = CrackProd(12, RxnRate) + RxnRate[37] + RxnRate[38] + RxnRate[39] + RxnRate[40] - RxnRate[46];
            RateOfChange[20] = RxnRate[63] - RxnRate[59] - RxnRate[37] + RxnRate[22] - RxnRate[19] - RxnRate[5] - RxnRate[73];
            RateOfChange[21] = RxnRate[64] - RxnRate[60] - RxnRate[38] + RxnRate[21] - RxnRate[22] - RxnRate[6] - RxnRate[74];
            RateOfChange[22] = RxnRate[65] - RxnRate[61] - RxnRate[39] + RxnRate[19] - RxnRate[20] - RxnRate[7] - RxnRate[75];
            RateOfChange[23] = RxnRate[66] - RxnRate[62] - RxnRate[40] + RxnRate[20] - RxnRate[21] - RxnRate[8] - RxnRate[76];
            RateOfChange[24] = RxnRate[5] + RxnRate[7] + 0.096 * (RxnRate[69] - RxnRate[68]);
            RateOfChange[25] = RxnRate[13] - RxnRate[14] + 0.232 * (RxnRate[6] + RxnRate[8]) + 0.209 * (RxnRate[69] - RxnRate[68]);
            RateOfChange[26] = RxnRate[14] - RxnRate[15] + 0.395 * (RxnRate[6] + RxnRate[8]) + 0.35 * (RxnRate[69] - RxnRate[68]);
            RateOfChange[27] = RxnRate[15] - RxnRate[13] + 0.373 * (RxnRate[6] + RxnRate[8]) + 0.345 * (RxnRate[69] - RxnRate[68]);
            RateOfChange[28] = RxnRate[41] + RxnRate[42] - RxnRate[45];
            RateOfChange[29] = 0.0 - RxnRate[9] - RxnRate[23] - RxnRate[41] - RxnRate[63] - RxnRate[64] - RxnRate[71];
            RateOfChange[30] = 0.0 - RxnRate[10] + RxnRate[23] - RxnRate[42] - RxnRate[65] - RxnRate[66] - RxnRate[72];
            RateOfChange[31] = RxnRate[9] + RxnRate[10] - RxnRate[69];

            for (int i = 1; i <= NumComp; i++)
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

            for (int i = 1; i <= 31; i++)
            {
                SumHF += RateOfChange[i] * (StdHeatForm[i] + (CpCoeft[i][1] * TD1 + CpCoeft[i][2] * TD2 + CpCoeft[i][3] * TD3 + CpCoeft[i][4] * TD4) * 100.0);
                SumCP += MoleFlowRate[i].lbMole_hr * (CpCoeft[i][1] + CpCoeft[i][2] * T1 + CpCoeft[i][3] * T2 + CpCoeft[i][4] * T3);
            }

            double DelTHB = SumHF / SumCP;
            RateOfChange[NumDepVar] = - DelTHB;
        }
    }
}