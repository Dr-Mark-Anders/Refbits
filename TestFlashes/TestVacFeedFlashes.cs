using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Units;
using Units.UOM;
using static ModelEngine.PrintInfo;

namespace TestVacFlashes
{
    [TestClass]
    public class TestVacFeedFlashes
    {
        [TestMethod]
        public void TestVacColumnFeedFlash()
        {
            FlowSheet fs = new();
            Components cc;

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            cc = DefaultStreams.ShortResidue3();

            fs.Add(cc);

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 0.4, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 400, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            var res = feed.Flash();
            PrintPortInfo(feed);

            var H = feed.H_;

            feed.Props.Clear();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 0.4, SourceEnum.Input);
            feed.H_ = H;
            feed.H_.origin = SourceEnum.Input;

            feed.Flash();
            PrintPortInfo(feed);

            ThermodynamicsClass.UpdateThermoProperties(feed.cc, feed.P_.Value, 273.15 + 400, cc.Thermo, enumFluidRegion.Liquid);

            var props = cc.ThermoLiq;
            var propsvap = cc.ThermoVap;

            var watch = Stopwatch.StartNew();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //Print PortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            Assert.IsTrue(res);

            Debug.Print(props.ToString());
            Debug.Print(propsvap.ToString());
        }

        [TestMethod]
        public void TestCDUColumn()
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new();
            BinaryFormatter b = new();
            DrawMaterialStream DrawMaterialStream = null;

