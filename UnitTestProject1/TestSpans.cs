using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace UnitTests
{
    /// <summary>
    /// Summary description for UnitTest4
    /// </summary>
    [TestClass]
    public class TestSpans
    {
        /*[DllImport("kernel32.dll", SetLastError = true)]
         [return: MarshalAs(UnmanagedType.Bool)]
         static extern bool AllocConsole();*/

        public TestSpans()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        [TestMethod]
        public void TestMethodForProfile()
        {
            int n = 100;
            var xy = MakeRandomOneDimensionalArray(n);
            var ai = MakeRandomOneDimensionalArray(n);
            var kijm = MakeRandomTwoDimensionalArray(n);
            var aij = new double[n, n];
            var sumai = new double[n];

            //var res1 = TimeIt("Original Method", () => CalcAmixOld(n, xy, ai, kijm, aij, sumai));
            // var res2 = TimeIt("Original pointers", () => CalcAmixInner(n, xy, ai, kijm, aij, sumai));
            // var res3 = TimeIt("Back to arrays", () => CalcAmixArraysInner(n, xy, ai, kijm, aij, sumai));
            //var res4 = TimeIt("Back to arrays 2", () => CalcAmixArraysInner2(n, xy, ai, kijm, aij, sumai));
            // var res5 = TimeIt("Back to arrays 3", () => CalcAmixArraysInner3(n, xy, ai, kijm, aij, sumai));
            var res6 = TimeIt("Back to arrays 3a", () => CalcAmixArraysInner3a(n, xy, ai, kijm, aij, sumai));
            // var res7 = TimeIt("Spans", () => CalcAmixSpansInner(n, xy, ai, kijm, aij, sumai));
            // var res8 = TimeIt("Spans 2", () => CalcAmixSpansInner2(n, xy, ai, kijm, aij, sumai));
            // var res9 = TimeIt("Spans 3", () => CalcAmixSpansInner3(n, xy, ai, kijm, aij, sumai));
        }

        [TestMethod]
        public void TestMethod1()
        {
            int n = 50;
            var xy = MakeRandomOneDimensionalArray(n);
            var ai = MakeRandomOneDimensionalArray(n);
            var kijm = MakeRandomTwoDimensionalArray(n);
            var aij = new double[n, n];
            var sumai = new double[n];

            var res1 = TimeIt("Original Method", () => CalcAmixOld(n, xy, ai, kijm, aij, sumai));
            var res2 = TimeIt("Original pointers", () => CalcAmixInner(n, xy, ai, kijm, aij, sumai));
            var res3 = TimeIt("Back to arrays", () => CalcAmixArraysInner(n, xy, ai, kijm, aij, sumai));
            var res4 = TimeIt("Back to arrays 2", () => CalcAmixArraysInner2(n, xy, ai, kijm, aij, sumai));
            var res5 = TimeIt("Back to arrays 3", () => CalcAmixArraysInner3(n, xy, ai, kijm, aij, sumai));
            var res6 = TimeIt("Back to arrays 3a", () => CalcAmixArraysInner3a(n, xy, ai, kijm, aij, sumai));
            var res7 = TimeIt("Spans", () => CalcAmixSpansInner(n, xy, ai, kijm, aij, sumai));
            var res8 = TimeIt("Spans 2", () => CalcAmixSpansInner2(n, xy, ai, kijm, aij, sumai));
            var res9 = TimeIt("Spans 3", () => CalcAmixSpansInner3(n, xy, ai, kijm, aij, sumai));

            MessageBox.Show(
           "Original Method " + res1 + "\n"
               + "Original pointers " + res2 + "\n"
               + "Back to arrays " + res3 + "\n"
               + "Back to arrays 2 " + res4 + "\n"
               + "Back to arrays 3 " + res5 + "\n"
               + "Back to arrays 3a " + res6 + "\n"
               + "Spans " + res7 + "\n"
               + "Spans 2 " + res8 + "\n"
               + "Spans 3 " + res9 + "\n");

            res1 = CalcAmixOld(n, xy, ai, kijm, aij, sumai);
            res2 = CalcAmixInner(n, xy, ai, kijm, aij, sumai);
            res3 = CalcAmixArraysInner(n, xy, ai, kijm, aij, sumai);
            res4 = CalcAmixArraysInner2(n, xy, ai, kijm, aij, sumai);
            res5 = CalcAmixArraysInner3(n, xy, ai, kijm, aij, sumai);
            res6 = CalcAmixArraysInner3a(n, xy, ai, kijm, aij, sumai);
            res7 = CalcAmixSpansInner(n, xy, ai, kijm, aij, sumai);
            res8 = CalcAmixSpansInner2(n, xy, ai, kijm, aij, sumai);
            res9 = CalcAmixSpansInner3(n, xy, ai, kijm, aij, sumai);

            MessageBox.Show(
                "Original Method " + res1 + "\n"
                + "Original pointers " + res2 + "\n"
                + "Back to arrays " + res3 + "\n"
                + "Back to arrays 2 " + res4 + "\n"
                + "Back to arrays 3 " + res5 + "\n"
                + "Back to arrays 3a " + res6 + "\n"
                + "Spans " + res7 + "\n"
                + "Spans 2 " + res8 + "\n"
                + "Spans 3 " + res9 + "\n");
        }

        static public double CalcAmixOld(int n, double[] XY, double[] ai, double[,] kijm, double[,] Aij, double[] sumAi)
        {
            // Debug.Print(": CalcAmix");
            //
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa
            //
            double amix = 0;
            Aij = new double[n, n];
            sumAi = new double[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        Aij[i, j] = ai[i];
                    //else if (i > j)
                    //    Aij[i, j] = Aij[j, i];
                    else
                        Aij[i, j] = Math.Sqrt(ai[i] * ai[j]) * (1 - kijm[i, j]);
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    sumAi[i] += XY[j] * Aij[i, j];

                amix += sumAi[i] * XY[i];
            }

            return amix;
        }

        static public unsafe double CalcAmixInner(int n, double[] XY, double[] ai, double[,] kijm, double[,] Aij, double[] sumAi)
        {
            // Debug.Print(": CalcAmix");
            //
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa

            double amix = 0;
            Aij = new double[n, n];
            sumAi = new double[n];
            int UpperBound0 = Aij.GetUpperBound(0);
            int UpperBound1 = Aij.GetUpperBound(1);

            fixed (double* ptr = Aij, aiPtr = ai, kijmPtr = kijm)
            {
                double* elementIJ = ptr;
                double* kijelement = kijmPtr;
                double* aielement = aiPtr;
                for (int i = 0; i <= UpperBound0; i++)
                {
                    for (int j = 0; j <= UpperBound1; j++)
                    {
                        if (i == j)
                            *elementIJ = *aielement;
                        //else if (i > j)
                        //    *elementIJ = *(ptr + j* UpperBound1 + i);
                        else
                            *elementIJ = Math.Sqrt(*(aiPtr + i) * *(aiPtr + j)) * (1 - *kijelement);

                        elementIJ++;
                        kijelement++;
                        //elementJI++;
                    }
                    aielement++;
                }
            }

            fixed (double* ptrAij = Aij, ptrXY = XY, ptrsumAI = sumAi)
            {
                double* elementAij = ptrAij;
                double* elementsumAI = ptrsumAI;
                double* elementXy = ptrXY;

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        *elementsumAI += *elementXy * *elementAij;
                        elementXy++;
                        elementAij++;
                    }

                    amix += *elementsumAI * *(ptrXY + i);
                    elementXy = ptrXY;
                    elementsumAI++;
                }
            }

            return amix;
        }

        static public double CalcAmixArraysInner(int n, double[] XY, double[] ai, double[,] kijm, double[,] Aij, double[] sumAi)
        {
            sumAi = new double[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        Aij[i, j] = ai[i];
                    else
                        Aij[i, j] = Math.Sqrt(ai[i] * ai[j]) * (1 - kijm[i, j]);
                }
            }

            double amix = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    sumAi[i] += XY[j] * Aij[i, j];

                amix += sumAi[i] * XY[i];
            }
            return amix;
        }

        static public double CalcAmixArraysInner2(int n, double[] XY, double[] ai, double[,] kijm, double[,] Aij, double[] sumAi)
        {
            for (int i = 0; i < n; i++)
            {
                var aii = ai[i];
                for (int j = 0; j < n; j++)
                    Aij[i, j] = Math.Sqrt(aii * ai[j]) * (1 - kijm[i, j]);

                Aij[i, i] = aii;
            }

            double amix = 0;
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                    sum += XY[j] * Aij[i, j];

                sumAi[i] = sum;
                amix += sum * XY[i];
            }
            return amix;
        }

        static public double CalcAmixArraysInner3(int n, double[] XY, double[] ai, double[,] kijm, double[,] Aij, double[] sumAi)
        {
            double amix = 0;

            for (int i = 0; i < n; i++)
            {
                var aii = ai[i];
                double sum = XY[i] * aii;

                for (int j = 0; j < n; j++)
                {
                    var value = Math.Sqrt(aii * ai[j]) * (1 - kijm[i, j]);

                    Aij[i, j] = value;
                    sum += XY[j] * value;
                }

                sum -= XY[i] * Aij[i, i];
                Aij[i, i] = aii;

                sumAi[i] = sum;
                amix += sum * XY[i];
            }

            return amix;
        }

        static unsafe public double CalcAmixArraysInner3a(int n, double[] XY, double[] ai, double[,] kijm, double[,] Aij, double[] sumAi)
        {
            double amix = 0;

            fixed (double* ptr = Aij, aiPtr = ai, kijmPtr = kijm, XYPtr = XY)
            {
                double value;
                double* elementIJ = ptr;
                double* kijelement = kijmPtr;
                double* aielement = aiPtr;
                double* JI = ptr;
                double* XYelement = XYPtr;

                for (int i = 0; i < n; i++)
                {
                    var aii = ai[i];
                    double sum = XY[i] * aii;

                    for (int j = 0; j < n; j++)
                    {
                        if (i > j)
                        {
                            value = Math.Sqrt(aii * ai[j]) * (1 - *kijelement);
                            *(JI + i * n + j) = value;
                        }
                        else
                        {
                            value = Math.Sqrt(aii * *aielement) * (1 - *kijelement);
                            *elementIJ = value;
                        }

                        sum += *XYelement * value;

                        aielement++;
                        kijelement++;
                        elementIJ++;
                        XYelement++;
                    }

                    XYelement = XYPtr;
                    aielement = aiPtr;

                    sum -= XY[i] * Aij[i, i];
                    Aij[i, i] = aii;

                    sumAi[i] = sum;
                    amix += sum * XY[i];
                }
            }

            return amix;
        }

        static public double CalcAmixSpansInner(int n,
                                                ReadOnlySpan<double> XY,
                                                ReadOnlySpan<double> ai,
                                                double[,] kijm,
                                                double[,] Aij,
                                                Span<double> sumAi)
        {
            for (int i = 0; i < n; i++)
            {
                var aii = ai[i];
                for (int j = 0; j < n; j++)
                    Aij[i, j] = Math.Sqrt(aii * ai[j]) * (1 - kijm[i, j]);

                Aij[i, i] = aii;
            }

            double amix = 0;
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                    sum += XY[j] * Aij[i, j];

                sumAi[i] = sum;
                amix += sum * XY[i];
            }
            return amix;
        }

        static public unsafe double CalcAmixSpansInner2(int n,
                                                 ReadOnlySpan<double> XY,
                                                 ReadOnlySpan<double> ai,
                                                 double[,] kijm,
                                                 double[,] Aij,
                                                 Span<double> sumAi)
        {
            fixed (double* kijmPtr = kijm, AijPtr = Aij)
            {
                var kijmSpan = new ReadOnlySpan<double>(kijmPtr, kijm.Length);
                var kijmRowISpan = kijmSpan.Slice(0);
                var AijSpan = new Span<double>(AijPtr, Aij.Length);
                var AijRowISpan = AijSpan.Slice(0);

                for (int i = 0; i < n; i++)
                {
                    var aii = ai[i];
                    for (int j = 0; j < n; j++)
                        AijRowISpan[j] = Math.Sqrt(aii * ai[j]) * (1 - kijmRowISpan[j]);

                    AijRowISpan[i] = aii;
                    kijmRowISpan = kijmRowISpan.Slice(n);
                    AijRowISpan = AijRowISpan.Slice(n);
                }

                double amix = 0;
                AijRowISpan = AijSpan.Slice(0);
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                        sum += XY[j] * AijRowISpan[j];

                    sumAi[i] = sum;
                    amix += sum * XY[i];
                    AijRowISpan = AijRowISpan.Slice(n);
                }
                return amix;
            }
        }

        static public unsafe double CalcAmixSpansInner3(int n,
                                                        ReadOnlySpan<double> XY,
                                                        ReadOnlySpan<double> ai,
                                                        double[,] kijm,
                                                        double[,] Aij,
                                                        Span<double> sumAi)
        {
            fixed (double* kijmPtr = kijm, AijPtr = Aij)
            {
                var kijmSpan = new ReadOnlySpan<double>(kijmPtr, kijm.Length);
                var kijmRowISpan = kijmSpan.Slice(0);
                var AijSpan = new Span<double>(AijPtr, Aij.Length);
                var AijRowISpan = AijSpan.Slice(0);
                double amix = 0;

                for (int i = 0; i < n; i++)
                {
                    var aii = ai[i];
                    double sum = XY[i] * aii;

                    for (int j = 0; j < n; j++)
                    {
                        var value = Math.Sqrt(aii * ai[j]) * (1 - kijmRowISpan[j]);
                        AijRowISpan[j] = value;
                        sum += XY[j] * value;
                    }

                    sum -= XY[i] * AijRowISpan[i];
                    AijRowISpan[i] = aii;

                    sumAi[i] = sum;
                    amix += sum * XY[i];

                    kijmRowISpan = kijmRowISpan.Slice(n);
                    AijRowISpan = AijRowISpan.Slice(n);
                }

                return amix;
            }
        }

        static Random _random = new Random(123);

        static double[] MakeRandomOneDimensionalArray(int n)
            => Enumerable.Range(0, n)
                         .Select(i => _random.NextDouble())
                         .ToArray();

        static double[,] MakeRandomTwoDimensionalArray(int n)
        {
            var arr = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    arr[i, j] = _random.NextDouble();
            return arr;
        }

        static double TimeIt(string caption, Action action)
        {
            const int iterationCount = 50000;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterationCount; i++)
                action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
