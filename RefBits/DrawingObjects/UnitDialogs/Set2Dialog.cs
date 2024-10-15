using ModelEngine;
using ModelEngine;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace Units
{
    public partial class SetDialog : BaseDialog, IUpdateableDialog
    {
        private readonly SetObject set;
        private StreamSignal signalstreamIn;
        private StreamSignal signalstreamOut;
        private StreamPropList propsIn;
        private StreamPropList propsOut;
        private DrawObject startObject, endObject
            ;

        public SetDialog()
        {
            InitializeComponent();
        }

        internal SetDialog(DrawSet dset, DrawSignalStream streamin, DrawSignalStream streamout, DrawObject startobject, DrawObject endobject)
        {
            dset.set.connectionType = ConnectionType.notvalid;
            if (startobject is not null && endobject is not null)
            {
                startObject = startobject;
                endObject = endobject;
                dset.set.connectionType = ConnectionType.in_out;
            }
            else if (startobject is null)
            {
                startObject = endobject;
                endObject = endobject;
                dset.set.connectionType = ConnectionType.outout;
            }
            else
            {
                startObject = startobject;
                endObject = startobject;
                dset.set.connectionType = ConnectionType.inin;
            }



            if (streamin is not null && streamout is not null)
            {
                signalstreamIn = streamin.Stream;
                signalstreamOut = streamout.Stream;
            }
            else if (streamin is null)
            {
                signalstreamIn = streamout.Stream;
                signalstreamOut = streamout.Stream;
            }
            else
            {
                signalstreamIn = streamin.Stream;
                signalstreamOut = streamin.Stream;
            }

            set = dset.set;
            InitializeComponent();
        }

        private void AdjustDialog_Load(object sender, System.EventArgs e)
        {
            StreamPropList propsdefault = new();

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("StreamProperty");

            switch (startObject)
            {
                case DrawMaterialStream dms:
                    this.propsIn = dms.Port.Properties;
                    foreach (KeyValuePair<ePropID, StreamProperty> item in this.propsIn)
                        if (item.Value.IsInput)
                            treeView1.Nodes[0].Nodes.Add(item.Key.ToString(), item.Value.Name);
                    break;

                case DrawRectangle dr:
                    break;
            }

            treeView1.ExpandAll();
            treeView1.SelectedNode = treeView1.Nodes[0].Nodes[set.MVpropid.ToString()];



            treeView2.Nodes.Clear();
            treeView2.Nodes.Add("StreamProperty");

            switch (endObject)
            {
                case DrawMaterialStream dms:
                    this.propsOut = dms.Port.Properties;

                    foreach (KeyValuePair<ePropID, StreamProperty> item in this.propsOut)
                        if (!item.Value.IsInput)

                            treeView2.Nodes[0].Nodes.Add(item.Key.ToString(), item.Value.Name);
                    break;

                case DrawRectangle dr:
                    break;
            }

            treeView2.ExpandAll();
            treeView2.SelectedNode = treeView2.Nodes[0].Nodes[set.CVpropid.ToString()];

            if (set != null && set.Dataout != null)
                uom2.UOMprop = set.Dataout;

            txtTargetValue.Text = set.FinalCV.ToString();

            this.Invalidate();
            this.Refresh();

            if (set != null)
                set.Solve();
            // UpdateFinalTargetValue();
        }

        private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            ePropID prop;

            if (propsIn is null)
            {
                uomTXTMVValue.UOMprop = null;
                return;
            }

            switch (e.Node.Text)
            {
                case "Temperature":
                    uomTXTMVValue.UOMprop = propsIn[ePropID.T];
                    break;

                case "Pressure":
                    uomTXTMVValue.UOMprop = propsIn[ePropID.P];
                    break;

                case "MassFlow":
                    uomTXTMVValue.UOMprop = propsIn[ePropID.MF];
                    break;

                case "Molar Flow":
                    uomTXTMVValue.UOMprop = propsIn[ePropID.MOLEF];
                    break;

                case "MolarEnthalpy":
                    uomTXTMVValue.UOMprop = propsIn[ePropID.H];
                    break;

                case "MolarEntropy":
                    uomTXTMVValue.UOMprop = propsIn[ePropID.S];
                    break;

                case "LiquidVolumeFlow":
                    uomTXTMVValue.UOMprop = propsIn[ePropID.VF];
                    break;

                case "VapourFraction":
                    uomTXTMVValue.UOMprop = propsIn[ePropID.Q];
                    break;
            }

            prop = uomTXTMVValue.UOMprop.Propid;
            set.MVpropid = prop;

            this.Invalidate();
            this.Refresh();
        }

        private void treeView2_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            ePropID prop;

            if (propsOut is null)
            {
                uomTXTCVValue.UOMprop = null;
                return;
            }

            switch (e.Node.Text)
            {
                case "Temperature":
                    uomTXTCVValue.UOMprop = propsOut[ePropID.T];
                    break;

                case "Pressure":
                    uomTXTCVValue.UOMprop = propsOut[ePropID.P];
                    break;

                case "Mass Flow":
                    uomTXTCVValue.UOMprop = propsOut[ePropID.MF];
                    break;

                case "Molar Flow":
                    uomTXTCVValue.UOMprop = propsOut[ePropID.MOLEF];
                    break;

                case "Molar Enthalpy":
                    uomTXTCVValue.UOMprop = propsOut[ePropID.H];
                    break;

                case "Molar Entropy":
                    uomTXTCVValue.UOMprop = propsOut[ePropID.S];
                    break;

                case "LiquidVolume Flow":
                    uomTXTCVValue.UOMprop = propsOut[ePropID.VF];
                    break;

                case "Vapour Fraction":
                    uomTXTCVValue.UOMprop = propsOut[ePropID.Q];
                    break;
            }

            prop = uomTXTCVValue.UOMprop.Propid;
            set.CVpropid = prop;
            // adjust.TargetValue = uomTextBox2.UOMprop.Value;
            // adjust.Solve();
            //  UpdateFinalTargetValue();

            this.Invalidate();
            this.Refresh();
        }

        private void txDelta_TextChanged(object sender, System.EventArgs e)
        {
            if (double.TryParse(txtTargetValue.Text, out double res))
            {
                if (double.TryParse(txDelta.Text, out double delta))
                    set.offset = delta;
                if (double.TryParse(txtBoxMult.Text, out double mult))
                    set.mult = mult;
                // adjust.CalcTarget();
            }

            // UpdateFinalTargetValue();

            this.Invalidate();
            this.Refresh();
        }

        private void txtBoxMult_TextChanged(object sender, System.EventArgs e)
        {
            if (double.TryParse(txtTargetValue.Text, out double res))
            {
                if (double.TryParse(txDelta.Text, out double delta))
                    set.offset = delta;
                if (double.TryParse(txtBoxMult.Text, out double mult))
                    set.mult = mult;
                // adjust.CalcTarget();
            }

            // UpdateFinalTargetValue();

            this.Invalidate();
            this.Refresh();
        }

        private void txtTargetValue_TextChanged(object sender, System.EventArgs e)
        {
            if (double.TryParse(txtTargetValue.Text, out double res))
            {
                set.FinalCV = res;
                //if (double.TryParse(txDelta.Text, out double delta))
                //     adjust.offset = delta;
                // if (double.TryParse(txtBoxMult.Text, out double mult))
                //     adjust.mult = mult;
                //adjust.CalcTarget();
            }

            //UpdateFinalTargetValue();

            this.Invalidate();
            this.Refresh();
        }

        /* public void UpdateFinalTargetValue()
         {
             if (adjust != null && adjust.Dataout != null)
             {
                 uom2.UOMprop = new UOMProperty(adjust.CVpropid);
                 uom2.UOMprop.BaseValue = (adjust.TargetValue + adjust.offset) * adjust.mult;
                 uom2.Value = uom2.UOMprop.BaseValue;
                 uom2.Invalidate();
                 uom2.Refresh();
                 uom2.TextBox.Invalidate();
                 uom2.TextBox.Refresh();
             }

             this.Invalidate();
             this.Refresh();
         }*/

    }
}