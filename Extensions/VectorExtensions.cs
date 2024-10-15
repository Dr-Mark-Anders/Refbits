using System.Drawing;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static Point Zoom(this Point pt, double zoom)
        {
            Point p = new Point((int)(pt.X * zoom), (int)(pt.Y * zoom));
            return p;
        }

        public static double[][] Transpose(this double[][] jarr)
        {
            int row = jarr.Length;
            int col = jarr[0].Length;

            double[][] C = new double[col][];
            for (int i = 0; i < col; i++)
            {
                C[i] = new double[row];
                for (int j = 0; j < row; j++)
                {
                    C[j][i] = jarr[i][j];
                }
            }
            return C;
        }
    }
}