using Extensions;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    using dataset = Tuple<double, double, double, double, double>;

    internal class data
    {
        public double P, T, ME, Flow, Q;

        public data()
        {
        }

        public void SetData(double P, double T, double MolarEnthalpy, double Flow, double Q)
        {
            this.P = P;
            this.T = T;
            this.ME = MolarEnthalpy;
            this.Flow = Flow;
            this.Q = Q;
        }

        public data Clone()
        {
            data d = new data();
            d.SetData(this.P, this.T, this.ME, this.Flow, this.Q);
            return d;
        }
    }

    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    [Serializable]
    public class DrawRecycle : DrawRectangle, ISerializable
    {
        private static int Count = 1;
        private RecycleDialog dlg;
        public Recycle recycle = new Recycle();
        public IterationDataStore datastore = new IterationDataStore();
        private data Xn = new data(), Fxn = new data(), Xn_1 = new data(), Fxn_1 = new data(), XNp1 = new data();
        private bool useWegstein = false;
        public bool firstPass = true;

        public bool UseWegstein
        { get { return useWegstein; } set => useWegstein = value; }

        public DrawRecycle() : this(0, 0, 1, 1)
        {
        }

        public int IteratioCount
        {
            get
            {
                return recycle.RecCounter;
            }
        }

        public DrawRecycle(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Hotspots.Add(new Node(this, 1f, 0.5f, "FEED", NodeDirections.Right, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Hotspots.Add(new Node(this, 0f, 0.5f, "PRODUCT", NodeDirections.Left, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Initialize();
            IsFeedObject = true;

            this.Name = "Recycle" + Count.ToString();
            Count++;
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return recycle.PortIn;
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return recycle.PortOut;
            }
        }

        [Category("Solver"), Description("IsSolved")]
        public bool Solved
        {
            get
            {
                return recycle.isSolved();
            }
        }

        public override bool UpdateAttachedModel()
        {
            Node Inlet, Outlet;

            Inlet = Hotspots["FEED"];
            Outlet = Hotspots["PRODUCT"];

            Inlet.PortGuid = recycle.PortIn.Guid;
            Outlet.PortGuid = recycle.PortOut.Guid;

            recycle.Name = this.Name;

            return true;
        }

        public bool CheckIsConverged()
        {
            return true;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawRecycle drawRectangle = new DrawRecycle();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new GraphicsPath();
            gp.FillMode = FillMode.Winding;

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Pen pen = new Pen(StreamColor, PenWidth);
            Point[] points = new Point[3];
            points[0] = R.BottomLeft();
            points[1] = R.Centre();
            points[2] = R.BottomRight();

            LinearGradientBrush BodyBrush = new LinearGradientBrush
                (new PointF(this.RotatedRectangle.Left, 0), new PointF(this.RotatedRectangle.Right, 0), col2, col1);

            gp.AddEllipse(DrawRectangle.GetNormalizedRectangle(rectangle));

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
            g.DrawPath(pen, gp);

            pen.Dispose();

            base.Draw(g);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Node Feed = this.Hotspots.Search("FEED");
            Node Product = this.Hotspots.Search("PRODUCT");

            Feeds.Add(Feed);
            Products.Add(Product);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new RecycleDialog(Flowsheet, this);
            dlg.rbWegstein.Checked = UseWegstein;
            UseWegstein = dlg.rbWegstein.Checked;

            dlg.ShowDialog();
            //recycle.IsSolved = false;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UseWegstein", useWegstein);
            info.AddValue("recycle", recycle);
            base.GetObjectData(info, context);
        }

        public DrawRecycle(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                useWegstein = info.GetBoolean("UseWegstein");
                recycle = (Recycle)info.GetValue("recycle", typeof(Recycle));
            }
            catch
            {
            }
        }
    }

    public class IterationDataStore
    {
        // P, T, MolarEnthalpy, Flow, Frac
        public int count = 0;

        public Dictionary<int, Tuple<double, double, double, double, double>> datastore = new Dictionary<int, Tuple<double, double, double, double, double>>();

        public IterationDataStore()
        {
        }

        public void Add(dataset set1)
        {
            datastore.Add(count, set1); // feeds
            //datastore.Add(count+1000, set2); // products
            count++;
        }

        public int Count()
        {
            return datastore.Count;
        }

        public void Erase()
        {
            datastore.Clear();
        }
    }
}