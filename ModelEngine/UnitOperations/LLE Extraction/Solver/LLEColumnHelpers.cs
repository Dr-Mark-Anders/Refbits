using Math2;
using MathNet.Numerics.LinearAlgebra;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace LLE_Solver
{
    public partial class LLESolver
    {
        /// <summary>
        /// calculates Keq at Temperature  T
        /// </summary>
        /// <param name="section"></param>
        /// <param name="trayNo"></param>
        /// <param name="comp"></param>
        /// <param name="T"></param>
        /// <return  s></return  s>

        public double GetRootFromThreePoints(double X1, double Y1, double X2, double Y2, double X3, double Y3)
        {
            double a, b, c, R1, R2;

            a = ((Y2 - Y1) * (X1 - X3) + (Y3 - Y1) * (X2 - X1)) / ((X1 - X3) * (X2 * X2 - X1 * X1) + (X2 - X1) * (X3 * X3 - X1 * X1));
            b = ((Y2 - Y1) - a * (X2 * X2 - X1 * X1)) / (X2 - X1);
            c = Y1 - a * X1 * X1 - b * X1;

            R1 = (-b + (Math.Sqrt(b * b - 4 * a * c))) / 2 / a;
            R2 = (-b - (Math.Sqrt(b * b - 4 * a * c))) / 2 / a;

            /*if (a < 0.00001)
                return   R1;
            else
                return   R2;*/

            if (R1 < X3 && R1 > X2)
            {
                return R1;
            }
            else if (R2 < X3 && R2 > X2)
            {
                return R2;
            }
            else
            {
                return R1;
            }
        }

        private void Tridiag(IColumn cd)
        {
            int NoComps = cd.NoComps;
            int TotNoTrays = cd.TotNoStages;

            double[][] temp = new double[NoComps][];
            for (int comp = 0; comp < NoComps; comp++)
                temp[comp] = TDMA.TriDiag.TDMASolve(TrDMatrix[comp], FeedMolarCompFlowsTotal[comp], TotNoTrays);

            for (int comp = 0; comp < NoComps; comp++)
            {
                for (int tray = 0; tray < TotNoTrays; tray++)
                    CompLiqMolarFlows[tray][comp] = temp[comp][tray];
            }
        }

        private void TridiagMod(IColumn cd)
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
                temp[comp] = ModifiedThomasAlgorithm.Solve4(TrDMatrix[comp], FeedMolarCompFlowsTotal[comp], LPoints, UPoints, out Matrix);

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

        private void FullMatrixInversion(IColumn cd)
        {
            if (NoComps < 20)
            {
                for (int n = 0; n < NoComps; n++)
                {
                    var A = Matrix<double>.Build.DenseOfRowArrays(TrDMatrix[n]);
                    var B = Vector<double>.Build.DenseOfArray(FeedMolarCompFlowsTotal[n]);
                    var C = A.Solve(B).ToArray();

                    for (int i = 0; i < C.Length; i++)
                        CompLiqMolarFlows[i][n] = C[i];
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
                        CompLiqMolarFlows[i][n] = C[i];
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