using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Units;
using static gv;

namespace ModelEngine
{
    [Serializable]
    public class HeatExchanger : UnitOperation, ISerializable
    {
        public Heater coldSide;
        public Cooler hotSide;

        public Port coldPortIn;
        public Port coldPortOut;
        public Port hotPortIn;
        public Port hotPortOut;

        public Port_Signal deltaTHI;
        public Port_Signal deltaTHO;

        public bool CounterCurrent { get; set; }

        public double lastLMTD;
        public double tinydt;
        public Port_Signal UA;

        //public  Tuple<string, Tuple<string, string, string>> unitOpMessage;
        private double step;
        private bool break1 = false;

        public HeatExchanger(FlowSheet fs)
        {
            //AddPorts(2, 2, 0);
            ports.Add(new Port(this, "In1_H", FlowDirection.ALL));
            ports.Add(new Port(this, "In1_C", FlowDirection.ALL));
            ports.Add(new Port(this, "Out1_H", FlowDirection.ALL));
            ports.Add(new Port(this, "Out1_C", FlowDirection.ALL));

            ports.Add(new Port_Signal(this, "DeltaP_H", FlowDirection.ALL));
            ports.Add(new Port_Signal(this, "DeltaP_C", FlowDirection.ALL));
            ports.Add(new Port_Signal(this, "DeltaT_H", FlowDirection.ALL));
            ports.Add(new Port_Signal(this, "DeltaT_C", FlowDirection.ALL));

            this.coldSide = new Heater(fs);
            this.hotSide = new Cooler(fs);
            coldSide.parent = this;
            hotSide.parent = this;

            fs.Add(coldSide);
            fs.Add(hotSide);
            // connect the energy ports
            fs.ConnectPorts("HotSide", OUT_PORT + "Q", "ColdSide", "In1" + "Q");
            // export the flow ports
            this.coldPortIn = this.coldSide.GetPort("In1");
            this.hotPortIn = this.hotSide.GetPort("In1");
            this.coldPortOut = this.coldSide.GetPort(OUT_PORT);
            this.hotPortOut = this.hotSide.GetPort(OUT_PORT);
            this.BorrowChildPort(this.coldPortIn, "In1_C");
            this.BorrowChildPort(this.hotPortIn, "In1_H");
            this.BorrowChildPort(this.coldPortOut, OUT_PORT + "_C");
            this.BorrowChildPort(this.hotPortOut, OUT_PORT + "_H");
            // export the delta P ports
            this.ports[DELTAP_PORT + "H"] = this.hotSide.GetPort(DELTAP_PORT);
            this.ports[DELTAP_PORT + "C"] = this.coldSide.GetPort(DELTAP_PORT);
            // create Delta T Ports
            this.deltaTHI = new Port_Signal(this, "THI", FlowDirection.ALL, ModelFlowType.SIG);
            //this.deltaTHI.SetSignalTypeg(DELTAT_VAR);
            this.deltaTHO = new Port_Signal(this, "THO", FlowDirection.ALL);
            //this.deltaTHO.SetSignalType(DELTAT_VAR);
            // default parameters
            this.CounterCurrent = true;
            this.UA = new Port_Signal(this, "UA", FlowDirection.ALL);
            this.lastLMTD = double.NaN;
            this.tinydt = 1E-05;
        }

        public virtual double calcDeltaT(Port hotPort, Port coldPort, Port_Signal dtPort)
        {
            double coldT;
            var hotT = hotPort.T;
            var dt = dtPort.Value;
            if (dtPort.IsKnown)
            {
                if (hotT != null)
                    coldPort.T.SetValue(hotT - dt, SourceEnum.CalcResult);
                else
                {
                    coldT = coldPort.T;
                    if (coldPort.T.IsKnown)
                        hotPort.T.SetValue(coldT + dt, SourceEnum.CalcResult);
                }
            }
            else if (hotPort.T.IsKnown)
            {
                coldT = coldPort.T;
                if (coldPort.T.IsKnown)
                {
                    dt = hotT - coldT;
                    dtPort.T.SetValue(dt, SourceEnum.CalcResult);
                }
            }
            return dt;
        }

