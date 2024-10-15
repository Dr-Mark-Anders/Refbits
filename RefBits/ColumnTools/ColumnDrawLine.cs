using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    ///<summary>
    ///Linegraphicobject
    ///</summary>
    internal class ColumnDrawLine : Units.DrawObject
    {
        private Point startPoint;
        private Point endPoint;

        private const string entryStart = "Start";
        private const string entryEnd = "End";

        ///<summary>
        ///Graphicobjectsforhittest
        ///</summary>
        private GraphicsPath areaPath = null;

        private Pen areaPen = null;
        private Region areaRegion = null;

        public ColumnDrawLine() : this(0, 0, 1, 0)
        {
        }

        public ColumnDrawLine(int x1, int y1, int x2, int y2) : base()
        {
            startPoint.X = x1;
            startPoint.Y = y1;
            endPoint.X = x2;
            endPoint.Y = y2;

            Initialize();
        }

        public ColumnDrawLine(Point p1, Point p2)
        : base()
        {
            startPoint.X = p1.X;
            startPoint.Y = p1.Y;
            endPoint.X = p2.X;
            endPoint.Y = p2.Y;

            Initialize();
        }

        ///<summary>
        ///Clonethisinstance
        ///</summary>
        public override DrawObject Clone()
        {
            ColumnDrawLine drawLine = new();
            drawLine.startPoint = this.startPoint;
            drawLine.endPoint = this.endPoint;

            FillDrawObjectFields(drawLine);
            return drawLine;
        }

        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new(StreamColor, PenWidth);
            g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);

            pen.Dispose();
        }

        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }

        ///<summary>
        ///Gethandlepoint by1-basednumber
        ///</summary>
        ///<paramname="handleNumber"></param>
        ///<return  s></return  s>
        public override Point GetHandleLocation(int handleNumber)
        {
            if (handleNumber == 1)
                return startPoint;
            else
                return endPoint;
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            CreateObjects();

            return AreaRegion.IsVisible(rectangle);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;

                default:
                    return Cursors.Default;
            }
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber == 1)
                startPoint = point;
            else
                endPoint = point;

            Invalidate();
        }

        public override void Move(int deltaX, int deltaY)
        {
            startPoint.X += deltaX;
            startPoint.Y += deltaY;

            endPoint.X += deltaX;
            endPoint.Y += deltaY;

            Invalidate();
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, StreamingContext context)
        {
            info.AddValue(
            String.Format(CultureInfo.InvariantCulture,
            "{0}{1}",
            entryStart, context),
            startPoint);

            info.AddValue(
            String.Format(CultureInfo.InvariantCulture,
            "{0}{1}",
            entryEnd, context),
            endPoint);

            base.GetObjectData(info, context);
        }

        public ColumnDrawLine(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            startPoint = (Point)info.GetValue(
            String.Format(CultureInfo.InvariantCulture,
            "{0}{1}",
            entryStart, context),
            typeof(Point));

            endPoint = (Point)info.GetValue(
            String.Format(CultureInfo.InvariantCulture,
            "{0}{1}",
            entryEnd, context),
            typeof(Point));
        }

        ///<summary>
        ///Invalidateobject.
        ///Whenobjectisinvalidated,pathusedforhittest
        ///isreleasedandshouldbecreatedagain.
        ///</summary>
        protected void Invalidate()
        {
            if (AreaPath != null)
            {
                AreaPath.Dispose();
                AreaPath = null;
            }

            if (AreaPen != null)
            {
                AreaPen.Dispose();
                AreaPen = null;
            }

            if (AreaRegion != null)
            {
                AreaRegion.Dispose();
                AreaRegion = null;
            }
        }

        ///<summary>
        ///Creategraphicobjectsusedfromhittest.
        ///</summary>
        protected virtual void CreateObjects()
        {
            if (AreaPath != null)
                return;

            //Createpathwhichcontainswideline
            //foreasymouseselection
            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.Black, 7);
            AreaPath.AddLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            AreaPath.Widen(AreaPen);

            //Createregionfromthepath
            AreaRegion = new Region(AreaPath);
        }

        protected GraphicsPath AreaPath
        {
            get
            {
                return areaPath;
            }
            set
            {
                areaPath = value;
            }
        }

        protected Pen AreaPen
        {
            get
            {
                return areaPen;
            }
            set
            {
                areaPen = value;
            }
        }

        protected Region AreaRegion
        {
            get
            {
                return areaRegion;
            }
            set
            {
                areaRegion = value;
            }
        }
    }
}