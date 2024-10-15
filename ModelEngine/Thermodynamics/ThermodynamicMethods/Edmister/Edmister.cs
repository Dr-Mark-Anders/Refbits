using System;
using Units.UOM;

namespace ModelEngine
{
    public class Edmister
    {
        public static double Omega(Temperature BP, Temperature TCrit, double Pcrit)
        {
            double Tr = BP / TCrit;
            double res = 3f / 7f * (Tr / (1 - Tr)) * (Math.Log10(Pcrit / 1.101325)) - 1;
            return res;
        }
    }
}