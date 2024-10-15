using Main.Images;
using ModelEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable]
    internal class DrawSet : DrawRectangle, ISerializable
    {
        public SetObject set = new SetObject();

        private Node Feed;
        private Node Product;
        private Node Q;

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = set.In.Guid;
            Hotspots["PRODUCT"].PortGuid = set.Out.Guid;
            set.Name = this.Name;

            return true;
        }

        public bool ValidateConnections(GraphicsList gl)
        {
            DrawStreamCollection fs = gl.ReturnExternalFeedStreams(this);
            DrawStreamCollection ps = gl.ReturnSideProductStreams(this, true);

            Guid startid = fs[0].StartDrawObjectGuid;

            foreach (DrawMaterialStream ds in ps)
            {
                if (ds.EndDrawObjectGuid == startid)
                {
                    return true;
                }
            }

            return false;
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return set.IsDirty;
            }
            set
            {
                set.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return set.IsSolved;
            }
          /*  set
            {
                set.IsSolved = value;
            }*/
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return set.In;
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return set.Out;
            }
        }

        public SetObject Cooler { get => set; set => set = value; }

        public DrawSet() : this(0, 0, 1, 1)
        {
        }

        public DrawSet(int x, int y, int width, int height)
            : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.SignalIn, HotSpotOwnerType.DrawRectangle));  //feed
            Product = Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Right, HotSpotType.SignalOut, HotSpotOwnerType.DrawRectangle));  //product
            Initialize();

            this.Name = "Set" + Count.ToString();
            Count++;
        }

        private static int Count;

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawSet drawRectangle = new();
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
            g.DrawImage(Images.Set(), this.rectangle);
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                set = (SetObject)info.GetValue("cooler", typeof(SetObject));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("cooler", set);
        }

        [Serializable]
        public class SaveableData
        {
            public SaveableData()
            {
            }

            public double ExitTemperature = 70;
            public double DeltaTemperature = 0;
            public double DeltaPress = 0.5;
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Feeds.Add(Feed);
            Products.Add(Product);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            DrawSignalStream streamin = (DrawSignalStream)drawArea.GraphicsList.GetObject(Feed.AttachedStreamGuid);
            DrawSignalStream streamout = (DrawSignalStream)drawArea.GraphicsList.GetObject(Product.AttachedStreamGuid);
            DrawObject startobject = drawArea.GraphicsList.GetObject(streamin.StartDrawObjectGuid);
            DrawObject endobject = drawArea.GraphicsList.GetObject(streamout.EndDrawObjectGuid);

            SetDialog dlg = new SetDialog(this, streamin, streamout, startobject, endobject);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            //this.IsSolved = false;
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            Product = this.Hotspots.Search("PRODUCT");
        }
    }
}