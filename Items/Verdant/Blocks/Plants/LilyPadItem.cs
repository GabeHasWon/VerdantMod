using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class LilyPadItem : ModItem
{
    public override void SetDefaults()
    {
        QuickItem.SetBlock(this, 22, 28, ModContent.TileType<Tiles.Verdant.Basic.Plants.LilyPad>());
        Item.value = Item.buyPrice(0, 0, 0, 0);
    }
}