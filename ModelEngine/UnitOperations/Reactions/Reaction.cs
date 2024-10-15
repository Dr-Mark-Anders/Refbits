using ModelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Units.UOM;

namespace ModelEngine
{
    
    public class Reaction
    {
        internal double Rgas = Global.Rgas;
        internal ReactionComponents CompList = new();
        internal Arrhenius ar = new();
        internal Dictionary<string, double> Changes = new();
        internal string reactionName;
        internal bool IsEquilibrium = true;
        private Components cc;
        private Dictionary<string, int> index;
        private double rate = 0;

        /// <summary>
        /// STANDARD FORM
        /// </summary>
        /// <param name="Comps"></param>
        /// <param name="k"></param>
        /// <param name="Ea"></param>
        /// <param name="kr"></param>
        /// <param name="Er"></param>
        public Reaction(ReactionComponents Comps, double k, double Ea, double kr, double Er, bool deltabasis = false)
        {
            this.CompList = Comps;

            if (deltabasis)
            {
                ar.K = k;
                ar.Ae = Ea;
                ar.Kr = k - kr;
                ar.Aer = Ea - Er;
            }
            else
            {
                ar.K = k;
                ar.Ae = Ea;
                ar.Aer = Er;
                ar.Kr = kr;
            }
        }
        public Reaction(Components cc, string ReactionName, string[] compNames, int[] Stoich, Arrhenius Ar, YieldPattern yieldPattern = null) // create a componenet list before adding reactions
        {
            this.cc = cc;
            this.ReactionName = ReactionName;

            if (compNames.Count() != Stoich.Count())
                throw new Exception();
            else
            {
                for (int i = 0; i < compNames.Count(); i++)
                {
                    ReactionComponent rc = null;

                    if (compNames[i] != "")
                    {
                        string compname = compNames[i].ToUpper();
                        BaseComp bc;

                        bc = cc[compname];

                        rc = new ReactionComponent(bc.Name, Stoich[i]);

                        if (Thermodata.Mix.Contains(compNames[i]))
                            rc.Type = CompType.Mixed;
                        else if (Thermodata.ShortNames.ContainsKey(compNames[i]))
                        {
                            rc.Type = CompType.Short;
                        }
                        else if (yieldPattern is not null && compNames[i] == yieldPattern.Name)
                        {
                            rc.Type = CompType.YieldPattern;
                            rc.Yieldpattern = yieldPattern;
                        }

                        if (rc != null)
                        {
                            CompList.Add(rc);
                        }
                    }
                }
            }

            if (Ar.Aer == 0)
                IsEquilibrium = false;

            this.ar = Ar;
        }

        public Reaction(string name, List<string> compNames, List<int> Stoich, Arrhenius Ar)
        {
            ReactionName = name;
            if (compNames.Count != Stoich.Count)
                throw new Exception();
            else
            {
                for (int i = 0; i < compNames.Count; i++)
                {
                    if (compNames[i] != "")
                    {
                        BaseComp bc = Thermodata.GetComponent(compNames[i]);
                        if (bc is not null)
                            CompList.Add(new ReactionComponent(bc.Name, Stoich[i]));
                    }
                }
            }
            this.ar = Ar;
        }

        public Reaction(List<string> comps, List<int> Stoich, double k, double Ea, double kr, double Er, bool deltabasis = false)
        {
            if (comps.Count != Stoich.Count)
                throw new Exception();
            else
            {
                for (int i = 0; i < comps.Count; i++)
                {
                    BaseComp bc = Thermodata.GetComponent(comps[i]);
                    CompList.Add(new ReactionComponent(bc.Name, Stoich[i]));
                }
            }

            if (deltabasis)
            {
                ar.K = k;
                ar.Ae = Ea;
                ar.Kr = k - kr;
                ar.Aer = Ea - Er;
            }
            else
            {
                ar.K = k;
                ar.Ae = Ea;
                ar.Aer = Er;
                ar.Kr = kr;
            }
        }

        public Reaction()
        {
        }

        public virtual double SumComponenents(Components cc, string Name, YieldPattern yieldpattern)
        {
            double res = 0;

            if (Name == yieldpattern.Name)
            {
                foreach (string comp in yieldpattern.Names)
                {
                    if (Thermodata.ShortNames.TryGetValue(comp, out string name))
                        res += cc[name].MoleFraction;
                    else
                        res += cc[comp].MoleFraction;
                }
            }
            else
                res = cc[Name].MassFraction;

            return res;
        }

