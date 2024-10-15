using ModelEngine;
using Extensions;
using System.Collections.Generic;
using System.Drawing;
using Units.UOM;

namespace Units
{
    public partial class CreateAssayClass
    {
        /*
            0   Check Stream Composition Type  (Pure, Mixed, PSeudo)
            1	Normalise Pure wt%
            2	Normalise Pure Vol%
            3	Total Pure Comp Density
            4	Convert stream flows to Vol and Mass
            5   Calcualte MeABP
            6   Calculate MW
            7   Check if All Pure or Mixed etc
            8   Wt% pure on Stream
            9   Wt% pure on Crude
            10  Total Vol% pure on Stream
            11  Total Vol% pure on Crude
            12  Total Vol% PC On stream
            13  Total Vol% PC On Crude
            14  PC Density
            15  Wt % Composition Pure on Crude
            16  Vol% Composiiton Pure on Crude
            17  Vol% Composiiton on Stream
            18  Wt% Composiiton On Stream
            19  Adjust for Light Ends
        */

        public void ProcessSimpleStreamAssay(PseudoPlantData pd, Components cc)
        {
            Density density = (Density)pd.Density.UOM;
        }

        public void ProcessDataFormat()
        {
            data.NoPSeudo = data.Components.Pseudo().Count;
            data.NoPure = data.Components.Pure().Count;
            data.NoStreams = data.StreamNames.Count;

            SGtoDensity();

            RawtoC();

            DistillationsToTBP();

            ConvertTBPCutstoC();

            CheckISPureMixedorPseudo();                     // 0

            NormaliseRawPureCompData();                     // 1

            NormaliseVolandMassPureComps();                 // 2

            PureStreamDensity();                            // 3      Needs normalised raw flows

            RawFlowToVolandMass();                          // 4      If volumes input, needs total stream density and pure stream density

            CalculateMeABP_C();                             // 5      Needs TBP data

            CalculateMW();                                  // 6      Needs Density and VABP

            CheckISPureMixedorPseudo();                     // 7

            StreamTotalWtPCTPure();                         // 8      Total Percent on stream

            TotalMassPureOnCrude();                         // 9

            TotalVolPureOnStream();                         // 10

            TotalVolPureOnCrude();                          // 11

            TotalPseudoVolPCTonStream();                    // 12

            TotalPseudoPercentsOnCrude();                   // 13

            PseudoDensity();                                // 14

            MassPureCrudeComposition();                     // 15

            VolPureCrudeComposition();                      // 16

            WtandVolPCTPurePercentsOnStream();              // 17 % 18

            DoFinalAdjustmentsOnFlows();                    // 19

            ModifyTBPtoAccountforPures();                   // 20
        }

        /// <summary>
        /// First element Contains total Pure flows
        /// </summary>
        private void DoFinalAdjustmentsOnFlows()
        {
            data.StreamFlowsFinalVol.Clear();
            data.StreamFlowsFinalMass.Clear();

            data.StreamFlowsFinalVol.Add(data.TotalPureOnCrudeVol.Sum());
            data.StreamFlowsFinalMass.Add(data.TotalPureOnCrudeMass.Sum());

            for (int stream = 1; stream < data.NoStreams; stream++)
            {
                data.StreamFlowsFinalVol.Add(data.StreamFlowsVol[stream] - data.TotalPureOnCrudeVol[stream]);
                data.StreamFlowsFinalMass.Add(data.StreamFlowsMass[stream] - data.TotalPureOnCrudeMass[stream]);
            }
        }

        private void ConvertTBPCutstoC()
        {
            data.TBPCutPointsC.Clear();
            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                switch (data.TBPCutPointBasis)
                {
                    case enumTemp.C:
                        data.TBPCutPointsC.Add(data.TBPCutPointsRaw[stream].CtoK());
                        break;

                    case enumTemp.F:
                        data.TBPCutPointsC.Add(data.TBPCutPointsRaw[stream].FtoK());
                        break;

                    case enumTemp.K:
                        data.TBPCutPointsC.Add(data.TBPCutPointsRaw[stream]);
                        break;

                    case enumTemp.R:
                        data.TBPCutPointsC.Add(data.TBPCutPointsRaw[stream].RtoK());
                        break;
                }
            }
        }

