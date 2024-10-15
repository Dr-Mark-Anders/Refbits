using Extensions;
using System.Collections.Generic;
using Units.UOM;

namespace ModelEngine
{


    public partial class Components
    {
        public double[] SynthesisCumulativeLVPCts
        {
            get
            {
                return synthesiseCUMLVPCts;
            }

            set
            {
                synthesiseCUMLVPCts = value;
            }
        }

        //private double SynthesisSGsofPUre;
        public Temperature SynthesisVABP
        {
            get
            {
                return labVABP;
            }

            set
            {
                labVABP = value;
            }
        }

        public static enumStreamSpecs GetStreamSpecsType()
        {
            enumStreamSpecs spec = enumStreamSpecs.None;
            return spec;
        }

        /// <summary>
        /// Check if Pure, Pseudo or mixed
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public enumStreamType CheckIfPurePseudoOrMixed()
        {
            if (Pseudo().Count > 0 && Pure().Count > 0)
                return enumStreamType.Mixed;
            else if (Pseudo().Count > 0)
                return enumStreamType.Pseudo;
            else if (Pure().Count > 0)
                return enumStreamType.Pure;
            else
                return enumStreamType.empty;
        }

        public int MinNonZeroBoiling
        {
            get
            {
                int res = 0;
                double temp = 1000;

                for (int i = 0; i < Count; i++)
                {
                    if (CompList[i] is not SolidComponent)
                    {
                        if (List[i].BP < temp && List[i].MoleFraction > 0)
                        {
                            temp = List[i].BP;
                            res = i;
                        }
                    }
                }
                return res;
            }
        }

        public int MaxNonZeroBoiling
        {
            get
            {
                int res = 0;
                double temp = 0;

                for (int i = 0; i < Count; i++)
                {
                    if (CompList[i] is not SolidComponent)
                    {
                        if (List[i].BP > temp && List[i].MoleFraction > 0)
                        {
                            temp = List[i].BP;
                            res = i;
                        }
                    }
                }
                return res;
            }
        }

        public double HydrocarbonPCT
        {
            get
            {
                double res = 0;
                for (int i = 0; i < CompList.Count; i++)
                {
                    BaseComp bc = CompList[i];
                    switch (bc.name)
                    {
                        case "H2":
                        case "O2":
                        case "N2":
                        case "H2O":
                            break;

                        default:
                            res += bc.MoleFraction;
                            break;
                    }
                }
                return res;
            }
        }

        public Gibbs GibbsFormation()
        {
            Gibbs res = 0;

            foreach (BaseComp item in CompList)
            {
                res += item.GForm25 * item.MoleFraction;
            }
            return res;
        }

        public Gibbs G(Quality Q)
        {
            if (Q == 1 && thermoVap != null)
                return thermoVap.G;
            else if (Q == 0 && ThermoLiq != null)
                return ThermoLiq.G;
            else if (Q > 0 && Q < 1)
                return thermoVap.G * Q + ThermoLiq.G * (1 - Q);
            else
                return double.NaN;
        }

        public Helmotz HelmholtzEnergy(Pressure P, Temperature T, Quality Q)
        {
            if (Count == 1 && CompList[0] is WaterSteam steam)
            {
                return steam.A_MASS(P, T);
            }

            if (Q == 1 && thermoVap != null)
                return thermoVap.A;
            else if (Q == 0 && ThermoLiq != null)
                return ThermoLiq.A;
            else if (Q > 0 && Q < 1
                && thermoLiq != null
                && thermoVap != null)
                return thermoVap.A * Q + ThermoLiq.A * (1 - Q);
            else
                return double.NaN;
        }

        public Enthalpy[] Hform25Array()
        {
            List<Enthalpy> res = new();

            foreach (BaseComp item in CompList)
            {
                res.Add(item.HForm25);
            }
            return res.ToArray();
        }

        public Enthalpy Hform25()
        {
            Enthalpy res = 0;

            foreach (BaseComp item in CompList)
            {
                res += item.HForm25 * item.MoleFraction;
            }
            return res;
        }

        public Enthalpy Hform25(double[] moles)
        {
            Enthalpy res = 0;

            for (int i = 0; i < CompList.Count; i++)
            {
                res += CompList[i].HForm25 * moles[i];
            }
            return res;
        }

        public HeatCapacity CP(Temperature T, Quality Q)
        {
            HeatCapacity res = 0;
            if (Count == 1 && CompList[0] is WaterSteam steam)
            {
                return steam.CP(T);
            }

            if (thermoLiqDerivatives != null && thermoVapDerivatives != null)
            {
                if (Q == 0)
                    res += thermoLiqDerivatives.Cp;
                else if (Q == 1)
                    res += thermoVapDerivatives.Cp;
                else
                    res += thermoLiqDerivatives.Cp * (1 - Q) + thermoVapDerivatives.Cp * Q;
            }
            return res;
        }

