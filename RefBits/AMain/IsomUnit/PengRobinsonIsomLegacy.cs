using EngineThermo;
using Extensions;
using System;

namespace Units
{
    public enum EnthalpyReferenceState
    { EnthalpyZero, GibbsZero }

    public enum GasReferenceState
    { Ideal, Real }

    [Serializable]
    public static class PengRobinson
    {
        private const double Rgas = 8.3144621;  //MPa

        private static double tc, pc, omega, tr, pr, t, p, root1, root2, root3, z1, z2, z3;
        private static double fug1, fug2, fug3;
        private static double vol1, vol2, vol3, Udep1, Udep2, Udep3;
        private static double m, m2m, m3m, kappa, alpha, theta1, a, b, PRZ, PPz, QQz, Aterm, Bterm;
        private static double a2CuEq, a1CuEq, a0CuEq, pCuEq, qCuEq, testroot;
        public static double fugratio, enthdep1, enthdep2, enthdep3, Entrdep1, Entrdep3;
        public static double BaseEnthdep1, BaseEnthdep2, BaseEnthdep3, BaseEntrdep1, BaseEntrdep3;
        public static bool isoneroot = false;
        public static GasReferenceState gasrefstate = GasReferenceState.Ideal;
        public static EnthalpyReferenceState enthrefstate = EnthalpyReferenceState.EnthalpyZero;
        public static enumFluidRegion state = enumFluidRegion.Liquid;

        public static void Initialise(double Tcrit, double Pcrit, double Omega)
        {
            tc = Tcrit + 273.15;
            pc = Pcrit / 10;  // bar to MPa
            omega = Omega;
        }

        public static void InitialiseAndCalc(double Tcrit, double Pcrit, double Omega, double t, double p)
        {
            tc = Tcrit + 273.15;
            pc = Pcrit / 10;
            omega = Omega;
            Update(t, p);
        }

        public static double PSat(double t, double p)
        {
            double Pguess = 5;
            double DP = 1;
            double oldfugratio = 5;

            Update(t, p);
            int count = 0;

            do
            {
                count++;
                if (fugratio < 1)
                {
                    Pguess += DP;
                }
                else if (fugratio > 1)
                {
                    Pguess -= DP;
                }

                if (Pguess <= 0)
                {
                    Pguess = 1e-6;
                }

                Update(t, Pguess);
                if (isoneroot && Pguess == 1e-6)
                    return 0;

                if (isoneroot)
                {
                    fugratio = 2;
                    oldfugratio = 2;
                }
                else
                {
                    if (oldfugratio < 1 && fugratio > 1 || oldfugratio > 1 && fugratio < 1)
                    {
                        DP = DP / 2;
                    }
                }

                oldfugratio = fugratio;
            } while (Math.Abs(fugratio - 1) > 0.0001 && count < 500 && Pguess > 1e-6);

            if (count >= 500)
                return double.NaN;
            else
                return Pguess;
        }

        public static double TSat(double TcK, double PcBar, double Omega, double tK, double pBar)
        {
            double OldFugRatio, Temp, delta;

            Temp = 273.15;
            PengRobinson.InitialiseAndCalc(tK, PcBar / 10, Omega, Temp, pBar / 10);

            if (PengRobinson.isoneroot)
            {
                return 0;
            }

            OldFugRatio = PengRobinson.fugratio;

            if (PengRobinson.fugratio < 1)

                delta = -10;
            else
                delta = 10;

            do
            {
                Temp = Temp + delta;
                PengRobinson.Update(Temp, pBar / 10);
                if (PengRobinson.fugratio > 1 & OldFugRatio < 1)
                    delta = -delta / 2;
                else if (PengRobinson.fugratio < 1 & OldFugRatio > 1)
                    delta = -delta / 2;

                OldFugRatio = PengRobinson.fugratio;

                if (OldFugRatio == 9999)
                {
                    return 0;
                }
            }

            while (Math.Abs(PengRobinson.fugratio - 1) > 0.00001);

            return Temp - 273.15;
        }

