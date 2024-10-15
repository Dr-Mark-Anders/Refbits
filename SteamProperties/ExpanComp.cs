using Units;
using Units.UOM;

namespace Steam
{
    public enum Factormethod
    { Shultz, Huntington };

    public enum EffType
    { Isen, poly }

    public class ExpansionCompression
    {
        private Func<Pressure, MassEnthalpy, double> func;

        private static double RGas = 8.31446261815324;
        private EffType efftype = EffType.Isen;
        private Factormethod method = Factormethod.Huntington;

        private double isentropicFluidHead = double.NaN; //kw
        private double polytropicFluidHead = double.NaN;
        private double isentropicHead = double.NaN; // m
        private double polytropicHead = double.NaN;
        private double isenEff = double.NaN, polyEff = double.NaN;
        private double polytropicFactor = double.NaN;
        private double power = double.NaN; //kw
        private double polyTropicExponent = double.NaN;
        private double isenTropicExponent = double.NaN;
        private double efficiencyRatio;
        private double tout = double.NaN;
        private double EnthalpyOut = double.NaN;
        private double schultz = double.NaN;
        private double HuntingtonFactor = double.NaN;

        public double IsentropicFluidHead { get => isentropicFluidHead; set => isentropicFluidHead = value; }
        public double PolytropicFluidHead { get => polytropicFluidHead; set => polytropicFluidHead = value; }
        public double IsentropicHead { get => isentropicHead; set => isentropicHead = value; }
        public double PolytropicHead { get => polytropicHead; set => polytropicHead = value; }
        public double IsenEff { get => isenEff; set => isenEff = value; }
        public double PolyEff { get => polyEff; set => polyEff = value; }
        public double PolytropicFactor { get => polytropicFactor; set => polytropicFactor = value; }
        public double Power { get => power; set => power = value; }
        public double PolyTropicExponent { get => polyTropicExponent; set => polyTropicExponent = value; }
        public double IsenTropicExponent { get => isenTropicExponent; set => isenTropicExponent = value; }
        public double EfficiencyRatio { get => efficiencyRatio; set => efficiencyRatio = value; }
        public double Tout { get => tout; set => tout = value; }

        public ExpansionCompression(Func<Pressure, MassEnthalpy, double> Tph,
            Func<Pressure, Temperature, ThermoPropsMass> massprops,
            Pressure Pin,
            Pressure Pout,
            Temperature Tin,
            MassFlow flow,
            double eff, EffType efftype,
            Factormethod method, bool IsExpander)

        {
            ThermoPropsMass PropsIn = massprops(Pin, Tin);
            this.func = Tph;
            this.IsenTropicExponent = PropsIn.K;

            this.efftype = efftype;
            this.method = method;

            if (eff > 1)
                eff /= 100;

            Temperature Tout;
            ThermoPropsMass IsenEffProps;
            MassEnthalpy Hout;

            this.IsenEff = eff;

            IsentropicFluidHead = IsenFluidHead(PropsIn, Pout);
            MassEnthalpy IsenHout = PropsIn.H - IsentropicFluidHead;
            Temperature IsenTout = Tph(Pout, IsenHout);
            ThermoPropsMass Isen100 = massprops(Pout, IsenTout);
            Isen100.H = IsenHout;
            schultz = Schultz(PropsIn, Isen100);
            HuntingtonFactor = Huntington(PropsIn, Isen100, Tph, massprops);

            this.IsenEff = eff;

            if (efftype == EffType.Isen)
            {
                this.IsenEff = eff;

                if (IsExpander)
                    Hout = PropsIn.H.BaseValue - IsentropicFluidHead * this.IsenEff;
                else
                    Hout = PropsIn.H.BaseValue - IsentropicFluidHead / this.IsenEff;

                Tout = Tph(Pout, Hout);

                IsenEffProps = massprops(Pout, Tout);

                PolytropicFluidHead = PolyHeadHeadF(PropsIn, IsenEffProps, IsExpander, method);
                efficiencyRatio = PolytropicFluidHead / IsentropicFluidHead;
                if (IsExpander)
                    PolyEff = eff / efficiencyRatio;
                else
                    PolyEff = eff * efficiencyRatio;

                PolyTropicExponent = Math.Log(PropsIn.P / Pout) / Math.Log(IsenEffProps.V / PropsIn.V);
            }
            else
            {
                this.PolyEff = eff;
                if (IsExpander)
                    Hout = PropsIn.H.BaseValue - IsentropicFluidHead * this.IsenEff;
                else
                    Hout = PropsIn.H.BaseValue - IsentropicFluidHead / this.IsenEff;

                Tout = Tph(Pout, Hout);
                IsenEffProps = massprops(Pout, Tout);
                IsenEffProps.H = Hout;

                int Count = 0;
                do
                {
                    PolytropicFluidHead = PolyHeadHeadF(PropsIn, IsenEffProps, IsExpander, method);

                    if (IsExpander)
                    {
                        efficiencyRatio = PolytropicFluidHead / IsentropicFluidHead;
                        this.IsenEff = eff * efficiencyRatio;
                        Hout = PropsIn.H.BaseValue - IsentropicFluidHead * this.IsenEff;
                    }
                    else
                    {
                        efficiencyRatio = PolytropicFluidHead / IsentropicFluidHead;
                        this.IsenEff = eff / efficiencyRatio;
                        Hout = PropsIn.H.BaseValue - IsentropicFluidHead / this.IsenEff;
                    }
                    Tout = Tph(Pout, Hout);
                    IsenEffProps = massprops(Pout, Tout);
                    Count++;
                } while (Count < 2);

                PolyTropicExponent = Math.Log(PropsIn.P / Pout) / Math.Log(IsenEffProps.V / PropsIn.V);
            }

            this.tout = Tout;
            polytropicHead = polytropicFluidHead / 9.8067 * 1000;
            isentropicHead = isentropicFluidHead / 9.8067 * 1000;

            if (IsExpander)
                power = polytropicFluidHead * PolyEff / 3600 * flow.kg_hr;
            else
                power = polytropicFluidHead / PolyEff / 3600 * flow.kg_hr;

            EnthalpyOut = IsenEffProps.H;
        }

