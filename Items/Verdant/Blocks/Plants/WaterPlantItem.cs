using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class WaterPlantItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 36, 28, ModContent.TileType<WaterPlant>());

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddCondition(Condition.NearWater)
                .AddIngredient<PinkPetal>(12)
                .AddTile(TileID.LivingLoom)
                .Register();
        }
    }
}
