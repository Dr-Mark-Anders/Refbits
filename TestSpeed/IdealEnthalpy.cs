using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftAntimalwareEngine;
using System.Buffers.Text;
using System.Numerics;
using Units.UOM;

namespace EnthalpyCalc
{
    public record class BaseComp(double[] IdealVapCP, double MW, double v1, double v2, double v3, double v4, double v5);
    public record class BaseCompFlat(double IdealVapCP0,
                                     double IdealVapCP1,
                                     double IdealVapCP2,
                                     double IdealVapCP3,
                                     double IdealVapCP4,
                                     double MW);
    public class Program
    {
        const double BaseT = 273.15 + 25;
        const double BaseT1 = BaseT;
        const double BaseT2 = BaseT1 * BaseT1;
        const double BaseT3 = BaseT2 * BaseT1;
        const double BaseT4 = BaseT2 * BaseT2;
        const double BaseT5 = BaseT4 * BaseT1;

        static readonly Temperature T, T2, T3, T4, T5;
        static readonly double DT1, DT2, DT3, DT4, DT5;
        static readonly BaseComp TestBaseComp;
        static readonly BaseCompFlat TestBaseCompFlat;

        static Program()
        {
            T = new Temperature(256);
            T2 = T * T;
            T3 = T2 * T;
            T4 = T3 * T;
            T5 = T4 * T;
            DT1 = T - BaseT;
            DT2 = (T2 - BaseT2)/2;
            DT3 = (T3 - BaseT3)/3;
            DT4 = (T4 - BaseT4)/4;
            DT5 = (T5 - BaseT5)/5;
            TestBaseComp = new BaseComp(new double[] { 1234, 4547, 2341, 9097, 8697 }, 5235.0, 1234, 4547, 2341, 9097, 8697);
            TestBaseCompFlat = new BaseCompFlat(5235.0, 1234, 4547, 2341, 9097, 8697);
        }

        static double StandardRun(Func<Temperature, BaseComp, double> computeEnthalpy)
        {
            double x = 0;
            for (int i = 0; i < 500; i++)
                x += computeEnthalpy(T, TestBaseComp);
            return x;
        }


        static double StandardRun(Func<Temperature, BaseCompFlat, double> computeEnthalpy)
        {
            double x = 0;
            for (int i = 0; i < 500; i++)
                x += computeEnthalpy(T, TestBaseCompFlat);
            return x;
        }

        static double UseLoops(Temperature Tk, BaseComp sc)
        {
            double VE = 0;
            double temp = Tk;
            double baseT = BaseT;
            double[] cp = sc.IdealVapCP;

            if (cp != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    VE += cp[i] * (temp - baseT) / (i + 1);
                    temp *= Tk;
                    baseT *= BaseT;
                }
            }
            return VE * sc.MW;
        }

