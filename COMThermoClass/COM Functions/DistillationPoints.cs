using ModelEngine;

namespace COMColumnNS
{
    public partial class COMThermo
    {
        public double DistillationPoint(object comps, object X, object SG, object BP, int method, string DistType, string distpoint)
        {
            Components cc = new Components();
            BaseComp sc;

            if (!Enum.TryParse(distpoint, out enumDistPoints dp))
                return double.NaN;

            if (!Enum.TryParse(DistType, out enumDistType dt))
                return double.NaN;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new PseudoComponent(bp[i], sg[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            return cc.DistPoint(dt, dp).Celsius;
        }
    }
}