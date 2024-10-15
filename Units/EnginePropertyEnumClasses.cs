using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Units
{
    public enum ePropID
    {
        NullUnits, T, P, H, S, Z, F, MOLEF, MF, VF, Q, SG, Density, FUG, DeltaP,
        SG_ACT, VolFlow_ACT, Density_ACT,

        DeltaT, EnergyFlow, LiquidVolumeFlow,
        Mass, LHV, SpecificVolume, DynViscosity, ElectricalFlow, Fueloil,
        HeatFlowRate, HeatFlux, KinViscosity, Length, LiquidVolume, Luminance,
        MolarSpecificEnergy, SpecificEnergy, SurfaceTension, ThermalConductivity,
        Time, VapourVolume, VapourVolumeFlow, Velocity, Voltage, VolumeRatio, VolumeSpecificEnergy,
        MassEnthalpy, MassEntropy, EnergyPrice, MassPrice, Quality, Percentage, Area,
        HeatTransferResistace,
        MolarHeatCapacity,
        MassCp,
        UA,
        Value,
        Components,
        HForm25,
        Gibbs,
        Entropyf25,
        Gibbsf25,
        U,
        A,
        Error,
        Undefined
    }

    public static class Enumhelpers
    {
        public static int SigFigures(ePropID pe)
        {
            switch (pe)
            {
                case ePropID.T:
                    return 1;

                default:
                    return 1;
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static IEnumerable<PropertyEnum> EnumToList<PropertyEnum>()
        {
            Type enumType = typeof(PropertyEnum);

            // Can't use generic type constraint s on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<PropertyEnum> enumValList = new List<PropertyEnum>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((PropertyEnum)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }
}