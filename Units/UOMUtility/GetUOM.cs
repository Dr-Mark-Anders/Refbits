using Units.UOM;

namespace Units
{
    public class UOMUtility
    {
        public static IUOM GetUOM(ePropID prop)
        {
            IUOM UOM = null;

            switch (prop)
            {
                case ePropID.U:
                    UOM = new InternalEnergy();
                    break;

                case ePropID.A:
                    UOM = new Helmotz();
                    break;

                case ePropID.HForm25:
                case ePropID.H:
                    UOM = new Enthalpy();
                    break;

                case ePropID.Gibbsf25:
                case ePropID.Gibbs:
                    UOM = new Gibbs();
                    break;

                case ePropID.Entropyf25:
                case ePropID.S:
                    UOM = new Entropy();
                    break;

                case ePropID.SG:
                case ePropID.SG_ACT:
                    UOM = new Null();
                    break;

                case ePropID.Density_ACT:
                    UOM = new Density();
                    break;

                case ePropID.NullUnits:
                    UOM = new Quality();
                    break;

                case ePropID.T:
                    UOM = new Temperature();
                    break;

                case ePropID.P:
                    UOM = new Pressure();
                    break;

                case ePropID.MF:
                    UOM = new MassFlow();
                    break;

                case ePropID.MassEnthalpy:
                    UOM = new Enthalpy();
                    break;

                case ePropID.MassEntropy:
                    UOM = new Entropy();
                    break;

                case ePropID.Quality:
                case ePropID.Q:
                    UOM = new Quality();
                    break;

                case ePropID.VolFlow_ACT:
                case ePropID.LiquidVolumeFlow:
                case ePropID.VF:
                    UOM = new VolumeFlow();
                    break;

                case ePropID.MOLEF:
                    UOM = new MoleFlow();
                    break;

                case ePropID.DeltaT:
                    UOM = new DeltaTemperature();
                    break;

                case ePropID.DeltaP:
                    UOM = new DeltaPressure();
                    break;

                case ePropID.EnergyFlow:
                    UOM = new EnergyFlow();
                    break;

                case ePropID.Mass:
                    UOM = new Mass();
                    break;

                case ePropID.LHV:
                    UOM = new MassEnthalpy();
                    break;

                case ePropID.Density:
                    UOM = new Density();
                    break;

                case ePropID.SpecificVolume:
                    UOM = new SpecificMolarVolume();
                    break;

                case ePropID.DynViscosity:
                    UOM = new KinViscosity();
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
                    UOM = new KinViscosity();
                    break;

                case ePropID.Length:
                    UOM = new Length();
                    break;

                case ePropID.LiquidVolume:
                    UOM = new Volume();
                    break;

                case ePropID.Luminance:
                    break;

                case ePropID.MolarSpecificEnergy:
                    break;

                case ePropID.SpecificEnergy:
                    break;

                case ePropID.SurfaceTension:
                    UOM = new SurfaceTension();
                    break;

                case ePropID.ThermalConductivity:
                    break;

                case ePropID.Time:
                    UOM = new Time();
                    break;

                case ePropID.VapourVolume:
                    break;

                case ePropID.VapourVolumeFlow:
                    break;

                case ePropID.Velocity:
                    UOM = new Velocity();
                    break;

                case ePropID.Voltage:
                    break;

                case ePropID.VolumeRatio:
                    break;

                case ePropID.VolumeSpecificEnergy:
                    break;

                case ePropID.EnergyPrice:
                    break;

                case ePropID.MassPrice:
                    break;

                case ePropID.Percentage:
                    UOM = new Null();
                    break;

                case ePropID.Area:
                    UOM = new Area();
                    break;

                case ePropID.HeatTransferResistace:
                    break;

                case ePropID.UA:
                    UOM = new UA();
                    break;

                case ePropID.MolarHeatCapacity:
                    UOM = new HeatCapacity();
                    break;

                case ePropID.MassCp:
                    UOM = new MassHeatCapacity();
                    break;

                case ePropID.Value:
                    UOM = new Null();
                    break;

                default:
                    break;
            }
            return UOM;
        }
    }
}