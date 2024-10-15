using ModelEngine;
using System;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Assay : UnitOperation, ISerializable
    {
        public Port_Material PortOut = new("Out1", FlowDirection.OUT);
        private string assayname = "TestAssay";
        private GenericAssayForm assayform;
        private readonly DataArrays data = new();
        private readonly AssayPropertyCollection apc = new();
        private readonly Components crude = new();

        public Assay() : base()
        {
            Add(PortOut);
            assayform = new GenericAssayForm(PortOut, data, apc, crude, assayname);
        }

        public Assay(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PortOut = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
            Add(PortOut);
            assayname = info.GetString("assayname");
            try
            {
                data = (DataArrays)info.GetValue("Data", typeof(DataArrays));
                apc = (AssayPropertyCollection)info.GetValue("apc", typeof(AssayPropertyCollection));
                crude = (Components)info.GetValue("crude", typeof(Components));
            }
            catch
            {
            }
            assayform = new GenericAssayForm(PortOut, data, apc, crude, assayname);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Out1", PortOut);
            info.AddValue("assayname", assayname);
            info.AddValue("Data", data);
            info.AddValue("apc", apc);
            info.AddValue("crude", crude);
        }

        public void ShowAssayForm()
        {
            assayform = new(PortOut, data, apc, crude, assayname);
            assayform.ShowDialog();
            assayname = assayform.AssayName;

            for (int i = 0; i < assayform.Crude.Count; i++)
            {
                BaseComp bc = assayform.Crude[i];
                if (PortOut.cc.Contains(bc))
                {
                    int Index = PortOut.cc.IndexOf(bc);
                    PortOut.cc[Index].MoleFraction = bc.MoleFraction;
                    PortOut.cc[Index].SG_60F = bc.SG_60F;
                    PortOut.cc[Index].CritT = bc.CritT;
                    PortOut.cc[Index].CritP = bc.CritP;
                    PortOut.cc[Index].CritV = bc.CritV;
                    PortOut.cc[Index].Omega = bc.Omega;
                    PortOut.cc[Index].CritZ = bc.CritZ;
                    PortOut.cc[Index].Properties = new(assayform.Crude.ComponentList[i].Properties);
                }
            }

            PortOut.RemoveNanComponents();
            PortOut.cc.Origin = SourceEnum.Input;
        }

        public override bool Solve()
        {
            PortOut.Flash();
            IsDirty = false;
            //IsSolved = true;
            return true;
        }
    }
}