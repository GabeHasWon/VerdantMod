using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc;

public class ResearchBooksItem : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Book of Research", "'Pages and pages of scribbles and text and theories'");
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<ResearchBooks>());
}
