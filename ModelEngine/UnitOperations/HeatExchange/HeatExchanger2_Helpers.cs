using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Units;

namespace ModelEngine
{
    public partial class HeatExchanger2
    {
        private void SolveForQandUA(double Q, double LMTD, double UA)
        {
            //case  for all T known. Put values in UA and Q. This will raise consistency errors if needed
            if (LMTD.HasValue())
            {
                if (Q.HasValue())
                {
                    UATemp = Q / LMTD;
                }
                else if (UA.HasValue())
                {
                    //QTemp = UA * LMTD;
                }
                return;
            }
        }

        // Calculates LMTD
        public virtual double CalcLMTD(double S1T1, double S2T1, double S1T2, double S2T2)
        {
            var dt1 = S1T1 - S2T1;
            var dt2 = S1T2 - S2T2 + 1E-30;
            if (dt1 * dt2 < 0.0)
            {
                //set the smaller dt to 1e-30
                if (Math.Abs(dt1) > Math.Abs(dt2))//dt2 is smaller
                    dt2 = dt1 / Math.Abs(dt1) * 1E-30;
                else
                    dt1 = dt2 / Math.Abs(dt2) * 1E-30;
            }
            else if (dt1 * dt2 == 0.0)
            {
                dt1 = 1E-30;
                dt2 = dt1;
            }
            if (Math.Abs(dt1 - dt2) < 1E-10)
                dt1 += 1E-10;

            return (dt1 - dt2) / (Math.Log(dt1 / dt2) + 1E-30);
        }

        public void calcApproachTs()
        {
            deltaTApproach.SetPortValue(ePropID.DeltaT, cooler.PortIn.T_ - heater.PortOut.T_, SourceEnum.UnitOpCalcResult);
            deltaTShellSide.SetPortValue(ePropID.DeltaT, cooler.PortOut.T_ - cooler.PortIn.T_, SourceEnum.UnitOpCalcResult);
            deltaTTubeSide.SetPortValue(ePropID.DeltaT, heater.PortOut.T_ - heater.PortIn.T_, SourceEnum.UnitOpCalcResult);
        }

        public double MinApproach()
        {
            List<double> list = new() { deltaTApproach.Value, deltaTApproach.Value };
            return list.Min();
        }

        public class SideInfo
        {
            public double T, P, H, F;
            public bool TIsIn;
            public double[] fracs;
            public string Prov { get; internal set; }
            public string Case { get; internal set; }

            public SideInfo(double T, double P, double H, double F, double[] Fracs)
            {
                this.T = T;
                this.P = P;
                this.H = H;
                this.F = F;
                this.fracs = Fracs;
            }
        }

        [Serializable]
        public class ExcSpec
        {
            public string Name;
            public Port port;
            public Guid guid;
            public SpecType type;

            public bool IsActive
            {
                get
                {
                    switch (this.port)
                    {
                        case Port_Signal ps:
                            return ps.Value.IsInput || ps.Value.IsCalcSpec;

                        case Port_Energy pe:
                            return pe.Value.IsInput || pe.Value.IsCalcSpec;
                    }
                    return false;
                }
            }

            public double Error
            {
                get
                {
                    switch (this.port)
                    {
                        case Port_Signal ps:
                            return ps.Value.BaseValue - ps.Value.estimate;

                        case Port_Energy pe:
                            return pe.Value.BaseValue - pe.Value.estimate;
                    }
                    return double.NaN;
                }
            }

            public StreamProperty value
            {
                get
                {
                    switch (this.port)
                    {
                        case Port_Signal ps:
                            return ps.Value;

                        case Port_Energy pe:
                            return pe.Value;
                    }
                    return null;
                }
            }

            public ExcSpec(Port port, string Name, SpecType type)
            {
                this.Name = Name;
                this.port = port;
                switch (this.port)
                {
                    case Port_Signal ps:
                        ps.Value.DisplayName = Name;
                        break;

                    case Port_Energy pe:
                        pe.Value.DisplayName = Name;
                        break;
                }

                this.Name = Name;
                guid = Guid.NewGuid();
                this.type = type;
            }

            public void SetProperty(double value)
            {
                switch (this.port)
                {
                    case Port_Signal ps:
                        ps.Value.BaseValue = value;
                        break;

                    case Port_Energy pe:
                        pe.Value.BaseValue = value;
                        break;
                }
            }

            public void Clear()
            {
                port.Clear();
            }

            public override string ToString()
            {
                return Name;
            }
        }

        [Serializable]
        public class SpecClass : IList<ExcSpec>
        {
            public List<ExcSpec> Specs = new();

            public ExcSpec Spec(SpecType type)
            {
                foreach (var item in Specs)
                {
                    if (item.type == type)
                        return item;
                }
                return null;
            }