        public static void Update(double tcent, double PBar)  //  Dependent on T and P
        {
            p = PBar / 10;  // bar to MPa
            t = tcent + 273.15; // to K

            pr = p / pc;
            tr = t / tc;

            kappa = 0.37464 + 1.54226 * omega - 0.26992 * Math.Pow(omega, 2);
            b = 0.0777960739 * Rgas * tc / pc;

            alpha = Math.Pow((1 + kappa * (1 - Math.Sqrt(tr))), 2);
            a = 0.4572355289 * Math.Pow((tc * Rgas), 2) * alpha / pc;

            Aterm = p * a / Math.Pow((Rgas * t), 2);
            Bterm = b * p / Rgas / t;
            a2CuEq = -(1 - Bterm);
            a1CuEq = Aterm - 3 * Math.Pow(Bterm, 2) - 2 * Bterm;
            a0CuEq = -(Aterm * Bterm - Bterm * Bterm - Bterm * Bterm * Bterm);
            pCuEq = (3 * a1CuEq - a2CuEq * a2CuEq) / 3;
            qCuEq = (2 * a2CuEq * a2CuEq * a2CuEq - 9 * a2CuEq * a1CuEq + 27 * a0CuEq) / 27;

            testroot = qCuEq * qCuEq / 4 + pCuEq * pCuEq * pCuEq / 27;

            if (testroot < 0) // three roots
            {
                isoneroot = false;
                m = 2 * Math.Sqrt(-pCuEq / 3);
                m2m = 3 * qCuEq / pCuEq / m;
                m3m = Math.Acos(m2m);
                theta1 = m3m / 3;

                root1 = m * Math.Cos(theta1);
                root2 = m * Math.Cos(theta1 + 4 * 3.14159 / 3);
                root3 = m * Math.Cos(theta1 + 2 * 3.14159 / 3);

                z1 = root1 - a2CuEq / 3;
                z2 = root2 - a2CuEq / 3;
                z3 = root3 - a2CuEq / 3;

                fug1 = p * Math.Exp(z1 - 1 - Math.Log(z1 - Bterm) - Aterm / Bterm / 2.8284 * Math.Log((z1 + 2.4142 * Bterm) / (z1 - 0.4142 * Bterm)));
                fug2 = p * Math.Exp(z2 - 1 - Math.Log(z2 - Bterm) - Aterm / Bterm / 2.8284 * Math.Log((z2 + 2.4142 * Bterm) / (z2 - 0.4142 * Bterm)));
                fug3 = p * Math.Exp(z3 - 1 - Math.Log(z3 - Bterm) - Aterm / Bterm / 2.8284 * Math.Log((z3 + 2.4142 * Bterm) / (z3 - 0.4142 * Bterm)));

                vol1 = z1 * Rgas * t / p;
                vol2 = z2 * Rgas * t / p;
                vol3 = z3 * Rgas * t / p;

                enthdep1 = Rgas * t * (z1 - 1 - Aterm / Bterm / 2.8284 * (1 + kappa * Math.Sqrt(tr / alpha)) * Math.Log((z1 + 2.4142 * Bterm) / (z1 - 0.4142 * Bterm)));
                enthdep2 = Rgas * t * (z2 - 1 - Aterm / Bterm / 2.8284 * (1 + kappa * Math.Sqrt(tr / alpha)) * Math.Log((z2 + 2.4142 * Bterm) / (z2 - 0.4142 * Bterm)));
                enthdep3 = Rgas * t * (z3 - 1 - Aterm / Bterm / 2.8284 * (1 + kappa * Math.Sqrt(tr / alpha)) * Math.Log((z3 + 2.4142 * Bterm) / (z3 - 0.4142 * Bterm)));

                Udep1 = enthdep1 - (z1 - 1) * Rgas * t;
                Udep2 = enthdep2 - (z2 - 1) * Rgas * t;
                Udep3 = enthdep3 - (z3 - 1) * Rgas * t;

                Entrdep1 = Rgas * Math.Log(z1 - Bterm) - Aterm * Rgas / Bterm / 2.8284 * kappa * Math.Sqrt(tr / alpha) * Math.Log((z1 + 2.4142 * Bterm) / (z1 - 0.4142 * Bterm));
                Entrdep3 = Rgas * Math.Log(z3 - Bterm) - Aterm * Rgas / Bterm / 2.8284 * kappa * Math.Sqrt(tr / alpha) * Math.Log((z3 + 2.4142 * Bterm) / (z3 - 0.4142 * Bterm));

                state = enumFluidRegion.TwoPhase;
            }
            else  // one root
            {
                isoneroot = true;
                PPz = (-qCuEq / 2 + Math.Sqrt(testroot)).CubeRoot();
                QQz = (-qCuEq / 2 - Math.Sqrt(testroot)).CubeRoot();

                root1 = PPz + QQz;
                root2 = 0;
                root3 = 0;

                PRZ = root1 - a2CuEq / 3;

                z1 = root1 - a2CuEq / 3;
                fug1 = p * Math.Exp(z1 - 1 - Math.Log(z1 - Bterm) - Aterm / Bterm / 2.8284
                    * Math.Log((z1 + 2.4142 * Bterm) / (z1 - 0.4142 * Bterm)));
                vol1 = z1 * Rgas * t / p;
                enthdep1 = Rgas * t * (z1 - 1 - Aterm / Bterm / 2.8284 * (1 + kappa
                    * Math.Sqrt(tr / alpha)) * Math.Log((z1 + 2.4142 * Bterm)
                    / (z1 - 0.4142 * Bterm)));
                Udep1 = enthdep1 - (z1 - 1) * Rgas * t;

                Entrdep1 = Rgas * Math.Log(z1 - Bterm) - Aterm * Rgas / Bterm / 2.8284
                    * kappa * Math.Sqrt(tr / alpha) * Math.Log((z1 + 2.4142 * Bterm)
                    / (z1 - 0.4142 * Bterm));

                enthdep2 = enthdep1;
                enthdep3 = enthdep1;

                if (p > pc && t > tc)
                    state = enumFluidRegion.SuperCritical;
                else if (p > pc && t < tc)
                    state = enumFluidRegion.Liquid;
                else if (p < pc && t > tc)
                    state = enumFluidRegion.Gaseous;
                else if (p < pc && t < tc)
                    state = enumFluidRegion.Liquid;
            }

            fugratio = fug1 / fug3;  // Vary P until Fugratio = 1 to get vapour Pressure , vary T until Fugratio = 1 to get sat T
        }

