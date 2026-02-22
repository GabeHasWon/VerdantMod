using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Verdant.Items;
using Verdant.Systems.TearRain;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class TearPlant : ModTile
{
    public class TearPlantItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 28, 34, ModContent.TileType<TearPlant>());
    }

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Height = 3;
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
        TileObjectData.newTile.Origin = new Point16(0, 2);
        TileObjectData.newTile.WaterDeath = false;
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidBottom | AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
        TileObjectData.newTile.DrawYOffset = -2;
        TileObjectData.addTile(Type);

        TileID.Sets.CountsAsWaterSource[Type] = true;

        AddMapEntry(new Color(43, 143, 145));
    }

    public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
    {
        if (Main.rand.NextBool(10) && tile.TileFrameX is 0 or 18 && tile.TileFrameY == 18)
            TearRainRendering.CreateRain(i, j, 3);
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;
}