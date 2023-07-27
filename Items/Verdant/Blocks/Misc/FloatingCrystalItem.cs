using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class FloatingCrystalItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<FloatingCrystal>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Anvils, 1, (ModContent.ItemType<GreenCrystalItem>(), 4), (ItemID.SoulofFlight, 1));
}
