using System;
using Units;

namespace ModelEngine
{
    public class PropertyAttribute : Attribute
    {
        private ePropID prop;
        private bool isspecified;

        public bool IsSpecified
        {
            get { return isspecified; }
            set { isspecified = value; }
        }

        public PropertyAttribute(ePropID prop)
        {
            this.prop = prop;
        }

        public ePropID Property
        {
            get
            {
                return this.prop;
            }
            set
            {
                this.prop = value;
            }
        }
    }
}