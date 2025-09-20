using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture;

public class LushLoom : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.Height = 2;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
        TileObjectData.addTile(Type);

        AdjTiles = new int[] { TileID.LivingLoom, TileID.Loom };

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
        AddMapEntry(new Color(114, 69, 39), Language.GetText("ItemName.Loom"));
        RegisterItemDrop(ModContent.ItemType<LushLoomItem>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}