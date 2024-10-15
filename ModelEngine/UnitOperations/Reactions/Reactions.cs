using ModelEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Units.UOM;

namespace ModelEngine
{
    public class Reactions : IEnumerable<Reaction>
    {
        private List<Reaction> reactions = new();
        private MixedComponents mixedcomps;
        public CalibrationFactors factors = new CalibrationFactors();
        private Components cc;

        public Reactions(Components cc)
        {
           // this.mixedcomps = mixedcomps;
            this.cc = cc;
        }

        public Reactions()
        {
        }

        public List<string> Names
        {
            get
            {
                List<string> res = new();
                for (int i = 0; i < reactions.Count; i++)
                {
                    for (int y = 0; y < reactions[i].CompNames.Count; y++)
                    {
                        string name = reactions[i].CompNames[y];
                        if (!res.Contains(name))
                            res.Add(name);
                    }
                }
                return res;
            }
        }

        public int Count
        {
            get { return reactions.Count; }
        }

        public void Add(Reaction Rx)
        {
            reactions.Add(Rx);
        }

        public Reaction this[int index]
        {
            get { return reactions[index]; }
        }

        public bool LoadFromFile(string filename)
        {
            ReactionComponents components = new();
            ReactionComponent comp;
            int RxNumberCount = 1;

            if (File.Exists(filename))
            {
                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var values = line.Split(',');
                        int Linelength = values.Count();
                        int RXNo = int.Parse(values[0]);
                        int pos = 0;

                        if (RXNo == RxNumberCount)
                        {
                            for (int i = 1; i < Linelength; i += 2)
                            {
                                string name = values[i];
                                if (name == "999")
                                {
                                    pos = i;
                                    break;
                                }
                                int stoich = int.Parse(values[i + 1]);
                                comp = new(name, stoich);
                                components.Add(comp);
                            }
                            if (!double.TryParse(values[pos + 1], out double k1))
                                return false;
                            if (!double.TryParse(values[pos + 2], out double ea1))
                                return false;
                            if (!double.TryParse(values[pos + 3], out double k2))
                                return false;
                            if (!double.TryParse(values[pos + 4], out double ea2))
                                return false;
                            Reaction rx = new(components, k1, ea1, k2, ea2);
                            reactions.Add(rx);
                            components = new();
                        }
                    }
                }
            }
            else
                return false;
            
            return true;
        }

        public double[] Solve(Pressure P, Temperature T, Components cc, double Step, double StepSize, out double[] Deltas, CalibrationFactors factors)
        {
            Deltas= new double[cc.Count];
            double[] res = new double[reactions.Count];
            for (int i = 0; i < reactions.Count; i++)
            {
                Reaction r = reactions[i];
                res[i] = r.solve(cc.Index, P, T, res, factors);
            }
            return res;
        }

        public IEnumerator<Reaction> GetEnumerator()
        {
            return reactions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return reactions.GetEnumerator();
        }

        public void AddReaction(string[] comps, int[] stoich, double k, double Ae, double kr, double Aer)
        {
            reactions.Add(new Reaction(comps.ToList(), stoich.ToList(), k, Ae, kr, Aer));
        }

        public void AddReaction(List<string> comps, List<int> stoich, double k, double Ae, double kr, double Aer)
        {
            reactions.Add(new Reaction(comps, stoich, k, Ae, kr, Aer));
        }

        public void AddReaction(List<string> comps, List<int> stoich, Arrhenius Ar)
        {
            reactions.Add(new Reaction("", comps, stoich, Ar));
        }

        public Components GetComponents()
        {
            Components cc = new();

            foreach (Reaction reac in reactions)
            {
                foreach (ReactionComponent item in reac.RComps.FComps.Values)
                {
                    BaseComp bc = Thermodata.GetComponent(item.Name);
                    cc.Add(bc);
                }
                foreach (ReactionComponent item in reac.RComps.RComps.Values)
                {
                    BaseComp bc = Thermodata.GetComponent(item.Name);
                    cc.Add(bc);
                }
            }
            return cc;
        }
    }
}