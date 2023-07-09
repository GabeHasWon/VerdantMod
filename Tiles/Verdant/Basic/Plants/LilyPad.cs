using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class LilyPad : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolidTop[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.Height = 1;
        TileObjectData.newTile.CoordinateHeights = new[] { 16 };
        TileObjectData.newTile.Origin = new Point16(1, 0);
        TileObjectData.newTile.AnchorAlternateTiles = new[] { ModContent.TileType<VerdantLillie>() };
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.AlternateTile, 1, 1);
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.OnlyInLiquid;
        TileObjectData.addTile(Type);
        
        AddMapEntry(new Color(107, 204, 87));

        DustType = DustID.Grass;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    public override void KillMultiTile(int i, int j, int frameX, int frameY) => 
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Verdant.Blocks.Plants.LilyPadItem>());

    public override void RandomUpdate(int i, int j)
    {
        if (Main.tile[i, j].TileFrameX == 18 && Main.rand.NextBool(3) && !Main.tile[i, j - 1].HasTile)
        {
            WorldGen.PlaceTile(i, j - 1, ModContent.TileType<LilyPadFlower>(), true, style: Main.rand.Next(3));

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
        }
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Texture2D tile = TextureAssets.Tile[Type].Value;
        Tile t = Framing.GetTileSafely(i, j);
        int x = i - (t.TileFrameX / 18);
        var pos = TileHelper.TileCustomPosition(i, j, new Vector2(VerdantLillie.SineOffset(x, j), 0));

        spriteBatch.Draw(tile, pos, new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Lighting.GetColor(i, j), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        return false;
    }
}