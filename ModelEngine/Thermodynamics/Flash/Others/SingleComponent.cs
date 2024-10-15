using System;
using Units.UOM;

namespace ModelEngine
{
    public class SingleComponent
    {
        public static Temperature AdiabaticSingleFlashP_H(Port_Material port, Pressure P, Temperature T, Enthalpy enthalpy, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            Components cc = port.cc;
            Temperature BP = ThermodynamicsClass.BubblePoint(port.cc, P, thermo, out _);
            Enthalpy liqenthalpy;
            Enthalpy vapenthalpy;
            Temperature Tres;

            port.T = BP;

            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, cc.Thermo, enumFluidRegion.Liquid);

            liqenthalpy = cc.ThermoLiq.H;
            vapenthalpy = cc.ThermoVap.H;

            if (enthalpy <= liqenthalpy)      // all liquid
            {
                Tres = VaryTempToSetMolarSpecificEnthalpy(port, enthalpy, ref cres, thermo);
                port.Q = 0;
            }
            else if (enthalpy >= vapenthalpy) // all vapour
            {
                Tres = VaryTempToSetMolarSpecificEnthalpy(port, enthalpy, ref cres, thermo);
                port.Q = 1;
            }
            else                              // partially vaporised
            {
                Tres = BP;
                port.Q = (enthalpy.kJ_kgmole - liqenthalpy.kJ_kgmole) / (vapenthalpy.kJ_kgmole - liqenthalpy.kJ_kgmole);
                cc[0].MoleFracVap = port.Q;
            }
            return Tres;
        }

        public static Pressure SingleFlashT_H(Port_Material port, Pressure P, Temperature T, Enthalpy enthalpy, ThermoDynamicOptions thermo, ref enumCalcResult cres)
        {
            Components cc = port.cc;
            Pressure BP = ThermodynamicsClass.CalcBubblePointP(cc, T, thermo);
            Enthalpy liqenthalpy;
            Enthalpy vapenthalpy;

            port.P = BP;

            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, cc.Thermo,enumFluidRegion.Liquid);

            liqenthalpy = cc.ThermoLiq.H;
            vapenthalpy = cc.ThermoVap.H;

