using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ModelEngine
{
    public partial class InconsistencyForm : Form
    {
        private readonly List<InconsistencyObject> list = new();
        private DataGridViewRow row;
        private readonly FlowSheet fs;

        public InconsistencyForm(FlowSheet fs)
        {
            this.fs = fs;
            InitializeComponent();
            list.AddRange(FlowSheet.InconsistencyStack.ToList());
            list.Reverse();
        }

        private void InconsistencyForm_Load(object sender, EventArgs e)
        {
            int rowno;

            foreach (InconsistencyObject iobj in list)
            {
                if (iobj.type == Inconsistencytype.Property)
                {
                    rowno = DGV.Rows.Add();
                    //Port o = Getobject (iobj.guid);

                    row = DGV.Rows[rowno];
                    row.Cells["Inconsistency"].Value = iobj.Name;
                    if (iobj.SourcePort != null && iobj.SourcePort.Owner != null)
                    {
                        row.Cells[1].Value = iobj.SourcePort.Owner.Name;
                        row.Cells[2].Value = iobj.SourcePort.Name;
                        row.Cells[3].Value = iobj.SourcePort.Properties[iobj.propid];
                    }

                    if (iobj.DestPort != null && iobj.DestPort.Owner != null)
                    {
                        row.Cells[4].Value = iobj.DestPort.Owner.Name;
                        row.Cells[5].Value = iobj.DestPort.Name;
                        row.Cells[6].Value = iobj.DestPort.Properties[iobj.propid];
                    }
                }
                else // component
                {
                    rowno = DGV.Rows.Add();
                    //Port o = Getobject (iobj.guid);

                    row = DGV.Rows[rowno];
                    row.Cells["Inconsistency"].Value = iobj.Name;
                    row.Cells[1].Value = iobj.SourcePort.Owner.Name;
                    row.Cells[2].Value = iobj.SourcePort.Name;
                    row.Cells[3].Value = iobj.SourcePort.cc[iobj.Bc].MoleFraction;

                    row.Cells[4].Value = iobj.DestPort.Owner.Name;
                    row.Cells[5].Value = iobj.DestPort.Name;
                    row.Cells[6].Value = iobj.DestPort.cc[iobj.Bc].MoleFraction;
                }
            }
        }

        public Port GetObject(Guid guid)
        {
            return fs.GetObjectData(guid);
        }
    }
}