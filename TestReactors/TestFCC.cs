using Extensions;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using ModelEngine.FCC.Petrof;
using Units;
using Units.UOM;

namespace TestReactors
{
    [TestClass]
    public class TestFCC
    {
        [TestMethod]
        public void Test_PetrofineModel()
        {
            // Design Data
            // Characterise Feed
            // Calculate Conversion / Riser
            // Regenerator

            Density Density = new(0.9179,DensityUnit.SG);
            Temperature MEABP, VABP, XABP;
            KinViscosity Visc;
            double MW, WPARA, CaTotal, KFact, T50_R;
            MassFlow FeedRate = 60.65;
            MassFraction Sul = 0.1;
            MassFraction Nit = new(500, MassFractionUnit.MassPPM);
            Length Diameter = 1, Height = 36.5;
            Pressure P = 2.5;
            Volume vol = 28.67;
            Temperature Triser = 535;
            double MWTotal;
            double RI20, RI60;

            Components cc = DefaultStreams.CrudeUrals();

            FCCDimensions fccdim = new(Diameter, Height, P, vol, Triser);

            DistPoints distPoints = new DistPoints(new List<double> { 382, 389, 397, 404, 420, 445, 521, 606, 643 }, TemperatureUnit.Celsius,
                new List<int> { 5, 10, 20, 30, 50, 70, 80, 90, 95 }, Density, enumDistType.D1160, SourceEnum.Input);

            FCCFeedCharacterise characterise = new(cc, FeedRate, distPoints, Nit, Sul);
            MEABP = characterise.MEABP;
            //VABP = characterise.VABP;
            XABP = characterise.XABP;
            VABP = MEABP;
            WPARA = characterise.WPARA;
            MW = characterise.MOLWT;
            Sul = characterise.SUL;
            KFact = MEABP.Rankine.Pow(1D / 3D) / Density.SG;
            T50_R = XABP;

            characterise.HighParrafin();
            characterise.LowParrafin();
            characterise.Aromatics();
            characterise.FractionDistance();

            TotalCorrelation tot = new();
            Visc = tot.Visc(T50_R, KFact);
            RI20 = tot.RI20t(Density, MEABP, VABP, MW);
            RI60 = tot.RI60t(Density, MEABP, VABP, MW);
            CaTotal = tot.CA(Density, MW, Sul, RI20, Visc);
            MWTotal = tot.MWt(Density, MEABP, VABP);

            FCCCrackFactor CracFct = new();
            CracFct.Solve(CaTotal, XABP, Nit);

            FCCCokingFactor CokeFcat = new();
            CokeFcat.Solve(CaTotal, XABP, Nit, WPARA);

            double MAT = 68;
            double Ca = CaTotal;
            double CRAC = CracFct.CRAC1;
            double molin = characterise.Mols;
            double Tavg = 538;

            FCCRiser riser = new();
            riser.Solve(FeedRate, Ca, CRAC, Tavg, MAT, molin);

            FCCRegenerator regenerator = new();
            regenerator.solve();
        }

        [TestMethod]
        public void TestPortMixing()
        {
            Components cc = new Components();

            BaseComp propane = Thermodata.GetComponent("propane");
            BaseComp nbutane = Thermodata.GetComponent("n-butane");
            BaseComp npentane = Thermodata.GetComponent("n-pentane");

            cc.Add(propane);
            cc.Add(nbutane);
            cc.Add(npentane);

            cc.SetMolFractions(new double[] { 0.2, 0.8, 0 });

            Port_Material port1 = new Port_Material();
            port1.cc = cc;
            port1.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1);
            port1.T_ = new StreamProperty(ePropID.T, 300 + 273.15);
            port1.P_ = new StreamProperty(ePropID.P, 1);
            port1.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1);

            Components cc2 = cc.Clone();
            cc2.SetMolFractions(new double[] { 0.1, 0.9, 0 });

