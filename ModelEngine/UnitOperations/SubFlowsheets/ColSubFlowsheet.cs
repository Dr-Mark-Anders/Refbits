using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public partial class COlSubFlowSheet : FlowSheet
    {
        public COlSubFlowSheet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Name = "SubFlowSheet";
            TriggerPorts.PortListChangedEvent += SubFlowSheet_SFSChanged;
        }

        private void SubFlowSheet_SFSChanged(object sender, Ports.Events.PropertyEventArgs e)
        {
            RaiseSFSChangedEvent(sender, e);
        }

        public COlSubFlowSheet() : base()
        {
            this.Name = "SubFlowSheet";
            TriggerPorts.PortListChangedEvent += SubFlowSheet_SFSChanged;
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public virtual void ResetSpecs()
        {
        }

        public override bool IsSolved
        {
            get
            {
                foreach (UnitOperation uo in ModelStack)
                {
                    if (!uo.IsSolved)
                        return false;
                }
                return true;
            }
        }

        public override bool IsDirty
        {
            get
            {
                foreach (UnitOperation uo in ModelStack)
                {
                    if (uo.IsDirty)
                        return true;
                }
                return false;
            }
        }
    }
}