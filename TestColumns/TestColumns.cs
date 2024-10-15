using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System.Diagnostics;
using Units;
using Units.PortForm;
using Units.UOM;
using static ModelEngine.PrintInfo;

namespace TestColumns
{
    [TestClass]
    public class TestColumns2
    {
        [TestMethod]
        public void TestA_C3SplitterHeatPumpVariableTrays()
        {
            int TrayNo = 182;
            FlowSheet fs = new();
            Pressure PressA = new(8.77, PressureUnit.BarA);
            Pressure PressB = new Pressure(9.8, PressureUnit.BarA);
            Temperature FreshFeedT = new Temperature(20, TemperatureUnit.Celsius);

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            double[] FeedCompBasic = new double[] { 0.0000, 0.0000, 0.2, 0.8, 0.0000, 0.000, 0.000 };
            //double[] FeedComp = new double[] { 0.0002, 0.0002, 0.1998, 0.7974, 0.0020, 0.0002, 0.0002 };
            double[] FeedComp2 = new double[] { 7.05886230208441e-004,1.03581087490322e-003,5.06320764755300e-003,0.993195095247335,0.0,0.0,0.0};

            var cmpNames = new List<string>() { "Ethane", "ethylene", "Propane", "Propene", "i-Butane", "n-butane", "1-butene" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.Thermo.UseBIPs = false;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(TrayNo);

            int FeedLocation = 131;
            //int FeedLocation = (int)(0.72 * TrayNo);

            /*Port_Material preflux = new();
            //preflux.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 12.71, SourceEnum.Input);
            preflux.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1.0, SourceEnum.Input);
            preflux.P_ = new StreamProperty(ePropID.P, new Pressure(9.013, PressureUnit.BarG), SourceEnum.Input);
            //preflux.T = new StreamProperty(ePropID.T, 15.77, SourceEnum.Input);
            preflux.Q_ = new StreamProperty(ePropID.Q, 0.1234, SourceEnum.Input);
            preflux.cc.Add(fs.ComponentList);
            preflux.cc.SetMolFractions(FeedComp2);
           // column.MainTraySection.Trays.First().feed = preflux;*/

            column.Specs.Add(new Specification("RR", 15, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column, true));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, new Pressure(9.89, PressureUnit.BarA), SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, FreshFeedT, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(FeedCompBasic);
            column.MainTraySection.Trays[FeedLocation].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = PressA;
            column.MainTraySection.BottomTray.P.BaseValue = PressB;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            int requiredspecs = column.RequiredSpecsCount;

            ThermoDynamicOptions thermo = new();
            feed.Flash(thermo: thermo); // Must Flash the feed ports

            var watch = Stopwatch.StartNew();
            bool res = false;
            int count = 1;
            for (int i = 0; i < count; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds / count;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //MessageBox.Show(elapsedMs.ToString());

            //PrintPortInfo(column.MainSectionStages.Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }
    }

    [TestClass]
    public class TestColumns
    {
        [TestMethod]
        public void AA_TestUralsFlowsheetMatchHigherFeed() // to match flowsheet
        {
            FlowSheet fs = new();

            double MolarFeedRate = 10;

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            //Components cc = TestStreams.Crude();
            Components cc = DefaultStreams.CrudeUrals();
            fs.Add(cc);

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, MolarFeedRate, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[8].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.01 * MolarFeedRate, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].TopTray, column));
            //column.Specs.Add(new  Specification("OffGas", new  Temperature (45,TemperatureUnit.Celsius), ePropID.T, eSpecType.Temperature , column[0], column[0].TopTray, column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.TopTray));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1 * MolarFeedRate, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            TraySection KeroStripper = new(3);
            column.TraySections.Add(KeroStripper);
            KeroStripper.TopTray.P.BaseValue = 3;
            KeroStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[2], KeroStripper, KeroStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", KeroStripper, KeroStripper.TopTray, column[0], column[0].Trays[2], false));

            Specification s = new("Kero", 0.1 * MolarFeedRate, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, KeroStripper, KeroStripper.BottomTray, KeroStripper);
            column.Specs.Add(s);

            Port_Material KeroStripFluid = new();
            KeroStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01 * MolarFeedRate, SourceEnum.Input);
            KeroStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            KeroStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            KeroStripFluid.cc.Add(fs.ComponentList);
            KeroStripFluid.cc.ClearMoleFractions();
            KeroStripFluid.cc.ComponentList[0].MoleFraction = 1;
            KeroStripFluid.cc.NormaliseFractions();
            KeroStripFluid.cc.Origin = SourceEnum.Input;
            KeroStripper.BottomTray.feed = KeroStripFluid;

            KeroStripFluid.Flash();

            TraySection GOStripper = new(3);
            column.TraySections.Add(GOStripper);
            GOStripper.TopTray.P.BaseValue = 3;
            GOStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column.MainTraySection, column[0].Trays[4], GOStripper, GOStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", GOStripper, GOStripper.TopTray, column[0], column[0].Trays[3], false));

            Specification GasoilSpec = new("Gasoil", 0.1 * MolarFeedRate, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, GOStripper, GOStripper.BottomTray, GOStripper);
            column.Specs.Add(GasoilSpec);

            Port_Material GOStripFluid = new();
            GOStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01 * MolarFeedRate, SourceEnum.Input);
            GOStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            GOStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            GOStripFluid.cc.Add(fs.ComponentList);
            GOStripFluid.cc.ClearMoleFractions();
            GOStripFluid.cc.ComponentList[0].MoleFraction = 1;
            GOStripFluid.cc.NormaliseFractions();
            GOStripFluid.cc.Origin = SourceEnum.Input;
            GOStripper.BottomTray.feed = GOStripFluid;

            GOStripFluid.Flash();

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.01 * MolarFeedRate, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            PumpAround TopPA = new PumpAround(column.MainTraySection);

