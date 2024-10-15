using MathNet.Numerics.RootFinding;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ModelEngine
{
    public partial class FlowSheet
    {
        private readonly RecycleMethod recMethod = RecycleMethod.DirectSub;

        private enum RecycleMethod
        { DirectSub, Wegstein }

        public FlowSheet CloneDeep()
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            IFormatter formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            using (var stream = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                formatter.Serialize(stream, this);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                stream.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                return (FlowSheet)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            }
        }

        public bool SolveInner(bool singlestep = false)
        {
            int coutIter = 0;
            UnitOperation uo;
            if (solveStack.Count == 0)
                return false;
            do
            {
                do
                {
                    uo = (UnitOperation)solveStack.Pop();
                } while (uo is null && !uo.IsActive && solveStack.Count > 0);

                if (uo.IsDirty || !uo.IsSolved)
                {
                    //if (backgroundworker != null && backgroundworker.CancellationPending)
                    //    return false;

                    switch (uo)
                    {
                        case HXSubFlowSheet sfs:
                            if (solveStack.CountMainFlowsheetUO() >= 0) // do all main flowsheet objects first if possible
                            {
                                if (uo.Name == "HALT")
                                    Debugger.Break();
                                //sfs.FlashAllPorts();
                                //sfs.EraseCalcValues(SourceEnum.PortCalcResult);
                                //sfs.EraseCalcValues(SourceEnum.UnitOpCalcResult); // seems to be required
                                sfs.ResetSpecs();
                                //Debug.Print("Solving: " + uo.Name);
                                if (uo.IsActive && sfs.Solve() && backgroundworker != null)
                                    backgroundworker.ReportProgress(0);
                                solveStack.Remove(sfs);
                                sfs.IsDirty = false;
                            }
                            else
                            {
                                solveStack.Push(uo);
                                solveStack.MoveFirstToLast();
                                continue;
                            }
                            break;

                        case COlSubFlowSheet sfs:
                            if (solveStack.CountMainFlowsheetUO() == 0) // do all main flowsheet object s first if possible
                            {
                                if (uo.Name == "HALT")
                                    Debugger.Break();
                                //sfs.FlashAllPorts();
                                //sfs.EraseCalcValues(SourceEnum.PortCalcResult);
                                //sfs.EraseCalcValues(SourceEnum.UnitOpCalcResult);
                                sfs.ResetSpecs();
                                Debug.Print("Solving: " + uo.Name);
                                if (uo.IsActive && sfs.Solve() && backgroundworker != null)
                                    backgroundworker.ReportProgress(0, sfs);
                                solveStack.Remove(sfs);
                                sfs.IsDirty = false;
                            }
                            else
                            {
                                solveStack.Push(uo);
                                solveStack.MoveFirstToLast();
                                continue;
                            }
                            break;

                        case StreamSignal ss:
                            ss.FlashAllPorts();
                            break;

                        default:
                            Debug.Print("Solving: " + uo.Name);
                            // private  readonly  object  balanceLock = new  object ();
                            if (uo.IsActive)
                            {
                                if (uo.Name == "HALT")
                                    Debugger.Break();
                                lock (uo)
                                {
                                    if (uo is StreamMaterial sm)
                                    {
                                        if (!sm.Ports[0].IsConnected)
                                            sm.FlashAllPorts();
                                    }
                                    else
                                    {
                                        uo.EraseValuesCalcFromThisUO();
                                        uo.Solve();
                                    }
                                }
                            }
                            else
                                solveStack.Remove(uo);
                            uo.IsDirty = false;

                            if (backgroundworker != null && !backgroundworker.CancellationPending)
                                backgroundworker.ReportProgress(0, null);
                            break;
                    }
                }
                //  Debug.WriteLine("Transferring: " + ((UnitOperation)GetUO("RecycleStream")).Ports["Out1"].MolarFlow.origin.ToString());
                if (coutIter > 100)
                    return false;
                coutIter++;
            } while (solveStack.Count > 0 && !singlestep);
            return true;
        }

        public virtual bool Solve(bool singlestep = false)
        {
            bool solveok;
            bool continueSolve = true;
            int IterationCount = 0;
            inconsistencyStack.Clear();

            foreach (Recycle r in recycleList)
            {
                r.EraseAllValues();
                r.PortOut.Flash();
            }

            do
            {
                switch (this)
                {
                    case HeatExchanger2 uo:
                        //uo.EraseCalcValues(SourceEnum.PortCalcResult);
                        EraseCalcEstimates();
                        break;

                    case HXSubFlowSheet _:
                        EraseCalcEstimates();  // calculated variables allready erased on by main flowsheet
                        break;

                    case FlowSheet _:
                        break;
                }

                InitialiseSolve();
                SolveInner(singlestep);

                if (backgroundworker != null && backgroundworker.CancellationPending)
                    return false;

                if (!RecycleSolved())
                {
                    SolveRecycles();
                    continueSolve = true;
                }
                else
                    continueSolve = false;

                if (!AdjustsSolved())
                {
                    SolveAdjusts();
                    continueSolve = true;
                }
                else
                    continueSolve = false;

                IterationCount++;
                if (IterationCount > 100)
                    continueSolve = false;
            } while (continueSolve && !singlestep);

            //CheckConsisitency();

            solveok = true;
            foreach (IUnitOperation item in ModelStack)
            {
                if (!item.IsSolved)
                    solveok = false;
            }

            return solveok;
        }

        public void InitialiseSolve()
        {
            solveStack.Clear();
            foreach (UnitOperation uo in ModelStack)  //Check all of them, resolve the whole flowsheet
            {
                uo.UOChanged -= UOChanged;
                uo.UOChanged += UOChanged; // allow all object s to respond to a port value change

                if (uo is COlSubFlowSheet sfs)
                {
                    sfs.SFSChangedEvent -= Sfs_SFSChanged;
                    sfs.SFSChangedEvent += Sfs_SFSChanged;
                }

                switch (uo)
                {
                    case Recycle recycle:
                        if (!recycleList.Contains(recycle))
                            recycleList.Add(recycle);
                        break;

                    case AdjustObject adjust:
                        //adjust.IterationNO = 0;
                        if (!adjustList.Contains(adjust))
                            adjustList.Add(adjust);
                        break;

                    case Column column:
                        if ((uo.IsDirty || !column.IsSolved) && !solveStack.Contains(uo) && uo.IsActive)
                        {
                            solveStack.Push(uo);
                        }
                        break;

                    default:
                        if ((uo.IsDirty || !uo.IsSolved) && !solveStack.Contains(uo) && uo.IsActive)
                        {
                            solveStack.Push(uo);
                        }
                        break;
                }
            }
        }

        private void Sfs_SFSChanged(object sender, EventArgs e)
        {
            this.solveStack.Push((UnitOperation)sender);
        }

        private new void UOChanged(IUnitOperation UO, EventArgs e)
        {
            if ((UO is not Recycle) && solveStack.Push(UO))
                Debug.Print(UO.Name + " Pushed to Stack");

            // if (UO is SubFlowSheet sfs)
            //     sfs.Sfs_SFSChanged(sfs, e);
        }

        private static BackgroundWorker backgroundworker;
        private static StreamWriter outputFile;

        public virtual bool PreSolve(bool singlestep = false)
        {
            //CheckAllSolveStates(this.ModelStack);

            bool res = false;
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (outputFile = new StreamWriter(Path.Combine(docPath, "log.txt")))
            {
                res = Solve(singlestep);
            }

            CheckConsisitency();
            return res;
        }

        public bool SolveOuter(Func<double, double> f, double lowerBound, double upperBound)
        {
            Brent.TryFindRoot(f, lowerBound, upperBound, 1e-3, 100, out double root);
            return false;
        }

        private void SolveRecycles()
        {
            if (recMethod == RecycleMethod.DirectSub)
            {
                foreach (Recycle item in recycleList)
                {
                    if (item.IsActive)
                    {
                        //if(item.Name=="HALT")
                        //    Debugger.Break();
                        item.Solve();
                        // item.TransferPortData(true); // Transfer new  values
                    }
                }
            }
        }

        private void SolveAdjusts()
        {
            foreach (AdjustObject item in adjustList)
            {
                if (item.IsActive && !item.IsSolved)
                {
                    item.Solve();
                    //  item.TransferPortData(true); // Transfer new  values
                }
            }
        }

        private bool RecycleSolved()
        {
            foreach (Recycle item in recycleList)
                if (!item.isSolved())
                    return false;

            return true;
        }

        private bool AdjustsSolved()
        {
            foreach (AdjustObject item in adjustList)
                if (!item.IsSolved)
                    return false;

            return true;
        }

        public static void SetEstimateMode()
        {
            CalcType = SourceEnum.CalcEstimate;
            TransferType = SourceEnum.TransEstimate;
            valuemode = eValueMode.Estimates;
        }

        public static void SetNormalMode()
        {
            CalcType = SourceEnum.UnitOpCalcResult;
            TransferType = SourceEnum.Transferred;
            valuemode = eValueMode.Normal;
        }

        public static void SetOverwriteEstimatesMode()
        {
            CalcType = SourceEnum.UnitOpCalcResult;
            TransferType = SourceEnum.Transferred;
            valuemode = eValueMode.OverwriteEstimates;
        }

        public void CheckConsisitency()
        {
            if (inconsistencyStack.Count > 0 && this is not HXSubFlowSheet)
            {
                InconsistencyForm form = new(this);
                form.ShowDialog();
            }
        }

        public void ConnectPorts(BaseStream stream, Port p)
        {
            p.ConnectPort(stream);
        }

        public string ConnectPorts(Port a, Port b)
        {
            a.StreamPort = b;
            return a.Name + ": " + b.Name;
        }

        public string ConnectPorts(string str1, string str2, string str3, string str4)
        {
            throw new NotImplementedException();
        }

        public static string ConnectPorts(UnitOperation mixer, string portNameOut, UnitOperation flash, string portNameIn)
        {
            Port p = mixer.Ports[portNameOut];
            Port p2 = flash.Ports[portNameIn];
            p.StreamPort = p2;

            return mixer.Name + ": " + portNameOut + " " + flash.Name + ": " + portNameIn;
        }
    }
}