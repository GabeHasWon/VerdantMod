using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class PlatinumLily : ModTile, IFlowerTile
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
        TileObjectData.newTile.AnchorValidTiles = new[] { TileID.Dirt, TileID.Grass, TileID.Mud, ModContent.TileType<LushSoil>(), ModContent.TileType<VerdantStrongVine>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassList());
        TileObjectData.newTile.AnchorAlternateTiles = new[] { ModContent.TileType<VerdantStrongVine>() };
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, 1, 1);
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(165, 108, 58));

        TileID.Sets.DisableSmartCursor[Type] = true;
        DustType = DustID.Grass;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override void RandomUpdate(int i, int j)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileFrameY == 18 || !Main.rand.NextBool(10))
            return;

        var vel = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-14, -10));
        int proj = Projectile.NewProjectile(new EntitySource_TileUpdate(i, j), new Vector2(i, j) * 16, vel, ProjectileID.SilverCoinsFalling, 0, 0, Main.myPlayer);

        if (Main.netMode != NetmodeID.SinglePlayer)
            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Verdant.Tools.BouncebloomItem>());

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        return true;
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(24, 16) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}