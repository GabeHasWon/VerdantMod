using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture;

internal class VerdantCandelabra : ModTile
{
    public override void SetStaticDefaults() => CandelabraHelper.Defaults(this, new Color(253, 221, 3), false);
    public override void KillMultiTile(int i, int j, int frameX, int frameY) => 
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantCandelabraItem>());
    public override void HitWire(int i, int j) => CandelabraHelper.WireHit(i, j);

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        Vector3 light = new Vector3(0.5f, 0.16f, 0.30f) * 3f;
        if (Framing.GetTileSafely(i, j).TileFrameX == 0)
        {
            r = light.X;
            g = light.Y;
            b = light.Z;
        }
    }
}
