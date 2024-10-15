using Extensions;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Main.Images
{
    internal class Images
    {
        private static readonly Color col2 = Color.White;
        private static readonly Color col1 = Color.Gray;
        private static readonly Color StreamColor = Color.Black;
        private static readonly int PenWidth = 1;

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <return  s>The resized image.</return  s>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            if(image is null)
                return null;
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }

        internal static Image Mixer()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new();
            Pen pen = new(Color.Black, PenWidth);

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Point[] p = new Point[4];
            p[0].X = rectangle.TopLeft().X;
            p[0].Y = rectangle.TopLeft().Y;
            p[1].X = rectangle.TopRight().X;
            p[1].Y = (rectangle.TopLeft().Y + rectangle.BottomLeft().Y) / 2;
            p[2].X = rectangle.BottomLeft().X;
            p[2].Y = rectangle.BottomLeft().Y;
            p[3].X = rectangle.TopLeft().X;
            p[3].Y = rectangle.TopLeft().Y;

            LinearGradientBrush BodyBrush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);

            gp.AddLines(p);
            gp.CloseAllFigures();

            BodyBrush.SetBlendTriangularShape(1f);

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);
            return bitmap;
        }

        public static Bitmap ThreePhaseSep()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new(Color.Black, 1);
            Color col2 = Color.White;
            Color col1 = Color.Gray;

            int rhoff = Convert.ToInt32(0.43 * rectangle.Width);
            int r = rectangle.Width - rhoff;
            int ArcWidth = Convert.ToInt32(rectangle.Height / 4);

            GraphicsPath gp = new();
            RectangleF ColumnBody = new(rectangle.Left + ArcWidth / 2, rectangle.Top, rectangle.Width - ArcWidth, rectangle.Height / 2);

            gp.AddArc(rectangle.Right - ArcWidth, rectangle.Top, ArcWidth, rectangle.Height / 2, 270, 180);
            gp.AddRectangle(ColumnBody);
            gp.AddArc(rectangle.Left, rectangle.Top, ArcWidth, rectangle.Height / 2, 90, 180);

            Rectangle sump = new(rectangle.Left + rectangle.Width * 2 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Width / 4, rectangle.Height / 4);
            gp.AddRectangle(sump);
            GraphicsPath gp2 = new();
            gp2.AddArc(sump.Left, sump.Bottom - rectangle.Height * 1 / 12, rectangle.Width / 4, rectangle.Height * 1 / 6, 0, 180);

            // gp.CloseAllFigures();

            LinearGradientBrush Brush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);

            g.FillPath(Brush, gp);
            g.DrawPath(pen, gp);

            g.FillPath(Brush, gp2);
            g.DrawPath(pen, gp2);

            return bitmap;
        }

        public static Bitmap AssayCutter()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color col2 = Color.White;
            Color col1 = Color.Gray;
            Rectangle R = rectangle;

            Pen pen = new(Color.Black, 2);

            int rhoff = Convert.ToInt32(0.43 * R.Width);
            int r = R.Width - rhoff;
            int right = R.TopRight().X;
            int l = 0;
            int w = r - l;
            int ArcHeight = Convert.ToInt32(w / 2);
            int t = R.Top;
            int b = R.Bottom;
            int h = b - t;

            g.DrawArc(pen, R.TopLeft().X, R.TopLeft().Y, R.Width, ArcHeight, 180, 180);
            g.DrawLine(pen, R.TopLeft().X, R.TopLeft().Y + ArcHeight / 2, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight / 2);
            g.DrawLine(pen, R.TopRight().X, R.TopRight().Y + ArcHeight / 2, R.BottomRight().X, R.BottomRight().Y - ArcHeight / 2);
            g.DrawArc(pen, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight, rectangle.Width, ArcHeight, 0, 180);

            Pen p = new(Color.Black, 1);
            LinearGradientBrush BodyBrush = new(new PointF(rectangle.Left, 0), new PointF(right, 0), col2, col1);
            BodyBrush.SetBlendTriangularShape(1f);
            g.SmoothingMode = SmoothingMode.HighSpeed;

            GraphicsPath Body = new();
            RectangleF ColumnBody = new(rectangle.Left, t + ArcHeight / 2, rectangle.Width, h - ArcHeight);
            Body.AddRectangle(ColumnBody);
            g.DrawPath(p, Body);
            g.FillPath(BodyBrush, Body);

            GraphicsPath Top = new();
            Top.AddArc(rectangle.Left, t, rectangle.Width, ArcHeight, 180, 180);
            PathGradientBrush TopBrush = new(Top);
            TopBrush.CenterPoint = new PointF(rectangle.Left, t + ArcHeight / 2);
            TopBrush.CenterColor = col2;
            TopBrush.SurroundColors = new Color[] { col1 };
            g.DrawPath(p, Top);
            g.FillPath(TopBrush, Top);

            GraphicsPath Bottom = new();
            Bottom.AddArc(rectangle.Left, b - ArcHeight, rectangle.Width, ArcHeight, 0, 180);
            Bottom.CloseAllFigures();

            PathGradientBrush BotBrush = new(Bottom);
            //BotBrush.SetSigmaBellShape(1f,1f);
            BotBrush.CenterPoint = new PointF(rectangle.Left, b - ArcHeight);
            BotBrush.CenterColor = col2;
            BotBrush.SurroundColors = new Color[] { col1 };
            g.DrawPath(p, Bottom);
            g.FillPath(BotBrush, Bottom);

            return bitmap;
        }

        public static Bitmap AssayFeed()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            Color col1 = Color.Gray, col2 = Color.White;

            Color FrameColor = Color.Black;
            Color LineColor = Color.Blue;

            float t = Convert.ToInt32(rectangle.Top);
            float l = Convert.ToInt32(rectangle.Left);
            float w = rectangle.Width;
            float h = rectangle.Height;
            float r = rectangle.Right;
            float b = rectangle.Bottom;

            Pen p = new(FrameColor, 1);
            GraphicsPath gp = new(FillMode.Winding);

            LinearGradientBrush BodyBrush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);
            BodyBrush.SetBlendTriangularShape(1f, 1f);
            g.SmoothingMode = SmoothingMode.HighSpeed;

            RectangleF ColumnBody = new(l, t, w, h);
            gp.AddRectangle(ColumnBody);

            g.FillPath(BodyBrush, gp);
            g.DrawPath(p, gp);

            RectangleF rt = new(l - w / 10f, t - 2f, w + w / 5f, 3);
            p.Color = Color.Black;
            gp.AddRectangle(rt);
            gp.CloseFigure();
            g.FillPath(Brushes.Black, gp);

            rt.Y = (t + b) / 2 - rt.Height / 2f;
            p.Color = Color.Black;
            gp.AddRectangle(rt);
            gp.CloseFigure();
            g.FillPath(Brushes.Black, gp);

            rt.Y = b - rt.Height;
            p.Color = Color.Black;
            gp.AddRectangle(rt);
            gp.CloseFigure();
            g.FillPath(Brushes.Black, gp);

            return bitmap;
        }

        public static Bitmap AssayImportCSV()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            float t = Convert.ToInt32(rectangle.Top);
            float l = Convert.ToInt32(rectangle.Left);
            float w = rectangle.Width;
            float h = rectangle.Height;
            float r = rectangle.Right;
            float b = rectangle.Bottom;

            Pen p = new(Color.Black, 1);
            GraphicsPath gp = new(FillMode.Winding);

            LinearGradientBrush BodyBrush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);
            BodyBrush.SetBlendTriangularShape(1f, 1f);
            g.SmoothingMode = SmoothingMode.HighSpeed;

            RectangleF ColumnBody = new(l, t, w, h);
            gp.AddRectangle(ColumnBody);

            RectangleF rt = new(l - w / 10f, t - 2f, w + w / 5f, 3);
            p.Color = Color.Black;
            gp.AddRectangle(rt);
            gp.CloseFigure();

            rt.Y = (t + b) / 2 - rt.Height / 2f;
            p.Color = Color.Black;
            gp.AddRectangle(rt);
            gp.CloseFigure();

            rt.Y = b - rt.Height;
            p.Color = Color.Black;
            gp.AddRectangle(rt);
            gp.CloseFigure();

            g.FillPath(BodyBrush, gp);
            g.DrawPath(p, gp);

            return bitmap;
        }

        public static Bitmap Blender()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);
            LinearGradientBrush BodyBrush = new(new PointF(r.Left, 0), new PointF(r.Right, 0), col2, col1);

            Pen pen = new(Color.Black, PenWidth);
            GraphicsPath gp = new();

            int rhoff = Convert.ToInt32(0.43 * r.Width);
            int ArcHeight = Convert.ToInt32(r.Width / 2);

            gp.AddArc(r.TopLeft().X, r.TopLeft().Y, r.Width, ArcHeight, 180, 180);
            gp.AddLine(r.TopRight().X, r.TopRight().Y + ArcHeight / 2, r.BottomRight().X, r.BottomRight().Y - ArcHeight / 2);
            gp.AddArc(r.BottomLeft().X, r.BottomLeft().Y - ArcHeight, r.Width, ArcHeight, 0, 180);
            gp.AddLine(r.TopLeft().X, r.TopLeft().Y + ArcHeight / 2, r.BottomLeft().X, r.BottomLeft().Y - ArcHeight / 2);

            gp.CloseFigure();

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            g.DrawLine(pen, r.TopCentre().X, r.TopCentre().Y, r.Center().X, r.Center().Y);

            int paddlesize = 20;
            pen.Width = 2;
            g.DrawEllipse(pen, r.Center().X - paddlesize, r.Center().Y - paddlesize / 2, paddlesize, paddlesize);
            g.DrawEllipse(pen, r.Center().X, r.Center().Y - paddlesize / 2, paddlesize, paddlesize);

            //BodyBrush = new  LinearGradientBrush(new  PointF(r.Left, 0), new  PointF(r.Right, 0), Color.Black, Color.Black);

            g.FillEllipse(BodyBrush, r.Center().X - paddlesize, r.Center().Y - paddlesize / 2, paddlesize, paddlesize);
            g.FillEllipse(BodyBrush, r.Center().X, r.Center().Y - paddlesize / 2, paddlesize, paddlesize);

            return bitmap;
        }

        public static Bitmap CaseStudy()
        {
            Bitmap bitmap = new(100, 100);

            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Coker()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Column()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new(FillMode.Winding);

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Pen pen = new(Color.Black, 2);

            int rhoff = Convert.ToInt32(0.43 * rectangle.Width);
            int r = rectangle.Width - rhoff;
            int right = rectangle.TopRight().X;
            int ArcHeight = Convert.ToInt32(rectangle.Width / 2);
            int t = rectangle.Top;
            int b = rectangle.Bottom;
            int h = b - t;

            Pen p = new(Color.Black, 1);

            LinearGradientBrush BodyBrush = new(new PointF(rectangle.Left, 0),
                    new PointF(rectangle.Right, 0), col2, col1);

            BodyBrush.SetBlendTriangularShape(1f);

            Rectangle ColumnBody = new(rectangle.Left, t + ArcHeight / 2, rectangle.Width, h - ArcHeight);
            gp.AddRectangle(ColumnBody);
            gp.CloseFigure();

            gp.AddArc(rectangle.Left, t, rectangle.Width, ArcHeight, 180, 180);
            gp.CloseFigure();

            gp.AddArc(rectangle.Left, b - ArcHeight - 1, rectangle.Width, ArcHeight, 0, 180);

            gp.AddArc(rectangle.TopLeft().X, rectangle.TopLeft().Y, rectangle.Width, ArcHeight, 180, 180);
            gp.CloseFigure();
            gp.AddLine(rectangle.TopLeft().X, rectangle.TopLeft().Y + ArcHeight / 2, rectangle.BottomLeft().X, rectangle.BottomLeft().Y - ArcHeight / 2);
            gp.CloseFigure();
            gp.AddLine(rectangle.TopRight().X, rectangle.TopRight().Y + ArcHeight / 2, rectangle.BottomRight().X, rectangle.BottomRight().Y - ArcHeight / 2);
            gp.CloseFigure();
            gp.AddArc(rectangle.BottomLeft().X, rectangle.BottomLeft().Y - ArcHeight, rectangle.Width, ArcHeight, 0, 180);
            gp.CloseFigure();

            // draw lines
            for (int i = 1; i < 5; i++)
            {
                int Height = ColumnBody.Height / 5 * i;
                gp.AddLine(ColumnBody.TopLeft().X, ColumnBody.TopLeft().Y + Height, ColumnBody.TopRight().X, ColumnBody.TopRight().Y + Height);
                gp.CloseFigure();
            }

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            return bitmap;
        }

        public static Bitmap ColumnCondenser()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap ColumnPA()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Reboiler()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap TraySection()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Compressor()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new(FillMode.Winding);

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Pen pen = new(StreamColor, PenWidth);
            LinearGradientBrush BodyBrush = new(new PointF(r.Left, 0), new PointF(r.Right, 0), col2, col1);

            Point[] points = new Point[4];

            points[0] = r.TopLeft();
            points[0].X = r.TopLeft().X + (int)(r.Width * 0.25);

            points[1] = r.TopRight();
            points[1].Offset(0, r.Height().X / 4);

            points[2] = r.BottomRight();
            points[2].Offset(0, -r.Height().X / 4);

            points[3].X = r.BottomLeft().X + (int)(r.Width * 0.25);
            points[3].Y = r.BottomLeft().Y;

            gp.AddRectangle(new RectangleF(r.BottomLeft().X, r.Centre().Y - 2, (int)(r.Width * 0.25), 5));
            gp.AddPolygon(points);
            gp.CloseAllFigures();

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            return bitmap;
        }

        public static Bitmap CompSplitter()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new(FillMode.Winding);

            Pen pen = new(StreamColor, PenWidth);

            int rhoff = Convert.ToInt32(0.43 * r.Width);
            int ArcHeight = Convert.ToInt32(r.Width / 2);
            int AH2 = ArcHeight / 2;

            //gp.AddLine(R.TopLeft().X, (R.TopLeft().Y + R.BottomLeft().Y) / 2, R.BottomRight().X, (R.TopLeft().Y + R.BottomLeft().Y) / 2);
            //gp.AddLine(R.BottomRight().X, R.BottomRight().Y, R.TopRight().X, R.TopRight().Y);

            Point[] p = new Point[4];
            p[0].X = r.TopLeft().X;
            p[0].Y = r.TopLeft().Y;
            p[1].X = r.TopRight().X;
            p[1].Y = r.TopRight().Y;
            p[2].X = r.BottomRight().X;
            p[2].Y = r.BottomRight().Y;
            p[3].X = r.BottomLeft().X;
            p[3].Y = r.BottomLeft().Y;

            gp.AddLines(p);
            gp.CloseAllFigures();

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            LinearGradientBrush BodyBrush = new(new PointF(r.Left, 0), new PointF(r.Right, 0), col2, col1);

            BodyBrush.SetBlendTriangularShape(1f);
            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            return bitmap;
        }

        public static Bitmap Cooler()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new(FillMode.Winding);

            Pen pen = new(StreamColor, PenWidth);
            pen.Color = Color.White;

            gp.AddEllipse(rectangle);

            pen.Color = Color.Black;

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            gp.AddEllipse(rectangle);
            gp.AddLine(rectangle.Left, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2);
            gp.AddLine(rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4);
            gp.AddLine(rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4);
            gp.AddLine(rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2);
            gp.AddLine(rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height / 2);

            LinearGradientBrush BodyBrush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            pen.Dispose();

            return bitmap;
        }

        public static Bitmap Divider()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new(FillMode.Winding);

            Pen pen = new(StreamColor, PenWidth);

            int rhoff = Convert.ToInt32(0.43 * r.Width);
            int ArcHeight = Convert.ToInt32(r.Width / 2);
            int AH2 = ArcHeight / 2;

            //gp.AddLine(R.TopLeft().X, (R.TopLeft().Y + R.BottomLeft().Y) / 2, R.BottomRight().X, (R.TopLeft().Y + R.BottomLeft().Y) / 2);
            //gp.AddLine(R.BottomRight().X, R.BottomRight().Y, R.TopRight().X, R.TopRight().Y);

            Point[] p = new Point[4];
            p[0].X = r.TopLeft().X;
            p[0].Y = (r.TopLeft().Y + r.BottomLeft().Y) / 2;
            p[1].X = r.TopRight().X;
            p[1].Y = r.TopRight().Y;
            p[2].X = r.BottomRight().X;
            p[2].Y = r.BottomRight().Y;
            p[3].X = r.TopLeft().X;
            p[3].Y = (r.TopLeft().Y + r.BottomLeft().Y) / 2;

            gp.AddLines(p);
            gp.CloseAllFigures();

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            LinearGradientBrush BodyBrush = new(new PointF(r.Left, 0),
                new PointF(r.Right, 0), col2, col1);

            BodyBrush.SetBlendTriangularShape(1f);

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            return bitmap;
        }

        public static Bitmap Ellipse()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Stream()
        {
            return Properties.Resources.Stream;
        }

        public static Bitmap Pipe()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;

            Color col2 = Color.White;
            Color col1 = Color.Gray;
            Pen pen = new(StreamColor, PenWidth);

            GraphicsPath gp = new(FillMode.Winding);

            Point[] Polygon = new Point[4];
            Polygon[0] = new Point(r.TopLeft().X, r.TopLeft().Y);
            Polygon[1] = new Point(r.BottomLeft().X, r.BottomLeft().Y);
            Polygon[2] = new Point(r.BottomRight().X, r.BottomRight().Y);
            Polygon[3] = new Point(r.TopRight().X, r.TopRight().Y);

            gp.AddPolygon(Polygon);

            PathGradientBrush BotBrush = new(gp);

            BotBrush.CenterPoint = new PointF((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);
            BotBrush.CenterColor = col2;
            BotBrush.SurroundColors = new Color[] { col1 };

            g.FillPath(BotBrush, gp);
            g.DrawPath(pen, gp);

            return bitmap;
        }

        public static Bitmap Expander()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new();
            gp.FillMode = FillMode.Winding;

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Pen pen = new(StreamColor, PenWidth);
            Point[] points = new Point[5];

            points[0] = r.TopLeft();
            points[0].Offset(0, r.Height().X / 4);

            points[1] = r.TopRight();
            points[1].Offset((int)(-r.Width * 0.25), 0);

            points[2] = r.BottomRight();
            points[2].Offset((int)(-r.Width * 0.25), 0);

            points[3] = r.BottomLeft();
            points[3].Offset(0, -r.Height().X / 4);

            points[4] = r.TopLeft();
            points[4].Offset(0, r.Height().X / 4);

            LinearGradientBrush BodyBrush = new(new PointF(r.Left, 0), new PointF(r.Right, 0), col2, col1);

            gp.AddRectangle(new RectangleF(r.BottomRight().X - (int)(r.Width * 0.25), r.Centre().Y - 2, (float)(r.Width * 0.25), 5));
            gp.AddPolygon(points);

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            return bitmap;
        }

        public static Bitmap FCC()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap GenericAssay()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            GraphicsPath gp = new(FillMode.Winding);
            Pen pen = new(StreamColor, PenWidth);

            LinearGradientBrush BodyBrush = new(new PointF(r.Left, 0), new PointF(r.Right, 0), col2, col1);
            BodyBrush.SetBlendTriangularShape(1f, 1f);
            g.SmoothingMode = SmoothingMode.HighSpeed;

            int padding = 10;
            RectangleF ColumnBody = new(r.Left + padding, r.Top, r.Width - padding * 2, r.Height);
            gp.AddRectangle(ColumnBody);

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            RectangleF rt = new(r.Left - r.Width / 10f, r.Top, r.Width + r.Width / 5f, 3);
            pen.Color = Color.Black;

            Brush brush = Brushes.Black;
            g.FillRectangle(brush, rt);

            rt.Y = (r.Top + r.Bottom) / 2 - rt.Height / 2f;
            pen.Color = Color.Black;
            g.FillRectangle(brush, rt);

            rt.Y = r.Bottom - rt.Height;
            pen.Color = Color.Black;
            g.FillRectangle(brush, rt);

            return bitmap;
        }

        public static Bitmap Adjust()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new(StreamColor, PenWidth);
            Color col2 = Color.White;
            Color col1 = Color.Gray;

            int rhoff = Convert.ToInt32(0.43 * rectangle.Width);
            int r = rectangle.Width - rhoff;
            int l = 0;
            int w = r - l;
            int ArcHeight = Convert.ToInt32(w / 2);
            int t = rectangle.Top;
            int b = rectangle.Bottom;
            int h = b - t;

            GraphicsPath gp = new();
            Rectangle ColumnBody = new(rectangle.Left, t + ArcHeight / 2, rectangle.Width, h - ArcHeight);

            gp.AddEllipse(ColumnBody);
            gp.CloseAllFigures();

            LinearGradientBrush Brush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);

            g.FillPath(Brush, gp);
            g.DrawPath(pen, gp);

            Font f = new("Arial", 16.0F);
            SizeF size = g.MeasureString("Adj", new Font("Arial", 16.0F, FontStyle.Regular, GraphicsUnit.Point));
            g.DrawString("Adj", f, Brushes.Black, rectangle.Left + rectangle.Width / 2 - size.Width / 2
                        , rectangle.Top + rectangle.Height / 2 - size.Height / 2);

            return bitmap;
        }

        public static Bitmap Set()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new(StreamColor, PenWidth);
            Color col2 = Color.White;
            Color col1 = Color.Gray;

            int rhoff = Convert.ToInt32(0.43 * rectangle.Width);
            int r = rectangle.Width - rhoff;
            int l = 0;
            int w = r - l;
            int ArcHeight = Convert.ToInt32(w / 2);
            int t = rectangle.Top;
            int b = rectangle.Bottom;
            int h = b - t;

            GraphicsPath gp = new();
            Rectangle ColumnBody = new(rectangle.Left, t + ArcHeight / 2, rectangle.Width, h - ArcHeight);

            gp.AddEllipse(ColumnBody);
            gp.CloseAllFigures();

            LinearGradientBrush Brush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);

            g.FillPath(Brush, gp);
            g.DrawPath(pen, gp);

            Font f = new("Arial", 16.0F);
            SizeF size = g.MeasureString("Set", new Font("Arial", 16.0F, FontStyle.Regular, GraphicsUnit.Point));
            g.DrawString("Set", f, Brushes.Black, rectangle.Left + rectangle.Width / 2 - size.Width / 2
                        , rectangle.Top + rectangle.Height / 2 - size.Height / 2);

            return bitmap;
        }

        public static Bitmap GibbsRx()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new(StreamColor, PenWidth);
            Color col2 = Color.White;
            Color col1 = Color.Gray;

            int rhoff = Convert.ToInt32(0.43 * rectangle.Width);
            int r = rectangle.Width - rhoff;
            int l = 0;
            int w = r - l;
            int ArcHeight = Convert.ToInt32(w / 2);
            int t = rectangle.Top;
            int b = rectangle.Bottom;
            int h = b - t;

            GraphicsPath gp = new();
            Rectangle ColumnBody = new(rectangle.Left, t + ArcHeight / 2, rectangle.Width, h - ArcHeight);

            gp.AddArc(rectangle.Left, t, rectangle.Width, ArcHeight, 180, 180);
            gp.AddRectangle(ColumnBody);
            gp.AddArc(rectangle.Left, b - ArcHeight - 1, rectangle.Width, ArcHeight, 0, 180);

            gp.CloseAllFigures();

            LinearGradientBrush Brush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);

            g.FillPath(Brush, gp);
            g.DrawPath(pen, gp);

            Font f = new("Arial", 16.0F);
            SizeF size = g.MeasureString("Grx", new Font("Arial", 16.0F, FontStyle.Regular, GraphicsUnit.Point));
            g.DrawString("Grx", f, Brushes.Black, rectangle.Left + rectangle.Width / 2 - size.Width / 2
                , rectangle.Top + rectangle.Height / 2 - size.Height / 2);

            return bitmap;
        }

        public static Bitmap Heater()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new(FillMode.Winding);

            Pen pen = new(StreamColor, PenWidth);
            pen.Color = Color.White;

            gp.AddEllipse(rectangle);

            pen.Color = Color.Black;

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            gp.AddEllipse(rectangle);
            gp.AddLine(rectangle.Left, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2);
            gp.AddLine(rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4);
            gp.AddLine(rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4);
            gp.AddLine(rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2);
            gp.AddLine(rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height / 2);

            LinearGradientBrush BodyBrush = new
                (new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            pen.Dispose();

            return bitmap;
        }

        public static Bitmap Isom()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Line()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap PlugFlowRx()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;

            Color col2 = Color.White;
            Color col1 = Color.Gray;
            Pen pen = new(StreamColor, PenWidth);

            GraphicsPath gp = new(FillMode.Winding);

            Point[] Polygon = new Point[4];
            Polygon[0] = new Point(r.TopLeft().X, r.TopLeft().Y);
            Polygon[1] = new Point(r.BottomLeft().X, r.BottomLeft().Y);
            Polygon[2] = new Point(r.BottomRight().X, r.BottomRight().Y);
            Polygon[3] = new Point(r.TopRight().X, r.TopRight().Y);

            gp.AddPolygon(Polygon);

            PathGradientBrush BotBrush = new(gp);

            BotBrush.CenterPoint = new PointF((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);
            BotBrush.CenterColor = col2;
            BotBrush.SurroundColors = new Color[] { col1 };

            g.FillPath(BotBrush, gp);
            g.DrawPath(pen, gp);

            g.DrawLine(pen, r.TopLeft(), r.BottomRight());
            g.DrawLine(pen, r.BottomLeft(), r.TopRight());

            return bitmap;
        }

        public static Bitmap Recycle()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle R = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new();
            gp.FillMode = FillMode.Winding;

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Pen pen = new(StreamColor, PenWidth);
            Point[] points = new Point[3];
            points[0] = R.BottomLeft();
            points[1] = R.Centre();
            points[2] = R.BottomRight();

            LinearGradientBrush BodyBrush = new(new PointF(R.Left, 0), new PointF(R.Right, 0), col2, col1);

            gp.AddEllipse(R);
            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            pen.Dispose();

            return bitmap;
        }

        public static Bitmap Reformer()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Separator()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new(StreamColor, PenWidth);
            Color col2 = Color.White;
            Color col1 = Color.Gray;

            int rhoff = Convert.ToInt32(0.43 * rectangle.Width);
            int r = rectangle.Width - rhoff;
            int l = 0;
            int w = r - l;
            int ArcHeight = Convert.ToInt32(w / 2);
            int t = rectangle.Top;
            int b = rectangle.Bottom;
            int h = b - t;

            GraphicsPath gp = new();
            RectangleF ColumnBody = new(rectangle.Left, t + ArcHeight / 2, rectangle.Width, h - ArcHeight);

            gp.AddArc(rectangle.Left, t, rectangle.Width, ArcHeight, 180, 180);
            gp.AddRectangle(ColumnBody);
            gp.AddArc(rectangle.Left, b - ArcHeight - 1, rectangle.Width, ArcHeight, 0, 180);

            gp.CloseAllFigures();

            LinearGradientBrush Brush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);

            g.FillPath(Brush, gp);
            g.DrawPath(pen, gp);

            return bitmap;
        }

        public static Image byteArrayToImage(byte[] source)
        {
            MemoryStream ms = new(source);
            Image ret = Image.FromStream(ms);
            return ret;
        }

        public static Bitmap Spreadsheet()
        {
            return Properties.Resources.Spreadsheet;
        }

        public static Bitmap Pointer()
        {
            return Properties.Resources.Pointer;
        }

        public static Bitmap Stripper()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Tray()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Valve()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle R = new(0, 0, 99, 99);
            Rectangle RotatedRectangle = R;
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;

            Color col2 = Color.White;
            Color col1 = Color.Gray;
            Pen pen = new(StreamColor, PenWidth);

            GraphicsPath gp = new(FillMode.Winding);

            int offset = 20;

            Point[] Polygon = new Point[4];
            Polygon[0] = new Point(R.TopLeft().X, R.TopLeft().Y + offset);
            Polygon[1] = new Point(R.BottomLeft().X, R.BottomLeft().Y - offset);
            Polygon[2] = new Point(R.TopRight().X, R.TopRight().Y + offset);
            Polygon[3] = new Point(R.BottomRight().X, R.BottomRight().Y - offset);

            gp.AddPolygon(Polygon);

            PathGradientBrush BotBrush = new(gp);

            BotBrush.CenterPoint = new PointF((RotatedRectangle.Left + RotatedRectangle.Right) / 2,
                (RotatedRectangle.Top + RotatedRectangle.Bottom) / 2);
            BotBrush.CenterColor = col2;
            BotBrush.SurroundColors = new Color[] { col1 };

            g.FillPath(BotBrush, gp);
            g.DrawPath(pen, gp);

            return bitmap;
        }

        public static Bitmap Visbreaker()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle r = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);

            return bitmap;
        }

        public static Bitmap Exchanger()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 99, 99);
            using Graphics g = Graphics.FromImage(bitmap);
            //draw image..

            g.SmoothingMode = SmoothingMode.HighQuality;

            Pen BlackPen = new(Color.Black, 1);

            LinearGradientBrush BodyBrush = new(new PointF(rectangle.Left, 0), new PointF(rectangle.Right, 0), col2, col1);
            g.FillEllipse(BodyBrush, rectangle);
            g.DrawLine(BlackPen, rectangle.Left, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2);
            g.DrawLine(BlackPen, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4);
            g.DrawLine(BlackPen, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4);
            g.DrawLine(BlackPen, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2);
            g.DrawLine(BlackPen, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height / 2);
            g.DrawEllipse(BlackPen, rectangle);

            BlackPen.Dispose();
            // bitmap.Save("C:\\Exchnager.png", ImageFormat.Png);
            return bitmap;
        }

        public static Bitmap Pump()
        {
            Bitmap bitmap = new(100, 100);
            Rectangle rectangle = new(0, 0, 98, 98);
            Rectangle RotatedRectangle = rectangle;
            using Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen blackpen = new(Color.Black, 2);

            Point[] points = new Point[4];
            points[0] = rectangle.BottomLeft();
            points[1] = rectangle.Centre();
            points[2] = rectangle.BottomRight();
            points[3] = rectangle.BottomLeft();

            Point[] points2 = new Point[4];
            points2[0] = rectangle.TopCentre();
            points2[1] = rectangle.TopRight();
            points2[2] = rectangle.RightMiddle();
            points2[3] = rectangle.Centre();

            LinearGradientBrush BodyBrush = new(new PointF(RotatedRectangle.Left, 0),
                new PointF(RotatedRectangle.Right, 0), col2, col1);

            g.FillPolygon(BodyBrush, points);
            g.DrawLines(blackpen, points);
            g.FillEllipse(BodyBrush, rectangle);
            g.DrawEllipse(blackpen, rectangle);
            blackpen.Width = 1;
            g.DrawLine(blackpen, rectangle.LeftMiddle(), rectangle.Center());
            g.DrawLine(blackpen, rectangle.TopCentre(), rectangle.TopRight());

            blackpen.Dispose();

            //bitmap.Save("C:\\Pump.png", ImageFormat.Png);

            return bitmap;
        }
    }
}