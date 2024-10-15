using Math2;
using System;
using System.Collections.Generic;
using Units.UOM;

namespace ModelEngine
{
    public partial class Components
    {
        private readonly DistPoints distpoints = new();

        private DistPoints tbp;

        public DistPoints ConvertToTBP()
        {
            DistPoints tbp;
            if (distpoints.Points.Count > 4)
                tbp = DistillationConversions.Convert(distpoints.Disttype, enumDistType.TBP_VOL, distpoints);
            else
                tbp = null;

            return tbp;
        }

        public Temperature MeABP()
        {
            double MeABP = double.NaN;

            if (Pure().Count > 0 && !(Pseudo().Count == 0))
            {
                MeABP = VolAveBP();
            }

            if (distpoints.Count == 11)
            {
                double x = (tbp[9].BP - tbp[10].BP) / 60;
                double y1 = 1.3846 * x * x - 0.5741 * x - 18.12;  //93.33
                double y2 = 1.9064 * x * x - 1.4736 * x - 18.025; //148.89
                double y3 = 1.6972 * x * x + 0.6693 * x - 18.708; //204.44
                double y4 = -0.2187 * x * x * x + 3.2053 * x * x - 0.6566 * x - 17.988; //260.00
                double y5 = -0.2219 * x * x * x + 2.9439 * x * x + 1.5903 * x - 18.182; //315.56

                if (MeABP < 93.33)
                {
                    MeABP -= y1;
                }
                else if (MeABP < 148.89)
                {
                    MeABP -= MeABP * (y2 - y1) / (148.89 - 93.33);
                }
                else if (MeABP < 204.44)
                {
                    MeABP -= MeABP * (y3 - y2) / (204.44 - 148.89);
                }
                else if (MeABP < 260.00)
                {
                    MeABP -= MeABP * (y4 - y3) / (260 - 204.44);
                }
                else if (MeABP < 315.56)
                {
                    MeABP -= MeABP * (y5 - y4) / (315.56 - 260.0);
                }
                else
                {
                    MeABP -= y5;
                }
            }
            else
            {
            }

            return MeABP;
        }

        public double Lab_FBP { get; set; }
        public double Lab_IBP { get; set; }

        public Temperature LBP(int i)
        {
            if (CompList[i].IsPure)
                return CompList[i].BP;

            Temperature res;
            if (i == 0)
                res = CompList[i].BP;
            else
                res = CompList[i].BP - (CompList[i].BP - CompList[i - 1].BP) / 2;

            return res;
        }

        public Temperature UBP(int i)
        {
            if (CompList[i].IsPure)
                return CompList[i].BP;

            Temperature res;
            if (i == CompList.Count - 1)
                res = CompList[i].BP;
            else
                res = CompList[i].BP + (CompList[i + 1].BP - CompList[i].BP) / 2;

            return res;
        }

        public static double GetFirstBPAfterIBP(Components cc, Temperature ibp)
        {
            for (int i = 0; i < cc.Count; i++)
            {
                if (cc[i].BP >= ibp)
                    return cc[i].BP;
            }
            return double.NaN;
        }

        public static double GetLastBPBeforeFBP(Components cc, Temperature fbp)
        {
            for (int i = 0; i < cc.Count; i++)
            {
                if (cc[i].BP >= fbp && i != 0)
                    return cc[i - 1].BP;
                else
                    return cc[0].BP;
            }
            return double.NaN;
        }

        internal static Temperature GetLowestTBoil(Components cc, out int index)
        {
            Temperature res = cc[0].BP;
            index = 0;
            for (int i = 0; i < cc.Count; i++)
            {
                BaseComp item = cc[i];
                if (item.BP < res)
                {
                    res = item.BP;
                    index = i;
                }
            }

            return res;
        }

        internal static Temperature GetHighestTBoil(Components cc, out int index)
        {
            Temperature res = cc[0].BP;
            index = 0;
            for (int i = 0; i < cc.Count; i++)
            {
                BaseComp item = cc[i];
                if (item.BP > res)
                {
                    res = item.BP;
                    index = i;
                }
            }

            return res;
        }

        public static double StreamDistillation(Components cc, double[] X, enumDistType type, enumDistPoints DistPoint)
        {
            Components cctemp = new();
            cctemp.Add(cc);
            cctemp.SetMolFractions(X);
            return cctemp.DistPoint(type, DistPoint);
        }

