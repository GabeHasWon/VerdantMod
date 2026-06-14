using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.Localization;
using Verdant.Items.Verdant;
using Verdant.Items;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffWorkbench : ModTile
{
    [Sacrifice(3)]
    public class PuffWorkbenchItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 30, 16, ModContent.TileType<PuffWorkbench>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, -1, 1, (ModContent.ItemType<LushLeaf>(), 4), (ModContent.ItemType<PuffMaterial>(), 6));
    }

    public override void SetStaticDefaults()
    {
        Main.tileSolidTop[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;
        
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
        TileObjectData.newTile.CoordinateHeights = [18];
        TileObjectData.addTile(Type);

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
        AddMapEntry(new Color(255, 112, 202), Language.GetText("ItemName.WorkBench"));

        TileID.Sets.DisableSmartCursor[Type] = true;
        AdjTiles = [TileID.WorkBenches];
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}