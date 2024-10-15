using System.Collections.Generic;

namespace ModelEngine
{
    public class GCData
    {
        private List<double> list = new List<double>();
        private enumMassMolarOrVol GCbasis = enumMassMolarOrVol.Mass;

        public void SetFractions(List<double> listin)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = listin[i];
            };
        }

        public double this[int i]
        {
            get
            {
                return list[i];
            }
        }

        public enumMassMolarOrVol GCBasis { get => GCbasis; set => GCbasis = value; }

        public List<double> List { get => list; set => list = value; }

        public void Add(double value)
        {
            list.Add(value);
        }

        public GCData Clone()
        {
            GCData newgcdata = new GCData();
            newgcdata.GCbasis = this.GCbasis;
            newgcdata.list.AddRange(this.list);
            return newgcdata;
        }

        public void Clear()
        {
            list.Clear();
        }
    }
}