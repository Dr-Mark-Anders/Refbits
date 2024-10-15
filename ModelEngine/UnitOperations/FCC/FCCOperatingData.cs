using Units.UOM;

namespace ModelEngine.UnitOperations.FCC
{
    public class FCCOperatingData
    {
        Temperature RiserT, AmbientAirT;
        Pressure FeedP, RiserP, RegenP, AmbientPressure;
        MassFlow FeedRate, CatCircRate, FreshMakeUpRate;
        Mass CatInventory;
        Null CatMAT;
        MassFraction Vanadium,Nickel,Sodium,Iron,Copper;
        MassFlow SteamMass; Temperature SteamTemperature; Pressure SteamPressure; // dispersion Steam
        Temperature DenseBedTemperature, CycloneTemperature, FlueGasTemperature, FlueGasDenseBedDeltaT;
        MoleFraction FlueGasO2DryPCT, FlueGasCODryPCT, FlueGasCO2Dry, FlueGasCO_CO2Ratio, FlueGasSOxDry;
        MassFraction CarbononRegenCat;
        VolumeFlow AirVolumeFlowWet, AirMassFlowWet, EnrichO2VolumeFlow;
        MassFlow EnrichO2MassFlow; Pressure EnrichO2Pressure; Temperature EnrichO2Temperature; EnergyFlow CatalystCoolerDuty; Temperature AirBlowerDischargeTemp;
        Density DenseBedBulkDensity; Mass CatalystInventory;MassFlow FlueQuenchWaterRate;Temperature FlueQuenchWaterTemp;Pressure FlueQuenchWaterPressure;
        Pressure ReactorPressure, RegeneratorPressure;

        public FCCOperatingData()
        {
        }

        public Temperature RiserT1 { get => RiserT; set => RiserT = value; }
        public Temperature AmbientAirT1 { get => AmbientAirT; set => AmbientAirT = value; }
        public Pressure FeedP1 { get => FeedP; set => FeedP = value; }
        public Pressure RiserP1 { get => RiserP; set => RiserP = value; }
        public Pressure RegenP1 { get => RegenP; set => RegenP = value; }
        public Pressure AmbientPressure1 { get => AmbientPressure; set => AmbientPressure = value; }
        public MassFlow FeedRate1 { get => FeedRate; set => FeedRate = value; }
        public MassFlow CatCircRate1 { get => CatCircRate; set => CatCircRate = value; }
        public MassFlow FreshMakeUpRate1 { get => FreshMakeUpRate; set => FreshMakeUpRate = value; }
        public Mass CatInventory1 { get => CatInventory; set => CatInventory = value; }
        public Null CatMAT1 { get => CatMAT; set => CatMAT = value; }
        public MassFraction Vanadium1 { get => Vanadium; set => Vanadium = value; }
        public MassFraction Nickel1 { get => Nickel; set => Nickel = value; }
        public MassFraction Sodium1 { get => Sodium; set => Sodium = value; }
        public MassFraction Iron1 { get => Iron; set => Iron = value; }
        public MassFraction Copper1 { get => Copper; set => Copper = value; }
        public MassFlow SteamMass1 { get => SteamMass; set => SteamMass = value; }
        public Temperature SteamTemperature1 { get => SteamTemperature; set => SteamTemperature = value; }
        public Pressure SteamPressure1 { get => SteamPressure; set => SteamPressure = value; }
        public Temperature DenseBedTemperature1 { get => DenseBedTemperature; set => DenseBedTemperature = value; }
        public Temperature CycloneTemperature1 { get => CycloneTemperature; set => CycloneTemperature = value; }
        public Temperature FlueGasTemperature1 { get => FlueGasTemperature; set => FlueGasTemperature = value; }
        public Temperature FlueGasDenseBedDeltaT1 { get => FlueGasDenseBedDeltaT; set => FlueGasDenseBedDeltaT = value; }
        public MoleFraction FlueGasO2DryPCT1 { get => FlueGasO2DryPCT; set => FlueGasO2DryPCT = value; }
        public MoleFraction FlueGasCODryPCT1 { get => FlueGasCODryPCT; set => FlueGasCODryPCT = value; }
        public MoleFraction FlueGasCO2Dry1 { get => FlueGasCO2Dry; set => FlueGasCO2Dry = value; }
        public MoleFraction FlueGasCO_CO2Ratio1 { get => FlueGasCO_CO2Ratio; set => FlueGasCO_CO2Ratio = value; }
        public MoleFraction FlueGasSOxDry1 { get => FlueGasSOxDry; set => FlueGasSOxDry = value; }
        public MassFraction CarbononRegenCat1 { get => CarbononRegenCat; set => CarbononRegenCat = value; }
        public VolumeFlow AirVolumeFlowWet1 { get => AirVolumeFlowWet; set => AirVolumeFlowWet = value; }
        public VolumeFlow AirMassFlowWet1 { get => AirMassFlowWet; set => AirMassFlowWet = value; }
        public VolumeFlow EnrichO2VolumeFlow1 { get => EnrichO2VolumeFlow; set => EnrichO2VolumeFlow = value; }
        public MassFlow EnrichO2MassFlow1 { get => EnrichO2MassFlow; set => EnrichO2MassFlow = value; }
        public Pressure EnrichO2Pressure1 { get => EnrichO2Pressure; set => EnrichO2Pressure = value; }
        public Temperature EnrichO2Temperature1 { get => EnrichO2Temperature; set => EnrichO2Temperature = value; }
        public EnergyFlow CatalystCoolerDuty1 { get => CatalystCoolerDuty; set => CatalystCoolerDuty = value; }
        public Temperature AirBlowerDischargeTemp1 { get => AirBlowerDischargeTemp; set => AirBlowerDischargeTemp = value; }
        public Density DenseBedBulkDensity1 { get => DenseBedBulkDensity; set => DenseBedBulkDensity = value; }
        public Mass CatalystInventory1 { get => CatalystInventory; set => CatalystInventory = value; }
        public MassFlow FlueQuenchWaterRate1 { get => FlueQuenchWaterRate; set => FlueQuenchWaterRate = value; }
        public Temperature FlueQuenchWaterTemp1 { get => FlueQuenchWaterTemp; set => FlueQuenchWaterTemp = value; }
        public Pressure FlueQuenchWaterPressure1 { get => FlueQuenchWaterPressure; set => FlueQuenchWaterPressure = value; }
        public Pressure ReactorPressure1 { get => ReactorPressure; set => ReactorPressure = value; }
        public Pressure RegeneratorPressure1 { get => RegeneratorPressure; set => RegeneratorPressure = value; }
    }
}
