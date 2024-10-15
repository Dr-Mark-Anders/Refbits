using ModelEngine;
using Main.Images;
using ModelEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Units.UOM;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    [Serializable]
    internal class DrawGenericAssay : DrawRectangle, ISerializable
    {
        private string activeassay = "none";
        public Node Product;
        public Port PortOut;

        public Assay assay = new();

        public override bool UpdateAttachedModel()
        {
            Product = Hotspots["PRODUCT"];
            PortOut = assay.PortOut;
            assay.Name = this.Name;
            Hotspots["PRODUCT"].PortGuid = assay.PortOut.Guid;
            return true;
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return assay.IsDirty;
            }
            set
            {
                assay.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return assay.IsSolved;
            }
            set
            {
                //assay.IsSolved = value;
            }
        }

        public string Activeassay
        {
            get { return activeassay; }
            set { activeassay = value; }
        }

        private double massflow = 100;
        private Temperature temperature = 25;
        private Pressure pressure = 5;
        private bool isassay = false;
        private bool assaychanged = false;

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

        private Port_Material oil = new();
        private static int Count;

        public Port_Material Oil
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
                oil.MF_.BaseValue = value;
            }
        }

        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public double Pressure
        {
            get { return pressure; }
            set { pressure = value; }
        }

        public DrawGenericAssay()
            : this(0, 0, 1, 1)
        {
            IsFeedObject = true;
        }

        public DrawGenericAssay(int x, int y, int width, int height)
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
            Product = Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Left, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            IsFeedObject = true;
            Initialize();

            this.Name = "Assay Feed" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawGenericAssay drawRectangle = new();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
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
            g.DrawImage(Images.GenericAssay(), this.rectangle);
            base.Draw(g);
        }

        /// <summary>
        /// Save objevt to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AssayName", activeassay);
            info.AddValue("MF", Massflow);
            info.AddValue("Pressure", Pressure);
            info.AddValue("Temperature", Temperature);
            info.AddValue("Components", oil.cc);
            info.AddValue("Oil", oil, typeof(Components));
            info.AddValue("ASSAY", assay);

            base.GetObjectData(info, context);
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawGenericAssay(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                ActiveAssay = info.GetString("AssayName");
                Massflow = info.GetDouble("MF");
                pressure = info.GetDouble("Pressure");
                temperature = info.GetDouble("Temperature");
                assay = (Assay)info.GetValue("ASSAY", typeof(Assay));
            }
            catch { }

            assaychanged = false;
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            assay.IsDirty = true;
            assay.ShowAssayForm();
            ActiveAssay = assay.Name;
        }
    }
}