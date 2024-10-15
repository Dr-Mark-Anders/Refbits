using ModelEngine;

namespace COMColumnNS
{
    public partial class COMThermo
    {
        public double CritT(double Tb, double SG, int method)
        {
            ThermoDynamicOptions thermo = new();
            thermo.CritTMethod = (enumCritTMethod)method;

            double MW = PropertyEstimation.CalcMW(Tb, SG, thermo);
            double res = PropertyEstimation.CalcCritT(Tb, SG, MW, thermo);
            return res;
        }

        public double CritP(double Tb, double SG, int method)
        {
            ThermoDynamicOptions thermo = new();
            thermo.CritTMethod = (enumCritTMethod)method;

            double MW = PropertyEstimation.CalcMW(Tb, SG, thermo);
            double res = PropertyEstimation.CalcCritP(Tb, SG, MW, thermo);
            return res;
        }

        public double MW(double Tb, double SG, int method)
        {
            double res;
            ThermoDynamicOptions thermo = new();
            thermo.MW_Method = (enumMW_Method)method;
            res = PropertyEstimation.CalcMW(Tb, SG, thermo);
            return res;
        }

        public double PSat(string comps, double T, int method)
        {
            Components cc = new();
            BaseComp sc = Thermodata.GetComponent(comps);
            sc.MoleFraction = 1;
            cc.Add(sc);

            //Debugger.Launch();
            double res = ThermodynamicsClass.CalcBubblePointP(cc, T, cc.Thermo);
            return res;
        }

        public double PSat2(object comps, object X, double T, int method)
        {
            double res;
            Components cc = new();
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

            res = ThermodynamicsClass.CalcBubblePointP(cc, T, cc.Thermo);
            return res;
        }

        public double TSat2(object comps, object X, double P, int method)
        {
            double res;
            Components cc = new();
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

            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;

            Port_Material port = new();
            port.cc = cc;
            port.Thermo = thermo;


            res = ThermodynamicsClass.BubblePoint(port.cc, P, cc.Thermo, out _);
            return res;
        }

        public double TCritMixReal(object comps, object X)
        {
            string[] c = (string[])comps;
            double[] x = (double[])X;

            for (int i = 0; i < x.Length; i++)
            {
                BaseComp sc = Thermodata.GetComponent(c[i]);
                sc.MoleFraction = x[i];
                new Components().Add(sc);
            }

            double res = new Components().TCritMix();
            return res;
        }

        public double TCritMixRealAndQuasi(object comps, object X, object SG, object BP, int method)
        {
            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = (enumEnthalpy)method;

            for (int i = 0; i < x.Length; i++)
            {
                BaseComp sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], thermo);
                    pc.MoleFraction = x[i];
                    new Components().Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    new Components().Add(sc);
                }
            }

            double res = new Components().TCritMix();
            return res;
        }

        public double PCritMix(object comps, object X)
        {
            Components cc = new();
            BaseComp sc;

            string[] c = (string[])comps;
            double[] x = (double[])X;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                sc.MoleFraction = x[i];
                cc.Add(sc);
            }

            double res = cc.PCritMix();
            return res;
        }

        public double PCritMixRealAndQuasi(object comps, object X, object SG, object BP, int method)
        {
            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = (enumEnthalpy)method;

            for (int i = 0; i < x.Length; i++)
            {
                BaseComp sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], thermo);
                    pc.MoleFraction = x[i];
                    new Components().Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    new Components().Add(sc);
                }
            }

            double res = new Components().PCritMix();
            return res;
        }

        public double LK_PCrit(double SG, double BP)
        {
            return LeeKesler.PCrit(SG, BP);
        }

        public double LK_TCrit(double SG, double BP)
        {
            return LeeKesler.TCrit(SG, BP);
        }

        public double LK_Omega(double SG, double BP, double TCrit, double PCrit)
        {
            return LeeKesler.Omega(SG, BP, TCrit, PCrit);
        }
    }
}