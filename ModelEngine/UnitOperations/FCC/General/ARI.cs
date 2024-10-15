using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEngine.UnitOperations.FCC
{
    internal class ARI
    {
        public static double FRI20(double RI20)
        {
            return (RI20.Pow(2) - 1) / (RI20.Pow(2) + 2);
        }

        public static double Solve(double MW, double RI20)
        {
            double FRi20 = FRI20(RI20);
            double ari = 2 * (MW / FRi20 - (3.5419 * MW + 73.1858))
                / ((3.5074 * MW - 91.972) - (3.5149 * MW + 73.1858));
            return ari;
        }
    }
}
