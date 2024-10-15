using Extensions;
using ModelEngine;
using System.Drawing;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace Units
{
    /// <summary>
    /// Base class  for all tools which create new  graphic object
    /// </summary>
    internal abstract class ToolObject : Units.Tool
    {
        private Cursor cursor;
        /* internal  Cursor Pencil = CursorHelper.FromByteArray(Units.Properties.Resources.Pencil);

         public  static class  CursorHelper
         {
             public  static Cursor FromByteArray(byte[] array)
             {
                 using   (MemoryStream memoryStream = new  MemoryStream(array))
                 {
                     return   new  Cursor(memoryStream);
                 }
             }
         }*/

        protected Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }

        public override void OnMouseUp(DrawArea drawArea, DrawMouseEventArgs e)
        {
            if (drawArea.GraphicsList.Count > 0)
                drawArea.AddCommandToHistory(new CommandAdd((DrawObject)drawArea.GraphicsList[0]));
            //drawArea.ActiveTool = DrawArea.DrawToolType.Point er; // leave active tool current

            drawArea.Capture = false;
            drawArea.Refresh();
        }

        protected static void AddnewObject(DrawArea drawArea, DrawObject o)
        {
            o.DrawArea = drawArea;
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            drawArea.GraphicsList.Add(o);

            drawArea.Capture = true; // true
            drawArea.Refresh();

            DrawArea.SetDirty();

            switch (o)
            {
                case DrawMaterialStream obj:
                    GlobalModel.Flowsheet.Add(obj.Stream);
                    break;

                case DrawColumn obj:
                    GlobalModel.Flowsheet.Add(obj.SubFlowSheet);
                    break;

                case DrawGenericAssay dga:
                    GlobalModel.Flowsheet.Add(dga.assay);
                    break;
            }

            if (o is DrawRectangle dr)
            {
                dr.drawName = new DrawName
                {
                    Attachedobject = dr,
                    Location = new Rectangle(dr.Rectangle.TopRight(), new Size(100, 11))
                };
                drawArea.GraphicsList.Add(dr.drawName);
            }

            if (o is DrawBaseStream db)
            {
                Point center = db.Center;
                center.Offset(db.NameOffSetX, db.NameOffSetY);
                db.drawName = new DrawName()
                {
                    Attachedstream = db,
                    Location = new Rectangle(center, new Size(100, 11))
                };
                drawArea.GraphicsList.Add(db.drawName);
            }

            //    if (drawArea.ColumnDesignForm != null)
            //    drawArea.ColumnDesignForm.AddObject(o);
            drawArea.GraphicsList.UnselectAll();
        }

        protected static void AddnewObject(DrawArea drawArea, TableControl o)
        {
            o.Drawarea = drawArea;
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            // drawArea.Tablecontrols.Add(o);

            drawArea.Capture = true; // true
            drawArea.Refresh();
            DrawArea.SetDirty();

            drawArea.Tablecontrols.Add(o);
            drawArea.Controls.Add(o);
            Helper.ControlMover.Init(o);
        }
    }
}