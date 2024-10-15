using ModelEngine;
using System;

namespace Units
{
    [Serializable]
    internal class PASpec : Specification
    {
        public PASpec(string name, ePropID property, eSpecType type)
        {
            this.SpecName = name;
            this.engineSpecType = type;
            this.propID = property;
            this.flowtype = enumflowtype.StdLiqVol;  // default property
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