        // return   dt1, dt2, LMTD to make the code compatible with older code
        public virtual Tuple<double, double, double> SolveForDeltas()
        {
            double dt1 = double.NaN;
            double dt2 = double.NaN;
            double LMTD = double.NaN;

            if (CounterCurrent)
            {
                dt1 = this.calcDeltaT(this.hotPortIn, this.coldPortOut, this.deltaTHI);
                dt2 = this.calcDeltaT(this.hotPortOut, this.coldPortIn, this.deltaTHO);
            }
            else
            {
                dt1 = this.calcDeltaT(this.hotPortIn, this.coldPortIn, this.deltaTHI);
                dt2 = this.calcDeltaT(this.hotPortOut, this.coldPortOut, this.deltaTHO);
            }

            if (!double.IsNaN(dt1) && !double.IsNaN(dt2))
            {
                if (dt1 >= 0.0 && dt2 >= 0.0)
                {
                    if (Math.Abs(dt1 - dt2) < this.tinydt || dt1 * dt2 <= this.tinydt)
                    {
                        //Use average
                        LMTD = (dt1 + dt2) / 2;
                    }
                    else
                    {
                        //Use ln
                        LMTD = (dt2 - dt1) / Math.Log(dt2 / dt1);
                    }
                }
                else
                {
                    // this.unitOpMessage = Tuple.Create("Temperature Cross", dt1.ToString() + dt2.ToString() + this.GetPath());
                    // throw new  SimError("Temperature Cross", Tuple.Create(dt1, dt2, this.GetPath()).ToString());
                }
            }
            else if (!double.IsNaN(dt1))
            {
                if (dt1 < 0.0)
                {
                    // this.unitOpMessage = Tuple.Create("Temperature Cross", dt1.ToString() + dt2.ToString() + this.GetPath());
                    //  throw new  SimError("Temperature Cross", Tuple.Create(dt1, dt2, this.GetPath()).ToString());
                }
            }
            else if (!double.IsNaN(dt2))
            {
                if (dt2 < 0.0)
                {
                    // this.unitOpMessage = Tuple.Create("Temperature Cross", dt1.ToString() + dt2.ToString() + this.GetPath());
                    // throw new  Error.SimError("Temperature Cross", Tuple.Create(dt1, dt2, this.GetPath()).ToString());
                }
            }
            return Tuple.Create(dt1, dt2, LMTD);
        }

