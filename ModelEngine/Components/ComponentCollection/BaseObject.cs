using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngine
{
    public enum flowBasis
    { mass, molar, volume, none }

    [Serializable]
    public class BaseObject : ISerializable
    {
        internal string name;

        //internal ePropIDtemporrarypropid=ePropID.NullUnits;
        public Guid guid = Guid.NewGuid();

        internal Guid _originUnitOPGuid;
        internal Guid _originPortGuid;
        public SourceEnum origin = SourceEnum.Empty;

        [Browsable(false)]
        public virtual double Value
        {
            get
            {
                return double.NaN;
            }
            set
            {
            }
        }

        [Browsable(false)]
        public BaseObject()
        { }

        [Browsable(false)]
        public virtual string Name { get => name; set => name = value; }

        [Browsable(false)]
        public virtual Guid Guid
        { get => guid; set => guid = value; }

        [Browsable(false)]
        public virtual Guid OriginPortGuid
        { get => _originPortGuid; set => _originPortGuid = value; }

        [Browsable(false)]
        public virtual Guid OriginUnitOPGuid
        { get => _originUnitOPGuid; set => _originUnitOPGuid = value; }

        [Browsable(false)]
        public virtual SourceEnum Source
        { get => origin; set => origin = value; }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("origin", origin);
            info.AddValue("name", name);
            info.AddValue("_originPortGuid", _originPortGuid);
            info.AddValue("_originUnitOPGuid", _originUnitOPGuid);
        }

        public BaseObject(SerializationInfo info, StreamingContext context)
        {
            
            try
            {
                origin = (SourceEnum)info.GetValue("origin", typeof(SourceEnum));
                name = info.GetString("name");
                _originPortGuid = (Guid)info.GetValue("_originPortGuid", typeof(Guid));
                _originUnitOPGuid = (Guid)info.GetValue("_originUnitOPGuid", typeof(Guid));
            }
            catch { }
        }

        public BaseObject(string name)
        {
            this.name = name;
        }
    }
}