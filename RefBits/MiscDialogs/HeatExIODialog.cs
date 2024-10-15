using ModelEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Units.DrawingObjectDialogs
{
    public partial class HeatExIODialog : Form
    {
        private int noconstraints;

        private DrawIOExchanger de;

        public HeatExIODialog()
        {
            InitializeComponent();
        }

        private void addrow(string txt, UOMProperty basicprop)
        {
            int rowno = dGV1.Rows.Add();
            DataGridViewRow dr = (DataGridViewRow)dGV1.Rows[rowno];

            dr.Cells["Constraint "].Value = txt;
            dr.Cells["Value"].Value = basicprop;
            if (basicprop.IsKnown)
            {
                dr.Cells["Active"].Value = true;
            }
            else
                dr.Cells["Active"].Value = false;

            if (basicprop.IsKnown)
            {
                DataGridViewCell cll;
                int NoRows = dGV1.Rows.Count;
                DataGridViewCellStyle st = new DataGridViewCellStyle();

                st.BackColor = Color.DarkGray;

                cll = (DataGridViewCell)dGV1.Rows[NoRows - 1].Cells[0];
                cll.Style = st;
                cll = (DataGridViewCell)dGV1.Rows[NoRows - 1].Cells[1];
                cll.Style = st;
                cll = (DataGridViewCell)dGV1.Rows[NoRows - 1].Cells[2];
                cll.Style = st;
            }
        }

        private void addrow(string txt, double value)
        {
            DataGridViewRow dr = dGV1.Rows[dGV1.Rows.Add()];
            dr.Cells["Constraint "].Value = txt;
            dr.Cells["Value"].Value = value;
            dr.Cells["Active"].Value = false;
        }

        internal HeatExIODialog(DrawIOExchanger ex)
        {
            InitializeComponent();

            de = ex;

            /*  addrow("TubeSide In", ex.TSIN.T);
              addrow("TubeSide Out", ex.TSOUT.T);
              addrow("ShellSide In", ex.SSIN.T);
              addrow("ShellSide Out", ex.SSOUT.T);*/

            addrow("Duty", ex.Duty);
            addrow("UA", ex.UA);
            addrow("Min Approach T", -999D);

            //dbUA.SetValues(ex.UA);
            //dbDuty.SetValues(ex.Duty);

            dbShellSideDT.BindValues(ex.ShellSideDT);
            dbTubeSideDT.BindValues(ex.TubeSideDT);

            dbShellSideDP.BindValues(ex.ShellSideDP);
            dbTubeSideDP.BindValues(ex.TubeSideDP);
        }

        private void dGV1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            bool b;
            DataGridViewRow r;
            noconstraints = -2;

            for (int n = 0; n < dGV1.Rows.Count; n++)
            {
                r = dGV1.Rows[n];
                b = (bool)r.Cells["Active"].Value;
                if (b)
                    noconstraints++;
            }

            tbDegreesOfFreedom.Text = noconstraints.ToString();

            if (dGV1.IsCurrentCellDirty)
            {
                dGV1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }

            DataGridViewCellStyle st = new DataGridViewCellStyle();
            st.BackColor = Color.DarkGray;

            /*if (noconstraint s == 0)
            {
                for (int  n = 0; n < dGV1.Rows.Count; n++)
                {
                    r = dGV1.Rows[n];

                    if (!(bool)r.Cells[2].Value)
                    {
                        st.BackColor = Color.DarkGray;
                        cll = (DataGridViewCell)dGV1.Rows[n].Cells[0];
                        cll.Style = st;
                        cll = (DataGridViewCell)dGV1.Rows[n].Cells[1];
                        cll.Style = st;
                        cll = (DataGridViewCell)dGV1.Rows[n].Cells[2];
                        cll.Style = st;
                    }
                }
            }
            else
            {
                for (int  n = 0; n < dGV1.Rows.Count; n++)
                {
                    r = dGV1.Rows[n];

                    st.BackColor = SystemColors.Control;
                    cll = (DataGridViewCell)dGV1.Rows[n].Cells[0];
                    cll.Style = st;
                    cll = (DataGridViewCell)dGV1.Rows[n].Cells[1];
                    cll.Style = st;
                    cll = (DataGridViewCell)dGV1.Rows[n].Cells[2];
                    cll.Style = st;
                }
            }*/
        }

        private void dGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (e.Value is UOMProperty)
                {
                    UOMProperty dc = (UOMProperty)e.Value;
                    if (dc.IsKnown)
                    {
                        e.CellStyle.BackColor = Color.DarkGray;
                    }
                }
            }
        }
    }
}