using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Blocks.VerdantFurniture
{
    public class VerdantChandelierBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Verdant Chandelier (Small)");
        public override void SetDefaults() => QuickItem.SetBlock(this, 32, 42, ModContent.TileType<Tiles.Verdant.Decor.VerdantFurniture.VerdantChandelier>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<RedPetal>(), 6), (ModContent.ItemType<VerdantStrongVineMaterial>(), 4), (ModContent.ItemType<Lightbulb>(), 3));
    }
}