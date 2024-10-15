using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Units;
using Units.UOM;
using Extensions;

namespace ModelEngine
{
    [Serializable]
    public partial class Port_Material : Port, ISerializable
    {
        public double RatioedLiquidFeed;
        public double RatioedVapourFeed;
        public bool SplitFeed;
        private Components comps = new();
        public bool AllowBackTransfer = true;

        internal StreamPropList _properties = new();

        private StreamProperty propIn = new StreamProperty(ePropID.Undefined);
        private StreamProperty propOut = new StreamProperty(ePropID.Undefined);

        public Dictionary<enumAssayPCProperty, IUOM> BulkAssayProperty = new();

        [Browsable(false)]
        public StreamPropList Properties
        {
            get
            {
                if (base.StreamPort is Port_Material pm)
                    return pm._properties; // should come from original source by recursion
                return _properties;
            }
            set
            {
                if (base.StreamPort is Port_Material pm)
                    pm.Properties = value;
                _properties = value;
            }
        }

        private void Clear(ePropID id)
        {
            StreamPropList props = Properties;

            props[id].Clear();
        }

        public Components cc
        {
            get
            {
                if (base.StreamPort is Port_Material pm)
                    return pm.cc; // recursive
                return comps;
            }
            set
            {
                if (base.StreamPort is Port_Material pm)
                    pm.cc = value;// recursive
                comps = value;
            }
        }

        public override Port_Material ConnectedPortNext
        {
            get
            {
                if (base.StreamPort is Port_Material pm)
                    return pm;
                else return null;
            }
        }

        public Dictionary<ePropID, StreamProperty> Props
        {
            get
            {
                return Properties.Props;
            }
        }

        public ThermoDynamicOptions Thermo
        {
            get
            {
                return cc.Thermo;
            }
            set
            {
                cc.Thermo = value;
            }
        }

        public BaseComp this[string name]
        {
            get
            {
                if (cc.Contains(name))
                {
                    return cc[name];
                }
                return null;
            }
        }

        public override string ToString()
        {
            if (Name is not null)
                return Name.ToString();

            return "NA";
        }

        private bool isflashed = false;

        public Port_Material(Components cc)
        {
            this.cc = cc;
            this.cc.Origin = SourceEnum.Input;
        }

        [Category("Flags")]
        public virtual bool IsSolved
        {
            get
            {
                if (MF_.IsKnown && VF_.IsKnown && MolarFlow_.IsKnown && H_.IsKnown &&
                    S_.IsKnown && T_.IsKnown && P_.IsKnown && Q_.IsKnown
                    && cc.Count > 0 && cc.MolFracSum().AlmostEquals(1))
                    // && isflashed)
                    return true;
                return false;
            }
        }

        [Browsable(true)]
        public double[] MoleFractions
        {
            get
            {
                return cc.MoleFractions;
            }
        }

        public ThermoPropsMass MassProps(Pressure P, Temperature T)
        {
            Port_Material pm = this.Clone();
            pm.P_.BaseValue = P.BaseValue;
            pm.T_.BaseValue = T.BaseValue;
            pm.Flash(true, enumFlashType.PT, true, this.cc.Thermo);
            return new ThermoPropsMass(pm.cc.ThermoVap, pm.MW);
        }

        public void RemoveNanComponents()
        {
            foreach (BaseComp bc in cc)
            {
                if (double.IsNaN(bc.MoleFraction))
                    bc.MoleFraction = 0;
            }
        }

