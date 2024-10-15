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
    internal class DrawAdjust : DrawRectangle, ISerializable
    {
        public AdjustObject adjust = new AdjustObject();

        private Node Feed;
        private Node Product;

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = adjust.In.Guid;
            Hotspots["PRODUCT"].PortGuid = adjust.Out.Guid;
            adjust.Name = this.Name;

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
                return adjust.IsDirty;
            }
            set
            {
                adjust.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return adjust.IsSolved;
            }
            set
            {
                //adjust.IsSolved = value;
            }
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return adjust.In;
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return adjust.Out;
            }
        }

        // public  Cooler Cooler { get => cooler; set => cooler = value; }

        public DrawAdjust() : this(0, 0, 1, 1)
        {
        }

        public DrawAdjust(int x, int y, int width, int height)
            : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.SignalIn, HotSpotOwnerType.DrawRectangle));  //feed
            Product = Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Right, HotSpotType.SignalOut, HotSpotOwnerType.DrawRectangle));  //product
            Initialize();

            this.Name = "Adjust" + Count.ToString();
            Count++;
        }

        private static int Count;

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawAdjust drawRectangle = new();
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
            g.DrawImage(Images.Adjust(), this.rectangle);
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawAdjust(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                adjust = (AdjustObject)info.GetValue("adjust", typeof(AdjustObject));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("adjust", adjust);
        }

        [Serializable]
        public class SaveableData
        {
            public SaveableData()
            {
            }
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Feeds.Add(Feed);
            Products.Add(Product);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            DrawObject startobject=null, endobject=null;

            DrawSignalStream streamin = (DrawSignalStream)drawArea.GraphicsList.GetObject(Feed.AttachedStreamGuid);
            if(streamin is not null)
                 startobject = drawArea.GraphicsList.GetObject(streamin.StartDrawObjectGuid);

            DrawSignalStream streamout = (DrawSignalStream)drawArea.GraphicsList.GetObject(Product.AttachedStreamGuid);
            if(streamout is not null)
                endobject = drawArea.GraphicsList.GetObject(streamout.EndDrawObjectGuid);

            AdjustDialog dlg = new AdjustDialog(this, streamin, streamout, startobject, endobject);

            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            this.IsSolved = false;
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            Product = this.Hotspots.Search("PRODUCT");
        }
    }
}