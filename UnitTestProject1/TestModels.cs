using EngineThermo;
using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Units;
using static ModelEngine.PrintInfo;
using StreamMaterial = ModelEngine.StreamMaterial;

namespace UnitTests
{
    [TestClass]
    public class TestModels
    {

        /*   "Q2"
           "HeaterOut"
           "HTR0"
           "ValveToHeater"
           "V0"
           "Q1"
           "PumpToValve"
           "Feed"
           "P0"
        */



        [TestMethod]
        public void TestPumpValveHeaterAltSequence()
        {
            Console.WriteLine(@"Init Pump Valve Heater ++++++++++++++++++++++++++++++");
            var flowsheet = new FlowSheet();
            flowsheet.Name = "Main Flowsheet";
            //#Set Thermo
            var pkgName = "RK";
            var cmpNames = new List<string>() { "n-BUTANE" };
            var thAdmin = new ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thCase = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thCase, pkgName);
            foreach (var i in cmpNames)
                flowsheet.AddComponent(i);

            flowsheet.SetThermoAdmin(thAdmin);
            flowsheet.SetThermo(thermo);
            var Feed = new StreamMaterial();
            flowsheet.Add(Feed, "Feed");

            var PumpToValve = new StreamMaterial();
            flowsheet.Add(PumpToValve, "PumpToValve");

            var ValveToHeater = new StreamMaterial();
            flowsheet.Add(ValveToHeater, "ValveToHeater");

            var heaterOutlet = new StreamMaterial();
            flowsheet.Add(heaterOutlet, "heaterOutlet");


            var feedport = Feed.GetPort("In1");
            feedport.SetCompositionValue("n-BUTANE", 1, SourceEnum.Input);
            feedport.SetPortValue(ePropID.T, SourceEnum.Input, 273.15 + 25);
            feedport.SetPortValue(ePropID.P, SourceEnum.Input, 5.0);
            feedport.SetPortValue(ePropID.MOLEF, SourceEnum.Input, 100.0);

            PumpToValve.PortIn.SetPortValue(ePropID.P, SourceEnum.Input, 10);
            ValveToHeater.PortIn.SetPortValue(ePropID.P, SourceEnum.Input, 5);
            heaterOutlet.PortIn.SetPortValue(ePropID.T, SourceEnum.Input, 100 + 273.15);

            var pump = new Pump();
            flowsheet.Add(pump, "Pump");

            pump.Eff.SetPortValue(ePropID.NullUnits, SourceEnum.Input, 70);

            pump.PortIn.ConnectPorts(Feed.GetPortOut);
            pump.PortOut.ConnectPorts(PumpToValve.GetPortIn);

            var valve = new Valve();
            flowsheet.Add(valve, "valve");
            valve.PortIn.ConnectPorts(PumpToValve.GetPortOut);
            valve.PortOut.ConnectPorts(ValveToHeater.GetPortIn);

            Heater heater = new Heater();
            heater.DP.SetPortValue(ePropID.DeltaP, SourceEnum.Input, 0);
            flowsheet.Add(heater, "Heater");

            heater.PortIn.ConnectPorts(ValveToHeater.GetPortOut);
            heater.PortOut.ConnectPorts(heaterOutlet.GetPortIn);

            flowsheet.PreSolve();

            PrintPortInfo(new List<StreamMaterial>() { Feed, ValveToHeater, PumpToValve, heaterOutlet });
            PrintPortInfo(pump);
            PrintPortInfo(valve);
            PrintPortInfo(heater);

            Debug.WriteLine(@"Finished Pump Valve Heater ++++++++++++++++++++++++++++++");
            flowsheet.CleanUp();
            thAdmin.CleanUp();
        }


        [TestMethod]
        public void TestPumpWithStreams()
        {
            Console.WriteLine(@"Init Pump Valve Heater ++++++++++++++++++++++++++++++");
            var flowsheet = new FlowSheet();
            flowsheet.Name = "Main Flowsheet";
            //#Set Thermo
            var pkgName = "RK";
            var cmpNames = new List<string>() { "n-BUTANE" };
            var thAdmin = new ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thCase = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thCase, pkgName);
            foreach (var i in cmpNames)
                flowsheet.AddComponent(i);

            flowsheet.SetThermoAdmin(thAdmin);
            flowsheet.SetThermo(thermo);
            var Feed = new StreamMaterial();
            flowsheet.Add(Feed, "Feed");

            var PumpToValve = new StreamMaterial();
            flowsheet.Add(PumpToValve, "PumpToValve");

