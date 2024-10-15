using System;

/*  Dimensioned numbers should store Value in default units
 *  Conversion factors
 *  and should convert value to required units only on demand (e.g. at int efaces or for code in non-standard units.
 *  File storage should be in default units as 'double '
 *
 * Pressure   - bar a
 * Temperature  C
 * Flow kg/s
 * Vol flow m3/hr
 *
 */

namespace Units
{
    public delegate double UnitDelegate(double X, string CUNIT, bool ToDefault, ref string[] Units, object o);

    // <summary>
    /// Unit Convert class
    /// </summary>
    ///
    public class UnitConv
    {
        public static UnitDelegate MassFlowDelegate = new UnitDelegate(MassFlow);
        public static UnitDelegate DeltaTDelegate = new UnitDelegate(DeltaTemp);
        public static UnitDelegate DeltaPDelegate = new UnitDelegate(DeltaPressure);
        public static UnitDelegate EnergyDelegate = new UnitDelegate(Energy);
        public static UnitDelegate LiquidVolumeFlowDelegate = new UnitDelegate(LiqVolFlow);
        public static UnitDelegate MassDelegate = new UnitDelegate(Mass);
        public static UnitDelegate MolarFlowDelegate = new UnitDelegate(MolarFlow);
        public static UnitDelegate PressureDelegate = new UnitDelegate(Pressure);
        public static UnitDelegate TemperatureDelegate = new UnitDelegate(Temperature);
        public static UnitDelegate VolumeSpecificEnergyDelegate = new UnitDelegate(VolSpecEnergy);
        public static UnitDelegate EnergyFlowDelegate = new UnitDelegate(EnergyFlow);
        public static UnitDelegate NullUnitDelegate = new UnitDelegate(NullUnits);
        public static UnitDelegate EntropyDelegate = new UnitDelegate(Entropy);
        public static UnitDelegate MolarEntropyDelegate = new UnitDelegate(MolarEntropy);
        public static UnitDelegate EnthalpyDelegate = new UnitDelegate(Enthalpy);
        public static UnitDelegate MolarEnthalpyDelegate = new UnitDelegate(MolarEnthalpy);
        public static UnitDelegate SpecificVolumeDelegate = new UnitDelegate(SpecificVolume);
        public static UnitDelegate EnergyPriceDelegate = new UnitDelegate(EnergyPrice);
        public static UnitDelegate MassPriceDelegate = new UnitDelegate(MassPrice);
        public static UnitDelegate LHVDelegate = new UnitDelegate(LHV);
        public static UnitDelegate AreaDelgate = new UnitDelegate(Area);
        public static UnitDelegate HeatTransferResistaceDelegate = new UnitDelegate(HeatTransferResistace);
        public static UnitDelegate LengthDelegate = new UnitDelegate(Length);
        public static UnitDelegate HeatFluxDelegate = new UnitDelegate(HeatFlux);
        public static UnitDelegate OverallUADelegate = new UnitDelegate(OverallUA);
        public static UnitDelegate PercentageDelegate = new UnitDelegate(Percentage);
        public static UnitDelegate VelocityDelegate = new UnitDelegate(Velocity);

        private static double Velocity(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "m/s" };

