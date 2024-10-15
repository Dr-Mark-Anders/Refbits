using Units.UOM;

namespace ModelEngine
{
    //  Estimate V from table, solve with bounded upper and lower V for Z

    public static class KaysMixingRules
    {
        public static double MixCritT(Components o)
        {
            int NoComp = o.Count;
            BaseComp J;
            double res = 0;

            for (int j = 0; j < NoComp; j++)
            {
                J = o[j];
                res += J.CritT * J.MoleFraction;
            }

            return res;
        }

        public static double MixCritP(Components o)
        {
            int NoComp = o.Count;
            BaseComp J;
            double res = 0;

            for (int j = 0; j < NoComp; j++)
            {
                J = o[j];
                res += J.CritP * J.MoleFraction;
            }
            return res;
        }

        public static void KaysMixing(Components cc, out Temperature TCritMix, out Pressure PCritMix)
        {
            TCritMix = new Temperature(0); PCritMix = 0;

            if (cc.ComponentList.Count == 1)
            {
                TCritMix = cc[0].CritT;
                PCritMix = cc[0].CritP;
                return;
            }

            foreach (BaseComp bc in cc)
            {
                TCritMix += bc.CritT * bc.MoleFraction;
                PCritMix += bc.CritP * bc.MoleFraction;
            }
        }
    }
}