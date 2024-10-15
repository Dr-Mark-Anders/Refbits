using ModelEngine;
using System;
using System.Collections.Generic;

namespace Units
{
    public partial class DividerDialog : BaseDialog, IUpdateableDialog
    {
        private readonly Divider divider;

        public DividerDialog()
        {
            InitializeComponent();
        }

        internal DividerDialog(DrawDivider drawdivider)
        {
            InitializeComponent();
            this.divider = drawdivider.divider;

            List<Port_Signal> splits = divider.splits;
            NodeCollection nodes = drawdivider.Hotspots.GetProducts();

            for (int i = 0; i < nodes.Count; i++)
            {
                Port_Signal p = splits[i];
                Port_Material pm = divider.Ports[i + 1];
                DrawMaterialStream sm = drawdivider.Hotspots[i + 1].GetAttachedStream(drawdivider.DrawArea.GraphicsList);
                if (sm != null)
                    Parameters.Add(p.Value, sm.Name);
            }

            List<StreamMaterial> streams = drawdivider.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, drawdivider.DrawArea.UOMDisplayList);
            Worksheet.ValueChanged += Worksheet_ValueChanged;
            Parameters.ValueChanged += Worksheet_ValueChanged;
        }

        private void Worksheet_ValueChanged(object sender, EventArgs e)
        {
            int count = 0;
            double sum = 0;
            Port_Signal ps = null;

            for (int i = 0; i < divider.splits.Count; i++)
            {
                Port_Signal p = divider.splits[i];
                if (!p.Value.IsInput)
                {
                    p.Value.Clear();
                }
            }

            for (int i = 0; i < divider.splits.Count; i++)
            {
                Port_Signal p = divider.splits[i];

                if (divider.Ports[i + 1].IsConnected && p.Value.IsInput)
                {
                    sum += p.Value;
                }
                else if (divider.Ports[i + 1].IsConnected && !p.Value.IsInput)
                {
                    count++;
                    ps = p;
                }
            }

            if (count == 1 && ps != null)
            {
                ps.Value.Value = 1 - sum;
                ps.Value.origin = SourceEnum.UnitOpCalcResult;
            }
            else
            {
                //return  ;
            }

            Worksheet.UpdateValues();

            base.RaiseValueChangedEvent(e);
        }

        public override void UpdateValues()
        {
            Parameters.UpdateValues();
            Worksheet.UpdateValues();
        }
    }
}