            Port_Material port2 = new Port_Material();
            port2.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 3);
            port2.cc = cc2;
            port2.T_ = new StreamProperty(ePropID.T, 600 + 273.15);
            port2.P_ = new StreamProperty(ePropID.P, 1);

            PortList port_List = new PortList();
            port_List.Add(port1);
            port_List.Add(port2);

            Port_Material p = port_List.CombineComponents(cc.Count);
        }

        [TestMethod]
        public void Test_Feed_Cat_Flash()
        {
            Port_Material Feed = new();
            Components cc = DefaultStreams.Default();
            cc.Add(new SolidComponent("Cat", 760, 1.1));

            PseudoPlantData FeedDist1 = new PseudoPlantData(cc, new List<double> { 262.0, 335.5, 360.7, 418.6, 460.1, 500.0, 550.3, 570.3, 620.2 },
            TemperatureUnit.Celsius, new List<int>() { 1, 5, 10, 30, 50, 70, 90, 95, 99 },
            new Density(923.3, DensityUnit.kg_m3),
            enumDistType.D1160, SourceEnum.Input,
            new VolumeFlow(250, VolFlowUnit.m3_hr));

            Feed = FeedDist1.Port;
            Feed.cc.Origin = SourceEnum.Input;
            Feed.T_ = new(ePropID.T, 300 + 273.15, SourceEnum.Input);
            Feed.P_ = new(ePropID.P, 1.06, SourceEnum.Input);

            Feed.Sulfur = 0.6578; // wt%
            Feed.Nitrogen = 2178; // ppmwt
            Feed.UpdateFlows();

            Components cc2 = cc.Clone();
            cc2.ClearMoleFractions();

            Port_Material Catalyst = new();
            Catalyst.cc.Add(cc2);
            Catalyst.cc.Origin = SourceEnum.Input;

            Catalyst["Cat"].MoleFraction = 1;
            Catalyst.NormaliseMoleFractions();
            Catalyst.MF_ = new(ePropID.MF, 7 * Feed.MF);
            Catalyst.UpdateFlows();
            Catalyst.T_ = new(ePropID.T, 600 + 273.15);
            Catalyst.P_ = new(ePropID.P, 1.06);

            var solids = Catalyst.cc.Solids();

            Catalyst.Flash();  //
            Feed.Flash();
            Feed.T_ = new(ePropID.T, 25 + 273.15);
            Feed.Flash(enumFlashType.PT);

            PortList ports = new();
            ports.Add(Feed);
            ports.Add(Catalyst);

            Port_Material ppout = ports.CombineComponents(Catalyst.cc.Count);

            ppout.T_ = new(ePropID.T, 600 + 273.15);
            ppout.P_ = new(ePropID.P, 1.06);
            ppout.cc.Origin = SourceEnum.Input;
            ppout.Flash(enumFlashType.PT); // componenets doubling????

            //Components cc = solids.RemoveSolids();

            Mixer mixer = new Mixer();

            Port_Material outport = new Port_Material();

            mixer.PortIn1.ConnectPort(Feed);
            mixer.PortIn2.ConnectPort(Catalyst);
            mixer.PortOut.ConnectPort(outport);

            mixer.Solve();

            Port_Material pout = mixer.PortOut;

            pout.Flash(enumFlashType.PH);
        }

        [TestMethod]
        public void Test_6LumpModel()
        {
            Port_Material Feed = new();
            Feed.cc.Add(DefaultStreams.Default());

            PseudoPlantData FeedDist1 = new PseudoPlantData(DefaultStreams.Default(),
                new List<double> { 262.0, 335.5, 360.7, 418.6, 460.1, 500.0, 550.3, 570.3, 620.2 },
                TemperatureUnit.Celsius, new List<int>() { 1, 5, 10, 30, 50, 70, 90, 95, 99 },
                new Density(923.3, DensityUnit.kg_m3),
                enumDistType.D1160, SourceEnum.Input,
                new VolumeFlow(250, VolFlowUnit.m3_hr));

            Feed = FeedDist1.Port;
            Feed.Sulfur = 0.6578; // wt%
            Feed.Nitrogen = 2178; // ppmwt

            Feed.MolarFlow_ = new(ePropID.MOLEF, 1, SourceEnum.Input);
            Feed.T_ = new(ePropID.T, 600 + 273.15, SourceEnum.Input);
            Feed.P_ = new(ePropID.P, 1.06, SourceEnum.Input);
            Feed.cc.Origin = SourceEnum.Input;

            Feed.Flash(calcderivatives: true);

            var res = Feed.CP();

            ModelEngine.FCC.SixLump._6LumpModel model = new();
            model.Solve(Feed);
        }

        [TestMethod]
        public void TestHeatOfFormation()
        {
            BaseComp bc = Thermodata.GetComponent("Benzene");
            var res = bc.HeatFormationHess();
        }

        [TestMethod]
        public void TestHeatOFCrackingEstimate()
        {
            PseudoPlantData VGO = new PseudoPlantData(DefaultStreams.Default(),
                        new List<double> { 262.0, 335.5, 360.7, 418.6, 460.1, 500.0, 550.3, 570.3, 620.2 },
                        TemperatureUnit.Celsius, new List<int>() { 1, 5, 10, 30, 50, 70, 90, 95, 99 },
                        new Density(923.3, DensityUnit.kg_m3),
                        enumDistType.D1160, SourceEnum.Input,
                        new VolumeFlow(250, VolFlowUnit.m3_hr));

            PseudoPlantData Naphtha = new PseudoPlantData(DefaultStreams.Default(),
                        new List<double> { 36, 40, 50, 60, 70, 80, 90, 100, 105 },
                        TemperatureUnit.Celsius, new List<int>() { 1, 5, 10, 30, 50, 70, 90, 95, 99 },
                        new Density(760, DensityUnit.kg_m3),
                        enumDistType.D86, SourceEnum.Input,
                        new VolumeFlow(250, VolFlowUnit.m3_hr));

            MassEnthalpy res = FCCHeatofCracking.HeatOfCrackMass(1.2, new Temperature(535, TemperatureUnit.Celsius), VGO.Port.cc, Naphtha.Port.cc);
        }

        [TestMethod]
        public void TestFCCFeedCharacterise()
        {
            Thermodata.ShortNames.Add("N2", "Nitrogen");
            Thermodata.ShortNames.Add("O2", "Oxygen");
            Thermodata.ShortNames.Add("CO", "CO");
            Thermodata.ShortNames.Add("CO2", "CO2");
            Thermodata.ShortNames.Add("H2", "Hydrogen");
            Thermodata.ShortNames.Add("C1", "methane");
            Thermodata.ShortNames.Add("C2", "ethane");
            Thermodata.ShortNames.Add("C2=", "ethylene");
            Thermodata.ShortNames.Add("C3", "propane");
            Thermodata.ShortNames.Add("C3=", "propene");
            Thermodata.ShortNames.Add("NC4", "N-butane");
            Thermodata.ShortNames.Add("IC4", "i-butane");
            Thermodata.ShortNames.Add("IC4=", "i-butene");
            Thermodata.ShortNames.Add("NC5", "N-pentane");
            Thermodata.ShortNames.Add("IC5", "i-pentane");
            Thermodata.ShortNames.Add("NC6", "N-hexane");
            Thermodata.ShortNames.Add("IC6", "2-Mpentane");
            Thermodata.ShortNames.Add("NC7", "n-heptane");
            Thermodata.ShortNames.Add("IC7", "2-Mhexane");

            Thermodata.ShortNames.Add("1-C4=", "2-Mhexane");

            Thermodata.ShortNames.Add("c2-C4=".ToUpper(), "cis2-Butene");
            Thermodata.ShortNames.Add("t2-C4=".ToUpper(), "tr2-Butene");
            Thermodata.ShortNames.Add("C4==".ToUpper(), "13-Butadiene");
            Thermodata.ShortNames.Add("H2S".ToUpper(), "H2S");

            List<int> BPs = new List<int>() { 1, 5, 10, 30, 50, 70, 90, 95, 99 };
            Port_Material Feed = new();
            Feed.cc.Add(DefaultStreams.Default());
            Feed.cc.Add(Thermodata.ShortNames);

            PseudoPlantData FeedDist1 = new PseudoPlantData(Feed.cc, new List<double> { 313.7, 488.6, 556.2, 713.4, 813.4, 902.7, 993.8, 1026.2, 1094.4 },
                TemperatureUnit.Fahrenheit, BPs, new Density(23.6, DensityUnit.API), enumDistType.D1160, SourceEnum.Input, new VolumeFlow(71991.6, VolFlowUnit.bpd));

            PseudoPlantData FeedDist2 = new PseudoPlantData(Feed.cc, new List<double> { 555.1, 640.2, 680.8, 774.9, 845.8, 915.7, 992.2, 1022.9, 1089.7 },
                 TemperatureUnit.Fahrenheit, BPs, new Density(22.2, DensityUnit.API), enumDistType.D1160, SourceEnum.Input, new VolumeFlow(16799.9, VolFlowUnit.bpd));

            PseudoPlantData FeedDist3 = new PseudoPlantData(Feed.cc, new List<double> { 515.1, 619.1, 658.1, 735.7, 782.7, 832.5, 912.4, 951.7, 1008.2 },
                 TemperatureUnit.Fahrenheit, BPs, new Density(30.9, DensityUnit.API), enumDistType.D1160, SourceEnum.Input, new VolumeFlow(2399.4, VolFlowUnit.bpd));

            Feed = FeedDist1.Port;
            Feed.Port(FeedDist2.Port);
            Feed.Port(FeedDist3.Port);

            ComponentPlantData FG = new ComponentPlantData(new List<string> { "N2", "O2", "CO", "CO2", "H2S", "H2", "C1", "C2", "C2=", "C3", "C3=", "nC4", "iC4", "iC4=" },
                new List<double> { 13.3, 0.2, 0.1, 0.2, 0.4, 15.5, 36.2, 14.3, 17.1, 0.4, 1.8, 0.1, 0.5, 0.0 }, new GasFlow(688.6, GasFlowUnit.MSCFH));

            ComponentPlantData LPG1 = new ComponentPlantData(new List<string> { "N2", "O2", "CO", "CO2", "H2S", "H2", "C1", "C2", "C2=", "C3", "C3=", "nC4", "iC4", "iC4=" },
                new List<double> { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.4, 0.1, 0.0, 26.7, 73.1, 0.0, 0.0, 0.0 }, new LiqGasFlow(9966.1, VolFlowUnit.bpd));

            ComponentPlantData LPG2 = new ComponentPlantData(new List<string> { "N2", "O2", "CO", "CO2", "H2S", "H2", "C1", "C2", "C2=", "C3", "C3=", "nC4", "iC4", "iC4=", "1-C4=", "c2-C4=", "t2-C4=", "C4==", "nC5" },
                new List<double> { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.6, 0.8, 8.0, 34.6, 0.0, 25.6, 11.1, 15.3, 0.0, 2.6 }, new LiqGasFlow(14573.1, VolFlowUnit.bpd));

            PseudoPlantData LNaphtha = new PseudoPlantData(Feed.cc, new List<double> { 97.1, 125.7, 135.0, 161.1, 200.4, 254.0, 328.7, 356.4, 390.8, },
                TemperatureUnit.Fahrenheit, BPs, new Density(60.6, DensityUnit.API), enumDistType.D86, SourceEnum.Input, new VolumeFlow(45799.8, VolFlowUnit.bpd));

            PseudoPlantData HNaphtha = new PseudoPlantData(Feed.cc, new List<double> { 125.2, 226.4, 258.6, 307.2, 333.2, 353.5, 376.0, 385.1, 397.3 },
                TemperatureUnit.Fahrenheit, BPs, new Density(39.4, DensityUnit.API), enumDistType.D1160, SourceEnum.Input, new VolumeFlow(6582.2, VolFlowUnit.bpd));

            PseudoPlantData LLCO = new PseudoPlantData(Feed.cc, new List<double> { 266.7, 391.8, 409.5, 451.3, 494.0, 542.1, 607.5, 688.5, 755.4, },
                TemperatureUnit.Fahrenheit, BPs, new Density(19, DensityUnit.API), enumDistType.D1160, SourceEnum.Input, new VolumeFlow(17059.0, VolFlowUnit.bpd));

            PseudoPlantData HLCO = new PseudoPlantData(Feed.cc, new List<double> { 261.8, 485.1, 545.4, 614.2, 647.2, 682.1, 742.6, 775.9, 878.4 },
                TemperatureUnit.Fahrenheit, BPs, new Density(5.9, DensityUnit.API), enumDistType.D1160, SourceEnum.Input, new VolumeFlow(3256.0, VolFlowUnit.bpd));

            PseudoPlantData Bottoms = new PseudoPlantData(Feed.cc, new List<double> { 450.0, 618.2, 647.9, 720.2, 773.3, 835.2, 951.3, 1083.7, 1242.4 },
                TemperatureUnit.Fahrenheit, BPs, new Density(-5, DensityUnit.API), enumDistType.D1160, SourceEnum.Input, new VolumeFlow(2982.4, VolFlowUnit.bpd));

            List<BasePlantData> Streams = new List<BasePlantData>();
            Streams.Add(FG);
            Streams.Add(LPG1);
            Streams.Add(LPG2);
            Streams.Add(LNaphtha);
            Streams.Add(HNaphtha);
            Streams.Add(LLCO);
            Streams.Add(HLCO);
            Streams.Add(Bottoms);

            Port_Material effluent = Synthesise.SynthesisePlantData(Feed.cc, Streams);

            CompSplitter effluentsplitter = new CompSplitter();
            effluentsplitter.PortIn = effluent;
            effluentsplitter.CutPoints = new TemperatureC[] { -273.15, 36, 150, 180, 250, 350, 365, 450, 550, 650, 850 };
            effluentsplitter.Solve();
            PortList OutPorts = effluentsplitter.Ports.PortsOut();

            CompSplitter feedsplitter = new();
            feedsplitter.PortIn = Feed;
            feedsplitter.CutPoints = new TemperatureC[] { -273.15, 36, 150, 180, 250, 350, 365, 450, 550, 650, 850 };
            feedsplitter.Solve();
            PortList FeedOutPorts = feedsplitter.Ports.PortsOut();

            double[] FeedCuts = feedsplitter.MolarYields.ToArray();
            double[] ProductCuts = effluentsplitter.MolarYields.ToArray();

            FCCFeedCharacterise feedchar = new();
            feedchar.Solve(Feed);
        }
    }
}