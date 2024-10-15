using Steam;
using System.Runtime.InteropServices;
using Units.UOM;

namespace Steam97
{
    [Guid(ContractGuids.Server)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class COMSteam : ICOMSteam
    {
        public const double Gravity = 9.80665;
        private double RGas = 8.31446261815324;
        public const double MW = 18.01528;

        public COMSteam()
        {
        }

        public double H(double P, double T)
        {
            Temperature t = new(T, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            return steam.hpt(P, t);
        }

        public double S(double P, double T)
        {
            Temperature t = new(T, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            return steam.spt(P, t);
        }

        public double Th(double P, double H)
        {
            MassEnthalpy h = new(H, MassEnthalpyUnit.kJ_kg);
            StmPropIAPWS97 steam = new();
            return steam.Tph(P, h) - 273.15;
        }

        public double Ts(double P, double S)
        {
            Entropy s = new(S, EntropyUnit.J_mole_K);
            StmPropIAPWS97 steam = new();
            return steam.Tps(P, s) - 273.15;
        }

        public double Cp(double P, double T)
        {
            Temperature t = new(T, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            return steam.cppt(P, t);
        }

        public double Cv(double P, double T)
        {
            Temperature t = new(T, TemperatureUnit.Celsius);
            StmPropIAPWS97 steam = new();
            return steam.cvpt(P, t);
        }

        public double Pump(double flow, double Pin, double Tin, double Pout, double eff)
        {
            // flow kg/hr
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);

            if (eff > 1)
                eff /= 100;

            MassEnthalpy Hin, Hout;
            double density = 1 / steam.vpt(Pin, tin);

            double DifferentialDP = Pout - Pin;
            double DifferentialHead = DifferentialDP * 100 / (density / 1000 * 9.81);
            double Hydraulicpower = DifferentialHead * flow / 1000 * 9.81 / 3600 / eff; //kw

            Hin = steam.hpt(Pin, tin);
            Hout = Hin + Hydraulicpower * 3600 / flow;

            var ResT = steam.Tph(Pout, Hout) - 273.15;

            return Hydraulicpower;
        }

        public double Expander(double flow, double Pin, double Tin, double Pout, double eff)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);
            Pressure pin = new(Pin, PressureUnit.BarA);

            if (eff > 1)
                eff /= 100;

            Pressure P = new Pressure(Pin, PressureUnit.BarA);
            ExpansionCompression exp = new(steam.Tph, StmPropIAPWS97.WaterPropsMass, Pin, Pout, tin, flow, eff, EffType.Isen, Factormethod.Huntington, true);
            return exp.Power;
        }

        public double Compressor(double flow, double Pin, double Tin, double Pout, double eff)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);
            Pressure P = new Pressure(Pin, PressureUnit.BarA);

            if (eff > 1)
                eff /= 100;

            ExpansionCompression exp = new(steam.Tph, StmPropIAPWS97.WaterPropsMass, P, Pout, tin, flow, eff, EffType.Isen, Factormethod.Huntington, false);

            return exp.Power;
        }

        public double DSP(double flow, double Pin, double Tin, double Pout, double Tout, double PWater, double TWater)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);
            Temperature tout = new(Tout, TemperatureUnit.Celsius);
            Temperature tw = new(TWater, TemperatureUnit.Celsius);
            MassEnthalpy Hin, Hout, Hwater;

            Hin = steam.hpt(Pin, tin);
            Hout = steam.hpt(Pout, tout);
            Hwater = steam.hpt(PWater, tw);
            double waterflow = flow * (Hin - Hout) / (Hout - Hwater);

            return waterflow;
        }

        public double Tsat(double P)
        {
            StmPropIAPWS97 steam = new();
            Temperature Tout = steam.Tsat(P);
            return Tout.Celsius;
        }

        public double Psat(double T)
        {
            StmPropIAPWS97 steam = new();
            Pressure Pout = steam.Psat(T);
            return Pout.BarA;
        }

        public double DSPF(double flow, double Pin, double Tin, double Pout, double WaterFlow, double PWater, double TWater)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);
            Temperature tout;
            Temperature tw = new(TWater, TemperatureUnit.Celsius);
            MassEnthalpy Hin, Hout, Hwater;

            Hin = steam.hpt(Pin, tin);
            Hwater = steam.hpt(PWater, tw);
            Hout = (Hin * flow + Hwater * WaterFlow) / (flow + WaterFlow);
            tout = steam.Tph(Pout, Hout);

            return tout.Celsius;
        }

        public double Valve(double flow, double Pin, double Tin, double Pout)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);
            MassEnthalpy Hin = steam.hpt(Pin, tin);
            Temperature Tout = steam.Tph(Pout, Hin);

            return Tout.Celsius;
        }

        public double Flash(double Pin, double Tin, double Pout)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);

            double Hin = steam.hpt(Pin, tin);
            double Tout = steam.Tph(Pout, Hin);

            Temperature Tsat = steam.Tsat(Pout);
            MassEnthalpy HSatLiq = steam.hfp(Pout);
            MassEnthalpy HSatVap = steam.hgp(Pout);

            if (Tout < Tsat)
                return 0;
            else if (Tout > Tsat)
                return 1;

            double FlashRatio = 1 + (Hin - HSatVap) / (HSatVap - HSatLiq);
            return FlashRatio;
        }

        public double Boiler(double flow, double Pin, double Tin, double Pout, double eff)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);
            if (eff > 1)
                eff /= 100;

            MassEnthalpy Hin = steam.hpt(Pin, tin);
            Temperature Tsat = steam.Tsat(Pout);
            MassEnthalpy HSatVap = steam.hgp(Pout);

            return (HSatVap - Hin) / eff * flow;
        }

        public double V(double Pin, double Tin)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);
            return steam.vpt(Pin, tin);
        }

        public double Z(double Pin, double Tin)
        {
            StmPropIAPWS97 steam = new();
            Temperature tin = new(Tin, TemperatureUnit.Celsius);
            Pressure P = new Pressure(Pin, PressureUnit.BarA);
            double volume = steam.vpt(Pin, tin);
            return (double)(P.kPa * volume * MW / (RGas * tin.Kelvin));
        }
    }
}