        private void NormaliseVolandMassPureComps()
        {
            data.PureCompListVolPCTNormalised.Init(data.NoPure, data.NoStreams);
            data.PureCompListMassPCTNormalised.Init(data.NoPure, data.NoStreams);

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                double sum = 0;
                if (stream < data.streamCompType.Count)
                {
                    switch (data.streamCompType[stream])
                    {
                        case enumStreamComponentType.Pure:
                        case enumStreamComponentType.Mixed:
                            switch (data.PureCompDataType[stream])
                            {
                                case enumPCTType.Mass_Stream:
                                case enumPCTType.Mass_Crude:
                                    // to mass
                                    for (int CompNo = 0; CompNo < data.NoPure; CompNo++)
                                        sum += data.PureCompListNormalisedRaw[CompNo][stream];

                                    for (int CompNO = 0; CompNO < data.NoPure; CompNO++)
                                        data.PureCompListMassPCTNormalised.Addvalue(CompNO, stream, data.PureCompListNormalisedRaw[CompNO][stream] / sum, 0);

                                    sum = 0;

                                    // to vol
                                    for (int CompNo = 0; CompNo < data.NoPure; CompNo++)
                                        sum += data.PureCompListNormalisedRaw[CompNo][stream] / data.Components[CompNo].Density;

                                    for (int CompName = 0; CompName < data.NoPure; CompName++)
                                        data.PureCompListVolPCTNormalised.Addvalue(CompName, stream, (data.PureCompListNormalisedRaw[CompName][stream] / data.Components[CompName].Density) / sum, 0);
                                    break;

                                case enumPCTType.LV_Crude:
                                case enumPCTType.LV_Stream:
                                    // to mass
                                    for (int CompNo = 0; CompNo < data.NoPure; CompNo++)
                                        sum += data.PureCompListNormalisedRaw[CompNo][stream] * data.StreamTotalDensity[stream];

                                    for (int compname = 0; compname < data.NoPure; compname++)
                                        data.PureCompListMassPCTNormalised.Addvalue(compname, stream, data.PureCompListNormalisedRaw[compname][stream] * data.StreamTotalDensity[stream] / sum, 0);

                                    sum = 0;

                                    // to vol
                                    for (int CompName = 0; CompName < data.NoPure; CompName++)
                                        sum += data.PureCompListNormalisedRaw[CompName][stream];

                                    for (int compname = 0; compname < data.NoPure; compname++)
                                        data.PureCompListVolPCTNormalised.Addvalue(compname, stream, (data.PureCompListNormalisedRaw[compname][stream]) / sum, 0);
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void TotalMassPureOnCrude()
        {
            data.TotalPureOnCrudeMass.Clear();

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                data.TotalPureOnCrudeMass.Add(data.TotalPureOnStreamMass[stream]
                    * data.StreamFlowsMass[stream]);
            }
        }

        private void TotalVolPureOnCrude()
        {
            data.TotalPureOnCrudeVol.Clear();

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                data.TotalPureOnCrudeVol.Add(data.TotalPureOnStreamVol[stream]
                    * data.StreamFlowsVol[stream] / 100);
            }
        }

        private void VolPureCrudeComposition()
        {
            data.PureCompListVolPCTOnCrude.Init(data.NoPure, data.NoStreams);

            for (int stream = 0; stream < data.NoStreams; stream++)
                for (int row = 0; row < data.NoPure; row++)
                    data.PureCompListVolPCTOnCrude.Addvalue(row, stream, data.PureCompListVolPCTNormalised[row][stream] * data.TotalPureOnCrudeVol[stream], 0);
        }

        private void MassPureCrudeComposition()
        {
            data.PureCompListMassPCTOnCrude.Init(data.NoPure, data.NoStreams);

            for (int stream = 0; stream < data.NoStreams; stream++)
                for (int row = 0; row < data.NoPure; row++)
                    data.PureCompListMassPCTOnCrude.Addvalue(row, stream, data.PureCompListMassPCTNormalised[row][stream]
                        * data.TotalPureOnCrudeMass[stream], 0);
        }

        private void TotalPseudoVolPCTonStream()
        {
            data.TotalPseudoOnStreamVol.Clear();

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                data.TotalPseudoOnStreamVol.Add(100 - data.TotalPureOnStreamVol[stream]);
            }
        }

        private void TotalPseudoPercentsOnCrude()
        {
            data.TotalPseudoOnCrudeVol.Clear();
            data.TotalPseudoOnCrudeMass.Clear();

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                data.TotalPseudoOnCrudeVol.Add(100 - data.TotalPureOnCrudeVol[stream]);
                data.TotalPseudoOnCrudeMass.Add(100 - data.TotalPureOnCrudeMass[stream]);
            }
        }

