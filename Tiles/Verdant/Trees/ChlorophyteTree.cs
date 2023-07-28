using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Misc;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Trees;

internal class ChlorophyteTree : ModTile
{
    private static Asset<Texture2D> _leaves;
    private static Asset<Texture2D> _tops;

    public override void Unload() => _leaves = _tops = null;

    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = false;
        Main.tileBlockLight[Type] = false;
        Main.tileFrameImportant[Type] = true;
        Main.tileAxe[Type] = true;

        TileID.Sets.IsATreeTrunk[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom | AnchorType.AlternateTile, 1, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Mud, TileID.Grass, TileID.JungleGrass, TileID.MushroomGrass, TileID.HallowedGrass, TileID.Dirt, TileID.Chlorophyte,
            ModContent.TileType<LushSoil>(), ModContent.TileType<VerdantGrassLeaves>() };
        TileObjectData.newTile.AnchorAlternateTiles = new int[] { Type };
        TileObjectData.addTile(Type);

        DustType = DustID.Chlorophyte;
        HitSound = SoundID.Shatter;

        AddMapEntry(new Color(36, 97, 51));
        _leaves = ModContent.Request<Texture2D>(Texture + "Leaves");
        _tops = ModContent.Request<Texture2D>(Texture + "Tops");
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        bool validAbove = WorldGen.SolidTile(i, j - 1) || TileHelper.ActiveType(i, j - 1, Type);
        bool validBelow = WorldGen.SolidTile(i, j + 1) || TileHelper.ActiveType(i, j + 1, Type);

        if (!validAbove && validBelow)
            Framing.GetTileSafely(i, j).TileFrameX = 36;
        else
            Framing.GetTileSafely(i, j).TileFrameX = (short)(Main.rand.NextBool(4) ? 18 : 0);
        Framing.GetTileSafely(i, j).TileFrameY = (short)(Main.rand.Next(2) * 18);

        if (WorldGen.SolidTile(i, j + 1))
        {
            if (TileHelper.ActiveType(i, j - 1, Type))
                Framing.GetTileSafely(i, j).TileFrameX = (short)(Main.rand.NextBool(2) ? 18 : 0);
            else
                Framing.GetTileSafely(i, j).TileFrameX = 36;
            Framing.GetTileSafely(i, j).TileFrameY = 36;
        }

        return false;
    }

    public override void RandomUpdate(int i, int j)
    {
        if (!Main.tile[i, j - 1].HasTile && Main.rand.NextBool(20))
            TileHelper.SyncedPlace(i, j - 1, Type, true);
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        yield return new Item(ItemID.ChlorophyteOre) { stack = Main.rand.Next(1, 3) };

        if (Main.tile[i, j].TileFrameX == 18 && Main.tile[i, j].TileFrameY <= 18)
            yield return new Item(ModContent.ItemType<ChlorophytePlant>());
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail || effectOnly)
            return;

        if (TileHelper.ActiveType(i, j - 1, Type))
            WorldGen.KillTile(i, j - 1, false, false, false);
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX == 18 && tile.TileFrameY <= 18)
        {
            Rectangle source = new(0, tile.TileFrameY, 24, 16);
            spriteBatch.Draw(_leaves.Value, TileHelper.TileCustomPosition(i, j, source.Size() / -2f + new Vector2(4, 0)), source, Lighting.GetColor(i, j), 0f, source.Size() / 2f, 1f, SpriteEffects.None, 0);
        }

        if (tile.TileFrameX == 36 && tile.TileFrameY <= 18)
        {
            Rectangle source = new(0, tile.TileFrameY / 18 * 38, 62, 34);
            Vector2 halfSize = source.Size() / -2f;
            TileSwaySystem.DrawTreeSway(i, j, _tops.Value, source, halfSize + new Vector2(8 - halfSize.X, 0), -halfSize);
        }
    }
}