using ModelEngine;
using System;
using Units;

namespace ModelEngine
{
    public enum Inconsistencytype
    { None, Property, Component }

    public class InconsistencyObject
    {
        public StreamProperty property;
        public Inconsistencytype type = Inconsistencytype.None;
        public BaseComp bc;
        public Port_Material SourcePort, DestPort;
        public string name;
        public ePropID propid;
        public double value;

        public Guid guid
        {
            get
            {
                switch (type)
                {
                    case Inconsistencytype.None:
                        break;

                    case Inconsistencytype.Property:
                        return property.guid;

                    case Inconsistencytype.Component:
                        return bc.guid;

                    default:
                        break;
                }
                return Guid.Empty;
            }
        }

        public string Name { get => name; set => name = value; }
        public BaseComp Bc { get => bc; set => bc = value; }
        internal Inconsistencytype Type { get => type; set => type = value; }
        public ePropID Propid { get => propid; set => propid = value; }
        public double Value { get => value; set => this.value = value; }

        public InconsistencyObject(StreamProperty property, Port sourceport, Port destport)
        {
            this.property = property;
            name = property.Name;
            type = Inconsistencytype.Property;
            value = property.Value;
            propid = property.Propid;
            this.property = property;

            if (sourceport is Port_Material)
                this.SourcePort = (Port_Material)sourceport;
            if (destport is Port_Material)
                this.DestPort = (Port_Material)destport;
        }

        public InconsistencyObject(BaseComp bc, ePropID propid, Port sourceport, Port destport)
        {
            name = bc.Name;
            this.propid = propid;
            type = Inconsistencytype.Component;
            value = bc.MoleFraction;
            this.bc = bc;

            this.SourcePort = (Port_Material)sourceport;
            this.DestPort = (Port_Material)destport;
        }
    }
}