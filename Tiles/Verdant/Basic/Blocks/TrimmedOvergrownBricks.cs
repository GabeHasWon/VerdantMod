using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Misc;

namespace Verdant.Tiles.Verdant.Basic.Blocks;

internal class TrimmedOvergrownBricks : ModTile
{
    public override string Texture => base.Texture.Replace("Trimmed", "");

    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Grass, SoundID.Grass, new Color(90, 120, 90), true, false);
        RegisterItemDrop(ModContent.ItemType<OvergrownBrickItem>());

        Main.tileBlendAll[Type] = true;
        Main.tileBrick[Type] = true;
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail)
        {
            Tile tile = Main.tile[i, j];
            tile.TileType = TileID.GrayBrick;
        }
    }
}