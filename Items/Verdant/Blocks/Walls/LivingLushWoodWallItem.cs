using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class LivingLushWoodWallItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Living Lush Wood Wall", "");
        public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<LivingLushWoodWall>());
        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, TileID.LivingLoom, 4, (ModContent.ItemType<VerdantWoodBlock>(), 1));
            QuickItem.AddRecipe(ModContent.ItemType<VerdantWoodBlock>(), TileID.LivingLoom, 1, (Item.type, 4));
        }
    }
}