        private void DistillationsToTBP()
        {
            DistPoints data;
            this.data.TBPdistPoints.Clear();

            for (int stream = 0; stream < this.data.NoStreams; stream++)
            {
                data = new DistPoints();
                data.Add(1, this.data.DistDataRawC[stream][0], TemperatureUnit.Celsius);
                data.Add(5, this.data.DistDataRawC[stream][1], TemperatureUnit.Celsius);
                data.Add(10, this.data.DistDataRawC[stream][2], TemperatureUnit.Celsius);
                data.Add(20, this.data.DistDataRawC[stream][3], TemperatureUnit.Celsius);
                data.Add(30, this.data.DistDataRawC[stream][4], TemperatureUnit.Celsius);
                data.Add(50, this.data.DistDataRawC[stream][5], TemperatureUnit.Celsius);
                data.Add(70, this.data.DistDataRawC[stream][6], TemperatureUnit.Celsius);
                data.Add(80, this.data.DistDataRawC[stream][7], TemperatureUnit.Celsius);
                data.Add(90, this.data.DistDataRawC[stream][8], TemperatureUnit.Celsius);
                data.Add(95, this.data.DistDataRawC[stream][9], TemperatureUnit.Celsius);
                data.Add(99, this.data.DistDataRawC[stream][10], TemperatureUnit.Celsius);

                data = DistillationConversions.Convert(this.data.DistTypeRaw[stream], enumDistType.TBP_WT, data);
                this.data.TBPdistPoints.Add(data); // store dist points
            }
        }

