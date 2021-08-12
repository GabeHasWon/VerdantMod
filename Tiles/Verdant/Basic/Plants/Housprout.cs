using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    internal class Housprout : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>() };
            TileObjectData.newTile.RandomStyleRange = 1;
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
        }

        public override void RandomUpdate(int i, int j)
        {
            //do tomorrow
        }
    }
}