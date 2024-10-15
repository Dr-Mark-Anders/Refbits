using Extensions;
using System;
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
    public class DrawName : DrawRectangle, ISerializable
    {
        private DrawBaseStream attachedstream;
        private DrawRectangle attachedobject;
        private FontFamily fontfamily = new("Arial");
        private Font font = new(FontFamily.GenericSansSerif, 12);
        private readonly int Count;

        public DrawBaseStream Attachedstream { get => attachedstream; set => attachedstream = value; }
        public DrawRectangle Attachedobject { get => attachedobject; set => attachedobject = value; }
        public FontFamily FontFamily { get => fontfamily; set => fontfamily = value; }
        public Font Font { get => font; set => font = value; }

        public Guid AttachedObjectGuid;

        public DrawName() : this(0, 0, 1, 1)
        {
        }

        public DrawName(int x, int y, int width, int height)
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
            DrawName drawRectangle = new DrawName();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        private Point loc;

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            try
            {
                Pen pen = new(StreamColor, PenWidth);
                SolidBrush brush = new(StreamColor);

                brush.Color = pen.Color;

                GraphicsPath gp = new(FillMode.Winding);

                if (Attachedstream != null)
                {
                    Point Center = attachedstream.Center;
                    Center.Offset(attachedstream.NameOffSetX, attachedstream.NameOffSetY);
                    Rectangle newloc = new(Center, new Size(20, 11));
                    this.rectangle = newloc;
                    loc = this.rectangle.TopLeft();
                    gp.AddString(attachedstream.Name, font.FontFamily, (int)font.Style, font.Size, loc, StringFormat.GenericDefault);
                }
                else if (Attachedobject != null)
                {
                    Rectangle newloc = new(attachedobject.Rectangle.TopRight(attachedobject.NameOffSetX, attachedobject.NameOffSetY), new Size(20, 11));
                    this.rectangle = newloc;
                    loc = this.rectangle.TopLeft();
                    gp.AddString(attachedobject.Name, font.FontFamily, (int)font.Style, font.Size, loc, StringFormat.GenericDefault);
                }
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
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawName(SerializationInfo info, StreamingContext context) : base(info, context)
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