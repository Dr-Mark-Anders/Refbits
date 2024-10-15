using Extensions;
using Math2;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Units.UOM;
using static ModelEngine.Components;

namespace Units
{
    ///<summary>
    ///SummarydescriptionforSimpleColumn.
    ///</summary>
    public class CrudeCutter
    {
        private List<Port_Material> StreamArr = new List<Port_Material>();
        private Guid guid;

        public CrudeCutter()
        {
        }

        public List<double> CutStream(Components CC, Temperature ibp, Temperature fbp, enumMassVolMol basis)//forassaypropsetc
        {
            if (CC is null)
                return null;

            List<double> res = new List<double>();
            List<double> lv = new List<double>();
            List<double> CumLV = new List<double>();

            BoilingPoints BP = GetBPCollection(CC);
            lv.AddRange(CC.VolFractions);
            CumLV = lv.CumulateArray();

            List<Temperature> AvgBP = new List<Temperature>();
            AvgBP.AddRange(BP.GetAverageBoilingPoints());

            double TotLV;
            double FirstBPAfterIBP, LastBPBeforeUBP;
            double LowerValue, UpperValue;
            double CurrentBP, PreviousBP;

            for (int i = 0; i < CC.ComponentList.Count; i++)//routineformidboilingComponents
            {
                FirstBPAfterIBP = Components.GetFirstBPAfterIBP(CC, ibp);
                LastBPBeforeUBP = Components.GetLastBPBeforeFBP(CC, fbp);

                CurrentBP = CC[i].MidBP;

                if (i == 0)
                    PreviousBP = 36;
                else
                    PreviousBP = CC[i - 1].MidBP;

                LowerValue = CubicSpline.CubSpline(eSplineMethod.Constrained, FirstBPAfterIBP, AvgBP, CumLV) - CubicSpline.CubSpline(eSplineMethod.Constrained, ibp, AvgBP, CumLV);
                UpperValue = CubicSpline.CubSpline(eSplineMethod.Constrained, fbp, AvgBP, CumLV) - CubicSpline.CubSpline(eSplineMethod.Constrained, LastBPBeforeUBP, AvgBP, CumLV);

                if (PreviousBP >= ibp && CurrentBP < fbp)//copywholelv%
                    res.Add(CC[i].STDLiqVolFraction);
                else if (PreviousBP < ibp && CurrentBP > ibp)//boundslowerboilingrange
                    res.Add(LowerValue);
                else if (PreviousBP < fbp && CurrentBP >= fbp)//boundsupperboilingrange
                    res.Add(UpperValue);
                else
                    res.Add(0.0);
            }

            TotLV = res.Sum();

            for (int i = 0; i < CC.ComponentList.Count; i++)
                res[i] /= TotLV;

            switch (basis)
            {
                case enumMassVolMol.Vol_PCT:
                    break;

                case enumMassVolMol.Wt_PCT:
                    double sum = 0;
                    for (int i = 0; i < CC.ComponentList.Count; i++)
                        res[i] *= CC[i].Density;

                    sum = res.Sum();
                    for (int i = 0; i < CC.ComponentList.Count; i++)
                        res[i] /= sum;

                    break;

                case enumMassVolMol.Mol_PCT:
                    for (int i = 0; i < CC.ComponentList.Count; i++)
                        res[i] *= (CC[i].Density / CC[i].MW);

                    sum = res.Sum();
                    for (int i = 0; i < CC.ComponentList.Count; i++)
                        res[i] /= sum;

                    break;
            }

            return res;
        }

        public List<Port_Material> CutStreams(Port_Material St, List<Tuple<Temperature, Temperature>> ICP, Guid guid)
        {
            this.guid = guid;

            if (St.cc is null || St.cc.Count == 0)
                return null;

            StreamArr.Clear();

            for (int s = 0; s < ICP.Count; s++)//GetVF's//Lenght+1=FBP
            {
                StreamArr.Add(CutStream(St, ICP[s].Item1, ICP[s].Item2, guid));
            }

            for (int ic = 0; ic < ICP.Count; ic++)
            {
                StreamArr[ic].P_ = St.P_;
                StreamArr[ic].T_ = St.T_;
            }
            return StreamArr;
        }

        public List<Port_Material> CutStreams(Port_Material St, Temperature[] ICP, Guid guid)
        {
            this.guid = guid;

            if (St.cc is null || St.cc.Count == 0)
                return null;

            StreamArr.Clear();

            for (int s = 1; s < ICP.Length; s++)//GetVF's//Lenght+1=FBP
            {
                StreamArr.Add(CutStream(St, ICP[s - 1], ICP[s], guid));
            }

            for (int ic = 0; ic < ICP.Length - 1; ic++)
            {
                StreamArr[ic].P_ = St.P_;
                StreamArr[ic].T_ = St.T_;
            }
            return StreamArr;
        }

