using System;
using Units.UOM;

namespace SimpleProps
{
    public class TestProps
    {
        //kJ/mol
        public const double n3HVap = 15968.1498929234;

        public const double n4HVap = 18814.7425158597;

        //kJ/mol     double
        public const double n5HVap = 24860;

        public const double n6Hvap = 16730;
        public const double n7Hvap = 18790;
        public const double n8HVap = 26510;
        public const double n9Hvap = 22920;
        public const double n10HVap = 28400;
        public const double n11HVap = 27050;
        public const double n12HVap = 29110;
        public const double n13HVap = 0;
        public const double n14HVap = 0;
        public const double n15HVap = 0;
        public const double n16HVap = 0;
        public const double n17HVap = 0;
        public const double n18HVap = 32070;
        public const double n20HVap = 34000;

        public const double n28HVap = 25790;

        //kJ/mol     double
        public const double PHForm = -103890;

        public const double BHForm = -126190;
        public const double HHForm = -187900;
        public const double OHForm = -208600;
        public const double DHForm = -249800;
        public const double n28HForm = -622800;
        public const double n18HForm = -414800;
        public const double n20HForm = -456100;
        public const double n18MW = 254.5;
        public const double n20MW = 282.5;
        public const double BaseT = 298.15;
        public const bool DataSet2 = true;

        private static double t2, a, b, c, d, e, f;

