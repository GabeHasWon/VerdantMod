using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.Items.Verdant.Blocks.Unobtainable;

public class ApotheosisPlacer : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 34, 24, ModContent.TileType<Apotheosis>(), maxStack: 1);
    public override void HoldItem(Player player) => Item.createTile = Main.hardMode ? ModContent.TileType<HardmodeApotheosis>() : ModContent.TileType<Apotheosis>();
}
