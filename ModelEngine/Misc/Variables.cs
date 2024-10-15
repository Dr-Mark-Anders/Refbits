// class  and constant definitions for variables within the simulator
//
// Group of constants:
// Properties -- written as XXX_VAR
// Parameters -- written as XXX_PAR
// Types Of Properties -- written as XXX_PROP
// Status Of Properties -- written as XXX_V
// Names For Ports -- written as XXX_PORT
// Types Of Ports -- these are used so often they are just IN, OUT, MAT, ENE, SIG ...
//
// class es:
// BasicProperty -- A property handler
// MATerialPropertyDict -- Dict with MATerial properties
// MATerialArrayPropertyDict -- Dict with MATerial properties
// EnergyPropertyDict -- Dict with energy properties
// ParameterDict -- Dict with parameters
// CompoundList -- List with compounds
//
//

public static class gv
{
    static gv()
    {
    }

    public static bool SETCURRENTPATH = false;
    public const double TINIEST_FLOW = 1E-40;
    public const string T_VAR = "T";
    public const string P_VAR = "P";
    public const string H_VAR = "H";
    public const string HMASS_VAR = "HMass";
    public const string MOLARV_VAR = "molarV";
    public const string molarV_VAR = MOLARV_VAR;
    public const string S_VAR = "S";
    public const string VPFRAC_VAR = "VapFrac";
    public const string MASSVPFRAC_VAR = "MassVapFrac";
    public const string MASSFLOW_VAR = "MassFlow";
    public const string MOLEFLOW_VAR = "MoleFlow";
    public const string VOLFLOW_VAR = "VolumeFlow";
    public const string STDVOLFLOW_VAR = "StdLiqVolumeFlow";
    public const string STDLIQDEN_VAR = "StdLiqMassDensity";
    public const string STDLIQVOL_VAR = "StdLiqMolarVol";
    public const string ENERGY_VAR = "Energy";
    public const string ZFACTOR_VAR = "ZFactor";
    public const string MOLEWT_VAR = "MolecularWeight";
    public const string MOLE_WT = MOLEWT_VAR;
    public const string DELTAT_VAR = "DT";
    public const string DELTAP_VAR = "DP";
    public const string GENERIC_VAR = "Generic";
    public const string LENGTH_VAR = "Length";
    public const string UA_VAR = "UA";
    public const string VOL_VAR = "Volume";
    public const string TIME_VAR = "Time";
    public const string MASS_VAR = "Mass";
    public const string U_VAR = "U";
    public const string CONCENTRATION_VAR = "Concentration";
    public const string RATERXNVOL_VAR = "ReactionRateVol";
    public const string RATERXNCAT_VAR = "ReactionRateCat";
    public const string HUMIDITY_VAR = "Humidity";
    public const string STDGASVOLFLOW_VAR = "StdGasVolumeFlow";
    public const string WORK_VAR = "Work";
    public const string CP_VAR = "Cp";
    public const string CV_VAR = "Cv";
    public const string CPMASS_VAR = "CpMass";
    public const string CVMASS_VAR = "CvMass";

    // public  const string HMASS_VAR = "HMass";

