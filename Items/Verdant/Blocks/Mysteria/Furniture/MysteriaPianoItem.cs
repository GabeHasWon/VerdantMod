using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

[Sacrifice(1)]
public class MysteriaPianoItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Piano", "");
    public override void SetDefaults() => QuickItem.SetBlock(this, 40, 30, ModContent.TileType<Tiles.Verdant.Decor.MysteriaFurniture.MysteriaPiano>());
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Sawmill, 1, (ModContent.ItemType<MysteriaWood>(), 15), (ItemID.Book, 1), (ItemID.Bone, 4));
}