        static double UseLoopsPreRangeCheck(Temperature Tk, BaseComp sc)
        {
            double VE = 0;
            double temp = Tk;
            double baseT = BaseT;
            double[] cp = sc.IdealVapCP;

            if (cp != null && cp.Length >= 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    VE += cp[i] * (temp - baseT) / (i + 1);
                    temp *= Tk;
                    baseT *= BaseT;
                }
            }
            return VE * sc.MW;
        }



        static double Plain0(Temperature Tk, BaseComp sc)
        {
            double VE = 0;
            double Tk1 = Tk;
            double[] cp = sc.IdealVapCP;

            if (cp != null)
            {
                VE = cp[0] * (Tk1 - BaseT1)
                   + cp[1] * (Tk1 * Tk1 - BaseT2) / 2
                   + cp[2] * (Tk1 * Tk1 * Tk1 - BaseT3) / 3
                   + cp[3] * (Tk1 * Tk1 * Tk1 * Tk1 - BaseT4) / 4
                   + cp[4] * (Tk1 * Tk1 * Tk1 * Tk1 * Tk1 - BaseT5) / 5;
            }
            return VE * sc.MW;
        }

        static double Plain(Temperature Tk, BaseComp sc)
        {
            double VE = 0;
            double Tk1 = Tk;
            double Tk2 = Tk1 * Tk1; // slightly faster
            double[] cp = sc.IdealVapCP;

            if (cp != null)
            {
                VE = cp[0] * (Tk1 - BaseT1)
                   + cp[1] * (Tk2 - BaseT2) / 2
                   + cp[2] * (Tk2 * Tk1 - BaseT3) / 3
                   + cp[3] * (Tk2 * Tk2 - BaseT4) / 4
                   + cp[4] * (Tk1 * Tk2 * Tk2 - BaseT5) / 5;
            }
            return VE * sc.MW;
        }

        static double PlainPreRangeCheck(Temperature Tk, BaseComp sc)
        {
            double VE = 0;
            double Tk1 = Tk;
            double Tk2 = Tk1 * Tk1; // slightly faster
            double[] cp = sc.IdealVapCP;

            if (cp != null && cp.Length >= 5)
            {
                VE = cp[0] * (Tk1 - BaseT1)
                   + cp[1] * (Tk2 - BaseT2) / 2
                   + cp[2] * (Tk2 * Tk1 - BaseT3) / 3
                   + cp[3] * (Tk2 * Tk2 - BaseT4) / 4
                   + cp[4] * (Tk1 * Tk2 * Tk2 - BaseT5) / 5;
            }
            return VE * sc.MW;
        }

        static double PlainPreRangeCheckPassAllTks(Temperature Tk, BaseComp sc)
        {

            double VE = 0;
            double[] cp = sc.IdealVapCP;

            if (cp != null && cp.Length >= 5)
            {
                VE = cp[0] * DT1
                   + cp[1] * DT2
                   + cp[2] * DT3
                   + cp[3] * DT4
                   + cp[4] * DT5;
            }
            return VE * sc.MW;
        }


        static double PlainReverse(Temperature Tk, BaseComp sc)
        {
            double VE = 0;
            double Tk1 = Tk;
            double Tk2 = Tk1 * Tk1; // slightly faster
            double[] cp = sc.IdealVapCP;

            if (cp != null && cp.Length >= 5)
            {
                VE = cp[4] * (Tk1 * Tk2 * Tk2 - BaseT5) / 5
                   + cp[3] * (Tk2 * Tk2 - BaseT4) / 4
                   + cp[2] * (Tk2 * Tk1 - BaseT3) / 3
                   + cp[1] * (Tk2 - BaseT2) / 2
                   + cp[0] * (Tk1 - BaseT1);
            }
            return VE * sc.MW;
        }

        static double PlainPreRangeCheckReverse(Temperature Tk, BaseComp sc)
        {
            double VE = 0;
            double Tk1 = Tk;
            double Tk2 = Tk1 * Tk1; // slightly faster
            double[] cp = sc.IdealVapCP;

            if (cp != null && cp.Length >= 5)
            {
                VE = cp[4] * (Tk1 * Tk2 * Tk2 - BaseT5) / 5
                   + cp[3] * (Tk2 * Tk2 - BaseT4) / 4
                   + cp[2] * (Tk2 * Tk1 - BaseT3) / 3
                   + cp[1] * (Tk2 - BaseT2) / 2
                   + cp[0] * (Tk1 - BaseT1);
            }
            return VE * sc.MW;
        }

        static double PlainFlat(Temperature Tk, BaseCompFlat sc)
        {
            double Tk1 = Tk;
            double Tk2 = Tk1 * Tk1;
            double VE = sc.IdealVapCP0 * (Tk1 - BaseT1)
                      + sc.IdealVapCP1 * (Tk2 - BaseT2) / 2
                      + sc.IdealVapCP2 * (Tk2 * Tk1 - BaseT3) / 3
                      + sc.IdealVapCP3 * (Tk2 * Tk2 - BaseT4) / 4
                      + sc.IdealVapCP4 * (Tk2 * Tk2 * Tk1 - BaseT5) / 5;
            return VE * sc.MW;
        }

        static double PlainPreRangeCheckPassAllDTsFlat(Temperature Tk, BaseComp sc)
        {

            double VE = 0;

            VE = sc.v1 * DT1
               + sc.v2 * DT2
               + sc.v3 * DT3
               + sc.v4 * DT4
               + sc.v5 * DT5;

            return VE * sc.MW;
        }


        [Benchmark] public void UseLoops() => StandardRun(UseLoops);
        [Benchmark] public void UseLoopsPreRangeCheck() => StandardRun(UseLoopsPreRangeCheck);
        [Benchmark] public void Plain0() => StandardRun(Plain0);
        [Benchmark] public void Plain() => StandardRun(Plain);
        [Benchmark] public void PlainPreRangeCheck() => StandardRun(PlainPreRangeCheck);
        [Benchmark] public void PlainReverse() => StandardRun(PlainReverse);
        [Benchmark] public void PlainPreRangeCheckReverse() => StandardRun(PlainPreRangeCheckReverse);
        [Benchmark] public void PlainPreRangeCheckPassAllTks() => StandardRun(PlainPreRangeCheckPassAllTks);

        [Benchmark] public void PlainFlat() => StandardRun(PlainFlat);

        [Benchmark] public void PlainPreRangeCheckPassAllDTsFlat() => StandardRun(PlainPreRangeCheckPassAllDTsFlat);


        public static void Main()
        {
            Console.WriteLine("Benchmarking...");
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}