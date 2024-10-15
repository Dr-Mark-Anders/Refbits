using Units;
using Units.UOM;

namespace ModelEngine
{
    public partial class Port_Material
    {
        public StreamProperty H_
        {
            get
            {
                ePropID e = ePropID.H;
                if (Properties[e] is null)
                    Properties[e] = new StreamProperty(ePropID.H, double.NaN);
                return Properties[e];
            }
            set
            {
                Properties[ePropID.H] = value;
            }
        }

        public StreamProperty S_

        {
            get
            {
                ePropID e = ePropID.S;
                if (Properties[e] is null)
                    Properties[e] = new StreamProperty(ePropID.S, double.NaN);
                return Properties[e];
            }
            set
            {
                Properties[ePropID.S] = value;
            }
        }

        public StreamProperty P_
        {
            get
            {
                ePropID e = ePropID.P;
                if (Properties[e] is null)
                    Properties[e] = new StreamProperty(ePropID.P, double.NaN);
                return Properties[e];
            }
            set
            {
                Properties[ePropID.P] = value;
            }
        }

        public StreamProperty T_
        {
            get
            {
                ePropID e = ePropID.T;
                if (Properties[e] is null)
                    Properties[e] = new StreamProperty(ePropID.T, double.NaN);
                return Properties[e];
            }
            set
            {
                Properties[ePropID.T] = value;
            }
        }

        public StreamProperty MF_
        {
            get
            {
                ePropID e = ePropID.MF;
                if (Properties[e] is null)
                    Properties[e] = new StreamProperty(ePropID.MF, double.NaN);
                return Properties[e];
            }
            set
            {
                Properties[ePropID.MF] = value;
            }
        }

        public StreamProperty VF_
        {
            get
            {
                ePropID e = ePropID.VF;
                if (Properties[e] is null)
                    Properties[e] = new StreamProperty(ePropID.VF, double.NaN);
                return Properties[e];
            }
            set
            {
                Properties[ePropID.VF] = value;
            }
        }

        public double SG
        {
            get
            {
                return cc.SG_Calc();
            }
        }

        public StreamProperty MolarFlow_
        {
            get
            {
                ePropID e = ePropID.MOLEF;
                if (Properties[e] is null)
                    Properties[e] = new StreamProperty(ePropID.MOLEF, double.NaN);
                return Properties[e];
            }
            set
            {
                Properties[ePropID.MOLEF] = value;
            }
        }

        public StreamProperty Q_
        {
            get
            {
                ePropID e = ePropID.Q;
                if (Properties[e] is null)
                    Properties[e] = new StreamProperty(ePropID.Q, double.NaN);
                return Properties[e];
            }
            set
            {
                Properties[ePropID.Q] = value;
            }
        }

        public double EnergyFlow
        {
            get
            {
                return H_ * MolarFlow_;  // Allways calculate
            }
        }

        public StreamProperty SignalIn
        {
            get
            {
                return propIn;
            }
            set
            {
                propIn = value;
            }
        }

        public StreamProperty SignalOut
        {
            get
            {
                return propOut;
            }
            set
            {
                propOut = value;
            }
        }

        public MassHeatCapacity MassCp
        {
            get
            {
                MassHeatCapacity Cp = new();

                return Cp;
            }
        }

        public Temperature T
        { get { return (Temperature)T_.UOM; } set => T_.UOM = value; }
        public Pressure P { get => (Pressure)P_.UOM; set => P_.UOM = value; }
        public Enthalpy H { get => (Enthalpy)H_.UOM; set => H_.UOM = value; }
        public MassFlow MF { get => (MassFlow)MF_.UOM; set => MF_.UOM = value; }

        public VolumeFlow VF { get => (VolumeFlow)VF_.UOM; set => VF_.UOM = value; }
        public MoleFlow MolarFlow { get => (MoleFlow)MolarFlow_.UOM; set => MolarFlow_.UOM = value; }
        public Entropy S { get => (Entropy)S_.UOM; set => S_.UOM = value; }
        public Quality Q { get => (Quality)Q_.UOM; set => Q_.UOM = value; }
    }
}