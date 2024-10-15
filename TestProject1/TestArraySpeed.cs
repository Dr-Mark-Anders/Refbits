using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Units;
using Units.UOM;

namespace UnitTests
{
    [TestClass]
    public class TestArraySpeed2
    {
        [TestMethod]
        public void TestClassVsStruct()
        {
            Temperature TStruct = new Temperature();
            TemperatureClass Tclass = new TemperatureClass();
            // temp
            var watch = Stopwatch.StartNew();

            double[] L = new double[1000000];

            for (int i = 0; i < L.Length; i++)
            {
                L[i] = TStruct * TStruct;
            }

            watch.Stop();
            MessageBox.Show(watch.ElapsedMilliseconds.ToString() + " " + L.Sum().ToString());

            watch = Stopwatch.StartNew();

            L = new double[1000000];

            for (int i = 0; i < L.Length; i++)
            {
                L[i] = Tclass.T * Tclass.T;
            }

            watch.Stop();
            MessageBox.Show(watch.ElapsedMilliseconds.ToString() + " " + L.Sum().ToString());
        }

        [TestClass]
        public class TestArraySpeed
        {
            [TestMethod]
            public void MyTestForLoop()
            {
                var watch = Stopwatch.StartNew();

                double[] L = new double[1000000];

                for (int i = 0; i < L.Length; i++)
                {
                    L[i] = Math.Exp(2d);
                }

                watch.Stop();
                MessageBox.Show(watch.ElapsedMilliseconds.ToString() + " " + L.Sum().ToString());
            }

            [TestMethod]
            public void MyTestForLoopFloat()
            {
                var watch = Stopwatch.StartNew();

                float[] L = new float[1000000];

                for (int i = 0; i < L.Length; i++)
                {
                    L[i] = (float)Math.Exp(2f);
                }

                watch.Stop();
                MessageBox.Show(watch.ElapsedMilliseconds.ToString() + " " + L.Sum().ToString());
            }

            [TestMethod]
            public void MyTestForLoop2()
            {
                var watch = Stopwatch.StartNew();
                int n = 1000000;

                double[] L = new double[n];

                for (int i = 0; i < n; i++)
                {
                    L[i] = Math.Pow(2.0, 3);
                }

                watch.Stop();
                MessageBox.Show(watch.ElapsedMilliseconds.ToString() + " " + L.Sum().ToString());
            }
        }
    }
}