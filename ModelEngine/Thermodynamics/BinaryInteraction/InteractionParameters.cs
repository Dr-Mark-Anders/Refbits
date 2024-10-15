using Extensions;
using Units.UOM;

namespace ModelEngine
{
    public class InteractionParameters
    {
        private static double[,] kij = null;

        public static double[,] KijArray { get => kij; set => kij = value; }

        // public static double[,] Kij { get => kij; set => kij = value; }

        public static double[,] Kij(Components cc, Temperature T)
        {
            if (cc is null)
                return null;

            if (kij is null || kij.GetUpperBound(0) + 1 != cc.Count)
                kij = Update(cc, T, enumBIPPredMethod.ChuePrausnitz);

            return kij;
        }

        public static bool CheckParametrs(int N)
        {
            return kij.GetUpperBound(0) == N;
        }

        public static double[,] Update(Components comps, Temperature T, enumBIPPredMethod bipmethod)
        {
            int Count = comps.ComponentList.Count;
            double[,] res = new double[Count, Count];

            for (int A = 0; A < Count; A++)
            {
                for (int B = 0; B < Count; B++)
                {
                    res[A, B] = CalcBI(comps.ComponentList[A], comps.ComponentList[B], T, bipmethod);
                }
            }

            kij = res;

            return res;
        }

        private static double CalcBI(BaseComp bc1, BaseComp bc2, Temperature T, enumBIPPredMethod method)
        {
            double Rgas = 8.314;
            double kij = 0;
            switch (method)
            {
                case enumBIPPredMethod.ChuePrausnitz:
                    double CritV1 = bc1.CritV, CrtiV2 = bc2.CritV;
                    kij = 1 - (2 * (CritV1 * CrtiV2).Pow(1 / 6D)) / (CritV1.Pow(1 / 3D) + CrtiV2.Pow(1 / 3D));
                    break;

                case enumBIPPredMethod.ValdReyes:
                    kij = 0;
                    break;

                case enumBIPPredMethod.Nishiumi1988:
                    kij = 1.224 - 0.00440 * T + 3.251e-05 * T;
                    break;

                case enumBIPPredMethod.Elnabawy:
                    double phi1 = 0.22806, phi2 = 0.18772;
                    double a1, b1, a2, b2, k, Omega = 0;
                    k = 0.37464 + 1.54226 * Omega - 0.26992 * Omega.Sqr();

                    a1 = 0.457 * bc1.CritT._Kelvin * bc1.CritT._Kelvin * Rgas * Rgas / bc1.CritP * (1 + k * (1 - (bc1.TRed(T).Sqrt()))).Sqr();
                    b1 = 0.0778 * bc1.CritT * Rgas / bc1.CritP;

                    a2 = 0.457 * bc2.CritT._Kelvin * bc2.CritT._Kelvin * Rgas * Rgas / bc1.CritP * (1 + k * (1 - (bc2.TRed(T).Sqrt()))).Sqr();
                    b2 = 0.0778 * bc2.CritT * Rgas / bc2.CritP;

                    kij = 1 / 2 * (b2 / b1) * (a1 / a2).Sqrt() - 1 / 2 * (b1 / b2) * (a2 / a1).Sqrt()
                        + 1 / 2 * b2 * Rgas * T / (a1 * a2).Sqrt() * phi1 / (T / bc1.CritT._Kelvin).Pow(phi2);

                    return kij;

                case enumBIPPredMethod.Tsonopoulos:
                    kij = 1 - (8 * (bc1.CritV * bc2.CritV).Sqrt() / (bc1.CritV.Pow(1 / 3.0) + bc2.CritV.Pow(1 / 3.0)).Pow(3));
                    return kij;

                case enumBIPPredMethod.Benedeck:
                    kij = 2 * 0.5 * 0.5 * (1 - (8 * (bc1.CritV * bc2.CritV).Sqrt() / (bc1.CritV.Pow(1 / 3.0) + bc2.CritV.Pow(1 / 3.0)).Pow(3)));
                    return kij;
            }

            return kij;
        }
    }
}