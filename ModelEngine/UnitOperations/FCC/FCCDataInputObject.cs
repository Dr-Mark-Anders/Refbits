using ModelEngine.FCC.Petrof;
using System.Collections.Generic;

namespace ModelEngine.UnitOperations.FCC
{
    public class FCCDataInputObject
    {
        private List<PseudoPlantData> Feeds = new();
        private List<ComponentPlantData> GasProducts = new();
        private List<PseudoPlantData> LiqProducts = new();
    }
}