        // Just need to to delta T stuff since children handle everything else
        public override bool Solve(FlowSheet fs)
        {
            bool tIsIn;
            double pIter, p, h, t, P, H, T, PIter;
            bool TIsIn;
            Port_Energy portQ = (Port_Energy)this.hotSide.GetPort(OUT_PORT + "Q");
            var Q = portQ.Value;
            var UA = this.UA.Value;
            double LMTD = double.NaN;
            var _tup_1 = this.SolveForDeltas();
            var dt1 = _tup_1.Item1;
            var dt2 = _tup_1.Item2;
            LMTD = _tup_1.Item3;
            double[] Fracs;
            double[] fracs;

            var isCounterCurrent = true;
            //case  for T known. Put values in UA and Q. This will raise consistency errors if needed
            if (!double.IsNaN(LMTD))
            {
                if (!double.IsNaN(Q))
                {
                    UA = Q / LMTD;
                    this.UA.SetValue(ePropID.UA, UA, SourceEnum.CalcResult);
                }
                else if (!double.IsNaN(UA))
                {
                    Q = UA * LMTD;
                    portQ.SetValue(Q, SourceEnum.CalcResult);
                }
            }
            else if (!double.IsNaN(UA))
            {
                //Anything new  that can be done will depend on UA spec
                //Solve if UA and a T in each side is known
                //Get F
                var F = this.hotPortIn.MolarFlow;
                var f = this.coldPortIn.MolarFlow;
                //Get T
                var T1 = this.hotPortIn.T;
                var T2 = this.hotPortOut.T;
                var t1 = this.coldPortIn.T;
                var t2 = this.coldPortOut.T;
                //If three T known and Q and U known but not the second flow
                if (!double.IsNaN(Q))
                {
                    var lstT = new List<double> { T1, T2, t1, t2 };
                    var nuUnkT = lstT.Count(null);
                    if (nuUnkT == 1)
                    {
                        var nuUnkF = new List<object> { F, f }.Count(null);
                        if (nuUnkF == 1)
                        {
                            //Let's try finding the missing T using   LMTD known
                            LMTD = Q / UA;
                            var idxUnkT = lstT.IndexOf(double.NaN);
                            var u = new Unknowns();
                            var numMethodSetings = new NumericMethodSettings(this);
                            numMethodSetings.solveMethod = EquationSolver.SECANT;
                            //T1
                            var isSpec = true;
                            if (T1 == null)
                            {
                                T1 = T2;
                                isSpec = false;
                            }
                            var unkVar = new SolverVariable("T1", T1, T1, isSpec, 100.0, 0.001, 1E+30);
                            u.AddUnknown(unkVar);
                            //T2
                            isSpec = true;
                            if (T2 == null)
                            {
                                T2 = T1;
                                isSpec = false;
                            }
                            unkVar = new SolverVariable("T2", T2, T2, isSpec, 100.0, 0.001, 1E+30);
                            u.AddUnknown(unkVar);
                            //t1
                            isSpec = true;
                            if (t1 == null)
                            {
                                t1 = t2;
                                isSpec = false;
                            }
                            var unkVar_t1 = new SolverVariable("t1", t1, t1, isSpec, 100.0, 0.001, 1E+30);
                            //t2
                            isSpec = true;
                            if (t2 == null)
                            {
                                t2 = t1;
                                isSpec = false;
                            }
                            var unkVar_t2 = new SolverVariable("t2", t2, t2, isSpec, 100.0, 0.001, 1E+30);
                            if (CounterCurrent)
                            {
                                u.AddUnknown(unkVar_t2);
                                u.AddUnknown(unkVar_t1);
                            }
                            else
                            {
                                u.AddUnknown(unkVar_t1);
                                u.AddUnknown(unkVar_t2);
                            }
                            //LMTD
                            isSpec = true;
                            unkVar = new SolverVariable("LMTD", LMTD, LMTD, isSpec, 100.0, 0.001, 1E+30);
                            u.AddUnknown(unkVar);
                            var _tup_2 = EquationSolver.SolveNonLinearEquations(this, u, numMethodSetings, new double[0]);
                            var x = _tup_2.Item1;
                            var rhs = _tup_2.Item2;
                            var converged = _tup_2.Item3;
                            if (converged)
                            {
                                var foundT = x[idxUnkT];
                                if (idxUnkT == 0)
                                {
                                    this.hotPortIn.T.SetValue(foundT, SourceEnum.CalcResult);
                                }
                                else if (idxUnkT == 1)
                                {
                                    this.hotPortOut.T.SetValue(foundT, SourceEnum.CalcResult);
                                }
                                else if (idxUnkT == 2)
                                {
                                    if (CounterCurrent)
                                    {
                                        foundT = x[idxUnkT + 1];
                                        this.coldPortIn.T.SetValue(foundT, SourceEnum.CalcResult);
                                    }
                                    else
                                    {
                                        this.coldPortIn.T.SetValue(foundT, SourceEnum.CalcResult);
                                    }
                                }
                                else if (idxUnkT == 3)
                                {
                                    if (CounterCurrent)
                                    {
                                        foundT = x[idxUnkT - 1];
                                        this.coldPortOut.T.SetValue(foundT, SourceEnum.CalcResult);
                                    }
                                    else
                                    {
                                        this.coldPortOut.T.SetValue(foundT, SourceEnum.CalcResult);
                                    }
                                }
                                var _tup_3 = this.SolveForDeltas();
                                dt1 = _tup_3.Item1;
                                dt2 = _tup_3.Item2;
                                LMTD = _tup_3.Item3;
                            }
                            return true;
                        }
                    }
                }
                //Dummy loop for breaking if needed
                var stay = 1;
                while (stay > 0)
                {
                    stay = 0;
                    //Upper case  for hot strem. Lower case  for cold stream
                    //Nothing can be done if extensive vars are unknown
                    //F, f = self.hotPortIn.GetPropValue(MOLEFLOW_VAR), self.coldPortIn.GetPropValue(MOLEFLOW_VAR)
                    var Cmps = this.hotPortIn.Components;
                    var cmps = this.coldPortIn.Components;
                    if (F == null || f == null)
                    {
                        break;
                    }
                    if (!Cmps.IsDefined || !cmps.IsDefined)
                    {
                        break;
                    }
                    else
                    {
                        Fracs = Cmps.MolFractions;
                        fracs = cmps.MolFractions;
                    }
                    //No Pressure s, no solution
                    var P1 = this.hotPortIn.P;
                    var P2 = this.hotPortOut.P;
                    var p1 = this.coldPortIn.P;
                    var p2 = this.coldPortOut.P;
                    if (new List<double> { P1, P2, p1, p2 }.Contains(double.NaN))
                    {
                        break;
                    }
                    //Get T
                    //T1, T2 = self.hotPortIn.GetPropValue(T_VAR), self.hotPortOut.GetPropValue(T_VAR)
                    //t1, t2 = self.coldPortIn.GetPropValue(T_VAR), self.coldPortOut.GetPropValue(T_VAR)
                    //Nothing can be done if two T are unknown in the same side
                    if (T1 == null)
                    {
                        if (T2 == null)
                        {
                            break;
                        }
                        else
                        {
                            T = T2;
                            H = this.hotPortOut.H;
                            P = P2;
                            PIter = (double)P1;
                            TIsIn = false;
                        }
                    }
                    else
                    {
                        T = T1;
                        H = this.hotPortIn.H;
                        P = P1;
                        PIter = (double)P2;
                        TIsIn = true;
                    }
                    if (t1 == null)
                    {
                        if (t2 == null)
                        {
                            break;
                        }
                        else
                        {
                            t = t2;
                            h = this.coldPortOut.H;
                            p = p2;
                            pIter = (double)p1;
                            tIsIn = false;
                        }
                    }
                    else
                    {
                        t = t1;
                        h = this.coldPortIn.H;
                        p = p1;
                        pIter = p2;
                        tIsIn = true;
                    }
                    if (TIsIn && tIsIn)
                    {
                        if (T < t)
                        {
                            // this.unitOpMessage = Tuple.Create("HotTLowerThanColdT", T.ToString() + t.ToString() + this.GetPath());
                            // throw new  Error.SimError("HotTLowerThanColdT", Tuple.Create(T, t, this.GetPath()).ToString());
                        }
                    }
                    var hotThermo = this.hotSide.GetThermo();
                    var coldThermo = this.coldSide.GetThermo();
                    var hotInfo = new SideInfo(T, P, H, F, Fracs, TIsIn);
                    var coldInfo = new SideInfo(t, p, h, f, fracs, tIsIn);
                    var scaleFactor = this.deltaTHI.scaleFactor;
                    var _tup_4 = this.SolveForQ(UA, hotInfo, coldInfo, PIter, (double)pIter, scaleFactor);
                    var TIter = _tup_4.Item1;
                    var conv = (bool?)_tup_4.Item2;
                    if (conv != null)
                    {
                        if (TIsIn)
                        {
                            this.hotPortOut.T.SetValue(TIter, SourceEnum.CalcResult);
                        }
                        else
                        {
                            this.hotPortIn.T.SetValue(TIter, SourceEnum.CalcResult);
                        }
                    }
                    else
                    {
                        // this.unitOpMessage = Tuple.Create("CouldNotConvergeUA", UA.ToString() + this.GetPath());
                        // this.InfoMessage("CouldNotConvergeUA", Tuple.Create(UA, this.GetPath()).ToString());
                    }
                    var _tup_5 = this.SolveForDeltas();
                    dt1 = _tup_5.Item1;
                    dt2 = _tup_5.Item2;
                    LMTD = _tup_5.Item3;
                }
            }
            return true;
        }

