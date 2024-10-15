using Units;
using Units.UOM;

namespace Steam97
{
    public partial class StmPropIAPWS97
    {
        private const double tRef = 1.0;
        private const double pRef = 1.0;
        private const double tRef_btu = 1.8;
        private const double pRef_btu = 0.1450377;  // psia
        private const double delT_F = 459.67;       // deg. F

        public const double MW = 18.01528;

        // normalizing constants
        private const double Rgas = 0.461526;       // kJ/kg-K

        private const double Rgas_btu = 0.1102336;  // Btu/lbm-F

        public Pressure Psat(Temperature t)
        {
            // t is the Temperature
            // return  s saturation Pressure
            // stat = 0 on successful completion

            int btuFlag = 0;

            // if btuFlag = 0, Temperature  is in deg. K, Pressure  is in kPa
            // if btuFlag = 1, Temperature  is in deg. F, Pressure  is in psia

            Pressure p = new Pressure();

            ClearErrors();

            if (btuFlag == 1)
                t = (t + delT_F) / tRef_btu;

            if (state.region != 4 || state.t != t)
                ClearState();

            if (t < T3p)
            {
                errorCondition = 1;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                p = P3p;
            }
            else if (t > Tc1)
            {
                errorCondition = 1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                p = Pc1;
            }
            else
            {
                p = Pi_4(t);

                state.region = 4;
                state.p = p;

                if (btuFlag == 1)
                    p *= pRef_btu;
                else
                    p *= pRef;
            }

            return p;
        }

        public double Tsat(Pressure P)
        {
            // p is the Pressure
            // return  s saturation Temperature
            // stat = 0 on successful completion

            // if btuFlag = 0, Temperature  is in deg. K, Pressure  is in kPa
            // if btuFlag = 1, Temperature  is in deg. F, Pressure  is in psia

            double t, p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (state.region != 4 || state.p != p)
                ClearState();

            if (p < P3p)
            {
                errorCondition = 1;
                AddErrorMessage("Pressure  is out of range. Results are for " + P3p.ToString() + " kPa");
                t = T3p;
            }
            else if (p > Pc1)
            {
                errorCondition = 1;
                AddErrorMessage("Pressure  is out of range. Results are for " + Pc1.ToString() + " kPa");
                t = Tc1;
            }
            else
            {
                t = Theta_4(p);

                state.region = 4;
                state.t = t;
            }

            t *= tRef;

            return t;
        }

        public double vfp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific volume of saturated liquid in m3/kg or ft3/lbm.
            double p = P.kPa;
            Temperature t = 0.0;
            double v = 0.0;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature

            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                v = Region1.V(p, t);
            else
                v = Region3.V(p, t, 1);

            v /= (p / Rgas / t);

            return v;
        }

        public double hfp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific enthalpy of saturated liquid in J/kg or Btu/lbm.

            Temperature t = 0.0;
            Enthalpy h = 0.0;
            double p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature

            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                h = Region1.H(p, t);
            else
                h = Region3.H(p, t, 1);

            h *= Rgas * t;

