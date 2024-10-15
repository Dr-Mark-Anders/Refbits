﻿using MathNet.Numerics.Interpolation;
using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Units;

namespace ModelEngine
{
    public enum ConnectionType { in_out, inin, outout, notvalid }

    [Serializable]
    public class AdjustObject : UnitOperation, ISerializable
    {
        bool IsAdjust = true;

        private ConnectionType connectionType = ConnectionType.notvalid;
        public Port_Signal In = new Port_Signal("PortIn");
        public Port_Signal Out = new Port_Signal("PortOut");

        public Guid InobjectGuid = Guid.Empty;
        public Guid OutobjectGuid = Guid.Empty;
        public Guid InPort = Guid.Empty;
        public Guid OutPort = Guid.Empty;
        private BalanceModelType type = BalanceModelType.None;
        public ePropID MVpropid;
        public ePropID CVpropid;
        public double offset = 0;
        public double mult = 1;
        private StreamProperty datain = null, dataout = null;
        public double FinalCV, CurrentCV = double.NaN, PreviousCV =double.NaN;
        public double MV, CurrentMV, PreviousMV;
        public double delta=1;
        public int IterationNO = 0;

        public StreamProperty Datain { get => datain; set => datain = value; }

        public StreamProperty Dataout { get => dataout; set => dataout = value; }
        public ConnectionType ConnectionType { get => connectionType; set => connectionType = value; }

        public AdjustObject(FlowSheet fs) : base()
        {
        }

        public AdjustObject(BalanceModelType type = BalanceModelType.None) : base()
        {
            this.type = type;
        }

        public AdjustObject(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                MVpropid = (ePropID)info.GetValue("propid", typeof(ePropID));
                In = (Port_Signal)info.GetValue("In", typeof(Port_Signal));
                Out = (Port_Signal)info.GetValue("Out", typeof(Port_Signal));
                offset = info.GetDouble("Offset");
                mult = info.GetDouble("Mult");
                CVpropid = (ePropID)info.GetValue("CVpropid", typeof(ePropID));
                FinalCV = info.GetDouble("targetvalue");
                connectionType = (ConnectionType)info.GetValue("connectionType", typeof(ConnectionType));
            }
            catch
            {
            }
            AddInput(In);
            AddOutput(Out);
        }

        public AdjustObject() : base()
        {
            Add(In);
            Add(Out);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("propid", MVpropid);
            info.AddValue("In", In);
            info.AddValue("Out", Out);
            info.AddValue("Offset", offset);
            info.AddValue("Mult", mult);
            info.AddValue("CVpropid", CVpropid);
            info.AddValue("targetvalue", FinalCV);
            info.AddValue("connectionType", connectionType);
        }

        public override bool Solve()
        {
            Port_Material InPort=null, OutPort=null;


            switch (connectionType)
            {
                case ConnectionType.in_out:
                    if (In.ConnectedPortNext is Port_Material pmi && Out.ConnectedPortNext is Port_Material pmo)
                    {
                        InPort = pmi;
                        OutPort = pmo;
                    }
                    break;
                case ConnectionType.inin:
                    if (In.ConnectedPortNext is Port_Material pm)
                    {
                        InPort = pm;
                        OutPort = pm;
                    }
                    break;
                case ConnectionType.outout:
                    if (Out.ConnectedPortNext is Port_Material pmoo)
                    {
                        InPort = pmoo;
                        OutPort = pmoo;
                    }
                    break;
                case ConnectionType.notvalid:
                    InPort = null;
                    OutPort = null;
                    break;
                default:
                    break;

            }

            CurrentCV = OutPort.SignalIn.Value;
            MV = OutPort.SignalOut.Value;


            if (IterationNO == 0)
            {
                OutPort.SignalOut = OutPort.Properties[MVpropid];
                InPort.SignalIn = InPort.Properties[CVpropid];

                CurrentMV = OutPort.Properties[MVpropid];
                CurrentCV = OutPort.Properties[CVpropid];

                OutPort.SignalOut.Value += delta;
                PreviousCV = CurrentCV;

                IterationNO++;
                this.IsDirty = true;

                if(OutPort.Owner is StreamMaterial sm)
                {
                    
                }
                OutPort.Owner.IsDirty = true;
                OutPort.IsDirty = true;
                //OutPort.Flash(true);

                if (double.IsInfinity(CurrentMV))
                    return true;
                return false;  // trigger another solve cycle
            }
            else
            {
             
                CurrentMV = OutPort.Properties[MVpropid];
                CurrentCV = OutPort.Properties[CVpropid];
                double Error = FinalCV - CurrentCV;
                double Change = CurrentCV - PreviousCV;
                double Gradient = Change / this.delta;
                double Newdelta=1;
                if (Gradient == 0 || double.IsNaN(Gradient))
                    Newdelta = 1;
                else
                    Newdelta =  Error / Gradient;

                delta = Newdelta;

                if (double.IsNaN(Newdelta))
                {
                    IterationNO = 0;
                    return true;
                }

                OutPort.Owner.IsDirty = true;

                if (FinalCV.AlmostEquals(CurrentCV))
                {
                    IterationNO = 0;
                    IsDirty = false;
                    //IsSolved = true;
                    return true;
                }
                else
                {
                    OutPort.SignalOut.Value += Newdelta;
                    OutPort.Owner.IsDirty = true;
                    OutPort.Flash(true);
                    OutPort.IsDirty = true;
                    PreviousCV = CurrentCV;
                    IterationNO++;
                    this.IsDirty = true;
                    return false;
                }

            }
        }

      /*  public double CalcTarget()
        {
            if (datain != null && dataout != null)
            {
                dataout.Clear();
                StreamProperty prop = (StreamProperty)datain.CloneDeep();
                prop.origin = SourceEnum.Transferred;
                prop.offset(offset);
                prop.mult(mult);
                return prop;
            }
            return double.NaN;
        }*/

        public void AddInput(Port_Signal port)
        {
            port.flowdirection = FlowDirection.IN;
            Ports.Add(port);
        }

        public void AddOutput(Port_Signal port)
        {
            port.flowdirection = FlowDirection.OUT;
            Ports.Add(port);
        }
    }
}