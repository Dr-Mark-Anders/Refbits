using Units;

namespace ModelEngine
{
    public static class GlobalModel
    {
        public static FlowSheet Flowsheet = new FlowSheet();
        public static DisplayUnits displayunits = new DisplayUnits();

        static GlobalModel()
        {
        }

        public static bool IsRunning { get; set; }

        public static void Solve()
        {
            Flowsheet.PreSolve();
        }
    }
}