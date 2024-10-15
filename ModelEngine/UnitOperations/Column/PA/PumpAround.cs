using ModelEngine;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PumpAround : ISerializable
    {
        private bool isactive = true;

        public PumpAround(TraySection traysection, string Name = "")
        {
            drawSection = traysection;
            returnSection = traysection;
            guid = Guid.NewGuid();
            name = Name;
        }

        private StreamMaterial streamIn = new();
        private StreamMaterial streamOut = new();

        public Tray drawTray, returnTray;
        public TraySection drawSection, returnSection;

        public double DrawFactor { get; set; }

        public Temperature ReturnTemp;
        public StreamProperty MoleFlow = new StreamProperty(ePropID.MOLEF, 0.1);

        private Guid guid;
        private string name;
       // private Port_Material returnPort; //= new Port_Material();
       // private Port_Material drawPort;// = new Port_Material();

        public EnthalpyDepartureLinearisation enthalpyDepartureLinearisation = new();
        public EnthalpySimpleLinearisation enthalpySimpleLinearisation = new();

        public override string ToString()
        {
            return this.Name;
        }

        protected PumpAround(SerializationInfo info, StreamingContext context)
        {
            guid = (Guid)info.GetValue("UOGuid", typeof(Guid));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UOGuid", guid);
        }

        public bool IsActive { get; set; }

        public int DrawTrayIndex
        {
            get
            {
                return drawSection.Trays.IndexOf(drawTray);
            }
        }

        public int ReturnTrayIndex
        {
            get
            {
                return drawSection.Trays.IndexOf(returnTray);
            }
        }

        public int DrawTrayNo, ReturnTrayNo;
        public Enthalpy ReturnEnthalpy;

        public void SetDrawTrayNos()
        {
            DrawTrayNo = DrawTrayIndex;
            ReturnTrayNo = ReturnTrayIndex;
        }

        public double tempDrawFactor { get; internal set; }
        public Guid Guid { get => guid; set => guid = value; }
        public string Name { get => name; set => name = value; }
       // public Port_Material ReturnPort { get => returnPort; set => returnPort = value; }
       // public Port_Material DrawPort { get => drawPort; set => drawPort = value; }
        public bool Isactive { get => isactive; set => isactive = value; }
        public StreamMaterial StreamIn { get => streamIn; set => streamIn = value; }
        public StreamMaterial StreamOut { get => streamOut; set => streamOut = value; }

        public void UpdateStreams()
        {
          //  streamIn.Port = DrawPort.Clone();
            streamOut.Port.Clear();
        //    streamOut.Port.cc = DrawPort.cc.Clone();
            streamOut.Port.cc.Origin = SourceEnum.Input;
         //   streamOut.Port.SetPortValue(ePropID.P, returnPort.P_, SourceEnum.Input);
         //   streamOut.Port.SetPortValue(ePropID.MOLEF, returnPort.MolarFlow_, SourceEnum.Input);
         //   streamOut.Port.SetPortValue(ePropID.H, ReturnEnthalpy, SourceEnum.Input);
            //streamOut.Port.SetPortValue(ePropID.T, 273.15+25, SourceEnum.Transferred);
            streamOut.FlashAllPorts();
        }

        internal double FlowEstimate(enumflowtype flowType, double mW, double sG)
        {
            double res = double.NaN;
            switch (flowType)
            {
                case enumflowtype.Molar:
                    res = MoleFlow;
                    break;

                case enumflowtype.Mass:
                    res = MoleFlow * drawTray.MW();
                    break;

                case enumflowtype.StdLiqVol:
                    res = MoleFlow * drawTray.MW() / drawTray.SG();
                    break;
            }
            return res;
        }

        internal void EraseStreams()
        {
            streamIn.Ports.Clear();
            StreamOut.Ports.Clear();
        }

        public Enthalpy ReturnEnthalpyCalc(Column col, Specification s)
        {
            Temperature Tt;

            switch (s.engineSpecType)
            {
                case eSpecType.PADeltaT:
                    Tt = drawTray.T - (Temperature)s.SpecValue;
                    ReturnEnthalpy = EnthalpyEstimate(col, Tt);
                    break;

                case eSpecType.PADuty:
                    ReturnEnthalpy = drawTray.LiqEnthalpy(col, drawTray.T) - (Enthalpy)s.SpecValue;
                    break;

                case eSpecType.PARetT:
                    Tt = new(s.SpecValue);
                    ReturnEnthalpy = EnthalpyEstimate(col, Tt);
                    break;

                default:
                    ReturnEnthalpy = new Enthalpy(double.NaN);
                    break;
            }
            //ReturnEnthalpy = 0; 
            return ReturnEnthalpy;
        }

        public Enthalpy EnthalpyEstimate(Column col, Temperature T)
        {
            Enthalpy res = double.NaN;
            switch (col.SolverOptions.LiqEnthalpyMethod)
            {
                case ColumnEnthalpyMethod.SimpleLinear:
                    res = enthalpySimpleLinearisation.LiqEstimate(T, drawTray.MW());
                    break;

                case ColumnEnthalpyMethod.BostonBrittHdep:
                    res = enthalpyDepartureLinearisation.LiqEnthalpy(col.Components, drawTray.LiqComposition, returnTray.P.BaseValue, T);
                    break;
            }
            return res;
        }

        public Enthalpy EnthalpyRigorous(Column col, Temperature T)
        {
            Port_Material p = streamIn.Port.Clone();
            p.Clear();
            p.cc = col.Components.Clone();
            p.cc.SetMolFractions(drawTray.LiqComposition);
            p.cc.Origin = SourceEnum.Input;
            p.SetPortValue(ePropID.P, returnTray.P, SourceEnum.Input);
            p.SetPortValue(ePropID.MOLEF, 1, SourceEnum.Input);
            p.SetPortValue(ePropID.T, T, SourceEnum.Input);
            p.Flash();

            return p.H_.BaseValue;
        }
    }
}