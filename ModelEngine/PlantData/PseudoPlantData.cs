using ModelEngine;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;
using System.Windows.Forms.DataVisualization.Charting;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class PseudoPlantData : BasePlantData, ISerializable
    {
        //Port_Material port = new();

        private DistPoints distpoints = new(true);
        private Dictionary<enumAssayPCProperty, double> properties = new();

        private Temperature ibp, fbp;
        private MassFlow massflow;
        private VolumeFlow volflow;
        private MoleFlow molflow;

        enumflowtype flowtype = enumflowtype.None;

        public PseudoPlantData()
        {
        }

        public Components Comps { get => Port.cc; set => Port.cc = value; }

        public PseudoPlantData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                distpoints = (DistPoints)info.GetValue("distpoints", typeof(DistPoints));
                properties = (Dictionary<enumAssayPCProperty, double>)info.GetValue("properties", typeof(Dictionary<enumAssayPCProperty, double>));
            }
            catch
            {
            }
        }

        public enumMassMolarOrVol GteFlowType()
        {
            if (double.IsNaN(massflow.BaseValue))
                return enumMassMolarOrVol.Mass;
            if (double.IsNaN(volflow))
                return enumMassMolarOrVol.Vol;
            if (double.IsNaN(molflow))
                return enumMassMolarOrVol.Molar;

            return enumMassMolarOrVol.notdefined;
        }

        public PseudoPlantData(DistPoints distpoints, UOMProperty density, Dictionary<enumAssayPCProperty, double> properties)
        {
            this.Distpoints = distpoints;
            this.Density = density;
            this.Properties = properties;
            CreatePort();
        }

        public PseudoPlantData(Components cc, List<double> distpoints, TemperatureUnit tempUnit, List<int> BPs, Density density, enumDistType distType, SourceEnum origin, MassFlow flow)
        {
            this.distpoints = new DistPoints(distpoints, tempUnit, BPs, density, distType, origin);
            this.massflow = flow;
            Port.cc = cc;
            flowtype = enumflowtype.Mass;
            CreatePort();
        }

        public PseudoPlantData(Components cc, List<double> distpoints, TemperatureUnit tempUnit, List<int> BPs, Density density, enumDistType distType, SourceEnum origin, VolumeFlow flow)
        {
            this.distpoints = new DistPoints(distpoints, tempUnit, BPs, density, distType, origin);
            this.volflow = flow;
            Port.cc = cc;
            flowtype = enumflowtype.StdLiqVol;
            CreatePort();
        }

        public PseudoPlantData(Components cc, List<double> distpoints, TemperatureUnit tempUnit, List<int> BPs, Density density, enumDistType distType, SourceEnum origin, MoleFlow flow)
        {
            this.distpoints = new DistPoints(distpoints, tempUnit, BPs, density, distType, origin);
            this.molflow = flow;
            Port.cc = cc;
            flowtype = enumflowtype.Molar;
            CreatePort();
        }

        private void CreatePort()
        {
            Port.cc.UpdateMoleFracs(Distpoints.ConvertToQuasiComps(Port.cc));

            switch (flowtype)
            {
                case enumflowtype.Mass:
                    Port.MF_.BaseValue = massflow.BaseValue;
                    Port.MF_.origin = SourceEnum.Input;
                    break;
                case enumflowtype.StdLiqVol:
                    Port.VF_.BaseValue = volflow.BaseValue;
                    Port.VF_.origin = SourceEnum.Input;
                    break;
                case enumflowtype.Molar:
                    Port.MolarFlow_.BaseValue = molflow;
                    Port.MolarFlow_.origin = SourceEnum.Input;
                    break;
                default:
                    Port.MolarFlow_.BaseValue = double.NaN;
                    break;
            }
        }

        public MoleFlow MoleFlow
        {
            get
            {
                return Port.MolarFlow_.BaseValue;
            }
        }


        public UOMProperty Density { get => distpoints.DENSITY; set => distpoints.DENSITY = value; }
        public DistPoints Distpoints { get => distpoints; set => distpoints = value; }
        public Dictionary<enumAssayPCProperty, double> Properties { get => properties; set => properties = value; }
        public Temperature IBP { get => ibp; set => ibp = value; }
        public Temperature FBP { get => fbp; set => fbp = value; }
  

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("distpoints", distpoints);
            info.AddValue("Density", Density, typeof(UOMProperty));
            info.AddValue("properties", properties);
        }

        internal void add(enumAssayPCProperty prop, IUOM value)
        {
            Port.Add(prop, value);
        }
    }
}