using ModelEngine.Ports.Events;
using System;

namespace ModelEngine
{
    public partial class Port
    {
        public Func<UnitOperation> OnRequestParent;

        protected UnitOperation RequestParent()
        {
            if (OnRequestParent == null)
                return null;

            return OnRequestParent();
        }

        public void ClearChangeEventHandlers()
        {
            if (PropertyChangedHandler != null)
                foreach (Delegate d in PropertyChangedHandler.GetInvocationList())
                {
                    PropertyChangedHandler -= (PropertyChangedEventHandler)d;
                }
        }

        // event happens in streamport, need to fire event in unitop port


        public delegate void PropertyChangedEventHandler(object sender, PropertyEventArgs e);
        public event PropertyChangedEventHandler PropertyChangedHandler;

        public virtual void RaiseStreamValueChangedEvent(object sender, PropertyEventArgs e)
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            if(PropertyChangedHandler != null)
                foreach (PropertyChangedEventHandler item in PropertyChangedHandler.GetInvocationList())
                {
                    item?.Invoke(this, e);
                }
        }

        internal void PropertyChanged(object sender, PropertyEventArgs e) // fire up to unitop
        {
            this.isDirty = true;
            RaisePortPropertyChangedEvent(this, e);
        }



        public delegate void PortPropertyChangedEventHandler(object sender, PropertyEventArgs e);
        public event PortPropertyChangedEventHandler PortPropertyChanged;

        internal virtual void RaisePortPropertyChangedEvent(object sender, PropertyEventArgs e)
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            PortPropertyChanged?.Invoke(this, e);
        }




        public delegate void PortCompositionChangedEvent(object sender, CompositionEventArgs e);
        public event PortCompositionChangedEvent PortCompositionChangedHandler;

        public virtual void RaiseCompositionChanged(object sender, CompositionEventArgs e)
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            PortCompositionChangedHandler?.Invoke(this, e);
        }
    }
}