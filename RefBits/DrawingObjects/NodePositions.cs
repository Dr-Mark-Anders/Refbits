using Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units;

namespace Units
{
    public partial class Node
    {
        private PointF relObjectPosition = new PointF();
        private Point absoluteposition = new Point();

        internal void SetAbsoluteLocation(int X, int Y)
        {
            absoluteposition.X = X;
            absoluteposition.Y = Y;
        }

        public Point SetAbsolutePosition { set => absoluteposition = value; }

        public float X
        {
            get
            {
                return RotatedFlipped.X;
            }
        }

        public float Y
        {
            get
            {
                return RotatedFlipped.Y;
            }
        }

        public Rectangle Absolute
        {
            get
            {
                Point p = new Point();
                Rectangle d = new Rectangle();

                switch (owner)
                {
                    case DrawTray dt:
                        d = dt.Location;
                        p.X = d.X + (int)(d.Width * Relative.X);
                        p.Y = d.Y + (int)(d.Height * Relative.Y);
                        break;

                    case DrawArea da:
                        p.X = (int)Relative.X;
                        p.Y = (int)Relative.Y;
                        break;

                    case DrawColumnTraySection dcst:
                        d = dcst.Location;
                        p.X = d.X + (int)(d.Width * Relative.X);
                        p.Y = d.Y + (int)(d.Height * Relative.Y);
                        break;

                    case DrawRectangle dr:
                        d = dr.RotatedRectangle;
                        p.X = d.X + (int)(d.Width * RotatedFlipped.X);
                        p.Y = d.Y + (int)(d.Height * RotatedFlipped.Y);
                        break;

                    case DrawMaterialStream dms:
                        p = dms.LineCenter(out bool? H);
                        break;

                    default:
                        p = absoluteposition;
                        break;
                }

                if (p.X > 10000)
                    p.X = 100;
                if (p.Y > 10000)
                    p.Y = 100;
                return new Rectangle(p, new Size(9, 9));
            }
        }

        public PointF Relative
        {
            get { return relObjectPosition; }

            set { relObjectPosition = value; }
        }

        public PointF RotatedFlipped
        {
            get
            {
                PointF rotpos = new PointF();

                DrawRectangle dr = null;
                switch (owner)
                {
                    case DrawRectangle:

                        dr = (DrawRectangle)owner;
                        switch (dr.Angle)
                        {
                            case enumRotationAngle.a0:
                                rotpos.X = relObjectPosition.X;
                                rotpos.Y = relObjectPosition.Y;
                                break;

                            case enumRotationAngle.a90:
                                rotpos.X = 1 - relObjectPosition.Y;
                                rotpos.Y = relObjectPosition.X;
                                break;

                            case enumRotationAngle.a180:
                                rotpos.X = 1 - relObjectPosition.X;
                                rotpos.Y = 1 - relObjectPosition.Y;
                                break;

                            case enumRotationAngle.a270:
                                rotpos.X = relObjectPosition.Y;
                                rotpos.Y = 1 - relObjectPosition.X;
                                break;
                        }

                        if (dr.FlipHorizontal)
                            rotpos.X = 1 - rotpos.X;

                        if (dr.FlipVertical)
                            rotpos.Y = 1 - rotpos.Y;

                        break;

                    case DrawArea:
                        rotpos.X = relObjectPosition.X;
                        rotpos.Y = relObjectPosition.Y;
                        break;
                }

                return rotpos;
            }
            set
            {
                relObjectPosition = value;
            }
        }

        public PointF RotatePoint(int angle, Point pt)
        {
            var a = angle * Math.PI / 180.0;
            float cosa = (float)Math.Cos(a), sina = (float)Math.Sin(a);
            return new PointF(pt.X * cosa - pt.Y * sina, pt.X * sina + pt.Y * cosa);
        }

        public Rectangle Centered()
        {
            return Absolute.Centered();
        }

        public Point CenterPoint()
        {
            return Absolute.Centered().Center();
        }
    }
}
