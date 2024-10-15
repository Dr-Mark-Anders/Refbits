using ModelEngine.ThermodynamicMethods.Activity_Models;
using System;

namespace ModelEngine.ThermodynamicMethods
{
    public class SolveNR
    {
        public Tuple<double, double> Solve(double activity1, double activity2, UNIQUAC uniquac)
        {
            double[] res;

            res = uniquac.SolveGamma();

            return new Tuple<double, double>(0, 0);
        }
    }
}