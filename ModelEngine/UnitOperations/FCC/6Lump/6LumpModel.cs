using ModelEngine;
using Extensions;
using System.Collections.Generic;
using Units;
using Units.UOM;

namespace ModelEngine.FCC.SixLump
{
    public class _6LumpModel
    {
        private double SlipFactor = 2;
        private double CatOilRatio = 9.072;
        private Length Height = 36.5, Diameter = 1;
        private Density CatDensity = 670;
        private MassFlow FeedRate = new(230, MassFlowUnit.te_hr);
        private MassFlow CatCirc = new(2099218.81, MassFlowUnit.kg_hr);
        private MassFraction CRC = new(0.05, MassFractionUnit.MassFrac);
        private MassFraction CSC = new(0.77, MassFractionUnit.MassFrac);
        private Temperature CatTemp = new(745.5, TemperatureUnit.Celsius);
        private Temperature OilTemp = new Temperature(350, TemperatureUnit.Celsius);
        private Temperature PreheatT = new Temperature(175, TemperatureUnit.Celsius);
        private Temperature MixTemp = new Temperature(600, TemperatureUnit.Celsius);
        private Pressure RiserP = 1.5;
        private MassHeatCapacity CatalystCP = 1.1;

        public _6LumpModel()
        {
        }

        public bool Solve(Port_Material Feed)
        {
            return Solve(Feed, FeedRate, CatCirc, CatTemp, OilTemp, RiserP);
        }

        public bool Solve(Port_Material Feed, MassFlow FeedRate, MassFlow CRC, Temperature CatTemp, Temperature OilTemp, Pressure RiserP)
        {
            RiserResidenceTime residencetime = new(Height, Diameter, CatDensity);

            residencetime.Solve(Feed, RiserP, MixTemp, CatOilRatio, SlipFactor);

            Mass CatRiserHoldup = residencetime.CatRiserHoldup;

            Reactions Reacts = Rxs(Feed, out Components comps, out double[] CutMolarFlows);

            Port_Material portin = new Port_Material();
            portin.cc = comps;
            portin.MolarFlow_ = new(ePropID.MOLEF, CutMolarFlows.Sum(), SourceEnum.Input);
            portin.T_ = new(ePropID.T, MixTemp, SourceEnum.Input);
            portin.P_ = new(ePropID.P, RiserP, SourceEnum.Input);

            //FCCRiserRx Rx = new(reactions, residencetime.ResidenceTime, RateBasis.catalyst);

            FCCRiserRx Rx = new(Reacts, new Time(1, TimeUnit.s), RateBasis.catalyst);
            Rx.PortIn = portin;
            Rx.DP.Value = new(ePropID.DeltaP, 0.5, SourceEnum.Input);
            Rx.Solve(SolverMethod.Euler);

            return true;
        }

        public Reactions Rxs(Port_Material Feed, out Components rcmps, out double[] CutMolarFlows)
        {
            CompSplitter feedsplitter = new();
            feedsplitter.PortIn = Feed;
            feedsplitter.CutPoints = new TemperatureC[] { -273.15, 0, 36, 204, 350, 850 };
            feedsplitter.Solve();

            List<Port_Material> FeedOutPorts = feedsplitter.Outports;
            MixedComponent GAS = new("GAS");
            GAS.Add(FeedOutPorts[0].cc);
            Enthalpy HFormGAS = GAS.HForm25Hess;

            MixedComponent LPG = new("LPG");
            LPG.Add(FeedOutPorts[1].cc);
            Enthalpy HFORMLPG = LPG.HForm25Hess;

            MixedComponent GASOLINE = new("GASOLINE");
            GASOLINE.Add(FeedOutPorts[2].cc);
            Enthalpy HFORMGASOLINE = GASOLINE.HForm25Hess;

            MixedComponent LCO = new("LCO");
            LCO.Add(FeedOutPorts[3].cc);
            Enthalpy HFORMLCO = LCO.HForm25Hess;

            MixedComponent HCO = new("HCO");
            HCO.Add(FeedOutPorts[4].cc);
            Enthalpy HFormHCO = HCO.HForm25Hess;

            MixedComponent COKE = new("COKE");
            COKE.IsSSolid = true;
            COKE.MoleFraction = 0;

            Components ReactComps = new();
            ReactComps.Add(GAS);
            ReactComps.Add(LPG);
            ReactComps.Add(GASOLINE);
            ReactComps.Add(LCO);
            ReactComps.Add(HCO);
            ReactComps.Add(COKE);
            ReactComps.Origin = SourceEnum.Input;

            Reactions reactions = new Reactions(ReactComps);
            HCO.MoleFraction = 1;
            ReactComps.NormaliseFractions();

            reactions.Add(new FCCCrackReaction(ReactComps, "Gas Oil → Diesel", new string[] { "HCO", "", "LCO", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(7957.29, 53.9277, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Gas Oil → Gasoline", new string[] { "HCO", "", "GASOLINE", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(14433.4, 57.1866, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Gas Oil → Coke", new string[] { "HCO", "", "COKE", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(40.253, 32.4336, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Gas Oil → LPG", new string[] { "HCO", "", "LPG", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(2337.1, 51.3087, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Gas Oil → Dry Ga", new string[] { "HCO", "", "GAS", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(449.917, 48.6204, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Diesel → Coke", new string[] { "LCO", "", "COKE", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(75.282, 61.1594, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Diesel → Gasoline", new string[] { "LCO", "", "GASOLINE", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(197.933, 48.1145, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Diesel → LPG", new string[] { "LCO", "", "LPG", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(3.506, 67.7929, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Diesel → Dry Gas", new string[] { "LCO", "", "GAS", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(3.395, 64.2666, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Gasoline → LPG", new string[] { "GASOLINE", "", "LPG", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(2.189, 56.1944, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Gasoline → Dry Gas", new string[] { "GASOLINE", "", "GAS", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(1.658, 63.3192, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Gasoline → Coke", new string[] { "GASOLINE", "", "COKE", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(2.031, 61.7851, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "LPG → Dry Gas", new string[] { "LPG", "", "GAS", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(3.411, 55.5131, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "LPG → Coke", new string[] { "LPG", "", "COKE", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(0.601, 52.5482, 0, 0)));
            reactions.Add(new FCCCrackReaction(ReactComps, "Dry Gas → Coke", new string[] { "GAS", "", "COKE", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(2.196, 53.046, 0, 0)));

            rcmps = ReactComps;
            CutMolarFlows = feedsplitter.MolarYields.ToArray();

            return reactions;
        }
    }
}