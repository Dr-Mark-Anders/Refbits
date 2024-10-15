using System.Runtime.InteropServices;

namespace COMColumnNS
{
    [Guid(ContractGuids.Interface)]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICOMColumn
    {
        double TestCritT();

        double LiqEnthalpyReal(object comps, object X, double T, double P, int method);

        double VapEnthalpyReal(object comps, object X, double T, double P, int method);

        double StreamComponentEnthalpy(string c, double T, double P, int method);

        double StreamEnthalpyReal(object comps, object X, double T, double P, int method);

        double StreamTemperatureReal(object comps, object X, double H, double P, int method);

        double LiqComponentEnthalpy(string comp, double T, double P, int method);

        double VapComponentEnthalpy(string comp, double T, double P, int method);

        double DistillationPoint(object comps, object X, object SG, object BP, int method, string DistType, string distpoint);

        double EnthalpyFormationReal(object comps, object X);

        double EnthalpyFormationRealAndQuasi(object comps, object X, object SG, object BP, int method);

        double StreamEnthalpyFixedProps(object comps, object X, double T, double P, object SG, object BP, object CritT, object CritP, object Critv, object MW, object Acentricity, int method, bool usebips);

        double StreamEnthalpy(object comps, object X, double T, double P, object SG, object BP, int method);

        double LiqEnthalpyQuasi(double BP, double SG, double T, double P, int method);

        double LiqEnthalpyRealAndQuasi(object comps, object X, double T, double P, object SG, object BP, int method);

        double VapEnthalpyQuasi(double BP, double SG, double T, double P, int method);

        double VapEnthalpyRealAndQuasi(object comps, object X, double T, double P, object SG, object BP, int method);

        double VapEnthalpyRealAndQuasiFixedProps(object comps, object X, double T, double P, object SG, object BP, object CritT, object CritP, object Critv, object MW, object Acentricity, int method, bool usebips);

        double LiqEnthalpyRealAndQuasiFixedProps(object comps, object X, double T, double P, object SG, object BP, object CritT, object CritP, object Critv, object MW, object Acentricity, int method, bool UseBips);

        double LiqEnthalpyDepPC(double BP, double SG, double T, double P, int method);

        double LiqEnthalpyDepPure(object comps, object X, double T, double P, int method);

        double LiqEnthalpyDepAll(object comps, object X, double T, double P, object SG, object BP, int method);

        double LiqEnthalpyDep(string comp, double T, double P, int method);

        double VapEnthalpyDepPC(double BP, double SG, double T, double P, int method);

        double VapEnthalpyDepAll(object comps, object X, double T, double P, object SG, object BP, int method);

        double VapEnthalpyDepPure(object comps, object X, double T, double P, int method);

        double VapEnthalpyDep(string comp, double T, double P, int method);

        double LiqH_Hig(object comps, object X, double T, double P, object SG, object BP, int method);

        double VapH_Hig(object comps, object X, double T, double P, object SG, object BP, int method);

        object Flash(object comps, object X, object SG, object BP, double T, double P, int method, bool UseBips);

        object FlashWithFixedQuasiProps(object comps, object X, object SG, object BPk, object MW, object TcritK, object PcritBar, object CritVol, object Omega, double Tk, double Pbar, int method, bool UseBips, ref double VFrac);

        object KMixFixedProps(object comps, object X, object Y, object SG, object BPk, object MW, object TcritK, object PcritBar, object CritVol, object Omega, double Tk, double Pbar, int method, bool UseBips);

        double KRealComp(string comp, double T, double P, int method);

        double KQuasiComp(double T, double P, double SG, double Meabp, int method, bool UseBips);

        double KMixPure(object comps, object X, object Y, double T, double P, int method, int comp, bool UseBips);

        object KMixPureArray(object comps, object X, object Y, double T, double P, int method);

        double KMixPureandQuasi(object comps, object SG, object BP, object X, object Y, double T, double P, int method, int comp);

        object KMixPureandQuasiArray(object comps, object SG, object BP, object X, object Y, double T, double P, int method, bool usebips);

        double LnKRealComp(string comp, double T, double P, int method);

        object LnkRealMix(object comps, object X, object Y, double T, double P, int method);

        double LnKRealMixComp(object comp, object X, object Y, double T, double P, int method, int compNo);

        object LnKMixPureandQuasiArray(object comps, object SG, object BP, object X, object Y, double T, double P, int method);

        double LK_Z_Liquid(double Tr, double Pr, double Omega);

        double LK_Z_Vapour(double Tr, double Pr, double Omega);

        double LK_Z0_Vapour_Rig(double Tr, double Pr);

        double LK_Z1_Vapour_Rig(double Tr, double Pr);

        double LK_Z0_Liquid_Rig(double Tr, double Pr);

        double LK_Z1_Liquid_Rig(double Tr, double Pr);

        double LK_Z0_VapourTableInterpolate(double Tr, double Pr);

        double LK_Z1_VapourTableInterpolate(double Tr, double Pr);

        double LK_Z0_LiquidTableInterpolate(double Tr, double Pr);

        double LK_Z1_LiquidTableInterpolate(double Tr, double Pr);

        double LK_Z0_LiquidBisectTable(double Tr, double Pr);

        double LK_Z1_VapourBisectTable(double Tr, double Pr);

        double LK_Z0_VapourBisectTable(double Tr, double Pr);

        double LK_Z1_LiquidBisectTable(double Tr, double Pr);

        double LK_EnDep0(double Z, double Tr, double Pr);

        double LK_EnDep1(double Z, double Tr, double Pr);

        double LK_EnthDepLiquid(double Tr, double Pr, double Omega);

        double LK_EnthDepVapour(double Tr, double Pr, double Omega);

        double LK_Ideal_Enthalpy(double T, double P, double MeABP, double sg, double MW, int MM);

        double LK_VP(double T, double SG, double bp, double CritT, double CritP, double Omega);

        double CritT(double Tb, double SG, int method);

        double CritP(double Tb, double SG, int method);

        double MW(double Tb, double SG, int method);

        double PSat(string comps, double T, int method);

        double PSat2(object comps, object X, double T, int method);

        double TSat2(object comps, object X, double P, int method);

        double TCritMixReal(object comps, object X);

        double TCritMixRealAndQuasi(object comps, object X, object SG, object BP, int method);

        double PCritMix(object comps, object X);

        double PCritMixRealAndQuasi(object comps, object X, object SG, object BP, int method);

        double LK_PCrit(double SG, double BP);

        double LK_TCrit(double SG, double BP);

        double LK_Omega(double SG, double BP, double TCrit, double PCrit);

        double FlashPH(object comps, object X, object SG, object BP, double H, double P, int method, bool UseBips);

        double FlashPHWithFixedQuasiProps(object comps, object X, object SG, object BPk, object MW, object TcritK, object PcritBar, object CritVol, object Omega, double H, double Pbar, int method, bool UseBips, ref double VFrac);
    }
}