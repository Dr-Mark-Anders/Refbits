using ModelEngine;
using System;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class CalcProperty : UOMProperty, ISerializable, ICloneable
    {
        private string displayName = "";

        public CalcProperty(string Name, ePropID id, double val) : base(id, val)
        {
            Source = SourceEnum.Empty;
            OriginPortGuid = Guid.Empty;
            guid = Guid.NewGuid();
            this.displayName = Name;
            this.DisplayUnit = UOM.DefaultUnit;
        }

        protected CalcProperty(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            UOM = (IUOM)info.GetValue("uom", typeof(IUOM));
            guid = Guid.NewGuid();
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public override string ToString()
        {
            if (base.Value.Equals(double.NaN))
                return double.NaN.ToString();
            else
                return base.Value.ToString();
        }

        public static implicit operator double(CalcProperty a)
        {
            if (a == null)
                return double.NaN;
            return a.Value;
        }

        public void Clear()
        {
            Source = SourceEnum.Empty;
            base.Value = double.NaN;
            OriginPortGuid = Guid.Empty;
        }

        public static double operator *(CalcProperty a, double b)
        {
            return a.Value * b;
        }

        public bool SetValue(UOMProperty newValue, SourceEnum source)
        {
            StreamProperty sp = new(newValue.Propid, newValue.BaseValue, source);
            return SetValue(sp);
        }

        /// <summary>
        /// Updatesvaluesandchecksforinconsistency
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="overrideinputs"></param>
        /// <returns></returns>
        public bool SetValue(StreamProperty newValue, bool overrideinputs = false, InconsistencyObject Iobj = null)
        {
            FlowSheet fs = null;
            SourceEnum temp = newValue.Source;

            if (FlowSheet.CalcType == SourceEnum.CalcEstimate
                && newValue.Source == SourceEnum.UnitOpCalcResult)
                temp = SourceEnum.CalcEstimate;

            if (FlowSheet.TransferType == SourceEnum.Transferred
                && newValue.Source == SourceEnum.CalcEstimate)
                temp = SourceEnum.CalcEstimate;

            if (this.Source == SourceEnum.FixedEstimate && newValue.Source == SourceEnum.FixedEstimate) //update independent variable
            {
                this.Value = newValue.Value;
                this.Source = temp;
                this.OriginPortGuid = newValue.Guid;
                return true;
            }

            if (!this.IsKnown) //no previous value
            {
                this.Value = newValue.Value;
                this.Source = temp;
                this.OriginPortGuid = newValue.Guid;
                return true;
            }

            bool IsConsistent = CheckConsistency(newValue);

            if (overrideinputs)
            {
                switch (Source)
                {
                    case SourceEnum.Input:
                    case SourceEnum.CalcEstimate:          // Can Be overwritten
                    case SourceEnum.Transferred:
                    case SourceEnum.UnitOpCalcResult:
                        this.Value = newValue.Value;
                        this.Source = temp;
                        this.OriginPortGuid = newValue.Guid;
                        return true;

                    default:
                        if (!IsConsistent)
                            if (!FlowSheet.InconsistencyStack.Contains(Iobj))
                                FlowSheet.InconsistencyStack.Add(Iobj);
                        return false;
                }
            }
            else
            {
                switch (Source) // old value source
                {
                    case SourceEnum.CalcEstimate:          //CanBeoverwritten
                        if (FlowSheet.Valuemode == eValueMode.OverwriteEstimates || FlowSheet.Valuemode == eValueMode.Estimates)
                        {
                            this.Value = newValue.Value;
                            this.Source = temp;
                            this.OriginPortGuid = newValue.Guid;
                        }
                        return true;

                    case SourceEnum.UnitOpCalcResult:
                    case SourceEnum.Transferred:
                    case SourceEnum.Input:
                    default:
                        if (!IsConsistent && fs != null)
                            if (!FlowSheet.InconsistencyStack.Contains(Iobj))
                                FlowSheet.InconsistencyStack.Add(Iobj);
                        return false;
                }
            }
        }

        internal void SetValue(double value, SourceEnum origin)
        {
            BaseValue = value;
            base.origin = origin;
        }

        public double Tolerance
        {
            get { return UOM.Tolerance; }
        }

        public bool CheckConsistency(double sc)
        {
            if (Math.Abs((this.Value - sc) / this.Value) > this.Tolerance)
                return false;
            return true;
        }

        public object CloneDeep()
        {
            StreamProperty res = new(base.Propid);
            res.BaseValue = base.BaseValue;
            res.OriginPortGuid = this.OriginPortGuid;
            res.guid = this.guid;
            res.origin = this.origin;
            return res;
        }

        public new object Clone()
        {
            StreamProperty res = new(base.Propid);
            res.BaseValue = base.BaseValue;
            res.OriginPortGuid = this.OriginPortGuid;
            res.origin = this.origin;
            return res;
        }

        public new bool IsKnown
        {
            get
            {
                if (double.IsNaN(base.Value) || origin == SourceEnum.Empty)
                    return false;
                else
                    return true;
            }
        }

        public new bool IsInput
        {
            get
            {
                if (origin == SourceEnum.Input)
                    return true;
                else
                    return false;
            }
        }

        public bool IsEditable
        {
            get
            {
                switch (this.Source)
                {
                    case SourceEnum.Input:
                    case SourceEnum.Empty:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public override string Name
        {
            get
            {
                if (UOM is null)
                    return "NA";
                else
                    return UOM.Name;
            }
        }

        public new string DisplayName { get => displayName; set => displayName = value; }
    }
}