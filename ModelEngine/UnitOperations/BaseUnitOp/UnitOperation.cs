using ModelEngine.Ports.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Units;

namespace ModelEngine
{
    [TypeConverter(typeof(ExpandableObjectConverter)), Serializable]
    public partial class UnitOperation : FlowSheetExpander, ISerializable, IUnitOperation, ICloneable
    {
        private Guid guid = Guid.NewGuid();
        public Guid InterfaceGuid;
        private PortList ports = new();
        private string name = "";
        private bool isactive = true;
        private bool issolved = false;
        private bool isDirty = true;

        private UnitOperation owner = null;

        public Dictionary<string, object> unitPropList = new();
        public Port_Signal DP = new("DP", ePropID.DeltaP, FlowDirection.ALL);

        public virtual Port_Material GetPortIn
        {
            get
            {
                return ports["In1"];
            }
        }

        public virtual Port_Material GetPortOut
        {
            get
            {
                return ports["Out1"];
            }
        }

        public UnitOperation Owner
        {
            get
            {
                if (owner is null)
                    owner = GetParent();
                return owner;
            }
        }

        protected UnitOperation GetParent()
        {
            if (OnRequestParent == null)
                return null;
            return OnRequestParent();
        }

        [Browsable(true)]
        public FlowSheet Flowsheet
        {
            get
            {
                switch (Owner)
                {
                    case null:
                        return null;

                    case FlowSheet fs:
                        return fs;

                    default:
                        do
                        {
                            owner = Owner.GetParent();
                            if (owner is null)
                                return null;
                        } while (owner is not FlowSheet);
                        return (FlowSheet)owner;
                }
            }
        }

        public UnitOperation()
        {
        }

        protected UnitOperation(SerializationInfo info, StreamingContext context)
        {
            try
            {
                guid = (Guid)info.GetValue("UOGuid", typeof(Guid));
                name = info.GetString("name");
                IsDirty = info.GetBoolean("IsDirty");
                issolved = info.GetBoolean("IsSolved");
                isactive = info.GetBoolean("active");
            }
            catch { }
            //ports.list = (List<Port>)info.GetValue("Ports", typeof(List<Port>));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UOGuid", guid);
            info.AddValue("name", name);
            info.AddValue("IsDirty", IsDirty);
            info.AddValue("IsSolved", issolved);
            info.AddValue("active", isactive);
            //info.AddValue("Ports", ports.list, typeof(List<Port>));
        }

        public void SetParameterValue(string prop, object v)
        {
            unitPropList[prop] = v;
        }

        public virtual bool Solve()
        {
            FlashAllPorts();
            return true;
        }

        public string GetPath()
        {
            return "";
        }

        public void AddInletPort(Guid guid)
        {
            Port_Material p = new("In" + ports.Count, FlowDirection.IN);
            p.Guid = guid;
            ports.Add(p);
        }

        public void AddOutletPort(Guid guid)
        {
            Port_Material p = new("Out" + ports.Count, FlowDirection.OUT);
            p.Guid = guid;
            ports.Add(p);
        }

        public void Add(Port port)
        {
            port.Owner = this;
            if (port != null)
            {
                port.OnRequestParent += new Func<UnitOperation>(delegate { return this; });
                port.PortPropertyChanged -= PortValueChanged;
                port.PortPropertyChanged += PortValueChanged;
                port.PortCompositionChangedHandler += PortCompositionChanged;

            }
            else
                return;

            switch (port)
            {
                case Port_Material pm:
                    ports.materialList.Add(pm);
                    break;

                case Port_Energy pe:
                    ports.energyList.Add(pe);
                    break;

                case Port_Signal ps:
                    ports.signalList.Add(ps);
                    break;
            }
        }

