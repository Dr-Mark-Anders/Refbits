using EngineThermo;
using Math2;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Units;
using Units.InsideOutExchanger;

namespace UnitTests
{
    [TestClass]
    public class TestThermodynamics
    {
        [TestMethod] // very fast
        public void TestModifiedThomasAlgorithm()
        {
            double[][] Matrix = new double[15][]
           {new double[15] { 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] {-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0}};

            double[] F = new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] res;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here


            for (int i = 0; i < 10000; i++)
            {
                res = ModifiedThomasAlgorithm.Solve(Matrix, F);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            System.Windows.Forms.MessageBox.Show(elapsedMs.ToString());

            return;
        }

        [TestMethod] // very fast
        public void TestModifiedThomasAlgorithm2()
        {
            double[][] Matrix = new double[15][]
           {new double[15] { 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] {-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0},
            new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0}};

            double[] F = new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] res;

            List<Point> UPoints = new List<Point>();
            List<Point> LPoints = new List<Point>();
            double[] X = new double[Matrix.Length];

            for (int r = 0; r < Matrix.Length; r++)
            {
                for (int c = 0; c < Matrix[0].Length; c++)
                {
                    if (r > c + 1 && Matrix[r][c] != 0)
                        LPoints.Add(new Point(r, c));
                    if (r < c - 1 && Matrix[r][c] != 0)
                        UPoints.Add(new Point(r, c));
                }
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 10000; i++)
            {
                res = ModifiedThomasAlgorithm.Solve2(Matrix, F, LPoints, UPoints);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            System.Windows.Forms.MessageBox.Show(elapsedMs.ToString());

            return;
        }

        [TestMethod] // very fast
        public void TestModifiedThomasAlgorithm3()
        {
            double[][] Matrix = new double[28][]
           {new double[28] { 1.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] {-1.0, 1.0, 1.0, 0.0, 0.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0}};

            double[] F = new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.1, 0.0, 0.0, 0.01, 0.0, 0.0, 0.01 };
            double[] res;

            List<Point> UPoints = new List<Point>();
            List<Point> LPoints = new List<Point>();
            double[] X = new double[Matrix.Length];

            for (int r = 0; r < Matrix.Length; r++)
            {
                for (int c = 0; c < Matrix[0].Length; c++)
                {
                    if (r > c + 1 && Matrix[r][c] != 0)
                        LPoints.Add(new Point(r, c));
                    if (r < c - 1 && Matrix[r][c] != 0)
                        UPoints.Add(new Point(r, c));
                }
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            for (int i = 0; i < 10000; i++)
            {
                res = ModifiedThomasAlgorithm.Solve3(Matrix, F, LPoints, UPoints);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            System.Windows.Forms.MessageBox.Show(elapsedMs.ToString());

            return;
        }

        [TestMethod] // fast
        public void TestNetNumerics()
        {
            double[][] Matrix = new double[28][]
           {new double[28] { 1.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] {-1.0, 1.0, 1.0, 0.0, 0.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0},
            new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0}};
            double[] F = new double[28] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.1, 0.0, 0.0, 0.01, 0.0, 0.0, 0.01 };

            Vector<double> res = Vector<double>.Build.DenseOfArray(F);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            var A = Matrix<double>.Build.DenseOfRowArrays(Matrix);
            var B = Vector<double>.Build.DenseOfArray(F);

            //MathNet.Numerics.Control.UseNativeMKL();         //176 ms
            //MathNet.Numerics.Control.UseNativeOpenBLAS();  // 1627 ms
            MathNet.Numerics.Control.UseManaged();         // 162 ms

            //MathNet.Numerics.Control.LinearAlgebraProvider;

            for (int i = 0; i < 10000; i++)
            {
                ModifiedThomasAlgorithm.testNetNumericsinversion(A, B, ref res);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            System.Windows.Forms.MessageBox.Show(elapsedMs.ToString());

            return;
        }

