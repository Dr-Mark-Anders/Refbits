using Extensions;
using ModelEngine.Ports.Events;
using System;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class SimpleFlash : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new("In", FlowDirection.IN);
        public Port_Material PortOutV = new("OutV", FlowDirection.OUT);
        public Port_Material PortOutL = new("OutL", FlowDirection.OUT);

        public SimpleFlash() : base()
        {
            Add(PortIn);
            Add(PortOutV);
            Add(PortOutL);
        }

        public SimpleFlash(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                PortIn = (Port_Material)info.GetValue("PortIn", typeof(Port_Material));
                PortOutV = (Port_Material)info.GetValue("PortOutV", typeof(Port_Material));
                PortOutL = (Port_Material)info.GetValue("PortOutL", typeof(Port_Material));
            }
            catch { }

            Add(PortIn);
            Add(PortOutV);
            Add(PortOutL);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("PortIn", PortIn);
            info.AddValue("PortOutV", PortOutV);
            info.AddValue("PortOutL", PortOutL);
        }

        public override void PortValueChanged(object sender, PropertyEventArgs e)
        {
            if (IsPropDirtyAndExternal(e))
            {
                RaiseUOChangedEvent(e.StreamPort);
                //Debug.Print ("PropertyChanged: " + this.Name + " " + e.port.Name
                //    + " " + e.prop.Name + " IsDirty:" + e.prop.Value.ToString());
            }
        }

        public override bool Solve()
        {
            PortIn.Flash();

            PortOutV.cc = PortIn.cc.Clone();
            PortOutL.cc = PortIn.cc.Clone();
            PortOutV.cc.Origin = SourceEnum.UnitOpCalcResult;
            PortOutL.cc.Origin = SourceEnum.UnitOpCalcResult;


            double[] VapFlows = new double[PortIn.ComponentList.Count], LiqFlows = new double[PortIn.ComponentList.Count];

            if (PortIn.IsFullyDefined)
            {
                for (int i = 0; i < PortIn.ComponentList.Count; i++)
                {
                    VapFlows[i] = PortIn.ComponentList[i].MoleFracVap * PortIn.ComponentList[i].MoleFraction;
                    LiqFlows[i] = (1 - PortIn.ComponentList[i].MoleFracVap) * PortIn.ComponentList[i].MoleFraction;
                }

                VapFlows = VapFlows.Normalise();
                LiqFlows = LiqFlows.Normalise();

                if (PortIn.Q == 0)
                    VapFlows[0] = 1;

                PortIn.cc.NormaliseFractions(FlowFlag.Molar);

                PortOutV.SetMoleFractions(VapFlows);
                PortOutL.SetMoleFractions(LiqFlows);

                PortOutV.SetPortValue(ePropID.MOLEF, PortIn.Q_ * PortIn.MolarFlow_, SourceEnum.UnitOpCalcResult, this);
                PortOutL.SetPortValue(ePropID.MOLEF, (1 - PortIn.Q_) * PortIn.MolarFlow_, SourceEnum.UnitOpCalcResult, this);

                PortOutV.SetPortValue(ePropID.T, PortIn.T_, SourceEnum.UnitOpCalcResult);
                PortOutL.SetPortValue(ePropID.T, PortIn.T_, SourceEnum.UnitOpCalcResult);

                PortOutV.SetPortValue(ePropID.P, PortIn.P_, SourceEnum.UnitOpCalcResult);
                PortOutL.SetPortValue(ePropID.P, PortIn.P_, SourceEnum.UnitOpCalcResult);

                PortOutV.SetPortValue(ePropID.Q, 1, SourceEnum.UnitOpCalcResult);
                PortOutL.SetPortValue(ePropID.Q, 0, SourceEnum.UnitOpCalcResult);

                PortOutV.Flash();
                PortOutL.Flash();
            }

            return true;
        }
    }
}