        public static void ReferenceStates(BaseComp bc)  //  Dependent on T and P
        {
            double omega = bc.Omega;

            p = ThermodynamicsClass.basep / 10;       // reference state 1 bar
            t = ThermodynamicsClass.baseTk + 273.15;  // reference t = 25C

            pr = p / (bc.CritP / 10);
            tr = t / (bc.CritT + 273.15);

            kappa = 0.37464 + 1.54226 * omega - 0.26992 * Math.Pow(omega, 2);
            b = 0.0777960739 * Rgas * tc / pc;

            alpha = Math.Pow((1 + kappa * (1 - Math.Sqrt(tr))), 2);
            a = 0.4572355289 * Math.Pow((tc * Rgas), 2) * alpha / pc;

            Aterm = p * a / Math.Pow((Rgas * t), 2);
            Bterm = b * p / Rgas / t;
            a2CuEq = -(1 - Bterm);
            a1CuEq = Aterm - 3 * Math.Pow(Bterm, 2) - 2 * Bterm;
            a0CuEq = -(Aterm * Bterm - Bterm * Bterm - Bterm * Bterm * Bterm);
            pCuEq = (3 * a1CuEq - a2CuEq * a2CuEq) / 3;
            qCuEq = (2 * a2CuEq * a2CuEq * a2CuEq - 9 * a2CuEq * a1CuEq + 27 * a0CuEq) / 27;

            testroot = qCuEq * qCuEq / 4 + pCuEq * pCuEq * pCuEq / 27;

            if (testroot < 0) // three roots
            {
                isoneroot = false;
                m = 2 * Math.Sqrt(-pCuEq / 3);
                m2m = 3 * qCuEq / pCuEq / m;
                m3m = Math.Acos(m2m);
                theta1 = m3m / 3;

                root1 = m * Math.Cos(theta1);
                root2 = m * Math.Cos(theta1 + 4 * 3.14159 / 3);
                root3 = m * Math.Cos(theta1 + 2 * 3.14159 / 3);

                z1 = root1 - a2CuEq / 3;
                z2 = root2 - a2CuEq / 3;
                z3 = root3 - a2CuEq / 3;

                fug1 = p * Math.Exp(z1 - 1 - Math.Log(z1 - Bterm) - Aterm / Bterm / 2.8284 * Math.Log((z1 + 2.4142 * Bterm) / (z1 - 0.4142 * Bterm)));
                fug2 = p * Math.Exp(z2 - 1 - Math.Log(z2 - Bterm) - Aterm / Bterm / 2.8284 * Math.Log((z2 + 2.4142 * Bterm) / (z2 - 0.4142 * Bterm)));
                fug3 = p * Math.Exp(z3 - 1 - Math.Log(z3 - Bterm) - Aterm / Bterm / 2.8284 * Math.Log((z3 + 2.4142 * Bterm) / (z3 - 0.4142 * Bterm)));

                vol1 = z1 * Rgas * t / p;
                vol2 = z2 * Rgas * t / p;
                vol3 = z3 * Rgas * t / p;

                BaseEnthdep1 = Rgas * t * (z1 - 1 - Aterm / Bterm / 2.8284 * (1 + kappa * Math.Sqrt(tr / alpha)) * Math.Log((z1 + 2.4142 * Bterm) / (z1 - 0.4142 * Bterm)));
                BaseEnthdep2 = Rgas * t * (z2 - 1 - Aterm / Bterm / 2.8284 * (1 + kappa * Math.Sqrt(tr / alpha)) * Math.Log((z2 + 2.4142 * Bterm) / (z2 - 0.4142 * Bterm)));
                BaseEnthdep3 = Rgas * t * (z3 - 1 - Aterm / Bterm / 2.8284 * (1 + kappa * Math.Sqrt(tr / alpha)) * Math.Log((z3 + 2.4142 * Bterm) / (z3 - 0.4142 * Bterm)));

                Udep1 = enthdep1 - (z1 - 1) * Rgas * t;
                Udep2 = enthdep2 - (z2 - 1) * Rgas * t;
                Udep3 = enthdep3 - (z3 - 1) * Rgas * t;

                BaseEntrdep1 = Rgas * Math.Log(z1 - Bterm) - Aterm * Rgas / Bterm / 2.8284 * kappa * Math.Sqrt(tr / alpha) * Math.Log((z1 + 2.4142 * Bterm) / (z1 - 0.4142 * Bterm));
                BaseEntrdep3 = Rgas * Math.Log(z3 - Bterm) - Aterm * Rgas / Bterm / 2.8284 * kappa * Math.Sqrt(tr / alpha) * Math.Log((z3 + 2.4142 * Bterm) / (z3 - 0.4142 * Bterm));
            }
            else  // one root
            {
                isoneroot = true;
                PPz = Math.Pow((-qCuEq / 2 + Math.Sqrt(testroot)), (1f / 3f));
                QQz = (-qCuEq / 2 - Math.Sqrt(testroot));

                root1 = PPz + QQz;
                root2 = 0;
                root3 = 0;

                PRZ = root1 - a2CuEq / 3;

                z1 = root1 - a2CuEq / 3;
                fug1 = p * Math.Exp(z1 - 1 - Math.Log(z1 - Bterm) - Aterm / Bterm / 2.8284
                    * Math.Log((z1 + 2.4142 * Bterm) / (z1 - 0.4142 * Bterm)));
                vol1 = z1 * Rgas * t / p;
                BaseEnthdep1 = Rgas * t * (z1 - 1 - Aterm / Bterm / 2.8284 * (1 + kappa
                    * Math.Sqrt(tr / alpha)) * Math.Log((z1 + 2.4142 * Bterm)
                    / (z1 - 0.4142 * Bterm)));
                Udep1 = enthdep1 - (z1 - 1) * Rgas * t;

                BaseEnthdep1 = Rgas * Math.Log(z1 - Bterm) - Aterm * Rgas / Bterm / 2.8284
                    * kappa * Math.Sqrt(tr / alpha) * Math.Log((z1 + 2.4142 * Bterm)
                    / (z1 - 0.4142 * Bterm));

                BaseEnthdep2 = BaseEnthdep1;
                BaseEnthdep3 = BaseEnthdep1;
            }

            if (gasrefstate == GasReferenceState.Ideal)
            {
                if (enthrefstate == EnthalpyReferenceState.EnthalpyZero)
                {
                    bc.RefEnthalpyDep1 = BaseEnthdep1;
                    bc.RefEnthalpyDep3 = BaseEnthdep3;
                    bc.RefEntropyDep1 = BaseEntrdep1;
                }
                else
                {
                    bc.RefEnthalpyDep1 = BaseEnthdep1;
                    bc.RefEnthalpyDep3 = BaseEnthdep3;
                    bc.RefEntropyDep1 = BaseEntrdep1;
                }
            }
            else
            {
                if (enthrefstate == EnthalpyReferenceState.EnthalpyZero)
                {
                    bc.RefEnthalpyDep1 = BaseEnthdep1;
                    bc.RefEnthalpyDep3 = BaseEnthdep3;
                    bc.RefEntropyDep1 = BaseEntrdep1;
                }
                else
                {
                    bc.RefEnthalpyDep1 = BaseEnthdep1;
                    bc.RefEnthalpyDep3 = BaseEnthdep3;
                    bc.RefEntropyDep1 = BaseEntrdep1;
                }
            }
            fugratio = fug1 / fug3;  // Vary P until Fugratio = 1 to get vapour Pressure , vary T until Fugratio = 1 to get sat T
        }

