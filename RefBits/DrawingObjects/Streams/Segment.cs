using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Main.DrawingObjects.Streams
{
    [Serializable]
    public class BaseSegment
    {
        private Guid guid { get; set; }
        private bool isFixed = false;

        public bool IsFixed { get => isFixed; set => isFixed = value; }

        public virtual int Length
        {
            get
            {
                return 0;
            }
        }

        public virtual int Center
        {
            get
            {
                return 0;
            }
        }

        public virtual void Move(int a, int b)
        {
        }

        public virtual FixablePoint Point1 { get { return null; } }
        public virtual FixablePoint Point2 { get { return null; } }

        internal void Update(BaseSegment segment)
        {
            if (this is SegmentH segH)
            {
                switch (segment)
                {
                    case SegmentH hnew:
                        if (!this.isFixed)
                            segH.Y = hnew.Y;
                        segH.X1 = hnew.X1;
                        segH.X2 = hnew.X2;
                        break;
                }
            }
            else if(this is SegmentV segv)
            {
                switch (segment)
                {
                    case SegmentV vnew:
                        if (!this.isFixed)
                            segv.X = vnew.X;
                        segv.Y1 = vnew.Y1;
                        segv.Y2= vnew.Y2;
                        break;
                }
            }
        }
    }

    [Serializable]
    public class SegmentH : BaseSegment, ISerializable
    {
        private int x1, x2, y;

        public SegmentH(SegmentH seg)
        {
            x1 = seg.X1; x2 = seg.X2; y = seg.Y;
        }

        public SegmentH(int X1, int X2, int Y)
        {
            x1 = X1; x2 = X2; y = Y;
        }

        public SegmentH()
        {
        }

        public int X1 { get => x1; set => x1 = value; }
        public int X2 { get => x2; set => x2 = value; }
        public int Y { get => y; set => y = value; }

        public int Mid
        {
            get
            {
                int m = 0;
                if (x2 >= x1)
                    m = (x2 - x1) / 2 + x1;
                else
                    m = (x1 - x2) / 1 + x2;

                return m;
            }
        }

        public void Halve()
        {
            x2 = Mid;
        }

        internal SegmentH Clone()
        {
            SegmentH segh = new SegmentH(this);
            return segh;
        }

        public override int Center
        {
            get
            {
                return Math.Abs(X2 - X1) / 2;
            }
        }

        public int Left
        {
            get
            {
                if (x2 > x1)
                    return x2;
                else
                    return x1;
            }
        }

        public int Right
        {
            get
            {
                if (x2 < x1)
                    return x2;
                else
                    return x1;
            }
        }

        public override int Length
        {
            get
            {
                return Math.Abs(x2 - x1);
            }
        }

        public override FixablePoint Point1
        {
            get
            {
                return new Point(X1, Y);
            }
        }

        public override FixablePoint Point2
        {
            get
            {
                return new Point(X2, Y);
            }
        }

        public override void Move(int dx, int dy)
        {
            x1 += dx;
            x2 += dx;
            y += dy;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X1", x1);
            info.AddValue("X2", x2);
            info.AddValue("Y", y);
            info.AddValue("Fixed", IsFixed);
        }

        public SegmentH(SerializationInfo info, StreamingContext context)
        {
            try
            {
                x1 = info.GetInt32("X1");
                x2 = info.GetInt32("X2");
                Y = info.GetInt32("Y");
                IsFixed = info.GetBoolean("Fixed");
            }
            catch { }
        }
    }

    [Serializable]
    public class SegmentV : BaseSegment,ISerializable
    {
        private int x, y1, y2;

        public SegmentV(SegmentV seg)
        {
            x = seg.X; y1 = seg.Y1; y2 = seg.Y2;
        }

        public SegmentV(int X, int Y1, int Y2)
        {
            x = X; y1 = Y1; y2 = Y2;
        }

        public SegmentV()
        {
        }

        public override int Length
        {
            get
            {
                return Math.Abs(y2 - y1);
            }
        }

        public override int Center
        {
            get
            {
                return Math.Abs(y2 - y1) / 2;
            }
        }

        public int X { get => x; set => x = value; }
        public int Y1 { get => y1; set => y1 = value; }
        public int Y2 { get => y2; set => y2 = value; }

        public int Mid
        {
            get
            {
                int m = 0;
                if (y2 >= y1)
                    m = (y2 - y1) / 2 + y1;
                else
                    m = (y1 - y2) / 1 + y2;

                return m;
            }
        }

        public void Halve()
        {
            y2 = Mid;
        }

        internal BaseSegment Clone()
        {
            SegmentV segv = new SegmentV(this);
            return segv;
        }

        public int Top
        {
            get
            {
                if (y2 > y1)
                    return y2;
                else
                    return y1;
            }
        }

        public int Bottom
        {
            get
            {
                if (y2 < y1)
                    return y2;
                else
                    return y1;
            }
        }

        public override FixablePoint Point1
        {
            get
            {
                return new Point(X, Y1);
            }
        }

        public override FixablePoint Point2
        {
            get
            {
                return new Point(X, Y2);
            }
        }

        public override void Move(int dx, int dy)
        {
            X += dx;
            y1 += dy;
            y2 += dy;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", x);
            info.AddValue("Y1", y1);
            info.AddValue("Y2", y2);
            info.AddValue("Fixed", IsFixed);
        }

        public SegmentV(SerializationInfo info, StreamingContext context)
        {
            try
            {
                X = info.GetInt32("X");
                y1 = info.GetInt32("Y1");
                Y2 = info.GetInt32("Y2");
                IsFixed = info.GetBoolean("Fixed");
            }
            catch { }
        }   
    }
}