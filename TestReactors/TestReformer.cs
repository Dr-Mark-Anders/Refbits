using ModelEngine;
using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using NaphthaReformerSI;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace TestReactors
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestReformer
    {
        public TestReformer()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use class Initialize to run code before running the first test in the class
        // [class Initialize()]
        // public  static void  Myclass Initialize(TestContext testContext) { }
        //
        // Use class Cleanup to run code after all tests in a class  have run
        // [class Cleanup()]
        // public  static void  Myclass Cleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public  void  MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public  void  MyTestCleanup() { }
        //

        [TestMethod]
        public void TestNewReformer()
        {
            CalibrationFactors calibFactors = new CalibrationFactors();

            BaseComp C1 = Thermodata.GetComponent("Methane");
            BaseComp C2 = Thermodata.GetComponent("Ethane");
            BaseComp C3 = Thermodata.GetComponent("Propane");
            BaseComp iC4 = Thermodata.GetComponent("i-butane");
            BaseComp nC4 = Thermodata.GetComponent("n-butane");
            BaseComp eb = Thermodata.GetComponent("e-benzene");
            BaseComp mx = Thermodata.GetComponent("m-Xylene");
            BaseComp ox = Thermodata.GetComponent("o-Xylene");
            BaseComp px = Thermodata.GetComponent("p-Xylene");

            Thermodata.ShortNames.Add("H2", "Hydrogen");
            Thermodata.ShortNames.Add("C1", "methane");
            Thermodata.ShortNames.Add("C2", "ethane");
            Thermodata.ShortNames.Add("C3", "propane");
            Thermodata.ShortNames.Add("NC4", "N-butane");
            Thermodata.ShortNames.Add("IC4", "i-butane");
            Thermodata.ShortNames.Add("NC5", "N-pentane");
            Thermodata.ShortNames.Add("IC5", "i-pentane");
            Thermodata.ShortNames.Add("NC6", "N-hexane");
            Thermodata.ShortNames.Add("IC6", "2-Mpentane");
            Thermodata.ShortNames.Add("NC7", "n-heptane");
            Thermodata.ShortNames.Add("IC7", "2-Mhexane");

            Thermodata.ShortNames.Add("C8P", "n-Octane");
            Thermodata.ShortNames.Add("C8CP", "113-MCC5");

            Thermodata.ShortNames.Add("PCP", "n-Pcycpentan");
            Thermodata.ShortNames.Add("ECH", "Ecyclohexane");
            Thermodata.ShortNames.Add("CH", "cyclohexane");

            Thermodata.ShortNames.Add("C9P", "n-Nonane");
            Thermodata.ShortNames.Add("C9CH", "n-Pcychexane");
            Thermodata.ShortNames.Add("C9CP", "n-Bcycpentan");
            Thermodata.ShortNames.Add("C9A", "n-PBenzene");
            Thermodata.ShortNames.Add("MCP", "Mcyclopentan");
            Thermodata.ShortNames.Add("MCH", "Mcyclohexane");
            Thermodata.ShortNames.Add("C7CP", "Ecyclopentan");
            Thermodata.ShortNames.Add("DMCH", "11-Mcychexan");

            Thermodata.ShortNames.Add("TOL", "Toluene");
            Thermodata.ShortNames.Add("EB", "E-Benzene");
            Thermodata.ShortNames.Add("BEN", "benzene");
            Thermodata.ShortNames.Add("PX", "p-xylene");
            Thermodata.ShortNames.Add("MX", "m-xylene");
            Thermodata.ShortNames.Add("OX", "o-xylene");
            Thermodata.ShortNames.Add("C8A", "MixedXylenes");
            Thermodata.ShortNames.Add("Hydrocracked", "C7Hydrocrack");

            MixedComponent mix0 = new("C8A");
            mix0.Add(eb, 0.096);
            mix0.Add(mx, 0.209);
            mix0.Add(ox, 0.35);
            mix0.Add(px, 0.345);
            mix0.Normalise();

            MixedComponent mix1 = new("XYLENES");
            mix1.Add(mx, 0.232);
            mix1.Add(ox, 0.395);
            mix1.Add(px, 0.373);
            mix1.Normalise();

            MixedComponents mixedcomps = new MixedComponents();
            mixedcomps.Add(mix0);
            mixedcomps.Add(mix1);

            YieldPattern C8A = new("C8A", Tuple.Create(
                 new string[4] { "E-Benzene", "m-xylene", "o-xylene", "p-xylene" },
                 new double[4] { 0.096, 0.209, 0.35, 0.345 }));

            YieldPattern Xylenes = new("Xylenes", Tuple.Create(
                new string[3] { "p-xylene", "m-xylene", "o-xylene" },
                  new double[3] { 0.232, 0.395, 0.373 }));

            YieldPattern C9PHydrocracked = new("C9PHydrocracked", Tuple.Create(
                 new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                 new double[23] { 0, 0.176, 0.283, 0.286, 0.091, 0.164, 0.172, 0.083, 0.206, 0.08, 0, 0, 0, 0.19, 0.093, 0, 0, 0, 0.176, 0, 0, 0, 0 }));

            YieldPattern C9CHHydrocracked = new("C9CHHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.176, 0.283, 0.308, 0.083, 0.15, 0.15, 0.083, 0.206, 0.102, 0, 0, 0, 0.19, 0.093, 0, 0, 0, 0.176, 0, 0, 0, 0 }));

            YieldPattern C9CPHydrocracked = new("C9CPHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.176, 0.283, 0.308, 0.083, 0.15, 0.15, 0.083, 0.206, 0.102, 0, 0, 0, 0.19, 0.093, 0, 0, 0, 0.176, 0, 0, 0, 0 }));

            YieldPattern C8PHydrocracked = new("C8PHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.199, 0.295, 0.376, 0.092, 0.168, 0.26, 0.116, 0.215, 0.08, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern ECHHydrocracked = new("ECHHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.199, 0.322, 0.349, 0.092, 0.168, 0.233, 0.116, 0.215, 0.107, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern DMCHHydrocracked = new("DMCHHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.199, 0.322, 0.349, 0.092, 0.168, 0.233, 0.116, 0.215, 0.107, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern PCPHydrocracked = new("PCPHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.199, 0.322, 0.349, 0.092, 0.168, 0.233, 0.116, 0.215, 0.107, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern C8CPHydrocracked = new("C8CPHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.199, 0.322, 0.349, 0.092, 0.168, 0.233, 0.116, 0.215, 0.107, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern IC7Hydrocracked = new("IC7Hydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.237, 0.366, 0.397, 0.141, 0.256, 0.234, 0.132, 0.158, 0.079, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern NC7Hydrocracked = new("NC7Hydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.237, 0.366, 0.397, 0.141, 0.256, 0.234, 0.132, 0.158, 0.079, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern MCHHydrocracked = new("MCHHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.237, 0.366, 0.397, 0.141, 0.256, 0.234, 0.132, 0.158, 0.079, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern C7CPHydrocracked = new("C7CPHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.237, 0.366, 0.397, 0.141, 0.256, 0.234, 0.132, 0.158, 0.079, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern IC6Hydrocracked = new("IC6Hydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.288, 0.462, 0.5, 0.164, 0.298, 0.192, 0.096, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern NC6Hydrocracked = new("NC6Hydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.288, 0.462, 0.5, 0.164, 0.298, 0.192, 0.096, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern CHHydrocracked = new("CHHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.288, 0.462, 0.5, 0.164, 0.298, 0.192, 0.096, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern MCPHydrocracked = new("MCPHydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.288, 0.462, 0.5, 0.164, 0.298, 0.192, 0.096, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern IC5Hydrocracked = new("IC5Hydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.5, 0.5, 0.5, 0.2, 0.3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern NC5Hydrocracked = new("NC5Hydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.5, 0.5, 0.5, 0.2, 0.3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern IC4Hydrocracked = new("IC4Hydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.665, 0.67, 0.665, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            YieldPattern NC4Hydrocracked = new("NC4Hydrocracked", Tuple.Create(
                  new string[23] { "H2", "C1", "C2", "C3", "IC4", "NC4", "IC5", "NC5", "IC6", "NC6", "CH", "MCP", "BEN", "IC7", "NC7", "MCH", "C7CP", "TOL", "C8P", "ECH", "DMCH", "PCP", "C8CP" },
                  new double[23] { 0, 0.665, 0.67, 0.665, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            Thermodata.Mix.Add(mixedcomps);

            Components feed = new Components();
            feed.Add(Thermodata.GetComponent("H2"));
            feed.Add(Thermodata.GetComponent("C1"));
            feed.Add(Thermodata.GetComponent("C2"));
            feed.Add(Thermodata.GetComponent("C3"));
            feed.Add(Thermodata.GetComponent("IC4"));
            feed.Add(Thermodata.GetComponent("NC4"));
            feed.Add(Thermodata.GetComponent("IC5"));
            feed.Add(Thermodata.GetComponent("NC5"));
            feed.Add(Thermodata.GetComponent("IC6"));
            feed.Add(Thermodata.GetComponent("NC6"));
            feed.Add(Thermodata.GetComponent("CH"));
            feed.Add(Thermodata.GetComponent("MCP"));
            feed.Add(Thermodata.GetComponent("BEN"));
            feed.Add(Thermodata.GetComponent("IC7"));
            feed.Add(Thermodata.GetComponent("NC7"));
            feed.Add(Thermodata.GetComponent("MCH"));
            feed.Add(Thermodata.GetComponent("C7CP"));
            feed.Add(Thermodata.GetComponent("TOL"));
            feed.Add(Thermodata.GetComponent("C8P"));
            feed.Add(Thermodata.GetComponent("ECH"));
            feed.Add(Thermodata.GetComponent("DMCH"));
            feed.Add(Thermodata.GetComponent("PCP"));
            feed.Add(Thermodata.GetComponent("C8CP"));
            feed.Add(Thermodata.GetComponent("EB"));
            feed.Add(Thermodata.GetComponent("PX"));
            feed.Add(Thermodata.GetComponent("MX"));
            feed.Add(Thermodata.GetComponent("OX"));
            feed.Add(Thermodata.GetComponent("C9P"));
            feed.Add(Thermodata.GetComponent("C9CH"));
            feed.Add(Thermodata.GetComponent("C9CP"));
            feed.Add(Thermodata.GetComponent("C9A"));

            // mixed components
            //feed.Add(Thermodata.GetComponent("C8A"));

            Reactions reactions = new Reactions(feed);

            reactions.Add(new Reaction(feed, "CH ↔ BEN + 3H2", new string[] { "Cyclohexane", "", "Benzene", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(8.68391, 60.91098, 47.71902, 220.65263, ReformerReactionType.CHDehydrogTerm)));
            reactions.Add(new Reaction(feed, "MCP → BEN + 3H2", new string[] { "Mcyclopentan", "", "Benzene", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(8.40468, 60.91098, 0, 0, ReformerReactionType.CPDehydrogTerm)));
            reactions.Add(new Reaction(feed, "MCH ↔ TOL + 3H2", new string[] { "Mcyclohexane", "", "Toluene", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(9.16635, 64.29493, 47.45232, 215.52336, ReformerReactionType.CHDehydrogTerm)));
            reactions.Add(new Reaction(feed, "C7CP ↔  TOL + 3H2", new string[] { "Ecyclopentan", "", "Toluene", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(8.87161, 64.29493, 43.0002, 195.42703, ReformerReactionType.CPDehydrogTerm)));
            reactions.Add(new Reaction(feed, "ECH ↔ EB + 3Hydrogen", new string[] { "Ecyclohexane", "", "E-Benzene", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.18649, 77.15392, 47.74953, 212.7123, ReformerReactionType.CHDehydrogTerm)));
            reactions.Add(new SumProductReaction(feed, "DMCH ↔ MixedXylenes + 3Hydrogen", new string[] { "11-Mcychexan", "", "Xylenes", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.14056, 77.15392, 47.48208, 208.35669, ReformerReactionType.CHDehydrogTerm), Xylenes));
            reactions.Add(new Reaction(feed, "PCP ↔ EB + 3H2", new string[] { "PCP", "", "E-Benzene", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(10.91626, 77.15392, 44.22683, 192.50512, ReformerReactionType.CPDehydrogTerm)));
            reactions.Add(new SumProductReaction(feed, "C8CP ↔ MixedXylenes + 3H2", new string[] { "C8CP", "", "Xylenes", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(10.87813, 77.15392, 47.7315, 213.52846, ReformerReactionType.CPDehydrogTerm), Xylenes));
            reactions.Add(new Reaction(feed, "C9CH ↔ C9A + 3H2", new string[] { "C9CH", "", "C9A", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(12.0676, 82.48015, 47.90527, 206.31514, ReformerReactionType.CHDehydrogTerm)));
            reactions.Add(new Reaction(feed, "C9CP ↔ C9A + 3H2", new string[] { "C9CP", "", "C9A", "Hydrogen" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.CPDehydrogTerm)));

            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));
            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));

            reactions.Add(new Reaction(feed, "OX ↔ PX", new string[] { "o-Xylene", "", "p-Xylene", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(12.908, 96.99666, -0.54851, -2.95608, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "PX ↔ MX", new string[] { "p-Xylene", "", "m-Xylene", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(12.908, 96.99666, 0.79004, -0.0545, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "MX ↔ OX", new string[] { "m-Xylene", "", "o-Xylene", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(12.908, 96.99666, -0.24153, 3.01243, ReformerReactionType.IsomTerm)));

            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));

            reactions.Add(new Reaction(feed, "CH ↔ MCP", new string[] { "CH", "", "MCP", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(8.2122, 96.99666, 4.88114, 15.60491, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "MCH ↔ C7CP", new string[] { "Mcyclohexane", "", "C7CP", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(9.5985, 96.99666, 4.45838, 20.13789, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "ECH ↔ PCP", new string[] { "ECH", "", "PCP", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(10.158, 96.99666, 3.52239, 20.20671, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "PCP ↔ C8CP", new string[] { "PCP", "", "C8CP", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(11.208, 96.99666, 0.29286, -12.87422, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "C8CP ↔ DMCH", new string[] { "C8CP", "", "DMCH", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(10.158, 96.99666, -3.43419, -16.26587, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "DMCH ↔ ECH", new string[] { "DMCH", "", "ECH", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(11.208, 96.99666, -0.38091, 8.93431, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "C9CH ↔ C9CP", new string[] { "C9CH", "", "C9CP", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(10.5148, 96.99666, 3.84955, 17.90604, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "NC7 ↔ IC7", new string[] { "NC7", "", "IC7", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(2.9775, 59.29729, 0.39547, -8.31492, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "NC6 ↔ IC6", new string[] { "NC6", "", "IC6", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(5.119, 78.27169, -0.16411, -7.74171, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "NC5 ↔ IC5", new string[] { "NC5", "", "IC5", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(3.6946, 71.56506, -0.60385, -7.73987, ReformerReactionType.IsomTerm)));
            reactions.Add(new Reaction(feed, "NC4 ↔ IC4", new string[] { "NC4", "", "IC4", "" }, new int[] { -1, 0, 1, 0 }, new Arrhenius(23.947, 212.70907, -1.81158, -8.24194, ReformerReactionType.IsomTerm)));

            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));
            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));

            reactions.Add(new Reaction(feed, "CH + H2 ↔ NC6", new string[] { "CH", "Hydrogen", "NC6", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(40.6743, 277.13333, -4.98164, -45.18335, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "MCP + H2 ↔ NC6", new string[] { "MCP", "Hydrogen", "NC6", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(40.6097, 277.13333, -9.8627, -60.78781, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "MCP + Hydrogen ↔ IC6", new string[] { "MCP", "Hydrogen", "IC6", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(40.5407, 277.13333, -9.94232, -65.68521, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "MCH + H2 ↔ NC7", new string[] { "Mcyclohexane", "Hydrogen", "NC7", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(41.0806, 277.13333, -6.34038, -36.7742, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "MCH + H2 ↔ IC7", new string[] { "Mcyclohexane", "Hydrogen", "IC7", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(41.0806, 277.13333, -6.10107, -42.13627, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "C7CP + H2 ↔ NC7", new string[] { "C7CP", "Hydrogen", "NC7", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(40.8983, 277.13333, -10.35908, -58.19892, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "C7CP + H2 ↔ IC7", new string[] { "C7CP", "Hydrogen", "IC7", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(40.8983, 277.13333, -10.39403, -65.07829, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "ECH + Hydrogen ↔ C8P", new string[] { "ECH", "Hydrogen", "C8P", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(31.57968, 221.70666, -5.55248, -41.66191, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "DMCH + H2 ↔ C8P", new string[] { "DMCH", "Hydrogen", "C8P", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(32.04248, 221.70666, -5.67535, -35.6338, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "PCP + H2 ↔ C8P", new string[] { "PCP", "Hydrogen", "C8P", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(31.57968, 221.70666, -9.43751, -61.74022, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "C8CP + H2 ↔ C8P", new string[] { "C8CP", "Hydrogen", "C8P", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(32.04248, 221.70666, -8.94501, -51.81469, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "C9CH + H2 ↔ C9P", new string[] { "C9CH", "Hydrogen", "C9P", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(23.95141, 172.32008, -5.40945, -29.92347, ReformerReactionType.CyclTerm)));
            reactions.Add(new Reaction(feed, "C9CP + H2 ↔ C9P", new string[] { "C9CP", "H2", "C9P", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(23.06294, 172.32008, -9.25892, -47.82905, ReformerReactionType.CyclTerm)));

            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));
            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));

            reactions.Add(new HydroCrackReaction(feed, "C9P + H2 → Hydrocracked products", new string[] { "C9P", "Hydrogen", "C9PHydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(22.84403, 176.48689, 0.0453, -54.84237, ReformerReactionType.PCrackTerm), C9PHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "C8P + H2 → Hydrocracked products", new string[] { "C8P", "Hydrogen", "C8PHydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(26.70394, 212.70907, 0, 0, ReformerReactionType.PCrackTerm), C8PHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "IC7 + H2 → Hydrocracked products", new string[] { "IC7", "Hydrogen", "IC7Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(26.39942, 207.33972, 0, 0, ReformerReactionType.PCrackTerm), IC7Hydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "NC7 + H2 → Hydrocracked products", new string[] { "NC7", "Hydrogen", "NC7Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(25.8799, 202.58006, 0, 0, ReformerReactionType.PCrackTerm), NC7Hydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "IC6 + H2 → Hydrocracked products", new string[] { "IC6", "Hydrogen", "IC6Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(27.88949, 223.59117, 0, 0, ReformerReactionType.PCrackTerm), IC6Hydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "NC6 + H2 → Hydrocracked products", new string[] { "NC6", "Hydrogen", "NC6Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(27.26899, 218.93533, 0, 0, ReformerReactionType.PCrackTerm), NC6Hydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "IC5 + H2 → Hydrocracked products", new string[] { "IC5", "Hydrogen", "IC5Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(30.34819, 239.94204, 0, 0, ReformerReactionType.PCrackTerm), IC5Hydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "NC5 + H2 → Hydrocracked products", new string[] { "NC5", "Hydrogen", "NC5Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(26.76859, 216.33028, 0, 0, ReformerReactionType.PCrackTerm), NC5Hydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "IC4 + H2 → Hydrocracked products", new string[] { "IC4", "Hydrogen", "IC4Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(23.63919, 212.70907, 0, 0, ReformerReactionType.PCrackTerm), IC4Hydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "NC4 + H2 → Hydrocracked products", new string[] { "NC4", "Hydrogen", "NC4Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new Arrhenius(25.88269, 218.93533, 0, 0, ReformerReactionType.PCrackTerm), NC4Hydrocracked));

            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));
            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));

            reactions.Add(new Reaction(feed, "MCH + H2 ↔ CH + C1", new string[] { "Mcyclohexane", "Hydrogen", "CH", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(12.69022, 147.80444, -1.25772, -53.34123, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "C7CP + H2 ↔ MCP + C1", new string[] { "C7CP", "Hydrogen", "MCP", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(12.69022, 147.80444, -0.83495, -57.87606, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "ECH + H2 ↔ MCH + C1", new string[] { "ECH", "Hydrogen", "Mcyclohexane", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(13.24982, 147.80444, 0.05072, -65.5993, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "DMCH + H2 ↔ MCH + C1", new string[] { "DMCH", "Hydrogen", "Mcyclohexane", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(13.24982, 147.80444, -0.33178, -56.67561, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "PCP + H2 ↔ C7CP + C1", new string[] { "PCP", "Hydrogen", "C7CP", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(13.24982, 147.80444, 0.9867, -65.66628, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "C8CP + H2 ↔ C7CP + C1", new string[] { "C8CP", "Hydrogen", "C7CP", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(13.24982, 147.80444, 0.6948, -52.78466, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "C9CH + H2 ↔ ECH + C1", new string[] { "C9CH", "Hydrogen", "ECH", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(13.60372, 147.80444, -0.60358, -48.24983, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "C9CH + H2 ↔ DMCH + C1", new string[] { "C9CH", "Hydrogen", "DMCH", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(13.60372, 147.80444, -0.22108, -57.17353, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "C9CP + H2 ↔ PCP + C1", new string[] { "C9CP", "Hydrogen", "PCP", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(13.60372, 147.80444, -0.93067, -45.9487, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "C9CP + H2 ↔ C8CP + C1", new string[] { "C9CP", "Hydrogen", "C8CP", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(13.30372, 147.80444, -0.63876, -58.83032, ReformerReactionType.DealkTerm)));
            reactions.Add(new Reaction(feed, "TOL + H2 ↔ BEN + C1", new string[] { "Toluene", "Hydrogen", "BEN", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(9.18362, 147.80444, -0.99102, -48.21196, ReformerReactionType.DealkTerm)));
            reactions.Add(new SumProductReaction(feed, "C8A + H2 ↔ TOL + C1", new string[] { "C8A", "Hydrogen", "Toluene", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(8.07386, 113.69572, -0.66898, -50.7893, ReformerReactionType.DealkTerm), C8A));
            reactions.Add(new SumProductReaction(feed, "C9A + H2 ↔ C8A + C1", new string[] { "C9A", "Hydrogen", "C8A", "C1" }, new int[] { -1, -1, 1, 1 }, new Arrhenius(14.48641, 140.76613, -0.33683, -53.85162, ReformerReactionType.DealkTerm), C8A));

            reactions.Add(new Reaction(feed, "Dummy", new string[] { "", "", "", "" }, new int[] { -1, 0, 1, 3 }, new Arrhenius(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.none)));

            reactions.Add(new HydroCrackReaction(feed, "C9CH + 2H2 → Hydrocracked products", new string[] { "C9CH", "Hydrogen", "C9CHHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(18.1665, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), C9CHHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "C9CP + 2H2 → Hydrocracked products", new string[] { "C9CP", "Hydrogen", "C9CPHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(16.8755, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), C9CPHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "ECH + 2H2 → Hydrocracked products", new string[] { "ECH", "Hydrogen", "ECHHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(17.1856, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), ECHHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "DMCH + 2H2 → Hydrocracked products", new string[] { "DMCH", "Hydrogen", "DMCHHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(17.1856, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), DMCHHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "PCP + 2H2 → Hydrocracked products", new string[] { "PCP", "Hydrogen", "PCPHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(15.8967, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), PCPHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "C8CP + 2H2 → Hydrocracked products", new string[] { "C8CP", "Hydrogen", "C8CPHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(15.9867, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), C8CPHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "MCH + 2H2 → Hydrocracked products", new string[] { "Mcyclohexane", "Hydrogen", "MCHHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(16.087, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), MCHHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "C7CP + 2H2 → Hydrocracked products", new string[] { "C7CP", "Hydrogen", "C7CPHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(14.7828, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), C7CPHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "CH + 2H2 → Hydrocracked products", new string[] { "CH", "Hydrogen", "CHHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(15.8639, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), CHHydrocracked));
            reactions.Add(new HydroCrackReaction(feed, "MCP + 2H2 → Hydrocracked products", new string[] { "MCP", "Hydrogen", "MCPHydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new Arrhenius(14.5545, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), MCPHydrocracked));

            feed.SetMolFractions(new double[]{0,0,0,0,0,0,0,0,1.75E-05,2.64E-05,0.007745686,0.009106834,0.008764042,0.029432987,0.033075045,0.017266401,0.01443287,0.030510695,
                                0.107090268,0.006525965,0.027807981,0.001071872,0.018002421,0.00449198,0.002582423,0.010836648,0.00575869,0.396609218,0.097705093,0.096777513,0.074361396 });

            Port_Material p = new(feed);
            p.MF_ = new StreamProperty(ePropID.MF, 1676.0 / 24 * 1000, SourceEnum.Input); //te/hr
            p.P_ = new StreamProperty(ePropID.P, 5, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 510 + 273.15, SourceEnum.Input);
            p.Flash();

            //double NaphthaMoldeFeed = p.MolarFlow;

            Components rec = new(feed);

            rec.SetMolFractions(new double[] { 1.0, 0.8, 0.065, 0.058, 0.013, 0.02, 0.012, 0.007, 0.007, 0.003, 0.0, 0.0, 0.003, 0.005, 0.002, 0.0, 0.0, 0.005, 0.002,
                                0.0, 0.0, 0.0, 0.0, 0.001, 0.0, 0.001, 0.0, 0.001, 0.0, 0.0, 0.001});

            double[] MoleFractions = null;
            double[] OldMoleFractions = null;
            bool IsFinished = false;
            SimpleFlash drum = new();
            double RecMoleFLow;

            do
            {
                double HCPCT = feed.HydrocarbonPCT;
                double RecHPCT = rec["hydrogen"].MoleFraction;
                double H2HCRatio = 2.2;
                RecMoleFLow = p.MolarFlow_ * HCPCT * H2HCRatio / RecHPCT;
                // double RecMoleFLow = p.MolarFlow * H2HCRatio; // To Match JRS but a bit dubious, not hactually H2/HC

                //Components cc = reactions.GetComponents();
                Components CombinedFeed = feed.Combine(rec, p.MolarFlow_.BaseValue, RecMoleFLow);
                CombinedFeed.NormaliseFractions();

                Port_Material portin = new Port_Material();
                portin.cc = CombinedFeed;
                portin.cc.Origin = SourceEnum.Input;
                portin.MolarFlow_ = new StreamProperty(ePropID.MOLEF, p.MolarFlow_ + RecMoleFLow, SourceEnum.Input); //te/hr
                portin.P_ = new StreamProperty(ePropID.P, new Pressure(4.71, PressureUnit.Kg_cm2_g), SourceEnum.Input);
                portin.T_ = new StreamProperty(ePropID.T, 510 + 273.15, SourceEnum.Input);
                portin.Flash();

                /*          CombinedFeed.SetMolFractions(new double[]{0.654084717,0.00887289,0.009101431,0.008900638,0.002476957,0.004155299,0.003123386,0.001379783,0.00172069,
                            0.000557446,0.00230531,0.002709267,0.00401729,0.010078829,0.01014497,0.005138268,0.004303116,0.010997774,0.032280556,
                            0.001940729,0.008270175,0.000319669,0.005358071,0.001533702,0.000998001,0.00371245,0.001914907,0.118388158,0.029052048,0.028778265,0.023385209, 0, 0, 0 });*/

                Length diameter = 2.56;
                Length length = 10;
                Density CatBulkDen = new Density(650);
                Mass CatalystWeight = new Mass(12659.23, MassUnits.kg);

                PlugFlowRx Rx = new(reactions, diameter, length, CatBulkDen, CatalystWeight, RateBasis.catalyst);
                Rx.PortIn = portin;
                Rx.DP.Value = new(ePropID.DeltaP, 0.5, SourceEnum.Input);
                Rx.Solve(SolverMethod.Euler);

                Rx.PortOut.Flash();

                PlugFlowRx Rx2 = new(reactions, diameter, length, CatBulkDen, CatalystWeight, RateBasis.catalyst);
                Rx2.PortIn = Rx.PortOut.Clone();
                Rx2.PortIn.H_.Clear();
                Rx2.PortIn.T_ = new StreamProperty(ePropID.T, 510 + 273.15, SourceEnum.Input);
                Rx2.DP.Value = new(ePropID.DeltaP, 0.5, SourceEnum.Input);
                // Rx2.PortIn.Flash();
                Rx2.Solve(SolverMethod.Euler);

                Rx2.PortOut.Flash();

                PlugFlowRx Rx3 = new(reactions, diameter, length, CatBulkDen, CatalystWeight, RateBasis.catalyst);
                Rx3.PortIn = Rx2.PortOut.Clone();
                Rx3.PortIn.H_.Clear();
                Rx3.PortIn.T_ = new StreamProperty(ePropID.T, 510 + 273.15, SourceEnum.Input);
                Rx3.DP.Value = new(ePropID.DeltaP, 0.5, SourceEnum.Input);
                // Rx3.PortIn.Flash();
                Rx3.Solve(SolverMethod.Euler);

                Port_Material portout = Rx.PortOut;

                //drum = new();
                drum.PortIn = Rx3.PortOut.Clone();
                drum.PortIn.P_ = new StreamProperty(ePropID.P, 2.2, SourceEnum.Input);
                drum.PortIn.T_ = new StreamProperty(ePropID.T, 38, SourceEnum.Input);
                drum.PortIn.H_.Clear();
                drum.Solve();

                rec.SetMolFractions(drum.PortOutV.MoleFractions);

                double[] Liq = drum.PortOutL.MoleFractions;

                IsFinished = false;

                if (OldMoleFractions != null && MoleFractions != null)
                {
                    double A = MoleFractions.SumDifference(OldMoleFractions);

                    if (Math.Abs((A)) > 1)
                        IsFinished = false;
                    else
                        IsFinished = true;
                }

                MoleFractions = drum.PortOutV.MoleFractions;
                OldMoleFractions = MoleFractions;
            } while (!IsFinished);

            double FlashGasFlow = drum.PortOutV.MolarFlow_;

            double NetGas = FlashGasFlow - RecMoleFLow;
        }

        [TestMethod]
        public void TestNewReformer2()
        {
            ReactionPackage reactorPackage = new ReformerReactionPackage();

            Components feed = reactorPackage.cc;

            feed.SetMolFractions(new double[]{0,0,0,0,0,0,0,0,1.75E-05,2.64E-05,0.007745686,0.009106834,0.008764042,0.029432987,0.033075045,0.017266401,0.01443287,0.030510695,
                                0.107090268,0.006525965,0.027807981,0.001071872,0.018002421,0.00449198,0.002582423,0.010836648,0.00575869,0.396609218,0.097705093,0.096777513,0.074361396 });

            Port_Material p = new(feed);
            p.MF_ = new StreamProperty(ePropID.MF, 1676.0 / 24 * 1000, SourceEnum.Input); //te/hr
            p.P_ = new StreamProperty(ePropID.P, 5, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 510 + 273.15, SourceEnum.Input);
            p.Flash();

            Components rec = new(feed);

            rec.SetMolFractions(new double[] { 1.0, 0.8, 0.065, 0.058, 0.013, 0.02, 0.012, 0.007, 0.007, 0.003, 0.0, 0.0, 0.003, 0.005, 0.002, 0.0, 0.0, 0.005, 0.002,
                                0.0, 0.0, 0.0, 0.0, 0.001, 0.0, 0.001, 0.0, 0.001, 0.0, 0.0, 0.001});

            double[] MoleFractions = null;
            double[] OldMoleFractions = null;
            bool IsFinished = false;
            SimpleFlash drum = new();
            double RecMoleFLow;

            do
            {
                double HCPCT = feed.HydrocarbonPCT;
                double RecHPCT = rec["hydrogen"].MoleFraction;
                double H2HCRatio = 2.2;
                RecMoleFLow = p.MolarFlow_ * HCPCT * H2HCRatio / RecHPCT;
                // double RecMoleFLow = p.MolarFlow * H2HCRatio; // To Match JRS but a bit dubious, not hactually H2/HC

                //Components cc = reactions.GetComponents();
                Components CombinedFeed = feed.Combine(rec, p.MolarFlow_.BaseValue, RecMoleFLow);
                CombinedFeed.NormaliseFractions();

                Port_Material portin = new Port_Material();
                portin.cc = CombinedFeed;
                portin.cc.Origin = SourceEnum.Input;
                portin.MolarFlow_ = new StreamProperty(ePropID.MOLEF, p.MolarFlow_ + RecMoleFLow, SourceEnum.Input); //te/hr
                portin.P_ = new StreamProperty(ePropID.P, new Pressure(4.71, PressureUnit.Kg_cm2_g), SourceEnum.Input);
                portin.T_ = new StreamProperty(ePropID.T, 510 + 273.15, SourceEnum.Input);
                portin.Flash();

                Length diameter = 2.56;
                Length length = 10;
                Density CatBulkDen = new Density(650);
                Mass CatalystWeight = new Mass(12659.23, MassUnits.kg);

                PlugFlowRx Rx = new(reactorPackage.Reactions, diameter, length, CatBulkDen, CatalystWeight, RateBasis.catalyst);
                Rx.PortIn = portin;
                Rx.DP.Value = new(ePropID.DeltaP, 0.5, SourceEnum.Input);
                Rx.Solve(SolverMethod.Euler);

                Rx.PortOut.Flash();

                PlugFlowRx Rx2 = new(reactorPackage.Reactions, diameter, length, CatBulkDen, CatalystWeight, RateBasis.catalyst);
                Rx2.PortIn = Rx.PortOut.Clone();
                Rx2.PortIn.H_.Clear();
                Rx2.PortIn.T_ = new StreamProperty(ePropID.T, 510 + 273.15, SourceEnum.Input);
                Rx2.DP.Value = new(ePropID.DeltaP, 0.5, SourceEnum.Input);
                // Rx2.PortIn.Flash();
                Rx2.Solve(SolverMethod.Euler);

                Rx2.PortOut.Flash();

                PlugFlowRx Rx3 = new(reactorPackage.Reactions, diameter, length, CatBulkDen, CatalystWeight, RateBasis.catalyst);
                Rx3.PortIn = Rx2.PortOut.Clone();
                Rx3.PortIn.H_.Clear();
                Rx3.PortIn.T_ = new StreamProperty(ePropID.T, 510 + 273.15, SourceEnum.Input);
                Rx3.DP.Value = new(ePropID.DeltaP, 0.5, SourceEnum.Input);
                // Rx3.PortIn.Flash();
                Rx3.Solve(SolverMethod.Euler);

                Port_Material portout = Rx.PortOut;

                //drum = new();
                drum.PortIn = Rx3.PortOut.Clone();
                drum.PortIn.P_ = new StreamProperty(ePropID.P, 2.2, SourceEnum.Input);
                drum.PortIn.T_ = new StreamProperty(ePropID.T, 38, SourceEnum.Input);
                drum.PortIn.H_.Clear();
                drum.Solve();

                rec.SetMolFractions(drum.PortOutV.MoleFractions);

                double[] Liq = drum.PortOutL.MoleFractions;

                IsFinished = false;

                if (OldMoleFractions != null && MoleFractions != null)
                {
                    double A = MoleFractions.SumDifference(OldMoleFractions);

                    if (Math.Abs((A)) > 1)
                        IsFinished = false;
                    else
                        IsFinished = true;
                }

                MoleFractions = drum.PortOutV.MoleFractions;
                OldMoleFractions = MoleFractions;
            } while (!IsFinished);

            double FlashGasFlow = drum.PortOutV.MolarFlow_;
            double NetGas = FlashGasFlow - RecMoleFLow;
        }

        #endregion Additional test attributes

        [TestMethod]
        public void TestReformer1()
        {
            //NapReformer NapRef = new  NapReformer();
            NapReformerSI NapRef = new NapReformerSI();
            AssayBasis assayBasis = AssayBasis.Volume;
            UOMProperty Psep;
            UOMProperty Tsep;

            Psep = new UOMProperty(new Pressure(2.5, PressureUnit.Kg_cm2_g), SourceEnum.Input, "Separator Pressure ");
            Tsep = new UOMProperty(new Temperature(38, TemperatureUnit.Celsius), SourceEnum.Input, "Separator Temperature ");

            var watch = Stopwatch.StartNew();

            DistPoints Feed = new DistPoints();
            Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> PNAO;
            double[] FeedComponents = new double[31];
            Pressure[] RxP = new Pressure[4];
            RxP[0].kg_cm2_g = 4.71;
            RxP[1].kg_cm2_g = 4.21;
            RxP[2].kg_cm2_g = 3.71;
            RxP[3].kg_cm2_g = double.NaN;

            DeltaPressure[] RxDp = new DeltaPressure[4];
            RxDp[0].kg_cm2 = 0.5;
            RxDp[1].kg_cm2 = 0.5;
            RxDp[2].kg_cm2 = 0.5;
            RxDp[3].kg_cm2 = double.NaN;

            List<UOMProperty> RxT = new List<UOMProperty>(4);
            RxT.Add(new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)));
            RxT.Add(new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)));
            RxT.Add(new UOMProperty(new Temperature(510, TemperatureUnit.Celsius)));
            RxT.Add(new UOMProperty(new Temperature(double.NaN)));

            //Feed.Add(new  DistPoint (0, new  Temperature (0, TemperatureUnit.Celsius))); // dummy as reformer arrays have base 1
            Feed.Add(new DistPoint(1, new Temperature(102, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(10, new Temperature(114, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(30, new Temperature(double.NaN, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(50, new Temperature(132, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(70, new Temperature(double.NaN, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(90, new Temperature(160, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(99, new Temperature(178, TemperatureUnit.Celsius)));

            PNAO = Tuple.Create<UOMProperty, UOMProperty, UOMProperty, UOMProperty>(new UOMProperty(60.87), new UOMProperty(28.34), new UOMProperty(10.79), new UOMProperty(0.0));

            Density density = new Density(0.747, DensityUnit.SG);
            MassFlow flow = new MassFlow();
            flow.te_d = 1675.6242;

            NapRef.ReadInput(flow, RxT, FeedComponents, assayBasis, Feed, density, PNAO, (Pressure)Psep.UOM,
                (Temperature)Tsep.UOM, new UOMProperty[] { new UOMProperty(20), new UOMProperty(30), new UOMProperty(50) }.ToList(), RxP.ToList(), RxDp.ToList(), 1.95, 3, 2.2, InputOption.Short);

            NapRef.SolveCase();

            watch.Stop();

            // MessageBox.Show(watch.Elapsed.ToString()+" S");
        }
    }
}