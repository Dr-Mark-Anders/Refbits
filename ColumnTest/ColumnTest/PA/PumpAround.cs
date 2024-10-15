using ModelEngine;
using ModelEngine;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngineTest
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PumpAroundTest : ISerializable
    {
        private bool isactive = true;

        public PumpAroundTest(TraySectionTest traysection, string Name = "")
        {
            drawSection = traysection;
            returnSection = traysection;
            guid = Guid.NewGuid();
            name = Name;
        }

        private StreamMaterial streamIn = new();
        private StreamMaterial streamOut = new();

        public TrayTest drawTray, returnTray;

        //public  int  DrawTrayIndex, returnTrayIndex
        public TraySectionTest drawSection, returnSection;

        public double DrawFactor { get; set; }
        public Temperature ReturnTemp;
        public StreamProperty MoleFlow = new StreamProperty(ePropID.MOLEF, 0.1);
        private Guid guid;
        private string name;
        private Port_Material returnPort = new Port_Material();
        private Port_Material drawPort = new Port_Material();

        public EnthalpyDepartureLinearisation enthalpyDepartureLinearisation = new();
        public EnthalpySimpleLinearisation enthalpySimpleLinearisation = new();

        public override string ToString()
        {
            return this.Name;
        }

        protected PumpAroundTest(SerializationInfo info, StreamingContext context)
        {
            guid = (Guid)info.GetValue("UOGuid", typeof(Guid));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UOGuid", guid);
        }

        public double MW
        {
            get
            {
                if (drawTray != null)
                    return returnPort.MW;
                else
                    return double.NaN;
            }
        }

        public double SG
        {
            get
            {
                if (drawTray != null)
                    return returnPort.SG;
                else
                    return double.NaN;
            }
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
        public Port_Material ReturnPort { get => returnPort; set => returnPort = value; }
        public Port_Material DrawPort { get => drawPort; set => drawPort = value; }
        public bool Isactive { get => isactive; set => isactive = value; }
        public StreamMaterial StreamIn { get => streamIn; set => streamIn = value; }
        public StreamMaterial StreamOut { get => streamOut; set => streamOut = value; }

        public void UpdateStreams()
        {
            streamIn.Port = DrawPort.Clone();
            streamOut.Port.Clear();
            streamOut.Port.cc = DrawPort.cc.Clone();
            streamOut.Port.cc.Origin = SourceEnum.Input;
            streamOut.Port.SetPortValue(ePropID.P, returnPort.P_, SourceEnum.Input);
            streamOut.Port.SetPortValue(ePropID.MOLEF, returnPort.MolarFlow_, SourceEnum.Input);
            streamOut.Port.SetPortValue(ePropID.H, ReturnEnthalpy, SourceEnum.Input);
            //streamOut.Port.SetPortValue(ePropID.T, 273.15+25, SourceEnum.Transferred);
            streamOut.FlashAllPorts();
        }

        internal double FlowEstimate(COMenumflowtype flowType, double mW, double sG)
        {
            double res = double.NaN;
            switch (flowType)
            {
                case COMenumflowtype.Molar:
                    res = MoleFlow;
                    break;

                case COMenumflowtype.Mass:
                    res = MoleFlow * MW;
                    break;

                case COMenumflowtype.StdLiqVol:
                    res = MoleFlow * MW / SG;
                    break;
            }
            return res;
        }

        internal void EraseStreams()
        {
            streamIn.Ports.Clear();
            StreamOut.Ports.Clear();
        }

        public Enthalpy ReturnEnthalpyCalc(COMColumn col, SpecificationTest s)
        {
            Temperature Tt;

            if (drawTray.T >= ReturnTemp.Kelvin)
            {
                Enthalpy enthalpy;
                switch (s.engineSpecType)
                {
                    case COMeSpecType.PADeltaT:
                        Tt = drawTray.T - (Temperature)s.SpecValue;
                        enthalpy = EnthalpyEstimate(col, Tt);
                        break;

                    case COMeSpecType.PADuty:
                        enthalpy = drawTray.LiqEnthalpy(col, drawTray.T) - (Enthalpy)s.SpecValue;
                        break;

                    case COMeSpecType.PARetT:
                        Tt = new(s.SpecValue);
                        enthalpy = EnthalpyEstimate(col, Tt);
                        break;

                    default:
                        enthalpy = new Enthalpy(double.NaN);
                        break;
                }
                ReturnEnthalpy = enthalpy;
                return enthalpy;
            }
            else
            {
                switch (s.engineSpecType)
                {
                    case COMeSpecType.PADeltaT:
                        Tt = drawTray.T - (Temperature)s.SpecValue;
                        ReturnEnthalpy = EnthalpyRigorous(col, Tt);
                        break;

                    case COMeSpecType.PADuty:
                        ReturnEnthalpy = StreamIn.Port.H_ - (Enthalpy)s.SpecValue;
                        break;

                    case COMeSpecType.PARetT:
                        Tt = new(s.SpecValue);
                        ReturnEnthalpy = EnthalpyRigorous(col, Tt);
                        break;

                    default:
                        ReturnEnthalpy = new Enthalpy(double.NaN);
                        break;
                }
            }
            return ReturnEnthalpy;
        }

        public Enthalpy EnthalpyEstimate(COMColumn col, Temperature T)
        {
            Enthalpy res = double.NaN;
            switch (col.SolverOptions.LiqEnthalpyMethod)
            {
                case COMColumnEnthalpyMethod.SimpleLinear:
                    res = enthalpySimpleLinearisation.LiqEstimate(T, MW);
                    break;

                case COMColumnEnthalpyMethod.BostonBrittHdep:
                    res = enthalpyDepartureLinearisation.LiqEnthalpy(col.Components, drawTray.LiqComposition, returnTray.P.BaseValue, T);
                    break;
            }
            return res;
        }

        public Enthalpy EnthalpyRigorous(COMColumn col, Temperature T)
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