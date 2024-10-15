using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelEngine
{
    public partial class ReactionEditor : Form
    {
        public ReactionEditor()
        {
            InitializeComponent();
        }

        public void ShowReactions(Reactions r)
        {
            for (int i = 0; i < r.Count; i++)
            {
                Reaction rr = r[i];

                int rowno =  dataGridView1.Rows.Add();

                DataGridViewRow row = dataGridView1.Rows[rowno];

                row.Cells[1].Value = rr.ReactionName;
                row.Cells[2].Value = rr.CompNames[0];
                row.Cells[3].Value = rr.CompNames[1];
                row.Cells[4].Value = rr.CompNames[2];
                row.Cells[5].Value = rr.CompNames[3];
                row.Cells[6].Value = rr.Index[rr.CompNames[0]];
                row.Cells[7].Value = rr.Index[rr.CompNames[1]];
                row.Cells[8].Value = rr.Index[rr.CompNames[2]];
                row.Cells[9].Value = rr.Index[rr.CompNames[3]];
                row.Cells[10].Value = rr.ar.K;
                row.Cells[11].Value = rr.ar.Ae;
                row.Cells[12].Value = rr.ar.Kr;
                row.Cells[13].Value = rr.ar.Aer;
            }
        }
    }
}


