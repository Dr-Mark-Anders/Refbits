using GenericRxBed;
using System;
using System.Collections.Generic;
using Units.UOM;

namespace RefReactor
{
    public enum ReformerReactionType
    { CHDehydrogTerm, CPDehydrogTerm, IsomTerm, CyclTerm, PCrackTerm, DealkTerm, NCrackTerm }

    public enum RefCompNames
    {
        H2, C1, C2, C3, IC4, NC4, IC5, NC5, IC6, NC6, CH, MCP, BEN, IC7, NC7, MCH, C7CP, TOL, C8P,
        ECH, DMCH, PCP, C8CP, EB, PX, MX, OX, C9P, C9CH, C9CP, C9A
    }

    public enum RefMixedComps
    { None, C8A, H2, MixedXylenes, Hydrocracked }

    public partial class NapRefBed : BaseRxBed
    {
        public double RefC5PlusLV, SumRON, SumMON,
            H2HC, DHCH, DHCP, ISOM, OPEN, PHC, NHC, HDA, MetalActiv, AcidActiv, AmtCat1, MetalActiv2, AcidActiv2, AmtCat2,
            MetalActiv3, AcidActiv3, AmtCat3, MetalActiv4, AcidActiv4, AmtCat4,
            CHDehydrogTerm = 1, CPDehydrogTerm = 1,
            PCrackTerm, NCrackTerm, DealkTerm, IsomTerm, CyclTerm,
            TotWHSV, OctSpec, P_LV, N_LV, A_LV, P_WT, N_WT, A_WT, APIFeed, LHSV;

        public new RefReactionSet rSet = new RefReactionSet();
        public new RefReaction[] reactions;
        public RefReaction[] MixedFeedsArray;
        public RefReaction[] MixedProductsArray;
        public new RefReaction[] NormalReactionsArray;
        public Dictionary<string, double> RateDict = new Dictionary<string, double>();
        private Dictionary<ReformerReactionType, double> Multipliers = new Dictionary<ReformerReactionType, double>();
        private string[] MixedProductCompNames = new string[] { "C8A", "MixedXylenes", "Hydrocracked" };
        private string[] MixedFeedCompNames = new string[] { "C8A" };
        private int EBIndex, PXIndex, MXIndex, OXIndex;

        private MixedCompclass MixedCompsHandling = new MixedCompclass();

        private string[] Names;

        public void InitialiseComponents()
        {
            Names = new string[] {"H2","C1","C2","C3","IC4","NC4","IC5","NC5","IC6","NC6","CH","MCP"
                ,"BEN","IC7","NC7","MCH","C7CP","TOL","C8P","ECH","DMCH","PCP","C8CP"
                ,"EB","PX","MX","OX","C9P","C9CH","C9CP","C9A" };

            HFORM25 = new double[] {0.0,-32200.2,-36424.8,-44676.0,-57870.0,-54270.0,-66456.0,-63000.0,-75778.2,-71928.0,-52974.0,-45900.0,35676.0,
                    -84576.6,-80802.0,-66582.0,-57498.3,21510.0,-92296.8,-73890.0,-77823.0,-63702.0,-68916.6,12816.0,7722.0,
                    7416.0,8172.0,-101057.4,-88524.0,-79380.0,-3681.0};

            Cp = new double[31][];
            Cp[0] = new double[] { 6.3539, 0.167945, -0.014925, 0.00046677 };
            Cp[1] = new double[] { 6.367, 0.102427, 0.067468, -0.00224337 };
            Cp[2] = new double[] { 2.3112, 2.01391, -0.015322, -0.00076208 };
            Cp[3] = new double[] { -1.0719, 4.03765, -0.11203, 0.0010955 };
            Cp[4] = new double[] { -2.4505, 5.6504, -0.177432, 0.00214335 };
            Cp[5] = new double[] { -0.1302, 5.01947, -0.128858, 0.0009526 };
            Cp[6] = new double[] { -2.28095, 6.692725, -0.194371, 0.00200046 };
            Cp[7] = new double[] { -0.4145, 6.28296, -0.167989, 0.00138127 };
            Cp[8] = new double[] { -5.9217, 9.1914463, -0.357933, 0.00612045 };
            Cp[9] = new double[] { -0.7017, 7.56254387, -0.209619, 0.0019052 };
            Cp[10] = new double[] { -11.9067, 7.71600485, -0.139477, -0.00114312 };
            Cp[11] = new double[] { -11.2874, 8.19076347, -0.23556, 0.00209572 };
            Cp[12] = new double[] { -10.52, 6.9534831, -0.277153, 0.00433432 };
            Cp[13] = new double[] { -0.955, 8.828329, -0.249743, 0.0023815 };
            Cp[14] = new double[] { -0.995, 8.828329, -0.249743, 0.0023815 };
            Cp[15] = new double[] { -12.8626, 9.70844364, -0.251213, 0.00133364 };
            Cp[16] = new double[] { -13.043, 9.98848057, -0.316836, 0.00359606 };
            Cp[17] = new double[] { -9.1226, 7.66801119, -0.272413, 0.0036675 };
            Cp[18] = new double[] { -1.2219, 10.096693, -0.289976, 0.0028578 };
            Cp[19] = new double[] { -13.5338, 11.1712523, -0.30787, 0.00223861 };
            Cp[20] = new double[] { -13.3602, 10.9259481, -0.27381, 0.00109549 };
            Cp[21] = new double[] { -15.94786, 12.1267424, -0.457856, 0.00752553 };
            Cp[22] = new double[] { -15.94786, 12.1267424, -0.457856, 0.00752553 };
            Cp[23] = new double[] { -10.2083, 9.336, -0.348471, 0.0050964 };
            Cp[24] = new double[] { -5.38286, 7.781393, -0.22259, 0.0019052 };
            Cp[25] = new double[] { -6.5457, 8.188448, -0.257013, 0.00281017 };
            Cp[26] = new double[] { -3.38786, 7.75355, -0.236148, 0.00247676 };
            Cp[27] = new double[] { -1.40476, 11.34312, -0.3284465, 0.00328647 };
            Cp[28] = new double[] { -13.8138, 12.6579, -0.370591, 0.00323884 };
            Cp[29] = new double[] { -16.0852, 13.35798, -0.494782, 0.00790657 };
            Cp[30] = new double[] { -9.1543, 10.2822752, -0.361993, 0.00485825 };

            NameDict.Clear();

            for (int i = 0; i < Names.Length; i++)
                NameDict.Add(Names[i], i);

            EBIndex = NameDict["EB"]; PXIndex = NameDict["PX"]; MXIndex = NameDict["MX"]; OXIndex = NameDict["OX"];
        }

