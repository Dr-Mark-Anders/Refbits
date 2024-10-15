using ModelEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace ModelEngine
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public partial class FlowSheet : UnitOperation, ISerializable
    {
        private Components Components = new();
        public ModelStack ModelStack = new();
        public List<Recycle> recycleList = new();
        public List<AdjustObject> adjustList = new();
        public Stack<StreamProperty> iterationStack = new();
        public StackIUnitOP solveStack = new();
        private static InconsistencyStack inconsistencyStack = new();
        private static string dataFile; // comp data
        public RichTextBox messages = new RichTextBox();

        private static SourceEnum calcType = SourceEnum.UnitOpCalcResult;
        private static SourceEnum transferType = SourceEnum.Transferred;
        private static eValueMode valuemode = eValueMode.Normal;

        private string name = "";

        public override string Name { get => name; set => name = value; }

        internal Port GetObjectData(Guid guid)
        {
            foreach (var uo in ModelStack) // is it a UO or stream?
            {
                foreach (Port port in uo.Ports) // is ita port
                {
                    if (port.Guid == guid)
                        return port;

                    switch (port)
                    {
                        case Port_Signal ps:
                            if (guid == ps.Value.guid)
                                return ps;
                            break;

                        case Port_Energy pe:
                            if (guid == pe.Value.guid)
                                return pe;
                            break;

                        case Port_Material pm:
                            foreach (var property in pm.Properties.Props.Values) // is it a property
                            {
                                if (guid == property.guid)
                                    return port;
                            }
                            break;
                    }
                }
            }
            return null;
        }

        public FlowSheet() : base()
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ComponentList", FlowsheetComponentList, typeof(Components));
            info.AddValue("UnitList", this.ModelStack, typeof(Dictionary<Guid, UnitOperation>));
        }

        protected FlowSheet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            FlowsheetComponentList = (Components)info.GetValue("ComponentList", typeof(Components));
            this.ModelStack = (ModelStack)info.GetValue("UnitList", typeof(ModelStack));
        }

        public List<string> GetCompoundNames()
        {
            List<string> names = new();
            foreach (BaseComp item in FlowsheetComponentList)
                names.Add(item.Name);

            return names;
        }

       

        public override void EraseNonFixedValues()
        {
            foreach (var UO in ModelStack)
            {
                switch (UO)
                {
                    case FlowSheet fs:
                        foreach (Port item in fs.Ports)
                        {
                            item.ClearNonInputs();
                            item.ClearEstimates();
                        }
                        fs.EraseNonFixedValues();
                        fs.IsDirty = true;
                        //fs.IsSolved = false;
                        break;

                    case Assay _:
                        break;

                    case Recycle r:
                        break;

                    case StreamMaterial sm:
                       /* if (sm.Port.ConnectedPortFinal is not null && !(sm.Port.ConnectedPortFinal.Owner is Recycle))
                        {
                            UO.EraseNonFixedValues();
                            UO.EraseNonFixedComponents();
                            UO.EraseEstimates();
                            UO.IsDirty = true;
                            UO.IsSolved = false;
                        }*/
                        break;
                    default:
                        UO.EraseNonFixedValues();
                        UO.EraseNonFixedComponents();
                        UO.EraseEstimates();
                        UO.IsDirty = true;
                        //UO.IsSolved = false;
                        break;
                }
            }
        }

        public override void EraseEstimates()
        {
            foreach (var UO in ModelStack)
                UO.EraseEstimates();
        }

        public override void EraseCalcEstimates()
        {
            foreach (var UO in ModelStack)
                UO.EraseCalcEstimates();
        }

        public void EraseMoleFractions()
        {
            foreach (IUnitOperation item in this.ModelStack)
                foreach (Port_Material port in item.Ports)
                    foreach (BaseComp basec in port.cc)
                        basec.MoleFraction = double.NaN;

            EraseNonFixedValues();
        }

        public new void Add(BaseComp bc)
        {
            FlowsheetComponentList.Add(bc);
            foreach (var item in ModelStack)
                item.Add(bc);
        }

        public void Add(Components cc)
        {
            foreach (BaseComp bc in cc)
            {
                FlowsheetComponentList.Add(bc);
                foreach (var item in ModelStack)
                    item.Add(bc);
            }

            FlowsheetComponentList.Thermo = cc.Thermo;

            foreach (var m in ModelStack)
                foreach (var p in m.Ports)
                    p.cc.Thermo = cc.Thermo;
        }

        public void Add(UnitOperation uo)
        {
            if (uo != null)
            {
                foreach (BaseComp comp in this.Components)
                    foreach (Port port in uo.Ports)
                        switch (port)
                        {
                            case Port_Signal _:
                            case Port_Energy _:
                                break;

                            case Port_Material pm:
                                pm.cc.Add(comp.Clone());
                                break;
                        }

                if (uo is HXSubFlowSheet && uo.Flowsheet != null)
                    uo.Flowsheet.ComponentList = this.ComponentList.Clone();

                ModelStack.Add(uo);
                uo.OnRequestParent += new Func<UnitOperation>(delegate { return this; });
            }
        }

        public void Add(UnitOperation uo, string name)
        {
            uo.Name = name;
            ModelStack.Add(uo);
            solveStack.Push(uo);
            uo.AddComponents(FlowsheetComponentList);

            uo.OnRequestParent += new Func<UnitOperation>(delegate { return this; });
        }

        public void AddComponent(string name)
        {
            BaseComp bc = Thermodata.GetComponent(name);

            if (bc != null)
            {
                foreach (UnitOperation uo in ModelStack)
                    foreach (var item in uo.Ports)
                        item.cc.Add(bc);

                this.Add(bc);
            }
        }

        public Components ComponentList
        {
            get
            {
                return Components;
            }
            set
            {
                Components = value;
            }
        }

        public Components FlowsheetComponentList { get => Components; set => Components = value; }
        public static BackgroundWorker BackGroundWorker { get => backgroundworker; set => backgroundworker = value; }
        public CaseStudies CaseStudies { get; set; }
        public static InconsistencyStack InconsistencyStack { get => inconsistencyStack; set => inconsistencyStack = value; }
        public static string DataFile { get => dataFile; set => dataFile = value; }
        public static SourceEnum CalcType { get => calcType; set => calcType = value; }
        public static SourceEnum TransferType { get => transferType; set => transferType = value; }
        public static eValueMode Valuemode { get => valuemode; set => valuemode = value; }
        public static StreamWriter OutputFile { get => outputFile; set => outputFile = value; }

        public Port_Material CreateMaterialPort(FlowDirection dir, string name)
        {
            Port_Material p = new(name, dir);

            foreach (BaseComp item in FlowsheetComponentList)
            {
                p.cc.Add(item.Clone());
            }
            return p;
        }

        public void DeleteCompound(string v)
        {
            FlowsheetComponentList.Remove(v);

            foreach (UnitOperation uo in ModelStack)
                foreach (var item in uo.Ports)
                    item.cc.Remove(v);
        }

        public void UpdateAllPortComponents(Components comps)
        {
            foreach (UnitOperation uo in ModelStack)
            {
                switch (uo)
                {
                    case HXSubFlowSheet sfs:
                        sfs.UpdateAllPortComponents(comps);
                        break;

                    case Column col:
                        foreach (TraySection ts in col.TraySections)
                            foreach (Tray t in ts.Trays)
                            {
                                t.feed.cc.Clear();
                                t.vapourDraw.cc.Clear();
                                t.liquidDrawRight.cc.Clear();
                                t.TrayLiquid.cc.Clear();
                                t.TrayVapour.cc.Clear();
                                t.WaterDraw.cc.Clear();
                            }
                        foreach (TraySection ts in col.TraySections)
                            foreach (Tray t in ts.Trays)
                            {
                                t.feed.cc.Add(comps.Clone());
                                t.vapourDraw.cc.Add(comps.Clone());
                                t.liquidDrawRight.cc.Add(comps.Clone());
                                t.TrayLiquid.cc.Add(comps.Clone());
                                t.TrayVapour.cc.Add(comps.Clone());
                                t.WaterDraw.cc.Add(comps.Clone());
                            }
                        break;

                    default:
                        foreach (Port_Material p in uo.Ports)
                        {
                            p.cc.Clear();
                            p.cc.Add(comps.Clone());
                        }
                        break;
                }
            }

            FlowsheetComponentList.Clear();
            FlowsheetComponentList.Add(comps);
        }

        public void ShowConsistency()
        {
            InconsistencyForm form = new(this);
            form.ShowDialog();
        }

        public void Clear()
        {
            ModelStack.Clear();
            solveStack.Clear();
        }
    }
}