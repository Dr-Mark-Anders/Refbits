using System.Collections.Generic;
using System.Windows.Forms;

namespace Units
{
    public partial class CopyStreamDLG : Form
    {
        internal DrawMaterialStream stream;
        private List<DrawMaterialStream> streams;

        internal CopyStreamDLG(List<DrawMaterialStream> streams)
        {
            InitializeComponent();

            this.streams = streams;

            for (int i = 0; i < streams.Count; i++)
            {
                comboBox1.Items.Add(streams[i].Name);
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, System.EventArgs e)
        {
            foreach (var item in streams)
            {
                if (item.Name == comboBox1.SelectedItem.ToString())
                {
                    stream = item;
                    return;
                }
            }
            stream = null;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            stream = null;
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}