using Extensions;
using System.Collections.Generic;
using Units.UOM;

namespace ModelEngine.FCC.Petrof
{
    public class FCCTest
    {
        public FCCTest()
        {
            Port_Material Feed = new();
            Feed.cc.Add(DefaultStreams.Default());
            Feed.cc.Add(Thermodata.ShortNames);

            PseudoPlantData fd = new PseudoPlantData(Feed.cc, new List<double> { 382, 389, 397, 404, 420, 445, 521, 606, 643, }, TemperatureUnit.Celsius,
                new List<int> { 5, 10, 20, 30, 50, 70, 80, 90, 95, }, 917.9, enumDistType.D1160, SourceEnum.Input, new MassFlow(60.65, MassFlowUnit.te_hr));

            fd.add(enumAssayPCProperty.N2, new MassFraction(500, MassFractionUnit.MassPPM));
            fd.add(enumAssayPCProperty.SULFUR, new MassFraction(0.1, MassFractionUnit.MassPCT));

            Port_Material feed = fd.Port;

            double Ri67 = feed.RI67;
            Temperature MeABP = feed.MeABP;
            double VABP = feed.VABPShortForm;
            double xabp = MeABP.Fahrenheit;
            double MW = feed.MW;
            MoleFlow moles = feed.MolarFlow_.BaseValue;

            StreamProperty RiserVolumne = null;
            double WHSV = feed.MF_ / RiserVolumne;
            double S = feed.S_;
            double N = feed.N_Frac;
            double SG = feed.SG;

            double RICORR = Ri67 - 0.0022 * S - 0.005 * N;
            double SGCORR = SG - 0.0064 * S - 0.01 * N;
            double XFact = 0.01 * xabp - 4;
            double viscosity = 7.37;
        }

        public void TestCharacterise(Components cc)
        {
            CharacteriseCaRings(cc);
        }

        public void CharacteriseCaRings(Components cc)
        {
            TotalCorrelation tc = new();
            for (int i = 0; i < cc.Count; i++)
            {
                BaseComp bc = cc[i];
                double Ca = tc.CA(bc.Density, bc.MW, bc.Properties[enumAssayPCProperty.SULFUR], bc.RI20, 7.7);
                List<double> TotC = new List<double>();

                for (int n = 1; n < 7; n++)
                    if (!bc.IsPure)
                        TotC.Add(Statistics.NORM_DIST(n, bc.ARI, 0.5));

                TotC = TotC.Normalise();

                for (int n = 1; n < 7; n++)
                {
                    switch (n)
                    {
                        case 1:
                            bc.Properties[enumAssayPCProperty.Ca1] = TotC[0];
                            break;

                        case 2:
                            bc.Properties[enumAssayPCProperty.Ca2] = TotC[1];
                            break;

                        case 3:
                            bc.Properties[enumAssayPCProperty.Ca3] = TotC[2];
                            break;

                        case 4:
                            bc.Properties[enumAssayPCProperty.Ca4] = TotC[3];
                            break;

                        case 5:
                            bc.Properties[enumAssayPCProperty.Ca5] = TotC[4];
                            break;

                        case 6:
                            bc.Properties[enumAssayPCProperty.Ca6] = TotC[5];
                            break;

                        case 7:
                            bc.Properties[enumAssayPCProperty.Ca7] = TotC[6];
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}