using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Units.UOM;

namespace ModelEngine.ThermodynamicMethods.UNIFAC
{
    public partial class UNIFAC
    {
        private void CalcActivity()
        {
            Activity = new double[cc.Count];

            for (int i = 0; i < cc.Count; i++)
            {
                Activity[i] = Math.Exp(LngR[i] - LNgRo[i] + LngC[i]);
            };
        }

        private void CalcLNActivity()
        {
            Activity = new double[cc.Count];

            for (int i = 0; i < cc.Count; i++)
            {
                Activity[i] = LngR[i] - LNgRo[i] + LngC[i];
            };
        }

        private void CalcTauL(Temperature T, int NoShortGroups)
        {
            tauL = new double[NoShortGroups, NoShortGroups];

            for (int x = 0; x < NoShortGroups; x++)
                for (int y = 0; y < NoShortGroups; y++)
                    tauL[x, y] = Math.Exp(-interactionParametersShort[x, y] / T.BaseValue);
        }

        /// <summary>
        /// Sum IParams * Xi
        /// </summary>
        private void CalcSknK()
        {
            Sknk = new double[cc.Count];
            for (int comp = 0; comp < cc.Count; comp++)
                Sknk[comp] += primaryGroupCount[comp].Sum() * cc[comp].MoleFraction;
        }

        private void CalcMix(int NoShortGroups)
        {
            for (int group = 0; group < NoShortGroups; group++)
                for (int comp = 0; comp < cc.Count; comp++)
                    SumVX[group] += primaryGroupCount[comp][group] * cc.MoleFractions[comp];

            SumVX[NoShortGroups] = Sknk.Sum();
        }

        private void CalcGamma(int NoShortGroups)
        {
            Gamma = new double[cc.Count + 1, NoShortGroups];

            for (int comp = 0; comp < cc.Count + 1; comp++)
                for (int group = 0; group < NoShortGroups; group++)
                    Gamma[comp, group] = UNIFAC_GROUPS_SHORT[group].Q * (1 - Math.Log(ThetaPsi[comp, group]) - SumThetaPSi_ThetaPsi[comp, group]);
        }

        private void CalcSumThetaPSi_ThetaPsi()
        {
            SumThetaPSi_ThetaPsi = tauL.Mult(Theta_ThetaPsi);
        }

        private void CaclTheta_ThetaPsi(int NoShortGroups)
        {
            Theta_ThetaPsi = new double[cc.Count + 1, NoShortGroups + 1];

            for (int comp = 0; comp < cc.Count + 1; comp++)
                for (int group = 0; group < NoShortGroups; group++)
                    Theta_ThetaPsi[comp, group] = Theta[comp, group] / ThetaPsi[comp, group];
        }

        private void CalcThetaPsi()
        {
            double[,] TauLTranspose;
            TauLTranspose = tauL.Transpose();
            ThetaPsi = TauLTranspose.Mult(Theta);
        }

        private void CalcTheta(int NoShortGroups)
        {
            for (int comp = 0; comp < cc.Count + 1; comp++)
                for (int group = 0; group < NoShortGroups; group++)
                    Theta[comp, group] = Xi[comp, group] * UNIFAC_GROUPS_SHORT[group].Q / Xi[comp, NoShortGroups];
        }

        private void CalcXi(int NoShortGroups)
        {
            Xi = new double[cc.Count + 1, NoShortGroups + 1];

            for (int comp = 0; comp < cc.Count; comp++)
                for (int group = 0; group < NoShortGroups; group++)
                    Xi[comp, group] = primaryGroupCount[comp][group] / N[comp];

            for (int group = 0; group < NoShortGroups; group++)
                Xi[cc.Count, group] = SumVX[group] / SumVX[NoShortGroups];

            for (int comp = 0; comp < cc.Count + 1; comp++)
                for (int group = 0; group < NoShortGroups; group++)
                    Xi[comp, NoShortGroups] += Xi[comp, group] * UNIFAC_GROUPS_SHORT[group].Q;
        }

        public void CalcThetaI()
        {
            for (int i = 0; i < cc.Count; i++)
                ThetaI[i] = (cc.MoleFractions[i] * Q[i]) / (cc.MoleFractions.SumProduct(Q));
        }

        public void CalcPhiI()
        {
            for (int i = 0; i < cc.Count; i++)
                PhiI[i] = (cc.MoleFractions[i] * R[i]) / (cc.MoleFractions.SumProduct(R));
        }

        public void lnGammaC()
        {
            for (int i = 0; i < cc.Count; i++)
            {
                LngC[i] = Math.Log(PhiI[i] / cc[i].MoleFraction) + (1 - PhiI[i] / cc[i].MoleFraction)
                    - 5 * Q[i] * (Math.Log(PhiI[i] / ThetaI[i]) + (1 - PhiI[i] / ThetaI[i]));

                if (double.IsNaN(LngC[i]))
                    LngC[i] = 0;
            }
        }

