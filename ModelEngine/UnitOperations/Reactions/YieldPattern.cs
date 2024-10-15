using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEngine
{
    public class YieldPattern
    {
        string name="";
        Tuple<string[], double[]> yieldPattern;

        public YieldPattern(string name, Tuple<string[], double[]> yieldPattern)
        {
            this.name = name.ToUpper();
            this.yieldPattern = yieldPattern;

            for (int i = 0; i < yieldPattern.Item1.Count(); i++)
            {
                this.yieldPattern.Item1[i] = yieldPattern.Item1[i].ToUpper();
            }
        }

        public string Name { get => name; set => name = value; }

        public int Count { get => yieldPattern.Item1.Count();}

        public string[] Names { get => yieldPattern.Item1; }
        public double[] Yields { get => yieldPattern.Item2; }

    }
}


