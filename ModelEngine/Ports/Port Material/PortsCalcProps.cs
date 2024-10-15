using Extensions;
using System.ComponentModel;
using System.Linq;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public partial class Port_Material
    {
        public CalcPropList PropsCalculated = new CalcPropList();

        public double Sulfur
        {
            get
            {
                return PropertyCalcs.Sulfur(cc);
            }
            set
            {
                PropertyCalcs.SetAssayProperty(cc, enumAssayPCProperty.SULFUR, value);
            }
        }

        public double Nitrogen
        {
            get
            {
                return PropertyCalcs.Nitrogen(cc);
            }
            set
            {
                PropertyCalcs.SetAssayProperty(cc, enumAssayPCProperty.NITROGEN, value);
            }
        }

        public double RI67
        {
            get
            {
                return PropertyCalcs.RI67(cc);
            }
        }

        public Temperature MeABP
        {
            get { return cc.MeABP(); }
        }

        public double VABPShortForm { get; internal set; }

        public double TCritMix()
        {
            return cc.TCritMix();
        }

        public MassHeatCapacity CP_MASS()
        {
            return cc.CP_MASS(P, T, Q);
        }

        public Pressure VapourPressure()
        {
            return PropertyCalcs.VapourPressure(cc, T);
        }

        public Enthalpy StreamEnthalpy()
        {
            return cc.StreamEnthalpy(Q);
        }

        internal Entropy StreamEntropy()
        {
            return cc.StreamEntropy(Q);
        }

        public Density ActLiqDensity()
        {
            return PropertyCalcs.ActLiqDensity(cc, P, T);
        }

        public MassFraction MassFractionLiquid()
        {
            double res = 0, mass=0;
            for (int i = 0; i < cc.Count; i++)
            {
                BaseComp bc = cc[i];
                res += bc.MoleFraction * bc.MoleFracVap * bc.MW;
            }
            return res / cc.MW();
        }

        public VolFraction VolFractionLiquid()
        {
            double res = 0, vol = 0;
            for (int i = 0; i < cc.Count; i++)
            {
                BaseComp bc = cc[i];
                vol += bc.MoleFraction * bc.MW / bc.SG_60F;
                res += bc.MoleFraction * bc.MW / bc.SG_60F * bc.MoleFracVap;
            }
            return res / vol;
        }

        public HeatCapacity CP()
        {
            return cc.CP(T, Q);
        }

        public Enthalpy HForm25()
        {
            double res = 0;

            for (int i = 0; i < cc.Count; i++)
            {
                res += cc[i].HForm25 * cc[i].MoleFraction;
            }
            return res;
        }

        public MoleFlow VapourMoleFlow
        {
            get
            {
                return Q_ * MolarFlow_;
            }
        }

        public MoleFlow LiquidMoleFlow
        {
            get
            {
                return (1 - Q_) * MolarFlow_;
            }
        }

        public Enthalpy LiquidEnthalpy
        {
            get
            {
                if (cc.ThermoLiq != null)
                    return cc.ThermoLiq.H;
                else
                    return double.NaN;
            }
        }

        public Enthalpy VapourEnthalpy
        {
            get
            {
                if (cc.ThermoLiq != null)
                    return cc.ThermoVap.H;
                return double.NaN;
            }
        }

        private bool isPADraw;

        public double MW
        {
            get
            {
                return cc.MW();
            }
        }

        public Density Dens_Act
        {
            get
            {
                return PropertyCalcs.ActLiqDensity(cc, P, T);
            }
        }

        public double Vol_Flow_Act
        {
            get
            {
                return MF_ / Dens_Act;
            }
        }

        [Category("Flags")]
        public bool IsPADraw { get => isPADraw; set => isPADraw = value; }

        public void PSat(ThermoDynamicOptions thermo)
        {
            Props[ePropID.P].BaseValue = ThermodynamicsClass.CalcBubblePointP(cc, T, thermo);
        }

        public void TSat(ThermoDynamicOptions thermo)
        {
            Props[ePropID.T].BaseValue = ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
        }

        public Pressure IdealVapourPressure(ThermoDynamicOptions thermo)
        {
            thermo.KMethod = enumEquiKMethod.Antoine;
            return ThermodynamicsClass.CalcBubblePointP(cc, T, thermo);
        }

        /// <summary>
        /// Set and Normalise Mole Fractions
        /// </summary>
        /// <param name="flows"></param>
        /// <returns></returns>
        public double SetMolarFlows(double[] flows)
        {
            if (flows.Length != cc.Count)
            {
                System.Windows.Forms.MessageBox.Show("Component Numbers don't match");
                return 0;
            }

            double sum = flows.Sum();
            MolarFlow_ = new(ePropID.MOLEF, sum, SourceEnum.Input);

            for (int i = 0; i < flows.Length; i++)
                cc[i].MoleFraction = flows[i] / sum;

            NormaliseMoleFractions();

            return sum;
        }

        public void AddRange(Components pcs)
        {
            foreach (BaseComp pc in pcs)
                if (!cc.Contains(pc))
                    this.cc.Add(pc.Clone());

            cc.UpdateArrayData();
        }

        public void NormaliseMoleFractions()
        {
            this.cc.NormaliseFractions(FlowFlag.Molar);
        }

        public MassFraction S_Frac
        {
            get
            {
                if (BulkAssayProperty.Keys.Contains(enumAssayPCProperty.SULFUR))
                    return ((MassFraction)BulkAssayProperty[enumAssayPCProperty.SULFUR]).BaseValue;
                else
                    return double.NaN;
            }
        }

        public MassFraction N_Frac
        {
            get
            {
                if (BulkAssayProperty.Keys.Contains(enumAssayPCProperty.N2))
                    return ((MassFraction)BulkAssayProperty[enumAssayPCProperty.N2]).BaseValue;
                else
                    return double.NaN;
            }
        }

        public Pressure IdealVapourPressure()
        {
            return IdealVapourPressure(this.Thermo);
        }

        public void TSat()
        {
            TSat(this.Thermo);
        }

        public void PSat()
        {
            PSat(this.Thermo);
        }
    }
}