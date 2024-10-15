using ModelEngine;
using System;

namespace ModelEngineTest
{
    public class CondenserTest : UnitOperation
    {
        public bool HasWaterDraw = true, HasLiquidDraw = true;
        public COMCondType CondenserType = COMCondType.Partial;

        public Port_Material In = new Port_Material("In", FlowDirection.ALL);
        public Port_Material Vap = new Port_Material("Vap", FlowDirection.OUT);
        public Port_Material Liq = new Port_Material("Liq", FlowDirection.OUT);
        public Port_Material Water = new Port_Material("Water", FlowDirection.OUT);
        public Port_Energy Q = new("Duty", FlowDirection.OUT);

        public CondenserTest() : base()
        {
            Add(In);
            Add(Vap);
            Add(Liq);
            Add(Water);
            Add(Q);
        }

        public Guid TraySectionGuid { get; internal set; }
    }
}