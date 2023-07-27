using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class PeaceTreetopItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 30, 36, ModContent.TileType<PeaceTreeTop>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<MysteriaWood>(), 20), (ModContent.ItemType<MysteriaClump>(), 16),
        (ItemID.PinkGel, 20));
}