        private void NormaliseRawPureCompData() //
        {
            data.PureCompListNormalisedRaw.Init(data.NoPure, data.NoStreams);

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                double sum = 0;
                if (stream < data.streamCompType.Count)
                {
                    switch (data.streamCompType[stream])
                    {
                        case enumStreamComponentType.Pure:
                        case enumStreamComponentType.Mixed:
                            switch (data.PureCompDataType[stream])
                            {
                                case enumPCTType.Mass_Stream:
                                case enumPCTType.Mass_Crude:
                                case enumPCTType.LV_Crude:
                                case enumPCTType.LV_Stream:

                                    for (int compno = 0; compno < data.PureCompDataRaw.Count; compno++)
                                        if (!double.IsNaN(data.PureCompDataRaw[compno][stream]))
                                            sum += data.PureCompDataRaw[compno][stream];

                                    for (int compno = 0; compno < data.PureCompDataRaw.Count; compno++)
                                        if (!double.IsNaN(data.PureCompDataRaw[compno][stream]))
                                            data.PureCompListNormalisedRaw.Addvalue(compno, stream,
                                                data.PureCompDataRaw[compno][stream] / sum, 0);

                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void CalculateMW()
        {
            data.MW.Clear();
            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                double density = data.StreamTotalDensity[stream];
                double MeABP = data.MidBP[stream];
                if (stream < data.streamCompType.Count)
                    switch (data.streamCompType[stream])
                    {
                        case enumStreamComponentType.Pseudo:

                            /*dataArrays.MW.Add(-12272.6 + 9486.4 * density / 1000
                                + (8.3741 - 5.9917 * density / 1000) * MeABP
                                + (1 - 0.77084 * density / 1000 - 0.02058 * (density / 1000).Pow(2))
                                * (0.7465 - 222.4666 / MeABP) * 10e7 / MeABP
                                + (1 - 0.80882 * density / 1000 + 0.02226 * (density / 1000).Pow(2)
                                * (0.3228 - 17.335 / MeABP) * 10e12 / MeABP.Pow(3)));*/
                            data.MW.Add(MW(density, MeABP));
                            break;

                        case enumStreamComponentType.Mixed:
                            data.MW.Add(MW(density, MeABP));
                            break;

                        case enumStreamComponentType.Pure:
                            double sum = 0;
                            for (int comp = 0; comp < data.Components.Count; comp++)
                                sum += data.Components[comp].MW * data.PureCompListMassPCTNormalised[comp, stream];

                            data.MW.Add(sum);
                            break;
                    }
            }
        }

        public static double MW(double Density, double MeABP)
        {
            double mw;
            //=-12272.6+9486.4*I40/1000+(8.3741-5.9917*I40/1000)*I69
            //+(1-0.77084*I40/1000-0.02058*(I40/1000)^2)*(0.7465-222.4666/I69)*10^7/I69
            //+(1-0.80882*I40/1000+0.02226*(I40/1000)^2)*(0.3228-17.335/I69)*10^12/I69^3
            mw = -12272.6 + 9486.4 * Density / 1000 + (8.3741 - 5.9917 * Density / 1000) * MeABP
                + (1 - 0.77084 * Density / 1000 - 0.02058 * (Density / 1000).Pow(2)) * (0.7465 - 222.4666 / MeABP) * 1e7 / MeABP
                + (1 - 0.80882 * Density / 1000 + 0.02226 * (Density / 1000).Pow(2)) * (0.3228 - 17.335 / MeABP) * 1e12 / MeABP.Pow(3);
            return mw;
        }

        private void CheckISPureMixedorPseudo()
        {
            data.streamCompType.Clear();
            switch (assayType)
            {
                case enumAssayType.Assay:
                case enumAssayType.MultipleStream:
                    for (int stream = 0; stream < data.NoStreams; stream++)
                    {
                        if (data.DistDataRaw.CountValidColumnEntries(stream) > 0 &&
                            data.PureCompDataRaw.CountValidEntries(stream) > 0)
                            data.streamCompType.Add(enumStreamComponentType.Mixed);
                        else if (data.PureCompDataRaw.CountValidEntries(stream) > 0)
                            data.streamCompType.Add(enumStreamComponentType.Pure);
                        else if (data.DistDataRaw.CountValidColumnEntries(stream) > 0)
                            data.streamCompType.Add(enumStreamComponentType.Pseudo);
                    }
                    break;

                case enumAssayType.SingleStream:
                    if (data.DistDataRaw.CountValidColumnEntries(0) > 0 && data.PureCompDataRaw.CountValidEntries(0) > 0)
                        data.streamCompType.Add(enumStreamComponentType.Mixed);
                    else if (data.PureCompDataRaw.CountValidEntries(0) > 0)
                        data.streamCompType.Add(enumStreamComponentType.Pure);
                    else if (data.DistDataRaw.CountValidColumnEntries(0) > 0)
                        data.streamCompType.Add(enumStreamComponentType.Pseudo);
                    break;
            }
        }

        public void TotalVolPureOnStream()
        {
            data.TotalPureOnStreamVol.Clear();

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                data.TotalPureOnStreamVol.Add(0);
                if (stream < data.streamCompType.Count)
                    switch (data.streamCompType[stream])
                    {
                        case enumStreamComponentType.Pure:
                            data.TotalPureOnStreamVol[stream] = 100;
                            break;

                        case enumStreamComponentType.Mixed:
                            //I18*I41/I40
                            data.TotalPureOnStreamVol[stream] = data.TotalPureOnStreamMass[stream] / data.StreamPureCompDensity[stream] * data.StreamTotalDensity[stream];
                            break;
                    }
            }
        }

        public void StreamTotalWtPCTPure()
        {
            data.TotalPureOnStreamMass = new List<double>(new double[data.NoStreams]);

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                if (stream < data.streamCompType.Count)
                    switch (data.streamCompType[stream])
                    {
                        case enumStreamComponentType.Pure:
                            data.TotalPureOnStreamMass[stream] = 100;
                            break;

                        case enumStreamComponentType.Pseudo:
                            data.TotalPureOnStreamMass[stream] = 0;
                            break;

                        case enumStreamComponentType.Mixed:
                            switch (data.PureCompDataType[stream])
                            {
                                case enumPCTType.LV_Stream:
                                    for (int comp = 0; comp < data.NoPure; comp++)
                                        data.TotalPureOnStreamMass[stream] +=
                                            data.PureCompDataRaw[comp][stream] * data.StreamPureCompDensity[stream]
                                            / data.StreamTotalDensity[stream];
                                    break;

                                case enumPCTType.LV_Crude:
                                    for (int comp = 0; comp < data.NoPure; comp++)
                                        data.TotalPureOnStreamMass[stream] +=
                                            data.PureCompDataRaw[comp][stream] * data.StreamPureCompDensity[stream]
                                            / data.StreamTotalDensity[stream] / data.StreamFlowsVol[stream]; break;
                                case enumPCTType.Mass_Stream:
                                    for (int comp = 0; comp < data.PureCompDataRaw.Count; comp++)
                                        data.TotalPureOnStreamMass[stream] += data.PureCompDataRaw[comp][stream];
                                    break;

                                case enumPCTType.Mass_Crude:
                                    for (int comp = 0; comp < data.PureCompDataRaw.Count; comp++)
                                        data.TotalPureOnStreamMass[stream] += data.PureCompDataRaw[comp][stream]
                                            / data.StreamFlowsMass[stream];
                                    break;

                                case enumPCTType.Mol_Stream:
                                    break;

                                case enumPCTType.Mol_Crude:
                                    break;
                            }
                            break;
                    }
            }
        }

        private void PseudoDensity()
        {
            data.StreamPseudoCompDensity.Clear();

            for (int stream = 0; stream < data.streamCompType.Count; stream++)
            {
                switch (data.streamCompType[stream])
                {
                    case enumStreamComponentType.Mixed:

                        // (I40 - I41 * I86 / 100) / (I89 / 100)
                        data.StreamPseudoCompDensity.Add((data.StreamTotalDensity[stream]
                            - data.StreamPureCompDensity[stream] * data.TotalPureOnStreamVol[stream] / 100)
                            / (data.TotalPseudoOnStreamVol[stream] / 100));
                        break;

                    case enumStreamComponentType.Pure:
                        data.StreamPseudoCompDensity.Add(0);
                        break;

                    case enumStreamComponentType.Pseudo:
                        data.StreamPseudoCompDensity.Add(data.StreamTotalDensity[stream]);
                        break;
                }
            }
        }

        private void WtandVolPCTPurePercentsOnStream()
        {
            data.PureCompListVolPCTOnStream.Init(data.NoPure, data.NoStreams);
            data.PureCompListMassPCTOnStream.Init(data.NoPure, data.NoStreams);

            for (int stream = 0; stream < data.NoStreams; stream++)
                for (int Comp = 0; Comp < data.NoPure; Comp++)
                {
                    data.PureCompListVolPCTOnStream[Comp][stream] = data.PureCompListVolPCTNormalised[Comp][stream]
                        * data.TotalPureOnStreamVol[stream];
                    data.PureCompListMassPCTOnStream[Comp][stream] = data.PureCompListMassPCTNormalised[Comp][stream]
                        * data.TotalPureOnStreamMass[stream];
                }
        }

        private void ModifyTBPtoAccountforPures()
        {
            DistPoints distPoints;
            DistPoint distPoint;
            data.distPointsModified.Clear();

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                distPoints = new DistPoints();
                double SumofPure = 0;
                if (stream < data.streamCompType.Count)
                    switch (data.streamCompType[stream])
                    {
                        case enumStreamComponentType.Mixed:
                            distPoint = new DistPoint();
                            for (int comp = 0; comp < data.NoPure; comp++)
                            {
                                distPoint.Pct = data.PureCompListVolPCTOnStream[comp][stream];
                                distPoint.BP = data.Components[comp].BP.Celsius;
                                SumofPure += distPoint.Pct;
                                distPoint.Pct = SumofPure;
                                distPoints.Add(distPoint);
                            }
                            for (int comp = 0; comp < data.DistTypeRaw.Count; comp++)
                            {
                                if (SumofPure > data.TBPdistPoints[stream][comp].Pct)
                                {
                                    distPoint.BP = data.TBPdistPoints[stream][comp].BP;
                                    distPoints.Add(distPoint);
                                }
                                else
                                {
                                    distPoint.Pct = data.TBPdistPoints[stream][comp].Pct;
                                    distPoint.BP = data.TBPdistPoints[stream][comp].BP;
                                    distPoints.Add(distPoint);
                                }
                            }
                            break;

                        case enumStreamComponentType.Pure:
                        case enumStreamComponentType.Pseudo:
                            distPoints = data.TBPdistPoints[stream];
                            break;
                    }

                data.distPointsModified.Add(distPoints);
            }
        }

