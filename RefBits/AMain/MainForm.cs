using DocToolkit;
using Extensions;
using Microsoft.Win32;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Windows.Forms;
using Units.CaseStudy;
using Units.PortForm;
using WeifenLuo.WinFormsUI.Docking;

namespace Units
{
    public partial class MainForm : Form
    {
        #region Members

        private readonly DrawArea drawArea = new("Top Level Draw Area");
        public MessagesForm messages = new();
        public PropertyGridForm propertiesgrid = new();

        public static DocManager DocManager { get; set; }
        private DragDropManager dragDropManager;
        private MruManager mruManager;
        private readonly PersistWindowState persistState;
        private readonly SetupSimulationForm setup;

        private string argumentFile = "";   // file name from command line

        private const string registryPath = "Software\\RefBits\\DrawTools";

        #endregion Members

        #region Properties

        /// <summary>
        /// File name from the command line
        /// </summary>
        public string ArgumentFile
        {
            get
            {
                return argumentFile;
            }
            set
            {
                argumentFile = value;
            }
        }

        /// <summary>
        /// Get reference to Edit menu item.
        /// Used to show context menu in DrawArea class .
        /// </summary>
        /// <value></value>
        public ToolStripMenuItem ContextParent
        {
            get
            {
                return editToolStripMenuItem;
            }
        }

        #endregion Properties

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            toolbox = new(this.drawArea);
            setup = new(this);

            persistState = new PersistWindowState(registryPath, this);

            dockPanel1.Theme = new VS2015BlueTheme();
            /*  dockPanel1.Theme.ColorPalette.TabSelectedActive.Button = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.TabButtonUnselectedTabHoveredButtonPressed.Background = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.TabSelectedActive.Background = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.TabButtonUnselectedTabHoveredButtonHovered.Background = Color.Goldenrod;
              //dockPanel1.Theme.ColorPalette.MainWindowActive.Background = Color.Goldenrod;

              //   dockPanel1.Theme.ColorPalette.ToolWindowCaptionActive.Grip = Color.Goldenrod;
              // dockPanel1.Theme.ColorPalette.ToolWindowCaptionActive.Background = Color.Goldenrod;
              // dockPanel1.Theme.ColorPalette.TabSelectedInactive.Background = Color.Goldenrod;
              //  dockPanel1.Theme.ColorPalette.TabUnselected.Background = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.ToolWindowCaptionActive.Background = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.ToolWindowCaptionButtonActiveHovered.Background = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.ToolWindowCaptionButtonActiveHovered.Glyph = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.ToolWindowTabSelectedActive.Background = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.ToolWindowTabSelectedInactive.Background = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.ToolWindowSeparator = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.ToolWindowTabUnselected.Background = Color.Goldenrod;
              dockPanel1.Theme.ColorPalette.ToolWindowTabUnselectedHovered.Background = Color.Goldenrod;

              dockPanel1.Theme.ColorPalette.AutoHideStripDefault.Background = Color.Goldenrod;

              dockPanel1.Theme.ColorPalette.OverflowButtonDefault.Glyph = Color.Goldenrod;
              //dockPanel1.Theme.ColorPalette.ToolWindowTabSelectedActive = Color.Goldenrod;

              dockPanel1.Theme.ColorPalette.CommandBarToolbarOverflowHovered.Background = Color.Goldenrod;*/

            dockPanel1.DockLeftPortion = 150;
            dockPanel1.DockBottomPortion = 200;
            dockPanel1.DockRightPortion = 300;

            messages.Show(dockPanel1, DockState.DockBottom);
            toolbox.Show(dockPanel1, DockState.DockBottom);
            drawArea.Show(dockPanel1, DockState.Document);
            setup.Show(dockPanel1, DockState.DockLeft);
#if DEBUG
            propertiesgrid.Show(dockPanel1, DockState.DockRight);
#endif

            drawArea.Initialize(this);
            drawArea.ContextMenuStrip = MainFormContextMenu;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        #endregion Constructor

        #region Toolbar Event Handlers

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            Commandnew();
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            CommandOpen();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            CommandSave();
        }

        private void toolStripButtonPointer_Click(object sender, EventArgs e)
        {
            CommandPointer();
        }

        private void toolStripButtonRectangle_Click(object sender, EventArgs e)
        {
            CommandRectangle();
        }

        private void toolStripButtonEllipse_Click(object sender, EventArgs e)
        {
            CommandEllipse();
        }

        private void toolStripButtonLine_Click(object sender, EventArgs e)
        {
            CommandLine();
        }