        public void UpdateCalcProperties()
        {
            this.cc.NormaliseFractions(FlowFlag.Molar);
            this.PropsCalculated.Add(new CalcProperty("Std. SG", ePropID.SG, this.cc.SG()));
            this.PropsCalculated.Add(new CalcProperty("Std. Density", ePropID.Density, this.cc.Density));
            this.PropsCalculated.Add(new CalcProperty("Act. SG", ePropID.SG_ACT, this.cc.ActLiqSG(P, T)));
            this.PropsCalculated.Add(new CalcProperty("Act. Density", ePropID.Density_ACT, this.ActLiqDensity()));
            this.PropsCalculated.Add(new CalcProperty("Act. Vol Flow", ePropID.VolFlow_ACT, MF_ / this.ActLiqDensity()));
            this.PropsCalculated.Add(new CalcProperty("Hform25C", ePropID.HForm25, this.cc.Hform25()));
            this.PropsCalculated.Add(new CalcProperty("Gform25C", ePropID.Gibbsf25, this.cc.GibbsFormation()));
            this.PropsCalculated.Add(new CalcProperty("Sform25C", ePropID.Entropyf25, this.cc.SForm25()));
            this.PropsCalculated.Add(new CalcProperty("HformT", ePropID.HForm25, this.cc.Hform25()));
            this.PropsCalculated.Add(new CalcProperty("GformT", ePropID.Gibbs, this.cc.GFormT(P, T, Q)));
            this.PropsCalculated.Add(new CalcProperty("SformT", ePropID.Entropyf25, this.cc.StreamEntropy(Q)));
            this.PropsCalculated.Add(new CalcProperty("H", ePropID.HForm25, this.cc.H(P, T, Q)));
            this.PropsCalculated.Add(new CalcProperty("S", ePropID.HForm25, this.cc.StreamEntropy(Q)));
            this.PropsCalculated.Add(new CalcProperty("U", ePropID.U, this.cc.InternalEnergy(P, T, Q)));
            this.PropsCalculated.Add(new CalcProperty("Cp Mass Ideal", ePropID.MassCp, this.cc.CP_MASS_Ideal(P, T)));
            this.PropsCalculated.Add(new CalcProperty("Cp Ideal", ePropID.MolarHeatCapacity, this.cc.CP_Ideal(T)));
            this.PropsCalculated.Add(new CalcProperty("Cp Mass", ePropID.MassCp, this.cc.CP_MASS(P, T, Q)));
            this.PropsCalculated.Add(new CalcProperty("Cp Molar", ePropID.MolarHeatCapacity, this.cc.CP(T, Q)));
            this.PropsCalculated.Add(new CalcProperty("Cv Mass", ePropID.MassCp, this.cc.CV_MASS(P, T, Q)));
            this.PropsCalculated.Add(new CalcProperty("Cv Molar", ePropID.MolarHeatCapacity, this.cc.CV(T, Q)));
            this.PropsCalculated.Add(new CalcProperty("CP/CV", ePropID.MolarHeatCapacity, this.cc.CP(T, Q) / this.cc.CV(T, Q)));
            this.PropsCalculated.Add(new CalcProperty("CP/(Cp-R)", ePropID.MolarHeatCapacity, this.cc.CP(T, Q) / (this.cc.CP(T, Q) - 8.314)));
            //this.PropsCalculated.Add(new CalcProperty("H", ePropID.H, this.Components.H()));
            //this.PropsCalculated.Add(new CalcProperty("G", ePropID.Gibbs, this.Components.G()));
            //this.PropsCalculated.Add(new CalcProperty("S", ePropID.S, this.Components.Entropy()));
            //this.PropsCalculated.Add(new CalcProperty("U", ePropID.U, this.Components.InternalEnergy()));
            this.PropsCalculated.Add(new CalcProperty("Helmholtz", ePropID.A, this.HelmholtzEnergy(P, T, Q)));
        }

        private double HelmholtzEnergy(Pressure p, Temperature t, Quality q)
        {
            return this.cc.HelmholtzEnergy(P, T, Q);
        }

