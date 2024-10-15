using ModelEngine;
using Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ModelEngine
{
    public class Synthesise
    {
        public static Port_Material SynthesisePlantData(Components cc, List<BasePlantData> data)
        {
            double[] MoleFlows = new double[cc.Count];

            for (int i = 0; i < data.Count; i++)
            {
                switch (data[i])
                {
                    case ComponentPlantData cpd:
                        for (int n = 0; n < cpd.Comps.Count; n++)
                        {
                            BaseComp bc = cpd.Comps[n];
                            BaseComp bc2 = cc[bc.Name];
                            MoleFlows[n] += cpd.Comps[n].MoleFraction * cpd.MoleFlow;
                            //bc2.MoleFraction += cpd.Comps[n].MoleFraction * cpd.MoleFlow;
                        }

                        break;

                    case PseudoPlantData ppd:
                        Components c = ppd.Port.cc;

                        for (int n = 0; n < c.Count; n++)
                        {
                            BaseComp bc = c[n];
                            BaseComp bc2 = cc[bc.Name];
                            MoleFlows[n] += ppd.Comps[n].MoleFraction * ppd.MoleFlow;
                            //bc2.MoleFraction += c[n].MoleFraction * ppd.MoleFlow;
                        }

                        break;

                    default:
                        break;
                }
            }

            Port_Material res = new();
            res.AddComponentsToPort(cc);
            res.MolarFlow_.BaseValue = MoleFlows.Sum();
            res.SetMoleFractions(MoleFlows.Normalise());

            return res;
        }
    }
}