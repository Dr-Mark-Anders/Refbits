using ModelEngine;
using Extensions;
using MatrixAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class GibbsReactor : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new("In1", FlowDirection.IN);
        public Port_Material PortOut1 = new("Out1", FlowDirection.OUT);
        public Port_Material PortOut2 = new("Out2", FlowDirection.OUT);

        private Gibbs[] Gibbs;
        private double[] guesses;
        private Dictionary<string, double> TotalElements;
        private double[,] ElementMatrix;

        public List<bool> actives = new();

        public GibbsReactor() : base()
        {
            Add(PortIn);
            Add(PortOut1);
            Add(PortOut2);
        }

        public GibbsReactor(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                PortIn = (Port_Material)info.GetValue("In1", typeof(Port_Material));
                PortOut1 = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
                PortOut2 = (Port_Material)info.GetValue("Out2", typeof(Port_Material));
                actives = (List<bool>)info.GetValue("actives", typeof(List<bool>));
            }
            catch { }

            Add(PortIn);
            Add(PortOut1);
            Add(PortOut2);
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", PortIn, typeof(Port_Material));
            info.AddValue("Out1", PortOut1, typeof(Port_Material));
            info.AddValue("Out2", PortOut2, typeof(Port_Material));

            info.AddValue("active", actives, typeof(List<bool>));
        }

        public PortList ActiveOutPorts()
        {
            PortList res = new();

            Add(PortIn);
            if (PortOut1.IsConnected)
                res.Add(PortOut1);
            if (PortOut2.IsConnected)
                res.Add(PortOut2);

            return res;
        }

        public override bool Solve()
        {
            if (!PortIn.cc.MoleFractionsValid)
                return false;

            int NoComps = PortIn.cc.Count;
            Temperature T = PortIn.T_.Value;

            guesses = new double[NoComps];
            double[] GRT = new double[NoComps];

            double hFormIn = PortIn.HForm25();

            Balance.Calculate(this);
            while (FlashAllPorts() > 0)
            {
                //   Balance.Calculate(this);
            }

            PortOut1.cc.Clear();
            PortOut2.cc.Clear();
            PortOut1.cc.Add(PortIn.cc);
            PortOut2.cc.Add(PortIn.cc);

            if (NoComps == 2)
            {
                Gibbs G;
                int count = 0;
                double[] rx = new double[] { -1, 1 };
                double x;
                double x1;

                do
                {
                    count++;
                    G = IdealGas.StreamGibbsFormation(PortIn.cc, T, rx);
                    double lnKa = -G / (ThermodynamicsClass.Rgas * T.BaseValue);
                    x = 1 / (Math.Exp(lnKa) + 1);
                    x1 = 1 - x;

                    PortOut1.Props.Clear();
                    PortOut1.SetMoleFractions(new double[] { x, x1 });
                    PortOut1.cc.Origin = SourceEnum.UnitOpCalcResult;
                    PortOut1.SetPortValue(ePropID.P, PortIn.P_, SourceEnum.UnitOpCalcResult);
                    PortOut1.SetPortValue(ePropID.MOLEF, PortIn.MolarFlow_, SourceEnum.UnitOpCalcResult);
                    PortOut1.SetPortValue(ePropID.H, PortIn.H_, SourceEnum.UnitOpCalcResult);
                    PortOut1.Flash();
                    if (Math.Abs(PortOut1.T_ - T) < 0.0001)
                        break;
                    T = PortOut1.T_.Value;
                } while (count < 10);

                PortOut2.SetMoleFractions(new double[] { x, x1 });
                PortOut2.cc.Origin = SourceEnum.UnitOpCalcResult;
                PortOut2.SetPortValue(ePropID.P, PortIn.P_, SourceEnum.UnitOpCalcResult);
                PortOut2.SetPortValue(ePropID.MOLEF, 0, SourceEnum.UnitOpCalcResult);
                PortOut2.SetPortValue(ePropID.T, T, SourceEnum.UnitOpCalcResult);
            }
            else
            {
                int count = 0;
                do
                {
                    count++;
                    Gibbs = IdealGas.StreamGibbsFormation(PortIn.cc, T);
                    // Initialise guesses
                    for (int x = 0; x < NoComps; x++)
                    {
                        guesses[x] = 2;
                        GRT[x] = Gibbs[x] / (ThermodynamicsClass.Rgas * T);
                    }

                    //GRT = new  double []{-24.089,0,-23.178,-47.612 }; // test working

                    bool end;
                    do
                    {
                        double[,] leftSide = LeftSide();
                        double[] rightSide = RightSide(PortIn.MolarFlow_.Value, GRT);

                        Matrix left = new(leftSide);
                        Matrix right = new(rightSide);
                        IMatrix leftinverse = left.Inverse;
                        double[] Mult = leftinverse.Multiply(right).ToArray();

                        List<double> lnGuesses = new();
                        for (int i = 0; i < PortIn.cc.Count; i++)
                        {
                            lnGuesses.Add(Math.Log(guesses[i]));
                        }

                        double sum = 0;
                        double[] oldguesses = guesses;
                        for (int i = 0; i < guesses.Length; i++)
                        {
                            guesses[i] = Math.Exp(lnGuesses[i] + Mult[i] * 0.5);
                            sum += Math.Abs(guesses[i] - oldguesses[i]);
                        }

                        if (sum > 0.0001)
                            end = true;
                        else
                            end = false;
                    } while (end);

                    guesses = guesses.Normalise();

                    //PortOut1.Props.Clear();
                    //PortOut1.Components.Clear();
                    PortOut1.SetMoleFractions(guesses);
                    PortOut1.cc.Origin = SourceEnum.UnitOpCalcResult;
                    PortOut1.SetPortValue(ePropID.P, PortIn.P_, SourceEnum.UnitOpCalcResult);
                    PortOut1.SetPortValue(ePropID.MOLEF, PortIn.MolarFlow_, SourceEnum.UnitOpCalcResult);
                    PortOut1.SetPortValue(ePropID.H, PortIn.H_, SourceEnum.UnitOpCalcResult);
                    PortOut1.Flash();
                    //PortIn.H.IsKnown;
                    if (Math.Abs(PortOut1.T_ - T) < 0.0001)
                        break;
                    T = PortOut1.T_.Value;
                } while (count < 10);

                PortOut2.SetMoleFractions(guesses);
                PortOut2.cc.Origin = SourceEnum.UnitOpCalcResult;
                PortOut2.SetPortValue(ePropID.P, PortIn.P_, SourceEnum.UnitOpCalcResult);
                PortOut2.SetPortValue(ePropID.MOLEF, 0, SourceEnum.UnitOpCalcResult);
                PortOut2.SetPortValue(ePropID.T, T, SourceEnum.UnitOpCalcResult);
            }

            FlashAllPorts();

            return true;
        }

        public double[,] LeftSide()
        {
            TotalElements = PortIn.Elements();

            foreach (var item in TotalElements.Keys)
            {
                if (TotalElements[item] == 0)
                    TotalElements.Remove(item);
            }

            List<string> elementList = TotalElements.Keys.ToList();
            int noelemts = TotalElements.Count;
            int NoComponents = PortIn.ComponentList.Count;
            ElementMatrix = new double[NoComponents, noelemts];

            for (int x = 0; x < NoComponents; x++)
            {
                Dictionary<string, int> ComponentElements = PortIn.cc[x].GetElements();

                foreach (var item in ComponentElements.Keys)
                {
                    if (!TotalElements.ContainsKey(item))
                        ComponentElements.Remove(item);
                }

                foreach (var item in ComponentElements)
                {
                    int loc = elementList.IndexOf(item.Key);
                    ElementMatrix[x, loc] = item.Value;
                }
            }

            double[,] Leftside = new double[NoComponents + noelemts + 1, NoComponents + noelemts + 1];

            // Initialise guesses
            for (int x = 0; x < NoComponents; x++)
            {
                Leftside[x, x] = 1;
            }

            for (int x = NoComponents; x < NoComponents + NoComponents; x++)
            {
                for (int y = 0; y < TotalElements.Count; y++)
                {
                    Leftside[x - NoComponents, y + NoComponents] = -ElementMatrix[x - NoComponents, y];
                }
            }

            for (int x = NoComponents; x < NoComponents + noelemts; x++)
            {
                for (int y = 0; y < NoComponents; y++)
                {
                    Leftside[x, y] = guesses[y] * ElementMatrix[y, x - NoComponents];
                }
            }

            for (int y = 0; y < NoComponents; y++)
            {
                Leftside[NoComponents + noelemts, y] = guesses[y];
            }

            for (int i = 0; i < NoComponents; i++)
            {
                if (i < NoComponents)
                    Leftside[i, NoComponents + noelemts] = -1;
            }

            Leftside[NoComponents + noelemts, NoComponents + noelemts] = -guesses.Sum();

            //ViewArray viewArray = new  ViewArray();
            //viewArray
            //(Leftside);

            return Leftside;
        }

        public double[] RightSide(MoleFlow feedmoles, double[] GRT)
        {
            int noelemts = TotalElements.Count;
            int NoComponents = PortIn.ComponentList.Count;

            double[] res = new double[NoComponents + noelemts + 1];

            for (int i = 0; i < NoComponents; i++)
            {
                res[i] = -(GRT[i] + Math.Log(guesses[i] / guesses.Sum()));
            }

            List<double> sumproduct = new();
            for (int element = 0; element < noelemts; element++)
            {
                double temp = 0;
                for (int comp = 0; comp < NoComponents; comp++)
                {
                    temp += guesses[comp] * ElementMatrix[comp, element];
                }
                sumproduct.Add(temp);
            }

            for (int i = 0; i < noelemts; i++)
            {
                res[NoComponents + i] = TotalElements.Values.ToArray()[i] - sumproduct[i];
            }

            for (int i = NoComponents; i < NoComponents + noelemts; i++)
            {
                res[NoComponents + noelemts] = guesses.Sum() - feedmoles;
            }

            return res;
        }
    }
}