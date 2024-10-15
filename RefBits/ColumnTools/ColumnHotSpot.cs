using System.Drawing;

namespace Units
{
    internal class ColumnHotSpot : Node
    {
        //PointFp=new PointF();

        private bool isconnected = false;

        private HotSpotType type;
        private DrawObject owner;

        public new DrawObject Owner
        {
            get { return owner; }

            set { owner = value; }
        }

        private new bool IsConnected
        {
            get { return isconnected; }
            set { isconnected = value; }
        }

        public ColumnHotSpot()
        {
            Color = Color.DarkBlue;
        }

        public ColumnHotSpot(float X, float Y, string Name)
        {
            this.Name = Name;
            RotatedFlipped = new PointF(X, Y);
            Color = Color.DarkBlue;
        }

        public ColumnHotSpot(object d, float X, float Y, string Name, NodeDirections LineDirection, HotSpotType NodeType)
        {
            RotatedFlipped = new PointF(X, Y);
            owner = (DrawObject)d;
            switch (NodeType)
            {
                case HotSpotType.Feed:
                    {
                        this.Color = Color.GreenYellow;
                        this.IsInput = true;
                        break;
                    }
                case HotSpotType.LiquidDraw:
                    {
                        Color = Color.Red;
                        IsInput = false;
                        break;
                    }
                case HotSpotType.Stream:
                    {
                        Color = Color.Blue;
                        this.IsInput = false;
                        break;
                    }
                case HotSpotType.Water:
                    {
                        Color = Color.Blue;
                        this.IsInput = false;
                        break;
                    }
            }
            this.Name = Name;
            this.LineDirection = LineDirection;
            this.type = NodeType;
        }
    }
}