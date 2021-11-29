using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantChandelierBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Chandelier (Small)");
        public override void SetDefaults() => QuickItem.SetBlock(this, 32, 42, TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantChandelier>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ItemType<LushLeaf>(), 6), (ItemType<RedPetal>(), 6), (ItemType<VerdantStrongVineMaterial>(), 4), (ItemType<Lightbulb>(), 3));
    }
}