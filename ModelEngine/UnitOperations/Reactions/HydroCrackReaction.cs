using ModelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Units.UOM;

namespace ModelEngine
{
    public class HydroCrackReaction : Reaction
    {
        public YieldPattern yieldPattern { get; set; }

        public HydroCrackReaction(Components cc, string ReactionName, string[] compNames, int[] Stoich, Arrhenius Ar, YieldPattern yieldPattern = null)
        {
            this.ReactionName = ReactionName;
            this.yieldPattern = yieldPattern;

            if (compNames.Count() != Stoich.Count())
                throw new Exception();
            else
            {
                for (int i = 0; i < compNames.Count(); i++)
                {
                    ReactionComponent rc = null;
                    if (compNames[i] != "" && Stoich[i] < 0)
                    {
                        string compname = compNames[i];
                        BaseComp bc = Thermodata.GetComponent(compname);
                        rc = new ReactionComponent(bc.Name, Stoich[i]);

                        if (rc != null)
                            CompList.Add(rc);
                    }
                }
            }

            if (Ar.Aer == 0)
                IsEquilibrium = false;

            this.ar = Ar;
        }

        public override double solve(Dictionary<string, int> Index, Pressure P, Temperature T, double[] MoleFracs, CalibrationFactors factors = null)
        {
            double res = 0, factor = 1;

            foreach (KeyValuePair<string, ReactionComponent> kvp in CompList.FComps)
            {
                {
                    if (Index.ContainsKey(kvp.Key))
                        kvp.Value.MoleFraction = MoleFracs[Index[kvp.Key]];
                    else
                        MessageBox.Show("Comp Not FOund");
                }
            }

            foreach (ReactionComponent rc in CompList.RComps.Values) // sum product mole pcts
            {
                if (rc.Type == CompType.YieldPattern) // sum up prduct mole pcts
                {
                    for (int i = 0; i < rc.Yieldpattern.Count; i++)
                    {
                        res += MoleFracs[Index[rc.Yieldpattern.Names[i]]];
                    }
                    rc.MoleFraction = res;
                }
                else
                {
                    if (Index.ContainsKey(rc.Name))
                        rc.MoleFraction = MoleFracs[Index[rc.Name]];
                    else
                        MessageBox.Show("Comp Not FOund");
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
            Rate = res;
            return res;
        }

        public override double solve2(Temperature T, ReactionComponents cc, double factor)
        {
            ReactionComponent Ca = cc.FComps.Values.ToList()[0];
            ReactionComponent Cb = cc.FComps.Values.ToList()[1];

            double s1 = Math.Abs(Ca.Stoich);
            double s2 = Math.Abs(Cb.Stoich);
            double rbr = 0;
            double rf = factor * Math.Pow(Ca.MoleFraction, s1) * Math.Exp(ar.K) * Math.Exp(-ar.Ae * 1000 / (T.Kelvin * Rgas));

            double r = rf - rbr;

            return r;
        }

        public override double solve3A(Temperature T, ReactionComponents cc, double factor)
        {
            ReactionComponent Ca = cc.FComps.Values.ToList()[0];
            ReactionComponent Cb = cc.FComps.Values.ToList()[1];
            ReactionComponent Cc = cc.RComps.Values.ToList()[0];

            double s1 = Math.Abs(Ca.Stoich);
            double s2 = Math.Abs(Cb.Stoich);
            double s3 = Math.Abs(Cc.Stoich);
            double rbr = 0;

            double rf = factor * Math.Pow(Ca.MoleFraction, s1) * Math.Pow(Cb.MoleFraction, s2) * Math.Exp(ar.K) * Math.Exp(-ar.Ae * 1000 / (T.Kelvin * Rgas));

            double r = rf - rbr;

            return r;
        }

        public override double solve3B(Temperature T, ReactionComponents cc, double factor)
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

        public override double solve4(Temperature T, ReactionComponents cc, double factor)
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
    }
}