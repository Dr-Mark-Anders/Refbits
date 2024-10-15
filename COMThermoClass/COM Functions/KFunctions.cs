using ModelEngine;
using System.Diagnostics;

namespace COMColumnNS
{
    public partial class COMThermo
    {
        public object KMixFixedProps(object comps, object X, object Y, object SG, object BPk, object MW, object TcritK, object PcritBar,
            object CritVol, object Omega, double Tk, double Pbar, int method, bool UseBips)
        {
            Components cc = new();
            BaseComp sc;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] y = (double[])Y;
            double[] sg = (double[])SG;
            double[] bp = (double[])BPk;
            double[] Tc = (double[])TcritK;
            double[] Pc = (double[])PcritBar;
            double[] Acentric = (double[])Omega;
            double[] MOleWt = (double[])MW;
            double[] CrV = (double[])CritVol;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            cc.Thermo.Enthalpy = enumEnthalpy.PR78;
            cc.Thermo.CritTMethod = enumCritTMethod.TWU;
            cc.Thermo.CritPMethod = enumCritPMethod.TWU;
            cc.Thermo.MW_Method = enumMW_Method.TWU;
            cc.Thermo.UseBIPs = UseBips;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], MOleWt[i], Acentric[i], Tc[i], Pc[i], CrV[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            double[] res = ThermodynamicsClass.KMixArray(cc, Pbar, Tk, x, y, out enumFluidRegion state, cc.Thermo);
            return res;
        }

        public double KRealComp(string comp, double T, double P, int method)
        {
            //Debugger.Launch();
            double[] res;
            Components cc = new();
            BaseComp sc = Thermodata.GetComponent(comp);
            if (sc != null)
            {
                sc.MoleFraction = 1;
                cc.Add(sc);
                cc.Thermo.KMethod = (enumEquiKMethod)method;
                res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out _, cc.Thermo);
                return res[0];
            }
            return double.NaN;
        }

        public double KQuasiComp(double T, double P, double SG, double Meabp, int method, bool UseBips)
        {
            double[] res;
            Components cc = new Components();
            ThermoDynamicOptions thermo = new();
            thermo.KMethod = (enumEquiKMethod)method;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            BaseComp sc = new PseudoComponent(Meabp, SG, thermo);
            sc.MoleFraction = 1;
            cc.Add(sc);
            cc.Thermo.UseBIPs = UseBips;

            res = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out _, thermo);
            return res[0];
        }

        public double KMixPure(object comps, object X, object Y, double T, double P, int method, int comp, bool UseBips)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] y = (double[])Y;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                sc.MoleFraction = x[i];
                cc.Add(sc);
            }
            cc.Thermo.UseBIPs = UseBips;

            res = ThermodynamicsClass.KMixArray(cc, P, T, x, y, out enumFluidRegion state, cc.Thermo)[comp];
            return res;
        }

        public object KMixPureArray(object comps, object X, object Y, double T, double P, int method)
        {
            object res;
            Components cc = new();
            BaseComp sc;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] y = (double[])Y;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                sc.MoleFraction = x[i];
                cc.Add(sc);
            }

            res = ThermodynamicsClass.KMixArray(cc, P, T, x, y, out enumFluidRegion state, cc.Thermo);
            return res;
        }

        public double KMixPureandQuasi(object comps, object SG, object BP, object X, object Y, double T, double P, int method, int comp)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] y = (double[])Y;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            res = ThermodynamicsClass.KMixArray(cc, P, T, x, y, out enumFluidRegion state, cc.Thermo)[comp];
            return res;
        }

        public object KMixPureandQuasiArray(object comps, object SG, object BP, object X, object Y, double T, double P, int method, bool usebips)
        {
            double[] res;
            Components cc = new();
            BaseComp sc;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] y = (double[])Y;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            cc.Thermo.UseBIPs = usebips;
            res = ThermodynamicsClass.KMixArray(cc, P, T, x, y, out _, cc.Thermo);
            return res;
        }

        public double LnKRealComp(string comp, double T, double P, int method)
        {
            double[] res;
            Components cc = new();
            BaseComp sc = Thermodata.GetComponent(comp);
            sc.MoleFraction = 1;
            cc.Add(sc);

            ThermoDynamicOptions thermo = new();
            thermo.KMethod = (enumEquiKMethod)method;

            res = ThermodynamicsClass.LnKMix(cc, P, T, out enumFluidRegion state, cc.Thermo);
            return res[0];
        }

        public object LnkRealMix(object comps, object X, object Y, double T, double P, int method)
        {
            double[] res;
            Components cc = new();
            BaseComp sc;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            cc.Thermo.UseBIPs = false;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] y = (double[])Y;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                sc.MoleFraction = x[i];
                cc.Add(sc);
            }

            res = ThermodynamicsClass.LnKMixArray(cc, P, T, x, y, out enumFluidRegion state, cc.Thermo);
            return res;
        }

        public double LnKRealMixComp(object comp, object X, object Y, double T, double P, int method, int compNo)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            cc.Thermo.UseBIPs = false;

            //Debugger.Launch();

            string[] c = (string[])comp;
            double[] x = (double[])X;
            double[] y = (double[])Y;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                sc.MoleFraction = x[i];
                cc.Add(sc);
            }

            res = ThermodynamicsClass.LnKMixArray(cc, P, T, x, y, out enumFluidRegion state, cc.Thermo)[compNo];
            return res;
        }

        public object LnKMixPureandQuasiArray(object comps, object SG, object BP, object X, object Y, double T, double P, int method)
        {
            double[] res;
            Components cc = new();
            BaseComp sc;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            cc.Thermo.UseBIPs = false;
            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] y = (double[])Y;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            res = ThermodynamicsClass.LnKMixArray(cc, P, T, x, y, out enumFluidRegion state, cc.Thermo);
            return res;
        }
    }
}