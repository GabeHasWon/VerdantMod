using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Verdant.Items.Verdant;
using Verdant.Items;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffBookcase : ModTile
{
    [Sacrifice(1)]
    public class PuffBookcaseItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<PuffBookcase>());
        public override void AddRecipes() 
            => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<PuffMaterial>(), 8), (ModContent.ItemType<LushLeaf>(), 8), (ItemID.Book, 10));
    }

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        Main.tileSolidTop[Type] = true;

        TileID.Sets.DisableSmartCursor[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 18];
        TileObjectData.addTile(Type);

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
        AddMapEntry(new Color(124, 93, 68), Terraria.Localization.Language.GetText("ItemName.Bookcase"));
        RegisterItemDrop(ModContent.ItemType<PuffBookcaseItem>());

        DustType = DustID.Grass;
        AdjTiles = [TileID.Bookcases];
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}