namespace TDMA
{
    public class TriDiag
    {
        //Implementation in C#
        public static double[] TDMASolve(double[][] aa, double[] d, int No)
        {
            double[] a = new double[No];
            double[] b = new double[No];
            double[] c = new double[No];
            double[] dd = (double[])d.Clone();

            for (int i = 0; i < No; i++)
            {
                if (i == 0)
                {
                    b[i] = aa[i][0 + i];
                    c[i] = aa[i][1 + i];
                }
                else if (i < No - 1)
                {
                    a[i] = aa[i][-1 + i];
                    b[i] = aa[i][0 + i];
                    c[i] = aa[i][+1 + i];
                }
                else
                {
                    a[i] = aa[i][-1 + i];
                    b[i] = aa[i][0 + i];
                }
            }

            return TDMASolve(a, b, c, dd);
        }

        public static double[] TDMASolve(double[] a, double[] b, double[] c, double[] d)
        {
            double[] cc = new double[c.Length];
            double[] dd = new double[d.Length];

            double[] x = new double[d.Length];
            int n = d.Length;
            c[0] /= b[0];	/* Division by zero risk. */
            d[0] /= b[0];	/* Division by zero would imply a singular matrix. */
            for (int i = 1; i < n; i++)
            {
                double id = 1 / (b[i] - c[i - 1] * a[i]);  /* Division by zero risk. */
                c[i] *= id;	                         /* Last value calculated is redundant. */
                d[i] = (d[i] - d[i - 1] * a[i]) * id;
            }

            /* Now back substitute. */
            x[n - 1] = d[n - 1];
            for (int i = n - 2; i >= 0; i--)
                x[i] = d[i] - c[i] * x[i + 1];

            return x;
        }
    }
}