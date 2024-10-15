using Extensions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Units.CaseStudy;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable]
    internal class DrawCaseStudy : DrawRectangle, ISerializable
    {
        private static int Count = 0;
        public ModelEngine.CaseStudy caseStudy = new();

        public DrawCaseStudy() : this(0, 0, 1, 1)
        {
        }

        public DrawCaseStudy(int x, int y, int width, int height) : base()
        {
            if (width < 20)
                width = 20;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Initialize();

            this.Name = "CaseStudy" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawCaseStudy(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                caseStudy = (ModelEngine.CaseStudy)info.GetValue("CaseStudy", typeof(ModelEngine.CaseStudy));
            }
            catch
            {
            }
        }

        public override void CreateFlowsheetUOModel()
        {
            if (this.Flowsheet.CaseStudies is not null)
            {
                this.Flowsheet.CaseStudies.Add(caseStudy);
                caseStudy.Name = this.Name;
            }
        }

        public CaseStudyForm2 Dlg { get; private set; }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawCaseStudy drawRectangle = new();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public override bool UpdateAttachedModel()
        {
            return true;
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                Pen pen = new(StreamColor, PenWidth);
                Color col2 = Color.White;
                Color col1 = Color.Gray;

                int rhoff = Convert.ToInt32(0.43 * this.rectangle.Width);
                int r = this.rectangle.Width - rhoff;
                int l = 0;
                int w = r - l;
                int ArcHeight = Convert.ToInt32(w / 2);
                int t = this.rectangle.Top;
                int b = this.rectangle.Bottom;
                int h = b - t;

                GraphicsPath gp = new();
                RectangleF ColumnBody = new(this.rectangle.Left, t, this.rectangle.Width, h);

                //gp.AddArc(this.rectangle.Left, t, this.rectangle.Width, ArcHeight, 180, 180);
                gp.AddRectangle(ColumnBody);
                //gp.AddArc(this.rectangle.Left, b - ArcHeight - 1, this.rectangle.Width, ArcHeight, 0, 180);

                gp.CloseAllFigures();

                if (Angle != enumRotationAngle.a0)
                    Rotate(gp, rectangle.Center());

                if (FlipHorizontal)
                {
                    Matrix m = new();
                    m.Scale(-1, 1);
                    m.Translate(RotatedRectangle.Width + 2 * RotatedRectangle.Left, 0, MatrixOrder.Append);
                    gp.Transform(m);
                }

                if (FlipVertical)
                {
                    Matrix m = new();
                    m.Scale(1, -1);
                    m.Translate(0, RotatedRectangle.Height + 2 * RotatedRectangle.Top, MatrixOrder.Append);
                    gp.Transform(m);
                }

                LinearGradientBrush Brush = new(new PointF(RotatedRectangle.Left, 0), new PointF(RotatedRectangle.Right, 0), col2, col1);

                g.FillPath(Brush, gp);
                g.DrawPath(pen, gp);
            }
            catch { }
        }

        public override void Dump()
        {
            base.Dump();
            Trace.WriteLine("rectangle.X = " + rectangle.X.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Y = " + rectangle.Y.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Width = " + rectangle.Width.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Height = " + rectangle.Height.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = GetNormalizedRectangle(rectangle);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Dlg = new CaseStudyForm2(caseStudy);
            Dlg.Show();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CaseStudy", caseStudy);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
        }

        public int Rows { get; set; } = 20;
        public int Cols { get; set; } = 10;
    }
}