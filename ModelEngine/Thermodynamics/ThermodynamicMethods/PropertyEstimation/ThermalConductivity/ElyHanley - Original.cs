using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ThermoServer
{
    public class StewartJacobsen // methane
    {
        private readonly double R = 0.08205616;
        private readonly double PCO = 45.387;
        private readonly double DCO = 10.23;
        private readonly double TCO = 190.555;
        private readonly double ZCO = 0.283742;
        private readonly double WO = 0.01131;
        private readonly double CMWO = 16.043;
        private readonly double GAMMA = 0.0096;
        private double DOO = 0;
        private readonly double NLIB = 61;
        private double FX = 1, DFXDT = 0, HX = 1, CMXV = 16.043;
        private readonly string[] NAME = new string[20];
        private readonly double[] F = new double[20];
        private readonly double[] H = new double[20];

        private struct PROP
        {
            public string Name;
            public string shortname;
            public double Pc, Vc, Dc, Tc, Omega, MW, NBP_K, Z;
            public double[] Ideal_Heat;

            public PROP(object[] data)
            {
                this.Ideal_Heat = new double[8];

                Name = (string)data[0];
                this.shortname = (string)data[1];
                this.Pc = (double)data[2];
                this.Vc = (double)data[3];
                this.Dc = 1000 / Vc;
                this.Tc = (double)data[4];
                this.Omega = (double)data[5];
                this.MW = (double)data[6];
                this.NBP_K = (double)data[7];
                this.Z = 1;

                for (int n = 1; n <= 7; n++)
                {
                    this.Ideal_Heat[n] = (double)data[n + 7];
                }
            }
        }

        private readonly PROP[] PROPS = new PROP[20];
        private readonly PROP[] P = new PROP[20];
        private readonly double[] OMK = new double[190];
        private readonly double[] OML = new double[190];
        private double CMXT = 0, ZCX = 0, CORT = 0;
        private double VR = 0, TR = 0, CMX = 0, FXS = 0, HXS = 0, TMP2, TMP3;
        private double WN = 0;
        private int NC = 0;

        private readonly double[] A = new double[33]{0,-1.184347314485E-02,7.540377272657E-01,-1.225769717554E+01
          , 6.260681393432E+02,-3.490654409121E+04, 5.301046385532E-04
          ,-2.875764479978E-01, 5.011947936427E+01,-2.821562800903E+04
          ,-2.064957753744E-05, 1.285951844828E-02,-1.106266656726E+00
          , 3.060813353408E-04,-3.174982181302E-03, 5.191608004779E+00
          ,-3.074944210271E-04, 1.071143181503E-05,-9.290851745353E-03
          , 1.610140169312E-04, 3.469830970789E+04,-1.370878559048E+06
          , 1.790105676252E+02, 1.615890743238E+06, 6.265306650288E-01
          , 1.820173769533E+01, 1.449888505811E-03,-3.159999123798E+01
          ,-5.290335668451E-06, 1.694350244152E-03, 8.612049038886E-09
          ,-2.598235689063E-06, 3.153374374912E-05};

        private readonly double[] CT = new double[] { 0, 0.090569, -0.862762, 0.316636, -0.46568 };
        private readonly double[] CP = new double[] { 0, 0.394901, -1.023545, -0.932813, -0.754639 };

        //                      VISCOSITY CORRELATION CONSTANTS

        private readonly double[] CO = new double[10]{0, 0.2907741307E+07,-0.3312814033E+07, 0.1608101838E+07,
              -0.4331904871E+06, 0.7062481330E+05,-0.7116620750E+04,
               0.4325174400E+03,-0.1445911210E+02, 0.2037119479E+00};

        //    readonly  double [] CD = new  double [5]{0, 1.6969859271E+00, -1.3337234608E-01,1.4000000000E+00,
        //           1.6800000000E+02};
        //    readonly  double [] CE = new  double [9]{0,-1.0239160427E+01, 1.7422822961E+02, 1.7460545674E+01,
        //          -2.8475328289E+03, 1.3368502192E-01, 1.4207239767E+02,
        //           5.0020669720E+03, 1.6280000000E-01};

        //                 THERMAL CONDUCTIVITY CORRELATlON
        //                       CONSTANTS

        //    readonly  double [] COT = new  double [10]{0,-0.214762100E+06, 0.219046100E+06,-0.861809700E+05,
        //           0.149609900E+05,-0.473066000E+03,-0.233117800E+03,
        //          0.377843900E+02,-0.232048100E+01, 0.531176400E-01};
        private readonly double[] CDT = new double[5]{0,-0.252762920E+00, 0.334328590E+00, 1.120000000E+00,
              0.168000000E+03};

        private readonly double[] CET = new double[9]{0,-7.1977082270E+00, 8.5678222640E+01, 1.2471834689E+01,
              -9.8462522975E+02, 3.5946850007E-01, 6.9798412538E+01,
             -8.7288332851E+02, 0.1628000000E+00};

        // Cp in BTU/lbMole/F
        private readonly object[] XPRP01 = new object[]{ "METHANE", "C1",  45.387E0,    97.752E0,
            190.555E0,      .011314E0,      16.043E0,   111.630E0,
            .76692963E+01, .33677582E-14, .97978154E-05, .16480983E-07,
            -.19759199E-10,-.12233376E-21, .31782271E-17};

        private readonly object[] XPRP02 = new object[]{ "ETHANE", "C2",48.077E0, 147.060E0,
            305.330E0,      .10038E0,      30.070E0,     184.55050,
            .77063866E+01,  .4587991E-05,  .80959840E-04, -.85756975E-07,
            .13183583E-10,  .21689626E-13, -.85251740E-17};

        private readonly object[] XPRP03 = new object[]{ "PROPANE", "C3", 41.914E0,201.610E0,
            369.820E0,      .15418E0,      44.097E0,     231.050E0,
            .54902525E+01, .40763758E-01, .95793455E-05, -.92506902E-08,
            -.71806497E-11, .42920084E-18, .26266881E-17 };

        private readonly object[] XPRP04 = new object[]{"ISOBUTANE","IC4", 36.003E0, 263.000E0,
            408.150E0,       .18300E0,      58.124E0,     261.420E0,
            .83448658E+01,  .40337532E-02, .28869652E-03, -.60188092E-06,
            .57571458E-09, -.27078809E-12, .50258511E-16};

        private readonly object[] XPRP05 = new object[]{ "N-BUTANE", "C4", 37.465E0,256.410E0,
           425.160E0,.20038E0,58.124E0,272.650E0,
          .86549559E+01,.34204803E-01,.82496176E-04,-.97523439E-07,
          -.35770071E-11,.46492802E-13,-.16554141E-15};

        public void CreateData()
        {
            PROPS[1] = new PROP(XPRP01);
            PROPS[2] = new PROP(XPRP02);
            PROPS[3] = new PROP(XPRP03);
            PROPS[4] = new PROP(XPRP04);
            PROPS[5] = new PROP(XPRP05);
        }

        public void TRAPP(string component, double Temp, double press)
        {
            /************************************
            C PURPOSE  DEMONSTRATION DRIVER FOR EXTENDED CORRESPONDING
            C STATES ESTIMATION OF VISCOSITY AND THERMAL
            C CONDUCTIVITY OF HYDROCARBON MIXTURES.
            C
            C VERSION G2.3 8/11/80
            C
            C CODED BY: J. F. ELY
            C NATIONAL BUREAU OF STANDARDS
            C
            C NATIONAL ENGINEERING LABORATORY
            C THERMOPHYSICAL PROPERTIES DIVISION
            C BOULDER, COLORADO 80303
            C*************************************/

            double[] X = new double[20],
                TA = new double[2] { 0, 459.67 },
                TD = new double[2] { 1, 1.8 },
                PD = new double[2] { 1.013250, 14.695950 },
                DM = new double[2] { 1.00, 0.062427950 },
                VM = new double[2] { 1.00, 2.419E-4 },
                TM = new double[2] { 1.0E-3, 5.777675E-4 };

            bool MIX;
            double XSUM, TIN, PIN, TK, PATM, DX, ETAX, TCX;
            int IU;

            CreateData();

            DialogResult INOT = DialogResult.Yes;

            if (INOT == DialogResult.Yes)
            {
                NC = (int)(NLIB / 2);
                for (int K = 1; K <= 1; K++)
                {
                    Debug.Print(PROPS[K].Name);
                }
            }

            IU = 0;// SEE IF ENGINEERING UNITS ARE DESIRED

            INOT = DialogResult.No;
            if (INOT == DialogResult.Yes)
            { IU = 2; }

            X[1] = 1.0;    // INITIALIZE PROGRAM PARAMETERS, MOLE FRACTIONS

            MIX = false;
            //       INPUT THE NUMBER OF Components
            //       IF IT IS ZERO OR LESS, STOP.

            NC = 1;
            if (NC <= 0)
                return;
            if (NC > 1)
                MIX = true;

            for (int K = 1; K <= NC; K++) // LOAD COMP PROPERTIES
            {
                NAME[1] = component;
                LIB(K);
            }

            if (MIX)     // IF A MIXTURE, GET THE COMPOSITION
            {
                XSUM = 0.0;
                for (int K = 1; K <= NC; K++)
                {
                    if (X[K] < 0.0)
                    { X[K] = 0; }
                    XSUM += X[K];
                }
                for (int K = 1; K <= NC; K++)
                {
                    X[K] = X[K] / XSUM;
                }
            }

            TIN = Temp;
            PIN = press;
            TK = (TIN + TA[IU]) / TD[IU];      // CONVERT TO K AND ATM FOR CALCULATIONS
            PATM = PIN / PD[IU];
            DX = 0;
            DCST(PATM, ref DX, TK, X);         // HC MIXTURE DENSITY
            ETAX = VM[IU] * VSCTY(DX, TK);     // CALCULATE METHANE VISCOSITY AT EQUIVALENT DENS AND T
            TCX = TM[IU] * THERMC(DX, TK, X);
            DX = DX * CMX * DM[IU];

            Debug.Print(DX + " " + ETAX + " " + TCX);
        }

        public double THETAF(double TR, double VR, double WN)
        {
            double res;
            res = 1.0 + (WN - WO) * (CT[1] + CT[2] * Math.Log(TR) + (CT[3] + CT[4] / TR) * (VR - 0.5));
            return res;
        }

        public double PHIF(double TR, double VR, double WN)
        {
            double res;
            res = (1.0 + (WN - WO) * (CP[1] * (VR + CP[2]) + CP[3] * (VR + CP[4]) * Math.Log(TR)));
            return res;
        }

        public void DCST(double PX, ref double DX, double TX, double[] X)
        {
            /*C ***********************************
            C
            C PURPOSE --- THIS ROUTINE CALCULATES A HYDROCARBON MIXTURE
            C DENSITY using   THE SHAPE FACTOR APPROACH TO
            C CORRESPONDING STATES.*/

            double PO, TO;
            bool[] MASK = new bool[20];
            double TOLERS = 1.0E-5;
            int K;
            double GN, S2, S3;

            // INITIALIZE THE SHAPE FACTORS AND CORRESPONDING STATE PARAMETERS

            FX = 1.0;
            HX = 1.0;
            for (int N = 1; N <= NC; N++)
            {
                MASK[N] = false;
                if (X[N] <= 0.0)
                    MASK[N] = true;
                F[N] = P[N].Tc / TCO;   // reduced T
                H[N] = DCO / P[N].Dc;   // reduced density
            }

            for (int LOOP = 1; LOOP <= 15; LOOP++)    // SHAPE FACTOR ITERATION LOOP
            {
                //   SAVE RESULTS FROM PREVIOUS ITERATION
                FXS = FX;
                HXS = HX;
                HX = 0.0;
                FX = 0.0;

                // CALCULATE CST PARAMETERS FOR MIXTURE

                for (int N = 1; N <= NC; N++)
                {
                    if (!MASK[N])
                    {
                        K = N * (N - 1) / 2;
                        GN = Math.Pow(H[N], 1f / 3f);
                        S2 = 0.0;
                        S3 = 0.0;
                        for (int M = 1; M <= N; M++)
                        {
                            if (!MASK[M])
                            {
                                TMP2 = 0.125 * X[M] * Math.Pow((GN + Math.Pow(H[M], 1f / 3f)), 3) * OML[K + M];
                                TMP3 = Math.Sqrt(F[N] * F[M]) * TMP2 * OMK[K + M];
                                S2 += TMP2;
                                S3 += TMP3;
                            }
                        }
                        HX += X[N] * (2.0 * S2 - TMP2);
                        FX += X[N] * (2.0 * S3 - TMP3);
                    }
                }

                FX /= HX;
                TO = TX / FX;
                PO = PX * HX / FX;
                DOO = RHOF(PO, TO);    //       CALCULATE THE EQUIVALENT METHANE DENSITY.
                //       RECALCULATE SHAPE FACTORS AND CHECK FOR
                //       CONVERGENCE IN FX AND HX

                for (int N = 1; N <= NC; N++)
                {
                    if (!MASK[N])
                    {
                        TR = Math.Min(2.0, F[N] * TO / P[N].Tc);
                        VR = Math.Min(2.0, Math.Max(0.5, H[N] * P[N].Dc / DOO));
                        WN = P[N].Omega;
                        F[N] = P[N].Tc * THETAF(TR, VR, WN) / TCO;
                        H[N] = DCO * PHIF(TR, VR, WN) * ZCO / (P[N].Z * P[N].Dc);
                    }
                }

                // TEST FOR CONVERGENCE

                if (Math.Abs(FX / FXS - 1.0) < TOLERS && Math.Abs(HX / HXS - 1.0) < TOLERS)
                {
                    DX = DOO / HX;                  // CONVERGENCE!!!
                    ECSTX(DX, TX, X);
                    return;
                }
            }
            //300   FORMAT(/" /DCST/ FAILED TO CONVERGE AT T=",F8.3," AND P=",G14.7)
            return;
        }

        public void ECSTX(double DX, double TX, double[] X)
        {
            /*
            C PURPOSE  THIS ROUTINE EVALUATES VARIOUS SUMS WHICH ARE USED
            C TO EVALUATE THE DERIVATIVES OF FX AND HX W.R.T.
            C T. IT ASSUMES THAT ECSTD HAS BEEN CALLED TO EVALUATE
            C DENSITY, FX AND HX.*/

            double MIJ = 0, GIJ = 0, FIJ = 0;
            double[,] S = new double[6, 4];
            double[] SJ = new double[6], Z = new double[6];
            double RBAR = 0.0;
            double DCMAX = 0.0;
            Z[1] = FIJ;
            Z[2] = GIJ;
            Z[3] = MIJ; // equivalnce

            int L, K;
            double GI, HIJ, RMIJ, TERM, TEMP;

            CMX = 0.0;      //  INITIALIZE
            CMXV = 0.0;
            CMXT = 0.0;

            for (int I = 1; I <= NC; I++)
            {
                if (P[I].Dc > DCMAX) DCMAX = P[I].Dc;
            }

            for (int I = 1; I <= NC; I++) // DO 120
            {
                if (X[I] <= 0.0)
                    break;
                K = I * (I - 1) / 2;
                GI = Math.Pow(H[I], 1f / 3f);

                for (int M = 1; M <= 5; M++)
                {
                    SJ[M] = 0.0;
                }
                for (int J = 1; J <= NC; J++)
                {
                    if (X[J] > 0.0)
                    {
                        L = K + J;
                        if (J > I)
                            L = I + J * (J - 1) / 2;
                        GIJ = 0.5 * (GI + Math.Pow(H[J], 1f / 3f));
                        HIJ = Math.Pow(GIJ, 3) * OML[L];
                        FIJ = Math.Sqrt(F[I] * F[J]) * OMK[L];
                        RMIJ = P[J].MW * P[J].MW / (P[J].MW + P[J].MW);
                        Z[4] = 1.0 / RMIJ;
                        MIJ = Math.Pow(HIJ, (1f / 3f)) * Math.Sqrt(RMIJ / FIJ);
                        GIJ /= GI;
                        TERM = X[J] * HIJ / GIJ;
                        Z[1] = FIJ;
                        Z[2] = GIJ;
                        Z[3] = MIJ;
                        for (int M = 1; M <= 5; M++)
                        {
                            SJ[M] = SJ[M] + TERM;
                            TERM *= Z[M];
                        }
                    }
                }
                double THETA = F[I] * TCO / P[I].Tc;
                double PHI = H[I] * P[I].Dc / DCO;
                TR = Math.Min(2.0, F[I] * TX / (FX * P[I].Tc));
                VR = Math.Min(2.0, Math.Max(0.5, H[I] * P[I].Dc / (HX * DX)));
                TEMP = (P[I].Omega - WO) / THETA;
                Z[1] = 1.0 - TEMP * (CT[2] - CT[4] * (VR - 0.5) / TR);
                Z[2] = TEMP * (CT[3] + CT[4] / TR) * VR;
                TEMP = (P[I].Omega - WO) * ZCO / (PHI * P[I].Z);
                Z[3] = 1.0 - TEMP * (CP[1] + CP[3] * Math.Log(TR)) * VR;
                Z[4] = TEMP * CP[3] * (VR + CP[4]);
                if (TR > 2.0)
                {
                    Z[1] = 1.0;
                    Z[4] = 0.0;
                }

                if (!(VR > 0.5 && VR < 2.0))
                {
                    Z[2] = 0.0;
                    Z[3] = 1.0;
                }

                Z[5] = Z[1] * Z[3] - Z[2] * Z[4];
                TERM = HX;
                for (int M = 1; M <= 3; M++)
                {
                    SJ[M] = X[I] * SJ[M] / (Z[5] * TERM);
                    TERM = FX * HX;
                    for (int N = 1; N <= 5; N++)
                        S[N, M] = S[N, M] + Z[N] * SJ[M];
                }
                CMX += X[I] * P[I].MW;
                CMXV += X[I] * SJ[4];
                CMXT += X[I] * SJ[5];
                ZCX += X[I] * P[I].Z;
                RBAR += X[I] * Math.Pow((DCMAX / P[I].Dc), 1f / 3f);
            }

            TERM = 1.0 + S[1, 1] - S[5, 1];
            TEMP = S[3, 3] + S[4, 2];
            double TRM2 = 1.0 + S[2, 3] + S[1, 2] - S[5, 2];
            DFXDT = 1.0 - TERM / (TERM * TEMP - S[4, 1] * TRM2);
            // DHXDT = S[4, 1] * (1.0 - DFXDT) / TERM;
            TERM = FX * Math.Pow(HX, (8.0 / 3.0));
            CMXV = 2.0 * CMXV * CMXV / TERM;
            CMXT = 2.0 * TERM / (CMXT * CMXT);
            DFXDT = FX * DFXDT / TX;
            //DHXDT = HX * DHXDT / TX;
            //RBAR = 1.0 / RBAR;
            //CORV = (0.161290 - 4.516129 * RBAR) / (1.0 - 5.354839 * RBAR);
            return;
        }

        public double RHOF(double P, double T)
        {
            /*C PURPOSE --- THIS ROUTINE CALCULATES THE DENSITY IN MOL/L OF A
            C FLUID FROM AN EQUATION OF STATE GIVEN THE Temperature
            C T IN KELVIN AND THE Pressure   P IN ATMOSPHERES.*/

            double[] AP = new double[] { 0, 6.3240720088E+00, -6.7447661253E+00, -2.5239605832E+00, 0.4188447111E+00 };
            double[] BP = new double[] { 0, 1.5213153011E+01, 9.2665399400E+00, 1.3551718205E+01 };
            double FOP;
            double TAU, TMP1, TMP2, PRR, PR, PS;

            PS = PCO;
            FOP = P / (R * T);
            if (T <= TCO)
            {
                // SUB-CRITICAL Temperature .
                // CALCULATE THE VAPOR Pressure  . IF T > 170 K,
                // USE THE CRITICAL REGION FUNCTION. OTHERWISE
                // THE FROST-KALKWARF TYPE EQUATION.

                TR = T / TCO;
                TAU = 1.0 - TR;
                if (T <= 170.0)
                {
                    TMP1 = AP[1] + AP[2] / TR + AP[3] * Math.Log(TR);
                    TMP2 = AP[4] / (TR * TR);
                    PRR = 0.100;
                    PR = 0;

                    for (int K = 1; K <= 50; K++)
                    {
                        PR = Math.Exp(TMP1 + TMP2 * PRR);
                        if (Math.Abs(PR / PRR - 1.0) < 1.0E-6)
                            break;
                        PRR = PR;
                    }
                }
                else
                {
                    PR = Math.Exp(BP[1] * (1.0 - 1.0 / TR) + BP[2] * TAU + BP[3] * TAU * TAU);
                }

                PS = PR * PCO;
                //           IF P % PS, USE A LIQUID DENSITY. OTHERWISE, USE
                //           IDEAL GAS DENSITY CALCULATED ABOVE.
            }

            if (P > PS)
                FOP = 3.0 * DCO;

            double RHOF1 = DO(P, T, FOP);
            return RHOF1;
        }

        public double DO(double PO, double TO, double FOP)
        {
            bool LIQUID = false;
            /*      double  PRECISION DLO, DHI, DP, PX, PO, TO, FOP, D, D1, DN
            C
            c
            C PURPOSE -- THIS ROUTINE CALCULATES THE DENSITY OF A FLUID AT
            C T AND P GIVEN AN INITIAL GUESS IN FOP. ON EXIT,
            C IT return  S THE FUGACITY COEFFICIENT IN FOP. IT
            C REQUIRES A ROUTINE "PVTO" WHICH CALCULATES P,
            C DPDD, AND F/P, GIVEN T AND D.*/

            // INITIALIZE PARAMETERS

            double TOLERD = 1.0E-8, TOLERP = 1.0E-8;
            double D1 = FOP;
            int NTRY = 0;
            double DMAX = 3.2E0 * DCO;

            // ESTABLISH BOUNDS AND START new TON RAPHSON

            do
            {
                double DLO = 0.0;
                double DHI = DMAX;
                double D = D1;
                double DD, DP, PX = 0, DPDD = 0;

                for (int LAP = 1; LAP <= 20; LAP++)
                {
                    PVTO(ref PX, D, TO, ref DPDD, ref FOP);

                    if (DPDD > 1.0E-2)  // IF DPDD IS ZERO OR NEGATIVE, TRY BISECTION
                    {
                        DP = PO - PX;
                        DD = DP / DPDD;  // SAVE DENSITY FOR POSSIBLE BISECTION

                        if (DP == 0)
                        {
                            return D;
                        }
                        else if (DP < 0)
                        { DHI = D; }
                        else
                        { DLO = D; }

                        double DN = D + DD;
                        // KEEP D WITHIN BOUNDS OR GOTO BISECTION

                        if (DN < 0.0 || DN > DMAX)
                        {
                        }
                        else
                        {
                            D = DN;
                            if (LAP != 1)
                            {
                                if (Math.Abs(DP / PO) < TOLERP || Math.Abs(DD / D) < TOLERD)
                                {
                                    return D;
                                }
                            }
                        }
                    }
                }

                //     new TON-RAPHSON FAILURE. TRY BISECTION
                NTRY++;
                if (NTRY > 2)
                {
                    Debug.Print("FAILED TO CONVERGE AT T0=" + TO + " " + PO);
                    return 0;
                }

                if (TO <= TCO)
                {
                    // SUB-CRITICAL. MAKE SURE THAT WE HAVE THE
                    // PROPER BOUNDS ON THE DENSITY.

                    if (D1 >= DCO)
                    {
                        LIQUID = true;
                        if (DLO <= DCO) DLO = DCO;
                        if (DHI <= DCO) DHI = DMAX;
                    }
                    else
                    {
                        LIQUID = false;
                        if (DLO >= DCO) DLO = 0.0;
                        if (DHI >= DCO) DHI = DCO;
                    }
                }

                // START THE BISECTION
                do
                {
                    D = 0.50E0 * (DLO + DHI);
                    PVTO(ref PX, D, TO, ref DPDD, ref FOP);
                    DP = PX - PO;
                    if (DPDD < 0.0)
                    {
                        if (!LIQUID)
                            DPDD = -DPDD;
                        if (DPDD < 0)
                        { DLO = D; }
                        if (DPDD > 0)
                        { DHI = D; }
                    }
                    else
                    {
                        if (DP < 0)
                        {
                            DLO = D;
                        }
                        else if (DP > 0)
                        {
                            DHI = D;
                        }
                        else
                        {
                            return D;
                        }
                    }

                    if (Math.Abs(DPDD) <= TOLERP && Math.Abs(DP) > TOLERP)// BISECTION FAILED. WE ARE PROBABLY TRYING ON
                    {                                                     // THE WRONG SIDE OF THE DOME.
                        D1 = 3.0 * DCO;
                        if (LIQUID)
                            D1 = 0.01;
                        break;
                    }
                    else if (Math.Abs(DP / PO) < TOLERP)
                    {
                        return D;
                    }
                } while (Math.Abs(DLO / DHI - 1.0) > TOLERP);
            } while (NTRY < 3);
            return 0;
        }

        public void PVTO(ref double PO, double DO, double TO, ref double DPDDO, ref double FOPO)
        {
            /***********************************
            C PURPOSE  THIS ROUTINE CALCULATES THE EQUIVALENT Pressure   AND
            C DERIVATIVE OF Pressure   WRT DENSITY OF METHANE. THE
            C EQUATION USED IS THAT OF STEWART AND JACOBSEN. THE
            C DENSITY IS IN G-MOL/LIT AND Temperature  IN KELVIN. IT
            C YIELDS THE Pressure   IN ATMOSPHERES AND RATIO OF THE
            C FUGACITY TO THE Pressure  .*/

            double[] G = new double[16], B = new double[10], F, S = new double[7];
            double TERM, ZO, FOP;

            B[0] = 9.0;
            F = new double[] { 0, 1.0, 1.0, 2.0, 6.0, 24.0, 120.0 };
            double T, D, T1, T2, TS, P1, D1, F1, P2, D2, F2, DSQ;
            T = TO;
            D = DO;
            TS = Math.Sqrt(TO);
            T1 = 1.0 / T;
            T2 = T1 * T1;
            G[1] = R * T;
            G[2] = A[1] * T + A[2] * TS + A[3] + A[4] * T1 + A[5] * T2;
            G[3] = A[6] * T + A[7] + A[8] * T1 + A[9] * T2;
            G[4] = A[10] * T + A[11] + A[12] * T1;
            G[5] = A[13];
            G[6] = A[14] * T1 + A[15] * T2;
            G[7] = A[16] * T1;
            G[8] = A[17] * T1 + A[18] * T2;
            G[9] = A[19] * T2;
            G[10] = T2 * (A[20] + A[21] * T1);
            G[11] = T2 * (A[22] + A[23] * T2);
            G[12] = T2 * (A[24] + A[25] * T1);
            G[13] = T2 * (A[26] + A[27] * T2);
            G[14] = T2 * (A[28] + A[29] * T1);
            G[15] = T2 * (A[30] + T1 * (A[31] + T1 * A[32]));

            for (int J = 2; J <= 9; J++)
            {
                B[J] = G[J] / (G[1] * (J - 1));
            }

            for (int K = 1; K <= 6; K++)
            {
                S[K] = 0.0;
                TERM = 1.0;
                for (int J = K; J <= 6; J++)
                {
                    S[K] = S[K] + F[J] * G[J + 9] * TERM;
                    TERM /= GAMMA;
                }
                S[K] = S[K] / (G[1] * F[K]);
            }

            P1 = 0.0;
            D1 = 0.0;
            F1 = 0.0;
            for (int J = 1; J <= 9; J++)
            {
                P1 = P1 * D + G[10 - J];
                D1 = D1 * D + P1;
                F1 = F1 * D + B[10 - J];
            }

            DSQ = D * D;
            P2 = 0.0;
            D2 = 0.0;
            F2 = 0.0;
            for (int J = 10; J <= 15; J++)
            {
                P2 = (P2 + G[25 - J]) * DSQ;
                D2 = D2 * DSQ + P2;
                F2 = F2 * DSQ + S[16 - J];
            }
            TERM = Math.Exp(-GAMMA * DSQ);
            PO = D * (P1 + P2 * TERM);

            if (PO == 0.0)
                PO = 1.0E-20;

            ZO = PO / (D * G[1]);
            DPDDO = (D1 + TERM * (P2 + 2.0 * (D2 - DSQ * GAMMA * P2)));
            FOP = F1 - (F2 * TERM - S[1]) / (2.0 * GAMMA) + ZO - 1.0 - Math.Log(ZO / PO);
            FOPO = Math.Exp(FOP) / PO;
            return;
        }

        public double VSCTY(double DX, double TX)
        {
            /*   ***********************************
            C   PURPOSE - - THIS ROUTINE CALCULATES THE VISCOSITY OF METHANE
            C   using   HANLEY'S CORRELATION */

            double FETA, ETAX, DO, ETA1 = 0, ETAO = 0, R1, R2;
            double CORV = 1, TO, ZCO = 0.28374;
            double TRMO = 0, TRM1 = 0;
            double TRM2 = 0;
            double TRMX = 0;

            double[] CO = new double[10]{0,0.2907741307E+07,-0.3312814033E+07, 0.1608101838E+07,
              -0.4331904871E+06, 0.7062481330E+05,-0.7116620750E+04,
               0.4325174400E+03,-0.1445911210E+02, 0.2037119479E+00};

            double[] CD = new double[5] { 0, 1.6969859271E+00, -1.3337234608E-01, 1.4000000000E+00, 1.6800000000E+02 };

            double[] CE = new double[9]{0,-1.0239160427E+01, 1.7422822961E+02, 1.7460545674E+01,
              -2.8475328289E+03, 1.3368502192E-01, 1.4207239767E+02,
               5.0020669720E+03, 1.6280000000E-01};

            double TI, T1, VSCTY;

            double TLAST = -1, PSI = 1.5;

            DO = 0.001 * DX * HX * CMWO;       // CALCULATE THE EQUIVALENT DENSITY
            TO = TX / FX;

            //       CALCULATE THE Temperature  DEPENDENT PART OF THE
            //       VISCOSITY CORRELATION IF T ISN"T THE SAME AS
            //       THE LAST CALL TO THIS ROUTINE (USUAL case )

            if (TO != TLAST)
            {
                TI = 1.0 / TO;
                ETA1 = CD[1] + CD[2] * Math.Pow((CD[3] - Math.Log(TO / CD[4])), 2);
                TRMO = CE[1] + CE[2] * TI;
                TRM1 = CE[3] + CE[4] * TI / Math.Sqrt(TO);
                TRM2 = CE[5] + TI * (CE[6] + CE[7] * TI);
                TRMX = Math.Exp(TRMO);
                T1 = Math.Pow(TO, 1f / 3f);
                ETAO = 0.0;
                for (int J = 1; J <= 9; J++)
                {
                    ETAO += CO[J] * TI;
                    TI *= T1;
                }
            }

            R1 = Math.Pow(DO, 0.10);                        // DENSITY DEPENDENCE IS CALCULATED HERE.
            R2 = Math.Sqrt(DO) * (DO / CE[8] - 1.0);
            ETAX = Math.Exp(TRMO + TRM1 * R1 + TRM2 * R2) - TRMX;
            FETA = Math.Sqrt(FX * CMXV / CMWO) / Math.Pow((HX * HX), 1f / 3f);
            CORV = Math.Sqrt((1.0 - TO * PSI * Math.Min(0.0, DFXDT)) * ZCX / ZCO) * CORV;
            VSCTY = (ETAO + ETA1 * DO + ETAX * CORV) * FETA;
            return VSCTY;
        }

        public double TCOND(double DX, double TX)
        {
            /************************************
            PURPOSE - - THIS ROUTINE CALCULATES THE TRANSLATIONAL PORTION
            OF THE METHANE THERMAL CONDUCTIVITY using   HANLEY"S
            CORRELATIONS
            ************************************/

            double TRMX, TI, T1, FLAM, R1, R2;
            double CON = 0.19434504, PSI = 1.00;
            double DO, TO, TCOND;

            //           CALCULATE THE Temperature  DEPENDENT PART OF THE
            //           THERMAL CONDUCTIVITY CORRELATION IF T ISN"T THE
            //           SAME AS THE LAST CALL TO THIS ROUTINE (USUAL
            //           case )

            DO = 0.001 * DX * HX * CMWO;   // CALCULATE THE EQUIVALENT DENSITY
            TO = TX / FX;
            TI = 1.0 / TO;
            double LAM1 = CDT[1] + CDT[2] * Math.Pow(CDT[3] - Math.Log(TO / CDT[4]), 2);
            double TRMO = CET[1] + CET[2] * TI;
            double TRM1 = CET[3] + CET[4] * TI / Math.Sqrt(TO);
            double TRM2 = CET[5] + TI * (CET[6] + CET[7] * TI);
            TRMX = Math.Exp(TRMO);
            T1 = Math.Pow(TO, 1f / 3f);

            //           CALCULATE THE IDEAL GAS TRANSLATIONAL THERMAL
            //           CONDUCTIVITY FROM THE EUCKEN FORMULA
            //           K = 15 * R * ETAO / (4 * M)
            double LAMO = 0.0;
            for (int J = 1; J <= 9; J++)
            {
                LAMO += CO[J] * TI;
                TI *= T1;
            }
            LAMO = CON * LAMO;

            // DENSITY DEPENDENCE IS CALCULATED HERE.
            R1 = Math.Pow(DO, 0.10);
            R2 = Math.Sqrt(DO) * (DO / CET[8] - 1.0);
            double LAMX = Math.Exp(TRMO + TRM1 * R1 + TRM2 * R2) - TRMX;
            FLAM = Math.Sqrt(FX * CMWO / CMXT) / Math.Pow(HX * HX, (1f / 3f));
            CORT = Math.Pow(((1.0 - TO * Math.Min(0.0, PSI * DFXDT)) * ZCO / ZCX), 1.5);
            TCOND = (LAMO + LAM1 * DO + LAMX) * FLAM * CORT;

            return TCOND;
        }

        public double THERMC(double DX, double TX, double[] X)
        {
            /* PURPOSE - THIS ROUTINE CALCULATES THE THERMAL CONDUCTIVITY OF
            A MIXTURE FROM THE EXTENDED CORRESPONDING STATES MODEL */

            double CPO, TRM, TIJ = 0;
            double[] TCint = new double[20];
            double Fint, TCTMIX, FXS, HXS, CMS, TCIMIX, THERMC;

            Fint = 1.32 * 4.184 / 10.0;
            TCTMIX = TCOND(DX, TX);  //CALCULATE THE TRANSLATIONAL THERMAL CONDUCTIVITY OF THE MIXTURE

            FXS = FX;
            HXS = HX;
            CMS = CMXV;
            TCIMIX = 0.0;

            // CALCULATE THE public  CONTRIBUTION
            for (int J = 1; J <= NC; J++)
            {
                FX = F[J];
                HX = H[J];
                CMXV = P[J].MW;
                CPO = P[J].Ideal_Heat[1];
                TRM = TX;
                for (int K = 2; K <= 7; K++)
                {
                    CPO += TRM * P[J].Ideal_Heat[K];
                    TRM *= TX;
                }
                TCint[J] = Fint * (CPO - 4.968) * VSCTY(0.0, TX) / P[J].MW;
            }

            for (int I = 1; I <= NC; I++)
            {
                TRM = 0.0;
                for (int J = 1; J <= I; J++)
                {
                    TIJ = X[J] * TCint[J] / (TCint[I] + TCint[J]);
                    TRM += TIJ;
                }
                TCIMIX += X[I] * TCint[I] * (2.0 * TRM - TIJ);
            }
            FX = FXS;
            HX = HXS;
            CMXV = CMS;
            THERMC = TCTMIX + 2.0 * TCIMIX;
            return THERMC;
        }

        public void LIB(int N)
        {
            /*       PURPOSE  THIS ROUTINE TAKES THE COMPONENT NUMBER "N"
            C       WHOSE NAME RESIDES IN NOM(N,l-7) AND COMPARES
            C       IT WITH NAMES IN THE DATABASE CONTAINED IN /LIBDAT/
            C       IF A MATCH IS OBTAINED, THE APPROPRIATE LOCATIONS
            C       OF THE COMMON BLOCK /COMPRP/ ARE LOADED WITH THE
            C       DATA FROM THE CORRESPONDING COMPONENT IN /LIBDAT/*/

            int L, K = 0, I;

            for (L = 1; L <= NLIB; L++)
            {
                if (NAME[N] == PROPS[L].Name)
                {
                    K = L;
                    break;
                }

                // if (NAME[N] != PROPS[L].Name)
                //    return  ;
            }

            if (NAME[N] != PROPS[K].Name)
                return;
            //           COMPONENT COULDN'T BE IDENTIFIED.
            //           COPY DATA FROM LIBRARY TO WORKING
            //           COMMON AREA AND CALCULATE ZC

            P[N].Pc = PROPS[K].Pc;
            P[N].Dc = 1000.0 / PROPS[K].Vc;
            P[N].Tc = PROPS[K].Tc;
            P[N].MW = PROPS[K].MW;
            P[N].Vc = PROPS[K].Vc;
            P[N].Z = PROPS[K].Pc / (0.08205616 * P[N].Dc * PROPS[K].Tc);
            P[N].Omega = PROPS[K].Omega;
            P[N].Ideal_Heat = PROPS[K].Ideal_Heat;

            //   DUMMY IN THE int ERACTION CONSTANTS

            for (L = 1; L <= N; L++)
            {
                I = L * (L - 1) / 2;
                for (int J = 1; J <= L; J++)
                {
                    K = I + J;
                    OMK[K] = 1.0;
                    OML[K] = 1.0;
                }
            }
        }
    }
}