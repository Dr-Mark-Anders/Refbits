using ModelEngine;
using Extensions;
using Math2;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.RootFinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using RefBitsEquationSolver;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Units;
using Units.UOM;

namespace UnitTests
{
    [TestClass]
    public class TestBWRS
    {
        private double R = 8.31447;
        private Temperature Kelvin;
        private Pressure kPaA;

        private double MW = 44.09700012; //# kg/kg.mol
        private Temperature Tc = 369.8980103; //# Kelvin
        private Pressure Pc = 4256.660156; //# kPaA
        private double w = 0.152400002;

        private double Bo = 0.060228125;
        private double Ao = 500.7254656;
        private double Co = 66030166.22;
        private double Do = 2090428137;
        private double Eo = 65541613091;
        private double b = 0.021289462;
        private double a = 67.21043849;
        private double d = 14027.43031;
        private double alpha = 0.000490006;
        private double c = 14209931.35;
        private double gamma = 0.017778556;
        private Pressure P = 500;
        private double T = 105 + 273.15;

        private double fsolve(Func<double, double> f, double rho)
        {
            return BrentSolver.Solve(0.0001, 1, rho, f, 0.000001);
        }

        private double BWRSDensity(double rho)
        {
            return P.kPa - (rho * R * T + (Bo * R * T - Ao - Co / T.Pow(2) + Do / T.Pow(3) - Eo / T.Pow(4)) * rho.Pow(2)
                + (b * R * T - a - d / T) * rho.Pow(3)
                + alpha * (a + d / T) * rho.Pow(6)
                + c * rho.Pow(3) / T.Pow(2) * (1 + gamma * rho.Pow(2)) * Math.Exp(-gamma * rho.Pow(2)));
        }

        private double BWRSPressure(double rho)
        {
            double P = -(rho * R * T + (Bo * R * T - Ao - Co / T.Pow(2) + Do / T.Pow(3) - Eo / T.Pow(4)) * rho.Pow(2)
                + (b * R * T - a - d / T) * rho.Pow(3)
                + alpha * (a + d / T) * rho.Pow(6)
                + c * rho.Pow(3) / T.Pow(2) * (1 + gamma * rho.Pow(2)) * Math.Exp(-gamma * rho.Pow(2)));

            return P;
        }

        private double BWRSFugacity(double rho)
        {
            double P = R * T * Math.Log(rho * R * T) + 2 * (Bo * R * T - Ao - Co / T.Pow(2) + 5 * Do / T.Pow(3) - Eo / T.Pow(4)) * rho
                + 3 / 2 * (b * R * T - a - d / T) * rho.Pow(2)
                + 6 / 5 * alpha * (a + d / T) * rho.Pow(5)
                + c / gamma / T.Pow(2) * (1 - (1 - 1 / 2 * gamma * rho.Pow(2) - gamma.Pow(2) * rho.Pow(4))) * Math.Exp(-gamma * rho.Pow(2));

            P = Math.Exp(P / R / T);

            return P;
        }

        //(MolarDensity*Rg*T_1+(B_0*Rg*T_1-A_0-C_0/T_1^2+D_0/T_1^3-E_0/T_1^4)*MolarDensity^2
        //+(b*Rg*T_1-a-d/T_1)*MolarDensity^3
        //+alpha*(a+d/T_1)*MolarDensity^6
        //+c_*MolarDensity^3/T_1^2*(1+gamma*MolarDensity^2)*EXP(-gamma*MolarDensity^2))

        //### Propane Properties ###

        //### BWRS-EOS ###

        [TestMethod]
        public void TestBWRS2()
        {
            EOS(273.15 + 105, 5);
            EOS(273.15 + 190, 25);
        }

        public void EOS(Temperature T, Pressure P)
        {
            this.P = P;
            double rho_molar = fsolve(BWRSDensity, 50);  //# m3/kg.mol
            double molar_volme = 1 / rho_molar * 1000;  // # cm3/mol
            double rho_mass = rho_molar * MW;         // # kg/m3
            double Z = P.kPa / rho_molar / R / T.Kelvin;

            double Pf = BWRSDensity(rho_molar);
            //# Departure Functions per K.E Starling
            //# REPORT ORO-5249-2 DEVELOPMENT OF WORKING FLUID THERMODYNAMIC PROPERTIES INFORMATION FOR GEOTHERMAL CYCLES - PHASE I
            //# Enthalpy Departure - Equation 6
            //# Entropy Departure - Equation 10

            double enthalpy_departure = (Bo * R * T - 2 * Ao - 4 * Co / T.Pow(2) + 5 * Do / T.Pow(3) - 6 * Eo / T.Pow(4))
                * rho_molar + 0.5 * (2 * b * R * T - 3 * a - 4 * d / T) * rho_molar.Pow(2)
                + 1 / 5 * alpha * (6 * a + 7 * d / T) * rho_molar
                + c / gamma / T.Pow(2) * (3 - (3 + 0.5 * gamma * rho_molar.Pow(2) - gamma.Pow(2) * rho_molar.Pow(4))
                * Math.Exp(-gamma * Math.Pow(rho_molar, 2)));

            double entropy_departure = -R * Math.Log(rho_molar * R * T)
                - (Bo * R + 2 * Co / T.Pow(3) - 3 * Do / T.Pow(4) + 4 * Eo / T.Pow(5)) * rho_molar
                - 0.5 * (b * R + d / T.Pow(2)) * rho_molar.Pow(2)
                + alpha * d * rho_molar.Pow(5) / (5 * T.Pow(2))
                + 2 * c / (gamma * T.Pow(3)) * (1 - (1 + 0.5 * gamma * rho_molar.Pow(2)) * Math.Exp(-gamma * rho_molar.Pow(2)));

            //# Cp Ideal from Aspen HYSYS

            double CpID = 8.43833E-31 * Math.Pow(T, 6) + -8.61031E-27 * Math.Pow(T, 5) + 3.70274E-11 * Math.Pow(T, 4) + -1.17682E-07 * Math.Pow(T, 3) + 5.24515E-05 * Math.Pow(T, 2) + 0.186450054 * T + 17.41831505;

            double CvID = CpID - 8.31447;

            double Phi = BWRSFugacity(rho_molar) / P.kPa;

            double entdep = enthalpy_departure / T - R * Math.Log(Phi);

            Debug.Print(entropy_departure.ToString());
        }

