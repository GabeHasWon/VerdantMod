using Terraria.DataStructures;

namespace Verdant.Systems.RealtimeGeneration;

public class RealtimeStep
{
    public readonly Point16 Position;
    public readonly TileAction.TileActionDelegate Action;

    public RealtimeStep(Point16 pos, TileAction.TileActionDelegate action)
    {
        Position = pos;
        Action = action;
    }

    public virtual void Invoke(int x, int y, ref bool success) => Action.Invoke(x, y, ref success); 
}
