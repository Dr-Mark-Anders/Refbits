using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Units.UOM;

namespace ModelEngine
{
    public partial class Components
    {
        public double NormaliseTempMolFlows()
        {
            double sum = 0;
            for (int i = 0; i < CompList.Count; i++)
                sum += CompList[i].TempMolarFlow;

            for (int i = 0; i < CompList.Count; i++)
                CompList[i].MoleFraction = CompList[i].TempMolarFlow / sum;

            return sum;
        }

        public bool NormaliseFractions(FlowFlag flowtype=FlowFlag.Molar)
        {
            int count = CompList.Count;
            double[] massflow = new double[count];
            double[] molarflow = new double[count];
            double[] volumeflow = new double[count];
            double relativemols = 0, relativevols = 0, relativemassflow = 0;
            BaseComp p;
            bool HasAtLeastOneValue = false;

            switch (flowtype)
            {
                case FlowFlag.Molar:
                    for (int i = 0; i < count; i++)
                    {
                        if (!double.IsNaN(CompList[i].MoleFraction))
                        {
                            HasAtLeastOneValue = true;
                            break;
                        }
                    }

                    if(MoleFractions.Sum() == 0)
                        for (int i = 0; i < count; i++)
                        {
                            CompList[i].MoleFraction = 1 / count;
                        }

                    break;

                case FlowFlag.Mass:
                    for (int i = 0; i < count; i++)
                    {
                        if (!double.IsNaN(CompList[i].MassFraction))
                        {
                            HasAtLeastOneValue = true;
                            break;
                        }
                    }
                    break;

                case FlowFlag.LiqVol:
                    for (int i = 0; i < count; i++)
                    {
                        if (!double.IsNaN(CompList[i].STDLiqVolFraction))
                        {
                            HasAtLeastOneValue = true;
                            break;
                        }
                    }
                    break;
            }

            if (HasAtLeastOneValue)
            {
                switch (flowtype)
                {
                    case FlowFlag.Molar:
                        for (int i = 0; i < count; i++)
                        {
                            if (double.IsNaN(CompList[i].MoleFraction))
                                CompList[i].MoleFraction = 0;
                        }
                        break;

                    case FlowFlag.Mass:
                        for (int i = 0; i < count; i++)
                        {
                            if (double.IsNaN(CompList[i].MassFraction))
                                CompList[i].MassFraction = 0;
                        }
                        break;

                    case FlowFlag.LiqVol:
                        for (int i = 0; i < count; i++)
                        {
                            if (double.IsNaN(CompList[i].STDLiqVolFraction))
                                CompList[i].STDLiqVolFraction = 0;
                        }
                        break;
                }

                switch (flowtype)
                {
                    case FlowFlag.Mass:
                        for (int i = 0; i < count; i++)
                        {
                            p = CompList[i];
                            molarflow[i] = p.MassFraction / p.Molwt;
                            volumeflow[i] = p.MassFraction / p.SG_60F;
                            relativemols += molarflow[i];
                            relativevols += volumeflow[i];
                        }

                        if (relativemols == 0)
                            relativemols = double.PositiveInfinity;

                        if (relativevols == 0)
                            relativevols = double.PositiveInfinity;

                        if (relativemols == 0)
                        {
                            for (int i = 0; i < count; i++)//setfractions
                            {
                                p = CompList[i];
                                p.MassFraction = 1 / count;
                                p.STDLiqVolFraction = 1 / count;
                                p.MoleFraction = 1 / count;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < count; i++)//setfractions
                            {
                                p = CompList[i];
                                p.MoleFraction = molarflow[i] / relativemols;
                                p.STDLiqVolFraction = volumeflow[i] / relativevols;
                            }
                        }
                        break;

                    case FlowFlag.LiqVol:

                        double totLiqVolComps = TotalCompFractions(FlowFlag.LiqVol);

                        if (Math.Abs(totLiqVolComps - 1) > 0.0000000000001)//mustbenormalisedfirst
                        {
                            for (int n = 0; n < count; n++)
                                CompList[n].STDLiqVolFraction /= totLiqVolComps;
                        }

                        for (int i = 0; i < count; i++)
                        {
                            p = CompList[i];
                            massflow[i] = p.STDLiqVolFraction * p.SG_60F;
                            molarflow[i] = massflow[i] / p.Molwt;
                            relativemols += molarflow[i];
                            relativemassflow += massflow[i];
                        }

                        if (relativemols == 0)
                        {
                            for (int i = 0; i < count; i++)//setfractions
                            {
                                p = CompList[i];
                                p.MassFraction = 1 / count;
                                p.STDLiqVolFraction = 1 / count;
                                p.MoleFraction = 1 / count;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < count; i++)//setfractions
                            {
                                p = CompList[i];
                                p.MoleFraction = molarflow[i] / relativemols;
                                p.MassFraction = massflow[i] / relativemassflow;
                            }
                        }
                        break;

                    case FlowFlag.Molar:
                        double relativevolflow = 0, relativemolflow = 0;
                        double[] volflow = new double[count];

                        for (int i = 0; i < CompList.Count; i++)
                        {
                            p = CompList[i];
                            massflow[i] = p.MoleFraction * p.Molwt;
                            volflow[i] = massflow[i] / p.SG_60F;
                            relativemassflow += massflow[i];
                            relativevolflow += volflow[i];
                            relativemolflow += p.MoleFraction;
                        }

                        if (relativemassflow == 0)
                        {
                            for (int i = 0; i < count; i++)//setfractions
                            {
                                p = CompList[i];
                                p.MassFraction = 0;
                                p.STDLiqVolFraction = 0;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < count; i++)
                            {
                                p = CompList[i];
                                if (relativevolflow == 0)
                                    p.STDLiqVolFraction = 0;
                                else
                                    p.STDLiqVolFraction = volflow[i] / relativevolflow;

                                if (relativemassflow == 0)
                                    p.MassFraction = 0;
                                else
                                    p.MassFraction = massflow[i] / relativemassflow;

                                if (relativemolflow == 0)
                                    p.MoleFraction = 0;//normalisemolfractions
                                else
                                    p.MoleFraction /= relativemolflow;//normalisemolfractions
                            }
                        }

                        break;

                    case FlowFlag.Undefined:
                        break;

                    default:
                        break;
                }
            }
            else//nothingdefined
            {
                for (int i = 0; i < count; i++)
                {
                    CompList[i].MoleFraction = double.NaN;
                    CompList[i].STDLiqVolFraction = double.NaN;
                    CompList[i].MassFraction = double.NaN;
                    CompList[i].origin = SourceEnum.Empty;
                }
                origin = SourceEnum.Empty;
            }

            //Reset_MW_SG();

            return false;
        }

