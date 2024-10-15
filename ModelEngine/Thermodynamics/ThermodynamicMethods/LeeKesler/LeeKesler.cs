using System;
using System.Collections.Generic;
using Units;
using Units.UOM;

namespace ModelEngine
{
    //  Estimate V from table, solve with bounded upper and lower V for Z
    public static partial class LeeKesler
    {
        private const double OmegaRef = 0.3978;

        public static double Z(double Tr, double Pr, double omega, enumFluidRegion ss)
        {
            double Z_0, Z_R;

            Z_0 = Z0_Rig(Tr, Pr, ss);
            Z_R = Z1_Rig(Tr, Pr, ss);

            //Z_0 = Z0_FromTable(Tr, Pr, ss);
            //Z_R = Z1_FromTable(Tr, Pr, ss);

            var Z1 = (Z_R - Z_0) / OmegaRef;
            var Z = Z_0 + Z1 * omega;

            return Z;
        }

        public static double H_Hig(Components cc, double[] X, Pressure P, Temperature T, enumFluidRegion state)
        {
            return EnthDeparture(T / cc.TCritMix(), P / cc.PCritMix(), cc.OmegaMix(), state);
        }


        public static double EnthDeparture(double Tr, double Pr, double Omega, enumFluidRegion ss)
        {
            double Z_0, Z_R, res, enth0, enthR;

            if (Tr < 0)
                return double.NaN;

            //Z_0 = Z0_Rig(Tr, Pr, ss);
            //Z_R = Z1_Rig(Tr, Pr, ss);

            //Z_0 = Z0_BisectTable(Tr, Pr, ss); // int erpolate then rigorous
            //Z_R = Z1_BisectTable(Tr, Pr, ss); // int erpolate then rigorous

            Z_0 = Z0_FromTable(Tr, Pr, ss, out List<double> Ts, out List<double> Ps, out List<double> Zs); // int erpolation Only
            Z_R = Z1_FromTable(Tr, Pr, ss, out Ts, out Ps, out Zs);// int erpolation Only

            enth0 = EnthDep0(Z_0, Tr, Pr);
            enthR = EnthDep1(Z_R, Tr, Pr);

            var enthd1 = (enthR - enth0) / OmegaRef;

            res = enth0 + enthd1 * Omega;  // combine simple and complex

            return res;
        }

        public static ThermoProps LeeKeslerThermo(Components cc, double[] X, Pressure P, Temperature T, enumFluidRegion state, double IdealEnthalpy, double IdealEntropy)
        {
            double zz0, zz1, zzR, EDep0, EDep1, SDp0, SDp1;
            double Zz, f, H, S, G, A, U, V;
            double H_Higm, S_Sigm, G_Gigm;
            const double Rgas = 8.314;

            double VMix, OmegaMix, Tr, Pr;
            Temperature CritTMix;
            Pressure CritPMix;

            OmegaMix = MixOmega(cc, X);

            LK_UpdateZ(cc);
            LK_UpdateCriticalVolumes(cc);

            VMix = MixVolumes(cc, X);

            CritTMix = MixCritT(cc, X, VMix);
            CritPMix = MixCritP(OmegaMix, CritTMix, VMix);

            Tr = T / CritTMix;
            Pr = P / CritPMix;

            //zz0 = Z0_Rig(Tr, Pr, state);
            //zzR = Z1_Rig(Tr, Pr, state);

            //zz0 = Z0_FromTable(Tr, Pr, state);
            //zzR = Z1_FromTable(Tr, Pr, state);

            if (Tr <= 4)
            {
                zz0 = Z0_BisectTable(Tr, Pr, state);
                zzR = Z1_BisectTable(Tr, Pr, state);
            }
            else
            {
                zz0 = Z0_Rig(Tr, Pr, state);
                zzR = Z1_Rig(Tr, Pr, state);
            }

            zz1 = zzR * 0.3978 + zz0;

            EDep0 = EnthDep0(zz0, Tr, Pr);
            EDep1 = EnthDep1(zz1, Tr, Pr);

            SDp0 = SDep0(zz0, Tr, Pr);
            SDp1 = SDep1(zz1, Tr, Pr);

            H_Higm = EDep0 + OmegaMix / OmegaRef * (EDep1 - EDep0); //H_Higm/RT
            S_Sigm = SDp0 + OmegaMix / OmegaRef * (SDp1 - SDp0);                      // S_Sigm/R

            H_Higm *= ThermodynamicsClass.Rgas * CritTMix;   // convert H_Higm/RT)
            S_Sigm *= ThermodynamicsClass.Rgas;              // convert S_Sigm/R)

            Zz = OmegaMix * zzR + zz0;

            G_Gigm = H_Higm - T * S_Sigm;

            f = P * Math.Exp(G_Gigm / (Rgas * T));
            H = IdealEnthalpy + H_Higm;
            S = IdealEntropy + S_Sigm;
            G = H - T * S;
            V = Zz * Rgas * T / P;
            U = H - Zz * Rgas * T;
            A = U - T * S;

            ThermoProps props = new ThermoProps(P, T, cc.MW(), Zz, f, H, S, G, A, U, V);

            props.H_higm = H_Higm;
            props.S_sigm = S_Sigm;
            props.H_ig = IdealEnthalpy;
            props.S_ig = IdealEntropy;

            return props;
        }
    }
}