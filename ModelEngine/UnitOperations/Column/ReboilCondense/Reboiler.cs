using System;

namespace ModelEngine
{
    public class Reboiler : UnitOperation
    {
        private ReboilerType reboilerType = ReboilerType.HeatEx;



        public Port_Material In = new Port_Material("In", FlowDirection.ALL);
        public Port_Material Vap = new Port_Material("Vap", FlowDirection.OUT);
        public Port_Material Liq = new Port_Material("Liq", FlowDirection.OUT);
        public Port_Material Water = new Port_Material("Water", FlowDirection.OUT);
        public Port_Energy Q = new("Duty", FlowDirection.OUT);

        public Reboiler() : base()
        {
            Add(In);
            Add(Vap);
            Add(Liq);
            Add(Water);
            Add(Q);
        }

        public object Specs { get; set; }
        public Guid TraySectionGuid { get; set; }
        public ReboilerType ReboilerType { get => reboilerType; set => reboilerType = value; }
    }

    public enum ReboilerOptions
    {
        NoReboiler,
        FixedTemperature,
        FixaedVapourRate,
        FixedDuty
    }
}