            temp = X;
            switch (CUNIT)
            {
                case "m/s":
                    temp = X;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        private static double Percentage(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "%" };

            temp = X;
            switch (CUNIT)
            {
                case "%":
                    temp = X;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        private static double OverallUA(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kJ/hr/C", "kW/C" };

            temp = X;
            switch (CUNIT)
            {
                case "kW/C":
                    temp = X / 3600;
                    break;

                case "kJ/hr/C":
                    temp = X;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        private static double HeatFlux(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "W/m2" };

            temp = X;
            switch (CUNIT)
            {
                case "W/m2":
                    temp = X;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        private static double Length(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "m", "cm", "mm", "ft", "inches" };

            temp = X;
            switch (CUNIT)
            {
                case "m":
                    temp = X;
                    break;

                case "cm":
                    temp = X * 100;
                    break;

                case "mm":
                    temp = X * 1000;
                    break;

                case "ft":
                    temp = X * 100 / 2.54 / 12;
                    break;

                case "inches":
                    temp = X * 100 / 2.54;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        private enum LengthUnits
        { m, cm, mm, ft, inches }

        private static double Length(double X, LengthUnits CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "m", "cm", "ft", "inches" };

            temp = X;
            switch (CUNIT)
            {
                case LengthUnits.m:
                    temp = X;
                    break;

                case LengthUnits.cm:
                    temp = X * 100;
                    break;

                case LengthUnits.mm:
                    temp = X * 100;
                    break;

                case LengthUnits.ft:
                    temp = X / 2.54 / 12;
                    break;

                case LengthUnits.inches:
                    temp = X / 2.54;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        private static double HeatTransferResistace(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "Km/W" };

            temp = X;
            switch (CUNIT)
            {
                case "Km/W":
                    temp = X;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        public static UnitDelegate GetPropertyDelegate(ePropID name)
        {
            switch (name)
            {
                case ePropID.DeltaT: { return DeltaTDelegate; }
                case ePropID.DeltaP: { return DeltaPDelegate; }
                case ePropID.EnergyFlow: { return EnergyDelegate; }
                case ePropID.LiquidVolumeFlow: { return LiquidVolumeFlowDelegate; }
                case ePropID.Mass: { return MassDelegate; }
                case ePropID.MF: { return MassFlowDelegate; }
                case ePropID.MOLEF: { return MolarFlowDelegate; }
                case ePropID.P: { return PressureDelegate; }
                case ePropID.T: { return TemperatureDelegate; }
                case ePropID.VolumeSpecificEnergy: { return VolumeSpecificEnergyDelegate; }
                case ePropID.NullUnits: { return NullUnitDelegate; }
                case ePropID.H: { return MolarEnthalpyDelegate; }
                case ePropID.S: { return MolarEntropyDelegate; }
                case ePropID.EnergyPrice: { return EnergyPriceDelegate; }
                case ePropID.MassPrice: { return MassPriceDelegate; }
                case ePropID.LHV: { return LHVDelegate; }
                case ePropID.SpecificVolume: { return SpecificVolumeDelegate; }
                case ePropID.SpecificEnergy: { return VolumeSpecificEnergyDelegate; }
                case ePropID.Length: { return LengthDelegate; }
                case ePropID.HeatFlux: { return HeatFluxDelegate; }
                case ePropID.HeatTransferResistace: { return HeatTransferResistaceDelegate; };
                case ePropID.Percentage: { return PercentageDelegate; };
                case ePropID.Velocity: { return VelocityDelegate; };
                case ePropID.Area: { return AreaDelgate; };
                default:
                    {
                        //MessageBox.Show("ConvFuncNotFound");
                        break;
                    }
            }
            return NullUnitDelegate;
        }

        public static double VolSpecEnergy(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp;

            Units = new string[] { "kJ/m3", "MJ/m3", "M Kcal/m3", "M Btu/US gall", "M Btu/IMP gall", "M Btu/Bbl" }; ;

            temp = X;
            switch (CUNIT)
            {
                case "kJ/m3":
                    temp = X * 4186.8 * 1000;
                    break;

                case "MJ/m3":
                    temp = X * 4186.8;
                    break;

                case "M Btu/US gall":
                    temp = X / 66.5784;
                    break;

                case "M Kcal/m3":
                    temp = X;
                    break;

                case "M Btu/IMP gall":
                    temp = X / 55.431318;
                    break;

                case "M Btu / Bbl":
                    temp = X / 1.585006;
                    break;

                default:
                    break;
            }

            if (ToDefault)
            {
                temp = X * X / temp;
            }
            return temp;
        }

        public static double DeltaTemp(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "C", "F", "K", "R" };

            temp = X;
            switch (CUNIT)
            {
                case "R":
                    temp = (X + 273.15) / 1.8;
                    break;

                case "K":
                    temp = (X + 273.15);
                    break;

                case "F":
                    temp = X / 1.8;
                    break;

                case "C":
                    temp = X;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        public static double DeltaPressure(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kg/cm2", "psi", "atm", "KPa", "bar", "mmHg", "Pa" };

            temp = X;
            switch (CUNIT)
            {
                case "mmHg":
                    temp = X * 760 / 1.033;
                    break;

                case "bar":
                    temp = X / 1.02;
                    break;

                case "KPa":
                    temp = X / 0.0102;
                    break;

                case "atm":
                    temp = X / 1.033;
                    break;

                case "psi":
                    temp = X * 14.22;
                    break;

                case "kg/cm2":
                    temp = X;
                    break;

                case "Pa":
                    temp = X * 98066.5; // 0.000102;
                    break;

                default:
                    break;
            }
            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        public static double NullUnits(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            Units = new string[] { "Null" };
            return X;
        }

        public static double LiqVolFlow(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "m3/hr", "cu ft/hr", "Bbl/day", "US gall/hr", "K m3/a", "K Bbl/day", "Bbl/hr", "m3/day", "cu ft/day", "IMP gall/hr", "m3/s" }; ;

            temp = X;
            switch (CUNIT)
            {
                case "Bbl/hr":
                    temp = X * 6.2905;
                    break;

                case "K Bbl/day":
                    temp = X * 6.2905 * 24 * 0.001;
                    break;

                case "K m3/a":
                    temp = X * 8000 * 0.001;
                    break;

                case "US gall/hr":
                    temp = X * 264.2;
                    break;

                case "Bbl/day":
                    temp = X * 6.2905 * 24;
                    break;

                case "cu ft/hr":
                    temp = X / 0.02831685;
                    break;

                case "m3/hr":
                    temp = X;
                    break;

                case "m3/s":
                    temp = X / 3600;
                    break;

                case "m3/day":
                    temp = X * 23.999808;
                    break;

                case "cu ft/day":
                    temp = X * 847.4576271;
                    break;

                case "IMP gall/hr":
                    temp = X * 219.9736032;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }
            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public static double Mass(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kg", "g", "te", "lb", "klb", "short ton", "long ton", "carat", "amu", "assarion", "bekah", "centigram", "dalton", "decigram", "dekagram", "denarius", "didrachma", "drachma", "dyne", "gerah", "grain", "hectogram", "hundredweight (US)", "hundredweight (UK)", "kip", "lepton", "mina (Bib Greek)", "mina (Bib Hebrew)", "pennyweight", "poundal", "quint al", "scruple", "shekel", "slug", "stone (US)", "stone (UK)", "troy ounce" }; ;

            switch (CUNIT)
            {
                case "g":
                    temp = X;
                    break;

                case "lb":
                    temp = X * 453.5923;
                    break;

                case "short ton":
                    temp = X * 453.5923 * 2000;
                    break;

                case "long ton":
                    temp = X * 2240 * 453.5923;
                    break;

                case "klb":
                    temp = X * 2240 * 1000;
                    break;

                case "te":
                    temp = X * 1000000;
                    break;

                case "kg":
                    temp = X * 1000;
                    break;

                case "carat":
                    temp = X * 0.2;
                    break;

                case "amu":
                    temp = X * 1.66054E-24;
                    break;

                case "assarion":
                    temp = X / 4.155844156;
                    break;

                case "bekah":
                    temp = X / 0.1754385965;
                    break;

                case "centigram":
                    temp = X / 100;
                    break;

                case "dalton":
                    temp = X / 6.022173643E+23;
                    break;

                case "decigram":
                    temp = X / 10;
                    break;

                case "dekagram":
                    temp = X * 10;
                    break;

                case "denarius":
                    temp = X / 0.2597402597;
                    break;

                case "didrachma":
                    temp = X / 0.1470588235;
                    break;

                case "drachma":
                    temp = X / 0.2941176471;
                    break;

                case "dyne":
                    temp = X / 980.665;
                    break;

                case "gerah":
                    temp = X / 1.754385965;
                    break;

                case "grain":
                    temp = X / 15.43235835;
                    break;

                case "hectogram":
                    temp = X / 0.01;
                    break;

                case "hundredweight (US)":
                    temp = X / 0.00002204622622;
                    break;

                case "hundredweight (UK)":
                    temp = X / 0.00001968413055;
                    break;

                case "kip":
                    temp = X / 0.000002204622622;
                    break;

                case "lepton":
                    temp = X / 33.24675325;
                    break;

                case "mina (Bib Greek)":
                    temp = X / 0.002941176471;
                    break;

                case "mina (Bib Hebrew)":
                    temp = X / 0.001754385965;
                    break;

                case "pennyweight":
                    temp = X / 0.6430149314;
                    break;

                case "poundal":
                    temp = X / 0.07098884842;
                    break;

                case "quint al":
                    temp = X / 0.00001;
                    break;

                case "scruple":
                    temp = X / 0.7716179176;
                    break;

                case "shekel":
                    temp = X / 0.08771929825;
                    break;

                case "slug":
                    temp = X / 0.00006852176586;
                    break;

                case "stone (US)":
                    temp = X / 0.0001763698097;
                    break;

                case "stone (UK)":
                    temp = X / 0.0001574730444;
                    break;

                case "troy ounce":
                    temp = X / 0.03215074843;
                    break;

                default:
                    break;
            }
            if (X == 0)
            {
                return 0;
            }

            if (!ToDefault)
                temp = X * X / temp;

            return temp;
        }

        public static double Pressure(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double F2, F3, F4, F5, temp = 0;

            Units = new string[] { "kg/cm2", "kg/cm2 g", "psia", "psig", "atm a", "atm g", "kPa", "kPa g", "bar a", "bar g", "mmHg", "mmHg g" }; ;

            F2 = 101.325 / 98.0665; //st atm (kN/m2)/ 1 atm (1 kgf/cm2) exact
            F3 = 98.0665 / (0.45359237 * 9.80665 / (1 / (1000000 / 645.16)) / 1000);
            F4 = 1 / 98.0665;
            F5 = 100 / 98.0665;  // bar exact

            temp = X;
            if (ToDefault)
            {
                switch (CUNIT)
                {
                    case "mmHg g":
                        temp = (X + 760) / 760 * F2;
                        break;

                    case "mmHg":
                        temp = X / 760 * F2;
                        break;

                    case "bar g":
                        temp = X * F5 + F2;
                        break;

                    case "bar a":
                        temp = X * F5;
                        break;

                    case "kPa g":
                        temp = X * F4 + F2;
                        break;

                    case "kPa":
                        temp = X * F4;
                        break;

                    case "atm g":
                        temp = X * F2 + F2;
                        break;

                    case "atm a":
                        temp = X * F2;
                        break;

                    case "psig":
                        temp = X / F3 + F2;
                        break;

                    case "psia":
                        temp = X / F3;
                        break;

                    case "kg/cm2 g":
                        temp = X + F2;
                        break;

                    case "kg/cm2":
                        temp = X;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (CUNIT)
                {
                    case "mmHg g":
                        temp = (X - F2) * 760 / F2;
                        break;

                    case "mmHg":
                        temp = X * 760 / F2;
                        break;

                    case "bar g":
                        temp = (X - F2) / F5;
                        break;

                    case "bar a":
                        temp = X / F5;
                        break;

                    case "kPa g":
                        temp = (X - F2) / F4;
                        break;

                    case "kPa":
                        temp = X / F4;
                        break;

                    case "atm g":
                        temp = (X - F2) / F2;
                        break;

                    case "atm a":
                        temp = X / F2;
                        break;

                    case "psig":
                        temp = (X - F2) * F3;
                        break;

                    case "psia":
                        temp = X * F3;
                        break;

                    case "kg/cm2 g":
                        temp = X - F2;
                        break;

                    case "kg/cm2":
                        temp = X;
                        break;

                    default:
                        break;
                }
            }
            return temp;
        }

        public static double MassFlow(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kg/hr", "te/hr", "Klb/hr", "ton/hr", "lb/hr", "Kte/a", "te/min", "te/day", "ton/min", "ton/day", "Klb/min", "Klb/day", "kg/s" };
            temp = X;
            switch (CUNIT)
            {
                case "Klb/day":
                    temp = X * 2.20462 * 24 / 1000;
                    break;

                case "Klb/min":
                    temp = X * 2.20462 / 60 / 1000;
                    break;

                case "ton/day":
                    temp = X * 24 / 0.907185 / 1000;
                    break;

                case "ton/min":
                    temp = X / (60 * 0.907185) / 1000;
                    break;

                case "te/day":
                    temp = X * 24 / 1000;
                    break;

                case "te/min":
                    temp = X / 60 / 1000;
                    break;

                case "Kte/a":
                    temp = X * 8000 * 0.001 / 1000;
                    break;

                case "lb/hr":
                    temp = X * 2204.62 / 1000;
                    break;

                case "kg/hr":
                    temp = X;
                    break;

                case "ton/hr":
                    temp = X / 0.907185 / 1000;
                    break;

                case "Klb/hr":
                    temp = X * 2.20462 / 1000;
                    break;

                case "te/hr":
                    temp = X / 1000;
                    break;

                case "kg/s":
                    temp = X / 3600;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        public static double MolarFlow(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double F2 = 2.20462, temp = 0;

            Units = new string[] { "kgmol/hr", "lbmol/hr", "kgmole/s" };

            temp = X;
            switch (CUNIT)
            {
                case "lbmol/hr":
                    temp = X * F2;
                    break;

                case "kgmol/hr":
                    temp = X;
                    break;

                case "kgmol/s":
                    temp = X / 60 / 60;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }
            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        public static double LHV(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kJ/kg", "Kcal/kg", "Btu/lb", "GJ/te", "MJ/kg" };

            temp = X;
            switch (CUNIT)
            {
                case "kJ/kg":
                case "Kcal/kg":
                    temp = X / 4.184;
                    break;

                case "GJ/te":
                case "MJ/kg":
                    temp = X / 1000;
                    break;

                case "Btu/lb":
                    temp = X / 1.0550559 / 2.2046226;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public static double Enthalpy(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kJ/kg", "Kcal/kg", "Btu/lb", "MJ/kg" };

            temp = X;
            switch (CUNIT)
            {
                case "kJ/kg":
                    temp = X;
                    break;

                case "MJ/kg":
                    temp = X / 1000;
                    break;

                case "Btu/lb":
                    temp = X / 4.1868 * 1.8;
                    break;

                case "Kcal/kg":
                    temp = X / 4.1868;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public static double MolarEnthalpy(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kJ/kg.mol", "Kcal/kg.mol", "Btu/lb.mol", "MJ/kg.mol" };

            temp = X;
            switch (CUNIT)
            {
                case "kJ/kg":
                    temp = X;
                    break;

                case "MJ/kg":
                    temp = X / 1000;
                    break;

                case "Btu/lb":
                    temp = X / 4.1868 * 1.8;
                    break;

                case "Kcal/kg":
                    temp = X / 4.1868;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public static double Entropy(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kJ/kg/C", "BTU/lb/F" };
            temp = X;
            switch (CUNIT)
            {
                case "kJ/kg/C":
                    temp = X;
                    break;

                case "BTU/lb/F":
                    temp = X;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public static double MolarEntropy(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kJ/kgmol/C" };
            temp = X;
            switch (CUNIT)
            {
                case "kJ/kgmol/C":
                    temp = X;
                    break;

                default:
                    break;
            }

            if (X == 0)
            {
                return 0;
            }

            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public enum TempEnum
        { C, F, K, R }

        public static double Temperature(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double F2 = 32, F3 = 1.8, F4 = 273.15, F5 = 491.67, a = 0.0001173, b = 0.9146, C = 6.038, temp = 0;

            Units = new string[] { "C", "F", "K", "R" };

            TempEnum tempe = TempEnum.C;

            o = tempe;

            temp = X;
            if (ToDefault)
            {
                switch (CUNIT)
                {
                    case "R":
                        temp = (X - F5) / F3;
                        break;

                    case "K":
                        temp = X - F4;
                        break;

                    case "F":
                        temp = (X - F2) / F3;
                        break;

                    case "C":
                        temp = X;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (CUNIT)
                {
                    case "F(1950)":
                        temp = X * F3 + F2;
                        if (temp > 650) temp = C + temp * (b + temp * a);
                        break;

                    case "R":
                        temp = X * F3 + F5;
                        break;

                    case "K":
                        temp = X + F4;
                        break;

                    case "F":
                        temp = X * F3 + F2;
                        break;

                    case "C":
                        temp = X;
                        break;

                    default:
                        break;
                }
            }
            return temp;
        }

        public static double Energy(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kJ", "J", "MJ", "GJ", "cal", "Kcal", "M Kcal", "Btu", "M Btu", "Therm", "W-h", "W-s", "erg" };

            temp = X;
            switch (CUNIT)
            {
                case "J":
                case "W-s":
                    temp = X;
                    break;

                case "W-h":
                    temp = X * 3600;
                    break;

                case "Btu":
                    temp = X * 1055.055853;
                    break;

                case "erg":
                    temp = X / 10000000;
                    break;

                case "Kcal":
                    temp = X * 4.1868 * 1000;
                    break;

                case "kJ":
                    temp = X * 1000;
                    break;

                case "GJ":
                    temp = X * 1000000000;
                    break;

                case "MJ":
                    temp = X * 100000;
                    break;

                case "M Btu":
                    temp = X * 1055.055853 * 1000000;
                    break;

                case "M Kcal":
                    temp = X * 4.1868 * 1000000;
                    break;

                case "Therm":
                    temp = X * 105505600;
                    break;

                case "cal":
                    temp = X * 4.1868;
                    break;

                default:
                    break;
            }
            if (X == 0)
                return 0;

            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public static double EnergyFlow(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "kJ/hr", "kcal/hr", "kJ/s", "kW", "MW", "BTU/hr", "BTU/s" };

            temp = X;
            switch (CUNIT)
            {
                case "kJ/hr":
                    temp = X;
                    break;

                case "kcal/hr":
                    temp = X / 4.1868;
                    break;

                case "kJ/s":
                    temp = X / 3600;
                    break;

                case "kW":
                    temp = X / 3600;
                    break;

                case "MW":
                    temp = X / 3600 / 1000;
                    break;

                case "BTU/hr":
                    temp = X / 1055.055853 * 1000;
                    break;

                case "BTU/s":
                    temp = X / 1055.055853 * 1000 / 3600;
                    break;

                default:
                    break;
            }

            if (X == 0)
                return 0;

            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public static double SpecificVolume(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "cm3/g", "ft3/lb", "ft3/oz" };

            temp = X;
            switch (CUNIT)
            {
                case "cm3/g":
                    temp = X;
                    break;

                case "ft3/lb":
                    temp = X * 1.601845326e-002;
                    break;

                case "ft3/oz":
                    temp = X * 1.001154331e-003;
                    break;

                default:
                    break;
            }

            if (X == 0)
                return 0;

            if (ToDefault)
                temp = X * X / temp;
            return temp;
        }

        public static double EnergyPrice(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "$/GJ", "c/kWhr", "$/MJ", "$/kWhr", "$/MWhr", "$/MMBTU", "$/BTU" };

            temp = X;
            switch (CUNIT)
            {
                case "$/GJ":
                    temp = X;
                    break;

                case "$/MJ":
                    temp = X / 1000;
                    break;

                case "$/MWhr":
                    temp = X / 1000 * 60 * 60;
                    break;

                case "$/kWhr":
                    temp = X / 1000000 * 3600;
                    break;

                case "c/kWhr":
                    temp = X / 1000000 * 3600 * 100;
                    break;

                case "$/MMBTU":
                    temp = X * 1.0550559;
                    break;

                case "$/BTU":
                    temp = X * 1.0550559 / 1e6;
                    break;

                default:
                    break;
            }
            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        public static double MassPrice(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "$/kg", "$/te" };

            temp = X;
            switch (CUNIT)
            {
                case "$/kg":
                    temp = X;
                    break;

                case "$/te":
                    temp = X / 1000;
                    break;

                default:
                    break;
            }
            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }

        public static double Area(double X, string CUNIT, bool ToDefault, ref string[] Units, object o)
        {
            double temp = 0;

            Units = new string[] { "m2", "ft2" };

            temp = X;
            switch (CUNIT)
            {
                case "m2":
                    temp = X;
                    break;

                case "ft2":
                    temp = X * Math.Pow(100 / 2.54 / 12, 2);
                    break;

                default:
                    break;
            }
            if (ToDefault)
                temp = X * X / temp;

            return temp;
        }
    }
}