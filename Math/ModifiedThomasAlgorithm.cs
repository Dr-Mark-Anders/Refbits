using Extensions;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Drawing;

namespace Math2
{
    public class ModifiedThomasAlgorithm
    {
        public static double[] Solve(double[][] Matrix, double[] F)
        {
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

            int PointRow = 0;
            int PointCol = 0;
            int activerow = 0;
            double Mult = 0;

            for (int i = 0; i < LPoints.Count; i++)
            {
                PointRow = LPoints[i].X;
                PointCol = LPoints[i].Y;
                for (int row = 0; row < PointRow - PointCol - 1; row++)
                {
                    activerow = PointCol + row;
                    Mult = Matrix[PointRow][PointCol + row] / Matrix[activerow][PointCol + row]; // move down and allong one place
                    for (int col = 0; col < PointRow - PointCol + 1; col++)
                    {
                        Matrix[PointRow][PointCol + col] = Matrix[PointRow][PointCol + col] - Matrix[activerow][PointCol + col] * Mult;
                    }
                    F[PointRow] = F[PointRow] - F[activerow] * Mult;
                }
            }

            //string TRDMAtrixFile = "C:\\Users\\andersm\\Documents\\Visual Studio 2017\\Projects\\TRDMatrix.xlsx";
            //DumpArrayToExcel DumpArrayExcel = new  ErrorReporting.DumpArrayToExcel();
            //DumpArrayExcel.Dump2DArray("TRDMAtrixMod", Matrix, TRDMAtrixFile, true, 2, 1);

            double[] Bb = new double[Matrix.Length];
            double[] Cc = new double[Matrix.Length];
            double[] Ff = new double[Matrix.Length];

            int startrow, NoOfPoints;
            double[][] p = new double[UPoints.Count][];

            for (int n = 0; n < UPoints.Count; n++)
            {
                for (int i = 0; i < Matrix.Length; i++)
                {
                    p[n] = new double[Matrix.Length];
                }
            }

            Point po;

            Bb[0] = 1;
            Cc[0] = Matrix[0][1] / Matrix[0][0];
            Ff[0] = F[0] / Bb[0];

            for (int i = 1; i < Matrix.Length; i++)
            {
                Bb[i] = 1 / (Matrix[i][i] - Matrix[i][i - 1] * Cc[i - 1]);
                if (i < Matrix.Length - 1)
                    Cc[i] = Bb[i] * Matrix[i][i + 1];
                Ff[i] = Bb[i] * (F[i] - Matrix[i][i - 1] * Ff[i - 1]);
                for (int P = 0; P < UPoints.Count; P++)
                {
                    po = UPoints[P];
                    startrow = po.X;
                    NoOfPoints = po.Y - po.X - 1;

                    if (i == startrow)
                        p[P][startrow] = Matrix[po.X][po.Y] * Bb[i];

                    if (startrow < i && i < startrow + NoOfPoints)
                    {
                        p[P][i] = p[P][i - 1] * Bb[i];
                    }
                    if (i == startrow + NoOfPoints)
                        Cc[i] -= Bb[i] * (p[P][i - 1] * Matrix[i][i - 1]);
                }
            }

            // Back fill

            X[Matrix.Length - 1] = Ff[Matrix.Length - 1];

            for (int i = Matrix.Length - 2; i >= 0; i--)
            {
                X[i] = Ff[i] - Cc[i] * X[i + 1];

                for (int Pnt = 0; Pnt < UPoints.Count; Pnt++)
                {
                    X[i] -= p[Pnt][i] * X[UPoints[Pnt].Y];  // multiply by x to give contributions
                }
            }

            return X;
        }

