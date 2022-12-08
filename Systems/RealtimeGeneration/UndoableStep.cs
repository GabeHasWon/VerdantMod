using Terraria.DataStructures;

namespace Verdant.Systems.RealtimeGeneration;

internal class UndoableStep : RealtimeStep
{
    private readonly string _name = string.Empty;
    private readonly Point16 _size = Point16.Zero;

    public UndoableStep(Point16 pos, TileAction.TileActionDelegate action, Point16 size, string name) : base(pos, action)
    {
        _name = name;
        _size = size;
    }

    public override void Invoke(int x, int y, ref bool success)
    {
        base.Invoke(x, y, ref success);
    }
}
