using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

public class SmokeBulb : ModTile
{
    internal class SmokeBulbFlag : ModSystem
    {
        internal static bool NearSmokeBulb = false;

        public override void ResetNearbyTileEffects() => NearSmokeBulb = false;
    }

    public override void SetStaticDefaults()
    {
        TileID.Sets.HasOutlines[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<LushSoil>()];
        TileObjectData.newTile.ExpandValidAnchors([.. VerdantGrassLeaves.VerdantGrassTypes]);
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.DrawYOffset = 2;

        QuickTile.SetMulti(this, 2, 2, DustID.Smoke, SoundID.Grass, false, new Color(78, 86, 85));
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (Main.tile[i, j].TileFrameY == 36)
            SmokeBulbFlag.NearSmokeBulb = true;
    }

    public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
    {
        if (Main.tile[i, j].TileFrameY == 36)
            Dust.NewDustPerfect(new Vector2(i + Main.rand.NextFloat(), j) * 16, DustID.Smoke, new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), Main.rand.NextFloat(-3, -1))).noGravity = true;
    }

    public override bool RightClick(int i, int j)
    {
        Tile tile = Main.tile[i, j];
        i -= tile.TileFrameX % 36 / 18;
        j -= tile.TileFrameY % 36 / 18;

        for (int x = 0; x < 2; ++x)
        {
            for (int y = 0; y < 2; ++y)
            {
                tile = Main.tile[i + x, j + y];

                if (tile.TileFrameY < 36)
                    tile.TileFrameY += 36;
                else
                    tile.TileFrameY -= 36;
            }
        }

        return true;
    }
}