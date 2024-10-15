using Extensions;
using MathNet.Numerics;
using System;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using Units.UOM;
using static alglib;

namespace ModelEngine.FCC.Petrof
{
    public class FCCFeedCharacterise
    {
        private double ri20, ri67;
        private Temperature meabp;
        private Temperature vabp;
        private double molwt;
        private double mols;
        private double whsv;
        private double nitrwtpct;
        private double ricorr, sgcorr;
        private double xfact;
        private double d1, d2, d3;
        private double ri0, ri100;
        private double pxp;
        private double wpara;
        private double ari;
        private double xabp;
        private double sul;
        private Density density;
        private double D1;
        private double D2;
        private double FDA;
        private double XFDA;
        private double WAROM;
        private double WPA;
        private double WNANB;
        private double WNB;
        private double WNAPH;
        private double RIAROM;
        private double CAMA;
        private double NapBenzene;
        private double CANB;
        private double Ratio;
        private double CANAPH;
        private double CASAM;
        private double RATIO;
        private double CAAROM;
        private double FRD1;
        private double FRD2;

        public double RI20 { get => ri20; set => ri20 = value; }
        public Temperature MEABP { get => meabp; set => meabp = value; }
        public Temperature VABP { get => vabp; set => vabp = value; }
        public double MOLWT { get => molwt; set => molwt = value; }
        public double Mols { get => mols; set => mols = value; }
        public double WHSV { get => whsv; set => whsv = value; }
        public double NITRWtpct { get => nitrwtpct; set => nitrwtpct = value; }
        public double RICORR { get => ricorr; set => ricorr = value; }
        public double SGCORR { get => sgcorr; set => sgcorr = value; }
        public double Xfact { get => xfact; set => xfact = value; }
        public double D3 { get => d3; set => d3 = value; }
        public double RI0 { get => ri0; set => ri0 = value; }
        public double RI100 { get => ri100; set => ri100 = value; }
        public double PXP { get => pxp; set => pxp = value; }
        public double WPARA { get => wpara; set => wpara = value; }
        public double Ari { get => ari; set => ari = value; }
        public double XABP { get => xabp; set => xabp = value; }
        public double SUL { get => sul; set => sul = value; }

        public FCCFeedCharacterise()
        {
        }

        public void Solve(Port_Material feed)
        {
        }

        public FCCFeedCharacterise(Components cc, MassFlow MF, DistPoints dist, double NWtpct, double SulWtpct)
        {
            FCCDimensions dim = new();
            cc = dist.ConvertToQuasiComps(cc);
            density = dist.DENSITY.BaseValue;
            meabp = dist.MeABP();
            vabp = Components.VABPShortForm(cc);
            xabp = meabp * 9 / 5 + 32;
            molwt = dist.APIMolWT(meabp, density);
            ri67 = 1 + 0.8447 * density.SG.Pow(1.2056) * (meabp + 273.16).Pow(-0.0557) * molwt.Pow(-0.0044);
            mols = MF * 1000000 / molwt;
            whsv = MF / dim.Volume;
            ricorr = ri20 - 0.0022 * SulWtpct - 0.005 * NWtpct;
            sgcorr = density.SG - 0.0064 * SulWtpct - 0.01 * nitrwtpct;
            xfact = 0.01 * meabp.Fahrenheit - 4;
            sul = SulWtpct;
        }

        public void HighParrafin()
        {
            d1 = ricorr - 1.475;
            ri100 = 1.398 + 0.01 * xfact - 0.00064 * xfact * xfact;
            d2 = ri100 - 1.475;
            d3 = d2 / d1;

            if (d3 > 0.00001)
                d3 *= 10000000000;
            else
                d3 = 1;

            pxp = 12.3 + 8.7 * 10D.Pow(d3);

            if (ricorr < 1.475)
                wpara = pxp;
            else
                wpara = LowParrafin();
        }

        public double LowParrafin()
        {
            d1 = 1.64 - 0.0109 * xfact;
            ri100 = ricorr - d1;
            d2 = 1.475 - d1;
            wpara = 8 + 13 * ri100 / d2;
            return wpara;
        }

        public void Aromatics()
        {
            ri0 = 0.924 + 0.62 * sgcorr - 0.8 * (sgcorr - 0.783).Pow(2);
            ri100 = 0.62 * (sgcorr - 0.9) + 1.51;
        }

        public void FractionDistance()
        {
            FRD1 = ricorr - ri0;
            FRD2 = ri100 - ri0;
            FDA = FRD1 / this.d2;
            XFDA = FRD1 / this.d2 + 1;
            WAROM = -1 + 14.918712 - 50.69636 * XFDA + 69.723218 * XFDA * XFDA - 45.131829 * XFDA.Pow(3) + 13.710609 * XFDA.Pow(4) - 1.5228703 * XFDA.Pow(5);
            WPA = wpara + WAROM;

            WNANB = wpara + WAROM;
            WNB = 5 + 0.3 * WAROM;
            WNAPH = WNANB - WNB;
            RIAROM = RICORR + 0.04 * (1 - WAROM / 100);
            CAMA = -0.1024 * (0.01 * XABP - 4) + 1.739;
            CAAROM = (0.2085 * RIAROM - 0.37) * (0.01 * XABP - 4) + (1.342 * RIAROM - 0.2);


           //Naphta Benzene
            NapBenzene = -0.000704 * XABP + 1.935;
            CANB = Math.Pow(10,NapBenzene);
            Ratio = 0.937 * RICORR - 1.262;
            CANAPH = RATIO * CANB;
            CASAM = CAAROM * WAROM / 100 + CANB * WNB / 100 + CANAPH * WNAPH / 100;

        }
    }
}
    
