namespace Units
{
    public enum StartLineDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

    public enum EndLineDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

    public enum NodeDirections
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum StreamDirections
    {
        TopIn,
        TopOut,
        BotIn,
        BotOut,
        ForwardIn,
        ForwardOut,
        BackwardIn,
        BackwardOut
    }

    public enum HitType
    { Object, ObjectHandle, Stream, StreamHandle, StreamConnection, DataNode, None }

    public enum HotSpotType
    {
        Feed,
        LiquidDraw,
        Stream,
        Water,
        Floating,
        VapourDraw,
        EnergyIn,
        EnergyOut,
        FeedRight,
        TrayNetLiquid,
        TrayNetVapour,
        BottomTrayLiquid,
        TopTrayVapour,
        LiquidDrawLeft,
        None,
        SignalIn,
        SignalOut,
        MaterialStream,
        Signal,
        InternalExport
    }

    public enum HotSpotOwnerType
    { DrawRectangle, Tray, DrawArea, ColDrawArea, Stream }
}