using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Dusts;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

internal class PuffTable : ModTile
{
    [Sacrifice(1)]
    public class PuffTableItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 28, ModContent.TileType<PuffTable>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<LushLeaf>(), 3), (ModContent.ItemType<PuffMaterial>(), 5));
    }

    public override void SetStaticDefaults()
    {
        Main.tileSolidTop[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.addTile(Type);

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
        AddMapEntry(new Color(255, 112, 202), Language.GetText("MapObject.Table"));

        TileID.Sets.DisableSmartCursor[Type] = true;

        AdjTiles = [TileID.Tables];
        DustType = ModContent.DustType<PuffDust>();
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}