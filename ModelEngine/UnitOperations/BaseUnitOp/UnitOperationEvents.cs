using ModelEngine.Ports.Events;
using System;
using System.Diagnostics;
using Units;

namespace ModelEngine
{
    public partial class UnitOperation
    {
        public Func<UnitOperation> OnRequestParent;

        public event UOEventHandler UOChanged;

        public delegate void UOEventHandler(IUnitOperation sender, UOChangedEventArgs e);

        public virtual void RaiseUOChangedEvent(Port p)
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            UOChanged?.Invoke(this, new UOChangedEventArgs(p));
        }

        public virtual void PortValueChanged(object sender, PropertyEventArgs e)
        {
            switch (this)
            {
                case StreamMaterial:
                case StreamEnergy:
                case StreamSignal:
                    break;

                default:
                    if (e is null)
                        return;

                    if (e.StreamPort is null)
                    {
                        return;
                    }
                    else if (IsPropDirtyAndExternal(e))
                    {
                        RaiseUOChangedEvent(e.StreamPort); // send port that is streamport, so both UOs get fired, only fire even if property chnages and is external to the port

                        Debug.Print("PropertyChanged: " + this.Name + " " + e.StreamPort.Name
                            + " " + e.prop.Name + " IsDirty:" + e.prop.Value.ToString());
                    }
                    break;
            }
        }

        public virtual void PortCompositionChanged(object sender, CompositionEventArgs e)
        {
            switch (this)
            {
                case StreamMaterial:
                case StreamEnergy:
                case StreamSignal:
                    break;

                default:
                    if (e is null)
                        return;

                    if (e.StreamPort is null)
                    {
                        return;
                    }
                    else
                    {
                        RaiseUOChangedEvent(e.StreamPort); // send port that is streamport, so both UOs get fired, only fire even if property chnages and is external to the port

                        Debug.Print("Composition Changed: " + this.Name + " " + e.StreamPort.Name);
                    }
                    break;
            }
        }

        internal void EraseValuesCalcFromThisUO()
        {
            foreach (Port_Material port in ports.materialList)
            {
                foreach (StreamProperty prop in port.Props.Values)
                {
                    if (prop.OriginUnitOPGuid == this.guid && prop.origin == SourceEnum.UnitOpCalcResult)
                    {
                        switch (prop.Propid)
                        {
                            case ePropID.MF:
                            case ePropID.VF:
                            case ePropID.MOLEF:
                                port.Props[ePropID.MF].Clear();
                                port.Props[ePropID.VF].Clear();
                                port.Props[ePropID.MOLEF].Clear();
                                break;

                            default:
                                prop.Clear();
                                break;
                        }
                    }
                }
            }

            foreach (Port_Signal port in ports.signalList)
            {
                if (port.Value.OriginUnitOPGuid == this.guid && port.Value.origin == SourceEnum.UnitOpCalcResult)
                {
                    port.Value.Clear();
                    break;
                }
            }
        }
    }
}