        public bool UpdateFlows()
        {
            if (cc.Count > 0)
            {
                switch (CheckForInputFlows())
                {
                    case enumMassMolarOrVol.Mass:
                        SetPortValue(ePropID.MOLEF, MF_ / cc.MW(), SourceEnum.PortCalcResult, false);
                        if (this.MolarFlow_ == 0)
                            SetPortValue(ePropID.VF, 0, SourceEnum.PortCalcResult, false);
                        else
                            SetPortValue(ePropID.VF, MF_ / cc.Density, SourceEnum.PortCalcResult, false);

                        break;

                    case enumMassMolarOrVol.Vol:
                        SetPortValue(ePropID.MF, VF_ * cc.Density, SourceEnum.PortCalcResult, false);
                        SetPortValue(ePropID.MOLEF, MF_ / cc.MW(), SourceEnum.PortCalcResult, false);
                        break;

                    case enumMassMolarOrVol.Molar:
                        SetPortValue(ePropID.MF, MolarFlow_ * cc.MW(), SourceEnum.PortCalcResult, false);

                        if (this.MolarFlow_ == 0)
                            SetPortValue(ePropID.VF, 0, SourceEnum.PortCalcResult, false);
                        else
                            SetPortValue(ePropID.VF, MF_ / cc.Density, SourceEnum.PortCalcResult, false);

                        break;
                }

                switch (CheckModelCalcFlows())
                {
                    case enumMassMolarOrVol.Mass:
                        SetPortValue(ePropID.MOLEF, MF_ / cc.MW(), SourceEnum.PortCalcResult, false);
                        if (this.MolarFlow_ == 0)
                            SetPortValue(ePropID.VF, 0, SourceEnum.PortCalcResult, false);
                        else
                            SetPortValue(ePropID.VF, MF_ / cc.Density, SourceEnum.PortCalcResult, false);
                        break;

                    case enumMassMolarOrVol.Vol:
                        SetPortValue(ePropID.MF, VF_ * cc.Density, SourceEnum.PortCalcResult, false);
                        SetPortValue(ePropID.MOLEF, MF_ / cc.MW(), SourceEnum.PortCalcResult, false);
                        break;

                    case enumMassMolarOrVol.Molar:
                        SetPortValue(ePropID.MF, MolarFlow_ * cc.MW(), SourceEnum.PortCalcResult, false);

                        if (this.MolarFlow_ == 0)
                            SetPortValue(ePropID.VF, 0, SourceEnum.PortCalcResult, false);
                        else
                            SetPortValue(ePropID.VF, MF_ / cc.Density, SourceEnum.PortCalcResult, false);
                        break;
                }
            }
            return true;
        }

        public void IncrementT(Temperature T)
        {
            this.T += T;
        }

        public void DecrementT(Temperature T)
        {
            this.T -= T;
        }

        public enumMassMolarOrVol CheckForInputFlows()
        {
            if (MF_.IsInput)
                return enumMassMolarOrVol.Mass;
            else if (VF_.IsInput)
                return enumMassMolarOrVol.Vol;
            else if (MolarFlow_.IsInput)
                return enumMassMolarOrVol.Molar;
            else return enumMassMolarOrVol.notdefined;
        }

        public enumMassMolarOrVol CheckModelCalcFlows()
        {
            if (MF_.IsFromUnitOP && MF_.IsKnown)
                return enumMassMolarOrVol.Mass;
            else if (VF_.IsFromUnitOP && VF_.IsKnown)
                return enumMassMolarOrVol.Vol;
            else if (MolarFlow_.IsFromUnitOP && MolarFlow_.IsKnown)
                return enumMassMolarOrVol.Molar;
            else return enumMassMolarOrVol.notdefined;
        }

        public List<BaseComp> ComponentList
        {
            get
            {
                return cc.List;
            }
            set
            {
                cc.List = value;
            }
        }

        public void AddComponentsToPort(Components comps)
        {
            if (this.cc is null)
                this.cc = new Components();

            this.cc.Add(comps);
        }

        [Category("Flags")]
        public bool IsFlashed
        {
            get
            {
                if (StreamPort is not null)
                    return ((Port_Material)StreamPort).isflashed;
                return isflashed;
            }
            set
            {
                if (StreamPort is not null)
                    ((Port_Material)StreamPort).isflashed = value;
                isflashed = value;
            }
        }

        public override void ClearIfNotExternallyFlashable()
        {
            if (Guid == FlasheableGuidSource())
                ClearInternalProperties();
        }

