using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Dusts;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffSink : ModTile
{
    [Sacrifice(3)]
    public class PuffSinkItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 32, 34, ModContent.TileType<PuffSink>());
        public override void AddRecipes() 
            => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<LushLeaf>(), 2), (ModContent.ItemType<PuffMaterial>(), 2), (ItemID.WaterBucket, 1));
    }

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.newTile.Direction = Terraria.Enums.TileObjectDirection.PlaceLeft;
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.Direction = Terraria.Enums.TileObjectDirection.PlaceRight;
        TileObjectData.addAlternate(1);
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(255, 112, 202), Terraria.Localization.Language.GetText("MapObject.Sink"));
        RegisterItemDrop(ModContent.ItemType<PuffSinkItem>());

        DustType = ModContent.DustType<PuffDust>();
        AdjTiles = [TileID.Sinks];
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}