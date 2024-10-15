using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units.UOM;

namespace ModelEngine
{
    public class CalibrationFactors
    {
        private double DHCHFact = 1; //2.75;
        private double DHCPFact = 5; //2.75;
        private double ISOMFact = 1;// 2.75;
        private double OPENFact = 7;// n2.75;
        private double PHCFact = 8;// 2.75;
        private double NHCFact = 1;// 2.75;
        private double HDAFact = 1;// 2.75;

        private double IsomPExp = 0.37;
        private double CyclPExp = -0.7;
        private double CrackPExp = 0.53; // .433
        private double DealkPExp = 0.5;

        private double MetalAct = 1;
        private double AcidAct = 1;

        private double NaphMolFeed = 0;

        public double CHDehydrogTerm = 1;
        public double CPDehydrogTerm = 1;
        public double IsomTerm = 1;
        public double CyclTerm = 1;
        public double PCrackTerm = 1;
        public double DealkTerm = 1;
        public double NCrackTerm = 1;
        internal double None = 1;

        public void Solve(Pressure ReactorP)
        {
            CHDehydrogTerm = MetalAct * DHCHFact;
            CPDehydrogTerm = MetalAct * DHCPFact;
            IsomTerm = AcidAct * Math.Pow(ReactorP, IsomPExp) * ISOMFact;
            CyclTerm = AcidAct * Math.Pow(ReactorP, CyclPExp) * OPENFact;
            PCrackTerm = AcidAct * Math.Pow(ReactorP, CrackPExp) * PHCFact;
            DealkTerm = AcidAct * Math.Pow(ReactorP, DealkPExp) * HDAFact;
            NCrackTerm = AcidAct * Math.Pow(ReactorP, CrackPExp) * NHCFact;
        }
    }
}
