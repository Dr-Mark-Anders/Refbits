using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Units.DrawingObjects.UnitDialogs;

namespace Units
{
    [Serializable]
    public partial class TableControl : UserControl, ISerializable
    {
        internal List<ePropID> props = new List<ePropID>() { ePropID.MF, ePropID.VF, ePropID.MOLEF, ePropID.T, ePropID.P };
        internal List<Guid> activestreams = new List<Guid>();
        private DrawArea drawarea;

        public TableControl()
        {
            InitializeComponent();
        }

        public TableControl(SerializationInfo info, StreamingContext context)
        {
            InitializeComponent();
            activestreams = (List<Guid>)info.GetValue("streamguids", typeof(List<Guid>));
            this.Location = (System.Drawing.Point)info.GetValue("Location", typeof(System.Drawing.Point));
            try
            {
                props = (List<ePropID>)info.GetValue("props", typeof(List<ePropID>));
            }
            catch
            {
            }
        }

        public void UpdateData()
        {
            int col;
            dgv.Columns.Clear();
            dgv.Rows.Clear();
            dgv.Columns.Add("Names", "Property");
            dgv.Columns[0].Width = 60;
            dgv.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            if (props.Count > 0)
            {
                dgv.Rows.Add(props.Count);

                for (int i = 0; i < props.Count; i++)
                {
                    dgv[0, i].Value = props[i].ToString();
                }

                foreach (var item in activestreams)
                {
                    DrawMaterialStream dms = drawarea.GraphicsList.ReturnStream(item);
                    if (dms != null)
                    {
                        col = dgv.Columns.Add(item.ToString(), dms.Name);
                        dgv.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dgv.Columns[col].Width = 60;
                        for (int i = 0; i < props.Count; i++)
                        {
                            if (dms.Port.Props.ContainsKey(props[i]))
                                dgv[col, i].Value = dms.Port.Props[props[i]].DisplayValueOut();
                        }
                    }
                }
            }

            this.Width = dgv.ColumnCount * 60 + 5;
            this.Height = dgv.RowCount * dgv.Rows[0].Height + dgv.ColumnHeadersHeight + 20;
            dgv.Height = dgv.RowCount * dgv.Rows[0].Height + dgv.ColumnHeadersHeight;
        }

        public TableControl(int x, int y, int v1, int v2)
        {
            InitializeComponent();
            this.Left = x;
            this.Top = y;
            //this.Width = 100;
            // this.Height = 100;
            Visible = true;
            //drawarea.Controls.Add(this);
            //Helper.ControlMover.Init(this);
            dgv.Rows.Add(props.Count);
        }

        public bool Selected { get; internal set; }
        public DrawArea Drawarea { get => drawarea; set => drawarea = value; }

        internal void Draw(Graphics g)
        {
            int col;
            dgv.Rows.Clear();
            if (props.Count > 0)
            {
                dgv.Rows.Add(props.Count);

                for (int i = 0; i < props.Count; i++)
                {
                    dgv[0, i].Value = props[i].ToString();
                }

                if (dgv.ColumnCount >= activestreams.Count)
                    foreach (var item in activestreams)
                    {
                        DrawMaterialStream dms = drawarea.GraphicsList.ReturnStream(item);
                        if (dms != null)
                        {
                            col = dgv.Columns[item.ToString()].Index;
                            for (int i = 0; i < props.Count; i++)
                            {
                                if (dms.Port.Props.ContainsKey(props[i]))
                                    dgv[col, i].Value = dms.Port.Props[props[i]].DisplayValueOut();
                            }
                        }
                    }

                this.Width = dgv.ColumnCount * 60 + 5;
                this.Height = dgv.RowCount * dgv.Rows[0].Height + dgv.ColumnHeadersHeight + 20;
                dgv.Height = dgv.RowCount * dgv.Rows[0].Height + dgv.ColumnHeadersHeight;
            }
        }

        private void dgv_doubleClick(object sender, EventArgs e)
        {
            List<DrawMaterialStream> streams = drawarea.GraphicsList.ReturnMaterialStreams();
            TableDialog ds = new TableDialog(this, streams);
            ds.ShowDialog();
            UpdateData();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("streamguids", activestreams);
            info.AddValue("Location", Location);
            info.AddValue("props", props);
        }

        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                drawarea.Tablecontrols.Remove(this);
                drawarea.Controls.Remove(this);
            }
            drawarea.Refresh();
        }
    }
}