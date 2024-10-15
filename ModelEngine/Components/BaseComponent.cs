using Extensions;
using ModelEngine.ThermoData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using Units.UOM;

namespace ModelEngine
{
    [TypeConverter(typeof(BaseComp)), Description("ExpandToSeeComponent")]
    [Serializable]
    public class BaseCompExpander : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }
    }

    [TypeConverter(typeof(List<BaseComp>)), Description("ExpandToSeeComponent")]
    [Serializable]
    public class BaseListCompExpander : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }
    }

    [TypeConverter(typeof(BaseComp)), Serializable]
    public class BaseComp : BaseObject, IConsistencyProperty, ICloneable, IComp, IComparable, IEqualityComparer<BaseComp>, ISerializable
    {
        private Temperature critT = new();//kelvin
        private double gform;
        private Temperature bp_k;
        private Temperature ubp, lbp;
        private double sG_60F;
        private double tempvalue;
        private double fractvap = 0;
        public double[] idealvapCp;
        private double liqVolFraction;
        private double[] props;
        private double vap_sg;
        private string sourcecrude = "";
        private double visc, thermcond;
        private double tempFraction = 0;
        public int RxStoichiometry = 0;
        private Pressure critP;
        private double omega;
        private double refenthalpydep1, refenthalpydep3, refentropydep1;
        private double lk_Z, lk_Vc;//LeeKeslervalues
        private bool ispure = true;
        private string formula;
        public object CompType { get; private set; }

        private string unifac;
        private double unifacR, unifacQ;
        private string cas;
        private double[] gibbsfree;

        private MixedComponent mix;

        public bool IsMixed
        {
            get
            {
                return mix != null;
            }
        }

        public double Bo = double.NaN; // propanae by default.
        public double Ao = double.NaN;
        public double Co = double.NaN;
        public double Do = double.NaN;
        public double Eo = double.NaN;
        public double b = double.NaN;
        public double a = double.NaN;
        public double d = double.NaN;
        public double alpha = double.NaN;
        public double c = double.NaN;
        public double gamma = double.NaN;

        public override string ToString()
        {
            return this.name;
        }

        private Dictionary<enumAssayPCProperty, double> properties = new();

        [Browsable(true)]
        public Dictionary<enumAssayPCProperty, double> Properties { get => properties; set => properties = value; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IsPure", IsPure);
            info.AddValue("CritT", CritT);
            info.AddValue("CritP", CritP);
            info.AddValue("CritZ", CritZ);
            info.AddValue("CritV", CritV);
            info.AddValue("Omega", Omega);
            info.AddValue("AntK", AntK);
            info.AddValue("VapEnth", IdealVapCP);
            info.AddValue("MW", molwt);
            info.AddValue("SG", SG_60F);
            info.AddValue("BP", BP);
            info.AddValue("FracVap", MoleFracVap);
            info.AddValue("refenthalpydep1", refenthalpydep1);
            info.AddValue("refentropydep1", refentropydep1);
            info.AddValue("refenthalpydep3", refenthalpydep3);
            info.AddValue("HForm25", HForm25);
            info.AddValue("GForm25", GForm25);
            info.AddValue("MoleFraction", molefraction);
            info.AddValue("MassFraction", Massfraction);
            info.AddValue("VolFraction", STDLiqVolFraction);
            info.AddValue("Formula", Formula);
            info.AddValue("CAS", cas);
            info.AddValue("properties", properties);
            info.AddValue("GibbsCoeff", gibbsfree);

            base.GetObjectData(info, context);
        }

        internal Entropy Sform()
        {
            Entropy res = (GForm25 - HForm25) / 298;
            return res;
        }

        public double StandardMolarVolume
        {
            get
            {
                return MW / SG_60F;
            }
        }

        public virtual double IdealGasCp(Temperature T, enumMassOrMolar mm)
        {
            double Cp;
            double t = T.Kelvin;

            Cp = 0;
            if (IdealVapCP != null)
            {
                Cp = IdealVapCP[0]
                + IdealVapCP[1] * t
                + IdealVapCP[2] * Math.Pow(t, 2)
                + IdealVapCP[3] * Math.Pow(t, 3)
                + IdealVapCP[4] * Math.Pow(t, 4);

                if (mm == enumMassOrMolar.Molar)
                    Cp *= MW;//Molar
            }
            return Cp;
        }

        public double GetProperty(enumAssayPCProperty prop)
        {
            if (properties.TryGetValue(prop, out double res))
                return res;
            else
                return double.NaN;
        }

        public void AddProperty(enumAssayPCProperty prop, double value)
        {
            switch (prop)
            {
                case enumAssayPCProperty.DENSITY15:
                    Density = value;
                    break;

                case enumAssayPCProperty.MW:
                    MW = value;
                    break;

                default:
                    if (properties.ContainsKey(prop))
                        properties[prop] = value;
                    else
                        properties.Add(prop, value);
                    break;
            }
        }

        private static readonly Dictionary<Elements, double> entropyformation = new()
        {
            //J/mol
            {Elements.C,5.74},
            {Elements.H2,130.68},
            {Elements.O2,205.15},
            {Elements.S,32.1},
            {Elements.N2,191.6},
            {Elements.Cl2,223.1},
            {Elements.Br2,245.5}
        };

        private double critZ;
        private double tempMolarFlow;
        private double tempMassFlow;
        private double tempVolFlow;
        private double molwt;
        private double hForm25;
        private double hVap25;
        private double Massfraction;
        private double molefraction;
        private double[] antK;
        public bool IsSSolid = false;

        public static Gibbs GibbsFormation(BaseComp comp)
        {
            Dictionary<string, int> elements = comp.GetElements();
            double products = 0;
            double feed;
            foreach (var item in elements)
            {
                switch (item.Key)
                {
                    case "C":
                        products += entropyformation[Elements.C] * item.Value;
                        break;

                    case "H":
                        products += entropyformation[Elements.H2] / 2 * item.Value;
                        break;

                    case "O":
                        products += entropyformation[Elements.O2] / 2 * item.Value;
                        break;

                    case "S":
                        products += entropyformation[Elements.S] * item.Value;
                        break;

                    case "Cl":
                        products += entropyformation[Elements.Cl2] / 2 * item.Value;
                        break;

                    case "Br":
                        products += entropyformation[Elements.Br2] / 2 * item.Value;
                        break;

                    default:
                        break;
                }
            }

            Components cc = new();
            cc.Add(comp);

            feed = IdealGas.StreamIdealGasMolarEntropy(cc, 1, 298.12, cc.MoleFractions);

            Gibbs res = products - feed;
            return res;
        }

        public Dictionary<string, int> GetElements()
        {
            Dictionary<string, int> composition = new();

            string form = Formula.Trim();
            int loc;
            int x;
            int NoofAtoms;

            string[] comps = Enum.GetNames(typeof(Atoms));

            for (int i = 0; i < comps.Length; i++)
            {
                NoofAtoms = 1;
                x = 1;
                loc = form.IndexOf(comps[i]);
                if (loc == -1)
                {
                    composition[comps[i].ToString()] = 0;
                }
                else
                {
                    int count = 0;
                    if (form.Length > loc + 1 && int.TryParse(form[loc + 1].ToString(), out int val))
                    {
                        NoofAtoms = val;
                        int res = val;
                        do
                        {
                            if (form.Length > loc + 2)
                            {
                                x++;
                                if (int.TryParse(form[loc + 2].ToString(), out int finalres))
                                {
                                    res *= 10 + finalres;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            count++;
                        } while (count < 3);
                    }
                    composition[comps[i].ToString()] = NoofAtoms;
                }
            }
            return composition;
        }

        public void ReEstimateCriticalProps(ThermoDynamicOptions thermo = null)
        {
            if (thermo == null)
                thermo = new();
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;
            thermo.CritTMethod = enumCritTMethod.LeeKesler;
            thermo.CritPMethod = enumCritPMethod.LeeKesler;
            thermo.CritZMethod = enumCritZMethod.LeeKesler;
            thermo.CritVMethod = enumCritVMethod.LeeKesler;

            this.CritT = PropertyEstimation.CalcCritT(BP, SG_60F, MW, thermo);
            this.CritP = PropertyEstimation.CalcCritP(BP, SG_60F, MW, thermo);
            this.Omega = PropertyEstimation.CalcOmega(BP, SG_60F, CritT, CritP, thermo);
            this.CritZ = PropertyEstimation.CalcCritZ(this.Omega);
            this.CritV = PropertyEstimation.CalcCritV(CritP, CritT, CritZ, MeABP, SG_60F, thermo);
        }

        [Browsable(false)]
        public Enthalpy HForm25
        {
            get { return hForm25; }
            set { hForm25 = value; }
        }

        [Browsable(false)]
        public double[] IdealVapCP
        {
            get { return idealvapCp; }
            set { idealvapCp = value; }
        }

        public int CompareTo(object obj)
        {
            if (name == ((BaseComp)obj).name)
                return 0;
            return 1;
        }

        ///<summary>
        ///cm3
        ///</summary>
        ///<paramname="MW"></param>
        ///<paramname="SG"></param>
        ///<return  s></return  s>
        public double LiquidMolarVolumeCM3()
        {
            return MW / SG_60F;
        }

        ///<summary>
        ///SIUnits
        ///</summary>
        ///<paramname="HeatVap"></param>
        ///<paramname="BoilinPoint "></param>
        ///<paramname="LiquidMolarVolume"></param>
        ///<return  s></return  s>
        public double HildebrandSolPar()
        {
            //1cal1/2cm−3/2=(4.184J)1/2(0.01m)−3/2=2.045×103Pa1/2
            const double R = 8.314;
            double res = Math.Pow((HeatVap25() - R * this.bp_k) / this.LiquidMolarVolumeCM3(), 0.5);//UnitsMPa^0.5/2.045for(cal/cm3)^0.5
            return res;
        }

        ///<summary>
        ///VetreMethod
        ///</summary>
        ///<return  s></return  s>
        public double HeatVap25()
        {
            double Tbr, Pc, R = 8.3144;
            double res;
            Pc = CritP;
            Tbr = bp_k / CritT;

            //VetreMethod
            res = R * CritT * Tbr * (0.4343 * Math.Log(Pc) - 0.68859 + 0.89584 * Tbr) / (0.37691 - 0.37306 * Tbr + 0.14878 / Pc / Tbr / Tbr);

            //Watsontoconvertto25C
            double Tr25 = (273.15 + 25) / CritT;
            res *= Math.Pow((1 - Tr25) / (1 - Tbr), 0.375);

            return res;
        }

        ///<summary>
        ///VetereMethod
        ///</summary>
        ///<return  s></return  s>
        public double HeatVapEstimate(Temperature T)
        {
            enumReboil HeatVapMethod = enumReboil.Mehmendoust;

            double Tbr, Pc;
            double res;
            Pc = CritP;
            double Tb = bp_k;
            Tbr = bp_k / CritT;

            switch (HeatVapMethod)
            {
                case enumReboil.Vetere79://=8.3144*Tb*(1-Tbr)^0.38*(LN(Pc-0.513+0.5066*Tc^2/(Pc*Tb^2))/(1-Tbr+(1-(1-Tbr)^0.38)*LN(Tbr)))
                    res = ThermodynamicsClass.Rgas * this.bp_k * (1 - Tbr).Pow(0.38) * (Math.Log(Pc - 0.513 + 0.5066 * CritT / (Pc * Tb.Pow(2)))
                    / (1 - Tbr + (1 - (1 - Tbr).Pow(0.38)) * Math.Log(Tbr)));
                    break;

                case enumReboil.Mehmendoust://mostaccurate,published2014
                    double A = 0.0129;
                    double b1 = 0.00086;
                    double b2 = -0.00206;
                    double b3 = 0.0115;
                    double c1 = -0.01983;
                    double c2 = 0.00632;
                    double c3 = -0.04279;
                    double d1 = 0.02086;
                    double d2 = -0.00459;
                    double d3 = 0.03544;

                    double B = b1 + b2 * Pc + b3 * Math.Log(Pc);
                    double C = c1 + c2 * Pc + c3 * Math.Log(Pc);
                    double D = d1 + d2 * Pc + d3 * Math.Log(Pc);

                    res = (A + B * Tbr + C * Tbr.Pow(2) + D * Tbr.Pow(3)) * 8.3145 * Tb;
                    res *= 1000;
                    break;

                default:
                    res = 0;
                    break;
            }

            //WatsontoconverttoT
            double Tr = T / CritT;
            if (Tr <= 1)
                res *= Math.Pow((1 - Tr) / (1 - Tbr), 0.375);
            return res;
        }

        public double WaterSolubilityWTPct(Temperature Tk)
        {
            double a = -34.6210000153573;
            double b = -0.00370000000002736;
            double c = 6.30833054765296E-06;
            double mw = this.MW;

            double A = a + b * mw + c * mw * mw;
            double B = 391.742;
            double C = 14.3711;

            double res = Math.Pow(10, (A + B / Tk + C * Math.Log10(Tk))) / 1000000;

            return res;
        }

        [Browsable(false)]
        public double Wk
        {
            get { return MeABP.Rankine.Pow(1.0 / 3.0) / SG_60F; }
        }

        public double TRed(Temperature T)
        {
            return T / CritT;
        }

        public double PRed(Pressure P)
        {
            return P / CritP;
        }

        public virtual Temperature MeABP
        {
            get
            {
                return this.BP;
            }
        }

        [Browsable(false)]
        public override double Value
        {
            get
            {
                if (double.IsNaN(molefraction))
                    return double.NaN;
                return molefraction;
            }
            set
            {
                if (value is double.NaN)
                {
                    molefraction = double.NaN;
                }
                else
                {
                    molefraction = value;
                }
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string SecondaryName
        {
            get;
            set;
        }

        public double GetFraction(flowBasis basis)
        {
            return basis switch
            {
                flowBasis.mass => Massfraction,
                flowBasis.molar => molefraction,
                flowBasis.volume => STDLiqVolFraction,
                _ => double.NaN,
            };
        }

        [Browsable(false)]
        public double LiqVolumePercent
        {
            get { return STDLiqVolFraction * 100; }
            set { STDLiqVolFraction = value / 100; }
        }

        [Browsable(false)]
        public double Thermcond
        {
            get { return thermcond; }
            set { thermcond = value; }
        }

        [Browsable(false)]
        public double Visc
        {
            get { return visc; }
            set { visc = value; }
        }

        [Browsable(false)]
        public double[] Props
        {
            get { return props; }
            set { props = value; }
        }

        [Browsable(false)]
        public string SourceCrude
        {
            get { return sourcecrude; }
            set { sourcecrude = value; }
        }

        [Browsable(false)]
        public bool IsPure
        {
            get { return ispure; }
            set { ispure = value; }
        }

        [Browsable(false)]
        public virtual double CritZ
        {
            get
            {
                if (critZ == 0)
                {
                    return (CritP.BaseValue * CritV * 1000) / (ThermodynamicsClass.Rgas * CritT.BaseValue) / 10;//Rgasshouldbem3barK-1mol-1
                }
                return critZ;
            }
            set { critZ = value; }
        }

        [Browsable(false)]
        public virtual double CritV { get; set; }

        [Browsable(false)]
        public double VAP_SG
        {
            get
            {
                if (vap_sg == 0)
                {
                    vap_sg = 1;
                }
                return SG_60F;
            }
            set
            {
                vap_sg = value;
            }
        }

        [Browsable(false)]
        public double RefEntropyDep1
        {
            get { return refentropydep1; }
            set { refentropydep1 = value; }
        }

        [Browsable(false)]
        public double RefEnthalpyDep3
        {
            get { return refenthalpydep3; }
            set { refenthalpydep3 = value; }
        }

        [Browsable(false)]
        public double RefEnthalpyDep1
        {
            get { return refenthalpydep1; }
            set { refenthalpydep1 = value; }
        }

        public BaseComp(string Name = "")
        {
            name = Name;
            molefraction = double.NaN;
            Massfraction = double.NaN;
            STDLiqVolFraction = double.NaN;
        }

        public BaseComp(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            IsPure = (bool)info.GetValue("IsPure", typeof(bool));
            CritT = (double)info.GetValue("CritT", typeof(double));
            CritP = (double)info.GetValue("CritP", typeof(double));
            CritZ = (double)info.GetValue("CritZ", typeof(double));
            CritV = (double)info.GetValue("CritV", typeof(double));
            Omega = (double)info.GetValue("Omega", typeof(double));
            AntK = (double[])info.GetValue("AntK", typeof(double[]));
            IdealVapCP = (double[])info.GetValue("VapEnth", typeof(double[]));
            molwt = (double)info.GetValue("MW", typeof(double));
            SG_60F = (double)info.GetValue("SG", typeof(double));
            BP = (double)info.GetValue("BP", typeof(double));
            MoleFracVap = (double)info.GetValue("FracVap", typeof(double));
            refenthalpydep1 = (double)info.GetValue("refenthalpydep1", typeof(double));
            refentropydep1 = (double)info.GetValue("refentropydep1", typeof(double));
            refenthalpydep3 = (double)info.GetValue("refenthalpydep3", typeof(double));
            HForm25 = (double)info.GetValue("HForm25", typeof(double));
            molefraction = info.GetDouble("MoleFraction");

            try
            {
                Massfraction = info.GetDouble("MassFraction");
                STDLiqVolFraction = info.GetDouble("VolFraction");
                Formula = info.GetString("Formula");
                GForm25 = (double)info.GetValue("GForm25", typeof(double));
                cas = info.GetString("CAS");
                properties = (Dictionary<enumAssayPCProperty, double>)info.GetValue("properties", typeof(Dictionary<enumAssayPCProperty, double>));
                gibbsfree = (double[])info.GetValue("GibbsCoeff", typeof(double[]));
            }
            catch
            {
                if (properties is null)
                    properties = new Dictionary<enumAssayPCProperty, double>();
            }
        }

        public BaseComp(Temperature LBP, Temperature UBP)
        {
            this.lbp = LBP;
            this.ubp = UBP;
            name = "Quasi" + LBP + "_" + ubp;
            this.BP = new Temperature((LBP + UBP) / 2);
        }

        public void Erase()
        {
            molefraction = double.NaN;
            Massfraction = double.NaN;
            STDLiqVolFraction = double.NaN;
        }

        public void SetZero()
        {
            molefraction = 0;
            Massfraction = 0;
            STDLiqVolFraction = 0;
        }

        //CASNumber,Name,Formula,Type,UNIFAC,MW,SG6060,TB,Tc,Pc,Vc,OMEGA,CPA,CPB,CPC,CPD,CPE,HFORM25,GFORM25,GIBBSA,GIBBSB,GIBBSC
        public BaseComp(string cas, string name, string Formula, string Type, string UNIFAC, double MW, double SG6060, double TB, double Tc, double Pc,
        double Vc, double OMEGA, double CPA, double CPB, double CPC, double CPD, double CPE, double HFORM25, double GFORM25, double GIBBSA,
        double GIBBSB, double GIBBSC) : base(name)
        {
            this.cas = cas;
            this.name = name;
            this.formula = Formula;
            this.CompType = Type;
            this.unifac = UNIFAC;
            this.MW = MW;
            this.SG_60F = SG6060;
            this.bp_k = TB;
            this.CritT = Tc;
            this.CritP = Pc;
            this.CritV = Vc;
            this.Omega = OMEGA;
            this.IdealVapCP = new double[] { CPA, CPB, CPC, CPD, CPE };
            this.HForm25 = HFORM25;
            this.GForm25 = GFORM25;
            this.GibbsFree = new double[] { GIBBSA, GIBBSB, GIBBSC };
        }

        public BaseComp Clone()
        {
            BaseComp p = new();
            p.name = this.name;
            p.formula = this.formula;
            p.ispure = this.ispure;
            p.BP = this.BP;
            p.ubp = this.ubp;
            p.lbp = this.lbp;
            p.CritT = this.CritT;
            p.CritP = this.CritP;
            p.CritZ = this.CritZ;
            p.CritV = this.CritV;
            p.Omega = this.Omega;
            p.AntK = this.AntK;
            p.IdealVapCP = this.IdealVapCP;
            p.molwt = this.molwt;
            p.SG_60F = this.SG_60F;
            p.fractvap = this.fractvap;
            p.refenthalpydep1 = this.refenthalpydep1;
            p.refentropydep1 = this.refentropydep1;
            p.refenthalpydep3 = this.refenthalpydep3;
            p.HForm25 = this.HForm25;
            p.GForm25 = this.GForm25;
            p.gibbsfree = this.gibbsfree;
            p.STDLiqVolFraction = this.STDLiqVolFraction;
            p.Massfraction = this.Massfraction;
            p.molefraction = this.molefraction;
            p.TempMolarFlow = this.TempMolarFlow;
            p.properties = new(this.Properties);
            p.cas = this.cas;
            p.unifac = this.unifac;
            p.unifacQ = this.unifacQ;
            p.unifacR = this.unifacR;
            p.UniquacQ = this.UniquacQ;
            p.UniquacR = this.UniquacR;
            p.SecondaryName = SecondaryName;
            p.IsSSolid = IsSSolid;
            return p;
        }

        object ICloneable.Clone()
        {
            BaseComp p = this.Clone();
            return p;
        }

        object IComp.Clone()
        {
            throw new NotImplementedException();
        }

        [Browsable(false)]
        public double MoleFracVap
        {
            get { return fractvap; }
            set { fractvap = value; }
        }

        [Browsable(false)]
        public virtual double MW
        {
            get { return molwt; }
            set { molwt = value; }
        }

        [Browsable(false)]
        public double HVap25
        {
            get
            {
                return hVap25;
            }

            set
            {
                hVap25 = value;
            }
        }

        [Browsable(false)]
        public double TempValue
        {
            get
            {
                return tempvalue;
            }

            set
            {
                tempvalue = value;
            }
        }

        [Browsable(false)]
        public double LK_Z
        {
            get
            {
                return lk_Z;
            }

            set
            {
                lk_Z = value;
            }
        }

        [Browsable(false)]
        public double LK_Vc
        {
            get
            {
                return lk_Vc;
            }

            set
            {
                lk_Vc = value;
            }
        }

        [Browsable(false)]
        public Temperature BP
        {
            get
            {
                return bp_k;
            }
            set
            {
                bp_k = value;
            }
        }

        [Browsable(false)]
        public virtual Temperature MidBP
        {
            get
            {
                return BP;
            }
        }

        [Browsable(false)]
        public Temperature UBP
        {
            get
            {
                if (ispure)
                    return BP;
                else
                    return ubp;
            }
            set
            {
                ubp = value;
            }
        }

        [Browsable(false)]
        public Temperature LBP
        {
            get
            {
                if (ispure)
                    return BP;
                else
                    return lbp;
            }
            set
            {
                lbp = value;
            }
        }

        [Browsable(false)]
        private Guid Origin
        {
            get { return guid; }

            set => guid = value;
        }

        [Browsable(false)]
        public virtual double Density { get => SG_60F * Constants.WaterSpecGravityAt60F; set => SG_60F = value / Constants.WaterSpecGravityAt60F; }

        [Browsable(false)]
        public string Formula { get => formula; set => formula = value; }

        [Browsable(false)]
        public Gibbs GForm25
        { get => gform; set => gform = value; }

        [Browsable(false)]
        public string CAS { get => cas; set => cas = value; }

        [Browsable(false)]
        public double[] GibbsFree { get => gibbsfree; set => gibbsfree = value; }

        [Browsable(false)]
        public virtual double Omega { get => omega; set => omega = value; }

        [Browsable(false)]
        public double TempMolarFlow { get => tempMolarFlow; set => tempMolarFlow = value; }

        [Browsable(false)]
        public double TempMassFlow { get => tempMassFlow; set => tempMassFlow = value; }

        [Browsable(false)]
        public double TempVolFlow { get => tempVolFlow; set => tempVolFlow = value; }

        [Browsable(false)]
        public double TempFraction { get => tempFraction; set => tempFraction = value; }

        [Browsable(false)]
        public virtual Temperature CritT { get => critT; set => critT = value; }

        [Browsable(true)]
        public virtual double Molwt { get => molwt; set => molwt = value; }

        [Browsable(false)]
        public double Gform { get => gform; set => gform = value; }

        [Browsable(false)]
        public double MassFraction { get => Massfraction; set => Massfraction = value; }

        [Browsable(true)]
        public double MoleFraction { get => molefraction; set => molefraction = value; }

        [Browsable(false)]
        public double Tempvalue { get => tempvalue; set => tempvalue = value; }

        [Browsable(false)]
        public double STDLiqVolFraction { get => liqVolFraction; set => liqVolFraction = value; }

        [Browsable(false)]
        public virtual Pressure CritP { get => critP; set => critP = value; }

        [Browsable(false)]
        public double[] AntK { get => antK; set => antK = value; }

        [Browsable(false)]
        public double[] IdealvapCp { get => idealvapCp; set => idealvapCp = value; }

        [Browsable(false)]
        public virtual double SG_60F { get => sG_60F; set => sG_60F = value; }

        public string Unifac { get => unifac; set => unifac = value; }
        public double UnifacR { get => unifacR; set => unifacR = value; }
        public double UnifacQ { get => unifacQ; set => unifacQ = value; }
        public double UniquacR { get; internal set; }
        public double UniquacQ { get; internal set; }
        internal MixedComponent Mix { get => mix; set => mix = value; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseComp);
        }

        public bool Equals(BaseComp obj)
        {
            if (obj == null) return false;

            if (obj.name == this.name) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(BaseComp x, BaseComp y)
        {
            return x.guid == y.guid;
        }

        public int GetHashCode([DisallowNull] BaseComp obj)
        {
            return obj.GetHashCode();
        }

        public static bool operator ==(BaseComp left, BaseComp right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(BaseComp left, BaseComp right)
        {
            return !(left == right);
        }

        public static bool operator <(BaseComp left, BaseComp right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        public static bool operator <=(BaseComp left, BaseComp right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(BaseComp left, BaseComp right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(BaseComp left, BaseComp right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }

        public bool HasEqualProps(BaseComp bc)
        {
            if (this.critP != bc.critP)
                return false;
            if (this.critT != bc.critT)
                return false;
            if (this.critZ != bc.critZ)
                return false;
            if (this.omega != bc.omega)
                return false;
            if (this.molwt != bc.molwt)
                return false;
            if (this.Density != bc.Density)
                return false;

            return true;
        }

        internal void UpateBWRS()
        {
            string expression = "Name = '" + Name + "'";
            DataRow[] foundRows;

            // Use the Select method to find all rows matching the filter.

            foundRows = BWRSData.BWRSdata.Select(expression);
            Ao = GetProp(foundRows[0], "BWRS_A0");
            Bo = GetProp(foundRows[0], "BWRS_B0");
            Co = GetProp(foundRows[0], "BWRS_C0");
            Do = GetProp(foundRows[0], "BWRS_D0");
            Eo = GetProp(foundRows[0], "BWRS_E0");
            a = GetProp(foundRows[0], "BWRS_a");
            b = GetProp(foundRows[0], "BWRS_b");
            c = GetProp(foundRows[0], "BWRS_c");
            d = GetProp(foundRows[0], "BWRS_d");
            alpha = GetProp(foundRows[0], "BWRS_alpha");
            gamma = GetProp(foundRows[0], "BWRS_gamma");
        }

        public static double GetProp(DataRow row, string property)
        {
            object o;

            if (BWRSData.BWRSdata.Columns.Contains(property))
                o = row[property];
            else
                return 0;

            if (o.GetType() == typeof(double))
            {
                return (double)o;
            }
            if (o.GetType() == typeof(string))
            {
                double res;
                if (double.TryParse(o.ToString(), out res))
                    return Convert.ToDouble(res);
                else
                    return 0;
            }
            return 0;
        }

        public double RI20
        {
            get
            {
                return 1 + 0.8447 * (this.Density / 1000).Pow(1.2056) * (this.Molwt + 273.16).Pow(-0.0557) * molwt.Pow(-0.0044);
            }
        }

        public double ARI
        {
            get
            {
                double RI20 = this.RI20;
                double DRI20 = (RI20.Pow(2) - 1) / (RI20.Pow(2) + 2);
                double res = 2 * (molwt / DRI20 - (3.5419 * molwt + 73.1858))
                / ((3.5074 * molwt - 91.972) - (3.5149 * molwt + 73.1858));
                return res;
            }
        }

        /// <summary>
        /// total method
        /// </summary>
        /// <returns></returns>
        public double H2Wtpct()  // Warning Haven't done Pure Comps yet
        {
            TotalCorrelation tot = new();
            double H2 = tot.H2wt(SG_60F,this.MW,this.Sulfur,this.RI20, this.visc, this.MeABP);
            return tot.H2;
        }

        public double Sulfur
        {
            get
            {
                if (properties.ContainsKey(enumAssayPCProperty.SULFUR))
                    return properties[enumAssayPCProperty.SULFUR];
                else return 0;
            }
        }

        public double Nitrogen
        {
            get
            {
                if (properties.ContainsKey(enumAssayPCProperty.N2))
                    return properties[enumAssayPCProperty.N2];
                else return 0;
            }
        }

        public Enthalpy HeatFormationHess()
        {
            double CO2HForm = -393.79;
            double H2OHForm = -285.83;

            SpecificEnergy HHV = HigherHeatValue();  // mass or mole ?????
            MassFraction H = H2Wtpct();
            MassFraction C = (double)(100 - H - Sulfur - Nitrogen);

            double ToMoles = H + C / 12 + Sulfur / 32;

            double H2Molepct = H / ToMoles;
            double SMolePCT = Sulfur / 32;
            double CMolePCT = 100 - H2Molepct - SMolePCT;

            double HFormGases = CMolePCT * CO2HForm + H2Molepct * H2OHForm;

            double HCHForm = HFormGases - HHV;

            return HCHForm;
        }

        // API[1983,3:14A1.3]
        public SpecificEnergy LowerHeatValue()  // Warning Haven't done Pure Comps yet
        {
            SpecificEnergy e;
            double api = API;
            double Se = Sulfur - 0.0015 * api * api - 0.1282 * api + 2.9479;  // minus typical sulfur content.
            double X = 16796 + 54.5 * api - 0.217 * Math.Pow(api, 2) - 0.0019 * Math.Pow(api, 3);
            double res = X * (1 - 0.01 * Se) + 40.5 * Se;
            e = new SpecificEnergy(res, SpecificEnergyUnit.btu_lb);
            return e;
        }

        public SpecificEnergy HigherHeatValue()  // Warning Haven't done Pure Comps yet
        {
            SpecificEnergy e;
            double api = API;
            var res = 17672 + 66.6 * api - 0.316 * api.Pow(2) - 0.0014 * api.Pow(3);
            e = new SpecificEnergy(res, SpecificEnergyUnit.btu_lb);
            return e;
        }

        public double API
        {
            get
            {
                return 141.5 / (this.Density / 1000) - 131.5; ;
            }
        }

        Guid IConsistencyProperty.Origin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}