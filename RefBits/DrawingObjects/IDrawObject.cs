using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    public interface IDrawObject
    {
        UnitOperation AttachedModel { get; }
        int CalcOrder { get; set; }
        bool Displayhotspots { get; set; }
        DrawArea DrawArea { get; set; }
        FlowSheet Flowsheet { get; }
        Guid Guid { get; set; }
        int HandleCount { get; }
        Color Hotspotcolor { get; set; }
        NodeCollection Hotspots { get; set; }
        bool IsFeedObject { get; set; }
        Guid ModelGuid { get; }
        string Name { get; set; }
        int PenWidth { get; set; }
        List<Port> Ports { get; }
        Rectangle Rectangle { get; set; }
        bool Selected { get; set; }
        Color StreamColor { get; set; }

        DrawObject Clone();

        void CreateFlowsheetUOModel();

        void Draw(Graphics g);

        void DrawHotSpot(Graphics g);

        void DrawTracker(Graphics g);

        void Dump();

        Cursor GetHandleCursor(int handleNumber);

        Point GetHandleLocation(int handleNumber);

        int GetHandleNumber(Point handleNumber);

        Rectangle GetHandleRectangle(int handleNumber);

        Point GetHotSpotCenter(Guid NodeID);

        Node GetHotSpotFromHandleNumber(int handlenumber);

        Rectangle GetHotSpotRectangle(Guid NodeID);

        PointF GetHotSpotRelativeCenter(Guid NodeID);

        void GetObjectData(SerializationInfo info, StreamingContext context);

        object GetParent();

        List<StreamMaterial> GetProductStreamListFromNodes();

        Point GetRotatedHandleLocation(int handleNumber);

        int GetRotatedHandleNumber(Point handleNumber);

        Rectangle GetRotatedHandleRectangle(int handleNumber);

        List<StreamMaterial> GetStreamListFromNodes();

        HitType HitTest(Point point);

        Node HitTestHotSpot(Point point);

        bool IntersectsWith(Rectangle rectangle);

        void Move(int deltaX, int deltaY);

        void MoveHandle(int dx, int dy, int handleNumber);

        void MoveHandleTo(Point point, int handleNumber);

        void MoveRotatedHandle(int dx, int dy, int handleNumber);

        void MoveRotatedHandleTo(Point point, int handleNumber);

        void Normalize();

        void PostSolve();

        void RaiseValueChangedEvent(EventArgs e);

        void ResetCalculateFlag();

        void RotatedMove(int deltaX, int deltaY);

        Dictionary<Guid, string> StreamNames();

        string ToString();

        bool UpdateAttachedModel();
    }
}