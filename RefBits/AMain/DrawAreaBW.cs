using ModelEngine;
using System.ComponentModel;
using System.Linq;

namespace Units
{
    public partial class DrawArea
    {
        private readonly BackgroundWorker backgroundWorker = new();

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            FlowSheet.BackGroundWorker = backgroundWorker;
            lock (GlobalModel.Flowsheet)
            {
                GlobalModel.Flowsheet.PreSolve();
            }
            e.Cancel = true;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.UserState)
            {
                case Column c:
                    DrawColumn dc = (DrawColumn)c.OnRequestParent();
                    if (dc.Cdlg.Error1.Lines.Count() < 1)
                        dc.Cdlg.UpdateInitConvergence(c.ConvergnceError.ToString("F1"));
                    dc.Cdlg.UpdateIteration1(c.Err1);
                    dc.Cdlg.UpdateIteration2(c.Err2);
                    break;

                case null:
                   // this.Refresh();
                    break;

                case COlSubFlowSheet:
                    break;

                default:
                    break;
            }
            this.Refresh();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GlobalModel.IsRunning = false;

            foreach (DrawObject unitop in GraphicsList)
                unitop.PostSolve();
            this.Refresh();

            UpdateForms(null);
        }
    }
}