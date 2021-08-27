using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantSofaItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Sofa", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 48, 32, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantSofa>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ItemType<LushLeaf>(), 10), (ItemType<RedPetal>(), 4));
    }
}