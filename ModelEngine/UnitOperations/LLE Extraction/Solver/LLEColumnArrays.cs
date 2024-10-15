namespace LLE_Solver
{
    public partial class LLESolver
    {
        private double[][] StripperVapreturnTotEnthalpies;
        internal double[][][] TrDMatrix;             // Tridagonal matrix, (Comp - Tray - Tray)
        private double[][][] TrDInverse;            // Tridagonal matrix, (Comp - Tray - Tray)
        internal double[][] Jacobian;                // Jacobian matrix of differentials (trays * trays)

        //double [] SpecErrors;
        //double [] BaseSpecErros;
        internal double[] Errors, BaseErrors;

        private double[][] TempTrayTempK;
        private double[][] FeedMolarCompFlowsLiquid;      // Trays, Comps
        private double[][] FeedMolarCompFlowsVapour;      // Trays, Comps
        private double[][] FeedMolarCompFlowsTotal;       // Trays, Comps
        private double[][] PAreturnEnthalpy;

        //double [][] Stripperreturn  s;
        private double[][] StripperLiqFeedEnthalpies;

        //double [] RefluxRatio;
        private double[][] CompLiqMolarFlows;

        private double[][] CompVapMolarFlows;
        private double[][] interSectionLiquidDraws;
        private double[][] interSectionLiquidFeeds;
        private double[][] interSectionVapourDraws;
        private double[][] interSectionVapourFeeds;
        //double [] TotalMolFractions; // comps

        private double[][] KBase;                      // (trays)
        private double[][] A, B;                       // Parameters for VP estimation, section, tray

        private double TotalFeeds = 0;

        internal double[] grads;
        private double[] deltas;
    }
}