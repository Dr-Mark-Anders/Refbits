using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Units.UOM;

public enum enumStreamSpecs
{ P, T, PT, PTQ, Q, QT, QP, PE, TE, SE, ST, None }

namespace ModelEngine
{
    ///<summary>
    ///Typesafecollectionclass forComponentobject s.Extendsthebaseclass
    ///CollectionBasetoinheritbasecollectionfunctionality.
    ///ImplementationofICustomTypeDescriptortoprovidecustomizedtypedescription.
    ///</summary>
    ///

    [TypeConverter(typeof(BaseListCompExpander)), Serializable]
    public partial class Components : ComponentsExpander, ISerializable, IEnumerable, IEquatable<Components>
    {
        private enumFlashAlgorithm flashalgorithm = enumFlashAlgorithm.RR;

        public List<BaseComp> CompList = new();

        //public AllComponentTypesCollection complist = new();

        public Components RemoveSolids(bool Normalise)
        {
            Components res = new Components();

            foreach (BaseComp comp in CompList)
            {
                switch (comp)
                {
                    case SolidComponent solid:
                        break;

                    default:
                        res.Add(comp.Clone());
                        break;
                }
            }

            if (Normalise)
                res.NormaliseFractions();

            return res;
        }

        public Components Solids()
        {
            Components res = new Components();

            foreach (BaseComp comp in CompList)
            {
                switch (comp)
                {
                    case SolidComponent solid:
                        res.Add(comp);
                        break;

                    default:

                        break;
                }
            }
            return res;
        }

        private AssayPropertyCollection apc = new();

        private readonly ComponentBips bipsclass = new();

        private double FreeWaterMoleFlow;

        private ThermoDynamicOptions thermo = new();
        private Guid oilGuid;
        private bool aqueusPhase = false;
        private bool hasSolids = false;

        private enumFluidRegion state = enumFluidRegion.Undefined;

        public double H(Pressure P, Temperature T, Quality Q, double SolidFraction = 0)
        {
            if (Count == 1 && CompList[0] is WaterSteam steam)
                return steam.H(P, T);

            if (Q == 1 && thermoVap != null)
                return thermoVap.H;

            if (Q == 0 && ThermoLiq != null)
                return ThermoLiq.H;

            if (Q > 0 && Q < 1 && thermoVap != null && thermoLiq != null)
            {
                return (thermoVap.H * Q + thermoLiq.H * (1 - Q)) * (1 - SolidFraction) + thermoSolids.H * SolidFraction;
            }
            else
                return double.NaN;
        }

        public double S(Pressure P, Temperature T, Quality Q)
        {
            if (Count == 1 && CompList[0] is WaterSteam steam)
                return steam.S(P, T);

            if (Q == 1 && thermoVap != null)
                return thermoVap.S;
            else if (Q == 0 && ThermoLiq != null)
            {
                return ThermoLiq.S;
            }
            else if (Q > 0 && Q < 1 && thermoVap != null && thermoLiq != null)
            {
                return thermoVap.S * Q + ThermoLiq.S * (1 - Q);
            }
            else
                return double.NaN;
        }

        public double[] synthesiseCUMLVPCts;
        public double[] SynthesiseLVPCts;
        public double[] synthesiseSGs;
        private double labVABP;
        private Guid guid = Guid.NewGuid();
        private SourceEnum origin = SourceEnum.Empty;
        private Guid originPortGuid = Guid.Empty;

        //public Temperature T = new(double.NaN);
        //public Pressure P = new(double.NaN);
        //public Quality Q = new(double.NaN);

        public static bool ContainsTypes(List<BaseComp> list, params Type[] types)
        {
            return types.All(type => list.Any(x => x != null && type == x.GetType()));
        }

        public bool IsPrimary { get; set; }

        private double[] sGArray;

        public int WaterLocation;

        public enumFluidRegion State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public enumCalcResult cres;

        public virtual void GetObjectData(SerializationInfo si, StreamingContext ctx)
        {
            si.AddValue("List", CompList, typeof(List<BaseComp>));
            si.AddValue("Origin", origin);
            si.AddValue("OriginID", originID);
        }

        public Components(SerializationInfo si, StreamingContext ctx)
        {
            CompList.Clear();
            CompList = ((List<BaseComp>)si.GetValue("List", typeof(List<BaseComp>)));

            try
            {
                this.origin = (SourceEnum)si.GetValue("Origin", typeof(SourceEnum));
                this.originID = (Guid)si.GetValue("OriginID", typeof(Guid));
            }
            catch
            {
                this.origin = SourceEnum.Empty;
            }
        }

        [OnDeserialized]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
            if (this.Contains("H2O"))
                WaterLocation = this.IndexOf("H2O");
            else
                WaterLocation = -1;
        }

        public Components(Components feed)
        {
            CompList.AddRange(feed.ComponentList.Clone());
        }

        public Components()
        {
        }

