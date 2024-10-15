using ModelEngine;
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
    [Serializable]
    public class DrawPA : DrawRectangle, ISerializable
    {
        private static int count;
        private ePASpecTypes espec1 = ePASpecTypes.Flow, espec2 = ePASpecTypes.ReturnT;
        private Guid drawtray = Guid.Empty, returntray = Guid.Empty, originguid, destinationguid;

        //enumflowtype flowtype = enumflowtype.Molar;
        private double returnTemp, drawFactor = 0, drawFactorBackup = 0, drawFlow = 1;

        private int origintraySection, destinatonTraySection;
        private PumpAround pa = new PumpAround(null);
        public DrawMaterialStream feed, effluent;

        public Guid ReturnTrayDrawGuid
        {
            get { return returntray; }

            set { returntray = value; }
        }

        public Guid DrawTrayDrawGuid
        {
            get { return drawtray; }
            set { drawtray = value; }
        }

        public Guid Originguid
        {
            get
            {
                return originguid;
            }

            set
            {
                originguid = value;
            }
        }

        public Guid Destinationguid
        {
            get
            {
                return destinationguid;
            }

            set
            {
                destinationguid = value;
            }
        }

        public int DrawSectionNo
        {
            get
            {
                return origintraySection;
            }

            set
            {
                origintraySection = value;
            }
        }

        public int ReturnSectionNo
        {
            get
            {
                return destinatonTraySection;
            }
            set
            {
                destinatonTraySection = value;
            }
        }

        /*Specification spec1 = new  Specification("Flow Rate", double.NaN, PropertyEnum.MassFlow, eSpecType.Flow);
        Specification spec2 = new  Specification("Duty", double.NaN, PropertyEnum.EnergyFlow, eSpecType.Energy);
        Specification spec3 = new  Specification("return   Temperature ", double.NaN, PropertyEnum.Temperature , eSpecType.Energy);
        Specification spec4 = new  Specification("Delta T", double.NaN, PropertyEnum.DeltaT, eSpecType.Temperature );*/

        [CategoryAttribute("Properties"),
        Description("kJ/hr")]
        public DrawPA() : this(0, 0, 1, 1)
        {
        }

        public DrawPA(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Initialize();

            /*Specs.Add(spec1);
            Specs.Add(spec2);
            Specs.Add(spec3);
            Specs.Add(spec4);*/

            this.Name = "PumpAround" + count.ToString();
            count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawPA newPA = new DrawPA();
            newPA.rectangle = this.rectangle;

            FillDrawObjectFields(newPA);

            newPA.espec1 = this.espec1;
            newPA.espec2 = this.espec2;
            newPA.drawtray = drawtray;
            newPA.returntray = returntray;
            //new PA.flowtype = enumflowtype.Molar;
            newPA.returnTemp = returnTemp;
            newPA.originguid = originguid;
            newPA.destinationguid = destinationguid;
            newPA.originguid = originguid;
            newPA.origintraySection = origintraySection;
            newPA.destinatonTraySection = destinatonTraySection;
            newPA.drawFactor = drawFactor;
            newPA.drawFactorBackup = drawFactorBackup;
            newPA.drawFlow = drawFlow;
            return newPA;
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Pen pen = new Pen(StreamColor, PenWidth);

            if (Active)
            {
                pen.Color = this.StreamColor;
            }
            else
                pen.Color = Color.Gray;

            if (Active)
                g.FillEllipse(pen.Brush, DrawRectangle.GetNormalizedRectangle(rectangle));

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            LinearGradientBrush BodyBrush = new System.Drawing.Drawing2D.LinearGradientBrush
         (new PointF(this.rectangle.Left, 0), new PointF(this.rectangle.Right, 0), col2, col1);

            if (Active)
                g.FillEllipse(BodyBrush, DrawRectangle.GetNormalizedRectangle(rectangle));
            g.DrawEllipse(pen, DrawRectangle.GetNormalizedRectangle(rectangle));

            g.DrawLine(pen, rectangle.Left, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2);
            g.DrawLine(pen, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4);
            g.DrawLine(pen, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4);
            g.DrawLine(pen, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2);
            g.DrawLine(pen, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height / 2);
            pen.Dispose();
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            PADialog psd = new(this);
            psd.ShowDialog();
        }

        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }

        public bool Active
        {
            get
            {
                return pa.IsActive;
            }

            set
            {
                pa.IsActive = value;
            }
        }

        public ePASpecTypes Espec1
        {
            get
            {
                return espec1;
            }

            set
            {
                espec1 = value;
            }
        }

        public ePASpecTypes Espec2
        {
            get
            {
                return espec2;
            }

            set
            {
                espec2 = value;
            }
        }

        public PumpAround PumpAround { get => pa; set => pa = value; }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawPA(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                DrawTrayDrawGuid = (Guid)info.GetValue("DrawTray", typeof(Guid));
                ReturnTrayDrawGuid = (Guid)info.GetValue("returnTray", typeof(Guid));
                originguid = (Guid)info.GetValue("OGuid", typeof(Guid));
                destinationguid = (Guid)info.GetValue("DGuid", typeof(Guid));
                Active = info.GetBoolean("Active");
                pa = (PumpAround)info.GetValue("pa", typeof(PumpAround));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("DrawTray", DrawTrayDrawGuid);
            info.AddValue("returnTray", ReturnTrayDrawGuid);
            info.AddValue("OGuid", originguid);
            info.AddValue("DGuid", destinationguid);
            info.AddValue("Active", Active);
            info.AddValue("pa", pa);
        }
    }
}