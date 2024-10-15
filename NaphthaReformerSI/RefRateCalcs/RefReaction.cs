using GenericRxBed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefReactor
{
    public class RefReaction : BaseReaction
    {
        public RefRateEquation rate = new();

        public string MixedCompName;// = Comp.None;
        public RefMixedComps MixedType = RefMixedComps.None;

        public bool IsMixedProducts = false;
        public bool IsMixedFeeds = false;

        public RefReaction(List<string> Feeds, List<string> Products)
        {
            this.Feeds = Feeds.ToArray();
            this.Products = Products.ToArray();
            MixedCompName = "";
        }

        public RefReaction(string Name, string[] comps, int[] stoich, RefRateEquation rate, Dictionary<string, int> NameDict,
            string[] MixedFeedCompNames, string[] MixedProductNames)
        {
            this.Name = Name;
            this.comps = comps;
            this.stoich = stoich;
            this.rate = rate;
            SortInput(NameDict, MixedFeedCompNames, MixedProductNames);
            MixedCompName = "";
        }

        public static RefMixedComps GetCompType(string comp)
        {
            switch (comp)
            {
                case "C8A":
                    return RefMixedComps.C8A;

                case "MixedXylenes":
                    return RefMixedComps.MixedXylenes;

                case "H2":
                    return RefMixedComps.H2;

                case "Hydrocracked":
                    return RefMixedComps.Hydrocracked;

                default:
                    return RefMixedComps.None;
            }
        }

        public void SortInput(Dictionary<string, int> NameDict, string[] MixedFeedCompNames, string[] MixedProductCompNames)
        {
            List<int> St = new();
            List<int> feedpos = new();
            List<int> prodpos = new();
            List<string> feedList = new();
            List<string> productList = new();

            for (int i = 0; i < comps.Length; i++)
            {
                if (stoich[i] < 0 && (NameDict.ContainsKey(comps[i]) || MixedFeedCompNames.Contains(comps[i])))
                {
                    feedList.Add(comps[i]);
                    St.Add(stoich[i]);

                    if (NameDict.ContainsKey(comps[i]))
                        feedpos.Add(NameDict[comps[i]]);
                    else
                        feedpos.Add(999); // dummy value should not be used anywhere

                    if (MixedFeedCompNames.Contains(comps[i]))
                    {
                        MixedType = GetCompType(comps[i]);
                        MixedCompName = comps[i];
                        IsProduct = false;
                        IsMixedFeeds = true;
                    }
                }
                else if (NameDict.ContainsKey(comps[i]) || MixedProductCompNames.Contains(comps[i]))
                {
                    productList.Add(comps[i]);

                    if (NameDict.ContainsKey(comps[i]))
                        prodpos.Add(NameDict[comps[i]]);
                    else
                        prodpos.Add(999); // dummy value should not be used anywhere

                    St.Add(stoich[i]);

                    if (MixedProductCompNames.Contains(comps[i]))
                    {
                        MixedType = GetCompType(comps[i]);
                        MixedCompName = comps[0];
                        IsProduct = true;
                        IsMixedProducts = true;
                    }
                }
            }

            this.stoich = St.ToArray<int>();
            FeedPositionIndex = feedpos.ToArray();
            ProdsPositionIndex = prodpos.ToArray();

            Feeds = feedList.ToArray();
            Products = productList.ToArray();

            FeedsCount = Feeds.Length;
            ProductsCount = Products.Length;
        }

        public override double SolveMixedFeed(double Tk, double[] Composition, double ForwardTerm, double H2Term, int H2Index, double FeedComposition)
        {
            int index;
            double Rf = rate.Rate(Tk);
            double Rr = rate.ReverseRate(Tk);
            double[] TempComp = Composition;
            TempComp[H2Index] = H2Term;
            double ForComp = 1, RevComp = 1;

            for (int i = 0; i < FeedsCount; i++)
            {
                switch (Feeds[i])
                {
                    case "C8A":
                        ForComp *= FeedComposition;
                        break;

                    case "H2":
                        ForComp *= H2Term;
                        break;

                    default:
                        ForComp *= Composition[FeedPositionIndex[i]];
                        break;
                }
            }

            if (Rr != 1)
            {
                for (int i = 0; i < ProductsCount; i++)
                {
                    index = ProdsPositionIndex[i];
                    if (index == 0)
                        RevComp *= H2Term;
                    else
                        RevComp *= Composition[ProdsPositionIndex[i]];
                }
                Rate = Rf * ForwardTerm * (ForComp - RevComp / Rr);
            }
            else
                Rate = Rf * ForComp * ForwardTerm;

            return Rate;
        }

        public override double SolveMixedProducts(double Tk, double[] Composition, double ForwardTerm, double H2Term, int H2Index, double C8AComp)
        {
            int index;
            double Rf = rate.Rate(Tk);
            double Rr = rate.ReverseRate(Tk);
            double[] TempComp = Composition;
            TempComp[H2Index] = H2Term;
            double ForComp = 1, RevComp = 1;

            for (int i = 0; i < FeedsCount; i++)
            {
                index = FeedPositionIndex[i];
                ForComp *= Composition[index];
            }

            if (Rr != 1)
            {
                for (int i = 0; i < ProductsCount; i++)
                {
                    switch (Products[i])
                    {
                        case "H2":
                            RevComp *= H2Term;
                            break;

                        case "C8A":
                            RevComp *= C8AComp;
                            break;

                        case "MixedXylenes":
                            RevComp *= C8AComp;
                            break;

                        case "Hydrocracked":
                            break;

                        default:
                            RevComp *= Composition[ProdsPositionIndex[i]];
                            break;
                    }
                }
                Rate = Rf * ForwardTerm * (ForComp - RevComp / Rr);
            }
            else
                Rate = Rf * ForComp * ForwardTerm;

            return Rate;
        }

        public double Solve(double Tk, double[] Composition, double ForwardTerm, double H2Term, int H2Index, double C8AComp)
        {
            int index;
            double Rf = rate.Rate(Tk);
            double Rr = rate.ReverseRate(Tk);
            double[] TempComp = Composition;
            TempComp[H2Index] = H2Term;
            double ForComp = 1, RevComp = 1;

            for (int i = 0; i < FeedsCount; i++)
            {
                index = FeedPositionIndex[i];
                ForComp *= Composition[index];
            }

            if (Rr != 1)
            {
                for (int i = 0; i < ProductsCount; i++)
                {
                    switch (Products[i])
                    {
                        case "H2":
                            RevComp *= H2Term;
                            break;

                        case "C8A":
                            RevComp *= C8AComp;
                            break;

                        case "Hydrocracked":
                            break;

                        default:
                            RevComp *= Composition[ProdsPositionIndex[i]];
                            break;
                    }
                }
                Rate = Rf * ForwardTerm * (ForComp - RevComp / Rr);
            }
            else
                Rate = Rf * ForComp * ForwardTerm;

            return Rate;
        }
    }
}