using ModelEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Units.UOM;

namespace Units
{
    public partial class AssayCutterDLG : BaseDialog, IUpdateableDialog
    {
        private AssayCutter cutter;
        private List<Port_Material> oilarrays = new();

        internal AssayCutterDLG(DrawAssayCutter dac, DrawAssayCutter drawcutter)
        {
            this.cutter = drawcutter.Cutter;
            InitializeComponent();

            List<StreamMaterial> streams = drawcutter.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, drawcutter.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();

            dataGridView1.Rows.Add(10);
            oilarrays = cutter.StreamList;
            updategrid();
        }

        public void updategrid()
        {
            Port_Material Portin = cutter.PortIn;

            for (int i = 0; i < cutter.CutPoints.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = cutter.CutPoints[i].Item1.Celsius;
                dataGridView1.Rows[i].Cells[1].Value = cutter.CutPoints[i].Item2.Celsius;
            }

            for (int i = 0; i < cutter.CutPoints.Count; i++)
            {
                dataGridView1.Rows[i].Cells[2].Value = oilarrays[i].MF_;
                dataGridView1.Rows[i].Cells[3].Value = oilarrays[i].VF_;
                dataGridView1.Rows[i].Cells[4].Value = oilarrays[i].MolarFlow_;
                dataGridView1.Rows[i].Cells[5].Value = oilarrays[i].MF_ / Portin.MF_;
                dataGridView1.Rows[i].Cells[6].Value = oilarrays[i].VF_ / Portin.VF_;
                dataGridView1.Rows[i].Cells[7].Value = oilarrays[i].MolarFlow_ / Portin.MolarFlow_;
            }
        }

        public void updatecutpoints()
        {
            Tuple<Temperature, Temperature> value;

            cutter.CutPoints.Clear();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value is not null &&
                dataGridView1.Rows[i].Cells[1].Value is not null)
                {
                    if (double.TryParse(dataGridView1.Rows[i].Cells[0].Value.ToString(), out double IBP) &&
                    double.TryParse(dataGridView1.Rows[i].Cells[1].Value.ToString(), out double FBP))
                    {
                        value = new Tuple<Temperature, Temperature>(IBP + 273.15, FBP + 273.15);
                        cutter.CutPoints.Add(value);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updatecutpoints();
            cutter.Solve();
            updategrid();
        }

        private void AssayCutterDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            updatecutpoints();
        }
    }
}