        public double TotalCompFractions(FlowFlag flag)
        {
            double res = 0;
            int count = CompList.Count;

            switch (flag)
            {
                case FlowFlag.LiqVol:
                    for (int n = 0; n < count; n++)
                        res += CompList[n].STDLiqVolFraction;
                    break;

                case FlowFlag.Mass:
                    for (int n = 0; n < count; n++)
                        res += CompList[n].MassFraction;
                    break;

                case FlowFlag.Molar:
                    for (int n = 0; n < count; n++)
                        res += CompList[n].MoleFraction;
                    break;
            }
            return res;
        }

        public double MolFracSum()
        {
            double temp = 0;
            int count = List.Count;
            for (int i = 0; i < count; i++)
                temp += List[i].MoleFraction;

            return temp;
        }

        public bool IsConsistent(Components comps)
        {
            if (comps.Count != List.Count)
                return false;
            for (int i = 0; i < List.Count; i++)
            {
                if (!comps[i].MoleFraction.AlmostEquals(List[i].MoleFraction))
                    return false;
                if (comps[i] != List[i])
                    return false;
            }

            return true;
        }

        public bool CompPropsConsistent(Components destcomps)
        {
            if (destcomps.Count != List.Count)
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                BaseComp comp = this[i];
                foreach (var prop in comp.Properties.Keys)
                {
                    if (destcomps[i].Properties.ContainsKey(prop))
                    {
                        if (comp.Properties[prop] != destcomps[i].Properties[prop])
                            return false;
                    }
                    else
                        return false;
                }
            }
            return true;
        }

