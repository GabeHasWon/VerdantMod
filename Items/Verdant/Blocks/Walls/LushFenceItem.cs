using Terraria.ModLoader;
using Verdant.Walls;

namespace Verdant.Items.Verdant.Blocks.Walls;

public class LushFenceItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetWall(this, 32, 30, ModContent.WallType<LushFence>());
}