using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.LushWood;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture
{
    public class LushBookshelf : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileSolidTop[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 18 };
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            AddMapEntry(new Color(114, 69, 39), Terraria.Localization.Language.GetText("ItemName.Bookcase"));
            RegisterItemDrop(ModContent.ItemType<LushBookshelfItem>());

            DustType = DustID.Grass;
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Bookcases };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}