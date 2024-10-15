using Extensions;
using ModelEngine;
using ModelEngineTest;
using RusselColumnTest;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Units;
using Units.UOM;

namespace COMColumnNS
{
    [Guid(ColumnTestGuids.ServerClass)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public partial class ColumnTest : IColumnTest
    {
        private COMColumn column;
        private enumCalcResult cres;

        public RusselSolverTest solver
        {
            get
            {
                return column.Russel;
            }
        }

        public ColumnTest()
        {
            // Debugger.Launch();
            column = new();
            Start(60);
        }

        public double TestCritT()
        {
            //Debugger.Launch();
            return 201;
        }

        public object GetKValues()
        {

            return column.MainTraySection.Kcomp;
        }

        public object GetT()
        {

            // Debugger.Launch();
            return column.MainTraySection.C;
        }

        public object GetCompositions()
        {
            object K = column.MainTraySection.LiqCompositions2;
            return K;
        }

        public object GetPredCompositions()
        {
            object K = column.MainTraySection.LiqCompositions2;
            return K;
        }

        public void UpdateKValues(object Tarray, object composition)   // UPDATE TRAY TEMPS AND CALCULATE KS
        {
            double[] Ts = Tarray as double[];

            if (Ts is null)
                return;

            if (solver is not null && solver.column is not null)
            {
                for (int i = 0; i < solver.column.TotNoStages; i++)
                {
                    TrayTest T = solver.column.TraySections[0].Trays[i];
                    T.T = Ts[i];
                }

                solver.UpdateAlphas(column.Thermo);

                for (int i = 0; i < solver.column.TotNoStages; i++)
                {
                    for (int x = 0; x < solver.NoComps; x++)
                    {
                        solver.column.MainTraySection.Trays[i].LiqComposition[x] = ((double[,])composition)[i, x];
                    }
                }

                solver.UpdateAlphas(column.Thermo);
            }
        }

        public object ComponentBalance(double T)
        {
            double ErrorSum = 0;
            enumCalcResult cres = enumCalcResult.Converged;
            solver.UpdateColumnBalance(ref ErrorSum, cres);
            double[,] comps = new double[solver.column.TotNoStages, solver.NoComps];

            for (int i = 0; i < solver.column.TotNoStages; i++)
            {
                for (int x = 0; x < solver.NoComps; x++)
                {
                    comps[i, x] = solver.column.MainTraySection.Trays[i].LiqCompositionPred[x];
                }
            }
            return comps;
        }

        public bool InitialiseFlowsETC()
        {
            //column.SolverOptions.AlphaMethod = ColumnAlphaMethod.Linear;
            //column.SolverOptions.KMethod = ColumnKMethod.Linear;
            Debugger.Launch();
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

            solver.SetBasicValues(column);

            solver.WaterLoc = column.Components.IndexOf("H2O", column.Components.MoleFractions, out double _);

            solver.NoComps = column.Components.Count;

            if (column.Components.Count == 0)
                return false;

            solver.TotNoTrays = column.TraySections.TotNoTrays();

            if (solver.TotNoTrays == 0)
                return false;

            solver.WaterCompNo = column.Components.GetWaterCompNo();

            int OuterLoopCounter = 0, TemperatureLoopCounter = 0, innerConvergenceLoop;
            bool FirstPass = true;
            double TrayTempError, KError;
            double ErrorSum = 0;
            //this.column = column;

            enumCalcResult cres = enumCalcResult.Converged;

            if (column.MainSectionStages is null)
                return false;

            double NoSpecs = column.Specs.GetActiveSpecs().Count();
            // no calcs here
            solver.InitialiseMatrices(column);

            solver.NoTrays = new int[column.TraySections.Count];
            for (int i = 0; i < solver.NoTrays.Length; i++)
            {
                solver.NoTrays[i] = solver.NoTraysToThisSection(i);
            }

            solver.InitTRDMatrix();

            column.LiquidStreams = column.ConnectingDraws.LiquidStreams;
            column.VapourStreams = column.ConnectingDraws.VapourStreams;
            column.ConnectingNetLiquidStreams = column.ConnectingNetFlows.LiquidStreams;
            column.ConnectingNetVapourStreams = column.ConnectingNetFlows.VapourStreams;

            solver.LiquidStreamCount = column.LiquidStreams.Count;
            solver.VapourStreamCount = column.VapourStreams.Count;
            solver.ConnectingNetLiquidStreamsCount = column.ConnectingNetLiquidStreams.Count;
            solver.ConnectingNetVapourStreamsCount = column.ConnectingNetVapourStreams.Count;
            solver.PA_Count = column.PumpArounds.Count;

            if (!solver.InterpolatePressures())
                return false;

            if (!solver.InitialiseFeeds(column))
                return false;

            if (!column.SolutionConverged)
                column.Thermo.KMethod = enumEquiKMethod.PR78;

            RusselSolverTest.InitialiseStrippers(column);
            RusselSolverTest.InitialisePumpArounds(column);

            if (column.IsReset)
            {
                switch (column.SolverOptions.InitFlowsMethod)
                {
                    case COMColumnInitialFlowsMethod.Excel:
                        solver.InitialiseFlows(column);
                        break;

                    case COMColumnInitialFlowsMethod.Simple:
                        solver.InitialiseFlowsSimplified(column);
                        break;

                    case COMColumnInitialFlowsMethod.Modified:
                        solver.InitialiseFlows2(column);
                        break;
                }
                solver.InitialiseTrayCompositions(column);
                solver.SetInitialTrayTemps(column);
            }

            if (!solver.UpdateInitialAlphas(column.Thermo, false))
                return false;

            solver.IntialiseStripFactorsEtc();

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
                        solver.SolveComponentBalance(column);
                        //va.View(TrDMatrix[0]);
                        solver.UpdatePredCompositions();
                        solver.UpdateLandV();
                        solver.UpdateCompositions();
                        solver.EstimateTs(column, column.solverOptions.KMethod);
                        solver.UpdateInitialCompositions(1);
                        solver.ConvergenceErr = solver.UpdateTrayTemps(solver.TempDampfactor);  // reset current tray temps

                        if (!solver.UpdateInitialAlphas(column.Thermo))     // update alphas using   new  tray temps
                            return false;
                        if (solver.ConvergenceErr < 10)
                            break;

                        if (FlowSheet.BackGroundWorker != null && FlowSheet.BackGroundWorker.IsBusy)
                            FlowSheet.BackGroundWorker.ReportProgress(1, column);
                    } while (LoopCount < MaxLoopCount);
                    break;

                case COMColumnInitialiseMethod.StripFactors:
                    MaxLoopCount = 20;
                    solver.ConvergenceErr = 11;
                    do
                    {
                        LoopCount++;
                        solver.SolveComponentBalance(column);  // va.View(TrDMatrix[0]);
                        solver.UpdatePredCompositions();
                        solver.UpdateLandV();
                        solver.UpdateCompositions();
                        solver.ReEstimateStripFactors();       // Helps if V guesses are too low and Temperature  estimates invalid
                        if (solver.ConvergenceErr < 10)
                            break;
                    } while (LoopCount < MaxLoopCount);
                    break;

                case COMColumnInitialiseMethod.Excel:
                    MaxLoopCount = 10;
                    do
                    {
                        LoopCount++;
                        //va.View(TrDMatrix[0]);
                        solver.UpdateCompositions();
                        solver.ReEstimateStripFactors();
                        solver.EstimateTs(column, column.solverOptions.KMethod);
                        solver.UpdateInitialCompositions(1);
                        solver.ConvergenceErr = solver.UpdateTrayTemps(solver.TempDampfactor);  // reset current tray temps
                        if (!solver.UpdateInitialAlphas(column.Thermo))    // update alphas using   new  tray temps
                            return false;
                        if (solver.ConvergenceErr < 10)
                            break;
                    } while (LoopCount < MaxLoopCount);
                    break;

                case COMColumnInitialiseMethod.Test:
                    MaxLoopCount = 10;
                    do
                    {
                        LoopCount++;
                        solver.SolveComponentBalance(column);
                        solver.UpdatePredCompositions();
                        solver.UpdateLandV();
                        solver.UpdateCompositions();
                        solver.ReEstimateStripFactors();
                        solver.UpdateInitialCompositions(1);
                        //err = UpdateTrayTemps(TempDampfactor);  // reset current tray temps
                        if (!solver.UpdateInitialAlphas(column.Thermo))     // update alphas using   new  tray temps
                            return false;
                        //if (err < 10)
                        //     break;
                    } while (LoopCount < MaxLoopCount);
                    break;

                case COMColumnInitialiseMethod.None:
                    break;
            }
#endif
            solver.SolveComponentBalance(column);
            solver.UpdatePredCompositions();
            solver.UpdateCompositions();
            solver.EstimateTs(column, column.solverOptions.KMethod);
            solver.Update_FlowFactors(column);

            solver.UpdateTrayTemps(solver.TempDampfactor);  // reset current tray temps
            if (!solver.UpdateAlphas(column.Thermo))
                return false;

            solver.SolveComponentBalance(column);
            solver.UpdatePredCompositions();
            solver.UpdateCompositions();
            solver.EstimateTs(column, column.solverOptions.KMethod);

#if !ColumnTest
            solver.UpdateTrayTemps(solver.TempDampfactor);
#endif
            solver.Update_FlowFactors(column);

            solver.UpdateVapEnthalpyParameters();
            solver.UpdateLiqEnthalpyParameters();
            solver.EnthalpiesUpdate(ref cres);    // Use Predicted Temperatures
            if (cres == enumCalcResult.Failed)
                return false;

            solver.DoCondenserCalcs(column);      // handle different condenser types
            solver.UpdateTrayEnthalpyBalances();

            solver.RemoveTrayHeatBalanceSpecs();
            solver.AddTrayHeatBalancesToSpecs();
            solver.InitialiseSpecs(column);

            column.Specs.Sort();
            solver.CalcErrors(true);  // temperatues are predicted Temperatures
            solver.ProcessActiveSpecs(out solver.BaseErrors, true);

            double factorDamping;
            int massbalanceloop;
            double StartError = solver.BaseErrors.SumSQR();
            int progresscount2 = 0;
            double ErrorSumOld = 0;
            return true;
        }

