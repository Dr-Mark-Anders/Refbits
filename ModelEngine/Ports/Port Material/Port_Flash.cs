using System;
using System.Linq;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public partial class Port_Material
    {
        public bool Flash(enumFlashType enumFlashType, bool calcderivatives = false, ThermoDynamicOptions thermo = null)
        {
            return this.Flash(true, enumFlashType, false, thermo);
        }

        public bool Flash(bool forceflash = false, enumFlashType flashtype = enumFlashType.None,
            bool calcderivatives = false, ThermoDynamicOptions thermo = null)
        {
            bool solved = false;
            if (thermo == null)
                thermo = this.cc.Thermo;

            if (Components.ContainsTypes(cc.CompList, typeof(SolidComponent)) && cc.Solids().MoleFractions.Sum().AlmostEquals(1))
            {
                isflashed = true; // only solids so assume flashed.
                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo, enumFluidRegion.TwoPhase);
                UpdatePortBulkProps(this, this.Guid, enumFlashType.solid);
                return true;
            }

            if (flashtype == enumFlashType.None)
            {
                flashtype = FlashTypes.FlashType(this);
            }

            if (Properties.CountValid < 2)
            {
                UpdateFlows();
                return false;
            }

            Guid PortGuid = FlasheableGuidSource();

            if (!forceflash && cc.Origin == SourceEnum.Empty)  // Don't flash is comps no set
                return false;

            cc.NormaliseFractions(FlowFlag.Molar); // ensure all fractions are upto date, vol and mass from molar

            if (!IsFullyDefined || forceflash)
            {
                ClearPortCalcValues();

                solved = FlashClass.Flash(this, thermo, PortGuid, flashtype, calcderivatives);

                if (solved)
                {
                    ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo, enumFluidRegion.TwoPhase);

                    if (calcderivatives)
                    {
                        ThermoLiqDerivatives = ThermodynamicsClass.UpdateThermoDerivativeProperties(cc, P, T, Thermo, enumFluidRegion.Liquid);
                        ThermoVapDerivatives = ThermodynamicsClass.UpdateThermoDerivativeProperties(cc, P, T, Thermo, enumFluidRegion.Vapour);
                    }

                    UpdatePortBulkProps(this, PortGuid, flashtype);

                    this.IsFlashed = true;

                    UpdateFlows();
                }
                else
                {
                    H_.Clear();
                    S_.Clear();
                    for (int i = 0; i < cc.Count; i++)
                        cc[i].MoleFracVap = double.NaN;
                    UpdateFlows();
                }
            }
            else
            {
                UpdateFlows();
            }

            IsDirty = false;
            return solved;
        }

        public void UpdatePortBulkPropsNew(Port_Material port, Guid originGuid, enumFlashType flashtype)
        {
            switch (flashtype)
            {
                case enumFlashType.solid:
                    port.SetPortValue(ePropID.Q, 0, SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.H, cc.ThermoSolids.H, SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.S, cc.ThermoSolids.S, SourceEnum.PortCalcResult);

                    port.Q_.OriginPortGuid = originGuid;
                    port.H_.OriginPortGuid = originGuid;
                    port.S_.OriginPortGuid = originGuid;

                    break;
                case enumFlashType.PT:

                    /*    port.Q_.origin = SourceEnum.PortCalcResult;
                        port.H = cc.StreamEnthalpy(port.Q);
                        port.H_.origin = SourceEnum.PortCalcResult;
                        port.S = cc.StreamEntropy(port.Q);
                        port.S_.origin = SourceEnum.PortCalcResult;*/

                    port.SetPortValue(ePropID.Q, port.Q, SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.H, cc.StreamEnthalpy(port.Q), SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.S, cc.StreamEnthalpy(port.Q), SourceEnum.PortCalcResult);

                    port.Q_.OriginPortGuid = originGuid;
                    port.H_.OriginPortGuid = originGuid;
                    port.S_.OriginPortGuid = originGuid;
                    break;

                case enumFlashType.PQ:
                    port.SetPortValue(ePropID.T, port.T, SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.H, cc.StreamEnthalpy(port.Q), SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.S, cc.StreamEnthalpy(port.Q), SourceEnum.PortCalcResult);

                    /*port.T_.origin = SourceEnum.PortCalcResult;
                    port.H = cc.StreamEnthalpy(port.Q);
                    port.H_.origin = SourceEnum.PortCalcResult;
                    port.S = cc.StreamEntropy(port.Q);
                    port.S_.origin = SourceEnum.PortCalcResult;*/

                    port.T_.OriginPortGuid = originGuid;
                    port.H_.OriginPortGuid = originGuid;
                    port.S_.OriginPortGuid = originGuid;
                    break;

                case enumFlashType.PH:
                    port.SetPortValue(ePropID.T, port.T, SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.Q, port.Q, SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.S, cc.StreamEntropy(port.Q), SourceEnum.PortCalcResult);

                    /*port.T_.origin = SourceEnum.PortCalcResult;
                    port.Q_.origin = SourceEnum.PortCalcResult;
                    port.S = cc.StreamEntropy(port.Q);
                    port.S_.origin = SourceEnum.PortCalcResult;*/

                    port.Q_.OriginPortGuid = originGuid;
                    port.T_.OriginPortGuid = originGuid;
                    port.S_.OriginPortGuid = originGuid;
                    break;

                case enumFlashType.PS:
                    port.SetPortValue(ePropID.T, port.T, SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.Q, port.Q, SourceEnum.PortCalcResult);
                    port.SetPortValue(ePropID.H, cc.StreamEnthalpy(port.Q), SourceEnum.PortCalcResult);


                    /*port.T_.origin = SourceEnum.PortCalcResult;
                    port.Q_.origin = SourceEnum.PortCalcResult;
                    port.H = cc.StreamEnthalpy(port.Q);
                    port.H_.origin = SourceEnum.PortCalcResult;*/

                    port.Q_.OriginPortGuid = originGuid;
                    port.H_.OriginPortGuid = originGuid;
                    port.T_.OriginPortGuid = originGuid;
                    break;
            }
        }


        public void UpdatePortBulkProps(Port_Material port, Guid originGuid, enumFlashType flashtype)
        {
            switch (flashtype)
            {
                case enumFlashType.solid:
                    port.H = cc.ThermoSolids.H;
                    port.H_.origin = SourceEnum.PortCalcResult;
                    port.S = cc.ThermoSolids.S;
                    port.S_.origin = SourceEnum.PortCalcResult;
                    port.Q = 0;
                    port.Q_.origin = SourceEnum.PortCalcResult;
                    break;

                case enumFlashType.PT:                  
                    port.Q_.origin = SourceEnum.PortCalcResult;
                    port.H = cc.StreamEnthalpy(port.Q);
                    port.H_.origin = SourceEnum.PortCalcResult;
                    port.S = cc.StreamEntropy(port.Q);
                    port.S_.origin = SourceEnum.PortCalcResult;                  
                    port.Q_.OriginPortGuid = originGuid;
                    port.H_.OriginPortGuid = originGuid;
                    port.S_.OriginPortGuid = originGuid;
                    break;

                case enumFlashType.PQ:
                    port.T_.origin = SourceEnum.PortCalcResult;
                    port.H = cc.StreamEnthalpy(port.Q);
                    port.H_.origin = SourceEnum.PortCalcResult;
                    port.S = cc.StreamEntropy(port.Q);
                    port.S_.origin = SourceEnum.PortCalcResult;

                    port.T_.OriginPortGuid = originGuid;
                    port.H_.OriginPortGuid = originGuid;
                    port.S_.OriginPortGuid = originGuid;
                    break;

                case enumFlashType.PH:
                    port.T_.origin = SourceEnum.PortCalcResult;
                    port.Q_.origin = SourceEnum.PortCalcResult;
                    port.S = cc.StreamEntropy(port.Q);
                    port.S_.origin = SourceEnum.PortCalcResult;

                    port.Q_.OriginPortGuid = originGuid;
                    port.T_.OriginPortGuid = originGuid;
                    port.S_.OriginPortGuid = originGuid;
                    break;

                case enumFlashType.PS:
                    port.T_.origin = SourceEnum.PortCalcResult;
                    port.Q_.origin = SourceEnum.PortCalcResult;
                    port.H = cc.StreamEnthalpy(port.Q);
                    port.H_.origin = SourceEnum.PortCalcResult;

                    port.Q_.OriginPortGuid = originGuid;
                    port.H_.OriginPortGuid = originGuid;
                    port.T_.OriginPortGuid = originGuid;
                    break;
            }
        }

        public double MassEnthalpyFlash(Pressure P, MassEnthalpy H)
        {
            Port_Material pm = this.Clone();
            pm.P = P;
            pm.H = (double)(H * pm.MW);
            pm.T_.Clear();
            pm.Flash(true, enumFlashType.PH, true, this.cc.Thermo);
            return pm.T_;
        }
    }
}