        private void toolStripButtonPencil_Click(object sender, EventArgs e)
        {
            CommandPolygon();
        }

        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            CommandAbout();
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            CommandUndo();
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            CommandRedo();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Commandnew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandOpen();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.GraphicsList.SelectAll();
            drawArea.Refresh();
        }

        private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.GraphicsList.UnselectAll();
            drawArea.Refresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandDelete command = new(drawArea.GraphicsList);

            if (drawArea.DeleteSelection())
            {
                DrawArea.SetDirty();
                drawArea.Refresh();
                drawArea.AddCommandToHistory(command);
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandDeleteAll command = new(drawArea.GraphicsList);

            if (drawArea.GraphicsList.Clear())
            {
                DrawArea.SetDirty();
                drawArea.Refresh();
                drawArea.AddCommandToHistory(command);
            }
        }

        private void moveToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawArea.GraphicsList.MoveSelectionToFront())
            {
                DrawArea.SetDirty();
                drawArea.Refresh();
            }
        }

        private void moveToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawArea.GraphicsList.MoveSelectionToBack())
            {
                DrawArea.SetDirty();
                drawArea.Refresh();
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawArea.GraphicsList.ShowPropertiesDialog(drawArea))
            {
                DrawArea.SetDirty();
                drawArea.Refresh();
            }
        }

        private void pointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandPointer();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandRectangle();
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandEllipse();
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandLine();
        }

        private void pencilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandPolygon();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandAbout();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandUndo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandRedo();
        }

        /// <summary>
        /// removes assembly name from type resolution
        /// </summary>
        public class TypeOnlyBinder : SerializationBinder
        {
            private static SerializationBinder defaultBinder = new BinaryFormatter().Binder;

            public override Type BindToType(string assemblyName, string typeName)
            {
                string toassname = assemblyName.Split(',')[0];
                Debug.Print(toassname + " " + typeName);

                if (assemblyName.Equals("NA"))
                    return Type.GetType(typeName);
                else
                    //return defaultBinder.BindToType(assemblyName, typeName);
                    return Type.GetType(typeName);

                /*try
                {
                    string toassname = assemblyName.Split(',')[0];
                    Assembly[] asmblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly ass in asmblies)
                    {
                        if (ass.FullName.Split(',')[0] == toassname)
                        {
                            ttd = ass.GetType(typeName);
                            break;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e.Message);
                }*/
                // return ttd;
            }

            public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                // but null out the assembly name
                assemblyName = "NA";
                typeName = serializedType.FullName;
            }

            private static object locker = new object();
            private static TypeOnlyBinder _default = null;

            public static TypeOnlyBinder Default
            {
                get
                {
                    lock (locker)
                    {
                        if (_default == null)
                            _default = new TypeOnlyBinder();
                    }
                    return _default;
                }
            }
        }

        private sealed class VersionDeserializer : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Debug.Print(assemblyName + " " + typeName);
                Type deserializeType = null;
                String thisAssembly = Assembly.GetExecutingAssembly().FullName;
                deserializeType = Type.GetType(String.Format("{0}, {1}",
                    typeName, thisAssembly));

                return deserializeType;
            }
        }

        /// <summary>
        /// Load document from the stream supplied by DocManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void docManager_LoadFileEvent(object sender, SerializationEventArgs e)
        {
            //e.Formatter.Binder = new TypeOnlyBinder();
            drawArea.Flowsheet = GlobalModel.Flowsheet;
            DrawArea.ComponentList.Clear();
            // DocManager asks to load document from supplied stream
            try
            {
                drawArea.GraphicsList = (GraphicsList)e.Formatter.Deserialize(e.SerializationStream);
                drawArea.GraphicsList.errorChangedEvent += drawArea.graphicsList_errorChangedEvent;
                drawArea.GraphicsList.graphicsValueChangedEvent += drawArea.graphicsValueChanged;
                drawArea.ResetDrawArea();
            }
            catch (ArgumentNullException ex)
            {
                HandleLoadException(ex, e);
            }
            catch (SerializationException ex)
            {
                HandleLoadException(ex, e);
            }
            catch (SecurityException ex)
            {
                HandleLoadException(ex, e);
            }

            drawArea.GraphicsList.ReconnectObjects();

            for (int i = 0; i < drawArea.Tablecontrols.Count; i++)
            {
                drawArea.Controls.Remove(drawArea.Tablecontrols[i]);
            }

            drawArea.Tablecontrols.Clear();

            try
            {
                drawArea.Tablecontrols = (List<TableControl>)e.Formatter.Deserialize(e.SerializationStream);
            }
            catch
            {
            }

            foreach (var item in drawArea.Tablecontrols)
            {
                item.Drawarea = drawArea;
                item.UpdateData();
                drawArea.Controls.Add(item);
                Helper.ControlMover.Init(item);
            }

            //GlobalModel.Flowsheet.UpdateAllPortComponents(drawArea.GraphicsList.Components);

            List<DrawName> ldn = drawArea.GraphicsList.ReturnDrawNames();

            foreach (DrawName dn in ldn) // remove drawnames
                drawArea.GraphicsList.Remove(dn);

            drawArea.GraphicsList.AddNameObjects(); // displayed text names
            drawArea.GraphicsList.UpdateAllConnections();  // reconnect ports to connected ports in flowsheet

            DrawArea.StaticStreamList.AddRange(drawArea.GraphicsList.ReturnStreams());
            //drawArea.GraphicsList.OnRequestParentObject += new  Func<object>(delegate { return   drawArea; });

            if (drawArea.GraphicsList.Components != null)
            {
                drawArea.Flowsheet.ComponentList.Clear();
                drawArea.Flowsheet.ComponentList.Add(drawArea.GraphicsList.Components);
                drawArea.Flowsheet.ComponentList.ClearMoleFractions();
            }

            drawArea.GraphicsList.ResetEvents();

            foreach (DrawObject d in drawArea.GraphicsList)
            {
                switch (d)
                {
                    case DrawColumn dc:
                        dc.UpdateAttachedModel();
                        dc.ResetDrawArea();
                        dc.graphicslist.ResetEvents();
                        break;

                    case DrawMaterialStream dms:
                        dms.OnRequestParent += new Func<DrawArea>(delegate { return GetDrawArea(); });
                        break;
                    // case SubFlowSheet sfs:
                    //     break;
                    default:
                        break;
                }
            }
            drawArea.SetUpModelStack(GlobalModel.Flowsheet);
            CleanPortProperties(drawArea.Flowsheet.ModelStack);
        }

        private DrawArea GetDrawArea()
        {
            return this.drawArea;
        }

        /// <summary>
        /// Save document to stream supplied by DocManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void docManager_SaveEvent(object sender, SerializationEventArgs e)
        {
            // GlobalModel.Flowsheet.ModelStack.Clear(); // only save componenet list
            // DocManager asks to save document to supplied stream
            drawArea.GraphicsList.Scale = drawArea.ScaleDraw;
            try
            {
                //e.Formatter.Serialize(e.SerializationStream, GlobalModel.flowsheet);
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                e.Formatter.Serialize(e.SerializationStream, drawArea.GraphicsList);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                e.Formatter.Serialize(e.SerializationStream, drawArea.Tablecontrols);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            }
            catch (ArgumentNullException ex)
            {
                HandleSaveException(ex, e);
            }
            catch (SerializationException ex)
            {
                HandleSaveException(ex, e);
            }
            catch (SecurityException ex)
            {
                HandleSaveException(ex, e);
            }
        }

        #endregion Toolbar Event Handlers

        #region Event Handlers

        private RbToolBox toolbox;

        public void ShowToolBox()
        {
            if (toolbox == null)
            {
                toolbox = new(this.drawArea);
                toolbox.Show();
                if (toolbox.Left > 300)
                    toolbox.Left = 200;
                if (toolbox.Top > 300)
                    toolbox.Top = 200;
            }
            else
            {
                toolbox.Hide();
                toolbox.Dispose();
                toolbox = null;
            }
        }

        [DllImport("user32.dll")]
        private static extern long LockWindowUpdate(IntPtr Handle);

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Lock Window...
                LockWindowUpdate(this.Handle);
                // Perform your paint ing / updates...

                InitializeHelperObjects();

                ResizeDrawArea();
                LoadSettingsFromRegistry();

                // Submit to Idle event to set controls state at idle time
                Application.Idle += delegate (object o, EventArgs a)
                {
                    SetStateOfControls();
                };

                // Open file passed in the command line
                if (ArgumentFile.Length > 0)
                    OpenDocument(ArgumentFile);

                // Subscribe to DropDownOpened event for each popup menu
                // (see details in MainForm_DropDownOpened)
                foreach (ToolStripItem item in menuStrip1.Items)
                    if (item.GetType() == typeof(ToolStripMenuItem))
                        ((ToolStripMenuItem)item).DropDownOpened += MainForm_DropDownOpened;
            }
            finally
            {
                // Release the lock...
                LockWindowUpdate(IntPtr.Zero);
            }
        }

        /// <summary>
        /// Resize draw area when form is resized
        /// </summary>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            //if (this.WindowState != FormWindowState.Minimized && drawArea != null)
        }

        /// <summary>
        /// Form is closing
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                if (!DocManager.CloseDocument())
                    e.Cancel = true;

            if (Toolbox != null)
            {
                Toolbox.SaveSettingsToRegistry();
                Toolbox.Dispose();
            }
            drawArea.Dispose();
            SaveSettingsToRegistry();
        }

        /// <summary>
        /// Popup menu item (File, Edit ...) is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_DropDownOpened(object sender, EventArgs e)
        {
            // Reset active tool to point er.
            // This prevents bug in rare case when non-point er tool is active, user opens
            // main main menu and after this clicks in the drawArea. MouseDown event is not
            // raised in this case (why ??), and MouseMove event works incorrectly.
            drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
        }

        #endregion Event Handlers

        #region Other Functions

        /// <summary>
        /// Set state of controls.
        /// Function is called at idle time.
        /// </summary>
        public void SetStateOfControls()
        {
            // Select active tool
            /*toolStripButtonPoint er.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Point er);
            toolStripButtonRectangle.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Rectangle);
            toolStripButtonEllipse.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Ellipse);
            toolStripButtonLine.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Line);
            toolStripButtonPencil.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Polygon);

            point erToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Point er);
            rectangleToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Rectangle);
            ellipseToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Ellipse);
            lineToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Line);
            pencilToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Polygon);*/

            bool objects = drawArea.GraphicsList.Count > 0;

            // File operations
            saveToolStripMenuItem.Enabled = objects;
            // toolStripButtonSave.Enabled = objects;
            saveAsToolStripMenuItem.Enabled = true;

            // Edit operations
            /* deleteToolStripMenuItem.Enabled = selectedObjects;
             deleteAllToolStripMenuItem.Enabled = objects;
             selectAllToolStripMenuItem.Enabled = objects;
             unselectAllToolStripMenuItem.Enabled = objects;
             moveToFrontToolStripMenuItem.Enabled = selectedObjects;
             moveToBackToolStripMenuItem.Enabled = selectedObjects;
             propertiesToolStripMenuItem.Enabled = selectedObjects;*/

            // Undo, Redo
            undoToolStripMenuItem.Enabled = drawArea.CanUndo;
            // toolStripButtonUndo.Enabled = drawArea.CanUndo;

            redoToolStripMenuItem.Enabled = drawArea.CanRedo;
            // toolStripButtonRedo.Enabled = drawArea.CanRedo;*/
        }

        /// <summary>
        /// Set draw area to all form client space except toolbar
        /// </summary>
        private void ResizeDrawArea()
        {
            Rectangle rect = this.ClientRectangle;

            //drawArea.Left = rect.Left;// + panel1.Width;
            //drawArea.Top = rect.Top;// + menuStrip1.Height + toolStrip1.Height;
            // drawArea.Width = rect.Width;// - panel1.Width;
            //drawArea.Height = rect.Height - menuStrip1.Height - toolStrip1.Height;
            dockPanel1.Top = rect.Top + menuStrip1.Height + toolStripContainer1.Height;
            dockPanel1.Height = rect.Height - menuStrip1.Height - toolStripContainer1.Height;
            dockPanel1.Left = rect.Left;
            dockPanel1.Width = rect.Width;// - -menuStrip1.Height - toolStrip1.Height;
        }

        /// Initialize helper objects from the DocToolkit Library.
        ///
        /// Called from Form1_Load. Initialized all objects except
        /// PersistWindowState wich must be initialized in the
        /// form constructor.
        /// </summary>
        private void InitializeHelperObjects()
        {
            // DocManager

            DocManagerData data = new();
            data.FormOwner = this;
            data.UpdateTitle = true;
            data.FileDialogFilter = "DrawTools files (*.dtl)|*.dtl|All Files (*.*)|*.*";
            data.NewDocName = "Untitled.dtl";
            data.RegistryPath = registryPath;

            DocManager = new DocManager(data);
            DocManager.RegisterFileType("dtl", "dtlfile", "DrawTools File");

            // Subscribe to DocManager events.
            DocManager.SaveEvent += docManager_SaveEvent;
            DocManager.LoadEvent += docManager_LoadFileEvent;

            // Make "inline subscription" using   anonymous methods.
            DocManager.OpenEvent += delegate (object sender, OpenFileEventArgs e)
            {
                // Update MRU List
                if (e.Succeeded)
                    mruManager.Add(e.FileName);
                else
                    mruManager.Remove(e.FileName);
            };

            DocManager.DocChangedEvent += delegate (object o, EventArgs e)
            {
                drawArea.Refresh();
                drawArea.ClearHistory();
            };

            DocManager.ClearEvent += delegate (object o, EventArgs e)
            {
                if (drawArea.GraphicsList != null)
                {
                    drawArea.GraphicsList.Clear();
                    drawArea.ClearHistory();
                    drawArea.Refresh();
                }
            };

            DocManager.newDocument();

            // DragDropManager
            dragDropManager = new DragDropManager(this);
            dragDropManager.FileDroppedEvent += delegate (object sender, FileDroppedEventArgs e)
            {
                OpenDocument(e.FileArray.GetValue(0).ToString());
            };

            // MruManager
            mruManager = new MruManager();
            mruManager.Initialize(
                this,                              // owner form
                recentFilesToolStripMenuItem,      // Recent Files menu item
                fileToolStripMenuItem,            // parent
                registryPath);                     // Registry path to keep MRU list*/

            mruManager.MruOpenEvent += delegate (object sender, MruFileOpenEventArgs e)
            {
                OpenDocument(e.FileName);
            };
        }

        /// <summary>
        /// Handle exception from docManager_LoadEvent function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        private void HandleLoadException(Exception ex, SerializationEventArgs e)
        {
            MessageBox.Show(this, "Open File operation failed. File name: " + e.FileName + "\n" +
                "Reason: " + ex.Message, Application.ProductName);

            e.Error = true;
        }

        /// <summary>
        /// Handle exception from docManager_SaveEvent function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        private void HandleSaveException(Exception ex, SerializationEventArgs e)
        {
            MessageBox.Show(this, "Save File operation failed. File name: " + e.FileName + "\n" +
                "Reason: " + ex.Message, Application.ProductName);

            e.Error = true;
        }

        /// <summary>
        /// Open document.
        /// Used to open file passed in command line or dropped int o the window
        /// </summary>
        /// <param name="file"></param>
        public static void OpenDocument(string file)
        {
            DocManager.OpenDocument(file);
        }

        /// <summary>
        /// Load application settings from the Registry
        /// </summary>
        private static void LoadSettingsFromRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
                DrawObject.LastUsedColor = Color.FromArgb((int)key.GetValue("Color", Color.Black.ToArgb()));
                DrawObject.LastUsedPenWidth = (int)key.GetValue("Width", 1);
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
        private static void SaveSettingsToRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
                key.SetValue("Color", DrawObject.LastUsedColor.ToArgb());
                key.SetValue("Width", DrawObject.LastUsedPenWidth);
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

        /// <summary>
        /// Set Point er draw tool
        /// </summary>
        private void CommandPointer()

        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandRectangle()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Rectangle;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandColumn()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Column;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandValve()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Valve;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandSeparator()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Separator;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandHeater()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Heater;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandPump()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Pump;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandAssayCutter()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.AssayCutter;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandReformer()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Reformer;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandIsom()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Isom;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandCoker()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Coker;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandVisbreaker()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Visbreaker;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandBlender()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Blender;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandFCC()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.FCC;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandStream()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Stream;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandMixer()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Mixer;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandCompressor()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Compressor;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandExpander()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Expander;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandAssayInput()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.AssayInput;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandRecycle()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Recycle;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandDivider()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Divider;
        }

        /// <summary>
        /// Set Ellipse draw tool
        /// </summary>
        private void CommandEllipse()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Ellipse;
        }

        /// <summary>
        /// Set Line draw tool
        /// </summary>
        private void CommandLine()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Line;
        }

        /// <summary>
        /// Set Polygon draw tool
        /// </summary>
        private void CommandPolygon()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Polygon;
        }

        /// <summary>
        /// Show About dialog
        /// </summary>
        private void CommandAbout()
        {
            FrmAbout frm = new();
            frm.ShowDialog(this);
        }

        /// <summary>
        /// Open new  file
        /// </summary>
        private static void Commandnew()
        {
            DrawArea.StaticStreamList.Clear();
            DocManager.newDocument();
        }

        private static void CommandClose()
        {
            DocManager.CloseDocument();
        }

        /// <summary>
        /// Open file
        /// </summary>
        private static void CommandOpen()
        {
            DrawArea.StaticStreamList.Clear();
            DocManager.OpenDocument("");
        }

        /// <summary>
        /// Save file
        /// </summary>
        private static void CommandSave()
        {
            DocManager.SaveDocument(DocManager.SaveType.Save);
        }

        /// <summary>
        /// Save As
        /// </summary>
        private static void CommandSaveAs()
        {
            DocManager.SaveDocument(DocManager.SaveType.SaveAs);
        }

        /// <summary>
        /// Save As
        /// </summary>
        private void CommandAssayFeed()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.AssayFeed;
        }

        /// <summary>
        /// Undo
        /// </summary>
        private void CommandUndo()
        {
            drawArea.Undo();
        }

        /// <summary>
        /// Redo
        /// </summary>
        private void CommandRedo()
        {
            drawArea.Redo();
        }

        #endregion Other Functions

        private void toolStripColumn_Click(object sender, EventArgs e)
        {
            CommandColumn();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            CommandPump();
        }

        private void toolStripMixer_Click(object sender, EventArgs e)
        {
            CommandMixer();
        }

        private void toolStripDivider_Click(object sender, EventArgs e)
        {
            CommandDivider();
        }

        private void toolStripValve_Click(object sender, EventArgs e)
        {
            CommandValve();
        }

        private void toolStripFlash_Click(object sender, EventArgs e)
        {
            CommandSeparator();
        }

        private void toolStripStream_Click(object sender, EventArgs e)
        {
            CommandStream();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            /* if (e.KeyCode == Keys.Add)
                 drawArea.ScaleDraw *= 1.1F;
             else if (e.KeyCode == Keys.Subtract)
                 drawArea.ScaleDraw /= 1.1F;*/

            if (drawArea != null)
                drawArea.Refresh();

            if (e.KeyCode == Keys.Delete)
            {
                drawArea.DeleteSelection();
                drawArea.Refresh();
            }
        }

        private void toolStripHeater_Click_1(object sender, EventArgs e)
        {
            CommandHeater();
        }

        private void insertPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.DrawArea_InsertPoint();
        }

        private void deletePointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.DrawArea_DeletePoint();
        }

        private void toolStripAssay_Click(object sender, EventArgs e)
        {
            CommandAssayFeed();
        }

        private void toolStripAssayCutter_Click(object sender, EventArgs e)
        {
            CommandAssayCutter();
        }

        private void toolStripReformer_Click(object sender, EventArgs e)
        {
            CommandReformer();
        }

        private void toolStripIsom_Click(object sender, EventArgs e)
        {
            CommandIsom();
        }

        private void toolStripFCC_Click(object sender, EventArgs e)
        {
            CommandFCC();
        }

        private void toolStripCoker_Click(object sender, EventArgs e)
        {
            CommandCoker();
        }

        private void toolStripVisbreaker_Click(object sender, EventArgs e)
        {
            CommandVisbreaker();
        }

        private void toolStripBlender_Click(object sender, EventArgs e)
        {
            CommandBlender();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandClose();
        }

        private void toolStripCompressor_Click(object sender, EventArgs e)
        {
            CommandCompressor();
        }

        private void toolStripExpander_Click(object sender, EventArgs e)
        {
            CommandExpander();
        }

        private void toolStripAssayInput_Click(object sender, EventArgs e)
        {
            CommandAssayInput();
        }

        private void streamToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void pumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void mixerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void dividerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void flashToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void columnToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void valveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void heaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void assayInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void assayToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void assayCutterToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void reformerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void isomToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void fCCToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void cokerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void visbreakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void blenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            drawArea.CalcActive = EnumCalcActive.No;
        }

        private void toolRecycle_Click(object sender, EventArgs e)
        {
            CommandRecycle();
        }

        public RbToolBox Toolbox { get => toolbox; set => toolbox = value; }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            toolbox = new(this.drawArea);
            toolbox.Show(dockPanel1, DockState.DockLeft);
        }

        private void deleteCalculatedVaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.Flowsheet.EraseNonFixedValues();
        }

        private void step1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.SolveSingleStep();
        }

        private void thermodynamicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (propertiesgrid != null && propertiesgrid.propertyGrid != null)
                propertiesgrid.propertyGrid.SelectedObject = this;
        }

        private void toolStripComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            ToolStripComboBox cb = (ToolStripComboBox)sender;
            drawArea.CalcType = cb.SelectedIndex switch
            {
                0 => EnumCalcSeq.BackProp,
                1 => EnumCalcSeq.threadedbackprop,
                2 => EnumCalcSeq.Off,
                _ => EnumCalcSeq.SeqMod,
            };
        }

        private void rotate90ClockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DrawObject> dr = drawArea.GraphicsList.GetSelected;
            foreach (var item in dr)
            {
                if (item is DrawRectangle rectangle)
                {
                    int r = (int)rectangle.Angle;
                    if (r + 90 >= 360)
                        r += 90 - 360;
                    else
                        r += 90;
                    rectangle.Angle = (enumRotationAngle)r;
                }
                drawArea.GraphicsList.UpdateStreams(((DrawRectangle)item));
            }
        }

        private void rotate90AntiClockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DrawObject> dr = drawArea.GraphicsList.GetSelected;
            foreach (var item in dr)
            {
                if (item is DrawRectangle rectangle)
                {
                    switch (rectangle.Angle)
                    {
                        case enumRotationAngle.a0:
                            rectangle.Angle = enumRotationAngle.a270;
                            break;

                        case enumRotationAngle.a90:
                            rectangle.Angle = enumRotationAngle.a0;
                            break;

                        case enumRotationAngle.a180:
                            rectangle.Angle = enumRotationAngle.a90;
                            break;

                        case enumRotationAngle.a270:
                            rectangle.Angle = enumRotationAngle.a180;
                            break;
                    }
                }
                drawArea.GraphicsList.UpdateStreams(((DrawRectangle)item));
            }
        }

        private void flipHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DrawObject> dr = drawArea.GraphicsList.GetSelected;
            foreach (var item in dr)
            {
                if (item is DrawRectangle rectangle)
                {
                    rectangle.FlipHorizontal = !rectangle.FlipHorizontal;
                    drawArea.GraphicsList.UpdateStreams(((DrawRectangle)item));

                    /*switch (rectangle.Angle)
                    {
                        case enumRotationAngle.a0:
                            break;

                        case enumRotationAngle.a90:
                            break;

                        case enumRotationAngle.a180:
                            break;

                        case enumRotationAngle.a270:
                            break;
                    }*/
                }
            }
        }

        private void flipVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DrawObject> dr = drawArea.GraphicsList.GetSelected;
            foreach (var item in dr)
            {
                if (item is DrawRectangle rectangle)
                {
                    rectangle.FlipVertical = !rectangle.FlipVertical;
                    drawArea.GraphicsList.UpdateStreams(((DrawRectangle)item));
                }
            }
        }

        private void toolStripButton1_CheckStateChanged(object sender, EventArgs e)
        {
            drawArea.SolverActive = ((ToolStripButton)sender).Checked;
        }

        public void Compositions_Click(object sender, EventArgs e)
        {
            ComponenetSelection cs = new(drawArea.GraphicsList, GlobalModel.Flowsheet);
            cs.ShowDialog();
            //GlobalModel.Flowsheet.UpdateAllPortComponents(drawArea.GraphicsList.Components);
        }

        public void Thermo_Click(object sender, EventArgs e)
        {
            ThermoCollectionForm tcf = new(drawArea.GraphicsList.Thermo);
            tcf.Show();
        }

        private void ResetValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalModel.Flowsheet.EraseNonFixedValues();
            this.Refresh();
            drawArea.Refresh();
        }

        public void WorkSheet_Click(object sender, EventArgs e)
        {
            List<DrawMaterialStream> dms = drawArea.GraphicsList.ReturnMaterialStreams();
            TableForm tf = new(dms, drawArea);
            tf.ShowDialog();
        }

        private void AbortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.Abort();
        }

        private void ViewConsistencyListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.Flowsheet.ShowConsistency();
        }

        private void ResetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.ScaleDraw = 1;
            this.Refresh();
            drawArea.Refresh();
        }

        private void FitToScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public void CaseStudy_Click(object sender, EventArgs e)
        {
            CaseStudyForm cs = new();
            cs.Show();
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
        }

        private void propertiesDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (propertiesgrid.IsDisposed)
                propertiesgrid = new();

            propertiesgrid.Show();
        }

        private void messagesWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (messages.IsDisposed)
                messages = new();

            messages.Show();
        }

        private void dockPanel1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void ribbon1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {
        }

        private void ribbon1_Click_1(object sender, EventArgs e)
        {
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void selectAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
        }

        internal Point DockControlLocation()
        {
            return new Point(dockPanel1.Left, dockPanel1.Top);
        }

        private void undoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
        }

        private void redoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
        }

        private void toolStripContainer1_Click(object sender, EventArgs e)
        {
        }

        private void toolBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolbox.Show(dockPanel1, DockState.DockLeft);
        }

        private void simulationToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setup.Show(dockPanel1, DockState.DockLeft);
        }

        private void propertyGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesgrid.Show(dockPanel1, DockState.DockRight);
        }

        private void AlighRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DrawObject> list = drawArea.GraphicsList.GetSelected;
            int Right = 0;

            foreach (DrawObject dobj in list)
            {
                if (dobj is DrawRectangle dr && dobj.Rectangle.Left > Right || Right == 0)
                    Right = dobj.Rectangle.Right;
            }

            foreach (DrawObject r in list)
            {
                if (r is DrawRectangle dr)
                    dr.Move(Right - dr.Rectangle.Right, 0);
            }
            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void BtnAlignLeft_Click(object sender, EventArgs e)
        {
            List<DrawObject> list = drawArea.GraphicsList.GetSelected;
            int Left = 0;

            foreach (DrawObject dobj in list)
            {
                if (dobj is DrawRectangle dr && dobj.Rectangle.Left < Left || Left == 0)
                    Left = dobj.Rectangle.Left;
            }

            foreach (DrawObject r in list)
            {
                if (r is DrawRectangle dr)
                    dr.Move(Left - dr.Rectangle.Left, 0);
            }
            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void btnAlignTop_Click(object sender, EventArgs e)
        {
            List<DrawObject> list = drawArea.GraphicsList.GetSelected;
            int Top = 0;

            foreach (DrawObject dobj in list)
            {
                if (dobj is DrawRectangle dr && dobj.Rectangle.Top < Top || Top == 0)
                    Top = dobj.Rectangle.Top;
            }

            foreach (DrawObject r in list)
            {
                if (r is DrawRectangle dr)
                    dr.Move(0, Top - dr.Rectangle.Top);
            }
            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void btnStripBottom_Click(object sender, EventArgs e)
        {
            List<DrawObject> list = drawArea.GraphicsList.GetSelected;
            int Bottom = 0;

            foreach (DrawObject dobj in list)
            {
                if (dobj is DrawRectangle dr && dobj.Rectangle.Bottom > Bottom || Bottom == 0)
                    Bottom = dobj.Rectangle.Bottom;
            }

            foreach (DrawObject r in list)
            {
                if (r is DrawRectangle dr)
                    dr.Move(0, Bottom - dr.Rectangle.Bottom);
            }
            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void BtnAlignCentre(object sender, EventArgs e)
        {
            List<DrawObject> list = drawArea.GraphicsList.GetSelected;
            int Centre = 0;

            foreach (DrawObject dobj in list)
                if (dobj is DrawRectangle dr && dobj.Rectangle.Center().X < Centre || Centre == 0)
                    Centre += dobj.Rectangle.Center().X / list.Count; // uise average

            foreach (DrawObject r in list)
                if (r is DrawRectangle dr)
                    dr.Move(Centre - dr.Rectangle.Center().X, 0);

            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void BtnAlignMiddle(object sender, EventArgs e)
        {
            List<DrawObject> list = drawArea.GraphicsList.GetSelected;
            int Middle = 0;

            foreach (DrawObject dobj in list)
                if (dobj is DrawRectangle dr && dobj.Rectangle.Center().Y < Middle || Middle == 0)
                    Middle += dobj.Rectangle.Center().Y / list.Count;

            foreach (DrawObject r in list)
                if (r is DrawRectangle dr)
                    dr.Move(0, Middle - dr.Rectangle.Center().Y);

            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void ViewFloatingToolBoxTool_Click(object sender, EventArgs e)
        {
            RbToolBox toolbox = new(drawArea);
            toolbox.Show();
        }

        private void ScaleToNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.ScaleDraw = 1;
            drawArea.Refresh();
        }

        private void ScaleToWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Left = 0, Top = 0, Bottom = 0, Right = 0;

            foreach (DrawObject d in drawArea.GraphicsList)
            {
                if (d.Rectangle.Left < Left)
                    Left = d.Rectangle.Left;
                if (d.Rectangle.Right > Right)
                    Right = d.Rectangle.Right;
                if (d.Rectangle.Top < Top)
                    Top = d.Rectangle.Top;
                if (d.Rectangle.Bottom > Bottom)
                    Bottom = d.Rectangle.Bottom;
            }

            int scaleX = (Right - Left) / drawArea.Width;
            int scaley = (Bottom - Top) / drawArea.Height;

            if (scaleX < 1 && scaley < 1)
                drawArea.ScaleDraw = 1;
            else
            {
                if (scaleX > scaley)
                    drawArea.ScaleDraw = 1 / scaleX;
                else
                    drawArea.ScaleDraw = 1 / scaley;
            }

            drawArea.Refresh();
        }

        private void cleanModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.Refresh();
            drawArea.SetUpModelStack(GlobalModel.Flowsheet);
            //GlobalModel.Flowsheet.EraseNonFixedValues();
            EraseNonFixedValuesDrawings();
            this.Refresh();
            drawArea.Refresh();
            //CleanModels(drawArea);
            //CleanPortProperties(drawArea.Flowsheet.ModelStack);
        }

        public void EraseNonFixedValuesDrawings()
        {
            foreach (DrawObject UO in drawArea.GraphicsList)
            {
                if (UO is null)
                    Debugger.Break();

                switch (UO)
                {
                    case DrawRectangle dr:
                        /* foreach (Port port in dr.Ports)
                         {
                             port.ClearNonInputs();
                             port.ClearEstimates();
                         }
                         dr.AttachedModel.EraseNonFixedValues();
                         dr.AttachedModel.EraseNonFixedComponents();
                         dr.AttachedModel.EraseEstimates();
                         dr.AttachedModel.IsDirty = true;
                         dr.AttachedModel.IsSolved = false;*/
                        break;

                    case DrawBaseStream sm:
                        DrawObject start = sm.startDrawObject;
                        DrawObject end = sm.endDrawObject;

                        Port_Material startport = null;
                        Port_Material endport = null;

                        if (start is DrawRectangle drs && drs.Ports.Count > 0)
                        {
                            startport = (Port_Material)sm.Ports[0];
                        }

                        if (end is DrawRectangle dre && dre.Ports.Count>0)
                        {
                            endport = (Port_Material)dre.Ports[0];
                        }

                        switch (start)
                        {
                            case DrawRecycle dr:
                                break;

                            case DrawExchanger de:
                                de.AttachedModel.EraseNonFixedValues(startport);
                                de.AttachedModel.EraseNonFixedComponents();
                                break;

                            case DrawRectangle d:
                                d.AttachedModel.EraseNonFixedValues();
                                d.AttachedModel.EraseNonFixedComponents();
                                break;
                        }
                        break;
                }
            }
        }

        public void CleanModels(DrawArea da)
        {
            foreach (DrawObject d in da.GraphicsList)
                if (d is DrawColumn dc)
                    CleanPortProperties(dc.SubFlowSheet.ModelStack);
        }

        /// <summary>
        /// Remove SG and Heat Flow
        /// </summary>
        public void CleanPortProperties(ModelStack stack)
        {
            foreach (UnitOperation d in stack)
            {
                switch (d)
                {
                    case Recycle r:
                        break;

                    case FlowSheet fs:
                        CleanPortProperties(fs.ModelStack);
                        break;

                    case UnitOperation uo:
                        foreach (var port in uo.Ports)
                        {
                            if (port.Props.ContainsKey(ePropID.SG))
                                port.Props.Remove(ePropID.SG);
                            if (port.Props.ContainsKey(ePropID.EnergyFlow))
                                port.Props.Remove(ePropID.EnergyFlow);
                        }
                        break;
                }
            }
        }

        private async void solveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            drawArea.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;



            var res = drawArea.SolveAsync();
            await res;

            foreach (Form frm in Application.OpenForms)
            {
                if (frm is PortPropertyForm2 form)
                {
                    form.UpdateValues();
                    form.Invalidate();
                    form.DGV.Invalidate();
                }
            }

            drawArea.Cursor = System.Windows.Forms.Cursors.Default;
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            drawArea.ScaleDraw *= 1.1f;

            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            drawArea.ScaleDraw /= 1.1f;

            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            drawArea.ScaleDraw = 1;

            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            int left = 0, top = 0, bottom = 0, right = 0;
            int drawHeight = drawArea.Height, drawWidth = drawArea.Width;

            List<DrawObject> list = drawArea.GraphicsList.GetSelected;

            foreach (DrawObject dobj in list)
            {
                switch (dobj)
                {
                    case DrawRectangle dr:
                        if (dr.Location.X < left)
                            left = dr.Location.X;
                        if (dr.Location.Y < top)
                            top = dr.Location.Y;
                        if (dr.Rectangle.Bottom() > bottom)
                            bottom = dr.Rectangle.Bottom();
                        if (dr.Rectangle.Right() > right)
                            right = dr.Rectangle.Right();
                        break;

                    case DrawBaseStream dbs:
                        if (dbs.Rectangle.X < left)
                            left = dbs.Rectangle.X;
                        if (dbs.Rectangle.Y < top)
                            top = dbs.Rectangle.Y;
                        if (dbs.Rectangle.Bottom() > bottom)
                            bottom = dbs.Rectangle.Bottom();
                        if (dbs.Rectangle.Right() > right)
                            right = dbs.Rectangle.Right();
                        break;
                }
            }

            int Height = bottom - top;
            int Width = right - left;
            float newZoomHeight = 1, newZoomWidth = 1, newZoom = 1;

            if (Height > drawHeight)
                newZoomHeight = drawHeight / Height;

            if (Width > drawWidth)
                newZoomWidth = drawWidth / Width;

            if (Height < drawHeight - 10)
                newZoomHeight = Height / (drawHeight - 10);

            if (Height < drawHeight - 10)
                newZoomWidth = Height / (drawHeight - 10);

            if (newZoomHeight > newZoomWidth)
                newZoom = newZoomHeight;

            if (newZoomHeight < newZoomWidth)
                newZoom = newZoomWidth;

            drawArea.ScaleDraw = newZoom;

            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            int left = (int)1E5, top = (int)1E5;
            int drawHeight = drawArea.Height, drawWidth = drawArea.Width;

            foreach (DrawObject dobj in drawArea.GraphicsList)
            {
                switch (dobj)
                {
                    case DrawName:
                        break;

                    case DrawRectangle dr:
                        if (dr.Location.X < left)
                            left = dr.Location.X;
                        if (dr.Location.Y < top)
                            top = dr.Location.Y;
                        break;

                    case DrawBaseStream dbs:
                        if (dbs.Rectangle.X < left)
                            left = dbs.Rectangle.X;
                        if (dbs.Rectangle.Y < top)
                            top = dbs.Rectangle.Y;
                        break;
                }
            }

            int dx = left - 20, dy = top - 20;

            foreach (DrawObject dobj in drawArea.GraphicsList)
            {
                switch (dobj)
                {
                    case DrawRectangle dr:
                        Rectangle loc = new Rectangle(new Point(dr.Location.X - dx, dr.Location.Y - dy), dr.Location.Size);
                        dr.Location = loc;
                        break;

                    case DrawBaseStream dbs:
                        dbs.Move(-dx, -dy);
                        break;
                }
            }

            drawArea.Invalidate();
            drawArea.Refresh();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GraphicsList copy = new GraphicsList();
            List<DrawObject> drawObjects = drawArea.GraphicsList.GetSelected;
            foreach (DrawObject d in drawObjects)
                copy.Add(d);

            p1 = MousePosition;

            Clipboard.SetData("graphics", copy);
        }

        private Point p1 = new Point(0, 0);

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GraphicsList gl = null;
            Point p2 = MousePosition;

            if (Clipboard.ContainsData("graphics"))
                gl = (GraphicsList)Clipboard.GetData("graphics");

            gl.UpdateGuidConnections();
            gl.RelocateTo((p2.X - p1.X), (p2.Y - p1.Y));

            if (gl != null)
                drawArea.GraphicsList.Add(gl);
        }

        private void MainFormContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void cleanAndSolveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cleanModelToolStripMenuItem_Click(sender, e);
            solveToolStripMenuItem1_Click(sender, e);
        }

        private void findGuidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindGuid fg = new(drawArea);
            fg.Show();
        }

        private void setToZeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.ScaleDraw = 0.564f;

            double XLoc = 1138 / (drawArea.ScaleDraw);
            double YLoc = 356 / (drawArea.ScaleDraw);

            drawArea.OffsetX = 0;
            drawArea.OffsetY = 0;

            drawArea.Refresh();
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.ScaleDraw = 0.564f * 1.1f;
            //drawArea.Offsetx1 = 0;
            //drawArea.Offsety1 = 0;
            drawArea.OffsetX = 0;
            drawArea.OffsetY = 0;
            drawArea.Refresh();
        }

        private void relocateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double x = 1138 / 0.564 - 1138 / (0.564 * 1.1);
            double y = 356 / 0.564 - 356 / (0.564 * 1.1);
            drawArea.OffsetX = -(int)x;
            drawArea.OffsetY = -(int)y;
            drawArea.Refresh();
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.ScaleDraw = 0.564f;

            double XLoc = 1138 / (drawArea.ScaleDraw);
            double YLoc = 356 / (drawArea.ScaleDraw);

            drawArea.OffsetX = 0;
            drawArea.OffsetY = 0;

            drawArea.ScaleDraw = 0.564f * 1.1f;

            double x = 1138 / 0.564 - 1138 / (0.564 * 1.1);
            double y = 356 / 0.564 - 356 / (0.564 * 1.1);

            drawArea.Refresh();
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void toolStripMenuItem8_Click_1(object sender, EventArgs e)
        {
        }

        private void resetStreamLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DrawObject> stream = drawArea.GraphicsList.GetSelected;

            foreach (DrawObject drawObject in stream)
            {
                if (drawObject is DrawBaseStream s)
                {
                    s.ResetOthoganals();
                }
            }
        }

        private void resetAllStreamLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DrawBaseStream> stream = drawArea.GraphicsList.ReturnStreams();

            foreach (DrawObject drawObject in stream)
            {
                if (drawObject is DrawBaseStream s)
                {
                    s.ResetOthoganals();
                }
            }
        }
    }
}