using ModelEngine;
using Units.UOM;

namespace ModelEngine
{
    public class FlashTypes
    {
        public static enumFlashType FlashType(Port_Material proplist)
        {
            enumFlashType flashtype;

            UOMProperty P = proplist.P_;
            UOMProperty Q = proplist.Q_;
            UOMProperty H = proplist.H_;
            UOMProperty S = proplist.S_;
            UOMProperty T = proplist.T_;

            if (P.IsFlashVariable)
            {
                if (H.IsFlashVariable)
                    flashtype = enumFlashType.PH;
                else if (T.IsFlashVariable)
                    flashtype = enumFlashType.PT;
                else if (S.IsFlashVariable)
                    flashtype = enumFlashType.PS;
                else if (Q.IsFlashVariable)
                    flashtype = enumFlashType.PQ;
                else
                    flashtype = enumFlashType.None;

                return flashtype;
            }

            if (T.IsFlashVariable)
            {
                if (H.IsFlashVariable)
                    flashtype = enumFlashType.TH;
                else if (S.IsFlashVariable)
                    flashtype = enumFlashType.TS;
                else if (Q.IsFlashVariable)
                    flashtype = enumFlashType.TQ;
                else
                    flashtype = enumFlashType.None;

                return flashtype;
            }

            if (Q.IsFlashVariable) // Assum if Q is set then Q is override ing T or P
            {
                if (H.IsFlashVariable)
                    flashtype = enumFlashType.HQ;
                else
                    flashtype = enumFlashType.None;

                return flashtype;
            }

            flashtype = enumFlashType.None;

            return flashtype;
        }
    }
}