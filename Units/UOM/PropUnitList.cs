using System;
using System.Runtime.Serialization;
using Units.UOM;

namespace Units
{
    [Serializable]
    public class UOMDisplayList : ISerializable
    {
        private TemperatureUnit t = TemperatureUnit.Celsius;
        private PressureUnit p = PressureUnit.BarA;
        private MassFlowUnit mf = MassFlowUnit.kg_hr;
        private VolFlowUnit vf = VolFlowUnit.m3_hr;
        private MoleFlowUnit moleF = MoleFlowUnit.kgmol_hr;
        private EnthalpyUnit h = EnthalpyUnit.kJ_kgmole;
        private EntropyUnit s = EntropyUnit.J_mole_K;
        private QualityUnit q = QualityUnit.Quality;

        public TemperatureUnit T { get => t; set => t = value; }
        public PressureUnit P { get => p; set => p = value; }
        public MassFlowUnit MF { get => mf; set => mf = value; }
        public VolFlowUnit VF { get => vf; set => vf = value; }
        public MoleFlowUnit MoleF { get => moleF; set => moleF = value; }
        public EnthalpyUnit H { get => h; set => h = value; }
        public EntropyUnit S { get => s; set => s = value; }
        public QualityUnit Q { get => q; set => q = value; }

        protected UOMDisplayList(SerializationInfo info, StreamingContext streamingContext)
        {
            t = (TemperatureUnit)info.GetValue("T", typeof(TemperatureUnit));
            p = (PressureUnit)info.GetValue("P", typeof(PressureUnit));
            s = (EntropyUnit)info.GetValue("S", typeof(EntropyUnit));
            h = (EnthalpyUnit)info.GetValue("H", typeof(EnthalpyUnit));
            mf = (MassFlowUnit)info.GetValue("MF", typeof(MassFlowUnit));
            vf = (VolFlowUnit)info.GetValue("VF", typeof(VolFlowUnit));
            moleF = (MoleFlowUnit)info.GetValue("MoleF", typeof(MoleFlowUnit));
        }

        public UOMDisplayList()
        {
        }

        public string DisplayUnit(ePropID propid)
        {
            string res = null;
            switch (propid)
            {
                case ePropID.NullUnits:
                    res = p.ToString();
                    break;

                case ePropID.T:
                    res = t.ToString();
                    break;

                case ePropID.P:
                    res = p.ToString();
                    break;

                case ePropID.H:
                    res = h.ToString();
                    break;

                case ePropID.S:
                    res = s.ToString();
                    break;

                case ePropID.Z:
                    break;

                case ePropID.F:
                    break;

                case ePropID.MOLEF:
                    res = moleF.ToString();
                    break;

                case ePropID.MF:
                    res = mf.ToString();
                    break;

                case ePropID.VF:
                case ePropID.LiquidVolumeFlow:
                    res = vf.ToString();
                    break;

                case ePropID.Q:
                    res = q.ToString();
                    break;

                case ePropID.SG:
                    break;

                case ePropID.FUG:
                    break;

                case ePropID.EnergyFlow:
                    break;

                case ePropID.DeltaP:
                    break;

                case ePropID.DeltaT:
                    break;

                case ePropID.Mass:
                    break;

                case ePropID.LHV:
                    break;

                case ePropID.Density:
                    break;

                case ePropID.SpecificVolume:
                    break;

                case ePropID.DynViscosity:
                    break;

                case ePropID.ElectricalFlow:
                    break;

                case ePropID.Fueloil:
                    break;

                case ePropID.HeatFlowRate:
                    break;

                case ePropID.HeatFlux:
                    break;

                case ePropID.KinViscosity:
                    break;

                case ePropID.Length:
                    break;

                case ePropID.LiquidVolume:
                    break;

                case ePropID.Luminance:
                    break;

                case ePropID.MolarSpecificEnergy:
                    break;

                case ePropID.SpecificEnergy:
                    break;

                case ePropID.SurfaceTension:
                    break;

                case ePropID.ThermalConductivity:
                    break;

                case ePropID.Time:
                    break;

                case ePropID.VapourVolume:
                    break;

                case ePropID.VapourVolumeFlow:
                    break;

                case ePropID.Velocity:
                    break;

                case ePropID.Voltage:
                    break;

                case ePropID.VolumeRatio:
                    break;

                case ePropID.VolumeSpecificEnergy:
                    break;

                case ePropID.MassEnthalpy:
                    break;

                case ePropID.MassEntropy:
                    break;

                case ePropID.EnergyPrice:
                    break;

                case ePropID.MassPrice:
                    break;

                case ePropID.Quality:
                    break;

                case ePropID.Percentage:
                    break;

                case ePropID.Area:
                    break;

                case ePropID.HeatTransferResistace:
                    break;

                case ePropID.MolarHeatCapacity:
                    break;

                case ePropID.MassCp:
                    break;

                default:
                    break;
            }
            if (res is null)
            {
            }

            return res;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("T", t);
            info.AddValue("P", p);
            info.AddValue("MF", mf);
            info.AddValue("VF", vf);
            info.AddValue("MoleF", moleF);
            info.AddValue("H", h);
            info.AddValue("S", s);
        }
    }
}