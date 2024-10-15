using ModelEngine;
using NaphthaReformerSI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace NaphthaReformer
{
    [Serializable]
    public class ReformerDataSet : ISerializable
    {
        public ReformerCalibData calibdata = new();
        public ReformerDesignData designdata = new();
        public ReformerSimulationData simulationdata = new();

        public ReformerDataSet()
        {
        }

        public ReformerDataSet(SerializationInfo info, StreamingContext context)
        {
            calibdata = (ReformerCalibData)info.GetValue("calibdata", typeof(ReformerCalibData));
            designdata = (ReformerDesignData)info.GetValue("designdata", typeof(ReformerDesignData));
            simulationdata = (ReformerSimulationData)info.GetValue("simulationdata", typeof(ReformerSimulationData));
            try
            {
            }
            catch
            {
            }
        }

        public string Name { get; internal set; } = "NewData";

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("calibdata", calibdata);
            info.AddValue("designdata", designdata);
            info.AddValue("simulationdata", simulationdata);
        }

        internal ReformerDataSet Clone()
        {
            ReformerDataSet res = new();
            res.calibdata = this.calibdata.Clone();
            return res;
        }
    }

    [Serializable]
    public class ReformerSimulationData : ISerializable
    {
        public bool IsActive = true;
        public int NoOfProducts = 5;
        public AssayBasis assayBasis = AssayBasis.Volume;
        public DistPoints feed = new(new() { 102, 114, 132, 160, 178 },TemperatureUnit.Celsius, new List<int>() { 1, 5, 50, 90, 99 }, new Density(747), enumDistType.D86, SourceEnum.Input);
        public List<RefomerKineticLumpList> Products = new();
        public RefomerKineticLumpList NetH2 = new();
        public RefomerKineticLumpList FG = new();
        public RefomerKineticLumpList LPG = new();
        public RefomerKineticLumpList REFORMATE = new();

        public Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> feedPNAO = Tuple.Create<UOMProperty, UOMProperty, UOMProperty, UOMProperty>(
                new UOMProperty(ePropID.Value, 60.87, SourceEnum.Input),
                new UOMProperty(ePropID.Value, 28.34, SourceEnum.Input),
                new UOMProperty(ePropID.Value, 10.79, SourceEnum.Input),
                new UOMProperty(ePropID.Value, 0.0, SourceEnum.Input));

        public RefomerShortCompList shortgc = new();
        public RefomerKineticLumpList fullgc = new();

        public int NoRx = 3;
        public List<DeltaPressure> RxDp = new() { 0.5, 0.5, 0.5, 0.5 };
        public List<Pressure> RxP = new() { 4.71, 4.21, 3.71, double.NaN };

        public List<UOMProperty> RxT = new()
        {
            new(new Temperature(510, TemperatureUnit.Celsius)),
            new(new Temperature(510, TemperatureUnit.Celsius)),
            new(new Temperature(510, TemperatureUnit.Celsius)),
            new(new Temperature(double.NaN, TemperatureUnit.Celsius))
        };

        public UOMProperty feeddensity = new(new Density(0.747, DensityUnit.SG));
        public UOMProperty feedflow = new(new MassFlow(1675.6242, MassFlowUnit.te_d));

        public List<UOMProperty> CatAmt = new() { new UOMProperty(ePropID.Mass, 20), new UOMProperty(ePropID.Mass, 30), new UOMProperty(ePropID.Mass, 50) };
        public double LHSV = 1.95;
        public InputOption inputoption = InputOption.Short;
        public UOMProperty PSep = new(new Pressure(2.5, PressureUnit.Kg_cm2_g), SourceEnum.Input, "Separator Pressure");
        public UOMProperty TSep = new(new Temperature(38, TemperatureUnit.Celsius), SourceEnum.Input, "Separator Temperature");
        public UOMProperty H2HC = new(ePropID.Mass, SourceEnum.Input, 2.2, "H2:HC (moles H2 to moles hydrocarbon)");

        internal string Name = "Default";
        public enumMassMolarOrVol gcbasis;
        public double RefRON, RefMON;
        public List<string> streamnames = new() { "Net H2", "FG", "LPG", "REFORMATE" };
        public Dictionary<string, UOMProperty> calibfactors = new();

        public UOMProperty R1CatLoad = new(ePropID.Mass, SourceEnum.Input, 20, "Catalyst loading (% of total catalyst)");
        public UOMProperty R1Tin = new(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
        public UOMProperty R1Pin = new(ePropID.P, SourceEnum.Input, 4.71, "Inlet Pressure");
        public UOMProperty R1PDrop = new(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
        public UOMProperty R1MetActivity = new(ePropID.T, SourceEnum.Input, 1, "Metal activity for fresh catalyst (0.1 to 10.0)");
        public UOMProperty R1AcidActivity = new(ePropID.T, SourceEnum.Input, 1, "Acid activity for fresh catalyst (0.1 to 10.0)");

        public UOMProperty R2CatLoad = new(ePropID.Mass, SourceEnum.Input, 30, "Catalyst loading (% of total catalyst)");
        public UOMProperty R2Tin = new(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
        public UOMProperty R2Pin = new(ePropID.P, SourceEnum.Input, 4.21, "Inlet Pressure");
        public UOMProperty R2PDrop = new(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
        public UOMProperty R2MetActivity = new(ePropID.T, SourceEnum.Input, 1, "Metal activity for fresh catalyst (0.1 to 10.0)");
        public UOMProperty R2AcidActivity = new(ePropID.T, SourceEnum.Input, 1, "Acid activity for fresh catalyst (0.1 to 10.0)");

        public UOMProperty R3CatLoad = new(ePropID.Mass, SourceEnum.Input, 50, "Catalyst loading (% of total catalyst)");
        public UOMProperty R3Tin = new(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
        public UOMProperty R3Pin = new(ePropID.P, SourceEnum.Input, 3.71, "Inlet Pressure");
        public UOMProperty R3PDrop = new(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
        public UOMProperty R3MetActivity = new(ePropID.T, SourceEnum.Input, 1, "Metal activity for fresh catalyst (0.1 to 10.0)");
        public UOMProperty R3AcidActivity = new(ePropID.T, SourceEnum.Input, 1, "Acid activity for fresh catalyst (0.1 to 10.0)");

        public UOMProperty CatAmount = new(ePropID.Mass, SourceEnum.Input, 38.0, "Value in chosen units");
        public UOMProperty CatDensity = new(ePropID.Density, SourceEnum.Input, 2.5, "Catalyst density in chosen units");

        public ReformerSimulationData()
        {
            feed = feed.FillMissingData();
            calibfactors.Add("CrackP6", new(1.0, "CrackP6"));
            calibfactors.Add("CrackP7", new(1.05, "CrackP7"));
            calibfactors.Add("CrackP8", new(1.09, "CrackP8"));
            calibfactors.Add("DealkA6", new(1.0, "DealkA6"));
            calibfactors.Add("DealkA7", new(1.3, "DealkA7"));
            calibfactors.Add("DealkA8", new(1.05, "DealkA8"));
            calibfactors.Add("CycleP6", new(1.0, "CycleP6"));
            calibfactors.Add("CycleP7", new(1.0, "CycleP7"));
            calibfactors.Add("CycleP8", new(1.25, "CycleP8"));
            calibfactors.Add("CycleP9", new(1.56, "CycleP9"));
            calibfactors.Add("DehydN6", new(1.9, "DehydN6"));
            calibfactors.Add("DehydN7", new(1.8, "DehydN7"));
            calibfactors.Add("DehydN8", new(1.5, "DehydN8"));
            calibfactors.Add("DehydN9", new(1.4, "DehydN9"));
            calibfactors.Add("EquilCyc9", new(1.0, "EquilCyc9"));
        }

        public ReformerSimulationData(SerializationInfo info, StreamingContext context)
        {
            IsActive = info.GetBoolean("IsActive");
            assayBasis = (AssayBasis)info.GetValue("assayBasis", typeof(AssayBasis));
            feed = (DistPoints)info.GetValue("feed", typeof(DistPoints));
            feedPNAO = (Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty>)info.GetValue("PNAO", typeof(Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty>));
            NoRx = (int)info.GetValue("NoRx", typeof(int));
            RxDp = (List<DeltaPressure>)info.GetValue("RxDp", typeof(List<DeltaPressure>));
            RxT = (List<UOMProperty>)info.GetValue("RxT", typeof(List<UOMProperty>));
            feeddensity = (UOMProperty)info.GetValue("density", typeof(UOMProperty));
            feedflow = (UOMProperty)info.GetValue("flow", typeof(UOMProperty));
            CatAmt = (List<UOMProperty>)info.GetValue("CatAmt", typeof(List<UOMProperty>));
            H2HC = (UOMProperty)info.GetValue("H2HC", typeof(UOMProperty));
            LHSV = (UOMProperty)info.GetValue("LHSV", typeof(UOMProperty));
            inputoption = (InputOption)info.GetValue("inputoption", typeof(InputOption));
            PSep = (UOMProperty)info.GetValue("pSep", typeof(UOMProperty));
            TSep = (UOMProperty)info.GetValue("tSep", typeof(UOMProperty));
            try
            {
                RefRON = info.GetDouble("RON");
                RefMON = info.GetDouble("MON");
            }
            catch
            {
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IsActive", IsActive);
            info.AddValue("assayBasis", assayBasis);
            info.AddValue("feed", feed);
            info.AddValue("PNAO", feedPNAO);
            info.AddValue("NoRx", NoRx);
            info.AddValue("RxDp", RxDp);
            info.AddValue("RxT", RxT);
            info.AddValue("density", feeddensity, typeof(UOMProperty));
            info.AddValue("flow", feedflow, typeof(UOMProperty));
            info.AddValue("CatAmt", CatAmt);
            info.AddValue("H2HC", H2HC);
            info.AddValue("LHSV", LHSV);
            info.AddValue("inputoption", inputoption);
            info.AddValue("pSep", PSep, typeof(UOMProperty));
            info.AddValue("tSep", TSep, typeof(UOMProperty));
            info.AddValue("RON", RefRON);
            info.AddValue("MON", RefMON);
        }

        internal ReformerSimulationData Clone()
        {
            ReformerSimulationData res = new();
            res.assayBasis = this.assayBasis;
            res.CatAmt = this.CatAmt;
            res.feeddensity = this.feeddensity;
            res.feed = this.feed;
            res.feedflow = this.feedflow;
            res.H2HC = this.H2HC;
            res.LHSV = this.LHSV;
            res.Name = this.Name;
            res.NoRx = this.NoRx;
            res.feedPNAO = this.feedPNAO;
            res.Products = this.Products;
            res.PSep = this.PSep;
            res.TSep = this.TSep;
            res.RxDp = this.RxDp;
            res.RxT = this.RxT;
            res.RxP = this.RxP;
            res.shortgc = this.shortgc.Clone();
            res.fullgc = this.fullgc.Clone();
            res.Name = this.Name + ":1";

            return res;
        }
    }

    [Serializable]
    public class ReformerCalibData : ISerializable
    {
        public int NoOfProducts = 5;
        public UOMProperty NoReactors = new(ePropID.Mass, SourceEnum.Input, 3, "Number of Reactors");
        public AssayBasis AssayBasis = AssayBasis.Volume;
        public DistPoints FeedDistillation = new(new List<double>() { 102, 114, 132, 160, 178 }, TemperatureUnit.Celsius, new List<int>() { 1, 5, 50, 90, 99 }, new Density(747),enumDistType.D86,SourceEnum.Input);
        public List<RefomerFullCompList> Products = new();

        public Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> FeedPNAO =
            Tuple.Create(
                new UOMProperty(ePropID.Value, 60.87),
                new UOMProperty(ePropID.Value, 28.34),
                new UOMProperty(ePropID.Value, 10.79),
                new UOMProperty(ePropID.Value, 0.0));

        public RefomerShortCompList Shortgc = new();
        public RefomerKineticLumpList Fullgc = new();

        public List<DeltaPressure> RxDp = new() { 0.5, 0.5, 0.5, 0.5 };
        public List<Pressure> RxP = new() { 4.71, 4.21, 3.71, double.NaN };

        public List<UOMProperty> RxT = new()
        {
            new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)),
            new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)),
            new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)),
            new UOMProperty(new Temperature(double.NaN, TemperatureUnit.Celsius))
        };

        public UOMProperty feeddensity = new(new Density(0.747, DensityUnit.SG));
        public UOMProperty feedflow = new(new MassFlow(1675.6242, MassFlowUnit.te_d));
        public UOMProperty H2HC = new(ePropID.NullUnits, SourceEnum.Input, 80, "H to HC Ratio");
        public double LHSV = 1.95;
        public InputOption inputoption = InputOption.Short;
        public UOMProperty PSep = new(new Pressure(3, PressureUnit.Kg_cm2_g), SourceEnum.Input, "Separator Pressure");
        public UOMProperty TSep = new(new Temperature(38, TemperatureUnit.Celsius), SourceEnum.Input, "Separator Temperature");
        internal string Name = "Default";
        public enumMassMolarOrVol gcbasis;
        public double RefRON, RefMON;
        public List<string> streamnames = new() { "Net H2", "FG", "LPG", "REFORMATE" };
        public Dictionary<string, UOMProperty> calibfactors = new();

        public UOMProperty R1CatLoad = new(ePropID.Mass, SourceEnum.Input, 20, "Catalyst loading (% of total catalyst)");
        public UOMProperty R1Tin = new(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
        public UOMProperty R1Pin = new(ePropID.P, SourceEnum.Input, 4.71, "Inlet Pressure");
        public UOMProperty R1PDrop = new(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
        public UOMProperty R1MetActivity = new(ePropID.T, SourceEnum.Input, 1, "Metal activity for fresh catalyst (0.1 to 10.0)");
        public UOMProperty R1AcidActivity = new(ePropID.T, SourceEnum.Input, 1, "Acid activity for fresh catalyst (0.1 to 10.0)");

        public UOMProperty R2CatLoad = new(ePropID.Mass, SourceEnum.Input, 30, "Catalyst loading (% of total catalyst)");
        public UOMProperty R2Tin = new(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
        public UOMProperty R2Pin = new(ePropID.P, SourceEnum.Input, 4.21, "Inlet Pressure");
        public UOMProperty R2PDrop = new(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
        public UOMProperty R2MetActivity = new(ePropID.T, SourceEnum.Input, 1, "Metal activity for fresh catalyst (0.1 to 10.0)");
        public UOMProperty R2AcidActivity = new(ePropID.T, SourceEnum.Input, 1, "Acid activity for fresh catalyst (0.1 to 10.0)");

        public UOMProperty R3CatLoad = new(ePropID.Mass, SourceEnum.Input, 50, "Catalyst loading (% of total catalyst)");
        public UOMProperty R3Tin = new(ePropID.T, SourceEnum.Input, 510, "Inlet Temperature");
        public UOMProperty R3Pin = new(ePropID.P, SourceEnum.Input, 3.71, "Inlet Pressure");
        public UOMProperty R3PDrop = new(ePropID.DeltaP, SourceEnum.Input, 0.5, "Bed Pressure Drop");
        public UOMProperty R3MetActivity = new(ePropID.T, SourceEnum.Input, 1, "Metal activity for fresh catalyst (0.1 to 10.0)");
        public UOMProperty R3AcidActivity = new(ePropID.T, SourceEnum.Input, 1, "Acid activity for fresh catalyst (0.1 to 10.0)");

        public UOMProperty CatAmount = new(ePropID.Mass, SourceEnum.Input, 38.0, "Value in chosen units");
        public UOMProperty CatDensity = new(ePropID.Density, SourceEnum.Input, 2.5, "Catalyst density in chosen units");
        public UOMProperty furneff = new(ePropID.NullUnits, SourceEnum.Input, 70, "Furnace eff(%)");

        public ReformerCalibData()
        {
            FeedDistillation = FeedDistillation.FillMissingData();
            calibfactors.Add("CrackP6", new(1.0, "CrackP6", SourceEnum.Input));
            calibfactors.Add("CrackP7", new(1.05, "CrackP7", SourceEnum.Input));
            calibfactors.Add("CrackP8", new(1.09, "CrackP8", SourceEnum.Input));
            calibfactors.Add("DealkA6", new(1.0, "DealkA6", SourceEnum.Input));
            calibfactors.Add("DealkA7", new(1.3, "DealkA7", SourceEnum.Input));
            calibfactors.Add("DealkA8", new(1.05, "DealkA8", SourceEnum.Input));
            calibfactors.Add("CycleP6", new(1.0, "CycleP6", SourceEnum.Input));
            calibfactors.Add("CycleP7", new(1.0, "CycleP7", SourceEnum.Input));
            calibfactors.Add("CycleP8", new(1.25, "CycleP8", SourceEnum.Input));
            calibfactors.Add("CycleP9", new(1.56, "CycleP9", SourceEnum.Input));
            calibfactors.Add("DehydN6", new(1.9, "DehydN6", SourceEnum.Input));
            calibfactors.Add("DehydN7", new(1.8, "DehydN7", SourceEnum.Input));
            calibfactors.Add("DehydN8", new(1.5, "DehydN8", SourceEnum.Input));
            calibfactors.Add("DehydN9", new(1.4, "DehydN9", SourceEnum.Input));
            calibfactors.Add("EquilCyc9", new(1.0, "EquilCyc9", SourceEnum.Input));

            Products.Add(new("NetH2"));
            Products.Add(new("FG"));
            Products.Add(new("LPG"));
            Products.Add(new("REFORMATE"));
        }

        public ReformerCalibData(SerializationInfo info, StreamingContext context)
        {
            AssayBasis = (AssayBasis)info.GetValue("assayBasis", typeof(AssayBasis));
            FeedDistillation = (DistPoints)info.GetValue("feed", typeof(DistPoints));
            FeedPNAO = (Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty>)info.GetValue("PNAO", typeof(Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty>));
            RxDp = (List<DeltaPressure>)info.GetValue("RxDp", typeof(List<DeltaPressure>));
            RxT = (List<UOMProperty>)info.GetValue("RxT", typeof(List<UOMProperty>));
            feeddensity = (UOMProperty)info.GetValue("density", typeof(UOMProperty));
            feedflow = (UOMProperty)info.GetValue("flow", typeof(UOMProperty));
            H2HC = (UOMProperty)info.GetValue("H2HC", typeof(UOMProperty));
            LHSV = info.GetDouble("LHSV");
            inputoption = (InputOption)info.GetValue("inputoption", typeof(InputOption));
            PSep = (UOMProperty)info.GetValue("pSep", typeof(UOMProperty));
            TSep = (UOMProperty)info.GetValue("tSep", typeof(UOMProperty));
            Shortgc = (RefomerShortCompList)info.GetValue("shortgc", typeof(RefomerShortCompList));
            try
            {
                RefRON = info.GetDouble("RON");
                RefMON = info.GetDouble("MON");
                calibfactors = (Dictionary<string, UOMProperty>)info.GetValue("shortgc", typeof(Dictionary<string, UOMProperty>));
            }
            catch
            {
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("assayBasis", AssayBasis);
            info.AddValue("feed", FeedDistillation);
            info.AddValue("PNAO", FeedPNAO);
            info.AddValue("RxDp", RxDp);
            info.AddValue("RxT", RxT);
            info.AddValue("density", feeddensity, typeof(UOMProperty));
            info.AddValue("flow", feedflow, typeof(UOMProperty));
            info.AddValue("H2HC", H2HC);
            info.AddValue("LHSV", LHSV);
            info.AddValue("inputoption", inputoption);
            info.AddValue("pSep", PSep, typeof(UOMProperty));
            info.AddValue("tSep", TSep, typeof(UOMProperty));
            info.AddValue("RON", RefRON);
            info.AddValue("MON", RefMON);
            info.AddValue("shortgc", Shortgc);
            info.AddValue("calibfactors", calibfactors);
        }

        internal ReformerCalibData Clone()
        {
            ReformerCalibData res = new();
            res.AssayBasis = this.AssayBasis;
            res.feeddensity = this.feeddensity;
            res.FeedDistillation = this.FeedDistillation;
            res.feedflow = this.feedflow;
            res.H2HC = this.H2HC;
            res.LHSV = this.LHSV;
            res.Name = this.Name;
            res.FeedPNAO = this.FeedPNAO;
            res.Products = this.Products;
            res.PSep = this.PSep;
            res.TSep = this.TSep;
            res.RxDp = this.RxDp;
            res.RxT = this.RxT;
            res.RxP = this.RxP;
            res.Shortgc = this.Shortgc.Clone();
            res.Fullgc = this.Fullgc.Clone();
            res.Name = this.Name + ":1";

            return res;
        }
    }

    [Serializable]
    public class ReformerDesignData : ISerializable
    {
        public int NoOfProducts = 5;
        public int NoRx = 3;
        public List<double> CatAmt = new() { 20, 30, 50 };
        public InputOption inputoption = InputOption.Short;
        public UOMProperty PSep = new(new Pressure(3, PressureUnit.Kg_cm2_g), SourceEnum.Input, "Separator Pressure");
        public UOMProperty tSep = new(new Temperature(38, TemperatureUnit.Celsius), SourceEnum.Input, "Separator Temperature");
        public string Name = "Default";
        public enumMassMolarOrVol gcbasis;
        public double RefRON, RefMON;
        public List<string> streamnames = new() { "Net H2", "FG", "LPG", "REFORMATE" };

        public ReformerDesignData()
        {
        }

        public ReformerDesignData(SerializationInfo info, StreamingContext context)
        {
            NoRx = info.GetInt32("NoRx");
            CatAmt = (List<double>)info.GetValue("CatAmt", typeof(List<double>));
            inputoption = (InputOption)info.GetValue("inputoption", typeof(InputOption));
            PSep = (UOMProperty)info.GetValue("pSep", typeof(UOMProperty));
            tSep = (UOMProperty)info.GetValue("tSep", typeof(UOMProperty));
            try
            {
                RefRON = info.GetDouble("RON");
                RefMON = info.GetDouble("MON");
            }
            catch
            {
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("NoRx", NoRx);
            info.AddValue("CatAmt", CatAmt);
            info.AddValue("inputoption", inputoption);
            info.AddValue("pSep", PSep, typeof(UOMProperty));
            info.AddValue("tSep", tSep, typeof(UOMProperty));
            info.AddValue("RON", RefRON);
            info.AddValue("MON", RefMON);
        }

        internal ReformerDesignData Clone()
        {
            ReformerDesignData res = new();
            res.CatAmt = this.CatAmt;
            res.Name = this.Name;
            res.NoRx = this.NoRx;
            res.PSep = this.PSep;
            res.tSep = this.tSep;
            res.Name = this.Name + ":1";
            return res;
        }
    }

    [Serializable]
    public class ReformerDataCollection : IList<ReformerDataSet>, ISerializable
    {
        private readonly List<ReformerDataSet> datasets = new();

        public ReformerDataCollection()
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("datasets", datasets);
        }

        public ReformerDataCollection(SerializationInfo info, StreamingContext context)
        {
            datasets = (List<ReformerDataSet>)info.GetValue("datasets", type: typeof(List<ReformerDataSet>));
        }

        public ReformerDataSet this[int index] { get => ((IList<ReformerDataSet>)datasets)[index]; set => ((IList<ReformerDataSet>)datasets)[index] = value; }

        public int Count => ((ICollection<ReformerDataSet>)datasets).Count;

        public bool IsReadOnly => ((ICollection<ReformerDataSet>)datasets).IsReadOnly;

        public void Add(ReformerDataSet item)
        {
            ((ICollection<ReformerDataSet>)datasets).Add(item);
        }

        public void Clear()
        {
            ((ICollection<ReformerDataSet>)datasets).Clear();
        }

        public bool Contains(ReformerDataSet item)
        {
            return ((ICollection<ReformerDataSet>)datasets).Contains(item);
        }

        public void CopyTo(ReformerDataSet[] array, int arrayIndex)
        {
            ((ICollection<ReformerDataSet>)datasets).CopyTo(array, arrayIndex);
        }

        public IEnumerator<ReformerDataSet> GetEnumerator()
        {
            return ((IEnumerable<ReformerDataSet>)datasets).GetEnumerator();
        }

        public int IndexOf(ReformerDataSet item)
        {
            return ((IList<ReformerDataSet>)datasets).IndexOf(item);
        }

        public void Insert(int index, ReformerDataSet item)
        {
            ((IList<ReformerDataSet>)datasets).Insert(index, item);
        }

        public bool Remove(ReformerDataSet item)
        {
            return ((ICollection<ReformerDataSet>)datasets).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<ReformerDataSet>)datasets).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)datasets).GetEnumerator();
        }

        internal string[] CaseNames()
        {
            List<string> res = new();
            for (int i = 0; i < datasets.Count; i++)
            {
                res.Add(datasets[i].designdata.Name);
            };
            return res.ToArray();
        }

        internal void Delete(string currentCase)
        {
            for (int i = 0; i < datasets.Count; i++)
            {
                if (datasets[i].designdata.Name == currentCase)
                {
                    datasets.RemoveAt(i);
                    break;
                }
            }
        }

        internal ReformerDataSet dataset(string selectedText)
        {
            ReformerDataSet dataset = new();

            for (int i = 0; i < datasets.Count; i++)
            {
                if (datasets[i].Name == selectedText)
                    dataset = datasets[i];
            }
            return dataset;
        }

        internal ReformerDataSet Add()
        {
            ReformerDataSet rd = new();
            datasets.Add(rd);
            return rd;
        }
    }
}