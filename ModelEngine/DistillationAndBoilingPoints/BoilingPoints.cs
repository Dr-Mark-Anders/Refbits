using System;
using System.Collections.Generic;
using Units.UOM;

namespace ModelEngine
{
    public class BoilingPoint : IComparable<BoilingPoint>
    {
        public int CompareTo(BoilingPoint boilingPoint)
        {
            // A null value means that object is greater.
            return this.name.CompareTo(boilingPoint.Name);
        }

        private string name;
        private Temperature ibp = new Temperature();
        private Temperature fbp = new Temperature();
        private Temperature nbp = new Temperature();

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Temperature BP
        {
            get
            {
                if (ibp == fbp && ibp == 0)
                    return nbp;
                return (ibp + fbp) * 0.5;
            }
        }

        public Temperature IBP { get => ibp; set => ibp = value; }
        public Temperature FBP { get => fbp; set => fbp = value; }

        public BoilingPoint(string name, Temperature IBP, Temperature FBP)
        {
            this.name = name;
            this.ibp = IBP;
            this.fbp = FBP;
            this.nbp = (IBP + FBP) / 2;
        }

        public BoilingPoint(string name, double IBP, double FBP, TemperatureUnit unit)
        {
            this.name = name;
            this.ibp = new Temperature(IBP, unit);
            this.fbp = new Temperature(FBP, unit);
            this.nbp = new Temperature((IBP + FBP) / 2, unit);
        }

        public BoilingPoint(string name, Temperature nbp, Temperature IBP, Temperature FBP)
        {
            this.name = name;
            this.ibp = IBP;
            this.fbp = FBP;
            this.nbp = nbp;
        }

        public BoilingPoint(string name, Temperature IBP)
        {
            this.name = name;
            ibp = IBP;
            fbp = IBP;
            nbp = ibp;
        }

        public BoilingPoint(string name, Temperature IBP, TemperatureUnit unit)
        {
            this.name = name;
            ibp = new Temperature(IBP, unit);
            fbp = new Temperature(IBP, unit);
            nbp = new Temperature(IBP, unit);
        }

        public double CutWidth()
        {
            return fbp - ibp;
        }
    }

    public class BoilingPoints
    {
        private List<BoilingPoint> thelist = new List<BoilingPoint>();

        public BoilingPoints(List<BoilingPoint> list)
        {
            thelist.AddRange(list);
        }

        public void Add(BoilingPoint bp)
        {
            thelist.Add(bp);
        }

        public BoilingPoint this[int i]
        {
            get
            {
                return thelist[i];
            }
        }

        private int endofPure = 0;

        public int NoOfPureComps
        {
            get
            {
                return endofPure;
            }

            set
            {
                endofPure = value;
            }
        }

        public int Count
        {
            get
            {
                return thelist.Count;
            }
        }

        public List<BoilingPoint> Components
        {
            get
            {
                return thelist;
            }

            set
            {
                thelist = value;
            }
        }

        public List<Temperature> DegCtoTemperature()
        {
            List<Temperature> res = new List<Temperature>();
            for (int i = 0; i < thelist.Count; i++)
            {
                res.Add(thelist[i].BP);
            }
            return res;
        }

