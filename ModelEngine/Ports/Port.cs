using ModelEngine.Ports.Events;
using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Serialization;
using Units;

public enum FlowDirection
{ IN, OUT, ALL, Up, Down };

namespace ModelEngine
{
    [TypeConverter(typeof(Port)), Description("Expand to see Port Details")]
    [Serializable]
    public class PortExpander : ExpandableObjectConverter //, ISerializable
    {
        public PortExpander()
        { }
    }

    [Serializable]
    public partial class Port : PortExpander, ISerializable
    {
        public FlowDirection flowdirection;
        private UnitOperation owner;
        private FlowSheet fs;
        private Port streamport = null;

        public void ConnectPort(BaseStream Stream)
        {
            /*if (Stream.Owner is not null && this.Owner is not null)
            {
                Debug.Print ("StreamPortValueChanged Delegate Added: " + this.Owner.Name + " " + this.Name + " " + Stream.Name);
            }*/

            switch (Stream)
            {
                case StreamMaterial sm:
                    streamport = sm.Port;
                    //streamport.StreamPortValueChanged -= Streamport_PortValueChanged;
                    streamport.PropertyChangedHandler += PropertyChanged;
                    streamport.PortCompositionChangedHandler += RaiseCompositionChanged;

                    break;

                case StreamEnergy se:
                    streamport = se.Port;
                    //streamport.StreamPortValueChanged -= Streamport_PortValueChanged;
                    streamport.PropertyChangedHandler += PropertyChanged;
                    streamport.PortCompositionChangedHandler += RaiseCompositionChanged;
                    break;

                case StreamSignal ss:
                    ss.Port.ConnectedPortNext = this;
                    streamport.PropertyChangedHandler += PropertyChanged;
                    streamport.PortCompositionChangedHandler += RaiseCompositionChanged;
                    break;
            }
        }

        public void ConnectPort(Port port)
        {
            if (port is not null && this != port) // do not allow a port to connect to itself, creates infinite loop
            {
                streamport = port;
                //port.StreamValueChangedHandler += StreamValueChanged;
            }
        }

        public bool SetPortValue(ePropID id, double value, SourceEnum origin, bool RaiseChangeEvent = true)
        {
            return SetPortValue(id, value, origin, this.Guid, null, RaiseChangeEvent);
        }

        public bool SetPortValue(ePropID id, double value, SourceEnum origin, UnitOperation UO, bool RaiseChangeEvent = true)
        {
            return SetPortValue(id, value, origin, this.Guid, UO, RaiseChangeEvent);
        }

        /// <summary>
        /// Value is either from UO or PortCalc, if from PortCalc, Guid is Port Guid
        /// </summary>
        public bool SetPortValue(ePropID id, double value, SourceEnum origin, Guid PortGuid, UnitOperation UO, bool RaiseChangeEvent = true)
        {
            StreamProperty OldValue = null;
            Port lowestevelport = null;

            if (double.IsNaN(value))
            {
                if (this is Port_Material pm)
                {
                    if (pm.Properties[id].origin == SourceEnum.UnitOpCalcResult)
                    {
                        pm.Properties[id].Clear();
                    }
                    OldValue = pm.Properties[id];

                    if(!OldValue.IsKnown) // both values NAN
                        return false;
                }

                if (this is Port_Signal ps)
                {
                    ps.Value.Clear();
                    OldValue = ps.Value;
                }

                Port tport;

                if(streamport is not null)
                    tport = streamport as Port;
                else
                    tport = this as Port;

                if (RaiseChangeEvent)
                    RaiseStreamValueChangedEvent(this, new PropertyEventArgs(this, UO, OldValue, tport));

                return false;
            }

            StreamProperty newValue = new(id, value, origin);
            newValue.OriginPortGuid = PortGuid;
            if (UO is not null)
                newValue.OriginUnitOPGuid = UO.Guid;
            else
                newValue.OriginUnitOPGuid = Guid.NewGuid();

            lowestevelport = this.StreamPortLowestLevel;

            Port p;

            if (lowestevelport is not null)
                p = lowestevelport;
            else
                p = this;

            switch (p) // find the old property
            {
                case Port_Energy pe:
                    OldValue = pe.Value;  // values from attached port if not null;
                    break;

                case Port_Signal ps:
                    OldValue = ps.Value;  // values from attached port if not null;
                    break;

                case Port_Material pm:
                    OldValue = pm.Properties[newValue.Propid];  // values from attached port if not null;
                    break;
            }

            if (OldValue.SetPropValue(newValue)) // value chnaged, consistncy check
            {
                if (RaiseChangeEvent)
                {
                    p.RaiseStreamValueChangedEvent(this, new PropertyEventArgs(this, UO, OldValue, lowestevelport)); // Lowest level is OnDeserializedAttribute a stream
                }
                return true;
            }
            return false;
        }

