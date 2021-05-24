using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantChairItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Chair", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantChair>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, Terraria.ID.TileID.WorkBenches, 1, (ItemType<LushLeaf>(), 2), (ItemType<PinkPetal>(), 2));
    }
}