using ModelEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class PONAdata : UserControl
    {
        public enumMassMolarOrVol basis = enumMassMolarOrVol.Molar;
        private readonly GCData data;

        public PONAdata(GCData data)
        {
            this.data = data;
            InitializeComponent();
            int rowno;

            for (int i = 4; i <= 13; i++)
            {
                rowno = DGV.Rows.Add();
                DGV.Rows[rowno].Cells[0].Value = "C" + i.ToString();
            }
        }

        public PONAdata()
        {
            InitializeComponent();
            int rowno;

            for (int i = 4; i <= 13; i++)
            {
                rowno = DGV.Rows.Add();
                DGV.Rows[rowno].Cells[0].Value = "C" + i.ToString();
            }
        }

        internal List<double> GetFractions()
        {
            List<double> res = new();
            for (int row = 4; row <= 13; row++)
            {
                for (int col = 1; col < 7; col++)
                {
                    if (DGV.Rows[row - 4].Cells[col].Value != null && double.TryParse(DGV.Rows[row - 4].Cells[col].Value.ToString(), out double resv))
                        res.Add(resv);
                    else
                        res.Add(double.NaN);
                }
            }
            return res;
        }

        internal void SetFractions(RefomerShortCompList gcdata)
        {
            int row;
            int col;
            if (gcdata.Components.Count == 64)
            {
                for (int count = 0; count < 60; count++)
                {
                    row = count / 6;
                    col = count - row * 6 + 1;
                    DGV.Rows[row].Cells[col].Value = gcdata.Components[count].MoleFraction;
                }
            }
        }

        public enumMassMolarOrVol Basis
        {
            get
            {
                return basis;
            }
            set
            {
                basis = value;
            }
        }

        public GCData Data => data;

        public new event MouseEventHandler MouseClick;

        private void MassMolarVol_MouseClick(object sender, MouseEventArgs e)
        {
            RaiseMouseEvent(MouseClick, e);
        }

        private void PONAdata_Resize(object sender, EventArgs e)
        {
            DGV.Left = PCTType.Right + 10;
            //DGV.Top = gb.Top + 20;
            // DGV.Height = gb.Height - 30;
            // DGV.Width = gb.Width - 20;
        }

        private void pg1_Load(object sender, EventArgs e)
        {
        }
    }
}