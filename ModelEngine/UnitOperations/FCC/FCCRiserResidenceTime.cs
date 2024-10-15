using System;
using Units.UOM;

namespace ModelEngine
{
    internal class RiserResidenceTime
    {
        MassFlow feedFlow;
        Volume riserVolume;
        SpecificMolarVolume feedVol;
        VolumeFlow hCVolumeFlow;
        VolumeFlow catalystVolFlow;
        VolumeFlow totalVolumnFlow;
        Time residenceTime;
        Mass catRiserHoldup;
        Length height;
        Length diameter;
        Density catDensity;


        public RiserResidenceTime(Length Height, Length Diameter, Density CatDensity)
        {
            this.height = Height;
            this.diameter = Diameter;
            this.catDensity = CatDensity;
        }

        public VolumeFlow HCVolumeFlow { get => hCVolumeFlow; set => hCVolumeFlow = value; }
        public VolumeFlow CatalystVolFlow { get => catalystVolFlow; set => catalystVolFlow = value; }
        public VolumeFlow TotalVolumnFlow { get => totalVolumnFlow; set => totalVolumnFlow = value; }
        public Time ResidenceTime { get => residenceTime; set => residenceTime = value; }
        public Mass CatRiserHoldup { get => catRiserHoldup; set => catRiserHoldup = value; }

        public double Solve(Port_Material Feed, Pressure RiserP,Temperature MixTemp, double CatOilRatio, double SlipFactor)
        {
            feedFlow = (MassFlow)Feed.MF_.UOM;

            riserVolume = (double)height * (diameter / 2).Pow(2) * Math.PI;

            Components cc = Feed.cc;

            feedVol = PropertyCalcs.IdealGasVolume(RiserP, MixTemp)._m3_mole;  //m3/mole

            hCVolumeFlow = feedVol * Feed.MolarFlow_;

            catalystVolFlow = CatOilRatio * feedFlow  / catDensity * SlipFactor;  //m3/te feed
            
            totalVolumnFlow = hCVolumeFlow + catalystVolFlow;

            residenceTime = riserVolume / totalVolumnFlow; // seconds

            catRiserHoldup = (double)riserVolume / residenceTime * catDensity;

            return residenceTime;
        }
    }
}