        public static Pressure VP(string Name, Temperature t)
        {
            double functionreturnValue = 0;

            switch (Name)
            {
                case "Propane":
                    functionreturnValue = Math.Exp(52.3785 + (-3490.55 / (t + 0)) + -6.10875 * Math.Log(t) + 1.11869E-05 * (Math.Pow(t, 2)));
                    break;

                case "n-Butane":
                    functionreturnValue = Math.Exp(66.945 + (-4604.09 / (t + 0)) + -8.25491 * Math.Log(t) + 1.15706E-05 * (Math.Pow(t, 2)));
                    break;

                case "n-Pentane":
                    functionreturnValue = Math.Exp(63.3315 + (-5117.78 / (t + 0)) + -7.48305 * Math.Log(t) + 7.76606E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-Hexane":
                    functionreturnValue = Math.Exp(70.4265 + (-6055.6 / (t + 0)) + -8.37865 * Math.Log(t) + 6.61666E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-Heptane":
                    functionreturnValue = Math.Exp(78.3285 + (-6947 / (t + 0)) + -9.44866 * Math.Log(t) + 6.47481E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-Octane":
                    functionreturnValue = Math.Exp(86.997 + (-7890.6 / (t + 0)) + -10.6255 * Math.Log(t) + 6.47441E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-Nonane":
                    functionreturnValue = Math.Exp(111.977 + (-9558.5 / (t + 0)) + -14.2702 * Math.Log(t) + 8.46031E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-Decane":
                    functionreturnValue = Math.Exp(123.136 + (-10635.2 / (t + 0)) + -15.8051 * Math.Log(t) + 8.64176E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-C11":
                    functionreturnValue = Math.Exp(121.162 + (-11079.2 / (t + 0)) + -15.3772 * Math.Log(t) + 7.59021E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-C12":
                    functionreturnValue = Math.Exp(125.191 + (-11737 / (t + 0)) + -15.8737 * Math.Log(t) + 7.29968E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-C13":
                    functionreturnValue = Math.Exp(14.1201 + (-3892.9 / (t + -98.93)) + 0 * Math.Log(t) + 0 * (Math.Pow(t, 2)));
                    break;

                case "n-C14":
                    functionreturnValue = Math.Exp(143.577 + (-13893.7 / (t + 0)) + -18.2992 * Math.Log(t) + 7.36401E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-C15":
                    functionreturnValue = Math.Exp(152.638 + (-14762.2 / (t + 0)) + -19.5485 * Math.Log(t) + 7.64105E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-C16":
                    functionreturnValue = Math.Exp(225.023 + (-18736.5 / (t + 0)) + -30.231 * Math.Log(t) + 1.38881E-05 * (Math.Pow(t, 2)));
                    break;

                case "n-C17":
                    functionreturnValue = Math.Exp(14.1361 + (-4294.53 / (t + -124)) + 0 * Math.Log(t) + 0 * (Math.Pow(t, 2)));
                    break;

                case "n-C18":
                    functionreturnValue = Math.Exp(14.1069 + (-4361.79 / (t + -129.899)) + 0 * Math.Log(t) + 0 * (Math.Pow(t, 2)));
                    break;

                case "n-C19":
                    functionreturnValue = Math.Exp(14.1381 + (-4450.43 / (t + -135.5)) + 0 * Math.Log(t) + 0 * (Math.Pow(t, 2)));
                    break;

                case "n-C20":
                    functionreturnValue = Math.Exp(196.75 + (-19441 / (t + 0)) + -25.525 * Math.Log(t) + 8.8382E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-C21":
                    functionreturnValue = Math.Exp(133.88 + (-17129 / (t + 0)) + -15.87 * Math.Log(t) + 3.5456E-18 * (Math.Pow(t, 2)));
                    break;

                case "n-C22":
                    functionreturnValue = Math.Exp(147.4 + (-18406 / (t + 0)) + -17.694 * Math.Log(t) + 3.9369E-18 * (Math.Pow(t, 2)));
                    break;

                case "n-C23":
                    functionreturnValue = Math.Exp(212.92 + (-21841 / (t + 0)) + -27.531 * Math.Log(t) + 8.4197E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-C24":
                    functionreturnValue = Math.Exp(204.51 + (-21711 / (t + 0)) + -26.255 * Math.Log(t) + 7.7485E-06 * (Math.Pow(t, 2)));
                    break;

                case "n-C25":
                    functionreturnValue = Math.Exp(152.24 + (-19976 / (t + 0)) + -18.161 * Math.Log(t) + 3.0543E-18 * (Math.Pow(t, 2)));
                    break;

                case "n-C26":
                    functionreturnValue = Math.Exp(148.73 + (-20116 / (t + 0)) + -17.616 * Math.Log(t) + 2.6734E-18 * (Math.Pow(t, 2)));
                    break;

                case "n-C27":
                    functionreturnValue = Math.Exp(148.85 + (-20612 / (t + 0)) + -17.548 * Math.Log(t) + 2.3171E-18 * (Math.Pow(t, 2)));
                    break;

                case "n-C28":
                    functionreturnValue = Math.Exp(285.21 + (-28200 / (t + 0)) + -37.544 * Math.Log(t) + 1.135E-05 * (Math.Pow(t, 2)));
                    break;

                case "n-CMath.29":
                    functionreturnValue = Math.Exp(201.65 + (-24971 / (t + 0)) + -24.748 * Math.Log(t) + 4.1617E-18 * (Math.Pow(t, 2)));
                    break;

                case "n-C30":
                    functionreturnValue = Math.Exp(188.81 + (-22404 / (t + 0)) + -23.359 * Math.Log(t) + 4.4305E-18 * (Math.Pow(t, 2)));

                    break;
            }
            return new Pressure(functionreturnValue / 100);
        }

        public static double VapEnth2(string Name, Temperature t)
        {
            double functionreturnValue = 0;

            switch (Name)
            {
                case "Propane":
                    functionreturnValue = 0.395 * (t - BaseT) + 0.00211409 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + 3.96486E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + -6.67176E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 1.67936E-13 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-Butane":
                    functionreturnValue = 0.00854058 * (t - BaseT) + 0.00327699 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.10968E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 1.76646E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -6.39926E-15 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-Pentane":
                    functionreturnValue = -0.0117017 * (t - BaseT) + 0.0033164 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.1705E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 1.99636E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -8.66485E-15 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-Hexane":
                    functionreturnValue = -0.096697 * (t - BaseT) + 0.00347649 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.3212E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.52365E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -1.34666E-14 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-Heptane":
                    functionreturnValue = -0.0968949 * (t - BaseT) + 0.003473 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.3302E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.55766E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -1.37726E-14 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-Octane":
                    functionreturnValue = -0.2701 * (t - BaseT) + 0.00399829 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.973E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 6.22796E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -9.38135E-14 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-Nonane":
                    functionreturnValue = -0.0652895 * (t - BaseT) + 0.00340288 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.25345E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.00955E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -2.23759E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-Decane":
                    functionreturnValue = -0.0556135 * (t - BaseT) + 0.00337665 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.23882E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 1.98716E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -3.70139E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C11":
                    functionreturnValue = -0.0537062 * (t - BaseT) + 0.00337144 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.23662E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 1.97836E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -2.08778E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C12":
                    functionreturnValue = -0.0547613 * (t - BaseT) + 0.00337268 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.24201E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 1.99451E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -3.4102E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C13":
                    functionreturnValue = -0.0567345 * (t - BaseT) + 0.0033764 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.24967E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.02038E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -1.34792E-22 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C14":
                    functionreturnValue = -0.0553483 * (t - BaseT) + 0.0033723 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.24726E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.01377E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -4.4377E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C15":
                    functionreturnValue = -0.0561178 * (t - BaseT) + 0.00337318 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.25103E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.02432E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -6.29863E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C16":
                    functionreturnValue = -0.0575014 * (t - BaseT) + 0.00337633 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.25675E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.04254E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -2.46365E-24 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C17":
                    functionreturnValue = -0.0580973 * (t - BaseT) + 0.00337688 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.25882E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.05021E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 4.21753E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C18":
                    functionreturnValue = -0.056861 * (t - BaseT) + 0.00337355 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.25642E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.04138E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -6.96619E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C19":
                    functionreturnValue = -0.0576885 * (t - BaseT) + 0.00337415 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.26001E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.05295E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -3.35504E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C20":
                    functionreturnValue = -0.07921 * (t - BaseT) + 0.00343135 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -1.3177E-06 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 2.23681E-10 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + -2.72426E-23 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C21":
                    functionreturnValue = 0.2843 * (t - BaseT) + 0.0028624 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.06426E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C22":
                    functionreturnValue = 0.2908 * (t - BaseT) + 0.0028633 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.07176E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C23":
                    functionreturnValue = 0.2963 * (t - BaseT) + 0.002864 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.07916E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C24":
                    functionreturnValue = 0.303499 * (t - BaseT) + 0.00286449 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.08416E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C25":
                    functionreturnValue = 0.3086 * (t - BaseT) + 0.0028654 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.09045E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C26":
                    functionreturnValue = 0.3145 * (t - BaseT) + 0.002866 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.09546E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C27":
                    functionreturnValue = 0.3196 * (t - BaseT) + 0.00286649 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.10035E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C28":
                    functionreturnValue = 0.324199 * (t - BaseT) + 0.002866 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.10536E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C29":
                    functionreturnValue = 0.329299 * (t - BaseT) + 0.0028675 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.10916E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;

                case "n-C30":
                    functionreturnValue = 0.334299 * (t - BaseT) + 0.002868 * (Math.Pow(t, 2) - Math.Pow(BaseT, 2)) + -7.11286E-07 * (Math.Pow(t, 3) - Math.Pow(BaseT, 3)) + 0 * (Math.Pow(t, 4) - Math.Pow(BaseT, 4)) + 0 * (Math.Pow(t, 5) - Math.Pow(BaseT, 5));
                    break;
            }
            return functionreturnValue;
        }

        public static double VapEnth(string name, Temperature t)
        {
            double res;
            switch (name)
            {
                case "Propane":
                    res = PropaneVapEnth(t);
                    break;

                case "n-Butane":
                    res = ButaneVapEnth(t);
                    break;

                case "n-Heptane":
                    res = HeptaneVapEnth(t);
                    break;

                case "n-Octane":
                    res = OctaneVapEnth(t);
                    break;

                case "n-Decane":
                    res = DecaneVapEnth(t);
                    break;

                case "n-C18":
                    res = n18VapEnth(t);
                    break;

                case "n-C20":
                    res = n20VapEnth(t);
                    break;

                case "n-C28":
                    res = n28VapEnth(t);
                    break;

                default:
                    res = -999;
                    break;
            }
            return res;
        }

        public static double LiqEnth(string Name, Temperature t)
        {
            double functionreturnValue = 0;
            switch (Name)
            {
                case "Propane":
                    functionreturnValue = PropaneLiqEnth(t);
                    break;

                case "n-Butane":
                    functionreturnValue = ButaneLiqEnth(t);
                    break;

                case "n-Heptane":
                    functionreturnValue = HeptaneLiqEnth(t);
                    break;

                case "n-Octane":
                    functionreturnValue = OctaneLiqEnth(t);
                    break;

                case "n-Decane":
                    functionreturnValue = DecaneLiqEnth(t);
                    break;

                case "n-C18":
                    functionreturnValue = n18LiqEnth(t);
                    break;

                case "n-C20":
                    functionreturnValue = n20LiqEnth(t);
                    break;

                case "n-C28":
                    functionreturnValue = n28LiqEnth(t);
                    break;
            }
            return functionreturnValue;
        }

        public static double ChenHVap(Temperature Tb, Temperature Tc, double Pc_bar)
        {
            return 8.3145 * Tb * (3.978 * Tb / Tc - 3.958 + 1.555 * Math.Log(Pc_bar / 100)) / (1.07 - Tb / Tc);
        }

        public static double ChenLiqEnth(string Name, Temperature t, Temperature Tb_C, double PcBar)
        {
            double res = 0;
            res = VapEnth(Name, t) + ChenHVap(Tb_C, t, PcBar);
            return res;
        }

        public static double PropaneVapEnth(Temperature t)
        {
            // kJ/kg
            double res;
            t2 = BaseT;
            a = 39.4889;
            b = 0.395;
            c = 0.00211409;
            d = 3.96486E-07;
            e = -6.67176E-10;
            f = 1.67936E-13;
            res = a + b * t + c * Math.Pow(t, 2) + d * Math.Pow(t, 3) + e * Math.Pow(t, 4) + f * Math.Pow(t, 5);
            res -= a + b * t2 + c * Math.Pow(t2, 2) + d * Math.Pow(t2, 3) + e * Math.Pow(t2, 4) + f * Math.Pow(t2, 5);
            res *= 44;
            if (Global.DeductHForm25)
                res += PHForm;

            return res;
        }

        public static double ButaneVapEnth(Temperature t)
        {
            // kJ/kg
            double res = 0;
            t2 = BaseT;
            a = 67.721;
            b = 0.00854058;
            c = 0.00327699;
            d = -1.10968E-06;
            e = 1.76646E-10;
            f = -6.39926E-15;
            res = a + b * t + c * Math.Pow(t, 2) + d * Math.Pow(t, 3) + e * Math.Pow(t, 4) + f * Math.Pow(t, 5);
            res -= a + b * t2 + c * Math.Pow(t2, 2) + d * Math.Pow(t2, 3) + e * Math.Pow(t2, 4) + f * Math.Pow(t2, 5);
            res *= 58.12;
            if (Global.DeductHForm25)
                res += BHForm;
            return res;
        }

        public static double HeptaneVapEnth(Temperature t)
        {
            // kJ/kg
            t2 = BaseT;
            a = 71.41;
            b = -0.0968949;
            c = 0.003473;
            d = -1.3302E-06;
            e = 2.55766E-10;
            f = -1.37726E-14;
            double res = a + b * t + c * Math.Pow(t, 2) + d * Math.Pow(t, 3) + e * Math.Pow(t, 4) + f * Math.Pow(t, 5);
            res -= a + b * t2 + c * Math.Pow(t2, 2) + d * Math.Pow(t2, 3) + e * Math.Pow(t2, 4) + f * Math.Pow(t2, 5);
            res *= 100.2;
            if (Global.DeductHForm25)
                res += HHForm;
            return res;
        }

        public static double OctaneVapEnth(Temperature t)
        {
            // kJ/kg
            t2 = BaseT;
            a = 126.507;
            b = -0.2701;
            c = 0.00399829;
            d = -1.973E-06;
            e = 6.22796E-10;
            f = -9.38135E-14;
            double res = a + b * t + c * Math.Pow(t, 2) + d * Math.Pow(t, 3) + e * Math.Pow(t, 4) + f * Math.Pow(t, 5);
            res -= a + b * t2 + c * Math.Pow(t2, 2) + d * Math.Pow(t2, 3) + e * Math.Pow(t2, 4) + f * Math.Pow(t2, 5);
            res *= 114.2;

            if (Global.DeductHForm25)
                res += PHForm;

            return res;
        }

        public static double DecaneVapEnth(Temperature t)
        {
            // kJ/kg
            t2 = BaseT;
            a = 7.33469E-09;
            b = -0.0556135;
            c = 0.00337665;
            d = -1.23882E-06;
            e = 1.98716E-10;
            f = -3.70139E-23;
            double res = a + b * t + c * Math.Pow(t, 2) + d * Math.Pow(t, 3) + e * Math.Pow(t, 4) + f * Math.Pow(t, 5);
            res -= a + b * t2 + c * Math.Pow(t2, 2) + d * Math.Pow(t2, 3) + e * Math.Pow(t2, 4) + f * Math.Pow(t2, 5);
            res *= 142.3;
            if (Global.DeductHForm25)
                res += DHForm;

            return res;
        }

        public static double n18VapEnth(Temperature t)
        {
            // kJ/kg
            double res = 0;
            t2 = BaseT;
            a = 1.28769E-08;
            b = -0.056861;
            c = 0.00337355;
            d = -1.25642E-06;
            e = 2.04138E-10;
            f = -6.96619E-23;
            res = a + b * t + c * Math.Pow(t, 2) + d * Math.Pow(t, 3) + e * Math.Pow(t, 4) + f * Math.Pow(t, 5);
            res -= a + b * t2 + c * Math.Pow(t2, 2) + d * Math.Pow(t2, 3) + e * Math.Pow(t2, 4) + f * Math.Pow(t2, 5);
            res *= n18MW;

            if (Global.DeductHForm25)
                res += n18HForm;

            return res;
        }

        public static double n20VapEnth(Temperature t)
        {
            // kJ/kg
            double res = 0;
            t2 = BaseT;
            a = 5.24019E-09;
            b = -0.07921;
            c = 0.00343135;
            d = -1.3177E-06;
            e = 2.23681E-10;
            f = -2.72426E-23;
            res = a + b * t + c * Math.Pow(t, 2) + d * Math.Pow(t, 3) + e * Math.Pow(t, 4) + f * Math.Pow(t, 5);
            res -= a + b * t2 + c * Math.Pow(t2, 2) + d * Math.Pow(t2, 3) + e * Math.Pow(t2, 4) + f * Math.Pow(t2, 5);
            res *= n20MW;
            if (Global.DeductHForm25)
                res += n20HForm;

            return res;
        }

        public static double n28VapEnth(Temperature t)
        {
            // kJ/kg
            double res = 0;
            t2 = BaseT;
            a = -55.033;
            b = 0.324199;
            c = 0.002866;
            d = -7.10536E-07;
            e = 0.0;
            f = 0;
            res = a + b * t + c * Math.Pow(t, 2) + d * Math.Pow(t, 3) + e * Math.Pow(t, 4) + f * Math.Pow(t, 5);
            res -= a + b * t2 + c * Math.Pow(t2, 2) + d * Math.Pow(t2, 3) + e * Math.Pow(t2, 4) + f * Math.Pow(t2, 5);
            res *= 394.7;
            if (Global.DeductHForm25)
                res += n28HForm;

            return res;
        }

        public static double PropaneLiqEnth(Temperature t)
        {
            return PropaneVapEnth(t) - n3HVap;
            /// 1000
        }

        public static double ButaneLiqEnth(Temperature t)
        {
            return ButaneVapEnth(t) - n4HVap;
            /// 1000
        }

        public static double HeptaneLiqEnth(Temperature t)
        {
            // kJ/kg
            return HeptaneVapEnth(t) - n5HVap;
        }

        public static double OctaneLiqEnth(Temperature t)
        {
            // kJ/kg
            return OctaneVapEnth(t) - n8HVap;
        }

        public static double DecaneLiqEnth(Temperature t)
        {
            // kJ/kg
            return DecaneVapEnth(t) - n10HVap;
        }

        public static double n18LiqEnth(Temperature t)
        {
            // kJ/kg
            return n18VapEnth(t) - n18HVap;
        }

        public static double n20LiqEnth(Temperature t)
        {
            // kJ/kg
            return n20VapEnth(t) - n20HVap;
        }

        public static double n28LiqEnth(Temperature t)
        {
            // kJ/kg
            return n28VapEnth(t) - n28HVap;
        }
    }
}