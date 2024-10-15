using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Units.UOM;
using static GenericRxBed.BaseMixedCompclass;

namespace ModelEngine
{
    public class MixedComponent:BaseComp, IComparable<MixedComponent>    
    {
        private Components basecomponents = new();
        public Enthalpy HeatOfCracking = 0;

        public MixedComponent(string name)
        {
            this.name = name;
        }

        public BaseComp this[int index] { get => ((IList<BaseComp>)basecomponents)[index]; set => ((IList<BaseComp>)basecomponents)[index] = value; }

        public int Count => ((ICollection<BaseComp>)basecomponents).Count;

        public bool IsReadOnly => ((ICollection<BaseComp>)basecomponents).IsReadOnly;

        public override Pressure CritP
        {
            get
            {
                return basecomponents.PCritMix();
            }
        }

        public override Temperature CritT
        {
            get
            {
                return basecomponents.TCritMix();
            }
        }

        public override double CritV
        {
            get
            {
                return basecomponents.VCritMix();
            }
        }

        public override  double CritZ
        {
            get
            {
                return basecomponents.ZCritMix();
            }
        }

        public override double Density
        {
            get
            {
                return basecomponents.Density;
            }
        }

        public override double Omega
        {
            get
            {
                return basecomponents.OmegaMix();
            }
        }

        public override double MW
        {
            get
            {
                return basecomponents.MW();
            }
        }

        public override double SG_60F
        {
            get
            {
                return basecomponents.SG();
            }
        }

        public double HForm25Hess
        {
            get
            {
                return basecomponents.Hform25();
            }
        }

        public override string Name { get => name; set => name = value; }
        public Components BaseComponents { get => basecomponents; set => basecomponents = value; }

        public void Add(Components cc)
        {
            foreach (BaseComp item in cc)
            {
                Add(item, item.MoleFraction);
            }

            this.Normalise();
        }


        public void Add(BaseComp bc, double Fraction)
        {
            bc.MoleFraction = Fraction;
            basecomponents.Add(bc.Clone());
        }

        public void Add(CrackList crackcomponents, double[] CrackFractions)
        {
            List<string> list = new List<string>();

            list.AddRange(Enum.GetNames(typeof(CrackList)));
            for (int i = 0; i < list.Count; i++)
            {
                BaseComp bc = Thermodata.GetComponent(list[i]);
                bc.MoleFraction *= CrackFractions[i];
                basecomponents.Add(bc.Clone());
            }
        }


        public void Normalise()
        {
            basecomponents.NormaliseFractions();
        }

        public void SetRatios(List<double> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Count == basecomponents.Count)
                {
                    basecomponents[i].MoleFraction = list[i];
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Not enough values");
                }
            }
            basecomponents.NormaliseFractions();
        }

        public void Add(CrackList cracks, Dictionary<string, double[]> crack)
        {
            throw new NotImplementedException();
        }

        public void Add(double v)
        {
            throw new NotImplementedException();
        }

        int IComparable<MixedComponent>.CompareTo(MixedComponent other)
        {        // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;

            // The temperature comparison depends on the comparison of
            // the underlying Double values.
            return name.CompareTo(other.name);
        }
    }
}