        public virtual double[] SplitYields(double[] res, Components cc, string CompName, YieldPattern yieldpattern)
        {
            string realname = CompName;
            if (CompName == yieldpattern.Name)
            {
                for (int i = 0; i < yieldpattern.Names.Length; i++)
                {
                    string comp = yieldpattern.Names[i];
                    if (Thermodata.ShortNames.TryGetValue(comp, out realname))
                    {
                        int loc = cc.IndexOf(realname);
                        res[loc] += cc[realname].MoleFraction;
                    }
                }
            }
            else
            {
                int loc = cc.IndexOf(realname);
                res[loc] += cc[realname].MoleFraction;
            }

            return res;
        }

        public int NoComps
        {
            get { return CompList.FComps.Count + CompList.RComps.Count; }
        }

        public ReactionComponents RComps { get => CompList; set => CompList = value; }

        public virtual double solve(Dictionary<string, int> Index, Pressure P, Temperature T, double[] MoleFracs, CalibrationFactors factors = null)
        {
            double res = 0, factor = 1;

            foreach (KeyValuePair<string, ReactionComponent> kvp in CompList.FComps)
            {
                if (cc.Contains(kvp.Key))
                    kvp.Value.MoleFraction = MoleFracs[Index[kvp.Key]];
                else
                    MessageBox.Show("Comp Not FOund");
            }

            foreach (KeyValuePair<string, ReactionComponent> kvp in CompList.RComps)
            {
                if (cc.Contains(kvp.Key))
                {
                    kvp.Value.MoleFraction = MoleFracs[Index[kvp.Key]];
                }
            }

            if (factors != null)
            {
                switch (ar.Rtype)
                {
                    case ReformerReactionType.CHDehydrogTerm:
                        factor = factors.CHDehydrogTerm;
                        CompList.RComps["Hydrogen"].MoleFraction *= P.ATMA; // partial pressure effect
                        break;

                    case ReformerReactionType.CPDehydrogTerm:
                        factor = factors.CPDehydrogTerm;
                        CompList.RComps["Hydrogen"].MoleFraction *= P.ATMA; // partial pressure effect
                        break;

                    case ReformerReactionType.IsomTerm:
                        factor = factors.IsomTerm;
                        break;

                    case ReformerReactionType.CyclTerm:
                        factor = factors.CyclTerm;
                        CompList.FComps["Hydrogen"].MoleFraction *= P.ATMA; // partial pressure effect
                        break;

                    case ReformerReactionType.DealkTerm:
                        factor = factors.DealkTerm;
                        break;

                    case ReformerReactionType.PCrackTerm:
                        factor = factors.PCrackTerm;
                        IsEquilibrium = false;
                        CompList.FComps["Hydrogen"].MoleFraction = 1; // no partial pressure/hydrogen effect?
                        break;

                    case ReformerReactionType.NCrackTerm:
                        IsEquilibrium = false;
                        CompList.FComps["Hydrogen"].MoleFraction = 1; // no partial pressure/hydrogen effect?
                        factor = factors.NCrackTerm;
                        break;

                    case ReformerReactionType.none:
                        factor = factors.None;
                        break;

                    default:
                        break;
                }
            }

            switch (NoComps)
            {
                case 0: // no reaction, dummy
                    res = 0;
                    break;

                case 1: // eg thermal Cracking

                    break;

                case 2:
                    res = solve2(T, RComps, factor);
                    break;

                case 3:
                    if (RComps.FComps.Count == 2 && RComps.RComps.Count == 1)
                        res = solve3A(T, RComps, factor); //  2 forward 1 back
                    else
                        res = solve3B(T, RComps, factor); // 1 forward 2 back
                    break;

                case 4:
                    res = solve4(T, RComps, factor);
                    break;
            }

            rate = res;

            return res;
        }

