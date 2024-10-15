namespace Extensions
{
    public static partial class Ext
    {
        public static double[] Take(this double[] arr, int[] locs)
        {
            double[] res = new double[locs.Length];

            for (int i = 0; i < locs.Length; i++)
            {
                res[i] = arr[locs[i]];
            }
            return res;
        }
    }
}