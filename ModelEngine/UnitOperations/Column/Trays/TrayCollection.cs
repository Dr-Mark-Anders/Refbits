using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public partial class TraySection : ExpandableObjectConverter, IEnumerable, ISerializable
    {
        private static int count = 0;
        private string name = "TraySection";
        public List<Tray> Trays = new();
        public UOMProperty reboilduty, condenserduty;
        public UOMProperty ovhdDP = new(ePropID.DeltaP, double.NaN);
        public UOMProperty condenserpressure = new(ePropID.P, double.NaN);
        public eSectionType sectionType = eSectionType.Main;
        private Guid guid = Guid.NewGuid();
        private CondType condensertype = CondType.None;
        private ReboilerType reboilertype = ReboilerType.None;
        public IColumn column = null;
        public int SectionIndex = 0;

        protected UnitOperation RequestParentObject()
        {
            if (OnRequestParentObject == null)
                throw new Exception("OnRequestParentObject handler is not assigned");

            return OnRequestParentObject();
        }

        public Func<UnitOperation> OnRequestParentObject;

        public double RefluxRatio
        {
            get
            {
                return Trays[0].L / (Trays[0].lss_estimate + Trays[0].V);
            }
        }

        public List<double> Errors
        {
            get
            {
                List<double> err = new();

                for (int i = 0; i < Trays.Count; i++)
                    err.Add(Trays[i].trayError);

                return err;
            }
        }

        public int CountUnHeatbalancedTrays
        {
            get
            {
                int count = 0;
                if (condensertype != CondType.None)
                    count++;
                if (reboilertype != ReboilerType.None)
                    count++;
                return count;
            }
        }

        public Guid Guid { get => guid; set => guid = value; }

        public bool HasReboiler
        {
            get
            {
                return reboilertype != ReboilerType.None;
            }
        }

        public bool HasCondenser
        {
            get
            {
                return condensertype != CondType.None;
            }
        }

        private bool isspecified = false;
        internal double RefluxRatioSpec, TotalFeeds, TotalTopAndSideProducts, VapSpec = double.NaN;
        internal bool HasReflux = false;

        public void MakeTemporaryFactors()
        {
            foreach (Tray tray in Trays)
                tray.BackupFactors();
        }

        public TraySection()
        {
            guid = Guid.NewGuid();
            count++;
            name = "TraySection" + count;
        }

        public TraySection(int No)
        {
            guid = Guid.NewGuid();
            Trays = new(No);
            for (int n = 0; n < No; n++)
            {
                Tray t = new(this.column);
                Trays.Add(t);
            }
            count++;
            name = "Tray" + count;
        }

        public PortList GetVapourStreams()
        {
            PortList sum = new();
            foreach (Tray item in Trays)
                if (item.vapourDraw != null && item.vapourDraw.IsConnected)
                    sum.Add(item.vapourDraw);

            return sum;
        }

        public List<double> Compositions()
        {
            List<double> res = new();
            for (int i = 0; i < Trays.Count; i++)
                res.Add(Trays[i].feed.MoleFractions[0]);

            return res;
        }

        private PortList GetLiquidDraws()
        {
            PortList sum = new();
            foreach (Tray item in Trays)
            {
                if (item.liquidDrawRight != null && item.liquidDrawRight.IsConnected)
                    sum.Add(item.liquidDrawRight);
            }
            return sum;
        }

        private PortList GetFeeds()
        {
            PortList sum = new();
            foreach (Tray item in Trays)
            {
                if (item.feed != null && item.feed.IsConnected)
                    sum.Add(item.feed);
            }
            return sum;
        }

        public TraySection(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                Trays = (List<Tray>)si.GetValue("TrayList", typeof(List<Tray>));
                Guid = (Guid)si.GetValue("guid", typeof(Guid));
                condensertype = (CondType)si.GetValue("CondenserType", typeof(CondType));
                ovhdDP = (UOMProperty)si.GetValue("OVHD_DP", typeof(UOMProperty));
                ReboilerType = (ReboilerType)si.GetValue("ReboilerType", typeof(ReboilerType));
            }
            catch
            {
            }
        }

        public void GetObjectData(SerializationInfo si, StreamingContext ctx)
        {
            si.AddValue("TrayList", Trays);
            si.AddValue("guid", guid);
            si.AddValue("CondenserType", condensertype);
            si.AddValue("OVHD_DP", ovhdDP, typeof(UOMProperty));
            si.AddValue("ReboilerType", ReboilerType);
        }

        public PortList FeedArray
        {
            get
            {
                PortList list = new();
                for (int i = 0; i < Trays.Count; i++)
                    list.Add(Trays[i].feed);

                return list;
            }
        }

        public double[] lss_spec
        {
            get
            {
                List<double> res = new();
                for (int i = 0; i < Trays.Count; i++)
                    res.Add(Trays[i].lss_spec);

                return res.ToArray();
            }
        }

        public double[] vss_spec
        {
            get
            {
                List<double> res = new();
                for (int i = 0; i < Trays.Count; i++)
                    res.Add(Trays[i].vss_spec);

                return res.ToArray();
            }
        }

        public double[] lss_estimate
        {
            get
            {
                List<double> res = new();
                for (int i = 0; i < Trays.Count; i++)
                    res.Add(Trays[i].lss_estimate);

                return res.ToArray();
            }
        }

        public double[] LiqDrawFactor
        {
            get
            {
                List<double> res = new();
                for (int i = 0; i < Trays.Count; i++)
                    res.Add(Trays[i].LiqDrawFactor);

                return res.ToArray();
            }
        }

        public double[] VapDrawFactor
        {
            get
            {
                List<double> res = new();
                for (int i = 0; i < Trays.Count; i++)
                    res.Add(Trays[i].VapDrawFactor);

                return res.ToArray();
            }
        }

        public double[] TPredC
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].TPredicted - 273.15;

                return res;
            }
        }

        public double[] P
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].P;

                return res;
            }
        }

        /*    public  double [] LnkComp
            {
                get
                {
                    double [] res = new  double [Trays.Count];
                    for (int  i = 0; i < Trays.Count; i++)
                        if (Trays[i].LnKComp != null)
                            res[i] = Trays[i].LnKComp[0];
                        else
                            res[i] = double.NaN;

                    return   res;
                }
            }

            public  double [] LnkCompGrad
            {
                get
                {
                    double [] res = new  double [Trays.Count];
                    for (int  i = 0; i < Trays.Count; i++)
                        if (Trays[i].lnKCompGrad != null)
                            res[i] = Trays[i].lnKCompGrad[0];
                        else
                            res[i] = double.NaN;

                    return   res;
                }
            }*/

        public double[] C
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].T - 273.15;

                return res;
            }
        }

        public double[] MoleFlow
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].feed.MolarFlow_;

                return res;
            }
        }

        public double[] T
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].T;
                return res;
            }
        }

        public double[] TPred
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].TPredicted;
                return res;
            }
        }

        public double[][] lnKcomp
        {
            get
            {
                double[][] res = new double[Trays.Count][];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].LLinear.LnKComp;
                return res;
            }
        }

        public double[,] Kcomp
        {
            get
            {
                double[,] res = new double[Trays.Count,column.NoComps];
                for (int i = 0; i < Trays.Count; i++)
                {
                    //res[i] = new double[column.NoComps];
                    Tray tray = Trays[i];
                    for (int j = 0; j < column.NoComps; j++)
                    {
                        res[i,j] = tray.K_TestFast(j, tray.T, column.SolverOptions.KMethod);
                    }
                }

                return res;
            }
        }

        public double[] LiqEnthalpy
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].liqenth;
                return res;
            }
        }

        public double[] VapEnthalpy
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].vapenth;

                return res;
            }
        }

        public double[] lnKBaseT
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].LLinear.BaseT;

                return res;
            }
        }

        public double[] EnthalpyBalances
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].enthalpyBalance;

                return res;
            }
        }

        public double[] TotalLiquid
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].L;

                return res;
            }
        }

        public double[] TotalVapour
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].V;

                return res;
            }
        }

        public double[] stripFact
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].StripFact;

                return res;
            }
        }

        public double[] lnKAvgT
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                    res[i] = Trays[i].LLinear.BaseT;

                return res;
            }
        }

        public double[][] KComp
        {
            get
            {
                double[][] res = null;
                if (Trays[0].LiqComposition != null)
                {
                    int NoComps = Trays[0].LiqComposition.Length;
                    res = new double[Trays.Count][];
                    for (int tray = 0; tray < Trays.Count; tray++)
                    {
                        res[tray] = new double[NoComps];
                        for (int CompNo = 0; CompNo < NoComps; CompNo++)
                            if (Trays[tray].LLinear.LnKComp != null)
                            {
                                res[tray][CompNo] = Math.Exp(Trays[tray].LLinear.LnKComp[CompNo]);
                            }
                    }
                }
                return res;
            }
        }

        public double[][] LiqCompositions
        {
            get
            {
                if (Trays[0].LiqComposition != null)
                {
                    int NoComps = Trays[0].LiqComposition.Length;
                    double[][] res = new double[Trays.Count][];

                    for (int tray = 0; tray < Trays.Count; tray++)
                    {
                        res[tray] = new double[NoComps];
                        for (int x = 0; x < NoComps; x++)
                            res[tray][x] = Trays[tray].LiqComposition[x];
                    }
                    return res;
                }
                return null;
            }
        }

        public double[,] LiqCompositions2
        {
            get
            {
                if (Trays[0].LiqComposition != null)
                {
                    int NoComps = Trays[0].LiqComposition.Length;
                    double[,] res = new double[Trays.Count,NoComps];

                    for (int tray = 0; tray < Trays.Count; tray++)
                    {
                        for (int x = 0; x < NoComps; x++)
                            res[tray,x] = Trays[tray].LiqComposition[x];
                    }
                    return res;
                }
                return null;
            }
        }

        public double[][] LiqCompositionsPredicted
        {
            get
            {
                if (Trays[0].LiqComposition != null)
                {
                    int NoComps = Trays[0].LiqCompositionPred.Length;
                    double[][] res = new double[Trays.Count][];

                    for (int tray = 0; tray < Trays.Count; tray++)
                    {
                        res[tray] = new double[NoComps];
                        for (int x = 0; x < NoComps; x++)
                            res[tray][x] = Trays[tray].LiqCompositionPred[x];
                    }
                    return res;
                }
                return null;
            }
        }

        public double[][] VapCompositionsPredicted
        {
            get
            {
                if (Trays[0].LiqComposition != null)
                {
                    int NoComps = Trays[0].LiqCompositionPred.Length;
                    double[][] res = new double[Trays.Count][];

                    for (int tray = 0; tray < Trays.Count; tray++)
                    {
                        res[tray] = new double[NoComps];
                        for (int x = 0; x < NoComps; x++)
                            res[tray][x] = Trays[tray].VapCompositionPred[x];
                    }
                    return res;
                }
                return null;
            }
        }

        public double[][] InitialLiqCompositions
        {
            get
            {
                if (Trays[0].LiqComposition != null)
                {
                    int NoComps = Trays[0].LiqComposition.Length;
                    double[][] res = new double[NoTrays][];

                    for (int tray = 0; tray < NoTrays; tray++)
                    {
                        res[tray] = new double[NoComps];
                        for (int x = 0; x < NoComps; x++)
                            res[tray][x] = Trays[tray].LiqCompositionInitial[x];
                    }
                    return res;
                }
                return null;
            }
        }

        public double[][] VapCompositions
        {
            get
            {
                if (Trays[0].LiqComposition != null)
                {
                    int NoComps = Trays[0].LiqComposition.Length;
                    double[][] res = new double[NoTrays][];

                    for (int tray = 0; tray < NoTrays; tray++)
                    {
                        res[tray] = new double[NoComps];
                        for (int x = 0; x < NoComps; x++)
                            res[tray][x] = Trays[tray].VapComposition[x];
                    }
                    return res;
                }
                return null;
            }
        }

        public double[][] InitialVapCompositions
        {
            get
            {
                if (Trays[0].LiqComposition != null)
                {
                    int NoComps = Trays[0].LiqComposition.Length;
                    double[][] res = new double[NoTrays][];

                    for (int tray = 0; tray < NoTrays; tray++)
                    {
                        res[tray] = new double[NoComps];
                        for (int x = 0; x < NoComps; x++)
                            res[tray][x] = Trays[tray].VapCompositionInitial[x];
                    }
                    return res;
                }
                return null;
            }
        }

        public double[] L
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                {
                    res[i] = Trays[i].L;
                }
                return res;
            }
        }

        public double[] V
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                {
                    res[i] = Trays[i].V;
                }
                return res;
            }
        }

        public double[] LTemp
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                {
                    res[i] = Trays[i].Ltemp;
                }
                return res;
            }
        }

        public double[] VTemp
        {
            get
            {
                double[] res = new double[Trays.Count];
                for (int i = 0; i < Trays.Count; i++)
                {
                    res[i] = Trays[i].Vtemp;
                }
                return res;
            }
        }



        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            for (int i = 0; i < Trays.Count; i++)
            {
                Trays[i].TrayChanged += TraySection_TrayChanged;
                Trays[i].Owner = this;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return Trays.GetEnumerator();
        }

        public void RepositionTrays(Rectangle columnrectangle)
        {
            float TrayHeight;

            if (Trays.Count == 0)
                return;

            if (Trays.Count == 1)
                TrayHeight = columnrectangle.Height;
            else
                TrayHeight = columnrectangle.Height / (Trays.Count);

            int width = columnrectangle.Width;

            for (int n = 0; n < Trays.Count; n++)
            {
                Rectangle r = Trays[n].Location;
                r.Y = Convert.ToInt32(columnrectangle.Y + n * TrayHeight);
                r.X = columnrectangle.X;
                r.Height = Convert.ToInt32(TrayHeight);
                r.Width = width;
                Trays[n].Location = r;
            }
        }

        public bool Contains(Point p)
        {
            foreach (Tray t in Trays)
            {
                if (t.Contains(p))
                    return true;
            }
            return false;
        }

        public void AddTrays(int v)
        {
            Tray t;
            for (int i = 0; i < v; i++)
            {
                t = new Tray(column);
                Trays.Add(t);
            }
        }

        public UnitOperation GetParent()
        {
            UnitOperation parent = RequestParentObject();
            return parent;
        }

        public Tray GetTray(Point p)
        {
            foreach (Tray t in Trays)
            {
                if (t.Contains(p))
                    return t;
            }
            return null;
        }

        public Tray GetTray(Guid p)
        {
            foreach (Tray t in Trays)
            {
                if (t.Guid == p)
                    return t;
            }
            return null;
        }

        public Tray this[Guid index]
        {
            get
            {
                foreach (var item in Trays)
                {
                    if (item.Guid == index)
                        return item;
                }
                return null;
            }
        }

        public void Clear()
        {
            Trays.Clear();
        }

        public object GetList()
        {
            return Trays;
        }

        public void Add(Tray t)
        {
            Trays.Add(t);
            t.Owner= this;
        }

        public void AddRange(List<Tray> t)
        {
            for (int i = 0; i < t.Count; i++)
            {
                this.Add(t[i]);
            }
        }

        public void AddRange(TraySection t)
        {
            if (t != null)
                for (int i = 0; i < t.Trays.Count; i++)
                {
                    Trays.Add(t.Trays[i]);
                }
        }

        public void Insert(int index, Tray t)
        {
            if (index >= 0)
            {
                t.Owner = this;
                Trays.Insert(index, t);
            }
            t.Owner = this;
        }

        public void Insert(int index, int t)
        {
            if (index >= 0)
            {
                for (int n = 0; n < t; n++)
                {
                    Tray T = new Tray(column);
                    T.Owner = this;
                    Trays.Insert(index, T);
                }
            }
        }

        public void Sort()
        {
            Trays.Sort();
        }

        public int IndexOf(Guid t)
        {
            for (int i = 0; i < Trays.Count; i++)
            {
                if (Trays[i].Guid == t)
                    return i;
            }
            return -1;
        }

        public void Remove(Tray t)
        {
            if (Trays.Count > 1)
            {
                Trays.Remove(t);
            }
        }

        public void RemoveAt(int index)
        {
            if (Trays.Count > 1)
            {
                Trays.RemoveAt(index);
            }
        }

        public void RemoveAt(int index, int NoTrays)
        {
            if (Trays.Count > 1 && Trays.Count > NoTrays)
            {
                for (int n = 0; n < NoTrays; n++)
                    Trays.RemoveAt(Trays.Count - 1);
            }
        }

        public bool IsSpecified
        {
            get { return isspecified; }
            set { isspecified = value; }
        }

        public Tray TopTray
        {
            get
            {
                if (Trays.Count != 0)
                    return Trays[0];
                return null;
            }
        }

        public Tray BottomTray
        {
            get
            {
                if (Trays.Count != 0)
                    return Trays[^1];
                return null;
            }
        }

        public CondType CondenserType
        {
            get
            {
                return condensertype;
            }
            set
            {
                condensertype = value;
            }
        }

        public ReboilerType ReboilerType
        {
            get
            {
                return reboilertype;
            }
            set
            {
                reboilertype = value;
            }
        }

        public int NoTrays
        {
            get
            {
                return Trays.Count;
            }
        }

        public object CondenserTrays { get; set; }
        public bool IsActive { get; internal set; }
        public bool IsMain { get; internal set; }
        public string Name { get => name; set => name = value; }

        public bool ValidateStripperConnections(List<object> colGraphicsList)
        {
            throw new NotImplementedException();
        }

        public void Insert(int v, object p)
        {
            throw new NotImplementedException();
        }

        internal TraySection CloneDeep(Column col)
        {
            TraySection ts = new();

            for (int i = 0; i < Trays.Count; i++)
            {
                ts.Trays.Add(Trays[i].CloneDeep(col));
            }
            return ts;
        }

        internal TraySection Clone()
        {
            TraySection ts = new();

            for (int i = 0; i < Trays.Count; i++)
            {
                ts.Trays.Add(Trays[i].Clone());
            }
            return ts;
        }
    }

    [Serializable]
    public class TraySectionCollection : IEnumerable, ISerializable
    {
        public readonly List<TraySection> traysections = new();
        private readonly IColumn column;

        public TraySectionCollection(SerializationInfo info, StreamingContext context)
        {
            traysections = (List<TraySection>)info.GetValue("trays", typeof(List<TraySection>));
        }

        public TraySectionCollection(IColumn column, bool IncludeMain = true)
        {
            this.column = column;
            if (IncludeMain)
            {
                TraySection tc = new();
                tc.IsMain = true;
                traysections.Add(tc);
            }
        }

        public List<TraySection> strippers()
        {
            List<TraySection> list = new();
            foreach (var item in traysections)
            {
                if (item.sectionType == eSectionType.stripper)
                    list.Add(item);
            }
            return list;
        }

        public TraySection MainTraySection
        {
            get
            {
                return traysections[0];
            }
            set
            {
                traysections[0] = value;
            }
        }

        public PortList InterSectionConnections()
        {
            PortList ports = new();

            foreach (TraySection traysection in traysections)
            {
                foreach (Tray tray in traysection) // only consider product draws that are connected to other section ports.
                {
                    if (tray.liquidDrawRight.ConnectedPortNext != null && tray.liquidDrawRight.SectionGuid != ((Port_Material)tray.liquidDrawRight.ConnectedPortNext).SectionGuid)
                        ports.Add(tray.liquidDrawRight);
                    if (tray.vapourDraw.ConnectedPortNext != null && tray.vapourDraw.SectionGuid != ((Port_Material)tray.vapourDraw.ConnectedPortNext).SectionGuid)
                        ports.Add(tray.vapourDraw);
                }
            }
            return ports;
        }

        public int TotNoTrays()
        {
            int no = 0;
            foreach (TraySection ds in traysections)
            {
                no += ds.Trays.Count;
            }
            return no;
        }

        public TraySection this[int index]
        {
            get
            {
                if (traysections != null && traysections.Count != 0 && traysections.Count > index)
                    return traysections[index];
                else
                    return null;
            }
            set
            {
                traysections[index] = value;
            }
        }

        public TraySection this[Guid guid]
        {
            get
            {
                foreach (TraySection item in traysections)
                {
                    if (item.Guid == guid)
                        return item;
                }
                return null;
            }
        }

        public void Add(TraySection tc)
        {
            tc.column = this.column;
            traysections.Add(tc);
        }

        public IEnumerator GetEnumerator()
        {
            return traysections.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return traysections.Count;
            }
        }

        internal Port_Material GetLargestFeedStream()
        {
            Port_Material p = new();
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0);

            foreach (var traysection in traysections)
            {
                foreach (Tray tray in traysection)
                {
                    if (tray.feed != null)
                    {
                        if (tray.feed.MolarFlow_ > p.MolarFlow_)
                            p = tray.feed;
                    }
                }
            }
            return p;
        }

        internal int UnheatBalancedTraysCount
        {
            get
            {
                int count = 0;
                foreach (var traysection in traysections)
                    count += traysection.CountUnHeatbalancedTrays;

                return count;
            }
        }

        internal void UnBackupFactor(IColumn column)
        {
            foreach (TraySection section in traysections)
                foreach (Tray tray in section)
                    tray.UnBackupFactors();

            foreach (PumpAround pa in column.PumpArounds)
                pa.DrawFactor = pa.tempDrawFactor;

            foreach (ConnectingStream cs in column.ConnectingDraws)
                cs.DrawFactor = cs.TempDrawFactor;
        }

        public void BackupFactors(IColumn column)
        {
            foreach (TraySection section in traysections)
                foreach (Tray tray in section)
                    tray.BackupFactors();

            foreach (PumpAround pa in column.PumpArounds)
                pa.tempDrawFactor = pa.DrawFactor;

            foreach (ConnectingStream cs in column.ConnectingDraws)
                cs.TempDrawFactor = cs.DrawFactor;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("trays", traysections);
        }

        public string[] Names()
        {
            string[] names = new string[Count];

            for (int i = 0; i < Count; i++)
                names[i] = traysections[i].Name;

            return names;
        }

        internal int IndexOf(TraySection drawsection)
        {
            return traysections.IndexOf(drawsection);
        }
    }
}