using Microsoft.Xna.Framework;
using System.Linq;
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
        public override bool IsLoadingEnabled(Mod mod) => false;

        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
            TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
            TileObjectData.newTile.RandomStyleRange = 1;
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(161, 226, 99));
        }
    }
}