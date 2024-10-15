using ModelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Units.UOM;

namespace ModelEngine
{
    public class FCCCrackReaction : Reaction
    {
        public YieldPattern yieldPattern { get; set; }

        public FCCCrackReaction(Components cc, string ReactionName, string[] compNames, int[] Stoich, Arrhenius Ar, YieldPattern yieldPattern = null)
        {
            this.CC = cc;
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

        public override double solve(Dictionary<string, int> Index, Pressure P, Temperature T, double[] MoleFracs, CalibrationFactors factors = null)
        {
            double res = 0, factor = 1;

            foreach (KeyValuePair<string, ReactionComponent> kvp in CompList.FComps)
            {
                if (Index.ContainsKey(kvp.Key))
                    kvp.Value.MoleFraction = MoleFracs[Index[kvp.Key]];
                else
                    MessageBox.Show("Comp Not FOund");
            }

            res = solve1(T, RComps, factor);

            Rate = res;
            return res;
        }

        public double solve1(Temperature T, ReactionComponents cc, double factor)
        {
            ReactionComponent Ca = cc.FComps.Values.ToList()[0];

            double s1 = Math.Abs(Ca.Stoich);
            double rf = factor * Math.Pow(Ca.MoleFraction, s1) * ar.K * Math.Exp(-ar.Ae * 1000 / (T.Kelvin * Rgas));
            double r = rf;

            return r;
        }
    }
}