using ModelEngine;

namespace COMColumnNS
{
    public partial class COMThermo
    {
        public double LiqEnthalpyDepPC(double BP, double SG, double T, double P, int method)
        {
            double res;
            Components cc = new();

            //Debugger.Launch();

            PseudoComponent sc = new(BP, SG, cc.Thermo);

            sc.MoleFraction = 1;
            cc.Add(sc);

            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo).H_higm;
            return res;
        }

        public double LiqEnthalpyDepPure(object comps, object X, double T, double P, int method)
        {
            double res;
            Components cc = new();
            string[] c = (string[])comps;
            double[] x = (double[])X;

            for (int i = 0; i < x.Length; i++)
            {
                BaseComp sc = Thermodata.GetComponent(c[i]);
                sc.MoleFraction = x[i];
                cc.Add(sc);
            }

            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo).H_higm;
            return res;
        }

        public double LiqEnthalpyDepAll(object comps, object X, double T, double P, object SG, object BP, int method)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i];
            }

            if (sum != 1)  // normalise
            {
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] /= sum;
                }
            }

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

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo).H_higm;

            /*Components c2 = cc.GetNonZeroComponents();
            c2.T = T;
            c2.P = P;

            res = Thermodynamics.BulkStreamThermo(c2, thermo, enumMassOrMolar.Molar, enumFluidRegion.Liquid, ref cres).H_higm;*/

            return res;
        }

        public double LiqEnthalpyDep(string comp, double T, double P, int method)
        {
            //Debugger.Launch();
            double res = 0;
            BaseComp sc = Thermodata.GetComponent(comp);
            sc.MoleFraction = 1;
            Components cc = new Components();
            cc.Add(sc);
            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo).H_higm;
            return res;
        }

        public double VapEnthalpyDepPC(double BP, double SG, double T, double P, int method)
        {
            double res = 0;
            Components cc = new Components();
            PseudoComponent sc = new PseudoComponent(BP, SG, cc.Thermo);

            sc.MoleFraction = 1;
            cc.Add(sc);
            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo).H_higm;
            return res;
        }

        public double VapEnthalpyDepAll(object comps, object X, double T, double P, object SG, object BP, int method)
        {
            double res = 0;
            Components cc = new Components();
            BaseComp sc;

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

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo).H_higm;
            return res;
        }

        public double VapEnthalpyDepPure(object comps, object X, double T, double P, int method)
        {
            double res = 0;
            Components cc = new Components();
            BaseComp sc;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                sc.MoleFraction = x[i];
                cc.Add(sc);
            }

            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo).H_higm;
            return res;
        }

        public double VapEnthalpyDep(string comp, double T, double P, int method)
        {
            double res = 0;
            BaseComp sc = Thermodata.GetComponent(comp);
            sc.MoleFraction = 1;
            Components cc = new Components();
            cc.Add(sc);

            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            //Debugger.Launch();

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo).H_higm;
            return res;
        }

        public double LiqH_Hig(object comps, object X, double T, double P, object SG, object BP, int method)
        {
            double res = 0;
            Components cc = new Components();
            BaseComp sc;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i];
            }

            if (sum != 1)  // normalise
            {
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] /= sum;
                }
            }

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

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo).H_higm;
            return res;
        }

        public double VapH_Hig(object comps, object X, double T, double P, object SG, object BP, int method)
        {
            double res = 0;
            Components cc = new Components();
            BaseComp sc;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i];
            }

            if (sum != 1)  // normalise
            {
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] /= sum;
                }
            }

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

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo).H_higm;
            return res;
        }
    }
}