        public static double ZCompress(double t, double p, int RootNo)
        {
            double a2, a1, a0, A, B, At, Bt, troot, zcompress;
            double m, m2, m3, theta;
            double Root1, Root2, Root3, Z1, Z2, Z3;

            A = a;
            B = b;
            Bt = Bterm;
            At = Aterm;

            a2 = a2CuEq;
            a1 = a1CuEq;
            a0 = a0CuEq;

            troot = testroot;

            if (troot > 0)  //1 root
                zcompress = root1 - a2CuEq / 3;
            else
            {
                m = 2 * Math.Sqrt(-pCuEq / 3);
                m2 = 3 * qCuEq / pCuEq / m;
                m3 = Math.Acos(m2m);
                theta = m3 / 3;

                Root1 = m * Math.Cos(theta);
                Root2 = m * Math.Acos(theta + 4 * 3.14159 / 3);
                Root3 = m * Math.Acos(theta + 2 * 3.14159 / 3);

                Z1 = Root1 - a2 / 3;
                Z2 = Root2 - a2 / 3;
                Z3 = Root3 - a2 / 3;

                if (RootNo == 1)
                    zcompress = Z1;
                else if (RootNo == 2)
                    zcompress = Z2;
                else
                    zcompress = Z3;
            }
            return zcompress;
        }
    }
}