        public static double[] Solve2(double[][] Matrix, double[] F, List<Point> LPoints, List<Point> UPoints)
        {
            //List<Point> UPoints = new  List<Point>();
            //List<Point> LPoints = new  List<Point>();
            double[] X = new double[Matrix.Length];
            int size = Matrix.Length;

            /*for (int  r = 0; r < Matrix.Length; r++)
            {
                for (int  c = 0; c < Matrix[0].Length; c++)
                {
                    if (r > c + 1 && Matrix[r][c] != 0)
                        LPoints.Add(new  Point(r, c));
                    if (r < c - 1 && Matrix[r][c] != 0)
                        UPoints.Add(new  Point(r, c));
                }
            }*/

            int PointRow = 0;
            int PointCol = 0;
            int activerow = 0;
            double Mult = 0;

            for (int i = 0; i < LPoints.Count; i++)
            {
                for (int row = 0; row < PointRow - PointCol - 1; row++)
                {
                    activerow = PointRow - PointCol + row;
                    Mult = Matrix[PointRow][PointCol + row] / Matrix[activerow][PointCol + row]; // move down and allong one place
                    for (int col = 0; col < PointRow - PointCol + 1; col++)
                    {
                        Matrix[PointRow][PointCol + col] = Matrix[PointRow][PointCol + col] - Matrix[activerow][PointCol + col] * Mult;
                    }
                    F[PointRow] = F[PointRow] - F[activerow] * Mult;
                }
            }

            double[] Bb = new double[size];
            double[] Cc = new double[size];
            double[] Ff = new double[size];

            int startrow, NoOfPoints;
            double[][] p = new double[UPoints.Count][];

            for (int n = 0; n < UPoints.Count; n++)
            {
                for (int i = 0; i < Matrix.Length; i++)
                {
                    p[n] = new double[size];
                }
            }

            Point po;

            Bb[0] = 1;
            Cc[0] = Matrix[0][1] / Matrix[0][0];
            Ff[0] = F[0] / Bb[0];

            for (int i = 1; i < size; i++)
            {
                Bb[i] = 1 / (Matrix[i][i] - Matrix[i][i - 1] * Cc[i - 1]);
                if (i < Matrix.Length - 1)
                    Cc[i] = Bb[i] * Matrix[i][i + 1];
                Ff[i] = Bb[i] * (F[i] - Matrix[i][i - 1] * Ff[i - 1]);
                for (int P = 0; P < UPoints.Count; P++)
                {
                    po = UPoints[P];
                    startrow = po.X;
                    NoOfPoints = po.Y - po.X - 1;

                    if (i == startrow)
                        p[P][startrow] = Matrix[po.X][po.Y] * Bb[i];

                    if (startrow < i && i < startrow + NoOfPoints)
                    {
                        p[P][i] = p[P][i - 1] * Bb[i];
                    }
                    if (i == startrow + NoOfPoints)
                        Cc[i] -= Bb[i] * (p[P][i - 1] * Matrix[i][i - 1]);
                }
            }

            // Back fill

            X[size - 1] = Ff[size - 1];

            for (int i = size - 2; i >= 0; i--)
            {
                X[i] = Ff[i] - Cc[i] * X[i + 1];

                for (int Pnt = 0; Pnt < UPoints.Count; Pnt++)
                {
                    if (p[Pnt][i] != 0) // makes no difference to time
                        X[i] -= p[Pnt][i] * X[UPoints[Pnt].Y];  // multiply by x to give contributions
                }
            }

            return X;
        }

