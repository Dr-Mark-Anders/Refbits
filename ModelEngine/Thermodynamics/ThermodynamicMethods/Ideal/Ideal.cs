using Units;
using Units.UOM;

namespace ModelEngine
{
    public class Ideal
    {
        public static ThermoProps ThermoBulk(Components cc, Pressure P, Temperature T, double[] x, enumFluidRegion state, double idealH, double idealS)
        {                                                  
            ThermoProps props = new ThermoProps();
            props.H = idealH;

            switch (state)
            {
                case enumFluidRegion.Liquid:
                case enumFluidRegion.CompressibleLiquid:
                    double HVap = 0;
                    for (int i = 0; i < cc.Count; i++)
                    {
                        HVap = cc[i].HeatVapEstimate(T) * x[i];
                        props.H -= HVap;
                    }
                    break;

                default:
                    break;
            }
            props.S = idealS;
            props.V = ThermodynamicsClass.Rgas * KaysMixingRules.MixCritT(cc) / KaysMixingRules.MixCritP(cc);
            props.Z = 1;
            props.G = new Gibbs(props.H - T * props.S);
            props.U = props.H - ThermodynamicsClass.Rgas * T;
            props.A = new Helmotz(props.U - T * props.S);

            return props;
        }
    }
}