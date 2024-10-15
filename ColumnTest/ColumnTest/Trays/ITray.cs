using ModelEngine;
using ModelEngine;
using System;
using System.Drawing;
using System.Runtime.Serialization;
using Units.UOM;

namespace ModelEngineTest
{
    public interface ITrayTest
    {
        Color Colour { get; set; }
        object Destinationguid { get; set; }
        int DestinatonTraySection { get; set; }
        int DrawSection { get; set; }
        double FeedEnthalpyLiq { get; }
        double FeedEnthalpyVap { get; }
        Guid Guid { get; set; }
        bool Hasstream { get; set; }
        float Height { get; set; }
        Rectangle Location { get; set; }
        string Name { get; set; }
        object Originguid { get; set; }

        void BackupFactors();

        bool Contains(Point p);

        double VapEnthDepEstimate(double C, double D, double TC_PC, double Tr, double TrStar);

        double EnthDepLiqCurrent(Components cc);

        double EnthDepVapCurrent(Components cc);

        Port GetFeedStreams(bool v);

        void GetObjectData(SerializationInfo info, StreamingContext context);

        UnitOperation GetParent();

        double LiqEnthalpy(Components cc, Temperature T, COMColumnEnthalpyMethod method);

        void LiqEnthalpyDepartureCoefsEstimate(COMColumn column, Components cc, double Tbase, double Testimate);

        void UpdateEnthalpyCoeefs(COMColumn column, Components cc, double Tbase, double Testimate);

        double VapEnthalpy(Components cc, Temperature T, COMColumnEnthalpyMethod method, ThermoDynamicOptions thermo);

        void VapEnthalpyDepartureCoefsEstimate(COMColumn column, Components cc, double Tbase, double Testimate);
    }
}