using Units.UOM;

namespace ModelEngine
{
    public class ComponentBips
    {
        public double[,] CreateBIPs(Components cc, Temperature T)
        {
            ThermoDynamicOptions therm = cc.Thermo;
            therm.BIPMethod = enumBIPPredMethod.ChuePrausnitz;

            if (cc.Count == 1)
            {
                double[,] res = new double[1, 1];
                res[0, 0] = 0;
                return res;
            }

            switch (therm.BIPMethod)
            {
                case enumBIPPredMethod.PPR78: // predictive peng robinson only
                    return PPR78e.CreateKij(cc, T);

                default:
                    return InteractionParameters.Update(cc,T, therm.BIPMethod);
            }
        }
    }
}