        public virtual int FlashAllPorts()
        {
            int count = 0;

            foreach (Port_Material port in ports.materialList)
            {
                /*if (port.ConnectedPort is Port_Material pm)
                {
                    if (!pm.IsFlashed && pm.Flash()) // Only Flash once, loop while ports still flashing
                        count++;
                    else
                        pm.UpdateFlows()
                }
                else*/

                if (!port.IsFlashDefined && port.Flash()) // Only Flash once, loop while ports still flashing
                    count++;
                else
                    port.UpdateFlows();

               /* foreach (StreamProperty sp in port.Props.Values)
                {
                    if (sp.origin == SourceEnum.PortCalcResult)
                        sp.OriginPortGuid = port.Guid;
                }*/
            }

            return count;
        }

        public List<string> GetPortNames(FlowDirection direction)
        {
            List<string> names = new();
            foreach (var item in ports)
            {
                if (item.flowdirection == direction)
                    names.Add(item.Name);
            }

            return names;
        }

        public void ClearAttachedPorts()
        {
            ports.ClearAttachedPorts();
        }

        public Port_Material GetPort(string v)
        {
            return ports[v];
        }

        public Port_Signal GetSignalPort(string v)
        {
            foreach (var item in ports.signalList)
            {
                if (item.Name == v)
                    return item;
            }
            return null;
        }

        public void SetCompositionValues(string pname, List<double> comps, SourceEnum origin)
        {
            Port_Material p = ports[pname];
            for (int i = 0; i < comps.Count; i++)
                p.cc[i].MoleFraction = comps[i];

            p.cc.Origin = origin;
        }

        internal virtual void EraseAllFixedValues()
        {
            foreach (Port p in ports.AllPorts)
                p.Clear();
        }

        public virtual void EraseNonFixedValues()
        {
            foreach (Port p in ports.AllPorts)
                p.ClearNonInputs();
        }

        public void EraseNonFixedValues(Port_Material port)
        {
            if(port is not null)
                port.ClearNonInputs();
        }

        public virtual void EraseNonFixedComponents()
        {
            foreach (Port p in ports)
                if (p is Port_Material pm
                    && pm.cc.Origin != SourceEnum.Input)
                {
                    pm.cc.Origin = SourceEnum.Empty;
                }
        }

        public virtual void EraseCalcValues(SourceEnum origin)
        {
            foreach (Port p in ports.AllPorts)
            {
                p.ClearCalcValues(origin);
            }

            IsDirty = true;
        }

        public virtual void ClearInternals()
        {
            foreach (Port p in ports.AllPorts)
                p.ClearInternalProperties();
            IsDirty = true;
        }

        public virtual void EraseEstimates()
        {
            foreach (var p in ports.AllPorts)
                p.ClearEstimates();
        }

        public void SetPropertyPort()
        {
            foreach (Port port in ports.AllPorts)
            {
                switch (port)
                {
                    case Port_Material pm:
                        pm.Properties.SetPort(pm);
                        break;

                    case Port_Signal ps:
                        ps.Value.Port = ps;
                        break;

                    case Port_Energy pe:
                        pe.Value.Port = pe;
                        break;
                }
            }
        }

        public virtual void EraseCalcEstimates()
        {
            foreach (var p in ports.AllPorts)
                p.ClearCalcEstimates();
        }

        public void AddComponents(Components comps)
        {
            foreach (Port_Material p in ports)
                p.AddComponentsToPort(comps);
        }

        public void SetCompositionValue(Port_Material port, string name, double value, SourceEnum source = SourceEnum.Input)
        {
            port.SetComposition(this.guid, name, value, source);
            port.cc.Origin = source;
        }

        public void SetCompositionValue(string port, string name, double value, SourceEnum source = SourceEnum.Input)
        {
            Port_Material p = ports[port];
            SetCompositionValue(p, name, value, source);
        }

        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public virtual Port_Signal Get_DP
        {
            get
            {
                return null;
            }
        }

        public Guid Guid { get => guid; set => guid = value; }
        public PortList Ports { get => ports; set => ports = value; }
        public bool IsInternal { get; set; }
        public bool IsActive { get => isactive; set => isactive = value; }
        public virtual bool IsSolved
        {
            get
            {
                return ports.IsSolved();
                //return issolved;
            }
          /*  set
            {
                issolved=value;
            }*/
        }

