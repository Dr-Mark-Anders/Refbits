using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Units.UOM;

namespace ModelEngine
{
    public enum DISTPCT { DIST1 = 1, DIST5 = 5, DIST10 = 10, DIST20 = 20, DIST30 = 30, DIST50 = 50, DIST70 = 70, DIST80 = 80, DIST90 = 90, DIST95 = 95, DIST99 = 99, UNDEFINED = 100 }

    [Serializable]
    public class DistPoint : IComparable<DistPoint>, ISerializable
    {
        private double pct;
        private DISTPCT stdpct;
        private UOMProperty bp = new UOMProperty(Units.ePropID.T);

        public int CompareTo(DistPoint comparePart)
        {
            // A null value means that this object is greater.
            return this.pct.CompareTo(comparePart.Pct);
        }

        public DistPoint(SerializationInfo info, StreamingContext context)
        {
            try
            {
                bp = (UOMProperty)info.GetValue("bp", typeof(UOMProperty));
                pct = info.GetDouble("pct");
            }
            catch
            {
                //bp = new((Temperature)info.GetValue("bp", typeof(Temperature)));
            }
        }

        public SourceEnum Origin
        {
            get
            {
                return bp.Source;
            }
            set
            {
                bp.Source = value;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("bp", bp, typeof(UOMProperty));
            info.AddValue("pct", Pct);
        }

        internal DistPoint Clone()
        {
            DistPoint n = new();
            n.pct = pct;
            n.bp = bp.Clone();
            n.stdpct = stdpct;

            return n;
        }

        public double Pct
        {
            get
            {
                return pct;
            }

            set
            {
                pct = value;
            }
        }

        public UOMProperty BP_UOM
        {
            get
            {
                return bp;
            }

            set
            {
                bp = value;
            }
        }

        public Temperature BP
        {
            get
            {
                return (Temperature)bp.UOM;
            }

            set
            {
                bp.UOM = value;
            }
        }

        public DISTPCT Stdpct { get => stdpct; set => stdpct = value; }

        public DistPoint(double PCT, UOMProperty BP)
        {
            pct = PCT;
            bp = BP;
        }

        public DistPoint(double PCT, Temperature BP, SourceEnum origin = SourceEnum.Empty)
        {
            pct = PCT;
            bp = new UOMProperty(Units.ePropID.T, origin, BP);

            if (Enum.IsDefined(typeof(DISTPCT), (int)pct))
            {
                this.stdpct = (DISTPCT)(int)pct;
            }
        }

        public DistPoint()
        {
        }
    }
}
