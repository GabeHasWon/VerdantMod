using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantLanternItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Lantern", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantLantern>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<LushLeaf>(), 6), (ItemType<Lightbulb>(), 1));
    }
}