        public BoilingPoints(bool defaultlist)
        {
            if (defaultlist)
            {
                thelist.Add(new BoilingPoint("Hydrogen", -252.6));
                thelist.Add(new BoilingPoint("Nitrogen", -195.8));
                thelist.Add(new BoilingPoint("CO", -191.45));
                thelist.Add(new BoilingPoint("Oxygen", -182.95));
                thelist.Add(new BoilingPoint("Methane", -161.53));
                thelist.Add(new BoilingPoint("Ethylene", -103.75));
                thelist.Add(new BoilingPoint("Ethane", -88.6));
                thelist.Add(new BoilingPoint("CO2", -78.55));
                thelist.Add(new BoilingPoint("H2S", -59.65));
                thelist.Add(new BoilingPoint("Propene", -47.75));
                thelist.Add(new BoilingPoint("Propane", -42.1));
                thelist.Add(new BoilingPoint("i-Butane", -11.73));
                thelist.Add(new BoilingPoint("n-Butane", -0.5));
                thelist.Add(new BoilingPoint("i-Pentane", 27.88));
                thelist.Add(new BoilingPoint("n-Pentane", 36.06));
                endofPure = thelist.Count;
                thelist.Add(new BoilingPoint("Quasi36_40C", 36.1, 40));
                thelist.Add(new BoilingPoint("Quasi40_50C", 40, 50));
                thelist.Add(new BoilingPoint("Quasi50_60C", 50, 60));
                thelist.Add(new BoilingPoint("Quasi60_70C", 60, 70));
                thelist.Add(new BoilingPoint("Quasi70_80C", 70, 80));
                thelist.Add(new BoilingPoint("Quasi80_90C", 80, 90));
                thelist.Add(new BoilingPoint("Quasi90_100C", 90, 100));
                thelist.Add(new BoilingPoint("Quasi100_110C", 100, 110));
                thelist.Add(new BoilingPoint("Quasi110_120C", 110, 120));
                thelist.Add(new BoilingPoint("Quasi120_130C", 120, 130));
                thelist.Add(new BoilingPoint("Quasi130_140C", 130, 140));
                thelist.Add(new BoilingPoint("Quasi140_150C", 140, 150));
                thelist.Add(new BoilingPoint("Quasi150_160C", 150, 160));
                thelist.Add(new BoilingPoint("Quasi160_170C", 160, 170));
                thelist.Add(new BoilingPoint("Quasi170_180C", 170, 180));
                thelist.Add(new BoilingPoint("Quasi180_190C", 180, 190));
                thelist.Add(new BoilingPoint("Quasi190_200C", 190, 200));
                thelist.Add(new BoilingPoint("Quasi200_210C", 200, 210));
                thelist.Add(new BoilingPoint("Quasi210_220C", 210, 220));
                thelist.Add(new BoilingPoint("Quasi220_230C", 220, 230));
                thelist.Add(new BoilingPoint("Quasi230_240C", 230, 240));
                thelist.Add(new BoilingPoint("Quasi240_250C", 240, 250));
                thelist.Add(new BoilingPoint("Quasi250_260C", 250, 260));
                thelist.Add(new BoilingPoint("Quasi260_270C", 260, 270));
                thelist.Add(new BoilingPoint("Quasi270_280C", 270, 280));
                thelist.Add(new BoilingPoint("Quasi280_290C", 280, 290));
                thelist.Add(new BoilingPoint("Quasi290_300C", 290, 300));
                thelist.Add(new BoilingPoint("Quasi300_310C", 300, 310));
                thelist.Add(new BoilingPoint("Quasi310_320C", 310, 320));
                thelist.Add(new BoilingPoint("Quasi320_330C", 320, 330));
                thelist.Add(new BoilingPoint("Quasi330_340C", 330, 340));
                thelist.Add(new BoilingPoint("Quasi340_350C", 340, 350));
                thelist.Add(new BoilingPoint("Quasi350_360C", 350, 360));
                thelist.Add(new BoilingPoint("Quasi360_370C", 360, 370));
                thelist.Add(new BoilingPoint("Quasi370_380C", 370, 380));
                thelist.Add(new BoilingPoint("Quasi380_390C", 380, 390));
                thelist.Add(new BoilingPoint("Quasi390_400C", 390, 400));
                thelist.Add(new BoilingPoint("Quasi400_410C", 400, 410));
                thelist.Add(new BoilingPoint("Quasi410_420C", 410, 420));
                thelist.Add(new BoilingPoint("Quasi420_430C", 420, 430));
                thelist.Add(new BoilingPoint("Quasi430_440C", 430, 440));
                thelist.Add(new BoilingPoint("Quasi440_450C", 440, 450));
                thelist.Add(new BoilingPoint("Quasi450_460C", 450, 460));
                thelist.Add(new BoilingPoint("Quasi460_470C", 460, 470));
                thelist.Add(new BoilingPoint("Quasi470_480C", 470, 480));
                thelist.Add(new BoilingPoint("Quasi480_490C", 480, 490));
                thelist.Add(new BoilingPoint("Quasi490_500C", 490, 500));
                thelist.Add(new BoilingPoint("Quasi500_510C", 500, 510));
                thelist.Add(new BoilingPoint("Quasi510_520C", 510, 520));
                thelist.Add(new BoilingPoint("Quasi520_530C", 520, 530));
                thelist.Add(new BoilingPoint("Quasi530_540C", 530, 540));
                thelist.Add(new BoilingPoint("Quasi540_560C", 540, 560));
                thelist.Add(new BoilingPoint("Quasi560_580C", 560, 580));
                thelist.Add(new BoilingPoint("Quasi580_600C", 580, 600));
                thelist.Add(new BoilingPoint("Quasi600_630C", 600, 630));
                thelist.Add(new BoilingPoint("Quasi630_660C", 630, 660));
                thelist.Add(new BoilingPoint("Quasi660_690C", 660, 690));
                thelist.Add(new BoilingPoint("Quasi690_720C", 690, 720));
                thelist.Add(new BoilingPoint("Quasi720_750C", 720, 750));
                thelist.Add(new BoilingPoint("Quasi750_780C", 750, 780));
                thelist.Add(new BoilingPoint("Quasi780_810C", 780, 810));
                thelist.Add(new BoilingPoint("Quasi810_850C", 810, 850));
            }
        }

        public BoilingPoint Find(string name)
        {
            return thelist.Find(x => x.Name == name);
        }

        public int IndexOf(string name)
        {
            return thelist.IndexOf(Find(name));
        }

        public String[] Names()
        {
            String[] res = new String[thelist.Count];
            for (int i = 0; i < thelist.Count; i++)
            {
                res[i] = thelist[i].Name;
            }
            return res;
        }


        public Temperature[] GetAverageBoilingPointsK()
        {
            Temperature[] res = new Temperature[thelist.Count];
            for (int i = 0; i < thelist.Count; i++)
            {
                res[i] = thelist[i].BP;
            }
            return res;
        }

        public Temperature[] GetAverageBoilingPointsC()
        {
            Temperature[] res = new Temperature[thelist.Count];
            for (int i = 0; i < thelist.Count; i++)
            {
                res[i] = new Temperature(thelist[i].BP);
            }
            return res;
        }

        public Temperature[] GetAverageBoilingPoints()
        {
            Temperature[] res = new Temperature[thelist.Count];
            for (int i = 0; i < thelist.Count; i++)
                res[i] = new Temperature(thelist[i].BP);

            return res;
        }

        public List<double> GetAverageBoilingPointsList()
        {
            List<double> res = new List<double>();
            for (int i = 0; i < thelist.Count; i++)
            {
                res.Add(new Temperature(thelist[i].BP));
            }
            return res;
        }

        public Temperature[] GetEndBoilingPoints()
        {
            Temperature[] res = new Temperature[thelist.Count];

            for (int i = 0; i < thelist.Count; i++)
            {
                res[i] = new Temperature(thelist[i].FBP);
            }
            return res;
        }

        public double[] GetEndBoilingPointsC()
        {
            double[] res = new double[thelist.Count];

            for (int i = 0; i < thelist.Count; i++)
            {
                res[i] = thelist[i].FBP;
            }
            return res;
        }
    }
}