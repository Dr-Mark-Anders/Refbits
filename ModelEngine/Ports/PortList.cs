using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public partial class PortList : IEnumerable<Port_Material>, ISerializable
    {
        public List<Port_Material> materialList = new();
        public List<Port_Signal> signalList = new();
        public List<Port_Energy> energyList = new();

        public PortList()
        {
            // ports.OnRequestParentobject  += new  Func<object >(delegate { return   this; });
        }

        protected PortList(SerializationInfo info, StreamingContext context)
        {
            materialList = (List<Port_Material>)info.GetValue("ports", typeof(List<Port_Material>));
            energyList = (List<Port_Energy>)info.GetValue("energyports", typeof(List<Port_Energy>));
            signalList = (List<Port_Signal>)info.GetValue("signalports", typeof(List<Port_Signal>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ports", materialList, typeof(List<Port_Material>));
            info.AddValue("energyports", energyList, typeof(List<Port_Energy>));
            info.AddValue("signalports", signalList, typeof(List<Port_Signal>));
        }

        public void SetOwner(UnitOperation uo)
        {
            foreach (var item in materialList)
            {
                item.Owner = uo;
            }
        }

        public bool IsDirty
        {
            get
            {
                foreach (Port p in AllPorts)
                {
                    if (p.IsDirty)
                        return true;
                }
                return false;
            }
        }

        public List<Port> AllPorts
        {
            get
            {
                List<Port> res = new();
                res.AddRange(materialList);
                res.AddRange(energyList);
                res.AddRange(signalList);
                return res;
            }
        }

        public List<Port_Signal> PortsSignal()
        {
            return signalList;
        }

        public Port_Material CombineComponents(int NoComps)
        {
            Port_Material pm = new();
            double totalmolarflow = 0, totalvolflow = 0, totalMassFlow = 0;

            double[] density = new double[NoComps], totalmoles = new double[NoComps];
            double[] MW = new double[NoComps];
            double[] MASSFRAC = new double[NoComps];
            double[] VOLFRAC = new double[NoComps];
            double[] MOLFRAC = new double[NoComps];

            if (materialList.Count == 0)
                return pm;

            density = new double[materialList[0].cc.Count];

            pm.cc.CreateNew(materialList[0].cc);

            foreach (Port_Material p in materialList)
            {
                p.UpdateFlows();
                p.cc.NormaliseFractions(FlowFlag.Molar);
                totalmolarflow += p.MolarFlow;
                totalvolflow += p.VF;
                totalMassFlow += p.MF;
            }

            foreach (Port_Material port in materialList)
            {
                for (int i = 0; i < port.cc.Count; i++)
                {
                    BaseComp bc = port.cc[i];
                    totalmoles[i] += bc.MoleFraction * port.MolarFlow;
                }
            }

            foreach (Port_Material port in materialList)
            {
                for (int i = 0; i < port.cc.Count; i++)
                {
                    BaseComp bc = port.cc[i];
                    MW[i] += bc.MW * bc.MoleFraction * port.MolarFlow / totalmoles[i];
                    MOLFRAC[i] += bc.MoleFraction * port.MolarFlow / totalmolarflow;
                    MASSFRAC[i] += port.MF * bc.MassFraction / totalMassFlow;
                    VOLFRAC[i] += port.VF * bc.STDLiqVolFraction / totalvolflow;
                    density[i] += bc.SG_60F * port.VF / totalvolflow;
                }
            }

            for (int i = 0; i < pm.cc.Count; i++)
            {
                BaseComp bc = pm.cc[i];
                bc.MoleFraction = MOLFRAC[i];
                bc.MassFraction = MASSFRAC[i];
                bc.STDLiqVolFraction = VOLFRAC[i];
                bc.SG_60F = density[i];
            }

            for (int i = 0; i < pm.cc.Count; i++)
            {
                if (!pm.cc[i].IsPure)
                {
                    pm.cc[i].ReEstimateCriticalProps();
                }
            }

            pm.MolarFlow = totalmolarflow;

            return pm;
        }

        /// <summary>
        /// return   ports with compoenenst mathing origin
        /// </summary>
        /// <param name="origin"></param>
        /// <return  s></return  s>
        public PortList GetPorts_UnknownComponents()
        {
            PortList res = new();
            foreach (var p in materialList)
            {
                if (p.cc.Origin == SourceEnum.Empty)
                    res.Add(p);
            }
            return res;
        }

        public PortList GetPorts_knownComponents()
        {
            PortList res = new();
            foreach (var p in materialList)
            {
                if (p.cc.Origin != SourceEnum.Empty)
                    res.Add(p);
            }
            return res;
        }

        public PortList GetPorts_ExternalComponents()
        {
            PortList res = new();
            foreach (var p in materialList)
            {
                switch (p.cc.Origin)
                {
                    case SourceEnum.Input:
                    case SourceEnum.Transferred:
                        res.Add(p);
                        break;
                    case SourceEnum.Empty:
                    case SourceEnum.UnitOpCalcResult:
                        break;
                    default:
                        break;
                }
            }
            return res;
        }
        public PortList GetPorts_InternalComponents()
        {
            PortList res = new();
            foreach (var p in materialList)
            {
                switch (p.cc.Origin)
                {
                    case SourceEnum.Input:
                    case SourceEnum.Transferred:
                        break;
                    case SourceEnum.Empty:
                    case SourceEnum.UnitOpCalcResult:
                        res.Add(p);
                        break;
                    default:
                        break;
                }
            }
            return res;
        }

        public List<string> Names
        {
            get
            {
                List<string> res = new();
                foreach (var item in materialList)
                    res.Add(item.Name);

                return res;
            }
        }

        public int Count
        {
            get
            {
                return materialList.Count;
            }
        }

        public int CountConnected
        {
            get
            {
                int i = 0;
                foreach (var item in materialList)
                {
                    if (item.IsConnected)
                        i++;
                }
                return i;
            }
        }

        public Port_Material GetFirst
        {
            get
            {
                if (materialList.Count > 0)
                    return materialList.First();
                else
                    return null;
            }
        }

        public Port this[Guid guid]
        {
            get
            {
                foreach (var item in materialList)
                {
                    if (item.Guid == guid)
                        return item;
                }
                foreach (var item in energyList)
                {
                    if (item.Guid == guid)
                        return item;
                }
                foreach (var item in signalList)
                {
                    if (item.Guid == guid)
                        return item;
                }
                return null;
            }
        }

        public Port_Material this[string name]
        {
            get
            {
                foreach (var i in materialList)
                {
                    if (i.Name == name)
                        return i;
                }
                return null;
            }
        }

        public Port_Material this[int i]
        {
            get
            {
                return materialList[i];
            }
            set
            {
                materialList[i] = value;
            }
        }

        public void Add(Port p)
        {
            p.PortPropertyChanged += P_MainPortValueChanged;
            switch (p)
            {
                case Port_Material pm:
                    materialList.Add(pm);
                    break;

                case Port_Energy pe:
                    energyList.Add(pe);
                    break;

                case Port_Signal ps:
                    signalList.Add(ps);
                    break;
            }
        }

        private void P_MainPortValueChanged(object sender, Ports.Events.PropertyEventArgs e)
        {
            PortListChanged(sender, e);
        }

        public void ClearAttachedPorts()
        {
            foreach (var item in AllPorts)
                item.StreamPort = null;
        }

        IEnumerator<Port_Material> IEnumerable<Port_Material>.GetEnumerator()
        {
            return materialList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return materialList.GetEnumerator();
        }

        public void AddComponents(Components c)
        {
            foreach (Port_Material item in materialList)
            {
                item.cc.Clear();

                foreach (BaseComp bc in c)
                {
                    item.cc.Add(bc.Clone());
                }
            }
        }

        public Port_Material GetLargestStream()
        {
            Port_Material Largest = materialList.ElementAt(0);
            foreach (var item in materialList)
            {
                if (item.MolarFlow_ > Largest.MolarFlow_)
                    Largest = item;
            }
            return Largest;
        }

        public double TotalMolarFlow()
        {
            double res = 0;
            foreach (var item in materialList)
            {
                res += item.MolarFlow_;
            }
            return res;
        }

        public double TotalVolumnFlow()
        {
            double res = 0;
            foreach (var item in materialList)
            {
                res += item.VF_;
            }
            return res;
        }

        public double TotalMassFlow()
        {
            double res = 0;
            foreach (var item in materialList)
            {
                res += item.MF_;
            }
            return res;
        }

        public bool AllSolved()
        {
            foreach (var item in materialList)
            {
                if (!item.IsSolved)
                    return false;
            }
            return true;
        }

        public void Clear()
        {
            materialList.Clear();
        }

        public PortList PortsOut()
        {
            PortList portList = new();
            foreach (Port item in materialList)
            {
                if (!item.IsFlowIn)
                    portList.Add(item);
            }

            return portList;
        }

        internal Port_Material GetAVGComposition()
        {
            double totmolarflow = 0;
            int compcount = materialList[0].ComponentList.Count();
            Port_Material portavg = new();
            portavg.cc.Add(materialList[0].cc);
            double[] avg = new double[compcount];

            foreach (Port_Material port in materialList)
            {
                totmolarflow += port.MolarFlow_;
                for (int no = 0; no < compcount; no++)
                {
                    avg[no] += port.MolarFlow_ * port.cc[no].MoleFraction;
                }
            }

            for (int i = 0; i < compcount; i++)
            {
                avg[i] /= totmolarflow;
            }

            portavg.cc.SetMolFractions(avg);
            portavg.SetPortValue(Units.ePropID.MOLEF, totmolarflow, SourceEnum.PortCalcResult, null);

            return portavg;
        }

        public void AddRange(PortList portList)
        {
            foreach (Port p in portList)
            {
                this.Add(p);
            }
        }

        public bool IsSolved()
        {
            foreach (Port_Material p in materialList)
            {
                if (!p.IsSolved)
                    return false;
            }
            foreach (Port_Signal p in signalList)
            {
                if (!p.IsSolved)
                    return false;
            }

            foreach (Port_Energy p in energyList)
            {
                if (!p.IsSolved)
                    return false;
            }
            return true;
        }

        public void Clearinternals()
        {
            foreach (Port p in AllPorts)
            {
                p.ClearInternalProperties();
            }
        }

        public void ClearExternals()
        {
            throw new NotImplementedException();
        }
    }
}