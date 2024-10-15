using ModelEngine;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units.UOM;

namespace ModelEngine
{
    public class TotalCorrelation
    {
        double mw;
        double rI20;
        double rI60;
        double ca;
        double h2;

        static public double mw_s;
        static public double rI20_s;
        static public double rI60_s;
        static public double ca_s;
        static public double h2_s;

        public TotalCorrelation()
        {
            //SolveMW(SG,MEABP,VABP,Sul,Visc,MW);
        }

        public double MW { get => mw; set => mw = value; }
        public double RI20 { get => rI20; set => rI20 = value; }
        public double RI60 { get => rI60; set => rI60 = value; }
        public double Ca { get => ca; set => ca = value; }
        public double H2 { get => h2; set => h2 = value; }

        public void SolveAP(Density density, Temperature MEABP, Temperature VABP, double Sul, double Visc, Temperature anilinePoint)
        {
           mw = 0.0078312 * density.SG.Pow(-0.0978) * MEABP.Celsius.Pow(0.1238) * VABP.Celsius.Pow(1.6971);
           rI20 = 1 + 0.8447 * density.SG.Pow(1.2056) * (MEABP.Celsius + 273.15).Pow(-0.0557) * mw.Pow(-0.0044);
           rI60 = 1 + 0.8156 * density.SG.Pow(1.2392) * (MEABP + 273.15).Pow(-0.0576) * mw.Pow(-0.0007);
           ca = -814.136 + 635.192 * rI20 - 129.266 * density.SG + 0.013 * mw - 0.34 * Sul - 6.872 * Math.Log(Visc);
           h2 = 52.825 - 14.26 * rI20 - 21.329 * density.SG - 0.0024 * mw - 0.052 * Sul + 0.757 * Math.Log(Visc);
        }

        public void SolveMW(Density density, Temperature MEABP, Temperature VABP, double Sul, double Visc, double MW)
        {
            mw = MW;
            rI20 = 1 + 0.8447 * density.SG.Pow(1.2056) * (MEABP + 273.15).Pow(-0.0557) * mw.Pow(-0.0044);
            rI60 = 1 + 0.8156 * density.SG.Pow(1.2392) * (MEABP + 273.15).Pow(-0.0576) * mw.Pow(-0.0007);
            ca = -814.136 + 635.192 * rI20 - 129.266 * density.SG + 0.013 * mw - 0.34 * Sul - 6.872 * Math.Log(Visc);
            h2 = 52.825 - 14.26 * rI20 - 21.329 * density.SG - 0.0024 * mw - 0.052 * Sul + 0.757 * Math.Log(Visc);
        }

        public void Solve(Density SG, Temperature MEABP, Temperature VABP, double Sul, double MW)
        {
            mw = MW;
            double T50_R = MEABP.Rankine;
            double KFact = T50_R.Pow(1 / 3D)/SG.SG;
            double Visc = this.Visc(T50_R, KFact);

            rI20 = 1 + 0.8447 * SG.Pow(1.2056) * (MEABP + 273.15).Pow(-0.0557) * mw.Pow(-0.0044);
            rI60 = 1 + 0.8156 * SG.Pow(1.2392) * (MEABP + 273.15).Pow(-0.0576) * mw.Pow(-0.0007);
            ca = -814.136 + 635.192 * rI20 - 129.266 * SG + 0.013 * mw - 0.34 * Sul - 6.872 * Math.Log(Visc);
            h2 = 52.825 - 14.26 * rI20 - 21.329 * SG - 0.0024 * mw - 0.052 * Sul + 0.757 * Math.Log(Visc);
        }

