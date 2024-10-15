using ModelEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Units.DrawingObjects.UnitDialogs;

namespace Units
{
    [Serializable]
    internal class DrawEnergyStream : DrawBaseStream, ISerializable, IDrawStream
    {
        private static int count = 0;
        private bool isColumnFeed = false;
        private int connectedTrayNo = 0;
        private int sectionnumber = 0;
        private SpecificationCollection spec = new();
        private StreamEnergy stream = new();

        public int Count { get; }

        private double drawFactor = 0;

        public DrawEnergyStream() : base()
        {
            Initialize();
            StreamColor = Color.Green;
            Name = "Energy " + count.ToString();
            count++;
        }

        public DrawEnergyStream(Node hsstart, Node hsend) : base(hsstart, hsend)
        {
            Active = true;
            Name = "Energy " + count.ToString();
            count++;
        }

        public DrawEnergyStream(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                stream = (StreamEnergy)info.GetValue("EnergyStream", typeof(StreamEnergy));
                stream.Name = this.Name;
            }
            catch
            {
            }
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

        public bool IsSolved
        {
            get
            {
                return stream.IsSolved;
            }
            set
            {
                //stream.IsSolved = value;
                base.isSolved = value;
            }
        }

        public override void Draw(Graphics g)
        {
            if (stream.Port.Value.IsKnown)
            {
                this.IsSolved = true;
                this.StreamColor = Color.Green;
            }
            else
            {
                this.IsSolved = false;
                this.StreamColor = Color.Red;
            }

            base.Draw(g);
        }

        [Category("Ports"), Description("In Port")]
        public Port_Energy Port
        {
            get
            {
                if (stream != null)
                    return stream.Port;
                else
                    return null;
            }
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawEnergyStream drawStream = new();
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

        public string EnergyFlow { get; internal set; }

        public StreamEnergy Stream { get => stream; set => stream = value; }
        public DrawName DrawName { get => drawName; set => drawName = value; }

        /// <summary>
        /// return   segment Number
        /// </summary>
        /// <param name="point "></param>
        /// <return  s></return  s>

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            EnergyStreamDialog dlg = new(this);
            dlg.Show();
        }

        public override bool Equals(object obj)
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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EnergyStream", stream);
        }
    }
}