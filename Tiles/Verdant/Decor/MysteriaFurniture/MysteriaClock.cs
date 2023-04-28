using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.MysteriaFurniture;

public class MysteriaClock : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
        TileObjectData.newTile.Height = 5;
        TileObjectData.newTile.Origin = new Point16(0, 4);
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(124, 93, 68));

        AdjTiles = new int[] { TileID.GrandfatherClocks };
    }

    public override bool RightClick(int x, int y)
    {
        TileHelper.PrintTime(Main.time);

        bool london = LocationTracker.GetMachineRegion() == "United States";
        string text = Language.GetTextValue("Mods.Verdant.TileInteraction.MysteriaClock." + (london ? "London" : "NewYork"));
        TileHelper.PrintTime(Main.time + (86400 / 24 * 4.5), text);
        return true;
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (closer)
            Main.SceneMetrics.HasClock = true;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<Items.Verdant.Blocks.LushWood.LushClockItem>());
}