        public virtual double solve2(Temperature T, ReactionComponents cc, double factor)
        {
            ReactionComponent Ca = cc.FComps.Values.ToList()[0];
            ReactionComponent Cb = cc.RComps.Values.ToList()[0];

            double s1 = Math.Abs(Ca.Stoich);
            double s2 = Math.Abs(Cb.Stoich);
            double rbr = 0;
            double rf = factor * Math.Pow(Ca.MoleFraction, s1) * Math.Exp(ar.K) * Math.Exp(-ar.Ae * 1000 / (T.Kelvin * Rgas));

            if (IsEquilibrium)
                rbr = factor * Math.Pow(Cb.MoleFraction, s2) * Math.Exp(ar.K - ar.Kr) * Math.Exp(-(ar.Ae - ar.Aer) * 1000 / (T.Kelvin * Rgas));

            double r = rf - rbr;

            return r;
        }

        public virtual double solve3A(Temperature T, ReactionComponents cc, double factor)
        {
            ReactionComponent Ca = cc.FComps.Values.ToList()[0];
            ReactionComponent Cb = cc.FComps.Values.ToList()[1];
            ReactionComponent Cc = cc.RComps.Values.ToList()[0];

            double s1 = Math.Abs(Ca.Stoich);
            double s2 = Math.Abs(Cb.Stoich);
            double s3 = Math.Abs(Cc.Stoich);
            double rbr = 0;

            double rf = factor * Math.Pow(Ca.MoleFraction, s1) * Math.Pow(Cb.MoleFraction, s2) * Math.Exp(ar.K) * Math.Exp(-ar.Ae * 1000 / (T.Kelvin * Rgas));

            if (IsEquilibrium)
                rbr = factor * Math.Pow(Cc.MoleFraction, s3) * Math.Exp(ar.K - ar.Kr) * Math.Exp(-(ar.Ae - ar.Aer) * 1000 / (T.Kelvin * Rgas));

            double r = rf - rbr;

            return r;
        }

        public virtual double solve3B(Temperature T, ReactionComponents cc, double factor)
        {
            ReactionComponent Ca = cc.FComps.Values.ToList()[0];
            ReactionComponent Cb = cc.RComps.Values.ToList()[0];
            ReactionComponent Cc = cc.RComps.Values.ToList()[1];

            double s1 = Math.Abs(Ca.Stoich);
            double s2 = Math.Abs(Cb.Stoich);
            double s3 = Math.Abs(Cc.Stoich);
            double rbr = 0;

            double rf = factor * Math.Pow(Ca.MoleFraction, s1) * Math.Exp(ar.K - ar.Ae * 1000 / (T.Kelvin * Rgas));

            if (IsEquilibrium)
                rbr = factor * Math.Pow(Cc.MoleFraction, s3) * Math.Pow(Cb.MoleFraction, s2) * Math.Exp(ar.K - ar.Kr) * Math.Exp(-(ar.Ae - ar.Aer) * 1000 / (T.Kelvin * Rgas));

            double r = rf - rbr;

            return r;
        }

        public virtual double solve4(Temperature T, ReactionComponents cc, double factor)
        {
            ReactionComponent Ca = cc.FComps.Values.ToList()[0];
            ReactionComponent Cb = cc.FComps.Values.ToList()[1];
            ReactionComponent Cc = cc.RComps.Values.ToList()[0];
            ReactionComponent Cd = cc.RComps.Values.ToList()[1];

            double s1 = Math.Abs(Ca.Stoich);
            double s2 = Math.Abs(Cb.Stoich);
            double s3 = Math.Abs(Cc.Stoich);
            double s4 = Math.Abs(Cd.Stoich);

            double rf = factor * Math.Pow(Ca.MoleFraction, s1) * Math.Pow(Cb.MoleFraction, s2) * Math.Exp(ar.K) * Math.Exp(-ar.Ae * 1000 / (T.Kelvin * Rgas));
            double rbr = factor * Math.Pow(Cc.MoleFraction, s3) * Math.Pow(Cd.MoleFraction, s4) * Math.Exp(ar.K - ar.Kr) * Math.Exp(-(ar.Ae - ar.Aer) * 1000 / (T.Kelvin * Rgas));

            double r = rf - rbr;

            return r;
        }

        public List<string> CompNames
        {
            get
            {
                List<string> list = new List<string>();
                list.AddRange(CompList.FComps.Keys);
                list.AddRange(CompList.RComps.Keys);
                return list;
            }
        }

        public string ReactionName { get => reactionName; set => reactionName = value; }
        public Arrhenius Ar { get => ar; set => ar = value; }
        public Dictionary<string, int> Index { get => index; set => index = value; }
        public double Rate { get => rate; set => rate = value; }
        public Components CC { get => cc; set => cc = value; }
    }
}