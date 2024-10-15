using ModelEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    public interface IUnitOperation
    {
        string Name { get; set; }

        void Add(BaseComp bc);

        void AddComponents(Components comps);

        void AddInletPort(Guid guid);

        void AddOutletPort(Guid guid);

        void CleanUp();

        void EraseNonFixedValues();

        int FlashAllPorts();

        double[] GetCompositionValues(Port_Material i);

        double[] GetCompositionValues(string i);

        void GetObjectData(SerializationInfo info, StreamingContext context);

        string GetPath();

        List<string> GetPortNames(FlowDirection direction);

        PortList GetPorts(FlowDirection fd);

        double GetPropValue(Port_Material i, ePropID id);

        double GetPropValue(string pname, ePropID id);

        void SetCompositionValue(Port_Material port, string name, double value, SourceEnum source = SourceEnum.Input);

        void SetCompositionValue(string port, string name, double value, SourceEnum source = SourceEnum.Input);

        void SetCompositionValues(string pname, List<double> comps, SourceEnum origin);

        bool Solve();

        //   void TransferPortData(bool IgnoreInputs = false);
        Guid Guid { get; set; }

        PortList Ports { get; set; }
        
        bool IsActive { get; set; }
        bool IsDirty { get; set; }
        bool IsSolved { get; }

        void EraseEstimates();

        void EraseCalcEstimates();

        void EraseNonFixedComponents();
    }
}