        /* [TestMethod]
        public void TestExtreme()
        {

            double[] MM = new double[225]{ 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
           -1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0, 0.0,
            0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0, 1.0,
            0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,-1.0, 1.0};

            //DenseMatrix<double> M = new DenseMatrix<double>( 15, 15, MM, Extreme.Mathematics.MatrixElementOrder.NotApplicable, false);

            //DenseVector<double> F = Vector new double[15] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            //DenseVector.


            Vector V = new Vector(VectorType.Column, F);

            Matrix res;
            for (int i = 0; i < 1000; i++)
            {
                DenseMatrix<double> Mm = new DenseMatrix<double>(15, 15, M, Extreme.Mathematics.MatrixElementOrder.NotApplicable, false);
            }

            return;
        }*/

        [TestMethod]
        public void TestH_Keq()
        {
            Thermodata data = new Thermodata();

            //enumCalcResult cres = enumCalcResult.Converged;
            double[] res;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("n-Butane");
            sc.molefraction = 0.9;
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("Hydrogen");
            sc.molefraction = 0.1;
            cc.Add(sc);

            cc.thermo.KMethod = enumEquiKMethod.PR76;

            cc.T = new Temperature(25);
            cc.P = new Pressure(6);

            res = Thermodynamics.KMix(cc);
        }

        [TestMethod]
        public void TestPSat()
        {
            Thermodata data = new Thermodata();

            double res = 0;
            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("n-Butane");
            sc.molefraction = 0.5;
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("propane");
            sc.molefraction = 0.5;
            cc.Add(sc);

            cc.T = new Temperature(25);
            cc.P = new Pressure(6);
            res = Thermodynamics.CalcBubblePointP(cc, enumEquiKMethod.PR78);
        }


        [TestMethod]
        public void TestPCKWilson()
        {
            Thermodata data = new Thermodata();
            double[] X = new double[1];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR76;
            thermo.KMethod = enumEquiKMethod.Wilson;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;
            thermo.MW_Method = enumMW_Method.LeeKesler;

            double[] res;
            Components cc = new Components();

            PseudoComponent pc = new PseudoComponent(0.615049, new Temperature(311.15), new Temperature(311.15), new Temperature(311.15), "Test", thermo);
            pc.Name = "PC" + cc.components.Count.ToString();
            pc.molefraction = 1;
            cc.Add(pc);

            X[0] = 1;

            cc.T = new Temperature(370);
            cc.P = new Pressure(6);

            res = Thermodynamics.KMix(cc, X, X);
        }