        public virtual Port ConnectedPortNext
        {
            get
            {
                return streamport;
            }
            set
            {
                streamport = value;
            }
        }


        public virtual Port ConnectedPortFinal
        {
            get
            {
                if (streamport is not null && streamport.IsConnected)
                    return streamport.ConnectedPortFinal;
                else
                    return streamport;
            }
        }

        public void CopyConnectedPropsToPortMainProps()
        {
            switch (streamport)
            {
                case null:
                    break;

                case Port_Material pm:
                    StreamPropList Mainprops = ((Port_Material)this)._properties;
                    foreach (StreamProperty prop in pm.Props.Values)
                    {
                        Mainprops[prop.Propid] = (StreamProperty)prop.CloneDeep();
                    }
                    break;

                case Port_Energy pe:
                    ((Port_Energy)this).Value = pe.Value;
                    break;

                case Port_Signal ps:
                    ((Port_Signal)this).Value = ps.Value;
                    break;
            }
        }

        private bool isDirty = false;

        [Category("Flags")]
        public bool IsDirty
        {
            get
            {
                if (streamport is null)
                    return isDirty;
                else
                    return streamport.isDirty;
            }

            set
            {
                if (streamport is null)
                    this.isDirty = value;
                else
                    streamport.isDirty = value;
            }
        }

        public UnitOperation Owner
        {
            get
            {
                if (owner is null)
                    owner = RequestParent();
                return owner;
            }
            set
            {
                owner = value;
            }
        }

        protected Port(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Guid = (Guid)info.GetValue("guid", typeof(Guid));
            flowdirection = (FlowDirection)info.GetValue("FlowDirection", typeof(FlowDirection));

            try
            {
                isDirty = info.GetBoolean("IsDirty");
            }
            catch { }

            if (Guid == Guid.Empty)
                Guid = Guid.NewGuid();
        }

        public Port()
        {
            Guid = Guid.NewGuid();
        }

        public Port(string name, FlowDirection flowdirection)
        {
            Name = name;
            this.flowdirection = flowdirection;
            Guid = Guid.NewGuid();
        }

        public FlowSheet FS
        {
            get
            {
                if (fs is null)
                    switch (Owner)
                    {
                        case null:
                            return null;

                        case FlowSheet f:
                            fs = f;
                            return fs;

                        case UnitOperation uo:
                            return uo.Flowsheet;
                    }
                return fs;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public virtual string Name { get; set; }

        // as basecomponenet list as properties may change on each stream
        [Category("Flags")]
        public bool IsConnected
        {
            get
            {
                return streamport is not null;
            }
        }

        internal static int counter;

        [Category("Flags")]
        public bool IsFlowIn
        {
            get
            {
                if (flowdirection == FlowDirection.IN)
                    return true;
                else
                    return false;
            }
        }

        [Category("Flags")]
        public virtual bool IsFixed
        {
            get
            {
                return true;
            }
        }

        public Guid Guid { get; set; }

        [Browsable(true), Category("Nodes")]
        public Port StreamPort
        {
            get
            {
                return streamport;
            }
            set
            {
                streamport = value;
            }
        }

        [Browsable(true), Category("Nodes")]
        public Port StreamPortLowestLevel
        {
            get
            {
                if (streamport != null)
                {
                    if (streamport.streamport is null)
                    {
                        return streamport;
                    }
                    else if (streamport.streamport.streamport is null)
                    {
                        return streamport.streamport;
                    }
                    return streamport;
                }
                return null;
            }
            set
            {
                streamport = value;
            }
        }


        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("guid", Guid);
            info.AddValue("FlowDirection", flowdirection);
            info.AddValue("IsDirty", isDirty);
        }
    }
}