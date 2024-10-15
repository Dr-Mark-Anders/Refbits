using System.Drawing;

namespace Units
{
    /// <summary>
    /// Ellipse graphic object
    /// </summary>
    internal class DrawEllipse : Units.DrawRectangle
    {
        public DrawEllipse() : this(0, 0, 1, 1)
        {
        }

        public DrawEllipse(int x, int y, int width, int height) : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawEllipse drawEllipse = new DrawEllipse();
            drawEllipse.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawEllipse);
            return drawEllipse;
        }

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(StreamColor, PenWidth);

            g.DrawEllipse(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));

            pen.Dispose();
        }
    }
}