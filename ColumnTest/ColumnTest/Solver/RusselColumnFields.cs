namespace RusselColumnTest
{
    public partial class RusselSolverTest
    {
        internal readonly double WaterDrawFactor = 100000;

        public event IterationEventHAndlerTest UpdateIteration1;

        public event IterationEventHAndlerTest UpdateIteration2;

        //DumpArrayToFile DumpArray;
        public int? WaterCompNo;

        internal double WaterDraw;

        public int NoComps;
        public int TotNoTrays;
        public int NoSections;
        public int JacobianSize;
    }
}