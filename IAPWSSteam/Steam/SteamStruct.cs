namespace ModelEngine
{
    /// <summary>
    /// used for 1967 forulation only
    /// </summary>
    public struct SteamStruct
    {
        public double p, psat, t, tsat, SteamMassFrac;
        public double enthalpy, entropy, specific_volume, public_energy;

        public SteamStruct(double p, double psat, double t, double tsat, double enthalpy, double entropy, double specific_volume, double public_energy, double SteamFrac)
        {
            this.p = p;
            this.psat = psat;
            this.tsat = tsat;
            this.t = t;
            this.enthalpy = enthalpy;
            this.entropy = entropy;
            this.specific_volume = specific_volume;
            this.public_energy = public_energy;
            this.SteamMassFrac = SteamFrac;
        }
    }
}