using Extensions;
using Math2;
using ModelEngine;
using NewtonRaphsonSolver;
using System.Collections.Generic;
using System.Linq;
using Units.UOM;

namespace Units
{
    public class GenericSynthesis
    {
        private readonly DataArrays data;
        private readonly ThermoDynamicOptions thermo;
        private Temperature FinalBP = new(875, TemperatureUnit.Celsius);
        private readonly int NoPureComps = 0;
        private readonly int NoPSeudoComps = 0;
        private readonly int NoAllComps = 0;
        private List<Temperature> InitialTBPCutPointsC;
        private readonly List<Temperature> FinalTBPCutPointsC;
        private readonly List<Temperature> CompMidBP;
        private readonly List<Temperature> CompEndBP;
        private readonly List<double> StreamLVFlows;
        private readonly List<Temperature> StreamMidBP;
        private List<double> CompCumLVPCts = new();
        private List<double> CompLvpcts = new();
        private List<double> massPCts = new();
        private List<double> molePCts = new();
        private List<double> StreamSGs = new();
        private DataArray MultStreamLVpcts = new();

        public List<double> Density { get => StreamSGs; set => StreamSGs = value; }
        public List<double> CumLVPCts { get => CompCumLVPCts; set => CompCumLVPCts = value; }
        public List<Temperature> EBP { get => InitialTBPCutPointsC; set => InitialTBPCutPointsC = value; }
        public List<double> MassPCts { get => massPCts; set => massPCts = value; }
        public List<double> MolePCts { get => molePCts; set => molePCts = value; }
        public List<double> LVPCts { get => CompLvpcts; set => CompLvpcts = value; }

        private readonly enumAssayType assytype;
        private readonly AssayPropertyCollection apc;

        public GenericSynthesis(Port_Material port, enumAssayType assaytype, DataArrays data, ThermoDynamicOptions thermo)
        {
            if (port == null
                || port.cc == null
                || port.cc.Count == 0)
                return;
            FinalBP = port.cc.ComponentList.Last().BP;
            apc = new AssayPropertyCollection();

            this.assytype = assaytype;
            this.thermo = thermo;
            this.data = data;

            NoPureComps = data.NoPure;
            NoPSeudoComps = data.Components.Pseudo().Count;
            NoAllComps = NoPureComps + NoPSeudoComps;

            CompCumLVPCts.Clear();
            CompLvpcts.Clear();
            StreamSGs.Clear();
            StreamSGs = data.StreamPseudoCompDensity;
            MultStreamLVpcts.Clear();

            InitialTBPCutPointsC = data.TBPCutPointsC;
            StreamLVFlows = data.StreamFlowsFinalVol.CumulateArray();

            InitialTBPCutPointsC.Add(FinalBP);
            FinalTBPCutPointsC = new List<Temperature>();
            FinalTBPCutPointsC.AddRange(InitialTBPCutPointsC.GetRange(1, InitialTBPCutPointsC.Count - 1));

            StreamMidBP = data.MidBP;

            CompMidBP = new List<Temperature>();
            CompEndBP = new List<Temperature>();
            CompMidBP.AddRange(Components.GetMidBPs(data.Components));
            CompEndBP.AddRange(Components.GetUpperBPs(data.Components));

            //CompMidBP.AddRange(fs.ComponentList.GetMidBPs());
            //CompEndBP.AddRange(fs.ComponentList.GetUpperBPs());
        }

