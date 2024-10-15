using System;
using Units.UOM;

namespace ModelEngine
{
    public class SinglePhase
    {
        public static double VaryTempToSetMolarSpecificEnthalpy(Components cc,Pressure P, Temperature T, Enthalpy Enthalpy, enumFluidRegion region, ThermoDynamicOptions thermo, ref enumCalcResult cres)
        {
            Enthalpy Aenthalpy1, Aenthalpy2;double Gradient;
            double count = 0;
            Temperature Test = T;
            double errorvalue;

            do
            {
                ThermodynamicsClass.UpdateThermoProperties(cc, P, Test, thermo, state: region);
                if (region == enumFluidRegion.Liquid)
                    Aenthalpy1 = cc.ThermoLiq.H;
                else
                    Aenthalpy1 = cc.ThermoVap.H;

                ThermodynamicsClass.UpdateThermoProperties(cc, P, Test+10, thermo, state: region);
                if (region == enumFluidRegion.Liquid)
                    Aenthalpy2 = cc.ThermoLiq.H;
                else
                    Aenthalpy2 = cc.ThermoVap.H;

                Gradient = (Aenthalpy2 - Aenthalpy1) / 10;

                Test = Test + (Enthalpy - Aenthalpy1) / Gradient;

                errorvalue = Math.Abs(Aenthalpy1 - Enthalpy);

                count++;
                if (count > 1000)
                {
                    cres = enumCalcResult.Failed;
                    return double.NaN;
                }
            } while (errorvalue > 0.01 && Test > -273.15 && Test < 5000);

            if (errorvalue > 1 || Test < -273.15 || Test > 5000)
            {
                cres = enumCalcResult.Failed;
            }

            return Test;
        }
    }
}