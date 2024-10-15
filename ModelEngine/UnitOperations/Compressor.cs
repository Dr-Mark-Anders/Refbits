using ModelEngine;
using Steam;
using System;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Compressor : UnitOperation, ISerializable
    {
        public const double Gravity = 9.80665;
        public Port_Material PortIn = new("In1", FlowDirection.ALL);
        public Port_Material PortOut = new("Out1", FlowDirection.ALL);
        public Port_Signal DT = new("DeltaT", ePropID.DeltaT, FlowDirection.IN);
        public Port_Energy Q = new("Q", FlowDirection.OUT);
        public Port_Signal AEff = new("AdiabaticEfficiency", ePropID.NullUnits, FlowDirection.OUT, 70);
        public Port_Signal PEff = new("PolytropicEfficiency", ePropID.NullUnits, FlowDirection.OUT, 70);

        public Port_Signal AdiabaticHead = new("AdiabaticHead", ePropID.Length, FlowDirection.OUT);
        public Port_Signal PolytropicHead = new("PolytropicHead", ePropID.Length, FlowDirection.OUT);
        public Port_Signal AdiabaticFluidHead = new("AdiabaticFluidHead", ePropID.SpecificEnergy, FlowDirection.OUT);
        public Port_Signal PolytropicFluidHead = new("PolytropicFluidHead", ePropID.SpecificEnergy, FlowDirection.OUT);
        public Port_Signal PowerConsumed = new("PowerConsumed", ePropID.EnergyFlow, FlowDirection.OUT);
        public Port_Signal PolytropicHeadFactor = new("PolytropicHeadFactor", ePropID.NullUnits, FlowDirection.OUT);
        public Port_Signal PolytropicExponent = new("PolytropicExponent", ePropID.NullUnits, FlowDirection.OUT);
        public Port_Signal IsentropicExponent = new("IsentropicExponent", ePropID.NullUnits, FlowDirection.OUT);
        public Port_Signal Speed = new("Speed", ePropID.NullUnits, FlowDirection.OUT);

        public Compressor() : base()
        {
            Add(AEff);
            Add(PEff);
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(Q);
            Add(AdiabaticHead);
            Add(PolytropicHead);
            Add(AdiabaticFluidHead);
            Add(PolytropicFluidHead);
            Add(PowerConsumed);
            Add(PolytropicHeadFactor);
            Add(PolytropicExponent);
            Add(IsentropicExponent);
            Add(Speed);
        }

        public Compressor(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PortIn = (Port_Material)info.GetValue("In1", typeof(Port_Material));
            PortOut = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
            DT = (Port_Signal)info.GetValue("DeltaT", typeof(Port_Signal));
            DP = (Port_Signal)info.GetValue("DeltaP", typeof(Port_Signal));
            Q = (Port_Energy)info.GetValue("Q", typeof(Port_Energy));

            try
            {
                PEff = (Port_Signal)info.GetValue("PEff", typeof(Port_Signal));
                AEff = (Port_Signal)info.GetValue("AEff", typeof(Port_Signal));

                AdiabaticHead = (Port_Signal)info.GetValue("AdiabaticHead", typeof(Port_Signal));
                PolytropicHead = (Port_Signal)info.GetValue("PolytropicHead", typeof(Port_Signal));
                AdiabaticFluidHead = (Port_Signal)info.GetValue("AdiabaticFluidHead", typeof(Port_Signal));
                PolytropicFluidHead = (Port_Signal)info.GetValue("PolytropicFluidHead", typeof(Port_Signal));
                PowerConsumed = (Port_Signal)info.GetValue("PowerConsumed", typeof(Port_Signal));
                PolytropicHeadFactor = (Port_Signal)info.GetValue("PolytropicHeadFactor", typeof(Port_Signal));
                PolytropicExponent = (Port_Signal)info.GetValue("PolytropicExponent", typeof(Port_Signal));
                IsentropicExponent = (Port_Signal)info.GetValue("IsentropicExponent", typeof(Port_Signal));
                Speed = (Port_Signal)info.GetValue("Speed", typeof(Port_Signal));
            }
            catch { }

            Add(AEff);
            Add(PEff);
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(Q);
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", PortIn);
            info.AddValue("Out1", PortOut);
            info.AddValue("DeltaT", DT);
            info.AddValue("DeltaP", DP);
            info.AddValue("Q", Q);
            info.AddValue("PEff", PEff);
            info.AddValue("AEff", AEff);

            info.AddValue("AdiabaticHead", AdiabaticHead);
            info.AddValue("PolytropicHead", PolytropicHead);
            info.AddValue("AdiabaticFluidHead", AdiabaticFluidHead);
            info.AddValue("PolytropicFluidHead", PolytropicFluidHead);
            info.AddValue("PowerConsumed", PowerConsumed);
            info.AddValue("PolytropicHeadFactor", PolytropicHeadFactor);
            info.AddValue("PolytropicExponent", PolytropicExponent);
            info.AddValue("IsentropicExponent", IsentropicExponent);
            info.AddValue("Speed", Speed);
        }

        public override bool Solve()
        {
            DP.flowdirection = FlowDirection.Up;

            AdiabaticHead.Clear();
            PolytropicHead.Clear();
            AdiabaticFluidHead.Clear();
            PolytropicFluidHead.Clear();
            PowerConsumed.Clear();
            PolytropicHeadFactor.Clear();
            PolytropicExponent.Clear();
            IsentropicExponent.Clear();
            Speed.Clear();

            Balance.Calculate(this);
            while (FlashAllPorts() > 0)
                Balance.Calculate(this);

            if (!PortIn.IsFullyDefined)
                return false;

            ThermodynamicsClass.UpdateThermoProperties(PortIn.cc, PortIn.P, PortIn.T, PortIn.Thermo);

            DP.SetPortValue(ePropID.DeltaP, PortOut.P_ - PortIn.P_, SourceEnum.UnitOpCalcResult);

            if (DP.IsKnown)
            {
                ThermodynamicsClass.UpdateThermoProperties(PortIn.cc, PortIn.P_.Value, PortIn.T_.Value, PortIn.cc.Thermo);
                ThermoDifferentialPropsCollection thermoDiffs = ThermodynamicsClass.UpdateThermoDerivativeProperties(PortIn.cc, PortIn.P_.Value, PortIn.T_.Value, PortIn.cc.Thermo, enumFluidRegion.Vapour);
                ExpansionCompression poly;
                ThermoPropsMass InProps = new ThermoPropsMass(PortIn.cc.ThermoVap, PortIn.MW);
                InProps.T = PortIn.T_.BaseValue;
                InProps.P = PortIn.P_.BaseValue;

                if (PEff.IsFixed)
                    poly = new ExpansionCompression(PortIn.MassEnthalpyFlash, PortIn.MassProps, PortIn.P_.Value, PortOut.P_.Value, PortIn.T_.Value, PortIn.MF_.Value, PEff.Value, EffType.poly, Factormethod.Huntington, false);
                else
                    poly = new ExpansionCompression(PortIn.MassEnthalpyFlash, PortIn.MassProps, PortIn.P_.Value, PortOut.P_.Value, PortIn.T_.Value, PortIn.MF_.Value, AEff.Value, EffType.Isen, Factormethod.Huntington, false);

                if (PEff.IsFixed)
                    AEff.Value.ForceSetValue(poly.IsenEff * 100, SourceEnum.UnitOpCalcResult);
                else
                    PEff.Value.ForceSetValue(poly.PolyEff * 100, SourceEnum.UnitOpCalcResult);

                Q.SetPortValue(ePropID.EnergyFlow, poly.Power, SourceEnum.UnitOpCalcResult);
                PortOut.SetPortValue(ePropID.H, PortIn.H_ + poly.Power * 3600 / PortIn.MF_, SourceEnum.UnitOpCalcResult);
                PortOut.Flash();

                PowerConsumed.SetPortValue(ePropID.EnergyFlow, Q.Value, SourceEnum.UnitOpCalcResult);
                AdiabaticHead.Value.ForceSetValue(poly.IsentropicHead, SourceEnum.UnitOpCalcResult);
                AdiabaticFluidHead.Value.ForceSetValue(poly.IsentropicFluidHead, SourceEnum.UnitOpCalcResult);
                IsentropicExponent.Value.ForceSetValue(poly.IsenTropicExponent, SourceEnum.UnitOpCalcResult);
                PolytropicHead.Value.ForceSetValue(poly.PolytropicHead, SourceEnum.UnitOpCalcResult);
                PolytropicFluidHead.Value.ForceSetValue(poly.PolytropicFluidHead, SourceEnum.UnitOpCalcResult);
                PolytropicExponent.Value.ForceSetValue(poly.PolyTropicExponent, SourceEnum.UnitOpCalcResult);
                PolytropicHeadFactor.Value.ForceSetValue(poly.PolytropicFactor, SourceEnum.UnitOpCalcResult);

                return true;
            }
            return false;
        }
    }
}