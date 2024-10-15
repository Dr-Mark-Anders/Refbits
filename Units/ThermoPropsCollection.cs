using System;
using Units.UOM;

public enum enumSatType
{ SatLiq, SatVap, Normal }

namespace Units
{
    [Serializable]
    public class ThermoProps
    {
        private double z = double.NaN;
        private double f = double.NaN;
        private double h = double.NaN;
        private double s = double.NaN;
        private double g = double.NaN;
        private double a = double.NaN;
        private double u = double.NaN;
        private double v = double.NaN;

        private double h_higm = double.NaN;
        private double s_sigm = double.NaN;
        private double g_gigm = double.NaN;

        private double h_ig = double.NaN;
        private double s_ig = double.NaN;

        private Pressure p = double.NaN; // bara
        private double t = double.NaN; // k
        private double mw = double.NaN;
        private double tsat = double.NaN;
        private double psat = double.NaN;

        private bool isdirty = true;

        public ThermoProps()
        {
        }

        public ThermoProps(Pressure P, Temperature T)
        {
        }

        public ThermoProps(Pressure P, Temperature T, double MW, double z, double f, double h, double s, double g, double a, double u, double v)
        {
            this.p = P.BarA;
            this.t = T.BaseValue;
            this.MW = MW;
            this.z = z; this.f = f; this.h = h; this.s = s; this.g = g; this.a = a; this.u = u; this.v = v;
        }

        public void ControlUpdateMode(ThermoProps tpc)
        {
            this.z = tpc.z; this.f = tpc.f; this.h = tpc.h; this.s = tpc.s; this.g = tpc.g; this.a = tpc.z; this.u = tpc.u; this.v = tpc.v;
            isdirty = false;
        }

        public double K
        {
            get
            {
                return Cp / Cv;
            }
        }

        public void Clear()
        {
            z = double.NaN;
            f = double.NaN;
            h = double.NaN;
            s = double.NaN;
            g = double.NaN;
            a = double.NaN;
            u = double.NaN;
            v = double.NaN;

            h_higm = double.NaN;
            s_sigm = double.NaN;
            g_gigm = double.NaN;

            h_ig = double.NaN;
            s_ig = double.NaN;
        }


        public ThermoProps Clone()
        {
            ThermoProps props = new(P, T, MW, z, f, h, s, g, a, u, v);
            return props;
        }

        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }

        public double F
        {
            get
            {
                return f;
            }

            set
            {
                f = value;
            }
        }

        public Enthalpy H
        {
            get
            {
                return h;
            }

            set
            {
                h = value;
            }
        }

        public Entropy S
        {
            get
            {
                return s;
            }

            set
            {
                s = value;
            }
        }

        public Gibbs G
        {
            get
            {
                return g;
            }

            set
            {
                g = value;
            }
        }

        public Helmotz A
        {
            get
            {
                return a;
            }

            set
            {
                a = value;
            }
        }

        public InternalEnergy U
        {
            get
            {
                return u;
            }

            set
            {
                u = value;
            }
        }

        public double V
        {
            get
            {
                return v;
            }

            set
            {
                v = value;
            }
        }

        public double H_higm
        {
            get
            {
                return h_higm;
            }

            set
            {
                h_higm = value;
            }
        }

        public double S_sigm
        {
            get
            {
                return s_sigm;
            }

            set
            {
                s_sigm = value;
            }
        }

        public double G_gigm
        {
            get
            {
                return g_gigm;
            }

            set
            {
                g_gigm = value;
            }
        }

        public bool Isdirty
        {
            get
            {
                return isdirty;
            }

            set
            {
                isdirty = value;
            }
        }

        public double H_ig
        {
            get
            {
                return h_ig;
            }

            set
            {
                h_ig = value;
            }
        }

        public double S_ig
        {
            get
            {
                return s_ig;
            }

            set
            {
                s_ig = value;
            }
        }