        private void CalculateMeABP_C()
        {
            data.MidBP.Clear();

            switch (assayType)
            {
                case enumAssayType.Assay:
                    for (int stream = 0; stream < data.NoStreams; stream++)
                    {
                        if (stream == data.TBPCutPointsC.Count - 1)
                            data.MidBP.Add((data.TBPCutPointsC[stream] + 830 + 273.15).Kelvin / 2);
                        else
                            data.MidBP.Add((data.TBPCutPointsC[stream] + data.TBPCutPointsC[stream + 1]).Kelvin / 2D);
                    }
                    break;

                case enumAssayType.MultipleStream:
                    for (int stream = 0; stream < data.NoStreams; stream++)
                    {
                        data.MidBP.Add((data.DistDataRawC[stream][1] + data.DistDataRawC[stream][3] + data.DistDataRawC[stream][5]
                                + data.DistDataRawC[stream][7] + data.DistDataRawC[stream][10]).CtoK() / 5);
                    }
                    break;

                case enumAssayType.SingleStream:
                    data.MidBP.Add((data.DistDataRawC[0][1] + data.DistDataRawC[0][3] + data.DistDataRawC[0][5]
                            + data.DistDataRawC[0][7] + data.DistDataRawC[0][10]).CtoK() / 5);
                    break;
            }
        }

