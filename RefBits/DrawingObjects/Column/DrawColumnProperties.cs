using ModelEngine;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    public partial class DrawColumn : DrawRectangle, ISerializable
    {
        [Category("Calculation")]
        public bool Active
        {
            get
            {
                return column.IsActive;
            }
            set
            {
                column.IsActive = value;
                subflowsheet.IsActive = column.IsActive;
            }
        }

        [Browsable(false)]
        [Category("General")]
        public FlowSheet ColumnFlowSheet
        {
            get
            {
                return this.subflowsheet;
            }
        }

        [Browsable(false)]
        [Category("General")]
        public DrawColumnTraySection MainSection
        {
            get
            {
                DrawColumnTraySectionCollection DrawTraySections = graphicslist.ReturnTraySections();
                if (DrawTraySections.Count > 0)
                {
                    DrawColumnTraySection res = DrawTraySections[0];
                    foreach (DrawColumnTraySection item in DrawTraySections)
                    {
                        if (item.NoTrays > res.NoTrays)
                            res = item;
                    }

                    return res;
                }
                return null;
            }
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return column.IsDirty;
            }
            set
            {
                column.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return column.IsSolved;
            }
        }

        [Category("Thermo")]
        public ThermoDynamicOptions ThermoOptions
        {
            get
            {
                if (column != null)
                    return column.Thermo;
                else
                    return null;
            }
            set
            {
                if (column != null)
                    column.Thermo = value;
            }
        }

        [Browsable(false)]
        public GraphicsList ParentGraphicsList
        {
            get
            {
                return DrawArea.GraphicsList;
            }
        }

        public Color PenColor { get; private set; }
        public bool Isdirty { get; internal set; }

        [Browsable(false)]
        public GraphicsList graphicslist
        {
            get
            {
                return columndrawarea.GraphicsList;
            }
            set
            {
                columndrawarea.GraphicsList = value;
            }
        }

        [Browsable(false)]
        public Column Column { get => column; set => column = value; }

        [Browsable(false)]
        public COlSubFlowSheet SubFlowSheet { get => subflowsheet; set => subflowsheet = value; }

        [Browsable(false)]
        internal ColumnDLG Cdlg { get => cdlg; set => cdlg = value; }
    }
}