        public NapRefBed()
        {
            InitialiseComponents();
            AddReactions();
        }

        public double[] Solve(Temperature Tk, Pressure P, double[] MoleFlows, double MetalAct, double AcidAct, MoleFlow NaphMolFeed)
        {
            RateOfChange = new double[NameDict.Count + 1];

            double FeedRate = 0;
            double[] Composition = new double[MoleFlows.Length];
            int NoRxs = rSet.reactions.Count;
            int NoComps = 30;

            for (int i = 0; i < MoleFlows.Length; i++)
            {
                FeedRate += MoleFlows[i];
                Composition[i] = MoleFlows[i] / NaphMolFeed._value;
            }

            double C8AComp = Composition[EBIndex] + Composition[PXIndex] + Composition[MXIndex] + Composition[OXIndex];
            double H2PP = MoleFlows[0] / FeedRate;
            double H2PP3 = Math.Pow(H2PP, 3);

            InitRateFactors(P, MetalAct, AcidAct, NaphMolFeed);

            int H2index = 0; //NameDict["H2"];

            Composition[H2index] = H2PP;

            //Parallel.For(0, NoRxs, i =>

            for (int i = 0; i < NoRxs; i++)
            {
                double forward = 1, H2Term = 1;
                RefReaction r = reactions[i];

                switch (r.rate.RateType)
                {
                    case ReformerReactionType.CHDehydrogTerm:
                        forward = CHDehydrogTerm;
                        H2Term = H2PP3;
                        break;

                    case ReformerReactionType.CPDehydrogTerm:
                        forward = CPDehydrogTerm;
                        H2Term = H2PP3;
                        break;

                    case ReformerReactionType.IsomTerm:
                        forward = IsomTerm;
                        break;

                    case ReformerReactionType.CyclTerm:
                        forward = CyclTerm;
                        H2Term = H2PP;//Composition[0];
                        H2Term *= P.ATMA;
                        break;

                    case ReformerReactionType.PCrackTerm:
                        forward = PCrackTerm;
                        H2Term = 1;//*= P.ATMA;
                        break;

                    case ReformerReactionType.DealkTerm:
                        forward = DealkTerm;
                        H2Term = H2PP;//Composition[0];
                        break;

                    case ReformerReactionType.NCrackTerm:
                        forward = NCrackTerm;
                        H2Term = 1;
                        break;

                    default:
                        break;
                }

                if (r.IsMixedFeeds)
                    RxnRate[i] = r.SolveMixedFeed(Tk._Kelvin, Composition, forward, H2Term, H2index, C8AComp);
                else if (r.IsMixedProducts)
                    RxnRate[i] = r.SolveMixedProducts(Tk._Kelvin, Composition, forward, H2Term, H2index, C8AComp);
                else
                    RxnRate[i] = r.Solve(Tk._Kelvin, Composition, forward, H2Term, H2index, C8AComp);
            }
            //);

            for (int i = 0; i < NormalReactionsArray.Length; i++)
            {
                RefReaction r = NormalReactionsArray[i];
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

            for (int i = 0; i < MixedFeedsArray.Length; i++)
            {
                RefReaction r = MixedFeedsArray[i];
                double[] MixRates = MixedCompsHandling.SolveMixFeeds(r.MixedType, r.MixedCompName, r.IsProduct);

                for (int j = 0; j < MixRates.Length; j++)
                    RateOfChange[j] += MixRates[j] * r.Rate;

                for (int j = 0; j < r.Products.Length; j++)
                {
                    int index = r.ProdsPositionIndex[j];
                    RateOfChange[index] += r.Rate * r.stoich[j + r.Feeds.Length];
                }
            }

            for (int i = 0; i < MixedProductsArray.Length; i++)
            {
                RefReaction r = MixedProductsArray[i];
                double[] MixRates = MixedCompsHandling.SolveMixProducts(r.MixedType, r.MixedCompName, r.IsProduct);

                for (int j = 0; j < MixRates.Length; j++)
                    RateOfChange[j] += MixRates[j] * r.Rate;

                for (int j = 0; j < r.Feeds.Length; j++)
                {
                    int index = r.FeedPositionIndex[j];
                    RateOfChange[index] += r.Rate * r.stoich[j];
                }
            }

            //Parallel.For(0, NoRxs, i =>

            /*  for (int  i = 0; i < NoRxs; i++)
               {
                   double  rxrate = RxnRate[i];
                   Reaction rx = rSet.reactions[i];

                   if (!rx.IsMixedFeeds)
                   {
                       for (int  J = 0; J < rx.Feeds.Count; J++)
                       {
                           int  index = rx.FeedPositionIndex[J];
                           RateOfChange[index] += rxrate * rx.stoich[J];
                       }
                   }

                   if (!rx.IsMixedProducts)
                   {
                       for (int  J = 0; J < rx.Products.Count; J++)
                       {
                           int  index = rx.ProdsPositionIndex[J];
                           RateOfChange[index] += rxrate * rx.stoich[J + rx.Feeds.Count];
                       }
                   }

                   // Handle Mixed Lumped Components
                  if (rx.IsMixedFeeds)
                  {
                       double [] MixRates = MixedCompsHandling.SolveMixFeeds(rx.MixedType, rx.MixedCompName, rx.IsProduct);

                       for (int  J = 0; J < MixRates.Length; J++)
                       {
                           RateOfChange[J] += MixRates[J] * rxrate;
                       }
                   }
                   else if (rx.IsMixedProducts)// Handle Mixed Lumped Components
                   {
                       double [] MixRates = MixedCompsHandling.SolveMixProducts(rx.MixedType, rx.MixedCompName, rx.IsProduct);

                       for (int  j = 0; j < MixRates.Length; j++)
                       {
                           RateOfChange[j] += MixRates[j] * rxrate;
                       }
                   }

                    Debug.Print (i + " " + RateOfChange[24].ToString());
               }*/
            //);

            for (int i = 0; i <= NoComps; i++)
                RateOfChange[i] *= NaphMolFeed;

            double T1 = Tk.Rankine * 0.01;
            double T2 = T1 * T1;
            double T3 = T1 * T2;
            double T4 = T1 * T3;
            double TD1 = T1 - 5.36688;  // = (273.15+25)*1.8
            double TD2 = T2 / 2.0 - 14.4017;
            double TD3 = T3 / 3.0 - 51.52813;
            double TD4 = T4 / 4.0 - 207.40898;
            double SumHF = 0.0;
            double SumCP = 0.0;

            for (int i = 0; i <= NoComps; i++)
            {
                SumHF += RateOfChange[i] * (HFORM25[i] + (Cp[i][0] * TD1 + Cp[i][1] * TD2 + Cp[i][2] * TD3 + Cp[i][3] * TD4) * 100); // enthlpy chnags
                SumCP += MoleFlows[i] * (Cp[i][0] + Cp[i][1] * T1 + Cp[i][2] * T2 + Cp[i][3] * T3); // Heat Capcity
            }

            double DelTHB = SumHF / SumCP;
            RateOfChange[NameDict.Count] = -DelTHB / 1.8;

            return RateOfChange;
        }
    }
}