        /// <summary>
        /// Convert Crude Assay Bulk EBP and LV% to Crude PC LV%'s
        /// </summary>
        public Components DoAssay(Components crude, bool optimise)
        {
            switch (assytype)
            {
                case enumAssayType.Assay:
                    CumLVPCts = CalcAssayCumLVpcts();
                    CompLvpcts = CompCumLVPCts.DeCumulateArray<double>();
                    AddPureComponents(CompLvpcts);
                    CompCumLVPCts = LVPCts.CumulateArray();
                    break;

                case enumAssayType.MultipleStream:
                    MultStreamLVpcts = CalcMultipleStreamLVpcts();
                    CombineStreamsLV(MultStreamLVpcts);
                    AddPureComponents(CompLvpcts);
                    CumLVPCts = CompLvpcts.CumulateArray();
                    break;

                case enumAssayType.SingleStream:
                    break;
            }

            //SolveForLVPCts();
            SetLVPCercentsInComponentList(CompLvpcts);
            data.PropertiesCalculated.Clear();

            List<double> Density;

            if (optimise)
            {
                Density = SolveForProperty(enumAssayPCProperty.DENSITY15, optimise);
                data.Components.AddProperty(enumAssayPCProperty.DENSITY15, Density);
            }
            else
            {
                Density = SGfromTBPCurveRaw();
                data.Components.AddProperty(enumAssayPCProperty.DENSITY15, Density);
            }

            List<double> MW = CalMW(CompMidBP, Density);

            data.Components.AddProperty(enumAssayPCProperty.MW, MW);

            massPCts = CompLvpcts.VolumeToMass(Density);
            molePCts = massPCts.MassToMole(MW);

            CreateNewCrude(crude, data.Components, CompLvpcts, Density, NoPureComps, NoAllComps, thermo);

            string[] propnames = data.Properties.Names();
            List<double> tempList;

            for (int i = 0; i < data.Properties.Count; i++)
            {
                if (apc.Contains(propnames[i]))
                {
                    enumAssayPCProperty prop = apc[propnames[i]].Prop;
                    if (prop != enumAssayPCProperty.DENSITY15)
                    {
                        tempList = SolveForProperty(prop, optimise);
                        crude.AddProperty(prop, tempList);
                    }
                }
            }

            return crude;
        }

        private void SetLVPCercentsInComponentList(List<double> LVPECENTS)
        {
            for (int i = NoPureComps; i < LVPCts.Count; i++)
                data.Components[i].STDLiqVolFraction = LVPECENTS[i];
        }

        public List<double> CalMW(List<Temperature> MidBP, List<double> SG)
        {
            List<double> mw = new(new double[NoAllComps]);

            for (int i = 0; i < NoPureComps; i++)
                mw[i] = data.Components[i].MW;

            if (SG != null && MidBP != null)
                for (int i = NoPureComps; i < NoAllComps; i++)
                    mw[i] = PropertyEstimation.CalcMW(MidBP[i], SG[i] / 1000, thermo);

            return mw;
        }

        private readonly EnumQuasiType enumQuasiType = EnumQuasiType.Type1;

        public List<double> CalcAssayCumLVpcts()
        {
            List<double> cumLVPCts = new(new double[NoAllComps]);
            List<Temperature> bps = new();

            bps.AddRange(data.Components.BPArray);

            if (enumQuasiType == EnumQuasiType.Type1)
                bps.AddRange(CompMidBP); // get quasi componenent boiling point  definitions
            else
                bps.AddRange(CompEndBP); // get quasi componenent boiling point  definitions

            for (int i = 0; i < NoPureComps; i++)
                cumLVPCts[i] = data.TotalPureOnCrudeVol.Sum();

            for (int compNo = NoPureComps; compNo < NoAllComps; compNo++) // Should include pures or decumulate re normalises
                cumLVPCts[compNo] = CubicSpline.CubSpline(eSplineMethod.Constrained, bps[compNo], FinalTBPCutPointsC.ToDouble(), StreamLVFlows);  // get lvpct from boiling point s;

            return cumLVPCts;
        }

        public List<double> CalcAssayCumLVpctsWithDelta(List<Temperature> TBPStream, List<double> LVStream, int n)
        {
            List<double> cumLVPCts = new();
            List<Temperature> bps;

            if (enumQuasiType == EnumQuasiType.Type1)
                bps = CompMidBP; // get quasi componenent boiling point  definitions
            else
                bps = CompEndBP;

            for (int i = 0; i < NoPureComps; i++)
                cumLVPCts.Add(data.TotalPureOnCrudeVol.Sum());

            TBPStream[n] += 1;

            for (int comp = NoPureComps; comp < NoAllComps; comp++) // Should include pures or decumulate re normalises
                cumLVPCts.Add(CubicSpline.CubSpline(eSplineMethod.Constrained, bps[comp], TBPStream.ToDouble(), LVStream));  // get lvpct from boiling point s;

            TBPStream[n] -= 1;

            return cumLVPCts;
        }

