using ModelEngine;
using System;
using System.Collections.Generic;

namespace Units
{
    public partial class CompressorDialog : BaseDialog, IUpdateableDialog
    {
        private readonly Compressor comp;

        public CompressorDialog()
        {
            InitializeComponent();
        }

        internal CompressorDialog(DrawCompressor dcompressor)
        {
            this.comp = dcompressor.compressor;
            InitializeComponent();

            List<StreamMaterial> streams = dcompressor.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, dcompressor.DrawArea.UOMDisplayList);

            Parameters.Add(comp.PEff.Value, "PolytropicEfficiency");
            Parameters.Add(comp.AEff.Value, "AdiabaticEfficiency");

            CreateDataGrid();

            Parameters.ValueChanged += ValueChanged;
            Worksheet.ValueChanged += ValueChanged;
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            base.RaiseValueChangedEvent(e);
        }

        public void CreateDataGrid()
        {
            Results.Add(comp.PolytropicHead.Value, "PolytropicHead");
            Results.Add(comp.AdiabaticHead.Value, "AdiabaticHead");
            Results.Add(comp.PolytropicFluidHead.Value, "PolytropicFluidHead");
            Results.Add(comp.AdiabaticFluidHead.Value, "AdiabaticFluidHead");
            Results.Add(comp.PEff.Value, "PolytropicEfficiency");
            Results.Add(comp.AEff.Value, "AdiabticEfficiency");
            Results.Add(comp.PowerConsumed.Value, "PowerConsumed");
            Results.Add(comp.IsentropicExponent.Value, "IsentropicExponent");
            Results.Add(comp.PolytropicExponent.Value, "PolytropicExponent");
            Results.Add(comp.PolytropicHeadFactor.Value, "PolytropicHeadFactor");
            //Results.Add(comp.Speed.Value,"Speed");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //comp.IsSolved=false;
            RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            Parameters.UpdateValues();
            Worksheet.UpdateValues();
            Results.UpdateValues();
        }
    }
}