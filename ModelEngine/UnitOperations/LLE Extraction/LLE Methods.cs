﻿using System.Collections.Generic;
using System.Windows.Forms;
using Units.UOM;

namespace ModelEngine
{
    public partial class LLESEP
    {
        public double TempDampfactor = 1;
        public string ErrHistory1;
        public string ErrHistory2;

        public bool InterpolatePressureAllSection()
        {
            TraySection section = TraySections[0];

            for (int i = 0; i < section.Trays.Count; i++)
            {
                if (!section.Trays[i].P.IsInput)
                    section.Trays[i].P.Clear();
            }

            if (section.TopTray.P.origin != SourceEnum.Input)
            {
                MessageBox.Show("Pressure Is not Set in Column Top");
                return false;
            }

            if (section.BottomTray.P.origin != SourceEnum.Input)
            {
                MessageBox.Show("Pressure Is not Set in Column Bottom");
                return false;
            }

            interpolatePressure(section);

            foreach (ConnectingStream cd in connectingDraws)
            {
                Tray EndTray = cd.engineReturnTray;
                Tray DrawTray = cd.engineDrawTray;

                if (DrawTray.P.IsKnown)
                {
                    EndTray.P.BaseValue = DrawTray.P.BaseValue;
                    EndTray.P.origin = SourceEnum.Transferred;
                }
            }

            for (int i = 1; i < TraySections.Count; i++)
            {
                interpolatePressure(TraySections[i]);
            }

            return true;
        }

        public static bool interpolatePressure(TraySection section)
        {
            List<int> FixedTrayLocs = new();
            List<double> DPs = new();

            for (int i = 0; i < section.Trays.Count; i++)
            {
                Tray t = section.Trays[i];
                if (t.P.IsInput)
                    FixedTrayLocs.Add(i);
            }

            if (section.ovhdDP.IsInput)
            {
                Tray t = section.Trays[1];
                t.P.BaseValue = section.TopTray.P + section.ovhdDP.BaseValue;
                t.P.origin = SourceEnum.UnitOpCalcResult;
                FixedTrayLocs.Add(1);
            }

            FixedTrayLocs.Sort();

            for (int i = 1; i < FixedTrayLocs.Count; i++)
            {
                DPs.Add((section.Trays[FixedTrayLocs[i]].P - section.Trays[FixedTrayLocs[i - 1]].P)
                    / (FixedTrayLocs[i] - FixedTrayLocs[i - 1]));
            }

            if (FixedTrayLocs.Count == 1) // only one value know set all values equal
            {
                Pressure P = section.Trays[FixedTrayLocs[0]].P.BaseValue;

                for (int i = 0; i < section.Trays.Count; i++)
                    if (!section.Trays[i].P.IsKnown)
                    {
                        section.Trays[i].P.BaseValue = P.BaseValue;
                        section.Trays[i].P.origin = SourceEnum.UnitOpCalcResult;
                    }
            }
            else  // Top and bottom should be known
            {
                for (int no = 1; no < FixedTrayLocs.Count; no++)
                    for (int i = FixedTrayLocs[no - 1]; i < FixedTrayLocs[no]; i++)
                        if (section.Trays[i].P.origin != SourceEnum.Input && section.Trays[i].P.origin != SourceEnum.UnitOpCalcResult)
                        {
                            section.Trays[i].P.BaseValue = section.Trays[i - 1].P + DPs[no - 1];
                            section.Trays[i].P.origin = SourceEnum.UnitOpCalcResult;
                        }
            }

            if (!section.ovhdDP.IsInput)
            {
                section.ovhdDP.BaseValue = section.Trays[1].P.BaseValue - section.TopTray.P;
                section.ovhdDP.origin = SourceEnum.UnitOpCalcResult;
            }
            return true;
        }

        public bool IsReset { get; set; } = true;

        public PumpAroundCollection PumpAroundsActive()
        {
            PumpAroundCollection pas = new PumpAroundCollection();
            foreach (PumpAround pa in pumpArounds)
            {
                if (pa.IsActive)
                    pas.Add(pa);
            }
            return pas;
        }

        public bool ValidateDesign()
        {
            return true;
        }

        public PumpAroundCollection GetPumpArounds()
        {
            PumpAroundCollection PAS = this.PumpAroundsActive();
            return PAS;
        }
    }
}