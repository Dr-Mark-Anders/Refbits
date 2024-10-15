using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Serialization;

namespace Main.DrawingObjects.Streams
{
    public enum XYFixed {Y, X, Both, None }

    [TypeConverter(typeof(FixablePoint)), Description("ExpandToSeeComponent")]
    public class FixablePointExpander : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType==typeof(FixablePoint);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is FixablePoint bc)
                return bc.Point;
            return null;
        }
    }



    [Serializable, TypeConverter(typeof(FixablePoint))]
    public class FixablePoint : ExpandableObjectConverter, ISerializable
    {
        private Point point = new Point();
        XYFixed xyfixed = XYFixed.None;

        public override string ToString()
        {
            return point.ToString() + " " + XYFixed.ToString();
        }

        public FixablePoint(Point point, XYFixed xyfixed=XYFixed.None)
        {
            int x = point.X;
            int y = point.Y;
            switch (this.XYFixed)
            {
                case XYFixed.Y:
                    point.X = x;
                    break;
                case XYFixed.X:
                    point.Y = y;
                    break;
                case XYFixed.Both:
                    break;
                case XYFixed.None:
                    point.X = x;
                    point.Y = y;
                    break;
                default:
                    break;
            }
            this.XYFixed = xyfixed;
        }

        public FixablePoint(int x, int y, XYFixed xyfixed = XYFixed.None)
        {
            switch (this.XYFixed)
            {
                case XYFixed.Y:
                    point.X = x;
                    break;
                case XYFixed.X:
                    point.Y = y;
                    break;
                case XYFixed.Both:
                    //point.X = x;
                    //point.Y = y;
                    break;
                case XYFixed.None:
                    point.X = x;
                    point.Y = y;
                    break;
                default:
                    break;
            }
            this.XYFixed = xyfixed;
        }

        public XYFixed XYFixed
        {
            get
            {
                return xyfixed; 
            }
            set
            {
                switch (value)
                {
                    case XYFixed.Y:
                        break;
                    case XYFixed.X:
                        break;
                    case XYFixed.Both:
                        break;
                    case XYFixed.None:
                        break;
                    default:
                        break;
                }
                xyfixed = value;
            }
        }

        public int X { get => point.X; set => point = new Point(value, point.Y); }
        public int Y { get => point.Y; set => point = new Point(point.X, value); }
        public Point Point { get => point; set => point = value; }

        public static implicit operator FixablePoint(Point v)
        {
            FixablePoint fp = new(v.X, v.Y);
            return fp;
        }

        public static implicit operator Point(FixablePoint v)
        {
            return v.point;
        }

        public static implicit operator PointF(FixablePoint v)
        {
            return v.point;
        }

        public FixablePoint(SerializationInfo info, StreamingContext context)
        {
            point = (Point)info.GetValue("point", typeof(Point));
            try
            {
                XYFixed = (XYFixed)info.GetValue("XY", typeof(XYFixed));
            }
            catch { }
        }

        public FixablePoint()
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("point", point);
            info.AddValue("XY", XYFixed);
        }

        internal void Update(Point point)
        {
            int x = point.X;
            int y = point.Y;

            switch (this.XYFixed)
            {
                case XYFixed.Y:
                    point.X = x;
                    break;
                case XYFixed.X:
                    point.Y = y;
                    break;
                case XYFixed.Both:
                    //point.X = x;
                    //point.Y = y;
                    break;
                case XYFixed.None:
                    point.X = x;
                    point.Y = y;
                    break;
                default:
                    break;
            }
        }

        internal void Update(int x, int y)
        {
            switch (this.XYFixed)
            {
                case XYFixed.Y:
                    point.X = x;
                    break;
                case XYFixed.X:
                    point.Y = y;
                    break;
                case XYFixed.Both:
                    //point.X = x;
                    //point.Y = y;
                    break;
                case XYFixed.None:
                    point.X = x;
                    point.Y = y;
                    break;
                default:
                    break;
            }
         }
    }

    [TypeConverter(typeof(FixablePointArrayExpander)), Description("ExpandToSeeComponent")]
    public class FixablePointArrayExpander : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(FixablePointArray))
                return true;
            else
                return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is FixablePointArray bc)
                return bc.ToArray();
            return null;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(FixablePointArray);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is FixablePointArray bc)
                return bc.Points;
            return null;
        }
    }

    [Serializable, TypeConverter(typeof(FixablePointArrayExpander))]
    public class FixablePointArray : ExpandableObjectConverter, IList<FixablePoint>, ISerializable
    {
        private List<FixablePoint> points = new List<FixablePoint>();

        public static implicit operator FixablePointArray(List<Point> v)
        {
            FixablePointArray p = new();

            for (int i = 0; i < v.Count; i++)
            {
                p.Add(new FixablePoint(v[i]));
            }
            return p;
        }

        public FixablePoint this[int index]
        {
            get 
            {
                return points[index]; 
            }
            set
            {
                points[index] = value;
            }
        }

        public int Count => ((ICollection<FixablePoint>)points).Count;

        public bool IsReadOnly => ((ICollection<FixablePoint>)points).IsReadOnly;

        public List<FixablePoint> Points { get => points; set => points = value; }

        public void Add(FixablePoint item)
        {
            ((ICollection<FixablePoint>)points).Add(item);
        }

        public void Clear()
        {
            ((ICollection<FixablePoint>)points).Clear();
        }

        public bool Contains(FixablePoint item)
        {
            return ((ICollection<FixablePoint>)points).Contains(item);
        }

        public void CopyTo(FixablePoint[] array, int arrayIndex)
        {
            ((ICollection<FixablePoint>)points).CopyTo(array, arrayIndex);
        }

        public IEnumerator<FixablePoint> GetEnumerator()
        {
            return ((IEnumerable<FixablePoint>)points).GetEnumerator();
        }

        public int IndexOf(FixablePoint item)
        {
            return ((IList<FixablePoint>)points).IndexOf(item);
        }

        public void Insert(int index, FixablePoint item)
        {
            ((IList<FixablePoint>)points).Insert(index, item);
        }

        public bool Remove(FixablePoint item)
        {
            return ((ICollection<FixablePoint>)points).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<FixablePoint>)points).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)points).GetEnumerator();
        }

        public PointF[] ToArray()
        {
            PointF[] p = new PointF[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                p[i] = (PointF)points[i];
            }
            return p;
        }

        public FixablePointArray(SerializationInfo info, StreamingContext context)
        {
            points = (List<FixablePoint>)info.GetValue("Points", typeof(List<FixablePoint>));
        }

        public FixablePointArray()
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Points", points);
        }

        internal void UnFreeze()
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].XYFixed=XYFixed.None;
            };
        }
    }
}
