using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Units.UOM;

namespace ModelEngine
{
    public partial class Components
    {
        // API[1983,3:14A1.3]

        /*public virtualvoid  CalcLiquidSH(Temperature T,Pressure  P,Quality Q)
        {
        if(double .IsNaN(Q))//stateundefined
        return  ;

        UpdateLiqVapFractionCompositions(Q);
        double OilPhaseMW=LiquidComponents.MW_Dry();

        if(LiquidComponents.Contains("H2O"))
        {
        aqueusPhase=true;
        BaseCompwater=LiquidComponents["H2O"];

        double DissolvedWaterMassFrac=LiquidComponents.WaterSolubilityMassFrac(T);//kg/kg
        double TotalWaterMassFrac=water.MoleFraction*water.MW/OilPhaseMW;//kg/kg
        double FreeWaterMassFrac=TotalWaterMassFrac-DissolvedWaterMassFrac;
        FreeWaterMoleFrac=FreeWaterMassFrac*(OilPhaseMW/water.MW);

        if(FreeWaterMassFrac>=0)
        LiquidComponents.NormaliseMassFracsFixOneComp("H2O",DissolvedWaterMassFrac);//moles
        else
        LiquidComponents.NormaliseMassFracsFixOneComp("H2O",TotalWaterMassFrac);//moles

        LiquidComponents.UpdateComponentFractions(FlowFlag.Mass);

        waterphase=new ();
        waterphase.Add(water.Clone());

        waterphase["H2O"].MoleFraction=1;

        waterphase.UpdateComponentFractions(FlowFlag.Molar);

        waterphase.T=this.T;
        waterphase.P.SetValue(this.P);
        waterphase.Q.SetValue(0D);

        this.thermoWater=Thermodynamicsclass .CalcSinglePhaseEnthalpyAndEntropy(waterphase,P,T,enumFluidRegion.Liquid);
        }
        else
        aqueusPhase=false;

        if(Q>0.99999)
        aqueusPhase=false;

        this.thermoLiq=Thermodynamicsclass .CalcSinglePhaseEnthalpyAndEntropy(LiquidComponents,P,T,enumFluidRegion.Liquid);

        return  ;
        }*/

        public double WaterSolubilityMassFrac(Temperature TempC)
        {
            double res = 0;

            foreach (BaseComp bc in CompList)//ignorewater
            {
                if (bc.Name != "H2O")
                    res += bc.WaterSolubilityWTPct(TempC) * bc.MassFraction;
            }

            BaseComp wwater = GetComponent("H2O");

            double waterflow;
            if (wwater != null)
                waterflow = wwater.MassFraction;
            else
                waterflow = 0;

            if (res > waterflow)
            {
                res = waterflow;
                AqueusPhase = false;
            }
            else
            {
                AqueusPhase = true;
            }

            return res;//massfraction
        }

        ///<summary>
        ///changepropvalueoraddnew property
        ///</summary>
        ///<paramname="prop"></param>
        ///<paramname="val"></param>
        public void SetValues(enumAssayPCProperty prop, List<double> val)
        {
            for (int i = 0; i < CompList.Count; i++)
            {
                if (CompList[i].Properties.ContainsKey(prop))
                    CompList[i].Properties[prop] = val[i];
                else
                    CompList[i].Properties.Add(prop, val[i]);
            }
        }

        [Browsable(true)]
        public SourceEnum Origin
        {
            get { return origin; }

            set { origin = value; }
        }

        public int? GetWaterCompNo()
        {
            for (int i = 0; i < CompList.Count; i++)
            {
                if (CompList[i].Name == "H2O" || CompList[i].Name == "WATER")
                    return i;
            }
            return null;//nowater
        }

        ///<summary>
        ///CalculateonDemand
        ///</summary>

        public void Insert(BaseComp bc)
        {
            CompList.Insert(0, bc);
        }

        private Guid originID;
        public bool FirstChanged;
        private double[] pCritBarAArray;
        private double[] tCritKArray;
        private double[] vCritArray;
        private double[] mWArray;

        [Browsable(true)]
        public double[] VapPhaseMolFractions
        {
            get
            {
                double[] MolFracs = new double[Count];

                for (int n = 0; n < CompList.Count; n++)
                {
                    BaseComp bc = CompList[n];
                    MolFracs[n] = bc.MoleFraction * bc.MoleFracVap;
                }

                return MolFracs.Normalise();
            }
        }

