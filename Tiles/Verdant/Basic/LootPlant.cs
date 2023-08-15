using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Misc;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic;

class LootPlant : ModTile
{
    public const int FrameHeight = 38;

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<VerdantRedPetal>(), ModContent.TileType<VerdantPinkPetal>(), 
            ModContent.TileType<LushSoil>(), TileID.HallowedGrass, TileID.Grass, TileID.JungleGrass, TileID.Hive };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassList());
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMulti(this, 2, 2, DustID.OrangeStainedGlass, SoundID.Grass, true, new Color(232, 167, 74), false, false, false, "Passionflower");
    }

    public override bool CanKillTile(int i, int j, ref bool blockDamaged) => Main.tile[i, j].TileFrameY >= FrameHeight;
    public override bool IsTileSpelunkable(int i, int j) => true;

    public override void RandomUpdate(int i, int j)
    {
        if (Main.rand.NextBool(1500))
            DecreaseFrame(new Point(i, j));
    }

    public override bool RightClick(int i, int j)
    {
        IncreaseFrame(new Point(i, j));
        return true;
    }

    internal static void IncreaseFrame(Point tL)
    {
        if (Main.tile[tL.X, tL.Y].TileFrameY >= FrameHeight)
            return;

        tL = TileHelper.GetTopLeft(tL);

        int item = Helper.SyncItem(new EntitySource_TileInteraction(Main.LocalPlayer, tL.X, tL.Y), tL.ToWorldCoordinates() + new Vector2(9), ModContent.ItemType<PassionflowerBulb>());
        Main.item[item].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), -Main.rand.NextFloat(6.6f, 8f)).RotatedByRandom(MathHelper.PiOver4 / 2);
        Main.item[item].noGrabDelay = 100;

        for (int i = 0; i < 10; ++i)
            Dust.NewDustPerfect(tL.ToWorldCoordinates() + new Vector2(18), DustID.OrangeStainedGlass, new Vector2(Main.rand.NextFloat(-1f, 1f), -Main.rand.NextFloat(4.2f, 7f)).RotatedByRandom(MathHelper.PiOver4), Main.rand.Next(0, 120));

        for (int i = tL.X; i < tL.X + 2; ++i)
        {
            for (int j = tL.Y; j < tL.Y + 2; ++j)
            {
                Tile t = Main.tile[i, j];
                t.TileFrameY += FrameHeight;
            }
        }
    }

    internal static void DecreaseFrame(Point tL)
    {
        if (Main.tile[tL.X, tL.Y].TileFrameY < FrameHeight)
            return;

        tL = TileHelper.GetTopLeft(tL);

        for (int i = tL.X; i < tL.X + 2; ++i)
        {
            for (int j = tL.Y; j < tL.Y + 2; ++j)
            {
                Tile t = Main.tile[i, j];
                t.TileFrameY -= FrameHeight;
            }
        }
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;

        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ModContent.ItemType<PassionflowerBulb>();
    }
}