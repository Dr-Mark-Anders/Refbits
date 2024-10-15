using Extensions;
using Steam97;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units.UOM;

namespace ModelEngine
{
    public static class EnthalpDepClass
    {
        public static Enthalpy HDeparture(Components cc, double[] XWet, Pressure P, Temperature T, enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            if (cc is null || cc.Count == 0 || !P.IsKnown || !T.IsKnown)
                return double.NaN;

            double[] XCopy = (double[])XWet.Clone();

            if (cc is null || cc.ComponentList is null)
                return double.NaN;

            bool HandleWaterSeperately = false;

            Enthalpy EnthDeparture = double.NaN;


            if (cc.Count > 1 || (cc.Count > 0 && !HandleWaterSeperately))
            {
                switch (thermo.Enthalpy)
                {
                    case enumEnthalpy.Ideal:
                        EnthDeparture = 0;
                        break;

                    case enumEnthalpy.LeeKesler:
                    case enumEnthalpy.ChaoSeader:
                    case enumEnthalpy.GraysonStreed:
                        EnthDeparture = LeeKesler.H_Hig(cc, XCopy, P, T, state);
                        break;

                    case enumEnthalpy.PR76:
                        EnthDeparture = PengRobinson.H_Hig(cc, XCopy, P, T, state, thermo);
                        break;

                    case enumEnthalpy.PR78:
                        EnthDeparture = PengRobinson.H_Hig(cc, XCopy, P, T, state, thermo);
                        break;

                    case enumEnthalpy.PRSV:
                        EnthDeparture = PengRobinson.H_Hig(cc, XCopy, P, T, state, thermo);
                        break;

                    case enumEnthalpy.SRK:
                        EnthDeparture = SRK.H_Hig(cc,P,T, XCopy, state);
                        break;

                    case enumEnthalpy.RK:
                        EnthDeparture = RK.H_Hig(cc, P, T, XCopy, state);
                        break;
                    case enumEnthalpy.BWRS:
                        EnthDeparture = BWRS.H_Hig(cc, XCopy, T);
                        break;

                    default:
                        EnthDeparture = double.NaN;
                        break;
                }
            }

            return EnthDeparture;
        }
    }
}