        public static void SolveTotal(Density SG, Temperature MEABP, Temperature VABP, double Sul, double MW)
        {
            double mw = MW;
            double T50_R = MEABP.Rankine;
            double KFact = T50_R.Pow(1 / 3D) / SG.SG;

            double A1 = 3.49310E+01 - 8.84387E-02 * T50_R + 6.73513E-05 * T50_R.Pow(2) - 1.01394E-08 * T50_R.Pow(3);
            double A2 = -2.92649 + 6.98405E-03 * T50_R - 5.09947E-06 * T50_R.Pow(2) + 7.49378E-10 * T50_R.Pow(3);
            double A3 = -1.35579 + 8.16059E-04 * T50_R + 8.38505E-07 * T50_R.Pow(2);
            double A4 = A1 + A2 * KFact;
            double A5 = 10D.Pow(A3);
            double A6 = 10D.Pow(A4);
            double V100 = A5 + A6;
            double P1 = T50_R * V100;
            double A7 = Math.Log10(P1);
            double A8 = -1.92353E+00 + 2.41071E-04 * T50_R + 5.11300E-01 * A7;

            double Visc = Math.Pow(10.0, A8);

            rI20_s = 1 + 0.8447 * SG.Pow(1.2056) * (MEABP + 273.15).Pow(-0.0557) * mw.Pow(-0.0044);
            rI60_s = 1 + 0.8156 * SG.Pow(1.2392) * (MEABP + 273.15).Pow(-0.0576) * mw.Pow(-0.0007);
            ca_s = -814.136 + 635.192 * rI20_s - 129.266 * SG + 0.013 * mw - 0.34 * Sul - 6.872 * Math.Log(Visc);
            h2_s = 52.825 - 14.26 * rI20_s - 21.329 * SG - 0.0024 * mw - 0.052 * Sul + 0.757 * Math.Log(Visc);
        }

        public double CA(Density density, double MW, MassFraction S, double RI20, KinViscosity v)
        {
            return -814.136 + 635.192 * RI20 - 129.266 * density.SG + 0.013 * MW - 0.34 * S - 6.872 * Math.Log(v);
        }

        public double H2wt(Density density, double MW, MassFraction S, double RI20, KinViscosity v, Temperature MEABP)
        {
            double rI20 = 1 + 0.8447 * density.SG.Pow(1.2056) * (MEABP + 273.15).Pow(-0.0557) * mw.Pow(-0.0044);
            double rI60 = 1 + 0.8156 * density.SG.Pow(1.2392) * (MEABP + 273.15).Pow(-0.0576) * mw.Pow(-0.0007);
            double ca = -814.136 + 635.192 * rI20 - 129.266 * density.SG + 0.013 * mw - 0.34 * S - 6.872 * Math.Log(v);
            double h2 = 52.825 - 14.26 * rI20 - 21.329 * density.SG - 0.0024 * mw - 0.052 * S + 0.757 * Math.Log(v);
            return h2;
        }

        public double MWt(Density density, Temperature MEABP, Temperature VABP)
        {
            return 0.0078312 * density.SG.Pow(-0.0978) * MEABP.Celsius.Pow(0.1238) * VABP.Celsius.Pow(1.6971);
        }

        public double RI20t(Density density, Temperature MEABP, Temperature VABP, double MW)
        {
            return 1 + 0.8447 * density.SG.Pow(1.2056) * (MEABP + 273.15).Pow(-0.0557) * MW.Pow(-0.0044);
        }

        public double RI60t(Density density, Temperature MEABP, Temperature VABP, double MW)
        {
            return 1 + 0.8156 * density.SG.Pow(1.2392) * (MEABP + 273.15).Pow(-0.0576) * mw.Pow(-0.0007); 
        }

        public KinViscosity Visc(double T50_R, double KFact)
        {
            double A1 = 3.49310E+01 - 8.84387E-02 * T50_R + 6.73513E-05 * T50_R.Pow(2) - 1.01394E-08 * T50_R.Pow(3);
            double A2 = -2.92649 + 6.98405E-03 * T50_R - 5.09947E-06 * T50_R.Pow(2) + 7.49378E-10 * T50_R.Pow(3);
            double A3 = -1.35579 + 8.16059E-04 * T50_R + 8.38505E-07 * T50_R.Pow(2);
            double A4 = A1 + A2 * KFact;
            double A5 = 10D.Pow(A3);
            double A6 = 10D.Pow(A4);
            double V100 = A5 + A6;
            double P1 = T50_R * V100;
            double A7 = Math.Log10(P1);
            double A8 = -1.92353E+00 + 2.41071E-04 * T50_R + 5.11300E-01 * A7;

            return Math.Pow(10.0, A8);

        }
    }
}
