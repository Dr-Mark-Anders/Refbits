using System;

namespace RefReactor
{
    public class RefRateEquation
    {
        public const double Rgas = 8.314;

        private double F_Ea, F_Ko, R_Ea, R_ko;
        public ReformerReactionType RateType;

        public RefRateEquation()
        {
        }

        public RefRateEquation(double F_Ko, double F_Ea, double R_ko, double R_Ea, ReformerReactionType type)
        {
            this.F_Ea = F_Ea * 1000 / Rgas;
            this.F_Ko = F_Ko;
            this.R_Ea = R_Ea * 1000 / Rgas;
            this.R_ko = R_ko;
            this.RateType = type;
        }

        public double Rate(double Tk)
        {
            return Math.Exp(F_Ko - F_Ea / Tk);
            //return   (F_Ko - F_Ea / Tk).exp1();
        }

        public double ReverseRate(double Tk)
        {
            return Math.Exp(R_ko - R_Ea / Tk);
            //return   (R_ko - R_Ea / Tk).exp1();
        }
    }
}