        [TestMethod]
        public void TestKMixAllComponenets()
        {
            double[] res;
            Components cc = new Components();
            BaseComp sc;

            Thermodata data = new Thermodata();

            string[] c = new string[]{"H2O","Hydrogen","Nitrogen","CO","Oxygen","Methane","Ethylene","Ethane","CO2","H2S","Propene","Propane","i-Butane",
            "n-Butane","i-Pentane","n-Pentane","Quasi38*","Quasi45*","Quasi55*","Quasi65*","Quasi75*","Quasi85*","Quasi95*","Quasi105*","Quasi115*","Quasi125*",
            "Quasi135*","Quasi145*","Quasi155*","Quasi165*","Quasi175*","Quasi185*","Quasi195*","Quasi205*","Quasi215*","Quasi225*","Quasi235*","Quasi245*",
            "Quasi255*","Quasi265*","Quasi275*","Quasi285*","Quasi295*","Quasi305*","Quasi315*","Quasi325*","Quasi335*","Quasi345*","Quasi355*","Quasi365*",
            "Quasi375*","Quasi385*","Quasi395*","Quasi405*","Quasi415*","Quasi425*","Quasi435*","Quasi445*","Quasi460*","Quasi480*","Quasi500*","Quasi520*",
            "Quasi540*","Quasi560*","Quasi580*","Quasi600*","Quasi620*","Quasi640*","Quasi660*","Quasi680*","Quasi700*","Quasi720*","Quasi740*","Quasi775*",
            "Quasi825*","Quasi875*"};

            //double[] x = new double[]  {0.11962, 0.00190, 0.00000, 0.00000, 0.00000, 0.00355, 0.00000, 0.01813, 0.00000, 0.00099, 0.00000, 0.01070, 0.02054, 0.01243, 0.02686, 0.04282, 0.00051, 0.00818, 0.01996, 0.01489, 0.01575, 0.02810, 0.02886, 0.04733, 0.01624, 0.03913, 0.02958, 0.03639, 0.03162, 0.03278, 0.02920, 0.03165, 0.02464, 0.01935, 0.02301, 0.02842, 0.02424, 0.02049, 0.02778, 0.01301, 0.01428, 0.01195, 0.00419, 0.00193, 0.00027, 0.00004, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000 };
            //double[] x = new double[] { 0.135732774414621, 0.00215468506415423, 0, 0, 0, 0.0040308022232848, 0, 0.0205695504978483, 0, 0.00112867577145799, 0, 0.012145088837949, 0.0233015561421689, 0.0141024421399901, 0.0304793692227981, 0.0485837079279351, 0.000575215436192718, 0.0092822005770224, 0.0226435863400046, 0.0168918871952106, 0.0178767122687875, 0.0318902910790664, 0.0327534194713751, 0.0537023657943689, 0.0184311210311017, 0.0444009322636192, 0.0335596469258279, 0.0412962160913424, 0.0358771869940697, 0.0371930155690997, 0.0331300014439141, 0.035914641309358, 0.0279557907376905, 0.021952113307905, 0.0261134905810872, 0.0322528765686226, 0.0275004545362876, 0.0232519132782944, 0.0315204595917805, 0.0147578354952504, 0.0162052371926885, 0.0135548574764113, 0.00474909354016855, 0.00219263685340534, 0.000302596460158383, 4.18107775868331E-05, 1.53775892680625E-06, 1.91804783680153E-07, 1.07983510399437E-08, 1.11511408933161E-09, 8.63899097261722E-11, 6.12581908949474E-12, 3.54050464074921E-13, 4.58746710586419E-14, 3.03699384603848E-15, 2.51208135433161E-16, 1.86238776871088E-17, 1.02074204103091E-18, 2.81077119585737E-20, 1.81203359796247E-22, 3.63679596787214E-25, 1.09412087254464E-27, 2.22854161297157E-30 };
            double[] x = new double[] { 0.986074, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000006, 0.000000, 0.000005, 0.000000, 0.000773, 0.009770, 0.003116, 0.000163, 0.000083, 0.000001, 0.000005, 0.000003, 0.000001, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000 };
            double[] sg = new double[] { 1, 0.06992, 0.80712, 0.80013, 1.13874, 0.29967, 0.38358, 0.35601, 0.82610, 0.78914, 0.52144, 0.50715, 0.56249, 0.58377, 0.62402, 0.63031, 0.61505, 0.63181, 0.65515, 0.67719, 0.69724, 0.71462, 0.72864, 0.73863, 0.74390, 0.74539, 0.75097, 0.76004, 0.77054, 0.78041, 0.78757, 0.79003, 0.79329, 0.80040, 0.80916, 0.81738, 0.82290, 0.82423, 0.82671, 0.83135, 0.83742, 0.84419, 0.85095, 0.85696, 0.86150, 0.86383, 0.86476, 0.86833, 0.87408, 0.88118, 0.88880, 0.89612, 0.90231, 0.90654, 0.91222, 0.91518, 0.91866, 0.92236, 0.92771, 0.93286, 0.93419, 0.93675, 0.94200, 0.94962, 0.95925, 0.97056, 0.98320, 0.99685, 1.01116, 1.02553, 1.03532, 1.04510, 1.05489, 1.07201, 1.09648, 1.12094 };
            double[] bp = new double[] { 373.15, 20.5548, 77.3498, 81.6996, 90.1996, 111.625, 169.399, 184.55, 194.59801, 213.498, 225.399, 231.048, 261.42001, 272.6480103, 301.02802, 309.20901, 311.15, 318.15, 328.15, 338.15, 348.15, 358.15, 368.15, 378.15, 388.15, 398.15, 408.15, 418.15, 428.15, 438.15, 448.15, 458.15, 468.15, 478.15, 488.15, 498.15, 508.15, 518.15, 528.15, 538.15, 548.15, 558.15, 568.15, 578.15, 588.15, 598.15, 608.15, 618.15, 628.15, 638.15, 648.15, 658.15, 668.15, 678.15, 688.15, 698.15, 708.15, 718.15, 733.15, 753.15, 773.15, 793.15, 813.15, 833.15, 853.15, 873.15, 893.15, 913.15, 933.15, 953.15, 973.15, 993.15, 1013.15, 1048.15, 1098.15, 1148.15 };

            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.Ideal;
            thermo.CritPMethod = enumCritPMethod.TWU;
            thermo.CritTMethod = enumCritTMethod.TWU;
            thermo.OmegaMethod = enumOmegaMethod.LeeKesler;

            for (int i = 0; i < x.Length; i++)
            {
                sc = Thermodata.GetRealComponent(c[i]);
                if (sc == null)
                {
                    PseudoComponent pc = new PseudoComponent(new Temperature(bp[i]), sg[i], thermo);
                    pc.molefraction = x[i];
                    cc.Add(pc);
                }
                else
                {
                    sc.molefraction = x[i];
                    cc.Add(sc);
                }
            }
            cc.T = new Temperature(100);
            cc.P = new Pressure(6);
            res = Thermodynamics.KMix(cc, x, x);
            return;
        }

