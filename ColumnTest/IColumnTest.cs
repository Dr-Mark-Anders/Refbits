using System.Runtime.InteropServices;

namespace COMColumnNS
{
    [Guid(ColumnTestGuids.Interface)]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IColumnTest
    {
        double TestCritT();

        object ComponentBalance(double T);

        object GetKValues();

        object GetT();

        object GetCompositions();

        object GetPredCompositions();

        void UpdateKValues(object T, object comps);

        bool Start(int notrays);

        bool InitialiseFlowsETC();

        void UpdateTAndSolve(object T);

        object GetkAtT(object T);
        object GetL();

        object GetV();

        bool OneJacobianStep();

        object GetPredC();

        object GetStripFactors();

        void SetStripFactors(object f);

        void SetT(object Tarray);

        void SetTPred(object Tarray);

        bool SolveComponenetBalance();

        object GetEnthalpyBalance();

        object GetErrors();

        object GetJacobian();

        object UpdateAlphas();

    }
}