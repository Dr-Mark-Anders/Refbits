using ModelEngine;
using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using ModelEngine.UnitOperations.FCC;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace TestReactors
{
    [TestClass]
    public class TestReactorClass
    {
       

        public void Opti()
        {
        }

      
        [TestMethod]
        public void TestPlugFlowReactorButaneIsom()
        {
            Reactions reactions = new();
            Components feed = new();

            BaseComp iC4 = Thermodata.GetComponent("i-butane");
            BaseComp nC4 = Thermodata.GetComponent("n-butane");

            feed.Add(nC4);
            feed.Add(iC4);
            feed.SetMolFractions(new double[] { 1, 0 });

            ReactionComponent c1 = new(nC4.Name, 1);
            ReactionComponent c2 = new(iC4.Name, -1);
            ReactionComponents comps = new();

            comps.Add(c1);
            comps.Add(c2);

            Reaction R1 = new(comps, 300, 35, 1870.67, 43.26); //  from reformer.xls

            reactions.Add(R1);

            feed.Add(nC4);
            feed.Add(iC4);

            Port_Material p = new(feed);
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 5, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 150 + 273.15, SourceEnum.Input);
            p.cc = feed;

            Length dim = 0.1;
            Length length = 1;
            Density catden = new Density(950);

            PlugFlowRx Rx = new(reactions, dim, length, catden, 0);
            Rx.PortIn = p;
            Rx.Solve(SolverMethod.Euler);
        }

        [TestMethod]
        public void TestPlugFlowReactorButanePentaneIsom()
        {
            Reactions reactions = new();
            Components feed = new();

            BaseComp nC4 = Thermodata.GetComponent("n-butane");
            BaseComp iC4 = Thermodata.GetComponent("i-butane");
            BaseComp nC5 = Thermodata.GetComponent("n-pentane");
            BaseComp iC5 = Thermodata.GetComponent("i-pentane");

            feed.Add(nC4);
            feed.Add(iC4);
            feed.Add(nC5);
            feed.Add(iC5);
            feed.SetMolFractions(new double[] { 1, 0, 1, 0 });

            reactions.AddReaction(new List<string> { "n-butane", "i-butane" }, new List<int> { 1, -1 }, new Arrhenius(300, 35, 1870.67, 43.26));
            reactions.AddReaction(new List<string> { "n-pentane", "i-pentane" }, new List<int> { 1, -1 }, new Arrhenius(300, 35, 1870.67, 43.26));

            Port_Material p = new(feed);
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 5, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 150 + 273.15, SourceEnum.Input);
            p.cc = feed;

            Length dim = 0.1;
            Length length = 1;
            Density catden = new Density(950);

            PlugFlowRx Rx = new(reactions, dim, length, catden, 0);
            Rx.PortIn = p;
            Rx.Solve(SolverMethod.RK4);
        }

        [TestMethod]
        public void TestShiftPlugFlowReactor()
        {
            Components cc = new();

            BaseComp CO = Thermodata.GetComponent("CO");
            BaseComp CO2 = Thermodata.GetComponent("CO2");
            BaseComp H2 = Thermodata.GetComponent("Hydrogen");
            BaseComp H2O = Thermodata.GetComponent("H2O");

            ReactionComponent c1 = new(CO.Name, 1);
            ReactionComponent c2 = new(CO2.Name, 1);
            ReactionComponent c3 = new(H2.Name, -1);
            ReactionComponent c4 = new(H2O.Name, -1);

            ReactionComponents comps = new();

            comps.Add(c1);
            comps.Add(c2);
            comps.Add(c3);
            comps.Add(c4);

            Reaction R1 = new(comps, Math.Exp(11.5), 118, Math.Exp(11.5), 118);
            Reactions reactions = new();
            reactions.Add(R1);

            cc.Add(CO2);
            cc.Add(H2);
            cc.Add(H2O);
            cc.Add(CO);

            Port_Material p = new(cc);
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 1, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 1000, SourceEnum.Input);

            PlugFlowRx Rx = new(reactions, 0.1, 1, 950, 0);
            Rx.PortIn = p;
            Rx.Solve();
        }

        [TestMethod]
        public void TestTatorayReactor()
        {
            Components cc = new();

            List<string> Names = new(){"HYDROGEN","METHANE","ETHANE","PROPANE","I-BUTANE","N-BUTANE",
                "I-PENTANE","N-PENTANE","CYCLOPENTANE","N-HEXANE","22-Mbutane","CYCLOHEXANE","Mcyclopentan",
                "BENZENE","N-HEPTANE","3-Epentane","MCYCLOHEXANE","11Mcycpentan","Ecycheptane","TOLUENE",
                "N-OCTANE","3-Mheptane","11-Di-E-CC6","113-MCC5","1M-TR3-ECC5","I-PCycHexane","Ecyclohexane",
                "E-Benzene","P-XYLENE","M-XYLENE","O-XYLENE","N-NONANE","I-PCycHexane","1-TR2-ECC5",
                "CUMENE","N-Pbenzene","M-Xylene","P-Xylene","O-Xylene","135-Mbenzene","124-Mbenzene",
                "123-Mbenzene","INDANE","N-DECANE","n-Bcychexane","N-Bbenzene","I-Bbenzene","Sec-Bbenzene",
                "tert-Bbenzen","1M2nPropylBZ","1M3nPropylBZ","1M4nPropylBZ","O-CYMENE","M-CYMENE","P-CYMENE",
                "12-E-BZ","13-E-BZ","14-EBenzene","3EoXylene","4EoXylene","2-ETHYL-M-XYLENE","4EmXylene",
                "5EmXylene","2EpXylene","1234-T-M-CC6","1234-T-M-CC6","1234-T-M-CC6","NAPHTHALENE","1-E-2-i-P-BZ",};

            for (int i = 0; i < Names.Count; i++)
            {
                BaseComp comp = Thermodata.GetComponent(Names[i]);
                if (comp != null)
                {
                    Debug.Print(Names[i] + ":" + comp.Name);
                    cc.Add(comp);
                }
                else
                    Debug.Print(Names[i]);
            }

            ReactionComponents comps = new();

            Reactions reactions = new();
            reactions.LoadFromFile("C:\\Users\\MarkA\\Desktop\\Refbits Files\\RefBitsNet6\\ModelEngine\\UnitOperations\\Reactions\\Tatoray.txt");

            ReactionComponent c1 = new("Toluene", 2);
            ReactionComponent c2 = new("P-Xylene", -1);
            ReactionComponent c3 = new("Benzene", -1);

            comps.Add(c1);
            comps.Add(c2);
            comps.Add(c3);
            Reaction rxn = new(comps, 7.15E-01, 27.7, 4.15E-02, 29);

            reactions.Add(rxn);

            Port_Material p = new(cc);
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 1, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 1000, SourceEnum.Input);

            reactions.Add(rxn);

            PlugFlowRx Rx = new(reactions, 0.1, 1, 950, 0);
            Rx.PortIn = p;
            Rx.Solve();
        }

        [TestMethod]
        public void TestSpeed()
        {
            CalibrationFactors calibFactors = new CalibrationFactors();

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

            Stopwatch sw = Stopwatch.StartNew();

            sw.Start();
            double res = 0;
            for (int i = 0; i < 10000; i++)
            {
                res += feed.IndexOf("Toluene");
            }
            sw.Stop();

            System.Windows.Forms.MessageBox.Show(sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                res += feed.FastIndex("C9A");
            }

            sw.Stop();

            System.Windows.Forms.MessageBox.Show(sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                res += feed.IndexOf("Toluene");
            }

            sw.Stop();

            System.Windows.Forms.MessageBox.Show(sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                res += feed["Toluene"].MoleFraction;
            }

            sw.Stop();

            System.Windows.Forms.MessageBox.Show(sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                res += feed[feed.FastIndex("C9A")].MoleFraction;
            }

            sw.Stop();

            System.Windows.Forms.MessageBox.Show(sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            sw.Start();

            double[] fracs = feed.MoleFractions;

            for (int i = 0; i < 10000; i++)
            {
                res += fracs[feed.FastIndex("C9A")];
            }

            sw.Stop();

            System.Windows.Forms.MessageBox.Show(sw.Elapsed.TotalMilliseconds.ToString());
        }
    }
}