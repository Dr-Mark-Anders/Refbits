using Extensions;
using System;

namespace ModelEngine
{
    //  Estimate V from table, solve with bounded upper and lower V for Z

    public static partial class LeeKesler
    {
        private const double A0 = 356.44;
        private const double A1 = 29.72;
        private const double A2 = 295.02;
        private const double A3 = 248.46;
        private const double B0 = 146.24;
        private const double B1 = 77.62;
        private const double B2 = 2.772;
        private const double B3 = 301.42;
        private const double D1 = 253.87;
        private const double D2 = 56.487;
        private const double D4 = 2.95;

        public static double[] GetIdealVapCpCoefficients(double Omega, double Wk, double SG) // Lee Kesler 1976 "Improve enthlpy of fractions"
        {
            double A0, A1, A2;
            double B0, B1, B2, C;
            double[] RES = new double[6];

            A0 = -1.41779 + 0.11828 * Wk;
            A1 = -(6.9972 - 8.69326 * Wk + 0.27715 * Wk.Sqr()) / 10000;
            A2 = -2.2582 * 1E-6;

            B0 = 1.09223 - 2.48245 * Omega;
            B1 = -(3.434 - 7.14 * Omega) / 1000;
            B2 = -(7.2661 - 9.2561 * Omega) * 1E-7;

            if (Wk > 12.8 || Wk < 10)
                C = 0;
            else
                C = ((12.8 - Wk) * (10 - Wk) / (10 * Omega)).Pow(2);

            //BTU/LB/R

            RES[0] = A0 - C * B0;
            RES[1] = A1 - C * B1;
            RES[2] = A2 - C * B2;
            RES[3] = 0;
            RES[4] = 0;
            RES[5] = 0;

            return RES;
        }

        /// <summary>
        /// Lee Kesler Idea Vapour H Calculation, BTU/LB
        /// </summary>
        /// <param name="K"></param>
        /// <param name="SG"></param>
        /// <return  s></return  s>
        public static double[] GetIdealVapEnthalpyCoefficients(double Wk, double SG)
        {
            double B1, B2, B3, B4;
            double[] RES = new double[6];

            if (SG > 0.885 || SG < 0.7 || Wk > 12.8 || Wk < 11)
                B4 = 0;
            else
                B4 = ((12.8 / Wk - 1) * (1 - 10 / Wk) * (SG - 0.885) * (SG - 0.7) * 10000).Pow(2);

            B1 = (-356.44 + 29.72 * Wk + B4 * (295.02 - 248.46 / SG)) / 1000;
            B2 = ((-146.24 + (77.62 - 2.772 * Wk) * Wk - B4 * (301.42 - 253.87 / SG)) / 1000000);
            B3 = (-56.487 - 2.95 * B4) / 1000000000;

            //BTU/LB/R
            RES[0] = 0;
            RES[1] = B1 * 4.1868;   //4.1868;
            RES[2] = B2 * 4.1868 * 1.8;   //4.1868 * 1.8;
            RES[3] = B3 * 4.1868 * 1.8 * 1.8;   //4.1868 * 1.8 * 1.8; // convert to kj/kg/C
            RES[4] = 0;
            RES[5] = 0;

            return RES;
        }

        public static double[] GetIdealVapEnthalpyCoefficientsOld(double K, double SG)
        {
            double B1, B2, B3, B4;
            double[] RES = new double[6];

            if (SG > 0.885 || SG < 0.7 || K > 12.8 || K < 11)
                B4 = 0;
            else
                B4 = ((12.8 / K - 1) * (1 - 10 / K) * (SG - 0.885) * (SG - 0.7) * 10000).Pow(2);

            B1 = (-356.44 + 29.72 * K + B4 * (295.02 - 248.46 / SG)) / 1000;
            B2 = ((-146.24 + (77.62 - 2.772 * K) * K - B4 * (301.42 - 253.87 / SG)) / 1000000);
            B3 = (-56.487 - 2.95 * B4) / 1000000000;

            RES[0] = 0;
            RES[1] = B1 * 2.326 * 1.8;
            RES[2] = B2 * 2.326 * 1.8 * 1.8;
            RES[3] = B3 * 2.326 * 1.8 * 1.8 * 1.8; // convert to kj/kg
            RES[4] = 0;
            RES[5] = 0;

            return RES;
        }

        // Tr, Pr, Liquid
        public static double ENTH80(double meabp, double sg, double tc)
        {
            double TRank, MABPRank, Wk, E80;
            double A1, A2, A3;
            MABPRank = meabp * 1.8 + 32 + 460;
            Wk = Math.Pow(MABPRank, (1.0 / 3.0)) / sg;
            TRank = tc * 9 / 5 + 32 + 460;

            A1 = (-1171.26 + (23.722 + 24.907 * sg) * Wk + (1149.82 - 46.535 * Wk) / sg) / 1000;
            A2 = (1 + 0.82463 * Wk) * (56.086 - 13.817 / sg) / 1000000;
            A3 = -(1 + 0.82463 * Wk) * (9.6757 - 2.3653 / sg) / 1000000000;

            E80 = (A1 * (TRank - 259.7)
                + A2 * (Math.Pow(TRank, 2) - Math.Pow(259.7, 2))
                + A3 * (Math.Pow(TRank, 3) - Math.Pow(259.7, 3)));
            return E80;
        }

