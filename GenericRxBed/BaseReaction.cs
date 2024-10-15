using System.Collections.Generic;
using System.Linq;

namespace GenericRxBed
{
    public class BaseReaction
    {
        public BaseRateEquation rate = new BaseRateEquation();
        public string[] Feeds;
        public string[] Products;
        public string[] comps;
        public int[] stoich;
        public int[] FeedPositionIndex, ProdsPositionIndex;
        public string Name;
        public bool IsProduct = true;
        public int FeedsCount = 0;
        public int ProductsCount = 0;
        public double Rate = double.NaN;

        public BaseReaction()
        {
        }

        public BaseReaction(List<string> Feeds, List<string> Products)
        {
            this.Feeds = Feeds.ToArray();
            this.Products = Products.ToArray();
        }

        public BaseReaction(string Name, string[] comps, int[] stoich, BaseRateEquation rate, Dictionary<string, int> NameDict)
        {
            this.Name = Name;
            this.comps = comps;
            this.stoich = stoich;
            this.rate = rate;
            SortInput(NameDict);
        }

        public void SortInput(Dictionary<string, int> NameDict)
        {
            List<int> St = new List<int>();
            List<int> feedpos = new List<int>();
            List<int> prodpos = new List<int>();
            List<string> feedList = new List<string>();
            List<string> productList = new List<string>();

            for (int i = 0; i < comps.Length; i++)
            {
                if (stoich[i] < 0 && (NameDict.ContainsKey(comps[i])))
                {
                    feedList.Add(comps[i]);
                    St.Add(stoich[i]);

                    if (NameDict.ContainsKey(comps[i]))
                        feedpos.Add(NameDict[comps[i]]);
                    else
                        feedpos.Add(999);//dummyvalueshouldnotbeusedanywhere
                }
                else if (NameDict.ContainsKey(comps[i]))
                {
                    productList.Add(comps[i]);

                    if (NameDict.ContainsKey(comps[i]))
                        prodpos.Add(NameDict[comps[i]]);
                    else
                        prodpos.Add(999);//dummyvalueshouldnotbeusedanywhere

                    St.Add(stoich[i]);
                }
            }

            this.stoich = St.ToArray<int>();
            FeedPositionIndex = feedpos.ToArray();
            ProdsPositionIndex = prodpos.ToArray();

            Feeds = feedList.ToArray();
            Products = productList.ToArray();

            FeedsCount = Feeds.Count();
            ProductsCount = Products.Count();
        }

        public virtual double SolveMixedFeed(double Tk, double[] Composition, double ForwardTerm, double H2Term, int H2Index, double FeedComposition)
        {
            return double.NaN;
        }

        public virtual double SolveMixedProducts(double Tk, double[] Composition, double ForwardTerm, double H2Term, int H2Index, double C8AComp)
        {
            return double.NaN;
        }

        public double Solve(double Tk, double[] Composition)
        {
            int index;
            double Rf = rate.Rate(Tk);
            double Rr = rate.ReverseRate(Tk);
            double ForComp = 1, RevComp = 1;

            for (int i = 0; i < FeedsCount; i++)
            {
                index = FeedPositionIndex[i];
                ForComp *= Composition[index];
            }

            if (Rr != 1)
            {
                for (int i = 0; i < ProductsCount; i++)
                    RevComp *= Composition[ProdsPositionIndex[i]];

                Rate = Rf * (ForComp - RevComp / Rr);
            }
            else
                Rate = Rf * ForComp;

            return Rate;
        }
    }
}