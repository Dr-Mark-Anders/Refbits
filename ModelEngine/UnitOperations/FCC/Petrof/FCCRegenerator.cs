using Units.UOM;

namespace ModelEngine.FCC.Petrof
{
    public class FCCRegenerator
    {
        private Length DensePhaseDiamter, DensePhaseHeight;
        private Length LightPhaseDiamter, LightPhaseHeight;
        private MassFlow CatalystFlow, AirFlow;
        private MassFraction CSC, CRC, CokeSulphur;
        private bool PartialBurn = false;

        private Temperature AirTemp, AirBlowerDischargeTemp, Tempin, TempOut;
        private MassFraction AirHumidity;
        private double DeltaCoke;
        private MassFraction CokeH2wtpct;
        private MoleFlow CokeBurntMoles;
        private MoleFlow CokeBurnH2;
        private double CO_CO2MoleRatio;
        private double O2inExhaustGas;
        private double MolesO2_MoleC;
        private Moles MolesO2;
        private Moles AirCarbon;
        private Moles AirH2;
        private Moles ExcessAir;

        private MassFraction MoistureInAir;

        public Length DensePhaseDiamter1 { get => DensePhaseDiamter; set => DensePhaseDiamter = value; }
        public Length DensePhaseHeight1 { get => DensePhaseHeight; set => DensePhaseHeight = value; }
        public Length LightPhaseDiamter1 { get => LightPhaseDiamter; set => LightPhaseDiamter = value; }
        public Length LightPhaseHeight1 { get => LightPhaseHeight; set => LightPhaseHeight = value; }
        public MassFlow CatalystFlow1 { get => CatalystFlow; set => CatalystFlow = value; }
        public MassFlow AirFlow1 { get => AirFlow; set => AirFlow = value; }
        public MassFraction CSC1 { get => CSC; set => CSC = value; }
        public MassFraction CRC1 { get => CRC; set => CRC = value; }
        public MassFraction CokeSulphur1 { get => CokeSulphur; set => CokeSulphur = value; }
        public bool PartialBurn1 { get => PartialBurn; set => PartialBurn = value; }
        public Temperature AirTemp1 { get => AirTemp; set => AirTemp = value; }
        public Temperature AirBlowerDischargeTemp1 { get => AirBlowerDischargeTemp; set => AirBlowerDischargeTemp = value; }
        public Temperature Tempin1 { get => Tempin; set => Tempin = value; }
        public Temperature TempOut1 { get => TempOut; set => TempOut = value; }
        public MassFraction AirHumidity1 { get => AirHumidity; set => AirHumidity = value; }
        public double DeltaCoke1 { get => DeltaCoke; set => DeltaCoke = value; }
        public MassFraction CokeH2wtpct1 { get => CokeH2wtpct; set => CokeH2wtpct = value; }
        public MoleFlow CokeBurntMoles1 { get => CokeBurntMoles; set => CokeBurntMoles = value; }
        public MoleFlow CokeBurnH21 { get => CokeBurnH2; set => CokeBurnH2 = value; }
        public double CO_CO2MoleRatio1 { get => CO_CO2MoleRatio; set => CO_CO2MoleRatio = value; }
        public double O2inExhaustGas1 { get => O2inExhaustGas; set => O2inExhaustGas = value; }
        public double MolesO2_MoleC1 { get => MolesO2_MoleC; set => MolesO2_MoleC = value; }
        public Moles MolesO21 { get => MolesO2; set => MolesO2 = value; }
        public Moles AirCarbon1 { get => AirCarbon; set => AirCarbon = value; }
        public Moles AirH21 { get => AirH2; set => AirH2 = value; }
        public Moles ExcessAir1 { get => ExcessAir; set => ExcessAir = value; }
        public MassFraction MoistureInAir1 { get => MoistureInAir; set => MoistureInAir = value; }

        public FCCRegenerator()
        {
        }


        /// <summary>
        // C -> CO2	 32.81 
        // CO -> CO2 10.11 
        // C -> CO	 22.70 
        // H2 -> H2O 120.97 

        /// </summary>
        /// <returns></returns>

        public bool solve()
        {
            MassFraction HinCoke = 0.07, SinCoke = 1 / 10000;
            MassFraction CinCoke = 1 - HinCoke - SinCoke;
            double CO_CO2Moleratio = 0.00009539;
            MassFraction O2inFlueGas = new(1.1, MassFractionUnit.MassPCT);

            Moles SO2 = SinCoke.MassFrac / 32;
            Moles H2O = SinCoke.MassFrac / 4;
            Moles CO2 = SinCoke.MassFrac / 12 * (1 - CO_CO2Moleratio);
            Moles CO = SinCoke.MassFrac / 12 * CO_CO2Moleratio;
            Moles O2 = SO2 + H2O / 2 + CO2;
            Moles StoichAir = O2 / 0.21;
            Moles N2 = StoichAir * 0.79;

            Moles CombustionProductMols = O2 + CO2 + CO + H2O + SO2 + N2;
            Moles ExcessAirRate = CombustionProductMols * O2inFlueGas;
            Moles TotalAirRate = ExcessAirRate + StoichAir;

            return true;
        }


        /// <summary>
        /// Assume constant temperature and constnat C level, ie perfectly mixed
        /// C->CO
        /// C->CO2
        /// CO->CO2
        /// S->SO2
        /// H->H2O
        /// </summary>
        /// <param name="CatRate"></param>
        /// <param name="CokeOnCat"></param>
        /// <param name="CatInletT"></param>
        public void HeatBalance(MassFlow CatRate, MassFraction CokeOnCat, Temperature CatInletT, 
            out Temperature CatOutTm, out MassFraction CokeOnRCat)
        {
            CatOutTm = 0;
            CokeOnCat = 0;
            CokeOnRCat = 0;
        }
    }
}