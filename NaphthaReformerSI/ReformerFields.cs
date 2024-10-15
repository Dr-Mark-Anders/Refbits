using ModelEngine;
using Units.UOM;

namespace NaphthaReformerSI
{
    public enum CatalystUnits
    { LHSV, kg, LBS }

    public enum InputOption
    { Short, Medium, Full }

    public enum AssayBasis
    { Mass, Molar, Volume }

    public partial class NapReformerSI
    {
        private CatalystUnits catunit = CatalystUnits.LHSV;
        private const double FactorBPDtoLbmHr = 14.574166;
        public AssayBasis assayBasis = AssayBasis.Mass;
        public string[] Names = null;

        public MassFlow ReformateMassFlow, RxEFF_MassFlow, SepVap_MassFlow, RecycleGas_MassFlow;
        public VolumeFlow ReformateVolFlow, RxEFF_VolFlow, SepVap_VolFlow, RecycleGas_VOlFlow;

        public const int NumComp = 31, NumRxn = 80, NumDepVar = 32, MaxNumReactor = 4;

        public double RefC5PlusLV, SumRON, SumMON,
            H2HC, DHCH, DHCP, ISOM, OPEN, PHC, NHC, HDA, MetalActiv, AcidActiv, AmtCat1, MetalActiv2, AcidActiv2, AmtCat2,
             MetalActiv3, AcidActiv3, AmtCat3, MetalActiv4, AcidActiv4, AmtCat4,
            DHCHFact, CHDehydrogTerm, DHCPFact, CPDehydrogTerm, ISOMFact, IsomTerm, OPENFact, CyclTerm, PHCFact,
            PCrackTerm, NHCFact, NCrackTerm, HDAFact, DealkTerm,
            TotWHSV, OctSpec, P_LV, N_LV, A_LV, P_WT, N_WT, A_WT, APIFeed, LHSV;

        public double NaphM3FeedRate;
        public double NaphMTPDFeedRate;
        public double SumMol = 0.0;
        public double SumLbs = 0.0;
        public double SumSCF = 0.0;
        public double SumNM3 = 0.0;
        public MassFlow RefC5PlusWT { get; private set; }
        public double Feed_MW { get; private set; }
        public double Feed_SG { get; private set; }

        public Temperature InitialTemp, InitialTemp2, InitialTemp3, InitialTemp4, TSep;
        public Temperature[] ReactorT_In;
        private double[] TCritR;
        private double[] PCritPSIA, WHSV;
        public Pressure PSep, SumRVP;
        public Pressure[] ReactorP, InletP;
        public DeltaPressure[] DP = new DeltaPressure[4];
        public VolumeFlow NaphVolFeed;
        public MoleFlow NaphMolFeed, MolH2Recy;
        public MassFlow NaphMassFeed;
        public Density SGFeed;

        public EnergyFlow[] Duty = new EnergyFlow[MaxNumReactor];
        public double[] F = new double[NumComp];
        public VolumeFlow[] RefVolFlow = new VolumeFlow[NumComp];
        public double[] RefVolFraction = new double[NumComp];
        private double[] RefMoleFraction;
        public MassFlow[] RefMassFlows = new MassFlow[NumComp];
        public double[] RefMassFraction = new double[NumComp];
        public MassFlow[] GasMassFlow = new MassFlow[NumComp];

        public Temperature[] ReactorT_Out = new Temperature[MaxNumReactor];
        public MoleFlow[] F_Recy = new MoleFlow[NumComp];
        public double[,] F_Inlet = new double[NumComp, MaxNumReactor];
        public double[] F_Eff = new double[NumComp];
        public double[] F_Ref = new double[NumComp];
        public double[] F_Vap = new double[NumComp];
        public double[] F_NetGas = new double[NumComp];
        public MoleFlow[] F_RecyNew = new MoleFlow[NumComp];
        public MoleFlow[] F_RecyOld = new MoleFlow[NumComp];
        public MoleFlow[] F_RecyStar = new MoleFlow[NumComp];
        public MoleFlow[] F_RecyStarNew = new MoleFlow[NumComp];
        public MoleFlow[] F_RecyStarOld = new MoleFlow[NumComp];
        public double[][] Eff = new double[MaxNumReactor][];

        public double[] D86, FurnEffic, C9plusFact, CatPercent, MetalAct, AcidAct,
            StdHeatForm, MW, SG, NBP, W, MolVol, Sol, RON, MON, RVP;

        public double[] MolFeed;
        public MassFlow[] MassFeed;
        public VolumeFlow[] VolFeed;
        public double[] Reformate, offgas;
        public Mass[] AmtCat;

        public int NumReactor;
        public Components Feed = new Components();
        public DistPoints FeedDistillation = new DistPoints();
        private bool SpecOct = false;

        private double[] A1, A2, B1, B2;

        private double[,] HCDistrib;
        private double[][] CpCoeft;
    }
}