        [Browsable(true)]
        public double[] LiqPhaseMolFractions
        {
            get
            {
                double[] MolFracs = new double[Count];

                for (int n = 0; n < CompList.Count; n++)
                {
                    BaseComp bc = CompList[n];
                    MolFracs[n] = bc.MoleFraction * (1 - bc.MoleFracVap);
                }
                return MolFracs.Normalise();
            }
        }

        [Browsable(true)]
        public double[] VapPhaseMassFractions
        {
            get
            {
                double[] MassFracs = new double[Count];

                for (int n = 0; n < CompList.Count; n++)
                {
                    BaseComp bc = CompList[n];
                    MassFracs[n] = bc.MassFraction * bc.MoleFracVap;
                }

                return MassFracs.Normalise();
            }
        }

        [Browsable(true)]
        public double[] LiqPhaseMassFractions
        {
            get
            {
                double[] MassFracs = new double[Count];

                for (int n = 0; n < CompList.Count; n++)
                {
                    BaseComp bc = CompList[n];
                    MassFracs[n] = bc.MassFraction * (1 - bc.MoleFracVap);
                }
                return MassFracs.Normalise();
            }
        }

        [Browsable(true)]
        public double[] VapPhaseVolFractions
        {
            get
            {
                double[] VolFracs = new double[Count];

                for (int n = 0; n < CompList.Count; n++)
                {
                    BaseComp bc = CompList[n];
                    VolFracs[n] = bc.MoleFraction * bc.MoleFracVap;
                }

                return VolFracs.Normalise();
            }
        }

        [Browsable(true)]
        public double[] LiqPhaseVolFractions
        {
            get
            {
                double[] VolFracs = new double[Count];

                for (int n = 0; n < CompList.Count; n++)
                {
                    BaseComp bc = CompList[n];
                    VolFracs[n] = bc.STDLiqVolFraction * (1 - bc.MoleFracVap);
                }
                return VolFracs.Normalise();
            }
        }

        [Browsable(true)]
        public double[] MoleFractions
        {
            get
            {
                double[] MolFracs = new double[Count];

                for (int n = 0; n < CompList.Count; n++)
                    MolFracs[n] = CompList[n].MoleFraction;

                return MolFracs;
            }
        }

