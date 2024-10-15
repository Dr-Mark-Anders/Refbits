using System;
using System.Collections.Generic;
using Units.UOM;

namespace RefReactor
{
    public partial class NapRefBed
    {
        private List<RefReaction> MixedFeeds = new List<RefReaction>();
        private List<RefReaction> MixedProducts = new List<RefReaction>();
        private List<RefReaction> NormalReactions = new List<RefReaction>();

        private void InitRateFactors(Pressure ReactorP, double MetalAct, double AcidAct, MoleFlow MolFeedRate)
        {
            double IsomPExp = 0.37;
            double CyclPExp = -0.7;
            double CrackPExp = 0.53;
            double DealkPExp = 0.5;

            double DHCHFact = 2.75, DHCPFact = 2.75, ISOMFact = 2.75;
            double OPENFact = 2.75, PHCFact = 2.75, HDAFact = 2.75, NHCFact = 2.75;

            CHDehydrogTerm = MetalAct * DHCHFact / MolFeedRate.lbMole_hr;
            CPDehydrogTerm = MetalAct * DHCPFact / MolFeedRate.lbMole_hr;
            IsomTerm = AcidAct * Math.Pow(ReactorP.ATMA, IsomPExp) * ISOMFact / MolFeedRate.lbMole_hr;
            CyclTerm = AcidAct * Math.Pow(ReactorP.ATMA, CyclPExp) * OPENFact / MolFeedRate.lbMole_hr;
            PCrackTerm = AcidAct * Math.Pow(ReactorP.ATMA, CrackPExp) * PHCFact / MolFeedRate.lbMole_hr;
            DealkTerm = AcidAct * Math.Pow(ReactorP.ATMA, DealkPExp) * HDAFact / MolFeedRate.lbMole_hr;
            NCrackTerm = AcidAct * Math.Pow(ReactorP.ATMA, CrackPExp) * NHCFact / MolFeedRate.lbMole_hr;

            Multipliers[ReformerReactionType.CHDehydrogTerm] = CHDehydrogTerm;
            Multipliers[ReformerReactionType.CPDehydrogTerm] = CPDehydrogTerm;
            Multipliers[ReformerReactionType.CyclTerm] = CyclTerm;
            Multipliers[ReformerReactionType.DealkTerm] = DealkTerm;
            Multipliers[ReformerReactionType.IsomTerm] = IsomTerm;
            Multipliers[ReformerReactionType.NCrackTerm] = NCrackTerm;
            Multipliers[ReformerReactionType.PCrackTerm] = PCrackTerm;
        }

