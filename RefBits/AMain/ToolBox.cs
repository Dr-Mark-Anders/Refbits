using Main.Images;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Security;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Units
{
    public partial class RbToolBox : DockContent
    {
        private readonly DrawArea drawarea;
        private readonly int ImageSize = 30;

        internal RbToolBox(DrawArea da)
        {
            drawarea = da;
            InitializeComponent();
            LoadSettingsFromRegistry();

            this.BTNPointer.Image = Images.ResizeImage(Images.Pointer(), ImageSize - 5, ImageSize - 5);
            this.BTNColumn.Image = Images.ResizeImage(Images.Column(), ImageSize, ImageSize);

            this.BTNStream.Image = Images.ResizeImage(Images.Stream(), ImageSize, ImageSize);
            //this.BTNStream.ImageAlign = ContentAlignment.MiddleCenter;

            this.BTNPump.Image = Images.ResizeImage(Images.Pump(), ImageSize, ImageSize);
            this.BTNDivider.Image = Images.ResizeImage(Images.Divider(), ImageSize, ImageSize);
            this.BTNMixer.Image = Images.ResizeImage(Images.Mixer(), ImageSize, ImageSize);
            this.BTNValve.Image = Images.ResizeImage(Images.Valve(), ImageSize, ImageSize);
            this.BTNExpander.Image = Images.ResizeImage(Images.Expander(), ImageSize, ImageSize);
            this.BTNCompressor.Image = Images.ResizeImage(Images.Compressor(), ImageSize, ImageSize);
            this.BTNHeater.Image = Images.ResizeImage(Images.Heater(), ImageSize, ImageSize);
            this.BTNExchanger.Image = Images.ResizeImage(Images.Exchanger(), ImageSize, ImageSize);
            this.BTNSep.Image = Images.ResizeImage(Images.Separator(), ImageSize, ImageSize);
            this.BTNCooler.Image = Images.ResizeImage(Images.Cooler(), ImageSize, ImageSize);
            this.BTNRecycle.Image = Images.ResizeImage(Images.Recycle(), ImageSize, ImageSize);
            this.BTNCOKER.Image = Images.ResizeImage(Images.Coker(), ImageSize, ImageSize);
            this.BTNBlender.Image = Images.ResizeImage(Images.Blender(), ImageSize, ImageSize);
            this.BTN3Phase.Image = Images.ResizeImage(Images.ThreePhaseSep(), ImageSize, ImageSize);
            this.BTNCompSplitter.Image = Images.ResizeImage(Images.CompSplitter(), ImageSize, ImageSize);
            this.BTNAssay.Image = Images.ResizeImage(Images.GenericAssay(), ImageSize, ImageSize);
            this.BTNAssayCutter.Image = Images.ResizeImage(Images.AssayCutter(), ImageSize, ImageSize);
            this.BTNGibbsRx.Image = Images.ResizeImage(Images.GibbsRx(), ImageSize, ImageSize);
            this.BTNReformer.Image = Images.ResizeImage(Images.Reformer(), ImageSize, ImageSize);
            this.BTNISOM.Image = Images.ResizeImage(Images.Isom(), ImageSize, ImageSize);
            this.BTNFCC.Image = Images.ResizeImage(Images.FCC(), ImageSize, ImageSize);
            this.BTNVisbreaker.Image = Images.ResizeImage(Images.Visbreaker(), ImageSize, ImageSize);
            this.BTNDataTable.Image = Images.ResizeImage(Images.Pump(), ImageSize, ImageSize);

            this.BTNPipe.Image = Images.ResizeImage(Images.Pipe(), ImageSize, ImageSize / 4);
            this.BTNPipe.ImageAlign = ContentAlignment.MiddleCenter;

            this.BTNSpreadsheet.Image = Images.ResizeImage(Images.Spreadsheet(), ImageSize, ImageSize);
            this.BTNCaseStudy.Image = Images.ResizeImage(Images.CaseStudy(), ImageSize, ImageSize);

            this.BTNPlugFlowRx.Image = Images.ResizeImage(Images.PlugFlowRx(), ImageSize, ImageSize / 3);
            this.BTNPlugFlowRx.ImageAlign = ContentAlignment.MiddleCenter;

            this.BTNLLE.Image = Images.ResizeImage(Images.Column(), ImageSize, ImageSize);
            // this.BTNLLE.ImageAlign = ContentAlignment.MiddleCenter;

            this.BTNAdjust2.Image = Images.ResizeImage(Images.Adjust(), ImageSize, ImageSize);
            //this.BTNAdjust2.ImageAlign = ContentAlignment.MiddleCenter;

            this.BTNSet.Image = Images.ResizeImage(Images.Set(), ImageSize, ImageSize);
            // this.BTNSet.ImageAlign = ContentAlignment.MiddleCenter;
        }

        private const string registryPath = "Software\\RefBitsSoftware\\RefBits";

        public void LoadSettingsFromRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);

                this.Top = (int)key.GetValue("Top", 10);
                this.Left = (int)key.GetValue("Left", 10);
            }
            catch (ArgumentNullException ex)
            {
                HandleRegistryException(ex);
            }
            catch (SecurityException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleRegistryException(ex);
            }
        }

        /// <summary>
        /// Save application settings to the Registry
        /// </summary>
        public void SaveSettingsToRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);

                key.SetValue("Left", this.Left);
                key.SetValue("Top", this.Top);
            }
            catch (SecurityException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleRegistryException(ex);
            }
        }

        private static void HandleRegistryException(Exception ex)
        {
            Trace.WriteLine("Registry operation failed: " + ex.Message);
        }

        private void BTNStream_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Stream;
        }

        private void BTNPump_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Pump;
        }

        private void BTNDivider_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Divider;
        }

        private void BTNBlender_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Blender;
        }

        private void BTNValve_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Valve;
        }

        private void BTNHeater_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Heater;
        }

        private void BTNColumn_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Column;
        }

        private void BTNAssay_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.AssayFeed;
        }

        private void BTNRecycle_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Recycle;
        }

        private void BTNAssInput_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.AssayInput;
        }

        private void BTNReformer_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Reformer;
        }

        private void BTNFCC_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.FCC;
        }

        private void BTNISOM_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Isom;
        }

        private void BTNCOKER_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Coker;
        }

        private void BTNVisbreaker_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Visbreaker;
        }

        private void BTNMixer_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Mixer;
        }

        private void BTNPointer_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Pointer;
        }

        private void BTNExpander_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Expander;
        }

        private void BTNCompressor_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Compressor;
        }

        private void BTNExchanger_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Exchanger;
        }

        private void BTNSep_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Separator;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.ThreePhaseSep;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.GenericAssay; ;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.AssayCutter;
        }

        private void btnCooler_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Cooler;
        }

        private void CompSplitter_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.CompSplitter;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.GibbsRX;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Summary;
        }

        private void btnPipe_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Pipe;
        }

        private void btnSheet_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Spreadsheet;
        }

        private void btnCaseStudy_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.CaseStudy;
        }

        private void btnPlugFlow_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.PlugFlowRx;
        }

        public void DoDragDrop(DrawArea.DrawToolType tool)
        {
            this.DoDragDrop(tool, DragDropEffects.Copy);
        }

        private void RbToolBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left
                && drawarea.ActiveTool != DrawArea.DrawToolType.Pointer)
            {
                flowLayoutPanel1.DoDragDrop(sender, DragDropEffects.Copy);
            }
        }

        private void FlowLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left
                && drawarea.ActiveTool != DrawArea.DrawToolType.Pointer)
            {
                flowLayoutPanel1.DoDragDrop(sender, DragDropEffects.Copy);
            }
            else
            {
            }
        }

        private void BTNDivider_MouseDown(object sender, MouseEventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Divider;
            //drawarea.Owner.propertiesgrid.propertyGrid.SelectedObject = sender;
        }

        private void BTN_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sender is Button b)
            {
                drawarea.ActiveTool = b.Text switch
                {
                    "Pump" => DrawArea.DrawToolType.Pump,
                    "Divider" => DrawArea.DrawToolType.Divider,
                    "Exchanger" => DrawArea.DrawToolType.Exchanger,
                    "Separator" => DrawArea.DrawToolType.Separator,
                    "3PSeparator" => DrawArea.DrawToolType.Separator,
                    "Mixer" => DrawArea.DrawToolType.Mixer,
                    "Heater" => DrawArea.DrawToolType.Heater,
                    "Cooler" => DrawArea.DrawToolType.Cooler,
                    "Assay" => DrawArea.DrawToolType.GenericAssay,
                    "Column" => DrawArea.DrawToolType.Column,
                    "Valve" => DrawArea.DrawToolType.Valve,
                    "Expander" => DrawArea.DrawToolType.Expander,
                    "Compressor" => DrawArea.DrawToolType.Compressor,
                    "Recycle" => DrawArea.DrawToolType.Recycle,
                    "Coker" => DrawArea.DrawToolType.Coker,
                    "Blender" => DrawArea.DrawToolType.Blender,
                    "Component Splitter" => DrawArea.DrawToolType.CompSplitter,
                    "Gibbs Rx" => DrawArea.DrawToolType.GibbsRX,
                    "Reformer" => DrawArea.DrawToolType.Reformer,
                    "Isom" => DrawArea.DrawToolType.Isom,
                    "FCC" => DrawArea.DrawToolType.FCC,
                    "Visbreaker" => DrawArea.DrawToolType.Visbreaker,
                    "Pipe" => DrawArea.DrawToolType.Pipe,
                    "SpreadSheet" => DrawArea.DrawToolType.Spreadsheet,
                    "Case Study" => DrawArea.DrawToolType.CaseStudy,
                    "Stream" => DrawArea.DrawToolType.Stream,
                    "LLE" => DrawArea.DrawToolType.LLEColumn,
                    "Set" => DrawArea.DrawToolType.SetValue,
                    "Adjust" => DrawArea.DrawToolType.AdjustValue,
                    _ => DrawArea.DrawToolType.Pointer,
                };
                flowLayoutPanel1.DoDragDrop(sender, DragDropEffects.Copy);
            }
        }

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            drawarea.Owner.propertiesgrid.propertyGrid.SelectedObject = this;
        }

        private void rbToolBox_MouseClick(object sender, MouseEventArgs e)
        {
            drawarea.Owner.propertiesgrid.propertyGrid.SelectedObject = this;
        }

        private void rbToolBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //this.Hide();
            this.Dispose();
        }

        private void LLE_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.LLEColumn;
        }

        private void SetValue_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.SetValue;
        }

        private void AdjustValue_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.AdjustValue;
        }

        private void BTNAdjust_Click(object sender, EventArgs e)
        {
        }
    }
}