        public Guid FlasheableGuidSource()
        {
            List<Guid> guids = new();

            if (P_.OriginPortGuid == H_.OriginPortGuid)
            {
                return P_.OriginPortGuid;
            }

            if (P_.OriginPortGuid == T_.OriginPortGuid)
            {
                return P_.OriginPortGuid;
            }

            if ((P_.OriginPortGuid == H_.OriginPortGuid) && (P_.OriginPortGuid == Q_.OriginPortGuid))
            {
                return P_.OriginPortGuid;
            }

            return this.Guid;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectedport"></param>
        /// <returns></returns>

        public void SetComposition(Guid originGuid, string name, double value, SourceEnum source)
        {
            this.cc[name].MoleFraction = value;
            this.cc[name].OriginPortGuid = originGuid;
            this.cc[name].Source = source;
            this.cc.Origin = source;
        }

        public void SetProperties(StreamPropList newprops, UnitOperation ownerGuid)
        {
            foreach (StreamProperty item in newprops.Props.Values)
                if (item != null)
                    SetPortValue(item.Propid, item.Value, item.origin, ownerGuid);
        }

        public void SetProperties(StreamPropList newprops)
        {
            foreach (StreamProperty item in newprops.Props.Values)
                if (item != null)
                    SetPortValue(item.Propid, item.Value, item.origin, null, true);
        }

        public void SetCompositionValue(string name, double value, SourceEnum source)
        {
            BaseComp bc = this.cc[name];
            if (bc != null)
            {
                this.cc[name].MoleFraction = value;
                this.cc[name].OriginPortGuid = Guid.Empty;
                this.cc[name].Source = source;
                this.cc.Origin = source;
            }
        }

        public Guid SectionGuid { get; set; }

        public void SetMoleFractions(double[] v)
        {
            for (int i = 0; i < ComponentList.Count; i++)
            {
                ComponentList[i].MoleFraction = v[i];
            }
        }

        private bool calcDerivatives;
        public bool CalcDerivatives { get => calcDerivatives; set => calcDerivatives = value; }
        public object CompProps { get; internal set; }

        public ThermoProps ThermoLiq { get => cc.ThermoLiq; set => cc.ThermoLiq = value; }
        public ThermoProps ThermoVap { get => cc.ThermoVap; set => cc.ThermoVap = value; }

        public ThermoDifferentialPropsCollection ThermoLiqDerivatives
        {
            get { return this.cc.ThermoLiqDerivatives; }
            set { this.cc.ThermoLiqDerivatives = value; }
        }

        public ThermoDifferentialPropsCollection ThermoVapDerivatives
        {
            get { return this.cc.ThermoVapDerivatives; }
            set { this.cc.ThermoVapDerivatives = value; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool IsInternal(ePropID prop)
        {
            if (Properties.Contains(prop))
                return Properties[prop].OriginPortGuid == this.Guid;
            return false;
        }

        public void NormaliseFractions(FlowFlag molar)
        {
            cc.NormaliseFractions(molar);
        }

        public bool HasEqualProps(Port_Material portIn)
        {
            if (this.cc.Count != portIn.cc.Count)
                return false;

            for (int i = 0; i < cc.Count; i++)
            {
                if (!cc[i].HasEqualProps(portIn.cc[i]))
                    return false;
            }

            return true;
        }

        public Port_Material Cut(Temperature ibp, Temperature fbp, out double cutfrac)
        {
            Port_Material port = new();
            port.cc = CrudeCutter.Cut(this.cc, ibp, fbp, out cutfrac);
            port.MolarFlow = this.MolarFlow * cutfrac;
            return port;
        }

        public void CutResult(Port_Material port)
        {
            cc = port.cc;
            MolarFlow_ = port.MolarFlow_;
        }

        public double Port(Port_Material port)
        {
            double[] moles = new double[this.cc.Count];

            for (int i = 0; i < cc.Count; i++)
            {
                moles[i] = this.cc[i].MoleFraction * MolarFlow_ + port.cc[i].MoleFraction * port.MolarFlow_;
            }

            moles = moles.Normalise();

            this.MolarFlow += port.MolarFlow;

            this.cc.SetMolFractions(moles);

            return port.MolarFlow_;
        }

        internal void Add(enumAssayPCProperty prop, IUOM value)
        {
            if (BulkAssayProperty.ContainsKey(prop))
                BulkAssayProperty[prop] = value;
            else
                BulkAssayProperty.Add(prop, value);
        }

        public void SetSolidMassRatio(int ratio)
        {
            UpdateFlows();
            Components solids = this.cc.Solids();
            Components NoSolids = this.cc.RemoveSolids(false);

            double[] soldfracs = solids.MoleFractions;

            if (soldfracs.Sum() == 0)
            {
                for (int i = 0; i < soldfracs.Length; i++)
                {
                    soldfracs[i] = 1 / soldfracs.Length;
                }
            }

            MassFlow NoSolidsMassFlow = this.MF * NoSolids.MassFractions.Sum();
            MassFlow SolidMassFlow = new MassFlow(NoSolidsMassFlow * ratio);

            double[] tempmassflow = new double[this.cc.Count];

            for (int i = 0; i < NoSolids.Count; i++)
            {
                tempmassflow[i] = NoSolids[i].MassFraction * this.MF;
            }

            for (int i = NoSolids.Count; i < this.cc.Count; i++)
            {
                tempmassflow[i] = soldfracs[i - NoSolids.Count] * this.MF * ratio;
            }

            tempmassflow = tempmassflow.Normalise();

            this.cc.SetMassFractions(tempmassflow);
        }

        public Port_Material(string Name, FlowDirection fd)
        {
            counter++;
            flowdirection = fd;
            this.Name = Name;
            Guid = Guid.NewGuid();
            Properties.SetPort(this);
        }

        public Dictionary<string, double> Elements()
        {
            Dictionary<string, int> elements;
            Dictionary<string, double> res = new();

            foreach (BaseComp comp in cc)
            {
                elements = comp.GetElements();
                foreach (var e in elements)
                {
                    if (res.ContainsKey(e.Key))
                    {
                        res[e.Key] = res[e.Key] + e.Value * comp.MoleFraction * this.MolarFlow_;
                    }
                    else
                    {
                        res[e.Key] = e.Value * comp.MoleFraction * this.MolarFlow_;
                    }
                }
            }
            return res;
        }

        public Port_Material(string Name, ThermoDynamicOptions thermo)
        {
            this.AllowBackTransfer = false;
            Properties.Props[ePropID.MF] = new StreamProperty(ePropID.MF);
            Properties.Props[ePropID.VF] = new StreamProperty(ePropID.VF);
            Properties.Props[ePropID.MOLEF] = new StreamProperty(ePropID.MOLEF);
            Properties.Props[ePropID.T] = new StreamProperty(ePropID.T);
            Properties.Props[ePropID.P] = new StreamProperty(ePropID.P);
            Properties.Props[ePropID.Q] = new StreamProperty(ePropID.Q);
            Guid = Guid.NewGuid();
            flowdirection = FlowDirection.OUT;
            this.Name = Name;
            cc.Thermo = thermo;

            Properties.SetPort(this);
        }

        public Port_Material(string Name = "Port", FlowDirection fd = FlowDirection.OUT, bool AllowBackTransfer = true)
        {
            this.AllowBackTransfer = AllowBackTransfer;
            Properties.Props[ePropID.MF] = new StreamProperty(ePropID.MF);
            Properties.Props[ePropID.VF] = new StreamProperty(ePropID.VF);
            Properties.Props[ePropID.MOLEF] = new StreamProperty(ePropID.MOLEF);
            Properties.Props[ePropID.T] = new StreamProperty(ePropID.T);
            Properties.Props[ePropID.P] = new StreamProperty(ePropID.P);
            Properties.Props[ePropID.Q] = new StreamProperty(ePropID.Q);
            Guid = Guid.NewGuid();
            flowdirection = fd;
            this.Name = Name;

            Properties.SetPort(this);
        }

        protected Port_Material(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            cc = (Components)info.GetValue("comps", typeof(Components));
            Properties = (StreamPropList)info.GetValue("Props", typeof(StreamPropList));
            try
            {
                isflashed = info.GetBoolean("IsFlashed");
            }
            catch { }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("comps", cc);
            info.AddValue("Props", Properties);
            info.AddValue("IsFlashed", isflashed);
        }

        /* [OnDeserialized]
          public void des()
          {
            if (Properties.Props.Count > 8)
                   ;
          }*/

        [Category("Flags")]
        public bool IsFullyDefined
        {
            get
            {
                if (MF_.IsKnown && VF_.IsKnown && MolarFlow_.IsKnown && H_.IsKnown && S_.IsKnown && Q_.IsKnown && T_.IsKnown && cc.IsDefined)
                    return true;
                else return false;
            }
        }

        [Category("Flags")]
        public bool IsFlashDefined
        {
            get
            {
                if (H_.IsKnown && S_.IsKnown && Q_.IsKnown && T_.IsKnown && cc.IsDefined)
                    return true;
                else return false;
            }
        }

        public double API
        {
            get
            {
                return cc.API;
            }
        }

        public void UpdateDisplysettings(UOMDisplayList uomDisplayList)
        {
            if (uomDisplayList != null)
            {
                foreach (var item in Properties.Props.Values)
                    if (item is not null)
                        item.DisplayUnit = uomDisplayList.DisplayUnit(item.Propid);
            }
        }

        public Port_Material CloneShallow()
        {
            Port_Material p = new();

            foreach (StreamProperty prop in Properties.Props.Values)
            {
                p.Properties.Add((StreamProperty)prop.CloneShallow());
            }

            p.cc = this.cc.Clone();
            return p;
        }

        /// <summary>
        /// Does it has comp list and Mole or vol flow
        /// </summary>

        public Port_Material Clone()
        {
            Port_Material p = new();

            foreach (StreamProperty prop in Properties.Props.Values)
            {
                p.Properties.Add((StreamProperty)prop.Clone());
            }

            p.cc = this.cc.Clone();

            p.ThermoLiq = cc.ThermoLiq.Clone();
            p.ThermoVap = cc.ThermoVap.Clone();
            return p;
        }

        public Port_Material CloneDeep()
        {
            Port_Material p = new();

            foreach (StreamProperty prop in Properties.Props.Values)
            {
                p.Properties.Add((StreamProperty)prop.CloneDeep());
            }

            p.cc = this.cc.Clone();

            p.Guid = this.Guid;

            return p;
        }

        public void Deblend(Port_Material inport, Port_Material outport)
        {
            this.cc.Clear();
            Components comps = new();
            comps.Add(inport.cc);
            comps.Add(outport.cc);// add any missing Components
            double TotIn, TotOut;

            MolarFlow_.BaseValue = inport.MolarFlow_ + outport.MolarFlow_;
            BaseComp bc, bcin, bcout;

            for (int i = 0; i < comps.Count; i++)
            {
                bc = comps[i].Clone();
                bcin = inport.cc[bc.Name];
                bcout = outport.cc[bc.Name];
                this.cc.Add(bc);

                if (bcin.SG_60F.AlmostEquals(bcout.SG_60F)) // nothing to do;
                    break;

                if (bcin != null && bcout != null)
                {
                    TotIn = bcin.STDLiqVolFraction * inport.VF_;
                    TotOut = bcout.STDLiqVolFraction * outport.VF_;
                    bc.SG_60F = (bcin.SG_60F * TotIn - bcout.SG_60F * TotOut) / (TotIn + TotOut);
                }
                else if (bcin != null)
                {
                    bc.SG_60F = bcin.SG_60F;
                }
                else if (bcout != null)
                {
                    bc.SG_60F = bcout.SG_60F;
                }
                bc.ReEstimateCriticalProps();
            }

            this.cc.Origin = SourceEnum.UnitOpCalcResult;
        }

        public void UpdateThermoProperties()
        {
            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, cc.Thermo);
        }

        internal void Update(Port_Material Port)
        {
            foreach (KeyValuePair<ePropID, StreamProperty> prop in Port.Props)
            {
                this.Properties[prop.Key] = prop.Value.Clone();
            }
            this.comps = Port.comps.Clone();
        }

        public static bool operator ==(Port_Material point1, Port_Material point2)
        {
            return ReferenceEquals(point1, point2);
        }

        public static bool operator !=(Port_Material point1, Port_Material point2)
        {
            return !ReferenceEquals(point1, point2);
        }
    }
}