using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Misc;
using Verdant.Tiles.TileEntities.Verdant;

namespace Verdant.Tiles.Verdant.Misc;

class HangingCrystal : ModTile
{
    private static Asset<Texture2D> _crystalTex;

    public override void Load() => _crystalTex = null;

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidBottom | AnchorType.SolidTile, 1, 0);
        TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMulti(this, 1, 2, DustID.Grass, SoundID.Grass, true, new Color(143, 21, 193));

        Main.tileLighted[Type] = true;

        _crystalTex = ModContent.Request<Texture2D>(Texture + "_Crystals");
    }

    public override void Unload() => _crystalTex = null;

    public override void PlaceInWorld(int i, int j, Item item) => ModContent.GetInstance<HangingCrystalTE>().Place(i, j);

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        ModContent.GetInstance<HangingCrystalTE>().Kill(i, j);
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<HangingCrystalItem>());
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        if (Main.tile[i, j].TileFrameY != 0)
            (r, g, b) = (0.1f, 0.5f, 0.2f);
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        if (Main.tile[i, j].TileFrameY != 18)
            return;
        
        var pos = TileHelper.TileCustomPosition(i, j, new Vector2(-8, -2));
        var source = new Rectangle(Main.tile[i, j].TileFrameX, 0, 16, 24);
        var scale = Vector2.One * (float)(Math.Pow(Math.Sin(Main.GlobalTimeWrappedHourly + (i + j) * 0.2f), 2) * 0.45f + 1f);
        spriteBatch.Draw(_crystalTex.Value, pos, source, Lighting.GetColor(i, j) * 0.45f, 0f, new Vector2(8, 12), scale, SpriteEffects.None, 0f);
    }

    public override void HitWire(int i, int j)
    {
        Tile tile = Main.tile[i, j];
        int topY = j - (tile.TileFrameY % 36 == 18 ? 1 : 0);
        short frameAdjustment = (short)(tile.TileFrameX < 54 ? 54 : -54);

        Main.tile[i, topY].TileFrameX += frameAdjustment;
        Main.tile[i, topY + 1].TileFrameX += frameAdjustment;

        Wiring.SkipWire(i, topY);
        Wiring.SkipWire(i, topY + 1);
        NetMessage.SendTileSquare(-1, i, topY + 1, 2, TileChangeType.None);
    }
}