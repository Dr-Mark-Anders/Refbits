namespace ModelEngine
{
    /// <summary>
    /// Steam and Water property Calculations.
    /// </summary>
    ///

    public class Steam1967
    {
        /* keenan.c
		   Steam table formulas.
		   This C program computes thermodynamic properties of water in the liquid
		   or vapor phase.  Independent variables are the Temperature  and either the
		   specific volume or the Pressure .  Outputs are the Pressure  or specific
		   volume and the specific public  energy, specific enthalpy, specific
		   entropy, and saturation Pressure .  The method used is from J. Keenan
		   et al., _Steam Tables_, 1969.

		   Reference:
		   Keenan, Joseph H., Frederick G. Keyes, Philip G. Hill, Joan G. Moore,
		   Steam Tables. Thermodynamic Properties of Water Including Vapor,
		   Liquid, and Solid Phases_, Wiley, 1969.
		   Appendix, pp 134 ff.

		   Outputs from this program have been spot checked for about 100 p,T pairs
		   against the tables provided.  Be aware, however, that this formulation
		   is not identical to ASME.

		   Program by Stephen L. Moshier
		   January, 2001  */

        /*
		 * Auxiliary function
		 *
		 *           5
		 *           -        i
		 *  psi0  =  >  C  / t    +   C  ln T   +   C  (ln T) / t
		 *           -   i             6             7
		 *          i=0
		 */

        /* All coefficients here are from Keenan et al.  */
        private static double[] C = new double[8] { 1857.065, 3229.12, -419.465, 36.6649, -20.5516, 4.85233, 46.0, -1011.249 };

        public static double psi0(double T)
        {
            double z, s, y;

            z = T / 1000.0;     /* 1/tau */
            y = ((((C[5] * z + C[4]) * z + C[3]) * z + C[2]) * z + C[1]) * z + C[0];
            s = Math.Log(T);
            y = y + C[6] * s + C[7] * s * z;
            return y;
        }

        /*
		 * Auxiliary function
		 *
		 *                6             [  7                         9         ]
		 *                -             [  -               i     -Er -     i-8 ]
		 *  Q = (t - t )  >  (t - t  )  [  >  A   (r - r  )  +  e    > A  r    ]
		 *            c   -        aj   [  -   ij       aj           -  ij     ]
		 *               j=0            [ i=0                       i=8        ]
		 *
		 *  r is the density, rho, in grams per cubic centimeter
		 *  t is 1000 / T
		 *  E = 4.8
		 *
		 *  t  = 1000/Tcrit = 1.544912
		 *   c
		 *
		 *  t   = 2.5 if j > 0, else t  if j = 0.
		 *   aj                       c
		 *
		 *  r   = 1.0 if j > 0, else 0.634 if j = 0.
		 *   aj
		 */

        private static double[,] A = new double[,]{{29.492937, -5.1985860, 6.8335354, -0.1564104, -6.3972405, -3.9661401,-0.69048554},
{-132.13917, 7.7779182, -26.149751, -0.72546108, 26.409282, 15.453061,2.7407416},
    {274.64632, -33.301902, 65.326396, -9.2734289, -47.740374, -29.142470,-5.1028070},
    {-360.93828, -16.254622, -26.181978, 4.3125840, 56.323130, 29.568796,3.9636085},
    {342.18431, -177.31074, 0.0, 0.0, 0.0, 0.0, 0.0},
    {-244.50042, 127.48742, 0.0, 0.0, 0.0, 0.0, 0.0},
    {155.18535, 137.46153, 0.0, 0.0, 0.0, 0.0, 0.0},
    {5.9728487, 155.97836, 0.0, 0.0, 0.0, 0.0, 0.0},
    {-410.30848, 337.31180, -137.46618, 6.7874983, 136.87317, 79.847970,13.041253},
    {-416.05860, -209.88866, -733.96848, 10.401717, 645.81880, 399.17570,71.531353}};

        private static double tauc = 1.544912;      /* 1000/Tcrit */
        private static double E = 4.8;

        private static double Q(double rho, double tau)
        {
            double e, p, q, s, y;
            double[] u = new double[7];
            int j;

            e = Math.Exp(-E * rho);
            for (j = 0; j < 7; j++)
            {
                if (j > 0)
                    p = rho - 1.0;
                else
                    p = rho - 0.634;
                s = ((((((A[7, j] * p + A[6, j]) * p + A[5, j]) * p + A[4, j]) * p
                    + A[3, j]) * p + A[2, j]) * p + A[1, j]) * p + A[0, j];
                u[j] = s + (A[9, j] * rho + A[8, j]) * e;
            }

            p = tau - 2.5;
            q = tau - tauc;
            y =
                ((((u[6] * p + u[5]) * p + u[4]) * p + u[3]) * p + u[2]) * p + u[1] +
                u[0] / q;
            y = q * y;
            return y;
        }

        /*
		 * Helmholtz free energy
		 *
		 *  psi = psi0(T) + R T [ ln r + r Q(r, t)
		 *
		 *  r is the density, rho, in grams per cubic centimeter
		 *  t is 1000 / T
		 *  Temperature  T is in degrees Kelvin
		 */

        private static double R = 0.46151;      /* J / g K */

        private static double hfe(double rho, double T)
        {
            double y;
            y = psi0(T) + R * T * (Math.Log(rho) + rho * Q(rho, 1000.0 / T));
            return y;
        }

        /* Derivatives.  */

        /* Partial derivative of Q with respect to rho.  */

        private static double dQ_drho(double rho, double tau)
        {
            double e, p, q, s, y;
            double[] u = new double[7];
            int j;

            e = Math.Exp(-E * rho);
            for (j = 0; j < 7; j++)
            {
                if (j > 0)
                    p = rho - 1.0;
                else
                    p = rho - 0.634;

                s =
                    (((((7.0
                    * A[7, j] * p + 6.0 * A[6, j]) * p + 5.0 * A[5, j]) * p +
                    4.0 * A[4, j]) * p + 3.0 * A[3, j]) * p + 2.0 * A[2, j]) * p +
                    A[1, j];
                u[j] = s + A[9, j] * (e - E * e * rho) - E * e * A[8, j];
            }
            p = tau - 2.5;
            q = tau - tauc;
            y =
                ((((u[6] * p + u[5]) * p + u[4]) * p + u[3]) * p + u[2]) * p + u[1] +
                u[0] / q;
            y = q * y;
            return y;
        }

        /* Partial derivative of Q with respect to tau.  */

        private static double dQ_dtau(double rho, double tau)
        {
            double e, p, q, s, r, dr, y;
            double[] u = new double[7];
            int j;

            e = Math.Exp(-E * rho);
            for (j = 0; j < 7; j++)
            {
                if (j > 0)
                    p = rho - 1.0;
                else
                    p = rho - 0.634;
                s = ((((((A[7, j] * p + A[6, j]) * p + A[5, j]) * p + A[4, j]) * p
                    + A[3, j]) * p + A[2, j]) * p + A[1, j]) * p + A[0, j];
                u[j] = s + (A[9, j] * rho + A[8, j]) * e;
            }

            p = tau - 2.5;
            q = tau - tauc;
            r =
                ((((u[6] * p + u[5]) * p + u[4]) * p + u[3]) * p + u[2]) * p + u[1] +
                u[0] / q;
            dr =
                (((5.0 * u[6] * p + 4.0 * u[5]) * p + 3.0 * u[4]) * p + 2.0 * u[3]) * p +
                u[2] - u[0] / (q * q);
            y = q * dr + r;
            return y;
        }

        /* Derivative of psi0 with respect to tau.  */

        private static double dpsi0_dtau(double T)
        {
            double z, y;

            z = T / 1000.0;     /* 1/tau */
            y =
                -((((5.0 * C[5] * z + 4.0 * C[4]) * z + 3.0 * C[3]) * z + 2.0 * C[2]) * z +
                C[1]) * z * z;
            y = y - C[6] * z - C[7] * z * z * (Math.Log(T) + 1.0);
            return y;
        }

        /* Derivative of psi0 with respect to T.  */

        private static double dpsi0_dT(double T)
        {
            double z, s, y;

            z = T / 1000.0;     /* 1/tau */
            y =
                ((((5.0 * C[5] * z + 4.0 * C[4]) * z + 3.0 * C[3]) * z + 2.0 * C[2]) * z +
                C[1]) / 1000.0;
            s = Math.Log(T);
            y = y + C[6] / T + C[7] * (1.0 + s) / 1000.0;

            return y;
        }

        /*
		 * Pressure .
		 *                                2
		 * p = rho R T { 1  +  rho Q + rho  dQ/drho
		 *
		 */

        private static double Pressure(double rho, double T)
        {
            double p, tau;

            /* 1 kg/m^3 = .001 g / cm^3 */
            rho = 0.001 * rho;

            tau = 1000.0 / T;
            p = (dQ_drho(rho, tau) * rho + Q(rho, tau)) * rho + 1.0;
            p = 10.0 * rho * R * T * p;

            return p;
        }

        /* return  s joules per gram.  */

        public static double public_energy(double rho, double T)
        {
            double tau, u;

            rho = 0.001 * rho;
            tau = 1000.0 / T;
            u = tau * (hfe(rho, T / 1.0001) - hfe(rho, T)) / (0.0001 * tau) + hfe(rho, T);
            u = tau * dpsi0_dtau(T) + psi0(T);
            u = u + rho * tau * R * T * dQ_dtau(rho, tau);

            return u;
        }

        /*
		 *  specific enthalpy = u + p v
		 *  v = specific volume
		 */

        public static double specific_enthalpy(double rho, double T)
        {
            double h;
            h = public_energy(rho, T) + 100.0 * Pressure(rho, T) / rho;
            return h;
        }

        public static double specific_entropy(double rho, double T)
        {
            double tau, s;
            rho = 0.001 * rho;
            tau = 1000.0 / T;

            s = Math.Log(rho) + rho * Q(rho, tau) - rho * tau * dQ_dtau(rho, tau);
            s = -R * s - dpsi0_dT(T);

            return s;
        }

        /* Saturation vapor Pressure .
		 * Keenan et al, op cit, p 141.
		 *
		 *                                   7
		 *                      -5           -                    k
		 * ln (p / p )  = tau 10   (t  - t)  >  F  (0.65 - 0.01 t)
		 *      s   c                c       -   k
		 *                                  k=0
		 *
		 * p  = critical Pressure
		 *  c
		 *
		 * t  = critical temparature
		 *  c
		 *
		 * t = Temperature , degrees Centigrade
		 *
		 * tau = 1000/T, T = Kelvin Temperature
		 *
		 */

        private static double[] F = new double[8] { -741.9242, -29.72100, -11.55286, -0.8685635, 0.1094098, 0.439993, 0.2520658, 0.05218684 };

        /* Critical Temperature , degrees C */
        private static double TC = 374.136;

        /* Critical Pressure , bars */
        private static double PC = 220.88;

        //
        private static double psat(double T)
        {
            double tau, t, f, p;
            tau = 1000.0 / T;
            t = T - 273.15;     /* Centigrade */
            f = (0.65 - 0.01 * t);
            p = ((((((F[7] * f + F[6]) * f + F[5]) * f + F[4]) * f + F[3]) * f + F[2]) *
                f + F[1]) * f + F[0];
            p = 1e-5 * tau * (TC - t) * p;
            p = PC * Math.Exp(p);
            return Math.Round(p, 4);
        }

        /* Iterate to find density as a function of Pressure  and Temperature .  */

        public static double specific_volume(double p0, double T, bool vapor)
        {
            double lr, r, hr, lp, p, hp, z;
            int iter;

            /* Boyle's law approximation for the vapor phase.  */
            /* FIXME: assuming English units input.  */
            if (vapor)
            {
                r = 220.0 * p0 / T;
                p = Pressure(r, T);
                z = 0.707;
            }
            else
            {
                r = 1000.0;     /* kg / m^3 */
                p = Pressure(r, T);
                z = 0.99;
            }
            /* Bracket the solution. */
            iter = 0;
            if (p > p0)
            {
                hp = p;
                hr = r;
                while (p > p0)
                {
                    r = z * r;
                    z = z * z;
                    p = Pressure(r, T);
                    if (++iter > 10)
                    {
                        return -1;
                    }
                }
                lp = p;
                lr = r;
            }
            else
            {
                lp = p;
                lr = r;
                z = 1.0 / z;
                while (p < p0)
                {
                    r = z * r;
                    z = z * z;
                    p = Pressure(r, T);
                    if (++iter > 10)
                    {
                        return -1;
                    }
                }
                hp = p;
                hr = r;
            }
            do
            {
                /* new  guess.  */
                z = (p0 - lp) / (hp - lp);
                if (z < 0.1)
                    z = 0.3;
                if (z > 0.9)
                    z = 0.7;
                r = lr + z * (hr - lr);
                p = Pressure(r, T);
                if (p > p0)
                {
                    hp = p;
                    hr = r;
                }
                else
                {
                    lp = p;
                    lr = r;
                }
                z = (hp - lp) / p0;
                if (++iter > 30)
                {
                    break;
                }
            }
            while (z > 1.0e-6);
            return (1.0 / r);
        }

        /* Test program.  */

        public static int test()
        {
            double T = 0, rho = 0, p = 10, u, s, v, h, ps;
            bool vapor = true, input_Pressure = true;

            Console.Write("\nSteam tables according to Keenan et al, 1969.\n");

            Console.Write("  p: Pressure  in bars.\n");
            Console.Write("  v: Specific volume, cubic meters per kilogram.\n");
            Console.Write("  u: Specific public  energy, kilojoules per kilogram.\n");
            Console.Write("  h: Specific enthalpy, kilojoules per kilogram.\n");
            Console.Write("  s: Specific entropy, kilojoules per kilogram degree Kelvin.\n");
            Console.Write("  psat: Saturation Pressure , bars.\n");

            Console.Write("\nEnter 1 for vapor, 0 for liquid phase ? \n");
            vapor = Convert.ToBoolean(Console.ReadLine());
            Console.Write("\nEnter 1 to input the Pressure , 0 to input specific volume ? \n");
            input_Pressure = Convert.ToBoolean(Console.ReadLine());
        loop:

            Console.Write("Temperature  (deg K) ? ");

            //Console.ReadLine ("%lf", &T);

            if (input_Pressure)
            {
                /* Input is specific volume.  */

                Console.Write("Specific volume (cubic meters per kilogram) ? ");
                rho = Convert.ToDouble(Console.ReadLine());
                rho = 1.0 / rho;
                p = Pressure(rho, T);
                Console.WriteLine("p = %.5g\n", p);
            }
            else
            {
                /* Input is Pressure .  */
                Console.Write("Pressure  (bars) ? ");
                p = Convert.ToDouble(Console.ReadLine());
            }

            /* Domain checks.  */

            if (vapor && ((T < 255) || (T > 1588) || (p < 0) || (p > 1034)))
            {
                Console.Write("Limits are T <= 1588 deg K, p < 1034 bar.\n");
                goto loop;
            }
            if (vapor && ((T < 255) || (T > 650) || (p < 0) || (p > 1378)))
            {
                Console.Write("Limits are T <= 650 deg K, p < 1378 bar.\n");
                goto loop;
            }

            v = specific_volume(p, T, vapor);
            rho = 1.0 / v;
            u = public_energy(rho, T);
            h = specific_enthalpy(rho, T);
            s = specific_entropy(rho, T);
            Console.Write("v %.5g\n", v);
            Console.Write("u %.5g\n", u);
            Console.Write("h %.5g\n", h);
            Console.Write("s %.5g\n", s);
            ps = psat(T);
            Console.Write("psat = %.5f\n", ps);
            if (vapor && (p > ps))
                Console.Write("Pressure  of vapor exceeds saturatation Pressure .\n");
            if (vapor && (p < ps))
                Console.Write("Pressure  of liquid is less than saturation Pressure .\n");
            goto loop;
        }

        public static SteamStruct GetProps(double Pbars, double Tc, double Enthalpy)
        {
            double Tk;

            Tk = Tc + 273.15;

            double rho = 0, pst = 0, enth = 0, entr = 0, spvol = 0, inenergy = 0, tst = 0;
            pst = psat(Tk);
            bool Vapour;
            Vapour = true;
            if (Pbars < pst)  // Vapour
            {
                Vapour = true;
            }
            else if (Pbars > pst)
            {
                Vapour = false;
            }
            else
            {
                return GetSatPropsFromEnthalpy(Pbars, Enthalpy);
            }
            spvol = specific_volume(Pbars, Tk, Vapour);
            rho = 1 / spvol;

            enth = specific_enthalpy(rho, Tk);
            entr = specific_entropy(rho, Tk);
            inenergy = public_energy(rho, Tk);
            tst = tsat(Pbars);
            return new SteamStruct(Pbars, pst, Tk - 273.15, tst - 273.15, enth, entr, spvol, inenergy, Convert.ToDouble(Vapour));
        }

        public static SteamStruct GetSatProps(double Pbars, bool Vapour)
        {
            double rho = 0, pst = 0, enthg = 0, entrg = 0, spvolg, inenergyg = 0, tst = 0, sf = 0;

            tst = tsat(Pbars);

            spvolg = specific_volume(Pbars, tst, Vapour);
            rho = 1 / spvolg;
            enthg = specific_enthalpy(rho, tst);
            entrg = specific_entropy(rho, tst);
            inenergyg = public_energy(rho, tst);
            if (Vapour)
                sf = 1;
            else
                sf = 0;

            return new SteamStruct(Pbars, pst, tst - 273.15, tst - 273.15, enthg, entrg, spvolg, inenergyg, sf);
        }

        public static SteamStruct GetPropsFromEntropy(double Pbars, double entropy)
        {
            double rho = 0, pst = 0, t,
                enth = 0, entr = 0, spvol = 0, inenergy = 0, tst = 0;

            tst = tsat(Pbars);
            SteamStruct pg = GetSatProps(Pbars, true);
            SteamStruct pl = GetSatProps(Pbars, false);
            bool Vapour;
            Vapour = true;

            double TStart, TEnd;

            if (entropy < pl.entropy)  // Sub Cooled
            {
                Vapour = false;
                TStart = 200;
                TEnd = tst;
            }
            else if (entropy > pg.entropy) // Superheated
            {
                Vapour = true;
                TStart = tst;
                TEnd = 1000;
            }
            else  // Saturated
            {
                return GetSatPropsFromEntropy(Pbars, entropy);
            }
            double inc = 10;

            for (t = TStart; t < TEnd; t += inc)  //Centigrade
            {
                spvol = specific_volume(Pbars, t, Vapour);
                rho = 1 / spvol;
                entr = specific_entropy(rho, t);
                if (entr > entropy)
                {
                    t = t - inc;
                    inc = inc / 2;
                }
                if (Math.Abs(entr - entropy) < 0.000001)
                {
                    break;   //kelvin
                }
                if (t < TStart | t > TEnd)
                {
                    System.Diagnostics.Debug.WriteLine("T outside Range for MolarEntropy Calc");
                }
            }

            pst = psat(t);

            enth = specific_enthalpy(rho, t);
            inenergy = public_energy(rho, t);

            return new SteamStruct(Pbars, pst, t - 273.15, tst - 273.15, enth, entr, spvol, inenergy, Convert.ToDouble(Vapour));
        }

        public static SteamStruct GetPropsFromEnthalpy(double Pbars, double enthalpy)
        {
            double rho = 0, pst = 0, t = 0, enth = 0, entr = 0, spvol = 0, inenergy = 0, tst = 0;

            tst = tsat(Pbars);
            double inc = 50;
            bool Vapor;
            Vapor = false;

            SteamStruct pg = GetSatProps(Pbars, true);
            SteamStruct pl = GetSatProps(Pbars, false);

            double TStart, TEnd;
            if (enthalpy < pl.enthalpy)  // Sub Cooled
            {
                Vapor = false;
                TStart = 270;
                TEnd = tst;
            }
            else if (enthalpy > pg.enthalpy) // Superheated
            {
                Vapor = true;
                TStart = tst;
                TEnd = 1000;
            }
            else  // Saturated
            {
                return GetSatPropsFromEnthalpy(Pbars, enthalpy);
            }

            for (t = TStart; t < TEnd + inc; t += inc)  //Centigrade
            {
                spvol = specific_volume(Pbars, t, Vapor);
                rho = 1 / spvol;
                enth = specific_enthalpy(rho, t);
                if (inc < 0.000000001)
                    break;
                if (enth > enthalpy)
                {
                    t = t - inc;
                    inc = inc / 2;
                }
                if (Math.Abs(enth - enthalpy) < 0.01)
                {
                    break;   //kelvin
                }
            }

            pst = psat(t);

            entr = specific_entropy(rho, t);
            enth = specific_enthalpy(rho, t);
            inenergy = public_energy(rho, t);

            return new SteamStruct(Pbars, pst, t - 273.15, tst - 273.15, enth, entr, spvol, inenergy, Convert.ToDouble(Vapor));
        }

        public static SteamStruct GetSatPropsFromEnthalpy(double Pbars, double enthalpy)
        {
            double rho = 0, pst = 0,
                enth = 0, entr = 0, spvol, inenergy = 0,
                enthl = 0, entrl = 0, spvoll, inenergyl = 0,
                enthg = 0, entrg = 0, spvolg, inenergyg = 0, tst = 0;

            tst = tsat(Pbars);

            spvolg = specific_volume(Pbars, tst, true);
            rho = 1 / spvolg;
            enthg = specific_enthalpy(rho, tst);
            entrg = specific_entropy(rho, tst);
            inenergyg = public_energy(rho, tst);

            spvoll = specific_volume(Pbars, tst, false);
            rho = 1 / spvoll;
            enthl = specific_enthalpy(rho, tst);
            entrl = specific_entropy(rho, tst);
            inenergyl = public_energy(rho, tst);

            double SteamMassFraction = 1 - (enthg - enthalpy) / (enthg - enthl);

            enth = enthg * SteamMassFraction + enthl * (1 - SteamMassFraction);
            entr = entrg * SteamMassFraction + entrl * (1 - SteamMassFraction);
            inenergy = inenergyg * SteamMassFraction + inenergyl * (1 - SteamMassFraction);
            spvol = spvolg * SteamMassFraction + spvoll * (1 - SteamMassFraction);

            return new SteamStruct(Pbars, pst, tst - 273.15, tst - 273.15,
                enth, entr, spvol, inenergy, SteamMassFraction);
        }

        public static SteamStruct GetSatPropsFromEntropy(double Pbars, double entropy)
        {
            double rho = 0, pst = 0,
                enth = 0, entr = 0, spvol = 0, inenergy = 0,
                enthl = 0, entrl = 0, spvoll, inenergyl = 0,
                enthg = 0, entrg = 0, spvolg, inenergyg = 0, tst = 0;

            tst = tsat(Pbars);

            pst = psat(tst);

            spvolg = specific_volume(Pbars, tst, true);
            rho = 1 / spvolg;
            enthg = specific_enthalpy(rho, tst);
            entrg = specific_entropy(rho, tst);
            inenergyg = public_energy(rho, tst);

            spvoll = specific_volume(Pbars, tst, false);
            rho = 1 / spvoll;
            enthl = specific_enthalpy(rho, tst);
            entrl = specific_entropy(rho, tst);
            inenergyl = public_energy(rho, tst);

            double SteamMassFraction = 1 - (entrg - entropy) / (entrg - entrl);

            enth = enthg * SteamMassFraction + enthl * (1 - SteamMassFraction);
            entr = entrg * SteamMassFraction + entrl * (1 - SteamMassFraction);
            inenergy = inenergyg * SteamMassFraction + inenergyl * (1 - SteamMassFraction);
            spvol = spvolg * SteamMassFraction + spvoll * (1 - SteamMassFraction);

            return new SteamStruct(Pbars, pst, tst - 273.15, tst - 273.15,
                enth, entr, spvol, inenergy, SteamMassFraction);
        }

        public static SteamStruct GetSatPropsFromSteamFrac(double Pbars, double SteamFrac)
        {
            double rho = 0, pst = 0,
                enth = 0, entr = 0, spvol = 0, inenergy = 0,
                enthl = 0, entrl = 0, spvoll, inenergyl = 0,
                enthg = 0, entrg = 0, spvolg, inenergyg = 0, tst = 0;

            tst = tsat(Pbars);

            pst = psat(tst);

            spvolg = specific_volume(Pbars, tst, true);
            rho = 1 / spvolg;
            enthg = specific_enthalpy(rho, tst);
            entrg = specific_entropy(rho, tst);
            inenergyg = public_energy(rho, tst);

            spvoll = specific_volume(Pbars, tst, false);
            rho = 1 / spvoll;
            enthl = specific_enthalpy(rho, tst);
            entrl = specific_entropy(rho, tst);
            inenergyl = public_energy(rho, tst);

            enth = enthg * SteamFrac + enthl * (1 - SteamFrac);
            entr = entrg * SteamFrac + entrl * (1 - SteamFrac);
            inenergy = inenergyg * SteamFrac + inenergyl * (1 - SteamFrac);
            spvol = spvolg * SteamFrac + spvoll * (1 - SteamFrac);

            return new SteamStruct(Pbars, pst, tst - 273.15, tst - 273.15,
                enth, entr, spvol, inenergy, SteamFrac);
        }

        public static double Enthalpy(double t, double p)
        {
            double enth = 0;
            double rho = 0;
            rho = 1 / specific_volume(p, t, true);
            enth = specific_enthalpy(rho, t);
            return enth;
        }

        public static double Entropy(double t, double p)
        {
            double ent = 0;
            double rho = 0;
            rho = 1 / specific_volume(p, t, true);
            ent = specific_entropy(rho, t);
            return ent;
        }

        private static double Temp(double enth, double p)
        {
            float inc = 10;
            double e = 0;
            for (double T = 0; T < 100; T += inc)  //Centigrade
            {
                e = Enthalpy(T + 273.15, p);
                if (e > enth)
                {
                    T = T - inc;
                    inc = inc / 2;
                }
                if (Math.Abs(e - enth) < 0.0001)
                {
                    return T - 273.15;   //kelvin
                }
            }
            return 0;
        }

        public static double TsatC(double P)
        {
            return tsat(P) - 273.15;
        }

        private static double tsat(double P)
        {
            if (P < 0.05)
            {
                return double.NaN;
            }
            else if (P != 0)
            {
                float inc = 50;
                double p = 0;
                for (double T = -200; T < 1000; T += inc)  //Centigrade
                {
                    p = psat(T + 273.15);
                    if (p > P)
                    {
                        T = T - inc;
                        inc = inc / 2;
                    }
                    if (Math.Abs(p - P) < 0.0001)
                    {
                        return T + 273.15;   //kelvin
                    }
                }
            }
            return 0;
        }
    }
}