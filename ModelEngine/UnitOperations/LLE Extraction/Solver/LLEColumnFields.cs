namespace LLE_Solver
{
    public partial class LLESolver
    {
        internal readonly double WaterDrawFactor = 100000;

        public event IterationEventHAndler UpdateIteration1;

        public event IterationEventHAndler UpdateIteration2;

        //DumpArrayToFile DumpArray;
        internal int? WaterCompNo;

        internal double WaterDraw;

        internal int NoComps;
        internal int TotNoTrays;
        internal int NoSections;

        internal int JacobianSize;
    }
}