        // In this case , is is only used for finding a T for LMTD knownn
        public virtual int CalculateRHS(List<double> x, List<double> rhs, List<bool> isFix, List<double> initx, int eqnNo = 0)
        {
            var S1T1 = x[0];
            var S1T2 = x[1];
            var S2T1 = x[2];
            var S2T2 = x[3];
            var LMTD = x[4];
            var scaleT = 100.0;
            //LMTD - CalcLMTD = 0
            rhs[eqnNo] = (LMTD - this.CalcLMTD(S1T1, S2T1, S1T2, S2T2)) / scaleT;
            eqnNo += 1;
            //Eqn's for known vars
            foreach (var idx in Enumerable.Range(0, x.Count))
            {
                if ((bool)isFix[idx])
                {
                    rhs[eqnNo] = (x[idx] - initx[idx]) / scaleT;
                    eqnNo += 1;
                }
            }
            return eqnNo;
        }

        // Calculates LMTD
        public virtual double CalcLMTD(double S1T1, double S2T1, double S1T2, double S2T2)
        {
            var dt1 = S1T1 - S2T1;
            var dt2 = S1T2 - S2T2 + 1E-30;
            if (dt1 * dt2 < 0.0)
            {
                //set the smaller dt to 1e-30
                if (Math.Abs(dt1) > Math.Abs(dt2))
                {
                    //dt2 is smaller
                    dt2 = dt1 / Math.Abs(dt1) * 1E-30;
                }
                else
                {
                    dt1 = dt2 / Math.Abs(dt2) * 1E-30;
                }
            }
            else if (dt1 * dt2 == 0.0)
            {
                dt1 = 1E-30;
                dt2 = dt1;
            }
            if (Math.Abs(dt1 - dt2) < 1E-10)
            {
                dt1 = dt1 + 1E-10;
            }
            return (dt1 - dt2) / (Math.Log(dt1 / dt2) + 1E-30);
        }

