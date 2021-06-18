using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantBathtubItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Bathtub", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantBathtub>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<LushLeaf>(), 10), (ItemType<RedPetal>(), 4));
    }
}