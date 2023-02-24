using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class HangingCrystalItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Hanging Crystal", "Spawns healing plants when placed\nCan be turned on/off with wiring\n'Glows with a gentle power'");
    public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<HangingCrystal>(), rarity: ItemRarityID.Green);

	public override bool? UseItem(Player player)
	{
		Item.placeStyle = Main.rand.Next(3);
		return null;
	}

	public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.Anvils, 1, (ModContent.ItemType<GreenCrystalItem>(), 2), (ModContent.ItemType<VerdantStrongVineMaterial>(), 1));
}
