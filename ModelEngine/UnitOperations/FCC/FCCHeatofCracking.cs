using Units.UOM;

namespace ModelEngine
{
    public class FCCHeatofCracking
    {
        public FCCHeatofCracking()
        {
        }

        public EnergyFlow Solve(MassFlow Feedrate, MassEnthalpy FeedEnthalpy, MassEnthalpy FeedEnthalpyTriser, Temperature MixedRiserInlet, Temperature RiserTop, MassFlow SteamIn, MassEnthalpy SteamH, MassEnthalpy SteamHOut, MassFlow CatFlow, MassHeatCapacity CatCp, MassEnthalpy Cabsorb, MassFlow CatCoke, EnergyFlow HeatLoss)
        {

            EnergyFlow HeatIn = (double)CatFlow*CatCp*MixedRiserInlet;
            HeatIn += Feedrate * FeedEnthalpy;
            HeatIn += SteamIn * SteamH;
            HeatIn += Cabsorb * CatCoke;

            EnergyFlow HeatOut = (double)CatFlow * CatCp * RiserTop;
            HeatOut += Feedrate * FeedEnthalpyTriser;
            HeatOut += SteamIn * SteamHOut;
            HeatOut += HeatLoss;

            EnergyFlow Balance = HeatIn - HeatOut;

            double HofC = Balance / Feedrate;

            return HofC;
        }

        public static MassEnthalpy HeatOfCrackMass(Pressure P, Temperature T, Components CC1, Components CC2)
        {
            MassEnthalpy A = new(), B = new();
            A.BaseValue = (ThermodynamicsClass.BulkStreamEnthalpy(CC1, CC1.MoleFractions, P, T, enumFluidRegion.Vapour, CC1.Thermo)/CC1.MW());
            B.BaseValue = (ThermodynamicsClass.BulkStreamEnthalpy(CC2, CC2.MoleFractions, P, T, enumFluidRegion.Vapour, CC2.Thermo)/CC2.MW());
            return A - B;
        }

    }
}
