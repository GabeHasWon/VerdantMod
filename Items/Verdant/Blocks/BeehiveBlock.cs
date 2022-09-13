using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic;

namespace Verdant.Items.Verdant.Blocks
{
    public class BeehiveBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Bumblebee Hive", "The buzzing is mildly concerning");
        public override void SetDefaults() => QuickItem.SetBlock(this, 28, 28, ModContent.TileType<Beehive>());

        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 1, (ItemID.Hive, 4));
            QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 2, (ItemID.BeeWax, 3));
        }
    }
}
