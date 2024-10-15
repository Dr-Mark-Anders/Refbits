using ModelEngine;
using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Units;
using Units.UOM;
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
            FlowSheet flowsheet = new();
            flowsheet.Name = "Main Flowsheet";

            var cmpNames = new List<string>() { "n-BUTANE" };
            var thAdmin = new ThermoAdmin();

            foreach (var i in cmpNames)
                flowsheet.AddComponent(i);

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
            feedport.SetPortValue(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feedport.SetPortValue(ePropID.P, 5.0, SourceEnum.Input);
            feedport.SetPortValue(ePropID.MOLEF, 100.0, SourceEnum.Input);

            PumpToValve.Port.SetPortValue(ePropID.P, 10, SourceEnum.Input);
            ValveToHeater.Port.SetPortValue(ePropID.P, 5, SourceEnum.Input);
            heaterOutlet.Port.SetPortValue(ePropID.T, 100 + 273.15, SourceEnum.Input);

            var pump = new Pump();
            flowsheet.Add(pump, "Pump");

            pump.Eff.SetPortValue(ePropID.NullUnits, 70, SourceEnum.Input);

            pump.PortIn.ConnectPort(Feed);
            pump.PortOut.ConnectPort(PumpToValve);

            var valve = new Valve();
            flowsheet.Add(valve, "valve");
            valve.PortIn.ConnectPort(PumpToValve);
            valve.PortOut.ConnectPort(ValveToHeater);

            Heater heater = new();
            heater.DP.SetPortValue(ePropID.DeltaP, 0, SourceEnum.Input);
            flowsheet.Add(heater, "Heater");

            heater.PortIn.ConnectPort(ValveToHeater);
            heater.PortOut.ConnectPort(heaterOutlet);

            flowsheet.PreSolve();

            PrintPortInfo(new List<StreamMaterial>() { Feed, ValveToHeater, PumpToValve, heaterOutlet });
            PrintInfo.PrintPortInfo(pump);
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
            FlowSheet flowsheet = new();
            flowsheet.Name = "Main Flowsheet";
            //#Set Thermo

            var cmpNames = new List<string>() { "n-BUTANE" };

            foreach (var i in cmpNames)
                flowsheet.AddComponent(i);

            var Feed = new StreamMaterial();
            flowsheet.Add(Feed, "Feed");

            var PumpToValve = new StreamMaterial();
            flowsheet.Add(PumpToValve, "PumpToValve");

            var feedport = Feed.GetPort("In1");
            feedport.SetCompositionValue("n-BUTANE", 1, SourceEnum.Input);
            feedport.SetPortValue(ePropID.T, 273.15 + 25, SourceEnum.Input);
            feedport.SetPortValue(ePropID.P, 5.0, SourceEnum.Input);
            feedport.SetPortValue(ePropID.MOLEF, 100.0, SourceEnum.Input);

            PumpToValve.Port.SetPortValue(ePropID.P, 10, SourceEnum.Input);

            var pump = new Pump();
            flowsheet.Add(pump, "Pump");

            pump.Eff.SetPortValue(ePropID.NullUnits, 70, SourceEnum.Input);

            pump.PortIn.ConnectPort(Feed);
            pump.PortOut.ConnectPort(PumpToValve);

            flowsheet.PreSolve();

            PrintPortInfo(new List<StreamMaterial>() { Feed, PumpToValve });
            PrintPortInfo(pump);

            Debug.WriteLine(@"Finished Pump Valve Heater ++++++++++++++++++++++++++++++");
            flowsheet.CleanUp();
        }

        [TestMethod]
        public void testDensity()
        {
            Components cc = new();

            cc.Thermo.Density = enumDensity.Rackett;
            BaseComp Butane = Thermodata.GetComponent("n-Butane");
            Butane.MoleFraction = 1;
            
            cc.Add(Butane);
            cc.NormaliseFractions(FlowFlag.Molar);

            Debug.Print(ThermodynamicsClass.ActLiqDensity(cc, new Temperature(273.15 + 100)).ToString());

            cc.Thermo.Density = enumDensity.Costald;
            Debug.Print(ThermodynamicsClass.ActLiqDensity(cc, new Temperature(273.15 + 100)).ToString());
        }

        [TestMethod]
        public void testPressureDrop()
        {
            PressureDrop();
        }

        public static double PressureDrop(double Flowrate_m_s = 22.7, double Density_kg_m3 = 1000, double Viscosity_cp = 1,
            double NomPipeSize = 3, double pipschedule = 40, double Length_m = 36.6, double P_Barg = 1.724, double statichead = 1.5)
        {
            PipeFittings pf = new();
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
            double inside_diamter_mm = pf.PipeinternalSizes[SizeIndex, scheduleindex] * 25.4;

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

        public static double FittingsLoss(double reynolds, double PipeID, PipeFittings pf)
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
            HXSubFlowSheet sub = new();
            Port port = new();
            port.SetPortValue(ePropID.MOLEF, 1, SourceEnum.UnitOpCalcResult);
            sub.Add(port);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, sub);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            HXSubFlowSheet obj = (HXSubFlowSheet)formatter.Deserialize(stream);
            stream.Close();
            PrintPortInfo(obj);
        }

        [TestMethod]
        public void Test_Flowsheet_Serialisation()
        {
            FlowSheet fs = new();
            Port port = new();
            port.SetPortValue(ePropID.MOLEF, 1, SourceEnum.UnitOpCalcResult);
            fs.Add(port);

            IFormatter formatter = new BinaryFormatter();
            System.IO.Stream stream = new FileStream("C:\\MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, fs);
            stream.Close();

            formatter = new BinaryFormatter();
            stream = new FileStream("C:\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            FlowSheet? obj = formatter.Deserialize(stream) as FlowSheet;
            stream.Close();
            PrintPortInfo(obj);
        }

        [TestMethod]
        public void TestSerialisation()
        {
            Port_Material port = new("Test");
            port.SetPortValue(ePropID.MOLEF, 1, SourceEnum.UnitOpCalcResult);

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
            HeatExchanger2 port = new();
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
            Pump pump = new();
            Port_Material port = new();
            port.SetPortValue(ePropID.MOLEF, 1, SourceEnum.UnitOpCalcResult);
            pump.Ports.Add(port);

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
            StreamMaterial str = new();
            str.Port.SetPortValue(ePropID.MOLEF, 1, SourceEnum.UnitOpCalcResult);

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
            Port_Material port = new("TestPort");
            port.SetPortValue(ePropID.MF, 1, SourceEnum.UnitOpCalcResult);

            PortList list = new();
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
            FlowSheet flowsheet = new();
            flowsheet.Name = "Main Flowsheet";
            //#Set Thermo
            var cmpNames = new List<string>() { "PROPANE", "n-BUTANE", "i-BUTANE", "n-PENTANE" };

            foreach (var i in cmpNames)
            {
                //thAdmin.AddCompound(provider, thcase , i);
                flowsheet.AddComponent(i);
            }
            // parent flowsheet

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
            hotInPort.SetPortValue(ePropID.T, 273.15 + 25, SourceEnum.Input);
            hotInPort.SetPortValue(ePropID.P, 5.0, SourceEnum.Input);
            hotInPort.SetPortValue(ePropID.MOLEF, 100.0, SourceEnum.Input);
            var coldInPort = coldInlet.GetPort("In1");
            coldInPort.SetCompositionValue("PROPANE", 0.0, SourceEnum.Input);
            coldInPort.SetCompositionValue("n-BUTANE", 1, SourceEnum.Input);
            coldInPort.SetCompositionValue("i-BUTANE", 0.0, SourceEnum.Input);
            coldInPort.SetCompositionValue("n-PENTANE", 0.0, SourceEnum.Input);
            coldInPort.SetPortValue(ePropID.T, 273.15 + 0, SourceEnum.Input);
            coldInPort.SetPortValue(ePropID.P, 3.0, SourceEnum.Input);
            coldInPort.SetPortValue(ePropID.MOLEF, 1.0, SourceEnum.Input);

            var exch = new HeatExchanger2();
            flowsheet.Add(exch, "Exchanger");
            exch.heater.DP.SetPortValue(ePropID.DeltaP, 0.5, SourceEnum.Input);
            exch.cooler.DP.SetPortValue(ePropID.DeltaP, 0.1, SourceEnum.Input);

            exch.CounterCurrent = true;

            exch.heater.PortIn.ConnectPort(coldInlet);
            exch.cooler.PortIn.ConnectPort(hotInlet);
            exch.heater.PortOut.ConnectPort(coldOutlet);
            exch.cooler.PortOut.ConnectPort(hotOutlet);

            /*exch.coldSide.DT.SetPortValueAddToSolveStack(ePropID.DeltaT, SourceEnum.Input, 5.0);
            flowsheet.Solve();
            PrintPortInfo(new  List<Stream>() { hotInlet, hotOutlet, coldInlet, coldOutlet });
            Print ExchangerInfo(exch);
            return  ;

            exch.coldSide.DT.Clear();
            exch.coldSide.Q.SetValue(ePropID.ENERGY, 0.1815*2, SourceEnum.Input); // kW

            flowsheet.Solve();
            PrintPortInfo(new  List<Stream>() { hotInlet, hotOutlet, coldInlet, coldOutlet });
            Print ExchangerInfo(exch);
            return  ;
            */

            exch.heater.Q.ClearConnections();
            exch.UA.Clear();
            exch.UA.SetPortValue(ePropID.UA, 0.02, SourceEnum.Input);
            flowsheet.PreSolve();

            PrintPortInfo(new List<StreamMaterial>() { hotInlet, hotOutlet, coldInlet, coldOutlet });
            PrintExchangerInfo(exch);

            Debug.WriteLine(@"Finished Heat Exchanger ++++++++++++++++++++++++++++++");
            flowsheet.CleanUp();
        }

        [TestMethod]
        public void TestPump()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init Pump Test ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-C30" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var pump = new Pump();

            //fs.Thermo.Enthalpy = enumEnthalpy.PR78;

            Port_Material p = pump.Ports["In1"];
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            p.cc.Add(fs.ComponentList);
            p.cc.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });

            pump.Ports["Out1"].SetPortValue(ePropID.P, 10, SourceEnum.Input);
            pump.Eff.SetPortValue(ePropID.NullUnits, 70, SourceEnum.Input);

            pump.Solve();

            PrintPortInfo(pump);

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
        }

        [TestMethod]
        public void TestHeater()
        {
            FlowSheet fs = new();

            Console.WriteLine("Init Pump Test ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "n-butane", "n-C30" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            var heater = new Heater();

            //fs.Thermo.Enthalpy = enumEnthalpy.PR78;

            Port_Material p = (Port_Material)heater.PortIn;
            p.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            p.P_ = new StreamProperty(ePropID.P, 6, SourceEnum.Input);
            p.T_ = new StreamProperty(ePropID.T, 273.15 + 25, SourceEnum.Input);
            p.cc.Add(fs.ComponentList);
            p.cc.SetMolFractions(new double[] { 0.5, 0.4, 0.1 });

            heater.PortOut.P_.BaseValue = 5;
            heater.PortOut.T_.BaseValue = 100;

            PortList Eports = heater.GetPorts(FlowDirection.IN);

            heater.Solve();

            foreach (var port in heater.Ports)
                PrintPortInfo(port, port.Name);

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
        }

        [TestMethod]
        public void TestobjectVSArray()
        {
            FlowSheet fs = new();

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

            p.Flash();

            var watch = Stopwatch.StartNew();
            double res = 0;
            int count = 1000;
            for (int i = 0; i < count; i++)
            {
                Tray tray = column[0].Trays[4];
                tray.T = i;
                res = tray.T;
            }
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Column Solution Time1 ms: " + elapsedMs.ToString() + " " + res.ToString());
            //MessageBox.Show("object  Field " + elapsedMs.ToString());

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