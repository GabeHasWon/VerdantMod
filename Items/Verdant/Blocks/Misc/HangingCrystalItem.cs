using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class HangingCrystalItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<HangingCrystal>(), rarity: ItemRarityID.Green);
	public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Anvils, 1, (ModContent.ItemType<GreenCrystalItem>(), 2), (ModContent.ItemType<VerdantStrongVineMaterial>(), 1));

	public override bool? UseItem(Player player)
	{
		Item.placeStyle = Main.rand.Next(3);
		return null;
	}
}