        [TestMethod]
        public void TestBWRS3()
        {
            Components cc = new();
            cc.Add("Propane");
            cc[0].MoleFraction = 1;
            Pressure P = 5;
            Temperature t = new Temperature(105, TemperatureUnit.Celsius);

            ThermoDynamicOptions thermo = new();
            thermo.Enthalpy = enumEnthalpy.BWRS;

            ThermoProps Props = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo);

            cc.Add("n-Butane");
            cc[0].MoleFraction = 0.5;
            cc[1].MoleFraction = 0.5;

            thermo.Enthalpy = enumEnthalpy.BWRS;

            Props = ThermodynamicsClass.BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo);
        }
    }

    [TestClass]
    public class TestSpeed
    {
        public static readonly double baseTk = 273.15 + 25;

        private static readonly double baset1 = baseTk,
        baset2 = baset1 * baset1,
        baset3 = baset2 * baset1,
        baset4 = baset3 * baset1,
        baset5 = baset4 * baset1;

        [TestMethod]
        public void TestIdealEnthalpySpeed()
        {
            FlowSheet fs = new FlowSheet();

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");

            Components cc = DefaultStreams.ShortResidue();

            cc.Thermo.Enthalpy = enumEnthalpy.PR78;
            cc.Thermo.KMethod = enumEquiKMethod.PR78;
            cc.Thermo.CritTMethod = enumCritTMethod.TWU;
            cc.Thermo.CritPMethod = enumCritPMethod.TWU;
            cc.Thermo.MW_Method = enumMW_Method.TWU;
            cc.Thermo.UseBIPs = false;

            TemperatureC T = 370;
            Pressure P = 3;

            var watch = new Stopwatch();

            Enthalpy enth = new();

            //int cases = 9738336;
            int cases = 1000000;

            Temperature T1 = new(300);
            Temperature T2 = T1 * T1;
            Temperature T3 = T1 * T2;
            Temperature T4 = T2 * T2;
            Temperature T5 = T4 * T1;

            double[] MolFracs = cc.MoleFractions;

            BaseComp[] comps = cc.ComponentList.ToArray();

            watch.Start();
            //Parallel.For(0, cases, i =>
            //{
                for (int i = 0; i < cases; i++)
                {
                enth.BaseValue += IdealGas.StreamIdealGasMolarEnthalpyAndFormation(cc, T, MolFracs);
                // EnthFormation.BaseValue += ThermodynamicsClass.EnthalpyFormation25(cc, MolFracs);
                // enth.BaseValue += EnthFormation.BaseValue;
            };
            watch.Stop();

            System.Windows.Forms.MessageBox.Show(enth.BaseValue.ToString() + ": Time " + (watch.ElapsedMilliseconds / 1000.0).ToString());

            /*
            var Temperatures = Vector<double>.Build.DenseOfArray(new double[] { T1 - baset1, T2 - baset2, T3 - baset3, T4 - baset4, T5 - baset5 , 0});

            watch.Reset();
            double res = 0;
            EnthFormation = 0;

            watch.Start();
            for (int i = 0; i < cases; i++)
            {
                for (int x = 0; x < cc.Count; x++)
                {
                    Vector<double> cp = Vector<double>.Build.DenseOfArray(cc[x].IdealVapCP);
                    res += cp.DotProduct(Temperatures);
                    res += EnthFormation.BaseValue;
                }
                EnthFormation.BaseValue += ThermodynamicsClass.EnthalpyFormation25(cc, cc.MolFractions);
            }

            watch.Stop();

            System.Windows.Forms.MessageBox.Show(enth.BaseValue.ToString() + ": Time " + (watch.ElapsedMilliseconds / 1000.0).ToString());
  */
        }

        public class FastExpclass
        {
            private static double[] ExpAdjustment = new double[256] {
                1.040389835,
                1.039159306,
                1.037945888,
                1.036749401,
                1.035569671,
                1.034406528,
                1.033259801,
                1.032129324,
                1.031014933,
                1.029916467,
                1.028833767,
                1.027766676,
                1.02671504,
                1.025678708,
                1.02465753,
                1.023651359,
                1.022660049,
                1.021683458,
                1.020721446,
                1.019773873,
                1.018840604,
                1.017921503,
                1.017016438,
                1.016125279,
                1.015247897,
                1.014384165,
                1.013533958,
                1.012697153,
                1.011873629,
                1.011063266,
                1.010265947,
                1.009481555,
                1.008709975,
                1.007951096,
                1.007204805,
                1.006470993,
                1.005749552,
                1.005040376,
                1.004343358,
                1.003658397,
                1.002985389,
                1.002324233,
                1.001674831,
                1.001037085,
                1.000410897,
                0.999796173,
                0.999192819,
                0.998600742,
                0.998019851,
                0.997450055,
                0.996891266,
                0.996343396,
                0.995806358,
                0.995280068,
                0.99476444,
                0.994259393,
                0.993764844,
                0.993280711,
                0.992806917,
                0.992343381,
                0.991890026,
                0.991446776,
                0.991013555,
                0.990590289,
                0.990176903,
                0.989773325,
                0.989379484,
                0.988995309,
                0.988620729,
                0.988255677,
                0.987900083,
                0.987553882,
                0.987217006,
                0.98688939,
                0.98657097,
                0.986261682,
                0.985961463,
                0.985670251,
                0.985387985,
                0.985114604,
                0.984850048,
                0.984594259,
                0.984347178,
                0.984108748,
                0.983878911,
                0.983657613,
                0.983444797,
                0.983240409,
                0.983044394,
                0.982856701,
                0.982677276,
                0.982506066,
                0.982343022,
                0.982188091,
                0.982041225,
                0.981902373,
                0.981771487,
                0.981648519,
                0.981533421,
                0.981426146,
                0.981326648,
                0.98123488,
                0.981150798,
                0.981074356,
                0.981005511,
                0.980944219,
                0.980890437,
                0.980844122,
                0.980805232,
                0.980773726,
                0.980749562,
                0.9807327,
                0.9807231,
                0.980720722,
                0.980725528,
                0.980737478,
                0.980756534,
                0.98078266,
                0.980815817,
                0.980855968,
                0.980903079,
                0.980955475,
                0.981017942,
                0.981085714,
                0.981160303,
                0.981241675,
                0.981329796,
                0.981424634,
                0.981526154,
                0.981634325,
                0.981749114,
                0.981870489,
                0.981998419,
                0.982132873,
                0.98227382,
                0.982421229,
                0.982575072,
                0.982735318,
                0.982901937,
                0.983074902,
                0.983254183,
                0.983439752,
                0.983631582,
                0.983829644,
                0.984033912,
                0.984244358,
                0.984460956,
                0.984683681,
                0.984912505,
                0.985147403,
                0.985388349,
                0.98563532,
                0.98588829,
                0.986147234,
                0.986412128,
                0.986682949,
                0.986959673,
                0.987242277,
                0.987530737,
                0.987825031,
                0.988125136,
                0.98843103,
                0.988742691,
                0.989060098,
                0.989383229,
                0.989712063,
                0.990046579,
                0.990386756,
                0.990732574,
                0.991084012,
                0.991441052,
                0.991803672,
                0.992171854,
                0.992545578,
                0.992924825,
                0.993309578,
                0.993699816,
                0.994095522,
                0.994496677,
                0.994903265,
                0.995315266,
                0.995732665,
                0.996155442,
                0.996583582,
                0.997017068,
                0.997455883,
                0.99790001,
                0.998349434,
                0.998804138,
                0.999264107,
                0.999729325,
                1.000199776,
                1.000675446,
                1.001156319,
                1.001642381,
                1.002133617,
                1.002630011,
                1.003131551,
                1.003638222,
                1.00415001,
                1.004666901,
                1.005188881,
                1.005715938,
                1.006248058,
                1.006785227,
                1.007327434,
                1.007874665,
                1.008426907,
                1.008984149,
                1.009546377,
                1.010113581,
                1.010685747,
                1.011262865,
                1.011844922,
                1.012431907,
                1.013023808,
                1.013620615,
                1.014222317,
                1.014828902,
                1.01544036,
                1.016056681,
                1.016677853,
                1.017303866,
                1.017934711,
                1.018570378,
                1.019210855,
                1.019856135,
                1.020506206,
                1.02116106,
                1.021820687,
                1.022485078,
                1.023154224,
                1.023828116,
                1.024506745,
                1.025190103,
                1.02587818,
                1.026570969,
                1.027268461,
                1.027970647,
                1.02867752,
                1.029389072,
                1.030114973,
                1.030826088,
                1.03155163,
                1.032281819,
                1.03301665,
                1.033756114,
                1.034500204,
                1.035248913,
                1.036002235,
                1.036760162,
                1.037522688,
                1.038289806,
                1.039061509,
                1.039837792,
                1.040618648
            };

            public static double FastExp(double x)
            {
                var tmp = (long)(1512775 * x + 1072632447);
                int index = (int)(tmp >> 12) & 0xFF;
                return BitConverter.Int64BitsToDouble(tmp << 32) * ExpAdjustment[index];
            }

            public static double FastExp2(double x)
            {
                var tmp = (long)(1512775 * x + 1072632447);
                return BitConverter.Int64BitsToDouble(tmp << 32);
            }
        }

        [TestMethod]
        public void TestFastSQRT()
        {
            double[] x = new double[1000000];
            double[] ex = new double[x.Length];
            double[] fx = new double[x.Length];
            Random r = new Random();
            for (int i = 0; i < x.Length; ++i)
                x[i] = r.NextDouble() * 40;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int j = 0; j < x.Length; ++j)
                ex[j] = Math.Sqrt(x[j]);
            sw.Stop();
            double builtin = sw.Elapsed.TotalMilliseconds;
            sw.Reset();
            sw.Start();
            for (int k = 0; k < x.Length; ++k)
                fx[k] = InvSqrt((float)x[k]);
            sw.Stop();
            double custom = sw.Elapsed.TotalMilliseconds;

            double min = 1, max = 1;
            for (int m = 0; m < x.Length; ++m)
            {
                double ratio = fx[m] / ex[m];
                if (min > ratio) min = ratio;
                if (max < ratio) max = ratio;
            }

            //Console.OpenStandardOutput();
            //Console.SetWindowSize(500,500);
            //Console.SetWindowPosition(0, 0);
            //Debug.WriteLine("minimum ratio = " + min.ToString() + ", maximum ratio = " + max.ToString() + ", speedup = " + (builtin / custom).ToString());
            //Console.ReadKey();
            MessageBox.Show((builtin / custom).ToString());
        }

        private float InvSqrt(float x)
        {
            float xhalf = 0.5f * x;
            int i = BitConverter.SingleToInt32Bits(x);
            i = 0x5f3759df - (i >> 1);
            x = BitConverter.Int32BitsToSingle(i);
            x = x * (1.5f - xhalf * x * x);
            return x;
        }

        [TestMethod]
        public void TestFastExp()
        {
            double[] x = new double[1000000];
            double[] ex = new double[x.Length];
            double[] fx = new double[x.Length];
            Random r = new Random();
            for (int i = 0; i < x.Length; ++i)
                x[i] = r.NextDouble() * 40;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int j = 0; j < x.Length; ++j)
                ex[j] = Ext.exp01(x[j]);
            sw.Stop();
            double builtin = sw.Elapsed.TotalMilliseconds;
            sw.Reset();
            sw.Start();
            for (int k = 0; k < x.Length; ++k)
                fx[k] = Ext.exp0(x[k]);
            sw.Stop();
            double custom = sw.Elapsed.TotalMilliseconds;

            double min = 1, max = 1;
            for (int m = 0; m < x.Length; ++m)
            {
                double ratio = fx[m] / ex[m];
                if (min > ratio) min = ratio;
                if (max < ratio) max = ratio;
            }

            //Console.OpenStandardOutput();
            //Console.SetWindowSize(500,500);
            //Console.SetWindowPosition(0, 0);
            //Debug.WriteLine("minimum ratio = " + min.ToString() + ", maximum ratio = " + max.ToString() + ", speedup = " + (builtin / custom).ToString());
            //Console.ReadKey();
            MessageBox.Show((builtin / custom).ToString());
        }

        [TestMethod]
        public void TestFastExpAccuracy()
        {
            double x = 0;
            for (int i = -10; i < 10; i++)
            {
                x = i;
                double X_2 = Ext.Exp2(x);
                double X0 = Ext.exp0(x);
                double X1 = Ext.exp1(x);
                double X2 = Ext.exp2(x);
                double X3 = Ext.exp3(x);
                double X4 = Ext.exp4(x);
                double X5 = Ext.exp5(x);
                double X6 = Ext.exp6(x);
                double X7 = Ext.exp7(x);

                //Debug.Print (Math.Exp(x).ToString() + " " + X0.ToString());
                Debug.Print((Math.Exp(x) / X0).ToString());
                //Debug.Print (Math.Exp(x).ToString() + " " + X_2.ToString() + " " + X0.ToString() + " " + X1.ToString() + " " + X2.ToString() + " " + X3.ToString() + " " + X4.ToString() + " " + X5.ToString() + " " + X6.ToString() + " " + X7.ToString());
            }
        }

        [TestMethod]
        public void TestFastLog()
        {
            var watch = new Stopwatch();
            FastLog.Log2(1); //create table for the fist time

            Console.WriteLine("benchmarking .net log10...");
            var Value = 1.0;
            watch.Reset();
            watch.Start();
            for (var i = 0; i < 1000000000; i++)
            {
                Math.Log10(Value);
                Value += 1.0;
            }
            watch.Stop();
            var netTime = watch.Elapsed;
            Console.WriteLine("result: {0}", netTime);

            Console.WriteLine("benchmarking fast log10...");
            var floatValue = 1.0F;
            watch.Reset();
            watch.Start();
            for (var i = 0; i < 1000000000; i++)
            {
                FastLog.Log10(floatValue);
                floatValue += 1.0F;
            }
            watch.Stop();

            var fastTime = watch.Elapsed;
            Console.WriteLine("result: {0}", fastTime);

            Console.WriteLine("ratio: {0}", netTime.TotalMilliseconds / (float)fastTime.TotalMilliseconds);

            Console.WriteLine("done.");
            Console.ReadKey();
        }

        public static class FastLog
        {
            [StructLayout(LayoutKind.Explicit)]
            private struct Ieee754
            {
                [FieldOffset(0)] public float Single;
                [FieldOffset(0)] public uint UnsignedBits;
                [FieldOffset(0)] public int SignedBits;

                public uint Sign
                {
                    get
                    {
                        return UnsignedBits >> 31;
                    }
                }

                public int Exponent
                {
                    get
                    {
                        return (SignedBits >> 23) & 0xFF;
                    }
                }

                public uint Mantissa
                {
                    get
                    {
                        return UnsignedBits & 0x007FFFFF;
                    }
                }
            }

            private static readonly float[] MantissaLogs = new float[(int)Math.Pow(2, 23)];
            private const float Base10 = 3.321928F;
            private const float BaseE = 1.442695F;

            static FastLog()
            {
                //creating lookup table
                for (uint i = 0; i < MantissaLogs.Length; i++)
                {
                    var n = new Ieee754 { UnsignedBits = i | 0x3F800000 }; //added the implicit 1 leading bit
                    MantissaLogs[i] = (float)Math.Log(n.Single, 2);
                }
            }

            public static float Log2(float value)
            {
                if (value == 0F)
                    return float.NegativeInfinity;

                var number = new Ieee754 { Single = value };

                if (number.UnsignedBits >> 31 == 1) //NOTE: didn't call Sign property for higher performance
                    return float.NaN;

                return (((number.SignedBits >> 23) & 0xFF) - 127) + MantissaLogs[number.UnsignedBits & 0x007FFFFF];
                //NOTE: didn't call Exponent and Mantissa properties for higher performance
            }

            public static float Log10(float value)
            {
                return Log2(value) / Base10;
            }

            public static float Ln(float value)
            {
                return Log2(value) / BaseE;
            }

            public static float Log(float value, float valueBase)
            {
                return Log2(value) / Log2(valueBase);
            }
        }

        [TestMethod]
        public void TestFieldProperty()
        {
            TestFeldPropertyclass TestClass = new();
            double t = 100;
            double res1 = 0, res2 = 0;

            var watch = Stopwatch.StartNew();
            for (long i = 0; i < 1e10; i++)
            {
                res1 += TestClass.Field;
            }

            watch.Stop();
            var elapsedMs1 = watch.ElapsedMilliseconds;

            watch = Stopwatch.StartNew();

            for (long i = 0; i < 1e10; i++)
            {
                res2 += TestClass.field;// TestClass .Field;
            }

            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;
            MessageBox.Show("Call with property " + res1.ToString() + " " + elapsedMs1.ToString() + " Call with field  " + res2.ToString() + " " + elapsedMs2.ToString());
        }

        public class TestFeldPropertyclass
        {
            public double field = 100;

            public double Field { get => field; set => field = value; }
        }

        [TestMethod]
        public void TestCallSpeed()
        {
            Temperature T = 100;
            double t = 100;
            double res1 = 0, res2 = 0;

            var watch = Stopwatch.StartNew();
            for (long i = 0; i < 1e10; i++)
            {
                res1 += TestCall(T);
            }

            watch.Stop();
            var elapsedMs1 = watch.ElapsedMilliseconds;

            watch = Stopwatch.StartNew();

            for (long i = 0; i < 1e10; i++)
            {
                res2 += TestCall2(t);
            }

            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;
            MessageBox.Show("Call with struct " + res1.ToString() + " " + elapsedMs1.ToString() + " Call with double   " + res2.ToString() + " " + elapsedMs2.ToString());
        }

        public double TestCall(Temperature t)
        {
            return t;// t.BaseValue; //  basevalue is not faster
        }

        public double TestCall2(double t)
        {
            return t;
        }

        [TestMethod]
        public void MyTestForLoop()
        {
            double[] L = new double[1000000];

            for (int i = 0; i < L.Length; i++)
            {
                L[i] = Math.Pow(2.0, 3);
            }
        }

        [TestMethod]
        public void TestParallel()
        {
            List<string> fruits = new List<string>();
            fruits.Add("Apple");
            fruits.Add("Banana");
            fruits.Add("Bilberry");
            fruits.Add("Blackberry");
            fruits.Add("Blackcurrant");
            fruits.Add("Blueberry");
            fruits.Add("Cherry");
            fruits.Add("Coconut");
            fruits.Add("Cranberry");
            fruits.Add("Date");
            fruits.Add("Fig");
            fruits.Add("Grape");
            fruits.Add("Guava");
            fruits.Add("Jack-fruit");
            fruits.Add("Kiwi fruit");
            fruits.Add("Lemon");
            fruits.Add("Lime");
            fruits.Add("Lychee");
            fruits.Add("Mango");
            fruits.Add("Melon");
            fruits.Add("Olive");
            fruits.Add("Orange");
            fruits.Add("Papaya");
            fruits.Add("Plum");
            fruits.Add("Pineapple");
            fruits.Add("Pomegranate");

            Debug.WriteLine("Print ing list using   foreach loop\n");

            var stopWatch = Stopwatch.StartNew();
            foreach (string fruit in fruits)
            {
                Debug.WriteLine("Fruit Name: {0}, Thread Id= {1}", fruit, Thread.CurrentThread.ManagedThreadId);
            }
            MessageBox.Show("foreach loop execution time = {0} seconds\n", stopWatch.Elapsed.TotalMilliseconds.ToString());
            Debug.WriteLine("Print ing list using   Parallel.ForEach");

            stopWatch = Stopwatch.StartNew();
            Parallel.ForEach(fruits, fruit =>
            {
                Debug.WriteLine("Fruit Name: {0}, Thread Id= {1}", fruit, Thread.CurrentThread.ManagedThreadId);
            });

            MessageBox.Show("Parallel.ForEach() execution time = {0} seconds", stopWatch.Elapsed.TotalMilliseconds.ToString());
        }

        [TestMethod]
        public void MyTestForLoop2()
        {
            double[] L = new double[1000000];

            for (int i = 0; i < 1000000; i++)
            {
                L[i] = 2.0.Pow(3);
            }
        }

        [TestMethod]
        public void MyTestMethod()
        {
            for (int i = 0; i < 10000000; i++)
            {
                sqrt1(10);
            }
        }

        [TestMethod]
        public void MyTestMethod2()
        {
            for (int i = 0; i < 10000000; i++)
            {
                sqrt2(10);
            }
        }

        public static double sqrt2(double x)
        {
            return Math.Sqrt(x);
        }

        public static double sqrt1(double x)
        {
            int number = Convert.ToInt32(x);
            double root = 1;
            int i = 0;
            //The Babylonian Method for Computing Square Roots
            while (i < 1)
            {
                i++;
                root = (number / root + root) / 2;
            }

            return root;
        }

        [TestMethod]
        public void TestBroyden()
        {
            double[] res = Broyden.FindRoot(test2, new double[] { 1, 2 });
        }

        [TestMethod]
        public void TestSolver()
        {
            Equation eq = new();
            NonLinearSolver.SolveEquations(eq, SolverType.NewtonRaphson);
        }

        [TestMethod]
        public void TestAxBSolve()
        {
            double[][] A = new double[21][];
            double[] B = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.00253406887346149, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            A[0] = new double[] { 1.00132802124834, -1.04422048926833E-22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[1] = new double[] { -1, 1, -0.00197628458498023, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[2] = new double[] { 0, -1, 1.00395256916996, -6.07393181729027E-22, 0, 0, 0, 0, 0, 0, 0, -2.59607563152789E-20, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[3] = new double[] { 0, 0, -1, 1, -3.38316503654543E-23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -2.05706199516463E-22, 0, 0, 0 };
            A[4] = new double[] { 0, 0, 0, -1, 1.001, -9.06630576497972E-23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[5] = new double[] { 0, 0, 0, 0, -1, 1, -0.001, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[6] = new double[] { 0, 0, 0, 0, 0, -1, 1.00109999999999, -3.02851925110863E-22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[7] = new double[] { 0, 0, 0, 0, 0, 0, -1, 1, -9.19404833516839E-22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[8] = new double[] { 0, 0, 0, 0, 0, 0, 0, -1, 1, -7.15925440417729E-20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[9] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, -2.00164641434734E-16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[10] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[11] = new double[] { 0, 0, -0.00197628458498023, 0, 0, 0, 0, 0, 0, 0, 0, 1, -5.71507937059022E-17, 0, 0, 0, 0, 0, 0, 0, 0 };
            A[12] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, -1.23929922795113E-17, 0, 0, 0, 0, 0, 0, 0 };
            A[13] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, -1.099, 0, 0, 0, 0, 0, 0 };
            A[14] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 2.099, -2.82575701033932E-17, 0, 0, 0, 0, 0 };
            A[15] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, -1.12629783761874E+56, 0, 0, 0, 0 };
            A[16] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1.12629783761874E+56, 0, 0, 0, 0 };
            A[17] = new double[] { 0, 0, 0, 0, -0.001, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -5.71173659871002E-20, 0, 0 };
            A[18] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, -3.13977144412483E-19, 0 };
            A[19] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1, -9.15312085238058E-19 };
            A[20] = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 1 };

            var Am = Matrix<double>.Build.DenseOfRowArrays(A);
            var Bm = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.DenseOfArray(B);

            for (int i = 0; i < 100; i++)
            {
                var Cm = Am.Solve(Bm).ToArray();
                for (int n = 0; n < Cm.Length; n++)
                {
                    if (double.IsInfinity(Cm[n]))
                        Debugger.Break();
                }
            }

            double[,] Al = new double[21, 21];
            double[] bl = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.00253406887346149, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int x = 0; x < 21; x++)
            {
                for (int y = 0; y < 21; y++)
                {
                    Al[x, y] = A[x][y];
                }
            }

            alglib.rmatrixsolvefast(Al, 21, ref bl, out int info);
        }

        public class Equation : ISolveable
        {
            private readonly double[][] J = new double[2][];
            private double[]? res;
            private double[] x = new double[] { 1, 2 };

            public double[] XInitial { get => x; set => x = value; }
            public double[] XFinal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public double[][] CalculateJacobian(double[] x)
            {
                for (int i = 0; i < J.Length; i++)
                    J[i] = new double[2];

                double[] x1 = (double[])x.Clone();
                x1[0] = +0.001;
                J[0] = test2(x1);
                x1[0] = -0.001;

                x1[1] = +0.001;
                J[1] = test2(x1);
                x1[1] = -0.001;

                return J;
            }

            public double[][] CalculateJacobian(double[] x, double[] RHS)
            {
                for (int i = 0; i < J.Length; i++)
                    J[i] = new double[2];

                double[] x1 = (double[])x.Clone();
                x1[0] = +0.001;
                J[0] = test2(x1);
                x1[0] = -0.001;

                x1[1] = +0.001;
                J[1] = test2(x1);
                x1[1] = -0.001;

                return J;
            }

            public double[] CalculateRHS(double[] x)
            {
                res = test2(x);
                return res;
            }
        }

        public static double[] test2(double[] x)
        {
            double[] res = new double[2];
            res[0] = 10 * x[0] + 3 + (x[1] - 2).Pow(2);
            return res;
        }

        public class Tempclass
        {
            public double value = 100;
            private readonly double v;

            public Tempclass(double v)
            {
                this.v = v;
            }

            public static Tempclass operator *(Tempclass t1, Tempclass t2)
            {
                return new Tempclass(t1.value * t2.value);
            }
        }

        [TestMethod]
        public void TestListOrArray()
        {
            int Nocases = 10000000;
            double Value = 100;
            double res1 = 0, res2 = 0;
            List<double> list = new(Nocases);
            double[] array = new double[Nocases];

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < Nocases; i++)
            {
                list.Add(Value);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            watch = Stopwatch.StartNew();

            array = new double[Nocases];
            for (int i = 0; i < Nocases; i++)
            {
                array[i] = Value;
            }

            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;
            watch = Stopwatch.StartNew();

            for (int i = 0; i < Nocases; i++)
            {
                res1 += list[i];
            }

            watch.Stop();
            var elapsedMs3 = watch.ElapsedMilliseconds;
            watch = Stopwatch.StartNew();

            for (int i = 0; i < Nocases; i++)
            {
                res2 += array[i];
            }

            watch.Stop();
            var elapsedMs4 = watch.ElapsedMilliseconds;

            MessageBox.Show(res1.ToString() + " " + res2.ToString() +
                " List " + elapsedMs.ToString() +
                " Array " + elapsedMs2.ToString() +
                " List Access " + elapsedMs3.ToString() +
                " Array access " + elapsedMs4.ToString());
        }

        [TestMethod]
        public void Testdouble()
        {
            double Value = 100;
            Temperature t = new();
            Temperature Tres = new(100);
            Tempclass tclass = new(100);
            double res;

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Value * Value;
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply double s " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                Tres.BaseValue = t.BaseValue * t.BaseValue;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply struct fields " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                Tres.BaseValue = t.BaseValue * t.BaseValue;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply struct property " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                Tres = t * t;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply Temperature  struct using   overloaded * operator " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = tclass.value * tclass.value;
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply class  fields " + elapsedMs.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double exp1(double x)
        {
            x = 1.0 + x / 256.0;
            x *= x; x *= x; x *= x; x *= x;
            x *= x; x *= x; x *= x; x *= x;
            return x;
        }

        private readonly double[] T1 = new double[60000000];
        private readonly double[] T2 = new double[60000000];
        private readonly double[] T3 = new double[60000000];

        private double Test(int i)
        {
            double res = 0;
            res += Math.Exp(T1[i] + T2[i] * T3[i]);
            return res;
        }

        [TestMethod]
        public void TestLnandExp()
        {
            double Value = 100;
            double res;

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Value * Value;
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply double s " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Math.Log(Value);
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Log " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Math.Exp(Math.Log(Value));
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp(Log) " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Math.Exp(Math.Log(Value) + Math.Log(Value));
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp(Log + Log) " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Math.Exp(2.565436);
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(2.565436);
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp2 " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestLnandExpShort()
        {
            double Value = 100;
            double res;

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Value * Value;
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply double s " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(Value);
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Log " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(Math.Log(Value));
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp(Log) " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(Math.Log(Value) + Math.Log(Value));
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp(Log + Log) " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(2.565436);
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(2.565436);
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp2 " + elapsedMs.ToString());
        }

        [TestMethod]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public unsafe void TestdoubleArrayWithPts()
        {
            double[,] Array = new double[10000, 1000];
            var watch = Stopwatch.StartNew();
            int UpperBound0 = Array.GetUpperBound(0);
            int UpperBound1 = Array.GetUpperBound(1);

            watch.Restart();
            for (int i = 0; i < Array.GetUpperBound(0); i++)
                for (int y = 0; y < Array.GetUpperBound(1); y++)
                    Array[i, y] = 10D * 10D;

            var elapsedMs1 = watch.ElapsedMilliseconds;

            watch.Restart();
            for (int i = 0; i < UpperBound0; i++)
                for (int y = 0; y < UpperBound1; y++)
                    Array[i, y] = 10D * 10D;

            var elapsedMs2 = watch.ElapsedMilliseconds;

            watch.Restart();
            fixed (double* ptr = Array)
            {
                for (int i = 0; i < Array.GetUpperBound(0); i++)
                    for (int y = 0; y < Array.GetUpperBound(1); y++)
                        *(ptr + i * Array.GetUpperBound(1) + y) = 10D * 10D;
            }

            var elapsedMs3 = watch.ElapsedMilliseconds;

            watch.Restart();
            fixed (double* ptr = Array)
            {
                double* dst = ptr;
                for (int i = 0; i < UpperBound0; i++)
                    for (int y = 0; y < UpperBound1; y++)
                        *dst++ = 10D * 10D;
            }

            var elapsedMs5 = watch.ElapsedMilliseconds;

            watch.Restart();
            fixed (double* ptr = Array)
            {
                for (int i = 0; i < UpperBound0; i++)
                    for (int y = 0; y < UpperBound1; y++)
                        *(ptr + i * UpperBound1 + y) = 10D * 10D;
            }

            var elapsedMs4 = watch.ElapsedMilliseconds;

            watch.Restart();
            fixed (double* ptr = Array)
            {
                double* dst = ptr;
                for (int i = 0; i < UpperBound0; i++)
                    for (int y = 0; y < UpperBound1; y++)
                        *dst++ = 10D * 10D;
            }

            var elapsedMs6 = watch.ElapsedMilliseconds;

            /* var arraySpan = new  Span<double >(Array);

             byte data = 0;
             for (int  ctr = 0; ctr < arraySpan.Length; ctr++)
                 arraySpan[ctr] = data++;

             int  arraySum = 0;
             foreach (var value in Array)
                 arraySum += value;*/

            System.Windows.Forms.MessageBox.Show(elapsedMs1.ToString()
                + " " + elapsedMs2.ToString()
                + " " + elapsedMs3.ToString()
                + " " + elapsedMs4.ToString()
                + " " + elapsedMs5.ToString()
                + " " + elapsedMs6.ToString()
                );
        }

        [TestMethod]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public unsafe void TestdoubleArrayWithPts2()
        {
            int Size = 100;
            double[] Array = new double[100000000];
            Size = Array.Length;

            var watch = Stopwatch.StartNew();

            watch.Restart();
            for (int i = 0; i < Size; i++)
                Array[i] = 10D * 10D;

            var elapsedMs1 = watch.ElapsedMilliseconds;

            watch.Restart();
            for (int i = 0; i < Array.Length; i++)
                Array[i] = 10D * 10D;

            var elapsedMs2 = watch.ElapsedMilliseconds;

            watch.Restart();
            fixed (double* ptr = Array)
            {
                for (int i = 0; i < Size; i++)
                    *(ptr + i) = 10D * 10D;
            }

            var elapsedMs3 = watch.ElapsedMilliseconds;

            watch.Restart();
            fixed (double* ptr = Array)
            {
                for (int i = 0; i < Array.Length; i++)
                    *(ptr + i) = 10D * 10D;
            }

            var elapsedMs4 = watch.ElapsedMilliseconds;

            Debug.Print(elapsedMs1.ToString());
            Debug.Print(elapsedMs2.ToString());
            Debug.Print(elapsedMs3.ToString());
            Debug.Print(elapsedMs4.ToString());
        }

        [TestMethod]
        public void TestdoubleArrayAccess()
        {
            double value = 100;
            Temperature t = new();
            Temperature Tres = new(100);
            Tempclass tclass = new(100);
            double[] res = new double[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = value * value;
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply double s int o object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = value * value;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply double s int o double  array" + elapsedMs.ToString());

            watch = Stopwatch.StartNew();
            double res1 = 0;
            for (int i = 0; i < 10000000; i++)
            {
                res1 += value * value;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply double s int o double  " + elapsedMs.ToString() + " " + res1.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = (Tres * Tres).BaseValue;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply structs & Load double s int o object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = t.BaseValue * t.BaseValue;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply struct fields & Load int o object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = new Temperature(t.BaseValue * t.BaseValue);
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply struct fields & Load new  struct int o object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = t * t;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply structs with overloaded operator a load struct int o object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = tclass.value * tclass.value;
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply class  fields & load int o object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = new Tempclass(tclass.value * tclass.value).value;
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply class  fields & load new  class  int o object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = (tclass * tclass).value;
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply class  using   overloaded * operator & load class  int o object array " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestdoubleMult()
        {
            double value = 100;
            double[] res = new double[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res[i] = value * value;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply double s " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestFloatMult()
        {
            float value = 100;
            float[] res = new float[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res[i] = value * value;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply floats " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestDoublDiv()
        {
            double value = 100;
            double[] res = new double[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res[i] = value * value;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Div double s " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestFloatDiv()
        {
            float value = 100;
            float[] res = new float[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res[i] = value / value;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Div floats " + elapsedMs.ToString() + res.ToString());
        }

        [TestMethod]
        public void TestdoubleMultNoMatrix()
        {
            double value = 100;
            double res = 0;

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res = value * value;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply double s " + elapsedMs.ToString() + res.ToString());
        }

        [TestMethod]
        public void TestFloatMultNoMatrix()
        {
            float value = 100;
            float res = 0;

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res = value * value;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply floats " + elapsedMs.ToString() + res.ToString());
        }

        [TestMethod]
        public void TestArrayAcces()
        {
            float value = 100;
            float[] res = new float[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                value = res[i];

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("ArrayAcces " + elapsedMs.ToString() + res[9].ToString());
        }

        [TestMethod]
        public void TestListAcces()
        {
            double doublevalue = 100;
            List<double> res = new(10000000);

            for (int i = 0; i < 10000000; i++)
                res.Add(doublevalue);

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                doublevalue = res[i];

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("List Access " + elapsedMs.ToString() + res[9].ToString());
        }
    }
}