        public void lnGammaRho(int NoShortGroups)
        {
            for (int comp = 0; comp < cc.Count; comp++)
                for (int group = 0; group < NoShortGroups; group++)
                    LNgRo[comp] += Gamma[comp, group] * primaryGroupCount[comp][group];
        }

        public void lnGammaR(int NoShortGroups)
        {
            for (int comp = 0; comp < cc.Count; comp++)
                for (int group = 0; group < NoShortGroups; group++)
                    LngR[comp] += Gamma[cc.Count, group] * primaryGroupCount[comp][group];
        }

        private void IdentifyActiveGroups(Components cc, PrimaryDataCollection GroupList, double[,] GROUP_intERACT_PARAMS, out int noShortGroups)
        {
            List<int> PrimaryGroupLocationList = new List<int>();

            foreach (BaseComp bc in cc)
            {
                string unifac = bc.Unifac;
                string[] tokens = unifac.Split(' ');
                List<Tuple<double, double, int>> CompValues = new();

                for (int i = 0; i < tokens.Length; i++)
                {
                    string str = tokens[i];
                    if (str != "")
                    {
                        int No = 1;
                        if (str[0] == '(')
                        {
                            No = (int)char.GetNumericValue(str.Last());
                            str = str.Substring(1, str.Length - 3);
                        }

                        if (GroupList.Contains(str))
                        {
                            PRIMARY_DATA Group = GroupList[str];
                            int Grouploc = GroupList.IndexOf(str);
                            if (!PrimaryGroupLocationList.Contains(Group.PRIM_LOCATION))
                                PrimaryGroupLocationList.Add(Group.PRIM_LOCATION);

                            PRIMARY_DATA res = GroupList[str];

                            if (!UNIFAC_GROUPS_SHORT.Contains(str))
                                UNIFAC_GROUPS_SHORT.Add(res);
                        }
                    }
                }
            }

            noShortGroups = UNIFAC_GROUPS_SHORT.Count;
            UNIFAC_GROUP_intERACT_PARAMS_SHORT = new double[noShortGroups, noShortGroups];

            PrimaryGroupLocationList.Sort();

            for (int i = 0; i < PrimaryGroupLocationList.Count; i++)
            {
                for (int ii = 0; ii < PrimaryGroupLocationList.Count; ii++)
                {
                    UNIFAC_GROUP_intERACT_PARAMS_SHORT[i, ii] =
                        GROUP_intERACT_PARAMS[PrimaryGroupLocationList[i], PrimaryGroupLocationList[ii]];
                }
            }
        }

        public void GetGroupData(BaseComp comp, int index, out double Rtot, out double Qtot, out int TotCompGroupCount, out List<int> NValues)
        {
            string unifac = comp.Unifac;
            string[] tokens = unifac.Split(' ');
            PrimaryDataCollection CompValues = new();
            NValues = new List<int>();
            Rtot = 0; Qtot = 0;

            for (int i = 0; i < tokens.Length; i++)
            {
                string str = tokens[i];
                if (str != "")
                {
                    int No = 1;
                    if (str[0] == '(')
                    {
                        No = (int)char.GetNumericValue(str.Last());
                        str = str.Substring(1, str.Length - 3);
                    }

                    if (UNIFAC_GROUPS_SHORT.Contains(str))
                    {
                        int loc = UNIFAC_GROUPS_SHORT.IndexOf(str);
                        PRIMARY_DATA res = UNIFAC_GROUPS_SHORT[str];

                        if (!UNIFAC_GROUPS_SHORT.Contains(str))
                            UNIFAC_GROUPS_SHORT.Add(res);

                        for (int ii = 0; ii < No; ii++)
                        {
                            CompValues.Add(UNIFAC_GROUPS_SHORT[str]);
                            if (!NValues.Contains(res.PRIM_LOCATION))
                                NValues.Add(res.PRIM_LOCATION);
                        }
                        primaryGroupCount[index][loc] = No;
                    }
                    else
                    {
                        VLEUnifacData vledata = new();
                        PRIMARY_DATA newpd = vledata.GroupList[str];
                        if (newpd != null)
                        {
                            CompValues.Add(newpd);
                            if (!NValues.Contains(newpd.PRIM_LOCATION))
                                NValues.Add(newpd.PRIM_LOCATION);
                        }
                        else
                        {
                            CompValues.Add(new PRIMARY_DATA(str, 1, 1, 999, 999, 999)); // does not exist
                        }
                    }
                }
            }

            TotCompGroupCount = CompValues.Count;

            foreach (PRIMARY_DATA val in CompValues)
            {
                Rtot += val.R;
                Qtot += val.Q;
            }

            return;
        }
    }
}