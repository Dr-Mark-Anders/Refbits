using Extensions;
using Math2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public partial class Components
    {
        public double SG_Calc()//shouldbebasedonmolefracs
        {
            BaseComp p;
            double mf = 0;
            double vf = 0;

            int count = CompList.Count;

            for (int i = 0; i < count; i++)
            {
                p = CompList[i];
                mf += p.MoleFraction * p.MW;
                vf += p.MoleFraction * p.MW / p.SG_60F;
            }

            return mf / vf;
        }

        public double MW()
        {
            double mw = 0;
            int count = CompList.Count;

            for (int i = 0; i < count; i++)
                mw += CompList[i].MoleFraction * CompList[i].MW;

            return mw;
        }

        public double MW(double[] MoleFracs)
        {
            double mw = 0;

            for (int i = 0; i < MoleFracs.Length; i++)
                mw += MoleFracs[i] * CompList[i].MW;

            return mw;
        }

        public double SG(double[] MoleFracs = null)
        {
            int count = CompList.Count;
            double VF = 0, MF = 0;

            if (SGArray is null || SGArray.Length != CompList.Count)
                UpdateArrayData();

            double sg;
            if (MoleFracs is null)
            {
                for (int i = 0; i < count; i++)
                {
                    MF += CompList[i].MoleFraction * MWArray[i];//currentcompositions
                    VF += CompList[i].MoleFraction * MWArray[i] / SGArray[i];//currentcompositions
                }

                sg = MF / VF;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    MF += MoleFracs[i] * MWArray[i];//currentcompositions
                    VF += MoleFracs[i] * MWArray[i] / SGArray[i];//currentcompositions
                }

                sg = MF / VF;
            }

            return sg;
        }
        public double SG()
        {
            return this.SG_Calc();
        }

        public double Density
        {
            get
            {
                return this.SG_Calc() * Extensions.Constants.WaterSpecGravityAt60F;//0.99898
            }
        }

        public double API
        {
            get
            {
                return 141.5 / this.SG() - 131.5; ;
            }
        }

        /// <summary>
        /// Watson K
        /// </summary>
        public double WatsonK()
        {
            double p = 1.0 / 3.0;
            return Math.Pow((MeABP() + 273.13) * 9 / 5 + 32, p) / SG();
        }


    }
}