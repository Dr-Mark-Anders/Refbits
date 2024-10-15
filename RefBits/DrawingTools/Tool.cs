namespace Units
{
    /// <summary>
    /// Base class  for all drawing tools
    /// </summary>
    internal abstract class Tool : ToolI
    {
        /// <summary>
        /// Left nous button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }

        /// <summary>
        /// Mouse is moved, left mouse button is pressed or none button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }

        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseUp(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }

        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)

        {
        }

        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }
    }

    internal interface ToolI
    {
        /// <summary>
        /// Left nous button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e);

        /// <summary>
        /// Mouse is moved, left mouse button is pressed or none button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e);

        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        void OnMouseUp(DrawArea drawArea, DrawMouseEventArgs e);

        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e);

        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        void OnMouseClick(DrawArea drawArea, DrawMouseEventArgs e);
    }
}