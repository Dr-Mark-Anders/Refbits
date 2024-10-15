using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEngine.FCC.KBC
{
    public class NDMCorrelations
    {
        double v;
        double w;
        double ca,cr, cn, cp;
        double rn;

        public double V { get => v; set => v = value; }
        public double W { get => w; set => w = value; }
        public double Ca { get => ca; set => ca = value; }
        public double Cr { get => cr; set => cr = value; }
        public double Cn { get => cn; set => cn = value; }
        public double Cp { get => cp; set => cp = value; }
        public double Rn { get => rn; set => rn = value; }

        public void NDMCorrleations(double D20, double RI20, double MW, double S)
        {
            v = 2.5 * (RI20 - 1.475) - (D20 - 0.851);
            w = (D20 - 0.851) - 1.11 * (RI20 - 1.475);
            
            if (w > 0)
            {
                ca = 430 * v + 3660 / MW;
            }
            else
            {
                ca = 670 * v + 3660 / MW;
            }

            if ((w > 0))
            {
                cr = 820 * w - 3 * S + 10000 / MW;
            }
            else
            {
                cr = 1440 * w - 3 * S + 10000 / MW;
            }

            cn = cr - ca;
            cp = 100 - cr;

            double Ra;
            if (v > 0)
                Ra = 0.44 + 0.055 * MW * v;
            else
                Ra = 0.44 + 0.08 * MW * v;

            double Rt;

            if (w > 0)
                Rt = 1.33 + 0.146 * MW * (w - 0.005 * S);
            else
                Rt = 1.33 + 0.18 * MW * (w - 0.005 * S);

            rn = Rt - Ra;
        }
    }
}
