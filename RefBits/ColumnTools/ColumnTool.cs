using System.Windows.Forms;

namespace Units
{
    ///<summary>
    ///Baseclass foralldrawingtools
    ///</summary>
    internal abstract class ColumnTool : ColumnToolI
    {
        ///<summary>
        ///Leftnousbuttonispressed
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        public virtual void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
        }

        ///<summary>
        ///Mouseismoved,leftmousebuttonispressedornonebuttonispressed
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        public virtual void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
        }

        ///<summary>
        ///Leftmousebuttonisreleased
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        public virtual void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
        }

        ///<summary>
        ///Leftmousebuttonisreleased
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        public virtual void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
        }
    }

    internal interface ColumnToolI
    {
        ///<summary>
        ///Leftnousbuttonispressed
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        void OnMouseDown(DrawArea drawArea, MouseEventArgs e);

        ///<summary>
        ///Mouseismoved,leftmousebuttonispressedornonebuttonispressed
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        void OnMouseMove(DrawArea drawArea, MouseEventArgs e);

        ///<summary>
        ///Leftmousebuttonisreleased
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        void OnMouseUp(DrawArea drawArea, MouseEventArgs e);

        ///<summary>
        ///Leftmousebuttonisreleased
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        void OnDoubleClick(DrawArea drawArea, MouseEventArgs e);
    }
}