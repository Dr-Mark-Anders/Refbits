﻿using System;

namespace ModelEngine.ThermodynamicMethods.UNIFAC
{
    public class VLEUnifacData : BaseUnifacData
    {
        /// <summary>
        /// R, Q and Primary Group
        /// </summary>
        public VLEUnifacData()
        {
            GroupList.Add("CH3", Tuple.Create(0.9011, 0.848, 1, 1, 1));
            GroupList.Add("CH2", Tuple.Create(0.6744, 0.54, 2, 2, 1));
            GroupList.Add("CH", Tuple.Create(0.4469, 0.228, 3, 3, 1));
            GroupList.Add("C", Tuple.Create(0.2195, 0D, 4, 4, 1));
            GroupList.Add("CH2=CH", Tuple.Create(1.3454, 1.176, 5, 5, 2));
            GroupList.Add("CH=CH", Tuple.Create(1.1167, 0.867, 6, 6, 2));
            GroupList.Add("CH2=C", Tuple.Create(1.1173, 0.988, 7, 7, 2));
            GroupList.Add("CH=C", Tuple.Create(0.8886, 0.676, 8, 8, 2));
            GroupList.Add("C=C", Tuple.Create(0.6605, 0.485, 9, 70, 2));
            GroupList.Add("ACH", Tuple.Create(0.5313, 0.4, 10, 9, 3));
            GroupList.Add("AC", Tuple.Create(0.3652, 0.12, 11, 10, 3));
            GroupList.Add("ACCH3", Tuple.Create(1.2663, 0.968, 12, 11, 4));
            GroupList.Add("ACCH2", Tuple.Create(1.0396, 0.66, 13, 12, 4));
            GroupList.Add("ACCH", Tuple.Create(0.8121, 0.348, 14, 13, 4));
            GroupList.Add("OH", Tuple.Create(1D, 1.2, 15, 14, 5));
            GroupList.Add("CH3OH", Tuple.Create(1.4311, 1.432, 16, 15, 6));
            GroupList.Add("H2O", Tuple.Create(0.92, 1.4, 17, 16, 7));
            GroupList.Add("ACOH", Tuple.Create(0.8952, 0.68, 18, 17, 8));
            GroupList.Add("CH3CO", Tuple.Create(1.6724, 1.488, 19, 18, 9));
            GroupList.Add("CH2CO", Tuple.Create(1.4457, 1.18, 20, 19, 9));
            GroupList.Add("CHO", Tuple.Create(0.998, 0.948, 21, 20, 10));
            GroupList.Add("CH3COO", Tuple.Create(1.9031, 1.728, 22, 21, 11));
            GroupList.Add("CH2COO", Tuple.Create(1.6764, 1.42, 23, 22, 11));
            GroupList.Add("HCOO", Tuple.Create(1.242, 1.188, 24, 23, 12));
            GroupList.Add("CH3O", Tuple.Create(1.145, 1.088, 25, 24, 13));
            GroupList.Add("CH2O", Tuple.Create(0.9183, 0.78, 26, 25, 13));
            GroupList.Add("CH-O", Tuple.Create(0.6908, 0.468, 27, 26, 13));
            GroupList.Add("FCH2O", Tuple.Create(0.9183, 1.1, 28, 27, 13));
            GroupList.Add("CH3NH2", Tuple.Create(1.5959, 1.544, 29, 28, 14));
            GroupList.Add("CH2NH2", Tuple.Create(1.3692, 1.236, 30, 29, 14));
            GroupList.Add("CHNH2", Tuple.Create(1.1417, 0.924, 31, 30, 14));
            GroupList.Add("CH3NH", Tuple.Create(1.4337, 1.244, 32, 31, 15));
            GroupList.Add("CH2NH", Tuple.Create(1.207, 0.936, 33, 32, 15));
            GroupList.Add("CHNH", Tuple.Create(0.9795, 0.624, 34, 33, 15));
            GroupList.Add("CH3N", Tuple.Create(1.1865, 0.94, 35, 34, 16));
            GroupList.Add("CH2N", Tuple.Create(0.9597, 0.632, 36, 35, 16));
            GroupList.Add("ACNH2", Tuple.Create(1.06, 0.816, 37, 36, 17));
            GroupList.Add("C5H5N", Tuple.Create(2.9993, 2.113, 38, 37, 18));
            GroupList.Add("C5H4N", Tuple.Create(2.8332, 1.833, 39, 38, 18));
            GroupList.Add("C5H3N", Tuple.Create(2.667, 1.553, 40, 39, 18));
            GroupList.Add("CH3CN", Tuple.Create(1.8701, 1.724, 41, 40, 19));
            GroupList.Add("CH2CN", Tuple.Create(1.6434, 1.416, 42, 41, 19));
            GroupList.Add("COOH", Tuple.Create(1.3013, 1.224, 43, 42, 20));
            GroupList.Add("HCOOH", Tuple.Create(1.528, 1.532, 44, 43, 20));
            GroupList.Add("CH2Cl", Tuple.Create(1.4654, 1.264, 45, 44, 21));
            GroupList.Add("CHCl", Tuple.Create(1.238, 0.952, 46, 45, 21));
            GroupList.Add("CCl", Tuple.Create(1.006, 0.724, 47, 46, 21));
            GroupList.Add("CH2Cl2", Tuple.Create(2.2564, 1.988, 48, 47, 22));
            GroupList.Add("CHCl2", Tuple.Create(2.0606, 1.684, 49, 48, 22));
            GroupList.Add("CCl2", Tuple.Create(1.8016, 1.448, 50, 49, 22));
            GroupList.Add("CHCl3", Tuple.Create(2.87, 2.41, 51, 50, 23));
            GroupList.Add("CCl3", Tuple.Create(2.6401, 2.184, 52, 51, 23));
            GroupList.Add("CCl4", Tuple.Create(3.39, 2.91, 53, 52, 24));
            GroupList.Add("ACCl", Tuple.Create(1.1562, 0.844, 54, 53, 25));
            GroupList.Add("CH3NO2", Tuple.Create(2.0086, 1.868, 55, 54, 26));
            GroupList.Add("CH2NO2", Tuple.Create(1.7818, 1.56, 56, 55, 26));
            GroupList.Add("CHNO2", Tuple.Create(1.5544, 1.248, 57, 56, 26));
            GroupList.Add("ACNO2", Tuple.Create(1.4199, 1.104, 58, 57, 27));
            GroupList.Add("CS2", Tuple.Create(2.507, 1.65, 59, 58, 28));
            GroupList.Add("CH3SH", Tuple.Create(1.877, 1.676, 60, 59, 29));
            GroupList.Add("CH2SH", Tuple.Create(1.651, 1.368, 61, 60, 29));
            GroupList.Add("furfural", Tuple.Create(3.168, 2.481, 62, 61, 30));
            GroupList.Add("(CH2OH)2", Tuple.Create(2.4088, 2.248, 63, 62, 31));
            GroupList.Add("I", Tuple.Create(1.264, 0.992, 64, 63, 32));
            GroupList.Add("Br", Tuple.Create(0.9492, 0.832, 65, 64, 33));
            GroupList.Add("CH=-C", Tuple.Create(1.2929, 1.088, 66, 65, 34));
            GroupList.Add("C=-C", Tuple.Create(1.0613, 0.784, 67, 66, 34));
            GroupList.Add("DMSO", Tuple.Create(2.8266, 2.472, 68, 67, 35));
            GroupList.Add("ACRY", Tuple.Create(2.3144, 2.052, 69, 68, 36));
            GroupList.Add("Cl-(C=C)", Tuple.Create(0.791, 0.724, 70, 69, 37));
            GroupList.Add("ACF", Tuple.Create(0.6948, 0.524, 71, 71, 38));
            GroupList.Add("DMF-1", Tuple.Create(3.0856, 2.736, 72, 72, 39));
            GroupList.Add("DMF-2", Tuple.Create(2.6322, 2.12, 73, 73, 39));
            GroupList.Add("CF3", Tuple.Create(1.406, 1.38, 74, 74, 40));
            GroupList.Add("CF2", Tuple.Create(1.0105, 0.92, 75, 75, 40));
            GroupList.Add("CF", Tuple.Create(0.615, 0.46, 76, 76, 40));
            GroupList.Add("COO", Tuple.Create(1.38, 1.2, 77, 77, 41));
            GroupList.Add("SiH3", Tuple.Create(1.6035, 1.263, 78, 78, 42));
            GroupList.Add("SiH2", Tuple.Create(1.4443, 1.006, 79, 79, 42));
            GroupList.Add("SiH", Tuple.Create(1.2851, 0.749, 80, 80, 42));
            GroupList.Add("Si", Tuple.Create(1.047, 0.41, 81, 81, 42));
            GroupList.Add("SiH2O", Tuple.Create(1.4838, 1.062, 82, 82, 43));
            GroupList.Add("SiHO", Tuple.Create(1.303, 0.764, 83, 83, 43));
            GroupList.Add("SiO", Tuple.Create(1.1044, 0.466, 84, 84, 43));
            GroupList.Add("NMP", Tuple.Create(3.981, 3.2, 85, 85, 44));
            GroupList.Add("CCl3F", Tuple.Create(3.0356, 2.644, 86, 86, 45));
            GroupList.Add("CCl2F", Tuple.Create(2.2287, 1.916, 87, 87, 45));
            GroupList.Add("HCCl2F", Tuple.Create(2.406, 2.116, 88, 88, 45));
            GroupList.Add("HCClF", Tuple.Create(1.6493, 1.416, 89, 89, 45));
            GroupList.Add("CClF2", Tuple.Create(1.8174, 1.648, 90, 90, 45));
            GroupList.Add("HCClF2", Tuple.Create(1.967, 1.828, 91, 91, 45));
            GroupList.Add("CClF3", Tuple.Create(2.1721, 2.1, 92, 92, 45));
            GroupList.Add("CCl2F2", Tuple.Create(2.6243, 2.376, 93, 93, 45));
            GroupList.Add("CONH2", Tuple.Create(1.4515, 1.248, 94, 94, 46));
            GroupList.Add("CONHCH3", Tuple.Create(2.1905, 1.796, 95, 95, 46));
            GroupList.Add("CONHCH2", Tuple.Create(1.9637, 1.488, 96, 96, 46));
            GroupList.Add("CON(CH3)2", Tuple.Create(2.8589, 2.428, 97, 97, 46));
            GroupList.Add("CONCH3CH2", Tuple.Create(2.6322, 2.12, 98, 98, 46));
            GroupList.Add("CON(CH2)2", Tuple.Create(2.4054, 1.812, 99, 99, 46));
            GroupList.Add("C2H5O2", Tuple.Create(2.1226, 1.904, 100, 100, 47));
            GroupList.Add("C2H4O2", Tuple.Create(1.8952, 1.592, 101, 101, 47));
            GroupList.Add("CH3S", Tuple.Create(1.613, 1.368, 102, 102, 48));
            GroupList.Add("CH2S", Tuple.Create(1.3863, 1.06, 103, 103, 48));
            GroupList.Add("CHS", Tuple.Create(1.1589, 0.748, 104, 104, 48));
            GroupList.Add("Morpholine", Tuple.Create(3.474, 2.796, 105, 105, 49));
            GroupList.Add("C4H4S", Tuple.Create(2.8569, 2.14, 106, 106, 50));
            GroupList.Add("C4H3S", Tuple.Create(2.6908, 1.86, 107, 107, 50));
            GroupList.Add("C4H2S", Tuple.Create(2.5247, 1.58, 108, 108, 50));
            GroupList.Add("Thio", Tuple.Create(0D, 0D, 109, 109, 51));
            GroupList.Add("CH2SuCH2", Tuple.Create(2.6869, 2.12, 110, 110, 52));
            GroupList.Add("CH2SuCH", Tuple.Create(2.4595, 1.808, 111, 111, 52));
            GroupList.Add("CH2OCH2", Tuple.Create(1.5926, 1.32, 112, 112, 53));
            GroupList.Add("CH2OCH", Tuple.Create(1.3652, 1.008, 113, 113, 53));
            GroupList.Add("CH2OC", Tuple.Create(1.1378, 0.78, 114, 114, 53));
            GroupList.Add("CHOCH", Tuple.Create(1.1378, 0.696, 115, 115, 53));
            GroupList.Add("CHOC", Tuple.Create(0.9103, 0.468, 116, 116, 53));
            GroupList.Add("COC", Tuple.Create(0.6829, 0.24, 117, 117, 53));
            GroupList.Add("O=COC=O", Tuple.Create(1.7732, 1.52, 118, 118, 54));
            GroupList.Add("AC-CN", Tuple.Create(1.3342, 0.996, 119, 119, 55));
            GroupList.Add("AC-Br", Tuple.Create(1.3629, 0.972, 120, 120, 56));

            GROUP_INTERACT_PARAMS = new double[56, 56]
            {{ 0,86.02,61.13,76.5,986.5,697.2,1318,1333,476.4,677,114.8,91.46,140,529,255.7,206.6,920.7,287.77,597,663.5,132.1,40.25,24.9,1397,11.44,661.5,543,153.6,184.4,354.55,85.84,4.68,221.4,317.6,526.5,689,-4.189,125.8,485.3,-2.859,-170,122.9,150.6,615.8,-5.869,390.9,553.3,187,216.1,92.99,245.4,562.2,267.6,88.63,0,153.72 },
            { -35.36,0,38.81,74.15,524.1,787.6,270.6,526.1,182.6,448.8,329.3,34.01,128,-34.36,245.21,61.11,749.3,280.5,336.9,318.9,110.4,-23.5,-13.99,-109.7,384.45,357.5,0,76.302,0,262.9,18.12,121.3,58.68,787.9,47.05,-52.87,-66.46,359.3,-70.45,449.4,428,140.8,26.41,191.6,347.13,200.2,268.1,-617,62.56,0,139.4,527.6,501.3,1913,72.19,0},
            {-11.12,3.446,0,167,636.1,637.35,903.8,1329,25.77,347.3,83.36,36.7,-31.52,110.2,122.8,90.49,648.2,-4.449,212.5,537.4,26.51,51.06,174.6,3,187,168,194.9,52.07,-10.43,-64.69,52.13,288.5,-154.2,234.4,169.9,383.9,-259.1,389.3,245.6,22.67,65.69,69.9,1112,221.8,-88.11,0,333.3,0,-59.58,-39.16,237.7,742.1,524.9,84.85,244.67,174.35},
            {-69.7,-113.6,-146.8,0,803.2,603.25,5695,884.9,-52.1,586.6,-30.48,-78.45,-72.88,13.89,-49.29,23.5,664.2,52.8,6096,872.3,1.163,160.9,41.38,-16.11,-211,3629,4448,-9.451,393.6,48.49,-44.85,-4.7,-101.12,-23.88,4284,-119.2,-282.5,101.4,5629,-245.39,296.4,134.7,614.52,6.214,369.89,0,421.9,0,-203.6,184.9,-242.8,856.3,68.95,796.9,795.38,-280.9},
            {156.4,457,89.6,25.82,0,-137.1,353.5,-259.7,84,-203.6,65.33,106.8,50.49,30.74,42.7,-323,-52.39,170,6.712,199,-28.7,70.32,64.07,143.1,123.5,256.5,157.1,488.9,147.5,-120.5,-22.31,-97.27,-2.504,167.9,-202.1,74.27,225.8,44.78,-143.9,0,223,402.5,-143.2,-504.2,72.96,-382.7,-248.3,0,104.7,57.65,-150,325.7,-25.87,794.4,0,147.97},
            {16.51,-12.52,-50,-44.5,249.1,0,-181,-101.7,23.39,306.4,-83.98,-32.69,-165.9,27.97,-20.98,53.9,489.7,580.5,53.28,-202,-25.38,-1.996,573,9.755,-28.25,75.14,457.88,-31.09,17.5,-61.76,-223.9,10.38,-123.6,-119.1,-399.3,-5.224,33.47,-48.25,-172.4,0,109.9,-97.05,397.4,0,-52.1,0,0,37.63,-59.4,-46.01,28.6,261.6,389.3,394.8,284.28,0},
            {300,496.1,362.3,377.6,-229.1,289.6,0,324.5,-195.4,-116,1139,5541,47.41,-11.92,168,304,459,459,112.6,-14.09,2000,370.4,124.2,132.4,133.9,220.6,399.5,887.1,0,188,247.5,1824,395.8,-86.88,-139,160.8,0,0,319,0,762.8,-127.8,419.1,-19.45,0,835.6,139.6,0,407.9,0,-17.4,561.6,738.9,517.5,0,580.28},
            {275.8,217.5,25.34,244.2,-451.6,-265.2,-601.8,0,-356.1,-271.1,-101.6,-52.65,-5.132,39.93,0,0,-305.5,-305.5,0,408.9,-47.63,16.62,-131.7,543.6,6915,0,-413.48,8484,0,0,31.87,21.5,-237.2,0,0,0,0,0,0,0,49.8,40.68,-157.3,-659,0,0,0,0,0,1005,-132.3,609.8,649.7,0,0,0},
            {26.76,42.92,140.1,365.8,164.5,108.7,472.5,-133.1,0,-37.36,24.82,-7.481,-31.95,-23.61,-174.2,-169,6201,7.341,481.7,669.4,-40.62,-130.3,249,161.1,-119.8,137.5,548.5,216.1,-46.28,-163.7,-22.97,28.41,-133.9,142.9,-44.58,-63.5,-34.57,0,-61.7,0,-138.4,19.56,-240.2,274.1,0,0,37.54,0,0,-162.6,185.4,461.6,64.16,-61.2,0,179.74},
            {505.7,56.3,23.39,106,529,-340.2,480.8,-155.6,128,0,-110.3,766,304.1,0,0,0,0,0,-106.4,497.5,751.9,67.52,-483.7,0,0,0,0,0,0,0,0,117,0,2.21,0,-339.2,172.4,0,-268.8,0,-275.5,0,0,0,0,0,0,0,0,0,0,0,79.71,0,0,0},
            {114.8,132.1,85.84,-170,245.4,249.63,200.8,-36.72,372.2,185.1,0,-241.8,-235.7,0,-73.5,-196.7,475.5,-0.13,494.6,660.2,-34.74,108.9,-209.7,54.57,442.4,-81.13,0,183,0,202.3,-101.7,148.3,18.88,71.48,52.08,-28.61,-275.2,0,85.33,0,560.2,0,0,0,0,0,151.8,0,0,0,0,0,36.34,446.9,0,0},
            {329.3,110.4,18.12,428,139.4,227.8,124.63,-234.25,385.4,-236.5,1167,0,-234,0,0,0,0,-233.4,-47.25,-268.1,0,31,-126.2,179.7,24.28,0,0,0,103.9,0,0,0,298.13,0,0,0,-11.4,0,308.9,0,-70.24,0,0,0,0,0,0,0,0,0,0,0,-77.96,0,0,0},
            {83.36,26.51,52.13,65.69,237.7,238.4,-314.7,-178.5,191.1,-7.838,461.3,457.3,0,-78.36,251.5,5422.3,-46.39,213.2,-18.51,664.6,301.1,137.8,-154.3,47.67,134.8,95.18,155.11,140.9,-8.538,170.1,-20.11,-149.5,-202.3,-156.57,128.8,0,240.2,-273.9,254.8,-172.51,417,1338,0,0,0,0,0,0,0,0,0,0,567,102.21,0,0},
            {-30.48,1.163,-44.85,296.4,-242.8,-481.7,-330.48,-870.8,0,0,0,0,222.1,0,-107.2,-41.11,-200.7,0,358.9,0,-82.92,0,0,-99.81,30.05,0,0,0,-70.14,0,0,0,0,0,874.19,0,0,0,-164,0,0,-664.4,275.9,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {65.33,-28.7,-22.31,223,-150,-370.3,-448.2,0,394.6,0,136,0,-56.08,127.4,0,-189.2,138.54,431.49,147.1,0,0,0,0,71.23,-18.93,0,0,0,0,0,939.07,0,0,0,0,0,0,570.9,-255.22,0,-38.77,448.1,-1327,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-83.98,-25.38,-223.9,109.9,28.6,-406.8,-598.8,0,225.3,0,2889,0,-194.1,38.89,865.9,0,287.43,0,1255.1,0,-182.91,-73.85,-352.9,-262,-181.9,0,0,0,0,0,0,0,0,0,243.1,0,0,-196.3,22.05,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1139,2000,247.5,762.8,-17.4,-118.1,-341.6,-253.1,-450.3,0,-294.8,0,285.36,-15.07,64.3,-24.46,0,89.7,-281.6,-396,287,-111,0,882,617.5,0,-139.3,0,0,0,0.1004,0,0,0,0,0,0,0,-334.4,0,-89.42,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-101.6,-47.63,31.87,49.8,-132.3,-378.2,-332.9,-341.6,29.1,0,8.87,554.4,-156.1,0,-207.66,0,117.4,0,-169.7,-153.7,0,-351.6,-114.7,-205.3,-2.17,0,2845,0,0,0,0,0,-60.78,0,0,0,160.7,-158.8,0,0,0,0,0,0,0,0,0,0,0,-136.6,0,0,0,98.82,0,0},
            {24.82,-40.62,-22.97,-138.4,185.4,162.6,242.8,0,-287.5,224.66,-266.6,99.37,38.81,-157.3,-108.5,-446.86,777.4,134.3,0,205.27,4.933,-152.7,-15.62,-54.86,-4.624,-0.515,0,230.9,0.4604,0,177.5,0,-62.17,-203,0,81.57,-55.77,0,-151.5,0,120.3,0,0,0,0,0,16.23,0,0,0,0,0,0,0,0,0},
            {315.3,1264,62.32,89.86,-151,339.8,-66.17,-11,-297.8,-165.5,-256.3,193.9,-338.5,0,0,0,493.8,-313.5,92.07,0,13.41,-44.7,39.63,183.4,-79.08,0,0,0,0,-208.9,0,228.4,-95,0,-463.6,0,-11.16,0,-228,0,-337,169.3,127.2,0,0,-322.3,0,0,0,0,0,0,12.55,-60.07,88.09,0},
            {91.46,40.25,4.68,122.9,562.2,529,698.2,0,286.3,-47.51,35.38,0,225.4,131.2,0,151.38,429.7,0,54.32,519.1,0,108.3,249.2,62.42,153,32.73,86.2,450.1,59.02,65.56,0,2.22,344.4,0,0,0,-168.2,0,6.57,0,63.67,0,0,0,0,0,0,0,0,0,0,0,-127.9,0,0,0},
            {34.01,-23.5,121.3,140.8,527.6,669.9,708.7,1633.5,82.86,190.6,-132.9,80.99,-197.7,0,0,-141.4,140.8,587.3,258.6,543.3,-84.53,0,0,56.33,223.1,108.9,0,0,0,149.56,0,177.6,315.9,0,215,0,-91.8,0,-160.28,0,-96.87,0,0,0,0,0,361.1,0,0,0,0,0,0,0,0,0},
            {36.7,51.06,288.5,69.9,742.1,649.1,826.76,0,552.1,242.8,176.5,235.6,-20.93,0,0,-293.7,0,18.98,74.04,504.2,-157.1,0,0,-30.1,192.1,0,0,116.6,0,-64.38,0,86.4,168.8,0,363.7,0,111.2,0,0,0,255.8,0,0,-35.68,0,0,0,565.9,0,0,0,0,165.67,0,0,0},
            {-78.45,160.9,-4.7,134.7,856.3,709.6,1201,10000,372,0,129.5,351.9,113.9,261.1,91.13,316.9,898.2,368.5,492,631,11.8,17.97,51.9,0,-75.97,490.9,534.7,132.2,0,546.7,0,247.8,146.6,0,337.7,369.5,187.1,215.2,498.6,0,256.5,0,233.1,0,0,0,423.1,63.95,0,108.5,0,585.19,291.87,532.73,0,127.16},
            {106.8,70.32,-97.27,402.5,325.7,612.8,-274.5,622.3,518.4,0,-171.1,383.3,-25.15,108.5,102.2,2951,334.9,20.18,363.5,993.4,-129.7,-8.309,-0.2266,-248.4,0,132.7,2213,0,0,0,0,0,593.4,0,1337.37,0,0,0,5143.14,309.58,-145.1,0,0,-209.7,0,0,434.1,0,0,0,0,0,0,0,0,8.48},
            {-32.69,-1.996,10.38,-97.05,261.6,252.6,417.9,0,-142.6,0,129.3,0,-94.49,0,0,0,0,0,0.2827,0,113,-9.639,0,-34.68,132.9,0,533.2,320.2,0,0,139.8,304.3,10.17,-27.7,0,0,10.76,0,-223.1,0,248.4,0,0,0,-218.9,0,0,0,0,-4.565,0,0,0,0,0,0},
            {5541,370.4,1824,-127.8,561.6,511.29,360.7,815.12,-101.5,0,0,0,220.66,0,0,0,134.9,2475,0,0,1971,0,0,514.6,-123.1,-85.12,0,0,0,0,0,2990,-124,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1742.53},
            {-52.65,16.62,21.5,40.68,609.8,914.2,1081,1421,303.7,0,243.8,0,112.4,0,0,0,0,0,335.7,0,-73.09,0,-26.06,-60.71,0,277.8,0,0,0,0,0,292.7,0,0,0,0,-47.37,0,0,0,469.8,0,0,0,0,0,0,0,0,0,0,0,0,684.78,0,0},
            {-7.481,-130.3,28.41,19.56,461.6,448.6,0,0,160.6,0,0,201.5,63.71,106.7,0,0,0,0,161,0,-27.94,0,0,0,0,0,0,0,0,0,0,0,0,0,31.66,0,0,0,78.92,0,0,0,0,1004,0,0,0,-18.27,0,0,0,0,0,0,0,0},
            {-25.31,82.64,157.3,128.8,521.6,287,23.48,0,317.5,0,-146.3,0,-87.31,0,0,0,0,0,0,570.6,-39.46,-116.21,48.48,-133.16,0,0,0,0,0,0,0,0,0,0,0,0,262.9,0,0,0,43.37,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {140,24.9,221.4,150.6,267.6,240.8,-137.4,838.4,135.4,0,152,0,9.207,0,-213.74,0,192.3,0,169.6,0,0,0,0,0,0,481.3,0,0,0,0,0,0,0,0,-417.2,0,0,0,302.2,0,347.8,0,0,-262,0,0,-353.5,0,0,0,0,0,0,0,0,0},
            {128,-13.99,58.68,26.41,501.3,431.3,0,0,138,245.9,21.92,0,476.6,0,0,0,0,0,0,616.6,179.25,-40.82,21.76,48.49,0,64.28,2448,-27.45,0,0,0,0,6.37,0,0,0,0,0,0,0,68.55,0,0,0,0,0,0,0,0,0,0,0,0,190.81,0,0},
            {-31.52,174.6,-154.2,1112,524.9,494.7,79.18,0,-142.6,0,24.37,-92.26,736.4,0,0,0,0,-42.71,136.9,5256,-262.3,-174.5,-46.8,77.55,-185.3,125.3,4288,0,0,0,0,37.1,0,0,32.9,0,-48.33,0,336.25,0,-195.1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-72.88,41.38,-101.12,614.52,68.95,967.71,0,0,443.6,-55.87,-111.45,0,173.77,0,0,0,0,0,329.1,0,0,0,0,0,0,174.4,0,0,0,0,0,0,0,0,0,0,2073,0,-119.8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {50.49,64.07,-2.504,-143.2,-25.87,695,-240,0,110.4,0,41.57,0,-93.51,-366.51,0,-257.2,0,0,0,-180.2,0,-215,-343.6,-58.43,-334.12,0,0,0,85.7,0,535.8,0,-111.2,0,0,0,0,0,-97.71,0,153.7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-165.9,573,-123.6,397.4,389.3,218.8,386.6,0,114.55,354,175.5,0,0,0,0,0,0,0,-42.31,0,0,0,0,-85.15,0,0,0,0,0,0,0,0,0,0,0,0,-208.8,0,-8.804,0,423.4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {47.41,124.2,395.8,419.1,738.9,528,0,0,-40.9,183.8,611.3,134.5,-217.9,0,0,0,0,281.6,335.2,898.2,383.2,301.9,-149.8,-134.2,0,379.4,0,167.9,0,82.64,0,0,322.42,631.5,0,837.2,0,0,255,0,730.8,0,0,0,0,0,0,2429,0,0,0,0,-127.06,0,0,0},
            {-5.132,-131.7,-237.2,-157.3,649.7,645.9,0,0,0,0,0,0,167.1,0,-198.8,116.5,0,159.8,0,0,0,0,0,-124.6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-110.65,-117.2,0,0,0,26.35,0,0,0,0,0,0,0,0,0,0,0,117.59},
            {-31.95,249,-133.9,-240.2,64.16,172.2,-287.1,0,97.04,13.89,-82.12,-116.7,-158.2,49.7,10.03,-185.2,343.7,0,150.6,-97.77,-55.21,397.24,0,-186.7,-374.16,223.6,0,0,-71,0,-191.7,0,-176.26,6.699,136.6,5.15,-137.7,0,0,-5.579,72.31,0,0,0,0,0,0,0,0,0,0,0,0,0,0,39.84},
            {147.3,62.4,140.6,839.83,0,0,0,0,0,0,0,0,278.15,0,0,0,0,0,0,0,0,0,0,0,33.95,0,0,0,0,0,0,0,0,0,0,0,0,0,55.8,0,0,0,0,0,111.8,0,0,0,0,0,0,0,0,0,0,0},
            {529,1397,317.6,615.8,88.63,171,284.4,-167.3,123.4,577.5,-234.9,65.37,-247.8,0,284.5,0,-22.1,0,-61.6,1179,182.2,305.4,-193,335.7,1107,-124.7,0,885.5,0,-64.28,-264.3,288.1,627.7,0,-29.34,-53.91,-198,0,-28.65,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-100.53,0,0},
            {-34.36,-109.7,787.9,191.6,1913,0,180.2,0,992.4,0,0,0,448.5,961.8,1464,0,0,0,0,2450,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2166,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {110.2,3,234.4,221.8,84.85,0,0,0,0,0,0,0,0,-125.2,1604,0,0,0,0,2496,0,0,0,70.81,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,745.3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {13.89,-16.11,-23.88,6.214,796.9,0,832.2,-234.7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-196.2,0,161.5,0,0,0,-274.1,0,262,0,0,0,0,0,-66.31,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {30.74,143.1,167.9,-504.2,794.4,762.7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,844,0,0,0,0,0,0,0,0,0,0,0,0,0,-32.17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {27.97,9.755,-119.1,0,394.8,0,-509.3,0,0,0,0,0,0,0,0,0,0,0,0,-70.25,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-11.92,132.4,-86.88,-19.45,517.5,0,-205.7,0,156.4,0,-3.444,0,0,0,0,0,0,0,119.2,0,0,-194.7,0,3.163,7.082,0,0,0,0,0,515.8,0,0,0,0,0,0,0,0,0,101.2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {39.93,543.6,0,-659,0,420,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-363.1,-11.3,0,0,0,0,6.971,0,0,0,0,0,0,0,148.9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-23.61,161.1,142.9,274.1,-61.2,-89.24,-384.3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-8.479,0,23.93,2.845,682.5,597.8,0,810.5,278.8,0,0,0,0,0,0,0,0,221.4,0,0,0,0,0,-79.34,0,176.3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {255.7,11.44,526.5,-5.869,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {245.21,384.45,47.05,347.13,72.19,265.75,627.39,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,75.04,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {122.8,187,169.9,-88.11,244.67,163.76,833.21,0,569.18,-1.25,-38.4,69.7,-375.6,0,0,0,0,0,0,600.78,291.1,0,-286.26,-52.93,0,0,0,0,0,0,0,0,0,0,0,0,177.12,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-49.29,-211,4284,369.89,795.38,0,0,0,-62.02,0,-229.01,0,-196.59,0,0,0,0,100.25,0,472.04,0,0,0,196.73,0,0,0,434.32,0,0,0,313.14,0,0,0,0,0,0,0,0,-244.59,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {42.7,123.5,-202.1,72.96,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,171.94,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {-20.98,-28.25,-399.3,-52.1,284.28,0,401.2,0,106.21,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-108.37,5.76,0,-272.01,0,0,0,0,0,0,0,0,0,0,107.84,-33.93,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0} };
        }
    }

    public class LLEUnifacData : BaseUnifacData
    {
        /// <summary>
        /// R, Q and Primary Group
        /// </summary>
        public LLEUnifacData()
        {
            GroupList.Add("CH3", 0.9011, 0.848, 1, 1, 1);
            GroupList.Add("CH2", 0.6744, 0.54, 2, 2, 1);
            GroupList.Add("CH", 0.4469, 0.228, 3, 3, 1);
            GroupList.Add("C", 0.2195, 0, 4, 4, 1);
            GroupList.Add("CH2=CH", 1.3454, 1.176, 5, 5, 2);
            GroupList.Add("CH=CH", 1.1167, 0.867, 6, 6, 2);
            GroupList.Add("CH=C", 0.8886, 0.676, 7, 7, 2);
            GroupList.Add("CH2=C", 1.1173, 0.988, 8, 8, 2);
            GroupList.Add("ACH", 0.5313, 0.4, 9, 9, 3);
            GroupList.Add("AC", 0.3652, 0.12, 10, 10, 3);
            GroupList.Add("ACCH3", 1.2663, 0.968, 11, 11, 4);
            GroupList.Add("ACCH2", 1.0396, 0.66, 12, 12, 4);
            GroupList.Add("ACCH", 0.8121, 0.348, 13, 13, 4);
            GroupList.Add("OH", 1, 1.2, 14, 14, 5);
            GroupList.Add("P1", 3.2499, 3.128, 15, 15, 6);
            GroupList.Add("P2", 3.2491, 3.124, 16, 16, 7);
            GroupList.Add("H2O", 0.92, 1.4, 17, 17, 8);
            GroupList.Add("ACOH", 0.8952, 0.68, 18, 18, 9);
            GroupList.Add("CH3CO", 1.6724, 1.488, 19, 19, 10);
            GroupList.Add("CH2CO", 1.4457, 1.18, 20, 20, 10);
            GroupList.Add("CHO", 0.998, 0.948, 21, 21, 11);
            GroupList.Add("furfural", 3.168, 2.484, 22, 22, 12);
            GroupList.Add("COOH", 1.3013, 1.224, 23, 23, 13);
            GroupList.Add("HCOOH", 1.528, 1.532, 24, 24, 13);
            GroupList.Add("CH3COO", 1.9031, 1.728, 25, 25, 14);
            GroupList.Add("CH2COO", 1.6764, 1.42, 26, 26, 14);
            GroupList.Add("CH3O", 1.145, 1.088, 27, 27, 15);
            GroupList.Add("CH2O", 0.9183, 0.78, 28, 28, 15);
            GroupList.Add("CH-O", 0.6908, 0.468, 29, 29, 15);
            GroupList.Add("FCH2O", 0.9183, 1.1, 30, 30, 15);
            GroupList.Add("CH2CL", 1.4654, 1.264, 31, 31, 16);
            GroupList.Add("CHCL", 1.238, 0.952, 32, 32, 16);
            GroupList.Add("CCL", 1.006, 0.724, 33, 33, 16);
            GroupList.Add("CH2CL2", 2.2564, 1.988, 34, 34, 17);
            GroupList.Add("CHCL2", 2.0606, 1.684, 35, 35, 17);
            GroupList.Add("CCL2", 1.8016, 1.448, 36, 36, 18);
            GroupList.Add("CHCL3", 2.87, 2.41, 37, 37, 18);
            GroupList.Add("CCL3", 2.6401, 2.184, 38, 38, 19);
            GroupList.Add("CCL4", 3.39, 2.91, 39, 39, 20);
            GroupList.Add("ACCL", 1.1562, 0.844, 40, 40, 21);
            GroupList.Add("CH3CN", 1.8701, 1.724, 41, 41, 21);
            GroupList.Add("CH2CN", 1.6434, 1.416, 42, 42, 22);
            GroupList.Add("ACHNH2", 1.06, 0.816, 43, 43, 23);
            GroupList.Add("CH3NO2", 2.0086, 1.868, 44, 44, 23);
            GroupList.Add("CH2NO2", 1.7818, 1.56, 45, 45, 23);
            GroupList.Add("CHNO2", 1.5544, 1.248, 46, 46, 23);
            GroupList.Add("ACNO2", 1.4199, 1.104, 47, 47, 24);
            GroupList.Add("(CH3OH)2", 2.4088, 2.248, 48, 48, 25);
            GroupList.Add("(HOCH2CH2)2O", 4.0013, 3.568, 49, 49, 26);
            GroupList.Add("C5H5N", 2.9993, 2.113, 50, 50, 27);
            GroupList.Add("C5H4N", 2.8332, 1.833, 51, 51, 27);
            GroupList.Add("C5H3N", 2.667, 1.553, 52, 52, 27);
            GroupList.Add("CCL2=CHCL", 3.3092, 2.86, 53, 53, 28);
            GroupList.Add("HCONHCH3", 2.4317, 2.192, 54, 54, 29);
            GroupList.Add("HCON(CH3)2", 3.0856, 2.736, 55, 55, 30);
            GroupList.Add("(CH2)4SO2)", 4.0358, 3.2, 56, 56, 31);
            GroupList.Add("(CH2)2SO", 2.8266, 2.472, 57, 57, 32);

            GROUP_INTERACT_PARAMS = new double[32, 32] {
            {0.00,74.54,-114.80,-115.70,644.60,329.60,310.70,1300.00,2255.00,472.60,158.10,383.00,139.40,972.40,662.10,42.14,-243.90,7.50,-5.55,924.80,696.80,902.20,556.70,575.70,527.50,269.20,-300.00,-63.60,928.30,331.00,561.40,956.50 },
            {292.30,0.00,340.70,4102.00,724.40,1731.00,1731.00,896.00,0.00,343.70,-214.70,0.00,1647.00,-577.50,289.30,99.61,337.10,4583.00,5831.00,0.00,405.90,0.00,425.70,0.00,0.00,0.00,0.00,0.00,500.70,115.40,784.40,265.40 },
            {156.50,-94.78,0.00,167.00,703.90,511.50,577.30,859.40,1649.00,593.70,362.80,31.14,461.80,6.00,32.14,-18.81,0.00,-231.90,3000.00,-878.10,29.13,1.64,-1.77,-11.19,358.90,363.50,-578.20,0.00,364.20,-58.10,21.97,84.16 },
            {104.40,-269.70,-146.80,0.00,4000.00,136.60,906.80,5695.00,292.60,916.70,1218.00,715.60,339.10,5688.00,213.10,-114.10,0.00,-12.14,-141.30,-107.30,1208.00,689.60,3629.00,-175.60,337.70,1023.00,-390.70,0.00,0.00,0.00,238.00,132.20 },
            {328.20,470.70,-9.21,1.27,0.00,937.30,991.30,28.73,-195.50,67.07,1409.00,-140.30,-104.00,195.60,262.50,62.05,272.20,-61.57,-41.75,-597.10,-189.30,-348.20,-30.70,-159.00,536.60,53.37,183.30,-44.44,0.00,0.00,0.00,0.00 },
            {-136.70,-135.70,-223.00,-162.60,-281.10,0.00,0.00,-61.29,-153.20,-47.41,-344.10,299.30,244.40,19.57,1970.00,-166.40,128.60,1544.00,224.60,0.00,0.00,0.00,150.80,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {-131.90,-135.70,-252.00,-273.60,-268.80,0.00,0.00,5.89,-153.20,353.80,-338.60,-241.80,-57.98,487.10,1970.00,-166.40,507.80,1544.00,-207.00,0.00,0.00,0.00,150.80,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {342.40,220.60,372.80,203.70,-122.40,247.00,104.90,0.00,344.50,-171.80,-349.90,66.95,-465.70,-6.32,64.42,315.90,370.70,356.80,502.90,-97.27,198.30,-109.80,1539.00,32.92,-269.20,0.00,-873.60,1429.00,-364.20,-117.40,18.41,0.00},
            {-159.80,0.00,-473.20,-470.40,-63.15,-547.20,-547.20,-595.90,0.00,-825.70,0.00,0.00,0.00,-898.30,0.00,0.00,0.00,0.00,4894.00,0.00,0.00,-851.60,0.00,-16.13,-538.60,0.00,-637.30,0.00,0.00,0.00,0.00,0.00},
            {66.56,306.10,-78.31,-73.87,216.00,401.70,-127.60,634.80,-568.00,0.00,-37.36,120.30,1247.00,258.70,5.20,1000.00,-301.00,12.01,-10.88,902.60,430.60,1010.00,400.00,-328.60,211.60,0.00,0.00,148.00,0.00,0.00,0.00,0.00},
            {146.10,517.00,-75.30,223.20,-431.30,643.40,231.40,623.70,0.00,128.00,0.00,1724.00,0.75,-245.80,0.00,751.80,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {14.78,0.00,-10.44,-184.90,444.70,-94.64,732.30,211.60,0.00,48.93,-311.60,0.00,1919.00,57.70,0.00,0.00,-347.90,-249.30,61.59,0.00,0.00,0.00,0.00,0.00,-278.20,0.00,-208.40,-13.91,0.00,173.80,0.00,0.00},
            {1744.00,-48.52,75.49,147.30,118.40,728.70,349.10,652.30,0.00,-101.30,1051.00,-115.70,0.00,-117.60,-96.62,19.77,1670.00,48.15,48.83,874.30,0.00,942.20,446.30,0.00,572.70,0.00,0.00,-2.16,0.00,0.00,0.00,0.00},
            {-320.10,485.60,114.80,-170.00,180.60,-76.64,-152.80,385.90,-337.30,58.84,1090.00,-46.13,1417.00,0.00,-235.70,0.00,108.90,-209.70,54.57,629.00,-149.20,0.00,0.00,0.00,343.10,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {1571.00,76.44,52.13,65.69,137.10,-218.10,-218.10,212.80,0.00,52.38,0.00,0.00,1402.00,461.30,0.00,301.10,137.80,-154.30,47.67,0.00,0.00,0.00,95.18,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {73.80,-24.36,4.68,122.90,455.10,351.50,351.50,770.00,0.00,483.90,-47.51,0.00,337.10,0.00,225.40,0.00,110.50,249.20,62.42,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {27.90,-52.71,0.00,0.00,669.20,-186.11,-401.60,740.40,0.00,550.60,0.00,808.80,437.70,-132.90,-197.70,-21.35,0.00,0.00,56.33,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {21.23,-185.10,288.50,33.60,418.40,-465.70,-465.70,793.20,0.00,432.20,0.00,203.14,370.40,176.50,-20.93,-157.10,0.00,0.00,-30.10,0.00,70.04,-75.50,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {89.97,-293.70,-4.70,134.70,713.50,-260.30,512.20,1205.00,1616.00,550.00,0.00,70.14,438.10,129.50,113.90,11.80,17.97,51.90,0.00,475.80,492.00,1302.00,490.90,534.70,0.00,0.00,18.98,0.00,0.00,0.00,0.00,0.00},
            {-59.06,0.00,777.80,-47.13,1989.00,0.00,0.00,390.70,0.00,190.50,0.00,0.00,1349.00,-246.30,0.00,0.00,0.00,0.00,-255.40,0.00,346.20,0.00,-154.50,0.00,124.80,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {29.08,34.78,56.41,-53.29,2011.00,0.00,0.00,63.48,0.00,-349.20,0.00,0.00,0.00,2.41,0.00,0.00,0.00,-15.62,-54.86,-465.20,0.00,0.00,0.00,0.00,0.00,0.00,-387.70,0.00,0.00,0.00,0.00,0.00},
            {175.80,0.00,-218.90,-15.41,529.00,0.00,0.00,-239.80,-860.30,857.70,0.00,0.00,681.40,0.00,0.00,0.00,0.00,-216.30,8455.00,0.00,0.00,0.00,0.00,179.90,125.30,0.00,134.30,0.00,0.00,0.00,0.00,0.00},
            {94.34,375.40,113.60,-97.05,483.80,264.70,264.70,13.32,0.00,377.00,0.00,0.00,152.40,0.00,-94.49,0.00,0.00,0.00,-34.68,794.40,0.00,0.00,0.00,0.00,139.80,0.00,924.50,0.00,0.00,0.00,0.00,0.00},
            {193.60,0.00,7.18,-127.10,332.60,0.00,0.00,439.90,-230.40,211.60,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,514.60,0.00,0.00,175.80,0.00,0.00,963.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {108.50,0.00,247.30,453.40,-289.30,0.00,0.00,-424.30,523.00,82.77,0.00,-75.23,-1707.00,29.86,0.00,0.00,0.00,0.00,0.00,-241.70,0.00,164.40,481.30,-246.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {81.49,0.00,1.49,-50.71,-30.28,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {-128.80,0.00,-225.30,-124.60,-319.20,0.00,0.00,203.00,-222.70,0.00,0.00,-201.90,0.00,0.00,0.00,0.00,0.00,-114.70,0.00,-906.50,-169.70,-944.90,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {147.30,0.00,0.00,0.00,837.90,0.00,0.00,1153.00,0.00,417.40,0.00,123.20,639.70,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {-11.91,176.70,-80.48,0.00,0.00,0.00,0.00,-311.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {14.91,132.10,-17.78,0.00,0.00,0.00,0.00,-262.60,0.00,0.00,0.00,-281.90,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {67.84,42.73,59.16,26.59,0.00,0.00,0.00,1.11,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00},
            {36.42,60.82,29.77,55.97,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00 } };
        }
    }
}