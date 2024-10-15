using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class RefomerShortCompList : SimpleCompList, ISerializable
    {
        private double[] mw, sg, BP, TCrit, PCrit, VCrit, AcentricFactor, Density, RON, MON, HFORM, NBP;

        public RefomerShortCompList()
        {
            Names = new List<string>() {
            "IC4"," NC4", "C4O", "DUMMY1", "DUMMY2","DUMMY3",
            "IC5", "NC5", "5CO", "5C5",  "DUMMY4",
            "IC6", "NC6", "6CO", "5C6",  "6C6", "BEN",
            "IC7", "NC7", "7CO", "5C7",  "6C7", "TOL",
            "IC8", "NC8", "8CO", "5C8",  "6C8", "C8A",
            "IC9", "NC9", "9CO", "5C9",  "6C9", "C9A",
            "IC10","NC10","10CO","5C10", "6C10", "C10A",
            "IC11","NC11","11CO","5C11", "6C11", "C11A",
            "IC12","NC12","12CO","5C12", "6C12", "C12A",
            "IC13","NC13","13CO","5C13", "6C13", "C13A",
            "PX","MX","OX"};

            mw = new double[] { 58.12, 58.12, 56.11, double.NaN, double.NaN, double.NaN, 72.15, 72.15, 70.13, 70.13, double.NaN, double.NaN, 86.18, 86.18, 84.16, 84.16, 84.16, 78.11, 100.2, 100.2, 98.19, 98.19, 98.19, 92.14, 114.23, 114.23, 112.22, 112.22, 112.22, 106.17, 128.26, 128.26, double.NaN, 126.24, 126.24, 120.19, 142.29, 142.29, double.NaN, 140.27, 140.27, 132.21, 156.31, 156.31, double.NaN, 154.3, 154.3, 148.25, 170.34, double.NaN, double.NaN, double.NaN, 168.32, 162.28, 184.37, double.NaN, double.NaN, 196.38, double.NaN, 162.28 };
            BP = new double[] { -11.72, -0.5, -6.24, double.NaN, double.NaN, double.NaN, 27.84, 36.07, 29.96, 49.25, double.NaN, double.NaN, 57.98, 68.73, 46.72, 71.81, 80.72, 80.09, 89.78, 98.43, 78.56, 87.85, 87.48, 110.63, 104.62, 125.68, 120.42, 126.12, 118.6, 138.36, 137.6, 150.82, double.NaN, 154.58, 156.75, 148.32, 165.45, 174.16, double.NaN, 177.57, 180.98, 182.14, 194.3, 195.93, double.NaN, 187.99, 192.67, 200.69, 214.84, double.NaN, double.NaN, double.NaN, 211.93, 207.98, 234.12, double.NaN, double.NaN, 246.26, double.NaN, 207.98 };
            PCrit = new double[] { 36.17, 37.68, 39.96, double.NaN, double.NaN, double.NaN, 33.45, 33.32, 34.95, 44.88, double.NaN, double.NaN, 30.85, 29.68, 30.99, 37.56, 40.52, 48.91, 28.62, 26.86, 27.82, 34.1, 34.1, 40.86, 25.79, 24.32, 24.97, 29.56, 28.93, 35.08, 22.83, 22.48, double.NaN, 27.59, 27.59, 31.69, 20.61, 20.61, double.NaN, 25.17, 25.17, 28.4, 19.01, 19.01, double.NaN, 19.67, 19.67, 25.52, 17.56, double.NaN, double.NaN, double.NaN, 18.24, 23.95, 16.53, double.NaN, double.NaN, 15.89, double.NaN, 23.95 };
            VCrit = new double[] { 0.25, 0.24, double.NaN, double.NaN, double.NaN, 0.31, 0.31, 0.3, 0.26, double.NaN, double.NaN, 0.36, 0.37, 0.35, 0.32, 0.31, 0.26, 0.39, 0.43, 0.41, 0.36, 0.36, 0.32, 0.47, 0.49, 0.47, 0.43, 0.45, 0.38, 0.53, 0.55, double.NaN, 0.48, 0.48, 0.43, 0.6, 0.6, double.NaN, 0.53, 0.53, 0.5, 0.66, 0.66, double.NaN, 0.64, 0.64, 0.55, 0.73, double.NaN, double.NaN, double.NaN, 0.7, 0.6, 0.77, double.NaN, double.NaN, 0.82, double.NaN, 0.6, double.NaN };
            AcentricFactor = new double[] { 0.18, 0.2, 0.19, double.NaN, double.NaN, double.NaN, 0.23, 0.25, 0.23, 0.19, double.NaN, double.NaN, 0.25, 0.3, 0.28, 0.23, 0.21, 0.21, 0.29, 0.35, 0.33, 0.27, 0.27, 0.26, 0.35, 0.4, 0.37, 0.27, 0.23, 0.33, 0.41, 0.44, double.NaN, 0.26, 0.26, 0.34, 0.48, 0.48, double.NaN, 0.27, 0.27, 0.39, 0.54, 0.54, double.NaN, 0.52, 0.52, 0.49, 0.57, double.NaN, double.NaN, double.NaN, 0.56, 0.39, 0.62, double.NaN, double.NaN, 0.65, double.NaN, 0.39 };
            Density = new double[] { 564.08, 584.23, 600.18, double.NaN, double.NaN, double.NaN, 626.12, 631.02, 645.29, 759.88, double.NaN, double.NaN, 666.68, 665.08, 676.59, 753.57, 781.87, 882.63, 699.01, 689.83, 701.15, 757.34, 749.17, 873.86, 715.83, 706.25, 718.97, 780.65, 784.94, 868.92, 724.11, 722.78, double.NaN, 797.72, 797.72, 867.99, 735.68, 735.68, double.NaN, 802.92, 802.92, 852.51, 744.1, 744.1, double.NaN, 753.7, 753.7, 861.99, 751.89, double.NaN, double.NaN, double.NaN, 762.57, 860.17, 761.33, double.NaN, double.NaN, 774.45, double.NaN, 860.17 };
            RON = new double[] { 101.43, 93.8, 97.4, double.NaN, double.NaN, double.NaN, 92.3, 61.7, 87.9, 101.43, double.NaN, double.NaN, 100, 24.8, 76.4, 91.3, 83, 106, 91.1, 0, 54.5, 92.3, 92.3, 120.08, 76.3, 0, 28.7, 81.1, 80.9, 116.4, 84, 0, double.NaN, 33.4, 70.1, 113.32, 86.4, 0, double.NaN, 33.4, 70.1, 110.32, 86.4, 0, double.NaN, 33.4, 70.1, 110.32, 86.4, double.NaN, double.NaN, double.NaN, 33.4, 110.32, 86.4, double.NaN, double.NaN, 33.4, double.NaN, 110.32 };
            MON = new double[] { 97.6, 89.6, 80.8, double.NaN, double.NaN, double.NaN, 90.3, 62.6, 77.1, 84.9, double.NaN, double.NaN, 94.3, 26, 63.4, 80, 77.2, 101, 88.5, 0, 50.7, 89.3, 89.3, 103.52, 81.7, 0, 34.7, 86.2, 78.7, 109.6, 91.6, 0, double.NaN, 28.2, 74.3, 98, 100, 0, double.NaN, 28.2, 74.3, 92, 100, 0, double.NaN, 28.2, 74.3, 92, 100, double.NaN, double.NaN, double.NaN, 28.2, 92, 100, double.NaN, double.NaN, 28.2, double.NaN, 92 };
            HFORM = new double[] { -134180, -125650, -540, double.NaN, double.NaN, double.NaN, -152970, -146710, -20920, -77027.4, double.NaN, double.NaN, -176800, -166940, -41672.6, -106692, -123135, 82926.9, -194100, -187650, -62299.8, -138280, -135850, 49998.8, -213800, -208820, -82926.9, -148072, -181000, 17238.1, -233700, -228865, double.NaN, -193301, -193301, 3933, -249534, -249534, double.NaN, -213175, -213175, -13140, -270286, -270286, double.NaN, -145300, -145300, -33810, -290872, double.NaN, double.NaN, double.NaN, -165420, -77600, -311499, double.NaN, double.NaN, -207310, double.NaN, -77600 };
            sg = new double[] { 0.564, 0.5842, 0.6001, double.NaN, double.NaN, double.NaN, 0.6261, 0.631, 0.6452, 0.7598, double.NaN, double.NaN, 0.6666, 0.665, 0.6765, 0.7535, 0.7818, 0.8826, 0.699, 0.6898, 0.7011, 0.7573, 0.7491, 0.8738, 0.7158, 0.7062, 0.7189, 0.7806, 0.7849, 0.8689, 0.7241, 0.7227, double.NaN, 0.7977, 0.7977, 0.8679, 0.7356, 0.7356, double.NaN, 0.8029, 0.8029, 0.8525, 0.7441, 0.7441, double.NaN, 0.7537, 0.7537, 0.8619, 0.7518, double.NaN, double.NaN, double.NaN, 0.7625, 0.8601, 0.7613, double.NaN, double.NaN, 0.7744, double.NaN, 0.8601 };
            NBP = new double[] { 261.43, 272.65, 266.91, double.NaN, double.NaN, double.NaN, 300.99, 309.22, 303.11, 322.4, double.NaN, double.NaN, 331.13, 341.88, 319.87, 344.96, 353.87, 353.24, 362.93, 371.58, 351.71, 361, 360.63, 383.78, 377.77, 398.83, 393.57, 399.27, 391.75, 411.51, 410.75, 423.97, double.NaN, 427.73, 429.9, 421.47, 438.6, 447.31, double.NaN, 450.72, 454.13, 455.29, 467.45, 469.08, double.NaN, 461.14, 465.82, 473.84, 487.99, double.NaN, double.NaN, double.NaN, 485.08, 481.13, 507.27, double.NaN, double.NaN, 519.41, double.NaN, 481.13 };
            TCrit = new double[] { 408.14, 425.18, 419.59, double.NaN, double.NaN, double.NaN, 460.43, 469.65, 464.78, 511.76, double.NaN, double.NaN, 499.98, 507.43, 504.03, 532.79, 553.54, 562.16, 537.35, 540.26, 537.29, 547, 551, 591.79, 563.4, 568.83, 566.6, 603, 591.15, 617.05, 590.15, 595.65, double.NaN, 639.15, 639.15, 631.15, 618.45, 618.45, double.NaN, 667, 667, 660.55, 638.76, 638.76, double.NaN, 638, 638, 672.9, 658.2, double.NaN, double.NaN, double.NaN, 657, 689, 675.8, double.NaN, double.NaN, 692, double.NaN, 689 };

            Components.Clear();

            for (int i = 0; i < 60; i++)
            {
                Components.Add(new PseudoComponent(Names[i], NBP[i], sg[i], mw[i], AcentricFactor[i], TCrit[i], PCrit[i], VCrit[i], HFORM[i]));
            }

            Components.Add(new PseudoComponent("EB", 409.35, 0.8739, 106.17, 0.3, 617.17, 35.77, 0.3, 29790.1));
            Components.Add(new PseudoComponent("PX", 411.51, 0.866, 106.17, 0.33, 616.26, 34.77, 0.33, 18030));
            Components.Add(new PseudoComponent("MX", 412.27, 0.8689, 106.17, 0.33, 617.05, 35.08, 0.33, 17238.1));
            Components.Add(new PseudoComponent("OX", 417.58, 0.8844, 106.17, 0.31, 630.37, 37.05, 0.31, 18995.3));
        }

        public RefomerShortCompList Clone()
        {
            RefomerShortCompList res = new RefomerShortCompList();
            for (int i = 0; i < Components.Count; i++)
            {
                res.Components[i].MoleFraction = Components[i].MoleFraction;
            }
            return res;
        }

        public RefomerShortCompList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}