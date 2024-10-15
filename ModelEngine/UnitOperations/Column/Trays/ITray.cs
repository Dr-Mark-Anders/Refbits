using ModelEngine;
using System;
using System.Drawing;
using System.Runtime.Serialization;
using Units.UOM;

namespace ModelEngine
{
    public interface ITray
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

        double LiqEnthalpy(Components cc, Temperature T, ColumnEnthalpyMethod method);

        void LiqEnthalpyDepartureCoefsEstimate(Column column, Components cc, double Tbase, double Testimate);

        void UpdateEnthalpyCoeefs(Column column, Components cc, double Tbase, double Testimate);

        double VapEnthalpy(Components cc, Temperature T, ColumnEnthalpyMethod method, ThermoDynamicOptions thermo);

        void VapEnthalpyDepartureCoefsEstimate(Column column, Components cc, double Tbase, double Testimate);
    }
}