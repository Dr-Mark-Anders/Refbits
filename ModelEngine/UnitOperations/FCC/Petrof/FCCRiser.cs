using Extensions;
using System;
using Units.UOM;

namespace ModelEngine.FCC.Petrof
{
    public class FCCRiser
    {
        private double CATOIL = 7.919;
        private double CRC = 0.03;
        private double CCR;
        private double CCRL;
        private double CRCFAC;
        private double XCA;
        private double POWER;
        private double ZTERM;
        private double TAVG;
        private double TTERM;
        private double TOTCONV;
        private double OilPP;
        private double PTERM;
        private double X;
        private double HXOUT;
        private double MAT;
        private double KINMAT;
        private double CONV;
        private double ATMPSI;
        private double MOLSTM;
        private double MOLIN;
        private double MOLOUT;
        private double RCTCO;

        private double CARCRC;
        private double KINCON;
        private double KINCONPOWER;
        private double CARCON;
        private double CARRT;
        private double CARPP;
        private double CARMAT;
        private double CARDIS;
        private double TFCARF;
        private double CATCAR;

        public void Solve(MassFlow FeedRate, double Ca, double CRAC, Temperature Tavg, double MAT, double molin)
        {
            CRC = 0.03; // coke on regen catalyst
                        // 
            CCR = CATOIL* FeedRate.st_hr;
            CCRL = CRC * 2000;
            
            if (CRC < 1.37026)
                CRCFAC = 1 - 0.6933 * CRC;
            else
                CRCFAC = 0.05;

            XCA = Ca;

            POWER = 0.420697 + 0.0113914 * XCA - 0.000275363 * XCA.Pow(2) + 0.00000226715 * XCA.Pow(3);
            ZTERM = CCRL.Pow((1 - POWER)) / (((1 - POWER) / POWER) * CATOIL * KINMAT * CRAC * CRCFAC * 2.06 * RCTCO);
            TTERM = Math.Exp(6.9601 - 19500 / 1.987 / (TAVG + 460));
            TOTCONV = 85;
            PTERM = OilPP.Pow(1 / 2);
            X = TOTCONV / 10;
            HXOUT = ((ZTERM * X) / (PTERM * TTERM)).Pow((1 / (1 - POWER)));
            KINMAT = MAT / (150 - MAT);
            CONV = TOTCONV / 100;
            ATMPSI = 20;
            MOLSTM = 1;
            MOLOUT = MOLIN / 0.7;
            RCTCO = 6.23;

            CARCRC = 0.911323 + 0.8839 * CRC;
            KINCON = TOTCONV / (100 - TOTCONV);
            KINCONPOWER = 0.6907 + 0.02257 * KINCON - (0.00214 * KINCON * KINCON);
            CARCON = KINCON.Pow(KINCONPOWER);

            double WSP = 600 * 9 / 5 + 460;
            CARRT = Math.Exp(-6.42 + (18000 / 1.987 / WSP));
            CARPP = OilPP.Pow(0.25);
            CARCRC = 0.911323 + 0.8839 * CRC;
            CARMAT = 1 / KINMAT.Pow(0.7);
            double REALST = 1;
            CARDIS = 1 - (1.488 - 2 * REALST) + (0.00072446 * REALST * REALST);
            TFCARF = 1;
            CATCAR = CARCON * CARRT * CARPP * CARCRC * CARMAT * CARDIS * TFCARF;
        }
    }
}