        public bool Start(int NoTrays)
        {
            //Debugger.Launch();
            TestA_C3SplitterHeatPumpVariableTrays(NoTrays);
            return true;
        }

        public void TestA_C3SplitterHeatPumpVariableTrays(int notrays)
        {
            int TrayNo = notrays;
            FlowSheet fs = new();
            Pressure PressTop = new(8.77, PressureUnit.BarA);
            Pressure PressBottom = new Pressure(9.8, PressureUnit.BarA);
            Temperature FreshFeedT = new Temperature(24, TemperatureUnit.Celsius);

            Console.WriteLine("Init ColumnTest ++++++++++++++++++++++++++++++");
            //#Set Thermo

            var cmpNames = new List<string>() { "Propane", "Propene", "i-Butane" };

            foreach (var i in cmpNames)
                fs.AddComponent(i);

            column = new ModelEngineTest.COMColumn(); // re-initialise as DLL is persistent in excel

            column.Thermo.Enthalpy = enumEnthalpy.PR78;
            column.Thermo.UseBIPs = false;

            column.TraySections.Add(new TraySectionTest());
            column.MainTraySection.AddTrays(TrayNo);

            int FeedLocation = (int)(0.72 * TrayNo);

            column.Specs.Add(new SpecificationTest("RR", 15, ePropID.NullUnits, COMeSpecType.RefluxRatio, column[0], column[0].Trays[0], column, false));
            //column.Specs.Add(new Specification("Condenser T", 312 - 273.15, ePropID.T, eSpecType.Temperature, column[0], column[0].Trays[0], column));
            //column.Specs.Add(new Specification("Top Vap", 0.05, ePropID.MOLEF, eSpecType.TrayNetVapFlow, column[0], column[0].Trays[0], column));

            //SideStream ss = column.LiquidSideStreams.Add(new SideStream("Name1", column.MainTraySection, column.MainTraySection.Trays[0]));
            column.Specs.Add(new SpecificationTest("Bottoms", 0.2, ePropID.MOLEF, COMeSpecType.TrayNetLiqFlow, column[0], column[0].Trays.Last()));

            Port_Material FreshFeed = new();
            FreshFeed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 1, SourceEnum.Input);
            FreshFeed.P_ = new StreamProperty(ePropID.P, new Pressure(9.89, PressureUnit.BarA), SourceEnum.Input);
            FreshFeed.T_ = new StreamProperty(ePropID.T, FreshFeedT, SourceEnum.Input);
            //FreshFeed.Q = new StreamProperty(ePropID.Q, 0.3498, SourceEnum.Input);
            FreshFeed.cc.Add(fs.ComponentList);
            FreshFeed.cc.SetMolFractions(new double[] { 0.2, 0.798, 0.002 });
            column.MainTraySection.Trays[FeedLocation].feed = FreshFeed;

