using Main.Images;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;
using UOMGrid;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable]
    internal class DrawSpreadhseet : DrawRectangle, ISerializable
    {
        public Node Feed, VapProduct, LiqProduct;
        private static int Count = 0;
        internal Dictionary<Guid, CellData> CellDataList = new();

        public DrawSpreadhseet() : this(0, 0, 1, 1)
        {
        }

        public DrawSpreadhseet(int x, int y, int width, int height) : base()
        {
            if (width < 20)
                width = 20;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            VapProduct = Hotspots.Add(new Node(this, 0.5f, 0f, "VapProduct", NodeDirections.Up, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            LiqProduct = Hotspots.Add(new Node(this, 0.5f, 1f, "LiqProduct", NodeDirections.Down, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Initialize();

            this.Name = "SpreadSheet" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawSpreadhseet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                Rows = info.GetInt32("RowNo");
                Cols = info.GetInt32("ColNo");
                CellDataList = (Dictionary<Guid, CellData>)info.GetValue("CellList", typeof(Dictionary<Guid, CellData>));
            }
            catch
            {
            }
        }

        public override void CreateFlowsheetUOModel()
        {
            Feed = Hotspots["FEED"];
            VapProduct = Hotspots["VapProduct"];
            LiqProduct = Hotspots["LiqProduct"];
        }

        public SpreadsheetDialog3 dlg { get; private set; }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawSpreadhseet drawRectangle = new DrawSpreadhseet();
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
                g.DrawImage(Images.Spreadsheet(), this.rectangle);
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
            Feeds.Add(Feed);
            Products.Add(VapProduct);
            Products.Add(LiqProduct);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            //dlg.DLGValueChangedEvent += new  BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);
            dlg = new SpreadsheetDialog3(this);
            dlg.SetUp();
            dlg.Show();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CellList", CellDataList);
            info.AddValue("RowNo", Rows);
            info.AddValue("ColNo", Cols);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            VapProduct = this.Hotspots.Search("VapProduct");
            LiqProduct = this.Hotspots.Search("LiqProduct");
        }

        public int Rows { get; set; } = 20;
        public int Cols { get; set; } = 10;
    }
}