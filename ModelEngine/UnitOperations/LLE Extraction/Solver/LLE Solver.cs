using ModelEngine;
using Extensions;
using ModelEngine;
using System;
using System.Diagnostics;
using Units;

namespace LLE_Solver
{
    [Serializable]
    public partial class LLESolver
    {
        private readonly object balanceLock = new();

        private LLESEP column;

        private double JDelta = 0.1;
        private double KDelta = 0;
        private double MaxDeltaT = 250;
        private int WaterLoc;
        private readonly double DefaultRefluxRatio = 3;

        //private  string docPath ="C:\\";
        // readonly  string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        [STAThread()]
        public bool Solve(LLESEP llesep)
        {
            //ViewArray va = new();
            double StartError;
            int OuterLoopCounter = 0, TemperatureLoopCounter = 0;
            double ErrorSum = 0;
            JDelta = 0.1;
            KDelta = 1;
            MaxDeltaT = 250;

            llesep.Thermo.Enthalpy = enumEnthalpy.LeeKesler;

            var watch = Stopwatch.StartNew();

            lock (balanceLock)
            {
                for (int i = 0; i < llesep.TraySections.Count; i++)
                {
                    TraySection ts = llesep.TraySections[i];
                    for (int trayNo = 0; trayNo < ts.Trays.Count; trayNo++)
                        ts.Trays[trayNo].Name = i.ToString() + ":" + trayNo.ToString();
                }

                SetBasicValues(llesep);

                WaterLoc = llesep.Components.IndexOf("H2O", llesep.Components.MoleFractions, out double watermolefrac);

                NoComps = llesep.Components.Count;
                if (llesep.Components.Count == 0)
                    return false;

                TotNoTrays = llesep.TraySections.TotNoTrays();

                if (TotNoTrays == 0)
                    return false;

                WaterCompNo = llesep.Components.GetWaterCompNo();
                this.column = llesep;

                enumCalcResult cres = enumCalcResult.Converged;

                if (llesep.MainSectionStages is null)
                    return false;

                double NoSpecs = llesep.Specs.GetActiveSpecs().Count();

                InitialiseMatrices(llesep); // no calcs here
                InitTRDMatrix();

                if (!interpolatePressures())
                    return false;

                if (!InitialiseFeeds(llesep))
                    return false;

                if (llesep.IsReset)
                {
                    InitialiseFlowsLLE(llesep);
                    InitialiseTrayCompositions(llesep);
                    SetInitialTrayTemps(llesep);
                }

                GetUniquacParams(llesep.Thermo, llesep.Components);

                if (!CreateInitialLLEAlphas(llesep.Thermo))
                    return false;

                intialiseStripFactorsEtc();

                SolveComponentBalance(llesep);
                //va.View(TrDMatrix[0]);

                UpdateActualCompositions();
                UpdateInitialCompositions(1);
                //UpdateInitialActivies(llesep.Thermo);

                UpdateVapEnthalpyParameters();
                UpdateLiqEnthalpyParameters();
                EnthalpiesUpdate(ref cres);
                UpdateTrayEnthalpyBalances();

                column.Specs.Clear();
                AddTrayHeatBalancesToSpecs();
                AddLLESpecs();

                llesep.Specs.Sort();
                CalcErrors(true);  // temperatues are predicted Temperature s
                ProcessActiveSpecs(out BaseErrors, true);

                StartError = BaseErrors.SumSQR();

                double ActivityError = 10;

                do
                {
                    do // alphaloop
                    {
                        TemperatureLoopCounter++;
                        CreateJacobianLLE(llesep, ref cres);
                        //va.View(Jacobian);
                        SolveJacobian(Jacobian, BaseErrors, ref grads);
                        llesep.TraySections.BackupFactors(llesep);
                        UpdateFactors(grads, 0.1);
                        UpdateColumnBalance(ref ErrorSum, cres);  //Temperature s are Re-estimated
                        BaseErrors = Errors;
                    } while (!double.IsNaN(ErrorSum) && ErrorSum > llesep.SpecificationTolerance);

                    var oldActivyError = ActivityError;

                    ActivityError = 0;
                    for (int i = 0; i < column.MainSectionStages.Trays.Count; i++)
                    {
                        ActivityError += column.MainTraySection.Trays[i].LLE_K.SumSQR();
                    }

                    if (Math.Abs(oldActivyError - ActivityError) < 1) // stop when activity stops changing
                        break;

                    UpdateInitialActivies(llesep.Thermo);

                    //Temperature LoopCounter = 0;
                    llesep.Err1 = TemperatureLoopCounter + ": " + SignificantDigits.Round(ErrorSum, 5) + " \r";
                    llesep.ErrHistory1 += llesep.Err1;

                    OuterLoopCounter++;

                    llesep.Err2 = OuterLoopCounter + ": " + SignificantDigits.Round(OuterLoopCounter, 5) + " \r";
                    llesep.ErrHistory2 += llesep.Err2;

                    if (FlowSheet.BackGroundWorker != null && FlowSheet.BackGroundWorker.IsBusy)
                        FlowSheet.BackGroundWorker.ReportProgress(1, llesep);
                }
                while (Math.Abs(1) > llesep.TrayTempTolerance && OuterLoopCounter < llesep.MaxOuterIterations);  // deg per tray error

                if (double.IsNaN(ErrorSum) || ErrorSum > llesep.SpecificationTolerance)
                {
                    //llesep.IsSolved = false;
                    return false;
                }
                else
                {
                    UpdateAlphas(llesep.Thermo);
                    Finishoff();
                    UpdatePorts(false);
                    //llesep.IsSolved = true;
                    return true;
                }
            }
        }

        private void GetUniquacParams(ThermoDynamicOptions thermo, Components cc)
        {
            UniquacData data = new();
            thermo.UniquacParams = new double[cc.Count][];

            for (int i = 0; i < column.Components.Count; i++)
            {
                thermo.UniquacParams[i] = new double[cc.Count];
            }

            for (int i = 0; i < column.Components.Count; i++)
                for (int x = 1; x < column.Components.Count; x++)
                {
                    var res = data.GetData(column.Components[i].Name, column.Components[x].Name);
                    if (res is not null)
                    {
                        thermo.UniquacParams[i][x] = res.Item1;
                        thermo.UniquacParams[x][i] = res.Item2;
                    }
                }

            data.GetUniquacR_P(cc);
        }

        private void InitialiseFlowsLLE(LLESEP llesep)
        {
            double HeavyFlow = column.MainSectionStages.TopTray.feed.MolarFlow_;
            double LightFlow = column.MainSectionStages.BottomTray.feed.MolarFlow_;

            for (int i = 0; i < column.MainSectionStages.Trays.Count; i++)
            {
                column.MainSectionStages.Trays[i].V = LightFlow;
                column.MainSectionStages.Trays[i].L = HeavyFlow;
            }
        }
    }
}