using System;
using System.Collections.Generic;

namespace DictonaryValueChanged
{
    public class ExtendedDictonary<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>
    {
        public Dictionary<Tkey, Tvalue> Items = new();

        public ExtendedDictonary() : base()
        {
        }

        private ExtendedDictonary(int capacity) : base(capacity)
        {
        }

        //
        // Do all your implementations here...
        //

        public event EventHandler ValueChanged;

        public void OnValueChanged(Object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddItem(Tkey key, Tvalue value)
        {
            try
            {
                Items.Add(key, value);
                OnValueChanged(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // Similarly Do implementation for Update , Delete and Value Changed checks (Note: Value change can be monitored using   a thread)
        //
    }
}