using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Verdant.Systems.Foreground.Parallax;

public class ChlorovineEntity : ZipvineEntity
{
    public override Vector2 HoldOffset => new(-6, 14);
    public override float ClimbSpeed => 0.9f;
    public override byte VineLength => 16;
    public override int VineId => 1;

    public ChlorovineEntity() : base("Parallax/Chlorovine")
    {
    }

    public ChlorovineEntity(Vector2 position, long priorWho = -1, long nextWho = -1) : base(position, -1, -1, "Parallax/Chlorovine")
    {
        //DateTime.MinValue.Ticks
        whoAmI = (int)(DateTime.UtcNow.Ticks % int.MaxValue);

        if (priorWho != -1)
            priorVine = ForegroundManager.PlayerLayerItems.First(x => x is ZipvineEntity zip && zip.whoAmI == priorWho) as ZipvineEntity;

        if (nextWho != -1)
            nextVine = ForegroundManager.PlayerLayerItems.First(x => x is ZipvineEntity zip && zip.whoAmI == nextWho) as ZipvineEntity;
    }
}