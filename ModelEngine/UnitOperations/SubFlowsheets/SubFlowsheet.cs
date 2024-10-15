using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class SubFlowSheet : FlowSheet
    {
        public SubFlowSheet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Name = "SubFlowSheet";
        }

        public SubFlowSheet() : base()
        {
            this.Name = "SubFlowSheet";
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public virtual void ResetSpecs()
        {
        }

        /*  internal void CopyStreamPortData(bool OverrideInputs = false)
          {
              foreach (Port p in Ports.AllPorts)
              {
                  if (p.IsConnected)
                  {
                      switch (p) // Transfer data both directions.
                      {
                          case Port_Energy ps:
                              break;

                          case Port_Signal ps:
                              break;

                          case Port_Material pm:
                              if(pm.Owner is StreamMaterial sm)
                              {
                              }
                              break;
                      }
                  }
              }
          }*/

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