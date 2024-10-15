using Units.UOM;

namespace Steam97
{
    public partial class StmPropIAPWS97
    {
        public int SubRegion(ref double p, ref Temperature t)
        {
            // return  s the region
            // t is Temperature  in K
            // p is Pressure  in kPa

            if (t == state.t && p == state.p)
                return state.region;

            int region = -1;

            if (p > 100000.0)
            {
                errorCondition = 1;
                AddErrorMessage("Pressure  is out of bounds, Results are for 100 MPa");
                p = 100000.0;
            }
            else if (p < 0.0)
            {
                errorCondition = 1;
                AddErrorMessage("Pressure  is out of bounds, Results are for 0");
                p = 0.0;
            }

            if (t < 273.15)
            {
                errorCondition = 1;
                AddErrorMessage("Temperature  is out of bounds, Results are for 273.15 K");
                t = 273.15;
                region = 1;
            }
            else if (t < 623.15)
            {
                // calculate saturated Pressure
                Pressure psat = Pi_4(t);
                if (p < psat.kPa)
                    region = 2;  // Vapour
                else
                    region = 1;  // Liquid
            }
            else if (t < 863.15)
            {
                // calculate boundary between regions 2 and 3

                Pressure p23 = P23(t);

                if (p < p23.kPa)
                    region = 2;
                else
                    region = 3;
            }
            else if (t < 1073.15)
            {
                region = 2;
            }
            else if (t < 2273.15)
            {
                if (p > 50000.0)
                {
                    errorCondition = 1;
                    AddErrorMessage("Pressure  is out of bounds, Results are for 50 MPa");
                    p = 50000.0;
                    region = 5;
                }
                region = 5;
            }
            else
            {
                errorCondition = 1;
                AddErrorMessage("Temperature  is out of bounds, Results are for 2273.15 K");
                if (p > 50000.0)
                {
                    AddErrorMessage("Pressure  is out of bounds, Results are for 50 MPa");
                    p = 50000.0;
                }
                t = 2273.15;
                region = 5;
            }

            state.region = region;
            return region;
        }
    }
}