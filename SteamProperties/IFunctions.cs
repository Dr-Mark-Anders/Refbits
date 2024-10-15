using System.Runtime.InteropServices;

namespace Steam97
{
    [Guid(ContractGuids.Interface)]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICOMSteam
    {
        double H(double P, double T);

        double S(double P, double T);

        double Th(double P, double H);

        double Ts(double P, double S);

        double Cp(double P, double T);

        double Cv(double P, double T);

        double Pump(double flow, double Pin, double Tin, double Pout, double eff);

        double Expander(double flow, double Pin, double Tin, double Pout, double eff);

        double Compressor(double flow, double Pin, double Tin, double Pout, double eff);

        double DSP(double flow, double Pin, double Tin, double Pout, double Tout, double PWater, double TWater);

        double Tsat(double P);

        public double DSPF(double flow, double Pin, double Tin, double Pout, double WaterFlow, double PWater, double TWater);

        public double Valve(double flow, double Pin, double Tin, double Pout);

        public double Flash(double Pin, double Tin, double Pout);

        public double Boiler(double flow, double Pin, double Tin, double Pout, double eff);

        public double V(double Pin, double Tin);

        public double Z(double Pin, double Tin);
    }
}