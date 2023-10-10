using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Aquamarine;

namespace Verdant.Tiles.Verdant.Decor.AquamarineDecor;

public class AquamarineTorch : ModTile
{
    private Asset<Texture2D> flameTexture;

    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileSolid[Type] = false;
        Main.tileNoAttach[Type] = true;
        Main.tileNoFail[Type] = true;
        Main.tileWaterDeath[Type] = true;

        TileID.Sets.FramesOnKillWall[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;
        TileID.Sets.Torch[Type] = true;

        DustType = DustID.Water;
        AdjTiles = new int[] { TileID.Torches };

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

        TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
        TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
        TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { TileID.WoodenBeam };
        TileObjectData.addAlternate(1);
        TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
        TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { TileID.WoodenBeam };
        TileObjectData.addAlternate(2);
        TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newAlternate.AnchorWall = true;
        TileObjectData.addAlternate(0);
        TileObjectData.addTile(Type);

        LocalizedText name = CreateMapEntryName();
        // name.SetDefault("Torch");
        AddMapEntry(new Color(138, 185, 200), name);

        if (!Main.dedServ)
            flameTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = Main.rand.Next(1, 3);

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameX < 66)
        {
            var light = new Vector3(0.315f, 0.676f, 2.147f);
            (r, g, b) = (light.X, light.Y, light.Z);
        }
    }

    public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
    {
        offsetY = 0;

        if (WorldGen.SolidTile(i, j - 1))
        {
            offsetY = 2;

            if (WorldGen.SolidTile(i - 1, j + 1) || WorldGen.SolidTile(i + 1, j + 1))
                offsetY = 4;
        }
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        int offsetY = 0;

        if (WorldGen.SolidTile(i, j - 1))
        {
            offsetY = 2;

            if (WorldGen.SolidTile(i - 1, j + 1) || WorldGen.SolidTile(i + 1, j + 1))
                offsetY = 4;
        }

        const int Width = 20;
        const int Height = 20;

        Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
        ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
        Color color = new(100, 100, 100, 0);
        var tile = Main.tile[i, j];
        int frameX = tile.TileFrameX;
        int frameY = tile.TileFrameY;

        for (int k = 0; k < 7; k++)
        {
            float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
            float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
            var pos = new Vector2(i * 16 - (int)Main.screenPosition.X - (Width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + offsetY + yy) + zero;

            spriteBatch.Draw(flameTexture.Value, pos, new Rectangle(frameX, frameY, Width, Height), color, 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}