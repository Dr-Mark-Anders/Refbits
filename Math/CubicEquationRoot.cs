namespace Cubics
{
    public class CubicEquationRoot
    {
        public CubicEquationRoot(double A, double B, double C, double D)
        {
            double E, F, G;

            E = B / A;
            F = C / A;
            G = D / A;

            //The Depressed Cubic:			y3 + by = c
            //  x = y - E / 3
            //  b = F - E2 / 3
            //  c = FE / 3 - 2E3 / 27 - G
            //  c2 + 4b3 / 27

            //double x =

            double b = E - E * E / 3;
            double c = F * E / 3 - 2 * E * E / 27 - G;
            double d = C * C + 4 * System.Math.Pow(b, 3) / 27;

            double R;

            if (d > 0)
            {
                if (System.Math.Abs(c + System.Math.Pow(d, 0.5) / 2) > 0)
                {
                    R = System.Math.Abs(c + System.Math.Pow(d, 0.5)) / 2;
                }
                else
                {
                    R = System.Math.Abs(c - System.Math.Pow(d, 2)) / 2;
                }
            }
            else
            {
                R = System.Math.Pow(System.Math.Pow(-b, 3) / 27, 0.5);
            }

            double phi;

            if (d >= 0)
            {
                if (c + System.Math.Pow(d, 0.5) > 0)
                {
                    phi = 0;
                }
                else
                {
                    phi = System.Math.PI;
                }
            }
            else
            {
                phi = System.Math.Acos(c / (2 * R));
            }

            double Q0, Q1;

            Q0 = System.Math.Pow(R, (1 / 3)) * System.Math.Cos(phi / 3);
            Q1 = System.Math.Pow(R, (1 / 3)) * System.Math.Cos((phi + 2 * System.Math.PI) / 3);
        }
    }
}