using ModelEngine;
using Units;

namespace System.Windows.Forms
{
    // Your class  should look like this:
    public class DataGridViewCompCell : DataGridViewTextBoxCell
    {
        public BaseComp comp;
        private eDisplayState CurrentState = eDisplayState.Mole;
        private string displayText = "";
        public SourceEnum origin;
        private string Format = "F4";

        public eDisplayState state
        {
            get
            {
                return CurrentState;
            }
            set
            {
                CurrentState = value;
            }
        }

        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                displayText = value;
                base.Value = value;
            }
        }

        public void CompValueUpdate()
        {
            if (Value != null && double.TryParse(Value.ToString(), out double res))
            {
                switch (CurrentState)
                {
                    case eDisplayState.Mole:
                        comp.MoleFraction = res;
                        break;

                    case eDisplayState.Mass:
                        comp.MassFraction = res;
                        break;

                    case eDisplayState.Vol:
                        comp.STDLiqVolFraction = res;
                        break;

                    default:
                        break;
                }
            }
        }

        public DataGridViewCompCell()
        {
            comp = null;
        }

        public DataGridViewCompCell(BaseComp comp, string format = "F20")
        {
            this.comp = comp;
            this.Format = format;

            switch (CurrentState)
            {
                case eDisplayState.Mole:
                    displayText = this.comp.MoleFraction.ToString(format);
                    break;

                case eDisplayState.Mass:
                    displayText = this.comp.MassFraction.ToString(format);
                    break;

                case eDisplayState.Vol:
                    displayText = this.comp.STDLiqVolFraction.ToString(format);
                    break;

                default:
                    break;
            }
            base.Value = displayText;
        }

        public void ValueUpdate(eDisplayState state)
        {
            CurrentState = state;
            switch (CurrentState)
            {
                case eDisplayState.Mole:
                    displayText = this.comp.MoleFraction.ToString(Format);
                    break;

                case eDisplayState.Mass:
                    displayText = this.comp.MassFraction.ToString(Format);
                    break;

                case eDisplayState.Vol:
                    displayText = this.comp.STDLiqVolFraction.ToString(Format);
                    break;

                default:
                    break;
            }
            base.Value = displayText;
        }
    }
}