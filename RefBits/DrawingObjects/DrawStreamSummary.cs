using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable]
    internal class DrawSummary : Units.DrawRectangle, ISerializable
    {
        private DrawBaseStream attachedstream;
        private DrawRectangle attachedobject;
        private FontFamily fontfamily = new FontFamily("Arial");
        private Font font = new Font(FontFamily.GenericSansSerif, 12);

        private int Count;

        public DrawBaseStream Attachedstream { get => attachedstream; set => attachedstream = value; }
        public DrawRectangle Attachedobject { get => attachedobject; set => attachedobject = value; }
        public FontFamily FontFamily { get => fontfamily; set => fontfamily = value; }
        public Font Font { get => font; set => font = value; }

        public DrawSummary() : this(0, 0, 1, 1)
        {
        }

        public DrawSummary(int x, int y, int width, int height)
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
            Initialize();

            this.Name = "DN" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawSummary drawRectangle = new DrawSummary();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        internal Dictionary<string, Tuple<DrawMaterialStream, bool>> streams = new Dictionary<string, Tuple<DrawMaterialStream, bool>>();

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            try
            {
                Pen pen = new Pen(StreamColor, PenWidth);
                SolidBrush brush = new SolidBrush(StreamColor);

                brush.Color = pen.Color;

                GraphicsPath gp = new GraphicsPath(FillMode.Winding);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                //this.rectangle.Location = loc;
                this.rectangle.Height = 12;
                this.rectangle.Width = 100;// (int )stringSize.Width;

                g.FillPath(brush, gp);
            }
            catch { }
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            List<DrawMaterialStream> streams = drawArea.GraphicsList.ReturnMaterialStreams();
            //SummaryDialog sd = new  SummaryDialog(this, streams);
            //sd.ShowDialog();
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawSummary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                font = (Font)info.GetValue("font", typeof(Font));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("font", font);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
        }
    }
}