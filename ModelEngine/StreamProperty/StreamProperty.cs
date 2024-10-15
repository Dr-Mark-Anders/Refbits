using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public partial class StreamProperty : UOMProperty, ISerializable
    {
        public Port Port;

        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        protected virtual void RaiseChangeEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            ValueChanged?.Invoke(this, new EventArgs());
        }

        public StreamProperty(ePropID id) : base(id, double.NaN)
        {
            Source = SourceEnum.Empty;
            OriginPortGuid = Guid.Empty;
            guid = Guid.NewGuid();
        }

        public StreamProperty(ePropID id, double val, SourceEnum origin = SourceEnum.Input) : base(id, val)
        {
            this.Source = origin;
            OriginPortGuid = Guid.Empty;
            guid = Guid.NewGuid();
        }

        public StreamProperty(ePropID id, double val, SourceEnum origin, Guid PortGuid) : base(id, val)
        {
            this.Source = origin;
            this.OriginPortGuid = PortGuid;
            guid = Guid.NewGuid();
        }

        protected StreamProperty(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            //propUOM = (IUOM)info.GetValue("uom", typeof(IUOM));
            guid = Guid.NewGuid();
        }

        public SourceEnum Origin
        {
            get
            {
                return origin;
            }
        }

        [Browsable(true)]
        public override Guid OriginPortGuid
        {
            get
            {
                return base.OriginPortGuid;
            }
        }

        [Browsable(true)]
        public override Guid OriginUnitOPGuid { get => base.OriginUnitOPGuid; set => base.OriginUnitOPGuid = value; }
            



        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public bool IsEstimate()
        {
            switch (this.Source)
            {
                case SourceEnum.CalcEstimate:
                case SourceEnum.FixedEstimate:
                    return true;

                case SourceEnum.Input:
                case SourceEnum.Default:
                case SourceEnum.UnitOpCalcResult:
                default:
                    return false;
            }
        }

        public bool IsCalcResult
        {
            get
            {
                switch (this.Source)
                {
                    case SourceEnum.UnitOpCalcResult:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public bool IsCalcSpec
        {
            get
            {
                switch (this.Source)
                {
                    case SourceEnum.CalculatedSpec:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public bool IsCalcEstimate()
        {
            switch (this.Source)
            {
                case SourceEnum.CalcEstimate:
                    return true;

                default:
                    return false;
            }
        }

        public bool IsErasable()
        {
            if (this != null)
            {
                switch (this.Source)
                {
                    case SourceEnum.Input:
                    case SourceEnum.FixedEstimate:
                    case SourceEnum.Default:
                        return false;

                    case SourceEnum.CalcEstimate:
                    case SourceEnum.Transferred:
                    case SourceEnum.UnitOpCalcResult:
                    default:
                        return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            if (base.Value.Equals(double.NaN))
                return double.NaN.ToString();
            else
                return base.Value.ToString();
        }

        public static implicit operator double(StreamProperty a)
        {
            if (a == null)
                return double.NaN;
            return a.Value;
        }

        public new void Clear()
        {
            Source = SourceEnum.Empty;
            base.Value = double.NaN;
            OriginPortGuid = Guid.Empty;
            OriginUnitOPGuid = Guid.Empty;
        }

        public static double operator *(StreamProperty a, double b)
        {
            return a.Value * b;
        }

        public bool IsConsistent(StreamProperty b)
        {
            if (this is null || b is null)
                return false;

            var a1 = (double)this;
            var b1 = (double)b;
            if (Math.Abs(a1 - b1) / a1 > this.Tolerance)
                return false;
            return true;
        }

        public double Tolerance
        {
            get { return UOM.Tolerance; }
        }

        public bool CheckConsistency(double sc)
        {
            if (!double.IsNormal(this.Value))
                return false;

            if (!double.IsNaN(sc) && double.IsNaN(this.Value))
                return false;

            if (double.IsNaN(sc) && !double.IsNaN(this.Value))
                return false;

            if (double.IsNaN(sc) && double.IsNaN(this.Value))
                return true;

            if (Math.Abs((this.Value - sc) / this.Value) > this.Tolerance)
                return false;
            return true;
        }

        public bool CheckRecycleConsistency(double sc)
        {
            if (!double.IsNaN(BaseValue) && double.IsNaN(sc))
                return false;
            if (Math.Abs((this.Value - sc) / this.Value) > this.Tolerance)
                return false;
            return true;
        }

        public object CloneDeep()
        {
            StreamProperty res = new(base.Propid);
            res.BaseValue = base.BaseValue;
            res.OriginPortGuid = base.OriginPortGuid;
            res.guid = this.guid;
            res.origin = this.origin;
            return res;
        }

        public UOMProperty CloneShallow() // no orginguid
        {
            StreamProperty res = new(base.Propid);
            res.BaseValue = base.BaseValue;
            res.origin = this.origin;
            return res;
        }

        public override StreamProperty Clone()
        {
            StreamProperty res = new(base.Propid);
            res.BaseValue = base.BaseValue;
            res.OriginPortGuid = base.OriginPortGuid;
            res.origin = this.origin;
            return res;
        }

        public new bool IsKnown
        {
            get
            {
                if (!double.IsFinite(base.Value) || origin == SourceEnum.Empty)
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

        public bool IsTransferred
        {
            get
            {
                switch (origin)
                {
                    case SourceEnum.Transferred:
                    case SourceEnum.Input:
                        return true;

                    case SourceEnum.UnitOpCalcResult:
                        break;

                    case SourceEnum.Default:
                        break;

                    case SourceEnum.Empty:
                        break;

                    case SourceEnum.CalcEstimate:
                        break;

                    case SourceEnum.FixedEstimate:
                        break;

                    case SourceEnum.NotConnected:
                        break;

                    case SourceEnum.TransEstimate:
                        break;

                    default:
                        break;
                }
                return false;
            }
        }
        public bool IsFromUnitOP
        {
            get
            {
                return origin == SourceEnum.UnitOpCalcResult;    
            }
        }

        public string ToString(string format)
        {
            return BaseValue.ToString(format);
        }
    }
}