        public List<double> AddPureComponents(List<double> LVPCts)
        {
            double sum;

            for (int i = 0; i < NoPureComps; i++)
                CompLvpcts[i] = 0;

            for (int i = 0; i < NoPureComps; i++)
            {
                sum = data.PureCompListVolPCTOnCrude[i].Sum();
                LVPCts[i] = sum;
            }

            return LVPCts;
        }

        /// <summary>
        /// Calculate LVPCTs for each stream from stream dist point s, for BBlending
        /// </summary>
        public DataArray CalcMultipleStreamLVpcts()
        {
            Temperature[] bp = StaticCompDef.GetAverageBoilingPoints();
            DataArray MultStreamLVpcts = new();

            for (int stream = 0; stream < data.StreamNames.Count; stream++)
            {
                switch (data.streamCompType[stream])
                {
                    case enumStreamComponentType.Pseudo:
                    case enumStreamComponentType.Mixed:
                        MultStreamLVpcts[stream] = DistillationConversions.GetPseudoCompCumulativeLVfromDistPoints(bp, data.TBPdistPoints[stream]).DeCumulateArray<double>().ToArray();
                        break;

                    case enumStreamComponentType.Pure:
                        MultStreamLVpcts[stream] = new double[data.Components.Count];
                        for (int i = 0; i < data.Components.Count; i++)
                            MultStreamLVpcts[stream][i] = data.PureCompListVolPCTOnStream[i][stream];

                        break;
                }
            }
            return MultStreamLVpcts;
        }

        public List<double> CombineStreamsLV(DataArray MultStreamLVpcts)  // add together to make full crude lV's
        {
            CompLvpcts = new List<double>(new double[data.Components.Count]);

            for (int stream = 0; stream < data.StreamNames.Count; stream++)
                for (int Comp = 0; Comp < data.Components.Count; Comp++)
                    switch (data.streamCompType[stream])
                    {
                        case enumStreamComponentType.Pseudo:
                        case enumStreamComponentType.Mixed:
                        case enumStreamComponentType.Pure:
                            CompLvpcts[Comp] += (MultStreamLVpcts[stream][Comp] / 100 * data.StreamFlowsFinalVol[stream]);
                            break;
                    }

            return CompLvpcts;
        }

        // Synthesise Whole Crude SG Curve from Bulk Stream values, General
        public List<double> SGfromTBPCurveRaw()
        {
            List<double> sg = new();
            if (StreamMidBP.Count == 0)
                return null;
            List<Temperature> PCvabp = StreamMidBP.GetRange(1, StreamMidBP.Count - 1);  // get rid of LE values
            List<double> PCsgstream = StreamSGs.GetRange(1, StreamSGs.Count - 1);

            for (int n = 0; n < NoAllComps; n++)
                sg.Add(CubicSpline.CubSpline(eSplineMethod.Constrained, CompMidBP[n], PCvabp.ToDouble(), PCsgstream));

            for (int i = 0; i < NoPureComps; i++)
                sg[i] = data.Components[i].Density;

            return sg;
        }

        private List<double> PropertyfromTBPCurve(List<double> StreamProperty, List<Temperature> StreamBP, double DELTA, int INDEX, bool allownegative)
        {
            List<double> PCproperty = new();
            double val;

            if (INDEX >= 0)
                StreamProperty[INDEX] += DELTA;

            for (int n = 0; n < NoAllComps; n++)  // only quasi componenets
            {
                val = CubicSpline.CubSpline(eSplineMethod.Constrained, CompMidBP[n], StreamBP, StreamProperty);
                if (!allownegative && val < 0)
                    PCproperty.Add(0);
                else
                    PCproperty.Add(val);
            }

            if (INDEX >= 0)
                StreamProperty[INDEX] -= DELTA;

            return PCproperty;
        }

        /// <summary>
        /// Create PC's from SG and BP, General
        /// </summary>
        public static void CreateNewCrude(Components crude, Components BCC, List<double> CompLvpcts, List<double> SG, int NoPureComps, int NoAllComps, ThermoDynamicOptions thermo)
        {
            if (SG is null)
                return;

            crude.Clear();

            for (int n = 0; n < NoPureComps; n++)
            {
                BaseComp bc = BCC[n];
                bc.STDLiqVolFraction = CompLvpcts[n];
                crude.ComponentList.Add(bc);
            }

            for (int n = NoPureComps; n < NoAllComps; n++)
            {
                PseudoComponent pc = new(SG[n], BCC[n].MidBP, BCC[n].LBP, BCC[n].UBP, BCC[n].Name, thermo);   // SG, BP, Name
                pc.STDLiqVolFraction = CompLvpcts[n];
                pc.HForm25 = -1826.1 * pc.MW + 1E-05;
                crude.ComponentList.Add(pc);
            }
            return;
        }

