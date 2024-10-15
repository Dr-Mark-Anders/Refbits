using ModelEngine;
using Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    public enum eFittingsTypes
    { Elbows90LR, Elbows45LR, TeesThru, TeesBranch, BallValves, ButterflyValves, GateValves, GlobeValves, CheckValves, PlugValves, PipeEntrance }

    [Serializable]
    public partial class PipeFittings : ISerializable
    {
        public string schedule;
        public string NomPipeSize;
        public UOMProperty PipeLength = new UOMProperty(ePropID.Length);
        public UOMProperty StaticHead = new UOMProperty(ePropID.Length);
        public UOMProperty P = new UOMProperty(ePropID.P);
        private UOMProperty innerDiameter = new UOMProperty(ePropID.Length);
        public UOMProperty InnerDiameter { get => innerDiameter; set => innerDiameter = value; }

        public Dictionary<string, int> FittingTypes = new Dictionary<string, int>{ { "Elbows90LR", 0 }, {"Elbows45LR", 0 }
            ,{"TeesThru",0 }, {"TeesBranch",0 }, {"BallValves",0 }, {"ButterflyValves",0 }, {"GateValves",0 },
            { "GlobeValves",0 }, {"CheckValves",0 }, {"PlugValves",0 }, {"PipeEntrance",0 } };

        public PipeFittings(int elbows90LR, int elbows45LR, int teesThru, int teesBranch, int ballValves, int butterflyValves, int gateValves, int globeValves, int checkValves, int plugValves, int pipeEntrance)
        {
            FittingTypes["Elbows90LR"] = elbows90LR;
            FittingTypes["Elbows45LR"] = elbows45LR;
            FittingTypes["TeesThru"] = teesThru;
            FittingTypes["TeesBranch"] = teesBranch;
            FittingTypes["BallValve"] = ballValves;
            FittingTypes["ButterflyValves"] = butterflyValves;
            FittingTypes["GateValves"] = gateValves;
            FittingTypes["GlobeValves"] = globeValves;
            FittingTypes["CheckValves"] = checkValves;
            FittingTypes["PlugValves"] = plugValves;
            FittingTypes["PipeEntrance"] = pipeEntrance;
        }

        public PipeFittings()
        {
        }

        public void setfittings()
        {
        }

        public static UOMProperty GetPipeInsideDiamter(string pipediameter, string pipeschedule)
        {
            if (pipediameter == "")
                return null;

            PipeFittings pf = new PipeFittings();

            int SizeIndex = pf.PipeIndex.IndexOf(pipediameter);
            int scheduleindex;
            if (pipeschedule != "")
            {
                scheduleindex = pf.Schedule[pipeschedule];
                return new UOMProperty(ePropID.Length, SourceEnum.UnitOpCalcResult, pf.PipeinternalSizes[SizeIndex, scheduleindex] * 25.4 / 1000);
            }
            return null;
        }

        public double ReynoldsNumber(double density, double PipeId_mm, double VolFlowrate_m3_hr, double viscosity_cp = 1)
        {
            double velocity = VolFlowrate_m3_hr / ((PipeId_mm / 1000) / 2).Sqr() / 3600; // m/s
            return (PipeId_mm / 1000) * velocity * density * 1000 / viscosity_cp;
        }

        public void PipePressureDrop(double PipeID, double ReynoldsNo)
        {
        }

        public double K(double Reynolds, double PipeID, PipeFitting pipefitting)
        {
            return pipefitting.ParamA / Reynolds + pipefitting.ParmaB * (1 + pipefitting.ParamC / PipeID);
        }

        public PipeFittings(SerializationInfo info, StreamingContext context)
        {
            FittingTypes = (Dictionary<string, int>)info.GetValue("FittingTypes", typeof(Dictionary<string, int>));
            try
            {
                schedule = info.GetString("schedule");
                NomPipeSize = info.GetString("NomPipeSize");
                PipeLength = (UOMProperty)info.GetValue("PipeLength", typeof(UOMProperty));
                StaticHead = (UOMProperty)info.GetValue("StaticHead", typeof(UOMProperty));
                P = (UOMProperty)info.GetValue("P", typeof(UOMProperty));
                InnerDiameter = (UOMProperty)info.GetValue("InnerDiameter", typeof(UOMProperty));
            }
            catch
            {
            }
            if (innerDiameter is null)
                innerDiameter = new UOMProperty(ePropID.Length);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FittingTypes", FittingTypes);
            info.AddValue("schedule", schedule); ;
            info.AddValue("NomPipeSize", NomPipeSize);
            info.AddValue("PipeLength", PipeLength, typeof(UOMProperty));
            info.AddValue("StaticHead", StaticHead, typeof(UOMProperty));
            info.AddValue("P", P, typeof(UOMProperty));
            info.AddValue("InnerDiameter", InnerDiameter, typeof(UOMProperty));
        }
    }
}