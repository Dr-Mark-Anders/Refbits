using System;
using System.Collections.Generic;
using Units;

namespace ModelEngine
{
    public partial class Port
    {
        public void ClearConnections()
        {
            streamport = null;
        }

        public void ClearPortCalcValues()
        {
            switch (this)
            {
                case Port_Material pm:
                    foreach (StreamProperty item in pm.Properties.Props.Values)
                    {
                        if(item is not null)
                            switch (item.origin)
                            {
                                case SourceEnum.PortCalcResult:
                                    item.Clear();
                                    break;
                            }
                    }
                    break;
            }
        }

        public void ClearUPCalcValues()
        {
            switch (this)
            {
                case Port_Material pm:
                    foreach (StreamProperty item in pm.Properties.Props.Values)
                    {
                        switch (item.origin)
                        {
                            case SourceEnum.UnitOpCalcResult:
                                item.Clear();
                                break;
                        }
                    }
                    break;
            }
        }

        public void ClearInternalProperties()
        {
            switch (this)
            {
                case Port_Material pm:
                    foreach (StreamProperty prop in pm.Properties.Props.Values)
                    {
                        if (prop is not null && prop.OriginPortGuid == Guid)
                            prop.Clear();
                    }

                    if (pm.cc.OriginPortGuid == Guid)
                        pm.cc.Clear();

                    if (pm.ConnectedPortNext is not null)
                        ((Port_Material)pm.ConnectedPortNext).IsFlashed = false;
                    else
                        ((Port_Material)pm).IsFlashed = false;
                    break;

                case Port_Energy pe:
                    if (pe.Value.OriginPortGuid == Guid)
                        pe.Clear();
                    break;

                case Port_Signal ps:
                    if (ps.Value.OriginPortGuid == Guid)
                        ps.Clear();
                    break;
            }
        }

        public void Clear()
        {
            switch (this)
            {
                case Port_Signal ps:
                    ps.Value.Clear();
                    break;

                case Port_Energy pe:
                    pe.Value.Clear();
                    break;

                case Port_Material pm:
                    foreach (KeyValuePair<ePropID, StreamProperty> item in pm.Properties)
                        item.Value.Clear();

                    pm.IsFlashed = false;

                    //foreach (BaseComp item in pm.cc)
                    //    item.Erase();

                    //pm.cc.Origin = SourceEnum.Empty;
                    // pm.cc.OriginPortGuid = Guid.Empty;
                    break;
            }
        }


        public void ClearComposition()
        {
            switch (this)
            {
                case Port_Material pm:
                    pm.IsFlashed = false;

                    foreach (BaseComp item in pm.cc)
                        item.Erase();

                    pm.cc.Origin = SourceEnum.Empty;
                    pm.cc.OriginPortGuid = Guid.Empty;
                    break;
            }
        }

        public void ClearNonInputs()
        {
            switch (this)
            {
                case Port_Signal ps:
                    if (!ps.Value.IsInput)
                        ps.Value.Clear();
                    break;

                case Port_Energy pe:
                    if (!pe.Value.IsInput)
                        pe.Value.Clear();
                    break;

                case Port_Material pm:
                    foreach (KeyValuePair<ePropID, StreamProperty> item in pm.Properties)
                        if (item.Value is not null && !item.Value.IsInput)
                        {
                            item.Value.Clear();
                            pm.IsFlashed = false;
                        }
                    if (!pm.cc.IsInput)
                    {
                        pm.IsFlashed = false;
                        foreach (BaseComp item in pm.cc)
                            item.Erase();

                        pm.cc.Origin = SourceEnum.Empty;
                        pm.cc.OriginPortGuid = Guid.Empty;
                    }
                    break;
            }
        }

