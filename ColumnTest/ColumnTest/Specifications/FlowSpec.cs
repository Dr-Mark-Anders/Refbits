using ModelEngine;
using System;
using System.Runtime.Serialization;
using Units;

namespace UnitsTest
{
    [Serializable]
    public class FlowSpecTest : Specification, ISerializable
    {
        public FlowSpecTest(string name, double Value, ePropID property, eSpecType type)
        {
            this.SpecName = name;
            this.engineSpecType = type;
            this.propID = property;
            this.flowtype = enumflowtype.StdLiqVol;  // default property
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", SpecName);
            info.AddValue("specType", graphicSpecType);
            info.AddValue("currentUnits", CurrentUnits);
            info.AddValue("stage", engineStageGuid);
            //info.AddValue("liquid", IsLiquid);
            //info.AddValue("issideproduct", IsSideProduct);
            //info.AddValue("isPumparound", IsPumparoundSpec);
            info.AddValue("guid", Guid);
            info.AddValue("isactive", IsActive);
        }

        public FlowSpecTest(SerializationInfo info, StreamingContext context)
        {
            try
            {
                SpecName = info.GetString("name");
                graphicSpecType = (eSpecType)info.GetValue("specType", typeof(eSpecType));
                engineStageGuid = (Guid)info.GetValue("stage", typeof(Guid));
                //IsLiquid = info.Getbool ean("liquid");
                //IsSideProduct = info.Getbool ean("issideproduct");
                //IsPumparoundSpec = info.Getbool ean("isPumparound");
                Guid = (Guid)info.GetValue("guid", typeof(Guid));
                IsActive = info.GetBoolean("isactive");
            }
            catch
            {
            }
        }

        private enumflowtype flowtype = enumflowtype.StdLiqVol;

        public new enumflowtype FlowType
        {
            get { return flowtype; }
            set
            {
                flowtype = value;
                switch (flowtype)
                {
                    case enumflowtype.StdLiqVol:
                        this.propID = ePropID.VF;
                        break;

                    case enumflowtype.Mass:
                        this.propID = ePropID.MF;
                        break;

                    case enumflowtype.Molar:
                        this.propID = ePropID.MOLEF;
                        break;
                }
                this.Units = UOM.DefaultUnit;
            }
        }
    }
}