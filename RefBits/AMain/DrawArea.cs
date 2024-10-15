using DocToolkit;
using ModelEngine;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Units.CalculationSeq;
using Units.PortForm;
using WeifenLuo.WinFormsUI.Docking;

namespace Units
{
    [Serializable]
    public partial class DrawArea : DockContent, IConnectable, ISerializable
    {
        private int id = 0;
        private rbColumnToolBox columnToolbox;
        private Guid guid;
        private FlowSheet flowsheet;
        private int noComponents = 0;
        private GraphicsList graphicsList = new();    // list of draw objects
        private List<TableControl> tablecontrols = new();
        private NodeCollection nodes = new NodeCollection();

        public DrawArea()
        {
            InitializeComponent();
            thread.DoWork += DoWork;
            thread.RunWorkerCompleted += RunWorkerCompleted;
            flowsheet = GlobalModel.Flowsheet;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.WorkerSupportsCancellation = true;
            graphicsList.graphicsValueChangedEvent += graphicsValueChanged;
            this.MouseWheel += MouseScroll;
        }

        public void graphicsValueChanged(object sender, EventArgs e)
        {
            if (sender is DrawRectangle dr)
                if (dr.AttachedModel is UnitOperation uo)
                    uo.EraseCalcValues(SourceEnum.UnitOpCalcResult);

            Refresh();
            SetUpModelStack(GlobalModel.Flowsheet);
            SolveAsync();
        }

        public DrawArea(string Name)
        {
            InitializeComponent();
            thread.DoWork += DoWork;
            thread.RunWorkerCompleted += RunWorkerCompleted;
            flowsheet = GlobalModel.Flowsheet;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.WorkerSupportsCancellation = true;
            graphicsList.graphicsValueChangedEvent += graphicsValueChanged;
            this.Name = Name;
            this.MouseWheel += MouseScroll;
        }

        private void DrawArea_Load(object sender, EventArgs e)
        {
            BringToFront();
        }

        public static void UpdateForms(ProgressChangedEventArgs e)
        {
            FormCollection of = Application.OpenForms;
            for (int i = 0; i < of.Count; i++)
            {
                switch (of[i])
                {
                    case ValveDialog vdg:
                        vdg.UpdateValues();
                        break;

                    case BaseDialog bdg:
                        bdg.UpdateValues();
                        break;

                    case PortPropertyForm2 PF2:
                        PF2.UpdateValues();
                        break;

                    case ColumnDLG cdg:
                        if (e != null && e.UserState == cdg.Column)
                        {
                            cdg.UpdateProgress();
                            cdg.UpdateSpecs();
                            cdg.Update();
                        }
                        break;

                    default:

                        break;
                }
            }
        }

        // This method demonstrates a pattern for making thread-safe
        // calls on a Windows Forms control.
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextCallback and calls itself asynchronously using   the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly.
        public delegate void SetTextCallback(string text);

        public void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it return  s true.

