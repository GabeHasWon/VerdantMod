using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.NPCs.Passive.Snails;

namespace Verdant.Tiles.Verdant.Decor.Terrariums;

public abstract class SnailTerrarium : ModTile
{
    public override string Texture => "Verdant/Tiles/Verdant/Decor/Terrariums/SnailTerrarium";
    protected abstract int NPCType { get; }
    protected abstract Point NPCSize { get; }

    protected abstract float GetOffset(int i, int j);

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileTable[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Width = 5;
        TileObjectData.newTile.Height = 3;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
        TileObjectData.newTile.Origin = new Point16(3, 1);
        TileObjectData.addTile(Type);

        DustType = DustID.Glass;

        AddMapEntry(new Color(22, 51, 81));
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX == 72 && tile.TileFrameY == 36)
        {
            float[] offsets =   [0f, 6, 12, 18, 18, 12,  6,  0, -6, -12, -18, -18, -12, -6, 0];
            float[] rotations = [0, 0f, 0f, 0,  1,  1,   1,  1,  1,  1,   1,   0,  0,    0, 0];

            Main.instance.LoadNPC(NPCType);
            Texture2D tex = TextureAssets.Npc[NPCType].Value;
            float offset = ((Main.GameUpdateCount + GetOffset(i, j)) * 0.02f) + ((i + j) * MathHelper.PiOver2);
            int index = (int)Math.Ceiling(offset) % offsets.Length;
            Vector2 off = new(MathHelper.Lerp(offsets[index], offsets[index == offsets.Length - 1 ? 0 : index + 1], offset % 1) + 28, 2);
            var src = new Rectangle(0, 0, NPCSize.X, NPCSize.Y);
            var effect = rotations[index] == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(tex, TileHelper.TileCustomPosition(i, j, new Vector2(MathF.Round(off.X), off.Y)), src, Lighting.GetColor(i, j), 0, new(11, 12), 1f, effect, 0);
        }
        return true;
    }
}

public class RedSnailTerrarium : SnailTerrarium
{
    protected override int NPCType => ModContent.NPCType<VerdantRedGrassSnail>();
    protected override Point NPCSize => new(30, 22);

    protected override float GetOffset(int i, int j) => (i + j) * MathHelper.PiOver2;
}

public class BulbSnailTerrarium : SnailTerrarium
{
    protected override int NPCType => ModContent.NPCType<VerdantBulbSnail>();
    protected override Point NPCSize => new(26, 16);

    protected override float GetOffset(int i, int j) => (i - j) * MathHelper.PiOver4;
}

public class ShellSnailTerrarium : SnailTerrarium
{
    protected override int NPCType => ModContent.NPCType<ShellSnail>();
    protected override Point NPCSize => new(38, 22);

    protected override float GetOffset(int i, int j) => (i - j) * MathHelper.PiOver4 + MathHelper.PiOver2;
}