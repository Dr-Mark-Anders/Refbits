using Extensions;
using Math2;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class DistPoints : ISerializable
    {
        public List<DistPoint> points = new List<DistPoint>();
        public enumDistType disttype = enumDistType.D86;
        public UOMProperty DENSITY;
        private TemperatureUnit temperatureUnits = TemperatureUnit.Celsius;

        public DistPoints(List<double> temps, TemperatureUnit tempunit, List<int> LV, Density den, enumDistType disttype, SourceEnum origin = SourceEnum.Input)
        {
            this.disttype = disttype;
            DENSITY = new UOMProperty(den);

            if (temps.Count == LV.Count)
                for (int i = 0; i < temps.Count; i++)
                {
                    DistPoint p = new DistPoint(LV[i],new Temperature(temps[i],tempunit), origin);
                    if (Enum.IsDefined(typeof(DISTPCT), LV[i]))
                        p.Stdpct = (DISTPCT)LV[i];
                    else
                        p.Stdpct = DISTPCT.UNDEFINED;

                    p.Origin = origin;
                    points.Add(p);
                }
        }

        public DistPoints(List<Temperature> temps, List<int> LV)
        {
            if (temps.Count == LV.Count)
                for (int i = 0; i < temps.Count; i++)
                    points.Add(new DistPoint(LV[i], temps[i]));
            DENSITY.BaseValue = double.NaN;
        }

        public DistPoints FillMissingData()
        {
            return DistillationConversions.FillMissingDataByProbability(this);
        }

        public DistPoints()
        {
        }

        public double[] ViewPoints
        {
            get
            {
                List<double> res = new();

                for (int i = 0; i < points.Count; i++)
                    res.Add(points[i].BP.Celsius);

                return res.ToArray();
            }
        }

        public Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty> PNAO;

        public DistPoints(bool create11points)
        {
            Initialise();
        }

        public void Initialise()
        {
            points.Add(new DistPoint(1, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "1%")));
            points.Add(new DistPoint(5, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "5%")));
            points.Add(new DistPoint(10, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "10%")));
            points.Add(new DistPoint(20, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "20%")));
            points.Add(new DistPoint(30, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "30%")));
            points.Add(new DistPoint(50, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "50%")));
            points.Add(new DistPoint(70, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "70%")));
            points.Add(new DistPoint(80, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "80%")));
            points.Add(new DistPoint(90, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "90%")));
            points.Add(new DistPoint(95, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "95%")));
            points.Add(new DistPoint(99, new UOMProperty(Units.ePropID.T, SourceEnum.Default, double.NaN, "99%")));

            UOMProperty P = new UOMProperty();
            UOMProperty N = new UOMProperty();
            UOMProperty A = new UOMProperty();
            UOMProperty O = new UOMProperty();

            PNAO = new Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty>(P, N, A, O);
        }

        private DistPoints TBP()
        {
            DistPoints data = DistillationConversions.Convert(this.Disttype, enumDistType.TBP_WT, this);
            return data;
        }

        public List<DistPoint> Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
            }
        }

        public enumDistType Disttype
        {
            get
            {
                return disttype;
            }

            set
            {
                disttype = value;
            }
        }

        public void Sort()
        {
            points.Sort();
        }

        public void Add(double pct, Temperature bp)
        {
            Points.Add(new DistPoint(pct, bp));
        }

        public void Add(int pct, Temperature bp)
        {
            Points.Add(new DistPoint(pct, bp));
        }

        public void Add(double pct, double bp, TemperatureUnit tu)
        {
            Temperature t = new Temperature(bp, tu);
            Points.Add(new DistPoint(pct, t));
        }

        public void Add(DistPoint p)
        {
            Points.Add(p);
        }

        public int Count
        {
            get
            {
                return Points.Count;
            }
        }

        public TemperatureUnit TemperatureUnits { get => temperatureUnits; set => temperatureUnits = value; }

        public bool isValid()
        {
            foreach (DistPoint item in points)
            {
                if (!item.BP.IsKnown)
                    return false;
            }
            return true;
        }

        public DistPoint this[int index]
        {
            get
            {
                if (index >= 0 && index < points.Count && points.Count != 0)
                    return points[index];
                return new DistPoint(0, new Temperature(0));
            }

            set
            {
                points[index] = value;
            }
        }

        public double[] GetKDoubles()
        {
            double[] res = new double[points.Count];

            for (int n = 0; n < points.Count; n++)
                res[n] = points[n].BP.Kelvin;

            return res;
        }

        public Temperature[] GetTemperatures()
        {
            Temperature[] res = new Temperature[points.Count];

            for (int n = 0; n < points.Count; n++)
                res[n] = points[n].BP;

            return res;
        }

        public void Clear()
        {
            points.Clear();
            DENSITY.BaseValue = double.NaN;
        }

        public double[] getBPs_C()
        {
            double[] res = new double[points.Count];

            for (int n = 0; n < points.Count; n++)
                res[n] = points[n].BP.Celsius;

            return res;
        }

        public double[] getPCTs()
        {
            double[] res = new double[points.Count];
            for (int n = 0; n < points.Count; n++)
            {
                double point = (double)points[n].Pct;
                res[n] = point;
            }
            return res;
        }

        public DistPoints(SerializationInfo info, StreamingContext context)
        {
            try
            {
                points = (List<DistPoint>)info.GetValue("points", typeof(List<DistPoint>));
                disttype = (enumDistType)info.GetValue("disttype", typeof(enumDistType));
                DENSITY = (UOMProperty)info.GetValue("density", typeof(UOMProperty));
                PNAO = (Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty>)info.GetValue("PNAO", typeof(Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty>));
            }
            catch
            {
                UOMProperty P = new UOMProperty();
                UOMProperty N = new UOMProperty();
                UOMProperty A = new UOMProperty();
                UOMProperty O = new UOMProperty();

                PNAO = new Tuple<UOMProperty, UOMProperty, UOMProperty, UOMProperty>(P, N, A, O);
            }
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            Initialise();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("points", points);
            info.AddValue("disttype", disttype);
            info.AddValue("density", DENSITY, typeof(UOMProperty));
            info.AddValue("PNAO", PNAO);
        }

        public Temperature MeABP()
        {
            if (points.Count == 9)
            {
                return points[4].BP;
            }
            else
            {
                return (points[0].BP + points[1].BP + points[5].BP + points[8].BP + points[10].BP) / 5;
            }
        }

        public double APIMolWT(Temperature MABP, Density density)
        {
           double SG = density.SG;
           return 20.486 * (Math.Exp(1.165 / 10000 * (MABP.Fahrenheit + 460) - 7.78712 * SG + 1.1582 / 1000 * (MABP.Fahrenheit + 460) * SG)) * ((MABP.Fahrenheit + 460).Pow(1.26007)) * SG.Pow(4.98308);
        }

        public DistPoints ConvertTo(enumDistType type)
        {
            return DistillationConversions.Convert(this.disttype, type, this);
        }

        /// <summary>
        /// Does not Handle Pure compoenents, includes SG estimate
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="data"></param>
        /// <param name="PureCompaAdjustor"></param>
        /// <param name="SetPureLVsToZero"></param>
        /// <returns></returns>
        public Components ConvertToQuasiComps(Components ccall)
        {
            Components cc = ccall;
            int count = cc.Count;
            Components LVPcts = cc.Clone();
            double[] CumLVPcts = new double[count];
            DistPoints tbp = ConvertTo(enumDistType.TBP_VOL);

            double KFact = (tbp.MeABP().Rankine).Pow(1 / 3D) / ((Density)tbp.DENSITY.UOM).SG;

            double res;
            double[] K = GetKDoubles();
            double[] PCts = getPCTs();
            Temperature[] BPs = cc.BPArray;

            for (int n = 0; n < count; n++)
            {
                res = CubicSpline.CubSpline(eSplineMethod.Constrained, BPs[n], K ,PCts );
                if (res < 0)
                    CumLVPcts[n] = 0;
                else if (res > 100)
                    CumLVPcts[n] = 100;
                else
                    CumLVPcts[n] = res;

                if (n == 0)
                {
                    LVPcts[0].STDLiqVolFraction = CumLVPcts[0];
                }
                else
                {
                    LVPcts[n].STDLiqVolFraction = (CumLVPcts[n] - CumLVPcts[n - 1]);
                }
            }

            LVPcts.NormaliseFractions(FlowFlag.LiqVol);

            for (int i = 0; i < cc.Count; i++)
            {
                if (LVPcts[i].MoleFraction >= 0 && LVPcts[i].MoleFraction <= 100)
                {
                    Temperature bp = LVPcts[i].MidBP;
                    Density d=new();
                    d.SG = bp.Rankine.Pow(1 / 3D) / KFact;
                    LVPcts[i].Density = d;
                    LVPcts[i].ReEstimateCriticalProps();
                }
            }

            return LVPcts;
        }
    }
}