            if (this.mainForm.messages.rtMessageBox.InvokeRequired)
            {
                SetTextCallback d = new(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
                this.mainForm.messages.rtMessageBox.Text = text;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        enum Threaded { BackGround, Multi, Single }

        public async Task SolveAsync()
        {
            Threaded threads = Threaded.Multi;

            switch (threads)
            {
                case Threaded.Single:
                    GlobalModel.Flowsheet.PreSolve();
                    Refresh();
                    UpdateForms(null);
                    break;
                case Threaded.BackGround:
                    if (!backgroundWorker.IsBusy)
                    {
                        SetUpModelStack(GlobalModel.Flowsheet);
                        GlobalModel.IsRunning = true;
                        backgroundWorker.RunWorkerAsync(flowsheet);
                    }
                    else
                    {
                        backgroundWorker.CancelAsync();
                        //     while (backgroundWorker.IsBusy)
                        //     Application.DoEvents();
                    }
                    break;
                case Threaded.Multi:
                    GlobalModel.IsRunning = true;
                    var tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;

                    Task a2 = RefreshGraphics.Run(token);
                    Task a1 = RunModel.Run();

                    await a1;

                    tokenSource.Cancel();

                    if (a2.Status.Equals(TaskStatus.Running))
                    {
                        MessageBox.Show("A2 still running");
                    }

                    Refresh();
                    UpdateForms(null);
                    GlobalModel.IsRunning=false;

                    break;
            }
        }

        public class RunModel
        {
            public static async Task Run()
            {
                await Task.Run(() =>
                {
                    GlobalModel.Flowsheet.PreSolve();
                    //DrawArea.UpdateForms(new ProgressChangedEventArgs(1, null));
                });
            }
        }

        public class RefreshGraphics
        {
            public static async Task Run(CancellationToken ct)
            {
                for (int i = 0; i < 600000; i++)
                {
                    await Task.Delay(1000);
                    await Task.Run(() =>
                    {
                        UpdateForms(null);
                    });

                    if (ct.IsCancellationRequested)
                    {
                        Debug.WriteLine("Task {0} cancelled", Task.CurrentId);
                        ct.ThrowIfCancellationRequested();
                    }
                }
            }
        }

        public void SolveSingleThread()
        {
            GlobalModel.Flowsheet.PreSolve();
        }

        public void SetUpModelStack(FlowSheet Flowsheet)
        {
            Flowsheet.ModelStack.Clear();
            Flowsheet.solveStack.Clear();
            foreach (DrawObject item in GraphicsList) // Add int erfaceObject to flowsheet
            {
                UnitOperation uo = item.AttachedModel;
                if (uo != null)
                {
                    uo.SetPropertyPort();    // re-attach port to each property
                    uo.ClearAttachedPorts(); // clear all connected ports
                }
                switch (item)
                {
                    case DrawBaseStream dbs:
                        Flowsheet.Add(uo);
                        break;

                    case null:
                        break;

                    case DrawRecycle:
                        Flowsheet.Add(uo);
                        break;

                    case DrawLLEColumn llecolumn:
                        Flowsheet.Add(uo);
                        llecolumn.SubFlowSheet.Clear();
                        llecolumn.SubFlowSheet.Add(llecolumn.Column);
                        foreach (DrawObject dobj in llecolumn.graphicslist)
                            if (dobj.AttachedModel != null)
                                llecolumn.SubFlowSheet.Add(dobj.AttachedModel);
                        break;

                    case DrawColumn dcolumn:
                        Flowsheet.Add(uo);
                        dcolumn.SubFlowSheet.Clear();
                        dcolumn.SubFlowSheet.Add(dcolumn.Column);
                        foreach (DrawObject dobj in dcolumn.graphicslist)
                            if (dobj.AttachedModel != null)
                                dcolumn.SubFlowSheet.Add(dobj.AttachedModel);
                        break;

                    case DrawName _:
                        break;

                    default:
                        Flowsheet.Add(uo);
                        break;
                }
            }

            graphicsList.UpdateAllConnections();  // should be after on deserialize method because of dictionaries

            foreach (DrawObject item in graphicsList)
            {
                switch (item)
                {
                    case DrawSet ds:
                        ds.set.Name = ds.Name;
                        ds.PortIn.Owner = ds.set;
                        ds.PortOut.Owner = ds.set;
                        break;

                    case DrawAdjust ds:
                        ds.adjust.Name = ds.Name;
                        ds.PortIn.Owner = ds.adjust;
                        ds.PortOut.Owner = ds.adjust;
                        break;

                    case DrawSignalStream ds:
                        ds.Stream.Name = ds.Name;
                        ds.Port.Owner = ds.Stream;
                        break;

                    case DrawEnergyStream ds:
                        ds.Stream.Name = ds.Name;
                        ds.Port.Owner = ds.Stream;
                        break;

                    case DrawMaterialStream dms:
                        dms.Stream.Name = dms.Name;
                        dms.Port.Owner = dms.Stream;
                        break;

                    case DrawPump ds:
                        ds.Pump.Name = ds.Name;
                        ds.PortIn.Owner = ds.Pump;
                        ds.PortOut.Owner = ds.Pump;
                        break;

                    case DrawHeater ds:
                        ds.Heater.Name = ds.Name;
                        ds.PortIn.Owner = ds.Heater;
                        ds.PortOut.Owner = ds.Heater;
                        break;

                    case DrawCooler ds:
                        ds.Cooler.Name = ds.Name;
                        ds.PortIn.Owner = ds.Cooler;
                        ds.PortOut.Owner = ds.Cooler;
                        break;

                    case DrawCompressor ds:
                        ds.compressor.Name = ds.Name;
                        ds.PortIn.Owner = ds.compressor;
                        ds.PortOut.Owner = ds.compressor;
                        break;

                    case DrawExpander ds:
                        ds.expander.Name = ds.Name;
                        ds.PortIn.Owner = ds.expander;
                        ds.PortOut.Owner = ds.expander;
                        break;

                    case DrawValve ds:
                        ds.valve.Name = ds.Name;
                        ds.PortIn.Owner = ds.valve;
                        ds.PortOut.Owner = ds.valve;
                        break;

                    case DrawRecycle ds:
                        ds.recycle.Name = ds.Name;
                        ds.PortIn.Owner = ds.recycle;
                        ds.PortOut.Owner = ds.recycle;
                        break;

                    case DrawCompSplitter dcs:
                        dcs.compsplitter.Name = dcs.Name;
                        dcs.PortIn.Owner = dcs.compsplitter;
                        dcs.PortListOut.SetOwner(dcs.compsplitter);
                        break;

                    case DrawExchanger ds:
                        ds.Exchanger.Name = ds.Name;
                        if (ds.tsin is not null)
                            ds.tsin.Owner = ds.Exchanger;
                        if (ds.tsout is not null)
                            ds.tsout.Owner = ds.Exchanger;
                        if (ds.ssin is not null)
                            ds.ssin.Owner = ds.Exchanger;
                        if (ds.ssout is not null)
                            ds.ssout.Owner = ds.Exchanger;
                        break;

                    case DrawColumn dc:
                        foreach (var dcitem in dc.graphicslist)
                            switch (dcitem)
                            {
                                case DrawMaterialStream ds:
                                    ds.Stream.Name = ds.Name;
                                    ds.Port.Owner = ds.Stream;
                                    break;
                            }
                        break;

                    default:
                        //item.AttachedModel.Name = item.Name;
                        break;
                }
            }

            foreach (UnitOperation item in Flowsheet.ModelStack)
                foreach (var p in item.Ports)
                    p.Owner = item;
        }

        public void SolveSingleStep()
        {
            graphicsList.UpdateAllConnections();  // should be after on deserialize method because of dictionaries
            GlobalModel.Flowsheet.PreSolve(true);
        }

        internal Point LastMousePosition;
        internal DrawObject lastobject;

        public void UpdateThermoMethods()
        {
            if (GraphicsList != null)
                foreach (DrawObject dob in GraphicsList.ReturnStreams())
                    ((DrawMaterialStream)dob).Components.Thermo = this.graphicsList.Thermo;
        }

        public static Components ComponentList
        {
            get
            {
                if (GlobalModel.Flowsheet != null && GlobalModel.Flowsheet.FlowsheetComponentList != null)
                    return GlobalModel.Flowsheet.FlowsheetComponentList;
                else
                    return null;
            }
        }

        #region Constructor

        private void DrawObject_ValueChangedEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion Constructor

        #region Enumerations

        public enum DrawToolType
        {
            Pointer,
            Rectangle,
            Ellipse,
            Line,
            Stream,
            Polygon,
            Column,
            Pump,
            Separator,
            Valve,
            Mixer,
            Divider,
            Heater,
            PumpAround,
            AssayFeed,
            AssayCutter,
            Reformer,
            Isom,
            FCC,
            Coker,
            Visbreaker,
            Blender,
            Compressor,
            Expander,
            Trays,
            Condenser,
            Reboiler,
            AssayInput,
            Recycle,
            Exchanger,
            IOExchanger,
            Stripper,
            BBAssayInput,
            ThreePhaseSep,
            Name,
            AssayImportCSV,
            GenericAssay,
            Cooler,
            CompSplitter,
            GibbsRX,
            Summary,
            Pipe,
            Spreadsheet,
            CaseStudy,
            PlugFlowRx,
            LLEColumn,
            SetValue,
            AdjustValue,
            NumberOfDrawTools
        }

        #endregion Enumerations

        #region Members

        // (instances of DrawObject-derived class es)

        private static List<DrawBaseStream> staticStreamList = new();

        private DrawToolType activeTool;      // active drawing tool
        private Tool[] tools;                 // array of tools

        // Information about owner form
        private MainForm mainForm;

        private DocManager docManager;

        //private  ContextMenuStrip m_ContextMenu;

        private UndoManager undoManager;

        #endregion Members

        #region Properties

        /// <summary>
        /// Reference to the owner form
        /// </summary>
        public new MainForm Owner
        {
            get
            {
                return mainForm;
            }
            set
            {
                mainForm = value;
            }
        }

        /// <summary>
        /// Reference to DocManager
        /// </summary>
        public DocManager DocManager
        {
            get
            {
                return docManager;
            }
            set
            {
                docManager = value;
            }
        }

        /// <summary>
        /// Active drawing tool.
        /// </summary>
        public DrawToolType ActiveTool
        {
            get
            {
                return activeTool;
            }
            set
            {
                activeTool = value;
            }
        }

        /// <summary>
        /// List of graphics objects.
        /// </summary>
        public GraphicsList GraphicsList
        {
            get
            {
                return graphicsList;
            }
            set
            {
                graphicsList = value;
            }
        }

        public List<DrawObject> Graphics
        {
            get
            {
                return graphicsList.Graphics;
            }
        }

        /// <summary>
        /// return   True if Undo operation is possible
        /// </summary>
        public bool CanUndo
        {
            get
            {
                if (undoManager != null)
                    return undoManager.CanUndo;
                return false;
            }
        }

        internal void ShowColumnToolBox()
        {
            if (columnToolbox == null)
            {
                columnToolbox = new(this);
                columnToolbox.Show();
                if (columnToolbox.Left > 300)
                    columnToolbox.Left = 200;
                if (columnToolbox.Top > 300)
                    columnToolbox.Top = 200;
            }
            else
            {
                columnToolbox.Hide();
                columnToolbox.Dispose();
                columnToolbox = null;
            }
        }

        internal void HideColumnToolBox()
        {
            if (columnToolbox != null)
                columnToolbox.Hide();
        }

        /// <summary>
        /// return   True if Redo operation is possible
        /// </summary>
        public bool CanRedo
        {
            get
            {
                if (undoManager != null)
                {
                    return undoManager.CanRedo;
                }
                return false;
            }
        }

        public Guid Guid
        {
            get
            {
                return guid;
            }

            set
            {
                guid = value;
            }
        }

        #endregion Properties

        #region Other Functions

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="docManager"></param>
        public void Initialize(object owner)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer, true);

            // Keep reference to owner form
            switch (owner)
            {
                case MainForm mf:
                    mainForm = mf;
                    break;

                case ColumnDesignerControl cdc:
                    columnDesigncontrol = cdc;
                    break;

                case LLESEP llsep:
                    //this.graphicsList=llsep
                    break;
            }

            this.DocManager = docManager;

            // set default tool
            activeTool = DrawToolType.Pointer;

            // create list of graphic objects
            if (graphicsList == null)
                graphicsList = new();

            graphicsList.errorChangedEvent += graphicsList_errorChangedEvent;

            graphicsList.OnRequestParentObject += new Func<object>(delegate { return this; });

            // Create undo manager
            undoManager = new UndoManager(graphicsList);

            // create array of drawing tools
            tools = new Tool[(int)DrawToolType.NumberOfDrawTools];
            tools[(int)DrawToolType.Pointer] = new ToolPointer();
            tools[(int)DrawToolType.Rectangle] = new ToolRectangle();
            tools[(int)DrawToolType.Ellipse] = new ToolEllipse();
            tools[(int)DrawToolType.Line] = new ToolLine();
            tools[(int)DrawToolType.Stream] = new ToolStream();
            tools[(int)DrawToolType.Polygon] = new ToolPolygon();
            tools[(int)DrawToolType.Column] = new ToolColumn();
            tools[(int)DrawToolType.Pump] = new ToolPump();
            tools[(int)DrawToolType.Separator] = new ToolSeparator();
            tools[(int)DrawToolType.Valve] = new ToolValve();
            tools[(int)DrawToolType.Mixer] = new ToolMixer();
            tools[(int)DrawToolType.Divider] = new ToolDivider();
            tools[(int)DrawToolType.Heater] = new ToolHeater();
            tools[(int)DrawToolType.PumpAround] = new ToolPA();
            //  tools[(int )DrawToolType.AssayFeed] = new  ToolAssayFeed();
            tools[(int)DrawToolType.AssayCutter] = new ToolAssayCutter();
            tools[(int)DrawToolType.Reformer] = new ToolReformer();
            tools[(int)DrawToolType.Isom] = new ToolIsom();
            tools[(int)DrawToolType.FCC] = new ToolFCC();
            tools[(int)DrawToolType.Coker] = new ToolCoker();
            tools[(int)DrawToolType.Visbreaker] = new ToolVisbreaker();
            tools[(int)DrawToolType.Blender] = new ToolBlender();
            tools[(int)DrawToolType.Compressor] = new ToolCompressor();
            tools[(int)DrawToolType.Expander] = new ToolExpander();
            tools[(int)DrawToolType.Trays] = new ColumnToolTrays();
            tools[(int)DrawToolType.Condenser] = new ColumnToolCondenser();
            tools[(int)DrawToolType.Reboiler] = new ColumnToolReboiler();
            tools[(int)DrawToolType.Recycle] = new ToolRecycle();
            tools[(int)DrawToolType.Exchanger] = new ToolExchanger();
            tools[(int)DrawToolType.IOExchanger] = new ToolIOExchanger();
            tools[(int)DrawToolType.Stripper] = new ColumnToolStripper();
            tools[(int)DrawToolType.ThreePhaseSep] = new Tool3PhaseSeparator();
            tools[(int)DrawToolType.Name] = new ToolName();
            // tools[(int )DrawToolType.AssayImportCSV] = new  ToolAssayImportCSV();
            tools[(int)DrawToolType.GenericAssay] = new ToolGenericAssay();
            tools[(int)DrawToolType.Cooler] = new ToolCooler();
            tools[(int)DrawToolType.CompSplitter] = new ToolCompSplitter();
            tools[(int)DrawToolType.GibbsRX] = new ToolGibbs();
            tools[(int)DrawToolType.Summary] = new ToolTable();
            tools[(int)DrawToolType.Pipe] = new ToolPipe();
            tools[(int)DrawToolType.Spreadsheet] = new ToolSpreadsheet();
            tools[(int)DrawToolType.CaseStudy] = new ToolCaseStudy();
            tools[(int)DrawToolType.PlugFlowRx] = new ToolPlugFlowRx();
            tools[(int)DrawToolType.LLEColumn] = new ToolLLEColumn();
            tools[(int)DrawToolType.SetValue] = new ToolSet();
            tools[(int)DrawToolType.AdjustValue] = new ToolAdjust();

            int top, left;

            left = this.graphicsList.GetLeftMost();
            top = this.graphicsList.GetTopMost();

            if (left < 0 || top < 0)
                this.graphicsList.Relocate(-left, -top + 50);
        }

