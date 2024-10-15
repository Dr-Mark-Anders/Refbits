using System.Collections.Generic;
using Units.UOM;

namespace GenericRxBed
{
    public partial class BaseRxBed
    {
        private List<BaseReaction> MixedFeeds = new List<BaseReaction>();
        private List<BaseReaction> MixedProducts = new List<BaseReaction>();
        private List<BaseReaction> NormalReactions = new List<BaseReaction>();

        public virtual void InitRateFactors(Pressure ReactorP, double MetalAct, double AcidAct, MoleFlow MolFeedRate)
        {
        }

        public virtual void AddReactions()
        {
            rSet.Add(new BaseReaction("MCP+2H2→Hydrocrackedproducts", new string[]
            {"MCP","H2","Hydrocracked",""},
            new int[] { -1, -2, 1, 0 },
            new BaseRateEquation(14.5545, 144.91302, 0, 0), NameDict));

            foreach (BaseReaction rx in rSet)
            {
                NormalReactions.Add(rx);
            }

            NormalReactionsArray = NormalReactions.ToArray();
            reactions = rSet.reactions.ToArray();
        }
    }
}