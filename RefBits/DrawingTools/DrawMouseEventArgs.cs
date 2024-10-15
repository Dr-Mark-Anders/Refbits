using System;
using System.Drawing;
using System.Windows.Forms;

namespace Units
{
    //
    // Summary:
    //     Provides data for the System.Windows.Forms.Control.MouseUp, System.Windows.Forms.Control.MouseDown,
    //     and System.Windows.Forms.Control.MouseMove events.
    public class DrawMouseEventArgs : EventArgs
    {
        private MouseButtons button;
        private int clicks, delta;
        private Point modXY;
        private Point mousexy;

        //
        // Summary:
        //     Gets which mouse button was pressed.
        //
        // return  s:
        //     One of the System.Windows.Forms.MouseButtons values.
        public MouseButtons Button
        {
            get
            {
                return button;
            }
        }

        //
        // Summary:
        //     Gets the number of times the mouse button was pressed and released.
        //
        // return  s:
        //     An System.Int32 that contains the number of times the mouse button was pressed
        //     and released.
        public int Clicks
        {
            get
            {
                return clicks;
            }
        }

        //
        // Summary:
        //     Gets the x-coordinate of the mouse during the generating mouse event.
        //
        // return  s:
        //     The x-coordinate of the mouse, in pixels.
        public Point ModXY
        {
            get
            {
                return modXY;
            }
        }

        public int X
        {
            get
            {
                return modXY.X;
            }
        }

        public int Y
        {
            get
            {
                return modXY.Y;
            }
        }

        //
        // Summary:
        //     Gets a signed count of the number of detents the mouse wheel has rotated, multiplied
        //     by the WHEEL_DELTA constant. A detent is one notch of the mouse wheel.
        //
        // return  s:
        //     A signed count of the number of detents the mouse wheel has rotated, multiplied
        //     by the WHEEL_DELTA constant.
        public int Delta
        {
            get
            {
                return delta;
            }
        }

        //
        // Summary:
        //     Gets the location of the mouse during the generating mouse event.
        //
        // return  s:
        //     A System.Drawing.Point  that contains the x- and y- mouse coordinates, in pixels,
        //     relative to the upper-left corner of the control.
        public Point Location
        {
            get
            {
                return Location;
            }
        }

        public Point MouseXY { get => mousexy; set => mousexy = value; }
        public Point ModifiedMouseXY { get => modXY; set => modXY = value; }

        //
        // Summary:
        //     Initializes a new  instance of the System.Windows.Forms.MouseEventArgs class .
        //
        // Parameters:
        //   button:
        //     One of the System.Windows.Forms.MouseButtons values that indicate which mouse
        //     button was pressed.
        //
        //   clicks:
        //     The number of times a mouse button was pressed.
        //
        //   x:
        //     The x-coordinate of a mouse click, in pixels.
        //
        //   y:
        //     The y-coordinate of a mouse click, in pixels.
        //
        //   delta:
        //     A signed count of the number of detents the wheel has rotated.
        public DrawMouseEventArgs(MouseButtons button, int clicks, int delta, Point MouseXY, Point ModMouseXY)
        {
            this.button = button;
            this.clicks = clicks;
            this.modXY = ModMouseXY;
            this.delta = delta;
            this.mousexy = MouseXY;
        }
    }
}