            myStream = File.Open(@"C:\Users\MarkA\Desktop\Refbits\CaseFiles\CDU Feed.prp", FileMode.Open);
            DrawMaterialStream = (DrawMaterialStream)b.Deserialize(myStream);
            // Code to write the stream goes here.
            myStream.Close();

            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DrawMaterialStream.Components;

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
            column.Specs.Add(new Specification("LGO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.05, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            feed.Flash(); // Must Flash the feed ports
                          // Print PortInfo(p, "In1")
                          // Debug.Print (column.Maint raySection.CondenserType.ToString())

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
            //Print PortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCDUColumn1000()
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new();
            BinaryFormatter b = new();
            DrawMaterialStream DrawMaterialStream = null;

            myStream = File.Open(@"C:\Users\MarkA\Desktop\Refbits\CaseFiles\CDU Feed.prp", FileMode.Open);
            DrawMaterialStream = (DrawMaterialStream)b.Deserialize(myStream);
            // Code to write the stream goes here.
            myStream.Close();

            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DrawMaterialStream.Components;

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
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1000, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[10].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            column.Specs.Add(new Specification("OffGas", 50, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 100, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 100, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("LGO Flow", 50, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 50, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            feed.Flash(); // Must Flash the feed ports
                          // Print PortInfo(p, "In1")
                          // Debug.Print (column.Maint raySection.CondenserType.ToString())

            Port_Material stripfluid = new();
            stripfluid.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 10, SourceEnum.Input);
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
            //Print PortInfo(column[0].Trays[0].liquidDrawRight, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCDU_2PA_3Mole()
        {
            Stream myStream;
            BinaryFormatter b = new();

            myStream = File.Open(@"C:\Users\MarkA\Desktop\Refbits\CaseFiles\CDU Feed.prp", FileMode.Open);
            DrawMaterialStream DrawMaterialStream = (DrawMaterialStream)b.Deserialize(myStream);
            // Code to write the stream goes here.
            myStream.Close();

            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DrawMaterialStream.Components;

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = true;

            cc.Thermo = thermo;

            fs.Add(cc);

            Column column = new();
            column.Thermo = thermo;
            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(11);

            column.Specs.Add(new Specification("OffGas", 0.01, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("LGO Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 0.1, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 3, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[9].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.Flash(); // Must Flash the feed ports
                          // Print PortInfo(p, "In1")
                          // Debug.Print (column.Maint raySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[2];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 373.15;
            pa.MoleFlow = new(ePropID.MOLEF, 0.01);

            Temperature T1 = 373.15;
            Temperature T2 = 473.15;

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA DeltaT", T1, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            PumpAround pa2 = new(column.MainTraySection);

            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[5];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 473.15;
            pa2.MoleFlow = new(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA2 Flow", 0.01, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA2 DeltaT", T2, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            Port_Material stripfluid2 = new();
            stripfluid2.MF_ = new StreamProperty(ePropID.MF, 1, SourceEnum.Input);
            stripfluid2.P_ = new StreamProperty(ePropID.P, 5, SourceEnum.Input);
            stripfluid2.Q_ = new StreamProperty(ePropID.Q, 1, SourceEnum.Input);
            stripfluid2.cc.Add(fs.ComponentList);
            stripfluid2.cc.ClearMoleFractions();
            stripfluid2.cc.ComponentList[0].MoleFraction = 1;
            stripfluid2.cc.NormaliseFractions();
            stripfluid2.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = stripfluid2;

            stripfluid2.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");

            column.FlashAllOutPorts();
            PrintPortInfo(column.TraySections[0].Trays[0].liquidDrawRight, "LiguidDraw");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCDU_2PA_3000Mole()
        {
            Stream myStream;
            BinaryFormatter b = new();

            //myStream = File.Open(@"C:\Users\MarkA\Desktop\Refbits Files\RefBitsNet6\CaseFiles\CDU Feed.prp", FileMode.Open);
            myStream = File.Open(@"C:\Users\MarkA\Desktop\Refbits\CaseFiles\new CDU Feed.prp", FileMode.Open);
            DrawMaterialStream DrawMaterialStream = (DrawMaterialStream)b.Deserialize(myStream);
            // Code to write the stream goes here.
            myStream.Close();

            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DrawMaterialStream.Components;

            ThermoDynamicOptions thermo = new();

            thermo.Enthalpy = enumEnthalpy.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.MW_Method = enumMW_Method.TWU;
            thermo.UseBIPs = true;

            cc.Thermo = thermo;

            fs.Add(cc);

            Column column = new();
            column.Thermo = thermo;
            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(11);

            //column.Specs.Add(new  Specification("OffGas", 10, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Cond T", 10, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            var NAPHTHA = column.LiquidSideStreams.Add(new SideStream("Naphtha", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Naphtha Flow", 100, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], NAPHTHA));

            var KERO = column.LiquidSideStreams.Add(new SideStream("Kerosine", column.MainTraySection, column.MainTraySection.Trays[2]));
            column.Specs.Add(new Specification("Kerosine Flow", 100, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[2], KERO));

            var GO = column.LiquidSideStreams.Add(new SideStream("GO", column.MainTraySection, column.MainTraySection.Trays[4]));
            column.Specs.Add(new Specification("LGO Flow", 100, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[4], GO));

            var HGO = column.LiquidSideStreams.Add(new SideStream("HGO", column.MainTraySection, column.MainTraySection.Trays[6]));
            column.Specs.Add(new Specification("HGO Flow", 100, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[6], HGO));

            Port_Material feed = new();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 3000, SourceEnum.Input);
            feed.P_ = new StreamProperty(ePropID.P, 3, SourceEnum.Input);
            feed.T_ = new StreamProperty(ePropID.T, 273.15 + 370, SourceEnum.Input);
            feed.cc.Add(fs.ComponentList);
            feed.cc.Origin = SourceEnum.Input;

            column.MainTraySection.Trays[9].feed = feed;
            column.MainTraySection.TopTray.P.BaseValue = 3;
            column.MainTraySection.BottomTray.P.BaseValue = 3;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.None;

            feed.Flash(); // Must Flash the feed ports
                          // Print PortInfo(p, "In1")
                          // Debug.Print (column.Maint raySection.CondenserType.ToString());

            PumpAround pa = new(column.MainTraySection);

            pa.drawTray = column.MainSectionStages.Trays[2];
            pa.returnTray = column.MainSectionStages.Trays[1];
            pa.ReturnTemp = new();
            pa.ReturnTemp.Celsius = 373.15;
            pa.MoleFlow = new(ePropID.MOLEF, 0.01);

            Temperature T1 = 373.15;
            Temperature T2 = 473.15;

            column.PumpArounds.Add(pa);
            column.Specs.Add(new Specification("PA Flow", 10, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa));
            column.Specs.Add(new Specification("PA DeltaT", T1, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa));

            PumpAround pa2 = new(column.MainTraySection);

            pa2.drawTray = column.MainSectionStages.Trays[6];
            pa2.returnTray = column.MainSectionStages.Trays[5];
            pa2.ReturnTemp = new();
            pa2.ReturnTemp.Celsius = 473.15;
            pa2.MoleFlow = new(ePropID.MOLEF, 0.01);

            column.PumpArounds.Add(pa2);
            column.Specs.Add(new Specification("PA2 Flow", 10, ePropID.MOLEF, eSpecType.PAFlow, column[0], column[0].Trays[0], pa2));
            column.Specs.Add(new Specification("PA2 DeltaT", T2, ePropID.T, eSpecType.PARetT, column[0], column[0].Trays[0], pa2));

            Port_Material stripfluid2 = new();
            stripfluid2.MF_ = new StreamProperty(ePropID.MF, 1, SourceEnum.Input);
            stripfluid2.P_ = new StreamProperty(ePropID.P, 5, SourceEnum.Input);
            stripfluid2.Q_ = new StreamProperty(ePropID.Q, 1, SourceEnum.Input);
            stripfluid2.cc.Add(fs.ComponentList);
            stripfluid2.cc.ClearMoleFractions();
            stripfluid2.cc.ComponentList[0].MoleFraction = 1;
            stripfluid2.cc.NormaliseFractions();
            stripfluid2.cc.Origin = SourceEnum.Input;
            column.MainTraySection.BottomTray.feed = stripfluid2;

            stripfluid2.Flash();

            var watch = Stopwatch.StartNew();

            bool res = false;

            for (int i = 0; i < 1; i++)
                res = column.Solve();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            PrintPortInfo(feed, "In1");

            column.FlashAllOutPorts();
            PrintPortInfo(column.TraySections[0].Trays[0].liquidDrawRight, "LiguidDraw");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(res);
        }
    }
}