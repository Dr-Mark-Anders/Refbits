using DialogControls;
using ModelEngine;

using ModelEngine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace Units.PortForm
{
    public partial class PortPropertyForm2 : Form
    {
        private readonly UOMDisplayList displaylist;
        private readonly Port_Material port;

        public DataGridView DGV { get => portworksheet.DGV; }
        public DrawMaterialStream drawMaterialStream { get; set; }

        public bool IsDirty { get; internal set; }

        private void PortProperty_ValueChanged(object sender, EventArgs e)
        {
            RaiseChangeEvent();
        }

        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        protected virtual void RaiseChangeEvent()
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            ValueChanged?.Invoke(this, new EventArgs());
        }

        public PortPropertyForm2(Port_Material port_Material, UOMDisplayList displayList)
        {
            InitializeComponent();
            port = port_Material;
            this.displaylist = displayList;
            portworksheet.ValueChanged += PortProperty_ValueChanged;
            portworksheet.PortsPropertyWorksheetInitialise(port, displaylist);
            if (port_Material is not null)
                txtStreamName.Text = port_Material.Owner.Name;
#if DEBUG
            FlashButton.Visible = true;
            LoadTestDataBtn.Visible = true;
#else
            FlashButton.Visible = false;
            LoadTestDataBtn.Visible = false;
#endif
        }

        public PortPropertyForm2(DrawMaterialStream stream, UOMDisplayList displayList)
        {
            InitializeComponent();
            this.drawMaterialStream = stream;
            port = stream.Stream.GetPortIn;
            this.displaylist = displayList;
            txtStreamName.Text = stream.Name;
        }

        private void SteamPropsForm_Load(object sender, EventArgs e)
        {
            portworksheet.PortsPropertyWorksheetInitialise(port, displaylist);
        }

        private void portPropertyWorksheet1_Load(object sender, EventArgs e)
        {
            portworksheet.DrawMaterialStream = drawMaterialStream;
        }

        private void btnComponents_Click(object sender, EventArgs e)
        {
            ComponenetSelection cs = new(drawMaterialStream);
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

            if (saveFileDialog1.ShowDialog() == DialogResult.OK
                && (myStream = saveFileDialog1.OpenFile()) != null)
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                b.Serialize(myStream, drawMaterialStream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                // Code to write the stream goes here.
                myStream.Close();
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
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    drawMaterialStream = (DrawMaterialStream)b.Deserialize(myStream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                    // Code to write the stream goes here.
                    myStream.Close();
                }

                if (drawMaterialStream != null)
                {
                    portworksheet.DrawMaterialStream = drawMaterialStream;
                    portworksheet.PortsPropertyWorksheetInitialise(drawMaterialStream.Port, displaylist);
                }

                this.port.cc = drawMaterialStream.Components.Clone();
                this.port.Properties = drawMaterialStream.Port.Properties.Clone();
                if (this.port.MolarFlow_.IsKnown)
                    this.port.MolarFlow_.origin = SourceEnum.Input;

                port.IsDirty = true;
                this.port.cc.Origin = SourceEnum.Input;
            }
        }

        public static Port_Material LoadData(string path)
        {
            Port_Material port = new();

            Stream myStream;
            DrawMaterialStream dms = new();
            OpenFileDialog openFileDialog1 = new();
            BinaryFormatter b = new();

            openFileDialog1.Filter = "data files (*.prp)|*.prp";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            // openFileDialog1.FileName = path;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    dms = (DrawMaterialStream)b.Deserialize(myStream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }

            port.cc = dms.Components.Clone();
            port.Properties = dms.Port.Properties.Clone();
            if (port.MolarFlow_.IsKnown)
                port.MolarFlow_.origin = SourceEnum.Input;

            port.IsDirty = true;
            port.cc.Origin = SourceEnum.Input;

            return port;
        }

        internal void UpdateValues()
        {
            portworksheet.UpdateValues();
        }

        private void txtStreamName_Validated(object sender, EventArgs e)
        {
            drawMaterialStream.Name = txtStreamName.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            port.Flash(true);
            UpdateValues();
        }

        private void PortPropertyForm2_KeyDown(object sender, KeyEventArgs e)
        {
            //portProperty.dgv
        }

        private void LoadTestData(object sender, EventArgs e)
        {
            drawMaterialStream = new DrawMaterialStream();
            List<string> list = new List<string>();
            list.AddRange(DefaultStreams.Names());
            SelectionDLG dlg = new(list.ToArray());

            dlg.ShowDialog();

            switch (dlg.DLGResult)
            {
                case "ReducedUralsCrude":
                    drawMaterialStream.Components.Add(DefaultStreams.ReducedUralsCrude());
                    break;

                case "ShortResidue3":
                    drawMaterialStream.Components.Add(DefaultStreams.ShortResidue3());
                    break;

                case "ShortResidue2":
                    drawMaterialStream.Components.Add(DefaultStreams.ShortResidue2());
                    break;

                case "ShortResidue":
                    drawMaterialStream.Components.Add(DefaultStreams.ShortResidue());
                    break;

                case "Residue":
                    drawMaterialStream.Components.Add(DefaultStreams.Residue());
                    break;

                case "Residue2":
                    drawMaterialStream.Components.Add(DefaultStreams.Residue2());
                    break;

                case "CrudeUrals":
                    drawMaterialStream.Components.Add(DefaultStreams.CrudeUrals());
                    break;

                case "Crude":
                    drawMaterialStream.Components.Add(DefaultStreams.Crude());
                    break;
            }

            if (drawMaterialStream != null)
            {
                List<DrawBaseStream> streams = portworksheet.DrawMaterialStream.DrawArea.GraphicsList.ReturnStreams();
                for (int i = 0; i < streams.Count; i++)
                {
                    if(streams[i] is DrawMaterialStream dms)
                    {
                        dms.ComponentList.Clear();
                    }
                }

                StreamMaterial dbs = (StreamMaterial)port.Owner;
                FlowSheet fs = dbs.Flowsheet;
                fs.ComponentList.Add(this.drawMaterialStream.Components);
                portworksheet.DrawMaterialStream.ComponentList.AddRange(this.drawMaterialStream.ComponentList.ToArray());
                portworksheet.DrawMaterialStream.Components.Origin = SourceEnum.Input;
                //portworksheet.PortsPropertyWorksheetInitialise(this.drawMaterialStream.Port, displaylist);
            }
        }

        private void ViewOutPort_Click(object sender, EventArgs e)
        {
            Port_Material outport = (Port_Material)port.Owner.GetPortOut;
            portworksheet.DGV.Rows.Clear();
            portworksheet.PortsPropertyWorksheetInitialise(outport, displaylist);
        }

        private void btnAssayCreate_Click(object sender, EventArgs e)
        {
            StreamMaterial sm = (StreamMaterial)port.Owner;
            Distillations distform;
            if (sm is not null)
            {
                distform = new Distillations(sm);
                distform.ShowDialog();
                sm.isassaydefined = true;
            }

            CreateAssayClass assay = new CreateAssayClass();
            assay.ProcessSimpleStreamAssay(sm.plantdata, port.cc);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Port_Material outport = (Port_Material)port.Owner.GetPortOut;
            if (outport is not null)
                outport.Flash();
        }
    }
}