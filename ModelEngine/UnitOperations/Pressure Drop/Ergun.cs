using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units.UOM;

namespace ModelEngine.UnitOperations.Pressure_Drop
{
    public class Ergun
    {
        public DeltaPressure Solve(KinViscosity u, Length length, Length ParticlDiameter, Null voidfraction, Velocity v, Density fluiddensity )
        {
            double res = 0;

            res = 150 * u * length / Math.Pow(ParticlDiameter, 2);
            res = res * (1 - voidfraction).Pow(2) / voidfraction.Pow(3) * v;
            res = res + 1.75 * length * fluiddensity / ParticlDiameter * (1 - voidfraction) / voidfraction.Pow(2) * v;

            return res;
        }
    }
}
