using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Main.DrawingObjects.Streams
{
    [Serializable, TypeConverter(typeof(SegmentArrayExpander))]
    public class SegmentArray
    {
        private List<BaseSegment> list = new();

        public enum SegArrayType
        {
            NONE,
            FFALBL, FFARBR,

            FBBLBR, FBARAL,

            FDALAR, FDBLBR,

            FUALAR, FUBLBR,


            BBALBL, BBARBR,

            BFALAR, BFBLBR,

            BDALAR, BDBLBR,

            BUBL, BUBLBR,


            UFARAR, UFBLBR,

            UUALAR, UUBLBR,

            UDALAR, UDBLBR,

            UBALAR, UBBLBR,


            DBALAR, DBBLBR,

            DFALAR, DFBLBR,

            DDALAR, DDBLBR,

            DUALAR, DUBLBR,
            FDBR,
            FDBL,
            FDAL,
            BDBR,
            DBBR,
            DBBL,
            DBAR,
            DBAL,
            UFBR,
            DFAL,
            DFAR,
            BUAL,
            FBBL,
            FBBR,
            FFBL,
            FFAL,
            UBAL,
            UBAR,
            UBBR,
            UBBL,
        }

        public SegArrayType segArrayType = SegArrayType.NONE;

        public SegmentArray()
        {
            list = new List<BaseSegment> { };
            Reset = true;
        }

        public void Recalculate()
        {
            for (int i = 1; i < list.Count - 1; i++)
            {
                BaseSegment segment1 = list[i - 1];
                BaseSegment segment2 = list[i];
                BaseSegment segment3 = list[i + 1];

                switch (segment2)
                {
                    case SegmentH segh:
                        if (segment1 is SegmentH || segment3 is SegmentH)
                            return; 

                        {
                            SegmentV Seg1 = (SegmentV)segment1;
                            SegmentV Seg3 = (SegmentV)segment3;

                            Seg1.Y2 = segh.Y;
                            Seg3.Y1 = segh.Y;

                            break;
                        }
                    case SegmentV segv:
                        if (segment1 is SegmentV || segment3 is SegmentV)
                            return;
                        {
                            SegmentH Seg1 = (SegmentH)segment1;
                            SegmentH Seg3 = (SegmentH)segment3;

                            Seg1.X2 = segv.X;
                            Seg3.X1 = segv.X;
                        }
                        break;
                }
            }
        }
    

        public BaseSegment GetSegment(Point p)
        {
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i])
                {
                    case SegmentH sgH:
                        if (Math.Abs(sgH.Y - p.Y) < 7)
                        {
                            if (sgH.X1 > p.X && sgH.X2 < p.X || sgH.X1 < p.X && sgH.X2 > p.X)
                                return sgH;
                        }
                        break;

                    case SegmentV sgV:
                        if (Math.Abs(sgV.X - p.X) < 7)
                        {
                            if (sgV.Y1 > p.Y && sgV.Y2 < p.Y || sgV.Y1 < p.Y && sgV.Y2 > p.Y)
                                return sgV;
                        }
                        break;
                }
            }

            return null;
        }

        public bool ContainsPoint(Point p)
        {
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i])
                {
                    case SegmentH sgH:
                        if (Math.Abs(sgH.Y - p.Y) < 7)
                        {
                            if (sgH.X1 > p.X && sgH.X2 < p.X || sgH.X1 < p.X && sgH.X2 > p.X)
                                return true;
                        }
                        break;

                    case SegmentV sgV:
                        if (Math.Abs(sgV.X - p.X) < 7)
                        {
                            if (sgV.Y1 > p.Y && sgV.Y2 < p.Y || sgV.Y1 < p.Y && sgV.Y2 > p.Y)
                                return true;
                        }
                        break;
                }
            }

            return false;
        }

        public void UpdateSegment(BaseSegment BaseSeg)
        {
            int Index;
            switch (BaseSeg)
            {
                case SegmentH segH:
                    Index = list.IndexOf(segH);
                    if (Index > 0 && Index < list.Count)
                    {
                        if (list[Index - 1] is SegmentV segv)
                        {
                            segv.Y2 = segH.Y;
                        }
                        if (list[Index + 1] is SegmentV seg1)
                        {
                            seg1.Y1 = segH.Y;
                        }
                    }
                    break;

                case SegmentV segV:
                    Index = list.IndexOf(segV);
                    if (Index > 0 && Index < list.Count)
                    {
                        if (list[Index - 1] is SegmentH segH)
                        {
                            segH.X2 = segH.Y;
                        }
                        if (list[Index + 1] is SegmentH segH1)
                        {
                            segH1.X1 = segH1.Y;
                        }
                    }
                    break;
            }
        }

        public void CreateArray(int no, bool firstIsVertical)
        {
            list.Clear();
            if (firstIsVertical)
            {
                for (int i = 0; i < no; i++)
                {
                    if (IsEven(i))
                        list.Add(new SegmentV(0, 0, 0));
                    else
                        list.Add(new SegmentH(0, 0, 0));
                }
            }
            else
            {
                for (int i = 0; i < no; i++)
                {
                    if (IsEven(i))
                        list.Add(new SegmentH(0, 0, 0));
                    else
                        list.Add(new SegmentV(0, 0, 0));
                }
            }
        }

        public bool IsEven(int num)
        {
            if (num % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ConnectSegments(BaseSegment segmentmoved)
        {
            int segno = list.IndexOf(segmentmoved);
            BaseSegment nextseg = null, previousseg = null;

            switch (segno)
            {
                case 0:
                    nextseg = list[1];
                    break;

                default:
                    previousseg = list[segno - 1];
                    nextseg = list[segno + 1];
                    break;
            }
        }

        public void SplitSegment(BaseSegment seg)
        {
            int segno = list.IndexOf(seg);

            switch (seg)
            {
                case SegmentV sv:
                    list.Insert(segno, new SegmentH(sv.X, sv.X, sv.Mid));
                    list.Insert(segno, new SegmentV(sv.X, sv.Mid, sv.Top));
                    sv.Halve();
                    break;

                case SegmentH sh:
                    list.Insert(segno, new SegmentV(sh.Mid, sh.Y, sh.Y));
                    list.Insert(segno, new SegmentH(sh.Mid, sh.Right, sh.Y));
                    sh.Halve();
                    break;
            }
        }

        public FixablePointArray Points()
        {
            FixablePointArray points = new FixablePointArray();
            for (int i = 0; i < list.Count - 1; i++)
            {
                switch (list[i])
                {
                    case SegmentH sh:
                        points.Add(sh.Point1);
                        break;

                    case SegmentV sv:
                        points.Add(sv.Point1);
                        break;
                }
            }

            switch (list[^1])
            {
                case SegmentH sh:
                    points.Add(sh.Point2);
                    break;

                case SegmentV sv:
                    points.Add(sv.Point2);
                    break;
            }
            return points;
        }

        public BaseSegment this[int index]
        {
            get
            {
                if(list.Count == 0) return null;
                    
                return ((IList<BaseSegment>)list)[index];
            }
            set
            {
                ((IList<BaseSegment>)list)[index] = value;
            }
        }

        public int Count => ((ICollection<BaseSegment>)list).Count;

        public bool IsReadOnly => ((ICollection<BaseSegment>)list).IsReadOnly;

        public List<BaseSegment> List { get => list; set => list = value; }

        public Point? Start
        {
            get
            {
                if (list.Count > 0 && list[0] is SegmentH sh)
                    return sh.Point1;
                if (list.Count > 0 && list[0] is SegmentV sv)
                    return sv.Point1;
                return null;
            }
        }

        public Point? End
        {
            get
            {
                if (list.Count == 0)
                    return null;
                if (list.Last() is SegmentH sh)
                    return sh.Point2;
                if (list.Last() is SegmentV sv)
                    return sv.Point2;
                return null;
            }
        }

        public void Add(BaseSegment item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            ((ICollection<BaseSegment>)list).Clear();
        }

        public bool Contains(BaseSegment item)
        {
            return ((ICollection<BaseSegment>)list).Contains(item);
        }

        public void CopyTo(BaseSegment[] array, int arrayIndex)
        {
            ((ICollection<BaseSegment>)list).CopyTo(array, arrayIndex);
        }

        public IEnumerator<BaseSegment> GetEnumerator()
        {
            return ((IEnumerable<BaseSegment>)list).GetEnumerator();
        }

        public int IndexOf(BaseSegment item)
        {
            return ((IList<BaseSegment>)list).IndexOf(item);
        }

        public void Insert(int index, BaseSegment item)
        {
            ((IList<BaseSegment>)list).Insert(index, item);
        }

        public bool Remove(BaseSegment item)
        {
            return ((ICollection<BaseSegment>)list).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<BaseSegment>)list).RemoveAt(index);
        }

        internal void Update(FixablePointArray pointArray)
        {
            for (int i = 0; i < pointArray.Count - 1; i++)
            {
                Point p1 = (Point)pointArray[i];
                Point p2 = (Point)pointArray[i + 1];
                if (Math.Abs(p1.X - p2.X) < 3) // vertical
                {
                    list.Add(new SegmentV(p1.X, p1.Y, p2.Y));
                }
                else //horzontal
                {
                    list.Add(new SegmentH(p1.X, p2.X, p1.Y));
                }
            };
        }

        public void Update(SegmentH h)
        {
            int loc = list.IndexOf(h);

            if (loc > 1 && loc < list.Count)
            {
                BaseSegment b = list[loc - 1];
                switch (b)
                {
                    case SegmentH segmentH:
                        break;

                    case SegmentV segmentV:
                        segmentV.Y2 = h.Y;
                        break;
                }
            }

            if (loc > 1 && loc < list.Count - 1)
            {
                BaseSegment b = list[loc + 1];
                switch (b)
                {
                    case SegmentH segmentH:
                        break;

                    case SegmentV segmentV:
                        segmentV.Y1 = h.Y;
                        break;
                }
            }

            if(loc!=list.Count-1 && loc!=0)
                h.IsFixed = true;
        }

        public void Update(SegmentV v) // only move segments if greater than 1 and less than count-2
        {
            int loc = list.IndexOf(v);

            if (loc > 1 && loc < list.Count-1)
            {
                BaseSegment b = list[loc - 1];
                switch (b)
                {
                    case SegmentH segmentH:
                        segmentH.X2 = v.X;
                        break;
                }

                BaseSegment b2 = list[loc + 1];
                switch (b2)
                {
                    case SegmentH segmentH:
                        segmentH.X1 = v.X;
                        break;
                }
            }

            if (loc != list.Count - 1 && loc != 0)
                v.IsFixed = true;
        }

        internal SegmentArray Clone()
        {
            SegmentArray segments = new SegmentArray();
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i])
                {
                    case SegmentH segH:
                        segments.Add(segH.Clone());
                        break;

                    case SegmentV segV:
                        segments.Add(segV.Clone());
                        break;
                }
            }
            return segments;
        }

        internal int Length()
        {
            int length = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[0] is SegmentH sh)
                    length += Math.Abs(sh.X2 - sh.X1);
                if (list[0] is SegmentV sv)
                    length += Math.Abs(sv.Y2 - sv.Y1);
            }
            return length;
        }

        internal int Width()
        {
            int width = Math.Abs(End.Value.X - Start.Value.X);
            return width;
        }

        internal int Height()
        {
            int height = Math.Abs(End.Value.Y - Start.Value.Y);
            return height;
        }

        internal void Move(int deltaX, int deltaY)
        {
            for (int i = 0; i < list.Count; i++)
            {
            };
        }

        internal void AddPoint(Point point)
        {
            // list.Add(new SegmentH(point.x))
        }

        internal void SetStart(Point point)
        {
            switch (list[0])
            {
                case SegmentH h:
                    h.X1 = point.X;
                    h.Y = point.Y;
                    break;

                case SegmentV v:
                    v.X = point.X;
                    v.Y1 = point.Y;
                    break;
            }
        }

        internal void SetEnd(Point point)
        {
            switch (list[^1])
            {
                case SegmentH h:
                    h.X2 = point.X;
                    h.Y = point.Y;
                    break;

                case SegmentV v:
                    v.X = point.X;
                    v.Y2 = point.Y;
                    break;
            }
        }


        internal Point[] PointArray
        {
            get
            {
                List<Point> points = new();

                for (int i = 0; i < list.Count - 1; i++)
                {
                    switch (list[i])
                    {
                        case SegmentH sh:
                            points.Add(sh.Point1);
                            break;

                        case SegmentV sv:
                            points.Add(sv.Point1);
                            break;
                    }
                }

                if (points.Count != 0)
                    switch (list[^1])
                    {
                        case SegmentH sh:
                            points.Add(sh.Point1);
                            points.Add(sh.Point2);
                            break;

                        case SegmentV sv:
                            points.Add(sv.Point1);
                            points.Add(sv.Point2);
                            break;
                    }
                return points.ToArray();
            }
        }

        public bool Reset { get; set; } = false;
    }

    [TypeConverter(typeof(SegmentArray)), Description("ExpandToSeeComponent")]
    public class SegmentArrayExpander : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(SegmentArray);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is SegmentArray bc)
                return bc.List.ToArray();
            return null;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(SegmentArray);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is SegmentArray bc)
                return bc.List;
            return null;
        }
    }
}