using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    public partial class PropertyCalcs
    {
        public static SpecificMolarVolume IdealGasVolume(Pressure p, Temperature T)
        {
            SpecificMolarVolume res = 22.4 * (T.Kelvin / 273.15) / p.ATM;
            return res;
        }

        public static SpecificEnergy LowerHeatValue(Components cc)  // Warning Haven't done Pure Comps yet
        {
            SpecificEnergy e;
            double api = cc.API;
            double Se = PropertyCalcs.Sulfur(cc) - 0.0015 * api * api - 0.1282 * api + 2.9479;  // minus typical sulfur content.
            double X = 16796 + 54.5 * api - 0.217 * Math.Pow(api, 2) - 0.0019 * Math.Pow(api, 3);
            double res = X * (1 - 0.01 * Se) + 40.5 * Se;
            e = new SpecificEnergy(res, SpecificEnergyUnit.btu_lb);
            return e;
        }

        public static SpecificEnergy HigherHeatValue(Components cc)  // Warning Haven't done Pure Comps yet
        {
            SpecificEnergy e;
            double api = cc.API;
            var res = 17672 + 66.6 * api - 0.316 * api.Pow(2) - 0.0014 * api.Pow(3);
            e = new SpecificEnergy(res, SpecificEnergyUnit.btu_lb);
            return e;
        }

        /// <summary>
        /// total method
        /// </summary>
        /// <returns></returns>
        public static double H2Wtpct(Components cc)  // Warning Haven't done Pure Comps yet
        {
            TotalCorrelation.SolveTotal(cc.SG(), cc.MeABP(), cc.VolAveBP(), PropertyCalcs.Sulfur(cc), cc.MW());
            return TotalCorrelation.h2_s;
        }

        public static Density ActLiqDensity(Components cc, Pressure P, Temperature T)
        {
            if (cc.Count == 1 && cc[0] is WaterSteam steam)
            {
                return steam.ActLiqDensity(P, T);
            }
            return DensityMethods.Density(cc.Thermo.Density, cc, T);
        }

        public static double CetaneNumber(double CI)
        {
            return 5.28 + 0.371 * CI + 0.0112 * CI * CI;
        }

        public static double CtoH(Components cc)
        {
            double C = 0, H = 0;
            for (int pc = 0; pc < cc.Count; pc++)
            {
                if (cc[pc].Props[(int)enumAssayPCProperty.CTOHRATIO] != -99999)
                {
                    C += cc[pc].Props[(int)enumAssayPCProperty.CTOHRATIO] * (100.0 - (cc[pc].Props[(int)enumAssayPCProperty.NITROGEN])
                    - cc[pc].Props[(int)enumAssayPCProperty.SULFUR])
                    / (1 + cc[pc].Props[(int)enumAssayPCProperty.CTOHRATIO]);
                    H += (100.0 - cc[pc].Props[(int)enumAssayPCProperty.NITROGEN] - cc[pc].Props[(int)enumAssayPCProperty.SULFUR])
                    / (1 + cc[pc].Props[(int)enumAssayPCProperty.CTOHRATIO]);
                }
            };
            double CH = C / H;
            return CH;
        }

        public static double RefractiveIndex(Components cc)
        {
            double Tbmix = cc.TbMixK();
            double M = cc.MW();
            double I;
            //double I = 0.3773*Tbmix.Power(0.02269)*SG.Power(0.9182); // Riazi-Daubert 2.115

            if (M < 300)
            {
                I = 2.34348e-2 * Math.Exp(7.029E-4 * Tbmix + 2.468 * cc.SG() - 1.0267E-3 * Tbmix * cc.SG()) * Tbmix.Pow(0.0572) * cc.SG().Pow(-0.720); // Riazi-Daubert 2.116 MW 70-300
            }
            else
            {
                I = 1.2419e-2 * Math.Exp(7.272e-4 * M + 3.3223 * cc.SG() - 8.867E-4 * M * cc.SG()) * M.Pow(0.006438) * cc.SG().Pow(-1.6117); // MW > 300 > C20  //70 - 700
            }
            double Ri = ((1 + 2 * I) / (1 - I)).Pow(0.5);

            return Ri;
        }

        public static double VapourPressure(Components cc, Temperature T)
        {
            return LeeKesler.PSat(cc, T);
        }

        public static double LiqViscosity(Components cc, Temperature T)
        {
            double Nl1, Nl2, Nlc, Visc = 0, Tr = cc.TCritMix() / T;
            switch (cc.Thermo.ViscLiqMethod)
            {
                case enumViscLiqMethod.LetsouStiel:
                    {
                        Nl1 = 0.015174 - 0.02135 * Tr + 0.0075 * Tr.Pow(2);
                        Nl2 = 0.042552 - 0.07674 * Tr + 0.0340 * Tr.Pow(2);
                        Nlc = Nl1 + cc.OmegaMix() * Nl2;

                        Visc = Nlc / (cc.TCritMix().Rankine.Pow(1 / 6) / (cc.MW().Pow(0.5) * cc.PCritMix().PSIA.Pow(2 / 3)));
                        break;
                    }
                case enumViscLiqMethod.ElyHanley:
                    {
                        foreach (BaseComp bc in cc) // ASTM D7152
                        {
                            Visc += (14.534 * Math.Log(Math.Log(bc.Visc + 0.8)) + 10.975) * bc.STDLiqVolFraction;
                        }
                        Visc = Math.Exp(Math.Exp((Visc - 10.975) / 14.534)) - 0.8;

                        break;
                    }
            }
            return Visc;
        }

        public double VapViscosity(Components cc)
        {
            double Visc = 0;
            switch (cc.Thermo.ViscVapMethod)
            {
                case enumViscVapMethod.Lucas:
                    break;

                case enumViscVapMethod.YoonThodos: // gases not complete
                    //Nl1 = 1 + 46.1 * Tr(T).Pow(0.618) - 20.4 * Math.Exp(-0.449 * Tr(T)) + 19.4 * Math.Exp(-4.058 * Tr(T));
                    //Visc = Nl1 / (TCritMix().Pow(1 / 6) / (MW().Pow(0.5) * (0.987 * PCritMix().Pow(2 / 3))));
                    Visc = -999;
                    break;

                case enumViscVapMethod.ElyHanley:
                    Visc = -999;
                    break;
            }
            return Visc;
        }

        public static double MassSum(Components components, int Prop)
        {
            double temp = 0;
            foreach (BaseComp pc in components)
            {
                if (pc.Props != null)
                {
                    if (pc.Props[Prop] != -99999)
                    {
                        temp += pc.Props[Prop] * pc.MassFraction;
                    }
                }
            }
            return temp;
        }

        public static double VolSum(Components components, int Prop)
        {
            double temp = 0;
            foreach (BaseComp pc in components)
            {
                if (pc.Props != null)
                {
                    if (pc.Props[Prop] != -99999)
                    {
                        temp += pc.Props[Prop] * (pc.MassFraction / pc.SG_60F);
                    }
                }
            }
            return temp;
        }

        public static double MolSum(Components components, int Prop)
        {
            double temp = 0;
            foreach (BaseComp pc in components)
            {
                if (pc.Props != null)
                {
                    if (pc.Props[Prop] != -99999)
                    {
                        temp += pc.Props[Prop] * (pc.MoleFraction);
                    }
                }
            }
            return temp;
        }

        public static double ThermalConductivity(Components cc, Pressure P, Temperature T, Quality Q)
        {
            double res;
            switch (cc.Thermo.ThermcondMethod)
            {
                case enumThermalConductivity.ElyHanley:
                    res = cc.TRAPP_Update(P, T).Item1;
                    break;

                case enumThermalConductivity.API_1981_3_12A3_1:
                    if (Q == 0)
                        res = API_3_12A3_1.LiquidThermalConductivity(cc.MeABP());
                    else
                        res = API_3_12A3_1.VapourThermalConductivity(T, cc.MW());
                    break;

                default:
                    res = double.NaN;
                    break;
            }
            return res;
        }

        public static double Sulfur(Components components)
        {
            double res = 0;
            foreach (BaseComp item in components)
            {
                if (item.Properties.ContainsKey(enumAssayPCProperty.SULFUR))
                    res += item.Properties[enumAssayPCProperty.SULFUR] * item.MassFraction;
            }

            return 0;
        }

        //ASTM D341
        public static double ViscT(Components components, double T)
        {
            double VT1 = ViscPC(components, (int)enumAssayPCProperty.VIS20);
            double VT2 = ViscPC(components, (int)enumAssayPCProperty.VIS40);
            double X1, X2;
            double T1 = 50, T2 = 100;
            if (VT1 < 2.0)
            {
                X1 = Math.Log10(Math.Log10(VT1 + 0.7 + Math.Exp(-1.47 - 1.84 * VT1 + 0.51 * VT1 * VT1)));
            }
            else
            {
                X1 = Math.Log10(Math.Log10(VT1 + 0.7));
            }
            if (VT1 < 2.0)
            {
                X2 = Math.Log10(Math.Log10(VT2 + 0.7 + Math.Exp(-1.47 - 1.84 * VT2 + 0.51 * VT2 * VT2)));
            }
            else
            {
                X2 = Math.Log10(Math.Log10(VT2 + 0.7));
            }

            double Y1 = Math.Log10(T1 + 273);
            double Y2 = Math.Log10(T2 + 273);

            double A = (X1 - X2) / (Y1 - Y2);
            double B = X1 - Y1 * A;
            double C = B + A * Math.Log10(T + 273);
            double D = Math.Pow(10, Math.Pow(10, C)) - 0.7;
            double Vt;
            if (D < 2.0)
            {
                Vt = D - Math.Exp(-0.7487 - 3.295 * D + 0.6119 * D * D - 0.3193 * D * D * D);
            }
            else
            {
                Vt = D;
            }
            return Vt;
        }

        public static double ViscPC(Components components, int Prop)
        {
            bool MassBased = true;
            double VB = 0;
            for (int pc = 0; pc < components.Count; pc++)
            {
                if (components[pc].Props[Prop] != -99999)
                {
                    double Visc = components[pc].Props[Prop];
                    double VBN;
                    if (Visc <= 0.21)
                        VBN = -56.0;
                    else
                        VBN = (23.1 + 33.47 * Math.Log10(Math.Log10(Visc + 0.8)));

                    if (MassBased)
                        VB += VBN * components[pc].STDLiqVolFraction * components[pc].Props[(int)enumAssayPCProperty.SG];
                    else
                        VB += VBN * components[pc].STDLiqVolFraction;
                }
            }

            /* if (MassBased)
             {
                 VB /= this.MF;
             }
             else
             {
                 VB /= this.VF;
             }*/

            double VS;
            if (VB < -56)
                VS = 0.21;
            else
            {
                double L = (VB - 23.1) / 33.47;
                VS = Math.Pow(10, Math.Pow(10, L)) - 0.8;
            }
            return VS;
        }

        // ASTM D1159
        public static double BromineNo(Components cc)
        {
            double Corr;
            double OleMwt;

            OleMwt = 59.359 + 0.29045 * cc.TBP[5].BP + 0.001063 * cc.TBP[5].BP * cc.TBP[5].BP
                + 1.5789e-7 * cc.TBP[5].BP * cc.TBP[5].BP * cc.TBP[5].BP;

            double delta = cc.TBP[10].BP - cc.TBP[0].BP;
            if (delta > 125)
            {
                Corr = 0.7;
            }
            else
            {
                Corr = 1.0004 - 0.0037047 * delta + 5.1582e-6 * delta * delta + 4.2122e-8 * delta * delta * delta;
            }
            return Olefins(cc) * 160 / (OleMwt * Corr);
        }

        public static double RVP(Components components, Temperature T)
        {
            double tvp = ThermodynamicsClass.CalcBubblePointP(components, T, components.Thermo);
            if (tvp < 0.72522)
            {
                return tvp - 0.09565;
            }
            else if (tvp < 21.7566)
            {
                return -0.0720983 + 0.9715271 * tvp - 0.00552898 * tvp * tvp;
            }
            else if (tvp < 43.5)
            {
                return tvp - 3.30871;
            }
            else
            {
                return tvp;
            }
        }

        public static double FlashPoint(Components components, DistPoints dist) // Should be D86
        {
            return 0.47 * components.TBP[0].BP + 0.3 * components.TBP[1].BP - 137.5; // ASTM/PMCC;
        }

        public static double Smoke(Components components) // Requires Aromatics
        {
            return 53.76 / Math.Pow(Aromatics(components), 2) + 0.03401 * Math.Pow(components.API, 1.5) + 1.0806;
        }

        public static double SmokeNoA(Components components) // Requires Aniline
        {
            double Ani = Aniline(components);
            if (Ani < 10)
            {
                Ani = 10;
            }
            return -255.26 + 2.04 * Ani - 240.8 * Math.Log(components.SG()) + 7727.0 * components.SG() / Ani;
        }

        public static double SmokePoint(Components cc) // Only if Aromatics Available
        {
            return (53.76 / Math.Pow(Aromatics(cc), 1 / 2) + 0.03401 * Math.Pow(cc.API, 1.5) + 1.0806);
        }

        public static double Lumin(Components components) // Requires Aromatics
        {
            return 139.776 / Math.Pow(Aromatics(components), 2) + 0.088426 * Math.Pow(components.API, 1.5) - 6.19044;
        }

        public static double LuminNoA(Components components) // Requires Aniline
        {
            double Ani = Aniline(components);
            if (Ani < 10)
            {
                Ani = 10;
            }
            return -2308.7 + 3412 * components.SG() + 43.2 * Ani - 56.93 * components.SG() * Ani + 17080 * components.SG() / Ani;
        }

        public static double DieselIndex(Components components) // Requires Aniline
        {
            double Ani = Aniline(components);
            if (Ani < 10)
            {
                Ani = 10;
            }
            return Ani * components.API / 100;
        }

        public static double CetaneIndexD976(Components components)  // Should be D86
        {
            double CI = -420.34 + 0.016 * components.API * components.API + 0.192 * components.API * Math.Log10(components.D86[5].BP)
                + 65.01 * Math.Pow(Math.Log10(components.D86[5].BP), 2) - 1.809E-4 * components.D86[5].BP * components.D86[5].BP;
            return CI;
        }

        public static double CetaneIndexD4737(Components components)  // Should be D86
        {
            double X = Math.Exp(-3.5 * (components.SG() - 0.85)) - 1;
            double CI = 45.2 + 0.0892 * (components.D86[2].BP - 215) + (0.131 + 0.901 * X) * (components.D86[5].BP - 260)
                + (0.0523 - 0.42 * X) * (components.D86[9].BP - 310)
                + 0.00049 * (Math.Pow(components.D86[2].BP - 215, 2) - Math.Pow(components.D86[9].BP - 310, 2))
                + 107 * X + 60 * X * X;
            return CI;
        }

        public static double RI67(Components cc)
        {
            return 1 + 0.8447 * cc.Density.Pow(1.2056) * cc.MeABP().Kelvin.Pow(-0.0557) * cc.MW().Pow(-0.0044);
        }

        public static double SurfaceTension(Components cc, Temperature T) //  densities not implemented// or mixingrules
        {
            double Tbred = cc.Tbreduced();
            double res = 0;
            double TbmixK = cc.TbMixK();
            double LiqMolarDens = 10;
            double VapMolarDens = 0.01;
            double Tr = T / cc.TCritMix();
            double n = RI67(cc);
            double Pc = cc.PCritMix();
            double Tc = cc.TCritMix();
            double omega = cc.OmegaMix();

            switch (cc.Thermo.SurfaceTensionMethod)
            {
                case enumSurfaceTensionMethod.BrockBird:
                    double Q = 0.1297 * (1 + Tbred * Math.Log(Pc) / (1 - Tbred)) - 0.281;
                    res = Pc.Pow(2f / 3f) * Tc.CtoK().Pow(1f / 3f) * Q * (1 - Tr).Pow(11f / 9f);
                    break;

                case enumSurfaceTensionMethod.EscobedoMansoori:
                    double Rmref = 6.987;
                    double R = (1 / (LiqMolarDens / 1000)) * (n.Pow(2) - 1) / (n.Pow(2) + 2);
                    double Rstar = R / Rmref;
                    double Po = 39.6431 * (0.22217 - 0.00291042 * (Rstar / Tbred.Pow(2f))) * Tc.CtoK().Pow(13f / 12f) / Pc.Pow(5f / 6f);
                    double P = Po * (1 - Tr).Pow(0.37) * Tr * Math.Exp(0.30066 / Tr + 0.86442 * Tr.Pow(9));
                    res = (P * (LiqMolarDens / 1000 - VapMolarDens / 1000)).Pow(4);
                    break;

                case enumSurfaceTensionMethod.HugillVanWelseness:
                    P = 40.1684 * (0.151 - 0.0464 * omega) * Tc.CtoK().Pow(13 / 12) / Pc.Pow(5 / 6);
                    res = (P * (LiqMolarDens / 1000 - VapMolarDens / 1000)).Pow(4);
                    break;

                case enumSurfaceTensionMethod.SastriRaoAcids:
                    res = 3.9529e-7 * Pc.Pow(0.5) * TbmixK.CtoK().Pow(-1.5) * Tc.CtoK().Pow(1.85) * ((1 - Tr) / (1 - Tbred)).Pow(11f / 9f);
                    break;

                case enumSurfaceTensionMethod.SastriRaoAlcohols:
                    res = 1.282e-4 * Pc.Pow(0.25) * TbmixK.Pow(0.175) * ((1 - Tr) / (1 - Tbred)).Pow(0.8);
                    break;
            }
            return res;
        }

        public static IUOM GetAssayProperty(Components cc, enumAssayPCProperty prop, enumMassMolarOrVol basis)
        {
            switch (basis)
            {
                case enumMassMolarOrVol.Mass:
                    MassFraction res = MassSum(cc, (int)prop);
                    return res;

                case enumMassMolarOrVol.Molar:
                    MoleFraction res1 = MolSum(cc, (int)prop);
                    return res1;

                case enumMassMolarOrVol.Vol:
                    VolFraction res2 = VolSum(cc, (int)prop);
                    return res2;
            }
            return null;
        }

        public static IUOM SetAssayProperty(Components cc, enumAssayPCProperty prop, double value)
        {
            for (int i = 0; i < cc.Count; i++)
            {
                cc[i].AddProperty(prop, value);
            }
            return null;
        }

        public static double UOPK(Components cc)
        {
            return Math.Pow((Components.VABPShortForm(cc) + 273.13) * 9 / 5 + 32, 1.0 / 3) / cc.SG();
        }

        public static double Aromatics(Components cc)
        {
            return (VolSum(cc, (int)enumAssayPCProperty.AROMATICS));
        }

        public static double Naphthenes(Components cc)
        {
            return (VolSum(cc, (int)enumAssayPCProperty.NAPHTHENES));
        }

        public static double Olefins(Components cc)
        {
            return (VolSum(cc, (int)enumAssayPCProperty.OLEFINS));
        }

        public static double IPara(Components cc)
        {
            return (VolSum(cc, (int)enumAssayPCProperty.ISOPARAFFINS));
        }

        public static double TPara(Components cc)
        {
            return 100 - Naphthenes(cc) - Aromatics(cc) - Olefins(cc);
        }

        public static double NPara(Components cc)
        {
            return (VolSum(cc, (int)enumAssayPCProperty.PARAFFINS));
        }

        public static double Nitrogen(Components cc)
        {
            return (MassSum(cc, (int)enumAssayPCProperty.NITROGEN));
        }

        public static double Aniline(Components cc)
        {
            return (MassSum(cc, (int)enumAssayPCProperty.ANILINEPOINT));
        }
    }
}