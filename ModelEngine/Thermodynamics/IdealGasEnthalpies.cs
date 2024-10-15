using Extensions;
using System;
using System.Collections.Generic;
using Units.UOM;

namespace ModelEngine
{
    public static class IdealGas
    {
        public const double Rgas = 8.314459848;
        private const double HysysPRef = 0.01;
        public const double basep = 1;
        public static readonly double baseTk = 273.15 + 25;
        public static readonly double Tk;

        private static readonly double baset1 = baseTk,
            baset2 = baset1 * baset1,
            baset3 = baset2 * baset1,
            baset4 = baset3 * baset1,
            baset5 = baset4 * baset1;

        private static double DT1, DT2, DT3, DT4, DT5;

        private static void SetupTemps(double Tk)
        {
            DT1 = (Tk - baset1);
            DT2 = (Tk.Pow(2) - baset2) / 2;
            DT3 = (Tk.Pow(3) - baset3) / 3;
            DT4 = (Tk.Pow(4) - baset4) / 4;
            DT5 = (Tk.Pow(5) - baset5) / 5;
        }

        public static Entropy StreamIdealGasMolarEntropy(Components cc, Pressure P, Temperature T, double[] x)
        {
            double res = 0;
            SetupTemps(T.BaseValue);
            for (int I = 0; I < cc.ComponentList.Count; I++)
                res += IdealGasEntropy(T, P, cc.ComponentList[I], enumMassOrMolar.Molar) * x[I];

            return new Entropy(res);
        }

        public static Enthalpy StreamIdealGasMolarEnthalpy(Components cc, Temperature T, double[] x)
        {
            double res = 0;
            SetupTemps(T.BaseValue);

            int Count = cc.ComponentList.Count;

            for (int i = 0; i < Count; i++)
                res += IdealGasMolarEnthalpy(cc.ComponentList[i]) * x[i];

            return res;
        }

        public static Enthalpy StreamIdealGasMolarEnthalpyAndFormation(Components cc, Temperature T, double[] x)
        {
            double res = 0;
            SetupTemps(T.BaseValue);

            int Count = cc.ComponentList.Count;

            for (int i = 0; i < Count; i++)
                res += IdealGasMolarEnthalpyAndFormation(T, cc.ComponentList[i]) * x[i];

            return res;
        }

        public static Entropy[] StreamIdealGasMolarEntropies(Components cc, Pressure P, Temperature T)
        {
            List<Entropy> res = new();
            for (int I = 0; I < cc.ComponentList.Count; I++)
                res.Add(IdealGasEntropy(T, P, cc.ComponentList[I], enumMassOrMolar.Molar) / cc[I].MW);
            return res.ToArray();
        }

        public static Enthalpy[] StreamIdealGasMolarEnthalpies(Components cc, Temperature T)
        {
            SetupTemps(T.BaseValue);
            List<Enthalpy> res = new();
            int Count = cc.ComponentList.Count;
            for (int I = 0; I < Count; I++)
                res.Add(IdealGasMolarEnthalpy(cc.ComponentList[I]) / cc[I].MW);

            return res.ToArray();
        }

        private static double IdealGasMolarEnthalpy(BaseComp sc)
        {
            double VE = 0;
            double[] cp = sc.IdealVapCP;
            //double baset = 273.15; dont use causes problems in column model

            if (cp != null && cp.Length >= 5)
            {
                VE = cp[0] * DT1
                   + cp[1] * DT2
                   + cp[2] * DT3
                   + cp[3] * DT4
                   + cp[4] * DT5;
            }
            return VE * sc.MW;
        }

        private static double IdealGasMolarEnthalpyAndFormation(Temperature Tk, BaseComp sc)
        {
            double VE = 0;
            double Tk1 = Tk;
            double Tk2 = Tk1 * Tk1; // slightly faster
            double[] cp = sc.IdealVapCP;
            //double baset = 273.15; dont use causes problems in column model

            if (cp != null && cp.Length >= 5)
            {
                VE = cp[0] * DT1
                   + cp[1] * DT2
                   + cp[2] * DT3
                   + cp[3] * DT4
                   + cp[4] * DT5;
            }
            return VE * sc.MW + sc.HForm25;
        }

        // From Correlations
        private static double IdealGasEntropy(Temperature tk, Pressure p, BaseComp sc, enumMassOrMolar mm)
        {
            double VE = double.NaN;
            double[] Cp = sc.IdealVapCP;

            if (Cp != null)
            {
                VE = (Cp[0] * Math.Log(tk)
                + Cp[1] * (tk)
                + Cp[2] * (tk * tk) / 2
                + Cp[3] * (tk * tk * tk) / 3
                + Cp[4] * (tk * tk * tk * tk) / 4
                + 1) * sc.MW
                - Rgas * Math.Log(p / HysysPRef);      // convert to mole basis  // Hysys basis
            }

            if (mm == enumMassOrMolar.Mass)
                return VE / sc.MW;   // kJ/kg
            else
                return VE; // kJ/kg mole
        }

        public static double StreamIdealCp(Components cc, Temperature T, double[] x, enumMassOrMolar mm)
        {
            double res = 0;
            for (int I = 0; I < cc.ComponentList.Count; I++)
                res += cc.ComponentList[I].IdealGasCp(T, enumMassOrMolar.Molar) * x[I];

            if (mm == enumMassOrMolar.Molar)
                return res / cc.MW();
            else
                return res;
        }

        public static Gibbs[] StreamGibbsFormation(Components cc, Temperature T)
        {
            Gibbs[] res = new Gibbs[cc.Count];
            for (int I = 0; I < cc.Count; I++)
                res[I] = GibbsFormation(T, cc[I]);

            return res;
        }

        public static Gibbs StreamGibbsFormation(Components cc, Temperature T, double[] x)
        {
            double res = 0;
            for (int I = 0; I < cc.Count; I++)
                res += GibbsFormation(T, cc[I]) * x[I];

            return new Gibbs(res);
        }

        private static Gibbs GibbsFormation(Temperature Tk, BaseComp sc)
        {
            double G = 0;
            double Tk1 = Tk;
            double Tk2 = Tk1 * Tk1; // slightly faster
            double[] gf = sc.GibbsFree;
            //double baset = 273.15; dont use causes problems in column model

            if (gf != null)
            {
                G = gf[0] + gf[1] * Tk + gf[2] * Tk2;
            }
            return G * 1000;
        }
    }
}