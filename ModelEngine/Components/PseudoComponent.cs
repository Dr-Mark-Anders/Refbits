using Extensions;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class PseudoComponent : BaseComp, ISerializable, IComp, IEnumerable, ICloneable, IEquatable<BaseComp>
    {
        private double[] pcprops1;

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public PseudoComponent(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public double[] PCProps1
        {
            get { return pcprops1; }
            set { pcprops1 = value; }
        }

        public new bool Equals(BaseComp other)
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

        private double refenthalpydep1, refenthalpydep3, refentropydep1;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Temperature MeABP
        {
            get { return BP; }
        }

        public override Temperature MidBP
        {
            get
            {
                return BP;
            }
        }

        public Temperature VABP
        {
            get { return MeABP; }
        }

        public double LMPercent
        {
            get { return MassFraction; }
            set { MassFraction = value; }
        }

        public PseudoComponent()
        {
            IsPure = false;
        }

        public PseudoComponent(string Name, double ICP, double ECP)
        {
            IsPure = false;
        }

        public PseudoComponent(Temperature MeABP, double SG, ThermoDynamicOptions thermo)
        {
            this.IsPure = false;
            this.SG_60F = SG;
            this.BP = MeABP;
            this.MW = PropertyEstimation.CalcMW(MeABP, SG, thermo);
            //this.HForm25 = -1826.1 * this.MW + 1E-05;  both seem reasoanble
            //this.HForm25 = 0.0002 * this.MW.Pow(3) - 0.219 * this.MW.Sqr() - 1724.2 * this.MW - 4046.8;//-5E-17 * this.MW.Pow(3) - 9E-11 * this.MW.Sqr() - 0.0006 * this.MW - 6.8558;
            this.HForm25 = -208541.6 / 114.2 * this.MW; // to match Hysys
            this.CritT = PropertyEstimation.CalcCritT(MeABP, SG, MW, thermo);
            this.CritP = PropertyEstimation.CalcCritP(MeABP, SG, MW, thermo);
            if (CritP < 5) // otherwife fugacity calcualtion go wild.
                CritP = 5;
            this.Omega = PropertyEstimation.CalcOmega(MeABP, SG, CritT, CritP, thermo);
            this.CritZ = PropertyEstimation.CalcCritZ(this.Omega);
            this.CritV = PropertyEstimation.CalcCritV(CritP, CritT, CritZ, MeABP, SG, thermo);  // L/mole
            this.Name = "Quasi" + MeABP.ToString();
            this.IdealVapCP = LeeKesler.GetIdealVapCpCoefficients(this.Omega, this.Wk, this.SG_60F);
        }

        public PseudoComponent(Temperature MeABP, double SG, double MW, double Omega, Temperature CriticalT,
            Pressure CriticalP, double CritV, ThermoDynamicOptions thermo)
        {
            this.IsPure = false;
            this.SG_60F = SG;
            this.BP = MeABP;
            this.MW = MW;
            //this.HForm25 = -1826.1 * this.MW + 1E-05;  both seem reasoanble
            //this.HForm25 = 0.0002 * this.MW.Pow(3) - 0.219 * this.MW.Sqr() - 1724.2 * this.MW - 4046.8;//-5E-17 * this.MW.Pow(3) - 9E-11 * this.MW.Sqr() - 0.0006 * this.MW - 6.8558;
            this.HForm25 = -208541.6 / 114.2 * this.MW;// to match Hysys
            this.CritT = CriticalT;
            this.CritP = CriticalP;
            this.Omega = Omega;
            this.CritZ = PropertyEstimation.CalcCritZ(this.Omega);
            this.CritV = CritV; // this.CritZ * Rgas * this.CritT / this.CritP;  // L/mole
            this.Name = "Quasi" + MeABP.ToString();
            this.IdealVapCP = LeeKesler.GetIdealVapCpCoefficients(this.Omega, this.Wk, this.SG_60F);
        }

        public PseudoComponent(string Name, Temperature MeABP, double SG, double MW, double Omega, Temperature CriticalT,
            Pressure CriticalP, double CritV, double Hform25, double[] Cp)
        {
            this.IsPure = false;
            this.SG_60F = SG;
            this.BP = MeABP;
            this.MW = MW;
            this.HForm25 = Hform25;
            this.CritT = CriticalT;
            this.CritP = CriticalP;
            this.Omega = Omega;
            this.CritZ = PropertyEstimation.CalcCritZ(this.Omega);
            this.CritV = CritV; // this.CritZ * Rgas * this.CritT / this.CritP;  // L/mole
            this.Name = Name;
            this.IdealVapCP = Cp;
        }

        public PseudoComponent(string Name, Temperature MeABP, double SG, double MW, double Omega, Temperature CriticalT,
            Pressure CriticalP, double CritV, double Hform25)
        {
            this.IsPure = false;
            this.SG_60F = SG;
            this.BP = MeABP;
            this.MW = MW;
            this.HForm25 = Hform25;
            this.CritT = CriticalT;
            this.CritP = CriticalP;
            this.Omega = Omega;
            this.CritZ = PropertyEstimation.CalcCritZ(this.Omega);
            this.CritV = CritV; // this.CritZ * Rgas * this.CritT / this.CritP;  // L/mole
            this.Name = Name;
            this.IdealVapCP = LeeKesler.GetIdealVapCpCoefficients(this.Omega, this.Wk, this.SG_60F);
        }

        public PseudoComponent(double SG, Temperature MeABP, string name, ThermoDynamicOptions thermo) : this(MeABP, SG, thermo)
        {
            this.Name = name;
        }

        public PseudoComponent(double SG_Density, Temperature NBP, Temperature LBP, Temperature UBP, string name, ThermoDynamicOptions thermo)
        {
            this.IsPure = false;
            if (SG_Density > 5) // input as density not SG;
                this.SG_60F = SG_Density / 1000;
            else
                this.SG_60F = SG_Density;

            this.BP = NBP;
            this.LBP = LBP;
            this.UBP = UBP;
            this.MW = PropertyEstimation.CalcMW(BP, SG_60F, thermo);
            this.HForm25 = -1826.1 * this.MW + 1E-05;

            this.CritT = PropertyEstimation.CalcCritT(BP, SG_60F, MW, thermo);
            this.CritP = PropertyEstimation.CalcCritP(BP, SG_60F, MW, thermo);
            this.Omega = PropertyEstimation.CalcOmega(BP, SG_60F, CritT, CritP, thermo);
            this.CritZ = PropertyEstimation.CalcCritZ(this.Omega);
            this.CritV = PropertyEstimation.CalcCritV(CritP, CritT, CritZ, MeABP, SG_60F, thermo);

            this.Name = name;

            this.IdealVapCP = LeeKesler.GetIdealVapCpCoefficients(this.Omega, this.Wk, this.SG_60F);
        }

        public double CalcIdealGasCp(double Wk, double MW, double SG, double Tcent)
        {
            double a, b, g, Cpg, Tr, Tk, res = 0;
            IdealHeatCapMethod t = IdealHeatCapMethod.RefBits;

            switch (t)
            {
                case IdealHeatCapMethod.LeeKesler:
                    Tk = Tcent + 273.17;
                    double A0 = -1.41779 + 0.11828 * Wk;
                    double A1 = -(6.99724 - 8.69326 * Wk + 0.27715 * Wk.Pow(2)) * 1e-4;
                    double A2 = -2.2582 * 1e-6;
                    double B0 = 1.09223 - 2.48245 * Omega;
                    double B1 = -(3.434 - 7.14 * Omega) * 1e-3;
                    double B2 = -(7.2661 - 9.2561 * Omega) * 1e-7;
                    double C = ((12.8 - Wk) * (10 - Wk) / 10 * Omega).Pow(2);

                    Cpg = MW * (A0 + A1 * Tk + A2 * Tk.Pow(2)
                        - C * (B0 + B1 * Tk + B2 * Tk.Pow(2)));  // KJ/Kg/C
                    res = Cpg;
                    break;

                case IdealHeatCapMethod.API:
                case IdealHeatCapMethod.Cavett:// Cavett 1962
                    Tr = (Tcent + 273.15) * 9 / 5;
                    a = (0.036863384 * Wk - 0.4673722) * MW;
                    b = (3.1865 * SG - 5 * Wk + 0.001045186) * MW;
                    g = -4.9572 * SG - 7 * MW;
                    Cpg = a + b * Tr * +g * Tr.Pow(2);    //BTU /lb Molr
                    Cpg = Cpg * 1.05505585 * 2.204 * MW;  // Convert to KJ/kg/C
                    res = Cpg;
                    break;

                case IdealHeatCapMethod.RefBits:
                    res = 9E-07 * BP.Pow(2) - 0.0003 * BP + 0.0534;  // very simple based on Tboil only do not use
                    res = res * MW; //Kj/kg/C
                    break;
            }
            return res;
        }

        public double WaterSolubility() //API Technical databook procedure 9A1.4).
        {
            return 0;
        }

        public new PseudoComponent Clone()
        {
            PseudoComponent p = new PseudoComponent();
            p.Name = this.Name;
            p.IsPure = this.IsPure;
            p.CritT = this.CritT;
            p.CritP = this.CritP;
            p.CritV = this.CritV;
            p.CritZ = this.CritZ;
            p.Omega = this.Omega;
            p.MW = this.MW;
            p.SG_60F = this.SG_60F;
            p.MoleFracVap = this.MoleFracVap;
            p.refenthalpydep1 = this.refenthalpydep1;
            p.refentropydep1 = this.refentropydep1;
            p.refenthalpydep3 = this.refenthalpydep3;
            p.HForm25 = this.HForm25;
            p.IdealVapCP = this.IdealVapCP;
            return p;
        }

        public double CalcMolWt(double SG, Temperature MeABPT, ThermoDynamicOptions thermo)  // APIMethod
        {
            double res = 0;

            switch (thermo.MW_Method)
            {
                case enumMW_Method.LeeKesler:
                    res = LeeKesler.MW(MeABPT, SG);
                    break;

                case enumMW_Method.RiaziDaubert:
                    res = RiaziDaubert.MW(MeABPT, SG);
                    break;
            }
            return res;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class PseudoCompList : IEnumerable, ICloneable
    {
        private ArrayList pp = new ArrayList();

        public object Clone()
        {
            PseudoCompList pcl = new PseudoCompList();
            foreach (BaseComp pc in pp)
                pcl.add((BaseComp)pc.Clone());

            return pcl;
        }

        public object CloneDeep()
        {
            PseudoCompList pcl = new PseudoCompList();
            foreach (BaseComp pc in pp)
                pcl.add((BaseComp)pc.Clone());

            return pcl;
        }

        public void add(BaseComp o)
        {
            //o.owner=this;
            pp.Add(o);
        }

        public bool MoveNext()
        {
            return true;
        }

        public IEnumerator GetEnumerator()
        {
            return pp.GetEnumerator();
        }

        public BaseComp this[int index]   // indexer declaration
        {
            get
            {
                if (index <= pp.Count && pp.Count != 0)
                    return (BaseComp)pp[index];
                else
                    return new BaseComp();
            }
            set
            {
                pp[index] = value;
            }
        }

        public void SortSG()
        {
            pp.Sort();
        }

        public int Count
        {
            get
            {
                return pp.Count;
            }
        }
    }

    public class PseudoCompConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            //Thermodata p = new Thermodata();
            return null;//new StandardValuesCollection(p.GetComponents.ToArray());
        }
    }
}