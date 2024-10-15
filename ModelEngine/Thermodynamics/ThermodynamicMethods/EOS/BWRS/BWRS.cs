using Extensions;
using Math2;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using System;
using System.Runtime.CompilerServices;
using System.Xml;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public static class BWRS
    {
        public const double R = 8.314459848;
        public const double Sqrt2 = 1.41421356;

        private static double Bo = 0.060228125; // propanae by default.
        private static double Ao = 500.7254656;
        private static double Co = 66030166.22;
        private static double Do = 2090428137;
        private static double Eo = 65541613091;
        private static double b = 0.021289462;
        private static double a = 67.21043849;
        private static double d = 14027.43031;
        private static double alpha = 0.000490006;
        private static double c = 14209931.35;
        private static double gamma = 0.017778556;

        private static Temperature T = new();
        private static Pressure P = new();


        public static Pressure PSat(Components cc, Temperature T)
        {
            double rho = fsolve(BWRSDensity, 50);
            double res = BWRSPressure(rho);
            return res;
        }


        public static void MixingRules(Components cc, double[] x)
        {
            Bo = 0; // 
            Ao = 0;
            Co = 0;
            Do = 0;
            Eo = 0;
            b = 0;
            a = 0;
            d = 0;
            alpha = 0;
            c = 0;
            gamma = 0;
            BaseComp bc;

            for (int i = 0; i < x.Length; i++)
            {
                bc = cc[i];
                Bo += x[i] * bc.Bo;
                Ao += (x[i] * bc.Ao).Pow(0.5);
                Co += (x[i] * bc.Co).Pow(0.5);
                Do += (x[i] * bc.Do).Pow(0.5);
                Eo += (x[i] * bc.Eo).Pow(0.5);
                a += (x[i] * bc.a).Pow(1 / 3d);
                b += (x[i] * bc.b).Pow(1 / 3d);
                c += (x[i] * bc.c).Pow(1 / 3d);
                d += (x[i] * bc.d).Pow(1 / 3d);
                alpha += (x[i] * bc.alpha).Pow(1 / 3d);
                gamma += (x[i] * bc.gamma).Pow(0.5);
            }

            Ao = Ao.Pow(2) / 1000;
            Co = Co.Pow(2) / 1000;
            Do = Do.Pow(2) / 1000;
            Eo = Eo.Pow(2) / 1000;
            a = a.Pow(3) / 1000;
            b = b.Pow(3);
            c = c.Pow(3) / 1000;
            d = d.Pow(3) / 1000;
            alpha = alpha.Pow(3);
            gamma = gamma.Pow(2);

        }



        public static Temperature TSat(Components cc, Pressure P)
        {
            double res = 0;
            return res;
        }

        public static ThermoProps ThermoBulk(Components cc, double[] X, Pressure P, Temperature T,
            double IdealEnthalpy, double IdealEntropy)
        {
            int NoComps = cc.ComponentList.Count;
            BWRS.P = P;
            BWRS.T = T;
            MixingRules(cc, X);

            //double res = BWRSPressure(50);


            double H_Higm = H_Hig(cc, X, T);
            double S_Sigm = S_Sig(cc,X,T, P) - R * Math.Log(P/0.01);

            
            double rho_molar = fsolve(BWRSDensity, 50);
            double Z = P.kPa / rho_molar / R / T.Kelvin;

            double f = BWRSFugacity(rho_molar); // P * Math.Exp(G_Gigm / (R * T));
            double G_Gigm = Math.Log(f / P)*(R * T);
            double phi = f / P.kPa;
            S_Sigm = H_Higm/T - R * Math.Log(phi);
            double H = IdealEnthalpy + H_Higm;
            double S = IdealEntropy + S_Sigm;
            double G = H - T * S;
            double V = Z * R * T / P;
            double U = H - Z * R * T;
            double Ae = U - T * S;

            ThermoProps props = new(P, T, cc.MW(), Z, f, H, S, G, Ae, U, V);
            props.H_ig = IdealEnthalpy;
            props.S_ig = IdealEntropy;
            props.H_higm = H_Higm;
            props.S_sigm = S_Sigm;

            return props;
        }

        public static double H_Hig(Components cc, double[] X, Temperature T)
        {
            int NoComps = cc.ComponentList.Count;
            double MW = cc.MW();

            double rho_molar = fsolve(BWRSDensity, 50);  //# m3/kg.mol
            double molar_volme = 1 / rho_molar * 1000;  // # cm3/mol
            double rho_mass = rho_molar * MW;         // # kg/m3
            double Z = P.kPa / rho_molar / R / T.Kelvin;

            double Pf = BWRSDensity(rho_molar);

            double enthalpy_departure = (Bo * R * T - 2 * Ao - 4 * Co / T.Pow(2) + 5 * Do / T.Pow(3) - 6 * Eo / T.Pow(4))
            * rho_molar + 0.5 * (2 * b * R * T - 3 * a - 4 * d / T) * rho_molar.Pow(2)
            + 1 / 5 * alpha * (6 * a + 7 * d / T) * rho_molar
            + c / gamma / T.Pow(2) * (3 - (3 + 0.5 * gamma * rho_molar.Pow(2) - gamma.Pow(2) * rho_molar.Pow(4))
            * Math.Exp(-gamma * Math.Pow(rho_molar, 2)));
            return enthalpy_departure;
        }


        public static double S_Sig(Components cc, double[] X, Temperature T, Pressure P)
        {
            int NoComps = cc.ComponentList.Count;
            double MW = cc.MW();

            BWRS.P = P;
            double rho_molar = fsolve(BWRSDensity, 50);  //# m3/kg.mol
            double molar_volme = 1 / rho_molar * 1000;  // # cm3/mol
            double rho_mass = rho_molar * MW;         // # kg/m3
            double Z = P.kPa / rho_molar / R / T.Kelvin;

            double Pf = BWRSDensity(rho_molar);

            double entropy_departure = -R * Math.Log(rho_molar * R * T)
                 - (Bo * R + 2 * Co / T.Pow(3) - 3 * Do / T.Pow(4) + 4 * Eo / T.Pow(5)) * rho_molar
                 - 0.5 * (b * R + d / T.Pow(2)) * rho_molar.Pow(2)
                 + alpha * d * rho_molar.Pow(5) / (5 * T.Pow(2))
                 + 2 * c / (gamma * T.Pow(3)) * (1 - (1 + 0.5 * gamma * rho_molar.Pow(2)) * Math.Exp(-gamma * rho_molar.Pow(2)))
                 * Math.Exp(-gamma * Math.Pow(rho_molar, 2));

            return entropy_departure;
        }



        public static double[] LnKmix(Components cc, Pressure P, Temperature T, double[] X, double[] Y)
        {
            return new double[1];
        }

        /// <summary>
        /// 3a
        /// </summary>
        /// <param name="n"></param>
        /// <param name="XY"></param>
        /// <param name="ai"></param>
        /// <param name="kijm"></param>
        /// <param name="Aij"></param>
        /// <param name="sumAi"></param>
        /// <return  s></return  s>

        public static double[] LnPhi(int n, double Z, double A, double B, double[] bi, double[] Amatrix)
        {
            double[] lnPhi_i = new double[n];
            return lnPhi_i;
        }


        private static double BWRSDensity(double rho)
        {
            return P.kPa - (rho * R * T + (Bo * R * T - Ao - Co / T.Pow(2) + Do / T.Pow(3) - Eo / T.Pow(4)) * rho.Pow(2)
                + (b * R * T - a - d / T) * rho.Pow(3)
                + alpha * (a + d / T) * rho.Pow(6)
                + c * rho.Pow(3) / T.Pow(2) * (1 + gamma * rho.Pow(2)) * Math.Exp(-gamma * rho.Pow(2)));
        }


        private static double BWRSPressure(double rho)
        {
            double P = -(rho * R * T + (Bo * R * T - Ao - Co / T.Pow(2) + Do / T.Pow(3) - Eo / T.Pow(4)) * rho.Pow(2)
                + (b * R * T - a - d / T) * rho.Pow(3)
                + alpha * (a + d / T) * rho.Pow(6)
                + c * rho.Pow(3) / T.Pow(2) * (1 + gamma * rho.Pow(2)) * Math.Exp(-gamma * rho.Pow(2)));

            return P;
        }

        private static double BWRSFugacity(double rho)
        {
            double P = R * T * Math.Log(rho * R * T) + 2 * (Bo * R * T - Ao - Co / T.Pow(2) + 5 * Do / T.Pow(3) - Eo / T.Pow(4)) * rho
                + 3 / 2 * (b * R * T - a - d / T) * rho.Pow(2)
                + 6 / 5 * alpha * (a + d / T) * rho.Pow(5)
                + c / gamma / T.Pow(2) * (1 - (1 - 1 / 2 * gamma * rho.Pow(2) - gamma.Pow(2) * rho.Pow(4))) * Math.Exp(-gamma * rho.Pow(2));

            P = Math.Exp(P / R / T);

            return P;
        }

        private static double fsolve(Func<double, double> f, double rho)
        {
            return BrentSolver.Solve(0.0001, 1, rho, f, 0.000001);
        }
    }
}