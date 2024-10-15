using ModelEngine;
using System;
using Units.UOM;



namespace ModelEngine
{
    public class Arrhenius
    {
        private double ae = 1, k = 1;
        private double aer = 1, kr=1;
        ReformerReactionType rtype = ReformerReactionType.none;

        public Arrhenius()
        {
        }

        public Arrhenius(double K, double Ae, double Kr, double Aer)
        {
            k = K;
            ae = Ae;
            kr = Kr;
            aer = Aer;
        }


        public Arrhenius(double K, double Ae, double Kr, double Aer, ReformerReactionType rtype = ReformerReactionType.none)
        {
            k = K;
            ae = Ae;
            kr = Kr;
            aer = Aer;
            this.rtype = rtype;
        }

        public double Ae { get => ae; set => ae = value; }
        public double K { get => k; set => k = value; }

        public double Aer { get => aer; set => aer = value; }
        public double Kr { get => kr; set => kr = value; }
        public ReformerReactionType Rtype { get => rtype; set => rtype = value; }

        public double Rate(Temperature T)
        {
            return K * Math.Exp(Ae / (ThermodynamicsClass.Rgas * T));
        }
    }
}