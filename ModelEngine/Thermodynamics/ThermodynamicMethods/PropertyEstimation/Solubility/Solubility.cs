using System;

namespace ModelEngine.ThermodynamicMethods.Solubility
{
    public static class Solubility
    {
        public static void SolubilityFunc(int i_index, double[] Tci, double[] Pci, double[] wi, double[,] kijm, double[,] lijm, double T, double P, double[] yi, double V2S, double Psub, double VG, double lnPhiVi_index, double F1)
        {
            const double R = 8.314;
            //lnPhiV_i(i_index, Tci, Pci, wi, kijm, lijm, T, P, yi, VG, ref lnPhiVi_index, enumPRMixMethod.standard);
            F1 = lnPhiVi_index + Math.Log(yi[i_index]) - V2S * (P - Psub) / (R * T) - Math.Log(Psub / P);
        }

        /// <summary>
        /// SI Units
        /// </summary>
        /// <param name="HeatVap"></param>
        /// <param name="BoilinPoint "></param>
        /// <param name="LiquidMolarVolume"></param>
        /// <return  s></return  s>
        public static double HildebrandSolPar(double HeatVap, double BoilinPoint, double LiquidMolarVolume, ref double SolCalPerCM)
        {
            //1 cal1/2 cm−3/2 = (4.184 J)1/2 (0.01 m)−3/2 = 2.045 × 103 Pa1/2
            const double R = 8.314;
            double res = Math.Pow((HeatVap * 1000 - R * (BoilinPoint + 273.15) / LiquidMolarVolume), 0.5);
            SolCalPerCM = res / 2.045;
            return Math.Pow((HeatVap * 1000 - R * (BoilinPoint + 273.15) / LiquidMolarVolume), 0.5);
        }
    }
}