        internal void AddRange(List<BaseComp> compList)
        {
            compList.AddRange(compList);
        }

        public bool Any()
        {
            return CompList.Any();
        }

        public void UpdateArrayData()
        {
            PCritBarAArray = new double[CompList.Count];
            TCritKArray = new double[CompList.Count];
            VCritArray = new double[CompList.Count];
            MWArray = new double[CompList.Count];
            SGArray = new double[CompList.Count];

            for (int i = 0; i < CompList.Count; i++)
            {
                PCritBarAArray[i] = CompList[i].CritP.BaseValue;
                TCritKArray[i] = CompList[i].CritT.BaseValue;
                VCritArray[i] = CompList[i].CritV;
                MWArray[i] = CompList[i].MW;
                SGArray[i] = CompList[i].SG_60F;
            }

            WaterLocation = this.IndexOf("H2O");
        }

        public double[] RemoveWater(double[] X, out double MoleFrac, in int loc)
        {
            MoleFrac = 0;
            double[] Xres = (double[])X.Clone();
            if (loc >= 0)
            {
                MoleFrac = X[loc];
                Xres[loc] = 0;
                Xres = Xres.Normalise();
                return Xres;
            }
            return Xres;
        }

        public void TakeOutWaterComp()
        {
            int loc = this.IndexOf("H2O");
            this.RemoveAt(loc);
            this.NormaliseFractions(FlowFlag.Molar);
        }

        public int IndexOf(BaseComp pc)
        {
            return this.CompList.IndexOf(pc);
        }

        public int IndexOf(String name, double[] Xin, out double X)
        {
            for (int i = 0; i < CompList.Count; i++)
                if (CompList[i].Name == name)
                {
                    X = Xin[i];
                    return i;
                }
            X = 0;
            return -1;
        }

        public int IndexOf(String name)
        {
            for (int i = 0; i < CompList.Count; i++)
                if (CompList[i].Name.ToUpper() == name.ToUpper())
                    return i;
            return -1;
        }

        public BaseComp this[String name]
        {
            get
            {
                return CompList.Find(x => x.Name.ToUpper() == name.ToUpper());
            }
        }

        public double MoleFractionLiquid()
        {
            double res = 0;
            for (int i = 0; i < CompList.Count; i++)
            {
                res = CompList[i].MoleFraction * CompList[i].MoleFracVap;
            }
            return res;
        }

        public void CreateNew(Components cc)
        {
            BaseComp comp;
            foreach (BaseComp item in cc)
            {
                switch (item)
                {
                    case SolidComponent sc:
                        comp = sc.Clone();
                        comp.MoleFraction = 0;
                        List.Add(comp);
                        break;

                    case MixedComponent mc:
                        comp = mc.Clone();
                        comp.MoleFraction = 0;
                        List.Add(comp);
                        break;

                    case BaseComp bc:
                        comp = bc.Clone();
                        comp.MoleFraction = 0;
                        List.Add(comp);
                        break;
                }
            }
        }

        public bool AqueusPhase
        {
            get
            {
                return aqueusPhase;
            }
            set
            {
                aqueusPhase = value;
            }
        }

        public int NoneZeroCompsCount
        {
            get
            {
                int count = 0;
                foreach (var item in CompList)
                {
                    if (item.MoleFraction != 0)
                        count++;
                }
                return count;
            }
        }

        public bool IsInput
        {
            get
            {
                if (this.origin == SourceEnum.Input)
                    return true;
                else return false;
            }
        }

        public bool HasSolids { get => hasSolids; set => hasSolids = value; }

        public int GetHeaviestCompIndex()
        {
            int index = 0;
            double density = 0;
            for (int i = 0; i < List.Count; i++)
            {
                BaseComp item = List[i];
                if (item.Density > density)
                {
                    density = item.Density;
                    index = i;
                }
            }
            return index;
        }

        public int GetLightestCompIndex()
        {
            int index = 0;
            double density = 2000;
            for (int i = 0; i < List.Count; i++)
            {
                BaseComp item = List[i];
                if (item.Density < density)
                {
                    density = item.Density;
                    index = i;
                }
            }
            return index;
        }

        public Components Combine(Components rec, MoleFlow flow1, MoleFlow flow2)
        {
            if ((CompList.Count != rec.Count))
            {
                return null;
            }
            Components combined = new(this);
            for (int i = 0; i < CompList.Count; i++)
            {
                combined[i].TempMolarFlow = CompList[i].MoleFraction * flow1 + rec[i].MoleFraction * flow2;
            }
            combined.NormaliseTempMolFlows();
            combined.UpdateIndex();
            return combined;
        }

        public void Add(Dictionary<string, string> shortNames)
        {
            foreach (string item in shortNames.Values)
            {
                BaseComp bc = Thermodata.GetComponent(item);
                bc.MoleFraction = 0;
                this.Add(bc);
            };

            NormaliseFractions();

            this.SortByBP();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}