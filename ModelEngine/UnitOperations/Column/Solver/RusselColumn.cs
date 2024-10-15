using Extensions;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Units;

namespace RusselColumn
{
    [Serializable]
    public partial class RusselSolver
    {
        public Column column;
        public double JDelta = 0.1;
        public double MaxDeltaT = 250;
        public double TempDampfactor = 1;
        public double CompDampfactor = 1;

        public int WaterLoc;
        private readonly double DefaultRefluxRatio = 3;
        public int[] NoTrays; // No trays in each section
        public double ConvergenceErr;

        public int LiquidStreamCount;
        public int VapourStreamCount;
        public int ConnectingNetLiquidStreamsCount;
        public int ConnectingNetVapourStreamsCount;
        public int PA_Count;
        public double MIND = 1e-10;
        public double MAXD = 10000;

        // private  string docPath ="C:\\";
        // readonly  string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        [STAThread()]
        public bool Solve(Column column)
        {
            //column.SolverOptions.AlphaMethod = ColumnAlphaMethod.Linear;
            //column.SolverOptions.KMethod = ColumnKMethod.Linear;

            column.SolverOptions.AlphaMethod = ColumnAlphaMethod.LogLinear;
            column.SolverOptions.KMethod = ColumnKMethod.LogLinear;
            //column.SolverOptions.AlphaMethod = ColumnAlphaMethod.MA;
            //column.SolverOptions.KMethod = ColumnKMethod.MA;

            //bool  CloseLocalOutputFile = false;
            var watch = Stopwatch.StartNew();

            SetBasicValues(column);

            WaterLoc = column.Components.IndexOf("H2O", column.Components.MoleFractions, out double _);

            NoComps = column.Components.Count;

            if (column.Components.Count == 0)
                return false;

            TotNoTrays = column.TraySections.TotNoTrays();

            if (TotNoTrays == 0)
                return false;

            WaterCompNo = column.Components.GetWaterCompNo();

            int OuterLoopCounter = 0, TemperatureLoopCounter = 0, innerConvergenceLoop;
            bool FirstPass = true;
            double TrayTempError;
            double ErrorSum = 0;
            this.column = column;

            enumCalcResult cres = enumCalcResult.Converged;

            if (column.MainSectionStages is null)
                return false;

            double NoSpecs = column.Specs.GetActiveSpecs().Count();

            // no calcs here
            InitialiseMatrices(column);

            InitTRDMatrix();

            if (!InterpolatePressures())
                return false;

            ResetPAs();

            if (!InitialiseFeeds(column))
                return false;

            if (!column.SolutionConverged)
                column.Thermo.KMethod = enumEquiKMethod.PR78;

            InitialiseStrippers(column);

            if (column.IsReset)
            {
                SetInitialTrayTemps(column);
            }

            InitialisePumpArounds(column);

            if (column.IsReset)
            {
                switch (column.SolverOptions.InitFlowsMethod)
                {
                    case ColumnInitialFlowsMethod.Excel:
                        InitialiseFlows(column);
                        break;

                    case ColumnInitialFlowsMethod.Simple:
                        InitialiseFlowsSimplified(column);
                        break;

                    case ColumnInitialFlowsMethod.Modified:
                        InitialiseFlows2(column);
                        break;
                }
                InitialiseTrayCompositions(column);
                //SetInitialTrayTemps(column);
            }

            if (!UpdateInitialAlphas(column.Thermo, true))
                return false;

            IntialiseStripFactorsEtc();

            ViewArray2 va = new();
            //va.View(TrDMatrix[0]);

            int LoopCount = 0;
            int MaxLoopCount;

#if !ColumnTest

            switch (column.SolverOptions.ColumnInitialiseMethod)
            {
                case ColumnInitialiseMethod.ConvergeOnK:
                    MaxLoopCount = 15;
                    do
                    {
                        LoopCount++;
                        SolveComponentBalance(column);

                        SaveArrayAsCSV(TrDMatrix, @"c:\TrDMatrixSim.csv");
                        SaveArrayAsCSVTransposed(FeedMolarCompFlowsTotal, @"c:\FeedSIm.csv");
                        SaveArrayAsCSVTransposed2(CompLiqMolarFlows, @"c:\CompLiqMolarFlows.csv");

                        //va.View(TrDMatrix[0]);
                        UpdatePredCompositions();
                        UpdateLandV();
                        UpdateCompositions();
                        EstimateTs(column, column.solverOptions.KMethod);
                        UpdateInitialCompositions(1);
                        ConvergenceErr = UpdateTrayTemps(TempDampfactor);  // reset current tray temps

                        if (!UpdateInitialAlphas(column.Thermo))     // update alphas using   new  tray temps
                            return false;
                        if (ConvergenceErr < 10)
                            break;

                        if (FlowSheet.BackGroundWorker != null && FlowSheet.BackGroundWorker.IsBusy)
                            FlowSheet.BackGroundWorker.ReportProgress(1, column);
                    } while (LoopCount < MaxLoopCount);
                    break;

                case ColumnInitialiseMethod.StripFactors:
                    MaxLoopCount = 20;
                    ConvergenceErr = 11;
                    do
                    {
                        LoopCount++;
                        SolveComponentBalance(column);  // va.View(TrDMatrix[0]);
                        UpdatePredCompositions();
                        UpdateLandV();
                        UpdateCompositions();
                        ReEstimateStripFactors();       // Helps if V guesses are too low and Temperature  estimates invalid
                        if (ConvergenceErr < 10)
                            break;
                    } while (LoopCount < MaxLoopCount);
                    break;

                case ColumnInitialiseMethod.Excel:
                    MaxLoopCount = 10;
                    do
                    {
                        LoopCount++;
                        //va.View(TrDMatrix[0]);
                        UpdateCompositions();
                        ReEstimateStripFactors();
                        EstimateTs(column, column.solverOptions.KMethod);
                        UpdateInitialCompositions(1);
                        ConvergenceErr = UpdateTrayTemps(TempDampfactor);  // reset current tray temps
                        if (!UpdateInitialAlphas(column.Thermo))    // update alphas using   new  tray temps
                            return false;
                        if (ConvergenceErr < 10)
                            break;
                    } while (LoopCount < MaxLoopCount);
                    break;

                case ColumnInitialiseMethod.Test:
                    MaxLoopCount = 10;
                    do
                    {
                        LoopCount++;
                        SolveComponentBalance(column);
                        UpdatePredCompositions();
                        //UpdateLandV();
                        UpdateCompositions();
                        ReEstimateStripFactors();
                        UpdateInitialCompositions(1);
                        //err = UpdateTrayTemps(TempDampfactor);  // reset current tray temps
                        if (!UpdateInitialAlphas(column.Thermo))     // update alphas using   new  tray temps
                            return false;
                        //if (err < 10)
                        //     break;
                    } while (LoopCount < MaxLoopCount);
                    break;

                case ColumnInitialiseMethod.None:
                    break;
            }
#endif
            SolveComponentBalance(column);
            UpdatePredCompositions();
            UpdateCompositions();
            EstimateTs(column, column.solverOptions.KMethod);
            Update_FlowFactors(column);

            UpdateTrayTemps(TempDampfactor);  // reset current tray temps
            if (!UpdateAlphas(column.Thermo))
                return false;

            SolveComponentBalance(column);
            UpdatePredCompositions();
            UpdateCompositions();
            EstimateTs(column, column.solverOptions.KMethod);

#if !ColumnTest
            UpdateTrayTemps(TempDampfactor);
#endif
            Update_FlowFactors(column);

            UpdateVapEnthalpyParameters();
            UpdateLiqEnthalpyParameters();
            EnthalpiesUpdate(ref cres);    // Use Predicted Temperatures
            if (cres == enumCalcResult.Failed)
                return false;

            DoCondenserCalcs(column);      // handle different condenser types
            UpdateTrayEnthalpyBalances();

            RemoveTrayHeatBalanceSpecs();
            AddTrayHeatBalancesToSpecs();
            InitialiseSpecs(column);

            column.Specs.Sort();
            CalcErrors(true);  // temperatues are predicted Temperatures
            ProcessActiveSpecs(out BaseErrors, true);

            double factorDamping;
            int massbalanceloop;
            double StartError = BaseErrors.SumSQR();
            int progresscount2 = 0;
            double ErrorSumOld = 0;

            do
            {
                UpdateInitialCompositions(CompDampfactor);
                if (!UpdateInitialAlphas(column.Thermo)) //  Update alpha with new  Tray Temperatures.
                    return false;

                UpdateLiqEnthalpyParameters();
                UpdateVapEnthalpyParameters();

                do // alphaloop
                {
                    TemperatureLoopCounter++;
                    CreateJacobianSF(column, ref cres);
                    SolveJacobian(Jacobian, BaseErrors, ref grads, JacobianSize);
                    column.TraySections.BackupFactors(column);

                    innerConvergenceLoop = 0;
                    do
                    {
                        if (TemperatureLoopCounter != 0)
                            StartError = ErrorSum;

                        factorDamping = 1;
                        massbalanceloop = 0;
                        do // Loop to make sure mass balance doesnt fail, reduce strip factors (e.g. factors may be too large).
                        {
                            column.TraySections.UnBackupFactor(column);
                            UpdateFactors(grads, factorDamping);
                            UpdateColumnBalance(ref ErrorSum, cres);  //  if (cres == enumCalcResult.Failed) // return   false;
                            factorDamping *= 0.1;
                            massbalanceloop++;
                        } while ((double.IsNaN(ErrorSum) || ErrorSum > StartError) && massbalanceloop < 3);

                        if (ErrorSum < StartError)
                            column.TraySections.BackupFactors(column);

                        CalcErrors(true);  // temperatues are predicted Temperatures
                        ProcessActiveSpecs(out BaseErrors, true);

                        CreateJacobianSF(column, ref cres);
                        SolveJacobian(Jacobian, BaseErrors, ref grads, JacobianSize);
                        //column.SolverOptions.SFUpdateMethod = ColumnSFUpdateMethod.Excel;
                        UpdateFactors(grads, factorDamping);
                        UpdateColumnBalance(ref ErrorSum, cres);//Pred Temperatures are Re-estimated
                        //UpdateTrayTemps(TempDampfactor);
                        if (ErrorSum < StartError)
                            column.TraySections.BackupFactors(column);

                        innerConvergenceLoop++;
                    } while (!double.IsNaN(ErrorSum) && ErrorSum > column.SpecificationTolerance && innerConvergenceLoop < column.MaxAlphaLoopIterations);

                    UpdateTrayTemps(TempDampfactor);  // reset current tray temps, should this be hear??  used to update enthalpies
                    UpdateCompositions();
                    UpdateTrayKValues();

                    if (Math.Abs(ErrorSum - ErrorSumOld) / ErrorSum > 0.01)
                    {
                        if (progresscount2 > 10)
                        {
                            progresscount2 = 0;
                            break;
                        }
                        progresscount2++;
                    }
                    ErrorSumOld = ErrorSum;
                } while (ErrorSum > column.SpecificationTolerance && TemperatureLoopCounter < column.MaxTemperatureLoopCount);            // InnerError loop, completed when error small enough

                TemperatureLoopCounter = 0;
                column.Err1 = TemperatureLoopCounter + ": " + SignificantDigits.Round(ErrorSum, 5) + " \r";
                column.ErrHistory1 += column.Err1;

                if (FlowSheet.BackGroundWorker != null && FlowSheet.BackGroundWorker.IsBusy)
                    FlowSheet.BackGroundWorker.ReportProgress(1, column);

                OuterLoopCounter++;

                if (FirstPass) // First Pass only
                {
                    FirstPass = false;
                    TrayTempError = 100;
                }
                else
                {
                    TrayTempError = 0;
                    for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
                    {
                        foreach (Tray tray in column.TraySections[sectionNo])
                        {
                            TrayTempError += Math.Abs(tray.T - tray.Told);
                            tray.Told = tray.T;
                        }
                        TrayTempError /= column.TotNoStages;
                    }
                    /* for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
                     {
                         TraySection section = column.TraySections[sectionNo];
                         foreach (Tray tray in section)
                         {
                             KError += tray.LnKCompOld.SumSQR() - tray.LnKCompOld.SumSQR();
                             tray.LnKCompOld = tray.ln;
                         }
                         KError /= column.TotNoStages;
                     }*/
                }

                column.Err2 = OuterLoopCounter + ": " + SignificantDigits.Round(TrayTempError, 5) + " \r";
                column.ErrHistory2 += column.Err2;

                // if (TrayTempError < 2)
                //     column.TempDampfactor = 0.9;
                //if (FlowSheet.outputFile != null)
                {
                    // FlowSheet.outputFile.WriteLine("Loop " + OuterLoopCounter.ToString() + " " + TrayTempError.ToString("F4") + " " + ErrorSum.ToString("F4"));
                    // FlowSheet.outputFile.WriteLine(column.MaintraySection.stripFact.ToString());
                }
                if (TrayTempError < 5 && OuterLoopCounter > 10) // could be oscillating
                    TempDampfactor = 0.9;

                //if (FlowSheet.BackGroundWorker != null && FlowSheet.BackGroundWorker.IsBusy)
                //     FlowSheet.BackGroundWorker.ReportProgress(1, column);
            }
            while (Math.Abs(TrayTempError) > column.TrayTempTolerance && OuterLoopCounter < column.MaxOuterIterations);  // deg per tray error

            if (double.IsNaN(ErrorSum) || ErrorSum > column.SpecificationTolerance || TrayTempError > column.TrayTempTolerance)
            {
                //if(CloseLocalOutputFile)
                //FlowSheet.outputFile.Close();
                return false;
            }
            else
            {
                if (!UpdateAlphas(column.Thermo))
                    return false;

                FinishoffCondenser();

                UpdatePorts(false);

                //if (FlowSheet.outputFile != null && File.Exists(docPath + "\\log.txt"))
                //    FlowSheet.outputFile.WriteLine("Solution Time: " + watch.ElapsedMilliseconds.ToString());
                //if (CloseLocalOutputFile)
                //    FlowSheet.outputFile.Close();
                return true;
            }
        }

