using ModelEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class DataStoreUnitOP : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new("HeaterIn", FlowDirection.IN);

        public DataStoreUnitOP() : base()
        {
            Add(PortIn);
        }

        protected DataStoreUnitOP(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public List<BaseComp> Comps
        {
            get { return Ports["In1"].cc.ComponentList; }
        }
    }
}