        public MassHeatCapacity CP_MASS(Pressure P, Temperature T, Quality Q)
        {
            if (Count == 1 && CompList[0] is WaterSteam steam)
            {
                return steam.CP_MASS(P, T);
            }

            MassHeatCapacity res = 0;
            res.BaseValue = CP(T,Q) / MW();
            return res;
        }

        public HeatCapacity CV(Temperature T, Quality Q)
        {
            HeatCapacity res = 0;

            if (Count == 1 && CompList[0] is WaterSteam steam)
            {
                return steam.CV(T);
            }

            if (thermoLiqDerivatives != null && thermoVapDerivatives != null)
            {
                if (Q == 0)
                    res += thermoLiqDerivatives.Cv;
                else if (Q == 1)
                    res += thermoVapDerivatives.Cv;
                else
                    res += thermoLiqDerivatives.Cv * (1 - Q) + thermoVapDerivatives.Cv * Q;
            }

            return res;
        }

        public MassHeatCapacity CV_MASS(Pressure P, Temperature T, Quality Q)
        {
            if (Count == 1 && CompList[0] is WaterSteam steam)
            {
                return steam.CV_MASS(T);
            }

            MassHeatCapacity res = 0;
            res.BaseValue = CV(T,Q) / MW();
            return res;
        }

        public Density ActLiqSG(Pressure P, Temperature T)
        {
            if (Count == 1 && CompList[0] is WaterSteam steam)
            {
                return steam.ActLiqDensity(P, T) / Constants.WaterSpecGravityAt60F;
            }

            return PropertyCalcs.ActLiqDensity(this, P, T) / Constants.WaterSpecGravityAt60F;
        }

        public MassHeatCapacity CP_MASS_Ideal(Pressure P, Temperature T)
        {
            if (Count == 1 && CompList[0] is WaterSteam steam)
            {
                return steam.CP_MASS(P, T);
            }

            return IdealGas.StreamIdealCp(this, T, this.MoleFractions, enumMassOrMolar.Mass);
        }

        public HeatCapacity CP_Ideal(Temperature T)
        {
            return IdealGas.StreamIdealCp(this, T, this.MoleFractions, enumMassOrMolar.Molar);
        }

        public Enthalpy StreamEnthalpy(Quality Q)
        {
            Enthalpy H = double.NaN;

            if (double.IsNaN(Q))
                return double.NaN;

            if (thermoLiq != null && ThermoVap != null && Q > 0 && Q < 1)
                H = thermoLiq.H * (1 - Q) + thermoVap.H * Q;
            else if (thermoLiq != null && Q == 0)
                H = thermoLiq.H;
            else if (thermoVap != null && Q == 1)
                H = thermoVap.H;

            return H;
        }

        public Entropy StreamEntropy(Quality Q)
        {
            Entropy S = 0;

            if (double.IsNaN(Q))
                return double.NaN;

            if (thermoLiq != null && ThermoVap != null && Q > 0 && Q < 1)
                S = thermoLiq.S * (1 - Q) + thermoVap.S * Q;
            else if (thermoLiq != null && Q == 0)
                S = thermoLiq.S;
            else if (thermoVap != null && Q == 1)
                S = thermoVap.S;

            return S;
        }

        public InternalEnergy InternalEnergy(Pressure P, Temperature T, Quality Q)
        {
            if (Count == 1 && CompList[0] is WaterSteam steam)
            {
                return steam.U(P, T);
            }

            if (Q == 1 && thermoVap != null)
                return thermoVap.U;
            else if (Q == 0 && ThermoLiq != null)
                return ThermoLiq.U;
            else if (Q > 0 && Q < 1
                && thermoLiq != null
                && thermoVap != null)
                return thermoVap.U * Q + ThermoLiq.U * (1 - Q);
            else
                return double.NaN;
        }

        public Gibbs GFormT(Pressure P, Temperature T, Quality Q)
        {
            return H(P,T,Q) - StreamEntropy(Q) * T;
        }

        public Entropy SForm25()
        {
            return (GibbsFormation() - Hform25()) / -298.15;
        }

        private double WaterMoleFrac;

        /// <summary>
        /// Also Normalises
        /// </summary>
        /// <returns></returns>
        public void ClearMoleFractions()
        {
            foreach (var bc in CompList)
            {
                bc.MoleFraction = double.NaN;
            }
        }

        public void ZeroMoleFractions()
        {
            foreach (var bc in CompList)
            {
                bc.MoleFraction = 0;
            }
        }
    }
}