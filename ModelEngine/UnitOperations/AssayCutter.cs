using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class AssayCutter : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new("In1", FlowDirection.IN);
        public Port_Material PortOut1 = new("Out1", FlowDirection.OUT);
        public Port_Material PortOut2 = new("Out2", FlowDirection.OUT);
        public Port_Material PortOut3 = new("Out3", FlowDirection.OUT);
        public Port_Material PortOut4 = new("Out4", FlowDirection.OUT);
        public Port_Material PortOut5 = new("Out5", FlowDirection.OUT);
        public Port_Material PortOut6 = new("Out6", FlowDirection.OUT);
        public Port_Material PortOut7 = new("Out7", FlowDirection.OUT);
        public Port_Material PortOut8 = new("Out8", FlowDirection.OUT);
        public Port_Material PortOut9 = new("Out9", FlowDirection.OUT);
        public Port_Material PortOut10 = new("Out10", FlowDirection.OUT);

        private List<Tuple<Temperature, Temperature>> cutPoints = new();

        public AssayCutter() : base()
        {
            Add(PortIn);
            Add(PortOut1);
            Add(PortOut2);
            Add(PortOut3);
            Add(PortOut4);
            Add(PortOut5);
            Add(PortOut6);
            Add(PortOut7);
            Add(PortOut8);
            Add(PortOut9);
            Add(PortOut10);
        }

        public AssayCutter(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                PortIn = (Port_Material)info.GetValue("In1", typeof(Port_Material));
                PortOut1 = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
                PortOut2 = (Port_Material)info.GetValue("Out2", typeof(Port_Material));
                PortOut3 = (Port_Material)info.GetValue("Out3", typeof(Port_Material));
                PortOut4 = (Port_Material)info.GetValue("Out4", typeof(Port_Material));
                PortOut5 = (Port_Material)info.GetValue("Out5", typeof(Port_Material));
                PortOut6 = (Port_Material)info.GetValue("Out6", typeof(Port_Material));
                PortOut7 = (Port_Material)info.GetValue("Out7", typeof(Port_Material));
                PortOut8 = (Port_Material)info.GetValue("Out8", typeof(Port_Material));
                PortOut9 = (Port_Material)info.GetValue("Out9", typeof(Port_Material));
                PortOut10 = (Port_Material)info.GetValue("Out10", typeof(Port_Material));
                cutPoints = (List<Tuple<Temperature, Temperature>>)info.GetValue("ICPList1", typeof(List<Tuple<Temperature, Temperature>>));
            }
            catch { }

            Add(PortIn);
            Add(PortOut1);
            Add(PortOut2);
            Add(PortOut3);
            Add(PortOut4);
            Add(PortOut5);
            Add(PortOut6);
            Add(PortOut7);
            Add(PortOut8);
            Add(PortOut9);
            Add(PortOut10);

            if (cutPoints.Count == 0)
            {
                cutPoints.Add(new Tuple<Temperature, Temperature>(0, 36 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(36 + 273.15, 150 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(150 + 273.15, 220 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(220 + 273.15, 250 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(250 + 273.15, 350 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(350 + 273.15, 360 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(360 + 273.15, 400 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(400 + 273.15, 450 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(450 + 273.15, 550 + 273.15));
                cutPoints.Add(new Tuple<Temperature, Temperature>(550 + 273.15, 850 + 273.15));
            }
        }

        public List<Port_Material> StreamList
        {
            get
            {
                List<Port_Material> res = new();
                res.Add(PortOut1);
                res.Add(PortOut2);
                res.Add(PortOut3);
                res.Add(PortOut4);
                res.Add(PortOut5);
                res.Add(PortOut6);
                res.Add(PortOut7);
                res.Add(PortOut8);
                res.Add(PortOut9);
                res.Add(PortOut10);
                return res;
            }
        }

        public List<Tuple<Temperature, Temperature>> CutPoints { get => cutPoints; set => cutPoints = value; }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", PortIn, typeof(Port_Material));
            info.AddValue("Out1", PortOut1, typeof(Port_Material));
            info.AddValue("Out2", PortOut2, typeof(Port_Material));
            info.AddValue("Out3", PortOut3, typeof(Port_Material));
            info.AddValue("Out4", PortOut4, typeof(Port_Material));
            info.AddValue("Out5", PortOut5, typeof(Port_Material));
            info.AddValue("Out6", PortOut6, typeof(Port_Material));
            info.AddValue("Out7", PortOut7, typeof(Port_Material));
            info.AddValue("Out8", PortOut8, typeof(Port_Material));
            info.AddValue("Out9", PortOut9, typeof(Port_Material));
            info.AddValue("Out10", PortOut10, typeof(Port_Material));

            info.AddValue("ICPList1", cutPoints, typeof(List<Tuple<Temperature, Temperature>>));
        }

        public override bool Solve()
        {
            //Balance.Calculate(this);
            while (FlashAllPorts() > 0)
            {
                //Balance.Calculate(this);
            }

            CrudeCutter cutter = new();

            if (PortIn.IsSolved)
            {
                List<Port_Material> streams = cutter.CutStreams(PortIn, cutPoints, Guid.Empty);
                PortList OutPorts = this.GetPorts(FlowDirection.OUT);

                OutPorts[0].cc = new();
                OutPorts[0].Properties = new();
                OutPorts[0].cc.Origin = SourceEnum.Input;
                OutPorts[0].Flash();

                for (int i = 0; i < streams.Count; i++)
                {
                    OutPorts[i].cc = streams[i].cc;
                    OutPorts[i].Properties = streams[i].Properties;
                    OutPorts[i].cc.Origin = SourceEnum.Input;
                    OutPorts[i].Flash();
                }
            }

            return true;
        }
    }
}