using Units.UOM;

namespace ModelEngine
{
    public class PropertyEstimation
    {
        public static double CalcCritZ(double omega) // Lee kesler
        {
            return 0.2905 - 0.085 * omega;
        }

        /// <summary>
        /// API 1976,1:4D4.1
        /// </summary>
        /// <param name="Tb"></param>
        /// <param name="SG"></param>
        /// <return  s></return  s>
        public static double CalcOmega(Temperature MeABP, double SG, Temperature CritT, double CritP, ThermoDynamicOptions thermo)
        {
            switch (thermo.OmegaMethod)
            {
                case enumOmegaMethod.LeeKesler:
                    return LeeKesler.Omega(SG, MeABP, CritT, CritP);

                case enumOmegaMethod.Edmister:
                    return Edmister.Omega(MeABP, CritT, CritP);

                case enumOmegaMethod.Cavett:
                    //double  res = cav.Cavett();
                    return -999;

                default:
                    return 0;
            }
        }

        public static double CalcMW(Temperature MeABP, double SG, ThermoDynamicOptions thermo)
        {
            switch (thermo.MW_Method)
            {
                case enumMW_Method.LeeKesler:
                    return LeeKesler.MW(MeABP, SG);

                case enumMW_Method.RiaziDaubert:
                    return RiaziDaubert.MW(MeABP, SG); ;
                case enumMW_Method.TWU:
                    TWU.Calc(MeABP, SG);
                    return TWU.MW;

                default:
                    return -999;
            }
        }

        public static double CalcCritV(double CritP, double CritT, double CritZ, Temperature MeABP, double SG, ThermoDynamicOptions thermo)
        {
            switch (thermo.CritVMethod)
            {
                case enumCritVMethod.LeeKesler:
                    double res = CritZ * 8.314 * CritT / CritP / 100;
                    if (res < 0)
                        res = 0.001;
                    return res;

                case enumCritVMethod.TWU:
                    TWU.Calc(MeABP, SG);
                    return TWU.VcOut;

                default:
                    return double.NaN;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="MeABP"></param>
        /// <param name="SG"></param>
        /// <param name="MW"></param>
        /// <param name="thermo"></param>
        /// <return  s></return  s>
        public static Temperature CalcCritT(Temperature MeABP, double SG, double MW, ThermoDynamicOptions thermo)
        {
            Temperature Tcrit;
            switch (thermo.CritTMethod)
            {
                case enumCritTMethod.LeeKesler:
                    Tcrit = LeeKesler.TCrit(SG, MeABP);
                    return Tcrit;

                case enumCritTMethod.RiaziDaubert1:
                    Tcrit = RiaziDaubert.RD0_TCrit(SG, MeABP);
                    return Tcrit;

                case enumCritTMethod.RiaziDaubert2:
                    Tcrit = RiaziDaubert.RD1_TCrit(SG, MeABP);
                    return Tcrit;

                case enumCritTMethod.RiaziDaubert3:
                    Tcrit = RiaziDaubert.RD2_TCrit(SG, MeABP);
                    return Tcrit;

                case enumCritTMethod.Vetere:
                    Tcrit = Vetere.CritT(SG, MeABP, MW);
                    return Tcrit;

                case enumCritTMethod.TWU:
                    TWU.Calc(MeABP, SG);
                    Tcrit = new Temperature(TWU.Tc);
                    return Tcrit;
            }

            return new Temperature(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="MeABP"></param>
        /// <param name="SG"></param>
        /// <param name="MW"></param>
        /// <param name="thermo"></param>
        /// <return  s></return  s>
        public static Pressure CalcCritP(Temperature MeABP, double SG, double MW, ThermoDynamicOptions thermo)
        {
            switch (thermo.CritPMethod)
            {
                case enumCritPMethod.LeeKesler:
                    return LeeKesler.PCrit(SG, MeABP);

                case enumCritPMethod.RiaziDaubert0:
                    return RiaziDaubert.RD0_PCrit(SG, MeABP);

                case enumCritPMethod.RiaziDaubert1:
                    return RiaziDaubert.RD1_PCrit(SG, MeABP);

                case enumCritPMethod.RiaziDaubert2:
                    return RiaziDaubert.RD2_PCrit(SG, MeABP);

                case enumCritPMethod.Vetere:
                    return Vetere.CritP(SG, MeABP, MW);

                case enumCritPMethod.TWU:
                    TWU.Calc(MeABP, SG);
                    return TWU.Pc;
            }
            return 0;
        }
    }
}