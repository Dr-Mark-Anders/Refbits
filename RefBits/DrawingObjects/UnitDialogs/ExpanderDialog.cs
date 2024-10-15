using ModelEngine;
using System;
using System.Collections.Generic;

namespace Units
{
    public partial class ExpanderDialog : BaseDialog, IUpdateableDialog
    {
        private readonly Expander expander;

        public ExpanderDialog()
        {
            InitializeComponent();
        }

        internal ExpanderDialog(DrawExpander dexp)
        {
            this.expander = dexp.expander;

            InitializeComponent();

            List<StreamMaterial> streams = dexp.GetStreamListFromNodes();
            worksheet.PortsPropertyWorksheetInitialise(streams, dexp.DrawArea.UOMDisplayList);

            Parameters.Add(expander.PEff.Value, "Polytropic Efficiency");
            Parameters.Add(expander.AEff.Value, "Adiabatic Efficiency");

            Parameters.ValueChanged += ValueChanged;
            worksheet.ValueChanged += ValueChanged;

            CreateDataGrid();
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            base.RaiseValueChangedEvent(e);
        }

        public void CreateDataGrid()
        {
            Results.Add(expander.AdiabaticHead.Value, "Adiabatic Head");
            Results.Add(expander.PolytropicHead.Value, "Polytropic Head");
            Results.Add(expander.AdiabaticFluidHead.Value, "Adiabatic Fluid Head");
            Results.Add(expander.PolytropicFluidHead.Value, "Polytropic Fluid Head");
            Results.Add(expander.AEff.Value, "Isentropic Efficiency");
            Results.Add(expander.PEff.Value, "Polytropic Efficiency");
            Results.Add(expander.PowerConsumed.Value, "Power");
            Results.Add(expander.IsentropicExponent.Value, "Isentropic Exponent");
            Results.Add(expander.PolytropicExponent.Value, "Polytropic Exponent");
            Results.Add(expander.PolytropicHeadFactor.Value, "Polytropic Head Factor");
            // Results.Add(expander.Speed.Value, "");
        }

        public override void UpdateValues()
        {
            Parameters.UpdateValues();
            worksheet.UpdateValues();
            Results.UpdateValues();
        }
    }
}