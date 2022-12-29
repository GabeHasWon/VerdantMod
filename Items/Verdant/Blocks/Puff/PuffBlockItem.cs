using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.Puff
{
    public class PuffBlockItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Block of Puff", "'Very soft!'");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, ModContent.TileType<PuffBlock>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.Loom, 1, (ModContent.ItemType<PuffMaterial>(), 1));
    }
}