        public void LVPCTVfromCumulative(Components crude, double[] EBP, double[] LVs)
        {
            if (EBP.Length != LVs.Length)
                return;

            crude.SynthesiseLVPCts = new double[NoAllComps];  // space to temporarily hold LVPC's

            for (int n = NoPureComps; n < NoAllComps; n++)
                crude.SynthesiseLVPCts[n] = crude.SynthesisCumulativeLVPCts[n] - crude.SynthesisCumulativeLVPCts[n - 1];
        }

        public static void CalculateVABPSFromCrudeCurve(Components Crude, PortList oils)
        {
            Temperature[] bps = StaticCompDef.GetAverageBoilingPoints(); // get quasi componenent boiling point  definitions
            Components o;

            for (int i = 0; i < oils.Count; i++)
            {
                double TOTLVPCTS = 0, VABP = 0;
                o = oils[i].cc;
                o.SynthesisVABP = 0;
                for (int n = 0; n < StaticCompDef.Count; n++)
                {
                    if (bps[n].Celsius > o.Lab_IBP && bps[n].Celsius < o.Lab_FBP)
                    {
                        TOTLVPCTS += Crude.SynthesisCumulativeLVPCts[n];  // get lvpcts from crude lvpcts
                        VABP += Crude.SynthesisCumulativeLVPCts[n] * bps[n];
                    }
                }
                VABP /= TOTLVPCTS;
                o.SynthesisVABP = VABP;
            }
        }

        public static void EstimateHForm25(Components crude)
        {
            for (int n = StaticCompDef.NoOfPureComps; n < crude.ComponentList.Count; n++)
                crude.ComponentList[n].HForm25 = -1826.10827 * crude.ComponentList[n].MW + 1E-05;

            return;
        }

        public void SolveForLVPCts()
        {
            int NonLeStreams = data.StreamNames.Count - 1;

            double[] BaseErrors = new double[NonLeStreams];
            double[,] Errors = new double[NonLeStreams, NonLeStreams];
            double Grad = 1;
            double[] NewOffsets;
            //List<double> TBP = InitialTBPCutPointsC.GetRange(1, InitialTBPCutPoint sC.Count-1);  //Knock of first ICP -273.1
            List<Temperature> TBP = FinalTBPCutPointsC;
            List<Temperature> tempTBP = new();
            List<double> CumStreamFlowsVol = data.StreamFlowsFinalVol.CumulateArray();
            List<double> CurrentOffsets = new(new double[NonLeStreams]);

            List<Temperature> ebpshort;
            List<double> lvsshort;

            ebpshort = CompMidBP;
            lvsshort = CompLvpcts.CumulateArray();

            NewtonRaphson NR = new(NonLeStreams, NonLeStreams);

            do
            {
                for (int i = 1; i < TBP.Count; i++) // Cumulative, LE don't chnage(fixed lV Percent)
                    BaseErrors[i - 1] = (CumStreamFlowsVol[i] - CumStreamFlowsVol[i - 1])
                        - (CubicSpline.CubSpline(eSplineMethod.Constrained, TBP[i], ebpshort.ToDouble(), lvsshort) - CubicSpline.CubSpline(eSplineMethod.Constrained, TBP[i - 1], ebpshort.ToDouble(), lvsshort));

                for (int column = 1; column < TBP.Count; column++) // ignore light ends
                {
                    lvsshort = CalcAssayCumLVpctsWithDelta(TBP, CumStreamFlowsVol, column).GetRange(NoPureComps - 1);

                    for (int row = 1; row < data.StreamNames.Count; row++)
                        Errors[column - 1, row - 1] = (CumStreamFlowsVol[row] - CumStreamFlowsVol[row - 1])
                            - (CubicSpline.CubSpline(eSplineMethod.Constrained, TBP[row], ebpshort.ToDouble(), lvsshort) - CubicSpline.CubSpline(eSplineMethod.Constrained, TBP[row - 1], ebpshort.ToDouble(), lvsshort));
                }

                NewOffsets = NR.Solve(BaseErrors, Errors, Grad, CurrentOffsets);

                CurrentOffsets.Clear();
                CurrentOffsets.AddRange(NewOffsets);

                tempTBP.Clear();
                tempTBP.Add(TBP[0]);
                for (int i = 0; i < NonLeStreams; i++)
                    tempTBP.Add(TBP[i + 1] - CurrentOffsets[i]);

                lvsshort = CalcAssayCumLVpctsWithDelta(tempTBP, CumStreamFlowsVol, 0).GetRange(StaticCompDef.NoOfPureComps - 1);
            } while (BaseErrors.SumSQR() > 1e-17);
        }

