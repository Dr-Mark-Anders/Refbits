using System;

namespace ModelEngine
{
    public class FlashClass
    {
        private static double errorValue = 1e-10;
        private static double pHErrorValue = 1e-5;
        private static double c_Tolerance = 1e-10;
        private static double k_Tolerance = 1e-10;

        public static double ErrorValue { get => errorValue; set => errorValue = value; }
        public static double PHErrorValue { get => pHErrorValue; set => pHErrorValue = value; }
        public static double C_Tolerance { get => c_Tolerance; set => c_Tolerance = value; }
        public static double K_Tolerance { get => k_Tolerance; set => k_Tolerance = value; }

        public static bool Flash(Port_Material port, ThermoDynamicOptions thermo, Guid originGuid, bool calcderivatives = false)
        {
            enumFlashType flashtype = FlashTypes.FlashType(port);
            return Flash(port, thermo, originGuid, flashtype, calcderivatives);
        }

        /// return null if flash fails
        public static bool Flash(Port_Material port, ThermoDynamicOptions thermo, Guid originGuid,
        enumFlashType flashtype, bool calcderivatives = false)
        {
            bool solved = false;
            OriginalMethods flashOld = new(port);
            Components cc = port.cc;

            switch (thermo.FlashMethod)
            {
                case enumFlashAlgorithm.IO:
                    FlashIO flashio = new(cc, thermo);
                    solved = flashio.Flash_IO(port);
                    break;

                case enumFlashAlgorithm.RR:
                    RachfordRice flashRR = new(port, thermo);
                    if (flashRR.Solve(flashtype))
                        solved = true;
                    else
                        solved = false; // flashOld.Solve(PropsIn, thermo);
                    break;

                default:
                    break;
            }

            return solved;
        }
    }
}