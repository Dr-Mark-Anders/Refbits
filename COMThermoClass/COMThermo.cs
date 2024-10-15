using System.Diagnostics;
using System.Runtime.InteropServices;

namespace COMColumnNS
{
    [Guid(ContractGuids.ServerClass)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public partial class COMThermo : ICOMColumn
    {
        public COMThermo()
        {
            //Thermodata data = new();
        }

        public double TestCritT()
        {
            Debugger.Launch();
            return 201;
        }
    }
}