        public double GetHXDT(bool TIsIn, bool tIsIn, double T, double t, double TIter, double tIter)
        {
            double dt1, dt2;
            if (this.CounterCurrent)
            {
                if (TIsIn)
                {
                    if (tIsIn)
                    {
                        dt1 = T - tIter;
                        dt2 = TIter - t;
                    }
                    else
                    {
                        dt1 = T - t;
                        dt2 = TIter - tIter;
                    }
                }
                else if (tIsIn)
                {
                    dt1 = TIter - tIter;
                    dt2 = T - t;
                }
                else
                {
                    dt1 = TIter - t;
                    dt2 = T - tIter;
                }
            }
            else if (TIsIn)
            {
                if (tIsIn)
                {
                    dt1 = T - t;
                    dt2 = TIter - tIter;
                }
                else
                {
                    dt1 = T - tIter;
                    dt2 = TIter - t;
                }
            }
            else if (tIsIn)
            {
                dt1 = TIter - t;
                dt2 = T - tIter;
            }
            else
            {
                dt1 = TIter - tIter;
                dt2 = T - t;
            }
            if (dt1 >= 0.0 && dt2 >= 0.0)
            {
                if (Math.Abs(dt1 - dt2) < tinydt || dt1 * dt2 <= tinydt)
                {
                    //Use average
                    return (dt1 + dt2) / 2;
                }
                else
                {
                    //Use ln
                    return (dt2 - dt1) / Math.Log(dt2 / dt1);
                }
            }
            else
            {
                return double.NaN;
            }
        }

        public class SideInfo
        {
            public double T, P, H, F;
            public bool TIsIn;
            public double[] fracs;
            public string Prov { get; internal set; }
            public string Case { get; internal set; }

        public SideInfo(double T, double P, double H, double F, double[] Fracs, bool TIsIn)
        {
            this.T = T;
            this.P = P;
            this.H = H;
            this.F = F;
            this.fracs = Fracs;
        }
    }

