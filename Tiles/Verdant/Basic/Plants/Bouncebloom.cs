using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using static Terraria.ModLoader.ModContent;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
    internal class Bouncebloom : ModTile
    {
        private int frameCounter = 0;

        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.AnchorValidTiles = new[] { TileID.Mud, TileType<VerdantSoilGrass>() };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(165, 108, 58));

            disableSmartCursor = true;
            dustType = DustID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ItemType<Items.Verdant.Tools.BouncebloomItem>());
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Framing.GetTileSafely(i, j);
            Texture2D tile = GetTexture("Verdant/Tiles/Verdant/Basic/Plants/Bouncebloom");
            Color col = Lighting.GetColor(i, j);

            int frameY = t.frameY;

            spriteBatch.Draw(tile, Helper.TileCustomPosition(i, j), new Rectangle(t.frameX, frameY, 16, 16), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) => this.frameCounter = ++frameCounter;

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile t = Framing.GetTileSafely(i, j);
            if (t.frameY >= 38 && frameCounter % 6 == 0)
            {
                t.frameY -= 38;
            }
        }
    }
}