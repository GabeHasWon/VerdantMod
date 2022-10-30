using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Puff;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class PinkPuff : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Pink Puff", "'Deceptively light'");
        public override void SetDefaults() => QuickItem.SetBlock(this, 30, 42, ModContent.TileType<BigPuff>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 1, (ModContent.ItemType<PuffMaterial>(), 4), (ModContent.ItemType<LushLeaf>(), 3));
    }
}
