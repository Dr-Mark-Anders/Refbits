using System.Collections.Generic;
using Units.UOM;

namespace ModelEngine
{
    public static class StaticCompDef
    {
        public static int NoOfPureComps { get; set; } = 0;

        public static int Count
        {
            get
            {
                return Components.Count;
            }
        }

        public static List<BoilingPoint> Components { get; set; } = new();

        public static BoilingPoints compdef
        {
            get
            {
                return new BoilingPoints(Components);
            }
        }

        static StaticCompDef()
        {
            Components.Add(new BoilingPoint("Nitrogen", -195.8, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("CO", -191.45, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Oxygen", -182.95, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Methane", -161.53, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Ethylene", -103.75, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Ethane", -88.6, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("CO2", -78.55, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("H2S", -59.65, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Propene", -47.75, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Propane", -42.1, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("i-Butane", -11.73, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("n-Butane", -0.5, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("i-Pentane", 27.88, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("n-Pentane", 36.06, TemperatureUnit.Celsius));
            NoOfPureComps = Components.Count;
            Components.Add(new BoilingPoint("Quasi36_40C", 36.06, 40, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi40_50C", 40, 50, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi50_60C", 50, 60, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi60_70C", 60, 70, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi70_80C", 70, 80, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi80_90C", 80, 90, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi90_100C", 90, 100, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi100_110C", 100, 110, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi110_120C", 110, 120, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi120_130C", 120, 130, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi130_140C", 130, 140, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi140_150C", 140, 150, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi150_160C", 150, 160, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi160_170C", 160, 170, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi170_180C", 170, 180, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi180_190C", 180, 190, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi190_200C", 190, 200, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi200_210C", 200, 210, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi210_220C", 210, 220, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi220_230C", 220, 230, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi230_240C", 230, 240, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi240_250C", 240, 250, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi250_260C", 250, 260, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi260_270C", 260, 270, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi270_280C", 270, 280, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi280_290C", 280, 290, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi290_300C", 290, 300, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi300_310C", 300, 310, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi310_320C", 310, 320, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi320_330C", 320, 330, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi330_340C", 330, 340, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi340_350C", 340, 350, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi350_360C", 350, 360, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi360_370C", 360, 370, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi370_380C", 370, 380, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi380_390C", 380, 390, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi390_400C", 390, 400, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi400_410C", 400, 410, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi410_420C", 410, 420, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi420_430C", 420, 430, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi430_440C", 430, 440, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi440_450C", 440, 450, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi450_460C", 450, 460, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi460_470C", 460, 470, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi470_480C", 470, 480, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi480_490C", 480, 490, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi490_500C", 490, 500, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi500_510C", 500, 510, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi510_520C", 510, 520, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi520_530C", 520, 530, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi530_540C", 530, 540, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi540_560C", 540, 560, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi560_580C", 560, 580, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi580_600C", 580, 600, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi600_630C", 600, 630, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi630_660C", 630, 660, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi660_690C", 660, 690, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi690_720C", 690, 720, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi720_750C", 720, 750, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi750_780C", 750, 780, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi780_810C", 780, 810, TemperatureUnit.Celsius));
            Components.Add(new BoilingPoint("Quasi810_850C", 810, 850, TemperatureUnit.Celsius));
        }

        public static BoilingPoint Find(string name)
        {
            return Components.Find(x => x.Name == name);
        }

        public static int IndexOf(string name)
        {
            return Components.IndexOf(Find(name));
        }

        public static Temperature[] GetAverageBoilingPoints()
        {
            Temperature[] res = new Temperature[Components.Count];
            for (int i = 0; i < Components.Count; i++)
            {
                res[i] = new Temperature(Components[i].BP);
            }
            return res;
        }

        public static Temperature[] GetEndBoilingPoints()
        {
            Temperature[] res = new Temperature[Components.Count];

            for (int i = 0; i < Components.Count; i++)
            {
                res[i] = new Temperature(Components[i].FBP);
            }
            return res;
        }
    }
}