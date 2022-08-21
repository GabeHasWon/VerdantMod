using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    class DyeBulbs : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>() };
            TileObjectData.newTile.RandomStyleRange = 1;
            TileObjectData.newTile.StyleHorizontal = true;

            QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(143, 21, 193));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (frameX == 0)
                Item.NewItem(new Vector2(i, j) * 16, new Vector2(32, 32), ModContent.ItemType<Items.Verdant.Blocks.PinkDyeBulb>(), 1);
            else
                Item.NewItem(new Vector2(i, j) * 16, new Vector2(32, 32), ModContent.ItemType<Items.Verdant.Blocks.RedDyeBulb>(), 1);
        }
    }
}