using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Mysteria;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class MysteriaAcorn : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Sapling", "Plants a Mysteria tree\nMysteria trees spread the Mysteria microbiome,\n" +
        " which has some unique content\nThe trees are solid");
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 24, ModContent.TileType<MysteriaSprout>());
}
