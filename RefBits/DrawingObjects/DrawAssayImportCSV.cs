using EngineThermo;
using Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    [Serializable]
    class DrawAssayImportCSV : DrawRectangle, ISerializable
    {
        string activeassay = "none";

        public string Activeassay
        {
            get { return activeassay; }
            set { activeassay = value; }
        }

        double massflow = 100;
        double Temperature = 25;
        double Pressure = 5;
        bool isassay = false;
        bool assaychanged = false;

        public bool AssayChanged
        {
            get { return assaychanged; }
            set { assaychanged = value; }
        }

        public bool IsAssay
        {
            get { return isassay; }
            set
            {
                isassay = value;
                activeassay = "none";
            }
        }

        Oil oil = new Oil();
        static private int Count;

        public Oil Oil
        {
            get { return oil; }
            set { oil = value; }
        }

        public double Massflow
        {
            get { return massflow; }
            set
            {
                massflow = value;
                oil.MF = value;
            }
        }

        public double Temperature
        {
            get { return Temperature; }
            set { Temperature = value; }
        }

        public double Pressure
        {
            get { return Pressure; }
            set { Pressure = value; }
        }

        public DrawAssayImportCSV()
            : this(0, 0, 1, 1)
        {
            IsFeedObject = true;
        }

        public DrawAssayImportCSV(int x, int y, int width, int height)
            : base()
        {
            if (width < 20)
                width = 20;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Left, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            IsFeedObject = true;
            Initialize();

            this.Name = "AI" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawAssayImportCSV drawRectangle = new DrawAssayImportCSV();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public class AssayList : CollectionConverter
        {
            AssayNamesData adata = new AssayNamesData();
            // Methods
            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new TypeConverter.StandardValuesCollection(this.adata.GetNames());
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }

        public string ActiveAssay
        {
            get
            {
                return activeassay;
            }
            set
            {
                activeassay = value;
            }
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            Color col1 = Color.Gray, col2 = Color.White;

            Color FrameColor = Color.Black;
            Color LineColor = Color.Blue;

            float t = Convert.ToInt32(this.Rectangle.Top);
            float l = Convert.ToInt32(this.Rectangle.Left);
            float w = this.Rectangle.Width;
            float h = this.Rectangle.Height;
            float r = this.Rectangle.Right;
            float b = this.Rectangle.Bottom;

            try
            {
                Pen p = new Pen(FrameColor, 1);
                GraphicsPath gp = new GraphicsPath(FillMode.Winding);

                LinearGradientBrush BodyBrush = new LinearGradientBrush(new PointF(RotatedRectangle.Left, 0), new PointF(RotatedRectangle.Right, 0), col2, col1);
                BodyBrush.SetBlendTriangularShape(1f, 1f);
                g.SmoothingMode = SmoothingMode.HighSpeed;

                RectangleF ColumnBody = new RectangleF(l, t, w, h);
                gp.AddRectangle(ColumnBody);

                RectangleF rt = new RectangleF(l - w / 10f, t - 2f, w + w / 5f, 3);
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

                if (Angle != enumRotationAngle.a0)
                    Rotate(gp, rectangle.Center());

                if (FlipHorizontal)
                {
                    Matrix m = new Matrix();
                    m.Scale(-1, 1);
                    m.Translate(RotatedRectangle.Width + 2 * RotatedRectangle.Left, 0, MatrixOrder.Append);
                    gp.Transform(m);
                }

                if (FlipVertical)
                {
                    Matrix m = new Matrix();
                    m.Scale(1, -1);
                    m.Translate(0, RotatedRectangle.Height + 2 * RotatedRectangle.Top, MatrixOrder.Append);
                    gp.Transform(m);
                }

                g.FillPath(BodyBrush, gp);
                g.DrawPath(p, gp);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = DrawRectangle.GetNormalizedRectangle(rectangle);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            AssayDataBaseManager.AssayImportFormCSV Assay = new AssayDataBaseManager.AssayImportFormCSV(ActiveAssay);
            //Assay.AssayName = ActiveAssay;

            if (activeassay != "none")
            {
                //Assay.LoadAssayFromFile(false);
            }

            Assay.ShowDialog();
            ActiveAssay = Assay.AssayName;

            if (Assay.Regenerated)
            {
                oil = Assay.Crude;
                IsSolved = false;
            }
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawAssayImportCSV(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ActiveAssay = info.GetString("AssayName");
            Massflow = info.GetDouble("MF");
            Pressure = info.GetDouble("Pressure");
            Temperature = info.GetDouble("Temperature");
            // oil.ComponentList = (BaseCompCollection)info.GetValue("Components", typeof(BaseCompCollection));
            oil.UpdateFlows(FlowFlag.Mass);

            assaychanged = false;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AssayName", activeassay);
            info.AddValue("MF", Massflow);
            info.AddValue("Pressure", Pressure);
            info.AddValue("Temperature", Temperature);
            info.AddValue("Components", oil.Components);
            base.GetObjectData(info, context);
        }
    }
}