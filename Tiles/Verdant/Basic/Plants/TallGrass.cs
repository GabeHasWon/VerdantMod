using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    internal class TallGrass : ModTile
    {
        public override bool IsLoadingEnabled(Mod mod) => false;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileCut[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            ItemDrop = 0;
            AddMapEntry(new Color(24, 135, 28));
            DustType = DustID.Grass;
            HitSound = SoundID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile t = Framing.GetTileSafely(i, j);
            if (TileHelper.ActiveType(i, j + 1, Type) && Framing.GetTileSafely(i, j + 1).TileFrameY > 0)
            {
                t.TileFrameX = Framing.GetTileSafely(i, j + 1).TileFrameX;
                t.TileFrameY = (short)(Framing.GetTileSafely(i, j + 1).TileFrameY - 18);
            }
            return false;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j - 1].HasTile && Main.rand.NextBool(10))
                WorldGen.PlaceTile(i, j - 1, Type, true, false);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (TileHelper.ActiveType(i, j - 1, Type))
                WorldGen.KillTile(i, j - 1, false, false, true);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!Main.tile[i, j + 1].HasTile) WorldGen.KillTile(i, j);
        }
    }
}