        [TestMethod]
        public void KMix()
        {
            double[] res;
            Components cc = new Components();
            BaseComp sc;
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.KMethod = enumEquiKMethod.PR76;
            //Debugger.Launch();

            sc = Thermodata.GetRealComponent("n-Butane");
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("Hydrogen");
            cc.Add(sc);

            double[] x = new double[] { 0.9, 0.1 };
            double[] y = new double[] { 0.9, 0.1 };

            cc.T = new Temperature(25);
            cc.P = new Pressure(6);
            res = Thermodynamics.KMix(cc, cc.T, cc.P, x, y, out enumFluidRegion state);
            //return res;
        }




        [TestMethod]
        public void TestFlash()
        {
            Thermodata data = new Thermodata();

            double[] X = new double[2];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.LeeKesler;

            Components cc = new Components();
            BaseComp sc;

            sc = Thermodata.GetRealComponent("propane");
            sc.molefraction = 0.5;
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("n-Butane");
            sc.molefraction = 0.4;
            cc.Add(sc);

            sc = Thermodata.GetRealComponent("n-C30");
            sc.molefraction = 0.1;
            cc.Add(sc);

            Port_Material p = new Port_Material();
            p.components.Add(cc);
            p.components.Origin = SourceEnum.Input;
            p.T = new StreamProperty(ePropID.T, 273.15 + 100);
            p.P.Value = new StreamProperty(ePropID.P, 6);

            p.Flash();

            Debug.Print(p.components.LiquidComponents[0].molefraction.ToString());
            Debug.Print(p.components.LiquidComponents[1].molefraction.ToString());
            Debug.Print(p.components.LiquidComponents[2].molefraction.ToString());
        }


