using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class Bouncebloom : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolidTop[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
        TileObjectData.newTile.Origin = new Point16(1, 1);
        TileObjectData.newTile.AnchorValidTiles = new[] { TileID.Mud, ModContent.TileType<LushSoil>(), ModContent.TileType<VerdantStrongVine>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.AnchorAlternateTiles = new[] { ModContent.TileType<VerdantStrongVine>() };
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, 1, 1);
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(165, 108, 58));

        TileID.Sets.DisableSmartCursor[Type] = true;
        DustType = DustID.Grass;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile t = Framing.GetTileSafely(i, j);
        Texture2D tile = ModContent.Request<Texture2D>("Verdant/Tiles/Verdant/Basic/Plants/Bouncebloom").Value;
        Color col = Lighting.GetColor(i, j);

        int frameY = t.TileFrameY;

        spriteBatch.Draw(tile, TileHelper.TileCustomPosition(i, j), new Rectangle(t.TileFrameX, frameY, 16, 16), new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        return false;
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(24, 16) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}