        public EnumCalcSeq CalcType
        {
            get
            {
                if (graphicsList != null)
                    return graphicsList.CalcType;
                else return EnumCalcSeq.BackProp;
            }
            set
            {
                if (graphicsList != null)
                    graphicsList.CalcType = value;
            }
        }

        public EnumCalcActive CalcActive
        {
            get
            {
                if (graphicsList != null)
                    return graphicsList.CalcActive;
                else
                    return EnumCalcActive.No;
            }
            set
            {
                if (graphicsList != null)
                    graphicsList.CalcActive = value;
            }
        }

        public FlowSheet Flowsheet { get => flowsheet; set => flowsheet = value; }
        public List<TableControl> Tablecontrols { get => tablecontrols; set => tablecontrols = value; }

        [Browsable(true), Category("Appearance")]
        public int OffsetX { get => graphicsList.Offsetx; set => graphicsList.Offsetx = value; }

        [Browsable(true), Category("Appearance")]
        public int OffsetY { get => graphicsList.Offsety; set => graphicsList.Offsety = value; }

        [Browsable(true), Category("Appearance")]
        public float ScaleDraw { get => graphicsList.Scale; set => graphicsList.Scale = value; }

        public int NoComponents { get => noComponents; set => noComponents = value; }
        public rbColumnToolBox ColumnToolbox { get => columnToolbox; set => columnToolbox = value; }

