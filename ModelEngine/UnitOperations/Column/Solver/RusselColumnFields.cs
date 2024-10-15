namespace RusselColumn
{
    public partial class RusselSolver
    {
        internal readonly double WaterDrawFactor = 100000;

        public event IterationEventHAndler UpdateIteration1;

        public event IterationEventHAndler UpdateIteration2;

        //DumpArrayToFile DumpArray;
        public int? WaterCompNo;

        internal double WaterDraw;

        public int NoComps;
        public int TotNoTrays;
        public int NoSections;
        public int JacobianSize;
    }
}