    // Runs new ton Raphson mehod to find a Q using   two temps knwon
    //
    //         UA = UA
    //         hotInfo = (T, P, H, F, Fracs, TIsIn) . TIsIn = true if the T is known in the inlet
    //         coldInfo = (t, p, h, f, fracs, tIsIn) . tIsIn = true if the t is known in the inlet
    //         PIter = P hot side of unknown T
    //         pIter = p cold side of unknown t
    //         scaleFactor = scaleFactor of Q
    //
    //         return  s - Tuple with (Q, Convergedbool ean)
    //
    //
    public virtual Tuple<double, bool> SolveForQ(double UA, SideInfo hotInfo, SideInfo coldInfo, double PIter, double pIter, double scaleFactor)
    {
        double stay;
        double try2 = 0;
        double try1 = 0;
        double maxLMTD;
        double minLMTD;
        double curr = 0;
        double dx_dF = 0;
        double dF;
        double  new Error = 0;
        double dx;
        double error = 0;
        double  new LMTD = 0;
        double tIter;
        double TIter = 0;
        double hIter = 0;
        double HIter = 0;
        double Q;
        double LMTD = 0;
        double s;
        double S;
        //Uses a sligthly modified version of the bisection method and a quasi new ton method to converge
        var maxIter = 40;
        var tolerance = 1E-06;
        var tinydt = this.tinydt;
        var minStep = 1E-08;
        //var largest = 1000;
        var damp = 0.9;
        var @switch = 0.01;
        var bigError = 1000;
        //Target = new Q - QIter = 0
        //Load vars
        var _tup_1 = hotInfo;
        var T = hotInfo.T;
        var P = hotInfo.P;
        var H = hotInfo.H;
        var F = hotInfo.F;
        var Fracs = hotInfo.fracs;
        var TIsIn = hotInfo.TIsIn;

        var t = coldInfo.T;
        var p = coldInfo.P;
        var h = coldInfo.H;
        var f = coldInfo.F;
        var fracs = coldInfo.fracs;
        var tIsIn = coldInfo.TIsIn;
        double actualAdjustment = 0;

        //Sign
        if (TIsIn)
            S = -1;
        else
            S = 1;

        if (tIsIn)
            s = 1;
        else
            s = -1;

        //Before anything, check if the last solution is good enough for new ton
        bool? new ton = false;

        if (!double.IsNaN(this.lastLMTD))
        {
            LMTD = this.lastLMTD;
            Q = UA * LMTD;
            HIter = H + S * Q * 3.6 / F;
            hIter = h + s * Q * 3.6 / f;
            try
            {
                var TIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, Fracs, ePropID.T);
                var tIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, fracs, ePropID.T);
                TIter = TIter1;
                tIter = tIter1;
                new LMTD = GetHXDT(TIsIn, tIsIn, T, t, TIter, tIter);
                if (!double.IsNaN(new LMTD))
                {
                    error = (new LMTD - LMTD) / scaleFactor;
                    if (Math.Abs(error) < @switch)
                        new ton = true;

                    //new ton could be done. Get a crude differential
                    dx = 0.01 * scaleFactor;
                    LMTD = this.lastLMTD + dx;
                    Q = UA * LMTD;
                    HIter = H + S * Q * 3.6 / F;
                    hIter = h + s * Q * 3.6 / f;
                    TIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, Fracs, ePropID.T);
                    tIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, fracs, ePropID.T);
                    TIter = TIter1;
                    tIter = tIter1;
                    new LMTD = GetHXDT(TIsIn, tIsIn, T, t, TIter, tIter);
                    if (!double.IsNaN(new LMTD))
                    {
                        new Error = (new LMTD - LMTD) / scaleFactor;
                        dF = new Error - error;
                        dx_dF = dx / dF;
                        curr = this.lastLMTD;
                    }
                    else
                    {
                        new ton = false;
                    }
                }
            }
            catch
            {
                new ton = true;
            }
        }
        if ((bool)new ton)
        {
            //estimate some boundaries for LMTD
            if (!this.CounterCurrent)
            {
                //Minimum LMTD
                minLMTD = Math.Abs(T - t) / 2;
                //Max LMTD
                if (TIsIn && tIsIn || !TIsIn && !tIsIn)
                {
                    maxLMTD = Math.Abs(T - t);
                }
                else
                {
                    //Something relatively large
                    maxLMTD = 2 * Math.Abs(T - t);
                }
            }
            else if (TIsIn && tIsIn)
            {
                minLMTD = tinydt;
                maxLMTD = Math.Abs(T - t);
            }
            else if (!TIsIn && !tIsIn)
            {
                minLMTD = Math.Abs(T - t);
                //Something relatively large
                maxLMTD = 3 * Math.Abs(T - t);
            }
            else
            {
                minLMTD = Math.Abs(T - t) / 2;
                //Something relatively large
                maxLMTD = 2 * Math.Abs(T - t);
            }
            //Test if values can be obtained in between the boundary
            error = double.NaN;
            while (true)
            {
                var step = (maxLMTD - minLMTD) / 2.0;
                LMTD = minLMTD + step;
                Q = UA * LMTD;
                HIter = H + S * Q * 3.6 / F;
                hIter = h + s * Q * 3.6 / f;
                try
                {
                    var TIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, Fracs, ePropID.T);
                    var tIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, Fracs, ePropID.T);
                    TIter = TIter1;
                    tIter = tIter1;
                    new LMTD = GetHXDT(TIsIn, tIsIn, T, t, TIter, tIter);
                    if (!double.IsNaN(new LMTD))
                        error = (new LMTD - LMTD) / scaleFactor;

                    //The flash was calculated properly. Now lets make sure the error doesn't get reduced in the max boundary
                    if (double.IsNaN(error))
                        error = bigError;

                    break;
                }
                catch
                {
                    //Max is too big, reduce it a little
                    maxLMTD *= damp;
                }
                if (maxLMTD <= tinydt)
                {
                    //Couldnt even initialize
                    this.lastLMTD = double.NaN;
                    return null;
                }
            }
            //Pick three point s evenly distributed within temporary boundaries
            step *= 0.5;
            curr = LMTD;
            try1 = LMTD + step;
            try2 = LMTD - step;
        }
        Tuple<int, double, double, double, double, double, double> infoBisect = null;
        var canSwitch = 1;
        var iter = 0;
        while (iter <= maxIter)
        {
            iter += 1;
            var lastCurr = curr;
            //Use quasi new ton when close to solution
            if (new ton != null)
            {
                var adjustment = -error * dx_dF;
                double stepLength = 1.0;
                while (true)
                {
                    actualAdjustment = stepLength * adjustment;
                    LMTD = curr + actualAdjustment;
                    Q = UA * LMTD;
                    HIter = H + S * Q * 3.6 / F;
                    hIter = h + s * Q * 3.6 / f;
                    try
                    {
                        var TIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, Fracs, ePropID.T);
                        var tIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, fracs, ePropID.T);
                        TIter = TIter1;
                        tIter = tIter1;
                        new LMTD = GetHXDT(TIsIn, tIsIn, T, t, TIter, tIter);
                        if (!double.IsNaN(new LMTD))
                        {
                            new Error = (new LMTD - LMTD) / scaleFactor;
                            if (Math.Abs(new Error) < Math.Abs(error))
                                break;
                        }
                    }
                    finally
                    {
                        // errors did not go down - back down step size
                        if (stepLength < minStep) // step size too small - bail
                            break1 = true;

                        stepLength /= 4.0;
                    }
                    if (break1)
                        break;
                }
                if (Math.Abs(new Error) > Math.Abs(error))
                {
                    if (infoBisect != null)
                    {
                        //Back to bisecting
                        iter = infoBisect.Item1;
                        curr = infoBisect.Item2;
                        try1 = infoBisect.Item3;
                        try2 = infoBisect.Item4;
                        new Error = infoBisect.Item5;
                        error = infoBisect.Item6;
                        step = infoBisect.Item7;
                    }
                    else
                        break;
                }
                else
                {
                    // update rate of change
                    dF = new Error - error;
                    dx = actualAdjustment;
                    dx_dF = dx / dF;
                    curr = new LMTD;
                }
            }
            else
            {
                //Far using   solution. Do bisection
                step *= 0.5;
                new Error = double.NaN;
                //Try the first guess
                LMTD = try1;
                Q = UA * LMTD;
                HIter = H + S * Q * 3.6 / F;
                hIter = h + s * Q * 3.6 / f;
                try
                {
                    var TIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, Fracs, ePropID.T);
                    var tIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, fracs, ePropID.T);
                    TIter = TIter1;
                    tIter = tIter1;
                    new LMTD = GetHXDT(TIsIn, tIsIn, T, t, TIter, tIter);
                    if (!double.IsNaN(new LMTD))
                    {
                        new Error = (new LMTD - LMTD) / scaleFactor;
                    }
                    if (double.IsNaN(new Error))
                    {
                        stay = 1;
                    }
                    else if (Math.Abs(new Error) > Math.Abs(error))
                    {
                        stay = 1;
                    }
                    else
                    {
                        stay = 0;
                        curr = LMTD;
                        if (new Error* error <= 1.0)
                            {
                            //Sign was crossed, give a smaller step using   last solution
                            if (curr > try1)
                            {
                                try1 = LMTD + step;
                                try2 = LMTD - step;
                            }
                            else
                            {
                                try1 = LMTD - step;
                                try2 = LMTD + step;
                            }
                        }
                            else
                        {
                            //Error was reduced, but sign didn't change.
                            if (curr > try1)
                            {
                                try1 = LMTD - step;
                                try2 = LMTD + step;
                            }
                            else
                            {
                                try1 = LMTD + step;
                                try2 = LMTD - step;
                            }
                        }
                    }
                }
                catch
                {
                    stay = 1;
                }
                if (!double.IsNaN(stay))
                {
                    //Try second guess
                    LMTD = try2;
                    Q = UA * LMTD;
                    HIter = H + S * Q * 3.6 / F;
                    hIter = h + s * Q * 3.6 / f;
                    try
                    {
                        var TIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, Fracs, ePropID.T);
                        var tIter1 = ThermoAdmin.GetProperties(Flowsheet.ComponentList, fracs, ePropID.T);
                        TIter = TIter1;
                        tIter = tIter1;
                        new LMTD = GetHXDT(TIsIn, tIsIn, T, t, TIter, tIter);

                        if (!double.IsNaN(new LMTD))
                            new Error = (new LMTD - LMTD) / scaleFactor;
                        if (double.IsNaN(new Error))
                            stay = 1;
                        else if (Math.Abs(new Error) > Math.Abs(error))
                            stay = 1;
                        else
                        {
                            stay = 0;
                            curr = LMTD;
                            if (new Error* error <= 1.0)
                                {
                                //Sign was crossed, give a smaller step using   last solution
                                if (curr > try1)
                                {
                                    try1 = LMTD + step;
                                    try2 = LMTD - step;
                                }
                                else
                                {
                                    try1 = LMTD - step;
                                    try2 = LMTD + step;
                                }
                            }
                                else
                            {
                                //Error was reduced, but sign didn't change.
                                if (curr > try1)
                                {
                                    try1 = LMTD - step;
                                    try2 = LMTD + step;
                                }
                                else
                                {
                                    try1 = LMTD + step;
                                    try2 = LMTD - step;
                                }
                            }
                        }
                    }
                    catch
                    {
                        stay = 1;
                    }
                    if (stay != null)
                    {
                        //new  guesses will be closer to current value
                        try1 = curr - step;
                        try2 = curr + step;
                        new Error = error;
                    }
                }
                //Check if it can switch to new ton
                if (Math.Abs(new Error) < @switch && iter > 3 && canSwitch == 1)
                {
                    dx = curr - lastCurr;
                    dF = new Error - error;
                    if (Math.Abs(dx) > tinydt && Math.Abs(dF) > tinydt / scaleFactor)
                    {
                        new ton = true;
                        canSwitch = 0;
                        dx_dF = dx / dF;
                        infoBisect = Tuple.Create(iter, curr, try1, try2, new Error, error, step);
                    }
                }
                //If step is too small leave
                if (step < minStep)
                {
                    // step size too small - bail
                    break;
                }
            }
            if (Math.Abs(new Error / tolerance) <= 1.0)
            {
                this.lastLMTD = LMTD;
                return Tuple.Create(TIter, true);
            }
            error = new Error;
        }
        //Just return   the last value used flagged as not converged.
        //Do not raise any errors or messages
        this.lastLMTD = double.NaN;
        return Tuple.Create(TIter, false);
    }
}
}