        public override void AddReactions()
        {
            rSet.Add(new RefReaction("CH ↔ BEN + 3H2", new string[] { "CH", "", "BEN", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(8.68391, 60.91098, 47.71902, 220.65263, ReformerReactionType.CHDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCP → BEN + 3H2", new string[] { "MCP", "", "BEN", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(8.40468, 60.91098, 0, 0, ReformerReactionType.CPDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCH ↔ TOL + 3H2", new string[] { "MCH", "", "TOL", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(9.16635, 64.29493, 47.45232, 215.52336, ReformerReactionType.CHDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C7CP ↔  TOL + 3H2", new string[] { "C7CP", "", "TOL", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(8.87161, 64.29493, 43.0002, 195.42703, ReformerReactionType.CPDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("ECH ↔ EB + 3H2", new string[] { "ECH", "", "EB", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(11.18649, 77.15392, 47.74953, 212.7123, ReformerReactionType.CHDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("DMCH ↔ MixedXylenes + 3H2", new string[] { "DMCH", "", "MixedXylenes", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(11.14056, 77.15392, 47.48208, 208.35669, ReformerReactionType.CHDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("PCP ↔ EB + 3H2", new string[] { "PCP", "", "EB", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(10.91626, 77.15392, 44.22683, 192.50512, ReformerReactionType.CPDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C8CP ↔ MixedXylenes + 3H2", new string[] { "C8CP", "", "MixedXylenes", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(10.87813, 77.15392, 47.7315, 213.52846, ReformerReactionType.CPDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CH ↔ C9A + 3H2", new string[] { "C9CH", "", "C9A", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(12.0676, 82.48015, 47.90527, 206.31514, ReformerReactionType.CHDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CP ↔ C9A + 3H2", new string[] { "C9CP", "", "C9A", "H2" }, new int[] { -1, 0, 1, 3 }, new RefRateEquation(11.78571, 82.48015, 44.0558, 188.40955, ReformerReactionType.CPDehydrogTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("OX ↔ PX", new string[] { "OX", "", "PX", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(12.908, 96.99666, -0.54851, -2.95608, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("PX ↔ MX", new string[] { "PX", "", "MX", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(12.908, 96.99666, 0.79004, -0.0545, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MX ↔ OX", new string[] { "MX", "", "OX", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(12.908, 96.99666, -0.24153, 3.01243, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("CH ↔ MCP", new string[] { "CH", "", "MCP", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(8.2122, 96.99666, 4.88114, 15.60491, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCH ↔ C7CP", new string[] { "MCH", "", "C7CP", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(9.5985, 96.99666, 4.45838, 20.13789, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("ECH ↔ PCP", new string[] { "ECH", "", "PCP", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(10.158, 96.99666, 3.52239, 20.20671, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("PCP ↔ C8CP", new string[] { "PCP", "", "C8CP", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(11.208, 96.99666, 0.29286, -12.87422, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C8CP ↔ DMCH", new string[] { "C8CP", "", "DMCH", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(10.158, 96.99666, -3.43419, -16.26587, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("DMCH ↔ ECH", new string[] { "DMCH", "", "ECH", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(11.208, 96.99666, -0.38091, 8.93431, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CH ↔ C9CP", new string[] { "C9CH", "", "C9CP", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(10.5148, 96.99666, 3.84955, 17.90604, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("NC7 ↔ IC7", new string[] { "NC7", "", "IC7", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(2.9775, 59.29729, 0.39547, -8.31492, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("NC6 ↔ IC6", new string[] { "NC6", "", "IC6", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(5.119, 78.27169, -0.16411, -7.74171, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("NC5 ↔ IC5", new string[] { "NC5", "", "IC5", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(3.6946, 71.56506, -0.60385, -7.73987, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("NC4 ↔ IC4", new string[] { "NC4", "", "IC4", "" }, new int[] { -1, 0, 1, 0 }, new RefRateEquation(23.947, 212.70907, -1.81158, -8.24194, ReformerReactionType.IsomTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("CH + H2 ↔ NC6", new string[] { "CH", "H2", "NC6", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(40.6743, 277.13333, -4.98164, -45.18335, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCP + H2 ↔ NC6", new string[] { "MCP", "H2", "NC6", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(40.6097, 277.13333, -9.8627, -60.78781, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCP + H2 ↔ IC6", new string[] { "MCP", "H2", "IC6", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(40.5407, 277.13333, -9.94232, -65.68521, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCH + H2 ↔ NC7", new string[] { "MCH", "H2", "NC7", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(41.0806, 277.13333, -6.34038, -36.7742, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCH + H2 ↔ IC7", new string[] { "MCH", "H2", "IC7", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(41.0806, 277.13333, -6.10107, -42.13627, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C7CP + H2 ↔ NC7", new string[] { "C7CP", "H2", "NC7", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(40.8983, 277.13333, -10.35908, -58.19892, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C7CP + H2 ↔ IC7", new string[] { "C7CP", "H2", "IC7", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(40.8983, 277.13333, -10.39403, -65.07829, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("ECH + H2 ↔ C8P", new string[] { "ECH", "H2", "C8P", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(31.57968, 221.70666, -5.55248, -41.66191, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("DMCH + H2 ↔ C8P", new string[] { "DMCH", "H2", "C8P", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(32.04248, 221.70666, -5.67535, -35.6338, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("PCP + H2 ↔ C8P", new string[] { "PCP", "H2", "C8P", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(31.57968, 221.70666, -9.43751, -61.74022, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C8CP + H2 ↔ C8P", new string[] { "C8CP", "H2", "C8P", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(32.04248, 221.70666, -8.94501, -51.81469, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CH + H2 ↔ C9P", new string[] { "C9CH", "H2", "C9P", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(23.95141, 172.32008, -5.40945, -29.92347, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CP + H2 ↔ C9P", new string[] { "C9CP", "H2", "C9P", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(23.06294, 172.32008, -9.25892, -47.82905, ReformerReactionType.CyclTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9P + H2 → Hydrocracked products", new string[] { "C9P", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(22.84403, 176.48689, 0.0453, -54.84237, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C8P + H2 → Hydrocracked products", new string[] { "C8P", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(26.70394, 212.70907, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("IC7 + H2 → Hydrocracked products", new string[] { "IC7", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(26.39942, 207.33972, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("NC7 + H2 → Hydrocracked products", new string[] { "NC7", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(25.8799, 202.58006, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("IC6 + H2 → Hydrocracked products", new string[] { "IC6", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(27.88949, 223.59117, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("NC6 + H2 → Hydrocracked products", new string[] { "NC6", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(27.26899, 218.93533, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("IC5 + H2 → Hydrocracked products", new string[] { "IC5", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(30.34819, 239.94204, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("NC5 + H2 → Hydrocracked products", new string[] { "NC5", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(26.76859, 216.33028, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("IC4 + H2 → Hydrocracked products", new string[] { "IC4", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(23.63919, 212.70907, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("NC4 + H2 → Hydrocracked products", new string[] { "NC4", "H2", "Hydrocracked", "" }, new int[] { -1, -1, 1, 0 }, new RefRateEquation(25.88269, 218.93533, 0, 0, ReformerReactionType.PCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCH + H2 ↔ CH + C1", new string[] { "MCH", "H2", "CH", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(12.69022, 147.80444, -1.25772, -53.34123, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C7CP + H2 ↔ MCP + C1", new string[] { "C7CP", "H2", "MCP", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(12.69022, 147.80444, -0.83495, -57.87606, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("ECH + H2 ↔ MCH + C1", new string[] { "ECH", "H2", "MCH", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(13.24982, 147.80444, 0.05072, -65.5993, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("DMCH + H2 ↔ MCH + C1", new string[] { "DMCH", "H2", "MCH", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(13.24982, 147.80444, -0.33178, -56.67561, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("PCP + H2 ↔ C7CP + C1", new string[] { "PCP", "H2", "C7CP", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(13.24982, 147.80444, 0.9867, -65.66628, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C8CP + H2 ↔ C7CP + C1", new string[] { "C8CP", "H2", "C7CP", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(13.24982, 147.80444, 0.6948, -52.78466, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CH + H2 ↔ ECH + C1", new string[] { "C9CH", "H2", "ECH", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(13.60372, 147.80444, -0.60358, -48.24983, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CH + H2 ↔ DMCH + C1", new string[] { "C9CH", "H2", "DMCH", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(13.60372, 147.80444, -0.22108, -57.17353, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CP + H2 ↔ PCP + C1", new string[] { "C9CP", "H2", "PCP", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(13.60372, 147.80444, -0.93067, -45.9487, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CP + H2 ↔ C8CP + C1", new string[] { "C9CP", "H2", "C8CP", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(13.30372, 147.80444, -0.63876, -58.83032, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("TOL + H2 ↔ BEN + C1", new string[] { "TOL", "H2", "BEN", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(9.18362, 147.80444, -0.99102, -48.21196, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C8A + H2 ↔ TOL + C1", new string[] { "C8A", "H2", "TOL", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(8.07386, 113.69572, -0.66898, -50.7893, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9A + H2 ↔ C8A + C1", new string[] { "C9A", "H2", "C8A", "C1" }, new int[] { -1, -1, 1, 1 }, new RefRateEquation(14.48641, 140.76613, -0.33683, -53.85162, ReformerReactionType.DealkTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CH + 2H2 → Hydrocracked products", new string[] { "C9CH", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(18.1665, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C9CP + 2H2 → Hydrocracked products", new string[] { "C9CP", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(16.8755, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("ECH + 2H2 → Hydrocracked products", new string[] { "ECH", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(17.1856, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("DMCH + 2H2 → Hydrocracked products", new string[] { "DMCH", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(17.1856, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("PCP + 2H2 → Hydrocracked products", new string[] { "PCP", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(15.8967, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C8CP + 2H2 → Hydrocracked products", new string[] { "C8CP", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(15.9867, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCH + 2H2 → Hydrocracked products", new string[] { "MCH", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(16.087, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("C7CP + 2H2 → Hydrocracked products", new string[] { "C7CP", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(14.7828, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("CH + 2H2 → Hydrocracked products", new string[] { "CH", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(15.8639, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));
            rSet.Add(new RefReaction("MCP + 2H2 → Hydrocracked products", new string[] { "MCP", "H2", "Hydrocracked", "" }, new int[] { -1, -2, 1, 0 }, new RefRateEquation(14.5545, 144.91302, 0, 0, ReformerReactionType.NCrackTerm), NameDict, MixedFeedCompNames, MixedProductCompNames));

            foreach (RefReaction rx in rSet)
            {
                if (rx.IsMixedFeeds)
                    MixedFeeds.Add(rx);
                else if (rx.IsMixedProducts)
                    MixedProducts.Add(rx);
                else
                    NormalReactions.Add(rx);
            }

            NormalReactionsArray = NormalReactions.ToArray();
            MixedFeedsArray = MixedFeeds.ToArray();
            MixedProductsArray = MixedProducts.ToArray();
            reactions = rSet.reactions.ToArray();
        }
    }
}