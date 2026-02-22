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

public class PuffPlatform : ModTile
{
    [Sacrifice(200)]
    public class PuffPlatformItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 10, ModContent.TileType<PuffPlatform>());

        public override void AddRecipes()
        {
            QuickItem.AddRecipe(this, -1, 2, (ModContent.ItemType<PuffMaterial>(), 1));
            QuickItem.AddRecipe(ModContent.ItemType<PuffMaterial>(), -1, 1, (Type, 2));
        }
    }

    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileSolid[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileID.Sets.Platforms[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        TileObjectData.newTile.CoordinateHeights = [16];
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.StyleMultiplier = 27;
        TileObjectData.newTile.StyleWrapLimit = 27;
        TileObjectData.newTile.UsesCustomCanPlace = false;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.addTile(Type);

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
        AddMapEntry(new Color(255, 112, 202));

        DustType = ModContent.DustType<PuffDust>();
        AdjTiles = [TileID.Platforms];
    }

    public override void PostSetDefaults() => Main.tileNoSunLight[Type] = false;
    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}