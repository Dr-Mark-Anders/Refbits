using ModelEngine;
using System.Collections.Generic;
using Units.PortForm;

namespace Units
{
    public partial class RecycleDialog : BaseDialog, IUpdateableDialog
    {
        private DrawRecycle drawRecycle;
        private Recycle rec;
        private IterationDataStore data;
        private FlowSheet fs;

        public RecycleDialog()
        {
            InitializeComponent();
        }

        //public  event PumpDialogValueChangedEventHandler PumpDLGValueChangedEvent;
        //public  delegate void  PumpDialogValueChangedEventHandler(object sender, EventArgs e);

        internal RecycleDialog(FlowSheet fs, DrawRecycle rec)
        {
            this.fs = fs;
            InitializeComponent();

            this.drawRecycle = rec;
            this.rec = (Recycle)rec.AttachedModel;

            rbWegstein.Checked = rec.UseWegstein;
            rbDirectSubstition.Checked = !rec.UseWegstein;

            // Worksheet.ShowColumns(2);
            data = rec.datastore;

            //Worksheet.populate(rec.Feeds[0], enumproptype.Conditions, 1);
            //Worksheet.update(rec.Products[0], enumproptype.Conditions, 2);

            UpdateDatastore();

            List<StreamMaterial> streams = rec.GetStreamListFromNodes();
            Worksheet.PortsPropertyWorksheetInitialise(streams, rec.DrawArea.UOMDisplayList);
            Worksheet.UpdateValues();
        }

        private void Worksheet_ValueChangedEvent(object sender, System.EventArgs e)
        {
            base.RaiseValueChangedEvent(e);
        }

        public void UpdateDatastore()
        {
            int count = data.Count();

            for (int i = 0; i < count; i++)
            {
                int RowNo = dataGridView1.Rows.Add(1);
                dataGridView1.Rows[RowNo].Cells[0].Value = data.datastore[i].Item1;
                dataGridView1.Rows[RowNo].Cells[1].Value = data.datastore[i].Item2;
                dataGridView1.Rows[RowNo].Cells[2].Value = data.datastore[i].Item3;
                dataGridView1.Rows[RowNo].Cells[3].Value = data.datastore[i].Item4;
                dataGridView1.Rows[RowNo].Cells[4].Value = data.datastore[i].Item5;
            }
        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            rec.IsActive = checkBox1.Checked;
            base.RaiseValueChangedEvent(e);
        }

        private void rbWegstein_CheckedChanged(object sender, System.EventArgs e)
        {
            if (drawRecycle != null)
                drawRecycle.UseWegstein = rbWegstein.Checked;
        }

        private void rbDirectSubstition_CheckedChanged(object sender, System.EventArgs e)
        {
            if (drawRecycle != null)
                drawRecycle.UseWegstein = rbWegstein.Checked;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            PortPropertyForm2 pf = new PortPropertyForm2(rec.Ports["Out2"], drawRecycle.DrawArea.UOMDisplayList);
            pf.ShowDialog();
        }

        private void RecycleDialog_Load(object sender, System.EventArgs e)
        {
            checkBox1.Checked = rec.IsActive;
        }
    }
}