        private void PureStreamDensity()
        {
            data.StreamPureCompDensity.Clear();

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                if (stream < data.streamCompType.Count)
                {
                    if (data.streamCompType[stream] == enumStreamComponentType.Pure)
                    {
                        double Dens = 0;
                        for (int Comp = 0; Comp < data.NoPure; Comp++)
                            Dens += (data.PureCompListVolPCTNormalised[Comp][stream] * data.Components[Comp].Density);

                        data.StreamTotalDensity[stream] = Dens;
                        data.StreamPureCompDensity.Add(Dens);
                    }
                    else if (data.streamCompType[stream] == enumStreamComponentType.Mixed)
                    {
                        double Dens = 0;
                        for (int comp = 0; comp < data.NoPure; comp++)
                            Dens += data.PureCompListVolPCTNormalised[comp][stream] * data.Components[comp].Density;

                        data.StreamPureCompDensity.Add(Dens);
                    }
                    else
                        data.StreamPureCompDensity.Add(0.0);
                }
            }
        }

        private void RawFlowToVolandMass()
        {
            data.StreamFlowsMass.Clear();
            data.StreamFlowsVol.Clear();

            double sum = data.StreamFlowsRaw.Sum();

            for (int stream = 0; stream < data.NoStreams; stream++) // normalise and convert to fractions
                data.StreamFlowsRaw[stream] /= sum;

            for (int stream = 0; stream < data.NoStreams; stream++)
            {
                if (data.StreamFlowBasis == enumMassVolMol.Vol_PCT)
                    data.StreamFlowsMass.Add(data.StreamFlowsRaw[stream] * data.StreamTotalDensity[stream]);
                else
                    data.StreamFlowsVol.Add(data.StreamFlowsRaw[stream] / data.StreamTotalDensity[stream]);
            }

            if (data.StreamFlowBasis == enumMassVolMol.Vol_PCT)
                sum = data.StreamFlowsMass.Sum();
            else
                sum = data.StreamFlowsVol.Sum();

            for (int stream = 0; stream < data.NoStreams; stream++) // normalise
            {
                if (data.StreamFlowBasis == enumMassVolMol.Vol_PCT)
                {
                    data.StreamFlowsVol.Add(data.StreamFlowsRaw[stream]);
                    data.StreamFlowsMass[stream] /= sum;
                }
                else
                {
                    data.StreamFlowsMass.Add(data.StreamFlowsRaw[stream]);
                    data.StreamFlowsVol[stream] /= sum;
                }
            }
        }

        private void RawtoC()
        {
            data.DistDataRawC = new double[data.NoStreams][];
            for (int i = 0; i < data.NoStreams; i++)
                data.DistDataRawC[i] = new double[11];

            if (assayType == enumAssayType.Assay)
            {
                for (int stream = 0; stream < data.NoStreams; stream++)
                {
                    for (int distpont = 0; distpont < 11; distpont++)
                        switch (data.DistTemperatureTypeRaw)
                        {
                            case enumTemp.F:
                                data.DistDataRawC[stream][distpont] = data.DistDataRaw[distpont][stream].FtoC();
                                break;

                            case enumTemp.K:
                                data.DistDataRawC[stream][distpont] = data.DistDataRaw[distpont][stream].KtoC();
                                break;

                            case enumTemp.C:
                                data.DistDataRawC[stream][distpont] = data.DistDataRaw[distpont][stream];
                                break;

                            case enumTemp.R:
                                data.DistDataRawC[stream][distpont] = data.DistDataRaw[distpont][stream].RtoC();
                                break;
                        }
                }
            }
        }

        private void SGtoDensity()
        {
            int row = findRow("Density15");

            bool isSG = false, isDensity = false; ;

            for (int i = 0; i < data.StreamTotalDensity.Count; i++)
            {
                if (data.StreamTotalDensity[i] < 2)
                    isSG = true;
                if (data.StreamTotalDensity[i] > 2)
                    isDensity = true;
            }

            if (isSG & isDensity) //error
            {
                DGV.Rows[row].Cells[1].Style.BackColor = Color.Tomato;
            }
            else if (isSG)
            {
                DGV.Rows[row].Cells[1].Style.BackColor = Color.White;

                for (int i = 0; i < data.StreamTotalDensity.Count; i++) // Convert to Density
                    data.StreamTotalDensity[i] *= 1000;
            }
        }
    }
}