        public virtual bool IsDirty
        {
            get
            {
                if (Ports.IsDirty)
                    return true;
                return isDirty;
            }
            set
            {
                isDirty = value;
            }
        }

        public virtual bool IsPropDirtyAndExternal(PropertyEventArgs e)
        {
            if (e.prop.Origin==SourceEnum.UnitOpCalcResult && e.prop.OriginUnitOPGuid == this.guid) // unit op does need re-calcualting
                return false;
            
            if (e.prop is null)
                return true;
            if (e.prop.IsDirty && e.prop.IsFromExternalPort(e.StreamPort))
                return true;

            return false;
        }

        public virtual bool SetAllUOPortsClean() // only works for one connection
        {
            foreach (Port p in ports)
            {
                p.IsDirty = false;
            }
            return true;
        }

        public void CleanUp()
        {
        }

        public void Add(BaseComp bc)
        {
            foreach (var item in ports)
                if (!item.cc.Contains(bc))
                    item.Owner.Add(bc);
        }

        public virtual double[] GetCompositionValues(Port_Material i)
        {
            if (i is null)
                return null;

            return i.cc.MoleFractions;
        }

        public virtual double[] GetCompositionValues(string i)
        {
            return GetCompositionValues(ports[i]);
        }

        public virtual double GetPropValue(Port_Material i, ePropID id)
        {
            if (i.Properties[id] is null)
                return double.NaN;
            return i.Properties[id].Value;
        }

        public virtual double GetPropValue(string pname, ePropID id)
        {
            Port_Material p = ports[pname];
            if (p is null || p.Properties[id] is null)
                return double.NaN;
            return p.Properties[id].Value;
        }

        public PortList GetPorts(FlowDirection fd)
        {
            PortList p = new();

            foreach (var item in ports)
            {
                if ((item.flowdirection == fd || fd == FlowDirection.ALL))
                    p.Add(item);
                else if (item.flowdirection == fd)
                    p.Add(item);
                else if (fd == FlowDirection.ALL)
                    p.Add(item);
            }
            return p;
        }

        public PortList GetConnectedPorts(FlowDirection fd)
        {
            PortList p = new();

            switch (this)
            {
                case StreamMaterial _:
                case Pump _:
                case Valve _:
                case Compressor _:
                case Expander _:
                case Heater _:
                case Cooler _:
                    if (fd == FlowDirection.IN && this.GetPortIn is not null && this.GetPortIn.ConnectedPortNext is not null)
                        p.Add(this.GetPortIn.ConnectedPortNext);
                    else if (this.GetPortOut is not null && this.GetPortOut.ConnectedPortNext is not null)
                        p.Add(this.GetPortOut.ConnectedPortNext);
                    return p;
            }

            foreach (Port port in ports)
            {
                if (port.IsConnected)
                {
                    if (port.flowdirection == fd)
                        p.Add(port.StreamPort);
                    else if (port.flowdirection == fd)
                        p.Add(port.StreamPort);
                    else if (fd == FlowDirection.ALL)
                        p.Add(port.StreamPort);
                }
            }
            return p;
        }

        public virtual object Clone()
        {
            UnitOperation uo = new();
            foreach (var item in Ports)
                uo.ports.Add(item.Clone());

            return uo;
        }

        public virtual UnitOperation CloneDeep(Column col)
        {
            UnitOperation uo = new();
            foreach (var item in Ports)
                uo.ports.Add(item.CloneDeep());

            return uo;
        }

        public virtual UnitOperation CloneSerialise()
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            IFormatter formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            using (var stream = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                formatter.Serialize(stream, this);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                stream.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                return (UnitOperation)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            }
        }

        public void ErasePropertiesFromUO(UnitOperation uo)
        {

        }
    }
}