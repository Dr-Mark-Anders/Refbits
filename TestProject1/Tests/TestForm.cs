using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Units;
using Units.PortForm;
using static gv;

namespace ModelEngine
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private readonly text1 test = new();

        private void button1_Click(object sender, EventArgs e)
        {
            text1.TestFlowSheet1();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            text1.TestSimpleFlash();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            test.TestSimpleFlash2LPhase();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            test.TestMixAndFlash();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            test.TestLiqLiqEx();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            test.TestHeater();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            test.TestHeatEx();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            test.TestFlowsh1();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            test.TestFlowsh2();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            test.TestRecycle();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            test.TestRecycle2();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            test.TestMoleBalance();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            test.TestButaneFlash();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            test.TestSingCompFlash();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            test.TestMultCompFlash();
        }

        private void TestNodeForm_Click(object sender, EventArgs e)
        {
            test.TestNodeForm();
        }

        private void TestSerialisation_Click(object sender, EventArgs e)
        {
            test.TestFlowSheetSerialisation();
        }

        private void TestColumn_Click(object sender, EventArgs e)
        {
            text1.TestColumn();
        }
    }

    public class text1
    {
        private readonly FlowSheet fs = new();

        public static void TestColumn()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            foreach (var i in new List<string>() { "Propane", "n-butane", "n-C30" })
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 1, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Condenser T", 11 + 273.15, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material p = new();
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            p.cc.Add(fs.ComponentList);
            p.cc.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });
            column.MainTraySection.Trays[4].feed = p;

            column.MainTraySection.TopTray.P.BaseValue = 6;
            column.MainTraySection.BottomTray.P.BaseValue = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            p.Flash(); // Must Flash the feed ports

            var watch = Stopwatch.StartNew();
            int count = 10;
            for (int i = 0; i < count; i++)
                column.Solve();

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds / count;
            Debug.WriteLine("Column Solution Time ms: " + elapsedMs.ToString());

            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            //Assert.IsTrue(res);
        }

        public void TestFlowSheetSerialisation()
        {
            fs.ModelStack.Clear();

            double[] comp;
            Debug.WriteLine(@"Init Flowsh1 ++++++++++++++++++++++++++++++");
            //#Set Thermo
            //var pkgName = "RK";
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
            {
                fs.AddComponent(i);
            }
            //Create unit Operations
            var stream = new StreamMaterial();
            var stream2 = new StreamMaterial();
            var mixer = new Mixer();
            var flash = new SimpleFlash();
            //Add all the units to a FlowSheet and connect them
            //var flsheet = new  FlowSheet();
            fs.Add(flash, "myFlash1");
            fs.Add(stream, "myStream1");
            fs.Add(stream2, "myStream2");
            fs.Add(mixer, "myMixer");
            //flsheet.SetParameterValue(NULIQPH_PAR, 1);
            //flsheet.SetThermoAdmin(thAdmin);
            //flsheet.SetThermo(thermo);
            //SetStream
            var portsIn = stream.GetPorts(FlowDirection.IN);
            //I know in advance there's only one port in
            stream.SetCompositionValue(portsIn["In1"], "PROPANE", 0.5, SourceEnum.Input);
            stream.SetCompositionValue(portsIn["In1"], "n-BUTANE", 0.5, SourceEnum.Input);
            stream.SetCompositionValue(portsIn["In1"], "i-BUTANE", 0.0, SourceEnum.Input);
            stream.SetCompositionValue(portsIn["In1"], "n-PENTANE", 0.0, SourceEnum.Input);
            stream.Ports["In1"].SetPortValue(ePropID.T, 273.15, SourceEnum.Input);
            stream.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.P, 1, SourceEnum.Input);
            stream.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            //stream.Solve(); // --- Could be done, but not yet
            //Set second Stream
            portsIn = stream2.GetPorts(FlowDirection.IN);
            //I know in advance there's only one port in
            stream2.SetCompositionValue(portsIn["In1"], "PROPANE", 0.0, SourceEnum.Input);
            stream2.SetCompositionValue(portsIn["In1"], "n-BUTANE", 0.0, SourceEnum.Input);
            stream2.SetCompositionValue(portsIn["In1"], "i-BUTANE", 0.5, SourceEnum.Input);
            stream2.SetCompositionValue(portsIn["In1"], "n-PENTANE", 0.5, SourceEnum.Input);
            stream2.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.T, 200.15, SourceEnum.Input);
            stream2.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.P, 2.0, SourceEnum.Input);
            stream2.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            //stream.Solve() --- Could be done, but not yet
            //Set a mixer
            //mixer.SetParameterValue(NUSTIN_PAR, 2);
            //Set Flash UO
            //I already know the names of the ports
            string uOpNameOut = "myStream1";
            string portNameOut;
            string uOpNameIn = "myMixer";
            string portNameIn;
            // Debug.WriteLine("conn " + fs.ConnectPorts(uOpNameOut, "Out1", uOpNameIn, "In1"));
            portNameIn = "In2";
            //  Debug.WriteLine("conn2 " + FlowSheet.ConnectPorts(stream2, "Out1", mixer, portNameIn));
            portNameOut = "Out1";
            portNameIn = "In1";
            //   Debug.WriteLine("conn3 " + FlowSheet.ConnectPorts(mixer, portNameOut, flash, portNameIn));
            fs.PreSolve();
            portsIn = flash.GetPorts(FlowDirection.IN);
            var portsOut = flash.GetPorts(FlowDirection.OUT);
            //Print  some info in

            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("");

            foreach (var i in stream.Ports)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of port in \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of mixer port in \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }

            foreach (var i in stream2.Ports)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of port in \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of mixer port in \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }

            foreach (var i in mixer.Ports)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of port in \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of mixer port in \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }

            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of port in \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of flash port in \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of flash port out \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of flash port out \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }
            Debug.WriteLine(@"Finished Flowsh1 ++++++++++++++++++++++++++++++");

            string FileName = "C:\\Users\\Mark\\Desktop\\new  folder\\Standalone.TMP";
            //string FileName = "C:\\Users\\Mark\\Desktop\\new  folder\\Combined.TMP";

            File.Delete(FileName);
            FileStream SaveFileStream = File.Create(FileName);
            BinaryFormatter serializer = new();
            serializer.Serialize(SaveFileStream, fs);
            SaveFileStream.Close();
            FlowSheet fs2 = new();
            if (File.Exists(FileName))
            {
                Console.WriteLine("Reading saved file");
                FileStream openFileStream = File.OpenRead(FileName);
                BinaryFormatter deserializer = new();
                fs2 = (FlowSheet)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
            }

            fs.ModelStack.Clear();
            fs2.Clear();
        }

        public void TestNodeForm()
        {
            //FlowSheet flsheet = new  FlowSheet();

            Console.WriteLine("Init SimpleFlash ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "Propane", "n-BUTANE" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //#Load vals

            var flash = new SimpleFlash();

            var cmps = flash.Flowsheet.GetCompoundNames();
            flash.SetParameterValue(NULIQPH_PAR, 1);
            Debug.WriteLine("Cmps: ", cmps);
            List<string> portsIn = flash.GetPortNames(FlowDirection.IN);
            Debug.WriteLine("Names Ports In: ", portsIn);
            List<string> portsOut = flash.GetPortNames(FlowDirection.OUT);
            Debug.WriteLine("Names Ports Out: ", portsOut);

            var comps = new List<double> { 0.5, 0.5 };
            flash.SetCompositionValues(portsIn[0], comps, SourceEnum.Input);

            flash.GetPort(portsIn[0]).SetPortValue(ePropID.T, 273.15 + 25, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            Debug.WriteLine("return   value from Solve()", flash.Solve());
            Debug.WriteLine("");

            Port_Material p = flash.Ports["In1"];

            PortPropertyForm2 F = new(p, new UOMDisplayList());
            F.ShowDialog();

            p.Properties.Clear();
            p.Properties[ePropID.P] = new(ePropID.P, 1);
            p.Properties[ePropID.T] = new(ePropID.T, 273.15);
            p.Flash();
            PrintPortInfo(p, "P&T");

            Console.WriteLine("Finished NodeForm Test ++++++++++++++++++++++++++++++");

            flash.CleanUp();
        }

        public void TestSingCompFlash()
        {
            //FlowSheet flsheet = new  FlowSheet();
            Console.WriteLine("Init SimpleFlash ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "n-BUTANE" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //#Load vals

            var flash = new SimpleFlash();

            var cmps = flash.Flowsheet.GetCompoundNames();
            flash.SetParameterValue(NULIQPH_PAR, 1);
            Debug.WriteLine("Cmps: ", cmps);
            List<string> portsIn = flash.GetPortNames(FlowDirection.IN);
            Debug.WriteLine("Names Ports In: ", portsIn);
            List<string> portsOut = flash.GetPortNames(FlowDirection.OUT);
            Debug.WriteLine("Names Ports Out: ", portsOut);

            var comps = new List<double> { 1 };
            flash.SetCompositionValues(portsIn[0], comps, SourceEnum.Input);

            flash.GetPort(portsIn[0]).SetPortValue(ePropID.T, 273.15 + 25, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            Debug.WriteLine("return   value from Solve()", flash.Solve());
            Debug.WriteLine("");

            Port_Material p = flash.Ports["In1"];

            p.Properties.Clear();
            p.Properties[ePropID.P] = new StreamProperty(ePropID.P, 1);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15);
            p.Flash();
            PrintPortInfo(p, "P&T");

            double Enth = p.Properties[ePropID.H].Value;

            p.Properties.Clear();
            p.Properties[ePropID.Q] = new StreamProperty(ePropID.Q, 1);
            p.Properties[ePropID.P] = new StreamProperty(ePropID.P, 1);
            p.Flash();
            PrintPortInfo(p, "Q&P");

            p.Properties.Clear();
            p.Properties[ePropID.Q] = new StreamProperty(ePropID.Q, 1);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15);
            p.Flash();
            PrintPortInfo(p, "Q&T");

            p.Properties.Clear();  // Not Complete
            p.Properties[ePropID.H] = new StreamProperty(ePropID.H, Enth);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15);
            p.Flash();
            PrintPortInfo(p, "H&T");

            Console.WriteLine("Finished Single Comp Flash ++++++++++++++++++++++++++++++");

            flash.CleanUp();
        }

        public void TestMultCompFlash()
        {
            //FlowSheet flsheet = new  FlowSheet();
            Console.WriteLine("Init SimpleFlash ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "Propane", "n-BUTANE" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //#Load vals

            var flash = new SimpleFlash();

            var cmps = flash.Flowsheet.GetCompoundNames();
            flash.SetParameterValue(NULIQPH_PAR, 1);
            Debug.WriteLine("Cmps: ", cmps);
            List<string> portsIn = flash.GetPortNames(FlowDirection.IN);
            Debug.WriteLine("Names Ports In: ", portsIn);
            List<string> portsOut = flash.GetPortNames(FlowDirection.OUT);
            Debug.WriteLine("Names Ports Out: ", portsOut);

            var comps = new List<double> { 0.5, 0.5 };
            flash.SetCompositionValues(portsIn[0], comps, SourceEnum.Input);

            flash.GetPort(portsIn[0]).SetPortValue(ePropID.T, 273.15 + 25, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            Debug.WriteLine("return   value from Solve()", flash.Solve());
            Debug.WriteLine("");

            Port_Material p = flash.Ports["In1"];

            p.Properties.Clear();
            p.Properties[ePropID.P] = new StreamProperty(ePropID.P, 1);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15);
            p.Flash();
            PrintPortInfo(p, "P&T");

            double Enth = p.Properties[ePropID.H].Value;

            p.Properties.Clear();
            p.Properties[ePropID.Q] = new StreamProperty(ePropID.Q, 1);
            p.Properties[ePropID.P] = new StreamProperty(ePropID.P, 1);
            p.Flash();
            PrintPortInfo(p, "Q&P");

            p.Properties.Clear();
            p.Properties[ePropID.Q] = new StreamProperty(ePropID.Q, 1);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15);
            p.Flash();
            PrintPortInfo(p, "Q&T");

            p.Properties.Clear();  // Not Complete
            p.Properties[ePropID.H] = new StreamProperty(ePropID.H, Enth);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15);
            p.Flash();
            PrintPortInfo(p, "H&T");

            Console.WriteLine("Finished Multi Comp Flash ++++++++++++++++++++++++++++++");

            flash.CleanUp();
        }

        public void TestButaneFlash()
        {
            //FlowSheet flsheet = new  FlowSheet();
            Port_Material port;
            double[] comp;
            Console.WriteLine("Init SimpleFlash ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "n-BUTANE" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //#Load vals

            var flash = new SimpleFlash();

            var cmps = flash.Flowsheet.GetCompoundNames();
            flash.SetParameterValue(NULIQPH_PAR, 1);
            Debug.WriteLine("Cmps: ", cmps);
            List<string> portsIn = flash.GetPortNames(FlowDirection.IN);
            Debug.WriteLine("Names Ports In: ", portsIn);
            List<string> portsOut = flash.GetPortNames(FlowDirection.OUT);
            Debug.WriteLine("Names Ports Out: ", portsOut);

            var comps = new List<double> { 1 };
            flash.SetCompositionValues(portsIn[0], comps, SourceEnum.Input);

            flash.GetPort(portsIn[0]).SetPortValue(ePropID.T, 273.15 + 25, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            Debug.WriteLine("return   value from Solve()", flash.Solve());
            Debug.WriteLine("");

            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port in \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port in \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
                port = flash.GetPort(i);
                Console.WriteLine($"Array of ");
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port out \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j].ToString());

                Console.WriteLine("");
                Console.WriteLine("Some props of port out \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
                port = flash.GetPort(i);
                Console.WriteLine("Array of " + ePropID.FUG + " of port out \"" + i + "\":");
                Console.WriteLine("");
            }
            Console.WriteLine("Finished SimpleFlash ++++++++++++++++++++++++++++++");

            Port_Material p = flash.Ports["In1"];

            p.Properties.Clear();
            p.Properties[ePropID.P] = new StreamProperty(ePropID.P, 1, SourceEnum.Input);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15, SourceEnum.Input);

            p.cc.Thermo.KMethod = enumEquiKMethod.GraysonStreed;
            p.Flash();
            p.cc.Thermo.KMethod = enumEquiKMethod.PR78;
            p.Flash();
            PrintPortInfo(p, "P&T PR78");
            p.cc.Thermo.KMethod = enumEquiKMethod.PR76;
            p.Flash();
            PrintPortInfo(p, "P&T PR76");

            PortPropertyForm2 ppf = new(p, new UOMDisplayList());
            ppf.ShowDialog();

            double Enth = p.Properties[ePropID.H].Value;

            p.Properties.Clear();
            p.Properties[ePropID.H] = new StreamProperty(ePropID.H, Enth, SourceEnum.Input);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15, SourceEnum.Input);
            p.Flash();
            PrintPortInfo(p, "H&T");

            p.Properties.Clear();
            p.Properties[ePropID.Q] = new StreamProperty(ePropID.Q, 1, SourceEnum.Input);
            p.Properties[ePropID.P] = new StreamProperty(ePropID.P, 1, SourceEnum.Input);
            p.Flash();
            PrintPortInfo(p, "Q&P");

            p.Properties.Clear();
            p.Properties[ePropID.Q] = new StreamProperty(ePropID.Q, 1, SourceEnum.Input);
            p.Properties[ePropID.T] = new StreamProperty(ePropID.T, 273.15, SourceEnum.Input);
            p.Flash();
            PrintPortInfo(p, "Q&T");

            ppf = new PortPropertyForm2(p, new UOMDisplayList());
            ppf.Show();

            flash.CleanUp();
        }

        public static void PrintPortInfo(Port_Material p, string Comment = "")
        {
            //Print  some info out
            var comp = p.cc;
            Debug.WriteLine("Composition of port \"" + p.Name + "\":");
            for (int j = 0; j < comp.Count; j++)
                Console.WriteLine("fraction of " + comp[j].Name + ": " + comp[j].MoleFraction.ToString());

            Debug.WriteLine("");
            Debug.WriteLine("Some props of port \"" + p.Name + "\":" + Comment);
            Debug.WriteLine(ePropID.T + ": " + p.Properties[ePropID.T]);
            Debug.WriteLine(ePropID.P + ": " + p.Properties[ePropID.P]);
            Debug.WriteLine(ePropID.H + ": " + p.Properties[ePropID.H]);
            Debug.WriteLine(ePropID.S + ": " + p.Properties[ePropID.S]);
            Debug.WriteLine(ePropID.Q + ": " + p.Properties[ePropID.Q]);
            Debug.WriteLine(ePropID.MOLEF + ": " + p.Properties[ePropID.MOLEF]);
            Debug.WriteLine("");
            //port = flash.GetPort(i);
            //Console.WriteLine("Array properties available: " + port.GetArrPropNames());
            //props = port.GetArrPropValue(ePropID.FUG);
            //Console.WriteLine("Array of " + ePropID.FUG + " of port out \"" + i + "\":");
            //if (props != null)
            //    foreach (var j in Enumerable.Range(0, props.Count()))
            //       Console.WriteLine("ln fug of " + cmpNames[j] + ": " + props[j]);

            Debug.WriteLine("");
        }

        public static void TestFlowSheet1()
        {
            FlowSheet flsheet = new();

            double[] comp;
            Debug.WriteLine(@"Init Flowsh1 ++++++++++++++++++++++++++++++");
            //#Set Thermo
            //var pkgName = "RK";
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
            {
                flsheet.AddComponent(i);
            }
            //Create unit Operations
            var stream = new StreamMaterial();
            var stream2 = new StreamMaterial();
            var mixer = new Mixer();
            var flash = new SimpleFlash();
            //Add all the units to a FlowSheet and connect them
            //var flsheet = new  FlowSheet();
            flsheet.Add(flash, "myFlash1");
            flsheet.Add(stream, "myStream1");
            flsheet.Add(stream2, "myStream2");
            flsheet.Add(mixer, "myMixer");
            //flsheet.SetParameterValue(NULIQPH_PAR, 1);
            //flsheet.SetThermoAdmin(thAdmin);
            //flsheet.SetThermo(thermo);
            //SetStream
            var portsIn = stream.GetPorts(FlowDirection.IN);
            //I know in advance there's only one port in
            stream.SetCompositionValue(portsIn["In1"], "PROPANE", 0.5, SourceEnum.Input);
            stream.SetCompositionValue(portsIn["In1"], "n-BUTANE", 0.5, SourceEnum.Input);
            stream.SetCompositionValue(portsIn["In1"], "i-BUTANE", 0.0, SourceEnum.Input);
            stream.SetCompositionValue(portsIn["In1"], "n-PENTANE", 0.0, SourceEnum.Input);
            stream.Ports["In1"].SetPortValue(ePropID.T, 273.15, SourceEnum.Input);
            stream.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.P, 1, SourceEnum.Input);
            stream.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            //stream.Solve(); // --- Could be done, but not yet
            //Set second Stream
            portsIn = stream2.GetPorts(FlowDirection.IN);
            //I know in advance there's only one port in
            stream2.SetCompositionValue(portsIn["In1"], "PROPANE", 0.0, SourceEnum.Input);
            stream2.SetCompositionValue(portsIn["In1"], "n-BUTANE", 0.0, SourceEnum.Input);
            stream2.SetCompositionValue(portsIn["In1"], "i-BUTANE", 0.5, SourceEnum.Input);
            stream2.SetCompositionValue(portsIn["In1"], "n-PENTANE", 0.5, SourceEnum.Input);
            stream2.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.T, 200.15, SourceEnum.Input);
            stream2.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.P, 2.0, SourceEnum.Input);
            stream2.GetPorts(FlowDirection.IN)["In1"].SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            //stream.Solve() --- Could be done, but not yet
            //Set a mixer
            //mixer.SetParameterValue(NUSTIN_PAR, 2);
            //Set Flash UO
            //I already know the names of the ports
            string uOpNameOut = "myStream1";
            string portNameOut;
            string uOpNameIn = "myMixer";
            string portNameIn;
            Debug.WriteLine("conn " + flsheet.ConnectPorts(mixer.PortIn1, stream.Port));
            portNameIn = "In2";
            Debug.WriteLine("conn2 " + FlowSheet.ConnectPorts(stream2, "Out1", mixer, portNameIn));
            portNameOut = "Out1";
            portNameIn = "In1";
            Debug.WriteLine("conn3 " + FlowSheet.ConnectPorts(mixer, portNameOut, flash, portNameIn));
            flsheet.PreSolve();
            portsIn = flash.GetPorts(FlowDirection.IN);
            var portsOut = flash.GetPorts(FlowDirection.OUT);
            //Print  some info in

            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("");

            foreach (var i in stream.Ports)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of port in \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of mixer port in \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }

            foreach (var i in stream2.Ports)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of port in \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of mixer port in \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }

            foreach (var i in mixer.Ports)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of port in \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of mixer port in \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }

            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of port in \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of flash port in \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Debug.WriteLine("Composition of flash port out \"" + i.Name + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Debug.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Debug.WriteLine("");
                Debug.WriteLine("Some props of flash port out \"" + i.Name + "\":");
                Debug.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Debug.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Debug.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Debug.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Debug.WriteLine("");
            }
            Debug.WriteLine(@"Finished Flowsh1 ++++++++++++++++++++++++++++++");
            flsheet.ModelStack.Clear();
        }

        public static void TestSimpleFlash()
        {
            FlowSheet flsheet = new();
            Port_Material port;
            double[] comp;
            Console.WriteLine("Init SimpleFlash ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };

            foreach (var i in cmpNames)
                flsheet.AddComponent(i);

            //#Load vals

            var flash = new SimpleFlash();
            var cmps = flash.Flowsheet.Flowsheet.GetCompoundNames();
            flash.SetParameterValue(NULIQPH_PAR, 1);
            Debug.WriteLine("Cmps: ", cmps);
            List<string> portsIn = flash.GetPortNames(FlowDirection.IN);
            Debug.WriteLine("Names Ports In: ", portsIn);
            List<string> portsOut = flash.GetPortNames(FlowDirection.OUT);
            Debug.WriteLine("Names Ports Out: ", portsOut);

            var comps = new List<double> { 0.25, 0.25, 0.25, 0.25 };
            flash.SetCompositionValues(portsIn[0], comps, SourceEnum.Input);

            flash.GetPort(portsIn[0]).SetPortValue(ePropID.T, 273.15, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            Debug.WriteLine("return   value from Solve()", flash.Solve());
            Debug.WriteLine("");

            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port in \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port in \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
                port = flash.GetPort(i);
                Console.WriteLine($"Array of ");
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port out \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j].ToString());

                Console.WriteLine("");
                Console.WriteLine("Some props of port out \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
                port = flash.GetPort(i);
                Console.WriteLine("");
            }
            Console.WriteLine("Finished SimpleFlash ++++++++++++++++++++++++++++++");
            flash.CleanUp();
        }

        public void TestSimpleFlash2LPhase()
        {
            double[] comp;
            Console.WriteLine(@"Init SimpleFlash2LPhase ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE", "H2O" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //#Load vals
            var flash = new SimpleFlash();
            //flash.SetThermoAdmin(thAdmin);
            //flash.SetThermo(thermo);
            flash.SetParameterValue(NULIQPH_PAR, 2);
            var cmps = flash.Flowsheet.GetCompoundNames();
            Console.WriteLine("Cmps: " + cmps);
            var portsIn = flash.GetPortNames(FlowDirection.IN);
            Console.WriteLine("Names Ports In: " + portsIn);
            var portsOut = flash.GetPortNames(FlowDirection.OUT);
            Console.WriteLine("Names Ports Out: " + portsOut);
            flash.SetCompositionValue(portsIn[0], "PROPANE", 0.2, SourceEnum.Input);
            flash.SetCompositionValue(portsIn[0], "n-BUTANE", 0.2, SourceEnum.Input);
            flash.SetCompositionValue(portsIn[0], "i-BUTANE", 0.2, SourceEnum.Input);
            flash.SetCompositionValue(portsIn[0], "n-PENTANE", 0.2, SourceEnum.Input);
            flash.SetCompositionValue(portsIn[0], "H2O", 0.2, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.T, 273.15, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            Console.WriteLine("return   value from Solve()" + flash.Solve());
            Console.WriteLine("");
            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port in \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port in \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port out \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port out \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            Console.WriteLine(@"Finished SimpleFlash2LPhase ++++++++++++++++++++++++++++++");
            flash.CleanUp();
            //thAdmin.CleanUp();
        }

        public void TestMixAndFlash()
        {
            fs.FlowsheetComponentList.Clear();
            double[] comp;
            Console.WriteLine(@"Init MixAndFlash ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };
            /* var thAdmin = new  ThermoAdmin();
             var providers = thAdmin.GetAvThermoProviderNames();
             var provider = providers.ToList()[0];
             var thcase   = "myTh";
             var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //var FlowSheet = new  FlowSheet();
            //FlowSheet.SetThermoAdmin(thAdmin);
            //FlowSheet.SetThermo(thermo);
            //#Load vals
            UnitOperation flash = new MixAndFlash();
            fs.Add(flash, "MixAndFlash");
            var cmps = flash.Flowsheet.GetCompoundNames();
            Console.WriteLine("Cmps: " + cmps);
            var portsIn = flash.GetPortNames(FlowDirection.IN);
            Console.WriteLine("Names Ports In: " + portsIn);
            var portsOut = flash.GetPortNames(FlowDirection.OUT);
            Console.WriteLine("Names Ports Out: " + portsOut);

            flash.SetCompositionValue(portsIn[0], "PROPANE", 0.5);
            flash.SetCompositionValue(portsIn[0], "n-BUTANE", 0.5);
            flash.SetCompositionValue(portsIn[0], "i-BUTANE", 0.0);
            flash.SetCompositionValue(portsIn[0], "n-PENTANE", 0.0);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.T, 273.15, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);

            flash.SetCompositionValue(portsIn[1], "PROPANE", 0.0);
            flash.SetCompositionValue(portsIn[1], "n-BUTANE", 0.0);
            flash.SetCompositionValue(portsIn[1], "i-BUTANE", 0.5);
            flash.SetCompositionValue(portsIn[1], "n-PENTANE", 0.5);
            flash.GetPort(portsIn[1]).SetPortValue(ePropID.T, 273.15, SourceEnum.Input);
            flash.GetPort(portsIn[1]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            flash.GetPort(portsIn[1]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);

            Console.WriteLine("return   value from Solve()" + fs.PreSolve());
            Console.WriteLine("");

            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port in " + i + ":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port in " + i + ":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port out " + i + ":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port out " + i + ":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.P + ": " + flash.GetPropValue(i, ePropID.P));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            Console.WriteLine(@"Finished MixAndFlash ++++++++++++++++++++++++++++++");
            flash.CleanUp();
            //thAdmin.CleanUp();
        }

        public void TestLiqLiqEx()
        {
            double[] comp;
            Console.WriteLine(@"Init LiqLiqEx ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "n-HEPTANE", "BENZENE", "TEGlycol" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //#Load vals
            var lle = new LiqLiqEx();
            //lle.SetThermoAdmin(thAdmin);
            //lle.SetThermo(thermo);
            var cmps = lle.Flowsheet.GetCompoundNames();
            Console.WriteLine("Cmps: " + cmps);
            var portsIn = lle.GetPortNames(FlowDirection.IN);
            Console.WriteLine("Names Ports In: " + portsIn);
            var portsOut = lle.GetPortNames(FlowDirection.OUT);
            Console.WriteLine("Names Ports Out: " + portsOut);
            //lle.SetParameterValue(LIQ_MOV, "BENZENE");

            //# Next line has a bug... so don't change the default
            //lle.SetParameterValue(NUSTAGES_PAR, 10)
            lle.SetCompositionValue(FEED_PORT, "n-HEPTANE", 0.5);
            lle.SetCompositionValue(FEED_PORT, "BENZENE", 0.5);
            lle.SetCompositionValue(FEED_PORT, "TEGlycol", 0.0);
            lle.GetPort(FEED_PORT).SetPortValue(ePropID.T, 273.15, SourceEnum.Input);
            lle.GetPort(FEED_PORT).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            lle.GetPort(FEED_PORT).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            lle.SetCompositionValue(SOLV_PORT, "n-HEPTANE", 0.0);
            lle.SetCompositionValue(SOLV_PORT, "BENZENE", 0.0);
            lle.SetCompositionValue(SOLV_PORT, "TEGlycol", 1.0);
            lle.GetPort(SOLV_PORT).SetPortValue(ePropID.T, 273.15, SourceEnum.Input);
            lle.GetPort(SOLV_PORT).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            lle.GetPort(SOLV_PORT).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            Console.WriteLine("return   value from Solve()" + lle.Solve());
            Console.WriteLine("");
            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = lle.GetCompositionValues(i);
                Console.WriteLine("Composition of port in \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port in \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + lle.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + lle.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + lle.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = lle.GetCompositionValues(i);
                Console.WriteLine("Composition of port out \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port out \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + lle.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + lle.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + lle.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            Console.WriteLine(@"Finished LiqLiqEx ++++++++++++++++++++++++++++++");
            lle.CleanUp();
            //thAdmin.CleanUp();
        }

        public void TestHeater()
        {
            double[] comp;
            Console.WriteLine(@"Init Heater ++++++++++++++++++++++++++++++");
            //#Set Thermo
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //#Load vals
            var heater = new Heater();
            //heater.SetThermoAdmin(thAdmin);
            //heater.SetThermo(thermo);
            var cmps = heater.Flowsheet.GetCompoundNames();
            Console.WriteLine("Cmps: " + cmps);
            var portsIn = heater.GetPortNames(FlowDirection.IN);
            Console.WriteLine("Names Ports In: " + portsIn);
            var portsOut = heater.GetPortNames(FlowDirection.OUT);
            Console.WriteLine($"Names Ports Out: ");
            heater.SetParameterValue(NULIQPH_PAR, 1);
            heater.SetCompositionValue(portsIn[0], "PROPANE", 0.25);
            heater.SetCompositionValue(portsIn[0], "n-BUTANE", 0.25);
            heater.SetCompositionValue(portsIn[0], "i-BUTANE", 0.25);
            heater.SetCompositionValue(portsIn[0], "n-PENTANE", 0.25);
            heater.GetPort(portsIn[0]).SetPortValue(ePropID.T, 273.15 + 50, SourceEnum.Input);
            heater.GetPort(portsIn[0]).SetPortValue(ePropID.P, 1, SourceEnum.Input);
            heater.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 10.0, SourceEnum.Input);
            heater.GetPort("InQ1").SetPortValue(ePropID.EnergyFlow, 1000.0, SourceEnum.Input);
            heater.DP.SetPortValue(ePropID.DeltaP, 0.0, SourceEnum.Input);
            Console.WriteLine("return   value from Solve()" + heater.Solve());
            Console.WriteLine("");
            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = heater.GetCompositionValues(i);
                Console.WriteLine("Composition of port in \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of port in \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + heater.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.P + ": " + heater.GetPropValue(i, ePropID.P));
                Console.WriteLine(ePropID.H + ": " + heater.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + heater.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = heater.GetCompositionValues(i);
                Console.WriteLine($"Composition of port out \"");
                foreach (var j in Enumerable.Range(0, count: comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j].ToString());

                Console.WriteLine("");
                Console.WriteLine("Some props of port out \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + heater.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.P + ": " + heater.GetPropValue(i, ePropID.P));
                Console.WriteLine(ePropID.H + ": " + heater.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + heater.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            Console.WriteLine(@"Finished Heater ++++++++++++++++++++++++++++++");
            heater.CleanUp();
            //thAdmin.CleanUp();
        }

        public void TestHeatEx()
        {
            Console.WriteLine(@"Init Heat Exchanger ++++++++++++++++++++++++++++++");

            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            // parent FlowSheet
            var FlowSheet = new FlowSheet();
            //FlowSheet.SetThermoAdmin(thAdmin);
            //FlowSheet.SetThermo(thermo);
            var hotInlet = new StreamMaterial();
            FlowSheet.Add(hotInlet, "HotIn");
            var coldInlet = new StreamMaterial();
            FlowSheet.Add(coldInlet, "ColdIn");
            var hotOutlet = new StreamMaterial();
            FlowSheet.Add(hotOutlet, "HotOut");
            var coldOutlet = new StreamMaterial();
            FlowSheet.Add(coldOutlet, "ColdOut");

            Port_Material hotInPort = hotInlet.Port;
            hotInPort.SetCompositionValue("PROPANE", 0.25, SourceEnum.Input);
            hotInPort.SetCompositionValue("n-BUTANE", 0.25, SourceEnum.Input);
            hotInPort.SetCompositionValue("i-BUTANE", 0.25, SourceEnum.Input);
            hotInPort.SetCompositionValue("n-PENTANE", 0.25, SourceEnum.Input);
            hotInPort.SetPortValue(ePropID.T, 375.0, SourceEnum.Input);
            hotInPort.SetPortValue(ePropID.P, 500.0, SourceEnum.Input);
            hotInPort.SetPortValue(ePropID.MOLEF, 800.0, SourceEnum.Input);

            var coldInPort = coldInlet.Port;
            coldInPort.SetCompositionValue("PROPANE", 0.95, SourceEnum.Input);
            coldInPort.SetCompositionValue("n-BUTANE", 0.05, SourceEnum.Input);
            coldInPort.SetCompositionValue("i-BUTANE", 0.0, SourceEnum.Input);
            coldInPort.SetCompositionValue("n-PENTANE", 0.0, SourceEnum.Input);
            coldInPort.SetPortValue(ePropID.Q, 0.0, SourceEnum.Input);
            coldInPort.SetPortValue(ePropID.P, 300.0, SourceEnum.Input);
            coldInPort.SetPortValue(ePropID.MOLEF, 1000.0, SourceEnum.Input);

            coldInPort.Flash();

            var exch = new HeatExchanger2();
            FlowSheet.Add(exch, "Exchanger");
            exch.GetSignalPort(DELTAP_PORT + "_H").SetPortValue(ePropID.DeltaP, 50.0, SourceEnum.Input);
            exch.GetSignalPort(DELTAP_PORT + "_C").SetPortValue(ePropID.T, 10.0, SourceEnum.Input);
            exch.GetSignalPort(DELTAT_PORT + "_H").SetPortValue(ePropID.H, 5.0, SourceEnum.Input);

            exch.SetParameterValue("COUNTER_CURRENT_PAR", 1);
            FlowSheet.ConnectPorts(hotInlet, exch.cooler.PortIn);
            FlowSheet.ConnectPorts(coldInlet, exch.heater.PortIn);
            FlowSheet.ConnectPorts(hotOutlet, exch.cooler.PortOut);
            FlowSheet.ConnectPorts(coldOutlet, exch.heater.PortOut);

            FlowSheet.PreSolve();

            foreach (var s in new List<StreamMaterial>() { hotInlet, hotOutlet, coldInlet, coldOutlet })
            {
                var port = (Port_Material)s.GetPort(OUT_PORT);
                var comp = port.MoleFractions;
                Console.WriteLine("Stream" + s.GetPath());
                Console.WriteLine(ePropID.T + ": " + port.T_);
                Console.WriteLine(ePropID.P + ": " + port.P_);
                Console.WriteLine(ePropID.H + ": " + port.H_);
                Console.WriteLine(ePropID.MOLEF + ": " + port.MolarFlow_);
                Console.WriteLine("");
                Console.WriteLine("Composition");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
            }
            Console.WriteLine(@"Finished Heat Exchanger ++++++++++++++++++++++++++++++");
            FlowSheet.Clear();
            //thAdmin.CleanUp();
        }

        public void TestFlowsh1()
        {
            double[] comp;
            Console.WriteLine(@"Init Flowsh1 ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };
            /*var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //Create unit Operations
            var stream = new StreamMaterial();
            var stream2 = new StreamMaterial();
            var mixer = new Mixer();
            var flash = new SimpleFlash();
            //Add all the units to a FlowSheet and connect them
            fs.Add(flash, "myFlash1");
            fs.Add(stream, "myStream1");
            fs.Add(stream2, "myStream2");
            fs.Add(mixer, "myMixer");
            fs.SetParameterValue(NULIQPH_PAR, 1);
            //SetStream
            var portsIn = stream.GetPortNames(FlowDirection.IN);
            //I know in advance there's only one port in
            stream.SetCompositionValue(portsIn[0], "PROPANE", 0.5);
            stream.SetCompositionValue(portsIn[0], "n-BUTANE", 0.5);
            stream.SetCompositionValue(portsIn[0], "i-BUTANE", 0.0);
            stream.SetCompositionValue(portsIn[0], "n-PENTANE", 0.0);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.T, 460.15, SourceEnum.Input);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.P, 715.0, SourceEnum.Input);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            //stream.Solve() --- Could be done, but not yet
            //Set second Stream
            portsIn = stream2.GetPortNames(FlowDirection.IN);
            //I know in advance there's only one port in
            stream2.SetCompositionValue(portsIn[0], "PROPANE", 0.0);
            stream2.SetCompositionValue(portsIn[0], "n-BUTANE", 0.0);
            stream2.SetCompositionValue(portsIn[0], "i-BUTANE", 0.5);
            stream2.SetCompositionValue(portsIn[0], "n-PENTANE", 0.5);
            stream2.GetPort(portsIn[0]).SetPortValue(ePropID.T, 200.15, SourceEnum.Input);
            stream2.GetPort(portsIn[0]).SetPortValue(ePropID.P, 715.0, SourceEnum.Input);
            stream2.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            //stream.Solve() --- Could be done, but not yet
            //Set a mixer
            mixer.SetParameterValue(NUSTIN_PAR, 2);
            //Set Flash UO
            //I already know the names of the ports
            Console.WriteLine("conn" + fs.ConnectPorts("myStream1", "Out1", "myMixer", "In1"));
            Console.WriteLine("conn2" + fs.ConnectPorts("myStream2", "Out1", "myMixer", "In2"));
            Console.WriteLine("conn3" + fs.ConnectPorts("myMixer", "Out1", "myFlash1", "In1"));
            fs.PreSolve();
            portsIn = flash.GetPortNames(FlowDirection.IN);
            var portsOut = flash.GetPortNames(FlowDirection.OUT);
            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port in \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of flash port in \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of flash port out \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, count: comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of flash port out \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            Console.WriteLine(@"Finished Flowsh1 ++++++++++++++++++++++++++++++");
            fs.Clear();
        }

        public void TestFlowsh2()
        {
            double[] comp;
            Console.WriteLine(@"Init Flowsh2 ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //SetStream
            var stream = new StreamMaterial();
            var stream2 = new StreamMaterial();
            var mixer = new Mixer();
            var flash = new SimpleFlash();
            //Add all the units to a FlowSheet and connect them
            fs.Add(flash, "myFlash1");
            fs.Add(stream, "myStream1");
            fs.Add(stream2, "myStream2");
            fs.Add(mixer, "myMixer");
            fs.SetParameterValue(NULIQPH_PAR, 1);
            //flsheet.SetThermoAdmin(thAdmin);
            //flsheet.SetThermo(thermo);
            List<string> portsIn = stream.GetPortNames(FlowDirection.IN);
            //I know in advance there's only one port in
            stream.SetCompositionValue(portsIn[0], "PROPANE", 0.5);
            stream.SetCompositionValue(portsIn[0], "n-BUTANE", 0.5);
            stream.SetCompositionValue(portsIn[0], "i-BUTANE", 0.0);
            stream.SetCompositionValue(portsIn[0], "n-PENTANE", 0.0);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.T, 460.15, SourceEnum.Input);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.P, 715.0, SourceEnum.Input);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            //stream.Solve() --- Could be done, but not yet
            //Set second Stream
            portsIn = stream2.GetPortNames(FlowDirection.IN);
            //I know in advance there's only one port in
            stream2.SetCompositionValue(portsIn[0], "PROPANE", 0.0);
            stream2.SetCompositionValue(portsIn[0], "n-BUTANE", 0.0);
            stream2.SetCompositionValue(portsIn[0], "i-BUTANE", 0.5);
            stream2.SetCompositionValue(portsIn[0], "n-PENTANE", 0.5);
            stream2.GetPort(portsIn[0]).SetPortValue(ePropID.T, 200.15, SourceEnum.Input);
            stream2.GetPort(portsIn[0]).SetPortValue(ePropID.P, 715.0, SourceEnum.Input);
            stream2.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            //stream.Solve() --- Could be done, but not yet
            //Set a mixer
            mixer.SetParameterValue(NUSTIN_PAR, 2);
            //Set Flash UO
            //I already know the names of the ports
            var uOpNameOut = "myStream1";
            var portNameOut = "Out";
            var uOpNameIn = "myMixer";
            var portNameIn = "In0";
            Console.WriteLine($"conn" + fs.ConnectPorts(uOpNameOut, portNameOut, uOpNameIn, portNameIn).ToString());
            uOpNameOut = "myStream2";
            portNameOut = "Out";
            uOpNameIn = "myMixer";
            portNameIn = "In1";
            Console.WriteLine($"conn2" + fs.ConnectPorts(uOpNameOut, portNameOut, uOpNameIn, portNameIn));
            uOpNameOut = "myMixer";
            portNameOut = "Out";
            uOpNameIn = "myFlash1";
            portNameIn = "In";
            Console.WriteLine($"conn3" + fs.ConnectPorts(uOpNameOut, portNameOut, uOpNameIn, portNameIn));
            fs.PreSolve();
            portsIn = flash.GetPortNames(FlowDirection.IN);
            var portsOut = flash.GetPortNames(FlowDirection.OUT);
            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of port in \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of flash port in \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of flash port out \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmpNames[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of flash port out \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }

            Console.WriteLine(@"Now lets add two cmps and delete one ++++++++++++++++++++++++++++++");

            fs.AddComponent("n-HEXANE");
            fs.AddComponent("n-C12");
            fs.DeleteCompound("n-PENTANE");

            portsIn = stream.GetPortNames(FlowDirection.IN);
            stream.SetCompositionValue(portsIn[0], "n-HEXANE", 0.25);
            stream.SetCompositionValue(portsIn[0], "n-C12", 0.25);

            portsIn = stream.GetPortNames(FlowDirection.IN);
            stream2.SetCompositionValue(portsIn[0], "n-HEXANE", 0.3);
            stream2.SetCompositionValue(portsIn[0], "n-C12", 0.2);

            fs.PreSolve();

            var cmps = flash.Flowsheet.GetCompoundNames();
            portsIn = flash.GetPortNames(FlowDirection.IN);
            portsOut = flash.GetPortNames(FlowDirection.OUT);

            //Print  some info in
            foreach (var i in portsIn)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of flash port in \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmps[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of flash port in \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            //Print  some info out
            foreach (var i in portsOut)
            {
                comp = flash.GetCompositionValues(i);
                Console.WriteLine("Composition of flash port out \"" + i + "\":");
                foreach (var j in Enumerable.Range(0, comp.Length))
                    Console.WriteLine("fraction of " + cmps[j] + ": " + comp[j]);

                Console.WriteLine("");
                Console.WriteLine("Some props of flash port out \"" + i + "\":");
                Console.WriteLine(ePropID.T + ": " + flash.GetPropValue(i, ePropID.T));
                Console.WriteLine(ePropID.H + ": " + flash.GetPropValue(i, ePropID.H));
                Console.WriteLine(ePropID.MOLEF + ": " + flash.GetPropValue(i, ePropID.MOLEF));
                Console.WriteLine("");
            }
            Console.WriteLine(@"Finished Flowsh2 ++++++++++++++++++++++++++++++");
            fs.Clear();
            //thAdmin.CleanUp();
        }

        public void TestRecycle()
        {
            Console.WriteLine(@"Init TestRecycle ++++++++++++++++++++++++++++++");
            //#Set Thermo

            //#    cmpNames = ("n-HEPTANE", "BENZENE", "TRIETHYLENE GLYCOL")
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-NONANE" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //Create a FlowSheet to contain all the units
            fs.SetParameterValue(NULIQPH_PAR, 1);
            //SetStream
            var stream = new StreamMaterial();
            fs.Add(stream, "Feed");
            //stream.SetThermoAdmin(thAdmin);
            //stream.SetThermo(thermo);
            var portsIn = stream.GetPortNames(FlowDirection.IN);
            //I know in advance there's only one port in
            stream.SetCompositionValue(portsIn[0], "PROPANE", 0.25);
            stream.SetCompositionValue(portsIn[0], "n-BUTANE", 0.25);
            stream.SetCompositionValue(portsIn[0], "i-BUTANE", 0.25);
            stream.SetCompositionValue(portsIn[0], "n-NONANE", 0.25);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.T, 360.15, SourceEnum.Input);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.P, 715.0, SourceEnum.Input);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 3000.0, SourceEnum.Input);
            var recycle = new StreamMaterial();
            fs.Add(recycle, "Recycle");
            //recycle.SetThermoAdmin(thAdmin);
            //recycle.SetThermo(thermo);
            portsIn = recycle.GetPortNames(FlowDirection.IN);
            var fixedGuess = SourceEnum.FixedEstimate;
            //I know in advance there's only one port in
            recycle.SetCompositionValue(portsIn[0], "PROPANE", 0.0, fixedGuess);
            recycle.SetCompositionValue(portsIn[0], "n-BUTANE", 0.0, fixedGuess);
            recycle.SetCompositionValue(portsIn[0], "i-BUTANE", 0.5, fixedGuess);
            recycle.SetCompositionValue(portsIn[0], "n-NONANE", 0.5, fixedGuess);
            recycle.GetPort(portsIn[0]).SetPortValue(ePropID.T, 460.15, fixedGuess);
            recycle.GetPort(portsIn[0]).SetPortValue(ePropID.P, 715.0, fixedGuess);
            recycle.GetPort(portsIn[0]).SetPortValue(ePropID.MOLEF, 300.0, fixedGuess);
            //Set a mixer
            var mixer = new Mixer();
            fs.Add(mixer, "Mixer");
            //mixer.SetThermoAdmin(thAdmin);
            //mixer.SetThermo(thermo);
            mixer.SetParameterValue(NUSTIN_PAR, 2);
            fs.ConnectPorts("Feed", "Out", "Mixer", "In0");
            fs.ConnectPorts("Recycle", "Out", "Mixer", "In1");
            //Set Flash UO
            var flash = new SimpleFlash();
            fs.Add(flash, "Flash");
            //flash.SetThermoAdmin(thAdmin);
            //flash.SetThermo(thermo);
            fs.ConnectPorts("Mixer", "Out", "Flash", "In");
            //Set a splitter
            var splitter = new Divider();
            fs.Add(splitter, "Splitter");
            //splitter.SetThermoAdmin(thAdmin);
            //splitter.SetThermo(thermo);
            splitter.SetParameterValue(NUSTOUT_PAR, 2);

            fs.ConnectPorts("Flash", "Liq0", "Splitter", "In");

            splitter.GetPort("Out1").SetPortValue(ePropID.MOLEF, 200.0, SourceEnum.Input);

            // close recycle
            fs.ConnectPorts("Splitter", "Out1", "Recycle", "In");

            fs.PreSolve();
            Console.WriteLine("***************");
            var cmps = splitter.GetCompositionValues("Out1");

            foreach (var j in Enumerable.Range(0, cmps.Length))
                Console.WriteLine("fraction of " + cmpNames[j] + ": " + cmps[j]);

            Console.WriteLine("***************");
            Console.WriteLine("Some properties of splitter Out0");
            Console.WriteLine(ePropID.T + ": " + splitter.GetPropValue("Out1", ePropID.T));
            Console.WriteLine(ePropID.P + ": " + splitter.GetPropValue("Out1", ePropID.P));
            Console.WriteLine(ePropID.H + ": " + splitter.GetPropValue("Out1", ePropID.H));
            Console.WriteLine(ePropID.MOLEF + ": " + splitter.GetPropValue("Out1", ePropID.MOLEF));
            Console.WriteLine("****reset Pressure ***");
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.P, 800.0, SourceEnum.Input);
            fs.PreSolve();
            Console.WriteLine("*************** splitter out1");
            cmps = splitter.GetCompositionValues("Out2");
            foreach (var j in Enumerable.Range(0, cmps.Length))
                Console.WriteLine("fraction of " + cmpNames[j] + ": " + cmps[j]);

            Console.WriteLine("***************");
            Console.WriteLine("Some properties of splitter Out2");
            Console.WriteLine(ePropID.T + ": " + splitter.GetPropValue("Out2", ePropID.T));
            Console.WriteLine(ePropID.P + ": " + splitter.GetPropValue("Out2", ePropID.P));
            Console.WriteLine(ePropID.H + ": " + splitter.GetPropValue("Out2", ePropID.H));
            Console.WriteLine(ePropID.MOLEF + ": " + splitter.GetPropValue("Out2", ePropID.MOLEF));
            Console.WriteLine(@"Finished TestRecycle ++++++++++++++++++++++++++++++");
            fs.Clear();
            //thAdmin.CleanUp();
        }

        public void TestRecycle2()
        {
            Console.WriteLine(@"Init TestRecycle2 ++++++++++++++++++++++++++++++");
            //#Set Thermo

            //#    cmpNames = ("n-HEPTANE", "BENZENE", "TRIETHYLENE GLYCOL")
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-NONANE" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            //flsheet.SetThermoAdmin(thAdmin);
            //flsheet.SetThermo(thermo);
            fs.SetParameterValue(NULIQPH_PAR, 1);
            //SetStream
            var stream = new StreamMaterial();
            fs.Add(stream, "Feed");
            var portsIn = stream.GetPortNames(FlowDirection.IN);
            //I know in advance there's only one port in
            stream.SetCompositionValue(portsIn[0], "PROPANE", 0.25);
            stream.SetCompositionValue(portsIn[0], "n-BUTANE", 0.25);
            stream.SetCompositionValue(portsIn[0], "i-BUTANE", 0.25);
            stream.SetCompositionValue(portsIn[0], "n-NONANE", 0.25);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.T, 360.15, SourceEnum.Input);
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.P, 715.0, SourceEnum.Input);
            var recycle = new StreamMaterial();
            fs.Add(recycle, "Recycle");
            //I know in advance there's only one port in
            //Set a mixer
            var mixer = new Mixer();
            fs.Add(mixer, "Mixer");
            mixer.SetParameterValue(NUSTIN_PAR, 2);
            fs.ConnectPorts("Feed", "Out", "Mixer", "In0");
            fs.ConnectPorts("Recycle", "Out", "Mixer", "In1");
            //Set mixed Stream
            var mixed = new StreamMaterial();
            fs.Add(mixed, "Mixed");
            var fixedGuess = SourceEnum.FixedEstimate;
            portsIn = mixed.GetPortNames(FlowDirection.IN);
            //Set Flash UO
            var flash = new SimpleFlash();
            fs.Add(flash, "Flash");
            // to start, just guess same as feed
            flash.SetCompositionValue(portsIn[0], "PROPANE", 0.25, fixedGuess);
            flash.SetCompositionValue(portsIn[0], "n-BUTANE", 0.25, fixedGuess);
            flash.SetCompositionValue(portsIn[0], "i-BUTANE", 0.25, fixedGuess);
            flash.SetCompositionValue(portsIn[0], "n-NONANE", 0.25, fixedGuess);
            flash.GetPort(portsIn[0]).SetPortValue(ePropID.T, 360.15, fixedGuess);
            fs.ConnectPorts("Mixer", "Out", "Mixed", "In");
            fs.ConnectPorts("Mixed", "Out", "Flash", "In");
            //Set a splitter
            var splitter = new Divider();
            fs.Add(splitter, "Splitter");
            splitter.SetParameterValue(NUSTOUT_PAR, 2);
            fs.ConnectPorts("Flash", "Liq0", "Splitter", "In");
            splitter.GetPort("Out1").SetPortValue(ePropID.MOLEF, 200.0, SourceEnum.Input);
            splitter.GetPort("Out1").SetPortValue(ePropID.P, 715.0, SourceEnum.Input);
            flash.GetPort("Out1").SetPortValue(ePropID.MOLEF, 1652.682, SourceEnum.Input);
            // close recycle
            fs.ConnectPorts("Splitter", "Out1", "Recycle", "In");
            // add balance to back calculate flow
            var bal = new Balanceobject();
            fs.Add(bal, "Balance");
            bal.SetParameterValue(NUSTIN_PAR + "MAT", 2);
            bal.SetParameterValue(NUSTOUT_PAR + "MAT", 1);
            bal.SetParameterValue("BalanceType", BalanceModelType.Molar);
            fs.ConnectPorts("Balance", "In1", "Flash", "Vap"); fs.ConnectPorts("Balance", "In0", "Splitter", "Out0");
            fs.ConnectPorts("Balance", "Out0", "Feed", "In");
            fs.PreSolve();

            Console.WriteLine("***************");
            var cmps = splitter.GetCompositionValues("Out1");
            foreach (var j in Enumerable.Range(0, cmps.Length))
                Console.WriteLine("fraction of " + cmpNames[j] + ": " + cmps[j]);

            Console.WriteLine("***************");
            Console.WriteLine("Some properties of splitter Out1");
            Console.WriteLine(ePropID.T + ": " + splitter.GetPropValue("Out1", ePropID.T));
            Console.WriteLine(ePropID.P + ": " + splitter.GetPropValue("Out1", ePropID.P));
            Console.WriteLine(ePropID.H + ": " + splitter.GetPropValue("Out1", ePropID.H));
            Console.WriteLine(ePropID.MOLEF + ": " + splitter.GetPropValue("Out1", ePropID.MOLEF));
            Console.WriteLine("Some properties of mixed Out");
            Console.WriteLine(ePropID.T + ": " + mixed.GetPropValue("Out1", ePropID.T));
            Console.WriteLine(ePropID.P + ": " + mixed.GetPropValue("Out1", ePropID.P));
            Console.WriteLine(ePropID.H + ": " + mixed.GetPropValue("Out1", ePropID.H));
            Console.WriteLine(ePropID.MOLEF + ": " + mixed.GetPropValue("Out1", ePropID.MOLEF));
            Console.WriteLine("Some properties of stream Out");
            Console.WriteLine(ePropID.T + ": " + stream.GetPropValue("Out1", ePropID.T));
            Console.WriteLine(ePropID.P + ": " + stream.GetPropValue("Out1", ePropID.P));
            Console.WriteLine(ePropID.H + ": " + stream.GetPropValue("Out1", ePropID.H));
            Console.WriteLine(ePropID.MOLEF + ": " + stream.GetPropValue("Out1", ePropID.MOLEF));
            Console.WriteLine("****reset Pressure ***");
            //stream.GetPort(portsIn[0]).SetPropValue(GlobalePropID.P, 800.0, GlobalSourceEnum.Fixed)
            stream.GetPort(portsIn[0]).SetPortValue(ePropID.T, 400.0, SourceEnum.Input);
            fs.PreSolve();

            Console.WriteLine("*************** splitter out1");

            cmps = splitter.GetCompositionValues("Out1");
            foreach (var j in Enumerable.Range(0, cmps.Length))
                Console.WriteLine("fraction of " + cmpNames[j] + ": " + cmps[j]);

            Console.WriteLine("***************");
            Console.WriteLine("Some properties of splitter Out1");
            Console.WriteLine(ePropID.T + ": " + splitter.GetPropValue("Out1", ePropID.T));
            Console.WriteLine(ePropID.P + ": " + splitter.GetPropValue("Out1", ePropID.P));
            Console.WriteLine(ePropID.H + ": " + splitter.GetPropValue("Out1", ePropID.H));
            Console.WriteLine(ePropID.MOLEF + ": " + splitter.GetPropValue("Out1", ePropID.MOLEF));
            Console.WriteLine(@"Finished TestRecycle2 ++++++++++++++++++++++++++++++");
            fs.Clear();
            //thAdmin.CleanUp();
        }

        public void TestMoleBalance()
        {
            Console.WriteLine(@"Init TestMoleBalance ++++++++++++++++++++++++++++++");

            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE" };
            /*var thAdmin = new  ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thcase   = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thcase  , pkgName);*/
            var flowSh = new FlowSheet();

            //flowSh.SetThermoAdmin(thAdmin);
            //flowSh.SetThermo(thermo);
            foreach (var i in cmpNames)
                fs.AddComponent(i);

            Port_Material pIn1 = flowSh.CreateMaterialPort(FlowDirection.IN, "pIn1");
            Port_Material pIn2 = flowSh.CreateMaterialPort(FlowDirection.IN, "pIn2");
            Port_Material pOut1 = flowSh.CreateMaterialPort(FlowDirection.OUT, "pOut1");
            Port_Material pOut2 = flowSh.CreateMaterialPort(FlowDirection.OUT, "pOut2");

            pIn1.SetComposition(Guid.Empty, "PROPANE", 0.3, SourceEnum.Input);
            pIn1.SetComposition(Guid.Empty, "n-BUTANE", 0.7, SourceEnum.Input);

            pIn2.SetComposition(Guid.Empty, "PROPANE", 0.4, SourceEnum.Input);
            pIn2.SetComposition(Guid.Empty, "n-BUTANE", 0.6, SourceEnum.Input);

            pOut1.SetComposition(Guid.Empty, "PROPANE", 0.6, SourceEnum.Input);
            pOut1.SetComposition(Guid.Empty, "n-BUTANE", 0.4, SourceEnum.Input);

            pOut2.SetComposition(Guid.Empty, "PROPANE", 0.8, SourceEnum.Input);
            pOut2.SetComposition(Guid.Empty, "n-BUTANE", 0.2, SourceEnum.Input);

            pIn1.SetPortValue(ePropID.MOLEF, 1000, SourceEnum.Input);
            pOut1.SetPortValue(ePropID.MOLEF, 1500, SourceEnum.Input);

            var myBalance = new Balanceobject(BalanceModelType.Matrix);

            myBalance.AddInput(pIn1);
            myBalance.AddInput(pIn2);
            myBalance.AddOutput(pOut1);
            myBalance.AddOutput(pOut2);

            myBalance.Balance();

            Console.WriteLine("moleFlowIn1 " + pIn1.MolarFlow_);
            Console.WriteLine("moleFlowIn2 " + pIn2.MolarFlow_);
            Console.WriteLine("moleFlowOut1 " + pOut1.MolarFlow_);
            Console.WriteLine("moleFlowOut2 " + pOut2.MolarFlow_);
            Console.WriteLine("moleBalance " + (pIn1.MolarFlow_ + pIn2.MolarFlow_ - pOut1.MolarFlow_ - pOut2.MolarFlow_).ToString());
            Console.WriteLine(@"Finished TestMoleBalance ++++++++++++++++++++++++++++++");
            flowSh.Clear();
            //thAdmin.CleanUp();
        }
    }
}