using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class CompSplitter : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new("In1", FlowDirection.IN);
        public Port_Material PortOut1 = new("Out1", FlowDirection.OUT);
        public Port_Material PortOut2 = new("Out2", FlowDirection.OUT);
        public Port_Material PortOut3 = new("Out3", FlowDirection.OUT);
        public Port_Material PortOut4 = new("Out4", FlowDirection.OUT);
        public Port_Material PortOut5 = new("Out5", FlowDirection.OUT);

        List<Port_Material> outports = new List<Port_Material>();

        public List<List<Port_Signal>> splits = new();
        public TemperatureC[] CutPoints = new TemperatureC[10];
        private bool UseCutPoints = true;
        private List<double> yields = new();
        private List<double> cutFracs = new();

        public List<double> MolarYields { get => yields; set => yields = value; }
        public List<double> CutFracs { get => cutFracs; set => cutFracs = value; }
        public List<Port_Material> Outports { get => outports; set => outports = value; }

        public CompSplitter() : base()
        {
            Add(PortIn);
            Add(PortOut1);
            Add(PortOut2);
            Add(PortOut3);
            Add(PortOut4);
            Add(PortOut5);
        }

        public CompSplitter(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PortIn = (Port_Material)info.GetValue("In1", typeof(Port_Material));
            PortOut1 = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
            PortOut2 = (Port_Material)info.GetValue("Out2", typeof(Port_Material));
            PortOut3 = (Port_Material)info.GetValue("Out3", typeof(Port_Material));
            PortOut4 = (Port_Material)info.GetValue("Out4", typeof(Port_Material));
            PortOut5 = (Port_Material)info.GetValue("Out5", typeof(Port_Material));

            Add(PortIn);
            Add(PortOut1);
            Add(PortOut2);
            Add(PortOut3);
            Add(PortOut4);
            Add(PortOut5);

            splits = (List<List<Port_Signal>>)info.GetValue("splits", typeof(List<List<Port_Signal>>));
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", PortIn, typeof(Port_Material));
            info.AddValue("Out1", PortOut1, typeof(Port_Material));
            info.AddValue("Out2", PortOut2, typeof(Port_Material));
            info.AddValue("Out3", PortOut3, typeof(Port_Material));
            info.AddValue("Out4", PortOut4, typeof(Port_Material));
            info.AddValue("Out5", PortOut5, typeof(Port_Material));
            info.AddValue("splits", splits);
        }

        public PortList ActiveOutPorts()
        {
            PortList res = new();

            if (PortOut1.IsConnected)
                res.Add(PortOut1);
            if (PortOut2.IsConnected)
                res.Add(PortOut2);
            if (PortOut3.IsConnected)
                res.Add(PortOut3);
            if (PortOut4.IsConnected)
                res.Add(PortOut4);
            if (PortOut5.IsConnected)
                res.Add(PortOut5);

            return res;
        }

        public override bool Solve()
        {
            if (UseCutPoints)
                SolveCuts();
            else
                SolveSplits();

            return true;
        }

        public bool SolveCuts()
        {
            Port_Material port = PortIn;          

            for (int i = 0; i < CutPoints.Length-1; i++)
            {
                Port_Material p = port.Cut(CutPoints[i], CutPoints[i+1], out double cutfrac);
                cutFracs.Add(cutfrac);
                MolarYields.Add(p.MolarFlow_);
                outports.Add(p);
            }

            return true;
        }

        public bool SolveSplits()
        {
            double Moles = 0;
            int PortNo = 0;
            if (!PortIn.cc.MoleFractionsValid)
                return false;

            if (PortIn.cc.Count < splits[0].Count)
            {
                splits.Clear();
            }

            Balance.Calculate(this);
            while (FlashAllPorts() > 0)
                Balance.Calculate(this);

            PortList outports = ActiveOutPorts();

            foreach (var outport in outports)
            {
                outport.cc = PortIn.cc.Clone();
                outport.cc.ClearMoleFractions();
            }

            int PortOutNo = -1;
            foreach (var outport in outports)
            {
                PortOutNo++;
                for (int i = 0; i < PortIn.ComponentList.Count; i++)
                {
                    double split = splits[PortNo][i].Value.BaseValue;
                    Moles += PortIn.cc[i].MoleFraction * split * PortIn.MolarFlow_;
                    outport.cc[i].TempMolarFlow = PortIn.cc[i].MoleFraction * split * PortIn.MolarFlow_;
                }

                for (int i = 0; i < PortIn.ComponentList.Count; i++)
                {
                    if (Moles == 0)
                        outport.cc[i].MoleFraction = PortIn.cc[i].MoleFraction;
                    else
                        outport.cc[i].MoleFraction = outport.cc[i].TempMolarFlow / Moles;
                }

                outport.SetPortValue(ePropID.MOLEF, Moles, SourceEnum.UnitOpCalcResult, this);
                outport.cc.Origin = SourceEnum.UnitOpCalcResult;
                outport.cc.NormaliseFractions();
                outport.SetPortValue(ePropID.T, PortIn.T_, SourceEnum.Transferred, this);
                outport.SetPortValue(ePropID.P, PortIn.P_, SourceEnum.Transferred, this);
                Moles = 0;
                PortNo++;
            }

            FlashAllPorts();

            return true;
        }

        public void CalcMissingSplits(int NoStreams, int NoComps)
        {
            double sum = 0;
            int countempty = 0;
            Port_Signal empty = null;

            for (int c = 0; c < NoComps; c++)
            {
                for (int p = 0; p < NoStreams; p++)
                {
                    if (splits[p][c].Value.origin == SourceEnum.UnitOpCalcResult)
                        splits[p][c].Value.Clear();

                    if (splits[p][c].Value.origin == SourceEnum.Empty)
                    {
                        empty = splits[p][c];
                        countempty++;
                    }
                    else
                        sum += splits[p][c].Value;
                }
                if (countempty == 1 && empty != null)
                {
                    empty.Clear();
                    empty.SetPortValue(ePropID.NullUnits, 1 - sum, SourceEnum.UnitOpCalcResult);
                }
                sum = 0;
                countempty = 0;
                empty = null;
            }
        }
    }
}