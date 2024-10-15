using System;
using System.Collections.Generic;

namespace ModelEngine
{
    public class UniquacData
    {
        public struct UNIQUCDATA
        {
            public string comp1, comp2;
            public double param1, param2;

            public UNIQUCDATA(string comp1, string comp2, double param1, double param2)
            {
                this.comp1 = comp1;
                this.comp2 = comp2;
                this.param1 = param1;
                this.param2 = param2;
            }
        }

        public static List<UNIQUCDATA> UniquacParams()
        {
            List<UNIQUCDATA> data = new();
            data.Add(new UNIQUCDATA("H2O", "78-93-3", 10.74651718, 1187));
            data.Add(new UNIQUCDATA("H2O", "79-09-4", -142.301239, 570.6130981));
            data.Add(new UNIQUCDATA("78-93-3", "79-09-4", 579.081359, -327.8381958));

            return data;
        }

        public void GetUniquacR_P(Components cc)
        {
            cc.UniquacRSet(new double[] { 0.920000017, 3.247900009, 2.87680006 });
            cc.UniquacQSet(new double[] { 1.399700046, 2.87590003, 2.611900091 });
        }

        public Tuple<double, double> GetData(string name1, string name2)
        {
            List<UNIQUCDATA> data = UniquacParams();
            Tuple<double, double> res = null;

            BaseComp Comp1, Comp2;

            Comp1 = Thermodata.GetComponent(name1);
            if (Comp1 is null)
                Comp1 = Thermodata.GetRealComponentCAS(name1);

            Comp2 = Thermodata.GetComponent(name2);
            if (Comp2 is null)
                Comp2 = Thermodata.GetRealComponentCAS(name2);

            if (Comp1 != null && Comp2 != null)
            {
                foreach (var item in data)
                {
                    if ((Comp1.name == item.comp1 || Comp1.CAS == item.comp1) &&
                        (Comp2.name == item.comp2 || Comp2.CAS == item.comp2))
                    {
                        res = new Tuple<double, double>(item.param1, item.param2);
                        return res;
                    }
                    else if ((Comp1.name == item.comp2 || Comp1.CAS == item.comp2) &&
                        (Comp2.name == item.comp1 || Comp2.CAS == item.comp1))
                    {
                        res = new Tuple<double, double>(item.param2, item.param1);
                        return res;
                    }
                }
            }

            return null;
        }
    }
}