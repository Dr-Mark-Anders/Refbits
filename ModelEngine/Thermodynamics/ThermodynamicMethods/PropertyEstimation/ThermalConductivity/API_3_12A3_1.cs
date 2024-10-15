using System;
using Units.UOM;

namespace ModelEngine
{
    ///<summary>
    ///BTUft/hft2FconvertedtoWm-1K-1
    ///</summary>
    public class API_3_12A3_1
    {
        public API_3_12A3_1()
        {
        }

        public static double LiquidThermalConductivity(double MeABP)
        {
            return (0.07727 - 0.00004558 * MeABP) * 1.730735;//converttoWm-1K-1
        }

        public static double VapourThermalConductivity(Temperature T, double MW)
        {
            double A, B;

            if (MW < 50)
                A = Math.Exp(-0.022807409 * (Math.Log(MW - 9.439369)));
            else
                A = 3.2587e-5;

            B = Math.Exp(-0.87651967 * Math.Log(MW) - 1.5001297);

            return (A * T.Fahrenheit + B) * 1.730735;//converttoWm-1K-1
        }
    }
}