            public ExcSpec this[SpecType type]
            {
                get
                {
                    foreach (var item in Specs)
                    {
                        if (item.type == type)
                            return item;
                    }
                    return null;
                }
            }

            public ExcSpec this[int index] { get => Specs[index]; set => Specs[index] = value; }

            public int Count => Specs.Count;

            public bool IsReadOnly => ((ICollection<ExcSpec>)Specs).IsReadOnly;

            public SpecType GetSolveType()
            {
                if (CountActive() > 1)
                    return SpecType.OverSpecified;
                if (CountActive() == 0)
                    return SpecType.UnderSpecified;

                return GetActive()[0].type;
            }

            public List<ExcSpec> GetActive()
            {
                List<ExcSpec> res = new();
                foreach (var item in Specs)
                {
                    if (item.IsActive)
                        res.Add(item);
                }
                return res;
            }

            public int CountActive()
            {
                int count = 0;
                foreach (var item in Specs)
                {
                    if (item.IsActive)
                        count++;
                }
                return count;
            }

            public void Add(ExcSpec item)
            {
                Specs.Add(item);
            }

            public void Clear()
            {
                Specs.Clear();
            }

            public bool Contains(SpecType spec)
            {
                foreach (ExcSpec item in Specs)
                {
                    if (item.type == spec)
                        return true;
                }

                return false;
            }

            public bool Contains(ExcSpec item)
            {
                return Specs.Contains(item);
            }

            public int IndexOf(ExcSpec item)
            {
                throw new NotImplementedException();
            }

            internal void Add(Port_Signal port, SpecType spectype)
            {
                Add(new ExcSpec(port, port.Value.DisplayName, spectype));
            }

            internal void Add(Port_Energy port, SpecType spectype)
            {
                Add(new ExcSpec(port, port.Value.DisplayName, spectype));
            }

            public void Insert(int index, ExcSpec item)
            {
                throw new NotImplementedException();
            }

            public bool Remove(ExcSpec item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(ExcSpec[] array, int arrayIndex)
            {
                ((ICollection<ExcSpec>)Specs).CopyTo(array, arrayIndex);
            }

            public IEnumerator<ExcSpec> GetEnumerator()
            {
                return ((IEnumerable<ExcSpec>)Specs).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)Specs).GetEnumerator();
            }

            internal void EraseCalcValues()
            {
                foreach (ExcSpec spec in Specs)
                {
                    if (!spec.value.IsInput)
                        spec.Clear();
                };
            }
        }

        public enum enumConections { all, tubeside, shellside, In, Out, none }

        public enumConections Connections()
        {
            if (tubePortIn.IsConnected && tubePortOut.IsConnected && shellPortIn.IsConnected && shellPortOut.IsConnected)
                return enumConections.all;

            if (tubePortIn.IsConnected && tubePortOut.IsConnected)
                return enumConections.tubeside;

            if (shellPortIn.IsConnected && shellPortOut.IsConnected)
                return enumConections.shellside;

            if (shellPortIn.IsConnected && tubePortIn.IsConnected)
                    return enumConections.In;

            if (shellPortOut.IsConnected && tubePortOut.IsConnected)
                return enumConections.Out;

            return enumConections.none;
        }

        public enum HotSide { shell, tube, TubeIn, TubeOut, ShellIn, ShellOut, Unknown }

        public HotSide IdentifyHotSide()
        {
            switch(Connections())
            {
                case enumConections.all:
                    if (tubePortIn.T_.IsKnown && shellPortIn.T_.IsKnown)
                    {
                        if (tubePortIn.T_ > shellPortIn.T_) // tube is hot side
                            return HotSide.tube;
                        else
                            return HotSide.shell;
                        
                    }
                    else if (shellPortOut.T.IsKnown && tubePortOut.T.IsKnown)
                    {
                        if (shellPortOut.T > tubePortOut.T)
                            return HotSide.shell;
                        else
                            return HotSide.tube;

                    }
                    break;
                case enumConections.tubeside:
                    if (tubePortOut.T > tubePortIn.T || deltaTTubeSide.Value > 0)
                        return HotSide.TubeOut;
                    if (tubePortOut.T < tubePortIn.T || deltaTTubeSide.Value < 0)
                        return HotSide.TubeIn;
                    break;
                case enumConections.shellside:
                    if (shellPortOut.T > shellPortIn.T || deltaTShellSide.Value > 0)
                        return HotSide.ShellOut;
                    if (shellPortOut.T < shellPortIn.T || deltaTShellSide.Value < 0)
                        return HotSide.ShellIn;
                    break;
            }
            
            return HotSide.Unknown;
        }

      /*  enum HeaterCooler { Heater, Cooler}
        public HeaterCooler HeaterOrCooler()
        {
            switch (Connections())
            {
                case enumConections.all:
                    break;
            }
        }*/
    }
}