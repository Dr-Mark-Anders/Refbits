namespace NaphthaReformerSI
{
    public partial class NapReformerSI
    {
        public double APIToSG(double value)
        {
            return 141.5 / (value + 131.5);
        }

        public double SGToAPI(double value)
        {
            return 141.5 / value - 131.5;
        }

        public double GmPerCm3ToLbsPerFt3(double value)
        {
            return value * 2.20462262 / 0.0353146667;
        }

        public double LbsPerFt3ToGmPerCm3(double value)
        {
            return value / 2.20462262 * 0.0353146667;
        }

        public double KgPerM3ToLbsPerFt3(double value)
        {
            return value / 16.01846;
        }

        public double LbsPerFt3ToKgPerM3(double value)
        {
            return value * 16.01846;
        }

        public double LbsPerFt3ToSG(double value)
        {
            return value / 62.37;
        }

        public double SGToLbsPerFt3(double value)
        {
            return value * 62.37;
        }

        public double LbsPerGalToSG(double value)
        {
            return value / 8.338;
        }

        public double SGToLbsPerGal(double value)
        {
            return value * 8.338;
        }

        public double LbsToKg(double value)
        {
            return value / 2.20462262;
        }

        public double KgToLbs(double value)
        {
            return value * 2.20462262;
        }

        public double LbsToMT(double value)
        {
            return value / 2204.62262;
        }

        public double MTToLbs(double value)
        {
            return value * 2204.62262;
        }

        public double PsigToPsia(double value)
        {
            return value + 14.6959488;
        }

        public double PsiaToPsig(double value)
        {
            return value - 14.6959488;
        }

        public double PsigToAtm(double value)
        {
            return value / 14.6959488 + 1.0;
        }

        public double PsiaToAtm(double value)
        {
            return value / 14.6959488;
        }

        public double AtmToPsia(double value)
        {
            return value * 14.6959488;
        }

        public double KgPerCm2GToAtm(double value)
        {
            return value / 1.033227555 + 1.0;
        }

        public double KgPerCm2ToAtm(double value)
        {
            return value / 1.033227555;
        }

        public double AtmToKgPerCm2G(double value)
        {
            return (value - 1.0) * 1.033227555;
        }

        public double AtmToKgPerCm2(double value)
        {
            return value * 1.033227555;
        }

        public double KgPerCm2GToPsia(double value)
        {
            return value / 0.070306958 + 14.6959488;
        }

        public double KgPerCm2ToPsig(double value)
        {
            return value / 0.070306958 - 14.6959488;
        }

        public double KgPerCm2ToPsi(double value)
        {
            return value / 0.070306958;
        }

        public double PsiaToKgPerCm2G(double value)
        {
            return (value - 14.6959488) * 0.070306958;
        }

        public double PsiToKgPerCm2(double value)
        {
            return value * 0.070306958;
        }

        public double PsigToKgPerCm2(double value)
        {
            return (value + 14.6959488) * 0.070306958;
        }

        public double MMBTUPerHrToMW(double value)
        {
            return value * 0.2930711;
        }

        public double FToR(double value)
        {
            return value + 459.67;
        }

        public double FToC(double value)
        {
            return (value - 32.0) / 1.8;
        }

        public double FToK(double value)
        {
            return (value + 459.67) / 1.8;
        }

        public double RToF(double value)
        {
            return value - 459.67;
        }

        public double RToC(double value)
        {
            return value / 1.8 - 273.15;
        }

        public double RToK(double value)
        {
            return value / 1.8;
        }

        public double CToF(double value)
        {
            return value * 1.8 + 32.0;
        }

        public double CToR(double value)
        {
            return (value + 273.15) * 1.8;
        }

        public double CToK(double value)
        {
            return value + 273.15;
        }

        public double KToF(double value)
        {
            return value * 1.8 - 459.67;
        }

        public double KToR(double value)
        {
            return value * 1.8;
        }

        public double KToC(double value)
        {
            return value - 459.67;
        }

        public double M3ToBbl(double value)
        {
            return value / 0.1589873;
        }

        public double BblToM3(double value)
        {
            return value * 0.1589873;
        }

        public double LbMolToSCF(double value)
        {
            return value * 379.482;
        }

        public double KgMolToNM3(double value)
        {
            return value * 22.415;
        }

        public double NM3ToKgMol(double value)
        {
            return value / 22.415;
        }
    }
}