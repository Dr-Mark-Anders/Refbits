using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units.UOM;

namespace ModelEngine.FCC.Petrof
{
    public class FCCDimensions
    {
        public Length Diameter = 1;
        public Length Height = 36.5;
        public Pressure Pressure = 2.5;
        public Volume Volume = 28.67;
        public Temperature Triser = 535;

        public FCCDimensions()
        {
        }

        public FCCDimensions(Length diameter, Length height, Pressure pressure, Volume volume, Temperature triser)
        {
            Diameter = diameter;
            Height = height;
            Pressure = pressure;
            Volume = volume;
            Triser = triser;
        }
    }
}




