using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

internal class LightbulbLeaves : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Grass, SoundID.Dig, new Color(255, 190, 112), true, false);

        Main.tileLighted[Type] = true;
        Main.tileBrick[Type] = true;
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        yield return new Item(ModContent.ItemType<LushLeaf>());
        yield return new Item(ModContent.ItemType<Lightbulb>());
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.44f, 0.17f, 0.28f);
}