using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture;

internal class VerdantChandelier : ModTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom, 2, 0);
        TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.newTile.Origin = new Point16(0, 2);
        TileObjectData.newTile.WaterDeath = false;
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMultiLocalized(this, 2, 3, DustID.Grass, SoundID.Grass, false, new Color(253, 221, 3), false, false, false, Language.GetText("MapObject.Chandelier"), null);

        Main.tileFrameImportant[Type] = true;
        Main.tileCut[Type] = false;

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (Framing.GetTileSafely(i, j).TileFrameX == 0 && Framing.GetTileSafely(i, j).TileFrameY == 18)
        {
            Vector2 p = (new Vector2(i, j) * 16);
            Lighting.AddLight(p + new Vector2(16, 16), new Vector3(0.1f, 0.03f, 0.06f) * 14);
            Lighting.AddLight(p + new Vector2(10, 30), new Vector3(0.1f, 0.03f, 0.06f) * 14);
            Lighting.AddLight(p + new Vector2(26, 26), new Vector3(0.1f, 0.03f, 0.06f) * 14);
        }
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        if (Main.netMode != NetmodeID.Server)
        {
            for (int v = 0; v < 4; ++v)
            {
                Vector2 off = new(Main.rand.Next(32), Main.rand.Next(54));
                Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + off, new Vector2(0), Main.rand.NextBool(2) ? Mod.Find<ModGore>("LushLeaf").Type : Mod.Find<ModGore>("RedPetalFalling").Type, 1);
            }
        }
    }

    public override void HitWire(int i, int j)
    {
        Tile tile = Main.tile[i, j];

        int leftX = i - tile.TileFrameX / 18 % 2;
        int topY = j - tile.TileFrameY / 18 % 3;
        short frameAdjustment = (short)(tile.TileFrameX < 36 ? 36 : -36);

        for (int k = 0; k < 2; ++k)
        {
            for (int b = 0; b < 3; ++b)
            {
                Main.tile[leftX + k, topY + b].TileFrameX += frameAdjustment;
                Wiring.SkipWire(leftX + k, topY + b);
            }
        }

        NetMessage.SendTileSquare(-1, leftX, topY + 1, 3, TileChangeType.None);
    }
}