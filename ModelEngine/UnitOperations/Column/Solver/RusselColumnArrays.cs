namespace RusselColumn
{
    public partial class RusselSolver
    {
        private double[][] StripperVapReturnTotEnthalpies;
        internal double[][][] TrDMatrix;             // Tridagonal matrix, (Comp - Tray - Tray)
        internal double[,] Jacobian;                // Jacobian matrix of differentials (trays * trays)

        //double [] SpecErrors;
        //double [] BaseSpecErros;
        public double[] Errors, BaseErrors;

        private double[][] TempTrayTempK;
        private double[][] FeedMolarCompFlowsLiquid;      // Trays, Comps
        private double[][] FeedMolarCompFlowsVapour;      // Trays, Comps
        private double[][] FeedMolarCompFlowsTotal;       // Trays, Comps

        //double [][] Stripperreturn  s;
        private double[][] StripperLiqFeedEnthalpies;

        //double [] RefluxRatio;
        private double[][] CompLiqMolarFlows;

        private double[][] CompVapMolarFlows;
        private double[][] InterSectionLiquidDraws;
        private double[][] InterSectionLiquidFeeds;
        private double[][] InterSectionVapourDraws;
        private double[][] InterSectionVapourFeeds;
        //double [] TotalMolFractions; // comps

        private double[][] KBase;                      // (trays)
        private double[][] A, B;                       // Parameters for VP estimation, section, tray

        private double TotalFeeds = 0;

        internal double[] grads;
        private double[] deltas;
    }
}