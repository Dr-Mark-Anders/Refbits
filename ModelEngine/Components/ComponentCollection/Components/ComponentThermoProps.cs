using System;
using System.Collections.Generic;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public partial class Components
    {
        private ThermoProps thermoLiq = new();
        private ThermoProps thermoVap = new();
        private ThermoProps thermoSolids = new();
        private ThermoProps thermoWater = new();

        private ThermoDifferentialPropsCollection thermoLiqDerivatives = new();
        private ThermoDifferentialPropsCollection thermoVapDerivatives = new();

        public ThermoProps ThermoSolids
        {
            get
            {
                return thermoSolids;
            }
            set
            {
                thermoSolids = value;
            }
        }

        public ThermoProps ThermoLiq
        {
            get
            {
                return thermoLiq;
            }
            set
            {
                thermoLiq = value;
            }
        }

        public ThermoProps ThermoVap
        {
            get
            {
                return thermoVap;
            }
            set
            {
                thermoVap = value;
            }
        }

        public ThermoProps ThermoWater
        {
            get => thermoWater;
            set => thermoWater = value;
        }

        public ThermoDifferentialPropsCollection ThermoLiqDerivatives
        {
            get
            {
                return thermoLiqDerivatives;//Thermodynamicsclass .UpdateThermoDerivativeProperties(this,thermo,this.State);
            }
            set
            {
                thermoLiqDerivatives = value;
            }
        }

        public ThermoDifferentialPropsCollection ThermoVapDerivatives
        {
            get
            {
                return thermoVapDerivatives;//Thermodynamicsclass .UpdateThermoDerivativeProperties(this,thermo,this.State);
            }
            set
            {
                thermoVapDerivatives = value;
            }
        }

        public Tuple<double, double> TRAPP_Update(Pressure P, Temperature T)
        {
            Tuple<double, double> res;
            ElyHanley eh = new();
            double visc;
            double thermcond;
            foreach (BaseComp bc in CompList)
            {
                eh.TRAPP2(bc, T, P, out visc, out thermcond, out double den);
                bc.Visc = visc;
                bc.Thermcond = thermcond;
                bc.Density = den;
            }

            visc = 0;
            thermcond = 0;
            foreach (BaseComp bc in CompList) // ASTM D7152
            {
                visc += (14.534 * Math.Log(Math.Log(bc.Visc + 1)) + 10.975) * bc.STDLiqVolFraction;
                thermcond += bc.Thermcond * bc.MassFraction;
            }

            var Viscosity = Math.Exp(Math.Exp((visc - 10.975) / 14.534)) - 1;

            res = new Tuple<double, double>(Viscosity, thermcond);

            return res;
        }

        public void UniquacRSet(double[] R)
        {
            for (int i = 0; i < List.Count; i++)
            {
                List[i].UniquacR = R[i];
            };
        }

        public void UniquacQSet(double[] Q)
        {
            for (int i = 0; i < List.Count; i++)
            {
                List[i].UniquacQ = Q[i];
            };
        }

        public double[] UniquacR
        {
            get
            {
                List<double> Res = new();
                for (int i = 0; i < List.Count; i++)
                {
                    Res.Add(List[i].UniquacR);
                }
                return Res.ToArray();
            }
        }

        public double[] UniquacQ
        {
            get
            {
                List<double> Res = new();
                for (int i = 0; i < List.Count; i++)
                {
                    Res.Add(List[i].UniquacQ);
                }
                return Res.ToArray();
            }
        }

        public double Tbreduced()
        {
            double res = 0;
            foreach (BaseComp bc in CompList)
                res += bc.MoleFraction * bc.MeABP / bc.CritT;

            return res;
        }

        public Temperature TbMixK()
        {
            double res = 0;
            foreach (BaseComp bc in CompList)
                res += bc.MoleFraction * bc.MeABP;

            return new Temperature(res);
        }

        public Temperature Treduced(Temperature T)
        {
            return TCritMix() / T;
        }

        public Temperature TCritMix()
        {
            double res = 0;
            Components cctem = this.RemoveSolids(false);
            cctem.NormaliseFractions();
            foreach (BaseComp bc in cctem)
                res += bc.MoleFraction * bc.CritT;

            return new Temperature(res);
        }

        public Pressure PCritMix()
        {
            double res = 0;
            foreach (BaseComp bc in CompList)
                res += bc.MoleFraction * bc.CritP;

            return new Pressure(res);
        }

        public double VCritMix()
        {
            double res = 0;
            foreach (BaseComp bc in CompList)
                res += bc.MoleFraction * bc.CritV;

            return res;
        }

        public double OmegaMix()
        {
            double res = 0;
            foreach (BaseComp bc in CompList)
                res += bc.MoleFraction * bc.Omega;

            return res;
        }

        public double ZCritMix()
        {
            double res = 0;
            foreach (BaseComp bc in CompList)
                res += bc.MoleFraction * bc.CritZ;

            return res;
        }

        public double TCritMix(double[] x)
        {
            double res = 0;
            for (int i = 0; i < x.Length; i++)
                res += x[i] * TCritKArray[i];//Components[i].CritT._Kelvin;
            return res;
        }

        public double PCritMix(double[] x)
        {
            double res = 0;
            for (int i = 0; i < x.Length; i++)
                res += x[i] * PCritBarAArray[i];//*Components[i].CritP.BaseValue;
            return res;
        }

        public double[] CritTArray
        {
            get
            {
                double[] CritT = new double[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                    CritT[n] = CompList[n].CritT;

                return CritT;
            }
        }

        public double[] CritPArray
        {
            get
            {
                double[] CritP = new double[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                    CritP[n] = CompList[n].CritP;

                return CritP;
            }
        }

        public string[] NameArray
        {
            get
            {
                string[] CritP = new string[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                    CritP[n] = CompList[n].Name;

                return CritP;
            }
        }

        public double[] FracVapArray
        {
            get
            {
                double[] res = new double[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                    res[n] = CompList[n].MoleFracVap;

                return res;
            }
        }

        public Temperature[] BPArray
        {
            get
            {
                Temperature[] bp = new Temperature[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                    bp[n] = CompList[n].MidBP;

                return bp;
            }
        }

        public double[] OmegaArray
        {
            get
            {
                double[] Omega = new double[CompList.Count];

                for (int n = 0; n < CompList.Count; n++)
                    Omega[n] = CompList[n].Omega;

                return Omega;
            }
        }
    }
}