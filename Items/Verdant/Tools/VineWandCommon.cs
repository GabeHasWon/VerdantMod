using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Syncing;

namespace Verdant.Items.Verdant.Tools;

internal class VineWandCommon
{
    public static ZipvineEntity BuildVine(int minDistance, ZipvineEntity lastVine, Vector2? position = null, bool fromNet = false)
    {
        if (lastVine is null)
        {
            var zipvine = ForegroundManager.AddItemDirect(new ZipvineEntity(position ?? Main.MouseWorld, -1, -1), true, true) as ZipvineEntity;

            if (Main.netMode != NetmodeID.SinglePlayer && !fromNet)
                new ZipvineModule(zipvine.position.X, zipvine.position.Y, null, (short)Main.myPlayer).Send();
            return zipvine;
        }
        else
        {
            Vector2 placePos = position ?? lastVine.position + lastVine.DirectionTo(Main.MouseWorld) * minDistance;
            var zipvine = ForegroundManager.AddItemDirect(new ZipvineEntity(placePos, lastVine.whoAmI, -1), true, true) as ZipvineEntity;
            lastVine.nextVine = zipvine;

            if (Main.netMode != NetmodeID.SinglePlayer && !fromNet)
                new ZipvineModule(zipvine.position.X, zipvine.position.Y, (short)ForegroundManager.PlayerLayerItems.IndexOf(lastVine), (short)Main.myPlayer).Send();
            return zipvine;
        }
    }
}
