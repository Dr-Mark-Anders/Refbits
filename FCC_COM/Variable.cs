using ModelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units;

namespace FCC_COM
{
    public class Variables
    {
       public  UOMProperty HeatofCrackingParameter = new(1.75E-02);
       public  UOMProperty KineticCokeActivityFactor = new(8.22E-02);
       public  UOMProperty ActivityonPathwaystoCL = new(1.37416013);
       public  UOMProperty ActivityonPathwaystoGLump = new(0.585547062);
       public  UOMProperty ActivityonPathwaystoLL = new(1.614849436);
       public  UOMProperty MetalsCokeActivity = new(9.14E-05);
       public  UOMProperty CatalystDeactivationFactor = new(0.798693701);
       public  UOMProperty StripperParameter = new(2.345862485);
       public  UOMProperty EffluentperMassofCatalystintoStripper = new(65.78323847);
       public  UOMProperty HtoCRatioforCoke = new(0.849424232);
       public  UOMProperty PreexponentialFactorforGasolineCracking = new(64.5);
       public  UOMProperty Ea_RforGasolineCracking = new(60000);
       public  UOMProperty FractionConcarbontoCoke = new(0.5);
       public  UOMProperty LightGasDelumpingtoEthane = new(5.449309295);
       public  UOMProperty LightGasDelumpingtoEthylene = new(3.674933391);
       public  UOMProperty LightGasDelumpingtoPropane = new(5.658019987);
       public  UOMProperty LightGasDelumpingtoPropylene = new(2.797665583);
       public  UOMProperty LightGasDelumpingtonButane = new(7.866074943);
       public  UOMProperty LightGasDelumpingtoisoButane = new(4.219544288);
       public  UOMProperty LightGasDelumpingtoButenes = new(2.536169002);
       public  UOMProperty LightGasDelumpingtonPentane = new(2.712886687);
       public  UOMProperty LightGasDelumpingtoisoPentane = new(0.897379247);
       public  UOMProperty LightGasDelumpingtoPentenes = new(1.248849307);
       
       public  UOMProperty TotalLength = new(ePropID.Length, SourceEnum.Input, 119.7506562);
       public  UOMProperty Diameter = new(ePropID.Length, SourceEnum.Input, 3.280839895);
       
       public  UOMProperty Height = new(ePropID.Length, 26.24671916);
       public  UOMProperty TermDiameter = new(ePropID.Length, 9.842519685);
       public  UOMProperty AnnulusDiameter = new(ePropID.Length, 4.265091864);
        
       public  UOMProperty DenseBedHeight = new(ePropID.Length, 14.76377953);
       public  UOMProperty DenseBedDiameter = new(ePropID.Length, 24.9343832);
       public  UOMProperty DilutePhaseDiameter = new(ePropID.Length, 24.9343832);
       public  UOMProperty InterfaceDiameter = new(ePropID.Length, 24.9343832);
       public  UOMProperty CycloneInletHeight = new(ePropID.Length, 49.21259843);
       public  UOMProperty CycloneInletDiameter = new(ePropID.Length, 7.545931759);
       public  UOMProperty CycloneOutletDiameter = new(ePropID.Length, 4.265091864);
        
       public  UOMProperty C1C4lump = new(0);
       public  UOMProperty C5430lump = new(0);
       public  UOMProperty _430_650Paraffins = new(3.16);
       public  UOMProperty _430_650Naphthenes = new(3.19);
       public  UOMProperty _430_650Aromaticsidechains = new(2.87);
       public  UOMProperty _430_650Oneringaromatics = new(0.89);
       public  UOMProperty _430_650Tworingaromatics = new(0.53);
       public  UOMProperty _650_950Paraffins = new(13.2);
       public  UOMProperty _650_950Naphthenes = new(15.97);
       public  UOMProperty _650_950Aromaticsidechain = new(21.62);
       public  UOMProperty _650_950Oneringaromatics = new(3.54);
       public  UOMProperty _650_950Tworingaromatics = new(4.23);
       public  UOMProperty _650_950ThreePlusringaromatics = new(3.66);
       public  UOMProperty _950PlusParaffins = new(1.19);
       public  UOMProperty _950PlusNaphthenes = new(8.96);
       public  UOMProperty _950PlusAromaticsidechains = new(12.4);
       public  UOMProperty _950PlusOneringaromatics = new(0.6);
       public  UOMProperty _950PlusTworingaromatics = new(1.48);
       public  UOMProperty _950PlusThreeringaromatics = new(2.5);
        
       public  UOMProperty RiserOutletTemperature = new(1015.736104);
       public  UOMProperty ReactorPlenumTemperature = new(1013);
       public  UOMProperty CatalystCirculationRate = new(4596476.182);
       public  UOMProperty Cat_OilRatio = new(9.075544612);
        
       public  UOMProperty LiftGasVolume = new(0);
       public  UOMProperty LiftGasMass = new(0);
       public  UOMProperty LiftGasTemperature = new(80.006);
       public  UOMProperty LiftGasPressure = new(22.00000001);
        
       public  UOMProperty StrippingSteamMassFlow = new(13878.80988);
       public  UOMProperty StrippingSteamTemperature = new(392);
       public  UOMProperty StrippingSteamPressure = new(145.0377);
       public  UOMProperty RatiotoCatalystCirculationRate = new(3);
        
       public  UOMProperty RisertoRegeneratorRatio = new(0);
       public  UOMProperty SpentCatalysttoRegenerator = new(4629655.404);
       public  UOMProperty SpentCatalysttoRiser = new(0);
        
       public  UOMProperty DenseBedTemperature = new(1373.779833);
       public  UOMProperty CycloneTemperature = new(1381.122026);
       public  UOMProperty FlueGasTemperature = new(1381.122026);
       public  UOMProperty FlueGasDenseBedDeltaT = new(7.342193226);
       public  UOMProperty FlueGasO2Dry = new(1);
       public  UOMProperty FlueGasCODry = new(7.82E-02);
       public  UOMProperty FlueGasCO2Dry = new(16.7731859);
       public  UOMProperty FlueGasCO_CO2Ratio = new(4.66E-03);
       public  UOMProperty FlueGasSOxDry = new(443.3827496);
       public  UOMProperty CarbononRegenCat = new(4.95E-02);
       public  UOMProperty AirVolumeFlowWet = new(146.9814722);
       public  UOMProperty AirMassFlowWet = new(461890.2385);
       public  UOMProperty EnrichO2VolumeFlow = new(0);
       public  UOMProperty EnrichO2MassFlow = new(0);
       public  UOMProperty EnrichO2Pressure = new(14.6488077);
       public  UOMProperty EnrichO2Temperature = new(212);
       public  UOMProperty CatalystCoolerDuty = new(0);
       public  UOMProperty AirBlowerDischargeTemp = new(392);
       public  UOMProperty DenseBedBulkDensity = new(33.71112);
       public  UOMProperty CatalystInventory = new(243026.037);
       public  UOMProperty FlueQuenchWaterRate = new(0);
       public  UOMProperty FlueQuenchWaterTemp = new(219.9999878);
       public  UOMProperty FlueQuenchWaterPressure = new(99.69627016);
        
       public  UOMProperty CatalystHeatCapacity = new(0.26273049);
       public  UOMProperty CokeHeatCapacity = new(0.398872653);

        public Variables()
        {
        }

    }
}
