﻿using System;
using System.Collections.Generic;

namespace RefReactor
{
    public class MixedCompclass
    {
        private Dictionary<string, double[]> C8AMix = new Dictionary<string, double[]>(){
            {"C8A", new  double []{0.096 , 0.209 , 0.35 , 0.345 }} ,
            {"C9A", new  double []{0.096 , 0.209 , 0.35 , 0.345 }} ,
            {"DMCH", new  double []{0, 0.232 , 0.395 , 0.373 }},
            {"C8CP", new  double []{0, 0.232 , 0.395 , 0.373 }} };

        private Dictionary<string, double[]> MixedXylenes = new Dictionary<string, double[]>(){
            {"C8A", new  double []{0.096 , 0.209 , 0.35 , 0.345}} ,
            {"C9A", new  double []{0.096 , 0.209 , 0.35 , 0.345}} ,
            {"DMCH", new  double []{0, 0.232 , 0.395 , 0.373 }},
            {"C8CP", new  double []{0, 0.232 , 0.395 , 0.373 }}};

        private Dictionary<string, double[]> Crack = new Dictionary<string, double[]>(){
            {"C9P",new  double []{0,0.176,0.283,0.286,0.091,0.164,0.172,0.083,0.206,0.08,0,0,0,0.19,0.093,0,0,0,0.176,0,0,0,0}},
            {"C9CH",new  double []{0,0.176,0.283,0.308,0.083,0.15,0.15,0.083,0.206,0.102,0,0,0,0.19,0.093,0,0,0,0.176,0,0,0,0}},
            {"C9CP",new  double []{0,0.176,0.283,0.308,0.083,0.15,0.15,0.083,0.206,0.102,0,0,0,0.19,0.093,0,0,0,0.176,0,0,0,0}},
            {"C8P",new  double []{0,0.199,0.295,0.376,0.092,0.168,0.26,0.116,0.215,0.08,0,0,0,0.133,0.066,0,0,0,0,0,0,0,0}},
            {"ECH",new  double []{0,0.199,0.322,0.349,0.092,0.168,0.233,0.116,0.215,0.107,0,0,0,0.133,0.066,0,0,0,0,0,0,0,0}},
            {"DMCH",new  double []{0,0.199,0.322,0.349,0.092,0.168,0.233,0.116,0.215,0.107,0,0,0,0.133,0.066,0,0,0,0,0,0,0,0}},
            {"PCP",new  double []{0,0.199,0.322,0.349,0.092,0.168,0.233,0.116,0.215,0.107,0,0,0,0.133,0.066,0,0,0,0,0,0,0,0}},
            {"C8CP",new  double []{0,0.199,0.322,0.349,0.092,0.168,0.233,0.116,0.215,0.107,0,0,0,0.133,0.066,0,0,0,0,0,0,0,0}},
            {"IC7",new  double []{0,0.237,0.366,0.397,0.141,0.256,0.234,0.132,0.158,0.079,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"NC7",new  double []{0,0.237,0.366,0.397,0.141,0.256,0.234,0.132,0.158,0.079,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"MCH",new  double []{0,0.237,0.366,0.397,0.141,0.256,0.234,0.132,0.158,0.079,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"C7CP",new  double []{0,0.237,0.366,0.397,0.141,0.256,0.234,0.132,0.158,0.079,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"IC6",new  double []{0,0.288,0.462,0.5,0.164,0.298,0.192,0.096,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"NC6",new  double []{0,0.288,0.462,0.5,0.164,0.298,0.192,0.096,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"CH",new  double []{0,0.288,0.462,0.5,0.164,0.298,0.192,0.096,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"MCP",new  double []{0,0.288,0.462,0.5,0.164,0.298,0.192,0.096,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"IC5",new  double []{0,0.5,0.5,0.5,0.2,0.3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"NC5",new  double []{0,0.5,0.5,0.5,0.2,0.3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"IC4",new  double []{0,0.665,0.67,0.665,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},
            {"NC4",new  double []{0,0.665,0.67,0.665,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}}};

        public enum CrackList
        {
            C9P, C9CH, C9CP, C8P, ECH, DMCH, PCP, C8CP, IC7, NC7, MCH, C7CP, IC6, NC6, CH, MCP, IC5, NC5, IC4, NC4
        }

        private double[][] Crack2 = new double[][]{
           new  double []{ 0, 0.176,0.283,0.286,0.091,0.164,0.172,0.083,0.206,0.08,0,0,0,0.19,0.093,0,0,0,0.176,0,0,0,0},
           new  double [] { 0, 0.176, 0.283, 0.308, 0.083, 0.15, 0.15, 0.083, 0.206, 0.102, 0, 0, 0, 0.19, 0.093, 0, 0, 0, 0.176, 0, 0, 0, 0 },
           new  double [] { 0, 0.176, 0.283, 0.308, 0.083, 0.15, 0.15, 0.083, 0.206, 0.102, 0, 0, 0, 0.19, 0.093, 0, 0, 0, 0.176, 0, 0, 0, 0 },
           new  double [] { 0, 0.199, 0.295, 0.376, 0.092, 0.168, 0.26, 0.116, 0.215, 0.08, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.199, 0.322, 0.349, 0.092, 0.168, 0.233, 0.116, 0.215, 0.107, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.199, 0.322, 0.349, 0.092, 0.168, 0.233, 0.116, 0.215, 0.107, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.199, 0.322, 0.349, 0.092, 0.168, 0.233, 0.116, 0.215, 0.107, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.199, 0.322, 0.349, 0.092, 0.168, 0.233, 0.116, 0.215, 0.107, 0, 0, 0, 0.133, 0.066, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.237, 0.366, 0.397, 0.141, 0.256, 0.234, 0.132, 0.158, 0.079, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.237, 0.366, 0.397, 0.141, 0.256, 0.234, 0.132, 0.158, 0.079, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.237, 0.366, 0.397, 0.141, 0.256, 0.234, 0.132, 0.158, 0.079, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.237, 0.366, 0.397, 0.141, 0.256, 0.234, 0.132, 0.158, 0.079, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.288, 0.462, 0.5, 0.164, 0.298, 0.192, 0.096, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.288, 0.462, 0.5, 0.164, 0.298, 0.192, 0.096, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.288, 0.462, 0.5, 0.164, 0.298, 0.192, 0.096, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.288, 0.462, 0.5, 0.164, 0.298, 0.192, 0.096, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.5, 0.5, 0.5, 0.2, 0.3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.5, 0.5, 0.5, 0.2, 0.3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.665, 0.67, 0.665, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           new  double [] { 0, 0.665, 0.67, 0.665, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};

        public double[] SolveMixFeeds(RefMixedComps MixName, string CompName, bool IsProduct)
        {
            double[] res = new double[30];
            int index;
            switch (MixName)
            {
                case RefMixedComps.C8A:
                    var item = C8AMix[CompName];
                    if (IsProduct)
                    {
                        index = (int)RefCompNames.H2;
                        res[index] = 3;
                        index = (int)RefCompNames.EB;
                        res[index] = item[0];
                        index = (int)RefCompNames.PX;
                        res[index] = item[1];
                        index = (int)RefCompNames.MX;
                        res[index] = item[2];
                        index = (int)RefCompNames.OX;
                        res[index] = item[3];
                    }
                    else
                    {
                        index = (int)RefCompNames.H2;
                        res[index] = -3;
                        index = (int)RefCompNames.EB;
                        res[index] = -item[0];
                        index = (int)RefCompNames.PX;
                        res[index] = -item[1];
                        index = (int)RefCompNames.MX;
                        res[index] = -item[2];
                        index = (int)RefCompNames.OX;
                        res[index] = -item[3];
                    }
                    break;

                case RefMixedComps.MixedXylenes:
                    item = C8AMix[CompName];
                    if (IsProduct)
                    {
                        index = (int)RefCompNames.H2;
                        res[index] = 3;
                        index = (int)RefCompNames.EB;
                        res[index] = item[0];
                        index = (int)RefCompNames.PX;
                        res[index] = item[1];
                        index = (int)RefCompNames.MX;
                        res[index] = item[2];
                        index = (int)RefCompNames.OX;
                        res[index] = item[3];
                    }
                    else
                    {
                        index = (int)RefCompNames.H2;
                        res[index] = -3;
                        index = (int)RefCompNames.EB;
                        res[index] = -item[0];
                        index = (int)RefCompNames.PX;
                        res[index] = -item[1];
                        index = (int)RefCompNames.MX;
                        res[index] = -item[2];
                        index = (int)RefCompNames.OX;
                        res[index] = -item[3];
                    }
                    break;

                case RefMixedComps.Hydrocracked:
                    //double [] CrackSplits = Crack[CompName];
                    double[] CrackSplits = Crack2[(int)Enum.Parse(typeof(CrackList), CompName)];
                    return CrackSplits;

                default:
                    break;
            }

            return res;
        }

        public double[] SolveMixProducts(RefMixedComps MixType, string CompName, bool IsProduct)
        {
            double[] res = new double[30];
            int index;
            switch (MixType)
            {
                case RefMixedComps.C8A:
                    var item = C8AMix[CompName];
                    if (IsProduct)
                    {
                        index = (int)RefCompNames.H2;
                        res[index] = 0;
                        index = (int)RefCompNames.C1;
                        res[index] = 1;
                        index = (int)RefCompNames.EB;
                        res[index] = item[0];
                        index = (int)RefCompNames.PX;
                        res[index] = item[1];
                        index = (int)RefCompNames.MX;
                        res[index] = item[2];
                        index = (int)RefCompNames.OX;
                        res[index] = item[3];
                    }
                    else
                    {
                        index = (int)RefCompNames.H2;
                        res[index] = 3;
                        index = (int)RefCompNames.EB;
                        res[index] = -item[0];
                        index = (int)RefCompNames.PX;
                        res[index] = -item[1];
                        index = (int)RefCompNames.MX;
                        res[index] = -item[2];
                        index = (int)RefCompNames.OX;
                        res[index] = -item[3];
                    }
                    break;

                case RefMixedComps.MixedXylenes:
                    item = MixedXylenes[CompName];
                    if (IsProduct)
                    {
                        index = (int)RefCompNames.H2;
                        res[index] = 3;
                        index = (int)RefCompNames.EB;
                        res[index] = item[0];
                        index = (int)RefCompNames.PX;
                        res[index] = item[1];
                        index = (int)RefCompNames.MX;
                        res[index] = item[2];
                        index = (int)RefCompNames.OX;
                        res[index] = item[3];
                    }
                    else
                    {
                        index = (int)RefCompNames.H2;
                        res[index] = 3;
                        index = (int)RefCompNames.EB;
                        res[index] = -item[0];
                        index = (int)RefCompNames.PX;
                        res[index] = -item[1];
                        index = (int)RefCompNames.MX;
                        res[index] = -item[2];
                        index = (int)RefCompNames.OX;
                        res[index] = -item[3];
                    }
                    break;

                case RefMixedComps.Hydrocracked:
                    double[] CrackSplits = Crack[CompName];
                    return CrackSplits;

                default:
                    break;
            }

            return res;
        }
    }
}