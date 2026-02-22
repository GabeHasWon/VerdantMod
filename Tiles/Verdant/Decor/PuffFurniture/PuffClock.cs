using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffClock : ModTile
{
    [Sacrifice(3)]
    public class PuffClockItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 24, 48, ModContent.TileType<PuffClock>());

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<LushLeaf>(), 4)
                .AddIngredient(ModContent.ItemType<PuffMaterial>(), 6)
                .AddRecipeGroup(RecipeGroupID.IronBar, 3)
                .AddIngredient(ItemID.Glass, 6)
                .Register();
        }
    }

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileID.Sets.HasOutlines[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
        TileObjectData.newTile.Height = 5;
        TileObjectData.newTile.Origin = new Point16(0, 4);
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16];
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(255, 112, 202), Language.GetText("ItemName.GrandfatherClock"));
        AdjTiles = [TileID.GrandfatherClocks];
    }

    public override bool RightClick(int x, int y)
    {
        TileHelper.PrintTime(Main.time);
        return true;
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (closer)
            Main.SceneMetrics.HasClock = true;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
}