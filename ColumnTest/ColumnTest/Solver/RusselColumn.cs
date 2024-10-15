using Extensions;
using ModelEngine;
using ModelEngineTest;
using System.Diagnostics;
using Units;
using COMColumn = ModelEngineTest.COMColumn;

namespace RusselColumnTest
{
    [Serializable]
    public partial class RusselSolverTest
    {
        public COMColumn? column;
        public double JDelta = 0.1;
        public double MaxDeltaT = 250;
        public double TempDampfactor = 1;
        public double CompDampfactor = 1;

        public int WaterLoc;
        private readonly double DefaultRefluxRatio = 3;
        public int[]? NoTrays; // No trays in each section
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
        public bool Init(COMColumn column)
        {
            //column.SolverOptions.AlphaMethod = ColumnAlphaMethod.Linear;
            //column.SolverOptions.KMethod = ColumnKMethod.Linear;

            column.SolverOptions.AlphaMethod = COMColumnAlphaMethod.LogLinear;
            column.SolverOptions.KMethod = COMColumnKMethod.LogLinear;
            //column.SolverOptions.AlphaMethod = ColumnAlphaMethod.MA;
            //column.SolverOptions.KMethod = ColumnKMethod.MA;

            //bool  CloseLocalOutputFile = false;
            var watch = Stopwatch.StartNew();

            /*if (FlowSheet.outputFile is null)
            {
                File.Delete(docPath + "\\log.txt");
                if (!File.Exists(docPath + "\\log.txt"))
                    FlowSheet.outputFile = new  StreamWriter(Path.Combine(docPath, "log.txt"));
                else
                    File.OpenWrite(docPath + "\\log.txt");
                CloseLocalOutputFile = true;
            }*/

            int OuterLoopCounter = 0, TemperatureLoopCounter = 0, innerConvergenceLoop;
            bool FirstPass = true;
            double TrayTempError, KError;
            double ErrorSum = 0;

            for (int i = 0; i < column.TraySections.Count; i++)
            {
                TraySectionTest ts = column.TraySections[i];
                for (int trayNo = 0; trayNo < ts.Trays.Count; trayNo++)
                {
                    ts.Trays[trayNo].Name = i.ToString() + ":" + trayNo.ToString();
                }
            }

            SetBasicValues(column);

            WaterLoc = column.Components.IndexOf("H2O", column.Components.MoleFractions, out double _);

            NoComps = column.Components.Count;

            if (column.Components.Count == 0)
                return false;

            TotNoTrays = column.TraySections.TotNoTrays();

            if (TotNoTrays == 0)
                return false;

            WaterCompNo = column.Components.GetWaterCompNo();


            this.column = column;

            enumCalcResult cres = enumCalcResult.Converged;

            if (column.MainSectionStages is null)
                return false;

            double NoSpecs = column.Specs.GetActiveSpecs().Count();
            // no calcs here
            InitialiseMatrices(column);

            NoTrays = new int[column.TraySections.Count];
            for (int i = 0; i < NoTrays.Length; i++)
            {
                NoTrays[i] = NoTraysToThisSection(i);
            }

            InitTRDMatrix();

            column.LiquidStreams = column.ConnectingDraws.LiquidStreams;
            column.VapourStreams = column.ConnectingDraws.VapourStreams;
            column.ConnectingNetLiquidStreams = column.ConnectingNetFlows.LiquidStreams;
            column.ConnectingNetVapourStreams = column.ConnectingNetFlows.VapourStreams;

            LiquidStreamCount = column.LiquidStreams.Count;
            VapourStreamCount = column.VapourStreams.Count;
            ConnectingNetLiquidStreamsCount = column.ConnectingNetLiquidStreams.Count;
            ConnectingNetVapourStreamsCount = column.ConnectingNetVapourStreams.Count;
            PA_Count = column.PumpArounds.Count;

            if (!InterpolatePressures())
                return false;

            if (!InitialiseFeeds(column))
                return false;

            if (!column.SolutionConverged)
                column.Thermo.KMethod = enumEquiKMethod.PR78;

            InitialiseStrippers(column);
            InitialisePumpArounds(column);

            if (column.IsReset)
            {
                switch (column.SolverOptions.InitFlowsMethod)
                {
                    case COMColumnInitialFlowsMethod.Excel:
                        InitialiseFlows(column);
                        break;

                    case COMColumnInitialFlowsMethod.Simple:
                        InitialiseFlowsSimplified(column);
                        break;

                    case COMColumnInitialFlowsMethod.Modified:
                        InitialiseFlows2(column);
                        break;
                }
                InitialiseTrayCompositions(column);
                SetInitialTrayTemps(column);
            }

            if (!UpdateInitialAlphas(column.Thermo, false))
                return false;

            IntialiseStripFactorsEtc();

            ViewArray2 va = new();
            //va.View(TrDMatrix[0]);

            int LoopCount = 0;
            int MaxLoopCount;

#if !ColumnTest

            switch (column.SolverOptions.ColumnInitialiseMethod)
            {
                case COMColumnInitialiseMethod.ConvergeOnK:
                    MaxLoopCount = 15;
                    do
                    {
                        LoopCount++;
                        SolveComponentBalance(column);
                        va.View(TrDMatrix[0]);
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

                case COMColumnInitialiseMethod.StripFactors:
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

                case COMColumnInitialiseMethod.Excel:
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

                case COMColumnInitialiseMethod.Test:
                    MaxLoopCount = 10;
                    do
                    {
                        LoopCount++;
                        SolveComponentBalance(column);
                        UpdatePredCompositions();
                        UpdateLandV();
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

                case COMColumnInitialiseMethod.None:
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

            return true;
        }

        public void OneJacobianStep()
        {
            double StartError = BaseErrors.SumSQR();

            UpdateInitialCompositions(CompDampfactor);
            if (!UpdateInitialAlphas(column.Thermo)) //  Update alpha with new  Tray Temperatures.
                return;

            UpdateLiqEnthalpyParameters();
            UpdateVapEnthalpyParameters();
            enumCalcResult cres = default;

            CreateJacobianSF(column, ref cres);
            SolveJacobian(Jacobian, BaseErrors, ref grads, JacobianSize);


        }

        public void SolveComponenetBalance()
        {
            SolveComponentBalance(column);
            UpdatePredCompositions();
            UpdateCompositions();
        }

        public void Init2()
        {
            int OuterLoopCounter = 0, TemperatureLoopCounter = 0, innerConvergenceLoop;
            bool FirstPass = true;
            double TrayTempError;
            double ErrorSum = 0;

            double factorDamping;
            int massbalanceloop;
            double StartError = BaseErrors.SumSQR();
            int progresscount2 = 0;
            double ErrorSumOld = 0;

            do
            {
                UpdateInitialCompositions(CompDampfactor);
                if (!UpdateInitialAlphas(column.Thermo)) //  Update alpha with new  Tray Temperatures.
                    return;

                UpdateLiqEnthalpyParameters();
                UpdateVapEnthalpyParameters();
                enumCalcResult cres = default;

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
                        UpdateFactors(grads, factorDamping);
                        UpdateColumnBalance(ref ErrorSum, cres);//Pred Temperatures are Re-estimated
                        
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
                        foreach (TrayTest tray in column.TraySections[sectionNo])
                        {
                            TrayTempError += Math.Abs(tray.T - tray.Told);
                            tray.Told = tray.T;
                        }
                        TrayTempError /= column.TotNoStages;
                    }
                }

                column.Err2 = OuterLoopCounter + ": " + SignificantDigits.Round(TrayTempError, 5) + " \r";
                column.ErrHistory2 += column.Err2;

                if (TrayTempError < 5 && OuterLoopCounter > 10) // could be oscillating
                    TempDampfactor = 0.9;

            }
            while (Math.Abs(TrayTempError) > column.TrayTempTolerance && OuterLoopCounter < column.MaxOuterIterations);  // deg per tray error
        }


        [STAThread()]
        public bool Solve(COMColumn column)
        {
            //column.SolverOptions.AlphaMethod = ColumnAlphaMethod.Linear;
            //column.SolverOptions.KMethod = ColumnKMethod.Linear;

            column.SolverOptions.AlphaMethod = COMColumnAlphaMethod.LogLinear;
            column.SolverOptions.KMethod = COMColumnKMethod.LogLinear;
            //column.SolverOptions.AlphaMethod = ColumnAlphaMethod.MA;
            //column.SolverOptions.KMethod = ColumnKMethod.MA;

            //bool  CloseLocalOutputFile = false;
            var watch = Stopwatch.StartNew();

            /*if (FlowSheet.outputFile is null)
            {
                File.Delete(docPath + "\\log.txt");
                if (!File.Exists(docPath + "\\log.txt"))
                    FlowSheet.outputFile = new  StreamWriter(Path.Combine(docPath, "log.txt"));
                else
                    File.OpenWrite(docPath + "\\log.txt");
                CloseLocalOutputFile = true;
            }*/

            for (int i = 0; i < column.TraySections.Count; i++)
            {
                TraySectionTest ts = column.TraySections[i];
                for (int trayNo = 0; trayNo < ts.Trays.Count; trayNo++)
                {
                    ts.Trays[trayNo].Name = i.ToString() + ":" + trayNo.ToString();
                }
            }

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
            double TrayTempError, KError;
            double ErrorSum = 0;
            this.column = column;

            enumCalcResult cres = enumCalcResult.Converged;

            if (column.MainSectionStages is null)
                return false;

            double NoSpecs = column.Specs.GetActiveSpecs().Count();
            // no calcs here
            InitialiseMatrices(column);

            NoTrays = new int[column.TraySections.Count];
            for (int i = 0; i < NoTrays.Length; i++)
            {
                NoTrays[i] = NoTraysToThisSection(i);
            }

            InitTRDMatrix();

            column.LiquidStreams = column.ConnectingDraws.LiquidStreams;
            column.VapourStreams = column.ConnectingDraws.VapourStreams;
            column.ConnectingNetLiquidStreams = column.ConnectingNetFlows.LiquidStreams;
            column.ConnectingNetVapourStreams = column.ConnectingNetFlows.VapourStreams;

            LiquidStreamCount = column.LiquidStreams.Count;
            VapourStreamCount = column.VapourStreams.Count;
            ConnectingNetLiquidStreamsCount = column.ConnectingNetLiquidStreams.Count;
            ConnectingNetVapourStreamsCount = column.ConnectingNetVapourStreams.Count;
            PA_Count = column.PumpArounds.Count;

            if (!InterpolatePressures())
                return false;

            if (!InitialiseFeeds(column))
                return false;

            if (!column.SolutionConverged)
                column.Thermo.KMethod = enumEquiKMethod.PR78;

            InitialiseStrippers(column);
            InitialisePumpArounds(column);

            if (column.IsReset)
            {
                switch (column.SolverOptions.InitFlowsMethod)
                {
                    case COMColumnInitialFlowsMethod.Excel:
                        InitialiseFlows(column);
                        break;

                    case COMColumnInitialFlowsMethod.Simple:
                        InitialiseFlowsSimplified(column);
                        break;

                    case COMColumnInitialFlowsMethod.Modified:
                        InitialiseFlows2(column);
                        break;
                }
                InitialiseTrayCompositions(column);
                SetInitialTrayTemps(column);
            }

            if (!UpdateInitialAlphas(column.Thermo, false))
                return false;

            IntialiseStripFactorsEtc();

            //ViewArray va = new();
            //va.View(TrDMatrix[0]);

            int LoopCount = 0;
            int MaxLoopCount;

#if !ColumnTest

            switch (column.SolverOptions.ColumnInitialiseMethod)
            {
                case COMColumnInitialiseMethod.ConvergeOnK:
                    MaxLoopCount = 15;
                    do
                    {
                        LoopCount++;
                        SolveComponentBalance(column);
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

                case COMColumnInitialiseMethod.StripFactors:
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

                case COMColumnInitialiseMethod.Excel:
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

                case COMColumnInitialiseMethod.Test:
                    MaxLoopCount = 10;
                    do
                    {
                        LoopCount++;
                        SolveComponentBalance(column);
                        UpdatePredCompositions();
                        UpdateLandV();
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

                case COMColumnInitialiseMethod.None:
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
                    KError = 0;
                    for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
                    {
                        foreach (TrayTest tray in column.TraySections[sectionNo])
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
                column.IsSolved = false;
                //if(CloseLocalOutputFile)
                //FlowSheet.outputFile.Close();
                return false;
            }
            else
            {
                if (!UpdateAlphas(column.Thermo))
                    return false;
                Finishoff();
                UpdatePorts(false);
                //if (FlowSheet.outputFile != null && File.Exists(docPath + "\\log.txt"))
                //    FlowSheet.outputFile.WriteLine("Solution Time: " + watch.ElapsedMilliseconds.ToString());
                column.IsSolved = true;
                //if (CloseLocalOutputFile)
                //    FlowSheet.outputFile.Close();
                return true;
            }
        }

        public void RemoveTrayHeatBalanceSpecs()
        {
            List<SpecificationTest> remove = new();

            foreach (SpecificationTest item in column.Specs)
                if (item.engineSpecType == COMeSpecType.TrayDuty || item.engineSpecType == COMeSpecType.LLE_KSpec)
                    remove.Add(item);

            foreach (SpecificationTest item in remove)
            {
                int loc = column.Specs.IndexOf(item);
                column.Specs.RemoveAt(loc);
            }
        }
    }
}