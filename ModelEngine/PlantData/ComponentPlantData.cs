using ModelEngine;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using Units.UOM;

namespace ModelEngine
{
    public class ComponentPlantData: BasePlantData
    {
        //private Port_Material port = new();

        private GasFlow flow;
        private LiqGasFlow liqflow;

        public ComponentPlantData(List<string> gascomps, List<double> MoleFracs, GasFlow flow)
        {
            if(MoleFracs.Count != gascomps.Count)
                MessageBox.Show("Comps do not equal Mol fracs");

            for (int i = 0; i < gascomps.Count; i++)
            {
                BaseComp bc = Thermodata.GetComponent(gascomps[i]);
                bc.MoleFraction = MoleFracs[i];
                Port.cc.Add(bc);
            }

            Port.NormaliseFractions(FlowFlag.Molar);

            this.flow = flow;
            double MW = Port.cc.MW();

            Port.MolarFlow_.BaseValue = flow.MoleFlow(MW);
        }

        public ComponentPlantData(List<string> gascomps, List<double> Fracs, LiqGasFlow flow)
        {
            for (int i = 0; i < gascomps.Count; i++)
            {
                BaseComp bc = Thermodata.GetComponent(gascomps[i]);
                bc.STDLiqVolFraction = Fracs[i];
                Port.cc.Add(bc);
            }

            Port.NormaliseFractions(FlowFlag.LiqVol);
            double MW = Port.cc.MW();
            Port.MolarFlow_.BaseValue = Port.cc.SG_Calc() / MW;

            this.liqflow = flow;
        }

        public MoleFlow MoleFlow
        {
            get
            {
                if (!double.IsNaN(flow))
                    return Port.MolarFlow_.BaseValue;
                else if (!double.IsNaN(liqflow))
                    return Port.MolarFlow_.BaseValue;
                else
                    return new MoleFlow(double.NaN);
            }
        }

        public Components Comps { get => Port.cc; set => Port.cc = value; }

        public enumMassMolarOrVol GteFlowType()
        {
            if (double.IsNaN(flow.BaseValue))
                return enumMassMolarOrVol.Mass;

            return enumMassMolarOrVol.notdefined;
        }
    }
}