        public void SortByBP()
        {
            CompList.Sort((x, y) => x.MidBP.CompareTo(y.MidBP));
        }

        public void SetMolFractions(double[] fractions, SourceEnum origin = SourceEnum.Input)
        {
            if (fractions.Length == CompList.Count)
                for (int n = 0; n < fractions.Length; n++)
                    CompList[n].MoleFraction = fractions[n];
            else
                System.Windows.Forms.MessageBox.Show("Should be " + CompList.Count + " Componenets, Not " + fractions.Length);

            this.origin = origin;

            NormaliseFractions();
        }

        public void SetMolFractionsIncSolids(double[] fractions, SourceEnum origin = SourceEnum.Input)
        {
            if (fractions.Length == CompList.Count)
            {
                int n = 0;
                for (n = 0; n < CompList.Count; n++)
                    CompList[n].MoleFraction = fractions[n];
                int loc = n;
            }
            else
                System.Windows.Forms.MessageBox.Show("Should be " + CompList.Count + " Componenets, Not " + fractions.Length);

            this.origin = origin;

            NormaliseFractions();
        }

        public void SetMassFractions(double[] fractions, SourceEnum origin = SourceEnum.Input)
        {
            if (fractions.Length == CompList.Count)
                for (int n = 0; n < CompList.Count; n++)
                    CompList[n].MassFraction = fractions[n];

            this.origin = origin;

            this.NormaliseFractions(FlowFlag.Mass);
        }

        public void SetVolFractions(double[] fractions, SourceEnum origin = SourceEnum.Input)
        {
            if (fractions.Length == CompList.Count)
                for (int n = 0; n < CompList.Count; n++)
                    CompList[n].STDLiqVolFraction = fractions[n];

            this.origin = origin;

            this.NormaliseFractions(FlowFlag.LiqVol);
        }

        ///<summary>
        ///
        ///</summary>
        ///<paramname="SG_Density"></param>
        ///<paramname="BP"></param>
        ///<paramname="name"></param>
        ///<paramname="thermo"></param>
        public void UpdateThermoProps(ThermoDynamicOptions thermo)
        {
            BaseComp bc;
            double SG, BP, MW;

            for (int i = 0; i < CompList.Count; i++)
            {
                bc = CompList[i];
                SG = bc.SG_60F;
                BP = bc.MidBP;

                MW = PropertyEstimation.CalcMW(BP, SG, thermo);

                bc.MW = MW;
                bc.HForm25 = -1826.1 * bc.MW + 1E-05;

                bc.CritT = PropertyEstimation.CalcCritT(BP, SG, MW, thermo);
                bc.CritP = PropertyEstimation.CalcCritP(BP, SG, MW, thermo);
                bc.Omega = PropertyEstimation.CalcOmega(BP, SG, bc.CritT, bc.CritP, thermo);
                bc.IdealVapCP = LeeKesler.GetIdealVapCpCoefficients(bc.Omega, bc.Wk, SG);
            }
        }

        public void SetFractVapour(double fraction)
        {
            for (int n = 0; n < CompList.Count; n++)
                CompList[n].MoleFracVap = fraction;
        }

        public void UpdateMoleFractions(double[] molefracs)
        {
            for (int i = 0; i < molefracs.Length; i++)
            {
                CompList[i].MoleFraction = molefracs[i];
            }
            NormaliseFractions();
        }

        internal void UpdateMoleFracs(Components components)
        {
            foreach (BaseComp bc in components)
            {
                this[bc.name].MoleFraction = bc.MoleFraction;
            }
        }
    }
}