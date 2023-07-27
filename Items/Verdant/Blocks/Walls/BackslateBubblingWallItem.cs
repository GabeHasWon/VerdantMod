using Terraria.ModLoader;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls
{
    public class BackslateBubblingWallItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetWall(this, 32, 32, ModContent.WallType<BubblingWall>());
    }
}