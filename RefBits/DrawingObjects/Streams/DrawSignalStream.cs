using ModelEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    [Serializable]
    public class DrawSignalStream : DrawBaseStream, ISerializable, IDrawStream
    {
        private static readonly int count = 0;
        private int connectedTrayNo = 0;
        private int sectionnumber = 0;
        private SpecificationCollection spec = new();
        private StreamSignal stream = new();
        public int Count { get; }

        private double drawFactor = 0;

        public static void Clear()
        {
        }

        public override void Draw(Graphics g)
        {
            if (stream.Port.Value.IsKnown)
            {
                //this.IsSolved = true;
                this.StreamColor = Color.Green;
            }
            else
            {
               // this.IsSolved = false;
                this.StreamColor = Color.Green;
            }

            base.Draw(g);
        }

        public bool IsDirty
        {
            get
            {
                return stream.IsDirty;
            }
            set
            {
                stream.IsDirty = value;
            }
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>

        public bool IsSolved
        {
            get
            {
                return stream.IsSolved;
            }
           /* set
            {
                //stream.IsSolved = value;
                base.isSolved = value;
            }*/
        }

        public DrawSignalStream() : base()
        {
            Initialize();
            StreamColor = Color.Yellow;
            Name = "Signal " + count.ToString();
        }

        [Category("Ports"), Description("In Port")]
        public Port_Signal Port
        {
            get
            {
                if (stream != null)
                    return stream.Port;
                else
                    return null;
            }
        }

        public DrawSignalStream(Node hsstart, Node hsend) : base(hsstart, hsend)
        {
            Guid guid = Guid.Empty;
            switch (hsstart.Owner)
            {
                case DrawRectangle dr:
                    guid = dr.Guid;
                    break;

                case DrawMaterialStream dms:
                    guid = dms.Guid;
                    break;

                default:
                    break;
            }
            this.startDrawObjectID = guid;

            guid = Guid.Empty;
            switch (hsend.Owner)
            {
                case DrawRectangle dr:
                    guid = dr.Guid;
                    break;

                case DrawMaterialStream dms:
                    guid = dms.Guid;
                    break;

                default:
                    break;
            }
            this.endDrawObjectGuid = guid;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawSignalStream drawStream = new();
            return drawStream;
        }

        public bool IsColumnFeed
        {
            get
            {
                return isColumnFeed;
            }

            set
            {
                isColumnFeed = value;
            }
        }

        public int ConnectedTray
        {
            get
            {
                return connectedTrayNo;
            }

            set
            {
                connectedTrayNo = value;
            }
        }

        public int SectionNumber
        {
            get
            {
                return sectionnumber;
            }

            set
            {
                sectionnumber = value;
            }
        }

        public SpecificationCollection Spec
        {
            get
            {
                return spec;
            }

            set
            {
                spec = value;
            }
        }

        public int ConnectedTrayNo
        {
            get
            {
                return connectedTrayNo;
            }

            set
            {
                connectedTrayNo = value;
            }
        }

        public double DrawFactor
        {
            get
            {
                return drawFactor;
            }

            set
            {
                drawFactor = value;
            }
        }

        public StreamSignal Stream { get => stream; set => stream = value; }
        public DrawName DrawName { get => drawName; set => drawName = value; }

        /// <summary>
        /// return   segment Number
        /// </summary>
        /// <param name="point "></param>
        /// <return  s></return  s>

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            //DrawObject startobject = drawArea.GraphicsList.GetObject(StartDrawObjectID);
            //DrawObject endobject = drawArea.GraphicsList.GetObject(EndDrawObjectID);
            //SignalStreamDialog stream = new  (this, startobject, endobject);
            //  stream.Show();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return   false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point  return   false.
            if (obj is not DrawMaterialStream)
            {
                return false;
            }

            // return   true if the fields match:
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public DrawSignalStream(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}