        [TestMethod]
        public void TestPCCreation()
        {
            DistPoint a = new DistPoint(1, new Temperature(145.0));
            DistPoint b = new DistPoint(5, new Temperature(158.0));
            DistPoint c = new DistPoint(10, new Temperature(164.0));
            DistPoint d = new DistPoint(20, new Temperature(169.0));
            DistPoint e = new DistPoint(30, new Temperature(172.0));
            DistPoint f = new DistPoint(50, new Temperature(179.0));
            DistPoint g = new DistPoint(70, new Temperature(185.0));
            DistPoint h = new DistPoint(80, new Temperature(189.0));
            DistPoint i = new DistPoint(90, new Temperature(195.0));
            DistPoint j = new DistPoint(95, new Temperature(200.0));
            DistPoint k = new DistPoint(99, new Temperature(213.0));
            DistPoints points = new DistPoints();
            points.Add(a);
            points.Add(b);
            points.Add(c);
            points.Add(d);
            points.Add(e);
            points.Add(f);
            points.Add(g);
            points.Add(h);
            points.Add(i);
            points.Add(j);
            points.Add(k);

            //double[] res = DistillationConvert.GetPseudoCompLVfromDistPoints(BoilingPoints.i(), points,1,false);
        }

        [TestMethod]
        public void TestFugacity2()
        {
            Thermodata data = new Thermodata();
            Excel_Thermo ethermo = new Excel_Thermo();

            double[] X = new double[3];
            double[] Y = new double[3];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR78;

            double[] res = new double[3];
            string[] names = new string[3];

            names[0] = "propane";
            names[1] = "n-butane";
            names[2] = "n-C30";

            X[0] = 0.95;
            X[1] = 0.05;
            X[2] = 0.0;

            Y[0] = 0.99;
            Y[1] = 0.01;
            Y[2] = 0.0;

            // res = ethermo.LnKMix(names, X, Y, (273.15 + 25), 6, (int)enumEnthalpy.PR78);

            res = (double[])ethermo.LnkRealMix(names, X, Y, (273.15 + 100), 6, (int)enumEnthalpy.PR78);


            //            Debug.Print(res.ToString() + " " + res2.ToString());
        }

        [TestMethod]
        public void TestFugacity()
        {
            Thermodata data = new Thermodata();
            Excel_Thermo ethermo = new Excel_Thermo();

            double[] X = new double[3];
            double[] Y = new double[3];
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            thermo.Enthalpy = enumEnthalpy.PR78;

            double[] res = new double[3];
            string[] names = new string[3];

            names[0] = "propane";
            names[1] = "n-butane";
            names[2] = "n-C30";

            X[0] = 0.5;
            X[1] = 0.5;
            X[2] = 0.0;

            Y[0] = 0.5;
            Y[1] = 0.5;
            Y[2] = 0.0;

            // res = ethermo.LnKMix(names, X, Y, (273.15 + 25), 6, (int)enumEnthalpy.PR78);

            res = (double[])ethermo.LnkRealMix(names, X, Y, (273.15 + 100), 6, (int)enumEnthalpy.PR78);


            //            Debug.Print(res.ToString() + " " + res2.ToString());
        }

        [TestMethod]
        public void TestIOExchanger()
        {
            UOMProperty cin = new UOMProperty(ePropID.T, 100);
            UOMProperty cout = new UOMProperty(ePropID.T, 120);
            UOMProperty hsin = new UOMProperty(ePropID.T, 150);
            UOMProperty hsout = new UOMProperty(ePropID.T, 130);


            cout.Source = SourceEnum.Transferred;
            hsin.Source = SourceEnum.Transferred;
            hsout.Source = SourceEnum.Empty;
            cin.Source = SourceEnum.Empty;

            IOExchanger ex = new IOExchanger(2, 3, hsin, hsout, cin, cout, true);

            ex.Init();
            ex.setconstraints(constraints.CSIN, 100, constraints.HSOUT, 130);
            ex.calcerrors();

            do
            {
                ex.CalcGradients();
                ex.createjacobian();

                ex.InverseJacobian();
                ex.UpdateDutyperKG();
                ex.Init();
                ex.calcerrors();
            }
            while (ex.geterror() > 0);
        }