        [Browsable(true)]
        public double[] MolFractionsZeroOutSolids
        {
            get
            {
                double[] MolFracs = new double[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                {
                    if (CompList[n] is SolidComponent sc)
                    {
                        MolFracs[n] = 0;
                    }
                    else
                        MolFracs[n] = CompList[n].MoleFraction;
                }

                return MolFracs;
            }
        }

        public double[] MassFractions
        {
            get
            {
                double[] MassFracs = new double[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                    MassFracs[n] = CompList[n].MassFraction;

                return MassFracs;
            }
        }

        public double[] VolFractions
        {
            get
            {
                double[] VolFracs = new double[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                    VolFracs[n] = CompList[n].STDLiqVolFraction;

                return VolFracs;
            }
        }

        public double[] VolFractionsCumulative
        {
            get
            {
                if (CompList.Count == 0)
                    return null;

                double[] VolFracs = new double[CompList.Count];
                this.UpdateVolFractions();//dataonlystoredasmolefractions
                VolFracs[0] = CompList[0].STDLiqVolFraction;
                for (int n = 1; n < CompList.Count; n++)
                    VolFracs[n] = VolFracs[n - 1] + CompList[n].STDLiqVolFraction;

                return VolFracs;
            }
        }

        private void UpdateVolFractions()
        {
            double VolSum = 0;
            for (int i = 0; i < List.Count; i++)
            {
                VolSum += List[i].MoleFraction * List[i].MW / List[i].SG_60F;
            };

            for (int i = 0; i < List.Count; i++)
            {
                List[i].STDLiqVolFraction = List[i].MoleFraction * List[i].MW / List[i].SG_60F / VolSum;
            };
        }

        public bool IsDefined
        {
            get
            {
                if (CompList.Count == 0)
                    return
                    false;

                if (MoleFractions is not null && MoleFractions.Contains(double.NaN))
                    return false;

                if (origin == SourceEnum.Empty)
                    return false;

                return true;
            }
        }

        public bool MoleFractionsValid
        {
            get
            {
                return !MoleFractions.Contains(double.NaN);
            }
        }

        public Temperature VolAveBP()
        {
            Temperature sum = new(0);
            double totlvpct = 0;
            for (int i = 0; i < CompList.Count; i++)
            {
                sum += CompList[i].STDLiqVolFraction * CompList[i].BP;
                totlvpct += CompList[i].STDLiqVolFraction;
            }
            return new(sum / totlvpct);
        }

        public bool Equals(Components obj)
        {
            if (obj == null)
                return false;

            if (obj is not Components cc)
                return false;
            else
            {
                if (cc.CompList.Count != this.Count)
                    return false;
                for (int n = 0; n < Count; n++)
                    if (!CompList[n].MoleFraction.AlmostEquals(cc[n].MoleFraction, 0.00001) || CompList[n].Name != cc[n].Name)
                        return false;
                return true;
            }
        }

        public BaseComp GetComponent(string compname)
        {
            int count = CompList.Count;

            for (int i = 0; i < count; i++)
            {
                if (CompList[i].Name == compname)
                    return CompList[i];
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return CompList.GetEnumerator();
        }

        public List<BaseComp> List
        {
            get
            {
                return CompList;
            }
            set
            {
                CompList = value;
            }
        }

        public int Count
        {
            get
            {
                return CompList.Count;
            }
        }

        public int CountIncSolids
        {
            get
            {
                return CompList.Count;
            }
        }

        public Guid OriginPortGuid
        { get => originPortGuid; set => originPortGuid = value; }

        public double FreeWaterMoleFlow1 { get => FreeWaterMoleFlow; set => FreeWaterMoleFlow = value; }
        public double FreeWaterMoleFrac { get; private set; }

        [Browsable(true)]
        public List<BaseComp> ComponentList { get => CompList; set => CompList = value; }

        // public Components LiquidComponents
        // { get => liquidComponents; set => liquidComponents = value; }

        // public Components VapourComponents
        // { get => vapourComponents; set => vapourComponents = value; }

        public AssayPropertyCollection Apc
        { get => apc; set => apc = value; }

        public double[] PCritBarAArray { get => pCritBarAArray; set => pCritBarAArray = value; }
        public double[] TCritKArray { get => tCritKArray; set => tCritKArray = value; }
        public double[] VCritArray { get => vCritArray; set => vCritArray = value; }
        public double[] MWArray { get => mWArray; set => mWArray = value; }
        public double[] SGArray { get => sGArray; set => sGArray = value; }

        public Guid Guid
        { get => guid; set => guid = value; }

        public void SetLiqVolFlows(List<double> property)
        {
            for (int i = 0; i < property.Count; i++)
                CompList[i].STDLiqVolFraction = property[i];
        }

        public double SetMolarFlows(double[] property)
        {
            double sum = property.Sum();

            for (int i = 0; i < property.Length; i++)
                CompList[i].MoleFraction = property[i] / sum;

            return sum;
        }

        private Dictionary<string, int> index = new Dictionary<string, int>(); //StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, int> Index { get => index; set => index = value; }
        public ThermoDynamicOptions Thermo { get => thermo; set => thermo = value; }

        public int FastIndex(string name)
        {
            if (index.ContainsKey(name)) return index[name];
            else if (Thermodata.ShortNames.TryGetValue(name, out string realname)) return index[realname];
            return -1;
        }

        ///<summary>
        ///OnlyClonesComponenents
        ///</summary>
        ///<return  s></return  s>
        public Components Clone()
        {
            Components cc = new();
            cc.Thermo = this.thermo;
            for (int n = 0; n < CompList.Count; n++)
            {
                switch (CompList[n])
                {
                    case SolidComponent sc:
                        cc.Add(sc.Clone());
                        break;

                    case BaseComp bc:
                        cc.Add(bc.Clone());
                        break;
                }
            }

            cc.origin = this.origin;
            cc.index = this.index;

            return cc;
        }

        public void Clear()
        {
            CompList.Clear();
            origin = SourceEnum.Empty;
            index.Clear();
        }

        public void ClearMolarComposition()
        {
            foreach (var item in CompList)
                item.MoleFraction = double.NaN;

            origin = SourceEnum.Empty;
            originID = Guid.Empty;
        }

        public Components Pure()
        {
            Components p = new();
            foreach (BaseComp pc in CompList)
                if (pc.IsPure)
                    p.Add(pc);
            return p;
        }

        public Components Pseudo()
        {
            Components p = new();
            foreach (BaseComp pc in CompList)
                if (!pc.IsPure)
                    p.Add(pc);
            return p;
        }

        public BaseComp Add(BaseComp bc)//addorcopyifallreadypresent.
        {
            switch (bc)
            {
                case null:
                    return null;

                case SolidComponent:
                    hasSolids = true;
                    if (!CompList.Contains(bc))
                    {
                        CompList.Add(bc);
                        UpdateArrayData();
                        UpdateIndex();
                        return bc;
                    }
                    break;

                default:
                    if (!CompList.Contains(bc))
                    {
                        CompList.Add(bc);
                        UpdateArrayData();
                        UpdateIndex();
                        return bc;
                    }

                    break;
            }
            return null;
        }

        public void UpdateIndex()
        {
            index.Clear();
            int i;
            for (i = 0; i < CompList.Count; i++)
            {
                index.Add(CompList[i].Name, i);
            }
        }

        public BaseComp Add(String pc)
        {
            //Thermodatadata=new Thermodata();
            //BaseCompbc=data.GetEnryptedPureComponent(pc);

            BaseComp bc = Thermodata.GetComponent(pc);

            if (bc != null)
            {
                if (CompList.Contains(bc))
                {
                    bc.TempMolarFlow += bc.TempMolarFlow;
                    return CompList[CompList.IndexOf(bc)];
                }
                else
                {
                    bc.UpateBWRS();
                    this.Add(bc);
                    return bc;
                }
            }
            return null;
        }

        public void Add(Components cc)
        {
            foreach (BaseComp bc in cc)
                Add(bc.Clone());
        }

        public string[] Names()
        {
            int n = CompList.Count;
            string[] res = new string[n];

            for (int i = 0; i < n; i++)
                res[i] = CompList[i].Name;

            return res;
        }

        public void ReplaceRange(Components pcs)
        {
            CompList.Clear();
            foreach (BaseComp pc in pcs)
                this.CompList.Add(pc.Clone());
        }

        public void AddProperty(enumAssayPCProperty prop, List<double> proplist)
        {
            if (proplist is null)
                return;

            Debug.Assert(proplist.Count == CompList.Count);

            for (int i = 0; i < CompList.Count; i++)
                CompList[i].AddProperty(prop, proplist[i]);//densityhandleddifferentlyhere(double [])
        }

        public bool Contains(BaseComp pc)
        {
            if (this.CompList.Contains(pc))
                return true;
            else
                return false;
        }

        public bool Contains(string pc)
        {
            return index.ContainsKey(pc);
        }

        ///<summary>
        ///
        ///</summary>
        ///<paramname="BaseComponenet"></param>
        public virtual void Remove(BaseComp pc)
        {
            this.CompList.Remove(pc);
        }

        public virtual void Remove(String pc)
        {
            foreach (BaseComp b in CompList)
            {
                if (pc == b.Name)
                {
                    CompList.Remove(b);
                    return;
                }
            }
        }

        public void RemoveAt(int loc)
        {
            if (CompList.Count > loc)
                this.CompList.RemoveAt(loc);
            return;
        }

        public BaseComp this[int index]
        {
            get
            {
                if (this.CompList.Count <= index)
                    return null;
                else
                    return this.CompList[index];
            }
            set
            {
                this.CompList[index] = value;
            }
        }

        /* public BaseComp this[string name]
         {
             get
             {
                 if (components.Count == 0)
                     return null;
                 else
                     return components.Find(x => x.Name.ToUpper() == name.ToUpper());
             }
         }*/

        public BaseComp this[BaseComp index]
        {
            get
            {
                if (CompList.Count == 0)
                    return null;
                else
                    return CompList[CompList.IndexOf(index)];
            }
        }

        //ImplementationofinterfaceICustomTypeDescriptor

        public void SetAllComponentsToVapour()
        {
            foreach (BaseComp bc in CompList)
                bc.MoleFracVap = 1;
        }

        public void SetAllComponentsToLiquid()
        {
            foreach (BaseComp bc in CompList)
                bc.MoleFracVap = 0;
        }

        public void ClearThermoValues()
        {
            if (thermoLiq != null)
                thermoLiq.Clear();
            if (thermoVap != null)
                thermoVap.Clear();
        }
    }
}