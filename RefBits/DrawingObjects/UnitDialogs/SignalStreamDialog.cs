using ModelEngine;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units.DrawingObjects.UnitDialogs
{
    public partial class SignalStreamDialog : Form
    {
        private StreamSignal signalstream;
        private StreamPropList props;

        internal SignalStreamDialog(DrawSignalStream stream, DrawObject startobject, DrawObject endobject)
        {
            InitializeComponent();

            signalstream = stream.Stream;

            //DrawObject startobj = stream.startObject;
            //DrawObject endobj = stream.endObject;
            treeViewProperty.Nodes.Clear();
            treeViewProperty.Nodes.Add("Stream Property");

            switch (startobject)
            {
                case DrawMaterialStream dms:
                    props = dms.Port.Properties;
                    if (props is not null)
                    {
                        foreach (KeyValuePair<ePropID, StreamProperty> item in props)
                        {
                            treeviewProptype.Nodes[0].Nodes.Add(item.Value.Name);
                        }
                    }

                    break;

                case DrawRectangle dr:
                    break;
            }
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void treeviewProptype_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Text)
            {
                case "Temperature":

                    break;
            }
        }
    }
}