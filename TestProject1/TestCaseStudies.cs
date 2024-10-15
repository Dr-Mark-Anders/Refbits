using ModelEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using Units.UOM;

namespace UnitTests
{
    [TestClass]
    public class TestcaseStudies
    {
        [TestMethod]
        public void Test()
        {
            ThermoDynamicOptions thermo = new();
            thermo.KMethod = enumEquiKMethod.PR78;

            Components cc = new();
            BaseComp sc;

            sc = Thermodata.GetComponent("n-Butane");
            cc.Add(sc);

            cc[0].MoleFraction = 1;

            StreamMaterial streamMaterial = new();
            streamMaterial.AddComponents(cc);
            streamMaterial.Port.cc.Origin = SourceEnum.Input;
            streamMaterial.Port.MF_.BaseValue = 1;
            streamMaterial.Port.MF_.origin = SourceEnum.Input;
            streamMaterial.Port.T= new Temperature(273.15);
            streamMaterial.Port.P = new Pressure(1);
            streamMaterial.Port.T_.origin = SourceEnum.Input;
            streamMaterial.Port.P_.origin = SourceEnum.Input;

            FlowSheet flowSheet = new();
            flowSheet.Add(streamMaterial);

            CaseStudy casestudy = new();
            casestudy.AddOutput(streamMaterial.Port.MolarFlow_);
            casestudy.AddOutput(streamMaterial.Port.T_);

            CaseSet caseset = new();
            caseset.AddValue(streamMaterial.Port.MF_, 100);

            casestudy.AddSet(caseset);

            caseset = new CaseSet();
            caseset.AddValue(streamMaterial.Port.MF_, 200);
            caseset.AddValue(streamMaterial.Port.T_, 100);

            casestudy.AddSet(caseset);

            casestudy.Runcases(flowSheet);

            caseResults res = casestudy.CaseResults;
        }
    }
}