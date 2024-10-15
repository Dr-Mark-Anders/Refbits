using ModelEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Units;

namespace RefbitsProperties
{
    public partial class PropsForm : Form
    {
        private readonly GraphicsList gl = new();
        private readonly FlowSheet fl = new();
        private DrawMaterialStream stream = new();
        private readonly UOMDisplayList units = new();

        public PropsForm()
        {
            InitializeComponent();
            fl.Add(stream.Stream);
        }

        private void SteamPropsForm_Load(object sender, EventArgs e)
        {
            portProperty.PortsPropertyWorksheetInitialise(stream.Port, units);
        }

        private void portPropertyWorksheet1_Load(object sender, EventArgs e)
        {
            portProperty.DrawMaterialStream = stream;
        }

        private void btnComponents_Click(object sender, EventArgs e)
        {
            ComponenetSelection cs = new(gl, fl);
            cs.ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SerializeNow();
        }

        public void SerializeNow()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new();
            BinaryFormatter b = new();

            saveFileDialog1.Filter = "data files (*.prp)|*.prp";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    b.Serialize(myStream, stream);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new();
            BinaryFormatter b = new();

            openFileDialog1.Filter = "data files (*.prp)|*.prp";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    stream = (DrawMaterialStream)b.Deserialize(myStream);
                    // Code to write the stream goes here.
                    myStream.Close();
                }

                if (stream != null)
                {
                    //stream.PortIn.Flash(calcderivatives:true);
                    portProperty.DrawMaterialStream = stream;
                    portProperty.PortsPropertyWorksheetInitialise(stream.Port, units);
                }
            }
        }
    }
}