        public void graphicsList_errorChangedEvent(object sender, EventArgs e)
        {
            CalcActive = EnumCalcActive.No;
        }

        /// <summary>
        /// Add command to history.
        /// </summary>
        public void AddCommandToHistory(Command command)
        {
            undoManager.AddCommandToHistory(command);
        }

        /// <summary>
        /// Clear Undo history.
        /// </summary>
        public void ClearHistory()
        {
            if (undoManager != null)
                undoManager.ClearHistory();
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            undoManager.Undo();
            Refresh();
        }

        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            undoManager.Redo();
            Refresh();
        }

        private bool propertygridactive = true;

        public bool ActivatePropertyGrid
        {
            get
            {
                return propertygridactive;
            }
            set
            {
                propertygridactive = value;
                if (mainForm != null && mainForm.propertiesgrid != null)
                    mainForm.propertiesgrid.propertyGrid.Visible = value;
                // this.Controls.Remove(splitContainer1);
            }
        }

        private bool messageBoxActive = true;

        public bool MessageBoxActive
        {
            get
            {
                return messageBoxActive;
            }
            set
            {
                messageBoxActive = value;
                if (mainForm != null)
                    mainForm.messages.rtMessageBox.Visible = value;
            }
        }

        public UOMDisplayList UOMDisplayList { get; set; } = new UOMDisplayList();
        public static List<DrawBaseStream> StaticStreamList { get => staticStreamList; set => staticStreamList = value; }
        public ColumnDesignerControl ColumnDesigncontrol { get => columnDesigncontrol; set => columnDesigncontrol = value; }

