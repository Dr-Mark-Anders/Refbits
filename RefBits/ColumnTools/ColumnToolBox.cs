using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security;
using System.Windows.Forms;

namespace Units
{
    public partial class rbColumnToolBox : Form
    {
        private DrawArea drawarea;

        internal rbColumnToolBox(DrawArea da)
        {
            drawarea = da;
            InitializeComponent();
            LoadSettingsFromRegistry();
        }

        private const string registryPath = "Software\\RefBitsSoftware\\RefBits";

        public void LoadSettingsFromRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);

                this.Top = (int)key.GetValue("ColumnTop", 10);
                this.Left = (int)key.GetValue("ColumnLeft", 10);
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

        ///<summary>
        ///SaveapplicationsettingstotheRegistry
        ///</summary>
        public void SaveSettingsToRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);

                key.SetValue("ColumnLeft", this.Left);
                key.SetValue("ColumnTop", this.Top);
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

        private void HandleRegistryException(Exception ex)
        {
            Trace.WriteLine("Registryoperationfailed:" + ex.Message);
        }

        private void BTNTraysection_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Trays;
        }

        private void BTNCondenser_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Condenser;
        }

        private void BTNReboiler_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Reboiler;
        }

        private void BTNPointer_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Pointer;
        }

        private void rbToolBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel=true;
            //this.Hide();
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            drawarea.ActiveTool = DrawArea.DrawToolType.Stream;
        }
    }
}