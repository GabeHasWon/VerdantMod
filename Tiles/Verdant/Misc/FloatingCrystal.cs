using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Misc;

namespace Verdant.Tiles.Verdant.Misc;

internal class FloatingCrystal : ModTile
{
    public override void Load() => On.Terraria.Projectile.VanillaAI += Projectile_VanillaAI;

    /// <summary>
    /// Makes this tiles solid for only hooks
    /// </summary>
    private void Projectile_VanillaAI(On.Terraria.Projectile.orig_VanillaAI orig, Projectile self)
    {
        Main.tileSolid[ModContent.TileType<FloatingCrystal>()] = true;
        orig(self);
        Main.tileSolid[ModContent.TileType<FloatingCrystal>()] = false;
    }

    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.EmptyTile, 2, 0);
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(20, 145, 41), CreateMapEntryName());

        HitSound = SoundID.Shatter;
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.3f, 0.6f, 0.3f);
    public override void KillMultiTile(int i, int j, int x, int y) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<FloatingCrystalItem>());

    public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
    {
        int uniqueAnimationFrame = Main.tileFrame[Type] + i;

        if (i % 2 == 0)
            uniqueAnimationFrame += 3;
        if (i % 3 == 0)
            uniqueAnimationFrame += 3;
        if (i % 4 == 0)
            uniqueAnimationFrame += 3;

        uniqueAnimationFrame %= 4;
        frameYOffset = uniqueAnimationFrame * 36;
    }

    public override void AnimateTile(ref int frame, ref int frameCounter)
    {
        if (++frameCounter >= 5)
        {
            frameCounter = 0;
            frame = ++frame % 4;
        }
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        var tile = Main.tile[i, j];
        var position = TileHelper.TileCustomPosition(i, j, new Vector2(0, MathF.Sin(Main.GameUpdateCount * 0.05f) * 4).ToPoint().ToVector2());

        int t = 0;
        int frameY = 0;
        AnimateIndividualTile(Type, i, j, ref t, ref frameY);

        var source = new Rectangle(tile.TileFrameX, tile.TileFrameY + frameY, 16, 16);
        spriteBatch.Draw(TextureAssets.Tile[Type].Value, position, source, Lighting.GetColor(i, j));
        return false;
    }
}