        /// <summary>
        /// Set dirty flag (file is changed after last save operation)
        /// </summary>
        public static void SetDirty()
        {
            MainForm.DocManager.Dirty = true;
        }

        /// <summary>
        /// Right-click handler
        /// </summary>
        /// <param name="e"></param>
        private void OnContextMenu(DrawMouseEventArgs e)
        {
            // Change current selection if necessary
            Point point = e.ModXY;
            int n = GraphicsList.Count;
            IDrawObject o = null;

            for (int i = 0; i < n; i++)
            {
                switch (GraphicsList[i].HitTest(point))
                {
                    case HitType.Object:
                        o = GraphicsList[i];
                        break;

                    case HitType.Stream:

                        break;
                }
            }

            if (o != null)
            {
                if (!o.Selected)
                    GraphicsList.UnselectAll();

                // Select clicked object
                o.Selected = true;
            }
            else
            {
                GraphicsList.UnselectAll();
            }

            Refresh();      // in the case selection was changed

            // Show context menu.
            // Context menu items are filled from owner form Edit menu items.
            //m_ContextMenu = new  ContextMenuStrip();

            int nItems = 0;
            /*  if (mainForm != null)
                  nItems = mainForm.ContextParent.DropDownItems.Count;*/

            // Read Edit items and move them to context menu.
            // Since every move reduces number of items, read them in reverse order.
            // To get items in direct order, insert each of them to beginning.
            /*  for (int  i = nItems - 1; i >= 0; i--)
              {
                  m_ContextMenu.Items.Insert(0, mainForm.ContextParent.DropDownItems[i]);
              }*/

            // Show context menu for owner form, so that it handles items selection.
            // Convert point  from this window coordinates to owner's coordinates.
            point.X += this.Left;
            point.Y += this.Top;

            //if (mainForm != null)
            //    m_ContextMenu.Show(mainForm, point );

            if (mainForm != null)
                Owner.SetStateOfControls();  // enable/disable menu items

            // Context menu is shown, but owner's Edit menu is now empty.
            // Subscribe to context menu Closed event and restore items there.
            //m_ContextMenu.Closed += delegate (object sender, ToolStripDropDownClosedEventArgs args)
            {
                // if (m_ContextMenu != null)      // precaution
                {
                    //   nItems = m_ContextMenu.Items.Count;

                    for (int k = nItems - 1; k >= 0; k--)
                    {
                        //  mainForm.ContextParent.DropDownItems.Insert(0, m_ContextMenu.Items[k]);
                    }
                }
            };
        }

        public void DrawArea_DeletePoint()
        {
            IDrawObject d = this.lastobject;

            switch (d)
            {
                case DrawMaterialStream dms:
                    int seg = dms.GetSegment(LastMousePosition);
                    dms.DeletePoint(seg);
                    this.Refresh();
                    break;
            }
        }

