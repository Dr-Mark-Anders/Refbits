using MatrixAlgebra;
using System.Collections.Generic;

namespace NewtonRaphsonSolver
{
    public class NewtonRaphson
    {
        private int r, c;

        private Matrix Err, BErr;
        private Matrix Grad;

        public NewtonRaphson(int Rows, int Columns)
        {
            r = Rows; c = Columns;
            Err = new Matrix(Rows, Columns);
            BErr = new Matrix(1, Columns);
            Grad = new Matrix(Rows, Columns);
        }

        public double[] Solve(double[] berr, double[,] err, double grad, List<double> currentoffsets)
        {
            double[] res = new double[r];
            AddErrors(err, grad);
            AddBaseError(berr);

            for (int row = 0; row < r; row++)
            {
                for (int col = 0; col < c; col++)
                {
                    Grad[row, col] = (BErr[0, col] - err[row, col]) / grad;
                }
            }

            IMatrix resm;
            if (Grad.Determinant != 0)
                resm = BErr.Multiply(Grad.Inverse);
            else
                return currentoffsets.ToArray();

            for (int i = 0; i < r; i++)
                res[i] = currentoffsets[i] - resm[0, i];

            return res;
        }

        private void AddErrors(double[,] err, double grad)
        {
            for (int i = 0; i < err.GetUpperBound(0); i++)
            {
                for (int j = 0; j < err.GetUpperBound(1); j++)
                {
                    Err[i, j] = err[i, j];
                }
            }
        }

        private void AddBaseError(double[] berr)
        {
            for (int i = 0; i < berr.Length; i++)
            {
                BErr[0, i] = berr[i];
            }
        }
    }
}