        public Port_Material CutStream(Port_Material St, Temperature ibp, Temperature fbp, Guid guid)//foregcrudecutter
        {
            Port_Material newstream = new Port_Material();
            Components newcomps = St.cc.Clone();
            Temperature LBP, UBP;

            if (newcomps is null)
                return null;

            newcomps.TakeOutWaterComp();

            for (int i = 0; i < newcomps.ComponentList.Count; i++)
            {
                LBP = newcomps.LBP(i);
                UBP = newcomps.UBP(i);

                if (LBP >= fbp || UBP <= ibp)
                    newcomps[i].SetZero();
                else if (UBP > fbp && LBP < fbp)
                    newcomps[i].STDLiqVolFraction *= (fbp - LBP) / (UBP - LBP);
                else if (UBP > ibp && LBP < ibp)
                    newcomps[i].STDLiqVolFraction *= (UBP - ibp) / (UBP - LBP);

                //if(LBP>fbp)
                //break;
            }

            double VF = newcomps.VolFractions.Sum() * St.VF_;
            newcomps.NormaliseFractions(FlowFlag.LiqVol);
            newstream.cc.Add(newcomps);
            newstream.VF_.ForceSetValue(VF, SourceEnum.Input);
            newstream.cc.NormaliseFractions(FlowFlag.LiqVol);
            newstream.UpdateFlows();

            return newstream;
        }

        ///<summary>
        ///LVPCiseitherweight,volumeormolar
        ///</summary>
        ///<paramname="PCt"></param>
        ///<paramname="Property"></param>
        ///<paramname="BlendMethod"></param>
        ///<return  s></return  s>
        public double StreamPropValue(List<double> PCt, List<double> Property, enumMassVolMol BlendMethod)
        {
            double res = 0;

            for (int i = 0; i < PCt.Count; i++)
            {
                switch (BlendMethod)
                {
                    case enumMassVolMol.Vol_PCT:
                        res += PCt[i] * Property[i];
                        break;

                    case enumMassVolMol.Wt_PCT:
                        res += PCt[i] * Property[i];
                        break;

                    case enumMassVolMol.Mol_PCT:
                        res += PCt[i] * Property[i];
                        break;
                }
            }
            return res;
        }

        public static Components Cut(Components cc, Temperature IBP, Temperature FBP, out double cutpct)
        {
            cc.SortByBP();
            cc.NormaliseFractions(FlowFlag.Molar);

            cc.ZeroMoleFractions();

            List<double> lv = new List<double>();
            List<double> CumLV = new List<double>();

            //BoilingPoints BP = GetBPCollection();
            lv.AddRange(cc.MoleFractions);
            CumLV = lv.CumulateArray();
            CumLV.Insert(0, 0);

            List<Temperature> AvgBP = new List<Temperature>();
            AvgBP.AddRange(cc.BPArray);
            AvgBP.Insert(0, 0);

            Temperature FirstBPAfterIBP, LastBPBeforeFBP;
            Temperature CurrentBP, PreviousBP;

            FirstBPAfterIBP = GetFirstBPAfterIBP(cc, IBP);
            LastBPBeforeFBP = GetLastBPBeforeFBP(cc, FBP);
            double fraction = 0;

            for (int i = 0; i < cc.Count; i++)         //routine form id boiling Components
            {
                BaseComp bc = cc[i];
                CurrentBP = cc[i].MidBP;

                if (i == 0)
                    PreviousBP = AvgBP[i];
                else
                    PreviousBP = AvgBP[i];

                if (CurrentBP < FBP && PreviousBP >= IBP)         //copy whole lv%
                    cc[i].MoleFraction = bc.MoleFraction;
                else if (CurrentBP > IBP && PreviousBP < IBP)     //bounds lower boiling range
                {
                    fraction = CubicSpline.CubSpline(eSplineMethod.Constrained, CurrentBP, AvgBP, CumLV) - CubicSpline.CubSpline(eSplineMethod.Constrained, IBP, AvgBP, CumLV);
                    cc[i].MoleFraction = fraction;
                }
                else if (CurrentBP >= FBP && PreviousBP < FBP)    //bounds upper boiling range
                {
                    fraction = CubicSpline.CubSpline(eSplineMethod.Constrained, FBP, AvgBP, CumLV) - CubicSpline.CubSpline(eSplineMethod.Constrained, PreviousBP, AvgBP, CumLV);
                    cc[i].MoleFraction = fraction;
                }
                else
                    cc[i].MoleFraction = 0.0;
            }

            cutpct = cc.MoleFractions.Sum();

            cc.NormaliseFractions();

            return cc;
        }

    }
}