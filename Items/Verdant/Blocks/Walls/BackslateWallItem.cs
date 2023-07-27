using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class BackslateWallItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<BackslateWall>());
    }
}