            Pressure Pres;
            if (enthalpy <= liqenthalpy)      // all liquid
            {
                Pres = VaryPressToSetMolarSpecificEnthalpy(cc, P, T, port.Q, enthalpy, ref cres, thermo);
                port.Q = 0;
            }
            else if (enthalpy >= vapenthalpy) // all vapour
            {
                Pres = VaryPressToSetMolarSpecificEnthalpy(cc, P, T, port.Q, enthalpy, ref cres, thermo);
                port.Q = 1;
            }
            else                              // partially vaporised
            {
                Pres = BP;
                port.Q = (enthalpy.kJ_kgmole - liqenthalpy.kJ_kgmole) / (vapenthalpy.kJ_kgmole - liqenthalpy.kJ_kgmole);
                cc[0].MoleFracVap = port.Q_;
                cres = enumCalcResult.Converged;
            }
            return new Pressure(Pres);
        }

        public static double AdiabaticSingleComponentFlashEntropy(Port_Material port, Pressure P, Temperature T, Entropy entropy, ThermoDynamicOptions thermo)
        {
            Components cc = port.cc;

            Temperature BP = ThermodynamicsClass.BubblePoint(port.cc, P, thermo, out _);
            Entropy liqentropy;
            Entropy vapentropy;
            Temperature Tres;

            port.T = BP;

            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, cc.Thermo, enumFluidRegion.Liquid);

            liqentropy = cc.ThermoLiq.S;
            vapentropy = cc.ThermoVap.S;

            if (entropy >= liqentropy && entropy <= vapentropy)  // partially vaporised
            {
                Tres = BP;
                port.Q = (entropy.BaseValue - liqentropy.BaseValue) / (vapentropy.BaseValue - liqentropy.BaseValue);
            }
            else if (entropy < liqentropy) // all liquid
            {
                Tres = VaryTempToSetMolarSpecificEntropy(cc, P, entropy, port.Q, thermo);
                port.Q = 0;
            }
            else if (entropy > vapentropy) // all vapour
            {
                Tres = VaryTempToSetMolarSpecificEntropy(cc, P, entropy, port.Q, thermo);
                port.Q = 1;
            }
            else
            {
                Tres = double.NaN;
            }
            return Tres;
        }

        public static double VaryTempToSetMolarSpecificEnthalpy(Port_Material port, Enthalpy Enthalpy, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            double d, dold = 0;
            double delta = 5;
            Components cc = port.cc;
            Temperature t = port.TCritMix() * 0.7;
            double Aenthalpy;
            double count = 0;

            do
            {
                ThermodynamicsClass.UpdateThermoProperties(cc, port.P, t, thermo);
                Aenthalpy = cc.H(port.P, t, port.Q);
                d = Aenthalpy - Enthalpy;
                if (Math.Abs(d) > 0.000000001)
                {
                    if (d > 0)
                        t -= delta;
                    else
                        t += delta;

                    if (d == dold)
                        return t;

                    if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                        delta /= 2;

                    dold = d;
                }

                port.T = t;

                count++;
                if (count > 1000)
                {
                    cres = enumCalcResult.Failed;
                    return double.NaN;
                }
            } while (Math.Abs(d) > FlashClass.ErrorValue && t > -273.15 && t < 5000);

            if (double.IsNaN(d) || t < -273.15 || t > 5000)
            {
                cres = enumCalcResult.Failed;
            }

            return t;
        }

        public static double VaryTempToSetMolarSpecificEntropy(Components cc, Pressure P, Entropy Entropy, Quality Q,ThermoDynamicOptions thermo)
        {
            double d, dold = 0;
            double delta = 5;
            Temperature T = cc.TCritMix() * 0.7;
            double Aentropy;
            do
            {
                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
                Aentropy = cc.S(P,T,Q);
                d = Aentropy - Entropy.BaseValue;
                if (Math.Abs(d) > 0.000000001)
                {
                    if (d > 0)
                        T -= delta;
                    else
                        T += delta;

                    if (d == dold)
                        return T;

                    if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                        delta /= 2;

                    dold = d;
                }
            } while (Math.Abs(d) > FlashClass.ErrorValue);

            //o.H.SetCalcValue(H);

            return T;
        }

        public static double VaryPressToSetMolarSpecificEnthalpy(Components cc, Pressure P, Temperature T, Quality Q, Enthalpy Enthalpy, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            double d, dold = 0;
            double delta = 0.1;
            double p = P;
            double Aenthalpy;

            do
            {
                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
                Aenthalpy = cc.H(P, T, Q);

                d = Aenthalpy - Enthalpy;
                if (Math.Abs(d) > 0.000000001)
                {
                    if (d > 0)
                        p -= delta;
                    else
                        p += delta;

                    if (d == dold)
                        return p;

                    if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                        delta /= 2;

                    dold = d;
                }
            } while (Math.Abs(d) > FlashClass.ErrorValue);

            if (double.IsNaN(d))
                cres = enumCalcResult.Failed;

            return p;
        }

        public static double VaryPressToSetMolarSpecificEntropy(Components cc, Pressure P, Temperature T, Quality Q, Entropy Entropy, ThermoDynamicOptions thermo)
        {
            double d, dold = 0;
            double delta = 0.1;
            double p = cc.PCritMix() * 0.1;
            double Aentropy;

            do
            {
                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
                Aentropy = cc.S(P, T, Q);
                d = Aentropy - Entropy;
                if (Math.Abs(d) > 0.000000001)
                {
                    if (d < 0)
                        p -= delta;
                    else
                        p += delta;

                    if (d == dold)
                        return p;

                    if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                        delta /= 2;

                    dold = d;
                }
            } while (Math.Abs(d) > FlashClass.ErrorValue && p < 500);

            //o.H.SetCalcValue(H);

            return p;
        }
    }
}