        public void DrawArea_InsertPoint()
        {
            IDrawObject d = this.lastobject;

            switch (d)
            {
                case DrawMaterialStream dms:
                    int seg = dms.GetSegment(LastMousePosition);
                    dms.InsertPoint(seg);
                    this.Refresh();
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <return  s></return  s>
        public IDrawObject HitTest(Point point)
        {
            for (int i = 0; i < this.graphicsList.Count; i++)
                if (this.graphicsList[i].HitTest(point) >= 0)  // not a handle
                    return this.graphicsList[i];

            return null;
        }


        public DrawObject returnObject(Point p, bool includetrays = false)
        {
            foreach (DrawObject o in graphicsList)  // only used if selected
            {
                HitType hittype = o.HitTest(p);
                if (hittype != HitType.None)
                    switch (o)
                    {
                        case DrawColumnTraySection dtc:
                            foreach (DrawTray item in dtc)
                            {
                                var res = item.HitTest(p);
                                if (res != HitType.None)
                                    if (includetrays)
                                        return item;
                                    else
                                        return item.Owner;
                            }
                            return o;

                        case DrawMaterialStream dms:
                            return dms;

                        default:
                            return o;
                    }
            }
            return null;
        }


        /// <summary>
        /// return   an Object which is not a Stream
        /// </summary>
        /// <param name="p"></param>
        /// <return  s></return  s>
        public DrawObject returnObject(Point p) // return   stream first
        {
            List<DrawBaseStream> streams = graphicsList.ReturnStreams();

            foreach (DrawBaseStream stream in streams)
            {
                HitType hittype = stream.HitTest(p);

                switch (hittype)
                {
                    case HitType.None:
                        break;

                    default:
                        return stream;
                }
            }

            foreach (DrawObject o in this.graphicsList)  // only used if selected
            {
                HitType hittype = o.HitTest(p);

                switch (hittype)
                {
                    case HitType.None:
                        break;

                    default:
                        return o;
                }
            }
            return null;
        }

        /// <summary>
        /// return   an Object which is not a Stream
        /// </summary>
        /// <param name="p"></param>
        /// <return  s></return  s>
        public DrawObject returnObject(Point p, DrawObject d = null) // return   stream first
        {
            List<DrawBaseStream> streams = graphicsList.ReturnStreams();

            /* foreach (DrawBaseStream stream in streams)
             {
                 if (stream == d)
                     return null;

                 HitType hittype = stream.HitTest(p);

                 switch (hittype)
                 {
                     case HitType.None:
                         break;

                     default:
                         return stream;
                 }
             }*/

            foreach (DrawObject o in this.graphicsList)  // only used if selected
            {
                if (o != d)
                {
                    HitType hittype = o.HitTest(p);

                    switch (hittype)
                    {
                        case HitType.None:
                            break;

                        default:
                            return o;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// return   an Object which is not a Stream
        /// </summary>
        /// <param name="p"></param>
        /// <return  s></return  s>
        public DrawObject returnNonStreamObject(Point p)
        {
            foreach (DrawObject o in this.graphicsList)  // only used if selected
            {
                switch (o)
                {
                    case DrawBaseStream:
                        break;

                    default:
                        HitType hittype = o.HitTest(p);

                        switch (hittype)
                        {
                            case HitType.None:
                                break;

                            default:
                                return o;
                        }
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// Only return   a stream object
        /// </summary>
        /// <param name="point "></param>
        /// <return  s></return  s>
        public DrawObject returnStreamObject(Point point)
        {
            foreach (DrawObject o in this.graphicsList)
            {
                switch (o)
                {
                    case DrawEnergyStream:
                    case DrawMaterialStream:
                    case DrawSignalStream:
                        HitType hittype = o.HitTest(point);

                        switch (hittype)
                        {
                            case HitType.None:
                                break;

                            default:
                                return o;
                        }
                        break;
                }
            }
            return null;
        }

        public void ResetDrawArea()
        {
            foreach (DrawObject d in graphicsList)
                d.DrawArea = this;
        }

        public bool DeleteSelection()
        {
            bool result = false;
            Node hs;
            DrawObject dobj;

            List<DrawObject> RemoveList = new();

            List<DrawObject> AttachedObjects = new();
            List<Port> AttachedPorts = new();

            int n = graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                DrawObject d = graphicsList[i];

                if (d.Selected)
                {
                    RemoveList.Add(d);

                    AttachedObjects.AddRange(graphicsList.ReturnConnectedObjects(d));
                    AttachedPorts.AddRange(GraphicsList.ReturnConnectedPorts(d));

                    switch (d)
                    {
                        case DrawMaterialStream s:
                            graphicsList.Remove(s.drawName);

                            dobj = this.graphicsList.GetObject(s.EndDrawObjectGuid);

                            switch (dobj)
                            {
                                case DrawColumnTraySection o:
                                    DrawTray tray = o.GetTray(s.EndDrawTrayID);
                                    if (tray != null)
                                    {
                                        hs = tray.GetNode(s.EndNodeGuid);
                                        if (hs != null)
                                            hs.IsConnected = false;
                                    }
                                    break;

                                case null:
                                    break;

                                default:
                                    hs = dobj.GetNode(s.EndNodeGuid);
                                    if (hs != null)
                                        hs.IsConnected = false;
                                    break;
                            }

                            dobj = this.graphicsList.GetObject(s.StartDrawObjectGuid);

                            switch (dobj)
                            {
                                case DrawColumnTraySection o:
                                    DrawTray tray = o.GetTray(s.StartDrawTrayGuid);
                                    if (tray != null)
                                    {
                                        hs = tray.GetNode(s.StartNodeGuid);
                                        if (hs != null)
                                            hs.IsConnected = false;
                                    }
                                    break;

                                case null:
                                    break;

                                default:
                                    hs = dobj.GetNode(s.StartNodeGuid);
                                    if (hs != null)
                                        hs.IsConnected = false;
                                    break;
                            }
                            break;

                        case DrawEnergyStream de:
                            graphicsList.Remove(de.DrawName);

                            dobj = this.graphicsList.GetObject(de.EndDrawObjectGuid);

                            switch (dobj)
                            {
                                case null:
                                    break;

                                default:
                                    hs = dobj.GetNode(de.EndNodeGuid);
                                    if (hs != null)
                                        hs.IsConnected = false;
                                    break;
                            }

                            dobj = this.graphicsList.GetObject(de.StartDrawObjectGuid);

                            switch (dobj)
                            {
                                case null:
                                    break;

                                default:
                                    hs = dobj.GetNode(de.StartNodeGuid);
                                    if (hs != null)
                                        hs.IsConnected = false;
                                    break;
                            }
                            break;

                        case DrawSignalStream ds:
                            graphicsList.Remove(ds.DrawName);

                            dobj = this.graphicsList.GetObject(ds.EndDrawObjectGuid);

                            switch (dobj)
                            {
                                case null:
                                    break;

                                default:
                                    hs = dobj.GetNode(ds.EndNodeGuid);
                                    if (hs != null)
                                        hs.IsConnected = false;
                                    break;
                            }

                            dobj = this.graphicsList.GetObject(ds.StartDrawObjectGuid);

                            switch (dobj)
                            {
                                case null:
                                    break;

                                default:
                                    hs = dobj.GetNode(ds.StartNodeGuid);
                                    if (hs != null)
                                        hs.IsConnected = false;
                                    break;
                            }
                            break;
                    }

                    if (d is DrawRectangle dr)
                        RemoveList.Add(dr.drawName);

                    if (GlobalModel.Flowsheet.ModelStack.Contains(d.AttachedModel))
                        GlobalModel.Flowsheet.ModelStack.Remove(d.AttachedModel);

                    //RemoveList.Add(d);
                    //graphicsList.RemoveAt(i);
                    result = true;
                }
            }

            for (int i = 0; i < RemoveList.Count; i++)
            {
                graphicsList.Remove(RemoveList[i]);
            }

            return result;
        }

        #endregion Other Functions

        #region Event Handlers

        // Declare the delegate (if using   non-generic pattern).
        public delegate void MouseDownEventHandler(object sender, EventArgs e);

        // Declare the event.
        public event MouseDownEventHandler MouseDownEvent;

        // Wrap the event in a protected virtual method
        // to enable derived class es to raise the event.
        protected virtual void RaiseMouseDownEvent()
        {
            // Raise the event in a thread-safe manner using   the ?. operator.
            MouseDownEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Mouse down.
        /// Left button down event is passed to active tool.
        /// Right button down event is handled in this class .
        /// </summary>
        private void DrawArea_MouseDown(object sender, MouseEventArgs e)
        {
            Point ModMousePoint = new((int)(e.X / ScaleDraw) - OffsetX, (int)(e.Y / ScaleDraw) - OffsetY);
            Point MousePoint = new(e.X, e.Y);
            DrawMouseEventArgs e2 = new(e.Button, e.Clicks, e.Delta, MousePoint, ModMousePoint);

            if (e.Button == MouseButtons.Left)
            {
                tools[(int)activeTool].OnMouseDown(this, e2);  //active tool should be point er
                switch (activeTool)
                {
                    case DrawToolType.Stream:
                    case DrawToolType.Line:
                        break;

                    default:
                        activeTool = DrawToolType.Pointer;
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                //OnContextMenu(e2);
                if (columnDesigncontrol is not null)
                    columnDesigncontrol.DrawArea1_MouseDown(sender, e2);
            }

            LastMouseMovePoint = e.Location;
            LastOffset.X = OffsetX;
            LastOffset.Y = OffsetY;
        }

        /// <summary>
        /// Mouse move.
        /// Moving without button pressed or with left button pressed
        /// is passed to active tool.
        /// </summary>
        private void DrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            Point ModMousePoint = new((int)(e.X / ScaleDraw) - OffsetX, (int)(e.Y / ScaleDraw) - OffsetY);
            Point MousePoint = new(e.X, e.Y);
            DrawMouseEventArgs e2 = new(e.Button, e.Clicks, e.Delta, MousePoint, ModMousePoint);

            //Debug.Print (e.X.ToString() + " " + e.Y.ToString());

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
                tools[(int)activeTool].OnMouseMove(this, e2);
            else
                this.Cursor = Cursors.Default;
        }

        private Point LastMouseMovePoint;
        private Point LastOffset = new();

        /// <summary>
        /// Mouse up event.
        /// Left button up event is passed to active tool.
        /// </summary>
        private void DrawArea_MouseUp(object sender, MouseEventArgs e)
        {
            Point ModMousePoint = new((int)(e.X / ScaleDraw) - OffsetX, (int)(e.Y / ScaleDraw) - OffsetY);
            Point MousePoint = new(e.X, e.Y);
            DrawMouseEventArgs e2 = new(e.Button, e.Clicks, e.Delta, MousePoint, ModMousePoint);

            if (e.Button == MouseButtons.Left)
                if (ActiveTool == DrawToolType.Pointer)
                    tools[(int)activeTool].OnMouseUp(this, e2);
                else
                    tools[(int)activeTool].OnMouseUp(this, e2);
        }

        #endregion Event Handlers

        private void DrawArea_MousedoubleClick(object sender, MouseEventArgs e)
        {
            Point ModMousePoint = new((int)(e.X / ScaleDraw) - OffsetX, (int)(e.Y / ScaleDraw) - OffsetY);
            Point MousePoint = new(e.X, e.Y);
            DrawMouseEventArgs e2 = new(e.Button, e.Clicks, e.Delta, MousePoint, ModMousePoint);

            tools[(int)activeTool].OnDoubleClick(this, e2);
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            mainForm.propertiesgrid.propertyGrid.Refresh();
        }

        private static readonly BackgroundWorker thread = new();

        public bool SolverActive = true;

        private static void DoWork(object sender, DoWorkEventArgs e)
        {
            if (sender is CalcSeq)
            {
                //  cs.GO();
            }
        }

        private static void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                Console.WriteLine("You canceled!");
            else if (e.Error != null)
                Console.WriteLine("Worker exception: " + e.Error.ToString());
            else
            {
                FormCollection forms = Application.OpenForms;

                // close every open form
                foreach (Form form in forms)
                {
                    if (form.GetType() == typeof(PortPropertyForm2))
                    {
                        ((PortPropertyForm2)form).UpdateValues();
                        //((StreamDataEntry)form).Invalidate();
                        //((StreamDataEntry)form).Refresh();
                    }
                }
            }
        }

        public void Calc(EnumCalcSeq SeqOrSimul, int nosteps)
        {
            CalcSeq cs = new(this);
            cs.Steps = nosteps;
            cs.Calcseq = SeqOrSimul;

            switch (SeqOrSimul)
            {
                case EnumCalcSeq.threadedbackprop:
                    if (!thread.IsBusy)
                    {
                        thread.RunWorkerAsync(cs);
                    }
                    //runThread(cs);
                    break;

                case EnumCalcSeq.BackProp:
                    {
                        //cs.GO();
                        break;
                    }
                case EnumCalcSeq.Off:
                    {
                        return;
                    }
                default:
                    // cs.GO();
                    break;
            }

            FormCollection forms = Application.OpenForms;

            // close every open form
            foreach (Form form in forms)
            {
                if (form is PortPropertyForm2 form1)
                {
                    form1.UpdateValues();
                    //((StreamDataEntry)form).Invalidate();
                    //((StreamDataEntry)form).Refresh();
                }
                if (form is IUpdateableDialog dialog)
                {
                    dialog.UpdateValues();
                    //((StreamDataEntry)form).Invalidate();
                    //((StreamDataEntry)form).Refresh();
                }
            }
            // get array because collection changes as we close forms
            //cs.GO();

            //propertyGrid.Refresh();
            ///ThreadPool.QueueUserWorkItem(ThreadProc, cs);
        }

        /// <summary>
        /// Draw graphic objects and
        /// group selection rectangle (optionally)
        /// </summary>
        private void DrawArea_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brush = new(this.BackColor);
            e.Graphics.FillRectangle(brush, this.ClientRectangle);

            if (graphicsList != null)
            {
                Draw(e.Graphics, OffsetX, OffsetY, ScaleDraw);
            }

            brush.Dispose();
        }

        public void Draw(Graphics g, int X, int Y, float scale)
        {
            int n = graphicsList.Count;
            IDrawObject o;

            g.ScaleTransform(scale, scale);
            g.TranslateTransform(X, Y);

            // Enumerate list in reverse order to get first
            // object on the top of Z-order.
            for (int i = n - 1; i >= 0; i--)
            {
                o = graphicsList[i];
                o.Draw(g);

                if (o.Selected)
                    o.DrawTracker(g);
                if (o.Displayhotspots && !o.Selected)
                    o.DrawHotSpot(g);
            }

            if (tablecontrols != null)
            {
                foreach (var item in tablecontrols)
                {
                    item.Draw(g);
                }
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            OffsetX = -hScrollBar1.Value * 30;
            this.Refresh();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            OffsetY = -vScrollBar1.Value * 10;
            this.Refresh();
        }

        public void MouseScroll(object sender, MouseEventArgs e)
        {
            double MouseX = e.X;
            double MouseY = e.Y;

            float zoom;

            if (e.Delta > 0)
                zoom = 1.1f;
            else
                zoom = 1 / 1.1f;

            if (zoom > 3)
                zoom = 3f;
            if (zoom < 1 / 100)
                zoom = 1 / 100f;

            graphicsList.X += MouseX / (ScaleDraw * zoom) - MouseX / ScaleDraw;
            graphicsList.Y += MouseY / (ScaleDraw * zoom) - MouseY / ScaleDraw;

            OffsetX = (int)graphicsList.X;// + OffsetX;
            OffsetY = (int)graphicsList.Y;// + OffsetY;

            ScaleDraw *= zoom;

            Refresh();
        }

        private void DrawArea_MouseClick(object sender, MouseEventArgs e)
        {
            Point ModMousePoint = new((int)(e.X / ScaleDraw) - OffsetX, (int)(e.Y / ScaleDraw) - OffsetY);
            Point MousePoint = new(e.X, e.Y);
            DrawMouseEventArgs e2 = new(e.Button, e.Clicks, e.Delta, MousePoint, ModMousePoint);

            tools[(int)activeTool].OnMouseClick(this, e2);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("BackColor", BackColor);
            info.AddValue("ScaleDraw", ScaleDraw);
            info.AddValue("Offsetx", OffsetX);
            info.AddValue("Offsety", OffsetY);
        }

        public void Abort()
        {
            backgroundWorker.CancelAsync();
        }

        protected DrawArea(SerializationInfo info, StreamingContext context)
        {
            InitializeComponent();

            try
            {
                BackColor = (Color)info.GetValue("BackColor", typeof(Color));
                ScaleDraw = info.GetSingle("ScaleDraw");
                OffsetX = info.GetInt32("Offsetx");
                OffsetY = info.GetInt32("Offsety");
            }
            catch
            {
                BackColor = Color.Gray;
            }

            thread.DoWork += DoWork;
            thread.RunWorkerCompleted += RunWorkerCompleted;
            graphicsList.graphicsValueChangedEvent += graphicsValueChanged;
        }

        private void DrawArea_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void DrawArea_DragDrop(object sender, DragEventArgs e)
        {
            Point DocLoc = this.Parent.Parent.Location;

            Point ModMousePoint = new((int)(e.X / ScaleDraw) - OffsetX - DocLoc.X, (int)(e.Y / ScaleDraw) - OffsetY - DocLoc.Y);
            Point MousePoint = new(e.X - DocLoc.X, e.Y - DocLoc.Y);

            DrawMouseEventArgs e2 = new(MouseButtons.Left, 1, 0, MousePoint, ModMousePoint);

            tools[(int)activeTool].OnMouseDown(this, e2);
            tools[(int)activeTool].OnMouseUp(this, e2);
            this.ActiveTool = DrawToolType.Pointer;
        }

        private void DrawArea_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        internal bool ControlPressed = false;
        private ColumnDesignerControl columnDesigncontrol;

        private void DrawArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift)
            {
                LastOffset.X = OffsetX;
                LastOffset.Y = OffsetY;
            }
        }
    }
}