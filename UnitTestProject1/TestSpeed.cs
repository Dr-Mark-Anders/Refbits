using Extensions;
using Math2;
using MathNet.Numerics.RootFinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using RefBitsEquationSolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace UnitTests
{
    [TestClass]
    public class TestSpeed
    {
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

        public double sqrt2(double x)
        {
            return Math.Sqrt(x);
        }

        public double sqrt1(double x)
        {
            int number = Convert.ToInt32(x);
            double root = 1;
            int i = 0;
            //The Babylonian Method for Computing Square Roots
            while (i < 1)
            {
                i = i + 1;
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
            Equation eq = new Equation();
            NonLinearSolver.SolveEquations(eq, SolverType.NewtonRaphson);
        }

        public class Equation : ISolveable
        {
            double[][] J = new double[2][];
            double[] res;
            double[] x = new double[] { 1, 2 };

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

        public class TempClass
        {
            public double value = 100;
            private double v;

            public TempClass(double v)
            {
                this.v = v;
            }

            public static TempClass operator *(TempClass t1, TempClass t2)
            {
                return new TempClass(t1.value * t2.value);
            }
        }

        [TestMethod]
        public void TestDouble()
        {
            double doubleValue = 100;
            Temperature t = new Temperature();
            Temperature Tres = new Temperature(100);
            TempClass tclass = new TempClass(100);
            double res;

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = doubleValue * doubleValue;
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply doubles " + elapsedMs.ToString());

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
            Debug.WriteLine("Multply temperature struct using overloaded * operator " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = tclass.value * tclass.value;
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply class fields " + elapsedMs.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        double exp1(double x)
        {
            x = 1.0 + x / 256.0;
            x *= x; x *= x; x *= x; x *= x;
            x *= x; x *= x; x *= x; x *= x;
            return x;
        }


        double[] T1 = new double[60000000], T2 = new double[60000000], T3 = new double[60000000];


        [TestMethod]
        public void TestExp()
        {
            double doubleValue = 1;
            double res = 0;
            Random r = new Random();
            for (int i = 0; i < 60000000; i++)
            {
                T1[i] = r.NextDouble();
                T2[i] = r.NextDouble();
                T3[i] = r.NextDouble();
            }

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 60000000; i++)
            {
                res += Test(i, 5);
            }
            watch.Stop();
            MessageBox.Show(watch.ElapsedMilliseconds.ToString() + " " + res.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 60000000; i++)
            {
                res += Testafst(i, 5);
            }
            watch.Stop();
            MessageBox.Show(watch.ElapsedMilliseconds.ToString() + " " + res.ToString());
        }

        double Test(int i, double t5)
        {
            double res = 0;
            res += Math.Exp(T1[i] + T2[i] * T3[i]);
            return res;
        }

        double Testafst(int i, double t5)
        {
            double res = 0;
            res += FastExp.Exp(T1[i] + T2[i] * T3[i]);
            return res;
        }

        [TestMethod]
        public void TestLnandExp()
        {
            double doubleValue = 100;
            double res;

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = doubleValue * doubleValue;
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply doubles " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Math.Log(doubleValue);
            }

            watch.Stop();


            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Log " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Math.Exp(Math.Log(doubleValue));
            }

            watch.Stop();


            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp(Log) " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = Math.Exp(Math.Log(doubleValue) + Math.Log(doubleValue));
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
            double doubleValue = 100;
            double res;

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = doubleValue * doubleValue;
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multply doubles " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(doubleValue);
            }

            watch.Stop();


            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Log " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(Math.Log(doubleValue));
            }

            watch.Stop();


            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Exp(Log) " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res = exp1(Math.Log(doubleValue) + Math.Log(doubleValue));
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
        public unsafe void TestDoubleDoubleArrayWithPts()
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

            /* var arraySpan = new Span<double>(Array);

             byte data = 0;
             for (int ctr = 0; ctr < arraySpan.Length; ctr++)
                 arraySpan[ctr] = data++;

             int arraySum = 0;
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
        public unsafe void TestDoubleArrayWithPts()
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
        public void TestDoubleArray()
        {
            double doublevalue = 100;
            Temperature t = new Temperature();
            Temperature Tres = new Temperature(100);
            TempClass tclass = new TempClass(100);
            double[] res = new double[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = doublevalue * doublevalue;
            }

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply doubles into object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = doublevalue * doublevalue;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply doubles into double array" + elapsedMs.ToString());

            watch = Stopwatch.StartNew();
            double res1=0;
            for (int i = 0; i < 10000000; i++)
            {
                res1 += doublevalue * doublevalue;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply doubles into double " + elapsedMs.ToString() +" " + res1.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = (Tres * Tres).BaseValue;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply structs & Load doubles into object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = t.BaseValue * t.BaseValue;
            }

            watch.Stop();


            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply struct fields & Load into object array " + elapsedMs.ToString());


            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = new Temperature(t.BaseValue * t.BaseValue);
            }

            watch.Stop();


            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply struct fields & Load new struct into object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = t * t;
            }

            watch.Stop();

            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply structs with overloaded operator a load struct into object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = tclass.value * tclass.value;
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply class fields & load into object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = new TempClass(tclass.value * tclass.value).value;
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply class fields & load new class into object array " + elapsedMs.ToString());

            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
            {
                res[i] = (tclass * tclass).value;
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply class using overloaded * operator & load class into object array " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestDoubleMult()
        {
            double doublevalue = 100;
            double[] res = new double[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res[i] = doublevalue * doublevalue;


            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply doubles " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestFloatMult()
        {
            float doublevalue = 100;
            float[] res = new float[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res[i] = doublevalue * doublevalue;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply floats " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestDoublDiv()
        {
            double doublevalue = 100;
            double[] res = new double[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res[i] = doublevalue * doublevalue;


            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Div doubles " + elapsedMs.ToString());
        }

        [TestMethod]
        public void TestFloatDiv()
        {
            float doublevalue = 100;
            float[] res = new float[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res[i] = doublevalue / doublevalue;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Div floats " + elapsedMs.ToString() + res.ToString());
        }

        [TestMethod]
        public void TestDoubleMultNoMatrix()
        {
            double doublevalue = 100;
            double res = 0;

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res = doublevalue * doublevalue;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply doubles " + elapsedMs.ToString() + res.ToString());
        }

        [TestMethod]
        public void TestFloatMultNoMatrix()
        {
            float doublevalue = 100;
            float res = 0;

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                res = doublevalue * doublevalue;

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("Multiply floats " + elapsedMs.ToString() + res.ToString());
        }

        [TestMethod]
        public void TestArrayAcces()
        {
            float doublevalue = 100;
            float[] res = new float[10000000];

            Stopwatch watch = Stopwatch.StartNew();
            watch = Stopwatch.StartNew();

            for (int i = 0; i < 10000000; i++)
                doublevalue = res[i];

            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("ArrayAcces " + elapsedMs.ToString() + res[9].ToString());
        }

        [TestMethod]
        public void TestListAcces()
        {
            double doublevalue = 100;
            List<double> res = new List<double>(10000000);

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

