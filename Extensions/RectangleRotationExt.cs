using System.Drawing;
using System.Drawing.Drawing2D;

namespace Extensions
{
    public static class RectExtensions
    {
        public static Rectangle Rotate(this Rectangle r, int angle)
        {
            using (Matrix matrix = new Matrix())
            {
                matrix.RotateAt((int)angle, r.Center());
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddRectangle(r);
                    path.Transform(matrix);
                    return Rectangle.Round(path.GetBounds());
                }
            }
        }

        public static Point TopCentre(this Rectangle r)
        {
            return new Point(r.X + r.Width / 2, r.Y);
        }

        public static Point Centre(this Rectangle r)
        {
            return new Point(r.X + r.Width / 2, r.Y + r.Height / 2);
        }

        public static Point LeftMiddle(this Rectangle r)
        {
            return new Point(r.Left, r.Top + r.Height / 2);
        }

        public static Point RightMiddle(this Rectangle r)
        {
            return new Point(r.Right, r.Top + r.Height / 2);
        }

        public static Point Height(this Rectangle r)
        {
            return new Point(r.Height);
        }

        public static Rectangle Centered(this Rectangle r)
        {
            Rectangle p = new Rectangle();
            p.X = r.Left - 5; // r.Width / 2;
            p.Y = r.Top - 5;  // r.Height / 2;
            p.Height = r.Height;
            p.Width = r.Width;
            return p;
        }

        public static void ReLocate(this Rectangle r, Point p)
        {
            r.X = p.X - r.Width / 2;
            r.Y = p.Y - r.Height / 2;
        }

        public static Point TopLeft(this Rectangle r)
        {
            Point p = new Point(r.Left, r.Top);
            return p;
        }

        public static Point BottomLeft(this Rectangle r)
        {
            Point p = new Point(r.Left, r.Top + r.Height);
            return p;
        }

        public static Point TopRight(this Rectangle r)
        {
            Point p = new Point(r.Left + r.Width, r.Top);
            return p;
        }

        public static Point TopRight(this Rectangle r, int dx, int dy)
        {
            Point p = new Point(r.Left + r.Width + dx, r.Top + dy);
            return p;
        }

        public static Point BottomRight(this Rectangle r)
        {
            Point p = new Point(r.Left + r.Width, r.Top + r.Height);
            return p;
        }
    }
}