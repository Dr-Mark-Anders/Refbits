#region using   directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;

#endregion

namespace Units
{
    using PointEnumerator = IEnumerator<Point>;
    using PointList = List<Point>;

    /// <summary>
    /// Polygon graphic object
    /// </summary>
    internal class DrawPolygon : Units.DrawLine
    {
        private PointList pointArray;         // list of point s

        private static Cursor handleCursor = new Cursor(typeof(DrawPolygon), "PolyHandle.cur");

        private const string entryLength = "Length";
        private const string entryPoint = "Point ";

        public DrawPolygon() : base()
        {
            pointArray = new PointList();

            Initialize();
        }

        public DrawPolygon(int x1, int y1, int x2, int y2) : base()
        {
            pointArray = new PointList();
            pointArray.Add(new Point(x1, y1));
            pointArray.Add(new Point(x2, y2));

            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawPolygon drawPolygon = new DrawPolygon();

            foreach (Point p in this.pointArray)
            {
                drawPolygon.pointArray.Add(p);
            }

            FillDrawObjectFields(drawPolygon);
            return drawPolygon;
        }

        public override void Draw(Graphics g)
        {
            int x1 = 0, y1 = 0;     // previous point
            int x2, y2;             // current point

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(StreamColor, PenWidth);

            PointEnumerator enumerator = pointArray.GetEnumerator();

            if (enumerator.MoveNext())
            {
                x1 = ((Point)enumerator.Current).X;
                y1 = ((Point)enumerator.Current).Y;
            }

            while (enumerator.MoveNext())
            {
                x2 = ((Point)enumerator.Current).X;
                y2 = ((Point)enumerator.Current).Y;

                g.DrawLine(pen, x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            pen.Dispose();
        }

        public void AddPoint(Point point)
        {
            pointArray.Add(point);
        }

        public override int HandleCount
        {
            get
            {
                return pointArray.Count;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, context),
                pointArray.Count);

            int i = 0;
            foreach (Point p in pointArray)
            {
                info.AddValue(
                    String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}-{2}",
                    entryPoint, context, i++),
                    p);
            }

            base.GetObjectData(info, context);  // ??
        }

        public DrawPolygon(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Point point;
            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, context));

            for (int i = 0; i < n; i++)
            {
                point = (Point)info.GetValue(
                    String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}-{2}",
                    entryPoint, context, i),
                    typeof(Point));

                pointArray.Add(point);
            }
        }

        /// <summary>
        /// Create graphic object used for hit test
        /// </summary>
        protected override void CreateObjects()
        {
            if (AreaPath != null)
                return;

            // Create closed path which contains all polygon vertexes
            AreaPath = new GraphicsPath();

            int x1 = 0, y1 = 0;     // previous point
            int x2, y2;             // current point

            PointEnumerator enumerator = pointArray.GetEnumerator();

            if (enumerator.MoveNext())
            {
                x1 = ((Point)enumerator.Current).X;
                y1 = ((Point)enumerator.Current).Y;
            }

            while (enumerator.MoveNext())
            {
                x2 = ((Point)enumerator.Current).X;
                y2 = ((Point)enumerator.Current).Y;

                AreaPath.AddLine(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            AreaPath.CloseFigure();

            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }
    }
}