    public const string SMASS_VAR = "SMass";
    public const string DPDVT_VAR = "dPdVt";
    public const string GIBBSFREEENERGY_VAR = "GibbsFreeEnergy";
    public const string HELMHOLTZENERGY_VAR = "HelmholtzEnergy";
    public const string IDEALGASCP_VAR = "IdealGasCp";
    public const string IDEALGASENTHALPY_VAR = "IdealGasEnthalpy";
    public const string IDEALGASENTROPY_VAR = "IdealGasEntropy";
    public const string IDEALGASFORMATION_VAR = "IdealGasFormation";
    public const string IDEALGASGIBBS_VAR = "IdealGasGibbs";
    public const string INTENERGY_VAR = "Internal Energy";
    public const string ISOTHERMALCOMPRESSIBILITY_VAR = "IsothermalCompressibility";
    public const string RESIDUALCP_VAR = "ResidualCp";
    public const string RESIDUALCV_VAR = "ResidualCv";
    public const string RESIDUALENTHALPY_VAR = "ResidualEnthalpy";
    public const string RESIDUALENTROPY_VAR = "ResidualEntropy";
    public const string RXNBASEH_VAR = "rxnBaseH";
    public const string MASSDEN_VAR = "MassDensity";
    public const string MECHANICALZFACTOR_VAR = "MechanicalZFactor";
    public const string SURFACETENSION_VAR = "SurfaceTension";
    public const string SPEEDOFSOUND_VAR = "SpeedOfSound";
    public const string THERMOCONDUCTIVITY_VAR = "ThermalConductivity";
    public const string VISCOSITY_VAR = "Viscosity";
    public const string KINEMATICVISCOSITY_VAR = "KINEMATICVISCOSITY";
    public const string AREA_VAR = "Area";
    public const string VELOCITY_VAR = "Velocity";
    public const string PH_VAR = "pH";
    public const string PSEUDOTC_VAR = "PseudoTc";
    public const string PSEUDOPC_VAR = "PseudoPc";
    public const string PSEUDOVC_VAR = "PseudoVc";
    public const string JT_VAR = "JTCoefficient";
    public const string BUBBLEPOINT_VAR = "BubblePoint";
    public const string DEWPOINT_VAR = "DewPoint";
    public const string WATERDEWPOINT_VAR = "WaterDewPoint";
    public const string BUBBLEPRESSURE_VAR = "BubblePressure";
    public const string RVPD323_VAR = "ReidVaporPressure_D323";
    public const string RVPD1267_VAR = "ReidVaporPressure_D1267";
    public const string FLASHPOINT_VAR = "FlashPoint";
    public const string POURPOINT_VAR = "PourPoint";
    public const string LIQUIDVISCOSITY_VAR = "LiquidViscosity";
    public const string PNA_VAR = "PNA";
    public const string BOILINGCURVE_VAR = "BoilingCurve";
    public const string CETANENUMBER_VAR = "CetaneNumber";
    public const string RON_VAR = "ResearchOctaneNumber";
    public const string MON_VAR = "MotorOctaneNumber";
    public const string GHV_VAR = "GHV";
    public const string NHV_VAR = "NHV";
    public const string NHVMASS_VAR = "NHVMass";
    public const string GHVMASS_VAR = "GHVMass";
    public const string RI_VAR = "RefractiveIndex";
    public const string CO2VSE_VAR = "CO2VSEFreezing";
    public const string CO2LSE_VAR = "CO2LSEFreezing";
    public const string LOWWOBBE_VAR = "LowerWobbeIdx";
    public const string HIGHWOBBE_VAR = "HigherWobbeIdx";
    public const string CUTTEMPERATURE_VAR = "CutTemperature";
    public const string HVAPCTEP_VAR = "HVapConstP";
    public const string HVAPCTET_VAR = "HVapConstT";
    public const string HYDRATETEMPERATURE_VAR = "HydrateTemperature";
    public const string GAPTEMPERATURE_VAR = "GapTemperature";
    public const string BOILINGCURVE_VEC = "BoilingCurve";
    public const string PROPERTYTABLE_MATRIX = "PropertyTable";
    public const string LNFUG_VAR = "LnFugacity";
    public const string CMPIDEALG_VAR = "IdealGasGibbs";
    public const string STDLIQMOLVOLPERCMP_VAR = "StdLiqMolVolPerCmp";
    public const string FRAC_VAR = "Fraction";
    public const string CMPMOLEFRAC_VAR = "MoleFraction";
    public const string MASSFRAC_VAR = "MassFraction";
    public const string STDVOLFRAC_VAR = "StdVolFraction";
    public const string CMPMASSFRAC_VAR = MASSFRAC_VAR;
    public const string NULIQPH_PAR = "LiquidPhases";
    public const string NUSOLPH_PAR = "SolidPhases";
    public const string NUTRAYS_PAR = "Trays";
    public const string NUSTAGES_PAR = "NumberStages";
    public const string NUSTIN_PAR = "NumberStreamsIn";
    public const string NUSTOUT_PAR = "NumberStreamsOut";
    public const string LIQ_MOV = "LiquidMoving";
    public const string MAXITER_PAR = "MaxNumIterations";
    public const string MAXITERCONT_PAR = "MaxControllerIter";
    public const string MAXERROR_PAR = "MaxError";
    public const string MAXABSERROR_PAR = "MaxAbsoluteError";
    public const string R_PAR = "RefluxRatio";
    public const string SIGTYPE_PAR = "SignalType";
    public const string IGNORED_PAR = "Ignored";
    public const string NUSECTIONS_PAR = "NumberSections";
    public const string STDVOLREFT_PAR = "StdLiqVolRefT";

    public const int INTENSIVE_PROP = 1;
    public const int EXTENSIVE_PROP = 2;
    public const int CANFLASH_PROP = 4;

    public const int UNKNOWN_V = 1;
    public const int FIXED_V = 2;
    public const int CALCULATED_V = 4;
    public const int PASSED_V = 8;
    public const int NEW_V = 16;
    public const int ESTIMATED_V = 32;
    public const int PARENT_V = 64;

    public const string IN_PORT = "In1";
    public const string OUT_PORT = "Out1";
    public const string COOLER_IN_PORT = "In1";
    public const string COOLER_OUT_PORT = "Out1";
    public const string HEATER_IN_PORT = "In1";
    public const string HEATER_OUT_PORT = "Out1";
    public const string SIG_PORT = "Signal";
    public const string V_PORT = "Vap";
    public const string L_PORT = "Liq";
    public const string S_PORT = "Solid";
    public const string FEED_PORT = "Feed";
    public const string SOLV_PORT = "Solvent";
    public const string EXTR_PORT = "Extract";
    public const string RAFF_PORT = "Raffinate";
    public const string DELTAP_PORT = "DeltaP";
    public const string DELTAT_PORT = "DeltaT";
    public const string U_PORT = "U";
    public const string UA_PORT = "UA";
    public const int IN = 1;
    public const int OUT = 2;
    public const int MAT = 4;
    public const int ENE = 8;
    public const int SIG = 16;
    public const int LIQUID_PHASE = 1;
    public const int OVERALL_PHASE = 6;
    public const int SOLID_PHASE = 4;
    public const int VAPOUR_PHASE = 0;
}