        public static double[] Solve3(double[][] MatrixIn, double[] Fin, List<Point> LPoints, List<Point> UPoints)
        {
            //List<Point> UPoints = new List<Point>();
            //List<Point> LPoints = new List<Point>();
            double[][] Matrix = (double[][])MatrixIn.CloneDeep();
            double[] F = (double[])Fin.Clone();

            double[] X = new double[Matrix.Length];
            int size = Matrix.Length;

            /*for (int  r = 0; r < Matrix.Length; r++)
            {
                for (int  c = 0; c < Matrix[0].Length; c++)
                {
                    if (r > c + 1 && Matrix[r][c] != 0)
                        LPoints.Add(new  Point(r, c));
                    if (r < c - 1 && Matrix[r][c] != 0)
                        UPoints.Add(new  Point(r, c));
                }
            }*/

            int PointRow = 0;
            int PointCol = 0;
            int activerow = 0;
            int NoIterations = 0;
            double Mult = 0;

            for (int i = 0; i < LPoints.Count; i++) // do sub diagonal elements, MUST do in right order
            {
                PointRow = LPoints[i].X;
                PointCol = LPoints[i].Y;
                activerow = PointCol + 1;

                NoIterations = PointRow - PointCol - 1;

                for (int iter = 0; iter < NoIterations; iter++)
                {
                    Mult = Matrix[activerow + iter][PointCol + iter] / Matrix[PointRow][PointCol + iter];

                    for (int col = iter; col < size - PointCol; col++)
                    {
                        Matrix[PointRow][PointCol + col] = Matrix[PointRow][PointCol + col] * Mult - Matrix[activerow + iter][PointCol + col];
                    }
                    F[PointRow] = F[PointRow] * Mult - F[activerow + iter];
                }
            }

            double[] Bb = new double[size];
            double[] Cc = new double[size];
            double[] Ff = new double[size];

            int startrow, NoOfPoints;
            double[][] p = new double[UPoints.Count][];

            for (int n = 0; n < UPoints.Count; n++)
            {
                for (int i = 0; i < Matrix.Length; i++)
                {
                    p[n] = new double[size];
                }
            }

            Point po;

            Bb[0] = 1 / Matrix[0][0];
            Cc[0] = Matrix[0][1] / Matrix[0][0];
            Ff[0] = F[0] * Bb[0];

            for (int i = 0; i < size; i++)
            {
                if (i == 0)
                {
                    Bb[0] = 1 / Matrix[0][0];
                    Cc[0] = Matrix[0][1] / Matrix[0][0];
                    Ff[0] = F[0] * Bb[0];
                }
                else
                {
                    Bb[i] = 1 / (Matrix[i][i] - Matrix[i][i - 1] * Cc[i - 1]);

                    if (i < Matrix.Length - 1)
                        Cc[i] = Bb[i] * Matrix[i][i + 1];

                    Ff[i] = Bb[i] * (F[i] - Matrix[i][i - 1] * Ff[i - 1]);
                }

                for (int P = 0; P < UPoints.Count; P++)
                {
                    po = UPoints[P];
                    startrow = po.X;
                    NoOfPoints = po.Y - po.X - 1;

                    if (i == startrow)
                        p[P][startrow] = Matrix[po.X][po.Y] * Bb[i];

                    if (startrow < i && i < startrow + NoOfPoints)
                    {
                        p[P][i] = p[P][i - 1] * Bb[i];
                    }
                    if (i == startrow + NoOfPoints)
                        Cc[i] -= Bb[i] * (p[P][i - 1] * Matrix[i][i - 1]);
                }
            }

            // Back fill

            X[size - 1] = Ff[size - 1];

            for (int i = size - 2; i >= 0; i--)
            {
                X[i] = Ff[i] - Cc[i] * X[i + 1];

                for (int Pnt = 0; Pnt < UPoints.Count; Pnt++)
                {
                    if (p[Pnt][i] != 0) // makes no difference to time
                        X[i] -= p[Pnt][i] * X[UPoints[Pnt].Y];  // multiply by x to give contributions
                }
            }

            return X;
        }

