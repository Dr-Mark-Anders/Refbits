﻿namespace Steam97
{
    public struct STATE
    {
        public double t;  // t/tStar
        public double p;  // p/pStar
        public int region;
        public double x;  // quality
    }

    public partial class StmPropIAPWS97
    {
        public STATE state;
        public REGION1 Region1;
        public REGION2 Region2;
        public REGION3 Region3;
        public REGION5 Region5;
        public TRANSPORT Transport;

        public int errorCondition;
        public string errorMessage;

        // critical properties in reduced units
        private const double Tc1 = 647.096;       // Tc/tStar

        private const double Pc1 = 22064.0;       // Pc/pStar

        // minimum values on the saturation line (triple point)
        private const double P3p = 0.611657;

        private const double T3p = 273.16;

        private const int LIQ_FLAG = 1;
        private const int STM_FLAG = 2;

        public StmPropIAPWS97()
        {
            state = new STATE();
            Region1 = new REGION1();
            Region2 = new REGION2();
            Region3 = new REGION3();
            Region5 = new REGION5();
            Transport = new TRANSPORT();

            Region3.daddy = this;
            Transport.daddy = this;

            errorCondition = 0;
            errorMessage = "Normal Termination";
        }

        public void AddErrorMessage(string errMsg)
        {
            if (errorMessage == "")
                errorMessage = errMsg;
            else
                errorMessage += "\r\n" + errMsg;
        }

        private void ClearErrors()
        {
            errorMessage = "";
            errorCondition = 0;
        }

        private void ClearState()
        {
            state.t = 0.0;
            state.p = 0.0;
            state.region = -1;
            state.x = double.MinValue;
        }
    }
}