            return h;
        }

        public double sfp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific entropy of saturated liquid in J/kg-K or Btu/lbm-R.

            Temperature t = 0.0, s = 0.0;
            double p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature

            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                s = Region1.S(p, t);
            else
                s = Region3.S(p, t, 1);

            s *= Rgas;

            return s;
        }

        public double mufp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific dynamic viscosity of saturated liquid in Pa-s or lbf-s/ft^2.

            double t = 0.0, v = 0.0, mu = 0.0;
            double p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature
            v = vfp(p);  // call in SI to get v in m3/kg

            mu = TRANSPORT.mu(t, 1.0 / v);

            return mu;
        }

        public double kfp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s thermal conductivity of saturated liquid in W/m-K or Btu/hr-ft-F.

            double t = 0.0, v = 0.0, k = 0.0;
            double p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature
            v = vfp(p);  // call in SI to get v in m3/kg

            k = Transport.tc(p, t, 1.0 / v);

            return k;
        }

        public double vgp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific volume of saturated vapor in m3/kg or ft3/lbm.

            Temperature t = 0.0, v = 0.0;
            double p = P.kPa;
            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature

            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                v = Region2.V(p, t);
            else
                v = Region3.V(p, t, 2);

            v /= (p / Rgas / t);

            return v;
        }

        public double hgp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific enthalpy of saturated vapor in J/kg or Btu/lbm.

            Temperature t = 0.0;
            Enthalpy h = 0.0;
            double p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature

            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                h = Region2.H(p, t);
            else
                h = Region3.H(p, t, 2);

            h *= Rgas * t;

            return h;
        }

        public double sgp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific entropy of saturated vapor in J/kg-K or Btu/lbm-R.

            Temperature t = 0.0;
            double s = 0.0, p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature

            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                s = Region2.S(p, t);
            else
                s = Region3.S(p, t, 2);

            s *= Rgas;

            return s;
        }

        public double mugp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific dynamic viscosity of saturated liquid in Pa-s or lbf-s/ft^2.

            double t = 0.0, v = 0.0, mu = 0.0;
            double p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature
            v = vgp(p);  // call in SI to get v in m3/kg

            mu = TRANSPORT.mu(t, 1.0 / v);

            return mu;
        }

        public double kgp(Pressure P)
        {
            // p is Pressure  in kPa or psia
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s thermal conductivity of saturated vapor in W/m-K or Btu/hr-ft-F.

            double t = 0.0, v = 0.0, k = 0.0;
            double p = P.kPa;

            ClearErrors();

            p /= pRef;

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > Pc1)
            {
                p = Pc1;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + Pc1.ToString() + " kPa");
                errorCondition = 1;
            }

            t = Theta_4(p);  // reduced saturation Temperature
            v = vgp(p);  // call in SI to get v in m3/kg

            k = Transport.tc(p, t, 1.0 / v);

            return k;
        }

        public double vft(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific volume of saturated liquid in m3/kg or ft3/lbm.

            double p = 0.0;
            double v = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure

            Pressure P = new Pressure(p);
            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                v = Region1.V(p, t);
            else
                v = Region3.V(p, t, 1);

            v /= (p / Rgas / t);

            return v;
        }

        public double hft(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific enthalpy of saturated liquid in J/kg or Btu/lbm.

            double p = 0.0;
            double h = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure
            Pressure P = new Pressure(p);

            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                h = Region1.H(p, t);
            else
                h = Region3.H(p, t, 1);

            h *= Rgas * t;

            return h;
        }

        public double sft(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific entropy of saturated liquid in J/kg-K or Btu/lbm-R.

            double p = 0.0;
            double s = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure
            Pressure P = new Pressure(p);
            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                s = Region1.S(p, t);
            else
                s = Region3.S(p, t, 1);

            s *= Rgas; ;

            return s;
        }

        public double muft(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific dynamic viscosity of saturated liquid in Pa-s or lbf-s/ft^2.

            double p = 0.0, v = 0.0, mu = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure

            v = vft(t);  // call in SI to get v in m3/kg

            mu = TRANSPORT.mu(t, 1.0 / v);

            return mu;
        }

        public double kft(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s thermal conductivity of saturated liquid in W/m-K or Btu/hr-ft-F.

            double p = 0.0, v = 0.0, k = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure

            v = vft(t);  // call in SI to get v in m3/kg

            k = Transport.tc(p, t, 1.0 / v);

            return k;
        }

        public double vgt(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific volume of saturated vapor in m3/kg or ft3/lbm.

            double p = 0.0, v = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure
            Pressure P = new Pressure(p);
            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                v = Region2.V(p, t);
            else
                v = Region3.V(p, t, 2);

            v /= (p / Rgas / t);

            return v;
        }

        public double hgt(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific enthalpy of saturated vapor in J/kg or Btu/lbm.

            double p = 0.0;
            double h = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure

            Pressure P = new Pressure(p);
            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                h = Region2.H(p, t);
            else
                h = Region3.H(p, t, 2);

            h *= Rgas * t;

            return h;
        }

        public double sgt(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific entropy of saturated vapor in J/kg-K or Btu/lbm-R.

            double p = 0.0;
            double s = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure
            Pressure P = new Pressure(p);

            int reg = SubRegion(ref p, ref t);
            if (reg == 1 || reg == 2)
                s = Region2.S(p, t);
            else
                s = Region3.S(p, t, 2);

            s *= Rgas;

            return s;
        }

        public double mugt(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific dynamic viscosity of saturated vapor in Pa-s or lbf-s/ft^2.

            double p = 0.0, v = 0.0, mu = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure

            v = vgt(t);  // call in SI to get v in m3/kg

            mu = TRANSPORT.mu(t, 1.0 / v);

            return mu;
        }

        public double kgt(Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s thermal conductivity of saturated vapor in W/m-K or Btu/hr-ft-F.

            double p = 0.0, v = 0.0, k = 0.0;

            ClearErrors();

            t /= tRef;

            if (t < T3p)
            {
                t = T3p;
                AddErrorMessage("Temperature  is out of range. Results are for " + T3p.ToString() + " °K");
                errorCondition = 1;
            }
            else if (t > Tc1)
            {
                t = Tc1;
                AddErrorMessage("Temperature  is out of range. Results are for " + Tc1.ToString() + " °K");
                errorCondition = 1;
            }

            p = Pi_4(t);     // reduced saturation Pressure

            v = vgt(t);  // call in SI to get v in m3/kg

            k = Transport.tc(p, t, 1.0 / v);

            return k;
        }

        public double vpt(Pressure Pbar, Temperature t)
        {
            // p is Pressure  in kPa or psia
            // t is Temperature  in K or deg F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific volume in m3/kg or ft3/lbm.
            double p = Pbar.kPa;
            double v = 1.0;

            ClearErrors();

            p /= pRef;

            // get the subregion

            int reg = SubRegion(ref p, ref t);

            if (reg == 1)
                v = Region1.V(p, t) / (p / Rgas / t);
            else if (reg == 2)
                v = Region2.V(p, t) / (p / Rgas / t);
            else if (reg == 3)
                v = 1.0 / Region3.Rho(p, t, 0);
            else if (reg == 5)
                v = Region5.V(p, t) / (p / Rgas / t);

            return v;
        }

        public double hpt(Pressure Pbar, Temperature t)
        {
            // p is Pressure  in kPa or psia
            // t is Temperature  in K or deg F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific enthalpy in kJ/kg or Btu/lbm.
            double p = Pbar.kPa;
            double h = 0.0;

            ClearErrors();

            p /= pRef;

            // get the subregion

            int reg = SubRegion(ref p, ref t);

            if (reg == 1)
                h = Region1.H(p, t);
            else if (reg == 2)
                h = Region2.H(p, t);
            else if (reg == 3)
                h = Region3.H(p, t, 0);
            else if (reg == 5)
                h = Region5.H(p, t);

            return h * Rgas * t;
        }

        public double upt(Pressure Pbar, Temperature t)
        {
            // p is Pressure  in kPa or psia
            // t is Temperature  in K or deg F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific enthalpy in kJ/kg or Btu/lbm.

            double p = Pbar.kPa;
            double u = 0.0;

            ClearErrors();

            p /= pRef;

            // get the subregion

            int reg = SubRegion(ref p, ref t);

            if (reg == 1)
                u = Region1.U(p, t);
            else if (reg == 2)
                u = Region2.U(p, t);
            else if (reg == 3)
                u = Region3.U(p, t);
            else if (reg == 5)
                u = Region5.U(p, t);

            return u * Rgas * t;
        }

        public double spt(Pressure Pbar, Temperature t)
        {
            // p is Pressure  in kPa or psia
            // t is Temperature  in K or deg F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific entropy in kJ/kg-K or Btu/lbm-F.

            double s = 0.0;
            double p = Pbar.kPa;

            ClearErrors();

            p /= pRef;

            // get the subregion

            int reg = SubRegion(ref p, ref t);

            if (reg == 1)
                s = Region1.S(p, t);
            else if (reg == 2)
                s = Region2.S(p, t);
            else if (reg == 3)
                s = Region3.S(p, t, 0);
            else if (reg == 5)
                s = Region5.S(p, t);

            return s * Rgas;
        }

        public double cppt(Pressure Pbar, Temperature t)
        {
            // p is Pressure  in kPa or psia
            // t is Temperature  in K or deg F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific heat at constant Pressure  in kJ/kg-K or Btu/lbm-F.

            double cp = 0.0;
            double p = Pbar.kPa;

            ClearErrors();

            p /= pRef;

            // get the subregion

            int reg = SubRegion(ref p, ref t);

            if (reg == 1)
                cp = Region1.CP(p, t);
            else if (reg == 2)
                cp = Region2.CP(p, t);
            else if (reg == 3)
                cp = Region3.CP(p, t, 0);
            else if (reg == 5)
                cp = Region5.CP(p, t);

            return cp * Rgas;
        }

        public double cvpt(Pressure Pbar, Temperature t)
        {
            // p is Pressure  in kPa or psia
            // t is Temperature  in K or deg F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific heat at constant Pressure  in kJ/kg-K or Btu/lbm-F.

            double cv = 0.0;
            double p = Pbar.kPa;

            ClearErrors();

            p /= pRef;

            // get the subregion

            int reg = SubRegion(ref p, ref t);

            if (reg == 1)
                cv = Region1.CV(p, t);
            else if (reg == 2)
                cv = Region2.CV(p, t);
            else if (reg == 3)
                cv = Region3.CV(p, t);
            else if (reg == 5)
                cv = Region5.CV(p, t);

            return cv * Rgas;
        }

        public double mupt(Pressure Pbar, Temperature t)
        {
            // p is Pressure  in kPa or psia
            // t is Temperature  in K or deg F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s specific dynamic viscosity of saturated liquid in Pa-s or lbf-s/ft^2.

            double v = 0.0, mu = 0.0;
            double p = Pbar.kPa;

            ClearErrors();

            p /= pRef;

            // get the subregion

            int reg = SubRegion(ref p, ref t);

            if (reg == 1)
                v = Region1.V(p, t);
            else if (reg == 2)
                v = Region2.V(p, t);
            else if (reg == 3)
                v = Region3.V(p, t, 0);
            else if (reg == 5)
                v = Region5.V(p, t);

            v /= (p / Rgas / t);

            mu = TRANSPORT.mu(t, 1.0 / v);

            return mu;
        }

        public double kpt(Pressure Pbar, Temperature t)
        {
            // t is Temperature  in K or F
            // stat = 0 on successful completion, stat != 0 on error
            // btuFlag = 1 for BTU units, btuFlag is 0 for SI
            // return  s thermal conductivity of saturated vapor in W/m-K or Btu/hr-ft-F.
            double p = Pbar.kPa;

            double v = 0.0, k = 0.0;

            ClearErrors();

            p /= pRef;

            // get the subregion

            int reg = SubRegion(ref p, ref t);

            if (reg == 1)
                v = Region1.V(p, t);
            else if (reg == 2)
                v = Region2.V(p, t);
            else if (reg == 3)
                v = Region3.V(p, t, 0);
            else if (reg == 5)
                v = Region5.V(p, t);

            v /= (p / Rgas / t);

            k = Transport.tc(p, t, 1.0 / v);

            return k;
        }

        public double Tph(Pressure Pbar, MassEnthalpy h)
        {
            // p is Pressure  in kPa or Btu/lbm
            // h is enthalpy in kJ/kg or Btu/lbm
            // stat is 0 on return   if successful completion
            // Units are in SI if btuFlag = 0, in BTU if btuFlag = 1

            Temperature t = 0.0;
            double x = double.MinValue;
            double p = Pbar.kPa;

            ClearErrors();

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > 100000.0)
            {
                p = 100000.0;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + p.ToString() + " kPa");
                errorCondition = 1;
            }

            /* double  hmax = ((7.38214E-14 * p - 6.22294E-09) * p - 4.57127E-03) * p + 4.16092E+03;
             double  hmin = Math.Max(((1.14504E-15 * p - 7.53710E-10) * p + 1.01820E-03) * p - 3.89901E-02, 0.0);

             if (h > hmax)
             {
                 h = hmax;
                 AddErrorMessage("Enthalpy is too high. Results are for " + h.ToString() + " kJ/kg");
                 errorCondition = 1;
             }
             else if (h < hmin)
             {
                 h = hmin;
                 AddErrorMessage("Enthalpy is too low. Results are for " + h.ToString() + " kJ/kg");
                 errorCondition = 1;
             }*/

            if (p < Pc1)
            {
                double tsat, hf, hg;

                tsat = Theta_4(p);

                if (tsat < 623.15)
                {
                    hf = Region1.H(p, tsat) * Rgas * tsat;
                    hg = Region2.H(p, tsat) * Rgas * tsat;
                    x = (h - hf) / (hg - hf);
                    if (x < 0.0)
                        t = Region1.Tph(p, h);
                    else if (x <= 1.0 && x >= 0.0)
                        t = tsat;
                    else
                        t = Region2.Tph(p, h);
                }
                else
                {
                    hf = Region3.H(p, tsat, 1) * Rgas * tsat;
                    hg = Region3.H(p, tsat, 2) * Rgas * tsat;
                    x = (h - hf) / (hg - hf);
                    if (x < 0.0)
                    {
                        // Approximate Region 1-3 boundary
                        double h13 = (((7.27071E-18 * p - 2.06464E-12) * p + 2.23637E-07) * p - 1.14480E-02) * p + 1.80136E+03;
                        if (h < h13)
                            t = Region1.Tph(p, h);
                        else
                            t = Region3.Tph(p, h);
                    }
                    else if (x <= 1.0 && x >= 0.0)
                        t = tsat;
                    else
                    {  // this could be in region 2 or 3.
                        double t23 = T23(p);
                        double h23 = Region2.H(p, t23) * Rgas * t23;
                        if (h < h23)
                            t = Region3.Tph(p, h);
                        else
                            t = Region2.Tph(p, h);
                    }
                }
            }
            else
            {
                int reg;
                bool regionFound = false;
                // we have to do some guessing

                // Approximate Region 1-3 boundary above critical Pressure
                double h13 = (((7.27071E-18 * p - 2.06464E-12) * p + 2.23637E-07) * p - 1.14480E-02) * p + 1.80136E+03;

                if (h < (h13 + 10.0))  // add some to be sure
                {
                    t = Region1.Tph(p, h);
                    reg = SubRegion(ref p, ref t);
                    if (reg == 1)
                        regionFound = true;  // got it.
                }

                if (!regionFound)
                {
                    // this is region 2 or 3.
                    // approximate region 2-3 boundary above critical Pressure
                    double h23;
                    if (p < 40000.0)
                        h23 = (((-1.53693E-15 * p + 1.99564E-10) * p - 9.49050E-06) * p + 1.94414E-01) * p + 1.17743E+03;
                    else
                        h23 = (((8.04207E-18 * p - 2.67059E-12) * p + 3.33749E-07) * p - 1.46824E-02) * p + 2.81053E+03;

                    if (h > h23 - 10.0)  // add some tolerance
                    {
                        t = Region2.Tph(p, h);
                        reg = SubRegion(ref p, ref t);
                        if (reg == 3)
                            regionFound = false;
                        else
                            regionFound = true;
                    }
                }

                if (!regionFound)
                {
                    // Not in region 1 or 2. Has to be region 3.
                    t = Region3.Tph(p, h);
                }
            }

            state.x = x;
            return t;
        }

        public double Tps(Pressure Pbar, Entropy s)
        {
            // p is Pressure  in kPa or Btu/lbm
            // s is entropy in kJ/kg-K or Btu/lbm-F
            // stat is 0 on return   if successful completion
            // Units are in SI if btuFlag = 0, in BTU if btuFlag = 1

            Temperature t = 0.0;
            double p = Pbar.kPa;

            double x = double.MinValue;

            ClearErrors();

            if (p < P3p)
            {
                p = P3p;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + P3p.ToString() + " kPa");
                errorCondition = 1;
            }
            else if (p > 100000.0)
            {
                p = 100000.0;
                AddErrorMessage("Pressure  is out of bounds. Results are for " + p.ToString() + " kPa");
                errorCondition = 1;
            }

            /*  double  smax = -5.58993E-01 * Math.Log(p) + 1.25450E+01;
              double  smin = 0.0;

              if (s > smax)
              {
                  s = smax;
                  AddErrorMessage("Entropy is too high. Results are for " + s.ToString() + " kJ/kg °K");
                  errorCondition = 1;
              }
              else if (s < smin)
              {
                  s = smin;
                  AddErrorMessage("Enthalpy is too low. Results are for " + s.ToString() + " kJ/kg °K");
                  errorCondition = 1;
              }*/

            if (p < Pc1)
            {
                double tsat, sf, sg;

                tsat = Theta_4(p);

                if (tsat < 623.15)
                {
                    sf = Region1.S(p, tsat) * Rgas;
                    sg = Region2.S(p, tsat) * Rgas;
                    x = (s - sf) / (sg - sf);
                    if (x < 0.0)
                        t = Region1.Tps(p, s);
                    else if (x <= 1.0 && x >= 0.0)
                        t = tsat;
                    else
                        t = Region2.Tps(p, s);
                }
                else
                {
                    sf = Region3.S(p, tsat, 1) * Rgas;
                    sg = Region3.S(p, tsat, 2) * Rgas;
                    x = (s - sf) / (sg - sf);
                    if (x < 0.0)
                    {
                        // Approximate Region 1-3 boundary
                        double s13 = (3.76683E-11 * p - 8.36729E-06) * p + 3.86940E+00;
                        if (s < s13)
                            t = Region1.Tps(p, s);
                        else
                            t = Region3.Tps(p, s);
                    }
                    else if (x <= 1.0 && x >= 0.0)
                        t = tsat;
                    else
                    {  // this could be in region 2 or 3.
                        double t23 = T23(p);
                        double s23 = Region2.S(p, t23) * Rgas;
                        if (s < s23)
                            t = Region3.Tps(p, s);
                        else
                            t = Region2.Tps(p, s);
                    }
                }
            }
            else
            {
                int reg;
                bool regionFound = false;
                // we have to do some guessing

                // Approximate Region 1-3 boundary above critical Pressure
                double s13 = (3.76683E-11 * p - 8.36729E-06) * p + 3.86940E+00;

                if (s < (s13 + 0.05))  // add some to be sure
                {
                    t = Region1.Tps(p, s);
                    reg = SubRegion(ref p, ref t);
                    if (reg == 1)
                        regionFound = true;  // got it.
                }

                if (!regionFound)
                {
                    // this is region 2 or 3.
                    // approximate region 2-3 boundary above critical Pressure
                    double s23;
                    if (p < 40000.0)
                        s23 = (((((-2.11798E-26 * p + 3.88682E-21) * p - 2.95203E-16) * p + 1.18836E-11) * p - 2.67319E-07) * p + 3.17149E-03) * p - 1.01618E+01;
                    else
                        s23 = ((((-2.96307E-25 * p + 1.17911E-19) * p - 1.89365E-14) * p + 1.54787E-09) * p - 6.35368E-05) * p + 6.08131E+00;

                    if (s > s23 - 0.05)  // add some tolerance
                    {
                        t = Region2.Tps(p, s);
                        reg = SubRegion(ref p, ref t);
                        if (reg == 3)
                            regionFound = false;
                        else
                            regionFound = true;
                    }
                }

                if (!regionFound)
                {
                    // Not in region 1 or 2. Has to be region 3.
                    t = Region3.Tps(p, s);
                }

                return t;
            }

            state.x = x;
            return t;
        }

        public double P3rt(Density r, Temperature t)
        {
            // This function is for test purposes only.
            // r is density in kg/m3 or lbm/ft3
            // t is Temperature  in K or F
            // stat is 0 on return   if successful completion
            // Units are in SI if btuFlag = 0, in BTU if btuFlag = 1

            double p;

            p = Region3.Prt(r, t);

            return p;
        }

        public static ThermoProps WaterPropsMolar(Pressure P, Temperature T, enumSatType SatType)
        {
            StmPropIAPWS97 steam = new();
            const double WaterHFormation25 = -239430.74; // kj/kgmol
            const double WaterH25 = 114.05985215170321;
            ThermoProps waterprops = new(P, T);
            waterprops.MW = 18.01528;

            switch (SatType)
            {
                case enumSatType.SatVap:
                    waterprops.H = (steam.hgp(P) - WaterH25) * 18.01528 + WaterHFormation25;
                    break;

                case enumSatType.SatLiq:
                    waterprops.H = (steam.hfp(P) - WaterH25) * 18.01528 + WaterHFormation25;
                    break;

                case enumSatType.Normal:
                    waterprops.H = (steam.hpt(P, T) - WaterH25) * 18.01528 + WaterHFormation25; // HForm
                    break;
            }

            waterprops.P = P;
            waterprops.Psat = steam.Psat(T.Celsius);
            waterprops.Tsat = steam.Tsat(P);
            waterprops.T = T;
            // waterprops.H = steam.hpt(P, T)*waterprops.MW;
            waterprops.S = steam.spt(P, T) * waterprops.MW;
            waterprops.V = steam.vpt(P, T) * waterprops.MW;
            waterprops.Cp = steam.cppt(P, T) * waterprops.MW;
            waterprops.Cv = steam.cvpt(P, T) * waterprops.MW;
            waterprops.Z = steam.Z(P, T);
            return waterprops;
        }

        public static ThermoPropsMass WaterPropsMass(Pressure P, Temperature T)
        {
            StmPropIAPWS97 steam = new();
            ThermoPropsMass waterprops = new(P, T);
            waterprops.MW = 18.01528;
            waterprops.P = P;
            waterprops.Psat = steam.Psat(T.Celsius);
            waterprops.Tsat = steam.Tsat(P);
            waterprops.T = T;
            waterprops.H = steam.hpt(P, T);
            waterprops.S = steam.spt(P, T);
            waterprops.V = steam.vpt(P, T);
            waterprops.Cp = steam.cppt(P, T);
            waterprops.Cv = steam.cvpt(P, T);
            waterprops.Z = steam.Z(P, T);
            return waterprops;
        }

        private double Z(Pressure p, Temperature t)
        {
            double RGas = 8.31446261815324;
            double vol = vpt(p, t);
            return p.kPa * vol * MW / (RGas * t.BaseValue);
        }
    }
}