        [TestMethod]
        public void TestPolyComp()
        {
            PolyCompressor pc = new PolyCompressor();
            pc.PolyCompCalcs(500, new Temperature(101.9), 41.5, new Temperature(40.18), 2.068, 9.0, 0.7, 45.5, 1.4);
            Debug.Print(pc.Polytropicgaspower.ToString());
            //Debug.Print(pc.);
        }

        [TestMethod]
        public void TestMethaneTRAPP1()
        {
            ElyHanley test = new ElyHanley();
            test.TRAPP("METHANE", 100, 1);
        }

        [TestMethod]
        public void TestButaneTRAPP1()
        {
            ElyHanley test = new ElyHanley();
            test.TRAPP("N-BUTANE", 0, 6);
            test.TRAPP("N-BUTANE", 300, 6);
        }

        [TestMethod]
        public void TestTRAPP2()
        {
            Thermodata data = new Thermodata();

            BaseComp bc = Thermodata.GetRealComponent("n-Butane");
            ElyHanley test = new ElyHanley();

            test.TRAPP2(bc, 25, 6, out double visc, out double thermc, out double den);
            //test.TRAPP2(11.61, 0.2550, 425.20, 0.2753, 200, 1);
        }

        [TestMethod]
        public void TestTRAPPMethane()
        {
            Thermodata data = new Thermodata();

            BaseComp bc = Thermodata.GetRealComponent("methane");
            ElyHanley test = new ElyHanley();

            test.TRAPP2(bc, new Temperature(100), 1, out double visc, out double thermc, out double den);
        }

        [TestMethod]
        public void TestDistConversion()
        {
            DistPoints res;
            DistPoint a = new DistPoint(1, new Temperature(-2.00));
            DistPoint b = new DistPoint(5, new Temperature(26.00));
            DistPoint c = new DistPoint(10, new Temperature(36.00));
            DistPoint d = new DistPoint(20, new Temperature(68.00));
            DistPoint e = new DistPoint(30, new Temperature(86.00));
            DistPoint f = new DistPoint(50, new Temperature(109.00));
            DistPoint g = new DistPoint(70, new Temperature(136.00));
            DistPoint h = new DistPoint(80, new Temperature(148.00));
            DistPoint i = new DistPoint(90, new Temperature(161.00));
            DistPoint j = new DistPoint(95, new Temperature(168.00));
            DistPoint k = new DistPoint(99, new Temperature(177.00));
            DistPoints points = new DistPoints();
            points.Add(a);
            points.Add(b);
            points.Add(c);
            points.Add(d);
            points.Add(e);
            points.Add(f);
            points.Add(g);
            points.Add(h);
            points.Add(i);
            points.Add(j);
            points.Add(k);

            enumDistType from = enumDistType.D86, to = enumDistType.TBP_VOL;
            res = DistillationConversions.Convert(from, to, points);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(res[n].BP.ToString());

            res = DistillationConversions.Convert(to, from, res);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(res[n].BP.ToString());

            from = enumDistType.D1160;
            to = enumDistType.TBP_VOL;
            res = DistillationConversions.Convert(from, to, points);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(res[n].BP.ToString());

            res = DistillationConversions.Convert(to, from, res);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(res[n].BP.ToString());

            from = enumDistType.D2887;
            to = enumDistType.TBP_VOL;
            res = DistillationConversions.Convert(from, to, points);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(res[n].BP.ToString());

            res = DistillationConversions.Convert(to, from, res);

            for (int n = 0; n < points.Count; n++)
                Debug.Print(res[n].BP.ToString());

        }

        [TestMethod]
        public void TestDistConversion2()
        {
            DistPoint a = new DistPoint(1, new Temperature(-48.19));
            DistPoint c = new DistPoint(10, new Temperature(7.78));
            DistPoint e = new DistPoint(30, new Temperature(76.17));
            DistPoint f = new DistPoint(50, new Temperature(109.36));
            DistPoint g = new DistPoint(70, new Temperature(143.29));
            DistPoint i = new DistPoint(90, new Temperature(173.22));
            DistPoint k = new DistPoint(99, new Temperature(187.24));
            DistPoints points = new DistPoints();
            points.Add(a);
            //points.Add(b);
            points.Add(c);
            points.Add(e);
            points.Add(f);
            points.Add(g);
            //points.Add(h);
            points.Add(i);
            //points.Add(j);
            points.Add(k);
            Debug.Print(Math2.CubicSpline.CubSpline(eSplineMethod.Constrained, 95D, points.getPCTs(), points.getBPs()).ToString());
        }

