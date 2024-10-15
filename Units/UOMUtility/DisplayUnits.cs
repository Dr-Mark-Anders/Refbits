using System;
using System.Collections.Generic;
using Units.UOM;

namespace Units
{
    public class DisplayUnits
    {
        public Dictionary<ePropID, string> UnitsDict = new Dictionary<ePropID, string>();

        public DisplayUnits()
        {
            Array props = Enum.GetValues(typeof(ePropID));

            foreach (var item in props)
            {
                IUOM uom = UOMUtility.GetUOM((ePropID)item);
                if (uom != null)
                    UnitsDict[(ePropID)item] = uom.DefaultUnit;
            }
        }
    }
}