        public static Temperature GetTBPFromCLV(Components cc, double LV)
        {
            double res = CubicSpline.CubSpline(eSplineMethod.Constrained, LV / 100, cc.VolFractionsCumulative, GetMidBPsK(cc));  // PC is defined by mid boiling point
            return new Temperature(res);
        }

        public Temperature DistPoint(enumDistType type, enumDistPoints point)
        {
            int index = (int)point;
            switch (type)
            {
                case enumDistType.D86:
                    return D86[index].BP;

                case enumDistType.D1160:
                    return D1160[index].BP;

                case enumDistType.D2887:
                    return D2887[index].BP;

                case enumDistType.TBP_WT:
                    return TBP_WT[index].BP;

                case enumDistType.TBP_VOL:
                    return TBP[index].BP;

                case enumDistType.NON:
                    return double.NaN;

                default:
                    return double.NaN;
            }
        }

        public DistPoints CreateShortTBPCurveFromLVpct()
        {
            tbp = new DistPoints();
            for (int n = 0; n < Global.Lvpct_standard.Length; n++)
            {
                tbp.Add(new DistPoint(Global.Lvpct_standard[n], GetTBPFromCLV(this, Global.Lv1[n]), SourceEnum.UnitOpCalcResult));
            }
            return tbp;
        }

        public DistPoints TBP
        {
            get
            {
                return CreateShortTBPCurveFromLVpct();
            }
        }

        public DistPoints TBP_WT
        {
            get
            {
                return DistillationConversions.Convert(enumDistType.TBP_VOL, enumDistType.TBP_WT, TBP);
            }
        }

        public DistPoints D86
        {
            get
            {
                return DistillationConversions.Convert(enumDistType.TBP_VOL, enumDistType.D86, TBP);
            }
        }

        public DistPoints D1160
        {
            get
            {
                return DistillationConversions.Convert(enumDistType.TBP_VOL, enumDistType.D1160, TBP);
            }
        }

        public DistPoints D2887
        {
            get
            {
                return DistillationConversions.Convert(enumDistType.TBP_VOL, enumDistType.D2887, TBP);
            }
        }

        public static Temperature VABPShortForm(Components cc)
        {
                if (cc.tbp is null)
                    return double.NaN;

                return ((cc.tbp[3].BP + cc.tbp[5].BP + cc.tbp[6].BP + cc.tbp[7].BP + cc.tbp[9].BP) / 5);
        }

        public static Temperature VABPForSynthesis(double[] LV, Temperature[] BP)
        {
            if (LV.Length != BP.Length)
            {
                return new Temperature(-999);
            }
            else
            {
                double res = 0;
                for (int n = 0; n < LV.Length; n++)
                {
                    res += LV[n] * BP[n];
                }
                return new Temperature(res);
            }
        }

        public static List<Temperature> GetMidBPs(Components components)
        {
            List<Temperature> res = new();
            for (int n = 0; n < components.Count; n++)
                res.Add(components[n].MidBP);
            return res;
        }
                
        public static List<double> GetMidBPs_C(Components components)
        {
            List<double> res = new();
            for (int n = 0; n < components.Count; n++)
                res.Add(components[n].MidBP.Celsius);

            return res;
        }
                
        public static double[] GetMidBPsK(Components components)
        {
            List<double> res = new();
            for (int n = 0; n < components.Count; n++)
                res.Add(components[n].MidBP);

            return res.ToArray();
        }
                
        public static List<double> GetLowerBPs(Components components)
        {
            List<double> res = new();
            for (int n = 0; n < components.Count; n++)
                res.Add(components[n].LBP);

            return res;
        }
                
        public static List<Temperature> GetUpperBPs(Components components)
        {
            List<Temperature> res = new();
            for (int n = 0; n < components.Count; n++)
                res.Add(components[n].UBP);

            return res;
        }
                
        public static BoilingPoints GetBPCollection(Components components)
        {
            BoilingPoints res = new(false);
            for (int n = 0; n < components.Count; n++)
                res.Add(new BoilingPoint(components.Names()[n], components[n].BP.Celsius, components[n].LBP, components[n].UBP));

            return res;
        }


    }
}