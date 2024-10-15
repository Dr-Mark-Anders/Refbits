using ModelEngine;
using ModelEngine;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace COMColumnNS
{
    public partial class COMThermo
    {
        public double LiqComponentEnthalpy(string comp, double T, double P, int method)
        {
            // Debugger.Launch();
            double res;
            BaseComp sc = Thermodata.GetComponent(comp);
            sc.MoleFraction = 1;
            Components cc = new();
            cc.Add(sc);
            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo).H;
            return res;
        }

        public double VapComponentEnthalpy(string comp, double T, double P, int method)
        {
            double res;
            BaseComp sc = Thermodata.GetComponent(comp);
            sc.MoleFraction = 1;
            Components cc = new();
            cc.Add(sc);
            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo).H;
            return res;
        }

        public double StreamEnthalpyReal(object comps, object X, double T, double P, int method)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            string[] c = (string[])comps;
            double[] x = (double[])X;

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
                    System.Windows.Forms.MessageBox.Show("Component " + c[i].ToString() + "Not Found In Database");
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            Port_Material port = new();
            port.T_ = new(ePropID.T,T);
            port.P_ = new(ePropID.P,P);

            FlashClass.Flash(port, port.Thermo, Guid.Empty, calcderivatives: false);

            res = cc.StreamEnthalpy(port.Q);
            return res;
        }

        public double StreamTemperatureReal(object comps, object X, double H, double P, int method)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            string[] c = (string[])comps;
            double[] x = (double[])X;

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
                    System.Windows.Forms.MessageBox.Show("Component " + c[i].ToString() + "Not Found In Database");
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            Port_Material port = new();
            port.H_ = new(ePropID.H, H);
            port.P_ = new(ePropID.P, P);

            FlashClass.Flash(port, port.Thermo, Guid.Empty, calcderivatives: false);

            res = port.T_;
            return res;
        }

        public double StreamComponentEnthalpy(string c, double T, double P, int method)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            cc.Thermo.Enthalpy = (enumEnthalpy)method;
            sc = Thermodata.GetComponent(c);

            if (sc == null)
            {
                System.Windows.Forms.MessageBox.Show("Component " + c.ToString() + "Not Found In Database");
            }
            else
            {
                sc.MoleFraction = 1;
                cc.Add(sc);
            }

            Port_Material port = new();
            port.T_ = new(ePropID.T, T);
            port.P_ = new(ePropID.P, P);

            FlashClass.Flash(port, port.Thermo, Guid.Empty, calcderivatives: false);

            res = cc.StreamEnthalpy(port.Q);
            return res;
        }

        public double LiqEnthalpyReal(object comps, object Xin, double T, double P, int method)
        {
            //Debugger.Launch();

            Components cc = new();
            BaseComp sc;

            string[]? c = null;
            double[] x = null;

            switch (comps)
            {
                case string s:
                    c = new string[] { s };
                    x = new double[] { Convert.ToDouble(Xin) };
                    break;

                case string[] s:
                    c = s;
                    x = (double[])Xin;
                    break;
            }

            if (x != null && c != null)
            {
                cc.Thermo.Enthalpy = (enumEnthalpy)method;
                cc.Thermo.UseBIPs = false;

                for (int i = 0; i < x.Length; i++)
                {
                    sc = Thermodata.GetComponent(c[i]);
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }

                ThermoProps props = ThermodynamicsClass.BulkStreamThermo(cc, x, P, T, enumFluidRegion.Liquid, cc.Thermo);

                if (props != null)
                    return props.H;
                else
                    return double.NaN;
            }
            else
            {
                return double.NaN;
            }
        }

        public double EnthalpyFormationReal(object comps, object X)
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

            return ThermodynamicsClass.EnthalpyFormation25(cc, x);
        }

        public double EnthalpyFormationRealAndQuasi(object comps, object X, object SG, object BP, int method)
        {
            Debugger.Break();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;
            new Components().Thermo.Enthalpy = (enumEnthalpy)method;

            for (int i = 0; i < x.Length; i++)
            {
                BaseComp sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], new Components().Thermo);
                    pc.MoleFraction = x[i];
                    new Components().Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    new Components().Add(sc);
                }
            }

            return ThermodynamicsClass.EnthalpyFormation25(new Components(), x);
        }

        public double StreamEnthalpyFixedProps(object comps, object X, double T, double P, object SG, object BP, object CritT,
            object CritP, object Critv, object MW, object Acentricity, int method, bool usebips)
        {
            Debugger.Break();
            double res;
            Components cc = new();
            BaseComp sc;

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;
            double[] Tc = (double[])CritT;
            double[] Pc = (double[])CritP;
            double[] Vc = (double[])Critv;
            double[] MoleWt = (double[])MW;
            double[] Omega = (double[])Acentricity;

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i];
            }

            if (sum != 1)  // normalise
            {
                for (int i = 0; i < x.Length; i++)
                    x[i] /= sum;
            }

            cc.Thermo.Enthalpy = (enumEnthalpy)method;
            cc.Thermo.KMethod = enumEquiKMethod.PR78;
            cc.Thermo.UseBIPs = usebips;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], MoleWt[i], Omega[i], Tc[i], Pc[i], Vc[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            Port_Material port = new();
            port.T_ = new(ePropID.T, T);
            port.P_ = new(ePropID.P, P);

            FlashClass.Flash(port, port.Thermo, Guid.Empty, calcderivatives: false);

            res = port.StreamEnthalpy();
            return res;
        }

        public double StreamEnthalpy(object comps, object X, double T, double P, object SG, object BP, int method)
        {
            double res;
            Components cc = new();
            Temperature Tt = T;
            Pressure Pp = P;
            BaseComp sc;

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

            Port_Material port = new();
            port.T_ = new(ePropID.T, T);
            port.P_ = new(ePropID.P, P);

            FlashClass.Flash(port, port.Thermo, Guid.Empty, calcderivatives: false);

            res = port.StreamEnthalpy();
            return res;
        }

        public double LiqEnthalpyQuasi(double BP, double SG, double T, double P, int method)
        {
            Components cc = new();
            PseudoComponent sc = new(BP, SG, cc.Thermo);

            sc.MoleFraction = 1;
            cc.Add(sc);

            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            double res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo).H;
            return res;
        }

        public double LiqEnthalpyRealAndQuasi(object comps, object X, double T, double P, object SG, object BP, int method)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            Debugger.Break();

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

            res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo).H;
            return res;
        }

        public double VapEnthalpyReal(object comps, object X, double T, double P, int method)
        {
            Components cc = new();
            BaseComp sc;
            string[]? c = null;
            double[]? x = null;

            switch (comps)
            {
                case string s:
                    c = new string[] { s };
                    x = new double[] { Convert.ToDouble(X) };
                    break;

                case string[] s:
                    c = s;
                    x = (double[])X;
                    break;
            }

            if (x != null && c != null)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    sc = Thermodata.GetComponent(c[i]);
                    if (sc != null)
                    {
                        sc.MoleFraction = x[i];
                        cc.Add(sc);
                    }
                    else
                    {
                        return double.NaN;
                    }
                }

                cc.Thermo.Enthalpy = (enumEnthalpy)method;

                double res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo).H;
                return res;
            }
            return double.NaN;
        }

        public double VapEnthalpyQuasi(double BP, double SG, double T, double P, int method)
        {
            Components cc = new();

            PseudoComponent sc = new(BP, SG, cc.Thermo);

            sc.MoleFraction = 1;
            cc.Add(sc);
            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            double res = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo).H;
            return res;
        }

        public double VapEnthalpyRealAndQuasi(object comps, object X, double T, double P, object SG, object BP, int method)
        {
            Components cc = new();
            BaseComp sc;

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

            ThermoProps props = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo);
            double res;
            if (props != null)
                res = props.H;
            else
                res = double.NaN;

            return res;
        }

        public double VapEnthalpyRealAndQuasiFixedProps(object comps, object X, double T, double P, object SG, object BP, object CritT,
            object CritP, object Critv, object MW, object Acentricity, int method, bool usebips)
        {
            double res;
            Components cc = new();
            BaseComp sc;

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;
            double[] Tc = (double[])CritT;
            double[] Pc = (double[])CritP;
            double[] Vc = (double[])Critv;
            double[] MoleWt = (double[])MW;
            double[] Omega = (double[])Acentricity;

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                sum += x[i];

            if (sum != 1)  // normalise
                for (int i = 0; i < x.Length; i++)
                    x[i] /= sum;

            cc.Thermo.Enthalpy = (enumEnthalpy)method;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], MoleWt[i], Omega[i], Tc[i], Pc[i], Vc[i], cc.Thermo);
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

            ThermoProps props = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, cc.Thermo);
            if (props != null)
                res = props.H;
            else
                res = double.NaN;

            return res;
        }

        public double LiqEnthalpyRealAndQuasiFixedProps(object comps, object X, double T, double P, object SG, object BP, object CritT,
           object CritP, object Critv, object MW, object Acentricity, int method, bool UseBips)
        {
            double res;
            Components cc = new();
            BaseComp sc;
            Debugger.Break();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;
            double[] Tc = (double[])CritT;
            double[] Pc = (double[])CritP;
            double[] Vc = (double[])Critv;
            double[] MoleWt = (double[])MW;
            double[] Omega = (double[])Acentricity;

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                sum += x[i];

            if (sum != 1)  // normalise
                for (int i = 0; i < x.Length; i++)
                    x[i] /= sum;

            cc.Thermo.Enthalpy = (enumEnthalpy)method;
            cc.Thermo.UseBIPs = UseBips;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new(bp[i], sg[i], MoleWt[i], Omega[i], Tc[i], Pc[i], Vc[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            Debugger.Break();
            ThermoProps props = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, cc.Thermo);
            if (props != null)
                res = props.H;
            else
                res = double.NaN;

            return res;
        }
    }
}