        public static double IdealVapEnthalpyAbsolute(double tc, double p, double CritT, double CritP, double MeABP, double sg)
        {
            double A1, A2, A3, B1, B2, B3, B4;
            double tf, K;
            double MW, Tr, X, OMEGA;
            double VE, TRank, PCritTRank, PCriticalPress, Enth80, Treduced, Preduced;
            double ppsi, PCritTRank70;
            double MeABP_F;
            double VapPress;

            ppsi = p * 14.7;

            MeABP_F = MeABP * 1.8 + 32;
            K = MeABP.R().Pow(1f / 3f) / sg;
            tf = tc * 9 / 5 + 32;
            TRank = tf + 460;

            A1 = (-1171.26 + (23.722 + 24.907 * sg) * K + (1149.82 - 46.535 * K) / sg) / 1000;
            A2 = (1 + 0.82463 * K) * (56.086 - 13.817 / sg) / 1000000;
            A3 = -(1 + 0.82463 * K) * (9.6757 - 2.3653 / sg) / 1000000000;

            if (sg > 0.885)
                B4 = 0;
            else
                B4 = ((12.8 / K - 1) * (1 - 10 / K) * (sg - 0.885) * (sg - 0.7) * 10000).Pow(2);

            B1 = (-356.44 + 29.72 * K + B4 * (295.02 - 248.46 / sg)) / 1000;
            B2 = ((-146.24 + (77.62 - 2.772 * K) * K - B4 * (301.42 - 253.87 / sg)) / 1000000);
            B3 = (-56.487 - 2.95 * B4) / 1000000000;

            MW = 20.486 * (Math.Exp(1.165 / 10000 * (MeABP_F + 460) - 7.78712 * sg + 1.1582 / 1000 *
                (MeABP_F + 460) * sg)) * Math.Pow((MeABP_F + 460), 1.26007) * Math.Pow(sg, 4.98308);

            PCritTRank = CritT.R();//  (API_PSEUD_TC(MeABP, sg) + 273.15) * 9 / 5;
            PCriticalPress = CritP;// API_PSEUD_PC(MeABP, sg);

            PCritTRank70 = PCritTRank * 0.7;

            Treduced = TRank / PCritTRank;
            Preduced = ppsi / PCriticalPress;

            Tr = (tf + 460) / PCritTRank;

            //VP1 = VP(MeABP, tf, K);
            //VP70 = VP(MeABP, (PCritTRank * 0.7) * 5 / 9 - 273.15, K);

            X = (((MeABP_F + 460) / PCritTRank70 - 0.0002867 * (MeABP_F + 460))) / (748.1 - 0.2145 * (MeABP_F + 460));

            TRank = tf + 460;

            if (X > 0.0022)
                VapPress = Math.Pow(10, (3000.538 * X - 6.76156) / (43 * X - 0.987672));
            else if (X > 0.0013)
                VapPress = Math.Pow(10, (2663.129 * X - 5.994296) / (95.76 * X - 0.972546));
            else
                VapPress = Math.Pow(10, (2770.085 * X - 6.412631) / (36 * X - 0.989679));

            OMEGA = -Math.Log10(VapPress * 14.7 / (PCriticalPress * 760)) - 1;

            Enth80 = ENTH80(MeABP, sg, ((PCritTRank * 0.8) / 1.8 - 273.15));

            VE = Enth80 + B1 * (TRank - 0.8 * PCritTRank)
                        + B2 * (Math.Pow(TRank, 2) - 0.64 * Math.Pow(PCritTRank, 2))
                        + B3 * (Math.Pow(TRank, 3) - 0.512 * Math.Pow(PCritTRank, 3))
                        + 1.986 * PCritTRank
                        / MW * (4.507 + 5.266 * OMEGA - PressureEffect(Preduced, Treduced, OMEGA));   // no Pressure   effect

            return VE * 1.0550559 / 0.45359237;
        }

        public static double PressureEffect(double Pr, double Tr, double Omega)
        {
            double res;  //=-Pr*((0.1445+0.073*Omega)-(0.66-0.92*Omega)*Tr^-1-(0.4155+1.5*Omega)*Tr^-2-(0.0484+0.388*Omega)*Tr^-3-0.0657*Omega*Tr^-8)
            res = -Pr * ((0.1445 + 0.073 * Omega) - (0.66 - 0.92 * Omega) / Tr - (0.4155 + 1.5 * Omega) / (Tr * Tr) -
                (0.0484 + 0.388 * Omega) * Math.Pow(Tr, -3) - 0.0657 * Omega * Math.Pow(Tr, -8));
            return res;
        }
    }
}