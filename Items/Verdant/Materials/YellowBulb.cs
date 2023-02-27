using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Materials;

class YellowBulb : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Yellow Bulb", "'So rare...'");
    public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<YellowSprouts>());
    public override void Update(ref float gravity, ref float maxFallSpeed) => maxFallSpeed = 0.9f;
}