        private void ResetPAs()
        {
            foreach (PumpAround pa in column.PumpArounds)
            {
                if (pa.returnTray is Tray t)
                {
                    t.feed.MolarFlow_.Clear();
                    t.feed.MF_.Clear();
                    t.feed.VF_.Clear();
                }
            }
        }

        public void RemoveTrayHeatBalanceSpecs()
        {
            List<Specification> remove = new();

            foreach (Specification item in column.Specs)
                if (item.engineSpecType == eSpecType.TrayDuty || item.engineSpecType == eSpecType.LLE_KSpec)
                    remove.Add(item);

            foreach (Specification item in remove)
            {
                int loc = column.Specs.IndexOf(item);
                column.Specs.RemoveAt(loc);
            }
        }

        public void SaveArrayAsCSV<T>(T[][][] arrayToSave, string fileName)
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                for (int i = 0; i < arrayToSave.GetUpperBound(0); i++)
                {
                    file.WriteLine();
                    for (int j = 0; j <= arrayToSave[0].GetUpperBound(0); j++)
                    {
                        file.WriteLine();
                        foreach (T item in arrayToSave[i][j])
                        {
                            file.Write(item + ",");
                        }
                    }
                }
            }
        }

        public void SaveArrayAsCSV<T>(T[][] arrayToSave, string fileName)
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                for (int i = 0; i <= arrayToSave.GetUpperBound(0); i++)
                {
                    file.WriteLine();
                    foreach (T item in arrayToSave[i])
                    {
                        file.Write(item + ",");
                    }
                }
            }
        }

        public void SaveArrayAsCSVTransposed<T>(T[][] arrayToSave, string fileName)
        {
            int Rows = arrayToSave.GetUpperBound(0) + 1;
            int Cols = arrayToSave[0].GetUpperBound(0) + 1;

            T[,] arr = new T[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                    arr[i, j] = arrayToSave[i][j];
            }

            using (StreamWriter file = new StreamWriter(fileName))
            {
                for (int i = 0; i < Rows; i++)
                {
                    file.WriteLine();
                    for (int j = 0; j < Cols; j++)
                    {
                        file.WriteLine(arr[i, j].ToString() + ",");
                    }
                }
            }
        }

        public void SaveArrayAsCSVTransposed2<T>(T[][] arrayToSave, string fileName)
        {
            int Rows = arrayToSave.GetUpperBound(0) + 1;
            int Cols = arrayToSave[0].GetUpperBound(0) + 1;

            T[,] arr = new T[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                    arr[i, j] = arrayToSave[i][j];
            }

            using (StreamWriter file = new StreamWriter(fileName))
            {
                for (int j = 0; j < Cols; j++)
                {
                    file.WriteLine();
                    for (int i = 0; i < Rows; i++)
                    {
                        file.WriteLine(arr[i, j].ToString() + ",");
                    }
                }
            }
        }

        public bool IsSolved
        {
            get
            {
                if(column.MainTraySection.TopTray.TrayVapour.IsSolved && column.MainTraySection.BottomTray.TrayLiquid.IsSolved)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}