using Units.UOM;

namespace ModelEngine
{
    internal class FCCCat_OilMixer
    {
        public Temperature Solve(MassFlow Catalyst, MassHeatCapacity CatCp, 
            Port_Material Oil, Temperature CatT, Pressure P)
        {
            Temperature Res;
            EnergyFlow CatHeat = Catalyst.kg_hr * CatCp.kJ_kg_C;
            Oil.Flash();
            Enthalpy H = Oil.H_.BaseValue;
            H += CatHeat;

            Temperature OilT = Oil.T_.BaseValue;
            MassFlow OilMF= Oil.MF_.BaseValue;

            MassHeatCapacity OilCP = Oil.CP_MASS();

            double CombinedT = OilCP.kJ_kg_C * OilMF.kg_hr * OilT.Kelvin + Catalyst.kg_hr * CatCp.kJ_kg_C * CatT.Kelvin;

           // MassEnthalpy TotEnthlpy = new MassEnthalpy(ePropID.MassEnthalpy, Total);


            Oil.H_ = new StreamProperty(Units.ePropID.H,H,SourceEnum.Input);
            Oil.Flash();

            Res = Oil.T_.BaseValue;
            return Res;
        }
    }
}
