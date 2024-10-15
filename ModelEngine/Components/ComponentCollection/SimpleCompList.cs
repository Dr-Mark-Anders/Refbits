using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class SimpleCompList : ISerializable
    {
        private List<BaseComp> _Components = new List<BaseComp>();
        private MassFlow massFlow;
        public enumMassMolarOrVol basis = enumMassMolarOrVol.Mass;
        public List<string> Names;
        private string name;

        public SimpleCompList()
        {
        }

        public void SetFractions(List<double> list)
        {
            if (list.Count == 60)
            {
                for (int i = 0; i < 60; i++)
                {
                    Components[i].MoleFraction = list[i];
                }
            }
        }

        public SimpleCompList(SerializationInfo info, StreamingContext context)
        {
            Components = (List<BaseComp>)info.GetValue("Components", typeof(List<BaseComp>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Components", Components);
        }

        public SimpleCompList(List<BaseComp> Components)
        {
            this.Components = Components;
        }

        public double SG
        {
            get
            {
                return MassFlow / VF;
            }
        }

        public double MW
        {
            get
            {
                double mw = 0;

                for (int i = 0; i < Components.Count; i++)
                {
                    mw = Components[i].MoleFraction * Components[i].Molwt;
                }

                return mw;
            }
        }

        public double VF
        {
            get
            {
                double mw = 0;

                for (int i = 0; i < Components.Count; i++)
                {
                    mw = Components[i].MoleFraction * Components[i].Molwt / Components[i].SG_60F;
                }

                return mw;
            }
        }

        public MassFlow MassFlow { get => massFlow; set => massFlow = value; }
        public List<BaseComp> Components { get => _Components; set => _Components = value; }
        public string Name { get => name; set => name = value; }
    }
}