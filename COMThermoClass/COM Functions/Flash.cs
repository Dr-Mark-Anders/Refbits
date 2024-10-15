using ModelEngine;
using ModelEngine;

namespace COMColumnNS
{
    public partial class COMThermo
    {
        public double FlashPHWithFixedQuasiProps(object comps, object X, object SG, object BPk, object MW, object
          TcritK, object PcritBar, object CritVol, object Omega, double H, double Pbar,
            int method, bool UseBips, ref double VFrac)
        {
            Components cc = new Components();
            BaseComp sc;

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BPk;
            double[] Tc = (double[])TcritK;
            double[] Pc = (double[])PcritBar;
            double[] Acentric = (double[])Omega;
            double[] MOleWt = (double[])MW;
            double[] CrV = (double[])CritVol;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            cc.Thermo.UseBIPs = UseBips;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new PseudoComponent(bp[i], sg[i], MOleWt[i], Acentric[i], Tc[i], Pc[i], CrV[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            cc.NormaliseFractions();

            Port_Material p = new(cc);
            p.H_ = new(Units.ePropID.H, H, SourceEnum.Input);
            p.P_ = new(Units.ePropID.P, Pbar, SourceEnum.Input);

            //Debugger.Launch();
            p.Flash();

            return p.T_;
        }

        public double FlashPH(object comps, object X, object SG, object BP, double H, double P, int method, bool UseBips)
        {
            Components cc = new();
            BaseComp sc;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            cc.Thermo.UseBIPs = UseBips;

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

            cc.NormaliseFractions();

            Port_Material p = new(cc);
            p.P_ = new(Units.ePropID.P, P, SourceEnum.Input);
            p.H_ = new(Units.ePropID.H, H, SourceEnum.Input);
            p.Flash();

            return p.T_.BaseValue;
        }

        public object Flash(object comps, object X, object SG, object BP, double T, double P, int method, bool UseBips)
        {
            Components cc = new();
            BaseComp sc;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BP;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            cc.Thermo.UseBIPs = UseBips;

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

            cc.NormaliseFractions();

            Port_Material p = new(cc);
            p.Flash();

            //FlashClass.Flash(cc, cc.Thermo, calcderivatives: false);

            double[,] res = new double[2, x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                res[0, i] = cc.LiqPhaseMolFractions[i];
            }

            for (int i = 0; i < x.Length; i++)
            {
                res[1, i] = cc.VapPhaseMolFractions[i];
            }

            return res;
        }

        public object FlashWithFixedQuasiProps(object comps, object X, object SG, object BPk, object MW, object
            TcritK, object PcritBar, object CritVol, object Omega, double Tk, double Pbar, int method, bool UseBips, ref double VFrac)
        {
            Components cc = new Components();
            BaseComp sc;

            //Debugger.Launch();

            string[] c = (string[])comps;
            double[] x = (double[])X;
            double[] sg = (double[])SG;
            double[] bp = (double[])BPk;
            double[] Tc = (double[])TcritK;
            double[] Pc = (double[])PcritBar;
            double[] Acentric = (double[])Omega;
            double[] MOleWt = (double[])MW;
            double[] CrV = (double[])CritVol;

            cc.Thermo.KMethod = (enumEquiKMethod)method;
            cc.Thermo.UseBIPs = UseBips;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new PseudoComponent(bp[i], sg[i], MOleWt[i], Acentric[i], Tc[i], Pc[i], CrV[i], cc.Thermo);
                    pc.MoleFraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.MoleFraction = x[i];
                    cc.Add(sc);
                }
            }

            cc.NormaliseFractions();

            Port_Material p = new(cc);
            p.Flash();

            VFrac = p.Q;

            double[,] res = new double[2, x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                res[0, i] = cc.LiqPhaseMolFractions[i];
            }

            for (int i = 0; i < x.Length; i++)
            {
                res[1, i] = cc.VapPhaseMolFractions[i];
            }

            return res;
        }
    }
}