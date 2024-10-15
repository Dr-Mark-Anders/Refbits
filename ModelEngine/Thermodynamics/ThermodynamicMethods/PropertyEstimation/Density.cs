using Extensions;
using Units.UOM;

namespace ModelEngine
{
    public class DensityMethods
    {
        public static double Density(enumDensity method, Components o, Temperature T)
        {
            double res = 0;

            //method = enumDensity.Rackett; // use racket for now (tested)

            switch (method)
            {
                case enumDensity.Costald:
                    res = COSTALD(o, T);
                    break;

                case enumDensity.Rackett:
                    res = Rackett(o, T);
                    break;
                    /* case  enumDensity.RRPS:
                         res = RRPS(o);
                         break;

                     case  enumDensity.YamadaGunn:
                         res = YamadaGunn(o);
                         break;*/
            }
            return res;
        }

        public static double Rackett(Components cc, Temperature T)
        {
            double Rgas = 10.731557089016; // psia/ft3/lbmol/rankine
            double TcritMix = cc.TCritMix().Rankine;
            double Trbase = new Temperature(273.15 + 16).Rankine / TcritMix;
            double experm = 1 + (1 - Trbase).Pow(2D / 7D);
            double aterm = Rgas * cc.TCritMix().Rankine / cc.PCritMix().PSIA;
            double res = 1 / cc.SG() / aterm;
            double Za = res.Pow(1 / experm);

            double inversdensity = aterm * Za.Pow(1 + (1 - T.Rankine / TcritMix).Pow(2D / 7D));
            return 1 / inversdensity * 1000;
        }

        public static double YamadaGunn(Components cc) // not correct yet
        {
            double Vc = cc.VCritMix();
            double Omega = cc.OmegaMix();
            double molarvolume, density;
            molarvolume = (Vc * 1000) * (0.29056 - 0.08775 * Omega).Pow((1 - cc.TbMixK()).Pow(2f / 7f));
            density = 1 / molarvolume * cc.MW() * 1000;  // kg/m3
            return density;
        }

        public static double RRPS(Components cc)
        {
            double Vc = cc.VCritMix();
            double Omega = cc.OmegaMix();
            double molarvolume = (1 + 0.85 * (1 - cc.Tbreduced()) + (1.6916 + 0.984 * Omega).Pow(((1 - cc.TbMixK()).Pow(1f / 3f))));
            double dens = molarvolume / Vc * cc.MW();
            return dens;
        }

        public static double COSTALD(Components cc, Temperature T)
        {
            double a = -1.52816;
            double b = 1.43907;
            double c = -0.81446;
            double d = 0.190454;
            double e = -0.296123;
            double f = 0.386914;
            double g = -0.0427258;
            double h = -0.0480645;

            double TcritMix; // cc.TCritMix();

            double Omega = cc.OmegaMix(); // use better mixing rule
            double Vc;// = cc.VCritMix(); // use better mixing rule

            double Vm;
            double SumXiVi = 0, SumXiVi23 = 0, SumXiVi13 = 0;
            double VT;

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                SumXiVi += cc[i].CritV * cc[i].MoleFraction;
                SumXiVi23 += cc[i].CritV.Pow(2 / 3D) * cc[i].MoleFraction;
                SumXiVi13 += cc[i].CritV.Pow(1 / 3D) * cc[i].MoleFraction;
            }

            Vm = 1 / 4D * (SumXiVi + 3 * SumXiVi23 * SumXiVi13);
            Vc = Vm;

            TcritMix = 0;
            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                for (int n = 0; n < cc.ComponentList.Count; n++)
                {
                    VT = (cc[i].CritV * cc[i].CritT * cc[n].CritV * cc[n].CritT).Pow(0.5);
                    TcritMix += cc[i].MoleFraction * cc[n].MoleFraction * VT / Vm;
                }
            }

            double Tr = T / TcritMix;

            //=1+a*(1-TrCost)^(1/3)+b*(1-TrCost)^(2/3)+cc*(1-TrCost)+d*(1-TrCost)^(4/3)
            double V0 = 1 + a * (1 - Tr).Pow(1f / 3f) + b * (1 - Tr).Pow(2f / 3f) + c * (1 - Tr) + d * (1 - Tr).Pow(4f / 3f);
            double V1 = (e + f * Tr + g * Tr.Pow(2) + h * Tr.Pow(3)) / (Tr - 1.00001);

            double molarvolume = Vc * V0 * (1 - Omega * V1);
            double den = 1 / molarvolume * cc.MW();
            return den;
        }
    }
}