        public List<double> SolveForProperty(enumAssayPCProperty prop, bool optimise)
        {
            enumMassVolMol blendtype = apc[prop].BlendType;
            bool allownegative = apc[prop].AllowNegative;

            int NonLeStreams = data.Properties[apc[prop].Name].Length;

            NewtonRaphson NR = new(NonLeStreams, NonLeStreams);
            CrudeCutter cc = new();

            double[] BaseErrors = new double[NonLeStreams];
            double[,] Errors = new double[NonLeStreams, NonLeStreams];
            double Delta = 1;
            double[] NewOffsets;
            //List<double> TBP = InitialTBPCutPoint sC.GetRange(1, InitialTBPCutPoint sC.Count-1);  //Knock of first ICP -273.1
            List<Temperature> TBP = FinalTBPCutPointsC;

            List<double> TempStreamProperty = new();
            List<double> StreamProperty = new();

            TempStreamProperty.Clear();

            switch (prop)
            {
                case enumAssayPCProperty.DENSITY15:
                    StreamProperty.AddRange(data.StreamPseudoCompDensity.GetRange(1)); // remove light ends density
                    Delta = 10;
                    break;

                case enumAssayPCProperty.SULFUR:
                    StreamProperty.AddRange(data.Properties[apc[prop].Name]);
                    Delta = 0.1;
                    break;

                default:
                    StreamProperty.AddRange(data.Properties[apc[prop].Name]);
                    break;
            }

            TempStreamProperty.AddRange(StreamProperty);

            List<double> CurrentOffsets = new(new double[NonLeStreams]);
            List<Temperature> FullStreamBPs, ShortStreamBPs = new();

            FullStreamBPs = StreamMidBP.GetRange(1, StreamMidBP.Count - 1); // Boiling point s, remove LE
            int row = data.Properties.GetRowIndex(apc[prop].Name);
            int StreamIndex;

            for (int i = 0; i < data.Properties[apc[prop].Name].Length; i++)
            {
                StreamIndex = data.Properties.DataIndex[row][i];
                ShortStreamBPs.Add(FullStreamBPs[StreamIndex]);
            }

            List<double> property;

            if (StreamProperty.Count <= 1) //  if only one data point  cant curve fit
            {
                property = new List<double>();
                for (int i = 0; i < data.Components.Count; i++)
                {
                    if (StreamProperty.Count == 1)
                        property.Add(StreamProperty[0]);
                    else
                        property.Add(0);
                }
                return property;
            }

            if (StreamProperty.Count == 2) //  if only two data point s make linear fit
            {
                double X1, X2, Y1, Y2;
                Y1 = StreamProperty[0];
                Y2 = StreamProperty[1];

                X1 = ShortStreamBPs[0];
                X2 = ShortStreamBPs[1];

                double Grad = (Y2 - Y1) / (X2 - X1);
                double NewValue;

                property = new List<double>();
                for (int i = 0; i < data.Components.Count; i++)
                {
                    NewValue = (data.Components[i].MidBP - X1) * Grad + Y1;
                    if (!allownegative && NewValue < 0)
                        property.Add(0);
                    else
                        property.Add(NewValue);
                }
                return property;
            }

            List<double> tempStreamLVCP;
            List<double> propvalue = new();
            Components TempCompList = data.Components.Clone();

            property = PropertyfromTBPCurve(TempStreamProperty, ShortStreamBPs, Delta, -1, allownegative); // -1 indicates no delta

            if (!optimise) // just do basic curve fit.
            {
                for (int i = 0; i < StreamProperty.Count; i++)
                {
                    StreamIndex = data.Properties.DataIndex[row][i];
                    tempStreamLVCP = cc.CutStream(TempCompList, TBP[StreamIndex], TBP[StreamIndex + 1], blendtype);    // Stream LVPCTS
                    propvalue.Add(cc.StreamPropValue(tempStreamLVCP, property, blendtype));                          // Calcuate stream properties
                }

                // Add calculated properties to dat.properties calculated
                if (data.PropertiesCalculated.Contains(prop.ToString()))
                    data.PropertiesCalculated[prop.ToString()] = propvalue.ToArray();
                else
                    data.PropertiesCalculated.AddRow(apc[prop].Name, propvalue.ToArray());

                return property;
            }

            //TempCompList.SetLiqVolFlows(property);

            propvalue.Clear();

            for (int i = 0; i < StreamProperty.Count; i++)
            {
                StreamIndex = data.Properties.DataIndex[row][i];
                tempStreamLVCP = cc.CutStream(TempCompList, TBP[StreamIndex], TBP[StreamIndex + 1], blendtype);    // Stream LVPCTS
                propvalue.Add(cc.StreamPropValue(tempStreamLVCP, property, blendtype));                          // Calcuate stream properties
            }

            for (int i = 0; i < StreamProperty.Count; i++)                                                       // Cumulative, LE don't change(fixed lV Percent)
                BaseErrors[i] = StreamProperty[i] - propvalue[i];

            int count = 0;
            do
            {
                count++;
                for (int perturb = 0; perturb < NonLeStreams; perturb++)
                {
                    propvalue.Clear();
                    property = PropertyfromTBPCurve(TempStreamProperty, ShortStreamBPs, Delta, perturb, allownegative); // Get new  curve

                    for (int stream = 0; stream < StreamProperty.Count; stream++)                                       // Ignore light ends
                    {
                        StreamIndex = data.Properties.DataIndex[row][stream];
                        tempStreamLVCP = cc.CutStream(TempCompList, TBP[StreamIndex], TBP[StreamIndex + 1], blendtype);          // Stream LVPCTS
                        propvalue.Add(cc.StreamPropValue(tempStreamLVCP, property, blendtype));                                  // Calcuate stream properties

                        Errors[perturb, stream] = StreamProperty[stream] - propvalue[stream];
                    }
                }

                NewOffsets = NR.Solve(BaseErrors, Errors, Delta, CurrentOffsets);

                for (int i = 0; i < NonLeStreams; i++)
                    CurrentOffsets[i] -= NewOffsets[i];

                for (int i = 0; i < NonLeStreams; i++)
                    TempStreamProperty[i] += CurrentOffsets[i];

                property.Clear();
                property.AddRange(PropertyfromTBPCurve(TempStreamProperty, ShortStreamBPs, Delta, -1, allownegative));          // Calculate new  values after

                propvalue.Clear();

                for (int i = 0; i < StreamProperty.Count; i++)
                {
                    StreamIndex = data.Properties.DataIndex[row][i];
                    tempStreamLVCP = cc.CutStream(TempCompList, TBP[StreamIndex], TBP[StreamIndex + 1], blendtype);    // Stream LVPCTS
                    propvalue.Add(cc.StreamPropValue(tempStreamLVCP, property, blendtype));                          // Calcuate stream properties
                }

                for (int i = 0; i < StreamProperty.Count; i++)                                                       // Cumulative, LE don't change(fixed lV Percent)
                    BaseErrors[i] = StreamProperty[i] - propvalue[i];

                // Add calculated properties to dat.properties calculated
                if (data.PropertiesCalculated.Contains(prop.ToString()))
                    data.PropertiesCalculated[prop.ToString()] = propvalue.ToArray();
                else
                    data.PropertiesCalculated.AddRow(apc[prop].Name, propvalue.ToArray());
            } while (BaseErrors.SumSQR() > 1e-5 && count < 10);

            if (prop == enumAssayPCProperty.DENSITY15) //  reset pure comp densities
                for (int i = 0; i < NoPureComps; i++)
                    property[i] = data.Components[i].Density;

            return property;
        }
    }
}