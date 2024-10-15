using System.Collections.Generic;

namespace Extensions
{
    public partial class Ext
    {
        public static int[] BoolArray(this int[] a, int b)
        {
            int[] res = new int[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b)
                    res[i] = 1;
                else
                    res[i] = 0;
            }
            return res;
        }

        public static int[] BoolArray(this List<int> a, int b)
        {
            int[] res = new int[a.Count];

            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] == b)
                    res[i] = 1;
                else
                    res[i] = 0;
            }
            return res;
        }
    }
}