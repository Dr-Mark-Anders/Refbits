using ModelEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static ModelEngine.HeatExchanger2;

namespace Units
{
    public partial class HeatExDialog : BaseDialog, IUpdateableDialog
    {
        private readonly HeatExchanger2 ex;

        public HeatExDialog()
        {
            InitializeComponent();
        }

        internal HeatExDialog(DrawExchanger de)
        {
            InitializeComponent();

            this.ex = de.Exchanger;

            Specifications.ValueChanged += Specifications_ValueChanged;
            Paremeters.ValueChanged += Parameters_ValueChanged;

            Results.Add(ex.UA.Value, "UA");
            Results.Add(ex.LMTD.Value, "LMTD");
            Results.Add(ex.Q.Value, "Duty");
            Results.Add(ex.deltaTApproach.Value, "Temp Approach");
            Results.Add(ex.deltaTShellSide.Value, "Delt T Shell Side");
            Results.Add(ex.deltaTTubeSide.Value, "Delt T Tube Side");

            Paremeters.Add(ex.deltaPShellSide.Value, "Delta Pressure Shell");
            Paremeters.Add(ex.deltaPTubeSide.Value, "Delta Pressure Tube");

            List<StreamMaterial> streams = de.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, de.DrawArea.UOMDisplayList);
            Worksheet.ValueChanged += Worksheet_ValueChanged;

            LoadDataGrid();
        }

        private void Worksheet_ValueChanged(object sender, EventArgs e)
        {
            RaiseValueChangedEvent(e);
        }

        private void Parameters_ValueChanged(object sender, EventArgs e)
        {
            RaiseValueChangedEvent(e);
        }

        private void Specifications_ValueChanged(object sender, EventArgs e)
        {
            ex.IsDirty = true;
            RaiseValueChangedEvent(e);
        }

        public void LoadDataGrid()
        {
            foreach (ExcSpec spec in ex.ExSpecs)
            {
                Specifications.Add(spec.value, spec.Name);
            }
        }

        public override void UpdateValues()
        {
            Paremeters.UpdateValues();
            Specifications.UpdateValues();
            Results.UpdateValues();
            Worksheet.UpdateValues();
            this.Invalidate();
            //this.Refresh();
        }

        public List<Control> getControls(string what, Control where)
        {
            List<Control> controles = new();
            foreach (Control c in where.Controls)
            {
                if (c.GetType().Name == what)
                    controles.Add(c);
                else if (c.Controls.Count > 0)
                    controles.AddRange(getControls(what, c));
            }
            return controles;
        }

        private void cbxActive_CheckedChanged(object sender, EventArgs e)
        {
            ex.IsActive = cbxActive.Checked;
        }

        private void HeatExDialog_Load(object sender, EventArgs e)
        {
            cbxActive.Checked = ex.IsActive;
        }
    }
}