        public static double[] Solve4(double[][] MatrixIn, double[] Fin, List<Point> LPoints, List<Point> UPoints, out double[][] Matrix)
        {
            //List<Point> UPoints = new  List<Point>();
            //List<Point> LPoints = new  List<Point>();
            Matrix = (double[][])MatrixIn.CloneDeep();
            double[] F = (double[])Fin.Clone();

            double[] X = new double[Matrix.Length];
            int size = Matrix.Length;

            /*for (int  r = 0; r < Matrix.Length; r++)
            {
                for (int  c = 0; c < Matrix[0].Length; c++)
                {
                    if (r > c + 1 && Matrix[r][c] != 0)
                        LPoints.Add(new  Point(r, c));
                    if (r < c - 1 && Matrix[r][c] != 0)
                        UPoints.Add(new  Point(r, c));
                }
            }*/

            int PointRow = 0;
            int PointCol = 0;
            int activerow = 0;
            int NoIterations = 0;
            double Mult = 0;

            for (int i = 0; i < LPoints.Count; i++) // do sub diagonal elements, MUST do in right order
            {
                PointRow = LPoints[i].X;
                PointCol = LPoints[i].Y;
                activerow = PointCol + 1;

                NoIterations = PointRow - PointCol - 1;

                for (int iter = 0; iter < NoIterations; iter++)
                {
                    Mult = Matrix[activerow + iter][PointCol + iter] / Matrix[PointRow][PointCol + iter];

                    for (int col = iter; col < size - PointCol; col++)
                    {
                        Matrix[PointRow][PointCol + col] = Matrix[PointRow][PointCol + col] * Mult - Matrix[activerow + iter][PointCol + col];
                    }
                    F[PointRow] = F[PointRow] * Mult - F[activerow + iter];
                }
            }

            double[] Bb = new double[size];
            double[] Cc = new double[size];
            double[] Ff = new double[size];

            int startrow, NoOfPoints;
            double[][] p = new double[UPoints.Count][];

            for (int n = 0; n < UPoints.Count; n++)
            {
                for (int i = 0; i < Matrix.Length; i++)
                {
                    p[n] = new double[size];
                }
            }

            Point po;

            Bb[0] = 1 / Matrix[0][0];
            Cc[0] = Matrix[0][1] / Matrix[0][0];
            Ff[0] = F[0] * Bb[0];

            for (int i = 0; i < size; i++)
            {
                if (i == 0)
                {
                    Bb[0] = 1 / Matrix[0][0];
                    Cc[0] = Matrix[0][1] / Matrix[0][0];
                    Ff[0] = F[0] * Bb[0];
                }
                else
                {
                    Bb[i] = 1 / (Matrix[i][i] - Matrix[i][i - 1] * Cc[i - 1]);

                    if (i < Matrix.Length - 1)
                        Cc[i] = Bb[i] * Matrix[i][i + 1];

                    Ff[i] = Bb[i] * (F[i] - Matrix[i][i - 1] * Ff[i - 1]);
                }

                for (int P = 0; P < UPoints.Count; P++)
                {
                    po = UPoints[P];
                    startrow = po.X;
                    NoOfPoints = po.Y - po.X - 1;

                    if (i == startrow)
                        Matrix[P][startrow] = Matrix[po.X][po.Y] * Bb[i];
                    //p[P][startrow] = Matrix[po.X][po.Y] * Bb[i];

                    if (startrow < i && i < startrow + NoOfPoints)
                    {
                        Matrix[P][i] = Matrix[P][i - 1] * Bb[i];
                    }
                    if (i == startrow + NoOfPoints)
                        Cc[i] -= Bb[i] * (p[P][i - 1] * Matrix[i][i - 1]);
                }
            }

            // Back fill

            X[size - 1] = Ff[size - 1];

            for (int i = size - 2; i >= 0; i--)
            {
                X[i] = Ff[i] - Cc[i] * X[i + 1];

                for (int Pnt = 0; Pnt < UPoints.Count; Pnt++)
                {
                    if (p[Pnt][i] != 0) // makes no difference to time
                        X[i] -= Matrix[Pnt][i] * X[UPoints[Pnt].Y];  // multiply by x to give contributions
                }
            }

            return X;
        }

