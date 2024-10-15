using ModelEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System.Diagnostics;
using Units;
using Units.UOM;

namespace TestActivityModels
{
    [TestClass]
    public class TestLLEColumn
    {
        private BaseComp sc;

        [TestMethod, STAThread]
        public void LLEColumn()
        {
            Console.WriteLine("Init LLE ColumnTest ++++++++++++++++++++++++++++++");
            Components cc = new();
            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.LeeKesler;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.KMethodLLE = enumEquiKMethod.UNIQUAC;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");

            H2O.MoleFraction = 0.830;
            MEK.MoleFraction = 0.100;
            PropionicAcid.MoleFraction = 0.070;

            cc.Add(H2O);
            cc.Add(MEK);
            cc.Add(PropionicAcid);

            TemperatureC T = 100;
            Pressure P = 1;

            thermo.UniquacParams = new double[3][]
            {   new  double []{1, 10.74651718, -142.301239},
                new  double []{1187, 1, 579.0813599},
                new  double []{570.6130981, -327.8381958, 1}};

            cc.UniquacRSet(new double[] { 0.920000017, 3.247900009, 2.87680006 });
            cc.UniquacQSet(new double[] { 1.399700046, 2.87590003, 2.611900091 });

            var column = new LLESEP("LLETower");
            column.Components.Add(cc);
            column.SolverOptions.InitFlowsMethod = ColumnInitialFlowsMethod.LLE;
            column.SolverOptions.ColumnInitialiseMethod = ColumnInitialiseMethod.LLE;
            column.SolverOptions.VapEnthalpyMethod = ColumnEnthalpyMethod.Rigorous;
            column.SolverOptions.LiqEnthalpyMethod = ColumnEnthalpyMethod.Rigorous;

            column.Thermo.Enthalpy = enumEnthalpy.LeeKesler;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            Port_Material HeavyLiquid = new("HeavyLiquid", thermo);
            HeavyLiquid.P_ = new StreamProperty(ePropID.P, 1, SourceEnum.Input);
            HeavyLiquid.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            HeavyLiquid.AddRange(cc);
            HeavyLiquid.cc.Origin = SourceEnum.Input;
            HeavyLiquid.SetMolarFlows(new double[] { 1, 0, 0 });

            Port_Material LightLiquid = new("LightLiquid", thermo);
            LightLiquid.P_ = new StreamProperty(ePropID.P, 1, SourceEnum.Input);
            LightLiquid.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            LightLiquid.AddRange(cc);
            LightLiquid.SetMolarFlows(new double[] { 0, 9, 0.001 });
            LightLiquid.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[0].feed = HeavyLiquid;
            column.MainTraySection.TopTray.P.BaseValue = 1;
            column.MainTraySection.BottomTray.P.BaseValue = 1;
            column.MainTraySection.CondenserType = CondType.None;
            column.MainTraySection.ReboilerType = ReboilerType.None;
            column.MainTraySection.Trays.Last().feed = LightLiquid;

            HeavyLiquid.Flash();
            LightLiquid.Flash();

            PrintInfo.PrintPortInfo(HeavyLiquid, "HeavyLiquid");
            PrintInfo.PrintPortInfo(LightLiquid, "LightLiquid");

            var watch = Stopwatch.StartNew();

            column.Thermo = thermo;
            bool res = column.Solve();

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());
            Console.WriteLine("Finished LLE Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }
    }
}