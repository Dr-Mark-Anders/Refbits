using EngineThermo;
using System;
using Units.UOM;

namespace NaphthaReformerUS
{
    public enum CatalystUnits { LHSV, kg, LBS }

    public enum InputOption { Short, Medium, Full }

    public enum AssayBasis { Mass, Molar, Volume }

    public partial class NapReformerUS
    {
        CatalystUnits catunit = CatalystUnits.LHSV;
        const double FactorBPDtoLbmHr = 14.574166;
        Tuple<double, double, double, double> PNAO;
        AssayBasis assayBasis = AssayBasis.Mass;

        MassFlow ReformateMassFlow, RxEFF_MassFlow, SepVap_MassFlow, RecycleGas_MassFlow;
        VolFlow ReformateVolFlow, RxEFF_VolFlow, SepVap_VolFlow, RecycleGas_VOlFlow;

        public const int NumComp = 31, NumRxn = 80, NumDepVar = 32, MaxNumReactor = 4, RowMol = 4,
            RowLbs = 41, RowBPD = 78, ColFeed = 2, ColRecy = 3, ColEff = 8, ColRef = 9, ColGas = 10;

        public double RefC5PlusLV, SumRON, SumMON,
            H2HC, DHCH, DHCP, ISOM, OPEN, PHC, NHC, HDA, MetalActiv, AcidActiv, AmtCat1, MetalActiv2, AcidActiv2, AmtCat2,
             MetalActiv3, AcidActiv3, AmtCat3, MetalActiv4, AcidActiv4, AmtCat4,
            DHCHFact, CHDehydrogTerm, DHCPFact, CPDehydrogTerm, ISOMFact, IsomTerm, OPENFact, CyclTerm, PHCFact,
            PCrackTerm, NHCFact, NCrackTerm, HDAFact, DealkTerm,
            TotWHSV, OctSpec, P_LV, N_LV, A_LV, P_WT, N_WT, A_WT, APIFeed, LHSV;

        public MassFlow RefC5PlusWT;
        public double Feed_MW;
        public double Feed_SG;
        public EnergyFlow[] Duty  = new EnergyFlow[4];
        public MoleFlow[] F  = new MoleFlow[32];
        public MassFlow[] RefMassFlows  = new MassFlow[32];
        public double[] RefMassFraction  = new double[32];
        public MassFlow[] GasMassFlow  = new MassFlow[32];
        public VolFlow[] RefVolFlow  = new VolFlow[32];
        public double[] RefVolFraction  = new double[32];
        public Temperature[] ReactorT_Out;
        public MoleFlow[] F_Recy  = new MoleFlow[32];
        MoleFlow[] F_Eff  = new MoleFlow[32];
        public MoleFlow[] F_Ref  = new MoleFlow[32];
        MoleFlow[] F_Vap  = new MoleFlow[32];
        public MoleFlow[] F_NetGas = new MoleFlow[32];
        MoleFlow[] F_RecyNew = new MoleFlow[32];
        MoleFlow[] F_RecyOld = new MoleFlow[32];
        MoleFlow[] F_RecyStar  = new MoleFlow[32];
        MoleFlow[] F_RecyStarNew  = new MoleFlow[32];
        MoleFlow[] F_RecyStarOld  = new MoleFlow[32];
        public MoleFlow[][] Eff  = new MoleFlow[5][];
        public VolFlow NaphVolFeed;
        MoleFlow NaphMolFeed;
        MoleFlow MolH2Recy;
        public MassFlow NaphMassFeed;
        public  Density SGFeed;
        public Pressure PSep;
        public Pressure SumRVP;
        public Pressure[] ReactorP;
        public Pressure[] InletP;
        public Temperature InitialTemp;
        public Temperature InitialTemp2;
        public Temperature InitialTemp3;
        public Temperature InitialTemp4;
        public Temperature TSep;
        public Temperature[] ReactorT_In { get; set; }
        public double NaphM3FeedRate;
        public double NaphMTPDFeedRate;
        public double SumMol = 0.0;
        public double SumLbs = 0.0;
        public double SumSCF = 0.0;
        public double SumNM3 = 0.0;
        public MoleFlow[] MolFeed { get => molFeed; set => molFeed = value; }
        MassFlow[] MassFeed { get => massFeed; set => massFeed = value; }
        VolFlow[] VolFeed { get => volFeed; set => volFeed = value; }
        MoleFlow[] Reformate { get => reformate; set => reformate = value; }
        MoleFlow[] Offgas { get => offgas; set => offgas = value; }
        public Mass[] AmtCat { get => amtCat; set => amtCat = value; }
        DistPoints FeedDistillation { get; set; } = new();
        public int NumReactor;
        Components Feed { get => feed; set => feed = value; }
        public double[] D86 { get => d86; set => d86 = value; }
        public double[] FurnEffic { get => furnEffic; set => furnEffic = value; }
        double[] C9plusFact { get => c9plusFact; set => c9plusFact = value; }
        public double[] CatPercent { get => catPercent; set => catPercent = value; }
        double[] MetalAct { get => metalAct; set => metalAct = value; }
        double[] AcidAct { get => acidAct; set => acidAct = value; }
        double[] StdHeatForm { get => stdHeatForm; set => stdHeatForm = value; }
        double[] MW { get => mW; set => mW = value; }
        double[] SG { get => sG; set => sG = value; }
        double[] NBP { get => nBP; set => nBP = value; }
        double[] W { get => w; set => w = value; }
        double[] MolVol { get => molVol; set => molVol = value; }
        double[] Sol { get => sol; set => sol = value; }
        double[] RON { get => rON; set => rON = value; }
        double[] MON { get => mON; set => mON = value; }
        double[] RVP { get => rVP; set => rVP = value; }
        public MoleFlow[,] F_Inlet { get; set; } = new MoleFlow[32, 5];

        private Temperature tSep;
        double[] TCritR;
        double[] PCritPSIA;
        private Pressure sumRVP;
        public DeltaPressure DP, DP2, DP3, DP4;
        private VolFlow naphVolFeed;
        private MoleFlow molH2Recy;
        private MassFlow naphMassFeed;
        private Density sGFeed;
        private double[] RefMoleFraction;
        private Temperature[] reactorT_Out = new Temperature[5];
        private double[] rVP;

        private MoleFlow[] molFeed;
        private MassFlow[] massFeed;
        private VolFlow[] volFeed;
        private MoleFlow[] offgas;
        private Mass[] amtCat;
        private Components feed = new();
        private bool SpecOct = false;

        private double[] A1, A2, B1, B2;

        double[,] HCDistrib;
        double[][] CpCoeft;
        MoleFlow[] reformate;
        double[] d86;
        double[] furnEffic;
        double[] c9plusFact;
        double[] catPercent;
        double[] metalAct;
        double[] acidAct;
        double[] stdHeatForm;
        double[] mW;
        double[] sG;
        double[] nBP;
        double[] w;
        double[] molVol;
        double[] sol;
        double[] rON;
        double[] mON;

        public NapReformerUS(double[] d86, double[] furnEffic, double[] c9plusFact, double[] catPercent, double[] metalAct, double[] acidAct, double[] stdHeatForm)
        {
            D86 = d86;
            FurnEffic = furnEffic;
            C9plusFact = c9plusFact;
            CatPercent = catPercent;
            MetalAct = metalAct;
            AcidAct = acidAct;
            StdHeatForm = stdHeatForm;
        }
    }
}
