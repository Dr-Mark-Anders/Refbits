using System;
using System.Collections.Generic;
using Units;

namespace ModelEngine
{
    public class ThermoAdmin
    {
        public ThermoAdmin()
        {
        }

        public void CleanUp()
        { }

        public List<string> GetAvThermoProviderNames()
        {
            return new List<string> { "RefBits" };
        }

        public void AddCompound(object provider, string thCase, string i)
        {
            throw new NotImplementedException();
        }

        public void DeleteCompound(object provider, string thCase, string v)
        {
            throw new NotImplementedException();
        }

        public ThermoProps AddPkgFromName(string provider, string thCase, string pkgName)
        {
            return new ThermoProps();
        }
    }
}