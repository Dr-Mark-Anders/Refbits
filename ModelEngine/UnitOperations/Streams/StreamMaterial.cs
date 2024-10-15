using ModelEngine;
using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class StreamMaterial : BaseStream, ISerializable
    {
        private Port_Material port = new("Main", FlowDirection.ALL);
       // private Port_Signal portdata = new("Data", FlowDirection.ALL);
      //  public Port_Signal Portdata { get => portdata; set => portdata = value; }
        public Port_Material Port { get => port; set => port = value; }

        public bool isNetBottomConnectedFlow = false;
        public bool isassaydefined = false;
        public PseudoPlantData plantdata = new();

        public StreamMaterial(string Name = "") : base()
        {
            this.Name = Name;
            IsDirty = true;
            Add(Port);
            Port.Owner = this;
        }

        protected StreamMaterial(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                //portdata = (Port_Signal)info.GetValue("Data", typeof(Port_Signal));
                plantdata = (PseudoPlantData)info.GetValue("PlantData", typeof(PseudoPlantData));

                Port = (Port_Material)info.GetValue("Main", typeof(Port_Material));
            }
            catch
            {
                if (plantdata is null)
                    plantdata = new();
            }
            Port.Owner = this;
        }

        [OnDeserialized]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
           // Add(portdata);
            Add(Port);
            Port.Name = this.Name + ": Port";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
          //  info.AddValue("Data", portdata);
            info.AddValue("Main", port);
            info.AddValue("PlantData", plantdata);
        }

        public override bool Solve()
        {
            // Balance.Calculate(this);
            Port.Flash();

            //HandleDataStore();

            if (port.IsSolved)
            {
                //IsSolved = true;
                return true;
            }
            else
            {
                //IsSolved = false;
                return false;
            }
        }

      /*  public void HandleDataStore()
        {
            Port_Signal connectedport = null;

            if (portdata.StreamPort is Port_Signal ps
                && ps.StreamPort is Port_Signal pss)
                connectedport = pss;

            if (connectedport is not null && connectedport.IsFlowIn)
                portdata.Datastore.IsInput = false;
            else
                portdata.Datastore.IsInput = true;

            if (portdata.IsConnected && portdata.Datastore.IsInput)
            {
                portSingle.Properties.EraseSignalTransfer(); // erase signal transfer values

                foreach (StreamProperty item in portdata.Datastore.Properties.Props.Values)
                {
                    if (portSingle.Properties[item.Propid].origin == SourceEnum.Empty
                        && item.IsKnown)
                    {
                        portSingle.Properties[item.Propid] = new StreamProperty(item.Propid, (StreamProperty)item, SourceEnum.SignalTransfer);
                    }
                    else
                    {
                        InconsistencyObject iobj = new InconsistencyObject(item, portdata, portSingle);
                    }
                }
            }
            else
            {
                portdata.Datastore = new(portSingle);
            }
        }*/

        public override bool IsSolved
        {
            get
            {
                return port.IsSolved;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}