        public void ClearCalcEstimates()
        {
            switch (this)
            {
                case Port_Signal ps:
                    if (ps.Value.IsCalcEstimate())
                        ps.Value.Clear();
                    break;

                case Port_Energy pe:
                    if (pe.Value.IsCalcEstimate())
                        pe.Value.Clear();
                    break;

                case Port_Material pm:
                    foreach (KeyValuePair<ePropID, StreamProperty> item in pm.Properties)
                    {
                        if (item.Value.IsEstimate())
                            item.Value.Clear();
                    }
                    pm.IsFlashed = false;

                    switch (pm.cc.Origin)
                    {
                        case SourceEnum.Input:
                        case SourceEnum.FixedEstimate:
                        case SourceEnum.Transferred:
                            break;

                        case SourceEnum.UnitOpCalcResult:
                            break;

                        case SourceEnum.CalcEstimate:
                        default:
                            foreach (BaseComp item in pm.cc)
                                item.Erase();

                            pm.cc.Origin = SourceEnum.Empty;
                            pm.cc.OriginPortGuid = Guid.Empty;
                            break;
                    }
                    break;
            }
        }

        public void ClearEstimates()
        {
            switch (this)
            {
                case Port_Signal ps:
                    if (ps.Value.IsEstimate())
                        ps.Value.Clear();
                    break;

                case Port_Energy pe:
                    if (pe.Value.IsEstimate())
                        pe.Value.Clear();
                    break;

                case Port_Material pm:
                    foreach (KeyValuePair<ePropID, StreamProperty> item in pm.Properties)
                    {
                        if (item.Value is not null && item.Value.IsEstimate())
                            item.Value.Clear();
                    }
                    pm.IsFlashed = false;

                    switch (pm.cc.Origin)
                    {
                        case SourceEnum.Input:
                        case SourceEnum.FixedEstimate:
                        case SourceEnum.Transferred:
                            break;

                        case SourceEnum.UnitOpCalcResult:
                        case SourceEnum.CalcEstimate:
                        default:
                            foreach (BaseComp item in pm.cc)
                                item.Erase();

                            pm.cc.Origin = SourceEnum.Empty;
                            pm.cc.OriginPortGuid = Guid.Empty;
                            break;
                    }
                    break;
            }
        }

        public void ClearCalcValues(SourceEnum origin)
        {
            switch (this)
            {
                case Port_Signal ps:
                    if (ps.Value.Source == origin)
                        ps.Value.Clear();
                    break;

                case Port_Energy pe:
                    if (pe.Value.Source == origin)
                        pe.Value.Clear();
                    break;

                case Port_Material pm:
                    foreach (KeyValuePair<ePropID, StreamProperty> item in pm.Properties)
                    {
                        if (item.Value is not null && item.Value.Source == origin)
                            switch (item.Key)
                            {
                                case ePropID.MOLEF: // if a flow delete all calc flows.
                                case ePropID.MF:
                                case ePropID.VF:
                                    pm.Properties[ePropID.MOLEF].Clear();
                                    pm.Properties[ePropID.VF].Clear();
                                    pm.Properties[ePropID.MF].Clear();
                                    break;
                                default:
                                    item.Value.Clear();
                                    break;
                            }
                            
                    }
                    pm.IsFlashed = false;

                    if (pm.cc.Origin == origin)
                    {
                        foreach (BaseComp item in pm.cc)
                            item.Erase();

                        pm.cc.Origin = SourceEnum.Empty;
                        pm.cc.OriginPortGuid = Guid.Empty;
                    }
                    break;
            }
        }

        public void ClearProps()
        {
            switch (this)
            {
                case Port_Signal ps:
                    if (ps.Value.IsErasable())
                        ps.Value.Clear();
                    break;

                case Port_Energy pe:
                    if (pe.Value.IsErasable())
                        pe.Value.Clear();
                    break;

                case Port_Material pm:
                    foreach (KeyValuePair<ePropID, StreamProperty> item in pm.Properties)
                    {
                        if (item.Value.IsErasable())
                            item.Value.Clear();
                    }
                    pm.IsFlashed = false;
                    break;
            }
        }

        public virtual void ClearIfNotExternallyFlashable()
        {
        }
    }
}