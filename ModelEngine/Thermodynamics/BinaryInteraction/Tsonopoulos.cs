using Extensions;

namespace ModelEngine.BinaryInteraction
{
    public class Tsonopoulos
    {
        public double kij(BaseComp a, BaseComp b)
        {
            return 1 - 8 * ((a.CritV * b.CritV).Pow(0.5)) / (a.CritV.Pow(1 / 3d) + b.CritV.Pow(1 / 3d)).Pow(3);
        }
    }
}