        [TestMethod]
        public void TestNormInv()
        {
            double val = 0;
            val = Probability.NormSDist(0.5);
            val = Probability.NormSInv(val);
        }

        [TestMethod]
        public void TestCubicSpline()
        {
            DistPoint a = new DistPoint(1, new Temperature(-2.00));
            DistPoint b = new DistPoint(5, new Temperature(26.00));
            DistPoint c = new DistPoint(10, new Temperature(36.00));
            DistPoint d = new DistPoint(20, new Temperature(68.00));
            DistPoint e = new DistPoint(30, new Temperature(86.00));
            DistPoint f = new DistPoint(50, new Temperature(109.00));
            DistPoint g = new DistPoint(70, new Temperature(136.00));
            DistPoint h = new DistPoint(80, new Temperature(148.00));
            DistPoint i = new DistPoint(90, new Temperature(161.00));
            DistPoint j = new DistPoint(95, new Temperature(168.00));
            DistPoint k = new DistPoint(99, new Temperature(177.00));
            DistPoints points = new DistPoints();
            points.Add(a);
            points.Add(b);
            points.Add(c);
            points.Add(d);
            points.Add(e);
            points.Add(f);
            points.Add(g);
            points.Add(h);
            points.Add(i);
            points.Add(j);
            points.Add(k);

            Debug.Print(Math2.CubicSpline.CubSpline(eSplineMethod.Normal, 150D, points.getPCTs(), points.getBPs()).ToString());
        }


        [TestMethod]
        public void TestPCCreation2()
        {
            DistPoint a = new DistPoint(1, new Temperature(355));
            DistPoint b = new DistPoint(5, new Temperature(391));
            DistPoint c = new DistPoint(10, new Temperature(411));
            DistPoint d = new DistPoint(20, new Temperature(436));
            DistPoint e = new DistPoint(30, new Temperature(454));
            DistPoint f = new DistPoint(50, new Temperature(484));
            DistPoint g = new DistPoint(70, new Temperature(514));
            DistPoint h = new DistPoint(80, new Temperature(531));
            DistPoint i = new DistPoint(90, new Temperature(553));
            DistPoint j = new DistPoint(95, new Temperature(574));
            DistPoint k = new DistPoint(99, new Temperature(624));
            DistPoints points = new DistPoints();
            points.Add(a);
            points.Add(b);
            points.Add(c);
            points.Add(d);
            points.Add(e);
            points.Add(f);
            points.Add(g);
            points.Add(h);
            points.Add(i);
            points.Add(j);
            points.Add(k);

            //double[] res = DistillationConvert.GetPseudoCompLVfromDistPoints(Global.BoilingPoints.DegCtoDegK(), points,1,false);
        }

        [TestMethod]
        public void TestPCCreation3()
        {
            ThermoDynamicOptions thermo = new ThermoDynamicOptions();
            PseudoComponent pc = new PseudoComponent(new Temperature(600), 0.9706, thermo);
        }

        [TestMethod]
        public void TestTWU()
        {
            TWU.Calc(new Temperature(600), 0.9706);
            Debug.Print("MW " + TWU.MW.ToString());
            Debug.Print("Tc " + TWU.Tc.ToString());
            Debug.Print("Pc " + TWU.Pc.ToString());
            Debug.Print("Vc " + TWU.Vc.ToString());
            Debug.Print("Zc " + TWU.Zc.ToString());
            Debug.Print("Omega " + TWU.Omega.ToString());
        }
    }
}