            Port_Material preflux = new();
            preflux.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 12.71, SourceEnum.Input);
            preflux.P_ = new StreamProperty(ePropID.P, new Pressure(9.013, PressureUnit.BarG), SourceEnum.Input);
            //preflux.T = new StreamProperty(ePropID.T, 15.77, SourceEnum.Input);
            preflux.Q_ = new StreamProperty(ePropID.Q, 0.1234, SourceEnum.Input);
            preflux.cc.Add(fs.ComponentList);
            preflux.cc.SetMolFractions(new double[] { 0.005, 0.995, 0 });
            column.MainTraySection.Trays.First().feed = preflux;

            column.MainTraySection.TopTray.P.BaseValue = PressTop;
            column.MainTraySection.BottomTray.P.BaseValue = PressBottom;
            column.MainTraySection.CondenserType = COMCondType.None;
            column.MainTraySection.ReboilerType = COMReboilerType.Kettle;

            int requiredspecs = column.RequiredSpecsCount;

            ThermoDynamicOptions thermo = new();
            FreshFeed.Flash(thermo: thermo); // Must Flash the feed ports
            preflux.Flash(thermo: thermo);
            //PrintPortInfo(p, "In1")
            //Debug.Print (column.MaintraySection.CondenserType.ToString());

            var watch = Stopwatch.StartNew();
            bool res = false;

            res = column.Init();
            //res = column.Solve();
            //res = InitialiseFlowsETC();

            Console.WriteLine("Finished Column Test ++++++++++++++++++++++++++++++");
        }

        public void UpdateTAndSolve(object T)
        {
            double[] Ts = T as double[];

            if (Ts is null)
                return;

            if (solver is not null && solver.column is not null)
            {
                for (int i = 0; i < solver.column.TotNoStages; i++)
                {
                    TrayTest tray = solver.column.TraySections[0].Trays[i];
                    tray.T = Ts[i];
                }
            }

            double error = 0;

            if (solver is not null)
                solver.UpdateColumnBalance(ref error, cres);
        }

        public object GetkAtT(object T)
        {
            //Debugger.Launch();

            double[] Ts = T as double[];
            double[,] K = new double[Ts.Length,column.NoComps];

            if (Ts is null)
                return null;

            if (solver is not null && solver.column is not null)
            {
                for (int i = 0; i < solver.column.TotNoStages; i++)
                {
                    TrayTest tray = solver.column.TraySections[0].Trays[i];
                    for (int c = 0; c < column.NoComps; c++)
                    {
                        K[i, c] = tray.K_TestFast(c, Ts[i], COMColumnKMethod.LogLinear);
                    }
                }                    
            }

            return K;
        }

        public object GetL()
        {

            // Debugger.Launch();
            return column.MainTraySection.L;
        }

        public object GetV()
        {

            // Debugger.Launch();
            return column.MainTraySection.V;
        }

        public bool OneJacobianStep()
        {

            // Debugger.Launch();
            solver.OneJacobianStep();
            return true;
        }

        public object GetPredC()
        {

            // Debugger.Launch();
            return column.MainTraySection.TPredC;
        }

        public object GetStripFactors()
        {

            //Debugger.Launch();
            return column.MainTraySection.stripFact;
        }

        public void SetStripFactors(object f)
        {
            double[] stripafcs = (double[])f;
            //Debugger.Launch();

            if (solver is not null && solver.column is not null)
            {
                for (int i = 0; i < solver.column.TotNoStages; i++)
                {
                    TrayTest tray = solver.column.MainTraySection.Trays[i];
                    tray.StripFact = stripafcs[i];
                }
            }

            return;
        }

        public void SetT(object Tarray)   // UPDATE TRAY TEMPS AND CALCULATE KS
        {
            double[] Ts = Tarray as double[];

            if (Ts is null)
                return;

            if (solver is not null && solver.column is not null)
            {
                for (int i = 0; i < solver.column.TotNoStages; i++)
                {
                    TrayTest T = solver.column.TraySections[0].Trays[i];
                    T.T = Ts[i];
                }
            }
        }

        public void SetTPred(object Tarray)   // UPDATE TRAY TEMPS AND CALCULATE KS
        {
            double[] Ts = Tarray as double[];

            if (Ts is null)
                return;

            if (solver is not null && solver.column is not null)
            {
                for (int i = 0; i < solver.column.TotNoStages; i++)
                {
                    TrayTest T = solver.column.TraySections[0].Trays[i];
                    T.TPredicted = Ts[i];
                }
            }
        }

        public bool SolveComponenetBalance()
        {

            // Debugger.Launch();
            solver.SolveComponenetBalance();
            return true;
        }

        public object GetEnthalpyBalance()
        {

            // Debugger.Launch();
            return column.MainTraySection.EnthalpyBalances;
        }

        public object GetErrors()
        {

            // Debugger.Launch();
            return column.MainTraySection.Errors.ToArray();
        }

        public object GetJacobian()
        {

            // Debugger.Launch();
            return solver.Jacobian;
        }

        public object UpdateAlphas()
        {
            if (solver is not null && solver.column is not null)
            {
                for (int i = 0; i < solver.column.TotNoStages; i++)
                {
                    TrayTest T = solver.column.TraySections[0].Trays[i];
                    T.UpdateAlphas(column.Components, T.LiqComposition, T.VapComposition, COMColumnAlphaMethod.LogLinear, column.Thermo);
                }
            }
            return true;
        }

    }
}