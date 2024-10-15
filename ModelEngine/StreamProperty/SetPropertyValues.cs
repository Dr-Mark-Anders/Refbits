using System;
using System.Reflection.Metadata.Ecma335;

namespace ModelEngine
{
    public partial class StreamProperty
    {
        public bool SetPropValue(StreamProperty newValue)
        {
            bool IsConsistent = CheckConsistency(newValue);
            if (IsConsistent) // nothing to do
                return false;

            if (FlowSheet.CalcType == SourceEnum.CalcEstimate
                && newValue.Source == SourceEnum.UnitOpCalcResult)
                newValue.Source = SourceEnum.CalcEstimate;

            if (this.OriginPortGuid == Guid.Empty)
                this.OriginPortGuid = newValue.OriginPortGuid;

            if (this.OriginUnitOPGuid == Guid.Empty)
                this.OriginUnitOPGuid = newValue.OriginUnitOPGuid;

            switch (this.Source)
            {
                case SourceEnum.PortCalcResult:
                    break;

                case SourceEnum.Input:
                    if (newValue.IsInput) // both inputs, can overwrite
                    {
                        this.Value = newValue.Value;
                        this.Source = newValue.Source;
                        this.OriginPortGuid = newValue.OriginPortGuid;
                        this.IsDirty = true;
                        return true;
                    }
                    break;

                case SourceEnum.UnitOpCalcResult:
                    if (this.OriginPortGuid == newValue.OriginPortGuid)
                    {
                        this.Value = newValue.Value;
                        this.Source = newValue.Source;
                        this.OriginPortGuid = newValue.OriginPortGuid;
                        this.IsDirty = true;
                        return true;
                    }
                    break;

                case SourceEnum.Empty:
                    {
                        this.Value = newValue.Value;
                        this.Source = newValue.Source;
                        this.OriginPortGuid = newValue.OriginPortGuid;
                        this.IsDirty = true;
                        return true;
                    }
                case SourceEnum.FixedEstimate:
                    if (newValue.Source == SourceEnum.FixedEstimate) // update independent variable
                    {
                        this.Value = newValue.Value;
                        this.Source = newValue.Source;
                        this.OriginPortGuid = newValue.Guid;
                        this.IsDirty = true;
                        return true;
                    }
                    break;
            }
            return false;
        }

        internal void ForceSetValue(double value, SourceEnum origin)
        {
            BaseValue = value;
            if (double.IsNaN(value))
                base.origin = SourceEnum.Empty;
            else
                base.origin = origin;
        }

        internal void offset(double offset)
        {
            double val = this.DisplayValueOut();
            val += offset;
            this.DisplayValueIn(val);
        }

        internal void mult(double mult)
        {
            double val = this.DisplayValueOut();
            val *= mult;
            this.DisplayValueIn(val);
        }

        /// <summary>
        /// NaN is not counted as external
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
      /*  internal bool IsFromExternalPort(Guid guid)
        {
            if (IsInput)
                return true;
            if (double.IsNaN(this.Value))
                return false;
            return guid != OriginPortGuid;
        }*/

        internal bool IsFromExternalPort(Port port)
        {
            if (port is null) // missiing port
                return false;

            switch (origin)
            {
                case SourceEnum.UnitOpCalcResult:
                    if (OriginUnitOPGuid != port.Owner.Guid)
                        return true;
                    break;

                case SourceEnum.PortCalcResult:
                    if (port.Guid != OriginPortGuid)  // value is not form Unit OP, either port calc or external
                        return true;
                    else
                        return false;

                    break;

                case SourceEnum.Input:
                    if (IsInput) // external
                        return true;
                    break;

                case SourceEnum.Empty:
                    if (double.IsNaN(this.Value)) // empty value
                        return false;
                    break;
            }

            return false;
        }
    }
}