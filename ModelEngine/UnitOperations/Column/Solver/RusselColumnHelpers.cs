using Extensions;
using Math2;
using MathNet.Numerics.LinearAlgebra;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace RusselColumn
{
    public partial class RusselSolver
    {
        /// <summary>
        /// calculates Keq at Temperature  T
        /// </summary>
        /// <param name="section"></param>
        /// <param name="trayNo"></param>
        /// <param name="comp"></param>
        /// <param name="T"></param>
        /// <return  s></return  s>

        private void Tridiag(Column cd)
        {
            int NoComps = cd.NoComps;
            int TotNoTrays = cd.TotNoStages;

            double[][] temp = new double[NoComps][];
            for (int comp = 0; comp < NoComps; comp++)
                temp[comp] = TDMA.TriDiag.TDMASolve(TrDMatrix[comp],FeedMolarCompFlowsTotal[comp], TotNoTrays);

            for (int comp = 0; comp < NoComps; comp++)
            {
                for (int tray = 0; tray < TotNoTrays; tray++)
                    CompLiqMolarFlows[tray][comp] = temp[comp][tray];
            }
        }

        private void TridiagMod(Column cd)
        {
            int NoComps = cd.NoComps;
            int TotNoTrays = cd.TotNoStages;

            //DumpArrayExcel.Dump2DArray("TRDMAtrix", TrDMatrix[0], TRDMAtrixFile, true, 2, 1);
            //DumpArrayExcel.Dump1DArray("Feed", FeedMolarCompFlowsTotal[0], TRDMAtrixFile, 2, 1);
            //DumpArrayExcel.Dump1DArray("Temp", temp[0], TRDMAtrixFile, 2, 1);

            List<Point> UPoints = new List<Point>();
            List<Point> LPoints = new List<Point>();
            double[] X = new double[TrDMatrix.Length];

            for (int r = 0; r < TrDMatrix[0].Length; r++)
            {
                for (int c = 0; c < TrDMatrix[0].Length; c++)
                {
                    if (r > c + 1 && TrDMatrix[0][r][c] != 0)
                        LPoints.Add(new Point(r, c));
                    if (r < c - 1 && TrDMatrix[0][r][c] != 0)
                        UPoints.Add(new Point(r, c));
                }
            }

            double[][] temp = new double[NoComps][];

            double[][] Matrix = null;

            for (int comp = 0; comp < NoComps; comp++)
                temp[comp] = ModifiedThomasAlgorithm.Solve4(TrDMatrix[comp], (double[])FeedMolarCompFlowsTotal.GetColumn(comp), LPoints, UPoints, out Matrix);

            //DumpArrayExcel.Dump2DArray("TRDMAtrixMod", TrDMatrix[0], TRDMAtrixFile, true, 2, 1);
            //DumpArrayExcel.Dump2DArray("TRDMAtrixTest", Matrix, TRDMAtrixFile, true, 2, 1);
            //DumpArrayExcel.Dump1DArray("Feed", FeedMolarCompFlowsTotal[0], TRDMAtrixFile, 2, 1);
            //DumpArrayExcel.Dump1DArray("Temp", temp[0], TRDMAtrixFile, 2, 1);

            for (int comp = 0; comp < NoComps; comp++)
            {
                for (int tray = 0; tray < TotNoTrays; tray++)
                    CompLiqMolarFlows[tray][comp] = temp[comp][tray];
            }
        }

        private void FullMatrixInversion(Column cd)
        {
            /* for (int  n = 0; n < NoComps; n++)
             {
                 double [] B = (double [])FeedMolarCompFlowsTotal[n].Clone();
                 alglib.rmatrixsolvefast(TrDMatrix[n].ToMultiArray(), TrDMatrix[n].Length, B, out int  info);

                 for (int  i = 0; i < B.Length; i++)
                 {
                     CompLiqMolarFlows[i][n] = B[i];
                     if (double .IsInfinity(C[i]))
                     {
                         Debugger.Break();
                         ViewArray va = new ();
                         va.View(TrDMatrix[n]);
                     }
                 }
             }*/

            if (NoComps < 20)
            {
                for (int n = 0; n < NoComps; n++)
                {
                    var A = Matrix<double>.Build.DenseOfRowArrays(TrDMatrix[n]);
                    var B = Vector<double>.Build.DenseOfArray(FeedMolarCompFlowsTotal[n]);
                    var C = A.Solve(B).ToArray();
                    //ViewArray va = new ();
                    // va.View(TrDMatrix[n]);
                    for (int i = 0; i < C.Length; i++)
                    {
                        CompLiqMolarFlows[i][n] = C[i];
                    }
                }
            }
            else
            {
                Parallel.For(0, NoComps, n =>
                {
                    var A = Matrix<double>.Build.DenseOfRowArrays(TrDMatrix[n]);
                    var B = Vector<double>.Build.DenseOfArray(FeedMolarCompFlowsTotal[n]);
                    var C = A.Solve(B).ToArray();

                    for (int i = 0; i < C.Length; i++)
                    {
                        CompLiqMolarFlows[i][n] = C[i];
                        /* if (double .IsInfinity(C[i]))
                        {
                            Debugger.Break();
                            ViewArray va = new ();
                            va.View(TrDMatrix[n]);
                        }*/
                    }
                });
            }
        }

        public double SumArraySquared(double[] arr)
        {
            double sum = 0;
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                    sum += Math.Pow(arr[i], 2);
            }
            return sum;
        }
    }
}