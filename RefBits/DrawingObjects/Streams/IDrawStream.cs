using System;
using System.Windows.Forms;

namespace Units
{
    public interface IDrawStream
    {
        //void AddPoint(global::System.Drawing.Point point);

        void DeletePoint(int segment);

        void Draw(System.Drawing.Graphics g);

        bool EndConnected { get; }
        EndLineDirection EndDirection { get; set; }
        Guid EndDrawObjectGuid { get; set; }
        string EndName { get; set; }
        Guid EndNodeGuid { get; set; }
        int FirstSegmentSize { get; set; }

        //enumFlashAlgorithm FlashAlgorithm { get; set; }
        bool FrontConnected { get; }

        Cursor GetHandleCursor(int handleNumber);

        System.Drawing.Point GetHandleLocation(int handleNumber);

        int GetHandleNumber(global::System.Drawing.Point point);

        int GetSegment(global::System.Drawing.Point point);

        int HandleCount { get; }

        HitType HitTest(global::System.Drawing.Point point);

        void InsertPoint(int Segment);

        bool IsVertical(int segment);

        bool LineContains(global::System.Drawing.Point a, global::System.Drawing.Point b, global::System.Drawing.Point x);

        void Move(int deltaX, int deltaY);

        void MoveHandleTo(global::System.Drawing.Point point, int handleNumber);

        string Name { get; set; }
        StartLineDirection StartDirection { get; set; }
        Guid StartDrawObjectGuid { get; set; }
        string StartName { get; set; }
        Guid StartNodeGuid { get; set; }
        Node StartNode { get; set; }
        Node EndNode { get; set; }

        void CreateFlowsheetUOModel();
    }
}