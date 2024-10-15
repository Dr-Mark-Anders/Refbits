using Extensions;
using System;

namespace ModelEngine.FCC.Petrof
{
    public class FCCCrackFactor
    {
        private double CATERM;
        private double ABPTERM;
        private double N2TERM;
        private double CRAC;

        public double CATERM1 { get => CATERM; set => CATERM = value; }
        public double ABPTERM1 { get => ABPTERM; set => ABPTERM = value; }
        public double N2TERM1 { get => N2TERM; set => N2TERM = value; }
        public double CRAC1 { get => CRAC; set => CRAC = value; }

        public void Solve(double Ca, double XABP, double NIT)
        {
            CATERM = 1.4592477 - (0.08810098 * Ca) + (0.0024612 * Ca * Ca) -(0.0000232873 * Ca * Ca * Ca);
            ABPTERM = Math.Exp(-0.42148 + (0.000511 * XABP));
            N2TERM = Math.Exp(2.0654 - (0.29597 * Math.Log(NIT * 10000)));
            CRAC = CATERM * ABPTERM * N2TERM;
        }
    }

    public class FCCCokingFactor
    {
        private  double CATERM;
        private  double ABPTERM;
        private  double N2TERM;
        private  double PATERM;
        private  double CRAC;

        public double CATERM1 { get => CATERM; set => CATERM = value; }
        public double ABPTERM1 { get => ABPTERM; set => ABPTERM = value; }
        public double N2TERM1 { get => N2TERM; set => N2TERM = value; }
        public double PATERM1 { get => PATERM; set => PATERM = value; }
        public double CRAC1 { get => CRAC; set => CRAC = value; }

        public void Solve(double Ca, double XABP, double NIT, double WPARA)
        {
            CATERM = 1 + (0.0077828 * Ca);
            ABPTERM = XABP.Pow(-0.13051);

            if (NIT < 0.07)
                N2TERM = 1;
            else
                N2TERM = 1 + (1.982388 * (NIT - 0.07));

            if (WPARA < 21)
                PATERM = Math.Exp(-0.0840343 * WPARA);
            else
                PATERM = 0.1712348 - (0.00440747 * (WPARA - 21));

            CRAC = 50.515 * CATERM * ABPTERM * N2TERM * PATERM;
        }
    }
}