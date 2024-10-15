﻿using ModelEngine;
using System;
using System.Collections.Generic;
using Units.UOM;

namespace NaphthaReformerSI
{
    public partial class NapReformerSI
    {
        public Dictionary<string, int> NameDict = new Dictionary<string, int>();
        private Tuple<double, double, double, double> PNAO;

        public void InitialiseComponents()
        {
            FeedDistillation.Add(new DistPoint(1, new Temperature(102, TemperatureUnit.Celsius)));
            FeedDistillation.Add(new DistPoint(5, new Temperature(114, TemperatureUnit.Celsius)));
            FeedDistillation.Add(new DistPoint(50, new Temperature(132, TemperatureUnit.Celsius)));
            FeedDistillation.Add(new DistPoint(90, new Temperature(160, TemperatureUnit.Celsius)));
            FeedDistillation.Add(new DistPoint(99, new Temperature(178, TemperatureUnit.Celsius)));

            PNAO = Tuple.Create<double, double, double, double>(60.87, 28.34, 10.79, 0.0);

            string[] Names = new string[] {"H2","C1","C2","C3","IC4","NC4","IC5","NC5","IC6","NC6","CH","MCP"
                ,"BEN","IC7","NC7","MCH","C7CP","TOL","C8P","ECH","DMCH","PCP","C8CP","EB","PX","MX","OX","C9+P","C9+CH","C9+CP","C9+A"};

            double[] SG = new double[] { 0.135,0.3,0.3564,0.5077,0.5631,0.5844,0.6247,0.631,0.6609,0.664,0.7834,0.7536,0.8844,0.6906,
            0.6882,0.774,0.7595,0.8718,0.7089,0.7922,0.779,0.7807,0.7713,0.8718,0.8657,0.8687,0.8848,0.7272,
            0.79,0.7825,0.8769,0.7272,0.7863,0.8769,0.7408,0.7946,0.8837,0.7511,0.8014,0.8919,0.76,0.8074};

            double[] NBP = new double[] {36.77,200.97,332.17,415.97,470.57,490.77,541.77,556.57,597.47,615.37,636.97,620.97,635.87,652.17,668.87,673.37,661.27,
            690.77,702.07,728.87,711.57,727.37,703.37,736.77,740.67,742.07,751.57,749.67,759.67,744.67,790.67 };

            double[] TCrit = new double[] {59.742,343.08,549.54,665.64,734.58,765.18,829.8,845.46,900.9,913.68,996.48,959.04,1011.96, 956.0,972.36,1029.96, 990.3,1065.24,
                1024.9,1054.1,1054.1,1044.9,1044.9,1115.8,1113.1,1114.9,1138.3,1055.8,1118.8,1108.8,1146.7};

            double[] PCrit = new double[] {190.8,673.1,709.8,617.4,529.1,550.7,483.0,489.5,449.5, 440.0, 561.0, 549.0, 714.0, 414.2, 396.8, 504.4,
            497.8, 590.0, 362.1, 468.1, 468.1, 448.4, 448.4, 540.0, 500.0, 510.0, 530.0, 347.0, 418.1, 412.1, 492.2};

            double[] W = new double[] {0.0,0.0,0.1064,0.1538,0.1825,0.1953,0.2104,0.2387,0.2861,0.2972,0.2032,0.2346,0.213,0.3427,0.3403,0.2421,
            0.3074,0.2591,0.3992,0.3558,0.3558,0.364,0.364,0.2936,0.2969,0.3045,0.2904,0.4406,0.4124,0.4164,0.3799};

            double[] MolVOl = new double[]{31.0, 52.0, 68.0, 84.0, 105.5, 101.4, 117.4, 116.1, 131.449, 131.6, 108.7, 113.1, 89.4, 147.097, 147.5, 128.3, 129.71, 106.8, 163.5, 143.532, 143.532,
            146.916, 146.916, 123.1, 124.0, 123.5, 121.2, 180.094, 158.936, 160.936, 140.22 };

            double[] Sol = new double[]{3.25,5.68,6.05,6.4,6.73,6.73,7.021,7.021,7.1561,7.266,8.196,7.849,9.158,7.2872,7.43,
            7.826,7.7851,8.915,7.551,7.9251,7.9251,7.826,7.826,8.787,8.769,8.818,8.987,7.433,8.3351,7.8351,8.5676};

            double[] RON = new double[] {0.0,0.0,0.0,0.0,100.2,95.0,92.704,61.7,82.742,31.0,100.0,96.0,120.0,66.879,0.0,73.8,78.994,
            110.0,58.0,46.5,72.547,31.2,65.0,124.0,146.0,145.0,120.0,56.667,43.77,0.0,110.7};

            double[] MON = new double[] {0.0,0.0,0.0,0.0,97.6,93.5,88.575,61.3,83.314,30.0,77.2,85.0,114.8,69.675,0.0,
            71.1,72.923,102.0,61.0,40.8,70.087,28.1,63.0,107.0,127.0,124.0,103.0,64.686,41.641,0.0,101.75};

            double[] RVP = new double[] {0.0,0.0,0.0,90.0,72.2,51.6,20.4,15.6,7.0,5.0,2.3,4.5,3.2,2.3,1.6,1.6,2.0,
            1.0,0.7,0.5,0.7,0.7,0.7,0.4,0.3,0.3,0.3,0.3,0.2,0.3,0.1};

            double[] HFORM25 = new double[] {0.0,-32200.2,-36424.8,-44676.0,-57870.0,-54270.0,-66456.0,-63000.0,-75778.2,-71928.0,-52974.0,-45900.0,35676.0,
            -84576.6,-80802.0,-66582.0,-57498.3,21510.0,-92296.8,-73890.0,-77823.0,-63702.0,-68916.6,12816.0,7722.0,
            7416.0,8172.0,-101057.4,-88524.0,-79380.0,-3681.0};

            double[] MW = new double[] {2.016,16.043,30.07,44.097,58.124,58.124,72.151,72.151,86.178,86.178,84.162,84.162,78.114,100.205,100.205,
            98.189,98.189,92.141,114.232,112.216,112.216,112.216,112.216,106.168,106.168,106.168,106.168,128.259,126.243,
            126.243,120.195,128.259,126.243,120.195,142.286,140.27,134.222,156.312,154.296,148.248,170.338,168.322};

            double[][] Cp = new double[31][];
            Cp[0] = new double[] { 6.3539, 0.167945, -0.014925, 0.00046677 };
            Cp[1] = new double[] { 6.367, 0.102427, 0.067468, -0.00224337 };
            Cp[2] = new double[] { 2.3112, 2.01391, -0.015322, -0.00076208 };
            Cp[3] = new double[] { -1.0719, 4.03765, -0.11203, 0.0010955 };
            Cp[4] = new double[] { -2.4505, 5.6504, -0.177432, 0.00214335 };
            Cp[5] = new double[] { -0.1302, 5.01947, -0.128858, 0.0009526 };
            Cp[6] = new double[] { -2.28095, 6.692725, -0.194371, 0.00200046 };
            Cp[7] = new double[] { -0.4145, 6.28296, -0.167989, 0.00138127 };
            Cp[8] = new double[] { -5.9217, 9.1914463, -0.357933, 0.00612045 };
            Cp[9] = new double[] { -0.7017, 7.56254387, -0.209619, 0.0019052 };
            Cp[10] = new double[] { -11.9067, 7.71600485, -0.139477, -0.00114312 };
            Cp[11] = new double[] { -11.2874, 8.19076347, -0.23556, 0.00209572 };
            Cp[12] = new double[] { -10.52, 6.9534831, -0.277153, 0.00433432 };
            Cp[13] = new double[] { -0.955, 8.828329, -0.249743, 0.0023815 };
            Cp[14] = new double[] { -0.995, 8.828329, -0.249743, 0.0023815 };
            Cp[15] = new double[] { -12.8626, 9.70844364, -0.251213, 0.00133364 };
            Cp[16] = new double[] { -13.043, 9.98848057, -0.316836, 0.00359606 };
            Cp[17] = new double[] { -9.1226, 7.66801119, -0.272413, 0.0036675 };
            Cp[18] = new double[] { -1.2219, 10.096693, -0.289976, 0.0028578 };
            Cp[19] = new double[] { -13.5338, 11.1712523, -0.30787, 0.00223861 };
            Cp[20] = new double[] { -13.3602, 10.9259481, -0.27381, 0.00109549 };
            Cp[21] = new double[] { -15.94786, 12.1267424, -0.457856, 0.00752553 };
            Cp[22] = new double[] { -15.94786, 12.1267424, -0.457856, 0.00752553 };
            Cp[23] = new double[] { -10.2083, 9.336, -0.348471, 0.0050964 };
            Cp[24] = new double[] { -5.38286, 7.781393, -0.22259, 0.0019052 };
            Cp[25] = new double[] { -6.5457, 8.188448, -0.257013, 0.00281017 };
            Cp[26] = new double[] { -3.38786, 7.75355, -0.236148, 0.00247676 };
            Cp[27] = new double[] { -1.40476, 11.34312, -0.3284465, 0.00328647 };
            Cp[28] = new double[] { -13.8138, 12.6579, -0.370591, 0.00323884 };
            Cp[29] = new double[] { -16.0852, 13.35798, -0.494782, 0.00790657 };
            Cp[30] = new double[] { -9.1543, 10.2822752, -0.361993, 0.00485825 };

            Feed.Clear();

            for (int i = 0; i < Names.Length; i++)
            {
                Feed.Add(new PseudoComponent(Names[i], NBP[i], SG[i], MW[i], W[i], TCrit[i], PCrit[i], MolVOl[i], HFORM25[i], Cp[i]));
            }

            NameDict.Clear();

            for (int i = 1; i < Names.Length; i++)
            {
                NameDict.Add(Names[i], i);
            }
        }
    }
}