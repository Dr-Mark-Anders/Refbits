using System.Collections.Generic;
using System.Windows.Forms;

namespace Units
{
    public partial class TableForm : Form
    {
        public TableForm(List<DrawMaterialStream> streams, DrawArea drawarea)
        {
            InitializeComponent();
            foreach (var item in streams)
            {
                tableControl1.activestreams.Add(item.Guid);
            }
            tableControl1.Drawarea = drawarea;
            tableControl1.UpdateData();
        }
    }
}