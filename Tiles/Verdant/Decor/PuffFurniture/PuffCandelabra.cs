using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

internal class PuffCandelabra : ModTile
{
    [Sacrifice(3)]
    public class PuffCandelabraItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<PuffCandelabra>());
        public override void AddRecipes() 
            => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<LushLeaf>(), 1), (ModContent.ItemType<PuffMaterial>(), 2), (ModContent.ItemType<Lightbulb>(), 1));
    }

    public override void SetStaticDefaults() => CandelabraHelper.Defaults<PuffCandelabraItem>(this, new Color(255, 112, 202), false);
    public override void HitWire(int i, int j) => CandelabraHelper.WireHit(i, j);

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        Vector3 light = new Vector3(0.5f, 0.16f, 0.30f) * 1.5f;
        if (Framing.GetTileSafely(i, j).TileFrameX is 0 or 18)
        {
            r = light.X;
            g = light.Y;
            b = light.Z;
        }
    }
}
