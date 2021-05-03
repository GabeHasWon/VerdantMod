using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Tiles.Verdant.Basic
{
    internal class VerdantDecor1x1 : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileType<VerdantSoilGrass>(), TileType<LushSoil>() };
            TileObjectData.newTile.RandomStyleRange = 7;
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 1, 1, DustID.Grass, SoundID.Grass, false, new Color(161, 226, 99));
            Main.tileCut[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    }

    internal class VerdantDecor1x1NoCut : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileType<VerdantSoilGrass>(), TileType<LushSoil>() };
            TileObjectData.newTile.RandomStyleRange = 4;
            TileObjectData.newTile.StyleHorizontal = true;
            Main.tileCut[Type] = false;
            QuickTile.SetMulti(this, 1, 1, DustID.Stone, SoundID.Dig, false, new Color(161, 226, 99));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    }

    internal class VerdantDecor2x1 : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileType<VerdantSoilGrass>(), TileType<LushSoil>() };
            TileObjectData.newTile.RandomStyleRange = 6;
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 2, 1, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
        }
    }

    internal class VerdantDecor2x2 : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileType<VerdantSoilGrass>(), TileType<LushSoil>() };
            TileObjectData.newTile.RandomStyleRange = 6;
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
        }
    }

    internal class VerdantDecor1x2 : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileType<VerdantSoilGrass>(), TileType<LushSoil>() };
            TileObjectData.newTile.RandomStyleRange = 6;
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 1, 2, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
            Main.tileCut[Type] = true;
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    }

    internal class VerdantDecor1x3 : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileType<VerdantSoilGrass>(), TileType<LushSoil>() };
            TileObjectData.newTile.RandomStyleRange = 6;
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 1, 3, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
            Main.tileCut[Type] = true;
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    }
}