        public static double[] SolveFinal(double[][] MatrixIn, double[] Fin, List<Point> LPoints, List<Point> UPoints, out double[][] Matrix)
        {
            //List<Point> UPoints = new  List<Point>();
            //List<Point> LPoints = new  List<Point>();
            Matrix = (double[][])MatrixIn.CloneDeep();
            double[] F = (double[])Fin.Clone();

            double[] X = new double[Matrix.Length];
            int size = Matrix.Length;

            int PointRow = 0;
            int PointCol = 0;
            int activerow = 0;
            int NoIterations = 0;
            double Mult = 0;

            for (int i = 0; i < LPoints.Count; i++) // do sub diagonal elements, MUST do in right order
            {
                PointRow = LPoints[i].X;
                PointCol = LPoints[i].Y;
                activerow = PointCol + 1;

                NoIterations = PointRow - PointCol - 1;

                for (int iter = 0; iter < NoIterations; iter++)
                {
                    Mult = Matrix[activerow + iter][PointCol + iter] / Matrix[PointRow][PointCol + iter];

                    for (int col = iter; col < size - PointCol; col++)
                    {
                        Matrix[PointRow][PointCol + col] = Matrix[PointRow][PointCol + col] * Mult - Matrix[activerow + iter][PointCol + col];
                    }
                    F[PointRow] = F[PointRow] * Mult - F[activerow + iter];
                }
            }

            double[] Bb = new double[size];
            double[] Cc = new double[size];
            double[] Ff = new double[size];

            int startrow, NoOfPoints;
            double[][] p = new double[UPoints.Count][];

            for (int n = 0; n < UPoints.Count; n++)
            {
                for (int i = 0; i < Matrix.Length; i++)
                {
                    p[n] = new double[size];
                }
            }

            Point po;

            Bb[0] = 1 / Matrix[0][0];
            Cc[0] = Matrix[0][1] / Matrix[0][0];
            Ff[0] = F[0] * Bb[0];

            for (int i = 0; i < size; i++)
            {
                if (i == 0)
                {
                    Bb[0] = 1 / Matrix[0][0];
                    Cc[0] = Matrix[0][1] / Matrix[0][0];
                    Ff[0] = F[0] * Bb[0];
                }
                else
                {
                    Bb[i] = 1 / (Matrix[i][i] - Matrix[i][i - 1] * Cc[i - 1]);

                    if (i < Matrix.Length - 1)
                        Cc[i] = Bb[i] * Matrix[i][i + 1];

                    Ff[i] = Bb[i] * (F[i] - Matrix[i][i - 1] * Ff[i - 1]);
                }

                for (int P = 0; P < UPoints.Count; P++)
                {
                    po = UPoints[P];
                    startrow = po.X;
                    NoOfPoints = po.Y - po.X - 1;

                    if (i == startrow)
                        Matrix[P][startrow] = Matrix[po.X][po.Y] * Bb[i];
                    //p[P][startrow] = Matrix[po.X][po.Y] * Bb[i];

                    if (startrow < i && i < startrow + NoOfPoints)
                    {
                        Matrix[P][i] = Matrix[P][i - 1] * Bb[i];
                    }
                    if (i == startrow + NoOfPoints)
                        Cc[i] -= Bb[i] * (p[P][i - 1] * Matrix[i][i - 1]);
                }
            }

            // Back fill

            X[size - 1] = Ff[size - 1];

            for (int i = size - 2; i >= 0; i--)
            {
                X[i] = Ff[i] - Cc[i] * X[i + 1];

                for (int Pnt = 0; Pnt < UPoints.Count; Pnt++)
                {
                    if (p[Pnt][i] != 0) // makes no difference to time
                        X[i] -= Matrix[Pnt][i] * X[UPoints[Pnt].Y];  // multiply by x to give contributions
                }
            }
            return X;
        }

        public static void testNetNumericsinversion(Matrix<double> A, Vector<double> B, ref Vector<double> x)
        {
            x = A.Solve(B);
        }
    }
}