using System;
using System.Collections;
using System.Runtime.Serialization;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class SolidComponent : BaseComp, ISerializable, IEnumerable, ICloneable, IEquatable<SolidComponent>
    {
        private double[] pcprops1;
        private MassHeatCapacity cPMass;
        private new string name;

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        public SolidComponent(SerializationInfo info, StreamingContext context)
        {
        }

        public double[] PCProps1
        {
            get { return pcprops1; }
            set { pcprops1 = value; }
        }

        public MassHeatCapacity CPMass { get => cPMass; set => cPMass = value; }

        public bool Equals(SolidComponent other)
        {
            if (this.Name == other.Name)
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            BaseComp personObj = obj as BaseComp;
            if (personObj == null)
                return false;
            else
                return Equals(personObj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public SolidComponent()
        {
            CPMass = 1;
            MW = 1;
            SG_60F = 0.76;
        }

        public SolidComponent(string Name, Density SG, MassHeatCapacity CpMass)
        {
            this.SG_60F = SG.SG;
            this.Name = Name;
            this.CPMass = CpMass;
            this.MW = 1;
            this.BP = 4000;
            this.MoleFracVap = 0;
            this.CritP = 1000;
            this.CritT = new Temperature(10000,TemperatureUnit.Celsius);
            this.Omega = 0;
            this.CritV = 1;
        }

        public double WaterSolubility() //API Technical databook procedure 9A1.4).
        {
            return 0;
        }

        public new SolidComponent Clone()
        {
            SolidComponent p = new SolidComponent();
            p.Name = this.Name;
            p.SG_60F = this.SG_60F;
            p.MW = MW;
            p.MoleFraction = this.MoleFraction;
            p.SG_60F = this.SG_60F;
            p.BP = 4000;
            p.MoleFracVap = 0;
            p.CritP = this.CritP;
            p.CritT = this.CritT;
            p.CritV = this.CritV;
            p.Omega = 0;
            return p;
        }

        public double CalcMolWt(double SG, Temperature MeABPT, ThermoDynamicOptions thermo)  // APIMethod
        {
            double res = 1;
            return res;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }

        internal double Enthalpy(Temperature t)
        {
            return (t.Celsius - 25) * CPMass;
        }
    }
}