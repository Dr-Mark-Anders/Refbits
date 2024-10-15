using System;
using System.Collections.Generic;
using Units.UOM;

namespace ModelEngine.ThermodynamicMethods.UNIFAC
{
    public partial class UNIFAC
    {
        private Components cc;
        private double[,] Xi, Theta, ThetaPsi, Theta_ThetaPsi, SumThetaPSi_ThetaPsi, Gamma;
        private double[] R, Q;
        private double[] N, SumVX;
        private double[][] CompGroupNo;
        private double[] Sknk;
        private double[,] tauL;
        private double[] ThetaI, PhiI, LngC, LNgRo, LngR;

        public static PrimaryDataCollection UNIFAC_GROUPS_SHORT = new();
        public static double[,] UNIFAC_GROUP_intERACT_PARAMS_SHORT;

        private int[][] primaryGroupCount;
        private double[,] interactionParametersShort;
        public double[] Activity;

        //
        public UNIFAC()
        {
        }

        public double[] Solve(BaseComp A, BaseComp B, Temperature T, BaseUnifacData UData)
        {
            Components cc = new Components();
            cc.Add(A);
            cc.Add(B);
            return SolveActivity(cc, cc.MoleFractions, T, UData);
        }

        public double[] SolveActivity(Components cc, double[] X, Temperature T, BaseUnifacData UData)
        {
            double[] act = SolveLNActivity(cc, X, T, UData);

            for (int i = 0; i < act.Length; i++)
            {
                act[i] = Math.Exp(act[i]);
            }

            return act;
        }

        public double[] SolveLNActivity(Components cc, double[] X, Temperature T, BaseUnifacData UData)
        {
            this.cc = cc.Clone();
            this.cc.SetMolFractions(X);

            int NoComps = cc.Count;

            CompGroupNo = new double[cc.Count][];
            R = new double[cc.Count];
            Q = new double[cc.Count];
            N = new double[cc.Count];

            IdentifyActiveGroups(cc, UData.GroupList, UData.GROUP_INTERACT_PARAMS, out int NoShortGroups);
            UNIFAC_GROUPS_SHORT.SortByIndex();

            //Xi = new  double [cc.Count+1, NoShortGroups + 1];
            Theta = new double[cc.Count + 1, NoShortGroups];
            ThetaPsi = new double[cc.Count + 1, NoShortGroups + 1];

            SumThetaPSi_ThetaPsi = new double[cc.Count, NoShortGroups + 1];

            primaryGroupCount = new int[NoComps][];

            LngC = new double[cc.Count];
            LngR = new double[cc.Count];
            LNgRo = new double[cc.Count];

            ThetaI = new double[cc.Count + 1];
            PhiI = new double[cc.Count + 1];

            SumVX = new double[NoShortGroups + 1];

            for (int i = 0; i < cc.Count; i++)
            {
                primaryGroupCount[i] = new int[NoShortGroups];
                GetGroupData(cc[i], i, out double Rt, out double Qt, out int Nt, out List<int> NValues);
                R[i] = Rt;
                Q[i] = Qt;
                cc[i].UnifacR = Rt;
                cc[i].UnifacQ = Qt;
                N[i] = Nt;
            }

            interactionParametersShort = new double[NoShortGroups, NoShortGroups];

            for (int i = 0; i < NoShortGroups; i++)
            {
                for (int y = 0; y < NoShortGroups; y++)
                {
                    interactionParametersShort[i, y] = UData.GROUP_INTERACT_PARAMS[UNIFAC_GROUPS_SHORT[y].SEC_LOCATION - 1, UNIFAC_GROUPS_SHORT[i].SEC_LOCATION - 1];
                }
            }

            CalcTauL(T, NoShortGroups);
            CalcSknK();
            CalcMix(NoShortGroups);
            CalcXi(NoShortGroups);
            CalcTheta(NoShortGroups);
            CalcThetaPsi();
            CaclTheta_ThetaPsi(NoShortGroups);
            CalcSumThetaPSi_ThetaPsi();
            CalcGamma(NoShortGroups);
            CalcThetaI();
            CalcPhiI();
            lnGammaC();
            lnGammaRho(NoShortGroups);
            lnGammaR(NoShortGroups);
            CalcActivity();

            return Activity;
        }
    }
}