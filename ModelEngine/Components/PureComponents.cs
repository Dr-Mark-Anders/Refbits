using System;
using System.ComponentModel;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class PureComp : BaseComp, ICloneable, IComp //, ISerializable
    {
        private double refenthalpydep1, refenthalpydep3, refentropydep1;

        public PureComp()
        {
        }

        public PureComp(string Name, Temperature CritT, double CritP, double Omega, double[] Antk, double[] VapEnth, double MW)
        {
            this.Name = Name;
            this.CritT = CritT;
            this.CritP = CritP;
            this.Omega = Omega;
            this.AntK = Antk;
            this.IdealVapCP = VapEnth;
            this.MW = MW;
            this.SG_60F = 1;
            this.MoleFracVap = double.NaN;
        }

        public double VCp(Temperature t)
        {
            double VE;
            VE = IdealVapCP[2]
                + 2 * IdealVapCP[3] * Math.Pow(t.Rankine, 1)
                + 3 * IdealVapCP[4] * Math.Pow(t.Rankine, 2)
                + 4 * IdealVapCP[5] * Math.Pow(t.Rankine, 3)
                + 5 * IdealVapCP[6] * Math.Pow(t.Rankine, 4);

            VE = VE * 2.326 * 1.8; // kJ/kg/C
            return VE;
        }

        public new PureComp Clone()
        {
            PureComp p = new();
            p.Name = this.Name;
            p.IsPure = this.IsPure;
            p.CritT = this.CritT;
            p.CritP = this.CritP;
            p.CritZ = this.CritZ;
            p.CritV = this.CritV;
            p.Omega = this.Omega;
            p.AntK = this.AntK;
            p.IdealVapCP = this.IdealVapCP;
            p.MW = this.MW;
            p.SG_60F = this.SG_60F;
            p.BP = this.BP;
            p.MoleFracVap = this.MoleFracVap;
            p.refenthalpydep1 = this.refenthalpydep1;
            p.refentropydep1 = this.refentropydep1;
            p.refenthalpydep3 = this.refenthalpydep3;
            p.HForm25 = this.HForm25;
            return p;
        }

        public new Temperature CritT
        {
            get { return base.CritT; }
            set { base.CritT = value; }
        }
    }

    public class PureComponentConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            Thermodata p = new();
            return new(p.GetComponentNames.ToArray());
        }
    }
}