            TopPA.drawTray = column.MainSectionStages.Trays[2];
            TopPA.returnTray = column.MainSectionStages.Trays[1];
            TopPA.ReturnTemp = new Temperature();
            TopPA.ReturnTemp.Celsius = 100;
            TopPA.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01 * MolarFeedRate);

            column.PumpArounds.Add(TopPA);
            column.Specs.Add(new Specification("PA Flow", 0.01 * MolarFeedRate, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], TopPA));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.T, eSpecType.PADeltaT, column[0], column[0].Trays[0], TopPA));

            PumpAround LowerPA = new(column.MainTraySection);

            LowerPA.drawTray = column.MainSectionStages.Trays[6];
            LowerPA.returnTray = column.MainSectionStages.Trays[5];
            LowerPA.ReturnTemp = new();
            LowerPA.ReturnTemp.Celsius = 25;
            LowerPA.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01 * MolarFeedRate);

            column.PumpArounds.Add(LowerPA);
            column.Specs.Add(new Specification("PA Flow", 0.1 * MolarFeedRate, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], LowerPA));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.DeltaT, eSpecType.PARetT, column[0], column[0].Trays[0], LowerPA));

            Port_Material BottomStripFluid = new();
            BottomStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01 * MolarFeedRate, SourceEnum.Input);
            BottomStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            BottomStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            BottomStripFluid.cc.Add(fs.ComponentList);
            BottomStripFluid.cc.ClearMoleFractions();
            BottomStripFluid.cc.ComponentList[0].MoleFraction = 1;
            BottomStripFluid.cc.NormaliseFractions();
            BottomStripFluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = BottomStripFluid;

            BottomStripFluid.Flash();

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            // Debug.Print (column.MaintraySection.TopTray.T.ToString());
            // Debug.Print (column.MaintraySection.RefluxRatio.ToString());

            column.FlashAllOutPorts();
            PrintColumnInfo(column);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod, STAThread]
        public void A_TestUralsFlowsheetMatch() // to match flowsheet
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            //Components cc = TestStreams.Crude();
            Components cc = DefaultStreams.CrudeUrals();

            //Port_Material p = PortPropertyForm2.LoadData(@"C:\Users\MarkA\Desktop\Refbits New\CaseFiles\Urals.prp");

            fs.Add(cc);

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[8].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.01, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].TopTray, column));
            //column.Specs.Add(new  Specification("OffGas", new  Temperature (45,TemperatureUnit.Celsius), ePropID.T, eSpecType.Temperature , column[0], column[0].TopTray, column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.TopTray));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            TraySection KeroStripper = new(3);
            column.TraySections.Add(KeroStripper);
            KeroStripper.TopTray.P.BaseValue = 3;
            KeroStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[2], KeroStripper, KeroStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", KeroStripper, KeroStripper.TopTray, column[0], column[0].Trays[1], false));

            Specification s = new("Kero", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, KeroStripper, KeroStripper.BottomTray, KeroStripper);
            column.Specs.Add(s);

            Port_Material KeroStripFluid = new();
            KeroStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            KeroStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            KeroStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            KeroStripFluid.cc.Add(fs.ComponentList);
            KeroStripFluid.cc.ClearMoleFractions();
            KeroStripFluid.cc.ComponentList[0].MoleFraction = 1;
            KeroStripFluid.cc.NormaliseFractions();
            KeroStripFluid.cc.Origin = SourceEnum.Input;
            KeroStripper.BottomTray.feed = KeroStripFluid;

            KeroStripFluid.Flash();

            TraySection GOStripper = new(3);
            column.TraySections.Add(GOStripper);
            GOStripper.TopTray.P.BaseValue = 3;
            GOStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column.MainTraySection, column[0].Trays[4], GOStripper, GOStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", GOStripper, GOStripper.TopTray, column[0], column[0].Trays[3], false));

            Specification GasoilSpec = new("Gasoil", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, GOStripper, GOStripper.BottomTray, GOStripper);
            column.Specs.Add(GasoilSpec);

            Port_Material GOStripFluid = new();
            GOStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            GOStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            GOStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            GOStripFluid.cc.Add(fs.ComponentList);
            GOStripFluid.cc.ClearMoleFractions();
            GOStripFluid.cc.ComponentList[0].MoleFraction = 1;
            GOStripFluid.cc.NormaliseFractions();
            GOStripFluid.cc.Origin = SourceEnum.Input;
            GOStripper.BottomTray.feed = GOStripFluid;

            GOStripFluid.Flash();

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.01, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            PumpAround TopPA = new PumpAround(column.MainTraySection, "TopPA");

            TopPA.drawTray = column.MainSectionStages.Trays[2];
            TopPA.returnTray = column.MainSectionStages.Trays[1];
            TopPA.ReturnTemp = new Temperature();
            TopPA.ReturnTemp.Celsius = 100;
            TopPA.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(TopPA);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], TopPA));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.T, eSpecType.PADeltaT, column[0], column[0].Trays[0], TopPA));

            PumpAround LowerPA = new(column.MainTraySection, "LowerPA");

            LowerPA.drawTray = column.MainSectionStages.Trays[6];
            LowerPA.returnTray = column.MainSectionStages.Trays[5];
            LowerPA.ReturnTemp = new();
            LowerPA.ReturnTemp.Celsius = 25;
            LowerPA.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(LowerPA);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], LowerPA));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.DeltaT, eSpecType.PADeltaT, column[0], column[0].Trays[0], LowerPA));

            Port_Material BottomStripFluid = new();
            BottomStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            BottomStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            BottomStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            BottomStripFluid.cc.Add(fs.ComponentList);
            BottomStripFluid.cc.ClearMoleFractions();
            BottomStripFluid.cc.ComponentList[0].MoleFraction = 1;
            BottomStripFluid.cc.NormaliseFractions();
            BottomStripFluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = BottomStripFluid;

            BottomStripFluid.Flash();

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            column.FlashAllOutPorts();
            PrintColumnInfo(column);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void A_TestUralsFlowsheetMatch2() // to match flowsheet
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            //Components cc = TestStreams.Crude();
            Components cc = DefaultStreams.CrudeUrals();
            fs.Add(cc);

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.01, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            //var KERO = column.LiquidSideStreams.Add(new  SideStream("Kerosine", column.MaintraySection, column.MaintraySection.Trays[2]));
            //column.Specs.Add(new  Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            TraySection KeroStripper = new(3);
            column.TraySections.Add(KeroStripper);
            KeroStripper.TopTray.P.BaseValue = 3;
            KeroStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[3], KeroStripper, KeroStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", KeroStripper, KeroStripper.TopTray, column[0], column[0].Trays[2], false));

            Specification s = new("Kero", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, KeroStripper, KeroStripper.BottomTray, KeroStripper);
            column.Specs.Add(s);

            Port_Material KeroStripFluid = new();
            KeroStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            KeroStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            KeroStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            KeroStripFluid.cc.Add(fs.ComponentList);
            KeroStripFluid.cc.ClearMoleFractions();
            KeroStripFluid.cc.ComponentList[0].MoleFraction = 1;
            KeroStripFluid.cc.NormaliseFractions();
            KeroStripFluid.cc.Origin = SourceEnum.Input;
            KeroStripper.Trays[2].feed = KeroStripFluid;

            KeroStripFluid.Flash();

            //var GO = column.LiquidSideStreams.Add(new  SideStream("GO", column.MaintraySection, column.MaintraySection.Trays[4]));
            //column.Specs.Add(new  Specification("GO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            TraySection GOStripper = new(3);
            column.TraySections.Add(GOStripper);
            GOStripper.TopTray.P.BaseValue = 3;
            GOStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column.MainTraySection, column[0].Trays[5], GOStripper, GOStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", GOStripper, GOStripper.TopTray, column[0], column[0].Trays[4], false));

            Specification GasoilSpec = new("Gasoil", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, GOStripper, GOStripper.BottomTray, GOStripper);
            column.Specs.Add(GasoilSpec);

            Port_Material GOStripFluid = new();
            GOStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            GOStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            GOStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            GOStripFluid.cc.Add(fs.ComponentList);
            GOStripFluid.cc.ClearMoleFractions();
            GOStripFluid.cc.ComponentList[0].MoleFraction = 1;
            GOStripFluid.cc.NormaliseFractions();
            GOStripFluid.cc.Origin = SourceEnum.Input;
            GOStripper.Trays[2].feed = GOStripFluid;

            GOStripFluid.Flash();

            //var HGO = column.LiquidSideStreams.Add(new  SideStream("HGO", column.MaintraySection, column.MaintraySection.Trays[7]));
            //column.Specs.Add(new  Specification("HGO Flow", 0.01, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[7], HGO));

            PumpAround pa = new PumpAround(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[3];
            pa.ReturnTemp = new Temperature();
            pa.ReturnTemp.Celsius = 100;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.T, eSpecType.PADeltaT, column[0], column[0].Trays[0], pa));

            PumpAround pa2 = new(column.MainTraySection);

            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[5];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 25;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.DeltaT, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            /* Port_Material BottomStripFluid = new ();
             BottomStripFluid.MolarFlow = new  StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
             BottomStripFluid.P = new  StreamProperty(ePropID.P, 3, SourceEnum.Input);
             BottomStripFluid.T = new  StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
             BottomStripFluid.Components.AddRange(fs.ComponentList);
             BottomStripFluid.Components.ClearMoleFractions();
             BottomStripFluid.Components.ComponentList[0].MoleFraction = 1;
             BottomStripFluid.Components.NormaliseMoleFractions();
             BottomStripFluid.Components.Origin = SourceEnum.Input;
             column.MaintraySection.BottomTray.feed = BottomStripFluid;

             BottomStripFluid.Flash();*/

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumnWithPAsAndSide2StrippersUrals() // to match flowsheet
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.CrudeUrals();
            fs.Add(cc);

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.01, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            //var KERO = column.LiquidSideStreams.Add(new  SideStream("Kerosine", column.MaintraySection, column.MaintraySection.Trays[2]));
            //column.Specs.Add(new  Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            TraySection KeroStripper = new(3);
            column.TraySections.Add(KeroStripper);
            KeroStripper.TopTray.P.BaseValue = 3;
            KeroStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[3], KeroStripper, KeroStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", KeroStripper, KeroStripper.TopTray, column[0], column[0].Trays[2], false));

            Specification s = new("Kero", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, KeroStripper, KeroStripper.BottomTray, KeroStripper);
            column.Specs.Add(s);

            Port_Material KeroStripFluid = new();
            KeroStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            KeroStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            KeroStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            KeroStripFluid.cc.Add(fs.ComponentList);
            KeroStripFluid.cc.ClearMoleFractions();
            KeroStripFluid.cc.ComponentList[0].MoleFraction = 1;
            KeroStripFluid.cc.NormaliseFractions();
            KeroStripFluid.cc.Origin = SourceEnum.Input;
            KeroStripper.Trays[2].feed = KeroStripFluid;

            KeroStripFluid.Flash();

            //var GO = column.LiquidSideStreams.Add(new  SideStream("GO", column.MaintraySection, column.MaintraySection.Trays[4]));
            //column.Specs.Add(new  Specification("GO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            TraySection GOStripper = new(3);
            column.TraySections.Add(GOStripper);
            GOStripper.TopTray.P.BaseValue = 3;
            GOStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column.MainTraySection, column[0].Trays[5], GOStripper, GOStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", GOStripper, GOStripper.TopTray, column[0], column[0].Trays[4], false));

            Specification GasoilSpec = new("Gasoil", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, GOStripper, GOStripper.BottomTray, GOStripper);
            column.Specs.Add(GasoilSpec);

            Port_Material GOStripFluid = new();
            GOStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            GOStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            GOStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            GOStripFluid.cc.Add(fs.ComponentList);
            GOStripFluid.cc.ClearMoleFractions();
            GOStripFluid.cc.ComponentList[0].MoleFraction = 1;
            GOStripFluid.cc.NormaliseFractions();
            GOStripFluid.cc.Origin = SourceEnum.Input;
            GOStripper.Trays[2].feed = GOStripFluid;

            GOStripFluid.Flash();

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[7]));
            column.Specs.Add(new Specification("HGO Flow", 0.01, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[7], HGO));

            PumpAround pa = new PumpAround(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[3];
            pa.ReturnTemp = new Temperature();
            pa.ReturnTemp.Celsius = 100;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.T, eSpecType.PADeltaT, column[0], column[0].Trays[0], pa));

            PumpAround pa2 = new(column.MainTraySection);

            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[5];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 25;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.DeltaT, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumnWithPAsAndSide2StrippersUralsBottomsStrip() // to match flowsheet
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.CrudeUrals();
            fs.Add(cc);

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.01, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            //var KERO = column.LiquidSideStreams.Add(new  SideStream("Kerosine", column.MaintraySection, column.MaintraySection.Trays[2]));
            //column.Specs.Add(new  Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            TraySection KeroStripper = new(3);
            column.TraySections.Add(KeroStripper);
            KeroStripper.TopTray.P.BaseValue = 3;
            KeroStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[3], KeroStripper, KeroStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", KeroStripper, KeroStripper.TopTray, column[0], column[0].Trays[2], false));

            Specification s = new("Kero", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, KeroStripper, KeroStripper.BottomTray, KeroStripper);
            column.Specs.Add(s);

            Port_Material KeroStripFluid = new();
            KeroStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            KeroStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            KeroStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            KeroStripFluid.cc.Add(fs.ComponentList);
            KeroStripFluid.cc.ClearMoleFractions();
            KeroStripFluid.cc.ComponentList[0].MoleFraction = 1;
            KeroStripFluid.cc.NormaliseFractions();
            KeroStripFluid.cc.Origin = SourceEnum.Input;
            KeroStripper.Trays[2].feed = KeroStripFluid;

            KeroStripFluid.Flash();

            //var GO = column.LiquidSideStreams.Add(new  SideStream("GO", column.MaintraySection, column.MaintraySection.Trays[4]));
            //column.Specs.Add(new  Specification("GO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            TraySection GOStripper = new(3);
            column.TraySections.Add(GOStripper);
            GOStripper.TopTray.P.BaseValue = 3;
            GOStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column.MainTraySection, column[0].Trays[5], GOStripper, GOStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", GOStripper, GOStripper.TopTray, column[0], column[0].Trays[4], false));

            Specification GasoilSpec = new("Gasoil", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, GOStripper, GOStripper.BottomTray, GOStripper);
            column.Specs.Add(GasoilSpec);

            Port_Material GOStripFluid = new();
            GOStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            GOStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            GOStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            GOStripFluid.cc.Add(fs.ComponentList);
            GOStripFluid.cc.ClearMoleFractions();
            GOStripFluid.cc.ComponentList[0].MoleFraction = 1;
            GOStripFluid.cc.NormaliseFractions();
            GOStripFluid.cc.Origin = SourceEnum.Input;
            GOStripper.Trays[2].feed = GOStripFluid;

            GOStripFluid.Flash();

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[7]));
            column.Specs.Add(new Specification("HGO Flow", 0.01, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[7], HGO));

            PumpAround pa = new PumpAround(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[3];
            pa.ReturnTemp = new Temperature();
            pa.ReturnTemp.Celsius = 100;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.T, eSpecType.PADeltaT, column[0], column[0].Trays[0], pa));

            PumpAround pa2 = new(column.MainTraySection);

            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[5];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 25;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA DT", 10, ePropID.DeltaT, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            Port_Material StripFluid = new();
            StripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.0001, SourceEnum.Input);
            StripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            StripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            StripFluid.cc.Add(fs.ComponentList);
            StripFluid.cc.ClearMoleFractions();
            StripFluid.cc.ComponentList[0].MoleFraction = 1;
            StripFluid.cc.NormaliseFractions();
            StripFluid.cc.Origin = SourceEnum.Input;
            column.TraySections[0].Trays.Last().feed = StripFluid;

            GOStripFluid.Flash();

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        public static void TestStripper()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] { 0.00050, 0.00804, 0.01950, 0.01446, 0.01520, 0.02690, 0.02739, 0.04448, 0.01510, 0.03594, 0.02677, 0.03236, 0.02755, 0.02792, 0.02428, 0.02567, 0.01947, 0.01491, 0.01737, 0.02121, 0.01817, 0.01583, 0.02298, 0.01218, 0.01646, 0.01926, 0.01197, 0.01602, 0.01355, 0.01977, 0.00933, 0.01537, 0.01112, 0.01422, 0.01324, 0.01115, 0.00767, 0.01205, 0.00983, 0.01039, 0.01014, 0.00758, 0.01131, 0.01746, 0.01022, 0.01078, 0.00922, 0.00568, 0.00788, 0.00316, 0.00512, 0.00272, 0.00591, 0.00474, 0.00559, 0.00611, 0.00423, 0.00803, 0.00219, 0.00174 };

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            Column column = new();
            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[0].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.None;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;
            stripfluid.Flash();
            var watch = Stopwatch.StartNew();

            column.SolverOptions.VapEnthalpyMethod = ColumnEnthalpyMethod.BostonBrittHdep;
            column.SolverOptions.LiqEnthalpyMethod = ColumnEnthalpyMethod.SimpleLinear;
            column.SolverOptions.ColumnInitialiseMethod = ColumnInitialiseMethod.Test;

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column.TraySections[0].Trays[0].liquidDrawRight, "LiguidDraw");
            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            //Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void ATestAPetyluk()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-Hexane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 3, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Condenser T", 13 + 273.15, ePropID.T, eSpecType.Temperature , column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var ss = column.LiquidSideStreams.Add(new SideStream("Distillate", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 12, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });
            column.MainTraySection.Trays[4].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 12;
            column.MainTraySection.BottomTray.P.BaseValue = 12;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 12;
            stripper.BottomTray.P.BaseValue = 12;

            var SideStripLiqDraw = column.ConnectingDraws.Add(new ConnectingStream("SideStripLiqDraw", column[0], column[0].Trays[2], column[1], column[1].Trays[0], true, 0.1));
            var SideStripVapDraw = column.ConnectingDraws.Add(new ConnectingStream("SideStripVapDraw", column[0], column[0].Trays[7], column[1], column[1].Trays[2], false, 0.01));

            column.Specs.Add(new Specification("Vapour to Side", 0.01, ePropID.MOLEF, eSpecType.VapStream, column[0], column[0].Trays[7], SideStripVapDraw));
            column.Specs.Add(new Specification("Liquid to Side", 0.1, ePropID.MOLEF, eSpecType.LiquidStream, column[0], column[0].Trays[3], SideStripLiqDraw));

            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[2], false, 0.01));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripLiq", column[1], column[1].Trays[2], column[0], column[0].Trays[7], true, 0.1));

            column.Thermo.UseBIPs = false;

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void ATestAPetyluk2()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-Hexane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 3, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Condenser T", 13 + 273.15, ePropID.T, eSpecType.Temperature , column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var ss = column.LiquidSideStreams.Add(new SideStream("Distillate", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 12, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });
            column.MainTraySection.Trays[4].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 12;
            column.MainTraySection.BottomTray.P.BaseValue = 12;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 12;
            stripper.BottomTray.P.BaseValue = 12;

            var SideStripLiqDraw = column.ConnectingDraws.Add(new ConnectingStream("SideStripLiqDraw", column[0], column[0].Trays[2], column[1], column[1].Trays[0], true, 0.1));
            var SideStripVapDraw = column.ConnectingDraws.Add(new ConnectingStream("SideStripVapDraw", column[0], column[0].Trays[7], column[1], column[1].Trays[2], false, 0.01));

            column.Specs.Add(new Specification("Vapour to Side", 0.01, ePropID.MOLEF, eSpecType.VapStream, column[0], column[0].Trays[7], SideStripVapDraw));
            column.Specs.Add(new Specification("Liquid to Side", 0.1, ePropID.MOLEF, eSpecType.LiquidStream, column[0], column[0].Trays[3], SideStripLiqDraw));

            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[2], false, 0.01));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripLiq", column[1], column[1].Trays[2], column[0], column[0].Trays[7], true, 0.1));

            var ss2 = column.LiquidSideStreams.Add(new SideStream("Side Draw", column.TraySections[1], column.TraySections[1].Trays[1]));
            column.Specs.Add(new Specification("Side Flow", 0.01, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[1], column[1].Trays[1], ss2));

            column.Thermo.UseBIPs = false;

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(feed, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void RefluxedAbsorber()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                PseudoComponent pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            var column = new Column("Tower");

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new("Feed");
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[4].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new("Steam");
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void RefluxedAbsorberFlowsheet()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            var column = new Column("Tower");
            fs.Add(column);

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            StreamMaterial feed = new("Feed");
            fs.Add(feed);
            feed.Port.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.Port.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.Port.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.Port.cc.Add(fs.ComponentList);
            feed.Port.cc.Origin = SourceEnum.Input;

            Valve v1 = new("Valve");
            fs.Add(v1);
            v1.PortIn.ConnectPort(feed);

            StreamMaterial Columnfeed = new("TowerFeed");
            fs.Add(Columnfeed);
            Columnfeed.Port.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            v1.PortOut.ConnectPort(Columnfeed);
            column.TraySections[0].Trays[4].feed.ConnectPort(Columnfeed);

            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            StreamMaterial stripfluid = new("Steam");
            fs.Add(stripfluid);
            stripfluid.Port.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.Port.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.Port.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.Port.cc.Add(fs.ComponentList);
            stripfluid.Port.cc.ClearMoleFractions();
            stripfluid.Port.cc[0].MoleFraction = 1;
            stripfluid.Port.cc.NormaliseFractions();
            stripfluid.Port.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed.ConnectPort(stripfluid);
            stripfluid.FlashAllPorts();  // not flashed in the flowsheet for some reason

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = fs.PreSolve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed.Port, "Feed In");
            PrintPortInfo(v1.PortIn, "V1 In");
            PrintPortInfo(v1.PortOut, "V1 Out");
            PrintPortInfo(Columnfeed.Port, "ColumnFeed Out");
            PrintPortInfo(stripfluid.Port, "StripFluid");
            PrintPortInfo(column.MainTraySection.TopTray.liquidDrawRight, "Distillate Out");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Valve()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            StreamMaterial feed = new("Feed");
            fs.Add(feed);
            feed.Port.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.Port.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.Port.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.Port.cc.Add(fs.ComponentList);
            feed.Port.cc.Origin = SourceEnum.Input;

            Valve v1 = new("Valve");
            fs.Add(v1);
            v1.PortIn.ConnectPort(feed);

            StreamMaterial ValveOut = new("TowerFeed");
            fs.Add(ValveOut);
            ValveOut.Port.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            v1.PortOut.ConnectPort(ValveOut);

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = fs.PreSolve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed.Port, "Feed In");
            PrintPortInfo(v1.PortIn, "V1 In");
            PrintPortInfo(v1.PortOut, "V1 Out");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void ValveFlash()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            StreamMaterial feed = new("Feed");
            fs.Add(feed);
            feed.Port.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.Port.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.Port.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.Port.cc.Add(fs.ComponentList);
            feed.Port.cc.Origin = SourceEnum.Input;

            feed.Port.Flash();

            Port_Material pm = new();
            pm.ComponentList = (List<BaseComp>)feed.Port.ComponentList.Clone();
            pm.P = feed.Port.P;
            pm.H = feed.Port.H;
            pm.MolarFlow = feed.Port.MolarFlow;

            pm.P_.origin = SourceEnum.Input;
            pm.H_.origin = SourceEnum.Input;
            pm.MolarFlow_.origin = SourceEnum.Input;
            pm.cc.Origin = SourceEnum.Input;

            pm.Flash();
        }

        [TestMethod]
        public void TestColumn_FixedCondenserT()
        {
            FlowSheet fs = new();
            Pressure Press = 12;

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-C30" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.Thermo.UseBIPs = false;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 1, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Condenser T", 312 - 273.15, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column.Guid));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material p = new();
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            p.cc.Add(fs.ComponentList);
            p.cc.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });
            column.MainTraySection.Trays[4].feed = p;

            column.MainTraySection.TopTray.P.BaseValue = Press;
            column.MainTraySection.BottomTray.P.BaseValue = Press;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            ThermoDynamicOptions thermo = new();
            p.Flash(thermo: thermo); // Must Flash the feed ports
                                     //PrintPortInfo(p, "In1")
                                     //Debug.Print (column.MaintraySection.CondenserType.ToString());

            var watch = Stopwatch.StartNew();
            bool res = false;
            int count = 1;
            for (int i = 0; i < count; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds / count;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //MessageBox.Show(elapsedMs.ToString());

            //PrintPortInfo(column.MainSectionStages.Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestA_C3Splitter()
        {
            FlowSheet fs = new();
            Pressure PressA = new Pressure(8.7, PressureUnit.BarG);
            Pressure PressB = new Pressure(9.8, PressureUnit.BarG);

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "Propene" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.Thermo.UseBIPs = false;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(182);

            column.Specs.Add(new Specification("RR", 15, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 9.8, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 20, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.2, 0.8 });
            column.MainTraySection.Trays[131].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = PressA;
            column.MainTraySection.BottomTray.P.BaseValue = PressB;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            ThermoDynamicOptions thermo = new();
            feed.Flash(thermo: thermo); // Must Flash the feed ports
                                        //PrintPortInfo(p, "In1")
                                        //Debug.Print (column.MaintraySection.CondenserType.ToString());

            var watch = Stopwatch.StartNew();
            bool res = false;
            int count = 1;
            for (int i = 0; i < count; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds / count;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //MessageBox.Show(elapsedMs.ToString());

            //PrintPortInfo(column.MainSectionStages.Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumn_FixedVapourFlow()
        {
            FlowSheet fs = new();
            Pressure Press = 12;

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-C30" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.Thermo.UseBIPs = false;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 1, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Condenser T", 25 + 273.15, ePropID.T, eSpecType.Temperature , column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material p = new();
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            p.cc.Add(fs.ComponentList);
            p.cc.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });
            column.MainTraySection.Trays[4].feed = p;

            column.MainTraySection.TopTray.P.BaseValue = Press;
            column.MainTraySection.BottomTray.P.BaseValue = Press;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            p.Flash(); // Must Flash the feed ports
                       //PrintPortInfo(p, "In1")
                       //Debug.Print (column.MaintraySection.CondenserType.ToString());

            var watch = Stopwatch.StartNew();
            bool res = false;
            int count = 1;
            for (int i = 0; i < count; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds / count;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //MessageBox.Show(elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWith0PATopVapSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            column.Specs.Add(new Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[9].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            /*    PumpAround pa = new  PumpAround(column.MaintraySection, "PA1");
                pa.drawTray = column.MainSectionStages[3];
                pa.return  Tray = column.MainSectionStages[1];
                pa.return  Temp = new  Temperature ();
                pa.return  Temp.Celsius = 25;
                pa.Flow = new  StreamProperty(ePropID.MOLEF, 1);

                column.pumpArounds.Add(pa);
                column.Specs.Add(new  Specification("PA1 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0][0], pa.Guid));
                column.Specs.Add(new  Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0][0], pa.Guid));

                PumpAround pa2 = new  PumpAround(column.MaintraySection, "PA2");
                pa2.drawTray = column.MainSectionStages[6];
                pa2.return  Tray = column.MainSectionStages[4];
                pa2.return  Temp = new  Temperature ();
                pa2.return  Temp.Celsius = 25;
                pa2.Flow = new  StreamProperty(ePropID.MOLEF, 0.1);

                column.pumpArounds.Add(pa2);
                column.Specs.Add(new  Specification("PA2 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0][0], pa2.Guid));
                column.Specs.Add(new  Specification("PA2 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0][0], pa2.Guid));*/

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWith1PATopVapSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            column.Specs.Add(new Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[9].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new PumpAround(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[3];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new Temperature();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA1 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWith1PATopVapSpec100Moles()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            column.Specs.Add(new Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 100, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[9].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new PumpAround(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[3];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new Temperature();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA1 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWith2PATopVapSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            column.Specs.Add(new Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[9].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[3];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA1 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            PumpAround pa2 = new(column.MainTraySection, "PA2");
            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[4];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 25;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.1);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA2 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA2 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWith3PATopVapSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            column.Specs.Add(new Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[9].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[3];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA1 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            PumpAround pa2 = new(column.MainTraySection, "PA2");
            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[4];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 25;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.1);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA2 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA2 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            PumpAround pa3 = new(column.MainTraySection, "PA2");
            pa3.drawTray = column.MainSectionStages.Trays[6];
            pa3.returnTray = column.MainSectionStages.Trays[4];
            pa3.ReturnTemp = new();
            pa3.ReturnTemp.Celsius = 25;
            pa3.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.1);

            column.PumpArounds.Add(pa3);
            column.Specs.Add(new Specification("PA2 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa3));
            column.Specs.Add(new Specification("PA2 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa3));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWithPAandStripperTempSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 3, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Condenser T", 13 + 273.15, ePropID.T, eSpecType.Temperature , column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[4].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 6;
            stripper.BottomTray.P.BaseValue = 6;

            Specification s = new("Stripper Product", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[1], column[1].Trays[2], column[1]);
            column.Specs.Add(s);
            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[6], column[1], column[1].Trays[0], true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[5], false));

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.05, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 100, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.SetMolFractions(new double[] { 1, 0 });
            stripper.Trays[2].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWithPATempSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 1, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Condenser T", 13 + 273.15, ePropID.T, eSpecType.Temperature , column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material p = new();
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            p.cc.Add(fs.ComponentList);
            p.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[4].feed = p;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            p.Flash(); // Must Flash the feed ports
                       // PrintPortInfo(p, "In1")
                       // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            var watch = Stopwatch.StartNew();
            bool res = false;
            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWithPATopVapSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 1, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material p = new();
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            p.cc.Add(fs.ComponentList);
            p.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[4].feed = p;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            p.Flash(); // Must Flash the feed ports
                       // PrintPortInfo(p, "In1")
                       // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestColumnWithStripperTempSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            Column column = new();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 1, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Condenser T", 13 + 273.15, ePropID.T, eSpecType.Temperature , column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.SetMolFractions(new double[] { 0.5, 0.5 });
            column.MainTraySection.Trays[4].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 6;
            stripper.BottomTray.P.BaseValue = 6;

            Specification s = new("Stripper Product", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[1], column[1].Trays[2], column[1]);
            column.Specs.Add(s);
            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[6], column[1], column[1].Trays[0], true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[5], false));

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.05, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 100, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.SetMolFractions(new double[] { 1, 0 });
            stripper.Trays[2].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrude1StripperColumnReboiled()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                PseudoComponent pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc.Thermo = thermo;

            fs.Add(cc);

            Column column = new();
            column.Thermo = thermo;
            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            var ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[4].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 3;
            stripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[6], column[1], column[1].Trays[0], true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[5], false, 0.01));

            column.Specs.Add(new Specification("RR", 3, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));
            column.Specs.Add(new Specification("Stripper Product", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[1], column[1].Trays[2], column[1]));

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            stripper.Trays[2].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column.TraySections[0].Trays[0].liquidDrawRight, "LiguidDraw");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrude1StripperColumnStripped()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc.Thermo = thermo;

            fs.Add(cc);

            Column column = new();
            column.Thermo = thermo;
            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            var ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[6].feed = feed;
            //column.MaintraySection.Trays[7].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 3;
            stripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[6], column[1], column[1].Trays[0], true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[5], false, 0.01));

            //column.Specs.Add(new  Specification("RR", 3, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));
            column.Specs.Add(new Specification("Stripper Product", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[1], column[1].Trays[2], column[1]));

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            stripper.Trays[2].feed = stripfluid;

            stripfluid.Flash();

            Port_Material stripfluid2 = new();
            stripfluid2.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid2.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid2.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid2.cc.Add(fs.ComponentList);
            stripfluid2.cc.ClearMoleFractions();
            stripfluid2.cc.ComponentList[0].MoleFraction = 1;
            stripfluid2.cc.NormaliseFractions();
            stripfluid2.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = stripfluid;

            stripfluid2.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //column.Russel.UpdatePorts(true);
            PrintPortInfo(column.TraySections[0].BottomTray.TrayLiquid, "LiquidDraw");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrude1StripperColumnStrippedWithDistSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.CritVMethod = enumCritVMethod.TWU;
            /*      thermo.CritTMethod = enumCritTMethod.LeeKesler;
                  thermo.CritPMethod = enumCritPMethod.LeeKesler;
                  thermo.CritVMethod = enumCritVMethod.LeeKesler;*/

            thermo.CritZMethod = enumCritZMethod.LeeKesler;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc.Thermo = thermo;

            fs.Add(cc);

            Column column = new();
            column.Thermo = thermo;
            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            var ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[6].feed = feed;
            //column.MaintraySection.Trays[7].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 3;
            stripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[6], column[1], column[1].Trays[0], true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[5], false, 0.01));

            //column.Specs.Add(new  Specification("RR", 3, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Distillate Flow", 0.25, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));
            column.Specs.Add(new Specification("Distillate D95", 150, ePropID.T, eSpecType.DistSpec, column[0], column[0].Trays[0], ss, enumDistType.TBP_VOL, enumDistPoints.D95, true));
            column.Specs.Add(new Specification("Stripper Product", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[1], column[1].Trays[2], column[1]));

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            stripper.Trays[2].feed = stripfluid;

            stripfluid.Flash();

            Port_Material stripfluid2 = new();
            stripfluid2.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid2.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid2.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid2.cc.Add(fs.ComponentList);
            stripfluid2.cc.ClearMoleFractions();
            stripfluid2.cc.ComponentList[0].MoleFraction = 1;
            stripfluid2.cc.NormaliseFractions();
            stripfluid2.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = stripfluid;

            stripfluid2.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            column.Russel.UpdatePorts(true);
            PrintPortInfo(column.TraySections[0].BottomTray.TrayLiquid, "LiquidDraw");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumn1SideDraw()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                PseudoComponent pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));
            //column.Specs.Add(new  Specification("Distillate D95", 100, ePropID.T, eSpecType.DistSpec, column[0], column[0].Trays[0], NAPHTHA.Guid, enumDistType.TBP_VOL, enumDistPoint s.D95));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            Assert.IsTrue(res);
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumn2SideDraws()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumn4SideDraws()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("LGO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            Assert.IsTrue(res);
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumn4SideDraws100Moles()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            Components cc = DefaultStreams.Crude();

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new(ePropID.MOLEF, 100, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 5, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 10, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 10, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("Kerosine Flow", 5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("Kerosine Flow", 5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumn4SideDrawsReducedCrudeFeed()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.ShortResidue2();

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(11);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.6510, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[8].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.01, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Cond T", 45, ePropID.T, eSpecType.Temperature , column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.4 * 0.6510, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[3]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.2 * 0.6510, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[3], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("GO Flow", 0.05 * 0.6510, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.05 * 0.6510, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));
            //column.Specs.Add(new  Specification("Liquid Flow", 0.05 * 0.6510, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[0], column[0].Trays[7], column));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01 * 0.6510, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            column.SolverOptions.ColumnInitialiseMethod = ColumnInitialiseMethod.Test;
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumn4SideDrawsReducedCrudeFeed1MoleFeedRate()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.ShortResidue2();

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(11);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[8].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new  Specification("Cond T", 45, ePropID.T, eSpecType.Temperature , column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.4, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[3]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.2, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[3], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("GO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));
            //column.Specs.Add(new  Specification("Liquid Flow", 0.05, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[0], column[0].Trays[7], column));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            column.SolverOptions.ColumnInitialiseMethod = ColumnInitialiseMethod.Test;
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumn4SideDrawsUralsReduced()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.ReducedUralsCrude();

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumn4SideDrawsUralsReducedNoSteam()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.ReducedUralsCrude();

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("GO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            /*  Port_Material stripfluid = new ();
              stripfluid.MolarFlow = new  StreamProperty(ePropID.MOLEF, SourceEnum.Input, 0.01);
              stripfluid.P = new  StreamProperty(ePropID.P, SourceEnum.Input, 3);
              stripfluid.T = new  StreamProperty(ePropID.T, SourceEnum.Input, 273.15 + 250);
              stripfluid.Components.AddRange(fs.ComponentList);
              stripfluid.Components.ClearMoleFractions();
              stripfluid.Components.ComponentList[0].MoleFraction = 1;
              stripfluid.Components.NormaliseMoleFractions();
              stripfluid.Components.Origin = SourceEnum.Input;
              column.MaintraySection.Trays[12].feed = stripfluid;*/

            //stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumnWithPAs()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("GO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            /*   PumpAround pa = new  PumpAround(column.MaintraySection);

               pa.drawTray = column.MainSectionStages[4];
               pa.return  Tray = column.MainSectionStages[3];
               pa.return  Temp = new  Temperature ();
               pa.return  Temp.Celsius = 100;
               pa.Flow = new  StreamProperty(ePropID.MOLEF, 0.01);

               column.pumpArounds.Add(pa);
               column.Specs.Add(new  Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0][0], pa.Guid));
               column.Specs.Add(new  Specification("PA RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0][0], pa.Guid));*/

            PumpAround pa2 = new(column.MainTraySection);

            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[5];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 25;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumnWithPAsAndSideStrippers()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                //pc = new  PseudoComponent(BPk[i], SG[i], thermo);
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("GO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            PumpAround pa = new PumpAround(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[3];
            pa.ReturnTemp = new Temperature();
            pa.ReturnTemp.Celsius = 100;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            PumpAround pa2 = new(column.MainTraySection);

            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[5];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 25;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.Trays[12].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudeColumnWithPAsAndSide2Strippers()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.Crude();
            //Components cc = TestStreams.CrudeUrals();
            fs.Add(cc);

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(13);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 0.01, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            //var KERO = column.LiquidSideStreams.Add(new  SideStream("Kerosine", column.MaintraySection, column.MaintraySection.Trays[2]));
            //column.Specs.Add(new  Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            TraySection KeroStripper = new(3);
            column.TraySections.Add(KeroStripper);
            KeroStripper.TopTray.P.BaseValue = 3;
            KeroStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[3], KeroStripper, KeroStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", KeroStripper, KeroStripper.TopTray, column[0], column[0].Trays[2], false));

            Specification s = new("Kero", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, KeroStripper, KeroStripper.BottomTray, KeroStripper);
            column.Specs.Add(s);

            Port_Material KeroStripFluid = new();
            KeroStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            KeroStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            KeroStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            KeroStripFluid.cc.Add(fs.ComponentList);
            KeroStripFluid.cc.ClearMoleFractions();
            KeroStripFluid.cc.ComponentList[0].MoleFraction = 1;
            KeroStripFluid.cc.NormaliseFractions();
            KeroStripFluid.cc.Origin = SourceEnum.Input;
            KeroStripper.Trays[2].feed = KeroStripFluid;

            KeroStripFluid.Flash();

            //var GO = column.LiquidSideStreams.Add(new  SideStream("GO", column.MaintraySection, column.MaintraySection.Trays[4]));
            //column.Specs.Add(new  Specification("GO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            TraySection GOStripper = new(3);
            column.TraySections.Add(GOStripper);
            GOStripper.TopTray.P.BaseValue = 3;
            GOStripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column.MainTraySection, column[0].Trays[5], GOStripper, GOStripper.TopTray, true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", GOStripper, GOStripper.TopTray, column[0], column[0].Trays[4], false));

            Specification GasoilSpec = new("Gasoil", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, GOStripper, GOStripper.BottomTray, GOStripper);
            column.Specs.Add(GasoilSpec);

            Port_Material GOStripFluid = new();
            GOStripFluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            GOStripFluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            GOStripFluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            GOStripFluid.cc.Add(fs.ComponentList);
            GOStripFluid.cc.ClearMoleFractions();
            GOStripFluid.cc.ComponentList[0].MoleFraction = 1;
            GOStripFluid.cc.NormaliseFractions();
            GOStripFluid.cc.Origin = SourceEnum.Input;
            GOStripper.Trays[2].feed = GOStripFluid;

            GOStripFluid.Flash();

            //  var HGO = column.LiquidSideStreams.Add(new  SideStream("HGO", column.MaintraySection, column.MaintraySection.Trays[6]));
            //  column.Specs.Add(new  Specification("HGO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            PumpAround pa = new PumpAround(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[3];
            pa.ReturnTemp = new Temperature();
            pa.ReturnTemp.Celsius = 100;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA RetT", 10, ePropID.T, eSpecType.PADeltaT, column[0], column[0].Trays[0], pa));

            /* PumpAround pa2 = new (column.MaintraySection);

             pa2.drawTray = column.MainSectionStages.Trays[6];
             pa2.return  Tray = column.MainSectionStages.Trays[5];
             pa2.return  Temp = new ();
             pa2.return  Temp.Celsius = 25;
             pa2.MoleFlow = new  StreamProperty(ePropID.MOLEF, 0.01);

             column.PumpArounds.Add(pa2);
             column.Specs.Add(new  Specification("PA Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
             column.Specs.Add(new  Specification("PA RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));*/

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            var watch = Stopwatch.StartNew();

            //for (int  i = 0; i < 2; i++)
            bool res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(feed, "In1");
            //PrintPortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudePAandStripperColumn()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] RealFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };

            double[] QuasiFractions = new double[] {0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                cc.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                cc.Add(pc);
            }

            cc.Thermo = thermo;

            fs.Add(cc);

            Column column = new();
            column.Thermo = thermo;
            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 3, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[4].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA DeltaT", 10, ePropID.T, eSpecType.PADeltaT, column[0], column[0].Trays[0], pa));

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 3;
            stripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[6], column[1], column[1].Trays[0], true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[5], false));

            Specification s = new("Stripper Product", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[1], column[1].Trays[2], column[1]);
            column.Specs.Add(s);

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_.BaseValue = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            stripper.Trays[2].feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column.TraySections[0].Trays[0].liquidDrawRight, "LiguidDraw");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudePAandStripperColumnDistSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components cc = new();

            string[] Names = new string[]{"H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane","Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            double[] Fractions = new double[] {0.00002, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 , 0.00050,0.00804,0.01950,0.01446,0.01520,0.02690,0.02739,0.04448,0.01510,0.03594,0.02677,0.03236,0.02755,0.02792,
                0.02428,0.02567,0.01947,0.01491,0.01737,0.02121,0.01817,0.01583,0.02298,0.01218,0.01646,0.01926,0.01197,0.01602,0.01355,0.01977,0.00933,0.01537,0.01112,
                0.01422,0.01324,0.01115,0.00767,0.01205,0.00983,0.01039,0.01014,0.00758,0.01131,0.01746,0.01022,0.01078,0.00922,0.00568,0.00788,0.00316,0.00512,0.00272,0.00591,
                0.00474,0.00559,0.00611,0.00423,0.00803,0.00219,0.00174};

            double[] SG = new double[] {0.99800,0.06990,0.80640,0.79940,1.13770,0.29940,0.38320,0.35570,0.82530,0.78840,0.52100,0.50670,0.56200,0.58320,0.62340,0.62970,
                0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933,
                0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,273.15,311.15,
                318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15,478.15,
                488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{ 647.25, 33.45, 126.15, 132.95, 154.75, 190.65, 282.35, 305.45, 304.15, 373.65, 365.05, 369.85, 408.05,
                 425.15, 460.35, 469.65, 465.55, 474.55, 488.75, 503.75, 518.25, 532.05, 544.95, 556.85, 567.35, 576.75, 587.25, 598.75, 610.55, 622.25, 633.05,
                 642.35, 651.85, 662.45, 673.55, 684.45, 694.45, 702.95, 711.75, 721.25, 731.25, 741.35, 751.55, 761.35, 770.65, 779.05, 786.85, 795.65, 805.15,
                 815.35, 825.65, 835.85, 845.65, 854.65, 864.15, 872.55, 881.15, 889.85, 900.95, 915.55, 930.05, 944.95, 961.15, 978.55, 996.95, 1016.35, 1036.45,
                    1057.25, 1078.45, 1099.85, 1118.95, 1138.25, 1157.55, 1185.85, 1227.65, 1273.85};

            double[] PCrit = new double[] { 221.2, 13.2, 33.9, 35.0, 50.8, 46.4, 50.3, 48.8, 73.7, 90.1, 46.2, 42.6, 36.5, 38.0, 33.3, 33.8, 32.5,
                 32.7, 32.8, 32.6, 32.4, 32.0, 31.5, 30.7, 29.6, 28.2, 27.3, 26.6, 26.1, 25.6, 24.9, 23.9, 23.1, 22.5, 22.1, 21.6, 21.1, 20.2, 19.5, 18.9, 18.4, 18.1,
                 17.7, 17.3, 16.9, 16.3, 15.6, 15.2, 14.9, 14.6, 14.5, 14.3, 14.1, 13.8, 13.5, 13.1, 12.8, 12.5, 12.2, 11.6, 10.7, 10.0, 9.4, 9.0, 8.8, 8.6, 8.5, 8.4, 8.4,
                 8.4, 8.2, 7.9, 7.7, 7.6, 7.3, 6.8};

            double[] VCrit = new double[] {0.0571,0.0515,0.0900,0.0893,0.0732,0.0990,0.1289,0.1480,0.0939,0.0980,0.1810,0.2000,0.2630,0.2550,
                0.3080,0.3110,0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {18.02,2.02,28.01,28.01,32.00,16.04,28.05,30.07,44.01,34.08,42.08,44.10,58.12,58.12,72.15,72.15,
                72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.3440,-0.1201,0.0400,0.0930,0.0190,0.0115,0.0850,0.0986,0.2389,0.0810,0.1480,0.1524,0.1848,0.2010,
                0.2222,0.2539,0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < Names.Length; i++)
            {
                sc = Thermodata.GetComponent(Names[i]);
                if (sc != null)
                {
                    sc.MoleFraction = Fractions[i];
                    cc.Add(sc);
                }
                else
                {
                    pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i], PCrit[i], VCrit[i], thermo);
                    pc.MoleFraction = Fractions[i];
                    cc.Add(pc);
                }
            }

            cc.Thermo = thermo;

            fs.Add(cc);

            Column column = new();
            column.Thermo = thermo;
            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            //column.Specs.Add(new  Specification("RR", 3, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column.Guid));
            column.Specs.Add(new Specification("OffGas", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var ss = column.LiquidSideStreams.Add(new SideStream("Distillate", column.MainTraySection, column.MainTraySection.Trays[0]));
            //column.Specs.Add(new  Specification("Distillate Flow", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss.Guid));
            column.Specs.Add(new Specification("Distillate D95", 195, ePropID.T, eSpecType.DistSpec, column[0], column[0].Trays[0], ss, enumDistType.TBP_VOL, enumDistPoints.D95));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[4].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[4];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 25;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA DeltaT", 10, ePropID.T, eSpecType.PADeltaT, column[0], column[0].Trays[0], pa));

            TraySection stripper = new(3);
            column.TraySections.Add(stripper);
            stripper.TopTray.P.BaseValue = 3;
            stripper.BottomTray.P.BaseValue = 3;

            column.ConnectingDraws.Add(new ConnectingStream("SideStripDraw", column[0], column[0].Trays[6], column[1], column[1].Trays[0], true));
            column.ConnectingNetFlows.Add(new ConnectingStream("SideStripVap", column[1], column[1].Trays[0], column[0], column[0].Trays[5], false, 0.01));

            Specification s = new("Stripper Product", 0.1, ePropID.MOLEF, eSpecType.TrayNetLiqFlow, column[1], column[1].Trays[2], column[1]);
            column.Specs.Add(s);

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            stripper.Trays[2].feed = stripfluid;

            stripfluid.Flash();

            Port_Material Bottomsstripfluid = new();
            Bottomsstripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            Bottomsstripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            Bottomsstripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            Bottomsstripfluid.cc.Add(fs.ComponentList);
            Bottomsstripfluid.cc.ClearMoleFractions();
            Bottomsstripfluid.cc.ComponentList[0].MoleFraction = 1;
            Bottomsstripfluid.cc.NormaliseFractions();
            Bottomsstripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = Bottomsstripfluid;

            Bottomsstripfluid.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");
            //PrintPortInfo(column.TraySections[0].Trays[0].liquidDrawRight, "LiguidDraw");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCrudePreflash()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.Crude();

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 160, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[7].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            //column.Specs.Add(new  Specification("OffGas", 0.03, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Cond T", 12, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));
            //column.Specs.Add(new  Specification("Distillate D95", 100, ePropID.T, eSpecType.DistSpec, column[0], column[0].Trays[0], NAPHTHA.Guid, enumDistType.TBP_VOL, enumDistPoint s.D95));

            //  var KERO = column.LiquidSideStreams.Add(new  SideStream("Kerosine", column.MaintraySection, column.MaintraySection.Trays[2]));
            //  column.Specs.Add(new  Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            feed.Flash(); // Must Flash the feed ports
                          // PrintPortInfo(p, "In1")
                          // Debug.Print (column.MaintraySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0.01, SourceEnum.Input);
            stripfluid.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            stripfluid.T_ = new StreamProperty(ePropID.T, 273.15 + 250, SourceEnum.Input);
            stripfluid.cc.Add(fs.ComponentList);
            stripfluid.cc.ClearMoleFractions();
            stripfluid.cc.ComponentList[0].MoleFraction = 1;
            stripfluid.cc.NormaliseFractions();
            stripfluid.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = stripfluid;

            stripfluid.Flash();

            var watch = Stopwatch.StartNew();

            column.Specs["Distillate Flow"].specvalue = 0.1;
            column.IsReset = true;
            column.Solve();
            watch.Stop();

            PrintPortInfo(column.MainTraySection.Trays[0].Ports["LiquidDrawRight"], "Drawright");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
        }

        [TestMethod]
        public void TestK()
        {
            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            BaseComp sc;
            PseudoComponent pc;
            Components Liq = new();

            string[] RealNames = new string[] { "H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
                "n-Butane","i-Pentane","n-Pentane"};

            string[] QuasiNames = new string[]{"Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
                "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
                "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
                "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
                "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
                "Quasi825*","Quasi875*"};

            //double [] RealFractions = new  double [] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01811, 0.00000, 0.00099, 0.00000, 0.01066, 0.02038, 0.01232, 0.02645, 0.04213 };
            double[] RealFractions = new double[] { 0.98700, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00001 };

            //double [] QuasiFractions = new  double [] { 0.00050, 0.00804, 0.01950, 0.01446, 0.01520, 0.02690, 0.02739, 0.04448, 0.01510, 0.03594, 0.02677, 0.03236, 0.02755, 0.02792, 0.02428, 0.02567, 0.01947, 0.01491, 0.01737, 0.02121, 0.01817, 0.01583, 0.02298, 0.01218, 0.01646, 0.01926, 0.01197, 0.01602, 0.01355, 0.01977, 0.00933, 0.01537, 0.01112, 0.01422, 0.01324, 0.01115, 0.00767, 0.01205, 0.00983, 0.01039, 0.01014, 0.00758, 0.01131, 0.01746, 0.01022, 0.01078, 0.00922, 0.00568, 0.00788, 0.00316, 0.00512, 0.00272, 0.00591, 0.00474, 0.00559, 0.00611, 0.00423, 0.00803, 0.00219, 0.00174 };
            double[] QuasiFractions = new double[] { 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00001, 0.00001, 0.00002, 0.00001, 0.00002, 0.00001, 0.00002, 0.00002, 0.00002, 0.00002, 0.00002, 0.00002, 0.00002, 0.00002, 0.00003, 0.00003, 0.00003, 0.00006, 0.00004, 0.00006, 0.00008, 0.00006, 0.00009, 0.00009, 0.00016, 0.00009, 0.00018, 0.00016, 0.00025, 0.00029, 0.00030, 0.00025, 0.00046, 0.00044, 0.00053, 0.00057, 0.00045, 0.00071, 0.00114, 0.00067, 0.00071, 0.00061, 0.00038, 0.00052, 0.00021, 0.00034, 0.00018, 0.00039, 0.00031, 0.00037, 0.00041, 0.00028, 0.00053, 0.00015, 0.00012 };

            double[] SG = new double[] {0.6150,0.6318,0.6552,0.6772,0.6972,0.7146,0.7286,0.7386,0.7439,0.7454,0.7510,0.7600,0.7705,0.7804,0.7876,0.7900,0.7933
                ,0.8004,0.8092,0.8174,0.8229,0.8242,0.8267,0.8313,0.8374,0.8442,0.8510,0.8570,0.8615,0.8638,0.8648,0.8683,0.8741,0.8812,0.8888,0.8961,0.9023,
                0.9065,0.9122,0.9152,0.9187,0.9224,0.9277,0.9329,0.9342,0.9368,0.9420,0.9496,0.9592,0.9706,0.9832,0.9969,1.0112,1.0255,1.0353,1.0451,1.0549,
                1.0720,1.0965,1.1209};

            double[] BPk = new double[] {311.15,318.15,328.15,338.15,348.15,358.15,368.15,378.15,388.15,398.15,408.15,418.15,428.15,438.15,448.15,458.15,468.15
                ,478.15,488.15,498.15,508.15,518.15,528.15,538.15,548.15,558.15,568.15,578.15,588.15,598.15,608.15,618.15,628.15,638.15,648.15,658.15,668.15,
                678.15,688.15,698.15,708.15,718.15,733.15,753.15,773.15,793.15,813.15,833.15,853.15,873.15,893.15,913.15,933.15,953.15,973.15,993.15,1013.15,
                1048.15,1098.15,1148.15};

            double[] TCrit = new double[]{192.45,201.38,215.56,230.62,245.12,258.91,271.82,283.67,294.23,303.65,314.14,325.60,337.45,349.09,
                359.89,369.19,378.66,389.27,400.38,411.30,421.28,429.78,438.60,448.09,458.05,468.23,478.38,488.23,497.50,505.90,513.69,522.46,532.05,
                542.17,552.52,562.75,572.51,581.46,591.01,599.40,607.99,616.67,627.82,642.37,656.86,671.81,688.00,705.37,723.80,743.17,763.33,784.11,805.33,
                826.67,845.82,865.07,884.43,912.66,954.54,1000.66};

            double[] PCrit = new double[] {32.49,32.70,32.75,32.57,32.36,32.02,31.49,30.69,29.56,28.22,27.25,26.60,26.09,25.58,24.91,23.93,23.06,22.49,22.07,21.64,21.05,20.20,
                19.46,18.89,18.45,18.07,17.71,17.33,16.87,16.28,15.63,15.17,14.86,14.65,14.49,14.32,14.09,13.76,13.53,13.14,12.81,12.49,12.16,11.59,
                10.73,10.00,9.45,9.04,8.77,8.58,8.47,8.42,8.39,8.37,8.16,7.95,7.74,7.61,7.32,6.81};

            double[] VCrit = new double[] {0.3177,0.3230,0.3331,0.3454,0.3574,0.3699,0.3837,0.4001,0.4200,0.4436,0.4645,0.4827,0.4996,0.5168,0.5370,0.5632,
                0.5891,0.6106,0.6297,0.6494,0.6734,0.7051,0.7357,0.7629,0.7876,0.8108,0.8338,0.8585,0.8868,0.9211,0.9601,0.9925,1.0186,1.0403,1.0596,
                1.0793,1.1022,1.1318,1.1560,1.1902,1.2226,1.2541,1.2906,1.3515,1.4435,1.5324,1.6087,1.6719,1.7223,1.7612,1.7906,1.8133,1.8320,1.8509,
                1.8944,1.9377,1.9809,2.0213,2.0925,2.1926};

            double[] MW = new double[] {72.15,75.82,77.91,81.47,86.24,90.25,94.82,99.11,104.52,111.05,116.10,121.80,127.15,132.91,139.17,145.90,152.57,159.99,167.38,174.10,181.49,191.20,199.55,208.22,
                218.47,226.20,235.38,245.42,256.00,266.28,278.11,290.26,300.13,310.04,318.24,326.33,336.54,348.32,360.98,375.47,388.79,402.29,418.45,444.11,476.53,508.59,
                538.64,566.82,593.45,619.04,649.68,671.71,699.43,724.88,757.55,818.75,862.73,922.64,1031.60,1158.92};

            double[] Omega = new double[] {0.2819,0.2667,0.2568,0.2553,0.2575,0.2630,0.2720,0.2845,0.3012,0.3219,0.3386,0.3517,0.3633,0.3757,0.3909,
                0.4111,0.4307,0.4464,0.4604,0.4752,0.4931,0.5158,0.5377,0.5573,0.5756,0.5932,0.6111,0.6301,0.6511,0.6750,0.7010,0.7242,0.7448,0.7639,
                0.7825,0.8018,0.8226,0.8462,0.8681,0.8939,0.9192,0.9445,0.9752,1.0223,1.0842,1.1456,1.2031,1.2566,1.3059,1.3508,1.3916,1.4284,1.4619,
                1.4930,1.5313,1.5670,1.5998,1.6387,1.7098,1.8108 };

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = false;

            for (int i = 0; i < RealNames.Length; i++)
            {
                sc = Thermodata.GetComponent(RealNames[i]);
                sc.MoleFraction = RealFractions[i];
                Liq.Add(sc);
            }

            for (int i = 0; i < QuasiNames.Length; i++)
            {
                pc = new(BPk[i], SG[i], MW[i], Omega[i], TCrit[i] + 273.15, PCrit[i], VCrit[i], thermo);
                pc.MoleFraction = QuasiFractions[i];
                Liq.Add(pc);
            }

            double[] RealFractionsV = new double[] { 0.9999, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000 };
            double[] QuasiFractionsV = new double[] { 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000 };

            Components Vap = Liq.Clone();

            for (int i = 0; i < RealNames.Length; i++)
            {
                Vap[i].MoleFraction = RealFractionsV[i];
            }

            for (int i = RealNames.Length; i < QuasiNames.Length; i++)
            {
                Vap[i].MoleFraction = QuasiFractionsV[i];
            }

            Pressure P = 1;
            Temperature T = 370;

            double[] res = ThermodynamicsClass.KMixArray(Liq, P, T, Liq.MoleFractions, Vap.MoleFractions, out _, thermo);
        }

        [TestMethod]
        public void TestStripperMain()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("WorkerRole1 entry point  called", "Information");

            Thread thread = new(new ThreadStart(TestStripper));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void TestVacColumnWith2PA()
        {
            FlowSheet fs = new();
            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            Components cc = DefaultStreams.Residue();

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(8);

            // column.Specs.Add(new  Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            // column.Specs.Add(new  Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("LVGO", column.MainTraySection, column.MainTraySection.Trays[2]));
            SideStream ss2 = column.LiquidSideStreams.Add(new SideStream("HVGO", column.MainTraySection, column.MainTraySection.Trays[5]));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 0.4, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 400, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);

            column.MainTraySection.Trays[7].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 0.4;
            column.MainTraySection.BottomTray.P.BaseValue = 0.4;
            column.MainTraySection.CondenserType = CondType.None;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.cc.Origin = SourceEnum.Input;
            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[2];
            pa.returnTray = column.MainSectionStages.Trays[0];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 45;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            //column.Specs.Add(new  Specification("PA1 Flow", 0.5, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[2], pa));
            PumpAround pa2 = new(column.MainTraySection, "PA2");
            pa2.drawTray = column.MainSectionStages.Trays[5];
            pa2.returnTray = column.MainSectionStages.Trays[3];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 250;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.1);

            column.PumpArounds.Add(pa2);

            column.Specs.Add(new Specification("LVGO Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], ss));
            column.Specs.Add(new Specification("HVGO Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[5], ss2));
            //column.Specs.Add(new Specification("Top Vap", 90, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            column.Specs.Add(new Specification("PA1 Flow", 1.017, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[5], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[2], pa));

            column.Specs.Add(new Specification("PA2 Flow", 1e-5, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[5], pa2));
            column.Specs.Add(new Specification("PA2 RetT", 273.15 + 250, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[5], pa2));

            var watch = Stopwatch.StartNew();
            bool res = false;

            column.MaxAlphaLoopIterations = 1000;
            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestVacColumnWith2PA_2()
        {
            FlowSheet fs = new();
            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            Components cc = DefaultStreams.ShortResidue3();

            fs.Add(cc);

            Column column = new();

            column.SolverOptions.InitFlowsMethod = ColumnInitialFlowsMethod.Simple;
            column.SolverOptions.AlphaMethod = ColumnAlphaMethod.Rigorous;

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            // column.Specs.Add(new  Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            // column.Specs.Add(new  Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 90, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("LVGO", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("LVGO Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], ss));

            SideStream ss2 = column.LiquidSideStreams.Add(new SideStream("HVGO", column.MainTraySection, column.MainTraySection.Trays[5]));
            column.Specs.Add(new Specification("HVGO Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[5], ss2));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 0.4, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 400, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);

            column.MainTraySection.Trays[7].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 0.4;
            column.MainTraySection.BottomTray.P.BaseValue = 0.4;
            column.MainTraySection.CondenserType = CondType.None;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.cc.Origin = SourceEnum.Input;
            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[2];
            pa.returnTray = column.MainSectionStages.Trays[0];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 45;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            //column.Specs.Add(new  Specification("PA1 Flow", 0.5, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[2], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[2], pa));

            PumpAround pa2 = new(column.MainTraySection, "PA2");
            pa2.drawTray = column.MainSectionStages.Trays[5];
            pa2.returnTray = column.MainSectionStages.Trays[3];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 250;
            pa2.MoleFlow = new(ePropID.MOLEF, 0.1);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA2 Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[5], pa2));
            column.Specs.Add(new Specification("PA2 RetT", 273.15 + 250, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[5], pa2));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestVacColumnWith2PA_2_ReducedFeed()
        {
            FlowSheet fs = new();
            double FeedRate = 0.3287;

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            Components cc = DefaultStreams.ShortResidue3();

            fs.Add(cc);

            var column = new Column();

            column.SolverOptions.InitFlowsMethod = ColumnInitialFlowsMethod.Simple;
            column.SolverOptions.AlphaMethod = ColumnAlphaMethod.Rigorous;

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            // column.Specs.Add(new  Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            // column.Specs.Add(new  Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 90, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("LVGO", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("LVGO Flow", 0.1 * FeedRate, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], ss));

            SideStream ss2 = column.LiquidSideStreams.Add(new SideStream("HVGO", column.MainTraySection, column.MainTraySection.Trays[5]));
            column.Specs.Add(new Specification("HVGO Flow", 0.1 * FeedRate, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[5], ss2));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, FeedRate, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 0.4, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 400, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);

            column.MainTraySection.Trays[7].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 0.4;
            column.MainTraySection.BottomTray.P.BaseValue = 0.4;
            column.MainTraySection.CondenserType = CondType.None;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.cc.Origin = SourceEnum.Input;
            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[2];
            pa.returnTray = column.MainSectionStages.Trays[0];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 45;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1 * FeedRate);

            column.PumpArounds.Add(pa);
            //column.Specs.Add(new  Specification("PA1 Flow", 0.5, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[2], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[2], pa));

            PumpAround pa2 = new(column.MainTraySection, "PA2");
            pa2.drawTray = column.MainSectionStages.Trays[5];
            pa2.returnTray = column.MainSectionStages.Trays[3];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 250;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.1 * FeedRate);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA2 Flow", 0.01 * FeedRate, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[5], pa2));
            column.Specs.Add(new Specification("PA2 RetT", 273.15 + 250, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[5], pa2));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestVacColumnWith2PAMassFlowSpec()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            Components cc = DefaultStreams.Residue2();

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            // column.Specs.Add(new  Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            // column.Specs.Add(new  Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 90, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("LVGO", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("LVGO Flow", 8.69, ePropID.MF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], ss));

            SideStream ss2 = column.LiquidSideStreams.Add(new SideStream("HVGO", column.MainTraySection, column.MainTraySection.Trays[5]));
            column.Specs.Add(new Specification("HVGO Flow", 8.69, ePropID.MF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[5], ss2));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 0.4, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 400, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);

            column.MainTraySection.Trays[7].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 0.4;
            column.MainTraySection.BottomTray.P.BaseValue = 0.4;
            column.MainTraySection.CondenserType = CondType.None;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.cc.Origin = SourceEnum.Input;
            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[2];
            pa.returnTray = column.MainSectionStages.Trays[0];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 45;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa);
            //column.Specs.Add(new  Specification("PA1 Flow", 0.5, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[2], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[2], pa));

            PumpAround pa2 = new(column.MainTraySection, "PA2");
            pa2.drawTray = column.MainSectionStages.Trays[5];
            pa2.returnTray = column.MainSectionStages.Trays[3];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 250;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 0.1);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA2 Flow", 0.869, ePropID.MF, eSpecType.PAFlow, column[0], column[0].Trays[5], pa2));
            column.Specs.Add(new Specification("PA2 RetT", 273.15 + 250, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[5], pa2));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestVacColumnWith2PAx10()
        {
            FlowSheet fs = new();
            Components cc;

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            cc = DefaultStreams.Residue2();

            fs.Add(cc);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            // column.Specs.Add(new  Specification("RR", 6, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            // column.Specs.Add(new  Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Top Vap", 90, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("LVGO", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("LVGO Flow", 1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], ss));

            SideStream ss2 = column.LiquidSideStreams.Add(new SideStream("HVGO", column.MainTraySection, column.MainTraySection.Trays[5]));
            column.Specs.Add(new Specification("HVGO Flow", 1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[5], ss2));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 10, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 0.4, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 400, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);

            column.MainTraySection.Trays[7].feed = feed;

            column.MainTraySection.TopTray.P.BaseValue = 0.4;
            column.MainTraySection.BottomTray.P.BaseValue = 0.4;
            column.MainTraySection.CondenserType = CondType.None;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.cc.Origin = SourceEnum.Input;
            feed.Flash(thermo: column.Thermo); // Must Flash the feed ports
                                               // PrintPortInfo(p, "In1")
                                               // Debug.Print (column.MaintraySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection, "PA1");
            pa.drawTray = column.MainSectionStages.Trays[2];
            pa.returnTray = column.MainSectionStages.Trays[0];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 45;
            pa.MoleFlow = new StreamProperty(ePropID.MOLEF, 10);

            column.PumpArounds.Add(pa);
            //column.Specs.Add(new  Specification("PA1 Flow", 0.5, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[2], pa));
            column.Specs.Add(new Specification("PA1 RetT", 273.15 + 25, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[2], pa));

            PumpAround pa2 = new(column.MainTraySection, "PA2");
            pa2.drawTray = column.MainSectionStages.Trays[5];
            pa2.returnTray = column.MainSectionStages.Trays[3];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 250;
            pa2.MoleFlow = new StreamProperty(ePropID.MOLEF, 1);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA2 Flow", 0.1, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[5], pa2));
            column.Specs.Add(new Specification("PA2 RetT", 273.15 + 250, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[5], pa2));

            var watch = Stopwatch.StartNew();
            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }
    }
}