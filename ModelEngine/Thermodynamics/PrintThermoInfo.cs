using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Units;
using Units.UOM;
using static gv;

namespace ModelEngine
{
    public static class PrintInfo
    {
        public static void PrintColumnInfo(Column col, string Comment = "")
        {
            foreach (TraySection section in col.TraySections)
            {
                Debug.Print(section.RefluxRatio.ToString(), "RefluxRatio: ");
                PrintArrayInfo(section.L, "L:" + section.Name);
            }

            foreach (TraySection section in col.TraySections)
            {
                PrintArrayInfo(section.V, "V:" + section.Name);
            }

            foreach (TraySection section in col.TraySections)
            {
                for (int i = 0; i < section.Trays.Count; i++)
                {
                    Tray tray = section.Trays[i];
                    PrintOutPortInfo(tray.Ports, "Tray: " + tray.Name);
                }
            }
        }

        public static void PrintArrayInfo(double[] p, string Comment = "")
        {
            Debug.Print(Comment);
            for (int i = 0; i < p.Length; i++)
            {
                double item = p[i];
                Debug.Print(i.ToString() + " " + item.ToString());
            }
        }

        public static void PrintPortInfo(double[] p, string Comment = "")
        {
            foreach (var item in p)
            {
                Debug.Print(item.ToString());
            }
        }

        public static void PrintPortInfo(double[,] p, string Comment = "")
        {
            for (int i = 0; i <= p.GetUpperBound(0); i++)
            {
                for (int y = 0; y <= p.GetUpperBound(1); y++)
                {
                    Debug.Print(p[i, y].ToString() + ", ");
                }
                Debug.WriteLine("");
            }
        }

        public static void PrintPortInfo(Tuple<double, double> p, string Comment = "")
        {
            Debug.Print(p.Item1.ToString());
            Debug.Print(p.Item2.ToString());
        }

        public static void PrintPortInfo(PortList p, string Comment = "")
        {
            foreach (var item in p)
            {
                PrintPortInfo(item);
            }
        }

        public static void PrintOutPortInfo(PortList p, string Comment = "")
        {
            Debug.Print(Comment);
            foreach (var item in p)
            {
                if (!item.IsFlowIn && item.IsSolved)
                    PrintPortInfo(item);
            }
        }

        public static void PrintPortInfo(List<StreamMaterial> p, string Comment = "")
        {
            foreach (var s in p)
            {
                var port = s.GetPort(OUT_PORT);
                var comp = port.cc;
                Debug.WriteLine("Stream" + s.Name);
                Debug.WriteLine(T_VAR + ": " + port.T_);
                Debug.WriteLine(P_VAR + ": " + port.P_);
                Debug.WriteLine(H_VAR + ": " + port.H_);
                Debug.WriteLine(VPFRAC_VAR + ": " + port.Q_);
                Debug.WriteLine(MOLEFLOW_VAR + ": " + port.MolarFlow_);
                Debug.WriteLine("");
                Debug.WriteLine("Composition");
                foreach (var j in Enumerable.Range(0, comp.Count))
                    Debug.WriteLine("fraction of " + comp[j].Name + ": " + comp[j].MoleFraction);

                Debug.WriteLine("");
            }
        }

        public static void PrintExchangerInfo(HeatExchanger2 uo)
        {
            PrintPortInfo(uo.heater.PortIn);
            PrintPortInfo(uo.heater.PortOut);
            PrintPortInfo(uo.cooler.PortIn);
            PrintPortInfo(uo.cooler.PortOut);
            Debug.WriteLine("UA " + uo.UA.Value.DefaultUnit + ": " + uo.UA);
            Debug.WriteLine("LMTD " + uo.LMTD.Value.DefaultUnit + ": " + uo.LMTD);
            Debug.WriteLine("Duty " + uo.Q.Value.DefaultUnit + ": " + uo.Q);
            Debug.WriteLine("HS DT " + uo.deltaTShellSide.Value.DefaultUnit + ": " + uo.deltaTShellSide);
            Debug.WriteLine("CS DT " + uo.deltaTTubeSide.Value.DefaultUnit + ": " + uo.deltaTTubeSide);
            Debug.WriteLine("HS Approach " + uo.deltaTApproach.Value.DefaultUnit + ": " + uo.deltaTApproach);
        }

        public static void PrintPortInfo(UnitOperation uo)
        {
            //Debug.Print("");
            //Debug.Print("Unit " + uo.Name);
            foreach (Port port in uo.Ports)
            {
                PrintPortInfo(port);
            }
        }

        public static void PrintPortInfo(Port p, string Comment = "")
        {
            Debug.WriteLine("");
            //Print some info out
            switch (p)
            {
                case Port_Signal s:
                    Debug.WriteLine("Port \"" + s.Name + "\":" + s.Value);
                    break;

                case Port_Energy e:
                    Debug.WriteLine("Port \"" + e.Name + "\":" + e.Value);
                    break;

                case Port_Material pm:
                    var comp = pm.cc;
                    Debug.WriteLine("Port \"" + p.Name + "\":");
                    for (int j = 0; j < comp.Count; j++)
                        Console.WriteLine("fraction of " + comp.ComponentList[j].Name + ": " + comp.ComponentList[j].MoleFraction.ToString());

                    Debug.WriteLine("Properties of \"" + p.Name + "\":" + Comment);
                    Debug.WriteLine(ePropID.T + ": " + ((Temperature)pm.Properties[ePropID.T].UOM).Celsius.ToString());
                    Debug.WriteLine(ePropID.P + ": " + pm.Properties[ePropID.P]);
                    Debug.WriteLine(ePropID.H + ": " + pm.Properties[ePropID.H]);
                    Debug.WriteLine(ePropID.S + ": " + pm.Properties[ePropID.S]);
                    Debug.WriteLine(ePropID.Q + ": " + pm.Properties[ePropID.Q]);
                    Debug.WriteLine(ePropID.MOLEF + ": " + pm.Properties[ePropID.MOLEF]);
                    //Debug.WriteLine(ePropID.EnergyFlow + ": " + pm.Properties[ePropID.EnergyFlow]);
                    break;
            }
        }
    }
}