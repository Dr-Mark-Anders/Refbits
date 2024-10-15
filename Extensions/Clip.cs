namespace Extensions
{
    public partial class Ext
    {
        public static double[] Clip(double[] x, double lowBound, double highBound)
        {
            double[] res = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] < lowBound)
                    res[i] = (double)lowBound;
                else if (x[i] > highBound)
                    res[i] = (double)highBound;
                else
                    res[i] = x[i];
            }
            return x;
        }

        public static double Clip(double x, double lowBound, double highBound)
        {
            double res = 0;
            if (x < lowBound)
                res = (double)lowBound;
            else
                res = x;

            if (x > highBound)
                res = (double)highBound;
            else
                res = x;

            return res;
        }

        public static double[] Clip(double[] x, double[] lowBound, double[] highBound)
        {
            double[] res = new double[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] < lowBound[i])
                    res[i] = lowBound[i];
                else if (x[i] > highBound[i])
                    res[i] = highBound[i];
                else
                    res[i] = x[i];
            }

            return res;
        }

        public static double[][] Clip(double[][] x, double lowBound, double highBound)
        {
            double[][] res = new double[x.Length][];

            for (int i = 0; i < x.GetUpperBound(0); i++)
            {
                res[i] = new double[x.GetUpperBound(0)];

                for (int y = 0; y < x.GetUpperBound(1); y++)
                {
                    if (x[i][y] < lowBound)
                        res[i][y] = (double)lowBound;
                    else
                        res[i][y] = x[i][y];

                    if (x[i][y] > highBound)
                        res[i][y] = (double)highBound;
                    else
                        res[i][y] = x[i][y];
                }
            }
            return res;
        }
    }
}