        private MassEnthalpy PolyHeadHeadF(ThermoPropsMass InProps, ThermoPropsMass OutProps,
            bool Isexpander, Factormethod method)
        {
            double PolyHead = PolyFluidHead(InProps, OutProps);

            if (method == Factormethod.Shultz)
            {
                PolyHead *= schultz;
                polytropicFactor = schultz;
            }
            else
            {
                PolyHead *= HuntingtonFactor;
                polytropicFactor = HuntingtonFactor;
            }

            return PolyHead;
        }

        private MassEnthalpy PolyFluidHead(ThermoPropsMass propsin, ThermoPropsMass propsout)
        {
            double Pratio = propsout.P / propsin.P;

            this.PolyTropicExponent = Math.Log(Pratio) / Math.Log(propsin.V / propsout.V);
            double n = PolyTropicExponent;

            double res = (MassEnthalpy)(-(n / (n - 1) * (propsin.Z * RGas * propsin.T.Kelvin / propsin.MW))
                * (Math.Pow(Pratio, (n - 1) / n) - 1));

            return res;
        }

        private MassEnthalpy IsenFluidHead(ThermoPropsMass propsin, Pressure Pout)
        {
            double K = propsin.K;
            double head = (MassEnthalpy)(-(K / (K - 1) * (propsin.Z * RGas * propsin.T / propsin.MW))
                * (Math.Pow(Pout / propsin.P, (K - 1) / K) - 1));
            return head;
        }

        private double Schultz(ThermoPropsMass InProps, ThermoPropsMass Isen100)
        {
            double ns = Math.Log(Isen100.P / InProps.P) / Math.Log(InProps.V / Isen100.V);
            double schultz = (Isen100.H - InProps.H) / (ns / (ns - 1)) / (Isen100.P.kPa * Isen100.V - InProps.P.kPa * InProps.V);
            return schultz;
        }

        private double Huntington(ThermoPropsMass InProps, ThermoPropsMass Isen100, Func<Pressure, MassEnthalpy, double> Tph,
            Func<Pressure, Temperature, ThermoPropsMass> massprops)
        {
            double ns = Math.Log(Isen100.P / InProps.P) / Math.Log(InProps.V / Isen100.V);
            double Z1 = InProps.Z;
            double Z2 = Isen100.Z;

            double PRatio = Isen100.P / InProps.P;
            Pressure P3 = InProps.P * Math.Sqrt(PRatio);

            MassEnthalpy FluidheadP3 = IsenFluidHead(InProps, P3); //kj/kg
            MassEnthalpy H3 = InProps.H - FluidheadP3;
            Temperature T3 = Tph(P3, H3);
            ThermoPropsMass P3_Huntington = massprops(P3, T3);

            double Z3 = P3_Huntington.Z;

            double b = (Z3 - Z1 - Z2 / 2 + Z1 / 2) / (-1 + Math.Sqrt(PRatio) + 1 / 2 - PRatio / 2);
            double a = Z1 - b;
            double c = (Z2 - a - b * PRatio) / (Math.Log(PRatio));

            double hunt = (Isen100.H - InProps.H) / (ns / (ns - 1)) / (Isen100.P.kPa * Isen100.V - InProps.P.kPa * InProps.V);
            return hunt;
        }
    }
}