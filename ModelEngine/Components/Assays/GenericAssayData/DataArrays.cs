using ModelEngine;
using System;
using System.Collections.Generic;
using Units.UOM;

namespace Units
{
    [Serializable]
    public class DataArrays
    {
        /// <summary>
        /// Key Data Arrays
        /// </summary>
        ///
        public List<string> StreamNames = new();

        public Components Components = new();

        public DataArray PureCompDataRaw = new();
        public List<enumPCTType> PureCompDataType = new();

        public List<double> TotalPureOnStreamVol = new();
        public List<double> TotalPureOnStreamMass = new();

        public List<double> TotalPureOnCrudeVol = new();
        public List<double> TotalPureOnCrudeMass = new();

        public List<double> TotalPseudoOnStreamVol = new();
        public List<double> TotalPseudoOnStreamMass = new();

        public List<double> TotalPseudoOnCrudeVol = new();
        public List<double> TotalPseudoOnCrudeMass = new();

        public DataArray PureCompListMassPCTOnStream = new();
        public DataArray PureCompListVolPCTOnStream = new();

        public DataArray PureCompListMassPCTOnCrude = new();
        public DataArray PureCompListVolPCTOnCrude = new();

        public DataArray PureCompListNormalisedRaw = new();
        public DataArray PureCompListMassPCTNormalised = new();
        public DataArray PureCompListVolPCTNormalised = new();

        public List<double> StreamFlowsRaw = new();
        public enumMassVolMol StreamFlowBasis = new();
        public List<double> StreamFlowsMass = new();
        public List<double> StreamFlowsVol = new();

        public List<double> StreamFlowsFinalMass = new();
        public List<double> StreamFlowsFinalVol = new();

        public List<double> StreamTotalDensityRaw = new();
        public List<enumDensity> StreamDensityBasis = new();
        public List<double> StreamPureCompDensity = new();
        public List<double> StreamPseudoCompDensity = new();
        public List<double> StreamTotalDensity = new();

        public enumTemp TBPCutPointBasis = new();
        public List<double> TBPCutPointsRaw = new();
        public List<Temperature> TBPCutPointsC = new();

        public List<Temperature> MidBP = new();
        public List<double> MW = new();

        public List<enumDistType> DistTypeRaw = new();
        public enumTemp DistTemperatureTypeRaw = new();

        public List<double[]> DistDataRaw = new();
        public double[][] DistDataRawC;

        public DataArray DistDataTBPVol = new();

        /// <summary>
        /// Pure, Pseudo, Mixed
        /// </summary>
        public List<enumStreamComponentType> streamCompType = new();

        public List<DistPoints> TBPdistPoints = new();
        public List<DistPoints> distPointsModified = new();

        public DataArray Properties = new();
        //public  Dictionary<enumAssayPCProperty,double []> Properties2 = new  Dictionary<enumAssayPCProperty, double []>();

        public DataArray PropertiesCalculated = new();

        public int NoPure, NoPSeudo, NoStreams;

        public int NoAllComps
        {
            get
            {
                return NoPSeudo + NoPure;
            }
        }
    }
}