using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Aquamarine;

internal class GemsparkAquamarine : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Dirt, SoundID.Dig, new Color(56, 144, 170), ModContent.ItemType<Items.Verdant.Blocks.Aquamarine.AquamarineItem>(), "", true, false);

        Main.tileBrick[Type] = true;
        Main.tileLighted[Type] = true;
        TileID.Sets.GemsparkFramingTypes[Type] = Type;
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.498f, 1, 0.831f);
}