            var feedport = Feed.GetPort("In1");
            feedport.SetCompositionValue("n-BUTANE", 1, SourceEnum.Input);
            feedport.SetPortValue(ePropID.T, SourceEnum.Input, 273.15 + 25);
            feedport.SetPortValue(ePropID.P, SourceEnum.Input, 5.0);
            feedport.SetPortValue(ePropID.MOLEF, SourceEnum.Input, 100.0);

            PumpToValve.PortIn.SetPortValue(ePropID.P, SourceEnum.Input, 10);

            var pump = new Pump();
            flowsheet.Add(pump, "Pump");

            pump.Eff.SetPortValue(ePropID.NullUnits, SourceEnum.Input, 70);

            pump.PortIn.ConnectPorts(Feed.GetPortOut);
            pump.PortOut.ConnectPorts(PumpToValve.GetPortIn);

            flowsheet.PreSolve();

            PrintPortInfo(new List<StreamMaterial>() { Feed, PumpToValve });
            PrintPortInfo(pump);

            Debug.WriteLine(@"Finished Pump Valve Heater ++++++++++++++++++++++++++++++");
            flowsheet.CleanUp();
            thAdmin.CleanUp();
        }

        [TestMethod]
        public void testDensity()
        {
            Components o = new Components();
            Thermodata data = new Thermodata();
            o.thermo.Density = enumDensity.Rackett;
            BaseComp Butane = Thermodata.GetRealComponent("n-Butane");
            Butane.molefraction = 1;
            o.T = new Temperature(273.15 + 100);
            o.Add(Butane);
            o.UpdateComponentFractions(FlowFlag.Molar);

            Debug.Print(Thermodynamics.ActLiqDensity(o, o.T).ToString());

            o.thermo.Density = enumDensity.Costald;
            Debug.Print(Thermodynamics.ActLiqDensity(o, o.T).ToString());
        }

        [TestMethod]
        public void testPressureDrop()
        {
            PressureDrop();
        }

        public double PressureDrop(double Flowrate_m_s = 22.7, double Density_kg_m3 = 1000, double Viscosity_cp = 1,
            double NomPipeSize = 3, double pipschedule = 40, double Length_m = 36.6, double P_Barg = 1.724, double statichead = 1.5)
        {
            PipeFittings pf = new PipeFittings();
            pf.FittingTypes[eFittingsTypes.Elbows90LR.ToString()] = 1;
            pf.FittingTypes[eFittingsTypes.Elbows45LR.ToString()] = 1;
            pf.FittingTypes[eFittingsTypes.TeesThru.ToString()] = 1;
            pf.FittingTypes[eFittingsTypes.TeesBranch.ToString()] = 1;
            pf.FittingTypes[eFittingsTypes.GateValves.ToString()] = 1;
            pf.FittingTypes[eFittingsTypes.CheckValves.ToString()] = 1;
            pf.FittingTypes[eFittingsTypes.PipeEntrance.ToString()] = 1;

            double roughness = 25.4 * 0.0018;
            double MiscDP = 0;
            double EquipDP = 0.34474;

            int SizeIndex = pf.PipeIndex.IndexOf(NomPipeSize.ToString());
            int scheduleindex = pf.Schedule[pipschedule.ToString()];
            double inside_diamter_mm = pf.PipeInternalSizes[SizeIndex, scheduleindex] * 25.4;

            double Area = Math.PI * (inside_diamter_mm / 1000).Sqr() / 4;
            double Velocity = Flowrate_m_s / (Area * 3600);
            double Re = (inside_diamter_mm / 1000) * Velocity * Density_kg_m3 * 1000 / Viscosity_cp;
            double e_D = roughness / inside_diamter_mm;
            double A = Math.Pow((-2.457 * Math.Log(Math.Pow((7 / Re), 0.9) + 0.27 * e_D)), 16);
            double B = Math.Pow(37530 / Re, 16);
            double f = 8 * Math.Pow(Math.Pow(8 / Re, 12) + 1 / Math.Pow(A + B, 1.5), 1 / 12D);
            double DP1 = Density_kg_m3 * f * 100 * Velocity.Sqr() / ((inside_diamter_mm / 1000) * 2 * 100000);

            double fittigsloss = FittingsLoss(Re, inside_diamter_mm, pf) * Velocity.Sqr() * Density_kg_m3 / (2 * 100000);

            double TotalDP = P_Barg + statichead * (Density_kg_m3 * 9.81 / 100000) - EquipDP - MiscDP - fittigsloss - DP1 * Length_m / 100;
            return TotalDP;
        }

        public double FittingsLoss(double reynolds, double PipeID, PipeFittings pf)
        {
            double total = 0;

            foreach (var item in pf.FittingTypes)
            {
                switch (item.Key)
                {
                    case "Elbows90LR":
                        total += pf.K(reynolds, PipeID, new Elbows90LR()) * item.Value;
                        break;
                    case "Elbows45LR":
                        total += pf.K(reynolds, PipeID, new Elbows45LR()) * item.Value;
                        break;
                    case "TeesThru":
                        total += pf.K(reynolds, PipeID, new TeesThru()) * item.Value;
                        break;
                    case "TeesBranch":
                        total += pf.K(reynolds, PipeID, new TeesBranch()) * item.Value;
                        break;
                    case "BallValves":
                        total += pf.K(reynolds, PipeID, new BallValves()) * item.Value;
                        break;
                    case "ButterflyValves":
                        total += pf.K(reynolds, PipeID, new ButterflyValves()) * item.Value;
                        break;
                    case "GateValves":
                        total += pf.K(reynolds, PipeID, new GateValves()) * item.Value;
                        break;
                    case "GlobeValves":
                        total += pf.K(reynolds, PipeID, new GlobeValves()) * item.Value;
                        break;
                    case "CheckValves":
                        total += pf.K(reynolds, PipeID, new CheckValves()) * item.Value;
                        break;
                    case "PlugValves":
                        total += pf.K(reynolds, PipeID, new PlugValves()) * item.Value;
                        break;
                    case "PipeEntrance":
                        total += pf.K(reynolds, PipeID, new PipeEntrance()) * item.Value;
                        break;
                    default:
                        break;
                }
            }
            return total;
        }

        [TestMethod]
        public void Test_SUBFS_Serialisation()
        {
            SubFlowSheet sub = new SubFlowSheet();
            Port port = new Port();
            port.SetPortValue(ePropID.MOLEF, SourceEnum.CalcResult, 1);
            sub.Add(port);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, sub);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            SubFlowSheet obj = (SubFlowSheet)formatter.Deserialize(stream);
            stream.Close();
            PrintPortInfo(obj);
        }


        [TestMethod]
        public void Test_Flowsheet_Serialisation()
        {
            FlowSheet fs = new FlowSheet();
            Port port = new Port();
            port.SetPortValue(ePropID.MOLEF, SourceEnum.CalcResult, 1);
            fs.Add(port);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, fs);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            FlowSheet obj = (FlowSheet)formatter.Deserialize(stream);
            stream.Close();
            PrintPortInfo(obj);
        }

        [TestMethod]
        public void TestSerialisation()
        {
            Port_Material port = new Port_Material("Test");
            port.SetPortValue(ePropID.MOLEF, SourceEnum.CalcResult, 1);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, port);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            Port_Material obj = (Port_Material)formatter.Deserialize(stream);
            stream.Close();
            obj.Flash();
            PrintPortInfo(obj);
        }

        [TestMethod]
        public void Test_HX_Serialisation()
        {
            HeatExchanger2 port = new HeatExchanger2();
            //port.SetPortValueCheckConsistency(ePropID.MOLEF, SourceEnum.CalcResult, 1);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, port);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            HeatExchanger2 obj = (HeatExchanger2)formatter.Deserialize(stream);
            stream.Close();
        }


        [TestMethod]
        public void Test_PUMP_Serialisation()
        {
            Pump pump = new Pump();
            Port_Material port = new Port_Material();
            port.SetPortValue(ePropID.MOLEF, SourceEnum.CalcResult, 1);
            pump.ports.Add(port);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, pump);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            Pump obj = (Pump)formatter.Deserialize(stream);
            stream.Close();
            PrintPortInfo(obj);
        }

        [TestMethod]
        public void Test_Stream_Serialisation()
        {
            StreamMaterial str = new StreamMaterial();
            Port port = new Port();
            str.PortIn.SetPortValue(ePropID.MOLEF, SourceEnum.CalcResult, 1);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, str);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamMaterial obj = (StreamMaterial)formatter.Deserialize(stream);
            stream.Close();
            PrintPortInfo(obj);
        }

        [TestMethod]
        public void TestPortSerialisation()
        {
            Port_Material port = new Port_Material("TestPort");
            port.SetPortValue(ePropID.MF, SourceEnum.CalcResult, 1);

            PortList list = new PortList();
            list.Add(port);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, list);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            PortList obj = (PortList)formatter.Deserialize(stream);
            stream.Close();
            PrintPortInfo(obj);
        }


        [TestMethod]
        public void TestHeatEx()
        {
            Console.WriteLine(@"Init Heat Exchanger ++++++++++++++++++++++++++++++");
            var flowsheet = new FlowSheet();
            flowsheet.Name = "Main Flowsheet";
            //#Set Thermo
            var pkgName = "RK";
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };
            var thAdmin = new ThermoAdmin();
            var providers = thAdmin.GetAvThermoProviderNames();
            var provider = providers.ToList()[0];
            var thCase = "myTh";
            var thermo = thAdmin.AddPkgFromName(provider, thCase, pkgName);
            foreach (var i in cmpNames)
            {
                //thAdmin.AddCompound(provider, thCase, i);
                flowsheet.AddComponent(i);
            }
            // parent flowsheet

            flowsheet.SetThermoAdmin(thAdmin);
            flowsheet.SetThermo(thermo);
            var hotInlet = new StreamMaterial();
            flowsheet.Add(hotInlet, "HotIn");
            var coldInlet = new StreamMaterial();
            flowsheet.Add(coldInlet, "ColdIn");
            var hotOutlet = new StreamMaterial();
            flowsheet.Add(hotOutlet, "HotOut");
            var coldOutlet = new StreamMaterial();
            flowsheet.Add(coldOutlet, "ColdOut");
            var hotInPort = hotInlet.GetPort("In1");
            hotInPort.SetCompositionValue("PROPANE", 0, SourceEnum.Input);
            hotInPort.SetCompositionValue("n-BUTANE", 1, SourceEnum.Input);
            hotInPort.SetCompositionValue("i-BUTANE", 0, SourceEnum.Input);
            hotInPort.SetCompositionValue("n-PENTANE", 0, SourceEnum.Input);
            hotInPort.SetPortValue(ePropID.T, SourceEnum.Input, 273.15 + 25);
            hotInPort.SetPortValue(ePropID.P, SourceEnum.Input, 5.0);
            hotInPort.SetPortValue(ePropID.MOLEF, SourceEnum.Input, 100.0);
            var coldInPort = coldInlet.GetPort("In1");
            coldInPort.SetCompositionValue("PROPANE", 0.0, SourceEnum.Input);
            coldInPort.SetCompositionValue("n-BUTANE", 1, SourceEnum.Input);
            coldInPort.SetCompositionValue("i-BUTANE", 0.0, SourceEnum.Input);
            coldInPort.SetCompositionValue("n-PENTANE", 0.0, SourceEnum.Input);
            coldInPort.SetPortValue(ePropID.T, SourceEnum.Input, 273.15 + 0);
            coldInPort.SetPortValue(ePropID.P, SourceEnum.Input, 3.0);
            coldInPort.SetPortValue(ePropID.MOLEF, SourceEnum.Input, 1.0);

            var exch = new HeatExchanger2();
            flowsheet.Add(exch, "Exchanger");
            exch.coldSide.DP.SetPortValue(ePropID.DeltaP, SourceEnum.Input, 0.5);
            exch.hotSide.DP.SetPortValue(ePropID.DeltaP, SourceEnum.Input, 0.1);

            exch.CounterCurrent = true;

            exch.coldPortIn.ConnectPorts(coldInlet.GetPortOut);
            exch.hotPortIn.ConnectPorts(hotInlet.GetPortOut);
            exch.coldPortOut.ConnectPorts(coldOutlet.GetPortIn);
            exch.hotPortOut.ConnectPorts(hotOutlet.GetPortIn);

            /*exch.coldSide.DT.SetPortValueAddToSolveStack(ePropID.DeltaT, SourceEnum.Input, 5.0);
            flowsheet.Solve();
            PrintPortInfo(new List<Stream>() { hotInlet, hotOutlet, coldInlet, coldOutlet });
            PrintExchangerInfo(exch);
            return;

            exch.coldSide.DT.Clear();
            exch.coldSide.Q.SetValue(ePropID.ENERGY, 0.1815*2, SourceEnum.Input); // kW

            flowsheet.Solve();
            PrintPortInfo(new List<Stream>() { hotInlet, hotOutlet, coldInlet, coldOutlet });
            PrintExchangerInfo(exch);
            return;
            */

            exch.coldSide.Q.Clear();
            exch.UA.Forget();
            exch.UA.SetPortValue(ePropID.UA, SourceEnum.Input, 0.02, true);
            flowsheet.PreSolve();

            PrintPortInfo(new List<StreamMaterial>() { hotInlet, hotOutlet, coldInlet, coldOutlet });
            PrintExchangerInfo(exch);

            Debug.WriteLine(@"Finished Heat Exchanger ++++++++++++++++++++++++++++++");
            flowsheet.CleanUp();
            thAdmin.CleanUp();
        }


        [TestMethod]
        public void TestPump()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init Pump Test ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-C30" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var pump = new Pump();

            //fs.Thermo.Enthalpy = enumEnthalpy.PR78;

            Port_Material p = pump.Ports["In1"];
            p.MolarFlow = new StreamProperty(ePropID.MOLEF, SourceEnum.Input, 1);
            p.P = new StreamProperty(ePropID.P, SourceEnum.Input, 6);
            p.T = new StreamProperty(ePropID.T, SourceEnum.Input, 273.15 + 25);
            p.components.AddRange(fs.ComponentList);
            p.components.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });

            pump.Ports["Out1"].SetPortValue(ePropID.P, SourceEnum.Input, 10);
            pump.Eff.SetPortValue(ePropID.NullUnits, SourceEnum.Input, 70);

            pump.Solve();

            PrintPortInfo(pump);

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
        }

        [TestMethod]
        public void TestHeater()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init Pump Test ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-C30" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var heater = new Heater();

            //fs.Thermo.Enthalpy = enumEnthalpy.PR78;

            Port_Material p = (Port_Material)heater.Ports["In1"];
            p.MolarFlow = new StreamProperty(ePropID.MOLEF, SourceEnum.Input, 1);
            p.P = new StreamProperty(ePropID.P, SourceEnum.Input, 6);
            p.T = new StreamProperty(ePropID.T, SourceEnum.Input, 273.15 + 25);
            p.components.AddRange(fs.ComponentList);
            p.components.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });

            heater.Ports["Out1"].P.BaseValue = 5;
            heater.Ports["Out1"].T.BaseValue = 100;

            PortList Eports = heater.GetPorts(FlowDirection.IN);

            heater.Solve();

            foreach (var port in heater.Ports)
                PrintPortInfo(port, port.Name);

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
        }



        [TestMethod]
        public void TestObjectVSArray()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-C30" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var column = new Column();

            column.Thermo.Enthalpy = enumEnthalpy.PR78;

            column.TraySections.Add(new TraySection());
            column.MainTraySection.AddTrays(10);

            column.Specs.Add(new Specification("RR", 1, ePropID.NullUnits, eSpecType.RefluxRatio, column[0], column[0].Trays[0], column));
            column.Specs.Add(new Specification("Condenser T", 11 + 273.15, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));

            SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new Specification("Distillate", 0.5, ePropID.MOLEF, eSpecType.LiquidProductDraw, column[0], column[0].Trays[0], ss));

            Port_Material p = new Port_Material();
            p.MolarFlow = new StreamProperty(ePropID.MOLEF, SourceEnum.Input, 1);
            p.P = new StreamProperty(ePropID.P, SourceEnum.Input, 6);
            p.T = new StreamProperty(ePropID.T, SourceEnum.Input, 273.15 + 25);
            p.components.AddRange(fs.ComponentList);
            p.components.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });
            column.MainTraySection.Trays[4].feed = p;

            column.MainTraySection.TopTray.P = 6;
            column.MainTraySection.BottomTray.P = 6;
            column.MainTraySection.CondenserType = CondType.Partial;
            column.MainTraySection.ReboilerType = ReboilerType.Kettle;

            p.Flash();   //PrintPortInfo(p, "In1")
                         //Debug.Print(column.MainTraySection.CondenserType.ToString());

            var watch = Stopwatch.StartNew();
            double res = 0;
            int count = 1000;
            Tray tray = column[0].Trays[4];
            for (int i = 0; i < count; i++)
            {
                tray = column[0].Trays[4];
                tray.T = i;
                res = tray.T;
            }
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time1 ms: " + elapsedMs.ToString() + " " + res.ToString());
            //MessageBox.Show("Object Field " + elapsedMs.ToString());

            double res2 = 0;
            int count2 = count;


            double[][] tray1 = new double[count2][];
            for (int i = 0; i < count2; i++)
                tray1[i] = new double[count2];

            watch = Stopwatch.StartNew();
            for (int i = 0; i < count2; i++)
            {
                tray1[i][i] = i;
                res2 = tray1[i][i];
            }
            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time2 ms: " + elapsedMs.ToString() + " " + res2.ToString());
            //MessageBox.Show("Jagged Matix " + elapsedMs.ToString());

            double res3 = 0;
            int count3 = count;



            double[,] tray3 = new double[count3, count3];
            watch = Stopwatch.StartNew();
            for (int i = 0; i < count3; i++)
            {
                tray3[i, i] = i;
                res3 = tray3[i, i];
            }
            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time3 ms: " + elapsedMs.ToString() + " " + res3.ToString());
            //MessageBox.Show("Matix " + elapsedMs.ToString());
            //PrintPortInfo(p, "In1");

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
            //Assert.IsTrue(res);
        }
    }
}