        public Pressure P { get => p; set => p = value; }
        public Temperature T { get => t; set => t = value; }
        public double MW { get => mw; set => mw = value; }
        public double Cp { get; set; }
        public double Cv { get; set; }
        public double Tsat { get => tsat; set => tsat = value; }
        public double Psat { get => psat; set => psat = value; }
    }

    public class ThermoPropsMass
    {
        private double z = double.NaN;
        private double f = double.NaN;
        private double h = double.NaN;
        private double s = double.NaN;
        private double g = double.NaN;
        private double a = double.NaN;
        private double u = double.NaN;
        private double v = double.NaN;

        private double h_higm = double.NaN;
        private double s_sigm = double.NaN;
        private double g_gigm = double.NaN;

        private double h_ig = double.NaN;
        private double s_ig = double.NaN;

        private Pressure p = double.NaN; // bara
        private double t = double.NaN; // k
        private double mw = double.NaN;
        private double tsat = double.NaN;
        private double psat = double.NaN;

        private bool isdirty = true;

        public ThermoPropsMass()
        {
        }

        public ThermoPropsMass(Pressure P, Temperature T)
        {
        }

        public ThermoPropsMass(ThermoProps props, double MW)
        {
            this.mw = MW;
            z = props.Z;
            f = props.F;
            h = props.H / mw;
            s = props.S / mw;
            g = props.S / mw;
            a = props.A / mw;
            u = props.U / mw;
            v = props.V / mw;

            h_higm = props.H_higm / MW;
            s_sigm = props.S_sigm / MW;
            g_gigm = props.G_gigm / MW;

            h_ig = props.H_ig / MW;
            s_ig = props.S_ig / MW;

            Cp = props.Cp / MW;
            Cv = props.Cv / MW;

            t = props.T;
            p = props.P;
        }

        public ThermoPropsMass(Pressure P, Temperature T, double MW, double z, double f, double h, double s, double g, double a, double u, double v)
        {
            this.p = P.BarA;
            this.t = T.BaseValue;
            this.MW = MW;
            this.z = z; this.f = f; this.h = h; this.s = s; this.g = g; this.a = a; this.u = u; this.v = v;
        }

        public void ControlUpdateMode(ThermoPropsMass tpc)
        {
            this.z = tpc.z; this.f = tpc.f; this.h = tpc.h; this.s = tpc.s; this.g = tpc.g; this.a = tpc.z; this.u = tpc.u; this.v = tpc.v;
            isdirty = false;
        }

        public double K
        {
            get
            {
                return Cp / Cv;
            }
        }

        public void Clear()
        {
            z = double.NaN;
            f = double.NaN;
            h = double.NaN;
            s = double.NaN;
            g = double.NaN;
            a = double.NaN;
            u = double.NaN;
            v = double.NaN;

            h_higm = double.NaN;
            s_sigm = double.NaN;
            g_gigm = double.NaN;

            h_ig = double.NaN;
            s_ig = double.NaN;
        }

        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }

        public double F
        {
            get
            {
                return f;
            }

            set
            {
                f = value;
            }
        }

        public MassEnthalpy H
        {
            get
            {
                return h;
            }

            set
            {
                h = value;
            }
        }

        public MassEntropy S
        {
            get
            {
                return s;
            }

            set
            {
                s = value;
            }
        }

        public Gibbs G
        {
            get
            {
                return g;
            }

            set
            {
                g = value;
            }
        }

        public Helmotz A
        {
            get
            {
                return a;
            }

            set
            {
                a = value;
            }
        }

        public InternalEnergy U
        {
            get
            {
                return u;
            }

            set
            {
                u = value;
            }
        }

        public double V
        {
            get
            {
                return v;
            }

            set
            {
                v = value;
            }
        }

        public MassEnthalpy H_higm
        {
            get
            {
                return h_higm;
            }

            set
            {
                h_higm = value;
            }
        }

        public MassEntropy S_sigm
        {
            get
            {
                return s_sigm;
            }

            set
            {
                s_sigm = value;
            }
        }

        public double G_gigm
        {
            get
            {
                return g_gigm;
            }

            set
            {
                g_gigm = value;
            }
        }

        public bool Isdirty
        {
            get
            {
                return isdirty;
            }

            set
            {
                isdirty = value;
            }
        }

        public double H_ig
        {
            get
            {
                return h_ig;
            }

            set
            {
                h_ig = value;
            }
        }

        public double S_ig
        {
            get
            {
                return s_ig;
            }

            set
            {
                s_ig = value;
            }
        }

        public Pressure P { get => p; set => p = value; }
        public Temperature T { get => t; set => t = value; }
        public double MW { get => mw; set => mw = value; }
        public MassHeatCapacity Cp { get; set; }
        public MassHeatCapacity Cv { get; set; }
        public Temperature Tsat { get => tsat; set => tsat = value; }
        public Pressure Psat { get => psat; set => psat = value; }
    }

    [Serializable]
    public class ThermoDifferentialPropsCollection
    {
        private double dp_dv_t = 0;
        private double dp_dt_v = 0;
        private double dt_dp_v = 0;
        private double da_dt_p = 0;
        private double db_dt_p = 0;
        private double dz_dt_p = 0;
        private double dv_dt_p = 0;

        private double cp, cv, cp_CV, sonicVelocity, jouleThompson;

        public double Dp_dv_t
        {
            get
            {
                return dp_dv_t;
            }

            set
            {
                dp_dv_t = value;
            }
        }

        public double Dp_dt_v
        {
            get
            {
                return dp_dt_v;
            }

            set
            {
                dp_dt_v = value;
            }
        }

        public double Dt_dp_v
        {
            get
            {
                return dt_dp_v;
            }

            set
            {
                dt_dp_v = value;
            }
        }

        public double Da_dt_p
        {
            get
            {
                return da_dt_p;
            }

            set
            {
                da_dt_p = value;
            }
        }

        public double Db_dt_p
        {
            get
            {
                return db_dt_p;
            }

            set
            {
                db_dt_p = value;
            }
        }

        public double Dz_dt_p
        {
            get
            {
                return dz_dt_p;
            }

            set
            {
                dz_dt_p = value;
            }
        }

        public double Dv_dt_p
        {
            get
            {
                return dv_dt_p;
            }

            set
            {
                dv_dt_p = value;
            }
        }

        public double Cp
        {
            get
            {
                return cp;
            }

            set
            {
                cp = value;
            }
        }

        public double Cv
        {
            get
            {
                return cv;
            }

            set
            {
                cv = value;
            }
        }

        public double Cp_Cv
        {
            get
            {
                return cp_CV;
            }

            set
            {
                cp_CV = value;
            }
        }

        public double SonicVelocity
        {
            get
            {
                return sonicVelocity;
            }

            set
            {
                sonicVelocity = value;
            }
        }

        public double JouleThompson
        {
            get
            {
                return jouleThompson;
            }

            set
            {
                jouleThompson = value;
            }
        }

        public ThermoDifferentialPropsCollection()
        {
        }

        public ThermoDifferentialPropsCollection(double dp_dv_t, double dp_dt_v, double dt_dp_v, double da_dt_p, double db_dt_p, double dz_dt_p, double dv_dt_p)
        {
            this.dp_dv_t = dp_dv_t; this.dp_dt_v = dp_dt_v; this.dt_dp_v = dt_dp_v; this.da_dt_p = da_dt_p; this.db_dt_p = db_dt_p; this.dz_dt_p = dz_dt_p; this.dv_dt_p = dv_dt_p;
        }

        public void ControlUpdateMode(ThermoDifferentialPropsCollection tpc)
        {
            this.dp_dv_t = tpc.dp_dv_t; this.dp_dt_v = tpc.dp_dt_v; this.dt_dp_v = tpc.dt_dp_v; this.da_dt_p = tpc.da_dt_p; this.db_dt_p = tpc.db_dt_p; this.dz_dt_p = tpc.dz_dt_p; this.dv_dt_p = tpc.dv_dt_p;
        }
    }
}